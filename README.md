# forth-oled-display

Forth words to drive a ssd1306 oled mini display
 with stm8ef (https://github.com/TG9541/stm8ef).
 
 These words utilize the i2c capabilities of the stm8.
 
 Assumed i2c-address: $3C
 
 ssdi ( --) 	  initialise i2c and ssd1306-display
 
 cls ( --)  	  clear screen
 
 dtxt ( adr --)	display text compiled with $"
 
 d#	( n --)     display number
 
 Text has to be compiled before it can be displayed.
 
 	: txt $" text to be displayed" ;
  

txt dtxt will display "text to be displayed" on the oled screen.

1234 d#	will display "1234".


The display has 8 pages (lines) of 128x8 dots.
Positioning is done by sending display commands:

dcmd (b --)	 	send one display command

dcmds (b b .. b n --)	send multiple (n) display commands

Display positioning commands:

$B0 - $B7	Vertical position: Line 0 - line7

0-$F		Horizontal position in steps of 1 dot

$10 -$17	Horizontal position in steps of 16 dots
 


    $B2 $13 $5 3 dcmds 

will position to third line ($B2), dot 53    ($13 = 3 x 16, $5 = 5, together 53).

dcmds needs the number of display commands to be sended.




TODO prevent freezing when no display is connected.
 
