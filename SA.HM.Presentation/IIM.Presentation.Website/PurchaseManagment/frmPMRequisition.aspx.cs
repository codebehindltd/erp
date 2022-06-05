using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.HMCommon;
using System.Globalization;
using HotelManagement.Entity.SalesManagment;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.UserInformation;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmPMRequisition : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        protected int isNewAddButtonEnable = 1;
        public frmPMRequisition() : base("ItemRequisitionInformation")
        {

        }
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO checkNApproveBO = new HMCommonSetupBO();
            //checkNApproveBO = commonSetupDA.GetCommonConfigurationInfo("IsRequisitionCheckedByEnable", "IsRequisitionCheckedByEnable");
            //if (Convert.ToInt32(checkNApproveBO.SetupValue) == 1)
            //{
            //    CheckNApproveDiv.Visible = true;
            //}
            //else
            //{
            //    CheckNApproveDiv.Visible = false;
            //}

            if (!IsPostBack)
            {
                companyProjectUserControl.ddlFirstValueVar = "select";
                LoadCommonDropDownHiddenField();
                LoadStockBy();
                LoadAllCostCentreTabInfo();
                LoadUserInformation();
                CheckObjectPermission();
                CheckPermission();
                IsInventoryIntegrateWithAccounts();
                LoadIsItemAttributeEnable();
                //LoadGLProject(false);
                //LoadCategory();
                //this.txtFromDate.Text = hmUtility.GetStringFromDateTime(DateTime.Now);
                //this.txtToDate.Text = hmUtility.GetStringFromDateTime(DateTime.Now);

            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);

            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridview();
        }
        protected void gvRequisitionInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                //PMRequisitionBO pMRequisitionBO = new PMRequisitionBO();
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                ImageButton imgApprove = (ImageButton)e.Row.FindControl("ImgApprove");
                ImageButton imgDetailsRequisition = (ImageButton)e.Row.FindControl("ImgDetailsRequisition");
                Label lblApprovedStatusValue = (Label)e.Row.FindControl("lblApprovedStatus");
                bool IsCanEdit = ((PMRequisitionBO)e.Row.DataItem).IsCanEdit.Value;
                bool IsCanDelete = ((PMRequisitionBO)e.Row.DataItem).IsCanDelete.Value;
                imgApprove.Visible = false;
                //(Label)e.Row.FindControl("") = (lblApprovedStatusValue.Text.Trim() == "Submit") ? "Submitted";
                Label lblIsCanCheckedValue = (Label)e.Row.FindControl("lblIsCanChecked");
                Label lblIsCanApprovedValue = (Label)e.Row.FindControl("lblIsCanApproved");


                if (lblIsCanCheckedValue.Text.Trim() == "1" || lblIsCanApprovedValue.Text.Trim() == "1")
                {
                    imgApprove.Visible = true;
                }
                else
                {
                    imgApprove.Visible = false;
                }

                if (lblIsCanCheckedValue.Text.Trim() == "1")
                {
                    imgApprove.ImageUrl = "~/Images/checked.png";
                    imgApprove.AlternateText = "Check";
                }
                else if (lblIsCanApprovedValue.Text.Trim() == "1")
                {
                    imgApprove.ImageUrl = "~/Images/approved.png";
                    imgApprove.AlternateText = "Approve";
                }


                if (IsCanDelete && isDeletePermission)
                {
                    imgDelete.Visible = true;
                }
                else
                {
                    imgDelete.Visible = false;
                }
                if (IsCanEdit && isUpdatePermission)
                {
                    imgUpdate.Visible = true;
                }
                else
                {
                    imgUpdate.Visible = false;
                }
                if (lblApprovedStatusValue.Text.Trim() == "Submit")
                {
                    lblApprovedStatusValue.Text = "Submitted";

                }
                else if (lblApprovedStatusValue.Text.Trim() == "Approved")
                {
                    imgApprove.Visible = false;
                    imgDetailsRequisition.Visible = false;
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                }
                //if (lblIsCanApprovedValue.Text.Trim() == "1")
                //{

                //    imgUpdate.Visible = isUpdatePermission;

                //    imgApprove.ImageUrl = "~/Images/approved.png";
                //    imgApprove.AlternateText = "Approve";
                //}
                //else
                //{
                //    imgDetailsRequisition.Visible = false;
                //    imgUpdate.Visible = false;
                //    imgDelete.Visible = false;

                //}
            }
        }
        protected void gvRequisitionInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvRequisitionInfo.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
        }
        protected void gvRequisitionInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            List<int> CheckByApproveByList = new List<int>();
            HMCommonDA commonDA = new HMCommonDA();
            try
            {
                if (e.CommandName == "CmdDelete")
                {
                    string TransactionNo = "";
                    string TransactionType = "";
                    string ApproveStatus = "";

                    PMRequisitionBO viewBo = new PMRequisitionBO();

                    int RequisitionId = Convert.ToInt32(e.CommandArgument.ToString());
                    string result = string.Empty;

                    PMRequisitionDA requsitionDa = new PMRequisitionDA();
                    bool status = false;
                    //bool status = requsitionDa.DeletePMRequisitionInfo(RequisitionId);


                    UserInformationBO userInformationBO = new UserInformationBO();

                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    viewBo = requsitionDa.GetPMRequisitionInfoByID(RequisitionId);


                    if (viewBo.CreatedBy == userInformationBO.UserInfoId && viewBo.ApprovedStatus == "Submit")
                    {
                        status = requsitionDa.DeletePMRequisitionInfo(viewBo.RequisitionId);
                        //statusType = "Hard Deleted";
                    }

                    else
                    {
                        status = requsitionDa.CancelPMRequisitionInfo(viewBo.RequisitionId, userInformationBO.UserInfoId, out TransactionNo, out TransactionType, out ApproveStatus);
                        if (status)
                        {
                            //prepare for sms send 

                            CommonMessageBO CommonMessage = new CommonMessageBO();
                            List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();
                            CheckByApproveByList = commonDA.GetCommonCheckByApproveByListForSMS("PMRequisition", "RequisitionId", RequisitionId.ToString(), "Requisition", "ApprovedStatus");

                            var str = "";
                            if (ApproveStatus == "Approved")
                            {
                                str += TransactionType + " No.(" + TransactionNo + ")  is Approved.";
                            }
                            else if (ApproveStatus == "Cancel")
                            {
                                str += TransactionType + " No.(" + TransactionNo + ")  is Canceled.";
                            }
                            else
                            {
                                str += TransactionType + " No.(" + TransactionNo + ") is waiting for your Approval Process.";
                            }
                            CommonMessage.Subjects = str;
                            CommonMessage.MessageBody = str;


                            if (CheckByApproveByList.Count > 0)
                            {

                                for (int i = 0; i < CheckByApproveByList.Count; i++)
                                {
                                    CommonMessageDetailsBO temp = new CommonMessageDetailsBO();
                                    temp.MessageTo = CheckByApproveByList[i];
                                    messageDetails.Add(temp);

                                }
                            }

                            SendMailByID(CommonMessage, messageDetails);
                        }
                    }

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.ProductRequisition.ToString(), RequisitionId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductRequisition));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Cancel, AlertType.Success);
                        LoadGridview();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                    this.SetTab("SearchTab");
                }
                else if (e.CommandName == "CmdReportRI")
                {
                    int RequisitionId = Convert.ToInt32(e.CommandArgument.ToString());
                    string url = "/PurchaseManagment/Reports/frmReportRequisitionOrderInvoice.aspx?RequisitionId=" + RequisitionId;
                    string s = "window.open('" + url + "', 'popup_window', 'width=800,height=680,left=300,top=50,resizable=yes');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
                else if (e.CommandName == "CmdApprove")
                {
                    //string[] commandArgs = e.CommandArgument.ToString().Split('|');
                    int RequisitionId = Convert.ToInt32(e.CommandArgument.ToString());
                    //Label lblApprovedStatusValue = //(Label)eRow.Row.FindControl("lblApprovedStatus");
                    //string ApprovalStatus = lblApprovedStatusValue.Text.Trim();// eRow.Row.DataItem;

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblApprovedStatusValue = (Label)row.Cells[4].FindControl("lblApprovedStatus");
                    string ApprovalStatus = "";

                    string TransactionNo;
                    string TransactionType;
                    string ApproveStatus;

                    ApprovalStatus = lblApprovedStatusValue.Text.Trim();
                    if (lblApprovedStatusValue.Text.Trim() == "Submitted" || lblApprovedStatusValue.Text.Trim() == "Partially Checked")
                    {
                        ApprovalStatus = "Checked";

                    }
                    else if (lblApprovedStatusValue.Text.Trim() == "Checked" || lblApprovedStatusValue.Text.Trim() == "Partially Approved")
                    {
                        ApprovalStatus = "Approved";
                    }
                    ReturnInfo rtninf = new ReturnInfo();
                    PurchaseOrderViewBO viewBo = new PurchaseOrderViewBO();
                    PMRequisitionDA requisitionDA = new PMRequisitionDA();

                    HMUtility hmUtility = new HMUtility();
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    rtninf.IsSuccess = requisitionDA.RequisitionOrderApproval(RequisitionId, ApprovalStatus, userInformationBO.UserInfoId, out TransactionNo, out TransactionType, out ApproveStatus);
                    //if (rtninf.IsSuccess)
                    //{
                    //    CheckByApproveByList = commonDA.GetCommonCheckByApproveByListForSMS("PMRequisition", "RequisitionId", RequisitionId.ToString(), "Requisition", "ApprovedStatus");
                    //}
                    if (!rtninf.IsSuccess)
                    {
                        rtninf.IsSuccess = false;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                    else
                    {
                        rtninf.IsSuccess = true;

                        ///
                        //prepare for sms send 

                        CommonMessageBO CommonMessage = new CommonMessageBO();
                        List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();
                        CheckByApproveByList = commonDA.GetCommonCheckByApproveByListForSMS("PMRequisition", "RequisitionId", RequisitionId.ToString(), "Requisition", "ApprovedStatus");

                        var str = "";
                        if (ApproveStatus == "Approved")
                        {
                            str += TransactionType + " No.(" + TransactionNo + ")  is Approved.";
                        }
                        else if (ApproveStatus == "Cancel")
                        {
                            str += TransactionType + " No.(" + TransactionNo + ")  is Canceled.";
                        }
                        else
                        {
                            str += TransactionType + " No.(" + TransactionNo + ") is waiting for your Approval Process.";
                        }
                        CommonMessage.Subjects = str;
                        CommonMessage.MessageBody = str;


                        if (CheckByApproveByList.Count > 0)
                        {

                            for (int i = 0; i < CheckByApproveByList.Count; i++)
                            {
                                CommonMessageDetailsBO temp = new CommonMessageDetailsBO();
                                temp.MessageTo = CheckByApproveByList[i];
                                messageDetails.Add(temp);

                            }
                        }

                        SendMailByID(CommonMessage, messageDetails);

                        ///

                        if (ApprovalStatus == HMConstants.ApprovalStatus.Checked.ToString())
                        {
                            //rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Checked, AlertType.Success);
                        }
                        else
                        {
                            // rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
                        }



                        LoadGridview();

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ApprovalStatus, EntityTypeEnum.EntityType.ProductRequisition.ToString(), RequisitionId,
                                   ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductRequisition));

                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);

            }
        }
        //************************ User Defined Function ********************//
        private void Cancel()
        {
            this.txtRequisitionBy.Text = string.Empty;
            this.txtRemarks.Text = string.Empty;
            this.txtReceivedByDate.Text = string.Empty;
            this.txtQuantity.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.txtQuantity.Focus();
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            return flag;
        }
        private void SetTab(string TabName)
        {
            if (TabName == "SearchTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.ItemRequisitionInformation.ToString());
            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void CheckPermission()
        {

            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }

        private void SendMailByID(CommonMessageBO CMB, List<CommonMessageDetailsBO> CMD)
        {
            bool status = false;
            //bool IsMessageSendAllGroupUser = false;
            HMUtility hmUtility = new HMUtility();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            CommonMessageBO message = new CommonMessageBO();
            List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();


            message = CMB;
            messageDetails = CMD;

            message.MessageFrom = userInformationBO.UserInfoId;
            message.MessageFromUserId = userInformationBO.UserId;
            message.MessageDate = DateTime.Now;
            message.Importance = "Normal";


            ReturnInfo info = new ReturnInfo();

            try
            {
                CommonMessageDA messageDa = new CommonMessageDA();
                status = messageDa.SaveMessageById(message, messageDetails);

                if (status)
                {
                    //info.IsSuccess = true;

                    //info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.MessageSent, AlertType.Success);
                    //info.Data = 0;

                    //CommonHelper.AlertInfo(innboardMessage, AlertMessage.MessageSent, AlertType.Success);


                }
                else
                {
                    //info.IsSuccess = false;
                    //info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                //info.IsSuccess = false;
                //info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);

            }

        }

        private void LoadGridview()
        {
            this.SetTab("SearchTab");
            PMRequisitionDA requisitionDA = new PMRequisitionDA();
            List<PMRequisitionBO> allrequisitionList = new List<PMRequisitionBO>();
            this.CheckObjectPermission();

            string startDate = string.Empty;
            string endDate = string.Empty;
            DateTime dateTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = this.txtFromDate.Text;
                this.txtFromDate.Text = startDate;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {

                endDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = this.txtToDate.Text;
                txtToDate.Text = startDate;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int companyId = 0, projectId = 0;

            string companyName = string.Empty;
            string projectName = string.Empty;

            if (hfSrcCompanyId.Value != "0" && hfSrcCompanyId.Value != "")
            {
                companyId = Convert.ToInt32(hfSrcCompanyId.Value);
                //companyName = ddlSrcGLCompany.SelectedItem.Text;
            }

            if (hfSrcProjectId.Value != "0" && hfSrcProjectId.Value != "")
            {
                projectId = Convert.ToInt32(hfSrcProjectId.Value);
                //projectName = hfSrcProjectId.Value != "0" ? ddlSrcGLProject.SelectedItem.Text : "All";
            }

            if (this.ddlDateType.SelectedIndex == 0)
            {
                allrequisitionList = requisitionDA.GetPMRequisitionInfoBySearchCriteriaWithUser(FromDate, ToDate, this.ddlStatus.SelectedItem.Text, "Pending", userInformationBO.UserInfoId);
            }
            else if (this.ddlDateType.SelectedIndex == 1)
            {
                allrequisitionList = requisitionDA.GetPMRequisitionInfoBySearchCriteriaWithUser(FromDate, ToDate, this.ddlStatus.SelectedItem.Text, "CreatedDate", userInformationBO.UserInfoId);
            }
            else if (this.ddlDateType.SelectedIndex == 2)
            {
                allrequisitionList = requisitionDA.GetPMRequisitionInfoBySearchCriteriaWithUser(FromDate, ToDate, this.ddlStatus.SelectedItem.Text, "ReceivedDate", userInformationBO.UserInfoId);
            }
            else if (this.ddlDateType.SelectedIndex == 3)
            {
                allrequisitionList = requisitionDA.GetPMRequisitionInfoBySearchCriteriaWithUser(FromDate, ToDate, this.ddlStatus.SelectedItem.Text, "CompanyProject", userInformationBO.UserInfoId, companyId, projectId);
            }

            gvRequisitionInfo.DataSource = allrequisitionList;
            gvRequisitionInfo.DataBind();
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadAllCostCentreTabInfo()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            List<CostCentreTabBO> requisitionToCostCentreList = new List<CostCentreTabBO>();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
            invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsUserpermissionAppliedToCostcenterFilteringForPOPR", "IsUserpermissionAppliedToCostcenterFilteringForPOPR");

            //if (Convert.ToInt16(invoiceTemplateBO.SetupValue) != 0)
            //{
            //    costCentreTabBOList = costCentreTabDA.GetUserWisePRPOCostCentreTabInfo(userInformationBO.UserInfoId, HMConstants.CostcenterFilteringForPOPR.Requisition.ToString());
            //}
            //else
            //{
            //    costCentreTabBOList = costCentreTabDA.GetUserWisePRPOCostCentreTabInfo(userInformationBO.UserInfoId, null);
            //}

            costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId)
                                    .Where(o => o.OutletType == 1 && o.CostCenterType == "Inventory").ToList();

            this.ddlCostCentre.DataSource = costCentreTabBOList;
            this.ddlCostCentre.DataTextField = "CostCenter";
            this.ddlCostCentre.DataValueField = "CostCenterId";
            this.ddlCostCentre.DataBind();

            ddlLocation.DataSource = costCentreTabBOList;
            ddlLocation.DataTextField = "CostCenterId";
            ddlLocation.DataValueField = "DefaultStockLocationId";
            ddlLocation.DataBind();

            requisitionToCostCentreList = costCentreTabDA.GetCostCentreTabInfoByType("Inventory");

            ddlRequisitionTo.DataSource = requisitionToCostCentreList;
            ddlRequisitionTo.DataTextField = "CostCenter";
            ddlRequisitionTo.DataValueField = "CostCenterId";
            ddlRequisitionTo.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            if (requisitionToCostCentreList.Count > 1)
                ddlRequisitionTo.Items.Insert(0, item); 
            if (costCentreTabBOList.Count > 1)
            { 
                ddlCostCentre.Items.Insert(0, item);
                ddlLocation.Items.Insert(0, item);
            }
        }
        private void LoadStockBy()
        {
            List<InvUnitHeadBO> headListBO = new List<InvUnitHeadBO>();
            InvUnitHeadDA da = new InvUnitHeadDA();
            headListBO = da.GetInvUnitHeadInfo();

            this.ddlStockBy.DataSource = headListBO;
            this.ddlStockBy.DataTextField = "HeadName";
            this.ddlStockBy.DataValueField = "UnitHeadId";
            this.ddlStockBy.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlStockBy.Items.Insert(0, item);
        }

        private void LoadIsItemAttributeEnable()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            List<HMCommonSetupBO> setUpBOList = new List<HMCommonSetupBO>();
            setUpBOList = commonSetupDA.GetAllCommonConfigurationInfo();


            setUpBO = setUpBOList.Where(x => x.TypeName == "IsItemAttributeEnable" && x.SetupName == "IsItemAttributeEnable").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsItemAttributeEnable.Value = setUpBO.SetupValue;
            }
        }
        private void LoadUserInformation()
        {
            //int checkedByUserInfoId = 0;
            //int approvedByUserInfoId = 0;
            //HMCommonSetupBO commonSetupBORequisition = new HMCommonSetupBO();
            //HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            //commonSetupBORequisition = commonSetupDA.GetCommonConfigurationInfo("IsRequisitionCheckedByEnable", "IsRequisitionCheckedByEnable");

            //if (commonSetupBORequisition != null)
            //{
            //    if (commonSetupBORequisition.SetupId > 0)
            //    {
            //        if (commonSetupBORequisition.SetupValue == "1")
            //        {
            //            checkedByUserInfoId = !string.IsNullOrWhiteSpace(commonSetupBORequisition.Description.Split('~')[0].ToString()) ? Convert.ToInt32(commonSetupBORequisition.Description.Split('~')[0].ToString()) : 0;
            //            approvedByUserInfoId = !string.IsNullOrWhiteSpace(commonSetupBORequisition.Description.Split('~')[1].ToString()) ? Convert.ToInt32(commonSetupBORequisition.Description.Split('~')[1].ToString()) : 0;
            //        }
            //    }
            //}


        }
        //private void LoadGLCompany(bool isSingle)
        //{
        //    hfIsSingle.Value = "0";
        //    GLCompanyDA entityDA = new GLCompanyDA();
        //    var List = entityDA.GetAllGLCompanyInfo();

        //    hfCompanyAll.Value = JsonConvert.SerializeObject(List);

        //    List<GLCompanyBO> companyList = new List<GLCompanyBO>();
        //    if (List.Count == 1)
        //    {
        //        companyList.Add(List[0]);
        //        ddlGLCompany.DataSource = companyList;
        //        ddlGLCompany.DataTextField = "Name";
        //        ddlGLCompany.DataValueField = "CompanyId";
        //        ddlGLCompany.DataBind();

        //        ddlSrcGLCompany.DataSource = companyList;
        //        ddlSrcGLCompany.DataTextField = "Name";
        //        ddlSrcGLCompany.DataValueField = "CompanyId";
        //        ddlSrcGLCompany.DataBind();

        //        LoadGLProjectByCompany(companyList[0].CompanyId);

        //    }
        //    else
        //    {
        //        ddlGLCompany.DataSource = List;
        //        ddlGLCompany.DataTextField = "Name";
        //        ddlGLCompany.DataValueField = "CompanyId";
        //        ddlGLCompany.DataBind();

        //        ddlSrcGLCompany.DataSource = List;
        //        ddlSrcGLCompany.DataTextField = "Name";
        //        ddlSrcGLCompany.DataValueField = "CompanyId";
        //        ddlSrcGLCompany.DataBind();

        //        ListItem itemCompany = new ListItem();
        //        itemCompany.Value = "0";
        //        itemCompany.Text = hmUtility.GetDropDownFirstValue();
        //        ddlGLCompany.Items.Insert(0, itemCompany);
        //        ddlSrcGLCompany.Items.Insert(0, itemCompany);
        //    }


        //}
        //private void LoadGLProjectByCompany(int companyId)
        //{
        //    GLProjectDA entityDA = new GLProjectDA();
        //    List<GLProjectBO> projectList = new List<GLProjectBO>();
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
        //    var List = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(Convert.ToInt32(companyId), Convert.ToInt32(userInformationBO.UserGroupId)).Where(x => x.IsFinalStage == false).ToList();


        //    ddlGLProject.DataSource = List;
        //    ddlGLProject.DataTextField = "Name";
        //    ddlGLProject.DataValueField = "ProjectId";
        //    ddlGLProject.DataBind();

        //    ddlSrcGLProject.DataSource = List;
        //    ddlSrcGLProject.DataTextField = "Name";
        //    ddlSrcGLProject.DataValueField = "ProjectId";
        //    ddlSrcGLProject.DataBind();

        //    if (List.Count > 1)
        //    {
        //        isSingle = false;
        //        hfIsSingle.Value = "0";
        //        ListItem itemProject = new ListItem();
        //        itemProject.Value = "0";
        //        itemProject.Text = hmUtility.GetDropDownFirstValue();
        //        ddlGLProject.Items.Insert(0, itemProject);

        //        ListItem itemSrcProject = new ListItem();
        //        itemSrcProject.Value = "0";
        //        itemSrcProject.Text = hmUtility.GetDropDownFirstAllValue();
        //        ddlSrcGLProject.Items.Insert(0, itemSrcProject);
        //    }
        //    else
        //    {
        //        hfIsSingle.Value = "1";
        //    }

        //}
        //private void LoadGLProject(bool isSingle)
        //{
        //    GLProjectDA entityDA = new GLProjectDA();
        //    List<GLProjectBO> projectList = new List<GLProjectBO>();
        //    var List = entityDA.GetAllGLProjectInfo();
        //    if (isSingle == true)
        //    {
        //        projectList.Add(List[0]);
        //        ddlGLProject.DataSource = projectList;
        //        ddlGLProject.DataTextField = "Name";
        //        ddlGLProject.DataValueField = "ProjectId";
        //        ddlGLProject.DataBind();

        //        ddlSrcGLProject.DataSource = List;
        //        ddlSrcGLProject.DataTextField = "Name";
        //        ddlSrcGLProject.DataValueField = "ProjectId";
        //        ddlSrcGLProject.DataBind();
        //    }
        //    else
        //    {
        //        ddlGLProject.DataSource = List;
        //        ddlGLProject.DataTextField = "Name";
        //        ddlGLProject.DataValueField = "ProjectId";
        //        ddlGLProject.DataBind();

        //        ddlSrcGLProject.DataSource = List;
        //        ddlSrcGLProject.DataTextField = "Name";
        //        ddlSrcGLProject.DataValueField = "ProjectId";
        //        ddlSrcGLProject.DataBind();

        //        ListItem itemProject = new ListItem();
        //        itemProject.Value = "0";
        //        itemProject.Text = hmUtility.GetDropDownFirstAllValue();
        //        ddlGLProject.Items.Insert(0, itemProject);
        //        ddlSrcGLProject.Items.Insert(0, itemProject);
        //    }
        //    SingleprojectId.Value = List[0].ProjectId.ToString();
        //}
        //private void LoadGLProject(string companyId)
        //{
        //    HMUtility hmUtility = new HMUtility();
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();


        //    GLProjectDA entityDA = new GLProjectDA();
        //    ddlGLProject.DataSource = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(Convert.ToInt32(companyId), Convert.ToInt32(userInformationBO.UserGroupId));
        //    ddlGLProject.DataTextField = "Name";
        //    ddlGLProject.DataValueField = "ProjectId";
        //    ddlGLProject.DataBind();

        //    ListItem itemProject = new ListItem();
        //    itemProject.Value = "0";
        //    itemProject.Text = hmUtility.GetDropDownFirstAllValue();
        //    ddlGLProject.Items.Insert(0, itemProject);
        //}
        private void IsInventoryIntegrateWithAccounts()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isInventoryIntegrateWithAccountsBO = new HMCommonSetupBO();
            isInventoryIntegrateWithAccountsBO = commonSetupDA.GetCommonConfigurationInfo("IsInventoryIntegrateWithAccounts", "IsInventoryIntegrateWithAccounts");

            if (isInventoryIntegrateWithAccountsBO != null)
            {
                //if (isInventoryIntegrateWithAccountsBO.SetupValue == "1")
                //{
                //    //LoadGLCompany(false);
                //}
                //else
                //    LoadDefaultGLCompanyNProject();
            }
        }
        //private void LoadDefaultGLCompanyNProject()
        //{
        //    GLCompanyDA entityDA = new GLCompanyDA();
        //    List<GLCompanyBO> companyList = new List<GLCompanyBO>();
        //    companyList.Add(entityDA.GetAllGLCompanyInfo().FirstOrDefault());

        //    ddlGLCompany.DataSource = companyList;
        //    ddlGLCompany.DataTextField = "Name";
        //    ddlGLCompany.DataValueField = "CompanyId";
        //    ddlGLCompany.DataBind();

        //    hfIsSingle.Value = "1";
        //    if (companyList.Count > 0)
        //    {
        //        GLProjectDA projectDA = new GLProjectDA();
        //        List<GLProjectBO> projectList = new List<GLProjectBO>();
        //        projectList.Add(projectDA.GetGLProjectInfoByGLCompany(companyList[0].CompanyId).FirstOrDefault());

        //        ddlGLProject.DataSource = projectList;
        //        ddlGLProject.DataTextField = "Name";
        //        ddlGLProject.DataValueField = "ProjectId";
        //        ddlGLProject.DataBind();
        //    }

        //}
        //************************ Web Method Function ********************//
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
        public static RequisitionViewBO FillForm(int requisitionId)
        {
            PMRequisitionDA requisitionDA = new PMRequisitionDA();
            RequisitionViewBO viewBo = new RequisitionViewBO();

            viewBo.Requisition = requisitionDA.GetPMRequisitionInfoByID(requisitionId);
            viewBo.RequisitionDetails = requisitionDA.GetPMRequisitionDetailsByID(requisitionId);

            return viewBo;
        }
        [WebMethod]
        public static ReturnInfo SaveRequsition(int requsitionId, PMRequisitionBO requsition,
            List<PMRequisitionDetailsBO> AddedItem, List<PMRequisitionDetailsBO> EditedItem,
            List<PMRequisitionDetailsBO> DeletedItem)
        {
            ReturnInfo rtninf = new ReturnInfo();
            int requsitionNewId = 0;
            string requisitionNumber = string.Empty;
            bool status = false;
            string TransactionNo;
            string TransactionType;
            string ApproveStatus;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                List<int> CheckByApproveByList = new List<int>();
                PMRequisitionDA requisitionDA = new PMRequisitionDA();
                HMCommonDA commonDA = new HMCommonDA();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsRequisitionApprovalEnable", "IsRequisitionApprovalEnable");

                if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.ProductReceiveTemplate.ApprovalEnable))
                {
                    requsition.ApprovedStatus = HMConstants.ApprovalStatus.Submit.ToString();
                }
                else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.ProductReceiveTemplate.ApprovalDisable))
                {
                    requsition.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();
                }

                requsition.RequisitionBy = userInformationBO.UserName;

                if (requsitionId == 0)
                {
                    requsition.CreatedBy = userInformationBO.UserInfoId;
                    status = requisitionDA.SavePMRequisitionInfo(requsition, AddedItem, out requsitionNewId, out requisitionNumber, out TransactionNo, out TransactionType, out ApproveStatus);
                    if (status)
                    {
                        CheckByApproveByList = commonDA.GetCommonCheckByApproveByListForSMS("PMRequisition", "RequisitionId", requsitionNewId.ToString(), "Requisition", "ApprovedStatus");
                    }
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.ProductRequisition.ToString(), requsitionNewId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductRequisition));
                        rtninf.IsSuccess = true;
                        rtninf.PrimaryKeyValue = requsitionNewId.ToString();
                        rtninf.TransactionNo = TransactionNo;
                        rtninf.TransactionType = TransactionType;
                        rtninf.TransactionStatus = ApproveStatus;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }
                else
                {
                    requsition.LastModifiedBy = userInformationBO.UserInfoId;
                    status = requisitionDA.UpdatePMRequisitionInfo(requsition, AddedItem, EditedItem, DeletedItem, out TransactionNo, out TransactionType, out ApproveStatus);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.ProductRequisition.ToString(), requsitionId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductRequisition));
                        rtninf.IsSuccess = true;
                        rtninf.PrimaryKeyValue = requsitionId.ToString();
                        rtninf.TransactionNo = TransactionNo;
                        rtninf.TransactionType = TransactionType;
                        rtninf.TransactionStatus = ApproveStatus;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }

                //HMCommonDA commonDa = new HMCommonDA();
                //CustomFieldBO customFieldObject = new CustomFieldBO();
                //customFieldObject = commonDa.GetCustomFieldByFieldName("ItemRequisitionApprovedByEmail");

                //if (customFieldObject != null)
                //{
                //    var req = requisitionDA.GetPMRequisitionInfoByID(requsition.RequisitionId);
                //    EmailHelper.SendEmail(string.Empty, customFieldObject.FieldValue.ToString(), "Approval Pending For Requisition No " + req.PRNumber,
                //        "Please Approved The Requisition.", string.Empty);
                //}

                if (!status)
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
        public static List<PMRequisitionDetailsBO> RequisitionDetails(int requisitionId)
        {
            PMRequisitionDA requisitionDA = new PMRequisitionDA();
            List<PMRequisitionDetailsBO> requisitionDetails = new List<PMRequisitionDetailsBO>();
            requisitionDetails = requisitionDA.GetPMRequisitionDetailsByID(requisitionId);

            return requisitionDetails;
        }
        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemSearch(string searchTerm, int companyId, int projectId, int costCenterId, int categoryId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();
            InvItemDA itemDa = new InvItemDA();
            //itemInfo = itemDa.GetItemDetailsForAutoSearch(searchTerm, costCenterId, ConstantHelper.CustomerSupplierAutoSearch.SupplierItem.ToString(), categoryId, 0);
            itemInfo = itemDa.GetItemForAutoSearchWithoutSupplier("Requisition", searchTerm, companyId, projectId, costCenterId, ConstantHelper.CustomerSupplierAutoSearch.SupplierItem.ToString(), categoryId);

            return itemInfo;
        }
        [WebMethod]
        public static List<InvCategoryBO> LoadCategoryFromCostCenter(int costCenetrId)
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            //List = da.GetAllInvItemCatagoryInfoByServiceType("All");
            if (costCenetrId == 0)
            {
                List = da.GetTPInvCategoryInfoByCustomString("WHERE ic.ActiveStat = 1 AND ic.ServiceType <> 'Service'");
            }
            else
            {
                List = da.GetTPInvCategoryInfoByCustomString("WHERE ic.ActiveStat = 1 AND ic.ServiceType <> 'Service' AND ccc.CostCenterId = " + costCenetrId.ToString());
            }

            return List;
        }
        [WebMethod]
        public static List<InvUnitHeadBO> LoadRelatedStockBy(int stockById)
        {
            InvUnitHeadDA unitHeadDA = new InvUnitHeadDA();
            List<InvUnitHeadBO> unitHeadList = new List<InvUnitHeadBO>();
            unitHeadList = unitHeadDA.GetRelatedStockBy(stockById);
            if (unitHeadList.Count > 0)
            {
                unitHeadList = unitHeadList.Where(i => i.UnitHeadId == stockById).ToList();
            }

            return unitHeadList;
        }

        [WebMethod]
        public static List<InvItemAttributeBO> GetInvItemAttributeByItemIdAndAttributeType(int ItemId, string attributeType)
        {
            InvItemAttributeDA DA = new InvItemAttributeDA();
            List<InvItemAttributeBO> InvItemAttributeBOList = new List<InvItemAttributeBO>();
            InvItemAttributeBOList = DA.GetInvItemAttributeByItemIdAndAttributeType(ItemId, attributeType);
            

            return InvItemAttributeBOList;
        }



        [WebMethod]
        public static InvItemStockInformationBO LoadCurrentStockQuantity(int costcenterId, int locationId,
            int itemId, int projectId)
        {
            InvItemDA mappingDA = new InvItemDA();
            InvItemStockInformationBO stockInfo = new InvItemStockInformationBO();

            stockInfo = mappingDA.GetInvItemStockInfoByItemLocationNProject(itemId, locationId, projectId);

            return stockInfo;
        }

        [WebMethod]
        public static InvItemStockInformationBO GetInvItemStockInfoByItemAndAttributeId(int itemId, int colorId, int sizeId, int styleId, int locationId)
        {
            InvItemDA DA = new InvItemDA();
            InvItemStockInformationBO StockInformation = new InvItemStockInformationBO();
            StockInformation = DA.GetInvItemStockInfoByItemAndAttributeId(itemId, colorId, sizeId, styleId, locationId);

            return StockInformation;
        }

        [WebMethod]
        public static List<InvLocationBO> InvLocationByCostCenter(int costCenterId)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvItemLocationByCostCenter(costCenterId);

            return location;
        }
    }
}