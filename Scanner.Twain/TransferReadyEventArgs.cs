using System;

namespace Scanner.Twain
{
    public class TransferReadyEventArgs : EventArgs
    {
        public bool ContinueScanning { get; set; }

        public TransferReadyEventArgs(bool continueScanning)
        {
            this.ContinueScanning = continueScanning;
        }

        public TransferReadyEventArgs()
        {
            this.ContinueScanning = true;
        }
    }
}
