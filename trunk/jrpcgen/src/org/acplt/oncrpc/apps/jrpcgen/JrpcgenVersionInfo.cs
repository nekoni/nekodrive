using System.IO;
namespace org.acplt.oncrpc.apps.jrpcgen
{
	/// <summary>
	/// The <code>JrpcgenVersionInfo</code> class contains information about a
	/// specific version of an ONC/RPC program as defined in a rpcgen "x"-file.
	/// </summary>
	/// <remarks>
	/// The <code>JrpcgenVersionInfo</code> class contains information about a
	/// specific version of an ONC/RPC program as defined in a rpcgen "x"-file.
	/// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:47 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
	/// <author>Jay Walters</author>
	internal class JrpcgenVersionInfo
	{
		/// <summary>Version number assigned to an ONC/RPC program.</summary>
		/// <remarks>
		/// Version number assigned to an ONC/RPC program. This attribute contains
		/// either an integer literal or an identifier (which must resolve to an
		/// integer).
		/// </remarks>
		public string versionNumber;

		/// <summary>Identifier assigned to the version number of an ONC/RPC program.</summary>
		/// <remarks>Identifier assigned to the version number of an ONC/RPC program.</remarks>
		public string versionId;

		/// <summary>Set of procedures specified for a particular ONC/RPC program.</summary>
		/// <remarks>
		/// Set of procedures specified for a particular ONC/RPC program.
		/// The elements in the set are of class
		/// <see cref="JrpcgenProcedureInfo">JrpcgenProcedureInfo</see>
		/// .
		/// </remarks>
		public System.Collections.ArrayList procedures;

		/// <summary>
		/// Constructs a new <code>JrpcgenVersionInfo</code> object containing
		/// information about a programs' version and a set of procedures
		/// defined by this program version.
		/// </summary>
		/// <remarks>
		/// Constructs a new <code>JrpcgenVersionInfo</code> object containing
		/// information about a programs' version and a set of procedures
		/// defined by this program version.
		/// </remarks>
		/// <param name="versionId">
		/// Identifier defined for this version of a
		/// particular ONC/RPC program.
		/// </param>
		/// <param name="versionNumber">Version number.</param>
		/// <param name="procedures">Vector of procedures defined for this ONC/RPC program.</param>
		public JrpcgenVersionInfo(string versionId, string versionNumber, System.Collections.ArrayList
			 procedures)
		{
			this.versionId = versionId;
			this.versionNumber = versionNumber;
			this.procedures = procedures;
		}

		/// <summary>
		/// Generates source code to define the version constant belonging to this
		/// program.
		/// </summary>
		/// <remarks>
		/// Generates source code to define the version constant belonging to this
		/// program.
		/// </remarks>
		/// <param name="out">PrintWriter to send source code to.</param>
		public virtual void dumpConstants(StreamWriter @out)
		{
			@out.WriteLine("    /* ONC/RPC program version number definition */");
			@out.WriteLine("    public final static int " + versionId + " = " + versionNumber +
				 ";");
		}
	}
}
