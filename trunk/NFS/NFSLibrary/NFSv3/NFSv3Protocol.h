/*
 * NekoDrive
 * 2010 by Mirko Gatto
 * mirko.gatto@gmail.com
 *
 * NFS Protocol V2 RPC Calls
 *
 * Users may use, copy or modify this library 
 * according GNU General Public License v3 (http://www.gnu.org/licenses/gpl.html)
 */


#ifndef __IncNFSV3Protocolh
#define __IncNFSV3Protocolh
#include <rpc/types.h>
#define FHSIZE 64
#define COOKIEVERFSIZE 8
#define CREATEVERFSIZE 8
#define WRITEVERFSIZE 8
#define MODE_FMT 0170000
#define MODE_DIR 0040000
#define MODE_CHR 0020000
#define MODE_BLK 0060000
#define MODE_REG 0100000
#define MODE_LNK 0120000
#define MODE_SOCK 0140000
#define MODE_FIFO 0010000

#pragma pack(1) 


typedef unsigned __int64 u_hyper;

typedef u_hyper uint64;
#ifdef __cplusplus
extern "C" {
bool_t xdr_uint64(...);
}
#else
bool_t xdr_uint64();
#endif


typedef __int64 int64;
#ifdef __cplusplus
extern "C" {
bool_t xdr_int64(...);
}
#else
bool_t xdr_int64();
#endif


typedef u_long uint32;
#ifdef __cplusplus
extern "C" {
bool_t xdr_uint32(...);
}
#else
bool_t xdr_uint32();
#endif


typedef long int32;
#ifdef __cplusplus
extern "C" {
bool_t xdr_int32(...);
}
#else
bool_t xdr_int32();
#endif


typedef char *filename3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_filename3(...);
}
#else
bool_t xdr_filename3();
#endif


typedef char *nfspath3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfspath3(...);
}
#else
bool_t xdr_nfspath3();
#endif


typedef uint64 fileid3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fileid3(...);
}
#else
bool_t xdr_fileid3();
#endif


typedef uint64 cookie3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_cookie3(...);
}
#else
bool_t xdr_cookie3();
#endif


typedef char cookieverf3[8];
#ifdef __cplusplus
extern "C" {
bool_t xdr_cookieverf3(...);
}
#else
bool_t xdr_cookieverf3();
#endif


typedef char createverf3[8];
#ifdef __cplusplus
extern "C" {
bool_t xdr_createverf3(...);
}
#else
bool_t xdr_createverf3();
#endif


typedef char writeverf3[8];
#ifdef __cplusplus
extern "C" {
bool_t xdr_writeverf3(...);
}
#else
bool_t xdr_writeverf3();
#endif


typedef uint32 uid3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_uid3(...);
}
#else
bool_t xdr_uid3();
#endif


typedef uint32 gid3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_gid3(...);
}
#else
bool_t xdr_gid3();
#endif


typedef uint64 size3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_size3(...);
}
#else
bool_t xdr_size3();
#endif


typedef uint64 offset3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_offset3(...);
}
#else
bool_t xdr_offset3();
#endif


typedef uint32 mode3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_mode3(...);
}
#else
bool_t xdr_mode3();
#endif


typedef uint32 count3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_count3(...);
}
#else
bool_t xdr_count3();
#endif


enum nfsstat3 {
	NFS3_OK = 0,
	NFS3ERR_PERM = 1,
	NFS3ERR_NOENT = 2,
	NFS3ERR_IO = 5,
	NFS3ERR_NXIO = 6,
	NFS3ERR_ACCES = 13,
	NFS3ERR_EXIST = 17,
	NFS3ERR_XDEV = 18,
	NFS3ERR_NODEV = 19,
	NFS3ERR_NOTDIR = 20,
	NFS3ERR_ISDIR = 21,
	NFS3ERR_INVAL = 22,
	NFS3ERR_FBIG = 27,
	NFS3ERR_NOSPC = 28,
	NFS3ERR_ROFS = 30,
	NFS3ERR_MLINK = 31,
	NFS3ERR_NAMETOOLONG = 63,
	NFS3ERR_NOTEMPTY = 66,
	NFS3ERR_DQUOT = 69,
	NFS3ERR_STALE = 70,
	NFS3ERR_REMOTE = 71,
	NFS3ERR_BADHANDLE = 10001,
	NFS3ERR_NOT_SYNC = 10002,
	NFS3ERR_BAD_COOKIE = 10003,
	NFS3ERR_NOTSUPP = 10004,
	NFS3ERR_TOOSMALL = 10005,
	NFS3ERR_SERVERFAULT = 10006,
	NFS3ERR_BADTYPE = 10007,
	NFS3ERR_JUKEBOX = 10008,
};
typedef enum nfsstat3 nfsstat3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfsstat3(...);
}
#else
bool_t xdr_nfsstat3();
#endif


enum ftype3 {
	NF3REG = 1,
	NF3DIR = 2,
	NF3BLK = 3,
	NF3CHR = 4,
	NF3LNK = 5,
	NF3SOCK = 6,
	NF3FIFO = 7,
};
typedef enum ftype3 ftype3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ftype3(...);
}
#else
bool_t xdr_ftype3();
#endif


