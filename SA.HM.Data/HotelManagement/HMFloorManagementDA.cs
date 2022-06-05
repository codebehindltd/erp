using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class HMFloorManagementDA : BaseService
    {
        public List<HMFloorManagementBO> GetAllRoomInfoByFloorId(int floorId, int blockId)
        {
            List<HMFloorManagementBO> floorList = new List<HMFloorManagementBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllRoomInfoByFloorId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FloorId", DbType.Int32, floorId);
                    dbSmartAspects.AddInParameter(cmd, "@BlockId", DbType.Int32, blockId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HMFloorManagementBO floorBO = new HMFloorManagementBO();
                                floorBO.FloorManagementId = Convert.ToInt32(reader["FloorManagementId"]);
                                floorBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                floorBO.RoomNumber = reader["RoomNumber"].ToString();
                                floorBO.RoomType = reader["RoomType"].ToString();
                                floorBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                floorList.Add(floorBO);
                            }
                        }
                    }
                }
            }
            return floorList;
        }
        public List<HMFloorManagementBO> GetHMFloorManagementInfoByFloorId(int floorId)
        {
            List<HMFloorManagementBO> floorList = new List<HMFloorManagementBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHMFloorManagementInfoByFloorId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FloorId", DbType.Int32, floorId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HMFloorManagementBO floorBO = new HMFloorManagementBO();
                                floorBO.FloorManagementId = Convert.ToInt32(reader["FloorManagementId"]);
                                floorBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                floorBO.RoomNumber = reader["RoomNumber"].ToString();
                                floorBO.StatusId = Convert.ToInt32(reader["StatusId"]);
                                floorBO.StatusName = reader["StatusName"].ToString();
                                floorBO.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                floorBO.RoomType = reader["RoomType"].ToString();
                                floorBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                floorBO.CSSClassName = reader["CSSClassName"].ToString();
                                floorBO.ColorCodeName = reader["ColorCodeName"].ToString();
                                floorBO.XCoordinate = Convert.ToDecimal(reader["XCoordinate"]);
                                floorBO.YCoordinate = Convert.ToDecimal(reader["YCoordinate"]);
                                floorBO.RoomWidth = Convert.ToInt32(reader["RoomWidth"]);
                                floorBO.RoomHeight = Convert.ToInt32(reader["RoomHeight"]);

                                floorList.Add(floorBO);
                            }
                        }
                    }
                }
            }
            return floorList;
        }

        public List<HMFloorManagementBO> GetHMFloorManagementInfoByFloorNBlockId(int floorId, int floorBlockId)
        {
            List<HMFloorManagementBO> floorList = new List<HMFloorManagementBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHMFloorManagementInfoByFloorNBlockId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FloorId", DbType.Int32, floorId);
                    dbSmartAspects.AddInParameter(cmd, "@BlockId", DbType.Int32, floorBlockId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HMFloorManagementBO floorBO = new HMFloorManagementBO();
                                floorBO.FloorManagementId = Convert.ToInt32(reader["FloorManagementId"]);
                                floorBO.TypeCode = reader["TypeCode"].ToString();
                                floorBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                floorBO.RoomNumber = reader["RoomNumber"].ToString();
                                floorBO.StatusId = Convert.ToInt32(reader["StatusId"]);
                                floorBO.StatusName = reader["StatusName"].ToString();
                                floorBO.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                floorBO.RoomType = reader["RoomType"].ToString();
                                floorBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                floorBO.CSSClassName = reader["CSSClassName"].ToString();
                                floorBO.ColorCodeName = reader["ColorCodeName"].ToString();
                                floorBO.XCoordinate = Convert.ToDecimal(reader["XCoordinate"]);
                                floorBO.YCoordinate = Convert.ToDecimal(reader["YCoordinate"]);
                                floorBO.RoomWidth = Convert.ToInt32(reader["RoomWidth"]);
                                floorBO.RoomHeight = Convert.ToInt32(reader["RoomHeight"]);
                                floorBO.FloorId = Convert.ToInt32(reader["FloorId"]);
                                floorBO.IsBillLockedAndPreview = Convert.ToInt32(reader["IsBillLockedAndPreview"]);

                                floorList.Add(floorBO);
                            }
                        }
                    }
                }
            }
            return floorList;
        }

        public HMFloorManagementBO GetRoomInfoByRoomId(int roomId)
        {
            HMFloorManagementBO floorBO = new HMFloorManagementBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomInfoByRoomId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomId", DbType.Int32, roomId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                floorBO.FloorManagementId = Convert.ToInt32(reader["FloorManagementId"]);
                                floorBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                floorBO.FloorId = Convert.ToInt32(reader["FloorId"]);
                                floorBO.BlockId = Convert.ToInt32(reader["BlockId"]);
                                
                            }
                        }
                    }
                }
            }

                return floorBO;
        }

        public Boolean SaveHMFloorManagementInfo(HMFloorManagementBO floorManagement, out int tmpFloorManagementId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveHMFloorManagementInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@FloorId", DbType.Int32, floorManagement.FloorId);
                    dbSmartAspects.AddInParameter(command, "@BlockId", DbType.Int32, floorManagement.BlockId);
                    dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, floorManagement.RoomId);
                    dbSmartAspects.AddInParameter(command, "@XCoordinate", DbType.Decimal, floorManagement.XCoordinate);
                    dbSmartAspects.AddInParameter(command, "@YCoordinate", DbType.Decimal, floorManagement.YCoordinate);
                    dbSmartAspects.AddInParameter(command, "@RoomWidth", DbType.Int32, floorManagement.RoomWidth);
                    dbSmartAspects.AddInParameter(command, "@RoomHeight", DbType.Int32, floorManagement.RoomHeight);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, floorManagement.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@FloorManagementId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpFloorManagementId = Convert.ToInt32(command.Parameters["@FloorManagementId"].Value);
                }
            }
            return status;
        }

        public Boolean UpdateHMFloorManagementInfo(HMFloorManagementBO floorManagement)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateHMFloorManagementInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@FloorManagementId", DbType.Int32, floorManagement.FloorManagementId);
                    //dbSmartAspects.AddInParameter(command, "@FloorId", DbType.Int32, floorManagement.FloorId);
                    //dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, floorManagement.RoomId);
                    dbSmartAspects.AddInParameter(command, "@XCoordinate", DbType.Decimal, floorManagement.XCoordinate);
                    dbSmartAspects.AddInParameter(command, "@YCoordinate", DbType.Decimal, floorManagement.YCoordinate);
                    dbSmartAspects.AddInParameter(command, "@RoomWidth", DbType.Int32, floorManagement.RoomWidth);
                    dbSmartAspects.AddInParameter(command, "@RoomHeight", DbType.Int32, floorManagement.RoomHeight);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, floorManagement.LastModifiedBy);
                    

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
