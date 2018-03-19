# stm8-peripherals-forth

This repository is based on https://github.com/TG9541/stm8ef .

Thomas supplied a Makefile that gets you started in no time:

   hook up a ST-link-V2 to your stm8 board
  
   run "make" in terminal and you are ready to go.
  
  
i2c.fs: I2C communication

ssd1306.fs: I2C oled display

ds1307.fs: I2C RTC module

ds3231.fs: another I2C RTC module, more accurate

sdinit.fs: Initialise SD card for SPI communication

sdfat.fs: SD card FAT32 communication

io.fs:   GPIO manipulation
 
