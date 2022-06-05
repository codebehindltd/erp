using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class StaffingBudgetDA : BaseService
    {
        public List<PayrollStaffRequisitionDetailsBO> GetStaffRequisitionDetails()
        {
            List<PayrollStaffRequisitionDetailsBO> viewlist = new List<PayrollStaffRequisitionDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetStaffRequisitionDetails_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "StaffRequisition");
                    DataTable Table = ds.Tables["StaffRequisition"];

                    viewlist = Table.AsEnumerable().Select(r => new PayrollStaffRequisitionDetailsBO
                    {
                        StaffRequisitionDetailsId = r.Field<Int64>("StaffRequisitionDetailsId"),
                        StaffRequisitionId = r.Field<Int64>("StaffRequisitionId"),
                        RequisitionDescription = r.Field<string>("RequisitionDescription")
                    }).ToList();
                }
            }
            return viewlist;
        }
        public List<PayrollStaffRequisitionDetailsBO> GetApprovedStaffRequisitionDetails()
        {
            List<PayrollStaffRequisitionDetailsBO> viewlist = new List<PayrollStaffRequisitionDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApprovedStaffRequisitionDetails_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "StaffRequisition");
                    DataTable Table = ds.Tables["StaffRequisition"];

                    viewlist = Table.AsEnumerable().Select(r => new PayrollStaffRequisitionDetailsBO
                    {
                        StaffRequisitionDetailsId = r.Field<Int64>("StaffRequisitionDetailsId"),
                        StaffRequisitionId = r.Field<Int64>("StaffRequisitionId"),
                        RequisitionDescription = r.Field<string>("RequisitionDescription")
                    }).ToList();
                }
            }
            return viewlist;
        }
        public PayrollStaffRequisitionDetailsBO GetStaffRequisitionDetailsById(int staffRequisitionDetailsId)
        {
            PayrollStaffRequisitionDetailsBO bank = new PayrollStaffRequisitionDetailsBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetStaffRequisitionDetailsById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@StaffRequisitionDetailsId", DbType.Int32, staffRequisitionDetailsId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bank.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                bank.StaffRequisitionDetailsId = Convert.ToInt32(reader["StaffRequisitionDetailsId"]);
                                bank.StaffRequisitionId = Convert.ToInt32(reader["StaffRequisitionId"]);
                                bank.JobType = Convert.ToInt32(reader["JobType"]);
                                bank.JobLevel = reader["JobLevel"].ToString();
                                bank.RequisitionQuantity = Convert.ToInt16(reader["RequisitionQuantity"]);
                                bank.DemandDateString = reader["DemandDateString"].ToString();
                            }
                        }
                    }
                }
            }
            return bank;
        }
        public Boolean SaveStaffingBudget(PayrollStaffingBudgetBO StaffingBudget, List<PayrollStaffingBudgeDetailsBO> NewlyAddedStaffingBudgetDetails, out long StaffingBudgetId)
        {
            Boolean status = false;
            //Int64 StaffingBudgetId = 0;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePayrollStaffingBudget_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, StaffingBudget.DepartmentId);
                                dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, StaffingBudget.ApprovedStatus);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, StaffingBudget.CreatedBy);
                                dbSmartAspects.AddOutParameter(command, "@StaffingBudgetId", DbType.Int64, sizeof(Int64));

                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                                StaffingBudgetId = Convert.ToInt32(command.Parameters["@StaffingBudgetId"].Value);
                            }

                            if (status)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePayrollStaffingBudgeDetails_SP"))
                                {
                                    foreach (PayrollStaffingBudgeDetailsBO staff in NewlyAddedStaffingBudgetDetails)
                                    {
                                        status = false;
                                        command.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(command, "@StaffingBudgetId", DbType.Int32, StaffingBudgetId);
                                        dbSmartAspects.AddInParameter(command, "@JobType", DbType.Int32, staff.JobType);
                                        dbSmartAspects.AddInParameter(command, "@JobLevel", DbType.String, staff.JobLevel);
                                        dbSmartAspects.AddInParameter(command, "@NoOfStaff", DbType.Int16, staff.NoOfStaff);
                                        dbSmartAspects.AddInParameter(command, "@BudgetAmount", DbType.Decimal, staff.BudgetAmount);
                                        dbSmartAspects.AddInParameter(command, "@FiscalYear", DbType.Int32, staff.FiscalYear);


                                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                    }
                                }
                            }

                            if (status)
                            {
                                transaction.Commit();
                                status = true;
                            }
                            else
                            {
                                transaction.Rollback();
                                status = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            status = false;
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
        public Boolean UpdateSaveStaffingBudget(PFSettingBO pfInfo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpPFInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@PFSettingId", DbType.Int32, pfInfo.PFSettingId);
                        dbSmartAspects.AddInParameter(command, "@EmpContributionInPercentage", DbType.Int32, pfInfo.EmployeeContributionInPercentage);
                        dbSmartAspects.AddInParameter(command, "@CompanyContributionInPercentange", DbType.Int32, pfInfo.CompanyContributionInPercentange);
                        dbSmartAspects.AddInParameter(command, "@EmpCanContributeMaxOfBasicSalary", DbType.Int32, pfInfo.EmployeeCanContributeMaxOfBasicSalary);
                        dbSmartAspects.AddInParameter(command, "@InterestDistributionRate", DbType.Decimal, pfInfo.InterestDistributionRate);
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
        public List<PayrollStaffingBudgetByDepartmentIdBO> GetStaffingBudgetByDepartmentId(int? departmentId, int? fiscalYear)
        {
            List<PayrollStaffingBudgetByDepartmentIdBO> viewlist = new List<PayrollStaffingBudgetByDepartmentIdBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollStaffingBudgetByDepartmentId_SP"))
                {
                    if (departmentId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);
                    if (fiscalYear != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FiscalYear", DbType.Int32, fiscalYear);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@FiscalYear", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "StaffingBudget");
                    DataTable Table = ds.Tables["StaffingBudget"];

                    viewlist = Table.AsEnumerable().Select(r => new PayrollStaffingBudgetByDepartmentIdBO
                    {
                        StaffingBudgetId = r.Field<Int64>("StaffingBudgetId"),
                        DepartmentId = r.Field<Int32>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        NumberOfStaff = r.Field<int>("NumberOfStaff"),
                        TotalBudget = r.Field<decimal>("TotalBudget")
                    }).ToList();
                }
            }
            return viewlist;
        }
        public List<PayrollStaffRequisitionBO> GetStaffRequisitionByDepartmentIdAndFiscalYear(int? departmentId, int? fiscalYear)
        {
            List<PayrollStaffRequisitionBO> viewlist = new List<PayrollStaffRequisitionBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollStaffRequisitionByDepartmentId_SP"))
                {
                    if (departmentId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);
                    if (fiscalYear != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FiscalYear", DbType.Int32, fiscalYear);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@FiscalYear", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "StaffRequisition");
                    DataTable Table = ds.Tables["StaffRequisition"];

                    viewlist = Table.AsEnumerable().Select(r => new PayrollStaffRequisitionBO
                    {
                        StaffRequisitionId = r.Field<Int64>("StaffRequisitionId"),
                        JobLevel = r.Field<string>("JobLevel"),
                        RequisitionQuantity = r.Field<Int16>("RequisitionQuantity"),
                        SalaryAmount = r.Field<decimal>("SalaryAmount"),
                        JobTypeName = r.Field<string>("JobTypeName"),
                        Department = r.Field<string>("Department"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus")
                    }).ToList();
                }
            }
            return viewlist;
        }
        public PayrollStaffRequisitionBO GetStaffRequisitionById(Int64? staffRequisitionId)
        {
            PayrollStaffRequisitionBO payrollStaff = new PayrollStaffRequisitionBO();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetStaffRequisitionById_SP"))
                    {
                        if (staffRequisitionId != 0)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@StaffRequisitionId", DbType.Int64, staffRequisitionId);
                        }
                        else dbSmartAspects.AddInParameter(cmd, "@StaffRequisitionId", DbType.Int64, DBNull.Value);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            while (reader.Read())
                            {
                                payrollStaff.StaffRequisitionId = Convert.ToInt64(reader["StaffRequisitionId"]);
                                payrollStaff.StaffRequisitionDetailsId = Convert.ToInt64(reader["StaffRequisitionDetailsId"]);
                                payrollStaff.JobLevel = reader["JobLevel"].ToString();
                                payrollStaff.RequisitionQuantity = Convert.ToInt16(reader["RequisitionQuantity"]);
                                payrollStaff.SalaryAmount = Convert.ToDecimal(reader["SalaryAmount"]);
                                payrollStaff.JobTypeName = reader["JobTypeName"].ToString();
                                payrollStaff.JobTypeId = Convert.ToInt32(reader["JobTypeId"]);
                                payrollStaff.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                payrollStaff.Department = reader["Department"].ToString();
                                payrollStaff.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                payrollStaff.FiscalYear = Convert.ToInt32(reader["FiscalYear"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return payrollStaff;
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
                                pfInfo.EmployeeContributionInPercentage = Convert.ToInt32(reader["EmpContributionInPercentage"]);
                                pfInfo.CompanyContributionInPercentange = Convert.ToInt32(reader["CompanyContributionInPercentange"]);
                                pfInfo.EmployeeCanContributeMaxOfBasicSalary = Convert.ToInt32(reader["EmpCanContributeMaxOfBasicSalary"]);
                                pfInfo.InterestDistributionRate = Convert.ToDecimal(reader["InterestDistributionRate"]);
                            }
                        }
                    }
                }
            }
            return pfInfo;
        }
        public PayrollStaffingBudgeDetailsBO GetPayrollStaffingBudget(int departmentId, int jobTypeId, string jobLevel, DateTime dateFrom, DateTime dateTo)
        {
            PayrollStaffingBudgeDetailsBO viewlist = new PayrollStaffingBudgeDetailsBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollStaffingBudget_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, Convert.ToInt32(departmentId));
                    dbSmartAspects.AddInParameter(cmd, "@JobType", DbType.Int32, Convert.ToInt32(jobTypeId));
                    dbSmartAspects.AddInParameter(cmd, "@JobLevel", DbType.String, jobLevel);
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, Convert.ToDateTime(dateFrom));
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, Convert.ToDateTime(dateTo));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "StaffingBudget");
                    DataTable Table = ds.Tables["StaffingBudget"];

                    viewlist = Table.AsEnumerable().Select(r => new PayrollStaffingBudgeDetailsBO
                    {
                        StaffingBudgetDetailsId = r.Field<Int64>("StaffingBudgetDetailsId"),
                        StaffingBudgetId = r.Field<Int64>("StaffingBudgetId"),
                        JobType = r.Field<int>("JobType"),
                        JobLevel = r.Field<string>("JobLevel"),
                        NoOfStaff = r.Field<Int16>("NoOfStaff"),
                        BudgetAmount = r.Field<decimal>("BudgetAmount"),
                        JobTypeName = r.Field<string>("JobTypeName")

                    }).FirstOrDefault();
                }
            }
            return viewlist;
        }
        public Boolean SaveStaffRequisition(PayrollStaffRequisitionBO StaffRequisition, List<PayrollStaffRequisitionDetailsBO> NewlyAddedStaffRequisitionDetails, out long StaffRequisitionId)
        {
            Boolean status = false;
            //Int64 StaffRequisitionId = 0;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveStaffRequisition_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, StaffRequisition.DepartmentId);
                                dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, StaffRequisition.ApprovedStatus);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, StaffRequisition.CreatedBy);

                                dbSmartAspects.AddOutParameter(command, "@StaffRequisitionId", DbType.Int64, sizeof(Int64));

                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                                StaffRequisitionId = Convert.ToInt32(command.Parameters["@StaffRequisitionId"].Value);
                            }

                            if (status)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePayrollStaffRequisitionDetails_SP"))
                                {
                                    foreach (PayrollStaffRequisitionDetailsBO staff in NewlyAddedStaffRequisitionDetails)
                                    {
                                        status = false;
                                        command.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(command, "@StaffRequisitionId", DbType.Int32, StaffRequisitionId);
                                        dbSmartAspects.AddInParameter(command, "@JobType", DbType.Int32, staff.JobType);
                                        dbSmartAspects.AddInParameter(command, "@JobLevel", DbType.String, staff.JobLevel);
                                        dbSmartAspects.AddInParameter(command, "@RequisitionQuantity", DbType.Int16, staff.RequisitionQuantity);
                                        dbSmartAspects.AddInParameter(command, "@SalaryAmount", DbType.Decimal, staff.SalaryAmount);
                                        dbSmartAspects.AddInParameter(command, "@DemandDate", DbType.DateTime, staff.DemandDate);
                                        dbSmartAspects.AddInParameter(command, "@FiscalYear", DbType.Int32, staff.FiscalYear);

                                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                    }
                                }
                            }

                            if (status)
                            {
                                transaction.Commit();
                                status = true;
                            }
                            else
                            {
                                transaction.Rollback();
                                status = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            status = false;
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

        public Boolean UpdateStaffRequisition(PayrollStaffRequisitionBO StaffRequisition, List<PayrollStaffRequisitionDetailsBO> EditedStaffRequisitionDetails)
        {
            Boolean status = false;
            //Int64 StaffRequisitionId = 0; 

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateStaffRequisition_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, StaffRequisition.DepartmentId);
                                dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, StaffRequisition.ApprovedStatus);
                                dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, StaffRequisition.CreatedBy);
                                dbSmartAspects.AddInParameter(command, "@StaffRequisitionId", DbType.Int64, StaffRequisition.StaffRequisitionId);

                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                                //StaffRequisitionId = Convert.ToInt32(command.Parameters["@StaffRequisitionId"].Value);
                            }

                            if (status)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateStaffRequisitionDetails_SP"))
                                {
                                    foreach (PayrollStaffRequisitionDetailsBO staff in EditedStaffRequisitionDetails)
                                    {
                                        status = false;
                                        command.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(command, "@StaffRequisitionId", DbType.Int32, staff.StaffRequisitionId);
                                        dbSmartAspects.AddInParameter(command, "@StaffRequisitionIdDetails", DbType.Int32, staff.StaffRequisitionDetailsId);
                                        dbSmartAspects.AddInParameter(command, "@JobType", DbType.Int32, staff.JobType);
                                        dbSmartAspects.AddInParameter(command, "@JobLevel", DbType.String, staff.JobLevel);
                                        dbSmartAspects.AddInParameter(command, "@RequisitionQuantity", DbType.Int16, staff.RequisitionQuantity);
                                        dbSmartAspects.AddInParameter(command, "@SalaryAmount", DbType.Decimal, staff.SalaryAmount);
                                        dbSmartAspects.AddInParameter(command, "@DemandDate", DbType.DateTime, staff.DemandDate);

                                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                    }
                                }
                            }

                            if (status)
                            {
                                transaction.Commit();
                                status = true;
                            }
                            else
                            {
                                transaction.Rollback();
                                status = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            status = false;
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
        public List<PayrollStaffingBudgeDetailsBO> GetPayrollStaffingBudgetForReport(int departmentId, int jobTypeId, string jobLevel, int fiscalYear)
        {
            List<PayrollStaffingBudgeDetailsBO> viewlist = new List<PayrollStaffingBudgeDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollStaffingBudgetForReport_SP"))
                {
                    if (departmentId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);
                    if (jobTypeId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@JobType", DbType.Int32, jobTypeId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@JobType", DbType.Int32, DBNull.Value);
                    if (jobLevel != "0")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@JobLevel", DbType.String, jobLevel);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@JobLevel", DbType.String, DBNull.Value);
                    if (jobLevel != "0")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@fiscalYear", DbType.String, fiscalYear);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@fiscalYear", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "StaffingBudget");
                    DataTable Table = ds.Tables["StaffingBudget"];

                    viewlist = Table.AsEnumerable().Select(r => new PayrollStaffingBudgeDetailsBO
                    {
                        StaffingBudgetDetailsId = r.Field<Int64>("StaffingBudgetDetailsId"),
                        StaffingBudgetId = r.Field<Int64>("StaffingBudgetId"),
                        JobType = r.Field<int>("JobType"),
                        JobLevel = r.Field<string>("JobLevel"),
                        NoOfStaff = r.Field<Int16>("NoOfStaff"),
                        BudgetAmount = r.Field<decimal>("BudgetAmount"),
                        JobTypeName = r.Field<string>("JobTypeName"),
                        Department = r.Field<string>("Department")
                    }).ToList();
                }
            }
            return viewlist;
        }
        public List<PayrollStaffRequisitionDetailsBO> GetPayrollStaffRequisitionForReport(int departmentId, int jobTypeId, string jobLevel, DateTime? dateFrom, DateTime? dateTo, int fiscalYear)
        {
            List<PayrollStaffRequisitionDetailsBO> viewlist = new List<PayrollStaffRequisitionDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollStaffRequisitionForReport_SP"))
                {
                    if (departmentId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);
                    if (jobTypeId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@JobType", DbType.Int32, jobTypeId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@JobType", DbType.Int32, DBNull.Value);
                    if (jobLevel != "0")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@JobLevel", DbType.String, jobLevel);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@JobLevel", DbType.String, DBNull.Value);
                    if (fiscalYear != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FiscalYear", DbType.Int32, fiscalYear);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@FiscalYear", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "StaffRequisition");
                    DataTable Table = ds.Tables["StaffRequisition"];

                    viewlist = Table.AsEnumerable().Select(r => new PayrollStaffRequisitionDetailsBO
                    {
                        StaffRequisitionDetailsId = r.Field<Int64>("StaffRequisitionDetailsId"),
                        StaffRequisitionId = r.Field<Int64>("StaffRequisitionId"),
                        JobType = r.Field<int>("JobType"),
                        JobLevel = r.Field<string>("JobLevel"),
                        RequisitionQuantity = r.Field<Int16>("RequisitionQuantity"),
                        SalaryAmount = r.Field<decimal>("SalaryAmount"),
                        JobTypeName = r.Field<string>("JobTypeName"),
                        Department = r.Field<string>("Department"),
                        DemandDateString = r.Field<string>("DemandDateString")
                    }).ToList();
                }
            }
            return viewlist;
        }
        public bool UpdateStaffRequisitionStatus(PayrollStaffRequisitionBO transferbo)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateStaffRequisitionStatus_SP"))
                {
                    conn.Open();

                    dbSmartAspects.AddInParameter(commandMaster, "@StaffRequisitionId", DbType.Int32, transferbo.StaffRequisitionId);
                    dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, transferbo.ApprovedStatus);
                    dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, transferbo.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(commandMaster);
                }
            }

            return (status > 0 ? true : false);
        }

        public PayrollStaffingBudgetBO GetPayrollStaffingBudgetById(long StaffingBudgetId)
        {
            PayrollStaffingBudgetBO StaffingBudget = new PayrollStaffingBudgetBO();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollStaffingBudgetById_SP"))
                    {
                        if (StaffingBudgetId != 0)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@StaffingBudgetId", DbType.Int64, StaffingBudgetId);
                        }
                        else dbSmartAspects.AddInParameter(cmd, "@StaffingBudgetId", DbType.Int64, DBNull.Value);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            while (reader.Read())
                            {

                                StaffingBudget.StaffingBudgetId = Convert.ToInt32(reader["StaffingBudgetId"]);
                                StaffingBudget.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                StaffingBudget.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                StaffingBudget.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);

                                //viewlist.Add(StaffingBudget);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return StaffingBudget;
        }
        public List<PayrollStaffingBudgeDetailsBO> payrollStaffingBudgeDetailsById(long StaffingBudgetId)
        {
            List<PayrollStaffingBudgeDetailsBO> StaffingBudgetDetailsList = new List<PayrollStaffingBudgeDetailsBO>();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollStaffingBudgetDetailsById_SP"))
                    {
                        if (StaffingBudgetId != 0)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@StaffingBudgetId", DbType.Int64, StaffingBudgetId);
                        }
                        else dbSmartAspects.AddInParameter(cmd, "@StaffingBudgetId", DbType.Int64, DBNull.Value);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            while (reader.Read())
                            {
                                PayrollStaffingBudgeDetailsBO StaffingBudgetDetails = new PayrollStaffingBudgeDetailsBO();

                                StaffingBudgetDetails.StaffingBudgetId = Convert.ToInt32(reader["StaffingBudgetId"]);
                                StaffingBudgetDetails.StaffingBudgetDetailsId = Convert.ToInt32(reader["StaffingBudgetDetailsId"]);
                                StaffingBudgetDetails.JobType = Convert.ToInt32(reader["JobType"]);
                                StaffingBudgetDetails.JobTypeName = reader["JobTypeName"].ToString();
                                StaffingBudgetDetails.JobLevel = reader["JobLevel"].ToString();
                                StaffingBudgetDetails.NoOfStaff = Convert.ToInt16(reader["NoOfStaff"]);
                                StaffingBudgetDetails.BudgetAmount = Convert.ToDecimal(reader["BudgetAmount"]);
                                StaffingBudgetDetails.FiscalYear = Convert.ToInt32(reader["FiscalYearId"]);
                                StaffingBudgetDetails.FiscalYearName = reader["FiscalYearName"].ToString();
                                StaffingBudgetDetails.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);

                                StaffingBudgetDetailsList.Add(StaffingBudgetDetails);


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return StaffingBudgetDetailsList;
        }

        public bool UpdateStaffingBudget(PayrollStaffingBudgetBO StaffingBudget, List<PayrollStaffingBudgeDetailsBO> NewlyAddedStaffingBudgetDetails, List<PayrollStaffingBudgeDetailsBO> EditedStaffingBudgetDetails, List<PayrollStaffingBudgeDetailsBO> DeletedStaffingBudgetDetails)
        {
            bool retVal = false;
            bool status = false;
            int tmpRequisitionDetailsId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if (NewlyAddedStaffingBudgetDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePayrollStaffingBudgetDetails_SP"))
                            {
                                foreach (PayrollStaffingBudgeDetailsBO staff in NewlyAddedStaffingBudgetDetails)
                                {
                                    status = false;
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@StaffingBudgetId", DbType.Int32, StaffingBudget.StaffingBudgetId);
                                    dbSmartAspects.AddInParameter(command, "@JobType", DbType.Int32, staff.JobType);
                                    dbSmartAspects.AddInParameter(command, "@JobLevel", DbType.String, staff.JobLevel);
                                    dbSmartAspects.AddInParameter(command, "@NoOfStaff", DbType.Int16, staff.NoOfStaff);
                                    dbSmartAspects.AddInParameter(command, "@BudgetAmount", DbType.Decimal, staff.BudgetAmount);
                                    dbSmartAspects.AddInParameter(command, "@FiscalYear", DbType.Int32, staff.FiscalYear);


                                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                }
                            }
                        }
                        if (EditedStaffingBudgetDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateStaffingBudgetDetailsInfo_SP"))
                            {
                                foreach (PayrollStaffingBudgeDetailsBO staff in EditedStaffingBudgetDetails)
                                {
                                    status = false;
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@StaffingBudgetId", DbType.Int32, StaffingBudget.StaffingBudgetId);
                                    dbSmartAspects.AddInParameter(command, "@StaffingBudgeDetailstId", DbType.Int32, staff.StaffingBudgetDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@JobType", DbType.Int32, staff.JobType);
                                    dbSmartAspects.AddInParameter(command, "@JobLevel", DbType.String, staff.JobLevel);
                                    dbSmartAspects.AddInParameter(command, "@NoOfStaff", DbType.Int16, staff.NoOfStaff);
                                    dbSmartAspects.AddInParameter(command, "@BudgetAmount", DbType.Decimal, staff.BudgetAmount);
                                    dbSmartAspects.AddInParameter(command, "@FiscalYear", DbType.Int32, staff.FiscalYear);


                                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                }
                            }
                        }
                        if (DeletedStaffingBudgetDetails.Count > 0)
                        {
                            using (DbCommand cmdDelete = dbSmartAspects.GetStoredProcCommand("DeletePayrollStaffingBudgetDetails_SP"))
                            {
                                foreach (PayrollStaffingBudgeDetailsBO rd in DeletedStaffingBudgetDetails)
                                {
                                    cmdDelete.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdDelete, "@StaffingBudgetDetailsId", DbType.Int32, rd.StaffingBudgetDetailsId);
                                    dbSmartAspects.AddInParameter(cmdDelete, "@StaffingBudgetId", DbType.Int32, rd.StaffingBudgetId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdDelete) > 0 ? true : false; ;
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        status = false;
                    }
                }
                return status;
            }
        }

        public bool DeleteStaffingBudget(long staffingBudgetId)
        {
            bool status;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeletePayrollStaffingBudget_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@StaffingBudgetId", DbType.Int64, staffingBudgetId);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }

            }
            return status;

        }
    }
}
