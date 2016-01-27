#define newserverx

using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Diagnostics;
using MAVcomm.Arduino;
using MAVcomm.Comms;
using MAVcomm;
using MAVcomm.Utilities;
using System.Net.Sockets;
using System.Net;

using clsExodn;
using Trimares;

namespace AP_Mavlink_router
{

    public partial class MsgRouter : Form
    {

        /// <summary>
        /// comunicacao com um cliente
        /// </summary>
        MavLinkConnection TCPClientConn;
        MainCrossData TCPClientData;
        MAVLink TCPMavLinkClient;



        /// <summary>
        /// comunicaçao com o mission planner
        /// </summary>
        /// 
        TCPCommServer ExoMonitorServer;

        TCPCommServer MissionPlannerServer;

        MAVLink mavComm = new MAVLink();

        /// <summary>
        /// comunicacao com trimares
        /// </summary>
        /// 
        UdpSerialServer TrimaresServer;



        MavLinkConnection TrimaresUDPServerConn;
        MainCrossData TrimaresUDPServerData;
        MAVLink TrimaresUDPMavLinkServer;

        Sonar.sonarcomm swatch;




        private object semaforo = new object();


        Thread tstartMissonPlannerConn;
        Thread tstartTrimaresConn;
        Thread tstartmyServerConn;



        Thread tstartExoReadings;
        Thread Treadings;
        


        ExoData ExoReadings;
        exo_waterquality current,lastexo;
        //List<exo_waterquality> Readings;
        BindingList<exo_waterquality> bindinglist;
        BindingSource source;




        public MsgRouter()
        {
            try
            {
                InitializeComponent();
                ListBox.CheckForIllegalCrossThreadCalls = false;
                PropertyGrid.CheckForIllegalCrossThreadCalls = false;
                listBox1.Items.Clear();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }


            try
            {

              //  DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
              //  DevExpress.XtraGrid.GridControl.CheckForIllegalCrossThreadCalls = false;


                TCPClientData = new MainCrossData();
                TCPClientData.myLink.BaseStream.BaudRate = 115200;


                string[] portnames = MAVcomm.Comms.SerialPort.GetPortNames();


                if (portnames.Length == 0)
                {
                    listBox1.Items.Add("ardupilot not found, going on as fake robot...");
                    Thread.Sleep(1000);
                    //   Thread.CurrentThread.Abort();
                }
                else
                {
                    gb_simulador.Enabled = true;
                   // gb_tcpclient.Enabled = false;
                   //' gb_realtrimares.Enabled = false;
                    tx_APcomport.Text = portnames[0];
                   // cbCType.Text = cbCType.Items[0].ToString();
                    //  ClientData.comPortName = cbCType.Text;
                    //  ClientData.myLink.BaseStream.PortName = ClientData.comPortName;
                }




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }



        }

        #region CONECCOES

        private void bt_ExoMonitor_Click(object sender, EventArgs e)
        {
            new Thread(startExoMonitorConn).Start();
        }
        
        /// <summary>
        /// inicializa comunicacao com mavlink
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_server_Click(object sender, EventArgs e)
        {
            new Thread(startMissionPlannerConn).Start();          
        }


        /// <summary>
        /// inicializa comunicacao com trimares
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_trimares_Click(object sender, EventArgs e)
        {
            tstartTrimaresConn = new System.Threading.Thread(startTrimaresConn);
            tstartTrimaresConn.Start();
        }


        /// <summary>
        /// inicializa comunicacao com swatch plus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_udp_Click(object sender, EventArgs e)
        {
            tstartmyServerConn = new System.Threading.Thread(startmyServerConn);
            tstartmyServerConn.Start();
        }
      


