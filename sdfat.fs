\ SPI communication with SD FAT32

\ All sd variables Big Endian
variable offh    \ Start of Logical sectors,
variable offl    \  offset high and low 
\  physical addr = logical addr + offset
variable spc     \ Sectors per cluster
variable rsec    \ Reserved sectors
variable fatlh   \ FAT length: sectors/FAT
variable fatll
variable crds    \ cardsize/(512*1024 bits)
variable ssrdl   \ Start sector of root directory
variable ssrdh
variable nfcl    \ nexr free cluster, byte 492 of FS Info
variable nfch
variable lcll    \ last cluster
variable lclh

#require D+
#require D=
#require D>

: sdwt ( -- )   \ sd-wait
   begin $FF spi $FF = until
;


: sdc ( u -- )    \ sd copy to buffer
   begin $ff spi $fe = until
   0 do $FF SPI sdb I + c! loop
   spi> spi>
;

: sdr ( ul uh -- )  \ read 512 bytes from phys. sector address
   &17 rot EXG rot EXG sd-cmd drop  
   &512 sdc
;

: sdw ( ul uh -- )   \ write buffer to phys. sector address
   &24 rot EXG rot EXG sd-cmd drop
   $FE >spi  \ 
   &512 0 do  sdb i + c@ >spi  loop
   $FF dup >spi >spi
   sdwt
;


: gsdd ( -- )      \ Get SD Layout Data
   9 0 0  sd-cmd drop &17 sdc
   sdb 8 + c@ $100 *
   sdb 9 + c@ 1+ + crds !
   0 0 sdr sdb &454 + dup  \ Partition table 1 rel. sector
   @ EXG dup offl ! swap 
   2+ @ EXG dup offh !
   sdr sdb dup &14 + @ EXG rsec ! ( sdb)
   dup &13 + C@ spc !
   dup &36 + dup
   @ EXG  dup fatll ! 2* rsec @ + >R
   2+ @ EXG dup fatlh ! 2* R> swap
   offl @ offh @ D+ ssrdh ! ssrdl !  \ start sector root dir.
   offl @ 1+ offh @ sdr   \ FSinfo sector
   &492 + dup @ EXG nfcl !
   2+ @ EXG nfch !
   
;

: c2s ( clsl clsh -- adrl adrh)  \ Cluster# > Phys.Sector
   2dup 0 0 D= if
     2drop 2 0 then   \ root cluster is 2 0 i.s.o. 0 0
   spc @ dup rot * 
   rot 2- rot um* rot +
   ssrdl @ ssrdh @ D+ 
;

: dmpr ( -- )   \ Dump first sector of root dir.
   gsdd ssrdl @ ssrdh @ sdr sdb &511 dump
;

\ Get FAT sector and index# from cluster#, 128 fat-rcrds/sector
: fats ( clnl clnh  -- i fatsl fatsh)
   2dup 0 0 D= if
      2drop 2 rsec @ offl @ + offh @ 
   else
      &128 UM/MOD ( r q ) 0 swap ( r 0 q )
      rsec @ + swap
      offl @ offh @ D+ ( r fatsl fatsh )
   then
;


\ Get FAT record plus index# from buffer
: fatr ( i fatsl fatsh  -- fatrl fatrh)
   sdr 
   4 * sdb + dup @ EXG 
   swap 2 + @ EXG
;

\ Write FAT record in buffer
: fatw ( i fatrl fatrh -- )  \ 0 < i < 127
   exg swap exg  swap rot 4 * sdb + 2!
;

\ dump cluster (4096 bytes)
: cdmp ( clustl clusth -- )
   c2s 4 0 do 2dup i 0 D+ sdr
   >R >R sdb $1ff dump R> R> loop 2drop
;

: gcl ( -- clnl clnh)  \ Get cluster nummer from buffer
      sdb dup
      &26 + @ exg dup lcll ! swap
      &20 + @ exg dup lclh !
;

: clc ( scl sch -- i)  \ file cluster count, max. 65535
   0 >R 2dup begin
      R> 1+ >R fats fatr
      2DUP $FFF7 $FF D>
      until
   2drop c2s sdr R> \ restore buffer
;

: ls
   gcl 2dup clc 0 do  \ number of clusters of dir.
     c2s spc @ 0 do  \ cycle through sectors of cluster
       2dup sdr sdb  ( d-sr ba)
       &16 0 do  \ cycle through dir. entries of sector
         dup 2dup ( d-sr ba ba ba ba)
         c@ dup 0 = if    ( d-sr ba ba ba b0)
           2drop drop leave ( d-sr ba)
         then 
         $e5 = if ( d-sr ba ba ba) \ deleted dir. entry
           2drop
         else
           &11 + c@ dup ( d-sr ba ba b11 b11)
           $F = not if  \ Long file name entry ( d-sr ba ba b11)
             $20 = if ( d-sr ba ba)
               cr &11 type 2 spaces ." f" 
             else
               cr &11 type 2 spaces ." d"
             then
           else
             2drop  ( d-sr ba)
           then
         then
         $20 +
       loop 
       drop 1 0  D+ ( d-sr)
     loop
     2drop
     lcll @ lclh @ 
     fats fatr 2dup lclh ! lcll ! ( d-cl)
   loop 2drop
;

ram

