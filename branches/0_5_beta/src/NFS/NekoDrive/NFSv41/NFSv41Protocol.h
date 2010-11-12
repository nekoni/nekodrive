#ifndef __IncNFSV41Protocolh
#define __IncNFSV41Protocolh

#pragma pack(1)

#include <rpc/types.h>
#ifndef _AUTH_SYS_DEFINE_FOR_NFSv41
#define _AUTH_SYS_DEFINE_FOR_NFSv41
#include <rpc/auth_uni.h>
typedef struct authunix_parms authsys_parms;
#endif _AUTH_SYS_DEFINE_FOR_NFSv41
#define NFS4_FHSIZE 128
#define NFS4_VERIFIER_SIZE 8
#define NFS4_OPAQUE_LIMIT 1024
#define NFS4_SESSIONID_SIZE 16
#define NFS4_INT64_MAX 0x7fffffffffffffff
#define NFS4_UINT64_MAX 0xffffffffffffffff
#define NFS4_INT32_MAX 0x7fffffff
#define NFS4_UINT32_MAX 0xffffffff
#define NFS4_MAXFILELEN 0xffffffffffffffff
#define NFS4_MAXFILEOFF 0xfffffffffffffffe

enum nfs_ftype4 {
	NF4REG = 1,
	NF4DIR = 2,
	NF4BLK = 3,
	NF4CHR = 4,
	NF4LNK = 5,
	NF4SOCK = 6,
	NF4FIFO = 7,
	NF4ATTRDIR = 8,
	NF4NAMEDATTR = 9,
};
typedef enum nfs_ftype4 nfs_ftype4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfs_ftype4(...);
}
#else
bool_t xdr_nfs_ftype4();
#endif


enum nfsstat4 {
	NFS4_OK = 0,
	NFS4ERR_PERM = 1,
	NFS4ERR_NOENT = 2,
	NFS4ERR_IO = 5,
	NFS4ERR_NXIO = 6,
	NFS4ERR_ACCESS = 13,
	NFS4ERR_EXIST = 17,
	NFS4ERR_XDEV = 18,
	NFS4ERR_NOTDIR = 20,
	NFS4ERR_ISDIR = 21,
	NFS4ERR_INVAL = 22,
	NFS4ERR_FBIG = 27,
	NFS4ERR_NOSPC = 28,
	NFS4ERR_ROFS = 30,
	NFS4ERR_MLINK = 31,
	NFS4ERR_NAMETOOLONG = 63,
	NFS4ERR_NOTEMPTY = 66,
	NFS4ERR_DQUOT = 69,
	NFS4ERR_STALE = 70,
	NFS4ERR_BADHANDLE = 10001,
	NFS4ERR_BAD_COOKIE = 10003,
	NFS4ERR_NOTSUPP = 10004,
	NFS4ERR_TOOSMALL = 10005,
	NFS4ERR_SERVERFAULT = 10006,
	NFS4ERR_BADTYPE = 10007,
	NFS4ERR_DELAY = 10008,
	NFS4ERR_SAME = 10009,
	NFS4ERR_DENIED = 10010,
	NFS4ERR_EXPIRED = 10011,
	NFS4ERR_LOCKED = 10012,
	NFS4ERR_GRACE = 10013,
	NFS4ERR_FHEXPIRED = 10014,
	NFS4ERR_SHARE_DENIED = 10015,
	NFS4ERR_WRONGSEC = 10016,
	NFS4ERR_CLID_INUSE = 10017,
	NFS4ERR_RESOURCE = 10018,
	NFS4ERR_MOVED = 10019,
	NFS4ERR_NOFILEHANDLE = 10020,
	NFS4ERR_MINOR_VERS_MISMATCH = 10021,
	NFS4ERR_STALE_CLIENTID = 10022,
	NFS4ERR_STALE_STATEID = 10023,
	NFS4ERR_OLD_STATEID = 10024,
	NFS4ERR_BAD_STATEID = 10025,
	NFS4ERR_BAD_SEQID = 10026,
	NFS4ERR_NOT_SAME = 10027,
	NFS4ERR_LOCK_RANGE = 10028,
	NFS4ERR_SYMLINK = 10029,
	NFS4ERR_RESTOREFH = 10030,
	NFS4ERR_LEASE_MOVED = 10031,
	NFS4ERR_ATTRNOTSUPP = 10032,
	NFS4ERR_NO_GRACE = 10033,
	NFS4ERR_RECLAIM_BAD = 10034,
	NFS4ERR_RECLAIM_CONFLICT = 10035,
	NFS4ERR_BADXDR = 10036,
	NFS4ERR_LOCKS_HELD = 10037,
	NFS4ERR_OPENMODE = 10038,
	NFS4ERR_BADOWNER = 10039,
	NFS4ERR_BADCHAR = 10040,
	NFS4ERR_BADNAME = 10041,
	NFS4ERR_BAD_RANGE = 10042,
	NFS4ERR_LOCK_NOTSUPP = 10043,
	NFS4ERR_OP_ILLEGAL = 10044,
	NFS4ERR_DEADLOCK = 10045,
	NFS4ERR_FILE_OPEN = 10046,
	NFS4ERR_ADMIN_REVOKED = 10047,
	NFS4ERR_CB_PATH_DOWN = 10048,
	NFS4ERR_BADIOMODE = 10049,
	NFS4ERR_BADLAYOUT = 10050,
	NFS4ERR_BAD_SESSION_DIGEST = 10051,
	NFS4ERR_BADSESSION = 10052,
	NFS4ERR_BADSLOT = 10053,
	NFS4ERR_COMPLETE_ALREADY = 10054,
	NFS4ERR_CONN_NOT_BOUND_TO_SESSION = 10055,
	NFS4ERR_DELEG_ALREADY_WANTED = 10056,
	NFS4ERR_BACK_CHAN_BUSY = 10057,
	NFS4ERR_LAYOUTTRYLATER = 10058,
	NFS4ERR_LAYOUTUNAVAILABLE = 10059,
	NFS4ERR_NOMATCHING_LAYOUT = 10060,
	NFS4ERR_RECALLCONFLICT = 10061,
	NFS4ERR_UNKNOWN_LAYOUTTYPE = 10062,
	NFS4ERR_SEQ_MISORDERED = 10063,
	NFS4ERR_SEQUENCE_POS = 10064,
	NFS4ERR_REQ_TOO_BIG = 10065,
	NFS4ERR_REP_TOO_BIG = 10066,
	NFS4ERR_REP_TOO_BIG_TO_CACHE = 10067,
	NFS4ERR_RETRY_UNCACHED_REP = 10068,
	NFS4ERR_UNSAFE_COMPOUND = 10069,
	NFS4ERR_TOO_MANY_OPS = 10070,
	NFS4ERR_OP_NOT_IN_SESSION = 10071,
	NFS4ERR_HASH_ALG_UNSUPP = 10072,
	NFS4ERR_CLIENTID_BUSY = 10074,
	NFS4ERR_PNFS_IO_HOLE = 10075,
	NFS4ERR_SEQ_FALSE_RETRY = 10076,
	NFS4ERR_BAD_HIGH_SLOT = 10077,
	NFS4ERR_DEADSESSION = 10078,
	NFS4ERR_ENCR_ALG_UNSUPP = 10079,
	NFS4ERR_PNFS_NO_LAYOUT = 10080,
	NFS4ERR_NOT_ONLY_OP = 10081,
	NFS4ERR_WRONG_CRED = 10082,
	NFS4ERR_WRONG_TYPE = 10083,
	NFS4ERR_DIRDELEG_UNAVAIL = 10084,
	NFS4ERR_REJECT_DELEG = 10085,
	NFS4ERR_RETURNCONFLICT = 10086,
	NFS4ERR_DELEG_REVOKED = 10087,
};
typedef enum nfsstat4 nfsstat4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfsstat4(...);
}
#else
bool_t xdr_nfsstat4();
#endif


typedef struct {
	u_int attrlist4_len;
	char *attrlist4_val;
} attrlist4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_attrlist4(...);
}
#else
bool_t xdr_attrlist4();
#endif


typedef struct {
	u_int bitmap4_len;
	uint32_t *bitmap4_val;
} bitmap4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_bitmap4(...);
}
#else
bool_t xdr_bitmap4();
#endif


typedef uint64_t changeid4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_changeid4(...);
}
#else
bool_t xdr_changeid4();
#endif


typedef uint64_t clientid4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_clientid4(...);
}
#else
bool_t xdr_clientid4();
#endif


typedef uint32_t count4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_count4(...);
}
#else
bool_t xdr_count4();
#endif


typedef uint64_t length4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_length4(...);
}
#else
bool_t xdr_length4();
#endif


typedef uint32_t mode4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_mode4(...);
}
#else
bool_t xdr_mode4();
#endif


typedef uint64_t nfs_cookie4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfs_cookie4(...);
}
#else
bool_t xdr_nfs_cookie4();
#endif


typedef struct {
	u_int nfs_fh4_len;
	char *nfs_fh4_val;
} nfs_fh4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfs_fh4(...);
}
#else
bool_t xdr_nfs_fh4();
#endif


typedef uint64_t offset4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_offset4(...);
}
#else
bool_t xdr_offset4();
#endif


typedef uint32_t qop4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_qop4(...);
}
#else
bool_t xdr_qop4();
#endif


typedef struct {
	u_int sec_oid4_len;
	char *sec_oid4_val;
} sec_oid4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_sec_oid4(...);
}
#else
bool_t xdr_sec_oid4();
#endif


typedef uint32_t sequenceid4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_sequenceid4(...);
}
#else
bool_t xdr_sequenceid4();
#endif


typedef uint32_t seqid4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_seqid4(...);
}
#else
bool_t xdr_seqid4();
#endif


typedef char sessionid4[NFS4_SESSIONID_SIZE];
#ifdef __cplusplus
extern "C" {
bool_t xdr_sessionid4(...);
}
#else
bool_t xdr_sessionid4();
#endif


typedef uint32_t slotid4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_slotid4(...);
}
#else
bool_t xdr_slotid4();
#endif


typedef struct {
	u_int utf8string_len;
	char *utf8string_val;
} utf8string;
#ifdef __cplusplus
extern "C" {
bool_t xdr_utf8string(...);
}
#else
bool_t xdr_utf8string();
#endif


typedef utf8string utf8str_cis;
#ifdef __cplusplus
extern "C" {
bool_t xdr_utf8str_cis(...);
}
#else
bool_t xdr_utf8str_cis();
#endif


typedef utf8string utf8str_cs;
#ifdef __cplusplus
extern "C" {
bool_t xdr_utf8str_cs(...);
}
#else
bool_t xdr_utf8str_cs();
#endif


typedef utf8string utf8str_mixed;
#ifdef __cplusplus
extern "C" {
bool_t xdr_utf8str_mixed(...);
}
#else
bool_t xdr_utf8str_mixed();
#endif


typedef utf8str_cs component4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_component4(...);
}
#else
bool_t xdr_component4();
#endif


typedef utf8str_cs linktext4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_linktext4(...);
}
#else
bool_t xdr_linktext4();
#endif


typedef struct {
	u_int pathname4_len;
	component4 *pathname4_val;
} pathname4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_pathname4(...);
}
#else
bool_t xdr_pathname4();
#endif


typedef char verifier4[NFS4_VERIFIER_SIZE];
#ifdef __cplusplus
extern "C" {
bool_t xdr_verifier4(...);
}
#else
bool_t xdr_verifier4();
#endif


struct nfstime4 {
	int64_t seconds;
	uint32_t nseconds;
};
typedef struct nfstime4 nfstime4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfstime4(...);
}
#else
bool_t xdr_nfstime4();
#endif


enum time_how4 {
	SET_TO_SERVER_TIME4 = 0,
	SET_TO_CLIENT_TIME4 = 1,
};
typedef enum time_how4 time_how4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_time_how4(...);
}
#else
bool_t xdr_time_how4();
#endif


struct settime4 {
	time_how4 set_it;
	union {
		nfstime4 time;
	} settime4_u;
};
typedef struct settime4 settime4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_settime4(...);
}
#else
bool_t xdr_settime4();
#endif


typedef uint32_t nfs_lease4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfs_lease4(...);
}
#else
bool_t xdr_nfs_lease4();
#endif


struct fsid4 {
	uint64_t major;
	uint64_t minor;
};
typedef struct fsid4 fsid4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fsid4(...);
}
#else
bool_t xdr_fsid4();
#endif


struct change_policy4 {
	uint64_t cp_major;
	uint64_t cp_minor;
};
typedef struct change_policy4 change_policy4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_change_policy4(...);
}
#else
bool_t xdr_change_policy4();
#endif


struct fs_location4 {
	struct {
		u_int server_len;
		utf8str_cis *server_val;
	} server;
	pathname4 rootpath;
};
typedef struct fs_location4 fs_location4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fs_location4(...);
}
#else
bool_t xdr_fs_location4();
#endif


struct fs_locations4 {
	pathname4 fs_root;
	struct {
		u_int locations_len;
		fs_location4 *locations_val;
	} locations;
};
typedef struct fs_locations4 fs_locations4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fs_locations4(...);
}
#else
bool_t xdr_fs_locations4();
#endif

#define ACL4_SUPPORT_ALLOW_ACL 0x00000001
#define ACL4_SUPPORT_DENY_ACL 0x00000002
#define ACL4_SUPPORT_AUDIT_ACL 0x00000004
#define ACL4_SUPPORT_ALARM_ACL 0x00000008

typedef uint32_t acetype4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_acetype4(...);
}
#else
bool_t xdr_acetype4();
#endif

#define ACE4_ACCESS_ALLOWED_ACE_TYPE 0x00000000
#define ACE4_ACCESS_DENIED_ACE_TYPE 0x00000001
#define ACE4_SYSTEM_AUDIT_ACE_TYPE 0x00000002
#define ACE4_SYSTEM_ALARM_ACE_TYPE 0x00000003

typedef uint32_t aceflag4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_aceflag4(...);
}
#else
bool_t xdr_aceflag4();
#endif

#define ACE4_FILE_INHERIT_ACE 0x00000001
#define ACE4_DIRECTORY_INHERIT_ACE 0x00000002
#define ACE4_NO_PROPAGATE_INHERIT_ACE 0x00000004
#define ACE4_INHERIT_ONLY_ACE 0x00000008
#define ACE4_SUCCESSFUL_ACCESS_ACE_FLAG 0x00000010
#define ACE4_FAILED_ACCESS_ACE_FLAG 0x00000020
#define ACE4_IDENTIFIER_GROUP 0x00000040
#define ACE4_INHERITED_ACE 0x00000080

typedef uint32_t acemask4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_acemask4(...);
}
#else
bool_t xdr_acemask4();
#endif

#define ACE4_READ_DATA 0x00000001
#define ACE4_LIST_DIRECTORY 0x00000001
#define ACE4_WRITE_DATA 0x00000002
#define ACE4_ADD_FILE 0x00000002
#define ACE4_APPEND_DATA 0x00000004
#define ACE4_ADD_SUBDIRECTORY 0x00000004
#define ACE4_READ_NAMED_ATTRS 0x00000008
#define ACE4_WRITE_NAMED_ATTRS 0x00000010
#define ACE4_EXECUTE 0x00000020
#define ACE4_DELETE_CHILD 0x00000040
#define ACE4_READ_ATTRIBUTES 0x00000080
#define ACE4_WRITE_ATTRIBUTES 0x00000100
#define ACE4_WRITE_RETENTION 0x00000200
#define ACE4_WRITE_RETENTION_HOLD 0x00000400
#define ACE4_DELETE 0x00010000
#define ACE4_READ_ACL 0x00020000
#define ACE4_WRITE_ACL 0x00040000
#define ACE4_WRITE_OWNER 0x00080000
#define ACE4_SYNCHRONIZE 0x00100000
#define ACE4_GENERIC_READ 0x00120081
#define ACE4_GENERIC_WRITE 0x00160106
#define ACE4_GENERIC_EXECUTE 0x001200A0

struct nfsace4 {
	acetype4 type;
	aceflag4 flag;
	acemask4 access_mask;
	utf8str_mixed who;
};
typedef struct nfsace4 nfsace4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfsace4(...);
}
#else
bool_t xdr_nfsace4();
#endif


typedef uint32_t aclflag4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_aclflag4(...);
}
#else
bool_t xdr_aclflag4();
#endif

#define ACL4_AUTO_INHERIT 0x00000001
#define ACL4_PROTECTED 0x00000002
#define ACL4_DEFAULTED 0x00000004

struct nfsacl41 {
	aclflag4 na41_flag;
	struct {
		u_int na41_aces_len;
		nfsace4 *na41_aces_val;
	} na41_aces;
};
typedef struct nfsacl41 nfsacl41;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfsacl41(...);
}
#else
bool_t xdr_nfsacl41();
#endif

#define MODE4_SUID 0x800
#define MODE4_SGID 0x400
#define MODE4_SVTX 0x200
#define MODE4_RUSR 0x100
#define MODE4_WUSR 0x080
#define MODE4_XUSR 0x040
#define MODE4_RGRP 0x020
#define MODE4_WGRP 0x010
#define MODE4_XGRP 0x008
#define MODE4_ROTH 0x004
#define MODE4_WOTH 0x002
#define MODE4_XOTH 0x001

struct mode_masked4 {
	mode4 mm_value_to_set;
	mode4 mm_mask_bits;
};
typedef struct mode_masked4 mode_masked4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_mode_masked4(...);
}
#else
bool_t xdr_mode_masked4();
#endif


struct specdata4 {
	uint32_t specdata1;
	uint32_t specdata2;
};
typedef struct specdata4 specdata4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_specdata4(...);
}
#else
bool_t xdr_specdata4();
#endif

