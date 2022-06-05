using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpSalaryProcessDA : BaseService
    {
        public List<SalaryProcessMonthBO> GetSalaryProcessMonth()
        {
            List<SalaryProcessMonthBO> monthBOList = new List<SalaryProcessMonthBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalaryProcessMonth_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalaryProcessMonthBO monthBO = new SalaryProcessMonthBO();

                                monthBO.MonthValue = reader["MonthValue"].ToString();
                                monthBO.MonthHead = reader["MonthHead"].ToString();

                                monthBOList.Add(monthBO);
                            }
                        }
                    }
                }
            }
            return monthBOList;
        }
        public SalaryProcessMonthBO GetTemporarySalaryProcessInformationByProcessId(int processId)
        {
            SalaryProcessMonthBO monthBO = new SalaryProcessMonthBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("GetTemporarySalaryProcessInformationByProcessId_SP"))
                {
                    dbSmartAspects.AddInParameter(commandMaster, "@ProcessId", DbType.Int32, processId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(commandMaster))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                monthBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                            }
                        }
                    }
                }
            }
            return monthBO;
        }
        public List<EmpSalaryProcessDetailBO> GetPayrollMonthlySalaryInfo(int empId, int processId, int totalAbsent, DateTime fromDate, DateTime toDate)
        {
            List<EmpSalaryProcessDetailBO> monthBOList = new List<EmpSalaryProcessDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollMonthlySalaryInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(cmd, "@TotalAbsent", DbType.Int32, totalAbsent);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryMonthStartDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryMonthEndDate", DbType.DateTime, toDate);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpSalaryProcessDetailBO monthBO = new EmpSalaryProcessDetailBO();

                                monthBO.ProcessId = processId;
                                monthBO.EmpId = Convert.ToInt32(reader["EmpId"]);
                                monthBO.SalaryHeadId = Convert.ToInt32(reader["SalaryHeadId"]);
                                monthBO.SalaryHeadNote = Convert.ToString(reader["SalaryHeadNote"]);
                                monthBO.SalaryType = reader["SalaryType"].ToString();
                                monthBO.SalaryAmount = Convert.ToDecimal(reader["SalaryHeadAmount"]);

                                monthBOList.Add(monthBO);
                            }
                        }
                    }
                }
            }
            return monthBOList;
        }
        public int GetEmployeeAbsentInfo(int empId, DateTime fromDate, DateTime toDate)
        {
            int absentCount = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("GetEmpAbsentInfoByEmpIdNDateRange_SP"))
                {
                    dbSmartAspects.AddInParameter(commandMaster, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(commandMaster, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(commandMaster, "@ToDate", DbType.DateTime, toDate);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(commandMaster))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalaryProcessMonthBO monthBO = new SalaryProcessMonthBO();

                                absentCount = Convert.ToInt32(reader["AbsentCount"]);
                            }
                        }
                    }
                }
            }
            return absentCount;
        }
        public Boolean MonthlySalaryProcessForEmployee(EmpSalaryProcessBO empSalaryProcessBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("MonthlySalaryProcessForEmployee_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@SalaryMonthStartDate", DbType.DateTime, empSalaryProcessBO.SalaryDateFrom);
                        dbSmartAspects.AddInParameter(commandMaster, "@SalaryMonthEndDate", DbType.DateTime, empSalaryProcessBO.SalaryDateTo);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, empSalaryProcessBO.CreatedBy);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction) > 0 ? true : false;

                        if (status)
                        {
                            transction.Commit();
                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                }
            }
            return status;
        }
        //public Boolean SaveMonthlySalaryProcessInfo(DateTime salaryMonthStartDate, DateTime salaryMonthEndDate, List<EmpSalaryProcessBO> empSalaryProcessBOList, List<EmpSalaryProcessDetailBO> detailBOList)
        //{
        //    bool retVal = false;
        //    int status = 0;

        //    Boolean deleteStatus = false;
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteMonthlySalaryProcessInfo_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(command, "@SalaryMonthStartDate", DbType.DateTime, salaryMonthStartDate);
        //            dbSmartAspects.AddInParameter(command, "@SalaryMonthEndDate", DbType.DateTime, salaryMonthEndDate);

        //            deleteStatus = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
        //        }
        //    }

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        conn.Open();
        //        using (DbTransaction transction = conn.BeginTransaction())
        //        {
        //            int transactionCount = 0;
        //            foreach (EmpSalaryProcessBO rowSalaryProcess in empSalaryProcessBOList)
        //            {
        //                int tmpProcessId = 0;
        //                List<EmpSalaryProcessDetailBO> detailBO = new List<EmpSalaryProcessDetailBO>();

        //                //detailBO = detailBOList.Where(test => test.EmpId == rowSalaryProcess.EmpId && test.ProcessId == rowSalaryProcess.ProcessId).ToList();

        //                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveMonthlySalaryProcessInfo_SP"))
        //                {
        //                    dbSmartAspects.AddInParameter(commandMaster, "@ProcessDate", DbType.DateTime, rowSalaryProcess.ProcessDate);
        //                    //dbSmartAspects.AddInParameter(commandMaster, "@EmpId", DbType.Int32, rowSalaryProcess.EmpId);
        //                    dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, rowSalaryProcess.CreatedBy);
        //                    dbSmartAspects.AddOutParameter(commandMaster, "@ProcessId", DbType.Int32, sizeof(Int32));

        //                    status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

        //                    tmpProcessId = Convert.ToInt32(commandMaster.Parameters["@ProcessId"].Value);

        //                    if (status > 0)
        //                    {
        //                        int count = 0;

        //                        using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveMonthlySalaryProcessDetailInfo_SP"))
        //                        {
        //                            foreach (EmpSalaryProcessDetailBO processDetailBO in detailBO)
        //                            {
        //                                if (processDetailBO.ProcessDetailId == 0)
        //                                {
        //                                    commandDetails.Parameters.Clear();

        //                                    dbSmartAspects.AddInParameter(commandDetails, "@ProcessId", DbType.Int32, tmpProcessId);
        //                                    dbSmartAspects.AddInParameter(commandDetails, "@SalaryHeadId", DbType.Int32, processDetailBO.SalaryHeadId);
        //                                    dbSmartAspects.AddInParameter(commandDetails, "@SalaryHeadNote", DbType.String, processDetailBO.SalaryHeadNote);
        //                                    dbSmartAspects.AddInParameter(commandDetails, "@SalaryType", DbType.String, processDetailBO.SalaryType);
        //                                    dbSmartAspects.AddInParameter(commandDetails, "@SalaryHeadAmount", DbType.Decimal, processDetailBO.SalaryAmount);

        //                                    count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
        //                                }
        //                            }
        //                        }
        //                    }

        //                }
        //                transactionCount = transactionCount + 1;
        //            }
        //            if (transactionCount == empSalaryProcessBOList.Count)
        //            {
        //                transction.Commit();
        //                retVal = true;
        //            }
        //            else
        //            {
        //                retVal = false;
        //            }
        //        }

        //        if (retVal == true)
        //        {
        //            using (DbCommand commandUpdate = dbSmartAspects.GetStoredProcCommand("UpdatePayrollMonthlySalaryInfo_SP"))
        //            {
        //                foreach (EmpSalaryProcessBO rowSalaryProcess in empSalaryProcessBOList)
        //                {
        //                    commandUpdate.Parameters.Clear();

        //                    // dbSmartAspects.AddInParameter(commandUpdate, "@EmpId", DbType.Int32, rowSalaryProcess.EmpId);
        //                    dbSmartAspects.AddInParameter(commandUpdate, "@SalaryMonthStartDate", DbType.DateTime, salaryMonthStartDate);
        //                    dbSmartAspects.AddInParameter(commandUpdate, "@SalaryMonthEndDate", DbType.DateTime, salaryMonthEndDate);

        //                    deleteStatus = dbSmartAspects.ExecuteNonQuery(commandUpdate) > 0 ? true : false;
        //                }
        //            }
        //        }
        //    }

        //    return retVal;
        //}

        public Boolean SaveMonthlySalaryProcess(DateTime salaryMonthStartDate, DateTime salaryMonthEndDate, int salaryYear, int createDate)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("MonthlySalaryProcess_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@SalaryDateFrom", DbType.DateTime, salaryMonthStartDate);
                            dbSmartAspects.AddInParameter(command, "@SalaryDateTo", DbType.DateTime, salaryMonthEndDate);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createDate);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }

                        if (status)
                        {
                            transction.Commit();
                            status = true;
                        }
                        else
                        {
                            transction.Rollback();
                            status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        status = false;
                        throw ex;
                    }
                }
            }

            return status;
        }
        public Boolean SaveMonthlySalaryProcessTemp(int companyId, int projectId, DateTime salaryMonthStartDate, DateTime salaryMonthEndDate, int salaryYear, int createDate, int processId)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if (processId == 1)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("MonthlySalaryProcessTemp_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int16, companyId);
                                dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int16, projectId);
                                dbSmartAspects.AddInParameter(command, "@SalaryDateFrom", DbType.DateTime, salaryMonthStartDate);
                                dbSmartAspects.AddInParameter(command, "@SalaryDateTo", DbType.DateTime, salaryMonthEndDate);
                                dbSmartAspects.AddInParameter(command, "@SalaryYear", DbType.Int16, salaryYear);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createDate);

                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            }
                        }
                        else if (processId == 2)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("MonthlySalaryProcessForRedcrossTemp_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@SalaryDateFrom", DbType.DateTime, salaryMonthStartDate);
                                dbSmartAspects.AddInParameter(command, "@SalaryDateTo", DbType.DateTime, salaryMonthEndDate);
                                dbSmartAspects.AddInParameter(command, "@SalaryYear", DbType.Int16, salaryYear);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createDate);

                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            }
                        }
                        else if (processId == 3)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("MonthlySalaryProcessIPTechTemp_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@SalaryDateFrom", DbType.DateTime, salaryMonthStartDate);
                                dbSmartAspects.AddInParameter(command, "@SalaryDateTo", DbType.DateTime, salaryMonthEndDate);
                                dbSmartAspects.AddInParameter(command, "@SalaryYear", DbType.Int16, salaryYear);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createDate);

                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            }
                        }
                        else if (processId == 4)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("MonthlySalaryProcessForSouthSudanTemp_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@SalaryDateFrom", DbType.DateTime, salaryMonthStartDate);
                                dbSmartAspects.AddInParameter(command, "@SalaryDateTo", DbType.DateTime, salaryMonthEndDate);
                                dbSmartAspects.AddInParameter(command, "@SalaryYear", DbType.Int16, salaryYear);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createDate);

                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            }
                        }

                        if (status)
                        {
                            transction.Commit();
                            status = true;
                        }
                        else
                        {
                            transction.Rollback();
                            status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        status = false;
                        throw ex;
                    }
                }
            }

            return status;
        }

        public Boolean ApprovedMonthlySalaryProcess(int salaryProcessId, int createBy)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ApprovedMonthlySalaryProcess_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@ProcessId", DbType.Int32, salaryProcessId);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, createBy);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }

                        if (status)
                        {
                            transction.Commit();
                            status = true;
                        }
                        else
                        {
                            transction.Rollback();
                            status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        status = false;
                        throw ex;
                    }
                }
            }

            return status;
        }
        public List<AttendanceAndLeaveApprovalBO> GetEmployeeLateAttendanceDeductionProcess(string processType, int reportType, DateTime salaryDateFrom, DateTime salaryDateTo, int userInfoId)
        {
            List<AttendanceAndLeaveApprovalBO> salaryProcess = new List<AttendanceAndLeaveApprovalBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeeLateAttendanceDeductionProcess_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessType", DbType.String, processType);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.Int16, reportType);
                    dbSmartAspects.AddInParameter(cmd, "@RosterDateFrom", DbType.DateTime, salaryDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@RosterDateTo", DbType.DateTime, salaryDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int16, userInfoId);

                    DataSet salarySheetDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, salarySheetDS, "Attendance");
                    DataTable Table = salarySheetDS.Tables["Attendance"];

                    salaryProcess = Table.AsEnumerable().Select(r => new AttendanceAndLeaveApprovalBO
                    {
                        EmpId = r.Field<Int32?>("EmpId"),
                        //MonthId = r.Field<Int32?>("MonthId"),
                        AttendanceId = r.Field<int?>("AttendanceId"),
                        AttendanceDate = r.Field<DateTime?>("AttendanceDate"),
                        InTime = r.Field<TimeSpan?>("InTime"),
                        OutTime = r.Field<TimeSpan?>("OutTime")
                        //,
                        //LeaveId = r.Field<int?>("LeaveId"),
                        //LeaveTypeId = r.Field<Int32?>("LeaveTypeId"),
                        //LeaveType = r.Field<string>("LeaveType"),
                        //LeaveMode = r.Field<string>("LeaveMode"),
                        //Reamrks = r.Field<string>("Reamrks"),
                        //AttendanceColor = r.Field<string>("AttendanceColor")

                    }).ToList();


                }
            }
            return salaryProcess;
        }
        public Boolean EmployeeLateAttendanceDeductionProcess(string processType, int reportType, DateTime salaryDateFrom, DateTime salaryDateTo, int userInfoId)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("EmployeeLateAttendanceDeductionProcess_SP"))
                        {
                            command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                            dbSmartAspects.AddInParameter(command, "@ProcessType", DbType.String, processType);
                            dbSmartAspects.AddInParameter(command, "@ReportType", DbType.Int16, reportType);
                            dbSmartAspects.AddInParameter(command, "@RosterDateFrom", DbType.DateTime, salaryDateFrom);
                            dbSmartAspects.AddInParameter(command, "@RosterDateTo", DbType.DateTime, salaryDateTo);
                            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int16, userInfoId);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }

                        if (status)
                        {
                            transction.Commit();
                            status = true;
                        }
                        else
                        {
                            transction.Rollback();
                            status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        status = false;
                        throw ex;
                    }
                }
            }

            return status;
        }
        public List<EmpSalaryProcessBO> GetSalaryProcessFromTempTable(int companyId, int projectId, int salaryYear, DateTime salaryDateFrom, DateTime salaryDateTo)
        {
            List<EmpSalaryProcessBO> salaryProcess = new List<EmpSalaryProcessBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalaryProcessFromTempTable_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int16, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int16, projectId);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryDateFrom", DbType.DateTime, salaryDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryDateTo", DbType.DateTime, salaryDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryYear", DbType.Int16, salaryYear);

                    DataSet salarySheetDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, salarySheetDS, "SalarySheet");
                    DataTable Table = salarySheetDS.Tables["SalarySheet"];

                    salaryProcess = Table.AsEnumerable().Select(r => new EmpSalaryProcessBO
                    {
                        ProcessId = r.Field<int>("ProcessId"),
                        ProcessDate = r.Field<DateTime>("ProcessDate"),
                        SalaryDateFrom = r.Field<DateTime>("SalaryDateFrom"),
                        SalaryDateTo = r.Field<DateTime>("SalaryDateTo"),
                        SalaryYear = r.Field<short>("SalaryYear"),
                        ProcessSequence = r.Field<byte>("ProcessSequence")

                    }).ToList();
                }
            }
            return salaryProcess;
        }

        public List<SalarySheetBO> EmployeeSalarySheet(int employeeId, int departmentId, int gradeId, int branchId, DateTime processDateFrom, DateTime processDateTo)
        {
            List<SalarySheetBO> salarySheetList = new List<SalarySheetBO>();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmpSalarySheetReport_SP"))
                {
                    if (employeeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, employeeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (gradeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, gradeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, DBNull.Value);

                    if (branchId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Branch", DbType.Int32, branchId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Branch", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, processDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, processDateTo);

                    DataSet salarySheetDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, salarySheetDS, "SalarySheet");

                    DataTable Table = salarySheetDS.Tables["SalarySheet"];

                    salarySheetList = Table.AsEnumerable().Select(r => new SalarySheetBO
                    {
                        EmpId = r.Field<int?>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        Position = r.Field<string>("Position"),
                        LocationId = r.Field<int?>("LocationId"),
                        Location = r.Field<string>("Location"),
                        DepartmentId = r.Field<int?>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        DaysFrom = r.Field<string>("DaysFrom"),
                        DaysTo = r.Field<string>("DaysTo"),
                        PaidDays = r.Field<int?>("PaidDays"),
                        Project = r.Field<string>("Project"),
                        GrossSalary = r.Field<decimal?>("GrossSalary"),
                        TaxableIncome = r.Field<decimal?>("TaxableIncome"),
                        NetSalary = r.Field<decimal?>("NetSalary"),
                        PayHeadId = r.Field<int?>("PayHeadId"),
                        PayHead = r.Field<string>("PayHead"),
                        PayHeadType = r.Field<string>("PayHeadType"),
                        PayHeadDescription = r.Field<string>("PayHeadDescription"),
                        PayHeadAmount = r.Field<decimal?>("PayHeadAmount")

                    }).ToList();

                }
            }
            return salarySheetList;
        }

        public List<SalarySheetBO> EmployeeSalarySheets(int glCompanyId, string reportType, int employeeId, int departmentId, int gradeId, int branchId, DateTime processDateFrom, DateTime processDateTo, short processYear, string currencyType)
        {
            List<SalarySheetBO> salarySheetList = new List<SalarySheetBO>();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeeSalarySheet_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);

                    if (glCompanyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, glCompanyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, DBNull.Value);

                    if (employeeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, employeeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (gradeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, gradeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, DBNull.Value);

                    if (branchId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Branch", DbType.Int32, branchId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Branch", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@CurrencyType", DbType.String, currencyType);

                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, processDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, processDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.Int16, processYear);

                    DataSet salarySheetDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, salarySheetDS, "SalarySheet");

                    DataTable Table = salarySheetDS.Tables["SalarySheet"];

                    // ProcessId	EmpId	EmpCode	EmployeeName	Designation	LocationId	Location	
                    // DepartmentId	DepartmentName	SalaryDateFrom	SalaryDateTo	PaidDays	DonorId	
                    // Project	BasicAmount	SalaryHeadId	SalaryHead	SalaryType	SalaryAmount	GrossAmount	HomeTakenAmount

                    salarySheetList = Table.AsEnumerable().Select(r => new SalarySheetBO
                    {
                        ProcessId = r.Field<int>("ProcessId"),
                        EmpId = r.Field<int?>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        Designation = r.Field<string>("Designation"),
                        LocationId = r.Field<int?>("LocationId"),
                        Location = r.Field<string>("Location"),
                        DepartmentId = r.Field<int?>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        SalaryDateFrom = r.Field<DateTime>("SalaryDateFrom"),
                        SalaryDateTo = r.Field<DateTime>("SalaryDateTo"),
                        PaidDays = r.Field<int?>("PaidDays"),
                        DonorName = r.Field<string>("DonorName"),
                        Company = r.Field<string>("Company"),
                        Project = r.Field<string>("Project"),
                        BasicAmount = r.Field<decimal>("BasicAmount"),
                        SalaryHeadId = r.Field<int>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        SalaryType = r.Field<string>("SalaryType"),
                        SalaryAmount = r.Field<decimal>("SalaryAmount"),
                        GrossAmount = r.Field<decimal>("GrossAmount"),
                        TotalAllowance = r.Field<decimal>("TotalAllowance"),
                        TotalDeduction = r.Field<decimal>("TotalDeduction"),
                        HomeTakenAmount = r.Field<decimal>("HomeTakenAmount"),
                        SalaryTypeRank = r.Field<int>("SalaryTypeRank"),
                        SalaryMonth = r.Field<string>("SalaryMonth"),
                        SalaryYearMonth = r.Field<string>("SalaryYearMonth"),
                        ShowJoiningDate = r.Field<string>("ShowJoiningDate"),
                        PayrollCurrencyId = r.Field<int>("PayrollCurrencyId"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        ConvertionRate = r.Field<decimal>("ConvertionRate"),
                        BasicAmountInEmployeeCurrency = r.Field<decimal>("BasicAmountInEmployeeCurrency")
                    }).ToList();

                }
            }
            return salarySheetList;
        }

        public List<SalarySheetBO> EmployeeSalarySheetForRedcross(int glCompanyId, string reportType, int employeeId, int departmentId, int gradeId, int branchId, DateTime processDateFrom, DateTime processDateTo, short processYear)
        {
            List<SalarySheetBO> salarySheetList = new List<SalarySheetBO>();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeeSalarySheetRedcross_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);

                    if (glCompanyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, glCompanyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, DBNull.Value);

                    if (employeeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, employeeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (gradeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, gradeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, DBNull.Value);

                    if (branchId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Branch", DbType.Int32, branchId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Branch", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, processDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, processDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.Int16, processYear);

                    DataSet salarySheetDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, salarySheetDS, "SalarySheet");

                    DataTable Table = salarySheetDS.Tables["SalarySheet"];

                    // ProcessId	EmpId	EmpCode	EmployeeName	Designation	LocationId	Location	
                    // DepartmentId	DepartmentName	SalaryDateFrom	SalaryDateTo	PaidDays	DonorId	
                    // Project	BasicAmount	SalaryHeadId	SalaryHead	SalaryType	SalaryAmount	GrossAmount	HomeTakenAmount

                    salarySheetList = Table.AsEnumerable().Select(r => new SalarySheetBO
                    {
                        ProcessId = r.Field<int>("ProcessId"),
                        EmpId = r.Field<int?>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        Designation = r.Field<string>("Designation"),
                        LocationId = r.Field<int?>("LocationId"),
                        Location = r.Field<string>("Location"),
                        DepartmentId = r.Field<int?>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        SalaryDateFrom = r.Field<DateTime?>("SalaryDateFrom"),
                        SalaryDateTo = r.Field<DateTime?>("SalaryDateTo"),
                        PaidDays = r.Field<int?>("PaidDays"),
                        Project = r.Field<string>("Project"),
                        BasicAmount = r.Field<decimal>("BasicAmount"),
                        SalaryHeadId = r.Field<int>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        SalaryType = r.Field<string>("SalaryType"),
                        SalaryAmount = r.Field<decimal>("SalaryAmount"),
                        GrossAmount = r.Field<decimal>("GrossAmount"),

                        MedicalAllowance = r.Field<decimal>("MedicalAllowance"),
                        NSSFEmployeeContribution = r.Field<decimal>("NSSFEmployeeContribution"),

                        TotalAllowance = r.Field<decimal>("TotalAllowance"),
                        TotalDeduction = r.Field<decimal>("TotalDeduction"),

                        TaxableIncome = r.Field<decimal>("TaxableIncome"),
                        HomeTakenAmount = r.Field<decimal>("HomeTakenAmount"),

                        SalaryTypeRank = r.Field<int>("SalaryTypeRank"),
                        SalaryMonth = r.Field<string>("SalaryMonth"),
                        SalaryYearMonth = r.Field<string>("SalaryYearMonth"),
                        ShowJoiningDate = r.Field<string>("ShowJoiningDate"),

                        SalaryEffectiveness = r.Field<string>("SalaryEffectiveness"),
                        PayHead = r.Field<string>("PayHead"),
                        PayHeadDescription = r.Field<string>("PayHeadDescription")

                    }).ToList();

                }
            }
            return salarySheetList;
        }
        public List<SalarySheetBO> EmployeeSalarySheetForSouthSudan(int glCompanyId, string reportType, int employeeId, int departmentId, int gradeId, int branchId, DateTime processDateFrom, DateTime processDateTo, short processYear)
        {
            List<SalarySheetBO> salarySheetList = new List<SalarySheetBO>();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                //using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeeSalarySheetRedcross_SP"))
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeeSalarySheetSouthSudan_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);

                    if (glCompanyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, glCompanyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, DBNull.Value);

                    if (employeeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, employeeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (gradeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, gradeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, DBNull.Value);

                    if (branchId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Branch", DbType.Int32, branchId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Branch", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, processDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, processDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.Int16, processYear);

                    DataSet salarySheetDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, salarySheetDS, "SalarySheet");

                    DataTable Table = salarySheetDS.Tables["SalarySheet"];

                    // ProcessId	EmpId	EmpCode	EmployeeName	Designation	LocationId	Location	
                    // DepartmentId	DepartmentName	SalaryDateFrom	SalaryDateTo	PaidDays	DonorId	
                    // Project	BasicAmount	SalaryHeadId	SalaryHead	SalaryType	SalaryAmount	GrossAmount	HomeTakenAmount

                    salarySheetList = Table.AsEnumerable().Select(r => new SalarySheetBO
                    {
                        ProcessId = r.Field<int>("ProcessId"),
                        EmpId = r.Field<int?>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        Designation = r.Field<string>("Designation"),
                        LocationId = r.Field<int?>("LocationId"),
                        Location = r.Field<string>("Location"),
                        DepartmentId = r.Field<int?>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        SalaryDateFrom = r.Field<DateTime?>("SalaryDateFrom"),
                        SalaryDateTo = r.Field<DateTime?>("SalaryDateTo"),
                        PaidDays = r.Field<int?>("PaidDays"),
                        Project = r.Field<string>("Project"),
                        BasicAmount = r.Field<decimal>("BasicAmount"),
                        SalaryHeadId = r.Field<int>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        SalaryType = r.Field<string>("SalaryType"),
                        SalaryAmount = r.Field<decimal>("SalaryAmount"),
                        GrossAmount = r.Field<decimal>("GrossAmount"),

                        MedicalAllowance = r.Field<decimal>("MedicalAllowance"),
                        NSSFEmployeeContribution = r.Field<decimal>("NSSFEmployeeContribution"),

                        TotalAllowance = r.Field<decimal>("TotalAllowance"),
                        TotalDeduction = r.Field<decimal>("TotalDeduction"),

                        TaxableIncome = r.Field<decimal>("TaxableIncome"),
                        HomeTakenAmount = r.Field<decimal>("HomeTakenAmount"),

                        SalaryTypeRank = r.Field<int>("SalaryTypeRank"),
                        SalaryMonth = r.Field<string>("SalaryMonth"),
                        SalaryYearMonth = r.Field<string>("SalaryYearMonth"),
                        ShowJoiningDate = r.Field<string>("ShowJoiningDate"),

                        SalaryEffectiveness = r.Field<string>("SalaryEffectiveness"),
                        PayHead = r.Field<string>("PayHead"),
                        PayHeadDescription = r.Field<string>("PayHeadDescription")

                    }).ToList();

                }
            }
            return salarySheetList;
        }

        public List<SalarySheetBO> EmployeeSalarySheetForIPTech(int glCompanyId, int employeeId, int departmentId, int gradeId, int branchId, DateTime processDateFrom, DateTime processDateTo, short processYear, string currencyType)
        {
            List<SalarySheetBO> salarySheetList = new List<SalarySheetBO>();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeeSalarySheetForIPTech_SP"))
                {
                    if (glCompanyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, glCompanyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, DBNull.Value);

                    if (employeeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, employeeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (gradeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, gradeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, DBNull.Value);

                    if (branchId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Branch", DbType.Int32, branchId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Branch", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@CurrencyType", DbType.String, currencyType);

                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, processDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, processDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.Int16, processYear);

                    DataSet salarySheetDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, salarySheetDS, "SalarySheet");

                    DataTable Table = salarySheetDS.Tables["SalarySheet"];

                    // ProcessId	EmpId	EmpCode	EmployeeName	Designation	LocationId	Location	
                    // DepartmentId	DepartmentName	SalaryDateFrom	SalaryDateTo	PaidDays	DonorId	
                    // Project	BasicAmount	SalaryHeadId	SalaryHead	SalaryType	SalaryAmount	GrossAmount	HomeTakenAmount

                    salarySheetList = Table.AsEnumerable().Select(r => new SalarySheetBO
                    {
                        ProcessId = r.Field<int>("ProcessId"),
                        EmpId = r.Field<int?>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        Designation = r.Field<string>("Designation"),
                        LocationId = r.Field<int?>("LocationId"),
                        Location = r.Field<string>("Location"),
                        DepartmentId = r.Field<int?>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        SalaryDateFrom = r.Field<DateTime>("SalaryDateFrom"),
                        SalaryDateTo = r.Field<DateTime>("SalaryDateTo"),
                        PaidDays = r.Field<int?>("PaidDays"),
                        Project = r.Field<string>("Project"),
                        BasicAmount = r.Field<decimal>("BasicAmount"),
                        SalaryHeadId = r.Field<int>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        SalaryType = r.Field<string>("SalaryType"),
                        SalaryAmount = r.Field<decimal>("SalaryAmount"),
                        GrossAmount = r.Field<decimal>("GrossAmount"),

                        TotalAllowance = r.Field<decimal>("TotalAllowance"),
                        TotalDeduction = r.Field<decimal>("TotalDeduction"),

                        HomeTakenAmount = r.Field<decimal>("HomeTakenAmount"),
                        SalaryTypeRank = r.Field<int>("SalaryTypeRank"),
                        SalaryMonth = r.Field<string>("SalaryMonth"),
                        SalaryYearMonth = r.Field<string>("SalaryYearMonth"),
                        ShowJoiningDate = r.Field<string>("ShowJoiningDate"),

                        PayrollCurrencyId = r.Field<int>("PayrollCurrencyId"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        ConvertionRate = r.Field<decimal>("ConvertionRate"),
                        BasicAmountInEmployeeCurrency = r.Field<decimal>("BasicAmountInEmployeeCurrency")

                    }).ToList();

                }
            }
            return salarySheetList;
        }


        public List<SalarySheetBO> EmployeeSalarySheetsTemp(int processId, DateTime processDateFrom, DateTime processDateTo, short processYear, string currencyType)
        {
            List<SalarySheetBO> salarySheetList = new List<SalarySheetBO>();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeeSalarySheetTemp_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessId", DbType.Int32, processId);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.Int16, processYear);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, processDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, processDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@CurrencyType", DbType.String, currencyType);

                    DataSet salarySheetDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, salarySheetDS, "SalarySheet");

                    DataTable Table = salarySheetDS.Tables["SalarySheet"];

                    salarySheetList = Table.AsEnumerable().Select(r => new SalarySheetBO
                    {
                        ProcessId = r.Field<int>("ProcessId"),
                        EmpId = r.Field<int?>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        Designation = r.Field<string>("Designation"),
                        LocationId = r.Field<int?>("LocationId"),
                        Location = r.Field<string>("Location"),
                        DepartmentId = r.Field<int?>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        SalaryDateFrom = r.Field<DateTime>("SalaryDateFrom"),
                        SalaryDateTo = r.Field<DateTime>("SalaryDateTo"),
                        PaidDays = r.Field<int?>("PaidDays"),
                        DonorName = r.Field<string>("DonorName"),
                        Company = r.Field<string>("Company"),
                        Project = r.Field<string>("Project"),
                        BasicAmount = r.Field<decimal>("BasicAmount"),
                        SalaryHeadId = r.Field<int>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        SalaryType = r.Field<string>("SalaryType"),
                        SalaryAmount = r.Field<decimal>("SalaryAmount"),
                        GrossAmount = r.Field<decimal>("GrossAmount"),
                        TotalAllowance = r.Field<decimal>("TotalAllowance"),
                        TotalDeduction = r.Field<decimal>("TotalDeduction"),
                        HomeTakenAmount = r.Field<decimal>("HomeTakenAmount"),
                        SalaryTypeRank = r.Field<int>("SalaryTypeRank"),
                        PayrollCurrencyId = r.Field<int>("PayrollCurrencyId"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        ConvertionRate = r.Field<decimal>("ConvertionRate"),
                        BasicAmountInEmployeeCurrency = r.Field<decimal>("BasicAmountInEmployeeCurrency"),
                        ShowJoiningDate = r.Field<string>("ShowJoiningDate"),
                    }).ToList();

                }
            }
            return salarySheetList;
        }

        public List<SalarySheetBO> EmployeeSalarySheetsTempForRedcross(int processId, DateTime processDateFrom, DateTime processDateTo, short processYear)
        {
            List<SalarySheetBO> salarySheetList = new List<SalarySheetBO>();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeeSalarySheetForRedcrossTemp_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProcessId", DbType.Int32, processId);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.Int16, processYear);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, processDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, processDateTo);

                    DataSet salarySheetDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, salarySheetDS, "SalarySheet");

                    DataTable Table = salarySheetDS.Tables["SalarySheet"];

                    // ProcessId	EmpId	EmpCode	EmployeeName	Designation	LocationId	Location	
                    // DepartmentId	DepartmentName	SalaryDateFrom	SalaryDateTo	PaidDays	DonorId	
                    // Project	BasicAmount	SalaryHeadId	SalaryHead	SalaryType	SalaryAmount	GrossAmount	HomeTakenAmount

                    salarySheetList = Table.AsEnumerable().Select(r => new SalarySheetBO
                    {
                        ProcessId = r.Field<int>("ProcessId"),
                        EmpId = r.Field<int?>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        Designation = r.Field<string>("Designation"),
                        LocationId = r.Field<int?>("LocationId"),
                        Location = r.Field<string>("Location"),
                        DepartmentId = r.Field<int?>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        SalaryDateFrom = r.Field<DateTime>("SalaryDateFrom"),
                        SalaryDateTo = r.Field<DateTime>("SalaryDateTo"),
                        PaidDays = r.Field<int?>("PaidDays"),
                        Project = r.Field<string>("Project"),
                        BasicAmount = r.Field<decimal>("BasicAmount"),
                        SalaryHeadId = r.Field<int>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        SalaryType = r.Field<string>("SalaryType"),
                        SalaryAmount = r.Field<decimal>("SalaryAmount"),
                        GrossAmount = r.Field<decimal>("GrossAmount"),

                        MedicalAllowance = r.Field<decimal>("MedicalAllowance"),
                        NSSFEmployeeContribution = r.Field<decimal>("NSSFEmployeeContribution"),

                        TotalAllowance = r.Field<decimal>("TotalAllowance"),
                        TotalDeduction = r.Field<decimal>("TotalDeduction"),

                        TaxableIncome = r.Field<decimal>("TaxableIncome"),
                        HomeTakenAmount = r.Field<decimal>("HomeTakenAmount"),

                        SalaryTypeRank = r.Field<int>("SalaryTypeRank"),
                        SalaryMonth = r.Field<string>("SalaryMonth"),
                        SalaryYearMonth = r.Field<string>("SalaryYearMonth"),
                        ShowJoiningDate = r.Field<string>("ShowJoiningDate"),

                        SalaryEffectiveness = r.Field<string>("SalaryEffectiveness"),
                        PayHead = r.Field<string>("PayHead"),
                        PayHeadDescription = r.Field<string>("PayHeadDescription")

                    }).ToList();

                }
            }
            return salarySheetList;
        }
        public List<SalarySheetBO> EmployeeSalarySheetsTempForSouthSudan(int processId, DateTime processDateFrom, DateTime processDateTo, short processYear)
        {
            List<SalarySheetBO> salarySheetList = new List<SalarySheetBO>();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeeSalarySheetForSouthSudanTemp_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProcessId", DbType.Int32, processId);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.Int16, processYear);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, processDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, processDateTo);

                    DataSet salarySheetDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, salarySheetDS, "SalarySheet");

                    DataTable Table = salarySheetDS.Tables["SalarySheet"];

                    // ProcessId	EmpId	EmpCode	EmployeeName	Designation	LocationId	Location	
                    // DepartmentId	DepartmentName	SalaryDateFrom	SalaryDateTo	PaidDays	DonorId	
                    // Project	BasicAmount	SalaryHeadId	SalaryHead	SalaryType	SalaryAmount	GrossAmount	HomeTakenAmount

                    salarySheetList = Table.AsEnumerable().Select(r => new SalarySheetBO
                    {
                        ProcessId = r.Field<int>("ProcessId"),
                        EmpId = r.Field<int?>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        Designation = r.Field<string>("Designation"),
                        LocationId = r.Field<int?>("LocationId"),
                        Location = r.Field<string>("Location"),
                        DepartmentId = r.Field<int?>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        SalaryDateFrom = r.Field<DateTime>("SalaryDateFrom"),
                        SalaryDateTo = r.Field<DateTime>("SalaryDateTo"),
                        PaidDays = r.Field<int?>("PaidDays"),
                        Project = r.Field<string>("Project"),
                        BasicAmount = r.Field<decimal>("BasicAmount"),
                        SalaryHeadId = r.Field<int>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        SalaryType = r.Field<string>("SalaryType"),
                        SalaryAmount = r.Field<decimal>("SalaryAmount"),
                        GrossAmount = r.Field<decimal>("GrossAmount"),

                        MedicalAllowance = r.Field<decimal>("MedicalAllowance"),
                        NSSFEmployeeContribution = r.Field<decimal>("NSSFEmployeeContribution"),

                        TotalAllowance = r.Field<decimal>("TotalAllowance"),
                        TotalDeduction = r.Field<decimal>("TotalDeduction"),

                        TaxableIncome = r.Field<decimal>("TaxableIncome"),
                        HomeTakenAmount = r.Field<decimal>("HomeTakenAmount"),

                        SalaryTypeRank = r.Field<int>("SalaryTypeRank"),
                        SalaryMonth = r.Field<string>("SalaryMonth"),
                        SalaryYearMonth = r.Field<string>("SalaryYearMonth"),
                        ShowJoiningDate = r.Field<string>("ShowJoiningDate"),

                        SalaryEffectiveness = r.Field<string>("SalaryEffectiveness"),
                        PayHead = r.Field<string>("PayHead"),
                        PayHeadDescription = r.Field<string>("PayHeadDescription")

                    }).ToList();

                }
            }
            return salarySheetList;
        }

        public List<SalarySheetBO> EmployeeSalarySheetsTempForIPTech(int processId, DateTime processDateFrom, DateTime processDateTo, short processYear, string currencyType)
        {
            List<SalarySheetBO> salarySheetList = new List<SalarySheetBO>();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeeSalarySheetTempForIPTech_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProcessId", DbType.Int32, processId);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.Int16, processYear);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, processDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, processDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@CurrencyType", DbType.String, currencyType);

                    DataSet salarySheetDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, salarySheetDS, "SalarySheet");

                    DataTable Table = salarySheetDS.Tables["SalarySheet"];

                    salarySheetList = Table.AsEnumerable().Select(r => new SalarySheetBO
                    {
                        ProcessId = r.Field<int>("ProcessId"),
                        EmpId = r.Field<int?>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        Designation = r.Field<string>("Designation"),
                        LocationId = r.Field<int?>("LocationId"),
                        Location = r.Field<string>("Location"),
                        DepartmentId = r.Field<int?>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        SalaryDateFrom = r.Field<DateTime>("SalaryDateFrom"),
                        SalaryDateTo = r.Field<DateTime>("SalaryDateTo"),
                        PaidDays = r.Field<int?>("PaidDays"),
                        Project = r.Field<string>("Project"),
                        BasicAmount = r.Field<decimal>("BasicAmount"),
                        SalaryHeadId = r.Field<int>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        SalaryType = r.Field<string>("SalaryType"),
                        SalaryAmount = r.Field<decimal>("SalaryAmount"),
                        GrossAmount = r.Field<decimal>("GrossAmount"),

                        TotalAllowance = r.Field<decimal>("TotalAllowance"),
                        TotalDeduction = r.Field<decimal>("TotalDeduction"),

                        HomeTakenAmount = r.Field<decimal>("HomeTakenAmount"),
                        SalaryTypeRank = r.Field<int>("SalaryTypeRank"),

                        PayrollCurrencyId = r.Field<int>("PayrollCurrencyId"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        ConvertionRate = r.Field<decimal>("ConvertionRate"),
                        BasicAmountInEmployeeCurrency = r.Field<decimal>("BasicAmountInEmployeeCurrency")

                    }).ToList();

                }
            }
            return salarySheetList;
        }


        public List<EmpReconsilationViewBO> GetEmpReconsilation(DateTime salaryDateFrom, DateTime salaryDateTo, int salaryYear, bool IsManagement)
        {
            List<EmpReconsilationViewBO> boList = new List<EmpReconsilationViewBO>();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("ReconcilationReportsForEmployee_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@IsManagement", DbType.Boolean, IsManagement);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryDateFrom", DbType.DateTime, salaryDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryDateTo", DbType.DateTime, salaryDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryYear", DbType.Int32, salaryYear);

                    DataSet salarySheetDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, salarySheetDS, "EmpReconsilation");

                    DataTable Table = salarySheetDS.Tables["EmpReconsilation"];

                    boList = Table.AsEnumerable().Select(r => new EmpReconsilationViewBO
                    {
                        SalaryHeadId = r.Field<int>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        SalaryAmount = r.Field<decimal>("SalaryAmount"),
                        TotoalAllowance = r.Field<decimal>("TotoalAllowance"),
                        TotalDeduction = r.Field<decimal>("TotalDeduction"),
                        NetSalary = r.Field<decimal>("NetSalary"),
                        TotalEmployee = r.Field<int>("TotalEmployee")

                    }).ToList();

                }
            }
            return boList;
        }

        public List<EmpReconsilationBankViewBO> GetEmpReconsilationBankInfo(int bankId, DateTime salaryDateFrom, DateTime salaryDateTo, int salaryYear, bool IsManagement)
        {
            List<EmpReconsilationBankViewBO> boList = new List<EmpReconsilationBankViewBO>();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBankWiseReconciliationTotal_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@IsManagement", DbType.Boolean, IsManagement);
                    if (bankId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@BankId", DbType.Int32, bankId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@BankId", DbType.Int32, DBNull.Value);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, salaryDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, salaryDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryYear", DbType.Int32, salaryYear);

                    DataSet salarySheetDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, salarySheetDS, "EmpReconsilationBankInfo");

                    DataTable Table = salarySheetDS.Tables["EmpReconsilationBankInfo"];

                    boList = Table.AsEnumerable().Select(r => new EmpReconsilationBankViewBO
                    {
                        BankId = r.Field<int>("BankId"),
                        BankName = r.Field<string>("BankName"),
                        TotalAmount = r.Field<decimal>("TotalAmount")

                    }).ToList();

                }
            }
            return boList;
        }

        public List<CostCenterWiseReconcilationSummaryViewBO> GetCostcenterWiseReconsilationSummary(DateTime salaryDateFrom, DateTime salaryDateTo, int salaryYear, bool IsManagement)
        {
            List<CostCenterWiseReconcilationSummaryViewBO> boList = new List<CostCenterWiseReconcilationSummaryViewBO>();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("CostcenterWiseReconcilationSummary_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@IsManagement", DbType.Boolean, IsManagement);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryDateFrom", DbType.DateTime, salaryDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryDateTo", DbType.DateTime, salaryDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryYear", DbType.Int32, salaryYear);

                    DataSet salarySheetDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, salarySheetDS, "CostcenterwiseSummary");

                    DataTable Table = salarySheetDS.Tables["CostcenterwiseSummary"];

                    boList = Table.AsEnumerable().Select(r => new CostCenterWiseReconcilationSummaryViewBO
                    {
                        SalaryHeadId = r.Field<int>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        SalaryAmount = r.Field<decimal>("SalaryAmount"),
                        TotoalAllowance = r.Field<decimal>("TotoalAllowance"),
                        TotalDeduction = r.Field<decimal>("TotalDeduction"),
                        NetSalary = r.Field<decimal>("NetSalary"),
                        TotalEmployee = r.Field<int>("TotalEmployee"),
                        SalaryAmountYtd = r.Field<decimal>("SalaryAmountYtd"),
                        TotoalAllowanceYtd = r.Field<decimal>("TotoalAllowanceYtd"),
                        TotalDeductionYtd = r.Field<decimal>("TotalDeductionYtd"),
                        NetSalaryYtd = r.Field<decimal>("NetSalaryYtd")

                    }).ToList();

                }
            }
            return boList;
        }
    }
}