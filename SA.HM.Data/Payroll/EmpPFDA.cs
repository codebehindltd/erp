using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpPFDA : BaseService
    {
        public Boolean SaveEmpPFInformation(PFSettingBO pfInfo, string pfEmployeeContId, string pfCompanyContId, string pfCompanyContributionOn, out int tmpId)
        {
            Boolean status = false;
            tmpId = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpPFInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@EmpContributionInPercentage", DbType.Decimal, pfInfo.EmployeeContributionInPercentage);
                        dbSmartAspects.AddInParameter(command, "@CompanyContributionInPercentange", DbType.Decimal, pfInfo.CompanyContributionInPercentange);
                        dbSmartAspects.AddInParameter(command, "@EmpCanContributeMaxOfBasicSalary", DbType.Decimal, pfInfo.EmployeeCanContributeMaxOfBasicSalary);
                        dbSmartAspects.AddInParameter(command, "@InterestDistributionRate", DbType.Decimal, pfInfo.InterestDistributionRate);
                        dbSmartAspects.AddInParameter(command, "@CompanyContributionElegibilityYear", DbType.Int32, pfInfo.CompanyContributionEligibilityYear);
                        //dbSmartAspects.AddInParameter(command, "@InterestDistributionRate", DbType.Decimal, pfInfo.InterestDistributionRate);
                        //dbSmartAspects.AddInParameter(command, "@CompanyContributionElegibilityYear", DbType.Int32, pfInfo.CompanyContributionEligibilityYear);
                        dbSmartAspects.AddInParameter(command, "@PFEmployeeContId", DbType.String, pfEmployeeContId);
                        dbSmartAspects.AddInParameter(command, "@PFCompanyContId", DbType.String, pfCompanyContId);
                        dbSmartAspects.AddInParameter(command, "@PFCompanyContributionOn", DbType.String, pfCompanyContributionOn);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, pfInfo.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@PFSettingId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpId = Convert.ToInt32(command.Parameters["@PFSettingId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean UpdateEmpPFInfo(PFSettingBO pfInfo, string pfEmployeeContId, string pfCompanyContId, string pfCompanyContributionOn)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpPFInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@PFSettingId", DbType.Int32, pfInfo.PFSettingId);
                        dbSmartAspects.AddInParameter(command, "@EmpContributionInPercentage", DbType.Decimal, pfInfo.EmployeeContributionInPercentage);
                        dbSmartAspects.AddInParameter(command, "@CompanyContributionInPercentange", DbType.Decimal, pfInfo.CompanyContributionInPercentange);
                        dbSmartAspects.AddInParameter(command, "@EmpCanContributeMaxOfBasicSalary", DbType.Decimal, pfInfo.EmployeeCanContributeMaxOfBasicSalary);
                        dbSmartAspects.AddInParameter(command, "@InterestDistributionRate", DbType.Decimal, pfInfo.InterestDistributionRate);
                        dbSmartAspects.AddInParameter(command, "@CompanyContributionEligibilityYear", DbType.Int32, pfInfo.CompanyContributionEligibilityYear);
                        dbSmartAspects.AddInParameter(command, "@PFEmployeeContId", DbType.String, pfEmployeeContId);
                        dbSmartAspects.AddInParameter(command, "@PFCompanyContId", DbType.String, pfCompanyContId);
                        dbSmartAspects.AddInParameter(command, "@PFCompanyContributionOn", DbType.String, pfCompanyContributionOn);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, pfInfo.LastModifiedBy);

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
        public PFSettingBO GetEmpPFInformation()
        {
            PFSettingBO pfInfo = new PFSettingBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpPFInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                pfInfo.PFSettingId = Convert.ToInt32(reader["PFSettingId"]);
                                pfInfo.EmployeeContributionInPercentage = Convert.ToDecimal(reader["EmpContributionInPercentage"]);
                                pfInfo.CompanyContributionInPercentange = Convert.ToDecimal(reader["CompanyContributionInPercentange"]);
                                pfInfo.EmployeeCanContributeMaxOfBasicSalary = Convert.ToDecimal(reader["EmpCanContributeMaxOfBasicSalary"]);
                                pfInfo.InterestDistributionRate = Convert.ToDecimal(reader["InterestDistributionRate"]);
                                pfInfo.CompanyContributionEligibilityYear = Convert.ToInt32(reader["CompanyContributionEligibilityYear"]);
                            }
                        }
                    }
                }
            }
            return pfInfo;
        }

        public Boolean ApprovePF(int empId, DateTime? pfEligibilityDate)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ApprovePF_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, empId);
                        dbSmartAspects.AddInParameter(command, "@PFEligibilityDate", DbType.DateTime, pfEligibilityDate);

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
        public Boolean UpdateEmpForPFTermination(int empId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpForPFTermination_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, empId);

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
        public List<PFReportViewBO> GetPFInfoForReport(string empId, string departmentId, DateTime? salaryDateFrom, DateTime? salaryDateTo, string reportType)
        {
            List<PFReportViewBO> viewlist = new List<PFReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPFInfoForReport_SP"))
                {

                    if (!string.IsNullOrEmpty(empId))
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, Convert.ToInt32(empId));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(departmentId))
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, Convert.ToInt32(departmentId));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (salaryDateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@SalaryDateFrom", DbType.DateTime, Convert.ToDateTime(salaryDateFrom));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SalaryDateFrom", DbType.DateTime, DBNull.Value);

                    if (salaryDateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@SalaryDateTo", DbType.DateTime, Convert.ToDateTime(salaryDateTo));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SalaryDateTo", DbType.DateTime, DBNull.Value);

                    if (!string.IsNullOrEmpty(reportType))
                        dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PFReport");
                    DataTable Table = ds.Tables["PFReport"];

                    viewlist = Table.AsEnumerable().Select(r => new PFReportViewBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        SalaryHeadId = r.Field<int>("SalaryHeadId"),
                        DisplayName = r.Field<String>("DisplayName"),
                        SalaryHead = r.Field<String>("SalaryHead"),
                        SalaryHeadAmount = r.Field<decimal>("SalaryAmount"),
                        //SalaryHeadNote = r.Field<String>("SalaryHeadNote"),
                        ProcessDate = r.Field<DateTime>("ProcessDate")

                    }).ToList();
                }
            }
            return viewlist;

        }

        public List<EmpPFBO> GetAllPFMember()
        {
            List<EmpPFBO> boList = new List<EmpPFBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllPFMember_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpPF");
                    DataTable Table = ds.Tables["EmpPF"];

                    boList = Table.AsEnumerable().Select(r => new EmpPFBO
                    {
                        PFCollectionId = r.Field<Int64>("PFCollectionId"),
                        EmpId = r.Field<Int32>("EmpId")
                    }).ToList();
                }
            }
            return boList;
        }
        public List<PFMemberReportViewBO> GetActivePFMemberList(string memberType)
        {
            List<PFMemberReportViewBO> boList = new List<PFMemberReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActivePFMemberList_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberType", DbType.String, memberType);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpPF");
                    DataTable Table = ds.Tables["EmpPF"];

                    boList = Table.AsEnumerable().Select(r => new PFMemberReportViewBO
                    {
                        Name = r.Field<string>("Name"),
                        Id = r.Field<string>("Id"),
                        Department = r.Field<string>("Department"),
                        Basic = r.Field<decimal?>("Basic"),
                        PFPercent = r.Field<decimal?>("PFPercent"),
                        PFAmount = r.Field<decimal?>("PFAmount")
                    }).ToList();
                }
            }
            return boList;
        }
        public List<PFStatementReportViewBO> GetPFStatementReportInfo(DateTime processDate, int year, int departmentId, int empId)
        {
            List<PFStatementReportViewBO> viewlist = new List<PFStatementReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPFStatementYearlyReport_SP"))
                {

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (processDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@PFToDate", DbType.DateTime, processDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PFFromDate", DbType.DateTime, DBNull.Value);

                    if (year != null)
                        dbSmartAspects.AddInParameter(cmd, "@PFYear", DbType.Int32, year);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PFYear", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PFReport");
                    DataTable Table = ds.Tables["PFReport"];

                    viewlist = Table.AsEnumerable().Select(r => new PFStatementReportViewBO
                    {
                        PFCollectionId = r.Field<long>("PFCollectionId"),
                        EmpId = r.Field<Int32>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        DisplayName = r.Field<string>("DisplayName"),
                        Department = r.Field<string>("Department"),
                        Designation = r.Field<string>("Designation"),
                        ProcessDate = r.Field<string>("ProcessDate"),
                        EmpContribution = r.Field<decimal?>("EmpContribution"),
                        CompanyContribution = r.Field<decimal?>("CompanyContribution"),
                        CommulativePFAmountCurrentYear = r.Field<decimal?>("CommulativePFAmountCurrentYear"),
                        CurrentYearBalance = r.Field<decimal?>("CurrentYearBalance"),
                        PreviousYearBalance = r.Field<decimal?>("PreviousYearBalance"),
                        Interest = r.Field<decimal?>("Interest"),
                        ShowPFEligibilityDate = r.Field<string>("ShowPFEligibilityDate")

                        //InterestRate = r.Field<decimal?>("InterestRate"),
                        //TotalAmount = r.Field<decimal?>("TotalAmount"),

                    }).ToList();
                }
            }
            return viewlist;

        }

        public List<PFMonthlyTotalAmountViewBO> GetMonthlyTotalPFAmount(DateTime? fromDate, DateTime? toDate)
        {
            List<PFMonthlyTotalAmountViewBO> boList = new List<PFMonthlyTotalAmountViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMonthlyTotalPFAmount_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "MonthlyPF");
                    DataTable Table = ds.Tables["MonthlyPF"];

                    boList = Table.AsEnumerable().Select(r => new PFMonthlyTotalAmountViewBO
                    {
                        Month = r.Field<string>("Month"),
                        Department = r.Field<string>("Department"),
                        EmpContribution = r.Field<decimal?>("EmpContribution"),
                        CmpContribution = r.Field<decimal?>("CmpContribution"),
                        TotalAmount = r.Field<decimal?>("TotalAmount")
                    }).ToList();
                }
            }
            return boList;
        }
        public Boolean SaveEmpPFOpeningBalance(List<EmpPFBO> pfInfo, Int16 year)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    foreach (var bo in pfInfo)
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpPFOpeningBalance_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, bo.EmpId);
                            dbSmartAspects.AddInParameter(command, "@EmpContribution", DbType.Decimal, bo.EmployeeContribution);
                            dbSmartAspects.AddInParameter(command, "@CompanyContribution", DbType.Decimal, bo.CompanyContribution);
                            dbSmartAspects.AddInParameter(command, "@ProvidentFundInterest", DbType.Decimal, bo.ProvidentFundInterest);
                            dbSmartAspects.AddInParameter(command, "@PFDateFrom", DbType.DateTime, bo.PFDateFrom);
                            dbSmartAspects.AddInParameter(command, "@PFDateTo", DbType.DateTime, bo.PFDateTo);
                            dbSmartAspects.AddInParameter(command, "@PFYear", DbType.Int16, year);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bo.CreatedBy);

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

        public List<PFSummaryReportViewBO> GetPFSummaryReport(Int32 year)
        {
            List<PFSummaryReportViewBO> boList = new List<PFSummaryReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPFSummaryReport_SP"))
                {                    
                    dbSmartAspects.AddInParameter(cmd, "@Year", DbType.Int32, year);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PFSummary");
                    DataTable Table = ds.Tables["PFSummary"];

                    boList = Table.AsEnumerable().Select(r => new PFSummaryReportViewBO
                    {                        
                        EmpId = r.Field<int>("EmpId"),
                        EmployeeName = r.Field<string>("EmployeeName"),                        
                        EmpCode = r.Field<string>("EmpCode"),                        
                        OpeningBalance = r.Field<decimal?>("OpeningBalance"),
                        EmpContribution = r.Field<decimal?>("EmpContribution"),
                        EmpInterest = r.Field<decimal?>("EmpInterest"),
                        CompanyContribution = r.Field<decimal?>("CompanyContribution"),
                        CompanyInterest = r.Field<decimal?>("CompanyInterest"),
                        YearTotal = r.Field<decimal?>("YearTotal"),
                        EndBalance = r.Field<decimal?>("EndBalance")

                    }).ToList();
                }
            }
            return boList;
        }
        public List<PFMonthlyStaffListViewBO> GetPFMonthlyStaffListReport(int year, int month)
        {
            List<PFMonthlyStaffListViewBO> boList = new List<PFMonthlyStaffListViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMonthlyPFStaffListForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Year", DbType.Int32, year);
                    dbSmartAspects.AddInParameter(cmd, "@Month", DbType.Int32, month);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PFMonthlyList");
                    DataTable Table = ds.Tables["PFMonthlyList"];

                    boList = Table.AsEnumerable().Select(r => new PFMonthlyStaffListViewBO
                    {
                        //PFCollectionId = r.Field<long>("PFCollectionId"),
                        EmpId = r.Field<int>("EmpId"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        MonthNo = r.Field<int>("MonthNo"),
                        PFMonthName = r.Field<string>("PFMonthName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        OpeningBalance = r.Field<decimal?>("OpeningBalance"),
                        CommulativePFAmount = r.Field<decimal?>("CommulativePFAmount"),
                        EmpContribution = r.Field<decimal?>("EmpContribution"),
                        CompanyContribution = r.Field<decimal?>("CompanyContribution")

                    }).ToList();
                }
            }
            return boList;
        }
        public List<PFProductCalculationViewBO> GetPFProductCalculationReport(Int32 year)
        {
            List<PFProductCalculationViewBO> boList = new List<PFProductCalculationViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPFProductCalculationReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Year", DbType.Int32, year);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PFProductCalculation");
                    DataTable Table = ds.Tables["PFProductCalculation"];

                    boList = Table.AsEnumerable().Select(r => new PFProductCalculationViewBO
                    {
                        EmpId = r.Field<int>("EmpId"),                       
                        EmpCode = r.Field<string>("EmpCode"),
                        EmpName = r.Field<string>("EmpName"),
                        OpeningBalance = r.Field<decimal?>("OpeningBalance"),
                        PFTotal = r.Field<decimal?>("PFTotal"),
                        TotalProduct = r.Field<decimal?>("TotalProduct")                        

                    }).ToList();
                }
            }
            return boList;
        }

        public List<PFProductCalculationViewBO> GetPFInterestCalculationReport(Int32 year, decimal interest)
        {
            List<PFProductCalculationViewBO> boList = new List<PFProductCalculationViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPFInterestCalculationReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Year", DbType.Int32, year);
                    dbSmartAspects.AddInParameter(cmd, "@Interest", DbType.Int32, interest);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PFInterestCalculation");
                    DataTable Table = ds.Tables["PFInterestCalculation"];

                    boList = Table.AsEnumerable().Select(r => new PFProductCalculationViewBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmpName = r.Field<string>("EmpName"),
                        OpeningBalance = r.Field<decimal?>("OpeningBalance"),
                        PFTotal = r.Field<decimal?>("PFTotal"),
                        TotalProduct = r.Field<decimal?>("TotalProduct"),
                        Interest = r.Field<decimal?>("Interest")

                    }).ToList();
                }
            }
            return boList;
        }
        public List<PFReportViewBO> GetEmpPFForProfile(int empId)
        {
            List<PFReportViewBO> boList = new List<PFReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpPFForProfile_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);                    

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PFfrProfile");
                    DataTable Table = ds.Tables["PFfrProfile"];

                    boList = Table.AsEnumerable().Select(r => new PFReportViewBO
                    {                        
                        ShowPFEligibilityDate = r.Field<string>("ShowPFEligibilityDate"),
                        MonthlyAmount = r.Field<decimal?>("MonthlyAmount"),
                        TotalBalance = r.Field<decimal?>("TotalBalance"),
                        MonthName = r.Field<string>("MonthName")

                    }).ToList();
                }
            }
            return boList;
        }
    }
}
