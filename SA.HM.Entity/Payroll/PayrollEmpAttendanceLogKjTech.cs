using System;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollEmpAttendanceLogKjTech
    {
        public int no = 0;
        public int vEnrollNumber = 0, vGranted = 0, vMethod = 0, vDoorMode = 0;
        public int vFunNumber = 0, vSensor = 0, vYear = 0, vMonth = 0, vDay = 0;
        public int vHour = 0, vMinute = 0, vSecond = 0;

        public int No
        {
            get { return no; }
        }

        public string Result
        {
            get { return vGranted == 1 ? "Granted" : "Denied"; }
        }

        public string ID
        {
            get { return vEnrollNumber == -1 ? "NONE" : String.Format("{0:D8}", vEnrollNumber); }
        }

        public string DoorMode
        {
            get
            {
                switch (vDoorMode)
                {
                    case 0: return "Any";
                    case 1: return "Finger";
                    case 2: return "CD or FP";
                    case 3: return "ID&FP or CD";
                    case 4: return "ID&FP or ID&CD";
                    case 5: return "ID&FP or CD&FP";
                    case 6: return "Open";
                    case 7: return "Close";
                    case 8: return "Card";
                    case 9: return "ID or FP";
                    case 10: return "ID or CD";
                    case 11: return "ID&CD";
                    case 12: return "CD&FP";
                    case 13: return "ID&FP";
                    case 14: return "ID&CD&FP";
                    default: return "Unknown";
                }
            }
        }

        public string Sensor
        {
            get
            {
                return vSensor == 1 ? "Open" : "Close";
            }
        }

        public string Function
        {
            get
            {
                if (vFunNumber == 40)
                    return "NONE";
                else
                    return "F" + (vFunNumber / 10 + 1) + "-" + (vFunNumber % 10);
            }
        }

        public string Method
        {
            get
            {
                string sMethod = "";
                int vmmode = vMethod & (Constants.GLOG_BY_ID | Constants.GLOG_BY_CD | Constants.GLOG_BY_FP);
                switch (vmmode)
                {
                    case 0: sMethod = "by CD2"; break;
                    case 1: sMethod = "by ID"; break;
                    case 2: sMethod = "by CD"; break;
                    case 3: sMethod = "by ID&CD"; break;
                    case 4: sMethod = "by FP"; break;
                    case 5: sMethod = "by ID&FP"; break;
                    case 6: sMethod = "by CD&FP"; break;
                    case 7: sMethod = "by ID&CD&FP"; break;
                }

                if ((vMethod & Constants.GLOG_BY_DURESS_BIT) == Constants.GLOG_BY_DURESS_BIT) sMethod = "[DURESS]";
                if ((vMethod & Constants.GLOG_BY_LIMITTIME) == Constants.GLOG_BY_LIMITTIME) sMethod = sMethod + " [LT]";
                if ((vMethod & Constants.GLOG_BY_ANTIPASS) == Constants.GLOG_BY_ANTIPASS) sMethod = sMethod + " [AP]";
                if ((vMethod & Constants.GLOG_BY_TIMEZONE) == Constants.GLOG_BY_TIMEZONE) sMethod = sMethod + " [TZ]";
                if ((vMethod & Constants.GLOG_BY_AREA) == Constants.GLOG_BY_AREA) sMethod = sMethod + " [FACE]";

                return sMethod;
            }
        }

        public string Time
        {
            get { return String.Format("{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}", vYear, vMonth, vDay, vHour, vMinute, vSecond & 0xFF); }
        }

        public bool CapturedPhoto
        {
            get { return ((vSecond >> 8) & 0xFF) == 1; }
        }
    }
}