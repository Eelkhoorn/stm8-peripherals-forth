\ Source Mecrisp-Stellaris io.fs, adapted for STM8S

: 0<>     ( n --- f)  0 = not ;         \ not equal to zero
: lshift  ( n --- n)  0 do $2 * loop ;  \ shift left n bits

\ turn a bit position into a single-bit mask
: bit ( u -- u )  1 swap lshift ;

\ hexadicimal output
: hex. base @ swap hex . base ! ;

\ Relative register addresses
: GPIO-BASE $5000 ;
: GPIO.ODR &0 ;
: GPIO.IDR &1 ;
: GPIO.DDR &2 ;
: GPIO.CR1 &3 ;
: GPIO.CR2 &4 ;

\ gpio modes, see STM8S-RefManual 11.3
: OMODE-PP %110 ;          \ output push/pull
: OMODE-PP-FAST %111 ;     \ output push/pull fast
: OMODE-OD %100 ;          \ output open drain
: OMODE-OD-FAST %101 ;     \ output open drain fast
: IMODE-FLOAT %000 ;       \ input floating
: IMODE-FLOAT-INT %001 ;   \ input floating with interrupt
: IMODE-PULL %010 ;        \ input pull-up 
: IMODE-PULL-INT %011 ;    \ input pull-up with interrupt

\ pin =  $port#pin# e.g. $13 = PB3, $0 = PA0, $37 = PD7

\ Convert pin to register address
: io-base ( pin -- GPIO.BASE) $F0 and $F / 5 * GPIO-BASE + ;

\ Convert pin to pin#
: io# ( pin -- pin#) $F and ;

\ Convert pin to port name
: io-port $F / &65 + emit ;

\ Set gpio registers 	e.g. 1 GPIO.DDR 23 io# sets DDR bit of PC3
: io-reg!  ( f reg pin --)  dup io-base rot + swap io# b! ;

: io-mode! ( mode pin --) 	2dup 2dup swap 4 / GPIO.DDR rot io-reg! 
						swap 2 / 1 and GPIO.CR1 rot io-reg!
					swap 1 and GPIO.CR2 rot io-reg! ;
\ Convert pin to mask
: io-mask ( pin -- m) io# bit ;

\ Get pin value
 : io@ ( pin -- f) 	dup io-mask swap 
	io-base GPIO.IDR + C@ and 0<>  negate ;
\ Set pin 
 : io! ( f pin -- )  dup io-base GPIO.ODR + swap io# b! ;
 
\ Toggle pin
: iox! ( pin -- )  dup io@ ( p f) 0= swap io! ;

\ display readable GPIO registers associated with a pin
: io. ( pin -- )  
 cr dup io-mask >R ( pin)
 ." Base-addr:0x" dup io-base  dup hex.
 ."   PIN:" swap dup io# . 
 ."   PORT: " dup io-port 
 io-base ( addr-b) 
 ."   ODR:" dup c@ R@ ( addrb b m) and 0<> negate . 1 +
 ."   IDR:" dup c@ R@ and 0<> negate . 1 +
 ."   DDR:" dup c@ R@ and 0<> negate . 1 +
 ."   CR1:" dup c@ R@ and 0<> negate . 1 +
 ."   CR2:" c@ R> and 0<> negate . drop ;					

