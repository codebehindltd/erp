using HotelManagement.Entity.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace HotelManagement.Data.Payroll
{
    public class EmpAttendanceDA : BaseService
    {
        public List<EmpAttendanceBO> GetAllAttendenceInfo()
        {
            List<EmpAttendanceBO> List = new List<EmpAttendanceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllAttendenceInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpAttendanceBO attendenceBO = new EmpAttendanceBO();
                                attendenceBO.Remark = reader["Remark"].ToString();
                                attendenceBO.AttendanceDate = Convert.ToDateTime(reader["AttendanceDate"].ToString());
                                attendenceBO.EntryTime = Convert.ToDateTime(reader["EntryTime"].ToString());
                                attendenceBO.ExitTime = Convert.ToDateTime(reader["ExitTime"].ToString());
                                attendenceBO.AttendanceId = Int32.Parse(reader["AttendanceId"].ToString());
                                attendenceBO.EmpId = Int32.Parse(reader["EmpId"].ToString());
                                attendenceBO.EmployeeName = reader["EmployeeName"].ToString();
                                List.Add(attendenceBO);
                            }
                        }
                    }
                }
            }
            return List;
        }
        public List<EmpAttendanceBO> GetAllAttendenceInfoByEmployeeIDForGridPaging(int employeeID, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<EmpAttendanceBO> lists = new List<EmpAttendanceBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllAttendenceInfoByEmployeeIDForGridPaging_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeID);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet LeaveDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "Leave");
                    DataTable Table = LeaveDS.Tables["Leave"];

                    lists = Table.AsEnumerable().Select(r => new EmpAttendanceBO
                    {
                        AttendanceId = r.Field<Int32>("AttendanceId"),
                        EmpId = r.Field<Int32>("EmpId"),
                        AttendanceDate = r.Field<DateTime>("AttendanceDate"),
                        EntryTime = r.Field<DateTime>("EntryTime"),
                        ExitTime = r.Field<DateTime?>("ExitTime"),
                        stringEntryTime = r.Field<string>("stringEntryTime"),
                        stringExitTime = r.Field<string>("stringExitTime"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        Remark = r.Field<string>("Remark")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return lists;
        }
        public EmpAttendanceBO GetEmpAttendenceInfoById(int EditId)
        {
            EmpAttendanceBO attendenceBO = new EmpAttendanceBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpAttendenceInfoById_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@AttendanceId", DbType.Int32, EditId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                attendenceBO.Remark = reader["Remark"].ToString();
                                attendenceBO.AttendanceDate = Convert.ToDateTime(reader["AttendanceDate"].ToString());
                                attendenceBO.EntryTime = Convert.ToDateTime(reader["EntryTime"].ToString());

                                if (!string.IsNullOrWhiteSpace(reader["ExitTime"].ToString()))
                                {
                                    attendenceBO.ExitTime = Convert.ToDateTime(reader["ExitTime"].ToString());
                                }

                                attendenceBO.AttendanceId = Int32.Parse(reader["AttendanceId"].ToString());
                                attendenceBO.EmpId = Int32.Parse(reader["EmpId"].ToString());
                                attendenceBO.EmployeeName = reader["EmployeeName"].ToString();
                                attendenceBO.EmpCode = reader["EmpCode"].ToString();
                                attendenceBO.AttendenceStatus = reader["AttendenceStatus"].ToString();
                                attendenceBO.Designation = reader["Designation"].ToString();
                                attendenceBO.Department = reader["Designation"].ToString();
                                attendenceBO.CancelReason = reader["CancelReason"].ToString();
                            }
                        }
                    }
                }
            }
            return attendenceBO;
        }
        public bool SaveEmpAttendenceInfo(EmpAttendanceBO attendenceBO, out int tmpUserInfoId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpAttendenceInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, attendenceBO.EmpId);

                        if (attendenceBO.EntryTime != null)
                            dbSmartAspects.AddInParameter(command, "@EntryTime", DbType.DateTime, attendenceBO.EntryTime);
                        else
                            dbSmartAspects.AddInParameter(command, "@EntryTime", DbType.DateTime, DBNull.Value);

                        if (attendenceBO.ExitTime != null)
                            dbSmartAspects.AddInParameter(command, "@ExitTime", DbType.DateTime, attendenceBO.ExitTime);
                        else
                            dbSmartAspects.AddInParameter(command, "@ExitTime", DbType.DateTime, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@AttendanceDate", DbType.DateTime, attendenceBO.AttendanceDate);
                        dbSmartAspects.AddInParameter(command, "@Remark", DbType.String, attendenceBO.Remark);
                        dbSmartAspects.AddInParameter(command, "@AttendenceStatus", DbType.String, attendenceBO.AttendenceStatus);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, attendenceBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@AttendanceId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpUserInfoId = Convert.ToInt32(command.Parameters["@AttendanceId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        public bool UpdateEmpAttendenceInfo(EmpAttendanceBO attendenceBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpAttendenceInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@AttendanceId", DbType.Int32, attendenceBO.AttendanceId);
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, attendenceBO.EmpId);
                        dbSmartAspects.AddInParameter(command, "@AttendenceStatus", DbType.String, attendenceBO.AttendenceStatus);

                        if (attendenceBO.EntryTime != null)
                            dbSmartAspects.AddInParameter(command, "@EntryTime", DbType.DateTime, attendenceBO.EntryTime);
                        else
                            dbSmartAspects.AddInParameter(command, "@EntryTime", DbType.DateTime, DBNull.Value);

                        if (attendenceBO.ExitTime != null)
                            dbSmartAspects.AddInParameter(command, "@ExitTime", DbType.DateTime, attendenceBO.ExitTime);
                        else
                            dbSmartAspects.AddInParameter(command, "@ExitTime", DbType.DateTime, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@AttendanceDate", DbType.DateTime, attendenceBO.AttendanceDate);
                        dbSmartAspects.AddInParameter(command, "@Remark", DbType.String, attendenceBO.Remark);
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
        public List<EmpAttendanceReportBO> GetEmpAttendanceInfoByDateRange(int isAdminUser, DateTime rosterDateFrom, DateTime rosterDateTo, int UserInfoId, int employeeId, string searchStatus, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<EmpAttendanceReportBO> empAttendanceList = new List<EmpAttendanceReportBO>();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpAttendanceInfoByDateRange"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@IsAdminUser", DbType.Int32, isAdminUser);
                    dbSmartAspects.AddInParameter(cmd, "@RosterDateFrom", DbType.DateTime, rosterDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@RosterDateTo", DbType.DateTime, rosterDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, UserInfoId);
                    if (employeeId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, employeeId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, DBNull.Value);
                    }

                    if (searchStatus != "")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AttendenceStatus", DbType.String, searchStatus);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AttendenceStatus", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet attendanceDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, attendanceDS, "EmployeeAttendance");

                    DataTable Table = attendanceDS.Tables["EmployeeAttendance"];

                    empAttendanceList = Table.AsEnumerable().Select(r => new EmpAttendanceReportBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        DisplayName = r.Field<string>("DisplayName"),
                        DepartmentId = r.Field<int>("DepartmentId"),
                        Department = r.Field<string>("Department"),
                        DesignationId = r.Field<int>("DesignationId"),
                        Designation = r.Field<string>("Designation"),
                        AttendanceStatus = r.Field<string>("AttendenceStatus"),
                        EntryTime = r.Field<DateTime>("EntryTime"),
                        ExitTime = r.Field<DateTime?>("ExitTime"),
                        SlabStartTime = r.Field<DateTime>("SlabStartTime"),
                        SlabEndTime = r.Field<DateTime>("SlabEndTime"),
                        AttendanceId = r.Field<int>("AttendanceId"),
                        AttendanceDate = r.Field<DateTime>("AttendanceDate"),
                        InTime = r.Field<string>("InTime"),
                        OutTime = r.Field<string>("OutTime"),
                        LateTime = r.Field<string>("LateTime"),
                        OTHour = r.Field<string>("OTHour"),
                        TimeSlabId = r.Field<int>("TimeSlabId"),
                        TimeSlabHead = r.Field<string>("TimeSlabHead"),
                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanCheck = r.Field<bool>("IsCanCheck"),
                        IsCanApprove = r.Field<bool>("IsCanApprove"),
                        RosterDate = r.Field<DateTime>("AttendanceDate")
                    }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return empAttendanceList;
        }
        public List<EmpAttendanceReportBO> GetEmpAttendanceReport(int reportType, int companyId, int projectId, DateTime rosterDateFrom, DateTime rosterDateTo, int employeeId, int departmentId, int workStationId, int timeSlab)
        {
            List<EmpAttendanceReportBO> empAttendanceList = new List<EmpAttendanceReportBO>();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeAttendanceListInAParticularDateForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.Int32, reportType);

                    if (companyId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                    }

                    if (projectId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RosterDateFrom", DbType.DateTime, rosterDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@RosterDateTo", DbType.DateTime, rosterDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, employeeId);
                    dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    if (employeeId == 0)
                    {
                        if (workStationId == 0)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@WorkStationId", DbType.Int32, DBNull.Value);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@WorkStationId", DbType.Int32, workStationId);
                        }
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@WorkStationId", DbType.Int32, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@TimeSlab", DbType.Int32, timeSlab);

                    DataSet attendanceDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, attendanceDS, "EmployeeAttendance");

                    DataTable Table = attendanceDS.Tables["EmployeeAttendance"];

                    empAttendanceList = Table.AsEnumerable().Select(r => new EmpAttendanceReportBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        DisplayName = r.Field<string>("DisplayName"),
                        DepartmentId = r.Field<int>("DepartmentId"),
                        Department = r.Field<string>("Department"),
                        DesignationId = r.Field<int>("DesignationId"),
                        Designation = r.Field<string>("Designation"),
                        AttendanceStatus = r.Field<string>("AttendanceStatus"),
                        EntryTime = r.Field<DateTime>("EntryTime"),
                        ExitTime = r.Field<DateTime?>("ExitTime"),
                        SlabStartTime = r.Field<DateTime>("SlabStartTime"),
                        SlabEndTime = r.Field<DateTime>("SlabEndTime"),
                        AttendanceDate = r.Field<DateTime>("AttendanceDate"),
                        InTime = r.Field<string>("InTime"),
                        OutTime = r.Field<string>("OutTime"),
                        WorkingHour = r.Field<string>("WorkingHour"),
                        LateTime = r.Field<string>("LateTime"),
                        OTHour = r.Field<string>("OTHour"),
                        TimeSlabId = r.Field<int>("TimeSlabId"),
                        TimeSlabHead = r.Field<string>("TimeSlabHead"),
                        RosterDate = r.Field<DateTime>("AttendanceDate"),
                        Remarks = r.Field<string>("Remarks"),
                        CancelReason = r.Field<string>("CancelReason"),
                        Description = r.Field<string>("Description")
                    }).ToList();
                }
            }

            return empAttendanceList;
        }

        public EmpAttendanceBO GetAttendenceByEmpCodeAndDate(string empCode, DateTime attendanceDate)
        {
            EmpAttendanceBO attendenceBO = new EmpAttendanceBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAttendenceByEmpCodeAndDate_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@EmpCode", DbType.String, empCode);
                    dbSmartAspects.AddInParameter(cmd, "@AttendanceDate", DbType.DateTime, attendanceDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                attendenceBO.Remark = reader["Remark"].ToString();
                                attendenceBO.AttendanceDate = Convert.ToDateTime(reader["AttendanceDate"].ToString());
                                attendenceBO.EntryTime = Convert.ToDateTime(reader["EntryTime"].ToString());
                                attendenceBO.ExitTime = Convert.ToDateTime(reader["ExitTime"].ToString());
                                attendenceBO.AttendanceId = Int32.Parse(reader["AttendanceId"].ToString());
                                attendenceBO.EmpId = Int32.Parse(reader["EmpId"].ToString());
                                attendenceBO.EmployeeName = reader["EmployeeName"].ToString();
                            }
                        }
                    }
                }
            }
            return attendenceBO;
        }
        public EmpAttendanceBO GetAttendenceByEmpIdAndDate(int EmpId, DateTime attendanceDate)
        {
            EmpAttendanceBO attendenceBO = new EmpAttendanceBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAttendenceByEmpIdAndDate_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, EmpId);
                    dbSmartAspects.AddInParameter(cmd, "@AttendanceDate", DbType.DateTime, attendanceDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                attendenceBO.AttendanceId = Int32.Parse(reader["AttendanceId"].ToString());
                                attendenceBO.EmpId = Int32.Parse(reader["EmpId"].ToString());
                                attendenceBO.EmployeeName = reader["EmployeeName"].ToString();
                                attendenceBO.AttendanceDate = Convert.ToDateTime(reader["AttendanceDate"].ToString());
                                attendenceBO.EntryTime = Convert.ToDateTime(reader["EntryTime"].ToString());
                                if (reader["ExitTime"].ToString() != "")
                                {
                                    attendenceBO.ExitTime = Convert.ToDateTime(reader["ExitTime"].ToString());
                                }

                                attendenceBO.Remark = reader["Remark"].ToString();
                                attendenceBO.AttendenceStatus = reader["AttendenceStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return attendenceBO;
        }
        public List<DailyEmpStatusViewBO> GetEmpAttendenceForDashboard()
        {
            List<DailyEmpStatusViewBO> viewList = new List<DailyEmpStatusViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeeAttendanceForDashboard_SP"))
                {
                    DataSet LeaveDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "EmpAttendance");
                    DataTable Table = LeaveDS.Tables["EmpAttendance"];

                    viewList = Table.AsEnumerable().Select(r => new DailyEmpStatusViewBO
                    {
                        NoOfEmp = r.Field<Int32>("NoOfEmp"),
                        Status = r.Field<string>("Status")
                    }).ToList();
                }
            }
            return viewList;
        }
        public List<EmpMonthlyAttendanceViewBO> GetEmpMonthlyAttendance(int empId)
        {
            List<EmpMonthlyAttendanceViewBO> viewList = new List<EmpMonthlyAttendanceViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMonthWiseEmpAttendance_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet LeaveDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "EmpAttendance");
                    DataTable Table = LeaveDS.Tables["EmpAttendance"];

                    viewList = Table.AsEnumerable().Select(r => new EmpMonthlyAttendanceViewBO
                    {
                        EmpId = r.Field<Int32>("EmpId"),
                        NoOfDays = r.Field<Int32>("NoOfDays"),
                        DisplayName = r.Field<string>("DisplayName"),
                        MonthName = r.Field<string>("MonthName"),
                    }).ToList();
                }
            }
            return viewList;
        }
        public List<EmpMonthlyAttendanceViewBO> GetEmpMonthlyAttendanceReport(int empId, int deptId, string processYear, string processMonth)
        {
            List<EmpMonthlyAttendanceViewBO> viewList = new List<EmpMonthlyAttendanceViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpMonthlyAttendance_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (empId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);
                    if (deptId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, deptId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.String, processYear);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessMonth", DbType.String, processMonth);

                    DataSet LeaveDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "EmpAttendance");
                    DataTable Table = LeaveDS.Tables["EmpAttendance"];

                    viewList = Table.AsEnumerable().Select(r => new EmpMonthlyAttendanceViewBO
                    {
                        //EmpId = r.Field<Int32>("EmpId"),
                        AttendanceDay = r.Field<Int32>("AttendanceDay"),
                        EmpCode = r.Field<string>("EmpCode"),
                        DisplayName = r.Field<string>("DisplayName"),
                        Department = r.Field<string>("Department"),
                        Designation = r.Field<string>("Designation")
                    }).ToList();
                }
            }
            return viewList;
        }
        public List<EmpMonthlyAttendanceViewBO> GetEmpMonthlyEntryReport(int companyId, int projectId, int empId, int departmentId, string processYear, string processMonth)
        {
            List<EmpMonthlyAttendanceViewBO> viewList = new List<EmpMonthlyAttendanceViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpMonthlyAttendanceForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (companyId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                    }

                    if (projectId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.String, processYear);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessMonth", DbType.String, processMonth);

                    DataSet LeaveDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "EmpEntry");
                    DataTable Table = LeaveDS.Tables["EmpEntry"];

                    viewList = Table.AsEnumerable().Select(r => new EmpMonthlyAttendanceViewBO
                    {
                        EmpId = r.Field<Int32>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        DisplayName = r.Field<string>("DisplayName"),
                        Day = r.Field<Int32>("Day"),
                        Entrytime = r.Field<string>("Entrytime"),
                        Exittime = r.Field<string>("Exittime")
                    }).ToList();
                }
            }
            return viewList;
        }
        public List<EmpMonthlyAttendanceViewBO> GetEmpMonthlyEntryDetailsReport(int companyId, int projectId, int empId, int departmentId, string processYear, string processMonth)
        {
            List<EmpMonthlyAttendanceViewBO> viewList = new List<EmpMonthlyAttendanceViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpMonthlyAttendanceDetailsForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (companyId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                    }

                    if (projectId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.String, processYear);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessMonth", DbType.String, processMonth);

                    DataSet LeaveDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "EmpEntryDetails");
                    DataTable Table = LeaveDS.Tables["EmpEntryDetails"];

                    viewList = Table.AsEnumerable().Select(r => new EmpMonthlyAttendanceViewBO
                    {
                        EmpId = r.Field<Int32>("EmpId"),
                        EmpCode = r.Field<String>("EmpCode"),
                        DisplayName = r.Field<string>("DisplayName"),
                        Day = r.Field<Int32>("Day"),
                        Entrytime = r.Field<string>("Entrytime"),
                        Exittime = r.Field<string>("Exittime"),
                        WorkingDaysThisMonth = r.Field<Int32>("WorkingDaysThisMonth"),
                        TotalPresentThisMonth = r.Field<Int32>("TotalPresentThisMonth"),
                        TotalLeaveThisMonth = r.Field<Int32>("TotalLeaveThisMonth"),
                        TotalHolidayThisMonth = r.Field<Int32>("TotalHolidayThisMonth"),
                        TotalDayOffThisMonth = r.Field<Int32>("TotalDayOffThisMonth"),
                        TotalAbsentThisMonth = r.Field<Int32>("TotalAbsentThisMonth"),
                        TotalDaysThisMOnth = r.Field<Int32>("TotalDaysThisMOnth")
                    }).ToList();
                }
            }
            return viewList;
        }
        public bool UpdateAttendanceStatus(EmpAttendanceBO attendenceBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateAttendanceStatus_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@AttendanceId", DbType.Int32, attendenceBO.AttendanceId);
                        dbSmartAspects.AddInParameter(command, "@AttendenceStatus", DbType.String, attendenceBO.AttendenceStatus);
                        dbSmartAspects.AddInParameter(command, "@CancelReason", DbType.String, attendenceBO.CancelReason);
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
        public List<EmpAttendanceBO> GetAttendenceByEmpIdAndDateRange(int empId, DateTime attendanceDateFrom, DateTime attendanceDateTo)
        {
            List<EmpAttendanceBO> attendence = new List<EmpAttendanceBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGetAttendenceByEmpIdAndDateRange_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(cmd, "@AttendanceDateFrom", DbType.DateTime, attendanceDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@AttendanceDateTo", DbType.DateTime, attendanceDateTo);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpAttendanceBO attendenceBO = new EmpAttendanceBO();

                                attendenceBO.Remark = reader["Remark"].ToString();
                                attendenceBO.AttendanceDate = Convert.ToDateTime(reader["AttendanceDate"].ToString());
                                attendenceBO.EntryTime = Convert.ToDateTime(reader["EntryTime"].ToString());
                                attendenceBO.ExitTime = Convert.ToDateTime(reader["ExitTime"].ToString());
                                attendenceBO.AttendanceId = Int32.Parse(reader["AttendanceId"].ToString());
                                attendenceBO.EmpId = Int32.Parse(reader["EmpId"].ToString());
                                attendenceBO.EmployeeName = reader["EmployeeName"].ToString();

                                attendence.Add(attendenceBO);
                            }
                        }
                    }
                }
            }
            return attendence;
        }

        //Attendance Synchronization
        public bool SynchronizeAttendance()
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("EmpAttendanceProcessInfo_SP"))
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

    }
}
