using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aristov.Communication.RT
{
    public class Server
    {
	    Socket _socket;
	    TcpListener _listener;// = new TcpListener ( IPAddress.Parse ( this.textBox1.Text ) , 123 );
	    Thread _clientThread;
	    ConcurrentBag<Session> _clientsSessions = new ConcurrentBag<Session>();
	    ConcurrentBag<TcpClient> _clients; 
		
		private int _maxClients = 2;
	    public int MaxClients {
		    get {return _maxClients; }
		    set { _maxClients = value; }
	    }

	    public Server(string endpoint = "localhost", int port=8012)
	    {
		    var end = IPAddress.Parse(endpoint);
			_listener = new TcpListener ( end , port );
			
			//_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,  ProtocolType.Tcp);
			//_socket.Bind(new IPEndPoint(IPAddress.Parse(endpoint), 554));
			//_clientThread = new Thread(AcceptLoop);
	    }

	    Thread _acceptThread;
	    public void Start()
	    {
			_listener.Start ();
			_acceptThread = new Thread(AcceptCircle);
			_acceptThread.Start(_listener);
			
			//return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	    }
		void AcceptCircle ( object listenerObj )
		{
			var listener = (TcpListener) listenerObj;
			while ( _clientsSessions.Count < MaxClients )
			{

				var iar=listener.BeginAcceptTcpClient(ar =>
				{
					Thread thread = new Thread (AcceptClient);
					var listenerTcp = (TcpListener) ar.AsyncState;
					var client = listenerTcp.EndAcceptTcpClient(ar);
					thread.Start ( client );
					
				}, listener);
				iar.AsyncWaitHandle.WaitOne();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="clientObj">must be TcpClient</param>
	    void AcceptClient(object clientObj)
		{
			var client = (TcpClient) clientObj;
			_clientsSessions.Add(new Session(client));

		}

	    public void NewFrame(byte[] frame)
	    {
		    try
		    {
				
			    foreach (var session in _clientsSessions)
			    {
				    ThreadPool.QueueUserWorkItem((state) =>
				    {
					    var parameters = state as object[];
					    (parameters[0] as Session).AddNewFrame(parameters[1] as byte[]);
				    },new object[]{session,frame});
				  //  session.AddNewFrame(frame);
			    }
		    }
		    catch (TimeoutException)
		    {

		    }
	    }
    }
}
