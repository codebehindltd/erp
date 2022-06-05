using HotelManagement.Data.Inventory;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmKitchenInformationDetails : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            hfUserInfoObj.Value = JsonConvert.SerializeObject(userInformationBO);

        }


        [WebMethod]
        public static GridViewDataNPaging<RestaurantKotSpecialRemarksDetailBO, GridPaging> GetSpecialRemarksDetails(int kotId, int itemId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            GridViewDataNPaging<RestaurantKotSpecialRemarksDetailBO, GridPaging> myGridData = new GridViewDataNPaging<RestaurantKotSpecialRemarksDetailBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            RestaurantKotSpecialRemarksDetailDA kotRemarksDa = new RestaurantKotSpecialRemarksDetailDA();
            List<RestaurantKotSpecialRemarksDetailBO> kotRemarks = new List<RestaurantKotSpecialRemarksDetailBO>();

            RestaurantKitchenDA restaurantKitchenDA = new RestaurantKitchenDA();

            kotRemarks = kotRemarksDa.GetInvItemSpecialRemarksInfoByIdForPegination(userInformationBO.UserInfoId, kotId, itemId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);



            myGridData.GridPagingProcessing(kotRemarks, totalRecords);
            return myGridData;

        }
        [WebMethod]
        public static GridViewDataNPaging<RestaurantKitchenBO, GridPaging> LoadKitchenInfo(int KitchenId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            List<RestaurantKitchenBO> RestaurantKitchenList = new List<RestaurantKitchenBO>();
            GridViewDataNPaging<RestaurantKitchenBO, GridPaging> myGridData = new GridViewDataNPaging<RestaurantKitchenBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);


            RestaurantKitchenDA restaurantKitchenDA = new RestaurantKitchenDA();

            RestaurantKitchenList = restaurantKitchenDA.GetALLKotByKitchenId(userInformationBO.UserInfoId, KitchenId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);



            myGridData.GridPagingProcessing(RestaurantKitchenList, totalRecords);
            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo completeDelivery(int itemId, int kotId)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            RestaurantKitchenDA restaurantKitchenDA = new RestaurantKitchenDA();
            try
            {
                status = restaurantKitchenDA.CompleteDeliveryProcess(itemId, kotId);
                if (status)
                {
                    info.IsSuccess = true;

                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delivery, AlertType.Success);
                    info.Data = 0;
                }
                else
                {
                    info.IsSuccess = false;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return info;
        }

        [WebMethod]
        public static GridViewDataNPaging<InvItemBO, GridPaging> LoadKotDetailsForKitchen(int KitchenId, int kotid, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            List<InvItemBO> InvItemBOList = new List<InvItemBO>();
            GridViewDataNPaging<InvItemBO, GridPaging> myGridData = new GridViewDataNPaging<InvItemBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);


            RestaurantKitchenDA restaurantKitchenDA = new RestaurantKitchenDA();

            InvItemBOList = restaurantKitchenDA.LoadKotDetailsForKitchen(userInformationBO.UserInfoId, KitchenId, kotid, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);



            myGridData.GridPagingProcessing(InvItemBOList, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static GridViewDataNPaging<RestaurantRecipeDetailBO, GridPaging> LoadRecipeByKotIdAndItemId(int ItemId, int kotid, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            GridViewDataNPaging<RestaurantRecipeDetailBO, GridPaging> myGridData = new GridViewDataNPaging<RestaurantRecipeDetailBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);


            List<RestaurantRecipeDetailBO> itemInfo = new List<RestaurantRecipeDetailBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.LoadRecipeByKotIdAndItemId(userInformationBO.UserInfoId, ItemId, kotid, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);




            myGridData.GridPagingProcessing(itemInfo, totalRecords);
            return myGridData;
        }
    }
}