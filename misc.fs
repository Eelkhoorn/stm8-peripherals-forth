
: emitAdr C@ . ;

: emitRange CR CR SWAP DUP ROT + SWAP DO  I emitAdr LOOP CR ;

: clean5 8 8 8 8 8 EMIT EMIT EMIT EMIT EMIT ;

: emitTim HEX CR BEGIN TIM . clean5 ?KEY UNTIL CR ." done " EMIT ;

: wait ( n -- ) BEGIN DUP TIM SWAP MOD 0 = UNTIL DROP ;

: waitms ( n -- ) 5 / TIM + BEGIN DUP TIM < UNTIL DROP ;

: mw ( n -- ) TIM SWAP wait TIM SWAP - . ;

: emitMw ( n -- ) CR BEGIN DUP mw ?KEY UNTIL DROP ;


