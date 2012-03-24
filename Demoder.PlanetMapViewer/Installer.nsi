; A little bit of useful information
Name "Demoder.PlanetMapViewer ${ASSEMBLY_VERSION}"
VIAddVersionKey "ProductName" "Demoder.PlanetMapViewer ${ASSEMBLY_VERSION} Installer"
VIAddVersionKey "CompanyName" "${ASSEMBLY_COMPANY}"
VIAddVersionKey "LegalCopyright" "${ASSEMBLY_COPYRIGHT}"
VIAddVersionKey "FileDescription" "${ASSEMBLY_DESCRIPTION}"
VIAddVersionKey "FileVersion" "${ASSEMBLY_VERSION}"
VIProductVersion ${ASSEMBLY_VERSION}

; The file to write
OutFile "bin\Release\Setup.exe"

; The default installation directory
InstallDir "$PROGRAMFILES\Demoder\PlanetMapViewer"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

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
    File "..\Dependency\*"
    
  
  ; .NET 4.0 full
  ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\" "Install"
  IntCmp $0 1 +4 ;If this value is 1, skip executing
    File "..\Dependency\dotNetFx40_Full_setup.exe"
    ExecWait '"$INSTDIR\Redist\dotNetFx40_Full_setup.exe" /passive /promptrestart'
  
  ; .NET 3.5
  ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5\" "Install"
  IntCmp $0 1 +3
    File "..\Dependency\dotNetFx35setup.exe"
    ExecWait '"$INSTDIR\Redist\dotNetFx35setup.exe" /passive /promptrestart'
  
  ; XNA Framework
  ReadRegDWORD $0 HKLM "SOFTWARE\Wow6432Node\Microsoft\XNA\Framework\v4.0\" "Installed" ; AMD64 Systems
  IntCmp $0 1 +5
   ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\XNA\Framework\v4.0\" "Installed" ; i386 Systems
  IntCmp $0 1 +3
   File "..\Dependency\xnafx40_redist.msi"
   ExecWait '"msiexec" /i "$INSTDIR\Redist\xnafx40_redist.msi"  /passive'

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
  File "..\Demoder.AoHookBridge\bin\Release\*.dll"
  File "..\dlls\*"

  ; Set output path to the directories we're copying
  SetOutPath $INSTDIR\Content\Fonts
  File "bin\Release\Content\\Fonts\*"
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