namespace org.acplt.oncrpc.apps.jrpcgen
{
	/// <summary>
	/// The <code>JrpcgenStruct</code> class represents a single structure defined
	/// in an rpcgen "x"-file.
	/// </summary>
	/// <remarks>
	/// The <code>JrpcgenStruct</code> class represents a single structure defined
	/// in an rpcgen "x"-file.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:47 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class JrpcgenStruct
	{
		/// <summary>Structure identifier.</summary>
		/// <remarks>Structure identifier.</remarks>
		public string identifier;

		/// <summary>Contains elements of structure.</summary>
		/// <remarks>
		/// Contains elements of structure. The elements are of class
		/// <see cref="JrpcgenDeclaration">JrpcgenDeclaration</see>
		/// .
		/// </remarks>
		public System.Collections.ArrayList elements;

		/// <summary>Returns just the identifier.</summary>
		/// <remarks>Returns just the identifier.</remarks>
		public override string ToString()
		{
			return identifier;
		}

		/// <summary>
		/// Constructs a <code>JrpcgenStruct</code> and sets the identifier and all
		/// its attribute elements.
		/// </summary>
		/// <remarks>
		/// Constructs a <code>JrpcgenStruct</code> and sets the identifier and all
		/// its attribute elements.
		/// </remarks>
		/// <param name="identifier">Identifier to be declared.</param>
		/// <param name="elements">
		/// Vector of atrribute elements of class
		/// <see cref="JrpcgenDeclaration">JrpcgenDeclaration</see>
		/// .
		/// </param>
		public JrpcgenStruct(string identifier, System.Collections.ArrayList elements)
		{
			this.identifier = identifier;
			this.elements = elements;
		}

		/// <summary>
		/// Dumps the structure together with its attribute elements to
		/// <code>System.out</code>.
		/// </summary>
		/// <remarks>
		/// Dumps the structure together with its attribute elements to
		/// <code>System.out</code>.
		/// </remarks>
		public virtual void dump()
		{
			System.Console.Out.WriteLine("STRUCT " + identifier + ":");
			int size = elements.Count;
			for (int idx = 0; idx < size; ++idx)
			{
				org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration d = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration
					)elements[idx];
				System.Console.Out.Write("  ");
				d.dump();
			}
			System.Console.Out.WriteLine();
		}
	}
}
