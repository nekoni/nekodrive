using System;
using System.IO;
using TUVienna.CS_CUP.Runtime;
using System.Collections;
namespace org.acplt.oncrpc.apps.jrpcgen
{
    /// <summary>
    /// The class <code>jrpcgen</code> implements a Java-based rpcgen RPC protocol
    /// compiler.
    /// </summary>
    /// <remarks>
    /// The class <code>jrpcgen</code> implements an rpcgen RPC protocol
    /// compiler. jrpcgen is a c#-based tool that generates source code of C#
    /// classes to implement an RPC protocol. The input to jrpcgen is a language
    /// similiar to C (but more probably much more similiar to FORTRAN) known as
    /// the RPC language (Remote Procedure Call Language).
    /// Converted to C# using the db4o Sharpen tool and then modified to work
    /// with the C# standard file I/O classes.
    /// </remarks>
    /// <version>$Revision: 1.6 $ $Date: 2007/05/29 19:38:30 $ $State: Exp $ $Locker:  $</version>
    /// <author>Harald Albrecht</author>
    /// <author>Jay Walters</author>
    public class jrpcgen
    {
        //
        // Personal note: this class probably suffers from a flashback on
        // procedural programming ... but where do we need to be today?
        //
        /// <summary>Print the help message describing the available command line options.</summary>
        /// <remarks>Print the help message describing the available command line options.</remarks>
        public static void printHelp()
        {
            System.Console.Out.WriteLine("Usage: jrpcgen [-options] x-file");
            System.Console.Out.WriteLine();
            System.Console.Out.WriteLine("where options include:");
            System.Console.Out.WriteLine("  -c <classname>  specify class name of client proxy stub"
                );
            System.Console.Out.WriteLine("  -d <dir>        specify directory where to place generated source code files"
                );
            System.Console.Out.WriteLine("  -p <package>    specify package name for generated source code files"
                );
            System.Console.Out.WriteLine("  -s <classname>  specify class name of server proxy stub"
                );
            System.Console.Out.WriteLine("  -ser            tag generated XDR classes as serializable"
                );
            System.Console.Out.WriteLine("  -bean           generate accessors for usage as bean, implies -ser"
                );
            System.Console.Out.WriteLine("  -noclamp        do not clamp version number in client method stubs"
                );
            System.Console.Out.WriteLine("  -withcallinfo   supply call information to server method stubs"
                );
            System.Console.Out.WriteLine("  -initstrings    initialize all strings to be empty instead of null"
                );
            System.Console.Out.WriteLine("  -nobackup       do not make backups of old source code files"
                );
            System.Console.Out.WriteLine("  -noclient       do not create client proxy stub");
            System.Console.Out.WriteLine("  -noserver       do not create server proxy stub");
            System.Console.Out.WriteLine("  -parseonly      parse x-file only but do not create source code files"
                );
            System.Console.Out.WriteLine("  -verbose        enable verbose output about what jrpcgen is doing"
                );
            System.Console.Out.WriteLine("  -version        print jrpcgen version and exit");
            System.Console.Out.WriteLine("  -debug          enables printing of diagnostic messages"
                );
            System.Console.Out.WriteLine("  -? -help        print this help message and exit"
                );
            System.Console.Out.WriteLine("  --              end options");
            System.Console.Out.WriteLine();
        }

        /// <summary>Current version of jrpcgen.</summary>
        /// <remarks>Current version of jrpcgen.</remarks>
        public static readonly string VERSION = "1.0.7";

        /// <summary>
        /// A remote procedure has no parameters and thus needs to use the
        /// XDR void wrapper class as a dummy.
        /// </summary>
        /// <remarks>
        /// A remote procedure has no parameters and thus needs to use the
        /// XDR void wrapper class as a dummy.
        /// </remarks>
        public const int PARAMS_VOID = 0;

        /// <summary>
        /// A remote procedure expects only a single parameter, which is a
        /// complex type (class).
        /// </summary>
        /// <remarks>
        /// A remote procedure expects only a single parameter, which is a
        /// complex type (class).
        /// </remarks>
        public const int PARAMS_SINGLE = 1;

        /// <summary>
        /// A remote procedure expects only a single parameter, which is of
        /// a base type, like integer, boolean, string, et cetera.
        /// </summary>
        /// <remarks>
        /// A remote procedure expects only a single parameter, which is of
        /// a base type, like integer, boolean, string, et cetera.
        /// </remarks>
        public const int PARAMS_SINGLE_BASETYPE = 2;

        /// <summary>
        /// A remote procedure expects more than one parameter and thus needs
        /// an XDR wrapping class.
        /// </summary>
        /// <remarks>
        /// A remote procedure expects more than one parameter and thus needs
        /// an XDR wrapping class.
        /// </remarks>
        public const int PARAMS_MORE = 3;

        /// <summary>String containing date/time when a jrpcgen run was started.</summary>
        /// <remarks>
        /// String containing date/time when a jrpcgen run was started. This string
        /// is used in the headers of the generated source code files.
        /// </remarks>
        public static readonly string startDate = DateTime.Now.ToString("d");

        /// <summary>
        /// Contains all global identifiers for type, structure and union specifiers
        /// as well as for constants and enumeration members.
        /// </summary>
        /// <remarks>
        /// Contains all global identifiers for type, structure and union specifiers
        /// as well as for constants and enumeration members. This static attribute
        /// is directly manipulated by the parser.
        /// </remarks>
        public static System.Collections.Hashtable globalIdentifiers = new System.Collections.Hashtable
            ();

        /// <summary>Disable automatic backup of old source code files, if <code>true</code>.
        /// 	</summary>
        /// <remarks>Disable automatic backup of old source code files, if <code>true</code>.
        /// 	</remarks>
        public static bool noBackups = false;

        /// <summary>
        /// Holds information about the remote program defined in the jrpcgen
        /// x-file.
        /// </summary>
        /// <remarks>
        /// Holds information about the remote program defined in the jrpcgen
        /// x-file.
        /// </remarks>
        public static System.Collections.ArrayList programInfos = null;

        /// <summary>
        /// Clamp version and program number in client method stubs to the
        /// version and program number specified in the x-file.
        /// </summary>
        /// <remarks>
        /// Clamp version and program number in client method stubs to the
        /// version and program number specified in the x-file.
        /// </remarks>
        public static bool clampProgAndVers = true;

        /// <summary>Supply (additional) call information to server method stubs.</summary>
        /// <remarks>Supply (additional) call information to server method stubs.</remarks>
        public static bool withCallInfo = false;

        /// <summary>Enable diagnostic messages when parsing the x-file.</summary>
        /// <remarks>Enable diagnostic messages when parsing the x-file.</remarks>
        public static bool debug = false;

        /// <summary>Verbosity flag.</summary>
        /// <remarks>
        /// Verbosity flag. If <code>true</code>, then jrpcgen will report about
        /// the steps it is taking when generating all the source code files.
        /// </remarks>
        public static bool verbose = false;

        /// <summary>
        /// Parse x-file only but do not create source code files if set to
        /// <code>true</code>.
        /// </summary>
        /// <remarks>
        /// Parse x-file only but do not create source code files if set to
        /// <code>true</code>.
        /// </remarks>
        public static bool parseOnly = false;

        /// <summary>
        /// The x-file to parse (not: the X Files, the latter ones are something
        /// completely different).
        /// </summary>
        /// <remarks>
        /// The x-file to parse (not: the X Files, the latter ones are something
        /// completely different).
        /// </remarks>
        public static string xFile = null;

        /// <summary>Destination directory where to place the generated files.</summary>
        /// <remarks>Destination directory where to place the generated files.</remarks>
        public static string destinationDir = ".";

        /// <summary>Current FileWriter object receiving generated source code.</summary>
        /// <remarks>Current FileWriter object receiving generated source code.</remarks>
        public static FileStream currentFileWriter = null;

        /// <summary>
        /// Current PrintWriter object sitting on top of the
        /// <see cref="currentFileWriter">currentFileWriter</see>
        /// object receiving generated source code.
        /// </summary>
        public static StreamWriter currentPrintWriter = null;

        /// <summary>Full name of the current source code file.</summary>
        /// <remarks>Full name of the current source code file.</remarks>
        public static string currentFilename = null;

        /// <summary>
        /// Specifies package name for generated source code, if not
        /// <code>null</code>.
        /// </summary>
        /// <remarks>
        /// Specifies package name for generated source code, if not
        /// <code>null</code>. If <code>null</code>, then no package statement
        /// is emitted.
        /// </remarks>
        public static string packageName = null;

        /// <summary>Name of class containing global constants.</summary>
        /// <remarks>
        /// Name of class containing global constants. It is derived from the
        /// filename with the extension (".x") and path removed.
        /// </remarks>
        public static string baseClassname = null;

        /// <summary>
        /// Do not generate source code for the client proxy stub if
        /// <code>true</code>.
        /// </summary>
        /// <remarks>
        /// Do not generate source code for the client proxy stub if
        /// <code>true</code>.
        /// </remarks>
        public static bool noClient = false;

        /// <summary>
        /// Do not generate source code for the server proxy stub if
        /// <code>true</code>.
        /// </summary>
        /// <remarks>
        /// Do not generate source code for the server proxy stub if
        /// <code>true</code>.
        /// </remarks>
        public static bool noServer = false;

        /// <summary>Name of class containing the ONC/RPC server stubs.</summary>
        /// <remarks>Name of class containing the ONC/RPC server stubs.</remarks>
        public static string serverClass = null;

        /// <summary>Name of class containing the ONC/RPC client stubs.</summary>
        /// <remarks>Name of class containing the ONC/RPC client stubs.</remarks>
        public static string clientClass = null;

        /// <summary>Enable tagging of XDR classes as being Serializable</summary>
        public static bool makeSerializable = false;

        /// <summary>Enable generation of accessors in order to use XDR classes as beans.</summary>
        /// <remarks>Enable generation of accessors in order to use XDR classes as beans.</remarks>
        public static bool makeBean = false;

        /// <summary>
        /// Enable automatic initialization of String with empty Strings
        /// instead of null reference.
        /// </summary>
        /// <remarks>
        /// Enable automatic initialization of String with empty Strings
        /// instead of null reference.
        /// </remarks>
        public static bool initStrings = false;

        /// <summary>
        /// Creates a new source code file for a Java class based on its class
        /// name.
        /// </summary>
        /// <remarks>
        /// Creates a new source code file for a Java class based on its class
        /// name. Same as
        /// <see cref="createJavaSourceFile(string, bool)">createJavaSourceFile(string, bool)
        /// 	</see>
        /// with
        /// the <code>emitImport</code> parameter set to <code>true</code>.
        /// </remarks>
        /// <param name="classname">
        /// Name of Java class to generate. Must not contain
        /// a file extension -- especially ".java" is invalid. When the source
        /// code file is created, ".java" is appended automatically.
        /// </param>
        /// <returns>PrintWriter to send source code to.</returns>
        public static StreamWriter createJavaSourceFile(string classname)
        {
            return createJavaSourceFile(classname, true);
        }

