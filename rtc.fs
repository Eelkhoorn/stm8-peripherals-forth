\ Real time clock modules DS1307 and DS3231, I2C communication
\ DS3231 is much more accurate and has 4kb eeprom
\ Registers 0:6  :  BCD data for sec, min, hour, #day(1:7), date, month, year(0:99)

$68 CONSTANT rtc     \ slave address clock
$57 CONSTANT eeprom  \ eeprom slave address

NVM

VARIABLE tb $19 allot

: RDC   \ Read clock
   tb 7 0 rtc i2rf
;

: SETC  ( YY MM DD d hh mm ss)  \ Set clock reg's 6:0, BCD input
   tb 7 0 DO DUP ROT SWAP C! 1+ LOOP DROP
   tb 7 0 rtc i2wf ( buf-adr n reg-adr i2c-adr --)
;

: SCRG  ( b reg-adr --)  \ set single clock register
   rtc i2wb ( b reg-adr i2c-adr)
;

: h. BASE @ >r HEX . r> BASE ! ;

: time
   RDC cr
   tb 2+ C@ $3F AND h. ."  : " tb 1+ C@ h. ."  : " tb C@ h.
;

: date
   RDC cr
   tb 4 + C@ h. ."  / " tb 5 + C@ h. ."  / '" tb 6 + C@ h.
;

\ EEPROM words, slave address $57

\ write n bytes from buffer to eeprom address
: eew ( buf-adr n #byte #page --)
   i2s eeprom 0 i2a i2sb i2sb i2sf i2p
;

\ load buffer with n bytes from eeprom address
: eer ( buf-adr n #byte #page --)
   i2s eeprom 0 i2a i2sb i2sb i2p i2s eeprom 1 i2a i2cf
;

RAM

\\ example

hex
18 3 15 4 23 55 45 setc  \ march 15 2018 23:55:45
date time
5 11 scrg				 \ set clock reg. 5 (month) to 11

Clock registers 0:6
0 sec
1 min
2 hr
3 #day (1-7)
4 day
5 month
6 year (0-99)



