using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace HotelManagement.Data.HotelManagement
{
    public class GuestHouseCheckOutDA : BaseService
    {
        public GuestHouseCheckOutDetailBO GetIsGuestTodaysBillAdd(string registrationId)
        {
            GuestHouseCheckOutDetailBO serviceBillMaster = new GuestHouseCheckOutDetailBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetIsGuestTodaysBillAdd_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                serviceBillMaster.IsGuestTodaysBillAdd = Convert.ToInt32(reader["IsGuestTodaysBillAdd"]);
                            }
                        }
                    }
                }
            }
            return serviceBillMaster;
        }
        public List<GuestHouseCheckOutDetailBO> GetGuestOtherPaymentForBillByRegiIdList(string registrationIdList)
        {
            List<GuestHouseCheckOutDetailBO> guestHouseCheckOutDetailList = new List<GuestHouseCheckOutDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestOtherPaymentForBillByRegiIdList_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationIdList);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestHouseCheckOutDetailBO serviceBillMaster = new GuestHouseCheckOutDetailBO();
                                if (!string.IsNullOrWhiteSpace(reader["RegistrationId"].ToString()))
                                {
                                    serviceBillMaster.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                    serviceBillMaster.BillPaidBy = Convert.ToInt32(reader["BillPaidBy"]);
                                    serviceBillMaster.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                    serviceBillMaster.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                    guestHouseCheckOutDetailList.Add(serviceBillMaster);
                                }
                            }
                        }
                    }
                }
            }
            return guestHouseCheckOutDetailList;
        }
        public List<GuestHouseCheckOutDetailBO> GetGuestServiceInformationForCheckOut(string registrationId, string serviceType, DateTime transactionDate, int createdBy)
        {
            RoomRegistrationDA roomregistrationDA = new RoomRegistrationDA();
            roomregistrationDA.RoomNightAuditProcess(registrationId, DateTime.Now, 0, createdBy);

            List<GuestHouseCheckOutDetailBO> guestHouseCheckOutDetailList = new List<GuestHouseCheckOutDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestServiceInformationForCheckOut_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationId);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceType", DbType.String, serviceType);
                    dbSmartAspects.AddInParameter(cmd, "@TransactionDate", DbType.DateTime, transactionDate);
                    dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, createdBy);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestHouseCheckOutDetailBO serviceBillMaster = new GuestHouseCheckOutDetailBO();
                                if (!string.IsNullOrWhiteSpace(reader["ServiceId"].ToString()))
                                {
                                    serviceBillMaster.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                    if (serviceBillMaster.RegistrationId > 0)
                                    {
                                        if (!string.IsNullOrWhiteSpace(reader["CheckOutDate"].ToString()))
                                        {
                                            serviceBillMaster.CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"]);
                                        }
                                        else
                                        {
                                            serviceBillMaster.CheckOutDate = null;
                                        }
                                        serviceBillMaster.BillPaidBy = Convert.ToInt32(reader["BillPaidBy"]);
                                        serviceBillMaster.BillPaidByRoomNumber = Convert.ToInt32(reader["BillPaidByRoomNumber"]);
                                        serviceBillMaster.IncomeNodeId = Convert.ToInt32(reader["IncomeNodeId"]);
                                        serviceBillMaster.ServiceBillId = Convert.ToInt64(reader["ServiceBillId"]);
                                        serviceBillMaster.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                        serviceBillMaster.GuestName = reader["GuestName"].ToString();
                                        serviceBillMaster.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                        serviceBillMaster.ServiceType = reader["ServiceType"].ToString();
                                        serviceBillMaster.CostCenter = reader["CostCenter"].ToString();
                                        serviceBillMaster.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                        serviceBillMaster.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                        serviceBillMaster.ServiceName = reader["ServiceName"].ToString();
                                        serviceBillMaster.ServiceRate = Convert.ToDecimal(reader["ServiceRate"]);
                                        serviceBillMaster.ServiceQuantity = Convert.ToDecimal(reader["ServiceQuantity"]);
                                        serviceBillMaster.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                        serviceBillMaster.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                        serviceBillMaster.CitySDCharge = Convert.ToDecimal(reader["CitySDCharge"]);
                                        serviceBillMaster.AdditionalCharge = Convert.ToDecimal(reader["AdditionalCharge"]);
                                        serviceBillMaster.ReferenceSalesCommission = Convert.ToDecimal(reader["ReferenceSalesCommission"]);
                                        serviceBillMaster.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                        serviceBillMaster.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                        serviceBillMaster.NightAuditApproved = reader["NightAuditApproved"].ToString();
                                        serviceBillMaster.VatAmountPercent = Convert.ToDecimal(reader["VatAmountPercent"]);
                                        serviceBillMaster.ServiceChargePercent = Convert.ToDecimal(reader["ServiceChargePercent"]);
                                        serviceBillMaster.CalculatedPercentAmount = Convert.ToDecimal(reader["CalculatedPercentAmount"]);
                                        serviceBillMaster.IsPaidService = Convert.ToBoolean(reader["IsPaidService"]);
                                        serviceBillMaster.IsPaidServiceAchieved = Convert.ToBoolean(reader["IsPaidServiceAchieved"]);
                                        if (Convert.ToDecimal(reader["CurrencyExchangeRate"]) > 0)
                                        {
                                            serviceBillMaster.UsdTotalAmount = (Convert.ToDecimal(reader["TotalAmount"]) / Convert.ToDecimal(reader["CurrencyExchangeRate"]));
                                        }
                                        else
                                        {
                                            serviceBillMaster.UsdTotalAmount = 0;
                                        }
                                        guestHouseCheckOutDetailList.Add(serviceBillMaster);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return guestHouseCheckOutDetailList;
        }
        public List<GuestHouseCheckOutDetailBO> GetGuestHouseBill(string registrationId, string serviceType, int createdBy)
        {
            RoomRegistrationDA roomregistrationDA = new RoomRegistrationDA();
            roomregistrationDA.RoomNightAuditProcess(registrationId, DateTime.Now, 0, createdBy);

            List<GuestHouseCheckOutDetailBO> guestHouseCheckOutDetailList = new List<GuestHouseCheckOutDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestHouseBill_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationId);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceType", DbType.String, serviceType);
                    dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, createdBy);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestHouseCheckOutDetailBO serviceBillMaster = new GuestHouseCheckOutDetailBO();
                                if (!string.IsNullOrWhiteSpace(reader["ServiceId"].ToString()))
                                {
                                    serviceBillMaster.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                    serviceBillMaster.BillPaidBy = Convert.ToInt32(reader["BillPaidBy"]);
                                    serviceBillMaster.BillPaidByRoomNumber = Convert.ToInt32(reader["BillPaidByRoomNumber"]);
                                    serviceBillMaster.IncomeNodeId = Convert.ToInt32(reader["IncomeNodeId"]);
                                    serviceBillMaster.ServiceBillId = Convert.ToInt32(reader["ServiceBillId"]);
                                    serviceBillMaster.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                    serviceBillMaster.GuestName = reader["GuestName"].ToString();
                                    serviceBillMaster.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                    serviceBillMaster.ServiceType = reader["ServiceType"].ToString();
                                    serviceBillMaster.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                    serviceBillMaster.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                    serviceBillMaster.ServiceName = reader["ServiceName"].ToString();
                                    serviceBillMaster.ServiceRate = Convert.ToDecimal(reader["ServiceRate"]);
                                    serviceBillMaster.ServiceQuantity = Convert.ToDecimal(reader["ServiceQuantity"]);
                                    serviceBillMaster.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                    serviceBillMaster.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                    serviceBillMaster.CitySDCharge = Convert.ToDecimal(reader["CitySDCharge"]);
                                    serviceBillMaster.AdditionalCharge = Convert.ToDecimal(reader["AdditionalCharge"]);
                                    serviceBillMaster.ReferenceSalesCommission = Convert.ToDecimal(reader["ReferenceSalesCommission"]);
                                    serviceBillMaster.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                    serviceBillMaster.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                    serviceBillMaster.NightAuditApproved = reader["NightAuditApproved"].ToString();
                                    serviceBillMaster.VatAmountPercent = Convert.ToDecimal(reader["VatAmountPercent"]);
                                    serviceBillMaster.ServiceChargePercent = Convert.ToDecimal(reader["ServiceChargePercent"]);
                                    serviceBillMaster.CalculatedPercentAmount = Convert.ToDecimal(reader["CalculatedPercentAmount"]);
                                    serviceBillMaster.IsPaidService = Convert.ToBoolean(reader["IsPaidService"]);
                                    serviceBillMaster.IsPaidServiceAchieved = Convert.ToBoolean(reader["IsPaidServiceAchieved"]);
                                    serviceBillMaster.CurrencyExchangeRate = Convert.ToDecimal(reader["CurrencyExchangeRate"]);
                                    if (Convert.ToDecimal(reader["CurrencyExchangeRate"]) > 0)
                                    {
                                        serviceBillMaster.UsdTotalAmount = (Convert.ToDecimal(reader["TotalAmount"]) / Convert.ToDecimal(reader["CurrencyExchangeRate"]));
                                    }
                                    else
                                    {
                                        serviceBillMaster.UsdTotalAmount = 0;
                                    }
                                    guestHouseCheckOutDetailList.Add(serviceBillMaster);
                                }
                            }
                        }
                    }
                }
            }
            return guestHouseCheckOutDetailList;
        }
        public List<GuestHouseCheckOutDetailBO> GetGuestHouseBillListWiseSummery(int currentRegistrationID, int roomId)
        {
            List<GuestHouseCheckOutDetailBO> serviceBillMasterList = new List<GuestHouseCheckOutDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestHouseBillListWiseSummery_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, currentRegistrationID);
                    dbSmartAspects.AddInParameter(cmd, "@RoomId", DbType.Int32, roomId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestHouseCheckOutDetailBO serviceBillMaster = new GuestHouseCheckOutDetailBO();

                                serviceBillMaster.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                serviceBillMaster.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                serviceBillMaster.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                serviceBillMasterList.Add(serviceBillMaster);
                            }
                        }
                    }
                }
            }
            return serviceBillMasterList;
        }
        public List<GuestHouseCheckOutBO> GetGuestHouseCheckOutInfo()
        {
            List<GuestHouseCheckOutBO> guestHouseCheckOutBOList = new List<GuestHouseCheckOutBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestHouseCheckOutInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestHouseCheckOutBO guestHouseCheckOut = new GuestHouseCheckOutBO();

                                guestHouseCheckOut.CheckOutId = Convert.ToInt32(reader["CheckOutId"]);
                                guestHouseCheckOut.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                guestHouseCheckOut.BillNumber = reader["BillNumber"].ToString();
                                guestHouseCheckOut.CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"]);
                                guestHouseCheckOut.PayMode = reader["PayMode"].ToString();
                                guestHouseCheckOut.BranchName = reader["BranchName"].ToString();
                                guestHouseCheckOut.ChecqueNumber = reader["ChecqueNumber"].ToString();
                                guestHouseCheckOut.CardNumber = reader["CardNumber"].ToString();
                                guestHouseCheckOut.CardType = reader["CardType"].ToString();
                                guestHouseCheckOut.CardHolderName = reader["CardHolderName"].ToString();
                                if (reader["ExpireDate"] != null)
                                {
                                    guestHouseCheckOut.ExpireDate = Convert.ToDateTime(reader["ExpireDate"].ToString());
                                }

                                guestHouseCheckOut.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                guestHouseCheckOutBOList.Add(guestHouseCheckOut);
                            }
                        }
                    }
                }
            }
            return guestHouseCheckOutBOList;
        }
        public Boolean SaveGuestHouseBillHoldUpInfo(List<GuestHouseCheckOutBO> guestHouseCheckOutListAll, List<GHServiceBillBO> entityRoomDetailBOList, List<GHServiceBillBO> entityDetailBOList, List<GuestBillPaymentBO> guestPaymentDetailList, List<GuestServiceBillApprovedBO> companyPaymentRoomIdList, List<GuestServiceBillApprovedBO> companyPaymentServiceIdList, string paymentFrom, int paymentModeId)
        {
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            List<GuestHouseCheckOutBO> guestHouseCheckOutList = new List<GuestHouseCheckOutBO>();
            foreach (GuestHouseCheckOutBO row in guestHouseCheckOutListAll)
            {
                RoomRegistrationBO registrationBO = new RoomRegistrationBO();
                registrationBO = registrationDA.GetNotCheckedOutRoomRegistrationInfoById(row.RegistrationId);
                if (registrationBO != null)
                {
                    if (registrationBO.RegistrationId > 0)
                    {
                        guestHouseCheckOutList.Add(row);
                    }
                }
            }

            int CheckOutId = 0;
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGuestHouseBillHoldUpProcessInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        int countGuestHouseCheckOutList = 0;
                        foreach (GuestHouseCheckOutBO guestHouseCheckOut in guestHouseCheckOutList)
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, guestHouseCheckOut.RegistrationId);
                            dbSmartAspects.AddInParameter(command, "@BillNumber", DbType.String, guestHouseCheckOut.BillNumber);
                            dbSmartAspects.AddInParameter(command, "@PayMode", DbType.String, guestHouseCheckOut.PayMode);
                            dbSmartAspects.AddInParameter(command, "@BranchName", DbType.String, guestHouseCheckOut.BranchName);
                            dbSmartAspects.AddInParameter(command, "@ChecqueNumber", DbType.String, guestHouseCheckOut.ChecqueNumber);
                            dbSmartAspects.AddInParameter(command, "@CardNumber", DbType.String, guestHouseCheckOut.CardNumber);
                            dbSmartAspects.AddInParameter(command, "@CardType", DbType.String, guestHouseCheckOut.CardType);
                            if (guestHouseCheckOut.PayMode == "Card")
                            {
                                if (guestHouseCheckOut.ExpireDate > DateTime.Now.AddDays(-1))
                                {
                                    dbSmartAspects.AddInParameter(command, "@ExpireDate", DbType.DateTime, guestHouseCheckOut.ExpireDate);
                                }
                            }

                            dbSmartAspects.AddInParameter(command, "@CardHolderName", DbType.String, guestHouseCheckOut.CardHolderName);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, guestHouseCheckOut.VatAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, guestHouseCheckOut.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, guestHouseCheckOut.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@TotalAmount", DbType.Decimal, guestHouseCheckOut.TotalAmount);
                            dbSmartAspects.AddInParameter(command, "@Balance", DbType.Decimal, guestHouseCheckOut.Balance);
                            dbSmartAspects.AddInParameter(command, "@RebateRemarks", DbType.String, guestHouseCheckOut.RebateRemarks);

                            int billPaidByRegistrationId = guestHouseCheckOut.BillPaidBy;
                            if (guestPaymentDetailList != null)
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                                {
                                    if (guestBillPaymentBO.PaymentType == "Other Room")
                                    {
                                        billPaidByRegistrationId = guestBillPaymentBO.BillPaidBy;
                                    }
                                }
                            }

                            dbSmartAspects.AddInParameter(command, "@BillPaidBy", DbType.Int32, billPaidByRegistrationId);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, guestHouseCheckOut.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@CheckOutId", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                            CheckOutId = Convert.ToInt32(command.Parameters["@CheckOutId"].Value);

                            if (status)
                            {
                                if (CheckOutId != 0)
                                {
                                    countGuestHouseCheckOutList = countGuestHouseCheckOutList + 1;

                                    if (entityRoomDetailBOList != null)
                                    {
                                        int countRoomDetailBOList = 0;
                                        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                                        using (DbCommand commandRoomGuestList = dbSmartAspects.GetStoredProcCommand("SaveGuestRoomBillApprovedInfo_SP"))
                                        {
                                            foreach (GHServiceBillBO row in entityRoomDetailBOList)
                                            {
                                                commandRoomGuestList.Parameters.Clear();
                                                if (guestHouseCheckOut.RegistrationId == row.RegistrationId)
                                                {
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@RegistrationId", DbType.Int32, row.RegistrationId);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ApprovedDate", DbType.DateTime, row.ApprovedDate);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ServiceBillId", DbType.Int32, row.ServiceBillId);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ServiceDate", DbType.DateTime, row.ServiceDate);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ServiceType", DbType.String, row.ServiceType);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ServiceId", DbType.Int32, row.ServiceId);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@RoomNumber", DbType.String, row.RoomNumber);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ServiceName", DbType.String, row.ServiceName);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ServiceQuantity", DbType.Decimal, row.ServiceQuantity);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ServiceRate", DbType.Decimal, row.ServiceRate);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@DiscountAmount", DbType.Decimal, row.DiscountAmount);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@VatAmount", DbType.Decimal, row.VatAmount);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ServiceCharge", DbType.Decimal, row.ServiceCharge);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ReferenceSalesCommission", DbType.Decimal, row.ReferenceSalesCommission);

                                                    if (row.ServiceRate > 0)
                                                    {
                                                        dbSmartAspects.AddInParameter(commandRoomGuestList, "@IsBillHoldUp", DbType.Boolean, false);
                                                    }
                                                    else
                                                    {
                                                        dbSmartAspects.AddInParameter(commandRoomGuestList, "@IsBillHoldUp", DbType.Boolean, true);
                                                    }
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@TotalCalculatedAmount", DbType.Decimal, row.TotalCalculatedAmount);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@CreatedBy", DbType.Int32, row.CreatedBy);
                                                    dbSmartAspects.AddOutParameter(commandRoomGuestList, "@ApprovedId", DbType.Int32, sizeof(Int32));

                                                    countRoomDetailBOList += dbSmartAspects.ExecuteNonQuery(commandRoomGuestList, transction);
                                                    int tmpApprovedId = Convert.ToInt32(commandRoomGuestList.Parameters["@ApprovedId"].Value);
                                                }
                                            }
                                        }
                                    }

                                    if (entityDetailBOList != null)
                                    {
                                        int countentityDetailBOList = 0;
                                        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                                        if (entityDetailBOList.Count != 0)
                                        {
                                            using (DbCommand commandGuestList = dbSmartAspects.GetStoredProcCommand("SaveGuestServiceBillApprovedInfo_SP"))
                                            {
                                                foreach (GHServiceBillBO row in entityDetailBOList)
                                                {
                                                    commandGuestList.Parameters.Clear();
                                                    if (guestHouseCheckOut.RegistrationId == row.RegistrationId)
                                                    {
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@PaymentMode", DbType.String, row.PaymentMode);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@RegistrationId", DbType.Int32, row.RegistrationId);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ApprovedDate", DbType.DateTime, row.ApprovedDate);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ServiceBillId", DbType.Int32, row.ServiceBillId);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ServiceDate", DbType.DateTime, row.ServiceDate);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ServiceType", DbType.String, row.ServiceType);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ServiceId", DbType.Int32, row.ServiceId);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ServiceName", DbType.String, row.ServiceName);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ServiceQuantity", DbType.Decimal, row.ServiceQuantity);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ServiceRate", DbType.Decimal, row.ServiceRate);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@DiscountAmount", DbType.Decimal, row.DiscountAmount);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@VatAmount", DbType.Decimal, row.VatAmount);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ServiceCharge", DbType.Decimal, row.ServiceCharge);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@IsPaidService", DbType.Boolean, row.IsPaidService);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@IsPaidServiceAchieved", DbType.Boolean, true);
                                                        row.RestaurantBillId = -1;
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@RestaurantBillId", DbType.Int32, row.RestaurantBillId);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@CreatedBy", DbType.Int32, row.CreatedBy);
                                                        dbSmartAspects.AddOutParameter(commandGuestList, "@ApprovedId", DbType.Int32, sizeof(Int32));

                                                        countentityDetailBOList += dbSmartAspects.ExecuteNonQuery(commandGuestList, transction);

                                                        int tmpApprovedId = Convert.ToInt32(commandGuestList.Parameters["@ApprovedId"].Value);
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            //if (guestHouseCheckOutList != null)
                                            //{
                                            //    using (DbCommand commandAchieved = dbSmartAspects.GetStoredProcCommand("UpdateGuestServiceBillApprovedAchievedInfoByRegistrationId_SP"))
                                            //    {
                                            //        foreach (GuestHouseCheckOutBO AchievedGuestHouseCheckOut in guestHouseCheckOutList)
                                            //        {
                                            //            commandAchieved.Parameters.Clear();

                                            //            dbSmartAspects.AddInParameter(commandAchieved, "@RegistrationId", DbType.Int32, AchievedGuestHouseCheckOut.RegistrationId);
                                            //            dbSmartAspects.AddInParameter(commandAchieved, "@CreatedBy", DbType.Int32, AchievedGuestHouseCheckOut.CreatedBy);
                                            //            dbSmartAspects.ExecuteNonQuery(commandAchieved, transction);
                                            //        }
                                            //    }
                                            //}
                                        }

                                        //Guest Payment information to GuestBillPayment Table--------
                                        if (guestPaymentDetailList != null)
                                        {
                                            int countGuestBillPaymentList = 0;
                                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfo_SP"))
                                            {
                                                foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                                                {
                                                    commandGuestBillPayment.Parameters.Clear();
                                                    if (guestHouseCheckOut.RegistrationId == guestBillPaymentBO.RegistrationId)
                                                    {
                                                        commandGuestBillPayment.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentFrom", DbType.String, paymentFrom);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "FrontOffice");
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, DBNull.Value);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentModeId", DbType.Int32, paymentModeId);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RefundAccountHead", DbType.Int32, guestBillPaymentBO.RefundAccountHead);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@Remarks", DbType.String, guestBillPaymentBO.Remarks);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestHouseCheckOut.CreatedBy);
                                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                                                        countGuestBillPaymentList += dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                                    }
                                                }
                                            }
                                            guestPaymentDetailList = null;
                                        }

                                        //Company Payment information to GuestBillApproved Table--------
                                        if (companyPaymentRoomIdList != null)
                                        {
                                            int countCompanyPaymentRoomIdList = 0;
                                            using (DbCommand commandCompanyRoomBillPayment = dbSmartAspects.GetStoredProcCommand("UpdateCompanyBillPaymentInfo_SP"))
                                            {
                                                foreach (GuestServiceBillApprovedBO entityBO in companyPaymentRoomIdList)
                                                {
                                                    commandCompanyRoomBillPayment.Parameters.Clear();
                                                    if (guestHouseCheckOut.RegistrationId == entityBO.RegistrationId)
                                                    {
                                                        commandCompanyRoomBillPayment.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                                        dbSmartAspects.AddInParameter(commandCompanyRoomBillPayment, "@RegistrationId", DbType.Int32, entityBO.RegistrationId);
                                                        dbSmartAspects.AddInParameter(commandCompanyRoomBillPayment, "@ServiceId", DbType.Int32, entityBO.ServiceId);
                                                        dbSmartAspects.AddInParameter(commandCompanyRoomBillPayment, "@ServiceFromDate", DbType.DateTime, entityBO.ArriveDate);
                                                        dbSmartAspects.AddInParameter(commandCompanyRoomBillPayment, "@ServiceEndDate", DbType.DateTime, entityBO.CheckOutDate);
                                                        dbSmartAspects.AddInParameter(commandCompanyRoomBillPayment, "@LastModifiedBy", DbType.Int32, guestHouseCheckOut.CreatedBy);
                                                        dbSmartAspects.AddInParameter(commandCompanyRoomBillPayment, "@ServiceType", DbType.String, "GuestRoom");

                                                        countCompanyPaymentRoomIdList += dbSmartAspects.ExecuteNonQuery(commandCompanyRoomBillPayment, transction);
                                                    }
                                                }

                                            }
                                        }

                                        //Company Payment information to GuestServiceBillApproved Table--------
                                        if (companyPaymentServiceIdList != null)
                                        {
                                            int countCompanyPaymentServiceIdList = 0;
                                            using (DbCommand commandCompanyServiceBillPayment = dbSmartAspects.GetStoredProcCommand("UpdateCompanyBillPaymentInfo_SP"))
                                            {
                                                foreach (GuestServiceBillApprovedBO entityBO in companyPaymentServiceIdList)
                                                {
                                                    commandCompanyServiceBillPayment.Parameters.Clear();
                                                    if (guestHouseCheckOut.RegistrationId == entityBO.RegistrationId)
                                                    {
                                                        commandCompanyServiceBillPayment.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                                        dbSmartAspects.AddInParameter(commandCompanyServiceBillPayment, "@RegistrationId", DbType.Int32, entityBO.RegistrationId);
                                                        dbSmartAspects.AddInParameter(commandCompanyServiceBillPayment, "@ServiceId", DbType.Int32, entityBO.ServiceId);
                                                        dbSmartAspects.AddInParameter(commandCompanyServiceBillPayment, "@ServiceFromDate", DbType.DateTime, entityBO.ArriveDate);
                                                        dbSmartAspects.AddInParameter(commandCompanyServiceBillPayment, "@ServiceEndDate", DbType.DateTime, entityBO.CheckOutDate);
                                                        dbSmartAspects.AddInParameter(commandCompanyServiceBillPayment, "@LastModifiedBy", DbType.Int32, guestHouseCheckOut.CreatedBy);
                                                        dbSmartAspects.AddInParameter(commandCompanyServiceBillPayment, "@ServiceType", DbType.String, "GuestService");

                                                        countCompanyPaymentServiceIdList += dbSmartAspects.ExecuteNonQuery(commandCompanyServiceBillPayment, transction);
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        countGuestHouseCheckOutList = 0;
                                    }
                                }
                            }
                        }

                        if (countGuestHouseCheckOutList == guestHouseCheckOutList.Count)
                        {
                            //-----------------------Update InHouseGuestLedger table Info---------------------------------------------------------
                            using (DbCommand commandFinalCheckOut = dbSmartAspects.GetStoredProcCommand("UpdateInHouseGuestLedgerForCheckOut_SP"))
                            {
                                foreach (GuestHouseCheckOutBO guestHouseCheckOut in guestHouseCheckOutList)
                                {
                                    commandFinalCheckOut.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandFinalCheckOut, "@RegistrationId", DbType.Int32, guestHouseCheckOut.RegistrationId);
                                    dbSmartAspects.ExecuteNonQuery(commandFinalCheckOut, transction);
                                }
                            }

                            transction.Commit();
                            status = true;
                        }
                        else
                        {
                            status = false;
                            transction.Rollback();
                        }
                    }
                }
            }
            return status;
        }
        public Boolean SaveGuestHouseCheckOutInfo(List<GuestHouseCheckOutBO> guestHouseCheckOutListAll, List<GHServiceBillBO> entityRoomDetailBOList, List<GHServiceBillBO> entityDetailBOList, List<GuestBillPaymentBO> guestPaymentDetailList, List<GuestServiceBillApprovedBO> companyPaymentRoomIdList, List<GuestServiceBillApprovedBO> companyPaymentServiceIdList, string paymentFrom, int paymentModeId)
        {
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            List<GuestHouseCheckOutBO> guestHouseCheckOutList = new List<GuestHouseCheckOutBO>();
            foreach (GuestHouseCheckOutBO row in guestHouseCheckOutListAll)
            {
                RoomRegistrationBO registrationBO = new RoomRegistrationBO();
                registrationBO = registrationDA.GetNotCheckedOutRoomRegistrationInfoById(row.RegistrationId);
                if (registrationBO != null)
                {
                    if (registrationBO.RegistrationId > 0)
                    {
                        guestHouseCheckOutList.Add(row);
                    }
                }
            }

            int CheckOutId = 0;
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGuestHouseCheckOutInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        int countGuestHouseCheckOutList = 0;
                        foreach (GuestHouseCheckOutBO guestHouseCheckOut in guestHouseCheckOutList)
                        {
                            command.Parameters.Clear();
                            command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                            dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, guestHouseCheckOut.RegistrationId);
                            dbSmartAspects.AddInParameter(command, "@BillNumber", DbType.String, guestHouseCheckOut.BillNumber);
                            dbSmartAspects.AddInParameter(command, "@PayMode", DbType.String, guestHouseCheckOut.PayMode);
                            dbSmartAspects.AddInParameter(command, "@BranchName", DbType.String, guestHouseCheckOut.BranchName);
                            dbSmartAspects.AddInParameter(command, "@ChecqueNumber", DbType.String, guestHouseCheckOut.ChecqueNumber);
                            dbSmartAspects.AddInParameter(command, "@CardNumber", DbType.String, guestHouseCheckOut.CardNumber);
                            dbSmartAspects.AddInParameter(command, "@CardType", DbType.String, guestHouseCheckOut.CardType);
                            if (guestHouseCheckOut.PayMode == "Card")
                            {
                                if (guestHouseCheckOut.ExpireDate > DateTime.Now.AddDays(-1))
                                {
                                    dbSmartAspects.AddInParameter(command, "@ExpireDate", DbType.DateTime, guestHouseCheckOut.ExpireDate);
                                }
                            }

                            dbSmartAspects.AddInParameter(command, "@CardHolderName", DbType.String, guestHouseCheckOut.CardHolderName);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, guestHouseCheckOut.VatAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, guestHouseCheckOut.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, guestHouseCheckOut.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@TotalAmount", DbType.Decimal, guestHouseCheckOut.TotalAmount);
                            dbSmartAspects.AddInParameter(command, "@RebateRemarks", DbType.String, guestHouseCheckOut.RebateRemarks);

                            int billPaidByRegistrationId = guestHouseCheckOut.BillPaidBy;
                            if (guestPaymentDetailList != null)
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                                {
                                    if (guestBillPaymentBO.PaymentType == "Other Room")
                                    {
                                        billPaidByRegistrationId = guestBillPaymentBO.BillPaidBy;
                                    }
                                }
                            }

                            dbSmartAspects.AddInParameter(command, "@BillPaidBy", DbType.Int32, billPaidByRegistrationId);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, guestHouseCheckOut.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@CheckOutId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                            CheckOutId = Convert.ToInt32(command.Parameters["@CheckOutId"].Value);

                            if (status)
                            {
                                if (CheckOutId != 0)
                                {
                                    countGuestHouseCheckOutList = countGuestHouseCheckOutList + 1;

                                    if (entityRoomDetailBOList != null)
                                    {
                                        int countRoomDetailBOList = 0;
                                        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                                        using (DbCommand commandRoomGuestList = dbSmartAspects.GetStoredProcCommand("SaveGuestRoomBillApprovedInfo_SP"))
                                        {
                                            foreach (GHServiceBillBO row in entityRoomDetailBOList)
                                            {
                                                commandRoomGuestList.Parameters.Clear();
                                                if (guestHouseCheckOut.RegistrationId == row.RegistrationId)
                                                {
                                                    commandRoomGuestList.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@RegistrationId", DbType.Int32, row.RegistrationId);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ApprovedDate", DbType.DateTime, row.ApprovedDate);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ServiceBillId", DbType.Int32, row.ServiceBillId);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ServiceDate", DbType.DateTime, row.ServiceDate);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ServiceType", DbType.String, row.ServiceType);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ServiceId", DbType.Int32, row.ServiceId);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@RoomNumber", DbType.String, row.RoomNumber);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ServiceName", DbType.String, row.ServiceName);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ServiceQuantity", DbType.Decimal, row.ServiceQuantity);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ServiceRate", DbType.Decimal, row.ServiceRate);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@DiscountAmount", DbType.Decimal, row.DiscountAmount);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@VatAmount", DbType.Decimal, row.VatAmount);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ServiceCharge", DbType.Decimal, row.ServiceCharge);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@ReferenceSalesCommission", DbType.Decimal, row.ReferenceSalesCommission);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@IsBillHoldUp", DbType.Boolean, row.IsBillHoldUp);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@TotalCalculatedAmount", DbType.Decimal, row.TotalCalculatedAmount);
                                                    dbSmartAspects.AddInParameter(commandRoomGuestList, "@CreatedBy", DbType.Int32, row.CreatedBy);
                                                    dbSmartAspects.AddOutParameter(commandRoomGuestList, "@ApprovedId", DbType.Int32, sizeof(Int32));

                                                    countRoomDetailBOList += dbSmartAspects.ExecuteNonQuery(commandRoomGuestList, transction);
                                                    int tmpApprovedId = Convert.ToInt32(commandRoomGuestList.Parameters["@ApprovedId"].Value);
                                                }
                                            }
                                        }
                                    }

                                    if (entityDetailBOList != null)
                                    {
                                        int countentityDetailBOList = 0;
                                        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                                        if (entityDetailBOList.Count != 0)
                                        {
                                            using (DbCommand commandGuestList = dbSmartAspects.GetStoredProcCommand("SaveGuestServiceBillApprovedInfo_SP"))
                                            {
                                                foreach (GHServiceBillBO row in entityDetailBOList)
                                                {
                                                    commandGuestList.Parameters.Clear();
                                                    if (guestHouseCheckOut.RegistrationId == row.RegistrationId)
                                                    {
                                                        commandGuestList.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@PaymentMode", DbType.String, row.PaymentMode);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@RegistrationId", DbType.Int32, row.RegistrationId);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ApprovedDate", DbType.DateTime, row.ApprovedDate);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ServiceBillId", DbType.Int32, row.ServiceBillId);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ServiceDate", DbType.DateTime, row.ServiceDate);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ServiceType", DbType.String, row.ServiceType);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ServiceId", DbType.Int32, row.ServiceId);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ServiceName", DbType.String, row.ServiceName);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ServiceQuantity", DbType.Decimal, row.ServiceQuantity);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ServiceRate", DbType.Decimal, row.ServiceRate);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@DiscountAmount", DbType.Decimal, row.DiscountAmount);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@VatAmount", DbType.Decimal, row.VatAmount);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@ServiceCharge", DbType.Decimal, row.ServiceCharge);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@IsPaidService", DbType.Boolean, row.IsPaidService);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@IsPaidServiceAchieved", DbType.Boolean, true);
                                                        row.RestaurantBillId = -1;
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@RestaurantBillId", DbType.Int32, row.RestaurantBillId);
                                                        dbSmartAspects.AddInParameter(commandGuestList, "@CreatedBy", DbType.Int32, row.CreatedBy);
                                                        dbSmartAspects.AddOutParameter(commandGuestList, "@ApprovedId", DbType.Int32, sizeof(Int32));

                                                        countentityDetailBOList += dbSmartAspects.ExecuteNonQuery(commandGuestList, transction);
                                                        int tmpApprovedId = Convert.ToInt32(commandGuestList.Parameters["@ApprovedId"].Value);
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            //if (guestHouseCheckOutList != null)
                                            //{
                                            //    using (DbCommand commandAchieved = dbSmartAspects.GetStoredProcCommand("UpdateGuestServiceBillApprovedAchievedInfoByRegistrationId_SP"))
                                            //    {
                                            //        foreach (GuestHouseCheckOutBO AchievedGuestHouseCheckOut in guestHouseCheckOutList)
                                            //        {
                                            //            commandAchieved.Parameters.Clear();

                                            //            dbSmartAspects.AddInParameter(commandAchieved, "@RegistrationId", DbType.Int32, AchievedGuestHouseCheckOut.RegistrationId);
                                            //            dbSmartAspects.AddInParameter(commandAchieved, "@CreatedBy", DbType.Int32, AchievedGuestHouseCheckOut.CreatedBy);
                                            //            dbSmartAspects.ExecuteNonQuery(commandAchieved, transction);
                                            //        }
                                            //    }
                                            //}
                                        }

                                        //Guest Payment information to GuestBillPayment Table--------
                                        if (guestPaymentDetailList != null)
                                        {
                                            int countGuestBillPaymentList = 0;
                                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfo_SP"))
                                            {
                                                foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                                                {
                                                    commandGuestBillPayment.Parameters.Clear();
                                                    if (guestHouseCheckOut.RegistrationId == guestBillPaymentBO.RegistrationId)
                                                    {
                                                        commandGuestBillPayment.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentFrom", DbType.String, paymentFrom);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "FrontOffice");
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, DBNull.Value);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentModeId", DbType.Int32, paymentModeId);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RefundAccountHead", DbType.Int32, guestBillPaymentBO.RefundAccountHead);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@Remarks", DbType.String, guestBillPaymentBO.Remarks);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestHouseCheckOut.CreatedBy);
                                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                                                        countGuestBillPaymentList += dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                                    }
                                                }
                                            }
                                            guestPaymentDetailList = null;
                                        }

                                        //Company Payment information to GuestBillApproved Table--------
                                        if (companyPaymentRoomIdList != null)
                                        {
                                            int countCompanyPaymentRoomIdList = 0;
                                            using (DbCommand commandCompanyRoomBillPayment = dbSmartAspects.GetStoredProcCommand("UpdateCompanyBillPaymentInfo_SP"))
                                            {
                                                foreach (GuestServiceBillApprovedBO entityBO in companyPaymentRoomIdList)
                                                {
                                                    commandCompanyRoomBillPayment.Parameters.Clear();
                                                    if (guestHouseCheckOut.RegistrationId == entityBO.RegistrationId)
                                                    {
                                                        commandCompanyRoomBillPayment.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                                        dbSmartAspects.AddInParameter(commandCompanyRoomBillPayment, "@RegistrationId", DbType.Int32, entityBO.RegistrationId);
                                                        dbSmartAspects.AddInParameter(commandCompanyRoomBillPayment, "@ServiceId", DbType.Int32, entityBO.ServiceId);
                                                        dbSmartAspects.AddInParameter(commandCompanyRoomBillPayment, "@ServiceFromDate", DbType.DateTime, entityBO.ArriveDate);
                                                        dbSmartAspects.AddInParameter(commandCompanyRoomBillPayment, "@ServiceEndDate", DbType.DateTime, entityBO.CheckOutDate);
                                                        dbSmartAspects.AddInParameter(commandCompanyRoomBillPayment, "@LastModifiedBy", DbType.Int32, guestHouseCheckOut.CreatedBy);
                                                        dbSmartAspects.AddInParameter(commandCompanyRoomBillPayment, "@ServiceType", DbType.String, "GuestRoom");

                                                        countCompanyPaymentRoomIdList += dbSmartAspects.ExecuteNonQuery(commandCompanyRoomBillPayment, transction);
                                                    }
                                                }

                                            }
                                        }

                                        //Company Payment information to GuestServiceBillApproved Table--------
                                        if (companyPaymentServiceIdList != null)
                                        {
                                            int countCompanyPaymentServiceIdList = 0;
                                            using (DbCommand commandCompanyServiceBillPayment = dbSmartAspects.GetStoredProcCommand("UpdateCompanyBillPaymentInfo_SP"))
                                            {
                                                foreach (GuestServiceBillApprovedBO entityBO in companyPaymentServiceIdList)
                                                {
                                                    commandCompanyServiceBillPayment.Parameters.Clear();
                                                    if (guestHouseCheckOut.RegistrationId == entityBO.RegistrationId)
                                                    {
                                                        commandCompanyServiceBillPayment.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                                        dbSmartAspects.AddInParameter(commandCompanyServiceBillPayment, "@RegistrationId", DbType.Int32, entityBO.RegistrationId);
                                                        dbSmartAspects.AddInParameter(commandCompanyServiceBillPayment, "@ServiceId", DbType.Int32, entityBO.ServiceId);
                                                        dbSmartAspects.AddInParameter(commandCompanyServiceBillPayment, "@ServiceFromDate", DbType.DateTime, entityBO.ArriveDate);
                                                        dbSmartAspects.AddInParameter(commandCompanyServiceBillPayment, "@ServiceEndDate", DbType.DateTime, entityBO.CheckOutDate);
                                                        dbSmartAspects.AddInParameter(commandCompanyServiceBillPayment, "@LastModifiedBy", DbType.Int32, guestHouseCheckOut.CreatedBy);
                                                        dbSmartAspects.AddInParameter(commandCompanyServiceBillPayment, "@ServiceType", DbType.String, "GuestService");

                                                        countCompanyPaymentServiceIdList += dbSmartAspects.ExecuteNonQuery(commandCompanyServiceBillPayment, transction);
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        countGuestHouseCheckOutList = 0;
                                    }
                                }
                            }
                        }

                        if (countGuestHouseCheckOutList == guestHouseCheckOutList.Count)
                        {
                            //-----------------------Update InHouseGuestLedger table Info---------------------------------------------------------
                            using (DbCommand commandFinalCheckOut = dbSmartAspects.GetStoredProcCommand("UpdateInHouseGuestLedgerForCheckOut_SP"))
                            {
                                foreach (GuestHouseCheckOutBO guestHouseCheckOut in guestHouseCheckOutList)
                                {
                                    commandFinalCheckOut.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandFinalCheckOut, "@RegistrationId", DbType.Int32, guestHouseCheckOut.RegistrationId);
                                    dbSmartAspects.ExecuteNonQuery(commandFinalCheckOut, transction);
                                }
                            }

                            transction.Commit();
                            status = true;
                        }
                        else
                        {
                            status = false;
                            transction.Rollback();
                        }
                    }
                }
            }
            return status;
        }
        public Boolean UpdateGuestHouseCheckOutInfo(GuestHouseCheckOutBO guestHouseCheckOut)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGuestHouseCheckOutInfo_SP"))
                {
                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(command, "@CheckOutId", DbType.Int32, guestHouseCheckOut.CheckOutId);
                    dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, guestHouseCheckOut.RegistrationId);
                    dbSmartAspects.AddInParameter(command, "@BillNumber", DbType.String, guestHouseCheckOut.BillNumber);
                    dbSmartAspects.AddInParameter(command, "@CheckOutDate", DbType.DateTime, guestHouseCheckOut.CheckOutDate);
                    dbSmartAspects.AddInParameter(command, "@PayMode", DbType.String, guestHouseCheckOut.PayMode);
                    dbSmartAspects.AddInParameter(command, "@BranchName", DbType.String, guestHouseCheckOut.BranchName);
                    dbSmartAspects.AddInParameter(command, "@ChecqueNumber", DbType.String, guestHouseCheckOut.ChecqueNumber);
                    dbSmartAspects.AddInParameter(command, "@CardNumber", DbType.String, guestHouseCheckOut.CardNumber);
                    dbSmartAspects.AddInParameter(command, "@CardType", DbType.String, guestHouseCheckOut.CardType);
                    dbSmartAspects.AddInParameter(command, "@ExpireDate", DbType.DateTime, guestHouseCheckOut.ExpireDate);
                    dbSmartAspects.AddInParameter(command, "@CardHolderName", DbType.String, guestHouseCheckOut.CardHolderName);
                    dbSmartAspects.AddInParameter(command, "@BillPaidBy", DbType.Int32, guestHouseCheckOut.BillPaidBy);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, guestHouseCheckOut.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public GuestHouseCheckOutBO GetGuestHouseCheckOutInfoById(int checkOutId)
        {
            GuestHouseCheckOutBO guestHouseCheckOut = new GuestHouseCheckOutBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestHouseCheckOutInfoById_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CheckOutId", DbType.Int32, checkOutId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                guestHouseCheckOut.CheckOutId = Convert.ToInt32(reader["CheckOutId"]);
                                guestHouseCheckOut.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                guestHouseCheckOut.BillNumber = reader["BillNumber"].ToString();
                                guestHouseCheckOut.CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"]);
                                guestHouseCheckOut.PayMode = reader["PayMode"].ToString();
                                //guestHouseCheckOut.BankId = Convert.ToInt32(reader["BankId"]);
                                guestHouseCheckOut.BranchName = reader["BranchName"].ToString();
                                guestHouseCheckOut.ChecqueNumber = reader["ChecqueNumber"].ToString();
                                guestHouseCheckOut.CardNumber = reader["CardNumber"].ToString();
                                guestHouseCheckOut.CardType = reader["CardType"].ToString();
                                guestHouseCheckOut.CardHolderName = reader["CardHolderName"].ToString();
                                if (reader["ExpireDate"] != null)
                                {
                                    guestHouseCheckOut.ExpireDate = Convert.ToDateTime(reader["ExpireDate"].ToString());
                                }
                                guestHouseCheckOut.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                            }
                        }
                    }
                }
            }
            return guestHouseCheckOut;
        }
        public List<GuestHouseCheckOutBO> GetGuestHouseCheckOutInfoByPaidBy(string currentRegistrationId, int paidById)
        {
            List<GuestHouseCheckOutBO> guestHouseCheckOutList = new List<GuestHouseCheckOutBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestHouseCheckOutInfoByPaidBy_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, currentRegistrationId);
                    dbSmartAspects.AddInParameter(cmd, "@PaidById", DbType.Int32, paidById);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestHouseCheckOutBO guestHouseCheckOut = new GuestHouseCheckOutBO();

                                guestHouseCheckOut.CheckOutId = Convert.ToInt32(reader["CheckOutId"]);
                                guestHouseCheckOut.RoomNumber = reader["RoomNumber"].ToString();
                                guestHouseCheckOut.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                guestHouseCheckOut.BillNumber = reader["BillNumber"].ToString();
                                guestHouseCheckOut.CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"]);
                                guestHouseCheckOut.PayMode = reader["PayMode"].ToString();
                                guestHouseCheckOut.BankId = Convert.ToInt32(reader["BankId"]);
                                guestHouseCheckOut.BranchName = reader["BranchName"].ToString();
                                guestHouseCheckOut.ChecqueNumber = reader["ChecqueNumber"].ToString();
                                guestHouseCheckOut.CardNumber = reader["CardNumber"].ToString();
                                guestHouseCheckOut.CardType = reader["CardType"].ToString();
                                guestHouseCheckOut.CardHolderName = reader["CardHolderName"].ToString();

                                if (reader["ExpireDate"] != null)
                                {
                                    guestHouseCheckOut.ExpireDate = Convert.ToDateTime(reader["ExpireDate"].ToString());
                                }

                                guestHouseCheckOut.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                guestHouseCheckOut.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                guestHouseCheckOut.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                guestHouseCheckOut.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                guestHouseCheckOutList.Add(guestHouseCheckOut);
                            }
                        }
                    }
                }
            }
            return guestHouseCheckOutList;
        }
        public Boolean DeleteGuestHouseCheckOutInfoById(int checkOutId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteGuestHouseCheckOutInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CheckOutId", DbType.Int32, checkOutId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<GuestServiceBillApprovedBO> GetGuestApprovedServiceInfo()
        {
            List<GuestServiceBillApprovedBO> entityBOList = new List<GuestServiceBillApprovedBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {                
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestApprovedServiceInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestServiceBillApprovedBO entityBO = new GuestServiceBillApprovedBO();
                                entityBO.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                entityBO.ServiceInfo = reader["ServiceInfo"].ToString();
                                entityBO.ServiceName = reader["ServiceName"].ToString();
                                entityBOList.Add(entityBO);

                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<GuestServiceBillApprovedBO> GetGuestBillInfoByRegistrationNServiceType(int registrationId, string serviceType, string companyName, string companyAddress, string companyWeb)
        {
            List<GuestServiceBillApprovedBO> entityBOList = new List<GuestServiceBillApprovedBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillInfoByRegistrationNServiceType_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceType", DbType.String, serviceType);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyAddress", DbType.String, companyAddress);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyWeb", DbType.String, companyWeb);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestServiceBillApprovedBO entityBO = new GuestServiceBillApprovedBO();

                                entityBO.HMCompanyProfile = reader["HMCompanyProfile"].ToString();
                                entityBO.HMCompanyAddress = reader["HMCompanyAddress"].ToString();
                                entityBO.HMCompanyWeb = reader["HMCompanyWeb"].ToString();
                                entityBO.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                entityBO.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                entityBO.RoomNumber = reader["RoomNumber"].ToString();
                                entityBO.RoomType = reader["RoomType"].ToString();
                                entityBO.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                entityBO.CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"]);
                                entityBO.TotalDay = Convert.ToInt32(reader["TotalDay"]);
                                entityBO.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                entityBO.GuestService = reader["GuestService"].ToString();
                                entityBO.ServiceRate = Convert.ToDecimal(reader["ServiceRate"]);
                                entityBO.ServiceQuantity = Convert.ToDecimal(reader["ServiceQuantity"]);
                                entityBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                entityBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                entityBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                entityBO.AdvancePayment = Convert.ToDecimal(reader["AdvancePayment"]);
                                entityBO.RestaurantGrandTotal = Convert.ToDecimal(reader["RestaurantGrandTotal"]);
                                entityBOList.Add(entityBO);

                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<GuestServiceBillApprovedBO> GetGuestBillInfoByDynamicCondition(string registrationIdList, string guestBillRoomIdParameterValue, string guestBillServiceIdParameterValue, int isBillSplited)
        {
            List<GuestServiceBillApprovedBO> entityBOList = new List<GuestServiceBillApprovedBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillServiceInformation_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationIdList);
                    dbSmartAspects.AddInParameter(cmd, "@GuestBillRoomIdParameterValue", DbType.String, guestBillRoomIdParameterValue);
                    dbSmartAspects.AddInParameter(cmd, "@GuestBillServiceIdParameterValue", DbType.String, guestBillServiceIdParameterValue);
                    dbSmartAspects.AddInParameter(cmd, "@IsBillSplited", DbType.Int32, isBillSplited);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestServiceBillApprovedBO entityBO = new GuestServiceBillApprovedBO();

                                entityBO.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                entityBO.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                entityBO.GuestService = reader["GuestService"].ToString();
                                entityBO.ServiceRate = Convert.ToDecimal(reader["ServiceRate"]);
                                entityBO.ServiceQuantity = Convert.ToDecimal(reader["ServiceQuantity"]);
                                entityBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                entityBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                entityBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                entityBO.IsBillSplited = Convert.ToInt32(reader["IsBillSplited"]);
                                entityBO.AdvanceAmount = Convert.ToDecimal(reader["AdvanceAmount"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<GuestServiceBillApprovedBO> GetGuestBillInfoByDynamicConditionForInvoice2(string registrationIdList, string guestBillRoomIdParameterValue, string guestBillServiceIdParameterValue, int isBillSplited, int IsCheckOutBillPreview)
        {
            List<GuestServiceBillApprovedBO> entityBOList = new List<GuestServiceBillApprovedBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillServiceInformationForInvoice_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationIdList);
                    dbSmartAspects.AddInParameter(cmd, "@GuestBillRoomIdParameterValue", DbType.String, guestBillRoomIdParameterValue);
                    dbSmartAspects.AddInParameter(cmd, "@GuestBillServiceIdParameterValue", DbType.String, guestBillServiceIdParameterValue);
                    dbSmartAspects.AddInParameter(cmd, "@IsBillSplited", DbType.Int32, isBillSplited);
                    dbSmartAspects.AddInParameter(cmd, "@IsCheckOutBillPreview", DbType.Int32, IsCheckOutBillPreview);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestServiceBillApprovedBO entityBO = new GuestServiceBillApprovedBO();
                                entityBO.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                entityBO.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                entityBO.Reference = reader["Reference"].ToString();
                                entityBO.GuestService = reader["GuestService"].ToString();
                                entityBO.BillAmount = Convert.ToDecimal(reader["BillAmount"]);
                                entityBO.PaidAmount = Convert.ToDecimal(reader["PaidAmount"]);
                                entityBO.IsBillSplited = Convert.ToInt32(reader["IsBillSplited"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public bool CancelGuestHouseCheckOutInfoByRegiId(int registrationId)
        {
            bool isCanceled = true;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("CancelGuestHouseCheckOutInfoByRegiId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);
                    dbSmartAspects.AddOutParameter(cmd, "@IsCanceled", DbType.Boolean, sizeof(bool));

                    dbSmartAspects.ExecuteNonQuery(cmd);
                    isCanceled = Convert.ToBoolean(cmd.Parameters["@IsCanceled"].Value);
                }
            }

            return isCanceled;
        }
        public List<GuestBillPaymentInvoiceReportViewBO> GetGuestPaymentInvoiceInfo(string searchCriteria)
        {
            List<GuestBillPaymentInvoiceReportViewBO> entityBOList = new List<GuestBillPaymentInvoiceReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestPaymentInvoiceInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestBillPaymentInvoiceReportViewBO entityBO = new GuestBillPaymentInvoiceReportViewBO();
                                entityBO.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                entityBO.BillNumber = reader["BillNumber"].ToString();
                                entityBO.PaymentType = reader["PaymentType"].ToString();
                                entityBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                entityBO.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                entityBO.GuestName = reader["GuestName"].ToString();
                                entityBO.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                entityBO.PaymentMode = reader["PaymentMode"].ToString();
                                entityBO.FieldId = Convert.ToInt32(reader["FieldId"]);
                                entityBO.CurrencyHead = reader["CurrencyHead"].ToString();
                                entityBO.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"]);
                                entityBO.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"]);
                                entityBO.BankId = Convert.ToInt32(reader["BankId"]);
                                entityBO.BranchName = reader["BranchName"].ToString();
                                entityBO.ChecqueNumber = reader["ChecqueNumber"].ToString();
                                entityBO.ChecqueDate = Convert.ToDateTime(reader["ChecqueDate"]);
                                entityBO.CardType = reader["CardType"].ToString();
                                entityBO.CardNumber = reader["CardNumber"].ToString();
                                entityBO.CardHolderName = reader["CardHolderName"].ToString();
                                entityBO.CardReference = reader["CardReference"].ToString();
                                entityBO.RoomNumber = reader["RoomNumber"].ToString();
                                entityBO.Remarks = reader["Remarks"].ToString();
                                entityBO.DetailDescription = reader["DetailDescription"].ToString();
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<GuestBillPaymentInvoiceReportViewBO> GetNoShowPaymentInvoiceInfo(string searchCriteria)
        {
            List<GuestBillPaymentInvoiceReportViewBO> entityBOList = new List<GuestBillPaymentInvoiceReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNoShowPaymentInvoiceInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestBillPaymentInvoiceReportViewBO entityBO = new GuestBillPaymentInvoiceReportViewBO();
                                entityBO.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                entityBO.BillNumber = reader["BillNumber"].ToString();
                                entityBO.PaymentType = reader["PaymentType"].ToString();
                                entityBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                entityBO.ReservationNumber = reader["ReservationNumber"].ToString();
                                entityBO.GuestName = reader["GuestName"].ToString();
                                entityBO.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                entityBO.PaymentMode = reader["PaymentMode"].ToString();
                                entityBO.FieldId = Convert.ToInt32(reader["FieldId"]);
                                entityBO.CurrencyHead = reader["CurrencyHead"].ToString();
                                entityBO.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"]);
                                entityBO.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"]);
                                entityBO.BankId = Convert.ToInt32(reader["BankId"]);
                                entityBO.BranchName = reader["BranchName"].ToString();
                                entityBO.ChecqueNumber = reader["ChecqueNumber"].ToString();
                                entityBO.ChecqueDate = Convert.ToDateTime(reader["ChecqueDate"]);
                                entityBO.CardType = reader["CardType"].ToString();
                                entityBO.CardNumber = reader["CardNumber"].ToString();
                                entityBO.CardHolderName = reader["CardHolderName"].ToString();
                                entityBO.CardReference = reader["CardReference"].ToString();
                                entityBO.RoomNumber = reader["RoomNumber"].ToString();
                                entityBO.Remarks = reader["Remarks"].ToString();
                                entityBO.DetailDescription = reader["DetailDescription"].ToString();
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<GuestBillPaymentInvoiceReportViewBO> GetBanquetTransactionInvoiceInfo(string searchCriteria)
        {
            List<GuestBillPaymentInvoiceReportViewBO> entityBOList = new List<GuestBillPaymentInvoiceReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetTransactionInvoiceInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestBillPaymentInvoiceReportViewBO entityBO = new GuestBillPaymentInvoiceReportViewBO();
                                entityBO.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                entityBO.BillNumber = reader["BillNumber"].ToString();
                                entityBO.PaymentType = reader["PaymentType"].ToString();
                                entityBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                entityBO.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                entityBO.GuestName = reader["GuestName"].ToString();
                                entityBO.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                entityBO.PaymentMode = reader["PaymentMode"].ToString();
                                entityBO.FieldId = Convert.ToInt32(reader["FieldId"]);
                                entityBO.CurrencyHead = reader["CurrencyHead"].ToString();
                                entityBO.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"]);
                                entityBO.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"]);
                                entityBO.BankId = Convert.ToInt32(reader["BankId"]);
                                entityBO.BranchName = reader["BranchName"].ToString();
                                entityBO.ChecqueNumber = reader["ChecqueNumber"].ToString();
                                entityBO.ChecqueDate = Convert.ToDateTime(reader["ChecqueDate"]);
                                entityBO.CardType = reader["CardType"].ToString();
                                entityBO.CardNumber = reader["CardNumber"].ToString();
                                entityBO.CardHolderName = reader["CardHolderName"].ToString();
                                entityBO.CardReference = reader["CardReference"].ToString();
                                entityBO.RoomNumber = reader["RoomNumber"].ToString();
                                entityBO.Remarks = reader["Remarks"].ToString();
                                entityBO.DetailDescription = reader["DetailDescription"].ToString();
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<GuestServiceBillApprovedBO> GetGuestServiceBillInfoDynamically(int frontOfficeInvoiceTemplate, string transactionType, string registrationIdList, string parameterValue, int isBillSplited, int IsCheckOutBillPreview)
        {
            List<GuestServiceBillApprovedBO> entityBOList = new List<GuestServiceBillApprovedBO>();

            if (frontOfficeInvoiceTemplate == 1)
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestServiceBillInfoDynamicallyForInvoice_SP"))
                    {
                        cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                        dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transactionType);
                        dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationIdList);
                        dbSmartAspects.AddInParameter(cmd, "@ParameterValue", DbType.String, parameterValue);
                        dbSmartAspects.AddInParameter(cmd, "@IsBillSplited", DbType.Int32, isBillSplited);
                        dbSmartAspects.AddInParameter(cmd, "@IsCheckOutBillPreview", DbType.Int32, IsCheckOutBillPreview);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    GuestServiceBillApprovedBO entityBO = new GuestServiceBillApprovedBO();

                                    entityBO.ApprovedId = Convert.ToInt64(reader["ApprovedId"]);
                                    entityBO.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                    entityBO.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                    entityBO.Reference = reader["Reference"].ToString();
                                    entityBO.GuestService = reader["GuestService"].ToString();
                                    entityBO.BillAmount = Convert.ToDecimal(reader["BillAmount"]);
                                    entityBO.PaidAmount = Convert.ToDecimal(reader["PaidAmount"]);
                                    entityBO.IsBillSplited = Convert.ToInt32(reader["IsBillSplited"]);

                                    entityBOList.Add(entityBO);
                                }
                            }
                        }
                    }
                }
            }
            else //if (frontOfficeInvoiceTemplate == 2) //(Same for 2, 3 and 4)
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFrontOfficeGuestBillInfoForInvoiceDynamically_SP"))
                    {
                        cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                        dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transactionType);
                        dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationIdList);
                        dbSmartAspects.AddInParameter(cmd, "@ParameterValue", DbType.String, parameterValue);
                        dbSmartAspects.AddInParameter(cmd, "@IsBillSplited", DbType.Int32, isBillSplited);
                        dbSmartAspects.AddInParameter(cmd, "@IsCheckOutBillPreview", DbType.Int32, IsCheckOutBillPreview);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    GuestServiceBillApprovedBO entityBO = new GuestServiceBillApprovedBO();

                                    entityBO.ApprovedId = Convert.ToInt64(reader["ApprovedId"]);
                                    entityBO.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                    entityBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                    entityBO.DisplayServiceDate = reader["DisplayServiceDate"].ToString();
                                    entityBO.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                    entityBO.Reference = reader["Reference"].ToString();
                                    entityBO.ServiceType = reader["ServiceType"].ToString();
                                    entityBO.GuestService = reader["GuestService"].ToString();
                                    entityBO.TotalRoomCharge = Convert.ToDecimal(reader["TotalRoomCharge"]);
                                    entityBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                    entityBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                    entityBO.ServiceRate = Convert.ToDecimal(reader["ServiceRate"]);
                                    entityBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                    entityBO.CitySDCharge = Convert.ToDecimal(reader["CitySDCharge"]);
                                    entityBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                    entityBO.AdditionalCharge = Convert.ToDecimal(reader["AdditionalCharge"]);
                                    entityBO.BillAmount = Convert.ToDecimal(reader["BillAmount"]);
                                    entityBO.PaidAmount = Convert.ToDecimal(reader["PaidAmount"]);
                                    entityBO.IsBillSplited = Convert.ToInt32(reader["IsBillSplited"]);
                                    entityBO.PaymentType = reader["PaymentType"].ToString();
                                    entityBO.CurrencyExchangeRate = Convert.ToDecimal(reader["CurrencyExchangeRate"]);
                                    entityBO.ServiceSummaryType = reader["ServiceSummaryType"].ToString();
                                    entityBO.ServiceSummaryTypeOrderBy = Convert.ToInt32(reader["ServiceSummaryTypeOrderBy"]);

                                    entityBOList.Add(entityBO);
                                }
                            }
                        }
                    }
                }
            }

            return entityBOList;
        }
        public List<GuestHouseCheckOutDetailBO> GetInHouseGuestLedgerInfo(string registrationId, string serviceType)
        {
            List<GuestHouseCheckOutDetailBO> guestHouseCheckOutDetailList = new List<GuestHouseCheckOutDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInHouseGuestLedgerInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationId);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceType", DbType.String, serviceType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestHouseCheckOutDetailBO serviceBillMaster = new GuestHouseCheckOutDetailBO();
                                serviceBillMaster.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                serviceBillMaster.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                serviceBillMaster.GuestName = reader["GuestName"].ToString();
                                serviceBillMaster.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                serviceBillMaster.ServiceType = reader["ServiceType"].ToString();
                                serviceBillMaster.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                serviceBillMaster.ServiceName = reader["ServiceName"].ToString();
                                serviceBillMaster.ServiceQuantity = Convert.ToDecimal(reader["ServiceQuantity"]);
                                serviceBillMaster.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);

                                guestHouseCheckOutDetailList.Add(serviceBillMaster);
                            }
                        }
                    }
                }
            }
            return guestHouseCheckOutDetailList;
        }
        public HotelGuestHouseCheckOutBO GetGuestHouseCheckOutInfoByRegistrationId(int currentRegistrationId)
        {
            HotelGuestHouseCheckOutBO guestHouseCheckOut = null;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestHouseCheckOutInfoByRegistrationId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, currentRegistrationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "CheckOut");
                    if (ds.Tables[0].Rows.Count > 0)
                        guestHouseCheckOut = ds.Tables[0].AsEnumerable().Select(r => new HotelGuestHouseCheckOutBO
                        {
                            CheckOutId = r.Field<int>("CheckOutId"),
                            RegistrationId = r.Field<int?>("RegistrationId"),
                            BillNumber = r.Field<string>("BillNumber"),
                            CheckOutDate = r.Field<DateTime?>("CheckOutDate"),
                            CheckOutDateForSync = r.Field<DateTime?>("CheckOutDate"),

                            PayMode = r.Field<string>("PayMode"),
                            BankId = r.Field<int?>("BankId"),
                            BranchName = r.Field<string>("BranchName"),
                            ChecqueNumber = r.Field<string>("ChecqueNumber"),
                            CardNumber = r.Field<string>("CardNumber"),
                            CardType = r.Field<string>("CardType"),
                            CardHolderName = r.Field<string>("CardHolderName"),
                            ExpireDate = r.Field<DateTime?>("ExpireDate"),

                            CardReference = r.Field<string>("CardReference"),
                            VatAmount = r.Field<decimal?>("VatAmount"),
                            ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                            DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                            TotalAmount = r.Field<decimal?>("TotalAmount"),
                            RebateRemarks = r.Field<string>("RebateRemarks"),
                            BillPaidBy = r.Field<int?>("BillPaidBy"),
                            BillPaidByGuidId = r.Field<Guid>("BillPaidByGuidId"),
                            DealId = r.Field<int?>("DealId"),
                            IsDayClosed = r.Field<bool?>("IsDayClosed"),

                            CreatedBy = r.Field<int?>("CreatedBy"),
                            CreatedDate = r.Field<DateTime?>("CreatedDate"),
                            LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                            LastModifiedDate = r.Field<DateTime?>("LastModifiedDate")

                        }).FirstOrDefault();
                }
            }
            return guestHouseCheckOut;
        }
        public List<GuestBillPaymentInvoiceReportViewBO> GetGuestPaymentInvoiceInformation(string searchCriteria)
        {
            List<GuestBillPaymentInvoiceReportViewBO> entityBOList = new List<GuestBillPaymentInvoiceReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomRegistrationPaymentForDrCr_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestBillPaymentInvoiceReportViewBO entityBO = new GuestBillPaymentInvoiceReportViewBO();

                                entityBO.PaymentMode = reader["PaymentMode"].ToString();
                                entityBO.Debit = Convert.ToDecimal(reader["Debit"]);
                                entityBO.Credit = Convert.ToDecimal(reader["Credit"]);
                                entityBO.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                entityBO.RegistrationNumber = (reader["RegistrationNumber"].ToString());
                                entityBO.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                entityBO.Narration = (reader["Narration"].ToString());
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<Guid> GetBillPaidForInfoByRegistrationId(int currentRegistrationId)
        {
            List<Guid> rooms = new List<Guid>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBillPaidForInfoByRegistrationId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, currentRegistrationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "CheckOut");

                    if (ds.Tables[0].Rows.Count > 0 )
                        rooms = ds.Tables[0].AsEnumerable().Select(r => (Guid)r["BillPaidByGuidId"]).ToList();
                }
            }
            return rooms;
        }
    }
}
