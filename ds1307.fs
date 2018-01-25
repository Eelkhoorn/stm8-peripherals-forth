\ RealTimeClock, "Tiny RTC I2C Module"

RAM
: _ ;

#require MARKER
\ #require i2c.fs

$68 CONSTANT i2c-adr

NVM

VARIABLE bf 7 ALLOT

MARKER clean

\ initialise clock with day 2, 23/01/'17 13:05:00
create buf $0005 , $1302 , $2301 , $17 C,
buf 7 0 i2c-adr i2c-wbf

NVM

: h. BASE @ >r HEX . r> BASE ! ;


: clck
   bf 7 0 i2c-adr i2c-lbf cr
   bf 2+ C@ $3F AND h. ."  : " bf 1+ C@ h. ."  : " bf C@ h.
;

: date 
   bf 7 0 i2cadr i2c-lbf cr
   bf 4 + C@ h. ."  / " bf 5 + C@ h. ."  / '" bf 6 + C@ h.
;
   
clean
