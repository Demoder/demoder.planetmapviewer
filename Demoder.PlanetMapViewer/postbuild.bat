cd /D %~1
mkdir ..\Rel
rem ############
rem # Installer
rem ############
mkdir ..\Rel\Installer
"..\Ext\Vha.Build\bin\%2\Vha.Build.exe" installer -nsisScript=Installer.nsi -assembly="bin\%2\Demoders PlanetMap Viewer.exe"
copy Readme.txt ..\Rel\Installer\
copy License.txt ..\Rel\Installer\

rem ##############
rem # Package
rem ###############
del /F /Q ..\Rel\Package
mkdir ..\Rel\Package
copy /Y bin\%2\*.dll ..\Rel\Package\
copy /Y "bin\%2\Demoders PlanetMap Viewer.exe" ..\Rel\Package\
copy /Y "bin\%2\Demoders PlanetMap Viewer.exe.config" ..\Rel\Package\
mkdir ..\Rel\Package\Content
xcopy /Y  bin\%2\Content ..\Rel\Package\Content /E
rem ######
rem # EasyHook
rem ######
copy /Y ..\Msc\dlls\*.dll ..\Rel\Package\
copy /Y ..\Msc\dlls\*.exe ..\Rel\Package\
rem #######
rem # Demoder.AoHookBridge
rem #######
copy /Y ..\Lib\Demoder.AoHookBridge\bin\%2\Demoder.AoHookBridge.dll ..\Rel\Package\
copy Readme.txt ..\Rel\Package\
copy License.txt ..\Rel\Package\
