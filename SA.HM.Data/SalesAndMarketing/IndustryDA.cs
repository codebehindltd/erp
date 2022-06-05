using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using HotelManagement.Entity.SalesAndMarketing;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class IndustryDA : BaseService
    {
        public List<IndustryBO> GetIndustryInfo()
        {
            List<IndustryBO> bankList = new List<IndustryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetIndustryInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                IndustryBO bank = new IndustryBO();

                                bank.IndustryId = Convert.ToInt32(reader["IndustryId"]);
                                bank.IndustryName = reader["IndustryName"].ToString();
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
        public Boolean SaveIndustryInfo(IndustryBO bankBO, out int tmpIndustryId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveIndustryInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@IndustryName", DbType.String, bankBO.IndustryName);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bankBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bankBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@IndustryId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpIndustryId = Convert.ToInt32(command.Parameters["@IndustryId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateIndustryInfo(IndustryBO bankBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateIndustryInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@IndustryId", DbType.Int32, bankBO.IndustryId);
                    dbSmartAspects.AddInParameter(command, "@IndustryName", DbType.String, bankBO.IndustryName);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bankBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, bankBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public IndustryBO GetIndustryInfoById(int bankId)
        {
            IndustryBO bank = new IndustryBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetIndustryInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@IndustryId", DbType.Int32, bankId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bank.IndustryId = Convert.ToInt32(reader["IndustryId"]);
                                bank.IndustryName = reader["IndustryName"].ToString();
                                bank.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bank.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return bank;
        }
        public List<IndustryBO> GetIndustryInfoBySearchCriteria(string IndustryName, bool ActiveStat)
        {
            List<IndustryBO> bankList = new List<IndustryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetIndustryInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@IndustryName", DbType.String, IndustryName);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, ActiveStat);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                IndustryBO bank = new IndustryBO();
                                bank.IndustryId = Convert.ToInt32(reader["IndustryId"]);
                                bank.IndustryName = reader["IndustryName"].ToString();
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
        public List<IndustryBO> GetIndustryInfoByAutoSearch(string IndustryName)
        {
            List<IndustryBO> bankList = new List<IndustryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetIndustryInfoByAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@IndustryName", DbType.String, IndustryName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                IndustryBO bank = new IndustryBO();
                                bank.IndustryId = Convert.ToInt32(reader["IndustryId"]);
                                bank.IndustryName = reader["IndustryName"].ToString();
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
        public List<IndustryBO> GetIndustryInformationBySearchCriteriaForPaging(string IndustryName, Boolean activeStat, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<IndustryBO> bankList = new List<IndustryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetIndustryInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@IndustryName", DbType.String, IndustryName);
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
                                IndustryBO bank = new IndustryBO();
                                bank.IndustryId = Convert.ToInt32(reader["IndustryId"]);
                                bank.IndustryName = reader["IndustryName"].ToString();
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
