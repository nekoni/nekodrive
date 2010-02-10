/*
 * NekoDrive
 * 2010 by Mirko Gatto
 * mirko.gatto@gmail.com
 *
 * NFS V2 Wrapper
 *
 * Users may use, copy or modify this library 
 * according GNU General Public License v3 (http://www.gnu.org/licenses/gpl.html)
 */

#include "stdafx.h"
#include "NFSv2.h"
#include "NFSv2MountProtocol.h"
#include "NFSv2Protocol.h"
#include "rpc/Pmap_cln.h"
#include <stdio.h>
#include <vector>
#include <string>
#include <time.h>

CNFSv2::CNFSv2()
{
	clntMountV2 = NULL;
	clntV2 = NULL;
	sSocket = RPC_ANYSOCK;
	strCurrentDevice = "";
	uiServer = 0;
	timeOut.tv_sec = 60;
	timeOut.tv_usec = 0;
	sSrvAddr.sin_family = AF_INET;
	memset(nfsCurrentFile, 0, sizeof(FHSIZE));
	memset(nfsCurrentDirectory, 0, sizeof(FHSIZE));
	authType = AUTH_UNIX;
	uid = -2;
	gid = -2;
}

int CNFSv2::Connect(unsigned int ServerAddress, int UserId, int GroupId)
{
	int Ret = NFS_ERROR;
	WORD wVersionRequested;
	WSADATA wsaData;
	int iWSAErr;
	wVersionRequested = MAKEWORD( 2, 2 );
	iWSAErr = WSAStartup( wVersionRequested, &wsaData );
	if ( iWSAErr != 0 ) 
	{
		char  buffer[200];
		sprintf_s(buffer, 200, "WSAStartup %d", iWSAErr);
		strLastError = buffer;
	}
	else
	{
		u_short uPort;
		sSrvAddr.sin_addr.s_addr = ServerAddress;
		strServer.assign(inet_ntoa(sSrvAddr.sin_addr));
		if( (uPort = pmap_getport(&sSrvAddr, MOUNTPROG, MOUNTVERS, IPPROTO_UDP)) == 0)
			strLastError = clnt_spcreateerror((char*)strServer.c_str());
		else
		{
			sSrvAddr.sin_port = htons(uPort);
			if ((clntMountV2 = clntudp_create(&sSrvAddr, MOUNTPROG, MOUNTVERS, timeOut, (int*)&sSocket)) == NULL) 
  				 strLastError = clnt_spcreateerror((char*)strServer.c_str());
			else
			{
				if( (uPort = pmap_getport(&sSrvAddr, NFS_PROGRAM, NFS_VERSION, IPPROTO_UDP)) == 0 )
					strLastError = clnt_spcreateerror((char*)strServer.c_str());
				else
				{
					sSrvAddr.sin_port = htons(uPort);
					sSocket = RPC_ANYSOCK;
					if ((clntV2 = clntudp_create(&sSrvAddr, NFS_PROGRAM, NFS_VERSION, timeOut, (int*)&sSocket)) == NULL) 
  						 strLastError = clnt_spcreateerror((char*)strServer.c_str());
					else
					{
						uid = UserId;
						gid = GroupId;
						Ret = CreateAuthentication();
					}
				}
			}
		}
	}
	return Ret;
}

CNFSv2::~CNFSv2()
{
	if(clntMountV2 != NULL)
	{
		Disconnect();
	}
}

int CNFSv2::Disconnect()
{
	int Ret = NFS_ERROR;
	if(clntMountV2 != NULL)
	{	
		if(UnMountDevice() == NFS_SUCCESS)
		{
			if(clntMountV2 != NULL)
			{
				nfs_auth_destroy(clntMountV2->cl_auth);
				nfs_clnt_destroy(clntMountV2);
			}
			clntMountV2 = NULL;
			if(clntV2 != NULL)
			{
				nfs_auth_destroy(clntV2->cl_auth);
				nfs_clnt_destroy(clntV2);
			}
			clntV2 = NULL;
			Ret = NFS_SUCCESS;
		}
	}
	WSACleanup();
	return Ret;
}

