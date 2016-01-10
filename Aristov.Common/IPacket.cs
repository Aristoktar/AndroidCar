using System;

namespace Aristov.Common
{
    public interface IPacket
    {
	    byte[] Data { get; }
	    byte[] GetSendData();
	    int FrameLength { get; }
	    int FrameId { get; }
		int Taile{ get; }
	    //int Init(byte[] data);
	    //int CalcNeededCount(int dataLength);
    }
}
