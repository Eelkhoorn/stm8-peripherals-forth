\ Code from https://lujji.github.io/blog/bare-metal-programming-stm8/
\ slightly adapted and ported to stm8ef

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

: i2c-init ( -- ) 
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


: i2c-start ( --)
   1 I2C_CR2 0 B!
   BEGIN I2C_SR1 C@ 1 AND UNTIL
;

: i2c-stop ( --)
   1 I2C_CR2 1 B!
   BEGIN I2C_SR3 C@ 1 AND NOT UNTIL
;

: i2c-w ( b --)   \ Send one byte
   I2C_DR C!
   BEGIN I2C_SR1 C@ $80 AND UNTIL
;

: i2c-a ( b f --)   \ Send address write (f=0) or read (f=1) mode
   SWAP 2* + I2C_DR C!
   BEGIN I2C_SR1 C@ 2 AND UNTIL
   I2C_SR3 C@ DROP 1 I2C_CR2 2 B!
;

: i2c-wb ( b reg-adr i2c-adr)   \ write one byte to register at i2c
   i2c-start 0 i2c-a i2c-w i2c-w i2c-stop
;

\ write n bytes from buffer to reg at i2c:
: i2c-wbf ( buf-adr n reg-adr i2c-adr --) 
   i2c-start 0 i2c-a i2c-w 
   0 do dup i + c@ i2c-w loop i2c-stop drop
;

: i2c-r ( -- b)   \ receive last byte
   BEGIN I2C_SR1 C@ $40 AND UNTIL  \ RXNE, data register not empty
   I2C_DR C@
;

: i2c-r2 ( -- b)   \ receive second last byte
   0 I2C_CR2 2 B!
   i2c-stop
   i2c-r
;   

: i2c-rbf ( buf-adr n -- )   \ read two or more bytes into buffer
    ( ba n) 2- 0 DO  \ 1 I2C_CR2 2 B!
           BEGIN I2C_SR1 C@ 4 AND UNTIL    \ Byte Transfer Finished
             DUP I2C_DR c@ SWAP C! 1+
        LOOP ( n-1+ba)
   DUP i2c-r2 SWAP C!  1+ i2c-r SWAP C!
;

: i2c-sr ( reg-adr i2c-adr -- )             \ set register for read
   DUP >R i2c-start 0 i2c-a i2c-w i2c-stop
   i2c-start R> 1 i2c-a
;

: i2c-rb ( reg-adr i2c-adr -- b)            \ read single byte
   i2c-sr i2c-r2
;

: i2c-lbf ( buf-adr n reg-adr i2c-adr --)   \ load buffer
   i2c-sr i2c-rbf 
;

variable bf 10 allot

regs   \ clean up


