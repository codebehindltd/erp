using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Data.HMCommon
{
    public class BankDA : BaseService
    {
        public List<BankBO> GetBankInfo()
        {
            List<BankBO> bankList = new List<BankBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBankInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BankBO bank = new BankBO();

                                bank.BankId = Convert.ToInt32(reader["BankId"]);
                                bank.BankName = reader["BankName"].ToString();
                                bank.BankAccountNameAndNumber = reader["BankAccountNameAndNumber"].ToString();
                                bank.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bank.ActiveStatus = reader["ActiveStatus"].ToString();

                                bankList.Add(bank);
                            }
                        }
                    }
                }
            }
            return bankList;
        }
        public Boolean SaveBankInfo(BankBO bankBO, out int tmpBankId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveBankInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@BankName", DbType.String, bankBO.BankName);

                    dbSmartAspects.AddInParameter(command, "@AccountName", DbType.String, bankBO.AccountName);
                    dbSmartAspects.AddInParameter(command, "@BranchName", DbType.String, bankBO.BranchName);
                    dbSmartAspects.AddInParameter(command, "@AccountNumber", DbType.String, bankBO.AccountNumber);
                    dbSmartAspects.AddInParameter(command, "@AccountType", DbType.String, bankBO.AccountType);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, bankBO.Description);
                    if(bankBO.BankHeadId == 0)
                    {
                        dbSmartAspects.AddInParameter(command, "@BankHeadId", DbType.Int32, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(command, "@BankHeadId", DbType.Int32, bankBO.BankHeadId);
                    }
                    

                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bankBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bankBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@BankId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpBankId = Convert.ToInt32(command.Parameters["@BankId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateBankInfo(BankBO bankBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBankInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int32, bankBO.BankId);
                    dbSmartAspects.AddInParameter(command, "@BankName", DbType.String, bankBO.BankName);

                    dbSmartAspects.AddInParameter(command, "@AccountName", DbType.String, bankBO.AccountName);
                    dbSmartAspects.AddInParameter(command, "@BranchName", DbType.String, bankBO.BranchName);
                    dbSmartAspects.AddInParameter(command, "@AccountNumber", DbType.String, bankBO.AccountNumber);
                    dbSmartAspects.AddInParameter(command, "@AccountType", DbType.String, bankBO.AccountType);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, bankBO.Description);
                    if (bankBO.BankHeadId == 0)
                    {
                        dbSmartAspects.AddInParameter(command, "@BankHeadId", DbType.Int32, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(command, "@BankHeadId", DbType.Int32, bankBO.BankHeadId);
                    }

                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bankBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, bankBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<BankBO> GetBankInfoForAutoComplete(string bankName)
        {
            List<BankBO> bankList = new List<BankBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBankInfoForAutoComplete_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BankName", DbType.String, bankName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BankBO bank = new BankBO();

                                bank.BankId = Convert.ToInt32(reader["BankId"]);
                                bank.BankName = reader["BankName"].ToString();

                                bank.AccountName = reader["AccountName"].ToString();
                                bank.BranchName = reader["BranchName"].ToString();
                                bank.AccountNumber = reader["AccountNumber"].ToString();
                                bank.AccountType = reader["AccountType"].ToString();
                                bank.Description = reader["Description"].ToString();
                                bank.BankHeadId = Convert.ToInt32(reader["BankHeadId"]);

                                bank.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bank.ActiveStatus = reader["ActiveStatus"].ToString();

                                bankList.Add(bank);
                            }
                        }
                    }
                }
            }
            return bankList;
        }
        public BankBO GetBankInfoById(int bankId)
        {
            BankBO bank = new BankBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBankInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BankId", DbType.Int32, bankId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bank.BankId = Convert.ToInt32(reader["BankId"]);
                                bank.BankName = reader["BankName"].ToString();

                                bank.AccountName = reader["AccountName"].ToString();
                                bank.BranchName = reader["BranchName"].ToString();
                                bank.AccountNumber = reader["AccountNumber"].ToString();
                                bank.AccountType = reader["AccountType"].ToString();
                                bank.Description = reader["Description"].ToString();
                                bank.BankHeadId = Convert.ToInt32(reader["BankHeadId"]);

                                bank.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bank.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return bank;
        }
        public List<BankBO> GetBankInfoBySearchCriteria(string BankName, bool ActiveStat)
        {
            List<BankBO> bankList = new List<BankBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBankInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BankName", DbType.String, BankName);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, ActiveStat);                    

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BankBO bank = new BankBO();
                                bank.BankId = Convert.ToInt32(reader["BankId"]);
                                bank.BankName = reader["BankName"].ToString();
                                bank.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bank.ActiveStatus = reader["ActiveStatus"].ToString();
                                bankList.Add(bank);
                            }
                        }
                    }                  
                }
            }
            return bankList;
        }
        public List<BankBO> GetBankInformationBySearchCriteriaForPaging(string BankName, Boolean activeStat, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<BankBO> bankList = new List<BankBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBankInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BankName", DbType.String, BankName);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, activeStat);
                    
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BankBO bank = new BankBO();
                                bank.BankId = Convert.ToInt32(reader["BankId"]);
                                bank.BankName = reader["BankName"].ToString();
                                bank.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bank.ActiveStatus = reader["ActiveStatus"].ToString();
                                bankList.Add(bank);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return bankList;
        }
    }
}
