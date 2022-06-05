﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using HotelManagement.Entity.SalesAndMarketing;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class LocationDA : BaseService
    {
        public List<LocationBO> GetLocationInfo()
        {
            List<LocationBO> bankList = new List<LocationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLocationInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                LocationBO bank = new LocationBO();

                                bank.LocationId = Convert.ToInt32(reader["LocationId"]);
                                bank.LocationName = reader["LocationName"].ToString();
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
        public List<LocationBO> GetLocationInfoByCityId(int cityId)
        {
            List<LocationBO> bankList = new List<LocationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLocationInfoByCityId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CityId", DbType.Int32, cityId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                LocationBO bank = new LocationBO();

                                bank.LocationId = Convert.ToInt32(reader["LocationId"]);
                                bank.LocationName = reader["LocationName"].ToString();
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
        public Boolean SaveLocationInfo(LocationBO bankBO, out int tmpLocationId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveLocationInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CityId", DbType.Int32, bankBO.CityId);
                    dbSmartAspects.AddInParameter(command, "@CountryId", DbType.Int64, bankBO.CountryId);
                    dbSmartAspects.AddInParameter(command, "@StateId", DbType.Int32, bankBO.StateId);
                    dbSmartAspects.AddInParameter(command, "@LocationName", DbType.String, bankBO.LocationName);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bankBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bankBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@LocationId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpLocationId = Convert.ToInt32(command.Parameters["@LocationId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateLocationInfo(LocationBO bankBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateLocationInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@LocationId", DbType.Int32, bankBO.LocationId);
                    dbSmartAspects.AddInParameter(command, "@CountryId", DbType.Int64, bankBO.CountryId);
                    dbSmartAspects.AddInParameter(command, "@StateId", DbType.Int32, bankBO.StateId);
                    dbSmartAspects.AddInParameter(command, "@CityId", DbType.Int32, bankBO.CityId);
                    dbSmartAspects.AddInParameter(command, "@LocationName", DbType.String, bankBO.LocationName);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bankBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, bankBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public LocationBO GetLocationInfoById(int bankId)
        {
            LocationBO bank = new LocationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLocationInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, bankId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bank.LocationId = Convert.ToInt32(reader["LocationId"]);
                                bank.LocationName = reader["LocationName"].ToString();
                                bank.CountryId = Convert.ToInt64(reader["CountryId"]);
                                bank.Country = reader["Country"].ToString();
                                bank.StateId = Convert.ToInt64(reader["StateId"]);
                                bank.State = reader["State"].ToString();
                                bank.CityName = reader["CityName"].ToString();
                                bank.CityId = Convert.ToInt32(reader["CityId"]);
                                bank.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bank.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return bank;
        }
        public List<LocationBO> GetLocationInfoBySearchCriteria(string LocationName, bool ActiveStat)
        {
            List<LocationBO> bankList = new List<LocationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLocationInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LocationName", DbType.String, LocationName);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, ActiveStat);                    

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                LocationBO bank = new LocationBO();
                                bank.LocationId = Convert.ToInt32(reader["LocationId"]);
                                bank.LocationName = reader["LocationName"].ToString();
                                bank.CountryId = Convert.ToInt64(reader["CountryId"]);
                                bank.Country = reader["Country"].ToString();
                                bank.StateId = Convert.ToInt64(reader["StateId"]);
                                bank.State = reader["State"].ToString();
                                bank.CityName = reader["CityName"].ToString();
                                bank.CityId = Convert.ToInt32(reader["CityId"]);
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
        public List<LocationBO> GetLocationInformationBySearchCriteriaForPaging(string LocationName, Boolean activeStat, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<LocationBO> bankList = new List<LocationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLocationInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LocationName", DbType.String, LocationName);
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
                                LocationBO bank = new LocationBO();
                                bank.LocationId = Convert.ToInt32(reader["LocationId"]);
                                bank.LocationName = reader["LocationName"].ToString();
                                bank.CityId = Convert.ToInt32(reader["CityId"]);
                                bank.CityName = reader["CityName"].ToString();
                                bank.CountryId = Convert.ToInt64(reader["CountryId"]);
                                bank.Country = reader["Country"].ToString();
                                bank.StateId = Convert.ToInt64(reader["StateId"]);
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
        public List<LocationBO> GetLocationInfoBySearchAutoSearchByCity(string LocationName, Int64 countryId, Int64 stateId, Int64 cityId)
        {
            List<LocationBO> bankList = new List<LocationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLocationInfoBySearchAutoSearchByCity_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchTerm", DbType.String, LocationName);

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

                    if (cityId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CityId", DbType.Int64, cityId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CityId", DbType.Int64, DBNull.Value);
                    }

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                LocationBO bank = new LocationBO();
                                bank.LocationId = Convert.ToInt32(reader["LocationId"]);
                                bank.CityId = Convert.ToInt32(reader["CityId"]);
                                bank.LocationName = reader["LocationName"].ToString();
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
    }
}
