using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpTaxDeductionDA: BaseService
    {
        public Boolean SaveEmpTaxDeductInfo(EmpTaxDeductionSettingBO taxDeductInfo, out int tmpId)
        {
            Boolean status = false;
            tmpId = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpTaxDeductionInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Gender", DbType.String, taxDeductInfo.Gender);
                        dbSmartAspects.AddInParameter(command, "@RangeFrom", DbType.Decimal, taxDeductInfo.RangeFrom);
                        dbSmartAspects.AddInParameter(command, "@RangeTo", DbType.Decimal, taxDeductInfo.RangeTo);
                        dbSmartAspects.AddInParameter(command, "@DeductionPercentage", DbType.Decimal, taxDeductInfo.DeductionPercentage);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, taxDeductInfo.Remarks);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, taxDeductInfo.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@TaxDeductionId", DbType.Int32, taxDeductInfo.TaxDeductionId);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpId = Convert.ToInt32(command.Parameters["@TaxDeductionId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public EmpTaxDeductionSettingBO GetEmpTaxDeductInfo(int taxDeductId)
        {
            EmpTaxDeductionSettingBO taxDeductInfo = new EmpTaxDeductionSettingBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpTaxDeductInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TaxDeductionId", DbType.Int32, taxDeductId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                taxDeductInfo.TaxDeductionId = Convert.ToInt32(reader["TaxDeductionId"]);
                                taxDeductInfo.Gender = reader["Gender"].ToString();
                                taxDeductInfo.RangeFrom = Convert.ToDecimal(reader["RangeFrom"]);
                                taxDeductInfo.RangeTo = Convert.ToDecimal(reader["RangeTo"]);
                                taxDeductInfo.DeductionPercentage = Convert.ToDecimal(reader["DeductionPercentage"]);
                                taxDeductInfo.Remarks = reader["Remarks"].ToString();                                
                            }
                        }
                    }
                }
            }
            return taxDeductInfo;
        }

        public List<EmpTaxDeductionSettingBO> GetEmpAllTaxDeductInfo()
        {
            List<EmpTaxDeductionSettingBO> taxDeductInfo = new List<EmpTaxDeductionSettingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpAllTaxDeductInfo_SP"))
                {                    
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpTaxDeductionSettingBO taxdBO = new EmpTaxDeductionSettingBO();
                                taxdBO.TaxDeductionId = Convert.ToInt32(reader["TaxDeductionId"]);
                                taxdBO.Gender = reader["Gender"].ToString();
                                taxdBO.RangeFrom = Convert.ToDecimal(reader["RangeFrom"]);
                                taxdBO.RangeTo = Convert.ToDecimal(reader["RangeTo"]);
                                taxdBO.DeductionPercentage = Convert.ToDecimal(reader["DeductionPercentage"]);
                                taxdBO.Remarks = reader["Remarks"].ToString();
                                taxDeductInfo.Add(taxdBO);
                            }
                        }
                    }
                }
            }
            return taxDeductInfo;
        }

        public Boolean UpdateEmpTaxDeductInfo(EmpTaxDeductionSettingBO taxDeductInfo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpTaxDeductInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@TaxDeductionId", DbType.Int32, taxDeductInfo.TaxDeductionId);
                        dbSmartAspects.AddInParameter(command, "@Gender", DbType.String, taxDeductInfo.Gender);
                        dbSmartAspects.AddInParameter(command, "@RangeFrom", DbType.Decimal, taxDeductInfo.RangeFrom);
                        dbSmartAspects.AddInParameter(command, "@RangeTo", DbType.Decimal, taxDeductInfo.RangeTo);
                        dbSmartAspects.AddInParameter(command, "@DeductionPercentage", DbType.Decimal, taxDeductInfo.DeductionPercentage);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, taxDeductInfo.Remarks);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, taxDeductInfo.LastModifiedBy);

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

        public List<EmpTaxDeductionBO> GetEmpTaxDeduction(DateTime? fromDate, DateTime? toDate, int departmentId)
        {
            List<EmpTaxDeductionBO> boList = new List<EmpTaxDeductionBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTaxDeduction_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    if (departmentId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    }
                    else {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpTaxDeduction");
                    DataTable Table = ds.Tables["EmpTaxDeduction"];

                    boList = Table.AsEnumerable().Select(r => new EmpTaxDeductionBO
                    {
                        TaxCollectionId = r.Field<long>("TaxCollectionId"),
                        EmpId = r.Field<int>("EmpId"),
                        TaxDateFrom = r.Field<DateTime>("TaxDateFrom"),
                        TaxDateTo = r.Field<DateTime>("TaxDateTo"),
                        BasicAmount = r.Field<decimal>("BasicAmount"),  
                        DeductionPercentage = r.Field<decimal>("DeductionPercentage"),
                        TaxAmount = r.Field<decimal>("TaxAmount"),
                        EmpName = r.Field<string>("EmpName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        Department = r.Field<string>("Department")
                    }).ToList();
                }
            }
            return boList;
        }
    }
}
