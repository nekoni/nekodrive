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


#ifndef __IncNFSV2Protocolh
#define __IncNFSV2Protocolh

#pragma pack(1) 

#include <rpc/types.h>
#define MAXDATA 8192
#define MAXPATHLEN 1024
#define MAXNAMLEN 255
#define COOKIESIZE 4
#define FHSIZE 32
#define FIFO_DEV -1
#define MODE_FMT 0170000
#define MODE_DIR 0040000
#define MODE_CHR 0020000
#define MODE_BLK 0060000
#define MODE_REG 0100000
#define MODE_LNK 0120000
#define MODE_SOCK 0140000
#define MODE_FIFO 0010000

enum nfsstat {
	NFS_OK = 0,
	NFSERR_PERM = 1,
	NFSERR_NOENT = 2,
	NFSERR_IO = 5,
	NFSERR_NXIO = 6,
	NFSERR_ACCES = 13,
	NFSERR_EXIST = 17,
	NFSERR_NODEV = 19,
	NFSERR_NOTDIR = 20,
	NFSERR_ISDIR = 21,
	NFSERR_FBIG = 27,
	NFSERR_NOSPC = 28,
	NFSERR_ROFS = 30,
	NFSERR_NAMETOOLONG = 63,
	NFSERR_NOTEMPTY = 66,
	NFSERR_DQUOT = 69,
	NFSERR_STALE = 70,
	NFSERR_WFLUSH = 99,
};
typedef enum nfsstat nfsstat;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfsstat(...);
}
#else
bool_t xdr_nfsstat();
#endif


enum ftype {
	NFNON = 0,
	NFREG = 1,
	NFDIR = 2,
	NFBLK = 3,
	NFCHR = 4,
	NFLNK = 5,
};
typedef enum ftype ftype;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ftype(...);
}
#else
bool_t xdr_ftype();
#endif


typedef char nfshandle[FHSIZE];
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfshandle(...);
}
#else
bool_t xdr_nfshandle();
#endif


typedef char *filename;
#ifdef __cplusplus
extern "C" {
bool_t xdr_filename(...);
}
#else
bool_t xdr_filename();
#endif


typedef char *path;
#ifdef __cplusplus
extern "C" {
bool_t xdr_path(...);
}
#else
bool_t xdr_path();
#endif


typedef u_int nfscookie;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfscookie(...);
}
#else
bool_t xdr_nfscookie();
#endif


struct nfstimeval {
	u_int seconds;
	u_int useconds;
};
typedef struct nfstimeval nfstimeval;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfstimeval(...);
}
#else
bool_t xdr_nfstimeval();
#endif


struct fattr {
	ftype type;
	u_int mode;
	u_int nlink;
	u_int uid;
	u_int gid;
	u_int size;
	u_int blocksize;
	u_int rdev;
	u_int blocks;
	u_int fsid;
	u_int fileid;
	nfstimeval atime;
	nfstimeval mtime;
	nfstimeval ctime;
};
typedef struct fattr fattr;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr(...);
}
#else
bool_t xdr_fattr();
#endif


struct sattr {
	u_int mode;
	u_int uid;
	u_int gid;
	u_int size;
	nfstimeval atime;
	nfstimeval mtime;
};
typedef struct sattr sattr;
#ifdef __cplusplus
extern "C" {
bool_t xdr_sattr(...);
}
#else
bool_t xdr_sattr();
#endif


struct attrstat {
	nfsstat status;
	union {
		fattr attributes;
	} attrstat_u;
};
typedef struct attrstat attrstat;
#ifdef __cplusplus
extern "C" {
bool_t xdr_attrstat(...);
}
#else
bool_t xdr_attrstat();
#endif


struct diropargs {
	nfshandle dir;
	filename name;
};
typedef struct diropargs diropargs;
#ifdef __cplusplus
extern "C" {
bool_t xdr_diropargs(...);
}
#else
bool_t xdr_diropargs();
#endif


struct diropok {
	nfshandle file;
	fattr attributes;
};
typedef struct diropok diropok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_diropok(...);
}
#else
bool_t xdr_diropok();
#endif


struct diropres {
	nfsstat status;
	union {
		diropok ok;
	} diropres_u;
};
typedef struct diropres diropres;
#ifdef __cplusplus
extern "C" {
bool_t xdr_diropres(...);
}
#else
bool_t xdr_diropres();
#endif


