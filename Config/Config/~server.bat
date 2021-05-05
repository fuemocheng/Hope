@echo off

echo update svn
echo TortoiseProc.exe /command:update /path:"." /closeonend:1

echo export configs json
~tabtoy_3.1.0.exe ^
--mode=v3 ^
--json_dir=.\out\go ^
--lan=zh_cn ^
--index=_Index.xlsx

echo copy json

if not exist ~server_path.txt (
	echo ~server_path�Ѵ������޸Ĵ��ļ����������ֱ�ӿ�����Ŀ���ļ�����
	echo ���뿽��Ŀ¼�滻���� >> ~server_path.txt
)

for /f %%a in ('type ~server_path.txt') do (
	echo path��%%a
	if exist %%a (
		xcopy .\out\go\ActionConfig.json %%a
		xcopy .\out\go\*   %%a   /y
	)
)

pause & exit