int CNFSv2::UnMountDevice()
{
	int Ret = NFS_ERROR;
	if(clntMountV2 != NULL)
	{
		if(strCurrentDevice != "")
		{
			dirpath dpArgs;
			dpArgs = (dirpath) strCurrentDevice.c_str();
			if( mountproc_umnt_1(&dpArgs, clntMountV2) == NULL ) 
				strLastError = clnt_sperror(clntMountV2, "mountproc_umnt_1");
			else
			{
				strCurrentDevice = "";
				Ret = NFS_SUCCESS;
			}
		}
		else
			strLastError = "No device mouted";
	}
	return Ret;
}

int CNFSv2::MountDevice(char* pDevice)
{
	int Ret = NFS_ERROR;
	if(pDevice != NULL)
	{
		if(clntMountV2 != NULL)
		{
			dirpath dpArgs = NULL;
			fhstatus* pFhStatus = NULL;
			dpArgs = pDevice;

			if( (pFhStatus = mountproc_mnt_1(&dpArgs, clntMountV2)) == NULL ) 
				strLastError = clnt_sperror(clntMountV2, "mountproc_mnt_1");
			else
			{
				if (pFhStatus->status == (u_int) NFS_OK ) 
				{
					memcpy(nfsCurrentDirectory, pFhStatus->fhstatus_u.directory, FHSIZE);
					strCurrentDevice = pDevice;
					Ret = NFS_SUCCESS;
				} 
				else 
				{
					char  buffer[200];
					sprintf_s(buffer, 200, "mountproc_mnt_1 %d", pFhStatus->status);
					strLastError = buffer;
					printf(strLastError.c_str());
				}
				xdr_free((xdrproc_t)xdr_fhstatus,(char*) pFhStatus);
			}
		}
	}
	return Ret;
}

char** CNFSv2::GetExportedDevices(int* pnSize)
{
	char** Ret = NULL;
	v.Str.clear();
	if(clntMountV2 == NULL)
		strLastError = "Mount Client is NULL";
	else
	{
		exports *device;
		exports tmp;
		if( (device = mountproc_export_1(NULL, clntMountV2) ) == NULL )
			strLastError = clnt_sperror(clntMountV2, "mountproc_export_1");
		else
		{
			tmp = *device;
			while(tmp != NULL)
			{
				v.Str.push_back(tmp->filesys);
				tmp = tmp->next;
			}
			xdr_free((xdrproc_t)xdr_exports,(char*) device);
			int nSize = (int) v.Str.size();
			Ret = new char*[nSize];
			for( int i = 0; i < nSize; i++)
				Ret[i] = (char*) v.Str[i].c_str();
			*pnSize = nSize;
		}
	}
	return Ret;
}

char** CNFSv2::GetItemsList(int* pnSize)
{
	char** Ret = NULL;
	v.Str.clear();
	entry* pEntry = NULL;
	if(clntV2 != NULL)
	{
		readdirargs dpRdArgs;
        readdirres *pReadDirRes;
		dpRdArgs.cookie = 0;
		dpRdArgs.count = 8192;
		memset(dpRdArgs.dir, 0, FHSIZE);
		memcpy(dpRdArgs.dir, nfsCurrentDirectory, FHSIZE);
		while(true)
		{
			if( (pReadDirRes = nfsproc_readdir_2(&dpRdArgs, clntV2)) == NULL ) 
			{
				strLastError = clnt_sperror(clntV2, "nfsproc_readdir_2");
				break;
			}
			else
			{
				if(pReadDirRes->status == NFS_OK)
				{
					pEntry = pReadDirRes->readdirres_u.ok.entries;
					while(pEntry != NULL)
					{
						v.Str.push_back(pEntry->name);
						dpRdArgs.cookie = pEntry->cookie;
						pEntry = pEntry->nextentry;
					}
				}
				else
				{
					char  buffer[200];
					sprintf_s(buffer, 200, "nfsproc_readdir_2 %d", pReadDirRes->status);
					strLastError = buffer;
					break;
				}
				if(pReadDirRes->readdirres_u.ok.eof)
					break;
			}
		}
		if(v.Str.size() > 0)
		{
			xdr_free((xdrproc_t)xdr_readdirres,(char*) pReadDirRes);
			int nSize = (int) v.Str.size();
			Ret = new char*[nSize];
			for( int i = 0; i < nSize; i++)
				Ret[i] = (char*) v.Str[i].c_str();
			*pnSize = nSize;
		}
	}
	else
		strLastError = "V2 Client is NULL";
	return Ret;
}