struct specdata3 {
	uint32 specdata1;
	uint32 specdata2;
};
typedef struct specdata3 specdata3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_specdata3(...);
}
#else
bool_t xdr_specdata3();
#endif


struct nfs_fh3 {
	struct {
		u_int data_len;
		char *data_val;
	} data;
};
typedef struct nfs_fh3 nfs_fh3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfs_fh3(...);
}
#else
bool_t xdr_nfs_fh3();
#endif


struct nfstime3 {
	uint32 seconds;
	uint32 nseconds;
};
typedef struct nfstime3 nfstime3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfstime3(...);
}
#else
bool_t xdr_nfstime3();
#endif


struct fattr3 {
	ftype3 type;
	mode3 mode;
	uint32 nlink;
	uid3 uid;
	gid3 gid;
	size3 size;
	size3 used;
	specdata3 rdev;
	uint64 fsid;
	fileid3 fileid;
	nfstime3 atime;
	nfstime3 mtime;
	nfstime3 ctime;
};
typedef struct fattr3 fattr3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr3(...);
}
#else
bool_t xdr_fattr3();
#endif


struct post_op_attr {
	bool_t attributes_follow;
	union {
		fattr3 attributes;
	} post_op_attr_u;
};
typedef struct post_op_attr post_op_attr;
#ifdef __cplusplus
extern "C" {
bool_t xdr_post_op_attr(...);
}
#else
bool_t xdr_post_op_attr();
#endif


struct wcc_attr {
	size3 size;
	nfstime3 mtime;
	nfstime3 ctime;
};
typedef struct wcc_attr wcc_attr;
#ifdef __cplusplus
extern "C" {
bool_t xdr_wcc_attr(...);
}
#else
bool_t xdr_wcc_attr();
#endif


struct pre_op_attr {
	bool_t attributes_follow;
	union {
		wcc_attr attributes;
	} pre_op_attr_u;
};
typedef struct pre_op_attr pre_op_attr;
#ifdef __cplusplus
extern "C" {
bool_t xdr_pre_op_attr(...);
}
#else
bool_t xdr_pre_op_attr();
#endif


struct wcc_data {
	pre_op_attr before;
	post_op_attr after;
};
typedef struct wcc_data wcc_data;
#ifdef __cplusplus
extern "C" {
bool_t xdr_wcc_data(...);
}
#else
bool_t xdr_wcc_data();
#endif


struct post_op_fh3 {
	bool_t handle_follows;
	union {
		nfs_fh3 handle;
	} post_op_fh3_u;
};
typedef struct post_op_fh3 post_op_fh3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_post_op_fh3(...);
}
#else
bool_t xdr_post_op_fh3();
#endif


enum time_how {
	DONT_CHANGE = 0,
	SET_TO_SERVER_TIME = 1,
	SET_TO_CLIENT_TIME = 2,
};
typedef enum time_how time_how;
#ifdef __cplusplus
extern "C" {
bool_t xdr_time_how(...);
}
#else
bool_t xdr_time_how();
#endif


struct set_mode3 {
	bool_t set_it;
	union {
		mode3 mode;
	} set_mode3_u;
};
typedef struct set_mode3 set_mode3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_set_mode3(...);
}
#else
bool_t xdr_set_mode3();
#endif


struct set_uid3 {
	bool_t set_it;
	union {
		uid3 uid;
	} set_uid3_u;
};
typedef struct set_uid3 set_uid3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_set_uid3(...);
}
#else
bool_t xdr_set_uid3();
#endif


struct set_gid3 {
	bool_t set_it;
	union {
		gid3 gid;
	} set_gid3_u;
};
typedef struct set_gid3 set_gid3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_set_gid3(...);
}
#else
bool_t xdr_set_gid3();
#endif


struct set_size3 {
	bool_t set_it;
	union {
		size3 size;
	} set_size3_u;
};
typedef struct set_size3 set_size3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_set_size3(...);
}
#else
bool_t xdr_set_size3();
#endif


struct set_atime {
	time_how set_it;
	union {
		nfstime3 atime;
	} set_atime_u;
};
typedef struct set_atime set_atime;
#ifdef __cplusplus
extern "C" {
bool_t xdr_set_atime(...);
}
#else
bool_t xdr_set_atime();
#endif


struct set_mtime {
	time_how set_it;
	union {
		nfstime3 mtime;
	} set_mtime_u;
};
typedef struct set_mtime set_mtime;
#ifdef __cplusplus
extern "C" {
bool_t xdr_set_mtime(...);
}
#else
bool_t xdr_set_mtime();
#endif


struct sattr3 {
	set_mode3 mode;
	set_uid3 uid;
	set_gid3 gid;
	set_size3 size;
	set_atime atime;
	set_mtime mtime;
};
typedef struct sattr3 sattr3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_sattr3(...);
}
#else
bool_t xdr_sattr3();
#endif


struct diropargs3 {
	nfs_fh3 dir;
	filename3 name;
};
typedef struct diropargs3 diropargs3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_diropargs3(...);
}
#else
bool_t xdr_diropargs3();
#endif


struct GETATTR3args {
	nfs_fh3 obj;
};
typedef struct GETATTR3args GETATTR3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GETATTR3args(...);
}
#else
bool_t xdr_GETATTR3args();
#endif


