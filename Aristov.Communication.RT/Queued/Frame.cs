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

		public FrameType FrameType { get; private set; }

		public byte[] GetBytes()
		{
			return _bites;
		}
	}
}