\ Driver for ssd1306 oled display 128x64 over i2c
\ font from mecrisp-stellaris 2.2.1a (GPL3) 
\ http://mecrisp.sourceforge.net

\ ssdi ( --) initialise i2c and ssd1306-display
\ cls ( --) clear screen
\ dtxt ( adr --)	display text compiled with $"
\ Text has to be compiled before it can be displayed.
\ : txt $" text to be displayed" ;
\ txt dtxt will display "text to be displayed" on the oled screen.
\ d#	( n --) display number
\ 1234 d#	will display "1234".

\ The display has 8 pages (lines) of 128x8 dots. Positioning is done by sending display commands:
\ dcmd (b --)	send one display command
\ dcmds (b b .. b n --)	send multiple (n) display commands

\ Display positioning commands:
\ $B0 - $B7	Vertical position: Line 0 - line7
\ 0-$F	Horizontal position in steps of 1 dot
\ $10 -$17	Horizontal position in steps of 16 dots
\ $B2 $13 $5 3 dcmds 
\ will position to third line ($B2), dot 53 ($13 = 3 x 16, $5 = 5, together 53).
\ dcmds needs the number of display commands to be send.

#require MARKER
\ #require i2c.fs

NVM

variable page &127 allot

MARKER i2caddr

$3c constant i2c-adr  

NVM

\ display command:
   : dcmd ( b --) 0 i2c-adr i2c-wb ;

\ multiple display commands:
   : dcmds ( b b .. b n --)
	0 do dcmd loop ;

create ssd-init
\ * = vccstate dependant
 $AE C,  \ SSD1306_DISPLAYOFF
 $D5 C,  \ SSD1306_SETDISPLAYCLOCKDIV
 $80 C,  \ 
 $A8 C,  \ SSD1306_SETMULTIPLEX
 $3F C,  \ SSD1306_LCDHEIGHT - 1
 $D3 C,  \ SSD1306_SETDISPLAYOFFSET
 $0  C,  \ no offset
 $40 C,  \ SSD1306_SETSTARTLINE \ line #0
 $8D C,  \ SSD1306_CHARGEPUMP
 $14 C,  \ *
 $20 C,  \ SSD1306_MEMORYMODE
 $0  C,  \ 0x0 act like ks0108
 $A0 C,  \ SSD1306_SEGREMAP
 $C0 C,  \ SSD1306_COMSCANDEC
 $DA C,  \ SSD1306_SETCOMPINS
 $12 C,  \ 
 $81 C,  \ SSD1306_SETCONTRAST
 $CF C,  \ *
 $D9 C,  \ SSD1306_SETPRECHARGE
 $F1 C,  \ *
 $DB C,  \ SSD1306_SETVCOMDETECT
 $40 C,  \ 
 $A4 C,  \ SSD1306_DISPLAYALLON_RESUME
 $A6 C,  \ SSD1306_NORMALDISPLAY
 $2E C,  \ SSD1306_DEACTIVATE_SCROLL
 $AF C,  \ SSD1306_DISPLAYON
 
\ Initialise display
: ssdi ( --)
   i2c-init ssd-init &27 0 i2c-adr i2c-wbf ;

\ write byte in display memory:
: wram ( b --) 
   $40 i2c-adr i2c-wb ;

\ Write page:
: wpage ( --)
   0 $10 2 dcmds page $80 $40 i2c-adr i2c-wbf ; 

\ Write screen = 8 pages: 
: wsc ( --)
   8 0 do i $B0 + dcmd wpage loop ;

\ Fill screen with character
: fscr ( b --)
   page $80 rot fill wsc ;

\ Clear screen
: cls  ( --)
   0 fscr ;      

\ Write pattern:
: test ( --)
   $b0 0 $10 3 dcmds 8 0 do i $b0 + dcmd
   $10 0 do $FF wram loop loop ;