struct GETATTR3resok {
	fattr3 obj_attributes;
};
typedef struct GETATTR3resok GETATTR3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GETATTR3resok(...);
}
#else
bool_t xdr_GETATTR3resok();
#endif


struct GETATTR3res {
	nfsstat3 status;
	union {
		GETATTR3resok resok;
	} GETATTR3res_u;
};
typedef struct GETATTR3res GETATTR3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GETATTR3res(...);
}
#else
bool_t xdr_GETATTR3res();
#endif


struct sattrguard3 {
	bool_t check;
	union {
		nfstime3 obj_ctime;
	} sattrguard3_u;
};
typedef struct sattrguard3 sattrguard3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_sattrguard3(...);
}
#else
bool_t xdr_sattrguard3();
#endif


struct SETATTR3args {
	nfs_fh3 obj;
	sattr3 new_attributes;
	sattrguard3 guard;
};
typedef struct SETATTR3args SETATTR3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SETATTR3args(...);
}
#else
bool_t xdr_SETATTR3args();
#endif


struct SETATTR3resok {
	wcc_data obj_wcc;
};
typedef struct SETATTR3resok SETATTR3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SETATTR3resok(...);
}
#else
bool_t xdr_SETATTR3resok();
#endif


struct SETATTR3resfail {
	wcc_data obj_wcc;
};
typedef struct SETATTR3resfail SETATTR3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SETATTR3resfail(...);
}
#else
bool_t xdr_SETATTR3resfail();
#endif


struct SETATTR3res {
	nfsstat3 status;
	union {
		SETATTR3resok resok;
		SETATTR3resfail resfail;
	} SETATTR3res_u;
};
typedef struct SETATTR3res SETATTR3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SETATTR3res(...);
}
#else
bool_t xdr_SETATTR3res();
#endif


struct LOOKUP3args {
	diropargs3 what;
};
typedef struct LOOKUP3args LOOKUP3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LOOKUP3args(...);
}
#else
bool_t xdr_LOOKUP3args();
#endif


struct LOOKUP3resok {
	nfs_fh3 obj;
	post_op_attr obj_attributes;
	post_op_attr dir_attributes;
};
typedef struct LOOKUP3resok LOOKUP3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LOOKUP3resok(...);
}
#else
bool_t xdr_LOOKUP3resok();
#endif


struct LOOKUP3resfail {
	post_op_attr dir_attributes;
};
typedef struct LOOKUP3resfail LOOKUP3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LOOKUP3resfail(...);
}
#else
bool_t xdr_LOOKUP3resfail();
#endif


struct LOOKUP3res {
	nfsstat3 status;
	union {
		LOOKUP3resok resok;
		LOOKUP3resfail resfail;
	} LOOKUP3res_u;
};
typedef struct LOOKUP3res LOOKUP3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LOOKUP3res(...);
}
#else
bool_t xdr_LOOKUP3res();
#endif

#define ACCESS3_READ 0x0001
#define ACCESS3_LOOKUP 0x0002
#define ACCESS3_MODIFY 0x0004
#define ACCESS3_EXTEND 0x0008
#define ACCESS3_DELETE 0x0010
#define ACCESS3_EXECUTE 0x0020

struct ACCESS3args {
	nfs_fh3 obj;
	uint32 access;
};
typedef struct ACCESS3args ACCESS3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ACCESS3args(...);
}
#else
bool_t xdr_ACCESS3args();
#endif


struct ACCESS3resok {
	post_op_attr obj_attributes;
	uint32 access;
};
typedef struct ACCESS3resok ACCESS3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ACCESS3resok(...);
}
#else
bool_t xdr_ACCESS3resok();
#endif


struct ACCESS3resfail {
	post_op_attr obj_attributes;
};
typedef struct ACCESS3resfail ACCESS3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ACCESS3resfail(...);
}
#else
bool_t xdr_ACCESS3resfail();
#endif


struct ACCESS3res {
	nfsstat3 status;
	union {
		ACCESS3resok resok;
		ACCESS3resfail resfail;
	} ACCESS3res_u;
};
typedef struct ACCESS3res ACCESS3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ACCESS3res(...);
}
#else
bool_t xdr_ACCESS3res();
#endif


struct READLINK3args {
	nfs_fh3 symlink;
};
typedef struct READLINK3args READLINK3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READLINK3args(...);
}
#else
bool_t xdr_READLINK3args();
#endif


struct READLINK3resok {
	post_op_attr symlink_attributes;
	nfspath3 data;
};
typedef struct READLINK3resok READLINK3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READLINK3resok(...);
}
#else
bool_t xdr_READLINK3resok();
#endif


struct READLINK3resfail {
	post_op_attr symlink_attributes;
};
typedef struct READLINK3resfail READLINK3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READLINK3resfail(...);
}
#else
bool_t xdr_READLINK3resfail();
#endif


struct READLINK3res {
	nfsstat3 status;
	union {
		READLINK3resok resok;
		READLINK3resfail resfail;
	} READLINK3res_u;
};
typedef struct READLINK3res READLINK3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READLINK3res(...);
}
#else
bool_t xdr_READLINK3res();
#endif


struct READ3args {
	nfs_fh3 file;
	offset3 offset;
	count3 count;
};
typedef struct READ3args READ3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READ3args(...);
}
#else
bool_t xdr_READ3args();
#endif