void CNFSv2::ReleaseBuffers(void** pBuffers)
{
	if(pBuffers != NULL)
		delete[] pBuffers;
}

void* CNFSv2::GetItemAttributes(char* pName)
{
	void* Ret = NULL;
	if(clntV2 != NULL)
	{
		diropargs dpDrArgs;
		diropres *pDirOpRes;
		memcpy(dpDrArgs.dir, nfsCurrentDirectory, FHSIZE);
		dpDrArgs.name = pName;
		if( (pDirOpRes = nfsproc_lookup_2(&dpDrArgs, clntV2)) == NULL ) 
			strLastError = clnt_sperror(clntV2,"nfsproc_lookup_2");
		else
		{
			if (pDirOpRes->status == NFS_OK) 
			{
				NFSData* pNfsData = new NFSData;
				InitStructure(pNfsData);
				memcpy(pNfsData->Handle, pDirOpRes->diropres_u.ok.file, FHSIZE);
				pNfsData->DateTime = pDirOpRes->diropres_u.ok.attributes.ctime.seconds;
				pNfsData->Type = pDirOpRes->diropres_u.ok.attributes.type;
				pNfsData->Size = pDirOpRes->diropres_u.ok.attributes.size;
				pNfsData->Blocks = pDirOpRes->diropres_u.ok.attributes.blocks;
				pNfsData->BlockSize = pDirOpRes->diropres_u.ok.attributes.blocksize;
				Ret = pNfsData;	
			}
			else
			{
				char  buffer[200];
				sprintf_s(buffer, 200, "nfsproc_lookup_2 %d", pDirOpRes->status);
				strLastError = buffer;
			}
			xdr_free((xdrproc_t)xdr_diropres,(char*) pDirOpRes);
		}
	}
	else
		strLastError = "V2 Client is NULL";
	return Ret;
}

void CNFSv2::ReleaseBuffer(void* pBuffer)
{
	if(pBuffer != NULL)
		delete pBuffer;
}

void CNFSv2::InitStructure(NFSData* pNfsData)
{
	memset(pNfsData->Handle, 0, FHSIZE);
	pNfsData->DateTime = 0;
	pNfsData->Type = 0;
	pNfsData->Blocks = 0;
	pNfsData->DateTime = 0;
	pNfsData->BlockSize = 0;
}

int CNFSv2::ChangeCurrentDirectory(char *pName)
{
	int Ret = NFS_ERROR;
	if(pName != NULL)
	{
		NFSData* pNfsData = (NFSData*) GetItemAttributes(pName);
		if(pNfsData != NULL)
		{
			memcpy(nfsCurrentDirectory, pNfsData->Handle, FHSIZE);
			ReleaseBuffer(pNfsData);
			Ret = NFS_SUCCESS;
		}
	}
	else
		strLastError = "Name is NULL";
	return Ret;
}

int CNFSv2::CreateDirectory(char* pName)
{
	int Ret = NFS_ERROR;
	if(clntV2 != NULL)
	{
		createargs dpArgCreate;
        diropres *pDirOpRes;
		dpArgCreate.attributes.atime.seconds = -1;
		dpArgCreate.attributes.atime.useconds = -1;
		dpArgCreate.attributes.mtime.seconds = -1;
		dpArgCreate.attributes.mtime.useconds = -1;
		dpArgCreate.attributes.mode = MODE_DIR | 0777;
		dpArgCreate.attributes.gid = gid;
		dpArgCreate.attributes.uid = uid;
		memcpy(dpArgCreate.where.dir, nfsCurrentDirectory, FHSIZE);
        dpArgCreate.where.name = pName;
        if( (pDirOpRes = nfsproc_mkdir_2(&dpArgCreate, clntV2)) == NULL ) 
			strLastError = clnt_sperror(clntV2, "nfsproc_mkdir_2");
		else
		{
			if (pDirOpRes->status == NFS_OK) 
				Ret = NFS_SUCCESS;
			else 
			{
				char  buffer[200];
				sprintf_s(buffer, 200, "nfsproc_mkdir_2 %d", pDirOpRes->status);
				strLastError = buffer;
			}
			xdr_free((xdrproc_t)xdr_diropres,(char*) pDirOpRes);
		}
	}
	else
		strLastError = "V2 Client is NULL";
	return Ret;
}

