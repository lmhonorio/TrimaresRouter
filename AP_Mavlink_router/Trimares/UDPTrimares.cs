using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace Trimares
{

    public class UDPComm
    {


        public static int UDPPort = 3000;
        public static bool oktoReceive = true;
        private Thread receiveThread;
        private Thread sendThread;
        private Thread connecThread;


        public List<string> myReceivedData;
        public delegate void MsgReceived(byte[] bytes);
        public event MsgReceived newMsg;

        UdpClient listener;
        IPEndPoint Tri_Mares;

        public UDPComm()
        {
            myReceivedData = new List<string>();


        }

        public void startConnection(long maxDeltaT)
        {
            connecThread = new Thread(new ParameterizedThreadStart(Connectto3Mares));
            connecThread.Start((object)maxDeltaT);
        }


        public void startReceive()
        {
            receiveThread = new Thread(new ThreadStart(receive));
            receiveThread.Start();
        }

        public void startSending(string msg)
        {
            sendThread = new Thread(new ParameterizedThreadStart(send));
            sendThread.Start(msg);

        }

        private void send(object objmsg)
        {
            try
            {
                string mystring = (string)objmsg;
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPAddress broadcast = IPAddress.Parse("127.0.0.1");
                byte[] sendbuf = Encoding.ASCII.GetBytes(mystring);
                IPEndPoint ep = new IPEndPoint(broadcast, UDPPort);
                s.SendTo(sendbuf, ep);                
                Console.WriteLine("Message sent to the broadcast address");
                s.Close();
            }
            catch { }
        }


        public void Send(byte[] pacote) 
        {
            try
            {
      
               // Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
               // IPAddress broadcast = IPAddress.Parse("127.0.0.1");
                listener.Send(pacote, pacote.Length);
              //  IPEndPoint ep = new IPEndPoint(broadcast, UDPPort);
              //  s.SendTo(pacote, Tri_Mares);
                Console.WriteLine("Message sent to the broadcast address");
               // s.Close();
            }
            catch { }
        }



        private void Connectto3Mares(object objmaxDeltaT)
        {
            long maxDeltaT = Convert.ToInt32(objmaxDeltaT);
            string validate = "i am here";
            string stringData = "";
            long t1 = DateTime.Now.Ticks;
            long t2 = DateTime.Now.Ticks;

            long deltaT = (long)TimeSpan.FromTicks(t2 - t1).TotalSeconds;

            try
            {
                while (stringData != validate && deltaT < maxDeltaT)
                {
                //    listener = new UdpClient(UDPPort);
                //     Tri_Mares = new IPEndPoint(IPAddress.Parse("127.0.0.1"),3000);
                ////    IPEndPoint Tri_Mares = new IPEndPoint(IPAddress.Parse("192.168.1.188"), UDPPort);
                //    IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, UDPPort);
                    byte[] sendbuf = Encoding.ASCII.GetBytes(validate);
                //    listener.Send(sendbuf, sendbuf.Length, Tri_Mares);
                //    sendbuf = listener.Receive(ref groupEP);
                //    stringData = Encoding.ASCII.GetString(sendbuf, 0, sendbuf.Length);
                //    t2 = DateTime.Now.Ticks;
                //    deltaT = (long)TimeSpan.FromTicks(t2 - t1).TotalSeconds;
                //    listener.Close();


                    listener = new UdpClient(UDPPort);
                    Tri_Mares = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000);
                    //    IPEndPoint Tri_Mares = new IPEndPoint(IPAddress.Parse("192.168.1.188"), UDPPort);
                    listener.Send(sendbuf, sendbuf.Length, Tri_Mares);
                    sendbuf = listener.Receive(ref Tri_Mares);
                    stringData = Encoding.ASCII.GetString(sendbuf, 0, sendbuf.Length);
                    t2 = DateTime.Now.Ticks;
                    deltaT = (long)TimeSpan.FromTicks(t2 - t1).TotalSeconds;
                    listener.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        private void receive()
        {
            try
            {
                UdpClient listener = new UdpClient(UDPPort);
                IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, UDPPort);
                while (oktoReceive)
                {
                    byte[] bytes = listener.Receive(ref groupEP);
                    newMsg(bytes);
                }
                listener.Close();
            }
            catch { }
        }

        public void close() 
        {
  
            connecThread.Abort();

        }

    }
}
