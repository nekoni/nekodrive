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