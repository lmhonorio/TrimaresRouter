using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Xml;
using System.Collections;
using System.Net;
using System.Net.Sockets;

using System.Windows.Forms;
using MAVcomm.Utilities;
using MAVcomm.Arduino;
using MAVcomm.Controls;
using MAVcomm.Comms;
using MAVcomm;
//using log4net;

namespace MAVcomm.Arduino
{
    public class MavLinkConnection
    {

       // private static readonly ILog log =  LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       // private Form connectionStatsForm;
        private ConnectionStats _connectionStats;
        bool serialThread = false;
        public Thread runingthread;
        private DateTime heatbeatSend = DateTime.Now;
        public MainCrossData mydata;
        public delegate void pkgIn(byte[] New_packet);
        public event pkgIn PacketReceived;
     
       // private readonly ConnectionControl _connectionControl;



        public MavLinkConnection(string portName, ref MainCrossData mynewdata) 
        {
            mydata = mynewdata;
            mydata.comPortName = portName;
   

            if (portName == "TCP")
             {
                 mydata.myLink.DefaultConnection("TCP", 115200);
                 //MavLinkClient = new MAVLink();
                 //MavLinkClient.DefaultConnection("TCP", 115200);
                 //MavLinkClient.BaseStream.HostIP = tx_clientIP.Text;
                 //MavLinkClient.BaseStream.Port = tx_clientPort.Text;
                 //MavLinkClient.BaseStream.newMsg += Client_PacketReceived;
                 //MavLinkClient.BaseStream.Open();
                 //if (mydata.comPortName == "TCP")
                 //    mydata.myLink.BaseStream = new UdpSerial();
               // mydata.myLink.DefaultConnection("TCPSERVER", 115200); // BaseStream = new TCPCommServer();                 
                mydata.myLink.BaseStream.newMsg+=BaseStream_newMsg;
               // mydata.myLink.BaseStream.Open();
          
             }
            else if (portName == "UDP")
            {
                mydata.myLink.BaseStream = new MAVcomm.Comms.UdpSerial();
            //    mydata.myLink.DefaultConnection("UDP", 115200);
                mydata.myLink.BaseStream.newMsg += BaseStream_newMsg;
            }
            else if (portName == "UDPSERVER")
            {
                mydata.myLink.BaseStream = new MAVcomm.Comms.UdpSerialServer();
                mydata.myLink.BaseStream.BaudRate = 115200;
                mydata.myLink.BaseStream.newMsg += BaseStream_newMsg;
            }
            else
            {
                mydata.myLink.BaseStream = new MAVcomm.Comms.SerialPort();
            }

            try
            {
                mydata.myLink.BaseStream.PortName = portName;

                mydata.myLink.BaseStream.BaudRate = 115200;

                // check for saved baud rate and restore
                if (mydata.config[portName + "_BAUD"] != null)
                {
                    Console.WriteLine(mydata.config[portName + "_BAUD"].ToString());
                }
            }
            catch { }

            if (mydata.comPortName == "TCP" || mydata.comPortName == "TCPSERVER" || mydata.comPortName == "UDPSERVER")
            {
              //  mydata.myLink.BaseStream.newMsgReceived += BaseStream_newMsg;
                new Thread(sendOneHBperSecond)
                {
                    IsBackground = true,
                    Name = "one hb per sec",
                    Priority = ThreadPriority.BelowNormal,
                }.Start();


                return;
            }

            // setup main serial reader
            new Thread(SerialReader)
            {
                IsBackground = true,
                Name = "Main Serial reader",
                Priority = ThreadPriority.AboveNormal
            }.Start();


        }

        public MavLinkConnection() 
        {
            mydata = new MainCrossData();
        }


        public MavLinkConnection(string portName, ref MainCrossData mynewdata, string serverport)
        {
            mydata = mynewdata;
            mydata.comPortName = portName;


            switch (portName) 
            {
                case "TCPSERVER":
                    mydata.myLink.DefaultConnection(portName, 115200);
                    mydata.myLink.BaseStream.Port = serverport;
                    mydata.myLink.BaseStream.Open();
                    mydata.myLink.BaseStream.newMsg += BaseStream_newMsg;
                    break;
                case "UDP":
                    mydata.myLink.BaseStream = new MAVcomm.Comms.UdpSerial(serverport, 14550);
                    //    mydata.myLink.DefaultConnection("UDP", 115200);
                    mydata.myLink.BaseStream.newMsg += BaseStream_newMsg;
                    break;
                default:
                    //throw new Exception("PORTA NAO ABERTA - NAO E SERVIDOR TCP");
                    break;
            }



            //try
            //{
            //   // mydata.myLink.BaseStream.PortName = portName;

            //    //mydata.myLink.BaseStream.BaudRate = 115200;

            //    // check for saved baud rate and restore
            //    //if (mydata.config[portName + "_BAUD"] != null)
            //    //{
            //    //    Console.WriteLine(mydata.config[portName + "_BAUD"].ToString());
            //    //}
            //}
            //catch { }

            if (mydata.comPortName == "UDP" || mydata.comPortName == "TCP" || mydata.comPortName == "TCPSERVER")
            {
                //  mydata.myLink.BaseStream.newMsgReceived += BaseStream_newMsg;
                runingthread = new Thread(sendOneHBperSecond);
                runingthread.IsBackground = true;
                runingthread.Priority = ThreadPriority.AboveNormal;
                runingthread.Start();

                return;
            }

            // setup main serial reader
            runingthread = new Thread(SerialReader);
            runingthread.IsBackground = true;
            runingthread.Priority = ThreadPriority.AboveNormal;
            runingthread.Start();


        }

