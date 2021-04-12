SET GOPATH=%~dp0../
echo %GOPATH%
cd tabtoy
go build -v -o ../tabtoy.exe
pause