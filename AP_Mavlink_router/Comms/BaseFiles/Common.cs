using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using MAVcomm.Attributes;


using System.Security.Cryptography.X509Certificates;

using System.Net;
using System.Net.Sockets;
using System.Xml; // config file
using System.Runtime.InteropServices; // dll imports
//using log4net;

using MAVcomm;
using System.Reflection;
using MAVcomm.Utilities;

using System.IO;

using System.Drawing.Drawing2D;

namespace MAVcomm
{


    public class Common
    {
       // private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public enum distances
        {
            Meters,
            Feet
        }

        public enum speeds
        {
            ms,
            fps,
            kph,
            mph,
            knots
        }

        public enum ap_product
        {
            [DisplayText("HIL")]
            AP_PRODUCT_ID_NONE = 0x00,	// Hardware in the loop
            [DisplayText("APM1 1280")]
            AP_PRODUCT_ID_APM1_1280 = 0x01,// APM1 with 1280 CPUs
            [DisplayText("APM1 2560")]
            AP_PRODUCT_ID_APM1_2560 = 0x02,// APM1 with 2560 CPUs
            [DisplayText("SITL")]
            AP_PRODUCT_ID_SITL = 0x03,// Software in the loop
            [DisplayText("APM2 ES C4")]
            AP_PRODUCT_ID_APM2ES_REV_C4 = 0x14,// APM2 with MPU6000ES_REV_C4
            [DisplayText("APM2 ES C5")]
            AP_PRODUCT_ID_APM2ES_REV_C5 = 0x15,	// APM2 with MPU6000ES_REV_C5
            [DisplayText("APM2 ES D6")]
            AP_PRODUCT_ID_APM2ES_REV_D6 = 0x16,	// APM2 with MPU6000ES_REV_D6
            [DisplayText("APM2 ES D7")]
            AP_PRODUCT_ID_APM2ES_REV_D7 = 0x17,	// APM2 with MPU6000ES_REV_D7
            [DisplayText("APM2 ES D8")]
            AP_PRODUCT_ID_APM2ES_REV_D8 = 0x18,	// APM2 with MPU6000ES_REV_D8	
            [DisplayText("APM2 C4")]
            AP_PRODUCT_ID_APM2_REV_C4 = 0x54,// APM2 with MPU6000_REV_C4 	
            [DisplayText("APM2 C5")]
            AP_PRODUCT_ID_APM2_REV_C5 = 0x55,	// APM2 with MPU6000_REV_C5 	
            [DisplayText("APM2 D6")]
            AP_PRODUCT_ID_APM2_REV_D6 = 0x56,	// APM2 with MPU6000_REV_D6 		
            [DisplayText("APM2 D7")]
            AP_PRODUCT_ID_APM2_REV_D7 = 0x57,	// APM2 with MPU6000_REV_D7 	
            [DisplayText("APM2 D8")]
            AP_PRODUCT_ID_APM2_REV_D8 = 0x58,	// APM2 with MPU6000_REV_D8 	
            [DisplayText("APM2 D9")]
            AP_PRODUCT_ID_APM2_REV_D9 = 0x59	// APM2 with MPU6000_REV_D9 
        }

        public enum apmmodes
        {
            [DisplayText("Manual")]
            MANUAL = 0,
            [DisplayText("Circle")]
            CIRCLE = 1,
            [DisplayText("Stabilize")]
            STABILIZE = 2,
            [DisplayText("FBW A")]
            FLY_BY_WIRE_A = 5,
            [DisplayText("FBW B")]
            FLY_BY_WIRE_B = 6,
            [DisplayText("Auto")]
            AUTO = 10,
            [DisplayText("RTL")]
            RTL = 11,
            [DisplayText("Loiter")]
            LOITER = 12,
            [DisplayText("Guided")]
            GUIDED = 15
        }

        public enum aprovermodes
        {
            [DisplayText("Manual")]
            MANUAL = 0,
            [DisplayText("Circle")]
            CIRCLE = 1,
            [DisplayText("Learning")]
            LEARNING = 2,
            [DisplayText("FBW A")]
            FLY_BY_WIRE_A = 5,
            [DisplayText("FBW B")]
            FLY_BY_WIRE_B = 6,
            [DisplayText("Auto")]
            AUTO = 10,
            [DisplayText("RTL")]
            RTL = 11,
            [DisplayText("Loiter")]
            LOITER = 12,
            [DisplayText("Guided")]
            GUIDED = 15,
            HEADALT = 17,
            SARSEC = 18,
            SARGRID = 19,
            THERMAL = 20,
            LAND = 21
        }

        public enum ac2modes
        {
            [DisplayText("Stabilize")]
            STABILIZE = 0,			// hold level position
            [DisplayText("Acro")]
            ACRO = 1,			// rate control
            [DisplayText("Alt Hold")]
            ALT_HOLD = 2,		// AUTO control
            [DisplayText("Auto")]
            AUTO = 3,			// AUTO control
            [DisplayText("Guided")]
            GUIDED = 4,		// AUTO control
            [DisplayText("Loiter")]
            LOITER = 5,		// Hold a single location
            [DisplayText("RTL")]
            RTL = 6,				// AUTO control
            [DisplayText("Circle")]
            CIRCLE = 7,
            [DisplayText("Pos Hold")]
            POSITION = 8,
            [DisplayText("Land")]
            LAND = 9,				// AUTO control
            OF_LOITER = 10,
            [DisplayText("Toy")]
			TOY = 11
        }

