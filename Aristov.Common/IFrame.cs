using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Aristov.Common {
	public interface IFrame
	{
		FrameType FrameType { get; }
		byte[] GetBytes();
		MemoryStream GetStream();
		int PacketsCount { get; }
		IDictionary<int,IPacket> Packets { get; }

		/// <summary>
		/// Adds packet to frame
		/// </summary>
		/// <param name="packet"></param>
		/// <returns>Is frame filled</returns>
		bool TryAddPacket(IPacket packet);
	}
}
