// NFSv2Mount.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "NFSv2.h"
#include "NFSv2MountProtocol.h"
#include "NFSv2Protocol.h"
#include "rpc/Pmap_cln.h"
#include <stdio.h>
#include <vector>
#include <string>


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
	//WINSOCK Initialization
	WORD wVersionRequested;
	WSADATA wsaData;
	int iWSAErr;
	wVersionRequested = MAKEWORD( 2, 2 );
	iWSAErr = WSAStartup( wVersionRequested, &wsaData );
	if ( iWSAErr != 0 ) {
		printf("Error on WSAStartup %d\n", iWSAErr);
		return NFS_ERROR;
	}
	//set up address and mount client
	u_short uPort;
	//set the address
	sSrvAddr.sin_addr.s_addr = ServerAddress;
	//get the server address xxx.xxx.xxx.xxx
	strServer.assign(inet_ntoa(sSrvAddr.sin_addr));
	//get the port number
	if( (uPort = pmap_getport(&sSrvAddr, MOUNTPROG, MOUNTVERS, IPPROTO_UDP)) == 0)
	{
		printf((strLastError = clnt_spcreateerror((char*)strServer.c_str())).c_str());
		return NFS_ERROR;
	}
	sSrvAddr.sin_port = htons(uPort);
	//create the mount client
	if ((clntMountV2 = clntudp_create(&sSrvAddr, MOUNTPROG, MOUNTVERS, timeOut, (int*)&sSocket)) == NULL) 
	{
  		 printf(clnt_spcreateerror((char*)strServer.c_str()));
         return NFS_ERROR;
	}
	//get the nfs v2 port
	if( (uPort = pmap_getport(&sSrvAddr, NFS_PROGRAM, NFS_VERSION, IPPROTO_UDP)) == 0 ) 
	{
		printf(clnt_spcreateerror((char*)strServer.c_str()));
        return NFS_ERROR;
	}
	sSrvAddr.sin_port = htons(uPort);
	sSocket = RPC_ANYSOCK;
	//create the nfs v2 client
	if ((clntV2 = clntudp_create(&sSrvAddr, NFS_PROGRAM, NFS_VERSION, timeOut, (int*)&sSocket)) == NULL) 
	{
  		 printf(clnt_spcreateerror((char*)strServer.c_str()));
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

	//this close also the opened socket
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
				printf(clnt_sperror(clntMountV2, "mountproc_umnt_1"));
				return(NFS_ERROR);
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
			//parameters
			dirpath dpArgs = NULL;
			fhstatus* pFhStatus = NULL;

			//set the device
			dpArgs = pDevice;

			//call the mount procedure
			if( (pFhStatus = mountproc_mnt_1(&dpArgs, clntMountV2)) == NULL ) 
			{
				printf(clnt_sperror(clntMountV2, "mountproc_mnt_1"));
				return(NFS_ERROR);
			}

			//check the status and save the current directory pointer
			if (pFhStatus->status == (u_int) NFS_OK ) 
			{
				memcpy(nfsCurrentDirectory, pFhStatus->fhstatus_u.directory, FHSIZE);
				strCurrentDevice = pDevice;
				return(NFS_SUCCESS);
			} 
			else 
			{
				printf("mountproc_mnt_1:  %d", (nfsstat)pFhStatus->status);
				return(NFS_ERROR);
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
		printf("Mount Client is NULL");
		return NULL;
	}

	//the connection is working.. 
	//now I get the exported devices
	exports *device;
	exports tmp;

	if( (device = mountproc_export_1(NULL, clntMountV2) ) == NULL )
	{
		printf(clnt_sperror(clntMountV2, "mountproc_export_1"));
		return NULL;
	}
	tmp = *device;
	
	//now I fill my structures
	while(tmp != NULL)
	{
		vstrDevices.push_back(tmp->filesys);
		tmp = tmp->next;
	}
	
	//fill the output 
	int nSize = (int) vstrDevices.size();
	char** strings = new char*[nSize];

	for( int i = 0; i < nSize; i++)
	{
		strings[i] = (char*) vstrDevices[i].c_str();
	}

	*pnSize = nSize;
	return  strings;
}


void CNFSv2::ReleaseExportedDevices(char** pDevices)
{
	if(pDevices != NULL)
		delete[] pDevices;
}

int CNFSv2::GetFilesLit()
{
	std::vector<NFSFileData> vdevFiles;
	vdevFiles.clear();
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
				printf(clnt_sperror(clntV2, "nfsproc_readdir_2"));
				return(NFS_ERROR);
			}
			
			if(pReadDirRes->status == NFS_OK)
			{
				pEntry = pReadDirRes->readdirres_u.ok.entries;
				while(pEntry != NULL)
				{
					NFSFileData nfsData = InitStructure();
					nfsData.FileName = pEntry->name;
					
					//get file statistics
					diropargs dpDrArgs;
					diropres *pDirOpRes;
					memcpy(dpDrArgs.dir, nfsCurrentDirectory, FHSIZE);
					dpDrArgs.name = pEntry->name;
					if( (pDirOpRes = nfsproc_lookup_2(&dpDrArgs, clntV2)) == NULL ) 
					{
						printf(clnt_sperror(clntV2,"nfsproc_lookup_2"));
						return(NFS_ERROR);
					}
					//check the result
					if (pDirOpRes->status == NFS_OK) 
					{
						memcpy(nfsData.Handle, pDirOpRes->diropres_u.ok.file, FHSIZE);
						nfsData.DateTime = pDirOpRes->diropres_u.ok.attributes.ctime.seconds;
						nfsData.Type = pDirOpRes->diropres_u.ok.attributes.type;
						nfsData.Size = pDirOpRes->diropres_u.ok.attributes.size;
						nfsData.Blocks = pDirOpRes->diropres_u.ok.attributes.blocks;
						nfsData.BlockSize = pDirOpRes->diropres_u.ok.attributes.blocksize;
						vdevFiles.push_back(nfsData);
						dpRdArgs.cookie = pEntry->cookie;
						pEntry = pEntry->nextentry;
					}
				}
			}
			else
				return (NFS_ERROR);

			if(pReadDirRes->readdirres_u.ok.eof)
				break;
		}
		return(NFS_SUCCESS);
	}
	return(NFS_ERROR);
}

NFSFileData CNFSv2::InitStructure()
{
	NFSFileData nfsData;
	memset(nfsData.Handle, 0, FHSIZE);
	nfsData.DateTime = 0;
	nfsData.FileName = "";
	nfsData.Next = NULL;
	nfsData.Type = 0;
	nfsData.Blocks = 0;
	nfsData.DateTime = 0;
	nfsData.BlockSize = 0;

	return nfsData;
}