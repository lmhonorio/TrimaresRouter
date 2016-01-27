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

    public class TcpSerial : CommsBase, ICommsSerial
    {
       // private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public TcpClient client = new TcpClient();
        IPEndPoint RemoteIpEndPoint;// = new IPEndPoint(IPAddress.Any, 0);
        object myKey = new object();
        Socket s;
        TcpListener listener;

        public bool oktoReceive = false;
        private Thread receiveThread;
        private Thread sendThread;
        private Thread startThread;

        string dest;
        string host;

        int retrys = 3;
        
        public int WriteBufferSize { get; set; }
        public int WriteTimeout { get; set; }
        public bool RtsEnable { get; set; }
        public Stream BaseFileStream { get { return this.BaseFileStream; } }

        ~TcpSerial()
        {
            this.Close();
            client = null;
        }

        public TcpSerial()
        {
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            Port = "9000";
            
        }

        private void startReceive()
        {
            receiveThread = new Thread(new ThreadStart(receive));
            receiveThread.Start();
        }

        public void receive()
        {
            try
            {
                while (oktoReceive)
                {
                    byte[] bytes = new byte[256];
                    client.Client.Receive(bytes);
                    newMsg(bytes);
                }
            }
            catch (Exception ex)
            {
                if (rMessage != null)
                {
                    rMessage("desconectadoTCP");
                    rMessage(ex.ToString());
                }

                client.Close();
                Thread.Sleep(200);
                client = null;
                
            }
        }

        private void start()
        {
            startThread = new Thread(new ThreadStart(startServer));
            startThread.Start();
        }

        private void startServer()
        {
            if (client.Client.Connected)
            {
                //log.Warn("tcpserial socket already open");
                return;
            }

            //log.Info("TCP Open");

            //  dest = "9000";
            //   host = "127.0.0.1";

            Port = dest;

            //OnSettings("TCP_port", Port, true);
            //OnSettings("TCP_host", host, true);

            client = new TcpClient();
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse(host), int.Parse(Port));
            client.Connect(RemoteIpEndPoint);
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(RemoteIpEndPoint);
            client.NoDelay = true;
            client.Client.NoDelay = true;
            if (rMessage != null)
            {
                rMessage("Cliente Conectado tcp");
            }
            // VerifyConnected();

          
            oktoReceive = true;
            startReceive();

        }


        public void Open()
        {
            start();
        }



        bool serialThread;

 

        public void toggleDTR()
        {
        }

        public string Port 
        { 
            get {return dest;   }
            set { dest = value; }
        }
        public string HostIP
        {
            get { return host; ; }
            set { host = value;  }
        }

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

        public bool IsOpen { get { try { return client.Client.Connected; } catch { return false; } } }

        public bool DtrEnable
        {
            get;
            set;
        }



        void VerifyConnected()
        {
            if (client == null || !IsOpen)
            {
                rMessage("erro coneccao");
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
            }
            catch { }

            try
            {
                client.Close();
            }
            catch { }

            client = new TcpClient();
        }



        public event MsgReceived newMsg;

        public Socket isocket
        {
            get { throw new NotImplementedException(); }
        }

        public TcpListener ilistener
        {
            get { throw new NotImplementedException(); }
        }

        public IPEndPoint iep
        {
            get { throw new NotImplementedException(); }
        }

        public TcpClient iclient
        {
            get { throw new NotImplementedException(); }
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