        public void startThreads() 
        {
            if (mydata.comPortName == "UDP" || mydata.comPortName == "TCP" || mydata.comPortName == "TCPSERVER")
            {
                //  mydata.myLink.BaseStream.newMsgReceived += BaseStream_newMsg;
                runingthread = new Thread(sendOneHBperSecond);
                runingthread.IsBackground = true;
                runingthread.Priority = ThreadPriority.AboveNormal;
                runingthread.Start();

                return;
            }

            // setup main serial reader
            runingthread = new Thread(SerialReader);
            runingthread.IsBackground = true;
            runingthread.Priority = ThreadPriority.AboveNormal;
            runingthread.Start();
        }
        

        void BaseStream_newMsg(byte[] bytes)
        {
            PacketReceived(bytes);
        }

        public static void Shootdown() 
        {
            Thread.CurrentPrincipal = null;
        }



        private void ResetConnectionStats()
        {
            _connectionStats = new ConnectionStats(mydata.myLink);
            //// If the form has been closed, or never shown before, we need do nothing, as 
            //// connection stats will be reset when shown
            //if (this.connectionStatsForm != null && connectionStatsForm.Visible)
            //{
            //    // else the form is already showing.  reset the stats
            //    this.connectionStatsForm.Controls.Clear();
            //    _connectionStats = new ConnectionStats(mydata.myLink);
            //    this.connectionStatsForm.Controls.Add(_connectionStats);
            //}
        }

        private void clearLogs() 
        {
            // cleanup from any previous sessions
            if (mydata.myLink.logfile != null)
                mydata.myLink.logfile.Close();

            if (mydata.myLink.rawlogfile != null)
                mydata.myLink.rawlogfile.Close();

            mydata.myLink.logfile = null;
            mydata.myLink.rawlogfile = null;

            //cleanup any log being played
            mydata.myLink.logreadmode = false;
            if (mydata.myLink.logplaybackfile != null)
                mydata.myLink.logplaybackfile.Close();
            mydata.myLink.logplaybackfile = null;
        }



