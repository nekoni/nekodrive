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
!include <win32.mak>

# If the rpc include directory is not included in the standard path
# you have to give the path to it here.
RPCINCLUDEPATH = ..\include

# If the rpc library is not included in the standard lib path
# you have to give the path to it here.
RPCLIBPATH = ..\librpc\lib\oncrpc_static\Debug\

# If you are on Windows NT uncomment the next line
OS=_NT

# If you are on Windows 95 uncomment the next line
#OS=_W95

all: portmap.exe

#if you are on Windows NT uncomment the next line
all: inst_pm.exe

# Update the object file if necessary

clean:
    del *.obj *.exe

portmap.exe: service.obj portmap.obj $(RPCLIBPATH)\oncrpc.lib
    $(link) $(ldebug) $(conflags) -out:portmap.exe portmap.obj service.obj $(conlibs) $(RPCLIBPATH)\oncrpc.lib wsock32.lib  advapi32.lib user32.lib
    copy portmap.exe ..\bin

inst_pm.exe: inst_pm.obj
    $(link) $(ldebug) $(conflags) -out:inst_pm.exe inst_pm.obj $(conlibs) advapi32.lib user32.lib
    copy inst_pm.exe ..\bin

.c.obj:
    $(cc) /I$(RPCINCLUDEPATH) -D$(OS) $(cdebug) $(cflags) $(cvars) $*.c
