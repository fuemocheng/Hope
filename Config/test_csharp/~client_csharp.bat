
echo CSHARP STARTING
tabtoy.exe ^
-mode=v3 ^
-package=main ^
-index=Index.xlsx ^
--csharp_out=.\out\csharp\Config.cs ^
--binary_out=.\out\csharp\Config.bytes ^
--luaenumintvalue=true ^
--lan=zh_cn

@--tag_action="nogentab:server|nogenfield_csharp:server|nogenfield_binary:server"

echo CSHARP END
pause