#define FH4_PERSISTENT 0x00000000
#define FH4_NOEXPIRE_WITH_OPEN 0x00000001
#define FH4_VOLATILE_ANY 0x00000002
#define FH4_VOL_MIGRATION 0x00000004
#define FH4_VOL_RENAME 0x00000008

struct netaddr4 {
	char *na_r_netid;
	char *na_r_addr;
};
typedef struct netaddr4 netaddr4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_netaddr4(...);
}
#else
bool_t xdr_netaddr4();
#endif


struct nfs_impl_id4 {
	utf8str_cis nii_domain;
	utf8str_cs nii_name;
	nfstime4 nii_date;
};
typedef struct nfs_impl_id4 nfs_impl_id4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfs_impl_id4(...);
}
#else
bool_t xdr_nfs_impl_id4();
#endif


struct stateid4 {
	uint32_t seqid;
	char other[12];
};
typedef struct stateid4 stateid4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_stateid4(...);
}
#else
bool_t xdr_stateid4();
#endif


enum layouttype4 {
	LAYOUT4_NFSV4_1_FILES = 0x1,
	LAYOUT4_OSD2_OBJECTS = 0x2,
	LAYOUT4_BLOCK_VOLUME = 0x3,
};
typedef enum layouttype4 layouttype4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_layouttype4(...);
}
#else
bool_t xdr_layouttype4();
#endif


struct layout_content4 {
	layouttype4 loc_type;
	struct {
		u_int loc_body_len;
		char *loc_body_val;
	} loc_body;
};
typedef struct layout_content4 layout_content4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_layout_content4(...);
}
#else
bool_t xdr_layout_content4();
#endif

/*
/* LAYOUT4_OSD2_OBJECTS loc_body description
 * is in a separate .x file
 */

/*
/* LAYOUT4_BLOCK_VOLUME loc_body description
 * is in a separate .x file
 */

struct layouthint4 {
	layouttype4 loh_type;
	struct {
		u_int loh_body_len;
		char *loh_body_val;
	} loh_body;
};
typedef struct layouthint4 layouthint4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_layouthint4(...);
}
#else
bool_t xdr_layouthint4();
#endif


enum layoutiomode4 {
	LAYOUTIOMODE4_READ = 1,
	LAYOUTIOMODE4_RW = 2,
	LAYOUTIOMODE4_ANY = 3,
};
typedef enum layoutiomode4 layoutiomode4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_layoutiomode4(...);
}
#else
bool_t xdr_layoutiomode4();
#endif


struct layout4 {
	offset4 lo_offset;
	length4 lo_length;
	layoutiomode4 lo_iomode;
	layout_content4 lo_content;
};
typedef struct layout4 layout4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_layout4(...);
}
#else
bool_t xdr_layout4();
#endif

#define NFS4_DEVICEID4_SIZE 16

typedef char deviceid4[NFS4_DEVICEID4_SIZE];
#ifdef __cplusplus
extern "C" {
bool_t xdr_deviceid4(...);
}
#else
bool_t xdr_deviceid4();
#endif


struct device_addr4 {
	layouttype4 da_layout_type;
	struct {
		u_int da_addr_body_len;
		char *da_addr_body_val;
	} da_addr_body;
};
typedef struct device_addr4 device_addr4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_device_addr4(...);
}
#else
bool_t xdr_device_addr4();
#endif


struct layoutupdate4 {
	layouttype4 lou_type;
	struct {
		u_int lou_body_len;
		char *lou_body_val;
	} lou_body;
};
typedef struct layoutupdate4 layoutupdate4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_layoutupdate4(...);
}
#else
bool_t xdr_layoutupdate4();
#endif


#define LAYOUT4_RET_REC_FILE 1
#define LAYOUT4_RET_REC_FSID 2
#define LAYOUT4_RET_REC_ALL 3


enum layoutreturn_type4 {
	LAYOUTRETURN4_FILE = LAYOUT4_RET_REC_FILE,
	LAYOUTRETURN4_FSID = LAYOUT4_RET_REC_FSID,
	LAYOUTRETURN4_ALL = LAYOUT4_RET_REC_ALL,
};
typedef enum layoutreturn_type4 layoutreturn_type4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_layoutreturn_type4(...);
}
#else
bool_t xdr_layoutreturn_type4();
#endif

       /* layouttype4 specific data */

struct layoutreturn_file4 {
	offset4 lrf_offset;
	length4 lrf_length;
	stateid4 lrf_stateid;
	struct {
		u_int lrf_body_len;
		char *lrf_body_val;
	} lrf_body;
};
typedef struct layoutreturn_file4 layoutreturn_file4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_layoutreturn_file4(...);
}
#else
bool_t xdr_layoutreturn_file4();
#endif


struct layoutreturn4 {
	layoutreturn_type4 lr_returntype;
	union {
		layoutreturn_file4 lr_layout;
	} layoutreturn4_u;
};
typedef struct layoutreturn4 layoutreturn4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_layoutreturn4(...);
}
#else
bool_t xdr_layoutreturn4();
#endif



enum fs4_status_type {
	STATUS4_FIXED = 1,
	STATUS4_UPDATED = 2,
	STATUS4_VERSIONED = 3,
	STATUS4_WRITABLE = 4,
	STATUS4_REFERRAL = 5,
};
typedef enum fs4_status_type fs4_status_type;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fs4_status_type(...);
}
#else
bool_t xdr_fs4_status_type();
#endif


struct fs4_status {
	bool_t fss_absent;
	fs4_status_type fss_type;
	utf8str_cs fss_source;
	utf8str_cs fss_current;
	int32_t fss_age;
	nfstime4 fss_version;
};
typedef struct fs4_status fs4_status;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fs4_status(...);
}
#else
bool_t xdr_fs4_status();
#endif

#define TH4_READ_SIZE 0
#define TH4_WRITE_SIZE 1
#define TH4_READ_IOSIZE 2
#define TH4_WRITE_IOSIZE 3

typedef length4 threshold4_read_size;
#ifdef __cplusplus
extern "C" {
bool_t xdr_threshold4_read_size(...);
}
#else
bool_t xdr_threshold4_read_size();
#endif


typedef length4 threshold4_write_size;
#ifdef __cplusplus
extern "C" {
bool_t xdr_threshold4_write_size(...);
}
#else
bool_t xdr_threshold4_write_size();
#endif


typedef length4 threshold4_read_iosize;
#ifdef __cplusplus
extern "C" {
bool_t xdr_threshold4_read_iosize(...);
}
#else
bool_t xdr_threshold4_read_iosize();
#endif


typedef length4 threshold4_write_iosize;
#ifdef __cplusplus
extern "C" {
bool_t xdr_threshold4_write_iosize(...);
}
#else
bool_t xdr_threshold4_write_iosize();
#endif


struct threshold_item4 {
	layouttype4 thi_layout_type;
	bitmap4 thi_hintset;
	struct {
		u_int thi_hintlist_len;
		char *thi_hintlist_val;
	} thi_hintlist;
};
typedef struct threshold_item4 threshold_item4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_threshold_item4(...);
}
#else
bool_t xdr_threshold_item4();
#endif


struct mdsthreshold4 {
	struct {
		u_int mth_hints_len;
		threshold_item4 *mth_hints_val;
	} mth_hints;
};
typedef struct mdsthreshold4 mdsthreshold4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_mdsthreshold4(...);
}
#else
bool_t xdr_mdsthreshold4();
#endif

#define RET4_DURATION_INFINITE 0xffffffffffffffff

struct retention_get4 {
	uint64_t rg_duration;
	struct {
		u_int rg_begin_time_len;
		nfstime4 *rg_begin_time_val;
	} rg_begin_time;
};
typedef struct retention_get4 retention_get4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_retention_get4(...);
}
#else
bool_t xdr_retention_get4();
#endif


struct retention_set4 {
	bool_t rs_enable;
	struct {
		u_int rs_duration_len;
		uint64_t *rs_duration_val;
	} rs_duration;
};
typedef struct retention_set4 retention_set4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_retention_set4(...);
}
#else
bool_t xdr_retention_set4();
#endif

#define FSCHARSET_CAP4_CONTAINS_NON_UTF8 0x1
#define FSCHARSET_CAP4_ALLOWS_ONLY_UTF8 0x2

typedef uint32_t fs_charset_cap4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fs_charset_cap4(...);
}
#else
bool_t xdr_fs_charset_cap4();
#endif


typedef bitmap4 fattr4_supported_attrs;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_supported_attrs(...);
}
#else
bool_t xdr_fattr4_supported_attrs();
#endif


typedef bitmap4 fattr4_suppattr_exclcreat;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_suppattr_exclcreat(...);
}
#else
bool_t xdr_fattr4_suppattr_exclcreat();
#endif


typedef nfs_ftype4 fattr4_type;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_type(...);
}
#else
bool_t xdr_fattr4_type();
#endif


typedef uint32_t fattr4_fh_expire_type;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_fh_expire_type(...);
}
#else
bool_t xdr_fattr4_fh_expire_type();
#endif


typedef changeid4 fattr4_change;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_change(...);
}
#else
bool_t xdr_fattr4_change();
#endif


typedef uint64_t fattr4_size;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_size(...);
}
#else
bool_t xdr_fattr4_size();
#endif


typedef bool_t fattr4_link_support;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_link_support(...);
}
#else
bool_t xdr_fattr4_link_support();
#endif


typedef bool_t fattr4_symlink_support;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_symlink_support(...);
}
#else
bool_t xdr_fattr4_symlink_support();
#endif


typedef bool_t fattr4_named_attr;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_named_attr(...);
}
#else
bool_t xdr_fattr4_named_attr();
#endif


typedef fsid4 fattr4_fsid;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_fsid(...);
}
#else
bool_t xdr_fattr4_fsid();
#endif


typedef bool_t fattr4_unique_handles;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_unique_handles(...);
}
#else
bool_t xdr_fattr4_unique_handles();
#endif


typedef nfs_lease4 fattr4_lease_time;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_lease_time(...);
}
#else
bool_t xdr_fattr4_lease_time();
#endif


typedef nfsstat4 fattr4_rdattr_error;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_rdattr_error(...);
}
#else
bool_t xdr_fattr4_rdattr_error();
#endif


typedef struct {
	u_int fattr4_acl_len;
	nfsace4 *fattr4_acl_val;
} fattr4_acl;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_acl(...);
}
#else
bool_t xdr_fattr4_acl();
#endif


typedef uint32_t fattr4_aclsupport;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_aclsupport(...);
}
#else
bool_t xdr_fattr4_aclsupport();
#endif


typedef bool_t fattr4_archive;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_archive(...);
}
#else
bool_t xdr_fattr4_archive();
#endif


typedef bool_t fattr4_cansettime;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_cansettime(...);
}
#else
bool_t xdr_fattr4_cansettime();
#endif


typedef bool_t fattr4_case_insensitive;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_case_insensitive(...);
}
#else
bool_t xdr_fattr4_case_insensitive();
#endif


typedef bool_t fattr4_case_preserving;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_case_preserving(...);
}
#else
bool_t xdr_fattr4_case_preserving();
#endif


typedef bool_t fattr4_chown_restricted;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_chown_restricted(...);
}
#else
bool_t xdr_fattr4_chown_restricted();
#endif


typedef uint64_t fattr4_fileid;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_fileid(...);
}
#else
bool_t xdr_fattr4_fileid();
#endif


typedef uint64_t fattr4_files_avail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_files_avail(...);
}
#else
bool_t xdr_fattr4_files_avail();
#endif


typedef nfs_fh4 fattr4_filehandle;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_filehandle(...);
}
#else
bool_t xdr_fattr4_filehandle();
#endif


typedef uint64_t fattr4_files_free;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_files_free(...);
}
#else
bool_t xdr_fattr4_files_free();
#endif


typedef uint64_t fattr4_files_total;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_files_total(...);
}
#else
bool_t xdr_fattr4_files_total();
#endif


typedef fs_locations4 fattr4_fs_locations;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_fs_locations(...);
}
#else
bool_t xdr_fattr4_fs_locations();
#endif


typedef bool_t fattr4_hidden;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_hidden(...);
}
#else
bool_t xdr_fattr4_hidden();
#endif


typedef bool_t fattr4_homogeneous;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_homogeneous(...);
}
#else
bool_t xdr_fattr4_homogeneous();
#endif


typedef uint64_t fattr4_maxfilesize;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_maxfilesize(...);
}
#else
bool_t xdr_fattr4_maxfilesize();
#endif


typedef uint32_t fattr4_maxlink;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_maxlink(...);
}
#else
bool_t xdr_fattr4_maxlink();
#endif


typedef uint32_t fattr4_maxname;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_maxname(...);
}
#else
bool_t xdr_fattr4_maxname();
#endif


typedef uint64_t fattr4_maxread;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_maxread(...);
}
#else
bool_t xdr_fattr4_maxread();
#endif


typedef uint64_t fattr4_maxwrite;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_maxwrite(...);
}
#else
bool_t xdr_fattr4_maxwrite();
#endif


typedef utf8str_cs fattr4_mimetype;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_mimetype(...);
}
#else
bool_t xdr_fattr4_mimetype();
#endif


typedef mode4 fattr4_mode;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_mode(...);
}
#else
bool_t xdr_fattr4_mode();
#endif


typedef mode_masked4 fattr4_mode_set_masked;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_mode_set_masked(...);
}
#else
bool_t xdr_fattr4_mode_set_masked();
#endif


typedef uint64_t fattr4_mounted_on_fileid;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_mounted_on_fileid(...);
}
#else
bool_t xdr_fattr4_mounted_on_fileid();
#endif


typedef bool_t fattr4_no_trunc;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_no_trunc(...);
}
#else
bool_t xdr_fattr4_no_trunc();
#endif


typedef uint32_t fattr4_numlinks;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_numlinks(...);
}
#else
bool_t xdr_fattr4_numlinks();
#endif


typedef utf8str_mixed fattr4_owner;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_owner(...);
}
#else
bool_t xdr_fattr4_owner();
#endif


typedef utf8str_mixed fattr4_owner_group;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_owner_group(...);
}
#else
bool_t xdr_fattr4_owner_group();
#endif


typedef uint64_t fattr4_quota_avail_hard;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_quota_avail_hard(...);
}
#else
bool_t xdr_fattr4_quota_avail_hard();
#endif


typedef uint64_t fattr4_quota_avail_soft;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_quota_avail_soft(...);
}
#else
bool_t xdr_fattr4_quota_avail_soft();
#endif


typedef uint64_t fattr4_quota_used;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_quota_used(...);
}
#else
bool_t xdr_fattr4_quota_used();
#endif


typedef specdata4 fattr4_rawdev;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_rawdev(...);
}
#else
bool_t xdr_fattr4_rawdev();
#endif


typedef uint64_t fattr4_space_avail;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_space_avail(...);
}
#else
bool_t xdr_fattr4_space_avail();
#endif


typedef uint64_t fattr4_space_free;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_space_free(...);
}
#else
bool_t xdr_fattr4_space_free();
#endif


typedef uint64_t fattr4_space_total;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_space_total(...);
}
#else
bool_t xdr_fattr4_space_total();
#endif


typedef uint64_t fattr4_space_used;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_space_used(...);
}
#else
bool_t xdr_fattr4_space_used();
#endif


typedef bool_t fattr4_system;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_system(...);
}
#else
bool_t xdr_fattr4_system();
#endif


typedef nfstime4 fattr4_time_access;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_time_access(...);
}
#else
bool_t xdr_fattr4_time_access();
#endif


typedef settime4 fattr4_time_access_set;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_time_access_set(...);
}
#else
bool_t xdr_fattr4_time_access_set();
#endif


typedef nfstime4 fattr4_time_backup;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_time_backup(...);
}
#else
bool_t xdr_fattr4_time_backup();
#endif


typedef nfstime4 fattr4_time_create;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_time_create(...);
}
#else
bool_t xdr_fattr4_time_create();
#endif


typedef nfstime4 fattr4_time_delta;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_time_delta(...);
}
#else
bool_t xdr_fattr4_time_delta();
#endif


typedef nfstime4 fattr4_time_metadata;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_time_metadata(...);
}
#else
bool_t xdr_fattr4_time_metadata();
#endif


typedef nfstime4 fattr4_time_modify;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_time_modify(...);
}
#else
bool_t xdr_fattr4_time_modify();
#endif


typedef settime4 fattr4_time_modify_set;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_time_modify_set(...);
}
#else
bool_t xdr_fattr4_time_modify_set();
#endif


typedef nfstime4 fattr4_dir_notif_delay;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_dir_notif_delay(...);
}
#else
bool_t xdr_fattr4_dir_notif_delay();
#endif


typedef nfstime4 fattr4_dirent_notif_delay;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_dirent_notif_delay(...);
}
#else
bool_t xdr_fattr4_dirent_notif_delay();
#endif


typedef bool_t fattr4_absent;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_absent(...);
}
#else
bool_t xdr_fattr4_absent();
#endif


typedef struct {
	u_int fattr4_fs_layout_types_len;
	layouttype4 *fattr4_fs_layout_types_val;
} fattr4_fs_layout_types;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_fs_layout_types(...);
}
#else
bool_t xdr_fattr4_fs_layout_types();
#endif


typedef fs4_status fattr4_fs_status;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_fs_status(...);
}
#else
bool_t xdr_fattr4_fs_status();
#endif


typedef fs_charset_cap4 fattr4_fs_charset_cap4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_fs_charset_cap4(...);
}
#else
bool_t xdr_fattr4_fs_charset_cap4();
#endif


typedef uint32_t fattr4_layout_alignment;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_layout_alignment(...);
}
#else
bool_t xdr_fattr4_layout_alignment();
#endif


