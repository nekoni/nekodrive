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

/* @(#)pmap_getmaps.c	2.2 88/08/01 4.0 RPCSRC */
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
static char sccsid[] = "@(#)pmap_getmaps.c 1.10 87/08/11 Copyr 1984 Sun Micro";
#endif

/*
 * pmap_getmap.c
 * Client interface to pmap rpc service.
 * contains pmap_getmaps, which is only tcp service involved
 *
 * Copyright (C) 1984, Sun Microsystems, Inc.
 */

#include "all_oncrpc.h"

#define NAMELEN 255
#define MAX_BROADCAST_SIZE 1400

#ifndef WIN32
extern int errno;
#endif

/*
 * Get a copy of the current port maps.
 * Calls the pmap service remotely to do get the maps.
 */
struct pmaplist *
pmap_getmaps(address)
	 struct sockaddr_in *address;
{
	struct pmaplist *head = (struct pmaplist *)NULL;
	int socket = -1;
	struct timeval minutetimeout;
	register CLIENT *client;

	minutetimeout.tv_sec = 60;
	minutetimeout.tv_usec = 0;
	address->sin_port = htons(PMAPPORT);
	client = clnttcp_create(address, PMAPPROG,
	    PMAPVERS, &socket, 50, 500);
	if (client != (CLIENT *)NULL) {
		if (CLNT_CALL(client, PMAPPROC_DUMP, xdr_void, NULL, xdr_pmaplist,
		    &head, minutetimeout) != RPC_SUCCESS) {
			clnt_perror(client, "pmap_getmaps rpc problem");
		}
		CLNT_DESTROY(client);
	}
#ifdef WIN32
	(void)closesocket(socket);
#else
	(void)close(socket);
#endif
	address->sin_port = 0;
	return (head);
}
