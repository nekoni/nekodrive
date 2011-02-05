namespace org.acplt.oncrpc.apps.jrpcgen
{
	/// <summary>
	/// The <code>JrpcgenUnion</code> class represents a single union defined
	/// in an rpcgen "x"-file.
	/// </summary>
	/// <remarks>
	/// The <code>JrpcgenUnion</code> class represents a single union defined
	/// in an rpcgen "x"-file.
	/// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:47 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
	/// <author>Jay Walters</author>
	public class JrpcgenUnion
	{
		/// <summary>Union identifier.</summary>
		/// <remarks>Union identifier.</remarks>
		public string identifier;

		/// <summary>
		/// <see cref="JrpcgenDeclaration">JrpcgenDeclaration</see>
		/// of descriminant element (containing its
		/// identifier and data type).
		/// </summary>
		public org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration descriminant;

		/// <summary>Contains arms of union.</summary>
		/// <remarks>
		/// Contains arms of union. The arms are of class
		/// <see cref="JrpcgenDeclaration">JrpcgenDeclaration</see>
		/// . The keys are the descriminant values.
		/// </remarks>
		public System.Collections.ArrayList elements;

		/// <summary>Returns just the identifier.</summary>
		/// <remarks>Returns just the identifier.</remarks>
		public override string ToString()
		{
			return identifier;
		}

		/// <summary>
		/// Constructs a <code>JrpcgenUnion</code> and sets the identifier, the
		/// descrimant element as well as all attribute elements.
		/// </summary>
		/// <remarks>
		/// Constructs a <code>JrpcgenUnion</code> and sets the identifier, the
		/// descrimant element as well as all attribute elements.
		/// </remarks>
		/// <param name="identifier">Identifier to be declared.</param>
		/// <param name="descriminant">
		/// Descriminant element of class
		/// <see cref="JrpcgenDeclaration">JrpcgenDeclaration</see>
		/// .
		/// </param>
		/// <param name="elements">
		/// Vector of atrribute elements of class
		/// <see cref="JrpcgenDeclaration">JrpcgenDeclaration</see>
		/// .
		/// </param>
		public JrpcgenUnion(string identifier, org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration
			 descriminant, System.Collections.ArrayList elements)
		{
			this.identifier = identifier;
			this.descriminant = descriminant;
			this.elements = elements;
		}

		/// <summary>
		/// Dumps the union together with its attribute elements end the
		/// descriminant to <code>System.out</code>.
		/// </summary>
		/// <remarks>
		/// Dumps the union together with its attribute elements end the
		/// descriminant to <code>System.out</code>.
		/// </remarks>
		public virtual void dump()
		{
			System.Console.Out.WriteLine("UNION " + identifier + ":");
			System.Console.Out.WriteLine("  switch (" + descriminant.type + " " + descriminant
				.identifier + ")");
			int size = elements.Count;
			for (int idx = 0; idx < size; ++idx)
			{
				org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnionArm a = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnionArm
					)elements[idx];
				System.Console.Out.Write("  ");
				a.dump();
			}
			System.Console.Out.WriteLine();
		}
	}
}
