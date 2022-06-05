using HotelManagement.Data.Banquet;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Banquet;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.Synchronization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.Synchronization
{
    public class DataSyncDA : BaseService
    {
        HotelGuestBillApprovedDA guestBillApprovedDA;
        GuestHouseCheckOutDA checkOutDA;
        HotelGuestDayLetCheckOutDA dayLateCheckOutDA;
        RestaurentBillDA billDA;
        KotBillMasterDA kotBillMasterDA;
        HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
        RestaurantKotSpecialRemarksDetailDA remarksDetailDA;
        BanquetReservationDA banquetDA;
        BanquetReservationDetailDA banquetDetailDA;

        public DataSyncDA()
        {
            guestBillApprovedDA = new HotelGuestBillApprovedDA();
            checkOutDA = new GuestHouseCheckOutDA();
            dayLateCheckOutDA = new HotelGuestDayLetCheckOutDA();
            billDA = new RestaurentBillDA();
            kotBillMasterDA = new KotBillMasterDA();
            remarksDetailDA = new RestaurantKotSpecialRemarksDetailDA();
            banquetDA = new BanquetReservationDA();
            banquetDetailDA = new BanquetReservationDetailDA();
        }

        public List<RegistrationDataSyncViewBO> GetRegistrationDetailsForSync(DateTime toDate)
        {
            List<RegistrationDataSyncViewBO> roomRegistrationList = new List<RegistrationDataSyncViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRegistrationDetailsForDataSync_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "roomRegistration");
                    DataTable Table = ds.Tables["roomRegistration"];

                    roomRegistrationList = Table.AsEnumerable().Select(r => new RegistrationDataSyncViewBO
                    {

                        RegistrationNumber = r.Field<string>("RegistrationNumber"),
                        RegistrationId = r.Field<int>("RegistrationId"),
                        RoomRate = r.Field<decimal>("RoomRate"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        GuestName = r.Field<string>("GuestName"),
                        DisplayArriveDate = r.Field<string>("Arrival"),
                        DisplayCheckOut = r.Field<string>("Departure"),
                        GuidId = r.Field<Guid?>("GuidId"),
                        IsSyncCompleted = r.Field<bool>("IsSyncCompleted"),
                        IsVatAmountEnable = r.Field<bool?>("IsVatAmountEnable"),
                        IsServiceChargeEnable = r.Field<bool?>("IsServiceChargeEnable"),
                        IsAdditionalChargeEnable = r.Field<bool?>("IsAdditionalChargeEnable"),
                        IsCityChargeEnable = r.Field<bool?>("IsCityChargeEnable"),
                        Nights = r.Field<int>("Nights"),
                        BillPaidBy = r.Field<int>("BillPaidBy")

                    }).ToList();
                }
            }
            return roomRegistrationList;
        }
        public List<RestaurantDataSyncViewBO> GetRestaurantBillsForSync(string registrationIdList, DateTime toDate)
        {
            List<RestaurantDataSyncViewBO> entityBOList = new List<RestaurantDataSyncViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBillsForSync_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationIdList", DbType.String, registrationIdList);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantDataSyncViewBO entityBO = new RestaurantDataSyncViewBO();

                                entityBO.BillId = Convert.ToInt32(reader["BillId"]);
                                if (reader["RegistrationId"] != DBNull.Value)
                                    entityBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                entityBO.BillNumber = reader["BillNumber"].ToString();
                                entityBO.CostCenter = reader["CostCenter"].ToString();
                                if (reader["RoundedGrandTotal"] != DBNull.Value)
                                    entityBO.BillAmount = Convert.ToDecimal(reader["RoundedGrandTotal"]);
                                if (reader["VatAmount"] != DBNull.Value)
                                    entityBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                entityBO.PaymentDescription = reader["PaymentMode"].ToString();
                                if (reader["GuidId"] != DBNull.Value)
                                    entityBO.GuidId = (Guid)reader["GuidId"];
                                entityBO.IsSyncCompleted = Convert.ToBoolean(reader["IsSyncCompleted"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }

            return entityBOList;
        }
        public List<ServiceBillDataSyncViewBO> GetServiceBillsForSync(string registrationIdList, DateTime toDate)
        {
            List<ServiceBillDataSyncViewBO> entityBOList = new List<ServiceBillDataSyncViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetServiceBillsForSync_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationIdList", DbType.String, registrationIdList);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ServiceBillDataSyncViewBO entityBO = new ServiceBillDataSyncViewBO();

                                entityBO.BillId = Convert.ToInt32(reader["ServiceBillId"]);
                                if (reader["RegistrationId"] != DBNull.Value)
                                    entityBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                entityBO.BillNumber = reader["BillNumber"].ToString();
                                entityBO.CostCenter = reader["CostCenter"].ToString();
                                if (reader["TotalCalculatedAmount"] != DBNull.Value)
                                    entityBO.BillAmount = Convert.ToDecimal(reader["TotalCalculatedAmount"]);
                                if (reader["VatAmount"] != DBNull.Value)
                                    entityBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                entityBO.PaymentDescription = reader["PaymentMode"].ToString();
                                if (reader["GuidId"] != DBNull.Value)
                                    entityBO.GuidId = (Guid)reader["GuidId"];
                                entityBO.IsSyncCompleted = Convert.ToBoolean(reader["IsSyncCompleted"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }

            return entityBOList;
        }
        public List<BanquetBillDataSyncViewBO> GetBanquetBillsForSync(string registrationIdList, DateTime toDate)
        {
            List<BanquetBillDataSyncViewBO> entityBOList = new List<BanquetBillDataSyncViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetBillsForSync_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationIdList", DbType.String, registrationIdList);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetBillDataSyncViewBO entityBO = new BanquetBillDataSyncViewBO();

                                entityBO.BillId = Convert.ToInt32(reader["Id"]);
                                if (reader["RegistrationId"] != DBNull.Value)
                                    entityBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                entityBO.BillNumber = reader["ReservationNumber"].ToString();
                                entityBO.CostCenter = reader["CostCenter"].ToString();
                                if (reader["RoundedGrandTotal"] != DBNull.Value)
                                    entityBO.BillAmount = Convert.ToDecimal(reader["RoundedGrandTotal"]);
                                if (reader["VatAmount"] != DBNull.Value)
                                    entityBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                entityBO.PaymentDescription = reader["PaymentMode"].ToString();
                                if (reader["GuidId"] != DBNull.Value)
                                    entityBO.GuidId = (Guid)reader["GuidId"];
                                entityBO.IsSyncCompleted = Convert.ToBoolean(reader["IsSyncCompleted"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }

            return entityBOList;
        }

        public RegistrationDataSyncBO GetRegistrationRelatedDataToSync(int registrationId, DateTime toDate)
        {
            RegistrationDataSyncBO registrationSyncBO = new RegistrationDataSyncBO();
            List<HotelRegistrationAireportPickupDropBO> AirportPickupDrops = new List<HotelRegistrationAireportPickupDropBO>();
            HMCommonSetupBO setUpBO = commonSetupDA.GetCommonConfigurationInfo("LastSyncDateTime", "LastSyncDateTime");
            DateTime lastSyncDateTime = DateTime.Now;

            if (setUpBO.SetupValue != null)
                lastSyncDateTime = Convert.ToDateTime(setUpBO.SetupValue);

            RegistrationDataSyncViewBO syncBO = FindSyncedRegistrationByRegistrationId(registrationId);

            registrationSyncBO.RoomRegistration = GetRoomRegistrationInfoByRegistrationId(registrationId);

            if (syncBO != null)
            {
                registrationSyncBO.GuidId = syncBO.GuidId;

                if (registrationSyncBO.RoomRegistration.LastModifiedDate == null || (registrationSyncBO.RoomRegistration.LastModifiedDate > toDate || registrationSyncBO.RoomRegistration.LastModifiedDate <= lastSyncDateTime))
                {
                    registrationSyncBO.RoomRegistration = null;
                }
                registrationSyncBO.GuestBillPayments = GetGuestBillPayments(registrationId)
                    .Where(g => g.CreatedDate > lastSyncDateTime && g.CreatedDate <= toDate && g.ServiceBillId == null).ToList();

                registrationSyncBO.CompanyPayments = GetCompanyPaymentLedgerByRegistrationId(registrationId)
                        .Where(c => c.CreatedDate > lastSyncDateTime && c.CreatedDate <= toDate).ToList();

                registrationSyncBO.ApprovedHotelGuestBills = guestBillApprovedDA.GetHotelGuestApprovedBillByRegistrationId(registrationId)
                            .Where(g => g.CreatedDate > lastSyncDateTime && g.CreatedDate <= toDate).ToList();

                registrationSyncBO.GuestHouseCheckOut = checkOutDA.GetGuestHouseCheckOutInfoByRegistrationId(registrationId);

                if (registrationSyncBO.GuestHouseCheckOut != null && registrationSyncBO.GuestHouseCheckOut.CreatedDate > toDate
                                        && registrationSyncBO.GuestHouseCheckOut.CreatedDate < lastSyncDateTime)
                    registrationSyncBO.GuestHouseCheckOut = null;
                if (registrationSyncBO.GuestHouseCheckOut != null)
                {
                    registrationSyncBO.IsSyncCompleted = true;
                }

                registrationSyncBO.HotelGuestDayLateCheckOut = GetDayLateInformation(registrationId);

                if (registrationSyncBO.HotelGuestDayLateCheckOut != null && registrationSyncBO.HotelGuestDayLateCheckOut.CreatedDate > toDate &&
                                registrationSyncBO.HotelGuestDayLateCheckOut.CreatedDate < lastSyncDateTime)
                    registrationSyncBO.HotelGuestDayLateCheckOut = null;
            }
            else
            {
                //For New Registration To sync
                registrationSyncBO.GuidId = registrationSyncBO.RoomRegistration.GuidId;

                registrationSyncBO.GuestRegistrationMappings = GetGuestRegistrationMappingsByRegistrationId(registrationId);

                registrationSyncBO.Guests = GetRegistrationGuestByRegistrationId(registrationId);

                AirportPickupDrops = GetHotelRegistrationAireportPickupDropByRegistrationId(registrationId);
                //Convert Timespan properties to DateTime
                registrationSyncBO.AirportPickupDrops = (from a in AirportPickupDrops
                                                         select new HotelRegistrationAireportPickupDropViewBO
                                                         {
                                                             APDId = a.APDId,
                                                             RegistrationId = a.RegistrationId,
                                                             ArrivalFlightName = a.ArrivalFlightName,
                                                             ArrivalFlightNumber = a.ArrivalFlightNumber,
                                                             ArrivalTime = new DateTime() + (a.ArrivalTime == null ? new TimeSpan(0, 0, 0, 0) : a.ArrivalTime),
                                                             DepartureAirlineId = a.DepartureAirlineId,
                                                             DepartureFlightName = a.DepartureFlightName,
                                                             DepartureFlightNumber = a.DepartureFlightNumber,
                                                             DepartureTime = new DateTime() + a.DepartureTime

                                                         }).ToList();
                registrationSyncBO.GuestBillPayments = GetGuestBillPayments(registrationId)
                    .Where(g => g.CreatedDate <= toDate && g.ServiceBillId == null).ToList();

                registrationSyncBO.CompanyPayments = GetCompanyPaymentLedgerByRegistrationId(registrationId)
                        .Where(c => c.CreatedDate <= toDate).ToList();

                registrationSyncBO.ApprovedHotelGuestBills = guestBillApprovedDA.GetHotelGuestApprovedBillByRegistrationId(registrationId)
                            .Where(g => g.CreatedDate <= toDate).ToList();

                registrationSyncBO.GuestHouseCheckOut = checkOutDA.GetGuestHouseCheckOutInfoByRegistrationId(registrationId);

                if (registrationSyncBO.GuestHouseCheckOut != null && registrationSyncBO.GuestHouseCheckOut.CreatedDate > toDate)
                    registrationSyncBO.GuestHouseCheckOut = null;
                if (registrationSyncBO.GuestHouseCheckOut != null)
                {
                    registrationSyncBO.IsSyncCompleted = true;
                }

                registrationSyncBO.HotelGuestDayLateCheckOut = GetDayLateInformation(registrationId);

                if (registrationSyncBO.HotelGuestDayLateCheckOut != null && registrationSyncBO.HotelGuestDayLateCheckOut.CreatedDate > toDate)
                    registrationSyncBO.HotelGuestDayLateCheckOut = null;
            }

            registrationSyncBO.BillPaidForGuidId = checkOutDA.GetBillPaidForInfoByRegistrationId(registrationId);
            return registrationSyncBO;
        }
        public ServiceBillDataSyncBO GetServiceBillRelatedDataToSync(int serviceBillId)
        {
            ServiceBillDataSyncBO serviceBillSyncBO = new ServiceBillDataSyncBO();

            string moduleName = "FrontOffice";

            ServiceBillDataSyncViewBO syncViewBo = FindSyncedServiceBillByBillId(serviceBillId);

            if (syncViewBo == null)
            {
                serviceBillSyncBO.ServiceBill = GetHotelGuestServiceBillById(serviceBillId);
                serviceBillSyncBO.GuestBillPayments = GetGuestBillPaymentsByModuleNameAndServiceBillId(moduleName, serviceBillId);
                serviceBillSyncBO.CompanyPayments = GetCompanyPaymentLedgerByModuleNameAndBillId(serviceBillId, "Front Office");
                serviceBillSyncBO.IsSyncCompleted = true;
            }
            else
            {
                serviceBillSyncBO = null;
            }

            return serviceBillSyncBO;
        }
        public RestaurantDataSyncBO GetRestaurantRelatedDataToSync(int billId)
        {
            RestaurantDataSyncBO restaurantDataSync = new RestaurantDataSyncBO();

            string moduleName = "Restaurant";
            RestaurantDataSyncViewBO restaurantDataSyncView = FindSyncedRestaurantBillByBillId(billId);
            if (restaurantDataSyncView == null)
            {
                restaurantDataSync.Bill = GetBillInfoByBillId(billId);
                restaurantDataSync.BillDetails = billDA.GetRestaurantBillDetailsByBillId(billId);
                restaurantDataSync.BillClassificationDiscounts = GetRestaurantBillClassificationDiscountInfoByBillId(billId);

                foreach (var item in restaurantDataSync.BillDetails)
                {
                    restaurantDataSync.KotBillMasters.Add(kotBillMasterDA.GetKotBillMasterInfoKotId(item.KotId));
                    restaurantDataSync.KotBillDetails.AddRange(GetRestaurantKotBillDetailsSync(item.KotId));
                    restaurantDataSync.KotSpecialRemarksDetails.AddRange(GetRestaurantKotSpecialRemarksDetailByKotId(item.KotId));
                }
                restaurantDataSync.GuestBillPayments = GetGuestBillPaymentsByModuleNameAndServiceBillId(moduleName, billId);
                restaurantDataSync.GuestExtraServiceApprovedBills = GetGuestExtraServiceBillApprovedByServiceBillId(billId);
                restaurantDataSync.CompanyPayments = GetCompanyPaymentLedgerByModuleNameAndBillId(billId, moduleName);
                restaurantDataSync.IsSyncCompleted = true;
            }
            else
            {
                return null;
            }
            return restaurantDataSync;
        }
        public BanquetDataSyncBO GetBanquetRelatedDataToSync(long reservationId)
        {
            BanquetDataSyncBO reservationDataSync = new BanquetDataSyncBO();

            string moduleName = "Banquet";
            BanquetBillDataSyncViewBO syncViewBo = FindSyncedBanquetBillByBillId(reservationId);

            if (syncViewBo == null)
            {
                reservationDataSync.BanquetReservation = GetBanquetReservationInfoById(reservationId);
                reservationDataSync.BanquetReservationDetails = GetBanquetReservationDetailInfoByReservationId(reservationId);
                reservationDataSync.ClassificationDiscounts = banquetDA.GetBanquetReservationClassificationDiscount((int)reservationId);
                reservationDataSync.GuestBillPayments = GetGuestBillPaymentsByModuleNameAndServiceBillId(moduleName, reservationId);
                reservationDataSync.CompanyPayments = GetCompanyPaymentLedgerByModuleNameAndBillId((int)reservationId, moduleName);
                reservationDataSync.IsSyncCompleted = true;
            }
            else
            {
                reservationDataSync = null;
            }
            return reservationDataSync;
        }

        public bool SaveOrUpdateRegistrationSyncInformation(Guid id, bool IsSyncCompleted)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateSyncRegistrationInformation_SP"))
                {
                    command.Parameters.Clear();

                    dbSmartAspects.AddInParameter(command, "@GuidId", DbType.Guid, id);
                    dbSmartAspects.AddInParameter(command, "@IsSyncCompleted", DbType.Boolean, IsSyncCompleted);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }

            }
            return status;
        }
        public bool SaveOrUpdateServiceBillSyncInformation(Guid id, bool IsSyncCompleted)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateSyncServiceBillInformation_SP"))
                {
                    command.Parameters.Clear();

                    dbSmartAspects.AddInParameter(command, "@GuidId", DbType.Guid, id);
                    dbSmartAspects.AddInParameter(command, "@IsSyncCompleted", DbType.Boolean, IsSyncCompleted);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }

            }
            return status;
        }
        public bool SaveOrUpdateRestaurantSyncInformation(Guid id, bool IsSyncCompleted)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateSyncRestaurantInformation_SP"))
                {
                    command.Parameters.Clear();

                    dbSmartAspects.AddInParameter(command, "@GuidId", DbType.Guid, id);
                    dbSmartAspects.AddInParameter(command, "@IsSyncCompleted", DbType.Boolean, IsSyncCompleted);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }

            }
            return status;
        }
        public bool SaveOrUpdateBanquetSyncInformation(Guid id, bool IsSyncCompleted)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateSyncBanquetInformation_SP"))
                {
                    command.Parameters.Clear();

                    dbSmartAspects.AddInParameter(command, "@GuidId", DbType.Guid, id);
                    dbSmartAspects.AddInParameter(command, "@IsSyncCompleted", DbType.Boolean, IsSyncCompleted);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }

            }
            return status;
        }

        public bool SaveSyncDataForSyncLater(List<TemporarySync> billList)
        {
            Boolean status = false;

            foreach (var bill in billList)
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveSyncDataTemporary_SP"))
                    {
                        command.Parameters.Clear();

                        dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, bill.BillId);
                        dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, bill.BillType);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }

                }

            }
            return status;
        }
        public bool DeleteAllTemporarySyncData()
        {
            Boolean status = false;
            string sqlCommand = "TRUNCATE TABLE TemporarySync";
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetSqlStringCommand(sqlCommand))
                    {
                        dbSmartAspects.ExecuteNonQuery(command);
                        status = true;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        public List<TemporarySync> GetTemporarySyncData()
        {
            List<TemporarySync> billList = new List<TemporarySync>();
            string sqlCommand = "SELECT * FROM  TemporarySync";

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetSqlStringCommand(sqlCommand))
                    {
                        DataSet dataSet = new DataSet();
                        dbSmartAspects.LoadDataSet(command, dataSet, "SyncData");

                        DataTable table = dataSet.Tables["SyncData"];

                        billList = table.AsEnumerable().Select(r => new TemporarySync()
                        {
                            BillId = r.Field<int>("BillId"),
                            BillType = r.Field<string>("BillType")
                        }).ToList();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return billList;
        }

        private HotelRoomRegistration GetRoomRegistrationInfoByRegistrationId(int registrationId)
        {
            //string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            HotelRoomRegistration roomRegistration = new HotelRoomRegistration();
            string query = string.Format("SELECT * FROM HotelRoomRegistration WHERE RegistrationId = {0} ", registrationId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomRegistration.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                roomRegistration.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                if (reader["ArriveDate"] != DBNull.Value)
                                    roomRegistration.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                if (reader["ExpectedCheckOutDate"] != DBNull.Value)
                                    roomRegistration.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);

                                if (reader["BillingStartDate"] != DBNull.Value)
                                    roomRegistration.BillingStartDate = Convert.ToDateTime(reader["BillingStartDate"]);
                                if (reader["BillHoldUpDate"] != DBNull.Value)
                                    roomRegistration.BillHoldUpDate = Convert.ToDateTime(reader["BillHoldUpDate"]);

                                if (reader["CheckOutDate"] != DBNull.Value)
                                {
                                    roomRegistration.CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"]);
                                    roomRegistration.CheckOutDateForAPI = Convert.ToDateTime(reader["CheckOutDate"]);
                                }

                                if (reader["ActualCheckOutDate"] != DBNull.Value)
                                    roomRegistration.ActualCheckOutDate = Convert.ToDateTime(reader["ActualCheckOutDate"]);

                                if (reader["RoomId"] != DBNull.Value)
                                    roomRegistration.RoomId = Convert.ToInt32(reader["RoomId"]);
                                if (reader["EntitleRoomType"] != DBNull.Value)
                                    roomRegistration.EntitleRoomType = Convert.ToInt32(reader["EntitleRoomType"]);
                                if (reader["CurrencyType"] != DBNull.Value)
                                    roomRegistration.CurrencyType = Convert.ToInt32(reader["CurrencyType"]);                                //roomRegistration.Currency = Convert.ToString(reader["Currency"]);
                                if (reader["ConversionRate"] != DBNull.Value)
                                    roomRegistration.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                if (reader["UnitPrice"] != DBNull.Value)
                                    roomRegistration.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                roomRegistration.DiscountType = reader["DiscountType"].ToString();
                                if (reader["DiscountAmount"] != DBNull.Value)
                                    roomRegistration.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                roomRegistration.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                if (reader["NoShowCharge"] != DBNull.Value)
                                    roomRegistration.NoShowCharge = Convert.ToDecimal(reader["NoShowCharge"]);
                                if (reader["IsServiceChargeEnable"] != DBNull.Value)
                                    roomRegistration.IsServiceChargeEnable = Convert.ToBoolean(reader["IsServiceChargeEnable"]);
                                if (reader["IsCityChargeEnable"] != DBNull.Value)
                                    roomRegistration.IsCityChargeEnable = Convert.ToBoolean(reader["IsCityChargeEnable"]);
                                if (reader["IsVatAmountEnable"] != DBNull.Value)
                                    roomRegistration.IsVatAmountEnable = Convert.ToBoolean(reader["IsVatAmountEnable"]);
                                if (reader["IsAdditionalChargeEnable"] != DBNull.Value)
                                    roomRegistration.IsAdditionalChargeEnable = Convert.ToBoolean(reader["IsAdditionalChargeEnable"]);
                                if (reader["TotalRoomRate"] != DBNull.Value)
                                    roomRegistration.TotalRoomRate = Convert.ToDecimal(reader["TotalRoomRate"]);
                                if (reader["IsCompanyGuest"] != DBNull.Value)
                                    roomRegistration.IsCompanyGuest = Convert.ToBoolean(reader["IsCompanyGuest"]);
                                if (reader["IsHouseUseRoom"] != DBNull.Value)
                                    roomRegistration.IsHouseUseRoom = Convert.ToBoolean(reader["IsHouseUseRoom"]);

                                roomRegistration.CommingFrom = reader["CommingFrom"].ToString();

                                roomRegistration.NextDestination = reader["NextDestination"].ToString();
                                roomRegistration.VisitPurpose = reader["VisitPurpose"].ToString();
                                if (reader["IsFromReservation"] != DBNull.Value)
                                    roomRegistration.IsFromReservation = Convert.ToBoolean(reader["IsFromReservation"]);
                                if (reader["ReservationId"] != DBNull.Value)
                                    roomRegistration.ReservationId = Convert.ToInt32(reader["ReservationId"]);                                //roomRegistration.ReservationInfo = reader["ReservationInfo"].ToString();
                                if (reader["IsFamilyOrCouple"] != DBNull.Value)
                                    roomRegistration.IsFamilyOrCouple = Convert.ToBoolean(reader["IsFamilyOrCouple"]);
                                if (reader["NumberOfPersonAdult"] != DBNull.Value)
                                    roomRegistration.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"]);
                                if (reader["NumberOfPersonChild"] != DBNull.Value)
                                    roomRegistration.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"]);
                                if (reader["IsListedCompany"] != DBNull.Value)
                                    roomRegistration.IsListedCompany = Convert.ToBoolean(reader["IsListedCompany"]);
                                roomRegistration.ReservedCompany = reader["ReservedCompany"].ToString();
                                if (reader["CompanyId"] != DBNull.Value)
                                    roomRegistration.CompanyId = Convert.ToInt32(reader["CompanyId"]);                                //roomRegistration.CompanyName = reader["CompanyName"].ToString();
                                roomRegistration.ContactPerson = reader["ContactPerson"].ToString();
                                roomRegistration.ContactNumber = reader["ContactNumber"].ToString();
                                roomRegistration.PaymentMode = reader["PaymentMode"].ToString();
                                if (reader["PayFor"] != DBNull.Value)
                                    roomRegistration.PayFor = Convert.ToInt32(reader["PayFor"]);
                                if (reader["BusinessPromotionId"] != DBNull.Value)
                                    roomRegistration.BusinessPromotionId = Convert.ToInt32(reader["BusinessPromotionId"]);
                                if (reader["IsRoomOwner"] != DBNull.Value)
                                    roomRegistration.IsRoomOwner = Convert.ToInt32(reader["IsRoomOwner"]);
                                if (reader["GuestSourceId"] != DBNull.Value)
                                    roomRegistration.GuestSourceId = Convert.ToInt32(reader["GuestSourceId"]);

                                if (reader["MealPlanId"] != DBNull.Value)
                                    roomRegistration.MealPlanId = Convert.ToInt32(reader["MealPlanId"]);
                                if (reader["ReferenceId"] != DBNull.Value)
                                    roomRegistration.ReferenceId = Convert.ToInt32(reader["ReferenceId"]);
                                if (reader["IsReturnedGuest"] != DBNull.Value)
                                    roomRegistration.IsReturnedGuest = Convert.ToBoolean(reader["IsReturnedGuest"]);

                                if (reader["IsVIPGuest"] != DBNull.Value)
                                    roomRegistration.IsVIPGuest = Convert.ToBoolean(reader["IsVIPGuest"]);
                                if (reader["VipGuestTypeId"] != DBNull.Value)
                                    roomRegistration.VipGuestTypeId = Convert.ToInt32(reader["VipGuestTypeId"]);
                                roomRegistration.Remarks = reader["Remarks"].ToString();
                                roomRegistration.AirportPickUp = reader["AirportPickUp"].ToString();
                                roomRegistration.AirportDrop = reader["AirportDrop"].ToString();
                                roomRegistration.CardType = reader["CardType"].ToString();
                                roomRegistration.CardNumber = reader["CardNumber"].ToString();
                                if (reader["CardExpireDate"] != DBNull.Value)
                                    roomRegistration.CardExpireDate = Convert.ToDateTime(reader["CardExpireDate"]);

                                roomRegistration.CardHolderName = reader["CardHolderName"].ToString();
                                roomRegistration.CardReference = reader["CardReference"].ToString();
                                if (reader["IsStopChargePosting"] != DBNull.Value)
                                    roomRegistration.IsStopChargePosting = Convert.ToBoolean(reader["IsStopChargePosting"]);
                                if (reader["IsBlankRegistrationCard"] != DBNull.Value)
                                    roomRegistration.IsBlankRegistrationCard = Convert.ToBoolean(reader["IsBlankRegistrationCard"]);
                                if (reader["CreatedBy"] != DBNull.Value)
                                    roomRegistration.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                if (reader["CreatedDate"] != DBNull.Value)
                                    roomRegistration.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                if (reader["LastModifiedBy"] != DBNull.Value)
                                    roomRegistration.LastModifiedBy = Convert.ToInt32(reader["LastModifiedBy"]);
                                if (reader["LastModifiedDate"] != DBNull.Value)
                                    roomRegistration.LastModifiedDate = Convert.ToDateTime(reader["LastModifiedDate"]);
                                roomRegistration.IsEarlyCheckInChargeEnable = Convert.ToBoolean(reader["IsEarlyCheckInChargeEnable"]);
                                roomRegistration.POSRemarks = reader["POSRemarks"].ToString();
                                if (reader["HoldUpAmount"] != DBNull.Value)
                                    roomRegistration.HoldUpAmount = Convert.ToDecimal(reader["HoldUpAmount"]);
                                if (reader["GuidId"] != DBNull.Value)
                                    roomRegistration.GuidId = (Guid)reader["GuidId"];
                            }
                        }
                    }
                }
            }
            return roomRegistration;
        }
        public List<GuestRegistrationMappingBO> GetGuestRegistrationMappingsByRegistrationId(int registrationId)
        {
            List<GuestRegistrationMappingBO> guestList = new List<GuestRegistrationMappingBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestRegistrationMappingByRegistrationId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int64, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestRegistrationMappingBO guest = new GuestRegistrationMappingBO();

                                guest.GuestRegistrationId = Convert.ToInt64(reader["GuestRegistrationId"]);
                                guest.RegistrationId = Convert.ToInt64(reader["RegistrationId"]);
                                guest.GuestId = Convert.ToInt64(reader["GuestId"]);

                                if (reader["CheckInDate"] != DBNull.Value)
                                    guest.CheckInDate = Convert.ToDateTime(reader["CheckInDate"]);
                                if (reader["CheckOutDate"] != DBNull.Value)
                                    guest.CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"]);
                                if (reader["PaxInRate"] != DBNull.Value)
                                    guest.PaxInRate = Convert.ToDecimal(reader["PaxInRate"]);

                                guestList.Add(guest);
                            }
                        }
                    }
                }
            }
            return guestList;
        }
        public List<HotelGuestInformation> GetRegistrationGuestByRegistrationId(int registrationId)
        {
            List<HotelGuestInformation> guestList = new List<HotelGuestInformation>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestByRegistrationIdForSync_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "registrationList");
                    DataTable Table = ds.Tables["registrationList"];

                    guestList = Table.AsEnumerable().Select(r => new HotelGuestInformation
                    {
                        GuestId = r.Field<int>("GuestId"),
                        Title = r.Field<string>("Title"),
                        FirstName = r.Field<string>("FirstName"),
                        LastName = r.Field<string>("LastName"),
                        GuestName = r.Field<string>("GuestName"),
                        GuestDOB = r.Field<DateTime?>("GuestDOB"),
                        GuestSex = r.Field<string>("GuestSex"),
                        GuestEmail = r.Field<string>("GuestEmail"),
                        ProfessionId = r.Field<int?>("ProfessionId"),
                        GuestPhone = r.Field<string>("GuestPhone"),
                        GuestAddress1 = r.Field<string>("GuestAddress1"),
                        GuestAddress2 = r.Field<string>("GuestAddress2"),
                        GuestCity = r.Field<string>("GuestCity"),
                        GuestZipCode = r.Field<string>("GuestZipCode"),
                        GuestCountryId = r.Field<int?>("GuestCountryId"),
                        GuestNationality = r.Field<string>("GuestNationality"),
                        GuestDrivinlgLicense = r.Field<string>("GuestDrivinlgLicense"),
                        GuestAuthentication = r.Field<string>("GuestAuthentication"),
                        NationalId = r.Field<string>("NationalId"),
                        PassportNumber = r.Field<string>("PassportNumber"),
                        PIssueDate = r.Field<DateTime?>("PIssueDate"),
                        PIssuePlace = r.Field<string>("PIssuePlace"),
                        PExpireDate = r.Field<DateTime?>("PExpireDate"),
                        VisaNumber = r.Field<string>("VisaNumber"),
                        VIssueDate = r.Field<DateTime?>("VIssueDate"),
                        VExpireDate = r.Field<DateTime?>("VExpireDate"),
                        GuestPreferences = r.Field<string>("GuestPreferences"),
                        ClassificationId = r.Field<int?>("ClassificationId"),
                        GuestBlock = r.Field<bool?>("GuestBlock"),
                        Description = r.Field<string>("Description")

                    }).ToList();
                }
            }

            return guestList;
        }
        public List<HotelGuestBillPaymentBO> GetGuestBillPayments(int registrationId)
        {
            List<HotelGuestBillPaymentBO> paymentList = new List<HotelGuestBillPaymentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillByRegistrationId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "paymentList");
                    DataTable Table = ds.Tables["paymentList"];

                    paymentList = Table.AsEnumerable().Select(r => new HotelGuestBillPaymentBO
                    {

                        PaymentId = r.Field<int>("PaymentId"),
                        BillNumber = r.Field<string>("BillNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        PaymentType = r.Field<string>("PaymentType"),
                        ServiceBillId = r.Field<int?>("ServiceBillId"),
                        RegistrationId = r.Field<int?>("RegistrationId"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        PaymentDate = r.Field<DateTime?>("PaymentDate"),
                        TransactionDate = r.Field<DateTime?>("TransactionDate"),
                        PaymentMode = r.Field<string>("PaymentMode"),
                        PaymentModeId = r.Field<int?>("PaymentModeId"),
                        FieldId = r.Field<int?>("FieldId"),
                        CurrencyAmount = r.Field<decimal?>("CurrencyAmount"),
                        PaymentAmount = r.Field<decimal?>("PaymentAmount"),
                        PaymentDescription = r.Field<string>("PaymentDescription"),
                        BankId = r.Field<int?>("BankId"),
                        BranchName = r.Field<string>("BranchName"),
                        ChecqueNumber = r.Field<string>("ChecqueNumber"),
                        ChecqueDate = r.Field<DateTime?>("ChecqueDate"),
                        CardType = r.Field<string>("CardType"),
                        CardNumber = r.Field<string>("CardNumber"),
                        ExpireDate = r.Field<DateTime?>("ExpireDate"),
                        CardHolderName = r.Field<string>("CardHolderName"),
                        CardReference = r.Field<string>("CardReference"),
                        AccountsPostingHeadId = r.Field<int?>("AccountsPostingHeadId"),
                        RefundAccountHead = r.Field<int?>("RefundAccountHead"),
                        Remarks = r.Field<string>("Remarks"),
                        DealId = r.Field<int?>("DealId"),
                        ServiceRate = r.Field<decimal?>("ServiceRate"),
                        ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                        CitySDCharge = r.Field<decimal?>("CitySDCharge"),
                        VatAmount = r.Field<decimal?>("VatAmount"),
                        AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                        CurrencyExchangeRate = r.Field<decimal?>("CurrencyExchangeRate"),
                        CreatedBy = r.Field<int?>("CreatedBy"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedDate"),
                        IsAdvancePayment = r.Field<bool>("IsAdvancePayment"),
                        ReservationId = r.Field<long>("ReservationId"),
                        RegistrationGuiId = r.Field<Guid?>("GuidId")

                    }).ToList();
                }
            }
            return paymentList;
        }

        private List<HotelGuestServiceBillBO> GetHotelGuestServiceBillByRegistrationId(int registrationId)
        {
            List<HotelGuestServiceBillBO> hotelGuestServiceBillList = new List<HotelGuestServiceBillBO>();

            string query = string.Format("SELECT * FROM HotelGuestServiceBill WHERE RegistrationId = {0}", registrationId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "hotelGuestServiceBill");
                    DataTable Table = ds.Tables["hotelGuestServiceBill"];


                    hotelGuestServiceBillList = Table.AsEnumerable().Select(r => new HotelGuestServiceBillBO
                    {

                        ServiceBillId = r.Field<int>("ServiceBillId"),
                        ServiceDate = r.Field<DateTime?>("ServiceDate"),
                        BillNumber = r.Field<string>("BillNumber"),
                        RegistrationId = r.Field<int?>("RegistrationId"),
                        GuestName = r.Field<string>("GuestName"),
                        ServiceId = r.Field<int?>("ServiceId"),
                        ServiceRate = r.Field<decimal?>("ServiceRate"),
                        ServiceQuantity = r.Field<int?>("ServiceQuantity"),
                        DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                        IsComplementary = r.Field<bool?>("IsComplementary"),
                        IsPaidService = r.Field<bool?>("IsPaidService"),
                        IsPaidServiceAchieved = r.Field<bool?>("IsPaidServiceAchieved"),
                        Remarks = r.Field<string>("Remarks"),
                        PaymentMode = r.Field<string>("PaymentMode"),
                        EmpId = r.Field<int?>("EmpId"),
                        CompanyId = r.Field<int?>("CompanyId"),
                        RackRate = r.Field<decimal?>("RackRate"),
                        ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                        CitySDCharge = r.Field<decimal?>("CitySDCharge"),
                        AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                        VatAmount = r.Field<decimal?>("VatAmount"),
                        InvoiceRackRate = r.Field<decimal?>("InvoiceRackRate"),
                        IsServiceChargeEnable = r.Field<bool?>("IsServiceChargeEnable"),
                        InvoiceServiceCharge = r.Field<decimal?>("InvoiceServiceCharge"),
                        IsCitySDChargeEnable = r.Field<bool?>("IsCitySDChargeEnable"),
                        InvoiceCitySDCharge = r.Field<decimal?>("InvoiceCitySDCharge"),
                        IsVatAmountEnable = r.Field<bool?>("IsVatAmountEnable"),
                        InvoiceVatAmount = r.Field<decimal?>("InvoiceVatAmount"),
                        IsAdditionalChargeEnable = r.Field<bool?>("IsAdditionalChargeEnable"),
                        InvoiceAdditionalCharge = r.Field<decimal?>("InvoiceAdditionalCharge"),
                        TotalCalculatedAmount = r.Field<decimal?>("TotalCalculatedAmount"),
                        CurrencyExchangeRate = r.Field<decimal?>("CurrencyExchangeRate"),
                        InvoiceUsdRackRate = r.Field<decimal?>("InvoiceUsdRackRate"),
                        InvoiceUsdServiceCharge = r.Field<decimal?>("InvoiceUsdServiceCharge"),
                        InvoiceUsdVatAmount = r.Field<decimal?>("InvoiceUsdVatAmount"),
                        ReferenceServiceBillId = r.Field<int?>("ReferenceServiceBillId"),
                        ApprovedStatus = r.Field<bool?>("ApprovedStatus"),
                        ApprovedDate = r.Field<DateTime?>("ApprovedDate"),
                        CreatedBy = r.Field<int?>("CreatedBy"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedDate"),
                        ReferenceBillNumber = r.Field<string>("ReferenceBillNumber")

                    }).ToList();
                }
            }


            return hotelGuestServiceBillList;
        }
        private List<HotelRegistrationAireportPickupDropBO> GetHotelRegistrationAireportPickupDropByRegistrationId(int registrationId)
        {
            List<HotelRegistrationAireportPickupDropBO> pickUpDrops = new List<HotelRegistrationAireportPickupDropBO>();

            string query = string.Format("SELECT * FROM HotelRegistrationAireportPickupDrop WHERE RegistrationId = {0}", registrationId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "pickUpDrop");
                    DataTable Table = ds.Tables["pickUpDrop"];

                    pickUpDrops = Table.AsEnumerable().Select(r => new HotelRegistrationAireportPickupDropBO
                    {
                        APDId = r.Field<int>("APDId"),
                        RegistrationId = r.Field<int?>("RegistrationId"),
                        ArrivalFlightName = r.Field<string>("ArrivalFlightName"),
                        ArrivalFlightNumber = r.Field<string>("ArrivalFlightNumber"),
                        ArrivalTime = r.Field<TimeSpan?>("ArrivalTime"),
                        DepartureAirlineId = r.Field<int?>("DepartureAirlineId"),
                        DepartureFlightName = r.Field<string>("DepartureFlightName"),
                        DepartureFlightNumber = r.Field<string>("DepartureFlightNumber"),
                        DepartureTime = r.Field<TimeSpan?>("DepartureTime"),
                        IsDepartureChargable = r.Field<bool?>("IsDepartureChargable")

                    }).ToList();
                }
            }

            return pickUpDrops;
        }
        private HotelGuestDayLetCheckOut GetDayLateInformation(int registrationId)
        {
            HotelGuestDayLetCheckOut bo = null;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDayLetsInformationByRegId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bo = new HotelGuestDayLetCheckOut();

                                bo.DayLetId = Convert.ToInt32(reader["DayLetId"]);
                                if (reader["RegistrationId"] != DBNull.Value)
                                    bo.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                if (reader["DayLetDiscount"] != DBNull.Value)
                                    bo.RoomRate = Convert.ToDecimal(reader["DayLetDiscount"]);
                                bo.DayLetDiscountType = reader["DayLetDiscountType"].ToString();
                                if (reader["DayLetDiscount"] != DBNull.Value)
                                    bo.DayLetDiscount = Convert.ToDecimal(reader["DayLetDiscount"]);
                                if (reader["DayLetDiscountAmount"] != DBNull.Value)
                                    bo.DayLetDiscountAmount = Convert.ToDecimal(reader["DayLetDiscountAmount"]);

                                bo.DayLetStatus = reader["DayLetStatus"].ToString();
                                if (reader["CreatedBy"] != DBNull.Value)
                                    bo.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                if (reader["CreatedDate"] != DBNull.Value)
                                    bo.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                if (reader["LastModifiedBy"] != DBNull.Value)
                                    bo.LastModifiedBy = Convert.ToInt32(reader["LastModifiedBy"]);
                                if (reader["LastModifiedDate"] != DBNull.Value)
                                    bo.LastModifiedDate = Convert.ToDateTime(reader["LastModifiedDate"]);
                            }
                        }
                    }
                }
                conn.Close();
            }
            return bo;

        }
        private List<GuestExtraServiceBillApprovedBO> GetGuestExtraServiceBillApprovedByServiceBillId(int billId)
        {
            List<GuestExtraServiceBillApprovedBO> approvedBills = new List<GuestExtraServiceBillApprovedBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestExtraServiceBillApprovedByServiceBillId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, billId);
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BillInfo");
                    DataTable Table = ds.Tables["BillInfo"];

                    approvedBills = Table.AsEnumerable().Select(r => new GuestExtraServiceBillApprovedBO
                    {
                        ApprovedId = r.Field<int>("ApprovedId"),
                        CostCenterId = r.Field<int?>("CostCenterId"),
                        RegistrationId = r.Field<int?>("RegistrationId"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        ApprovedDate = r.Field<DateTime?>("ApprovedDate"),
                        ServiceBillId = r.Field<int>("ServiceBillId"),
                        ServiceDate = r.Field<DateTime?>("ServiceDate"),
                        ServiceType = r.Field<string>("ServiceType"),
                        ServiceId = r.Field<int?>("ServiceId"),
                        ServiceName = r.Field<string>("ServiceName"),
                        ServiceQuantity = r.Field<decimal?>("ServiceQuantity"),
                        ServiceRate = r.Field<decimal?>("ServiceRate"),
                        DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                        VatAmount = r.Field<decimal?>("VatAmount"),
                        ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                        CitySDCharge = r.Field<decimal?>("CitySDCharge"),
                        AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                        InvoiceServiceRate = r.Field<decimal?>("InvoiceServiceRate"),
                        IsServiceChargeEnable = r.Field<bool?>("IsServiceChargeEnable"),
                        InvoiceServiceCharge = r.Field<decimal?>("InvoiceServiceCharge"),
                        IsVatAmountEnable = r.Field<bool?>("IsVatAmountEnable"),
                        InvoiceVatAmount = r.Field<decimal?>("InvoiceVatAmount"),
                        IsAdditionalChargeEnable = r.Field<bool?>("IsAdditionalChargeEnable"),
                        InvoiceAdditionalCharge = r.Field<decimal?>("InvoiceAdditionalCharge"),
                        CurrencyExchangeRate = r.Field<decimal?>("CurrencyExchangeRate"),
                        InvoiceUsdRackRate = r.Field<decimal?>("InvoiceUsdRackRate"),
                        InvoiceUsdServiceCharge = r.Field<decimal?>("InvoiceUsdServiceCharge"),
                        InvoiceUsdVatAmount = r.Field<decimal?>("InvoiceUsdVatAmount"),
                        CalculatedTotalAmount = r.Field<decimal?>("CalculatedTotalAmount"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        PaymentMode = r.Field<string>("PaymentMode"),
                        IsPaidService = r.Field<bool?>("IsPaidService"),
                        IsPaidServiceAchieved = r.Field<bool?>("IsPaidServiceAchieved"),
                        IsDayClosed = r.Field<bool?>("IsDayClosed"),
                        CreatedBy = r.Field<int?>("CreatedBy"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedDate"),
                    }).ToList();
                }
            }

            return approvedBills;
        }
        private RegistrationDataSyncViewBO FindSyncedRegistrationByRegistrationId(int registrationId)
        {
            RegistrationDataSyncViewBO syncBO = null;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSyncedRegistrationByRegistrationId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                syncBO = new RegistrationDataSyncViewBO();
                                syncBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                syncBO.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                syncBO.GuidId = (Guid)(reader["GuidId"]);
                                syncBO.IsSyncCompleted = Convert.ToBoolean(reader["IsSyncCompleted"]);
                            }
                        }
                    }
                }
            }
            return syncBO;
        }

        private HotelGuestServiceBillBO GetHotelGuestServiceBillById(int ServiceBillId)
        {
            HotelGuestServiceBillBO serviceBill = new HotelGuestServiceBillBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelGuestServiceBillById_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, ServiceBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "hotelGuestServiceBill");
                    DataTable Table = ds.Tables["hotelGuestServiceBill"];

                    serviceBill = Table.AsEnumerable().Select(r => new HotelGuestServiceBillBO
                    {
                        ServiceBillId = r.Field<int>("ServiceBillId"),
                        ServiceDate = r.Field<DateTime?>("ServiceDate"),
                        BillNumber = r.Field<string>("BillNumber"),
                        RegistrationId = r.Field<int?>("RegistrationId"),
                        GuestName = r.Field<string>("GuestName"),
                        ServiceId = r.Field<int?>("ServiceId"),
                        ServiceRate = r.Field<decimal?>("ServiceRate"),
                        ServiceQuantity = r.Field<int?>("ServiceQuantity"),
                        DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                        IsComplementary = r.Field<bool?>("IsComplementary"),
                        IsPaidService = r.Field<bool?>("IsPaidService"),
                        IsPaidServiceAchieved = r.Field<bool?>("IsPaidServiceAchieved"),
                        Remarks = r.Field<string>("Remarks"),
                        PaymentMode = r.Field<string>("PaymentMode"),
                        EmpId = r.Field<int?>("EmpId"),
                        CompanyId = r.Field<int?>("CompanyId"),
                        RackRate = r.Field<decimal?>("RackRate"),
                        ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                        CitySDCharge = r.Field<decimal?>("CitySDCharge"),
                        AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                        VatAmount = r.Field<decimal?>("VatAmount"),
                        InvoiceRackRate = r.Field<decimal?>("InvoiceRackRate"),
                        IsServiceChargeEnable = r.Field<bool?>("IsServiceChargeEnable"),
                        InvoiceServiceCharge = r.Field<decimal?>("InvoiceServiceCharge"),
                        IsCitySDChargeEnable = r.Field<bool?>("IsCitySDChargeEnable"),
                        InvoiceCitySDCharge = r.Field<decimal?>("InvoiceCitySDCharge"),
                        IsVatAmountEnable = r.Field<bool?>("IsVatAmountEnable"),
                        InvoiceVatAmount = r.Field<decimal?>("InvoiceVatAmount"),
                        IsAdditionalChargeEnable = r.Field<bool?>("IsAdditionalChargeEnable"),
                        InvoiceAdditionalCharge = r.Field<decimal?>("InvoiceAdditionalCharge"),
                        TotalCalculatedAmount = r.Field<decimal?>("TotalCalculatedAmount"),
                        CurrencyExchangeRate = r.Field<decimal?>("CurrencyExchangeRate"),
                        InvoiceUsdRackRate = r.Field<decimal?>("InvoiceUsdRackRate"),
                        InvoiceUsdServiceCharge = r.Field<decimal?>("InvoiceUsdServiceCharge"),
                        InvoiceUsdVatAmount = r.Field<decimal?>("InvoiceUsdVatAmount"),
                        ReferenceServiceBillId = r.Field<int?>("ReferenceServiceBillId"),
                        ApprovedStatus = r.Field<bool?>("ApprovedStatus"),
                        ApprovedDate = r.Field<DateTime?>("ApprovedDate"),
                        CreatedBy = r.Field<int?>("CreatedBy"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedDate"),
                        ReferenceBillNumber = r.Field<string>("ReferenceBillNumber"),
                        GuidId = r.Field<Guid>("GuidId"),
                        RegistrationGuidId = r.Field<Guid?>("RegistrationGuidId"),

                    }).FirstOrDefault();
                }
            }

            return serviceBill;
        }

        private RestaurantBill GetBillInfoByBillId(int billId)
        {
            RestaurantBill bill = new RestaurantBill();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBillInfoByBillIdForAPI_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BillInfo");
                    DataTable Table = ds.Tables["BillInfo"];

                    bill = Table.AsEnumerable().Select(r => new RestaurantBill()
                    {

                        BillId = r.Field<int>("BillId"),
                        BillDate = r.Field<DateTime?>("BillDate"),
                        BillNumber = r.Field<string>("BillNumber"),
                        IsBillSettlement = r.Field<bool?>("IsBillSettlement"),
                        IsComplementary = r.Field<bool?>("IsComplementary"),
                        IsNonChargeable = r.Field<bool?>("IsNonChargeable"),
                        SourceName = r.Field<string>("SourceName"),
                        BillPaidBySourceId = r.Field<int?>("BillPaidBySourceId"),
                        CostCenterId = r.Field<int?>("CostCenterId"),
                        PaxQuantity = r.Field<int?>("PaxQuantity"),
                        CustomerName = r.Field<string>("CustomerName"),
                        PayMode = r.Field<string>("PayMode"),
                        PayModeSourceId = r.Field<int?>("PayModeSourceId"),
                        PaySourceCurrentBalance = r.Field<decimal?>("PaySourceCurrentBalance"),
                        BankId = r.Field<int?>("BankId"),
                        CardType = r.Field<string>("CardType"),
                        CardNumber = r.Field<string>("CardNumber"),
                        ExpireDate = r.Field<DateTime?>("ExpireDate"),
                        CardHolderName = r.Field<string>("CardHolderName"),
                        RegistrationId = r.Field<int?>("RegistrationId"),
                        SalesAmount = r.Field<decimal?>("SalesAmount"),
                        DiscountType = r.Field<string>("DiscountType"),
                        DiscountTransactionId = r.Field<int?>("DiscountTransactionId"),
                        DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                        CalculatedDiscountAmount = r.Field<decimal?>("CalculatedDiscountAmount"),
                        ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                        CitySDCharge = r.Field<decimal?>("CitySDCharge"),
                        VatAmount = r.Field<decimal?>("VatAmount"),
                        AdditionalChargeType = r.Field<string>("AdditionalChargeType"),
                        AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                        InvoiceServiceRate = r.Field<decimal?>("InvoiceServiceRate"),
                        IsInvoiceServiceChargeEnable = r.Field<bool?>("IsInvoiceServiceChargeEnable"),
                        InvoiceServiceCharge = r.Field<decimal?>("InvoiceServiceCharge"),
                        IsInvoiceCitySDChargeEnable = r.Field<bool?>("IsInvoiceCitySDChargeEnable"),
                        InvoiceCitySDCharge = r.Field<decimal?>("InvoiceCitySDCharge"),
                        IsInvoiceVatAmountEnable = r.Field<bool?>("IsInvoiceVatAmountEnable"),
                        InvoiceVatAmount = r.Field<decimal?>("InvoiceVatAmount"),
                        IsInvoiceAdditionalChargeEnable = r.Field<bool?>("IsInvoiceAdditionalChargeEnable"),
                        InvoiceAdditionalCharge = r.Field<decimal?>("InvoiceAdditionalCharge"),
                        GrandTotal = r.Field<decimal?>("GrandTotal"),
                        RoundedAmount = r.Field<decimal?>("RoundedAmount"),
                        RoundedGrandTotal = r.Field<decimal?>("RoundedGrandTotal"),
                        BillStatus = r.Field<string>("BillStatus"),
                        BillVoidBy = r.Field<int?>("BillVoidBy"),
                        Remarks = r.Field<string>("Remarks"),
                        Reference = r.Field<string>("Reference"),
                        DealId = r.Field<int?>("DealId"),
                        UserType = r.Field<string>("UserType"),
                        ApprovedStatus = r.Field<bool?>("ApprovedStatus"),
                        ApprovedDate = r.Field<DateTime?>("ApprovedDate"),
                        IsBillReSettlement = r.Field<bool?>("IsBillReSettlement"),
                        IsActive = r.Field<bool?>("IsActive"),
                        IsDeleted = r.Field<bool?>("IsDeleted"),
                        IsLocked = r.Field<bool?>("IsLocked"),
                        CreatedBy = r.Field<int?>("CreatedBy"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedDate"),
                        TransactionType = r.Field<string>("TransactionType"),
                        TransactionId = r.Field<long?>("TransactionId"),
                        IsBillPreviewButtonEnable = r.Field<bool?>("IsBillPreviewButtonEnable"),
                        ServiceChargeConfig = r.Field<decimal?>("ServiceChargeConfig"),
                        CitySDChargeConfig = r.Field<decimal?>("CitySDChargeConfig"),
                        VatAmountConfig = r.Field<decimal?>("VatAmountConfig"),
                        AdditionalChargeConfig = r.Field<decimal?>("AdditionalChargeConfig"),
                        CurrencyExchangeRate = r.Field<decimal?>("CurrencyExchangeRate"),
                        PaymentRemarks = r.Field<string>("PaymentRemarks"),
                        GuidId = r.Field<Guid>("GuidId"),
                        RegistrationGuidId = r.Field<Guid?>("RegistrationGuidId")

                    }).FirstOrDefault();

                }
            }


            return bill;
        }
        private List<RestaurantBillClassificationDiscount> GetRestaurantBillClassificationDiscountInfoByBillId(int billId)
        {
            List<RestaurantBillClassificationDiscount> billClassifications = new List<RestaurantBillClassificationDiscount>();
            string query = string.Format("SELECT * FROM RestaurantBillClassificationDiscount WHERE BillId = {0}", billId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BillInfo");
                    DataTable Table = ds.Tables["BillInfo"];

                    billClassifications = Table.AsEnumerable().Select(r => new RestaurantBillClassificationDiscount()
                    {
                        DiscountId = r.Field<int>("DiscountId"),
                        BillId = r.Field<int?>("BillId"),
                        ClassificationId = r.Field<int?>("ClassificationId"),
                        DiscountAmount = r.Field<decimal?>("DiscountAmount")

                    }).ToList();
                }
            }

            return billClassifications;
        }
        private List<RestaurantKotBillDetail> GetRestaurantKotBillDetailsSync(int kotId)
        {
            List<RestaurantKotBillDetail> kotDetailList = new List<RestaurantKotBillDetail>();
            string query = string.Format("SELECT * FROM RestaurantKotBillDetail WHERE KotId = {0}", kotId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "restaurantKotBillDetail");
                    DataTable Table = ds.Tables["restaurantKotBillDetail"];

                    kotDetailList = Table.AsEnumerable().Select(r => new RestaurantKotBillDetail
                    {
                        KotDetailId = r.Field<int>("KotDetailId"),
                        KotId = r.Field<int?>("KotId"),
                        ItemType = r.Field<string>("ItemType"),
                        ItemId = r.Field<int?>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        ItemUnit = r.Field<decimal?>("ItemUnit"),
                        ItemCost = r.Field<decimal?>("ItemCost"),
                        ItemTotalAmount = r.Field<decimal?>("ItemTotalAmount"),
                        DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                        DiscountedAmount = r.Field<decimal?>("DiscountedAmount"),
                        Amount = r.Field<decimal?>("Amount"),
                        UnitRate = r.Field<decimal?>("UnitRate"),
                        ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                        ServiceRate = r.Field<decimal?>("ServiceRate"),
                        VatAmount = r.Field<decimal?>("VatAmount"),
                        CitySDCharge = r.Field<decimal?>("CitySDCharge"),
                        AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                        PrintFlag = r.Field<bool?>("PrintFlag"),
                        IsChanged = r.Field<bool?>("IsChanged"),
                        IsDeleted = r.Field<bool?>("IsDeleted"),
                        IsDispatch = r.Field<bool?>("IsDispatch"),
                        EmpId = r.Field<int?>("EmpId"),
                        DeliveryStatus = r.Field<string>("DeliveryStatus"),
                        CreatedBy = r.Field<int?>("CreatedBy"),
                        LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedDate")

                    }).ToList();
                }
            }
            return kotDetailList;
        }
        private List<RestaurantKotSpecialRemarksDetailBO> GetRestaurantKotSpecialRemarksDetailByKotId(int kotId)
        {
            List<RestaurantKotSpecialRemarksDetailBO> entityRestaurantKotSpecialRemarks = new List<RestaurantKotSpecialRemarksDetailBO>();

            string query = string.Format("SELECT * FROM RestaurantKotSpecialRemarksDetail WHERE KotId = {0}", kotId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantKotSpecialRemarksDetailBO entityBO = new RestaurantKotSpecialRemarksDetailBO();

                                entityBO.RemarksDetailId = Convert.ToInt32(reader["RemarksDetailId"]);
                                entityBO.KotId = Convert.ToInt32(reader["KotId"].ToString());
                                entityBO.ItemId = Convert.ToInt32(reader["ItemId"].ToString());
                                entityBO.SpecialRemarksId = Convert.ToInt32(reader["SpecialRemarksId"]);
                                entityBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"].ToString());
                                if (reader["CreatedDate"] != DBNull.Value)
                                    entityBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                                if (reader["LastModifiedBy"] != DBNull.Value)
                                    entityBO.LastModifiedBy = Convert.ToInt32(reader["LastModifiedBy"].ToString());
                                if (reader["LastModifiedDate"] != DBNull.Value)
                                    entityBO.LastModifiedDate = Convert.ToDateTime(reader["[LastModifiedDate]"].ToString());

                                entityRestaurantKotSpecialRemarks.Add(entityBO);
                            }
                        }
                    }
                }
            }

            return entityRestaurantKotSpecialRemarks;
        }
        private List<HotelCompanyPaymentLedger> GetCompanyPaymentLedgerByRegistrationId(int registrationId)
        {
            List<HotelCompanyPaymentLedger> ledgers = new List<HotelCompanyPaymentLedger>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyPaymentLedgerByRegistrationIdAndModuleName_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "CompanyBillLedger");
                    DataTable Table = ds.Tables["CompanyBillLedger"];

                    ledgers = Table.AsEnumerable().Select(r => new HotelCompanyPaymentLedger()
                    {
                        CompanyPaymentId = r.Field<long>("CompanyPaymentId"),
                        ModuleName = r.Field<string>("ModuleName"),
                        PaymentType = r.Field<string>("PaymentType"),
                        PaymentId = r.Field<long?>("PaymentId"),
                        LedgerNumber = r.Field<string>("LedgerNumber"),
                        BillId = r.Field<int?>("BillId"),
                        BillNumber = r.Field<string>("BillNumber"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        CompanyId = r.Field<int>("CompanyId"),
                        CurrencyId = r.Field<int>("CurrencyId"),
                        ConvertionRate = r.Field<decimal?>("ConvertionRate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        CurrencyAmount = r.Field<decimal?>("CurrencyAmount"),
                        PaidAmount = r.Field<decimal?>("PaidAmount"),
                        PaidAmountCurrent = r.Field<decimal?>("PaidAmountCurrent"),
                        DueAmount = r.Field<decimal?>("DueAmount"),
                        AdvanceAmount = r.Field<decimal?>("AdvanceAmount"),
                        AdvanceAmountRemaining = r.Field<decimal?>("AdvanceAmountRemaining"),
                        DayConvertionRate = r.Field<decimal?>("DayConvertionRate"),
                        AccountsPostingHeadId = r.Field<long?>("AccountsPostingHeadId"),
                        GainOrLossAmount = r.Field<decimal?>("GainOrLossAmount"),
                        RoundedAmount = r.Field<decimal?>("RoundedAmount"),
                        ChequeNumber = r.Field<string>("ChequeNumber"),
                        Remarks = r.Field<string>("Remarks"),
                        PaymentStatus = r.Field<string>("PaymentStatus"),
                        BillGenerationId = r.Field<long?>("BillGenerationId"),
                        RefCompanyPaymentId = r.Field<long?>("RefCompanyPaymentId"),
                        CreatedBy = r.Field<int?>("CreatedBy"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedDate")

                    }).ToList();
                }
            }


            return ledgers;
        }
        private List<HotelGuestBillPaymentBO> GetGuestBillPaymentsByModuleNameAndServiceBillId(string moduleName, long serviceBillId)
        {
            List<HotelGuestBillPaymentBO> paymentList = new List<HotelGuestBillPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillPaymentsByModuleNameAndServiceBillId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ModuleName", DbType.String, moduleName);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, serviceBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "paymentList");
                    DataTable Table = ds.Tables["paymentList"];

                    paymentList = Table.AsEnumerable().Select(r => new HotelGuestBillPaymentBO
                    {

                        PaymentId = r.Field<int>("PaymentId"),
                        BillNumber = r.Field<string>("BillNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        PaymentType = r.Field<string>("PaymentType"),
                        ServiceBillId = r.Field<int?>("ServiceBillId"),
                        RegistrationId = r.Field<int?>("RegistrationId"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        PaymentDate = r.Field<DateTime?>("PaymentDate"),
                        TransactionDate = r.Field<DateTime?>("TransactionDate"),
                        PaymentMode = r.Field<string>("PaymentMode"),
                        PaymentModeId = r.Field<int?>("PaymentModeId"),
                        FieldId = r.Field<int?>("FieldId"),
                        CurrencyAmount = r.Field<decimal?>("CurrencyAmount"),
                        PaymentAmount = r.Field<decimal?>("PaymentAmount"),
                        PaymentDescription = r.Field<string>("PaymentDescription"),
                        BankId = r.Field<int?>("BankId"),
                        BranchName = r.Field<string>("BranchName"),
                        ChecqueNumber = r.Field<string>("ChecqueNumber"),
                        ChecqueDate = r.Field<DateTime?>("ChecqueDate"),
                        CardType = r.Field<string>("CardType"),
                        CardNumber = r.Field<string>("CardNumber"),
                        ExpireDate = r.Field<DateTime?>("ExpireDate"),
                        CardHolderName = r.Field<string>("CardHolderName"),
                        CardReference = r.Field<string>("CardReference"),
                        AccountsPostingHeadId = r.Field<int?>("AccountsPostingHeadId"),
                        RefundAccountHead = r.Field<int?>("RefundAccountHead"),
                        Remarks = r.Field<string>("Remarks"),
                        DealId = r.Field<int?>("DealId"),
                        ServiceRate = r.Field<decimal?>("ServiceRate"),
                        ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                        CitySDCharge = r.Field<decimal?>("CitySDCharge"),
                        VatAmount = r.Field<decimal?>("VatAmount"),
                        AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                        CurrencyExchangeRate = r.Field<decimal?>("CurrencyExchangeRate"),
                        CreatedBy = r.Field<int?>("CreatedBy"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedDate"),
                        IsAdvancePayment = r.Field<bool>("IsAdvancePayment"),
                        ReservationId = r.Field<long>("ReservationId"),
                        RegistrationGuiId = r.Field<Guid?>("GuidId")

                    }).ToList();
                }
            }
            return paymentList;
        }
        private List<HotelCompanyPaymentLedger> GetCompanyPaymentLedgerByModuleNameAndBillId(int billId, string moduleName)
        {
            List<HotelCompanyPaymentLedger> ledger = new List<HotelCompanyPaymentLedger>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyPaymentLedgerByModuleNameAndBillId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ModuleName", DbType.String, moduleName);
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "CompanyBillLedger");
                    DataTable Table = ds.Tables["CompanyBillLedger"];

                    ledger = Table.AsEnumerable().Select(r => new HotelCompanyPaymentLedger()
                    {
                        CompanyPaymentId = r.Field<long>("CompanyPaymentId"),
                        ModuleName = r.Field<string>("ModuleName"),
                        PaymentType = r.Field<string>("PaymentType"),
                        PaymentId = r.Field<long?>("PaymentId"),
                        LedgerNumber = r.Field<string>("LedgerNumber"),
                        BillId = r.Field<int?>("BillId"),
                        BillNumber = r.Field<string>("BillNumber"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        CompanyId = r.Field<int>("CompanyId"),
                        CurrencyId = r.Field<int>("CurrencyId"),
                        ConvertionRate = r.Field<decimal?>("ConvertionRate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        CurrencyAmount = r.Field<decimal?>("CurrencyAmount"),
                        PaidAmount = r.Field<decimal?>("PaidAmount"),
                        PaidAmountCurrent = r.Field<decimal?>("PaidAmountCurrent"),
                        DueAmount = r.Field<decimal?>("DueAmount"),
                        AdvanceAmount = r.Field<decimal?>("AdvanceAmount"),
                        AdvanceAmountRemaining = r.Field<decimal?>("AdvanceAmountRemaining"),
                        DayConvertionRate = r.Field<decimal?>("DayConvertionRate"),
                        AccountsPostingHeadId = r.Field<long?>("AccountsPostingHeadId"),
                        GainOrLossAmount = r.Field<decimal?>("GainOrLossAmount"),
                        RoundedAmount = r.Field<decimal?>("RoundedAmount"),
                        ChequeNumber = r.Field<string>("ChequeNumber"),
                        Remarks = r.Field<string>("Remarks"),
                        PaymentStatus = r.Field<string>("PaymentStatus"),
                        BillGenerationId = r.Field<long?>("BillGenerationId"),
                        RefCompanyPaymentId = r.Field<long?>("RefCompanyPaymentId"),
                        CreatedBy = r.Field<int?>("CreatedBy"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedDate")
                    }).ToList();
                }
            }

            return ledger;
        }

        private ServiceBillDataSyncViewBO FindSyncedServiceBillByBillId(int billId)
        {
            ServiceBillDataSyncViewBO syncBO = null;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSyncedServiceBillByRegistrationId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                syncBO = new ServiceBillDataSyncViewBO();
                                syncBO.BillId = Convert.ToInt32(reader["ServiceBillId"]);
                                syncBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                syncBO.GuidId = (Guid)(reader["GuidId"]);
                                syncBO.IsSyncCompleted = Convert.ToBoolean(reader["IsSyncCompleted"]);
                            }
                        }
                    }
                }
            }
            return syncBO;
        }
        private RestaurantDataSyncViewBO FindSyncedRestaurantBillByBillId(int billId)
        {
            RestaurantDataSyncViewBO syncBO = null;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSyncedRestaurantBillByRegistrationId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                syncBO = new RestaurantDataSyncViewBO();
                                syncBO.BillId = Convert.ToInt32(reader["BillId"]);
                                syncBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                syncBO.CostCenter = reader["CostCenter"].ToString();
                                syncBO.GuidId = (Guid)(reader["GuidId"]);
                                syncBO.IsSyncCompleted = Convert.ToBoolean(reader["IsSyncCompleted"]);

                            }
                        }
                    }
                }
            }
            return syncBO;
        }
        private BanquetBillDataSyncViewBO FindSyncedBanquetBillByBillId(long reservationId)
        {
            BanquetBillDataSyncViewBO syncBO = null;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSyncedBanquetBillByRegistrationId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                syncBO = new BanquetBillDataSyncViewBO();
                                syncBO.BillId = Convert.ToInt32(reader["Id"]);
                                syncBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                syncBO.CostCenter = reader["CostCenter"].ToString();
                                syncBO.GuidId = (Guid)(reader["GuidId"]);
                                syncBO.IsSyncCompleted = Convert.ToBoolean(reader["IsSyncCompleted"]);

                            }
                        }
                    }
                }
            }
            return syncBO;
        }
        private BanquetReservation GetBanquetReservationInfoById(long reservationId)
        {
            BanquetReservation banquet;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelGuestServiceBillByIdForSync_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, reservationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BanquetReservation");
                    DataTable Table = ds.Tables["BanquetReservation"];

                    banquet = Table.AsEnumerable().Select(r => new BanquetReservation()
                    {
                        Id = r.Field<long>("Id"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        ReservationMode = r.Field<string>("ReservationMode"),
                        BanquetId = r.Field<long?>("BanquetId"),
                        CostCenterId = r.Field<int?>("CostCenterId"),
                        IsListedCompany = r.Field<bool?>("IsListedCompany"),
                        CompanyId = r.Field<int?>("CompanyId"),
                        Name = r.Field<string>("Name"),
                        Address = r.Field<string>("Address"),
                        CityName = r.Field<string>("CityName"),
                        ZipCode = r.Field<string>("ZipCode"),
                        CountryId = r.Field<int?>("CountryId"),
                        PhoneNumber = r.Field<string>("PhoneNumber"),
                        EmailAddress = r.Field<string>("EmailAddress"),
                        BookingFor = r.Field<string>("BookingFor"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        ContactEmail = r.Field<string>("ContactEmail"),
                        ContactPhone = r.Field<string>("ContactPhone"),
                        ArriveDate = r.Field<DateTime?>("ArriveDate"),
                        DepartureDate = r.Field<DateTime?>("DepartureDate"),
                        OccessionTypeId = r.Field<long?>("OccessionTypeId"),
                        SeatingId = r.Field<long?>("SeatingId"),
                        BanquetRate = r.Field<decimal?>("BanquetRate"),
                        BanquetDiscount = r.Field<decimal?>("BanquetDiscount"),
                        BanquetDiscountedAmount = r.Field<decimal?>("BanquetDiscountedAmount"),
                        BanquetRackRate = r.Field<decimal?>("BanquetRackRate"),
                        BanquetServiceCharge = r.Field<decimal?>("BanquetServiceCharge"),
                        BanquetCitySDCharge = r.Field<decimal?>("BanquetCitySDCharge"),
                        BanquetVatAmount = r.Field<decimal?>("BanquetVatAmount"),
                        BanquetAdditionalCharge = r.Field<decimal?>("BanquetAdditionalCharge"),
                        InvoiceServiceRate = r.Field<decimal?>("InvoiceServiceRate"),
                        IsInvoiceServiceChargeEnable = r.Field<bool?>("IsInvoiceServiceChargeEnable"),
                        InvoiceServiceCharge = r.Field<decimal?>("InvoiceServiceCharge"),
                        IsInvoiceCitySDChargeEnable = r.Field<bool?>("IsInvoiceCitySDChargeEnable"),
                        InvoiceCitySDCharge = r.Field<decimal?>("InvoiceCitySDCharge"),
                        IsInvoiceVatAmountEnable = r.Field<bool?>("IsInvoiceVatAmountEnable"),
                        InvoiceVatAmount = r.Field<decimal?>("InvoiceVatAmount"),
                        IsInvoiceAdditionalChargeEnable = r.Field<bool?>("IsInvoiceAdditionalChargeEnable"),
                        AdditionalChargeType = r.Field<string>("AdditionalChargeType"),
                        InvoiceAdditionalCharge = r.Field<decimal?>("InvoiceAdditionalCharge"),
                        NumberOfPersonAdult = r.Field<int?>("NumberOfPersonAdult"),
                        NumberOfPersonChild = r.Field<int?>("NumberOfPersonChild"),
                        CancellationReason = r.Field<string>("CancellationReason"),
                        SpecialInstructions = r.Field<string>("SpecialInstructions"),
                        RefferenceId = r.Field<long?>("RefferenceId"),
                        IsReturnedClient = r.Field<bool?>("IsReturnedClient"),
                        Comments = r.Field<string>("Comments"),
                        TotalAmount = r.Field<decimal?>("TotalAmount"),
                        DiscountType = r.Field<string>("DiscountType"),
                        DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                        CalculatedDiscountAmount = r.Field<decimal?>("CalculatedDiscountAmount"),
                        DiscountedAmount = r.Field<decimal?>("DiscountedAmount"),
                        ServiceRate = r.Field<decimal?>("ServiceRate"),
                        ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                        CitySDCharge = r.Field<decimal?>("CitySDCharge"),
                        VatAmount = r.Field<decimal?>("VatAmount"),
                        AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                        GrandTotal = r.Field<decimal?>("GrandTotal"),
                        RoundedAmount = r.Field<decimal?>("RoundedAmount"),
                        RoundedGrandTotal = r.Field<decimal?>("RoundedGrandTotal"),
                        RebateRemarks = r.Field<string>("RebateRemarks"),
                        IsBillSettlement = r.Field<bool?>("IsBillSettlement"),
                        RegistrationId = r.Field<int?>("RegistrationId"),
                        ActiveStatus = r.Field<int?>("ActiveStatus"),
                        BillStatus = r.Field<string>("BillStatus"),
                        BillVoidBy = r.Field<int?>("BillVoidBy"),
                        CurrencyExchangeRate = r.Field<decimal?>("CurrencyExchangeRate"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedBy = r.Field<long?>("CreatedBy"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        LastModifiedBy = r.Field<long?>("LastModifiedBy"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedDate"),
                        SettlementDate = r.Field<DateTime?>("SettlementDate"),
                        MarketSegmentId = r.Field<int?>("MarketSegmentId"),
                        GuestSourceId = r.Field<int?>("GuestSourceId"),
                        BookersName = r.Field<string>("BookersName"),
                        GuidId = r.Field<Guid>("GuidId"),
                        RegistrationGuidId = r.Field<Guid?>("RegistrationGuidId"),
                        ReservationDiscountType = r.Field<string>("ReservationDiscountType"),
                        ReservationDiscountAmount = r.Field<decimal>("ReservationDiscountAmount"),

                    }).FirstOrDefault();
                }
            }
            return banquet;
        }
        private List<BanquetReservationDetailBO> GetBanquetReservationDetailInfoByReservationId(long ReservationId)
        {
            List<BanquetReservationDetailBO> detailList = new List<BanquetReservationDetailBO>();

            string query = string.Format("SELECT * FROM BanquetReservationDetail WHERE ReservationId = {0}", ReservationId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetReservationDetailBO detail = new BanquetReservationDetailBO();

                                detail.Id = Int64.Parse(reader["Id"].ToString());
                                detail.ReservationId = Int64.Parse(reader["ReservationId"].ToString());
                                detail.ItemTypeId = Convert.ToInt64(reader["ItemTypeId"].ToString());
                                detail.ItemType = reader["ItemType"].ToString();
                                detail.ItemId = Int64.Parse(reader["ItemId"].ToString());
                                detail.ItemName = reader["ItemName"].ToString();

                                detail.IsComplementary = Convert.ToBoolean(reader["IsComplementary"]);
                                if (reader["ItemUnit"] != DBNull.Value)
                                    detail.ItemUnit = Convert.ToDecimal(reader["ItemUnit"].ToString());
                                if (reader["UnitPrice"] != DBNull.Value)
                                    detail.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());
                                if (reader["TotalPrice"] != DBNull.Value)
                                    detail.TotalPrice = Convert.ToDecimal(reader["TotalPrice"].ToString());
                                detail.DiscountType = reader["DiscountType"] != DBNull.Value ? reader["DiscountType"].ToString() : string.Empty;

                                if (reader["DiscountAmount"] != DBNull.Value)
                                    detail.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                if (reader["DiscountedAmount"] != DBNull.Value)
                                    detail.DiscountedAmount = Convert.ToDecimal(reader["DiscountedAmount"]);
                                if (reader["ServiceCharge"] != DBNull.Value)
                                    detail.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                if (reader["ServiceRate"] != DBNull.Value)
                                    detail.ServiceRate = Convert.ToDecimal(reader["ServiceRate"]);
                                if (reader["CitySDCharge"] != DBNull.Value)
                                    detail.CitySDCharge = Convert.ToDecimal(reader["CitySDCharge"]);
                                if (reader["VatAmount"] != DBNull.Value)
                                    detail.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                if (reader["AdditionalCharge"] != DBNull.Value)
                                    detail.AdditionalCharge = Convert.ToDecimal(reader["AdditionalCharge"]);

                                detailList.Add(detail);
                            }
                        }
                    }
                }
            }
            return detailList;
        }

        public SetUpData GetAllSetupData()
        {
            CommonDA commonDA = new CommonDA();
            SetUpData setUpData = new SetUpData();

            setUpData.CommonCostCenters = commonDA.GetAllTableRow<CommonCostCenter>("CommonCostCenter");
            setUpData.HotelGuestCompanys = commonDA.GetAllTableRow<HotelGuestCompany>("HotelGuestCompany");
            setUpData.InvUnitHeads = commonDA.GetAllTableRow<InvUnitHead>("InvUnitHead");
            setUpData.InvUnitConversions = commonDA.GetAllTableRow<InvUnitConversion>("InvUnitConversion");
            setUpData.InvLocations = commonDA.GetAllTableRow<InvLocation>("InvLocation");
            setUpData.InvLocationCostCenterMappings = commonDA.GetAllTableRow<InvLocationCostCenterMapping>("InvLocationCostCenterMapping");
            setUpData.InvCategorys = commonDA.GetAllTableRow<InvCategory>("InvCategory");
            setUpData.InvCategoryCostCenterMappings = commonDA.GetAllTableRow<InvCategoryCostCenterMapping>("InvCategoryCostCenterMapping");
            setUpData.InvInventoryAccountVsItemCategoryMapppings = commonDA.GetAllTableRow<InvInventoryAccountVsItemCategoryMappping>("InvInventoryAccountVsItemCategoryMappping");
            setUpData.InvCogsAccountVsItemCategoryMapppings = commonDA.GetAllTableRow<InvCogsAccountVsItemCategoryMappping>("InvCogsAccountVsItemCategoryMappping");
            setUpData.InvItems = commonDA.GetAllTableRow<InvItem>("InvItem");
            setUpData.InvItemCostCenterMappings = commonDA.GetAllTableRow<InvItemCostCenterMapping>("InvItemCostCenterMapping");
            setUpData.InvItemClassifications = commonDA.GetAllTableRow<InvItemClassification>("InvItemClassification");
            setUpData.InvItemClassificationCostCenterMappings = commonDA.GetAllTableRow<InvItemClassificationCostCenterMapping>("InvItemClassificationCostCenterMapping");
            setUpData.RestaurantRecipeDetails = commonDA.GetAllTableRow<RestaurantRecipeDetail>("RestaurantRecipeDetail");
            setUpData.HotelRoomTypes = commonDA.GetAllTableRow<HotelRoomType>("HotelRoomType");
            return setUpData;
        }
    }
}
