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

#include "stdafx.h"
#include "NFSv3.h"
#include "NFSv3MountProtocol.h"
#include "NFSv3Protocol.h"
#include "NFSCommon.h"
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
	memset(nfsCurrentFile, 0, sizeof(FHSIZE));
	memset(nfsCurrentDirectory, 0, sizeof(FHSIZE));
	memset(nfsRootDirectory, 0, sizeof(FHSIZE));
	timeOut.tv_sec = 60;
	timeOut.tv_usec = 0;
	sSrvAddr.sin_family = AF_INET;
	authType = AUTH_UNIX;
	uid = -2;
	gid = -2;
}

int CNFSv3::Connect(const char* ServerAddress, int UserId, int GroupId, long CommandTimeout)
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
		sprintf_s(buffer, 200, "WSAStartup: %d", iWSAErr);
		strLastError = buffer;
	}
	else
	{
		u_short uPort;
		sSrvAddr.sin_addr.s_addr = inet_addr(ServerAddress);
		strServer.assign(inet_ntoa(sSrvAddr.sin_addr));
		if( (uPort = pmap_getport(&sSrvAddr, MOUNT_PROGRAM, MOUNT_V3, IPPROTO_UDP)) == 0)
			strLastError = clnt_spcreateerror((char*)strServer.c_str());
		else
		{
			timeOut.tv_sec = CommandTimeout;
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
				memset(nfsCurrentFile, 0, sizeof(FHSIZE));
				memset(nfsCurrentDirectory, 0, sizeof(FHSIZE));
				Ret = NFS_SUCCESS;
			}
		}
		else
			Ret = NFS_SUCCESS;
	}
	else
		strLastError = "mountproc3_umnt_3: Mount Client is NULL";

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
					memcpy(nfsCurrentDirectory, pMountRes3->mountres3_u.mountinfo.fhandle.data.data_val, pMountRes3->mountres3_u.mountinfo.fhandle.data.data_len);
					memcpy(nfsRootDirectory, pMountRes3->mountres3_u.mountinfo.fhandle.data.data_val, pMountRes3->mountres3_u.mountinfo.fhandle.data.data_len);
					strCurrentDevice = pDevice;
					Ret = NFS_SUCCESS;
				} 
				else 
				{
					char  buffer[200];
					sprintf_s(buffer, 200, "mountproc3_mnt_3: %d", pMountRes3->fhs_status);
					strLastError = buffer;
					printf(strLastError.c_str());
				}
				xdr_free((xdrproc_t)xdr_mountres3,(char*) pMountRes3);
			}
		}
		else
			strLastError = "mountproc3_mnt_3: Mount Client is NULL";
	}
	
	return Ret;
}

