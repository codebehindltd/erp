using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using System.Web.Services;
using HotelManagement.Entity;
using HotelManagement.Entity.Membership;
using HotelManagement.Data.Membership;

namespace HotelManagement.Presentation.Website.Membership
{
    public partial class frmMemberBillPayment : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected bool isSingle = true;
        protected int isMessageBoxEnable = -1;
        protected int isIntegratedGeneralLedgerDiv = 1;
        protected int isNewAddButtonEnable = 1;
        protected int isSearchPanelEnable = -1;
        protected int isCompanyProjectPanelEnable = -1;
        protected int LocalCurrencyId;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        private Boolean isViewPermission = false;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            isIntegratedGeneralLedgerDiv = -1;
            isSingle = hmUtility.GetSingleProjectAndCompany();
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
            }
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.lblDisplayConvertionRate.Text = string.Empty;
                this.btnGroupPaymentPreview.Visible = false;
                string cardValidation = System.Web.Configuration.WebConfigurationManager.AppSettings["CardValidation"].ToString();
                txtCardValidation.Value = cardValidation.ToString();
                this.LoadBank();
                this.LoadCurrency();
                LoadLocalCurrencyId();
                LoadIsConversionRateEditable();
                IsLocalCurrencyDefaultSelected();
                this.LoadCommonDropDownHiddenField();
                this.LoadGLCompany(false);
                this.LoadAccountHeadInfo();
                this.LoadGLProject(false);
                this.ClearCommonSessionInformation();

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

                Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                if (!isIntegrated)
                {
                    isIntegratedGeneralLedgerDiv = -1;
                }

                string roomId = Request.QueryString["RoomId"];
                if (!string.IsNullOrEmpty(roomId))
                {
                    RoomNumberDA numberDA = new RoomNumberDA();
                    RoomNumberBO numberBO = numberDA.GetRoomNumberInfoById(Int32.Parse(roomId));
                    txtSrcMemberId.Text = numberBO.RoomNumber;
                    this.SearchInformation();
                }
            }
        }
        protected void gvGuestHouseService_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvGuestHouseService.PageIndex = e.NewPageIndex;
        }
        protected void gvGuestHouseService_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
            }
        }
        protected void gvGuestHouseService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdPaymentPreview")
            {
                string url = "/Membership/Reports/frmReportMemberPayment.aspx?PaymentIdList=" + Convert.ToInt32(e.CommandArgument.ToString());
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=820,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
                this.SearchInformation();
            }
        }
        protected void ddlRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadRelatedInformation();
        }
        protected void btnSrcMember_Click(object sender, EventArgs e)
        {
            MemMemberBasicsBO memberBO = new MemMemberBasicsBO();
            MemMemberBasicDA memberDA = new MemMemberBasicDA();
            if (!string.IsNullOrEmpty(txtSrcMemberId.Text))
            {
                memberBO = memberDA.GetMemberInfoByMembershipNo(txtSrcMemberId.Text);
            }

            if (memberBO == null)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Member Id.", AlertType.Warning);
                this.txtSrcMemberId.Focus();
                return;
            }
            else txtMemberName.Text = memberBO.FullName;
            SetTab("EntryTab");
        }
        protected void btnPaymentPreview_Click(object sender, EventArgs e)
        {
            string paymentIdList = string.Empty;
            if (!string.IsNullOrWhiteSpace(txtSrcMemberId.Text))
            {
                foreach (GridViewRow row in gvPaymentInfo.Rows)
                {
                    bool isSelect = ((CheckBox)row.FindControl("ChkSelect")).Checked;

                    if (isSelect)
                    {
                        Label lblObjectTabIdValue = (Label)row.FindControl("lblid");
                        if (string.IsNullOrWhiteSpace(paymentIdList.Trim()))
                        {
                            paymentIdList = paymentIdList + Convert.ToInt32(lblObjectTabIdValue.Text);
                        }
                        else
                        {
                            paymentIdList = paymentIdList + "," + Convert.ToInt32(lblObjectTabIdValue.Text);
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(paymentIdList))
            {
                string url = "/HotelManagement/Reports/frmReportGuestPaymentInvoice.aspx?PaymentIdList=" + paymentIdList;
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
                this.SearchInformation();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.LoadLocalCurrencyId();
            string transactionHead = string.Empty;

            if (!IsFrmValid())
            {
                return;
            }

            Boolean isNumberValue = hmUtility.IsNumber(this.txtLedgerAmount.Text);
            if (!isNumberValue)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Enter Numeric Value.", AlertType.Warning);
                this.txtLedgerAmount.Focus();
                return;
            }

            if (isSingle == false)
            {
                if (this.ddlGLCompany.SelectedValue == "0")
                {
                    Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                    if (isIntegrated)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Please Select Company Name.", AlertType.Warning);
                        this.ddlGLCompany.Focus();
                        return;
                    }
                }
                else if (this.ddlGLCompany.SelectedValue != "0")
                {
                    if (this.ddlGLProject.SelectedValue == "0")
                    {
                        Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                        if (isIntegrated)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Please select Project Name.", AlertType.Warning);
                            this.ddlGLProject.Focus();
                            return;
                        }
                    }
                }
            }

            HMCommonDA hmCommonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();

            if (customField == null)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Contact With Administrator for Accounts Mapping.", AlertType.Warning);
                return;
            }
            else
            {
                MemMemberBasicsBO memberBO = new MemMemberBasicsBO();
                MemMemberBasicDA memberDA = new MemMemberBasicDA();
                if (!string.IsNullOrEmpty(txtSrcMemberId.Text))
                {
                    memberBO = memberDA.GetMemberInfoByMembershipNo(txtSrcMemberId.Text);
                }

                if (memberBO == null)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Member Id.", AlertType.Warning);
                    this.txtSrcMemberId.Focus();
                    return;
                }

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();
                GuestBillPaymentDA reservationBillPaymentDA = new GuestBillPaymentDA();
                PMMemberPaymentLedgerBO memPaymentLedgerBO = new PMMemberPaymentLedgerBO();
                MemberPaymentDA memPaymentDA = new MemberPaymentDA();

                guestBillPaymentBO.PaymentType = "Advance";
                guestBillPaymentBO.RegistrationId = memberBO.MemberId;
                guestBillPaymentBO.Remarks = this.txtRemarks.Text;

                if (hfCurrencyType.Value == "Local")
                {
                    guestBillPaymentBO.FieldId = LocalCurrencyId;
                    guestBillPaymentBO.ConvertionRate = 1;
                    decimal tmpCurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                    guestBillPaymentBO.CurrencyAmount = tmpCurrencyAmount * guestBillPaymentBO.ConvertionRate;
                    guestBillPaymentBO.PaymentAmount = tmpCurrencyAmount * guestBillPaymentBO.ConvertionRate;
                }
                else
                {
                    guestBillPaymentBO.FieldId = Convert.ToInt32(this.ddlCurrency.SelectedValue);
                    if (txtConversionRate.ReadOnly != true)
                    {
                        guestBillPaymentBO.ConvertionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
                    }
                    else
                    {
                        if (hfConversionRate.Value != "")
                            guestBillPaymentBO.ConvertionRate = Convert.ToDecimal(hfConversionRate.Value);
                    }
                    decimal tmpCurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                    guestBillPaymentBO.CurrencyAmount = tmpCurrencyAmount;
                    guestBillPaymentBO.PaymentAmount = tmpCurrencyAmount * guestBillPaymentBO.ConvertionRate;
                }

                guestBillPaymentBO.PaymentMode = ddlPayMode.SelectedValue;
                guestBillPaymentBO.PaymentModeId = memberBO.MemberId;
                guestBillPaymentBO.ChecqueDate = DateTime.Now;
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlBankId.SelectedValue);
                guestBillPaymentBO.ServiceBillId = null;

                if (ddlPayMode.SelectedValue == "Cash")
                {
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashReceiveAccountsInfo.SelectedValue);
                    guestBillPaymentBO.ChecqueNumber = "";
                    guestBillPaymentBO.CardReference = "";
                    guestBillPaymentBO.CardNumber = "";
                    guestBillPaymentBO.BranchName = "";
                    guestBillPaymentBO.PaymentDescription = string.Empty;
                }
                else if (ddlPayMode.SelectedValue == "Card")
                {
                    guestBillPaymentBO.CardType = this.ddlCardType.SelectedValue.ToString();
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCardReceiveAccountsInfo.SelectedValue);
                    guestBillPaymentBO.CardNumber = this.txtCardNumber.Text;
                    if (string.IsNullOrEmpty(this.txtExpireDate.Text))
                    {
                        guestBillPaymentBO.ExpireDate = null;
                    }
                    else
                    {
                        guestBillPaymentBO.ExpireDate = hmUtility.GetDateTimeFromString(this.txtExpireDate.Text, userInformationBO.ServerDateFormat);
                    }
                    guestBillPaymentBO.CardHolderName = this.txtCardHolderName.Text;
                    guestBillPaymentBO.ChecqueNumber = "";
                    guestBillPaymentBO.PaymentDescription = ddlCardType.SelectedItem.Text;
                }
                else if (ddlPayMode.SelectedValue == "Cheque")
                {
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlChecquePaymentAccountHeadId.SelectedValue);
                    guestBillPaymentBO.BankId = Convert.ToInt32(ddlCompanyBank.SelectedValue);
                    guestBillPaymentBO.ChecqueNumber = txtChecqueNumber.Text;

                    guestBillPaymentBO.CardReference = "";
                    guestBillPaymentBO.CardNumber = "";
                    guestBillPaymentBO.BranchName = "";
                    guestBillPaymentBO.PaymentDescription = txtChecqueNumber.Text;
                }
                else if (ddlPayMode.SelectedValue == "Company")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCompanyPaymentAccountHead.SelectedValue);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCompanyPaymentAccountHead.SelectedValue);
                    guestBillPaymentBO.PaymentDescription = string.Empty;
                }
                guestBillPaymentBO.ModuleName = "Membership";

                //Member Payment Ledger
                memPaymentLedgerBO.PaymentType = guestBillPaymentBO.PaymentMode;
                memPaymentLedgerBO.BillNumber = "";
                if (!string.IsNullOrEmpty(txtPaymentDate2.Text))
                {
                    memPaymentLedgerBO.PaymentDate = CommonHelper.DateTimeToMMDDYYYY(txtPaymentDate2.Text);
                }
                else
                    memPaymentLedgerBO.PaymentDate = DateTime.Now;

                memPaymentLedgerBO.MemberId = memberBO.MemberId;
                memPaymentLedgerBO.CurrencyId = guestBillPaymentBO.FieldId;
                memPaymentLedgerBO.ConvertionRate = guestBillPaymentBO.ConvertionRate;
                memPaymentLedgerBO.DRAmount = guestBillPaymentBO.PaymentAmount;
                memPaymentLedgerBO.CRAmount = 0;
                memPaymentLedgerBO.CurrencyAmount = guestBillPaymentBO.CurrencyAmount;
                memPaymentLedgerBO.Remarks = guestBillPaymentBO.Remarks;
                memPaymentLedgerBO.PaymentStatus = HMConstants.ApprovalStatus.Approved.ToString();

                if (string.IsNullOrWhiteSpace(hfMemberPaymentId.Value))
                {
                    Boolean status = false;
                    int tmpPaymentId = 0;
                    long tmpMemberPaymentId = 0;
                    guestBillPaymentBO.CreatedBy = userInformationBO.UserInfoId;
                    memPaymentLedgerBO.CreatedBy = userInformationBO.UserInfoId;
                    status = reservationBillPaymentDA.SaveGuestBillPaymentInfo(guestBillPaymentBO, out tmpPaymentId, "Member");
                    status = memPaymentDA.SaveMemberPaymentLedgerInfo(memPaymentLedgerBO, out tmpMemberPaymentId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestBillPayment.ToString(), tmpPaymentId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestBillPayment));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        this.Cancel();
                    }
                }
                else
                {
                    Boolean status = false;
                    memPaymentLedgerBO.MemberPaymentId = Convert.ToInt32(hfMemberPaymentId.Value);
                    memPaymentLedgerBO.LastModifiedBy = userInformationBO.UserInfoId;
                    
                    status = memPaymentDA.UpdateMemberPaymentLedgerInfo(memPaymentLedgerBO);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GuestBillPayment.ToString(), guestBillPaymentBO.DealId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestBillPayment));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        this.Cancel();
                    }
                }
            }
            SetTab("EntryTab");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            btnGroupPaymentPreview.Visible = true;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            MemberPaymentDA memberDA = new MemberPaymentDA();
            List<PMMemberPaymentLedgerBO> paymentList = new List<PMMemberPaymentLedgerBO>();

            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                fromDate = CommonHelper.DateTimeToMMDDYYYY(txtFromDate.Text);
            }

            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                toDate = CommonHelper.DateTimeToMMDDYYYY(txtToDate.Text);
            }

            paymentList = memberDA.GetMemberPaymentLedger(fromDate, toDate, string.Empty, false);
            if (paymentList.Count > 0)
            {
                gvGuestHouseService.DataSource = paymentList;
                gvGuestHouseService.DataBind();
                gvPaymentInfo.DataSource = paymentList;
                gvPaymentInfo.DataBind();
            }
            else
            {
                gvGuestHouseService.DataSource = null;
                gvGuestHouseService.DataBind();
                gvPaymentInfo.DataSource = null;
                gvPaymentInfo.DataBind();
            }
            SetTab("SearchTab");
            isSearchPanelEnable = 1;
        }
        //************************ User Defined Function ********************//
        private void LoadBank()
        {
            BankDA bankDA = new BankDA();
            List<BankBO> entityBOList = new List<BankBO>();
            entityBOList = bankDA.GetBankInfo();

            this.ddlBankId.DataSource = entityBOList;
            this.ddlBankId.DataTextField = "BankName";
            this.ddlBankId.DataValueField = "BankId";
            this.ddlBankId.DataBind();

            this.ddlCompanyBank.DataSource = entityBOList;
            this.ddlCompanyBank.DataTextField = "BankName";
            this.ddlCompanyBank.DataValueField = "BankId";
            this.ddlCompanyBank.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBankId.Items.Insert(0, itemBank);
            this.ddlCompanyBank.Items.Insert(0, itemBank);
        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("All");

            this.ddlCurrency.DataSource = currencyListBO;
            this.ddlCurrency.DataTextField = "CurrencyName";
            this.ddlCurrency.DataValueField = "CurrencyId";
            this.ddlCurrency.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCurrency.Items.Insert(0, item);
        }
        private void LoadIsConversionRateEditable()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsConversionRateEditable", "IsConversionRateEditable");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        this.txtConversionRate.ReadOnly = true;
                    }
                    else
                    {
                        this.txtConversionRate.ReadOnly = false;
                    }
                }
            }
        }
        private void IsLocalCurrencyDefaultSelected()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsLocalCurrencyDefaultSelected", "IsLocalCurrencyDefaultSelected");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        CommonCurrencyDA headDA = new CommonCurrencyDA();
                        List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
                        currencyListBO = headDA.GetConversionHeadInfoByType("All");

                        ddlCurrency.DataSource = currencyListBO;
                        ddlCurrency.DataTextField = "CurrencyName";
                        ddlCurrency.DataValueField = "CurrencyId";
                        ddlCurrency.DataBind();

                        CommonCurrencyBO currencyBO = currencyListBO.Where(x => x.CurrencyType == "Local").SingleOrDefault();
                        ddlCurrency.SelectedValue = currencyBO.CurrencyId.ToString();
                    }
                }
            }
        }
        private void LoadLocalCurrencyId()
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetLocalCurrencyInfo();
            LocalCurrencyId = commonCurrencyBO.CurrencyId;
        }
        private void SearchMember()
        {
            MemMemberBasicsBO memberBO = new MemMemberBasicsBO();
            MemMemberBasicDA memberDA = new MemMemberBasicDA();
            if (!string.IsNullOrEmpty(txtSrcMemberId.Text))
            {
                memberBO = memberDA.GetMemberInfoByMembershipNo(txtSrcMemberId.Text);
            }

            if (memberBO == null)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Member Id.", AlertType.Warning);
                this.txtSrcMemberId.Focus();
            }
            else txtMemberName.Text = memberBO.FullName;
        }
        private void LoadRelatedInformation()
        {
            if (ddlRoomId.SelectedIndex != -1)
            {
                int roomId = Convert.ToInt32(this.ddlRoomId.SelectedValue);
            }
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmGuestBillPayment.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (ddlPayMode.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Select Payment Mode.", AlertType.Warning);
                this.ddlPayMode.Focus();
                flag = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtPaymentDate2.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Payment Date.", AlertType.Warning);
                this.txtPaymentDate2.Focus();
                flag = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text))
            {
                this.isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Payment Amount.", AlertType.Warning);
                this.txtLedgerAmount.Focus();
                flag = false;
            }
            else if (this.ddlCurrency.SelectedValue == "0")
            {
                this.isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, "Please Select Currency Type.", AlertType.Warning);
                flag = false;
            }
            return flag;
        }
        private void LoadGridView(string reservationId)
        {
            this.CheckObjectPermission();
            if (reservationId == "-1")
            {
                isSearchPanelEnable = -1;
                return;
            }
            if (Convert.ToInt32(reservationId) > 0)
            {
                GuestBillPaymentDA da = new GuestBillPaymentDA();
                List<GuestBillPaymentBO> files = da.GetGuestBillPaymentInfoByRegistrationId("FrontOffice", Convert.ToInt32(reservationId));
                isSearchPanelEnable = 1;
                this.gvGuestHouseService.DataSource = files;
                this.gvGuestHouseService.DataBind();
                this.gvPaymentInfo.DataSource = files;
                this.gvPaymentInfo.DataBind();

                if (files.Count() > 0)
                {
                    this.btnPaymentPreview.Visible = true;
                    this.btnGroupPaymentPreview.Visible = true;
                }
                else
                {
                    this.btnPaymentPreview.Visible = false;
                    this.btnGroupPaymentPreview.Visible = false;
                }
            }
            else
            {
                isSearchPanelEnable = -1;
                this.gvGuestHouseService.DataSource = null;
                this.gvGuestHouseService.DataBind();
                this.gvPaymentInfo.DataSource = null;
                this.gvPaymentInfo.DataBind();
                this.btnPaymentPreview.Visible = false;
                this.btnGroupPaymentPreview.Visible = false;
            }
        }
        private void LoadSingleProjectAndCompany()
        {
            this.LoadGLCompany(true);
            this.LoadGLProject(true);
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
        }
        private void Cancel()
        {
            this.txtLedgerAmount.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.hfMemberPaymentId.Value = string.Empty;
            this.txtDealId.Value = string.Empty;
            this.txtCalculatedLedgerAmount.Text = string.Empty;
            this.txtCalculatedLedgerAmountHiddenField.Value = string.Empty;
            this.txtConversionRate.Text = string.Empty;
            this.ddlPayMode.SelectedIndex = 0;
            this.ClearCommonSessionInformation();
            this.txtRemarks.Text = string.Empty;
            this.ddlBankId.SelectedValue = "0";
            this.txtSrcMemberId.Focus();
            hfConversionRate.Value = "";
        }
        private void LoadAccountHeadInfo()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            this.lblPaymentAccountHead.Text = "Payment Receive In";
            CustomFieldBO CashReceiveAccountsInfo = new CustomFieldBO();
            CashReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CashReceiveAccountsInfo");

            this.ddlCashReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CashReceiveAccountsInfo.FieldValue.ToString() + ") AND NodeId != 8");
            this.ddlCashReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCashReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCashReceiveAccountsInfo.DataBind();

            this.ddlPaymentFromAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CashReceiveAccountsInfo.FieldValue.ToString() + ") AND NodeId != 8");
            this.ddlPaymentFromAccountsInfo.DataTextField = "NodeHead";
            this.ddlPaymentFromAccountsInfo.DataValueField = "NodeId";
            this.ddlPaymentFromAccountsInfo.DataBind();

            CustomFieldBO PaymentToAccountsInfo = new CustomFieldBO();
            PaymentToAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("PaymentToCustomerForCashOut");
            this.ddlPaymentToAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + PaymentToAccountsInfo.FieldValue.ToString() + ") AND NodeId != 8");
            this.ddlPaymentToAccountsInfo.DataTextField = "NodeHead";
            this.ddlPaymentToAccountsInfo.DataValueField = "NodeId";
            this.ddlPaymentToAccountsInfo.DataBind();

            CustomFieldBO CardReceiveAccountsInfo = new CustomFieldBO();
            CardReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CardReceiveAccountsInfo");
            this.ddlCardReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CardReceiveAccountsInfo.FieldValue.ToString() + ") AND NodeId != 8");
            this.ddlCardReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCardReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCardReceiveAccountsInfo.DataBind();

            CustomFieldBO ChequeReceiveAccountsInfo = new CustomFieldBO();
            ChequeReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("ChequeReceiveAccountsInfo");
            this.ddlChequeReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + ChequeReceiveAccountsInfo.FieldValue.ToString() + ") AND NodeId != 8");
            this.ddlChequeReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlChequeReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlChequeReceiveAccountsInfo.DataBind();

            CustomFieldBO IncomeSourceAccountsInfo = new CustomFieldBO();
            IncomeSourceAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("IncomeSourceAccountsInfo");
            this.ddlIncomeSourceAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + IncomeSourceAccountsInfo.FieldValue.ToString() + ") AND NodeId != 8");
            this.ddlIncomeSourceAccountsInfo.DataTextField = "NodeHead";
            this.ddlIncomeSourceAccountsInfo.DataValueField = "NodeId";
            this.ddlIncomeSourceAccountsInfo.DataBind();

            CustomFieldBO CompanyPaymentAccountsInfo = new CustomFieldBO();
            CompanyPaymentAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CompanyPaymentAccountsInfo");
            this.ddlCompanyPaymentAccountHead.DataSource = entityDA.GetNodeMatrixInfoByAncestorNodeId(Convert.ToInt32(CompanyPaymentAccountsInfo.FieldValue));
            this.ddlCompanyPaymentAccountHead.DataTextField = "NodeHead";
            this.ddlCompanyPaymentAccountHead.DataValueField = "NodeId";
            this.ddlCompanyPaymentAccountHead.DataBind();
        }
        private void AdvanceVoucherPost(out int tmpGLMasterId)
        {
            List<GLLedgerBO> ledgerDetailListBOForDebit = new List<GLLedgerBO>();

            GLLedgerBO detailBODebit = new GLLedgerBO();
            detailBODebit.LedgerId = 0;
            if (this.ddlPayMode.SelectedIndex == 0)
            {
                detailBODebit.NodeId = Convert.ToInt32(this.ddlCashReceiveAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = this.ddlCashReceiveAccountsInfo.SelectedItem.Text;
            }
            else if (this.ddlPayMode.SelectedIndex == 1)
            {
                detailBODebit.NodeId = Convert.ToInt32(this.ddlCardReceiveAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = this.ddlCardReceiveAccountsInfo.SelectedItem.Text;
            }
            else if (this.ddlPayMode.SelectedIndex == 2)
            {
                detailBODebit.NodeId = Convert.ToInt32(this.ddlChequeReceiveAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = this.ddlChequeReceiveAccountsInfo.SelectedItem.Text;
            }
            detailBODebit.LedgerMode = 1;
            detailBODebit.FieldId = Convert.ToInt32(this.ddlCurrency.SelectedValue); //45 for Local Currency

            if (hfCurrencyType.Value != "Local")
            {
                decimal mConvertionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
                decimal mCurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBODebit.CurrencyAmount = mCurrencyAmount;
                detailBODebit.LedgerAmount = mCurrencyAmount * mConvertionRate;
                detailBODebit.LedgerDebitAmount = mCurrencyAmount * mConvertionRate;
            }
            else
            {
                detailBODebit.CurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBODebit.LedgerAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBODebit.LedgerDebitAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
            }

            detailBODebit.NodeNarration = "";
            ledgerDetailListBOForDebit.Add(detailBODebit);
            Session["TransactionDetailList"] = ledgerDetailListBOForDebit;

            List<GLLedgerBO> ledgerDetailListBO = Session["TransactionDetailList"] == null ? new List<GLLedgerBO>() : Session["TransactionDetailList"] as List<GLLedgerBO>;
            //Oppusite Head (Payment Head Information)
            GLLedgerBO detailBOCredit = new GLLedgerBO();
            detailBOCredit.LedgerId = 1;
            detailBOCredit.NodeId = Convert.ToInt32(this.ddlIncomeSourceAccountsInfo.SelectedValue);
            detailBOCredit.NodeHead = this.ddlIncomeSourceAccountsInfo.SelectedItem.Text;
            detailBOCredit.LedgerMode = 2;
            detailBOCredit.FieldId = Convert.ToInt32(this.ddlCurrency.SelectedValue);

            if (hfCurrencyType.Value != "Local")
            {
                decimal mConvertionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
                decimal mCurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBOCredit.CurrencyAmount = mCurrencyAmount;
                detailBOCredit.LedgerAmount = mCurrencyAmount * mConvertionRate;
                detailBOCredit.LedgerDebitAmount = mCurrencyAmount * mConvertionRate;
            }
            else
            {
                detailBOCredit.CurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBOCredit.LedgerAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBOCredit.LedgerDebitAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
            }

            detailBOCredit.NodeNarration = "";
            ledgerDetailListBO.Add(detailBOCredit);
            Session["TransactionDetailList"] = ledgerDetailListBO;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GLDealMasterDA glMasterDA = new GLDealMasterDA();
            GLDealMasterBO glMasterBO = new GLDealMasterBO();

            Boolean isCash = true;
            if (this.ddlPayMode.SelectedItem.Text != "Cash")
            {
                isCash = false;
            }
            if (isCash)
            {
                glMasterBO.CashChequeMode = 1;
                glMasterBO.VoucherType = "CR";
            }
            else
            {
                glMasterBO.CashChequeMode = 2;
                glMasterBO.VoucherType = "BR";
            }

            glMasterBO.ProjectId = Convert.ToInt32(this.ddlGLProject.SelectedValue);


            if (this.ddlPayMode.SelectedIndex == 0)
            {
                glMasterBO.VoucherMode = 2;
            }
            else
            {
                glMasterBO.VoucherMode = 3;
            }

            glMasterBO.VoucherDate = DateTime.Now;
            glMasterBO.PayerOrPayee = "";

            // Voucher Approved Information ------------------------------------------------------------------------------------Start
            HMCommonSetupBO commonSetupBOCheckedBy = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOApprovedBy = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBOCheckedBy = commonSetupDA.GetCommonConfigurationInfo("VoucherApproveSystem", "CheckedBySetup");
            commonSetupBOApprovedBy = commonSetupDA.GetCommonConfigurationInfo("VoucherApproveSystem", "ApprovedBySetup");
            List<GLVoucherApprovedInfoBO> approvedBOList = new List<GLVoucherApprovedInfoBO>();
            // CheckedBy -----------------
            if (commonSetupBOCheckedBy != null)
            {
                if (commonSetupBOCheckedBy.SetupId > 0)
                {
                    GLVoucherApprovedInfoBO approvedBOCheckedBy = new GLVoucherApprovedInfoBO();
                    approvedBOCheckedBy.ApprovedType = "CheckedBy";
                    approvedBOCheckedBy.UserInfoId = Convert.ToInt32(commonSetupBOCheckedBy.SetupValue);
                    approvedBOList.Add(approvedBOCheckedBy);
                }
            }

            // ApprovedBy -----------------
            if (commonSetupBOApprovedBy != null)
            {
                if (commonSetupBOApprovedBy.SetupId > 0)
                {
                    GLVoucherApprovedInfoBO approvedBOApprovedBy = new GLVoucherApprovedInfoBO();
                    approvedBOApprovedBy.ApprovedType = "ApprovedBy";
                    approvedBOApprovedBy.UserInfoId = Convert.ToInt32(commonSetupBOApprovedBy.SetupValue);
                    approvedBOList.Add(approvedBOApprovedBy);
                }
            }
            // Voucher Approved Information ------------------------------------------------------------------------------------End


            string currentVoucherNo = string.Empty;
            glMasterBO.CreatedBy = userInformationBO.UserInfoId;
            Boolean status = glMasterDA.SaveGLMasterInfo(glMasterBO, out tmpGLMasterId, out currentVoucherNo, Session["TransactionDetailList"] as List<GLLedgerBO>, approvedBOList);
            this.txtLedgerAmount.Text = "0";
        }
        private void CashOutVoucherPost(out int tmpGLMasterId)
        {
            List<GLLedgerBO> ledgerDetailListBOForDebit = new List<GLLedgerBO>();

            GLLedgerBO detailBODebit = new GLLedgerBO();
            detailBODebit.LedgerId = 0;
            if (this.ddlPayMode.SelectedIndex == 0)
            {
                detailBODebit.NodeId = Convert.ToInt32(this.ddlPaymentToAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = this.ddlPaymentToAccountsInfo.SelectedItem.Text;
            }
            else if (this.ddlPayMode.SelectedIndex == 1)
            {
                detailBODebit.NodeId = Convert.ToInt32(this.ddlPaymentToAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = this.ddlPaymentToAccountsInfo.SelectedItem.Text;
            }
            else if (this.ddlPayMode.SelectedIndex == 2)
            {
                detailBODebit.NodeId = Convert.ToInt32(this.ddlPaymentToAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = this.ddlPaymentToAccountsInfo.SelectedItem.Text;
            }
            detailBODebit.LedgerMode = 1;
            detailBODebit.FieldId = Convert.ToInt32(this.ddlCurrency.SelectedValue); //45 for Local Currency

            if (hfCurrencyType.Value != "Local")
            {
                decimal mConvertionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
                decimal mCurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBODebit.CurrencyAmount = mCurrencyAmount;
                detailBODebit.LedgerAmount = mCurrencyAmount * mConvertionRate;
                detailBODebit.LedgerDebitAmount = mCurrencyAmount * mConvertionRate;
            }
            else
            {
                detailBODebit.CurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBODebit.LedgerAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBODebit.LedgerDebitAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
            }

            detailBODebit.NodeNarration = "";
            ledgerDetailListBOForDebit.Add(detailBODebit);
            Session["TransactionDetailList"] = ledgerDetailListBOForDebit;

            List<GLLedgerBO> ledgerDetailListBO = Session["TransactionDetailList"] == null ? new List<GLLedgerBO>() : Session["TransactionDetailList"] as List<GLLedgerBO>;
            //Oppusite Head (Payment Head Information)
            GLLedgerBO detailBOCredit = new GLLedgerBO();
            detailBOCredit.LedgerId = 1;
            detailBOCredit.NodeId = Convert.ToInt32(this.ddlPaymentFromAccountsInfo.SelectedValue);
            detailBOCredit.NodeHead = this.ddlPaymentFromAccountsInfo.SelectedItem.Text;
            detailBOCredit.LedgerMode = 2;
            detailBOCredit.FieldId = Convert.ToInt32(this.ddlCurrency.SelectedValue);

            if (hfCurrencyType.Value != "Local")
            {
                decimal mConvertionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
                decimal mCurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBOCredit.CurrencyAmount = mCurrencyAmount;
                detailBOCredit.LedgerAmount = mCurrencyAmount * mConvertionRate;
                detailBOCredit.LedgerDebitAmount = mCurrencyAmount * mConvertionRate;
            }
            else
            {
                detailBOCredit.CurrencyAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBOCredit.LedgerAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
                detailBOCredit.LedgerDebitAmount = !string.IsNullOrWhiteSpace(this.txtLedgerAmount.Text) ? Convert.ToDecimal(this.txtLedgerAmount.Text) : 0;
            }

            detailBOCredit.NodeNarration = "";
            ledgerDetailListBO.Add(detailBOCredit);
            Session["TransactionDetailList"] = ledgerDetailListBO;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GLDealMasterDA glMasterDA = new GLDealMasterDA();
            GLDealMasterBO glMasterBO = new GLDealMasterBO();

            glMasterBO.CashChequeMode = 1;
            glMasterBO.VoucherType = "CP";
            glMasterBO.ProjectId = Convert.ToInt32(this.ddlGLProject.SelectedValue);
            glMasterBO.VoucherMode = 1;

            glMasterBO.VoucherDate = DateTime.Now;
            glMasterBO.PayerOrPayee = "";

            // Voucher Approved Information ------------------------------------------------------------------------------------Start
            HMCommonSetupBO commonSetupBOCheckedBy = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBOApprovedBy = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBOCheckedBy = commonSetupDA.GetCommonConfigurationInfo("VoucherApproveSystem", "CheckedBySetup");
            commonSetupBOApprovedBy = commonSetupDA.GetCommonConfigurationInfo("VoucherApproveSystem", "ApprovedBySetup");
            List<GLVoucherApprovedInfoBO> approvedBOList = new List<GLVoucherApprovedInfoBO>();
            // CheckedBy -----------------
            if (commonSetupBOCheckedBy != null)
            {
                if (commonSetupBOCheckedBy.SetupId > 0)
                {
                    GLVoucherApprovedInfoBO approvedBOCheckedBy = new GLVoucherApprovedInfoBO();
                    approvedBOCheckedBy.ApprovedType = "CheckedBy";
                    approvedBOCheckedBy.UserInfoId = Convert.ToInt32(commonSetupBOCheckedBy.SetupValue);
                    approvedBOList.Add(approvedBOCheckedBy);
                }
            }

            // ApprovedBy -----------------
            if (commonSetupBOApprovedBy != null)
            {
                if (commonSetupBOApprovedBy.SetupId > 0)
                {
                    GLVoucherApprovedInfoBO approvedBOApprovedBy = new GLVoucherApprovedInfoBO();
                    approvedBOApprovedBy.ApprovedType = "ApprovedBy";
                    approvedBOApprovedBy.UserInfoId = Convert.ToInt32(commonSetupBOApprovedBy.SetupValue);
                    approvedBOList.Add(approvedBOApprovedBy);
                }
            }
            // Voucher Approved Information ------------------------------------------------------------------------------------End


            string currentVoucherNo = string.Empty;
            glMasterBO.CreatedBy = userInformationBO.UserInfoId;
            Boolean status = glMasterDA.SaveGLMasterInfo(glMasterBO, out tmpGLMasterId, out currentVoucherNo, Session["TransactionDetailList"] as List<GLLedgerBO>, approvedBOList);
            this.txtLedgerAmount.Text = "0";
        }
        private void SearchInformation()
        {
            this.CheckObjectPermission();
            SearchMember();
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        private void ClearCommonSessionInformation()
        {
            Session["TransactionDetailList"] = null;
        }
        //************************ User Defined Function ********************//       
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
        [WebMethod]
        public static string DeleteData(int sEmpId)
        {
            string result = string.Empty;
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("HotelGuestBillPayment", "PaymentId", sEmpId);
                if (status)
                {
                    result = "success";
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Data Deleted Failed.";
                throw ex;
            }

            return result;
        }
        [WebMethod]
        public static PMMemberPaymentLedgerBO FillForm(int EditId)
        {
            MemberPaymentDA memberDA = new MemberPaymentDA();
            PMMemberPaymentLedgerBO paymentBO = new PMMemberPaymentLedgerBO();

            paymentBO = memberDA.GetMemberPaymentLedgerById(EditId);

            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetCommonCurrencyInfoById(paymentBO.CurrencyId);
            paymentBO.CurrencyType = commonCurrencyBO.CurrencyType;

            return paymentBO;
        }
        [WebMethod]
        public static CommonCurrencyBO LoadCurrencyType(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetCommonCurrencyInfoById(currecyType);
            return commonCurrencyBO;
        }
        [WebMethod]
        public static CommonCurrencyConversionBO LoadCurrencyConversionRate(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyConversionBO conversionBO = new CommonCurrencyConversionBO();
            conversionBO = commonCurrencyDA.GetCurrencyConversionRate(currecyType);
            return conversionBO;
        }
    }
}