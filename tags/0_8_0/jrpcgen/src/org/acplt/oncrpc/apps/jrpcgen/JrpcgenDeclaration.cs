namespace org.acplt.oncrpc.apps.jrpcgen
{
	/// <summary>
	/// The <code>JrpcgenDeclaration</code> class represents a single declaration
	/// from an rpcgen "x"-file.
	/// </summary>
	/// <remarks>
	/// The <code>JrpcgenDeclaration</code> class represents a single declaration
	/// from an rpcgen "x"-file.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.2 $ $Date: 2003/08/14 08:08:34 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class JrpcgenDeclaration : System.ICloneable
	{
		/// <summary>Identifier.</summary>
		/// <remarks>Identifier.</remarks>
		public string identifier;

		/// <summary>Type specifier.</summary>
		/// <remarks>Type specifier.</remarks>
		public string type;

		/// <summary>Kind of declaration (scalar, fixed size vector, dynamic vector).</summary>
		/// <remarks>Kind of declaration (scalar, fixed size vector, dynamic vector).</remarks>
		/// <seealso cref="SCALAR">SCALAR</seealso>
		/// <seealso cref="FIXEDVECTOR">FIXEDVECTOR</seealso>
		/// <seealso cref="DYNAMICVECTOR">DYNAMICVECTOR</seealso>
		/// <seealso cref="INDIRECTION">INDIRECTION</seealso>
		public int kind;

		/// <summary>Fixed size or upper limit for size of vector.</summary>
		/// <remarks>Fixed size or upper limit for size of vector.</remarks>
		public string size;

		/// <summary>Indicates that a scalar is declared.</summary>
		/// <remarks>Indicates that a scalar is declared.</remarks>
		public const int SCALAR = 0;

		/// <summary>Indicates that a vector (an array) with fixed size is declared.</summary>
		/// <remarks>Indicates that a vector (an array) with fixed size is declared.</remarks>
		public const int FIXEDVECTOR = 1;

		/// <summary>
		/// Indicates that a vector (an array) with dynamic (or unknown) size
		/// is declared.
		/// </summary>
		/// <remarks>
		/// Indicates that a vector (an array) with dynamic (or unknown) size
		/// is declared.
		/// </remarks>
		public const int DYNAMICVECTOR = 2;

		/// <summary>
		/// Indicates that an indirection (reference, pointer, whatever you like
		/// to call it nowadays) is declared.
		/// </summary>
		/// <remarks>
		/// Indicates that an indirection (reference, pointer, whatever you like
		/// to call it nowadays) is declared.
		/// </remarks>
		public const int INDIRECTION = 3;

		/// <summary>Returns the identifier.</summary>
		/// <remarks>Returns the identifier.</remarks>
		public override string ToString()
		{
			return identifier;
		}

		/// <summary>
		/// Constructs a <code>JrpcgenDeclaration</code> and sets the identifier
		/// and its data type.
		/// </summary>
		/// <remarks>
		/// Constructs a <code>JrpcgenDeclaration</code> and sets the identifier
		/// and its data type. The
		/// <see cref="kind">kind</see>
		/// of the
		/// declaration is assumed to be
		/// <see cref="SCALAR">SCALAR</see>
		/// .
		/// </remarks>
		/// <param name="identifier">Identifier to be declared.</param>
		/// <param name="type">Data type the identifier is declared of.</param>
		public JrpcgenDeclaration(string identifier, string type)
		{
			this.identifier = identifier;
			this.type = type;
			this.kind = SCALAR;
		}

		/// <summary>
		/// Constructs a <code>JrpcgenDeclaration</code> and sets the identifier,
		/// its data type, kind and size of vector.
		/// </summary>
		/// <remarks>
		/// Constructs a <code>JrpcgenDeclaration</code> and sets the identifier,
		/// its data type, kind and size of vector. This constructur is typically
		/// used when declaring either fixed-size or dynamic arrays.
		/// </remarks>
		/// <param name="identifier">Identifier to be declared.</param>
		/// <param name="type">Data type the identifier is declared of.</param>
		/// <param name="kind">Kind of declaration (scalar, vector, indirection).</param>
		/// <param name="size">Size of array (if fixed-sized, otherwise <code>null</code>).</param>
		public JrpcgenDeclaration(string identifier, string type, int kind, string size)
		{
			this.identifier = identifier;
			this.type = type;
			this.kind = kind;
			this.size = size;
		}

		/// <summary>Dumps the declaration to <code>System.out</code>.</summary>
		/// <remarks>Dumps the declaration to <code>System.out</code>.</remarks>
		public virtual void dump()
		{
			System.Console.Out.Write(type);
			System.Console.Out.Write(kind == org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration
				.INDIRECTION ? " *" : " ");
			System.Console.Out.Write(identifier);
			switch (kind)
			{
				case org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration.FIXEDVECTOR:
				{
					System.Console.Out.Write("[" + size + "]");
					break;
				}

				case org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration.DYNAMICVECTOR:
				{
					if (size != null)
					{
						System.Console.Out.Write("<" + size + ">");
					}
					else
					{
						System.Console.Out.Write("<>");
					}
					break;
				}
			}
			System.Console.Out.WriteLine();
		}

		/// <summary>Clones declaration object.</summary>
		/// <remarks>Clones declaration object.</remarks>
		/// <exception cref="java.lang.CloneNotSupportedException"></exception>
		internal new object MemberwiseClone()
		{
			return base.MemberwiseClone();
		}

		object System.ICloneable.Clone()
		{
			return MemberwiseClone();
		}
	}
}
