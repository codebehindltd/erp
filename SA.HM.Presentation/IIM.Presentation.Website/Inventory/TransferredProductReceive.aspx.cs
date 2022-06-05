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
    public partial class TransferredProductReceive : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadAllCostCentreInfo();
                CheckPermission();
            }
        }
        private void LoadAllCostCentreInfo()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();

            //HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            //HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
            //invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsUserpermissionAppliedToCostcenterFilteringForPOPR", "IsUserpermissionAppliedToCostcenterFilteringForPOPR");

            //if (Convert.ToInt16(invoiceTemplateBO.SetupValue) != 0)
            //{
            //    costCentreTabBOList = costCentreTabDA.GetUserWisePRPOCostCentreTabInfo(userInformationBO.UserInfoId, HMConstants.CostcenterFilteringForPOPR.Receive.ToString());
            //}
            //else
            //{
            costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId)
                                        .Where(o => o.CostCenterType == "Inventory").ToList();
            //}

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            ddlCostCenter.DataSource = costCentreTabBOList;
            ddlCostCenter.DataTextField = "CostCenter";
            ddlCostCenter.DataValueField = "CostCenterId";
            ddlCostCenter.DataBind();
            if (costCentreTabBOList.Count > 1)
                ddlCostCenter.Items.Insert(0, item);

        }

        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }

        [WebMethod]
        public static List<InvLocationBO> InvLocationByCostCenter(int costCenterId)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvItemLocationByCostCenter(costCenterId);

            return location;
        }

        [WebMethod]
        public static GridViewDataNPaging<PMProductOutBO, GridPaging> SearchOutOrder(Int64 costCenterId, Int64 locationId, DateTime? fromDate, DateTime? toDate,
                                                                                      string status, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage
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

            orderList = receiveDA.GetTransferOrderForReceive(costCenterId, locationId, fromDate, toDate, status, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(orderList, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo ItemReceiveOutOrder(string productOutFor, int outId, int requisitionOrSalesId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductOutDA orderDa = new PMProductOutDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDa.ItemReceiveOutOrder(productOutFor, outId, requisitionOrSalesId, userInformationBO.UserInfoId);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Received, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.ReceivedAfterOut.ToString(), EntityTypeEnum.EntityType.PMProductOut.ToString(), outId,
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
        public static OutOrderViewBO EditItemOut(string issueType, int outId, int requisitionOrSalesId)
        {
            OutOrderViewBO viewBo = new OutOrderViewBO();
            PMProductOutDA orderDa = new PMProductOutDA();

            viewBo.ProductSerialInfo = orderDa.GetItemOutSerialById(outId);
            viewBo.ProductOutDetails = orderDa.GetItemOutDetailsByOutId(outId);

            return viewBo;
        }

    }
}