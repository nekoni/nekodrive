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
}

int CNFSv2::Connect(unsigned int ServerAddress)
{
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
		printf(strLastError.c_str());
		return NFS_ERROR;
	}
	u_short uPort;
	sSrvAddr.sin_addr.s_addr = ServerAddress;
	strServer.assign(inet_ntoa(sSrvAddr.sin_addr));
	if( (uPort = pmap_getport(&sSrvAddr, MOUNTPROG, MOUNTVERS, IPPROTO_UDP)) == 0)
	{
		strLastError = clnt_spcreateerror((char*)strServer.c_str());
		return NFS_ERROR;
	}
	sSrvAddr.sin_port = htons(uPort);
	if ((clntMountV2 = clntudp_create(&sSrvAddr, MOUNTPROG, MOUNTVERS, timeOut, (int*)&sSocket)) == NULL) 
	{
  		 strLastError = clnt_spcreateerror((char*)strServer.c_str());
		 printf(strLastError.c_str());
         return NFS_ERROR;
	}
	if( (uPort = pmap_getport(&sSrvAddr, NFS_PROGRAM, NFS_VERSION, IPPROTO_UDP)) == 0 ) 
	{
		strLastError = clnt_spcreateerror((char*)strServer.c_str());
		printf(strLastError.c_str());
        return NFS_ERROR;
	}
	sSrvAddr.sin_port = htons(uPort);
	sSocket = RPC_ANYSOCK;
	if ((clntV2 = clntudp_create(&sSrvAddr, NFS_PROGRAM, NFS_VERSION, timeOut, (int*)&sSocket)) == NULL) 
	{
  		 strLastError = clnt_spcreateerror((char*)strServer.c_str());
		 printf(strLastError.c_str());
         return NFS_ERROR;
	}

	return NFS_SUCCESS;
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
	if(clntMountV2 != NULL)
	{	
		if(UnMountDevice() == NFS_SUCCESS)
		{
			if(clntMountV2 != NULL)
				nfs_clnt_destroy(clntMountV2);

			clntMountV2 = NULL;
			
			if(clntV2 != NULL)
				nfs_clnt_destroy(clntV2);

			clntV2 = NULL;
		}
		else
			return NFS_ERROR;
	}

	WSACleanup();

	return NFS_SUCCESS;
}

int CNFSv2::UnMountDevice()
{
	if(clntMountV2 != NULL)
	{
		if(strCurrentDevice != "")
		{
			dirpath dpArgs;
			dpArgs = (dirpath) strCurrentDevice.c_str();
			if( mountproc_umnt_1(&dpArgs, clntMountV2) == NULL ) 
			{
				strLastError = clnt_sperror(clntMountV2, "mountproc_umnt_1");
				printf(strLastError.c_str());
				return NFS_ERROR;
			}
			strCurrentDevice = "";
			return NFS_SUCCESS;
		}
		else
			return NFS_SUCCESS;
	}
	return NFS_ERROR;
}

int CNFSv2::MountDevice(char* pDevice)
{
	if(pDevice != NULL)
	{
		if(clntMountV2 != NULL)
		{
			dirpath dpArgs = NULL;
			fhstatus* pFhStatus = NULL;
			dpArgs = pDevice;

			if( (pFhStatus = mountproc_mnt_1(&dpArgs, clntMountV2)) == NULL ) 
			{
				strLastError = clnt_sperror(clntMountV2, "mountproc_mnt_1");
				printf(strLastError.c_str());
				return NFS_ERROR;
			}
			if (pFhStatus->status == (u_int) NFS_OK ) 
			{
				memcpy(nfsCurrentDirectory, pFhStatus->fhstatus_u.directory, FHSIZE);
				strCurrentDevice = pDevice;
				xdr_free((xdrproc_t)xdr_fhstatus,(char*) pFhStatus);
				return NFS_SUCCESS;
			} 
			else 
			{
				char  buffer[200];
				sprintf_s(buffer, 200, "mountproc_mnt_1 %d", pFhStatus->status);
				strLastError = buffer;
				printf(strLastError.c_str());
				xdr_free((xdrproc_t)xdr_fhstatus,(char*) pFhStatus);
				return NFS_ERROR;
			}
		}
	}

	return NFS_ERROR;
}