struct READ3resok {
	post_op_attr file_attributes;
	count3 count;
	bool_t eof;
	struct {
		u_int data_len;
		char *data_val;
	} data;
};
typedef struct READ3resok READ3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READ3resok(...);
}
#else
bool_t xdr_READ3resok();
#endif


struct READ3resfail {
	post_op_attr file_attributes;
};
typedef struct READ3resfail READ3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READ3resfail(...);
}
#else
bool_t xdr_READ3resfail();
#endif


struct READ3res {
	nfsstat3 status;
	union {
		READ3resok resok;
		READ3resfail resfail;
	} READ3res_u;
};
typedef struct READ3res READ3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READ3res(...);
}
#else
bool_t xdr_READ3res();
#endif


enum stable_how {
	UNSTABLE = 0,
	DATA_SYNC = 1,
	FILE_SYNC = 2,
};
typedef enum stable_how stable_how;
#ifdef __cplusplus
extern "C" {
bool_t xdr_stable_how(...);
}
#else
bool_t xdr_stable_how();
#endif


struct WRITE3args {
	nfs_fh3 file;
	offset3 offset;
	count3 count;
	stable_how stable;
	struct {
		u_int data_len;
		char *data_val;
	} data;
};
typedef struct WRITE3args WRITE3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_WRITE3args(...);
}
#else
bool_t xdr_WRITE3args();
#endif


struct WRITE3resok {
	wcc_data file_wcc;
	count3 count;
	stable_how committed;
	writeverf3 verf;
};
typedef struct WRITE3resok WRITE3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_WRITE3resok(...);
}
#else
bool_t xdr_WRITE3resok();
#endif


struct WRITE3resfail {
	wcc_data file_wcc;
};
typedef struct WRITE3resfail WRITE3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_WRITE3resfail(...);
}
#else
bool_t xdr_WRITE3resfail();
#endif


struct WRITE3res {
	nfsstat3 status;
	union {
		WRITE3resok resok;
		WRITE3resfail resfail;
	} WRITE3res_u;
};
typedef struct WRITE3res WRITE3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_WRITE3res(...);
}
#else
bool_t xdr_WRITE3res();
#endif


enum createmode3 {
	UNCHECKED = 0,
	GUARDED = 1,
	EXCLUSIVE = 2,
};
typedef enum createmode3 createmode3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_createmode3(...);
}
#else
bool_t xdr_createmode3();
#endif


struct createhow3 {
	createmode3 mode;
	union {
		sattr3 obj_attributes;
		createverf3 verf;
	} createhow3_u;
};
typedef struct createhow3 createhow3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_createhow3(...);
}
#else
bool_t xdr_createhow3();
#endif


struct CREATE3args {
	diropargs3 where;
	createhow3 how;
};
typedef struct CREATE3args CREATE3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CREATE3args(...);
}
#else
bool_t xdr_CREATE3args();
#endif


struct CREATE3resok {
	post_op_fh3 obj;
	post_op_attr obj_attributes;
	wcc_data dir_wcc;
};
typedef struct CREATE3resok CREATE3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CREATE3resok(...);
}
#else
bool_t xdr_CREATE3resok();
#endif


struct CREATE3resfail {
	wcc_data dir_wcc;
};
typedef struct CREATE3resfail CREATE3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CREATE3resfail(...);
}
#else
bool_t xdr_CREATE3resfail();
#endif


struct CREATE3res {
	nfsstat3 status;
	union {
		CREATE3resok resok;
		CREATE3resfail resfail;
	} CREATE3res_u;
};
typedef struct CREATE3res CREATE3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CREATE3res(...);
}
#else
bool_t xdr_CREATE3res();
#endif


struct MKDIR3args {
	diropargs3 where;
	sattr3 attributes;
};
typedef struct MKDIR3args MKDIR3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_MKDIR3args(...);
}
#else
bool_t xdr_MKDIR3args();
#endif


struct MKDIR3resok {
	post_op_fh3 obj;
	post_op_attr obj_attributes;
	wcc_data dir_wcc;
};
typedef struct MKDIR3resok MKDIR3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_MKDIR3resok(...);
}
#else
bool_t xdr_MKDIR3resok();
#endif


struct MKDIR3resfail {
	wcc_data dir_wcc;
};
typedef struct MKDIR3resfail MKDIR3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_MKDIR3resfail(...);
}
#else
bool_t xdr_MKDIR3resfail();
#endif


struct MKDIR3res {
	nfsstat3 status;
	union {
		MKDIR3resok resok;
		MKDIR3resfail resfail;
	} MKDIR3res_u;
};
typedef struct MKDIR3res MKDIR3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_MKDIR3res(...);
}
#else
bool_t xdr_MKDIR3res();
#endif


struct symlinkdata3 {
	sattr3 symlink_attributes;
	nfspath3 symlink_data;
};
typedef struct symlinkdata3 symlinkdata3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_symlinkdata3(...);
}
#else
bool_t xdr_symlinkdata3();
#endif


struct SYMLINK3args {
	diropargs3 where;
	symlinkdata3 symlink;
};
typedef struct SYMLINK3args SYMLINK3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SYMLINK3args(...);
}
#else
bool_t xdr_SYMLINK3args();
#endif


