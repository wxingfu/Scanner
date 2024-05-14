using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner.Twain
{
    public class ScanningExceptionArgs : EventArgs
    {
        public Exception Exception { get; set; }

        public ScanningExceptionArgs(string message)
        {
            Exception exception = new Exception(message);
            this.Exception = exception;
        }

        public ScanningExceptionArgs(Exception exception)
        {
            this.Exception = exception;
        }

        public ScanningExceptionArgs()
        {
            this.Exception = null;
        }
    }
}
