using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class GuestRegistrationDA : BaseService
    {
        public List<GuestCheckOutViewBO> GetActiveGuestInfoByRegiId(int registrationId)
        {
            List<GuestCheckOutViewBO> activeGuestList = new List<GuestCheckOutViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestCheckOutInfoByRegId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestCheckOutViewBO activeGuest = new GuestCheckOutViewBO();

                                activeGuest.Id = Convert.ToInt32(reader["GuestRegistrationId"]);
                                activeGuest.GuestName = reader["GuestName"].ToString();
                                activeGuest.GuestPhone = reader["GuestPhone"].ToString();
                                activeGuest.CheckInDate = Convert.ToDateTime(reader["CheckInDate"]);
                                if (!string.IsNullOrEmpty(reader["CheckOutDate"].ToString()))
                                {
                                    activeGuest.CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"].ToString());
                                }
                                else
                                {
                                    activeGuest.CheckOutDate = null;
                                }
                                activeGuest.PaxInRate = Convert.ToDecimal(reader["PaxInRate"]);

                                activeGuestList.Add(activeGuest);
                            }
                        }
                    }
                }
            }
            return activeGuestList;
        }
        public GuestCheckOutViewBO GetGuestRegInfoById(int guestRegId)
        {
            GuestCheckOutViewBO guestRegInfo = new GuestCheckOutViewBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestRegistrationById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GuestRegistrationId", DbType.Int32, guestRegId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                if (!string.IsNullOrEmpty(reader["CheckOutDate"].ToString()))
                                {
                                    guestRegInfo.CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"].ToString());
                                }
                                else
                                {
                                    guestRegInfo.CheckOutDate = null;
                                }                                                               
                            }
                        }
                    }
                }
            }
            return guestRegInfo;
        }

        public bool UpdateGuestRegistrationById(int guestRegId, string IsCheckout, int UserInfoId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGuestRegistrationById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@GuestRegistrationId", DbType.Int32, guestRegId);
                    dbSmartAspects.AddInParameter(command, "@IsCheckOut", DbType.String, IsCheckout);
                    dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, UserInfoId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        //GetWalkInReport_SP GetRoomBlocksReport_SP
        public List<ComplementeryGuestInfoBO> GetComplementeryGuestInfoReport(DateTime dateFrom, DateTime dateTo, string roomNo, string regNo, int companyId)
        {
            List<ComplementeryGuestInfoBO> guestInfoBOs = new List<ComplementeryGuestInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetComplementeryGuestInfoReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, dateTo);
                    if (string.IsNullOrEmpty(roomNo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNo);
                    }
                    if (string.IsNullOrEmpty(regNo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RegistrationNumber", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RegistrationNumber", DbType.String, regNo);
                    }
                    if (companyId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId); 
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.String, DBNull.Value);
                    }

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ComplementeryGuestInfoBO infoBO = new ComplementeryGuestInfoBO()
                                {
                                    RoomNumber = reader["RoomNumber"].ToString(),
                                    RegistrationNumber = reader["RegistrationNumber"].ToString(),
                                    FollioNo = reader["FollioNo"].ToString(),
                                    PaymentMode = reader["PaymentMode"].ToString(),
                                    Pax = Convert.ToInt32( reader["Pax"]),
                                    Adult = Convert.ToInt32(reader["Adult"]),
                                    Child = Convert.ToInt32(reader["Child"]),
                                    GuestName = reader["GuestName"].ToString(),
                                    ComplementaryRemarks = reader["ComplementaryRemarks"].ToString(),
                                    Status = reader["Status"].ToString(),
                                    GuestNationality = reader["GuestNationality"].ToString(),
                                    ArriveDate = Convert.ToDateTime(reader["ArriveDate"]),
                                    ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]),
                                    ArriveDateString = reader["ArriveDateString"].ToString(),
                                    ExpectedCheckOutDateString = reader["ExpectedCheckOutDateString"].ToString(),
                                    BillInstruction = reader["BillInstruction"].ToString(),
                                    BusinessSource = reader["BusinessSource"].ToString(),
                                    MarketSegment = reader["MarketSegment"].ToString(),
                                    UserName = reader["UserName"].ToString(),
                                    GuestCompany = reader["GuestCompany"].ToString(),
                                };
                                guestInfoBOs.Add(infoBO);
                            }
                        }
                    }
                }
            }

                return guestInfoBOs;
        }

        public List<WalkInGuestReportBO> GetWalkInGuestReport_SP(DateTime dateFrom, DateTime dateTo, string roomNo, string regNo)
        {
            List<WalkInGuestReportBO> guestInfoBOs = new List<WalkInGuestReportBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetWalkInReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, dateTo);

                    if (string.IsNullOrEmpty(roomNo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNo);
                    }
                    if (string.IsNullOrEmpty(regNo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RegistrationNumber", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RegistrationNumber", DbType.String, regNo);
                    }
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                WalkInGuestReportBO walkInGuest = new WalkInGuestReportBO
                                {
                                    RoomNumber = reader["RoomNumber"].ToString(),
                                    FolioNo = reader["FolioNo"].ToString(),
                                    Pax = Convert.ToInt32(reader["Pax"]),
                                    Adult = Convert.ToInt32(reader["Adult"]),
                                    Child = Convert.ToInt32(reader["Child"]),
                                    RegistrationNumber = reader["RegistrationNumber"].ToString(),
                                    GuestName = reader["GuestName"].ToString(),
                                    CompanyName = reader["CompanyName"].ToString(),
                                    //ArriveDate = Convert.ToDateTime(reader["ArriveDate"]),
                                    //ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]),
                                    ArriveDateString = reader["ArriveDateString"].ToString(),
                                    ExpectedCheckOutDateString = reader["ExpectedCheckOutDateString"].ToString(),
                                    GuestNationality = reader["GuestNationality"].ToString(),
                                    UserName = reader["UserName"].ToString(),
                                    PL = reader["PL"].ToString()
                                };
                                guestInfoBOs.Add(walkInGuest);
                            }
                        }
                    }
                }
            }

                return guestInfoBOs;
        }

        public List<RoomBlocksReportBO> GetRoomBlocksReport(DateTime dateFrom, DateTime dateTo, string roomNo)
        {
            List<RoomBlocksReportBO> roomBlocks = new List<RoomBlocksReportBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                try
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomBlocksReport_SP"))
                    {
                        cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, dateFrom);
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, dateTo);

                        if (string.IsNullOrEmpty(roomNo))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, DBNull.Value);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNo);
                        }

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    RoomBlocksReportBO room = new RoomBlocksReportBO
                                    {

                                        RoomNumber = reader["RoomNumber"].ToString(),
                                        Reference = reader["Reference"].ToString(),
                                        TypeCode = reader["TypeCode"].ToString(),
                                        RoomType = reader["RoomType"].ToString(),
                                        StatusName = reader["StatusName"].ToString(),
                                        FromDate = Convert.ToDateTime(reader["FromDate"]),
                                        ToDate = Convert.ToDateTime(reader["ToDate"]),
                                        Description = reader["Description"].ToString(),
                                        UserName = reader["UserName"].ToString()
                                    };
                                    roomBlocks.Add(room);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                
            }

                return roomBlocks;
        }
    }
}