struct sattrargs {
	nfshandle file;
	sattr attributes;
};
typedef struct sattrargs sattrargs;
#ifdef __cplusplus
extern "C" {
bool_t xdr_sattrargs(...);
}
#else
bool_t xdr_sattrargs();
#endif


struct readlinkres {
	nfsstat status;
	union {
		path data;
	} readlinkres_u;
};
typedef struct readlinkres readlinkres;
#ifdef __cplusplus
extern "C" {
bool_t xdr_readlinkres(...);
}
#else
bool_t xdr_readlinkres();
#endif


struct readargs {
	nfshandle file;
	u_int offset;
	u_int count;
	u_int totalcount;
};
typedef struct readargs readargs;
#ifdef __cplusplus
extern "C" {
bool_t xdr_readargs(...);
}
#else
bool_t xdr_readargs();
#endif


struct readresok {
	fattr attributes;
	struct {
		u_int data_len;
		char *data_val;
	} data;
};
typedef struct readresok readresok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_readresok(...);
}
#else
bool_t xdr_readresok();
#endif


struct readres {
	nfsstat status;
	union {
		readresok ok;
	} readres_u;
};
typedef struct readres readres;
#ifdef __cplusplus
extern "C" {
bool_t xdr_readres(...);
}
#else
bool_t xdr_readres();
#endif


struct writeargs {
	nfshandle file;
	u_int beginoffset;
	u_int offset;
	u_int totalcount;
	struct {
		u_int data_len;
		char *data_val;
	} data;
};
typedef struct writeargs writeargs;
#ifdef __cplusplus
extern "C" {
bool_t xdr_writeargs(...);
}
#else
bool_t xdr_writeargs();
#endif


struct createargs {
	diropargs where;
	sattr attributes;
};
typedef struct createargs createargs;
#ifdef __cplusplus
extern "C" {
bool_t xdr_createargs(...);
}
#else
bool_t xdr_createargs();
#endif


struct renameargs {
	diropargs from;
	diropargs to;
};
typedef struct renameargs renameargs;
#ifdef __cplusplus
extern "C" {
bool_t xdr_renameargs(...);
}
#else
bool_t xdr_renameargs();
#endif


struct linkargs {
	nfshandle from;
	diropargs to;
};
typedef struct linkargs linkargs;
#ifdef __cplusplus
extern "C" {
bool_t xdr_linkargs(...);
}
#else
bool_t xdr_linkargs();
#endif


struct symlinkargs {
	diropargs from;
	path to;
	sattr attributes;
};
typedef struct symlinkargs symlinkargs;
#ifdef __cplusplus
extern "C" {
bool_t xdr_symlinkargs(...);
}
#else
bool_t xdr_symlinkargs();
#endif


struct readdirargs {
	nfshandle dir;
	nfscookie cookie;
	u_int count;
};
typedef struct readdirargs readdirargs;
#ifdef __cplusplus
extern "C" {
bool_t xdr_readdirargs(...);
}
#else
bool_t xdr_readdirargs();
#endif


struct entry {
	u_int fileid;
	filename name;
	nfscookie cookie;
	struct entry *nextentry;
};
typedef struct entry entry;
#ifdef __cplusplus
extern "C" {
bool_t xdr_entry(...);
}
#else
bool_t xdr_entry();
#endif


struct readdirok {
	entry *entries;
	bool_t eof;
};
typedef struct readdirok readdirok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_readdirok(...);
}
#else
bool_t xdr_readdirok();
#endif


struct readdirres {
	nfsstat status;
	union {
		readdirok ok;
	} readdirres_u;
};
typedef struct readdirres readdirres;
#ifdef __cplusplus
extern "C" {
bool_t xdr_readdirres(...);
}
#else
bool_t xdr_readdirres();
#endif


struct info {
	u_int tsize;
	u_int bsize;
	u_int blocks;
	u_int bfree;
	u_int bavail;
};
typedef struct info info;
#ifdef __cplusplus
extern "C" {
bool_t xdr_info(...);
}
#else
bool_t xdr_info();
#endif


struct statfsres {
	nfsstat status;
	union {
		info ok;
	} statfsres_u;
};
typedef struct statfsres statfsres;
#ifdef __cplusplus
extern "C" {
bool_t xdr_statfsres(...);
}
#else
bool_t xdr_statfsres();
#endif


