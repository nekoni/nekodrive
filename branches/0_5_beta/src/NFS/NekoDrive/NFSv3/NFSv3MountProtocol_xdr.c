/*
 * NekoDrive
 * 2010 by Mirko Gatto
 * mirko.gatto@gmail.com
 *
 *
 * Users may use, copy or modify this library 
 * according GNU General Public License v3 (http://www.gnu.org/licenses/gpl.html)
 */

#pragma pack(1)

#include <rpc/rpc.h>
#include "NFSv3MountProtocol.h"


bool_t
xdr_fhandle3(xdrs, objp)
	XDR *xdrs;
	fhandle3 *objp;
{
	if (!xdr_bytes(xdrs, (char **)&objp->data.data_val, (u_int *)&objp->data.data_len, 64)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_dirpath(xdrs, objp)
	XDR *xdrs;
	dirpath *objp;
{
	if (!xdr_string(xdrs, objp, 1024)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_name(xdrs, objp)
	XDR *xdrs;
	name *objp;
{
	if (!xdr_string(xdrs, objp, 255)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_mountstat3(xdrs, objp)
	XDR *xdrs;
	mountstat3 *objp;
{
	if (!xdr_enum(xdrs, (enum_t *)objp)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_mountres3_ok(xdrs, objp)
	XDR *xdrs;
	mountres3_ok *objp;
{
	if (!xdr_fhandle3(xdrs, &objp->fhandle)) {
		return (FALSE);
	}
	if (!xdr_array(xdrs, (char **)&objp->auth_flavors.auth_flavors_val, (u_int *)&objp->auth_flavors.auth_flavors_len, ~0, sizeof(int), xdr_int)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_mountres3(xdrs, objp)
	XDR *xdrs;
	mountres3 *objp;
{
	if (!xdr_mountstat3(xdrs, &objp->fhs_status)) {
		return (FALSE);
	}
	switch (objp->fhs_status) {
	case MNT3_OK:
		if (!xdr_mountres3_ok(xdrs, &objp->mountres3_u.mountinfo)) {
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
	if (!xdr_name(xdrs, &objp->ml_hostname)) {
		return (FALSE);
	}
	if (!xdr_dirpath(xdrs, &objp->ml_directory)) {
		return (FALSE);
	}
	if (!xdr_mountlist(xdrs, &objp->ml_next)) {
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
	if (!xdr_name(xdrs, &objp->gr_name)) {
		return (FALSE);
	}
	if (!xdr_groups(xdrs, &objp->gr_next)) {
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
	if (!xdr_dirpath(xdrs, &objp->ex_dir)) {
		return (FALSE);
	}
	if (!xdr_groups(xdrs, &objp->ex_groups)) {
		return (FALSE);
	}
	if (!xdr_exports(xdrs, &objp->ex_next)) {
		return (FALSE);
	}
	return (TRUE);
}


