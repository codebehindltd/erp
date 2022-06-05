using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
   public class HMFloorDA:BaseService
    {
       public List<HMFloorBO> GetHMFloorInfo()
        {
            List<HMFloorBO> floorList = new List<HMFloorBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHMFloorInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HMFloorBO floor = new HMFloorBO();

                                floor.FloorId = Convert.ToInt32(reader["FloorId"]);
                                floor.FloorName = reader["FloorName"].ToString();
                                floor.FloorDescription = reader["FloorDescription"].ToString();
                                floor.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                floor.ActiveStatus = reader["ActiveStatus"].ToString();

                                floorList.Add(floor);
                            }
                        }
                    }
                }
            }
            return floorList;
        }
        public List<HMFloorBO> GetActiveHMFloorInfo()
        {
            List<HMFloorBO> floorList = new List<HMFloorBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveHMFloorInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HMFloorBO floor = new HMFloorBO();

                                floor.FloorId = Convert.ToInt32(reader["FloorId"]);
                                floor.FloorName = reader["FloorName"].ToString();
                                floor.FloorDescription = reader["FloorDescription"].ToString();
                                floor.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                floor.ActiveStatus = reader["ActiveStatus"].ToString();

                                floorList.Add(floor);
                            }
                        }
                    }
                }
            }
            return floorList;
        }
        public Boolean SaveHMFloorInfo(HMFloorBO floorBO, out int tmpFloorId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveHMFloorInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@FloorName", DbType.String, floorBO.FloorName);
                        dbSmartAspects.AddInParameter(command, "@FloorDescription", DbType.String, floorBO.FloorDescription);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, floorBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, floorBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@FloorId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpFloorId = Convert.ToInt32(command.Parameters["@FloorId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
       public Boolean UpdateHMFloorInfo(HMFloorBO floorBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateHMFloorInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@FloorId", DbType.Int32, floorBO.FloorId);
                        dbSmartAspects.AddInParameter(command, "@FloorName", DbType.String, floorBO.FloorName);
                        dbSmartAspects.AddInParameter(command, "@FloorDescription", DbType.String, floorBO.FloorDescription);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, floorBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, floorBO.LastModifiedBy);

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
       public HMFloorBO GetHMFloorInfoById(int floorId)
        {
            HMFloorBO floor = new HMFloorBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHMFloorInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FloorId", DbType.Int32, floorId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                floor.FloorId = Convert.ToInt32(reader["FloorId"]);
                                floor.FloorName = reader["FloorName"].ToString();
                                floor.FloorDescription = reader["FloorDescription"].ToString();
                                floor.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                floor.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return floor;
        }
       public List<HMFloorBO> GetHMFloorInfoBySearchCriteria(string FloorName, bool ActiveState)
       {
           List<HMFloorBO> floorList = new List<HMFloorBO>();
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHMFloorInfoBySearchCriteria_SP"))
               {

                   dbSmartAspects.AddInParameter(cmd, "@FloorName", DbType.String, FloorName);
                   dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, ActiveState);

                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               HMFloorBO floor = new HMFloorBO();

                               floor.FloorId = Convert.ToInt32(reader["FloorId"]);
                               floor.FloorName = reader["FloorName"].ToString();
                               floor.FloorDescription = reader["FloorDescription"].ToString();
                               floor.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                               floor.ActiveStatus = reader["ActiveStatus"].ToString();

                               floorList.Add(floor);
                           }
                       }
                   }
               }
           }
           return floorList;
       }
    }
}
