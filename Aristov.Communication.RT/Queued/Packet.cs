using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Aristov.Common;

namespace Aristov.Communication.RT.Queued
{
    public class Packet:IPacket
    {
	    public static readonly int HeaderLength = 2*sizeof(int);
	    public static readonly int DataLength;
		public static readonly int TaileLength = sizeof(int);
	    public byte[] Data { get;private set; }
	    public int Id { get; private set; }
	    public byte[] GetSendData()
	    {
			byte[] header = BitConverter.GetBytes ( FrameLength )
							.Union ( BitConverter.GetBytes ( FrameId ) ).ToArray ();
			byte[] taile = BitConverter.GetBytes ( ( FrameId + FrameLength ) / 2 ).ToArray ();
			return header.Union ( Data ).Union ( taile ).ToArray ();
	    }

	    public int FrameLength { get; private set; }
	    public int FrameId { get; private set; }
	    public int Taile { get; private set; }

	    public static int BufferSize = HeaderLength + DataLength + TaileLength;

		/// <summary>
		/// For server side (with setting of frame attributes)
		/// </summary>
		/// <param name="data"></param>
		/// <param name="frameId"></param>
		/// <param name="frameLength"></param>
	    public Packet(byte[] data,int frameId,int frameLength)
		{
		    if (data.Length != DataLength){ throw new ArgumentException("Data length not valid"); }
			Data = data;
			FrameId = frameId;
			FrameLength = frameLength;

		}
		/// <summary>
		/// For Client Side (with parsing of frame attributes from data)
		/// </summary>
		/// <param name="data"></param>
		/// <param name="readedCount"></param>
	    public Packet(byte[] data,int readedCount)
		{
			int offset = sizeof (int);
			FrameLength = BitConverter.ToInt32(data, 0);
			FrameId = BitConverter.ToInt32(data, offset);
			Taile = BitConverter.ToInt32(data, readedCount - offset);
			if ( Taile != ( FrameId + FrameLength ) / 2 )
			{
				throw new MyException {ExceptionType = MyExceptionType.TaileNotConsistent};
			}
			Data = data.Skip(2*offset).Take(readedCount - 3*offset).ToArray();
		}

	    public static int CalcNeededCount(int dataLength)
	    {
			return dataLength / BufferSize + 1;
	    }
    }
}
