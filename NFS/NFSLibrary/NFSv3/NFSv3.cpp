/*
 * NekoDrive
 * 2010 by Mirko Gatto
 * mirko.gatto@gmail.com
 *
 * NFS V3 Wrapper
 *
 * Users may use, copy or modify this library 
 * according GNU General Public License v3 (http://www.gnu.org/licenses/gpl.html)
 */

#include "stdafx.h"
#include "NFSv3.h"
#include "NFSv3MountProtocol.h"
#include "NFSv3Protocol.h"
#include "rpc/Pmap_cln.h"
#include <stdio.h>
#include <vector>
#include <string>
#include <time.h>

NFSV3_API CNFSv3* CreateCNFSv3()
{
    return new CNFSv3();
}

NFSV3_API void DisposeCNFSv3(CNFSv3* pObject)
{
    if(pObject != NULL)
    {
		pObject->v.Str.~vector<std::string>();
        delete pObject;
        pObject = NULL;
    }
}

CNFSv3::CNFSv3()
{
	clntMountV3 = NULL;
	clntV3 = NULL;
	sSocket = RPC_ANYSOCK;
	strCurrentDevice = "";
	nfsCurrentFile.data.data_val = new char[FHSIZE];
	memset(nfsCurrentFile.data.data_val, 0, sizeof(FHSIZE));
	nfsCurrentFile.data.data_len = FHSIZE;
	nfsCurrentDirectory.data.data_val = new char[FHSIZE];
	memset(nfsCurrentDirectory.data.data_val, 0, sizeof(FHSIZE));
	nfsCurrentDirectory.data.data_len = FHSIZE;
	uiServer = 0;
	timeOut.tv_sec = 60;
	timeOut.tv_usec = 0;
	sSrvAddr.sin_family = AF_INET;
	authType = AUTH_UNIX;
	uid = -2;
	gid = -2;
}

