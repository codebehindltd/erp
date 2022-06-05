using HotelManagement.Data.HMCommon;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class ItemConsumptionInformation : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckPermission();
            if (!IsPostBack)
            {
                IsItemConsumptionDeliveryChallanEnable();
            }
        }
        private void IsItemConsumptionDeliveryChallanEnable()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            hfIsItemConsumptionDeliveryChallanEnable.Value = "0";
            HMCommonSetupBO isItemConsumptionDeliveryChallanEnableBO = new HMCommonSetupBO();
            isItemConsumptionDeliveryChallanEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsItemConsumptionDeliveryChallanEnable", "IsItemConsumptionDeliveryChallanEnable");
            if (isItemConsumptionDeliveryChallanEnableBO != null)
            {
                if (isItemConsumptionDeliveryChallanEnableBO.SetupId > 0)
                {
                    if (isItemConsumptionDeliveryChallanEnableBO.SetupValue == "1")
                    {
                        hfIsItemConsumptionDeliveryChallanEnable.Value = "1";
                    }
                }
            }
        }
        [WebMethod]
        public static GridViewDataNPaging<PMProductOutBO, GridPaging> GetConsumptionDetails(DateTime? fromDate, DateTime? toDate,
            string status, string issueType, string issueNumber, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            GridViewDataNPaging<PMProductOutBO, GridPaging> myGridData = new GridViewDataNPaging<PMProductOutBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMUtility hmUtility = new HMUtility();
            PMProductOutDA outDA = new PMProductOutDA();
            List<PMProductOutBO> outList = new List<PMProductOutBO>();

            string productOutFor = "DirectOut";

            outList = outDA.GetProductDirectOutForSearch(productOutFor, issueType, issueNumber, fromDate, toDate, status, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(outList, totalRecords);
            return myGridData;
        }

        [WebMethod]
        public static List<PMProductOutDetailsBO> GetProductOutDetails(int outId)
        {
            PMProductOutDA outDA = new PMProductOutDA();
            return outDA.GetProductOutDetailsInfoByOutId(outId);
        }

        [WebMethod]
        public static ReturnInfo CancelConsumption(int outId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            bool status = false;

            PMProductOutBO productOut = new PMProductOutBO();
            PMProductOutDA productOutDA = new PMProductOutDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            string statusType = string.Empty;
            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                productOut = productOutDA.GetProductOutById(outId);
                if (productOut.CreatedBy == userInformationBO.UserInfoId && productOut.Status == HMConstants.ApprovalStatus.Pending.ToString())
                {
                    status = productOutDA.DeleteProductOutNotCheckedInfo(outId);
                    statusType = "Hard Deleted";
                }
                else if (productOut.Status == HMConstants.ApprovalStatus.Approved.ToString())
                {
                    status = productOutDA.DeleteProductOutInfo(outId);
                }
                else
                {
                    status = productOutDA.CancelProductOutInfo(outId);
                }
                if (status)
                {
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                                   EntityTypeEnum.EntityType.PMProductOut.ToString(), outId,
                                   ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                                   hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductOut) + statusType);
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;
        }

        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
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

                rtninf.IsSuccess = orderDa.DirectOutOrderApproval(productOutFor, outId, approvedStatus, requisitionOrSalesId, userInformationBO.UserInfoId);

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

    }
}