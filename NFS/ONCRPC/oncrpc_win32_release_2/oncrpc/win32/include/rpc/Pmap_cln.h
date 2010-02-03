/*
 * NekoDrive
 * copyright 2009 by Mirko Gatto
 * mirko.gatto@gmail.com
 *
 * 2010-27-01: C++ compatible
 *
 * Users may use, copy or modify Sun RPC for the Windows NT Operating 
 * System according to the Sun copyright below.
 */
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
/*********************************************************************
 * RPC for the Windows NT Operating System
 * 1993 by Martin F. Gergeleit
 * Users may use, copy or modify Sun RPC for the Windows NT Operating 
 * System according to the Sun copyright below.
 *
 * RPC for the Windows NT Operating System COMES WITH ABSOLUTELY NO 
 * WARRANTY, NOR WILL I BE LIABLE FOR ANY DAMAGES INCURRED FROM THE 
 * USE OF. USE ENTIRELY AT YOUR OWN RISK!!!
 *********************************************************************/

/* @(#)pmap_clnt.h	2.1 88/07/29 4.0 RPCSRC; from 1.11 88/02/08 SMI */
/*
 * Sun RPC is a product of Sun Microsystems, Inc. and is provided for
 * unrestricted use provided that this legend is included on all tape
 * media and as a part of the software program in whole or part.  Users
 * may copy or modify Sun RPC without charge, but are not authorized
 * to license or distribute it to anyone else except as part of a product or
 * program developed by the user.
 * 
 * SUN RPC IS PROVIDED AS IS WITH NO WARRANTIES OF ANY KIND INCLUDING THE
 * WARRANTIES OF DESIGN, MERCHANTIBILITY AND FITNESS FOR A PARTICULAR
 * PURPOSE, OR ARISING FROM A COURSE OF DEALING, USAGE OR TRADE PRACTICE.
 * 
 * Sun RPC is provided with no support and without any obligation on the
 * part of Sun Microsystems, Inc. to assist in its use, correction,
 * modification or enhancement.
 * 
 * SUN MICROSYSTEMS, INC. SHALL HAVE NO LIABILITY WITH RESPECT TO THE
 * INFRINGEMENT OF COPYRIGHTS, TRADE SECRETS OR ANY PATENTS BY SUN RPC
 * OR ANY PART THEREOF.
 * 
 * In no event will Sun Microsystems, Inc. be liable for any lost revenue
 * or profits or other special, indirect and consequential damages, even if
 * Sun has been advised of the possibility of such damages.
 * 
 * Sun Microsystems, Inc.
 * 2550 Garcia Avenue
 * Mountain View, California  94043
 */

#ifndef __INCpmap_clnth
#define __INCpmap_clnth

/*
 * pmap_clnt.h
 * Supplies C routines to get to portmap services.
 *
 * Copyright (C) 1984, Sun Microsystems, Inc.
 */

/*
 * Usage:
 *	success = pmap_set(program, version, protocol, port);
 *	success = pmap_unset(program, version);
 *	port = pmap_getport(address, program, version, protocol);
 *	head = pmap_getmaps(address);
 *	clnt_stat = pmap_rmtcall(address, program, version, procedure,
 *		xdrargs, argsp, xdrres, resp, tout, port_ptr)
 *		(works for udp only.) 
 * 	clnt_stat = clnt_broadcast(program, version, procedure,
 *		xdrargs, argsp,	xdrres, resp, eachresult)
 *		(like pmap_rmtcall, except the call is broadcasted to all
 *		locally connected nets.  For each valid response received,
 *		the procedure eachresult is called.  Its form is:
 *	done = eachresult(resp, raddr)
 *		bool_t done;
 *		caddr_t resp;
 *		struct sockaddr_in raddr;
 *		where resp points to the results of the call and raddr is the
 *		address if the responder to the broadcast.
 */

#ifdef __cplusplus
extern "C" {
#endif

DllExport bool_t		pmap_set(u_long program, u_long version, u_int protocol, int port);
DllExport bool_t		pmap_unset(u_long program, u_long version);
DllExport struct pmaplist	*pmap_getmaps(struct sockaddr_in *address);
DllExport enum clnt_stat		pmap_rmtcall(struct sockaddr_in *addr, u_long prog,
								u_long vers, u_long proc, xdrproc_t xdrargs,
								caddr_t argsp, xdrproc_t xdrres, caddr_t resp,
								struct timeval tout, u_long *port_ptr);
DllExport enum clnt_stat		clnt_broadcast(u_long prog, u_long vers, u_long proc,
								xdrproc_t xargs, caddr_t argsp,
								xdrproc_t xresults, caddr_t resultsp,
								bool_t (*eachresult)());
DllExport u_short		pmap_getport(struct sockaddr_in *address, u_long  program, u_long version, u_int protocol);
#ifdef __cplusplus
}
#else

DllExport bool_t		pmap_set(u_long program, u_long version, u_int protocol, int port);
DllExport bool_t		pmap_unset(u_long program, u_long version);
DllExport struct pmaplist	*pmap_getmaps(struct sockaddr_in *address);
DllExport enum clnt_stat		pmap_rmtcall(struct sockaddr_in *addr, u_long prog,
								u_long vers, u_long proc, xdrproc_t xdrargs,
								caddr_t argsp, xdrproc_t xdrres, caddr_t resp,
								struct timeval tout, u_long *port_ptr);
DllExport enum clnt_stat		clnt_broadcast(u_long prog, u_long vers, u_long proc,
								xdrproc_t xargs, caddr_t argsp,
								xdrproc_t xresults, caddr_t resultsp,
								bool_t (*eachresult)());
DllExport u_short		pmap_getport(struct sockaddr_in *address, u_long  program, u_long version, u_int protocol);


#endif

#endif