using AccountingDomain.Common;
using InnboardService.Services;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using InnboardDomain.ViewModel;
using AutoMapper;

namespace InnboardAPI.Controllers
{
    [RoutePrefix("api/Sync")]
    [Authorize]
    public class SyncController : ApiController
    {
        private HotelRoomRegistrationService registrationService;
        private HotelGuestInformationService guestInformationService;
        private HotelGuestRegistrationService guestRegistrationService;
        private HotelGuestBillPaymentService guestBillPaymentService;
        private HotelGuestBillApprovedService guestBillApprovedService;
        private HotelGuestHouseCheckOutService guestHouseCheckOutService;
        private HotelGuestDayLetCheckOutService guestDayLateCheckOutService;
        private HotelGuestServiceBillService guestServiceBillService;
        private HotelRegistrationAireportPickupDropService pickupDropService;
        private HotelGuestExtraServiceBillApprovedService extraServiceBillApprovedService;
        private RestaurantBillService billService;
        private RestaurantBillDetailService BillDetailService;
        private RestaurantBillClassificationDiscountService classificationDiscountService;
        private RestaurantKotBillMasterService kotBillMasterService;
        private RestaurantKotBillDetailService kotBillDetailService;
        private RestaurantKotSpecialRemarksDetailService kotSpecialRemarksDetailService;
        private BanquetReservationService reservationService;
        private HotelRoomNumberService roomNumberService;
        private HotelCompanyPaymentLedgerService companyPaymentLedgerService;
        CommonCostCenterService costCenterService;
        InvUnitHeadService unitHeadService;
        InvUnitConversionService unitConversionService;
        InvLocationService invLocationService;
        InvLocationCostCenterMappingService invLocationCostCenterMappingService;
        InvCategoryService invCategoryService;
        InvCategoryCostCenterMappingService invCategoryCostCenterMappingService;
        InvInventoryAccountVsItemCategoryMapppingService invInventoryAccountVsItemCategoryMapppingService;
        InvCogsAccountVsItemCategoryMapppingService invCogsAccountVsItemCategoryMapppingService;
        InvItemService invItemService = new InvItemService();
        InvItemCostCenterMappingService invItemCostCenterMappingService;
        RestaurantRecipeDetailService restaurantRecipeDetailService;
        InvItemClassificationService invItemClassificationService;
        InvItemClassificationCostCenterMappingService invItemClassificationCostCenterMappingService;
        HotelRoomTypeService hotelRoomTypeService;
        HotelGuestCompanyService hotelGuestCompanyService;


        public SyncController()
        {
            registrationService = new HotelRoomRegistrationService();
            guestInformationService = new HotelGuestInformationService();
            guestRegistrationService = new HotelGuestRegistrationService();
            guestBillPaymentService = new HotelGuestBillPaymentService();
            guestBillApprovedService = new HotelGuestBillApprovedService();
            guestHouseCheckOutService = new HotelGuestHouseCheckOutService();
            guestDayLateCheckOutService = new HotelGuestDayLetCheckOutService();
            guestServiceBillService = new HotelGuestServiceBillService();
            extraServiceBillApprovedService = new HotelGuestExtraServiceBillApprovedService();
            pickupDropService = new HotelRegistrationAireportPickupDropService();
            billService = new RestaurantBillService();
            BillDetailService = new RestaurantBillDetailService();
            classificationDiscountService = new RestaurantBillClassificationDiscountService();
            kotBillMasterService = new RestaurantKotBillMasterService();
            kotBillDetailService = new RestaurantKotBillDetailService();
            kotSpecialRemarksDetailService = new RestaurantKotSpecialRemarksDetailService();
            reservationService = new BanquetReservationService();
            roomNumberService = new HotelRoomNumberService();
            companyPaymentLedgerService = new HotelCompanyPaymentLedgerService();
            costCenterService = new CommonCostCenterService();
            unitHeadService = new InvUnitHeadService();
            unitConversionService = new InvUnitConversionService();
            invLocationService = new InvLocationService();
            invLocationCostCenterMappingService = new InvLocationCostCenterMappingService();
            invCategoryService = new InvCategoryService();
            invCategoryCostCenterMappingService = new InvCategoryCostCenterMappingService();
            invInventoryAccountVsItemCategoryMapppingService = new InvInventoryAccountVsItemCategoryMapppingService();
            invCogsAccountVsItemCategoryMapppingService = new InvCogsAccountVsItemCategoryMapppingService();
            invItemService = new InvItemService();
            invItemCostCenterMappingService = new InvItemCostCenterMappingService();
            restaurantRecipeDetailService = new RestaurantRecipeDetailService();
            invItemClassificationService = new InvItemClassificationService();
            invItemClassificationCostCenterMappingService = new InvItemClassificationCostCenterMappingService();
            hotelRoomTypeService = new HotelRoomTypeService();
            hotelGuestCompanyService = new HotelGuestCompanyService();

        }

