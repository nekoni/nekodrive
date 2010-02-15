#include <rpc/rpc.h>
#include "NFSv3Protocol.h"

/* Default timeout can be changed using clnt_control() */
static struct timeval TIMEOUT = { 25, 0 };

void *
nfsproc3_null_3(argp, clnt)
	void *argp;
	CLIENT *clnt;
{
	static char res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_NULL, xdr_void, argp, xdr_void, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return ((void *)&res);
}


GETATTR3res *
nfsproc3_getattr_3(argp, clnt)
	GETATTR3args *argp;
	CLIENT *clnt;
{
	static GETATTR3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_GETATTR, xdr_GETATTR3args, argp, xdr_GETATTR3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


SETATTR3res *
nfsproc3_setattr_3(argp, clnt)
	SETATTR3args *argp;
	CLIENT *clnt;
{
	static SETATTR3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_SETATTR, xdr_SETATTR3args, argp, xdr_SETATTR3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


LOOKUP3res *
nfsproc3_lookup_3(argp, clnt)
	LOOKUP3args *argp;
	CLIENT *clnt;
{
	static LOOKUP3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_LOOKUP, xdr_LOOKUP3args, argp, xdr_LOOKUP3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


ACCESS3res *
nfsproc3_access_3(argp, clnt)
	ACCESS3args *argp;
	CLIENT *clnt;
{
	static ACCESS3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_ACCESS, xdr_ACCESS3args, argp, xdr_ACCESS3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


READLINK3res *
nfsproc3_readlink_3(argp, clnt)
	READLINK3args *argp;
	CLIENT *clnt;
{
	static READLINK3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_READLINK, xdr_READLINK3args, argp, xdr_READLINK3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


READ3res *
nfsproc3_read_3(argp, clnt)
	READ3args *argp;
	CLIENT *clnt;
{
	static READ3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_READ, xdr_READ3args, argp, xdr_READ3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


WRITE3res *
nfsproc3_write_3(argp, clnt)
	WRITE3args *argp;
	CLIENT *clnt;
{
	static WRITE3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_WRITE, xdr_WRITE3args, argp, xdr_WRITE3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


CREATE3res *
nfsproc3_create_3(argp, clnt)
	CREATE3args *argp;
	CLIENT *clnt;
{
	static CREATE3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_CREATE, xdr_CREATE3args, argp, xdr_CREATE3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


MKDIR3res *
nfsproc3_mkdir_3(argp, clnt)
	MKDIR3args *argp;
	CLIENT *clnt;
{
	static MKDIR3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_MKDIR, xdr_MKDIR3args, argp, xdr_MKDIR3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


SYMLINK3res *
nfsproc3_symlink_3(argp, clnt)
	SYMLINK3args *argp;
	CLIENT *clnt;
{
	static SYMLINK3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_SYMLINK, xdr_SYMLINK3args, argp, xdr_SYMLINK3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


MKNOD3res *
nfsproc3_mknod_3(argp, clnt)
	MKNOD3args *argp;
	CLIENT *clnt;
{
	static MKNOD3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_MKNOD, xdr_MKNOD3args, argp, xdr_MKNOD3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


REMOVE3res *
nfsproc3_remove_3(argp, clnt)
	REMOVE3args *argp;
	CLIENT *clnt;
{
	static REMOVE3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_REMOVE, xdr_REMOVE3args, argp, xdr_REMOVE3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


RMDIR3res *
nfsproc3_rmdir_3(argp, clnt)
	RMDIR3args *argp;
	CLIENT *clnt;
{
	static RMDIR3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_RMDIR, xdr_RMDIR3args, argp, xdr_RMDIR3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


RENAME3res *
nfsproc3_rename_3(argp, clnt)
	RENAME3args *argp;
	CLIENT *clnt;
{
	static RENAME3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_RENAME, xdr_RENAME3args, argp, xdr_RENAME3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


LINK3res *
nfsproc3_link_3(argp, clnt)
	LINK3args *argp;
	CLIENT *clnt;
{
	static LINK3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_LINK, xdr_LINK3args, argp, xdr_LINK3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


READDIR3res *
nfsproc3_readdir_3(argp, clnt)
	READDIR3args *argp;
	CLIENT *clnt;
{
	static READDIR3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_READDIR, xdr_READDIR3args, argp, xdr_READDIR3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


READDIRPLUS3res *
nfsproc3_readdirplus_3(argp, clnt)
	READDIRPLUS3args *argp;
	CLIENT *clnt;
{
	static READDIRPLUS3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_READDIRPLUS, xdr_READDIRPLUS3args, argp, xdr_READDIRPLUS3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


FSSTAT3res *
nfsproc3_fsstat_3(argp, clnt)
	FSSTAT3args *argp;
	CLIENT *clnt;
{
	static FSSTAT3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_FSSTAT, xdr_FSSTAT3args, argp, xdr_FSSTAT3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


FSINFO3res *
nfsproc3_fsinfo_3(argp, clnt)
	FSINFO3args *argp;
	CLIENT *clnt;
{
	static FSINFO3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_FSINFO, xdr_FSINFO3args, argp, xdr_FSINFO3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


PATHCONF3res *
nfsproc3_pathconf_3(argp, clnt)
	PATHCONF3args *argp;
	CLIENT *clnt;
{
	static PATHCONF3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_PATHCONF, xdr_PATHCONF3args, argp, xdr_PATHCONF3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


COMMIT3res *
nfsproc3_commit_3(argp, clnt)
	COMMIT3args *argp;
	CLIENT *clnt;
{
	static COMMIT3res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC3_COMMIT, xdr_COMMIT3args, argp, xdr_COMMIT3res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}

