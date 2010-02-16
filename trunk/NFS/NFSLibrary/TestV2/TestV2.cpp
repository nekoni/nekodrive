// TestMountV2.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#ifdef NFSV2
#include "NFSv2.h"
#else
#include "NFSv3.h"
#endif
#include <string>
#include <vector>

#ifdef NFSV2

int writeFile(const char* File, CNFSv2* nfs)
{
	FILE *fp = NULL;
	std::string stPath = "e:\\Test\\";
	char OutputFile[80];
	memset(OutputFile, 0, 80);
	strcat(OutputFile, stPath.c_str());
	strcat(OutputFile, File);
	u_int offset = 0;
	u_long Size = 0;
	char buf[4096];
	int n = 0;

	if(nfs->CreateFile((char*)File) == NFS_SUCCESS)
	{
		if(nfs->Open((char*)File) == NFS_SUCCESS)
		{ 
			if ((fp = fopen(OutputFile, "rb")) != NULL) 
			{
				for (offset = 0; (n = fread(buf, 1, sizeof(buf), fp)) > 0; offset += n) 
				{
					if(nfs->Write(offset, n, buf, &Size) != NFS_SUCCESS)
						break;
					printf(".");
				}			
				fclose(fp);
			}
			nfs->CloseFile();
			printf("\n");
		}
	}

	remove(OutputFile);

	return 0;
}

int saveFile(const char* File, CNFSv2* nfs, unsigned int totsize, unsigned int blocksize)
{
	long offset;
	int size = 0;
	FILE *fp = NULL;

	std::string stPath = "e:\\Test\\";
	char OutputFile[80];
	memset(OutputFile, 0, 80);
	strcat(OutputFile, stPath.c_str());
	strcat(OutputFile, File);
	if((fp = fopen(OutputFile, "wb")) != NULL)
	{
		nfs->Open((char*) File);
		char* buffer = new char[blocksize];
		for(offset = 0; offset < totsize; )
		{
			memset(buffer, 0, blocksize);
			if(nfs->Read(offset, blocksize, buffer, (u_long*) &size) == NFS_SUCCESS)
			{
				fwrite(buffer, size, 1, fp);
				printf("#");
				fflush(fp);
				offset += size;
			}
			else
			{
				printf(nfs->GetLastNfsError());
				break;
			}
		}
		delete buffer;
		nfs->CloseFile();
		printf("\n");
		fclose(fp);
	}
	
	return 0;
}


int _tmain(int argc, _TCHAR* argv[])
{
	for (int x = 0; x < 1000; x++)
	{
		std::vector<std::string> strD;
		std::vector<std::string> strI;
		CNFSv2* nfs = new CNFSv2();
		unsigned long ServerAddress = inet_addr("192.168.56.3");
		nfs->Connect(ServerAddress, 0, 0);
		
		int iDevices = 0;
		char** pDevices  = nfs->GetExportedDevices(&iDevices);
		int i = 0;
		for(char** iList = pDevices; i < iDevices; ++iList, i++)
		{
			strD.push_back(*iList);
		}
		nfs->ReleaseBuffers((void**)pDevices);
		nfs->MountDevice((char*)strD[0].c_str());
		int iItems = 0;
		nfs->ChangeCurrentDirectory("test");
		char** pItems = nfs->GetItemsList(&iItems);
		i = 0;
		for(char** iList = pItems; i < iItems; ++iList, i++)
		{
			strI.push_back(*iList);
			NFSData* nfsData = (NFSData*) nfs->GetItemAttributes((char*)strI[i].c_str());
			printf("%s %d %d %d\n", strI[i].c_str(), nfsData->Blocks, nfsData->BlockSize, nfsData->Size);
			saveFile(strI[i].c_str(), nfs, nfsData->Size, nfsData->BlockSize);
			writeFile(strI[i].c_str(), nfs);
			nfs->ReleaseBuffer((void*)nfsData);
		}
		nfs->ReleaseBuffers((void**)pItems);
		nfs->UnMountDevice();
		nfs->Disconnect();

		strD.clear();
		strI.clear();

		delete nfs;
	}
	
	Sleep(60000);

	return 0;

}