struct SYMLINK3resok {
	post_op_fh3 obj;
	post_op_attr obj_attributes;
	wcc_data dir_wcc;
};
typedef struct SYMLINK3resok SYMLINK3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SYMLINK3resok(...);
}
#else
bool_t xdr_SYMLINK3resok();
#endif


struct SYMLINK3resfail {
	wcc_data dir_wcc;
};
typedef struct SYMLINK3resfail SYMLINK3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SYMLINK3resfail(...);
}
#else
bool_t xdr_SYMLINK3resfail();
#endif


struct SYMLINK3res {
	nfsstat3 status;
	union {
		SYMLINK3resok resok;
		SYMLINK3resfail resfail;
	} SYMLINK3res_u;
};
typedef struct SYMLINK3res SYMLINK3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SYMLINK3res(...);
}
#else
bool_t xdr_SYMLINK3res();
#endif


struct devicedata3 {
	sattr3 dev_attributes;
	specdata3 spec;
};
typedef struct devicedata3 devicedata3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_devicedata3(...);
}
#else
bool_t xdr_devicedata3();
#endif


struct mknoddata3 {
	ftype3 type;
	union {
		devicedata3 device;
		sattr3 pipe_attributes;
	} mknoddata3_u;
};
typedef struct mknoddata3 mknoddata3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_mknoddata3(...);
}
#else
bool_t xdr_mknoddata3();
#endif


struct MKNOD3args {
	diropargs3 where;
	mknoddata3 what;
};
typedef struct MKNOD3args MKNOD3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_MKNOD3args(...);
}
#else
bool_t xdr_MKNOD3args();
#endif


struct MKNOD3resok {
	post_op_fh3 obj;
	post_op_attr obj_attributes;
	wcc_data dir_wcc;
};
typedef struct MKNOD3resok MKNOD3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_MKNOD3resok(...);
}
#else
bool_t xdr_MKNOD3resok();
#endif


struct MKNOD3resfail {
	wcc_data dir_wcc;
};
typedef struct MKNOD3resfail MKNOD3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_MKNOD3resfail(...);
}
#else
bool_t xdr_MKNOD3resfail();
#endif


struct MKNOD3res {
	nfsstat3 status;
	union {
		MKNOD3resok resok;
		MKNOD3resfail resfail;
	} MKNOD3res_u;
};
typedef struct MKNOD3res MKNOD3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_MKNOD3res(...);
}
#else
bool_t xdr_MKNOD3res();
#endif


struct REMOVE3args {
	diropargs3 obj;
};
typedef struct REMOVE3args REMOVE3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_REMOVE3args(...);
}
#else
bool_t xdr_REMOVE3args();
#endif


struct REMOVE3resok {
	wcc_data dir_wcc;
};
typedef struct REMOVE3resok REMOVE3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_REMOVE3resok(...);
}
#else
bool_t xdr_REMOVE3resok();
#endif


struct REMOVE3resfail {
	wcc_data dir_wcc;
};
typedef struct REMOVE3resfail REMOVE3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_REMOVE3resfail(...);
}
#else
bool_t xdr_REMOVE3resfail();
#endif


struct REMOVE3res {
	nfsstat3 status;
	union {
		REMOVE3resok resok;
		REMOVE3resfail resfail;
	} REMOVE3res_u;
};
typedef struct REMOVE3res REMOVE3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_REMOVE3res(...);
}
#else
bool_t xdr_REMOVE3res();
#endif


struct RMDIR3args {
	diropargs3 obj;
};
typedef struct RMDIR3args RMDIR3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RMDIR3args(...);
}
#else
bool_t xdr_RMDIR3args();
#endif


struct RMDIR3resok {
	wcc_data dir_wcc;
};
typedef struct RMDIR3resok RMDIR3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RMDIR3resok(...);
}
#else
bool_t xdr_RMDIR3resok();
#endif


struct RMDIR3resfail {
	wcc_data dir_wcc;
};
typedef struct RMDIR3resfail RMDIR3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RMDIR3resfail(...);
}
#else
bool_t xdr_RMDIR3resfail();
#endif


struct RMDIR3res {
	nfsstat3 status;
	union {
		RMDIR3resok resok;
		RMDIR3resfail resfail;
	} RMDIR3res_u;
};
typedef struct RMDIR3res RMDIR3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RMDIR3res(...);
}
#else
bool_t xdr_RMDIR3res();
#endif


struct RENAME3args {
	diropargs3 from;
	diropargs3 to;
};
typedef struct RENAME3args RENAME3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RENAME3args(...);
}
#else
bool_t xdr_RENAME3args();
#endif


struct RENAME3resok {
	wcc_data fromdir_wcc;
	wcc_data todir_wcc;
};
typedef struct RENAME3resok RENAME3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RENAME3resok(...);
}
#else
bool_t xdr_RENAME3resok();
#endif


struct RENAME3resfail {
	wcc_data fromdir_wcc;
	wcc_data todir_wcc;
};
typedef struct RENAME3resfail RENAME3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RENAME3resfail(...);
}
#else
bool_t xdr_RENAME3resfail();
#endif


