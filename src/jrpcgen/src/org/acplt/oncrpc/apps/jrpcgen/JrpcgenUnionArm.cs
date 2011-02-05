namespace org.acplt.oncrpc.apps.jrpcgen
{
	/// <summary>
	/// The <code>JrpcgenUnionArm</code> class represents a single union arm defined
	/// for a particular union in an rpcgen "x"-file.
	/// </summary>
	/// <remarks>
	/// The <code>JrpcgenUnionArm</code> class represents a single union arm defined
	/// for a particular union in an rpcgen "x"-file.
	/// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.2 $ $Date: 2003/08/14 08:09:59 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
	/// <author>Jay Walters</author>
	public class JrpcgenUnionArm
	{
		/// <summary>Value for which the descriminated union arm is valid.</summary>
		/// <remarks>Value for which the descriminated union arm is valid.</remarks>
		public string value;

		/// <summary>
		/// Attribute element of descriminated arm (of class
		/// <see cref="JrpcgenDeclaration">JrpcgenDeclaration</see>
		/// ).
		/// </summary>
		public org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration element;

		/// <summary>
		/// Constructs a <code>JrpcgenUnionArm</code> and sets decrimated arm's
		/// value and the associated attribute element.
		/// </summary>
		/// <remarks>
		/// Constructs a <code>JrpcgenUnionArm</code> and sets decrimated arm's
		/// value and the associated attribute element.
		/// </remarks>
		/// <param name="value">Value for which descriminated arm is valid.</param>
		/// <param name="element">
		/// Descriminated arm element of class
		/// <see cref="JrpcgenDeclaration">JrpcgenDeclaration</see>
		/// .
		/// </param>
		public JrpcgenUnionArm(string value, org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration
			 element)
		{
			this.value = value;
			this.element = element;
		}

		/// <summary>Dumps the union arm to <code>System.out</code>.</summary>
		/// <remarks>Dumps the union arm to <code>System.out</code>.</remarks>
		public virtual void dump()
		{
			if (value == null)
			{
				if (element == null)
				{
					System.Console.Out.WriteLine("  default: -");
				}
				else
				{
					if (element.identifier != null)
					{
						System.Console.Out.Write("  default: ");
						element.dump();
					}
					else
					{
						System.Console.Out.WriteLine("  default: void");
					}
				}
			}
			else
			{
				if (element == null)
				{
					System.Console.Out.WriteLine(" " + value + ": -");
				}
				else
				{
					if (element.identifier != null)
					{
						System.Console.Out.Write("  " + value + ": ");
						element.dump();
					}
					else
					{
						System.Console.Out.WriteLine("  " + value + ": void");
					}
				}
			}
		}
	}
}
