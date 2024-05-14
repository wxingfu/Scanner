﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Scanner.Twain
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class Status
    {
        public ConditionCode ConditionCode;
        public short Reserved;
    }
}
