using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using HotelManagement.Entity.SalesAndMarketing;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class CityDA : BaseService
    {
        public List<CityBO> GetCityInfo()
        {
            List<CityBO> bankList = new List<CityBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCityInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CityBO bank = new CityBO();

                                bank.CityId = Convert.ToInt32(reader["CityId"]);
                                bank.CityName = reader["CityName"].ToString();
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
        public Boolean SaveCityInfo(CityBO bankBO, out int tmpCityId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCityInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CityName", DbType.String, bankBO.CityName);
                    dbSmartAspects.AddInParameter(command, "@CountryId", DbType.Int64, bankBO.CountryId);
                    dbSmartAspects.AddInParameter(command, "@StateId", DbType.Int64, bankBO.StateId);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bankBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bankBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@CityId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpCityId = Convert.ToInt32(command.Parameters["@CityId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateCityInfo(CityBO bankBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCityInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CityId", DbType.Int32, bankBO.CityId);
                    dbSmartAspects.AddInParameter(command, "@CountryId", DbType.Int64, bankBO.CountryId);
                    dbSmartAspects.AddInParameter(command, "@StateId", DbType.Int64, bankBO.StateId);
                    dbSmartAspects.AddInParameter(command, "@CityName", DbType.String, bankBO.CityName);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bankBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, bankBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public CityBO GetCityInfoById(int bankId)
        {
            CityBO bank = new CityBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCityInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CityId", DbType.Int32, bankId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bank.CityId = Convert.ToInt32(reader["CityId"]);
                                bank.CityName = reader["CityName"].ToString();
                                bank.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bank.ActiveStatus = reader["ActiveStatus"].ToString();
                                bank.CountryId = Convert.ToInt64(reader["CountryId"]);
                                bank.StateId = Convert.ToInt64(reader["StateId"]);
                                bank.Country = reader["Country"].ToString();
                                bank.State = reader["State"].ToString();
                                bank.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return bank;
        }
        public List<CityBO> GetCityInfoBySearchCriteria(string CityName, bool ActiveStat)
        {
            List<CityBO> bankList = new List<CityBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCityInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CityName", DbType.String, CityName);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, ActiveStat);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CityBO bank = new CityBO();
                                bank.CityId = Convert.ToInt32(reader["CityId"]);
                                bank.CityName = reader["CityName"].ToString();
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
        public List<CityBO> GetCityInfoBySearchAutoSearchByState(string CityName, Int64 countryId, Int64 stateId)
        {
            List<CityBO> bankList = new List<CityBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCityListForAutoSearchByState_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchTerm", DbType.String, CityName);
                    if (countryId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int64, countryId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int64, DBNull.Value);
                    }

                    if (stateId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@StateId", DbType.Int64, stateId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@StateId", DbType.Int64, DBNull.Value);
                    }
                    
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CityBO bank = new CityBO();
                                bank.CityId = Convert.ToInt64(reader["CityId"]);
                                bank.CityName = reader["CityName"].ToString();
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
        public List<CityBO> GetCityInformationBySearchCriteriaForPaging(string CityName, Boolean activeStat, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<CityBO> bankList = new List<CityBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCityInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CityName", DbType.String, CityName);
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
                                CityBO bank = new CityBO();
                                bank.CityId = Convert.ToInt32(reader["CityId"]);
                                bank.CityName = reader["CityName"].ToString();
                                bank.Country = reader["Country"].ToString();
                                bank.State = reader["State"].ToString();
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