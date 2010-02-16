/*
 * NekoDrive
 * 2010 by Mirko Gatto
 * mirko.gatto@gmail.com
 *
 *
 * Users may use, copy or modify this library 
 * according GNU General Public License v3 (http://www.gnu.org/licenses/gpl.html)
 */
#ifndef __IncNFSCommonh
#define __IncNFSCommonh

#pragma pack(1) 

#include <rpc/rpc.h>

#ifdef __cplusplus
extern "C" {
void nfs_clnt_destroy(...);
}
#else
void nfs_clnt_destroy();
#endif

#ifdef __cplusplus
extern "C" {
void nfs_auth_destroy(...);
}
#else
void nfs_auth_destroy();
#endif

#endif