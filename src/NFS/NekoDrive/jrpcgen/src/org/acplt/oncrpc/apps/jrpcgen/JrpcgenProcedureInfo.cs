namespace org.acplt.oncrpc.apps.jrpcgen
{
	/// <summary>
	/// The <code>JrpcgenProcedureInfo</code> class contains information about a
	/// specific version of an ONC/RPC program as defined in an rpcgen "x"-file.
	/// </summary>
	/// <remarks>
	/// The <code>JrpcgenProcedureInfo</code> class contains information about a
	/// specific version of an ONC/RPC program as defined in an rpcgen "x"-file.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.3 $ $Date: 2003/08/14 11:26:50 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	internal class JrpcgenProcedureInfo
	{
		/// <summary>
		/// Procedure number assigned to the procedure of a particular verions of
		/// an ONC/RPC program.
		/// </summary>
		/// <remarks>
		/// Procedure number assigned to the procedure of a particular verions of
		/// an ONC/RPC program. This attribute contains either an integer literal
		/// or an identifier (which must resolve to an integer).
		/// </remarks>
		public string procedureNumber;

		/// <summary>
		/// Identifier assigned to the procedure number of an a particular
		/// procedure of a particular version of an ONC/RPC program.
		/// </summary>
		/// <remarks>
		/// Identifier assigned to the procedure number of an a particular
		/// procedure of a particular version of an ONC/RPC program.
		/// </remarks>
		public string procedureId;

		/// <summary>Type specifier of the result returned by the remote procedure.</summary>
		/// <remarks>Type specifier of the result returned by the remote procedure.</remarks>
		public string resultType;

		/// <summary>Parameter(s) to the remote procedure.</summary>
		/// <remarks>Parameter(s) to the remote procedure.</remarks>
		public System.Collections.ArrayList parameters;

		/// <summary>
		/// Constructs a new <code>JrpcgenProcedureInfo</code> object containing
		/// information about a programs' version and a set of procedures
		/// defined by this program version.
		/// </summary>
		/// <remarks>
		/// Constructs a new <code>JrpcgenProcedureInfo</code> object containing
		/// information about a programs' version and a set of procedures
		/// defined by this program version.
		/// </remarks>
		/// <param name="procedureId">
		/// Identifier assigned to the procedure of a particular
		/// version of an ONC/RPC program.
		/// </param>
		/// <param name="procedureNumber">Procedure number assigned to remote procedure.</param>
		/// <param name="resultType">Type specifier of result returned by remote procedure.</param>
		/// <param name="parameters">Type specifier of parameter to the remote procedure.</param>
		public JrpcgenProcedureInfo(string procedureId, string procedureNumber, string resultType
			, System.Collections.ArrayList parameters)
		{
			this.procedureId = procedureId;
			this.procedureNumber = procedureNumber;
			this.resultType = resultType;
			this.parameters = parameters;
		}
	}
}