char** CNFSv2::GetExportedDevices(int* pnSize)
{
	std::vector<std::string> vstrDevices;
	vstrDevices.clear();

	if(clntMountV2 == NULL)
	{
		strLastError = "Mount Client is NULL";
		printf(strLastError.c_str());
		return NULL;
	}

	exports *device;
	exports tmp;

	if( (device = mountproc_export_1(NULL, clntMountV2) ) == NULL )
	{
		strLastError = clnt_sperror(clntMountV2, "mountproc_export_1");
		printf(strLastError.c_str());
		return NULL;
	}
	tmp = *device;
	
	while(tmp != NULL)
	{
		vstrDevices.push_back(tmp->filesys);
		tmp = tmp->next;
	}
	
	xdr_free((xdrproc_t)xdr_exports,(char*) device);

	int nSize = (int) vstrDevices.size();
	char** strings = new char*[nSize];

	for( int i = 0; i < nSize; i++)
	{
		strings[i] = (char*) vstrDevices[i].c_str();
	}

	*pnSize = nSize;
	return  strings;
}

char** CNFSv2::GetItemsList(int* pnSize)
{
	std::vector<std::string> vstrItems;
	entry* pEntry = NULL;

	if(clntV2 != NULL)
	{
		readdirargs dpRdArgs;
        readdirres *pReadDirRes;
		
		dpRdArgs.cookie = 0;
		memset(dpRdArgs.dir, 0, FHSIZE);
		memcpy(dpRdArgs.dir, nfsCurrentDirectory, FHSIZE);
		dpRdArgs.count = 512;

		while(true)
		{
			if( (pReadDirRes = nfsproc_readdir_2(&dpRdArgs, clntV2)) == NULL ) 
			{
				strLastError = clnt_sperror(clntV2, "nfsproc_readdir_2");
				printf(strLastError.c_str());
				return NULL;
			}
			if(pReadDirRes->status == NFS_OK)
			{
				pEntry = pReadDirRes->readdirres_u.ok.entries;
				while(pEntry != NULL)
				{
					vstrItems.push_back(pEntry->name);
					dpRdArgs.cookie = pEntry->cookie;
					pEntry = pEntry->nextentry;
				}
			}
			else
			{
				char  buffer[200];
				sprintf_s(buffer, 200, "nfsproc_readdir_2 %d", pReadDirRes->status);
				strLastError = buffer;
				printf(strLastError.c_str());
				return NULL;
			}
			
			int iBreak = pReadDirRes->readdirres_u.ok.eof;
			xdr_free((xdrproc_t)xdr_readdirres,(char*) pReadDirRes);

			if(iBreak)
				break;
		}

		int nSize = (int) vstrItems.size();
		char** strings = new char*[nSize];

		for( int i = 0; i < nSize; i++)
		{
			strings[i] = (char*) vstrItems[i].c_str();
		}

		*pnSize = nSize;
		return  strings;
	}
	else
	{
		strLastError = "V2 Client is NULL";
		printf(strLastError.c_str());
	}

	return NULL;
}

void CNFSv2::ReleaseBuffer(char** pBuffer)
{
	if(pBuffer != NULL)
		delete[] pBuffer;
}

void* CNFSv2::GetItemAttributes(char* pItem)
{
	if(clntV2 != NULL)
	{
		diropargs dpDrArgs;
		diropres *pDirOpRes;
		memcpy(dpDrArgs.dir, nfsCurrentDirectory, FHSIZE);
		dpDrArgs.name = pItem;
		if( (pDirOpRes = nfsproc_lookup_2(&dpDrArgs, clntV2)) == NULL ) 
		{
			strLastError = clnt_sperror(clntV2,"nfsproc_lookup_2");
			printf(strLastError.c_str());
			return NULL;
		}
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
			xdr_free((xdrproc_t)xdr_diropres,(char*) pDirOpRes);
			return (void*) pNfsData;	
		}
		else
		{
			char  buffer[200];
            sprintf_s(buffer, 200, "nfsproc_lookup_2 %d", pDirOpRes->status);
			strLastError = buffer;
			printf(strLastError.c_str());
			xdr_free((xdrproc_t)xdr_diropres,(char*) pDirOpRes);
            return NULL;
		}
	}
	else
	{
		strLastError = "V2 Client is NULL";
		printf(strLastError.c_str());
	}

	return NULL;
}

void CNFSv2::ReleaseBuffer(char* pBuffer)
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

void CNFSv2::ChangeCurrentDirectory(char *pHandle)
{
	if(pHandle != NULL)
		memcpy(nfsCurrentDirectory, pHandle, FHSIZE);
}