typedef uint32_t fattr4_layout_blksize;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_layout_blksize(...);
}
#else
bool_t xdr_fattr4_layout_blksize();
#endif


typedef layouthint4 fattr4_layout_hint;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_layout_hint(...);
}
#else
bool_t xdr_fattr4_layout_hint();
#endif


typedef struct {
	u_int fattr4_layout_types_len;
	layouttype4 *fattr4_layout_types_val;
} fattr4_layout_types;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_layout_types(...);
}
#else
bool_t xdr_fattr4_layout_types();
#endif


typedef mdsthreshold4 fattr4_mdsthreshold;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_mdsthreshold(...);
}
#else
bool_t xdr_fattr4_mdsthreshold();
#endif


typedef retention_get4 fattr4_retention_get;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_retention_get(...);
}
#else
bool_t xdr_fattr4_retention_get();
#endif


typedef retention_set4 fattr4_retention_set;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_retention_set(...);
}
#else
bool_t xdr_fattr4_retention_set();
#endif


typedef retention_get4 fattr4_retentevt_get;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_retentevt_get(...);
}
#else
bool_t xdr_fattr4_retentevt_get();
#endif


typedef retention_set4 fattr4_retentevt_set;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_retentevt_set(...);
}
#else
bool_t xdr_fattr4_retentevt_set();
#endif


typedef uint64_t fattr4_retention_hold;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_retention_hold(...);
}
#else
bool_t xdr_fattr4_retention_hold();
#endif


typedef nfsacl41 fattr4_dacl;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_dacl(...);
}
#else
bool_t xdr_fattr4_dacl();
#endif


typedef nfsacl41 fattr4_sacl;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_sacl(...);
}
#else
bool_t xdr_fattr4_sacl();
#endif

#define FATTR4_SUPPORTED_ATTRS 0
#define FATTR4_TYPE 1
#define FATTR4_FH_EXPIRE_TYPE 2
#define FATTR4_CHANGE 3
#define FATTR4_SIZE 4
#define FATTR4_LINK_SUPPORT 5
#define FATTR4_SYMLINK_SUPPORT 6
#define FATTR4_NAMED_ATTR 7
#define FATTR4_FSID 8
#define FATTR4_UNIQUE_HANDLES 9
#define FATTR4_LEASE_TIME 10
#define FATTR4_RDATTR_ERROR 11
#define FATTR4_FILEHANDLE 19
#define FATTR4_SUPPATTR_EXCLCREAT 75
#define FATTR4_ACL 12
#define FATTR4_ACLSUPPORT 13
#define FATTR4_ARCHIVE 14
#define FATTR4_CANSETTIME 15
#define FATTR4_CASE_INSENSITIVE 16
#define FATTR4_CASE_PRESERVING 17
#define FATTR4_CHOWN_RESTRICTED 18
#define FATTR4_FILEID 20
#define FATTR4_FILES_AVAIL 21
#define FATTR4_FILES_FREE 22
#define FATTR4_FILES_TOTAL 23
#define FATTR4_FS_LOCATIONS 24
#define FATTR4_HIDDEN 25
#define FATTR4_HOMOGENEOUS 26
#define FATTR4_MAXFILESIZE 27
#define FATTR4_MAXLINK 28
#define FATTR4_MAXNAME 29
#define FATTR4_MAXREAD 30
#define FATTR4_MAXWRITE 31
#define FATTR4_MIMETYPE 32
#define FATTR4_MODE 33
#define FATTR4_NO_TRUNC 34
#define FATTR4_NUMLINKS 35
#define FATTR4_OWNER 36
#define FATTR4_OWNER_GROUP 37
#define FATTR4_QUOTA_AVAIL_HARD 38
#define FATTR4_QUOTA_AVAIL_SOFT 39
#define FATTR4_QUOTA_USED 40
#define FATTR4_RAWDEV 41
#define FATTR4_SPACE_AVAIL 42
#define FATTR4_SPACE_FREE 43
#define FATTR4_SPACE_TOTAL 44
#define FATTR4_SPACE_USED 45
#define FATTR4_SYSTEM 46
#define FATTR4_TIME_ACCESS 47
#define FATTR4_TIME_ACCESS_SET 48
#define FATTR4_TIME_BACKUP 49
#define FATTR4_TIME_CREATE 50
#define FATTR4_TIME_DELTA 51
#define FATTR4_TIME_METADATA 52
#define FATTR4_TIME_MODIFY 53
#define FATTR4_TIME_MODIFY_SET 54
#define FATTR4_MOUNTED_ON_FILEID 55
#define FATTR4_DIR_NOTIF_DELAY 56
#define FATTR4_DIRENT_NOTIF_DELAY 57
#define FATTR4_DACL 58
#define FATTR4_SACL 59
#define FATTR4_CHANGE_POLICY 60
#define FATTR4_FS_STATUS 61
#define FATTR4_FS_LAYOUT_TYPE 62
#define FATTR4_LAYOUT_HINT 63
#define FATTR4_LAYOUT_TYPE 64
#define FATTR4_LAYOUT_BLKSIZE 65
#define FATTR4_LAYOUT_ALIGNMENT 66
#define FATTR4_FS_LOCATIONS_INFO 67
#define FATTR4_MDSTHRESHOLD 68
#define FATTR4_RETENTION_GET 69
#define FATTR4_RETENTION_SET 70
#define FATTR4_RETENTEVT_GET 71
#define FATTR4_RETENTEVT_SET 72
#define FATTR4_RETENTION_HOLD 73
#define FATTR4_MODE_SET_MASKED 74
#define FATTR4_FS_CHARSET_CAP 76

struct fattr4 {
	bitmap4 attrmask;
	attrlist4 attr_vals;
};
typedef struct fattr4 fattr4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4(...);
}
#else
bool_t xdr_fattr4();
#endif


struct change_info4 {
	bool_t atomic;
	changeid4 before;
	changeid4 after;
};
typedef struct change_info4 change_info4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_change_info4(...);
}
#else
bool_t xdr_change_info4();
#endif


typedef netaddr4 clientaddr4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_clientaddr4(...);
}
#else
bool_t xdr_clientaddr4();
#endif


struct cb_client4 {
	uint32_t cb_program;
	netaddr4 cb_location;
};
typedef struct cb_client4 cb_client4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_cb_client4(...);
}
#else
bool_t xdr_cb_client4();
#endif


struct nfs_client_id4 {
	verifier4 verifier;
	struct {
		u_int id_len;
		char *id_val;
	} id;
};
typedef struct nfs_client_id4 nfs_client_id4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfs_client_id4(...);
}
#else
bool_t xdr_nfs_client_id4();
#endif


struct client_owner4 {
	verifier4 co_verifier;
	struct {
		u_int co_ownerid_len;
		char *co_ownerid_val;
	} co_ownerid;
};
typedef struct client_owner4 client_owner4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_client_owner4(...);
}
#else
bool_t xdr_client_owner4();
#endif


struct server_owner4 {
	uint64_t so_minor_id;
	struct {
		u_int so_major_id_len;
		char *so_major_id_val;
	} so_major_id;
};
typedef struct server_owner4 server_owner4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_server_owner4(...);
}
#else
bool_t xdr_server_owner4();
#endif


struct state_owner4 {
	clientid4 clientid;
	struct {
		u_int owner_len;
		char *owner_val;
	} owner;
};
typedef struct state_owner4 state_owner4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_state_owner4(...);
}
#else
bool_t xdr_state_owner4();
#endif


typedef state_owner4 open_owner4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_open_owner4(...);
}
#else
bool_t xdr_open_owner4();
#endif


typedef state_owner4 lock_owner4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_lock_owner4(...);
}
#else
bool_t xdr_lock_owner4();
#endif


enum nfs_lock_type4 {
	READ_LT = 1,
	WRITE_LT = 2,
	READW_LT = 3,
	WRITEW_LT = 4,
};
typedef enum nfs_lock_type4 nfs_lock_type4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfs_lock_type4(...);
}
#else
bool_t xdr_nfs_lock_type4();
#endif


/* Input for computing subkeys */

enum ssv_subkey4 {
	SSV4_SUBKEY_MIC_I2T = 1,
	SSV4_SUBKEY_MIC_T2I = 2,
	SSV4_SUBKEY_SEAL_I2T = 3,
	SSV4_SUBKEY_SEAL_T2I = 4,
};
typedef enum ssv_subkey4 ssv_subkey4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ssv_subkey4(...);
}
#else
bool_t xdr_ssv_subkey4();
#endif



/* Input for computing smt_hmac */

struct ssv_mic_plain_tkn4 {
	uint32_t smpt_ssv_seq;
	struct {
		u_int smpt_orig_plain_len;
		char *smpt_orig_plain_val;
	} smpt_orig_plain;
};
typedef struct ssv_mic_plain_tkn4 ssv_mic_plain_tkn4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ssv_mic_plain_tkn4(...);
}
#else
bool_t xdr_ssv_mic_plain_tkn4();
#endif



/* SSV GSS PerMsgToken token */

struct ssv_mic_tkn4 {
	uint32_t smt_ssv_seq;
	struct {
		u_int smt_hmac_len;
		char *smt_hmac_val;
	} smt_hmac;
};
typedef struct ssv_mic_tkn4 ssv_mic_tkn4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ssv_mic_tkn4(...);
}
#else
bool_t xdr_ssv_mic_tkn4();
#endif



/* Input for computing ssct_encr_data and ssct_hmac */

struct ssv_seal_plain_tkn4 {
	struct {
		u_int sspt_confounder_len;
		char *sspt_confounder_val;
	} sspt_confounder;
	uint32_t sspt_ssv_seq;
	struct {
		u_int sspt_orig_plain_len;
		char *sspt_orig_plain_val;
	} sspt_orig_plain;
	struct {
		u_int sspt_pad_len;
		char *sspt_pad_val;
	} sspt_pad;
};
typedef struct ssv_seal_plain_tkn4 ssv_seal_plain_tkn4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ssv_seal_plain_tkn4(...);
}
#else
bool_t xdr_ssv_seal_plain_tkn4();
#endif



/* SSV GSS SealedMessage token */

struct ssv_seal_cipher_tkn4 {
	uint32_t ssct_ssv_seq;
	struct {
		u_int ssct_iv_len;
		char *ssct_iv_val;
	} ssct_iv;
	struct {
		u_int ssct_encr_data_len;
		char *ssct_encr_data_val;
	} ssct_encr_data;
	struct {
		u_int ssct_hmac_len;
		char *ssct_hmac_val;
	} ssct_hmac;
};
typedef struct ssv_seal_cipher_tkn4 ssv_seal_cipher_tkn4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ssv_seal_cipher_tkn4(...);
}
#else
bool_t xdr_ssv_seal_cipher_tkn4();
#endif



struct fs_locations_server4 {
	int32_t fls_currency;
	struct {
		u_int fls_info_len;
		char *fls_info_val;
	} fls_info;
	utf8str_cis fls_server;
};
typedef struct fs_locations_server4 fs_locations_server4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fs_locations_server4(...);
}
#else
bool_t xdr_fs_locations_server4();
#endif

#define FSLI4BX_GFLAGS 0
#define FSLI4BX_TFLAGS 1
#define FSLI4BX_CLSIMUL 2
#define FSLI4BX_CLHANDLE 3
#define FSLI4BX_CLFILEID 4
#define FSLI4BX_CLWRITEVER 5
#define FSLI4BX_CLCHANGE 6
#define FSLI4BX_CLREADDIR 7
#define FSLI4BX_READRANK 8
#define FSLI4BX_WRITERANK 9
#define FSLI4BX_READORDER 10
#define FSLI4BX_WRITEORDER 11
#define FSLI4GF_WRITABLE 0x01
#define FSLI4GF_CUR_REQ 0x02
#define FSLI4GF_ABSENT 0x04
#define FSLI4GF_GOING 0x08
#define FSLI4GF_SPLIT 0x10
#define FSLI4TF_RDMA 0x01

struct fs_locations_item4 {
	struct {
		u_int fli_entries_len;
		fs_locations_server4 *fli_entries_val;
	} fli_entries;
	pathname4 fli_rootpath;
};
typedef struct fs_locations_item4 fs_locations_item4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fs_locations_item4(...);
}
#else
bool_t xdr_fs_locations_item4();
#endif


struct fs_locations_info4 {
	uint32_t fli_flags;
	int32_t fli_valid_for;
	pathname4 fli_fs_root;
	struct {
		u_int fli_items_len;
		fs_locations_item4 *fli_items_val;
	} fli_items;
};
typedef struct fs_locations_info4 fs_locations_info4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fs_locations_info4(...);
}
#else
bool_t xdr_fs_locations_info4();
#endif

#define FSLI4IF_VAR_SUB 0x00000001

typedef fs_locations_info4 fattr4_fs_locations_info;
#ifdef __cplusplus
extern "C" {
bool_t xdr_fattr4_fs_locations_info(...);
}
#else
bool_t xdr_fattr4_fs_locations_info();
#endif

#define NFL4_UFLG_MASK 0x0000003F
#define NFL4_UFLG_DENSE 0x00000001
#define NFL4_UFLG_COMMIT_THRU_MDS 0x00000002
#define NFL4_UFLG_STRIPE_UNIT_SIZE_MASK 0xFFFFFFC0

typedef uint32_t nfl_util4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfl_util4(...);
}
#else
bool_t xdr_nfl_util4();
#endif



enum filelayout_hint_care4 {
	NFLH4_CARE_DENSE = NFL4_UFLG_DENSE,
	NFLH4_CARE_COMMIT_THRU_MDS = NFL4_UFLG_COMMIT_THRU_MDS,
	NFLH4_CARE_STRIPE_UNIT_SIZE = 0x00000040,
	NFLH4_CARE_STRIPE_COUNT = 0x00000080,
};
typedef enum filelayout_hint_care4 filelayout_hint_care4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_filelayout_hint_care4(...);
}
#else
bool_t xdr_filelayout_hint_care4();
#endif


/* Encoded in the loh_body field of type layouthint4: */


struct nfsv4_1_file_layouthint4 {
	uint32_t nflh_care;
	nfl_util4 nflh_util;
	count4 nflh_stripe_count;
};
typedef struct nfsv4_1_file_layouthint4 nfsv4_1_file_layouthint4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfsv4_1_file_layouthint4(...);
}
#else
bool_t xdr_nfsv4_1_file_layouthint4();
#endif




typedef struct {
	u_int multipath_list4_len;
	netaddr4 *multipath_list4_val;
} multipath_list4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_multipath_list4(...);
}
#else
bool_t xdr_multipath_list4();
#endif


/* Encoded in the da_addr_body field of type device_addr4: */

struct nfsv4_1_file_layout_ds_addr4 {
	struct {
		u_int nflda_stripe_indices_len;
		uint32_t *nflda_stripe_indices_val;
	} nflda_stripe_indices;
	struct {
		u_int nflda_multipath_ds_list_len;
		multipath_list4 *nflda_multipath_ds_list_val;
	} nflda_multipath_ds_list;
};
typedef struct nfsv4_1_file_layout_ds_addr4 nfsv4_1_file_layout_ds_addr4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfsv4_1_file_layout_ds_addr4(...);
}
#else
bool_t xdr_nfsv4_1_file_layout_ds_addr4();
#endif



/* Encoded in the loc_body field of type layout_content4: */

struct nfsv4_1_file_layout4 {
	deviceid4 nfl_deviceid;
	nfl_util4 nfl_util;
	uint32_t nfl_first_stripe_index;
	offset4 nfl_pattern_offset;
	struct {
		u_int nfl_fh_list_len;
		nfs_fh4 *nfl_fh_list_val;
	} nfl_fh_list;
};
typedef struct nfsv4_1_file_layout4 nfsv4_1_file_layout4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfsv4_1_file_layout4(...);
}
#else
bool_t xdr_nfsv4_1_file_layout4();
#endif


/*
 * Encoded in the lou_body field of type layoutupdate4:
 *      Nothing. lou_body is a zero length array of octets.
 */

#define ACCESS4_READ 0x00000001
#define ACCESS4_LOOKUP 0x00000002
#define ACCESS4_MODIFY 0x00000004
#define ACCESS4_EXTEND 0x00000008
#define ACCESS4_DELETE 0x00000010
#define ACCESS4_EXECUTE 0x00000020

struct ACCESS4args {
	uint32_t access;
};
typedef struct ACCESS4args ACCESS4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ACCESS4args(...);
}
#else
bool_t xdr_ACCESS4args();
#endif


struct ACCESS4resok {
	uint32_t supported;
	uint32_t access;
};
typedef struct ACCESS4resok ACCESS4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ACCESS4resok(...);
}
#else
bool_t xdr_ACCESS4resok();
#endif


struct ACCESS4res {
	nfsstat4 status;
	union {
		ACCESS4resok resok4;
	} ACCESS4res_u;
};
typedef struct ACCESS4res ACCESS4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ACCESS4res(...);
}
#else
bool_t xdr_ACCESS4res();
#endif


struct CLOSE4args {
	seqid4 seqid;
	stateid4 open_stateid;
};
typedef struct CLOSE4args CLOSE4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CLOSE4args(...);
}
#else
bool_t xdr_CLOSE4args();
#endif


struct CLOSE4res {
	nfsstat4 status;
	union {
		stateid4 open_stateid;
	} CLOSE4res_u;
};
typedef struct CLOSE4res CLOSE4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CLOSE4res(...);
}
#else
bool_t xdr_CLOSE4res();
#endif


struct COMMIT4args {
	offset4 offset;
	count4 count;
};
typedef struct COMMIT4args COMMIT4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_COMMIT4args(...);
}
#else
bool_t xdr_COMMIT4args();
#endif