hex
create font   \ 5x8    
  00 c, 00 c, 00 c, 00 c, 00 c, \
  00 c, 00 c, 4F c, 00 c, 00 c, \ !
  00 c, 03 c, 00 c, 03 c, 00 c, \ "
  14 c, 3E c, 14 c, 3E c, 14 c, \ #
  24 c, 2A c, 7F c, 2A c, 12 c, \ $
  63 c, 13 c, 08 c, 64 c, 63 c, \ %
  36 c, 49 c, 55 c, 22 c, 50 c, \ &
  00 c, 00 c, 07 c, 00 c, 00 c, \ '
  00 c, 1C c, 22 c, 41 c, 00 c, \ (
  00 c, 41 c, 22 c, 1C c, 00 c, \ )
  0A c, 04 c, 1F c, 04 c, 0A c, \ *
  04 c, 04 c, 1F c, 04 c, 04 c, \ +
  50 c, 30 c, 00 c, 00 c, 00 c, \ ,
  08 c, 08 c, 08 c, 08 c, 08 c, \ -
  60 c, 60 c, 00 c, 00 c, 00 c, \ .
  00 c, 60 c, 1C c, 03 c, 00 c, \ /
  3E c, 41 c, 49 c, 41 c, 3E c, \ 0
  00 c, 02 c, 7F c, 00 c, 00 c, \ 1
  46 c, 61 c, 51 c, 49 c, 46 c, \ 2
  21 c, 49 c, 4D c, 4B c, 31 c, \ 3
  18 c, 14 c, 12 c, 7F c, 10 c, \ 4
  4F c, 49 c, 49 c, 49 c, 31 c, \ 5
  3E c, 51 c, 49 c, 49 c, 32 c, \ 6
  01 c, 01 c, 71 c, 0D c, 03 c, \ 7
  36 c, 49 c, 49 c, 49 c, 36 c, \ 8
  26 c, 49 c, 49 c, 49 c, 3E c, \ 9
  00 c, 33 c, 33 c, 00 c, 00 c, \ :
  00 c, 53 c, 33 c, 00 c, 00 c, \ ;
  00 c, 08 c, 14 c, 22 c, 41 c, \ <
  14 c, 14 c, 14 c, 14 c, 14 c, \ =
  41 c, 22 c, 14 c, 08 c, 00 c, \ >
  06 c, 01 c, 51 c, 09 c, 06 c, \ ?
  3E c, 41 c, 49 c, 15 c, 1E c, \ @
  78 c, 16 c, 11 c, 16 c, 78 c, \ A
  7F c, 49 c, 49 c, 49 c, 36 c, \ B
  3E c, 41 c, 41 c, 41 c, 22 c, \ C
  7F c, 41 c, 41 c, 41 c, 3E c, \ D
  7F c, 49 c, 49 c, 49 c, 49 c, \ E
  7F c, 09 c, 09 c, 09 c, 09 c, \ F
  3E c, 41 c, 41 c, 49 c, 7B c, \ G
  7F c, 08 c, 08 c, 08 c, 7F c, \ H
  00 c, 41 c, 7F c, 41 c, 00 c, \ I
  38 c, 40 c, 40 c, 41 c, 3F c, \ J
  7F c, 08 c, 08 c, 14 c, 63 c, \ K
  7F c, 40 c, 40 c, 40 c, 40 c, \ L
  7F c, 06 c, 18 c, 06 c, 7F c, \ M
  7F c, 06 c, 18 c, 60 c, 7F c, \ N
  3E c, 41 c, 41 c, 41 c, 3E c, \ O
  7F c, 09 c, 09 c, 09 c, 06 c, \ P
  3E c, 41 c, 51 c, 21 c, 5E c, \ Q
  7F c, 09 c, 19 c, 29 c, 46 c, \ R
  26 c, 49 c, 49 c, 49 c, 32 c, \ S
  01 c, 01 c, 7F c, 01 c, 01 c, \ T
  3F c, 40 c, 40 c, 40 c, 7F c, \ U
  0F c, 30 c, 40 c, 30 c, 0F c, \ V
  1F c, 60 c, 1C c, 60 c, 1F c, \ W
  63 c, 14 c, 08 c, 14 c, 63 c, \ X
  03 c, 04 c, 78 c, 04 c, 03 c, \ Y
  61 c, 51 c, 49 c, 45 c, 43 c, \ Z
  00 c, 7F c, 41 c, 00 c, 00 c, \ [
  00 c, 03 c, 1C c, 60 c, 00 c, \ \
  00 c, 41 c, 7F c, 00 c, 00 c, \ ]
  0C c, 02 c, 01 c, 02 c, 0C c, \ ^
  40 c, 40 c, 40 c, 40 c, 40 c, \ _
  00 c, 01 c, 02 c, 04 c, 00 c, \ `
  20 c, 54 c, 54 c, 54 c, 78 c, \ a
  7F c, 48 c, 44 c, 44 c, 38 c, \ b
  38 c, 44 c, 44 c, 44 c, 44 c, \ c
  38 c, 44 c, 44 c, 48 c, 7F c, \ d
  38 c, 54 c, 54 c, 54 c, 18 c, \ e
  08 c, 7E c, 09 c, 09 c, 00 c, \ f
  0C c, 52 c, 52 c, 54 c, 3E c, \ g
  7F c, 08 c, 04 c, 04 c, 78 c, \ h
  00 c, 00 c, 7D c, 00 c, 00 c, \ i
  00 c, 40 c, 3D c, 00 c, 00 c, \ j
  7F c, 10 c, 28 c, 44 c, 00 c, \ k
  00 c, 00 c, 3F c, 40 c, 00 c, \ l
  7C c, 04 c, 18 c, 04 c, 78 c, \ m
  7C c, 08 c, 04 c, 04 c, 78 c, \ n
  38 c, 44 c, 44 c, 44 c, 38 c, \ o
  7F c, 12 c, 11 c, 11 c, 0E c, \ p
  0E c, 11 c, 11 c, 12 c, 7F c, \ q
  00 c, 7C c, 08 c, 04 c, 04 c, \ r
  48 c, 54 c, 54 c, 54 c, 24 c, \ s
  04 c, 3E c, 44 c, 44 c, 00 c, \ t
  3C c, 40 c, 40 c, 20 c, 7C c, \ u
  1C c, 20 c, 40 c, 20 c, 1C c, \ v
  1C c, 60 c, 18 c, 60 c, 1C c, \ w
  44 c, 28 c, 10 c, 28 c, 44 c, \ x
  46 c, 28 c, 10 c, 08 c, 06 c, \ y
  44 c, 64 c, 54 c, 4C c, 44 c, \ z
  00 c, 08 c, 77 c, 41 c, 00 c, \ {
  00 c, 00 c, 7F c, 00 c, 00 c, \ |
  00 c, 41 c, 77 c, 08 c, 00 c, \ }
  10 c, 08 c, 18 c, 10 c, 08 c, \ ~
decimal

\ Translates ASCII to address of bitpatterns:
: a>bp ( c -- c-adr ) 
  &32 max &127 min  &32 - 5 * font + ;

\ Display character:
: drc ( c --)
   a>bp 5 0 do dup c@ wram 1 + loop drop ;

\ spaces
: spc ( u --) 0 do 0 wram loop ;

\ display text compiled with $"
: dtxt ( adr --)
   count 0 do dup c@ dup &32 = if 3 spc drop 
   else drc 1 spc then 1+ loop drop ;
   
\ display number
: d# ( n --) dup abs <# #s swap sign #> 0 
	do dup c@ drc 1 spc 1+ loop drop ;

i2caddr
