# Microsoft Developer Studio Project File - Name="oncrpc_static" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Static Library" 0x0104

CFG=oncrpc_static - Win32 Debug
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "oncrpc_static.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "oncrpc_static.mak" CFG="oncrpc_static - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "oncrpc_static - Win32 Release" (based on "Win32 (x86) Static Library")
!MESSAGE "oncrpc_static - Win32 Debug" (based on "Win32 (x86) Static Library")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""
# PROP Scc_LocalPath ""
CPP=cl.exe
RSC=rc.exe

!IF  "$(CFG)" == "oncrpc_static - Win32 Release"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Release"
# PROP BASE Intermediate_Dir "Release"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Release"
# PROP Intermediate_Dir "Release"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_MBCS" /D "_LIB" /YX /FD /c
# ADD CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_MBCS" /D "_LIB" /YX /FD /c
# ADD BASE RSC /l 0x40c /d "NDEBUG"
# ADD RSC /l 0x40c /d "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LIB32=link.exe -lib
# ADD BASE LIB32 /nologo
# ADD LIB32 /nologo

!ELSEIF  "$(CFG)" == "oncrpc_static - Win32 Debug"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 1
# PROP BASE Output_Dir "Debug"
# PROP BASE Intermediate_Dir "Debug"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 1
# PROP Output_Dir "Debug"
# PROP Intermediate_Dir "Debug"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_MBCS" /D "_LIB" /YX /FD /GZ  /c
# ADD CPP /nologo /W3 /Gm /GX /ZI /Od /I "D:\oncrpc\win32\include" /D "_DEBUG" /D "WIN32" /D "_WINDOWS" /D "_X86_" /D "_NT" /YX /FD /GZ  /c
# ADD BASE RSC /l 0x40c /d "_DEBUG"
# ADD RSC /l 0x40c /d "_DEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LIB32=link.exe -lib
# ADD BASE LIB32 /nologo
# ADD LIB32 /nologo /out:"Debug\oncrpc.lib"

!ENDIF 

# Begin Target

# Name "oncrpc_static - Win32 Release"
# Name "oncrpc_static - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Source File

SOURCE=..\..\Auth_non.c
# End Source File
# Begin Source File

SOURCE=..\..\Auth_uni.c
# End Source File
# Begin Source File

SOURCE=..\..\authunix.c
# End Source File
# Begin Source File

SOURCE=..\..\bcopy.c
# End Source File
# Begin Source File

SOURCE=..\..\Bindresv.c
# End Source File
# Begin Source File

SOURCE=..\..\clnt_gen.c
# End Source File
# Begin Source File

SOURCE=..\..\Clnt_per.c
# End Source File
# Begin Source File

SOURCE=..\..\clnt_raw.c
# End Source File
# Begin Source File

SOURCE=..\..\clnt_sim.c
# End Source File
# Begin Source File

SOURCE=..\..\Clnt_tcp.c
# End Source File
# Begin Source File

SOURCE=..\..\Clnt_udp.c
# End Source File
# Begin Source File

SOURCE=..\..\Get_myad.c
# End Source File
# Begin Source File

SOURCE=..\..\Getrpcen.c
# End Source File
# Begin Source File

SOURCE=..\..\Getrpcpo.c
# End Source File
# Begin Source File

SOURCE=..\..\nt.c
# End Source File
# Begin Source File

SOURCE=..\..\Pmap_cln.c
# End Source File
# Begin Source File

SOURCE=..\..\pmap_get.c
# End Source File
# Begin Source File

SOURCE=..\..\pmap_gma.c
# End Source File
# Begin Source File

SOURCE=..\..\pmap_pr.c
# End Source File
# Begin Source File

SOURCE=..\..\pmap_pro.c
# End Source File
# Begin Source File

SOURCE=..\..\Pmap_rmt.c
# End Source File
# Begin Source File

SOURCE=..\..\portmap.c
# End Source File
# Begin Source File

SOURCE=..\..\rpc_call.c
# End Source File
# Begin Source File

SOURCE=..\..\rpc_comm.c
# End Source File
# Begin Source File

SOURCE=..\..\rpc_prot.c
# End Source File
# Begin Source File

SOURCE=..\..\Svc.c
# End Source File
# Begin Source File

SOURCE=..\..\svc_auth.c
# End Source File
# Begin Source File

SOURCE=..\..\Svc_autu.c
# End Source File
# Begin Source File

SOURCE=..\..\svc_raw.c
# End Source File
# Begin Source File

SOURCE=..\..\Svc_run.c
# End Source File
# Begin Source File

SOURCE=..\..\Svc_simp.c
# End Source File
# Begin Source File

SOURCE=..\..\Svc_tcp.c
# End Source File
# Begin Source File

SOURCE=..\..\Svc_udp.c
# End Source File
# Begin Source File

SOURCE=..\..\Xdr.c
# End Source File
# Begin Source File

SOURCE=..\..\Xdr_arra.c
# End Source File
# Begin Source File

SOURCE=..\..\xdr_floa.c
# End Source File
# Begin Source File

SOURCE=..\..\xdr_mem.c
# End Source File
# Begin Source File

SOURCE=..\..\Xdr_rec.c
# End Source File
# Begin Source File

SOURCE=..\..\Xdr_refe.c
# End Source File
# Begin Source File

SOURCE=..\..\xdr_stdi.c
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# End Group
# End Target
# End Project
