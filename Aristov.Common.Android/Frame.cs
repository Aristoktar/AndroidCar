using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Aristov.Common.Android {
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

		public MemoryStream GetStream()
		{
			throw new NotImplementedException();
		}

		public int PacketsCount { get; private set; }
		public IDictionary<int, IPacket> Packets { get; private set; }
		public bool TryAddPacket(IPacket packet)
		{
			throw new NotImplementedException();
		}
	}
}