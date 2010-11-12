#**********************************************************************
#* ONC RPC for WIN32.
#* 1997 by WD Klotz
#* ESRF, BP 220, F-38640 Grenoble, CEDEX
#* klotz-tech@esrf.fr
#*
#* SUN's ONC RPC for Windows NT and Windows 95. Ammended port from
#* Martin F.Gergeleit's distribution. This version has been modified
#* and cleaned, such as to be compatible with Windows NT and Windows 95. 
#* Compiler: MSVC++ version 4.2 and 5.0.
#*
#* RPC for the Windows NT Operating System COMES WITH ABSOLUTELY NO 
#* WARRANTY, NOR WILL I BE LIABLE FOR ANY DAMAGES INCURRED FROM THE 
#* USE OF. USE ENTIRELY AT YOUR OWN RISK!!!
#**********************************************************************
# Nmake macros for building Windows 32-Bit apps
!include <ntwin32.mak>

# If the rpc include directory is not included in the standard path
# you have to give the path to it here.
RPCINCLUDEPATH = ..\include

# If the rpc library is not included in the standard lib path
# you have to give the path to it here.
RPCLIBPATH = ..\librpc\lib\release\\

OBJS= rpcinfo.obj

GOAL=rpcinfo.exe

$(GOAL): $(OBJS) $(RPCLIBPATH)\oncrpc.lib
	$(link) $(ldebug) $(conflags) $(OBJS) -out:$(GOAL) $(conlibsdll) $(RPCLIBPATH)\oncrpc.lib wsock32.lib
	copy $(GOAL) ..\bin

clean:
	-del $(GOAL) $(OBJS)

.c.obj:
    $(cc) /I$(RPCINCLUDEPATH) $(cdebug) $(cflags) $(cvarsdll) $*.c

rpcinfo.obj: rpcinfo.c getopt.c

