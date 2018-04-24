\ Source Mecrisp-Stellaris io.fs, adapted for STM8S
\ GPIO manipulation.
\ Portpin naming: $port#pin#  e.g. PC3 => $23, PA0 => $00
\ O-PP-F 1 mode! sets PA1 in fast output mode push/pull
\ $10 io. displays register settings for PB0
\ 1 $34 io! sets PD4
\ 1 iox! toggles PA1

RAM
: _ ;

#require MARKER

MARKER regs

\ Relative register addresses
$5000 CONSTANT GPIO-BASE
0 CONSTANT GPIO.ODR
1 CONSTANT GPIO.IDR
2 CONSTANT GPIO.DDR
3 CONSTANT GPIO.CR1
4 CONSTANT GPIO.CR2

NVM

: 0<>     ( n --- f)  0 = not ;         \ not equal to zero
: lshift  ( n --- n)  dup 0= if 
    drop else
    0 do 2 * loop 
  then
;  \ shift left n bits

\ turn a bit position into a single-bit mask
: bit ( u -- u )  1 swap lshift ;

\ hexadicimal output
: h. base @ swap hex . base ! ;


\ gpio modes, see STM8S-RefManual 11.3
: O-PP %110 ;          \ output push/pull
: O-PP-F %111 ;     \ output push/pull fast
: O-OD %100 ;          \ output open drain
: O-OD-F %101 ;     \ output open drain fast
: I-F %000 ;       \ input floating
: I-F-I %001 ;   \ input floating with interrupt
: I-P %010 ;        \ input pull-up 
: I-P-I %011 ;    \ input pull-up with interrupt

\ pin =  $port#pin# e.g. $13 = PB3, $0 = PA0, $37 = PD7

\ Convert pin to register address
: io-b ( pin -- GPIO.BASE)
   $F0 and $F / 5 * GPIO-BASE + ;

\ Convert pin to pin#
: io# ( pin -- pin#) $F and ;

\ Convert pin to port name
: io-p $F / &65 + emit ;

\ Set gpio registers 	e.g. 1 GPIO.DDR $23 io# sets DDR bit of PC3
: io-r!  ( f reg pin --)
   dup io-b rot + swap io# b! 
;

\ Set pin mode   e.g. o-pp $23 m! configures PC3 as output port push/pull
: m! ( mode pin --) 	
   2dup 2dup swap 4 / GPIO.DDR rot io-r! 
   swap 2 / 1 and GPIO.CR1 rot io-r!
   swap 1 and GPIO.CR2 rot io-r! 
;

\ Convert pin to mask
: io-m ( pin -- m) io# bit ;

\ Get pin value
: io@ ( pin -- f)
   dup io-m swap 
   io-b GPIO.IDR + C@ and 0<>  negate 
;

\ Set pin 
: io! ( f pin -- )
   dup io-b GPIO.ODR + swap io# b! 
;
 
\ Toggle pin
: iox ( pin -- )
   dup io@ ( p f) 0= swap io! 
;

\ display readable GPIO registers associated with a pin
: io. ( pin -- )  
   cr dup io-m >R ( pin)
   ." Base-addr:0x" dup io-b  dup h.
   ."   PIN:" swap dup io# . 
   ."   PORT: " dup io-p 
   io-b ( addr-b) 
   ."   ODR:" dup c@ R@ ( addrb b m) and 0<> negate . 1 +
   ."   IDR:" dup c@ R@ and 0<> negate . 1 +
   ."   DDR:" dup c@ R@ and 0<> negate . 1 +
   ."   CR1:" dup c@ R@ and 0<> negate . 1 +
   ."   CR2:" c@ R> and 0<> negate . drop 
;

regs
