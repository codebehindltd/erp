using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data
{
    public class RoomTypeDA : BaseService
    {
        public List<RoomTypeBO> GetRoomTypeInfo()
        {
            List<RoomTypeBO> roomTypeList = new List<RoomTypeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomTypeInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomTypeBO roomType = new RoomTypeBO();

                                roomType.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomType.RoomType = reader["RoomType"].ToString();
                                roomType.TypeCode = reader["TypeCode"].ToString();
                                roomType.LocalCurrencyHead = reader["LocalCurrencyHead"].ToString();
                                roomType.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                roomType.RoomRateUSD = Convert.ToDecimal(reader["RoomRateUSD"]);
                                roomType.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                roomType.ActiveStatus = reader["ActiveStatus"].ToString();
                                roomType.PaxQuantity = Convert.ToInt32(reader["PaxQuantity"]);
                                roomType.ChildQuantity = Convert.ToInt32(reader["ChildQuantity"]);
                                //roomType.AccountsPostingHeadId = Convert.ToInt32(reader["AccountsPostingHeadId"]);

                                roomTypeList.Add(roomType);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }
        public List<RoomTypeBO> GetRoomTypeInfoWithRoomCount()
        {
            List<RoomTypeBO> roomTypeList = new List<RoomTypeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGetRoomTypeInfoWithRoomCount_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomTypeBO roomType = new RoomTypeBO();

                                roomType.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomType.RoomType = reader["RoomType"].ToString();
                                roomType.TypeCode = reader["TypeCode"].ToString();
                                roomType.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                roomType.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                roomType.ActiveStatus = reader["ActiveStatus"].ToString();
                                roomType.TotalRoom = Convert.ToInt32(reader["TotalRooms"]);
                                roomType.PaxQuantity = Convert.ToInt32(reader["PaxQuantity"]);
                                //roomType.AccountsPostingHeadId = Convert.ToInt32(reader["AccountsPostingHeadId"]);

                                roomTypeList.Add(roomType);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }
        public Boolean SaveRoomTypeInfo(RoomTypeBO roomType, out int tmpRoomTypeId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRoomTypeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@RoomType", DbType.String, roomType.RoomType);
                        dbSmartAspects.AddInParameter(command, "@TypeCode", DbType.String, roomType.TypeCode);
                        dbSmartAspects.AddInParameter(command, "@RoomRate", DbType.Decimal, roomType.RoomRate);
                        dbSmartAspects.AddInParameter(command, "@RoomRateUSD", DbType.Decimal, roomType.RoomRateUSD);
                        dbSmartAspects.AddInParameter(command, "@MinimumRoomRate", DbType.Decimal, roomType.MinimumRoomRate);
                        dbSmartAspects.AddInParameter(command, "@MinimumRoomRateUSD", DbType.Decimal, roomType.MinimumRoomRateUSD);
                        dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, roomType.PaxQuantity);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, roomType.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@SuiteType", DbType.Boolean, roomType.SuiteType);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, roomType.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@RoomTypeId", DbType.Int32, sizeof(Int32));
                        dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int64, roomType.AccountsPostingHeadId);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpRoomTypeId = Convert.ToInt32(command.Parameters["@RoomTypeId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean UpdateRoomTypeInfo(RoomTypeBO roomType)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRoomTypeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@RoomTypeId", DbType.Int32, roomType.RoomTypeId);
                        dbSmartAspects.AddInParameter(command, "@RoomType", DbType.String, roomType.RoomType);
                        dbSmartAspects.AddInParameter(command, "@TypeCode", DbType.String, roomType.TypeCode);
                        dbSmartAspects.AddInParameter(command, "@RoomRate", DbType.Decimal, roomType.RoomRate);
                        dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, roomType.PaxQuantity);
                        dbSmartAspects.AddInParameter(command, "@RoomRateUSD", DbType.Decimal, roomType.RoomRateUSD);
                        dbSmartAspects.AddInParameter(command, "@MinimumRoomRate", DbType.Decimal, roomType.MinimumRoomRate);
                        dbSmartAspects.AddInParameter(command, "@MinimumRoomRateUSD", DbType.Decimal, roomType.MinimumRoomRateUSD);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, roomType.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@SuiteType", DbType.Boolean, roomType.SuiteType);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, roomType.LastModifiedBy);
                        dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int64, roomType.AccountsPostingHeadId);

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
        public RoomTypeBO GetRoomTypeInfoById(int roomTypeId)
        {
            RoomTypeBO roomType = new RoomTypeBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomTypeInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomTypeId", DbType.Int32, roomTypeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomType.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomType.RoomType = reader["RoomType"].ToString();
                                roomType.TypeCode = reader["TypeCode"].ToString();
                                roomType.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                roomType.RoomRateUSD = Convert.ToDecimal(reader["RoomRateUSD"]);
                                roomType.MinimumRoomRate = Convert.ToDecimal(reader["MinimumRoomRate"]);
                                roomType.MinimumRoomRateUSD = Convert.ToDecimal(reader["MinimumRoomRateUSD"]);
                                roomType.PaxQuantity = Convert.ToInt32(reader["PaxQuantity"]);
                                roomType.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                roomType.SuiteType = Convert.ToBoolean(reader["SuiteType"]);
                                roomType.ActiveStatus = reader["ActiveStatus"].ToString();
                                roomType.AccountsPostingHeadId = Convert.ToInt64(reader["AccountsPostingHeadId"]);
                            }
                        }
                    }
                }
            }
            return roomType;
        }
        public List<RoomTypeBO> GetRoomTypeInfoBySearchCriteria(string RoomType, bool ActiveStat, int suiteType)
        {
            List<RoomTypeBO> roomTypeList = new List<RoomTypeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomTypeInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomType", DbType.String, RoomType);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, ActiveStat);
                    dbSmartAspects.AddInParameter(cmd, "@SuiteType", DbType.Int32, suiteType);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomTypeBO roomType = new RoomTypeBO();
                                roomType.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomType.RoomType = reader["RoomType"].ToString();
                                roomType.TypeCode = reader["TypeCode"].ToString();
                                roomType.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                roomType.PaxQuantity = Convert.ToInt32(reader["PaxQuantity"]);
                                roomType.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                roomType.ActiveStatus = reader["ActiveStatus"].ToString();
                                roomType.AccountsPostingHeadId = Convert.ToInt64(reader["AccountsPostingHeadId"]);

                                roomTypeList.Add(roomType);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }
    }
}
