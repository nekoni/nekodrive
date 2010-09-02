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

/* @(#)pmap_clnt.c	2.2 88/08/01 4.0 RPCSRC */
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
#if !defined(lint) && defined(SCCSIDS)
static char sccsid[] = "@(#)pmap_clnt.c 1.37 87/08/11 Copyr 1984 Sun Micro";
#endif

/*
 * pmap_clnt.c
 * Client interface to pmap rpc service.
 *
 * Copyright (C) 1984, Sun Microsystems, Inc.
 */

#include "all_oncrpc.h"

static struct timeval timeout = { 5, 0 };
static struct timeval tottimeout = { 60, 0 };

void clnt_perror();


/*
 * Set a mapping between program,version and port.
 * Calls the pmap service remotely to do the mapping.
 */
bool_t
pmap_set(program, version, protocol, port)
	u_long program;
	u_long version;
	int protocol;
	u_short port;
{
	struct sockaddr_in myaddress;
	int socket = -1;
	register CLIENT *client;
	struct pmap parms;
	bool_t rslt;

	get_myaddress(&myaddress);
	client = clntudp_bufcreate(&myaddress, PMAPPROG, PMAPVERS,
	    timeout, &socket, RPCSMALLMSGSIZE, RPCSMALLMSGSIZE);
	if (client == (CLIENT *)NULL)
		return (FALSE);
	parms.pm_prog = program;
	parms.pm_vers = version;
	parms.pm_prot = protocol;
	parms.pm_port = port;
	if (CLNT_CALL(client, PMAPPROC_SET, xdr_pmap, &parms, xdr_bool, &rslt,
	    tottimeout) != RPC_SUCCESS) {
		clnt_perror(client, "Cannot register service");
		return (FALSE);
	}
	CLNT_DESTROY(client);
#ifdef WIN32
	(void)closesocket(socket);
#else
	(void)close(socket);
#endif
	return (rslt);
}

/*
 * Remove the mapping between program,version and port.
 * Calls the pmap service remotely to do the un-mapping.
 */
bool_t
pmap_unset(program, version)
	u_long program;
	u_long version;
{
	struct sockaddr_in myaddress;
	int socket = -1;
	register CLIENT *client;
	struct pmap parms;
	bool_t rslt;

	get_myaddress(&myaddress);
	client = clntudp_bufcreate(&myaddress, PMAPPROG, PMAPVERS,
	    timeout, &socket, RPCSMALLMSGSIZE, RPCSMALLMSGSIZE);
	if (client == (CLIENT *)NULL)
		return (FALSE);
	parms.pm_prog = program;
	parms.pm_vers = version;
	parms.pm_port = parms.pm_prot = 0;
	CLNT_CALL(client, PMAPPROC_UNSET, xdr_pmap, &parms, xdr_bool, &rslt,
	    tottimeout);
	CLNT_DESTROY(client);
#ifdef WIN32
	(void)closesocket(socket);
#else
	(void)close(socket);
#endif
	return (rslt);
}
