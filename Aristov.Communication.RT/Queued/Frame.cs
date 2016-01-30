using System.Collections.Generic;
using System.IO;
using Aristov.Common;

namespace Aristov.Communication.RT.Queued {
	public class Frame: IFrame
	{
		private readonly byte[] _bites;
		public Frame(byte[] frameBytes,FrameType type = FrameType.ByteData)
		{
			_bites = frameBytes;
			FrameType = type;
		}
		public Frame(int packetsCount)
		{
			PacketsCount = packetsCount;
			Packets= new Dictionary<int, IPacket>();
		}
		public FrameType FrameType { get; private set; }
		public byte[] GetBytes()
		{
			if (PacketsCount != Packets.Count)
				throw new MyException {ExceptionType = MyExceptionType.PacketsCountNotMatch};
			return _bites;
		}

		public MemoryStream GetStream()
		{
			return new MemoryStream(GetBytes());
		}

		public int PacketsCount { get;private set; }
		public IDictionary<int, IPacket> Packets { get;private set; }
		public bool TryAddPacket(IPacket packet)
		{
			if(Packets.ContainsKey(packet.FrameId))
				throw new MyException{ExceptionType = MyExceptionType.PacketAlreadyContains};
			Packets.Add(packet.FrameId, packet);
			if (Packets.Count==PacketsCount)
			{
				return true;
			}
			return false;
		}
	}
}