struct COMMIT4resok {
	verifier4 writeverf;
};
typedef struct COMMIT4resok COMMIT4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_COMMIT4resok(...);
}
#else
bool_t xdr_COMMIT4resok();
#endif


struct COMMIT4res {
	nfsstat4 status;
	union {
		COMMIT4resok resok4;
	} COMMIT4res_u;
};
typedef struct COMMIT4res COMMIT4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_COMMIT4res(...);
}
#else
bool_t xdr_COMMIT4res();
#endif


struct createtype4 {
	nfs_ftype4 type;
	union {
		linktext4 linkdata;
		specdata4 devdata;
	} createtype4_u;
};
typedef struct createtype4 createtype4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_createtype4(...);
}
#else
bool_t xdr_createtype4();
#endif


struct CREATE4args {
	createtype4 objtype;
	component4 objname;
	fattr4 createattrs;
};
typedef struct CREATE4args CREATE4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CREATE4args(...);
}
#else
bool_t xdr_CREATE4args();
#endif


struct CREATE4resok {
	change_info4 cinfo;
	bitmap4 attrset;
};
typedef struct CREATE4resok CREATE4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CREATE4resok(...);
}
#else
bool_t xdr_CREATE4resok();
#endif


struct CREATE4res {
	nfsstat4 status;
	union {
		CREATE4resok resok4;
	} CREATE4res_u;
};
typedef struct CREATE4res CREATE4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CREATE4res(...);
}
#else
bool_t xdr_CREATE4res();
#endif


struct DELEGPURGE4args {
	clientid4 clientid;
};
typedef struct DELEGPURGE4args DELEGPURGE4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_DELEGPURGE4args(...);
}
#else
bool_t xdr_DELEGPURGE4args();
#endif


struct DELEGPURGE4res {
	nfsstat4 status;
};
typedef struct DELEGPURGE4res DELEGPURGE4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_DELEGPURGE4res(...);
}
#else
bool_t xdr_DELEGPURGE4res();
#endif


struct DELEGRETURN4args {
	stateid4 deleg_stateid;
};
typedef struct DELEGRETURN4args DELEGRETURN4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_DELEGRETURN4args(...);
}
#else
bool_t xdr_DELEGRETURN4args();
#endif


struct DELEGRETURN4res {
	nfsstat4 status;
};
typedef struct DELEGRETURN4res DELEGRETURN4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_DELEGRETURN4res(...);
}
#else
bool_t xdr_DELEGRETURN4res();
#endif


struct GETATTR4args {
	bitmap4 attr_request;
};
typedef struct GETATTR4args GETATTR4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GETATTR4args(...);
}
#else
bool_t xdr_GETATTR4args();
#endif


struct GETATTR4resok {
	fattr4 obj_attributes;
};
typedef struct GETATTR4resok GETATTR4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GETATTR4resok(...);
}
#else
bool_t xdr_GETATTR4resok();
#endif


struct GETATTR4res {
	nfsstat4 status;
	union {
		GETATTR4resok resok4;
	} GETATTR4res_u;
};
typedef struct GETATTR4res GETATTR4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GETATTR4res(...);
}
#else
bool_t xdr_GETATTR4res();
#endif


struct GETFH4resok {
	nfs_fh4 object;
};
typedef struct GETFH4resok GETFH4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GETFH4resok(...);
}
#else
bool_t xdr_GETFH4resok();
#endif


struct GETFH4res {
	nfsstat4 status;
	union {
		GETFH4resok resok4;
	} GETFH4res_u;
};
typedef struct GETFH4res GETFH4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GETFH4res(...);
}
#else
bool_t xdr_GETFH4res();
#endif


struct LINK4args {
	component4 newname;
};
typedef struct LINK4args LINK4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LINK4args(...);
}
#else
bool_t xdr_LINK4args();
#endif


struct LINK4resok {
	change_info4 cinfo;
};
typedef struct LINK4resok LINK4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LINK4resok(...);
}
#else
bool_t xdr_LINK4resok();
#endif


struct LINK4res {
	nfsstat4 status;
	union {
		LINK4resok resok4;
	} LINK4res_u;
};
typedef struct LINK4res LINK4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LINK4res(...);
}
#else
bool_t xdr_LINK4res();
#endif


struct open_to_lock_owner4 {
	seqid4 open_seqid;
	stateid4 open_stateid;
	seqid4 lock_seqid;
	lock_owner4 lock_owner;
};
typedef struct open_to_lock_owner4 open_to_lock_owner4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_open_to_lock_owner4(...);
}
#else
bool_t xdr_open_to_lock_owner4();
#endif


struct exist_lock_owner4 {
	stateid4 lock_stateid;
	seqid4 lock_seqid;
};
typedef struct exist_lock_owner4 exist_lock_owner4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_exist_lock_owner4(...);
}
#else
bool_t xdr_exist_lock_owner4();
#endif


struct locker4 {
	bool_t new_lock_owner;
	union {
		open_to_lock_owner4 open_owner;
		exist_lock_owner4 lock_owner;
	} locker4_u;
};
typedef struct locker4 locker4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_locker4(...);
}
#else
bool_t xdr_locker4();
#endif


struct LOCK4args {
	nfs_lock_type4 locktype;
	bool_t reclaim;
	offset4 offset;
	length4 length;
	locker4 locker;
};
typedef struct LOCK4args LOCK4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LOCK4args(...);
}
#else
bool_t xdr_LOCK4args();
#endif


struct LOCK4denied {
	offset4 offset;
	length4 length;
	nfs_lock_type4 locktype;
	lock_owner4 owner;
};
typedef struct LOCK4denied LOCK4denied;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LOCK4denied(...);
}
#else
bool_t xdr_LOCK4denied();
#endif


struct LOCK4resok {
	stateid4 lock_stateid;
};
typedef struct LOCK4resok LOCK4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LOCK4resok(...);
}
#else
bool_t xdr_LOCK4resok();
#endif


struct LOCK4res {
	nfsstat4 status;
	union {
		LOCK4resok resok4;
		LOCK4denied denied;
	} LOCK4res_u;
};
typedef struct LOCK4res LOCK4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LOCK4res(...);
}
#else
bool_t xdr_LOCK4res();
#endif


struct LOCKT4args {
	nfs_lock_type4 locktype;
	offset4 offset;
	length4 length;
	lock_owner4 owner;
};
typedef struct LOCKT4args LOCKT4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LOCKT4args(...);
}
#else
bool_t xdr_LOCKT4args();
#endif


struct LOCKT4res {
	nfsstat4 status;
	union {
		LOCK4denied denied;
	} LOCKT4res_u;
};
typedef struct LOCKT4res LOCKT4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LOCKT4res(...);
}
#else
bool_t xdr_LOCKT4res();
#endif


struct LOCKU4args {
	nfs_lock_type4 locktype;
	seqid4 seqid;
	stateid4 lock_stateid;
	offset4 offset;
	length4 length;
};
typedef struct LOCKU4args LOCKU4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LOCKU4args(...);
}
#else
bool_t xdr_LOCKU4args();
#endif


struct LOCKU4res {
	nfsstat4 status;
	union {
		stateid4 lock_stateid;
	} LOCKU4res_u;
};
typedef struct LOCKU4res LOCKU4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LOCKU4res(...);
}
#else
bool_t xdr_LOCKU4res();
#endif


struct LOOKUP4args {
	component4 objname;
};
typedef struct LOOKUP4args LOOKUP4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LOOKUP4args(...);
}
#else
bool_t xdr_LOOKUP4args();
#endif


struct LOOKUP4res {
	nfsstat4 status;
};
typedef struct LOOKUP4res LOOKUP4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LOOKUP4res(...);
}
#else
bool_t xdr_LOOKUP4res();
#endif


struct LOOKUPP4res {
	nfsstat4 status;
};
typedef struct LOOKUPP4res LOOKUPP4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LOOKUPP4res(...);
}
#else
bool_t xdr_LOOKUPP4res();
#endif


struct NVERIFY4args {
	fattr4 obj_attributes;
};
typedef struct NVERIFY4args NVERIFY4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_NVERIFY4args(...);
}
#else
bool_t xdr_NVERIFY4args();
#endif


struct NVERIFY4res {
	nfsstat4 status;
};
typedef struct NVERIFY4res NVERIFY4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_NVERIFY4res(...);
}
#else
bool_t xdr_NVERIFY4res();
#endif


enum createmode4 {
	UNCHECKED4 = 0,
	GUARDED4 = 1,
	EXCLUSIVE4 = 2,
	EXCLUSIVE4_1 = 3,
};
typedef enum createmode4 createmode4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_createmode4(...);
}
#else
bool_t xdr_createmode4();
#endif


struct creatverfattr {
	verifier4 cva_verf;
	fattr4 cva_attrs;
};
typedef struct creatverfattr creatverfattr;
#ifdef __cplusplus
extern "C" {
bool_t xdr_creatverfattr(...);
}
#else
bool_t xdr_creatverfattr();
#endif


struct createhow4 {
	createmode4 mode;
	union {
		fattr4 createattrs;
		verifier4 createverf;
		creatverfattr ch_createboth;
	} createhow4_u;
};
typedef struct createhow4 createhow4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_createhow4(...);
}
#else
bool_t xdr_createhow4();
#endif


enum opentype4 {
	OPEN4_NOCREATE = 0,
	OPEN4_CREATE = 1,
};
typedef enum opentype4 opentype4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_opentype4(...);
}
#else
bool_t xdr_opentype4();
#endif


struct openflag4 {
	opentype4 opentype;
	union {
		createhow4 how;
	} openflag4_u;
};
typedef struct openflag4 openflag4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_openflag4(...);
}
#else
bool_t xdr_openflag4();
#endif


enum limit_by4 {
	NFS_LIMIT_SIZE = 1,
	NFS_LIMIT_BLOCKS = 2,
};
typedef enum limit_by4 limit_by4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_limit_by4(...);
}
#else
bool_t xdr_limit_by4();
#endif


struct nfs_modified_limit4 {
	uint32_t num_blocks;
	uint32_t bytes_per_block;
};
typedef struct nfs_modified_limit4 nfs_modified_limit4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfs_modified_limit4(...);
}
#else
bool_t xdr_nfs_modified_limit4();
#endif


struct nfs_space_limit4 {
	limit_by4 limitby;
	union {
		uint64_t filesize;
		nfs_modified_limit4 mod_blocks;
	} nfs_space_limit4_u;
};
typedef struct nfs_space_limit4 nfs_space_limit4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfs_space_limit4(...);
}
#else
bool_t xdr_nfs_space_limit4();
#endif

#define OPEN4_SHARE_ACCESS_READ 0x00000001
#define OPEN4_SHARE_ACCESS_WRITE 0x00000002
#define OPEN4_SHARE_ACCESS_BOTH 0x00000003
#define OPEN4_SHARE_DENY_NONE 0x00000000
#define OPEN4_SHARE_DENY_READ 0x00000001
#define OPEN4_SHARE_DENY_WRITE 0x00000002
#define OPEN4_SHARE_DENY_BOTH 0x00000003
#define OPEN4_SHARE_ACCESS_WANT_DELEG_MASK 0xFF00
#define OPEN4_SHARE_ACCESS_WANT_NO_PREFERENCE 0x0000
#define OPEN4_SHARE_ACCESS_WANT_READ_DELEG 0x0100
#define OPEN4_SHARE_ACCESS_WANT_WRITE_DELEG 0x0200
#define OPEN4_SHARE_ACCESS_WANT_ANY_DELEG 0x0300
#define OPEN4_SHARE_ACCESS_WANT_NO_DELEG 0x0400
#define OPEN4_SHARE_ACCESS_WANT_CANCEL 0x0500
#define OPEN4_SHARE_ACCESS_WANT_SIGNAL_DELEG_WHEN_RESRC_AVAIL 0x10000
#define OPEN4_SHARE_ACCESS_WANT_PUSH_DELEG_WHEN_UNCONTENDED 0x20000

enum open_delegation_type4 {
	OPEN_DELEGATE_NONE = 0,
	OPEN_DELEGATE_READ = 1,
	OPEN_DELEGATE_WRITE = 2,
	OPEN_DELEGATE_NONE_EXT = 3,
};
typedef enum open_delegation_type4 open_delegation_type4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_open_delegation_type4(...);
}
#else
bool_t xdr_open_delegation_type4();
#endif


enum open_claim_type4 {
	CLAIM_NULL = 0,
	CLAIM_PREVIOUS = 1,
	CLAIM_DELEGATE_CUR = 2,
	CLAIM_DELEGATE_PREV = 3,
	CLAIM_FH = 4,
	CLAIM_DELEG_CUR_FH = 5,
	CLAIM_DELEG_PREV_FH = 6,
};
typedef enum open_claim_type4 open_claim_type4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_open_claim_type4(...);
}
#else
bool_t xdr_open_claim_type4();
#endif


struct open_claim_delegate_cur4 {
	stateid4 delegate_stateid;
	component4 file;
};
typedef struct open_claim_delegate_cur4 open_claim_delegate_cur4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_open_claim_delegate_cur4(...);
}
#else
bool_t xdr_open_claim_delegate_cur4();
#endif


struct open_claim4 {
	open_claim_type4 claim;
	union {
		component4 file;
		open_delegation_type4 delegate_type;
		open_claim_delegate_cur4 delegate_cur_info;
		component4 file_delegate_prev;
		stateid4 oc_delegate_stateid;
	} open_claim4_u;
};
typedef struct open_claim4 open_claim4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_open_claim4(...);
}
#else
bool_t xdr_open_claim4();
#endif


struct OPEN4args {
	seqid4 seqid;
	uint32_t share_access;
	uint32_t share_deny;
	open_owner4 owner;
	openflag4 openhow;
	open_claim4 claim;
};
typedef struct OPEN4args OPEN4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_OPEN4args(...);
}
#else
bool_t xdr_OPEN4args();
#endif


struct open_read_delegation4 {
	stateid4 stateid;
	bool_t recall;
	nfsace4 permissions;
};
typedef struct open_read_delegation4 open_read_delegation4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_open_read_delegation4(...);
}
#else
bool_t xdr_open_read_delegation4();
#endif


struct open_write_delegation4 {
	stateid4 stateid;
	bool_t recall;
	nfs_space_limit4 space_limit;
	nfsace4 permissions;
};
typedef struct open_write_delegation4 open_write_delegation4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_open_write_delegation4(...);
}
#else
bool_t xdr_open_write_delegation4();
#endif


enum why_no_delegation4 {
	WND4_NOT_WANTED = 0,
	WND4_CONTENTION = 1,
	WND4_RESOURCE = 2,
	WND4_NOT_SUPP_FTYPE = 3,
	WND4_WRITE_DELEG_NOT_SUPP_FTYPE = 4,
	WND4_NOT_SUPP_UPGRADE = 5,
	WND4_NOT_SUPP_DOWNGRADE = 6,
	WND4_CANCELED = 7,
	WND4_IS_DIR = 8,
};
typedef enum why_no_delegation4 why_no_delegation4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_why_no_delegation4(...);
}
#else
bool_t xdr_why_no_delegation4();
#endif


struct open_none_delegation4 {
	why_no_delegation4 ond_why;
	union {
		bool_t ond_server_will_push_deleg;
		bool_t ond_server_will_signal_avail;
	} open_none_delegation4_u;
};
typedef struct open_none_delegation4 open_none_delegation4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_open_none_delegation4(...);
}
#else
bool_t xdr_open_none_delegation4();
#endif


struct open_delegation4 {
	open_delegation_type4 delegation_type;
	union {
		open_read_delegation4 read;
		open_write_delegation4 write;
		open_none_delegation4 od_whynone;
	} open_delegation4_u;
};
typedef struct open_delegation4 open_delegation4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_open_delegation4(...);
}
#else
bool_t xdr_open_delegation4();
#endif

#define OPEN4_RESULT_CONFIRM 0x00000002
#define OPEN4_RESULT_LOCKTYPE_POSIX 0x00000004
#define OPEN4_RESULT_PRESERVE_UNLINKED 0x00000008
#define OPEN4_RESULT_MAY_NOTIFY_LOCK 0x00000020

struct OPEN4resok {
	stateid4 stateid;
	change_info4 cinfo;
	uint32_t rflags;
	bitmap4 attrset;
	open_delegation4 delegation;
};
typedef struct OPEN4resok OPEN4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_OPEN4resok(...);
}
#else
bool_t xdr_OPEN4resok();
#endif


struct OPEN4res {
	nfsstat4 status;
	union {
		OPEN4resok resok4;
	} OPEN4res_u;
};
typedef struct OPEN4res OPEN4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_OPEN4res(...);
}
#else
bool_t xdr_OPEN4res();
#endif


struct OPENATTR4args {
	bool_t createdir;
};
typedef struct OPENATTR4args OPENATTR4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_OPENATTR4args(...);
}
#else
bool_t xdr_OPENATTR4args();
#endif


struct OPENATTR4res {
	nfsstat4 status;
};
typedef struct OPENATTR4res OPENATTR4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_OPENATTR4res(...);
}
#else
bool_t xdr_OPENATTR4res();
#endif


struct OPEN_CONFIRM4args {
	stateid4 open_stateid;
	seqid4 seqid;
};
typedef struct OPEN_CONFIRM4args OPEN_CONFIRM4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_OPEN_CONFIRM4args(...);
}
#else
bool_t xdr_OPEN_CONFIRM4args();
#endif


struct OPEN_CONFIRM4resok {
	stateid4 open_stateid;
};
typedef struct OPEN_CONFIRM4resok OPEN_CONFIRM4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_OPEN_CONFIRM4resok(...);
}
#else
bool_t xdr_OPEN_CONFIRM4resok();
#endif


