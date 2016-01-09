using System;
using System.Collections.Generic;
using System.Text;

namespace Aristov.Communication.RT.Queued
{
    public interface IPacket
    {
	    byte[] Data { get; }
	    //int Init(byte[] data);
	    //int CalcNeededCount(int dataLength);
    }
}
