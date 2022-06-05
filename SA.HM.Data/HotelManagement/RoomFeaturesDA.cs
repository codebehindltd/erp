using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.HotelManagement
{
    public class RoomFeaturesDA : BaseService
    {
        public bool SaveRoomFeatures(RoomFeaturesBO roomFtBO, out long tmpId)
        {
            tmpId = 0;
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRoomFeatures_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@Features", DbType.String, roomFtBO.Features);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, roomFtBO.Description);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, roomFtBO.ActiveStatus);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, roomFtBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@Id", DbType.Int64, sizeof(long));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpId = Convert.ToInt32(command.Parameters["@Id"].Value);
                }
            }
            return status;
        }

        public bool UpdateRoomFeatures(RoomFeaturesBO roomFtBO)
        {
            bool status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRoomFeatures_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, roomFtBO.Id);
                    dbSmartAspects.AddInParameter(command, "@Features", DbType.String, roomFtBO.Features);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, roomFtBO.Description);
                    dbSmartAspects.AddInParameter(command, "@ActiveStatus", DbType.Boolean, roomFtBO.ActiveStatus);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, roomFtBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public RoomFeaturesBO GetRoomFeaturesById(long FtId)
        {
            RoomFeaturesBO roomFtBO = new RoomFeaturesBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetRoomFeaturesById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, FtId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(command))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomFtBO.Id = Convert.ToInt64(reader["Id"]);
                                roomFtBO.Features = reader["Features"].ToString();
                                roomFtBO.Description = reader["Description"].ToString();
                                roomFtBO.ActiveStatus = Convert.ToBoolean(reader["ActiveStatus"]);
                                roomFtBO.ActiveStatusSt = reader["ActiveStatusSt"].ToString();

                            }
                        }
                    }
                }

            }
            return roomFtBO;
        }

        public List<RoomFeaturesBO> GetFeaturesInfoBySearchCriteriaForPaging(string features, bool activeStat, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<RoomFeaturesBO> roomFtList = new List<RoomFeaturesBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFeaturesInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Features", DbType.String, features);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStatus", DbType.Boolean, activeStat);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomFeaturesBO roomFtBO = new RoomFeaturesBO
                                {
                                    Id = Convert.ToInt64(reader["Id"]),
                                    Features = reader["Features"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    ActiveStatus = Convert.ToBoolean(reader["ActiveStatus"]),
                                    ActiveStatusSt = reader["ActiveStatusSt"].ToString()
                                };

                                roomFtList.Add(roomFtBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return roomFtList;
        }


        public List<RoomFeaturesBO> GetAllActiveRoomFeatures()
        {
            List<RoomFeaturesBO> roomFtList = new List<RoomFeaturesBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllActiveRoomFeatures_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomFeaturesBO roomFtBO = new RoomFeaturesBO
                                {
                                    Id = Convert.ToInt64(reader["Id"]),
                                    Features = reader["Features"].ToString(),
                                    ActiveStatus = Convert.ToBoolean(reader["ActiveStatus"]),
                                    ActiveStatusSt = reader["ActiveStatusSt"].ToString()
                                };

                                roomFtList.Add(roomFtBO);
                                //if(roomFtBO.ActiveStatusSt=="Active")
                                //{
                                //    roomFtList.Add(roomFtBO);
                                //}
                            }
                        }
                    }
                }
            }
            return roomFtList;

        }

    }
}