        public enum ac2ch7modes
        {
            [DisplayText("Do Nothing")]
            CH7_DO_NOTHING = 0,
            [DisplayText("Set Hover")]
            CH7_SET_HOVER = 1,
            [DisplayText("Flip")]
            CH7_FLIP = 2,
            [DisplayText("Simple Mode")]
            CH7_SIMPLE_MODE = 3,
            [DisplayText("Return to Launch")]
            CH7_RTL = 4,
            [DisplayText("Automatic Trim")]
            CH7_AUTO_TRIM = 5,
            [DisplayText("ADC Filter")]
            CH7_ADC_FILTER = 6,
            [DisplayText("Save Waypoint")]
            CH7_SAVE_WP = 7
        }

        public enum ac2ch6modes
        {
            // CH_6 Tuning
            // -----------
            CH6_NONE = 0,
            // Attitude
            CH6_STABILIZE_KP = 1,
            CH6_STABILIZE_KI = 2,
            CH6_YAW_KP = 3,
            // Rate
            CH6_RATE_KP = 4,
            CH6_RATE_KI = 5,
            CH6_RATE_KD = 21,
            CH6_YAW_RATE_KP = 6,
            // Altitude rate controller
            CH6_THROTTLE_KP = 7,
            // Extras
            CH6_TOP_BOTTOM_RATIO = 8,
            CH6_RELAY = 9,
            CH6_TRAVERSE_SPEED = 10,

            CH6_NAV_P = 11,
            CH6_LOITER_P = 12,
            CH6_HELI_EXTERNAL_GYRO = 13,

            // altitude controller
            CH6_THR_HOLD_KP = 14,
            CH6_Z_GAIN = 15,
            //CH6_DAMP = 16,

            // optical flow controller
            CH6_OPTFLOW_KP = 17,
            CH6_OPTFLOW_KI = 18,
            CH6_OPTFLOW_KD = 19,

            CH6_NAV_I = 20,
            CH6_LOITER_RATE_P = 22,
            CH6_LOITER_RATE_D = 23,
            CH6_YAW_KI = 24,
            CH6_ACRO_KP = 25,
            CH6_YAW_RATE_KD = 26,
            CH6_LOITER_KI = 27,
            CH6_LOITER_RATE_KI = 28,
            CH6_STABILIZE_KD = 29
        }

        public static void linearRegression()
        {
            double[] values = { 4.8, 4.8, 4.5, 3.9, 4.4, 3.6, 3.6, 2.9, 3.5, 3.0, 2.5, 2.2, 2.6, 2.1, 2.2 };
            
            double xAvg = 0;
            double yAvg = 0;

            for (int x = 0; x < values.Length; x++)
            {
                xAvg += x;
                yAvg += values[x];
            }

            xAvg = xAvg / values.Length;
            yAvg = yAvg / values.Length;


            double v1 = 0;
            double v2 = 0;

            for (int x = 0; x < values.Length; x++)
            {
                v1 += (x - xAvg) * (values[x] - yAvg);
                v2 += Math.Pow(x - xAvg, 2);
            }

            double a = v1 / v2;
            double b = yAvg - a * xAvg;

           // log.Debug("y = ax + b");
           // log.DebugFormat("a = {0}, the slope of the trend line.", Math.Round(a, 2));
           // log.DebugFormat("b = {0}, the intercept of the trend line.", Math.Round(b, 2));

            //Console.ReadLine();
        }
       


        
        public static bool getFilefromNet(string url,string saveto) {
            try
            {
                // this is for mono to a ssl server
                //ServicePointManager.CertificatePolicy = new NoCheckCertificatePolicy(); 

                ServicePointManager.ServerCertificateValidationCallback =
    new System.Net.Security.RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });

                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 10000;
                // Set the Method property of the request to POST.
                request.Method = "GET";
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
               // log.Info(((HttpWebResponse)response).StatusDescription);
                if (((HttpWebResponse)response).StatusCode != HttpStatusCode.OK)
                    return false;
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();

                long bytes = response.ContentLength;
                long contlen = bytes;

                byte[] buf1 = new byte[1024];

                FileStream fs = new FileStream(saveto + ".new", FileMode.Create);

                DateTime dt = DateTime.Now;

                while (dataStream.CanRead && bytes > 0)
                {
                    Application.DoEvents();
                    //log.Debug(saveto + " " + bytes);
                    int len = dataStream.Read(buf1, 0, buf1.Length);
                    bytes -= len;
                    fs.Write(buf1, 0, len);
                }

                fs.Close();
                dataStream.Close();
                response.Close();

                File.Delete(saveto);
                File.Move(saveto + ".new", saveto);

                return true;
            }
            catch (Exception ex) {  return false; }
        }
        
        public static Type getModes()
        {
            if (MainCrossData.cs.firmware == MainCrossData.Firmwares.ArduPlane)
            {
                return typeof(apmmodes);
            }
            else if (MainCrossData.cs.firmware == MainCrossData.Firmwares.ArduCopter2)
            {
                return typeof(ac2modes);
            }
            else if (MainCrossData.cs.firmware == MainCrossData.Firmwares.ArduRover)
            {
                return typeof(aprovermodes);
            }

            return null;
        }

        public static List<KeyValuePair<int, string>> getModesList(CurrentState cs)
        {
          //  log.Info("getModesList Called");

            // ensure we get the correct list




            return null;
        }

       

      


 
       
    }

    


}
