using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;
using System.Collections;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.Inventory;

namespace HotelManagement.Data.HotelManagement
{
    public class RoomReservationDA : BaseService
    {
        public List<RoomReservationBO> GetRoomReservationInfo()
        {
            List<RoomReservationBO> roomReservationList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomReservation = new RoomReservationBO();

                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"]);
                                roomReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                roomReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);

                                if (!string.IsNullOrEmpty(reader["ConfirmationDate"].ToString()))
                                    roomReservation.ConfirmationDate = Convert.ToDateTime(reader["ConfirmationDate"]);

                                roomReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                                roomReservation.ContactPerson = reader["ContactPerson"].ToString();
                                roomReservation.ContactNumber = reader["ContactNumber"].ToString();
                                roomReservation.GuestId = Int32.Parse(reader["GuestId"].ToString());
                                roomReservation.Reason = reader["Reason"].ToString();
                                roomReservation.BusinessPromotionId = Int32.Parse(reader["BusinessPromotionId"].ToString());
                                roomReservation.TotalRoomNumber = Convert.ToInt32(reader["TotalRoomNumber"]);
                                roomReservation.ReservedMode = reader["ReservedMode"].ToString();
                                roomReservation.ReservationType = reader["ReservationType"].ToString();
                                roomReservation.ReservationMode = reader["ReservationMode"].ToString();
                                roomReservation.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomReservation.ShowDateIn = reader["ShowDateIn"].ToString();
                                roomReservationList.Add(roomReservation);
                            }
                        }
                    }
                }
            }
            return roomReservationList;
        }
        public List<RoomReservationBO> GetRoomReservationInfoWithReservationNumber()
        {
            List<RoomReservationBO> roomReservationList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfoWithReservationNumber_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomReservation = new RoomReservationBO();

                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"].ToString());
                                roomReservation.DateIn = Convert.ToDateTime(reader["DateIn"].ToString());
                                roomReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);

                                if (!string.IsNullOrEmpty(reader["ConfirmationDate"].ToString()))
                                    roomReservation.ConfirmationDate = Convert.ToDateTime(reader["ConfirmationDate"]);

                                roomReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                                roomReservation.ContactPerson = reader["ContactPerson"].ToString();
                                roomReservation.ContactNumber = reader["ContactNumber"].ToString();
                                roomReservation.GuestId = Int32.Parse(reader["GuestId"].ToString());
                                roomReservation.Reason = reader["Reason"].ToString();
                                roomReservation.BusinessPromotionId = Int32.Parse(reader["BusinessPromotionId"].ToString());
                                roomReservation.TotalRoomNumber = Convert.ToInt32(reader["TotalRoomNumber"]);
                                roomReservation.ReservedMode = reader["ReservedMode"].ToString();
                                roomReservation.ReservationType = reader["ReservationType"].ToString();
                                roomReservation.ReservationMode = reader["ReservationMode"].ToString();
                                roomReservationList.Add(roomReservation);
                            }
                        }
                    }
                }
            }
            return roomReservationList;
        }
        public List<RoomReservationBO> GetRoomReservationInfoForRegistration(int IsAllActiveReservation)
        {
            List<RoomReservationBO> roomReservationList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfoForRegistration_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@IsAllActiveReservation", DbType.Int32, IsAllActiveReservation);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomReservation = new RoomReservationBO();
                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"]);
                                roomReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                roomReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);

                                if (!string.IsNullOrEmpty(reader["ConfirmationDate"].ToString()))
                                    roomReservation.ConfirmationDate = Convert.ToDateTime(reader["ConfirmationDate"]);

                                roomReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                                roomReservation.ContactPerson = reader["ContactPerson"].ToString();
                                roomReservation.ContactNumber = reader["ContactNumber"].ToString();
                                roomReservation.GuestId = Int32.Parse(reader["GuestId"].ToString());
                                roomReservation.Reason = reader["Reason"].ToString();
                                roomReservation.TotalRoomNumber = Convert.ToInt32(reader["TotalRoomNumber"]);
                                roomReservation.ReservedMode = reader["ReservedMode"].ToString();
                                roomReservation.ReservationType = reader["ReservationType"].ToString();
                                roomReservation.ReservationMode = reader["ReservationMode"].ToString();
                                roomReservationList.Add(roomReservation);
                            }
                        }
                    }
                }
            }
            return roomReservationList;
        }
        public List<RoomReservationBO> GetRoomReservationInformationForRegistration(int IsAllActiveReservation)
        {
            List<RoomReservationBO> roomReservationList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInformationForRegistration_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@IsAllActiveReservation", DbType.Int32, IsAllActiveReservation);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomReservation = new RoomReservationBO();
                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                                roomReservation.ReservationNDetailNRoomId = reader["ReservationId"].ToString() + "~" + reader["ReservationDetailId"].ToString() + "~" + reader["RoomId"].ToString() + "~" + reader["GuestId"].ToString();
                                roomReservationList.Add(roomReservation);
                            }
                        }
                    }
                }
            }
            return roomReservationList;
        }
        public List<RoomReservationBO> GetRoomReservationInfoForRegistrationSearch(string reservationNumber, string guestName, int IsAllActiveReservation)
        {
            List<RoomReservationBO> roomReservationList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfoForRegistrationSearch_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@IsAllActiveReservation", DbType.Int32, IsAllActiveReservation);

                    if (!string.IsNullOrWhiteSpace(reservationNumber))
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, reservationNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(guestName))
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, guestName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomReservation = new RoomReservationBO();

                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"]);
                                roomReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                roomReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);

                                if (!string.IsNullOrEmpty(reader["ConfirmationDate"].ToString()))
                                    roomReservation.ConfirmationDate = Convert.ToDateTime(reader["ConfirmationDate"]);

                                roomReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                                roomReservation.ContactPerson = reader["ContactPerson"].ToString();
                                roomReservation.ContactNumber = reader["ContactNumber"].ToString();
                                roomReservation.GuestId = Int32.Parse(reader["GuestId"].ToString());
                                roomReservation.Reason = reader["Reason"].ToString();
                                roomReservation.TotalRoomNumber = Convert.ToInt32(reader["TotalRoomNumber"]);
                                roomReservation.ReservedMode = reader["ReservedMode"].ToString();
                                roomReservation.ReservationType = reader["ReservationType"].ToString();
                                roomReservation.ReservationMode = reader["ReservationMode"].ToString();
                                roomReservationList.Add(roomReservation);
                            }
                        }
                    }
                }
            }
            return roomReservationList;
        }
        public List<RoomReservationBO> GetRoomReservationInfoByOnlyDateRange(DateTime fromDate, DateTime toDate)
        {
            List<RoomReservationBO> roomReservationList = new List<RoomReservationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfoByOnlyDateRange_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);


                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomReservation = new RoomReservationBO();

                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"]);
                                roomReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                roomReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);
                                roomReservation.ReservationDetailId = Int32.Parse(reader["ReservationDetailId"].ToString());
                                roomReservation.RoomTypeId = Int32.Parse(reader["RoomTypeId"].ToString());
                                
                                roomReservationList.Add(roomReservation);
                            }
                        }
                    }
                }
                return roomReservationList;
            }
        }
        public List<RoomReservationBO> GetRoomReservationRoomInfoByDateRange(DateTime fromDate, DateTime toDate)
        {
            List<RoomReservationBO> roomReservationList = new List<RoomReservationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationRoomInfoByOnlyDateRange_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomReservation = new RoomReservationBO();

                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"]);
                                roomReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                roomReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);                                
                                roomReservation.RoomTypeId = Int32.Parse(reader["RoomTypeId"].ToString());
                                roomReservation.TotalRoomNumber = Convert.ToInt32(reader["RoomQuantity"]);
                                roomReservationList.Add(roomReservation);
                            }
                        }
                    }
                }
                return roomReservationList;
            }
        }

        public Boolean SaveRoomReservationInfo(RoomReservationBO roomReservation, out int tmpReservationId, List<ReservationDetailBO> detailBO, List<ReservationComplementaryItemBO> complementaryItemBOList, out string currentReservationNumber, List<RegistrationServiceInfoBO> paidServiceDetails, bool paidServiceTobeDelete)
        {
            bool retVal = false;
            int status = 0;
            tmpReservationId = 0;
            currentReservationNumber = string.Empty;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveRoomReservationInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@OnlineReservationId", DbType.Int32, roomReservation.OnlineReservationId);
                            dbSmartAspects.AddInParameter(commandMaster, "@DateIn", DbType.DateTime, roomReservation.DateIn);
                            dbSmartAspects.AddInParameter(commandMaster, "@DateOut", DbType.DateTime, roomReservation.DateOut);
                            dbSmartAspects.AddInParameter(commandMaster, "@ConfirmationDate", DbType.DateTime, roomReservation.ConfirmationDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservedCompany", DbType.String, roomReservation.ReservedCompany);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactAddress", DbType.String, roomReservation.ContactAddress);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, roomReservation.ContactPerson);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactNumber", DbType.String, roomReservation.ContactNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@MobileNumber", DbType.String, roomReservation.MobileNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@FaxNumber", DbType.String, roomReservation.FaxNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactEmail", DbType.String, roomReservation.ContactEmail);
                            dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, roomReservation.GuestId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Reason", DbType.String, roomReservation.Reason);
                            dbSmartAspects.AddInParameter(commandMaster, "@BusinessPromotionId", DbType.Int32, roomReservation.BusinessPromotionId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReferenceId", DbType.Int32, roomReservation.ReferenceId);
                            dbSmartAspects.AddInParameter(commandMaster, "@TotalRoomNumber", DbType.Int32, roomReservation.TotalRoomNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservedMode", DbType.String, roomReservation.ReservedMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationType", DbType.String, roomReservation.ReservationType);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationMode", DbType.String, roomReservation.ReservationMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsListedCompany", DbType.Boolean, roomReservation.IsListedCompany);
                            dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonAdult", DbType.Int32, roomReservation.NumberOfPersonAdult);
                            dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonChild", DbType.Int32, roomReservation.NumberOfPersonChild);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsFamilyOrCouple", DbType.Boolean, roomReservation.IsFamilyOrCouple);
                            dbSmartAspects.AddInParameter(commandMaster, "@CurrencyType", DbType.Int32, roomReservation.CurrencyType);
                            dbSmartAspects.AddInParameter(commandMaster, "@ConversionRate", DbType.Decimal, roomReservation.ConversionRate);
                            dbSmartAspects.AddInParameter(commandMaster, "@PaymentMode", DbType.String, roomReservation.PaymentMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@PayFor", DbType.Int32, roomReservation.PayFor);
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, roomReservation.CompanyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, roomReservation.Remarks);
                            //--Aireport Pickup Drop Information-------------------------------------------
                            dbSmartAspects.AddInParameter(commandMaster, "@AirportPickUp", DbType.String, roomReservation.AirportPickUp);
                            dbSmartAspects.AddInParameter(commandMaster, "@AirportDrop", DbType.String, roomReservation.AirportDrop);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsAirportPickupDropExist", DbType.Int32, roomReservation.IsAirportPickupDropExist);
                            dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightName", DbType.String, roomReservation.ArrivalFlightName);
                            dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightNumber", DbType.String, roomReservation.ArrivalFlightNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ArrivalTime", DbType.DateTime, roomReservation.ArrivalTime);
                            dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightName", DbType.String, roomReservation.DepartureFlightName);
                            dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightNumber", DbType.String, roomReservation.DepartureFlightNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@DepartureTime", DbType.DateTime, roomReservation.DepartureTime);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsRoomRateShowInPreRegistrationCard", DbType.Boolean, roomReservation.IsRoomRateShowInPreRegistrationCard);
                            //--Aireport Pickup Drop Information------------------------------End--------
                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, roomReservation.CreatedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@tempResId", DbType.Int32, roomReservation.ReservationId);
                            dbSmartAspects.AddOutParameter(commandMaster, "@ReservationId", DbType.Int32, sizeof(Int32));
                            dbSmartAspects.AddOutParameter(commandMaster, "@ReservationNumber", DbType.String, 50);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                            tmpReservationId = Convert.ToInt32(commandMaster.Parameters["@ReservationId"].Value);
                            currentReservationNumber = (commandMaster.Parameters["@ReservationNumber"].Value).ToString();
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveReservationDetailInfo_SP"))
                            {
                                foreach (ReservationDetailBO reservationDetailBO in detailBO)
                                {
                                    commandDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int32, tmpReservationId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeId", DbType.Int32, reservationDetailBO.RoomTypeId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomId", DbType.Int32, reservationDetailBO.RoomId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomRate", DbType.Decimal, reservationDetailBO.RoomRate);
                                    dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, reservationDetailBO.UnitPrice);
                                    dbSmartAspects.AddInParameter(commandDetails, "@DiscountAmount", DbType.Decimal, reservationDetailBO.DiscountAmount);
                                    dbSmartAspects.AddInParameter(commandDetails, "@DiscountType", DbType.String, reservationDetailBO.DiscountType);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeWisePaxQuantity", DbType.String, reservationDetailBO.RoomTypeWisePaxQuantity);
                                    dbSmartAspects.AddInParameter(commandDetails, "@IsServiceChargeEnable", DbType.Boolean, reservationDetailBO.IsServiceChargeEnable);
                                    dbSmartAspects.AddInParameter(commandDetails, "@IsCityChargeEnable", DbType.Boolean, reservationDetailBO.IsCityChargeEnable);
                                    dbSmartAspects.AddInParameter(commandDetails, "@IsVatAmountEnable", DbType.Boolean, reservationDetailBO.IsVatAmountEnable);
                                    dbSmartAspects.AddInParameter(commandDetails, "@IsAdditionalChargeEnable", DbType.Boolean, reservationDetailBO.IsAdditionalChargeEnable);
                                    dbSmartAspects.AddInParameter(commandDetails, "@TotalRoomRate", DbType.Decimal, reservationDetailBO.TotalCalculatedAmount);


                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }

                            //if (count == detailBO.Count)
                            if (complementaryItemBOList != null && status > 0)
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveReservationComplementaryItemInfo_SP"))
                                {
                                    foreach (ReservationComplementaryItemBO complementaryItemBO in complementaryItemBOList)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int32, tmpReservationId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ComplementaryItemId", DbType.Int32, complementaryItemBO.ComplementaryItemId);
                                        status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            //-------------Paid Service Save, Update, Delete ----------------------------

                            if (paidServiceTobeDelete && status > 0)
                            {
                                using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteHotelReservationPaidService_SP"))
                                {
                                    commandDelete.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDelete, "@ReservationId", DbType.Int32, tmpReservationId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDelete);
                                }
                            }

                            if (status > 0)
                            {
                                using (DbCommand commandPaidSave = dbSmartAspects.GetStoredProcCommand("SaveReservationServiceInfo_SP"))
                                {
                                    if (paidServiceDetails != null)
                                    {
                                        if (paidServiceDetails.Count > 0)
                                        {
                                            foreach (RegistrationServiceInfoBO comItemBO in paidServiceDetails)
                                            {
                                                commandPaidSave.Parameters.Clear();

                                                dbSmartAspects.AddInParameter(commandPaidSave, "@ReservationId", DbType.Int32, tmpReservationId);
                                                dbSmartAspects.AddInParameter(commandPaidSave, "@ServiceId", DbType.Int32, comItemBO.ServiceId);

                                                if (roomReservation.IsListedCompany == true)
                                                {
                                                    dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, 0);
                                                }
                                                else
                                                {
                                                    if (roomReservation.CurrencyType == 45)
                                                    {
                                                        dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice);
                                                    }
                                                    else
                                                    {
                                                        dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice * roomReservation.ConversionRate);
                                                    }
                                                }

                                                dbSmartAspects.AddInParameter(commandPaidSave, "@IsAchieved", DbType.Boolean, comItemBO.IsAchieved);
                                                dbSmartAspects.AddOutParameter(commandPaidSave, "@DetailServiceId", DbType.Int32, sizeof(Int32));
                                                status = dbSmartAspects.ExecuteNonQuery(commandPaidSave);
                                            }
                                        }
                                    }
                                }
                            }
                            //-------------** End Region -- Paid Service Save, Update, Delete ----------------------------
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
        // ---- New Save, Update
        public Boolean SaveRoomReservationInfo(RoomReservationBO RoomReservation, List<ReservationDetailBO> RoomReservationDetail, HotelReservationAireportPickupDropBO AireportPickupDrop, List<ReservationComplementaryItemBO> ComplementaryItem, List<RegistrationServiceInfoBO> PaidServiceDetails, bool paidServiceDeleted, out int tmpReservationId, out string currentReservationNumber)
        {
            bool retVal = false;
            int status = 0;
            tmpReservationId = 0;
            currentReservationNumber = string.Empty;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveRoomReservationInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@OnlineReservationId", DbType.Int32, RoomReservation.OnlineReservationId);
                            dbSmartAspects.AddInParameter(commandMaster, "@DateIn", DbType.DateTime, RoomReservation.DateIn);
                            dbSmartAspects.AddInParameter(commandMaster, "@DateOut", DbType.DateTime, RoomReservation.DateOut);

                            if (RoomReservation.ConfirmationDate != null)
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@ConfirmationDate", DbType.DateTime, RoomReservation.ConfirmationDate);
                            }
                            else
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@ConfirmationDate", DbType.DateTime, DBNull.Value);
                            }

                            dbSmartAspects.AddInParameter(commandMaster, "@ReservedCompany", DbType.String, RoomReservation.ReservedCompany);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactAddress", DbType.String, RoomReservation.ContactAddress);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, RoomReservation.ContactPerson);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactNumber", DbType.String, RoomReservation.ContactNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@MobileNumber", DbType.String, RoomReservation.MobileNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@FaxNumber", DbType.String, RoomReservation.FaxNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactEmail", DbType.String, RoomReservation.ContactEmail);
                            dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, RoomReservation.GuestId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Reason", DbType.String, RoomReservation.Reason);
                            dbSmartAspects.AddInParameter(commandMaster, "@BusinessPromotionId", DbType.Int32, RoomReservation.BusinessPromotionId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReferenceId", DbType.Int32, RoomReservation.ReferenceId);
                            dbSmartAspects.AddInParameter(commandMaster, "@TotalRoomNumber", DbType.Int32, RoomReservation.TotalRoomNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservedMode", DbType.String, RoomReservation.ReservedMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationType", DbType.String, RoomReservation.ReservationType);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationMode", DbType.String, RoomReservation.ReservationMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsListedCompany", DbType.Boolean, RoomReservation.IsListedCompany);
                            dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonAdult", DbType.Int32, RoomReservation.NumberOfPersonAdult);
                            dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonChild", DbType.Int32, RoomReservation.NumberOfPersonChild);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsFamilyOrCouple", DbType.Boolean, RoomReservation.IsFamilyOrCouple);
                            dbSmartAspects.AddInParameter(commandMaster, "@CurrencyType", DbType.Int32, RoomReservation.CurrencyType);
                            dbSmartAspects.AddInParameter(commandMaster, "@ConversionRate", DbType.Decimal, RoomReservation.ConversionRate);
                            dbSmartAspects.AddInParameter(commandMaster, "@PaymentMode", DbType.String, RoomReservation.PaymentMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@PayFor", DbType.Int32, RoomReservation.PayFor);
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, RoomReservation.CompanyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, RoomReservation.Remarks);

                            //--Aireport Pickup Drop Information-------------------------------------------
                            dbSmartAspects.AddInParameter(commandMaster, "@AirportPickUp", DbType.String, RoomReservation.AirportPickUp);
                            dbSmartAspects.AddInParameter(commandMaster, "@AirportDrop", DbType.String, RoomReservation.AirportDrop);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsAirportPickupDropExist", DbType.Int32, RoomReservation.IsAirportPickupDropExist);

                            if (RoomReservation.IsAirportPickupDropExist == 1)
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightName", DbType.String, AireportPickupDrop.ArrivalFlightName);
                                dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightNumber", DbType.String, AireportPickupDrop.ArrivalFlightNumber);

                                if (AireportPickupDrop.ArrivalTime != null)
                                {
                                    dbSmartAspects.AddInParameter(commandMaster, "@ArrivalTime", DbType.DateTime, AireportPickupDrop.ArrivalTime);
                                }
                                else
                                {
                                    dbSmartAspects.AddInParameter(commandMaster, "@ArrivalTime", DbType.DateTime, DBNull.Value);
                                }

                                dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightName", DbType.String, AireportPickupDrop.DepartureFlightName);
                                dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightNumber", DbType.String, AireportPickupDrop.DepartureFlightNumber);
                                if (AireportPickupDrop.DepartureTime != null)
                                {
                                    dbSmartAspects.AddInParameter(commandMaster, "@DepartureTime", DbType.DateTime, AireportPickupDrop.DepartureTime);
                                }
                                else
                                {
                                    dbSmartAspects.AddInParameter(commandMaster, "@DepartureTime", DbType.DateTime, DBNull.Value);
                                }
                            }
                            else
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightName", DbType.String, DBNull.Value);
                                dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightNumber", DbType.String, DBNull.Value);
                                dbSmartAspects.AddInParameter(commandMaster, "@ArrivalTime", DbType.DateTime, DBNull.Value);
                                dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightName", DbType.String, DBNull.Value);
                                dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightNumber", DbType.String, DBNull.Value);
                                dbSmartAspects.AddInParameter(commandMaster, "@DepartureTime", DbType.DateTime, DBNull.Value);
                            }
                            //--Aireport Pickup Drop Information------------------------------End--------

                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, RoomReservation.CreatedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@TempResId", DbType.Int32, RoomReservation.ReservationTempId);
                            dbSmartAspects.AddOutParameter(commandMaster, "@ReservationId", DbType.Int32, sizeof(Int32));
                            dbSmartAspects.AddOutParameter(commandMaster, "@ReservationNumber", DbType.String, 50);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                            tmpReservationId = Convert.ToInt32(commandMaster.Parameters["@ReservationId"].Value);
                            currentReservationNumber = (commandMaster.Parameters["@ReservationNumber"].Value).ToString();
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveReservationDetailInfo_SP"))
                            {
                                foreach (ReservationDetailBO reservationDetailBO in RoomReservationDetail)
                                {
                                    commandDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int32, tmpReservationId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeId", DbType.Int32, reservationDetailBO.RoomTypeId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomId", DbType.Int32, reservationDetailBO.RoomId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomRate", DbType.Decimal, reservationDetailBO.RoomRate);
                                    dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, reservationDetailBO.UnitPrice);
                                    dbSmartAspects.AddInParameter(commandDetails, "@DiscountAmount", DbType.Decimal, reservationDetailBO.DiscountAmount);
                                    dbSmartAspects.AddInParameter(commandDetails, "@DiscountType", DbType.String, reservationDetailBO.DiscountType);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeWisePaxQuantity", DbType.String, reservationDetailBO.RoomTypeWisePaxQuantity);
                                    dbSmartAspects.AddInParameter(commandDetails, "@IsServiceChargeEnable", DbType.Boolean, reservationDetailBO.IsServiceChargeEnable);
                                    dbSmartAspects.AddInParameter(commandDetails, "@IsCityChargeEnable", DbType.Boolean, reservationDetailBO.IsCityChargeEnable);
                                    dbSmartAspects.AddInParameter(commandDetails, "@IsVatAmountEnable", DbType.Boolean, reservationDetailBO.IsVatAmountEnable);
                                    dbSmartAspects.AddInParameter(commandDetails, "@IsAdditionalChargeEnable", DbType.Boolean, reservationDetailBO.IsAdditionalChargeEnable);
                                    dbSmartAspects.AddInParameter(commandDetails, "@TotalRoomRate", DbType.Decimal, reservationDetailBO.TotalCalculatedAmount);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }

                            //if (count == detailBO.Count)
                            if (ComplementaryItem != null && status > 0)
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveReservationComplementaryItemInfo_SP"))
                                {
                                    foreach (ReservationComplementaryItemBO complementaryItemBO in ComplementaryItem)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int32, tmpReservationId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ComplementaryItemId", DbType.Int32, complementaryItemBO.ComplementaryItemId);
                                        status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            //-------------Paid Service Save, Update, Delete ----------------------------

                            if (paidServiceDeleted && status > 0)
                            {
                                using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteHotelReservationPaidService_SP"))
                                {
                                    commandDelete.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDelete, "@ReservationId", DbType.Int32, tmpReservationId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDelete);
                                }
                            }

                            if (status > 0)
                            {
                                using (DbCommand commandPaidSave = dbSmartAspects.GetStoredProcCommand("SaveReservationServiceInfo_SP"))
                                {
                                    if (PaidServiceDetails != null)
                                    {
                                        if (PaidServiceDetails.Count > 0)
                                        {
                                            foreach (RegistrationServiceInfoBO comItemBO in PaidServiceDetails)
                                            {
                                                commandPaidSave.Parameters.Clear();
                                                dbSmartAspects.AddInParameter(commandPaidSave, "@ReservationId", DbType.Int32, tmpReservationId);
                                                dbSmartAspects.AddInParameter(commandPaidSave, "@ServiceId", DbType.Int32, comItemBO.ServiceId);

                                                if (RoomReservation.IsListedCompany == true)
                                                {
                                                    dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, 0);
                                                }
                                                else
                                                {
                                                    if (RoomReservation.CurrencyType == 1)
                                                    {
                                                        dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice);
                                                    }
                                                    else
                                                    {
                                                        dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice * RoomReservation.ConversionRate);
                                                    }
                                                }

                                                dbSmartAspects.AddInParameter(commandPaidSave, "@IsAchieved", DbType.Boolean, comItemBO.IsAchieved);
                                                dbSmartAspects.AddOutParameter(commandPaidSave, "@DetailServiceId", DbType.Int32, sizeof(Int32));
                                                status = dbSmartAspects.ExecuteNonQuery(commandPaidSave);
                                            }
                                        }
                                    }
                                }
                            }
                            //-------------** End Region -- Paid Service Save, Update, Delete ----------------------------
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
        public Boolean SaveRoomReservationInfoNew(RoomReservationBO RoomReservation, List<ReservationDetailBO> RoomReservationDetail, List<ReservationDetailBO> RoomReservationSummary, List<HotelReservationAireportPickupDropBO> AireportPickupDrop, List<HotelReservationAireportPickupDropBO> AddedAirportPickupDrop, List<ReservationComplementaryItemBO> ComplementaryItem, List<RegistrationServiceInfoBO> PaidServiceDetails, bool paidServiceDeleted, out int tmpReservationId, out string currentReservationNumber, string displayRoomNumberNType, out int tmpapdId)
        {
            bool retVal = false;
            int status = 0;
            tmpReservationId = 0;
            currentReservationNumber = string.Empty;
            tmpapdId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveRoomReservationInfoNew_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@OnlineReservationId", DbType.Int32, RoomReservation.OnlineReservationId);
                            dbSmartAspects.AddInParameter(commandMaster, "@DateIn", DbType.DateTime, RoomReservation.DateIn);
                            dbSmartAspects.AddInParameter(commandMaster, "@DateOut", DbType.DateTime, RoomReservation.DateOut);

                            if (RoomReservation.ConfirmationDate != null)
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@ConfirmationDate", DbType.DateTime, RoomReservation.ConfirmationDate);
                            }
                            else
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@ConfirmationDate", DbType.DateTime, DBNull.Value);
                            }

                            dbSmartAspects.AddInParameter(commandMaster, "@ReservedCompany", DbType.String, RoomReservation.ReservedCompany);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactAddress", DbType.String, RoomReservation.ContactAddress);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, RoomReservation.ContactPerson);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactNumber", DbType.String, RoomReservation.ContactNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@MobileNumber", DbType.String, RoomReservation.MobileNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@FaxNumber", DbType.String, RoomReservation.FaxNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactEmail", DbType.String, RoomReservation.ContactEmail);
                            dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, RoomReservation.GuestId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Reason", DbType.String, RoomReservation.Reason);
                            dbSmartAspects.AddInParameter(commandMaster, "@BusinessPromotionId", DbType.Int32, RoomReservation.BusinessPromotionId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReferenceId", DbType.Int32, RoomReservation.ReferenceId);
                            dbSmartAspects.AddInParameter(commandMaster, "@MarketSegmentId", DbType.Int32, RoomReservation.MarketSegmentId);
                            dbSmartAspects.AddInParameter(commandMaster, "@GuestSourceId", DbType.Int32, RoomReservation.GuestSourceId);
                            dbSmartAspects.AddInParameter(commandMaster, "@TotalRoomNumber", DbType.Int32, RoomReservation.TotalRoomNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservedMode", DbType.String, RoomReservation.ReservedMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationType", DbType.String, RoomReservation.ReservationType);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationMode", DbType.String, RoomReservation.ReservationMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsListedCompany", DbType.Boolean, RoomReservation.IsListedCompany);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsListedContact", DbType.Boolean, RoomReservation.IsListedContact);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactId", DbType.Int64, RoomReservation.ContactId);
                            dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonAdult", DbType.Int32, RoomReservation.NumberOfPersonAdult);
                            dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonChild", DbType.Int32, RoomReservation.NumberOfPersonChild);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsFamilyOrCouple", DbType.Boolean, RoomReservation.IsFamilyOrCouple);
                            dbSmartAspects.AddInParameter(commandMaster, "@CurrencyType", DbType.Int32, RoomReservation.CurrencyType);
                            dbSmartAspects.AddInParameter(commandMaster, "@ConversionRate", DbType.Decimal, RoomReservation.ConversionRate);
                            dbSmartAspects.AddInParameter(commandMaster, "@PaymentMode", DbType.String, RoomReservation.PaymentMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@PayFor", DbType.Int32, RoomReservation.PayFor);
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, RoomReservation.CompanyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, RoomReservation.Remarks);
                            dbSmartAspects.AddInParameter(commandMaster, "@GuestRemarks", DbType.String, RoomReservation.GuestRemarks);
                            dbSmartAspects.AddInParameter(commandMaster, "@POSRemarks", DbType.String, RoomReservation.POSRemarks);
                            dbSmartAspects.AddInParameter(commandMaster, "@RoomInfo", DbType.String, displayRoomNumberNType);

                            //--Aireport Pickup Drop Information-------------------------------------------
                            dbSmartAspects.AddInParameter(commandMaster, "@AirportPickUp", DbType.String, RoomReservation.AirportPickUp);
                            dbSmartAspects.AddInParameter(commandMaster, "@AirportDrop", DbType.String, RoomReservation.AirportDrop);
                            //--Aireport Pickup Drop Information------------------------------End--------

                            dbSmartAspects.AddInParameter(commandMaster, "@IsRoomRateShowInReservationLetter", DbType.Boolean, RoomReservation.IsRoomRateShowInReservationLetter);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsRoomRateShowInPreRegistrationCard", DbType.Boolean, RoomReservation.IsRoomRateShowInPreRegistrationCard);

                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, RoomReservation.CreatedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@TempResId", DbType.Int32, RoomReservation.ReservationTempId);
                            dbSmartAspects.AddOutParameter(commandMaster, "@ReservationId", DbType.Int32, sizeof(Int32));
                            dbSmartAspects.AddOutParameter(commandMaster, "@ReservationNumber", DbType.String, 50);


                            dbSmartAspects.AddInParameter(commandMaster, "@MealPlanId", DbType.Int32, RoomReservation.MealPlanId);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsVIPGuest", DbType.Boolean, RoomReservation.IsVIPGuest);
                            dbSmartAspects.AddInParameter(commandMaster, "@VipGuestTypeId", DbType.Int32, RoomReservation.VipGuestTypeId);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsComplementaryGuest", DbType.Boolean, RoomReservation.IsComplementaryGuest);
                            dbSmartAspects.AddInParameter(commandMaster, "@ClassificationId", DbType.Int32, RoomReservation.ClassificationId);
                            dbSmartAspects.AddInParameter(commandMaster, "@BookersName", DbType.String, RoomReservation.BookersName);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                            tmpReservationId = Convert.ToInt32(commandMaster.Parameters["@ReservationId"].Value);
                            currentReservationNumber = (commandMaster.Parameters["@ReservationNumber"].Value).ToString();
                        }

                        if (status > 0)
                        {
                            if (RoomReservation.IsAirportPickupDropExist == 1)
                            {
                                if (AireportPickupDrop.Count > 0)
                                {
                                    using (DbCommand commandPickupDropAdd = dbSmartAspects.GetStoredProcCommand("SaveAirportPickupDropInfo_SP"))
                                    {
                                        foreach (HotelReservationAireportPickupDropBO pickupDropBO in AireportPickupDrop)
                                        {
                                            commandPickupDropAdd.Parameters.Clear();
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ReservationId", DbType.Int32, tmpReservationId);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@GuestId", DbType.Int32, pickupDropBO.GuestId);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalAirlineId", DbType.Int32, pickupDropBO.ArrivalAirlineId);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalFlightName", DbType.String, pickupDropBO.ArrivalFlightName);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalFlightNumber", DbType.String, pickupDropBO.ArrivalFlightNumber);

                                            if (pickupDropBO.ArrivalTime != null)
                                            {
                                                //dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalTime", DbType.DateTime, pickupDropBO.ArrivalTime);
                                                if (string.IsNullOrWhiteSpace(pickupDropBO.ArrivalFlightName.Replace("--- Please Select ---", "")) && string.IsNullOrWhiteSpace(pickupDropBO.ArrivalFlightNumber))
                                                {
                                                    dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalTime", DbType.DateTime, DBNull.Value);
                                                }
                                                else
                                                {
                                                    dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalTime", DbType.DateTime, pickupDropBO.ArrivalTime);
                                                }
                                            }
                                            else
                                            {
                                                dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalTime", DbType.DateTime, DBNull.Value);
                                            }
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureAirlineId", DbType.Int32, pickupDropBO.DepartureAirlineId);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureFlightName", DbType.String, pickupDropBO.DepartureFlightName);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureFlightNumber", DbType.String, pickupDropBO.DepartureFlightNumber);
                                            if (pickupDropBO.DepartureTime != null)
                                            {
                                                //dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureTime", DbType.DateTime, pickupDropBO.DepartureTime);
                                                if (string.IsNullOrWhiteSpace(pickupDropBO.DepartureFlightName.Replace("--- Please Select ---", "")) && string.IsNullOrWhiteSpace(pickupDropBO.DepartureFlightNumber))
                                                {
                                                    dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureTime", DbType.DateTime, DBNull.Value);
                                                }
                                                else
                                                {
                                                    dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureTime", DbType.DateTime, pickupDropBO.DepartureTime);
                                                }
                                            }
                                            else
                                            {
                                                dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureTime", DbType.DateTime, DBNull.Value);
                                            }
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@PickupDropType", DbType.String, pickupDropBO.PickupDropType);

                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@IsArrivalChargable", DbType.Boolean, pickupDropBO.IsArrivalChargable);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@IsDepartureChargable", DbType.Boolean, pickupDropBO.IsDepartureChargable);

                                            dbSmartAspects.AddOutParameter(commandPickupDropAdd, "@APDId", DbType.Int32, sizeof(Int32));
                                            status = dbSmartAspects.ExecuteNonQuery(commandPickupDropAdd, transction);
                                            tmpapdId = Convert.ToInt32(commandPickupDropAdd.Parameters["@APDId"].Value);
                                        }
                                    }
                                }
                                if (status > 0 && AddedAirportPickupDrop.Count > 0)
                                {
                                    using (DbCommand commandPickupDropAdd = dbSmartAspects.GetStoredProcCommand("SaveAirportPickupDropInfo_SP"))
                                    {
                                        foreach (HotelReservationAireportPickupDropBO pickupDropBO in AddedAirportPickupDrop)
                                        {
                                            commandPickupDropAdd.Parameters.Clear();
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ReservationId", DbType.Int32, tmpReservationId);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@GuestId", DbType.Int32, pickupDropBO.GuestId);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalAirlineId", DbType.Int32, pickupDropBO.ArrivalAirlineId);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalFlightName", DbType.String, pickupDropBO.ArrivalFlightName);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalFlightNumber", DbType.String, pickupDropBO.ArrivalFlightNumber);

                                            if (pickupDropBO.ArrivalTime != null)
                                            {
                                                //dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalTime", DbType.DateTime, pickupDropBO.ArrivalTime);
                                                if (string.IsNullOrWhiteSpace(pickupDropBO.ArrivalFlightName.Replace("--- Please Select ---", "")) && string.IsNullOrWhiteSpace(pickupDropBO.ArrivalFlightNumber))
                                                {
                                                    dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalTime", DbType.DateTime, DBNull.Value);
                                                }
                                                else
                                                {
                                                    dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalTime", DbType.DateTime, pickupDropBO.ArrivalTime);
                                                }
                                            }
                                            else
                                            {
                                                dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalTime", DbType.DateTime, DBNull.Value);
                                            }
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureAirlineId", DbType.Int32, pickupDropBO.DepartureAirlineId);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureFlightName", DbType.String, pickupDropBO.DepartureFlightName);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureFlightNumber", DbType.String, pickupDropBO.DepartureFlightNumber);
                                            if (pickupDropBO.DepartureTime != null)
                                            {
                                                //dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureTime", DbType.DateTime, pickupDropBO.DepartureTime);
                                                if (string.IsNullOrWhiteSpace(pickupDropBO.DepartureFlightName.Replace("--- Please Select ---", "")) && string.IsNullOrWhiteSpace(pickupDropBO.DepartureFlightNumber))
                                                {
                                                    dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureTime", DbType.DateTime, DBNull.Value);
                                                }
                                                else
                                                {
                                                    dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureTime", DbType.DateTime, pickupDropBO.DepartureTime);
                                                }
                                            }
                                            else
                                            {
                                                dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureTime", DbType.DateTime, DBNull.Value);
                                            }
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@PickupDropType", DbType.String, pickupDropBO.PickupDropType);

                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@IsArrivalChargable", DbType.Boolean, pickupDropBO.IsArrivalChargable);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@IsDepartureChargable", DbType.Boolean, pickupDropBO.IsDepartureChargable);

                                            dbSmartAspects.AddOutParameter(commandPickupDropAdd, "@APDId", DbType.Int32, sizeof(Int32));
                                            status = dbSmartAspects.ExecuteNonQuery(commandPickupDropAdd, transction);
                                            tmpapdId = Convert.ToInt32(commandPickupDropAdd.Parameters["@APDId"].Value);
                                        }
                                    }
                                }
                            }

                            if (ComplementaryItem != null && status > 0)
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveReservationDetailInfo_SP"))
                                {
                                    foreach (ReservationDetailBO reservationDetailBO in RoomReservationDetail)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int32, tmpReservationId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeId", DbType.Int32, reservationDetailBO.RoomTypeId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomId", DbType.Int32, reservationDetailBO.RoomId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomRate", DbType.Decimal, reservationDetailBO.RoomRate);
                                        dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, reservationDetailBO.UnitPrice);
                                        dbSmartAspects.AddInParameter(commandDetails, "@DiscountAmount", DbType.Decimal, reservationDetailBO.DiscountAmount);
                                        dbSmartAspects.AddInParameter(commandDetails, "@DiscountType", DbType.String, reservationDetailBO.DiscountType);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeWisePaxQuantity", DbType.String, reservationDetailBO.RoomTypeWisePaxQuantity);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsServiceChargeEnable", DbType.Boolean, reservationDetailBO.IsServiceChargeEnable);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsCityChargeEnable", DbType.Boolean, reservationDetailBO.IsCityChargeEnable);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsVatAmountEnable", DbType.Boolean, reservationDetailBO.IsVatAmountEnable);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsAdditionalChargeEnable", DbType.Boolean, reservationDetailBO.IsAdditionalChargeEnable);
                                        dbSmartAspects.AddInParameter(commandDetails, "@TotalRoomRate", DbType.Decimal, reservationDetailBO.TotalCalculatedAmount);

                                        status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }


                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveReservationSummaryInfo_SP"))
                                {
                                    foreach (ReservationDetailBO reservationSummaryBO in RoomReservationSummary)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int64, tmpReservationId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeId", DbType.Int32, reservationSummaryBO.RoomTypeId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomQuantity", DbType.Int32, reservationSummaryBO.RoomQuantity);
                                        dbSmartAspects.AddInParameter(commandDetails, "@PaxQuantity", DbType.Int32, reservationSummaryBO.PaxQuantity);
                                        status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            if (ComplementaryItem != null && status > 0)
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveReservationComplementaryItemInfo_SP"))
                                {
                                    foreach (ReservationComplementaryItemBO complementaryItemBO in ComplementaryItem)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int32, tmpReservationId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ComplementaryItemId", DbType.Int32, complementaryItemBO.ComplementaryItemId);
                                        status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            //-------------Paid Service Save, Update, Delete ----------------------------

                            if (paidServiceDeleted && status > 0)
                            {
                                using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteHotelReservationPaidService_SP"))
                                {
                                    commandDelete.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDelete, "@ReservationId", DbType.Int32, tmpReservationId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDelete);
                                }
                            }

                            if (status > 0)
                            {
                                using (DbCommand commandPaidSave = dbSmartAspects.GetStoredProcCommand("SaveReservationServiceInfo_SP"))
                                {
                                    if (PaidServiceDetails != null)
                                    {
                                        if (PaidServiceDetails.Count > 0)
                                        {
                                            foreach (RegistrationServiceInfoBO comItemBO in PaidServiceDetails)
                                            {
                                                commandPaidSave.Parameters.Clear();
                                                dbSmartAspects.AddInParameter(commandPaidSave, "@ReservationId", DbType.Int32, tmpReservationId);
                                                dbSmartAspects.AddInParameter(commandPaidSave, "@ServiceId", DbType.Int32, comItemBO.ServiceId);

                                                if (RoomReservation.IsListedCompany == true)
                                                {
                                                    dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, 0);
                                                }
                                                else
                                                {
                                                    if (RoomReservation.CurrencyType == 1)
                                                    {
                                                        dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice);
                                                    }
                                                    else
                                                    {
                                                        dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice * RoomReservation.ConversionRate);
                                                    }
                                                }

                                                dbSmartAspects.AddInParameter(commandPaidSave, "@IsAchieved", DbType.Boolean, comItemBO.IsAchieved);
                                                dbSmartAspects.AddOutParameter(commandPaidSave, "@DetailServiceId", DbType.Int32, sizeof(Int32));
                                                status = dbSmartAspects.ExecuteNonQuery(commandPaidSave);
                                            }
                                        }
                                    }
                                }
                            }
                            //-------------** End Region -- Paid Service Save, Update, Delete ----------------------------
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
        public Boolean UpdateRoomReservationInfo(RoomReservationBO roomReservation, List<ReservationDetailBO> reservationDetailAddedList, List<ReservationDetailBO> reservationDetailEditedList, List<ReservationDetailBO> reservationDetailDeletedList, HotelReservationAireportPickupDropBO AireportPickupDrop, List<ReservationComplementaryItemBO> newlyAddedComplementaryItem, List<ReservationComplementaryItemBO> deletedComplementaryItem, List<RegistrationServiceInfoBO> paidServiceDetails, bool paidServiceTobeDelete, out string currentReservationNumber)
        {
            bool retVal = false;
            int status = 0;
            currentReservationNumber = string.Empty;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateRoomReservationInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int32, roomReservation.ReservationId);
                            dbSmartAspects.AddInParameter(commandMaster, "@DateIn", DbType.DateTime, roomReservation.DateIn);
                            dbSmartAspects.AddInParameter(commandMaster, "@DateOut", DbType.DateTime, roomReservation.DateOut);

                            if (roomReservation.ConfirmationDate != null)
                                dbSmartAspects.AddInParameter(commandMaster, "@ConfirmationDate", DbType.DateTime, roomReservation.ConfirmationDate);
                            else
                                dbSmartAspects.AddInParameter(commandMaster, "@ConfirmationDate", DbType.DateTime, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandMaster, "@ReservedCompany", DbType.String, roomReservation.ReservedCompany);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactAddress", DbType.String, roomReservation.ContactAddress);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, roomReservation.ContactPerson);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactNumber", DbType.String, roomReservation.ContactNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@MobileNumber", DbType.String, roomReservation.MobileNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@FaxNumber", DbType.String, roomReservation.FaxNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactEmail", DbType.String, roomReservation.ContactEmail);
                            dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, roomReservation.GuestId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Reason", DbType.String, roomReservation.Reason);
                            dbSmartAspects.AddInParameter(commandMaster, "@BusinessPromotionId", DbType.Int32, roomReservation.BusinessPromotionId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReferenceId", DbType.Int32, roomReservation.ReferenceId);
                            dbSmartAspects.AddInParameter(commandMaster, "@TotalRoomNumber", DbType.Int32, roomReservation.TotalRoomNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservedMode", DbType.String, roomReservation.ReservedMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationType", DbType.String, roomReservation.ReservationType);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationMode", DbType.String, roomReservation.ReservationMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsListedCompany", DbType.Boolean, roomReservation.IsListedCompany);
                            dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonAdult", DbType.Int32, roomReservation.NumberOfPersonAdult);
                            dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonChild", DbType.Int32, roomReservation.NumberOfPersonChild);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsFamilyOrCouple", DbType.Boolean, roomReservation.IsFamilyOrCouple);
                            dbSmartAspects.AddInParameter(commandMaster, "@CurrencyType", DbType.Int32, roomReservation.CurrencyType);
                            dbSmartAspects.AddInParameter(commandMaster, "@ConversionRate", DbType.Decimal, roomReservation.ConversionRate);
                            dbSmartAspects.AddInParameter(commandMaster, "@PaymentMode", DbType.String, roomReservation.PaymentMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@PayFor", DbType.Int32, roomReservation.PayFor);
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, roomReservation.CompanyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, roomReservation.Remarks);

                            //--Aireport Pickup Drop Information-------------------------------------------
                            dbSmartAspects.AddInParameter(commandMaster, "@AirportPickUp", DbType.String, roomReservation.AirportPickUp);
                            dbSmartAspects.AddInParameter(commandMaster, "@AirportDrop", DbType.String, roomReservation.AirportDrop);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsAirportPickupDropExist", DbType.Int32, roomReservation.IsAirportPickupDropExist);

                            if (roomReservation.IsAirportPickupDropExist == 1)
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightName", DbType.String, AireportPickupDrop.ArrivalFlightName);
                                dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightNumber", DbType.String, AireportPickupDrop.ArrivalFlightNumber);
                                if (AireportPickupDrop.ArrivalTime != null)
                                    dbSmartAspects.AddInParameter(commandMaster, "@ArrivalTime", DbType.DateTime, AireportPickupDrop.ArrivalTime);
                                else
                                    dbSmartAspects.AddInParameter(commandMaster, "@ArrivalTime", DbType.DateTime, DBNull.Value);

                                dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightName", DbType.String, AireportPickupDrop.DepartureFlightName);
                                dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightNumber", DbType.String, AireportPickupDrop.DepartureFlightNumber);
                                if (AireportPickupDrop.DepartureTime != null)
                                    dbSmartAspects.AddInParameter(commandMaster, "@DepartureTime", DbType.DateTime, AireportPickupDrop.DepartureTime);
                                else
                                    dbSmartAspects.AddInParameter(commandMaster, "@DepartureTime", DbType.DateTime, DBNull.Value);
                            }
                            else
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightName", DbType.String, DBNull.Value);
                                dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightNumber", DbType.String, DBNull.Value);
                                dbSmartAspects.AddInParameter(commandMaster, "@ArrivalTime", DbType.DateTime, DBNull.Value);
                                dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightName", DbType.String, DBNull.Value);
                                dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightNumber", DbType.String, DBNull.Value);
                                dbSmartAspects.AddInParameter(commandMaster, "@DepartureTime", DbType.DateTime, DBNull.Value);
                            }

                            //--Aireport Pickup Drop Information------------------------------End--------
                            dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, roomReservation.LastModifiedBy);
                            dbSmartAspects.AddOutParameter(commandMaster, "@ReservationNumber", DbType.String, 50);
                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                            currentReservationNumber = (commandMaster.Parameters["@ReservationNumber"].Value).ToString();
                        }

                        //-- Delete Room Reservation
                        if (status > 0 && (reservationDetailDeletedList != null && reservationDetailDeletedList.Count > 0))
                        {
                            foreach (ReservationDetailBO rd in reservationDetailDeletedList)
                            {
                                using (DbCommand roomDelete = dbSmartAspects.GetStoredProcCommand("DeleteReservationDetailByDetailId_SP"))
                                {
                                    roomDelete.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(roomDelete, "@ReservationDetailId", DbType.Int32, rd.ReservationDetailId);
                                    status = dbSmartAspects.ExecuteNonQuery(roomDelete, transction);
                                }
                            }
                        }

                        //-- Edit Room Reservation
                        if (status > 0 && (reservationDetailEditedList != null && reservationDetailEditedList.Count > 0))
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReservationDetailInfo_SP"))
                            {
                                foreach (ReservationDetailBO rd in reservationDetailEditedList)
                                {
                                    commandDetails.Parameters.Clear();

                                    if (rd.UnitPrice > 0)
                                    {
                                        dbSmartAspects.AddInParameter(commandDetails, "@ReservationDetailId", DbType.Int32, rd.ReservationDetailId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int32, roomReservation.ReservationId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeId", DbType.Int32, rd.RoomTypeId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomId", DbType.Int32, rd.RoomId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomRate", DbType.Decimal, rd.RoomRate);
                                        dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, rd.UnitPrice);
                                        dbSmartAspects.AddInParameter(commandDetails, "@DiscountAmount", DbType.Decimal, rd.DiscountAmount);
                                        dbSmartAspects.AddInParameter(commandDetails, "@DiscountType", DbType.String, rd.DiscountType);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsServiceChargeEnable", DbType.Boolean, rd.IsServiceChargeEnable);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsCityChargeEnable", DbType.Boolean, rd.IsCityChargeEnable);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsVatAmountEnable", DbType.Boolean, rd.IsVatAmountEnable);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsAdditionalChargeEnable", DbType.Boolean, rd.IsAdditionalChargeEnable);
                                        dbSmartAspects.AddInParameter(commandDetails, "@TotalRoomRate", DbType.Decimal, rd.TotalCalculatedAmount);


                                        status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }
                        }

                        //-- Add New Room Reservation
                        if (status > 0 && (reservationDetailAddedList != null && reservationDetailAddedList.Count > 0))
                        {
                            foreach (ReservationDetailBO rd in reservationDetailAddedList)
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveReservationDetailInfo_SP"))
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int32, roomReservation.ReservationId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeId", DbType.Int32, rd.RoomTypeId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomId", DbType.Int32, rd.RoomId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomRate", DbType.Decimal, rd.RoomRate);
                                    dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, rd.UnitPrice);
                                    dbSmartAspects.AddInParameter(commandDetails, "@DiscountAmount", DbType.Decimal, rd.DiscountAmount);
                                    dbSmartAspects.AddInParameter(commandDetails, "@DiscountType", DbType.String, rd.DiscountType);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeWisePaxQuantity", DbType.String, rd.RoomTypeWisePaxQuantity);
                                    dbSmartAspects.AddInParameter(commandDetails, "@IsServiceChargeEnable", DbType.Boolean, rd.IsServiceChargeEnable);
                                    dbSmartAspects.AddInParameter(commandDetails, "@IsCityChargeEnable", DbType.Boolean, rd.IsCityChargeEnable);
                                    dbSmartAspects.AddInParameter(commandDetails, "@IsVatAmountEnable", DbType.Boolean, rd.IsVatAmountEnable);
                                    dbSmartAspects.AddInParameter(commandDetails, "@IsAdditionalChargeEnable", DbType.Boolean, rd.IsAdditionalChargeEnable);
                                    dbSmartAspects.AddInParameter(commandDetails, "@TotalRoomRate", DbType.Decimal, rd.TotalCalculatedAmount);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (deletedComplementaryItem.Count() > 0 && status > 0)
                        {
                            foreach (ReservationComplementaryItemBO complementaryItemBO in deletedComplementaryItem)
                            {
                                using (DbCommand commandComplementaryDeletes = dbSmartAspects.GetStoredProcCommand("DeleteReservationComplementaryItemInfo_SP"))
                                {
                                    commandComplementaryDeletes.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandComplementaryDeletes, "@ReservationId", DbType.Int32, complementaryItemBO.ReservationId);
                                    dbSmartAspects.AddInParameter(commandComplementaryDeletes, "@ComplementaryItemId", DbType.Int32, complementaryItemBO.ComplementaryItemId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandComplementaryDeletes, transction);
                                }
                            }
                        }

                        if (newlyAddedComplementaryItem.Count() > 0 && status > 0)
                        {
                            foreach (ReservationComplementaryItemBO complementaryItemBO in newlyAddedComplementaryItem)
                            {
                                using (DbCommand commandComplementaryNewAdded = dbSmartAspects.GetStoredProcCommand("SaveReservationComplementaryItemInfo_SP"))
                                {
                                    commandComplementaryNewAdded.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandComplementaryNewAdded, "@ReservationId", DbType.Int32, complementaryItemBO.ReservationId);
                                    dbSmartAspects.AddInParameter(commandComplementaryNewAdded, "@ComplementaryItemId", DbType.Int32, complementaryItemBO.ComplementaryItemId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandComplementaryNewAdded, transction);
                                }
                            }
                        }

                        //-------------Paid Service Save, Update, Delete ----------------------------
                        if (paidServiceTobeDelete && status > 0)
                        {
                            HMCommonDA hmCommonDA = new HMCommonDA();
                            Boolean delStatus = hmCommonDA.DeleteInfoById("HotelReservationServiceInfo", "ReservationId", roomReservation.ReservationId);

                            if (delStatus)
                            {
                                status = 1;
                            }
                        }

                        if (paidServiceDetails != null && status > 0)
                        {
                            using (DbCommand commandPaidSave = dbSmartAspects.GetStoredProcCommand("SaveReservationServiceInfo_SP"))
                            {
                                if (paidServiceDetails.Count > 0)
                                {
                                    foreach (RegistrationServiceInfoBO comItemBO in paidServiceDetails)
                                    {
                                        commandPaidSave.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandPaidSave, "@ReservationId", DbType.Int32, comItemBO.ReservationId);
                                        dbSmartAspects.AddInParameter(commandPaidSave, "@ServiceId", DbType.Int32, comItemBO.ServiceId);
                                        if (roomReservation.IsListedCompany == true)
                                        {
                                            dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, 0);
                                        }
                                        else
                                        {
                                            if (roomReservation.CurrencyType == 45)
                                            {
                                                dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice);
                                            }
                                            else
                                            {
                                                dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice * roomReservation.ConversionRate);
                                            }
                                        }

                                        dbSmartAspects.AddInParameter(commandPaidSave, "@IsAchieved", DbType.Boolean, comItemBO.IsAchieved);
                                        dbSmartAspects.AddOutParameter(commandPaidSave, "@DetailServiceId", DbType.Int32, sizeof(Int32));
                                        status = dbSmartAspects.ExecuteNonQuery(commandPaidSave);
                                    }
                                }
                            }
                        }
                        //-------------** End Region -- Paid Service Save, Update, Delete ----------------------------

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
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
        // ---- Ended New Save, Update
        public Boolean UpdateRoomReservationInfoNew(RoomReservationBO roomReservation, List<ReservationDetailBO> reservationDetailAddedList, List<ReservationDetailBO> reservationDetailEditedList, List<ReservationDetailBO> reservationDetailDeletedList, List<ReservationDetailBO> RoomReservationSummary, List<int> previousRoomTypeIdList, List<HotelReservationAireportPickupDropBO> AireportPickupDrop, List<HotelReservationAireportPickupDropBO> AddedAireportPickupDrop, List<HotelReservationAireportPickupDropBO> DeletedAireportPickupDrop, List<ReservationComplementaryItemBO> newlyAddedComplementaryItem, List<ReservationComplementaryItemBO> deletedComplementaryItem, List<RegistrationServiceInfoBO> paidServiceDetails, bool paidServiceTobeDelete, string displayRoomNumberNType, out string currentReservationNumber)
        {
            bool retVal = false;
            int status = 0;
            int tmpapdId = 0;
            currentReservationNumber = string.Empty;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateRoomReservationInfoNew_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int32, roomReservation.ReservationId);
                            dbSmartAspects.AddInParameter(commandMaster, "@DateIn", DbType.DateTime, roomReservation.DateIn);
                            dbSmartAspects.AddInParameter(commandMaster, "@DateOut", DbType.DateTime, roomReservation.DateOut);

                            if (roomReservation.ConfirmationDate != null)
                                dbSmartAspects.AddInParameter(commandMaster, "@ConfirmationDate", DbType.DateTime, roomReservation.ConfirmationDate);
                            else
                                dbSmartAspects.AddInParameter(commandMaster, "@ConfirmationDate", DbType.DateTime, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandMaster, "@ReservedCompany", DbType.String, roomReservation.ReservedCompany);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactAddress", DbType.String, roomReservation.ContactAddress);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, roomReservation.ContactPerson);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactNumber", DbType.String, roomReservation.ContactNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@MobileNumber", DbType.String, roomReservation.MobileNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@FaxNumber", DbType.String, roomReservation.FaxNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactEmail", DbType.String, roomReservation.ContactEmail);
                            dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, roomReservation.GuestId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Reason", DbType.String, roomReservation.Reason);
                            dbSmartAspects.AddInParameter(commandMaster, "@BusinessPromotionId", DbType.Int32, roomReservation.BusinessPromotionId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReferenceId", DbType.Int32, roomReservation.ReferenceId);
                            dbSmartAspects.AddInParameter(commandMaster, "@MarketSegmentId", DbType.Int32, roomReservation.MarketSegmentId);
                            dbSmartAspects.AddInParameter(commandMaster, "@GuestSourceId", DbType.Int32, roomReservation.GuestSourceId);
                            dbSmartAspects.AddInParameter(commandMaster, "@TotalRoomNumber", DbType.Int32, roomReservation.TotalRoomNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservedMode", DbType.String, roomReservation.ReservedMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationType", DbType.String, roomReservation.ReservationType);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationMode", DbType.String, roomReservation.ReservationMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsListedCompany", DbType.Boolean, roomReservation.IsListedCompany);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsListedContact", DbType.Boolean, roomReservation.IsListedContact);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactId", DbType.Int64, roomReservation.ContactId);
                            dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonAdult", DbType.Int32, roomReservation.NumberOfPersonAdult);
                            dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonChild", DbType.Int32, roomReservation.NumberOfPersonChild);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsFamilyOrCouple", DbType.Boolean, roomReservation.IsFamilyOrCouple);
                            dbSmartAspects.AddInParameter(commandMaster, "@CurrencyType", DbType.Int32, roomReservation.CurrencyType);
                            dbSmartAspects.AddInParameter(commandMaster, "@ConversionRate", DbType.Decimal, roomReservation.ConversionRate);
                            dbSmartAspects.AddInParameter(commandMaster, "@PaymentMode", DbType.String, roomReservation.PaymentMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@PayFor", DbType.Int32, roomReservation.PayFor);
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, roomReservation.CompanyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, roomReservation.Remarks);
                            dbSmartAspects.AddInParameter(commandMaster, "@GuestRemarks", DbType.String, roomReservation.GuestRemarks);
                            dbSmartAspects.AddInParameter(commandMaster, "@POSRemarks", DbType.String, roomReservation.POSRemarks);
                            dbSmartAspects.AddInParameter(commandMaster, "@RoomInfo", DbType.String, displayRoomNumberNType);

                            dbSmartAspects.AddInParameter(commandMaster, "@MealPlanId", DbType.Int32, roomReservation.MealPlanId);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsVIPGuest", DbType.Boolean, roomReservation.IsVIPGuest);
                            dbSmartAspects.AddInParameter(commandMaster, "@VipGuestTypeId", DbType.Int32, roomReservation.VipGuestTypeId);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsComplementaryGuest", DbType.Boolean, roomReservation.IsComplementaryGuest);
                            dbSmartAspects.AddInParameter(commandMaster, "@ClassificationId", DbType.Int32, roomReservation.ClassificationId);
                            dbSmartAspects.AddInParameter(commandMaster, "@BookersName", DbType.String, roomReservation.BookersName);

                            dbSmartAspects.AddInParameter(commandMaster, "@IsRoomRateShowInReservationLetter", DbType.Boolean, roomReservation.IsRoomRateShowInReservationLetter);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsRoomRateShowInPreRegistrationCard", DbType.Boolean, roomReservation.IsRoomRateShowInPreRegistrationCard);

                            //--Aireport Pickup Drop Information-------------------------------------------
                            dbSmartAspects.AddInParameter(commandMaster, "@AirportPickUp", DbType.String, roomReservation.AirportPickUp);
                            dbSmartAspects.AddInParameter(commandMaster, "@AirportDrop", DbType.String, roomReservation.AirportDrop);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsAirportPickupDropExist", DbType.Int32, roomReservation.IsAirportPickupDropExist);
                            //--Aireport Pickup Drop Information------------------------------End--------
                            dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, roomReservation.LastModifiedBy);
                            dbSmartAspects.AddOutParameter(commandMaster, "@ReservationNumber", DbType.String, 50);
                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            currentReservationNumber = (commandMaster.Parameters["@ReservationNumber"].Value).ToString();
                        }

                        if (status > 0 && roomReservation.IsAirportPickupDropExist == 1)
                        {
                            if (AireportPickupDrop.Count > 0)
                            {
                                foreach (HotelReservationAireportPickupDropBO pickupDropBO in AireportPickupDrop)
                                {
                                    if (pickupDropBO.APDId != 0)
                                    {
                                        using (DbCommand commandPickupDropEdit = dbSmartAspects.GetStoredProcCommand("UpdateAirportPickupDropInfo_SP"))
                                        {
                                            commandPickupDropEdit.Parameters.Clear();
                                            dbSmartAspects.AddInParameter(commandPickupDropEdit, "@APDId", DbType.Int32, pickupDropBO.APDId);
                                            dbSmartAspects.AddInParameter(commandPickupDropEdit, "@ReservationId", DbType.Int32, roomReservation.ReservationId);
                                            dbSmartAspects.AddInParameter(commandPickupDropEdit, "@GuestId", DbType.Int32, pickupDropBO.GuestId);
                                            dbSmartAspects.AddInParameter(commandPickupDropEdit, "@ArrivalAirlineId", DbType.Int32, pickupDropBO.ArrivalAirlineId);
                                            dbSmartAspects.AddInParameter(commandPickupDropEdit, "@ArrivalFlightName", DbType.String, pickupDropBO.ArrivalFlightName);
                                            dbSmartAspects.AddInParameter(commandPickupDropEdit, "@ArrivalFlightNumber", DbType.String, pickupDropBO.ArrivalFlightNumber);

                                            if (pickupDropBO.ArrivalTime != null)
                                            {
                                                //dbSmartAspects.AddInParameter(commandPickupDropEdit, "@ArrivalTime", DbType.DateTime, pickupDropBO.ArrivalTime);
                                                if (string.IsNullOrWhiteSpace(pickupDropBO.ArrivalFlightName.Replace("--- Please Select ---", "")) && string.IsNullOrWhiteSpace(pickupDropBO.ArrivalFlightNumber))
                                                {
                                                    dbSmartAspects.AddInParameter(commandPickupDropEdit, "@ArrivalTime", DbType.DateTime, DBNull.Value);
                                                }
                                                else
                                                {
                                                    dbSmartAspects.AddInParameter(commandPickupDropEdit, "@ArrivalTime", DbType.DateTime, pickupDropBO.ArrivalTime);
                                                }
                                            }
                                            else
                                            {
                                                dbSmartAspects.AddInParameter(commandPickupDropEdit, "@ArrivalTime", DbType.DateTime, DBNull.Value);
                                            }
                                            dbSmartAspects.AddInParameter(commandPickupDropEdit, "@DepartureAirlineId", DbType.Int32, pickupDropBO.DepartureAirlineId);
                                            dbSmartAspects.AddInParameter(commandPickupDropEdit, "@DepartureFlightName", DbType.String, pickupDropBO.DepartureFlightName);
                                            dbSmartAspects.AddInParameter(commandPickupDropEdit, "@DepartureFlightNumber", DbType.String, pickupDropBO.DepartureFlightNumber);
                                            if (pickupDropBO.DepartureTime != null)
                                            {
                                                //dbSmartAspects.AddInParameter(commandPickupDropEdit, "@DepartureTime", DbType.DateTime, pickupDropBO.DepartureTime);
                                                if (string.IsNullOrWhiteSpace(pickupDropBO.DepartureFlightName.Replace("--- Please Select ---", "")) && string.IsNullOrWhiteSpace(pickupDropBO.DepartureFlightNumber))
                                                {
                                                    dbSmartAspects.AddInParameter(commandPickupDropEdit, "@DepartureTime", DbType.DateTime, DBNull.Value);
                                                }
                                                else
                                                {
                                                    dbSmartAspects.AddInParameter(commandPickupDropEdit, "@DepartureTime", DbType.DateTime, pickupDropBO.DepartureTime);
                                                }
                                            }
                                            else
                                            {
                                                dbSmartAspects.AddInParameter(commandPickupDropEdit, "@DepartureTime", DbType.DateTime, DBNull.Value);
                                            }

                                            dbSmartAspects.AddInParameter(commandPickupDropEdit, "@IsArrivalChargable", DbType.Boolean, pickupDropBO.IsArrivalChargable);
                                            dbSmartAspects.AddInParameter(commandPickupDropEdit, "@IsDepartureChargable", DbType.Boolean, pickupDropBO.IsDepartureChargable);

                                            dbSmartAspects.AddInParameter(commandPickupDropEdit, "@PickupDropType", DbType.String, pickupDropBO.PickupDropType);
                                            status = dbSmartAspects.ExecuteNonQuery(commandPickupDropEdit, transction);
                                        }
                                    }
                                    else
                                    {
                                        using (DbCommand commandPickupDropAdd = dbSmartAspects.GetStoredProcCommand("SaveAirportPickupDropInfo_SP"))
                                        {
                                            commandPickupDropAdd.Parameters.Clear();
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ReservationId", DbType.Int32, roomReservation.ReservationId);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@GuestId", DbType.Int32, pickupDropBO.GuestId);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalAirlineId", DbType.Int32, pickupDropBO.ArrivalAirlineId);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalFlightName", DbType.String, pickupDropBO.ArrivalFlightName);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalFlightNumber", DbType.String, pickupDropBO.ArrivalFlightNumber);

                                            if (pickupDropBO.ArrivalTime != null)
                                            {
                                                //dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalTime", DbType.DateTime, pickupDropBO.ArrivalTime);
                                                if (string.IsNullOrWhiteSpace(pickupDropBO.ArrivalFlightName.Replace("--- Please Select ---", "")) && string.IsNullOrWhiteSpace(pickupDropBO.ArrivalFlightNumber))
                                                {
                                                    dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalTime", DbType.DateTime, DBNull.Value);
                                                }
                                                else
                                                {
                                                    dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalTime", DbType.DateTime, pickupDropBO.ArrivalTime);
                                                }
                                            }
                                            else
                                            {
                                                dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalTime", DbType.DateTime, DBNull.Value);
                                            }
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureAirlineId", DbType.Int32, pickupDropBO.DepartureAirlineId);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureFlightName", DbType.String, pickupDropBO.DepartureFlightName);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureFlightNumber", DbType.String, pickupDropBO.DepartureFlightNumber);
                                            if (pickupDropBO.DepartureTime != null)
                                            {
                                                //dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureTime", DbType.DateTime, pickupDropBO.DepartureTime);
                                                if (string.IsNullOrWhiteSpace(pickupDropBO.DepartureFlightName.Replace("--- Please Select ---", "")) && string.IsNullOrWhiteSpace(pickupDropBO.DepartureFlightNumber))
                                                {
                                                    dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureTime", DbType.DateTime, DBNull.Value);
                                                }
                                                else
                                                {
                                                    dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureTime", DbType.DateTime, pickupDropBO.DepartureTime);
                                                }
                                            }
                                            else
                                            {
                                                dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureTime", DbType.DateTime, DBNull.Value);
                                            }
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@PickupDropType", DbType.String, pickupDropBO.PickupDropType);

                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@IsArrivalChargable", DbType.Boolean, pickupDropBO.IsArrivalChargable);
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@IsDepartureChargable", DbType.Boolean, pickupDropBO.IsDepartureChargable);

                                            dbSmartAspects.AddOutParameter(commandPickupDropAdd, "@APDId", DbType.Int32, sizeof(Int32));
                                            status = dbSmartAspects.ExecuteNonQuery(commandPickupDropAdd, transction);
                                            tmpapdId = Convert.ToInt32(commandPickupDropAdd.Parameters["@APDId"].Value);
                                        }
                                    }
                                }
                            }
                            if (status > 0 && AddedAireportPickupDrop.Count > 0)
                            {
                                foreach (HotelReservationAireportPickupDropBO pickupDropBO in AddedAireportPickupDrop)
                                {
                                    using (DbCommand commandPickupDropAdd = dbSmartAspects.GetStoredProcCommand("SaveAirportPickupDropInfo_SP"))
                                    {
                                        commandPickupDropAdd.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ReservationId", DbType.Int32, roomReservation.ReservationId);
                                        dbSmartAspects.AddInParameter(commandPickupDropAdd, "@GuestId", DbType.Int32, pickupDropBO.GuestId);
                                        dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalAirlineId", DbType.Int32, pickupDropBO.ArrivalAirlineId);
                                        dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalFlightName", DbType.String, pickupDropBO.ArrivalFlightName);
                                        dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalFlightNumber", DbType.String, pickupDropBO.ArrivalFlightNumber);

                                        if (pickupDropBO.ArrivalTime != null)
                                        {
                                            //dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalTime", DbType.DateTime, pickupDropBO.ArrivalTime);
                                            if (string.IsNullOrWhiteSpace(pickupDropBO.ArrivalFlightName.Replace("--- Please Select ---", "")) && string.IsNullOrWhiteSpace(pickupDropBO.ArrivalFlightNumber))
                                            {
                                                dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalTime", DbType.DateTime, DBNull.Value);
                                            }
                                            else
                                            {
                                                dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalTime", DbType.DateTime, pickupDropBO.ArrivalTime);
                                            }
                                        }
                                        else
                                        {
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@ArrivalTime", DbType.DateTime, DBNull.Value);
                                        }
                                        dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureAirlineId", DbType.Int32, pickupDropBO.DepartureAirlineId);
                                        dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureFlightName", DbType.String, pickupDropBO.DepartureFlightName);
                                        dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureFlightNumber", DbType.String, pickupDropBO.DepartureFlightNumber);
                                        if (pickupDropBO.DepartureTime != null)
                                        {
                                            //dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureTime", DbType.DateTime, pickupDropBO.DepartureTime);
                                            if (string.IsNullOrWhiteSpace(pickupDropBO.DepartureFlightName.Replace("--- Please Select ---", "")) && string.IsNullOrWhiteSpace(pickupDropBO.DepartureFlightNumber))
                                            {
                                                dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureTime", DbType.DateTime, DBNull.Value);
                                            }
                                            else
                                            {
                                                dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureTime", DbType.DateTime, pickupDropBO.DepartureTime);
                                            }
                                        }
                                        else
                                        {
                                            dbSmartAspects.AddInParameter(commandPickupDropAdd, "@DepartureTime", DbType.DateTime, DBNull.Value);
                                        }
                                        dbSmartAspects.AddInParameter(commandPickupDropAdd, "@PickupDropType", DbType.String, pickupDropBO.PickupDropType);
                                        dbSmartAspects.AddOutParameter(commandPickupDropAdd, "@APDId", DbType.Int32, sizeof(Int32));
                                        status = dbSmartAspects.ExecuteNonQuery(commandPickupDropAdd, transction);
                                        tmpapdId = Convert.ToInt32(commandPickupDropAdd.Parameters["@APDId"].Value);
                                    }
                                }
                            }
                        }
                        else
                        {
                            using (DbCommand commandPickupDropDel = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                commandPickupDropDel.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandPickupDropDel, "@TableName", DbType.String, "HotelReservationAireportPickupDrop");
                                dbSmartAspects.AddInParameter(commandPickupDropDel, "@TablePKField", DbType.String, "ReservationId");
                                dbSmartAspects.AddInParameter(commandPickupDropDel, "@TablePKId", DbType.String, roomReservation.ReservationId);
                                dbSmartAspects.ExecuteNonQuery(commandPickupDropDel, transction);
                                status = 1;
                            }
                        }

                        //-- Delete Room Reservation
                        if (status > 0 && (reservationDetailDeletedList != null && reservationDetailDeletedList.Count > 0))
                        {
                            foreach (ReservationDetailBO rd in reservationDetailDeletedList)
                            {
                                using (DbCommand roomDelete = dbSmartAspects.GetStoredProcCommand("DeleteReservationDetailByDetailId_SP"))
                                {
                                    roomDelete.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(roomDelete, "@ReservationDetailId", DbType.Int32, rd.ReservationDetailId);
                                    status = dbSmartAspects.ExecuteNonQuery(roomDelete, transction);
                                }
                            }
                        }

                        //-- Edit Room Reservation
                        if (status > 0 && (reservationDetailEditedList != null && reservationDetailEditedList.Count > 0))
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReservationDetailInfo_SP"))
                            {
                                foreach (ReservationDetailBO rd in reservationDetailEditedList)
                                {
                                    commandDetails.Parameters.Clear();

                                    if (rd.UnitPrice > 0)
                                    {
                                        dbSmartAspects.AddInParameter(commandDetails, "@ReservationDetailId", DbType.Int32, rd.ReservationDetailId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int32, roomReservation.ReservationId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeId", DbType.Int32, rd.RoomTypeId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomId", DbType.Int32, rd.RoomId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomRate", DbType.Decimal, rd.RoomRate);
                                        dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, rd.UnitPrice);
                                        dbSmartAspects.AddInParameter(commandDetails, "@DiscountAmount", DbType.Decimal, rd.DiscountAmount);
                                        dbSmartAspects.AddInParameter(commandDetails, "@DiscountType", DbType.String, rd.DiscountType);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeWisePaxQuantity", DbType.Int32, rd.RoomTypeWisePaxQuantity);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsServiceChargeEnable", DbType.Boolean, rd.IsServiceChargeEnable);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsCityChargeEnable", DbType.Boolean, rd.IsCityChargeEnable);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsVatAmountEnable", DbType.Boolean, rd.IsVatAmountEnable);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsAdditionalChargeEnable", DbType.Boolean, rd.IsAdditionalChargeEnable);
                                        dbSmartAspects.AddInParameter(commandDetails, "@TotalRoomRate", DbType.Decimal, rd.TotalCalculatedAmount);

                                        status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }
                        }

                        //-- Add New Room Reservation
                        if (status > 0 && (reservationDetailAddedList != null && reservationDetailAddedList.Count > 0))
                        {
                            foreach (ReservationDetailBO rd in reservationDetailAddedList)
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveReservationDetailInfo_SP"))
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int32, roomReservation.ReservationId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeId", DbType.Int32, rd.RoomTypeId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomId", DbType.Int32, rd.RoomId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomRate", DbType.Decimal, rd.RoomRate);
                                    dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, rd.UnitPrice);
                                    dbSmartAspects.AddInParameter(commandDetails, "@DiscountAmount", DbType.Decimal, rd.DiscountAmount);
                                    dbSmartAspects.AddInParameter(commandDetails, "@DiscountType", DbType.String, rd.DiscountType);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeWisePaxQuantity", DbType.Int32, rd.RoomTypeWisePaxQuantity);
                                    dbSmartAspects.AddInParameter(commandDetails, "@IsServiceChargeEnable", DbType.Boolean, rd.IsServiceChargeEnable);
                                    dbSmartAspects.AddInParameter(commandDetails, "@IsCityChargeEnable", DbType.Boolean, rd.IsCityChargeEnable);
                                    dbSmartAspects.AddInParameter(commandDetails, "@IsVatAmountEnable", DbType.Boolean, rd.IsVatAmountEnable);
                                    dbSmartAspects.AddInParameter(commandDetails, "@IsAdditionalChargeEnable", DbType.Boolean, rd.IsAdditionalChargeEnable);
                                    dbSmartAspects.AddInParameter(commandDetails, "@TotalRoomRate", DbType.Decimal, rd.TotalCalculatedAmount);


                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && (RoomReservationSummary != null && RoomReservationSummary.Count > 0))
                        {
                            List<int> currentRoomTypeIdList = RoomReservationSummary.Select(i => i.RoomTypeId).ToList();
                            List<int> deleteRoomTypeIdlist = previousRoomTypeIdList.Where(i => !currentRoomTypeIdList.Contains(i)).ToList();

                            foreach (var item in deleteRoomTypeIdlist)
                            {
                                using (DbCommand commandSummary = dbSmartAspects.GetStoredProcCommand("DeleteReservationSummaryInfo_SP"))
                                {
                                    commandSummary.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandSummary, "@ReservationId", DbType.Int64, roomReservation.ReservationId);
                                    dbSmartAspects.AddInParameter(commandSummary, "@RoomTypeId", DbType.Int64, item);
                                    status = dbSmartAspects.ExecuteNonQuery(commandSummary, transction);
                                }
                            }


                            foreach (var item in RoomReservationSummary)
                            {

                                if (!previousRoomTypeIdList.Contains(item.RoomTypeId))
                                {
                                    using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveReservationSummaryInfo_SP"))
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int64, item.ReservationId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeId", DbType.Int32, item.RoomTypeId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomQuantity", DbType.Int32, item.RoomQuantity);
                                        dbSmartAspects.AddInParameter(commandDetails, "@PaxQuantity", DbType.Int32, item.PaxQuantity);
                                        status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                                else
                                {
                                    using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReservationSummaryInfo_SP"))
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int64, item.ReservationId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeId", DbType.Int32, item.RoomTypeId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomQuantity", DbType.Int32, item.RoomQuantity);
                                        dbSmartAspects.AddInParameter(commandDetails, "@PaxQuantity", DbType.Int32, item.PaxQuantity);
                                        status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }
                        }

                        if (deletedComplementaryItem.Count() > 0 && status > 0)
                        {
                            foreach (ReservationComplementaryItemBO complementaryItemBO in deletedComplementaryItem)
                            {
                                using (DbCommand commandComplementaryDeletes = dbSmartAspects.GetStoredProcCommand("DeleteReservationComplementaryItemInfo_SP"))
                                {
                                    commandComplementaryDeletes.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandComplementaryDeletes, "@ReservationId", DbType.Int32, complementaryItemBO.ReservationId);
                                    dbSmartAspects.AddInParameter(commandComplementaryDeletes, "@ComplementaryItemId", DbType.Int32, complementaryItemBO.ComplementaryItemId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandComplementaryDeletes, transction);
                                }
                            }
                        }

                        if (newlyAddedComplementaryItem.Count() > 0 && status > 0)
                        {
                            foreach (ReservationComplementaryItemBO complementaryItemBO in newlyAddedComplementaryItem)
                            {
                                using (DbCommand commandComplementaryNewAdded = dbSmartAspects.GetStoredProcCommand("SaveReservationComplementaryItemInfo_SP"))
                                {
                                    commandComplementaryNewAdded.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandComplementaryNewAdded, "@ReservationId", DbType.Int32, complementaryItemBO.ReservationId);
                                    dbSmartAspects.AddInParameter(commandComplementaryNewAdded, "@ComplementaryItemId", DbType.Int32, complementaryItemBO.ComplementaryItemId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandComplementaryNewAdded, transction);
                                }
                            }
                        }

                        //-------------Paid Service Save, Update, Delete ----------------------------
                        if (paidServiceTobeDelete && status > 0)
                        {
                            HMCommonDA hmCommonDA = new HMCommonDA();
                            Boolean delStatus = hmCommonDA.DeleteInfoById("HotelReservationServiceInfo", "ReservationId", roomReservation.ReservationId);

                            if (delStatus)
                            {
                                status = 1;
                            }
                        }

                        if (paidServiceDetails != null && status > 0)
                        {
                            using (DbCommand commandPaidSave = dbSmartAspects.GetStoredProcCommand("SaveReservationServiceInfo_SP"))
                            {
                                if (paidServiceDetails.Count > 0)
                                {
                                    foreach (RegistrationServiceInfoBO comItemBO in paidServiceDetails)
                                    {
                                        commandPaidSave.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandPaidSave, "@ReservationId", DbType.Int32, comItemBO.ReservationId);
                                        dbSmartAspects.AddInParameter(commandPaidSave, "@ServiceId", DbType.Int32, comItemBO.ServiceId);
                                        if (roomReservation.IsListedCompany == true)
                                        {
                                            dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, 0);
                                        }
                                        else
                                        {
                                            if (roomReservation.CurrencyType == 45)
                                            {
                                                dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice);
                                            }
                                            else
                                            {
                                                dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice * roomReservation.ConversionRate);
                                            }
                                        }

                                        dbSmartAspects.AddInParameter(commandPaidSave, "@IsAchieved", DbType.Boolean, comItemBO.IsAchieved);
                                        dbSmartAspects.AddOutParameter(commandPaidSave, "@DetailServiceId", DbType.Int32, sizeof(Int32));
                                        status = dbSmartAspects.ExecuteNonQuery(commandPaidSave);
                                    }
                                }
                            }
                        }
                        //-------------** End Region -- Paid Service Save, Update, Delete ----------------------------

                        using (DbCommand commandGuestReservationRoomInfo = dbSmartAspects.GetStoredProcCommand("UpdateHotelGuestReservationRoomInfo"))
                        {
                            dbSmartAspects.AddInParameter(commandGuestReservationRoomInfo, "@ReservationId", DbType.Int32, roomReservation.ReservationId);
                            //status = dbSmartAspects.ExecuteNonQuery(commandGuestReservationRoomInfo, transction);
                            dbSmartAspects.ExecuteNonQuery(commandGuestReservationRoomInfo, transction);
                        }


                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
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
        public Boolean UpdateRoomReservationInfo(RoomReservationBO roomReservation, List<ReservationDetailBO> detailUpdateBO, List<ReservationDetailBO> detailBO, List<ReservationComplementaryItemBO> newlyAddedComplementaryItem, List<ReservationComplementaryItemBO> deletedComplementaryItem, List<ReservationDetailBO> AnassignedDetailRoomDelete, List<ReservationDetailBO> UnassignedDetailRoomDelete, List<ReservationDetailBO> deletedReservationDetailListByRoomType, out string currentReservationNumber, List<RegistrationServiceInfoBO> paidServiceDetails, bool paidServiceTobeDelete)
        {
            bool retVal = false;
            int status = 0;
            int tmpReservationId = 0;
            currentReservationNumber = string.Empty;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateRoomReservationInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int32, roomReservation.ReservationId);
                            dbSmartAspects.AddInParameter(commandMaster, "@DateIn", DbType.DateTime, roomReservation.DateIn);
                            dbSmartAspects.AddInParameter(commandMaster, "@DateOut", DbType.DateTime, roomReservation.DateOut);
                            dbSmartAspects.AddInParameter(commandMaster, "@ConfirmationDate", DbType.DateTime, roomReservation.ConfirmationDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservedCompany", DbType.String, roomReservation.ReservedCompany);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactAddress", DbType.String, roomReservation.ContactAddress);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, roomReservation.ContactPerson);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactNumber", DbType.String, roomReservation.ContactNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@MobileNumber", DbType.String, roomReservation.MobileNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@FaxNumber", DbType.String, roomReservation.FaxNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactEmail", DbType.String, roomReservation.ContactEmail);
                            dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, roomReservation.GuestId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Reason", DbType.String, roomReservation.Reason);
                            dbSmartAspects.AddInParameter(commandMaster, "@BusinessPromotionId", DbType.Int32, roomReservation.BusinessPromotionId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReferenceId", DbType.Int32, roomReservation.ReferenceId);
                            dbSmartAspects.AddInParameter(commandMaster, "@TotalRoomNumber", DbType.Int32, roomReservation.TotalRoomNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservedMode", DbType.String, roomReservation.ReservedMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationType", DbType.String, roomReservation.ReservationType);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationMode", DbType.String, roomReservation.ReservationMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsListedCompany", DbType.Boolean, roomReservation.IsListedCompany);
                            dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonAdult", DbType.Int32, roomReservation.NumberOfPersonAdult);
                            dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonChild", DbType.Int32, roomReservation.NumberOfPersonChild);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsFamilyOrCouple", DbType.Boolean, roomReservation.IsFamilyOrCouple);
                            dbSmartAspects.AddInParameter(commandMaster, "@CurrencyType", DbType.Int32, roomReservation.CurrencyType);
                            dbSmartAspects.AddInParameter(commandMaster, "@ConversionRate", DbType.Decimal, roomReservation.ConversionRate);
                            dbSmartAspects.AddInParameter(commandMaster, "@PaymentMode", DbType.String, roomReservation.PaymentMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@PayFor", DbType.Int32, roomReservation.PayFor);
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, roomReservation.CompanyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, roomReservation.Remarks);
                            //--Aireport Pickup Drop Information-------------------------------------------
                            dbSmartAspects.AddInParameter(commandMaster, "@AirportPickUp", DbType.String, roomReservation.AirportPickUp);
                            dbSmartAspects.AddInParameter(commandMaster, "@AirportDrop", DbType.String, roomReservation.AirportDrop);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsAirportPickupDropExist", DbType.Int32, roomReservation.IsAirportPickupDropExist);
                            dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightName", DbType.String, roomReservation.ArrivalFlightName);
                            dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightNumber", DbType.String, roomReservation.ArrivalFlightNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ArrivalTime", DbType.DateTime, roomReservation.ArrivalTime);
                            dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightName", DbType.String, roomReservation.DepartureFlightName);
                            dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightNumber", DbType.String, roomReservation.DepartureFlightNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@DepartureTime", DbType.DateTime, roomReservation.DepartureTime);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsRoomRateShowInPreRegistrationCard", DbType.Boolean, roomReservation.IsRoomRateShowInPreRegistrationCard);
                            //--Aireport Pickup Drop Information------------------------------End--------
                            dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, roomReservation.LastModifiedBy);
                            dbSmartAspects.AddOutParameter(commandMaster, "@ReservationNumber", DbType.String, 50);
                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            tmpReservationId = roomReservation.ReservationId;
                            currentReservationNumber = (commandMaster.Parameters["@ReservationNumber"].Value).ToString();
                        }

                        if (status > 0)
                        {
                            //Room Type Wise Room Delete Information------------------------------------
                            if (deletedReservationDetailListByRoomType != null)
                            {
                                string roomType = string.Empty;

                                foreach (ReservationDetailBO reservationDetailBO in deletedReservationDetailListByRoomType)
                                {
                                    if (roomType != reservationDetailBO.RoomType)
                                    {
                                        using (DbCommand roomTypeWiseRoomDelete = dbSmartAspects.GetStoredProcCommand("DeleteReservationDetailByRoomTypeNReservationId_SP"))
                                        {
                                            roomTypeWiseRoomDelete.Parameters.Clear();
                                            dbSmartAspects.AddInParameter(roomTypeWiseRoomDelete, "@ReservationId", DbType.Int32, reservationDetailBO.ReservationId);
                                            dbSmartAspects.AddInParameter(roomTypeWiseRoomDelete, "@RoomTypeId", DbType.Int32, reservationDetailBO.RoomTypeId);
                                            status = dbSmartAspects.ExecuteNonQuery(roomTypeWiseRoomDelete, transction);
                                        }

                                        roomType = reservationDetailBO.RoomType;
                                    }
                                }
                            }

                            //Assigned Room Delete Information------------------------------------
                            if (AnassignedDetailRoomDelete != null && status > 0 && deletedReservationDetailListByRoomType == null)
                            {
                                foreach (ReservationDetailBO reservationDetailBO in AnassignedDetailRoomDelete)
                                {
                                    using (DbCommand unAssignedRoomDelete = dbSmartAspects.GetStoredProcCommand("DeleteReservationDetailByDetailId_SP"))
                                    {
                                        unAssignedRoomDelete.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(unAssignedRoomDelete, "@ReservationDetailId", DbType.Int32, reservationDetailBO.ReservationDetailId);
                                        status = dbSmartAspects.ExecuteNonQuery(unAssignedRoomDelete, transction);
                                    }
                                }
                            }

                            //Unassigned Room Delete Information------------------------------------
                            if (UnassignedDetailRoomDelete != null && status > 0)
                            {
                                foreach (ReservationDetailBO reservationDetailBO in UnassignedDetailRoomDelete)
                                {
                                    using (DbCommand unAssignedRoomDelete = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                    {
                                        dbSmartAspects.AddInParameter(unAssignedRoomDelete, "@TableName", DbType.String, "HotelRoomReservationDetail");
                                        dbSmartAspects.AddInParameter(unAssignedRoomDelete, "@TablePKField", DbType.String, "ReservationDetailId");
                                        dbSmartAspects.AddInParameter(unAssignedRoomDelete, "@TablePKId", DbType.String, reservationDetailBO.ReservationDetailId);
                                        status = dbSmartAspects.ExecuteNonQuery(unAssignedRoomDelete, transction);
                                    }
                                }
                            }

                            //--If Is Update Data ------------------------------
                            if (detailUpdateBO != null && status > 0)
                            {
                                foreach (ReservationDetailBO reservationDetailBO in detailUpdateBO)
                                {
                                    if (reservationDetailBO.UnitPrice > 0)
                                    {
                                        using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReservationDetailInfo_SP"))
                                        {
                                            commandDetails.Parameters.Clear();
                                            dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int32, tmpReservationId);
                                            dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeId", DbType.Int32, reservationDetailBO.RoomTypeId);
                                            dbSmartAspects.AddInParameter(commandDetails, "@RoomId", DbType.Int32, reservationDetailBO.RoomId);
                                            dbSmartAspects.AddInParameter(commandDetails, "@RoomRate", DbType.Decimal, reservationDetailBO.RoomRate);
                                            dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, reservationDetailBO.UnitPrice);
                                            dbSmartAspects.AddInParameter(commandDetails, "@DiscountAmount", DbType.Decimal, reservationDetailBO.DiscountAmount);
                                            dbSmartAspects.AddInParameter(commandDetails, "@DiscountType", DbType.String, reservationDetailBO.DiscountType);
                                            dbSmartAspects.AddInParameter(commandDetails, "@IsServiceChargeEnable", DbType.Boolean, reservationDetailBO.IsServiceChargeEnable);
                                            dbSmartAspects.AddInParameter(commandDetails, "@IsCityChargeEnable", DbType.Boolean, reservationDetailBO.IsCityChargeEnable);
                                            dbSmartAspects.AddInParameter(commandDetails, "@IsVatAmountEnable", DbType.Boolean, reservationDetailBO.IsVatAmountEnable);
                                            dbSmartAspects.AddInParameter(commandDetails, "@IsAdditionalChargeEnable", DbType.Boolean, reservationDetailBO.IsAdditionalChargeEnable);
                                            dbSmartAspects.AddInParameter(commandDetails, "@TotalRoomRate", DbType.Decimal, reservationDetailBO.TotalCalculatedAmount);
                                            status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                        }
                                    }
                                }
                            }

                            if (detailBO != null && status > 0)
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveReservationDetailInfo_SP"))
                                {
                                    foreach (ReservationDetailBO reservationDetailBO in detailBO)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int32, tmpReservationId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeId", DbType.Int32, reservationDetailBO.RoomTypeId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomId", DbType.Int32, reservationDetailBO.RoomId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomRate", DbType.Decimal, reservationDetailBO.RoomRate);
                                        dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, reservationDetailBO.UnitPrice);
                                        dbSmartAspects.AddInParameter(commandDetails, "@DiscountAmount", DbType.Decimal, reservationDetailBO.DiscountAmount);
                                        dbSmartAspects.AddInParameter(commandDetails, "@DiscountType", DbType.String, reservationDetailBO.DiscountType);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeWisePaxQuantity", DbType.String, reservationDetailBO.RoomTypeWisePaxQuantity);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsServiceChargeEnable", DbType.Boolean, reservationDetailBO.IsServiceChargeEnable);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsCityChargeEnable", DbType.Boolean, reservationDetailBO.IsCityChargeEnable);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsVatAmountEnable", DbType.Boolean, reservationDetailBO.IsVatAmountEnable);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsAdditionalChargeEnable", DbType.Boolean, reservationDetailBO.IsAdditionalChargeEnable);
                                        dbSmartAspects.AddInParameter(commandDetails, "@TotalRoomRate", DbType.Decimal, reservationDetailBO.TotalCalculatedAmount);
                                        status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            if (deletedComplementaryItem.Count() > 0 && status > 0)
                            {
                                using (DbCommand commandComplementaryDeletes = dbSmartAspects.GetStoredProcCommand("DeleteReservationComplementaryItemInfo_SP"))
                                {
                                    foreach (ReservationComplementaryItemBO complementaryItemBO in deletedComplementaryItem)
                                    {
                                        commandComplementaryDeletes.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandComplementaryDeletes, "@ReservationId", DbType.Int32, complementaryItemBO.ReservationId);
                                        dbSmartAspects.AddInParameter(commandComplementaryDeletes, "@ComplementaryItemId", DbType.Int32, complementaryItemBO.ComplementaryItemId);
                                        status = dbSmartAspects.ExecuteNonQuery(commandComplementaryDeletes, transction);
                                    }
                                }
                            }

                            if (newlyAddedComplementaryItem.Count() > 0 && status > 0)
                            {
                                using (DbCommand commandComplementaryNewAdded = dbSmartAspects.GetStoredProcCommand("SaveReservationComplementaryItemInfo_SP"))
                                {
                                    foreach (ReservationComplementaryItemBO complementaryItemBO in newlyAddedComplementaryItem)
                                    {
                                        commandComplementaryNewAdded.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandComplementaryNewAdded, "@ReservationId", DbType.Int32, complementaryItemBO.ReservationId);
                                        dbSmartAspects.AddInParameter(commandComplementaryNewAdded, "@ComplementaryItemId", DbType.Int32, complementaryItemBO.ComplementaryItemId);
                                        status = dbSmartAspects.ExecuteNonQuery(commandComplementaryNewAdded, transction);
                                    }
                                }
                            }

                            //-------------Paid Service Save, Update, Delete ----------------------------
                            if (paidServiceTobeDelete && status > 0)
                            {
                                HMCommonDA hmCommonDA = new HMCommonDA();
                                Boolean delStatus = hmCommonDA.DeleteInfoById("HotelReservationServiceInfo", "ReservationId", roomReservation.ReservationId);

                                if (delStatus)
                                {
                                    status = 1;
                                }
                            }

                            if (paidServiceDetails != null && status > 0)
                            {
                                using (DbCommand commandPaidSave = dbSmartAspects.GetStoredProcCommand("SaveReservationServiceInfo_SP"))
                                {
                                    if (paidServiceDetails.Count > 0)
                                    {
                                        foreach (RegistrationServiceInfoBO comItemBO in paidServiceDetails)
                                        {
                                            commandPaidSave.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandPaidSave, "@ReservationId", DbType.Int32, comItemBO.ReservationId);
                                            dbSmartAspects.AddInParameter(commandPaidSave, "@ServiceId", DbType.Int32, comItemBO.ServiceId);
                                            if (roomReservation.IsListedCompany == true)
                                            {
                                                dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, 0);
                                            }
                                            else
                                            {
                                                if (roomReservation.CurrencyType == 45)
                                                {
                                                    dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice);
                                                }
                                                else
                                                {
                                                    dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice * roomReservation.ConversionRate);
                                                }
                                            }

                                            dbSmartAspects.AddInParameter(commandPaidSave, "@IsAchieved", DbType.Boolean, comItemBO.IsAchieved);
                                            dbSmartAspects.AddOutParameter(commandPaidSave, "@DetailServiceId", DbType.Int32, sizeof(Int32));
                                            status = dbSmartAspects.ExecuteNonQuery(commandPaidSave);
                                        }
                                    }
                                }
                            }
                            //-------------** End Region -- Paid Service Save, Update, Delete ----------------------------
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
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
        public RoomReservationBO GetRoomReservationSummaryInfoById(Int64 reservationId)
        {
            RoomReservationBO roomReservation = new RoomReservationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationSummaryInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int64, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"].ToString());
                                roomReservation.RoomQuantity = Convert.ToInt32(reader["RoomQuantity"].ToString());
                                roomReservation.PaxQuantity = Convert.ToInt32(reader["PaxQuantity"].ToString());
                            }
                        }
                    }
                }
            }
            return roomReservation;
        }
        public RoomReservationBO GetRoomReservationInfoById(int reservationId)
        {
            RoomReservationBO roomReservation = new RoomReservationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"]);
                                roomReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                roomReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);

                                if (!string.IsNullOrWhiteSpace(reader["ConfirmationDate"].ToString()))
                                {
                                    roomReservation.ConfirmationDate = Convert.ToDateTime(reader["ConfirmationDate"]);
                                }
                                else
                                {
                                    roomReservation.ConfirmationDate = null;
                                }

                                roomReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                                roomReservation.GuestId = Int32.Parse(reader["GuestId"].ToString());
                                roomReservation.ContactAddress = reader["ContactAddress"].ToString();
                                roomReservation.ContactPerson = reader["ContactPerson"].ToString();
                                roomReservation.ContactNumber = reader["ContactNumber"].ToString();
                                roomReservation.MobileNumber = reader["MobileNumber"].ToString();
                                roomReservation.FaxNumber = reader["FaxNumber"].ToString();
                                roomReservation.ContactEmail = reader["ContactEmail"].ToString();
                                roomReservation.TotalRoomNumber = Convert.ToInt32(reader["TotalRoomNumber"]);
                                roomReservation.ReservedMode = reader["ReservedMode"].ToString();
                                roomReservation.ReservationType = reader["ReservationType"].ToString();
                                roomReservation.ReservationMode = reader["ReservationMode"].ToString();
                                roomReservation.IsListedCompany = Convert.ToBoolean(reader["IsListedCompany"]);
                                roomReservation.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                roomReservation.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                roomReservation.IsFamilyOrCouple = Convert.ToBoolean(reader["IsFamilyOrCouple"]);
                                roomReservation.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                roomReservation.CompanyName = reader["CompanyName"].ToString();
                                roomReservation.BusinessPromotionId = Int32.Parse(reader["BusinessPromotionId"].ToString());
                                roomReservation.Reason = reader["Reason"].ToString();
                                roomReservation.ReferenceId = Convert.ToInt32(reader["ReferenceId"]);
                                roomReservation.PaymentMode = reader["PaymentMode"].ToString();
                                roomReservation.PayFor = Convert.ToInt32(reader["PayFor"].ToString());
                                roomReservation.CurrencyType = Convert.ToInt32(reader["CurrencyType"].ToString());
                                roomReservation.ConversionRate = Convert.ToDecimal(reader["ConversionRate"].ToString());
                                roomReservation.Remarks = reader["Remarks"].ToString();
                                roomReservation.AirportPickUp = reader["AirportPickUp"].ToString();
                                roomReservation.AirportDrop = reader["AirportDrop"].ToString();
                                roomReservation.ArrivalFlightName = reader["ArrivalFlightName"].ToString();
                                roomReservation.ArrivalFlightNumber = reader["ArrivalFlightNumber"].ToString();

                                if (!string.IsNullOrWhiteSpace(reader["ArrivalTime"].ToString()))
                                {
                                    roomReservation.ArrivalTime = Convert.ToDateTime(reader["ArrivalTime"].ToString());
                                }

                                roomReservation.DepartureFlightName = reader["DepartureFlightName"].ToString();
                                roomReservation.DepartureFlightNumber = reader["DepartureFlightNumber"].ToString();

                                if (!string.IsNullOrWhiteSpace(reader["DepartureTime"].ToString()))
                                {
                                    roomReservation.DepartureTime = Convert.ToDateTime(reader["DepartureTime"].ToString());
                                    roomReservation.DepartureTimeString = Convert.ToDateTime(reader["DepartureTime"].ToString()).ToString("HH:mm");
                                }

                                roomReservation.DiscountType = reader["DiscountType"].ToString();
                                roomReservation.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"].ToString());

                                roomReservation.IsServiceChargeEnable = Convert.ToBoolean(reader["IsServiceChargeEnable"]);
                                roomReservation.IsCityChargeEnable = Convert.ToBoolean(reader["IsCityChargeEnable"]);
                                roomReservation.IsVatAmountEnable = Convert.ToBoolean(reader["IsVatAmountEnable"]);
                                roomReservation.IsAdditionalChargeEnable = Convert.ToBoolean(reader["IsAdditionalChargeEnable"]);

                                roomReservation.MealPlanId = Convert.ToInt32(reader["MealPlanId"]);
                                roomReservation.ClassificationId = Convert.ToInt32(reader["ClassificationId"]);

                                roomReservation.IsVIPGuest = Convert.ToBoolean(reader["IsVIPGuest"]);
                                roomReservation.VipGuestTypeId = Convert.ToInt32(reader["VipGuestTypeId"].ToString());
                                roomReservation.IsComplementaryGuest = Convert.ToBoolean(reader["IsComplementaryGuest"]);
                            }
                        }
                    }
                }
            }
            return roomReservation;
        }
        public RoomReservationBO GetRoomReservationIsVatAmountEnableInfoById(int reservationId)
        {
            RoomReservationBO roomReservation = new RoomReservationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationIsVatAmountEnableInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomReservation.IsVatAmountEnable = Convert.ToBoolean(reader["IsVatAmountEnable"]);
                            }
                        }
                    }
                }
            }
            return roomReservation;
        }
        public RoomReservationBO GetRoomReservationInfoByReservationNumber(string reservationNumber)
        {
            RoomReservationBO roomReservation = new RoomReservationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfoByReservationNumber_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, reservationNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationNumber = reader["ReservationNumber"].ToString();

                                roomReservation.GuestName = reader["GuestName"].ToString();
                                roomReservation.GuestEmail = reader["GuestEmail"].ToString();
                                roomReservation.GuestPhone = reader["GuestPhone"].ToString();

                                roomReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"]);
                                roomReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                roomReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);
                                roomReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                            }
                        }
                    }
                }
            }
            return roomReservation;
        }
        public RoomReservationBO GetRoomReservationInfoWithAirportDropById(int reservationId)
        {
            RoomReservationBO roomReservation = new RoomReservationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfoWithAirportDropById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"]);
                                roomReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                roomReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);

                                if (!string.IsNullOrWhiteSpace(reader["ConfirmationDate"].ToString()))
                                {
                                    roomReservation.ConfirmationDate = Convert.ToDateTime(reader["ConfirmationDate"]);
                                }
                                else
                                {
                                    roomReservation.ConfirmationDate = null;
                                }

                                roomReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                                roomReservation.GuestId = Int32.Parse(reader["GuestId"].ToString());
                                roomReservation.ContactAddress = reader["ContactAddress"].ToString();
                                roomReservation.ContactPerson = reader["ContactPerson"].ToString();
                                roomReservation.ContactNumber = reader["ContactNumber"].ToString();
                                roomReservation.MobileNumber = reader["MobileNumber"].ToString();
                                roomReservation.FaxNumber = reader["FaxNumber"].ToString();
                                roomReservation.ContactEmail = reader["ContactEmail"].ToString();
                                roomReservation.TotalRoomNumber = Convert.ToInt32(reader["TotalRoomNumber"]);
                                roomReservation.ReservedMode = reader["ReservedMode"].ToString();
                                roomReservation.ReservationType = reader["ReservationType"].ToString();
                                roomReservation.ReservationMode = reader["ReservationMode"].ToString();
                                roomReservation.IsListedCompany = Convert.ToBoolean(reader["IsListedCompany"]);
                                roomReservation.IsListedContact = Convert.ToBoolean(reader["IsListedContact"]);
                                roomReservation.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                roomReservation.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                roomReservation.IsFamilyOrCouple = Convert.ToBoolean(reader["IsFamilyOrCouple"]);
                                roomReservation.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                roomReservation.ContactId = Convert.ToInt64(reader["ContactId"]);
                                roomReservation.CompanyName = reader["CompanyName"].ToString();
                                roomReservation.BusinessPromotionId = Int32.Parse(reader["BusinessPromotionId"].ToString());
                                roomReservation.Reason = reader["Reason"].ToString();
                                roomReservation.ReferenceId = Convert.ToInt32(reader["ReferenceId"]);
                                roomReservation.PaymentMode = reader["PaymentMode"].ToString();
                                roomReservation.PayFor = Convert.ToInt32(reader["PayFor"].ToString());
                                roomReservation.CurrencyType = Convert.ToInt32(reader["CurrencyType"].ToString());
                                roomReservation.ConversionRate = Convert.ToDecimal(reader["ConversionRate"].ToString());
                                roomReservation.Remarks = reader["Remarks"].ToString();
                                roomReservation.POSRemarks = reader["POSRemarks"].ToString();
                                roomReservation.AirportPickUp = reader["AirportPickUp"].ToString();
                                roomReservation.AirportDrop = reader["AirportDrop"].ToString();
                                roomReservation.ArrivalFlightName = reader["ArrivalFlightName"].ToString();
                                roomReservation.ArrivalFlightNumber = reader["ArrivalFlightNumber"].ToString();

                                if (!string.IsNullOrWhiteSpace(reader["ArrivalTime"].ToString()))
                                {
                                    roomReservation.ArrivalTime = Convert.ToDateTime(reader["ArrivalTime"].ToString());
                                }
                                roomReservation.DepartureAirlineId = Convert.ToInt32(reader["DepartureAirlineId"]);
                                roomReservation.DepartureFlightName = reader["DepartureFlightName"].ToString();
                                roomReservation.DepartureFlightNumber = reader["DepartureFlightNumber"].ToString();

                                if (!string.IsNullOrWhiteSpace(reader["DepartureTime"].ToString()))
                                {
                                    roomReservation.DepartureTime = Convert.ToDateTime(reader["DepartureTime"].ToString());
                                    roomReservation.DepartureTimeString = Convert.ToDateTime(reader["DepartureTime"].ToString()).ToString("HH:mm");
                                }

                                roomReservation.DiscountType = reader["DiscountType"].ToString();
                                roomReservation.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"].ToString());

                                roomReservation.MealPlanId = Convert.ToInt32(reader["MealPlanId"]);
                                roomReservation.IsVIPGuest = Convert.ToBoolean(reader["IsVIPGuest"]);
                                roomReservation.VipGuestTypeId = Convert.ToInt32(reader["VipGuestTypeId"]);
                                roomReservation.ClassificationId = Convert.ToInt32(reader["ClassificationId"]);
                                roomReservation.BookersName = reader["BookersName"].ToString();
                                roomReservation.GuestSourceId = Convert.ToInt32(reader["GuestSourceId"]);
                                roomReservation.MarketSegmentId = Convert.ToInt32(reader["MarketSegmentId"]);
                                roomReservation.IsComplementaryGuest = Convert.ToBoolean(reader["IsComplementaryGuest"]);
                            }
                        }
                    }
                }
            }
            return roomReservation;
        }
        public RoomReservationBO GetRoomReservationInfoByIdNew(Int64 reservationId)
        {
            RoomReservationBO roomReservation = new RoomReservationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfoByIdNew_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int64, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"]);
                                roomReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                roomReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);

                                if (!string.IsNullOrWhiteSpace(reader["ConfirmationDate"].ToString()))
                                {
                                    roomReservation.ConfirmationDate = Convert.ToDateTime(reader["ConfirmationDate"]);
                                }
                                else
                                {
                                    roomReservation.ConfirmationDate = null;
                                }

                                roomReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                                roomReservation.GuestId = Int32.Parse(reader["GuestId"].ToString());
                                roomReservation.ContactAddress = reader["ContactAddress"].ToString();
                                roomReservation.ContactPerson = reader["ContactPerson"].ToString();
                                roomReservation.ContactNumber = reader["ContactNumber"].ToString();
                                roomReservation.MobileNumber = reader["MobileNumber"].ToString();
                                roomReservation.FaxNumber = reader["FaxNumber"].ToString();
                                roomReservation.ContactEmail = reader["ContactEmail"].ToString();
                                roomReservation.TotalRoomNumber = Convert.ToInt32(reader["TotalRoomNumber"]);
                                roomReservation.ReservedMode = reader["ReservedMode"].ToString();
                                roomReservation.ReservationType = reader["ReservationType"].ToString();
                                roomReservation.ReservationMode = reader["ReservationMode"].ToString();
                                roomReservation.IsListedCompany = Convert.ToBoolean(reader["IsListedCompany"]);
                                roomReservation.IsListedContact = Convert.ToBoolean(reader["IsListedContact"]);
                                roomReservation.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                roomReservation.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                roomReservation.IsFamilyOrCouple = Convert.ToBoolean(reader["IsFamilyOrCouple"]);
                                roomReservation.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                roomReservation.ContactId = Convert.ToInt64(reader["ContactId"]);
                                roomReservation.CompanyName = reader["CompanyName"].ToString();
                                roomReservation.BusinessPromotionId = Int32.Parse(reader["BusinessPromotionId"].ToString());
                                roomReservation.Reason = reader["Reason"].ToString();
                                roomReservation.ReferenceId = Convert.ToInt32(reader["ReferenceId"]);
                                roomReservation.MarketSegmentId = Convert.ToInt32(reader["MarketSegmentId"]);
                                roomReservation.GuestSourceId = Convert.ToInt32(reader["GuestSourceId"]);
                                roomReservation.PaymentMode = reader["PaymentMode"].ToString();
                                roomReservation.PayFor = Convert.ToInt32(reader["PayFor"].ToString());
                                roomReservation.CurrencyType = Convert.ToInt32(reader["CurrencyType"].ToString());
                                roomReservation.ConversionRate = Convert.ToDecimal(reader["ConversionRate"].ToString());
                                roomReservation.Remarks = reader["Remarks"].ToString();
                                roomReservation.GuestRemarks = reader["GuestRemarks"].ToString();
                                roomReservation.POSRemarks = reader["POSRemarks"].ToString();
                                roomReservation.AirportPickUp = reader["AirportPickUp"].ToString();
                                roomReservation.AirportDrop = reader["AirportDrop"].ToString();
                                roomReservation.DiscountType = reader["DiscountType"].ToString();
                                roomReservation.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"].ToString());

                                roomReservation.IsRoomRateShowInReservationLetter = Convert.ToBoolean(reader["IsRoomRateShowInReservationLetter"]);
                                roomReservation.IsRoomRateShowInPreRegistrationCard = Convert.ToBoolean(reader["IsRoomRateShowInPreRegistrationCard"]);

                                roomReservation.MealPlanId = Convert.ToInt32(reader["MealPlanId"]);
                                roomReservation.IsVIPGuest = Convert.ToBoolean(reader["IsVIPGuest"]);
                                roomReservation.VipGuestTypeId = Convert.ToInt32(reader["VipGuestTypeId"]);
                                roomReservation.IsComplementaryGuest = Convert.ToBoolean(reader["IsComplementaryGuest"]);
                                roomReservation.ClassificationId = Convert.ToInt32(reader["ClassificationId"]);
                                roomReservation.BookersName = reader["BookersName"].ToString();
                            }
                        }
                    }
                }
            }
            return roomReservation;
        }
        public List<HotelReservationAireportPickupDropBO> GetAirportPickupDropInfoByReservationId(int reservationId)
        {
            List<HotelReservationAireportPickupDropBO> list = new List<HotelReservationAireportPickupDropBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAirportPickupDropInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HotelReservationAireportPickupDropBO bo = new HotelReservationAireportPickupDropBO();
                                bo.APDId = Convert.ToInt32(reader["APDId"].ToString());
                                bo.ReservationId = Convert.ToInt32(reader["ReservationId"].ToString());
                                bo.GuestId = Convert.ToInt32(reader["GuestId"].ToString());
                                bo.GuestName = reader["GuestName"].ToString();
                                bo.ArrivalAirlineId = Convert.ToInt32(reader["ArrivalAirlineId"].ToString());
                                bo.ArrivalFlightName = reader["ArrivalFlightName"].ToString();
                                bo.ArrivalFlightNumber = reader["ArrivalFlightNumber"].ToString();
                                bo.ArrivalTimeShow = reader["ArrivalTimeShow"].ToString();
                                bo.DepartureAirlineId = Convert.ToInt32(reader["DepartureAirlineId"].ToString());
                                bo.DepartureFlightName = reader["DepartureFlightName"].ToString();
                                bo.DepartureFlightNumber = reader["DepartureFlightNumber"].ToString();
                                bo.DepartureTimeShow = reader["DepartureTimeShow"].ToString();
                                bo.PickupDropType = reader["PickupDropType"].ToString();
                                bo.AirportPickUp = reader["AirportPickUp"].ToString();
                                bo.AirportDrop = reader["AirportDrop"].ToString();
                                bo.IsArrivalChargable = Convert.ToBoolean(reader["IsArrivalChargable"].ToString());
                                bo.IsDepartureChargable = Convert.ToBoolean(reader["IsDepartureChargable"].ToString());

                                list.Add(bo);
                            }
                        }
                    }
                }
            }
            return list;
        }
        public RoomReservationBO GetOnlineRoomReservationInfoById(int reservationId)
        {
            RoomReservationBO roomReservation = new RoomReservationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOnlineRoomReservationInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"]);
                                roomReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                roomReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);

                                if (!string.IsNullOrEmpty(reader["ConfirmationDate"].ToString()))
                                    roomReservation.ConfirmationDate = Convert.ToDateTime(reader["ConfirmationDate"]);

                                roomReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                                roomReservation.GuestId = Int32.Parse(reader["GuestId"].ToString());
                                roomReservation.ContactAddress = reader["ContactAddress"].ToString();
                                roomReservation.ContactPerson = reader["ContactPerson"].ToString();
                                roomReservation.ContactNumber = reader["ContactNumber"].ToString();
                                roomReservation.MobileNumber = reader["MobileNumber"].ToString();
                                roomReservation.FaxNumber = reader["FaxNumber"].ToString();
                                roomReservation.ContactEmail = reader["ContactEmail"].ToString();
                                roomReservation.TotalRoomNumber = Convert.ToInt32(reader["TotalRoomNumber"]);
                                roomReservation.ReservedMode = reader["ReservedMode"].ToString();
                                roomReservation.ReservationType = reader["ReservationType"].ToString();
                                roomReservation.ReservationMode = reader["ReservationMode"].ToString();
                                roomReservation.IsListedCompany = Convert.ToBoolean(reader["IsListedCompany"]);
                                roomReservation.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                roomReservation.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                roomReservation.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                roomReservation.CompanyName = reader["CompanyName"].ToString();
                                roomReservation.BusinessPromotionId = Int32.Parse(reader["BusinessPromotionId"].ToString());
                                roomReservation.Reason = reader["Reason"].ToString();
                                roomReservation.ReferenceId = Convert.ToInt32(reader["ReferenceId"]);
                                roomReservation.PaymentMode = reader["PaymentMode"].ToString();
                                roomReservation.PayFor = Convert.ToInt32(reader["PayFor"].ToString());
                                roomReservation.CurrencyType = Convert.ToInt32(reader["CurrencyType"].ToString());
                                roomReservation.ConversionRate = Convert.ToDecimal(reader["ConversionRate"].ToString());
                                roomReservation.ArrivalFlightName = reader["ArrivalFlightName"].ToString();
                                roomReservation.Remarks = reader["Remarks"].ToString();
                                roomReservation.ArrivalFlightNumber = reader["ArrivalFlightNumber"].ToString();
                                if (!string.IsNullOrWhiteSpace(reader["ArrivalTime"].ToString()))
                                {
                                    roomReservation.ArrivalTime = Convert.ToDateTime(reader["ArrivalTime"].ToString());
                                }
                                roomReservation.DepartureFlightName = reader["DepartureFlightName"].ToString();
                                roomReservation.DepartureFlightNumber = reader["DepartureFlightNumber"].ToString();
                                if (!string.IsNullOrWhiteSpace(reader["DepartureTime"].ToString()))
                                {
                                    roomReservation.DepartureTime = Convert.ToDateTime(reader["DepartureTime"].ToString());
                                }

                                roomReservation.DiscountType = reader["DiscountType"].ToString();
                                roomReservation.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"].ToString());
                            }
                        }
                    }
                }
            }
            return roomReservation;
        }
        public List<RoomReservationBO> GetOnlineRoomReservationListByReservationNumber(string ReservationNumber)
        {
            List<RoomReservationBO> roomReservationList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOnlineRoomReservationInfoByReservationNumber_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, ReservationNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomReservation = new RoomReservationBO();

                                roomReservation.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomReservation.GuestName = reader["GuestName"].ToString();
                                if (!string.IsNullOrWhiteSpace(reader["ArrivalTime"].ToString()))
                                {
                                    roomReservation.ArrivalTime = Convert.ToDateTime(reader["ArrivalTime"].ToString());
                                }
                                if (!string.IsNullOrWhiteSpace(reader["DepartureTime"].ToString()))
                                {
                                    roomReservation.DepartureTime = Convert.ToDateTime(reader["DepartureTime"].ToString());
                                }
                                if (!string.IsNullOrWhiteSpace(reader["ReservationDate"].ToString()))
                                {
                                    roomReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"].ToString());
                                }
                                roomReservation.InvoiceNo = reader["InvoiceNo"].ToString();
                                roomReservation.ReceivedBy = reader["ReceivedBy"].ToString();
                                roomReservation.CurrencyName = reader["CurrencyName"].ToString();
                                roomReservation.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"].ToString());
                                roomReservation.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"].ToString());
                                roomReservationList.Add(roomReservation);
                            }
                        }
                    }
                }
            }
            return roomReservationList;
        }
        public RoomReservationBO GetOnlineRoomReservationInfoByReservationNumber(string ReservationNumber)
        {
            RoomReservationBO roomReservation = new RoomReservationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOnlineRoomReservationInfoByReservationNumber_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, ReservationNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if(reader != null)
                        {
                            while (reader.Read())
                            {
                                roomReservation.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomReservation.GuestName = reader["GuestName"].ToString();
                                if (!string.IsNullOrWhiteSpace(reader["ArrivalTime"].ToString()))
                                {
                                    roomReservation.ArrivalTime = Convert.ToDateTime(reader["ArrivalTime"].ToString());
                                }
                                if (!string.IsNullOrWhiteSpace(reader["DepartureTime"].ToString()))
                                {
                                    roomReservation.DepartureTime = Convert.ToDateTime(reader["DepartureTime"].ToString());
                                }
                                if (!string.IsNullOrWhiteSpace(reader["ReservationDate"].ToString()))
                                {
                                    roomReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"].ToString());
                                }
                                roomReservation.InvoiceNo = reader["InvoiceNo"].ToString();
                                roomReservation.ReceivedBy = reader["ReceivedBy"].ToString();
                                roomReservation.CurrencyName = reader["CurrencyName"].ToString();
                                roomReservation.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"].ToString());
                                roomReservation.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"].ToString());
                            }
                        }
                    }
                }
            }
            return roomReservation;
        }
        public Boolean DeleteReservationDetailInfoById(int roomReservationId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteReservationDetailInfoById_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int32, roomReservationId);
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
        public Boolean UpdateHotelReservationContactInformation(int roomReservationId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateHotelReservationContactInformation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int32, roomReservationId);
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
        public Boolean UpdateHotelRoomReservationRoomDetail(int roomReservationId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateHotelRoomReservationRoomDetail_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int64, roomReservationId);
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
        public ReservationBillPaymentBO GetReservationAdvanceTotalAmountInfoById(int reservationId)
        {
            ReservationBillPaymentBO roomReservation = new ReservationBillPaymentBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservationAdvanceTotalAmountInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomReservation.PaymentAmount = Convert.ToDecimal(reader["TotalAmount"]);
                            }
                        }
                    }
                }
            }
            return roomReservation;
        }
        public ReservationBillPaymentBO GetRoomReservationAdvanceAmountInfoById(int reservationId)
        {
            ReservationBillPaymentBO roomReservation = new ReservationBillPaymentBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationAdvanceAmountInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomReservation.PaymentAmount = Convert.ToDecimal(reader["TotalAmount"]);
                            }
                        }
                    }
                }
            }
            return roomReservation;
        }
        public int SaveTemporaryGuest(GuestInformationBO registrationDetailBO, string tempRegId, List<GuestPreferenceMappingBO> preferenList)
        {
            bool retVal = false;
            int status = 0;
            int TempGuestId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveGuestInformationForReservationNew_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        commandMaster.Parameters.Clear();
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int32, tempRegId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                        dbSmartAspects.AddInParameter(commandMaster, "@Title", DbType.String, registrationDetailBO.Title);
                        dbSmartAspects.AddInParameter(commandMaster, "@FirstName", DbType.String, registrationDetailBO.FirstName);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastName", DbType.String, registrationDetailBO.LastName);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestName", DbType.String, registrationDetailBO.GuestName);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestDOB", DbType.DateTime, registrationDetailBO.GuestDOB);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestSex", DbType.String, registrationDetailBO.GuestSex);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestEmail", DbType.String, registrationDetailBO.GuestEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestPhone", DbType.String, registrationDetailBO.GuestPhone);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestAddress1", DbType.String, registrationDetailBO.GuestAddress1);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestAddress2", DbType.String, registrationDetailBO.GuestAddress2);
                        dbSmartAspects.AddInParameter(commandMaster, "@ProfessionId", DbType.Int32, registrationDetailBO.ProfessionId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestCity", DbType.String, registrationDetailBO.GuestCity);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestZipCode", DbType.String, registrationDetailBO.GuestZipCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestCountryId", DbType.Int32, registrationDetailBO.GuestCountryId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestNationality", DbType.String, registrationDetailBO.GuestNationality);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestDrivinlgLicense", DbType.String, registrationDetailBO.GuestDrivinlgLicense);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestAuthentication", DbType.String, registrationDetailBO.GuestAuthentication);
                        dbSmartAspects.AddInParameter(commandMaster, "@NationalId", DbType.String, registrationDetailBO.NationalId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PassportNumber", DbType.String, registrationDetailBO.PassportNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@PIssueDate", DbType.DateTime, registrationDetailBO.PIssueDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@PIssuePlace", DbType.String, registrationDetailBO.PIssuePlace);
                        dbSmartAspects.AddInParameter(commandMaster, "@PExpireDate", DbType.DateTime, registrationDetailBO.PExpireDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@VisaNumber", DbType.String, registrationDetailBO.VisaNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@VIssueDate", DbType.DateTime, registrationDetailBO.VIssueDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@VExpireDate", DbType.DateTime, registrationDetailBO.VExpireDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomId", DbType.Int32, registrationDetailBO.RoomId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ClassificationId", DbType.Int32, registrationDetailBO.ClassificationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@AdditionalRemarks", DbType.String, registrationDetailBO.AdditionalRemarks);
                        dbSmartAspects.AddOutParameter(commandMaster, "@TempGuestId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        TempGuestId = Convert.ToInt32(commandMaster.Parameters["@TempGuestId"].Value);

                        using (DbCommand commandPreference = dbSmartAspects.GetStoredProcCommand("SaveGuestPreferenceMappingInfo_SP"))
                        {
                            string preferences = string.Join(",", preferenList.Select(i => i.PreferenceId));

                            commandPreference.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandPreference, "@GuestId", DbType.Int32, TempGuestId);
                            dbSmartAspects.AddInParameter(commandPreference, "@PreferenceIdList", DbType.String, preferences);

                            status = dbSmartAspects.ExecuteNonQuery(commandPreference, transction);

                        }
                        transction.Commit();
                    }
                }
            }
            return TempGuestId;
        }
        //express check in
        public bool SaveTemporaryGuestNew(GuestInformationBO registrationDetailBO, string tempRegId, List<GuestPreferenceMappingBO> preferenList)
        {
            bool retVal = false;
            int status = 0;
            int TempGuestId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveGuestInformationForReservationNew_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        commandMaster.Parameters.Clear();
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int32, tempRegId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                        dbSmartAspects.AddInParameter(commandMaster, "@Title", DbType.String, registrationDetailBO.Title);
                        if (registrationDetailBO.Title == null && registrationDetailBO.FirstName == null)
                            dbSmartAspects.AddInParameter(commandMaster, "@FirstName", DbType.String, registrationDetailBO.GuestName);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@FirstName", DbType.String, registrationDetailBO.FirstName);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastName", DbType.String, registrationDetailBO.LastName);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestName", DbType.String, registrationDetailBO.GuestName);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestDOB", DbType.DateTime, registrationDetailBO.GuestDOB);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestSex", DbType.String, registrationDetailBO.GuestSex);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestEmail", DbType.String, registrationDetailBO.GuestEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestPhone", DbType.String, registrationDetailBO.GuestPhone);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestAddress1", DbType.String, registrationDetailBO.GuestAddress1);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestAddress2", DbType.String, registrationDetailBO.GuestAddress2);
                        dbSmartAspects.AddInParameter(commandMaster, "@ProfessionId", DbType.Int32, registrationDetailBO.ProfessionId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestCity", DbType.String, registrationDetailBO.GuestCity);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestZipCode", DbType.String, registrationDetailBO.GuestZipCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestCountryId", DbType.Int32, registrationDetailBO.GuestCountryId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestNationality", DbType.String, registrationDetailBO.GuestNationality);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestDrivinlgLicense", DbType.String, registrationDetailBO.GuestDrivinlgLicense);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestAuthentication", DbType.String, registrationDetailBO.GuestAuthentication);
                        dbSmartAspects.AddInParameter(commandMaster, "@NationalId", DbType.String, registrationDetailBO.NationalId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PassportNumber", DbType.String, registrationDetailBO.PassportNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@PIssueDate", DbType.DateTime, registrationDetailBO.PIssueDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@PIssuePlace", DbType.String, registrationDetailBO.PIssuePlace);
                        dbSmartAspects.AddInParameter(commandMaster, "@PExpireDate", DbType.DateTime, registrationDetailBO.PExpireDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@VisaNumber", DbType.String, registrationDetailBO.VisaNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@VIssueDate", DbType.DateTime, registrationDetailBO.VIssueDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@VExpireDate", DbType.DateTime, registrationDetailBO.VExpireDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomId", DbType.Int32, registrationDetailBO.RoomId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ClassificationId", DbType.Int32, registrationDetailBO.ClassificationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@AdditionalRemarks", DbType.String, registrationDetailBO.AdditionalRemarks);
                        dbSmartAspects.AddOutParameter(commandMaster, "@TempGuestId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        TempGuestId = Convert.ToInt32(commandMaster.Parameters["@TempGuestId"].Value);

                        using (DbCommand commandPreference = dbSmartAspects.GetStoredProcCommand("SaveGuestPreferenceMappingInfo_SP"))
                        {
                            string preferences = string.Join(",", preferenList.Select(i => i.PreferenceId));

                            commandPreference.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandPreference, "@GuestId", DbType.Int32, TempGuestId);
                            dbSmartAspects.AddInParameter(commandPreference, "@PreferenceIdList", DbType.String, preferences);

                            status = dbSmartAspects.ExecuteNonQuery(commandPreference, transction);

                        }
                        transction.Commit();
                    }
                }
            }
            if (status > 0)
                return true;
            else
                return false;
        }
        public bool SaveTemporaryGuestOnline(GuestInformationBO registrationDetailBO, string tempRegId)
        {
            bool retVal = false;
            int status = 0;
            int TempGuestId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveGuestInformationForReservationOnline_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        commandMaster.Parameters.Clear();
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int32, tempRegId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                        dbSmartAspects.AddInParameter(commandMaster, "@Title", DbType.String, registrationDetailBO.Title);
                        dbSmartAspects.AddInParameter(commandMaster, "@FirstName", DbType.String, registrationDetailBO.FirstName);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastName", DbType.String, registrationDetailBO.LastName);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestName", DbType.String, registrationDetailBO.GuestName);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestDOB", DbType.DateTime, registrationDetailBO.GuestDOB);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestSex", DbType.String, registrationDetailBO.GuestSex);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestEmail", DbType.String, registrationDetailBO.GuestEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestPhone", DbType.String, registrationDetailBO.GuestPhone);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestAddress1", DbType.String, registrationDetailBO.GuestAddress1);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestAddress2", DbType.String, registrationDetailBO.GuestAddress2);
                        dbSmartAspects.AddInParameter(commandMaster, "@ProfessionId", DbType.Int32, registrationDetailBO.ProfessionId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestCity", DbType.String, registrationDetailBO.GuestCity);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestZipCode", DbType.String, registrationDetailBO.GuestZipCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestCountryId", DbType.Int32, registrationDetailBO.GuestCountryId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestNationality", DbType.String, registrationDetailBO.GuestNationality);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestDrivinlgLicense", DbType.String, registrationDetailBO.GuestDrivinlgLicense);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestAuthentication", DbType.String, registrationDetailBO.GuestAuthentication);
                        dbSmartAspects.AddInParameter(commandMaster, "@NationalId", DbType.String, registrationDetailBO.NationalId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PassportNumber", DbType.String, registrationDetailBO.PassportNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@PIssueDate", DbType.DateTime, registrationDetailBO.PIssueDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@PIssuePlace", DbType.String, registrationDetailBO.PIssuePlace);
                        dbSmartAspects.AddInParameter(commandMaster, "@PExpireDate", DbType.DateTime, registrationDetailBO.PExpireDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@VisaNumber", DbType.String, registrationDetailBO.VisaNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@VIssueDate", DbType.DateTime, registrationDetailBO.VIssueDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@VExpireDate", DbType.DateTime, registrationDetailBO.VExpireDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomId", DbType.Int32, registrationDetailBO.RoomId);
                        dbSmartAspects.AddOutParameter(commandMaster, "@TempGuestId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        TempGuestId = Convert.ToInt32(commandMaster.Parameters["@TempGuestId"].Value);
                        transction.Commit();
                    }
                }
            }
            return true;
        }
        public bool DeleteTempGuestReservation(int reservationId, int guestId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteTempGuestReservation_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int32, reservationId);
                    dbSmartAspects.AddInParameter(command, "@GuestId", DbType.Int32, guestId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<RoomReservationBO> GetRoomReservationInfoByDate(DateTime FromDate, DateTime ToDate, string reservationNumber, string reservationGuest)
        {
            string Where = GenarateWhereCondition(FromDate, ToDate, reservationNumber, reservationGuest);
            List<RoomReservationBO> roomReservationList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfoByDate_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomReservation = new RoomReservationBO();
                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"]);
                                roomReservation.GuestName = reader["GuestName"].ToString();
                                roomReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                roomReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);

                                if (!string.IsNullOrEmpty(reader["ConfirmationDate"].ToString()))
                                    roomReservation.ConfirmationDate = Convert.ToDateTime(reader["ConfirmationDate"]);

                                roomReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                                roomReservation.ContactPerson = reader["ContactPerson"].ToString();
                                roomReservation.ContactNumber = reader["ContactNumber"].ToString();
                                roomReservation.GuestId = Int32.Parse(reader["GuestId"].ToString());
                                roomReservation.Reason = reader["Reason"].ToString();
                                roomReservation.BusinessPromotionId = Int32.Parse(reader["BusinessPromotionId"].ToString());
                                roomReservation.TotalRoomNumber = Convert.ToInt32(reader["TotalRoomNumber"]);
                                roomReservation.ReservedMode = reader["ReservedMode"].ToString();
                                roomReservation.ReservationType = reader["ReservationType"].ToString();
                                roomReservation.ReservationMode = reader["ReservationMode"].ToString();
                                roomReservationList.Add(roomReservation);
                            }
                        }
                    }
                }
            }
            return roomReservationList;
        }
        public List<RoomReservationBO> GetRoomReservationInformationBySearchCriteriaForPaging(DateTime? fromDate, DateTime? toDate, string guestName, string reserveNo, string companyName, string contactPerson, string contactPhone, string contactEmail, int srcMarketSegment, int srcGuestSource, int srcReferenceId, int ordering, string status, int recordPerPage, int pageIndex, out int totalRecords)
        {
            string Where = GenerateReservationSearchWhereCondition(fromDate, toDate, guestName, companyName, reserveNo, contactPerson, contactPhone, contactEmail, srcMarketSegment, srcGuestSource, srcReferenceId, status);
            List<RoomReservationBO> roomreservationList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfoBySearchCriteriaForpagination_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);
                    dbSmartAspects.AddInParameter(cmd, "@Ordering", DbType.Int32, ordering);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomreservationDetail = new RoomReservationBO();
                                roomreservationDetail.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomreservationDetail.PickUpDropCount = Convert.ToInt32(reader["PickUpDropCount"]);
                                roomreservationDetail.GuestName = reader["GuestName"].ToString();
                                roomreservationDetail.ReservationNumber = reader["ReservationNumber"].ToString();
                                string reservedate = Convert.ToDateTime(reader["ReservationDate"].ToString()).ToString("dd-MMM-yyyy");
                                roomreservationDetail.ReservationDate = Convert.ToDateTime(reservedate);
                                string dateIn = Convert.ToDateTime(reader["DateIn"].ToString()).ToString("dd-MMM-yyyy");
                                roomreservationDetail.DateIn = Convert.ToDateTime(dateIn);
                                string dateOut = Convert.ToDateTime(reader["DateOut"].ToString()).ToString("dd-MMM-yyyy");
                                roomreservationDetail.DateOut = Convert.ToDateTime(dateOut);
                                roomreservationDetail.ReservationNumber = roomreservationDetail.ReservationNumber.ToString();
                                roomreservationDetail.ReservationMode = reader["ReservationMode"].ToString();
                                roomreservationDetail.CompanyName = reader["CompanyName"].ToString();
                                roomreservationDetail.RoomInformation = reader["RoomInformation"].ToString();
                                roomreservationDetail.GroupMasterId = Convert.ToInt64(reader["GroupMasterId"]);
                                roomreservationList.Add(roomreservationDetail);
                            }
                        }
                    }

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return roomreservationList;
        }
        public List<RoomReservationBO> GetRoomReservationInfoByStringSearchCriteria(DateTime? fromDate, DateTime? toDate, string guestName, string reserveNo, string companyName, string contactPerson, string contactPhone, string contactEmail, int srcMarketSegment, int srcGuestSource, int srcReferenceId, string status)
        {
            string Where = GenerateReservationSearchWhereCondition(fromDate, toDate, guestName, companyName, reserveNo, contactPerson, contactPhone, contactEmail, srcMarketSegment, srcGuestSource, srcReferenceId, status);
            List<RoomReservationBO> roomreservationList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfoByStringSearchCriteria_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomreservationBO = new RoomReservationBO();
                                roomreservationBO.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomreservationBO.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomreservationBO.ReservationDateDisplay = reader["ReservationDateDisplay"].ToString();
                                roomreservationBO.DateInDisplay = reader["DateInDisplay"].ToString();
                                roomreservationBO.DateOutDisplay = reader["DateOutDisplay"].ToString();
                                roomreservationBO.GuestName = reader["GuestName"].ToString();
                                roomreservationBO.CompanyName = reader["CompanyName"].ToString();
                                roomreservationBO.Reason = reader["Reason"].ToString();                                
                                roomreservationBO.IsTaggedOnGroupReservation = Convert.ToBoolean(reader["IsTaggedOnGroupReservation"]);
                                roomreservationList.Add(roomreservationBO);
                            }
                        }
                    }

                    //totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return roomreservationList;
        }
        public List<RoomReservationBO> GetGroupRoomReservationInfoByStringSearchCriteria(DateTime? fromDate, DateTime? toDate, string reservationNumber, string groupName)
        {
            string Where = GenerateGroupRoomReservationSearchWhereCondition(fromDate, toDate, reservationNumber, groupName);
            List<RoomReservationBO> roomreservationList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGroupRoomReservationInfoByStringSearchCriteria_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomreservationBO = new RoomReservationBO();
                                roomreservationBO.Id = Convert.ToInt32(reader["Id"]);
                                roomreservationBO.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomreservationBO.ReservationDateDisplay = reader["ReservationDateDisplay"].ToString();
                                roomreservationBO.GroupName = reader["GroupName"].ToString();
                                roomreservationBO.CheckInDateDisplay = reader["CheckInDateDisplay"].ToString();
                                roomreservationBO.CheckOutDateDisplay = reader["CheckOutDateDisplay"].ToString();                                
                                roomreservationBO.ReservationDetails = reader["ReservationDetails"].ToString();
                                roomreservationBO.GroupDescription = reader["GroupDescription"].ToString();
                                roomreservationList.Add(roomreservationBO);
                            }
                        }
                    }

                    //totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return roomreservationList;
        }
        public string GenerateReservationSearchWhereCondition(DateTime? fromDate, DateTime? toDate, string guestName, string companyName, string reserveNo, string contactPerson, string contactPhone, string contactEmail, int srcMarketSegment, int srcGuestSource, int srcReferenceId, string status)
        {
            string Where = string.Empty;
            guestName = guestName.Trim();
            reserveNo = reserveNo.Trim();
            contactPerson = contactPerson.Trim();

            //Date
            if (fromDate != null)
            {
                string strFromDate = fromDate.HasValue ? fromDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                Where = "(dbo.FnDate(rr.DateIn) BETWEEN dbo.FnDate('" + strFromDate + "') AND dbo.FnDate('" + strFromDate + "') )";
            }
            if (toDate != null)
            {
                string strToDate = toDate.HasValue ? toDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                Where = "(dbo.FnDate(rr.DateIn) BETWEEN dbo.FnDate('" + strToDate + "') AND dbo.FnDate('" + strToDate + "') )";
            }
            if (fromDate != null && toDate != null)
            {
                string strFromDate = fromDate.HasValue ? fromDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                string strToDate = toDate.HasValue ? toDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                Where = "(dbo.FnDate(rr.DateIn) BETWEEN dbo.FnDate('" + strFromDate + "') AND dbo.FnDate('" + strToDate + "') )";
            }

            //GuestName
            if (!string.IsNullOrWhiteSpace(guestName))
            {
                if (!string.IsNullOrWhiteSpace(Where))
                {
                    Where += " AND dbo.FnGuestInformationListWithCommaSeperator(rr.ReservationId, 'Reservation') LIKE '%" + guestName + "%'";
                }
                else
                {
                    Where += "dbo.FnGuestInformationListWithCommaSeperator(rr.ReservationId, 'Reservation') LIKE '%" + guestName + "%'";
                }
            }

            //CompanyName
            if (!string.IsNullOrWhiteSpace(companyName))
            {
                if (!string.IsNullOrWhiteSpace(Where))
                {
                    Where += " AND CompanyName LIKE '%" + companyName + "%'";
                }
                else
                {
                    Where += "CompanyName LIKE '%" + companyName + "%'";
                }
            }

            //ReservationNumber
            if (!string.IsNullOrWhiteSpace(reserveNo))
            {
                if (!string.IsNullOrWhiteSpace(Where))
                {
                    Where += " AND rr.ReservationNumber LIKE '%" + reserveNo + "%'";
                }
                else
                {
                    Where += "rr.ReservationNumber LIKE '%" + reserveNo + "%'";
                }
            }

            //ContactPerson
            if (!string.IsNullOrWhiteSpace(contactPerson))
            {
                if (!string.IsNullOrWhiteSpace(Where))
                {
                    Where += " AND rr.ContactPerson LIKE '%" + contactPerson + "%'";
                }
                else
                {
                    Where += "rr.ContactPerson LIKE '%" + contactPerson + "%'";
                }
            }

            //ContactPhone
            if (!string.IsNullOrWhiteSpace(contactPhone))
            {
                if (!string.IsNullOrWhiteSpace(Where))
                {
                    Where += " AND rr.ContactNumber LIKE '%" + contactPhone + "%'";
                }
                else
                {
                    Where += "rr.ContactNumber LIKE '%" + contactPhone + "%'";
                }
            }

            //ContactEmail
            if (!string.IsNullOrWhiteSpace(contactEmail))
            {
                if (!string.IsNullOrWhiteSpace(Where))
                {
                    Where += " AND rr.ContactEmail LIKE '%" + contactEmail + "%'";
                }
                else
                {
                    Where += "rr.ContactEmail LIKE '%" + contactEmail + "%'";
                }
            }

            //MarketSegment
            if (srcMarketSegment > 0)
            {
                if (!string.IsNullOrWhiteSpace(Where))
                {
                    Where += " AND rr.MarketSegmentId = '" + srcMarketSegment + "'";
                }
                else
                {
                    Where += "rr.MarketSegmentId = '" + srcMarketSegment + "'";
                }
            }

            //GuestSource
            if (srcGuestSource > 0)
            {
                if (!string.IsNullOrWhiteSpace(Where))
                {
                    Where += " AND rr.GuestSourceId = '" + srcGuestSource + "'";
                }
                else
                {
                    Where += "rr.GuestSourceId = '" + srcGuestSource + "'";
                }
            }

            //Reference
            if (srcReferenceId > 0)
            {
                if (!string.IsNullOrWhiteSpace(Where))
                {
                    Where += " AND rr.ReferenceId = '" + srcReferenceId + "'";
                }
                else
                {
                    Where += "rr.ReferenceId = '" + srcReferenceId + "'";
                }
            }

            //Status
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (!string.IsNullOrWhiteSpace(Where))
                {
                    Where += " AND rr.ReservationMode = '" + status + "'";
                }
                else
                {
                    Where += "rr.ReservationMode = '" + status + "'";
                }
            }

            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = " WHERE " + Where;
            }

            return Where;
        }
        public string GenerateGroupRoomReservationSearchWhereCondition(DateTime? fromDate, DateTime? toDate, string reservationNumber, string groupName)
        {
            string Where = string.Empty;
            groupName = groupName.Trim();

            //Date
            if (fromDate != null)
            {
                string strFromDate = fromDate.HasValue ? fromDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                Where = "(dbo.FnDate(hgrr.CheckInDate) BETWEEN dbo.FnDate('" + strFromDate + "') AND dbo.FnDate('" + strFromDate + "') )";
            }
            if (toDate != null)
            {
                string strToDate = toDate.HasValue ? toDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                Where = "(dbo.FnDate(hgrr.CheckInDate) BETWEEN dbo.FnDate('" + strToDate + "') AND dbo.FnDate('" + strToDate + "') )";
            }
            if (fromDate != null && toDate != null)
            {
                string strFromDate = fromDate.HasValue ? fromDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                string strToDate = toDate.HasValue ? toDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                Where = "(dbo.FnDate(hgrr.CheckInDate) BETWEEN dbo.FnDate('" + strFromDate + "') AND dbo.FnDate('" + strToDate + "') )";
            }

            //Reservation Number
            if (!string.IsNullOrWhiteSpace(reservationNumber))
            {
                if (!string.IsNullOrWhiteSpace(Where))
                {
                    Where += " AND hgrr.ReservationNumber LIKE '%" + reservationNumber + "%'";
                }
                else
                {
                    Where += "hgrr.ReservationNumber LIKE '%" + reservationNumber + "%'";
                }
            }

            //Group Name
            if (!string.IsNullOrWhiteSpace(groupName))
            {
                if (!string.IsNullOrWhiteSpace(Where))
                {
                    Where += " AND hgrr.GroupName LIKE '%" + groupName + "%'";
                }
                else
                {
                    Where += "hgrr.GroupName LIKE '%" + groupName + "%'";
                }
            }

            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = " WHERE " + Where;
            }

            return Where;
        }
        public List<RoomReservationBO> GetOnlineRoomReservationInformationBySearchCriteriaForPaging(DateTime? fromDate, DateTime? toDate, string guestName, string reserveNo, string companyName, string contactPerson, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<RoomReservationBO> roomreservationList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOnlineRoomReservationInfoBySearchCriteriaForpagination_SP"))
                {

                    if (fromDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                    }

                    if (toDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(guestName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, guestName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(reserveNo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReserveNo", DbType.String, reserveNo);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReserveNo", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(companyName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(contactPerson))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ContactPerson", DbType.String, contactPerson);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ContactPerson", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomreservationDetail = new RoomReservationBO();

                                roomreservationDetail.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomreservationDetail.GuestName = reader["GuestName"].ToString();
                                roomreservationDetail.ReservationNumber = reader["ReservationNumber"].ToString();
                                string reservedate = Convert.ToDateTime(reader["ReservationDate"].ToString()).ToString("dd-MMM-yyyy");
                                roomreservationDetail.ReservationDate = Convert.ToDateTime(reservedate);
                                string dateIn = Convert.ToDateTime(reader["DateIn"].ToString()).ToString("dd-MMM-yyyy");
                                roomreservationDetail.DateIn = Convert.ToDateTime(dateIn);
                                string dateOut = Convert.ToDateTime(reader["DateOut"].ToString()).ToString("dd-MMM-yyyy");
                                roomreservationDetail.DateOut = Convert.ToDateTime(dateOut);
                                roomreservationDetail.ReservationNumber = roomreservationDetail.ReservationNumber.ToString();
                                roomreservationDetail.ReservationMode = reader["ReservationMode"].ToString();
                                roomreservationList.Add(roomreservationDetail);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return roomreservationList;
        }
        public string GenarateWhereCondition(DateTime FromDate, DateTime ToDate, string reservationGuest, string reservationNumber)
        {
            string Where = string.Empty;
            reservationNumber = reservationNumber.Trim();
            reservationGuest = reservationGuest.Trim();
            if (!string.IsNullOrEmpty(reservationNumber))
            {
                Where = "(dbo.FnDate(DateIn) BETWEEN dbo.FnDate('" + FromDate.ToString("yyyy-MM-dd") + "') AND dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "') )";
                if (!string.IsNullOrEmpty(reservationNumber))
                {
                    if (!string.IsNullOrEmpty(Where))
                    {
                        Where += " AND ReservationNumber LIKE '%" + reservationNumber + "%'";
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(reservationGuest))
                {

                    Where = "(dbo.FnDate(DateIn) BETWEEN dbo.FnDate('" + FromDate.ToString("yyyy-MM-dd") + "') AND dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "') )";
                    if (!string.IsNullOrEmpty(reservationGuest))
                    {
                        if (!string.IsNullOrEmpty(Where))
                        {
                            Where += " AND dbo.FnGuestInformationListWithCommaSeperator(ReservationId, 'Reservation') LIKE '%" + reservationGuest + "%'";
                        }
                    }
                }
                else
                {
                    Where = "dbo.FnDate(DateIn) BETWEEN dbo.FnDate('" + FromDate.ToString("yyyy-MM-dd") + "') AND dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "')";
                    if (!string.IsNullOrEmpty(reservationNumber))
                    {
                        if (!string.IsNullOrEmpty(Where))
                        {
                            Where += " OR  ReservationNumber LIKE '%" + reservationGuest + "%'";
                            Where += " OR  dbo.FnGetFirstGuestNameForReservation(ReservationId) LIKE '%" + reservationGuest + "%'";
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = " WHERE " + Where;
            }

            return Where;
        }
        public RoomReservationBO GetRoomReservationInfoByRoomId(int RoomId, DateTime RoomStatusDate)
        {
            RoomReservationBO reservationBO = new RoomReservationBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfoByRoomId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomId", DbType.String, RoomId);
                    dbSmartAspects.AddInParameter(cmd, "@RoomStatusDate", DbType.DateTime, RoomStatusDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                reservationBO.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                reservationBO.ReservationNumber = reader["ReservationNumber"].ToString();
                                reservationBO.ReservationDate = Convert.ToDateTime(reader["ReservationDate"]);
                                reservationBO.GuestName = reader["GuestName"].ToString();
                                reservationBO.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                reservationBO.DateOut = Convert.ToDateTime(reader["DateOut"]);

                                if (!string.IsNullOrEmpty(reader["ConfirmationDate"].ToString()))
                                {
                                    reservationBO.ConfirmationDate = Convert.ToDateTime(reader["ConfirmationDate"]);
                                }

                                reservationBO.ReservedCompany = reader["ReservedCompany"].ToString();
                                reservationBO.ContactPerson = reader["ContactPerson"].ToString();
                                reservationBO.ContactNumber = reader["ContactNumber"].ToString();
                                reservationBO.TotalRoomNumber = Convert.ToInt32(reader["TotalRoomNumber"]);
                                reservationBO.ReservedMode = reader["ReservedMode"].ToString();
                                reservationBO.ReservationType = reader["ReservationType"].ToString();
                                reservationBO.ReservationMode = reader["ReservationMode"].ToString();
                            }
                        }
                    }
                }
            }
            return reservationBO;
        }
        public RoomReservationBO GetRoomReservationInformationByRoomIdNRoomStatusDate(int roomId, DateTime roomStatusDate)
        {
            RoomReservationBO roomReservation = new RoomReservationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInformationByRoomIdNRoomStatusDate_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomId", DbType.Int32, roomId);
                    dbSmartAspects.AddInParameter(cmd, "@RoomStatusDate", DbType.DateTime, roomStatusDate);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                                roomReservation.ReservationNDetailNRoomId = reader["ReservationId"].ToString() + "~" + reader["ReservationDetailId"].ToString() + "~" + reader["RoomId"].ToString();

                                roomReservation.IsServiceChargeEnable = Convert.ToBoolean(reader["IsServiceChargeEnable"]);
                                roomReservation.IsCityChargeEnable = Convert.ToBoolean(reader["IsCityChargeEnable"]);
                                roomReservation.IsVatAmountEnable = Convert.ToBoolean(reader["IsVatAmountEnable"]);
                                roomReservation.IsAdditionalChargeEnable = Convert.ToBoolean(reader["IsAdditionalChargeEnable"]);
                            }
                        }
                    }
                }
            }
            return roomReservation;
        }
        public List<RoomReservationInfoByDateRangeReportBO> GetRoomReservationInfoByDateRange(DateTime fromDate, DateTime toDate, string reservationNumber, string company, string reference, string reservationMode, string roomNumber, string filterBy, int reservedBy)
        {
            List<RoomReservationInfoByDateRangeReportBO> roomReservation = new List<RoomReservationInfoByDateRangeReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfoByDateRange_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);

                    if (!string.IsNullOrEmpty(reservationNumber))
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNo", DbType.String, reservationNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNo", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(company))
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.String, company);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(reference))
                        dbSmartAspects.AddInParameter(cmd, "@ReferenceId", DbType.String, reference);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReferenceId", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(reservationMode))
                        dbSmartAspects.AddInParameter(cmd, "@ReservationMode", DbType.String, reservationMode);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReservationMode", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(roomNumber))
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@FilterBy", DbType.String, filterBy);

                    if (reservedBy > 0)
                        dbSmartAspects.AddInParameter(cmd, "@ReservedBy", DbType.Int32, reservedBy);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReservedBy", DbType.Int32, DBNull.Value);

                    DataSet reservationDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, reservationDS, "Reservation");
                    DataTable Tabel = reservationDS.Tables["Reservation"];

                    roomReservation = Tabel.AsEnumerable().Select(r => new RoomReservationInfoByDateRangeReportBO
                    {
                        FromDate = r.Field<DateTime?>("FromDate"),
                        ToDate = r.Field<DateTime?>("ToDate"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        BookingDate = r.Field<string>("BookingDate"),
                        CheckIn = r.Field<string>("CheckIn"),
                        ProbableCheckInTime = r.Field<string>("ProbableCheckInTime"),
                        DateOut = r.Field<string>("DateOut"),
                        CurrencyTypeId = r.Field<int?>("CurrencyTypeId"),
                        GuestName = r.Field<string>("GuestName"),
                        GuestList = r.Field<string>("GuestList"),
                        TotalPerson = r.Field<int?>("TotalPerson"),
                        CompanyORPrivate = r.Field<string>("CompanyORPrivate"),
                        RoomType = r.Field<string>("RoomType"),
                        Quantity = r.Field<int?>("Quantity"),
                        RoomNumberList = r.Field<string>("RoomNumberList"),
                        RoomRate = r.Field<decimal?>("RoomRate"),
                        NoOfNights = r.Field<int?>("NoOfNights"),
                        CurrencyType = r.Field<string>("CurrencyType"),
                        PaymentMode = r.Field<string>("PaymentMode"),
                        GuestReferance = r.Field<string>("GuestReferance"),
                        ReservationMode = r.Field<string>("ReservationMode"),
                        Reason = r.Field<string>("Reason"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        ContactNumber = r.Field<string>("ContactNumber"),
                        ReservedBy = r.Field<string>("ReservedBy"),
                        ReservationAdvanceTotal = r.Field<decimal>("ReservationAdvanceTotal"),
                        ReservationDescription = r.Field<string>("ReservationDescription"),
                    }).ToList();
                }
                return roomReservation;
            }
        }
        public List<RoomReservationBillBO> GetRoomReservationBill(int reservationNumber)
        {
            List<RoomReservationBillBO> roomReservationList = new List<RoomReservationBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("RoomReservationBillGenerateForEmailSend"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.Int32, reservationNumber);

                    DataSet reservationDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, reservationDS, "Reservation");
                    DataTable Tabel = reservationDS.Tables["Reservation"];

                    roomReservationList = Tabel.AsEnumerable().Select(r => new RoomReservationBillBO
                    {
                        CompanyName = r.Field<string>("CompanyName"),
                        CompanyAddress = r.Field<string>("CompanyAddress"),
                        WebAddress = r.Field<string>("WebAddress"),
                        GuestName = r.Field<string>("GuestName"),
                        GuestList = r.Field<string>("GuestList"),
                        TotalGuest = r.Field<int?>("TotalGuest"),
                        GuestCompanyName = r.Field<string>("GuestCompanyName"),
                        GuestCompanyAddress = r.Field<string>("GuestCompanyAddress"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        ContactDesignation = r.Field<string>("ContactDesignation"),
                        TelephoneNumber = r.Field<string>("TelephoneNumber"),
                        ContactNumber = r.Field<string>("ContactNumber"),
                        FaxNumber = r.Field<string>("FaxNumber"),
                        ContactEmail = r.Field<string>("ContactEmail"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        ReferencePerson = r.Field<string>("ReferencePerson"),
                        ReferenceDesignation = r.Field<string>("ReferenceDesignation"),
                        ReferenceOrganization = r.Field<string>("ReferenceOrganization"),
                        ReferenceTelephone = r.Field<string>("ReferenceTelephone"),
                        ReferenceCellNumber = r.Field<string>("ReferenceCellNumber"),
                        ReferenceEmail = r.Field<string>("ReferenceEmail"),
                        ReservedMode = r.Field<string>("ReservedMode"),
                        PaymentMode = r.Field<string>("PaymentMode"),
                        MethodOfPayment = r.Field<string>("MethodOfPayment"),
                        RoomType = r.Field<string>("RoomType"),
                        RoomTypeWisePaxQuantity = r.Field<int>("RoomTypeWisePaxQuantity"),
                        TypeWiseTotalRooms = r.Field<int?>("TypeWiseTotalRooms"),
                        UnitPrice = r.Field<decimal?>("UnitPrice"),
                        RoomRate = r.Field<decimal?>("RoomRate"),
                        TotalNumberOfRooms = r.Field<decimal?>("TotalNumberOfRooms"),
                        LocalCurrencyType = r.Field<string>("LocalCurrencyType"),
                        CurrencyTypeId = r.Field<int?>("CurrencyTypeId"),
                        CurrencyType = r.Field<string>("CurrencyType"),
                        ConversionRate = r.Field<decimal>("ConversionRate"),
                        AirportPickUp = r.Field<string>("AirportPickUp"),
                        ArrivalFlightName = r.Field<string>("ArrivalFlightName"),
                        ArrivalFlightNumber = r.Field<string>("ArrivalFlightNumber"),
                        ArrivalDate = r.Field<DateTime>("ArrivalDate"),
                        StringArrivalDate = r.Field<string>("StringArrivalDate"),
                        ArrivalTime = r.Field<TimeSpan?>("ArrivalTime"),
                        ArrivalDateTime = r.Field<DateTime?>("ArrivalDateTime"),
                        AirportDrop = r.Field<string>("AirportDrop"),
                        DepartureFlightName = r.Field<string>("DepartureFlightName"),
                        DepartureFlightNumber = r.Field<string>("DepartureFlightNumber"),
                        DepartureDate = r.Field<DateTime>("DepartureDate"),
                        StringDepartureDate = r.Field<string>("StringDepartureDate"),
                        DepartureTime = r.Field<TimeSpan?>("DepartureTime"),
                        DepartureDateTime = r.Field<DateTime?>("DepartureDateTime"),
                        RoomOfNights = r.Field<int?>("RoomOfNights"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        Remarks = r.Field<string>("Remarks"),
                        APDId = r.Field<int?>("APDId"),
                        ReservationMode = r.Field<string>("ReservationMode"),
                        IsRoomRateShowInPreRegistrationCard = r.Field<Boolean>("IsRoomRateShowInPreRegistrationCard"),
                        IsOtherChargeEnabled = r.Field<int>("IsOtherChargeEnabled")
                    }).ToList();
                }

                return roomReservationList;
            }
        }
        public List<RoomReservationInfoReportBO> GetNoShowRoomReservationInfoByDateRange(DateTime? fromDate, DateTime? toDate)
        {
            List<RoomReservationInfoReportBO> noShowReservation = new List<RoomReservationInfoReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNoShowRoomReservationInfoByDateRange_SP"))
                {
                    if (fromDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);
                    }

                    if (toDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RoomReservationInfoReportBO");
                    DataTable Table = ds.Tables["RoomReservationInfoReportBO"];

                    noShowReservation = Table.AsEnumerable().Select(r => new RoomReservationInfoReportBO
                    {
                        FromDate = r.Field<DateTime?>("FromDate"),
                        ToDate = r.Field<DateTime?>("ToDate"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        BookingDate = r.Field<string>("BookingDate"),
                        CheckIn = r.Field<string>("CheckIn"),
                        ProbableCheckInTime = r.Field<string>("ProbableCheckInTime"),
                        DateOut = r.Field<string>("DateOut"),
                        GuestName = r.Field<string>("GuestName"),
                        GuestList = r.Field<string>("GuestList"),
                        TotalPerson = r.Field<int?>("TotalPerson"),
                        ReservedBy = r.Field<string>("ReservedBy"),
                        CompanyORPrivate = r.Field<string>("CompanyORPrivate"),
                        RoomType = r.Field<string>("RoomType"),
                        NoShowQuantity = r.Field<int>("NoShowQuantity"),
                        ReservedRoom = r.Field<string>("ReservedRoom"),
                        NoShowRoomList = r.Field<string>("NoShowRoomList"),
                        RoomRate = r.Field<decimal?>("RoomRate"),
                        CurrencyTypeId = r.Field<int?>("CurrencyTypeId"),
                        CurrencyType = r.Field<string>("CurrencyType"),
                        PaymentMode = r.Field<string>("PaymentMode"),
                        GuestReferance = r.Field<string>("GuestReferance"),
                        ReservationMode = r.Field<string>("ReservationMode"),
                        Reason = r.Field<string>("Reason")
                    }).ToList();
                }
            }
            return noShowReservation;
        }
        public List<ReservationBillTransactionReportBO> GetReservationBillTransaction(string reservationNumber, string paymentMode, string paymentType, DateTime fromDate, DateTime toDate, string receivedBy)
        {
            List<ReservationBillTransactionReportBO> billTransaction = new List<ReservationBillTransactionReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservationBillTransactionReport_SP"))
                {
                    if (!string.IsNullOrEmpty(reservationNumber))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, reservationNumber);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(paymentMode))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PaymentMode", DbType.String, paymentMode);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PaymentMode", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(paymentType))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PaymentType", DbType.String, paymentType);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PaymentType", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);

                    if (!string.IsNullOrEmpty(receivedBy))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReceivedBy", DbType.Int32, Convert.ToInt32(receivedBy));
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReceivedBy", DbType.Int32, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ReservationBill");
                    DataTable Table = ds.Tables["ReservationBill"];

                    billTransaction = Table.AsEnumerable().Select(r => new ReservationBillTransactionReportBO
                    {
                        PaymentDate = r.Field<string>("PaymentDate"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        PaymentMode = r.Field<string>("PaymentMode"),
                        PaymentType = r.Field<string>("PaymentType"),
                        PaymentDescription = r.Field<string>("PaymentDescription"),
                        POSTerminalBank = r.Field<string>("POSTerminalBank"),
                        ReceivedAmount = r.Field<Decimal?>("ReceivedAmount"),
                        PaidAmount = r.Field<Decimal?>("PaidAmount"),
                        OperatedBy = r.Field<string>("OperatedBy")

                    }).ToList();
                }
            }

            return billTransaction;
        }
        public List<RoomReservationBO> GetRoomReservationInfoForNoShow(DateTime? searchDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<RoomReservationBO> roomreservationList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfoForNoShow_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (searchDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SearchDate", DbType.DateTime, searchDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SearchDate", DbType.DateTime, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomreservationDetail = new RoomReservationBO();
                                roomreservationDetail.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomreservationDetail.ReservationDetailId = Convert.ToInt32(reader["ReservationDetailId"]);
                                roomreservationDetail.GuestName = reader["GuestName"].ToString();
                                roomreservationDetail.ReservationNumber = reader["ReservationNumber"].ToString();
                                string reservedate = Convert.ToDateTime(reader["ReservationDate"].ToString()).ToString("dd-MMM-yyyy");
                                roomreservationDetail.ReservationDate = Convert.ToDateTime(reservedate);
                                string dateIn = Convert.ToDateTime(reader["DateIn"].ToString()).ToString("dd-MMM-yyyy");
                                roomreservationDetail.DateIn = Convert.ToDateTime(dateIn);
                                roomreservationDetail.DateIn = Convert.ToDateTime(reader["DateIn"].ToString());
                                string dateOut = Convert.ToDateTime(reader["DateOut"].ToString()).ToString("dd-MMM-yyyy");
                                roomreservationDetail.DateOut = Convert.ToDateTime(dateOut);
                                roomreservationDetail.ReservationNumber = roomreservationDetail.ReservationNumber.ToString();
                                roomreservationDetail.ReservationMode = reader["ReservationMode"].ToString();
                                roomreservationDetail.CompanyName = reader["CompanyName"].ToString();
                                roomreservationDetail.RoomInformation = reader["RoomInformation"].ToString();
                                roomreservationDetail.Status = reader["Status"].ToString();
                                roomreservationList.Add(roomreservationDetail);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return roomreservationList;
        }
        public Boolean UpdateRoomReservationStatus(int reservationId, int detailId, string reservationStatus)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRoomReservationStatus_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ReservationDetailId", DbType.Int32, detailId);
                        dbSmartAspects.AddInParameter(command, "@Status", DbType.String, reservationStatus);
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
        public Boolean CancelRoomReservationNoShow(int reservationId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CancelRoomReservationNoShow_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int32, reservationId);
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
        public Boolean CancelOrActiveRoomReservation(RoomReservationBO reservationBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CancelRoomReservation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int32, reservationBO.ReservationId);
                        dbSmartAspects.AddInParameter(command, "@Reason", DbType.String, reservationBO.Reason);
                        dbSmartAspects.AddInParameter(command, "@ReservationMode", DbType.String, reservationBO.ReservationMode);

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
        public Boolean CheckReservationCanBeCancel(int reservationId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CheckReservationCanBeCancel_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int32, reservationId);
                      
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? false : true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        
        public List<RoomReservationBO> GetNoShowRoomReservationInfo(DateTime? searchDate, int noshowType)
        {
            List<RoomReservationBO> noShowList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNoShowRoomReservations_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchDate", DbType.DateTime, searchDate);
                    dbSmartAspects.AddInParameter(cmd, "@NoShowType", DbType.Int32, noshowType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomReservation = new RoomReservationBO();
                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationNumber = reader["ReservationNumber"].ToString();
                                noShowList.Add(roomReservation);
                            }
                        }
                    }
                }
            }
            return noShowList;
        }
        public Boolean HotelRoomReservationNoShowProcess(DateTime processDate, int createdBy)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("HotelRoomReservationNoShowProcess_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@NoShowProcessDate", DbType.DateTime, processDate);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);
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
        ////------------Online Reservation------------------------------------------------------
        public Boolean SaveOnlineRoomReservationInfo(RoomReservationBO roomReservation, out int tmpReservationId, List<ReservationDetailBO> detailBO, List<ReservationComplementaryItemBO> complementaryItemBOList, out string currentReservationNumber, List<RegistrationServiceInfoBO> paidServiceDetails, bool paidServiceTobeDelete)
        {
            bool retVal = false;
            int status = 0;
            tmpReservationId = 0;
            currentReservationNumber = string.Empty;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveOnlineRoomReservationInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@OnlineReservationId", DbType.Int32, roomReservation.OnlineReservationId);
                            dbSmartAspects.AddInParameter(commandMaster, "@DateIn", DbType.DateTime, roomReservation.DateIn);
                            dbSmartAspects.AddInParameter(commandMaster, "@DateOut", DbType.DateTime, roomReservation.DateOut);
                            dbSmartAspects.AddInParameter(commandMaster, "@ConfirmationDate", DbType.DateTime, roomReservation.ConfirmationDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservedCompany", DbType.String, roomReservation.ReservedCompany);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactAddress", DbType.String, roomReservation.ContactAddress);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, roomReservation.ContactPerson);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactNumber", DbType.String, roomReservation.ContactNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@MobileNumber", DbType.String, roomReservation.MobileNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@FaxNumber", DbType.String, roomReservation.FaxNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ContactEmail", DbType.String, roomReservation.ContactEmail);
                            dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, roomReservation.GuestId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Reason", DbType.String, roomReservation.Reason);
                            dbSmartAspects.AddInParameter(commandMaster, "@BusinessPromotionId", DbType.Int32, roomReservation.BusinessPromotionId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReferenceId", DbType.Int32, roomReservation.ReferenceId);
                            dbSmartAspects.AddInParameter(commandMaster, "@TotalRoomNumber", DbType.Int32, roomReservation.TotalRoomNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservedMode", DbType.String, roomReservation.ReservedMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationType", DbType.String, roomReservation.ReservationType);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationMode", DbType.String, roomReservation.ReservationMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsListedCompany", DbType.Boolean, roomReservation.IsListedCompany);
                            dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonAdult", DbType.Int32, roomReservation.NumberOfPersonAdult);
                            dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonChild", DbType.Int32, roomReservation.NumberOfPersonChild);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsFamilyOrCouple", DbType.Boolean, roomReservation.IsFamilyOrCouple);
                            dbSmartAspects.AddInParameter(commandMaster, "@CurrencyType", DbType.Int32, roomReservation.CurrencyType);
                            dbSmartAspects.AddInParameter(commandMaster, "@ConversionRate", DbType.Decimal, roomReservation.ConversionRate);
                            dbSmartAspects.AddInParameter(commandMaster, "@PaymentMode", DbType.String, roomReservation.PaymentMode);
                            dbSmartAspects.AddInParameter(commandMaster, "@PayFor", DbType.Int32, roomReservation.PayFor);
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, roomReservation.CompanyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, roomReservation.Remarks);
                            //--Aireport Pickup Drop Information-------------------------------------------
                            dbSmartAspects.AddInParameter(commandMaster, "@AirportPickUp", DbType.String, roomReservation.AirportPickUp);
                            dbSmartAspects.AddInParameter(commandMaster, "@AirportDrop", DbType.String, roomReservation.AirportDrop);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsAirportPickupDropExist", DbType.Int32, roomReservation.IsAirportPickupDropExist);
                            //--Aireport Pickup Drop Information------------------------------End--------
                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, roomReservation.CreatedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@tempResId", DbType.Int32, roomReservation.ReservationTempId);
                            dbSmartAspects.AddOutParameter(commandMaster, "@ReservationId", DbType.Int32, sizeof(Int32));
                            dbSmartAspects.AddOutParameter(commandMaster, "@ReservationNumber", DbType.String, 50);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                            tmpReservationId = Convert.ToInt32(commandMaster.Parameters["@ReservationId"].Value);
                            currentReservationNumber = (commandMaster.Parameters["@ReservationNumber"].Value).ToString();
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveOnlineReservationDetailInfo_SP"))
                            {
                                foreach (ReservationDetailBO reservationDetailBO in detailBO)
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int32, tmpReservationId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomTypeId", DbType.Int32, reservationDetailBO.RoomTypeId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomId", DbType.Int32, reservationDetailBO.RoomId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RoomRate", DbType.Int32, reservationDetailBO.RoomRate);
                                    dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Int32, reservationDetailBO.UnitPrice);
                                    dbSmartAspects.AddInParameter(commandDetails, "@DiscountAmount", DbType.Decimal, reservationDetailBO.DiscountAmount);
                                    dbSmartAspects.AddInParameter(commandDetails, "@DiscountType", DbType.String, reservationDetailBO.DiscountType);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }

                            //if (count == detailBO.Count)
                            if (complementaryItemBOList != null && status > 0)
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveReservationComplementaryItemInfo_SP"))
                                {
                                    foreach (ReservationComplementaryItemBO complementaryItemBO in complementaryItemBOList)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int32, tmpReservationId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ComplementaryItemId", DbType.Int32, complementaryItemBO.ComplementaryItemId);
                                        status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            //-------------Paid Service Save, Update, Delete ----------------------------

                            if (paidServiceTobeDelete && status > 0)
                            {
                                using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteHotelReservationPaidService_SP"))
                                {
                                    commandDelete.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDelete, "@ReservationId", DbType.Int32, tmpReservationId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDelete);
                                }
                            }

                            if (status > 0)
                            {
                                using (DbCommand commandPaidSave = dbSmartAspects.GetStoredProcCommand("SaveReservationServiceInfo_SP"))
                                {
                                    if (paidServiceDetails != null)
                                    {
                                        if (paidServiceDetails.Count > 0)
                                        {
                                            foreach (RegistrationServiceInfoBO comItemBO in paidServiceDetails)
                                            {
                                                commandPaidSave.Parameters.Clear();

                                                dbSmartAspects.AddInParameter(commandPaidSave, "@ReservationId", DbType.Int32, tmpReservationId);
                                                dbSmartAspects.AddInParameter(commandPaidSave, "@ServiceId", DbType.Int32, comItemBO.ServiceId);

                                                if (roomReservation.IsListedCompany == true)
                                                {
                                                    dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, 0);
                                                }
                                                else
                                                {
                                                    if (roomReservation.CurrencyType == 45)
                                                    {
                                                        dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice);
                                                    }
                                                    else
                                                    {
                                                        dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice * roomReservation.ConversionRate);
                                                    }
                                                }

                                                dbSmartAspects.AddInParameter(commandPaidSave, "@IsAchieved", DbType.Boolean, comItemBO.IsAchieved);
                                                dbSmartAspects.AddOutParameter(commandPaidSave, "@DetailServiceId", DbType.Int32, sizeof(Int32));
                                                status = dbSmartAspects.ExecuteNonQuery(commandPaidSave);
                                            }
                                        }
                                    }
                                }
                            }
                            //-------------** End Region -- Paid Service Save, Update, Delete ----------------------------
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
        public List<RoomReservationBO> GetExpressCheckInInformationByReservationId(int reservationId)
        {
            List<RoomReservationBO> roomReservationList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetExpressCheckInInformationByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomReservation = new RoomReservationBO();
                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationDetailId = Convert.ToInt32(reader["ReservationDetailId"]);
                                roomReservation.GuestId = Convert.ToInt32(reader["GuestId"]);
                                roomReservation.GuestName = reader["GuestName"].ToString();
                                roomReservation.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomReservation.RoomTypeCode = reader["RoomTypeCode"].ToString();
                                roomReservation.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomReservation.RoomNumber = reader["RoomNumber"].ToString();
                                roomReservation.DiscountType = reader["DiscountType"].ToString();
                                roomReservation.DiscountAmount = Convert.ToDecimal(reader["Amount"]);
                                roomReservation.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                roomReservation.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                roomReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                                roomReservation.ReservationNDetailNRoomId = reader["ReservationId"].ToString() + "~" + reader["ReservationDetailId"].ToString() + "~" + reader["RoomId"].ToString();
                                roomReservation.CurrencyType = Convert.ToInt32(reader["CurrencyType"]);
                                roomReservation.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                roomReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                roomReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);
                                roomReservation.IsRegistered = Convert.ToInt32(reader["IsRegistered"]);
                                roomReservation.PaxQuantity = Convert.ToInt32(reader["PaxQuantity"]);
                                roomReservation.TotalPaxQuantity = Convert.ToInt32(reader["TotalPaxQuantity"]);
                                roomReservationList.Add(roomReservation);
                            }
                        }
                    }
                }
            }
            return roomReservationList;
        }
        public List<RoomReservationBO> GetTypeWisePaxQuantityByReservationId(int reservationId)
        {
            List<RoomReservationBO> roomReservationList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTypeWisePaxQuantityByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomReservation = new RoomReservationBO();
                                roomReservation.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomReservation.RoomTypeCode = reader["RoomTypeCode"].ToString();
                                roomReservation.PaxQuantity = Convert.ToInt32(reader["PaxQuantity"]);
                                roomReservation.TotalPaxQuantity = Convert.ToInt32(reader["TotalPaxQuantity"]);
                                roomReservation.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                roomReservation.DiscountType = reader["DiscountType"].ToString();
                                roomReservation.DiscountAmount = Convert.ToDecimal(reader["Amount"]);
                                roomReservation.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                roomReservation.TypeWiseRoomQuantity = Convert.ToInt32(reader["RoomQuantity"]);
                                roomReservationList.Add(roomReservation);
                            }
                        }
                    }
                }
            }
            return roomReservationList;
        }
        public List<RoomReservationBO> GetRoomReservationInformationForRoomAssignment(string reservationNumber)
        {
            List<RoomReservationBO> roomReservationList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInformationForRoomAssignment_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, reservationNumber);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomReservation = new RoomReservationBO();
                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomReservation.ReservationDetailId = Convert.ToInt32(reader["ReservationDetailId"]);
                                roomReservation.GuestId = Convert.ToInt32(reader["GuestId"]);
                                roomReservation.GuestName = reader["GuestName"].ToString();
                                roomReservation.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomReservation.RoomTypeCode = reader["RoomTypeCode"].ToString();
                                roomReservation.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomReservation.RoomNumber = reader["RoomNumber"].ToString();
                                roomReservation.DiscountType = reader["DiscountType"].ToString();
                                roomReservation.DiscountAmount = Convert.ToDecimal(reader["Amount"]);
                                roomReservation.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                roomReservation.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                roomReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                                roomReservation.ReservationNDetailNRoomId = reader["ReservationId"].ToString() + "~" + reader["ReservationDetailId"].ToString() + "~" + reader["RoomId"].ToString();
                                roomReservation.CurrencyType = Convert.ToInt32(reader["CurrencyType"]);
                                roomReservation.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                roomReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                roomReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);
                                roomReservation.IsRegistered = Convert.ToInt32(reader["IsRegistered"]);
                                roomReservation.PaxQuantity = Convert.ToInt32(reader["PaxQuantity"]);
                                roomReservation.RoomType = Convert.ToString(reader["RoomType"]);
                                roomReservation.TypeWiseRoomQuantity = Convert.ToInt32(reader["RoomQuantity"]);
                                roomReservation.Remarks = reader["Remarks"].ToString();
                                roomReservation.GuestRemarks = reader["GuestRemarks"].ToString();
                                roomReservation.POSRemarks = reader["POSRemarks"].ToString();
                                roomReservationList.Add(roomReservation);
                            }
                        }
                    }
                }
            }
            return roomReservationList;
        }
        public List<RoomReservationBO> GetExpressCheckedInnInformationByReservationId(int reservationId)
        {
            List<RoomReservationBO> roomReservationList = new List<RoomReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetExpressCheckedInnInformationByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomReservationBO roomReservation = new RoomReservationBO();
                                roomReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomReservation.ReservationDetailId = Convert.ToInt32(reader["ReservationDetailId"]);
                                roomReservation.GuestName = reader["GuestName"].ToString();
                                roomReservation.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);
                                roomReservation.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomReservation.RoomNumber = reader["RoomNumber"].ToString();
                                roomReservation.DiscountType = reader["DiscountType"].ToString();
                                roomReservation.DiscountAmount = Convert.ToDecimal(reader["Amount"]);
                                roomReservation.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                roomReservation.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                roomReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                                roomReservation.ReservationNDetailNRoomId = reader["ReservationId"].ToString() + "~" + reader["ReservationDetailId"].ToString() + "~" + reader["RoomId"].ToString();
                                roomReservation.CurrencyType = Convert.ToInt32(reader["CurrencyType"]);
                                roomReservation.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                roomReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                roomReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);
                                roomReservation.DateInString = reader["DateIn"].ToString();
                                roomReservation.DateOutString = reader["DateOut"].ToString();
                                roomReservation.IsRegistered = Convert.ToInt32(reader["IsRegistered"]);
                                roomReservationList.Add(roomReservation);
                            }
                        }
                    }
                }
            }
            return roomReservationList;
        }
        public Boolean HotelGuestReservationDataCleanForRoomAssignment(int reservationId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("HotelGuestReservationDataCleanForRoomAssignment_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int32, reservationId);
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
        public Boolean UpdateReservationDetailForRoomAssignment(RoomReservationBO roomReservationDetailBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateReservationDetailForRoomAssignment_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int32, roomReservationDetailBO.ReservationId);
                        dbSmartAspects.AddInParameter(command, "@ReservationDetailId", DbType.Int32, roomReservationDetailBO.ReservationDetailId);
                        dbSmartAspects.AddInParameter(command, "@RoomTypeId", DbType.Int32, roomReservationDetailBO.RoomTypeId);
                        dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, roomReservationDetailBO.RoomId);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, roomReservationDetailBO.CreatedBy);
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
        public Boolean UpdateReservationRoomInfoByReservationId(RoomReservationBO roomReservationDetailBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateReservationRoomInfoByReservationId_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int32, roomReservationDetailBO.ReservationId);
                        dbSmartAspects.AddInParameter(command, "@RoomInfo", DbType.String, roomReservationDetailBO.RoomInfo);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, roomReservationDetailBO.CreatedBy);
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
        public Boolean UpdateReservationStatusByReservationId(int reservationId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateReservationStatusByReservationId_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int32, reservationId);
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
        public Boolean UpdateBlankRegistrationInfoByReservationId(int reservationId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBlankRegistrationInfoByReservationId_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int32, reservationId);
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
        public Boolean TransferReservationBillPaymentInfo(List<RoomReservationBO> roomReservationPaymentTransferList, int fromReservationId, int transferReservationId, int userInfoId)
        {
            Boolean status = false;
            int transactionCount = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("TransferReservationBillPaymentInfo_SP"))
                    {
                        foreach (RoomReservationBO roomReservationPaymentTransfer in roomReservationPaymentTransferList)
                        {
                            cmd.Parameters.Clear();

                            dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int32, roomReservationPaymentTransfer.PaymentId);
                            dbSmartAspects.AddInParameter(cmd, "@PaymentAmount", DbType.Decimal, roomReservationPaymentTransfer.PaymentAmount);
                            dbSmartAspects.AddInParameter(cmd, "@Remarks", DbType.String, roomReservationPaymentTransfer.Remarks);
                            dbSmartAspects.AddInParameter(cmd, "@FromReservationId", DbType.Int32, fromReservationId);
                            dbSmartAspects.AddInParameter(cmd, "@TransferReservationId", DbType.Int32, transferReservationId);
                            dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, userInfoId);

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                            if (status)
                            {
                                transactionCount = transactionCount + 1;
                            }
                        }
                    }


                    if (roomReservationPaymentTransferList.Count == transactionCount)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
            }

            return status;
        }
        public bool SaveGroupRoomReservation(int companyId, string groupName, DateTime reservationDate, string reservationDetails, List<RoomReservationBO> groupRoomReservationList, int userInfoId, out long groupMasterId)
        {
            Boolean status = false;
            groupMasterId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGroupRoomReservationInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@ReservationDate", DbType.DateTime, reservationDate);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, companyId);
                            dbSmartAspects.AddInParameter(command, "@GroupName", DbType.String, groupName);
                            dbSmartAspects.AddInParameter(command, "@ReservationDetails", DbType.String, reservationDetails);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, userInfoId);
                            dbSmartAspects.AddOutParameter(command, "@GroupMasterId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            groupMasterId = Convert.ToInt64(command.Parameters["@GroupMasterId"].Value);
                        }                        

                        if (status)
                        {
                            if (groupRoomReservationList != null)
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveGroupRoomReservationDetailInfo_SP"))
                                {
                                    foreach (RoomReservationBO groupRoomReservationBO in groupRoomReservationList)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@GroupMasterId", DbType.Int64, groupMasterId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int64, groupRoomReservationBO.ReservationId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@CreatedBy", DbType.Int32, userInfoId);
                                        status = dbSmartAspects.ExecuteNonQuery(commandDetails, transaction) > 0 ? true : false;
                                    }
                                }
                            }
                        }
                        
                        if (status)
                            transaction.Commit();
                        else
                            transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status;
        }
        public bool UpdateGroupRoomReservation(long groupMasterId, int companyId, string groupName, DateTime reservationDate, string reservationDetails, List<RoomReservationBO> groupRoomReservationList, int userInfoId)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGroupRoomReservationInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@GroupMasterId", DbType.Int64, groupMasterId);
                            dbSmartAspects.AddInParameter(command, "@ReservationDate", DbType.DateTime, reservationDate);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, companyId);
                            dbSmartAspects.AddInParameter(command, "@GroupName", DbType.String, groupName);
                            dbSmartAspects.AddInParameter(command, "@ReservationDetails", DbType.String, reservationDetails);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, userInfoId);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }

                        if (status)
                        {
                            if (groupRoomReservationList != null)
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveGroupRoomReservationDetailInfo_SP"))
                                {
                                    foreach (RoomReservationBO groupRoomReservationBO in groupRoomReservationList)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@GroupMasterId", DbType.Int64, groupMasterId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int64, groupRoomReservationBO.ReservationId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@CreatedBy", DbType.Int32, userInfoId);
                                        status = dbSmartAspects.ExecuteNonQuery(commandDetails, transaction) > 0 ? true : false;
                                    }
                                }
                            }
                        }

                        if (status)
                            transaction.Commit();
                        else
                            transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status;
        }
        public List<RoomReservationBillBO> GetGroupRoomReservationBillInfoById(int transactionId)
        {
            List<RoomReservationBillBO> roomReservationList = new List<RoomReservationBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGroupRoomReservationBillInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.Int32, transactionId);

                    DataSet reservationDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, reservationDS, "Reservation");
                    DataTable Tabel = reservationDS.Tables["Reservation"];

                    roomReservationList = Tabel.AsEnumerable().Select(r => new RoomReservationBillBO
                    {
                        GroupReservationNumber = r.Field<string>("GroupReservationNumber"),
                        CompanyName = r.Field<string>("CompanyName"),
                        CompanyAddress = r.Field<string>("CompanyAddress"),
                        WebAddress = r.Field<string>("WebAddress"),
                        CompanyDetails = r.Field<string>("CompanyDetails"),
                        GroupName = r.Field<string>("GroupName"),
                        BillingAddress = r.Field<string>("BillingAddress"),
                        GroupDescription = r.Field<string>("GroupDescription"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        GuestName = r.Field<string>("GuestName"),
                        TotalNumberOfRooms = r.Field<decimal?>("TotalNumberOfRooms"),
                        RoomType = r.Field<string>("RoomType"),
                        TypeWiseTotalRooms = r.Field<int?>("TypeWiseTotalRooms"),
                        RoomRate = r.Field<decimal?>("RoomRate"),
                        LocalCurrencyType = r.Field<string>("LocalCurrencyType"),
                        CurrencyTypeId = r.Field<int?>("CurrencyTypeId"),
                        CurrencyType = r.Field<string>("CurrencyType"),
                        ConversionRate = r.Field<decimal>("ConversionRate"),
                        CheckInDateDisplay = r.Field<string>("CheckInDateDisplay"),
                        CheckOutDateDisplay = r.Field<string>("CheckOutDateDisplay"),
                        ModeOfPayment = r.Field<string>("ModeOfPayment"),
                        IsOtherChargeEnabled = r.Field<int>("IsOtherChargeEnabled"),
                        CreatedDateDisplay = r.Field<string>("CreatedDateDisplay")
                    }).ToList();
                }

                return roomReservationList;
            }
        }
        public RoomReservationBO GetGroupRoomReservationInfoById(long transactionId)
        {
            RoomReservationBO roomReservation = new RoomReservationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGroupRoomReservationInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.Int32, transactionId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomReservation.Id = Convert.ToInt64(reader["Id"]);
                                roomReservation.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"]);
                                //roomReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                //roomReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);
                                roomReservation.GroupDescription = reader["GroupDescription"].ToString();
                            }
                        }
                    }
                }
            }
            return roomReservation;
        }
        public Boolean CancelGroupRoomReservation(long reservationId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CancelGroupRoomReservation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, reservationId);

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
        public List<RoomReservationBillBO> GetGroupRoomReservationInfo()
        {
            List<RoomReservationBillBO> roomReservationList = new List<RoomReservationBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGroupRoomReservationInfo_SP"))
                {
                    DataSet reservationDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, reservationDS, "Reservation");
                    DataTable Tabel = reservationDS.Tables["Reservation"];

                    roomReservationList = Tabel.AsEnumerable().Select(r => new RoomReservationBillBO
                    {
                        GroupMasterId = r.Field<long>("Id"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        ReservationDateDisplay = r.Field<string>("ReservationDateDisplay"),
                        GroupName = r.Field<string>("GroupName"),
                        CheckInDateDisplay = r.Field<string>("CheckInDateDisplay"),
                        CheckOutDateDisplay = r.Field<string>("CheckOutDateDisplay"),
                        ReservationDetails = r.Field<string>("ReservationDetails"),
                        GroupDescription = r.Field<string>("GroupDescription")
                    }).ToList();
                }

                return roomReservationList;
            }
        }
    }
}