        /// <summary>
        /// Creates a new source code file for a Java class based on its class
        /// name.
        /// </summary>
        /// <remarks>
        /// Creates a new source code file for a Java class based on its class
        /// name. If an old version of the source file exists, it is renamed first.
        /// The backup will have the same name as the original file with "~"
        /// appended.
        /// </remarks>
        /// <param name="classname">
        /// Name of Java class to generate. Must not contain
        /// a file extension -- especially ".java" is invalid. When the source
        /// code file is created, ".java" is appended automatically.
        /// </param>
        /// <param name="emitImports">
        /// if <code>true</code>, then import statements for
        /// the remotetea ONC/RPC package and IOExceptions.
        /// </param>
        /// <returns>PrintWriter to send source code to.</returns>
        public static StreamWriter createJavaSourceFile(string classname, bool emitImports
            )
        {
            string filename = classname + ".cs";
            if (debug)
            {
                System.Console.Out.WriteLine("Generating source code for \"" + filename + "\" in \""
                     + destinationDir + "\"");
            }
            string filePath = destinationDir + "/" + filename;
            //
            // If an old file of the same name already exists, then rename it
            // before creating the new file.
            //
            if (File.Exists(filePath) && !noBackups)
            {
                FileAttributes attrs = File.GetAttributes(filePath);
                if ((~(FileAttributes.Normal | FileAttributes.Archive) & attrs) != 0)
                {
                    //
                    // If the file to be created already exists and is not a
                    // regular file, then bail out with an error.
                    //
                    System.Console.Error.WriteLine("error: source file \"" + filename + "\"already exists and is not a regular file"
                        );
                    System.Environment.Exit(1);
                }
                // Now create a backup by adding '~' to the filename
                string backupPath = filePath + "~";
                if (File.Exists(backupPath))
                {
                    attrs = File.GetAttributes(backupPath);
                    if ((~(FileAttributes.Normal | FileAttributes.Archive) & attrs) != 0)
                    {
                        System.Console.Error.WriteLine("error: backup source file \"" + filename + "~\" is not a regular file"
                            );
                        System.Environment.Exit(1);
                    }
                    else
                    {
                        File.Delete(backupPath);
                    }
                }
                try
                {
                    File.Move(filePath, backupPath);
                }
                catch (Exception)
                {
                    System.Console.Error.WriteLine("error: can not rename old source code file \"" +
                        filename + "\"");
                    System.Environment.Exit(1);
                }

                if (verbose)
                {
                    System.Console.Out.WriteLine("Saved old source code file as \"" + filename + "~\""
                        );
                }
            }
            //
            // Now create a new source code file...
            //
            try
            {
                currentFileWriter = File.OpenWrite(filePath);
            }
            catch (System.IO.IOException e)
            {
                System.Console.Error.WriteLine("error: can not create \"" + filename + "\": " + e
                    .Message);
                System.Environment.Exit(1);
            }
            if (verbose)
            {
                System.Console.Out.Write("Creating source code file \"" + filename + "\"...");
            }
            currentFilename = filename;
            currentPrintWriter = new StreamWriter(currentFileWriter);
            //
            // Create automatic header(s)...
            // Note that we always emit the import statements, regardless of
            // whether we're emitting a class file or an interface file consisting
            // of an enumeration.
            //
            currentPrintWriter.WriteLine("/*");
            currentPrintWriter.WriteLine(" * Automatically generated by jrpcgen " + VERSION + " on " + startDate
                );
            currentPrintWriter.WriteLine(" * jrpcgen is part of the \"Remote Tea.Net\" ONC/RPC package for C#");
            currentPrintWriter.WriteLine(" * See http://remotetea.sourceforge.net for details");
            currentPrintWriter.WriteLine(" */");
            //
            // Only generated package statement if a package name has been specified.
            //
            if ((packageName != null) && (packageName.Length > 0))
            {
                currentPrintWriter.WriteLine("namespace " + packageName + "{");
            }
            if (emitImports)
            {
                currentPrintWriter.WriteLine("using org.acplt.oncrpc;");
                currentPrintWriter.WriteLine();
            }
            return currentPrintWriter;
        }

        /// <summary>
        /// Create a new hash function object and initialize it using a class
        /// and package name.
        /// </summary>
        /// <remarks>
        /// Create a new hash function object and initialize it using a class
        /// and package name.
        /// </remarks>
        /// <param name="classname">Name of class.</param>
        /// <returns>hash function object.</returns>
        public static org.acplt.oncrpc.apps.jrpcgen.JrpcgenSHA createSHA(string classname
            )
        {
            org.acplt.oncrpc.apps.jrpcgen.JrpcgenSHA hash = new org.acplt.oncrpc.apps.jrpcgen.JrpcgenSHA
                ();
            if ((packageName != null) && (packageName.Length > 0))
            {
                hash.update(packageName + "." + classname);
            }
            else
            {
                hash.update(classname);
            }
            return hash;
        }

        /// <summary>
        /// Closes the source code file previously opened with
        /// <code>createJavaSourceFile</code>.
        /// </summary>
        /// <remarks>
        /// Closes the source code file previously opened with
        /// <code>createJavaSourceFile</code>. This method writes a trailer
        /// before closing the file.
        /// </remarks>
        public static void closeJavaSourceFile()
        {
            //
            // Create automatic footer before closing the file.
            //
            currentPrintWriter.WriteLine("// End of " + currentFilename);
            if (verbose)
            {
                System.Console.Out.WriteLine();
            }
            try
            {
                currentPrintWriter.Close();
                currentFileWriter.Close();
            }
            catch (System.IO.IOException e)
            {
                System.Console.Error.WriteLine("Can not close source code file: " + e.Message);
            }
        }

        /// <summary>
        /// Dump the value of a constant and optionally first dump all constants
        /// it depends on.
        /// </summary>
        /// <remarks>
        /// Dump the value of a constant and optionally first dump all constants
        /// it depends on.
        /// </remarks>
        public static void dumpConstantAndDependency(StreamWriter @out, org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst
             c)
        {
            //
            // This simple test avoids endless recursions: we already dumped this
            // particular constant in some place, so we should not proceed.
            //
            if (c.dontTraverseAnyMore)
            {
                return;
            }
            //
            // Since we will dump the constant below, we already set the flag,
            // to avoid endless recursions.
            //
            c.dontTraverseAnyMore = true;
            string dependencyIdentifier = c.getDependencyIdentifier();
            if (dependencyIdentifier != null)
            {
                //
                // There is a dependency, so try to resolve that first. In case
                // we depend on another identifier belonging to the same enclosure,
                // we dump this other identifier first. However, if the identifier
                // we depend on belongs to a different enclosure, then we must not
                // dump it: this will be the job of a later call when the proper
                // enclosure is in the works.
                //
                org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst dc = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst
                    )globalIdentifiers[dependencyIdentifier];
                if (dc != null)
                {
                    if (string.Compare(c.enclosure, dc.enclosure, true) != 0)
                    {
                        //
                        // In case we depend on a constant which belongs to a
                        // different enclosure then also dump the enclosure (that
                        // is, "enclosure.valueidentifier").
                        //
                        // Note that this code depends on the "value" starts
                        // with the identifier we depend on (which is currently
                        // the case), so we just need to prepend the enclosure.
                        //
                        @out.WriteLine("    public const int " + c.identifier + " = " + dc.enclosure
                             + "." + c.value + ";");
                        return;
                    }
                    //
                    // Only dump the identifier we're dependent on, if it's in
                    // the same enclosure.
                    //
                    dumpConstantAndDependency(@out, dc);
                }
            }
            //
            // Just dump the plain value (without enclosure).
            //
            @out.WriteLine("    public const int " + c.identifier + " = " + c.value + ";"
                );
        }

