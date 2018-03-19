\  I2C
\ Master receiver and master transmitter
\ Standard speed 100 kHz

RAM
: _ ;
#require MARKER

MARKER regs

\res MCU: STM8S103
\res export I2C_CR1
\res export I2C_CR2
\res export I2C_FREQR
\res export I2C_OARL
\res export I2C_OARH
\res export I2C_CCRL
\res export I2C_CCRH
\res export I2C_TRISER
\res export I2C_DR
\res export I2C_SR1
\res export I2C_SR2
\res export I2C_SR3


NVM

: i2i ( -- ) \ initialise
   0 I2C_CR1 0 B!     \ Periferal disable
   1 I2C_FREQR 4 B!   \ CPU freq 16 MHz
   $A0 I2C_OARL C!    \ own address 0xA0
   0 I2C_CCRH 6 B!    \ duty cycle
   1 I2C_OARH 6 B!    \ mandatory
   $50 I2C_CCRL C!    \ i2c freq 100 kHz, CCR = f.master/(2 f.i2c)
   $11 I2C_TRISER C!  \ TRISER = CPU freq in MHz + 1
   1 I2C_CR1 0 B!     \ Periferal enable
   1 I2C_CR2 2 B!     \ acknoledge enable
;

: i2s ( --)   \ start
   1 I2C_CR2 0 B!
   BEGIN I2C_SR1 C@ 1 AND UNTIL
;

: i2p ( --)   \ stop
   1 I2C_CR2 1 B!
   BEGIN I2C_SR3 C@ 1 AND NOT UNTIL
;

: i2w ( b --)   \ Send one byte
   I2C_DR C!
   BEGIN I2C_SR1 C@ $80 AND UNTIL
;

: i2a ( b f --)   \ Send address write (f=0) or read (f=1) mode
   SWAP 2* + I2C_DR C!
   BEGIN I2C_SR1 C@ 2 AND UNTIL
   I2C_SR3 C@ DROP 1 I2C_CR2 2 B!
;

: i2b ( b reg-adr i2c-adr)   \ write one byte to register at i2c
   i2s 0 i2a i2w i2w i2p
;

\ write n bytes from buffer to reg at i2c:
: i2wf ( buf-adr n reg-adr i2c-adr --) 
   i2s 0 i2a i2w 
   0 do dup i + c@ i2w loop i2p drop
;

: DR>BUF ( buf-adr -- buf-adr+1)  \ receive byte, increment pointer
     I2C_DR C@ OVER C! 1+ 
;
 
: CLA ( --)	                       \ Clear ADDR
   I2C_SR1 C@ I2C_SR3 C@ 2DROP
;

: SR1 ( b --)
   >R BEGIN I2C_SR1 C@ R@ AND UNTIL R> DROP
;

: RXNE ( --)                \ RXNE, data register not empty
   $40 SR1  
;

: BTF                       \ RXNE ADN BTF, byte transfer finished
   $44 SR1
;

: i2cf ( buf-adr n -- )     \ receive two or more bytes and store in buffer
   DUP 2 = IF
     DROP
     1 I2C_CR2 3 B!          \ set POS
     CLA 0 I2C_CR2 2 B!      \ reset ACK
     BTF
     1 I2C_CR2 1 B!          \ set STOP
     DR>BUf BTF DR>BUf
     ELSE
     CLA 
     BEGIN
     DUP 3 = IF
       DROP RXNE
       DR>BUF RXNE
       0 I2C_CR2 2 B!        \ reset ACK
	   DR>BUf
	   1 I2C_CR2 1 B!        \ set STOP
	   BTF DR>BUf 0
	   ELSE
	   RXNE
	   SWAP DR>BUF SWAP 1-
	 THEN DUP 0= UNTIL DROP
   THEN DROP
;

: i2cf1 ( -- b)             \ receive single byte
   0 I2C_CR2 2 B!            \ reset ACK
   CLA 1 I2C_CR2 1 B!	     \ set STOP
   RXNE I2C_DR C@
;

: i2sr ( reg-adr i2c-adr -- )             \ set register for read
   DUP >R i2s 0 i2a i2w i2p
   i2s R> 1 i2a
;

: i2rb ( reg-adr i2c-adr -- b)            \ read single byte from reg. at slave
   i2sr i2cf1
;

: i2rf ( buf-adr n reg-adr i2c-adr --)   \ load buffer from reg. at slave
   i2sr i2cf 
;

regs   \ clean up


