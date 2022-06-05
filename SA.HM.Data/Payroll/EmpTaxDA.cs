using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpTaxDA: BaseService
    {
        public Boolean SaveEmpTaxInformation(TaxSettingBO taxInfo, out int tmpId)
        {
            Boolean status = false;
            tmpId = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpTaxInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@TaxBandForMale", DbType.Decimal, taxInfo.TaxBandForMale);
                        dbSmartAspects.AddInParameter(command, "@TaxBandForFemale", DbType.Decimal, taxInfo.TaxBandForFemale);
                        dbSmartAspects.AddInParameter(command, "@IsTaxPaidByCompany", DbType.Boolean, taxInfo.IsTaxPaidByCompany);
                        dbSmartAspects.AddInParameter(command, "@CompanyContributionType", DbType.String, taxInfo.CompanyContributionType);
                        dbSmartAspects.AddInParameter(command, "@CompanyContributionAmount", DbType.Decimal, taxInfo.CompanyContributionAmount);
                        dbSmartAspects.AddInParameter(command, "@IsTaxDeductFromSalary", DbType.Boolean, taxInfo.IsTaxDeductFromSalary);
                        dbSmartAspects.AddInParameter(command, "@EmployeeContributionType", DbType.String, taxInfo.EmployeeContributionType);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, taxInfo.Remarks);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, taxInfo.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@TaxSettingId", DbType.Int32, taxInfo.TaxSettingId);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpId = Convert.ToInt32(command.Parameters["@TaxSettingId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public TaxSettingBO GetEmpTaxInformation()
        {
            TaxSettingBO taxInfo = new TaxSettingBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpTaxInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                taxInfo.TaxSettingId = Convert.ToInt32(reader["TaxSettingId"]);
                                taxInfo.TaxBandForMale = Convert.ToDecimal(reader["TaxBandForMale"]);
                                taxInfo.TaxBandForFemale = Convert.ToDecimal(reader["TaxBandForFemale"]);
                                taxInfo.IsTaxPaidByCompany = Convert.ToBoolean(reader["IsTaxPaidByCompany"]);
                                taxInfo.CompanyContributionType = reader["CompanyContributionType"].ToString();
                                taxInfo.CompanyContributionAmount = Convert.ToDecimal(reader["CompanyContributionAmount"]);
                                taxInfo.IsTaxDeductFromSalary = Convert.ToBoolean(reader["IsTaxDeductFromSalary"]);
                                taxInfo.EmployeeContributionType = reader["EmpContributionType"].ToString();
                                taxInfo.Remarks = reader["Remarks"].ToString();                                
                            }
                        }
                    }
                }
            }
            return taxInfo;
        }

        public Boolean UpdateEmpTaxInfo(TaxSettingBO taxInfo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpTaxInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@TaxSettingId", DbType.Int32, taxInfo.TaxSettingId);
                        dbSmartAspects.AddInParameter(command, "@TaxBandForMale", DbType.Decimal, taxInfo.TaxBandForMale);
                        dbSmartAspects.AddInParameter(command, "@TaxBandForFemale", DbType.Decimal, taxInfo.TaxBandForFemale);
                        dbSmartAspects.AddInParameter(command, "@IsTaxPaidByCompany", DbType.Boolean, taxInfo.IsTaxPaidByCompany);
                        dbSmartAspects.AddInParameter(command, "@CompanyContributionType", DbType.String, taxInfo.CompanyContributionType);
                        dbSmartAspects.AddInParameter(command, "@CompanyContributionAmount", DbType.Decimal, taxInfo.CompanyContributionAmount);
                        dbSmartAspects.AddInParameter(command, "@IsTaxDeductFromSalary", DbType.Boolean, taxInfo.IsTaxDeductFromSalary);
                        dbSmartAspects.AddInParameter(command, "@EmpContributionType", DbType.String, taxInfo.EmployeeContributionType);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, taxInfo.Remarks);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, taxInfo.LastModifiedBy);

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
