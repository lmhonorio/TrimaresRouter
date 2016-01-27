using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Threading;

namespace MAVcomm.Comms
{
    public class SerialPort : System.IO.Ports.SerialPort,ICommsSerial
    {
        public new void Open()
        {
            // 500ms write timeout - win32 api default
            this.WriteTimeout = 500;

            if (base.IsOpen)
                return;

            base.Open();
        }

        public void toggleDTR()
        {
            bool open = this.IsOpen;

            if (!open)
                this.Open();

            base.DtrEnable = false;
            base.RtsEnable = false;

            System.Threading.Thread.Sleep(50);

            base.DtrEnable = true;
            base.RtsEnable = true;

            System.Threading.Thread.Sleep(50);

            if (!open)
                this.Close();
        }

        public new static string[] GetPortNames()
        {
            List<string> allPorts = new List<string>();

            if (Directory.Exists("/dev/"))
            {
                if (Directory.Exists("/dev/serial/by-id/"))
                    allPorts.AddRange(Directory.GetFiles("/dev/serial/by-id/", "*"));
                allPorts.AddRange(Directory.GetFiles("/dev/", "ttyACM*"));
                allPorts.AddRange(Directory.GetFiles("/dev/", "ttyUSB*"));
                allPorts.AddRange(Directory.GetFiles("/dev/", "rfcomm*"));
            }

            string[] ports = System.IO.Ports.SerialPort.GetPortNames()
            .Select(p => p.TrimEnd())
            .Select(FixBlueToothPortNameBug)
            .ToArray();

            allPorts.AddRange(ports);

            return allPorts.ToArray();
        }


        // .NET bug: sometimes bluetooth ports are enumerated with bogus characters 
        // eg 'COM10' becomes 'COM10c' - one workaround is to remove the non numeric  
        // char. Annoyingly, sometimes a numeric char is added, which means this 
        // does not work in all cases. 
        // See http://connect.microsoft.com/VisualStudio/feedback/details/236183/system-io-ports-serialport-getportnames-error-with-bluetooth 
        private static string FixBlueToothPortNameBug(string portName)
        {
            if (!portName.StartsWith("COM"))
                return portName;
            var newPortName = "COM";                                // Start over with "COM" 
            foreach (var portChar in portName.Substring(3).ToCharArray())  //  Remove "COM", put the rest in a character array 
            {
                if (char.IsDigit(portChar))
                    newPortName += portChar.ToString(); // Good character, append to portName 
                //  else
                //log.WarnFormat("Bad (Non Numeric) character in port name '{0}' - removing", portName);
            }

            return newPortName;
        }



        public event MsgReceived newMsg;

        public System.Net.Sockets.Socket isocket
        {
            get { throw new NotImplementedException(); }
        }

        public System.Net.Sockets.TcpListener ilistener
        {
            get { throw new NotImplementedException(); }
        }

        public System.Net.IPEndPoint iep
        {
            get { throw new NotImplementedException(); }
        }

        public System.Net.Sockets.TcpClient iclient
        {
            get { throw new NotImplementedException(); }
        }


        public Stream BaseFileStream
        {
            get { throw new NotImplementedException(); }
        }


        public string HostIP
        {
            get;
            set;
        }


        public string Port
        {
            get;           
            set; 
        }


        public event ReturnMessage rMessage;


        void sendingHB()
        {
            while (true)
            {
                if (this.IsOpen)
                {
                    byte[] pabyte = new byte[] { 254, 9, 7, 1, 1, 0, 0, 0, 0, 0, 2, 3, 81, 4, 3, 250, 173 };
                    this.Write(pabyte, 0, pabyte.Length);
                }
                Thread.Sleep(950);
            }
        }

        Thread shb;
        public void StartSendingHB()
        {
            shb = new Thread(sendingHB);
            shb.Start();

        }
    }
}