int CNFSv2::CreateDirectory(char* pName)
{
	if(clntV2 == NULL)
	{
		createargs dpArgCreate;
        diropres *pDirOpRes;
        nfstimeval createTime;
		time_t now;
        time(&now);
		createTime.seconds = (u_int) now;
		dpArgCreate.attributes.atime = createTime;
        dpArgCreate.attributes.mtime = createTime;
		dpArgCreate.attributes.mode = MODE_DIR | 0777;
		memcpy(dpArgCreate.where.dir, nfsCurrentDirectory, FHSIZE);
        dpArgCreate.where.name = pName;
        
        if( (pDirOpRes = nfsproc_mkdir_2(&dpArgCreate, clntV2)) == NULL ) 
		{
			strLastError = clnt_sperror(clntV2, "nfsproc_mkdir_2");
			return(NFS_ERROR);
        }
        if (pDirOpRes->status == NFS_OK) 
		{
			xdr_free((xdrproc_t)xdr_diropres,(char*) pDirOpRes);
            return(NFS_SUCCESS);
        } 
		else 
		{
			char  buffer[200];
            sprintf_s(buffer, 200, "nfsproc_mkdir_2 %d", pDirOpRes->status);
			strLastError = buffer;
			xdr_free((xdrproc_t)xdr_diropres,(char*) pDirOpRes);
            return(NFS_ERROR);
        }
	}
	else
		strLastError = "V2 Client is NULL";

	return NFS_ERROR;
}

int CNFSv2::DeleteDirectory(char* pName)
{
	if(clntV2 == NULL)
	{
		diropargs dpArgDelete;
        nfsstat *pNfsStat;
		memcpy(dpArgDelete.dir, nfsCurrentDirectory, FHSIZE);
        dpArgDelete.name = pName;
        
        if( (pNfsStat = nfsproc_rmdir_2(&dpArgDelete, clntV2)) == NULL ) 
		{
			strLastError = clnt_sperror(clntV2, "nfsproc_rmdir_2");
			return NFS_ERROR;
        }
        if (*pNfsStat == NFS_OK) 
		{
			xdr_free((xdrproc_t)xdr_nfsstat,(char*) pNfsStat);
            return NFS_SUCCESS;
        } 
		else 
		{
			char  buffer[200];
            sprintf_s(buffer, 200, "nfsproc_rmdir_2 %d", pNfsStat);
			strLastError = buffer;
			xdr_free((xdrproc_t)xdr_nfsstat,(char*) pNfsStat);
            return NFS_ERROR;
        }
	}
	else
		strLastError = "V2 Client is NULL";

	return NFS_ERROR;
}

int CNFSv2::DeleteFile(char* pName)
{
	if(clntV2 == NULL)
	{
		diropargs dpArgDelete;
        nfsstat *pNfsStat;
		memcpy(dpArgDelete.dir, nfsCurrentDirectory, FHSIZE);
        dpArgDelete.name = pName;
        
        if( (pNfsStat = nfsproc_remove_2(&dpArgDelete, clntV2)) == NULL ) 
		{
			strLastError = clnt_sperror(clntV2, "nfsproc_remove_2");
			return NFS_ERROR;
        }
        if (*pNfsStat == NFS_OK) 
		{
			xdr_free((xdrproc_t)xdr_nfsstat,(char*) pNfsStat);
            return NFS_SUCCESS;
        } 
		else 
		{
			char  buffer[200];
            sprintf_s(buffer, 200, "nfsproc_remove_2 %d", pNfsStat);
			strLastError = buffer;
			xdr_free((xdrproc_t)xdr_nfsstat,(char*) pNfsStat);
            return NFS_ERROR;
        }
	}
	else
		strLastError = "V2 Client is NULL";

	return NFS_ERROR;
}

int CNFSv2::CreateFile(char* pName)
{
	if(clntV2 == NULL)
	{
		createargs dpArgCreate;
        diropres *pDirOpRes;
        nfstimeval createTime;
		time_t now;
        time(&now);
		createTime.seconds = (u_int) now;
		dpArgCreate.attributes.atime = createTime;
        dpArgCreate.attributes.mtime = createTime;
		dpArgCreate.attributes.mode = MODE_REG | 0777;
		memcpy(dpArgCreate.where.dir, nfsCurrentDirectory, FHSIZE);
        dpArgCreate.where.name = pName;
        
        if( (pDirOpRes = nfsproc_create_2(&dpArgCreate, clntV2)) == NULL ) 
		{
			strLastError = clnt_sperror(clntV2, "nfsproc_create_2");
			return NFS_ERROR;
        }
        if (pDirOpRes->status == NFS_OK) 
		{
			xdr_free((xdrproc_t)xdr_diropres,(char*) pDirOpRes);
            return NFS_SUCCESS;
        } 
		else 
		{
			char  buffer[200];
            sprintf_s(buffer, 200, "nfsproc_create_2 %d", pDirOpRes->status);
			strLastError = buffer;
			xdr_free((xdrproc_t)xdr_diropres,(char*) pDirOpRes);
            return NFS_ERROR;
        }
	}
	else
		strLastError = "V2 Client is NULL";

	return NFS_ERROR;
}

