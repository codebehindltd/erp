using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpGratuityDA : BaseService
    {
        public Boolean SaveEmpGratuityInfo(GratutitySettingsBO gratuityInfo, out int tmpId)
        {
            Boolean status = false;
            tmpId = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpGratuityInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@GratuityWillAffectAfterJobLengthInYear", DbType.Int32, gratuityInfo.GratuityWillAffectAfterJobLengthInYear);
                        dbSmartAspects.AddInParameter(command, "@IsGratuityBasedOnBasic", DbType.Boolean, gratuityInfo.IsGratuityBasedOnBasic);
                        dbSmartAspects.AddInParameter(command, "@GratutiyPercentage", DbType.Decimal, gratuityInfo.GratutiyPercentage);
                        dbSmartAspects.AddInParameter(command, "@NumberOfGratuityAdded", DbType.Int32, gratuityInfo.NumberOfGratuityAdded);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, gratuityInfo.CreatedBy);
                        dbSmartAspects.AddInParameter(command, "@CreatedDate", DbType.DateTime, gratuityInfo.CreatedDate);
                        dbSmartAspects.AddOutParameter(command, "@GratuityId", DbType.Int32, gratuityInfo.GratuityId);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpId = Convert.ToInt32(command.Parameters["@GratuityId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public GratutitySettingsBO GetEmpGratuityInfo()
        {
            GratutitySettingsBO gratuityInfo = new GratutitySettingsBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpGratuityInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                gratuityInfo.GratuityId = Convert.ToInt32(reader["GratuityId"]);
                                gratuityInfo.GratuityWillAffectAfterJobLengthInYear = Convert.ToInt32(reader["GratuityWillAffectAfterJobLengthInYear"]);
                                gratuityInfo.IsGratuityBasedOnBasic = Convert.ToBoolean(reader["IsGratuityBasedOnBasic"]);
                                gratuityInfo.GratutiyPercentage = Convert.ToDecimal(reader["GratutiyPercentage"]);
                                gratuityInfo.NumberOfGratuityAdded = Convert.ToInt32(reader["NumberOfGratuityAdded"]);
                            }
                        }
                    }
                }
            }
            return gratuityInfo;
        }
        public Boolean UpdateEmpGratuityInfo(GratutitySettingsBO gratuityInfo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpGratuityInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@GratuityId", DbType.Int32, gratuityInfo.GratuityId);
                        dbSmartAspects.AddInParameter(command, "@GratuityWillAffectAfterJobLengthInYear", DbType.Int32, gratuityInfo.GratuityWillAffectAfterJobLengthInYear);
                        dbSmartAspects.AddInParameter(command, "@IsGratuityBasedOnBasic", DbType.Boolean, gratuityInfo.IsGratuityBasedOnBasic);
                        dbSmartAspects.AddInParameter(command, "@GratutiyPercentage", DbType.Decimal, gratuityInfo.GratutiyPercentage);
                        dbSmartAspects.AddInParameter(command, "@NumberOfGratuityAdded", DbType.Int32, gratuityInfo.NumberOfGratuityAdded);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, gratuityInfo.LastModifiedBy);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedDate", DbType.DateTime, gratuityInfo.LastModifiedDate);

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
        public List<EmployeeBO> GetEmpGratuityForApproval(int empId, int departmentId, DateTime? searchDate)
        {
            List<EmployeeBO> boList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpForGratuityApproval_SP"))
                {
                    if (empId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);
                    if (departmentId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@SearchDate", DbType.DateTime, searchDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpGratuity");
                    DataTable Table = ds.Tables["EmpGratuity"];

                    boList = Table.AsEnumerable().Select(r => new EmployeeBO
                    {
                        EmpId = r.Field<Int32>("EmpId"),
                        DisplayName = r.Field<string>("DisplayName"),
                        Department = r.Field<string>("Department")
                    }).ToList();
                }
            }
            return boList;
        }
        public Boolean ApproveGratuity(int empId, DateTime? gretuityEligibilityDate)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ApproveGratuity"))
                    {
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, empId);
                        dbSmartAspects.AddInParameter(command, "@GratuityEligibilityDate", DbType.DateTime, gretuityEligibilityDate);

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
        public List<EmpGratuityBO> GetEmpGratuityInfoForReport(int empId)
        {
            List<EmpGratuityBO> boList = new List<EmpGratuityBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpGratuityInfoForReport_SP"))
                {
                    if (empId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpGratuityInfo");
                    DataTable Table = ds.Tables["EmpGratuityInfo"];

                    boList = Table.AsEnumerable().Select(r => new EmpGratuityBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        Department = r.Field<string>("Department"),
                        BasicAmount = r.Field<decimal>("BasicAmount"),
                        NumberOfGratuity = r.Field<int>("NumberOfGratuity"),
                        GratuityAmount = r.Field<decimal>("GratuityAmount"),
                        GratuityDate = r.Field<DateTime>("GratuityDate")

                    }).ToList();
                }
            }
            return boList;
        }
        public List<GratuityEligibleEmpListViewBO> GetGratuityEligibleEmpList()
        {
            List<GratuityEligibleEmpListViewBO> boList = new List<GratuityEligibleEmpListViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGratuityEligibleEmpList_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpGratuity");
                    DataTable Table = ds.Tables["EmpGratuity"];

                    boList = Table.AsEnumerable().Select(r => new GratuityEligibleEmpListViewBO
                    {
                        Name = r.Field<string>("Name"),
                        Id = r.Field<string>("Id"),
                        Department = r.Field<string>("Department"),
                        Basic = r.Field<decimal?>("Basic"),
                        GratuityAmount = r.Field<decimal?>("GratuityAmount")
                    }).ToList();
                }
            }
            return boList;
        }
        public List<GratuityMonthlyBalanceViewBO> GetMonthlyTotalGratuityAmount(DateTime? fromDate, DateTime? toDate)
        {
            List<GratuityMonthlyBalanceViewBO> boList = new List<GratuityMonthlyBalanceViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMonthlyTotalGratuityAmount_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "MonthlyGratuity");
                    DataTable Table = ds.Tables["MonthlyGratuity"];

                    boList = Table.AsEnumerable().Select(r => new GratuityMonthlyBalanceViewBO
                    {
                        Month = r.Field<string>("Month"),
                        Department = r.Field<string>("Department"),
                        GratuityAmount = r.Field<decimal?>("GratuityAmount")

                    }).ToList();
                }
            }
            return boList;
        }
        public List<EmpGratuityBO> GetGratuityStatementInfo(DateTime processDate, bool isManagement)
        {
            List<EmpGratuityBO> boList = new List<EmpGratuityBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetStatementOfGratuity_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDate", DbType.DateTime, processDate);
                    dbSmartAspects.AddInParameter(cmd, "@IsManagement", DbType.Boolean, isManagement);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GratuityStatement");
                    DataTable Table = ds.Tables["GratuityStatement"];

                    boList = Table.AsEnumerable().Select(r => new EmpGratuityBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        BasicAmount = r.Field<decimal>("BasicAmount"),
                        JoinDate = r.Field<DateTime>("JoinDate"),
                        GratuityEligibilityDate = r.Field<DateTime?>("GratuityEligibilityDate"),
                        ServiceYear = r.Field<int?>("ServiceYear"),
                        GratuityAmount = r.Field<decimal>("GratuityAmount")

                    }).ToList();
                }
            }
            return boList;
        }
        public List<EmpGratuityBO> GetEmpGratuityForProfile(int empId)
        {
            List<EmpGratuityBO> boList = new List<EmpGratuityBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpGratuityForProfile_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GratuityforProfile");
                    DataTable Table = ds.Tables["GratuityforProfile"];

                    boList = Table.AsEnumerable().Select(r => new EmpGratuityBO
                    {                       
                        ShowGratuityEligibilityDate = r.Field<string>("ShowGratuityEligibilityDate"),
                        GratuityAmount = r.Field<decimal?>("GratuityAmount"),
                        NumberOfGratuity = r.Field<int>("NumberOfGratuity")                        

                    }).ToList();
                }
            }
            return boList;
        }
    }
}
