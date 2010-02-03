/*
 * NekoDrive
 * 2010 by Mirko Gatto
 * mirko.gatto@gmail.com
 *
 *
 * Users may use, copy or modify this library 
 * according GNU General Public License v3 (http://www.gnu.org/licenses/gpl.html)
 */


#include "rpc/rpc.h"
#include "NFSv2MountProtocol.h"


bool_t
xdr_fhandle(xdrs, objp)
	XDR *xdrs;
	fhandle objp;
{
	if (!xdr_opaque(xdrs, objp, FHSIZE)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_dirpath(xdrs, objp)
	XDR *xdrs;
	dirpath *objp;
{
	if (!xdr_string(xdrs, objp, MNTPATHLEN)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_name(xdrs, objp)
	XDR *xdrs;
	name *objp;
{
	if (!xdr_string(xdrs, objp, MNTNAMLEN)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_fhstatus(xdrs, objp)
	XDR *xdrs;
	fhstatus *objp;
{
	if (!xdr_u_int(xdrs, &objp->status)) {
		return (FALSE);
	}
	switch (objp->status) {
	case 0:
		if (!xdr_fhandle(xdrs, &objp->fhstatus_u.directory)) {
			return (FALSE);
		}
		break;
	}
	return (TRUE);
}




bool_t
xdr_mountlist(xdrs, objp)
	XDR *xdrs;
	mountlist *objp;
{
	if (!xdr_pointer(xdrs, (char **)objp, sizeof(struct mountbody), xdr_mountbody)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_mountbody(xdrs, objp)
	XDR *xdrs;
	mountbody *objp;
{
	if (!xdr_name(xdrs, &objp->hostname)) {
		return (FALSE);
	}
	if (!xdr_dirpath(xdrs, &objp->directory)) {
		return (FALSE);
	}
	if (!xdr_mountlist(xdrs, &objp->nextentry)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_groups(xdrs, objp)
	XDR *xdrs;
	groups *objp;
{
	if (!xdr_pointer(xdrs, (char **)objp, sizeof(struct groupnode), xdr_groupnode)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_groupnode(xdrs, objp)
	XDR *xdrs;
	groupnode *objp;
{
	if (!xdr_name(xdrs, &objp->grname)) {
		return (FALSE);
	}
	if (!xdr_groups(xdrs, &objp->grnext)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_exports(xdrs, objp)
	XDR *xdrs;
	exports *objp;
{
	if (!xdr_pointer(xdrs, (char **)objp, sizeof(struct exportnode), xdr_exportnode)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_exportnode(xdrs, objp)
	XDR *xdrs;
	exportnode *objp;
{
	if (!xdr_dirpath(xdrs, &objp->filesys)) {
		return (FALSE);
	}
	if (!xdr_groups(xdrs, &objp->exgroups)) {
		return (FALSE);
	}
	if (!xdr_exports(xdrs, &objp->next)) {
		return (FALSE);
	}
	return (TRUE);
}


