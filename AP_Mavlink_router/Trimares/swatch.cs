using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using MAVcomm;

namespace AP_Mavlink_router.Trimares
{


    public class swatch
    {

        public swatch() { }

        public static byte[] ReturnEullerBytes(double nrow, double npitch, double nyaw) 
        {
           // euller Euller = new euller { row = nrow, pitch = npitch, yaw = nyaw };
            byte[] t1 = BitConverter.GetBytes(nrow);
            byte[] t2 = BitConverter.GetBytes(npitch);
            byte[] t3 = BitConverter.GetBytes(nyaw);

            //if (BitConverter.IsLittleEndian)
            //{
            //    Array.Reverse(t1);
            //    Array.Reverse(t2);
            //    Array.Reverse(t3);
            //}
            byte[] t = new byte[24];

            Array.Copy(t1, 0, t, 0, 8);
            Array.Copy(t2, 0, t, 8, 8);
            Array.Copy(t3, 0, t, 16, 8);


         //   byte[] t = Array.ConstrainedCopy //MavlinkUtil.StructureToByteArray(Euller);
            return t;
        }


        public static byte[] ReturnLatLonBytes(double lat, double lon)
        {
            // euller Euller = new euller { row = nrow, pitch = npitch, yaw = nyaw };
            byte[] t1 = BitConverter.GetBytes(lat);
            byte[] t2 = BitConverter.GetBytes(lon);

            //if (BitConverter.IsLittleEndian)
            //{
            //    Array.Reverse(t1);
            //    Array.Reverse(t2);
            //    Array.Reverse(t3);
            //}
            byte[] t = new byte[16];

            Array.Copy(t1, 0, t, 0, 8);
            Array.Copy(t2, 0, t, 8, 8);


            //   byte[] t = Array.ConstrainedCopy //MavlinkUtil.StructureToByteArray(Euller);
            return t;
        }

        public byte[] CreateMessage(double nrow, double npitch, double nyaw) 
        {
            byte[] msgEnviar = new byte[17];

            msgEnviar[0] = 0xFA;
            msgEnviar[1] = 0xFF;
            msgEnviar[2] = 0x32;
            msgEnviar[3] = 0x0C;
            byte[] eullers = swatch.ReturnEullerBytes(nrow, npitch, nyaw);
           // byte[] eullers = new byte[] { 191, 233, 145, 109, 63, 125, 227, 15, 194, 9, 169, 122 };
            Array.Copy(eullers, 0, msgEnviar, 4, 12);

            msgEnviar[16] = Convert.ToByte(CheckSum(msgEnviar));

            return msgEnviar;
        }

        private int CheckSum(byte[] msg)
        {
            int sum = 0;
            for (int i = 1; i < msg.Length-1; i++)
            {
                sum += msg[i];
            }
            int checksum;
            checksum = 256 - (sum % 256);

            return checksum;
        }

        public static int CheckSumPOSMV(byte[] msg)
        {
            int sum = 0;
            for (int i = 1; i < msg.Length - 1; i++)
            {
                sum += msg[i];
            }
            int checksum;
            checksum = 65536 - (sum % 65536);

            return checksum;
        }


        public const byte MAVLINK_MSG_ID_SWATCH = 210;
        [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12)]
        public struct euller
        {
            /// <summary> Latitude in 1E7 degrees </summary>
            public float row;
            /// <summary> Longitude in 1E7 degrees </summary>
            public float pitch;
            /// <summary> Altitude in 1E3 meters (millimeters) above MSL </summary>
            public float yaw;
            
        };


        public const byte MAVLINK_MSG_ID_gps = 211;
        [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12)]
        public struct gps
        {
            /// <summary> Latitude in 1E7 degrees </summary>
            public float lat;
            /// <summary> Longitude in 1E7 degrees </summary>
            public float lng;
            /// <summary> Altitude in 1E3 meters (millimeters) above MSL </summary>
            public float alt;

        };

    }


}
