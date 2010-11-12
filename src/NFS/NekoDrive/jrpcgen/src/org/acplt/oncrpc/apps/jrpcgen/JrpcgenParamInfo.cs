namespace org.acplt.oncrpc.apps.jrpcgen
{
	/// <summary>
	/// The <code>JrpcgenParamInfo</code> class contains information about the
	/// data type of a procedure's parameter, as well as the parameter's optional
	/// name.
	/// </summary>
	/// <remarks>
	/// The <code>JrpcgenParamInfo</code> class contains information about the
	/// data type of a procedure's parameter, as well as the parameter's optional
	/// name.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.2 $ $Date: 2003/08/14 08:09:59 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	internal class JrpcgenParamInfo
	{
		public string parameterType;

		public string parameterName;

		/// <summary>
		/// Constructs a new <code>JrpcgenParamInfo</code> object containing
		/// information about ...
		/// </summary>
		/// <remarks>
		/// Constructs a new <code>JrpcgenParamInfo</code> object containing
		/// information about ...
		/// </remarks>
		public JrpcgenParamInfo(string parameterType, string parameterName)
		{
			this.parameterType = parameterType;
			this.parameterName = parameterName;
		}
	}
}
