// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the NFSV41_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// NFSV41_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef NFSV41_EXPORTS
#define NFSV41_API __declspec(dllexport)
#else
#define NFSV41_API __declspec(dllimport)
#endif

// This class is exported from the NFSv41.dll
class NFSV41_API CNFSv41 {
public:
	CNFSv41(void);
	// TODO: add your methods here.
};

extern NFSV41_API int nNFSv41;

NFSV41_API int fnNFSv41(void);
