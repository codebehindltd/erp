using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.GeneralLedger;
using System.Web.Services;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.Collections;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.UserInformation;
using Newtonsoft.Json;


namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmVoucher : System.Web.UI.Page
    {
        ArrayList arrayDelete;
        HiddenField innboardMessage;
        Int64 ConfigureAccountHeadId = 0;
        int ReceivePaymentLedgerId = 0;
        protected bool isSingle = true;        
        protected int isProjectDdlEnable = -1;
        protected int isEnableTransectionMode = -1;
        HMUtility hmUtility = new HMUtility();
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        protected int isReceivedOrPaymentVoucher = 1;
        protected int isCompanyProjectPanelEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            isSingle = hmUtility.GetSingleProjectAndCompany();
            this.AddEditODeleteDetail();
            this.txtIsCheckFirstTimeValidation.Value = "1";
            if (!IsPostBack)
            {
                TotalCalculateDebitCreditAmount.Visible = false;
                this.txtIsCheckFirstTimeValidation.Value = "0";
                this.hfChequeNumberForConfigureAccountHead.Value = string.Empty;
                this.txtGoToScrolling.Text = "EntryPanel";
                this.LoadCurrentDate();

                if (isSingle == true)
                {
                    isCompanyProjectPanelEnable = 1;
                    LoadSingleProjectAndCompany();
                }
                else
                {
                    this.LoadGLCompany(false);
                    this.LoadGLProject(false);
                }

                //this.LoadGLCompany();
                //this.LoadGLProject();
                this.LoadCurrency();
                this.LoadUserInformation();
                Session["DetailList"] = null;
                this.LoadAccountHead();
                string DealId = Request.QueryString["DealId"];
                if (!String.IsNullOrEmpty(DealId))
                {
                    FillVoucherForm(Int32.Parse(DealId));
                }
            }
            this.CheckObjectPermission();

            if (isReceivedOrPaymentVoucher > -1)
            {
                if (this.ddlConfigureAccountHead.SelectedValue != "0")
                {
                    this.lblConfigureAccountHeadText.Text = this.ddlConfigureAccountHead.SelectedItem.Text;
                }
            }
        }
        protected void gvRoomOwnerDtail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdDelete")
            {
                int _detailId = Convert.ToInt32(e.CommandArgument.ToString());
                var DetailBO = (List<GLLedgerBO>)Session["DetailList"];
                var ownerDetail = DetailBO.Where(x => x.LedgerId == _detailId).FirstOrDefault();
                DetailBO.Remove(ownerDetail);
                Session["DetailList"] = DetailBO;
                arrayDelete.Add(_detailId);
                this.gvDetail.DataSource = Session["DetailList"] as List<GLLedgerBO>;
                this.gvDetail.DataBind();
                this.CalculateotalDetailAmountT();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                return;
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GLDealMasterDA glMasterDA = new GLDealMasterDA();
            GLDealMasterBO glMasterBO = new GLDealMasterBO();

            glMasterBO.ProjectId = Convert.ToInt32(this.ddlGLProject.SelectedValue);
            glMasterBO.VoucherType = this.ddlVoucherType.SelectedValue.ToString();
            glMasterBO.VoucherMode = this.ddlVoucherMode.SelectedIndex;
            glMasterBO.CashChequeMode = this.ddlCashChequeMode.SelectedIndex;
            glMasterBO.VoucherNo = this.txtVoucherNo.Text;
            glMasterBO.VoucherDate = hmUtility.GetDateTimeFromString(this.txtVoucherDate.Text, userInformationBO.ServerDateFormat);
            glMasterBO.PayerOrPayee = this.txtPayerOrPayee.Text;
            glMasterBO.Narration = this.txtNarration.Text;
            glMasterBO.CheckedBy = Convert.ToInt32(this.ddlCheckedBy.SelectedValue);
            glMasterBO.ApprovedBy = Convert.ToInt32(this.ddlApprovedBy.SelectedValue);
            
            // CheckedBy and ApprovedBy Information --------------------
            List<GLVoucherApprovedInfoBO> approvedBOList = new List<GLVoucherApprovedInfoBO>();
            
            // CheckedBy -----------------
            GLVoucherApprovedInfoBO approvedBOCheckedBy = new GLVoucherApprovedInfoBO();
            if (this.ddlCheckedBy.SelectedValue != "0")
            {
                approvedBOCheckedBy.ApprovedType = "CheckedBy";
                approvedBOCheckedBy.UserInfoId = Convert.ToInt32(this.ddlCheckedBy.SelectedValue);
                approvedBOList.Add(approvedBOCheckedBy);
                //glMasterBO.GLStatus = string.Empty;
            }
            else
            {
                approvedBOCheckedBy.ApprovedType = "CheckedBy";
                approvedBOCheckedBy.UserInfoId = Convert.ToInt32(this.ddlCheckedBy.SelectedValue);
                approvedBOList.Add(approvedBOCheckedBy);
                glMasterBO.GLStatus = "Checked";
            }
            
            // ApprovedBy -----------------
            if (this.ddlApprovedBy.SelectedValue != "0")
            {
                GLVoucherApprovedInfoBO approvedBOApprovedBy = new GLVoucherApprovedInfoBO();
                approvedBOApprovedBy.ApprovedType = "ApprovedBy";
                approvedBOApprovedBy.UserInfoId = Convert.ToInt32(this.ddlApprovedBy.SelectedValue);
                approvedBOList.Add(approvedBOApprovedBy);
            }

            if (this.btnSave.Text.Equals("Save"))
            {
                int tmpGLMasterId = 0;
                string currentVoucherNo = string.Empty;
                glMasterBO.CreatedBy = userInformationBO.UserInfoId;
                //Boolean status = glMasterDA.SaveGLMasterInfo(glMasterBO, out tmpGLMasterId, out currentVoucherNo, Session["DetailList"] as List<GLLedgerBO>, approvedBOList);
                Boolean status = glMasterDA.SaveGLMasterInfo(glMasterBO, out tmpGLMasterId, out currentVoucherNo, Session["DetailList"] as List<GLLedgerBO>, approvedBOList);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Voucher Number : '" + currentVoucherNo + "' Saved Successfully.", AlertType.Success);                    
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GeneralLedgerVoucher.ToString(), tmpGLMasterId,
                    ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GeneralLedgerVoucher));
                    this.Cancel();
                }
            }
            else
            {
                glMasterBO.DealId = Int32.Parse(txtDealId.Value);
                glMasterBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = glMasterDA.UpdateGLMasterInfo(glMasterBO, Session["DetailList"] as List<GLLedgerBO>, Session["arrayDelete"] as ArrayList, approvedBOList);
                //Boolean status = glMasterDA.UpdateGLMasterInfoWithApprovedInfo(glMasterBO, Session["DetailList"] as List<GLLedgerBO>, Session["arrayDelete"] as ArrayList, approvedBOList);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GeneralLedgerVoucher.ToString(), glMasterBO.DealId,
                        ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GeneralLedgerVoucher));
                    //Response.Redirect("/GeneralLedger/frmVoucherSearch.aspx");
                    btnSave.Text = "Save";
                    txtDealId.Value = "";
                    this.Cancel();
                }
            }
        }
        protected void btnAddDetail_Click(object sender, EventArgs e)
        {
            if (this.ddlVoucherType.SelectedValue == "BP")
            {
                if (string.IsNullOrWhiteSpace(hfChequeNumberForConfigureAccountHead.Value))
                {
                    hfChequeNumberForConfigureAccountHead.Value = this.txtChequeNumber.Text;
                }
            }
            else if (this.ddlVoucherType.SelectedValue == "BR")
            {
                if (string.IsNullOrWhiteSpace(hfChequeNumberForConfigureAccountHead.Value))
                {
                    hfChequeNumberForConfigureAccountHead.Value = this.txtChequeNumber.Text;
                }
            }
            else if (this.ddlVoucherType.SelectedValue == "CV")
            {
                if (string.IsNullOrWhiteSpace(hfChequeNumberForConfigureAccountHead.Value))
                {
                    hfChequeNumberForConfigureAccountHead.Value = this.txtJVChequeNumber.Text;
                }
            }
            else if (this.ddlVoucherType.SelectedValue == "JV")
            {
                if (string.IsNullOrWhiteSpace(hfChequeNumberForConfigureAccountHead.Value))
                {
                    hfChequeNumberForConfigureAccountHead.Value = this.txtJVChequeNumber.Text;
                }
            }
            if (string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Ledger Amount.", AlertType.Warning);
                this.txtLedgerAmount.Focus();
                return;
            }
            else if (Convert.ToDecimal(this.txtLedgerAmount.Text) <= 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Ledger Amount.", AlertType.Warning);
                this.txtLedgerAmount.Focus();
                return;
            }
            if (this.ddlCurrency.SelectedIndex != 0)
            {
                if (string.IsNullOrWhiteSpace(this.txtConversionRate.Text))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Conversion Rate.", AlertType.Warning);
                    this.txtConversionRate.Focus();
                    return;
                }
            }
            if (this.ddlVoucherType.SelectedValue != "None")
            {
                if (this.ddlNodeId.SelectedValue != "0")
                {
                    int voucherType = 1;
                    if (this.ddlVoucherType.SelectedValue == "JV")
                    {
                        voucherType = 0;
                        isReceivedOrPaymentVoucher = -1;
                        isEnableTransectionMode = 1;

                        if (this.ddlLedgerMode.SelectedIndex == 0)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Transaction Mode.", AlertType.Warning);
                            this.ddlLedgerMode.Focus();
                            return;
                        }
                    }
                    if (this.ddlVoucherType.SelectedValue == "CV")
                    {
                        voucherType = 0;
                        isReceivedOrPaymentVoucher = -1;
                        isEnableTransectionMode = 1;

                        if (this.ddlLedgerMode.SelectedIndex == 0)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Transaction Mode.", AlertType.Warning);
                            this.ddlLedgerMode.Focus();
                            return;
                        }
                    }

                    int dynamicDetailId = 0;
                    List<GLLedgerBO> reservationDetailListBO = Session["DetailList"] == null ? new List<GLLedgerBO>() : Session["DetailList"] as List<GLLedgerBO>;

                    if (!string.IsNullOrWhiteSpace(lblHiddenId.Text))
                        dynamicDetailId = Convert.ToInt32(lblHiddenId.Text);

                    GLLedgerBO detailBO = dynamicDetailId == 0 ? new GLLedgerBO() : reservationDetailListBO.Where(x => x.LedgerId == dynamicDetailId).FirstOrDefault();
                    if (reservationDetailListBO.Contains(detailBO))
                        reservationDetailListBO.Remove(detailBO);

                    detailBO.NodeId = Convert.ToInt32(this.ddlNodeId.SelectedValue);
                    detailBO.NodeHead = this.ddlNodeId.SelectedItem.Text;
                    if (voucherType == 0)
                    {
                        detailBO.LedgerMode = this.ddlLedgerMode.SelectedIndex;
                        if (this.ddlVoucherType.SelectedValue == "CV")
                        {
                            detailBO.ChequeNumber = this.txtJVChequeNumber.Text;
                        }
                        else if (this.ddlVoucherType.SelectedValue == "JV")
                        {
                            detailBO.ChequeNumber = this.txtJVChequeNumber.Text;
                        }
                    }
                    else
                    {
                        if (this.ddlVoucherType.SelectedValue == "CP" || this.ddlVoucherType.SelectedValue == "BP")
                        {
                            detailBO.LedgerMode = 1;
                            this.ddlLedgerMode.SelectedIndex = 1;
                            detailBO.ChequeNumber = string.Empty;
                            if (this.ddlVoucherType.SelectedValue == "BP")
                            {
                                detailBO.ChequeNumber = this.txtChequeNumber.Text;
                            }
                        }
                        else if (this.ddlVoucherType.SelectedValue == "CR" || this.ddlVoucherType.SelectedValue == "BR")
                        {
                            detailBO.LedgerMode = 2;
                            this.ddlLedgerMode.SelectedIndex = 2;
                            detailBO.ChequeNumber = string.Empty;
                            if (this.ddlVoucherType.SelectedValue == "BR")
                            {
                                detailBO.ChequeNumber = this.txtChequeNumber.Text;
                            }
                        }
                        else if (this.ddlVoucherType.SelectedValue == "CV")
                        {
                            detailBO.ChequeNumber = this.txtJVChequeNumber.Text;
                        }
                        else if (this.ddlVoucherType.SelectedValue == "JV")
                        {
                            detailBO.ChequeNumber = this.txtJVChequeNumber.Text;
                        }
                    }

                    detailBO.FieldId = Convert.ToInt32(this.ddlCurrency.SelectedValue);

                    if (this.ddlCurrency.SelectedIndex == 0)
                    {
                        if (this.ddlLedgerMode.SelectedIndex == 1)
                        {
                            detailBO.LedgerDebitAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                        }
                        else
                        {
                            detailBO.LedgerCreditAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                        }
                        detailBO.CurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                    }
                    else
                    {
                        detailBO.CurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                        decimal lAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                        decimal crAmount = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 0;
                        this.txtLedgerAmount.Text = (lAmount * crAmount).ToString();

                        if (this.ddlLedgerMode.SelectedIndex == 1)
                        {
                            detailBO.LedgerDebitAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                        }
                        else
                        {
                            detailBO.LedgerCreditAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                        }
                    }

                    detailBO.LedgerAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;

                    detailBO.NodeNarration = this.txtNodeNarration.Text;
                    if (this.btnSave.Text.Equals("Update"))
                    {
                        detailBO.LedgerId = 0;
                    }
                    else
                    {
                        detailBO.LedgerId = dynamicDetailId == 0 ? reservationDetailListBO.Count + 1 : dynamicDetailId;
                    }
                    reservationDetailListBO.Add(detailBO);
                    Session["DetailList"] = reservationDetailListBO;

                    this.gvDetail.DataSource = Session["DetailList"] as List<GLLedgerBO>;
                    this.gvDetail.DataBind();

                    //---For Payment or Received Voucher----------------------------------------------------------------
                    if (voucherType != 0)
                    {
                        if (this.btnSave.Text.Equals("Update"))
                        {
                            var accountDetailListBO = (List<GLLedgerBO>)Session["DetailList"];
                            if (this.ddlVoucherType.SelectedValue == "CP" || this.ddlVoucherType.SelectedValue == "BP")
                            {
                                ConfigureAccountHeadId = accountDetailListBO.Where(x => x.LedgerMode == 2).FirstOrDefault().NodeId;
                                ReceivePaymentLedgerId = accountDetailListBO.Where(x => x.LedgerMode == 2).FirstOrDefault().LedgerId;
                                if (ConfigureAccountHeadId.ToString() == this.ddlConfigureAccountHead.SelectedValue)
                                {
                                    this.ddlConfigureAccountHead.SelectedValue = ConfigureAccountHeadId.ToString();
                                }
                                var deleteAccountDetail = accountDetailListBO.Where(x => x.LedgerMode == 2).FirstOrDefault();
                                accountDetailListBO.Remove(deleteAccountDetail);
                                Session["ReceivePaymentLedgerId"] = ReceivePaymentLedgerId;
                            }
                            else if (this.ddlVoucherType.SelectedValue == "CR" || this.ddlVoucherType.SelectedValue == "BR")
                            {
                                ConfigureAccountHeadId = accountDetailListBO.Where(x => x.LedgerMode == 1).FirstOrDefault().NodeId;
                                ReceivePaymentLedgerId = accountDetailListBO.Where(x => x.LedgerMode == 1).FirstOrDefault().LedgerId;
                                if (ConfigureAccountHeadId.ToString() == this.ddlConfigureAccountHead.SelectedValue)
                                {
                                    this.ddlConfigureAccountHead.SelectedValue = ConfigureAccountHeadId.ToString();
                                }
                                var deleteAccountDetail = accountDetailListBO.Where(x => x.LedgerMode == 1).FirstOrDefault();
                                accountDetailListBO.Remove(deleteAccountDetail);
                                Session["ReceivePaymentLedgerId"] = ReceivePaymentLedgerId;
                            }

                            this.SelectedNodeId.Value = string.Empty;

                            if (accountDetailListBO.Count == 1)
                            {
                                this.SelectedNodeId.Value = string.Empty;
                            }
                            else
                            {
                                Session["DetailList"] = accountDetailListBO;
                            }
                        }
                        else
                        {
                            string selectedNodeId = hfConfigureAccountHead.Value;
                            this.ddlConfigureAccountHead.SelectedValue = selectedNodeId;
                            ConfigureAccountHeadId = Convert.ToInt32(selectedNodeId);
                        }

                        decimal tmpLedgerAmount = 0;
                        int workingLedgerMode = 0;
                        int oppositeLedgerMode = 0;

                        if (this.ddlVoucherType.SelectedValue == "CP" || this.ddlVoucherType.SelectedValue == "BP")
                        {
                            workingLedgerMode = 2;
                            oppositeLedgerMode = 1;
                        }
                        else if (this.ddlVoucherType.SelectedValue == "CR" || this.ddlVoucherType.SelectedValue == "BR")
                        {
                            workingLedgerMode = 1;
                            oppositeLedgerMode = 2;
                        }

                        List<GLLedgerBO> rpReservationDetailListBO = Session["DetailList"] == null ? new List<GLLedgerBO>() : Session["DetailList"] as List<GLLedgerBO>;
                        int rpDynamicDetailId = 0;
                        if (!string.IsNullOrWhiteSpace(this.SelectedNodeId.Value))
                        {
                            rpDynamicDetailId = Convert.ToInt32(this.SelectedNodeId.Value);
                        }

                        GLLedgerBO rpDetailBO = rpDynamicDetailId == 0 ? new GLLedgerBO() : rpReservationDetailListBO.Where(x => x.NodeId == rpDynamicDetailId).FirstOrDefault();
                        if (rpDetailBO != null)
                        {
                            if (workingLedgerMode == 2)
                            {
                                tmpLedgerAmount = rpDetailBO.LedgerCreditAmount;
                            }
                            else
                            {
                                tmpLedgerAmount = rpDetailBO.LedgerDebitAmount;
                            }
                        }
                        else
                        {
                            rpDetailBO = new GLLedgerBO();
                        }

                        if (rpReservationDetailListBO.Contains(rpDetailBO))
                            rpReservationDetailListBO.Remove(rpDetailBO);

                        detailBO.ChequeNumber = string.Empty;
                        if (this.ddlVoucherType.SelectedValue == "BP")
                        {
                            this.txtChequeNumber.Text = hfChequeNumberForConfigureAccountHead.Value;
                            rpDetailBO.ChequeNumber = this.txtChequeNumber.Text;
                        }
                        else if (this.ddlVoucherType.SelectedValue == "BR")
                        {
                            this.txtChequeNumber.Text = hfChequeNumberForConfigureAccountHead.Value;
                            rpDetailBO.ChequeNumber = this.txtChequeNumber.Text;
                        }
                        else if (this.ddlVoucherType.SelectedValue == "CV")
                        {
                            this.txtJVChequeNumber.Text = hfChequeNumberForConfigureAccountHead.Value;
                            rpDetailBO.ChequeNumber = this.txtChequeNumber.Text;
                        }
                        else if (this.ddlVoucherType.SelectedValue == "JV")
                        {
                            this.txtJVChequeNumber.Text = hfChequeNumberForConfigureAccountHead.Value;
                            rpDetailBO.ChequeNumber = this.txtJVChequeNumber.Text;
                        }

                        if (string.IsNullOrEmpty(this.SelectedNodeId.Value))
                        {
                            this.SelectedNodeId.Value = this.ddlConfigureAccountHead.SelectedValue;
                            tmpLedgerAmount = CalculateAmountTotal(oppositeLedgerMode);
                            rpDetailBO.NodeId = Convert.ToInt32(this.SelectedNodeId.Value);
                            rpDetailBO.NodeHead = this.ddlConfigureAccountHead.SelectedItem.Text;
                            rpDetailBO.LedgerMode = workingLedgerMode;
                            rpDetailBO.LedgerAmount = tmpLedgerAmount;
                            if (workingLedgerMode == 2)
                            {
                                rpDetailBO.LedgerCreditAmount = tmpLedgerAmount;
                            }
                            else
                            {
                                rpDetailBO.LedgerDebitAmount = tmpLedgerAmount;
                            }

                            //decimal lAmount = tmpLedgerAmount;
                            //decimal crAmount = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 0;
                            rpDetailBO.CurrencyAmount = tmpLedgerAmount; // crAmount;
                            rpDetailBO.FieldId = Convert.ToInt32(this.ddlCurrency.SelectedValue);
                            rpDetailBO.NodeNarration = this.txtNodeNarration.Text;
                            if (this.btnSave.Text.Equals("Update"))
                            {
                                ReceivePaymentLedgerId = Convert.ToInt32(Session["ReceivePaymentLedgerId"]);
                                rpDetailBO.LedgerId = ReceivePaymentLedgerId;
                            }
                            else
                            {
                                rpDetailBO.LedgerId = rpDynamicDetailId == 0 ? rpReservationDetailListBO.Count + 1 : rpDynamicDetailId;
                            }

                            rpReservationDetailListBO.Add(rpDetailBO);
                            Session["DetailList"] = rpReservationDetailListBO;
                        }
                        else
                        {
                            tmpLedgerAmount = CalculateAmountTotal(oppositeLedgerMode);
                            rpDetailBO.NodeId = Convert.ToInt32(this.SelectedNodeId.Value);
                            rpDetailBO.NodeHead = this.ddlConfigureAccountHead.SelectedItem.Text;
                            rpDetailBO.LedgerMode = workingLedgerMode;
                            rpDetailBO.LedgerAmount = tmpLedgerAmount;
                            if (workingLedgerMode == 2)
                            {
                                rpDetailBO.LedgerCreditAmount = tmpLedgerAmount;
                            }
                            else
                            {
                                rpDetailBO.LedgerDebitAmount = tmpLedgerAmount;
                            }

                            decimal lAmount = tmpLedgerAmount;
                            decimal crAmount = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
                            rpDetailBO.CurrencyAmount = lAmount / crAmount;

                            rpDetailBO.FieldId = Convert.ToInt32(this.ddlCurrency.SelectedValue);
                            rpDetailBO.NodeNarration = this.txtNodeNarration.Text;

                            rpDetailBO.LedgerId = rpDynamicDetailId == 0 ? rpReservationDetailListBO.Count + 1 : rpDynamicDetailId;
                            rpReservationDetailListBO.Add(rpDetailBO);
                            Session["DetailList"] = rpReservationDetailListBO;

                        }

                        //detailBO.CurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;

                    }
                    //--------------------------------------------------------------------------------------------------

                    this.gvDetail.DataSource = Session["DetailList"] as List<GLLedgerBO>;
                    this.gvDetail.DataBind();
                    this.ClearDetailPart();
                }
                this.txtGoToScrolling.Text = "EntryPanel";
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Voucher Type.", AlertType.Warning);
                this.ddlVoucherType.Focus();
            }
            this.lblConfigureAccountHeadText.Text = this.ddlConfigureAccountHead.SelectedItem.Text;

            string mIsVisible = this.IsActiveChangeAccountHead.Value;

            if (mIsVisible == "5")
            {
                this.btnChangeAccountHead.Visible = true;
            }
            else
            {
                this.btnChangeAccountHead.Visible = false;
            }
            this.CalculateotalDetailAmountT();
        }
        protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");

                if (this.SelectedNodeId.Value.Equals(lblValue.Text))
                {
                    imgDelete.Visible = false;
                }
                else
                {
                    imgDelete.Visible = true;
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/GeneralLedger/frmVoucher.aspx");
        }
        protected void gvDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _selectedNodeId;
            int voucherType = 1;
            int workingLedgerMode = 2;
            int oppositeLedgerMode = 1;
            int configurableLedgerId = 1;
            if (this.ddlVoucherType.SelectedValue == "JV")
            {
                voucherType = 0;
                isEnableTransectionMode = 1;
            }
            if (this.ddlVoucherType.SelectedValue == "CV")
            {
                voucherType = 0;
                isEnableTransectionMode = 1;
            }
            if (this.ddlVoucherType.SelectedValue == "BR")
            {
                workingLedgerMode = 1;
                oppositeLedgerMode = 2;
            }
            if (this.ddlVoucherType.SelectedValue == "CR")
            {
                workingLedgerMode = 1;
                oppositeLedgerMode = 2;
            }
            if (e.CommandName == "CmdDelete")
            {
                _selectedNodeId = Convert.ToInt32(e.CommandArgument.ToString());
                if (voucherType == 1)
                {
                    int _selectedLedgerId = 0;
                    var accountDetailListBO = (List<GLLedgerBO>)Session["DetailList"];
                    var deleteAccountDetail = accountDetailListBO.Where(x => x.NodeId == _selectedNodeId).FirstOrDefault();
                    _selectedLedgerId = accountDetailListBO.Where(x => x.NodeId == _selectedNodeId).FirstOrDefault().LedgerId;
                    accountDetailListBO.Remove(deleteAccountDetail);
                    arrayDelete.Add(_selectedLedgerId);
                    if (accountDetailListBO.Count == 1)
                    {
                        configurableLedgerId = accountDetailListBO[0].LedgerId;
                        Session["DetailList"] = null;
                        this.SelectedNodeId.Value = string.Empty;
                    }
                    else
                    {
                        Session["DetailList"] = accountDetailListBO;
                    }

                    this.gvDetail.DataSource = Session["DetailList"] as List<GLLedgerBO>;
                    this.gvDetail.DataBind();

                    //---For Payment or Received Voucher----------------------------------------------------------------
                    if (voucherType != 0)
                    {
                        decimal tmpLedgerAmount = 0;
                        List<GLLedgerBO> rpReservationDetailListBO = Session["DetailList"] == null ? new List<GLLedgerBO>() : Session["DetailList"] as List<GLLedgerBO>;
                        int rpDynamicDetailId = 0;
                        if (!string.IsNullOrWhiteSpace(this.SelectedNodeId.Value))
                        {
                            rpDynamicDetailId = Convert.ToInt32(this.SelectedNodeId.Value);
                        }

                        GLLedgerBO rpDetailBO = rpDynamicDetailId == 0 ? new GLLedgerBO() : rpReservationDetailListBO.Where(x => x.NodeId == rpDynamicDetailId).FirstOrDefault();
                        if (rpDetailBO != null)
                        {
                            tmpLedgerAmount = rpDetailBO.LedgerCreditAmount;
                        }
                        if (rpReservationDetailListBO.Contains(rpDetailBO))
                            rpReservationDetailListBO.Remove(rpDetailBO);

                        if (string.IsNullOrEmpty(this.SelectedNodeId.Value))
                        {
                            this.SelectedNodeId.Value = this.ddlConfigureAccountHead.SelectedValue;
                            tmpLedgerAmount = CalculateAmountTotal(workingLedgerMode);
                            rpDetailBO.NodeId = Convert.ToInt32(this.SelectedNodeId.Value);
                            rpDetailBO.NodeHead = this.ddlConfigureAccountHead.SelectedItem.Text;
                            rpDetailBO.LedgerMode = workingLedgerMode;
                            rpDetailBO.LedgerAmount = tmpLedgerAmount;
                            rpDetailBO.LedgerCreditAmount = tmpLedgerAmount;
                            if (this.btnSave.Text.Equals("Update"))
                            {
                                rpDetailBO.LedgerId = configurableLedgerId;
                            }
                            else
                            {
                                rpDetailBO.LedgerId = rpDynamicDetailId == 0 ? rpReservationDetailListBO.Count + 1 : rpDynamicDetailId;
                            }
                            rpReservationDetailListBO.Add(rpDetailBO);
                            Session["DetailList"] = rpReservationDetailListBO;
                        }
                        else
                        {
                            tmpLedgerAmount = CalculateAmountTotal(oppositeLedgerMode);
                            rpDetailBO.NodeId = Convert.ToInt32(this.SelectedNodeId.Value);
                            rpDetailBO.NodeHead = this.ddlConfigureAccountHead.SelectedItem.Text;
                            rpDetailBO.LedgerMode = workingLedgerMode;
                            rpDetailBO.LedgerAmount = tmpLedgerAmount;
                            if (workingLedgerMode == 1)
                            {
                                rpDetailBO.LedgerDebitAmount = tmpLedgerAmount;
                            }
                            else
                            {
                                rpDetailBO.LedgerCreditAmount = tmpLedgerAmount;
                            }
                            if (this.btnSave.Text.Equals("Update"))
                            {
                                if (Session["ReceivePaymentLedgerId"] != null)
                                {
                                    ReceivePaymentLedgerId = Convert.ToInt32(Session["ReceivePaymentLedgerId"]);
                                    rpDetailBO.LedgerId = ReceivePaymentLedgerId;
                                }
                            }
                            else
                            {
                                rpDetailBO.LedgerId = rpDynamicDetailId == 0 ? rpReservationDetailListBO.Count + 1 : rpDynamicDetailId;
                            }


                            rpReservationDetailListBO.Add(rpDetailBO);
                            Session["DetailList"] = rpReservationDetailListBO;

                        }
                    }
                    //--------------------------------------------------------------------------------------------------

                    this.gvDetail.DataSource = Session["DetailList"] as List<GLLedgerBO>;
                    this.gvDetail.DataBind();
                }
                else
                {
                    var DetailBO = (List<GLLedgerBO>)Session["DetailList"];
                    var voucherDetail = DetailBO.Where(x => x.NodeId == _selectedNodeId).FirstOrDefault();
                    var currentLedgerId = DetailBO.Where(x => x.NodeId == _selectedNodeId).FirstOrDefault().LedgerId;
                    DetailBO.Remove(voucherDetail);
                    Session["DetailList"] = DetailBO;
                    arrayDelete.Add(currentLedgerId);
                    this.gvDetail.DataSource = Session["DetailList"] as List<GLLedgerBO>;
                    this.gvDetail.DataBind();
                }
            }

            this.CalculateotalDetailAmountT();
        }
        //************************ User Defined Function ********************//
        private void LoadSingleProjectAndCompany()
        {
            this.LoadGLCompany(true);
            this.LoadGLProject(true);
        }
        private void LoadCurrency()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();

            fields = commonDA.GetCustomField("Currency", hmUtility.GetDropDownFirstValue());
            if (fields != null)
            {
                if (fields.Count > 1)
                {
                    fields.RemoveAt(0);
                }
            }

            this.ddlCurrency.DataSource = fields;
            this.ddlCurrency.DataTextField = "FieldValue";
            this.ddlCurrency.DataValueField = "FieldId";
            this.ddlCurrency.DataBind();
        }
        private void LoadUserInformation()
        {
            UserInformationDA entityDA = new UserInformationDA();

            this.ddlCheckedBy.DataSource = entityDA.GetUserInformation();
            this.ddlCheckedBy.DataTextField = "UserName";
            this.ddlCheckedBy.DataValueField = "UserInfoId";
            this.ddlCheckedBy.DataBind();

            ListItem itemEmployee = new ListItem();
            itemEmployee.Value = "0";
            itemEmployee.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCheckedBy.Items.Insert(0, itemEmployee);


            this.ddlApprovedBy.DataSource = entityDA.GetUserInformation();
            this.ddlApprovedBy.DataTextField = "UserName";
            this.ddlApprovedBy.DataValueField = "UserInfoId";
            this.ddlApprovedBy.DataBind();

            this.ddlApprovedBy.Items.Insert(0, itemEmployee);
        }
        private void FillVoucherForm(int DealId)
        {
            int voucherType = 1;
            GLDealMasterDA masterDA = new GLDealMasterDA();
            GLDealMasterBO masterBO = masterDA.GetVoucherInfoByDealId(DealId);
            this.ddlGLCompany.SelectedValue = masterBO.CompanyId.ToString();
            this.txtVoucherNo.Text = masterBO.VoucherNo;
            this.txtDealId.Value = DealId.ToString();
            btnSave.Text = "Update";

            this.isProjectDdlEnable = 1;
            this.ddlGLProject.SelectedValue = masterBO.ProjectId.ToString();
            this.txtVoucherDate.Text = masterBO.VoucherDate.ToShortDateString();
            this.txtPayerOrPayee.Text = masterBO.PayerOrPayee;
            this.ddlCashChequeMode.SelectedIndex = Convert.ToInt32(masterBO.CashChequeMode.ToString());
            this.txtNarration.Text = masterBO.Narration;
            this.ddlCheckedBy.SelectedValue = masterBO.CheckedBy.ToString();
            this.ddlApprovedBy.SelectedValue = masterBO.ApprovedBy.ToString();

            int VoucherMode = masterBO.VoucherMode;
            if (VoucherMode == 1)
            {
                if (masterBO.CashChequeMode == 1)
                {
                    this.ddlVoucherType.SelectedValue = "CP";
                }
                else
                {
                    this.ddlVoucherType.SelectedValue = "BP";
                }
                this.ddlVoucherMode.SelectedIndex = 1;
            }
            else if (VoucherMode == 2)
            {
                if (masterBO.CashChequeMode == 1)
                {
                    this.ddlVoucherType.SelectedValue = "CR";

                }
                else
                {
                    this.ddlVoucherType.SelectedValue = "BR";
                }
                this.ddlVoucherMode.SelectedIndex = 2;
            }
            else if (VoucherMode == 3)
            {
                voucherType = 0;
                this.ddlVoucherType.SelectedValue = "JV";
                this.ddlVoucherMode.SelectedIndex = 3;
                isEnableTransectionMode = 1;
            }
            else if (VoucherMode == 4)
            {
                voucherType = 0;
                this.ddlVoucherType.SelectedValue = "CV";
                this.ddlVoucherMode.SelectedIndex = 4;
                isEnableTransectionMode = 1;
            }

            List<GLLedgerBO> List = masterDA.GetVoucherDetailsInfoByDealId(DealId);
            Session["DetailList"] = List;

            if (voucherType != 0)
            {
                if (this.btnSave.Text.Equals("Update"))
                {

                    var accountDetailListBO = (List<GLLedgerBO>)Session["DetailList"];
                    //if (this.ddlVoucherType.SelectedValue == "CP" || this.ddlVoucherType.SelectedValue == "BP")
                    //{
                    //    ConfigureAccountHeadId = accountDetailListBO.Where(x => x.LedgerMode == 2).FirstOrDefault().NodeId;
                    //    ReceivePaymentLedgerId = accountDetailListBO.Where(x => x.LedgerMode == 2).FirstOrDefault().LedgerId;
                    //    this.ddlConfigureAccountHead.SelectedValue = ConfigureAccountHeadId.ToString();
                    //    this.SelectedNodeId.Value = this.ddlConfigureAccountHead.SelectedValue;
                    //    Session["ReceivePaymentLedgerId"] = ReceivePaymentLedgerId;
                    //}
                    //else if (this.ddlVoucherType.SelectedValue == "CR" || this.ddlVoucherType.SelectedValue == "BR")
                    //{
                    //    ConfigureAccountHeadId = accountDetailListBO.Where(x => x.LedgerMode == 1).FirstOrDefault().NodeId;
                    //    ReceivePaymentLedgerId = accountDetailListBO.Where(x => x.LedgerMode == 1).FirstOrDefault().LedgerId;
                    //    this.ddlConfigureAccountHead.SelectedValue = ConfigureAccountHeadId.ToString();
                    //    this.SelectedNodeId.Value = this.ddlConfigureAccountHead.SelectedValue;
                    //    Session["ReceivePaymentLedgerId"] = ReceivePaymentLedgerId;
                    //}


                    if (VoucherMode == 1)
                    {
                        ConfigureAccountHeadId = accountDetailListBO.Where(x => x.LedgerMode == 2).FirstOrDefault().NodeId;
                        ReceivePaymentLedgerId = accountDetailListBO.Where(x => x.LedgerMode == 2).FirstOrDefault().LedgerId;
                        this.ddlConfigureAccountHead.SelectedValue = ConfigureAccountHeadId.ToString();
                        this.SelectedNodeId.Value = this.ddlConfigureAccountHead.SelectedValue;
                        Session["ReceivePaymentLedgerId"] = ReceivePaymentLedgerId;
                    }
                    if (VoucherMode == 2)
                    {
                        ConfigureAccountHeadId = accountDetailListBO.Where(x => x.LedgerMode == 1).FirstOrDefault().NodeId;
                        ReceivePaymentLedgerId = accountDetailListBO.Where(x => x.LedgerMode == 1).FirstOrDefault().LedgerId;
                        this.ddlConfigureAccountHead.SelectedValue = ConfigureAccountHeadId.ToString();
                        this.SelectedNodeId.Value = this.ddlConfigureAccountHead.SelectedValue;
                        Session["ReceivePaymentLedgerId"] = ReceivePaymentLedgerId;
                    }
                }
            }

            this.lblConfigureAccountHeadText.Text = this.ddlConfigureAccountHead.SelectedItem.Text;
            this.gvDetail.DataSource = Session["DetailList"] as List<GLLedgerBO>;
            this.gvDetail.DataBind();
            this.CalculateotalDetailAmountT();
        }
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtVoucherDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        }
        private void LoadGLCompany(bool isSingle)
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            var List = entityDA.GetAllGLCompanyInfo();
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            if (isSingle == true)
            {
                companyList.Add(List[0]);
                this.ddlGLCompany.DataSource = companyList;
                this.ddlGLCompany.DataTextField = "Name";
                this.ddlGLCompany.DataValueField = "CompanyId";
                this.ddlGLCompany.DataBind();
            }
            else
            {
                this.ddlGLCompany.DataSource = List;
                this.ddlGLCompany.DataTextField = "Name";
                this.ddlGLCompany.DataValueField = "CompanyId";
                this.ddlGLCompany.DataBind();
                ListItem itemCompany = new ListItem();
                itemCompany.Value = "0";
                itemCompany.Text = hmUtility.GetDropDownFirstValue();
                this.ddlGLCompany.Items.Insert(0, itemCompany);
            }


        }
        private void LoadGLProject(bool isSingle)
        {
            GLProjectDA entityDA = new GLProjectDA();
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            var List = entityDA.GetAllGLProjectInfo();
            if (isSingle == true)
            {
                projectList.Add(List[0]);
                this.ddlGLProject.DataSource = projectList;
                this.ddlGLProject.DataTextField = "Name";
                this.ddlGLProject.DataValueField = "ProjectId";
                this.ddlGLProject.DataBind();
            }
            else
            {
                this.ddlGLProject.DataSource = List;
                this.ddlGLProject.DataTextField = "Name";
                this.ddlGLProject.DataValueField = "ProjectId";
                this.ddlGLProject.DataBind();

                ListItem itemProject = new ListItem();
                itemProject.Value = "0";
                itemProject.Text = hmUtility.GetDropDownFirstValue();
                this.ddlGLProject.Items.Insert(0, itemProject);
            }
            this.SingleprojectId.Value = List[0].ProjectId.ToString();
        }
        private void ClearVoucherNumber()
        {
            if (Convert.ToInt32(this.ddlGLProject.SelectedValue) < 1)
            {
                this.txtVoucherNo.Text = string.Empty;
            }
        }
        private void LoadAccountHead()
        {
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            this.ddlNodeId.DataSource = nodeMatrixDA.GetNodeMatrixInfo();
            this.ddlNodeId.DataTextField = "NodeHead";
            this.ddlNodeId.DataValueField = "NodeId";
            this.ddlNodeId.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlNodeId.Items.Insert(0, itemNodeId);

            this.ddlConfigureAccountHead.DataSource = nodeMatrixDA.GetNodeMatrixInfo();
            this.ddlConfigureAccountHead.DataTextField = "NodeHead";
            this.ddlConfigureAccountHead.DataValueField = "NodeId";
            this.ddlConfigureAccountHead.DataBind();

            this.ddlConfigureAccountHead.Items.Insert(0, itemNodeId);
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmVoucher.ToString());

            //isSavePermission = objectPermissionBO.IsSavePermission;
            //isDeletePermission = objectPermissionBO.IsDeletePermission;
            //btnSave.Visible = isSavePermission;
            //btnAddDetail.Visible = isSavePermission;
        }
        public decimal CalculateAmountTotal(int voucherType)
        {
            decimal CalculateAmountTotal = 0, AmtTmp;

            for (int i = 0; i < gvDetail.Rows.Count; i++)
            {
                AmtTmp = 0;
                if (voucherType == 1)
                {
                    if (decimal.TryParse(((Label)gvDetail.Rows[i].FindControl("lblLedgerDebitAmount")).Text, out AmtTmp))
                        CalculateAmountTotal += AmtTmp;
                }
                else
                {
                    if (decimal.TryParse(((Label)gvDetail.Rows[i].FindControl("lblLedgerCreditAmount")).Text, out AmtTmp))
                        CalculateAmountTotal += AmtTmp;
                }
            }

            return CalculateAmountTotal;
        }
        public void CalculateotalDetailAmountT()
        {
            TotalCalculateDebitCreditAmount.Visible = true;
            decimal debitAmtTotal = 0, creditAmtTotal = 0, tmpDebitAmount, tmpCreditAmount;

            for (int i = 0; i < gvDetail.Rows.Count; i++)
            {
                tmpDebitAmount = 0;
                tmpCreditAmount = 0;

                if ((decimal.TryParse(((Label)gvDetail.Rows[i].FindControl("lblLedgerDebitAmount")).Text, out tmpDebitAmount)))
                    debitAmtTotal += tmpDebitAmount;

                if ((decimal.TryParse(((Label)gvDetail.Rows[i].FindControl("lblLedgerCreditAmount")).Text, out tmpCreditAmount)))
                    creditAmtTotal += tmpCreditAmount;
            }

            this.lblTotalCalculateDebitAmount.Text = new HMCommonDA().CurrencyMask(debitAmtTotal.ToString());
            this.lblTotalCalculateCreditAmount.Text = new HMCommonDA().CurrencyMask(creditAmtTotal.ToString());
        }
        private void AddEditODeleteDetail()
        {
            //Delete------------
            if (Session["arrayDelete"] == null)
            {
                arrayDelete = new ArrayList();
                Session.Add("arrayDelete", arrayDelete);
            }
            else
                arrayDelete = Session["arrayDelete"] as ArrayList;
        }
        private void ClearDetailPart()
        {
            btnAddDetail.Text = "Add";
            this.ddlNodeId.SelectedValue = "0";
            this.txtLedgerAmount.Text = string.Empty;
            this.txtCalculatedAmount.Text = string.Empty;
            //this.txtConversionRate.Text = string.Empty;
            this.txtChequeNumber.Text = string.Empty;
            this.txtJVChequeNumber.Text = string.Empty;
            this.txtNodeNarration.Text = string.Empty;
            this.lblHiddenId.Text = string.Empty;
        }
        private bool IsFormValid()
        {
            bool status = true;
            List<GLLedgerBO> listDetailBO = new List<GLLedgerBO>();
            listDetailBO = Session["DetailList"] as List<GLLedgerBO>;

            if (listDetailBO == null)
            {
                CommonHelper.AlertInfo("No Account Head added in Voucher.", AlertType.Warning);
                this.txtPayerOrPayee.Focus();
                status = false;
            }
            else if (this.ddlVoucherMode.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Voucher Mode.", AlertType.Warning);
                this.ddlVoucherMode.Focus();
                status = false;
            }
            else if (this.ddlCashChequeMode.SelectedIndex == 0)
            {
                if (this.ddlVoucherMode.SelectedIndex == 4)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Payment Mode.", AlertType.Warning);
                    this.ddlCashChequeMode.Focus();
                    status = false;
                }
            }
            else if (string.IsNullOrWhiteSpace(this.txtVoucherDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Voucher Date.", AlertType.Warning);
                status = false;
            }
            else if (this.ddlApprovedBy.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "ApprovedBy.", AlertType.Warning);
                this.ddlApprovedBy.Focus();
                status = false;
            }
            else if (listDetailBO != null)
            {
                decimal DrAmount = 0;
                decimal CrAmount = 0;

                foreach (GLLedgerBO row in listDetailBO)
                {
                    if (row.LedgerMode == 1)
                    {
                        DrAmount += row.LedgerDebitAmount;
                    }
                    else
                    {
                        CrAmount += row.LedgerCreditAmount;
                    }
                }

                if (DrAmount != CrAmount)
                {
                    if (this.ddlVoucherType.SelectedValue == "JV" || this.ddlVoucherType.SelectedValue == "CV")
                    {
                        this.isEnableTransectionMode = 1;
                    }

                    CommonHelper.AlertInfo("Your Entered Debit Amount and Credit Amount are not same.", AlertType.Warning);
                    status = false;
                }

            }


            return status;
        }
        private void Cancel()
        {
            this.txtVoucherNo.Text = string.Empty;
            this.txtPayerOrPayee.Text = string.Empty;
            this.txtNarration.Text = string.Empty;
            this.ClearDetailPart();
            Session["DetailList"] = null;
            this.gvDetail.DataSource = Session["DetailList"] as List<GLLedgerBO>;
            this.hfChequeNumberForConfigureAccountHead.Value = string.Empty;
            this.gvDetail.DataBind();
            Session["ReceivePaymentLedgerId"] = null;
            Session["arrayDelete"] = null;
            this.txtChequeNumber.Text = string.Empty;
            this.txtJVChequeNumber.Text = string.Empty;
            this.lblTotalCalculateDebitAmount.Text = "0";
            this.lblTotalCalculateCreditAmount.Text = "0";
            TotalCalculateDebitCreditAmount.Visible = false;
            this.ddlCheckedBy.SelectedIndex = 0;
            this.ddlApprovedBy.SelectedIndex = 0;
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static string GenerateVoucherNumber(string projectId, string voucherType, string voucherDate, int userId)
        {
            string generateVoucherNumber = string.Empty;
            /*
            //string projectId = "1";
            //    string voucherType = "CP";
            //        string voucherDate = "06/05/2013";
            //        int userId = 1;
            
            if (!string.IsNullOrEmpty(projectId))
            {
                if (voucherType != "None")
                {
                    if (!string.IsNullOrWhiteSpace(voucherDate))
                    {
                        if (!string.IsNullOrEmpty(voucherType))
                        {
                            GLAccountConfigurationDA entityDA = new GLAccountConfigurationDA();
                            GLAccountConfigurationBO entityBO = new GLAccountConfigurationBO();

                            string generatedVoucherNumber = entityDA.GetVoucherNumber(hmUtility.ParseDateTime(voucherDate), voucherType, Convert.ToInt32(projectId), userId);
                            if (!string.IsNullOrWhiteSpace(generatedVoucherNumber))
                            {
                                generateVoucherNumber = generatedVoucherNumber;
                                //this.txtVoucherNo.Text = generatedVoucherNumber;
                                //this.txtVoucherNo.Enabled = false;
                            }
                            else
                            {
                                //this.txtVoucherNo.Text = string.Empty;
                                //this.txtVoucherNo.Enabled = false;
                            }
                        }
                    }
                }
                else
                {
                    generateVoucherNumber = "None";
                    //this.txtVoucherNo.Text = string.Empty;
                    //this.txtVoucherNo.Enabled = false;
                }
            }
            else
            {
                //this.txtVoucherNo.Text = string.Empty;
                //this.txtVoucherNo.Enabled = false;
            }
            */
            return generateVoucherNumber;
        }
        [WebMethod]
        public static ArrayList PopulateGLProject(int companyId)
        {
            ArrayList list = new ArrayList();
            GLProjectDA entityDA = new GLProjectDA();
            List<GLProjectBO> entityListBO = new List<GLProjectBO>();

            entityListBO = entityDA.GetGLProjectInfoByGLCompany(Convert.ToInt32(companyId));

            foreach (GLProjectBO rows in entityListBO)
            {
                list.Add(new ListItem(rows.Name.ToString(), rows.ProjectId.ToString()));
            }

            return list;
        }
        [WebMethod]
        public static ArrayList PopulateConfigureAccountHead(string projectId, string accountType, int voucherType)
        {
            ArrayList list = new ArrayList();

            if (!string.IsNullOrEmpty(projectId))
            {
                GLAccountConfigurationDA entityDA = new GLAccountConfigurationDA();
                GLAccountConfigurationBO entityBO = new GLAccountConfigurationBO();

                entityBO = entityDA.GetAccountConfigurationInfoByProjectIdNAccountType(Convert.ToInt32(projectId), accountType);
                if (entityBO != null)
                {
                    Int64 accountRootHeadId = entityBO.NodeId;
                    int headCount = entityBO.HeadCount;
                    Boolean isEnableChangeButton = false;
                    if (headCount > 1)
                    {
                        isEnableChangeButton = true;
                    }

                    NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
                    List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();

                    nodeMatrixBOList = nodeMatrixDA.GetNodeMatrixInfoByNodeIdForGroupData(accountRootHeadId);

                    foreach (NodeMatrixBO rows in nodeMatrixBOList)
                    {
                        list.Add(new ListItem(rows.HeadWithCode.ToString(), rows.NodeId.ToString(), isEnableChangeButton));
                    }
                }
            }
            return list;
        }
        [WebMethod]
        public static int IsVisibleChangeButton(string projectId, string accountType, int voucherType)
        {
            int totalDataCount = 0;

            if (!string.IsNullOrEmpty(projectId))
            {
                GLAccountConfigurationDA entityDA = new GLAccountConfigurationDA();
                GLAccountConfigurationBO entityBO = new GLAccountConfigurationBO();

                entityBO = entityDA.GetAccountConfigurationInfoByProjectIdNAccountType(Convert.ToInt32(projectId), accountType);
                if (entityBO != null)
                {
                    totalDataCount = entityBO.HeadCount;
                }
            }
            return totalDataCount;
        }
        [WebMethod]
        public static string CheckDuplicateVoucherNumber(string voucherNo, string projectId, string voucherType)
        {
            string validVoucherNumber = string.Empty;
            if (!string.IsNullOrEmpty(projectId))
            {
                if (voucherType != "None")
                {
                    if (!string.IsNullOrEmpty(voucherType))
                    {
                        GLAccountConfigurationDA entityDA = new GLAccountConfigurationDA();
                        GLAccountConfigurationBO entityBO = new GLAccountConfigurationBO();

                        validVoucherNumber = entityDA.GetVoucherNumberForCheckDuplicate(voucherNo, voucherType, Convert.ToInt32(projectId));
                        if (Convert.ToInt32(validVoucherNumber) > 0)
                        {
                            validVoucherNumber = "Not Valid";
                        }
                        else
                        {
                            validVoucherNumber = "Valid";
                        }
                    }

                }
                else
                {
                    validVoucherNumber = "Not Valid";
                }
            }
            else
            {
                validVoucherNumber = "Not Valid";
            }
            return validVoucherNumber;
        }
        [WebMethod]
        public static ArrayList PopulateChangeConfigureAccountHead(string projectId, string voucherType)
        {
            ArrayList list = new ArrayList();

            if (!string.IsNullOrEmpty(projectId))
            {
                GLAccountConfigurationDA entityDA = new GLAccountConfigurationDA();
                List<GLAccountConfigurationBO> entityBOList = new List<GLAccountConfigurationBO>();

                entityBOList = entityDA.GetAllAccountConfigurationInfoByProjectIdNAccountType(Convert.ToInt32(projectId), voucherType);
                foreach (GLAccountConfigurationBO acrows in entityBOList)
                {
                    NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
                    NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();

                    nodeMatrixBO = nodeMatrixDA.GetNodeMatrixInfoById(acrows.NodeId);
                    if (nodeMatrixBO != null)
                    {
                        list.Add(new ListItem(nodeMatrixBO.HeadWithCode.ToString(), nodeMatrixBO.NodeId.ToString()));
                    }
                }
            }
            return list;
        }
        [WebMethod]
        public static ArrayList PopulateAccountHeadByNodeId(string NodeId)
        {
            ArrayList list = new ArrayList();

            if (!string.IsNullOrEmpty(NodeId) && NodeId != "0")
            {
                NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
                NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();

                nodeMatrixBO = nodeMatrixDA.GetNodeMatrixInfoById(Convert.ToInt32(NodeId));
                if (nodeMatrixBO != null)
                {
                    list.Add(new ListItem(nodeMatrixBO.HeadWithCode.ToString(), nodeMatrixBO.NodeId.ToString()));
                }

            }
            return list;
        }
        [WebMethod]
        public static List<NodeMatrixBO> GetAutoCompleteData(string searchText)
        {
            List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            nodeMatrixBOList = nodeMatrixDA.GetNodeMatrixInfoByAccountHead(searchText);

            return nodeMatrixBOList;
        }
        [WebMethod]
        public static List<string> GetAutoCompleteData1(string searchText)
        {
            HMUtility hmUtility = new HMUtility();
            searchText = hmUtility.sqlInjectionFilter(searchText, false);

            List<string> nodeMatrixBOList = new List<string>();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            nodeMatrixBOList = nodeMatrixDA.GetNodeMatrixInfoByAccountHead1(searchText, 1);

            return nodeMatrixBOList;
        }
        [WebMethod]
        public static string FillForm(string searchText)
        {
            string nodeMatrixBO = string.Empty;
            HMUtility hmUtility = new HMUtility();
            searchText = hmUtility.sqlInjectionFilter(searchText, false);

            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                nodeMatrixBO = nodeMatrixDA.GetNodeMatrixInfoByAccountHead2(searchText);
            }
            return nodeMatrixBO;
        }
        [WebMethod]
        public static string ValidateChequeNumber(int NodeId, string ChequeNumber)
        {
            string isValid = "true";
            // Validate For Current Context
            var ownerDetailBO = (List<GLLedgerBO>)HttpContext.Current.Session["DetailList"];
            if (ownerDetailBO != null)
            {
                //var ownerDetail = ownerDetailBO.Where(x => x.ChequeNumber == ChequeNumber && x.NodeId == NodeId).FirstOrDefault();
                var ownerDetail = ownerDetailBO.Where(x => x.ChequeNumber == ChequeNumber).FirstOrDefault();

                if (ownerDetail != null)
                {
                    if (!string.IsNullOrEmpty(ownerDetail.ChequeNumber))
                    {
                        isValid = "false";
                    }
                }
            }

            // Validate For Database Context
            GLLedgerDA ledgerDA = new GLLedgerDA();
            GLLedgerBO ledgerBO = new GLLedgerBO();
            ledgerBO = ledgerDA.IsChequeNumberExistByNodeId(NodeId, ChequeNumber);
            if (!string.IsNullOrEmpty(ledgerBO.ChequeNumber))
            {
                isValid = "false";
            }

            return isValid;
        }
    }
}