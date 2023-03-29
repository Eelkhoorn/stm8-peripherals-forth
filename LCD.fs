
#require ]C!
#require ]B!

$5200 CONSTANT SPI_CR1
$5201 CONSTANT SPI_CR2
$5203 CONSTANT SPI_SR
$5204 CONSTANT SPI_DR

$500A CONSTANT PC_ODR  \ Port C data output latch register         0x00
$500B CONSTANT PC_IDR  \ Port C input pin value register           0xXX
$500C CONSTANT PC_DDR  \ Port C data direction register            0x00
$500D CONSTANT PC_CR1  \ Port C control register 1                 0x00
$500E CONSTANT PC_CR2  \ Port C control register 2                 0x00

$500F CONSTANT PD_ODR  \ Port D data output latch register         0x00
$5010 CONSTANT PD_IDR  \ Port D input pin value register           0xXX
$5011 CONSTANT PD_DDR  \ Port D data direction register            0x00
$5012 CONSTANT PD_CR1  \ Port D control register 1                 0x00
$5013 CONSTANT PD_CR2  \ Port D control register 2                 0x00

NVM

: waitms ( n -- ) 5 / TIM + BEGIN DUP TIM < UNTIL DROP ;

: SPIon ( -- )
  [ $34 SPI_CR1 ]C! \ 00110100 -> 0: LSB first, 0: disabled, 110: f_MASTER/128, 1: Master, 0: SCK 0 when idle, 0: first clock transistion capture edge
  [ $01 SPI_CR2 ]C! \ no NSS, FD, no CRC
  [ 1 SPI_CR1 6 ]B!    \ SPI enable
;

\ disable SPI
: SPIoff ( -- )
  [ 0 SPI_CR1 ]C!    \ disable SPI
;

\ Perform SPI byte cycle with result c
: SPI ( c -- c)
  [ $E601 ,                 \ LD A,(1,X)
    $C7 C,  SPI_DR ,        \ LD SPI_DR,A
    $7201 , SPI_SR , $FB C, \ BTJF SPI_SR,#SPIRXNE_WAIT (0)
    $C6 C,  SPI_DR ,        \ LD A,SPI_DR
    $E701 ,                 \ LD (1,X),A
    $7F C, ]                \ CLR  (X)
;


: LCDrst ( -- n n )
  \ configure port and bit# for chip enable (/RST)
  PD_ODR 2 \ PD2
;

: LCDsce ( -- n n )
  \ configure port and bit# for chip enable (/SCE)
  PC_ODR 3   \ PC3
;

: LCDdc ( -- n n )
  \ configure port and bit# for data/command (D/C)
  PC_ODR 4   \ PC4
;

\ send c through SPI, discard input
: LCDout ( c -- )
  [ 0 LCDsce ]B!
  SPI
  [ 1 LCDsce ]B!
  DROP
;

\ LCD cmd wrapper
: LCDcmd ( c -- )
  [ 0 LCDdc ]B!
  LCDout
  [ 1 LCDdc ]B!
;

\ set horizontal cursor to c ( 0..83 )
: LCDx ( c -- )
  $80 + LCDcmd
;

\ set vertical cursor to c ( 0..5 )
: LCDy ( c -- )
  $40 + LCDcmd
;

\ set cursor to (0,0)
: LCDhome ( -- )
  0 LCDx 0 LCDy
;

\ fill LCD with pattern c (use y auto increment)
: LCDfill ( c -- )
  LCDhome
  503 FOR
    DUP LCDout
  NEXT DROP
;

\ : LCDhide [ 0 LCDdc ]B! $08 LCDout [ 1 LCDdc ]B! ; 
: LCDhide $08 LCDcmd ; 
\ : LCDunhide [ 0 LCDdc ]B! $0C LCDout [ 1 LCDdc ]B! ; 
: LCDunhide $0C LCDcmd ; 

: LCDblink 5 0 DO LCDhide 100 waitms LCDunhide 400 waitms 1 +LOOP ;

\ init ports, SPI, PD8544 with Vop c (0..127)
: LCDinit ( c -- )
  [ 0 LCDrst ]B! \ set reset low
  20 waitms
  [ 1 LCDrst ]B! \ set reset high
  [ 0 LCDdc ]B!
  $21 LCDout \ instructions extended
  $14 LCDout \ bias 4
  $80 + LCDout \ Vop, e.g. 60
  $20 LCDout \ instructions normal
  $0C LCDout \ display conf ($08:blank,$09:on,$0C:normal,$0D:inv)
  [ 1 LCDdc ]B!
  0 LCDfill
;

\ copy n chars starting at a to PD8544
: LCDcpy ( a n -- )
  FOR DUP C@ LCDout 1+ NEXT DROP
;


: init
  [ $18 PC_DDR ]C!
  [ $18 PC_CR1 ]C!
  1 SPIon 
  $2E LCDinit  \ init, set contrast value
;

RAM


\ test
init
\ $0A LCDfill \ fill the LCD with horizontal lines
