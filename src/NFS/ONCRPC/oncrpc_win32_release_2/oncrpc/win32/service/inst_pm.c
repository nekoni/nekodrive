/**********************************************************************
 * ONC RPC for WIN32.
 * 1997 by WD Klotz
 * ESRF, BP 220, F-38640 Grenoble, CEDEX
 * klotz-tech@esrf.fr
 *
 * SUN's ONC RPC for Windows NT and Windows 95. Ammended port from
 * Martin F.Gergeleit's distribution. This version has been modified
 * and cleaned, such as to be compatible with Windows NT and Windows 95. 
 * Compiler: MSVC++ version 4.2 and 5.0.
 *
 * Users may use, copy or modify Sun RPC for the Windows NT Operating 
 * System according to the Sun copyright below.
 * RPC for the Windows NT Operating System COMES WITH ABSOLUTELY NO 
 * WARRANTY, NOR WILL I BE LIABLE FOR ANY DAMAGES INCURRED FROM THE 
 * USE OF. USE ENTIRELY AT YOUR OWN RISK!!!
 **********************************************************************/
/*********************************************************************
 * RPC for the Windows NT Operating System
 * 1993 by Martin F. Gergeleit
 *
 * RPC for the Windows NT Operating System COMES WITH ABSOLUTELY NO 
 * WARRANTY, NOR WILL I BE LIABLE FOR ANY DAMAGES INCURRED FROM THE 
 * USE OF. USE ENTIRELY AT YOUR OWN RISK!!!
 *********************************************************************/

#include <windows.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

SC_HANDLE   service;
SC_HANDLE   manager;

VOID
InstallService(LPCTSTR serviceName, LPCTSTR serviceExe)
{
    LPCTSTR lpszBinaryPathName = serviceExe;

    service = CreateService(
        manager,
        serviceName,
        serviceName,
        SERVICE_ALL_ACCESS,
        SERVICE_WIN32_OWN_PROCESS,
        SERVICE_DEMAND_START,
        SERVICE_ERROR_NORMAL,
        lpszBinaryPathName,
        NULL,
        NULL,
        NULL,
        NULL,
        NULL);

    if (service == NULL) {
        printf("failure: CreateService (0x%02x)\n", GetLastError());
        return;
    } else
        printf("CreateService SUCCESS\n");

    CloseServiceHandle(service);
}

VOID
RemoveService(LPCTSTR serviceName)
{
    service = OpenService(manager, serviceName, SERVICE_ALL_ACCESS);

    if (service == NULL) {
        printf("failure: OpenService (0x%02x)\n", GetLastError());
        return;
    }

    if (DeleteService(service))
        printf("DeleteService SUCCESS\n");
    else
        printf("failure: DeleteService (0x%02x)\n", GetLastError());
}

VOID
main(int argc, char *argv[])
{
    if (argc != 2) {
        printf("usage: inst_pm <full pathname>\\portmap.exe\n");
        printf("           to install portmap, or:\n");
        printf("       inst_pm remove\n");
        printf("           to remove it.\n");
        exit(1);
    }

    manager = OpenSCManager(
                        NULL,
                        NULL,
                        SC_MANAGER_ALL_ACCESS
                        );

    if (manager == NULL) {
        printf("OpenSCManager() failed! (missing priviledges?)\n");
        exit(1);
    }
    	
    if (!stricmp(argv[1], "remove"))
        RemoveService("Portmap");
    else
        InstallService("Portmap", argv[1]);

    CloseServiceHandle(manager);
}