        public IHttpActionResult Post(RegistrationDataSync registration)
        {
            if (ModelState.IsValid)
            {
                GenericService<RegistrationDataSync> converisonService = new GenericService<RegistrationDataSync>();

                var response = converisonService.ConvertDateTimeUTCtoLocalTime(registration);
                if (response.Success)
                    response = registrationService.Sync(registration);

                return Json(new { response.Success, registration.RoomRegistration.GuidId, response.Data.IsSyncCompleted });
            }
            else
                return Json(new { Success = false, ErrorMessage = "Model is not in valid format" });
        }

        [Route("SyncRestaurant")]
        public IHttpActionResult PostRestaurant(RestaurantDataSync restaurant)
        {
            if (ModelState.IsValid)
            {
                GenericService<RestaurantDataSync> converisonService = new GenericService<RestaurantDataSync>();

                if (restaurant.Bill != null)
                {
                    var response = converisonService.ConvertDateTimeUTCtoLocalTime(restaurant);
                    if (response.Success)
                        billService.Sync(restaurant);
                    return Json(new { response.Success, restaurant.Bill.GuidId, restaurant.IsSyncCompleted });
                }
                else
                    return Json(new { Success = false });
            }
            else
                return Json(new { Success = false, ErrorMessage = "Model is not in valid format" });
        }

        [Route("SyncServiceBill")]
        public IHttpActionResult PostServiceBill(ServiceBillDataSync serviceBill)
        {
            if (ModelState.IsValid)
            {
                GenericService<ServiceBillDataSync> converisonService = new GenericService<ServiceBillDataSync>();

                if (serviceBill.ServiceBill != null)
                {
                    var response = converisonService.ConvertDateTimeUTCtoLocalTime(serviceBill);

                    if (response.Success)
                        guestServiceBillService.Sync(serviceBill);
                    return Json(new { response.Success, serviceBill.ServiceBill.GuidId, serviceBill.IsSyncCompleted });
                }
                else
                    return Json(new { Success = false });
            }
            else
                return Json(new { Success = false, ErrorMessage = "Model is not in valid format" });
        }

        [Route("SyncBanquetBill")]
        public IHttpActionResult PostBanquetBill(BanquetBillDataSync banquet)
        {
            if (ModelState.IsValid)
            {
                GenericService<BanquetBillDataSync> converisonService = new GenericService<BanquetBillDataSync>();

                if (banquet.BanquetReservation != null)
                {
                    banquet.BanquetReservation.BanquetReservationDetail = banquet.BanquetReservationDetails;
                    banquet.BanquetReservation.BanquetReservationClassificationDiscount = banquet.ClassificationDiscounts;

                    var response = converisonService.ConvertDateTimeUTCtoLocalTime(banquet);

                    if (response.Success)
                        response = reservationService.Sync(banquet);
                    return Json(new { response.Success, banquet.BanquetReservation.GuidId, banquet.IsSyncCompleted });
                }
                else
                    return Json(new { Success = false });
            }
            else
                return Json(new { Success = false, ErrorMessage = "Model is not in valid format" });
        }

