\ RealTimeClock, "Tiny RTC I2C Module" DS1307

RAM
: _ ;

#require MARKER
\ #require i2c.fs

$68 CONSTANT rtc

MARKER clean

NVM

VARIABLE tb 7 ALLOT


: h. BASE @ >r HEX . r> BASE ! ;

: setc  ( YY MM DD d hh mm ss)  \ Set clock reg's 6:0, BCD input
   tb 7 0 DO DUP ROT SWAP C! 1+ LOOP DROP
   tb 7 0 rtc i2wf
;
	
: time
   tb 7 0 rtc i2rf cr
   tb 2+ C@ $3F AND h. ."  : " tb 1+ C@ h. ."  : " tb C@ h.
;

: date
   tb 7 0 rtc i2rf cr
   tb 4 + C@ h. ."  / " tb 5 + C@ h. ."  / '" tb 6 + C@ h.
;
   
clean
