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
    public partial class ConsumptionReturnInformation : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        //private void LoadProductOutFor()
        //{
        //    ListItem itemR = new ListItem();
        //    itemR.Value = "OutReturn";
        //    itemR.Text = "Out Item Return";
        //    ddlSearchReturnType.Items.Insert(0, itemR);

        //    ListItem itemRe = new ListItem();
        //    itemRe.Value = "SalesReturn";
        //    itemRe.Text = "Sales Return";
        //    ddlSearchReturnType.Items.Insert(1, itemRe);
        //    ddlSearchReturnType.Items.Insert(0, new ListItem(hmUtility.GetDropDownFirstValue(), "All"));
        //}

        [WebMethod]
        public static GridViewDataNPaging<PMProductReturnBO, GridPaging> SearchReturnOrder(string returnType, DateTime? fromDate, DateTime? toDate,
                                                                                        string returnNumber, string status,
                                                                                        int gridRecordsCount, int pageNumber,
                                                                                        int isCurrentOrPreviousPage
                                                                                       )
        {

            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<PMProductReturnBO, GridPaging> myGridData = new GridViewDataNPaging<PMProductReturnBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (fromDate == null) fromDate = DateTime.Now;
            if (toDate == null) toDate = DateTime.Now;

            PMProductReturnDA returnda = new PMProductReturnDA();
            List<PMProductReturnBO> orderList = new List<PMProductReturnBO>();

            orderList = returnda.GetProductReturnForSearch(returnType, fromDate, toDate, returnNumber, status, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(orderList, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo OutOrderApproval(string returnType, Int64 returnId, string approvedStatus, Int64 transactionId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductReturnDA orderDa = new PMProductReturnDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDa.ReturnTransferItemApproval(returnType, returnId, approvedStatus, transactionId, userInformationBO.UserInfoId);

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

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(approvedStatus, EntityTypeEnum.EntityType.PMProductReturn.ToString(), returnId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductReturn));


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
        public static ReturnInfo OutOrderDelete(string returnType, Int64 returnId, string approvedStatus, Int64 transactionId, int createdBy)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductReturnDA orderDa = new PMProductReturnDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDa.ReturnTransferItemDelete(returnType, returnId, approvedStatus, transactionId, createdBy, userInformationBO.UserInfoId);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.PMProductReturn.ToString(), returnId,
                              ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductReturn));

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