#ifndef __IncNFSV2h
#define __IncNFSV2h

#ifdef NFSV2_EXPORTS
#define NFSV2_API __declspec(dllexport)
#else
#define NFSV2_API __declspec(dllimport)
#endif

#include <iostream>
#include <vector>
#include <string>
#include <rpc/rpc.h>
#include "NFSv2Protocol.h"

#define MAX_FILE_NAME 255
#define NFS_SUCCESS 0
#define NFS_ERROR -1

struct NFSFileData
{
	std::string FileName;
	unsigned long DateTime;
	unsigned int Type;
	unsigned int Size;
	unsigned int Blocks;
	unsigned int BlockSize;
	char Handle[FHSIZE];
	NFSFileData* Next;
};


class NFSV2_API CNFSv2
{
private:
	unsigned int uiServer;
	std::string strServer;
	CLIENT* clntMountV2;
	CLIENT* clntV2;
	SOCKET sSocket;
	struct sockaddr_in sSrvAddr;
	struct timeval timeOut;
	std::string strCurrentDevice;
	std::string strLastError;
	nfshandle      nfsCurrentDirectory;
	//functions
	NFSFileData InitStructure();
public:
	CNFSv2();
	~CNFSv2();
	//Connect to a server
	int Connect(unsigned int ServerAddress);
	//Disconnect from a server
	int Disconnect();
	//Get the exported devices
	char** GetExportedDevices(int* pnSize);
	//Release the string buffers
	void ReleaseExportedDevices(char** pDevices);
	//Mount the remote device
	int MountDevice(char* pDevice);
	//Unmount the remote device
	int UnMountDevice();
	//Get the files list
	int GetFilesLit();
};

#endif
