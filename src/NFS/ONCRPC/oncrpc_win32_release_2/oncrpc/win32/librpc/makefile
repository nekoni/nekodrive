# Nmake macros for building Windows 32-Bit apps
!include <ntwin32.mak>

# If the rpc include directory is not included in the standard path
# you have to give the path to it here.
RPCINCLUDEPATH = ..

# If the rpc library is not included in the standard lib path
# you have to give the path to it here.
RPCLIBPATH = ..\bin\\

DEFINITION =	ONCRPC.DEF

OBJS =		CLNT_RAW.OBJ \
		XDR.OBJ \
		CLNT_TCP.OBJ \
		CLNT_UDP.OBJ \
		PMAP_RMT.OBJ \
		RPC_PROT.OBJ \
		SVC_AUTU.OBJ \
		SVC_AUTH.OBJ \
		SVC_RAW.OBJ \
		SVC_RUN.OBJ \
		SVC_TCP.OBJ \
		SVC_UDP.OBJ \
		XDR_MEM.OBJ \
		XDR_REC.OBJ \
		AUTH_NON.OBJ \
		AUTH_UNI.OBJ \
		AUTHUNIX.OBJ \
		BINDRESV.OBJ \
		CLNT_GEN.OBJ \
		CLNT_PER.OBJ \
		CLNT_SIM.OBJ \
		GET_MYAD.OBJ \
		GETRPCEN.OBJ \
		GETRPCPO.OBJ \
		PMAP_CLN.OBJ \
		PMAP_GET.OBJ \
		PMAP_GMA.OBJ \
		PMAP_PRO.OBJ \
		PMAP_PR.OBJ \
		RPC_CALL.OBJ \
		RPC_COMM.OBJ \
		SVC_SIMP.OBJ \
		XDR_ARRA.OBJ \
		XDR_FLOA.OBJ \
		XDR_REFE.OBJ \
		XDR_STDI.OBJ \
		SVC.OBJ \
		BCOPY.OBJ \
		NT.OBJ

all: oncrpc.dll portmap.exe

clean:
	del $(OBJS) oncrpc.lib oncrpc.dll oncrpc.exp portmap.obj portmap.exe ..\rpcgen\oncrpc.dll

portmap.exe:	oncrpc.lib portmap.obj
     $(link) $(conlflags) $(ldebug) -out:portmap.exe PORTMAP.obj $(RPCLIBPATH)oncrpc.lib $(conlibsdll) wsock32.lib
	copy portmap.exe ..\bin\pm_ascii.exe

oncrpc.lib:	$(OBJS) oncrpc.def
    $(implib) /out:oncrpc.lib /def:$(DEFINITION) $(OBJS)

oncrpc.dll:	$(OBJS) oncrpc.lib oncrpc.exp
    $(link) /DLL /out:oncrpc.dll -entry:_DllMainCRTStartup$(DLLENTRY) $(ldebug) oncrpc.exp $(OBJS) $(conlibsdll) wsock32.lib advapi32.lib
    copy oncrpc.lib ..\bin
    copy oncrpc.dll ..\bin
    copy oncrpc.dll ..\rpcgen

.c.obj:
    $(cc) /I$(RPCINCLUDEPATH) /DONCRPCDLL $(cdebug) $(cflags) $(cvarsdll) $*.c
