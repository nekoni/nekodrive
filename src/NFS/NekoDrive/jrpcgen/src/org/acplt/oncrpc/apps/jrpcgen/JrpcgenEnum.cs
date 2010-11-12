namespace org.acplt.oncrpc.apps.jrpcgen
{
	/// <summary>
	/// The <code>JrpcgenEnum</code> class represents a single enumeration
	/// from an rpcgen "x"-file.
	/// </summary>
	/// <remarks>
	/// The <code>JrpcgenEnum</code> class represents a single enumeration
	/// from an rpcgen "x"-file. It is a "container" for the elements (constants)
	/// belonging to this enumeration.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:45 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class JrpcgenEnum
	{
		/// <summary>Enumeration identifier.</summary>
		/// <remarks>Enumeration identifier.</remarks>
		public string identifier;

		/// <summary>Contains enumeration elements as well as their values.</summary>
		/// <remarks>
		/// Contains enumeration elements as well as their values. The elements
		/// are of class
		/// <see cref="JrpcgenConst">JrpcgenConst</see>
		/// .
		/// </remarks>
		public System.Collections.ArrayList enums;

		/// <summary>Returns the fully qualified identifier.</summary>
		/// <remarks>
		/// Returns the fully qualified identifier.
		/// return fully qualified identifier.
		/// </remarks>
		public override string ToString()
		{
			return identifier;
		}

		/// <summary>
		/// Constructs a <code>JrpcgenEnum</code> and sets the identifier and all
		/// its enumeration elements.
		/// </summary>
		/// <remarks>
		/// Constructs a <code>JrpcgenEnum</code> and sets the identifier and all
		/// its enumeration elements.
		/// </remarks>
		/// <param name="identifier">Identifier to be declared.</param>
		/// <param name="enums">
		/// Vector of enumeration elements of class
		/// <see cref="JrpcgenConst">JrpcgenConst</see>
		/// .
		/// </param>
		public JrpcgenEnum(string identifier, System.Collections.ArrayList enums)
		{
			this.identifier = identifier;
			this.enums = enums;
		}

		/// <summary>
		/// Dumps the enumeration together with its elements to
		/// <code>System.out</code>.
		/// </summary>
		/// <remarks>
		/// Dumps the enumeration together with its elements to
		/// <code>System.out</code>.
		/// </remarks>
		public virtual void dump()
		{
			System.Console.Out.WriteLine("ENUM " + identifier);
			int size = enums.Count;
			for (int idx = 0; idx < size; ++idx)
			{
				org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst c = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst
					)enums[idx];
				System.Console.Out.Write("  ");
				c.dump();
			}
			System.Console.Out.WriteLine();
		}
	}
}
