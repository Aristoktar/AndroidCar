using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Timers;


namespace Aristov.Communication.RT
{
    public class Client
    {
	    ConcurrentQueue<byte[]> images { get; set; }

		public event NewFrameEvent NewFrameEventHandler;
		Timer _timer = new Timer();
	    TcpClient _client;

		public bool Stoped {
			get;
			set;
		}

	    public Client(string host, int port)
	    {
		    _client = new TcpClient(host, port);
		    Stoped = false;
			_timer.Elapsed += _timer_Elapsed;
			_timer.Start();
			
	    }

		void _timer_Elapsed ( object sender , ElapsedEventArgs e ) {
			EnqueueImage ( _client.GetStream () );
		}

	    public void Disconnect()
	    {
		    Stoped = true;
	    }


	    private void EnqueueImage ( NetworkStream stream )
	    {
		    int suzeofInt = sizeof (Int32);
		    var buffer = new byte[suzeofInt];
			stream.WriteByte(new byte());
		    stream.Read(buffer, 0, suzeofInt);
		    PacketType packetType = (PacketType)BitConverter.ToInt32(buffer, 0);
			stream.Read ( buffer ,0 , suzeofInt );
			int length = BitConverter.ToInt32 ( buffer , 0 );
			
			var data  =new byte[length];
		    stream.Read(data, 0, length);
			if (NewFrameEventHandler != null)
			{
				NewFrameEventHandler(this, new NewFrameEventArgs()
				{
					FrameBytes = data,
					Type = MediaType.Jpeg //Hardcode!=) for a while

				});
			}
			if(!Stoped)
				EnqueueImage(stream);
			//images.Enqueue(data);
	    }
    }
}
