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
 * Users may use, copy or modify Sun RPC for the Windows NT Operating 
 * System according to the Sun copyright below.
 *
 * RPC for the Windows NT Operating System COMES WITH ABSOLUTELY NO 
 * WARRANTY, NOR WILL I BE LIABLE FOR ANY DAMAGES INCURRED FROM THE 
 * USE OF. USE ENTIRELY AT YOUR OWN RISK!!!
 *********************************************************************/

/*
 *  bcopy.c --
 *      Implements bcopy(2) and bzero(2) byte operations.
 *
 *  Author:
 *      See-Mong Tan, 6/26/88
 */

#include "all_oncrpc.h"

/*
 *  bcopy(char *s1, char *s2, int len) --
 *      Copies len bytes from s1 to s2
 */
void
bcopy(s1, s2, len)
	char *s1, *s2;
	int len;
{
	for(; len > 0; len--)
		*s2++ = *s1++;
}

/*
 *  bzero(char *s, int len) --
 *      Places len zero byes in s
 */
void
bzero(s, len)
	char *s;
	int len;
{
	for(; len > 0; len--)
		*s++ = (char) 0;
}

/*
 *  bcmp() compares byte  string  b1  against  byte  string  b2, 
 *  returning  zero  if  they are identical, non-zero otherwise. 
 *  Both strings are assumed to be length bytes long.  bcmp() of 
 *  length zero bytes always returns zero.                       
*/
int
bcmp(s1, s2, len)
	char *s1, *s2;
	int len;
{
	for(; len > 0; len--, s1++, s2++)
		if (*s1 != *s2)
			return 1;
	return 0;
}
	
