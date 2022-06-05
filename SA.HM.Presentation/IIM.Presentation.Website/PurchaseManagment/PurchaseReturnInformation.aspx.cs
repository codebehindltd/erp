using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class PurchaseReturnInformation : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckPermission();
        }

        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }

        [WebMethod]
        public static GridViewDataNPaging<PMSupplierProductReturnBO, GridPaging> SearchReturnOrder(DateTime? fromDate, DateTime? toDate,
                                                                                     string returnNumber, string status,
                                                                                     int gridRecordsCount, int pageNumber,
                                                                                     int isCurrentOrPreviousPage
                                                                                    )
        {

            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<PMSupplierProductReturnBO, GridPaging> myGridData = new GridViewDataNPaging<PMSupplierProductReturnBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (fromDate == null) fromDate = DateTime.Now;
            if (toDate == null) toDate = DateTime.Now;

            PMProductReturnDA returnda = new PMProductReturnDA();
            List<PMSupplierProductReturnBO> orderList = new List<PMSupplierProductReturnBO>();

            orderList = returnda.GetReceivedOrderReturnForSearch(fromDate, toDate, returnNumber, status, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(orderList, totalRecords);
            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo ReceiveReturnApproval(Int64 returnId, string approvedStatus, int receivedId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductReturnDA orderDa = new PMProductReturnDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDa.ReceiveReturnApproval(returnId, approvedStatus, receivedId, userInformationBO.UserInfoId);

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
        public static ReturnInfo ReceiveReturnDelete(Int64 returnId, string approvedStatus, Int32 receivedId, int createdBy)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductReturnDA orderDa = new PMProductReturnDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDa.ReceiveReturnDelete(returnId, approvedStatus, receivedId, createdBy, userInformationBO.UserInfoId);

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