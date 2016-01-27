using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Net; // dns, ip address
using System.Net.Sockets; // tcplistner
//using log4net;
using System.IO;


namespace MAVcomm.Comms
{


    public class TCPCommServer : CommsBase, ICommsSerial
    {

        public int cPort = 9000;
        public bool oktoReceive = false;
        private Thread receiveThread;
        private Thread sendThread;
        private Thread startThread;
        public List<string> myReceivedData;


        public event MsgReceived newMsg;
        public Socket s;
        TcpListener listener;
        IPEndPoint ep;
        TcpClient client;

        public TCPCommServer()
        {
            
            //startThread = new Thread(new ThreadStart(startServer));
            //startThread.Start();

        }

        public TCPCommServer(int port)
        {
            cPort = port;
            //startThread = new Thread(new ThreadStart(startServer));
            //startThread.Start();

        }

        private void start() 
        {
            startThread = new Thread(new ThreadStart(startServer));
            startThread.Start();
        }

        private void startServer()
        {
           // UDPPort = UDPPort;





            this.BaudRate = 115200;
            ep = new IPEndPoint(IPAddress.Any, cPort);



            listener = new TcpListener(ep);
            listener.Start(1);

            if (rMessage != null)
            {
                rMessage("Esperando Cliente na porta " + cPort);
            }

            s = listener.AcceptSocket();
            client = listener.AcceptTcpClient();
            client.Client = s;

            if (rMessage != null)
            {
                rMessage("conectadoTCPserver");
                rMessage("Cliente Encontrado na porta: " + cPort);
            }

            oktoReceive = true;


            startReceive();

        }

        private void startReceive()
        {
            receiveThread = new Thread(new ThreadStart(receive));
            receiveThread.Start();
        }


        public void startSending(byte[] msg)
        {
            sendThread = new Thread(new ParameterizedThreadStart(send));
            sendThread.Start(msg);
        }



        public void send(object sendbuf)
        {
            try
            {
                //atencao

                byte[] msg = ((byte[])sendbuf);
                client.Client.Send(msg,msg.Length,SocketFlags.None);
               // this.Write(msg, 0, msg.Length);
               // Console.WriteLine("Message sent to the broadcast address");
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString()); 
            }
        }


        public void receive()
        {
        
            
            try
            {
                while (oktoReceive)
                {
                    byte[] bytes = new byte[256];
                    client.Client.Receive(bytes);
                    if(newMsg!=null)
                        newMsg(bytes);
                }
            }
            catch (Exception ex)
            {
                if (rMessage != null)
                {
                    rMessage("desconectadoTCPserver");
                }

                s.Disconnect(true);
                s.Close();
                client.Close();
                listener.Stop();

                Thread.Sleep(200);
                client = null;
                oktoReceive = false;
                ep = null;
                listener = null;
 
               
               // rMessage("erro no receive servidor");
           
            }
        }


        //IMPLEMENTAÇÃO DAS INTERFACES

        //private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        IPEndPoint RemoteIpEndPoint;// = new IPEndPoint(IPAddress.Any, 0);
        public int WriteBufferSize { get; set; }
        public int WriteTimeout { get; set; }
        public bool RtsEnable { get; set; }
        public Stream BaseFileStream { get { return this.BaseFileStream; } }


         public void toggleDTR()
        {
        }

        public string Port { get; set; }

        public int ReadTimeout
        {
            get;// { return client.ReceiveTimeout; }
            set;// { client.ReceiveTimeout = value; }
        }

        public int ReadBufferSize { get; set; }

        public int BaudRate { get; set; }
        public StopBits StopBits { get; set; }
        public Parity Parity { get; set; }
        public int DataBits { get; set; }

        public string PortName { get; set; }

        public int BytesToRead
        {
            get { /*Console.WriteLine(DateTime.Now.Millisecond + " tcp btr " + (client.Available + rbuffer.Length - rbufferread));*/ return (int)client.Available; }
        }

        public int BytesToWrite { get { return 0; } }

        public bool IsOpen { get { try { return (client != null &&  client.Client.Connected); } catch { return false; } } }

        public bool DtrEnable
        {
            get;
            set;
        }

        public void Open()
        {
            if (client !=null && client.Client.Connected)
            {
                //log.Warn("tcpserial socket already open");
                return;
            }

            start();
    
            //VerifyConnected();

            return;
        }

