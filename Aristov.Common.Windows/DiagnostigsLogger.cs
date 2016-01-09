using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aristov.Common.Windows
{
    public class DiagnostigsLogger:ILogger
    {
	    public void Warning(string messageFormat, params object[] args)
	    {
		    Trace.TraceWarning(messageFormat,args);
	    }

	    public void Warning(string message)
	    {
		    Warning(message, null);
	    }

	    public void Info(string messageFormat, params object[] args)
	    {
		    Trace.TraceInformation(messageFormat,args);
	    }

	    public void Info(string message)
	    {
		    Info(message, null);
	    }

	    public void Error(string messageFormat, params object[] args)
	    {
		    Trace.TraceError(messageFormat,args);
	    }

	    public void Error(string message)
	    {
		    Trace.TraceError(message, null);
	    }

	    public void Error(Exception ex)
	    {
		   Error("Exception:{0}",ex);
	    }

	    public void Debug(string messageFormat, params object[] args)
	    {
		    Trace.TraceWarning("Debug:"+messageFormat,args);
	    }

	    public void Debug(string message)
	    {
		    Debug(message, null);
	    }
    }
}
