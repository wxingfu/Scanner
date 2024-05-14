using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
