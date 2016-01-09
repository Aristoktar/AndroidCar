using System;

namespace Aristov.Common
{
    public interface ILogger
    {
	    void Warning(string messageFormat,params object[] args);
		void Warning ( string message);
		void Info ( string messageFormat , params object[] args );
	    void Info(string message);
		void Error ( string messageFormat , params object[] args );
	    void Error(string message);
		void Error ( Exception ex );
		void Debug ( string messageFormat , params object[] args );
		void Debug ( string message );

    }
}
