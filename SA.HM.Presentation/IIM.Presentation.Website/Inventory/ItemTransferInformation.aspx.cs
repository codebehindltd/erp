using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using System.Collections;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.SalesManagment;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using Newtonsoft.Json;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class ItemTransferInformation : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProductOutFor();
                CheckPermission();
            }
        }

        private void LoadProductOutFor()
        {
            ListItem itemR = new ListItem();
            itemR.Value = "Requisition";
            itemR.Text = "Requisition Wise Transfer";
            ddlSearchProductOutFor.Items.Insert(0, itemR);

            ListItem itemRe = new ListItem();
            itemRe.Value = "StockTransfer";
            itemRe.Text = "Stock Transfer";
            ddlSearchProductOutFor.Items.Insert(1, itemRe);

            ddlSearchProductOutFor.Items.Insert(0, new ListItem(hmUtility.GetDropDownFirstAllValue(), "All"));
        }

        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value =  isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";

        }

        [WebMethod]
        public static GridViewDataNPaging<PMProductOutBO, GridPaging> SearchOutOrder(string outType, DateTime? fromDate, DateTime? toDate,
                                                                                        string issueNumber, string status,
                                                                                        int gridRecordsCount, int pageNumber,
                                                                                        int isCurrentOrPreviousPage
                                                                                       )
        {

            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<PMProductOutBO, GridPaging> myGridData = new GridViewDataNPaging<PMProductOutBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (fromDate == null) fromDate = DateTime.Now;
            if (toDate == null) toDate = DateTime.Now;

            PMProductOutDA receiveDA = new PMProductOutDA();
            List<PMProductOutBO> orderList = new List<PMProductOutBO>();

            orderList = receiveDA.GetProductOutForSearch(outType, fromDate, toDate, issueNumber, status, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords).Where(x => x.IssueType != "FixedAsset").ToList();

            myGridData.GridPagingProcessing(orderList, totalRecords);
            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo OutOrderApproval(string productOutFor, int outId, string approvedStatus, int requisitionOrSalesId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductOutDA orderDa = new PMProductOutDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDa.OutOrderApproval(productOutFor, outId, approvedStatus, requisitionOrSalesId, userInformationBO.UserInfoId);

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isTransferProductReceiveDisable = new HMCommonSetupBO();

                isTransferProductReceiveDisable = commonSetupDA.GetCommonConfigurationInfo("IsTransferProductReceiveDisable", "IsTransferProductReceiveDisable");

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                   

                    rtninf.IsSuccess = true;

                    if (approvedStatus == HMConstants.ApprovalStatus.Checked.ToString())
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                    }
                    else
                    {
                        if (isTransferProductReceiveDisable.SetupValue == "1")
                        {
                            rtninf.IsSuccess = orderDa.ItemReceiveOutOrder(productOutFor, outId, requisitionOrSalesId, userInformationBO.UserInfoId);
                        }
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    }

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.PMProductOut.ToString(), outId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductOut));


                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }

        [WebMethod]
        public static ReturnInfo OutOrderDelete(string issueType, int outId, string approvedStatus, int createdBy)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductOutDA orderDa = new PMProductOutDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDa.OutOrderDelete(issueType, outId, approvedStatus, createdBy, userInformationBO.UserInfoId);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.PMProductOut.ToString(), outId,
                              ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductOut));

                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }

    }
}