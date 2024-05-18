using System;

namespace Scanner.Twain
{
    public class TransferFileEventArgs : EventArgs
    {
        public string FileName { get; set; }

        public bool ContinueScanning { get; set; }

        public TransferFileEventArgs(string fileName, bool continueScanning)
        {
            this.FileName = fileName;
            this.ContinueScanning = continueScanning;
        }

    }
}
