using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trimares
{

    // No support for strongly typed enums
    public enum MsgType
    {

        // System group
        SYS_ACK = 1,
        SYS_RESET = 2,
        SYS_CHKCON = 3,
        SYS_ALIVE = 4,
        SYS_SET_COMM = 5,
        SYS_REG = 6,
        SYS_TEXT = 7,
        SYS_START = 8,
        SYS_STOP = 9,

        // Status group
        STA_GET = 10,
        STA_SEND = 11,
        STA_GET_CTR = 12,
        STA_SEND_CTR = 13,
        STA_GET_MSGS = 14,
        STA_STOP_MSGS = 15,	//Jan 2011, NABREU

        // Acoustics group
        ACS_PING = 20,
        ACS_EVENT = 21,
        ACS_RESET = 22,
        ACS_SET_COMM = 23,
        ACS_SET_REPLY_CH = 24,
        ACS_GET_REPLY_CH = 25,
        ACS_SEND_REPLY_CH = 26,
        ACS_SET_REPLY_CH_VEC = 27,
        ACS_GET_REPLY_CH_VEC = 28,
        ACS_SEND_REPLY_CH_VEC = 29,
        ACS_SET_POT_LEVEL = 30,
        ACS_GET_POT_LEVEL = 31,
        ACS_SEND_POT_LEVEL = 32,
        ACS_SET_POT_LEVEL_VEC = 33,
        ACS_GET_POT_LEVEL_VEC = 34,
        ACS_SEND_POT_LEVEL_VEC = 35,
        ACS_UPDOWN_POT_LEVEL = 36, // Nov/2007

        // FT (file transfer group)
        FT_HEADER = 40,
        FT_ACK = 41,
        FT_PACKET = 42,

        // GPS group
        GPS_GET_2D = 50,
        GPS_SEND_2D = 51,
        GPS_GET_RATE = 52,
        GPS_SET_RATE = 53,
        GPS_SEND_RATE = 54,
        GPS_SET_COMM = 55,
        GPS_SEND_UBLOXSVINFO = 56,   // Oct/2007, NLS
        GPS_SEND_3D = 57,   				 // Fev/2008, ACM

        // LBL group
        LBL_SEND_RANGE = 60,
        LBL_SEND_2D = 61,
        LBL_SET_COMM = 62,
        LBL_SET_BUOYPOS = 63,        // Oct/2007, NLS
        LBL_MOB_SEND_RANGE = 64,     // Apr/2008, NLS
        LBL_MOB_SEND_2D = 65,				 // Apr/2008, NLS
        LBL_MOB_SET_BASELINEMAP = 66,// Jun/2008, NLS
        LBL_SET_BASELINEMAP = 67, // Mar/2011, NA

        // Navigation group
        NAV_GET_ABS_STATUS = 70,
        NAV_GET_REL_STATUS = 71,
        NAV_SEND_ABS_STATUS = 72,
        NAV_SEND_REL_STATUS = 73,
        NAV_SET_POS = 74,
        NAV_MOB_GET_STATUS = 75,		 // Apr/2008, NLS
        NAV_MOB_SEND_STATUS = 76,		 // Apr/2008, NLS
        NAV_MOB_SET_NETWORK = 77, 	 // Apr/2008, NLS
        NAV_MOB_SET_POS = 78,				 // Apr/2008, NLS

        // Pressure group
        PRS_SET_SURF = 80,
        PRS_RESET_SURF = 81,
        PRS_GET = 82,
        PRS_SEND = 83,
        PRS_STATUS = 84,

        // Mission group
        MIS_START = 90,
        MIS_STARTED = 91,
        MIS_STOP = 92,
        MIS_ABORT = 93,

        // Tracking group
        TRK_POS = 100,

        // Actuation group
        ACT_MOTORS_CMD = 110,
        ACT_MOTORS_REAL = 111,
        ACT_THRUSTERS_CMD = 112,	// Jan 2010, BMF
        ACT_THRUSTERS_REAL = 113,	// Jan 2010, BMF

        // Compass group
        CPS_SET_DEV = 120,
        CPS_SET_CAL_START = 121,
        CPS_SET_CAL_END = 123,
        CPS_SEND_CAL = 124,
        CPS_GET = 125,
        CPS_SEND = 126,

        //Jan2011, JLM
        CPS_MTI = 127,


        // Kalman filter group
        KLM_SET_POS = 130,
        KLM_DEAD_RECK = 131,
        KLM_RNG_CRT = 132,
        KLM_MOB_SET_POS = 133, 			// Apr/2008, NLS
        KLM_MOB_SET_NETWORK = 134,		// Apr/2008, NLS	
        KLM_MOB_INITIATE = 135,			// Apr/2008, NLS


        // Payload CTD Sensor
        CTD_GET = 140,
        CTD_SEND = 141,

        // Guidance group
        GDC_MANEUVER = 150,
        GDC_SET_REFERENCE = 151,   // JUL/2008, NLS
        EXT_H_SPEED_REF = 152,
        EXT_V_SPEED_REF = 153,
        MOTION_REF = 154,


        // Log Control				
        LOG_NEW_FILE = 160,			//Mar/2010, NA       OLD LOGGER
        LOG_ENABLE_MSGTYPE = 161,	//Mar/2010, NA
        LOG_DISABLE_MSGTYPE = 162,	//Mar/2010, NA

        // ASV specific data
        ASV_STATE = 170,			// Feb/2008, ACM
        ASV_ACTUATION = 171,		// Feb/2008, ACM
        ASV_INTERNAL = 172,			// Feb/2008, ACM
        ASV_SET_MODE = 173,			// Feb/2008, ACM
        ASV_REMOTE_COMMAND = 174,	// Feb/2008, ACM
        ASV_REMOTE_LINE = 175,		// Feb/2008, ACM
        ASV_LINE_TRACK = 176,		// Mar/2008, ACM
        ASV_ANCHOR = 177,			// Apr/2008, ACM

        ASV_SET_PARAM = 180,		// Aug/2008, ACM

        // Hardware specific
        AX3500_STATE = 200,     	 // Feb/2008, ACM
        AX3500_ACTUATION = 201, 	 // Feb/2008, ACM
        AX3500_COMMAND = 202,    	 // Apr/2008, ACM

        // Thermocline Tracking
        TCT_STATE = 210,                // Jul/2008, NAC 

        // Ecopucks
        ECOPUCKS_DATA = 211,	// Oct/2008

        // BAM
        BAM_DATA = 212,	// Nov/2010

        // Altimeter
        ALTIMETER_DATA = 213,	// Nov/2010    *RETURNS DOUBLE

        // Battery Status
        BATT_STATUS = 214,

        //Process Manager
        PROC_START = 220,		// Feb/2011, NA
        PROC_SUSPEND = 221,	// Feb/2011, NA
        PROC_RESUME = 222,	// Feb/2011, NA
        PROC_STOP = 223,		// Feb/2011, NA
        PROC_LIST = 224,
        PROC_REBOOT = 225,

        // Supervision
        SUPERVISION_LEAK = 230,
    };

    public enum TypeConvertion
    {

    }

    /// <summary>
    /// Classe inicial de tradução
    /// </summary>
    public class clsMsgTranslator
    {
        public IPMsg myMsg;
        public LBLSend2D myLBLSend2D;
        public PrsSend myPrsSend;
        public NavSendRelStatus myNavSendRelStatus;
        public NavSendAbsStatus myNavSendAbsStatus; //dados corrigidos pelo F.K.
        public BattStatus myBattStatus;
        public CpsSend myCpsSend;
        public CpsMti myCpsMti;
        public GPSSend2D myGPSSend2D;
        public GPSSend3D myGPSSend3D;
        public AltimeterData myAltimeterData;
        public CtdSend myCtdSend;
        public SupervisionLeak mySupervisionLeak;
        public tSurf mytSurf;
        public MotionRef myMotionRef;
        public SysAck mySysAck;
        public KlmSetPos myKlmSetPos;
        public KlmDeadReck myKlmDeadReck;
        public KlmRngCrt myKlmRngCrt;
        public KlmMobInitiate myKlmMobInitiate;
        public KlmMobSetPos myKlmMobSetPos;
        public KlmMobSetNetwork myKlmMobSetNetwork;
        public LBLSetBuoyPos myLBLSetBuoyPos;
        public MisStart myMisStart;

        public clsMsgTranslator() { }

        public void translate(byte[] bytes)
        {
            myMsg = new IPMsg(bytes);

            switch ((MsgType)myMsg.header.msgtype)
            {

                case MsgType.LBL_SEND_2D:
                    if (myMsg.msgbyte.Length != 28)
                        break;
                    myLBLSend2D = new LBLSend2D(myMsg.msgbyte);
                    break;

                case MsgType.PRS_SEND:
                    if (myMsg.msgbyte.Length != 10)
                        break;
                    myPrsSend = new PrsSend(myMsg.msgbyte);
                    break;

                case MsgType.BATT_STATUS:
                    if (myMsg.msgbyte.Length != 36)
                        break;
                    myBattStatus = new BattStatus(myMsg.msgbyte);
                    break;

                case MsgType.NAV_SEND_REL_STATUS:
                    if (myMsg.msgbyte.Length != 96)
                        break;
                    myNavSendRelStatus = new NavSendRelStatus(myMsg.msgbyte);
                    break;

                case MsgType.CPS_SEND:
                    if (myMsg.msgbyte.Length != 62)
                        break;
                    myCpsSend = new CpsSend(myMsg.msgbyte);
                    break;

                case MsgType.CPS_MTI:
                    if (myMsg.msgbyte.Length != 80)
                        break;
                    myCpsMti = new CpsMti(myMsg.msgbyte);
                    break;

                case MsgType.GPS_SEND_2D:
                    if (myMsg.msgbyte.Length != 24)
                        break;
                    myGPSSend2D = new GPSSend2D(myMsg.msgbyte);
                    break;

                case MsgType.GPS_SEND_3D:
                    if (myMsg.msgbyte.Length != 72)
                        break;
                    myGPSSend3D = new GPSSend3D(myMsg.msgbyte);
                    break;

                case MsgType.NAV_SEND_ABS_STATUS:
                    if (myMsg.msgbyte.Length != 96)
                        break;
                    myNavSendAbsStatus = new NavSendAbsStatus(myMsg.msgbyte);
                    break;

                case MsgType.ALTIMETER_DATA:
                    if (myMsg.msgbyte.Length != 20)
                        break;
                    myAltimeterData = new AltimeterData(myMsg.msgbyte);
                    break;

                case MsgType.CTD_SEND:
                    if (myMsg.msgbyte.Length != 40)
                        break;
                    myCtdSend = new CtdSend(myMsg.msgbyte);
                    break;

                case MsgType.SUPERVISION_LEAK:
                    if (myMsg.msgbyte.Length != 4)
                        break;
                    mySupervisionLeak = new SupervisionLeak(myMsg.msgbyte);
                    break;

                case MsgType.PRS_SET_SURF:
                    if (myMsg.msgbyte.Length != 8)
                        break;
                    mytSurf = new tSurf(myMsg.msgbyte);
                    break;

                case MsgType.MOTION_REF:
                    if (myMsg.msgbyte.Length != 60)
                        break;
                    myMotionRef = new MotionRef(myMsg.msgbyte);
                    break;

                case MsgType.SYS_ACK:
                    if (myMsg.msgbyte.Length != 2)
                        break;
                    mySysAck = new SysAck(myMsg.msgbyte);
                    break;

                case MsgType.KLM_SET_POS:
                    if (myMsg.msgbyte.Length != 32)
                        break;
                    myKlmSetPos = new KlmSetPos(myMsg.msgbyte);
                    break;

                case MsgType.KLM_DEAD_RECK:
                    if (myMsg.msgbyte.Length != 16)
                        break;
                    myKlmDeadReck = new KlmDeadReck(myMsg.msgbyte);
                    break;

                case MsgType.KLM_RNG_CRT:
                    if (myMsg.msgbyte.Length != 36)
                        break;
                    myKlmRngCrt = new KlmRngCrt(myMsg.msgbyte);
                    break;

                case MsgType.KLM_MOB_INITIATE:
                    if (myMsg.msgbyte.Length != 56)
                        break;
                    myKlmMobInitiate = new KlmMobInitiate(myMsg.msgbyte);
                    break;

                case MsgType.KLM_MOB_SET_POS:
                    if (myMsg.msgbyte.Length != 24)
                        break;
                    myKlmMobSetPos = new KlmMobSetPos(myMsg.msgbyte);
                    break;

                case MsgType.KLM_MOB_SET_NETWORK:
                    if (myMsg.msgbyte.Length != 32)
                        break;
                    myKlmMobSetNetwork = new KlmMobSetNetwork(myMsg.msgbyte);
                    break;

                case MsgType.LBL_SET_BUOYPOS:
                    if (myMsg.msgbyte.Length != 26)
                        break;
                    myLBLSetBuoyPos = new LBLSetBuoyPos(myMsg.msgbyte);
                    break;

                case MsgType.MIS_START:
                    if (myMsg.msgbyte.Length != 8)
                        break;
                    myMisStart = new MisStart(myMsg.msgbyte);
                    break;

                default:
                    break;

            }

        }
    }

    public class IPMsgHeader
    {


        public char id;
        public char subid;
        public ushort msgtype;

        public IPMsgHeader()
        {

        }

        public IPMsgHeader(byte[] bytes)
        {
            id = Convert.ToChar(bytes[0]);
            subid = Convert.ToChar(bytes[1]);
            msgtype = BitConverter.ToUInt16(new byte[] { bytes[2], bytes[3] }, 0);

        }

    }

    public class IPMsg
    {

        public IPMsgHeader header;
        public byte[] msgbyte;

        public IPMsg() { }

        public IPMsg(byte[] bytes)
        {
            byte[] hbyte = new byte[4];
            msgbyte = new byte[bytes.Length - 4];

            Array.Copy(bytes, 0, hbyte, 0, 4);
            Array.Copy(bytes, 4, msgbyte, 0, msgbyte.Length);

            header = new IPMsgHeader(hbyte);

        }

    }

    /// <summary>
    /// LBL_SEND_2D (#61): Posição absoluta da navegação acústica
    /// </summary>
    public class LBLSend2D
    {
        /// <summary>
        /// time: (uint32) tempo desde o arranque do software (ms)
        /// </summary>
        uint time;
        /// <summary>
        /// lon: (double) longitude (rad)
        /// </summary>
        double lon;
        /// <summary>
        /// lat: (double) latitude (rad)
        /// </summary>
        double lat;
        /// <summary>
        /// hacc: (double) precisão (m)
        /// </summary>
        double hacc;

        public uint ptime { get { return time; } }
        public double plon { get { return lon; } }
        public double plat { get { return lat; } }
        public double phacc { get { return hacc; } }

        public LBLSend2D() { }

        public LBLSend2D(byte[] bytes)
        {

            byte[] b1 = new byte[4];
            byte[] b2 = new byte[8];
            byte[] b3 = new byte[8];
            byte[] b4 = new byte[8];

            Array.Copy(bytes, 0, b1, 0, 4);
            Array.Copy(bytes, 4, b2, 0, 8);
            Array.Copy(bytes, 12, b3, 0, 8);
            Array.Copy(bytes, 20, b4, 0, 8);

            time = BitConverter.ToUInt16(b1, 0);
            lon = BitConverter.ToDouble(b2, 0);
            lat = BitConverter.ToDouble(b3, 0);
            hacc = BitConverter.ToDouble(b4, 0);
        }
    }

    /// <summary>
    /// GPS_SEND_2D (#51): Latitude e longitude fornecidas por GPS
    /// </summary>
    public class GPSSend2D
    {
        /// <summary>
        /// tempo da semana GPS (ms)
        /// </summary>
        uint timeofweek;
        /// <summary>
        /// latitude (rad)
        /// </summary>
        double lon;
        /// <summary>
        /// longitude (rad)
        /// </summary>
        double lat;
        /// <summary>
        /// precisão horizontal (mm)
        /// </summary>
        uint hacc;

        public uint ptimeofweek { get { return timeofweek; } }
        public double plon { get { return lon; } }
        public double plat { get { return lat; } }
        public uint phacc { get { return hacc; } }

        public GPSSend2D() { }

        public GPSSend2D(byte[] bytes)
        {
            byte[] b1 = new byte[4];
            byte[] b2 = new byte[8];
            byte[] b3 = new byte[8];
            byte[] b4 = new byte[4];

            Array.Copy(bytes, 0, b1, 0, 4);
            Array.Copy(bytes, 4, b2, 0, 8);
            Array.Copy(bytes, 12, b3, 0, 8);
            Array.Copy(bytes, 20, b4, 0, 4);

            timeofweek = BitConverter.ToUInt32(b1, 0);
            lon = BitConverter.ToDouble(b2, 0);
            lat = BitConverter.ToDouble(b3, 0);
            hacc = BitConverter.ToUInt32(b4, 0);

        }
    }

    /// <summary>
    /// NAV_SEND_REL_STATUS (#73): Posição relativa, orientação e velocidades
    /// </summary> 
    public class NavSendRelStatus
    {
        /// <summary>
        /// north: (double) longitude (rad)
        /// </summary>
        double north;
        /// <summary>
        /// east: (double) latitude (rad)
        /// </summary>
        double east;
        /// <summary>
        /// depth: (double) profundidade (m)
        /// </summary>
        double depth;
        /// <summary>
        /// roll: (double) ângulo de roll (rad)
        /// </summary>
        double roll;
        /// <summary>
        /// pitch: (double) ângulo de pitch (rad)
        /// </summary>
        double pitch;
        /// <summary>
        /// yaw: (double) ângulo de yaw (rad)
        /// </summary>
        double yaw;
        /// <summary>
        /// velnorth: double velocidade longitudinal (m/s)
        /// </summary>
        double velnorth;
        /// <summary>
        /// veleast: (double) velocidade transversal (m/s)
        /// </summary>
        double veleast;
        /// <summary>
        /// veldepth: (double) velocidade descendente (m/s)
        /// </summary>
        double veldepth;
        /// <summary>
        /// velroll: (double) velocidade angular segundo x (rad/s)
        /// </summary>
        double velroll;
        /// <summary>
        /// velpitch: (double) velocidade angular segundo y (rad/s)
        /// </summary>
        double velpitch;
        /// <summary>
        /// velyaw: (double) velocidade angular segundo z (rad/s)
        /// </summary>
        double velyaw;

        public double pnorth { get { return north; } }
        public double peast { get { return east; } }
        public double pdepth { get { return depth; } }
        public double proll { get { return roll; } }
        public double ppitch { get { return pitch; } }
        public double pyaw { get { return yaw; } }
        public double pvelnorth { get { return velnorth; } }
        public double pveleast { get { return veleast; } }
        public double pveldepth { get { return veldepth; } }
        public double pvelroll { get { return velroll; } }
        public double pvelpitch { get { return velpitch; } }
        public double pvelyaw { get { return velyaw; } }

        public NavSendRelStatus() { }

        public NavSendRelStatus(byte[] bytes)
        {
            byte[] b1 = new byte[8];
            byte[] b2 = new byte[8];
            byte[] b3 = new byte[8];
            byte[] b4 = new byte[8];
            byte[] b5 = new byte[8];
            byte[] b6 = new byte[8];
            byte[] b7 = new byte[8];
            byte[] b8 = new byte[8];
            byte[] b9 = new byte[8];
            byte[] b10 = new byte[8];
            byte[] b11 = new byte[8];
            byte[] b12 = new byte[8];

            Array.Copy(bytes, 0, b1, 0, 8);
            Array.Copy(bytes, 8, b2, 0, 8);
            Array.Copy(bytes, 16, b3, 0, 8);
            Array.Copy(bytes, 24, b4, 0, 8);
            Array.Copy(bytes, 32, b5, 0, 8);
            Array.Copy(bytes, 40, b6, 0, 8);
            Array.Copy(bytes, 48, b7, 0, 8);
            Array.Copy(bytes, 56, b8, 0, 8);
            Array.Copy(bytes, 64, b9, 0, 8);
            Array.Copy(bytes, 72, b10, 0, 8);
            Array.Copy(bytes, 80, b11, 0, 8);
            Array.Copy(bytes, 88, b12, 0, 8);

            north = BitConverter.ToDouble(b1, 0);
            east = BitConverter.ToDouble(b2, 0);
            depth = BitConverter.ToDouble(b3, 0);
            roll = BitConverter.ToDouble(b4, 0);
            pitch = BitConverter.ToDouble(b5, 0);
            yaw = BitConverter.ToDouble(b6, 0);
            velnorth = BitConverter.ToDouble(b7, 0);
            veleast = BitConverter.ToDouble(b8, 0);
            veldepth = BitConverter.ToDouble(b9, 0);
            velroll = BitConverter.ToDouble(b10, 0);
            velpitch = BitConverter.ToDouble(b11, 0);
            velyaw = BitConverter.ToDouble(b12, 0);
        }

    }

    /// <summary>
    /// CPS_SEND (#126): Ângulos de Euler e magnetômetros
    /// </summary>
    public class CpsSend
    {
        /// <summary>
        /// temperature: temperatura (graus Celsius)
        /// </summary>
        double temperature;
        /// <summary>
        /// yaw: ângulo de yaw (rad)
        /// </summary>
        double yaw;
        /// <summary>
        /// pitch: ângulo de pitch (rad)
        /// </summary>
        double pitch;
        /// <summary>
        /// ângulo de roll (rad)
        /// </summary>
        double roll;
        /// <summary>
        /// Bx: componente x do campo magnético ()
        /// </summary>
        double Bx;
        /// <summary>
        /// By: componente y do campo magnético ()
        /// </summary>
        double By;
        /// <summary>
        /// Bz: componente z do campo magnético ()
        /// </summary>
        double Bz;
        /// <summary>
        /// yawstatus: se diferente de 0 indica erro na medida
        /// </summary>
        short yawstatus;
        /// <summary>
        /// pitchstatus: se diferente de 0 indica erro na medida
        /// </summary>
        short pitchstatus;
        /// <summary>
        /// rollstatus: se diferente de 0 indica erro na medida
        /// </summary>
        short rollstatus;

        public double ptemperature { get { return temperature; } }
        public double pyaw { get { return yaw; } }
        public double ppitch { get { return pitch; } }
        public double proll { get { return roll; } }
        public double pBx { get { return Bx; } }
        public double pBy { get { return By; } }
        public double pBz { get { return Bz; } }
        public short pyawstatus { get { return yawstatus; } }
        public short ppitchstatus { get { return pitchstatus; } }
        public short prollstatus { get { return rollstatus; } }

        public CpsSend() { }

        public CpsSend(byte[] bytes)
        {
            byte[] b1 = new byte[8];
            byte[] b2 = new byte[8];
            byte[] b3 = new byte[8];
            byte[] b4 = new byte[8];
            byte[] b5 = new byte[8];
            byte[] b6 = new byte[8];
            byte[] b7 = new byte[8];
            byte[] b8 = new byte[2];
            byte[] b9 = new byte[2];
            byte[] b10 = new byte[2];

            Array.Copy(bytes, 0, b1, 0, 8);
            Array.Copy(bytes, 8, b2, 0, 8);
            Array.Copy(bytes, 16, b3, 0, 8);
            Array.Copy(bytes, 24, b4, 0, 8);
            Array.Copy(bytes, 32, b5, 0, 8);
            Array.Copy(bytes, 40, b6, 0, 8);
            Array.Copy(bytes, 48, b7, 0, 8);
            Array.Copy(bytes, 56, b8, 0, 2);
            Array.Copy(bytes, 58, b9, 0, 2);
            Array.Copy(bytes, 60, b10, 0, 2);

            temperature = BitConverter.ToDouble(b1, 0);
            yaw = BitConverter.ToDouble(b2, 0);
            pitch = BitConverter.ToDouble(b3, 0);
            roll = BitConverter.ToDouble(b4, 0);
            Bx = BitConverter.ToDouble(b5, 0);
            By = BitConverter.ToDouble(b6, 0);
            Bz = BitConverter.ToDouble(b7, 0);
            yawstatus = BitConverter.ToInt16(b8, 0);
            pitchstatus = BitConverter.ToInt16(b9, 0);
            rollstatus = BitConverter.ToInt16(b10, 0);
        }
    }

    /// <summary>
    /// CPS_MTI (#127): Dados de um sistema inercial
    /// </summary>
    public class CpsMti
    {
        /// <summary>
        /// componente 0 do quaternião
        /// </summary>
        double q0;
        /// <summary>
        /// componente 1 do quaternião
        /// </summary>
        double q1;
        /// <summary>
        /// componente 2 do quaternião
        /// </summary>
        double q2;
        /// <summary>
        /// componente 3 do quaternião
        /// </summary>
        double q3;
        /// <summary>
        /// velocidade angular em x (rad/s)
        /// </summary>
        double gyrX;
        /// <summary>
        /// velocidade angular em y (rad/s)
        /// </summary>
        double gyrY;
        /// <summary>
        /// velocidade angular em z (rad/s)
        /// </summary>
        double gyrZ;
        /// <summary>
        /// aceleração em x (m/s2)
        /// </summary>
        double accX;
        /// <summary>
        /// aceleração em y (m/s2)
        /// </summary>
        double accY;
        /// <summary>
        /// aceleração em z (m/s2)
        /// </summary>
        double accZ;

        public double pq0 { get { return q0; } }
        public double pq1 { get { return q1; } }
        public double pq2 { get { return q2; } }
        public double pq3 { get { return q3; } }
        public double pgyrX { get { return gyrX; } }
        public double pgyrY { get { return gyrY; } }
        public double pgyrZ { get { return gyrZ; } }
        public double paccX { get { return accX; } }
        public double paccY { get { return accY; } }
        public double paccZ { get { return accZ; } }

        public CpsMti() { }

        public CpsMti(byte[] bytes)
        {
            byte[] bq0 = new byte[8];
            byte[] bq1 = new byte[8];
            byte[] bq2 = new byte[8];
            byte[] bq3 = new byte[8];
            byte[] bgyrX = new byte[8];
            byte[] bgyrY = new byte[8];
            byte[] bgyrZ = new byte[8];
            byte[] bpaccX = new byte[8];
            byte[] bpaccY = new byte[8];
            byte[] bpaccZ = new byte[8];

            Array.Copy(bytes, 0, bq0, 0, 8);
            Array.Copy(bytes, 8, bq1, 0, 8);
            Array.Copy(bytes, 16, bq2, 0, 8);
            Array.Copy(bytes, 24, bq3, 0, 8);
            Array.Copy(bytes, 32, bgyrX, 0, 8);
            Array.Copy(bytes, 40, bgyrY, 0, 8);
            Array.Copy(bytes, 48, bgyrZ, 0, 8);
            Array.Copy(bytes, 56, bpaccX, 0, 8);
            Array.Copy(bytes, 64, bpaccY, 0, 8);
            Array.Copy(bytes, 72, bpaccZ, 0, 8);

            q0 = BitConverter.ToDouble(bq0, 0);
            q1 = BitConverter.ToDouble(bq1, 0);
            q2 = BitConverter.ToDouble(bq2, 0);
            q3 = BitConverter.ToDouble(bq3, 0);
            gyrX = BitConverter.ToDouble(bgyrX, 0);
            gyrY = BitConverter.ToDouble(bgyrY, 0);
            gyrZ = BitConverter.ToDouble(bgyrZ, 0);
            accX = BitConverter.ToDouble(bpaccX, 0);
            accY = BitConverter.ToDouble(bpaccY, 0);
            accZ = BitConverter.ToDouble(bpaccZ, 0);
        }
    }

    /// <summary>
    /// BATT_STATUS (#214): Estado das baterias
    /// </summary>
    public class BattStatus
    {
        /// <summary>
        /// current: (double) corrente total (A)
        /// </summary>
        double totalcurrent;
        /// <summary>
        /// voltage: (double) tensão média (V)
        /// </summary>
        double avgvoltage;
        /// <summary>
        /// stateofcharge: (uint32) carga disponível (%)
        /// </summary>
        uint stateofcharge;
        /// <summary>
        /// timetoempty: (uint32) tempo previsto de descarga total (min)
        /// </summary>
        uint timetoempty;
        /// <summary>
        /// timetofull: (uint32) tempo previsto para carga total (min)
        /// </summary>
        uint timetofull;
        /// <summary>
        /// temperature: (double) temperatura (graus Celsius)
        /// </summary>
        double temperature;

        public double ptotalcurrent { get { return totalcurrent; } }
        public double pavgvoltage { get { return avgvoltage; } }
        public uint pstateofcharge { get { return stateofcharge; } }
        public uint ptimetoempty { get { return timetoempty; } }
        public uint ptimetofull { get { return timetofull; } }
        public double ptemperature { get { return temperature; } }

        public BattStatus() { }

        public BattStatus(byte[] bytes)
        {
            byte[] btotalcurrent = new byte[8];
            byte[] bavgvoltage = new byte[8];
            byte[] bstateofcharge = new byte[4];
            byte[] btimetoempty = new byte[4];
            byte[] btimetofull = new byte[4];
            byte[] btemperature = new byte[8];

            Array.Copy(bytes, 0, btotalcurrent, 0, 8);
            Array.Copy(bytes, 8, bavgvoltage, 0, 8);
            Array.Copy(bytes, 16, bstateofcharge, 0, 4);
            Array.Copy(bytes, 20, btimetoempty, 0, 4);
            Array.Copy(bytes, 24, btimetofull, 0, 4);
            Array.Copy(bytes, 28, btemperature, 0, 8);

            totalcurrent = BitConverter.ToDouble(btotalcurrent, 0);
            avgvoltage = BitConverter.ToDouble(bavgvoltage, 0);
            stateofcharge = BitConverter.ToUInt32(bstateofcharge, 0);
            timetoempty = BitConverter.ToUInt32(btimetoempty, 0);
            timetofull = BitConverter.ToUInt32(btimetofull, 0);
            temperature = BitConverter.ToDouble(btemperature, 0);
        }
    }

    /// <summary>
    /// PRS_SEND (#83): Pressão
    /// </summary>
    public class PrsSend
    {
        /// <summary>
        /// error: (int16) se diferente de zero indica erro na medida
        /// </summary>
        short error;
        /// <summary>
        /// pressure: (double) pressão (dbar)
        /// </summary>
        double pressure;

        public short perror { get { return error; } }
        public double ppressure { get { return pressure; } }

        public PrsSend() { }

        public PrsSend(byte[] bytes)
        {
            byte[] berror = new byte[2];
            byte[] bpressure = new byte[8];

            Array.Copy(bytes, 0, berror, 0, 2);
            Array.Copy(bytes, 2, bpressure, 0, 8);

            error = BitConverter.ToInt16(berror, 0);
            pressure = BitConverter.ToDouble(bpressure, 0);

        }
    }

    /// <summary>
    /// GPS_SEND_3D (#57): 
    /// </summary>
    public class GPSSend3D
    {
        /// <summary>
        /// (segundos)
        /// </summary>
        double timeofweek;
        /// <summary>
        /// (radianos)
        /// </summary>
        double lat;
        /// <summary>
        /// (radianos)
        /// </summary>
        double lon;
        /// <summary>
        /// (metros)
        /// </summary>
        double alt;
        /// <summary>
        /// (metros/segundo)
        /// </summary>
        double vN;
        /// <summary>
        /// (metros/segundo)
        /// </summary>
        double vE;
        /// <summary>
        /// (metros/segundo)
        /// </summary>
        double vD;
        /// <summary>
        /// (metros)
        /// </summary>
        double hErr;
        /// <summary>
        /// (metros)
        /// </summary>
        double vErr;

        #region propriedades
        public double ptimeofweek { get { return timeofweek; } }
        public double plat { get { return lat; } }
        public double plon { get { return lon; } }
        public double palt { get { return alt; } }
        public double pvN { get { return vN; } }
        public double pvE { get { return vE; } }
        public double pvD { get { return vD; } }
        public double phErr { get { return hErr; } }
        public double pvErr { get { return vErr; } }
        #endregion

        public GPSSend3D() { }

        public GPSSend3D(byte[] bytes)
        {

            #region definicoes
            byte[] b1 = new byte[8];
            byte[] b2 = new byte[8];
            byte[] b3 = new byte[8];
            byte[] b4 = new byte[8];
            byte[] b5 = new byte[8];
            byte[] b6 = new byte[8];
            byte[] b7 = new byte[8];
            byte[] b8 = new byte[8];
            byte[] b9 = new byte[8];

            Array.Copy(bytes, 0, b1, 0, 8);
            Array.Copy(bytes, 8, b2, 0, 8);
            Array.Copy(bytes, 16, b3, 0, 8);
            Array.Copy(bytes, 24, b4, 0, 8);
            Array.Copy(bytes, 32, b5, 0, 8);
            Array.Copy(bytes, 40, b6, 0, 8);
            Array.Copy(bytes, 48, b7, 0, 8);
            Array.Copy(bytes, 56, b8, 0, 8);
            Array.Copy(bytes, 64, b9, 0, 8);
            #endregion

            timeofweek = BitConverter.ToDouble(b1, 0);
            lat = BitConverter.ToDouble(b2, 0);
            lon = BitConverter.ToDouble(b3, 0);
            alt = BitConverter.ToDouble(b4, 0);
            vN = BitConverter.ToDouble(b5, 0);
            vE = BitConverter.ToDouble(b6, 0);
            vD = BitConverter.ToDouble(b7, 0);
            hErr = BitConverter.ToDouble(b8, 0);
            vErr = BitConverter.ToDouble(b9, 0);
        }
    }

    /// <summary>
    /// NAV_SEND_ABS_STATUS (#72): Posição absoluta, orientação e velocidades
    /// </summary>
    public class NavSendAbsStatus
    {
        /// <summary>
        /// longitude (rad)
        /// </summary>
        double lon;
        /// <summary>
        /// latitude (rad)
        /// </summary>
        double lat;
        /// <summary>
        /// profundidade (m)
        /// </summary>
        double depth;
        /// <summary>
        /// ângulo de roll (rad)
        /// </summary>
        double roll;
        /// <summary>
        /// ângulo de pitch (rad)
        /// </summary>
        double pitch;
        /// <summary>
        /// ângulo de yaw (rad)
        /// </summary>
        double yaw;
        /// <summary>
        /// velocidade longitudinal (m/s)
        /// </summary>
        double velnorth;
        /// <summary>
        /// velocidade transversal (m/s)
        /// </summary>
        double veleast;
        /// <summary>
        /// velocidade descendente (m/s)
        /// </summary>
        double veldepth;
        /// <summary>
        /// velocidade angular segundo x (rad/s)
        /// </summary>
        double velroll;
        /// <summary>
        /// velocidade angular segundo y (rad/s)
        /// </summary>
        double velpitch;
        /// <summary>
        /// velocidade angular segundo z (rad/s)
        /// </summary>
        double velyaw;

        public double plon { get { return lon; } }
        public double plat { get { return lat; } }
        public double pdepth { get { return depth; } }
        public double proll { get { return roll; } }
        public double ppitch { get { return pitch; } }
        public double pyaw { get { return yaw; } }
        public double pvelnorth { get { return velnorth; } }
        public double pveleast { get { return veleast; } }
        public double pveldepth { get { return veldepth; } }
        public double pvelroll { get { return velroll; } }
        public double pvelpitch { get { return velpitch; } }
        public double pvelyaw { get { return velyaw; } }

        public NavSendAbsStatus() { }

        public NavSendAbsStatus(byte[] bytes)
        {
            byte[] b1 = new byte[8];
            byte[] b2 = new byte[8];
            byte[] b3 = new byte[8];
            byte[] b4 = new byte[8];
            byte[] b5 = new byte[8];
            byte[] b6 = new byte[8];
            byte[] b7 = new byte[8];
            byte[] b8 = new byte[8];
            byte[] b9 = new byte[8];
            byte[] b10 = new byte[8];
            byte[] b11 = new byte[8];
            byte[] b12 = new byte[8];

            Array.Copy(bytes, 0, b1, 0, 8);
            Array.Copy(bytes, 8, b2, 0, 8);
            Array.Copy(bytes, 16, b3, 0, 8);
            Array.Copy(bytes, 24, b4, 0, 8);
            Array.Copy(bytes, 32, b5, 0, 8);
            Array.Copy(bytes, 40, b6, 0, 8);
            Array.Copy(bytes, 48, b7, 0, 8);
            Array.Copy(bytes, 56, b8, 0, 8);
            Array.Copy(bytes, 64, b9, 0, 8);
            Array.Copy(bytes, 72, b10, 0, 8);
            Array.Copy(bytes, 80, b11, 0, 8);
            Array.Copy(bytes, 88, b12, 0, 8);

            lon = BitConverter.ToDouble(b1, 0);
            lat = BitConverter.ToDouble(b2, 0);
            depth = BitConverter.ToDouble(b3, 0);
            roll = BitConverter.ToDouble(b4, 0);
            pitch = BitConverter.ToDouble(b5, 0);
            yaw = BitConverter.ToDouble(b6, 0);
            velnorth = BitConverter.ToDouble(b7, 0);
            veleast = BitConverter.ToDouble(b8, 0);
            veldepth = BitConverter.ToDouble(b9, 0);
            velroll = BitConverter.ToDouble(b10, 0);
            velpitch = BitConverter.ToDouble(b11, 0);
            velyaw = BitConverter.ToDouble(b12, 0);
        }
    }

    /// <summary>
    /// ALTIMETER_DATA (#213)
    public class AltimeterData
    {
        /// <summary>
        /// Faixa
        /// </summary>
        int range;
        /// <summary>
        /// Largura do pulso
        /// </summary>
        int pulseLength;
        /// <summary>
        /// Ganho
        /// </summary>
        int gain;
        /// <summary>
        /// Profundidade
        /// </summary>
        double depth;

        public int prange { get { return range; } }
        public int ppulseLength { get { return pulseLength; } }
        public int pgain { get { return gain; } }
        public double pdepth { get { return depth; } }

        public AltimeterData() { }

        public AltimeterData(byte[] bytes)
        {
            byte[] b1 = new byte[4];
            byte[] b2 = new byte[4];
            byte[] b3 = new byte[4];
            byte[] b4 = new byte[8];

            Array.Copy(bytes, 0, b1, 0, 4);
            Array.Copy(bytes, 4, b2, 0, 4);
            Array.Copy(bytes, 8, b3, 0, 4);
            Array.Copy(bytes, 12, b4, 0, 8);

            range = BitConverter.ToInt32(b1, 0);
            pulseLength = BitConverter.ToInt32(b2, 0);
            gain = BitConverter.ToInt32(b3, 0);
            depth = BitConverter.ToDouble(b4, 0);
        }
    }

    /// <summary>
    /// CTD_SEND (#141): Payload CTD Sensor
    /// </summary>
    public class CtdSend
    {
        double temperature;
        double conductivity;
        double pressure;
        double salinity;
        double soundvelocity;

        public double ptemperature { get { return temperature; } }
        public double pconductivity { get { return conductivity; } }
        public double ppressure { get { return pressure; } }
        public double psalinity { get { return salinity; } }
        public double psoundvelocity { get { return soundvelocity; } }

        public CtdSend() { }

        public CtdSend(byte[] bytes)
        {
            byte[] b1 = new byte[8];
            byte[] b2 = new byte[8];
            byte[] b3 = new byte[8];
            byte[] b4 = new byte[8];
            byte[] b5 = new byte[8];

            Array.Copy(bytes, 0, b1, 0, 8);
            Array.Copy(bytes, 8, b2, 0, 8);
            Array.Copy(bytes, 16, b3, 0, 8);
            Array.Copy(bytes, 24, b4, 0, 8);
            Array.Copy(bytes, 32, b5, 0, 8);

            temperature = BitConverter.ToDouble(b1, 0);
            conductivity = BitConverter.ToDouble(b2, 0);
            pressure = BitConverter.ToDouble(b3, 0);
            salinity = BitConverter.ToDouble(b4, 0);
            soundvelocity = BitConverter.ToDouble(b5, 0);
        }
    }

    /// <summary>
    /// SUPERVISION_LEAK (#230)
    /// </summary>
    public class SupervisionLeak
    {
        /// <summary>
        /// user defined code for multiple leak sensors
        /// </summary>
        char leak_location;
        /// <summary>
        /// 0 = ok, 1 = water detected, 2 = broken cable)
        /// </summary>
        char leak_code;

        public char pleak_location { get { return leak_location; } }
        public char pleak_code { get { return leak_code; } }

        public SupervisionLeak() { }

        public SupervisionLeak(byte[] bytes)
        {
            byte[] b1 = new byte[2];
            byte[] b2 = new byte[2];

            Array.Copy(bytes, 0, b1, 0, 2);
            Array.Copy(bytes, 2, b2, 0, 2);

            leak_location = BitConverter.ToChar(b1, 0);
            leak_code = BitConverter.ToChar(b2, 0);
        }
    }

    /// <summary>
    /// PRS_SET_SURF (#80)
    /// </summary>
    public class tSurf
    {
        /// <summary>
        /// surfpressure (bar)
        /// </summary>
        double surfpressure;

        public double psurfpressure { get { return surfpressure; } }

        public tSurf() { }

        public tSurf(byte[] bytes)
        {
            byte[] b1 = new byte[8];

            Array.Copy(bytes, 0, b1, 0, 8);

            surfpressure = BitConverter.ToDouble(b1, 0);
        }
    }

    /// <summary>
    /// MOTION_REF (#154)
    /// </summary>
    public class MotionRef
    {
        /// <summary>
        /// Component along the x axis
        /// </summary>
        double x;
        /// <summary>
        /// Component along the y axis
        /// </summary>
        double y;
        /// <summary>
        /// Component along the z axis
        /// </summary>
        double z;
        double roll;
        double pitch;
        double yaw;
        /// <summary>
        /// Define the referential and the physical quantitie of the component x
        /// </summary>
        ushort x_qty;
        /// <summary>
        /// Define the referential and the physical quantitie of the component y
        /// </summary>
        ushort y_qty;
        /// <summary>
        /// Define the referential and the physical quantitie of the component z
        /// </summary>
        ushort z_qty;
        ushort roll_qty;
        ushort pitch_qty;
        ushort yaw_qty;

        public double px { get { return x; } }
        public double py { get { return y; } }
        public double pz { get { return z; } }
        public double proll { get { return roll; } }
        public double ppitch { get { return pitch; } }
        public double pyaw { get { return yaw; } }
        public ushort px_qty { get { return x_qty; } }
        public ushort py_qty { get { return y_qty; } }
        public ushort pz_qty { get { return z_qty; } }
        public ushort proll_qty { get { return roll_qty; } }
        public ushort ppitch_qty { get { return pitch_qty; } }
        public ushort pyaw_qty { get { return yaw_qty; } }

        public MotionRef() { }

        public MotionRef(byte[] bytes)
        {
            byte[] b1 = new byte[8];
            byte[] b2 = new byte[8];
            byte[] b3 = new byte[8];
            byte[] b4 = new byte[8];
            byte[] b5 = new byte[8];
            byte[] b6 = new byte[8];
            byte[] b7 = new byte[2];
            byte[] b8 = new byte[2];
            byte[] b9 = new byte[2];
            byte[] b10 = new byte[2];
            byte[] b11 = new byte[2];
            byte[] b12 = new byte[2];

            Array.Copy(bytes, 0, b1, 0, 8);
            Array.Copy(bytes, 8, b2, 0, 8);
            Array.Copy(bytes, 16, b3, 0, 8);
            Array.Copy(bytes, 24, b4, 0, 8);
            Array.Copy(bytes, 32, b5, 0, 8);
            Array.Copy(bytes, 40, b6, 0, 8);
            Array.Copy(bytes, 48, b7, 0, 2);
            Array.Copy(bytes, 50, b8, 0, 2);
            Array.Copy(bytes, 52, b9, 0, 2);
            Array.Copy(bytes, 54, b10, 0, 2);
            Array.Copy(bytes, 56, b11, 0, 2);
            Array.Copy(bytes, 58, b12, 0, 2);

            x = BitConverter.ToDouble(b1, 0);
            y = BitConverter.ToDouble(b2, 0);
            z = BitConverter.ToDouble(b3, 0);
            roll = BitConverter.ToDouble(b4, 0);
            pitch = BitConverter.ToDouble(b5, 0);
            yaw = BitConverter.ToDouble(b6, 0);
            x_qty = BitConverter.ToUInt16(b7, 0);
            y_qty = BitConverter.ToUInt16(b8, 0);
            z_qty = BitConverter.ToUInt16(b9, 0);
            roll_qty = BitConverter.ToUInt16(b10, 0);
            pitch_qty = BitConverter.ToUInt16(b11, 0);
            yaw_qty = BitConverter.ToUInt16(b12, 0);
        }
    };

    /// <summary>
    /// SYS_ACK (#1)
    /// </summary>
    public class SysAck
    {
        /// <summary>
        /// Acknowledge type
        /// </summary>
        ushort acktype;

        public ushort packtype { get { return acktype; } }

        public SysAck() { }

        public SysAck(byte[] bytes)
        {
            byte[] b1 = new byte[2];
            Array.Copy(bytes, 0, b1, 0, 2);
            acktype = BitConverter.ToUInt16(b1, 0);
        }
    }

    /// <summary>
    /// SYS_ALIVE (#4)
    /// </summary>
    public class SysAlive
    {
    }

    /// <summary>
    /// KLM_SET_POS (#130)
    /// Kalman filter group
    /// </summary>
    public class KlmSetPos
    {
        /// <summary>
        /// Norte
        /// </summary>
        double north;
        /// <summary>
        /// Leste
        /// </summary>
        double east;
        /// <summary>
        /// Covpos0
        /// </summary>
        double covpos0;
        /// <summary>
        /// Covw0
        /// </summary>
        double covw0;

        public double pnorth { get { return north; } }
        public double peast { get { return east; } }
        public double pcovpos0 { get { return covpos0; } }
        public double pcovw0 { get { return covw0; } }

        public KlmSetPos() { }

        public KlmSetPos(byte[] bytes)
        {
            byte[] b1 = new byte[8];
            byte[] b2 = new byte[8];
            byte[] b3 = new byte[8];
            byte[] b4 = new byte[8];

            Array.Copy(bytes, 0, b1, 0, 8);
            Array.Copy(bytes, 8, b2, 0, 8);
            Array.Copy(bytes, 16, b3, 0, 8);
            Array.Copy(bytes, 24, b4, 0, 8);

            north = BitConverter.ToDouble(b1, 0);
            east = BitConverter.ToDouble(b2, 0);
            covpos0 = BitConverter.ToDouble(b3, 0);
            covw0 = BitConverter.ToDouble(b4, 0);
        }
    }

    /// <summary>
    /// KLM_DEAD_RECK (#131)
    /// </summary>
    public class KlmDeadReck
    {
        /// <summary>
        /// hspeed
        /// </summary>
        double hspeed;
        /// <summary>
        /// Desvio (yaw)
        /// </summary>
        double yaw;

        public double phspeed { get { return hspeed; } }
        public double pyaw { get { return yaw; } }

        public KlmDeadReck() { }

        public KlmDeadReck(byte[] bytes)
        {
            byte[] b1 = new byte[8];
            byte[] b2 = new byte[8];

            Array.Copy(bytes, 0, b1, 0, 8);
            Array.Copy(bytes, 8, b2, 0, 8);

            hspeed = BitConverter.ToDouble(b1, 0);
            yaw = BitConverter.ToDouble(b2, 0);
        }
    }


    /// <summary>
    /// KLM_RNG_CRT (#132)
    /// </summary>
    public class KlmRngCrt
    {
        /// <summary>
        /// Índice
        /// </summary>
        ushort index;
        /// <summary>
        /// Faixa (range)
        /// </summary>
        double range;
        /// <summary>
        /// Profundidade (depth)
        /// </summary>
        double depth;
        /// <summary>
        /// Result
        /// </summary>
        short result;
        /// <summary>
        /// rangediff
        /// </summary>
        double rangediff;
        /// <summary>
        /// rangelevel
        /// </summary>
        double rangelevel;

        public ushort pindex { get { return index; } }
        public double prange { get { return range; } }
        public double pdepth { get { return depth; } }
        public short presult { get { return result; } }
        public double prangediff { get { return rangediff; } }
        public double prangelevel { get { return rangelevel; } }

        public KlmRngCrt() { }

        public KlmRngCrt(byte[] bytes)
        {
            byte[] b1 = new byte[2];
            byte[] b2 = new byte[8];
            byte[] b3 = new byte[8];
            byte[] b4 = new byte[2];
            byte[] b5 = new byte[8];
            byte[] b6 = new byte[8];

            Array.Copy(bytes, 0, b1, 0, 2);
            Array.Copy(bytes, 2, b2, 0, 8);
            Array.Copy(bytes, 10, b3, 0, 8);
            Array.Copy(bytes, 18, b4, 0, 2);
            Array.Copy(bytes, 20, b5, 0, 8);
            Array.Copy(bytes, 28, b6, 0, 8);

            index = BitConverter.ToUInt16(b1, 0);
            range = BitConverter.ToDouble(b2, 0);
            depth = BitConverter.ToDouble(b3, 0);
            result = BitConverter.ToInt16(b4, 0);
            rangediff = BitConverter.ToDouble(b5, 0);
            rangelevel = BitConverter.ToDouble(b6, 0);
        }
    }


    /// <summary>
    /// KLM_MOB_INITIATE (#135)
    /// </summary>
    public class KlmMobInitiate
    {
        /// <summary>
        /// x
        /// </summary>
        double x;
        /// <summary>
        /// y
        /// </summary>
        double y;
        /// <summary>
        /// alfa
        /// </summary>
        double alpha;
        /// <summary>
        /// baseline
        /// </summary>
        double baseline;
        /// <summary>
        /// covpos0
        /// </summary>
        double covpos0;
        /// <summary>
        /// covalpha0
        /// </summary>
        double covalpha0;
        /// <summary>
        /// covbaseline0
        /// </summary>
        double covbaseline0;

        public double px { get { return x; } }
        public double py { get { return y; } }
        public double palpha { get { return alpha; } }
        public double pbaseline { get { return baseline; } }
        public double pcovpos0 { get { return covpos0; } }
        public double pcovalpha0 { get { return covalpha0; } }
        public double pcovbaseline0 { get { return covbaseline0; } }

        public KlmMobInitiate() { }

        public KlmMobInitiate(byte[] bytes)
        {
            byte[] b1 = new byte[8];
            byte[] b2 = new byte[8];
            byte[] b3 = new byte[8];
            byte[] b4 = new byte[8];
            byte[] b5 = new byte[8];
            byte[] b6 = new byte[8];
            byte[] b7 = new byte[8];

            Array.Copy(bytes, 0, b1, 0, 8);
            Array.Copy(bytes, 8, b2, 0, 8);
            Array.Copy(bytes, 16, b3, 0, 8);
            Array.Copy(bytes, 24, b4, 0, 8);
            Array.Copy(bytes, 32, b5, 0, 8);
            Array.Copy(bytes, 40, b6, 0, 8);
            Array.Copy(bytes, 48, b7, 0, 8);

            x = BitConverter.ToDouble(b1, 0);
            y = BitConverter.ToDouble(b2, 0);
            alpha = BitConverter.ToDouble(b3, 0);
            baseline = BitConverter.ToDouble(b4, 0);
            covpos0 = BitConverter.ToDouble(b5, 0);
            covalpha0 = BitConverter.ToDouble(b6, 0);
            covbaseline0 = BitConverter.ToDouble(b7, 0);
        }
    }

    /// <summary>
    /// KLM_MOB_SET_POS (#133)
    /// </summary>
    public class KlmMobSetPos
    {
        /// <summary>
        /// x
        /// </summary>
        double x;
        /// <summary>
        /// y
        /// </summary>
        double y;
        /// <summary>
        /// covpos0
        /// </summary>
        double covpos0;

        public double px { get { return x; } }
        public double py { get { return y; } }
        public double pcovpos0 { get { return covpos0; } }

        public KlmMobSetPos() { }

        public KlmMobSetPos(byte[] bytes)
        {
            byte[] b1 = new byte[8];
            byte[] b2 = new byte[8];
            byte[] b3 = new byte[8];

            Array.Copy(bytes, 0, b1, 0, 8);
            Array.Copy(bytes, 8, b2, 0, 8);
            Array.Copy(bytes, 16, b3, 0, 8);

            x = BitConverter.ToDouble(b1, 0);
            y = BitConverter.ToDouble(b2, 0);
            covpos0 = BitConverter.ToDouble(b3, 0);
        }
    }

    /// <summary>
    /// KLM_MOB_SET_NETWORK (#134)
    /// </summary>
    public class KlmMobSetNetwork
    {
        /// <summary>
        /// baseline
        /// </summary>
        double baseline;
        /// <summary>
        /// alpha
        /// </summary>
        double alpha;
        /// <summary>
        /// covbaseline0
        /// </summary>
        double covbaseline0;
        /// <summary>
        /// covalpha0
        /// </summary>
        double covalpha0;

        public double pbaseline { get { return baseline; } }
        public double palpha { get { return alpha; } }
        public double pcovbaseline0 { get { return covbaseline0; } }
        public double pcovalpha0 { get { return covalpha0; } }

        public KlmMobSetNetwork() { }

        public KlmMobSetNetwork(byte[] bytes)
        {
            byte[] b1 = new byte[8];
            byte[] b2 = new byte[8];
            byte[] b3 = new byte[8];
            byte[] b4 = new byte[8];

            Array.Copy(bytes, 0, b1, 0, 8);
            Array.Copy(bytes, 8, b2, 0, 8);
            Array.Copy(bytes, 16, b3, 0, 8);
            Array.Copy(bytes, 24, b4, 0, 8);

            baseline = BitConverter.ToDouble(b1, 0);
            alpha = BitConverter.ToDouble(b2, 0);
            covbaseline0 = BitConverter.ToDouble(b3, 0);
            covalpha0 = BitConverter.ToDouble(b4, 0);
        }
    }

    // LBL_SET_BUOYPOS (#63)
    public class LBLSetBuoyPos
    {
        /// <summary>
        /// buoy identification
        /// </summary>
        char id;
        /// latitude (radianos)
        double lat;
        /// <summary>
        /// longitude (radianos)
        /// </summary>
        double lon;
        /// <summary>
        ///   // depth (m)
        /// </summary>
        double depth;

        public char pid { get { return id; } }
        public double plat { get { return lat; } }
        public double plon { get { return lon; } }
        public double pdepth { get { return depth; } }

        public LBLSetBuoyPos() { }

        public LBLSetBuoyPos(byte[] bytes)
        {
            byte[] b1 = new byte[2];
            byte[] b2 = new byte[8];
            byte[] b3 = new byte[8];
            byte[] b4 = new byte[8];

            Array.Copy(bytes, 0, b1, 0, 2);
            Array.Copy(bytes, 2, b2, 0, 8);
            Array.Copy(bytes, 10, b3, 0, 8);
            Array.Copy(bytes, 18, b4, 0, 8);

            id = BitConverter.ToChar(b1, 0);
            lat = BitConverter.ToDouble(b2, 0);
            lon = BitConverter.ToDouble(b3, 0);
            depth = BitConverter.ToDouble(b4, 0);
        }
    }


    // Falta corrigir:


    /// <summary>
    /// SYS_TEXT (#7)
    /// </summary>
    public class SysText
    {
        /// <summary>
        /// str (array de char tamanho 241)
        /// </summary>
        char[] str = new char[241];

        public char[] pstr { get { return str; } }

        public SysText() { }

        public SysText(byte[] bytes)
        {
            Convert.ToBase64CharArray(bytes, 0, bytes.Length, str, 0);
        }
    }


    /// <summary>
    /// MIS_START (#90)
    /// </summary>
    public class MisStart
    {
        char[] missionname = new char[241];

        public char[] pmissionname { get { return missionname; } }

        public MisStart() { }

        public MisStart(byte[] bytes)
        {
            Convert.ToBase64CharArray(bytes, 0, bytes.Length, missionname, 0);
        }
    }
}
