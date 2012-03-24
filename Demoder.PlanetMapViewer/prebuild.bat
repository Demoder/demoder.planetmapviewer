cd /D %~1
"..\Ext\Vha.Build\bin\%2\Vha.Build.exe" template -inputFile="Properties\AssemblyInfo.Template.cs" -outputFile="Properties\AssemblyInfo.cs"