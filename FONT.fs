

\ #r LCD.fs
\ #require LCD.fs

HEX

\ NVM

\ font handling
: getCadr ( n -- a b )
  $20 - 5 * $4000 + dup 4 +  ;

: get_char ( n -- a b c d e ) 
  getCadr DO  I C@ -1 +LOOP ;

: emitLCD get_char 0 LCDout LCDout LCDout LCDout LCDout LCDout ;

: testline  &13 DO $40 I + emitLCD -1 +LOOP ;
: testlines init . LCDhome FOR . testline NEXT ;

\ : dbg .s cr ;
: dbg ;
: lower dbg &16 / dbg $4270 + C@ dbg ;
: upper dbg &16 MOD $4270 + C@ dbg ;
: eu1 upper dup LCDout LCDout ;
: el1 lower dup LCDout LCDout ;
: emit_upper get_char 0 eu1 eu1 eu1 eu1 eu1 eu1 ;
: emit_lower get_char 0 el1 el1 el1 el1 el1 el1 ;
: emit_big dup LCDhome emit_upper 0 LCDx 1 LCDy emit_lower ;
: tt init . $4F emit_big ;

VARIABLE LCDBUF 8 ALLOT
0 VARIABLE LCDx
0 VARIABLE LCDy


: emitBuf 3 FOR CR I .  6 FOR I . CR NEXT drop NEXT ;


\ RAM
