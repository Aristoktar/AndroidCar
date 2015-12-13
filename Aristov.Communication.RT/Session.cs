using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace Aristov.Communication.RT
{
    public class Session
    {
	    private TcpClient _client;
	    public Guid Id { get; private set; }
	    public TimeSpan TimeOut  = new TimeSpan();


	    public Session ( TcpClient client )
	    {
			Id=new Guid();
			_client = client;
	    }

	    public void AddNewFrame(byte[] frameBytes)
	    {
			Stopwatch timer = new Stopwatch();
			timer.Start();
		    var stream = _client.GetStream();
		    int request = stream.ReadByte();
		    if (timer.Elapsed > TimeOut)
			    throw new TimeoutException(String.Format("Time between last frame and request is more than {0}", TimeOut));
			timer.Stop();
			var packet = new Packet(frameBytes, false);
		    var senddata = packet.GetSendData();
			stream.Write(senddata,0,senddata.Length);
	    }
    }
}
