using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MD.Equipos.GPS.Garming
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct sat_data_type
    {
        public byte svid;
        public UInt16 snr;
        public byte elev;
        public short azmth;
        public byte status;
    }

}