int CNFSv2::DeleteDirectory(char* pName)
{
	int Ret = NFS_ERROR;
	if(clntV2 != NULL)
	{
		diropargs dpArgDelete;
        nfsstat *pNfsStat;
		memcpy(dpArgDelete.dir, nfsCurrentDirectory, FHSIZE);
        dpArgDelete.name = pName;
        if( (pNfsStat = nfsproc_rmdir_2(&dpArgDelete, clntV2)) == NULL ) 
			strLastError = clnt_sperror(clntV2, "nfsproc_rmdir_2");
		else
		{
			if (*pNfsStat == NFS_OK) 
				Ret = NFS_SUCCESS;
			else 
			{
				char  buffer[200];
				sprintf_s(buffer, 200, "nfsproc_rmdir_2 %d", pNfsStat);
				strLastError = buffer;
			}
			xdr_free((xdrproc_t)xdr_nfsstat,(char*) pNfsStat);
		}
	}
	else
		strLastError = "V2 Client is NULL";
	return Ret;
}

int CNFSv2::DeleteFile(char* pName)
{
	int Ret = NFS_ERROR;
	if(clntV2 != NULL)
	{
		diropargs dpArgDelete;
        nfsstat *pNfsStat;
		memcpy(dpArgDelete.dir, nfsCurrentDirectory, FHSIZE);
        dpArgDelete.name = pName;
        if( (pNfsStat = nfsproc_remove_2(&dpArgDelete, clntV2)) == NULL ) 
			strLastError = clnt_sperror(clntV2, "nfsproc_remove_2");
		else
		{
			if (*pNfsStat == NFS_OK) 
				Ret = NFS_SUCCESS;
			else 
			{
				char  buffer[200];
				sprintf_s(buffer, 200, "nfsproc_remove_2 %d", pNfsStat);
				strLastError = buffer;
			}
			xdr_free((xdrproc_t)xdr_nfsstat,(char*) pNfsStat);
		}
	}
	else
		strLastError = "V2 Client is NULL";
	return Ret;
}

int CNFSv2::CreateFile(char* pName)
{
	int Ret = NFS_ERROR;
	if(clntV2 != NULL)
	{
		createargs dpArgCreate;
        diropres *pDirOpRes;
		dpArgCreate.attributes.atime.seconds = -1;
		dpArgCreate.attributes.atime.useconds = -1;
		dpArgCreate.attributes.mtime.seconds = -1;
		dpArgCreate.attributes.mtime.useconds = -1;
		dpArgCreate.attributes.mode = MODE_REG | 0777;
		dpArgCreate.attributes.gid = gid;
		dpArgCreate.attributes.size = -1;
		dpArgCreate.attributes.uid = uid;
		memcpy(dpArgCreate.where.dir, nfsCurrentDirectory, FHSIZE);
        dpArgCreate.where.name = pName;
        if( (pDirOpRes = nfsproc_create_2(&dpArgCreate, clntV2)) == NULL ) 
			strLastError = clnt_sperror(clntV2, "nfsproc_create_2");
		else
		{
			if (pDirOpRes->status == NFS_OK) 
				Ret = NFS_SUCCESS;
			else 
			{
				char  buffer[200];
				sprintf_s(buffer, 200, "nfsproc_create_2 %d", pDirOpRes->status);
				strLastError = buffer;
			}
			xdr_free((xdrproc_t)xdr_diropres,(char*) pDirOpRes);
		}
	}
	else
		strLastError = "V2 Client is NULL";
	return Ret;
}

int CNFSv2::Open(char* pName)
{
	int Ret = NFS_ERROR;
	if(pName != NULL)
	{
		NFSData* pNfsData = (NFSData*) GetItemAttributes(pName);
		if(pNfsData != NULL)
		{
			memcpy(nfsCurrentFile, pNfsData->Handle, FHSIZE);
			ReleaseBuffer(pNfsData);
			Ret = NFS_SUCCESS;
		}
	}
	return Ret;
}

void CNFSv2::CloseFile()
{
	memset(nfsCurrentFile, 0, sizeof(FHSIZE));
}

