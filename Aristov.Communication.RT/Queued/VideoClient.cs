using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Aristov.Common;

namespace Aristov.Communication.RT.Queued
{
	
    public class VideoClient
    {
		ILogger Log = LoggerFactory.Create ();
	    Socket _socket;
	    int _bufferSize;
	    Thread _receiveThread;
		public event NewFrameEventHandler NewFrame;
	    public VideoClient(string host, int port)
	    {
		    _bufferSize = Packet.BufferSize;
		    _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//_socket.AddressFamily = AddressFamily.InterNetwork;
		    _socket.Connect(new IPEndPoint(IPAddress.Parse(host), port));
		    _socket.ReceiveBufferSize = _bufferSize;
			_receiveThread = new Thread(Recive);
			_receiveThread.Start();
			Log.Debug("Video client .ctor inited");
	    }

	    public void Recive()
	    {
		    Dictionary<int, IFrame> frames = new Dictionary<int, IFrame>();
		    while (true)
		    {
				byte[] buffer = new byte[_bufferSize];
			    int bytesReceived = _socket.Receive(buffer);

				//bytes received?
				IPacket packet = new Packet(buffer,bytesReceived);
			    IFrame frame;
			    if (!frames.TryGetValue(packet.FrameId, out frame)) continue;

			    if (!frame.TryAddPacket(packet)) continue;

			    if ( NewFrame != null ) {
				    NewFrame ( this , new NewFrameEventArgs {Frame = frame} );
			    }
		    }
	    }
    }

	public delegate void NewFrameEventHandler ( object sender , NewFrameEventArgs e );

	public class NewFrameEventArgs:EventArgs
	{
		public IFrame Frame { get; set; }
	}
}
