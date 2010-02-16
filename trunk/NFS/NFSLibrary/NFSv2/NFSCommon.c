
#include "NFSCommon.h"

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