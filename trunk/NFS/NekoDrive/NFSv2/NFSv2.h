/*
 * NekoDrive
 * 2010 by Mirko Gatto
 * mirko.gatto@gmail.com
 *
 *
 * Users may use, copy or modify this library 
 * according GNU General Public License v3 (http://www.gnu.org/licenses/gpl.html)
 */

#ifndef __IncNFSV2h
#define __IncNFSV2h

#pragma pack(1) 

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

#define MAX_HOST_NAME 255
#define MAX_FILE_NAME 255
#define NFS_SUCCESS 0
#define NFS_ERROR -1

struct NFSData
{
	unsigned long DateTime;
	unsigned int Type;
	unsigned int Size;
	unsigned int Blocks;
	unsigned int BlockSize;
	char Handle[FHSIZE];
};


struct StringVector
{
	std::vector<std::string> Str;
};


class NFSV2_API CNFSv2
{
private:
	std::string strServer;
	CLIENT* clntMountV2;
	CLIENT* clntV2;
	SOCKET sSocket;
	struct sockaddr_in sSrvAddr;
	struct timeval timeOut;
	std::string strCurrentDevice;
	std::string strLastError;
	nfshandle nfsRootDirectory;
	nfshandle nfsCurrentDirectory;
	nfshandle nfsCurrentFile;
	int authType;
	int uid;
	int gid;
	void InitStructure(NFSData* pNfsData);
	int CheckOpenHandle();
	int CreateAuthentication();
public:
	StringVector v;
	CNFSv2();
	~CNFSv2();
	int Connect(const char* ServerAddress, int UserId, int GroupId, long CommandTimeout);
	int Disconnect();
	char** GetExportedDevices(int* pnSize);
	char** GetItemsList(int* pnSize);
	void ReleaseBuffers(void** pBuffers);
	void ReleaseBuffer(void* pBuffer);
	int MountDevice(char* pDevice);
	int UnMountDevice();
	void* GetItemAttributes(char* pName);
	int ChangeCurrentDirectory(char* pName);
	int CreateDirectory(char* pName);
	int DeleteDirectory(char* pName);
	int DeleteFile(char* pName);
	int CreateFile(char* pName);
	int Open(char* pName);
	void CloseFile();
	int Read(u_int Offset, u_int Count, char* pBuffer, u_long* pSize);
	int Write(u_int Offset, u_int Count, char* pBuffer, u_long* pSize);
	int Rename(char* pOldName, char* pNewName);
	int Move(char* pOldFolder, char* pOldName, char* pNewFolder, char* pNewName);
	const char* GetLastNfsError();
	int ChangeMode(char* pName, int Mode);
	int ChangeOwner(char* pName, int UID, int GUID);
};

NFSV2_API CNFSv2* CreateCNFSv2();

NFSV2_API void DisposeCNFSv2(CNFSv2* pObject);

#endif
