#ifndef __IncNFSV2MOUNTh
#define __IncNFSV2MOUNTh

#include <rpc/types.h>
#define MNTPATHLEN 1024
#define MNTNAMLEN 255
#define FHSIZE 32


typedef char fhandle[FHSIZE];
#ifdef __cplusplus
extern "C" {
bool_t xdr_fhandle(...);
}
#else
bool_t xdr_fhandle();
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


struct fhstatus {
	u_int status;
	union {
		fhandle directory;
	} fhstatus_u;
};
typedef struct fhstatus fhstatus;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fhstatus(...);
}
#else
bool_t xdr_fhstatus();
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
	name hostname;
	dirpath directory;
	mountlist nextentry;
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
	name grname;
	groups grnext;
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
	dirpath filesys;
	groups exgroups;
	exports next;
};
typedef struct exportnode exportnode;
#ifdef __cplusplus
extern "C" {
bool_t xdr_exportnode(...);
}
#else
bool_t xdr_exportnode();
#endif


#define MOUNTPROG ((u_long)100005)
#define MOUNTVERS ((u_long)1)
#define MOUNTPROC_NULL ((u_long)0)
#ifdef __cplusplus
extern "C" {
extern void *mountproc_null_1(...);
}
#else
extern void *mountproc_null_1();
#endif /* __cplusplus */
#define MOUNTPROC_MNT ((u_long)1)
#ifdef __cplusplus
extern "C" {
extern fhstatus *mountproc_mnt_1(...);
}
#else
extern fhstatus *mountproc_mnt_1();
#endif /* __cplusplus */
#define MOUNTPROC_DUMP ((u_long)2)
#ifdef __cplusplus
extern "C" {
extern mountlist *mountproc_dump_1(...);
}
#else
extern mountlist *mountproc_dump_1();
#endif /* __cplusplus */
#define MOUNTPROC_UMNT ((u_long)3)
#ifdef __cplusplus
extern "C" {
extern void *mountproc_umnt_1(...);
}
#else
extern void *mountproc_umnt_1();
#endif /* __cplusplus */
#define MOUNTPROC_UMNTALL ((u_long)4)
#ifdef __cplusplus
extern "C" {
extern void *mountproc_umntall_1(...);
}
#else
extern void *mountproc_umntall_1();
#endif /* __cplusplus */
#define MOUNTPROC_EXPORT ((u_long)5)
#ifdef __cplusplus
extern "C" {
extern exports *mountproc_export_1(...);
}
#else
extern exports *mountproc_export_1();
#endif /* __cplusplus */
#endif

