using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aristov.Common {
	public class MyException:Exception
	{

		public MyExceptionType ExceptionType { get; set; }
	}

	public enum MyExceptionType
	{
		PacketsCountNotMatch =0,
		TaileNotConsistent=1

	}
}
