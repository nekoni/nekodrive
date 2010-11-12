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
RPCINCLUDEPATH = ..

# If the rpc library is not included in the standard lib path
# you have to give the path to it here.
RPCLIBPATH = ..\bin\\

OBJS= rpc_main.obj rpc_hout.obj rpc_cout.obj rpc_pars.obj rpc_scan.obj rpc_util.obj \
	rpc_svco.obj rpc_clnt.obj

GOAL=rpcgen.exe

$(GOAL): $(OBJS) $(RPCLIBPATH)\oncrpc.lib
	$(link) $(ldebug) $(conflags) $(OBJS) -out:$(GOAL) $(conlibsdll) $(RPCLIBPATH)\oncrpc.lib
	copy $(GOAL) ..\bin

clean:
	-del $(GOAL) $(OBJS)

.c.obj:
    $(cc) /I$(RPCINCLUDEPATH) $(cdebug) $(cflags) $(cvarsdll) $*.c

rpc_main.obj: rpc_main.c
rpc_main.obj: .\rpc_util.h
rpc_main.obj: .\rpc_pars.h
rpc_main.obj: .\rpc_scan.h
rpc_hout.obj: rpc_hout.c
rpc_hout.obj: .\rpc_util.h
rpc_hout.obj: .\rpc_pars.h
rpc_cout.obj: rpc_cout.c
rpc_cout.obj: .\rpc_util.h
rpc_cout.obj: .\rpc_pars.h
rpc_pars.obj: rpc_pars.c
rpc_pars.obj: .\rpc_util.h
rpc_pars.obj: .\rpc_scan.h
rpc_pars.obj: .\rpc_pars.h
rpc_scan.obj: rpc_scan.c
rpc_scan.obj: .\rpc_scan.h
rpc_scan.obj: .\rpc_util.h
rpc_util.obj: rpc_util.c
rpc_util.obj: .\rpc_scan.h
rpc_util.obj: .\rpc_pars.h
rpc_util.obj: .\rpc_util.h
rpc_svco.obj: rpc_svco.c
rpc_svco.obj: .\rpc_pars.h
rpc_svco.obj: .\rpc_util.h
rpc_clnt.obj: rpc_clnt.c
rpc_clnt.obj: .\rpc_pars.h
rpc_clnt.obj: .\rpc_util.h