int CNFSv2::Read(char* pHandle, u_int Offset, u_int Count, char* pBuffer, u_long* pSize)
{
	if(clntV2 == NULL)
	{
		readargs dpArgRead;
		readres *pReadRes;
		memcpy(dpArgRead.file, pHandle, FHSIZE);
        dpArgRead.offset = Offset;
        dpArgRead.count = Count;

		if( (pReadRes = nfsproc_read_2(&dpArgRead, clntV2)) == NULL ) 
		{
			strLastError = clnt_sperror(clntV2, "nfsproc_read_2");
			return NFS_ERROR;
        }
		if (pReadRes->status == NFS_OK) 
		{
            *pSize = pReadRes->readres_u.ok.data.data_len;
            memcpy(pBuffer, pReadRes->readres_u.ok.data.data_val, pReadRes->readres_u.ok.data.data_len);
            return NFS_SUCCESS;
        } 
		else 
		{
			char  buffer[200];
            sprintf_s(buffer, 200, "nfsproc_read_2 %d", pReadRes->status);
			strLastError = buffer;
            return NFS_ERROR;
        }
		xdr_free((xdrproc_t)xdr_readres,(char*) pReadRes);
	}
	else
		strLastError = "V2 Client is NULL";

	return NFS_ERROR;
}

int CNFSv2::Write(char* pHandle, u_int Offset, u_int Count, char* pBuffer, u_long* pSize)
{
	if(clntV2 == NULL)
	{
		writeargs dpArgWrite;
        attrstat *pAttrStat;
		memcpy(dpArgWrite.file, pHandle, FHSIZE);
        dpArgWrite.offset = Offset;
        dpArgWrite.data.data_len = Count;
		dpArgWrite.data.data_val = pBuffer;

        if( (pAttrStat = nfsproc_write_2(&dpArgWrite, clntV2)) == NULL )
        {
			strLastError = clnt_sperror(clntV2, "nfsproc_write_2");
			return NFS_ERROR;
        }
        if (pAttrStat->status == NFS_OK) 
		{
            *pSize = pAttrStat->attrstat_u.attributes.size;
			xdr_free((xdrproc_t)xdr_attrstat,(char*) pAttrStat);
            return NFS_SUCCESS;
        } 
		else 
		{
            char  buffer[200];
            sprintf_s(buffer, 200, "nfsproc_write_2 %d", pAttrStat->status);
			strLastError = buffer;
			xdr_free((xdrproc_t)xdr_attrstat,(char*) pAttrStat);
            return NFS_ERROR;
        }
	}
	else
		strLastError = "V2 Client is NULL";

	return NFS_ERROR;
}

int CNFSv2::Rename(char* pOldName, char* pNewName)
{
	if(clntV2 == NULL)
	{
		renameargs dpArgRename;
        nfsstat *pNfsStat;
		memcpy(dpArgRename.from.dir, nfsCurrentDirectory, FHSIZE);
		memcpy(dpArgRename.to.dir, nfsCurrentDirectory, FHSIZE);
        dpArgRename.from.name = pOldName;
		dpArgRename.to.name = pNewName;

        if( (pNfsStat = nfsproc_rename_2(&dpArgRename, clntV2)) == NULL ) 
		{
			strLastError = clnt_sperror(clntV2, "nfsproc_rename_2");
			return(NFS_ERROR);
        }
        if (*pNfsStat == NFS_OK) 
		{
			xdr_free((xdrproc_t)xdr_nfsstat,(char*) pNfsStat);
            return(NFS_SUCCESS);
        } 
		else 
		{
			char  buffer[200];
            sprintf_s(buffer, 200, "nfsproc_rename_2 %d", pNfsStat);
			strLastError = buffer;
			xdr_free((xdrproc_t)xdr_nfsstat,(char*) pNfsStat);
            return(NFS_ERROR);
        }
	}
	else
		strLastError = "V2 Client is NULL";

	return NFS_ERROR;
}