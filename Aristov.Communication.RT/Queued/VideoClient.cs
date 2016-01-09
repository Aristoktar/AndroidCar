using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Aristov.Common;

namespace Aristov.Communication.RT.Queued
{
    public class VideoClient
    {
		ILogger Log = LoggerFactory.Create ();
	    Socket _socket;
	    public VideoClient(string host, int port)
	    {
		    _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
			_socket.Connect ( new IPEndPoint ( IPAddress.Parse ( host ) , port ) );
		    _socket.ReceiveBufferSize = Packet.BufferSize;
	    }

	    public void Recive()
	    {
		    while (true)
		    {
			    byte[] buffer =new byte[Packet.BufferSize];
			    int bytesReceived = _socket.Receive(buffer);
				IFrame frame = new Frame(buffer.Take(bytesReceived).ToArray());

		    }
	    }
    }
}
