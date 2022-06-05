using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmVatAdjustment : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCostCenter();
            }
        }

        public void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> AllList = new List<CostCentreTabBO>();
            AllList = costCentreTabDA.GetCostCentreTabInfo();

            List<CostCentreTabBO> shortList = new List<CostCentreTabBO>();
            CostCentreTabBO bo = new CostCentreTabBO();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int userInfoId = currentUserInformationBO.UserInfoId;

            RestaurantBearerDA restaurantBearerDA = new RestaurantBearerDA();
            List<RestaurantBearerBO> costCenterInfoBOList = new List<RestaurantBearerBO>();
            costCenterInfoBOList = restaurantBearerDA.GetRestaurantInfoForBearerByEmpIdNIsBearer(userInfoId, 0);

            if (costCenterInfoBOList.Count == 0)
            {
                shortList = AllList;
            }
            else
            {
                foreach (RestaurantBearerBO rb in costCenterInfoBOList)
                {
                    bo = AllList.Where(a => a.CostCenterId == rb.CostCenterId).FirstOrDefault();
                    shortList.Add(bo);
                }
            }

            this.ddlCostCenter.DataSource = shortList;
            this.ddlCostCenter.DataTextField = "CostCenter";
            this.ddlCostCenter.DataValueField = "CostCenterId";
            this.ddlCostCenter.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCostCenter.Items.Insert(0, item);
        }

        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static GridViewDataNPaging<RestaurantBillBO, GridPaging> SearchBillAndLoadGridInformation(string strSearchDate, string costCenterId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int userInfoId = currentUserInformationBO.UserInfoId;
            //int costCenterId = currentUserInformationBO.WorkingCostCenterId;
            int costCenter = Convert.ToInt32(costCenterId);
            string billNo = "";

            int totalRecords = 0;

            GridViewDataNPaging<RestaurantBillBO, GridPaging> myGridData = new GridViewDataNPaging<RestaurantBillBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            RestaurentBillDA rrDA = new RestaurentBillDA();
            List<RestaurantBillBO> reservationInfoList = new List<RestaurantBillBO>();

            DateTime fromDate = DateTime.Now.AddDays(-30);
            DateTime toDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(strSearchDate))
            {
                fromDate = hmUtility.GetDateTimeFromString(strSearchDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(strSearchDate))
            {
                toDate = hmUtility.GetDateTimeFromString(strSearchDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            reservationInfoList = rrDA.GetRestaurantBillInfoBySearchCriteriaForpagination(fromDate, toDate, billNo, "", "", userInfoId, costCenter, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<RestaurantBillBO> distinctItems = new List<RestaurantBillBO>();
            distinctItems = reservationInfoList.GroupBy(test => test.BillId).Select(group => group.First()).ToList();


            HMCommonDA hmCoomnoDA = new HMCommonDA();
            DayCloseBO dayCloseBO = new DayCloseBO();

            DateTime transactionDate = !string.IsNullOrWhiteSpace(strSearchDate) ? hmUtility.GetDateTimeFromString(strSearchDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) : DateTime.Now;

            foreach (RestaurantBillBO row in distinctItems)
            {
                dayCloseBO = hmCoomnoDA.GetHotelDayCloseInformation(transactionDate);
                if (dayCloseBO != null)
                {
                    if (dayCloseBO.DayCloseId > 0)
                    {
                        row.IsDayClosed = 1;
                    }
                    else
                    {
                        row.IsDayClosed = 0;
                    }
                }
                else
                {
                    row.IsDayClosed = 0;
                }

                Boolean isSavePermission = false;
                Boolean isDeletePermission = false;
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
                objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmVatAdjustment.ToString());
                isSavePermission = objectPermissionBO.IsSavePermission;
                isDeletePermission = objectPermissionBO.IsDeletePermission;
                if (!isSavePermission)
                {
                    row.IsDayClosed = 1;
                }

                if (!isDeletePermission)
                {
                    row.IsDayClosed = 1;
                }
            }

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo UpdateBill(List<string> deletedBillList, List<string> savedBillList)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            bool status = false;

            try
            {                
                RestaurentBillDA restaurantBillDA = new RestaurentBillDA();
                if (deletedBillList.Count > 0)
                {
                    status = restaurantBillDA.UpdateBillForVatAdjustment(deletedBillList, "Delete");
                }

                if (savedBillList.Count > 0)
                {
                    status = restaurantBillDA.UpdateBillForVatAdjustment(savedBillList, "Save");
                }

                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }

                if (!status)
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }
    }
}