#else

int writeFile(const char* File, CNFSv3* nfs)
{
	FILE *fp = NULL;
	std::string stPath = "d:\\Test\\";
	char OutputFile[80];
	memset(OutputFile, 0, 80);
	strcat(OutputFile, stPath.c_str());
	strcat(OutputFile, File);
	u_int offset = 0;
	u_int Size = 0;
	char buf[4096];
	int n = 0;

	if(nfs->DeleteFile((char*)File) == NFS_SUCCESS)
	{
		if(nfs->CreateFile((char*)File) == NFS_SUCCESS)
		{
			if(nfs->Open((char*)File) == NFS_SUCCESS)
			{ 
				if ((fp = fopen(OutputFile, "rb")) != NULL) 
				{
					for (offset = 0; (n = fread(buf, 1, sizeof(buf), fp)) > 0; offset += n) 
					{
						if(nfs->Write(offset, n, buf, &Size) != NFS_SUCCESS)
							break;
						printf(".");
					}			
					fclose(fp);
				}
				nfs->CloseFile();
				printf("\n");
			}
		}
		else
			printf("%s\n", nfs->GetLastNfsError());
	}

	remove(OutputFile);

	return 0;
}

int saveFile(const char* File, CNFSv3* nfs, unsigned __int64 totsize, unsigned int blocksize)
{
	u_int offset = 0;
	u_int size = 0;
	FILE *fp = NULL;

	std::string stPath = "d:\\Test\\";
	char OutputFile[80];
	memset(OutputFile, 0, 80);
	strcat(OutputFile, stPath.c_str());
	strcat(OutputFile, File);
	if((fp = fopen(OutputFile, "wb")) != NULL)
	{
		nfs->Open((char*) File);
		char* buffer = new char[blocksize];
		for(offset = 0; offset < totsize; )
		{
			memset(buffer, 0, blocksize);
			if(nfs->Read(offset, blocksize, buffer, &size) == NFS_SUCCESS)
			{
				fwrite(buffer, size, 1, fp);
				printf("#");
				fflush(fp);
				offset += size;
			}
			else
			{
				printf(nfs->GetLastNfsError());
				break;
			}
		}
		delete buffer;
		nfs->CloseFile();
		printf("\n");
		fclose(fp);
	}
	
	return 0;
}


int _tmain(int argc, _TCHAR* argv[])
{
	for (int x = 0; x < 1000; x++)
	{
		std::vector<std::string> strD;
		std::vector<std::string> strI;
		CNFSv3* nfs = new CNFSv3();
		unsigned long ServerAddress = inet_addr("192.168.56.3");//inet_addr("192.168.56.3");
		nfs->Connect(ServerAddress, 0, 0);
		int iDevices = 0;
		char** pDevices  = nfs->GetExportedDevices(&iDevices);
		int i = 0;
		for(char** iList = pDevices; i < iDevices; ++iList, i++)
		{
			strD.push_back(*iList);
		}
		nfs->ReleaseBuffers((void**)pDevices);
		nfs->MountDevice((char*)strD[0].c_str());
		
		int iItems = 0;
		char** pItems = nfs->GetItemsList(&iItems);
		i = 0;
		for(char** iList = pItems; i < iItems; ++iList, i++)
		{
			strI.push_back(*iList);
			NFSData* nfsData = (NFSData*) nfs->GetItemAttributes((char*)strI[i].c_str());
			printf("%s %d\n", strI[i].c_str(), nfsData->Size);
			saveFile(strI[i].c_str(), nfs, nfsData->Size, 8192);
			writeFile(strI[i].c_str(), nfs);
			nfs->ReleaseBuffer((void*)nfsData);
		}
		nfs->ReleaseBuffers((void**)pItems);
		nfs->UnMountDevice();
		nfs->Disconnect();
		delete nfs;
	}
	Sleep(60000);
	return 0;
}

#endif

