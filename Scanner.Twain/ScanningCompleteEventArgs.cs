using System;

namespace Scanner.Twain
{
    public class ScanningCompleteEventArgs : EventArgs
    {
        public Exception Exception { get; private set; }

        public ScanningCompleteEventArgs(Exception exception)
        {
            this.Exception = exception;
        }
    }
}