struct OPEN_CONFIRM4res {
	nfsstat4 status;
	union {
		OPEN_CONFIRM4resok resok4;
	} OPEN_CONFIRM4res_u;
};
typedef struct OPEN_CONFIRM4res OPEN_CONFIRM4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_OPEN_CONFIRM4res(...);
}
#else
bool_t xdr_OPEN_CONFIRM4res();
#endif


struct OPEN_DOWNGRADE4args {
	stateid4 open_stateid;
	seqid4 seqid;
	uint32_t share_access;
	uint32_t share_deny;
};
typedef struct OPEN_DOWNGRADE4args OPEN_DOWNGRADE4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_OPEN_DOWNGRADE4args(...);
}
#else
bool_t xdr_OPEN_DOWNGRADE4args();
#endif


struct OPEN_DOWNGRADE4resok {
	stateid4 open_stateid;
};
typedef struct OPEN_DOWNGRADE4resok OPEN_DOWNGRADE4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_OPEN_DOWNGRADE4resok(...);
}
#else
bool_t xdr_OPEN_DOWNGRADE4resok();
#endif


struct OPEN_DOWNGRADE4res {
	nfsstat4 status;
	union {
		OPEN_DOWNGRADE4resok resok4;
	} OPEN_DOWNGRADE4res_u;
};
typedef struct OPEN_DOWNGRADE4res OPEN_DOWNGRADE4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_OPEN_DOWNGRADE4res(...);
}
#else
bool_t xdr_OPEN_DOWNGRADE4res();
#endif


struct PUTFH4args {
	nfs_fh4 object;
};
typedef struct PUTFH4args PUTFH4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_PUTFH4args(...);
}
#else
bool_t xdr_PUTFH4args();
#endif


struct PUTFH4res {
	nfsstat4 status;
};
typedef struct PUTFH4res PUTFH4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_PUTFH4res(...);
}
#else
bool_t xdr_PUTFH4res();
#endif


struct PUTPUBFH4res {
	nfsstat4 status;
};
typedef struct PUTPUBFH4res PUTPUBFH4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_PUTPUBFH4res(...);
}
#else
bool_t xdr_PUTPUBFH4res();
#endif


struct PUTROOTFH4res {
	nfsstat4 status;
};
typedef struct PUTROOTFH4res PUTROOTFH4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_PUTROOTFH4res(...);
}
#else
bool_t xdr_PUTROOTFH4res();
#endif


struct READ4args {
	stateid4 stateid;
	offset4 offset;
	count4 count;
};
typedef struct READ4args READ4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READ4args(...);
}
#else
bool_t xdr_READ4args();
#endif


struct READ4resok {
	bool_t eof;
	struct {
		u_int data_len;
		char *data_val;
	} data;
};
typedef struct READ4resok READ4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READ4resok(...);
}
#else
bool_t xdr_READ4resok();
#endif


struct READ4res {
	nfsstat4 status;
	union {
		READ4resok resok4;
	} READ4res_u;
};
typedef struct READ4res READ4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READ4res(...);
}
#else
bool_t xdr_READ4res();
#endif


struct READDIR4args {
	nfs_cookie4 cookie;
	verifier4 cookieverf;
	count4 dircount;
	count4 maxcount;
	bitmap4 attr_request;
};
typedef struct READDIR4args READDIR4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READDIR4args(...);
}
#else
bool_t xdr_READDIR4args();
#endif


struct entry4 {
	nfs_cookie4 cookie;
	component4 name;
	fattr4 attrs;
	struct entry4 *nextentry;
};
typedef struct entry4 entry4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_entry4(...);
}
#else
bool_t xdr_entry4();
#endif


struct dirlist4 {
	entry4 *entries;
	bool_t eof;
};
typedef struct dirlist4 dirlist4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_dirlist4(...);
}
#else
bool_t xdr_dirlist4();
#endif


struct READDIR4resok {
	verifier4 cookieverf;
	dirlist4 reply;
};
typedef struct READDIR4resok READDIR4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READDIR4resok(...);
}
#else
bool_t xdr_READDIR4resok();
#endif


struct READDIR4res {
	nfsstat4 status;
	union {
		READDIR4resok resok4;
	} READDIR4res_u;
};
typedef struct READDIR4res READDIR4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READDIR4res(...);
}
#else
bool_t xdr_READDIR4res();
#endif


struct READLINK4resok {
	linktext4 link;
};
typedef struct READLINK4resok READLINK4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READLINK4resok(...);
}
#else
bool_t xdr_READLINK4resok();
#endif


struct READLINK4res {
	nfsstat4 status;
	union {
		READLINK4resok resok4;
	} READLINK4res_u;
};
typedef struct READLINK4res READLINK4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_READLINK4res(...);
}
#else
bool_t xdr_READLINK4res();
#endif


struct REMOVE4args {
	component4 target;
};
typedef struct REMOVE4args REMOVE4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_REMOVE4args(...);
}
#else
bool_t xdr_REMOVE4args();
#endif


struct REMOVE4resok {
	change_info4 cinfo;
};
typedef struct REMOVE4resok REMOVE4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_REMOVE4resok(...);
}
#else
bool_t xdr_REMOVE4resok();
#endif


struct REMOVE4res {
	nfsstat4 status;
	union {
		REMOVE4resok resok4;
	} REMOVE4res_u;
};
typedef struct REMOVE4res REMOVE4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_REMOVE4res(...);
}
#else
bool_t xdr_REMOVE4res();
#endif


struct RENAME4args {
	component4 oldname;
	component4 newname;
};
typedef struct RENAME4args RENAME4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RENAME4args(...);
}
#else
bool_t xdr_RENAME4args();
#endif


struct RENAME4resok {
	change_info4 source_cinfo;
	change_info4 target_cinfo;
};
typedef struct RENAME4resok RENAME4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RENAME4resok(...);
}
#else
bool_t xdr_RENAME4resok();
#endif


struct RENAME4res {
	nfsstat4 status;
	union {
		RENAME4resok resok4;
	} RENAME4res_u;
};
typedef struct RENAME4res RENAME4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RENAME4res(...);
}
#else
bool_t xdr_RENAME4res();
#endif


struct RENEW4args {
	clientid4 clientid;
};
typedef struct RENEW4args RENEW4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RENEW4args(...);
}
#else
bool_t xdr_RENEW4args();
#endif


struct RENEW4res {
	nfsstat4 status;
};
typedef struct RENEW4res RENEW4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RENEW4res(...);
}
#else
bool_t xdr_RENEW4res();
#endif


struct RESTOREFH4res {
	nfsstat4 status;
};
typedef struct RESTOREFH4res RESTOREFH4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RESTOREFH4res(...);
}
#else
bool_t xdr_RESTOREFH4res();
#endif


struct SAVEFH4res {
	nfsstat4 status;
};
typedef struct SAVEFH4res SAVEFH4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SAVEFH4res(...);
}
#else
bool_t xdr_SAVEFH4res();
#endif


struct SECINFO4args {
	component4 name;
};
typedef struct SECINFO4args SECINFO4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SECINFO4args(...);
}
#else
bool_t xdr_SECINFO4args();
#endif


enum rpc_gss_svc_t {
	RPC_GSS_SVC_NONE = 1,
	RPC_GSS_SVC_INTEGRITY = 2,
	RPC_GSS_SVC_PRIVACY = 3,
};
typedef enum rpc_gss_svc_t rpc_gss_svc_t;
#ifdef __cplusplus
extern "C" {
bool_t xdr_rpc_gss_svc_t(...);
}
#else
bool_t xdr_rpc_gss_svc_t();
#endif


struct rpcsec_gss_info {
	sec_oid4 oid;
	qop4 qop;
	rpc_gss_svc_t service;
};
typedef struct rpcsec_gss_info rpcsec_gss_info;
#ifdef __cplusplus
extern "C" {
bool_t xdr_rpcsec_gss_info(...);
}
#else
bool_t xdr_rpcsec_gss_info();
#endif


struct secinfo4 {
	uint32_t flavor;
	union {
		rpcsec_gss_info flavor_info;
	} secinfo4_u;
};
typedef struct secinfo4 secinfo4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_secinfo4(...);
}
#else
bool_t xdr_secinfo4();
#endif


typedef struct {
	u_int SECINFO4resok_len;
	secinfo4 *SECINFO4resok_val;
} SECINFO4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SECINFO4resok(...);
}
#else
bool_t xdr_SECINFO4resok();
#endif


struct SECINFO4res {
	nfsstat4 status;
	union {
		SECINFO4resok resok4;
	} SECINFO4res_u;
};
typedef struct SECINFO4res SECINFO4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SECINFO4res(...);
}
#else
bool_t xdr_SECINFO4res();
#endif


struct SETATTR4args {
	stateid4 stateid;
	fattr4 obj_attributes;
};
typedef struct SETATTR4args SETATTR4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SETATTR4args(...);
}
#else
bool_t xdr_SETATTR4args();
#endif


struct SETATTR4res {
	nfsstat4 status;
	bitmap4 attrsset;
};
typedef struct SETATTR4res SETATTR4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SETATTR4res(...);
}
#else
bool_t xdr_SETATTR4res();
#endif


struct SETCLIENTID4args {
	nfs_client_id4 client;
	cb_client4 callback;
	uint32_t callback_ident;
};
typedef struct SETCLIENTID4args SETCLIENTID4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SETCLIENTID4args(...);
}
#else
bool_t xdr_SETCLIENTID4args();
#endif


struct SETCLIENTID4resok {
	clientid4 clientid;
	verifier4 setclientid_confirm;
};
typedef struct SETCLIENTID4resok SETCLIENTID4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SETCLIENTID4resok(...);
}
#else
bool_t xdr_SETCLIENTID4resok();
#endif


struct SETCLIENTID4res {
	nfsstat4 status;
	union {
		SETCLIENTID4resok resok4;
		clientaddr4 client_using;
	} SETCLIENTID4res_u;
};
typedef struct SETCLIENTID4res SETCLIENTID4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SETCLIENTID4res(...);
}
#else
bool_t xdr_SETCLIENTID4res();
#endif


struct SETCLIENTID_CONFIRM4args {
	clientid4 clientid;
	verifier4 setclientid_confirm;
};
typedef struct SETCLIENTID_CONFIRM4args SETCLIENTID_CONFIRM4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SETCLIENTID_CONFIRM4args(...);
}
#else
bool_t xdr_SETCLIENTID_CONFIRM4args();
#endif


struct SETCLIENTID_CONFIRM4res {
	nfsstat4 status;
};
typedef struct SETCLIENTID_CONFIRM4res SETCLIENTID_CONFIRM4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SETCLIENTID_CONFIRM4res(...);
}
#else
bool_t xdr_SETCLIENTID_CONFIRM4res();
#endif


struct VERIFY4args {
	fattr4 obj_attributes;
};
typedef struct VERIFY4args VERIFY4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_VERIFY4args(...);
}
#else
bool_t xdr_VERIFY4args();
#endif


struct VERIFY4res {
	nfsstat4 status;
};
typedef struct VERIFY4res VERIFY4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_VERIFY4res(...);
}
#else
bool_t xdr_VERIFY4res();
#endif


enum stable_how4 {
	UNSTABLE4 = 0,
	DATA_SYNC4 = 1,
	FILE_SYNC4 = 2,
};
typedef enum stable_how4 stable_how4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_stable_how4(...);
}
#else
bool_t xdr_stable_how4();
#endif


struct WRITE4args {
	stateid4 stateid;
	offset4 offset;
	stable_how4 stable;
	struct {
		u_int data_len;
		char *data_val;
	} data;
};
typedef struct WRITE4args WRITE4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_WRITE4args(...);
}
#else
bool_t xdr_WRITE4args();
#endif


struct WRITE4resok {
	count4 count;
	stable_how4 committed;
	verifier4 writeverf;
};
typedef struct WRITE4resok WRITE4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_WRITE4resok(...);
}
#else
bool_t xdr_WRITE4resok();
#endif


struct WRITE4res {
	nfsstat4 status;
	union {
		WRITE4resok resok4;
	} WRITE4res_u;
};
typedef struct WRITE4res WRITE4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_WRITE4res(...);
}
#else
bool_t xdr_WRITE4res();
#endif


struct RELEASE_LOCKOWNER4args {
	lock_owner4 lock_owner;
};
typedef struct RELEASE_LOCKOWNER4args RELEASE_LOCKOWNER4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RELEASE_LOCKOWNER4args(...);
}
#else
bool_t xdr_RELEASE_LOCKOWNER4args();
#endif


struct RELEASE_LOCKOWNER4res {
	nfsstat4 status;
};
typedef struct RELEASE_LOCKOWNER4res RELEASE_LOCKOWNER4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RELEASE_LOCKOWNER4res(...);
}
#else
bool_t xdr_RELEASE_LOCKOWNER4res();
#endif


struct ILLEGAL4res {
	nfsstat4 status;
};
typedef struct ILLEGAL4res ILLEGAL4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ILLEGAL4res(...);
}
#else
bool_t xdr_ILLEGAL4res();
#endif


typedef struct {
	u_int gsshandle4_t_len;
	char *gsshandle4_t_val;
} gsshandle4_t;
#ifdef __cplusplus
extern "C" {
bool_t xdr_gsshandle4_t(...);
}
#else
bool_t xdr_gsshandle4_t();
#endif


struct gss_cb_handles4 {
	rpc_gss_svc_t gcbp_service;
	gsshandle4_t gcbp_handle_from_server;
	gsshandle4_t gcbp_handle_from_client;
};
typedef struct gss_cb_handles4 gss_cb_handles4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_gss_cb_handles4(...);
}
#else
bool_t xdr_gss_cb_handles4();
#endif


struct callback_sec_parms4 {
	uint32_t cb_secflavor;
	union {
		authsys_parms cbsp_sys_cred;
		gss_cb_handles4 cbsp_gss_handles;
	} callback_sec_parms4_u;
};
typedef struct callback_sec_parms4 callback_sec_parms4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_callback_sec_parms4(...);
}
#else
bool_t xdr_callback_sec_parms4();
#endif


struct BACKCHANNEL_CTL4args {
	uint32_t bca_cb_program;
	struct {
		u_int bca_sec_parms_len;
		callback_sec_parms4 *bca_sec_parms_val;
	} bca_sec_parms;
};
typedef struct BACKCHANNEL_CTL4args BACKCHANNEL_CTL4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_BACKCHANNEL_CTL4args(...);
}
#else
bool_t xdr_BACKCHANNEL_CTL4args();
#endif


struct BACKCHANNEL_CTL4res {
	nfsstat4 bcr_status;
};
typedef struct BACKCHANNEL_CTL4res BACKCHANNEL_CTL4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_BACKCHANNEL_CTL4res(...);
}
#else
bool_t xdr_BACKCHANNEL_CTL4res();
#endif


enum channel_dir_from_client4 {
	CDFC4_FORE = 0x1,
	CDFC4_BACK = 0x2,
	CDFC4_FORE_OR_BOTH = 0x3,
	CDFC4_BACK_OR_BOTH = 0x7,
};
typedef enum channel_dir_from_client4 channel_dir_from_client4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_channel_dir_from_client4(...);
}
#else
bool_t xdr_channel_dir_from_client4();
#endif


struct BIND_CONN_TO_SESSION4args {
	sessionid4 bctsa_sessid;
	channel_dir_from_client4 bctsa_dir;
	bool_t bctsa_use_conn_in_rdma_mode;
};
typedef struct BIND_CONN_TO_SESSION4args BIND_CONN_TO_SESSION4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_BIND_CONN_TO_SESSION4args(...);
}
#else
bool_t xdr_BIND_CONN_TO_SESSION4args();
#endif


enum channel_dir_from_server4 {
	CDFS4_FORE = 0x1,
	CDFS4_BACK = 0x2,
	CDFS4_BOTH = 0x3,
};
typedef enum channel_dir_from_server4 channel_dir_from_server4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_channel_dir_from_server4(...);
}
#else
bool_t xdr_channel_dir_from_server4();
#endif


struct BIND_CONN_TO_SESSION4resok {
	sessionid4 bctsr_sessid;
	channel_dir_from_server4 bctsr_dir;
	bool_t bctsr_use_conn_in_rdma_mode;
};
typedef struct BIND_CONN_TO_SESSION4resok BIND_CONN_TO_SESSION4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_BIND_CONN_TO_SESSION4resok(...);
}
#else
bool_t xdr_BIND_CONN_TO_SESSION4resok();
#endif


struct BIND_CONN_TO_SESSION4res {
	nfsstat4 bctsr_status;
	union {
		BIND_CONN_TO_SESSION4resok bctsr_resok4;
	} BIND_CONN_TO_SESSION4res_u;
};
typedef struct BIND_CONN_TO_SESSION4res BIND_CONN_TO_SESSION4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_BIND_CONN_TO_SESSION4res(...);
}
#else
bool_t xdr_BIND_CONN_TO_SESSION4res();
#endif

#define EXCHGID4_FLAG_SUPP_MOVED_REFER 0x00000001
#define EXCHGID4_FLAG_SUPP_MOVED_MIGR 0x00000002
#define EXCHGID4_FLAG_BIND_PRINC_STATEID 0x00000100
#define EXCHGID4_FLAG_USE_NON_PNFS 0x00010000
#define EXCHGID4_FLAG_USE_PNFS_MDS 0x00020000
#define EXCHGID4_FLAG_USE_PNFS_DS 0x00040000
#define EXCHGID4_FLAG_MASK_PNFS 0x00070000
#define EXCHGID4_FLAG_UPD_CONFIRMED_REC_A 0x40000000
#define EXCHGID4_FLAG_CONFIRMED_R 0x80000000

struct state_protect_ops4 {
	bitmap4 spo_must_enforce;
	bitmap4 spo_must_allow;
};
typedef struct state_protect_ops4 state_protect_ops4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_state_protect_ops4(...);
}
#else
bool_t xdr_state_protect_ops4();
#endif


