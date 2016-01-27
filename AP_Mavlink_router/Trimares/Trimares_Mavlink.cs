using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAVcomm;

namespace Trimares
{

                //propertyGrid1.SelectedObject = myTranslator.myGPSSend2D;
                //propertyGrid2.SelectedObject = myTranslator.myLBLSend2D;
                //propertyGrid3.SelectedObject = myTranslator.myNavSendRelStatus;
                //propertyGrid4.SelectedObject = myTranslator.myPrsSend;
                //propertyGrid5.SelectedObject = myTranslator.myCpsSend;
                //propertyGrid6.SelectedObject = myTranslator.myCpsMti;
                //propertyGrid7.SelectedObject = myTranslator.myBattStatus;
    public class Trimares_Mavlink
    {
        public static MAVLink.mavlink_sys_status_t to_mavlink_sys_status_t(BattStatus bStatus) 
        {
            MAVLink.mavlink_sys_status_t c = new MAVLink.mavlink_sys_status_t() 
            {
                current_battery = (short)bStatus.ptotalcurrent,
                voltage_battery = (ushort)bStatus.pavgvoltage,
                

            };
            return c;
        }
    }
}
