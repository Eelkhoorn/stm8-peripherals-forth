\ 137 bytes
\ STM8S103 SPI
\ derived from al177/stm8ef/HC12/boardcore.inc
\ refer to github.com/TG9541/stm8ef/blob/master/LICENSE.md

RAM
: _ ;

#require MARKER

marker clean

\res MCU: STM8S103
\res export SPI_CR1	\ $5200
\res export SPI_CR2	\ $5201
\res export SPI_ICR	\ $5202
\res export SPI_SR	\ $5203
\res export SPI_DR	\ $5204
\res export SPI_CRCPR	\ $5205
\res export SPI_RXCRCR	\ $5206
\res export SPI_TXCRCR	\ $5207


NVM

\ Init and enable SPI, Fmaster/64
: SPIonS ( -- )
  $00 SPI_CR2 C!  \ no NSS, FD, no CRC
  $6C SPI_CR1 C!  \ enable SPI, CLK/64, master, CPOL=CPHA=0, MSB first
;

\ Init and enable SPI Fmaster/4
: SPIonF ( -- )
  $00 SPI_CR2 C!  \ no NSS, FD, no CRC
  $4C SPI_CR1 C!  \ enable SPI, CLK/4, master, CPOL=CPHA=0, MSB first
;


\ disable SPI 
: SPIoff ( -- )
  0 SPI_CR1  C!  \ disable SPI
;

\ Perform SPI byte cycle with result c
: SPI ( b -- b)
   	SPI_DR c!
	begin SPI_SR c@ 3 and until
	SPI_DR c@
;

: >SPI ( b -- ) SPI drop ;      \ send byte, drop response
: SPI> ( -- )   $FF >spi  ;     \ generate 8 clocks

clean