int CNFSv2::CheckOpenHandle()
{
	int sum = 0;
	for(int x = 0; x < FHSIZE; x++)
		sum += nfsCurrentFile[x];
	return sum;
}

int CNFSv2::Read(u_int Offset, u_int Count, char* pBuffer, u_long* pSize)
{
	int Ret = NFS_ERROR;
	if(clntV2 != NULL)
	{
		if(CheckOpenHandle() == NULL)
			strLastError = "handle closed";
		else
		{
			readargs dpArgRead;
			readres *pReadRes;
			memcpy(dpArgRead.file, nfsCurrentFile, FHSIZE);
			dpArgRead.offset = Offset;
			dpArgRead.count = Count;
			if( (pReadRes = nfsproc_read_2(&dpArgRead, clntV2)) == NULL ) 
				strLastError = clnt_sperror(clntV2, "nfsproc_read_2");
			else
			{
				if (pReadRes->status == NFS_OK) 
				{
					*pSize = pReadRes->readres_u.ok.data.data_len;
					memcpy(pBuffer, pReadRes->readres_u.ok.data.data_val, pReadRes->readres_u.ok.data.data_len);
					Ret = NFS_SUCCESS;
				} 
				else 
				{
					char  buffer[200];
					sprintf_s(buffer, 200, "nfsproc_read_2 %d", pReadRes->status);
					strLastError = buffer;
				}
				xdr_free((xdrproc_t)xdr_readres,(char*) pReadRes);
			}
		}
	}
	else
		strLastError = "V2 Client is NULL";
	return Ret;
}

int CNFSv2::Write(u_int Offset, u_int Count, char* pBuffer, u_long* pSize)
{
	int Ret = NFS_ERROR;
	if(clntV2 != NULL)
	{
		if(CheckOpenHandle() == NULL)
			strLastError = "handle closed";
		else
		{
			writeargs dpArgWrite;
			attrstat *pAttrStat;
			memcpy(dpArgWrite.file, nfsCurrentFile, FHSIZE);
			dpArgWrite.offset = Offset;
			dpArgWrite.data.data_len = Count;
			dpArgWrite.data.data_val = pBuffer;
			if( (pAttrStat = nfsproc_write_2(&dpArgWrite, clntV2)) == NULL )
				strLastError = clnt_sperror(clntV2, "nfsproc_write_2");
			else
			{
				if (pAttrStat->status == NFS_OK) 
				{
					*pSize = pAttrStat->attrstat_u.attributes.size;
					Ret = NFS_SUCCESS;
				} 
				else 
				{
					char  buffer[200];
					sprintf_s(buffer, 200, "nfsproc_write_2 %d", pAttrStat->status);
					strLastError = buffer;
				}
				xdr_free((xdrproc_t)xdr_attrstat,(char*) pAttrStat);
			}
		}
	}
	else
		strLastError = "V2 Client is NULL";
	return Ret;
}

int CNFSv2::Rename(char* pOldName, char* pNewName)
{
	int Ret = NFS_ERROR;
	if(clntV2 != NULL)
	{
		renameargs dpArgRename;
        nfsstat *pNfsStat;
		memcpy(dpArgRename.from.dir, nfsCurrentDirectory, FHSIZE);
		memcpy(dpArgRename.to.dir, nfsCurrentDirectory, FHSIZE);
        dpArgRename.from.name = pOldName;
		dpArgRename.to.name = pNewName;
        if( (pNfsStat = nfsproc_rename_2(&dpArgRename, clntV2)) == NULL ) 
			strLastError = clnt_sperror(clntV2, "nfsproc_rename_2");
		else
		{
			if (*pNfsStat == NFS_OK) 
				Ret = NFS_SUCCESS;
			else 
			{
				char  buffer[200];
				sprintf_s(buffer, 200, "nfsproc_rename_2 %d", pNfsStat);
				strLastError = buffer;
			}
			xdr_free((xdrproc_t)xdr_nfsstat,(char*) pNfsStat);
		}
	}
	else
		strLastError = "Client is NULL";
	return Ret;
}

const char* CNFSv2::GetLastNfsError()
{
	return strLastError.c_str();
}

