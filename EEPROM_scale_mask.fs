

#require LOCK 
#require ULOCK

\ write robotron font to eeprom from ASCII 32 to 127 from byte 0 on eeprom 
ULOCK 
HEX 

\ saving scale LUT entry %0000 -> %00000000 into $4270 
 %00000000 $4270 C! 

\ saving scale LUT entry %0001 -> %00000011 into $4271 
 %00000011 $4271 C! 

\ saving scale LUT entry %0010 -> %00001100 into $4272 
 %00001100 $4272 C! 

\ saving scale LUT entry %0011 -> %00001111 into $4273 
 %00001111 $4273 C! 

\ saving scale LUT entry %0100 -> %00110000 into $4274 
 %00110000 $4274 C! 

\ saving scale LUT entry %0101 -> %00110011 into $4275 
 %00110011 $4275 C! 

\ saving scale LUT entry %0110 -> %00111100 into $4276 
 %00111100 $4276 C! 

\ saving scale LUT entry %0111 -> %00111111 into $4277 
 %00111111 $4277 C! 

\ saving scale LUT entry %1000 -> %11000000 into $4278 
 %11000000 $4278 C! 

\ saving scale LUT entry %1001 -> %11000011 into $4279 
 %11000011 $4279 C! 

\ saving scale LUT entry %1010 -> %11001100 into $427A 
 %11001100 $427A C! 

\ saving scale LUT entry %1011 -> %11001111 into $427B 
 %11001111 $427B C! 

\ saving scale LUT entry %1100 -> %11110000 into $427C 
 %11110000 $427C C! 

\ saving scale LUT entry %1101 -> %11110011 into $427D 
 %11110011 $427D C! 

\ saving scale LUT entry %1110 -> %11111100 into $427E 
 %11111100 $427E C! 

\ saving scale LUT entry %1111 -> %11111111 into $427F 
 %11111111 $427F C! 

LOCK

