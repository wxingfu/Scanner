using System;

namespace Scanner.Twain
{
    public class ScanningEventArgs : EventArgs
    {
        public short Message{get; set;}

        public ScanningEventArgs(short message)
        {
            this.Message = message;
        }

    }
}
