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

/* @(#)svc_simple.c	2.2 88/08/01 4.0 RPCSRC */
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
static char sccsid[] = "@(#)svc_simple.c 1.18 87/08/11 Copyr 1984 Sun Micro";
#endif

/*
 * svc_simple.c
 * Simplified front end to rpc.
 *
 * Copyright (C) 1984, Sun Microsystems, Inc.
 */

#include "all_oncrpc.h"

static struct proglst {
	char *(*p_progname)();
	int  p_prognum;
	int  p_procnum;
	xdrproc_t p_inproc, p_outproc;
	struct proglst *p_nxt;
} *proglst;
static void universal();
static SVCXPRT *transp;
struct proglst *pl;

registerrpc(prognum, versnum, procnum, progname, inproc, outproc)
	char *(*progname)();
	xdrproc_t inproc, outproc;
{

	if (procnum == NULLPROC) {
#ifdef WIN32
		nt_rpc_report(
		    "can't reassign procedure number 0\n");
#else
		(void) fprintf(stderr,
		    "can't reassign procedure number %d\n", NULLPROC);
#endif
		return (-1);
	}
	if (transp == 0) {
		transp = svcudp_create(RPC_ANYSOCK);
		if (transp == NULL) {
#ifdef WIN32
			nt_rpc_report("couldn't create an rpc server\n");
#else
			(void) fprintf(stderr, "couldn't create an rpc server\n");
#endif
			return (-1);
		}
	}
	(void) pmap_unset((u_long)prognum, (u_long)versnum);
	if (!svc_register(transp, (u_long)prognum, (u_long)versnum,
	    universal, IPPROTO_UDP)) {
#ifdef WIN32
	    	nt_rpc_report("couldn't register prog");
#else
	    	(void) fprintf(stderr, "couldn't register prog %d vers %d\n",
		    prognum, versnum);
#endif
		return (-1);
	}
	pl = (struct proglst *)malloc(sizeof(struct proglst));
	if (pl == NULL) {
#ifdef WIN32
		nt_rpc_report("registerrpc: out of memory\n");
#else
		(void) fprintf(stderr, "registerrpc: out of memory\n");
#endif
		return (-1);
	}
	pl->p_progname = progname;
	pl->p_prognum = prognum;
	pl->p_procnum = procnum;
	pl->p_inproc = inproc;
	pl->p_outproc = outproc;
	pl->p_nxt = proglst;
	proglst = pl;
	return (0);
}

static void
universal(rqstp, transp)
	struct svc_req *rqstp;
	SVCXPRT *transp;
{
	int prog, proc;
	char *outdata;
	char xdrbuf[UDPMSGSIZE];
	struct proglst *pl;

	/*
	 * enforce "procnum 0 is echo" convention
	 */
	if (rqstp->rq_proc == NULLPROC) {
		if (svc_sendreply(transp, xdr_void, (char *)NULL) == FALSE) {
#ifdef WIN32
			nt_rpc_report(stderr, "xxx\n");
#else
			(void) fprintf(stderr, "xxx\n");
#endif
			exit(1);
		}
		return;
	}
	prog = rqstp->rq_prog;
	proc = rqstp->rq_proc;
	for (pl = proglst; pl != NULL; pl = pl->p_nxt)
		if (pl->p_prognum == prog && pl->p_procnum == proc) {
			/* decode arguments into a CLEAN buffer */
			bzero(xdrbuf, sizeof(xdrbuf)); /* required ! */
			if (!svc_getargs(transp, pl->p_inproc, xdrbuf)) {
				svcerr_decode(transp);
				return;
			}
			outdata = (*(pl->p_progname))(xdrbuf);
			if (outdata == NULL && pl->p_outproc != xdr_void)
				/* there was an error */
				return;
			if (!svc_sendreply(transp, pl->p_outproc, outdata)) {
#ifdef WIN32
				nt_rpc_report(
				    "trouble replying to prog\n"
				    /*, pl->p_prognum*/);
#else
				(void) fprintf(stderr,
				    "trouble replying to prog %d\n",
				    pl->p_prognum);
#endif
				exit(1);
			}
			/* free the decoded arguments */
			(void)svc_freeargs(transp, pl->p_inproc, xdrbuf);
			return;
		}
#ifdef WIN32
	nt_rpc_report("never registered prog %d\n"/*, prog*/);
#else
	(void) fprintf(stderr, "never registered prog %d\n", prog);
#endif
	exit(1);
}

