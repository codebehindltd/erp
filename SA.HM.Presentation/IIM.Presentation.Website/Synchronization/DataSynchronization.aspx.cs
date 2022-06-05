using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Synchronization;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Synchronization;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.Synchronization
{

    public partial class DataSynchronization : System.Web.UI.Page
    {
        static HttpClient client = new HttpClient();
        HMUtility hMUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadApiUrl();
                LoadLastSyncDateTime();
                CostCenterWiseSetting();
            }

        }

        private void LoadApiUrl()
        {
            hfSyncApiUrl.Value = System.Web.Configuration.WebConfigurationManager.AppSettings["SyncApiUrl"].ToString();
        }
        private void CostCenterWiseSetting()
        {
            List<CostCentreTabBO> costCentreTabBO = new List<CostCentreTabBO>();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoByType("FrontOffice");
            if (costCentreTabBO != null)
            {
                if (costCentreTabBO.Count > 0)
                {
                    hfInclusiveHotelManagementBill.Value = costCentreTabBO[0].IsVatSChargeInclusive.ToString();
                    //hfIsDiscountApplicableOnRackRate.Value = costCentreTabBO[0].IsDiscountApplicableOnRackRate ? "1" : "0";
                    hfAdditionalCharge.Value = costCentreTabBO[0].AdditionalCharge.ToString();
                    hfAdditionalChargeType.Value = costCentreTabBO[0].AdditionalChargeType.ToString();

                    hfIsVatEnableOnGuestHouseCityCharge.Value = costCentreTabBO[0].IsVatOnSDCharge ? "1" : "0";

                    hfCityCharge.Value = costCentreTabBO[0].CitySDCharge.ToString();
                    hfGuestHouseVat.Value = costCentreTabBO[0].VatAmount.ToString();
                    hfGuestHouseServiceCharge.Value = costCentreTabBO[0].ServiceCharge.ToString();
                    hfIsRatePlusPlus.Value = costCentreTabBO[0].IsRatePlusPlus.ToString();
                }
            }
        }
        [WebMethod]
        public static string LoadLastSyncDateTime()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO setUpBO = commonSetupDA.GetCommonConfigurationInfo("LastSyncDateTime", "LastSyncDateTime");
            DateTime dateTime = Convert.ToDateTime(setUpBO.SetupValue);
            return dateTime.ToString();

        }
        [WebMethod]
        public static List<RegistrationDataSyncViewBO> GetRegistrationListToSync(DateTime toDate)
        {
            DateTime searchDateTime = new DateTime();
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            TimeSpan maxTime = DateTime.MaxValue.TimeOfDay;

            if (toDate.Date < DateTime.Now.Date)
                searchDateTime = toDate.Add(maxTime);
            else
                searchDateTime = toDate.Add(currentTime);
            List<RegistrationDataSyncViewBO> registrationList = new List<RegistrationDataSyncViewBO>();
            DataSyncDA dataAccess = new DataSyncDA();
            registrationList = dataAccess.GetRegistrationDetailsForSync(searchDateTime);

            return registrationList;
        }
        [WebMethod]
        public static List<RestaurantDataSyncViewBO> GetRestaurantListToSync(List<int> registrationIdList, DateTime toDate)
        {
            DateTime searchDateTime = new DateTime();
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            TimeSpan maxTime = DateTime.MaxValue.TimeOfDay;

            if (toDate.Date < DateTime.Now.Date)
                searchDateTime = toDate.Add(maxTime);
            else
                searchDateTime = toDate.Add(currentTime);
            List<RestaurantDataSyncViewBO> restaurantList = new List<RestaurantDataSyncViewBO>();
            DataSyncDA dataAccess = new DataSyncDA();
            string regIdList = string.Join(",", registrationIdList.Select(l => l.ToString()).ToArray());
            restaurantList = dataAccess.GetRestaurantBillsForSync(regIdList, searchDateTime);

            return restaurantList;
        }
        [WebMethod]
        public static List<ServiceBillDataSyncViewBO> GetServiceBillListToSync(List<int> registrationIdList, DateTime toDate)
        {
            DateTime searchDateTime = new DateTime();
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            TimeSpan maxTime = DateTime.MaxValue.TimeOfDay;

            if (toDate.Date < DateTime.Now.Date)
                searchDateTime = toDate.Add(maxTime);
            else
                searchDateTime = toDate.Add(currentTime);

            List<ServiceBillDataSyncViewBO> serviceBillList = new List<ServiceBillDataSyncViewBO>();
            DataSyncDA dataAccess = new DataSyncDA();
            string regIdList = string.Join(",", registrationIdList.Select(l => l.ToString()).ToArray());

            serviceBillList = dataAccess.GetServiceBillsForSync(regIdList, searchDateTime);

            return serviceBillList;
        }
        [WebMethod]
        public static List<BanquetBillDataSyncViewBO> GetBanquetBillListToSync(List<int> registrationIdList, DateTime toDate)
        {
            DateTime searchDateTime = new DateTime();
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            TimeSpan maxTime = DateTime.MaxValue.TimeOfDay;

            if (toDate.Date < DateTime.Now.Date)
                searchDateTime = toDate.Add(maxTime);
            else
                searchDateTime = toDate.Add(currentTime);

            List<BanquetBillDataSyncViewBO> banquetBillList = new List<BanquetBillDataSyncViewBO>();
            DataSyncDA dataAccess = new DataSyncDA();
            string regIdList = string.Join(",", registrationIdList.Select(l => l.ToString()).ToArray());
            banquetBillList = dataAccess.GetBanquetBillsForSync(regIdList, searchDateTime);

            return banquetBillList;
        }

        [WebMethod]
        public static ReturnInfo SaveRegistrationSyncInformation(Guid id, bool IsSyncCompleted)
        {
            DataSyncDA dataAccess = new DataSyncDA();
            bool status = false;
            ReturnInfo returnInfo = new ReturnInfo();

            status = dataAccess.SaveOrUpdateRegistrationSyncInformation(id, IsSyncCompleted);

            if (status)
            {
                returnInfo.IsSuccess = true;
                returnInfo.Data = new { registrationId = id };
            }
            else
            {
                returnInfo.IsSuccess = false;
            }
            return returnInfo;
        }
        [WebMethod]
        public static ReturnInfo SaveServiceBillSyncInformation(Guid id, bool IsSyncCompleted)
        {
            DataSyncDA dataAccess = new DataSyncDA();
            bool status = false;
            ReturnInfo returnInfo = new ReturnInfo();

            status = dataAccess.SaveOrUpdateServiceBillSyncInformation(id, IsSyncCompleted);

            if (status)
            {
                returnInfo.IsSuccess = true;
                returnInfo.Data = new { registrationId = id };
            }
            else
            {
                returnInfo.IsSuccess = false;
            }
            return returnInfo;
        }
        [WebMethod]
        public static ReturnInfo SaveRestaurantSyncInformation(Guid id, bool IsSyncCompleted)
        {
            DataSyncDA dataAccess = new DataSyncDA();
            bool status = false;
            ReturnInfo returnInfo = new ReturnInfo();

            status = dataAccess.SaveOrUpdateRestaurantSyncInformation(id, IsSyncCompleted);

            if (status)
            {
                returnInfo.IsSuccess = true;
                returnInfo.Data = new { registrationId = id };
            }
            else
            {
                returnInfo.IsSuccess = false;
            }
            return returnInfo;
        }
        [WebMethod]
        public static ReturnInfo SaveBanquetSyncInformation(Guid id, bool IsSyncCompleted)
        {
            DataSyncDA dataAccess = new DataSyncDA();
            bool status = false;
            ReturnInfo returnInfo = new ReturnInfo();

            status = dataAccess.SaveOrUpdateBanquetSyncInformation(id, IsSyncCompleted);

            if (status)
            {
                returnInfo.IsSuccess = true;
                returnInfo.Data = new { registrationId = id };
            }
            else
            {
                returnInfo.IsSuccess = false;
            }
            return returnInfo;
        }


        [WebMethod]
        public static RegistrationDataSyncBO GetRegistrationRelatedDataToSync(int registrationId, DateTime toDate)
        {
            DateTime searchDateTime = new DateTime();
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            TimeSpan maxTime = DateTime.MaxValue.TimeOfDay;

            if (toDate.Date < DateTime.Now.Date)
                searchDateTime = toDate.Add(maxTime);
            else
                searchDateTime = toDate.Add(currentTime);

            RegistrationDataSyncBO registrationSyncData = new RegistrationDataSyncBO();
            DataSyncDA dataAccess = new DataSyncDA();
            registrationSyncData = dataAccess.GetRegistrationRelatedDataToSync(registrationId, searchDateTime);
            registrationSyncData = CheckIsNull(registrationSyncData);

            return registrationSyncData;
        }

        private static RegistrationDataSyncBO CheckIsNull(RegistrationDataSyncBO dataSyncBO)
        {
            if (dataSyncBO.RoomRegistration != null || dataSyncBO.GuestBillPayments.Count > 0
                    || dataSyncBO.CompanyPayments.Count > 0 || dataSyncBO.ApprovedHotelGuestBills.Count > 0
                    || dataSyncBO.GuestServiceBills.Count > 0 || dataSyncBO.AirportPickupDrops.Count > 0
                    || dataSyncBO.BillPaidForGuidId.Count > 0
                    || dataSyncBO.GuestHouseCheckOut != null || dataSyncBO.HotelGuestDayLateCheckOut != null)
                return dataSyncBO;
            else
                return null;
        }
        [WebMethod]
        public static ServiceBillDataSyncBO GetServiceBillRelatedDataToSync(int serviceBillId)
        {
            ServiceBillDataSyncBO serviceSyncData = new ServiceBillDataSyncBO();
            DataSyncDA dataAccess = new DataSyncDA();
            serviceSyncData = dataAccess.GetServiceBillRelatedDataToSync(serviceBillId);

            return serviceSyncData;
        }
        [WebMethod]
        public static RestaurantDataSyncBO GetRestaurantRelatedDataToSync(int billId)
        {
            RestaurantDataSyncBO restaurantSyncData = new RestaurantDataSyncBO();
            DataSyncDA dataAccess = new DataSyncDA();
            restaurantSyncData = dataAccess.GetRestaurantRelatedDataToSync(billId);

            return restaurantSyncData;
        }
        [WebMethod]
        public static BanquetDataSyncBO GetBanquetRelatedDataToSync(int id)
        {
            BanquetDataSyncBO banquetSyncData = new BanquetDataSyncBO();
            DataSyncDA dataAccess = new DataSyncDA();
            banquetSyncData = dataAccess.GetBanquetRelatedDataToSync(id);

            return banquetSyncData;
        }

        [WebMethod]
        public static ReturnInfo UpdateSyncDateTime(DateTime toDate)
        {
            DateTime searchDateTime = new DateTime();
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            TimeSpan maxTime = DateTime.MaxValue.TimeOfDay;

            if (toDate.Date < DateTime.Now.Date)
                searchDateTime = toDate.Add(maxTime);
            else
                searchDateTime = toDate.Add(currentTime);

            int tmpFloorId = 0;
            ReturnInfo returnInfo = new ReturnInfo();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO setUpBO = commonSetupDA.GetCommonConfigurationInfo("LastSyncDateTime", "LastSyncDateTime");

            setUpBO.SetupValue = searchDateTime.ToString();
            returnInfo.IsSuccess = commonSetupDA.SaveOrUpdateCommonConfiguration(setUpBO, out tmpFloorId);
            if (returnInfo.IsSuccess)
                DeleteAllTemporarySyncData();
            return returnInfo;
        }

        public static bool DeleteAllTemporarySyncData()
        {
            bool status = false;
            DataSyncDA dataSyncDA = new DataSyncDA();
            try
            {
                status = dataSyncDA.DeleteAllTemporarySyncData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        [WebMethod]
        public static ReturnInfo SaveSyncDataForSyncLater(List<TemporarySync> billList)
        {
            HMUtility hMUtility = new HMUtility();
            ReturnInfo returnInfo = new ReturnInfo();
            DataSyncDA dataSyncDA = new DataSyncDA();
            try
            {
                returnInfo.IsSuccess = DeleteAllTemporarySyncData();
                if (returnInfo.IsSuccess)
                {
                    returnInfo.IsSuccess = dataSyncDA.SaveSyncDataForSyncLater(billList);

                    if (returnInfo.IsSuccess)
                    {
                        // Boolean logStatus = hMUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                        // EntityTypeEnum.EntityType.SMStage.ToString(), id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMStage));
                        returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                    else
                    {
                        returnInfo.IsSuccess = false;
                        returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }
                else
                {
                    returnInfo.IsSuccess = false;
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                returnInfo.IsSuccess = false;
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return returnInfo;
        }
        [WebMethod]
        public static List<TemporarySync> GetTemporarySyncData()
        {
            List<TemporarySync> billList = new List<TemporarySync>();
            DataSyncDA dataSyncDA = new DataSyncDA();

            billList = dataSyncDA.GetTemporarySyncData();

            return billList;
        }
        [WebMethod]
        public static bool ProcessRoomRate()
        {
            ReturnInfo returnInfo = new ReturnInfo();
            UserInformationBO userInformationBO = new UserInformationBO();

            HMUtility hMUtility = new HMUtility();
            userInformationBO = hMUtility.GetCurrentApplicationUserInfo();

            string registrationIdList = string.Empty;
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomRegistrationBO> ActiveRoomRegistrationInfoBO = new List<RoomRegistrationBO>();
            ActiveRoomRegistrationInfoBO = roomRegistrationDA.GetActiveRoomRegistrationInfo();
            foreach (RoomRegistrationBO row in ActiveRoomRegistrationInfoBO)
            {
                if (string.IsNullOrWhiteSpace(registrationIdList))
                {
                    registrationIdList = row.RegistrationId.ToString();
                }
                else if (registrationIdList == "0")
                {
                    registrationIdList = row.RegistrationId.ToString();
                }
                else
                {
                    registrationIdList += "," + row.RegistrationId.ToString();
                }
            }
            DateTime toDate = DateTime.Now.AddDays(1).AddSeconds(-1);
            RoomRegistrationDA roomregistrationDA = new RoomRegistrationDA();
            //roomregistrationDA.RoomNightAuditProcess(registrationIdList, DateTime.Now, 0, userInformationBO.UserInfoId);
            return roomregistrationDA.RoomNightAuditProcess(registrationIdList, toDate, 0, userInformationBO.UserInfoId);
        }

        [WebMethod(EnableSession = true)]
        public static void LoadRegistrationIdToSession(int registrationId)
        {
            HttpContext.Current.Session["CheckOutRegistrationIdList"] = registrationId.ToString();
        }

        [WebMethod]
        public static SetUpData GetSetupData()
        {
            SetUpData setUpData = new SetUpData();
            DataSyncDA dataSyncDA = new DataSyncDA();
            setUpData = dataSyncDA.GetAllSetupData();
            return setUpData;
        }

    }
}