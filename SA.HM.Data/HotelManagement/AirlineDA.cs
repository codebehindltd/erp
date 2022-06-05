using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.HotelManagement
{
    public class AirlineDA : BaseService
    {
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
        public List<AirlineBO> GetAirlineInfo()
        {
            List<AirlineBO> airlineList = new List<AirlineBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                //todo have to add procedure
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAirlineInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                AirlineBO airline = new AirlineBO();

                                airline.AirlineId = Convert.ToInt32(reader["AirlineId"]);
                                airline.AirlineName = reader["AirlineName"].ToString();
                                airline.FlightNumber = reader["FlightNumber"].ToString();
                                if (reader["AirlineTime"] != null)
                                {
                                    airline.AirlineTime = Convert.ToDateTime(currentDate + " " + reader["AirlineTime"]);
                                }
                                airline.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                airline.ActiveStatus = reader["ActiveStatus"].ToString();

                                airlineList.Add(airline);
                            }
                        }
                    }
                }
            }
            return airlineList;
        }
        public List<AirlineBO> GetActiveAirlineInfo()
        {
            List<AirlineBO> airlineList = new List<AirlineBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                //todo have to add procedure
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveAirlineInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                AirlineBO airline = new AirlineBO();

                                airline.AirlineId = Convert.ToInt32(reader["AirlineId"]);
                                airline.AirlineName = reader["AirlineName"].ToString();
                                airline.FlightNumber = reader["FlightNumber"].ToString();
                                if (reader["AirlineTime"] != null)
                                {
                                    airline.AirlineTime = Convert.ToDateTime(currentDate + " " + reader["AirlineTime"]);
                                }
                                airline.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                airline.ActiveStatus = reader["ActiveStatus"].ToString();

                                airlineList.Add(airline);
                            }
                        }
                    }
                }
            }
            return airlineList;
        }
        public Boolean SaveAirlineInfo(AirlineBO airlineBO, out int tmpAirlineId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                //todo have to add procedure
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveAirlineInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@AirlineName", DbType.String, airlineBO.AirlineName);
                    dbSmartAspects.AddInParameter(command, "@FlightNumber", DbType.String, airlineBO.FlightNumber);
                    dbSmartAspects.AddInParameter(command, "@AirlineTime", DbType.DateTime, airlineBO.AirlineTime);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, airlineBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, airlineBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@AirlineId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpAirlineId = Convert.ToInt32(command.Parameters["@AirlineId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateAirlineInfo(AirlineBO airlineBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                //todo have to add procedure
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateAirlineInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@AirlineId", DbType.Int32, airlineBO.AirlineId);
                    dbSmartAspects.AddInParameter(command, "@AirlineName", DbType.String, airlineBO.AirlineName);
                    dbSmartAspects.AddInParameter(command, "@FlightNumber", DbType.String, airlineBO.FlightNumber);
                    dbSmartAspects.AddInParameter(command, "@AirlineTime", DbType.DateTime, airlineBO.AirlineTime);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, airlineBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, airlineBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public AirlineBO GetAirlineInfoById(int airlineId)
        {
            AirlineBO airlineBO = new AirlineBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                //todo have to add procedure
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAirlineInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@AirlineId", DbType.Int32, airlineId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                airlineBO.AirlineId = Convert.ToInt32(reader["AirlineId"]);
                                airlineBO.AirlineName = reader["AirlineName"].ToString();
                                airlineBO.FlightNumber = reader["FlightNumber"].ToString();
                                airlineBO.AirlineTimeString = reader["AirlineTimeString"].ToString();
                                if (reader["AirlineTime"] != null)
                                {
                                    airlineBO.AirlineTime = Convert.ToDateTime(currentDate + " " + reader["AirlineTime"]);
                                }
                                airlineBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                airlineBO.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return airlineBO;
        }

        public List<AirlineBO> GetAirlineInformationBySearchCriteriaForPaging(string AirlineName, Boolean activeStat, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<AirlineBO> airlineList = new List<AirlineBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                //todo have to add procedure
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAirlineInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@AirlineName", DbType.String, AirlineName);
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
                                AirlineBO airline = new AirlineBO();
                                airline.AirlineId = Convert.ToInt32(reader["AirlineId"]);
                                airline.AirlineName = reader["AirlineName"].ToString();
                                airline.FlightNumber = reader["FlightNumber"].ToString();
                                if (reader["AirlineTime"] != null)
                                {
                                    airline.AirlineTime = Convert.ToDateTime(currentDate + " " + reader["AirlineTime"]);
                                }
                                airline.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                airline.ActiveStatus = reader["ActiveStatus"].ToString();
                                airlineList.Add(airline);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return airlineList;
        }
    }
}