struct RENAME3res {
	nfsstat3 status;
	union {
		RENAME3resok resok;
		RENAME3resfail resfail;
	} RENAME3res_u;
};
typedef struct RENAME3res RENAME3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RENAME3res(...);
}
#else
bool_t xdr_RENAME3res();
#endif


struct LINK3args {
	nfs_fh3 file;
	diropargs3 link;
};
typedef struct LINK3args LINK3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LINK3args(...);
}
#else
bool_t xdr_LINK3args();
#endif


struct LINK3resok {
	post_op_attr file_attributes;
	wcc_data linkdir_wcc;
};
typedef struct LINK3resok LINK3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LINK3resok(...);
}
#else
bool_t xdr_LINK3resok();
#endif


struct LINK3resfail {
	post_op_attr file_attributes;
	wcc_data linkdir_wcc;
};
typedef struct LINK3resfail LINK3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LINK3resfail(...);
}
#else
bool_t xdr_LINK3resfail();
#endif


struct LINK3res {
	nfsstat3 status;
	union {
		LINK3resok resok;
		LINK3resfail resfail;
	} LINK3res_u;
};
typedef struct LINK3res LINK3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LINK3res(...);
}
#else
bool_t xdr_LINK3res();
#endif


struct READDIR3args {
	nfs_fh3 dir;
	cookie3 cookie;
	cookieverf3 cookieverf;
	count3 count;
};
typedef struct READDIR3args READDIR3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READDIR3args(...);
}
#else
bool_t xdr_READDIR3args();
#endif


struct entry3 {
	fileid3 fileid;
	filename3 name;
	cookie3 cookie;
	struct entry3 *nextentry;
};
typedef struct entry3 entry3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_entry3(...);
}
#else
bool_t xdr_entry3();
#endif


struct dirlist3 {
	entry3 *entries;
	bool_t eof;
};
typedef struct dirlist3 dirlist3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_dirlist3(...);
}
#else
bool_t xdr_dirlist3();
#endif


struct READDIR3resok {
	post_op_attr dir_attributes;
	cookieverf3 cookieverf;
	dirlist3 reply;
};
typedef struct READDIR3resok READDIR3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READDIR3resok(...);
}
#else
bool_t xdr_READDIR3resok();
#endif


struct READDIR3resfail {
	post_op_attr dir_attributes;
};
typedef struct READDIR3resfail READDIR3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READDIR3resfail(...);
}
#else
bool_t xdr_READDIR3resfail();
#endif


struct READDIR3res {
	nfsstat3 status;
	union {
		READDIR3resok resok;
		READDIR3resfail resfail;
	} READDIR3res_u;
};
typedef struct READDIR3res READDIR3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READDIR3res(...);
}
#else
bool_t xdr_READDIR3res();
#endif


struct READDIRPLUS3args {
	nfs_fh3 dir;
	cookie3 cookie;
	cookieverf3 cookieverf;
	count3 dircount;
	count3 maxcount;
};
typedef struct READDIRPLUS3args READDIRPLUS3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READDIRPLUS3args(...);
}
#else
bool_t xdr_READDIRPLUS3args();
#endif


struct entryplus3 {
	fileid3 fileid;
	filename3 name;
	cookie3 cookie;
	post_op_attr name_attributes;
	post_op_fh3 name_handle;
	struct entryplus3 *nextentry;
};
typedef struct entryplus3 entryplus3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_entryplus3(...);
}
#else
bool_t xdr_entryplus3();
#endif


struct dirlistplus3 {
	entryplus3 *entries;
	bool_t eof;
};
typedef struct dirlistplus3 dirlistplus3;
#ifdef __cplusplus
extern "C" {
bool_t xdr_dirlistplus3(...);
}
#else
bool_t xdr_dirlistplus3();
#endif


struct READDIRPLUS3resok {
	post_op_attr dir_attributes;
	cookieverf3 cookieverf;
	dirlistplus3 reply;
};
typedef struct READDIRPLUS3resok READDIRPLUS3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READDIRPLUS3resok(...);
}
#else
bool_t xdr_READDIRPLUS3resok();
#endif


struct READDIRPLUS3resfail {
	post_op_attr dir_attributes;
};
typedef struct READDIRPLUS3resfail READDIRPLUS3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READDIRPLUS3resfail(...);
}
#else
bool_t xdr_READDIRPLUS3resfail();
#endif


struct READDIRPLUS3res {
	nfsstat3 status;
	union {
		READDIRPLUS3resok resok;
		READDIRPLUS3resfail resfail;
	} READDIRPLUS3res_u;
};
typedef struct READDIRPLUS3res READDIRPLUS3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READDIRPLUS3res(...);
}
#else
bool_t xdr_READDIRPLUS3res();
#endif


struct FSSTAT3args {
	nfs_fh3 fsroot;
};
typedef struct FSSTAT3args FSSTAT3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_FSSTAT3args(...);
}
#else
bool_t xdr_FSSTAT3args();
#endif


struct FSSTAT3resok {
	post_op_attr obj_attributes;
	size3 tbytes;
	size3 fbytes;
	size3 abytes;
	size3 tfiles;
	size3 ffiles;
	size3 afiles;
	uint32 invarsec;
};
typedef struct FSSTAT3resok FSSTAT3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_FSSTAT3resok(...);
}
#else
bool_t xdr_FSSTAT3resok();
#endif


