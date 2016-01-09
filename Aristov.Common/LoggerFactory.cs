using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aristov.Common {
	public static class LoggerFactory
	{
		private static Type _loggerType;

		public static void Setup ( Type loggerType )
		{
			_loggerType = loggerType;
		}

		public static void Setup ( string loggerTypeString )
		{
			_loggerType = Type.GetType(loggerTypeString, true);
		}

		public static ILogger Create()
		{
			return (ILogger) Activator.CreateInstance(_loggerType);
		}
	}
}