int CNFSv3::Connect(unsigned int ServerAddress, int UserId, int GroupId)
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
		if( (uPort = pmap_getport(&sSrvAddr, MOUNT_PROGRAM, MOUNT_V3, IPPROTO_UDP)) == 0)
			strLastError = clnt_spcreateerror((char*)strServer.c_str());
		else
		{
			sSrvAddr.sin_port = htons(uPort);
			if ((clntMountV3 = clntudp_create(&sSrvAddr, MOUNT_PROGRAM, MOUNT_V3, timeOut, (int*)&sSocket)) == NULL) 
  				 strLastError = clnt_spcreateerror((char*)strServer.c_str());
			else
			{
				if( (uPort = pmap_getport(&sSrvAddr, NFS_PROGRAM, NFS_V3, IPPROTO_UDP)) == 0 )
					strLastError = clnt_spcreateerror((char*)strServer.c_str());
				else
				{
					sSrvAddr.sin_port = htons(uPort);
					sSocket = RPC_ANYSOCK;
					if ((clntV3 = clntudp_create(&sSrvAddr, NFS_PROGRAM, NFS_V3, timeOut, (int*)&sSocket)) == NULL) 
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

CNFSv3::~CNFSv3()
{
	if(clntMountV3 != NULL)
	{
		Disconnect();
	}
}

int CNFSv3::Disconnect()
{
	int Ret = NFS_ERROR;
	if(clntMountV3 != NULL)
	{	
		if(UnMountDevice() == NFS_SUCCESS)
		{
			if(clntMountV3 != NULL)
			{
				//nfs_auth_destroy(clntMountV3->cl_auth);
				nfs_clnt_destroy(clntMountV3);
			}
			clntMountV3 = NULL;
			if(clntV3 != NULL)
			{
				nfs_auth_destroy(clntV3->cl_auth);
				nfs_clnt_destroy(clntV3);
			}
			clntV3 = NULL;
			Ret = NFS_SUCCESS;
		}
	}
	WSACleanup();
	return Ret;
}

int CNFSv3::UnMountDevice()
{
	int Ret = NFS_ERROR;
	if(clntMountV3 != NULL)
	{
		if(strCurrentDevice != "")
		{
			dirpath dpArgs;
			dpArgs = (dirpath) strCurrentDevice.c_str();
			if( mountproc3_umnt_3(&dpArgs, clntMountV3) == NULL ) 
				strLastError = clnt_sperror(clntMountV3, "mountproc_umnt_1");
			else
			{
				strCurrentDevice = "";
				Ret = NFS_SUCCESS;
			}
		}
		else
			Ret = NFS_SUCCESS;
	}
	return Ret;
}

int CNFSv3::MountDevice(char* pDevice)
{
	int Ret = NFS_ERROR;
	if(pDevice != NULL)
	{
		if(clntMountV3 != NULL)
		{
			dirpath dpArgs = NULL;
			mountres3* pMountRes3 = NULL;
			dpArgs = pDevice;

			if( (pMountRes3 = mountproc3_mnt_3(&dpArgs, clntMountV3)) == NULL ) 
				strLastError = clnt_sperror(clntMountV3, "mountproc3_mnt_3");
			else
			{
				if (pMountRes3->fhs_status == MNT3_OK ) 
				{
					memcpy(nfsCurrentDirectory.data.data_val, pMountRes3->mountres3_u.mountinfo.fhandle.data.data_val, pMountRes3->mountres3_u.mountinfo.fhandle.data.data_len);
					nfsCurrentDirectory.data.data_len = pMountRes3->mountres3_u.mountinfo.fhandle.data.data_len;
					strCurrentDevice = pDevice;
					Ret = NFS_SUCCESS;
				} 
				else 
				{
					char  buffer[200];
					sprintf_s(buffer, 200, "mountproc3_mnt_3 %d", pMountRes3->fhs_status);
					strLastError = buffer;
					printf(strLastError.c_str());
				}
				xdr_free((xdrproc_t)xdr_mountres3,(char*) pMountRes3);
			}
		}
	}
	
	return Ret;
}

char** CNFSv3::GetExportedDevices(int* pnSize)
{
	char** Ret = NULL;
	v.Str.clear();
	if(clntMountV3 == NULL)
		strLastError = "Mount Client is NULL";
	else
	{
		exports *device;
		exports tmp;
		if( (device = mountproc3_export_3(NULL, clntMountV3) ) == NULL )
			strLastError = clnt_sperror(clntMountV3, "mountproc3_export_3");
		else
		{
			tmp = *device;
			while(tmp != NULL)
			{
				v.Str.push_back(tmp->ex_dir);
				tmp = tmp->ex_next;
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

char** CNFSv3::GetItemsList(int* pnSize)
{
	char** Ret = NULL;
	v.Str.clear();
	entry3* pEntry = NULL;
	if(clntV3 != NULL)
	{
		READDIR3args dpRdArgs;
        READDIR3res *pReadDirRes;
		dpRdArgs.cookie = 0;
		memset(dpRdArgs.cookieverf, 0, COOKIEVERFSIZE);
		dpRdArgs.count = 8192;

		dpRdArgs.dir.data.data_len = FHSIZE;
		dpRdArgs.dir.data.data_val = new char[FHSIZE];
		memset(dpRdArgs.dir.data.data_val, 0, FHSIZE);
		
		memcpy(dpRdArgs.dir.data.data_val, nfsCurrentDirectory.data.data_val, FHSIZE);
		while(true)
		{
			if( (pReadDirRes = nfsproc3_readdir_3(&dpRdArgs, clntV3)) == NULL ) 
			{
				strLastError = clnt_sperror(clntV3, "nfsproc3_readdir_3");
				break;
			}
			else
			{
				if(pReadDirRes->status == NFS3_OK)
				{
					pEntry = pReadDirRes->READDIR3res_u.resok.reply.entries;
					while(pEntry != NULL)
					{
						v.Str.push_back(pEntry->name);
						dpRdArgs.cookie = pEntry->cookie;
						memcpy(dpRdArgs.cookieverf, pReadDirRes->READDIR3res_u.resok.cookieverf, COOKIEVERFSIZE);
						pEntry = pEntry->nextentry;
					}
				}
				else
				{
					char  buffer[200];
					sprintf_s(buffer, 200, "nfsproc3_readdir_3 %d", pReadDirRes->status);
					strLastError = buffer;
					break;
				}
				if(pReadDirRes->READDIR3res_u.resok.reply.eof)
					break;
			}
		}
		if(v.Str.size() > 0)
		{
			xdr_free((xdrproc_t)xdr_READDIR3res,(char*) pReadDirRes);
			int nSize = (int) v.Str.size();
			Ret = new char*[nSize];
			for( int i = 0; i < nSize; i++)
				Ret[i] = (char*) v.Str[i].c_str();
			*pnSize = nSize;
		}
	}
	else
		strLastError = "Client is NULL";
	return Ret;
}

void CNFSv3::ReleaseBuffers(void** pBuffers)
{
	if(pBuffers != NULL)
		delete[] pBuffers;
}

void* CNFSv3::GetItemAttributes(char* pName)
{
	void* Ret = NULL;
	//if(clntV3 != NULL)
	//{
	//	diropargs dpDrArgs;
	//	diropres *pDirOpRes;
	//	memcpy(dpDrArgs.dir, nfsCurrentDirectory, FHSIZE);
	//	dpDrArgs.name = pName;
	//	if( (pDirOpRes = nfsproc_lookup_2(&dpDrArgs, clntV3)) == NULL ) 
	//		strLastError = clnt_sperror(clntV3,"nfsproc_lookup_2");
	//	else
	//	{
	//		if (pDirOpRes->status == NFS_OK) 
	//		{
	//			NFSData* pNfsData = new NFSData;
	//			InitStructure(pNfsData);
	//			memcpy(pNfsData->Handle, pDirOpRes->diropres_u.ok.file, FHSIZE);
	//			pNfsData->DateTime = pDirOpRes->diropres_u.ok.attributes.ctime.seconds;
	//			pNfsData->Type = pDirOpRes->diropres_u.ok.attributes.type;
	//			pNfsData->Size = pDirOpRes->diropres_u.ok.attributes.size;
	//			pNfsData->Blocks = pDirOpRes->diropres_u.ok.attributes.blocks;
	//			pNfsData->BlockSize = pDirOpRes->diropres_u.ok.attributes.blocksize;
	//			Ret = pNfsData;	
	//		}
	//		else
	//		{
	//			char  buffer[200];
	//			sprintf_s(buffer, 200, "nfsproc_lookup_2 %d", pDirOpRes->status);
	//			strLastError = buffer;
	//		}
	//		xdr_free((xdrproc_t)xdr_diropres,(char*) pDirOpRes);
	//	}
	//}
	//else
	//	strLastError = "Client is NULL";
	return Ret;
}

void CNFSv3::ReleaseBuffer(void* pBuffer)
{
	if(pBuffer != NULL)
		delete pBuffer;
}

void CNFSv3::InitStructure(NFSData* pNfsData)
{
	//memset(pNfsData->Handle, 0, FHSIZE);
	pNfsData->DateTime = 0;
	pNfsData->Type = 0;
	pNfsData->Blocks = 0;
	pNfsData->DateTime = 0;
	pNfsData->BlockSize = 0;
}

int CNFSv3::ChangeCurrentDirectory(char *pName)
{
	int Ret = NFS_ERROR;
	//if(pName != NULL)
	//{
	//	NFSData* pNfsData = (NFSData*) GetItemAttributes(pName);
	//	if(pNfsData != NULL)
	//	{
	//		memcpy(nfsCurrentDirectory, pNfsData->Handle, FHSIZE);
	//		ReleaseBuffer(pNfsData);
	//		Ret = NFS_SUCCESS;
	//	}
	//}
	//else
	//	strLastError = "Name is NULL";
	return Ret;
}

int CNFSv3::CreateDirectory(char* pName)
{
	int Ret = NFS_ERROR;
	//if(clntV3 != NULL)
	//{
	//	createargs dpArgCreate;
 //       diropres *pDirOpRes;
	//	dpArgCreate.attributes.atime.seconds = -1;
	//	dpArgCreate.attributes.atime.useconds = -1;
	//	dpArgCreate.attributes.mtime.seconds = -1;
	//	dpArgCreate.attributes.mtime.useconds = -1;
	//	dpArgCreate.attributes.mode = MODE_DIR | 0777;
	//	dpArgCreate.attributes.gid = gid;
	//	dpArgCreate.attributes.uid = uid;
	//	memcpy(dpArgCreate.where.dir, nfsCurrentDirectory, FHSIZE);
 //       dpArgCreate.where.name = pName;
 //       if( (pDirOpRes = nfsproc_mkdir_2(&dpArgCreate, clntV3)) == NULL ) 
	//		strLastError = clnt_sperror(clntV3, "nfsproc_mkdir_2");
	//	else
	//	{
	//		if (pDirOpRes->status == NFS_OK) 
	//			Ret = NFS_SUCCESS;
	//		else 
	//		{
	//			char  buffer[200];
	//			sprintf_s(buffer, 200, "nfsproc_mkdir_2 %d", pDirOpRes->status);
	//			strLastError = buffer;
	//		}
	//		xdr_free((xdrproc_t)xdr_diropres,(char*) pDirOpRes);
	//	}
	//}
	//else
	//	strLastError = "Client is NULL";
	return Ret;
}

int CNFSv3::DeleteDirectory(char* pName)
{
	int Ret = NFS_ERROR;
	//if(clntV3 != NULL)
	//{
	//	diropargs dpArgDelete;
 //       nfsstat *pNfsStat;
	//	memcpy(dpArgDelete.dir, nfsCurrentDirectory, FHSIZE);
 //       dpArgDelete.name = pName;
 //       if( (pNfsStat = nfsproc_rmdir_2(&dpArgDelete, clntV3)) == NULL ) 
	//		strLastError = clnt_sperror(clntV3, "nfsproc_rmdir_2");
	//	else
	//	{
	//		if (*pNfsStat == NFS_OK) 
	//			Ret = NFS_SUCCESS;
	//		else 
	//		{
	//			char  buffer[200];
	//			sprintf_s(buffer, 200, "nfsproc_rmdir_2 %d", pNfsStat);
	//			strLastError = buffer;
	//		}
	//		xdr_free((xdrproc_t)xdr_nfsstat,(char*) pNfsStat);
	//	}
	//}
	//else
	//	strLastError = "Client is NULL";
	return Ret;
}

int CNFSv3::DeleteFile(char* pName)
{
	int Ret = NFS_ERROR;
	//if(clntV3 != NULL)
	//{
	//	diropargs dpArgDelete;
 //       nfsstat *pNfsStat;
	//	memcpy(dpArgDelete.dir, nfsCurrentDirectory, FHSIZE);
 //       dpArgDelete.name = pName;
 //       if( (pNfsStat = nfsproc_remove_2(&dpArgDelete, clntV3)) == NULL ) 
	//		strLastError = clnt_sperror(clntV3, "nfsproc_remove_2");
	//	else
	//	{
	//		if (*pNfsStat == NFS_OK) 
	//			Ret = NFS_SUCCESS;
	//		else 
	//		{
	//			char  buffer[200];
	//			sprintf_s(buffer, 200, "nfsproc_remove_2 %d", pNfsStat);
	//			strLastError = buffer;
	//		}
	//		xdr_free((xdrproc_t)xdr_nfsstat,(char*) pNfsStat);
	//	}
	//}
	//else
	//	strLastError = "Client is NULL";
	return Ret;
}

int CNFSv3::CreateFile(char* pName)
{
	int Ret = NFS_ERROR;
	//if(clntV3 != NULL)
	//{
	//	createargs dpArgCreate;
 //       diropres *pDirOpRes;
	//	dpArgCreate.attributes.atime.seconds = -1;
	//	dpArgCreate.attributes.atime.useconds = -1;
	//	dpArgCreate.attributes.mtime.seconds = -1;
	//	dpArgCreate.attributes.mtime.useconds = -1;
	//	dpArgCreate.attributes.mode = MODE_REG | 0777;
	//	dpArgCreate.attributes.gid = gid;
	//	dpArgCreate.attributes.size = -1;
	//	dpArgCreate.attributes.uid = uid;
	//	memcpy(dpArgCreate.where.dir, nfsCurrentDirectory, FHSIZE);
 //       dpArgCreate.where.name = pName;
 //       if( (pDirOpRes = nfsproc_create_2(&dpArgCreate, clntV3)) == NULL ) 
	//		strLastError = clnt_sperror(clntV3, "nfsproc_create_2");
	//	else
	//	{
	//		if (pDirOpRes->status == NFS_OK) 
	//			Ret = NFS_SUCCESS;
	//		else 
	//		{
	//			char  buffer[200];
	//			sprintf_s(buffer, 200, "nfsproc_create_2 %d", pDirOpRes->status);
	//			strLastError = buffer;
	//		}
	//		xdr_free((xdrproc_t)xdr_diropres,(char*) pDirOpRes);
	//	}
	//}
	//else
	//	strLastError = "Client is NULL";
	return Ret;
}

int CNFSv3::Open(char* pName)
{
	int Ret = NFS_ERROR;
	//if(pName != NULL)
	//{
	//	NFSData* pNfsData = (NFSData*) GetItemAttributes(pName);
	//	if(pNfsData != NULL)
	//	{
	//		memcpy(nfsCurrentFile, pNfsData->Handle, FHSIZE);
	//		ReleaseBuffer(pNfsData);
	//		Ret = NFS_SUCCESS;
	//	}
	//}
	return Ret;
}

void CNFSv3::CloseFile()
{
	//memset(nfsCurrentFile, 0, sizeof(FHSIZE));
}

int CNFSv3::CheckOpenHandle()
{
	int sum = 0;
	/*for(int x = 0; x < FHSIZE; x++)
		sum += nfsCurrentFile[x];*/
	return sum;
}

int CNFSv3::Read(u_int Offset, u_int Count, char* pBuffer, u_long* pSize)
{
	int Ret = NFS_ERROR;
	//if(clntV3 != NULL)
	//{
	//	if(CheckOpenHandle() == NULL)
	//		strLastError = "handle closed";
	//	else
	//	{
	//		readargs dpArgRead;
	//		readres *pReadRes;
	//		memcpy(dpArgRead.file, nfsCurrentFile, FHSIZE);
	//		dpArgRead.offset = Offset;
	//		dpArgRead.count = Count;
	//		if( (pReadRes = nfsproc_read_2(&dpArgRead, clntV3)) == NULL ) 
	//			strLastError = clnt_sperror(clntV3, "nfsproc_read_2");
	//		else
	//		{
	//			if (pReadRes->status == NFS_OK) 
	//			{
	//				*pSize = pReadRes->readres_u.ok.data.data_len;
	//				memcpy(pBuffer, pReadRes->readres_u.ok.data.data_val, pReadRes->readres_u.ok.data.data_len);
	//				Ret = NFS_SUCCESS;
	//			} 
	//			else 
	//			{
	//				char  buffer[200];
	//				sprintf_s(buffer, 200, "nfsproc_read_2 %d", pReadRes->status);
	//				strLastError = buffer;
	//			}
	//			xdr_free((xdrproc_t)xdr_readres,(char*) pReadRes);
	//		}
	//	}
	//}
	//else
	//	strLastError = "Client is NULL";
	return Ret;
}

int CNFSv3::Write(u_int Offset, u_int Count, char* pBuffer, u_long* pSize)
{
	int Ret = NFS_ERROR;
	//if(clntV3 != NULL)
	//{
	//	if(CheckOpenHandle() == NULL)
	//		strLastError = "handle closed";
	//	else
	//	{
	//		writeargs dpArgWrite;
	//		attrstat *pAttrStat;
	//		memcpy(dpArgWrite.file, nfsCurrentFile, FHSIZE);
	//		dpArgWrite.offset = Offset;
	//		dpArgWrite.data.data_len = Count;
	//		dpArgWrite.data.data_val = pBuffer;
	//		if( (pAttrStat = nfsproc_write_2(&dpArgWrite, clntV3)) == NULL )
	//			strLastError = clnt_sperror(clntV3, "nfsproc_write_2");
	//		else
	//		{
	//			if (pAttrStat->status == NFS_OK) 
	//			{
	//				*pSize = pAttrStat->attrstat_u.attributes.size;
	//				Ret = NFS_SUCCESS;
	//			} 
	//			else 
	//			{
	//				char  buffer[200];
	//				sprintf_s(buffer, 200, "nfsproc_write_2 %d", pAttrStat->status);
	//				strLastError = buffer;
	//			}
	//			xdr_free((xdrproc_t)xdr_attrstat,(char*) pAttrStat);
	//		}
	//	}
	//}
	//else
	//	strLastError = "Client is NULL";
	return Ret;
}

int CNFSv3::Rename(char* pOldName, char* pNewName)
{
	int Ret = NFS_ERROR;
	//if(clntV3 != NULL)
	//{
	//	renameargs dpArgRename;
 //       nfsstat *pNfsStat;
	//	memcpy(dpArgRename.from.dir, nfsCurrentDirectory, FHSIZE);
	//	memcpy(dpArgRename.to.dir, nfsCurrentDirectory, FHSIZE);
 //       dpArgRename.from.name = pOldName;
	//	dpArgRename.to.name = pNewName;
 //       if( (pNfsStat = nfsproc_rename_2(&dpArgRename, clntV3)) == NULL ) 
	//		strLastError = clnt_sperror(clntV3, "nfsproc_rename_2");
	//	else
	//	{
	//		if (*pNfsStat == NFS_OK) 
	//			Ret = NFS_SUCCESS;
	//		else 
	//		{
	//			char  buffer[200];
	//			sprintf_s(buffer, 200, "nfsproc_rename_2 %d", pNfsStat);
	//			strLastError = buffer;
	//		}
	//		xdr_free((xdrproc_t)xdr_nfsstat,(char*) pNfsStat);
	//	}
	//}
	//else
	//	strLastError = "Client is NULL";
	return Ret;
}

const char* CNFSv3::GetLastNfsError()
{
	return strLastError.c_str();
}

int CNFSv3::ChangeMode(char* pName, int Mode)
{
	int Ret = NFS_ERROR;
	//if(clntV3 != NULL)
	//{
	//	sattrargs dpArgSAttr;
	//	diropargs dpArgs;
	//	attrstat *pAttrStat;
	//	dpArgs.name = pName;
	//	memcpy(dpArgs.dir, nfsCurrentDirectory, FHSIZE);
	//	NFSData *pNfsData = (NFSData*)GetItemAttributes(pName);
	//	if(pNfsData != NULL)
	//	{
	//		memcpy(dpArgSAttr.file, pNfsData->Handle, FHSIZE);
	//		dpArgSAttr.attributes.atime.seconds = -1;
	//		dpArgSAttr.attributes.atime.useconds = -1;
	//		dpArgSAttr.attributes.gid = -1;
	//		dpArgSAttr.attributes.mode = Mode;
	//		dpArgSAttr.attributes.mtime.seconds = -1;
	//		dpArgSAttr.attributes.mtime.useconds = -1;
	//		dpArgSAttr.attributes.size = -1;
	//		dpArgSAttr.attributes.uid = -1;
	//		if( (pAttrStat = nfsproc_setattr_2(dpArgSAttr, clntV3)) == NULL)
	//			strLastError = clnt_sperror(clntV3, "nfsproc_setattr_2");
	//		else
	//		{
	//			if(pAttrStat->status == NFS_OK)
	//				Ret = NFS_SUCCESS;
	//			else
	//			{
	//				char  buffer[200];
	//				sprintf_s(buffer, 200, "nfsproc_setattr_2 %d", pAttrStat->status);
	//				strLastError = buffer;
	//			}
	//			xdr_free((xdrproc_t)xdr_attrstat,(char*) pAttrStat);
	//		}
	//		ReleaseBuffer(pNfsData);
	//	}
	//}
	//else
	//	strLastError = "Client is NULL";
	return Ret;
}
int CNFSv3::ChangeOwner(char* pName, int UID, int GID)
{
	int Ret = NFS_ERROR;
	//if(clntV3 != NULL)
	//{
	//	sattrargs dpArgSAttr;
	//	diropargs dpArgs;
	//	attrstat *pAttrStat;
	//	dpArgs.name = pName;
	//	memcpy(dpArgs.dir, nfsCurrentDirectory, FHSIZE);
	//	NFSData *pNfsData = (NFSData*)GetItemAttributes(pName);
	//	if(pNfsData != NULL)
	//	{
	//		memcpy(dpArgSAttr.file, pNfsData->Handle, FHSIZE);
	//		dpArgSAttr.attributes.atime.seconds = -1;
	//		dpArgSAttr.attributes.atime.useconds = -1;
	//		dpArgSAttr.attributes.gid = GID;
	//		dpArgSAttr.attributes.mode = -1;
	//		dpArgSAttr.attributes.mtime.seconds = -1;
	//		dpArgSAttr.attributes.mtime.useconds = -1;
	//		dpArgSAttr.attributes.size = -1;
	//		dpArgSAttr.attributes.uid = UID;
	//		if( (pAttrStat = nfsproc_setattr_2(dpArgSAttr, clntV3)) == NULL)
	//			strLastError = clnt_sperror(clntV3, "nfsproc_setattr_2");
	//		else
	//		{
	//			if(pAttrStat->status == NFS_OK)
	//				Ret = NFS_SUCCESS;
	//			else
	//			{
	//				char  buffer[200];
	//				sprintf_s(buffer, 200, "nfsproc_setattr_2 %d", pAttrStat->status);
	//				strLastError = buffer;
	//			}
	//			xdr_free((xdrproc_t)xdr_attrstat,(char*) pAttrStat);
	//		}
	//		ReleaseBuffer(pNfsData);
	//	}
	//}
	//else
	//	strLastError = "Client is NULL";
	return Ret;
}
int CNFSv3::CreateAuthentication()
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
				if(clntV3!= NULL)
					clntV3->cl_auth = auth;
				if(clntMountV3 != NULL)
					clntMountV3->cl_auth = auth;
				Ret = NFS_SUCCESS;
			}
		}
	}
	return Ret;
}