#define NFS_PROGRAM ((u_long)100003)
#define NFS_VERSION ((u_long)2)
#define NFSPROC_NULL ((u_long)0)
#ifdef __cplusplus
extern "C" {
extern void *nfsproc_null_2(...);
}
#else
extern void *nfsproc_null_2();
#endif /* __cplusplus */
#define NFSPROC_GETATTR ((u_long)1)
#ifdef __cplusplus
extern "C" {
extern attrstat *nfsproc_getattr_2(...);
}
#else
extern attrstat *nfsproc_getattr_2();
#endif /* __cplusplus */
#define NFSPROC_SETATTR ((u_long)2)
#ifdef __cplusplus
extern "C" {
extern attrstat *nfsproc_setattr_2(...);
}
#else
extern attrstat *nfsproc_setattr_2();
#endif /* __cplusplus */
#define NFSPROC_ROOT ((u_long)3)
#ifdef __cplusplus
extern "C" {
extern void *nfsproc_root_2(...);
}
#else
extern void *nfsproc_root_2();
#endif /* __cplusplus */
#define NFSPROC_LOOKUP ((u_long)4)
#ifdef __cplusplus
extern "C" {
extern diropres *nfsproc_lookup_2(...);
}
#else
extern diropres *nfsproc_lookup_2();
#endif /* __cplusplus */
#define NFSPROC_READLINK ((u_long)5)
#ifdef __cplusplus
extern "C" {
extern readlinkres *nfsproc_readlink_2(...);
}
#else
extern readlinkres *nfsproc_readlink_2();
#endif /* __cplusplus */
#define NFSPROC_READ ((u_long)6)
#ifdef __cplusplus
extern "C" {
extern readres *nfsproc_read_2(...);
}
#else
extern readres *nfsproc_read_2();
#endif /* __cplusplus */
#define NFSPROC_WRITECACHE ((u_long)7)
#ifdef __cplusplus
extern "C" {
extern void *nfsproc_writecache_2(...);
}
#else
extern void *nfsproc_writecache_2();
#endif /* __cplusplus */
#define NFSPROC_WRITE ((u_long)8)
#ifdef __cplusplus
extern "C" {
extern attrstat *nfsproc_write_2(...);
}
#else
extern attrstat *nfsproc_write_2();
#endif /* __cplusplus */
#define NFSPROC_CREATE ((u_long)9)
#ifdef __cplusplus
extern "C" {
extern diropres *nfsproc_create_2(...);
}
#else
extern diropres *nfsproc_create_2();
#endif /* __cplusplus */
#define NFSPROC_REMOVE ((u_long)10)
#ifdef __cplusplus
extern "C" {
extern nfsstat *nfsproc_remove_2(...);
}
#else
extern nfsstat *nfsproc_remove_2();
#endif /* __cplusplus */
#define NFSPROC_RENAME ((u_long)11)
#ifdef __cplusplus
extern "C" {
extern nfsstat *nfsproc_rename_2(...);
}
#else
extern nfsstat *nfsproc_rename_2();
#endif /* __cplusplus */
#define NFSPROC_LINK ((u_long)12)
#ifdef __cplusplus
extern "C" {
extern nfsstat *nfsproc_link_2(...);
}
#else
extern nfsstat *nfsproc_link_2();
#endif /* __cplusplus */
#define NFSPROC_SYMLINK ((u_long)13)
#ifdef __cplusplus
extern "C" {
extern nfsstat *nfsproc_symlink_2(...);
}
#else
extern nfsstat *nfsproc_symlink_2();
#endif /* __cplusplus */
#define NFSPROC_MKDIR ((u_long)14)
#ifdef __cplusplus
extern "C" {
extern diropres *nfsproc_mkdir_2(...);
}
#else
extern diropres *nfsproc_mkdir_2();
#endif /* __cplusplus */
#define NFSPROC_RMDIR ((u_long)15)
#ifdef __cplusplus
extern "C" {
extern nfsstat *nfsproc_rmdir_2(...);
}
#else
extern nfsstat *nfsproc_rmdir_2();
#endif /* __cplusplus */
#define NFSPROC_READDIR ((u_long)16)
#ifdef __cplusplus
extern "C" {
extern readdirres *nfsproc_readdir_2(...);
}
#else
extern readdirres *nfsproc_readdir_2();
#endif /* __cplusplus */
#define NFSPROC_STATFS ((u_long)17)
#ifdef __cplusplus
extern "C" {
extern statfsres *nfsproc_statfs_2(...);
}
#else
extern statfsres *nfsproc_statfs_2();
#endif /* __cplusplus */

#endif