struct ssv_sp_parms4 {
	state_protect_ops4 ssp_ops;
	struct {
		u_int ssp_hash_algs_len;
		sec_oid4 *ssp_hash_algs_val;
	} ssp_hash_algs;
	struct {
		u_int ssp_encr_algs_len;
		sec_oid4 *ssp_encr_algs_val;
	} ssp_encr_algs;
	uint32_t ssp_window;
	uint32_t ssp_num_gss_handles;
};
typedef struct ssv_sp_parms4 ssv_sp_parms4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ssv_sp_parms4(...);
}
#else
bool_t xdr_ssv_sp_parms4();
#endif


enum state_protect_how4 {
	SP4_NONE = 0,
	SP4_MACH_CRED = 1,
	SP4_SSV = 2,
};
typedef enum state_protect_how4 state_protect_how4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_state_protect_how4(...);
}
#else
bool_t xdr_state_protect_how4();
#endif


struct state_protect4_a {
	state_protect_how4 spa_how;
	union {
		state_protect_ops4 spa_mach_ops;
		ssv_sp_parms4 spa_ssv_parms;
	} state_protect4_a_u;
};
typedef struct state_protect4_a state_protect4_a;
#ifdef __cplusplus
extern "C" {
bool_t xdr_state_protect4_a(...);
}
#else
bool_t xdr_state_protect4_a();
#endif


struct EXCHANGE_ID4args {
	client_owner4 eia_clientowner;
	uint32_t eia_flags;
	state_protect4_a eia_state_protect;
	struct {
		u_int eia_client_impl_id_len;
		nfs_impl_id4 *eia_client_impl_id_val;
	} eia_client_impl_id;
};
typedef struct EXCHANGE_ID4args EXCHANGE_ID4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_EXCHANGE_ID4args(...);
}
#else
bool_t xdr_EXCHANGE_ID4args();
#endif


struct ssv_prot_info4 {
	state_protect_ops4 spi_ops;
	uint32_t spi_hash_alg;
	uint32_t spi_encr_alg;
	uint32_t spi_ssv_len;
	uint32_t spi_window;
	struct {
		u_int spi_handles_len;
		gsshandle4_t *spi_handles_val;
	} spi_handles;
};
typedef struct ssv_prot_info4 ssv_prot_info4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ssv_prot_info4(...);
}
#else
bool_t xdr_ssv_prot_info4();
#endif


struct state_protect4_r {
	state_protect_how4 spr_how;
	union {
		state_protect_ops4 spr_mach_ops;
		ssv_prot_info4 spr_ssv_info;
	} state_protect4_r_u;
};
typedef struct state_protect4_r state_protect4_r;
#ifdef __cplusplus
extern "C" {
bool_t xdr_state_protect4_r(...);
}
#else
bool_t xdr_state_protect4_r();
#endif


struct EXCHANGE_ID4resok {
	clientid4 eir_clientid;
	sequenceid4 eir_sequenceid;
	uint32_t eir_flags;
	state_protect4_r eir_state_protect;
	server_owner4 eir_server_owner;
	struct {
		u_int eir_server_scope_len;
		char *eir_server_scope_val;
	} eir_server_scope;
	struct {
		u_int eir_server_impl_id_len;
		nfs_impl_id4 *eir_server_impl_id_val;
	} eir_server_impl_id;
};
typedef struct EXCHANGE_ID4resok EXCHANGE_ID4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_EXCHANGE_ID4resok(...);
}
#else
bool_t xdr_EXCHANGE_ID4resok();
#endif


struct EXCHANGE_ID4res {
	nfsstat4 eir_status;
	union {
		EXCHANGE_ID4resok eir_resok4;
	} EXCHANGE_ID4res_u;
};
typedef struct EXCHANGE_ID4res EXCHANGE_ID4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_EXCHANGE_ID4res(...);
}
#else
bool_t xdr_EXCHANGE_ID4res();
#endif


struct channel_attrs4 {
	count4 ca_headerpadsize;
	count4 ca_maxrequestsize;
	count4 ca_maxresponsesize;
	count4 ca_maxresponsesize_cached;
	count4 ca_maxoperations;
	count4 ca_maxrequests;
	struct {
		u_int ca_rdma_ird_len;
		uint32_t *ca_rdma_ird_val;
	} ca_rdma_ird;
};
typedef struct channel_attrs4 channel_attrs4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_channel_attrs4(...);
}
#else
bool_t xdr_channel_attrs4();
#endif

#define CREATE_SESSION4_FLAG_PERSIST 0x00000001
#define CREATE_SESSION4_FLAG_CONN_BACK_CHAN 0x00000002
#define CREATE_SESSION4_FLAG_CONN_RDMA 0x00000004

struct CREATE_SESSION4args {
	clientid4 csa_clientid;
	sequenceid4 csa_sequence;
	uint32_t csa_flags;
	channel_attrs4 csa_fore_chan_attrs;
	channel_attrs4 csa_back_chan_attrs;
	uint32_t csa_cb_program;
	struct {
		u_int csa_sec_parms_len;
		callback_sec_parms4 *csa_sec_parms_val;
	} csa_sec_parms;
};
typedef struct CREATE_SESSION4args CREATE_SESSION4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CREATE_SESSION4args(...);
}
#else
bool_t xdr_CREATE_SESSION4args();
#endif


struct CREATE_SESSION4resok {
	sessionid4 csr_sessionid;
	sequenceid4 csr_sequence;
	uint32_t csr_flags;
	channel_attrs4 csr_fore_chan_attrs;
	channel_attrs4 csr_back_chan_attrs;
};
typedef struct CREATE_SESSION4resok CREATE_SESSION4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CREATE_SESSION4resok(...);
}
#else
bool_t xdr_CREATE_SESSION4resok();
#endif


struct CREATE_SESSION4res {
	nfsstat4 csr_status;
	union {
		CREATE_SESSION4resok csr_resok4;
	} CREATE_SESSION4res_u;
};
typedef struct CREATE_SESSION4res CREATE_SESSION4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CREATE_SESSION4res(...);
}
#else
bool_t xdr_CREATE_SESSION4res();
#endif


struct DESTROY_SESSION4args {
	sessionid4 dsa_sessionid;
};
typedef struct DESTROY_SESSION4args DESTROY_SESSION4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_DESTROY_SESSION4args(...);
}
#else
bool_t xdr_DESTROY_SESSION4args();
#endif


struct DESTROY_SESSION4res {
	nfsstat4 dsr_status;
};
typedef struct DESTROY_SESSION4res DESTROY_SESSION4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_DESTROY_SESSION4res(...);
}
#else
bool_t xdr_DESTROY_SESSION4res();
#endif


struct FREE_STATEID4args {
	stateid4 fsa_stateid;
};
typedef struct FREE_STATEID4args FREE_STATEID4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_FREE_STATEID4args(...);
}
#else
bool_t xdr_FREE_STATEID4args();
#endif


struct FREE_STATEID4res {
	nfsstat4 fsr_status;
};
typedef struct FREE_STATEID4res FREE_STATEID4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_FREE_STATEID4res(...);
}
#else
bool_t xdr_FREE_STATEID4res();
#endif


typedef nfstime4 attr_notice4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_attr_notice4(...);
}
#else
bool_t xdr_attr_notice4();
#endif


struct GET_DIR_DELEGATION4args {
	bool_t gdda_signal_deleg_avail;
	bitmap4 gdda_notification_types;
	attr_notice4 gdda_child_attr_delay;
	attr_notice4 gdda_dir_attr_delay;
	bitmap4 gdda_child_attributes;
	bitmap4 gdda_dir_attributes;
};
typedef struct GET_DIR_DELEGATION4args GET_DIR_DELEGATION4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GET_DIR_DELEGATION4args(...);
}
#else
bool_t xdr_GET_DIR_DELEGATION4args();
#endif


struct GET_DIR_DELEGATION4resok {
	verifier4 gddr_cookieverf;
	stateid4 gddr_stateid;
	bitmap4 gddr_notification;
	bitmap4 gddr_child_attributes;
	bitmap4 gddr_dir_attributes;
};
typedef struct GET_DIR_DELEGATION4resok GET_DIR_DELEGATION4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GET_DIR_DELEGATION4resok(...);
}
#else
bool_t xdr_GET_DIR_DELEGATION4resok();
#endif


enum gddrnf4_status {
	GDD4_OK = 0,
	GDD4_UNAVAIL = 1,
};
typedef enum gddrnf4_status gddrnf4_status;
#ifdef __cplusplus
extern "C" {
bool_t xdr_gddrnf4_status(...);
}
#else
bool_t xdr_gddrnf4_status();
#endif


struct GET_DIR_DELEGATION4res_non_fatal {
	gddrnf4_status gddrnf_status;
	union {
		GET_DIR_DELEGATION4resok gddrnf_resok4;
		bool_t gddrnf_will_signal_deleg_avail;
	} GET_DIR_DELEGATION4res_non_fatal_u;
};
typedef struct GET_DIR_DELEGATION4res_non_fatal GET_DIR_DELEGATION4res_non_fatal;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GET_DIR_DELEGATION4res_non_fatal(...);
}
#else
bool_t xdr_GET_DIR_DELEGATION4res_non_fatal();
#endif


struct GET_DIR_DELEGATION4res {
	nfsstat4 gddr_status;
	union {
		GET_DIR_DELEGATION4res_non_fatal gddr_res_non_fatal4;
	} GET_DIR_DELEGATION4res_u;
};
typedef struct GET_DIR_DELEGATION4res GET_DIR_DELEGATION4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GET_DIR_DELEGATION4res(...);
}
#else
bool_t xdr_GET_DIR_DELEGATION4res();
#endif


struct GETDEVICEINFO4args {
	deviceid4 gdia_device_id;
	layouttype4 gdia_layout_type;
	count4 gdia_maxcount;
	bitmap4 gdia_notify_types;
};
typedef struct GETDEVICEINFO4args GETDEVICEINFO4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GETDEVICEINFO4args(...);
}
#else
bool_t xdr_GETDEVICEINFO4args();
#endif


struct GETDEVICEINFO4resok {
	device_addr4 gdir_device_addr;
	bitmap4 gdir_notification;
};
typedef struct GETDEVICEINFO4resok GETDEVICEINFO4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GETDEVICEINFO4resok(...);
}
#else
bool_t xdr_GETDEVICEINFO4resok();
#endif


struct GETDEVICEINFO4res {
	nfsstat4 gdir_status;
	union {
		GETDEVICEINFO4resok gdir_resok4;
		count4 gdir_mincount;
	} GETDEVICEINFO4res_u;
};
typedef struct GETDEVICEINFO4res GETDEVICEINFO4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GETDEVICEINFO4res(...);
}
#else
bool_t xdr_GETDEVICEINFO4res();
#endif


struct GETDEVICELIST4args {
	layouttype4 gdla_layout_type;
	count4 gdla_maxdevices;
	nfs_cookie4 gdla_cookie;
	verifier4 gdla_cookieverf;
};
typedef struct GETDEVICELIST4args GETDEVICELIST4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GETDEVICELIST4args(...);
}
#else
bool_t xdr_GETDEVICELIST4args();
#endif


struct GETDEVICELIST4resok {
	nfs_cookie4 gdlr_cookie;
	verifier4 gdlr_cookieverf;
	struct {
		u_int gdlr_deviceid_list_len;
		deviceid4 *gdlr_deviceid_list_val;
	} gdlr_deviceid_list;
	bool_t gdlr_eof;
};
typedef struct GETDEVICELIST4resok GETDEVICELIST4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GETDEVICELIST4resok(...);
}
#else
bool_t xdr_GETDEVICELIST4resok();
#endif


struct GETDEVICELIST4res {
	nfsstat4 gdlr_status;
	union {
		GETDEVICELIST4resok gdlr_resok4;
	} GETDEVICELIST4res_u;
};
typedef struct GETDEVICELIST4res GETDEVICELIST4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_GETDEVICELIST4res(...);
}
#else
bool_t xdr_GETDEVICELIST4res();
#endif


struct newtime4 {
	bool_t nt_timechanged;
	union {
		nfstime4 nt_time;
	} newtime4_u;
};
typedef struct newtime4 newtime4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_newtime4(...);
}
#else
bool_t xdr_newtime4();
#endif


struct newoffset4 {
	bool_t no_newoffset;
	union {
		offset4 no_offset;
	} newoffset4_u;
};
typedef struct newoffset4 newoffset4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_newoffset4(...);
}
#else
bool_t xdr_newoffset4();
#endif


struct LAYOUTCOMMIT4args {
	offset4 loca_offset;
	length4 loca_length;
	bool_t loca_reclaim;
	stateid4 loca_stateid;
	newoffset4 loca_last_write_offset;
	newtime4 loca_time_modify;
	layoutupdate4 loca_layoutupdate;
};
typedef struct LAYOUTCOMMIT4args LAYOUTCOMMIT4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LAYOUTCOMMIT4args(...);
}
#else
bool_t xdr_LAYOUTCOMMIT4args();
#endif


struct newsize4 {
	bool_t ns_sizechanged;
	union {
		length4 ns_size;
	} newsize4_u;
};
typedef struct newsize4 newsize4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_newsize4(...);
}
#else
bool_t xdr_newsize4();
#endif


struct LAYOUTCOMMIT4resok {
	newsize4 locr_newsize;
};
typedef struct LAYOUTCOMMIT4resok LAYOUTCOMMIT4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LAYOUTCOMMIT4resok(...);
}
#else
bool_t xdr_LAYOUTCOMMIT4resok();
#endif


struct LAYOUTCOMMIT4res {
	nfsstat4 locr_status;
	union {
		LAYOUTCOMMIT4resok locr_resok4;
	} LAYOUTCOMMIT4res_u;
};
typedef struct LAYOUTCOMMIT4res LAYOUTCOMMIT4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LAYOUTCOMMIT4res(...);
}
#else
bool_t xdr_LAYOUTCOMMIT4res();
#endif


struct LAYOUTGET4args {
	bool_t loga_signal_layout_avail;
	layouttype4 loga_layout_type;
	layoutiomode4 loga_iomode;
	offset4 loga_offset;
	length4 loga_length;
	length4 loga_minlength;
	stateid4 loga_stateid;
	count4 loga_maxcount;
};
typedef struct LAYOUTGET4args LAYOUTGET4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LAYOUTGET4args(...);
}
#else
bool_t xdr_LAYOUTGET4args();
#endif


struct LAYOUTGET4resok {
	bool_t logr_return_on_close;
	stateid4 logr_stateid;
	struct {
		u_int logr_layout_len;
		layout4 *logr_layout_val;
	} logr_layout;
};
typedef struct LAYOUTGET4resok LAYOUTGET4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LAYOUTGET4resok(...);
}
#else
bool_t xdr_LAYOUTGET4resok();
#endif


struct LAYOUTGET4res {
	nfsstat4 logr_status;
	union {
		LAYOUTGET4resok logr_resok4;
		bool_t logr_will_signal_layout_avail;
	} LAYOUTGET4res_u;
};
typedef struct LAYOUTGET4res LAYOUTGET4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LAYOUTGET4res(...);
}
#else
bool_t xdr_LAYOUTGET4res();
#endif


struct LAYOUTRETURN4args {
	bool_t lora_reclaim;
	layouttype4 lora_layout_type;
	layoutiomode4 lora_iomode;
	layoutreturn4 lora_layoutreturn;
};
typedef struct LAYOUTRETURN4args LAYOUTRETURN4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LAYOUTRETURN4args(...);
}
#else
bool_t xdr_LAYOUTRETURN4args();
#endif


struct layoutreturn_stateid {
	bool_t lrs_present;
	union {
		stateid4 lrs_stateid;
	} layoutreturn_stateid_u;
};
typedef struct layoutreturn_stateid layoutreturn_stateid;
#ifdef __cplusplus
extern "C" {
bool_t xdr_layoutreturn_stateid(...);
}
#else
bool_t xdr_layoutreturn_stateid();
#endif


struct LAYOUTRETURN4res {
	nfsstat4 lorr_status;
	union {
		layoutreturn_stateid lorr_stateid;
	} LAYOUTRETURN4res_u;
};
typedef struct LAYOUTRETURN4res LAYOUTRETURN4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_LAYOUTRETURN4res(...);
}
#else
bool_t xdr_LAYOUTRETURN4res();
#endif


enum secinfo_style4 {
	SECINFO_STYLE4_CURRENT_FH = 0,
	SECINFO_STYLE4_PARENT = 1,
};
typedef enum secinfo_style4 secinfo_style4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_secinfo_style4(...);
}
#else
bool_t xdr_secinfo_style4();
#endif


typedef secinfo_style4 SECINFO_NO_NAME4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SECINFO_NO_NAME4args(...);
}
#else
bool_t xdr_SECINFO_NO_NAME4args();
#endif


typedef SECINFO4res SECINFO_NO_NAME4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SECINFO_NO_NAME4res(...);
}
#else
bool_t xdr_SECINFO_NO_NAME4res();
#endif


struct SEQUENCE4args {
	sessionid4 sa_sessionid;
	sequenceid4 sa_sequenceid;
	slotid4 sa_slotid;
	slotid4 sa_highest_slotid;
	bool_t sa_cachethis;
};
typedef struct SEQUENCE4args SEQUENCE4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SEQUENCE4args(...);
}
#else
bool_t xdr_SEQUENCE4args();
#endif

