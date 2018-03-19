\ Real time clock module DS3231, I2C communication
\ Registers 0:6  :  BCD data for sec, min, hour, #day(1:7), date, month, year(0:99)

$68 CONSTANT rtc  \ slave address

NVM
VARIABLE tb 15 allot

: RDC   \ Read clock
   tb 7 0 rtc i2rf
;

: SETC  ( YY MM DD d hh mm ss)  \ Set clock reg's 6:0, BCD input
   tb 7 0 DO DUP ROT SWAP C! 1+ LOOP DROP
   tb 7 0 rtc i2wf ( buf-adr n reg-adr i2c-adr --)
;

: SCRG  ( b reg-adr --)  \ set single clock register
   rtc i2b ( b reg-adr i2c-adr)
;

: PRC  \  Print clock data
  base @ >R hex
  tb 7 0 do dup c@ cr i . space . 1+ loop drop
  R> base !
;

: time
   tb 7 0 rtc i2rf cr
   tb 2+ C@ $3F AND h. ."  : " tb 1+ C@ h. ."  : " tb C@ h.
;

: date
   tb 7 0 rtc i2rf cr
   tb 4 + C@ h. ."  / " tb 5 + C@ h. ."  / '" tb 6 + C@ h.
;

RAM

\\ example

hex
18 3 15 4 23 55 45 setc  \ march 15 2018 23:55:45
rdc prc					 \ read and display clock data
5 11 scrg				 \ set clock reg. 5 (month) to 11

0 sec
1 min
2 hr
3 #day (1-7)
4 day
5 month
6 year (0-99)



