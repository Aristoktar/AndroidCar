using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace Aristov.Communication.RT.Queued
{
    public class Packet:IPacket
    {
	    public static readonly int HeaderLength = 2*sizeof(int);
	    public static readonly int DataLength;
		public static readonly int TaileLength = sizeof(int);
	    public byte[] Data { get;private set; }
	    public static int BufferSize = HeaderLength + DataLength + TaileLength;


	    public Packet(byte[] data,int frameId,int frameLength)
	    {
		    if (data.Length != DataLength){ throw new ArgumentException("Data length not valid"); }
		    byte[] header = BitConverter.GetBytes ( frameLength )
							.Union ( BitConverter.GetBytes ( frameId ) ).ToArray ();
		    byte[] taile = BitConverter.GetBytes((frameId + frameLength)/2).ToArray();
		    Data = header.Union(data).Union(taile).ToArray();
	    }

	    public static int CalcNeededCount(int dataLength)
	    {
			return dataLength / BufferSize + 1;
	    }
    }
}
