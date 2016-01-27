using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MAVcomm;

namespace clsExodn
{

    public struct exo_waterquality
    {


        /// <summary>
        /// 
        /// </summary>
        public double lat { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double lon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ulong id { get; set; }
        /// <summary>
        /// < Timestamp (microseconds since UNIX epoch or microseconds since system boot)>
        /// </summary>
        public ulong time_usec { get; set; }
        /// <summary>
        /// < Depth in the measured position in cm (m*100). If unknown, set to: 0 >
        /// </summary>
        /// 
        public Single depth { get; set; }
        /// <summary>
        /// <  Condutivity measured in microSiemens/centimeter. If unknown, set to: 65535>
        /// </summary>
        public Single conductivity { get; set; }
        /// <summary>
        /// < Temperature measured in Celsius. If unknown, set to: 65535>
        /// </summary>
        public Single temperature { get; set; }
        /// <summary>
        /// < Salinity measured in pu. If unknown, set to: 65535>
        /// </summary>
        public Single salinity { get; set; }
        /// <summary>
        /// < Non linear function condutivity. If unknown, set to: 65535>
        /// </summary>
        public Single nLF { get; set; }
        /// <summary>
        /// < Specific conductance. If unknown, set to: 65535>
        /// </summary>
        public Single conductance { get; set; }
        /// <summary>
        /// < Total of dissolved solids. If unknown, set to: 65535>
        /// </summary>
        public Single tot_dissolved_solids { get; set; }
        /// <summary>
        /// < Dissolved oxygen. If unknown, set to: 65535>
        /// </summary>
        public Single dissolved_oxygen { get; set; }
        /// <summary>
        /// < Chloride guarded sensor measured in mg/L-CI. If unknown, set to: 65535>
        /// </summary>
        public Single chloride { get; set; }
        /// <summary>
        /// < Ph measured in . If unknown, set to: 65535>
        /// </summary>
        public Single ph { get; set; }
        /// <summary>
        /// < Oxidizing-reducing potential measured in millivolts. If unknown, set to: 65535>
        /// </summary>
        public Single ORP { get; set; }
        /// <summary>
        /// < Turbidity measured in formazin nephelometric units, FNU. If unknown, set to: 65535>
        /// </summary>
        public Single turbidity { get; set; }


        /// <summary>
        /// <function > set the values</function>
        /// </summary>
        /// <param name="ntime_usec">Timestamp (microseconds since UNIX epoch or microseconds since system boot)</param>
        /// <param name="ndepth">Depth in the measured position in cm (m*100). If unknown, set to: 0</param>
        /// <param name="nconductivity">Condutivity measured in microSiemens/centimeter. If unknown, set to: 65535</param>
        /// <param name="ntemperature">Salinity measured in pu. If unknown, set to: 65535</param>
        /// <param name="nsalinity">Salinity measured in pu. If unknown, set to: 65535</param>
        /// <param name="nnLF">Non linear function condutivity. If unknown, set to: 65535</param>
        /// <param name="nconductance">Specific conductance. If unknown, set to: 65535</param>
        /// <param name="ntot_dissolved_solids">Total of dissolved solids. If unknown, set to: 65535</param>
        /// <param name="ndissolved_oxygen">Dissolved oxygen. If unknown, set to: 65535</param>
        /// <param name="nchloride">Chloride guarded sensor measured in mg/L-CI. If unknown, set to: 65535</param>
        /// <param name="nph">Oxidizing-reducing potential measured in millivolts. If unknown, set to: 65535</param>
        /// <param name="nORP">Oxidizing-reducing potential measured in millivolts. If unknown, set to: 65535</param>
        /// <param name="nturbidity">Turbidity measured in formazin nephelometric units, FNU. If unknown, set to: 65535</param>
        public void setValues(UInt32 ntime_usec, UInt32 ndepth, UInt32 nconductivity, UInt32 ntemperature, UInt32 nsalinity,
                                UInt32 nnLF, UInt32 nconductance, UInt32 ntot_dissolved_solids, UInt32 ndissolved_oxygen,
                                UInt32 nchloride, UInt32 nph, UInt32 nORP, UInt32 nturbidity)
        {
            time_usec = ntime_usec;
            depth = ndepth;
            conductivity = nconductivity;
            temperature = ntemperature;
            salinity = nsalinity;
            nLF = nnLF;
            conductance = nconductance;
            tot_dissolved_solids = ntot_dissolved_solids;
            dissolved_oxygen = ndissolved_oxygen;
            chloride = nchloride;
            ph = nph;
            ORP = nORP;
            turbidity = nturbidity;
        }

        public void setLatLon(double nlat, double nlon) 
        {
            lat = nlat;
            lon = nlon;
        }

         public static string returnID()
        {
            return String.Format("{0:yyMMddHHmmss}", DateTime.Now); 
        }

        public void setValues(UInt32[] parameters)
        {
            string s = returnID();
            time_usec = Convert.ToUInt64(s);
            depth = parameters[0];
            conductivity = parameters[1];
            temperature = parameters[2];
            salinity = parameters[3];
            nLF = parameters[4];
            conductance = parameters[5];
            tot_dissolved_solids = parameters[6];
            dissolved_oxygen = parameters[7];
            chloride = parameters[8];
            ph = parameters[8];
            ORP = parameters[10];
            turbidity = parameters[11];
        }

        public void setValues(string[] parameters)
        {

            float[] spara = new float[14];

            for (int i = 1; i < 13; i++)
            {
                spara[i-1] = parameters[i] == "-nan" ? 65000 : Convert.ToSingle(parameters[i]);

            }

            try
            {
                int hour = Convert.ToInt16(parameters[1].Substring(0, 2));
                int min = Convert.ToInt16(parameters[1].Substring(2, 2));
                int sec = Convert.ToInt16(parameters[1].Substring(4, 2));
                DateTime x = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, hour, min, sec);
                x.ToLongDateString();
                string s = String.Format("{0:yyMMddHHmmss}", DateTime.Now); 
                time_usec = Convert.ToUInt64(s);
            }
            catch { time_usec = 0; }
            id = time_usec;
            depth = spara[1];
            temperature = spara[2];
            salinity = spara[3];
            conductivity = spara[4];
            conductance = spara[5];
            nLF = spara[6];
            tot_dissolved_solids = spara[7];
            dissolved_oxygen = spara[8];
            chloride = spara[9];
            ph = spara[10];
            ORP = spara[11];
            turbidity = spara[12];
        }



