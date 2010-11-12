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
/*
*  Added on suggestion from Martin Johnson (MJJ)
*
*  The -v flag means VERBOSE, i.e. generate NT Application
*  Event Log messages in Portmap.c/find_service() when
*  clients try to connect.
*
*  To enable VERBOSE mode the service has to be started manually from 
*  Control Panel/Services, with something in the "Startup Parameters"
*  field.  (Note: "NET START PORTMAP -V" won't work as the -V gets
*  thrown away.)
*/

#include <rpc/rpc.h>
#include <windows.h>
#include <stdio.h>
#include <stdlib.h>
#include <rpc/pmap_pro.h>

void reg_service();

#ifdef _NT

SERVICE_STATUS          ssStatus;
SERVICE_STATUS_HANDLE   sshStatusHandle;
DWORD                   dwGlobalErr;
HANDLE                  pipeHandle;

VOID    service_main(DWORD dwArgc, LPTSTR *lpszArgv);
VOID    WINAPI service_ctrl(DWORD dwCtrlCode);
BOOL    ReportStatusToSCMgr(DWORD dwCurrentState,
                            DWORD dwWin32ExitCode,
                            DWORD dwCheckPoint,
                            DWORD dwWaitHint);
VOID    portmap_main(VOID *notUsed);
VOID    StopPortmapService(LPTSTR lpszMsg);
VOID	Report(LPTSTR lpszMsg);


VOID
main() 
{
    SERVICE_TABLE_ENTRY dispatchTable[] = {
        { TEXT("PortmapService"), (LPSERVICE_MAIN_FUNCTION)service_main },
        { NULL, NULL }
    };

    if (!StartServiceCtrlDispatcher(dispatchTable)) {
        StopPortmapService("StartServiceCtrlDispatcher failed.");
    }
}
#endif


HANDLE                  hServDoneEvent = NULL;
HANDLE                  threadHandle = NULL;
DWORD                   TID = 0;
int verbose = 0;              /* MJJ: 14-11-96 added this variable */

void
#ifdef _NT
service_main(DWORD dwArgc, LPTSTR *lpszArgv)
#elif defined _W95
main(DWORD dwArgc, LPTSTR *lpszArgv)
#endif
{
    DWORD                   dwWait;
    int sock;
    struct sockaddr_in addr;
    SVCXPRT *xprt;
    int len = sizeof(struct sockaddr_in);

#if !defined _NT && !defined _W95
	MessageBox(NULL, "Wrong executable: either '_NT' or '_W95' not defined during build","Fatal",MB_OK |MB_ICONEXCLAMATION);
	return;
#endif
    if (dwArgc > 1)             /* MJJ: 14-11-96 added this 'if' block */
    {
      /*
       *  If we get here then the service was started manually from 
       *  Control Panel/Services, with something in the "Startup Parameters"
       *  field.  (Note: "NET START PORTMAP -V" won't work as the -V gets
       *  thrown away.)
       */
      DWORD i;
      for (i=1; i<dwArgc; i++)
        if ( (!strcmp(lpszArgv[i], "-v")) || (!strcmp(lpszArgv[i], "-V")) )
          verbose = 1;
    }

#ifdef _NT
	sshStatusHandle = RegisterServiceCtrlHandler(
                                    TEXT("PortmapService"),
                                    (LPHANDLER_FUNCTION)service_ctrl);

    if (!sshStatusHandle)
        goto exit_portmap;

    ssStatus.dwServiceType = SERVICE_WIN32_OWN_PROCESS;
    ssStatus.dwServiceSpecificExitCode = 0;

    if (!ReportStatusToSCMgr(SERVICE_START_PENDING, NO_ERROR, 1, 3000))
        goto exit_portmap;
#endif

    hServDoneEvent = CreateEvent(NULL, TRUE, FALSE, NULL);

    if (hServDoneEvent == (HANDLE)NULL)
        goto exit_portmap;


#ifdef _NT
    if (!ReportStatusToSCMgr(SERVICE_START_PENDING, NO_ERROR, 2, 3000))
        goto exit_portmap;
#endif

    if (rpc_nt_init() != 0) {
	goto exit_portmap;
	}

	if ((sock = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP)) == INVALID_SOCKET) 
	{
		char msg[]="cannot create socket";
#ifdef _NT
		StopPortmapService(msg);
#elif defined _W95
		printf("%s\n",msg);
#endif
		return;
	}

	addr.sin_addr.s_addr = 0;
	addr.sin_family = AF_INET;
	addr.sin_port = htons(PMAPPORT);
	if (bind(sock, (struct sockaddr *)&addr, len) != 0) 
	{
		char msg[]="cannot bind";
#ifdef _NT
		StopPortmapService(msg);
#elif defined _W95
		printf("%s\n",msg);
#endif
		return;
	}

	if ((xprt = svcudp_create(sock)) == (SVCXPRT *)NULL) 
	{
		char msg[]="couldn't do udp_create";
#ifdef _NT
		StopPortmapService(msg);
#elif defined _W95
		printf("%s\n",msg);
#endif
		return;
	}

	if ((sock = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP)) < 0) 
	{
		char msg[]="cannot create socket";
#ifdef _NT
		StopPortmapService(msg);
#elif defined _W95
		printf("%s\n",msg);
#endif
		return;
	}
	if (bind(sock, (struct sockaddr *)&addr, len) != 0) 
	{
		char msg[]="cannot bind";
#ifdef _NT
		StopPortmapService(msg);
#elif defined _W95
		printf("%s\n",msg);
#endif
		return;
	}
	if ((xprt = svctcp_create(sock, 0, 0)) == (SVCXPRT *)NULL) 
	{
		char msg[]="couldn't do tcp_create";
#ifdef _NT
		StopPortmapService(msg);
#elif defined _W95
		printf("%s\n",msg);
#endif
		return;
	}

        (void)svc_register(xprt, PMAPPROG, PMAPVERS, reg_service, FALSE);

    threadHandle = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)svc_run, NULL, 0, &TID);

    if (!threadHandle)
        goto exit_portmap;

