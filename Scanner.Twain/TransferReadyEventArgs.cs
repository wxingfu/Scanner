using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
