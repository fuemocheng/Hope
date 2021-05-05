
@echo off

echo CHECK OUT
p4 edit -c default ..\..\Client\Assets\AssetBundles\Luas\ConfigTables\...
p4 edit -c default ..\..\Client\Assets\AssetBundles\Luas\LuaFrame\LuaTables.lua
p4 edit -c default ..\..\Client\Assets\Scripts\Hotfix\Config\Config.cs
p4 edit -c default ..\..\Client\Assets\Scripts\Hotfix\Config\Config.cs.meta
p4 edit -c default ..\..\Client\Assets\AssetBundles\Config\Config.bytes
p4 edit -c default ..\..\Client\Assets\AssetBundles\Config\Config.bytes.meta
echo CHECK OUT OK

echo CHARP STARTING

~tabtoy-cli.exe ^
--mode=v3 ^
--package=main ^
--csharp_out=.\out\csharp\Config.cs ^
--binary_out=.\out\csharp\Config.bytes ^
--luaenumintvalue=true ^
--lan=zh_cn ^
--tag_action="nogentab:server|nogenfield_csharp:server|nogenfield_binary:server" ^
-index=_Index.xlsx

set b=Assets\AssetBundles\Config
set c=Assets\Scripts\Hotfix\Config
set a=..\\..\\Client

if exist %a% (
	echo hello...
	echo test..%b%
	copy out\\csharp\\Config.bytes %a%\\%b%
	echo copy to %a%\\%b% ...
	copy out\\csharp\\Config.cs %a%\\%c%
	echo copy to %a%\\%c% ...
)

@IF %ERRORLEVEL% NEQ 0 pause

echo CHARP END

echo LUA STARTING

~tabtoy_3.1.0.exe ^
-mode=v3 ^
-package=main ^
-index=_Index.xlsx ^
-lua_dir=.\out\lua\ ^
-lua_out=.\out\lua\Config.lua

del out\\lua\\Config.lua
echo É¾³ýConfig.lua

del out\\lua\\DropConfig.lua
echo É¾³ýDropConfig.lua

del out\\lua\\DropDataConfig.lua
echo É¾³ýDropDataConfig.lua

del out\\lua\\DropPoolConfig.lua
echo É¾³ýDropPoolConfig.lua

set a=..\\..\\Client\\Assets\AssetBundles\Luas\ConfigTables

if exist %a% (
	for %%b in (out\\lua\\*.lua) do (
		copy %%b %a%\\
		echo copy %%b to %a%\\ ...
	)
)
LuaTableOpt.exe -luaDir %a%
@IF %ERRORLEVEL% NEQ 0 pause

echo LUA END
pause