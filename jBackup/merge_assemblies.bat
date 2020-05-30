echo Started DmpPostBuildEvent processing.

REM =================================================================
REM Developer Customization Required!
REM Change SUBASSEMBLYLIST to match the list of non-Windows references 
REM for your assembly. You must use the complete path, and use a space 
REM between each assembly listed. Use the value NONE when there are no 
REM sub-assemblies to be merged.
REM =================================================================


set SUBASSEMBLYLIST=.\jBackupAPI.dll

REM =================================================================
echo Sub-Assemblies: %SUBASSEMBLYLIST%

REM Get parameters: $(TargetFileName) $(TargetName) $(TargetExt) $(ConfigurationName)
set ASSEMBLYFILE=%1
set ASSEMBLYNAME=%2
set ASSEBMLYTYPE=%3
set BUILDMODE=%4
echo Parameters: %ASSEMBLYFILE% %ASSEMBLYNAME% %ASSEBMLYTYPE% %BUILDMODE%



REM do not run in debug mode
if "%BUILDMODE%"=="Debug" goto SnDone

if "%ASSEBMLYTYPE%"==".exe" goto UsingEXE
if "%ASSEBMLYTYPE%"==".EXE" goto UsingEXE
if "%ASSEBMLYTYPE%"==".dll" goto UsingDLL
if "%ASSEBMLYTYPE%"==".DLL" goto UsingDLL

:UsingDLL
set ASSEMBLYEXT=dll
goto UsingExtDone

:UsingEXE
set ASSEMBLYEXT=exe
goto UsingExtDone

:UsingExtDone

echo Deleting previous build ilmerge result files.
if exist ILMergeResults.txt del ILMergeResults.txt

echo Deleting previous build non-merged assembly file.
if exist %ASSEMBLYNAME%_nonmerged.%ASSEMBLYEXT% del %ASSEMBLYNAME%_nonmerged.%ASSEMBLYEXT%
if exist %ASSEMBLYNAME%_nonmerged.pdb del %ASSEMBLYNAME%_nonmerged.pdb

if "%SUBASSEMBLYLIST%"=="NONE" goto SkipILMerge
if "%SUBASSEMBLYLIST%"=="none" goto SkipILMerge

echo Renaming the current assembly to a temp name for merging.
ren %ASSEMBLYFILE% %ASSEMBLYNAME%_nonmerged.%ASSEMBLYEXT%
if exist %ASSEMBLYNAME%.pdb ren %ASSEMBLYNAME%.pdb %ASSEMBLYNAME%_nonmerged.pdb

echo Running ILMERGE to combine assemblies.
if "%BUILDMODE%"=="Debug" goto ILMergeDebug
if "%BUILDMODE%"=="Release" goto ILMergeRelease

:ILMergeRelease
start /wait c:\tools\ilmerge "/targetplatform:v4,C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1" /ndebug /internalize %ASSEMBLYNAME%_nonmerged.%ASSEMBLYEXT% %SUBASSEMBLYLIST% /out:%ASSEMBLYFILE% /log:ILMergeResults.txt
goto ILMergeDone

:ILMergeDebug
start /wait c:\tools\ilmerge "/targetplatform:v4,C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1" /ndebug /internalize %ASSEMBLYNAME%_nonmerged.%ASSEMBLYEXT% %SUBASSEMBLYLIST% /out:%ASSEMBLYFILE% /log:ILMergeResults.txt
goto ILMergeDone

:ILMergeDone

:SkipILMerge

echo Running SN to sign the combined assembly.
if "%BUILDMODE%"=="Debug" goto SnDebug
if "%BUILDMODE%"=="Release" goto SnRelease

:SnDebug
:start /wait c:\tools\sn.exe -Vr %ASSEMBLYFILE%
goto SnDone

:SnRelease
:start /wait C:\tools\sn.exe -R %ASSEMBLYFILE% ..\..\..\keys\publicprivatekeypair.snk
goto SnDone

:SnDone

echo Completed DmpPostBuildEvent processing.
