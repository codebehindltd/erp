using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.HotelManagement
{
     public class RoomFeaturesInfoDA : BaseService
    {
        public List<RoomFeaturesInfoBO> GetRoomFtInfoByRoomId(long roomId)
        {
            List<RoomFeaturesInfoBO> alreadySavedRoomList = new List<RoomFeaturesInfoBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomFtInfoByRoomId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomId", DbType.Int64, roomId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomFeaturesInfoBO featuresInfoBO = new RoomFeaturesInfoBO
                                {
                                    Id = Convert.ToInt64(reader["Id"]),
                                    RoomId = Convert.ToInt64(reader["RoomId"]),
                                    FeaturesId = Convert.ToInt64(reader["FeaturesId"])
                                };
                                alreadySavedRoomList.Add(featuresInfoBO);
                            }
                        }
                    }
                }
            }
            return alreadySavedRoomList;
        }

        public bool DeleteRoomFeatureInfo( List<RoomFeaturesInfoBO> deletList)
        {
            bool status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteRoomFeatureInfo_SP"))
                        {
                            foreach (var item in deletList)
                            {
                                command.Parameters.Clear();
                                dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int64, item.RoomId);
                                dbSmartAspects.AddInParameter(command, "@FeaturesId", DbType.Int64, item.FeaturesId);
                                dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, item.Id);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;

                            }
                        }
                        if (status == true)
                        {
                            transction.Commit();
                        }
                        else
                        {
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                }
            }
            return status;

        }

        public bool SaveRoomFeaturesInfo(List<RoomFeaturesInfoBO> newAddList, int createdBy, out long tmpId)
        {
            tmpId = 0;
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRoomFeaturesInfo_SP"))
                        {

                            foreach (var item in newAddList)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int64, item.RoomId);
                                dbSmartAspects.AddInParameter(command, "@FeaturesId", DbType.Int64, item.FeaturesId);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int64, createdBy);

                                dbSmartAspects.AddOutParameter(command, "@Id", DbType.Int64, sizeof(Int64));

                                status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;

                                tmpId = Convert.ToInt32(command.Parameters["@Id"].Value);
                            }

                        }
                        if (status==true)
                        {
                            transction.Commit();
                        }
                        else
                        {
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                }
                
            }
            return status;
        }

        public bool UpdateRoomFeaturesInfo(List<RoomFeaturesInfoBO> updateList, int updatedBy)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRoomFeaturesInfo_SP"))
                        {
                            foreach (var item in updateList)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int64, item.RoomId);
                                dbSmartAspects.AddInParameter(command, "@FeaturesId", DbType.Int64, item.FeaturesId);
                                dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, item.Id);
                                dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, updatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;
                            }
                        }
                        if (status == true)
                        {
                            transction.Commit();
                        }
                        else
                        {
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                }
            }
            return status;

        }
    }
}
