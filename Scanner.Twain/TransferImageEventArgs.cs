using System;
using System.Drawing;

namespace Scanner.Twain
{
    public class TransferImageEventArgs : EventArgs
    {

        public Bitmap Image { get; private set; }

        public bool ContinueScanning { get; set; }

        public int Bit { get; private set; }

        public TransferImageEventArgs(Bitmap image, bool continueScanning)
        {
            this.Image = image;
            this.ContinueScanning = continueScanning;
        }


        public TransferImageEventArgs(Bitmap image, int bit, bool continueScanning)
        {
            this.Image = image;
            this.Bit = bit;
            this.ContinueScanning = continueScanning;
        }
    }
}
