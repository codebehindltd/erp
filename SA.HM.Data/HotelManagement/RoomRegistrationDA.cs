using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace HotelManagement.Data.HotelManagement
{
    public class RoomRegistrationDA : BaseService
    {
        public List<RoomRegistrationBO> GetRoomRegistrationInfo()
        {
            List<RoomRegistrationBO> roomRegistrationList = new List<RoomRegistrationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomRegistrationInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();

                                roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                roomRegistration.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                roomRegistration.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomRegistration.EntitleRoomType = Convert.ToInt32(reader["EntitleRoomType"]);
                                roomRegistration.IsCompanyGuest = Convert.ToBoolean(reader["IsCompanyGuest"]);
                                roomRegistration.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                roomRegistration.CommingFrom = reader["CommingFrom"].ToString();
                                roomRegistration.NextDestination = reader["NextDestination"].ToString();
                                roomRegistration.VisitPurpose = reader["VisitPurpose"].ToString();
                                roomRegistration.IsFromReservation = Convert.ToBoolean(reader["IsFromReservation"]);
                                roomRegistration.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomRegistration.BusinessPromotionId = Convert.ToInt32(reader["BusinessPromotionId"]);

                                roomRegistrationList.Add(roomRegistration);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public List<RoomRegistrationBO> GetActiveRoomRegistrationInfo()
        {
            List<RoomRegistrationBO> roomRegistrationList = new List<RoomRegistrationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveRoomRegistrationInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
                                roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomRegistrationList.Add(roomRegistration);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public List<RoomRegistrationBO> GetActiveRoomRegistrationWithRoomType()
        {
            List<RoomRegistrationBO> roomRegistrationList = new List<RoomRegistrationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveRoomRegistrationWithRoomType_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();

                                roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                //roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                roomRegistration.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                roomRegistration.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomRegistration.RoomTypeId = Convert.ToInt32(reader["RoomTypeId"]);

                                roomRegistrationList.Add(roomRegistration);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public List<RoomRegistrationBO> GetActiveRoomRegistrationInfoByTransactionDate(DateTime transactionDate)
        {
            List<RoomRegistrationBO> roomRegistrationList = new List<RoomRegistrationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveRoomRegistrationInfoByTransactionDate_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TransactionDate", DbType.DateTime, transactionDate);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();

                                roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                roomRegistration.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                roomRegistration.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomRegistration.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                roomRegistration.GuestName = reader["GuestName"].ToString();
                                roomRegistration.IsCompanyGuest = Convert.ToBoolean(reader["IsCompanyGuest"]);
                                roomRegistration.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                roomRegistration.CommingFrom = reader["CommingFrom"].ToString();
                                roomRegistration.NextDestination = reader["NextDestination"].ToString();
                                roomRegistration.VisitPurpose = reader["VisitPurpose"].ToString();
                                roomRegistration.IsFromReservation = Convert.ToBoolean(reader["IsFromReservation"]);
                                roomRegistration.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomRegistration.IsGuestCheckedOut = Convert.ToInt32(reader["IsGuestCheckedOut"]);

                                roomRegistrationList.Add(roomRegistration);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public List<RoomRegistrationBO> GetCheckOutRoomRegistrationInfoByCheckOutDate(DateTime checkOutDate)
        {
            List<RoomRegistrationBO> roomRegistrationList = new List<RoomRegistrationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCheckOutRoomRegistrationInfoByCheckOutDate_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CheckOutDate", DbType.DateTime, checkOutDate);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();

                                roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                roomRegistration.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                roomRegistration.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomRegistration.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                roomRegistration.GuestName = reader["GuestName"].ToString();
                                roomRegistration.IsCompanyGuest = Convert.ToBoolean(reader["IsCompanyGuest"]);
                                roomRegistration.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                roomRegistration.CommingFrom = reader["CommingFrom"].ToString();
                                roomRegistration.NextDestination = reader["NextDestination"].ToString();
                                roomRegistration.VisitPurpose = reader["VisitPurpose"].ToString();
                                roomRegistration.IsFromReservation = Convert.ToBoolean(reader["IsFromReservation"]);
                                roomRegistration.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomRegistration.IsGuestCheckedOut = Convert.ToInt32(reader["IsGuestCheckedOut"]);

                                roomRegistration.BillPaidByRegistrationId = Convert.ToInt32(reader["BillPaidByRegistrationId"]);
                                roomRegistration.BillPaidByRegistrationNumber = reader["BillPaidByRegistrationNumber"].ToString();
                                roomRegistration.BillPaidByRoomNumber = reader["BillPaidByRoomNumber"].ToString();

                                roomRegistrationList.Add(roomRegistration);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public List<RoomRegistrationBO> GetRoomRegistrationInfoByRegistrationIdList(string registrationIdList)
        {
            List<RoomRegistrationBO> roomRegistrationList = new List<RoomRegistrationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomRegistrationInfoByRegistrationIdList_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationIdList);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();

                                roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                roomRegistration.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                roomRegistration.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomRegistration.EntitleRoomType = Convert.ToInt32(reader["EntitleRoomType"]);
                                roomRegistration.IsCompanyGuest = Convert.ToBoolean(reader["IsCompanyGuest"]);
                                roomRegistration.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                roomRegistration.CommingFrom = reader["CommingFrom"].ToString();
                                roomRegistration.NextDestination = reader["NextDestination"].ToString();
                                roomRegistration.VisitPurpose = reader["VisitPurpose"].ToString();
                                roomRegistration.IsFromReservation = Convert.ToBoolean(reader["IsFromReservation"]);
                                roomRegistration.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomRegistration.BusinessPromotionId = Convert.ToInt32(reader["BusinessPromotionId"]);

                                roomRegistrationList.Add(roomRegistration);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public Boolean SaveRoomRegistrationInfo(RoomRegistrationBO roomRegistration, out int tmpRegistrationId, List<GuestInformationBO> detailBO, GuestBillPaymentBO guestBillPaymentBOX, List<RegistrationComplementaryItemBO> complementaryItemBOList, string tempRegId, List<GuestBillPaymentBO> guestPaymentDetailList, List<RegistrationServiceInfoBO> paidServiceDetails, bool paidServiceTobeDelete)
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveRoomRegistrationInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@ArriveDate", DbType.DateTime, roomRegistration.ArriveDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@ExpectedCheckOutDate", DbType.DateTime, roomRegistration.ExpectedCheckOutDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsExpectedCheckOutTimeEnable", DbType.Boolean, roomRegistration.IsExpectedCheckOutTimeEnable);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomId", DbType.Int32, roomRegistration.RoomId);
                        dbSmartAspects.AddInParameter(commandMaster, "@tempRegId", DbType.Int32, roomRegistration.RegistrationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@EntitleRoomType", DbType.Int32, roomRegistration.EntitleRoomType);
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrencyType", DbType.Int32, roomRegistration.CurrencyType);
                        dbSmartAspects.AddInParameter(commandMaster, "@ConversionRate", DbType.Decimal, roomRegistration.ConversionRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@UnitPrice", DbType.Decimal, roomRegistration.UnitPrice);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomRate", DbType.Decimal, roomRegistration.RoomRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsServiceChargeEnable", DbType.Boolean, roomRegistration.IsServiceChargeEnable);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsCityChargeEnable", DbType.Boolean, roomRegistration.IsCityChargeEnable);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsVatAmountEnable", DbType.Boolean, roomRegistration.IsVatAmountEnable);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsAdditionalChargeEnable", DbType.Boolean, roomRegistration.IsAdditionalChargeEnable);
                        dbSmartAspects.AddInParameter(commandMaster, "@DiscountType", DbType.String, roomRegistration.DiscountType);
                        dbSmartAspects.AddInParameter(commandMaster, "@DiscountAmount", DbType.Decimal, roomRegistration.DiscountAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsCompanyGuest", DbType.Boolean, roomRegistration.IsCompanyGuest);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsHouseUseRoom", DbType.Boolean, roomRegistration.IsHouseUseRoom);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsFamilyOrCouple", DbType.Boolean, roomRegistration.IsFamilyOrCouple);
                        dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonAdult", DbType.Int32, roomRegistration.NumberOfPersonAdult);
                        dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonChild", DbType.Int32, roomRegistration.NumberOfPersonChild);
                        dbSmartAspects.AddInParameter(commandMaster, "@CommingFrom", DbType.String, roomRegistration.CommingFrom);
                        dbSmartAspects.AddInParameter(commandMaster, "@NextDestination", DbType.String, roomRegistration.NextDestination);
                        dbSmartAspects.AddInParameter(commandMaster, "@VisitPurpose", DbType.String, roomRegistration.VisitPurpose);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsFromReservation", DbType.Boolean, roomRegistration.IsFromReservation);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int32, roomRegistration.ReservationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsListedCompany", DbType.Boolean, roomRegistration.IsListedCompany);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsListedContact", DbType.Boolean, roomRegistration.IsListedContact);
                        dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, roomRegistration.CompanyId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactId", DbType.Int64, roomRegistration.ContactId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservedCompany", DbType.String, roomRegistration.ReservedCompany);

                        dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, roomRegistration.ContactPerson);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactNumber", DbType.String, roomRegistration.ContactNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@PaymentMode", DbType.String, roomRegistration.PaymentMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@PayFor", DbType.Int32, roomRegistration.PayFor);

                        //----------dbSmartAspects.AddInParameter(commandMaster, "@CompanyPhone", DbType.String, roomRegistration.CompanyPhone);
                        dbSmartAspects.AddInParameter(commandMaster, "@BusinessPromotionId", DbType.Int32, roomRegistration.BusinessPromotionId);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsRoomOwner", DbType.Int32, roomRegistration.IsRoomOwner);
                        dbSmartAspects.AddInParameter(commandMaster, "@MarketSegmentId", DbType.Int32, roomRegistration.MarketSegmentId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestSourceId", DbType.Int32, roomRegistration.GuestSourceId);
                        dbSmartAspects.AddInParameter(commandMaster, "@MealPlanId", DbType.Int32, roomRegistration.MealPlanId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReferenceId", DbType.Int32, roomRegistration.ReferenceId);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsReturnedGuest", DbType.Boolean, roomRegistration.IsReturnedGuest);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsVIPGuest", DbType.Boolean, roomRegistration.IsVIPGuest);
                        dbSmartAspects.AddInParameter(commandMaster, "@VIPGuestTypeId", DbType.Int32, roomRegistration.VIPGuestTypeId);
                        dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, roomRegistration.Remarks);
                        dbSmartAspects.AddInParameter(commandMaster, "@POSRemarks", DbType.String, roomRegistration.POSRemarks);

                        dbSmartAspects.AddInParameter(commandMaster, "@IsEarlyCheckInChargeEnable", DbType.Boolean, roomRegistration.IsEarlyCheckInChargeEnable);
                        //--Aireport Pickup Drop Information-------------------------------------------
                        dbSmartAspects.AddInParameter(commandMaster, "@AirportPickUp", DbType.String, roomRegistration.AirportPickUp);
                        dbSmartAspects.AddInParameter(commandMaster, "@AirportDrop", DbType.String, roomRegistration.AirportDrop);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsAirportPickupDropExist", DbType.Int32, roomRegistration.IsAirportPickupDropExist);
                        dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightName", DbType.String, roomRegistration.ArrivalFlightName);
                        dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightNumber", DbType.String, roomRegistration.ArrivalFlightNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@ArrivalTime", DbType.DateTime, roomRegistration.ArrivalTime);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartureAirlineId", DbType.Int32, roomRegistration.DepartureAirlineId);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightName", DbType.String, roomRegistration.DepartureFlightName);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightNumber", DbType.String, roomRegistration.DepartureFlightNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartureTime", DbType.DateTime, roomRegistration.DepartureTime);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsDepartureChargable", DbType.Boolean, roomRegistration.IsDepartureChargable);
                        //--Aireport Pickup Drop Information------------------------------End--------

                        //--Credit Card Information ------------
                        dbSmartAspects.AddInParameter(commandMaster, "@CardType", DbType.String, roomRegistration.CardType);
                        dbSmartAspects.AddInParameter(commandMaster, "@CardNumber", DbType.String, roomRegistration.CardNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@CardExpireDate", DbType.DateTime, roomRegistration.CardExpireDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@CardHolderName", DbType.String, roomRegistration.CardHolderName);
                        dbSmartAspects.AddInParameter(commandMaster, "@CardReference", DbType.String, roomRegistration.CardReference);
                        //--Credit Card Information End------------

                        //dbSmartAspects.AddInParameter(commandMaster, "@IsEarlyCheckInChargeEnable", DbType.Boolean, roomRegistration.IsEarlyCheckInChargeEnable);

                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationDetailId", DbType.Int64, roomRegistration.ReservationDetailId);

                        dbSmartAspects.AddInParameter(commandMaster, "@PackageId", DbType.Int64, roomRegistration.PackageId);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, roomRegistration.CreatedBy);
                        dbSmartAspects.AddOutParameter(commandMaster, "@RegistrationId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        tmpRegistrationId = Convert.ToInt32(commandMaster.Parameters["@RegistrationId"].Value);
                    }

                    if (status > 0)
                    {
                        using (DbCommand commandComplementary = dbSmartAspects.GetStoredProcCommand("SaveRegistrationComplementaryItemInfo_SP"))
                        {
                            foreach (RegistrationComplementaryItemBO complementaryItemBO in complementaryItemBOList)
                            {
                                commandComplementary.Parameters.Clear();

                                dbSmartAspects.AddInParameter(commandComplementary, "@RegistrationId", DbType.Int32, tmpRegistrationId);
                                dbSmartAspects.AddInParameter(commandComplementary, "@ComplementaryItemId", DbType.Int32, complementaryItemBO.ComplementaryItemId);

                                status = dbSmartAspects.ExecuteNonQuery(commandComplementary, transction);
                            }
                        }

                        if (guestPaymentDetailList != null && status > 0)
                        {
                            GuestBillPaymentDA paymentDA = new GuestBillPaymentDA();

                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                                {
                                    commandGuestBillPayment.Parameters.Clear();

                                    guestBillPaymentBO.CreatedBy = roomRegistration.CreatedBy;
                                    guestBillPaymentBO.PaymentDate = DateTime.Now;

                                    int companyId = 0;
                                    if (guestBillPaymentBO.CompanyId != null)
                                    {
                                        companyId = guestBillPaymentBO.CompanyId;
                                    }

                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "FrontOffice");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, 0);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, 0);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, tmpRegistrationId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, companyId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, string.Empty);

                                    if (guestBillPaymentBO.PaymentMode == "Card")
                                    {
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                    }

                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                    dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));

                                    status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                }

                            }
                        }

                        //-------------Paid Service Save, Update, Delete ----------------------------

                        if (paidServiceTobeDelete && status > 0)
                        {
                            using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteHotelRegistrationPaidService_SP"))
                            {
                                commandDelete.Parameters.Clear();

                                dbSmartAspects.AddInParameter(commandDelete, "@RegistrationId", DbType.Int32, roomRegistration.RegistrationId);
                                status = dbSmartAspects.ExecuteNonQuery(commandDelete, transction);
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandPaidSave = dbSmartAspects.GetStoredProcCommand("SaveRegistrationServiceInfo_SP"))
                            {
                                if (paidServiceDetails != null)
                                {
                                    if (paidServiceDetails.Count > 0)
                                    {
                                        foreach (RegistrationServiceInfoBO comItemBO in paidServiceDetails)
                                        {
                                            commandPaidSave.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandPaidSave, "@RegistrationId", DbType.Int32, tmpRegistrationId);
                                            dbSmartAspects.AddInParameter(commandPaidSave, "@ServiceId", DbType.Int32, comItemBO.ServiceId);

                                            if (roomRegistration.IsCompanyGuest == false)
                                            {
                                                if (roomRegistration.CurrencyType == 1)
                                                {
                                                    dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice);
                                                }
                                                else
                                                {
                                                    dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice * roomRegistration.ConversionRate);
                                                }
                                            }
                                            else
                                            {
                                                dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, 0);
                                            }

                                            dbSmartAspects.AddInParameter(commandPaidSave, "@IsAchieved", DbType.Boolean, comItemBO.IsAchieved);
                                            dbSmartAspects.AddOutParameter(commandPaidSave, "@DetailServiceId", DbType.Int32, sizeof(Int32));

                                            status = dbSmartAspects.ExecuteNonQuery(commandPaidSave, transction);
                                        }
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
                        transction.Rollback();
                        retVal = false;
                    }
                }
            }
            return retVal;
        }
        public Boolean SaveBlankRoomRegistrationInfo(RoomRegistrationBO roomRegistration, out int tmpRegistrationId, List<GuestInformationBO> detailBO, GuestBillPaymentBO guestBillPaymentBOX, List<RegistrationComplementaryItemBO> complementaryItemBOList, string tempRegId, List<GuestBillPaymentBO> guestPaymentDetailList, List<RegistrationServiceInfoBO> paidServiceDetails, bool paidServiceTobeDelete)
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveBlankRoomRegistrationInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@ArriveDate", DbType.DateTime, roomRegistration.ArriveDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@ExpectedCheckOutDate", DbType.DateTime, roomRegistration.ExpectedCheckOutDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomId", DbType.Int32, roomRegistration.RoomId);
                        dbSmartAspects.AddInParameter(commandMaster, "@tempRegId", DbType.Int32, roomRegistration.RegistrationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@EntitleRoomType", DbType.Int32, roomRegistration.EntitleRoomType);
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrencyType", DbType.Int32, roomRegistration.CurrencyType);
                        dbSmartAspects.AddInParameter(commandMaster, "@ConversionRate", DbType.Decimal, roomRegistration.ConversionRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@UnitPrice", DbType.Decimal, roomRegistration.UnitPrice);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomRate", DbType.Decimal, roomRegistration.RoomRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsServiceChargeEnable", DbType.Boolean, roomRegistration.IsServiceChargeEnable);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsVatAmountEnable", DbType.Boolean, roomRegistration.IsVatAmountEnable);
                        dbSmartAspects.AddInParameter(commandMaster, "@DiscountType", DbType.String, roomRegistration.DiscountType);
                        dbSmartAspects.AddInParameter(commandMaster, "@DiscountAmount", DbType.Decimal, roomRegistration.DiscountAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsCompanyGuest", DbType.Boolean, roomRegistration.IsCompanyGuest);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsHouseUseRoom", DbType.Boolean, roomRegistration.IsHouseUseRoom);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsFamilyOrCouple", DbType.Boolean, roomRegistration.IsFamilyOrCouple);
                        dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonAdult", DbType.Int32, roomRegistration.NumberOfPersonAdult);
                        dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonChild", DbType.Int32, roomRegistration.NumberOfPersonChild);
                        dbSmartAspects.AddInParameter(commandMaster, "@CommingFrom", DbType.String, roomRegistration.CommingFrom);
                        dbSmartAspects.AddInParameter(commandMaster, "@NextDestination", DbType.String, roomRegistration.NextDestination);
                        dbSmartAspects.AddInParameter(commandMaster, "@VisitPurpose", DbType.String, roomRegistration.VisitPurpose);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsFromReservation", DbType.Boolean, roomRegistration.IsFromReservation);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int32, roomRegistration.ReservationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsListedCompany", DbType.Boolean, roomRegistration.IsListedCompany);
                        dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, roomRegistration.CompanyId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservedCompany", DbType.String, roomRegistration.ReservedCompany);

                        dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, roomRegistration.ContactPerson);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactNumber", DbType.String, roomRegistration.ContactNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@PaymentMode", DbType.String, roomRegistration.PaymentMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@PayFor", DbType.Int32, roomRegistration.PayFor);

                        //----------dbSmartAspects.AddInParameter(commandMaster, "@CompanyPhone", DbType.String, roomRegistration.CompanyPhone);
                        dbSmartAspects.AddInParameter(commandMaster, "@BusinessPromotionId", DbType.Int32, roomRegistration.BusinessPromotionId);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsRoomOwner", DbType.Int32, roomRegistration.IsRoomOwner);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestSourceId", DbType.Int32, roomRegistration.GuestSourceId);
                        dbSmartAspects.AddInParameter(commandMaster, "@MealPlanId", DbType.Int32, roomRegistration.MealPlanId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReferenceId", DbType.Int32, roomRegistration.ReferenceId);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsReturnedGuest", DbType.Boolean, roomRegistration.IsReturnedGuest);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsVIPGuest", DbType.Boolean, roomRegistration.IsVIPGuest);
                        dbSmartAspects.AddInParameter(commandMaster, "@VIPGuestTypeId", DbType.Int32, roomRegistration.VIPGuestTypeId);
                        dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, roomRegistration.Remarks);

                        //--Aireport Pickup Drop Information-------------------------------------------
                        dbSmartAspects.AddInParameter(commandMaster, "@AirportPickUp", DbType.String, roomRegistration.AirportPickUp);
                        dbSmartAspects.AddInParameter(commandMaster, "@AirportDrop", DbType.String, roomRegistration.AirportDrop);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsAirportPickupDropExist", DbType.Int32, roomRegistration.IsAirportPickupDropExist);
                        dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightName", DbType.String, roomRegistration.ArrivalFlightName);
                        dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightNumber", DbType.String, roomRegistration.ArrivalFlightNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@ArrivalTime", DbType.DateTime, roomRegistration.ArrivalTime);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightName", DbType.String, roomRegistration.DepartureFlightName);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightNumber", DbType.String, roomRegistration.DepartureFlightNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartureTime", DbType.DateTime, roomRegistration.DepartureTime);
                        //--Aireport Pickup Drop Information------------------------------End--------

                        //--Credit Card Information ------------
                        dbSmartAspects.AddInParameter(commandMaster, "@CardType", DbType.String, roomRegistration.CardType);
                        dbSmartAspects.AddInParameter(commandMaster, "@CardNumber", DbType.String, roomRegistration.CardNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@CardExpireDate", DbType.DateTime, roomRegistration.CardExpireDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@CardHolderName", DbType.String, roomRegistration.CardHolderName);
                        dbSmartAspects.AddInParameter(commandMaster, "@CardReference", DbType.String, roomRegistration.CardReference);
                        //--Credit Card Information End------------

                        dbSmartAspects.AddInParameter(commandMaster, "@IsBlankRegistrationCard", DbType.Int32, roomRegistration.IsBlankRegistrationCard);

                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, roomRegistration.CreatedBy);
                        dbSmartAspects.AddOutParameter(commandMaster, "@RegistrationId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        tmpRegistrationId = Convert.ToInt32(commandMaster.Parameters["@RegistrationId"].Value);
                    }

                    if (status > 0)
                    {
                        //using (DbCommand commandComplementary = dbSmartAspects.GetStoredProcCommand("SaveRegistrationComplementaryItemInfo_SP"))
                        //{
                        //    foreach (RegistrationComplementaryItemBO complementaryItemBO in complementaryItemBOList)
                        //    {
                        //        commandComplementary.Parameters.Clear();

                        //        dbSmartAspects.AddInParameter(commandComplementary, "@RegistrationId", DbType.Int32, tmpRegistrationId);
                        //        dbSmartAspects.AddInParameter(commandComplementary, "@ComplementaryItemId", DbType.Int32, complementaryItemBO.ComplementaryItemId);

                        //        status = dbSmartAspects.ExecuteNonQuery(commandComplementary, transction);
                        //    }
                        //}

                        //if (guestPaymentDetailList != null && status > 0)
                        //{
                        //    GuestBillPaymentDA paymentDA = new GuestBillPaymentDA();

                        //    using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                        //    {
                        //        foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                        //        {
                        //            commandGuestBillPayment.Parameters.Clear();

                        //            guestBillPaymentBO.CreatedBy = roomRegistration.CreatedBy;
                        //            guestBillPaymentBO.PaymentDate = DateTime.Now;

                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "FrontOffice");
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, 0);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, tmpRegistrationId);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, guestBillPaymentBO.CompanyId);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                        //            if (guestBillPaymentBO.PaymentMode == "Card")
                        //            {
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                        //            }
                        //            //dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                        //            dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                        //            //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        //            //tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);

                        //            status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);

                        //            //int tmpApprovedId = Convert.ToInt32(commandRoomGuestList.Parameters["@ApprovedId"].Value);
                        //        }

                        //    }
                        //}

                        //-------------Paid Service Save, Update, Delete ----------------------------

                        //if (paidServiceTobeDelete && status > 0)
                        //{
                        //    using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteHotelRegistrationPaidService_SP"))
                        //    {
                        //        commandDelete.Parameters.Clear();

                        //        dbSmartAspects.AddInParameter(commandDelete, "@RegistrationId", DbType.Int32, roomRegistration.RegistrationId);
                        //        status = dbSmartAspects.ExecuteNonQuery(commandDelete);
                        //    }
                        //}

                        //if (status > 0)
                        //{
                        //    using (DbCommand commandPaidSave = dbSmartAspects.GetStoredProcCommand("SaveRegistrationServiceInfo_SP"))
                        //    {
                        //        if (paidServiceDetails != null)
                        //        {
                        //            if (paidServiceDetails.Count > 0)
                        //            {
                        //                foreach (RegistrationServiceInfoBO comItemBO in paidServiceDetails)
                        //                {
                        //                    commandPaidSave.Parameters.Clear();

                        //                    dbSmartAspects.AddInParameter(commandPaidSave, "@RegistrationId", DbType.Int32, tmpRegistrationId);
                        //                    dbSmartAspects.AddInParameter(commandPaidSave, "@ServiceId", DbType.Int32, comItemBO.ServiceId);

                        //                    if (roomRegistration.IsCompanyGuest == false)
                        //                    {
                        //                        if (roomRegistration.CurrencyType == 1)
                        //                        {
                        //                            dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice);
                        //                        }
                        //                        else
                        //                        {
                        //                            dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice * roomRegistration.ConversionRate);
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, 0);
                        //                    }

                        //                    dbSmartAspects.AddInParameter(commandPaidSave, "@IsAchieved", DbType.Boolean, comItemBO.IsAchieved);
                        //                    dbSmartAspects.AddOutParameter(commandPaidSave, "@DetailServiceId", DbType.Int32, sizeof(Int32));

                        //                    status = dbSmartAspects.ExecuteNonQuery(commandPaidSave);
                        //                }
                        //            }
                        //        }
                        //    }

                        //}
                        //-------------** End Region -- Paid Service Save, Update, Delete ----------------------------
                    }

                    if (status > 0)
                    {
                        transction.Commit();
                        retVal = true;
                    }
                    else
                    {
                        transction.Rollback();
                        retVal = false;
                    }
                }
            }
            return retVal;
        }
        public Boolean UpdateRoomRegistrationInfo(RoomRegistrationBO roomRegistration, List<GuestInformationBO> detailBO, ArrayList arrayDelete, GuestBillPaymentBO guestBillPaymentBO, List<RegistrationComplementaryItemBO> newlyAddedcomplementaryItem, List<RegistrationComplementaryItemBO> deletedComplementaryItem, List<RegistrationServiceInfoBO> paidServiceDetails, bool paidServiceTobeDelete, string deletedGuest)
        {
            bool retVal = false;
            int status = 0;
            int tmpRegistrationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateRoomRegistrationInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@RegistrationId", DbType.Int32, roomRegistration.RegistrationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ExpectedCheckOutDate", DbType.DateTime, roomRegistration.ExpectedCheckOutDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsExpectedCheckOutTimeEnable", DbType.Boolean, roomRegistration.IsExpectedCheckOutTimeEnable);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomId", DbType.Int32, roomRegistration.RoomId);
                        dbSmartAspects.AddInParameter(commandMaster, "@tempRegId", DbType.Int32, roomRegistration.RegistrationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@EntitleRoomType", DbType.Int32, roomRegistration.EntitleRoomType);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsCompanyGuest", DbType.Boolean, roomRegistration.IsCompanyGuest);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsHouseUseRoom", DbType.Boolean, roomRegistration.IsHouseUseRoom);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsFamilyOrCouple", DbType.Boolean, roomRegistration.IsFamilyOrCouple);
                        dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonAdult", DbType.Int32, roomRegistration.NumberOfPersonAdult);
                        dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonChild", DbType.Int32, roomRegistration.NumberOfPersonChild);
                        dbSmartAspects.AddInParameter(commandMaster, "@CommingFrom", DbType.String, roomRegistration.CommingFrom);
                        dbSmartAspects.AddInParameter(commandMaster, "@NextDestination", DbType.String, roomRegistration.NextDestination);
                        dbSmartAspects.AddInParameter(commandMaster, "@VisitPurpose", DbType.String, roomRegistration.VisitPurpose);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsFromReservation", DbType.Boolean, roomRegistration.IsFromReservation);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int32, roomRegistration.ReservationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsListedCompany", DbType.Boolean, roomRegistration.IsListedCompany);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsListedContact", DbType.Boolean, roomRegistration.IsListedContact);
                        dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, roomRegistration.CompanyId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactId", DbType.Int64, roomRegistration.ContactId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservedCompany", DbType.String, roomRegistration.ReservedCompany);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, roomRegistration.ContactPerson);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactNumber", DbType.String, roomRegistration.ContactNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@PaymentMode", DbType.String, roomRegistration.PaymentMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@PayFor", DbType.Int32, roomRegistration.PayFor);
                        dbSmartAspects.AddInParameter(commandMaster, "@BusinessPromotionId", DbType.Int32, roomRegistration.BusinessPromotionId);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsRoomOwner", DbType.Int32, roomRegistration.IsRoomOwner);
                        dbSmartAspects.AddInParameter(commandMaster, "@MarketSegmentId", DbType.Int32, roomRegistration.MarketSegmentId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestSourceId", DbType.Int32, roomRegistration.GuestSourceId);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsVIPGuest", DbType.Boolean, roomRegistration.IsVIPGuest);
                        dbSmartAspects.AddInParameter(commandMaster, "@VIPGuestTypeId", DbType.Int32, roomRegistration.VIPGuestTypeId);
                        dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, roomRegistration.Remarks);
                        dbSmartAspects.AddInParameter(commandMaster, "@POSRemarks", DbType.String, roomRegistration.POSRemarks);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsEarlyCheckInChargeEnable", DbType.Boolean, roomRegistration.IsEarlyCheckInChargeEnable);
                        dbSmartAspects.AddInParameter(commandMaster, "@MealPlanId", DbType.Int32, roomRegistration.MealPlanId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReferenceId", DbType.Int32, roomRegistration.ReferenceId);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsReturnedGuest", DbType.Boolean, roomRegistration.IsReturnedGuest);
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrencyType", DbType.Int32, roomRegistration.CurrencyType);
                        dbSmartAspects.AddInParameter(commandMaster, "@ConversionRate", DbType.Decimal, roomRegistration.ConversionRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@UnitPrice", DbType.Decimal, roomRegistration.UnitPrice);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomRate", DbType.Decimal, roomRegistration.RoomRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsServiceChargeEnable", DbType.Boolean, roomRegistration.IsServiceChargeEnable);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsCityChargeEnable", DbType.Boolean, roomRegistration.IsCityChargeEnable);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsVatAmountEnable", DbType.Boolean, roomRegistration.IsVatAmountEnable);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsAdditionalChargeEnable", DbType.Boolean, roomRegistration.IsAdditionalChargeEnable);
                        dbSmartAspects.AddInParameter(commandMaster, "@DiscountType", DbType.String, roomRegistration.DiscountType);
                        dbSmartAspects.AddInParameter(commandMaster, "@DiscountAmount", DbType.Decimal, roomRegistration.DiscountAmount);


                        //--Aireport Pickup Drop Information-------------------------------------------
                        dbSmartAspects.AddInParameter(commandMaster, "@AirportPickUp", DbType.String, roomRegistration.AirportPickUp);
                        dbSmartAspects.AddInParameter(commandMaster, "@AirportDrop", DbType.String, roomRegistration.AirportDrop);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsAirportPickupDropExist", DbType.Int32, roomRegistration.IsAirportPickupDropExist);
                        dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightName", DbType.String, roomRegistration.ArrivalFlightName);
                        dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightNumber", DbType.String, roomRegistration.ArrivalFlightNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@ArrivalTime", DbType.DateTime, roomRegistration.ArrivalTime);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartureAirlineId", DbType.Int32, roomRegistration.DepartureAirlineId);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightName", DbType.String, roomRegistration.DepartureFlightName);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightNumber", DbType.String, roomRegistration.DepartureFlightNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartureTime", DbType.DateTime, roomRegistration.DepartureTime);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsDepartureChargable", DbType.Boolean, roomRegistration.IsDepartureChargable);
                        //--Aireport Pickup Drop Information------------------------------End--------


                        //--Credit Card Information ------------
                        dbSmartAspects.AddInParameter(commandMaster, "@CardType", DbType.String, roomRegistration.CardType);
                        dbSmartAspects.AddInParameter(commandMaster, "@CardNumber", DbType.String, roomRegistration.CardNumber);
                        if (roomRegistration.CardExpireDate == null)
                            dbSmartAspects.AddInParameter(commandMaster, "@CardExpireDate", DbType.DateTime, DBNull.Value);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@CardExpireDate", DbType.DateTime, roomRegistration.CardExpireDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@CardHolderName", DbType.String, roomRegistration.CardHolderName);
                        dbSmartAspects.AddInParameter(commandMaster, "@CardReference", DbType.String, roomRegistration.CardReference);
                        //--Credit Card Information End------------

                        dbSmartAspects.AddInParameter(commandMaster, "@PackageId", DbType.Int64, roomRegistration.PackageId);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, roomRegistration.LastModifiedBy);
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        tmpRegistrationId = roomRegistration.RegistrationId;
                    }

                    if (status > 0)
                    {
                        if (newlyAddedcomplementaryItem.Count() > 0 && status > 0)
                        {
                            using (DbCommand commandComplementary = dbSmartAspects.GetStoredProcCommand("SaveRegistrationComplementaryItemInfo_SP"))
                            {
                                foreach (RegistrationComplementaryItemBO complementaryItemBO in newlyAddedcomplementaryItem)
                                {
                                    commandComplementary.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandComplementary, "@RegistrationId", DbType.Int32, complementaryItemBO.RegistrationId);
                                    dbSmartAspects.AddInParameter(commandComplementary, "@ComplementaryItemId", DbType.Int32, complementaryItemBO.ComplementaryItemId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandComplementary, transction);
                                }
                            }
                        }

                        if (deletedComplementaryItem.Count() > 0 && status > 0)
                        {
                            using (DbCommand commandComplementaryDelete = dbSmartAspects.GetStoredProcCommand("DeleteRegistrationComplementaryItemInfo_SP"))
                            {
                                foreach (RegistrationComplementaryItemBO complementaryItemBO in deletedComplementaryItem)
                                {
                                    commandComplementaryDelete.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandComplementaryDelete, "@RegistrationId", DbType.Int32, complementaryItemBO.RegistrationId);
                                    dbSmartAspects.AddInParameter(commandComplementaryDelete, "@ComplementaryItemId", DbType.Int32, complementaryItemBO.ComplementaryItemId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandComplementaryDelete, transction);
                                }
                            }
                        }

                        //-------------Paid Service Save, Update, Delete ----------------------------
                        if (paidServiceTobeDelete && status > 0)
                        {
                            using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteHotelRegistrationPaidService_SP"))
                            {
                                commandDelete.Parameters.Clear();

                                dbSmartAspects.AddInParameter(commandDelete, "@RegistrationId", DbType.Int32, roomRegistration.RegistrationId);
                                //status = dbSmartAspects.ExecuteNonQuery(commandDelete);
                                status = dbSmartAspects.ExecuteNonQuery(commandDelete, transction);
                            }
                        }

                        if (paidServiceDetails != null && status > 0)
                        {
                            using (DbCommand commandPaidSave = dbSmartAspects.GetStoredProcCommand("SaveRegistrationServiceInfo_SP"))
                            {
                                if (paidServiceDetails.Count > 0)
                                {
                                    foreach (RegistrationServiceInfoBO comItemBO in paidServiceDetails)
                                    {
                                        commandPaidSave.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandPaidSave, "@RegistrationId", DbType.Int32, comItemBO.RegistrationId);
                                        dbSmartAspects.AddInParameter(commandPaidSave, "@ServiceId", DbType.Int32, comItemBO.ServiceId);

                                        if (roomRegistration.IsCompanyGuest == false)
                                        {
                                            if (roomRegistration.CurrencyType == 45)
                                            {
                                                dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice);
                                            }
                                            else
                                            {
                                                dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, comItemBO.UnitPrice * roomRegistration.ConversionRate);
                                            }
                                        }
                                        else
                                        {
                                            dbSmartAspects.AddInParameter(commandPaidSave, "@UnitPrice", DbType.Decimal, 0);
                                        }

                                        dbSmartAspects.AddInParameter(commandPaidSave, "@IsAchieved", DbType.Boolean, comItemBO.IsAchieved);
                                        dbSmartAspects.AddOutParameter(commandPaidSave, "@DetailServiceId", DbType.Int32, sizeof(Int32));

                                        //status = dbSmartAspects.ExecuteNonQuery(commandPaidSave);
                                        status = dbSmartAspects.ExecuteNonQuery(commandPaidSave, transction);
                                    }
                                }
                            }
                        }
                        //-------------** End Region -- Paid Service Save, Update, Delete ----------------------------

                        //-------------** Already Registered Guest Delete --------------------------------------------

                        if (!string.IsNullOrEmpty(deletedGuest) && status > 0)
                        {
                            string[] deleteGuest = deletedGuest.Split('#');
                            string[] deleteId;

                            using (DbCommand commandDeleteGuest = dbSmartAspects.GetStoredProcCommand("DeleteTempGuestRegistrationInfoByGuestId_SP"))
                            {
                                foreach (string g in deleteGuest)
                                {
                                    deleteId = g.Split(',');
                                    commandDeleteGuest.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDeleteGuest, "@RegistrationId", DbType.Int32, deleteId[1]);
                                    dbSmartAspects.AddInParameter(commandDeleteGuest, "@GuestId", DbType.Int32, deleteId[0]);

                                    //status = dbSmartAspects.ExecuteNonQuery(commandDeleteGuest);
                                    dbSmartAspects.ExecuteNonQuery(commandDeleteGuest, transction);
                                }
                            }
                        }
                        //-------------** Already Registered Guest Delete --------------------------------------------                       
                    }

                    if (status > 0)
                    {
                        transction.Commit();
                        retVal = true;
                    }
                    else
                    {
                        transction.Rollback();
                        retVal = false;
                    }
                }

            }
            return retVal;
        }
        public RoomRegistrationBO GetRoomRegistrationInfoById(int registrationId)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomRegistrationInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                roomRegistration.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                roomRegistration.IsExpectedCheckOutTimeEnable = Convert.ToBoolean(reader["IsExpectedCheckOutTimeEnable"]);
                                if (reader["BillingStartDate"] != DBNull.Value)
                                    roomRegistration.BillingStartDate = Convert.ToDateTime(reader["BillingStartDate"]);
                                if (reader["CheckOutDate"] != DBNull.Value)
                                {
                                    roomRegistration.CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"]);
                                    roomRegistration.CheckOutDateForAPI = Convert.ToDateTime(reader["CheckOutDate"]);
                                }

                                if (reader["ActualCheckOutDate"] != DBNull.Value)
                                    roomRegistration.ActualCheckOutDate = Convert.ToDateTime(reader["ActualCheckOutDate"]);

                                roomRegistration.IsGuestCheckedOut = Convert.ToInt32(reader["IsGuestCheckedOut"]);
                                roomRegistration.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomRegistration.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                roomRegistration.EntitleRoomType = Convert.ToInt32(reader["EntitleRoomType"]);
                                roomRegistration.CurrencyType = Convert.ToInt32(reader["CurrencyType"]);
                                roomRegistration.Currency = Convert.ToString(reader["Currency"]);
                                roomRegistration.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                roomRegistration.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                roomRegistration.DiscountType = reader["DiscountType"].ToString();
                                roomRegistration.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                roomRegistration.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                roomRegistration.IsServiceChargeEnable = Convert.ToBoolean(reader["IsServiceChargeEnable"]);
                                roomRegistration.IsCityChargeEnable = Convert.ToBoolean(reader["IsCityChargeEnable"]);
                                roomRegistration.IsVatAmountEnable = Convert.ToBoolean(reader["IsVatAmountEnable"]);
                                roomRegistration.IsAdditionalChargeEnable = Convert.ToBoolean(reader["IsAdditionalChargeEnable"]);
                                roomRegistration.TotalRoomRate = Convert.ToDecimal(reader["TotalRoomRate"]);
                                roomRegistration.IsCompanyGuest = Convert.ToBoolean(reader["IsCompanyGuest"]);
                                roomRegistration.IsHouseUseRoom = Convert.ToBoolean(reader["IsHouseUseRoom"]);
                                roomRegistration.CommingFrom = reader["CommingFrom"].ToString();
                                roomRegistration.NextDestination = reader["NextDestination"].ToString();
                                roomRegistration.VisitPurpose = reader["VisitPurpose"].ToString();
                                roomRegistration.IsFromReservation = Convert.ToBoolean(reader["IsFromReservation"]);
                                roomRegistration.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomRegistration.ReservationInfo = reader["ReservationInfo"].ToString();
                                roomRegistration.IsFamilyOrCouple = Convert.ToBoolean(reader["IsFamilyOrCouple"]);
                                roomRegistration.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                roomRegistration.IsListedCompany = Convert.ToBoolean(reader["IsListedCompany"]);
                                roomRegistration.IsListedContact = Convert.ToBoolean(reader["IsListedContact"]);
                                roomRegistration.ReservedCompany = reader["ReservedCompany"].ToString();
                                roomRegistration.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                roomRegistration.ContactId = Convert.ToInt64(reader["ContactId"]);
                                roomRegistration.CompanyName = reader["CompanyName"].ToString();
                                roomRegistration.ContactPerson = reader["ContactPerson"].ToString();
                                roomRegistration.ContactNumber = reader["ContactNumber"].ToString();
                                roomRegistration.PaymentMode = reader["PaymentMode"].ToString();
                                roomRegistration.PayFor = Convert.ToInt32(reader["PayFor"]);
                                roomRegistration.BusinessPromotionId = Convert.ToInt32(reader["BusinessPromotionId"]);
                                roomRegistration.IsRoomOwner = Convert.ToInt32(reader["IsRoomOwner"]);
                                roomRegistration.MarketSegmentId = Convert.ToInt32(reader["MarketSegmentId"]);
                                roomRegistration.GuestSourceId = Convert.ToInt32(reader["GuestSourceId"]);
                                roomRegistration.IsReturnedGuest = Convert.ToBoolean(reader["IsReturnedGuest"]);
                                roomRegistration.IsVIPGuest = Convert.ToBoolean(reader["IsVIPGuest"]);
                                roomRegistration.VIPGuestTypeId = Convert.ToInt32(reader["VIPGuestTypeId"]);
                                roomRegistration.Remarks = reader["Remarks"].ToString();
                                roomRegistration.POSRemarks = reader["POSRemarks"].ToString();
                                roomRegistration.MealPlanId = Convert.ToInt32(reader["MealPlanId"]);
                                roomRegistration.ReferenceId = Convert.ToInt32(reader["ReferenceId"]);
                                roomRegistration.AirportPickUp = reader["AirportPickUp"].ToString();
                                roomRegistration.AirportDrop = reader["AirportDrop"].ToString();
                                roomRegistration.APDId = Convert.ToInt32(reader["APDId"]);
                                roomRegistration.ArrivalFlightName = reader["ArrivalFlightName"].ToString();
                                roomRegistration.ArrivalFlightNumber = reader["ArrivalFlightNumber"].ToString();
                                if (roomRegistration.APDId != 0)
                                {
                                    roomRegistration.ArrivalTime = Convert.ToDateTime(currentDate + " " + reader["ArrivalTime"]);
                                }
                                roomRegistration.DepartureAirlineId = Convert.ToInt32(reader["DepartureAirlineId"]);
                                roomRegistration.DepartureFlightName = reader["DepartureFlightName"].ToString();
                                roomRegistration.DepartureFlightNumber = reader["DepartureFlightNumber"].ToString();
                                roomRegistration.IsDepartureChargable = Convert.ToBoolean(reader["IsDepartureChargable"]);

                                if (roomRegistration.APDId != 0)
                                {
                                    roomRegistration.DepartureTime = Convert.ToDateTime(currentDate + " " + reader["DepartureTime"]);
                                }
                                roomRegistration.IsPaidServiceExist = Convert.ToBoolean(reader["IsPaidServiceExist"]);
                                roomRegistration.LocalCurrencyHead = reader["LocalCurrencyHead"].ToString();
                                roomRegistration.CardType = reader["CardType"].ToString();
                                roomRegistration.CardNumber = reader["CardNumber"].ToString();
                                roomRegistration.CardExpireDateShow = reader["CardExpireDate"].ToString();
                                roomRegistration.CardHolderName = reader["CardHolderName"].ToString();
                                roomRegistration.CardReference = reader["CardReference"].ToString();
                                roomRegistration.IsBillHoldUp = Convert.ToBoolean(reader["IsBillHoldUp"]);
                                roomRegistration.IsStopChargePosting = Convert.ToBoolean(reader["IsStopChargePosting"]);
                                roomRegistration.IsBlankRegistrationCard = Convert.ToBoolean(reader["IsBlankRegistrationCard"]);
                                roomRegistration.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                roomRegistration.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                roomRegistration.IsEarlyCheckInChargeEnable = Convert.ToBoolean(reader["IsEarlyCheckInChargeEnable"]);
                                //roomRegistration.POSRemarks = reader["POSRemarks"].ToString();
                                roomRegistration.HoldUpAmount = Convert.ToDecimal(reader["HoldUpAmount"]);
                                roomRegistration.GuestName = reader["GuestName"].ToString();
                                roomRegistration.MarketSegmentId = Convert.ToInt32(reader["MarketSegmentId"]);
                                if (reader["PackageId"] != DBNull.Value)
                                    roomRegistration.PackageId = Convert.ToInt64(reader["PackageId"]);
                                //roomRegistration.GuidId =(Guid)reader["GuidId"];
                            }
                        }
                    }
                }
            }
            return roomRegistration;
        }
        public List<RoomRegistrationBO> GetRoomRegistrationInfoByRoomId(int roomId)
        {
            List<RoomRegistrationBO> roomRegistrationList = new List<RoomRegistrationBO>();
            if (roomId > 0)
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomRegistrationInfoByRoomId_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RroomId", DbType.Int32, roomId);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    RoomRegistrationBO roomRegistration = new RoomRegistrationBO();

                                    roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                    roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                    roomRegistration.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                    roomRegistration.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                    roomRegistration.RoomId = Convert.ToInt32(reader["RoomId"]);
                                    roomRegistration.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                    roomRegistration.EntitleRoomType = Convert.ToInt32(reader["EntitleRoomType"]);
                                    roomRegistration.IsCompanyGuest = Convert.ToBoolean(reader["IsCompanyGuest"]);
                                    roomRegistration.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                    roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                    roomRegistration.CommingFrom = reader["CommingFrom"].ToString();
                                    roomRegistration.NextDestination = reader["NextDestination"].ToString();
                                    roomRegistration.VisitPurpose = reader["VisitPurpose"].ToString();
                                    roomRegistration.IsFromReservation = Convert.ToBoolean(reader["IsFromReservation"]);
                                    roomRegistration.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                    roomRegistration.GuestName = reader["GuestName"].ToString();
                                    roomRegistrationList.Add(roomRegistration);
                                }
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }

        //--Link / D-Link-- start
        public List<HotelLinkedRoomMasterBO> GetAllMasterRoom()
        {
            List<HotelLinkedRoomMasterBO> linkedMasterRoomList = new List<HotelLinkedRoomMasterBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllMasterRoom_SP"))
                {

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HotelLinkedRoomMasterBO hotelLinkedRoomMaster = new HotelLinkedRoomMasterBO
                                {
                                    Id = Convert.ToInt64(reader["Id"]),
                                    RegistrationId = Convert.ToInt64(reader["RegistrationId"]),
                                    LinkName = Convert.ToString(reader["LinkName"]),
                                    Description = Convert.ToString(reader["Description"])
                                };
                                linkedMasterRoomList.Add(hotelLinkedRoomMaster);
                            }
                        }
                    }
                }
            }
            return linkedMasterRoomList;
        }
        public List<HotelLinkedRoomDetailsBO> GetAllDetailsRoom()
        {
            List<HotelLinkedRoomDetailsBO> hotelLinkedRoomDetailsList = new List<HotelLinkedRoomDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllDetailsRoom_SP"))
                {
                    //dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int64, regId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HotelLinkedRoomDetailsBO hotelLinkedRoomDetails = new HotelLinkedRoomDetailsBO
                                {
                                    Id = Convert.ToInt64(reader["Id"]),
                                    RegistrationId = Convert.ToInt64(reader["RegistrationId"]),
                                    MasterId = Convert.ToInt64(reader["MasterId"])
                                };
                                hotelLinkedRoomDetailsList.Add(hotelLinkedRoomDetails);
                            }
                        }
                    }
                }
            }

            return hotelLinkedRoomDetailsList;
        }
        public List<HotelLinkedRoomMasterBO> GetLinkedMasterRoomInfoByRegistrationId(Int64 registrationId)
        {
            List<HotelLinkedRoomMasterBO> linkedMasterRoomList = new List<HotelLinkedRoomMasterBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLinkedMasterRoomInfoByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int64, registrationId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HotelLinkedRoomMasterBO hotelLinkedRoomMaster = new HotelLinkedRoomMasterBO
                                {
                                    Id = Convert.ToInt64(reader["Id"]),
                                    RegistrationId = Convert.ToInt64(reader["RegistrationId"]),
                                    LinkName = Convert.ToString(reader["LinkName"]),
                                    Description = Convert.ToString(reader["Description"])
                                };
                                linkedMasterRoomList.Add(hotelLinkedRoomMaster);
                            }
                        }
                    }
                }
            }
            return linkedMasterRoomList;
        }
        public List<HotelLinkedRoomDetailsBO> GetLinkedDetailsRoomInfoByRegistrationId(Int64 registrationId)
        {
            List<HotelLinkedRoomDetailsBO> hotelLinkedRoomDetailsList = new List<HotelLinkedRoomDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLinkedDetailsRoomInfoByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int64, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HotelLinkedRoomDetailsBO hotelLinkedRoomDetails = new HotelLinkedRoomDetailsBO
                                {
                                    Id = Convert.ToInt64(reader["Id"]),
                                    MasterId = Convert.ToInt64(reader["MasterId"]),
                                    RegistrationId = Convert.ToInt64(reader["RegistrationId"]),
                                    RoomId = Convert.ToInt64(reader["RoomId"])
                                };
                                hotelLinkedRoomDetailsList.Add(hotelLinkedRoomDetails);
                            }
                        }
                    }
                }
            }

            return hotelLinkedRoomDetailsList;
        }
        public List<HotelLinkedRoomDetailsBO> GetDetailRoomsByMasterId(long masterId)
        {
            List<HotelLinkedRoomDetailsBO> detailRooms = new List<HotelLinkedRoomDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDetailRoomsByMasterId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MasterId", DbType.Int64, masterId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HotelLinkedRoomDetailsBO hotelLinkedRoomDetails = new HotelLinkedRoomDetailsBO
                                {
                                    Id = Convert.ToInt64(reader["Id"]),
                                    RegistrationId = Convert.ToInt64(reader["RegistrationId"]),
                                    MasterId = Convert.ToInt64(reader["MasterId"])
                                };
                                detailRooms.Add(hotelLinkedRoomDetails);
                            }
                        }
                    }
                }
            }
            return detailRooms;
        }
        public List<HotelLinkedRoomDetailsBO> GetHotelLinkedRoomDetailsByRegId(long regId)
        {
            List<HotelLinkedRoomDetailsBO> hotelLinkedRoomDetailsList = new List<HotelLinkedRoomDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelLinkedRoomDetailsByRegId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int64, regId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HotelLinkedRoomDetailsBO hotelLinkedRoomDetails = new HotelLinkedRoomDetailsBO
                                {
                                    Id = Convert.ToInt64(reader["Id"]),
                                    RegistrationId = Convert.ToInt64(reader["RegistrationId"]),
                                    MasterId = Convert.ToInt64(reader["MasterId"])
                                };
                                hotelLinkedRoomDetailsList.Add(hotelLinkedRoomDetails);
                            }
                        }
                    }
                }
            }

            return hotelLinkedRoomDetailsList;
        }
        public List<HotelLinkedRoomMasterBO> GetMasterRoomInfoByMasterId(long masterId)
        {
            List<HotelLinkedRoomMasterBO> linkedMasterRoomList = new List<HotelLinkedRoomMasterBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMasterRoomInfoByMasterId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MasterId", DbType.Int64, masterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HotelLinkedRoomMasterBO hotelLinkedRoomMaster = new HotelLinkedRoomMasterBO
                                {
                                    Id = Convert.ToInt64(reader["Id"]),
                                    RegistrationId = Convert.ToInt64(reader["RegistrationId"]),
                                    LinkName = Convert.ToString(reader["LinkName"]),
                                    Description = Convert.ToString(reader["Description"])
                                };
                                linkedMasterRoomList.Add(hotelLinkedRoomMaster);
                            }
                        }
                    }
                }
            }
            return linkedMasterRoomList;
        }
        public List<HotelLinkedRoomMasterBO> GetMasterRoomInfoByRegId(long regId)
        {
            List<HotelLinkedRoomMasterBO> linkedMasterRoomList = new List<HotelLinkedRoomMasterBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMasterRoomInfoByRegId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int64, regId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HotelLinkedRoomMasterBO hotelLinkedRoomMaster = new HotelLinkedRoomMasterBO
                                {
                                    Id = Convert.ToInt64(reader["Id"]),
                                    RegistrationId = Convert.ToInt64(reader["RegistrationId"]),
                                    LinkName = Convert.ToString(reader["LinkName"]),
                                    Description = Convert.ToString(reader["Description"])
                                };
                                linkedMasterRoomList.Add(hotelLinkedRoomMaster);
                            }
                        }
                    }
                }
            }
            return linkedMasterRoomList;
        }
        public List<RoomAlocationBO> GetLinkedRoomInfo()
        {
            List<RoomAlocationBO> roomAlocationsList = new List<RoomAlocationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLinkedRoomInfo_SP"))
                {

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomAlocationBO roomAlocation = new RoomAlocationBO()
                                {
                                    RegistrationId = Convert.ToInt32(reader["RegistrationId"]),
                                    GuestName = reader["GuestName"].ToString(),
                                    RoomId = Convert.ToInt32(reader["RoomId"]),
                                    RoomNumber = reader["RoomNumber"].ToString(),
                                    MasterId = Convert.ToInt64(reader["MasterId"]),
                                    RoomName = reader["RoomName"].ToString()
                                };
                                roomAlocationsList.Add(roomAlocation);

                            }
                        }
                    }
                }
            }

            return roomAlocationsList;
        }

        public RoomAlocationBO GetRegistrationInfoByRegistrationIdNEW(long regId)
        {
            RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRegistrationInfoByRegistrationIdNEW_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegId", DbType.Int64, regId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomAllocationBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomAllocationBO.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomAllocationBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomAllocationBO.RoomNumber = reader["RoomNumber"].ToString();
                                roomAllocationBO.GuestName = reader["GuestName"].ToString();
                                roomAllocationBO.IsStopChargePosting = Convert.ToBoolean(reader["IsStopChargePosting"]);

                            }
                        }
                    }
                }
            }
            return roomAllocationBO;
        }
        public Boolean SaveUpdateRoomStopChargePostingLink(string strRegIds, string strStringSQL, int createdBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveUpdateRoomStopChargePosting_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RegistrationIdList", DbType.String, strRegIds);
                    dbSmartAspects.AddInParameter(command, "@StringSQL", DbType.String, strStringSQL);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean UpdateStopChargePosting(string strRegIds, int createdBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateStopChargePosting_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RegistrationIdList", DbType.String, strRegIds);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }

            return status;
        }
        public Boolean BillPendingReleaseProcess(string registrationNumber)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("BillPendingReleaseProcess_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RegistrationNumber", DbType.String, registrationNumber);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }

            return status;
        }
        public bool DeleteDetailLinkRoooms(List<HotelLinkedRoomDetailsBO> roomDetailsBOs)
        {
            bool status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDetailLinkRoooms_SP"))
                        {
                            foreach (var item in roomDetailsBOs)
                            {
                                command.Parameters.Clear();
                                dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, item.RegistrationId);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;
                            }
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }


            return status;
        }
        public bool DeleteMasterLinkRoooms(List<HotelLinkedRoomMasterBO> roomMasterBOs)
        {
            bool status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteMasterLinkRoooms_SP"))
                        {
                            foreach (var item in roomMasterBOs)
                            {
                                command.Parameters.Clear();
                                dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, item.RegistrationId);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;
                            }
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }


            return status;
        }
        //--Link / D-Link-- end

        public RoomAlocationBO GetRegistrationInfoByRegistrationId(long regId)
        {
            RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRegistrationInfoByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegId", DbType.Int64, regId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomAllocationBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomAllocationBO.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomAllocationBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomAllocationBO.RoomNumber = reader["RoomNumber"].ToString();
                                roomAllocationBO.GuestName = reader["GuestName"].ToString();

                            }
                        }
                    }
                }
            }
            return roomAllocationBO;
        }
        public bool SaveMasterRoom(List<HotelLinkedRoomMasterBO> newAddedMaster, int createdBy, out long tmpId)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveLinkedMasterRoom_SP"))
                        {
                            foreach (var item in newAddedMaster)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int64, item.RegistrationId);
                                dbSmartAspects.AddInParameter(command, "@LinkName", DbType.String, item.LinkName);
                                dbSmartAspects.AddInParameter(command, "@Description", DbType.String, item.Description);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int64, createdBy);

                                dbSmartAspects.AddOutParameter(command, "@Id", DbType.Int64, sizeof(Int64));

                                status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;

                                tmpId = Convert.ToInt32(command.Parameters["@Id"].Value);
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
        public bool UpdateMasterRoomLinkName(List<HotelLinkedRoomMasterBO> updateMaster, int updatedBy, long masterId)
        {
            bool status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateMasterRoom_SP"))
                        {
                            foreach (var item in updateMaster)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, masterId);
                                dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int64, item.RegistrationId);
                                dbSmartAspects.AddInParameter(command, "@LinkName", DbType.String, item.LinkName);
                                dbSmartAspects.AddInParameter(command, "@Description", DbType.String, item.Description);
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
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            return status;
        }
        public bool UpdateMasterRoom(List<HotelLinkedRoomMasterBO> updateMaster, int updatedBy, long masterId)
        {
            bool status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateMasterRoom_SP"))
                        {
                            foreach (var item in updateMaster)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, masterId);
                                dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int64, item.RegistrationId);
                                dbSmartAspects.AddInParameter(command, "@LinkName", DbType.String, item.LinkName);
                                dbSmartAspects.AddInParameter(command, "@Description", DbType.String, item.Description);
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
        public bool SaveDetailsRoom(List<HotelLinkedRoomDetailsBO> newAddDetails, long masterPk, int createdBy, out long tmpId)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveLinkedDetailsRoom_SP"))
                        {

                            foreach (var item in newAddDetails)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int64, item.RegistrationId);
                                dbSmartAspects.AddInParameter(command, "@MasterId", DbType.Int64, masterPk);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int64, createdBy);

                                dbSmartAspects.AddOutParameter(command, "@Id", DbType.Int64, sizeof(Int64));

                                status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;

                                tmpId = Convert.ToInt32(command.Parameters["@Id"].Value);
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
        public bool SaveMasterRoomInDetailTable(long regId, long masterPk, int createdBy, out long tmpId)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveMasterRoomInDetailTable_SP"))
                        {


                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int64, regId);
                            dbSmartAspects.AddInParameter(command, "@MasterId", DbType.Int64, masterPk);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);

                            dbSmartAspects.AddOutParameter(command, "@Id", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;

                            tmpId = Convert.ToInt32(command.Parameters["@Id"].Value);


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
        public RoomRegistrationBO GetRoomRegistrationNRoomDetailsByRoomId(int roomId)
        {
            RoomRegistrationBO roomRegistration = new RoomRegistrationBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomRegistrationInfoByRoomId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RroomId", DbType.Int32, roomId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                roomRegistration.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                roomRegistration.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomRegistration.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                roomRegistration.EntitleRoomType = Convert.ToInt32(reader["EntitleRoomType"]);
                                roomRegistration.IsCompanyGuest = Convert.ToBoolean(reader["IsCompanyGuest"]);
                                roomRegistration.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                roomRegistration.CommingFrom = reader["CommingFrom"].ToString();
                                roomRegistration.NextDestination = reader["NextDestination"].ToString();
                                roomRegistration.VisitPurpose = reader["VisitPurpose"].ToString();
                                roomRegistration.IsFromReservation = Convert.ToBoolean(reader["IsFromReservation"]);
                                roomRegistration.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomRegistration.GuestName = reader["GuestName"].ToString();
                                roomRegistration.IsStopChargePosting = Convert.ToBoolean(reader["IsStopChargePosting"]);
                            }
                        }
                    }
                }
            }
            return roomRegistration;
        }
        public List<RoomRegistrationBO> GetRoomRegistrationInfoByRoomIdGHService(int roomId)
        {
            List<RoomRegistrationBO> roomRegistrationList = new List<RoomRegistrationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomRegistrationInfoByRoomIdGHService_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RroomId", DbType.Int32, roomId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();

                                roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                roomRegistration.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                roomRegistration.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomRegistration.EntitleRoomType = Convert.ToInt32(reader["EntitleRoomType"]);
                                roomRegistration.IsCompanyGuest = Convert.ToBoolean(reader["IsCompanyGuest"]);
                                roomRegistration.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                roomRegistration.CommingFrom = reader["CommingFrom"].ToString();
                                roomRegistration.NextDestination = reader["NextDestination"].ToString();
                                roomRegistration.VisitPurpose = reader["VisitPurpose"].ToString();
                                roomRegistration.IsFromReservation = Convert.ToBoolean(reader["IsFromReservation"]);
                                roomRegistration.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomRegistrationList.Add(roomRegistration);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public Boolean DeleteRoomRegistrationInfoById(int roomRegistrationId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteRoomRegistrationInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, roomRegistrationId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<SearchGuestBO> GetGuestInfoBySearchCriteria(string guestName, string companyName, string roomNumber)
        {
            List<SearchGuestBO> roomRegistrationList = new List<SearchGuestBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, guestName);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SearchGuestBO searchGuestBO = new SearchGuestBO();

                                searchGuestBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                searchGuestBO.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                searchGuestBO.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                searchGuestBO.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                searchGuestBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                searchGuestBO.RoomNumber = reader["RoomNumber"].ToString();
                                searchGuestBO.EntitleRoomType = Convert.ToInt32(reader["EntitleRoomType"]);
                                searchGuestBO.IsCompanyGuest = Convert.ToBoolean(reader["IsCompanyGuest"]);
                                searchGuestBO.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                searchGuestBO.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                searchGuestBO.CommingFrom = reader["CommingFrom"].ToString();
                                searchGuestBO.NextDestination = reader["NextDestination"].ToString();
                                searchGuestBO.VisitPurpose = reader["VisitPurpose"].ToString();
                                searchGuestBO.IsFromReservation = Convert.ToBoolean(reader["IsFromReservation"]);
                                searchGuestBO.ReservationId = Convert.ToInt32(reader["ReservationId"]);

                                searchGuestBO.RegistrationDetailId = Convert.ToInt32(reader["RegistrationDetailId"]);
                                searchGuestBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                searchGuestBO.GuestName = reader["GuestName"].ToString();
                                searchGuestBO.GuestDOB = reader["GuestDOB"].ToString();
                                searchGuestBO.GuestSex = reader["GuestSex"].ToString();
                                searchGuestBO.GuestEmail = reader["GuestEmail"].ToString();
                                searchGuestBO.GuestPhone = reader["GuestPhone"].ToString();
                                searchGuestBO.GuestAddress1 = reader["GuestAddress1"].ToString();
                                searchGuestBO.GuestAddress2 = reader["GuestAddress2"].ToString();
                                searchGuestBO.GuestCity = reader["GuestCity"].ToString();
                                searchGuestBO.GuestZipCode = reader["GuestZipCode"].ToString();
                                searchGuestBO.GuestCountryId = reader["GuestCountryId"].ToString();
                                searchGuestBO.GuestNationality = reader["GuestNationality"].ToString();
                                searchGuestBO.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                searchGuestBO.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                searchGuestBO.NationalId = reader["NationalId"].ToString();
                                searchGuestBO.PassportNumber = reader["PassportNumber"].ToString();
                                searchGuestBO.PIssueDate = reader["PIssueDate"].ToString();
                                searchGuestBO.PIssuePlace = reader["PIssuePlace"].ToString();
                                searchGuestBO.PExpireDate = reader["PExpireDate"].ToString();
                                searchGuestBO.VisaNumber = reader["VisaNumber"].ToString();
                                searchGuestBO.VIssueDate = reader["VIssueDate"].ToString();
                                searchGuestBO.VExpireDate = reader["VExpireDate"].ToString();

                                roomRegistrationList.Add(searchGuestBO);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public int GetIsValidNightAuditDateTime(DateTime auditDate)
        {
            int status = 1;
            Boolean ExeStatus = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetIsValidNightAuditDateTime_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@AuditDateTime", DbType.DateTime, auditDate);
                    dbSmartAspects.AddOutParameter(command, "@IsValid", DbType.Int32, sizeof(Int32));

                    ExeStatus = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    status = Convert.ToInt32(command.Parameters["@IsValid"].Value);
                }
            }
            return status;
        }
        public Boolean GetIsValidNightAuditProcess(DateTime auditDate, string validationType, out string failedNightAuditProcessMessage)
        {
            int status = 1;
            Boolean exeStatus = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetIsValidNightAuditProcess_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@AuditDateTime", DbType.DateTime, auditDate);
                    dbSmartAspects.AddInParameter(command, "@ValidationType", DbType.String, validationType);
                    dbSmartAspects.AddOutParameter(command, "@FailedNightAuditProcessMessage", DbType.String, 500);
                    dbSmartAspects.AddOutParameter(command, "@IsValid", DbType.Int32, sizeof(Int32));
                    exeStatus = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    failedNightAuditProcessMessage = command.Parameters["@FailedNightAuditProcessMessage"].Value.ToString();
                    status = Convert.ToInt32(command.Parameters["@IsValid"].Value);

                    if (status > 0)
                    {
                        exeStatus = true;
                    }
                }
            }
            return exeStatus;
        }
        public Boolean DeleteCheckOutDataCleanUpProcess(DateTime transactionDate)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteCheckOutDataCleanUpProcess_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TransactionDate", DbType.DateTime, transactionDate);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean RoomNightAuditProcess(string prmRegistrationIdList, DateTime nightAuditDate, int IsAdvanceAuditProcess, int createdBy)
        {
            Boolean status = false;
            if (!string.IsNullOrWhiteSpace(prmRegistrationIdList))
            {
                List<string> registrationIdList = new List<string>();
                registrationIdList = prmRegistrationIdList.Split(',').ToList();

                foreach (string regiId in registrationIdList)
                {
                    Int64 registrationId = Convert.ToInt32(regiId);
                    using (DbConnection conn = dbSmartAspects.CreateConnection())
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("RoomNightAuditProcess_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int64, registrationId);
                            dbSmartAspects.AddInParameter(command, "@NightAuditDate", DbType.DateTime, nightAuditDate);
                            dbSmartAspects.AddInParameter(command, "@IsAdvanceAuditProcess", DbType.Int32, IsAdvanceAuditProcess);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                    }
                }
            }
            return status;
        }
        public Boolean RoomNightAuditAutoApprovalProcess(string prmRegistrationIdList, DateTime nightAuditDate, int createdBy)
        {
            RoomNightAuditProcess(prmRegistrationIdList, nightAuditDate, 0, createdBy);

            Boolean status = false;
            if (!string.IsNullOrWhiteSpace(prmRegistrationIdList))
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("RoomNightAuditAutoApprovalProcess_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@RegistrationIdList", DbType.String, prmRegistrationIdList);
                        dbSmartAspects.AddInParameter(command, "@NightAuditDate", DbType.DateTime, nightAuditDate);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            return status;
        }
        public List<AvailableGuestListBO> GetGuestRoomNightAuditInfo(string registrationId, DateTime nightAuditDate, int createdBy)
        {
            string[] strDate = nightAuditDate.ToString().Split('/');
            RoomNightAuditProcess(registrationId, nightAuditDate, 0, createdBy);
            List<AvailableGuestListBO> availableGuestListBO = new List<AvailableGuestListBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestRoomNightAudit_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationId);
                    dbSmartAspects.AddInParameter(cmd, "@NightAuditDate", DbType.DateTime, nightAuditDate);
                    dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, createdBy);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                AvailableGuestListBO availableGuestBO = new AvailableGuestListBO();

                                availableGuestBO.ApprovedId = Convert.ToInt32(reader["ApprovedId"]);
                                availableGuestBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                availableGuestBO.GuestName = reader["GuestName"].ToString();
                                availableGuestBO.RoomType = reader["RoomType"].ToString();
                                availableGuestBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                availableGuestBO.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                availableGuestBO.ServiceName = reader["ServiceName"].ToString();
                                availableGuestBO.PreviousRoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                availableGuestBO.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                availableGuestBO.BPPercentAmount = Convert.ToDecimal(reader["BPPercentAmount"]);
                                availableGuestBO.BPDiscountAmount = Convert.ToDecimal(reader["BPDiscountAmount"]);
                                availableGuestBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                availableGuestBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                availableGuestBO.CitySDCharge = Convert.ToDecimal(reader["CitySDCharge"]);
                                availableGuestBO.AdditionalCharge = Convert.ToDecimal(reader["AdditionalCharge"]);
                                availableGuestBO.VatAmountPercent = Convert.ToDecimal(reader["VatAmountPercent"]);
                                availableGuestBO.ServiceChargePercent = Convert.ToDecimal(reader["ServiceChargePercent"]);
                                availableGuestBO.ReferenceSalesCommission = Convert.ToDecimal(reader["ReferenceSalesCommission"]);
                                availableGuestBO.ReferenceSalesCommissionPercent = Convert.ToDecimal(reader["ReferenceSalesCommissionPercent"]);
                                availableGuestBO.CalculatedPercentAmount = Convert.ToDecimal(reader["CalculatedPercentAmount"]);
                                availableGuestBO.TotalCalculatedAmount = Convert.ToDecimal(reader["TotalCalculatedAmount"]);
                                availableGuestBO.ApprovedStatus = Convert.ToBoolean(reader["ApprovedStatus"]);
                                availableGuestBO.IsRoomOwner = Convert.ToInt32(reader["IsRoomOwner"]);
                                availableGuestBO.CalculatedRoomRate = Convert.ToDecimal(reader["CalculatedRoomRate"]);
                                availableGuestBO.IsGuestCheckedOut = Convert.ToInt32(reader["IsGuestCheckedOut"]);
                                availableGuestListBO.Add(availableGuestBO);
                            }
                        }
                    }
                }
            }

            return availableGuestListBO;
        }
        public List<AvailableGuestListBO> GetAvailableGuestList(string registrationId, DateTime searchDate, string searchRoomNumber)
        {
            string[] strDate = searchDate.ToString().Split('/');
            List<AvailableGuestListBO> availableGuestListBO = new List<AvailableGuestListBO>();

            //using (DbConnection conn = dbSmartAspects.CreateConnection())
            //{
            //    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAvailableGuestList_SP"))
            //    {
            //        dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationId);
            //        dbSmartAspects.AddInParameter(cmd, "@SearchDate", DbType.DateTime, searchDate);
            //        dbSmartAspects.AddInParameter(cmd, "@SearchRoomNumber", DbType.String, searchRoomNumber);

            //        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
            //        {
            //            if (reader != null)
            //            {
            //                while (reader.Read())
            //                {
            //                    AvailableGuestListBO availableGuestBO = new AvailableGuestListBO();

            //                    availableGuestBO.ApprovedId = Convert.ToInt32(reader["ApprovedId"]);
            //                    availableGuestBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
            //                    availableGuestBO.GuestName = reader["GuestName"].ToString();
            //                    availableGuestBO.RoomType = reader["RoomType"].ToString();
            //                    availableGuestBO.RoomId = Convert.ToInt32(reader["RoomId"]);
            //                    availableGuestBO.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
            //                    availableGuestBO.PreviousRoomRate = Convert.ToDecimal(reader["RoomRate"]);
            //                    availableGuestBO.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
            //                    availableGuestBO.BPPercentAmount = Convert.ToDecimal(reader["BPPercentAmount"]);
            //                    availableGuestBO.BPDiscountAmount = Convert.ToDecimal(reader["BPDiscountAmount"]);
            //                    availableGuestBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
            //                    availableGuestBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
            //                    availableGuestBO.VatAmountPercent = Convert.ToDecimal(reader["VatAmountPercent"]);
            //                    availableGuestBO.ServiceChargePercent = Convert.ToDecimal(reader["ServiceChargePercent"]);
            //                    availableGuestBO.ReferenceSalesCommission = Convert.ToDecimal(reader["ReferenceSalesCommission"]);
            //                    availableGuestBO.ReferenceSalesCommissionPercent = Convert.ToDecimal(reader["ReferenceSalesCommissionPercent"]);
            //                    availableGuestBO.CalculatedPercentAmount = Convert.ToDecimal(reader["CalculatedPercentAmount"]);
            //                    availableGuestBO.TotalCalculatedAmount = Convert.ToDecimal(reader["TotalCalculatedAmount"]);
            //                    availableGuestBO.ApprovedStatus = Convert.ToBoolean(reader["ApprovedStatus"]);
            //                    availableGuestBO.IsRoomOwner = Convert.ToInt32(reader["IsRoomOwner"]);
            //                    availableGuestBO.CalculatedRoomRate = Convert.ToDecimal(reader["CalculatedRoomRate"]);

            //                    availableGuestListBO.Add(availableGuestBO);
            //                }
            //            }
            //        }
            //    }
            //}
            return availableGuestListBO;
        }
        public List<AvailableGuestListBO> GetApprovedNightAuditedDataForUpdate(string serviceType, Int64 transactionId, DateTime searchDate, string searchRoomNumber)
        {
            string[] strDate = searchDate.ToString().Split('/');
            List<AvailableGuestListBO> availableGuestListBO = new List<AvailableGuestListBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                //using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApprovedRoomBillForUpdate_SP"))
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApprovedNightAuditedDataForUpdate_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceType", DbType.String, serviceType);
                    dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.Int64, transactionId);
                    dbSmartAspects.AddInParameter(cmd, "@SearchDate", DbType.DateTime, searchDate);
                    dbSmartAspects.AddInParameter(cmd, "@SearchRoomNumber", DbType.String, searchRoomNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                AvailableGuestListBO availableGuestBO = new AvailableGuestListBO();
                                availableGuestBO.ApprovedServiceType = serviceType;
                                availableGuestBO.ApprovedId = Convert.ToInt32(reader["ApprovedId"]);
                                availableGuestBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                availableGuestBO.GuestName = reader["GuestName"].ToString();
                                //availableGuestBO.RoomType = reader["RoomType"].ToString();
                                //availableGuestBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                //availableGuestBO.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                availableGuestBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                //availableGuestBO.PreviousRoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                //availableGuestBO.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                availableGuestBO.ApprovedStatus = Convert.ToBoolean(reader["ApprovedStatus"]);
                                //availableGuestBO.IsRoomOwner = Convert.ToInt32(reader["IsRoomOwner"]);
                                //availableGuestBO.CalculatedRoomRate = Convert.ToDecimal(reader["CalculatedRoomRate"]);
                                //availableGuestBO.BPPercentAmount = Convert.ToDecimal(reader["BPPercentAmount"]);
                                //availableGuestBO.BPDiscountAmount = Convert.ToDecimal(reader["BPDiscountAmount"]);
                                //availableGuestBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                //availableGuestBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                //availableGuestBO.VatAmountPercent = Convert.ToDecimal(reader["VatAmountPercent"]);
                                //availableGuestBO.ServiceChargePercent = Convert.ToDecimal(reader["ServiceChargePercent"]);
                                //availableGuestBO.ReferenceSalesCommission = Convert.ToDecimal(reader["ReferenceSalesCommission"]);
                                //availableGuestBO.ReferenceSalesCommissionPercent = Convert.ToDecimal(reader["ReferenceSalesCommissionPercent"]);
                                //availableGuestBO.CalculatedPercentAmount = Convert.ToDecimal(reader["CalculatedPercentAmount"]);
                                availableGuestBO.TotalCalculatedAmount = Convert.ToDecimal(reader["TotalCalculatedAmount"]);


                                //availableGuestBO.IsBillInclusive = Convert.ToInt32(reader["IsBillInclusive"]);
                                //availableGuestBO.GuestServiceChargeRate = Convert.ToDecimal(reader["GuestServiceChargeRate"]);
                                //availableGuestBO.GuestVatAmountRate = Convert.ToDecimal(reader["GuestVatAmountRate"]);
                                //availableGuestBO.InvoiceServiceCharge = Convert.ToInt32(reader["InvoiceServiceCharge"]);
                                availableGuestBO.IsServiceChargeEnable = Convert.ToInt32(reader["IsServiceChargeEnable"]);
                                availableGuestBO.IsCitySDChargeEnable = Convert.ToInt32(reader["IsCitySDChargeEnable"]);
                                availableGuestBO.IsVatAmountEnable = Convert.ToInt32(reader["IsVatAmountEnable"]);
                                availableGuestBO.IsAdditionalChargeEnable = Convert.ToInt32(reader["IsAdditionalChargeEnable"]);


                                availableGuestBO.ServiceChargeConfig = Convert.ToDecimal(reader["ServiceChargeConfig"]);
                                availableGuestBO.CitySDChargeConfig = Convert.ToDecimal(reader["CitySDChargeConfig"]);
                                availableGuestBO.VatAmountConfig = Convert.ToDecimal(reader["VatAmountConfig"]);
                                availableGuestBO.AdditionalChargeTypeConfig = reader["AdditionalChargeTypeConfig"].ToString();
                                availableGuestBO.AdditionalChargeConfig = Convert.ToDecimal(reader["AdditionalChargeConfig"]);
                                availableGuestBO.IsDiscountApplicableOnRackRateConfig = Convert.ToInt32(reader["IsDiscountApplicableOnRackRateConfig"]);
                                availableGuestBO.IsVatOnSDChargeConfig = Convert.ToInt32(reader["IsVatOnSDChargeConfig"]);
                                availableGuestBO.IsRatePlusPlusConfig = Convert.ToInt32(reader["IsRatePlusPlusConfig"]);





                                availableGuestListBO.Add(availableGuestBO);
                            }
                        }
                    }
                }
            }
            return availableGuestListBO;
        }
        public List<GHServiceBillBO> GetServiceBillNightAudit(string guestServiceType, int serviceBillId, DateTime serviceDate, string roomNumber)
        {
            List<GHServiceBillBO> ghServiceBillList = new List<GHServiceBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetServiceBillNightAudit_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GuestServiceType", DbType.String, guestServiceType);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, serviceBillId);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceDate", DbType.DateTime, serviceDate);
                    dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GHServiceBillBO ghServiceBill = new GHServiceBillBO();

                                ghServiceBill.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                ghServiceBill.ApprovedId = Convert.ToInt32(reader["ApprovedId"]);
                                ghServiceBill.ServiceBillId = Convert.ToInt32(reader["ServiceBillId"]);
                                ghServiceBill.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                ghServiceBill.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                ghServiceBill.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                ghServiceBill.BillNumber = reader["BillNumber"].ToString();
                                if (reader["RoomNumber"].ToString() == "Cash")
                                {
                                    ghServiceBill.RoomNumber = "Walk-In";
                                }
                                else
                                {
                                    ghServiceBill.RoomNumber = reader["RoomNumber"].ToString();
                                }
                                ghServiceBill.ServiceType = reader["ServiceType"].ToString();
                                ghServiceBill.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                ghServiceBill.ServiceName = reader["ServiceName"].ToString();
                                ghServiceBill.ServiceRate = Convert.ToDecimal(reader["ServiceRate"]);
                                ghServiceBill.ServiceQuantity = Convert.ToDecimal(reader["ServiceQuantity"]);
                                ghServiceBill.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                ghServiceBill.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                ghServiceBill.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);

                                ghServiceBill.CitySDCharge = Convert.ToDecimal(reader["CitySDCharge"]);
                                ghServiceBill.AdditionalCharge = Convert.ToDecimal(reader["AdditionalCharge"]);


                                ghServiceBill.VatAmountPercent = Convert.ToDecimal(reader["VatAmountPercent"]);
                                ghServiceBill.ServiceChargePercent = Convert.ToDecimal(reader["ServiceChargePercent"]);
                                ghServiceBill.CalculatedPercentAmount = Convert.ToDecimal(reader["CalculatedPercentAmount"]);
                                ghServiceBill.TotalCalculatedAmount = Convert.ToDecimal(reader["TotalCalculatedAmount"]);
                                ghServiceBill.ApprovedStatus = Convert.ToBoolean(reader["ApprovedStatus"]);
                                ghServiceBill.IsPaidService = Convert.ToBoolean(reader["IsPaidService"]);
                                ghServiceBill.IsPaidServiceAchieved = Convert.ToBoolean(reader["IsPaidServiceAchieved"]);

                                ghServiceBillList.Add(ghServiceBill);
                            }
                        }
                    }
                }
            }
            return ghServiceBillList;
        }
        public List<GHServiceBillBO> GetGHServiceBillInfoForNightAuditSearch(string guestServiceType, int serviceBillId, DateTime serviceDate, string roomNumber)
        {
            List<GHServiceBillBO> ghServiceBillList = new List<GHServiceBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGHServiceBillInfoForNightAuditSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GuestServiceType", DbType.String, guestServiceType);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, serviceBillId);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceDate", DbType.DateTime, serviceDate);
                    dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GHServiceBillBO ghServiceBill = new GHServiceBillBO();

                                ghServiceBill.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                ghServiceBill.ApprovedId = Convert.ToInt32(reader["ApprovedId"]);
                                ghServiceBill.ServiceBillId = Convert.ToInt32(reader["ServiceBillId"]);
                                ghServiceBill.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                ghServiceBill.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                ghServiceBill.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                ghServiceBill.BillNumber = reader["BillNumber"].ToString();
                                if (reader["RoomNumber"].ToString() == "Cash")
                                {
                                    ghServiceBill.RoomNumber = "Walk-In";
                                }
                                else
                                {
                                    ghServiceBill.RoomNumber = reader["RoomNumber"].ToString();
                                }
                                ghServiceBill.ServiceType = reader["ServiceType"].ToString();
                                ghServiceBill.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                ghServiceBill.ServiceName = reader["ServiceName"].ToString();
                                ghServiceBill.ServiceRate = Convert.ToDecimal(reader["ServiceRate"]);
                                ghServiceBill.ServiceQuantity = Convert.ToDecimal(reader["ServiceQuantity"]);
                                ghServiceBill.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                ghServiceBill.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                ghServiceBill.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                ghServiceBill.VatAmountPercent = Convert.ToDecimal(reader["VatAmountPercent"]);
                                ghServiceBill.ServiceChargePercent = Convert.ToDecimal(reader["ServiceChargePercent"]);
                                ghServiceBill.CalculatedPercentAmount = Convert.ToDecimal(reader["CalculatedPercentAmount"]);
                                ghServiceBill.TotalCalculatedAmount = Convert.ToDecimal(reader["TotalCalculatedAmount"]);
                                ghServiceBill.ApprovedStatus = Convert.ToBoolean(reader["ApprovedStatus"]);
                                ghServiceBill.IsPaidService = Convert.ToBoolean(reader["IsPaidService"]);
                                ghServiceBill.IsPaidServiceAchieved = Convert.ToBoolean(reader["IsPaidServiceAchieved"]);

                                ghServiceBillList.Add(ghServiceBill);
                            }
                        }
                    }
                }
            }
            return ghServiceBillList;
        }
        public List<GHServiceBillBO> GetApprovedServiceBillForUpdate(int serviceBillId, DateTime serviceDate, string roomNumber)
        {
            List<GHServiceBillBO> ghServiceBillList = new List<GHServiceBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApprovedServiceBillForUpdate_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, serviceBillId);
                    dbSmartAspects.AddInParameter(cmd, "@SearchDate", DbType.DateTime, serviceDate);
                    dbSmartAspects.AddInParameter(cmd, "@SearchRoomNumber", DbType.String, roomNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GHServiceBillBO ghServiceBill = new GHServiceBillBO();

                                ghServiceBill.ApprovedId = Convert.ToInt32(reader["ApprovedId"]);
                                ghServiceBill.ServiceBillId = Convert.ToInt32(reader["ServiceBillId"]);
                                ghServiceBill.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                ghServiceBill.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                ghServiceBill.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                ghServiceBill.BillNumber = reader["BillNumber"].ToString();
                                ghServiceBill.RoomNumber = reader["RoomNumber"].ToString();
                                ghServiceBill.ServiceType = reader["ServiceType"].ToString();
                                ghServiceBill.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                ghServiceBill.ServiceName = reader["ServiceName"].ToString();
                                ghServiceBill.ServiceRate = Convert.ToDecimal(reader["ServiceRate"]);
                                ghServiceBill.ServiceQuantity = Convert.ToDecimal(reader["ServiceQuantity"]);
                                ghServiceBill.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                ghServiceBill.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                ghServiceBill.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                ghServiceBill.VatAmountPercent = Convert.ToDecimal(reader["VatAmountPercent"]);
                                ghServiceBill.ServiceChargePercent = Convert.ToDecimal(reader["ServiceChargePercent"]);
                                ghServiceBill.CalculatedPercentAmount = Convert.ToDecimal(reader["CalculatedPercentAmount"]);
                                ghServiceBill.TotalCalculatedAmount = Convert.ToDecimal(reader["TotalCalculatedAmount"]);
                                ghServiceBill.ApprovedStatus = Convert.ToBoolean(reader["ApprovedStatus"]);

                                ghServiceBillList.Add(ghServiceBill);
                            }
                        }
                    }
                }
            }
            return ghServiceBillList;
        }
        public Boolean SavePendingGuestBillApprovedInfo(AvailableGuestListBO availableGuestList, out int tmpApprovedId)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SavePendingGuestBillApprovedInfo_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@RegistrationId", DbType.Int32, availableGuestList.RegistrationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ApprovedDate", DbType.DateTime, availableGuestList.ApprovedDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceDate", DbType.DateTime, availableGuestList.ServiceDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomType", DbType.String, availableGuestList.RoomType);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomId", DbType.Int32, availableGuestList.RoomId);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomNumber", DbType.String, availableGuestList.RoomNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceName", DbType.String, availableGuestList.ServiceName);
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousRoomRate", DbType.Decimal, availableGuestList.PreviousRoomRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomRate", DbType.Decimal, availableGuestList.RoomRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@BPPercentAmount", DbType.Decimal, availableGuestList.BPPercentAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@BPDiscountAmount", DbType.Decimal, availableGuestList.BPDiscountAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@VatAmount", DbType.Decimal, availableGuestList.VatAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceCharge", DbType.Decimal, availableGuestList.ServiceCharge);
                        dbSmartAspects.AddInParameter(commandMaster, "@CitySDCharge", DbType.Decimal, availableGuestList.CitySDCharge);
                        dbSmartAspects.AddInParameter(commandMaster, "@AdditionalCharge", DbType.Decimal, availableGuestList.AdditionalCharge);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReferenceSalesCommission", DbType.Decimal, availableGuestList.ReferenceSalesCommission);
                        dbSmartAspects.AddInParameter(commandMaster, "@TotalCalculatedAmount", DbType.Decimal, availableGuestList.TotalCalculatedAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, availableGuestList.CreatedBy);
                        dbSmartAspects.AddOutParameter(commandMaster, "@ApprovedId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        tmpApprovedId = Convert.ToInt32(commandMaster.Parameters["@ApprovedId"].Value);

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        public Boolean SaveGuestBillApprovedInfo(AvailableGuestListBO availableGuestList, out int tmpApprovedId)
        {
            bool retVal = false;
            int status = 0;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveGuestBillApprovedInfo_SP"))
                    {
                        conn.Open();

                        using (DbTransaction transction = conn.BeginTransaction())
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@RegistrationId", DbType.Int32, availableGuestList.RegistrationId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedDate", DbType.DateTime, availableGuestList.ApprovedDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@ServiceDate", DbType.DateTime, availableGuestList.ServiceDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@RoomType", DbType.String, availableGuestList.RoomType);
                            dbSmartAspects.AddInParameter(commandMaster, "@RoomId", DbType.Int32, availableGuestList.RoomId);
                            dbSmartAspects.AddInParameter(commandMaster, "@RoomNumber", DbType.String, availableGuestList.RoomNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@ServiceName", DbType.String, availableGuestList.ServiceName);
                            dbSmartAspects.AddInParameter(commandMaster, "@PreviousRoomRate", DbType.Decimal, availableGuestList.PreviousRoomRate);
                            dbSmartAspects.AddInParameter(commandMaster, "@RoomRate", DbType.Decimal, availableGuestList.RoomRate);
                            dbSmartAspects.AddInParameter(commandMaster, "@BPPercentAmount", DbType.Decimal, availableGuestList.BPPercentAmount);
                            dbSmartAspects.AddInParameter(commandMaster, "@BPDiscountAmount", DbType.Decimal, availableGuestList.BPDiscountAmount);
                            dbSmartAspects.AddInParameter(commandMaster, "@VatAmount", DbType.Decimal, availableGuestList.VatAmount);
                            dbSmartAspects.AddInParameter(commandMaster, "@ServiceCharge", DbType.Decimal, availableGuestList.ServiceCharge);
                            dbSmartAspects.AddInParameter(commandMaster, "@CitySDCharge", DbType.Decimal, availableGuestList.CitySDCharge);
                            dbSmartAspects.AddInParameter(commandMaster, "@AdditionalCharge", DbType.Decimal, availableGuestList.AdditionalCharge);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReferenceSalesCommission", DbType.Decimal, availableGuestList.ReferenceSalesCommission);
                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, availableGuestList.CreatedBy);
                            dbSmartAspects.AddOutParameter(commandMaster, "@ApprovedId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                            tmpApprovedId = Convert.ToInt32(commandMaster.Parameters["@ApprovedId"].Value);

                            if (status > 0)
                            {
                                transction.Commit();
                                retVal = true;
                            }
                            else
                            {
                                retVal = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        public Boolean UpdateGuestBillApprovedInfo(AvailableGuestListBO availableGuestList)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateGuestBillApprovedInfo_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@ApprovedId", DbType.Int32, availableGuestList.ApprovedId);
                        dbSmartAspects.AddInParameter(commandMaster, "@RegistrationId", DbType.Int32, availableGuestList.RegistrationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ApprovedDate", DbType.DateTime, availableGuestList.ApprovedDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomType", DbType.String, availableGuestList.RoomType);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomId", DbType.Int32, availableGuestList.RoomId);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomNumber", DbType.String, availableGuestList.RoomNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousRoomRate", DbType.Decimal, availableGuestList.PreviousRoomRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomRate", DbType.Decimal, availableGuestList.RoomRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@BPPercentAmount", DbType.Decimal, availableGuestList.BPPercentAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@BPDiscountAmount", DbType.Decimal, availableGuestList.BPDiscountAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@VatAmount", DbType.Decimal, availableGuestList.VatAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceCharge", DbType.Decimal, availableGuestList.ServiceCharge);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReferenceSalesCommission", DbType.Decimal, availableGuestList.ReferenceSalesCommission);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, availableGuestList.CreatedBy);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        public Boolean SavePendingGuestServiceBillApprovedInfo(GHServiceBillBO serviceList, out int tmpApprovedId)
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SavePendingGuestServiceBillApprovedInfo_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@RegistrationId", DbType.Int32, serviceList.RegistrationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ApprovedDate", DbType.DateTime, serviceList.ApprovedDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceBillId", DbType.Int32, serviceList.ServiceBillId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceDate", DbType.DateTime, serviceList.ServiceDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceType", DbType.String, serviceList.ServiceType);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceId", DbType.Int32, serviceList.ServiceId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceName", DbType.String, serviceList.ServiceName);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceQuantity", DbType.Decimal, serviceList.ServiceQuantity);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceRate", DbType.Decimal, serviceList.ServiceRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@DiscountAmount", DbType.Decimal, serviceList.DiscountAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@VatAmount", DbType.Decimal, serviceList.VatAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceCharge", DbType.Decimal, serviceList.ServiceCharge);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsPaidService", DbType.Boolean, serviceList.IsPaidService);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, serviceList.CreatedBy);
                        dbSmartAspects.AddOutParameter(commandMaster, "@ApprovedId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        tmpApprovedId = Convert.ToInt32(commandMaster.Parameters["@ApprovedId"].Value);

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        public Boolean SaveGuestServiceBillApprovedInfo(GHServiceBillBO serviceList, out int tmpApprovedId)
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveGuestServiceBillApprovedInfo_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@PaymentMode", DbType.String, serviceList.PaymentMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@RegistrationId", DbType.Int32, serviceList.RegistrationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ApprovedDate", DbType.DateTime, serviceList.ApprovedDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceBillId", DbType.Int32, serviceList.ServiceBillId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceDate", DbType.DateTime, serviceList.ServiceDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceType", DbType.String, serviceList.ServiceType);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceId", DbType.Int32, serviceList.ServiceId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceName", DbType.String, serviceList.ServiceName);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceQuantity", DbType.Decimal, serviceList.ServiceQuantity);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceRate", DbType.Decimal, serviceList.ServiceRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@DiscountAmount", DbType.Decimal, serviceList.DiscountAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@VatAmount", DbType.Decimal, serviceList.VatAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceCharge", DbType.Decimal, serviceList.ServiceCharge);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsPaidService", DbType.Boolean, serviceList.IsPaidService);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsPaidServiceAchieved", DbType.Boolean, !string.IsNullOrWhiteSpace(serviceList.IsPaidServiceAchieved.ToString()) ? Convert.ToBoolean(serviceList.IsPaidServiceAchieved.ToString()) : false);
                        dbSmartAspects.AddInParameter(commandMaster, "@RestaurantBillId", DbType.Int32, serviceList.RestaurantBillId);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, serviceList.CreatedBy);
                        dbSmartAspects.AddOutParameter(commandMaster, "@ApprovedId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        tmpApprovedId = Convert.ToInt32(commandMaster.Parameters["@ApprovedId"].Value);

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        public Boolean UpdateGuestServiceBillApprovedInfo(GHServiceBillBO serviceList)
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateGuestServiceBillApprovedInfo_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@ApprovedId", DbType.Int32, serviceList.ApprovedId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PaymentMode", DbType.String, serviceList.PaymentMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@RegistrationId", DbType.Int32, serviceList.RegistrationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ApprovedDate", DbType.DateTime, serviceList.ApprovedDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceBillId", DbType.Int32, serviceList.ServiceBillId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceDate", DbType.DateTime, serviceList.ServiceDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceType", DbType.String, serviceList.ServiceType);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceId", DbType.Int32, serviceList.ServiceId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceName", DbType.String, serviceList.ServiceName);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceQuantity", DbType.Decimal, serviceList.ServiceQuantity);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceRate", DbType.Decimal, serviceList.ServiceRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@DiscountAmount", DbType.Decimal, serviceList.DiscountAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@VatAmount", DbType.Decimal, serviceList.VatAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceCharge", DbType.Decimal, serviceList.ServiceCharge);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsPaidServiceAchieved", DbType.Boolean, !string.IsNullOrWhiteSpace(serviceList.IsPaidServiceAchieved.ToString()) ? Convert.ToBoolean(serviceList.IsPaidServiceAchieved.ToString()) : false);
                        dbSmartAspects.AddInParameter(commandMaster, "@RestaurantBillId", DbType.Int32, !string.IsNullOrWhiteSpace(serviceList.RestaurantBillId.ToString()) ? Convert.ToInt32(serviceList.RestaurantBillId.ToString()) : 0);

                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, serviceList.CreatedBy);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);


                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        public RoomAlocationBO GetRoomAlocationInfo(string RoomNumber)
        {
            RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRegistrationInfoByRoomNumber_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, RoomNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                roomAllocationBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomAllocationBO.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomAllocationBO.GuestName = reader["GuestName"].ToString();
                                roomAllocationBO.RoomType = reader["RoomType"].ToString();
                                roomAllocationBO.RoomRate = Decimal.Parse(reader["RoomRate"].ToString());
                                roomAllocationBO.RoomTypeId = Int32.Parse(reader["RoomTypeId"].ToString());

                            }
                        }
                    }
                }
            }

            return roomAllocationBO;
        }
        public List<RoomAlocationBO> GetActiveRegistrationInfo()
        {
            List<RoomAlocationBO> roomAllocationBOList = new List<RoomAlocationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveRegistrationInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                                roomAllocationBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomAllocationBO.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomAllocationBO.GuestName = reader["GuestName"].ToString();
                                roomAllocationBO.RoomType = reader["RoomType"].ToString();
                                roomAllocationBO.RoomRate = Decimal.Parse(reader["RoomRate"].ToString());
                                roomAllocationBO.RoomTypeId = Int32.Parse(reader["RoomTypeId"].ToString());
                                roomAllocationBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomAllocationBO.RoomNumber = reader["RoomNumber"].ToString();
                                roomAllocationBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                roomAllocationBO.AccountHeadCompanyId = Convert.ToInt32(reader["AccountHeadCompanyId"]);
                                roomAllocationBO.CompanyName = reader["CompanyName"].ToString();
                                roomAllocationBO.NodeId = Convert.ToInt32(reader["NodeId"]);
                                roomAllocationBO.CurrencyType = Convert.ToInt32(reader["CurrencyType"]);
                                roomAllocationBO.LocalCurrencyHead = reader["LocalCurrencyHead"].ToString();
                                roomAllocationBO.CurrencyTypeHead = reader["CurrencyTypeHead"].ToString();
                                roomAllocationBO.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                roomAllocationBO.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                roomAllocationBO.BillingStartDate = Convert.ToDateTime(reader["BillingStartDate"]);
                                roomAllocationBO.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                roomAllocationBO.DiscountType = reader["DiscountType"].ToString();
                                roomAllocationBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                roomAllocationBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                roomAllocationBO.BusinessPromotionId = Convert.ToInt32(reader["BusinessPromotionId"]);
                                roomAllocationBO.IsCompanyGuest = Convert.ToBoolean(reader["IsCompanyGuest"]);
                                roomAllocationBO.IsHouseUseRoom = Convert.ToBoolean(reader["IsHouseUseRoom"]);
                                roomAllocationBO.IsStopChargePosting = Convert.ToBoolean(reader["IsStopChargePosting"]);
                                roomAllocationBO.TransactionDate = Convert.ToDateTime(reader["TransactionDate"]);
                                roomAllocationBO.Remarks = reader["Remarks"].ToString();

                                roomAllocationBOList.Add(roomAllocationBO);
                            }
                        }
                    }
                }
            }

            return roomAllocationBOList;
        }
        public RoomAlocationBO GetActiveRegistrationInfoByRoomNumber(string RoomNumber)
        {
            RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveRegistrationInfoByRoomNumber_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, RoomNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                roomAllocationBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomAllocationBO.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomAllocationBO.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomAllocationBO.GuestName = reader["GuestName"].ToString();
                                roomAllocationBO.RoomType = reader["RoomType"].ToString();
                                roomAllocationBO.RoomRate = Decimal.Parse(reader["RoomRate"].ToString());
                                roomAllocationBO.RoomTypeId = Int32.Parse(reader["RoomTypeId"].ToString());
                                roomAllocationBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomAllocationBO.RoomNumber = reader["RoomNumber"].ToString();
                                roomAllocationBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                roomAllocationBO.AccountHeadCompanyId = Convert.ToInt32(reader["AccountHeadCompanyId"]);
                                roomAllocationBO.CompanyName = reader["CompanyName"].ToString();
                                roomAllocationBO.NodeId = Convert.ToInt32(reader["NodeId"]);
                                roomAllocationBO.CurrencyType = Convert.ToInt32(reader["CurrencyType"]);
                                roomAllocationBO.LocalCurrencyHead = reader["LocalCurrencyHead"].ToString();
                                roomAllocationBO.CurrencyTypeHead = reader["CurrencyTypeHead"].ToString();
                                roomAllocationBO.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                roomAllocationBO.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                roomAllocationBO.BillingStartDate = Convert.ToDateTime(reader["BillingStartDate"]);
                                roomAllocationBO.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                roomAllocationBO.DiscountType = reader["DiscountType"].ToString();
                                roomAllocationBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                roomAllocationBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                roomAllocationBO.BusinessPromotionId = Convert.ToInt32(reader["BusinessPromotionId"]);
                                roomAllocationBO.IsCompanyGuest = Convert.ToBoolean(reader["IsCompanyGuest"]);
                                roomAllocationBO.IsHouseUseRoom = Convert.ToBoolean(reader["IsHouseUseRoom"]);
                                roomAllocationBO.IsStopChargePosting = Convert.ToBoolean(reader["IsStopChargePosting"]);
                                roomAllocationBO.TransactionDate = Convert.ToDateTime(reader["TransactionDate"]);
                                roomAllocationBO.Remarks = reader["Remarks"].ToString();
                                roomAllocationBO.MinimumRoomRate = Convert.ToDecimal(reader["MinimumRoomRate"]);
                            }
                        }
                    }
                }
            }

            return roomAllocationBO;
        }
        public RoomAlocationBO GetActiveRegistrationInfoForBillHoldUpByRegistrationId(int registrationId)
        {
            RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveRegistrationInfoForBillHoldUpByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                roomAllocationBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomAllocationBO.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomAllocationBO.GuestName = reader["GuestName"].ToString();
                                roomAllocationBO.RoomType = reader["RoomType"].ToString();
                                roomAllocationBO.RoomRate = Decimal.Parse(reader["RoomRate"].ToString());
                                roomAllocationBO.RoomTypeId = Int32.Parse(reader["RoomTypeId"].ToString());
                                roomAllocationBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomAllocationBO.RoomNumber = reader["RoomNumber"].ToString();
                                roomAllocationBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                roomAllocationBO.AccountHeadCompanyId = Convert.ToInt32(reader["AccountHeadCompanyId"]);
                                roomAllocationBO.CompanyName = reader["CompanyName"].ToString();
                                roomAllocationBO.NodeId = Convert.ToInt32(reader["NodeId"]);
                                roomAllocationBO.CurrencyType = Convert.ToInt32(reader["CurrencyType"]);
                                roomAllocationBO.LocalCurrencyHead = reader["LocalCurrencyHead"].ToString();
                                roomAllocationBO.CurrencyTypeHead = reader["CurrencyTypeHead"].ToString();
                                roomAllocationBO.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                roomAllocationBO.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                roomAllocationBO.BillingStartDate = Convert.ToDateTime(reader["BillingStartDate"]);
                                roomAllocationBO.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                roomAllocationBO.DiscountType = reader["DiscountType"].ToString();
                                roomAllocationBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                roomAllocationBO.BusinessPromotionId = Convert.ToInt32(reader["BusinessPromotionId"]);
                            }
                        }
                    }
                }
            }

            return roomAllocationBO;
        }
        public Boolean UpdateCurrentRoom(int roomRegistrationId, int roomId, int roomTypeId, string discountType, decimal unitPrice, decimal discountAmount, decimal roomRate, string Remarks, Boolean IsCompanyGuest, Boolean IsHouseUseRoom, int lastModifiedBy)
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateRoomInfoByRegiID_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@RegistrationId", DbType.Int32, roomRegistrationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomId", DbType.Int32, roomId);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomTypeID", DbType.Int32, roomTypeId);
                        dbSmartAspects.AddInParameter(commandMaster, "@DiscountType", DbType.String, discountType);
                        dbSmartAspects.AddInParameter(commandMaster, "@UnitPrice", DbType.Decimal, unitPrice);
                        dbSmartAspects.AddInParameter(commandMaster, "@DiscountAmount", DbType.Decimal, discountAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomRate", DbType.Decimal, roomRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, Remarks);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsCompanyGuest", DbType.Boolean, IsCompanyGuest);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsHouseUseRoom", DbType.Boolean, IsHouseUseRoom);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        public List<SearchGuestBO> GetRoomRegistrationInfoBySearchCriteria(string GuestName, string GuestCompany, string RoomNumber, string RegistrationNumber)
        {
            List<SearchGuestBO> roomRegistrationList = new List<SearchGuestBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomRegistrationInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, GuestName);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, GuestCompany);
                    dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, RoomNumber);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationNumber", DbType.String, RegistrationNumber);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SearchGuestBO searchGuestBO = new SearchGuestBO();
                                searchGuestBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                searchGuestBO.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                searchGuestBO.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                searchGuestBO.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                searchGuestBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                searchGuestBO.RoomNumber = reader["RoomNumber"].ToString();
                                searchGuestBO.EntitleRoomType = Convert.ToInt32(reader["EntitleRoomType"]);
                                searchGuestBO.IsCompanyGuest = Convert.ToBoolean(reader["IsCompanyGuest"]);
                                searchGuestBO.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                searchGuestBO.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                searchGuestBO.CommingFrom = reader["CommingFrom"].ToString();
                                searchGuestBO.NextDestination = reader["NextDestination"].ToString();
                                searchGuestBO.VisitPurpose = reader["VisitPurpose"].ToString();
                                searchGuestBO.IsFromReservation = Convert.ToBoolean(reader["IsFromReservation"]);
                                searchGuestBO.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                searchGuestBO.RegistrationDetailId = Convert.ToInt32(reader["RegistrationDetailId"]);
                                searchGuestBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                searchGuestBO.GuestName = reader["GuestName"].ToString();
                                searchGuestBO.GuestDOB = reader["GuestDOB"].ToString();
                                searchGuestBO.GuestSex = reader["GuestSex"].ToString();
                                searchGuestBO.GuestEmail = reader["GuestEmail"].ToString();
                                searchGuestBO.GuestPhone = reader["GuestPhone"].ToString();
                                searchGuestBO.GuestAddress1 = reader["GuestAddress1"].ToString();
                                searchGuestBO.GuestAddress2 = reader["GuestAddress2"].ToString();
                                searchGuestBO.GuestCity = reader["GuestCity"].ToString();
                                searchGuestBO.GuestZipCode = reader["GuestZipCode"].ToString();
                                searchGuestBO.GuestCountryId = reader["GuestCountryId"].ToString();
                                searchGuestBO.GuestNationality = reader["GuestNationality"].ToString();
                                searchGuestBO.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                searchGuestBO.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                searchGuestBO.NationalId = reader["NationalId"].ToString();
                                searchGuestBO.PassportNumber = reader["PassportNumber"].ToString();
                                searchGuestBO.PIssueDate = reader["PIssueDate"].ToString();
                                searchGuestBO.PIssuePlace = reader["PIssuePlace"].ToString();
                                searchGuestBO.PExpireDate = reader["PExpireDate"].ToString();
                                searchGuestBO.VisaNumber = reader["VisaNumber"].ToString();
                                searchGuestBO.VIssueDate = reader["VIssueDate"].ToString();
                                searchGuestBO.VExpireDate = reader["VExpireDate"].ToString();

                                roomRegistrationList.Add(searchGuestBO);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public bool SaveTemporaryGuest(GuestInformationBO registrationDetailBO, string tempRegId, DateTime checkInDate, decimal paxInRate, List<GuestPreferenceMappingBO> preferenList)
        {
            bool retVal = false;
            int status = 0;
            int TempGuestId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveGuestInformation_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {

                        if (registrationDetailBO.GuestId <= 0)
                        {
                            commandMaster.Parameters.Clear();
                            dbSmartAspects.AddInParameter(commandMaster, "@RegistrationId", DbType.Int32, tempRegId);
                            dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
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
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckInDate", DbType.DateTime, checkInDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@PaxInRate", DbType.Decimal, paxInRate);
                            dbSmartAspects.AddOutParameter(commandMaster, "@TempGuestId", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            TempGuestId = Convert.ToInt32(commandMaster.Parameters["@TempGuestId"].Value);
                            using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateDocumentInfo_SP"))
                            {
                                commandDocument.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandDocument, "@GuestId", DbType.Int32, TempGuestId);
                                dbSmartAspects.AddInParameter(commandDocument, "@TempOwnerId", DbType.String, registrationDetailBO.tempOwnerId);
                                status = dbSmartAspects.ExecuteNonQuery(commandDocument, transction);
                            }

                            using (DbCommand commandPreference = dbSmartAspects.GetStoredProcCommand("SaveGuestPreferenceMappingInfo_SP"))
                            {
                                string preferences = string.Join(",", preferenList.Select(i => i.PreferenceId));

                                commandPreference.Parameters.Clear();

                                dbSmartAspects.AddInParameter(commandPreference, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                                dbSmartAspects.AddInParameter(commandPreference, "@PreferenceIdList", DbType.String, preferences);

                                status = dbSmartAspects.ExecuteNonQuery(commandPreference, transction);

                            }
                        }
                        else
                        {

                            GuestInformationDA infoDA = new GuestInformationDA();
                            bool success = infoDA.UpdateGuestInformationWithOutDocument(registrationDetailBO);

                            using (DbCommand commandGR = dbSmartAspects.GetStoredProcCommand("SaveGuestRegistrationInfo_SP"))
                            {
                                commandGR.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandGR, "@RegistrationId", DbType.Int32, tempRegId);
                                dbSmartAspects.AddInParameter(commandGR, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                                status = dbSmartAspects.ExecuteNonQuery(commandGR, transction);
                            }

                            using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateDocumentInfo_SP"))
                            {
                                commandDocument.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandDocument, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                                dbSmartAspects.AddInParameter(commandDocument, "@TempOwnerId", DbType.String, registrationDetailBO.tempOwnerId);
                                status = dbSmartAspects.ExecuteNonQuery(commandDocument, transction);
                            }
                        }
                        transction.Commit();

                    }
                }
            }
            return true;
        }
        public bool SaveTemporaryGuestNew(GuestInformationBO registrationDetailBO, string tempRegId, DateTime checkInDate, decimal paxInRate, List<GuestPreferenceMappingBO> preferenList)
        {
            bool retVal = false;
            int status = 0;
            int TempGuestId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveGuestInformationNew_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {

                        if (registrationDetailBO.GuestId <= 0)
                        {
                            commandMaster.Parameters.Clear();
                            dbSmartAspects.AddInParameter(commandMaster, "@RegistrationId", DbType.Int32, tempRegId);
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
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckInDate", DbType.DateTime, checkInDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@PaxInRate", DbType.Decimal, paxInRate);
                            dbSmartAspects.AddInParameter(commandMaster, "@AdditionalRemarks", DbType.String, registrationDetailBO.AdditionalRemarks);
                            dbSmartAspects.AddOutParameter(commandMaster, "@TempGuestId", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            TempGuestId = Convert.ToInt32(commandMaster.Parameters["@TempGuestId"].Value);
                            using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateDocumentInfo_SP"))
                            {
                                commandDocument.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandDocument, "@GuestId", DbType.Int32, TempGuestId);
                                dbSmartAspects.AddInParameter(commandDocument, "@TempOwnerId", DbType.String, registrationDetailBO.tempOwnerId);
                                status = dbSmartAspects.ExecuteNonQuery(commandDocument, transction);
                            }

                            using (DbCommand commandPreference = dbSmartAspects.GetStoredProcCommand("SaveGuestPreferenceMappingInfo_SP"))
                            {
                                string preferences = string.Join(",", preferenList.Select(i => i.PreferenceId));

                                commandPreference.Parameters.Clear();

                                dbSmartAspects.AddInParameter(commandPreference, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                                dbSmartAspects.AddInParameter(commandPreference, "@PreferenceIdList", DbType.String, preferences);

                                status = dbSmartAspects.ExecuteNonQuery(commandPreference, transction);

                            }


                        }
                        else
                        {

                            GuestInformationDA infoDA = new GuestInformationDA();
                            bool success = infoDA.UpdateGuestInformationWithOutDocument(registrationDetailBO);

                            using (DbCommand commandGR = dbSmartAspects.GetStoredProcCommand("SaveGuestRegistrationInfo_SP"))
                            {
                                commandGR.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandGR, "@RegistrationId", DbType.Int32, tempRegId);
                                dbSmartAspects.AddInParameter(commandGR, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                                status = dbSmartAspects.ExecuteNonQuery(commandGR, transction);
                            }

                            using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateDocumentInfo_SP"))
                            {
                                commandDocument.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandDocument, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                                dbSmartAspects.AddInParameter(commandDocument, "@TempOwnerId", DbType.String, registrationDetailBO.tempOwnerId);
                                status = dbSmartAspects.ExecuteNonQuery(commandDocument, transction);
                            }


                            using (DbCommand commandPreference = dbSmartAspects.GetStoredProcCommand("SaveGuestPreferenceMappingInfo_SP"))
                            {
                                string preferences = string.Join(",", preferenList.Select(i => i.PreferenceId));

                                commandPreference.Parameters.Clear();

                                dbSmartAspects.AddInParameter(commandPreference, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                                dbSmartAspects.AddInParameter(commandPreference, "@PreferenceIdList", DbType.String, preferences);

                                status = dbSmartAspects.ExecuteNonQuery(commandPreference, transction);

                            }

                        }
                        transction.Commit();

                    }
                }
            }
            return true;
        }
        public bool SaveTemporaryPaxInInfo(GuestInformationBO registrationDetailBO, string tempRegId, DateTime checkInDate, decimal paxInRate, out int TempGuestId)
        {
            bool retVal = false;
            int status = 0;
            TempGuestId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveGuestInformationNew_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        if (registrationDetailBO.GuestId <= 0)
                        {
                            commandMaster.Parameters.Clear();
                            dbSmartAspects.AddInParameter(commandMaster, "@RegistrationId", DbType.Int32, tempRegId);
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
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckInDate", DbType.DateTime, checkInDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@PaxInRate", DbType.Decimal, paxInRate);
                            dbSmartAspects.AddInParameter(commandMaster, "@AdditionalRemarks", DbType.String, registrationDetailBO.AdditionalRemarks);
                            dbSmartAspects.AddOutParameter(commandMaster, "@TempGuestId", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            TempGuestId = Convert.ToInt32(commandMaster.Parameters["@TempGuestId"].Value);
                            using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateDocumentInfo_SP"))
                            {
                                commandDocument.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandDocument, "@GuestId", DbType.Int32, TempGuestId);
                                dbSmartAspects.AddInParameter(commandDocument, "@TempOwnerId", DbType.String, registrationDetailBO.tempOwnerId);
                                status = dbSmartAspects.ExecuteNonQuery(commandDocument, transction);
                            }
                        }
                        else
                        {
                            GuestInformationDA infoDA = new GuestInformationDA();
                            bool success = infoDA.UpdateGuestInformationWithOutDocument(registrationDetailBO);

                            using (DbCommand commandGR = dbSmartAspects.GetStoredProcCommand("SaveGuestRegistrationForPaxInInfo_SP"))
                            {
                                commandGR.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandGR, "@RegistrationId", DbType.Int32, tempRegId);
                                dbSmartAspects.AddInParameter(commandGR, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                                dbSmartAspects.AddInParameter(commandGR, "@PaxInRate", DbType.Decimal, paxInRate);
                                status = dbSmartAspects.ExecuteNonQuery(commandGR, transction);
                            }

                            using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateDocumentInfo_SP"))
                            {
                                commandDocument.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandDocument, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                                dbSmartAspects.AddInParameter(commandDocument, "@TempOwnerId", DbType.String, registrationDetailBO.tempOwnerId);
                                status = dbSmartAspects.ExecuteNonQuery(commandDocument, transction);
                            }
                        }
                        transction.Commit();

                    }
                }
            }
            return true;
        }
        public bool DeleteTempGuestRegistration(int tempRegId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteTempGuestRegistration_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, tempRegId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool DeleteTempGuestRegistrationInfoByGuestId(int tempRegId, int GuestId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteTempGuestRegistrationInfoByGuestId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, tempRegId);
                    dbSmartAspects.AddInParameter(command, "@GuestId", DbType.Int32, GuestId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public RoomRegistrationBO GetNotCheckedOutRoomRegistrationInfoById(int registrationId)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNotCheckedOutRoomRegistrationInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                roomRegistration.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                roomRegistration.IsGuestCheckedOut = Convert.ToInt32(reader["IsGuestCheckedOut"]);
                                roomRegistration.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomRegistration.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                roomRegistration.EntitleRoomType = Convert.ToInt32(reader["EntitleRoomType"]);
                                roomRegistration.CurrencyType = Convert.ToInt32(reader["CurrencyType"]);
                                roomRegistration.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                roomRegistration.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                roomRegistration.DiscountType = reader["DiscountType"].ToString();
                                roomRegistration.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                roomRegistration.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                roomRegistration.IsCompanyGuest = Convert.ToBoolean(reader["IsCompanyGuest"]);
                                roomRegistration.CommingFrom = reader["CommingFrom"].ToString();
                                roomRegistration.NextDestination = reader["NextDestination"].ToString();
                                roomRegistration.VisitPurpose = reader["VisitPurpose"].ToString();
                                roomRegistration.IsFromReservation = Convert.ToBoolean(reader["IsFromReservation"]);
                                roomRegistration.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomRegistration.ReservationInfo = reader["ReservationInfo"].ToString();
                                roomRegistration.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                roomRegistration.IsListedCompany = Convert.ToBoolean(reader["IsListedCompany"]);
                                roomRegistration.ReservedCompany = reader["ReservedCompany"].ToString();
                                roomRegistration.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                roomRegistration.ContactPerson = reader["ContactPerson"].ToString();
                                roomRegistration.ContactNumber = reader["ContactNumber"].ToString();
                                roomRegistration.PaymentMode = reader["PaymentMode"].ToString();
                                roomRegistration.PayFor = Convert.ToInt32(reader["PayFor"]);
                                roomRegistration.BusinessPromotionId = Convert.ToInt32(reader["BusinessPromotionId"]);
                                roomRegistration.IsRoomOwner = Convert.ToInt32(reader["IsRoomOwner"]);
                                roomRegistration.GuestSourceId = Convert.ToInt32(reader["GuestSourceId"]);
                                roomRegistration.IsReturnedGuest = Convert.ToBoolean(reader["IsReturnedGuest"]);
                                roomRegistration.ReferenceId = Convert.ToInt32(reader["ReferenceId"]);
                                roomRegistration.APDId = Convert.ToInt32(reader["APDId"]);
                                roomRegistration.ArrivalFlightName = reader["ArrivalFlightName"].ToString();
                                roomRegistration.ArrivalFlightNumber = reader["ArrivalFlightNumber"].ToString();
                                if (roomRegistration.APDId != 0)
                                {
                                    roomRegistration.ArrivalTime = Convert.ToDateTime(currentDate + " " + reader["ArrivalTime"]);
                                }
                                roomRegistration.DepartureFlightName = reader["DepartureFlightName"].ToString();
                                roomRegistration.DepartureFlightNumber = reader["DepartureFlightNumber"].ToString();
                                if (roomRegistration.APDId != 0)
                                {
                                    roomRegistration.DepartureTime = Convert.ToDateTime(currentDate + " " + reader["DepartureTime"]);
                                }
                            }
                        }
                    }
                }
            }
            return roomRegistration;
        }
        public List<RoomRegistrationBO> GetGuestRegistrationHistoryByGuestId(int GuestId)
        {
            List<RoomRegistrationBO> roomRegistrationList = new List<RoomRegistrationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestRegistrationHistoryByGuestId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GuestId", DbType.Int32, GuestId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
                                roomRegistration.RegistrationId = Int32.Parse(reader["RegistrationId"].ToString());
                                roomRegistration.GuestName = reader["GuestName"].ToString(); ;
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.OriginalArriveDate = Convert.ToDateTime(reader["OriginalArriveDate"]);
                                roomRegistration.OriginalArriveDateDisplay = reader["OriginalArriveDateDisplay"].ToString();
                                roomRegistration.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                roomRegistration.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                int isCancelCheckOut = 0;
                                isCancelCheckOut = Convert.ToInt32(reader["IsRoomNumberCheckoutOrRegistationAfterCurrentGuestCheckOut"].ToString());
                                if (isCancelCheckOut == 0)
                                {
                                    roomRegistration.IsRoomNumberCheckoutOrRegistationAfterCurrentGuestCheckOut = true;
                                }
                                else
                                {
                                    roomRegistration.IsRoomNumberCheckoutOrRegistationAfterCurrentGuestCheckOut = false;
                                }

                                var str = reader["CheckOutDate"].ToString();
                                if (!string.IsNullOrEmpty(reader["CheckOutDate"].ToString()))
                                {
                                    roomRegistration.CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"]);
                                }
                                roomRegistration.CheckOutDateDisplay = reader["CheckOutDateDisplay"].ToString();

                                roomRegistration.LocalCurrencyHead = reader["LocalCurrencyHead"].ToString();
                                roomRegistration.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                roomRegistrationList.Add(roomRegistration);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public RoomRegistrationBO GetGuestLastRegistrationByGuestId(int GuestId)
        {
            RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestLastRegistrationByGuestId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GuestId", DbType.Int32, GuestId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomRegistration.RegistrationId = Int32.Parse(reader["RegistrationId"].ToString());
                                roomRegistration.GuestId = Int32.Parse(reader["GuestId"].ToString());
                                roomRegistration.GuestName = reader["GuestName"].ToString(); ;
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                roomRegistration.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                var str = reader["CheckOutDate"].ToString();
                                if (!string.IsNullOrEmpty(reader["CheckOutDate"].ToString()))
                                {
                                    roomRegistration.CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"]);
                                }
                                roomRegistration.PaxInRate = Convert.ToDecimal(reader["PaxInRate"]);
                                roomRegistration.CountryName = reader["CountryName"].ToString(); ;
                            }
                        }
                    }
                }
            }
            return roomRegistration;
        }
        public List<RoomAlocationBO> GetActiveRegistrationInfoBySearchCriteria(string RoomNumber, string GueastName, DateTime CheckInDate, string CompanyName, int CountryId, string registrationNo)
        {
            List<RoomAlocationBO> roomAllocationBOList = new List<RoomAlocationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveRegistrationInfoBySearchCriteria_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(GueastName))
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, GueastName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(CompanyName))
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, CompanyName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, DBNull.Value);

                    if (CheckInDate != null)
                    {
                        if (CheckInDate.Date <= DateTime.Now.Date)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@CheckInDate", DbType.DateTime, CheckInDate);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@CheckInDate", DbType.DateTime, DBNull.Value);
                        }
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CheckInDate", DbType.DateTime, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(RoomNumber))
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, RoomNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, DBNull.Value);

                    if (CountryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int32, CountryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(registrationNo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RegistrationNo", DbType.String, registrationNo);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RegistrationNo", DbType.String, DBNull.Value);
                    }

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();

                                roomAllocationBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomAllocationBO.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomAllocationBO.GuestName = reader["GuestName"].ToString();
                                roomAllocationBO.RoomType = reader["RoomType"].ToString();
                                roomAllocationBO.RoomRate = Decimal.Parse(reader["RoomRate"].ToString());
                                roomAllocationBO.RoomTypeId = Int32.Parse(reader["RoomTypeId"].ToString());
                                roomAllocationBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomAllocationBO.RoomNumber = reader["RoomNumber"].ToString();
                                roomAllocationBO.NodeId = Convert.ToInt32(reader["NodeId"]);

                                roomAllocationBOList.Add(roomAllocationBO);
                            }
                        }
                    }
                }
            }

            return roomAllocationBOList;
        }
        public RoomRegistrationBO GetRoomRegistrationInfoByReservationId(int ReservationId)
        {
            RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomRegistrationInfoByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, ReservationId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                roomRegistration.RegistrationId = Int32.Parse(reader["RegistrationId"].ToString());
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                            }
                        }
                    }
                }
            }
            return roomRegistration;
        }
        public List<GuestLedgerTranscriptReportBO> GetGuestLedgerTranscriptReport()
        {
            List<GuestLedgerTranscriptReportBO> ledgerTranscriptReport = new List<GuestLedgerTranscriptReportBO>();

            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInHouseGuestLedgerInformationForReport_SP"))
            {
                cmd.CommandTimeout = 10000;
                DataSet salarySheetDS = new DataSet();
                dbSmartAspects.LoadDataSet(cmd, salarySheetDS, "SalarySheet");

                DataTable Table = salarySheetDS.Tables["SalarySheet"];

                ledgerTranscriptReport = Table.AsEnumerable().Select(r => new GuestLedgerTranscriptReportBO
                {
                    RegistrationId = r.Field<int>("RegistrationId"),
                    RoomNumber = r.Field<int>("RoomNumber"),
                    GuestName = r.Field<string>("GuestName"),
                    CompanyName = r.Field<string>("CompanyName"),
                    ServiceName = r.Field<string>("ServiceName").Trim(),
                    ServiceAmount = r.Field<decimal>("ServiceAmount")

                }).ToList();

            }

            return ledgerTranscriptReport;
        }
        public List<InhouseGuestLedgerBO> GetInHouseGuestLedgerForReport(string companyName, string companyAddress, string companyWeb)
        {
            List<InhouseGuestLedgerBO> roomRegistrationList = new List<InhouseGuestLedgerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInHouseGuestLedgerForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyAddress", DbType.String, companyAddress);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyWeb", DbType.String, companyWeb);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InhouseGuestLedgerBO roomRegistration = new InhouseGuestLedgerBO();
                                roomRegistration.HMCompanyProfile = reader["HMCompanyProfile"].ToString();
                                roomRegistration.HMCompanyAddress = reader["HMCompanyAddress"].ToString();
                                roomRegistration.HMCompanyWeb = reader["HMCompanyWeb"].ToString();
                                roomRegistration.PrintDate = Convert.ToDateTime(reader["PrintDate"]);
                                roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomRegistration.RoomNumber = Convert.ToInt32(reader["RoomNumber"].ToString());
                                roomRegistration.GuestName = reader["GuestName"].ToString();
                                roomRegistration.CompanyName = reader["CompanyName"].ToString();
                                roomRegistration.GuestReference = reader["GuestReference"].ToString();
                                roomRegistration.Pay = reader["Pay"].ToString();
                                roomRegistration.Araival = Convert.ToString(reader["Araival"]);
                                roomRegistration.Departure = Convert.ToString(reader["Departure"]);
                                roomRegistration.RoomRent = Convert.ToDecimal(reader["RoomRent"]);
                                roomRegistration.Amount = Convert.ToDecimal(reader["Amount"]);
                                roomRegistration.Credit = Convert.ToDecimal(reader["Credit"]);
                                roomRegistration.Balance = Convert.ToDecimal(reader["Balance"]);
                                roomRegistration.TotalRoom = Convert.ToInt32(reader["TotalRoom"]);
                                roomRegistration.TotalPerson = Convert.ToInt32(reader["TotalPerson"]);

                                if (!string.IsNullOrEmpty(reader["SatyedNights"].ToString()))
                                    roomRegistration.SatyedNights = Convert.ToInt32(reader["SatyedNights"].ToString());

                                roomRegistrationList.Add(roomRegistration);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public RoomRegistrationBO GetMinimumArrivalDateInfoByRegistrationIdList(string registrationIdList)
        {
            RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMinimumArrivalDateInfoByRegistrationIdList_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationIdList);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomRegistration.ArriveDate = Convert.ToDateTime(reader["ArriveDate"].ToString());
                            }
                        }
                    }
                }
            }
            return roomRegistration;
        }
        public RoomRegistrationBO GetRackRateServiceChargeVatInformation(string negotiatedRoomRate, string isServiceChargeEnable, string isVatEnable)
        {
            RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRackRateServiceChargeVatInformation_SP"))
                {
                    decimal roomRate;
                    bool res = decimal.TryParse(negotiatedRoomRate, out roomRate);
                    if (res == false)
                    {
                        // String is not a number.
                        roomRate = 0;
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RegotiatedRoomRate", DbType.Decimal, roomRate);
                    dbSmartAspects.AddInParameter(cmd, "@IsServiceChargeEnable", DbType.Int32, Convert.ToInt32(isServiceChargeEnable));
                    dbSmartAspects.AddInParameter(cmd, "@IsVatEnable", DbType.Int32, Convert.ToInt32(isVatEnable));
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomRegistration.RackRate = Convert.ToDecimal(reader["TotalRoomRate"].ToString());
                                roomRegistration.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"].ToString());
                                roomRegistration.VatAmount = Convert.ToDecimal(reader["VatAmount"].ToString());
                            }
                        }
                    }
                }
            }
            return roomRegistration;
        }
        public List<ComplimentaryGuestReportViewBO> GetComplimentaryGuestInfo(DateTime DateFrom, DateTime DateTo, string guestName, string companyName, string passportNo, string mobileNo)
        {
            List<ComplimentaryGuestReportViewBO> guestInfo = new List<ComplimentaryGuestReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetComplimentaryGuestInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DateTo);
                    if (!string.IsNullOrWhiteSpace(guestName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, guestName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(companyName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(passportNo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PassportNo", DbType.String, passportNo);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PassportNo", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(mobileNo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MobileNo", DbType.String, mobileNo);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MobileNo", DbType.String, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ComplimentaryGuest");
                    DataTable Table = ds.Tables["ComplimentaryGuest"];
                    guestInfo = Table.AsEnumerable().Select(r =>
                                new ComplimentaryGuestReportViewBO
                                {
                                    CurrentDate = r.Field<DateTime>("CurrentDate"),
                                    CheckInDate = r.Field<DateTime?>("CheckInDate"),
                                    CheckOutDate = r.Field<DateTime?>("CheckOutDate"),
                                    GuestName = r.Field<string>("GuestName"),
                                    CompanyName = r.Field<string>("CompanyName"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    Remarks = r.Field<string>("Remarks")

                                }).ToList();
                }

                return guestInfo;
            }
        }
        public List<GuestHouseInfoForReportBO> GetFrontOfficeExtraBedInfoForReport(DateTime DateFrom, DateTime DateTo)
        {
            List<GuestHouseInfoForReportBO> guestInfo = new List<GuestHouseInfoForReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFrontOfficeExtraBedInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DateTo);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ComplimentaryGuest");
                    DataTable Table = ds.Tables["ComplimentaryGuest"];
                    guestInfo = Table.AsEnumerable().Select(r =>
                                new GuestHouseInfoForReportBO
                                {
                                    RRNumber = r.Field<string>("RRNumber"),
                                    GuestName = r.Field<string>("GuestName"),
                                    GroupName = r.Field<string>("GroupName"),
                                    VipGuestTypeId = r.Field<int?>("VipGuestTypeId"),
                                    NumberOfPersonAdult = r.Field<int?>("NumberOfPersonAdult"),
                                    NumberOfPersonChild = r.Field<int?>("NumberOfPersonChild"),
                                    GuestNationality = r.Field<string>("GuestNationality"),
                                    GuestCompany = r.Field<string>("GuestCompany"),
                                    TotalPerson = r.Field<int?>("TotalPerson"),
                                    DateIn = r.Field<string>("DateIn"),
                                    DateOut = r.Field<string>("DateOut"),
                                    ProbableArrivalTime = r.Field<string>("ProbableArrivalTime"),
                                    RoomType = r.Field<string>("RoomType"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    Remarks = r.Field<string>("Remarks")
                                }).ToList();
                }

                return guestInfo;
            }
        }
        public Boolean UpdateGuestPaxInRateInfo(int roomRegistrationId, int guestId, decimal paxInRate, int lastModifiedBy)
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateGuestPaxInRateInfo_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@RegistrationId", DbType.Int32, roomRegistrationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, guestId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PaxInRate", DbType.Decimal, paxInRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        public List<RoomAlocationBO> GetRoomRegistrationInformationBySearchCriteriaForPaging(string roomNumber, string guestName, DateTime? checkInDate, string companyName, int countryId, string registrationNo, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<RoomAlocationBO> roomregistrationList = new List<RoomAlocationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomRegistrationInfoBySearchCriteriaForpagination_SP"))
                {

                    if (!string.IsNullOrWhiteSpace(guestName))
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, guestName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(companyName))
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, DBNull.Value);

                    if (checkInDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CheckInDate", DbType.DateTime, checkInDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CheckInDate", DbType.DateTime, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(roomNumber))
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, DBNull.Value);

                    if (countryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int32, countryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(registrationNo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RegistrationNo", DbType.String, registrationNo);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RegistrationNo", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RoomAlocationBO");
                    DataTable Table = ds.Tables["RoomAlocationBO"];
                    roomregistrationList = Table.AsEnumerable().Select(r =>
                                new RoomAlocationBO
                                {
                                    RegistrationId = r.Field<int>("RegistrationId"),
                                    RegistrationNumber = r.Field<string>("RegistrationNumber"),
                                    GuestName = r.Field<string>("GuestName")

                                }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return roomregistrationList;
        }
        public InhouseGuestLedgerBO GetInhouseGuestLedgerDateInfo()
        {
            InhouseGuestLedgerBO roomRegistration = new InhouseGuestLedgerBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInhouseGuestLedgerDateInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomRegistration.InhouseGuestLedgerDate = Convert.ToDateTime(reader["InhouseGuestLedgerDate"]);
                            }
                        }
                    }
                }
            }
            return roomRegistration;
        }
        public InhouseGuestLedgerBO GetIsPreviousDayTransaction(DateTime transactionDate)
        {
            InhouseGuestLedgerBO roomRegistration = new InhouseGuestLedgerBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetIsPreviousDayTransaction_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TransactionDate", DbType.DateTime, transactionDate);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomRegistration.TransactionDate = Convert.ToDateTime(reader["TransactionDate"]);
                            }
                        }
                    }
                }
            }
            return roomRegistration;
        }
        public List<RoomRegistrationBO> GetTodaysRegistrationDetailInfo()
        {
            List<RoomRegistrationBO> roomRegistrationList = new List<RoomRegistrationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTodaysRegistrationDetailInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
                                roomRegistration.RegistrationId = Int32.Parse(reader["RegistrationId"].ToString());
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.RoomId = Int32.Parse(reader["RoomId"].ToString());

                                roomRegistrationList.Add(roomRegistration);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public List<RegistrationCardInfoBO> GetRoomRegistrationDetailByRegistrationId(int registrationId, string companyName, string companyAddress, string companyWeb)
        {
            List<RegistrationCardInfoBO> roomRegistrationList = new List<RegistrationCardInfoBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomRegistrationDetailByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyAddress", DbType.String, companyAddress);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyWeb", DbType.String, companyWeb);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RegistrationCardInfoBO roomRegistration = new RegistrationCardInfoBO();

                                roomRegistration.HMCompanyProfile = reader["HMCompanyProfile"].ToString();
                                roomRegistration.HMCompanyAddress = reader["HMCompanyAddress"].ToString();
                                roomRegistration.HMCompanyWeb = reader["HMCompanyWeb"].ToString();
                                roomRegistration.CompanyCountryId = Convert.ToInt32(reader["CompanyCountryId"]);
                                roomRegistration.GuestName = reader["GuestName"].ToString();
                                roomRegistration.GuestDOB = reader["GuestDOB"].ToString();
                                roomRegistration.GuestSex = reader["GuestSex"].ToString();
                                roomRegistration.GuestEmail = reader["GuestEmail"].ToString();
                                roomRegistration.GuestPhone = reader["GuestPhone"].ToString();
                                roomRegistration.GuestAddress1 = reader["GuestAddress1"].ToString();
                                roomRegistration.GuestAddress2 = reader["GuestAddress2"].ToString();
                                roomRegistration.GuestCity = reader["GuestCity"].ToString();
                                roomRegistration.CountryName = reader["CountryName"].ToString();
                                roomRegistration.GuestNationality = reader["GuestNationality"].ToString();
                                roomRegistration.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                roomRegistration.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                roomRegistration.NationalId = reader["NationalId"].ToString();
                                roomRegistration.PassportNumber = reader["PassportNumber"].ToString();
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.CommingFrom = reader["CommingFrom"].ToString();
                                roomRegistration.NextDestination = reader["NextDestination"].ToString();
                                roomRegistration.VisitPurpose = reader["VisitPurpose"].ToString();
                                roomRegistration.GuestZipCode = reader["GuestZipCode"].ToString();
                                roomRegistration.PIssueDate = reader["PIssueDate"].ToString();
                                roomRegistration.PIssuePlace = reader["PIssuePlace"].ToString();
                                roomRegistration.PExpireDate = reader["PExpireDate"].ToString();
                                roomRegistration.VisaNumber = reader["VisaNumber"].ToString();
                                roomRegistration.VIssueDate = reader["VIssueDate"].ToString();
                                roomRegistration.VExpireDate = reader["VExpireDate"].ToString();
                                roomRegistration.RoomNumber = reader["RoomNumber"].ToString();
                                roomRegistration.RoomType = reader["RoomType"].ToString();
                                roomRegistration.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"].ToString());
                                roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"].ToString());
                                roomRegistration.ArriveDate = reader["ArriveDate"].ToString();
                                roomRegistration.ExpectedCheckOutDate = reader["ExpectedCheckOutDate"].ToString();
                                roomRegistration.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                roomRegistration.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                roomRegistration.CurrencyTypeId = Convert.ToInt32(reader["CurrencyTypeId"].ToString());
                                roomRegistration.CurrencyType = reader["CurrencyType"].ToString();
                                roomRegistration.EntRoomType = reader["EntRoomType"].ToString();
                                roomRegistration.EntRoomRate = Convert.ToDecimal(reader["EntRoomRate"]);
                                roomRegistrationList.Add(roomRegistration);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public List<RegistrationCardInfoBO> GetPreRegistrationCardByReservationId(int reservationId)
        {
            List<RegistrationCardInfoBO> roomRegistrationList = new List<RegistrationCardInfoBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPreRegistrationCardByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RegistrationCardInfoBO roomRegistration = new RegistrationCardInfoBO();

                                roomRegistration.HMCompanyProfile = reader["HMCompanyProfile"].ToString();
                                roomRegistration.HMCompanyAddress = reader["HMCompanyAddress"].ToString();
                                roomRegistration.HMCompanyWeb = reader["HMCompanyWeb"].ToString();
                                roomRegistration.CompanyCountryId = Convert.ToInt32(reader["CompanyCountryId"]);
                                roomRegistration.GuestName = reader["GuestName"].ToString();
                                roomRegistration.GuestDOB = reader["GuestDOB"].ToString();
                                roomRegistration.GuestSex = reader["GuestSex"].ToString();
                                roomRegistration.GuestEmail = reader["GuestEmail"].ToString();
                                roomRegistration.GuestPhone = reader["GuestPhone"].ToString();
                                roomRegistration.GuestAddress1 = reader["GuestAddress1"].ToString();
                                roomRegistration.GuestAddress2 = reader["GuestAddress2"].ToString();
                                roomRegistration.GuestCity = reader["GuestCity"].ToString();
                                roomRegistration.CountryName = reader["CountryName"].ToString();
                                roomRegistration.GuestNationality = reader["GuestNationality"].ToString();
                                roomRegistration.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                roomRegistration.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                roomRegistration.NationalId = reader["NationalId"].ToString();
                                roomRegistration.PassportNumber = reader["PassportNumber"].ToString();
                                roomRegistration.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomRegistration.GuestZipCode = reader["GuestZipCode"].ToString();
                                roomRegistration.PIssueDate = reader["PIssueDate"].ToString();
                                roomRegistration.PIssuePlace = reader["PIssuePlace"].ToString();
                                roomRegistration.PExpireDate = reader["PExpireDate"].ToString();
                                roomRegistration.VisaNumber = reader["VisaNumber"].ToString();
                                roomRegistration.VIssueDate = reader["VIssueDate"].ToString();
                                roomRegistration.VExpireDate = reader["VExpireDate"].ToString();
                                roomRegistration.RoomNumber = reader["RoomNumber"].ToString();
                                roomRegistration.RoomType = reader["RoomType"].ToString();
                                roomRegistration.ArriveDate = reader["ArriveDate"].ToString();
                                roomRegistration.ExpectedCheckOutDate = reader["ExpectedCheckOutDate"].ToString();
                                roomRegistration.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                roomRegistration.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                roomRegistration.CurrencyTypeId = Convert.ToInt32(reader["CurrencyTypeId"].ToString());
                                roomRegistration.CurrencyType = reader["CurrencyType"].ToString();
                                roomRegistration.ArrivalFlightName = reader["ArrivalFlightName"].ToString();
                                roomRegistration.ArrivalFlightNumber = reader["ArrivalFlightNumber"].ToString();
                                roomRegistration.ETA = reader["ETA"].ToString();
                                roomRegistration.DepartureFlightName = reader["DepartureFlightName"].ToString();
                                roomRegistration.DepartureFlightNumber = reader["DepartureFlightNumber"].ToString();
                                roomRegistration.ETD = reader["ETD"].ToString();
                                roomRegistration.TermsAndConditions = reader["TermsAndConditions"].ToString();

                                //roomRegistration.EntRoomRate = Convert.ToDecimal(reader["EntRoomRate"]);
                                roomRegistration.DurationOfStay = Convert.ToInt32(reader["DurationOfStay"].ToString());
                                //roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"].ToString());
                                //roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"].ToString());

                                roomRegistrationList.Add(roomRegistration);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public List<RegistrationCardInfoBO> GetRegistrationCardInfoByIdNType(Int64 transactionId, string transactionType)
        {
            List<RegistrationCardInfoBO> roomRegistrationList = new List<RegistrationCardInfoBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRegistrationCardInfoByIdNType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.Int64, transactionId);
                    dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transactionType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RegistrationCardInfoBO roomRegistration = new RegistrationCardInfoBO();

                                roomRegistration.HMCompanyProfile = reader["HMCompanyProfile"].ToString();
                                roomRegistration.HMCompanyAddress = reader["HMCompanyAddress"].ToString();
                                roomRegistration.HMCompanyWeb = reader["HMCompanyWeb"].ToString();
                                roomRegistration.ReservationNumber = reader["ReservationNumber"].ToString();
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.RoomType = reader["RoomType"].ToString();
                                roomRegistration.RoomNumber = reader["RoomNumber"].ToString();
                                roomRegistration.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                roomRegistration.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                roomRegistration.CurrencyTypeId = Convert.ToInt32(reader["CurrencyTypeId"].ToString());
                                roomRegistration.CurrencyType = reader["CurrencyType"].ToString();
                                roomRegistration.PaymentMode = reader["PaymentMode"].ToString();
                                roomRegistration.ArriveDate = reader["ArriveDate"].ToString();
                                roomRegistration.ArrivalFlightName = reader["ArrivalFlightName"].ToString();
                                roomRegistration.ArrivalFlightNumber = reader["ArrivalFlightNumber"].ToString();
                                roomRegistration.ArrivalFlightTime = reader["ArrivalFlightTime"].ToString();
                                roomRegistration.ExpectedCheckOutDate = reader["ExpectedCheckOutDate"].ToString();
                                roomRegistration.DepartureFlightName = reader["DepartureFlightName"].ToString();
                                roomRegistration.DepartureFlightNumber = reader["DepartureFlightNumber"].ToString();
                                roomRegistration.DepartureFlightTime = reader["DepartureFlightTime"].ToString();
                                roomRegistration.DurationOfStay = Convert.ToInt32(reader["DurationOfStay"].ToString());
                                roomRegistration.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"].ToString());
                                roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"].ToString());
                                roomRegistration.CompanyName = reader["CompanyName"].ToString();
                                roomRegistration.GuestAddress = reader["GuestAddress"].ToString();
                                roomRegistration.GuestName = reader["GuestName"].ToString();
                                roomRegistration.GuestPhone = reader["GuestPhone"].ToString();
                                roomRegistration.GuestEmail = reader["GuestEmail"].ToString();
                                roomRegistration.GuestSex = reader["GuestSex"].ToString();
                                roomRegistration.GuestDOB = reader["GuestDOB"].ToString();
                                roomRegistration.CompanyCountryId = Convert.ToInt32(reader["CompanyCountryId"]);
                                roomRegistration.CountryName = reader["CountryName"].ToString();
                                roomRegistration.GuestNationality = reader["GuestNationality"].ToString();
                                roomRegistration.GuestCity = reader["GuestCity"].ToString();
                                roomRegistration.GuestZipCode = reader["GuestZipCode"].ToString();
                                roomRegistration.VisitPurpose = reader["VisitPurpose"].ToString();
                                roomRegistration.CommingFrom = reader["CommingFrom"].ToString();
                                roomRegistration.NextDestination = reader["NextDestination"].ToString();
                                roomRegistration.ProfessionName = reader["ProfessionName"].ToString();
                                roomRegistration.NationalId = reader["NationalId"].ToString();
                                roomRegistration.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                roomRegistration.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                roomRegistration.PassportNumber = reader["PassportNumber"].ToString();
                                roomRegistration.PIssuePlace = reader["PIssuePlace"].ToString();
                                roomRegistration.PIssueDate = reader["PIssueDate"].ToString();
                                roomRegistration.PExpireDate = reader["PExpireDate"].ToString();
                                roomRegistration.VisaNumber = reader["VisaNumber"].ToString();
                                roomRegistration.VIssueDate = reader["VIssueDate"].ToString();
                                roomRegistration.VExpireDate = reader["VExpireDate"].ToString();
                                roomRegistration.SpecialInstructions = reader["SpecialInstructions"].ToString();

                                roomRegistrationList.Add(roomRegistration);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public List<RegistrationCardInfoBO> GetBlankRoomRegistrationDetailByRegistrationId(int registrationId, string companyName, string companyAddress, string companyWeb)
        {
            List<RegistrationCardInfoBO> roomRegistrationList = new List<RegistrationCardInfoBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBlankRoomRegistrationDetailByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyAddress", DbType.String, companyAddress);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyWeb", DbType.String, companyWeb);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RegistrationCardInfoBO roomRegistration = new RegistrationCardInfoBO();

                                roomRegistration.HMCompanyProfile = reader["HMCompanyProfile"].ToString();
                                roomRegistration.HMCompanyAddress = reader["HMCompanyAddress"].ToString();
                                roomRegistration.HMCompanyWeb = reader["HMCompanyWeb"].ToString();
                                roomRegistration.CompanyCountryId = Convert.ToInt32(reader["CompanyCountryId"]);
                                roomRegistration.GuestName = reader["GuestName"].ToString();
                                roomRegistration.GuestDOB = reader["GuestDOB"].ToString();
                                roomRegistration.GuestSex = reader["GuestSex"].ToString();
                                roomRegistration.GuestEmail = reader["GuestEmail"].ToString();
                                roomRegistration.GuestPhone = reader["GuestPhone"].ToString();
                                roomRegistration.GuestAddress1 = reader["GuestAddress1"].ToString();
                                roomRegistration.GuestAddress2 = reader["GuestAddress2"].ToString();
                                roomRegistration.GuestCity = reader["GuestCity"].ToString();
                                roomRegistration.CountryName = reader["CountryName"].ToString();
                                roomRegistration.GuestNationality = reader["GuestNationality"].ToString();
                                roomRegistration.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                roomRegistration.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                roomRegistration.NationalId = reader["NationalId"].ToString();
                                roomRegistration.PassportNumber = reader["PassportNumber"].ToString();
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.CommingFrom = reader["CommingFrom"].ToString();
                                roomRegistration.NextDestination = reader["NextDestination"].ToString();
                                roomRegistration.VisitPurpose = reader["VisitPurpose"].ToString();
                                roomRegistration.GuestZipCode = reader["GuestZipCode"].ToString();
                                roomRegistration.PIssueDate = reader["PIssueDate"].ToString();
                                roomRegistration.PIssuePlace = reader["PIssuePlace"].ToString();
                                roomRegistration.PExpireDate = reader["PExpireDate"].ToString();
                                roomRegistration.VisaNumber = reader["VisaNumber"].ToString();
                                roomRegistration.VIssueDate = reader["VIssueDate"].ToString();
                                roomRegistration.VExpireDate = reader["VExpireDate"].ToString();
                                roomRegistration.RoomNumber = reader["RoomNumber"].ToString();
                                roomRegistration.RoomType = reader["RoomType"].ToString();
                                roomRegistration.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"].ToString());
                                roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"].ToString());
                                roomRegistration.ArriveDate = reader["ArriveDate"].ToString();
                                roomRegistration.ExpectedCheckOutDate = reader["ExpectedCheckOutDate"].ToString();
                                roomRegistration.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                roomRegistration.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                roomRegistration.CurrencyTypeId = Convert.ToInt32(reader["CurrencyTypeId"].ToString());
                                roomRegistration.CurrencyType = reader["CurrencyType"].ToString();
                                roomRegistration.EntRoomType = reader["EntRoomType"].ToString();
                                roomRegistration.EntRoomRate = Convert.ToDecimal(reader["EntRoomRate"]);
                                roomRegistrationList.Add(roomRegistration);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public RoomRegistrationBO GetHoldUpRoomRegistrationInfoById(int registrationId, DateTime searchDate)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHoldUpRoomRegistrationInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);
                    dbSmartAspects.AddInParameter(cmd, "@SearchDate", DbType.DateTime, searchDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                            }
                        }
                    }
                }
            }
            return roomRegistration;
        }
        public List<RoomRegistrationBO> GetHoldBillRoomRegistrations()
        {
            List<RoomRegistrationBO> roomRegistrationList = new List<RoomRegistrationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHoldBillRoomRegistrations_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();

                                roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                roomRegistration.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                roomRegistration.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomRegistration.EntitleRoomType = Convert.ToInt32(reader["EntitleRoomType"]);
                                roomRegistration.IsCompanyGuest = Convert.ToBoolean(reader["IsCompanyGuest"]);
                                roomRegistration.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                roomRegistration.CommingFrom = reader["CommingFrom"].ToString();
                                roomRegistration.NextDestination = reader["NextDestination"].ToString();
                                roomRegistration.VisitPurpose = reader["VisitPurpose"].ToString();
                                roomRegistration.IsFromReservation = Convert.ToBoolean(reader["IsFromReservation"]);
                                roomRegistration.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomRegistration.BusinessPromotionId = Convert.ToInt32(reader["BusinessPromotionId"]);

                                roomRegistrationList.Add(roomRegistration);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public List<RoomRegistrationBO> GetBlankRegistrationList()
        {
            List<RoomRegistrationBO> roomRegistrationList = new List<RoomRegistrationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBlankRegistrationList_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomRegistrationBO roomRegistration = new RoomRegistrationBO();

                                roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                roomRegistration.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                roomRegistration.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                roomRegistration.RoomId = Convert.ToInt32(reader["RoomId"]);
                                roomRegistration.EntitleRoomType = Convert.ToInt32(reader["EntitleRoomType"]);
                                roomRegistration.IsCompanyGuest = Convert.ToBoolean(reader["IsCompanyGuest"]);
                                roomRegistration.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                roomRegistration.CommingFrom = reader["CommingFrom"].ToString();
                                roomRegistration.NextDestination = reader["NextDestination"].ToString();
                                roomRegistration.VisitPurpose = reader["VisitPurpose"].ToString();
                                roomRegistration.IsFromReservation = Convert.ToBoolean(reader["IsFromReservation"]);
                                roomRegistration.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                roomRegistration.BusinessPromotionId = Convert.ToInt32(reader["BusinessPromotionId"]);

                                roomRegistrationList.Add(roomRegistration);
                            }
                        }
                    }
                }
            }
            return roomRegistrationList;
        }
        public Boolean UpdateHoldRoomRegistrationInfo(RoomRegistrationBO roomRegistration, List<RegistrationComplementaryItemBO> newlyAddedcomplementaryItem, List<RegistrationComplementaryItemBO> deletedComplementaryItem)
        {
            bool retVal = false;
            int status = 0;
            int tmpRegistrationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateHoldRoomRegistrationInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@RegistrationId", DbType.Int32, roomRegistration.RegistrationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ExpectedCheckOutDate", DbType.DateTime, roomRegistration.ExpectedCheckOutDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomId", DbType.Int32, roomRegistration.RoomId);
                        dbSmartAspects.AddInParameter(commandMaster, "@EntitleRoomType", DbType.Int32, roomRegistration.EntitleRoomType);
                        dbSmartAspects.AddInParameter(commandMaster, "@UnitPrice", DbType.Decimal, roomRegistration.UnitPrice);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomRate", DbType.Decimal, roomRegistration.RoomRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsServiceChargeEnable", DbType.Boolean, roomRegistration.IsServiceChargeEnable);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsVatAmountEnable", DbType.Boolean, roomRegistration.IsVatAmountEnable);
                        dbSmartAspects.AddInParameter(commandMaster, "@DiscountType", DbType.String, roomRegistration.DiscountType);
                        dbSmartAspects.AddInParameter(commandMaster, "@DiscountAmount", DbType.Decimal, roomRegistration.DiscountAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsCompanyGuest", DbType.Boolean, roomRegistration.IsCompanyGuest);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsHouseUseRoom", DbType.Boolean, roomRegistration.IsHouseUseRoom);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsFamilyOrCouple", DbType.Boolean, roomRegistration.IsFamilyOrCouple);
                        dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonAdult", DbType.Int32, roomRegistration.NumberOfPersonAdult);
                        dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonChild", DbType.Int32, roomRegistration.NumberOfPersonChild);
                        dbSmartAspects.AddInParameter(commandMaster, "@CommingFrom", DbType.String, roomRegistration.CommingFrom);
                        dbSmartAspects.AddInParameter(commandMaster, "@NextDestination", DbType.String, roomRegistration.NextDestination);
                        dbSmartAspects.AddInParameter(commandMaster, "@VisitPurpose", DbType.String, roomRegistration.VisitPurpose);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, roomRegistration.ContactPerson);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactNumber", DbType.String, roomRegistration.ContactNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsRoomOwner", DbType.Int32, roomRegistration.IsRoomOwner);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestSourceId", DbType.Int32, roomRegistration.GuestSourceId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReferenceId", DbType.Int32, roomRegistration.ReferenceId);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsReturnedGuest", DbType.Boolean, roomRegistration.IsReturnedGuest);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsVIPGuest", DbType.Boolean, roomRegistration.IsVIPGuest);
                        dbSmartAspects.AddInParameter(commandMaster, "@VIPGuestTypeId", DbType.Int32, roomRegistration.VIPGuestTypeId);
                        dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, roomRegistration.Remarks);

                        //--Aireport Pickup Drop Information-------------------------------------------
                        dbSmartAspects.AddInParameter(commandMaster, "@AirportPickUp", DbType.String, roomRegistration.AirportPickUp);
                        dbSmartAspects.AddInParameter(commandMaster, "@AirportDrop", DbType.String, roomRegistration.AirportDrop);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsAirportPickupDropExist", DbType.Int32, roomRegistration.IsAirportPickupDropExist);
                        dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightName", DbType.String, roomRegistration.ArrivalFlightName);
                        dbSmartAspects.AddInParameter(commandMaster, "@ArrivalFlightNumber", DbType.String, roomRegistration.ArrivalFlightNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@ArrivalTime", DbType.DateTime, roomRegistration.ArrivalTime);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightName", DbType.String, roomRegistration.DepartureFlightName);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartureFlightNumber", DbType.String, roomRegistration.DepartureFlightNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartureTime", DbType.DateTime, roomRegistration.DepartureTime);
                        //--Aireport Pickup Drop Information------------------------------End-------- 

                        //--Credit Card Information ------------
                        dbSmartAspects.AddInParameter(commandMaster, "@CardType", DbType.String, roomRegistration.CardType);
                        dbSmartAspects.AddInParameter(commandMaster, "@CardNumber", DbType.String, roomRegistration.CardNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@CardExpireDate", DbType.DateTime, roomRegistration.CardExpireDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@CardHolderName", DbType.String, roomRegistration.CardHolderName);
                        dbSmartAspects.AddInParameter(commandMaster, "@CardReference", DbType.String, roomRegistration.CardReference);
                        //--Credit Card Information End------------

                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, roomRegistration.LastModifiedBy);
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        tmpRegistrationId = roomRegistration.RegistrationId;
                    }

                    if (status > 0)
                    {
                        if (newlyAddedcomplementaryItem.Count() > 0 && status > 0)
                        {
                            using (DbCommand commandComplementary = dbSmartAspects.GetStoredProcCommand("SaveRegistrationComplementaryItemInfo_SP"))
                            {
                                foreach (RegistrationComplementaryItemBO complementaryItemBO in newlyAddedcomplementaryItem)
                                {
                                    commandComplementary.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandComplementary, "@RegistrationId", DbType.Int32, complementaryItemBO.RegistrationId);
                                    dbSmartAspects.AddInParameter(commandComplementary, "@ComplementaryItemId", DbType.Int32, complementaryItemBO.ComplementaryItemId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandComplementary, transction);
                                }
                            }
                        }

                        if (deletedComplementaryItem.Count() > 0 && status > 0)
                        {
                            using (DbCommand commandComplementaryDelete = dbSmartAspects.GetStoredProcCommand("DeleteRegistrationComplementaryItemInfo_SP"))
                            {
                                foreach (RegistrationComplementaryItemBO complementaryItemBO in deletedComplementaryItem)
                                {
                                    commandComplementaryDelete.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandComplementaryDelete, "@RegistrationId", DbType.Int32, complementaryItemBO.RegistrationId);
                                    dbSmartAspects.AddInParameter(commandComplementaryDelete, "@ComplementaryItemId", DbType.Int32, complementaryItemBO.ComplementaryItemId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandComplementaryDelete, transction);
                                }
                            }
                        }
                    }

                    if (status > 0)
                    {
                        transction.Commit();
                        retVal = true;
                    }
                    else
                    {
                        transction.Rollback();
                        retVal = false;
                    }
                }

            }
            return retVal;
        }
        public bool CancelBlankRegistration(int tempRegId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CancelBlankRegistration_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, tempRegId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public RoomRegistrationBO GetGuestCheckedOutInfoByRegistrationId(int registrationId)
        {
            //string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            RoomRegistrationBO roomRegistration = new RoomRegistrationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestCheckedOutInfoByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomRegistration.CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"]);
                            }
                        }
                    }
                }
            }
            return roomRegistration;
        }
        public int SaveUpdateGuestBillPymentForPaymentTypeChange(DateTime restaurantBillDate, List<GuestBillPaymentBO> guestPaymentDetailList, string deletedPaymentIds, string deletedTransferedPaymentIds, string deletedAchievementPaymentIds, int billID)
        {
            int status = 0;
            int companyId = 0;
            bool isWorkAny = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    status = 1;

                    if (guestPaymentDetailList != null)
                    {
                        using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                        {
                            foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                            {
                                if (guestBillPaymentBO.PaymentMode != "Other Room")
                                {
                                    if (guestBillPaymentBO.ServiceBillId == null) // guestBillPaymentBO.PaidServiceId == 0
                                    {
                                        commandGuestBillPayment.Parameters.Clear();
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        if (guestBillPaymentBO.CompanyId != null)
                                        {
                                            companyId = guestBillPaymentBO.CompanyId;
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, restaurantBillDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "FrontOffice");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, billID);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, 0);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, companyId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, string.Empty);

                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            if (guestBillPaymentBO.ExpireDate != null)
                                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));

                                        if (status > 0)
                                        {
                                            status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                            isWorkAny = true;
                                        }
                                    }
                                }

                                //if (guestBillPaymentBO.PaymentMode == "Other Room" && status > 0)
                                //{
                                //    using (DbCommand commandGuestBillPayment2 = dbSmartAspects.GetStoredProcCommand("UpdateGuestHouseCheckOutPaymentChangeforOtherRoomPayment_SP"))
                                //    {
                                //        if (guestBillPaymentBO.ServiceBillId == null)
                                //        {
                                //            commandGuestBillPayment.Parameters.Clear();

                                //            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                //            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@BillPaidBy", DbType.Int32, guestBillPaymentBO.BillPaidBy);
                                //            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);

                                //            if (status > 0)
                                //            {
                                //                status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment2);
                                //                isWorkAny = true;
                                //            }
                                //        }
                                //    }
                                //}
                            }
                        }

                        if (!string.IsNullOrEmpty(deletedPaymentIds) && status > 0)
                        {
                            string[] paymentId = deletedPaymentIds.Split(',');

                            using (DbCommand commandDeletePayment = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                for (int i = 0; i < paymentId.Count(); i++)
                                {
                                    commandDeletePayment.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@TableName", DbType.String, "HotelGuestBillPayment");
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@TablePKField", DbType.String, "PaymentId");
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@TablePKId", DbType.String, paymentId[i]);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePayment, transction);
                                    isWorkAny = true;
                                }
                            }
                        }

                        //-----------------------------------Delete from Company Payment Ledger------------------------------------------
                        if (!string.IsNullOrEmpty(deletedPaymentIds) && status > 0)
                        {
                            string[] paymentId = deletedPaymentIds.Split(',');

                            using (DbCommand commandDeletePayment = dbSmartAspects.GetStoredProcCommand("DeleteHotelCompanyPaymentLedger_SP"))
                            {
                                for (int i = 0; i < paymentId.Count(); i++)
                                {
                                    int iPaymentId = !string.IsNullOrWhiteSpace(paymentId[i]) ? Convert.ToInt32(paymentId[i]) : 0;
                                    commandDeletePayment.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@PaymentId", DbType.Int32, iPaymentId);
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@CompanyId", DbType.Int32, companyId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePayment, transction);
                                    status = 1;
                                    isWorkAny = true;
                                }
                            }
                        }
                    }

                    if (status > 0 && isWorkAny)
                    {
                        transction.Commit();
                    }
                    else
                    {
                        if (isWorkAny)
                            transction.Rollback();
                    }
                }
            }

            return status;
        }
        public Boolean UpdateOccupiedRoomGroupUnAssignment(int roomRegistrationId, int reservationId, int lastModifiedBy)
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateOccupiedRoomGroupUnAssignment_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@RegistrationId", DbType.Int32, roomRegistrationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int32, reservationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        public Boolean UpdateOccupiedRoomGroupAssignment(int roomRegistrationId, int roomId, int reservationId, int lastModifiedBy)
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateOccupiedRoomGroupAssignment_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@RegistrationId", DbType.Int32, roomRegistrationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomId", DbType.Int32, roomId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int32, reservationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        public bool UpdateRoomSwapInformation(int fromRegistrationId, int toRegistrationId, int fromGuestId, int toGuestId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRoomSwapInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@FromRegistrationId", DbType.Int32, fromRegistrationId);
                    dbSmartAspects.AddInParameter(command, "@ToRegistrationId", DbType.Int32, toRegistrationId);
                    dbSmartAspects.AddInParameter(command, "@FromGuestId", DbType.Int32, fromGuestId);
                    dbSmartAspects.AddInParameter(command, "@ToGuestId", DbType.Int32, toGuestId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool UpdateRoomSwapInformationMultiGuest(int fromRegId, int toRegId, string fromGuestIds, string toGuestIds)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRoomSwapInformationMultiGuest_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@FromRegistrationId", DbType.Int32, fromRegId);
                    dbSmartAspects.AddInParameter(command, "@ToRegistrationId", DbType.Int32, toRegId);

                    dbSmartAspects.AddInParameter(command, "@FromGuestIdList", DbType.String, fromGuestIds);
                    dbSmartAspects.AddInParameter(command, "@ToGuestIdList", DbType.String, toGuestIds);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool UpdateRoomAmendStayInformation(int registrationId, DateTime expectedCheckOutDate)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRoomAmendStayInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, registrationId);
                    dbSmartAspects.AddInParameter(command, "@ExpectedCheckOutDate", DbType.DateTime, expectedCheckOutDate);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool UpdateRoomAmendStayInformationLinked(string regIdQuery, DateTime expectedCheckOutDate, int createdBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRoomAmendStayInformationLinked_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RegistrationIdList", DbType.String, regIdQuery);

                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);
                    dbSmartAspects.AddInParameter(command, "@ExpectedCheckOutDate", DbType.DateTime, expectedCheckOutDate);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean SaveUpdateRoomStopChargePosting(string strRegIds, string strStringSQL, int createdBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveUpdateRoomStopChargePosting_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RegistrationIdList", DbType.String, strRegIds);
                    dbSmartAspects.AddInParameter(command, "@StringSQL", DbType.String, strStringSQL);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool UpdateBillPrintPreviewAndBillLock(Int64 registrationId, Int64 masterRegistrationId, int lastModifiedBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBillPrintPreviewAndBillLock_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int64, registrationId);
                    dbSmartAspects.AddInParameter(command, "@MasterRegistrationId", DbType.Int64, masterRegistrationId);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool ApprovedNightAuditedData(string serviceType, Int64 registrationId, Int64 transactionId, decimal txtCalculatedTotalRoomRateData, Boolean cbCalculateServiceChargeData, Boolean cbCalculateCityChargeData, Boolean cbCalculateVatChargeData, Boolean cbCalculateAdditionalChargeData, Int64 lastModifiedBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateApprovedNightAuditedData_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ServiceType", DbType.String, serviceType);
                    dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int64, registrationId);
                    dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, transactionId);

                    dbSmartAspects.AddInParameter(command, "@CalculatedTotalRoomRate", DbType.Decimal, txtCalculatedTotalRoomRateData);
                    dbSmartAspects.AddInParameter(command, "@IsCbServiceChargeEnable", DbType.Boolean, cbCalculateServiceChargeData);
                    dbSmartAspects.AddInParameter(command, "@IsCbCitySDChargeEnable", DbType.Boolean, cbCalculateCityChargeData);
                    dbSmartAspects.AddInParameter(command, "@IsCbVatEnable", DbType.Boolean, cbCalculateVatChargeData);
                    dbSmartAspects.AddInParameter(command, "@IsCbAdditionalChargeEnable", DbType.Boolean, cbCalculateAdditionalChargeData);

                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool DeleteLinkedRoomInfoByRegistrationId(Int64 tempRegId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteLinkedRoomInfoByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int64, tempRegId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool UpdateHotelRoomReservationDetailForGuestCheckIn(Int64 registrationId, Int64 roomId, Int64 reservationDetailId, int lastModifiedBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateHotelRoomReservationDetailForGuestCheckIn_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int64, registrationId);
                    dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int64, roomId);
                    dbSmartAspects.AddInParameter(command, "@ReservationDetailId", DbType.Int64, reservationDetailId);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
