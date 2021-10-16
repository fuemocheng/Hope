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

for %%b in (out\\*.cs) do (
	if exist %c% (
		copy %%b %c%
		echo copy %%b to %c% ...
	)
)

echo GEN PROTO CSHARP FILE OK
pause