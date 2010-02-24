/*
 * NekoDrive
 * 2010 by Mirko Gatto
 * mirko.gatto@gmail.com
 *
 *
 * Users may use, copy or modify this library 
 * according GNU General Public License v3 (http://www.gnu.org/licenses/gpl.html)
 */


#ifndef __IncNFSV3MountProtocolh
#define __IncNFSV3MountProtocolh

#pragma pack(1) 

#include <rpc/types.h>
#define FHSIZE 64

struct fhandle3 {
	struct {
		u_int data_len;
		char *data_val;
	} data;
};
typedef struct fhandle3 fhandle3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fhandle3(...);
}
#else
bool_t xdr_fhandle3();
#endif


typedef char *dirpath;
#ifdef __cplusplus
extern "C" {
bool_t xdr_dirpath(...);
}
#else
bool_t xdr_dirpath();
#endif


typedef char *name;
#ifdef __cplusplus
extern "C" {
bool_t xdr_name(...);
}
#else
bool_t xdr_name();
#endif


enum mountstat3 {
	MNT3_OK = 0,
	MNT3ERR_PERM = 1,
	MNT3ERR_NOENT = 2,
	MNT3ERR_IO = 5,
	MNT3ERR_ACCES = 13,
	MNT3ERR_NOTDIR = 20,
	MNT3ERR_INVAL = 22,
	MNT3ERR_NAMETOOLONG = 63,
	MNT3ERR_NOTSUPP = 10004,
	MNT3ERR_SERVERFAULT = 10006,
};
typedef enum mountstat3 mountstat3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_mountstat3(...);
}
#else
bool_t xdr_mountstat3();
#endif


struct mountres3_ok {
	fhandle3 fhandle;
	struct {
		u_int auth_flavors_len;
		int *auth_flavors_val;
	} auth_flavors;
};
typedef struct mountres3_ok mountres3_ok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_mountres3_ok(...);
}
#else
bool_t xdr_mountres3_ok();
#endif


struct mountres3 {
	mountstat3 fhs_status;
	union {
		mountres3_ok mountinfo;
	} mountres3_u;
};
typedef struct mountres3 mountres3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_mountres3(...);
}
#else
bool_t xdr_mountres3();
#endif


typedef struct mountbody *mountlist;
#ifdef __cplusplus
extern "C" {
bool_t xdr_mountlist(...);
}
#else
bool_t xdr_mountlist();
#endif


struct mountbody {
	name ml_hostname;
	dirpath ml_directory;
	mountlist ml_next;
};
typedef struct mountbody mountbody;
#ifdef __cplusplus
extern "C" {
bool_t xdr_mountbody(...);
}
#else
bool_t xdr_mountbody();
#endif


typedef struct groupnode *groups;
#ifdef __cplusplus
extern "C" {
bool_t xdr_groups(...);
}
#else
bool_t xdr_groups();
#endif


struct groupnode {
	name gr_name;
	groups gr_next;
};
typedef struct groupnode groupnode;
#ifdef __cplusplus
extern "C" {
bool_t xdr_groupnode(...);
}
#else
bool_t xdr_groupnode();
#endif


typedef struct exportnode *exports;
#ifdef __cplusplus
extern "C" {
bool_t xdr_exports(...);
}
#else
bool_t xdr_exports();
#endif


struct exportnode {
	dirpath ex_dir;
	groups ex_groups;
	exports ex_next;
};
typedef struct exportnode exportnode;
#ifdef __cplusplus
extern "C" {
bool_t xdr_exportnode(...);
}
#else
bool_t xdr_exportnode();
#endif


#define MOUNT_PROGRAM ((u_long)100005)
#define MOUNT_V3 ((u_long)3)
#define MOUNTPROC3_NULL ((u_long)0)
#ifdef __cplusplus
extern "C" {
extern void *mountproc3_null_3(...);
}
#else
extern void *mountproc3_null_3();
#endif /* __cplusplus */
#define MOUNTPROC3_MNT ((u_long)1)
#ifdef __cplusplus
extern "C" {
extern mountres3 *mountproc3_mnt_3(...);
}
#else
extern mountres3 *mountproc3_mnt_3();
#endif /* __cplusplus */
#define MOUNTPROC3_DUMP ((u_long)2)
#ifdef __cplusplus
extern "C" {
extern mountlist *mountproc3_dump_3(...);
}
#else
extern mountlist *mountproc3_dump_3();
#endif /* __cplusplus */
#define MOUNTPROC3_UMNT ((u_long)3)
#ifdef __cplusplus
extern "C" {
extern void *mountproc3_umnt_3(...);
}
#else
extern void *mountproc3_umnt_3();
#endif /* __cplusplus */
#define MOUNTPROC3_UMNTALL ((u_long)4)
#ifdef __cplusplus
extern "C" {
extern void *mountproc3_umntall_3(...);
}
#else
extern void *mountproc3_umntall_3();
#endif /* __cplusplus */
#define MOUNTPROC3_EXPORT ((u_long)5)
#ifdef __cplusplus
extern "C" {
extern exports *mountproc3_export_3(...);
}
#else
extern exports *mountproc3_export_3();
#endif /* __cplusplus */

#endif