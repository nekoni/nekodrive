// TestMountV2.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "NFSv2.h"

int _tmain(int argc, _TCHAR* argv[])
{
	CNFSv2* nfs = new CNFSv2();
	unsigned long ServerAddress = inet_addr("192.168.56.102");
	nfs->Connect(ServerAddress);
	int iDevices = 0;
	char** pDevices  = nfs->GetExportedDevices(&iDevices);
	nfs->ReleaseBuffers(pDevices);
	nfs->MountDevice("/rd0");
	int iItems = 0;
	nfs->GetItemsList(&iItems);
	nfs->GetItemAttributes("libatm.so.1.0.0");
	nfs->Open("new file");
	char pBuffer[4096];
	u_long pSize;
	int ret = nfs->Read(0, 4096, pBuffer, &pSize);
	nfs->CreateFile("test.t");
	nfs->UnMountDevice();
	delete nfs;
	
	return 0;

}

