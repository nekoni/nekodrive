# Microsoft Developer Studio Generated NMAKE File, Format Version 4.20
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) External Target" 0x0106

!IF "$(CFG)" == ""
CFG=rpcgen - Win32 Debug
!MESSAGE No configuration specified.  Defaulting to rpcgen - Win32 Debug.
!ENDIF 

!IF "$(CFG)" != "rpcgen - Win32 Release" && "$(CFG)" != "rpcgen - Win32 Debug"
!MESSAGE Invalid configuration "$(CFG)" specified.
!MESSAGE You can specify a configuration when running NMAKE on this makefile
!MESSAGE by defining the macro CFG on the command line.  For example:
!MESSAGE 
!MESSAGE NMAKE /f "rpcgen.mak" CFG="rpcgen - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "rpcgen - Win32 Release" (based on "Win32 (x86) External Target")
!MESSAGE "rpcgen - Win32 Debug" (based on "Win32 (x86) External Target")
!MESSAGE 
!ERROR An invalid configuration is specified.
!ENDIF 

!IF "$(OS)" == "Windows_NT"
NULL=
!ELSE 
NULL=nul
!ENDIF 
################################################################################
# Begin Project
# PROP Target_Last_Scanned "rpcgen - Win32 Debug"

!IF  "$(CFG)" == "rpcgen - Win32 Release"

# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Release"
# PROP BASE Intermediate_Dir "Release"
# PROP BASE Target_Dir ""
# PROP BASE Cmd_Line "NMAKE /f rpcgen.mak"
# PROP BASE Rebuild_Opt "/a"
# PROP BASE Target_File "rpcgen.exe"
# PROP BASE Bsc_Name "rpcgen.bsc"
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Release"
# PROP Intermediate_Dir "Release"
# PROP Target_Dir ""
# PROP Cmd_Line "NMAKE /f Makefile"
# PROP Rebuild_Opt "/a"
# PROP Target_File "rpcgen.exe"
# PROP Bsc_Name "rpcgen.bsc"
OUTDIR=.\Release
INTDIR=.\Release

ALL : 

CLEAN : 
	-@erase 

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

!ELSEIF  "$(CFG)" == "rpcgen - Win32 Debug"

# PROP BASE Use_Debug_Libraries 1
# PROP BASE Output_Dir "Debug"
# PROP BASE Intermediate_Dir "Debug"
# PROP BASE Target_Dir ""
# PROP BASE Cmd_Line "NMAKE /f rpcgen.mak"
# PROP BASE Rebuild_Opt "/a"
# PROP BASE Target_File "rpcgen.exe"
# PROP BASE Bsc_Name "rpcgen.bsc"
# PROP Use_Debug_Libraries 1
# PROP Output_Dir "Debug"
# PROP Intermediate_Dir "Debug"
# PROP Target_Dir ""
# PROP Cmd_Line "NMAKE /f Makefile clean"
# PROP Rebuild_Opt "/a"
# PROP Target_File "rpcgen.exe"
# PROP Bsc_Name "rpcgen.bsc"
OUTDIR=.\Debug
INTDIR=.\Debug

ALL : 

CLEAN : 
	-@erase 

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

!ENDIF 

################################################################################
# Begin Target

# Name "rpcgen - Win32 Release"
# Name "rpcgen - Win32 Debug"

!IF  "$(CFG)" == "rpcgen - Win32 Release"

".\rpcgen.exe" : 
   CD C:\users\klotz\oncrpc\W32\rpcgen
   NMAKE /f Makefile

!ELSEIF  "$(CFG)" == "rpcgen - Win32 Debug"

".\rpcgen.exe" : 
   CD C:\users\klotz\oncrpc\W32\rpcgen
   NMAKE /f Makefile clean

!ENDIF 

################################################################################
# Begin Source File

SOURCE=.\MAKEFILE

!IF  "$(CFG)" == "rpcgen - Win32 Release"

!ELSEIF  "$(CFG)" == "rpcgen - Win32 Debug"

!ENDIF 

# End Source File
# End Target
# End Project
################################################################################
