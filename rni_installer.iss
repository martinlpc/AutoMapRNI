[Setup]
#define AppVersion "1_14_1"
AppName="AutoMap RNI"
AppVersion=1.14.1
DefaultDirName="{pf}\ENACOM\AutoMap RNI"
DefaultGroupName=ENACOM
PrivilegesRequired=admin
ArchitecturesAllowed=x64 x86
OutputBaseFilename=AutoMapRNI_v{#AppVersion}

[Files]
; Archivos que van en la carpeta Windows
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\bin\Debug\e300amrni.txt"; DestDir: "{win}"; Flags: ignoreversion
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\bin\Debug\n550spamrni.txt"; DestDir: "{win}"; Flags: ignoreversion
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\bin\Debug\n550amrni.txt"; DestDir: "{win}"; Flags: ignoreversion

; Subcarpetas de \AutoMapRNI
; \Drivers\*
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\Garmin USB Driver\*"; DestDir: "{app}\Drivers\Garmin USB Driver"; Flags: ignoreversion recursesubdirs
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\GlobalSat USB Driver\*"; DestDir: "{app}\Drivers\GlobalSat USB Driver"; Flags: ignoreversion recursesubdirs
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\NARDA USB Driver\*"; DestDir: "{app}\Drivers\NARDA USB Driver";Flags: ignoreversion recursesubdirs
; \files
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\Resources\_Escala.bmp"; DestDir: "{app}\files";Flags: ignoreversion
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\bin\Debug\files\Escala.PNG"; DestDir: "{app}\files";Flags: ignoreversion
; \rep
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\Resources\Modelo_averif.xlsx"; DestDir: "{app}\rep"; Flags: ignoreversion
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\Resources\Modelo_reporte.xlsx"; DestDir: "{app}\rep"; Flags: ignoreversion
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\Resources\Modelo_reporte_P.xlsx"; DestDir: "{app}\rep"; Flags: ignoreversion
; Archivos en carpeta {app}
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\bin\Release\AutoMap RNI.exe"; DestDir: {app}; Flags: ignoreversion
Source: "C:\Program Files (x86)\Reference Assemblies\Microsoft\VBPowerPacks\v10.0\Microsoft.VisualBasic.PowerPacks.Vs.dll"; DestDir: {app}
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\MD.Equipos.GPS.dll"; DestDir: {app}
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\bin\Release\Ionic.Zip.dll"; DestDir: {app}
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\bin\Release\GMap.NET.WindowsForms.dll"; DestDir: {app}
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\bin\Release\GMap.NET.Core.dll"; DestDir: {app}
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\bin\Release\AutoMap RNI.exe.config"; DestDir: {app}
Source: "C:\Users\User\Desktop\Proyectos\AutoMapRNI\Rastreador GPS+Maps\Resources\ENACOM_sintexto_final.ico"; DestDir: {app}; Flags: ignoreversion

[Icons]
; Acc dir en escritorio
Name: "{commondesktop}\AutoMap RNI v{#AppVersion}"; Filename: "{app}\AutoMap RNI.exe"; IconFilename: "{app}\ENACOM_sintexto_final.ico"

;Acc dir en menu inicio
Name: "{group}\AutoMap RNI v{#AppVersion}"; Filename: "{app}\AutoMap RNI.exe"; IconFilename: "{app}\ENACOM_sintexto_final.ico"
