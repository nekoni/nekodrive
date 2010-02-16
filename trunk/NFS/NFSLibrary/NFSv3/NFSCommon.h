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