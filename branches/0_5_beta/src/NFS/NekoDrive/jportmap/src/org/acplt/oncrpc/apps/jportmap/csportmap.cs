using System.Net;
using System.Net.Sockets;
using System;
using org.acplt.oncrpc.server;

namespace org.acplt.oncrpc.apps.jportmap
{
	/// <summary>
	/// The class <code>csportmap</code> is the command-line main
	/// for an ONC/RPC port mapper, speaking the widely deployed
	/// protocol version 2.
	/// </summary>
	/// <remarks>
	/// The class <code>csportmap</code> is the command-line main
	/// for an ONC/RPC port mapper, speaking the widely deployed
	/// protocol version 2.
	/// </remarks>
	/// <version>$Revision: 1.2 $ $Date: 2003/08/14 11:26:50 $ $State: Exp $ $Locker:  $</version>
	/// <author>Jay Walters</author>
	public class csportmap : jportmap
	{
		/// <summary>
		/// Create a new portmap instance, create the transport registration
		/// information and UDP and TCP-based transports, which will be bound
		/// later to port 111.
		/// </summary>
		/// <remarks>
		/// Create a new portmap instance, create the transport registration
		/// information and UDP and TCP-based transports, which will be bound
		/// later to port 111. The constructor does not start the dispatcher loop.
		/// </remarks>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		public csportmap() : base()
		{
        }

		/// <summary>Create an instance of an ONC/RPC portmapper and run it.</summary>
		/// <remarks>
		/// Create an instance of an ONC/RPC portmapper and run it. As we have
		/// to bootstrap the ONC/RPC port information chain, we do not use the
		/// usual overloaded <code>run()</code> method without any parameters,
		/// but instead supply it the transports to handle. Registration and
		/// deregistration is not necessary and not possible.
		/// </remarks>
		public static void Main(string[] args)
		{
			try
			{
				csportmap pmap = new csportmap
					();
				pmap.run(pmap.transports);
				pmap.Close(pmap.transports);
			}
			catch (org.acplt.oncrpc.OncRpcException e)
			{
                Console.Out.WriteLine(e.Message);
                Console.Out.WriteLine(e.StackTrace);
			}
			catch (System.IO.IOException e)
			{
                Console.Out.WriteLine(e.Message);
                Console.Out.WriteLine(e.StackTrace);
			}
		}
	}
}