        int retrys = 3;
        void VerifyConnected()
        {
            if (client == null || !IsOpen)
            {
                try
                {
                    client.Close();
                }
                catch { }

                // this should only happen if we have established a connection in the first place
                if (client != null && retrys > 0)
                {
                    //log.Info("tcp reconnect");
                    client.Connect(OnSettings("TCP_host", ""), int.Parse(OnSettings("TCP_port", "")));
                    retrys--;
                }

                throw new Exception("The socket/serialproxy is closed");
            }
        }

        public int Read(byte[] readto, int offset, int length)
        {
            VerifyConnected();
            try
            {
                if (length < 1) { return 0; }

                return client.Client.Receive(readto, offset, length, SocketFlags.None);
                /*
                                byte[] temp = new byte[length];
                                clientbuf.Read(temp, 0, length);

                                temp.CopyTo(readto, offset);

                                return length;*/
            }
            catch { throw new Exception("Socket Closed"); }
        }

        public int ReadByte()
        {
            VerifyConnected();
            int count = 0;
            while (this.BytesToRead == 0)
            {
                System.Threading.Thread.Sleep(1);
                if (count > ReadTimeout)
                    throw new Exception("NetSerial Timeout on read");
                count++;
            }
            byte[] buffer = new byte[1];
            Read(buffer, 0, 1);
            return buffer[0];
        }

        public int ReadChar()
        {
            return ReadByte();
        }

        public string ReadExisting()
        {
            VerifyConnected();
            byte[] data = new byte[client.Available];
            if (data.Length > 0)
                Read(data, 0, data.Length);

            string line = Encoding.ASCII.GetString(data, 0, data.Length);

            return line;
        }

        public void WriteLine(string line)
        {
            VerifyConnected();
            line = line + "\n";
            Write(line);
        }

        public void Write(string line)
        {
            VerifyConnected();
            byte[] data = new System.Text.ASCIIEncoding().GetBytes(line);
            Write(data, 0, data.Length);
        }

        public void Write(byte[] write, int offset, int length)
        {
            VerifyConnected();
            try
            {
                client.Client.Send(write, length, SocketFlags.None);
            }
            catch { }//throw new Exception("Comport / Socket Closed"); }
        }

        public void DiscardInBuffer()
        {
            VerifyConnected();
            int size = (int)client.Available;
            byte[] crap = new byte[size];
            //log.InfoFormat("TcpSerial DiscardInBuffer {0}", size);
            Read(crap, 0, size);
        }

        public string ReadLine()
        {
            byte[] temp = new byte[4000];
            int count = 0;
            int timeout = 0;

            while (timeout <= 100)
            {
                if (!this.IsOpen) { break; }
                if (this.BytesToRead > 0)
                {
                    byte letter = (byte)this.ReadByte();

                    temp[count] = letter;

                    if (letter == '\n') // normal line
                    {
                        break;
                    }


                    count++;
                    if (count == temp.Length)
                        break;
                    timeout = 0;
                }
                else
                {
                    timeout++;
                    System.Threading.Thread.Sleep(5);
                }
            }

            Array.Resize<byte>(ref temp, count + 1);

            return Encoding.ASCII.GetString(temp, 0, temp.Length);
        }

        public void Close()
        {
            try
            {
                if (client.Client.Connected)
                {
                    client.Client.Close();
                    client.Close();
                }
                if (listener != null) 
                {
                    listener.Stop();
                    listener = null;
                }
                if (client != null) 
                {
                    client.Close();
                    client = null;
                }
                if (shb != null) 
                {
                    shb.Abort();
                    shb = null;
                }
            }
            catch { }

            try
            {
                client.Close();
            }
            catch { }

            client = new TcpClient();
        }




        public Socket isocket
        {
            get { return s; }
        }

        public TcpListener ilistener
        {
            get { return listener; }
        }

        public IPEndPoint iep
        {
            get { return ep; }
        }

        public TcpClient iclient
        {
            get { return client; }
        }


        public string HostIP
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public event ReturnMessage rMessage;

        bool sendHBThread = false;

        void sendingOneHBps() 
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
            
           // mydata.myLink.sendPacket(htb);

            while (sendHBThread)
            {
                try
                {
                    Thread.Sleep(950);

                    if (this.IsOpen)
                    {
                         this.Write(pabyte, 0, pabyte.Length);
                    }


                }
                catch (Exception e)
                {
                    //log.Error("HB sending fail :" + e.ToString());
                    try
                    {
                        this.Close();
                    }
                    catch { }
                }
            }
        }


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









   



