# forth-oled-display

Forth words to drive a ssd1306 oled mini display
 with stm8ef (https://github.com/TG9541/stm8ef).
 
 These words utilize the i2c capabilities of the stm8.
 Assumed i2c-address: $3C
 
 ssdi ( --) 	  initialise i2c and ssd1306-display
 
 cls ( --)  	  clear screen
 
 dtxt ( adr --)	display text compiled with $"
 
 d#	( n --)     display number
 
 Text has to be compiled before it can be displayed
 	: txt $" text to be displayed" ;

txt dtxt will display "text to be displayed" on the oled screen.

1234 d#	will display "1234".

TODO prevent freezing when no display is connected.
 
