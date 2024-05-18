using System;

namespace Scanner.Twain
{
    public class ScanningStartEventArgs : EventArgs
    {
        public bool Cancel { get; set; }

        public ScanningStartEventArgs()
        {
            this.Cancel = false;
        }

    }
}
