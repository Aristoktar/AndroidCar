using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Aristov.Communication.RT
{
    public class Client
    {
	    ConcurrentQueue<byte[]> images { get; set; }

	    public Client(string host, int port)
	    {
		    TcpClient client = new TcpClient(host, port);

			EnqueueImage(client.GetStream());
	    }

		public void EnqueueImage ( NetworkStream stream )
	    {
		    int suzeofInt = sizeof (Int32);
		    var buffer = new byte[suzeofInt];

		    stream.Read(buffer, 0, suzeofInt);
		    PacketType packetType=  (PacketType)BitConverter.ToInt32(buffer, 0);
			stream.Read ( buffer ,0 , suzeofInt );
			int length = BitConverter.ToInt32 ( buffer , 0 );
			
			var data  =new byte[length];
		    stream.Read(data, 0, length);
			images.Enqueue(data);
	    }
    }
}
