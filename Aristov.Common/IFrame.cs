using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aristov.Common {
	public interface IFrame
	{
		FrameType FrameType { get; }
		byte[] GetBytes();
	}
}
