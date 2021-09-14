

\ #r LCD.fs

HEX

NVM

\ font handling
: getCadr ( n -- a b )
  $20 - 5 * $4000 + dup 4 +  ;

: get_char ( n -- a b c ) 
  getCadr DO  I C@ -1 +LOOP ;

: emitLCD get_char 0 LCDout LCDout LCDout LCDout LCDout LCDout ;

RAM
