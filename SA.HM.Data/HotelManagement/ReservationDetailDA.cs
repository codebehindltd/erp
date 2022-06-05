using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class ReservationDetailDA : BaseService
    {
        public List<ReservationDetailBO> GetReservationDetailByReservationId(int reservationId)
        {
            List<ReservationDetailBO> registrationDetailList = new List<ReservationDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservationDetailByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ReservationDetailBO registrationDetail = new ReservationDetailBO();

                                registrationDetail.ReservationDetailId = Convert.ToInt32(reader["ReservationDetailId"]);
                                registrationDetail.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                registrationDetail.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                registrationDetail.RoomType = reader["RoomType"].ToString();
                                registrationDetail.RoomId = Convert.ToInt32(reader["RoomId"]);
                                registrationDetail.RoomNumber = reader["RoomNumber"].ToString();

                                registrationDetailList.Add(registrationDetail);
                            }
                        }
                    }
                }
            }
            return registrationDetailList;
        }
        //public List<ReservationDetailBO> GetOnlineReservationDetailByReservationId(int reservationId)
        //{
        //    List<ReservationDetailBO> registrationDetailList = new List<ReservationDetailBO>();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOnlineReservationDetailByReservationId_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        ReservationDetailBO registrationDetail = new ReservationDetailBO();

        //                        registrationDetail.ReservationDetailId = Convert.ToInt32(reader["ReservationDetailId"]);
        //                        registrationDetail.ReservationId = Convert.ToInt32(reader["ReservationId"]);
        //                        registrationDetail.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
        //                        registrationDetail.RoomType = reader["RoomType"].ToString();
        //                        registrationDetail.RoomId = Convert.ToInt32(reader["RoomId"]);
        //                        registrationDetail.RoomNumber = reader["RoomNumber"].ToString();

        //                        registrationDetailList.Add(registrationDetail);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return registrationDetailList;
        //}
        public List<ReservationDetailBO> GetRoomReservationInfoByExpectedArrivalDate(DateTime expectedArrivalDate, string guestName, string companyName, string reservNumber)
        {
            int counter = 1;
            List<ReservationDetailBO> registrationDetailList = new List<ReservationDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfoByExpectedArrivalDate_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ExpectedArrivalDate", DbType.DateTime, expectedArrivalDate);
                    dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, guestName);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    dbSmartAspects.AddInParameter(cmd, "@ReservNumber", DbType.String, reservNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ReservationDetailBO registrationDetail = new ReservationDetailBO();

                                registrationDetail.ReservationDetailId = counter++;
                                registrationDetail.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                registrationDetail.ReservationNumber = reader["ReservationNumber"].ToString();
                                registrationDetail.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                registrationDetail.RoomType = reader["RoomType"].ToString();
                                registrationDetail.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                registrationDetail.DiscountType = reader["DiscountType"].ToString();
                                registrationDetail.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                registrationDetail.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                registrationDetail.RoomNumberIdList = reader["RoomNumberIdList"].ToString();
                                registrationDetail.RoomNumberList = reader["RoomNumberList"].ToString();
                                registrationDetail.RoomNumberListInfoWithCount = reader["RoomNumberListInfoWithCount"].ToString();
                                registrationDetail.ReservationDetailIdList = reader["ReservationDetailIdList"].ToString();
                                registrationDetail.TotalRoom = Convert.ToInt32(reader["TotalRoom"].ToString());
                                registrationDetail.RoomTypeWisePaxQuantity = Convert.ToInt32(reader["RoomTypeWisePaxQuantity"].ToString());
                                registrationDetail.IsAdditionalChargeEnable = Convert.ToBoolean(reader["IsAdditionalChargeEnable"]);
                                registrationDetail.IsCityChargeEnable = Convert.ToBoolean(reader["IsCityChargeEnable"]);
                                registrationDetail.IsServiceChargeEnable = Convert.ToBoolean(reader["IsServiceChargeEnable"]);
                                registrationDetail.IsVatAmountEnable = Convert.ToBoolean(reader["IsVatAmountEnable"]);
                                if (reader["TotalRoomRate"] != DBNull.Value)
                                    registrationDetail.TotalCalculatedAmount = Convert.ToDecimal(reader["TotalRoomRate"]);

                                registrationDetail.ArrivalDate = reader["ArrivalDate"].ToString();
                                registrationDetail.DepartureDate = reader["DepartureDate"].ToString();

                                registrationDetailList.Add(registrationDetail);
                            }
                        }
                    }
                }
            }
            return registrationDetailList;
        }
        public List<ReservationDetailBO> GetReservationDetailByRegiIdForGrid(int reservationDetailId, int isRegisteredAlso)
        {
            int counter = 1;
            List<ReservationDetailBO> registrationDetailList = new List<ReservationDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservationDetailByReservId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationDetailId);
                    dbSmartAspects.AddInParameter(cmd, "@IsRegisteredAlso", DbType.Int32, isRegisteredAlso);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ReservationDetailBO registrationDetail = new ReservationDetailBO();

                                registrationDetail.ReservationDetailId = counter++;
                                registrationDetail.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                registrationDetail.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                registrationDetail.RoomType = reader["RoomType"].ToString();
                                registrationDetail.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                registrationDetail.DiscountType = reader["DiscountType"].ToString();
                                registrationDetail.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                registrationDetail.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                registrationDetail.RoomNumberIdList = reader["RoomNumberIdList"].ToString();
                                registrationDetail.RoomNumberList = reader["RoomNumberList"].ToString();
                                registrationDetail.RoomNumberListInfoWithCount = reader["RoomNumberListInfoWithCount"].ToString();
                                registrationDetail.ReservationDetailIdList = reader["ReservationDetailIdList"].ToString();
                                registrationDetail.TotalRoom = Convert.ToInt32(reader["TotalRoom"].ToString());
                                registrationDetail.RoomTypeWisePaxQuantity = Convert.ToInt32(reader["RoomTypeWisePaxQuantity"].ToString());
                                registrationDetail.IsAdditionalChargeEnable =Convert.ToBoolean( reader["IsAdditionalChargeEnable"]);
                                registrationDetail.IsCityChargeEnable= Convert.ToBoolean(reader["IsCityChargeEnable"]);
                                registrationDetail.IsServiceChargeEnable= Convert.ToBoolean(reader["IsServiceChargeEnable"]);
                                registrationDetail.IsVatAmountEnable= Convert.ToBoolean(reader["IsVatAmountEnable"]);
                                if(reader["TotalRoomRate"]!=DBNull.Value)
                                    registrationDetail.TotalCalculatedAmount = Convert.ToDecimal(reader["TotalRoomRate"]);

                                registrationDetail.ArrivalDate = reader["ArrivalDate"].ToString();
                                registrationDetail.DepartureDate = reader["DepartureDate"].ToString();

                                registrationDetailList.Add(registrationDetail);
                            }
                        }
                    }
                }
            }
            return registrationDetailList;
        }
        public List<ReservationDetailBO> GetOnlineReservationDetailByRegiIdForGrid(int reservationDetailId, int isRegisteredAlso)
        {
            List<ReservationDetailBO> registrationDetailList = new List<ReservationDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOnlineReservationDetailByReservId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationDetailId);
                    dbSmartAspects.AddInParameter(cmd, "@IsRegisteredAlso", DbType.Int32, isRegisteredAlso);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ReservationDetailBO registrationDetail = new ReservationDetailBO();

                                //registrationDetail.ReservationDetailId = Convert.ToInt32(reader["ReservationDetailId"]);
                                registrationDetail.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                registrationDetail.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                registrationDetail.RoomType = reader["RoomType"].ToString();
                                registrationDetail.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                registrationDetail.DiscountType = reader["DiscountType"].ToString();
                                registrationDetail.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                registrationDetail.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                registrationDetail.RoomNumberIdList = reader["RoomNumberIdList"].ToString();
                                registrationDetail.RoomNumberList = reader["RoomNumberList"].ToString();
                                registrationDetail.RoomNumberListInfoWithCount = reader["RoomNumberListInfoWithCount"].ToString();                                
                                registrationDetail.TotalRoom = Convert.ToInt32(reader["TotalRoom"].ToString());

                                registrationDetailList.Add(registrationDetail);
                            }
                        }
                    }
                }
            }
            return registrationDetailList;
        }
        public List<ReservationDetailBO> GetReservationDetailForRegistrationByRegiId(int reservationDetailId)
        {
            List<ReservationDetailBO> registrationDetailList = new List<ReservationDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservationDetailForRegistrationByRegiId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationDetailId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ReservationDetailBO registrationDetail = new ReservationDetailBO();

                                //registrationDetail.ReservationDetailId = Convert.ToInt32(reader["ReservationDetailId"]);
                                registrationDetail.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                registrationDetail.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                registrationDetail.RoomType = reader["RoomType"].ToString();
                                registrationDetail.RoomNumber = reader["RoomNumber"].ToString();
                                registrationDetail.DirtyRoomNumber = reader["DirtyRoom"].ToString();

                                registrationDetailList.Add(registrationDetail);
                            }
                        }
                    }
                }
            }
            return registrationDetailList;
        }
        public List<ReservationDetailBO> GetReservationDetailForNoShowReservations(int reservationId, string noshowType)
        {
            List<ReservationDetailBO> reservationDetailList = new List<ReservationDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservationDetailsForNoShowReservation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
                    dbSmartAspects.AddInParameter(cmd, "@NoShowType", DbType.String, noshowType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ReservationDetailBO reservationDetail = new ReservationDetailBO();

                                reservationDetail.ReservationDetailId = Convert.ToInt32(reader["ReservationDetailId"]);
                                reservationDetail.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                reservationDetail.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                reservationDetail.RoomType = reader["RoomType"].ToString();
                                reservationDetail.RoomId = Convert.ToInt32(reader["RoomId"]);
                                reservationDetail.RoomNumber = reader["RoomNumber"].ToString();
                                reservationDetail.CurrencyHead = reader["CurrencyHead"].ToString();
                                reservationDetail.CurrencyType = Convert.ToInt32(reader["CurrencyType"]);
                                reservationDetail.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                reservationDetail.NoShowCharge = Convert.ToDecimal(reader["NoShowCharge"]);
                                reservationDetail.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                reservationDetailList.Add(reservationDetail);
                            }
                        }
                    }
                }
            }
            return reservationDetailList;
        }
        public Boolean UpdateReservationDetailForNoShowCharge(List<ReservationDetailBO> reservationDetails)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReservationDetailForNoShowCharge_SP"))
                {
                    foreach (ReservationDetailBO reservationDetailBO in reservationDetails)
                    {
                        commandDetails.Parameters.Clear();

                        dbSmartAspects.AddInParameter(commandDetails, "@ReservationDetailId", DbType.Int32, reservationDetailBO.ReservationDetailId);
                        dbSmartAspects.AddInParameter(commandDetails, "@NoShowCharge", DbType.Decimal, reservationDetailBO.NoShowCharge);

                        status = dbSmartAspects.ExecuteNonQuery(commandDetails) > 0 ? true : false; ;
                    }
                }
            }
            return status;
        }
        public ReservationDetailBO GetRoomReservationDetailByResIdAndRoomTypeId(int reservationId, int roomTypeId)
        {
            ReservationDetailBO detailBO = new ReservationDetailBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationDetailByResIdAndRoomTypeId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
                    dbSmartAspects.AddInParameter(cmd, "@RoomTypeId", DbType.Int32, roomTypeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                detailBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                detailBO.DiscountType = reader["DiscountType"].ToString();
                                detailBO.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                detailBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                detailBO.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                detailBO.CurrencyType = Convert.ToInt32(reader["CurrencyType"]);
                                detailBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                detailBO.ReferenceId = Convert.ToInt32(reader["ReferenceId"]);
                            }
                        }
                    }
                }
            }
            return detailBO;
        }
        public ReservationDetailBO GetRoomReservationDetailByResIdAndRoomId(int reservationId, int reservationDetailId, int roomId)
        {
            ReservationDetailBO detailBO = new ReservationDetailBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationDetailByResIdAndRoomId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
                    dbSmartAspects.AddInParameter(cmd, "@ReservationDetailId", DbType.Int32, reservationDetailId);
                    dbSmartAspects.AddInParameter(cmd, "@RoomId", DbType.Int32, roomId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                detailBO.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                detailBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                detailBO.DiscountType = reader["DiscountType"].ToString();
                                detailBO.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                detailBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                detailBO.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                detailBO.CurrencyType = Convert.ToInt32(reader["CurrencyType"]);
                                detailBO.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                detailBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                detailBO.RoomNumber = reader["RoomNumber"].ToString();
                                detailBO.ReferenceId = Convert.ToInt32(reader["ReferenceId"]);
                                detailBO.GuestId = Convert.ToInt32(reader["GuestId"]);
                                detailBO.ReservationNDetailNRoomId = reader["ReservationId"].ToString() + "~" + reader["ReservationDetailId"].ToString() + "~" + reader["RoomId"].ToString() + "~" + reader["GuestId"].ToString();
                            }
                        }
                    }
                }
            }
            return detailBO;
        }
        public ReservationDetailBO GetRoomTypeInfoForReservation(int reservationId, int reservationDetailId, int roomId)
        {
            ReservationDetailBO detailBO = new ReservationDetailBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGetRoomTypeInfoForReservation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
                    dbSmartAspects.AddInParameter(cmd, "@ReservationDetailId", DbType.Int32, reservationDetailId);
                    dbSmartAspects.AddInParameter(cmd, "@RoomId", DbType.Int32, roomId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                detailBO.ReservationDetailId = Convert.ToInt32(reader["ReservationDetailId"]);
                                detailBO.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                //detailBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                //detailBO.DiscountType = reader["DiscountType"].ToString();
                                //detailBO.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                //detailBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                //detailBO.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                //detailBO.CurrencyType = Convert.ToInt32(reader["CurrencyType"]);
                                detailBO.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                detailBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                //detailBO.ReferenceId = Convert.ToInt32(reader["ReferenceId"]);
                                //detailBO.GuestId = Convert.ToInt32(reader["GuestId"]);
                                //detailBO.ReservationNDetailNRoomId = reader["ReservationId"].ToString() + "~" + reader["ReservationDetailId"].ToString() + "~" + reader["RoomId"].ToString();
                            }
                        }
                    }
                }
            }
            return detailBO;
        }

        ////------------Online Reservation------------------------------------------------------
        public List<ReservationDetailBO> GetOnlineReservationDetailByReservationId(int reservationId)
        {
            List<ReservationDetailBO> registrationDetailList = new List<ReservationDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOnlineReservationDetailByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ReservationDetailBO registrationDetail = new ReservationDetailBO();

                                registrationDetail.ReservationDetailId = Convert.ToInt32(reader["ReservationDetailId"]);
                                registrationDetail.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                registrationDetail.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                registrationDetail.RoomType = reader["RoomType"].ToString();
                                registrationDetail.RoomId = Convert.ToInt32(reader["RoomId"]);
                                registrationDetail.RoomNumber = reader["RoomNumber"].ToString();

                                registrationDetailList.Add(registrationDetail);
                            }
                        }
                    }
                }
            }
            return registrationDetailList;
        }
    }
}
