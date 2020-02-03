using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MD.Equipos.GPS.Garming
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct pvt_data_type
    {
        public float alt; // altitude above WGS 84 
        public float epe; // estimated position error, 2 sigma (meters) 
        public float eph; // epe, but horizontal only (meters) 
        public float epv; // epe but vertical only (meters ) 
        public UInt16 fix; // 0 - failed integrity check
        // 1 - invalid or unavailable fix
        // 2 - 2D
        // 3 - 3D
        // 4 - 2D Diff
        // 5 - 3D Diff
        public double gps_tow; // gps time os week (seconds) */
        public double lat;     // latitude (radians) */
        public double lon;     // longitude (radians) */
        public float lon_vel;  // velovity east (meters/second) */
        public float lat_vel;  // velovity north (meters/second) */
        public float alt_vel;  // velovity up (meters/sec) */
        public float msl_hght; // height of WGS 84 above MSL */
        public short leap_sec; // diff between GPS and UTC (seconds) */
        public UInt64 grmn_days;
    }

}
