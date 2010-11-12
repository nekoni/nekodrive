namespace org.acplt.oncrpc.apps.jrpcgen
{
	/// <summary>
	/// The <code>JrpcgenConst</code> class represents a single constant defined
	/// in an rpcgen "x"-file.
	/// </summary>
	/// <remarks>
	/// The <code>JrpcgenConst</code> class represents a single constant defined
	/// in an rpcgen "x"-file.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:45 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class JrpcgenConst
	{
		/// <summary>Constant identifier.</summary>
		/// <remarks>Constant identifier.</remarks>
		public string identifier;

		/// <summary>Contains value (or identifier refering to another constant) of constant.
		/// 	</summary>
		/// <remarks>Contains value (or identifier refering to another constant) of constant.
		/// 	</remarks>
		public string value;

		/// <summary>
		/// Specifies the enclosure (scope) within the identifier must be
		/// addressed for a constant defined by an enumumeration.
		/// </summary>
		/// <remarks>
		/// Specifies the enclosure (scope) within the identifier must be
		/// addressed for a constant defined by an enumumeration.
		/// </remarks>
		public string enclosure;

		/// <summary>
		/// Returns value as integer literal (and thus resolving identifiers
		/// recursively, if necessary).
		/// </summary>
		/// <remarks>
		/// Returns value as integer literal (and thus resolving identifiers
		/// recursively, if necessary). This is only possible for simple
		/// subsitutions, that is A is defined as B, B as C, and C as 42, thus
		/// A is eventually defined as 42.
		/// <p>This simple kind of resolving is necessary when defining a particular
		/// version of an ONC/RPC protocol. We need to be able to resolve the
		/// version to an integer literal because we need to append the version
		/// number to any remote procedure defined to avoid identifier clashes if
		/// the same remote procedure is defined for several versions.
		/// </remarks>
		/// <returns>
		/// integer literal as <code>String</code> or <code>null</code>,
		/// if the identifier could not be resolved to an integer literal.
		/// </returns>
		public virtual string resolveValue()
		{
			if (value.Length > 0)
			{
				//
				// If the value is an integer literal, then we just have to
				// return it. That's it.
				//
				if (char.IsDigit(value[0]) || (value[0] == '-'))
				{
					return value;
				}
				//
				// It's an identifier, which we now have to resolve. First,
				// look it up in the list of global identifiers. Then recursively
				// resolve the value.
				//
				object id = org.acplt.oncrpc.apps.jrpcgen.jrpcgen.globalIdentifiers[identifier];
				if ((id != null) && (id is org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst))
				{
					return ((org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst)id).resolveValue();
				}
			}
			return null;
		}

		/// <summary>
		/// Constructs a <code>JrpcgenConst</code> and sets the identifier and
		/// the associated value.
		/// </summary>
		/// <remarks>
		/// Constructs a <code>JrpcgenConst</code> and sets the identifier and
		/// the associated value.
		/// </remarks>
		/// <param name="identifier">Constant identifier to define.</param>
		/// <param name="value">Value assigned to constant.</param>
		public JrpcgenConst(string identifier, string value) : this(identifier, value, null
			)
		{
		}

		/// <summary>
		/// Constructs a <code>JrpcgenConst</code> and sets the identifier and
		/// the associated value of an enumeration etc.
		/// </summary>
		/// <remarks>
		/// Constructs a <code>JrpcgenConst</code> and sets the identifier and
		/// the associated value of an enumeration etc.
		/// </remarks>
		/// <param name="identifier">Constant identifier to define.</param>
		/// <param name="value">Value assigned to constant.</param>
		/// <param name="enclosure">Name of enclosing enumeration, etc.</param>
		public JrpcgenConst(string identifier, string value, string enclosure)
		{
			this.identifier = identifier;
			this.value = value;
			this.enclosure = enclosure;
		}

		/// <summary>
		/// Returns the identifier this constant depends on or <code>null</code>,
		/// if no dependency exists.
		/// </summary>
		/// <remarks>
		/// Returns the identifier this constant depends on or <code>null</code>,
		/// if no dependency exists.
		/// </remarks>
		/// <returns>dependency identifier or <code>null</code>.</returns>
		public virtual string getDependencyIdentifier()
		{
			int len = value.Length;
			int idx = 0;
			char c;
			//
			// Check to see if it's an identifier and search for its end.
			// This is necessary as elements of an enumeration might have
			// "+x" appended, where x is an integer literal.
			//
			while (idx < len)
			{
				c = value[idx++];
				if (!(((c >= 'A') && (c <= 'Z')) || ((c >= 'a') && (c <= 'z')) || (c == '_') || (
					(c >= '0') && (c <= '9') && (idx > 0))))
				{
					--idx;
					// back up to the char not belonging to the identifier.
					break;
				}
			}
			if (idx > 0)
			{
                return value.Substring(0, idx);
			}
			return null;
		}

		/// <summary>Dumps the constant as well as its value to <code>System.out</code>.</summary>
		/// <remarks>Dumps the constant as well as its value to <code>System.out</code>.</remarks>
		public virtual void dump()
		{
			System.Console.Out.WriteLine(identifier + " = " + value);
		}

		/// <summary>
		/// Flag indicating whether this constant and its dependencies should be
		/// traversed any more.
		/// </summary>
		/// <remarks>
		/// Flag indicating whether this constant and its dependencies should be
		/// traversed any more.
		/// </remarks>
		public bool dontTraverseAnyMore = false;
	}
}
