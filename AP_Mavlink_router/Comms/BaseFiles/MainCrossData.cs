using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAVcomm
{
    public class MainCrossData
    {

        //const int SW_SHOWNORMAL = 1;
        //const int SW_HIDE = 0;

        /// <summary>
        /// Main Comport interface
        /// </summary>
        public MAVLink myLink = new MAVLink();
        /// <summary>
        /// Comport name
        /// </summary>
        public string comPortName = "";
        /// <summary>
        /// use to store all internal config
        /// </summary>
        public Hashtable config = new Hashtable();
        /// <summary>
        /// used to prevent comport access for exclusive use
        /// </summary>
        public static bool giveComport = false;
        /// <summary>
        /// mono detection
        /// </summary>
        public bool MONO = false;
        ///// <summary>
        ///// joystick static class
        ///// </summary>
        //public static Joystick joystick = null;
        /// <summary>
        /// track last joystick packet sent. used to track timming
        /// </summary>
       // DateTime lastjoystick = DateTime.Now;
        ///// <summary>
        ///// hud background image grabber from a video stream - not realy that efficent. ie no hardware overlays etc.
        ///// </summary>
        //public static WebCamService.Capture cam = null;
        /// <summary>
        /// the static global state of the currently connected MAV
        /// </summary>
        public static CurrentState cs = new CurrentState();
        /// <summary>
        /// controls the main serial reader thread
        /// </summary>
        //bool serialThread = false;
        /// <summary>
        /// track the last heartbeat sent
        /// </summary>
       // private DateTime heatbeatSend = DateTime.Now;

       /// <summary>
        /// used to feed in a network link kml to the http server
        /// </summary>
       // public string georefkml = "";

        /// <summary>
        /// store the time we first connect
        /// </summary>
        public DateTime connecttime = DateTime.Now;

        /// <summary>
        /// enum of firmwares
        /// </summary>
        public enum Firmwares
        {
            ArduPlane,
            ArduCopter2,
            ArduHeli,
            ArduRover,
            Ateryx
        }




        public void setMavlinkPointer(ref MAVLink newMav) 
        {
            newMav = myLink;
        }

   

    }
}
