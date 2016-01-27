using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;

namespace clsExodn
{
    public delegate void DataRead(string x);
    public delegate void StrcExoRead(exo_waterquality exor);

    public class ExoData:IDisposable 
    {
        public event DataRead dataread;
        public event StrcExoRead exoRead;

        public ExoReading readings;
        public List<exo_waterquality> Exodata;

        StreamWriter sw;

        /// <summary>
        /// 
        /// </summary>
        public double currentlat { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double currentlon { get; set; }

        string name;

        public ExoData() 
        {
            Exodata = new List<exo_waterquality>();
            readings = new ExoReading();
            readings.exoRead += ExoReadings_exoRead;
            readings.dataread += ExoReadings_dataread;

            name = String.Format("{0:yyMMddHHmmss}.dat", DateTime.Now);            

        }

        ~ExoData() 
        {
            if(sw!=null){
                sw.Close();
                sw.Dispose();
                sw = null;
            }

            
        }

       

        private void ExoReadings_dataread(string x)
        {

            if (dataread != null)
                dataread(x);

           
        }

        private void ExoReadings_exoRead(exo_waterquality exor)
        {

            exor.setLatLon(currentlat, currentlon);

            if (sw != null)
            {
                sw.WriteLine(exor.toText());
            }


            Exodata.Add(exor);

            if (exoRead != null)
                exoRead(exor);
        }

        public void setlatlon(double nlat, double nlon) 
        {

        }


        public void starReadings() 
        {
            try
            {
                if (sw == null)
                {

                    sw = new StreamWriter(new FileStream(name,FileMode.Create),Encoding.ASCII);
                }
                readings.startReadings();
            }
            catch 
            {
                if (sw != null) 
                {
                    sw.Close();
                    sw.Dispose();
                    sw = null;
                }
            }
        }

        public void stopReadings() 
        {
            readings.stopReadings();
        }

        public void ClearBuffer() 
        {
            readings.ClearBuffer();
        }



    
public void Dispose()
{
    if (sw != null)
    {
        sw.Close();
        sw.Dispose();
        sw = null;
    }
}



}

    public class ExoReading
    {
        SerialPort port;
        public event DataRead dataread;
        public event StrcExoRead exoRead;
     

        byte[] clear = { 0x0D, 0x0A };//<CR><LF>
        byte[] deploy = { 0x72, 0x75, 0x6E, 0x0D };//<R><U><N><CR><LF> , 0x0A 
        byte[] abort = { 0x30, 0x0D, 0x0A };//<0><CR><LF>

        public ExoReading() 
        {
            try
            {
                port = new SerialPort("COM2");
                //port.WriteBufferSize= 2048;
                port.BaudRate = 9600;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;

                if (port.IsOpen) 
                {
                    stopReadings();
                    ClearBuffer();
                    port.Close();
                    Thread.Sleep(100);
                }


                port.DataReceived += port_DataReceived;
                port.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            string x = port.ReadLine();
            
            convert(x);

            
        }

        public void convert(string x)
        {
            try
            {

                string[] par = x.Split(" ".ToCharArray());

                if (x.Length > 20 && par.Length == 15)
                {
                    exo_waterquality exo = new exo_waterquality();
                    exo.setValues(par);
                    exoRead(exo);
                    dataread(x);
                }
                else
                {
                   // dataread(x);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void startReadings() 
        {
            if (port != null && port.IsOpen) 
            {
                port.Write(deploy, 0, 4);
                Thread.Sleep(100);
                port.Write(deploy, 0, 4);
                Thread.Sleep(100);
                port.Write(deploy, 0, 4);
            }
        }

        public void ClearBuffer()
        {
            if (port != null && port.IsOpen)
            {
                port.Write(clear, 0, 2);
            }
        }

        public void stopReadings()
        {
            if (port != null && port.IsOpen)
            {
                port.Write(abort, 0, 3);
            }
        }

        
    }

}