        [Route("SyncLocation")]
        public IHttpActionResult PostSetupData(SetUpData setupData)
        {
            bool response = false;

            if (ModelState.IsValid)
            {
                GenericService<SetUpData> converisonService = new GenericService<SetUpData>();
                converisonService.ConvertDateTimeUTCtoLocalTime(setupData);

                if (setupData.CommonCostCenters.Count > 0)
                    response = costCenterService.TruncateAllAndInsertWithTransaction(setupData.CommonCostCenters).Success;

                if (setupData.HotelGuestCompanys.Count > 0)
                    response = hotelGuestCompanyService.TruncateAllAndInsertWithTransaction(setupData.HotelGuestCompanys).Success;

                if (setupData.InvUnitHeads.Count > 0)
                    response = unitHeadService.TruncateAllAndInsertWithTransaction(setupData.InvUnitHeads).Success;

                if (setupData.InvUnitConversions.Count > 0)
                    response = unitConversionService.TruncateAllAndInsertWithTransaction(setupData.InvUnitConversions).Success;

                if (setupData.InvLocations.Count > 0)
                    response = invLocationService.TruncateAllAndInsertWithTransaction(setupData.InvLocations).Success;

                if (setupData.InvLocationCostCenterMappings.Count > 0)
                    response = invLocationCostCenterMappingService.TruncateAllAndInsertWithTransaction(setupData.InvLocationCostCenterMappings).Success;

                if (setupData.InvCategorys.Count > 0)
                    response = invCategoryService.TruncateAllAndInsertWithTransaction(setupData.InvCategorys).Success;

                if (setupData.InvCategoryCostCenterMappings.Count > 0)
                    response = invCategoryCostCenterMappingService.TruncateAllAndInsertWithTransaction(setupData.InvCategoryCostCenterMappings).Success;

                if (setupData.InvInventoryAccountVsItemCategoryMapppings.Count > 0)
                    response = invInventoryAccountVsItemCategoryMapppingService.TruncateAllAndInsertWithTransaction(setupData.InvInventoryAccountVsItemCategoryMapppings).Success;

                if (setupData.InvCogsAccountVsItemCategoryMapppings.Count > 0)
                    response = invCogsAccountVsItemCategoryMapppingService.TruncateAllAndInsertWithTransaction(setupData.InvCogsAccountVsItemCategoryMapppings).Success;

                if (setupData.InvItems.Count > 0)
                    response = invItemService.TruncateAllAndInsertWithTransaction(setupData.InvItems).Success;

                if (setupData.InvItemCostCenterMappings.Count > 0)
                    response = invItemCostCenterMappingService.TruncateAllAndInsertWithTransaction(setupData.InvItemCostCenterMappings).Success;

                if (setupData.RestaurantRecipeDetails.Count > 0)
                    response = restaurantRecipeDetailService.TruncateAllAndInsertWithTransaction(setupData.RestaurantRecipeDetails).Success;

                if (setupData.InvItemClassificationCostCenterMappings.Count > 0)
                    response = invItemClassificationCostCenterMappingService.TruncateAll().Success;

                if (setupData.InvItemClassifications.Count > 0)
                    response = invItemClassificationService.DeleteAll().Success;

                if (setupData.InvItemClassifications.Count > 0)
                    response = invItemClassificationService.InsertWithIdentity(setupData.InvItemClassifications).Success;

                if (setupData.InvItemClassificationCostCenterMappings.Count > 0)
                    response = invItemClassificationCostCenterMappingService.InsertWithIdentity(setupData.InvItemClassificationCostCenterMappings).Success;

                if (setupData.HotelRoomTypes.Count > 0)
                    response = hotelRoomTypeService.TruncateAllAndInsertWithTransaction(setupData.HotelRoomTypes).Success;

                if (setupData.HotelRoomNumbers.Count > 0)
                    response = roomNumberService.TruncateAllAndInsertWithTransaction(setupData.HotelRoomNumbers).Success;

                if (response)
                    return Json(new { Success = true, AlertMessage = "Setup Data Sync Successful." });
                else
                    return Json(new { Success = false, AlertMessage = "Setup Data Sync Failed." });
            }

            else
                return Json(new { Success = false, ErrorMessage = "Model is not in valid format." });
        }


    }
}
