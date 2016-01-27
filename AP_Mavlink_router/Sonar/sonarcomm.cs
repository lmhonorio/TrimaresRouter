using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace AP_Mavlink_router.Sonar
{
    public class sonarcomm
    {
        double Roll, Pitch, Yaw, Lat, Lng;
        UdpClient UDPswatch;
        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPAddress send_to_address = IPAddress.Parse("127.0.0.1");
        IPEndPoint sending_end_point;
        bool oktosend = false;

        public sonarcomm() 
        {
            sending_end_point = new IPEndPoint(send_to_address, 3000);

           // timer1.Start();
            UDPswatch = new UdpClient("120.0.0.1", 3000);
            oktosend = true;
        }
        public void setPosition(double lat, double lng) 
        {
            Lat = lat;
            Lng = lng;
        }
        public void setAttitude(double roll, double pitch, double yaw) 
        {
            Roll = roll;
            Pitch = pitch;
            Yaw = yaw;
        }

        int counter = 0;

        public void sendData_NMEA() 
        {
            double lat = (int)Lat+ ((Lat - (int)Lat) * .6f);
            double lng = (int)Lng + ((Lng - (int)Lng) * .6f);
            string line = string.Format("$GP{0},{1:HHmmss.fff},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},", "GGA", DateTime.Now.ToUniversalTime(), Math.Abs(lat * 100), Lat < 0 ? "S" : "N", Math.Abs(lng * 100), Lng < 0 ? "W" : "E", 1f, 2, 0, 0, "M", 0, "M", "");

            string checksum = GetChecksum(line);
            string fline = line + "*" + checksum;
            sending_socket.SendTo(getbytefromString(fline), sending_end_point);
           // comPort.WriteLine(line + "*" + checksum);

            line = string.Format("$GP{0},{1:HHmmss.fff},{2},{3},{4},{5},{6},{7},{8},{9:ddMMyy},{10},", "RMC", DateTime.Now.ToUniversalTime(), "A", Math.Abs(lat * 100), Lat < 0 ? "S" : "N", Math.Abs(lng * 100), Lng < 0 ? "W" : "E", 0 * 3.6,0, DateTime.Now, 0);

            checksum = GetChecksum(line);
            fline = (line + "*" + checksum);
            sending_socket.SendTo(getbytefromString(fline), sending_end_point);
            if (counter % 20 == 0 && Lat != 0 && Lng != 0)
            {
                line = string.Format("$GP{0},{1:HHmmss.fff},{2},{3},{4},{5},{6},{7},", "HOM", DateTime.Now.ToUniversalTime(), Math.Abs(Lat * 100), Lat < 0 ? "S" : "N", Math.Abs(Lng * 100), Lng < 0 ? "W" : "E", 0, "M");

                fline = (line + "*" + checksum);
                sending_socket.SendTo(getbytefromString(fline), sending_end_point);
            }

            line = string.Format("$GP{0},{1},{2},{3},", "RPY", Roll, Pitch,Yaw);

            fline = (line + "*" + checksum);
            sending_socket.SendTo(getbytefromString(fline), sending_end_point);

            counter++;
        }

        public byte[] getbytefromString(string line) 
        {
            char[] cline = line.ToCharArray();

            byte[] bline = new byte[cline.Length];

            for (int i = 0; i < cline.Length; i++) 
            {
                bline[i] = Convert.ToByte(cline[i]);
            }

            return bline;

        }

        public void sendData_POSMV102() 
        {
            try
            {
                if (!oktosend)
                    return;
                //double roll = ((CurrentState)bindingSource1.Current).roll;
                //double pitch = ((CurrentState)bindingSource1.Current).pitch;
                //double yaw = ((CurrentState)bindingSource1.Current).yaw;

                //double lat = ((CurrentState)bindingSource1.Current).lat;
                //double lng = ((CurrentState)bindingSource1.Current).lng;

                //  byte[] enviar = P.CreateMessage(roll, pitch, yaw);
                byte[] teste = new byte[136];

                byte[] header = new byte[4];
                header[0] = Convert.ToByte('$');
                header[1] = Convert.ToByte('G');
                header[2] = Convert.ToByte('R');
                header[3] = Convert.ToByte('P');
                ushort mid = 102;
                byte[] ID = BitConverter.GetBytes(mid);
                ushort bcount = 128;
                byte[] bc = BitConverter.GetBytes(bcount);
                byte[] corpo = new byte[80];

                Array.Copy(header, 0, teste, 0, header.Length);
                Array.Copy(ID, 0, teste, 4, 2);
                Array.Copy(bc, 0, teste, 6, 2);

                byte[] latlon = Trimares.swatch.ReturnLatLonBytes(Lat, Lng);
               // byte[] latlon = Trimares.swatch.ReturnLatLonBytes(-21.7774042,-43.3730544);
                Array.Copy(latlon, 0, teste, 34, latlon.Length);

                byte[] imu = Trimares.swatch.ReturnEullerBytes(Roll, Pitch, Yaw);
                Array.Copy(imu, 0, teste, 70, imu.Length);



                ushort chk = (ushort)Trimares.swatch.CheckSumPOSMV(teste);
                byte[] ck = BitConverter.GetBytes(chk);
                Array.Copy(ck, 0, teste, 132, 2);



                byte[] final = new byte[2];
                final[0] = Convert.ToByte('$');
                final[1] = Convert.ToByte('#');



                Array.Copy(final, 0, teste, 134, 2);
              
                // Array.Copy(enviar, teste, enviar.Length);
                // sending_socket.Send(enviar, SocketFlags.None);
                sending_socket.SendTo(teste, sending_end_point);
            }
            catch { }
        }

        string GetChecksum(string sentence)
        {
            // Loop through all chars to get a checksum
            int Checksum = 0;
            foreach (char Character in sentence.ToCharArray())
            {
                switch (Character)
                {
                    case '$':
                        // Ignore the dollar sign
                        break;
                    case '*':
                        // Stop processing before the asterisk
                        continue;
                    default:
                        // Is this the first value for the checksum?
                        if (Checksum == 0)
                        {
                            // Yes. Set the checksum to the value
                            Checksum = Convert.ToByte(Character);
                        }
                        else
                        {
                            // No. XOR the checksum with this character's value
                            Checksum = Checksum ^ Convert.ToByte(Character);
                        }
                        break;
                }
            }
            // Return the checksum formatted as a two-character hexadecimal
            return Checksum.ToString("X2");
        }


    }
}
