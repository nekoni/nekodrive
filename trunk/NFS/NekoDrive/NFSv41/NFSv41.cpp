// NFSv41.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "NFSv41.h"


// This is an example of an exported variable
NFSV41_API int nNFSv41=0;

// This is an example of an exported function.
NFSV41_API int fnNFSv41(void)
{
	return 42;
}

// This is the constructor of a class that has been exported.
// see NFSv41.h for the class definition
CNFSv41::CNFSv41()
{
	return;
}