        /// <summary>
        /// Generate source code file containing all constants defined in the
        /// x-file as well as all implicitely defined constants, like program,
        /// version and procedure numbers, etc.
        /// </summary>
        /// <remarks>
        /// Generate source code file containing all constants defined in the
        /// x-file as well as all implicitely defined constants, like program,
        /// version and procedure numbers, etc. This method creates a public
        /// interface with the constants as public static final integers.
        /// </remarks>
        public static void dumpConstants()
        {
            //
            // Create new source code file containing a Java interface representing
            // all XDR constants.
            //
            StreamWriter @out = createJavaSourceFile(baseClassname, false);
            //
            // Spit out some description for javadoc & friends...
            //
            @out.WriteLine("/**");
            @out.WriteLine(" * A collection of constants used by the \"" + baseClassname + "\" ONC/RPC program."
                );
            @out.WriteLine(" */");
            @out.WriteLine("public class " + baseClassname + " {");
            System.Collections.IEnumerator globals = globalIdentifiers.Values.GetEnumerator();
            while (globals.MoveNext())
            {
                object o = globals.Current;
                if (o is org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst)
                {
                    org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst c = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst
                        )o;
                    //
                    // Dump only such constants which belong to the global
                    // constants enclosure. Ignore all other constants, as those
                    // belong to other Java class enclosures.
                    //
                    if (baseClassname.Equals(c.enclosure))
                    {
                        dumpConstantAndDependency(@out, c);
                    }
                }
            }
            @out.WriteLine("}");
            closeJavaSourceFile();
        }

        /// <summary>
        /// Generate a source code file containing all elements of an enumeration
        /// defined in a x-file.
        /// </summary>
        /// <remarks>
        /// Generate a source code file containing all elements of an enumeration
        /// defined in a x-file.
        /// </remarks>
        /// <param name="e">
        /// 
        /// <see cref="JrpcgenEnum">Description</see>
        /// of XDR enumeration.
        /// </param>
        public static void dumpEnum(org.acplt.oncrpc.apps.jrpcgen.JrpcgenEnum e)
        {
            //
            // Create new source code file containing a Java interface representing
            // the XDR enumeration.
            //
            StreamWriter @out = createJavaSourceFile(e.identifier, false);
            //
            // Spit out some description for javadoc & friends...
            //
            @out.WriteLine("/**");
            @out.WriteLine(" * Enumeration (collection of constants).");
            @out.WriteLine(" */");
            @out.WriteLine("public class " + e.identifier + " {");
            @out.WriteLine();
            System.Collections.IEnumerator enums = e.enums.GetEnumerator();
            while (enums.MoveNext())
            {
                org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst c = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst
                    )enums.Current;
                //
                // In case an element depends on a global constant, then
                // this constant will automatically be duplicated as part
                // of this enumeration.
                //
                dumpConstantAndDependency(@out, c);
            }
            //
            // Close class...
            //
            @out.WriteLine();
            @out.WriteLine("}");
            closeJavaSourceFile();
        }

        /// <summary>
        /// Java base data types for which are XDR encoding and decoding helper
        /// methods available.
        /// </summary>
        /// <remarks>
        /// Java base data types for which are XDR encoding and decoding helper
        /// methods available.
        /// </remarks>
        private static string[] baseTypes = new string[] { "void", "bool", "byte", "short"
			, "int", "long", "float", "double", "string" };

        /// <summary>
        /// Given a name of a data type return the name of the equivalent Java
        /// data type (if it exists), otherwise return <code>null</code>.
        /// </summary>
        /// <remarks>
        /// Given a name of a data type return the name of the equivalent Java
        /// data type (if it exists), otherwise return <code>null</code>.
        /// NOTE: "opaque" is considered like "byte" to be a base type...
        /// FIXME: char/byte?
        /// </remarks>
        /// <returns>
        /// Name of Java base data type or <code>null</code> if the
        /// given data type is not equivalent to one of Java's base data
        /// types.
        /// </returns>
        public static string xdrBaseType(string type)
        {
            int size = baseTypes.Length;
            if ("opaque".CompareTo(type) == 0)
            {
                type = "byte";
            }
            else if ("bool".CompareTo(type) == 0)
            {
                return "XdrBoolean";
            }

            for (int idx = 0; idx < size; ++idx)
            {
                if (baseTypes[idx].CompareTo(type) == 0)
                {
                    //
                    // For base data types simply convert the first letter to
                    // an upper case letter.
                    //
                    return "Xdr" + type.Substring(0, 1).ToUpper() + type.Substring(1);
                }
            }
            return null;
        }

        /// <summary>
        /// Return the en-/decoding syllable XXX appropriate for a base data
        /// type including arrays of base data types.
        /// </summary>
        /// <remarks>
        /// Return the en-/decoding syllable XXX appropriate for a base data
        /// type including arrays of base data types.
        /// </remarks>
        /// <param name="decl">declaration of a member of RPC struct or union.</param>
        /// <returns>
        /// <code>null</code>, if the declaration does not specify a base data
        /// type. Otherwise a three-element String array, with [0] containing
        /// the type syllable for base type (including arrays), [1] containing
        /// parameter options when encoding (like maximum sizes, etc), and [2]
        /// containing options for decoding.
        /// </returns>
        internal static org.acplt.oncrpc.apps.jrpcgen.JrpcgenEnDecodingInfo baseEnDecodingSyllable
            (org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration decl)
        {
            string syllable = decl.type;
            bool isBase = false;
            //
            // Check for Java base data types... if a match is found, then convert
            // the data type name, so that it becomes a valid syllable for use
            // with XDR en-/decoding functions xdrEncodingXXX() etc.
            // Example: "int" --> "Int" (because of xdrEncodingInt())
            // NOTE: we consider "opaque" to be a base type here...
            //
            int size = baseTypes.Length;
            string type = decl.type;
            if ("opaque".CompareTo(type) == 0)
            {
                type = "byte";
            }
            for (int idx = 0; idx < size; ++idx)
            {
                if (baseTypes[idx].CompareTo(type) == 0)
                {
                    //
                    // For base data types simply convert the first letter to
                    // an upper case letter.
                    //
                    isBase = true;
                    if ("bool" == type)
                    {
                        syllable = "Boolean";
                    }
                    else
                    {
                        syllable = syllable.Substring(0, 1).ToUpper() + syllable.Substring(1);
                    }
                    break;
                }
            }
            //
            // Handle special case of enumerations, which have to be represented
            // using ints in the Java language.
            //
            if (!isBase)
            {
                object o = globalIdentifiers[decl.type];
                if (o is org.acplt.oncrpc.apps.jrpcgen.JrpcgenEnum)
                {
                    isBase = true;
                    syllable = "Int";
                }
            }
            //
            // In case we're dealing with an array, then add "Vector" to
            // the syllable to use the appropriate vector en-/decoding method
            // for base data types.
            // NOTE: unfortunately, strings do not adhere to this scheme, as
            // they are considered to be arrays of characters... what a silly
            // idea, as this makes a typedef necessary in case someone needs
            // an array of strings.
            // NOTE: also opaques break the scheme somehow, but the char=byte
            // versus opaque schisma anyhow drives me crazy...
            //
            if (isBase)
            {
                string encodingOpts = null;
                string decodingOpts = null;
                if ((decl.kind == org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration.FIXEDVECTOR) ||
                     (decl.kind == org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration.DYNAMICVECTOR))
                {
                    if ("opaque".Equals(decl.type))
                    {
                        if (decl.kind == org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration.FIXEDVECTOR)
                        {
                            syllable = "Opaque";
                            encodingOpts = checkForEnumValue(decl.size);
                            decodingOpts = checkForEnumValue(decl.size);
                        }
                        else
                        {
                            syllable = "DynamicOpaque";
                            encodingOpts = null;
                            decodingOpts = null;
                        }
                    }
                    else
                    {
                        if (!"string".Equals(decl.type))
                        {
                            if (decl.kind == org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration.FIXEDVECTOR)
                            {
                                syllable = syllable + "Fixed";
                                encodingOpts = checkForEnumValue(decl.size);
                                decodingOpts = checkForEnumValue(decl.size);
                            }
                            syllable = syllable + "Vector";
                        }
                    }
                }
                org.acplt.oncrpc.apps.jrpcgen.JrpcgenEnDecodingInfo result = new org.acplt.oncrpc.apps.jrpcgen.JrpcgenEnDecodingInfo
                    (syllable, encodingOpts, decodingOpts);
                return result;
            }
            return null;
        }

        /// <summary>Return en- or decoding method appropriate for a struct or union member.</summary>
        /// <remarks>Return en- or decoding method appropriate for a struct or union member.</remarks>
        public static string codingMethod(org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration
             decl, bool encode)
        {
            return codingMethod(decl, encode, null);
        }

        /// <summary>Return en- or decoding method appropriate for a struct or union member.</summary>
        /// <remarks>Return en- or decoding method appropriate for a struct or union member.</remarks>
        /// <param name="decl">
        /// declaration for which the en-/decoding Java source code be
        /// returned.
        /// </param>
        /// <param name="encode">
        /// <code>true</code> if encoding method should be returned,
        /// <code>false</code> if decoding method is to be returned.
        /// </param>
        /// <param name="oref">
        /// name of object reference or <code>null</code> if
        /// "this" should be used instead.
        /// </param>
        public static string codingMethod(org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration
             decl, bool encode, string oref)
        {
            //
            // Skip entries for void arms etc...
            //
            if (decl.identifier == null)
            {
                return string.Empty;
            }
            System.Text.StringBuilder code = new System.Text.StringBuilder();
            JrpcgenEnDecodingInfo data = baseEnDecodingSyllable(decl);
            //
            // In case no type was specified for the outer element, assume no
            // name, otherwise convert into a suitable prefix for code generation
            // by appending a dot.
            //
            if (oref == null)
            {
                oref = string.Empty;
            }
            else
            {
                oref = oref + ".";
            }
            if (data != null)
            {
                //
                // It's a base data type (including vectors). So we can use the
                // predefined en-/decoding methods:
                //   - xdr.xdrEncodeXXX(value);
                //   - value = xdr.xdrDecodeXXX(value);
                //
                if (encode)
                {
                    code.Append("        xdr.xdrEncode");
                    code.Append(data.syllable);
                    code.Append("(");
                    code.Append(oref + decl.identifier);
                    if (data.encodingOptions != null)
                    {
                        code.Append(", ");
                        code.Append(data.encodingOptions);
                    }
                    code.Append(");\n");
                }
                else
                {
                    code.Append("        ");
                    code.Append(oref + decl.identifier);
                    code.Append(" = xdr.xdrDecode");
                    code.Append(data.syllable);
                    code.Append("(");
                    if (data.decodingOptions != null)
                    {
                        code.Append(data.decodingOptions);
                    }
                    code.Append(");\n");
                }
                return code.ToString();
            }
            else
            {
                //
                // It's not a built-in base data type but instead something that
                // is represented by a class.
                //   - foo.xdrEncode(xdr);
                //   - foo = new FOO();
                //     foo.xdrDecode(xdr);
                // In case of arrays, this is going to be hairy...
                //
                if (decl.kind == JrpcgenDeclaration.SCALAR)
                {
                    code.Append("        ");
                    if (encode)
                    {
                        code.Append(oref + decl.identifier);
                        code.Append(".xdrEncode(xdr);\n");
                    }
                    else
                    {
                        code.Append(oref + decl.identifier);
                        code.Append(" = new ");
                        code.Append(decl.type);
                        code.Append("(xdr);\n");
                    }
                    return code.ToString();
                }
                else
                {
                    //
                    // It's not a built-in base data type but instead an indirection
                    // (reference) to some instance (optional data).
                    //
                    if (decl.kind == JrpcgenDeclaration.INDIRECTION)
                    {
                        code.Append("        ");
                        if (encode)
                        {
                            code.Append("if ( ");
                            code.Append(oref + decl.identifier);
                            code.Append(" != null ) { ");
                            code.Append("xdr.xdrEncodeBoolean(true); ");
                            code.Append(oref + decl.identifier);
                            code.Append(".xdrEncode(xdr);");
                            code.Append(" } else { ");
                            code.Append("xdr.xdrEncodeBoolean(false);");
                            code.Append(" };\n");
                        }
                        else
                        {
                            code.Append(oref + decl.identifier);
                            code.Append(" = xdr.xdrDecodeBoolean() ? new ");
                            code.Append(decl.type);
                            code.Append("(xdr) : null;\n");
                        }
                        return code.ToString();
                    }
                }
                //
                // Array case... Urgh!
                //
                if (encode)
                {
                    code.Append("        { ");
                    code.Append("int _size = ");
                    if (decl.kind == JrpcgenDeclaration.DYNAMICVECTOR)
                    {
                        //
                        // Dynamic array size. So we need to use the current size
                        // of the Java array.
                        //
                        code.Append(oref + decl.identifier);
                        code.Append(".Length");
                    }
                    else
                    {
                        code.Append(checkForEnumValue(decl.size));
                    }
                    code.Append("; ");
                    if (decl.kind == JrpcgenDeclaration.DYNAMICVECTOR)
                    {
                        //
                        // Dynamic array size. So we need to encode size information.
                        //
                        code.Append("xdr.xdrEncodeInt(_size); ");
                    }
                    //
                    // Now encode all elements.
                    //
                    code.Append("for ( int _idx = 0; _idx < _size; ++_idx ) { ");
                    code.Append(oref + decl.identifier);
                    code.Append("[_idx].xdrEncode(xdr); ");
                    code.Append("} }\n");
                }
                else
                {
                    code.Append("        { ");
                    code.Append("int _size = ");
                    if (decl.kind == JrpcgenDeclaration.DYNAMICVECTOR)
                    {
                        //
                        // Dynamic array size. So we need to decode size information.
                        //
                        code.Append("xdr.xdrDecodeInt()");
                    }
                    else
                    {
                        code.Append(checkForEnumValue(decl.size));
                    }
                    code.Append("; ");
                    //
                    // Now encode all elements.
                    //
                    code.Append(oref + decl.identifier);
                    code.Append(" = new ");
                    code.Append(decl.type);
                    code.Append("[_size]; ");
                    code.Append("for ( int _idx = 0; _idx < _size; ++_idx ) { ");
                    code.Append(oref + decl.identifier);
                    code.Append("[_idx] = new ");
                    code.Append(decl.type);
                    code.Append("(xdr); ");
                    code.Append("} }\n");
                }
                return code.ToString();
            }
        }

        /// <summary>
        /// Checks whether a given data type identifier refers to an enumeration
        /// type and then returns Java's int data type instead.
        /// </summary>
        /// <remarks>
        /// Checks whether a given data type identifier refers to an enumeration
        /// type and then returns Java's int data type instead. In case of the
        /// pseudo-type "opaque" return Java's byte data type. For all other
        /// data types, the data type identifier is returned unaltered.
        /// </remarks>
        /// <param name="dataType">data type identifier to check.</param>
        /// <returns>data type identifier.</returns>
        public static string checkForSpecials(string dataType)
        {
            if (globalIdentifiers[dataType] is org.acplt.oncrpc.apps.jrpcgen.JrpcgenEnum)
            {
                return "int";
            }
            else
            {
                if ("opaque".Equals(dataType))
                {
                    return "byte";
                }
            }
            return dataType;
        }

        /// <summary>
        /// Checks whether a given value references an identifier and then
        /// returns the qualified identifier (interface where the value is
        /// defined in) or simply the value in case of an integer literal.
        /// </summary>
        /// <remarks>
        /// Checks whether a given value references an identifier and then
        /// returns the qualified identifier (interface where the value is
        /// defined in) or simply the value in case of an integer literal.
        /// </remarks>
        /// <param name="value">Either an identifier to resolve or an integer literal.</param>
        /// <returns>Integer literal or qualified identifier.</returns>
        public static string checkForEnumValue(string value)
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
                // It's an identifier: we now need to find out in which
                // enclosure it lives, so we can return a qualified identifier.
                //
                object id = org.acplt.oncrpc.apps.jrpcgen.jrpcgen.globalIdentifiers[value];
                if ((id != null) && (id is org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst))
                {
                    org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst c = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst
                        )id;
                    if (c.enclosure == null)
                    {
                        return c.value;
                    }
                    return c.enclosure + "." + c.identifier;
                }
            }
            return value;
        }

        /// <summary>
        /// Generate a source code file containing all elements of a struct
        /// defined in a x-file.
        /// </summary>
        /// <remarks>
        /// Generate a source code file containing all elements of a struct
        /// defined in a x-file.
        /// </remarks>
        /// <param name="s">
        /// 
        /// <see cref="JrpcgenStruct">Description</see>
        /// of XDR struct.
        /// </param>
        public static void dumpStruct(org.acplt.oncrpc.apps.jrpcgen.JrpcgenStruct s)
        {
            //
            // Create new source code file containing a Java class representing
            // the XDR struct.
            //
            string access = "    public ";
            // modify encapsulation with beans
            StreamWriter @out = createJavaSourceFile(s.identifier);
            if (makeSerializable)
            {
                @out.Write("[Serializable]");
            }
            @out.Write("public class " + s.identifier + " : XdrAble");
            @out.WriteLine(" {");
            //
            // Generate declarations of all members of this XDR struct. This the
            // perfect place to also update the hash function using the elements
            // together with their type.
            //
            bool useIteration = false;
            JrpcgenSHA hash = createSHA(s.identifier);
            for (int i = 0; i < s.elements.Count; ++i)
            {
                JrpcgenDeclaration d = (JrpcgenDeclaration
                    )s.elements[i];
                hash.update(d.type);
                hash.update(d.kind);
                hash.update(d.identifier);
                @out.Write(access + checkForSpecials(d.type) + " ");
                if (((d.kind == JrpcgenDeclaration.FIXEDVECTOR) ||
                    (d.kind == JrpcgenDeclaration.DYNAMICVECTOR)) && !
                    d.type.Equals("string"))
                {
                    @out.Write("[] ");
                }
                if (initStrings && d.type.Equals("string"))
                {
                    @out.WriteLine(d.identifier + " = \"\"; ");
                }
                else
                {
                    @out.WriteLine(d.identifier + ";");
                }
                //
                // If the last element in the XDR struct is a reference to
                // the type of the XDR struct (that is, a linked list), then
                // we can convert this tail recursion into an iteration,
                // avoiding deep recursions for large lists.
                //
                if (i == (s.elements.Count - 1) && d.kind == org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration
                    .INDIRECTION && d.type.Equals(s.identifier))
                {
                    useIteration = true;
                }
            }
            //
            // Generate serial version unique identifier
            //
            if (makeSerializable)
            {
                @out.WriteLine();
                @out.WriteLine("    private static readonly long serialVersionUID = " + hash.getHash()
                     + "L;");
                if (makeBean)
                {
                    //
                    // Also generate accessors (getters and setters) so that
                    // class can be used as a bean.
                    //
                    for (int i = 0; i < s.elements.Count; ++i)
                    {
                        @out.WriteLine();
                        org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration d = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration
                            )s.elements[i];
                        string jbName = d.identifier.Substring(0, 1).ToUpper() + d.identifier.Substring(1);
                        bool isArray = (((d.kind == org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration.FIXEDVECTOR
                            ) || (d.kind == org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration.DYNAMICVECTOR))
                             && !d.type.Equals("string"));
                        //
                        // Generate the setter(s)
                        //
                        if (isArray)
                        {
                            @out.WriteLine("    public void set" + jbName + "(" + checkForSpecials(d.type) + "[] x) { this."
                                 + d.identifier + " = x; }");
                            @out.WriteLine("    public void set" + jbName + "(int index, " + checkForSpecials(d
                                .type) + " x) { this." + d.identifier + "[index] = x; }");
                        }
                        else
                        {
                            @out.WriteLine("    public void set" + jbName + "(" + checkForSpecials(d.type) + " x) { this."
                                 + d.identifier + " = x; }");
                        }
                        //
                        // Generate the getter(s)
                        //
                        if (isArray)
                        {
                            @out.WriteLine("    public " + checkForSpecials(d.type) + "[] get" + jbName + "() { return this."
                                 + d.identifier + "; }");
                            @out.WriteLine("    public " + checkForSpecials(d.type) + " get" + jbName + "(int index) { return this."
                                 + d.identifier + "[index]; }");
                        }
                        else
                        {
                            @out.WriteLine("    public " + checkForSpecials(d.type) + " get" + jbName + "() { return this."
                                 + d.identifier + "; }");
                        }
                    }
                }
            }
            //
            // Now generate code for encoding and decoding this class (structure).
            //
            @out.WriteLine();
            @out.WriteLine("    public " + s.identifier + "() {");
            @out.WriteLine("    }");
            @out.WriteLine();
            @out.WriteLine("    public " + s.identifier + "(XdrDecodingStream xdr) {");
            @out.WriteLine("        xdrDecode(xdr);");
            @out.WriteLine("    }");
            @out.WriteLine();
            @out.WriteLine("    public void xdrEncode(XdrEncodingStream xdr) {");
            IEnumerator decls = s.elements.GetEnumerator();
            if (useIteration)
            {
                @out.WriteLine("        " + s.identifier + " _this = this;");
                @out.WriteLine("        do {");
                org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration decl = null;
                //
                // when using the iteration loop for serializing emit code for
                // all but the tail element, which is the reference to our type.
                //
                decls.MoveNext();
                for (int size = s.elements.Count; size > 1; --size)
                {
                    decl = (JrpcgenDeclaration)decls.Current;
                    @out.Write("    " + codingMethod(decl, true, "_this"));
                    decls.MoveNext();
                }
                decl = (JrpcgenDeclaration)decls.Current;
                @out.WriteLine("            _this = _this." + decl.identifier + ";");
                @out.WriteLine("            xdr.xdrEncodeBoolean(_this != null);");
                @out.WriteLine("        } while ( _this != null );");
            }
            else
            {
                while (decls.MoveNext())
                {
                    @out.Write(codingMethod((JrpcgenDeclaration)decls.Current
                        , true));
                }
            }
            @out.WriteLine("    }");
            @out.WriteLine();
            @out.WriteLine("    public void xdrDecode(XdrDecodingStream xdr) {");
            decls = s.elements.GetEnumerator();
            if (useIteration)
            {
                @out.WriteLine("        " + s.identifier + " _this = this;");
                @out.WriteLine("        " + s.identifier + " _next;");
                @out.WriteLine("        do {");
                JrpcgenDeclaration decl = null;
                //
                // when using the iteration loop for serializing emit code for
                // all but the tail element, which is the reference to our type.
                //
                decls.MoveNext();
                for (int size = s.elements.Count; size > 1; --size)
                {
                    decl = (JrpcgenDeclaration)decls.Current;
                    @out.Write("    " + codingMethod(decl, false, "_this"));
                    decls.MoveNext();
                }
                decl = (JrpcgenDeclaration)decls.Current;
                @out.WriteLine("            _next = xdr.xdrDecodeBoolean() ? new " + s.identifier +
                     "() : null;");
                @out.WriteLine("            _this." + decl.identifier + " = _next;");
                @out.WriteLine("            _this = _next;");
                @out.WriteLine("        } while ( _this != null );");
            }
            else
            {
                while (decls.MoveNext())
                {
                    @out.Write(codingMethod((JrpcgenDeclaration)decls.Current
                        , false));
                }
            }
            @out.WriteLine("    }");
            //
            // Close class...
            //
            @out.WriteLine();
            @out.WriteLine("}");
            closeJavaSourceFile();
        }

        /// <summary>
        /// Generate a source code file containing all elements of a union
        /// defined in a x-file.
        /// </summary>
        /// <remarks>
        /// Generate a source code file containing all elements of a union
        /// defined in a x-file.
        /// </remarks>
        /// <param name="u">
        /// 
        /// <see cref="JrpcgenUnion">Description</see>
        /// of XDR union.
        /// </param>
        public static void dumpUnion(org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnion u)
        {
            //
            // Create new source code file containing a Java class representing
            // the XDR union.
            //
            StreamWriter @out = createJavaSourceFile(u.identifier);
            @out.Write("public class " + u.identifier + " : XdrAble");
            if (makeSerializable)
            {
                @out.Write(", java.io.Serializable");
            }
            @out.WriteLine(" {");
            //
            // Note that the descriminant can not be of an array type, string, etc.
            // so we don't have to handle all the special cases here.
            //
            @out.WriteLine("    public " + checkForSpecials(u.descriminant.type) + " " + u.descriminant
                .identifier + ";");
            bool boolDescriminant = u.descriminant.type.Equals("bool");
            org.acplt.oncrpc.apps.jrpcgen.JrpcgenSHA hash = createSHA(u.identifier);
            System.Collections.IEnumerator arms = u.elements.GetEnumerator();
            while (arms.MoveNext())
            {
                org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnionArm a = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnionArm
                    )arms.Current;
                //
                // Skip all arms which do not contain a variable but are
                // declared as "void" instead. Also skip all arms which are
                // mapped to another arm.
                //
                if ((a.element == null) || (a.element.identifier == null))
                {
                    continue;
                }
                //
                // In case we are working on the default arm and this arm
                // contains some variables, we hash the dummy descriminator
                // value "default".
                //
                if (a.value != null)
                {
                    hash.update(a.value);
                }
                else
                {
                    hash.update("default");
                }
                hash.update(a.element.type);
                hash.update(a.element.kind);
                hash.update(a.element.identifier);
                @out.Write("    public " + checkForSpecials(a.element.type) + " ");
                if (((a.element.kind == org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration.FIXEDVECTOR
                    ) || (a.element.kind == org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration.DYNAMICVECTOR
                    )) && !a.element.type.Equals("string"))
                {
                    @out.Write("[] ");
                }
                @out.WriteLine(a.element.identifier + ";");
            }
            //
            // Generate serial version unique identifier
            //
            if (makeSerializable)
            {
                @out.WriteLine();
                @out.WriteLine("    private static readonly long serialVersionUID = " + hash.getHash()
                     + "L;");
            }
            //
            // Now generate code for encoding and decoding this class (structure).
            //
            @out.WriteLine();
            @out.WriteLine("    public " + u.identifier + "() {");
            @out.WriteLine("    }");
            @out.WriteLine();
            @out.WriteLine("    public " + u.identifier + "(XdrDecodingStream xdr) {");
            @out.WriteLine("        xdrDecode(xdr);");
            @out.WriteLine("    }");
            @out.WriteLine();
            @out.WriteLine("    public void xdrEncode(XdrEncodingStream xdr) {");
            @out.Write(codingMethod(u.descriminant, true));
            if (!boolDescriminant)
            {
                //
                // Produce code using an ordinary switch statement...
                //
                @out.WriteLine("        switch ( " + u.descriminant.identifier + " ) {");
                arms = u.elements.GetEnumerator();
                while (arms.MoveNext())
                {
                    org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnionArm a = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnionArm
                        )arms.Current;
                    if (a.value != null)
                    {
                        @out.WriteLine("        case " + checkForEnumValue(a.value) + ":");
                    }
                    else
                    {
                        //
                        // It's the default arm.
                        //
                        @out.WriteLine("        default:");
                    }
                    //
                    // Only emit code if arm does not map to another arm.
                    //
                    if (a.element != null)
                    {
                        if (a.element.identifier != null)
                        {
                            //
                            // Arm does not contain void, so we need to spit out
                            // encoding instructions.
                            //
                            @out.Write("    ");
                            @out.Write(codingMethod(a.element, true));
                        }
                        @out.WriteLine("            break;");
                    }
                }
                @out.WriteLine("        }");
            }
            else
            {
                //
                // boolean descriminant: here we can have at most two arms, guess
                // why.
                //
                bool firstArm = true;
                arms = u.elements.GetEnumerator();
                while (arms.MoveNext())
                {
                    org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnionArm a = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnionArm
                        )arms.Current;
                    if (a.value == null)
                    {
                        //
                        // Skip default branch this time...
                        //
                        continue;
                    }
                    if (a.element.identifier != null)
                    {
                        //
                        // Arm contains data, so we need to create encoding
                        // instructions.
                        //
                        @out.Write("        ");
                        if (!firstArm)
                        {
                            @out.Write("else ");
                        }
                        else
                        {
                            firstArm = false;
                        }
                        @out.WriteLine("if ( " + u.descriminant.identifier + " == " + checkForEnumValue(a.value
                            ) + " ) {");
                        @out.Write("    ");
                        @out.Write(codingMethod(a.element, true));
                        @out.WriteLine("        }");
                    }
                }
                arms = u.elements.GetEnumerator();
                while (arms.MoveNext())
                {
                    org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnionArm a = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnionArm
                        )arms.Current;
                    if ((a.value == null) && (a.element.identifier != null))
                    {
                        @out.Write("        ");
                        if (!firstArm)
                        {
                            @out.Write("else ");
                        }
                        @out.WriteLine("{");
                        @out.Write("    ");
                        @out.Write(codingMethod(a.element, true));
                        @out.WriteLine("        }");
                    }
                }
            }
            @out.WriteLine("    }");
            @out.WriteLine();
            @out.WriteLine("    public void xdrDecode(XdrDecodingStream xdr) {");
            @out.Write(codingMethod(u.descriminant, false));
            if (!boolDescriminant)
            {
                //
                // Produce code using an ordinary switch statement...
                //
                @out.WriteLine("        switch ( " + u.descriminant.identifier + " ) {");
                arms = u.elements.GetEnumerator();
                while (arms.MoveNext())
                {
                    org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnionArm a = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnionArm
                        )arms.Current;
                    if (a.value != null)
                    {
                        @out.WriteLine("        case " + checkForEnumValue(a.value) + ":");
                    }
                    else
                    {
                        //
                        // It's the default arm.
                        //
                        @out.WriteLine("        default:");
                    }
                    //
                    // Only emit code if arm does not map to another arm.
                    //
                    if (a.element != null)
                    {
                        if (a.element.identifier != null)
                        {
                            //
                            // Arm does not contain void, so we need to spit out
                            // encoding instructions.
                            //
                            @out.Write("    ");
                            @out.Write(codingMethod(a.element, false));
                        }
                        @out.WriteLine("            break;");
                    }
                }
                @out.WriteLine("        }");
            }
            else
            {
                //
                // boolean descriminant: here we can have at most two arms, guess
                // why.
                //
                bool firstArm = true;
                arms = u.elements.GetEnumerator();
                while (arms.MoveNext())
                {
                    org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnionArm a = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnionArm
                        )arms.Current;
                    if (a.value == null)
                    {
                        //
                        // Skip default branch this time...
                        //
                        continue;
                    }
                    if (a.element.identifier != null)
                    {
                        //
                        // Arm contains data, so we need to create encoding
                        // instructions.
                        //
                        @out.Write("        ");
                        if (!firstArm)
                        {
                            @out.Write("else ");
                        }
                        else
                        {
                            firstArm = false;
                        }
                        @out.WriteLine("if ( " + u.descriminant.identifier + " == " + checkForEnumValue(a.value
                            ) + " ) {");
                        @out.Write("    ");
                        @out.Write(codingMethod(a.element, false));
                        @out.WriteLine("        }");
                    }
                }
                arms = u.elements.GetEnumerator();
                while (arms.MoveNext())
                {
                    org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnionArm a = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnionArm
                        )arms.Current;
                    if ((a.value == null) && (a.element.identifier != null))
                    {
                        @out.Write("        ");
                        if (!firstArm)
                        {
                            @out.Write("else ");
                        }
                        @out.WriteLine("{");
                        @out.Write("    ");
                        @out.Write(codingMethod(a.element, false));
                        @out.WriteLine("        }");
                    }
                }
            }
            @out.WriteLine("    }");
            //
            // Close class...
            //
            @out.WriteLine();
            @out.WriteLine("}");
            closeJavaSourceFile();
        }

        /// <summary>
        /// Generate a source code file containing a wrapper class for a typedef
        /// defined in a x-file.
        /// </summary>
        /// <remarks>
        /// Generate a source code file containing a wrapper class for a typedef
        /// defined in a x-file.
        /// </remarks>
        /// <param name="d">
        /// 
        /// <see cref="JrpcgenDeclaration">Description</see>
        /// of XDR typedef.
        /// </param>
        public static void dumpTypedef(org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration d
            )
        {
            //
            // Create new source code file containing a Java class representing
            // the XDR struct.
            //
            StreamWriter @out = createJavaSourceFile(d.identifier);
            @out.Write("public class " + d.identifier + " : XdrAble");
            if (makeSerializable)
            {
                @out.Write(", java.io.Serializable");
            }
            @out.WriteLine(" {");
            @out.WriteLine();
            string paramType = checkForSpecials(d.type);
            if (((d.kind == org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration.FIXEDVECTOR) ||
                (d.kind == org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration.DYNAMICVECTOR)) && !
                d.type.Equals("string"))
            {
                paramType += " []";
            }
            @out.Write("    public " + paramType + " value;");
            @out.WriteLine();
            //
            // Generate serial version unique identifier
            //
            if (makeSerializable)
            {
                org.acplt.oncrpc.apps.jrpcgen.JrpcgenSHA hash = createSHA(d.identifier);
                hash.update(d.type);
                hash.update(d.kind);
                @out.WriteLine();
                @out.WriteLine("    private static readonly long serialVersionUID = " + hash.getHash()
                     + "L;");
            }
            //
            // Now generate code for encoding and decoding this class (typedef).
            //
            org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration dstar = null;
            dstar = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration)d.MemberwiseClone();
            dstar.identifier = "value";
            @out.WriteLine();
            @out.WriteLine("    public " + d.identifier + "() {");
            @out.WriteLine("    }");
            @out.WriteLine();
            @out.WriteLine("    public " + d.identifier + "(" + paramType + " value) {");
            @out.WriteLine("        this.value = value;");
            @out.WriteLine("    }");
            @out.WriteLine();
            @out.WriteLine("    public " + d.identifier + "(XdrDecodingStream xdr) {");
            @out.WriteLine("        xdrDecode(xdr);");
            @out.WriteLine("    }");
            @out.WriteLine();
            @out.WriteLine("    public void xdrEncode(XdrEncodingStream xdr) {");
            @out.Write(codingMethod(dstar, true));
            @out.WriteLine("    }");
            @out.WriteLine();
            @out.WriteLine("    public void xdrDecode(XdrDecodingStream xdr) {");
            @out.Write(codingMethod(dstar, false));
            @out.WriteLine("    }");
            //
            // Close class...
            //
            @out.WriteLine();
            @out.WriteLine("}");
            closeJavaSourceFile();
        }

        /// <summary>
        /// Generate source code files for all structures, unions and enumerations
        /// as well as constants.
        /// </summary>
        /// <remarks>
        /// Generate source code files for all structures, unions and enumerations
        /// as well as constants. All constants, which do not belong to enumerations,
        /// are emitted to a single interface.
        /// </remarks>
        public static void dumpClasses()
        {
            System.Collections.IEnumerator globals = globalIdentifiers.Values.GetEnumerator();
            while (globals.MoveNext())
            {
                object o = globals.Current;
                if (o is org.acplt.oncrpc.apps.jrpcgen.JrpcgenEnum)
                {
                    dumpEnum((org.acplt.oncrpc.apps.jrpcgen.JrpcgenEnum)o);
                }
                else
                {
                    if (o is org.acplt.oncrpc.apps.jrpcgen.JrpcgenStruct)
                    {
                        dumpStruct((org.acplt.oncrpc.apps.jrpcgen.JrpcgenStruct)o);
                    }
                    else
                    {
                        if (o is org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnion)
                        {
                            dumpUnion((org.acplt.oncrpc.apps.jrpcgen.JrpcgenUnion)o);
                        }
                        else
                        {
                            if (o is org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration)
                            {
                                dumpTypedef((org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration)o);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generate source code for client-side stub methods for a particular
        /// remote program version.
        /// </summary>
        /// <remarks>
        /// Generate source code for client-side stub methods for a particular
        /// remote program version. The client-side stub methods take the
        /// parameter(s) from the caller, encode them and throw them over to the
        /// server. After receiving a reply, they will unpack and return it as
        /// the outcome of the method call.
        /// </remarks>
        /// <param name="out">Printer writer to send source code to.</param>
        /// <param name="versionInfo">
        /// Information about the remote program version for
        /// which source code is to be generated.
        /// </param>
        internal static void dumpClientStubMethods(StreamWriter @out, org.acplt.oncrpc.apps.jrpcgen.JrpcgenVersionInfo
             versionInfo)
        {
            int size = versionInfo.procedures.Count;
            for (int idx = 0; idx < size; ++idx)
            {
                org.acplt.oncrpc.apps.jrpcgen.JrpcgenProcedureInfo proc = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenProcedureInfo
                    )versionInfo.procedures[idx];

                string paramClassName = "";
                if (proc.parameters != null && proc.parameters.Count > 1)
                {
                    paramClassName = "XdrAble_" + proc.procedureNumber;
                    @out.WriteLine("        class " + paramClassName + ": XdrAble {");
                    int psize = proc.parameters.Count;
                    for (int pidx = 0; pidx < psize; ++pidx)
                    {
                        org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo pinfo = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo
                            )proc.parameters[pidx];
                        @out.WriteLine("            public " + checkForSpecials(pinfo.parameterType) + " "
                            + pinfo.parameterName + ";");
                    }
                    @out.WriteLine("            public void xdrEncode(XdrEncodingStream xdr) {");
                    //
                    // Emit serialization code for all parameters.
                    // Note that not we do not need to deal with all kinds of
                    // parameters here, as things like "int<5>" are invalid,
                    // a typedef declaration is then necessary.
                    //
                    org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration decl = new org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration
                        (null, null);
                    for (int pidx = 0; pidx < psize; ++pidx)
                    {
                        org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo pinfo = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo
                            )proc.parameters[pidx];
                        decl.kind = org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration.SCALAR;
                        decl.identifier = pinfo.parameterName;
                        decl.type = pinfo.parameterType;
                        @out.Write("        ");
                        @out.Write(codingMethod(decl, true));
                    }
                    @out.WriteLine("            }");
                    @out.WriteLine("            public void xdrDecode(XdrDecodingStream xdr) {");
                    @out.WriteLine("            }");
                    @out.WriteLine("        };");
                }

                //
                // First spit out the stub method. While we don't need to
                // fiddle around with the data types of the method's
                // parameter(s) and result, we later have to care about
                // some primitive data types when serializing them.
                //
                string resultType = checkForSpecials(proc.resultType);
                @out.WriteLine("    /**");
                @out.WriteLine("     * Call remote procedure " + proc.procedureId + ".");
                //
                // If there are no parameters, skip the parameter documentation
                // section, otherwise dump javadoc @param entries for every
                // parameter encountered.
                //
                if (proc.parameters != null)
                {
                    System.Collections.IEnumerator @params = proc.parameters.GetEnumerator();
                    while (@params.MoveNext())
                    {
                        org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo param = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo
                            )@params.Current;
                        @out.WriteLine("     * @param " + param.parameterName + " parameter (of type " + param
                            .parameterType + ") to the remote procedure call.");
                    }
                }
                //
                // Only generate javadoc for result, when it is non-void.
                //
                if (proc.resultType.CompareTo("void") != 0)
                {
                    @out.WriteLine("     * @return Result from remote procedure call (of type " + proc.
                        resultType + ").");
                }
                @out.WriteLine("     * @throws OncRpcException if an ONC/RPC error occurs.");
                @out.WriteLine("     * @throws IOException if an I/O error occurs.");
                @out.WriteLine("     */");
                @out.Write("    public " + resultType + " " + proc.procedureId + "(");
                //
                // If the remote procedure does not have any parameters, then
                // parameters will be null. Otherwise it contains a vector with
                // information about the individual parameters, which we use
                // in order to generate the parameter list. Note that all
                // parameters are named at this point (they will either have a
                // user supplied name, or an automatically generated one).
                //
                int paramsKind;
                if (proc.parameters != null)
                {
                    int psize = proc.parameters.Count;
                    for (int pidx = 0; pidx < psize; ++pidx)
                    {
                        org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo paramInfo = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo
                            )proc.parameters[pidx];
                        if (pidx > 0)
                        {
                            @out.Write(", ");
                        }
                        @out.Write(checkForSpecials(paramInfo.parameterType));
                        @out.Write(" ");
                        @out.Write(paramInfo.parameterName);
                    }
                    //
                    // Now find out what kind of parameter(s) we have. In case
                    // the remote procedure only expects a single parameter, check
                    // whether it is a base type. In this case we later need to
                    // wrap the single parameter. If the remote procedure expects
                    // more than a single parameter, then we always need a
                    // XDR wrapper.
                    //
                    if (psize > 1)
                    {
                        paramsKind = PARAMS_MORE;
                    }
                    else
                    {
                        //
                        // psize must be equal to one, otherwise proc.parameters
                        // must have been null.
                        //
                        string firstParamType = ((org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo)proc.parameters
                            [0]).parameterType;
                        if (xdrBaseType(checkForSpecials(firstParamType)) == null)
                        {
                            //
                            // No, it is not a base type, so we don't need one
                            // of the special XDR wrapper classes.
                            //
                            paramsKind = PARAMS_SINGLE;
                        }
                        else
                        {
                            //
                            // The single parameter to the remote procedure is
                            // a base type, so we will later need a wrapper.
                            //
                            paramsKind = PARAMS_SINGLE_BASETYPE;
                        }
                    }
                }
                else
                {
                    //
                    // Remote procedure does not expect parameters at all.
                    //
                    paramsKind = PARAMS_VOID;
                }
                @out.WriteLine(") {");
                //
                // Do generate code for wrapping parameters here, if necessary.
                //
                string xdrParamsName = null;
                switch (paramsKind)
                {
                    case PARAMS_VOID:
                        {
                            // Name of variable representing XDR-able arguments
                            xdrParamsName = "args_";
                            @out.WriteLine("        XdrVoid args_ = XdrVoid.XDR_VOID;");
                            break;
                        }

                    case PARAMS_SINGLE:
                        {
                            org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo paramInfo = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo
                                )proc.parameters[0];
                            xdrParamsName = paramInfo.parameterName;
                            //
                            // We do not need to emit an args_ declaration here, as we
                            // can immediately make use of the one and only argument
                            // the remote procedure expects.
                            //
                            break;
                        }

                    case PARAMS_SINGLE_BASETYPE:
                        {
                            org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo paramInfo = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo
                                )proc.parameters[0];
                            xdrParamsName = "args_";
                            string xdrParamsType = xdrBaseType(checkForSpecials(paramInfo.parameterType));
                            @out.WriteLine("        " + xdrParamsType + " args_ = new " + xdrParamsType + "(" +
                                 paramInfo.parameterName + ");");
                            break;
                        }

                    case PARAMS_MORE:
                        {
                            xdrParamsName = "args_";
                            int psize = proc.parameters.Count;
                            @out.WriteLine("        " + paramClassName + " args_ = new " + paramClassName + "();");
                            for (int pidx = 0; pidx < psize; ++pidx)
                            {
                                org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo pinfo = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo
                                    )proc.parameters[pidx];
                                @out.WriteLine("        args_." + pinfo.parameterName + " = " + pinfo.parameterName
                                     + ";");
                            }
                            break;
                        }
                }
                //
                // Check the return data type of the result to be of one of
                // the base data types, like int, boolean, etc. In this case we
                // have to unwrap the result from one of the special XDR wrapper
                // classes and return the base data type instead.
                //
                string xdrResultType = xdrBaseType(resultType);
                //
                // Handle the result of the method: similiar to what we did
                // above. However, in all other cases we always need to
                // create a result object, regardless of whether we have to
                // deal with a basic data type (except void) or with some
                // "complex" data type.
                //
                if (resultType.Equals("void"))
                {
                    @out.WriteLine("        XdrVoid result_ = XdrVoid.XDR_VOID;");
                }
                else
                {
                    if (xdrResultType != null)
                    {
                        @out.WriteLine("        " + xdrResultType + " result_ = new " + xdrResultType + "();"
                            );
                    }
                    else
                    {
                        @out.WriteLine("        " + resultType + " result_ = new " + resultType + "();");
                    }
                }
                //
                // Now emit the real ONC/RPC call using the (optionally
                // wrapped) parameter and (optionally wrapped) result.
                //
                if (clampProgAndVers)
                {
                    @out.WriteLine("        client.call(" + baseClassname + "." + proc.procedureId + ", "
                         + baseClassname + "." + versionInfo.versionId + ", " + xdrParamsName + ", result_);"
                        );
                }
                else
                {
                    @out.WriteLine("        client.call(" + baseClassname + "." + proc.procedureId + ", client.getVersion(), "
                         + xdrParamsName + ", result_);");
                }
                //
                // In case of a wrapped result we need to return the value
                // of the wrapper, otherwise we can return the result
                // itself (which then is not a base data type). As a special
                // case, we can not return void values...anyone for a
                // language design with first class void objects?!
                //
                if (xdrResultType != null)
                {
                    //
                    // Data type of result is a Java base data type, so we need
                    // to unwrap the XDR-able result -- if it's not a void, which
                    // we do not need to return at all.
                    //
                    if (!resultType.Equals("void"))
                    {
                        @out.WriteLine("        return result_." + resultType.ToLower() + "Value();");
                    }
                }
                else
                {
                    //
                    // Data type of result is a complex type (class), so we
                    // do not unwrap it but can return it immediately.
                    //
                    @out.WriteLine("        return result_;");
                }
                //
                // Close the stub method (just as a hint, as it is
                // getting rather difficult to see what code is produced
                // at this stage...)
                //
                @out.WriteLine("    }");
                @out.WriteLine();
            }
        }

        /// <summary>Generate source code for the client stub proxy object.</summary>
        /// <remarks>
        /// Generate source code for the client stub proxy object. This client
        /// stub proxy object is then used by client applications to make remote
        /// procedure (aka method) calls to an ONC/RPC server.
        /// </remarks>
        internal static void dumpClient(org.acplt.oncrpc.apps.jrpcgen.JrpcgenProgramInfo programInfo
            )
        {
            //
            // When several versions of a program are defined, we search for the
            // latest and greatest one. This highest version number ist then
            // used to create the necessary <code>OncRpcClient</code> for
            // communication when the client proxy stub is constructed.
            //
            int version = int.Parse(((org.acplt.oncrpc.apps.jrpcgen.JrpcgenVersionInfo)programInfo
                .versions[0]).versionNumber);
            int versionSize = programInfo.versions.Count;
            for (int idx = 1; idx < versionSize; ++idx)
            {
                int anotherVersion = int.Parse(((org.acplt.oncrpc.apps.jrpcgen.JrpcgenVersionInfo
                    )programInfo.versions[idx]).versionNumber);
                if (anotherVersion > version)
                {
                    version = anotherVersion;
                }
            }
            //
            // Create new source code file containing a Java class representing
            // the XDR struct.
            // In case we have several programs defines, build the source code
            // file name from the program's name (this case is identified by a
            // null clientClass name).
            //
            string clientClass = org.acplt.oncrpc.apps.jrpcgen.jrpcgen.clientClass;
            if (clientClass == null)
            {
                clientClass = baseClassname + "_" + programInfo.programId + "_Client";
                System.Console.Out.WriteLine("CLIENT: " + clientClass);
            }
            StreamWriter @out = createJavaSourceFile(clientClass);
            @out.WriteLine("using System.Net;");
            @out.WriteLine();
            @out.WriteLine("/**");
            @out.WriteLine(" * The class <code>" + clientClass + "</code> implements the client stub proxy"
                );
            @out.WriteLine(" * for the " + programInfo.programId + " remote program. It provides method stubs"
                );
            @out.WriteLine(" * which, when called, in turn call the appropriate remote method (procedure)."
                );
            @out.WriteLine(" */");
            @out.WriteLine("public class " + clientClass + " : OncRpcClientStub {");
            @out.WriteLine();
            //
            // Generate constructors...
            //
            @out.WriteLine("    /**");
            @out.WriteLine("     * Constructs a <code>" + clientClass + "</code> client stub proxy object"
                );
            @out.WriteLine("     * from which the " + programInfo.programId + " remote program can be accessed."
                );
            @out.WriteLine("     * @param host Internet address of host where to contact the remote program."
                );
            @out.WriteLine("     * @param protocol {@link org.acplt.oncrpc.OncRpcProtocols Protocol} to be"
                );
            @out.WriteLine("     *   used for ONC/RPC calls.");
            @out.WriteLine("     * @throws OncRpcException if an ONC/RPC error occurs.");
            @out.WriteLine("     * @throws IOException if an I/O error occurs.");
            @out.WriteLine("     */");
            @out.Write("    public " + clientClass + "(IPAddress host, int protocol) : ");
            @out.WriteLine("        base(host, " + baseClassname + "." + programInfo.programId
                 + ", " + version + ", 0, protocol) {");
            @out.WriteLine("    }");
            @out.WriteLine();
            @out.WriteLine("    /**");
            @out.WriteLine("     * Constructs a <code>" + clientClass + "</code> client stub proxy object"
                );
            @out.WriteLine("     * from which the " + programInfo.programId + " remote program can be accessed."
                );
            @out.WriteLine("     * @param host Internet address of host where to contact the remote program."
                );
            @out.WriteLine("     * @param port Port number at host where the remote program can be reached."
                );
            @out.WriteLine("     * @param protocol {@link org.acplt.oncrpc.OncRpcProtocols Protocol} to be"
                );
            @out.WriteLine("     *   used for ONC/RPC calls.");
            @out.WriteLine("     * @throws OncRpcException if an ONC/RPC error occurs.");
            @out.WriteLine("     * @throws IOException if an I/O error occurs.");
            @out.WriteLine("     */");
            @out.Write("    public " + clientClass + "(IPAddress host, int port, int protocol) :"
                );
            @out.WriteLine("        base(host, " + baseClassname + "." + programInfo.programId
                 + ", " + version + ", port, protocol) {");
            @out.WriteLine("    }");
            @out.WriteLine();
            @out.WriteLine("    /**");
            @out.WriteLine("     * Constructs a <code>" + clientClass + "</code> client stub proxy object"
                );
            @out.WriteLine("     * from which the " + programInfo.programId + " remote program can be accessed."
                );
            @out.WriteLine("     * @param client ONC/RPC client connection object implementing a particular"
                );
            @out.WriteLine("     *   protocol.");
            @out.WriteLine("     * @throws OncRpcException if an ONC/RPC error occurs.");
            @out.WriteLine("     * @throws IOException if an I/O error occurs.");
            @out.WriteLine("     */");
            @out.WriteLine("    public " + clientClass + "(OncRpcClient client) : base(client) {");
            @out.WriteLine("    }");
            @out.WriteLine();
            @out.WriteLine("    /**");
            @out.WriteLine("     * Constructs a <code>" + clientClass + "</code> client stub proxy object"
                );
            @out.WriteLine("     * from which the " + programInfo.programId + " remote program can be accessed."
                );
            @out.WriteLine("     * @param host Internet address of host where to contact the remote program."
                );
            @out.WriteLine("     * @param program Remote program number.");
            @out.WriteLine("     * @param version Remote program version number.");
            @out.WriteLine("     * @param protocol {@link org.acplt.oncrpc.OncRpcProtocols Protocol} to be"
                );
            @out.WriteLine("     *   used for ONC/RPC calls.");
            @out.WriteLine("     * @throws OncRpcException if an ONC/RPC error occurs.");
            @out.WriteLine("     * @throws IOException if an I/O error occurs.");
            @out.WriteLine("     */");
            @out.Write("    public " + clientClass + "(IPAddress host, int program, int version, int protocol) :"
                );
            @out.WriteLine("        base(host, program, version, 0, protocol) {");
            @out.WriteLine("    }");
            @out.WriteLine();
            @out.WriteLine("    /**");
            @out.WriteLine("     * Constructs a <code>" + clientClass + "</code> client stub proxy object"
                );
            @out.WriteLine("     * from which the " + programInfo.programId + " remote program can be accessed."
                );
            @out.WriteLine("     * @param host Internet address of host where to contact the remote program."
                );
            @out.WriteLine("     * @param program Remote program number.");
            @out.WriteLine("     * @param version Remote program version number.");
            @out.WriteLine("     * @param port Port number at host where the remote program can be reached."
                );
            @out.WriteLine("     * @param protocol {@link org.acplt.oncrpc.OncRpcProtocols Protocol} to be"
                );
            @out.WriteLine("     *   used for ONC/RPC calls.");
            @out.WriteLine("     * @throws OncRpcException if an ONC/RPC error occurs.");
            @out.WriteLine("     * @throws IOException if an I/O error occurs.");
            @out.WriteLine("     */");
            @out.Write("    public " + clientClass + "(IPAddress host, int program, int version, int port, int protocol) :"
                );
            @out.WriteLine("        base(host, program, version, port, protocol) {");
            @out.WriteLine("    }");
            @out.WriteLine();
            //
            // Generate method stubs... This is getting hairy in case someone
            // uses basic data types as parameters or the procedure's result.
            // In these cases we need to encapsulate these basic data types in
            // XDR-able data types.
            //
            for (int versionIdx = 0; versionIdx < versionSize; ++versionIdx)
            {
                org.acplt.oncrpc.apps.jrpcgen.JrpcgenVersionInfo versionInfo = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenVersionInfo
                    )programInfo.versions[versionIdx];
                dumpClientStubMethods(@out, versionInfo);
            }
            //
            // Close class...done!
            //
            @out.WriteLine("}");
            closeJavaSourceFile();
        }

        internal static void dumpServerStubStructs(StreamWriter @out, org.acplt.oncrpc.apps.jrpcgen.JrpcgenProcedureInfo
             proc)
        {
            //
            // Check for special return types, like enumerations, which we
            // map to their corresponding Java base data type.
            //
            string resultType = checkForSpecials(proc.resultType);
            //
            // If the remote procedure does not have any parameters, then
            // parameters will be null. Otherwise it contains a vector with
            // information about the individual parameters, which we use
            // in order to generate the parameter list. Note that all
            // parameters are named at this point (they will either have a
            // user supplied name, or an automatically generated one).
            //
            if (proc.parameters != null)
            {
                int psize = proc.parameters.Count;
                //
                // Now find out what kind of parameter(s) we have. In case
                // the remote procedure only expects a single parameter, check
                // whether it is a base type. In this case we later need to
                // wrap the single parameter. If the remote procedure expects
                // more than a single parameter, then we always need a
                // XDR wrapper.
                //
                if (psize > 1)
                {
                    //
                    //
                    //
                    System.Text.StringBuilder paramsBuff = new System.Text.StringBuilder();
                    @out.WriteLine("                class XdrAble_" + proc.procedureNumber + " : XdrAble {");
                    for (int pidx = 0; pidx < psize; ++pidx)
                    {
                        org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo pinfo = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo
                            )proc.parameters[pidx];
                        @out.WriteLine("                    public " + checkForSpecials(pinfo.parameterType
                            ) + " " + pinfo.parameterName + ";");
                    }
                    @out.WriteLine("                    public void xdrEncode(XdrEncodingStream xdr) {");
                    @out.WriteLine("                    }");
                    @out.WriteLine("                    public void xdrDecode(XdrDecodingStream xdr) {");
                    //
                    // Emit serialization code for all parameters.
                    // Note that not we do not need to deal with all kinds of
                    // parameters here, as things like "int<5>" are invalid,
                    // a typedef declaration is then necessary.
                    //
                    org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration decl = new org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration
                        (null, null);
                    for (int pidx = 0; pidx < psize; ++pidx)
                    {
                        org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo pinfo = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo
                            )proc.parameters[pidx];
                        decl.kind = org.acplt.oncrpc.apps.jrpcgen.JrpcgenDeclaration.SCALAR;
                        decl.identifier = pinfo.parameterName;
                        decl.type = pinfo.parameterType;
                        @out.Write("                ");
                        @out.Write(codingMethod(decl, false));
                    }
                    @out.WriteLine("                    }");
                    @out.WriteLine("                };");
                }
            }
        }

        internal static void dumpServerStubMethodCall(StreamWriter @out, org.acplt.oncrpc.apps.jrpcgen.JrpcgenProcedureInfo
             proc)
        {
            //
            // Check for special return types, like enumerations, which we
            // map to their corresponding Java base data type.
            //
            string resultType = checkForSpecials(proc.resultType);
            //
            // If the remote procedure does not have any parameters, then
            // parameters will be null. Otherwise it contains a vector with
            // information about the individual parameters, which we use
            // in order to generate the parameter list. Note that all
            // parameters are named at this point (they will either have a
            // user supplied name, or an automatically generated one).
            //
            int paramsKind;
            if (proc.parameters != null)
            {
                int psize = proc.parameters.Count;
                //
                // Now find out what kind of parameter(s) we have. In case
                // the remote procedure only expects a single parameter, check
                // whether it is a base type. In this case we later need to
                // wrap the single parameter. If the remote procedure expects
                // more than a single parameter, then we always need a
                // XDR wrapper.
                //
                if (psize > 1)
                {
                    paramsKind = PARAMS_MORE;
                }
                else
                {
                    //
                    // psize must be equal to one, otherwise proc.parameters
                    // must have been null.
                    //
                    string firstParamType = ((org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo)proc.parameters
                        [0]).parameterType;
                    if (xdrBaseType(checkForSpecials(firstParamType)) == null)
                    {
                        //
                        // No, it is not a base type, so we don't need one
                        // of the special XDR wrapper classes.
                        //
                        paramsKind = PARAMS_SINGLE;
                    }
                    else
                    {
                        //
                        // The single parameter to the remote procedure is
                        // a base type, so we will later need a wrapper.
                        //
                        paramsKind = PARAMS_SINGLE_BASETYPE;
                    }
                }
            }
            else
            {
                //
                // Remote procedure does not expect parameters at all.
                //
                paramsKind = PARAMS_VOID;
            }
            //
            // Do generate code for unwrapping here, if necessary.
            //
            string @params = string.Empty;
            switch (paramsKind)
            {
                case PARAMS_VOID:
                    {
                        //
                        // Almost nothing to do here -- well, we need to retrieve nothing
                        // so the RPC layer can do its book keeping.
                        //
                        @out.WriteLine("                call.retrieveCall(XdrVoid.XDR_VOID);");
                        @params = withCallInfo ? "call" : string.Empty;
                        break;
                    }

                case PARAMS_SINGLE:
                    {
                        //
                        // Only a single parameter, which is in addition immediately
                        // ready for serialization.
                        //
                        org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo paramInfo = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo
                            )proc.parameters[0];
                        @out.WriteLine("                " + paramInfo.parameterType + " args_ = new " + paramInfo
                            .parameterType + "();");
                        @out.WriteLine("                call.retrieveCall(args_);");
                        @params = (withCallInfo ? "call, " : string.Empty) + "args_";
                        break;
                    }

                case PARAMS_SINGLE_BASETYPE:
                    {
                        //
                        // Only a single parameter, but we have to unwrap it first.
                        //
                        org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo paramInfo = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo
                            )proc.parameters[0];
                        string paramsType = checkForSpecials(paramInfo.parameterType);
                        string xdrParamsType = xdrBaseType(paramsType);
                        @out.WriteLine("                " + xdrParamsType + " args_ = new " + xdrParamsType
                             + "();");
                        @out.WriteLine("                call.retrieveCall(args_);");
                        @params = (withCallInfo ? "call, " : string.Empty) + "args_." + paramsType.ToLower
                            () + "Value()";
                        break;
                    }

                case PARAMS_MORE:
                    {
                        //
                        // We only need to  refer to the struct here, we don't declare it as
                        // that isn't valid C# syntac
                        //
                        int psize = proc.parameters.Count;
                        @out.WriteLine("                XdrAble_" + proc.procedureNumber + " args_ = new XdrAble_"
                            + proc.procedureNumber + "();");
                        @out.WriteLine("                call.retrieveCall(args_);");
                        System.Text.StringBuilder paramsBuff = new System.Text.StringBuilder();
                        if (withCallInfo)
                        {
                            if (psize > 0)
                            {
                                paramsBuff.Append("call, ");
                            }
                            else
                            {
                                paramsBuff.Append("call");
                            }
                        }
                        for (int pidx = 0; pidx < psize; ++pidx)
                        {
                            org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo pinfo = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo
                                )proc.parameters[pidx];
                            if (pidx > 0)
                            {
                                paramsBuff.Append(", ");
                            }
                            paramsBuff.Append("args_.");
                            paramsBuff.Append(pinfo.parameterName);
                        }
                        @params = paramsBuff.ToString();
                        break;
                    }
            }
            //
            // Check the return data type of the result to be of one of
            // the base data types, like int, boolean, etc. In this case we
            // have to unwrap the result from one of the special XDR wrapper
            // classes and return the base data type instead.
            //
            string procName = proc.procedureId.Substring(0, 1).ToUpper() + proc.procedureId.Substring(1);
            string xdrResultType = xdrBaseType(resultType);
            if (resultType.Equals("void"))
            {
                //
                // It's a remote procedure, so it does return simply nothing.
                // We use the singleton XDR_VOID to return a "nothing".
                //
                @out.WriteLine("                " + procName + "(" + @params + ");");
                @out.WriteLine("                call.reply(XdrVoid.XDR_VOID);");
            }
            else
            {
                if (xdrResultType != null)
                {
                    //
                    // The return type is some Java base data type, so we need to
                    // wrap the return value before we can serialize it.
                    //
                    @out.WriteLine("                " + xdrResultType + " result_ = new " + xdrResultType
                         + "(" + procName + "(" + @params + "));");
                    @out.WriteLine("                call.reply(result_);");
                }
                else
                {
                    //
                    // The return type is a complex type which supports XdrAble.
                    //
                    @out.WriteLine("                " + resultType + " result_ = " + procName +
                         "(" + @params + ");");
                    @out.WriteLine("                call.reply(result_);");
                }
            }
        }

        /// <summary>
        /// Generate public abstract method signatures for all remote procedure
        /// calls.
        /// </summary>
        /// <remarks>
        /// Generate public abstract method signatures for all remote procedure
        /// calls. This ensures that they have to be implemented before any
        /// derived server class gets usefull.
        /// </remarks>
        internal static void dumpServerStubMethods(StreamWriter @out, org.acplt.oncrpc.apps.jrpcgen.JrpcgenVersionInfo
             versionInfo)
        {
            int procSize = versionInfo.procedures.Count;
            for (int idx = 0; idx < procSize; ++idx)
            {
                org.acplt.oncrpc.apps.jrpcgen.JrpcgenProcedureInfo proc = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenProcedureInfo
                    )versionInfo.procedures[idx];
                //
                // Fold enumerations et cetera back to their Java base data types.
                //
                string resultType = checkForSpecials(proc.resultType);
                //
                // Now emit the method signature, checking each argument for
                // specials, like enumerations. Also take care of no parameters
                // at all... Fortunately, this is relatively easy as we do not
                // need to care about parameter wrapping/unwrapping here.
                //
                string procName = proc.procedureId.Substring(0, 1).ToUpper() + proc.procedureId.Substring(1);
                @out.Write("    public abstract " + resultType + " " + procName + "(");
                if (proc.parameters != null)
                {
                    if (withCallInfo)
                    {
                        @out.Write("OncRpcCallInformation call_, ");
                    }
                    int psize = proc.parameters.Count;
                    for (int pidx = 0; pidx < psize; ++pidx)
                    {
                        org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo paramInfo = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenParamInfo
                            )proc.parameters[pidx];
                        if (pidx > 0)
                        {
                            @out.Write(", ");
                        }
                        @out.Write(checkForSpecials(paramInfo.parameterType));
                        @out.Write(" ");
                        @out.Write(paramInfo.parameterName);
                    }
                }
                else
                {
                    if (withCallInfo)
                    {
                        @out.Write("OncRpcCallInformation call_");
                    }
                }
                @out.WriteLine(");");
                @out.WriteLine();
            }
        }

        internal static void dumpServer(org.acplt.oncrpc.apps.jrpcgen.JrpcgenProgramInfo programInfo
            )
        {
            //
            // Create new source code file containing a Java class representing
            // the XDR struct.
            // In case we have several programs defines, build the source code
            // file name from the program's name (this case is identified by a
            // null clientClass name).
            //
            string serverClass = jrpcgen.serverClass;
            if (serverClass == null)
            {
                serverClass = baseClassname + "_" + programInfo.programId + "_ServerStub";
            }
            StreamWriter @out = createJavaSourceFile(serverClass);
            @out.WriteLine("using System.Net;");
            @out.WriteLine();
            @out.WriteLine("using org.acplt.oncrpc.server;");
            @out.WriteLine();
            @out.WriteLine("/**");
            @out.WriteLine(" */");
            @out.WriteLine("public abstract class " + serverClass + " : OncRpcServerStub, OncRpcDispatchable {"
                );
            @out.WriteLine();
            //
            // Generate constructor(s)...
            //
            @out.WriteLine("    public " + serverClass + "() : this(0) {");
            @out.WriteLine("    }");
            @out.WriteLine();
            @out.WriteLine("    public " + serverClass + "(int port) : this(null, port) {");
            @out.WriteLine("    }");
            @out.WriteLine();
            @out.WriteLine("    public " + serverClass + "(IPAddress bindAddr, int port)");
            @out.WriteLine("           {");
            //
            // For every version specified, create both UDP and TCP-based
            // transports.
            //
            @out.WriteLine("        info = new OncRpcServerTransportRegistrationInfo [] {");
            int versionSize = programInfo.versions.Count;
            for (int versionIdx = 0; versionIdx < versionSize; ++versionIdx)
            {
                JrpcgenVersionInfo versionInfo = (JrpcgenVersionInfo
                    )programInfo.versions[versionIdx];
                @out.WriteLine("            new OncRpcServerTransportRegistrationInfo(" + baseClassname
                     + "." + programInfo.programId + ", " + versionInfo.versionNumber + "),");
            }
            @out.WriteLine("        };");
            @out.WriteLine("        transports = new OncRpcServerTransport [] {");
            @out.WriteLine("            new OncRpcUdpServerTransport(this, bindAddr, port, info, 32768),"
                );
            @out.WriteLine("            new OncRpcTcpServerTransport(this, bindAddr, port, info, 32768)"
                );
            @out.WriteLine("        };");
            //
            // Finish constructor method...
            //
            @out.WriteLine("    }");
            @out.WriteLine();
            //
            // First we need to create all the STRUCTs so that they are outside member
            // function scope for C#.
            //
            for (int versionIdx = 0; versionIdx < versionSize; ++versionIdx)
            {
                JrpcgenVersionInfo versionInfo = (JrpcgenVersionInfo
                    )programInfo.versions[versionIdx];
                int procSize = versionInfo.procedures.Count;
                for (int procIdx = 0; procIdx < procSize; ++procIdx)
                {
                    //
                    // We only need to emit STRUCTs required here
                    //
                    JrpcgenProcedureInfo procInfo = (JrpcgenProcedureInfo
                        )versionInfo.procedures[procIdx];
                    dumpServerStubStructs(@out, procInfo);
                }
            }
            //
            // Now generate dispatcher code using the previously generated STRUCTs
            // where applicable
            //
            @out.WriteLine("    public void dispatchOncRpcCall(OncRpcCallInformation call, int program, int version, int procedure)"
                );
            @out.WriteLine("            {");
            for (int versionIdx = 0; versionIdx < versionSize; ++versionIdx)
            {
                JrpcgenVersionInfo versionInfo = (JrpcgenVersionInfo
                    )programInfo.versions[versionIdx];
                @out.Write(versionIdx == 0 ? "        " : "        } else ");
                @out.WriteLine("if ( version == " + versionInfo.versionNumber + " ) {");
                int procSize = versionInfo.procedures.Count;
                @out.WriteLine("            switch ( procedure ) {");
                for (int procIdx = 0; procIdx < procSize; ++procIdx)
                {
                    //
                    // Emit case arms for every procedure defined. We have to
                    // take care that the procedure number might be a constant
                    // comming from an enumeration: in this case we need also to
                    // dump the enclosure.
                    //
                    JrpcgenProcedureInfo procInfo = (JrpcgenProcedureInfo
                        )versionInfo.procedures[procIdx];
                    @out.WriteLine("            case " + checkForEnumValue(procInfo.procedureNumber) +
                        ": {");
                    dumpServerStubMethodCall(@out, procInfo);
                    @out.WriteLine("                break;");
                    @out.WriteLine("            }");
                }
                @out.WriteLine("            default:");
                @out.WriteLine("                call.failProcedureUnavailable();");
                @out.WriteLine("                break;");
                @out.WriteLine("            }");
            }
            @out.WriteLine("        } else {");
            @out.WriteLine("            call.failProgramUnavailable();");
            @out.WriteLine("        }");
            @out.WriteLine("    }");
            @out.WriteLine();
            //
            // Generate the abstract stub methods for all specified remote procedures.
            //
            for (int versionIdx = 0; versionIdx < versionSize; ++versionIdx)
            {
                org.acplt.oncrpc.apps.jrpcgen.JrpcgenVersionInfo versionInfo = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenVersionInfo
                    )programInfo.versions[versionIdx];
                dumpServerStubMethods(@out, versionInfo);
            }
            //
            // Close class...done!
            //
            @out.WriteLine("}");
            closeJavaSourceFile();
        }

        /// <summary>
        /// Create the source code files based on the parsed information from the
        /// x-file.
        /// </summary>
        /// <remarks>
        /// Create the source code files based on the parsed information from the
        /// x-file.
        /// </remarks>
        public static void dumpFiles()
        {
            dumpConstants();
            dumpClasses();
            for (int i = 0; i < programInfos.Count; ++i)
            {
                org.acplt.oncrpc.apps.jrpcgen.JrpcgenProgramInfo progInfo = (org.acplt.oncrpc.apps.jrpcgen.JrpcgenProgramInfo
                    )programInfos[i];
                if (!noClient)
                {
                    dumpClient(progInfo);
                }
                if (!noServer)
                {
                    dumpServer(progInfo);
                }
            }
        }

        /// <summary>The main part of jrpcgen where all things start.</summary>
        /// <remarks>The main part of jrpcgen where all things start.</remarks>
        public static void Main(string[] args)
        {
            //
            // First parse the command line (options)...
            //
            int argc = args.Length;
            int argIdx = 0;
            for (; argIdx < argc; ++argIdx)
            {
                //
                // Check to see whether this is an option...
                //
                string arg = args[argIdx];
                if ((arg.Length > 0) && (arg[0] != '-'))
                {
                    break;
                }
                //
                // ...and which option is it?
                //
                if (arg.Equals("-d"))
                {
                    // -d <dir>
                    if (++argIdx >= argc)
                    {
                        System.Console.Out.WriteLine("jrpcgen: missing directory");
                        System.Environment.Exit(1);
                    }
                    destinationDir = args[argIdx];
                }
                else
                {
                    if (arg.Equals("-package") || arg.Equals("-p"))
                    {
                        // -p <package name>
                        if (++argIdx >= argc)
                        {
                            System.Console.Out.WriteLine("jrpcgen: missing package name");
                            System.Environment.Exit(1);
                        }
                        packageName = args[argIdx];
                    }
                    else
                    {
                        if (arg.Equals("-c"))
                        {
                            // -c <class name>
                            if (++argIdx >= argc)
                            {
                                System.Console.Out.WriteLine("jrpcgen: missing client class name");
                                System.Environment.Exit(1);
                            }
                            clientClass = args[argIdx];
                        }
                        else
                        {
                            if (arg.Equals("-s"))
                            {
                                // -s <class name>
                                if (++argIdx >= argc)
                                {
                                    System.Console.Out.WriteLine("jrpcgen: missing server class name");
                                    System.Environment.Exit(1);
                                }
                                serverClass = args[argIdx];
                            }
                            else
                            {
                                if (arg.Equals("-ser"))
                                {
                                    makeSerializable = true;
                                }
                                else
                                {
                                    if (arg.Equals("-bean"))
                                    {
                                        makeSerializable = true;
                                        makeBean = true;
                                    }
                                    else
                                    {
                                        if (arg.Equals("-initstrings"))
                                        {
                                            initStrings = true;
                                        }
                                        else
                                        {
                                            if (arg.Equals("-noclamp"))
                                            {
                                                clampProgAndVers = false;
                                            }
                                            else
                                            {
                                                if (arg.Equals("-withcallinfo"))
                                                {
                                                    withCallInfo = true;
                                                }
                                                else
                                                {
                                                    if (arg.Equals("-debug"))
                                                    {
                                                        debug = true;
                                                    }
                                                    else
                                                    {
                                                        if (arg.Equals("-nobackup"))
                                                        {
                                                            noBackups = true;
                                                        }
                                                        else
                                                        {
                                                            if (arg.Equals("-noclient"))
                                                            {
                                                                noClient = true;
                                                            }
                                                            else
                                                            {
                                                                if (arg.Equals("-noserver"))
                                                                {
                                                                    noServer = true;
                                                                }
                                                                else
                                                                {
                                                                    if (arg.Equals("-parseonly"))
                                                                    {
                                                                        parseOnly = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (arg.Equals("-verbose"))
                                                                        {
                                                                            verbose = true;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (arg.Equals("-version"))
                                                                            {
                                                                                System.Console.Out.WriteLine("jrpcgen version \"" + VERSION + "\"");
                                                                                System.Environment.Exit(1);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (arg.Equals("-help") || arg.Equals("-?"))
                                                                                {
                                                                                    printHelp();
                                                                                    System.Environment.Exit(1);
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (arg.Equals("--"))
                                                                                    {
                                                                                        //
                                                                                        // End of options...
                                                                                        //
                                                                                        ++argIdx;
                                                                                        break;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //
                                                                                        // It's an unknown option!
                                                                                        //
                                                                                        System.Console.Out.WriteLine("Unrecognized option: " + arg);
                                                                                        System.Environment.Exit(1);
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //
            // Otherwise we regard the current command line argument to be the
            // name of the x-file to compile. Check, that there is exactly one
            // x-file specified.
            //
            if ((argIdx >= argc) || (argIdx < argc - 1))
            {
                printHelp();
                System.Environment.Exit(1);
            }
            string xfilename = args[argIdx];
            xFile = "./" + xfilename;
            //
            // Try to parse the file and generate the different class source
            // code files...
            //
            try
            {
                doParse();
            }
            catch (System.Exception t)
            {
                System.Console.Out.WriteLine(t.Message);
                System.Console.Out.WriteLine(t.StackTrace);
                //
                // Exit application with non-zero outcome, so in case jrpcgen is
                // used as part of, for instance, a make process, such tools can
                // detect that there was a problem.
                //
                System.Environment.Exit(1);
            }
        }

        /// <summary>The real parsing and code generation part.</summary>
        /// <remarks>
        /// The real parsing and code generation part. This has been factored out
        /// of main() in order to make it available as an Ant task.
        /// </remarks>
        /// <exception cref="java.io.FileNotFoundException"></exception>
        /// <exception cref="System.Exception"></exception>
        public static void doParse()
        {
            //
            // Get the base name for the client and server classes, it is derived
            // from the filename.
            //
            if (baseClassname == null)
            {
                string name = Path.GetFileName(xFile);
                int dotIdx = name.LastIndexOf('.');
                if (dotIdx < 0)
                {
                    baseClassname = name;
                }
                else
                {
                    baseClassname = name.Substring(0, dotIdx);
                }
            }
            //
            //
            //
            StreamReader @in = null;
            try
            {
                @in = new StreamReader(xFile);
            }
            catch (FileNotFoundException)
            {
                throw (new FileNotFoundException("jrpcgen: can not open source x-file \""
                     + Path.GetFullPath(xFile) + "\""));
            }
            org.acplt.oncrpc.apps.jrpcgen.JrpcgenScanner scanner = new org.acplt.oncrpc.apps.jrpcgen.JrpcgenScanner
                (@in);
            org.acplt.oncrpc.apps.jrpcgen.JrpcgenParser parser = new org.acplt.oncrpc.apps.jrpcgen.JrpcgenParser
                (scanner);
            org.acplt.oncrpc.apps.jrpcgen.jrpcgen.globalIdentifiers["TRUE"] = new org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst
                ("TRUE", "true");
            org.acplt.oncrpc.apps.jrpcgen.jrpcgen.globalIdentifiers["FALSE"] = new org.acplt.oncrpc.apps.jrpcgen.JrpcgenConst
                ("FALSE", "false");
            try
            {
                Symbol sym = parser.parse();
                if (!parseOnly)
                {
                    if (programInfos.Count <= 1)
                    {
                        if (clientClass == null)
                        {
                            clientClass = baseClassname + "Client";
                        }
                        if (serverClass == null)
                        {
                            serverClass = baseClassname + "ServerStub";
                        }
                    }
                    dumpFiles();
                }
            }
            catch (org.acplt.oncrpc.apps.jrpcgen.JrpcgenParserException pe)
            {
                throw (new System.Exception("jrpcgen: compilation aborted (" + pe.Message + ")"));
            }
        }
    }

    /// <summary>
    /// The class <code>JrpcgenEnDecodingInfo</code> contains information which
    /// is necessary to generate source code calling appropriate XDR encoding
    /// and decoding methods.
    /// </summary>
    /// <remarks>
    /// The class <code>JrpcgenEnDecodingInfo</code> contains information which
    /// is necessary to generate source code calling appropriate XDR encoding
    /// and decoding methods.
    /// </remarks>
    /// <version>$Revision: 1.6 $ $Date: 2007/05/29 19:38:30 $ $State: Exp $ $Locker:  $</version>
    /// <author>Harald Albrecht</author>
    internal class JrpcgenEnDecodingInfo
    {
        /// <summary>
        /// Construct a <code>JrpcgenEnDecodingInfo</code> object containing
        /// information for generating source code for encoding and decoding
        /// of XDR/Java base data types.
        /// </summary>
        /// <remarks>
        /// Construct a <code>JrpcgenEnDecodingInfo</code> object containing
        /// information for generating source code for encoding and decoding
        /// of XDR/Java base data types.
        /// </remarks>
        /// <param name="syllable">Syllable of encoding/decoding method.</param>
        /// <param name="encodingOptions">
        /// Optional parameters necessary to encode
        /// base data type.
        /// </param>
        /// <param name="decodingOptions">
        /// Optional parameters necessary to decode
        /// base data type.
        /// </param>
        public JrpcgenEnDecodingInfo(string syllable, string encodingOptions, string decodingOptions
            )
        {
            this.syllable = syllable;
            this.encodingOptions = encodingOptions;
            this.decodingOptions = decodingOptions;
        }

        /// <summary>(Type) syllable of the encoding or decoding method.</summary>
        /// <remarks>
        /// (Type) syllable of the encoding or decoding method. The full name
        /// of the encoding or decoding method is always in the form of
        /// "xdrEncodeXXX(...)" or "xdrDecodeXXX(...)", where "XXX" is the
        /// syllable contained in this attribute.
        /// </remarks>
        public string syllable;

        /// <summary>Optional parameters to use when encoding a base data type.</summary>
        /// <remarks>
        /// Optional parameters to use when encoding a base data type. This
        /// typically includes the size parameter for encoding fixed-size
        /// vectors/arrays. When this attribute is not <code>null</code>, then
        /// these parameters need to be appended. The attribute never contains
        /// a leading parameter separator (aka "comma").
        /// </remarks>
        public string encodingOptions;

        /// <summary>Optional parameters to use when decoding a base data type.</summary>
        /// <remarks>
        /// Optional parameters to use when decoding a base data type. This
        /// typically includes the size parameter for decoding fixed-size
        /// vectors/arrays. When this attribute is not <code>null</code>, then
        /// these parameters need to be appended. The attribute never contains
        /// a leading parameter separator (aka "comma").
        /// </remarks>
        public string decodingOptions;
    }
}
