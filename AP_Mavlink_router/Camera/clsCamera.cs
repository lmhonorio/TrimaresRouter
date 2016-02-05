using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace AP_Mavlink_router.Camera
{
    public class clsCamera
    {
        SerialPort serial_camera;

        public bool camera_on = false;

        public clsCamera() 
        {
            serial_camera = new SerialPort() { BaudRate = 9600, Parity = Parity.Odd, StopBits = StopBits.Two, DataBits = 8 };


        }

        public bool writeByte(string code) 
        {
            try
            {
                serial_camera.Open();
                Thread.Sleep(100);
                if (serial_camera.IsOpen)
                    serial_camera.WriteLine(code);
                Thread.Sleep(100);
                serial_camera.Close();

                //througle the camera status
                if (code == "0")
                    camera_on = (camera_on? false : true);

                return true;
            }
            catch
            {
                if (serial_camera.IsOpen)
                    serial_camera.Close();

                return false;
            }
        }

    }
}