        public string toText()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}", id, lat, lon, depth, temperature, salinity, conductivity, conductance, nLF, 
                tot_dissolved_solids, dissolved_oxygen, chloride, ph, ORP, turbidity);
        }

        public static explicit operator MAVLink.mavlink_exo_waterquality_t(exo_waterquality strtype) 
        {
            MAVLink.mavlink_exo_waterquality_t exo = new MAVLink.mavlink_exo_waterquality_t();
           // exo.id = (uint)mavtype.time_usec;
            exo.time_usec = strtype.time_usec;
            exo.lat = (int)(strtype.lat*1e7);
            exo.lon = (int)(strtype.lon*1e7);
            exo.chloride = (int)(strtype.chloride*1000f);
            exo.conductance = (int)(strtype.conductance * 1000f);
            exo.conductivity = (int)(strtype.conductivity * 1000f);
            exo.depth = (int)(strtype.depth * 1000f);
            exo.dissolved_oxygen = (int)(strtype.dissolved_oxygen * 1000f);
            exo.nLF = (int)(strtype.nLF * 1000f);
            exo.ORP = (int)(strtype.ORP * 1000f);
            exo.ph = (int)(strtype.ph * 1000f);
            exo.salinity = (int)(strtype.salinity * 1000f);
            exo.temperature = (int)(strtype.temperature * 1000f);
            exo.tot_dissolved_solids = (int)(strtype.tot_dissolved_solids * 1000f);
            exo.turbidity = (int)(strtype.turbidity * 1000f);

            return exo;
        }

        public static explicit operator exo_waterquality(MAVLink.mavlink_exo_waterquality_t mavtype)
        {

            exo_waterquality exo = new exo_waterquality();

            exo.id =  mavtype.time_usec;
            exo.time_usec = mavtype.time_usec;
            exo.lat = ((double)mavtype.lat / (double)1e7);
            exo.lon = ((double)mavtype.lon / (double)1e7);
            exo.chloride = ((float)mavtype.chloride)/1000.0f;
            exo.conductance = ((float)mavtype.conductance)/ 1000.0f;
            exo.conductivity = (float)mavtype.conductivity / 1000.0f;
            exo.depth = (float)mavtype.depth / 1000.0f;
            exo.dissolved_oxygen = (float)mavtype.dissolved_oxygen / 1000.0f;
            exo.nLF = (float)mavtype.nLF / 1000.0f;
            exo.ORP = (float)mavtype.ORP / 1000.0f;
            exo.ph = (float)mavtype.ph / 1000.0f;
            exo.salinity = (float)mavtype.salinity / 1000.0f;
            exo.temperature = (float)mavtype.temperature / 1000.0f;
            exo.tot_dissolved_solids = (float)mavtype.tot_dissolved_solids / 1000.0f;
            exo.turbidity = (float)mavtype.turbidity / 1000.0f;

            return exo;
        }

    };
}
