\ Forth i2c words to be used with stm8ef (https://github.com/TG9541/stm8ef).

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
   $50 I2C_CCRL C!    \ i2c freq 100 kHz
   $11 I2C_TRISER C!  \ TRISER = CPU freq in MHz + 1
   1 I2C_CR1 0 B!     \ Periferal enable
   1 I2C_CR2 2 B!     \ acknoledge enable
;

\ Check event:
: i2c-ce ( u -- f)  
   dup >R $0004 = if I2C_SR2 C@ $04 AND  ( last_event) 
                      \ Slave acknowledge failure
   else
         I2C_SR1 C@   ( ?)          \ StatusRegister1
         I2C_SR3 C@   ( ? ?)        \ StatusRegister3
         $100 * or  
   then  ( last_event)
   R@ and R> = if -1 else 0 then
;

: i2c-start ( --)
   1 I2C_CR2 0 B!
;

: i2c-stop ( ? --) 
   if begin $0684 i2c-ce until   \ slave_byte_transmitted
   else begin $0784 i2c-ce until \ master_byte_transmitted
   1 I2C_CR2 1 B!
;

\ write mode send address:
: i2c-wsa ( adr --) 
   begin $0301 i2c-ce until      \ master_mode_select
   2* I2C_DR C!                  \ push left shifted addr to DR
   begin $0302 i2c-ce until ;    \ master_receiver_mode_selected

\ send byte:
: i2c-sb ( c --) 
   begin $0780 i2c-ce            \ master_byte_transmitting
   until I2C_DR c!
;	         

: i2c-w ( b reg-adr i2c-adr) 
   i2c-start i2c-wsa i2c-sb i2c-sb 0 i2c-stop ;

\ write n bytes:
: i2c-wsn ( pointer n reg-adr i2c-adr --) 
   i2c-start i2c-wsa i2c-sb 
   0 do dup i + c@ i2c-sb loop 0 i2c-stop drop
;

regs