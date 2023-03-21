using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class LeaveInformationDA : BaseService
    {
        public List<LeaveInformationBO> GetEmpLeaveInformationByEmpId(int empId)
        {
            List<LeaveInformationBO> leaveInformationList = new List<LeaveInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpLeaveInformationByEmpId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                LeaveInformationBO leaveInformation = new LeaveInformationBO();

                                leaveInformation.LeaveId = Convert.ToInt32(reader["LeaveId"]);
                                leaveInformation.EmpId = Convert.ToInt32(reader["EmpId"]);
                                leaveInformation.EmployeeName = reader["EmployeeName"].ToString();
                                leaveInformation.LeaveMode = reader["LeaveMode"].ToString();
                                leaveInformation.LeaveTypeId = Convert.ToInt32(reader["LeaveTypeId"]);
                                leaveInformation.TypeName = reader["TypeName"].ToString();
                                leaveInformation.FromDate = Convert.ToDateTime(reader["FromDate"]);
                                leaveInformation.ToDate = Convert.ToDateTime(reader["ToDate"]);
                                leaveInformation.NoOfDays = Convert.ToInt32(reader["NoOfDays"]);
                                leaveInformation.LeaveStatus = reader["LeaveStatus"].ToString();
                                leaveInformation.Reason = reader["Reason"].ToString();

                                leaveInformationList.Add(leaveInformation);
                            }
                        }
                    }
                }
            }
            return leaveInformationList;
        }
        public List<LeaveInformationBO> GetEmpLeaveInformationByEmpIdNDateRange(int empId, DateTime fromDate, DateTime toDate)
        {
            List<LeaveInformationBO> leaveInformationList = new List<LeaveInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpLeaveInformationByEmpIdNDateRange_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                LeaveInformationBO leaveInformation = new LeaveInformationBO();

                                leaveInformation.LeaveId = Convert.ToInt32(reader["LeaveId"]);
                                leaveInformation.EmpId = Convert.ToInt32(reader["EmpId"]);
                                leaveInformation.EmployeeName = reader["EmployeeName"].ToString();
                                leaveInformation.LeaveMode = reader["LeaveMode"].ToString();
                                leaveInformation.LeaveTypeId = Convert.ToInt32(reader["LeaveTypeId"]);
                                leaveInformation.TypeName = reader["TypeName"].ToString();
                                leaveInformation.FromDate = Convert.ToDateTime(reader["FromDate"]);
                                leaveInformation.ToDate = Convert.ToDateTime(reader["ToDate"]);
                                leaveInformation.NoOfDays = Convert.ToInt32(reader["NoOfDays"]);
                                leaveInformation.LeaveStatus = reader["LeaveStatus"].ToString();
                                leaveInformation.Reason = reader["Reason"].ToString();

                                leaveInformationList.Add(leaveInformation);
                            }
                        }
                    }
                }
            }
            return leaveInformationList;
        }
        public List<EmpOverTimeBO> GetPayrollInsteadLeaveForApproval(LeaveInformationBO leaveInfo)
        {
            List<EmpOverTimeBO> empOverTimeBOList = new List<EmpOverTimeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollInsteadLeaveForApproval_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, leaveInfo.EmpId);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, leaveInfo.FromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, leaveInfo.ToDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpOverTimeBO empOvertimeBO = new EmpOverTimeBO();

                                empOvertimeBO.OverTimeId = Convert.ToInt32(reader["OverTimeId"]);
                                empOvertimeBO.EmpId = Convert.ToInt32(reader["EmpId"]);
                                empOvertimeBO.OverTimeDate = Convert.ToDateTime(reader["OverTimeDate"]);
                                //empOvertimeBO.OverTimeHour = Convert.ToInt32(reader["OverTimeHour"]);
                                //empOvertimeBO.OverTimeStatusForIL = reader["OverTimeStatusForIL"].ToString();


                                empOverTimeBOList.Add(empOvertimeBO);
                            }
                        }
                    }
                }
            }
            return empOverTimeBOList;
        }
        public Boolean SaveEmpLeaveInformation(LeaveInformationBO leaveInformation, out int tmpLeaveId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpLeaveInformation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, leaveInformation.EmpId);
                        if (leaveInformation.LeaveMode == "0")
                        {
                            dbSmartAspects.AddInParameter(command, "@LeaveMode", DbType.String, DBNull.Value);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(command, "@LeaveMode", DbType.String, leaveInformation.LeaveMode);
                        }
                        dbSmartAspects.AddInParameter(command, "@LeaveTypeId", DbType.Int32, leaveInformation.LeaveTypeId);
                        dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, leaveInformation.FromDate);
                        dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, leaveInformation.ToDate);
                        dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, leaveInformation.TransactionType);
                        dbSmartAspects.AddInParameter(command, "@NoOfDays", DbType.Int32, leaveInformation.NoOfDays);
                        dbSmartAspects.AddInParameter(command, "@WorkHandover", DbType.Int32, leaveInformation.WorkHandover);

                        if (leaveInformation.TransactionType == "Addition")
                        {
                            dbSmartAspects.AddInParameter(command, "@ExpireDate", DbType.DateTime, leaveInformation.ExpireDate);
                        }

                        dbSmartAspects.AddInParameter(command, "@LeaveStatus", DbType.String, leaveInformation.LeaveStatus);
                        dbSmartAspects.AddInParameter(command, "@Reason", DbType.String, leaveInformation.Reason);
                        dbSmartAspects.AddInParameter(command, "@ReportingTo", DbType.String, leaveInformation.ReportingTo);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, leaveInformation.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@LeaveId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpLeaveId = Convert.ToInt32(command.Parameters["@LeaveId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean SaveAndUpdateEmpLeaveInformation(LeaveInformationBO leaveInformation, out int tmpLeaveId)
        {
            Boolean status = false;
            tmpLeaveId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveAndUpdateEmpLeaveInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, leaveInformation.EmpId);
                    dbSmartAspects.AddInParameter(command, "@LeaveMode", DbType.String, leaveInformation.LeaveMode);
                    dbSmartAspects.AddInParameter(command, "@LeaveTypeId", DbType.Int32, leaveInformation.LeaveTypeId);
                    dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, leaveInformation.FromDate);
                    dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, leaveInformation.ToDate);
                    dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, leaveInformation.TransactionType);
                    dbSmartAspects.AddInParameter(command, "@NoOfDays", DbType.Int32, leaveInformation.NoOfDays);
                    if (leaveInformation.TransactionType == "Addition")
                    {
                        dbSmartAspects.AddInParameter(command, "@ExpireDate", DbType.DateTime, leaveInformation.ExpireDate);
                    }
                    dbSmartAspects.AddInParameter(command, "@LeaveStatus", DbType.String, leaveInformation.LeaveStatus);
                    dbSmartAspects.AddInParameter(command, "@Reason", DbType.String, leaveInformation.Reason);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, leaveInformation.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@LeaveId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpLeaveId = Convert.ToInt32(command.Parameters["@LeaveId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateUserInformation(LeaveInformationBO leaveInformation)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpLeaveInformation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@LeaveId", DbType.Int32, leaveInformation.LeaveId);
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, leaveInformation.EmpId);
                        dbSmartAspects.AddInParameter(command, "@LeaveMode", DbType.String, leaveInformation.LeaveMode);
                        dbSmartAspects.AddInParameter(command, "@LeaveTypeId", DbType.Int32, leaveInformation.LeaveTypeId);
                        dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, leaveInformation.FromDate);
                        dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, leaveInformation.ToDate);
                        dbSmartAspects.AddInParameter(command, "@NoOfDays", DbType.Int32, leaveInformation.NoOfDays);
                        dbSmartAspects.AddInParameter(command, "@LeaveStatus", DbType.String, leaveInformation.LeaveStatus);
                        dbSmartAspects.AddInParameter(command, "@Reason", DbType.String, leaveInformation.Reason);
                        dbSmartAspects.AddInParameter(command, "@ReportingTo", DbType.Int32, leaveInformation.ReportingTo);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, leaveInformation.LastModifiedBy);

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
        public LeaveInformationBO GetEmpLeaveInformationById(int leaveId)
        {
            LeaveInformationBO leaveInformation = new LeaveInformationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpLeaveInformationById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LeaveId", DbType.Int32, leaveId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                leaveInformation.LeaveId = Convert.ToInt32(reader["LeaveId"]);
                                leaveInformation.EmpId = Convert.ToInt32(reader["EmpId"]);
                                leaveInformation.EmpCode = Convert.ToString(reader["EmpCode"]);
                                leaveInformation.EmployeeName = reader["EmployeeName"].ToString();
                                leaveInformation.LeaveMode = reader["LeaveMode"].ToString();
                                leaveInformation.LeaveTypeId = Convert.ToInt32(reader["LeaveTypeId"]);
                                leaveInformation.TypeName = reader["TypeName"].ToString();
                                leaveInformation.FromDate = Convert.ToDateTime(reader["FromDate"]);
                                leaveInformation.ToDate = Convert.ToDateTime(reader["ToDate"]);
                                leaveInformation.NoOfDays = Convert.ToInt32(reader["NoOfDays"]);
                                leaveInformation.LeaveStatus = reader["LeaveStatus"].ToString();
                                leaveInformation.Reason = reader["Reason"].ToString();
                                leaveInformation.Designation = reader["Designation"].ToString();
                                leaveInformation.DepartmentName = reader["Department"].ToString();
                                leaveInformation.ReportingToDesignation = reader["ReportingToDesignation"].ToString();
                                leaveInformation.ReportingTo = Convert.ToInt32(reader["ReportingTo"]);
                                leaveInformation.ShowFromDate = reader["ShowFromDate"].ToString();
                                leaveInformation.ShowToDate = reader["ShowToDate"].ToString();
                                leaveInformation.WorkHandover = Convert.ToInt32(reader["WorkHandover"]);
                                leaveInformation.WorkHandoverStatus = Convert.ToString(reader["WorkHandoverStatus"]);
                            }
                        }
                    }
                }
            }
            return leaveInformation;
        }
        public List<LeaveInformationBO> GetEmpLeaveWorkHandoverInformationByEmpIdForGridPaging(int isAdminUser, int userInfoId, int empId, string leaveStatus, string leaveType, string leaveMode, DateTime fromDate, DateTime toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<LeaveInformationBO> leaveInformationList = new List<LeaveInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpLeaveWorkHandoverInformationByEmpIdForGridPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@IsAdminUser", DbType.Int32, isAdminUser);
                    if (userInfoId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, userInfoId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, DBNull.Value);
                    if (empId == 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeID", DbType.Int32, DBNull.Value);
                    }
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeID", DbType.Int32, empId);

                    if (!string.IsNullOrEmpty(leaveType))
                        dbSmartAspects.AddInParameter(cmd, "@LeaveType", DbType.Int32, Convert.ToInt32(leaveType));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LeaveType", DbType.Int32, DBNull.Value);

                    if (leaveMode != "0")
                        dbSmartAspects.AddInParameter(cmd, "@LeaveMode", DbType.String, leaveMode);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LeaveMode", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@LeaveFromDate", DbType.DateTime, Convert.ToDateTime(fromDate));
                    dbSmartAspects.AddInParameter(cmd, "@LeaveToDate", DbType.DateTime, Convert.ToDateTime(toDate));

                    if (!string.IsNullOrEmpty(leaveStatus))
                        dbSmartAspects.AddInParameter(cmd, "@LeaveStatus", DbType.String, leaveStatus);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LeaveStatus", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet LeaveDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "Leave");
                    DataTable Table = LeaveDS.Tables["Leave"];

                    leaveInformationList = Table.AsEnumerable().Select(r => new LeaveInformationBO
                    {
                        LeaveId = r.Field<int>("LeaveId"),
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        LeaveMode = r.Field<string>("LeaveMode"),
                        LeaveTypeId = r.Field<int>("LeaveTypeId"),
                        TypeName = r.Field<string>("TypeName"),
                        FromDate = r.Field<DateTime>("FromDate"),
                        ToDate = r.Field<DateTime>("ToDate"),
                        NoOfDays = r.Field<int>("NoOfDays"),
                        LeaveStatus = r.Field<string>("LeaveStatus"),
                        //IsCanEdit = r.Field<bool>("IsCanEdit"),
                        //IsCanCheck = r.Field<bool>("IsCanCheck"),
                        //IsCanApprove = r.Field<bool>("IsCanApprove"),
                        Reason = r.Field<string>("Reason"),
                        WorkHandover = r.Field<int>("WorkHandover"),
                        WorkHandoverStatus = r.Field<string>("WorkHandoverStatus")

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return leaveInformationList;
        }
        public List<LeaveInformationBO> GetEmpLeaveInformationByEmpIdForGridPaging(int isAdminUser, int userInfoId, int empId, string leaveStatus, string leaveType, string leaveMode, DateTime fromDate, DateTime toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<LeaveInformationBO> leaveInformationList = new List<LeaveInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpLeaveInformationByEmpIdForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@IsAdminUser", DbType.Int32, isAdminUser);
                    if (userInfoId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, userInfoId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, DBNull.Value);
                    if (empId == 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeID", DbType.Int32, DBNull.Value);
                    }
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeID", DbType.Int32, empId);

                    if (!string.IsNullOrEmpty(leaveType))
                        dbSmartAspects.AddInParameter(cmd, "@LeaveType", DbType.Int32, Convert.ToInt32(leaveType));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LeaveType", DbType.Int32, DBNull.Value);

                    if (leaveMode != "0")
                        dbSmartAspects.AddInParameter(cmd, "@LeaveMode", DbType.String, leaveMode);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LeaveMode", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@LeaveFromDate", DbType.DateTime, Convert.ToDateTime(fromDate));
                    dbSmartAspects.AddInParameter(cmd, "@LeaveToDate", DbType.DateTime, Convert.ToDateTime(toDate));

                    if (!string.IsNullOrEmpty(leaveStatus))
                        dbSmartAspects.AddInParameter(cmd, "@LeaveStatus", DbType.String, leaveStatus);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LeaveStatus", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet LeaveDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "Leave");
                    DataTable Table = LeaveDS.Tables["Leave"];

                    leaveInformationList = Table.AsEnumerable().Select(r => new LeaveInformationBO
                    {
                        LeaveId = r.Field<int>("LeaveId"),
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        LeaveMode = r.Field<string>("LeaveMode"),
                        LeaveTypeId = r.Field<int>("LeaveTypeId"),
                        TypeName = r.Field<string>("TypeName"),
                        CreatedDateString = r.Field<string>("CreatedDateString"),
                        FromDate = r.Field<DateTime>("FromDate"),
                        ToDate = r.Field<DateTime>("ToDate"),
                        NoOfDays = r.Field<int>("NoOfDays"),
                        LeaveStatus = r.Field<string>("LeaveStatus"),
                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanCheck = r.Field<bool>("IsCanCheck"),
                        IsCanApprove = r.Field<bool>("IsCanApprove"),
                        Reason = r.Field<string>("Reason"),
                        WorkHandover = r.Field<int>("WorkHandover"),
                        WorkHandoverStatus = r.Field<string>("WorkHandoverStatus")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return leaveInformationList;
        }
        public bool UpdateEmpLeaveStatus(LeaveInformationBO leaveInformation)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpLeaveStatus_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@LeaveId", DbType.Int32, leaveInformation.LeaveId);
                        dbSmartAspects.AddInParameter(command, "@LeaveStatus", DbType.String, leaveInformation.LeaveStatus);
                        dbSmartAspects.AddInParameter(command, "@CancelReason", DbType.String, leaveInformation.CancelReason);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, leaveInformation.LastModifiedBy);

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
        public List<LeaveInformationBO> GetEmpLeaveInformationReport(int companyId, int projectId, string empId, string leaveType, string departmentId)
        {
            List<LeaveInformationBO> leaveInformationList = new List<LeaveInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpLeaveInformationReport_SP"))
                {
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

                    if (!string.IsNullOrEmpty(empId))
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeID", DbType.Int32, Convert.ToInt32(empId));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeID", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(leaveType))
                        dbSmartAspects.AddInParameter(cmd, "@LeaveType", DbType.Int32, Convert.ToInt32(leaveType));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LeaveType", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(departmentId))
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    DataSet LeaveDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "Leave");
                    DataTable Table = LeaveDS.Tables["Leave"];

                    leaveInformationList = Table.AsEnumerable().Select(r => new LeaveInformationBO
                    {
                        LeaveId = r.Field<int>("LeaveId"),
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        LeaveMode = r.Field<string>("LeaveMode"),
                        LeaveTypeId = r.Field<int>("LeaveTypeId"),
                        TypeName = r.Field<string>("TypeName"),
                        FromDate = r.Field<DateTime>("FromDate"),
                        ToDate = r.Field<DateTime>("ToDate"),
                        NoOfDays = r.Field<int>("NoOfDays"),
                        LeaveStatus = r.Field<string>("LeaveStatus"),
                        Reason = r.Field<string>("Reason"),

                        DepartmentId = r.Field<int>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName")

                    }).ToList();
                }
            }
            return leaveInformationList;
        }

        public List<LeaveInformationBO> GetLeaveDetailsYearlyReport(int yearId, int departmentId)
        {
            List<LeaveInformationBO> leaveInformationList = new List<LeaveInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLeaveDetailsByEmployeeNYear_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@YearId", DbType.Int32, yearId);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    DataSet LeaveDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "Leave");
                    DataTable Table = LeaveDS.Tables["Leave"];

                    leaveInformationList = Table.AsEnumerable().Select(r => new LeaveInformationBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        JoinDate = r.Field<DateTime?>("JoinDate"),
                        DepartmentId = r.Field<int>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        DesignationId = r.Field<int>("DesignationId"),
                        Designation = r.Field<string>("Designation"),

                        LeaveTypeId = r.Field<int>("LeaveTypeId"),
                        TypeName = r.Field<string>("TypeName"),
                        YearlyLeave = r.Field<int?>("YearlyLeave"),
                        MonthId = r.Field<int?>("MonthId"),
                        LeaveMonth = r.Field<string>("LeaveMonth"),
                        LeaveTaken = r.Field<int?>("LeaveTaken"),
                        TotalLeaveTaken = r.Field<int?>("TotalLeaveTaken"),
                        LeaveBalance = r.Field<int?>("LeaveBalance")

                    }).ToList();
                }
            }
            return leaveInformationList;
        }


        //Leave Balance Closing Report
        public List<LeaveBalanceApproveViewBO> GetLeaveBalanceCloseForEmployee(int empId)
        {
            List<LeaveBalanceApproveViewBO> viewlist = new List<LeaveBalanceApproveViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLeaveBalanceCloseForEmployee_SP"))
                {
                    if (empId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "LeaveBalanceClosing");
                    DataTable Table = ds.Tables["LeaveBalanceClosing"];

                    viewlist = Table.AsEnumerable().Select(r => new LeaveBalanceApproveViewBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        Employeename = r.Field<string>("Employeename"),
                        LeaveTypeId = r.Field<int>("LeaveTypeId"),
                        LeaveType = r.Field<string>("LeaveType"),
                        YearlyLeave = r.Field<int>("YearlyLeave"),
                        LeaveTaken = r.Field<int>("LeaveTaken"),
                        RemainingLeave = r.Field<int>("RemainingLeave"),
                        CanCarryForward = r.Field<bool>("CanCarryForward"),
                        MaxDayCanKeepAsCarryForwardLeave = r.Field<byte>("MaxDayCanKeepAsCarryForwardLeave"),
                        MaxDayCanCarryForwardYearly = r.Field<byte>("MaxDayCanCarryForwardYearly"),
                        CarryForwardedLeave = r.Field<int>("CarryForwardedLeave"),
                        CanEncash = r.Field<bool>("CanEncash"),
                        MaxDayCanEncash = r.Field<byte>("MaxDayCanEncash"),
                        TotalCarryforwardLeave = r.Field<int>("TotalCarryforwardLeave")

                        //EmpId EmpCode Employeename LeaveTypeId LeaveType YearlyLeave LeaveTaken 
                        //RemainingLeave  CanCarryForward MaxDayCanKeepAsCarryForwardLeave    MaxDayCanCarryForwardYearly 
                        //CarryForwardedLeave CanEncash MaxDayCanEncash TotalCarryforwardLeave


                    }).ToList();
                }
            }
            return viewlist;

        }
        public Boolean SaveOpeningLeave(List<LeaveBalanceClosingBO> addList, int createdBy)
        {
            Boolean status = false;
            int count = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOpeningLeaveInfo_SP"))
                    {
                        foreach (LeaveBalanceClosingBO bo in addList)
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int64, bo.EmpId);
                            dbSmartAspects.AddInParameter(command, "@LeaveTypeId", DbType.Int32, bo.LeaveTypeId);
                            dbSmartAspects.AddInParameter(command, "@OpeningLeave", DbType.Decimal, bo.OpeningLeave);
                            dbSmartAspects.AddInParameter(command, "@TakenLeave", DbType.Decimal, bo.TakenLeave);
                            dbSmartAspects.AddInParameter(command, "@RemainingLeave", DbType.Decimal, bo.RemainingLeave);
                            dbSmartAspects.AddInParameter(command, "@MaxDayCanCarryForwardYearly", DbType.Int16, bo.MaxDayCanCarryForwardYearly);
                            dbSmartAspects.AddInParameter(command, "@CarryForwardedLeave", DbType.Int16, bo.CarryForwardedLeave);
                            dbSmartAspects.AddInParameter(command, "@MaxDayCanKeepAsCarryForwardLeave", DbType.Int16, bo.MaxDayCanKeepAsCarryForwardLeave);
                            dbSmartAspects.AddInParameter(command, "@TotalCarryforwardLeave", DbType.Int16, bo.TotalCarryforwardLeave);
                            dbSmartAspects.AddInParameter(command, "@MaxDayCanEncash", DbType.Int16, bo.MaxDayCanEncash);
                            dbSmartAspects.AddInParameter(command, "@EncashableLeave", DbType.Int16, bo.EncashableLeave);
                            dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, bo.ApprovedStatus);
                            dbSmartAspects.AddInParameter(command, "@Status", DbType.String, bo.Status);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);

                            count += dbSmartAspects.ExecuteNonQuery(command);
                            //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                        if (count == addList.Count)
                        {
                            status = true;
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
        public Boolean UpdateOpeningLeave(List<LeaveBalanceClosingBO> addtList, List<LeaveBalanceClosingBO> editList, List<LeaveBalanceClosingBO> deleteList, int modifiedBy)
        {
            Boolean status = false;
            int count = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateOpeningLeaveInfo_SP"))
                        {

                            foreach (LeaveBalanceClosingBO bo in editList)
                            {
                                command.Parameters.Clear();
                                dbSmartAspects.AddInParameter(command, "@LeaveClosingId", DbType.Int64, bo.LeaveClosingId);
                                dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int64, bo.EmpId);
                                dbSmartAspects.AddInParameter(command, "@LeaveTypeId", DbType.Int32, bo.LeaveTypeId);
                                dbSmartAspects.AddInParameter(command, "@OpeningLeave", DbType.Decimal, bo.OpeningLeave);
                                dbSmartAspects.AddInParameter(command, "@TakenLeave", DbType.Decimal, bo.TakenLeave);
                                dbSmartAspects.AddInParameter(command, "@RemainingLeave", DbType.Decimal, bo.RemainingLeave);
                                dbSmartAspects.AddInParameter(command, "@MaxDayCanCarryForwardYearly", DbType.Int16, bo.MaxDayCanCarryForwardYearly);
                                dbSmartAspects.AddInParameter(command, "@CarryForwardedLeave", DbType.Int16, bo.CarryForwardedLeave);
                                dbSmartAspects.AddInParameter(command, "@MaxDayCanKeepAsCarryForwardLeave", DbType.Int16, bo.MaxDayCanKeepAsCarryForwardLeave);
                                dbSmartAspects.AddInParameter(command, "@TotalCarryforwardLeave", DbType.Int16, bo.TotalCarryforwardLeave);
                                dbSmartAspects.AddInParameter(command, "@MaxDayCanEncash", DbType.Int16, bo.MaxDayCanEncash);
                                dbSmartAspects.AddInParameter(command, "@EncashableLeave", DbType.Int16, bo.EncashableLeave);
                                dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, bo.ApprovedStatus);
                                dbSmartAspects.AddInParameter(command, "@Status", DbType.String, bo.Status);
                                dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, modifiedBy);

                                count += dbSmartAspects.ExecuteNonQuery(command);
                            }

                            status = true;

                            //if (count == editList.Count)
                            //{
                            //    status = true;
                            //    count = 0;
                            //}
                        }
                        if (status)
                        {
                            if (addtList != null)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOpeningLeaveInfo_SP"))
                                {
                                    foreach (LeaveBalanceClosingBO bo in addtList)
                                    {
                                        command.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int64, bo.EmpId);
                                        dbSmartAspects.AddInParameter(command, "@LeaveTypeId", DbType.Int32, bo.LeaveTypeId);
                                        dbSmartAspects.AddInParameter(command, "@OpeningLeave", DbType.Decimal, bo.OpeningLeave);
                                        dbSmartAspects.AddInParameter(command, "@TakenLeave", DbType.Decimal, bo.TakenLeave);
                                        dbSmartAspects.AddInParameter(command, "@RemainingLeave", DbType.Decimal, bo.RemainingLeave);
                                        dbSmartAspects.AddInParameter(command, "@MaxDayCanCarryForwardYearly", DbType.Int16, bo.MaxDayCanCarryForwardYearly);
                                        dbSmartAspects.AddInParameter(command, "@CarryForwardedLeave", DbType.Int16, bo.CarryForwardedLeave);
                                        dbSmartAspects.AddInParameter(command, "@MaxDayCanKeepAsCarryForwardLeave", DbType.Int16, bo.MaxDayCanKeepAsCarryForwardLeave);
                                        dbSmartAspects.AddInParameter(command, "@TotalCarryforwardLeave", DbType.Int16, bo.TotalCarryforwardLeave);
                                        dbSmartAspects.AddInParameter(command, "@MaxDayCanEncash", DbType.Int16, bo.MaxDayCanEncash);
                                        dbSmartAspects.AddInParameter(command, "@EncashableLeave", DbType.Int16, bo.EncashableLeave);
                                        dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, bo.ApprovedStatus);
                                        dbSmartAspects.AddInParameter(command, "@Status", DbType.String, bo.Status);
                                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, modifiedBy);

                                        count += dbSmartAspects.ExecuteNonQuery(command);
                                    }

                                    status = true;

                                    //if (count == addtList.Count)
                                    //{
                                    //    status = true;
                                    //    count = 0;
                                    //}
                                }
                            }
                            if (deleteList != null)
                            {
                                foreach (LeaveBalanceClosingBO bo in deleteList)
                                {
                                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteOpeningLeaveInfo_SP"))
                                    {
                                        dbSmartAspects.AddInParameter(command, "@LeaveClosingId", DbType.Int64, bo.LeaveClosingId);
                                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, modifiedBy);

                                        count = dbSmartAspects.ExecuteNonQuery(command);
                                    }

                                    status = true;

                                    //if (count == deleteList.Count)
                                    //{
                                    //    status = true;
                                    //}
                                }
                            }
                        }
                        if (status)
                        {
                            transction.Commit();
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

        public Boolean SaveClosingLeave(int empId, int leaveTypeId, int createdBy)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveClosingLeaveInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int64, empId);
                        dbSmartAspects.AddInParameter(command, "@LeaveTypeId", DbType.Int32, leaveTypeId);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);

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

        public List<LeaveBalanceClosingViewBO> GetLeaveBalanceClosingBySearchCriteriaForPaging(long empId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<LeaveBalanceClosingViewBO> boList = new List<LeaveBalanceClosingViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLeaveBalanceClosingBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int64, empId);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "OpeningLeave");
                    DataTable Table = ds.Tables["OpeningLeave"];

                    boList = Table.AsEnumerable().Select(r => new LeaveBalanceClosingViewBO
                    {
                        LeaveClosingId = r.Field<Int64>("LeaveClosingId"),
                        EmpId = r.Field<Int64>("EmpId"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        Deparment = r.Field<string>("Deparment"),
                        Designation = r.Field<string>("Designation")

                    }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return boList;
        }
        public List<LeaveBalanceClosingBO> GetLeaveBalanceClosingByEmpId(long empId, int userInfoId)
        {
            List<LeaveBalanceClosingBO> viewlist = new List<LeaveBalanceClosingBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLeaveBalanceClosingByEmpId_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int64, empId);
                        dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int64, userInfoId);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "LeaveBalanceClosing");
                        DataTable Table = ds.Tables["LeaveBalanceClosing"];

                        viewlist = Table.AsEnumerable().Select(r => new LeaveBalanceClosingBO
                        {
                            LeaveClosingId = r.Field<long>("LeaveClosingId"),
                            EmpId = r.Field<long>("EmpId"),
                            LeaveTypeId = r.Field<int>("LeaveTypeId"),
                            FiscalYearId = r.Field<int>("FiscalYearId"),
                            OpeningLeave = r.Field<decimal>("OpeningLeave"),
                            TakenLeave = r.Field<decimal>("TakenLeave"),
                            RemainingLeave = r.Field<decimal>("RemainingLeave"),
                            MaxDayCanCarryForwardYearly = r.Field<byte>("MaxDayCanCarryForwardYearly"),
                            CarryForwardedLeave = r.Field<byte>("CarryForwardedLeave"),
                            MaxDayCanKeepAsCarryForwardLeave = r.Field<byte>("MaxDayCanKeepAsCarryForwardLeave"),
                            TotalCarryforwardLeave = r.Field<byte>("TotalCarryforwardLeave"),
                            MaxDayCanEncash = r.Field<byte>("MaxDayCanEncash"),
                            EncashableLeave = r.Field<byte>("EncashableLeave"),
                            ApprovedStatus = r.Field<string>("ApprovedStatus"),
                            Status = r.Field<string>("Status")

                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return viewlist;
        }
        public List<LeaveClosingInfoBO> GetLeaveClosingInfo(int departmentId)
        {
            List<LeaveClosingInfoBO> viewlist = new List<LeaveClosingInfoBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLeaveClosingInfo_SP"))
                    {
                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "LeaveClosing");
                        DataTable Table = ds.Tables["LeaveClosing"];

                        viewlist = Table.AsEnumerable().Select(r => new LeaveClosingInfoBO
                        {
                            EmpId = r.Field<int?>("EmpId"),
                            EmpCode = r.Field<string>("EmpCode"),
                            EmployeeName = r.Field<string>("EmployeeName"),
                            DepartmentId = r.Field<int?>("DepartmentId"),
                            DepartmentName = r.Field<string>("DepartmentName"),
                            DesignationId = r.Field<int?>("DesignationId"),
                            DesignationName = r.Field<string>("DesignationName"),
                            LeaveTypeId = r.Field<int?>("LeaveTypeId"),
                            LeaveTypeName = r.Field<string>("LeaveTypeName"),
                            OpeningLeave = r.Field<decimal>("OpeningLeave"),
                            TotalLeaveTaken = r.Field<int?>("TotalLeaveTaken"),
                            RemainingLeave = r.Field<decimal?>("RemainingLeave"),
                            CanCarryForward = r.Field<bool?>("CanCarryForward"),
                            MaxDayCanCarryForwardYearly = r.Field<byte>("MaxDayCanCarryForwardYearly"),
                            MaxDayCanKeepAsCarryForwardLeave = r.Field<byte>("MaxDayCanKeepAsCarryForwardLeave"),
                            LastYearCarryForwardedLeave = r.Field<byte>("LastYearCarryForwardedLeave"),
                            CurrentYearCarryForwardedLeave = r.Field<decimal?>("CurrentYearCarryForwardedLeave"),
                            CarryForwardedLeave = r.Field<decimal?>("CarryForwardedLeave"),
                            CanEncash = r.Field<bool?>("CanEncash"),
                            MaxDayCanEncash = r.Field<byte>("MaxDayCanEncash"),
                            CurrentYearEncashableLeave = r.Field<decimal?>("CurrentYearEncashableLeave")

                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return viewlist;
        }
        public List<LeaveInformationBO> GetEmpMonthlyLeaveStatus(int empId)
        {
            List<LeaveInformationBO> leaveStatusList = new List<LeaveInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpLeaveStatus_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, Convert.ToInt32(empId));

                    DataSet LeaveDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "LeaveStatus");
                    DataTable Table = LeaveDS.Tables["LeaveStatus"];

                    leaveStatusList = Table.AsEnumerable().Select(r => new LeaveInformationBO
                    {
                        TypeName = r.Field<string>("TypeName"),
                        Taken = r.Field<int>("Taken"),
                        Available = r.Field<int>("Available"),
                        Month = r.Field<string>("Month"),
                        CommulativeLeave = r.Field<int>("CommulativeLeave")

                    }).ToList();
                }
            }
            return leaveStatusList;
        }
        public List<LeaveInformationBO> GetEmpLeaveSummary(int empId)
        {
            List<LeaveInformationBO> leaveStatusList = new List<LeaveInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpLeaveSummary_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, Convert.ToInt32(empId));

                    DataSet LeaveDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "LeaveSummary");
                    DataTable Table = LeaveDS.Tables["LeaveSummary"];

                    leaveStatusList = Table.AsEnumerable().Select(r => new LeaveInformationBO
                    {
                        TypeName = r.Field<string>("TypeName"),
                        Taken = r.Field<int>("Taken"),
                        Available = r.Field<int>("Available")
                        //Month = r.Field<string>("Month"),
                        //CommulativeLeave = r.Field<int>("CommulativeLeave")

                    }).ToList();
                }
            }
            return leaveStatusList;
        }

        public List<LeaveInformationBO> GetEmpDuplicateLeaveInformation(int empId, DateTime fromDate, DateTime toDate)
        {
            List<LeaveInformationBO> leaveInformationList = new List<LeaveInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpDuplicateLeaveInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(cmd, "@LeaveFromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@LeaveToDate", DbType.DateTime, toDate);

                    DataSet LeaveDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "Leave");
                    DataTable Table = LeaveDS.Tables["Leave"];

                    leaveInformationList = Table.AsEnumerable().Select(r => new LeaveInformationBO
                    {
                        LeaveId = r.Field<int>("LeaveId"),
                        EmpId = r.Field<int>("EmpId"),
                        LeaveMode = r.Field<string>("LeaveMode"),
                        LeaveTypeId = r.Field<int>("LeaveTypeId"),
                        FromDate = r.Field<DateTime>("FromDate"),
                        ToDate = r.Field<DateTime>("ToDate"),
                        NoOfDays = r.Field<int>("NoOfDays"),
                        LeaveStatus = r.Field<string>("LeaveStatus"),
                        Reason = r.Field<string>("Reason")
                    }).ToList();
                }
            }
            return leaveInformationList;
        }

        public List<LeaveInformationBO> GetLeaveInformationByEmpIdAndDateRange(int empId, DateTime fromDate, DateTime toDate)
        {
            List<LeaveInformationBO> leaveInformationList = new List<LeaveInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLeaveInformationByEmpIdAndDateRange_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(cmd, "@LeaveFromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@LeaveToDate", DbType.DateTime, toDate);

                    DataSet LeaveDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "Leave");
                    DataTable Table = LeaveDS.Tables["Leave"];

                    leaveInformationList = Table.AsEnumerable().Select(r => new LeaveInformationBO
                    {
                        LeaveId = r.Field<int>("LeaveId"),
                        EmpId = r.Field<int>("EmpId"),
                        LeaveMode = r.Field<string>("LeaveMode"),
                        LeaveTypeId = r.Field<int>("LeaveTypeId"),
                        FromDate = r.Field<DateTime>("FromDate"),
                        ToDate = r.Field<DateTime>("ToDate"),
                        NoOfDays = r.Field<int>("NoOfDays"),
                        LeaveStatus = r.Field<string>("LeaveStatus"),
                        Reason = r.Field<string>("Reason")
                    }).ToList();
                }
            }
            return leaveInformationList;
        }

        public List<LeaveInformationBO> GetEmployeeWiseLeaveBalance(long empId)
        {
            List<LeaveInformationBO> viewlist = new List<LeaveInformationBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeWiseLeaveBalance_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int64, empId);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "LeaveBalanceClosing");
                        DataTable Table = ds.Tables["LeaveBalanceClosing"];

                        viewlist = Table.AsEnumerable().Select(r => new LeaveInformationBO
                        {
                            LeaveTypeId = r.Field<int>("LeaveTypeId"),
                            TypeName = r.Field<string>("TypeName"),
                            OpeningLeave = r.Field<decimal>("OpeningLeave"),
                            LeaveModeId = r.Field<int>("LeaveModeId"),
                            LeaveMode = r.Field<string>("LeaveMode")

                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return viewlist;
        }

        //Monthly Leave & Attendance Save
        public Boolean SaveMonthlyLeaveAndAttendance(List<EmpAttendanceBO> attendance, List<LeaveInformationBO> leaveInformation, int createdBy, int empId, DateTime fromDate, DateTime toDate, out int tmpLeaveId)
        {
            Boolean status = false;
            tmpLeaveId = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        try
                        {
                            string query = string.Empty;
                            query = string.Format(@"DELETE FROM PayrollEmpAttendance
                                        WHERE dbo.FnDate(AttendanceDate) BETWEEN dbo.FnDate('{0}') AND dbo.FnDate('{1}') and EmpId = {2}

                                        DELETE FROM PayrollEmpLeaveInformation
                                        WHERE (dbo.FnDate(FromDate) BETWEEN dbo.FnDate('{0}') AND dbo.FnDate('{1}')) AND (dbo.FnDate(ToDate) BETWEEN dbo.FnDate('{0}') AND dbo.FnDate('{1}')) and EmpId = {2}", fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"), empId);

                            using (DbCommand command = dbSmartAspects.GetSqlStringCommand(query))
                            {
                                dbSmartAspects.ExecuteNonQuery(command, transction);
                            }

                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpAttendenceInfo_SP"))
                            {
                                foreach (EmpAttendanceBO atn in attendance)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, atn.EmpId);

                                    if (atn.EntryTime != null)
                                        dbSmartAspects.AddInParameter(command, "@EntryTime", DbType.DateTime, atn.EntryTime);
                                    else
                                        dbSmartAspects.AddInParameter(command, "@EntryTime", DbType.DateTime, DBNull.Value);

                                    if (atn.ExitTime != null)
                                        dbSmartAspects.AddInParameter(command, "@ExitTime", DbType.DateTime, atn.ExitTime);
                                    else
                                        dbSmartAspects.AddInParameter(command, "@ExitTime", DbType.DateTime, DBNull.Value);

                                    dbSmartAspects.AddInParameter(command, "@AttendanceDate", DbType.DateTime, atn.AttendanceDate);

                                    if (!string.IsNullOrEmpty(atn.Remark))
                                        dbSmartAspects.AddInParameter(command, "@Remark", DbType.DateTime, atn.Remark);
                                    else
                                        dbSmartAspects.AddInParameter(command, "@Remark", DbType.DateTime, DBNull.Value);

                                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);
                                    dbSmartAspects.AddOutParameter(command, "@AttendanceId", DbType.Int32, sizeof(Int32));


                                    status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;

                                }
                            }

                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpLeaveInformation_SP"))
                            {
                                foreach (LeaveInformationBO liv in leaveInformation)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, liv.EmpId);
                                    dbSmartAspects.AddInParameter(command, "@LeaveMode", DbType.String, liv.LeaveMode);
                                    dbSmartAspects.AddInParameter(command, "@LeaveTypeId", DbType.Int32, liv.LeaveTypeId);
                                    dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, liv.FromDate);
                                    dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, liv.ToDate);
                                    dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, liv.TransactionType);
                                    dbSmartAspects.AddInParameter(command, "@NoOfDays", DbType.Int32, liv.NoOfDays);

                                    if (liv.ExpireDate != null)
                                    {
                                        dbSmartAspects.AddInParameter(command, "@ExpireDate", DbType.DateTime, liv.ExpireDate);
                                    }
                                    else
                                        dbSmartAspects.AddInParameter(command, "@ExpireDate", DbType.DateTime, DBNull.Value);

                                    dbSmartAspects.AddInParameter(command, "@LeaveStatus", DbType.String, liv.LeaveStatus);

                                    if (!string.IsNullOrEmpty(liv.Reason))
                                        dbSmartAspects.AddInParameter(command, "@Reason", DbType.String, liv.Reason);
                                    else
                                        dbSmartAspects.AddInParameter(command, "@Reason", DbType.String, DBNull.Value);

                                    if (liv.ReportingTo != 0)
                                        dbSmartAspects.AddInParameter(command, "@ReportingTo", DbType.String, liv.ReportingTo);
                                    else
                                        dbSmartAspects.AddInParameter(command, "@ReportingTo", DbType.String, DBNull.Value);


                                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);
                                    dbSmartAspects.AddOutParameter(command, "@LeaveId", DbType.Int32, sizeof(Int32));

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;
                                    tmpLeaveId = Convert.ToInt32(command.Parameters["@LeaveId"].Value);
                                }
                            }

                            if (status)
                            {
                                transction.Commit();
                            }
                            else
                            {
                                transction.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            status = false;
                            transction.Rollback();
                            throw ex;
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

        public void a(int empId, DateTime fromDate, DateTime toDate)
        {
            string query = string.Empty;
            query = string.Format(@"DELETE FROM PayrollEmpAttendance
                    WHERE dbo.fnDate(AttendanceDate) BETWEEN 'dbo.fnDate({0})' AND 'dbo.fnDate({1})' and EmpId = {2}

                    DELETE FROM PayrollEmpLeaveInformation
                    WHERE (dbo.fnDate(FromDate) BETWEEN 'dbo.fnDate({0})' AND 'dbo.fnDate({1})') AND (dbo.fnDate(ToDate) BETWEEN 'dbo.fnDate({0})' AND 'dbo.fnDate({1})') and EmpId = {2}", fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"), empId);


        }

        public Boolean SaveLeaveAndAttendance(List<EmpAttendanceBO> attendanceEntry, List<EmpAttendanceBO> attendanceUpdate, List<LeaveInformationBO> leaveInformation, int createdBy)
        {
            int status = 0, tmpLeaveId = 0;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        try
                        {
                            if (leaveInformation.Count > 0)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpLeaveInformation_SP"))
                                {
                                    foreach (LeaveInformationBO li in leaveInformation)
                                    {
                                        command.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, li.EmpId);
                                        dbSmartAspects.AddInParameter(command, "@LeaveMode", DbType.String, li.LeaveMode);
                                        dbSmartAspects.AddInParameter(command, "@LeaveTypeId", DbType.Int32, li.LeaveTypeId);
                                        dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, li.FromDate);
                                        dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, li.ToDate);
                                        dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, li.TransactionType);
                                        dbSmartAspects.AddInParameter(command, "@NoOfDays", DbType.Int32, li.NoOfDays);

                                        if (li.TransactionType == "Addition")
                                        {
                                            dbSmartAspects.AddInParameter(command, "@ExpireDate", DbType.DateTime, li.ExpireDate);
                                        }

                                        dbSmartAspects.AddInParameter(command, "@LeaveStatus", DbType.String, li.LeaveStatus);
                                        dbSmartAspects.AddInParameter(command, "@Reason", DbType.String, li.Reason);
                                        dbSmartAspects.AddInParameter(command, "@ReportingTo", DbType.String, li.ReportingTo);
                                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, li.CreatedBy);
                                        dbSmartAspects.AddOutParameter(command, "@LeaveId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                        tmpLeaveId = Convert.ToInt32(command.Parameters["@LeaveId"].Value);
                                    }
                                }
                            }

                            if (attendanceEntry.Count > 0)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpAttendenceInfo_SP"))
                                {
                                    foreach (EmpAttendanceBO ad in attendanceEntry)
                                    {
                                        command.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, ad.EmpId);

                                        if (ad.EntryTime != null)
                                            dbSmartAspects.AddInParameter(command, "@EntryTime", DbType.DateTime, ad.EntryTime);
                                        else
                                            dbSmartAspects.AddInParameter(command, "@EntryTime", DbType.DateTime, DBNull.Value);

                                        if (ad.ExitTime != null)
                                            dbSmartAspects.AddInParameter(command, "@ExitTime", DbType.DateTime, ad.ExitTime);
                                        else
                                            dbSmartAspects.AddInParameter(command, "@ExitTime", DbType.DateTime, DBNull.Value);

                                        dbSmartAspects.AddInParameter(command, "@AttendanceDate", DbType.DateTime, ad.AttendanceDate);
                                        dbSmartAspects.AddInParameter(command, "@Remark", DbType.String, ad.Remark);
                                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);
                                        dbSmartAspects.AddOutParameter(command, "@AttendanceId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                        tmpLeaveId = Convert.ToInt32(command.Parameters["@AttendanceId"].Value);
                                    }
                                }
                            }

                            if (attendanceUpdate.Count > 0)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpAttendenceInfo_SP"))
                                {
                                    foreach (EmpAttendanceBO ad in attendanceUpdate)
                                    {
                                        command.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(command, "@AttendanceId", DbType.Int32, ad.AttendanceId);
                                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, ad.EmpId);

                                        if (ad.EntryTime != null)
                                            dbSmartAspects.AddInParameter(command, "@EntryTime", DbType.DateTime, ad.EntryTime);
                                        else
                                            dbSmartAspects.AddInParameter(command, "@EntryTime", DbType.DateTime, DBNull.Value);

                                        if (ad.ExitTime != null)
                                            dbSmartAspects.AddInParameter(command, "@ExitTime", DbType.DateTime, ad.ExitTime);
                                        else
                                            dbSmartAspects.AddInParameter(command, "@ExitTime", DbType.DateTime, DBNull.Value);

                                        dbSmartAspects.AddInParameter(command, "@AttendanceDate", DbType.DateTime, ad.AttendanceDate);
                                        dbSmartAspects.AddInParameter(command, "@Remark", DbType.String, ad.Remark);
                                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, createdBy);

                                        status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                    }
                                }
                            }

                            if (status > 0)
                            {
                                transction.Commit();
                            }
                            else
                            {
                                transction.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            status = 0;
                            transction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (status > 0);
        }

        public bool LeaveApproval(int Id, string approvedStatus, int checkedOrApprovedBy)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("LeaveApproval_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@Id", DbType.Int32, Id);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, checkedOrApprovedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        }
                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }
            return retVal;
        }
        public bool UpdateEmpLeaveWorkHandoverStatus(LeaveInformationBO leaveInformationBO)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateEmpLeaveWorkHandoverStatus_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@LeaveId", DbType.Int32, leaveInformationBO.LeaveId);
                            dbSmartAspects.AddInParameter(commandMaster, "@WorkHandoverStatus", DbType.String, leaveInformationBO.WorkHandoverStatus);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        }
                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }
            return retVal;
        }

    }
}
