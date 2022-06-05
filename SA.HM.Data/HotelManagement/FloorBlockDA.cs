using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
   public class FloorBlockDA : BaseService
    {
       public List<FloorBlockBO> GetFloorBlockInfo()
        {
            List<FloorBlockBO> floorList = new List<FloorBlockBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFloorBlockInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                FloorBlockBO floor = new FloorBlockBO();

                                floor.BlockId = Convert.ToInt32(reader["BlockId"]);
                                floor.BlockName = reader["BlockName"].ToString();
                                floor.BlockDescription = reader["BlockDescription"].ToString();
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
        public List<FloorBlockBO> GetActiveFloorBlockInfo()
        {
            List<FloorBlockBO> floorList = new List<FloorBlockBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveFloorBlockInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                FloorBlockBO floor = new FloorBlockBO();

                                floor.BlockId = Convert.ToInt32(reader["BlockId"]);
                                floor.BlockName = reader["BlockName"].ToString();
                                floor.BlockDescription = reader["BlockDescription"].ToString();
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
        public Boolean SaveFloorBlockInfo(FloorBlockBO floorBO, out int tmpFloorId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveFloorBlockInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@BlockName", DbType.String, floorBO.BlockName);
                        dbSmartAspects.AddInParameter(command, "@BlockDescription", DbType.String, floorBO.BlockDescription);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, floorBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, floorBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@BlockId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpFloorId = Convert.ToInt32(command.Parameters["@BlockId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
       public Boolean UpdateFloorBlockInfo(FloorBlockBO floorBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateFloorBlockInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@BlockId", DbType.Int32, floorBO.BlockId);
                        dbSmartAspects.AddInParameter(command, "@BlockName", DbType.String, floorBO.BlockName);
                        dbSmartAspects.AddInParameter(command, "@BlockDescription", DbType.String, floorBO.BlockDescription);
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
       public FloorBlockBO GetFloorBlockInfoById(int floorId)
        {
            FloorBlockBO floor = new FloorBlockBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFloorBlockInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BlockId", DbType.Int32, floorId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                floor.BlockId = Convert.ToInt32(reader["BlockId"]);
                                floor.BlockName = reader["BlockName"].ToString();
                                floor.BlockDescription = reader["BlockDescription"].ToString();
                                floor.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                floor.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return floor;
        }
       public List<FloorBlockBO> GetFloorBlockInfoBySearchCriteria(string FloorName, bool ActiveState)
       {
           List<FloorBlockBO> floorList = new List<FloorBlockBO>();
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFloorBlockInfoBySearchCriteria_SP"))
               {

                   dbSmartAspects.AddInParameter(cmd, "@BlockName", DbType.String, FloorName);
                   dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, ActiveState);

                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               FloorBlockBO floor = new FloorBlockBO();

                               floor.BlockId = Convert.ToInt32(reader["BlockId"]);
                               floor.BlockName = reader["BlockName"].ToString();
                               floor.BlockDescription = reader["BlockDescription"].ToString();
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
