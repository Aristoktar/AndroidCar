using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aristov.Common {
	interface IFrameConverter<T> where T:IFrame
	{
		T ConvertFromBytes(byte[] bytes );
		byte[] ConvertToBytes(T image);
	}
}
