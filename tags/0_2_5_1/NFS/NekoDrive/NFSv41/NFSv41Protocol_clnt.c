#include <rpc/rpc.h>
#include "NFSv41Protocol.h"

/* Default timeout can be changed using clnt_control() */
static struct timeval TIMEOUT = { 25, 0 };

void *
nfsproc4_null_4(argp, clnt)
	void *argp;
	CLIENT *clnt;
{
	static char res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC4_NULL, xdr_void, argp, xdr_void, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return ((void *)&res);
}


COMPOUND4res *
nfsproc4_compound_4(argp, clnt)
	COMPOUND4args *argp;
	CLIENT *clnt;
{
	static COMPOUND4res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, NFSPROC4_COMPOUND, xdr_COMPOUND4args, argp, xdr_COMPOUND4res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}


void *
cb_null_1(argp, clnt)
	void *argp;
	CLIENT *clnt;
{
	static char res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, CB_NULL, xdr_void, argp, xdr_void, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return ((void *)&res);
}


CB_COMPOUND4res *
cb_compound_1(argp, clnt)
	CB_COMPOUND4args *argp;
	CLIENT *clnt;
{
	static CB_COMPOUND4res res;

	bzero((char *)&res, sizeof(res));
	if (clnt_call(clnt, CB_COMPOUND, xdr_CB_COMPOUND4args, argp, xdr_CB_COMPOUND4res, &res, TIMEOUT) != RPC_SUCCESS) {
		return (NULL);
	}
	return (&res);
}

