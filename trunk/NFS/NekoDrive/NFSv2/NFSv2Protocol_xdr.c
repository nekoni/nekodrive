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
#include "NFSv2Protocol.h"


bool_t
xdr_nfsstat(xdrs, objp)
	XDR *xdrs;
	nfsstat *objp;
{
	if (!xdr_enum(xdrs, (enum_t *)objp)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_ftype(xdrs, objp)
	XDR *xdrs;
	ftype *objp;
{
	if (!xdr_enum(xdrs, (enum_t *)objp)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_nfshandle(xdrs, objp)
	XDR *xdrs;
	nfshandle objp;
{
	if (!xdr_opaque(xdrs, objp, FHSIZE)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_filename(xdrs, objp)
	XDR *xdrs;
	filename *objp;
{
	if (!xdr_string(xdrs, objp, MAXNAMLEN)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_path(xdrs, objp)
	XDR *xdrs;
	path *objp;
{
	if (!xdr_string(xdrs, objp, MAXPATHLEN)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_nfscookie(xdrs, objp)
	XDR *xdrs;
	nfscookie *objp;
{
	if (!xdr_u_int(xdrs, objp)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_nfstimeval(xdrs, objp)
	XDR *xdrs;
	nfstimeval *objp;
{
	if (!xdr_u_int(xdrs, &objp->seconds)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->useconds)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_fattr(xdrs, objp)
	XDR *xdrs;
	fattr *objp;
{
	if (!xdr_ftype(xdrs, &objp->type)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->mode)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->nlink)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->uid)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->gid)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->size)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->blocksize)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->rdev)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->blocks)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->fsid)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->fileid)) {
		return (FALSE);
	}
	if (!xdr_nfstimeval(xdrs, &objp->atime)) {
		return (FALSE);
	}
	if (!xdr_nfstimeval(xdrs, &objp->mtime)) {
		return (FALSE);
	}
	if (!xdr_nfstimeval(xdrs, &objp->ctime)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_sattr(xdrs, objp)
	XDR *xdrs;
	sattr *objp;
{
	if (!xdr_u_int(xdrs, &objp->mode)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->uid)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->gid)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->size)) {
		return (FALSE);
	}
	if (!xdr_nfstimeval(xdrs, &objp->atime)) {
		return (FALSE);
	}
	if (!xdr_nfstimeval(xdrs, &objp->mtime)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_attrstat(xdrs, objp)
	XDR *xdrs;
	attrstat *objp;
{
	if (!xdr_nfsstat(xdrs, &objp->status)) {
		return (FALSE);
	}
	switch (objp->status) {
	case NFS_OK:
		if (!xdr_fattr(xdrs, &objp->attrstat_u.attributes)) {
			return (FALSE);
		}
		break;
	}
	return (TRUE);
}




bool_t
xdr_diropargs(xdrs, objp)
	XDR *xdrs;
	diropargs *objp;
{
	if (!xdr_nfshandle(xdrs, objp->dir)) {
		return (FALSE);
	}
	if (!xdr_filename(xdrs, &objp->name)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_diropok(xdrs, objp)
	XDR *xdrs;
	diropok *objp;
{
	if (!xdr_nfshandle(xdrs, objp->file)) {
		return (FALSE);
	}
	if (!xdr_fattr(xdrs, &objp->attributes)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_diropres(xdrs, objp)
	XDR *xdrs;
	diropres *objp;
{
	if (!xdr_nfsstat(xdrs, &objp->status)) {
		return (FALSE);
	}
	switch (objp->status) {
	case NFS_OK:
		if (!xdr_diropok(xdrs, &objp->diropres_u.ok)) {
			return (FALSE);
		}
		break;
	}
	return (TRUE);
}




bool_t
xdr_sattrargs(xdrs, objp)
	XDR *xdrs;
	sattrargs *objp;
{
	if (!xdr_nfshandle(xdrs, objp->file)) {
		return (FALSE);
	}
	if (!xdr_sattr(xdrs, &objp->attributes)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_readlinkres(xdrs, objp)
	XDR *xdrs;
	readlinkres *objp;
{
	if (!xdr_nfsstat(xdrs, &objp->status)) {
		return (FALSE);
	}
	switch (objp->status) {
	case NFS_OK:
		if (!xdr_path(xdrs, &objp->readlinkres_u.data)) {
			return (FALSE);
		}
		break;
	}
	return (TRUE);
}




bool_t
xdr_readargs(xdrs, objp)
	XDR *xdrs;
	readargs *objp;
{
	if (!xdr_nfshandle(xdrs, objp->file)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->offset)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->count)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->totalcount)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_readresok(xdrs, objp)
	XDR *xdrs;
	readresok *objp;
{
	if (!xdr_fattr(xdrs, &objp->attributes)) {
		return (FALSE);
	}
	if (!xdr_bytes(xdrs, (char **)&objp->data.data_val, (u_int *)&objp->data.data_len, ~0)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_readres(xdrs, objp)
	XDR *xdrs;
	readres *objp;
{
	if (!xdr_nfsstat(xdrs, &objp->status)) {
		return (FALSE);
	}
	switch (objp->status) {
	case NFS_OK:
		if (!xdr_readresok(xdrs, &objp->readres_u.ok)) {
			return (FALSE);
		}
		break;
	}
	return (TRUE);
}




bool_t
xdr_writeargs(xdrs, objp)
	XDR *xdrs;
	writeargs *objp;
{
	if (!xdr_nfshandle(xdrs, objp->file)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->beginoffset)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->offset)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->totalcount)) {
		return (FALSE);
	}
	if (!xdr_bytes(xdrs, (char **)&objp->data.data_val, (u_int *)&objp->data.data_len, ~0)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_createargs(xdrs, objp)
	XDR *xdrs;
	createargs *objp;
{
	if (!xdr_diropargs(xdrs, &objp->where)) {
		return (FALSE);
	}
	if (!xdr_sattr(xdrs, &objp->attributes)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_renameargs(xdrs, objp)
	XDR *xdrs;
	renameargs *objp;
{
	if (!xdr_diropargs(xdrs, &objp->from)) {
		return (FALSE);
	}
	if (!xdr_diropargs(xdrs, &objp->to)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_linkargs(xdrs, objp)
	XDR *xdrs;
	linkargs *objp;
{
	if (!xdr_nfshandle(xdrs, objp->from)) {
		return (FALSE);
	}
	if (!xdr_diropargs(xdrs, &objp->to)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_symlinkargs(xdrs, objp)
	XDR *xdrs;
	symlinkargs *objp;
{
	if (!xdr_diropargs(xdrs, &objp->from)) {
		return (FALSE);
	}
	if (!xdr_path(xdrs, &objp->to)) {
		return (FALSE);
	}
	if (!xdr_sattr(xdrs, &objp->attributes)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_readdirargs(xdrs, objp)
	XDR *xdrs;
	readdirargs *objp;
{
	if (!xdr_nfshandle(xdrs, objp->dir)) {
		return (FALSE);
	}
	if (!xdr_nfscookie(xdrs, &objp->cookie)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->count)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_entry(xdrs, objp)
	XDR *xdrs;
	entry *objp;
{
	if (!xdr_u_int(xdrs, &objp->fileid)) {
		return (FALSE);
	}
	if (!xdr_filename(xdrs, &objp->name)) {
		return (FALSE);
	}
	if (!xdr_nfscookie(xdrs, &objp->cookie)) {
		return (FALSE);
	}
	if (!xdr_pointer(xdrs, (char **)&objp->nextentry, sizeof(entry), xdr_entry)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_readdirok(xdrs, objp)
	XDR *xdrs;
	readdirok *objp;
{
	if (!xdr_pointer(xdrs, (char **)&objp->entries, sizeof(entry), xdr_entry)) {
		return (FALSE);
	}
	if (!xdr_bool(xdrs, &objp->eof)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_readdirres(xdrs, objp)
	XDR *xdrs;
	readdirres *objp;
{
	if (!xdr_nfsstat(xdrs, &objp->status)) {
		return (FALSE);
	}
	switch (objp->status) {
	case NFS_OK:
		if (!xdr_readdirok(xdrs, &objp->readdirres_u.ok)) {
			return (FALSE);
		}
		break;
	}
	return (TRUE);
}




bool_t
xdr_info(xdrs, objp)
	XDR *xdrs;
	info *objp;
{
	if (!xdr_u_int(xdrs, &objp->tsize)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->bsize)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->blocks)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->bfree)) {
		return (FALSE);
	}
	if (!xdr_u_int(xdrs, &objp->bavail)) {
		return (FALSE);
	}
	return (TRUE);
}




bool_t
xdr_statfsres(xdrs, objp)
	XDR *xdrs;
	statfsres *objp;
{
	if (!xdr_nfsstat(xdrs, &objp->status)) {
		return (FALSE);
	}
	switch (objp->status) {
	case NFS_OK:
		if (!xdr_info(xdrs, &objp->statfsres_u.ok)) {
			return (FALSE);
		}
		break;
	}
	return (TRUE);
}


