using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class ItemRequisitionInformation : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStatusDropDown();
                IsRequisitionCheckedByEnable();
                CheckObjectPermission();
                CheckPermission();
                companyProjectUserControl.ConnfigurationID = "IsInventoryIntegrateWithAccounts";
                LoadRequisitionBy();
            }
        }
        protected void btnNewRequisition_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PurchaseManagment/frmPMRequisition.aspx");
        }
        private void LoadStatusDropDown()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("ApprovalPolicyConfiguration", "ApprovalPolicyConfiguration");
            if (homePageSetupBO != null)
            {
                if (homePageSetupBO.SetupId > 0)
                {
                    if (homePageSetupBO.SetupValue == "0")
                    {
                        this.ddlStatus.Items.Remove(ddlStatus.Items.FindByValue("Partially Checked"));
                        this.ddlStatus.Items.Remove(ddlStatus.Items.FindByValue("Partially Approved"));
                    }
                }
            }
        }
        private void IsRequisitionCheckedByEnable()
        {
            hfIsRequisitionCheckedByEnable.Value = "1";
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO checkNApproveBO = new HMCommonSetupBO();
            checkNApproveBO = commonSetupDA.GetCommonConfigurationInfo("IsRequisitionCheckedByEnable", "IsRequisitionCheckedByEnable");
            if (Convert.ToInt32(checkNApproveBO.SetupValue) != 1)
            {
                hfIsRequisitionCheckedByEnable.Value = "0";
                this.ddlStatus.Items.Remove(ddlStatus.Items.FindByValue(HMConstants.ApprovalStatus.Checked.ToString()));
            }
        }        
        private void LoadRequisitionBy()
        {
            UserInformationDA empDa = new UserInformationDA();
            List<UserInformationBO> allUserInformation = new List<UserInformationBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            allUserInformation = empDa.GetAllUserInformation();

            ddlRequisitionBy.DataSource = allUserInformation;
            ddlRequisitionBy.DataTextField = "UserIdAndUserName";
            ddlRequisitionBy.DataValueField = "UserInfoId";
            ddlRequisitionBy.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlRequisitionBy.Items.Insert(0, item);
        }
        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.ItemRequisitionInformation.ToString());
        }
        [WebMethod]
        public static List<GLProjectBO> GetGLProjectByGLCompanyId(int companyId)
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            GLProjectDA entityDA = new GLProjectDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            projectList = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(companyId, userInformationBO.UserGroupId);
            if (projectList.Count == 0)
                projectList.Add(entityDA.GetGLProjectInfoByGLCompany(companyId).FirstOrDefault());
            return projectList;
        }
        [WebMethod]
        public static GridViewDataNPaging<PMRequisitionViewBO, GridPaging> GetRequisitionDetails(string rFromDate, string rToDate, string requisitionNo, string status, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage, int companyId, int projectId, int requisitionBy)
        {
            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<PMRequisitionViewBO, GridPaging> myGridData = new GridViewDataNPaging<PMRequisitionViewBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            List<PMRequisitionViewBO> allrequisitionList = new List<PMRequisitionViewBO>();
            PMRequisitionDA requisitionDA = new PMRequisitionDA();

            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (!string.IsNullOrWhiteSpace(rFromDate))
            {
                fromDate = hmUtility.GetDateTimeFromString(rFromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(rToDate))
            {
                toDate = hmUtility.GetDateTimeFromString(rToDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            allrequisitionList = requisitionDA.GetPMRequisitionInfoByStatus(fromDate, toDate, requisitionNo, status, userInformationBO.UserInfoId, companyId, projectId, requisitionBy, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(allrequisitionList, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static string GetRequisitionDetailsForApproval(int requisitionid)
        {
            PMRequisitionDA requisitionDA = new PMRequisitionDA();
            List<PMRequisitionDetailsBO> requisitionDetails = new List<PMRequisitionDetailsBO>();
            requisitionDetails = requisitionDA.GetPMRequisitionDetailsByID(requisitionid);

            int row = 0;
            string tr = string.Empty;

            tr = "<table id='DetailsRequisitionGrid' class='table table-bordered table-condensed table-responsive' style='width: 100%;'> " +
                 "<thead> " +
                 "    <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'> " +
                 "       <th style='width: 4%;'>" + "Select" +
                            "<input type = \"checkbox\" value = \"\" checked= \"checked\" id = \"OrderCheck\" onclick = \"CheckAllOrder()\" />" +
                        "</th> " +
                 "       <th style='width: 14%;'> " +
                 "            Item " +
                 "        </th> " +
                 "        <th style='width: 8%;'> " +
                 "            Unit " +
                 "        </th> " +
                 "        <th style='width: 8%;'> " +
                 "            Quantity " +
                 "        </th> " +
                 "        <th style='width: 14%;'> " +
                 "            Approved Quantity " +
                 "        </th> " +
                 "        <th style='width: 8%;'> " +
                 "            Remarks " +
                 "        </th> " +
                 "        <th style='display: none;'> " +
                 "            Details Id " +
                 "        </th> " +
                 "        <th style='display: none;'> " +
                 "            Is Edit" +
                 "        </th> " +
                 "        <th style='display: none;'> " +
                 "            InitialApprovedStatus" +
                 "        </th> " +

                 "        <th style='width: 12%;'> " +
                 "            Available Quantity" +
                 "        </th> " +
                 "        <th style='width: 16%;'> " +
                 "            Last Requisition Quantity " +
                 "        </th> " +
                 "        <th style='width: 14%;'> " +
                 "            Last Transfer Quantity " +
                 "        </th> " +
                 "    </tr> " +
                 "</thead> " +
                 "<tbody> ";

            string requisitionRemarks = string.Empty;
            foreach (PMRequisitionDetailsBO rd in requisitionDetails)
            {
                if (row % 2 == 0)
                {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else
                {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                if (rd.ApprovedStatus == HMConstants.ApprovalStatus.Approved.ToString() || rd.ApprovedStatus == HMConstants.ApprovalStatus.Cancel.ToString())
                {
                    tr += "<td style='width:4%;'> <input type='checkbox' checked= 'checked' disabled='disabled' id='chkb" + rd.RequisitionDetailsId.ToString() + "' /> </td>";
                }
                else if (rd.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString())
                {
                    tr += "<td style='width:4%;'> <input type='checkbox' checked= 'checked' id='chkb" + rd.RequisitionDetailsId.ToString() + "' /> </td>";
                }

                tr += "<td style='width:14%;'>" + rd.ItemName + "</td>";
                tr += "<td style='width:8%;'>" + rd.HeadName + "</td>";
                tr += "<td style='width:8%;'>" + rd.Quantity + "</td>";

                if (rd.ApprovedStatus == HMConstants.ApprovalStatus.Approved.ToString() || rd.ApprovedStatus == HMConstants.ApprovalStatus.Cancel.ToString())
                {
                    tr += "<td style='width:14%; text-align:center;'> <input type='text' id='txt" + rd.RequisitionDetailsId.ToString() + "' value = '" + rd.ApprovedQuantity + "' disabled='disabled' onblur='CheckInputValue(this)' class='form-control' style='width:65px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                }
                else if (rd.ApprovedStatus == HMConstants.ApprovalStatus.Checked.ToString())
                {
                    tr += "<td style='width:14%; text-align:center;'> <input type='text' id='txt" + rd.RequisitionDetailsId.ToString() + "' value = '" + rd.ApprovedQuantity + "' onblur='CheckInputValue(this)' class='form-control' style='width:65px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                }
                else
                {
                    tr += "<td style='width:14%; text-align:center;'> <input type='text' id='txt" + rd.RequisitionDetailsId.ToString() + "' value = '" + rd.Quantity + "' onblur='CheckInputValue(this)' class='form-control' style='width:65px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                }

                if (rd.ApprovedStatus == HMConstants.ApprovalStatus.Approved.ToString())
                {
                    tr += "<td style='width:8%; '>" + rd.ItemRemarks + "</td>";
                    //tr += "<td style='width:20%; text-align:center;'><a href='#' onclick= 'EditApprovedRequisition(this)' ><img alt='Delete' src='../Images/edit.png' /></a></td>";
                }
                else
                {
                    tr += "<td style='width:8%; '>" + rd.ItemRemarks + "</td>";
                }

                tr += "<td style='display:none;'>" + rd.RequisitionDetailsId + "</td>";
                tr += "<td style='display:none;'>0</td>";
                tr += "<td style='display:none;'>" + rd.InitialApprovedStatus.ToString() + "</td>";

                tr += "<td style='width:12%; '>" + rd.CurrentStockFromStore + "</td>";
                tr += "<td style='width:16%; '>" + rd.LastRequisitionQuantity + "</td>";
                if (rd.LastTransferQuantity != 0)
                    tr += "<td style='width:14%; '>" + rd.LastTransferQuantity + "(" + rd.LastTransferType + ")</td>";
                else
                    tr += "<td style='width:14%; '>" + rd.LastTransferQuantity + "</td>";

                tr += "</tr>";
                requisitionRemarks = rd.RequisitionRemarks;

                row++;
            }

            tr += "</tbody> " + "</table> ";

            tr += "<div class='divClear'></ div > ";
            tr += "<div>Requisition Remarks: " + requisitionRemarks + "</ div >";
            tr += "<div><b>Requisition By: " + requisitionDetails[0].RequisitionBy + "</b></ div >";
            if (requisitionDetails[0].CheckedBy != null)
            {
                tr += "<div><b>Checked By: " + requisitionDetails[0].CheckedBy + "</b></ div >";
            }
            
            return tr;
        }
        [WebMethod]
        public static ReturnInfo ApprovedRequsition(int hfIsRequisitionCheckedByEnable, int requsitionId, List<PMRequisitionDetailsBO> approvedItem, List<PMRequisitionDetailsBO> pendingItem)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;
            string TransactionNo;
            string TransactionType;
            string ApproveStatus;
            string requisitionApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                PMRequisitionBO requsition = new PMRequisitionBO();
                PMRequisitionDA requisitionDA = new PMRequisitionDA();

                requsition.RequisitionId = requsitionId;
                if (approvedItem.Count == 0)
                {
                    requsition.ApprovedStatus = HMConstants.ApprovalStatus.Cancel.ToString();
                    requsition.LastModifiedBy = userInformationBO.UserInfoId;
                }
                else if (hfIsRequisitionCheckedByEnable == 1)
                {

                    requsition = requisitionDA.GetPMRequisitionInfoByID(requsition.RequisitionId);
                    if (requsition != null)
                    {
                        requisitionApprovedStatus = requsition.ApprovedStatus;
                        if (requisitionApprovedStatus == HMConstants.ApprovalStatus.Submit.ToString() || requisitionApprovedStatus.Replace(" ", "") == HMConstants.ApprovalStatus.PartiallyChecked.ToString())
                        {
                            requsition.ApprovedStatus = HMConstants.ApprovalStatus.Checked.ToString();
                            requsition.CheckedBy = userInformationBO.UserInfoId;
                        }
                        else if (requisitionApprovedStatus == HMConstants.ApprovalStatus.Checked.ToString() || requisitionApprovedStatus.Replace(" ", "") == HMConstants.ApprovalStatus.PartiallyApproved.ToString())
                        {
                            requsition.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();
                            requsition.ApprovedBy = userInformationBO.UserInfoId;
                        }
                    }
                }
                else
                {
                    requsition.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();
                    requisitionApprovedStatus = requsition.ApprovedStatus;
                    requsition.ApprovedBy = userInformationBO.UserInfoId;
                }
                if (approvedItem.Count == 0)
                    status = requisitionDA.CancelPMRequisitionInfo(requsition.RequisitionId, userInformationBO.UserInfoId, out TransactionNo, out TransactionType, out ApproveStatus);
                else
                    status = requisitionDA.ApprovePMRequisitionInfo(requsition, approvedItem, pendingItem, out TransactionNo, out TransactionType, out ApproveStatus);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ProductRequisition.ToString(), requsitionId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductRequisition));
                    rtninf.IsSuccess = true;
                    rtninf.PrimaryKeyValue = requsition.RequisitionId.ToString();
                    rtninf.TransactionNo = TransactionNo;
                    rtninf.TransactionType = TransactionType;
                    rtninf.TransactionStatus = ApproveStatus;
                    if (rtninf.TransactionStatus == "Cancel")
                    {

                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Cancel, AlertType.Success);
                    }
                    else if (rtninf.TransactionStatus == "Partially Checked" || rtninf.TransactionStatus == "Checked")
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);

                    }
                    else if (rtninf.TransactionStatus == "Partially Approved" || rtninf.TransactionStatus == "Approved")
                    {

                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    }
                    else
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
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

        [WebMethod]
        public static ReturnInfo RequisitionOrderApproval(int requisitionId, string approvedStatus)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PurchaseOrderViewBO viewBo = new PurchaseOrderViewBO();
            PMRequisitionDA requisitionDA = new PMRequisitionDA();
            string TransactionNo;
            string TransactionType;
            string ApproveStatus;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = requisitionDA.RequisitionOrderApproval(requisitionId, approvedStatus, userInformationBO.UserInfoId, out TransactionNo, out TransactionType, out ApproveStatus);

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

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(approvedStatus, EntityTypeEnum.EntityType.ProductRequisition.ToString(), requisitionId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductRequisition));

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
        public static ReturnInfo CancelRequisition(int requsitionId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            bool status = false;
            PMRequisitionBO requsition = new PMRequisitionBO();
            requsition.RequisitionId = requsitionId;
            PMRequisitionDA requisitionDA = new PMRequisitionDA();
            PMRequisitionBO viewBo = new PMRequisitionBO();
            UserInformationBO userInformationBO = new UserInformationBO();
            string TransactionNo = "";
            string TransactionType = "";
            string ApproveStatus = "";
            string statusType = string.Empty;
            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                viewBo = requisitionDA.GetPMRequisitionInfoByID(requsition.RequisitionId);


                if (viewBo.CreatedBy == userInformationBO.UserInfoId && viewBo.ApprovedStatus == "Submit")
                {
                    status = requisitionDA.DeletePMRequisitionInfo(viewBo.RequisitionId);
                    statusType = "Hard Deleted";
                }

                else
                    status = requisitionDA.CancelPMRequisitionInfo(viewBo.RequisitionId, userInformationBO.UserInfoId, out TransactionNo, out TransactionType, out ApproveStatus);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.ProductRequisition.ToString(), requsitionId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductRequisition) + statusType);


                    rtninf.PrimaryKeyValue = viewBo.RequisitionId.ToString();
                    rtninf.TransactionNo = TransactionNo;
                    rtninf.TransactionType = TransactionType;
                    rtninf.TransactionStatus = ApproveStatus;
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Cancel, AlertType.Success);
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

        [WebMethod]
        public static ReturnInfo CompleteRequisitionOrder(int requisitionid)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMRequisitionDA requisitionDA = new PMRequisitionDA();
            HMUtility hmUtility = new HMUtility();

            try
            {
                rtninf.IsSuccess = requisitionDA.CopmpleteRequisitionOrder(requisitionid);

                if (rtninf.IsSuccess)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.CompleteRequisition.ToString(), EntityTypeEnum.EntityType.ProductRequisition.ToString(), requisitionid,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductRequisition));

                    rtninf.AlertMessage = CommonHelper.AlertInfo("Requisition Completeness Is Succeed", AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
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
        public static ReturnInfo CompleteRequisitionTransferOrder(int requisitionid)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMRequisitionDA requisitionDA = new PMRequisitionDA();
            HMUtility hmUtility = new HMUtility();

            try
            {
                rtninf.IsSuccess = requisitionDA.CompleteRequisitionTransferOrder(requisitionid);

                if (rtninf.IsSuccess)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.CompleteRequisitionTransfer.ToString(), EntityTypeEnum.EntityType.ProductRequisition.ToString(), requisitionid,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductRequisition));

                    rtninf.AlertMessage = CommonHelper.AlertInfo("Transfer Completeness Is Succeed", AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
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