using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class RoomStatusDA : BaseService
    {
        public List<RoomStatusBO> GetRoomStatusInfo()
        {
            List<RoomStatusBO> roomTypeList = new List<RoomStatusBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomStatusInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomStatusBO roomStatus = new RoomStatusBO();

                                roomStatus.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomStatus.StatusName = reader["StatusName"].ToString();
                                roomStatus.Remarks = reader["Remarks"].ToString();

                                roomTypeList.Add(roomStatus);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }
        public List<RoomStatusBO> GetRoomStatusInfoWithAll()
        {
            List<RoomStatusBO> roomTypeList = new List<RoomStatusBO>();
            RoomStatusBO roomStatusall = new RoomStatusBO();
            roomStatusall.StatusId = 0;
            roomStatusall.StatusName = "All";
            roomTypeList.Add(roomStatusall);
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomStatusInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomStatusBO roomStatus = new RoomStatusBO();

                                roomStatus.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomStatus.StatusName = reader["StatusName"].ToString();
                                roomStatus.Remarks = reader["Remarks"].ToString();

                                roomTypeList.Add(roomStatus);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }
        public List<RoomStatusInfoSummaryBO> GetRoomStatusInfoByDate(DateTime searchDate)
        {
            List<RoomStatusInfoSummaryBO> RoomStatusInfoList = new List<RoomStatusInfoSummaryBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomStatusInfoByDate_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchDate", DbType.DateTime, searchDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomStatusInfoSummaryBO RoomStatusInfo = new RoomStatusInfoSummaryBO();

                                RoomStatusInfo.RoomType = reader["RoomType"].ToString();
                                RoomStatusInfo.TotalRoom = Convert.ToInt32(reader["TotalRoom"]);
                                RoomStatusInfo.Reserved = Convert.ToInt32(reader["Reserved"]);
                                RoomStatusInfo.Booked = Convert.ToInt32(reader["Booked"]);
                                RoomStatusInfo.RoomAvailable = Convert.ToInt32(reader["RoomAvailable"]);

                                RoomStatusInfoList.Add(RoomStatusInfo);
                            }
                        }
                    }
                }
            }
            return RoomStatusInfoList;
        }
        public List<RoomStatusPossiblePathHeadBO> GetRoomStatusPossiblePathHead()
        {
            List<RoomStatusPossiblePathHeadBO> boList = new List<RoomStatusPossiblePathHeadBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomStatusPossiblePathHead_SP"))
                {
                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "RoomStatusHead");
                    DataTable Table = SaleServiceDS.Tables["RoomStatusHead"];

                    boList = Table.AsEnumerable().Select(r => new RoomStatusPossiblePathHeadBO
                    {
                        PathId = r.Field<int>("PathId"),
                        PossiblePath = r.Field<string>("PossiblePath"),
                        DisplayText = r.Field<string>("DisplayText")

                    }).ToList();
                }
            }
            return boList;
        }
        public List<RoomStatusPossiblePathBO> GetPermittedRoomStatusPossiblePath(int userGroupId, string pathType)
        {
            List<RoomStatusPossiblePathBO> boList = new List<RoomStatusPossiblePathBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPermittedRoomStatusPossiblePath_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);
                    dbSmartAspects.AddInParameter(cmd, "@PossiblePathType", DbType.String, pathType);

                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "PermittedRoomStatus");
                    DataTable Table = SaleServiceDS.Tables["PermittedRoomStatus"];

                    boList = Table.AsEnumerable().Select(r => new RoomStatusPossiblePathBO
                    {
                        MappingId = r.Field<int>("MappingId"),
                        PathId = r.Field<int>("PathId"),
                        UserGroupId = r.Field<int>("UserGroupId"),
                        PossiblePathType = r.Field<string>("PossiblePathType"),
                        DisplayText = r.Field<string>("DisplayText"),
                        DisplayOrder = r.Field<int>("DisplayOrder")

                    }).ToList();
                }
            }
            return boList;
        }
        public RoomStatusPossiblePathBO GetPermittedRoomStatusPossiblePathId(int userGroupId, string pathType, int pathId)
        {
            RoomStatusPossiblePathBO bo = new RoomStatusPossiblePathBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPermittedRoomStatusPossiblePathId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);
                    dbSmartAspects.AddInParameter(cmd, "@PossiblePathType", DbType.String, pathType);
                    dbSmartAspects.AddInParameter(cmd, "@PathId", DbType.Int32, pathId);

                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "PermittedRoomStatus");
                    DataTable Table = SaleServiceDS.Tables["PermittedRoomStatus"];

                    bo = Table.AsEnumerable().Select(r => new RoomStatusPossiblePathBO
                    {
                        MappingId = r.Field<int>("MappingId"),
                        PathId = r.Field<int>("PathId"),
                        UserGroupId = r.Field<int>("UserGroupId"),
                        PossiblePathType = r.Field<string>("PossiblePathType"),
                        DisplayText = r.Field<string>("DisplayText"),
                        DisplayOrder = r.Field<int>("DisplayOrder")

                    }).FirstOrDefault();
                }
            }
            return bo;
        }
        public Boolean SaveRoomStatusPossiblePathPermission(List<RoomStatusPossiblePathBO> RoomStatusPossiblePathAdded, List<RoomStatusPossiblePathBO> RoomStatusPossiblePathEdited, List<RoomStatusPossiblePathBO> RoomStatusPossiblePathDeleted)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if (RoomStatusPossiblePathAdded.Count > 0)
                        {
                            using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("SaveRoomStatusPossiblePathPermission_SP"))
                            {
                                foreach (RoomStatusPossiblePathBO bo in RoomStatusPossiblePathAdded)
                                {
                                    commandAdd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandAdd, "@UserGroupId", DbType.Int32, bo.UserGroupId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@PossiblePathType", DbType.String, bo.PossiblePathType);
                                    dbSmartAspects.AddInParameter(commandAdd, "@PathId", DbType.Int32, bo.PathId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@DisplayText", DbType.String, bo.DisplayText);
                                    dbSmartAspects.AddInParameter(commandAdd, "@DisplayOrder", DbType.Int32, bo.DisplayOrder);

                                    status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
                                }
                            }
                        }
                        else
                        {
                            status = 1;
                        }

                        if (status > 0 && RoomStatusPossiblePathEdited.Count > 0)
                        {
                            using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("UpdateRoomStatusPossiblePathPermission_SP"))
                            {
                                foreach (RoomStatusPossiblePathBO bo in RoomStatusPossiblePathEdited)
                                {
                                    commandAdd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandAdd, "@MappingId", DbType.Int32, bo.MappingId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@UserGroupId", DbType.Int32, bo.UserGroupId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@PossiblePathType", DbType.String, bo.PossiblePathType);
                                    dbSmartAspects.AddInParameter(commandAdd, "@PathId", DbType.Int32, bo.PathId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@DisplayText", DbType.String, bo.DisplayText);
                                    dbSmartAspects.AddInParameter(commandAdd, "@DisplayOrder", DbType.Int32, bo.DisplayOrder);

                                    status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
                                }
                            }
                        }

                        if (status > 0 && RoomStatusPossiblePathDeleted.Count > 0)
                        {
                            using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (RoomStatusPossiblePathBO bo in RoomStatusPossiblePathDeleted)
                                {
                                    commandAdd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandAdd, "@TableName", DbType.String, "HotelRoomStatusPossiblePath");
                                    dbSmartAspects.AddInParameter(commandAdd, "@TablePKField", DbType.String, "MappingId");
                                    dbSmartAspects.AddInParameter(commandAdd, "@TablePKId", DbType.String, bo.MappingId.ToString());

                                    status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();

                        throw ex;
                    }
                }
            }

            return retVal;
        }


        public List<DailyOccupiedRoomListVwBO> GetDailyOccupiedRoomList(DateTime dateFrom, DateTime dateTo, int years)
        {
            List<DailyOccupiedRoomListVwBO> boList = new List<DailyOccupiedRoomListVwBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDailyOccupiedRoomList"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    dbSmartAspects.AddInParameter(cmd, "@Year", DbType.Int32, years);

                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "DailyOccupiedRoomList");
                    DataTable Table = SaleServiceDS.Tables["DailyOccupiedRoomList"];

                    boList = Table.AsEnumerable().Select(r => new DailyOccupiedRoomListVwBO
                    {
                        ServiceDate = r.Field<DateTime>("ServiceDate"),
                        RoomType = r.Field<string>("RoomType"),
                        RoomCount = r.Field<int>("RoomCount")

                    }).ToList();
                }
            }
            return boList;
        }


    }
}
