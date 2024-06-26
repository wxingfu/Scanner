﻿using System.Runtime.InteropServices;

namespace Scanner.Twain
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class ImageInfo
    {
        public int XResolution;

        public int YResolution;

        public int ImageWidth;

        public int ImageLength;

        public short SamplesPerPixel;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public short[] BitsPerSample;

        public short BitsPerPixel;

        public short Planar;

        public short PixelType;

        public Compression Compression;

    }
}