#define SEQ4_STATUS_CB_PATH_DOWN 0x00000001
#define SEQ4_STATUS_CB_GSS_CONTEXTS_EXPIRING 0x00000002
#define SEQ4_STATUS_CB_GSS_CONTEXTS_EXPIRED 0x00000004
#define SEQ4_STATUS_EXPIRED_ALL_STATE_REVOKED 0x00000008
#define SEQ4_STATUS_EXPIRED_SOME_STATE_REVOKED 0x00000010
#define SEQ4_STATUS_ADMIN_STATE_REVOKED 0x00000020
#define SEQ4_STATUS_RECALLABLE_STATE_REVOKED 0x00000040
#define SEQ4_STATUS_LEASE_MOVED 0x00000080
#define SEQ4_STATUS_RESTART_RECLAIM_NEEDED 0x00000100
#define SEQ4_STATUS_CB_PATH_DOWN_SESSION 0x00000200
#define SEQ4_STATUS_BACKCHANNEL_FAULT 0x00000400
#define SEQ4_STATUS_DEVID_CHANGED 0x00000800
#define SEQ4_STATUS_DEVID_DELETED 0x00001000

struct SEQUENCE4resok {
	sessionid4 sr_sessionid;
	sequenceid4 sr_sequenceid;
	slotid4 sr_slotid;
	slotid4 sr_highest_slotid;
	slotid4 sr_target_highest_slotid;
	uint32_t sr_status_flags;
};
typedef struct SEQUENCE4resok SEQUENCE4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SEQUENCE4resok(...);
}
#else
bool_t xdr_SEQUENCE4resok();
#endif


struct SEQUENCE4res {
	nfsstat4 sr_status;
	union {
		SEQUENCE4resok sr_resok4;
	} SEQUENCE4res_u;
};
typedef struct SEQUENCE4res SEQUENCE4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SEQUENCE4res(...);
}
#else
bool_t xdr_SEQUENCE4res();
#endif


struct ssa_digest_input4 {
	SEQUENCE4args sdi_seqargs;
};
typedef struct ssa_digest_input4 ssa_digest_input4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ssa_digest_input4(...);
}
#else
bool_t xdr_ssa_digest_input4();
#endif


struct SET_SSV4args {
	struct {
		u_int ssa_ssv_len;
		char *ssa_ssv_val;
	} ssa_ssv;
	struct {
		u_int ssa_digest_len;
		char *ssa_digest_val;
	} ssa_digest;
};
typedef struct SET_SSV4args SET_SSV4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SET_SSV4args(...);
}
#else
bool_t xdr_SET_SSV4args();
#endif


struct ssr_digest_input4 {
	SEQUENCE4res sdi_seqres;
};
typedef struct ssr_digest_input4 ssr_digest_input4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_ssr_digest_input4(...);
}
#else
bool_t xdr_ssr_digest_input4();
#endif


struct SET_SSV4resok {
	struct {
		u_int ssr_digest_len;
		char *ssr_digest_val;
	} ssr_digest;
};
typedef struct SET_SSV4resok SET_SSV4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SET_SSV4resok(...);
}
#else
bool_t xdr_SET_SSV4resok();
#endif


struct SET_SSV4res {
	nfsstat4 ssr_status;
	union {
		SET_SSV4resok ssr_resok4;
	} SET_SSV4res_u;
};
typedef struct SET_SSV4res SET_SSV4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_SET_SSV4res(...);
}
#else
bool_t xdr_SET_SSV4res();
#endif


struct TEST_STATEID4args {
	struct {
		u_int ts_stateids_len;
		stateid4 *ts_stateids_val;
	} ts_stateids;
};
typedef struct TEST_STATEID4args TEST_STATEID4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_TEST_STATEID4args(...);
}
#else
bool_t xdr_TEST_STATEID4args();
#endif


struct TEST_STATEID4resok {
	struct {
		u_int tsr_status_codes_len;
		nfsstat4 *tsr_status_codes_val;
	} tsr_status_codes;
};
typedef struct TEST_STATEID4resok TEST_STATEID4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_TEST_STATEID4resok(...);
}
#else
bool_t xdr_TEST_STATEID4resok();
#endif


struct TEST_STATEID4res {
	nfsstat4 tsr_status;
	union {
		TEST_STATEID4resok tsr_resok4;
	} TEST_STATEID4res_u;
};
typedef struct TEST_STATEID4res TEST_STATEID4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_TEST_STATEID4res(...);
}
#else
bool_t xdr_TEST_STATEID4res();
#endif


struct deleg_claim4 {
	open_claim_type4 dc_claim;
	union {
		open_delegation_type4 dc_delegate_type;
	} deleg_claim4_u;
};
typedef struct deleg_claim4 deleg_claim4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_deleg_claim4(...);
}
#else
bool_t xdr_deleg_claim4();
#endif


struct WANT_DELEGATION4args {
	uint32_t wda_want;
	deleg_claim4 wda_claim;
};
typedef struct WANT_DELEGATION4args WANT_DELEGATION4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_WANT_DELEGATION4args(...);
}
#else
bool_t xdr_WANT_DELEGATION4args();
#endif


struct WANT_DELEGATION4res {
	nfsstat4 wdr_status;
	union {
		open_delegation4 wdr_resok4;
	} WANT_DELEGATION4res_u;
};
typedef struct WANT_DELEGATION4res WANT_DELEGATION4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_WANT_DELEGATION4res(...);
}
#else
bool_t xdr_WANT_DELEGATION4res();
#endif


struct DESTROY_CLIENTID4args {
	clientid4 dca_clientid;
};
typedef struct DESTROY_CLIENTID4args DESTROY_CLIENTID4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_DESTROY_CLIENTID4args(...);
}
#else
bool_t xdr_DESTROY_CLIENTID4args();
#endif


struct DESTROY_CLIENTID4res {
	nfsstat4 dcr_status;
};
typedef struct DESTROY_CLIENTID4res DESTROY_CLIENTID4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_DESTROY_CLIENTID4res(...);
}
#else
bool_t xdr_DESTROY_CLIENTID4res();
#endif


struct RECLAIM_COMPLETE4args {
	bool_t rca_one_fs;
};
typedef struct RECLAIM_COMPLETE4args RECLAIM_COMPLETE4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RECLAIM_COMPLETE4args(...);
}
#else
bool_t xdr_RECLAIM_COMPLETE4args();
#endif


struct RECLAIM_COMPLETE4res {
	nfsstat4 rcr_status;
};
typedef struct RECLAIM_COMPLETE4res RECLAIM_COMPLETE4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_RECLAIM_COMPLETE4res(...);
}
#else
bool_t xdr_RECLAIM_COMPLETE4res();
#endif


/* new operations for NFSv4.1 */


enum nfs_opnum4 {
	OP_ACCESS = 3,
	OP_CLOSE = 4,
	OP_COMMIT = 5,
	OP_CREATE = 6,
	OP_DELEGPURGE = 7,
	OP_DELEGRETURN = 8,
	OP_GETATTR = 9,
	OP_GETFH = 10,
	OP_LINK = 11,
	OP_LOCK = 12,
	OP_LOCKT = 13,
	OP_LOCKU = 14,
	OP_LOOKUP = 15,
	OP_LOOKUPP = 16,
	OP_NVERIFY = 17,
	OP_OPEN = 18,
	OP_OPENATTR = 19,
	OP_OPEN_CONFIRM = 20,
	OP_OPEN_DOWNGRADE = 21,
	OP_PUTFH = 22,
	OP_PUTPUBFH = 23,
	OP_PUTROOTFH = 24,
	OP_READ = 25,
	OP_READDIR = 26,
	OP_READLINK = 27,
	OP_REMOVE = 28,
	OP_RENAME = 29,
	OP_RENEW = 30,
	OP_RESTOREFH = 31,
	OP_SAVEFH = 32,
	OP_SECINFO = 33,
	OP_SETATTR = 34,
	OP_SETCLIENTID = 35,
	OP_SETCLIENTID_CONFIRM = 36,
	OP_VERIFY = 37,
	OP_WRITE = 38,
	OP_RELEASE_LOCKOWNER = 39,
	OP_BACKCHANNEL_CTL = 40,
	OP_BIND_CONN_TO_SESSION = 41,
	OP_EXCHANGE_ID = 42,
	OP_CREATE_SESSION = 43,
	OP_DESTROY_SESSION = 44,
	OP_FREE_STATEID = 45,
	OP_GET_DIR_DELEGATION = 46,
	OP_GETDEVICEINFO = 47,
	OP_GETDEVICELIST = 48,
	OP_LAYOUTCOMMIT = 49,
	OP_LAYOUTGET = 50,
	OP_LAYOUTRETURN = 51,
	OP_SECINFO_NO_NAME = 52,
	OP_SEQUENCE = 53,
	OP_SET_SSV = 54,
	OP_TEST_STATEID = 55,
	OP_WANT_DELEGATION = 56,
	OP_DESTROY_CLIENTID = 57,
	OP_RECLAIM_COMPLETE = 58,
	OP_ILLEGAL = 10044,
};
typedef enum nfs_opnum4 nfs_opnum4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfs_opnum4(...);
}
#else
bool_t xdr_nfs_opnum4();
#endif


struct nfs_argop4 {
	nfs_opnum4 argop;
	union {
		ACCESS4args opaccess;
		CLOSE4args opclose;
		COMMIT4args opcommit;
		CREATE4args opcreate;
		DELEGPURGE4args opdelegpurge;
		DELEGRETURN4args opdelegreturn;
		GETATTR4args opgetattr;
		LINK4args oplink;
		LOCK4args oplock;
		LOCKT4args oplockt;
		LOCKU4args oplocku;
		LOOKUP4args oplookup;
		NVERIFY4args opnverify;
		OPEN4args opopen;
		OPENATTR4args opopenattr;
		OPEN_CONFIRM4args opopen_confirm;
		OPEN_DOWNGRADE4args opopen_downgrade;
		PUTFH4args opputfh;
		READ4args opread;
		READDIR4args opreaddir;
		REMOVE4args opremove;
		RENAME4args oprename;
		RENEW4args oprenew;
		SECINFO4args opsecinfo;
		SETATTR4args opsetattr;
		SETCLIENTID4args opsetclientid;
		SETCLIENTID_CONFIRM4args opsetclientid_confirm;
		VERIFY4args opverify;
		WRITE4args opwrite;
		RELEASE_LOCKOWNER4args oprelease_lockowner;
		BACKCHANNEL_CTL4args opbackchannel_ctl;
		BIND_CONN_TO_SESSION4args opbind_conn_to_session;
		EXCHANGE_ID4args opexchange_id;
		CREATE_SESSION4args opcreate_session;
		DESTROY_SESSION4args opdestroy_session;
		FREE_STATEID4args opfree_stateid;
		GET_DIR_DELEGATION4args opget_dir_delegation;
		GETDEVICEINFO4args opgetdeviceinfo;
		GETDEVICELIST4args opgetdevicelist;
		LAYOUTCOMMIT4args oplayoutcommit;
		LAYOUTGET4args oplayoutget;
		LAYOUTRETURN4args oplayoutreturn;
		SECINFO_NO_NAME4args opsecinfo_no_name;
		SEQUENCE4args opsequence;
		SET_SSV4args opset_ssv;
		TEST_STATEID4args optest_stateid;
		WANT_DELEGATION4args opwant_delegation;
		DESTROY_CLIENTID4args opdestroy_clientid;
		RECLAIM_COMPLETE4args opreclaim_complete;
	} nfs_argop4_u;
};
typedef struct nfs_argop4 nfs_argop4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfs_argop4(...);
}
#else
bool_t xdr_nfs_argop4();
#endif


struct nfs_resop4 {
	nfs_opnum4 resop;
	union {
		ACCESS4res opaccess;
		CLOSE4res opclose;
		COMMIT4res opcommit;
		CREATE4res opcreate;
		DELEGPURGE4res opdelegpurge;
		DELEGRETURN4res opdelegreturn;
		GETATTR4res opgetattr;
		GETFH4res opgetfh;
		LINK4res oplink;
		LOCK4res oplock;
		LOCKT4res oplockt;
		LOCKU4res oplocku;
		LOOKUP4res oplookup;
		LOOKUPP4res oplookupp;
		NVERIFY4res opnverify;
		OPEN4res opopen;
		OPENATTR4res opopenattr;
		OPEN_CONFIRM4res opopen_confirm;
		OPEN_DOWNGRADE4res opopen_downgrade;
		PUTFH4res opputfh;
		PUTPUBFH4res opputpubfh;
		PUTROOTFH4res opputrootfh;
		READ4res opread;
		READDIR4res opreaddir;
		READLINK4res opreadlink;
		REMOVE4res opremove;
		RENAME4res oprename;
		RENEW4res oprenew;
		RESTOREFH4res oprestorefh;
		SAVEFH4res opsavefh;
		SECINFO4res opsecinfo;
		SETATTR4res opsetattr;
		SETCLIENTID4res opsetclientid;
		SETCLIENTID_CONFIRM4res opsetclientid_confirm;
		VERIFY4res opverify;
		WRITE4res opwrite;
		RELEASE_LOCKOWNER4res oprelease_lockowner;
		BACKCHANNEL_CTL4res opbackchannel_ctl;
		BIND_CONN_TO_SESSION4res opbind_conn_to_session;
		EXCHANGE_ID4res opexchange_id;
		CREATE_SESSION4res opcreate_session;
		DESTROY_SESSION4res opdestroy_session;
		FREE_STATEID4res opfree_stateid;
		GET_DIR_DELEGATION4res opget_dir_delegation;
		GETDEVICEINFO4res opgetdeviceinfo;
		GETDEVICELIST4res opgetdevicelist;
		LAYOUTCOMMIT4res oplayoutcommit;
		LAYOUTGET4res oplayoutget;
		LAYOUTRETURN4res oplayoutreturn;
		SECINFO_NO_NAME4res opsecinfo_no_name;
		SEQUENCE4res opsequence;
		SET_SSV4res opset_ssv;
		TEST_STATEID4res optest_stateid;
		WANT_DELEGATION4res opwant_delegation;
		DESTROY_CLIENTID4res opdestroy_clientid;
		RECLAIM_COMPLETE4res opreclaim_complete;
		ILLEGAL4res opillegal;
	} nfs_resop4_u;
};
typedef struct nfs_resop4 nfs_resop4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfs_resop4(...);
}
#else
bool_t xdr_nfs_resop4();
#endif


struct COMPOUND4args {
	utf8str_cs tag;
	uint32_t minorversion;
	struct {
		u_int argarray_len;
		nfs_argop4 *argarray_val;
	} argarray;
};
typedef struct COMPOUND4args COMPOUND4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_COMPOUND4args(...);
}
#else
bool_t xdr_COMPOUND4args();
#endif


struct COMPOUND4res {
	nfsstat4 status;
	utf8str_cs tag;
	struct {
		u_int resarray_len;
		nfs_resop4 *resarray_val;
	} resarray;
};
typedef struct COMPOUND4res COMPOUND4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_COMPOUND4res(...);
}
#else
bool_t xdr_COMPOUND4res();
#endif


#define NFS4_PROGRAM ((u_long)100003)
#define NFS_V4 ((u_long)4)
#define NFSPROC4_NULL ((u_long)0)
#ifdef __cplusplus
extern "C" {
extern void *nfsproc4_null_4(...);
}
#else
extern void *nfsproc4_null_4();
#endif /* __cplusplus */
#define NFSPROC4_COMPOUND ((u_long)1)
#ifdef __cplusplus
extern "C" {
extern COMPOUND4res *nfsproc4_compound_4(...);
}
#else
extern COMPOUND4res *nfsproc4_compound_4();
#endif /* __cplusplus */


struct CB_GETATTR4args {
	nfs_fh4 fh;
	bitmap4 attr_request;
};
typedef struct CB_GETATTR4args CB_GETATTR4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_GETATTR4args(...);
}
#else
bool_t xdr_CB_GETATTR4args();
#endif


struct CB_GETATTR4resok {
	fattr4 obj_attributes;
};
typedef struct CB_GETATTR4resok CB_GETATTR4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_GETATTR4resok(...);
}
#else
bool_t xdr_CB_GETATTR4resok();
#endif


struct CB_GETATTR4res {
	nfsstat4 status;
	union {
		CB_GETATTR4resok resok4;
	} CB_GETATTR4res_u;
};
typedef struct CB_GETATTR4res CB_GETATTR4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_GETATTR4res(...);
}
#else
bool_t xdr_CB_GETATTR4res();
#endif


struct CB_RECALL4args {
	stateid4 stateid;
	bool_t truncate;
	nfs_fh4 fh;
};
typedef struct CB_RECALL4args CB_RECALL4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_RECALL4args(...);
}
#else
bool_t xdr_CB_RECALL4args();
#endif


struct CB_RECALL4res {
	nfsstat4 status;
};
typedef struct CB_RECALL4res CB_RECALL4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_RECALL4res(...);
}
#else
bool_t xdr_CB_RECALL4res();
#endif


struct CB_ILLEGAL4res {
	nfsstat4 status;
};
typedef struct CB_ILLEGAL4res CB_ILLEGAL4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_ILLEGAL4res(...);
}
#else
bool_t xdr_CB_ILLEGAL4res();
#endif


enum layoutrecall_type4 {
	LAYOUTRECALL4_FILE = LAYOUT4_RET_REC_FILE,
	LAYOUTRECALL4_FSID = LAYOUT4_RET_REC_FSID,
	LAYOUTRECALL4_ALL = LAYOUT4_RET_REC_ALL,
};
typedef enum layoutrecall_type4 layoutrecall_type4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_layoutrecall_type4(...);
}
#else
bool_t xdr_layoutrecall_type4();
#endif


struct layoutrecall_file4 {
	nfs_fh4 lor_fh;
	offset4 lor_offset;
	length4 lor_length;
	stateid4 lor_stateid;
};
typedef struct layoutrecall_file4 layoutrecall_file4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_layoutrecall_file4(...);
}
#else
bool_t xdr_layoutrecall_file4();
#endif