struct FSSTAT3resfail {
	post_op_attr obj_attributes;
};
typedef struct FSSTAT3resfail FSSTAT3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_FSSTAT3resfail(...);
}
#else
bool_t xdr_FSSTAT3resfail();
#endif


struct FSSTAT3res {
	nfsstat3 status;
	union {
		FSSTAT3resok resok;
		FSSTAT3resfail resfail;
	} FSSTAT3res_u;
};
typedef struct FSSTAT3res FSSTAT3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_FSSTAT3res(...);
}
#else
bool_t xdr_FSSTAT3res();
#endif

#define FSF3_LINK 0x0001
#define FSF3_SYMLINK 0x0002
#define FSF3_HOMOGENEOUS 0x0008
#define FSF3_CANSETTIME 0x0010

struct FSINFO3args {
	nfs_fh3 fsroot;
};
typedef struct FSINFO3args FSINFO3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_FSINFO3args(...);
}
#else
bool_t xdr_FSINFO3args();
#endif


struct FSINFO3resok {
	post_op_attr obj_attributes;
	uint32 rtmax;
	uint32 rtpref;
	uint32 rtmult;
	uint32 wtmax;
	uint32 wtpref;
	uint32 wtmult;
	uint32 dtpref;
	size3 maxfilesize;
	nfstime3 time_delta;
	uint32 properties;
};
typedef struct FSINFO3resok FSINFO3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_FSINFO3resok(...);
}
#else
bool_t xdr_FSINFO3resok();
#endif


struct FSINFO3resfail {
	post_op_attr obj_attributes;
};
typedef struct FSINFO3resfail FSINFO3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_FSINFO3resfail(...);
}
#else
bool_t xdr_FSINFO3resfail();
#endif


struct FSINFO3res {
	nfsstat3 status;
	union {
		FSINFO3resok resok;
		FSINFO3resfail resfail;
	} FSINFO3res_u;
};
typedef struct FSINFO3res FSINFO3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_FSINFO3res(...);
}
#else
bool_t xdr_FSINFO3res();
#endif


struct PATHCONF3args {
	nfs_fh3 obj;
};
typedef struct PATHCONF3args PATHCONF3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_PATHCONF3args(...);
}
#else
bool_t xdr_PATHCONF3args();
#endif


struct PATHCONF3resok {
	post_op_attr obj_attributes;
	uint32 linkmax;
	uint32 name_max;
	bool_t no_trunc;
	bool_t chown_restricted;
	bool_t case_insensitive;
	bool_t case_preserving;
};
typedef struct PATHCONF3resok PATHCONF3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_PATHCONF3resok(...);
}
#else
bool_t xdr_PATHCONF3resok();
#endif


struct PATHCONF3resfail {
	post_op_attr obj_attributes;
};
typedef struct PATHCONF3resfail PATHCONF3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_PATHCONF3resfail(...);
}
#else
bool_t xdr_PATHCONF3resfail();
#endif


struct PATHCONF3res {
	nfsstat3 status;
	union {
		PATHCONF3resok resok;
		PATHCONF3resfail resfail;
	} PATHCONF3res_u;
};
typedef struct PATHCONF3res PATHCONF3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_PATHCONF3res(...);
}
#else
bool_t xdr_PATHCONF3res();
#endif


struct COMMIT3args {
	nfs_fh3 file;
	offset3 offset;
	count3 count;
};
typedef struct COMMIT3args COMMIT3args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_COMMIT3args(...);
}
#else
bool_t xdr_COMMIT3args();
#endif


struct COMMIT3resok {
	wcc_data file_wcc;
	writeverf3 verf;
};
typedef struct COMMIT3resok COMMIT3resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_COMMIT3resok(...);
}
#else
bool_t xdr_COMMIT3resok();
#endif


struct COMMIT3resfail {
	wcc_data file_wcc;
};
typedef struct COMMIT3resfail COMMIT3resfail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_COMMIT3resfail(...);
}
#else
bool_t xdr_COMMIT3resfail();
#endif


struct COMMIT3res {
	nfsstat3 status;
	union {
		COMMIT3resok resok;
		COMMIT3resfail resfail;
	} COMMIT3res_u;
};
typedef struct COMMIT3res COMMIT3res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_COMMIT3res(...);
}
#else
bool_t xdr_COMMIT3res();
#endif


