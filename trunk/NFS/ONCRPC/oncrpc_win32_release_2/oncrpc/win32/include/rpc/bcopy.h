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

DllExport void bcopy(char *,char*,int);
//void bcopy_nf(char *,char *,int);
//void bcopy_fn(char *,char *,int);
//void bcopy_ff(char *,char *,int);
DllExport void bzero(char*,int);
DllExport int bcmp(char *s1, char *s2, int len);
