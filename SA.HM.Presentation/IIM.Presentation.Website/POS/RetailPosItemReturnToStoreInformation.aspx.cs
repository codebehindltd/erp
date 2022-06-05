using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class RetailPosItemReturnToStoreInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static List<RestaurantBillForReturnToStoreView> GetBills(DateTime? fromDate, DateTime? toDate, string billNumber)
        {

            RestaurentBillDA billDA = new RestaurentBillDA();
            List<RestaurantBillForReturnToStoreView> billList = new List<RestaurantBillForReturnToStoreView>();

            if (fromDate == null)
                fromDate = DateTime.Now.Date;

            if (toDate == null)
                toDate = DateTime.Now.Date;

            billList = billDA.GetRestaurantBillInfoBySearchCriteriaForReturnToStock(fromDate, toDate, billNumber);

            return billList;
        }

        [WebMethod]
        public static List<RestaurantSalesReturnItemViewBO> GetReturnItemDetails(int billId)
        {

            RestaurentPosDA posDA = new RestaurentPosDA();
            List<RestaurantSalesReturnItemViewBO> itemList = new List<RestaurantSalesReturnItemViewBO>();

            itemList = posDA.GetReturnItemDetailsByBillId(billId);

            return itemList;
        }

        [WebMethod]
        public static ReturnInfo SaveItemReturn(List<RestaurantSalesReturnItemViewBO> returnItemsList)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            RestaurentPosDA posDA = new RestaurentPosDA();

            returnInfo.IsSuccess = posDA.UpdateStockInformationAfterItemReturn(returnItemsList, userInformationBO.UserInfoId);

            if (returnInfo.IsSuccess)
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RestaurantSalesReturnItem.ToString(), returnItemsList[0].ReturnId,
                        ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantSalesReturnItem));

            }
            else
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return returnInfo;
        }

    }
}