#define NFS_PROGRAM ((u_long)100003)
#define NFS_V3 ((u_long)3)
#define NFSPROC3_NULL ((u_long)0)
#ifdef __cplusplus
extern "C" {
extern void *nfsproc3_null_3(...);
}
#else
extern void *nfsproc3_null_3();
#endif /* __cplusplus */
#define NFSPROC3_GETATTR ((u_long)1)
#ifdef __cplusplus
extern "C" {
extern GETATTR3res *nfsproc3_getattr_3(...);
}
#else
extern GETATTR3res *nfsproc3_getattr_3();
#endif /* __cplusplus */
#define NFSPROC3_SETATTR ((u_long)2)
#ifdef __cplusplus
extern "C" {
extern SETATTR3res *nfsproc3_setattr_3(...);
}
#else
extern SETATTR3res *nfsproc3_setattr_3();
#endif /* __cplusplus */
#define NFSPROC3_LOOKUP ((u_long)3)
#ifdef __cplusplus
extern "C" {
extern LOOKUP3res *nfsproc3_lookup_3(...);
}
#else
extern LOOKUP3res *nfsproc3_lookup_3();
#endif /* __cplusplus */
#define NFSPROC3_ACCESS ((u_long)4)
#ifdef __cplusplus
extern "C" {
extern ACCESS3res *nfsproc3_access_3(...);
}
#else
extern ACCESS3res *nfsproc3_access_3();
#endif /* __cplusplus */
#define NFSPROC3_READLINK ((u_long)5)
#ifdef __cplusplus
extern "C" {
extern READLINK3res *nfsproc3_readlink_3(...);
}
#else
extern READLINK3res *nfsproc3_readlink_3();
#endif /* __cplusplus */
#define NFSPROC3_READ ((u_long)6)
#ifdef __cplusplus
extern "C" {
extern READ3res *nfsproc3_read_3(...);
}
#else
extern READ3res *nfsproc3_read_3();
#endif /* __cplusplus */
#define NFSPROC3_WRITE ((u_long)7)
#ifdef __cplusplus
extern "C" {
extern WRITE3res *nfsproc3_write_3(...);
}
#else
extern WRITE3res *nfsproc3_write_3();
#endif /* __cplusplus */
#define NFSPROC3_CREATE ((u_long)8)
#ifdef __cplusplus
extern "C" {
extern CREATE3res *nfsproc3_create_3(...);
}
#else
extern CREATE3res *nfsproc3_create_3();
#endif /* __cplusplus */
#define NFSPROC3_MKDIR ((u_long)9)
#ifdef __cplusplus
extern "C" {
extern MKDIR3res *nfsproc3_mkdir_3(...);
}
#else
extern MKDIR3res *nfsproc3_mkdir_3();
#endif /* __cplusplus */
#define NFSPROC3_SYMLINK ((u_long)10)
#ifdef __cplusplus
extern "C" {
extern SYMLINK3res *nfsproc3_symlink_3(...);
}
#else
extern SYMLINK3res *nfsproc3_symlink_3();
#endif /* __cplusplus */
#define NFSPROC3_MKNOD ((u_long)11)
#ifdef __cplusplus
extern "C" {
extern MKNOD3res *nfsproc3_mknod_3(...);
}
#else
extern MKNOD3res *nfsproc3_mknod_3();
#endif /* __cplusplus */
#define NFSPROC3_REMOVE ((u_long)12)
#ifdef __cplusplus
extern "C" {
extern REMOVE3res *nfsproc3_remove_3(...);
}
#else
extern REMOVE3res *nfsproc3_remove_3();
#endif /* __cplusplus */
#define NFSPROC3_RMDIR ((u_long)13)
#ifdef __cplusplus
extern "C" {
extern RMDIR3res *nfsproc3_rmdir_3(...);
}
#else
extern RMDIR3res *nfsproc3_rmdir_3();
#endif /* __cplusplus */
#define NFSPROC3_RENAME ((u_long)14)
#ifdef __cplusplus
extern "C" {
extern RENAME3res *nfsproc3_rename_3(...);
}
#else
extern RENAME3res *nfsproc3_rename_3();
#endif /* __cplusplus */
#define NFSPROC3_LINK ((u_long)15)
#ifdef __cplusplus
extern "C" {
extern LINK3res *nfsproc3_link_3(...);
}
#else
extern LINK3res *nfsproc3_link_3();
#endif /* __cplusplus */
#define NFSPROC3_READDIR ((u_long)16)
#ifdef __cplusplus
extern "C" {
extern READDIR3res *nfsproc3_readdir_3(...);
}
#else
extern READDIR3res *nfsproc3_readdir_3();
#endif /* __cplusplus */
#define NFSPROC3_READDIRPLUS ((u_long)17)
#ifdef __cplusplus
extern "C" {
extern READDIRPLUS3res *nfsproc3_readdirplus_3(...);
}
#else
extern READDIRPLUS3res *nfsproc3_readdirplus_3();
#endif /* __cplusplus */
#define NFSPROC3_FSSTAT ((u_long)18)
#ifdef __cplusplus
extern "C" {
extern FSSTAT3res *nfsproc3_fsstat_3(...);
}
#else
extern FSSTAT3res *nfsproc3_fsstat_3();
#endif /* __cplusplus */
#define NFSPROC3_FSINFO ((u_long)19)
#ifdef __cplusplus
extern "C" {
extern FSINFO3res *nfsproc3_fsinfo_3(...);
}
#else
extern FSINFO3res *nfsproc3_fsinfo_3();
#endif /* __cplusplus */
#define NFSPROC3_PATHCONF ((u_long)20)
#ifdef __cplusplus
extern "C" {
extern PATHCONF3res *nfsproc3_pathconf_3(...);
}
#else
extern PATHCONF3res *nfsproc3_pathconf_3();
#endif /* __cplusplus */
#define NFSPROC3_COMMIT ((u_long)21)
#ifdef __cplusplus
extern "C" {
extern COMMIT3res *nfsproc3_commit_3(...);
}
#else
extern COMMIT3res *nfsproc3_commit_3();
#endif /* __cplusplus */

#endif

