@echo off

if exist out (
	rd /s /Q out
)

md out

for %%i in (proto\\*.proto) do (
	echo export %%i
	bin\\protoc -I=proto --csharp_out=out %%i
)

set c=..\\..\\Test\Assets\Scripts\Proto
set s=..\\..\\Server\Server\GameProtocol

for %%b in (out\\*.cs) do (
	if exist %c% (
		copy %%b %c%
		echo copy %%b to %c% ...
	)
	if exist %s% (
		copy %%b %s%
		echo copy %%b to %s% ...
	)
)

echo GEN PROTO CSHARP FILE OK
pause