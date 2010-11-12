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
/*
 * xdr_strarr.c, Generic XDR routine implementation.
 *
 * Copyrleft (C) 1998, ESRF.
 *
 * This is one of the "non-trivial" xdr primitives used to serialize and de-serialize
 * arrays.  See xdr.h for more info on the interface to xdr.
 */
#include "all_oncrpc.h"

/*
 * XDR an array of string elements.
 * addrp is a pointer to the string pointer array, *sizep is the number of 
 * string elements. If *addrp is NULL (*sizep) char pointers are allocated.
 */
bool_t
xdr_strarray(xdrs, addrp, sizep, maxsize)
	register XDR *xdrs;
	register char ***addrp;	/* array pointer */
	u_int *sizep;		    /* number of elements */
	u_int maxsize;		    /* max numberof elements */
{
	register u_int i;
	register char **target = *addrp;
	register u_int c;  /* the actual element count */
	register bool_t stat = TRUE;

	/* like strings, arrays are really counted arrays */
	if (! xdr_u_int(xdrs, sizep)) {
		return (FALSE);
	}
	c = *sizep;
	if ((c > maxsize) && (xdrs->x_op != XDR_FREE)) {
		return (FALSE);
	}

	/*
	 * if we are deserializing, we may need to allocate a pointer array.
	 * We also save time by checking for a null array if we are freeing.
	 */
	if (target == NULL)
		switch (xdrs->x_op) {
		case XDR_DECODE:
			if (c == 0)
				return (TRUE);
			/* allocate and zero */
			*addrp = target = (char**) calloc( c, sizeof(char*));
			if (target == NULL) {
#ifdef WIN32
				nt_rpc_report(
#else
				(void) fprintf(stderr, 
#endif
					"xdr_strarray: out of memory\n");
				return (FALSE);
			}
			break;

		case XDR_FREE:
			return (TRUE);
	}

	/*
	 * now we xdr each element (string) of the array
	 */
	for (i = 0; (i < c) && stat; i++) {
		stat = xdr_string(xdrs, target, maxsize);
		target ++;
	}

	/*
	 * the array may need freeing
	 */
	if (xdrs->x_op == XDR_FREE) {
		mem_free(*addrp, c* sizeof(char*));
		*addrp = NULL;
	}
	return (stat);
}

