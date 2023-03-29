

#require LOCK 
#require ULOCK

\ write robotron font to eeprom from ASCII 32 to 127 from byte 0 on eeprom 
ULOCK 
HEX 

\ ASCII 32 : ' ' at global address 4000 or EEPROM address: 0
$0000 $4000 ! $0000 $4002 ! $00 $4004 C! 

\ ASCII 33 : '!' at global address 4005 or EEPROM address: 5
$0000 $4005 ! $4F00 $4007 ! $00 $4009 C! 

\ ASCII 34 : '"' at global address 400A or EEPROM address: a
$0007 $400A ! $0007 $400C ! $00 $400E C! 

\ ASCII 35 : '#' at global address 400F or EEPROM address: f
$147F $400F ! $147F $4011 ! $14 $4013 C! 

\ ASCII 36 : '$' at global address 4014 or EEPROM address: 14
$242A $4014 ! $7F2A $4016 ! $12 $4018 C! 

\ ASCII 37 : '%' at global address 4019 or EEPROM address: 19
$2313 $4019 ! $0864 $401B ! $62 $401D C! 

\ ASCII 38 : '&' at global address 401E or EEPROM address: 1e
$304E $401E ! $5926 $4020 ! $50 $4022 C! 

\ ASCII 39 : ''' at global address 4023 or EEPROM address: 23
$0004 $4023 ! $0200 $4025 ! $01 $4027 C! 

\ ASCII 40 : '(' at global address 4028 or EEPROM address: 28
$001C $4028 ! $2241 $402A ! $00 $402C C! 

\ ASCII 41 : ')' at global address 402D or EEPROM address: 2d
$0041 $402D ! $221C $402F ! $00 $4031 C! 

\ ASCII 42 : '*' at global address 4032 or EEPROM address: 32
$1408 $4032 ! $3E08 $4034 ! $14 $4036 C! 

\ ASCII 43 : '+' at global address 4037 or EEPROM address: 37
$0808 $4037 ! $3E08 $4039 ! $08 $403B C! 

\ ASCII 44 : ',' at global address 403C or EEPROM address: 3c
$0040 $403C ! $3000 $403E ! $00 $4040 C! 

\ ASCII 45 : '-' at global address 4041 or EEPROM address: 41
$0808 $4041 ! $0808 $4043 ! $08 $4045 C! 

\ ASCII 46 : '.' at global address 4046 or EEPROM address: 46
$0060 $4046 ! $6000 $4048 ! $00 $404A C! 

\ ASCII 47 : '/' at global address 404B or EEPROM address: 4b
$2010 $404B ! $0804 $404D ! $02 $404F C! 

\ ASCII 48 : '0' at global address 4050 or EEPROM address: 50
$3E51 $4050 ! $4945 $4052 ! $3E $4054 C! 

\ ASCII 49 : '1' at global address 4055 or EEPROM address: 55
$0042 $4055 ! $7F40 $4057 ! $00 $4059 C! 

\ ASCII 50 : '2' at global address 405A or EEPROM address: 5a
$4261 $405A ! $5149 $405C ! $46 $405E C! 

\ ASCII 51 : '3' at global address 405F or EEPROM address: 5f
$2141 $405F ! $454B $4061 ! $31 $4063 C! 

\ ASCII 52 : '4' at global address 4064 or EEPROM address: 64
$1814 $4064 ! $127F $4066 ! $10 $4068 C! 

\ ASCII 53 : '5' at global address 4069 or EEPROM address: 69
$2745 $4069 ! $4545 $406B ! $39 $406D C! 

\ ASCII 54 : '6' at global address 406E or EEPROM address: 6e
$3C4A $406E ! $4949 $4070 ! $30 $4072 C! 

\ ASCII 55 : '7' at global address 4073 or EEPROM address: 73
$0171 $4073 ! $0905 $4075 ! $03 $4077 C! 

\ ASCII 56 : '8' at global address 4078 or EEPROM address: 78
$3649 $4078 ! $4949 $407A ! $36 $407C C! 

\ ASCII 57 : '9' at global address 407D or EEPROM address: 7d
$0649 $407D ! $4929 $407F ! $1E $4081 C! 

\ ASCII 58 : ':' at global address 4082 or EEPROM address: 82
$0036 $4082 ! $3600 $4084 ! $00 $4086 C! 

\ ASCII 59 : ';' at global address 4087 or EEPROM address: 87
$0040 $4087 ! $3400 $4089 ! $00 $408B C! 

\ ASCII 60 : '<' at global address 408C or EEPROM address: 8c
$0814 $408C ! $2241 $408E ! $00 $4090 C! 

\ ASCII 61 : '=' at global address 4091 or EEPROM address: 91
$1414 $4091 ! $1414 $4093 ! $14 $4095 C! 

\ ASCII 62 : '>' at global address 4096 or EEPROM address: 96
$0041 $4096 ! $2214 $4098 ! $08 $409A C! 

\ ASCII 63 : '?' at global address 409B or EEPROM address: 9b
$0201 $409B ! $7109 $409D ! $06 $409F C! 

\ ASCII 64 : '@' at global address 40A0 or EEPROM address: a0
$3E41 $40A0 ! $5D55 $40A2 ! $5E $40A4 C! 

\ ASCII 65 : 'A' at global address 40A5 or EEPROM address: a5
$7E09 $40A5 ! $0909 $40A7 ! $7E $40A9 C! 

\ ASCII 66 : 'B' at global address 40AA or EEPROM address: aa
$7F49 $40AA ! $4949 $40AC ! $36 $40AE C! 

\ ASCII 67 : 'C' at global address 40AF or EEPROM address: af
$3E41 $40AF ! $4141 $40B1 ! $22 $40B3 C! 

\ ASCII 68 : 'D' at global address 40B4 or EEPROM address: b4
$417F $40B4 ! $4141 $40B6 ! $3E $40B8 C! 

\ ASCII 69 : 'E' at global address 40B9 or EEPROM address: b9
$7F49 $40B9 ! $4949 $40BB ! $41 $40BD C! 

\ ASCII 70 : 'F' at global address 40BE or EEPROM address: be
$7F09 $40BE ! $0909 $40C0 ! $01 $40C2 C! 

\ ASCII 71 : 'G' at global address 40C3 or EEPROM address: c3
$3E41 $40C3 ! $4151 $40C5 ! $72 $40C7 C! 

\ ASCII 72 : 'H' at global address 40C8 or EEPROM address: c8
$7F08 $40C8 ! $0808 $40CA ! $7F $40CC C! 

\ ASCII 73 : 'I' at global address 40CD or EEPROM address: cd
$0041 $40CD ! $7F41 $40CF ! $00 $40D1 C! 

\ ASCII 74 : 'J' at global address 40D2 or EEPROM address: d2
$2040 $40D2 ! $413F $40D4 ! $01 $40D6 C! 

\ ASCII 75 : 'K' at global address 40D7 or EEPROM address: d7
$7F08 $40D7 ! $1422 $40D9 ! $41 $40DB C! 

\ ASCII 76 : 'L' at global address 40DC or EEPROM address: dc
$7F40 $40DC ! $4040 $40DE ! $40 $40E0 C! 

\ ASCII 77 : 'M' at global address 40E1 or EEPROM address: e1
$7F02 $40E1 ! $0C02 $40E3 ! $7F $40E5 C! 

\ ASCII 78 : 'N' at global address 40E6 or EEPROM address: e6
$7F04 $40E6 ! $0810 $40E8 ! $7F $40EA C! 

\ ASCII 79 : 'O' at global address 40EB or EEPROM address: eb
$3E41 $40EB ! $4141 $40ED ! $3E $40EF C! 

\ ASCII 80 : 'P' at global address 40F0 or EEPROM address: f0
$7F09 $40F0 ! $0909 $40F2 ! $06 $40F4 C! 

\ ASCII 81 : 'Q' at global address 40F5 or EEPROM address: f5
$3E41 $40F5 ! $5121 $40F7 ! $5E $40F9 C! 

\ ASCII 82 : 'R' at global address 40FA or EEPROM address: fa
$7F09 $40FA ! $1929 $40FC ! $46 $40FE C! 

\ ASCII 83 : 'S' at global address 40FF or EEPROM address: ff
$4649 $40FF ! $4949 $4101 ! $31 $4103 C! 

\ ASCII 84 : 'T' at global address 4104 or EEPROM address: 104
$0101 $4104 ! $7F01 $4106 ! $01 $4108 C! 

\ ASCII 85 : 'U' at global address 4109 or EEPROM address: 109
$3F40 $4109 ! $4040 $410B ! $3F $410D C! 

\ ASCII 86 : 'V' at global address 410E or EEPROM address: 10e
$1F20 $410E ! $4020 $4110 ! $1F $4112 C! 

\ ASCII 87 : 'W' at global address 4113 or EEPROM address: 113
$7F20 $4113 ! $1820 $4115 ! $7F $4117 C! 

\ ASCII 88 : 'X' at global address 4118 or EEPROM address: 118
$6314 $4118 ! $0814 $411A ! $63 $411C C! 

\ ASCII 89 : 'Y' at global address 411D or EEPROM address: 11d
$0708 $411D ! $7008 $411F ! $07 $4121 C! 

\ ASCII 90 : 'Z' at global address 4122 or EEPROM address: 122
$6151 $4122 ! $4945 $4124 ! $43 $4126 C! 

\ ASCII 91 : '[' at global address 4127 or EEPROM address: 127
$007F $4127 ! $4141 $4129 ! $00 $412B C! 

\ ASCII 92 : '\' at global address 412C or EEPROM address: 12c
$0204 $412C ! $0810 $412E ! $20 $4130 C! 

\ ASCII 93 : ']' at global address 4131 or EEPROM address: 131
$0041 $4131 ! $417F $4133 ! $00 $4135 C! 

\ ASCII 94 : '^' at global address 4136 or EEPROM address: 136
$6018 $4136 ! $0718 $4138 ! $60 $413A C! 

\ ASCII 95 : '_' at global address 413B or EEPROM address: 13b
$2020 $413B ! $2020 $413D ! $20 $413F C! 

\ ASCII 96 : '`' at global address 4140 or EEPROM address: 140
$0002 $4140 ! $0408 $4142 ! $00 $4144 C! 

\ ASCII 97 : 'a' at global address 4145 or EEPROM address: 145
$3844 $4145 ! $4448 $4147 ! $3C $4149 C! 

\ ASCII 98 : 'b' at global address 414A or EEPROM address: 14a
$7F48 $414A ! $4444 $414C ! $38 $414E C! 

\ ASCII 99 : 'c' at global address 414F or EEPROM address: 14f
$3844 $414F ! $4444 $4151 ! $28 $4153 C! 

\ ASCII 100 : 'd' at global address 4154 or EEPROM address: 154
$3844 $4154 ! $4448 $4156 ! $3F $4158 C! 

\ ASCII 101 : 'e' at global address 4159 or EEPROM address: 159
$3854 $4159 ! $545C $415B ! $18 $415D C! 

\ ASCII 102 : 'f' at global address 415E or EEPROM address: 15e
$0004 $415E ! $7E05 $4160 ! $00 $4162 C! 

\ ASCII 103 : 'g' at global address 4163 or EEPROM address: 163
$18A4 $4163 ! $A4A8 $4165 ! $7C $4167 C! 

\ ASCII 104 : 'h' at global address 4168 or EEPROM address: 168
$7F08 $4168 ! $0404 $416A ! $78 $416C C! 

\ ASCII 105 : 'i' at global address 416D or EEPROM address: 16d
$0000 $416D ! $3D40 $416F ! $00 $4171 C! 

\ ASCII 106 : 'j' at global address 4172 or EEPROM address: 172
$0080 $4172 ! $7D00 $4174 ! $00 $4176 C! 

\ ASCII 107 : 'k' at global address 4177 or EEPROM address: 177
$7F10 $4177 ! $1824 $4179 ! $40 $417B C! 

\ ASCII 108 : 'l' at global address 417C or EEPROM address: 17c
$0000 $417C ! $3F40 $417E ! $00 $4180 C! 

\ ASCII 109 : 'm' at global address 4181 or EEPROM address: 181
$7C04 $4181 ! $7804 $4183 ! $78 $4185 C! 

\ ASCII 110 : 'n' at global address 4186 or EEPROM address: 186
$7C08 $4186 ! $0404 $4188 ! $78 $418A C! 

\ ASCII 111 : 'o' at global address 418B or EEPROM address: 18b
$3844 $418B ! $4444 $418D ! $38 $418F C! 

\ ASCII 112 : 'p' at global address 4190 or EEPROM address: 190
$FC28 $4190 ! $2424 $4192 ! $18 $4194 C! 

\ ASCII 113 : 'q' at global address 4195 or EEPROM address: 195
$1824 $4195 ! $2428 $4197 ! $FC $4199 C! 

\ ASCII 114 : 'r' at global address 419A or EEPROM address: 19a
$7C08 $419A ! $0404 $419C ! $08 $419E C! 

\ ASCII 115 : 's' at global address 419F or EEPROM address: 19f
$4854 $419F ! $5454 $41A1 ! $20 $41A3 C! 

\ ASCII 116 : 't' at global address 41A4 or EEPROM address: 1a4
$0004 $41A4 ! $3F44 $41A6 ! $00 $41A8 C! 

\ ASCII 117 : 'u' at global address 41A9 or EEPROM address: 1a9
$3C40 $41A9 ! $4020 $41AB ! $7C $41AD C! 

\ ASCII 118 : 'v' at global address 41AE or EEPROM address: 1ae
$1C20 $41AE ! $4020 $41B0 ! $1C $41B2 C! 

\ ASCII 119 : 'w' at global address 41B3 or EEPROM address: 1b3
$3C40 $41B3 ! $3C40 $41B5 ! $3C $41B7 C! 

\ ASCII 120 : 'x' at global address 41B8 or EEPROM address: 1b8
$4428 $41B8 ! $1028 $41BA ! $44 $41BC C! 

\ ASCII 121 : 'y' at global address 41BD or EEPROM address: 1bd
$1CA0 $41BD ! $A0A0 $41BF ! $7C $41C1 C! 

\ ASCII 122 : 'z' at global address 41C2 or EEPROM address: 1c2
$4464 $41C2 ! $544C $41C4 ! $44 $41C6 C! 

\ ASCII 123 : '{' at global address 41C7 or EEPROM address: 1c7
$0008 $41C7 ! $3641 $41C9 ! $00 $41CB C! 

\ ASCII 124 : '|' at global address 41CC or EEPROM address: 1cc
$0000 $41CC ! $7F00 $41CE ! $00 $41D0 C! 

\ ASCII 125 : '}' at global address 41D1 or EEPROM address: 1d1
$0041 $41D1 ! $3608 $41D3 ! $00 $41D5 C! 

\ ASCII 126 : '~' at global address 41D6 or EEPROM address: 1d6
$1008 $41D6 ! $0810 $41D8 ! $10 $41DA C! 


LOCK

