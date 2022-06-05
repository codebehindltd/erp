using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.HMCommon;
using System.Collections;
using HotelManagement.Entity.HMCommon;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.SalesManagment
{
    public partial class frmPMSalesBillPayment : System.Web.UI.Page
    {
        protected bool isSingle = true;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        protected int isCompanyProjectPanelEnable = -1;
        protected int isIntegratedGeneralLedgerDiv = 1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            isSingle = hmUtility.GetSingleProjectAndCompany();
            if (!IsPostBack)
            {
                this.ddlPaymentMode.Enabled = true;
                this.ddlPaymentAccountHead.Enabled = true;
                Session["TransactionDetailList"] = null;
                //this.LoadGLCompany();
                LoadAccountHeadInfo();
                LoadBank();
                this.LoadIncomeAccountHead();
                LoadCashReceiveAccountsInfo();
                this.LoadGLProject();
                this.LoadCommonDropDownHiddenField();
                string cardValidation = System.Web.Configuration.WebConfigurationManager.AppSettings["CardValidation"].ToString();
                if (isSingle == true)
                {
                    isCompanyProjectPanelEnable = 1;
                    LoadSingleProjectAndCompany();
                    this.GLCompanyAndProjectInformation.Visible = false;
                }
                else
                {
                    this.LoadGLCompany(false);
                    this.LoadGLProject(false);
                    this.GLCompanyAndProjectInformation.Visible = true;
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadInvoiceDetail();
        }
        protected void btnReceive_Click(object sender, EventArgs e)
        {
            //if (!isValidForm())
            //{
            //    return;
            //}

            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            //PMSalesBillPaymentBO paymentBO = new PMSalesBillPaymentBO();
            //PMSalesBillPaymentDA paymentDA = new PMSalesBillPaymentDA();
            //paymentBO.CustomerId = Int32.Parse(txtCustomerId.Value.ToString());
            //paymentBO.NodeId = Convert.ToInt32(this.ddlPaymentAccountHead.SelectedValue);
            //paymentBO.CurrencyAmount = Convert.ToDecimal(txtReceiveAmount.Text);
            //if (!string.IsNullOrEmpty(txtConvertionRate.Text))
            //{
            //    paymentBO.PaymentLocalAmount = paymentBO.CurrencyAmount * (Convert.ToDecimal(txtConvertionRate.Text));
            //}
            //else
            //{
            //    paymentBO.PaymentLocalAmount = paymentBO.CurrencyAmount;
            //}
            //paymentBO.FieldId = Convert.ToInt32(txtHiddenFieldId.Value);
            //paymentBO.PaymentDate = DateTime.Now;

            //if (string.IsNullOrWhiteSpace(txtPaymentId.Value))
            //{
            //    paymentBO.CreatedBy = userInformationBO.UserInfoId;
            //    int tmpPaymentId = 0;
            //    Boolean status = paymentDA.SaveSalesBillPaymentInfo(paymentBO, out tmpPaymentId);
            //    if (status)
            //    {
            //        int tmpGLMasterId;
            //        this.isMessageBoxEnable = 2;
            //        lblMessage.Text = "Saved Operation Successfull";
            //        this.VoucherPost(paymentBO.PaymentLocalAmount, paymentBO.CurrencyAmount, out tmpGLMasterId);

            //        this.ddlPaymentMode.Enabled = true;
            //        this.ddlPaymentAccountHead.Enabled = true;

            //        HMCommonDA hmCommonDA = new HMCommonDA();
            //        Boolean postingStatus = hmCommonDA.UpdateVoucherPostTableForDealId("PMSalesBillPayment", "PaymentId", tmpPaymentId, tmpGLMasterId);

            //        this.LoadInvoiceDetail();
            //    }
            //}

            PMSalesBillPaymentBO paymentBO = new PMSalesBillPaymentBO();
            PMSalesBillPaymentDA paymentDA = new PMSalesBillPaymentDA();
            Boolean status = paymentDA.SaveBillPaymentInfo(Session["GuestPaymentDetailListForGrid"] as List<PMSalesBillPaymentBO>, Int32.Parse(txtCustomerId.Value));
            if (status)
            {
                this.isMessageBoxEnable = 2;
                lblMessage.Text = "Saved Operation Successfull";
            }
        }
        protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadAccountHeadInfo();
        }
        protected void btnAddDetail_Click(object sender, EventArgs e)
        {
            if (!IsDetailFormValid())
            {
                return;
            }
            this.ddlPaymentMode.Enabled = false;
            this.ddlPaymentAccountHead.Enabled = false;
            //--- Credit Part-------------------------------------------------------------------
            int dynamicDetailId = 0;
            List<GLLedgerBO> reservationDetailListBO = Session["TransactionDetailList"] == null ? new List<GLLedgerBO>() : Session["TransactionDetailList"] as List<GLLedgerBO>;

            if (!string.IsNullOrWhiteSpace(lblHiddenId.Text))
                dynamicDetailId = Convert.ToInt32(lblHiddenId.Text);

            GLLedgerBO detailBO = dynamicDetailId == 0 ? new GLLedgerBO() : reservationDetailListBO.Where(x => x.LedgerId == dynamicDetailId).FirstOrDefault();
            if (reservationDetailListBO.Contains(detailBO))
                reservationDetailListBO.Remove(detailBO);

            detailBO.NodeId = Convert.ToInt32(this.ddlIncomeAccountHead.SelectedValue);
            detailBO.NodeHead = this.ddlIncomeAccountHead.SelectedItem.Text;
            detailBO.LedgerMode = 2;
            detailBO.FieldId = Convert.ToInt32(45);
            detailBO.LedgerCreditAmount = !string.IsNullOrWhiteSpace(this.txtReceiveAmount.Text) ? Convert.ToDecimal(this.txtReceiveAmount.Text) : 0;
            detailBO.CurrencyAmount = null;
            detailBO.LedgerAmount = !string.IsNullOrWhiteSpace(this.txtReceiveAmount.Text) ? Convert.ToDecimal(this.txtReceiveAmount.Text) : 0;
            detailBO.NodeNarration = string.Empty;
            detailBO.LedgerId = dynamicDetailId == 0 ? reservationDetailListBO.Count + 1 : dynamicDetailId;
            reservationDetailListBO.Add(detailBO);
            Session["TransactionDetailList"] = reservationDetailListBO;

            this.gvDetail.DataSource = Session["TransactionDetailList"] as List<GLLedgerBO>;
            this.gvDetail.DataBind();

            //--- Debit Part-------------------------------------------------------------------
            List<GLLedgerBO> rpReservationDetailListBO = Session["TransactionDetailList"] == null ? new List<GLLedgerBO>() : Session["TransactionDetailList"] as List<GLLedgerBO>;
            int rpDynamicDetailId = 0;
            if (!string.IsNullOrWhiteSpace(this.ddlPaymentAccountHead.SelectedValue))
            {
                rpDynamicDetailId = Convert.ToInt32(this.ddlPaymentAccountHead.SelectedValue);
            }

            GLLedgerBO rpDetailBO = rpDynamicDetailId == 0 ? new GLLedgerBO() : rpReservationDetailListBO.Where(x => x.NodeId == rpDynamicDetailId).FirstOrDefault();
            if (rpDetailBO == null)
            {
                rpDetailBO = new GLLedgerBO();
            }

            if (rpReservationDetailListBO.Contains(rpDetailBO))
                rpReservationDetailListBO.Remove(rpDetailBO);

            decimal tmpLedgerAmount = CalculateAmountTotal(2);
            rpDetailBO.NodeId = Convert.ToInt32(this.ddlPaymentAccountHead.SelectedValue);
            rpDetailBO.NodeHead = this.ddlPaymentAccountHead.SelectedItem.Text;
            rpDetailBO.LedgerMode = 1;
            rpDetailBO.LedgerAmount = tmpLedgerAmount;

            rpDetailBO.LedgerDebitAmount = tmpLedgerAmount;

            rpDetailBO.LedgerId = rpDynamicDetailId == 0 ? rpReservationDetailListBO.Count + 1 : rpDynamicDetailId;
            rpReservationDetailListBO.Add(rpDetailBO);
            Session["TransactionDetailList"] = rpReservationDetailListBO;

            this.gvDetail.DataSource = Session["TransactionDetailList"] as List<GLLedgerBO>;
            this.gvDetail.DataBind();
        }
        protected void gvDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _selectedNodeId;
            if (e.CommandName == "CmdDelete")
            {
                _selectedNodeId = Convert.ToInt32(e.CommandArgument.ToString());

                var DetailBO = (List<GLLedgerBO>)Session["TransactionDetailList"];
                var voucherDetail = DetailBO.Where(x => x.NodeId == _selectedNodeId).FirstOrDefault();
                var currentLedgerId = DetailBO.Where(x => x.NodeId == _selectedNodeId).FirstOrDefault().LedgerId;
                DetailBO.Remove(voucherDetail);
                Session["TransactionDetailList"] = DetailBO;
                if (DetailBO != null)
                {
                    if (DetailBO.Count == 1)
                    {
                        Session["TransactionDetailList"] = null;
                    }
                    else
                    {
                        List<GLLedgerBO> rpReservationDetailListBO = Session["TransactionDetailList"] == null ? new List<GLLedgerBO>() : Session["TransactionDetailList"] as List<GLLedgerBO>;
                        int rpDynamicDetailId = 0;
                        if (!string.IsNullOrWhiteSpace(this.ddlPaymentAccountHead.SelectedValue))
                        {
                            rpDynamicDetailId = Convert.ToInt32(this.ddlPaymentAccountHead.SelectedValue);
                        }

                        GLLedgerBO rpDetailBO = rpDynamicDetailId == 0 ? new GLLedgerBO() : rpReservationDetailListBO.Where(x => x.NodeId == rpDynamicDetailId).FirstOrDefault();
                        if (rpDetailBO == null)
                        {
                            rpDetailBO = new GLLedgerBO();
                        }

                        if (rpReservationDetailListBO.Contains(rpDetailBO))
                        {
                            rpReservationDetailListBO.Remove(rpDetailBO);
                            Session["TransactionDetailList"] = rpReservationDetailListBO;

                            this.gvDetail.DataSource = Session["TransactionDetailList"] as List<GLLedgerBO>;
                            this.gvDetail.DataBind();
                        }

                        decimal tmpLedgerAmount = CalculateAmountTotal(2);
                        rpDetailBO.NodeId = Convert.ToInt32(this.ddlPaymentAccountHead.SelectedValue);
                        rpDetailBO.NodeHead = this.ddlPaymentAccountHead.SelectedItem.Text;
                        rpDetailBO.LedgerMode = 1;
                        rpDetailBO.LedgerAmount = tmpLedgerAmount;

                        rpDetailBO.LedgerDebitAmount = tmpLedgerAmount;

                        rpDetailBO.LedgerId = rpDynamicDetailId == 0 ? rpReservationDetailListBO.Count + 1 : rpDynamicDetailId;
                        rpReservationDetailListBO.Add(rpDetailBO);
                        Session["TransactionDetailList"] = rpReservationDetailListBO;
                    }
                }

                this.gvDetail.DataSource = Session["TransactionDetailList"] as List<GLLedgerBO>;
                this.gvDetail.DataBind();
            }
        }
        protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");

                if (this.ddlPaymentAccountHead.SelectedValue.Equals(lblValue.Text))
                {
                    imgDelete.Visible = false;
                }
                else
                {
                    imgDelete.Visible = true;
                }
            }
        }
        //************************ User Defined Function ********************//
        private void LoadBank()
        {
            BankDA bankDA = new BankDA();
            var DataSource = bankDA.GetBankInfo();
            this.ddlBankName.DataSource = DataSource;
            this.ddlBankName.DataTextField = "BankName";
            this.ddlBankName.DataValueField = "BankId";
            this.ddlBankName.DataBind();

            this.ddlBankId.DataSource = DataSource;
            this.ddlBankId.DataTextField = "BankName";
            this.ddlBankId.DataValueField = "BankId";
            this.ddlBankId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBankName.Items.Insert(0, itemBank);
            this.ddlBankId.Items.Insert(0, itemBank);
        }
        public bool isValidForm()
        {
            bool status = true;
            List<GLLedgerBO> listDetailBO = new List<GLLedgerBO>();
            listDetailBO = Session["TransactionDetailList"] as List<GLLedgerBO>;
            if (string.IsNullOrWhiteSpace(this.txtCustomerId.Value))
            {
                status = false;
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Provide Customer Information";
                this.txtInvoiceNumber.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtReceiveAmount.Text))
            {
                status = false;
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Provide Receive Amount";
                this.txtReceiveAmount.Focus();
            }
            else if (isSingle == false)
            {
                if (this.ddlGLCompany.SelectedValue == "0")
                {
                    Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                    if (isIntegrated)
                    {
                        this.isMessageBoxEnable = 1;
                        lblMessage.Text = "Please Select Company Name.";
                        this.ddlGLCompany.Focus();
                        status = false;
                    }
                }
                else if (this.ddlGLCompany.SelectedValue != "0")
                {
                    if (this.ddlGLProject.SelectedValue == "0")
                    {
                        Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                        if (isIntegrated)
                        {
                            this.isMessageBoxEnable = 1;
                            lblMessage.Text = "Please select Project Name.";
                            this.ddlGLProject.Focus();
                            status = false;
                        }
                    }
                }
            }
            //else if (this.ddlGLProject.SelectedIndex == 0)
            //{
            //    this.isMessageBoxEnable = 1;
            //    lblMessage.Text = "Please Select Project Name";
            //    this.ddlGLProject.Focus();
            //    status = false;
            //}

            if (listDetailBO != null)
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
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Your Entered Debit Amount and Credit Amount are not same.";
                    status = false;
                }

            }
            return status;
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
        //private void LoadGLCompany()
        //{
        //    GLCompanyDA entityDA = new GLCompanyDA();
        //    this.ddlGLCompany.DataSource = entityDA.GetAllGLCompanyInfo();
        //    this.ddlGLCompany.DataTextField = "Name";
        //    this.ddlGLCompany.DataValueField = "CompanyId";
        //    this.ddlGLCompany.DataBind();

        //    ListItem itemCompany = new ListItem();
        //    itemCompany.Value = "0";
        //    itemCompany.Text = hmUtility.GetDropDownFirstValue();
        //    this.ddlGLCompany.Items.Insert(0, itemCompany);
        //}

        private void LoadGLProject()
        {
            GLProjectDA entityDA = new GLProjectDA();
            this.ddlGLProject.DataSource = entityDA.GetAllGLProjectInfo();
            this.ddlGLProject.DataTextField = "Name";
            this.ddlGLProject.DataValueField = "ProjectId";
            this.ddlGLProject.DataBind();

            ListItem itemProject = new ListItem();
            itemProject.Value = "0";
            itemProject.Text = hmUtility.GetDropDownFirstValue();
            this.ddlGLProject.Items.Insert(0, itemProject);

        }
        private void LoadAccountHeadInfo()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            if (this.ddlPaymentMode.SelectedIndex == 0)
            {
                this.lblPaymentAccountHead.Text = "Payment Through Cash";
                //this.ddlPaymentAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + Application["CashAccountInfoForSalesBillPayment"].ToString() + ") AND NodeId != 8");
                CustomFieldBO CashReceiveAccountsInfo = new CustomFieldBO();
                CashReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CashReceiveAccountsInfo");

                this.ddlPaymentAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CashReceiveAccountsInfo.FieldValue.ToString() + ")");
            }
            else
            {
                this.lblPaymentAccountHead.Text = "Payment Through Bank";
                //this.ddlPaymentAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + Application["BankAccountInfoForSalesBillPayment"].ToString() + ") AND NodeId != 8");
                CustomFieldBO CardReceiveAccountsInfo = new CustomFieldBO();
                CardReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CardReceiveAccountsInfo");
                this.ddlPaymentAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CardReceiveAccountsInfo.FieldValue.ToString() + ")");
            }

            this.ddlPaymentAccountHead.DataTextField = "NodeHead";
            this.ddlPaymentAccountHead.DataValueField = "NodeId";
            this.ddlPaymentAccountHead.DataBind();
        }
        private void LoadIncomeAccountHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();

            this.ddlIncomeAccountHead.DataSource = entityDA.GetNodeMatrixInfoByAncestorNodeId(3);
            this.ddlIncomeAccountHead.DataTextField = "NodeHead";
            this.ddlIncomeAccountHead.DataValueField = "NodeId";
            this.ddlIncomeAccountHead.DataBind();
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmPMSalesBillPayment.ToString());
            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void LoadInvoiceDetail()
        {
            this.CheckObjectPermission();
            string InvoiceNumber = txtInvoiceNumber.Text;
            string CustomerCode = txtCustomerCode.Text;
            string CustomerName=txtSCustomerName.Text;
            if (string.IsNullOrEmpty(InvoiceNumber) && string.IsNullOrEmpty(CustomerCode))
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Provide Customer Code Or Invoice Number";
                this.txtInvoiceNumber.Focus();
                return;
            }
            
            List<BillReceiveVeiwBO> viewList = new List<BillReceiveVeiwBO>();
            PMSalesBillPaymentDA paymentDA = new PMSalesBillPaymentDA();
            List<PMSalesBillPaymentBO> paymentList = new List<PMSalesBillPaymentBO>();
            viewList = paymentDA.GetInvoiceDetailsByInvoiceNumberAndCustomerCode(InvoiceNumber, CustomerCode, CustomerName);
            if (viewList.Count == 0)
            {
                return;
            }


            var singleItem = viewList[0];
            //lblBillTo.Text = singleItem.BillToDate.ToString("dd/MM/yyyy");
            //lblBillForm.Text = singleItem.BillFromDate.ToString("dd/MM/yyyy");
            lblBillTo.Text = singleItem.BillToDate.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            lblBillForm.Text = singleItem.BillFromDate.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            lblCustomerName.Text = singleItem.CustomerName.ToString();
            lblCode.Text = singleItem.CustomerCode.ToString();
            lblInvoiceAmount.Text = singleItem.DueOrAdvanceAmount.ToString();
            txtCustomerId.Value = singleItem.CustomerId.ToString();
            txtInvoiceId.Value = singleItem.InvoiceId.ToString();
            txtHiddenFieldId.Value = singleItem.FieldId.ToString();

            if (singleItem.FieldId == 45)
            {
                this.lblConvertionRate.Visible = false;
                this.txtConvertionRate.Visible = false;
            }
            else
            {
                this.lblConvertionRate.Visible = true;
                this.txtConvertionRate.Visible = true;
            }
            paymentList = paymentDA.GetPaymentDetailsByDateRangeAndCustomerId(singleItem.CustomerId, singleItem.BillFromDate, singleItem.BillToDate);
            gvPaymentDetails.DataSource = paymentList;
            gvPaymentDetails.DataBind();
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void VoucherPost(decimal transactionAmount, decimal currencyAmount, out int tmpGLMasterId)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GLDealMasterDA glMasterDA = new GLDealMasterDA();
            GLDealMasterBO glMasterBO = new GLDealMasterBO();

            glMasterBO.ProjectId = Convert.ToInt32(this.ddlGLProject.SelectedValue);

            this.ddlPaymentMode.Enabled = true;
            this.ddlPaymentAccountHead.Enabled = true;

            if (this.ddlPaymentMode.SelectedIndex == 0)
            {
                glMasterBO.VoucherType = "CR";
                glMasterBO.CashChequeMode = 1;
            }
            else
            {
                glMasterBO.VoucherType = "BR";
                glMasterBO.CashChequeMode = 2;
            }
            glMasterBO.VoucherMode = 2;
            glMasterBO.VoucherDate = DateTime.Now;
            glMasterBO.PayerOrPayee = string.Empty;
            glMasterBO.Narration = "Amount Received for the Customer Name: " + lblCustomerName.Text + " and Code: " + lblCode.Text;


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
        private bool IsDetailFormValid()
        {
            bool status = true;
            if (string.IsNullOrWhiteSpace(this.txtReceiveAmount.Text))
            {
                status = false;
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Provide Receive Amount";
                this.txtReceiveAmount.Focus();
            }

            List<GLLedgerBO> rpReservationDetailListBO = Session["TransactionDetailList"] == null ? new List<GLLedgerBO>() : Session["TransactionDetailList"] as List<GLLedgerBO>;

            int rpDynamicDetailId = 0;
            if (!string.IsNullOrWhiteSpace(this.ddlIncomeAccountHead.SelectedValue))
            {
                rpDynamicDetailId = Convert.ToInt32(this.ddlIncomeAccountHead.SelectedValue);
            }

            GLLedgerBO rpDetailBO = rpDynamicDetailId == 0 ? new GLLedgerBO() : rpReservationDetailListBO.Where(x => x.NodeId == rpDynamicDetailId).FirstOrDefault();

            if (rpDetailBO != null)
            {
                if (rpDetailBO.NodeId > 0)
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Your Selected Item Already Added.";
                    status = false;
                }
            }
            return status;
        }
        private void LoadCashReceiveAccountsInfo()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            this.lblPaymentAccountHead.Text = "Payment Receive In";
            CustomFieldBO CashReceiveAccountsInfo = new CustomFieldBO();
            CashReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CashReceiveAccountsInfo");

            this.ddlCashReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CashReceiveAccountsInfo.FieldValue.ToString() + ")");
            this.ddlCashReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCashReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCashReceiveAccountsInfo.DataBind();

            CustomFieldBO CardReceiveAccountsInfo = new CustomFieldBO();
            CardReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CardReceiveAccountsInfo");
            this.ddlCardReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CardReceiveAccountsInfo.FieldValue.ToString() + ")");
            this.ddlCardReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCardReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCardReceiveAccountsInfo.DataBind();

            CustomFieldBO CompanyPaymentAccountsInfo = new CustomFieldBO();
            CompanyPaymentAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CompanyPaymentAccountsInfo");
            this.ddlCompanyPaymentAccountHead.DataSource = entityDA.GetNodeMatrixInfoByAncestorNodeId(Convert.ToInt32(CompanyPaymentAccountsInfo.FieldValue));
            this.ddlCompanyPaymentAccountHead.DataTextField = "NodeHead";
            this.ddlCompanyPaymentAccountHead.DataValueField = "NodeId";
            this.ddlCompanyPaymentAccountHead.DataBind();

        }

        [WebMethod(EnableSession = true)]
        public static string PerformSaveGuestPaymentDetailsInformationByWebMethod(bool isEdit, string ddlPayMode, string txtReceiveLeadgerAmount, string ddlCashPaymentAccountHead, string txtCardNumber, string ddlCardType, string txtExpireDate, string txtCardHolderName, string txtChecqueNumber, string ddlBankId, string ddlCompanyPaymentAccountHead)
        {

            int dynamicDetailId = 0;
            List<PMSalesBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<PMSalesBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<PMSalesBillPaymentBO>;

            PMSalesBillPaymentBO singleEntityBOEdit = guestPaymentDetailListForGrid.Where(x => x.PaymentMode == ddlPayMode).FirstOrDefault();
            if (guestPaymentDetailListForGrid.Contains(singleEntityBOEdit))
            {
                guestPaymentDetailListForGrid.Remove(singleEntityBOEdit);
            }

            if (guestPaymentDetailListForGrid != null)
            {
                dynamicDetailId = guestPaymentDetailListForGrid.Count + 1;
            }

            PMSalesBillPaymentBO guestBillPaymentBO = new PMSalesBillPaymentBO();
            if (ddlPayMode == "Company")
            {
                guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCompanyPaymentAccountHead);
                guestBillPaymentBO.PaymentType = ddlPayMode;
            }
            else if (ddlPayMode == "Other Room")
            {
                guestBillPaymentBO.NodeId = Convert.ToInt32(45);
                guestBillPaymentBO.PaymentType = ddlPayMode;
            }
            else
            {
                guestBillPaymentBO.NodeId = Convert.ToInt32(45);
                guestBillPaymentBO.PaymentType = "Advance";
            }

            guestBillPaymentBO.FieldId = 45; // Convert.ToInt32(ddlCurrency);

            //ddlPayMode, txtReceiveLeadgerAmount, ddlCashReceiveAccountsInfo, , , txtExpireDate,, ddlCompanyPaymentAccountHead


            guestBillPaymentBO.ConvertionRate = 1;
            guestBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            guestBillPaymentBO.PaymentAmout = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            //}
            HMUtility hmUtility = new HMUtility();
            if (string.IsNullOrEmpty(txtExpireDate))
            {
                guestBillPaymentBO.ExpireDate = null;
            }
            else
            {
                guestBillPaymentBO.ExpireDate = hmUtility.GetDateTimeFromString(txtExpireDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            guestBillPaymentBO.ChecqueDate = DateTime.Now;
            guestBillPaymentBO.PaymentMode = ddlPayMode;
            guestBillPaymentBO.PaymentId = dynamicDetailId;
            guestBillPaymentBO.CardNumber = txtCardNumber;
            guestBillPaymentBO.CardType = ddlCardType;
            guestBillPaymentBO.ChecqueNumber = txtChecqueNumber;
            //guestBillPaymentBO.PayMode = Int32.Parse(ddlPayMode);
            guestBillPaymentBO.CardHolderName = txtCardHolderName;
            guestBillPaymentBO.BankId = Int32.Parse(ddlBankId);

            guestPaymentDetailListForGrid.Add(guestBillPaymentBO);
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;
            return LoadGuestPaymentDetailGridViewByWM();
        }
        public static string LoadGuestPaymentDetailGridViewByWM()
        {
            string strTable = "";
            List<PMSalesBillPaymentBO> detailList = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<PMSalesBillPaymentBO>;
            if (detailList != null)
            {
                strTable += "<table style='width:100%' cellspacing='0' cellpadding='4' id='ReservationDetailGrid'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Payment Mode</th><th align='left' scope='col'>Amount</th><th align='center' scope='col'>Action</th></tr>";
                int counter = 0;
                foreach (PMSalesBillPaymentBO dr in detailList)
                {
                    counter++;
                    if (counter % 2 == 0)
                    {
                        // It's even
                        strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 40%;'>" + dr.PaymentMode + "</td>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:White;'><td align='left' style='width: 40%;'>" + dr.PaymentMode + "</td>";
                    }
                    strTable += "<td align='left' style='width: 40%;'>" + dr.PaymentAmout + "</td>";
                    strTable += "<td align='center' style='width: 15%;'>";
                    strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformGuestPaymentDetailDelete(" + dr.PaymentId + ")' alt='Delete Information' border='0' />";
                    strTable += "</td></tr>";
                }
                strTable += "</table>";
                if (strTable == "")
                {
                    strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
                }
            }
            return strTable;
        }
        [WebMethod(EnableSession = true)]
        public static string PerformGetTotalPaidAmountByWebMethod()
        {
            decimal sum = 0;
            var List = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<PMSalesBillPaymentBO>;
            if (List != null)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    sum = sum + List[i].PaymentAmout;
                }
            }
            return sum.ToString();
        }
    }
}