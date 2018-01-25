\ Initialise spi communication with sd card

RAM
: _ ;

#require MARKER
\ #require spi.fs

NVM
variable sdb 511 allot   \ data buffer, 512 bytes 

MARKER regs

\res MCU: STM8S103
\res export PA_ODR
\res export PA_DDR
\res export PA_CR1
\res export PA_CR2
\res export SPI_DR

NVM
: uss ( u -- ) 1 swap 0 do 2* loop drop ;  \ delay

: +spi ( -- ) 0 PA_ODR 3 b! ;        \ select SPI
: -spi ( -- ) 1 PA_ODR 3 b! ;        \ deselect SPI

: sd-cmd ( cmd argl argh -- u )    \ argl and argh Little Endian
     rot dup 8 = if 6 >R $87 >R     \ SEND_IF_COND, 4 bytes
     else dup 58 = if 6 >R $FF >R   \ Read OCR, 4 bytes
     else 2 >R $95 >R then then
     $ff >spi
     $40 or >spi
     dup >spi
     $100 / >spi
     dup >spi
     $100 / >spi
     R> >spi                         \ CRC
     2 uss
     R> 0 do spi> loop  5 uss SPI_DR c@
;

: sdi    \ initialise sd card and buffers
   1 PA_DDR 3 B!             \ cs pin PA3 output
   1 PA_CR1 3 B!             \ PA3 push/pull
   1 PA_CR2 3 B!             \ PA3 fast mode
   SPIonS                    \ spi slow
   -spi 10 0 do spi> loop 1 uss +spi   \ Forse SPI mode
   0 0 0 sd-cmd  drop        \ CMD0 go idle 
   8 $aa01 0 sd-cmd          
   begin 55 0 0 sd-cmd drop
      41 0 $40 sd-cmd
      0= until
      drop
   SPIonF  
;

regs

