\ I2C
\ Master receiver and master transmitter
\ Standard speed 100 kHz
\ Uncomment one \res MCU: line

RAM
: _ ;
#require MARKER
#require ]B!
#require ]C!

MARKER regs

\ \res MCU: STM8S103
\res MCU: STM8L052
\res export PB_DDR PB_CR1
\res export I2C_CR1 I2C_CR2 I2C_FREQR I2C_OARL I2C_OARH
\res export I2C_CCRL I2C_CCRH I2C_TRISER I2C_DR
\res export I2C_SR1 I2C_SR2 I2C_SR3
\res export CLK_PCKENR1

NVM

VARIABLE SAA   \ Slave address aknowledged

\ Delay, can't get address acknowledge failure switch right without this
: dl 0 DO I DROP LOOP ;  

: i2i ( -- ) \ initialise
   [ 0 I2C_CR1 0 ]B!     \ Periferal disable
   [ 1 CLK_PCKENR1 3 ]B! \ enable SYSCLK to I2C, needed for stm8l052
   [ $80 I2C_CR2 ]C!     \ reset BSY
   [ 0 I2C_CR2 ]C!       
   [ 1 I2C_FREQR 4 ]B!   \ CPU freq 16 MHz
   [ $A0 I2C_OARL ]C!    \ own address 0xA0
   [ $40 I2C_OARH ]C!    \ 7 bit address mode
   [ 0 I2C_CCRH 6 ]B!    \ duty cycle  
   [ $50 I2C_CCRL ]C!    \ i2c freq 100 kHz, CCR = f.master/(2 f.i2c)
   [ $11 I2C_TRISER ]C!  \ TRISER = CPU freq in MHz + 1
   [ 1 I2C_CR1 0 ]B!     \ Periferal enable
   [ 1 I2C_CR2 2 ]B!     \ acknoledge enable
;

: sr1 ( b --)   \ wait for sr1 flag
   BEGIN DUP I2C_SR1 C@ AND UNTIL DROP
;

: cla ( --)	                       \ Clear ADDR
   I2C_SR1 C@ I2C_SR3 C@ 2DROP
;

: i2s ( --)   \ start
   [ 1 I2C_CR2 0 ]B!
\   1 sr1
;

: i2p ( --)   \ stop
   [ 1 I2C_CR2 1 ]B!
   BEGIN I2C_SR3 C@ 1 AND NOT UNTIL
;

: i2sb ( b --)   \ Send one byte
   I2C_DR C!
   $80 sr1
;


: i2sf  ( buf-adr n --)   \ send n bytes
   0 DO DUP i + C@ i2sb LOOP DROP
;

: i2a ( b f --)   \ Send address write (f=0) or read (f=1) mode
   1 sr1    \ SB cleared by mcu after writing DR
   SWAP 2* + I2C_DR C!
   100 dl I2C_SR2 C@ 4 AND
   IF
     CR ." Slave address not acknowledged"
     [ 0 I2C_SR2 2 ]B!  \ reset AF
     [ 0 SAA ]C!
     i2p
     ELSE
     [ 1 SAA ]C!
     2 sr1
     cla [ 1 I2C_CR2 2 ]B!  \ clear ADDR, enable acknowledge
   THEN
;

: i2wb ( b reg-adr sl-adr)   \ write one byte to register at slave
   i2s 0 i2a i2sb i2sb i2p
;

\ write n bytes from buffer to reg at i2c:
: i2wf ( buf-adr n reg-adr sl-adr --)
   i2s 0 i2a i2sb i2sf i2p
;

: dr>bf ( buf-adr -- buf-adr+1)  \ receive byte, increment pointer
     I2C_DR C@ OVER C! 1+ 
;

: i2cb ( -- b)             \ receive single byte
   [ 0 I2C_CR2 2 ]B!            \ reset ACK
   [ cla 1 I2C_CR2 1 ]B!	     \ set STOP
   $40 sr1 I2C_DR C@
;

\ receive two or more bytes and store in buffer
: i2cf ( buf-adr n -- )
   DUP 2 = IF
     DROP
     [ 1 I2C_CR2 3 ]B!          \ set POS
     [ cla 0 I2C_CR2 2 ]B!      \ reset ACK
     $44 sr1
     [ 1 I2C_CR2 1 ]B!          \ set STOP
     dr>bf $44 sr1 dr>bf
     ELSE
     cla
     BEGIN
     DUP 3 = IF
       DROP $40 sr1
       dr>bf $40 sr1
       [ 0 I2C_CR2 2 ]B!        \ reset ACK
	   dr>bf
	   [ 1 I2C_CR2 1 ]B!        \ set STOP
	   $44 sr1 dr>bf 0
	   ELSE
	   $40 sr1
	   SWAP dr>bf SWAP 1-
	 THEN DUP 0= UNTIL DROP
   THEN DROP
;

: i2rr ( reg-adr sl-adr -- )             \ set register for read
   DUP i2s 0 i2a SWAP i2sb i2p
   i2s 1 i2a
;

: i2rb ( reg-adr sl-adr -- b)            \ read single byte from reg. at slave
   i2rr i2cb
;

\ load buffer with n bytes from reg. at slave
: i2rf ( buf-adr n reg-adr sl-adr --)
   i2rr i2cf 
;

\ Scan I2C addresses
: scan &128 0 do i dup 0 i2s i2a saa c@ if cr . i2p then loop ;

\ Display I2C registers
: drg I2C_CR1 $a 0 do cr dup dup . c@ space . 1+ loop drop ;


regs   \ clean up
