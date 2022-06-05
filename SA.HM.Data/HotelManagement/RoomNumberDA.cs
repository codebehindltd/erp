using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Data
{
    public class RoomNumberDA : BaseService
    {
        public List<RoomNumberBO> GetRoomNumberInfo()
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomNumberInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomNumberBO roomNumber = new RoomNumberBO();

                                roomNumber.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumber.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumber.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumber.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumber.ActiveStatus = reader["ActiveStatus"].ToString();
                                roomNumber.RoomType = reader["RoomType"].ToString();
                                roomNumber.IsSmokingRoom = Convert.ToBoolean(reader["IsSmokingRoom"]);
                                roomNumber.HKRoomStatusId = Convert.ToInt64(reader["HKRoomStatusId"]);
                                roomNumber.CleanupStatus = reader["CleanupStatus"].ToString();

                                roomNumberList.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public Boolean SaveRoomNumberInfo(RoomNumberBO roomNumber, out int tmpRoomNumberId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRoomNumberInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@RoomTypeId", DbType.Int32, roomNumber.RoomTypeId);
                        dbSmartAspects.AddInParameter(command, "@RoomNumber", DbType.String, roomNumber.RoomNumber);
                        dbSmartAspects.AddInParameter(command, "@RoomName", DbType.String, roomNumber.RoomName);
                        dbSmartAspects.AddInParameter(command, "@StatusId", DbType.Int32, roomNumber.StatusId);
                        dbSmartAspects.AddInParameter(command, "@HKRoomStatusId", DbType.Int32, roomNumber.HKRoomStatusId);
                        dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, roomNumber.ToDate);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, roomNumber.Remarks);
                        dbSmartAspects.AddInParameter(command, "@IsSmokingRoom", DbType.Boolean, roomNumber.IsSmokingRoom);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, roomNumber.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@RoomId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpRoomNumberId = Convert.ToInt32(command.Parameters["@RoomId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean UpdateRoomNumberInfo(RoomNumberBO roomNumber)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRoomNumberInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, roomNumber.RoomId);
                        dbSmartAspects.AddInParameter(command, "@RoomTypeId", DbType.Int32, roomNumber.RoomTypeId);
                        dbSmartAspects.AddInParameter(command, "@RoomNumber", DbType.String, roomNumber.RoomNumber);
                        dbSmartAspects.AddInParameter(command, "@RoomName", DbType.String, roomNumber.RoomName);
                        dbSmartAspects.AddInParameter(command, "@StatusId", DbType.Int32, roomNumber.StatusId);
                        dbSmartAspects.AddInParameter(command, "@HKRoomStatusId", DbType.Int32, roomNumber.HKRoomStatusId);
                        dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, roomNumber.ToDate);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, roomNumber.Remarks);
                        dbSmartAspects.AddInParameter(command, "@IsSmokingRoom", DbType.Boolean, roomNumber.IsSmokingRoom);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, roomNumber.LastModifiedBy);

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
        public Boolean UpdateRoomNumberInfoById(int roomId, int statusId, DateTime toDate, string remarks)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRoomNumberInfoById_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, roomId);
                        dbSmartAspects.AddInParameter(command, "@StatusId", DbType.Int32, statusId);
                        dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, toDate);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, remarks);

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
        public RoomNumberBO GetRoomNumberInfoById(int roomNumberId)
        {
            RoomNumberBO roomNumber = new RoomNumberBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomNumberInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomId", DbType.Int32, roomNumberId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomNumber.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumber.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumber.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumber.RoomName = reader["RoomName"].ToString();
                                roomNumber.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumber.ActiveStatus = reader["ActiveStatus"].ToString();
                                roomNumber.HKRoomStatusId = Convert.ToInt32(reader["HKRoomStatusId"]);
                                roomNumber.CleanupStatus = reader["CleanupStatus"].ToString();
                                roomNumber.LastCleanDate = reader["LastCleanDate"].ToString();
                                roomNumber.IsSmokingRoom = Convert.ToBoolean(reader["IsSmokingRoom"]);
                                if (roomNumber.StatusId == 3)
                                {
                                    roomNumber.ToDate = Convert.ToDateTime(reader["ToDate"]);
                                }

                                roomNumber.Remarks = reader["Remarks"].ToString();

                            }
                        }
                    }
                }
            }
            return roomNumber;
        }
        public List<RoomNumberBO> GetAvailableRoomNumberInfoForRegistrationForm(int roomTypeId, int isReservation, DateTime fromDate, DateTime toDate)
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAvailableRoomNumberInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@IsReservation", DbType.Int32, isReservation);
                    dbSmartAspects.AddInParameter(cmd, "@RoomTypeId", DbType.Int32, roomTypeId);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomNumberBO roomNumber = new RoomNumberBO();
                                roomNumber.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumber.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumber.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumber.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumber.ActiveStatus = reader["ActiveStatus"].ToString();
                                roomNumberList.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public List<RoomNumberBO> GetRoomNumberInfoByCondition(int roomTypeId, int condition)
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomNumberInfoByCondition_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RoomTypeId", DbType.Int32, roomTypeId);
                    dbSmartAspects.AddInParameter(cmd, "@Condition", DbType.Int32, condition);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomNumberBO roomNumber = new RoomNumberBO();

                                roomNumber.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumber.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumber.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumber.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumber.ActiveStatus = reader["ActiveStatus"].ToString();

                                roomNumberList.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public List<RoomNumberBO> GetTodaysExpectedCheckOutRoomNumberInfo()
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTodaysExpectedCheckOutRoomNumberInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomNumberBO roomNumber = new RoomNumberBO();

                                roomNumber.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumber.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumber.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumber.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumber.ActiveStatus = reader["ActiveStatus"].ToString();
                                roomNumber.RegistrationId = Convert.ToInt64(reader["RegistrationId"]);
                                roomNumberList.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public List<RoomNumberBO> GetRoomNumberInfoWithStopChargePosting(int costCenterId)
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomNumberInfoWithStopChargePosting_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomNumberBO roomNumber = new RoomNumberBO();

                                roomNumber.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumber.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumber.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumber.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumber.ActiveStatus = reader["ActiveStatus"].ToString();

                                roomNumberList.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public List<RoomNumberBO> GetRoomNumberInfoByRoomType(int roomType)
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomNumberInfoByRoomType_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);

                    if(roomType > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RoomType", DbType.Int32, roomType);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RoomType", DbType.Int32, DBNull.Value);
                    }

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomNumberBO roomNumber = new RoomNumberBO();

                                roomNumber.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumber.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumber.RoomType = reader["RoomType"].ToString();
                                roomNumber.TypeCode = reader["TypeCode"].ToString();
                                roomNumber.RoomName = reader["RoomName"].ToString();
                                roomNumber.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumber.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumber.ActiveStatus = reader["ActiveStatus"].ToString();
                                roomNumber.CSSClassName = reader["CSSClassName"].ToString();
                                roomNumber.ColorCodeName = reader["ColorCodeName"].ToString();
                                roomNumber.IsBillLockedAndPreview = Convert.ToInt32(reader["IsBillLockedAndPreview"]);

                                roomNumberList.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public List<RoomNumberBO> GetRoomStatusInfoByReservationId(int reservationId)
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomStatusInfoByReservationId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);

                        dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomNumberBO roomNumber = new RoomNumberBO();

                                roomNumber.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumber.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumber.RoomType = reader["RoomType"].ToString();
                                roomNumber.TypeCode = reader["TypeCode"].ToString();
                                roomNumber.RoomName = reader["RoomName"].ToString();
                                roomNumber.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumber.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumber.ActiveStatus = reader["ActiveStatus"].ToString();
                                roomNumber.CSSClassName = reader["CSSClassName"].ToString();
                                roomNumber.ColorCodeName = reader["ColorCodeName"].ToString();
                                roomNumber.IsBillLockedAndPreview = Convert.ToInt32(reader["IsBillLockedAndPreview"]);

                                roomNumberList.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public List<RoomNumberBO> GetRoomNumberStatusInfoByReservationId(int reservationId)
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomNumberStatusInfoByReservationId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomNumberBO roomNumber = new RoomNumberBO();

                                roomNumber.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumber.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumber.RoomType = reader["RoomType"].ToString();
                                roomNumber.TypeCode = reader["TypeCode"].ToString();
                                roomNumber.RoomName = reader["RoomName"].ToString();
                                roomNumber.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumber.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumber.ActiveStatus = reader["ActiveStatus"].ToString();
                                roomNumber.CSSClassName = reader["CSSClassName"].ToString();
                                roomNumber.ColorCodeName = reader["ColorCodeName"].ToString();

                                roomNumberList.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public List<RoomNumberBO> GetRoomNumberByRoomType(int roomType)
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomNumberByRoomType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomType", DbType.Int32, roomType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomNumberBO roomNumber = new RoomNumberBO();

                                roomNumber.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumber.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumber.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumber.StatusId = Convert.ToInt32(reader["StatusId"]);

                                roomNumberList.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public List<RoomNumberBO> GetRoomNumberInfoForRestaurantBillByRoomType(int roomType)
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomNumberInfoForRestaurantBillByRoomType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomType", DbType.Int32, roomType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomNumberBO roomNumber = new RoomNumberBO();

                                roomNumber.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumber.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumber.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumber.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumber.ActiveStatus = reader["ActiveStatus"].ToString();

                                roomNumberList.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public List<RoomNumberBO> GetRoomNumberInfoByCleanup(string cleanup, string roomNumber, string companyName, string companyAddress, string companyWeb)
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomNumberInfoByCleanup_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Cleanup", DbType.String, cleanup);
                    dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNumber);
                    //dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    //dbSmartAspects.AddInParameter(cmd, "@CompanyAddress", DbType.String, companyAddress);
                    //dbSmartAspects.AddInParameter(cmd, "@CompanyWeb", DbType.String, companyWeb);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomNumberBO roomNumberBO = new RoomNumberBO();

                                roomNumberBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumberBO.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumberBO.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumberBO.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumberBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                roomNumberBO.CleanupStatus = reader["CleanupStatus"].ToString();
                                roomNumberBO.CleanDate = reader["CleanDate"].ToString();
                                roomNumberBO.CleanTime = reader["CleanDate"].ToString();
                                roomNumberBO.LastCleanDate = reader["LastCleanDate"].ToString();
                                roomNumberBO.Remarks = reader["Remarks"].ToString();

                                roomNumberList.Add(roomNumberBO);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public RoomNumberBO GetRoomInfoByRoomNumber(string roomNumber)
        {
            RoomNumberBO roomNumberBO = new RoomNumberBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomInfoByRoomNumber_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                roomNumberBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumberBO.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumberBO.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumberBO.RoomType = reader["RoomType"].ToString();
                                roomNumberBO.RoomRate = Convert.ToDecimal(reader["RoomRate"].ToString());
                                roomNumberBO.FromDate = Convert.ToDateTime(reader["FromDate"].ToString());
                                roomNumberBO.ToDate = Convert.ToDateTime(reader["ToDate"].ToString());
                                roomNumberBO.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumberBO.StatusName = reader["StatusName"].ToString();
                                roomNumberBO.Remarks = reader["Remarks"].ToString();
                            }
                        }
                    }
                }
            }
            return roomNumberBO;
        }
        public RoomNumberBO GetRoomInformationByRoomNumber(string roomNumber)
        {
            RoomNumberBO roomNumberBO = new RoomNumberBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomInformationByRoomNumber_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomNumberBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumberBO.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumberBO.RoomType = reader["RoomType"].ToString();
                                roomNumberBO.TypeCode = reader["TypeCode"].ToString();
                                roomNumberBO.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumberBO.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumberBO.StatusName = reader["StatusName"].ToString();
                                roomNumberBO.HKRoomStatusId = Convert.ToInt32(reader["HKRoomStatusId"]);
                            }
                        }
                    }
                }
            }
            return roomNumberBO;
        }
        public RoomNumberBO GetRoomInformationByRoomNumberForExpressCheckIn(string roomNumber)
        {
            RoomNumberBO roomNumberBO = new RoomNumberBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomInformationByRoomNumberForExpressCheckIn_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomNumberBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumberBO.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumberBO.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumberBO.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumberBO.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                roomNumberBO.RoomRateUSD = Convert.ToDecimal(reader["RoomRateUSD"]);
                            }
                        }
                    }
                }
            }
            return roomNumberBO;
        }
        public Boolean UpdateRoomCleanInfo(RoomNumberBO roomNumber)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRoomCleanInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, roomNumber.RoomId);
                    dbSmartAspects.AddInParameter(command, "@CleanupStatus", DbType.String, roomNumber.CleanupStatus);
                    dbSmartAspects.AddInParameter(command, "@CleanDate", DbType.DateTime, roomNumber.CleanDate);
                    dbSmartAspects.AddInParameter(command, "@LastCleanDate", DbType.DateTime, roomNumber.LastCleanDate);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, roomNumber.Remarks);

                    dbSmartAspects.AddInParameter(command, "@StatusId", DbType.Int32, roomNumber.StatusId);
                    dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, roomNumber.FromDate);
                    dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, roomNumber.ToDate);


                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, roomNumber.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<RoomNumberBO> GetRoomNumberInfoForCalender(DateTime fromDate, DateTime toDate)
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomNumberInfoForCalender_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomNumberBO roomNumber = new RoomNumberBO();
                                roomNumber.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumber.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumber.RoomType = reader["RoomType"].ToString();
                                roomNumber.TypeCode = reader["TypeCode"].ToString();
                                roomNumber.RoomTypeOrCode = reader["RoomTypeOrCode"].ToString();
                                roomNumber.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumber.RoomInformation = reader["RoomInformation"].ToString();
                                roomNumber.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumber.ActiveStatus = reader["ActiveStatus"].ToString();
                                roomNumber.FloorName = reader["FloorName"].ToString();
                                roomNumber.BlockName = reader["BlockName"].ToString();
                                roomNumber.FloorAndBlockName = reader["FloorAndBlockName"].ToString();
                                roomNumber.GroupByRowNo = Convert.ToInt64(reader["GroupByRowNo"]);

                                roomNumberList.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public List<RoomNumberBO> GetRoomNumberInfoBySearchCriteria(string roomNumber)
        {
            //string Where = GenarateWhereCondition(roomNumber, roomName);
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomNumberInfoBySearchCriteria_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNumber);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomNumberBO Room = new RoomNumberBO();
                                Room.RoomId = Convert.ToInt32(reader["RoomId"]);
                                Room.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                Room.RoomNumber = reader["RoomNumber"].ToString();
                                Room.RoomName = reader["RoomName"].ToString();
                                Room.StatusId = Convert.ToInt32(reader["StatusId"]);
                                Room.ActiveStatus = reader["ActiveStatus"].ToString();
                                Room.RoomType = reader["RoomType"].ToString();
                                roomNumberList.Add(Room);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public List<RoomNumberBO> GetAvailableRoomNumberInformation(int roomTypeId, int isReservation, DateTime fromDate, DateTime toDate, int reservationId)
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAvailableRoomNumberInformation_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@IsReservation", DbType.Int32, isReservation);
                    dbSmartAspects.AddInParameter(cmd, "@RoomTypeId", DbType.Int32, roomTypeId);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomNumberBO roomNumber = new RoomNumberBO();

                                roomNumber.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumber.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumber.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumber.RoomName = reader["RoomName"].ToString();
                                roomNumber.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumber.ActiveStatus = reader["ActiveStatus"].ToString();

                                roomNumberList.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public List<RoomNumberBO> GetAvailableRoomInformationForReservation(int reservationId, int roomTypeId)
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAvailableRoomInformationForReservation_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
                    dbSmartAspects.AddInParameter(cmd, "@RoomTypeId", DbType.Int32, roomTypeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomNumberBO roomNumber = new RoomNumberBO();

                                roomNumber.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumber.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomNumber.RoomNumber = reader["RoomNumber"].ToString();
                                roomNumber.RoomName = reader["RoomName"].ToString();
                                roomNumber.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumber.ActiveStatus = reader["ActiveStatus"].ToString();

                                roomNumberList.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public List<HouseKeepingReportViewBO> GetHouseKeepingInfo(string cleanUp, string roomNumber, int roomStatus, DateTime lastCleanDate)
        {
            List<HouseKeepingReportViewBO> houseKeeping = new List<HouseKeepingReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHouseKeepingInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@Cleanup", DbType.String, cleanUp);
                    dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNumber);
                    dbSmartAspects.AddInParameter(cmd, "@RoomStatus", DbType.Int32, roomStatus);
                    dbSmartAspects.AddInParameter(cmd, "@LastCleanDate", DbType.DateTime, lastCleanDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "HouseKeeping");
                    DataTable Table = ds.Tables["HouseKeeping"];
                    houseKeeping = Table.AsEnumerable().Select(r =>
                                new HouseKeepingReportViewBO
                                {
                                    RoomId = r.Field<int>("RoomId"),
                                    RoomTypeId = r.Field<int>("RoomTypeId"),
                                    RoomNumber = r.Field<int>("RoomNumber"),
                                    TotalInhouseGuest = r.Field<int>("TotalInhouseGuest"),
                                    StatusId = r.Field<int>("StatusId"),
                                    ActiveStatus = r.Field<string>("ActiveStatus"),
                                    CleanupStatus = r.Field<string>("CleanupStatus"),
                                    CleanDate = r.Field<DateTime?>("CleanDate"),
                                    LastCleanDate = r.Field<DateTime?>("LastCleanDate"),

                                    StringCleanDate = r.Field<string>("StringCleanDate"),
                                    StringLastCleanDate = r.Field<string>("StringLastCleanDate"),

                                    Remarks = r.Field<string>("Remarks")
                                }).ToList();
                }
            }
            return houseKeeping;
        }
        public List<HouseKeepingReportViewBO> GetHouseKeepingRoomDetailInfo(int roomTypeId, int roomNumberId, int roomStatusId, int khStatusId)
        {
            List<HouseKeepingReportViewBO> houseKeeping = new List<HouseKeepingReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHouseKeepingRoomDetailInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RoomTypeId", DbType.Int32, roomTypeId);
                    dbSmartAspects.AddInParameter(cmd, "@RoomNumberId", DbType.Int32, roomNumberId);
                    dbSmartAspects.AddInParameter(cmd, "@RoomStatusId", DbType.Int32, roomStatusId);
                    dbSmartAspects.AddInParameter(cmd, "@HKStatusId", DbType.Int32, khStatusId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HouseKeepingReportViewBO roomNumber = new HouseKeepingReportViewBO();
                                roomNumber.RoomType = reader["RoomType"].ToString();
                                roomNumber.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                roomNumber.FORoomStatus = reader["FORoomStatus"].ToString();
                                roomNumber.HKRoomStatus = reader["HKRoomStatus"].ToString();
                                roomNumber.ReservationStatus = reader["ReservationStatus"].ToString();

                                houseKeeping.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return houseKeeping;
        }
        public List<RoomNumberInfoByRoomStatusReportBO> GetRoomNumberInfoByRoomStatus(string roomStatus)
        {
            List<RoomNumberInfoByRoomStatusReportBO> rommStatus = new List<RoomNumberInfoByRoomStatusReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomNumberInfoByRoomStatus_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomStatus", DbType.String, roomStatus);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PurchaseInfo");
                    DataTable Table = ds.Tables["PurchaseInfo"];

                    rommStatus = Table.AsEnumerable().Select(r => new RoomNumberInfoByRoomStatusReportBO
                    {
                        RoomId = r.Field<int>("RoomId"),
                        RoomTypeId = r.Field<int?>("RoomTypeId"),
                        RoomType = r.Field<string>("RoomType"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        StatusId = r.Field<int?>("StatusId"),
                        ActiveStatus = r.Field<string>("ActiveStatus"),
                        CSSClassName = r.Field<string>("CSSClassName")

                    }).ToList();
                }
            }

            return rommStatus;
        }
        public List<RoomNumberBO> GetRoomNumberInfoByReservationId(int reservationId)
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomNumberInfoByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomNumberBO roomNumber = new RoomNumberBO();

                                roomNumber.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumber.RoomNumber = reader["RoomNumber"].ToString();

                                roomNumberList.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public Boolean UpdateRoomFromHk(List<RoomNumberBO> roomList)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {

                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRoomFromHK_SP"))
                    {
                        foreach (RoomNumberBO bo in roomList)
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, bo.RoomId);
                            dbSmartAspects.AddInParameter(command, "@HKRoomStatusId", DbType.Int64, bo.HKRoomStatusId);
                            dbSmartAspects.AddInParameter(command, "@LastCleanDate", DbType.DateTime, bo.LastCleanDate2);
                            dbSmartAspects.AddInParameter(command, "@HKRoomStatusName", DbType.String, bo.HKRoomStatusName);
                            dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, bo.FromDate);
                            dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, bo.ToDate);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, bo.Remarks);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public RoomLogFileBO GetRoomLogFileInfo(int roomId)
        {
            RoomLogFileBO roomNumber = new RoomLogFileBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOutofOrderRoomInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RoomId", DbType.Int32, roomId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomNumber.RoomLogFileId = Convert.ToInt32(reader["RoomLogFileId"]);
                                roomNumber.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomNumber.StatusId = Convert.ToInt32(reader["StatusId"]);
                                roomNumber.FromDate = Convert.ToDateTime(reader["FromDate"]);
                                roomNumber.ToDate = Convert.ToDateTime(reader["ToDate"]);
                                roomNumber.Remarks = reader["Remarks"].ToString();
                            }
                        }
                    }
                }
            }
            return roomNumber;
        }
        public List<RoomLogFileBO> GetRoomLogFileInfoByDateRange(DateTime fromDate, DateTime toDate)
        {
            List<RoomLogFileBO> roomInfos = new List<RoomLogFileBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOutofOrderRoomInfoByDateRange_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomLogFileBO log = new RoomLogFileBO();
                                log.RoomLogFileId = Convert.ToInt32(reader["RoomLogFileId"]);
                                log.RoomId = Convert.ToInt32(reader["RoomId"]);
                                log.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                log.StatusId = Convert.ToInt32(reader["StatusId"]);
                                log.StatusName = reader["StatusName"].ToString();
                                log.FromDate = Convert.ToDateTime(reader["FromDate"]);
                                log.ToDate = Convert.ToDateTime(reader["ToDate"]);
                                roomInfos.Add(log);
                            }
                        }
                    }
                }
            }
            return roomInfos;
        }
        public bool SynchronizeHouseKeepingMorningDirtyHour()
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("HouseKeepingMorningDirtyHourProcess_SP"))
                    {
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
        public bool SynchronizeRoomInformationProcess()
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("RoomInformationProcess_SP"))
                    {
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
    }
}
