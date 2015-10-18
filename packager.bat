md ucspackages\%1
md ucspackages\%1\gamefiles
md ucspackages\%1\lib
md ucspackages\%1\tools
md ucspackages\%1\logs
xcopy "Ultrapowa Clash Server\bin\Release\gamefiles" ucspackages\%1\gamefiles /s /e /I
rem copy "Ultrapowa Clash Server\bin\Release\gamefiles\fingerprint.json" ucspackages\%1\gamefiles\fingerprint.json.backup
xcopy "Ultrapowa Clash Server\bin\Release\x64" ucspackages\%1\lib\x64 /s /e /I
xcopy "Ultrapowa Clash Server\bin\Release\x86" ucspackages\%1\lib\x86 /s /e /I
xcopy "Ultrapowa Clash Server\bin\Release\*.dll" ucspackages\%1\lib
xcopy "Ultrapowa Clash Server\bin\Release\ucs.exe" ucspackages\%1
xcopy "Ultrapowa Clash Server\bin\Release\ucs.exe.config" ucspackages\%1
xcopy "Ultrapowa Clash Server\bin\Release\ucsconf.config" ucspackages\%1
xcopy ..\ucsbuildsha\ucsbuildsha\bin\Release\ucsbuildsha.exe ucspackages\%1\tools
xcopy ..\ucsbuildsha\ucsbuildsha\bin\Release\gen_patch.bat ucspackages\%1\tools
xcopy ..\ucsbuildsha\ucsbuildsha\bin\Release\ucsgflzma.exe ucspackages\%1\tools
xcopy ucsdb.sql ucspackages\%1\tools
xcopy ucsdb ucspackages\%1
xcopy readme.txt ucspackages\%1