#ifdef _NT
	if (!ReportStatusToSCMgr(SERVICE_RUNNING, NO_ERROR, 0, 0))
        goto exit_portmap;
#elif defined _W95
	printf("%s\n", "Portmap service for Windows95 up and ready!");
#endif

    dwWait = WaitForSingleObject(hServDoneEvent, INFINITE);

    TerminateThread(threadHandle, 0);

exit_portmap:

	{
		char msg[]="Portmap service terminates!";
#ifdef _NT
		Report(msg);
#elif defined _W95
		printf("%s\n",msg);
#endif
	}

    rpc_nt_exit();

    if (hServDoneEvent != NULL)
        CloseHandle(hServDoneEvent);

#ifdef _NT
    if (sshStatusHandle != 0)
        (VOID)ReportStatusToSCMgr(SERVICE_STOPPED, dwGlobalErr, 0, 0);
#endif

    return;
}


#ifdef _NT
VOID
WINAPI service_ctrl(DWORD dwCtrlCode)
{
    DWORD  dwState = SERVICE_RUNNING;

    switch(dwCtrlCode) {

        case SERVICE_CONTROL_PAUSE:

            if (ssStatus.dwCurrentState == SERVICE_RUNNING) {
                SuspendThread(threadHandle);
                dwState = SERVICE_PAUSED;
            }
            break;

        case SERVICE_CONTROL_CONTINUE:

            if (ssStatus.dwCurrentState == SERVICE_PAUSED) {
                ResumeThread(threadHandle);
                dwState = SERVICE_RUNNING;
            }
            break;

        case SERVICE_CONTROL_STOP:

            dwState = SERVICE_STOP_PENDING;

            ReportStatusToSCMgr(SERVICE_STOP_PENDING, NO_ERROR, 1, 3000);

            SetEvent(hServDoneEvent);
            return;

        case SERVICE_CONTROL_INTERROGATE:
            break;

        default:
            break;

    }

    ReportStatusToSCMgr(dwState, NO_ERROR, 0, 0);
}


BOOL
ReportStatusToSCMgr(DWORD dwCurrentState,
                    DWORD dwWin32ExitCode,
                    DWORD dwCheckPoint,
                    DWORD dwWaitHint)
{
    BOOL fResult;

    if (dwCurrentState == SERVICE_START_PENDING)
        ssStatus.dwControlsAccepted = 0;
    else
        ssStatus.dwControlsAccepted = SERVICE_ACCEPT_STOP |
            SERVICE_ACCEPT_PAUSE_CONTINUE;

    ssStatus.dwCurrentState = dwCurrentState;
    ssStatus.dwWin32ExitCode = dwWin32ExitCode;
    ssStatus.dwCheckPoint = dwCheckPoint;

    ssStatus.dwWaitHint = dwWaitHint;

    if (!(fResult = SetServiceStatus(sshStatusHandle, &ssStatus)))
            StopPortmapService("SetServiceStatus");

    return fResult;
}


VOID
StopPortmapService(LPTSTR lpszMsg)
{
    CHAR    chMsg[256];
    HANDLE  hEventSource;
    LPTSTR  lpszStrings[2];

    dwGlobalErr = GetLastError();

    hEventSource = RegisterEventSource(NULL,
                            TEXT("Portmap"));

    sprintf(chMsg, "Portmap error: %d", dwGlobalErr);
    lpszStrings[0] = chMsg;
    lpszStrings[1] = lpszMsg;

    if (hEventSource != NULL) {
        ReportEvent(hEventSource,
            EVENTLOG_ERROR_TYPE,
            0,
            0,
            NULL,
            2,
            0,
            lpszStrings,
            NULL);

        (VOID) DeregisterEventSource(hEventSource);
    }

    SetEvent(hServDoneEvent);
}


VOID
Report(LPTSTR lpszMsg)
{
    CHAR    chMsg[256];
    HANDLE  hEventSource;
    LPTSTR  lpszStrings[2];

    dwGlobalErr = GetLastError();

    hEventSource = RegisterEventSource(NULL,
                            TEXT("Portmap"));

    sprintf(chMsg, "Portmap report: %d", dwGlobalErr);
    lpszStrings[0] = chMsg;
    lpszStrings[1] = lpszMsg;

    if (hEventSource != NULL) {
        ReportEvent(hEventSource,
            EVENTLOG_INFORMATION_TYPE,
            0,
            0,
            NULL,
            2,
            0,
            lpszStrings,
            NULL);

        (VOID) DeregisterEventSource(hEventSource);
    }
}
#endif /* _NT */
