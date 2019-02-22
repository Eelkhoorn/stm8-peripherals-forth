# stm8-peripherals-forth

This repository is based on https://github.com/TG9541/stm8ef .

Thomas supplied a Makefile that gets you started in no time:

   hook up a ST-link-V2 to your stm8 board, 
   run "make" in terminal and you are ready to go.
  
i2c.fs:     I2C communication

ssd1306.fs: I2C oled display

rtc.fs: I2C RealTimeClock modules

sdinit.fs:  Initialise SD card for SPI communication

sdfat.fs:   SD card FAT32 communication

io.fs:      GPIO manipulation

nRF:         remote console with nRF24 radio's based on Richard's library

nRF-L:       the same for STM8L051
