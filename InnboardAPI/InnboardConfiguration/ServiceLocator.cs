using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using InnboardDomain.Models;
using InnboardDomain.Interfaces;
using InnboardAPI.DataAccesses;
using InnboardDataAccess.DataAccesses;

namespace InnboardConfiguration
{
    public class ServiceLocator
    {
        private static IUnityContainer _container;

        public static void RegisterMappings(IUnityContainer container)
        {
            _container = container;

            //container.RegisterType<IUserRepository, UserDataAccess>();
            //container.RegisterType<IGenericRepository<UserModel>, GenericDataAccess<UserModel>>();
            container.RegisterType<IBanquetReservation, BanquetReservationDataAccess>();
            container.RegisterType<IGenericRepository<BanquetReservation>, GenericDataAccess<BanquetReservation>>();

            container.RegisterType<IBanquetReservationDetail, BanquetReservationDetailDataAccess>();
            container.RegisterType<IGenericRepository<BanquetReservationDetail>, GenericDataAccess<BanquetReservationDetail>>();

            container.RegisterType<IBanquetReservationClassificationDiscount, BanquetReservationClassificationDiscountDataAccess>();
            container.RegisterType<IGenericRepository<BanquetReservationClassificationDiscount>, GenericDataAccess<BanquetReservationClassificationDiscount>>();

            container.RegisterType<IGLLedgerMaster, GLLedgerMasterDataAccess>();
            container.RegisterType<IGenericRepository<GLLedgerMaster>, GenericDataAccess<GLLedgerMaster>>();

            container.RegisterType<IHotelGuestBillApproved, HotelGuestBillApprovedDataAccess>();
            container.RegisterType<IGenericRepository<HotelGuestBillApproved>, GenericDataAccess<HotelGuestBillApproved>>();

            container.RegisterType<IHotelGuestBillPayment, HotelGuestBillPaymentDataAccess>();
            container.RegisterType<IGenericRepository<HotelGuestBillPayment>, GenericDataAccess<HotelGuestBillPayment>>();

            container.RegisterType<IHotelGuestCompany, HotelGuestCompanyDataAccess>();
            container.RegisterType<IGenericRepository<HotelGuestCompany>, GenericDataAccess<HotelGuestCompany>>();

            container.RegisterType<IHotelGuestDayLetCheckOut, HotelGuestDayLetCheckOutDataAccess>();
            container.RegisterType<IGenericRepository<HotelGuestDayLetCheckOut>, GenericDataAccess<HotelGuestDayLetCheckOut>>();

            container.RegisterType<IHotelGuestHouseCheckOut, HotelGuestHouseCheckOutDataAccess>();
            container.RegisterType<IGenericRepository<HotelGuestHouseCheckOut>, GenericDataAccess<HotelGuestHouseCheckOut>>();
            
            container.RegisterType<IHotelGuestInformation, HotelGuestInformationDataAccess>();
            container.RegisterType<IGenericRepository<HotelGuestInformation>, GenericDataAccess<HotelGuestInformation>>();

            container.RegisterType<IHotelGuestInformationOnline, HotelGuestInformationOnlineDataAccess>();
            container.RegisterType<IGenericRepository<HotelGuestInformationOnline>, GenericDataAccess<HotelGuestInformationOnline>>();

            container.RegisterType<IHotelGuestRegistration, HotelGuestRegistrationDataAccess>();
            container.RegisterType<IGenericRepository<HotelGuestRegistration>, GenericDataAccess<HotelGuestRegistration>>();

            container.RegisterType<IHotelGuestServiceBill, HotelGuestServiceBillDataAccess>();
            container.RegisterType<IGenericRepository<HotelGuestServiceBill>, GenericDataAccess<HotelGuestServiceBill>>();

            container.RegisterType<IHotelRegistrationAireportPickupDrop, HotelRegistrationAireportPickupDropDataAccess>();
            container.RegisterType<IGenericRepository<HotelRegistrationAireportPickupDrop>, GenericDataAccess<HotelRegistrationAireportPickupDrop>>();
            
            container.RegisterType<IHotelRoomRegistration, HotelRoomRegistrationDataAccess>();
            container.RegisterType<IGenericRepository<HotelRoomRegistration>, GenericDataAccess<HotelRoomRegistration>>();

            container.RegisterType<IHotelRoomReservationOnline, HotelRoomReservationOnlineDataAccess>();
            container.RegisterType<IGenericRepository<HotelRoomReservationOnline>, GenericDataAccess<HotelRoomReservationOnline>>();

            container.RegisterType<IHotelRoomReservationDetailOnline, HotelRoomReservationDetailOnlineDataAccess>();
            container.RegisterType<IGenericRepository<HotelRoomReservationDetailOnline>, GenericDataAccess<HotelRoomReservationDetailOnline>>();

            container.RegisterType<IHotelGuestExtraServiceBillApproved, HotelGuestExtraServiceBillApprovedDataAccess>();
            container.RegisterType<IGenericRepository<HotelGuestExtraServiceBillApproved>, GenericDataAccess<HotelGuestExtraServiceBillApproved>>();

            container.RegisterType<IRestaurantBill, RestaurantBillDataAccess>();
            container.RegisterType<IGenericRepository<RestaurantBill>, GenericDataAccess<RestaurantBill>>();

            container.RegisterType<IRestaurantBillClassificationDiscount, RestaurantBillClassificationDiscountDataAccess>();
            container.RegisterType<IGenericRepository<RestaurantBillClassificationDiscount>, GenericDataAccess<RestaurantBillClassificationDiscount>>();

            container.RegisterType<IRestaurantBillDetail, RestaurantBillDetailDataAccess>();
            container.RegisterType<IGenericRepository<RestaurantBillDetail>, GenericDataAccess<RestaurantBillDetail>>();

            container.RegisterType<IRestaurantKotBillDetail, RestaurantKotBillDetailDataAccess>();
            container.RegisterType<IGenericRepository<RestaurantKotBillDetail>, GenericDataAccess<RestaurantKotBillDetail>>();

            container.RegisterType<IRestaurantKotBillMaster, RestaurantKotBillMasterDataAccess>();
            container.RegisterType<IGenericRepository<RestaurantKotBillMaster>, GenericDataAccess<RestaurantKotBillMaster>>();

            container.RegisterType<IRestaurantKotSpecialRemarksDetail, RestaurantKotSpecialRemarksDetailDataAccess>();
            container.RegisterType<IGenericRepository<RestaurantKotSpecialRemarksDetail>, GenericDataAccess<RestaurantKotSpecialRemarksDetail>>();

            container.RegisterType<IHotelRoomNumber, HotelRoomNumberDataAccess>();
            container.RegisterType<IGenericRepository<HotelRoomNumber>, GenericDataAccess<HotelRoomNumber>>();

            container.RegisterType<IHotelCompanyPaymentLedger, HotelCompanyPaymentLedgerDataAccess>();
            container.RegisterType<IGenericRepository<HotelCompanyPaymentLedger>, GenericDataAccess<HotelCompanyPaymentLedger>>();

            container.RegisterType<ICommonCostCenter, CommonCostCenterDataAccess>();
            container.RegisterType<IGenericRepository<CommonCostCenter>, GenericDataAccess<CommonCostCenter>>();

            container.RegisterType<IInvUnitHead, InvUnitHeadDataAccess>();
            container.RegisterType<IGenericRepository<InvUnitHead>, GenericDataAccess<InvUnitHead>>();

            container.RegisterType<IInvUnitConversion, InvUnitConversionDataAccess>();
            container.RegisterType<IGenericRepository<InvUnitConversion>, GenericDataAccess<InvUnitConversion>>();

            container.RegisterType<IInvLocation, InvLocationDataAccess>();
            container.RegisterType<IGenericRepository<InvLocation>, GenericDataAccess<InvLocation>>();

            container.RegisterType<IInvLocationCostcenterMapping, InvLocationCostCenterMappingDataAccess>();
            container.RegisterType<IGenericRepository<InvLocationCostCenterMapping>, GenericDataAccess<InvLocationCostCenterMapping>>();

            container.RegisterType<IInvCategory, InvCategoryDataAccess>();
            container.RegisterType<IGenericRepository<InvCategory>, GenericDataAccess<InvCategory>>();

            container.RegisterType<IInvCategoryCostcenterMapping, InvCategoryCostCenterMappingDataAccess>();
            container.RegisterType<IGenericRepository<InvCategoryCostCenterMapping>, GenericDataAccess<InvCategoryCostCenterMapping>>();

            container.RegisterType<IInvInventoryAccountVsItemCategoryMappping, InvInventoryAccountVsItemCategoryMapppingDataAccess>();
            container.RegisterType<IGenericRepository<InvInventoryAccountVsItemCategoryMappping>, GenericDataAccess<InvInventoryAccountVsItemCategoryMappping>>();

            container.RegisterType<IInvCogsAccountVsItemCategoryMappping, InvCogsAccountVsItemCategoryMapppingDataAccess>();
            container.RegisterType<IGenericRepository<InvCogsAccountVsItemCategoryMappping>, GenericDataAccess<InvCogsAccountVsItemCategoryMappping>>();

            container.RegisterType<IInvItem, InvItemDataAccess>();
            container.RegisterType<IGenericRepository<InvItem>, GenericDataAccess<InvItem>>();

            container.RegisterType<IInvItemCostCenterMapping, InvItemCostCenterMappingDataAccess>();
            container.RegisterType<IGenericRepository<InvItemCostCenterMapping>, GenericDataAccess<InvItemCostCenterMapping>>();

            container.RegisterType<IRestaurantRecipeDetail, RestaurantRecipeDetailDataAccess>();
            container.RegisterType<IGenericRepository<RestaurantRecipeDetail>, GenericDataAccess<RestaurantRecipeDetail>>();

            container.RegisterType<IInvItemClassification, InvItemClassificationDataAccess>();
            container.RegisterType<IGenericRepository<InvItemClassification>, GenericDataAccess<InvItemClassification>>();

            container.RegisterType<IInvItemClassificationCostCenterMapping, InvItemClassificationCostCenterMappingDataAccess>();
            container.RegisterType<IGenericRepository<InvItemClassificationCostCenterMapping>, GenericDataAccess<InvItemClassificationCostCenterMapping>>();

            container.RegisterType<IHotelRoomType, HotelRoomTypeDataAccess>();
            container.RegisterType<IGenericRepository<HotelRoomType>, GenericDataAccess<HotelRoomType>>();


        }
        public static T GetInstance<T>()
        {
            try
            {
                return _container.Resolve<T>();
            }
            catch
            {
                return default(T);
            }
        }
    }
}
