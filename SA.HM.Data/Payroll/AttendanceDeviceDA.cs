using HotelManagement.Entity.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace HotelManagement.Data.Payroll
{
    public class AttendanceDeviceDA : BaseService
    {
        public List<AttendanceDeviceBO> GetAllAttendanceDeviceInfo()
        {
            List<AttendanceDeviceBO> List = new List<AttendanceDeviceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllAttendanceDeviceInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                AttendanceDeviceBO attendenceBO = new AttendanceDeviceBO();
                                attendenceBO.ReaderId = reader["ReaderId"].ToString();
                                attendenceBO.ReaderType = reader["ReaderType"].ToString();
                                attendenceBO.DeviceType = reader["DeviceType"].ToString();
                                attendenceBO.IP = reader["IP"].ToString();
                                attendenceBO.Password = reader["Password"].ToString();
                                if (reader["PortNumber"] != DBNull.Value)
                                    attendenceBO.PortNumber = Int32.Parse(reader["PortNumber"].ToString());
                                attendenceBO.Name = reader["Name"].ToString();
                                attendenceBO.UserName = reader["UserName"].ToString();

                                List.Add(attendenceBO);
                            }
                        }
                    }
                }
            }
            return List;
        }
        public AttendanceDeviceBO GetAttendanceDeviceInfoById(string readerId)
        {
            AttendanceDeviceBO attendenceBO = new AttendanceDeviceBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAttendanceDeviceInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReaderId", DbType.String, readerId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                attendenceBO.ReaderId = reader["ReaderId"].ToString();
                                attendenceBO.ReaderType = reader["ReaderType"].ToString();
                                attendenceBO.Name = reader["Name"].ToString();
                            }
                        }
                    }
                }
            }
            return attendenceBO;
        }
        public bool UpdateAttendanceDeviceInfo(AttendanceDeviceBO attendenceBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateAttendanceDeviceInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ReaderId", DbType.String, attendenceBO.ReaderId);
                        dbSmartAspects.AddInParameter(command, "@ReaderType", DbType.String, attendenceBO.ReaderType);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, attendenceBO.LastModifiedBy);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        //public List<EmpAttendanceBO> GetAllAttendenceInfoByEmployeeIDForGridPaging(int employeeID, int recordPerPage, int pageIndex, out int totalRecords)
        //{
        //    List<EmpAttendanceBO> lists = new List<EmpAttendanceBO>();

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        conn.Open();

        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllAttendenceInfoByEmployeeIDForGridPaging_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeID);
        //            dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
        //            dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
        //            dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

        //            DataSet LeaveDS = new DataSet();
        //            dbSmartAspects.LoadDataSet(cmd, LeaveDS, "Leave");
        //            DataTable Table = LeaveDS.Tables["Leave"];

        //            lists = Table.AsEnumerable().Select(r => new EmpAttendanceBO
        //            {
        //                AttendanceId = r.Field<Int32>("AttendanceId"),
        //                EmpId = r.Field<Int32>("EmpId"),
        //                AttendanceDate = r.Field<DateTime>("AttendanceDate"),
        //                EntryTime = r.Field<DateTime>("EntryTime"),
        //                ExitTime = r.Field<DateTime>("ExitTime"),
        //                EmployeeName = r.Field<string>("EmployeeName"),
        //                Remark = r.Field<string>("Remark")
        //            }).ToList();

        //            totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
        //        }
        //    }

        //    return lists;
        //}
        //public EmpAttendanceBO GetEmpAttendenceInfoById(int EditId)
        //{
        //    EmpAttendanceBO attendenceBO = new EmpAttendanceBO();

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpAttendenceInfoById_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@AttendanceId", DbType.Int32, EditId);

        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        attendenceBO.Remark = reader["Remark"].ToString();
        //                        attendenceBO.AttendanceDate = Convert.ToDateTime(reader["AttendanceDate"].ToString());
        //                        attendenceBO.EntryTime = Convert.ToDateTime(reader["EntryTime"].ToString());
        //                        attendenceBO.ExitTime = Convert.ToDateTime(reader["ExitTime"].ToString());
        //                        attendenceBO.AttendanceId = Int32.Parse(reader["AttendanceId"].ToString());
        //                        attendenceBO.EmpId = Int32.Parse(reader["EmpId"].ToString());
        //                        attendenceBO.EmployeeName = reader["EmployeeName"].ToString();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return attendenceBO;
        //}
        //public bool SaveEmpAttendenceInfo(EmpAttendanceBO attendenceBO, out int tmpUserInfoId)
        //{
        //    Boolean status = false;
        //    try
        //    {
        //        using (DbConnection conn = dbSmartAspects.CreateConnection())
        //        {
        //            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpAttendenceInfo_SP"))
        //            {
        //                dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, attendenceBO.EmpId);
        //                dbSmartAspects.AddInParameter(command, "@EntryTime", DbType.DateTime, attendenceBO.EntryTime);
        //                dbSmartAspects.AddInParameter(command, "@ExitTime", DbType.DateTime, attendenceBO.ExitTime);
        //                dbSmartAspects.AddInParameter(command, "@AttendanceDate", DbType.DateTime, attendenceBO.AttendanceDate);
        //                dbSmartAspects.AddInParameter(command, "@Remark", DbType.String, attendenceBO.Remark);
        //                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, attendenceBO.CreatedBy);
        //                dbSmartAspects.AddOutParameter(command, "@AttendanceId", DbType.Int32, sizeof(Int32));
        //                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
        //                tmpUserInfoId = Convert.ToInt32(command.Parameters["@AttendanceId"].Value);
        //            }
        //        }
        //    }
        //    catch(Exception ex) {
        //        throw ex;
        //    }

        //    return status;
        //}
        //public bool UpdateEmpAttendenceInfo(EmpAttendanceBO attendenceBO)
        //{
        //    Boolean status = false;
        //    try
        //    {
        //        using (DbConnection conn = dbSmartAspects.CreateConnection())
        //        {
        //            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpAttendenceInfo_SP"))
        //            {
        //                dbSmartAspects.AddInParameter(command, "@AttendanceId", DbType.Int32, attendenceBO.AttendanceId);
        //                dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, attendenceBO.EmpId);
        //                dbSmartAspects.AddInParameter(command, "@EntryTime", DbType.DateTime, attendenceBO.EntryTime);
        //                dbSmartAspects.AddInParameter(command, "@ExitTime", DbType.DateTime, attendenceBO.ExitTime);
        //                dbSmartAspects.AddInParameter(command, "@AttendanceDate", DbType.DateTime, attendenceBO.AttendanceDate);
        //                dbSmartAspects.AddInParameter(command, "@Remark", DbType.String, attendenceBO.Remark);
        //                dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, attendenceBO.LastModifiedBy);
        //                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
        //            }
        //        }
        //    }
        //    catch (Exception ex) {
        //        throw ex;
        //    }
        //    return status;
        //}
        //public List<EmpAttendanceReportBO> GetEmpAttendanceReport(int reportType, DateTime rosterDateFrom, DateTime rosterDateTo, int employeeId, int departmentId, int timeSlab)
        //{
        //    List<EmpAttendanceReportBO> empAttendanceList = new List<EmpAttendanceReportBO>();

        //    using (DbConnection con = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeAttendanceListInAParticularDateForReport_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.Int32, reportType);
        //            dbSmartAspects.AddInParameter(cmd, "@RosterDateFrom", DbType.DateTime, rosterDateFrom);
        //            dbSmartAspects.AddInParameter(cmd, "@RosterDateTo", DbType.DateTime, rosterDateTo);
        //            dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, employeeId);
        //            dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
        //            dbSmartAspects.AddInParameter(cmd, "@TimeSlab", DbType.Int32, timeSlab);

        //            DataSet attendanceDS = new DataSet();
        //            dbSmartAspects.LoadDataSet(cmd, attendanceDS, "EmployeeAttendance");

        //            DataTable Table = attendanceDS.Tables["EmployeeAttendance"];

        //            empAttendanceList = Table.AsEnumerable().Select(r => new EmpAttendanceReportBO
        //            {
        //                EmpId = r.Field<int>("EmpId"),
        //                EmpCode = r.Field<string>("EmpCode"),
        //                DisplayName = r.Field<string>("DisplayName"),
        //                DepartmentId = r.Field<int>("DepartmentId"),
        //                Department = r.Field<string>("Department"),
        //                DesignationId = r.Field<int>("DesignationId"),
        //                Designation = r.Field<string>("Designation"),
        //                AttendanceStatus = r.Field<string>("Status"),
        //                EntryTime = r.Field<DateTime>("EntryTime"),
        //                ExitTime = r.Field<DateTime?>("ExitTime"),
        //                SlabStartTime = r.Field<DateTime>("SlabStartTime"),
        //                SlabEndTime = r.Field<DateTime>("SlabEndTime"),
        //                InTime = r.Field<string>("InTime"),
        //                OutTime = r.Field<string>("OutTime"),
        //                Late = r.Field<string>("Late"),
        //                OTHour = r.Field<string>("OTHour"),
        //                TimeSlabId = r.Field<int>("TimeSlabId"),
        //                TimeSlabHead = r.Field<string>("TimeSlabHead"),
        //                RosterDate = r.Field<DateTime>("AttendanceDate")
        //            }).ToList();
        //        }
        //    }

        //    return empAttendanceList;
        //}
        //public EmpAttendanceBO GetAttendenceByEmpCodeAndDate(string empCode, DateTime attendanceDate)
        //{
        //    EmpAttendanceBO attendenceBO = new EmpAttendanceBO();

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAttendenceByEmpCodeAndDate_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@EmpCode", DbType.String, empCode);
        //            dbSmartAspects.AddInParameter(cmd, "@AttendanceDate", DbType.DateTime, attendanceDate);

        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        attendenceBO.Remark = reader["Remark"].ToString();
        //                        attendenceBO.AttendanceDate = Convert.ToDateTime(reader["AttendanceDate"].ToString());
        //                        attendenceBO.EntryTime = Convert.ToDateTime(reader["EntryTime"].ToString());
        //                        attendenceBO.ExitTime = Convert.ToDateTime(reader["ExitTime"].ToString());
        //                        attendenceBO.AttendanceId = Int32.Parse(reader["AttendanceId"].ToString());
        //                        attendenceBO.EmpId = Int32.Parse(reader["EmpId"].ToString());
        //                        attendenceBO.EmployeeName = reader["EmployeeName"].ToString();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return attendenceBO;
        //}        

        //public List<EmpAttendanceBO> GetAllAttendenceInfoByEmployeeIDForGridPaging(int employeeID, int recordPerPage, int pageIndex, out int totalRecords)
        //{
        //    List<EmpAttendanceBO> lists = new List<EmpAttendanceBO>();

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        conn.Open();

        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllAttendenceInfoByEmployeeIDForGridPaging_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeID);
        //            dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
        //            dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
        //            dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

        //            DataSet LeaveDS = new DataSet();
        //            dbSmartAspects.LoadDataSet(cmd, LeaveDS, "Leave");
        //            DataTable Table = LeaveDS.Tables["Leave"];

        //            lists = Table.AsEnumerable().Select(r => new EmpAttendanceBO
        //            {
        //                AttendanceId = r.Field<Int32>("AttendanceId"),
        //                EmpId = r.Field<Int32>("EmpId"),
        //                AttendanceDate = r.Field<DateTime>("AttendanceDate"),
        //                EntryTime = r.Field<DateTime>("EntryTime"),
        //                ExitTime = r.Field<DateTime>("ExitTime"),
        //                EmployeeName = r.Field<string>("EmployeeName"),
        //                Remark = r.Field<string>("Remark")
        //            }).ToList();

        //            totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
        //        }
        //    }

        //    return lists;
        //}

        public List<AttendanceAndLeaveApprovalBO> GetAttendanceAndLeaveApproval(int empId, int monthId, int monthStartIndex, int monthEndedIndex, DateTime attendanceDate)
        {
            List<AttendanceAndLeaveApprovalBO> lists = new List<AttendanceAndLeaveApprovalBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("AttendanceAndLeaveApproval_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(cmd, "@MonthId", DbType.Int32, monthId);
                    dbSmartAspects.AddInParameter(cmd, "@MonthStartIndex", DbType.Int32, monthStartIndex);
                    dbSmartAspects.AddInParameter(cmd, "@MonthEndedIndex", DbType.Int32, monthEndedIndex);
                    dbSmartAspects.AddInParameter(cmd, "@AttendanceDate", DbType.DateTime, attendanceDate);

                    DataSet LeaveDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "Leave");
                    DataTable Table = LeaveDS.Tables["Leave"];

                    lists = Table.AsEnumerable().Select(r => new AttendanceAndLeaveApprovalBO
                    {
                        EmpId = r.Field<Int32?>("EmpId"),
                        MonthId = r.Field<Int32?>("MonthId"),
                        AttendanceId = r.Field<int?>("AttendanceId"),
                        AttendanceDate = r.Field<DateTime?>("AttendanceDate"),
                        InTime = r.Field<TimeSpan?>("InTime"),
                        OutTime = r.Field<TimeSpan?>("OutTime"),
                        LeaveId = r.Field<int?>("LeaveId"),
                        LeaveTypeId = r.Field<Int32?>("LeaveTypeId"),
                        LeaveType = r.Field<string>("LeaveType"),
                        LeaveMode = r.Field<string>("LeaveMode"),
                        Reamrks = r.Field<string>("Reamrks"),
                        AttendanceColor = r.Field<string>("AttendanceColor")

                    }).ToList();

                }

                return lists;
            }
        }

        public bool SaveAttendenceInfoForKjTech(List<PayrollEmpAttendanceLogKjTech> logList)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    foreach (var item in logList)
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveAttendenceInfoForKjTech_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@EnrollNumber", DbType.Int32, item.vEnrollNumber);
                            dbSmartAspects.AddInParameter(command, "@Granted", DbType.Int32, item.vGranted);
                            dbSmartAspects.AddInParameter(command, "@Method", DbType.Int32, item.vMethod);
                            dbSmartAspects.AddInParameter(command, "@DoorMode", DbType.Int32, item.vDoorMode);
                            dbSmartAspects.AddInParameter(command, "@FunNumber", DbType.Int32, item.vFunNumber);
                            dbSmartAspects.AddInParameter(command, "@Sensor", DbType.Int32, item.vSensor);
                            dbSmartAspects.AddInParameter(command, "@Year", DbType.Int32, item.vYear);
                            dbSmartAspects.AddInParameter(command, "@Month", DbType.Int32, item.vMonth);
                            dbSmartAspects.AddInParameter(command, "@Day", DbType.Int32, item.vDay);
                            dbSmartAspects.AddInParameter(command, "@Hour", DbType.Int32, item.vHour);
                            dbSmartAspects.AddInParameter(command, "@Minute", DbType.Int32, item.vMinute);
                            dbSmartAspects.AddInParameter(command, "@Second", DbType.Int32, item.vSecond);


                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        public bool UpdateKjTechAttendenceInfoToPayrollEmpAttendance()
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKjTechAttendenceInfoToPayrollEmpAttendance_SP"))
                    {
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }

        public bool UpdateActatekAttendenceInfoToPayrollEmpAttendance()
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateActatekAttendenceInfoToPayrollEmpAttendance_SP"))
                    {
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }

        public bool SaveAttendenceInfoForActatek(List<LogDetailActaTec> logList)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    foreach (var item in logList)
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveAttendenceInfoForActatek_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@logIDField", DbType.Int64, item.logID);
                            dbSmartAspects.AddInParameter(command, "@userIDField", DbType.String, item.userID);
                            dbSmartAspects.AddInParameter(command, "@userNameField", DbType.String, item.userName);
                            dbSmartAspects.AddInParameter(command, "@departmentNameField", DbType.String, item.departmentName);
                            dbSmartAspects.AddInParameter(command, "@accessMethodField", DbType.String, item.accessMethod);
                            dbSmartAspects.AddInParameter(command, "@terminalSNField", DbType.String, item.terminalSN);
                            dbSmartAspects.AddInParameter(command, "@timestampField", DbType.DateTime, item.timestamp);
                            dbSmartAspects.AddInParameter(command, "@triggerField", DbType.String, item.trigger);


                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        public bool SaveEmpInfoForAttendanceDevice(List<Device> Devices)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    foreach (var Device in Devices)
                    {
                        foreach (var Employee in Device.Employees)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpInfoForAttendanceDevice_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@DeviceId", DbType.Int64, Device.DeviceId);
                                dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, Employee.EmpId);
                                dbSmartAspects.AddInParameter(command, "@EmployeeName", DbType.String, Employee.EmployeeName);
                                dbSmartAspects.AddInParameter(command, "@EmpCode", DbType.String, Employee.EmpCode);


                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
    }
}
