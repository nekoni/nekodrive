// TestMountV2.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "NFSv2.h"
#include <string>
#include <vector>

int writeFile(const char* File, CNFSv2* nfs)
{

	//std::string stPath = "D:\\SOURCE\\SOURCE_CODE\\nekodrive\\NFS\\Build\\x86\\Debug\\Test\\";
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
	if((fp = fopen(OutputFile, "w")) != NULL)
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
	remove(OutputFile);
	
	return 0;
}



int _tmain(int argc, _TCHAR* argv[])
{
	std::vector<std::string> strD;
	std::vector<std::string> strI;
	CNFSv2* nfs = new CNFSv2();
	//unsigned long ServerAddress = inet_addr("161.55.201.250");
	unsigned long ServerAddress = inet_addr("192.168.56.102");
	nfs->Connect(ServerAddress, 0, 0);
	for (int x = 0; x < 100; x++)
	{
		strD.clear();
		strI.clear();
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
			printf("%s %d %d %d\n", strI[i].c_str(), nfsData->Blocks, nfsData->BlockSize, nfsData->Size);
			saveFile(strI[i].c_str(), nfs, nfsData->Size, nfsData->BlockSize);
			writeFile(strI[i].c_str(), nfs);
			nfs->ReleaseBuffer((void*)nfsData);
		}
		nfs->ReleaseBuffers((void**)pItems);
		nfs->UnMountDevice();
	}
	
	//nfs->CreateFile("logo_good.gif");
	//nfs->Open("logo_good.gif");
	//char pBuffer[4096];
	//u_long pSize;
	////int ret = nfs->Read(0, 4096, pBuffer, &pSize);
	//int ret = nfs->Write(0, 1024, pBuffer, &pSize);
	//nfs->CreateFile("test.t");
	delete nfs;
	
	Sleep(60000);

	return 0;

}