        public void connect() 
        {
            MainCrossData.giveComport = false;

            if (mydata.myLink.BaseStream.IsOpen && MainCrossData.cs.groundspeed > 4)
            {
                if (DialogResult.No == MessageBox.Show("Robo em movimento, deseja mesmo desconectar?", "Desconectar", MessageBoxButtons.YesNo))
                {
                    return;
                }
            }


            //clear any log beeing played/opened
            clearLogs();

            // decide if this is a connect or disconnect
            if (mydata.myLink.BaseStream.IsOpen)
            {
                try
                {
                    mydata.myLink.BaseStream.DtrEnable = false;
                    mydata.myLink.Close();
                }
                catch (Exception ex)
                {
                    //log.Error(ex);
                }

               
            }
            else
            {

               
                try
                {
                    // set port, then options
                   // MainCrossData.comPort.BaseStream.PortName = _connectionControl.CMB_serialport.Text;

                    mydata.myLink.DefaultConnection(mydata.comPortName, 115200);

                    // Here we want to reset the connection stats counter etc.
                    this.ResetConnectionStats();

                    // prevent serialreader from doing anything
                    MainCrossData.giveComport = true;

                    // reset on connect logic.
                    if (mydata.config["CHK_resetapmonconnect"] == null || bool.Parse(mydata.config["CHK_resetapmonconnect"].ToString()) == true)
                        mydata.myLink.BaseStream.toggleDTR();

                    MainCrossData.giveComport = false;

                    // setup to record new logs
                    try
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + @"logs");
                    //    mydata.myLink.logfile = new BinaryWriter(File.Open(Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + @"logs" + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".tlog", FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None));
                    //    mydata.myLink.rawlogfile = new BinaryWriter(File.Open(Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + @"logs" + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".rlog", FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None));
                    }
                    catch (Exception exp2) { MessageBox.Show("Failed to create log - wont log this session"); } // soft fail

                    // reset connect time - for timeout functions
                    mydata.connecttime = DateTime.Now;

                   
                    // do the connect - false = do not get parameters
                    //mydata.myLink.Open(true);
                    mydata.myLink.Open(false);

                }
                catch (Exception ex)
                {
                    //log.Warn(ex);
                   
                    MessageBox.Show("Can not establish a connection\n\n" + ex.Message);
                    return;
                }
            }




        }

        bool sendHBThread = false;

        private void sendOneHBperSecond()
        {
            if (sendHBThread == true)
                return;
            sendHBThread = true;

            MAVLink.mavlink_heartbeat_t htb = new MAVLink.mavlink_heartbeat_t()
            {
                type = (byte)MAVLink.MAV_TYPE.QUADROTOR,
                autopilot = (byte)MAVLink.MAV_AUTOPILOT.ARDUPILOTMEGA,
                mavlink_version = 3
            };
           

            byte[] pabyte = new byte[] { 254, 9, 7, 1, 1, 0, 0, 0, 0, 0, 2, 3, 81, 4, 3, 250, 173 };

            //mydata.myLink.ReturnByteArray(

           // myServer.Write(pabyte, 0, pabyte.Length);

            mydata.myLink.sendPacket(htb);

            while (sendHBThread)
            {
                try
                {
                    Thread.Sleep(950);

                    if (mydata.myLink.BaseStream.IsOpen)
                    {
                        mydata.myLink.sendPacket(htb);
                       // mydata.myLink.BaseStream.Write(pabyte, 0, pabyte.Length);
                    }


                }
                catch (Exception e)
                {
                    //log.Error("HB sending fail :" + e.ToString());
                    try
                    {
                        mydata.myLink.Close();
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// main serial reader thread
        /// controls
        /// serial reading
        /// link quality stats
        /// speech voltage - custom - alt warning - data lost
        /// heartbeat packet sending
        /// 
        /// and can't fall out
        /// </summary>
        private void SerialReader()
        {
            if (serialThread == true)
                return;
            serialThread = true;


            int minbytes = 0;

            DateTime speechcustomtime = DateTime.Now;

            DateTime linkqualitytime = DateTime.Now;

            while (serialThread)
            {
                try
                {
                    Thread.Sleep(5);
                  
                    // if not connected or busy, sleep and loop
                    if (mydata.myLink.BaseStream!=null && !mydata.myLink.BaseStream.IsOpen || MainCrossData.giveComport == true)
                    {
                        
                        System.Threading.Thread.Sleep(100);
                        continue;
                    }


                    // make sure we attenuate the link quality if we dont see any valid packets
                    if ((DateTime.Now - mydata.myLink.lastvalidpacket).TotalSeconds > 10)
                    {
                        MainCrossData.cs.linkqualitygcs = 0;
                    }

                    // attenuate the link qualty over time
                    if ((DateTime.Now - mydata.myLink.lastvalidpacket).TotalSeconds >= 1)
                    {
                        if (linkqualitytime.Second != DateTime.Now.Second)
                        {
                            MainCrossData.cs.linkqualitygcs = (ushort)(MainCrossData.cs.linkqualitygcs * 0.8f);
                            linkqualitytime = DateTime.Now;
                        }
                    }

                    //// send a hb every seconds from gcs to ap
                    if (heatbeatSend.Second != DateTime.Now.Second)
                    {
                        
                        MAVLink.mavlink_heartbeat_t htb = new MAVLink.mavlink_heartbeat_t()
                        {
                            type = (byte)MAVLink.MAV_TYPE.GCS,
                            autopilot = (byte)MAVLink.MAV_AUTOPILOT.ARDUPILOTMEGA,
                            mavlink_version = 3
                        };

                        mydata.myLink.sendPacket(htb);

                         
  

                        heatbeatSend = DateTime.Now;

                        byte[] data = MavlinkUtil.StructureToByteArray(htb);
                        PacketReceived(data);
                    }


                    // actauly read the packets
                    while (mydata.myLink.BaseStream.BytesToRead > minbytes && MainCrossData.giveComport == false)
                    {
                        try
                        {
                            byte[] packetIn = mydata.myLink.readPacket();
                            PacketReceived(packetIn);
                        }
                        catch (Exception e) {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
                catch (Exception e)
                {
                    //log.Error("Serial Reader fail :" + e.ToString());
                    try
                    {
                        mydata.myLink.Close();
                    }
                    catch { }
                }
            }
        }

    }
}
