/*
 * $Header: /cvsroot/remotetea/remotetea/src/org/acplt/oncrpc/XdrVoid.java,v 1.1.1.1 2003/08/13 12:03:41 haraldalbrecht Exp $
 *
 * Copyright (c) 1999, 2000
 * Lehrstuhl fuer Prozessleittechnik (PLT), RWTH Aachen
 * D-52064 Aachen, Germany.
 * All rights reserved.
 *
 * This library is free software; you can redistribute it and/or modify
 * it under the terms of the GNU Library General Public License as
 * published by the Free Software Foundation; either version 2 of the
 * License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Library General Public License for more details.
 *
 * You should have received a copy of the GNU Library General Public
 * License along with this program (see the file COPYING.LIB for more
 * details); if not, write to the Free Software Foundation, Inc.,
 * 675 Mass Ave, Cambridge, MA 02139, USA.
 */

namespace org.acplt.oncrpc
{
	/// <summary>
	/// Instances of the class <code>XdrChar</code> represent (de-)serializeable
	/// chars, which are especially useful in cases where a result with only a
	/// single char is expected from a remote function call or only a single
	/// char parameter needs to be supplied.
	/// </summary>
	/// <remarks>
	/// Instances of the class <code>XdrChar</code> represent (de-)serializeable
	/// chars, which are especially useful in cases where a result with only a
	/// single char is expected from a remote function call or only a single
	/// char parameter needs to be supplied.
	/// <p>Please note that this class is somewhat modelled after Java's primitive
	/// data type wrappers. As for these classes, the XDR data type wrapper classes
	/// follow the concept of values with no identity, so you are not allowed to
	/// change the value after you've created a value object.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:44 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class XdrChar : org.acplt.oncrpc.XdrAble
	{
		/// <summary>Constructs and initializes a new <code>XdrChar</code> object.</summary>
		/// <remarks>Constructs and initializes a new <code>XdrChar</code> object.</remarks>
		/// <param name="value">Char value.</param>
		public XdrChar(char value)
		{
			this.value = value;
		}

		/// <summary>Constructs and initializes a new <code>XdrChar</code> object.</summary>
		/// <remarks>Constructs and initializes a new <code>XdrChar</code> object.</remarks>
		public XdrChar()
		{
			this.value = (char)0;
		}

		/// <summary>
		/// Returns the value of this <code>XdrChar</code> object as a char
		/// primitive.
		/// </summary>
		/// <remarks>
		/// Returns the value of this <code>XdrChar</code> object as a char
		/// primitive.
		/// </remarks>
		/// <returns>The primitive <code>char</code> value of this object.</returns>
		public virtual char charValue()
		{
			return this.value;
		}

		/// <summary>
		/// Encodes -- that is: serializes -- a XDR char into a XDR stream in
		/// compliance to RFC 1832.
		/// </summary>
		/// <remarks>
		/// Encodes -- that is: serializes -- a XDR char into a XDR stream in
		/// compliance to RFC 1832.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual void xdrEncode(org.acplt.oncrpc.XdrEncodingStream xdr)
		{
			xdr.xdrEncodeByte((byte)value);
		}

		/// <summary>
		/// Decodes -- that is: deserializes -- a XDR char from a XDR stream in
		/// compliance to RFC 1832.
		/// </summary>
		/// <remarks>
		/// Decodes -- that is: deserializes -- a XDR char from a XDR stream in
		/// compliance to RFC 1832.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual void xdrDecode(org.acplt.oncrpc.XdrDecodingStream xdr)
		{
			value = (char)xdr.xdrDecodeByte();
		}

		/// <summary>The encapsulated char value itself.</summary>
		/// <remarks>The encapsulated char value itself.</remarks>
		private char value;
	}
}
