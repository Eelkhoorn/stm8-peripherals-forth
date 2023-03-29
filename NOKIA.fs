
#require ]C!
#require ]B!

$5200 CONSTANT SPI_CR1
$5201 CONSTANT SPI_CR2
$5203 CONSTANT SPI_SR
$5204 CONSTANT SPI_DR

NVM

: SPIon ( baud -- )
  [
    $5C C,              \  INCW    X         ; pull baud
    $F6 C,              \  LD      A,(X)
    $5C C,              \  INCW    X
    $A407 ,             \  AND     A,#7      ; CPOL=CPHA=0
    $4E C,              \  SWAP    A         ; 16 *
    $47 C,              \  SRA     A         ; 2 /
    $AA04 ,             \  OR      A,#4      ; set master mode
    $C7 C, SPI_CR1 , ]  \  LD      SPI_CR1,A
   $01 SPI_CR2 C!     \ no NSS, FD, no CRC
   1 SPI_CR1 6 B!    \ SPI enable
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


$500A CONSTANT PC_ODR
$500B CONSTANT PC_IDR  \ Port C input pin value register           0xXX
$500C CONSTANT PC_DDR  \ Port C data direction register            0x00
$500D CONSTANT PC_CR1  \ Port C control register 1                 0x00
$500E CONSTANT PC_CR2  \ Port C control register 2                 0x00


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

\ set horizontal cursor to c ( 0..83 )
: LCDx ( c -- )
  [ 0 LCDdc ]B!
  $80 + LCDout
  [ 1 LCDdc ]B!
;

\ set vertical cursor to c ( 0..5 )
: LCDy ( c -- )
  [ 0 LCDdc ]B!
  $40 + LCDout
  [ 1 LCDdc ]B!
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

\ init ports, SPI, PD8544 with Vop c (0..127)
: LCDinit ( c -- )
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
  65 LCDinit  \ init, set contrast value
;

RAM


\ test
init
$AA LCDfill \ fill the LCD with horizontal lines
