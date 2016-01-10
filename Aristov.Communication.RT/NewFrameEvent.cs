using System;
using System.Collections.Generic;
using System.Text;

namespace Aristov.Communication.RT
{
    public class NewFrameEventArgsOLd:EventArgs
    {
	    public byte[] FrameBytes { get; set; }

	    public MediaType Type { get; set; }
    }

	public delegate void NewFrameEvent ( object sender , Queued.NewFrameEventArgs e );
}
