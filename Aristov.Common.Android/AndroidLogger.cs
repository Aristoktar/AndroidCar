using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Util;

namespace Aristov.Common.Android
{
	public class AndroidLogger:ILogger
	{
		string _logTag;

		public AndroidLogger()
		{
			_logTag = "Aristov.Common";
		}

		public void Warning(string messageFormat, params object[] args)
		{
			Log.Warn(_logTag, messageFormat, args);
		}

		public void Warning(string message)
		{
			Log.Warn(_logTag, message);
		}

		public void Info(string messageFormat, params object[] args) 
		{
			Log.Info(_logTag , messageFormat , args );
		}

		public void Info(string message)
		{
			Log.Info(_logTag , message );
		}

		public void Error(string messageFormat, params object[] args)
		{
			Log.Error(_logTag, messageFormat, args);
		}

		public void Error(string message)
		{
			Log.Error(_logTag , message );
		}

		public void Error(Exception ex)
		{
			Log.Error(_logTag, ex.ToString());
		}

		public void Debug(string messageFormat, params object[] args)
		{
			Log.Debug(_logTag , messageFormat , args );
		}

		public void Debug(string message)
		{
			Log.Debug(_logTag , message );
		}
	}
}
