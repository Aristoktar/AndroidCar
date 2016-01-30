using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Aristov.Common;

namespace Aristov.Communication.RT.Queued
{
    public class VideoServer
    {
	    ILogger Log = LoggerFactory.Create();
	    private ConcurrentQueue< IPacket> _dataQueue;
	    private Socket _serverSocket;

	    private List<Socket> _clientsSockets; 
	    public bool Connected { get; private set; }
	    int frameCounter = 0;
	    Thread _acceptThread;
	    Thread _sendThread;
	    public bool Running { get; set; }
	    private int _maxConnections = 10;

	    public VideoServer ( string endpoint = "localhost" , int port = 8012 )
	    {


			_dataQueue = new ConcurrentQueue<IPacket>();
			_clientsSockets = new List<Socket> ();
			_serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
			_serverSocket.Bind(new IPEndPoint(IPAddress.Parse(endpoint), port));
		    _serverSocket.SendBufferSize = Packet.DataLength;
			_serverSocket.Listen(_maxConnections);
			_acceptThread = new Thread ( AcceptCircle );
			_sendThread = new Thread(Send);
			_acceptThread.Start ();
			_sendThread.Start();
			Connected = true;
			Log.Info("VideoServerCreated");
	    }

	    private void Send()
	    {
		    while (Connected)
		    {
			    IPacket packet;
			    if (_dataQueue.TryDequeue(out packet))
			    {
				    foreach (var clientsSocket in _clientsSockets)
				    {
					    if (clientsSocket.Connected)
					    {
						    clientsSocket.Send(packet.GetSendData());
					    }
					    else
					    {
						    _clientsSockets.Remove(clientsSocket);
						    if (_clientsSockets.Any())
						    {
							    Connected = false;
						    }
					    }
				    }
			    }
		    }
	    }

	    public void NewFrame(byte[] frameData)
	    {
		    Log.Info("New frame: {0}", frameCounter);
		    var packets = DataToList(frameData);
		    foreach (var packet in packets)
		    {
			    _dataQueue.Enqueue(packet);
		    }
			frameCounter++;
	    }

	    private List<IPacket> DataToList(byte[] data )
	    {
		    int readLeft = data.Length;
		    int dataLength = readLeft;
			List<IPacket> packets = new List<IPacket> ();
		    var packetsCount = Packet.CalcNeededCount(data.Length);
			int bytesRead = 0;

		    int packetDataLength = Packet.DataLength;
		    for (int i = 0; i < packetsCount; i++)
		    {
			    var bytes = data.Skip(bytesRead).Take(packetDataLength).ToArray();
			    Packet packet = new Packet(bytes, frameCounter, dataLength);
				packets.Add (packet);
				bytesRead += packetDataLength;
		    }
		    return packets;
	    }

	    private void AcceptCircle()
	    {
			while (Running)
		    {
			    //var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

				var iar = _serverSocket.BeginAccept ( new AsyncCallback ( AcceptClient ) , _serverSocket );
			    using (var handle = iar.AsyncWaitHandle)
			    {
				    handle.WaitOne();
			    }
				break;
		    }
	    }

	    private void AcceptClient ( IAsyncResult iar )
	    {
		    Socket serverSocket = (Socket) iar.AsyncState;
		    Socket clientSocket = null;

		    clientSocket = serverSocket.EndAccept(iar);
			_clientsSockets.Add(clientSocket);
		    Connected = true;
			Log.Debug("Client connected");
	    }
    }
}
 