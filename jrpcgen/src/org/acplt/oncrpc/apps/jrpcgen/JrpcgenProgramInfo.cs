using System.IO;
namespace org.acplt.oncrpc.apps.jrpcgen
{
	/// <summary>
	/// The <code>JrpcgenProgramInfo</code> class contains information about a
	/// single ONC/RPC program as defined in an rpcgen "x"-file.
	/// </summary>
	/// <remarks>
	/// The <code>JrpcgenProgramInfo</code> class contains information about a
	/// single ONC/RPC program as defined in an rpcgen "x"-file.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:46 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	internal class JrpcgenProgramInfo
	{
		/// <summary>Program number assigned to an ONC/RPC program.</summary>
		/// <remarks>
		/// Program number assigned to an ONC/RPC program. This attribute contains
		/// either an integer literal or an identifier (which must resolve to an
		/// integer).
		/// </remarks>
		public string programNumber;

		/// <summary>Identifier assigned to the program number of an ONC/RPC program.</summary>
		/// <remarks>Identifier assigned to the program number of an ONC/RPC program.</remarks>
		public string programId;

		/// <summary>Set of versions specified for a particular ONC/RPC program.</summary>
		/// <remarks>
		/// Set of versions specified for a particular ONC/RPC program.
		/// The elements in the set are of class
		/// <see cref="JrpcgenVersionInfo">JrpcgenVersionInfo</see>
		/// .
		/// </remarks>
		public System.Collections.ArrayList versions;

		/// <summary>
		/// Construct a new <code>JrpcgenProgramInfo</code> object containing the
		/// programs's identifier and number, as well as the versions defined
		/// for this particular ONC/RPC program.
		/// </summary>
		/// <remarks>
		/// Construct a new <code>JrpcgenProgramInfo</code> object containing the
		/// programs's identifier and number, as well as the versions defined
		/// for this particular ONC/RPC program.
		/// </remarks>
		/// <param name="programId">Identifier defined for this ONC/RPC program.</param>
		/// <param name="programNumber">Program number assigned to this ONC/RPC program.</param>
		/// <param name="versions">Vector of versions defined for this ONC/RPC program.</param>
		public JrpcgenProgramInfo(string programId, string programNumber, System.Collections.ArrayList
			 versions)
		{
			this.programId = programId;
			this.programNumber = programNumber;
			this.versions = versions;
		}

		/// <summary>
		/// Generates source code to define all constants belonging to this
		/// program.
		/// </summary>
		/// <remarks>
		/// Generates source code to define all constants belonging to this
		/// program.
		/// </remarks>
		/// <param name="out">PrintWriter to send source code to.</param>
		public virtual void dumpConstants(StreamWriter @out)
		{
			@out.WriteLine("    /* ONC/RPC program number definition */");
			@out.WriteLine("    public final static int " + programId + " = " + programNumber +
				 ";");
			int size = versions.Count;
			for (int idx = 0; idx < size; ++idx)
			{
				org.acplt.oncrpc.apps.jrpcgen.JrpcgenVersionInfo version = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenVersionInfo
					)versions[idx];
				version.dumpConstants(@out);
			}
		}
	}
}
