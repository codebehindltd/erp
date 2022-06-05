using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class RoomCalenderDA : BaseService
    {

        public List<RoomCalenderBO> GetRoomInfoForCalender(DateTime StartDate, DateTime EndDate)
        {
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomInfoForCalender"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, StartDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, EndDate);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomCalenderBO calenderBO = new RoomCalenderBO();

                                calenderBO.TransectionId = Convert.ToInt32(reader["TransactionId"]);
                                calenderBO.TransectionStatus = reader["TransactionStatus"].ToString();
                                calenderBO.RoomTypeId = Int32.Parse(reader["RoomTypeId"].ToString());
                                calenderBO.RoomType = reader["RoomType"].ToString();
                                calenderBO.GuestName = reader["GuestName"].ToString();
                                calenderBO.RoomNumber = reader["RoomNumber"].ToString();
                                calenderBO.RoomId = Int32.Parse(reader["RoomId"].ToString());
                                calenderBO.RoomInformation = reader["RoomInformation"].ToString();
                                calenderBO.CheckIn = Convert.ToDateTime(reader["CheckIn"].ToString());
                                calenderBO.CheckOut = Convert.ToDateTime(reader["CheckOut"].ToString());
                                calenderBO.OriginalCheckOutDate = Convert.ToDateTime(reader["OriginalCheckOutDate"].ToString());
                                calenderBO.ColorCodeName = reader["ColorCodeName"].ToString();
                                calenderBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                calenderList.Add(calenderBO);
                            }
                        }
                    }
                }
            }
            return calenderList;

        }
        public List<RoomCalenderBO> GetCheckedOutVacantDirtyRoomInfoForCalender()
        {
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCheckedOutVacantDirtyRoomInfoForCalender_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomCalenderBO calenderBO = new RoomCalenderBO();

                                calenderBO.TransectionId = Convert.ToInt32(reader["TransactionId"]);
                                calenderBO.TransectionStatus = reader["TransactionStatus"].ToString();
                                calenderBO.RoomTypeId = Int32.Parse(reader["RoomTypeId"].ToString());
                                calenderBO.RoomType = reader["RoomType"].ToString();
                                calenderBO.GuestName = reader["GuestName"].ToString();
                                calenderBO.RoomNumber = reader["RoomNumber"].ToString();
                                calenderBO.RoomId = Int32.Parse(reader["RoomId"].ToString());
                                calenderBO.RoomInformation = reader["RoomInformation"].ToString();
                                calenderBO.CheckIn = Convert.ToDateTime(reader["CheckIn"].ToString());
                                calenderBO.CheckOut = Convert.ToDateTime(reader["CheckOut"].ToString());
                                calenderBO.OriginalCheckOutDate = Convert.ToDateTime(reader["OriginalCheckOutDate"].ToString());
                                calenderBO.ColorCodeName = reader["ColorCodeName"].ToString();
                                calenderBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                calenderList.Add(calenderBO);
                            }
                        }
                    }
                }
            }
            return calenderList;

        }
    }
}
