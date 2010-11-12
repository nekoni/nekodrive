namespace org.acplt.oncrpc.apps.jrpcgen
{
	/// <summary>
	/// The <code>JrpcgenParserException</code> class represents a parser
	/// exception indicating to abort parsing the x-file.
	/// </summary>
	/// <remarks>
	/// The <code>JrpcgenParserException</code> class represents a parser
	/// exception indicating to abort parsing the x-file.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:46 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	[System.Serializable]
	internal class JrpcgenParserException : System.Exception
	{
		/// <summary>Constructs a <code>JrpcgenParserException</code> with no detail message.
		/// 	</summary>
		/// <remarks>Constructs a <code>JrpcgenParserException</code> with no detail message.
		/// 	</remarks>
		public JrpcgenParserException() : base()
		{
		}

        public JrpcgenParserException(string msg)
            : base(msg)
        {
        }
	}
}