char** CNFSv3::GetExportedDevices(int* pnSize)
{
	char** Ret = NULL;
	v.Str.clear();
	if(clntMountV3 == NULL)
		strLastError = "mountproc3_export_3: Mount Client is NULL";
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

char** CNFSv3::GetItemsList(char* pDirectory, int* pnSize)
{
	char** Ret = NULL;
	if(GetItemHandle(pDirectory, nfsCurrentDirectory) == NFS_SUCCESS)
		Ret = GetItemsList(pnSize);
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
		dpRdArgs.count = 4096;
		dpRdArgs.dir.data.data_len = FHSIZE;
		dpRdArgs.dir.data.data_val = nfsCurrentDirectory;
		dpRdArgs.cookie = 0;
		memset(dpRdArgs.cookieverf, 0, COOKIEVERFSIZE);
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
					sprintf_s(buffer, 200, "nfsproc3_readdir_3: %d", pReadDirRes->status);
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
		strLastError = "nfsproc3_readdir_3: Client is NULL";
	return Ret;
}

void CNFSv3::ReleaseBuffers(void** pBuffers)
{
	if(pBuffers != NULL)
		delete[] pBuffers;
}

void* CNFSv3::GetItemAttributes(char* pName, char* pDirectory)
{
	void* Ret = NULL;
	if(GetItemHandle(pDirectory, nfsCurrentDirectory) == NFS_SUCCESS)
		Ret = GetItemAttributes(pName);
	return Ret;
}

void* CNFSv3::GetItemAttributes(char* pName)
{
	void* Ret = NULL;
	if(clntV3 != NULL)
	{
		LOOKUP3args dpLookUpArgs;
		LOOKUP3res *pLookUpRes;
		dpLookUpArgs.what.dir.data.data_val = nfsCurrentDirectory;
		dpLookUpArgs.what.dir.data.data_len = FHSIZE;
		dpLookUpArgs.what.name = pName;
		
		if( (pLookUpRes = nfsproc3_lookup_3(&dpLookUpArgs, clntV3)) == NULL ) 
			strLastError = clnt_sperror(clntV3,"nfsproc3_lookup_3");
		else
		{
			if (pLookUpRes->status == NFS3_OK) 
			{
				NFSData* pNfsData = new NFSData;
				InitStructure(pNfsData);
				memcpy(pNfsData->Handle, pLookUpRes->LOOKUP3res_u.resok.obj.data.data_val, pLookUpRes->LOOKUP3res_u.resok.obj.data.data_len);
				pNfsData->DateTime = pLookUpRes->LOOKUP3res_u.resok.obj_attributes.post_op_attr_u.attributes.ctime.seconds;
				pNfsData->Type = pLookUpRes->LOOKUP3res_u.resok.obj_attributes.post_op_attr_u.attributes.type;
				pNfsData->Size = pLookUpRes->LOOKUP3res_u.resok.obj_attributes.post_op_attr_u.attributes.size;
				Ret = pNfsData;	
			}
			else
			{
				char  buffer[200];
				sprintf_s(buffer, 200, "nfsproc3_lookup_3: %d", pLookUpRes->status);
				strLastError = buffer;
			}
			xdr_free((xdrproc_t)xdr_LOOKUP3res,(char*) pLookUpRes);
		}
	}
	else
		strLastError = "nfsproc3_lookup_3: Client is NULL";
	return Ret;
}

void CNFSv3::ReleaseBuffer(void* pBuffer)
{
	if(pBuffer != NULL)
		delete pBuffer;
}

void CNFSv3::InitStructure(NFSData* pNfsData)
{
	memset(pNfsData->Handle, 0, FHSIZE);
	pNfsData->DateTime = 0;
	pNfsData->Type = 0;
}

int CNFSv3::ChangeCurrentDirectory(char *pName)
{
	int Ret = NFS_ERROR;
	if(pName != NULL)
	{
		if(strcmp(pName, ".") ==0)
		{
			memcpy(nfsCurrentDirectory, nfsRootDirectory, FHSIZE);
			Ret = NFS_SUCCESS;
		}
		else
		{
			NFSData* pNfsData = (NFSData*) GetItemAttributes(pName);
			if(pNfsData != NULL)
			{
				memcpy(nfsCurrentDirectory, pNfsData->Handle, FHSIZE);
				ReleaseBuffer(pNfsData);
				Ret = NFS_SUCCESS;
			}
		}
	}
	else
		strLastError = "ChangeCurrentDirectory: Name is NULL";
	return Ret;
}

int CNFSv3::CreateDirectory(char* pName, char* pDirectory)
{
	int Ret = NFS_ERROR;
	if((Ret = GetItemHandle(pDirectory, nfsCurrentDirectory)) == NFS_SUCCESS)
		Ret = CreateDirectory(pName);
	return Ret;
}

int CNFSv3::CreateDirectory(char* pName)
{
	int Ret = NFS_ERROR;
	if(clntV3 != NULL)
	{
		MKDIR3args dpMkDirArgs;
        MKDIR3res *pMkDirRes;
		dpMkDirArgs.attributes.atime.set_it = DONT_CHANGE;
		dpMkDirArgs.attributes.mtime.set_it = DONT_CHANGE;
		dpMkDirArgs.attributes.size.set_it = DONT_CHANGE;
		dpMkDirArgs.attributes.mode.set_it = DONT_CHANGE;
		dpMkDirArgs.attributes.mode.set_mode3_u.mode = 0777;
		dpMkDirArgs.attributes.gid.set_it = TRUE;
		dpMkDirArgs.attributes.gid.set_gid3_u.gid = gid;
		dpMkDirArgs.attributes.uid.set_it = TRUE;
		dpMkDirArgs.attributes.uid.set_uid3_u.uid = gid;
		dpMkDirArgs.where.dir.data.data_val = nfsCurrentDirectory;
		dpMkDirArgs.where.dir.data.data_len = FHSIZE;
        dpMkDirArgs.where.name = pName;
        if( (pMkDirRes = nfsproc3_mkdir_3(&dpMkDirArgs, clntV3)) == NULL ) 
			strLastError = clnt_sperror(clntV3, "nfsproc3_mkdir_3");
		else
		{
			if (pMkDirRes->status == NFS3_OK) 
				Ret = NFS_SUCCESS;
			else 
			{
				char  buffer[200];
				sprintf_s(buffer, 200, "nfsproc3_mkdir_3 %d", pMkDirRes->status);
				strLastError = buffer;
			}
			xdr_free((xdrproc_t)xdr_MKDIR3res,(char*) pMkDirRes);
		}
	}
	else
		strLastError = "nfsproc3_mkdir_3: Client is NULL";
	return Ret;
}

int CNFSv3::DeleteDirectory(char* pName, char* pDirectory)
{
	int Ret = NFS_ERROR;
	if((Ret = GetItemHandle(pDirectory, nfsCurrentDirectory)) == NFS_SUCCESS)
		Ret = DeleteDirectory(pName);
	return Ret;
}

int CNFSv3::DeleteDirectory(char* pName)
{
	int Ret = NFS_ERROR;
	if(clntV3 != NULL)
	{
		RMDIR3args dpRmDirArgs;
        RMDIR3res *pRmDirRes;
		dpRmDirArgs.obj.dir.data.data_val = nfsCurrentDirectory;
		dpRmDirArgs.obj.dir.data.data_len = FHSIZE;
		dpRmDirArgs.obj.name = pName;
        if( (pRmDirRes = nfsproc3_rmdir_3(&dpRmDirArgs, clntV3)) == NULL ) 
			strLastError = clnt_sperror(clntV3, "nfsproc3_rmdir_3");
		else
		{
			if (pRmDirRes->status == NFS3_OK) 
				Ret = NFS_SUCCESS;
			else 
			{
				char  buffer[200];
				sprintf_s(buffer, 200, "nfsproc3_rmdir_3: %d", pRmDirRes->status);
				strLastError = buffer;
			}
			xdr_free((xdrproc_t)xdr_RMDIR3res,(char*) pRmDirRes);
		}
	}
	else
		strLastError = "nfsproc3_rmdir_3: Client is NULL";
	return Ret;
}

int CNFSv3::DeleteFile(char* pName, char* pDirectory)
{
	int Ret = NFS_ERROR;
	if((Ret = GetItemHandle(pDirectory, nfsCurrentDirectory)) == NFS_SUCCESS)
		Ret = DeleteFile(pName);
	return Ret;
}

int CNFSv3::DeleteFile(char* pName)
{
	int Ret = NFS_ERROR;
	if(clntV3 != NULL)
	{
		REMOVE3args dpRemoveArgs;
        REMOVE3res *pRemoveRes;
		dpRemoveArgs.obj.dir.data.data_val = nfsCurrentDirectory;
		dpRemoveArgs.obj.dir.data.data_len = FHSIZE;
		dpRemoveArgs.obj.name = pName;
        if( (pRemoveRes = nfsproc3_remove_3(&dpRemoveArgs, clntV3)) == NULL ) 
			strLastError = clnt_sperror(clntV3, "nfsproc3_remove_3");
		else
		{
			if (pRemoveRes->status == NFS3_OK) 
				Ret = NFS_SUCCESS;
			else 
			{
				char  buffer[200];
				sprintf_s(buffer, 200, "nfsproc3_remove_3: %d", pRemoveRes->status);
				strLastError = buffer;
			}
			xdr_free((xdrproc_t)xdr_REMOVE3res,(char*) pRemoveRes);
		}
	}
	else
		strLastError = "nfsproc3_remove_3: Client is NULL";
	return Ret;
}

int CNFSv3::CreateFile(char* pName, char* pDirectory)
{
	int Ret = NFS_ERROR;
	if((Ret = GetItemHandle(pDirectory, nfsCurrentDirectory)) == NFS_SUCCESS)
		Ret = CreateFile(pName);
	return Ret;
}

int CNFSv3::CreateFile(char* pName)
{
	int Ret = NFS_ERROR;
	if(clntV3 != NULL)
	{
		CREATE3args dpCreateArgs;
        CREATE3res *pCreateRes;
		dpCreateArgs.how.createhow3_u.obj_attributes.atime.set_it = DONT_CHANGE;
		dpCreateArgs.how.createhow3_u.obj_attributes.mtime.set_it = DONT_CHANGE;
		dpCreateArgs.how.createhow3_u.obj_attributes.size.set_it = DONT_CHANGE;
		dpCreateArgs.how.createhow3_u.obj_attributes.mode.set_it = DONT_CHANGE;
		dpCreateArgs.how.createhow3_u.obj_attributes.mode.set_mode3_u.mode = 0777;
		dpCreateArgs.how.createhow3_u.obj_attributes.gid.set_it = TRUE;
		dpCreateArgs.how.createhow3_u.obj_attributes.gid.set_gid3_u.gid = gid;
		dpCreateArgs.how.createhow3_u.obj_attributes.uid.set_it = TRUE;
		dpCreateArgs.how.createhow3_u.obj_attributes.uid.set_uid3_u.uid = gid;
		dpCreateArgs.how.mode = UNCHECKED;
		dpCreateArgs.where.dir.data.data_val = nfsCurrentDirectory;
		dpCreateArgs.where.dir.data.data_len = FHSIZE;
        dpCreateArgs.where.name = pName;
		memset(dpCreateArgs.how.createhow3_u.verf, 0, CREATEVERFSIZE);
		if( (pCreateRes = nfsproc3_create_3(&dpCreateArgs, clntV3)) == NULL ) 
			strLastError = clnt_sperror(clntV3, "nfsproc3_create_3");
		else
		{
			if (pCreateRes->status == NFS3_OK) 
				Ret = NFS_SUCCESS;
			else 
			{
				char  buffer[200];
				sprintf_s(buffer, 200, "nfsproc3_create_3: %d", pCreateRes->status);
				strLastError = buffer;
			}
			xdr_free((xdrproc_t)xdr_CREATE3res,(char*) pCreateRes);
		}
	}
	else
		strLastError = "nfsproc3_create_3: Client is NULL";
	return Ret;
}

int CNFSv3::Open(char* pName)
{
	int Ret = NFS_ERROR;
	if(pName != NULL)
		Ret = GetItemHandle(pName, nfsCurrentFile);
	return Ret;
}

void CNFSv3::CloseFile()
{
	memset(nfsCurrentFile, 0, sizeof(FHSIZE));
}

int CNFSv3::CheckOpenHandle()
{
	int sum = 0;
	for(int x = 0; x < FHSIZE; x++)
	{
		if(nfsCurrentFile[x] == 0)
		{
			sum++;
		}
	}
	if(sum == FHSIZE)
		return 0;
	else
		return sum;
}

int CNFSv3::Read(char* pName, unsigned __int64 Offset, u_long Count, char* pBuffer, u_int* pSize)
{
	int Ret = NFS_ERROR;
	if((Ret = Open(pName)) == NFS_SUCCESS)
	{
		Ret = Read(Offset, Count, pBuffer, pSize);
		CloseFile();
	}
	return Ret;
}

int CNFSv3::Read(unsigned __int64 Offset, u_long Count, char* pBuffer, u_int* pSize)
{
	int Ret = NFS_ERROR;
	if(clntV3 != NULL)
	{
		if(CheckOpenHandle() == NULL)
			strLastError = "nfsproc3_read_3: handle closed";
		else
		{
			READ3args dpReadArgs;
			READ3res *pReadRes;
			dpReadArgs.file.data.data_val = nfsCurrentFile;
			dpReadArgs.file.data.data_len = FHSIZE;
			dpReadArgs.offset = Offset;
			dpReadArgs.count = Count;
			if( (pReadRes = nfsproc3_read_3(&dpReadArgs, clntV3)) == NULL ) 
				strLastError = clnt_sperror(clntV3, "nfsproc3_read_3");
			else
			{
				if (pReadRes->status == NFS3_OK) 
				{
					*pSize = pReadRes->READ3res_u.resok.data.data_len;
					memcpy(pBuffer, pReadRes->READ3res_u.resok.data.data_val, pReadRes->READ3res_u.resok.data.data_len);
					Ret = NFS_SUCCESS;
				} 
				else 
				{
					char  buffer[200];
					sprintf_s(buffer, 200, "nfsproc3_read_3: %d", pReadRes->status);
					strLastError = buffer;
				}
				xdr_free((xdrproc_t)xdr_READ3res,(char*) pReadRes);
			}
		}
	}
	else
		strLastError = "nfsproc3_read_3: Client is NULL";
	return Ret;
}

int CNFSv3::Write(char* pName, uint64 Offset, uint32 Count, char* pBuffer, u_int* pSize)
{
	int Ret = NFS_ERROR;
	if((Ret = Open(pName)) == NFS_SUCCESS)
	{
		Ret = Write(Offset, Count, pBuffer, pSize);
		CloseFile();
	}
	return Ret;
}

int CNFSv3::Write(uint64 Offset, uint32 Count, char* pBuffer, u_int* pSize)
{
	int Ret = NFS_ERROR;
	if(clntV3 != NULL)
	{
		if(CheckOpenHandle() == NULL)
			strLastError = "nfsproc3_write_3: handle closed";
		else
		{
			WRITE3args dpWriteArgs;
			WRITE3res *pWriteRes;
			dpWriteArgs.file.data.data_val = nfsCurrentFile;
			dpWriteArgs.file.data.data_len = FHSIZE;
			dpWriteArgs.offset = Offset;
			dpWriteArgs.count = Count;
			dpWriteArgs.data.data_len = Count;
			dpWriteArgs.data.data_val = pBuffer;
			dpWriteArgs.stable = UNSTABLE;

			if( (pWriteRes = nfsproc3_write_3(&dpWriteArgs, clntV3)) == NULL )
				strLastError = clnt_sperror(clntV3, "nfsproc3_write_3");
			else
			{
				if (pWriteRes->status == NFS3_OK) 
				{
					*pSize = pWriteRes->WRITE3res_u.resok.count;
					Ret = NFS_SUCCESS;
				} 
				else 
				{
					char  buffer[200];
					sprintf_s(buffer, 200, "nfsproc3_write_3: %d", pWriteRes->status);
					strLastError = buffer;
				}
				xdr_free((xdrproc_t)xdr_WRITE3res,(char*) pWriteRes);
			}
		}
	}
	else
		strLastError = "nfsproc3_write_3: Client is NULL";
	return Ret;
}

int CNFSv3::Rename(char* pOldName, char* pNewName)
{
	int Ret = NFS_ERROR;
	if(clntV3 != NULL)
	{
		RENAME3args dpRenameArgs;
        RENAME3res *pRenameRes;
		dpRenameArgs.from.dir.data.data_val = nfsCurrentDirectory;
		dpRenameArgs.from.dir.data.data_len = FHSIZE;
		dpRenameArgs.to.dir.data.data_val = nfsCurrentDirectory;
		dpRenameArgs.to.dir.data.data_len = FHSIZE;
        dpRenameArgs.from.name = pOldName;
		dpRenameArgs.to.name = pNewName;
        if( (pRenameRes = nfsproc3_rename_3(&dpRenameArgs, clntV3)) == NULL ) 
			strLastError = clnt_sperror(clntV3, "nfsproc3_rename_3");
		else
		{
			if (pRenameRes->status == NFS3_OK) 
				Ret = NFS_SUCCESS;
			else 
			{
				char  buffer[200];
				sprintf_s(buffer, 200, "nfsproc3_rename_3: %d", pRenameRes->status);
				strLastError = buffer;
			}
			xdr_free((xdrproc_t)xdr_RENAME3res,(char*) pRenameRes);
		}
	}
	else
		strLastError = "nfsproc3_rename_3: Client is NULL";
	return Ret;
}

int CNFSv3::GetItemHandle(char* Path, char* Handle)
{
	int Ret = NFS_ERROR;
	char *pName;
	char currentItem[FHSIZE];

	memcpy(currentItem, nfsRootDirectory, FHSIZE);
	pName = strtok(Path, "\\");
	while(pName != NULL)
	{
		LOOKUP3args dpLookUpArgs;
		LOOKUP3res *pLookUpRes;
		dpLookUpArgs.what.dir.data.data_val = currentItem;
		dpLookUpArgs.what.dir.data.data_len = FHSIZE;
		dpLookUpArgs.what.name = pName;
		
		if( (pLookUpRes = nfsproc3_lookup_3(&dpLookUpArgs, clntV3)) == NULL ) 
			strLastError = clnt_sperror(clntV3,"nfsproc3_lookup_3");
		else
		{
			if (pLookUpRes->status == NFS3_OK) 
			{
				memcpy(currentItem, pLookUpRes->LOOKUP3res_u.resok.obj.data.data_val, pLookUpRes->LOOKUP3res_u.resok.obj.data.data_len);
				Ret = NFS_SUCCESS;
			}
			else
			{
				Ret = NFS_ERROR;
			}
			xdr_free((xdrproc_t)xdr_LOOKUP3res,(char*) pLookUpRes);
		}

		pName = strtok(NULL, "\\");

		if(Ret == NFS_ERROR)
			break;
	}

	if(Ret != NFS_ERROR)
		memcpy(Handle, currentItem, FHSIZE);

	return Ret;
}

int CNFSv3::IsDirectory(char* Path)
{
	int Ret = NFS_ERROR;
	char *pName;
	char currentItem[FHSIZE];

	memcpy(currentItem, nfsRootDirectory, FHSIZE);
	pName = strtok(Path, "\\");
	while(pName != NULL)
	{
		LOOKUP3args dpLookUpArgs;
		LOOKUP3res *pLookUpRes;
		dpLookUpArgs.what.dir.data.data_val = currentItem;
		dpLookUpArgs.what.dir.data.data_len = FHSIZE;
		dpLookUpArgs.what.name = pName;
		
		if( (pLookUpRes = nfsproc3_lookup_3(&dpLookUpArgs, clntV3)) == NULL ) 
			strLastError = clnt_sperror(clntV3,"nfsproc3_lookup_3");
		else
		{
			if (pLookUpRes->status == NFS3_OK) 
			{
				memcpy(currentItem, pLookUpRes->LOOKUP3res_u.resok.obj.data.data_val, pLookUpRes->LOOKUP3res_u.resok.obj.data.data_len);
				if(pLookUpRes->LOOKUP3res_u.resok.obj_attributes.post_op_attr_u.attributes.type == 2)
					Ret = NFS_SUCCESS;
				else
					Ret = NFS_ERROR;
			}
			else
			{
				Ret = NFS_ERROR;
			}
			xdr_free((xdrproc_t)xdr_LOOKUP3res,(char*) pLookUpRes);
		}

		pName = strtok(NULL, "\\");

		if(Ret == NFS_ERROR)
			break;
	}

	return Ret;
}


int CNFSv3::Move(char* pOldFolder, char* pOldName, char* pNewFolder, char* pNewName)
{
	int Ret = NFS_ERROR;
	if(clntV3 != NULL)
	{
		char OldPath[FHSIZE];
		char NewPath[FHSIZE];

		if(GetItemHandle(pOldFolder, OldPath) != NFS_SUCCESS ||
		GetItemHandle(pNewFolder, NewPath) != NFS_SUCCESS)
			return Ret;

		RENAME3args dpRenameArgs;
        RENAME3res *pRenameRes;
		dpRenameArgs.from.dir.data.data_val = OldPath;
		dpRenameArgs.from.dir.data.data_len = FHSIZE;
		dpRenameArgs.to.dir.data.data_val = NewPath;
		dpRenameArgs.to.dir.data.data_len = FHSIZE;
        dpRenameArgs.from.name = pOldName;
		dpRenameArgs.to.name = pNewName;
        if( (pRenameRes = nfsproc3_rename_3(&dpRenameArgs, clntV3)) == NULL ) 
			strLastError = clnt_sperror(clntV3, "nfsproc3_rename_3");
		else
		{
			if (pRenameRes->status == NFS3_OK) 
				Ret = NFS_SUCCESS;
			else 
			{
				char  buffer[200];
				sprintf_s(buffer, 200, "nfsproc3_rename_3: %d", pRenameRes->status);
				strLastError = buffer;
			}
			xdr_free((xdrproc_t)xdr_RENAME3res,(char*) pRenameRes);
		}
	}
	else
		strLastError = "nfsproc3_rename_3: Client is NULL";

	return Ret;
}

const char* CNFSv3::GetLastNfsError()
{
	return strLastError.c_str();
}

int CNFSv3::ChangeMode(char* pName, char* pDirectory, int Mode)
{
	return NFS_ERROR;
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

int CNFSv3::ChangeOwner(char* pName, char* pDirectory, int UID, int GID)
{
	return NFS_ERROR;
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