int CNFSv2::ChangeMode(char* pName, int Mode)
{
	int Ret = NFS_ERROR;
	if(clntV2 != NULL)
	{
		sattrargs dpArgSAttr;
		diropargs dpArgs;
		attrstat *pAttrStat;
		dpArgs.name = pName;
		memcpy(dpArgs.dir, nfsCurrentDirectory, FHSIZE);
		NFSData *pNfsData = (NFSData*)GetItemAttributes(pName);
		if(pNfsData != NULL)
		{
			memcpy(dpArgSAttr.file, pNfsData->Handle, FHSIZE);
			dpArgSAttr.attributes.atime.seconds = -1;
			dpArgSAttr.attributes.atime.useconds = -1;
			dpArgSAttr.attributes.gid = -1;
			dpArgSAttr.attributes.mode = Mode;
			dpArgSAttr.attributes.mtime.seconds = -1;
			dpArgSAttr.attributes.mtime.useconds = -1;
			dpArgSAttr.attributes.size = -1;
			dpArgSAttr.attributes.uid = -1;
			if( (pAttrStat = nfsproc_setattr_2(dpArgSAttr, clntV2)) == NULL)
				strLastError = clnt_sperror(clntV2, "nfsproc_setattr_2");
			else
			{
				if(pAttrStat->status == NFS_OK)
					Ret = NFS_SUCCESS;
				else
				{
					char  buffer[200];
					sprintf_s(buffer, 200, "nfsproc_setattr_2 %d", pAttrStat->status);
					strLastError = buffer;
				}
				xdr_free((xdrproc_t)xdr_attrstat,(char*) pAttrStat);
			}
			ReleaseBuffer(pNfsData);
		}
	}
	else
		strLastError = "Client is NULL";
	return Ret;
}
int CNFSv2::ChangeOwner(char* pName, int UID, int GID)
{
	int Ret = NFS_ERROR;
	if(clntV2 != NULL)
	{
		sattrargs dpArgSAttr;
		diropargs dpArgs;
		attrstat *pAttrStat;
		dpArgs.name = pName;
		memcpy(dpArgs.dir, nfsCurrentDirectory, FHSIZE);
		NFSData *pNfsData = (NFSData*)GetItemAttributes(pName);
		if(pNfsData != NULL)
		{
			memcpy(dpArgSAttr.file, pNfsData->Handle, FHSIZE);
			dpArgSAttr.attributes.atime.seconds = -1;
			dpArgSAttr.attributes.atime.useconds = -1;
			dpArgSAttr.attributes.gid = GID;
			dpArgSAttr.attributes.mode = -1;
			dpArgSAttr.attributes.mtime.seconds = -1;
			dpArgSAttr.attributes.mtime.useconds = -1;
			dpArgSAttr.attributes.size = -1;
			dpArgSAttr.attributes.uid = UID;
			if( (pAttrStat = nfsproc_setattr_2(dpArgSAttr, clntV2)) == NULL)
				strLastError = clnt_sperror(clntV2, "nfsproc_setattr_2");
			else
			{
				if(pAttrStat->status == NFS_OK)
					Ret = NFS_SUCCESS;
				else
				{
					char  buffer[200];
					sprintf_s(buffer, 200, "nfsproc_setattr_2 %d", pAttrStat->status);
					strLastError = buffer;
				}
				xdr_free((xdrproc_t)xdr_attrstat,(char*) pAttrStat);
			}
			ReleaseBuffer(pNfsData);
		}
	}
	else
		strLastError = "Client is NULL";
	return Ret;
}
int CNFSv2::CreateAuthentication()
{
	int Ret = NFS_ERROR;
	int gis[1];
	if(authType == AUTH_UNIX)
	{
		char HostName[MAX_HOST_NAME + 1];
		if(gethostname(HostName, MAX_HOST_NAME) != -1)
		{
			HostName[MAX_HOST_NAME] = 0;
			gis[0] = gid;
			AUTH *auth = authunix_create(HostName, uid, gid, 1, gis);
			if(auth != NULL)
			{
				if(clntV2!= NULL)
					clntV2->cl_auth = auth;
				if(clntMountV2 != NULL)
					clntMountV2->cl_auth = auth;
				Ret = NFS_SUCCESS;
			}
		}
	}
	return Ret;
}