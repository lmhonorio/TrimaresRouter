﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace MAVcomm.Utilities
{

    

    public struct Locationwp
    {
        public byte id;				// command id
        public byte options;
        public float p1;				// param 1
        public float p2;				// param 2
        public float p3;				// param 3
        public float p4;				// param 4
        public float lat;				// Lattitude * 10**7
        public float lng;				// Longitude * 10**7
        public float alt;				// Altitude in centimeters (meters * 100)

        public static List<Locationwp> getWP(ref MainCrossData mydata)
        {
            List<Locationwp> cmds = new List<Locationwp>();

            try
            {
                MAVLink port = mydata.myLink;

                if (!port.BaseStream.IsOpen)
                {
                    throw new Exception("Please Connect First!");
                }

                MainCrossData.giveComport = true;


                int cmdcount = port.getWPCount();

                for (ushort a = 0; a < cmdcount; a++)
                {
                    cmds.Add(port.getWP(a));
                }

                port.setWPACK();

            }
            catch { }

            return cmds;

        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", id, options, p1, p2, p3, p4, lat, lng, alt);
        }
    };


    public class PointLatLngAlt
    {
        public double Lat = 0;
        public double Lng = 0;
        public double Alt = 0;
        public string Tag = "";
        public Color color = Color.White;

        const float rad2deg = (float)(180 / Math.PI);
        const float deg2rad = (float)(1.0 / rad2deg);

        public PointLatLngAlt(double lat, double lng, double alt, string tag)
        {
            this.Lat = lat;
            this.Lng = lng;
            this.Alt = alt;
            this.Tag = tag;
        }

        public PointLatLngAlt()
        {

        }

        public PointLatLngAlt(GMap.NET.PointLatLng pll)
        {
            this.Lat = pll.Lat;
            this.Lng = pll.Lng;
        }

        public PointLatLngAlt(Locationwp locwp)
        {
            this.Lat = locwp.lat;
            this.Lng = locwp.lng;
            this.Alt = locwp.alt;
        }

        public PointLatLngAlt(PointLatLngAlt plla)
        {
            this.Lat = plla.Lat;
            this.Lng = plla.Lng;
            this.Alt = plla.Alt;
            this.color = plla.color;
            this.Tag = plla.Tag;
        }

        public PointLatLng Point()
        {
            return new PointLatLng(Lat, Lng);
        }

        public override bool Equals(Object pllaobj)
        {
            PointLatLngAlt plla = (PointLatLngAlt)pllaobj;

            if (plla == null)
                return false;

            if (this.Lat == plla.Lat &&
            this.Lng == plla.Lng &&
            this.Alt == plla.Alt &&
            this.color == plla.color &&
            this.Tag == plla.Tag)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (int)((Lat + Lng + Alt) * 100);
        }

        public override string ToString()
        {
            return Lat + "," + Lng + "," + Alt;
        }

      

        /// <summary>
        /// Calc Distance in M
        /// </summary>
        /// <param name="p2"></param>
        /// <returns>Distance in M</returns>
        public double GetDistance(PointLatLngAlt p2)
        {
            double d = Lat * 0.017453292519943295;
            double num2 = Lng * 0.017453292519943295;
            double num3 = p2.Lat * 0.017453292519943295;
            double num4 = p2.Lng * 0.017453292519943295;
            double num5 = num4 - num2;
            double num6 = num3 - d;
            double num7 = Math.Pow(Math.Sin(num6 / 2.0), 2.0) + ((Math.Cos(d) * Math.Cos(num3)) * Math.Pow(Math.Sin(num5 / 2.0), 2.0));
            double num8 = 2.0 * Math.Atan2(Math.Sqrt(num7), Math.Sqrt(1.0 - num7));
            return (6371 * num8) * 1000.0; // M
        }

        public double GetDistance2(PointLatLngAlt p2)
        {
            //http://www.movable-type.co.uk/scripts/latlong.html
            var R = 6371; // 6371 km
            var dLat = (p2.Lat - Lat) * deg2rad;
            var dLon = (p2.Lng - Lng) * deg2rad;
            var lat1 = Lat * deg2rad;
            var lat2 = p2.Lat * deg2rad;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c * 1000.0; // M

            return d;
        }
    }
}
