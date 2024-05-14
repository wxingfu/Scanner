using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
