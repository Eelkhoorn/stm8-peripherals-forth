STM8EF_BOARD=MINDEV
STM8EF_VER=2.2.28
STM8EF_BIN=stm8ef-bin.zip
STM8EF_URL=https://github.com/TG9541/stm8ef/releases/download/${STM8EF_VER}/${STM8EF_BIN}

all: flash

defaults:
	stm8flash -c stlinkv2 -p stm8s103f3 -s opt -w tools/stm8s103FactoryDefaults.bin

flash: depend
	stm8flash -c stlinkv2 -p stm8s103f3 -w out/MINDEV/MINDEV.ihx

load: depend
	tools/codeload.py serial main.fs

simload: depend
	tools/simload.sh $(STM8EF_BOARD)

flash_oled: flash 
	tools/codeload.py serial I2ISR
	tools/codeload.py serial DIA
	tools/codeload.py serial OLED

depend:
	if [ ! -d "out" ]; then \
		curl -# -L -O ${STM8EF_URL}; \
		unzip -q -o ${STM8EF_BIN} -x out/*; \
		unzip -q -o ${STM8EF_BIN} out/${STM8EF_BOARD}/*; \
		rm ${STM8EF_BIN}; \
		ln -s out/${STM8EF_BOARD}/target target; \
	fi

clean:
	rm -rf target FORTH.efr STM8S103.efr STM8S105.efr docs lib inc mcu out tools forth.asm forth.mk main.c LICENSE.md
