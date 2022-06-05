using HotelManagement.Data;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Presentation.Website.Common.AttendanceDevice;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HotelManagement.Presentation.Website
{
    public partial class SyncronizeData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SynchronizeAttendanceForSuprima();
            SynchronizeHouseKeepingMorningDirtyHour();
            SynchronizeAttendanceForKjTech();
            SynchronizeAttendanceForActatek();
        }
        private void SynchronizeAttendanceForSuprima()
        {
            try
            {
                EmpAttendanceDA entityDA = new EmpAttendanceDA();
                bool status = entityDA.SynchronizeAttendance();
            }
            catch (Exception ex)
            {
                using (StreamWriter errorLog = new StreamWriter(Server.MapPath("~/DataSyncronizeScheduleTask/errorlog.txt"), true))
                {
                    errorLog.WriteLine(DateTime.Now.ToShortDateString() + "    " + ex.Message);
                }
            }

            Response.Write("Time is :" + DateTime.Now.ToString());
        }
        private void SynchronizeHouseKeepingMorningDirtyHour()
        {
            try
            {
                RoomNumberDA entityDA = new RoomNumberDA();
                bool status = entityDA.SynchronizeHouseKeepingMorningDirtyHour();
            }
            catch (Exception ex)
            {
                using (StreamWriter errorLog = new StreamWriter(Server.MapPath("~/DataSyncronizeScheduleTask/errorlog.txt"), true))
                {
                    errorLog.WriteLine(DateTime.Now.ToShortDateString() + "    " + ex.Message);
                }
            }

            Response.Write("Time is :" + DateTime.Now.ToString());
        }
        private void SynchronizeRoomInformation()
        {
            try
            {
                RoomNumberDA entityDA = new RoomNumberDA();
                bool status = entityDA.SynchronizeRoomInformationProcess();
            }
            catch (Exception ex)
            {
                using (StreamWriter errorLog = new StreamWriter(Server.MapPath("~/DataSyncronizeScheduleTask/errorlog.txt"), true))
                {
                    errorLog.WriteLine(DateTime.Now.ToShortDateString() + "    " + ex.Message);
                }
            }

            Response.Write("Time is :" + DateTime.Now.ToString());
        }

        private void SynchronizeAttendanceForKjTech()
        {
            try
            {
                AttendanceDeviceDA deviceDA = new AttendanceDeviceDA();
                List<AttendanceDeviceBO> deviceList = deviceDA.GetAllAttendanceDeviceInfo();

                foreach (var device in deviceList)
                {
                    if (device.DeviceType == "KjTech")
                    {
                        LoginKjTech(device.ReaderId, device.IP, device.PortNumber, Convert.ToInt32(device.Password));

                        int deviceId = int.Parse(device.ReaderId);
                        GetLogDataForKjTech(deviceId);
                    }
                }
                deviceDA.UpdateKjTechAttendenceInfoToPayrollEmpAttendance();

            }
            catch (Exception ex)
            {
                using (StreamWriter errorLog = new StreamWriter(Server.MapPath("~/DataSyncronizeScheduleTask/errorlog.txt"), true))
                {
                    errorLog.WriteLine(DateTime.Now.ToShortDateString() + "    " + ex.Message);
                }
            }

            Response.Write("Time is :" + DateTime.Now.ToString());
        }

        private void SynchronizeAttendanceForActatek()
        {
            try
            {
                AttendanceDeviceDA deviceDA = new AttendanceDeviceDA();
                List<AttendanceDeviceBO> deviceList = deviceDA.GetAllAttendanceDeviceInfo();

                foreach (var device in deviceList)
                {
                    if (device.DeviceType == "Actatek")
                    {
                        GetActatekLogData(device.IP, device.UserName, device.Password);
                    }
                }
                deviceDA.UpdateActatekAttendenceInfoToPayrollEmpAttendance();

            }
            catch (Exception ex)
            {
                using (StreamWriter errorLog = new StreamWriter(Server.MapPath("~/DataSyncronizeScheduleTask/errorlog.txt"), true))
                {
                    errorLog.WriteLine(DateTime.Now.ToShortDateString() + "    " + ex.Message);
                }
            }

            Response.Write("Time is :" + DateTime.Now.ToString());
        }

        private void LoginKjTech(string machineId, string ip, int portNumber, int password)
        {
            int deviceId = int.Parse(machineId);
            Program.GetSFC3KPC_DLL().ConnectTcpip(deviceId, ip, portNumber, password);
        }

        private void GetLogDataForKjTech(int machineId)
        {
            List<PayrollEmpAttendanceLogKjTech> glogList = new List<PayrollEmpAttendanceLogKjTech>();
            bool bRet;
            int i;

            if (!Program.EnableDevice(false, machineId))
            {
                return;
            }
            // should call "BeginLogTransaction" first, to prevent event-losing
            SendBeginEventTransaction(true);

            //Application.DoEvents();
            bRet = Program.GetSFC3KPC_DLL().ReadGeneralLogData(machineId);

            glogList.Clear();

            if (bRet)
            {
                //Do not call "DeleteReadGeneralLogData" function in case we use EventTransaction
                //if (chkReadAndDelete.Checked) bRet = Program.GetSFC3KPC_DLL().DeleteReadGeneralLogData(Program.machineId);
                bRet = Program.EnableDevice(true, machineId);

                //Application.DoEvents();

                i = 1;
                while (true)
                {
                    PayrollEmpAttendanceLogKjTech data = new PayrollEmpAttendanceLogKjTech();
                    bRet = Program.GetSFC3KPC_DLL().GetGeneralLogData(machineId,
                        ref data.vEnrollNumber,
                        ref data.vGranted,
                        ref data.vMethod,
                        ref data.vDoorMode,
                        ref data.vFunNumber,
                        ref data.vSensor,
                        ref data.vYear, ref data.vMonth, ref data.vDay, ref data.vHour, ref data.vMinute, ref data.vSecond);

                    if (!bRet) break;

                    data.no = i;
                    glogList.Add(data);
                    i = i + 1;
                }
                if (glogList.Count > 0)
                    SaveLogForKjTech(glogList);
                // Call "FinishEventTransaction" after we succeeded to read logs and save into database.
                Program.EnableDevice(false, machineId);
                SendFinishEventTransaction();
                Program.EnableDevice(true, machineId);
            }
            else
                bRet = Program.EnableDevice(true, machineId);
        }

        private bool SendBeginEventTransaction(bool bGLog)
        {
            string strXml;
            bool bRet;

            strXml = Program.GetXmlHeader("BeginEventTransaction");
            Program.GetSFC3KPC_DLL().XML_AddInt(ref strXml, "LogType", bGLog ? 1 : 2);
            bRet = Program.GetSFC3KPC_DLL().GeneralOperationXML(ref strXml);
            return bRet;
        }

        private void SendFinishEventTransaction()
        {
            string strXml;
            bool bRet;

            strXml = Program.GetXmlHeader("FinishEventTransaction");
            bRet = Program.GetSFC3KPC_DLL().GeneralOperationXML(ref strXml);
        }

        private void SaveLogForKjTech(List<PayrollEmpAttendanceLogKjTech> logList)
        {
            AttendanceDeviceDA empAttendanceDA = new AttendanceDeviceDA();
            empAttendanceDA.SaveAttendenceInfoForKjTech(logList);
        }

        private void StartReadGLogData(int machineId)
        {
            bool bRet;

            if (!Program.EnableDevice(false, machineId))
            {
                return;
            }

            bRet = Program.GetSFC3KPC_DLL().StartReadGeneralLogData(Program.machineId);

            bRet = Program.EnableDevice(true, machineId);
        }

        private void GetActatekLogData(string ip, string userName, string passWord)
        {
            Actatek.ACTAtek aCTAtek = new Actatek.ACTAtek();

            aCTAtek.Url = "http://" + ip + "/cgi-bin/rpcrouter";

            long sessionId = 0;

            try
            {
                sessionId = aCTAtek.login(userName, passWord);

                DateTime from = DateTime.Now.Date;
                DateTime to = DateTime.Now.AddDays(1).Date.AddSeconds(-1);
                Actatek.getLogsCriteria logsCriteria = new Actatek.getLogsCriteria()
                {
                    from = DateTime.Now.Date,
                    to = DateTime.Now.AddDays(1).Date.AddSeconds(-1),
                    toSpecified = true,
                    fromSpecified = true
                };

                Actatek.Log[] logData = aCTAtek.getLogs(sessionId, logsCriteria);
                //.Where(i => i.timestamp.Date >= from & i.timestamp.Date <= to).ToArray();
                List<LogDetailActaTec> logDetails = new List<LogDetailActaTec>();

                //CommonHelper.ConvertDateTimePropertiesUTCtoLocalTime(logDetails);

                if (logData != null)
                    logDetails = (from lg in logData
                                  select new LogDetailActaTec
                                  {
                                      logID = lg.logID,
                                      userID = lg.userID,
                                      //userName = lg.userName,
                                      //departmentName = lg.departmentName,
                                      //accessMethod = lg.accessMethod,
                                      terminalSN = lg.terminalSN,
                                      timestamp = lg.timestamp.ToLocalTime(),
                                      trigger = lg.trigger.ToString()
                                  }).ToList();

                AttendanceDeviceDA empAttendanceDA = new AttendanceDeviceDA();
                if (logDetails.Count > 0)
                    empAttendanceDA.SaveAttendenceInfoForActatek(logDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}