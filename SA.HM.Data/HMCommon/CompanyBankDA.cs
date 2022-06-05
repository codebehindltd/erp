using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HMCommon
{
   public class CompanyBankDA :BaseService
    {
        public List<CompanyBankBO> GetCompanyBankInfo()
        {
            List<CompanyBankBO> companyBankList = new List<CompanyBankBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyBankInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CompanyBankBO companyBankBO = new CompanyBankBO();

                                companyBankBO.BankId = Convert.ToInt32(reader["BankId"]);
                                companyBankBO.BankName = reader["BankName"].ToString();
                                companyBankBO.AccountName = reader["AccountName"].ToString();
                                companyBankBO.AccountNo1 = reader["AccountNo1"].ToString();
                                companyBankBO.AccountNo2 = reader["AccountNo2"].ToString();
                                companyBankBO.BranchName = reader["BranchName"].ToString();
                                companyBankBO.SwiftCode = reader["SwiftCode"].ToString();
                                companyBankList.Add(companyBankBO);
                            }
                        }
                    }
                }
            }
            return companyBankList;
        }

        public bool SaveCompanyBankInfo(CompanyBankBO companyBankBO, out int tmpBankId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCompanyBankInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@BankName", DbType.String, companyBankBO.BankName);
                    dbSmartAspects.AddInParameter(command, "@BranchName", DbType.String, companyBankBO.BranchName);
                    dbSmartAspects.AddInParameter(command, "@AccountNo1", DbType.String, companyBankBO.AccountNo1);
                    dbSmartAspects.AddInParameter(command, "@AccountNo2", DbType.String, companyBankBO.AccountNo2);
                    dbSmartAspects.AddInParameter(command, "@AccountName", DbType.String, companyBankBO.AccountName);
                    dbSmartAspects.AddInParameter(command, "@SwiftCode", DbType.String, companyBankBO.SwiftCode);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, companyBankBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@BankId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpBankId = Convert.ToInt32(command.Parameters["@BankId"].Value);
                }
            }
            return status;
        }

        public bool UpdateCompanyBankInfo(CompanyBankBO companyBankBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCompanyBankInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@BankId", DbType.String, companyBankBO.BankId);
                    dbSmartAspects.AddInParameter(command, "@BankName", DbType.String, companyBankBO.BankName);
                    dbSmartAspects.AddInParameter(command, "@BranchName", DbType.String, companyBankBO.BranchName);
                    dbSmartAspects.AddInParameter(command, "@AccountNo1", DbType.String, companyBankBO.AccountNo1);
                    dbSmartAspects.AddInParameter(command, "@AccountNo2", DbType.String, companyBankBO.AccountNo2);
                    dbSmartAspects.AddInParameter(command, "@AccountName", DbType.String, companyBankBO.AccountName);
                    dbSmartAspects.AddInParameter(command, "@SwiftCode", DbType.String, companyBankBO.SwiftCode);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, companyBankBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
