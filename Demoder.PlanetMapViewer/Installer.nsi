!include "MUI2.nsh"
; A little bit of useful information
Name "Demoder.PlanetMapViewer ${ASSEMBLY_VERSION}"
VIAddVersionKey "ProductName" "Demoder.PlanetMapViewer ${ASSEMBLY_VERSION} Installer"
VIAddVersionKey "CompanyName" "${ASSEMBLY_COMPANY}"
VIAddVersionKey "LegalCopyright" "${ASSEMBLY_COPYRIGHT}"
VIAddVersionKey "FileDescription" "${ASSEMBLY_DESCRIPTION}"
VIAddVersionKey "FileVersion" "${ASSEMBLY_VERSION}"
VIProductVersion ${ASSEMBLY_VERSION}

; The file to write
OutFile "..\Rel\Installer\Setup.exe"

; Request application privileges for Windows Vista
RequestExecutionLevel admin


InstallDir "$PROGRAMFILES\Demoders PlanetMap Viewer"

; The default installation directory
;ReadRegString $0 HKLM "SOFTWARE\Demoder\PlanetMapViewer" "InstallDir"
;StrCmp $0 "" SetDefaultInstallDir SetExistingInstallDir
;    SetDefaultInstallDir:
;        InstallDir "$PROGRAMFILES\Demoders PlanetMap Viewer"
;    SetExistingInstallDir:
;        InstallDir $0

;--------------------------------
; Pages

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------
; The stuff to install

Section "Prequesites (required)"
    SectionIn RO
    SetOutPath $INSTDIR\Redist  
  
  ; .NET 4.0 Client Profile
  ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client\" "Install"
  IntCmp $0 1 +4 ;If this value is 1, skip executing
    File "..\Dep\dotNetFx40_Client_setup.exe"
    ExecWait '"$INSTDIR\Redist\dotNetFx40_Client_setup.exe" /passive /promptrestart'
  
  ; .NET 3.5
  ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5\" "Install"
  IntCmp $0 1 +3
    File "..\Dep\dotNetFx35setup.exe"
    ExecWait '"$INSTDIR\Redist\dotNetFx35setup.exe" /passive /promptrestart'
  
  ; XNA Framework
  ReadRegDWORD $0 HKLM "SOFTWARE\Wow6432Node\Microsoft\XNA\Framework\v4.0\" "Installed" ; AMD64 Systems
  IntCmp $0 1 +5
   ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\XNA\Framework\v4.0\" "Installed" ; i386 Systems
  IntCmp $0 1 +3
   File "..\Dep\xnafx40_redist.msi"
   ExecWait '"msiexec" /i "$INSTDIR\Redist\xnafx40_redist.msi"  /passive'

   File "..\Demoder.PmvInstaller.Tester\bin\Release\Demoder.PmvInstaller.Tester.exe"
   ExecWait '"$INSTDIR\Redist\Demoder.PmvInstaller.Tester.exe" "$INSTDIR"'

   RMDir /r "$INSTDIR\Redist"
  SetOutPath $INSTDIR

  
SectionEnd

Section "Demoder.PlanetMapViewer (required)"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Add files
  File "bin\Release\Demoders PlanetMap Viewer.exe"
  File "bin\Release\*.dll"
  File "bin\Release\*.txt"
  ;File "bin\Release\Debug.bat"
  File "..\lib\Demoder.AoHookBridge\bin\Release\*.dll"
  ; EasyHook
  File "..\Msc\dlls\*.dll"
  File "..\Msc\dlls\*.exe"

  ; Fonts
  SetOutPath $INSTDIR\Content\Fonts
  File "bin\Release\Content\Fonts\*"
  ; Textures
  SetOutPath $INSTDIR\Content\Textures
  File "bin\Release\Content\Textures\*"

  ; Write the installation path into the registry
  WriteRegStr HKLM "SOFTWARE\Demoder\PlanetMapViewer" "InstallDir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Demoder.PlanetMapViewer" "DisplayName" "Demoder.PlanetMapViewer ${ASSEMBLY_VERSION}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Demoder.PlanetMapViewer" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Demoder.PlanetMapViewer" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Demoder.PlanetMapViewer" "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  SetOutPath $INSTDIR
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"
  CreateDirectory "$SMPROGRAMS\Demoder.PlanetMapViewer"
  CreateShortCut "$SMPROGRAMS\Demoder.PlanetMapViewer\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\Demoder.PlanetMapViewer\Demoders PlanetMap Viewer.lnk" "$INSTDIR\Demoders PlanetMap Viewer.exe" "" "$INSTDIR\Demoders PlanetMap Viewer.exe" 0
SectionEnd

Section "Desktop Shortcut"
  CreateShortCut "$DESKTOP\Demoders PlanetMap Viewer.lnk" "$INSTDIR\Demoders PlanetMap Viewer.exe" "" "$INSTDIR\Demoders PlanetMap Viewer.exe" 0
SectionEnd

Section "Launch Application"
    SetOutPath $INSTDIR
    Exec "$INSTDIR\Demoders PlanetMap Viewer.exe"
SectionEnd

;--------------------------------
; Uninstaller

Section "Uninstall"
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Demoder.PlanetMapViewer"
  DeleteRegKey HKLM "SOFTWARE\Demoder\PlanetMapViewer"

  ; Remove desktop shortcut
  Delete "$DESKTOP\Demoders PlanetMap Viewer.lnk"
  
  ; Remove directories used
  RMDir /r "$SMPROGRAMS\Demoder.PlanetMapViewer"
  RMDir /r "$INSTDIR"
SectionEnd
