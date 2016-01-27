using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;

namespace MAVcomm.Comms
{
    public class CommsFile : ICommsSerial
    {
        // Methods
        public void Close() { BaseFileStream.Close(); }
        public void DiscardInBuffer() { }
        //void DiscardOutBuffer();
        public void Open()
        {
            BaseFileStream = File.OpenRead(PortName);
        }
        public int Read(byte[] buffer, int offset, int count)
        {
            return BaseFileStream.Read(buffer, offset, count);
        }
        //int Read(char[] buffer, int offset, int count);
        public int ReadByte() { return BaseFileStream.ReadByte(); }
        public int ReadChar() { return BaseFileStream.ReadByte(); }
        public string ReadExisting() { return ""; }
        public string ReadLine() { return ""; }
        //string ReadTo(string value);
        public void Write(string text) { }
        public void Write(byte[] buffer, int offset, int count) { }
        //void Write(char[] buffer, int offset, int count);
        public void WriteLine(string text) { }

        public void toggleDTR() { }

        // Properties
        public Stream BaseFileStream { get; private set; }
        public int BaudRate { get; set; }
        public int BytesToRead { get { return (int)(BaseFileStream.Length - BaseFileStream.Position); } }
        public int BytesToWrite { get; set; }
        public int DataBits  { get; set; }
        public bool DtrEnable { get; set; }
        public bool IsOpen { get { return (BaseFileStream != null); } }

        public Parity Parity { get; set; }

        public string PortName { get; set; }
        public int ReadBufferSize { get; set; }
        public int ReadTimeout { get; set; }
        public bool RtsEnable { get; set; }
        public StopBits StopBits { get; set; }
        public int WriteBufferSize { get; set; }
        public int WriteTimeout { get; set; }

       // public event MsgReceived newMsgReceived;

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


        public string Port
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


        public void StartSendingHB()
        {

        }
    }
}
