#include <rpc/rpc.h>
#include "NFSv3MountProtocol.h"

/* Default timeout can be changed using clnt_control() */
static struct timeval TIMEOUT = { 25, 0 };

void *
mountproc3_null_3(argp, clnt)
	void *argp;
	CLIENT *clnt;
{
	static char res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, MOUNTPROC3_NULL, xdr_void, argp, xdr_void, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return ((void *)&res);
}


mountres3 *
mountproc3_mnt_3(argp, clnt)
	dirpath *argp;
	CLIENT *clnt;
{
	static mountres3 res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, MOUNTPROC3_MNT, xdr_dirpath, argp, xdr_mountres3, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


mountlist *
mountproc3_dump_3(argp, clnt)
	void *argp;
	CLIENT *clnt;
{
	static mountlist res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, MOUNTPROC3_DUMP, xdr_void, argp, xdr_mountlist, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


void *
mountproc3_umnt_3(argp, clnt)
	dirpath *argp;
	CLIENT *clnt;
{
	static char res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, MOUNTPROC3_UMNT, xdr_dirpath, argp, xdr_void, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return ((void *)&res);
}


void *
mountproc3_umntall_3(argp, clnt)
	void *argp;
	CLIENT *clnt;
{
	static char res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, MOUNTPROC3_UMNTALL, xdr_void, argp, xdr_void, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return ((void *)&res);
}


exports *
mountproc3_export_3(argp, clnt)
	void *argp;
	CLIENT *clnt;
{
	static exports res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, MOUNTPROC3_EXPORT, xdr_void, argp, xdr_exports, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}

void 
nfs_clnt_destroy(clnt)
	CLIENT *clnt;
{
	clnt_destroy(clnt);
}

void 
nfs_auth_destroy(auth)
	AUTH *auth;
{
	auth_destroy(auth);
}