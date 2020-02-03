using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MD.Equipos.GPS.Garming
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Packet_t
    {
        public byte mPacketType;
        public byte mReserved1;
        public Int16 mReserved2;
        public Int16 mPacketId;
        public Int16 mReserved3;
        public Int64 mDataSize;
        public IntPtr mData;
    };

}
