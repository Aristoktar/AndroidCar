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
	    ConcurrentBag<Session> _clientsSessions;
	    ConcurrentBag<TcpClient> _clients; 
		
		private int _maxClients = 2;
	    public int MaxClients {
		    get {return _maxClients; }
		    set { _maxClients = value; }
	    }

	    public Server(string endpoint = "localhost",int port=123)
	    {
			_listener = new TcpListener ( IPAddress.Parse ( endpoint ) , port );
			
			//_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,  ProtocolType.Tcp);
			//_socket.Bind(new IPEndPoint(IPAddress.Parse(endpoint), 554));
			//_clientThread = new Thread(AcceptLoop);
	    }
		
	    public void Start()
	    {
			_listener.Start ();
			AcceptCircle ( _listener );
			//return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	    }
		void AcceptCircle ( TcpListener listener ) {
			while (_clients.Count < MaxClients)
			{

				listener.BeginAcceptTcpClient(ar =>
				{
					Thread thread = new Thread (AcceptClient);
					var listenerTcp = (TcpListener) ar.AsyncState;
					var client = listenerTcp.EndAcceptTcpClient(ar);
					thread.Start ( client );
				}, listener);
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
				    session.AddNewFrame(frame);
			    }
		    }
		    catch (TimeoutException)
		    {
		    }
	    }
    }
}
