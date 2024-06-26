﻿using System;
using System.Runtime.InteropServices;

namespace Scanner.Twain
{
    public static class Gdi32Native
    {

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern int SetDIBitsToDevice(IntPtr hdc, int xdst, int ydst, int width, int height, int xsrc, int ysrc, int start, int lines, IntPtr bitsptr, IntPtr bmiptr, int color);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern bool DeleteObject(IntPtr hObject);

    }
}