struct layoutrecall4 {
	layoutrecall_type4 lor_recalltype;
	union {
		layoutrecall_file4 lor_layout;
		fsid4 lor_fsid;
	} layoutrecall4_u;
};
typedef struct layoutrecall4 layoutrecall4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_layoutrecall4(...);
}
#else
bool_t xdr_layoutrecall4();
#endif


struct CB_LAYOUTRECALL4args {
	layouttype4 clora_type;
	layoutiomode4 clora_iomode;
	bool_t clora_changed;
	layoutrecall4 clora_recall;
};
typedef struct CB_LAYOUTRECALL4args CB_LAYOUTRECALL4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_LAYOUTRECALL4args(...);
}
#else
bool_t xdr_CB_LAYOUTRECALL4args();
#endif


struct CB_LAYOUTRECALL4res {
	nfsstat4 clorr_status;
};
typedef struct CB_LAYOUTRECALL4res CB_LAYOUTRECALL4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_LAYOUTRECALL4res(...);
}
#else
bool_t xdr_CB_LAYOUTRECALL4res();
#endif


enum notify_type4 {
	NOTIFY4_CHANGE_CHILD_ATTRS = 0,
	NOTIFY4_CHANGE_DIR_ATTRS = 1,
	NOTIFY4_REMOVE_ENTRY = 2,
	NOTIFY4_ADD_ENTRY = 3,
	NOTIFY4_RENAME_ENTRY = 4,
	NOTIFY4_CHANGE_COOKIE_VERIFIER = 5,
};
typedef enum notify_type4 notify_type4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_notify_type4(...);
}
#else
bool_t xdr_notify_type4();
#endif


struct notify_entry4 {
	component4 ne_file;
	fattr4 ne_attrs;
};
typedef struct notify_entry4 notify_entry4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_notify_entry4(...);
}
#else
bool_t xdr_notify_entry4();
#endif


struct prev_entry4 {
	notify_entry4 pe_prev_entry;
	nfs_cookie4 pe_prev_entry_cookie;
};
typedef struct prev_entry4 prev_entry4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_prev_entry4(...);
}
#else
bool_t xdr_prev_entry4();
#endif


struct notify_remove4 {
	notify_entry4 nrm_old_entry;
	nfs_cookie4 nrm_old_entry_cookie;
};
typedef struct notify_remove4 notify_remove4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_notify_remove4(...);
}
#else
bool_t xdr_notify_remove4();
#endif


struct notify_add4 {
	struct {
		u_int nad_old_entry_len;
		notify_remove4 *nad_old_entry_val;
	} nad_old_entry;
	notify_entry4 nad_new_entry;
	struct {
		u_int nad_new_entry_cookie_len;
		nfs_cookie4 *nad_new_entry_cookie_val;
	} nad_new_entry_cookie;
	struct {
		u_int nad_prev_entry_len;
		prev_entry4 *nad_prev_entry_val;
	} nad_prev_entry;
	bool_t nad_last_entry;
};
typedef struct notify_add4 notify_add4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_notify_add4(...);
}
#else
bool_t xdr_notify_add4();
#endif


struct notify_attr4 {
	notify_entry4 na_changed_entry;
};
typedef struct notify_attr4 notify_attr4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_notify_attr4(...);
}
#else
bool_t xdr_notify_attr4();
#endif


struct notify_rename4 {
	notify_remove4 nrn_old_entry;
	notify_add4 nrn_new_entry;
};
typedef struct notify_rename4 notify_rename4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_notify_rename4(...);
}
#else
bool_t xdr_notify_rename4();
#endif


struct notify_verifier4 {
	verifier4 nv_old_cookieverf;
	verifier4 nv_new_cookieverf;
};
typedef struct notify_verifier4 notify_verifier4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_notify_verifier4(...);
}
#else
bool_t xdr_notify_verifier4();
#endif


typedef struct {
	u_int notifylist4_len;
	char *notifylist4_val;
} notifylist4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_notifylist4(...);
}
#else
bool_t xdr_notifylist4();
#endif


struct notify4 {
	bitmap4 notify_mask;
	notifylist4 notify_vals;
};
typedef struct notify4 notify4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_notify4(...);
}
#else
bool_t xdr_notify4();
#endif


struct CB_NOTIFY4args {
	stateid4 cna_stateid;
	nfs_fh4 cna_fh;
	struct {
		u_int cna_changes_len;
		notify4 *cna_changes_val;
	} cna_changes;
};
typedef struct CB_NOTIFY4args CB_NOTIFY4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_NOTIFY4args(...);
}
#else
bool_t xdr_CB_NOTIFY4args();
#endif


struct CB_NOTIFY4res {
	nfsstat4 cnr_status;
};
typedef struct CB_NOTIFY4res CB_NOTIFY4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_NOTIFY4res(...);
}
#else
bool_t xdr_CB_NOTIFY4res();
#endif


struct CB_PUSH_DELEG4args {
	nfs_fh4 cpda_fh;
	open_delegation4 cpda_delegation;
};
typedef struct CB_PUSH_DELEG4args CB_PUSH_DELEG4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_PUSH_DELEG4args(...);
}
#else
bool_t xdr_CB_PUSH_DELEG4args();
#endif


struct CB_PUSH_DELEG4res {
	nfsstat4 cpdr_status;
};
typedef struct CB_PUSH_DELEG4res CB_PUSH_DELEG4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_PUSH_DELEG4res(...);
}
#else
bool_t xdr_CB_PUSH_DELEG4res();
#endif

#define RCA4_TYPE_MASK_RDATA_DLG 0
#define RCA4_TYPE_MASK_WDATA_DLG 1
#define RCA4_TYPE_MASK_DIR_DLG 2
#define RCA4_TYPE_MASK_FILE_LAYOUT 3
#define RCA4_TYPE_MASK_BLK_LAYOUT 4
#define RCA4_TYPE_MASK_OBJ_LAYOUT_MIN 8
#define RCA4_TYPE_MASK_OBJ_LAYOUT_MAX 9
#define RCA4_TYPE_MASK_OTHER_LAYOUT_MIN 12
#define RCA4_TYPE_MASK_OTHER_LAYOUT_MAX 15

struct CB_RECALL_ANY4args {
	uint32_t craa_objects_to_keep;
	bitmap4 craa_type_mask;
};
typedef struct CB_RECALL_ANY4args CB_RECALL_ANY4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_RECALL_ANY4args(...);
}
#else
bool_t xdr_CB_RECALL_ANY4args();
#endif


struct CB_RECALL_ANY4res {
	nfsstat4 crar_status;
};
typedef struct CB_RECALL_ANY4res CB_RECALL_ANY4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_RECALL_ANY4res(...);
}
#else
bool_t xdr_CB_RECALL_ANY4res();
#endif


typedef CB_RECALL_ANY4args CB_RECALLABLE_OBJ_AVAIL4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_RECALLABLE_OBJ_AVAIL4args(...);
}
#else
bool_t xdr_CB_RECALLABLE_OBJ_AVAIL4args();
#endif


struct CB_RECALLABLE_OBJ_AVAIL4res {
	nfsstat4 croa_status;
};
typedef struct CB_RECALLABLE_OBJ_AVAIL4res CB_RECALLABLE_OBJ_AVAIL4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_RECALLABLE_OBJ_AVAIL4res(...);
}
#else
bool_t xdr_CB_RECALLABLE_OBJ_AVAIL4res();
#endif


struct CB_RECALL_SLOT4args {
	slotid4 rsa_target_highest_slotid;
};
typedef struct CB_RECALL_SLOT4args CB_RECALL_SLOT4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_RECALL_SLOT4args(...);
}
#else
bool_t xdr_CB_RECALL_SLOT4args();
#endif


struct CB_RECALL_SLOT4res {
	nfsstat4 rsr_status;
};
typedef struct CB_RECALL_SLOT4res CB_RECALL_SLOT4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_RECALL_SLOT4res(...);
}
#else
bool_t xdr_CB_RECALL_SLOT4res();
#endif


struct referring_call4 {
	sequenceid4 rc_sequenceid;
	slotid4 rc_slotid;
};
typedef struct referring_call4 referring_call4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_referring_call4(...);
}
#else
bool_t xdr_referring_call4();
#endif


struct referring_call_list4 {
	sessionid4 rcl_sessionid;
	struct {
		u_int rcl_referring_calls_len;
		referring_call4 *rcl_referring_calls_val;
	} rcl_referring_calls;
};
typedef struct referring_call_list4 referring_call_list4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_referring_call_list4(...);
}
#else
bool_t xdr_referring_call_list4();
#endif


struct CB_SEQUENCE4args {
	sessionid4 csa_sessionid;
	sequenceid4 csa_sequenceid;
	slotid4 csa_slotid;
	slotid4 csa_highest_slotid;
	bool_t csa_cachethis;
	struct {
		u_int csa_referring_call_lists_len;
		referring_call_list4 *csa_referring_call_lists_val;
	} csa_referring_call_lists;
};
typedef struct CB_SEQUENCE4args CB_SEQUENCE4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_SEQUENCE4args(...);
}
#else
bool_t xdr_CB_SEQUENCE4args();
#endif


struct CB_SEQUENCE4resok {
	sessionid4 csr_sessionid;
	sequenceid4 csr_sequenceid;
	slotid4 csr_slotid;
	slotid4 csr_highest_slotid;
	slotid4 csr_target_highest_slotid;
};
typedef struct CB_SEQUENCE4resok CB_SEQUENCE4resok;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_SEQUENCE4resok(...);
}
#else
bool_t xdr_CB_SEQUENCE4resok();
#endif


struct CB_SEQUENCE4res {
	nfsstat4 csr_status;
	union {
		CB_SEQUENCE4resok csr_resok4;
	} CB_SEQUENCE4res_u;
};
typedef struct CB_SEQUENCE4res CB_SEQUENCE4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_SEQUENCE4res(...);
}
#else
bool_t xdr_CB_SEQUENCE4res();
#endif


struct CB_WANTS_CANCELLED4args {
	bool_t cwca_contended_wants_cancelled;
	bool_t cwca_resourced_wants_cancelled;
};
typedef struct CB_WANTS_CANCELLED4args CB_WANTS_CANCELLED4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_WANTS_CANCELLED4args(...);
}
#else
bool_t xdr_CB_WANTS_CANCELLED4args();
#endif


struct CB_WANTS_CANCELLED4res {
	nfsstat4 cwcr_status;
};
typedef struct CB_WANTS_CANCELLED4res CB_WANTS_CANCELLED4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_WANTS_CANCELLED4res(...);
}
#else
bool_t xdr_CB_WANTS_CANCELLED4res();
#endif


struct CB_NOTIFY_LOCK4args {
	nfs_fh4 cnla_fh;
	lock_owner4 cnla_lock_owner;
};
typedef struct CB_NOTIFY_LOCK4args CB_NOTIFY_LOCK4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_NOTIFY_LOCK4args(...);
}
#else
bool_t xdr_CB_NOTIFY_LOCK4args();
#endif


struct CB_NOTIFY_LOCK4res {
	nfsstat4 cnlr_status;
};
typedef struct CB_NOTIFY_LOCK4res CB_NOTIFY_LOCK4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_NOTIFY_LOCK4res(...);
}
#else
bool_t xdr_CB_NOTIFY_LOCK4res();
#endif


enum notify_deviceid_type4 {
	NOTIFY_DEVICEID4_CHANGE = 1,
	NOTIFY_DEVICEID4_DELETE = 2,
};
typedef enum notify_deviceid_type4 notify_deviceid_type4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_notify_deviceid_type4(...);
}
#else
bool_t xdr_notify_deviceid_type4();
#endif


struct notify_deviceid_delete4 {
	layouttype4 ndd_layouttype;
	deviceid4 ndd_deviceid;
};
typedef struct notify_deviceid_delete4 notify_deviceid_delete4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_notify_deviceid_delete4(...);
}
#else
bool_t xdr_notify_deviceid_delete4();
#endif


struct notify_deviceid_change4 {
	layouttype4 ndc_layouttype;
	deviceid4 ndc_deviceid;
	bool_t ndc_immediate;
};
typedef struct notify_deviceid_change4 notify_deviceid_change4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_notify_deviceid_change4(...);
}
#else
bool_t xdr_notify_deviceid_change4();
#endif


struct CB_NOTIFY_DEVICEID4args {
	struct {
		u_int cnda_changes_len;
		notify4 *cnda_changes_val;
	} cnda_changes;
};
typedef struct CB_NOTIFY_DEVICEID4args CB_NOTIFY_DEVICEID4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_NOTIFY_DEVICEID4args(...);
}
#else
bool_t xdr_CB_NOTIFY_DEVICEID4args();
#endif


struct CB_NOTIFY_DEVICEID4res {
	nfsstat4 cndr_status;
};
typedef struct CB_NOTIFY_DEVICEID4res CB_NOTIFY_DEVICEID4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_NOTIFY_DEVICEID4res(...);
}
#else
bool_t xdr_CB_NOTIFY_DEVICEID4res();
#endif


/* Callback operations new to NFSv4.1 */

enum nfs_cb_opnum4 {
	OP_CB_GETATTR = 3,
	OP_CB_RECALL = 4,
	OP_CB_LAYOUTRECALL = 5,
	OP_CB_NOTIFY = 6,
	OP_CB_PUSH_DELEG = 7,
	OP_CB_RECALL_ANY = 8,
	OP_CB_RECALLABLE_OBJ_AVAIL = 9,
	OP_CB_RECALL_SLOT = 10,
	OP_CB_SEQUENCE = 11,
	OP_CB_WANTS_CANCELLED = 12,
	OP_CB_NOTIFY_LOCK = 13,
	OP_CB_NOTIFY_DEVICEID = 14,
	OP_CB_ILLEGAL = 10044,
};
typedef enum nfs_cb_opnum4 nfs_cb_opnum4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfs_cb_opnum4(...);
}
#else
bool_t xdr_nfs_cb_opnum4();
#endif


struct nfs_cb_argop4 {
	u_int argop;
	union {
		CB_GETATTR4args opcbgetattr;
		CB_RECALL4args opcbrecall;
		CB_LAYOUTRECALL4args opcblayoutrecall;
		CB_NOTIFY4args opcbnotify;
		CB_PUSH_DELEG4args opcbpush_deleg;
		CB_RECALL_ANY4args opcbrecall_any;
		CB_RECALLABLE_OBJ_AVAIL4args opcbrecallable_obj_avail;
		CB_RECALL_SLOT4args opcbrecall_slot;
		CB_SEQUENCE4args opcbsequence;
		CB_WANTS_CANCELLED4args opcbwants_cancelled;
		CB_NOTIFY_LOCK4args opcbnotify_lock;
		CB_NOTIFY_DEVICEID4args opcbnotify_deviceid;
	} nfs_cb_argop4_u;
};
typedef struct nfs_cb_argop4 nfs_cb_argop4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfs_cb_argop4(...);
}
#else
bool_t xdr_nfs_cb_argop4();
#endif


struct nfs_cb_resop4 {
	u_int resop;
	union {
		CB_GETATTR4res opcbgetattr;
		CB_RECALL4res opcbrecall;
		CB_LAYOUTRECALL4res opcblayoutrecall;
		CB_NOTIFY4res opcbnotify;
		CB_PUSH_DELEG4res opcbpush_deleg;
		CB_RECALL_ANY4res opcbrecall_any;
		CB_RECALLABLE_OBJ_AVAIL4res opcbrecallable_obj_avail;
		CB_RECALL_SLOT4res opcbrecall_slot;
		CB_SEQUENCE4res opcbsequence;
		CB_WANTS_CANCELLED4res opcbwants_cancelled;
		CB_NOTIFY_LOCK4res opcbnotify_lock;
		CB_NOTIFY_DEVICEID4res opcbnotify_deviceid;
		CB_ILLEGAL4res opcbillegal;
	} nfs_cb_resop4_u;
};
typedef struct nfs_cb_resop4 nfs_cb_resop4;
#ifdef __cplusplus
extern "C" {
bool_t xdr_nfs_cb_resop4(...);
}
#else
bool_t xdr_nfs_cb_resop4();
#endif


struct CB_COMPOUND4args {
	utf8str_cs tag;
	uint32_t minorversion;
	uint32_t callback_ident;
	struct {
		u_int argarray_len;
		nfs_cb_argop4 *argarray_val;
	} argarray;
};
typedef struct CB_COMPOUND4args CB_COMPOUND4args;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_COMPOUND4args(...);
}
#else
bool_t xdr_CB_COMPOUND4args();
#endif


struct CB_COMPOUND4res {
	nfsstat4 status;
	utf8str_cs tag;
	struct {
		u_int resarray_len;
		nfs_cb_resop4 *resarray_val;
	} resarray;
};
typedef struct CB_COMPOUND4res CB_COMPOUND4res;
#ifdef __cplusplus
extern "C" {
bool_t xdr_CB_COMPOUND4res(...);
}
#else
bool_t xdr_CB_COMPOUND4res();
#endif


#define NFS4_CALLBACK ((u_long)0x40000000)
#define NFS_CB ((u_long)1)
#define CB_NULL ((u_long)0)
#ifdef __cplusplus
extern "C" {
extern void *cb_null_1(...);
}
#else
extern void *cb_null_1();
#endif /* __cplusplus */
#define CB_COMPOUND ((u_long)1)
#ifdef __cplusplus
extern "C" {
extern CB_COMPOUND4res *cb_compound_1(...);
}
#else
extern CB_COMPOUND4res *cb_compound_1();
#endif /* __cplusplus */

enum authparam
{
	RPCSEC_GSS = 6
};

#endif
