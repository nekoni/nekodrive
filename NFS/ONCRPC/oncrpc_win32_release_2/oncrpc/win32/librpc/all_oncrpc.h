/**********************************************************************
 * ONC RPC for WIN32.
 * 1997 by WD Klotz
 * ESRF, BP 220, F-38640 Grenoble, CEDEX
 * klotz-tech@esrf.fr
 *
 * SUN's ONC RPC for Windows NT and Windows 95. Ammended port from
 * Martin F.Gergeleit's distribution. This version has been modified
 * and cleaned, such as to be compatible with Windows NT and Windows 95. 
 * Compiler: MSVC++ version 4.2 and 5.0.
 *
 * Users may use, copy or modify Sun RPC for the Windows NT Operating 
 * System according to the Sun copyright below.
 * RPC for the Windows NT Operating System COMES WITH ABSOLUTELY NO 
 * WARRANTY, NOR WILL I BE LIABLE FOR ANY DAMAGES INCURRED FROM THE 
 * USE OF. USE ENTIRELY AT YOUR OWN RISK!!!
 **********************************************************************/
#ifndef __all_oncrpc_includes__
#define __all_oncrpc_includes__

#ifdef WIN32
#ifndef DllExport
#define DllExport	__declspec( dllexport )
#endif
#ifndef DllImport
#define DllImport   __declspec( dllimport )
#endif

#if defined _W95 || defined _NT
#include <stdio.h>
#include <time.h>
#include <string.h>
#include <stdlib.h>
#include <malloc.h>
#endif

#include <rpc/netdb.h>
#include <rpc/rpc.h>
#include <rpc/xdr.h>
#include <rpc/auth.h>
#include <rpc/auth_uni.h>
#include <rpc/clnt.h>
#include <rpc/pmap_cln.h>
#include <rpc/pmap_pro.h>
#include <rpc/pmap_rmt.h>
#include <rpc/xdr.h>
#include <sys/types.h>
#include <io.h>
#include <errno.h>
#include <winsock.h>

#else  /* not WIN32 */
#ifndef DllExport
#define DllExport	extern
#endif
#ifndef DllImport
#define DllImport   extern
#endif

#include <rpc/types.h>
#include <rpc/xdr.h>
#include <rpc/auth.h>
#include <rpc/auth_unix.h>
#include <rpc/clnt.h>
#include <rpc/pmap_clnt.h>
#include <rpc/pmap_prot.h>
#include <rpc/pmap_rmt.h>
#include <rpc/xdr.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <sys/ioctl.h>
#include <sys/param.h>
#include <sys/errno.h>
#include <netinet/in.h>
#include <netdb.h>
#include <net/if.h>
#include <arpa/inet.h>
#endif

DllExport void get_myaddress(struct sockaddr_in *addr);
int bindresvport(int sd,struct sockaddr_in *sin);
void bcopy(char *s1,char *s2, int len);
void bzero(char *s, int len);
int bcmp(char *s1, char *s2, int len);

#endif  /*__all_oncrpc_includes__*/