        private void bt_exotest_Click(object sender, EventArgs e)
        {

            try
            {
                if (ExoReadings == null)
                {
                    tstartExoReadings = new System.Threading.Thread(startExoReadings);
                    tstartExoReadings.Start();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void bt_client_Click(object sender, EventArgs e)
        {
            try
            {
                TCPClientData = new MainCrossData();
                TCPClientConn = new MavLinkConnection("TCP", ref TCPClientData);
                TCPClientConn.mydata.setMavlinkPointer(ref TCPMavLinkClient);
                TCPClientConn.mydata.myLink.BaseStream.HostIP = tx_clientIP.Text;
                TCPClientConn.mydata.myLink.BaseStream.Port = tx_clientPort.Text;
                TCPClientConn.PacketReceived += APSerialConn_PacketReceived;
                TCPClientConn.mydata.myLink.BaseStream.Open();
                TCPClientConn.mydata.myLink.BaseStream.rMessage += showMessages;

                //MavLinkClient = new MAVLink();
                //MavLinkClient.DefaultConnection("TCP", 115200);
                //MavLinkClient.BaseStream.HostIP = tx_clientIP.Text;
                //MavLinkClient.BaseStream.Port = tx_clientPort.Text;
                //MavLinkClient.BaseStream.newMsg += Client_PacketReceived;
                //MavLinkClient.BaseStream.Open();

                gb_robot.Enabled = false;
            }
            catch { }


        }

        private void bt_connArdupilot_Click(object sender, EventArgs e)
        {

                tstartTrimaresConn = new System.Threading.Thread(startAPMsimulation);
                tstartTrimaresConn.Start();
              
               // bg_missionplanner.Enabled = false;
 
        }





        void startExoReadings() 
        {
            try
            {
                //exogridControl.Enabled = false;
                ExoReadings = new ExoData();

                //exoGridView.Enabled = false;

          
                bindinglist = new BindingList<exo_waterquality>(ExoReadings.Exodata);
                source = new BindingSource(bindinglist, null);

                this.Invoke((MethodInvoker)delegate 
                {
                    exoGridView.DataSource = source;
                    //exogridControl.DataSource = source;
                });
               // exogridControl.DataSource = source;

                // exoGridView.DataSource = source;

                ExoReadings.exoRead += ExoReadings_exoRead;
                ExoReadings.dataread += ExoReadings_dataread;
                ExoReadings.starReadings();
            }
            catch { }
        }


        /// <summary>
        /// Inicializa comunicaçao do router com o ExoMonitor
        /// </summary>
        private void startExoMonitorConn()
        {
            int port;
            try
            {
                try
                {
                    port = int.Parse(tx_ExoPort.Text);
                }
                catch 
                {
                    MessageBox.Show("Erro na definição de porta");
                    return;
                }
                ExoMonitorServer = new TCPCommServer(port);
                ExoMonitorServer.Open();
                ExoMonitorServer.rMessage += showMessages;
                ExoMonitorServer.newMsg += TCPServer_packetReceived;
                ExoMonitorServer.StartSendingHB();


            }
            catch
            {
                ExoMonitorServer.Close();
                ExoMonitorServer = null;
            }
        }

        /// <summary>
        /// Inicializa comunicaçao do router com o mission planner
        /// </summary>
        private void startMissionPlannerConn() 
        {
            try
            {

                MissionPlannerServer = new TCPCommServer();
                MissionPlannerServer.Open();
                MissionPlannerServer.rMessage += showMessages;
                MissionPlannerServer.newMsg += TCPServer_packetReceived;
                MissionPlannerServer.StartSendingHB();

               
            }
            catch
            {
                MissionPlannerServer.Close();
                MissionPlannerServer = null;
            }
        }


        void startAPMsimulation()
        {
            try
            {
                TrimaresUDPServerData = new MainCrossData();
                TrimaresUDPServerConn = new MavLinkConnection(tx_APcomport.Text, ref TrimaresUDPServerData);
                // TrimaresUDPServerConn.mydata.setMavlinkPointer(ref TrimaresUDPMavLinkServer);
                TrimaresUDPServerConn.connect();
                TrimaresUDPServerConn.PacketReceived += TrimaresUDP_PacketReceived;
               // TrimaresUDPServerConn.mydata.myLink.BaseStream.rMessage += showMessages;
                TrimaresUDPServerConn.mydata.myLink.requestDatastream(MAVLink.MAV_DATA_STREAM.ALL, MainCrossData.cs.rateattitude);
            }
            catch
            {
                TrimaresUDPMavLinkServer = null;
                TrimaresUDPServerData = null;
                TrimaresUDPServerConn = null;

            }

            //TCPClientData = new MainCrossData();
            //TCPClientConn = new MavLinkConnection(tx_APcomport.Text, ref TCPClientData);
            //TCPClientConn.mydata.setMavlinkPointer(ref TCPMavLinkClient);
            ////TCPClientConn.mydata.myLink.BaseStream.Open();

            //TCPClientConn.connect();

            //TCPClientConn.mydata.myLink.BaseStream.rMessage += showMessages;
            //TCPClientConn.PacketReceived += APSerialConn_PacketReceived; 
            //TCPClientData.myLink.requestDatastream(MAVLink.MAV_DATA_STREAM.ALL, MainCrossData.cs.rateattitude);



            gb_robot.Enabled = false;
        }


        /// <summary>
        /// inicializa comunicacao do router com o trimares
        /// </summary>
        private void startTrimaresConn()
        {
            try
            {
                TrimaresServer = new MAVcomm.Comms.UdpSerialServer();
                TrimaresServer.HostIP = tx_udpIP.Text;
                TrimaresServer.Port = tx_udpport.Text;
                TrimaresServer.newMsg += TrimaresUDP_PacketReceived;
                TrimaresServer.Open();
                TrimaresServer.rMessage += showMessages;
                //TrimaresUDPServerData = new MainCrossData();
                //TrimaresUDPServerConn = new MavLinkConnection("UDPSERVER", ref TrimaresUDPServerData);
                //TrimaresUDPServerConn.mydata.setMavlinkPointer(ref TrimaresUDPMavLinkServer);
                //TrimaresUDPServerConn.mydata.myLink.BaseStream.HostIP = tx_udpIP.Text;
                //TrimaresUDPServerConn.mydata.myLink.BaseStream.Port = tx_udpport.Text;
                //TrimaresUDPServerConn.PacketReceived += TrimaresUDP_PacketReceived;
                //TrimaresUDPServerConn.mydata.myLink.BaseStream.Open();
                //TrimaresUDPServerConn.mydata.myLink.BaseStream.rMessage += showMessages;
            }
            catch 
            {
                TrimaresServer.Close();
                TrimaresServer = null;

                TrimaresUDPMavLinkServer = null;
                TrimaresUDPServerData = null;
                TrimaresUDPServerConn = null;

            }
        }

        /// <summary>
        /// inicializa comunicacao do router com o swatch plus
        /// </summary>
        private void startmyServerConn()
        {
            //MissionTCPServerData = new MainCrossData();
            //MissionTCPServerConn = new MavLinkConnection("UDP", ref MissionTCPServerData, tx_udpIP.Text);
            //MissionTCPServerConn.mydata.setMavlinkPointer(ref MissionTCPMavLinkServer);
            //MissionTCPServerConn.mydata.myLink.BaseStream.HostIP = tx_udpIP.Text;
            //MissionTCPServerConn.mydata.myLink.BaseStream.Port = tx_udpport.Text;
            //MissionTCPServerConn.PacketReceived += Server_PacketReceived;
            //MissionTCPServerConn.mydata.myLink.BaseStream.Open();
            //MissionTCPServerConn.mydata.myLink.BaseStream.rMessage += showMessages;
        }




        #endregion




        #       region Bridge

        List<int> pks = new List<int> { 0, 1, 3, 24, 27, 29, 30, 34, 35, 36, 42, 62, 74, 79, 150, 152, 163, 165 };


        void APSerialConn_PacketReceived(byte[] bytes) 
        {
            if (bytes.Length > 5)
            {

                if (bytes[5] == MAVLink.MAVLINK_MSG_ID_ATTITUDE)
                {
                    MAVLink.mavlink_attitude_t attitude = bytes.ByteArrayToStructure<MAVLink.mavlink_attitude_t>(6);
                    this.Invoke((MethodInvoker)delegate()
                    {
                        listBox1.Items.Add(bytes[5].ToString());
                    });

                }

                if (bytes[5] == MAVLink.MAVLINK_MSG_ID_EXO_WATERQUALITY)
                {
                    MAVLink.mavlink_exo_waterquality_t exo0 = bytes.ByteArrayToStructure<MAVLink.mavlink_exo_waterquality_t>(6);
                    this.Invoke((MethodInvoker)delegate()
                    {
                        listBox1.Items.Add(bytes[5].ToString());
                        listBox1.Items.Add(exo0.time_usec.ToString());
                    });

                }
                if (bytes[5] == MAVLink.MAVLINK_MSG_ID_HEARTBEAT)
                {
                    MAVLink.mavlink_heartbeat_t hb = bytes.ByteArrayToStructure<MAVLink.mavlink_heartbeat_t>(6);
                    TCPClientData.myLink.requestDatastream(MAVLink.MAV_DATA_STREAM.ALL, MainCrossData.cs.rateattitude);
                  
                    this.Invoke((MethodInvoker)delegate()
                    {
                        listBox1.Items.Add(hb.mavlink_version.ToString());
                    });

                }
            }
        }




        void TCPServer_packetReceived(byte[] bytes) 
        {
            if (TrimaresUDPServerData != null && TrimaresUDPServerData.myLink.BaseStream.IsOpen) 
            {
                TrimaresUDPServerData.myLink.BaseStream.Write(bytes, 0, bytes.Length);
            }
        }

        int lat, lon;
        object locker = new object();
        void TrimaresUDP_PacketReceived(byte[] bytes)
        {
            if (bytes[5] == MAVLink.MAVLINK_MSG_ID_GPS_RAW_INT)
            {
                MAVLink.mavlink_gps_raw_int_t gps = bytes.ByteArrayToStructure<MAVLink.mavlink_gps_raw_int_t>(6);
                lock (locker)
                {
                    lat = gps.lat;
                    lon = gps.lon;
                }
                //  mavlink_mission_request_list_t pacote = newPacket.ByteArrayToStructure<mavlink_mission_request_list_t>(6);
            }

            if (bytes[5] == MAVLink.MAVLINK_MSG_ID_ATTITUDE)
            {
                MAVLink.mavlink_attitude_t att = bytes.ByteArrayToStructure<MAVLink.mavlink_attitude_t>(6);
                int i = 1;
                //  mavlink_mission_request_list_t pacote = newPacket.ByteArrayToStructure<mavlink_mission_request_list_t>(6);
            }



            try
            {

                if (bytes.Length > 5)
                {

                    if (MissionPlannerServer != null && MissionPlannerServer.IsOpen) 
                    {
                        MissionPlannerServer.Write(bytes, 0, bytes.Length);

                    }


                    ////  if (myServer != null && myServer.IsOpen)
                    //if (MissionTCPMavLinkServer != null && MissionTCPMavLinkServer.BaseStream.IsOpen)
                    //{

                    //    //var newmsg = from n in pks
                    //    //             where n == Convert.ToInt16(bytes[5])
                    //    //             select n;



                    //    //MissionTCPMavLinkServer.HandlePacket(bytes, ref MavLinkClient);

                    //    MissionTCPMavLinkServer.BaseStream.Write(bytes, 0, bytes.Length);


                    //}

                    // Console.WriteLine(bytes[5]);
                    if (swatch != null)
                    {
                        if (bytes[5] == MAVLink.MAVLINK_MSG_ID_ATTITUDE)
                        {
                            MAVLink.mavlink_attitude_t attitude = bytes.ByteArrayToStructure<MAVLink.mavlink_attitude_t>(6);
                            swatch.setAttitude(attitude.roll * 180.0 / Math.PI, attitude.pitch * 180.0 / Math.PI, attitude.yaw * 180.0 / Math.PI);

                            //  mavlink_mission_request_list_t pacote = newPacket.ByteArrayToStructure<mavlink_mission_request_list_t>(6);
                        }
                        if (bytes[5] == MAVLink.MAVLINK_MSG_ID_GPS_RAW_INT)
                        {
                            MAVLink.mavlink_gps_raw_int_t gps = bytes.ByteArrayToStructure<MAVLink.mavlink_gps_raw_int_t>(6);
                            swatch.setPosition(gps.lat * 1.0e-7f, gps.lon * 1.0e-7f);

                            //  mavlink_mission_request_list_t pacote = newPacket.ByteArrayToStructure<mavlink_mission_request_list_t>(6);
                        }

                        swatch.sendData_POSMV102();
                    }




                }
            }
            catch { }



        }


        void MissionPlannerTCP_PacketReceived(byte[] bytes)
        {
       
            try
            {
                if (bytes.Length > 5)
                {


                    if (TrimaresServer != null && TrimaresServer.IsOpen) 
                    {
                        TrimaresServer.Write(bytes, 0, bytes.Length);
                    }

                    if (TrimaresUDPMavLinkServer != null && TrimaresUDPMavLinkServer.BaseStream.IsOpen)
                    {
                        //var newmsg = from n in pks
                        //                where n == Convert.ToInt16(bytes[5])
                        //                select n;


                        //UDPMavLinkServer.HandlePacket(bytes, ref MavLinkServer);
                        TrimaresUDPMavLinkServer.BaseStream.Write(bytes, 0, bytes.Length);

                        //this.Invoke((MethodInvoker)delegate()
                        //{
                        //    listBox1.Items.Add(bytes[5].ToString());
                        //});
                    }



                    if (TCPMavLinkClient != null && TCPMavLinkClient.BaseStream.IsOpen)
                    {
                        var newmsg = from n in pks
                                     where n == Convert.ToInt16(bytes[5])
                                     select n;

                        this.Invoke((MethodInvoker)delegate()
                        {
                            listBox1.Items.Add(bytes[5].ToString());
                        });

                       // MavLinkClient.HandlePacket(bytes, ref MavLinkServer);
                        TCPMavLinkClient.BaseStream.Write(bytes, 0, bytes.Length);
                    }


                }
                //else if(myClient != null && myClient.IsOpen)
                //{
                //    myClient.Write(bytes, 0, bytes.Length);
                //}

            }
            catch { }

                


        }



        #endregion




        private void showMessages(string messages) 
        {
            this.Invoke((MethodInvoker)delegate()
            {
                listBox1.Items.Add(messages);
            });
        }


        
        private void bt_swatch_Click(object sender, EventArgs e)
        {
            try
            {
                swatch = new Sonar.sonarcomm();
                ProcessStartInfo start = new ProcessStartInfo();
                // Enter in the command line arguments, everything you would enter after the executable name itself
                // Enter the executable to run, including the complete path
                start.FileName = @".\trimares\trimares.sxs";
                Process.Start(start);
            }
            catch { }
            //trimares.sxs

        }



        void ExoReadings_dataread(string x)
        {

        }

        void ExoReadings_exoRead(exo_waterquality exor)
        {

            try
            {
                current = exor;

                if (current.id != 0 && current.id != lastexo.id)
                {

                    lastexo = current;
                    Treadings = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(SendingReadings));
                    Treadings.Priority = System.Threading.ThreadPriority.Highest;
                    Treadings.Start(lastexo);

                }


                this.Invoke((MethodInvoker)delegate()
                  {

                      source.ResetBindings(true);

                      tx_utime.Text = exor.time_usec.ToString();
                      tx_lat.Text = exor.lat.ToString();
                      tx_lon.Text = exor.lon.ToString();
                      tx_id.Text = exor.id.ToString();
                      tx_chloride.Text = exor.chloride.ToString();
                      tx_condctivity.Text = exor.conductivity.ToString();
                      tx_conductance.Text = exor.conductance.ToString();
                      tx_depth.Text = exor.depth.ToString();
                      tx_salinity.Text = exor.salinity.ToString();
                      tx_temperature.Text = exor.temperature.ToString();
                      tx_turbidity.Text = exor.turbidity.ToString();
                      tx_nlf.Text = exor.nLF.ToString();
                      tx_ph.Text = exor.ph.ToString();
                      tx_tds.Text = exor.tot_dissolved_solids.ToString();
                      tx_do.Text = exor.dissolved_oxygen.ToString();
                  });
            }
            catch { }

        }




        void SendingReadings(object objexor)
        {

            try
            {
                exo_waterquality exor = (exo_waterquality)objexor;

                MAVLink.mavlink_exo_waterquality_t mavlinkexo = (MAVLink.mavlink_exo_waterquality_t)exor;

                if (MissionPlannerServer != null && MissionPlannerServer.IsOpen) 
                {
                    byte[] pck = mavComm.ReturnByteArray(mavlinkexo);
                    MissionPlannerServer.Write(pck, 0, pck.Length);


                }

                if (ExoMonitorServer != null && ExoMonitorServer.IsOpen) 
                {
                    byte[] pck = mavComm.ReturnByteArray(mavlinkexo);
                    ExoMonitorServer.Write(pck, 0, pck.Length);
                }


                if (TCPClientConn != null)
                {
                    TCPClientConn.mydata.myLink.sendPacket(mavlinkexo);
                }
            }
            catch { }
      
        }





        private void CloseStream(ICommsSerial basestream)
        {
            try
            {
                if (basestream.IsOpen)
                    basestream.Close();
            }
            catch { }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            try
            {

                if (tstartMissonPlannerConn != null)
                {
                    tstartMissonPlannerConn.Abort();
                    Thread.Sleep(100);
                    tstartMissonPlannerConn = null;
                }

                closeThread(tstartMissonPlannerConn);
                closeThread(tstartmyServerConn);
                closeThread(tstartTrimaresConn);
                closeThread(tstartExoReadings);
                closeThread(Treadings);

                UDPComm.oktoReceive = false;

                if (MissionPlannerServer != null) 
                {
                    MissionPlannerServer.Close();
                    MissionPlannerServer = null;
                }

                if (TrimaresServer != null)
                {
                    TrimaresServer.Close();
                    TrimaresServer = null;
                }

                



                if (TCPClientConn != null)
                {
                    CloseStream(TCPClientConn.mydata.myLink.BaseStream);
                    TCPClientConn = null;
                }

                if (TCPMavLinkClient != null)
                {
                    TCPMavLinkClient = null;
                }

                if (TCPClientData != null)
                {
                    TCPClientData = null;
                }



                if (TrimaresUDPServerConn != null)
                {
                    CloseStream(TrimaresUDPServerConn.mydata.myLink.BaseStream);
                    TrimaresUDPServerConn = null;
                }

                if (TrimaresUDPServerData != null)
                    TrimaresUDPServerData = null;

                if (TrimaresUDPMavLinkServer != null)
                    TrimaresUDPMavLinkServer = null;

                foreach (Process p in System.Diagnostics.Process.GetProcessesByName("AP_Mavlink_router.exe"))
                {
                    p.Kill();
                    p.WaitForExit();
                }
            }
            catch 
            {
                Application.ExitThread();
                Application.Exit();
            }

            Application.ExitThread();
            Application.Exit();
         

        }

        private void bt_closeall_Click(object sender, EventArgs e)
        {

            try
            {
                UDPComm.oktoReceive = false;

                closeThread(tstartMissonPlannerConn);
                closeThread(tstartmyServerConn);
                closeThread(tstartTrimaresConn);
                closeThread(tstartExoReadings);
                closeThread(Treadings);


                if (TCPClientConn != null)
                    TCPClientConn = null;

                if (TCPClientData != null)
                    TCPClientData = null;


                if (TCPMavLinkClient != null)
                    TCPMavLinkClient = null;

                            

                if (TrimaresUDPServerConn != null)
                    TrimaresUDPServerConn = null;

                if (TrimaresUDPServerData != null)
                    TrimaresUDPServerData = null;

                if (TrimaresUDPMavLinkServer != null)
                    TrimaresUDPMavLinkServer = null;
            }
            catch { }


            
            //Application.ExitThread();
            //Application.Exit(); 
        }

        void closeThread(Thread t) 
        {
            try
            {
                if (t != null)
                {
                    t.Abort();
                    Thread.Sleep(100);
                    t = null;
                }
            }
            catch { }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            lock (semaforo)
            {
                try
                {
                    //exogridControl.Enabled = true;
                    //exoGridView.Enabled = true;
                    ExoReadings.stopReadings();
                    current.id = 0;
                    lastexo.id = 0;
                }
                catch { }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            lock (semaforo)
            {
                try
                {
                    ExoReadings.ClearBuffer();
                    ExoReadings.Exodata.Clear();
                    //exoGridView.DataSource = null;
                }
                catch { }

            }
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MissionPlannerServer != null && MissionPlannerServer.IsOpen)
            {
                byte[] pabyte = new byte[] { 254, 9, 7, 1, 1, 0, 0, 0, 0, 0, 2, 3, 81, 4, 3, 250, 173 };
                MissionPlannerServer.Write(pabyte, 0, pabyte.Length);
                //GroundStationComm.mydata.myLink.sendPacket(htb);
            }

            if (TrimaresUDPServerData != null && TrimaresUDPServerData.myLink.BaseStream.IsOpen)
            {
                byte[] pabyte = new byte[] { 254, 9, 7, 1, 1, 0, 0, 0, 0, 0, 2, 3, 81, 4, 3, 250, 173 };
                TrimaresUDPServerData.myLink.BaseStream.Write(pabyte, 0, pabyte.Length);
            }

           
        }

        private void bt_sendExoTest_Click(object sender, EventArgs e)
        {

            timer1.Start();


        }


        Random rnd = new Random();
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            exo_waterquality exor = new exo_waterquality();
            exor.time_usec = Convert.ToUInt64(exo_waterquality.returnID());           
            exor.chloride = 9.65f + (float)(rnd.NextDouble() * 10f);
            exor.depth = 10f  + (float)(rnd.NextDouble() * 8f);
            exor.temperature = 29.65f + (float)(rnd.NextDouble() * 5f) - 0.5f*exor.depth;

            MAVLink.mavlink_exo_waterquality_t mavlinkexo = (MAVLink.mavlink_exo_waterquality_t)exor;

            lock (locker) 
            {
                mavlinkexo.lat = lat;
                mavlinkexo.lon = lon;
            }

            //if (MissionTCPServerConn != null && MissionTCPServerConn.mydata.myLink.BaseStream.IsOpen) 
            //{
            //    MissionTCPServerConn.mydata.myLink.sendPacket(mavlinkexo);
            //}

            if (MissionPlannerServer != null && MissionPlannerServer.IsOpen)
            {
                //MissionPlannerServer.  sendPacket(mavlinkexo);
                 byte[] pck = mavComm.ReturnByteArray(mavlinkexo);
                 MissionPlannerServer.Write(pck, 0, pck.Length);
                //MissionTCPServerConn.mydata.myLink.ReturnByteArray(mavlinkexo);
            }


            if (ExoMonitorServer != null && ExoMonitorServer.IsOpen)
            {
                //MissionPlannerServer.  sendPacket(mavlinkexo);
                byte[] pck = mavComm.ReturnByteArray(mavlinkexo);
                ExoMonitorServer.Write(pck, 0, pck.Length);
                //MissionTCPServerConn.mydata.myLink.ReturnByteArray(mavlinkexo);
            }
        }

        private void MsgRouter_Load(object sender, EventArgs e)
        {

        }



    }
}
