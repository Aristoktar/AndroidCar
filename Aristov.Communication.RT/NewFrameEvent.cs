using System;
using System.Collections.Generic;
using System.Text;

namespace Aristov.Communication.RT
{
    public class NewFrameEventArgs:EventArgs
    {
	    public byte[] FrameBytes { get; set; }

	    public MediaType Type { get; set; }
    }

	public delegate void  NewFrameEvent(object sender, NewFrameEvent e);
}
