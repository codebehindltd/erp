using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data;
using System.Globalization;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmReservationBillPayment : BasePage
    {
        HiddenField innboardMessage;
        protected bool isSingle = true;
        protected int isIntegratedGeneralLedgerDiv = 1;
        protected int isNewAddButtonEnable = 1;
        protected int isSearchPanelEnable = -1;
        protected int isCompanyProjectPanelEnable = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            isSingle = hmUtility.GetSingleProjectAndCompany();
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                string cardValidation = System.Web.Configuration.WebConfigurationManager.AppSettings["CardValidation"].ToString();
                txtCardValidation.Value = cardValidation.ToString();

                this.LoadCurrency();
                this.CheckObjectPermission();
                LoadIsConversionRateEditable();
                IsLocalCurrencyDefaultSelected();               
                this.LoadCommonDropDownHiddenField();
                this.LoadGLCompany(false);
                this.LoadAccountHeadInfo();
                this.LoadGLProject(false);
                this.LoadBank();
                Session["TransactionDetailList"] = null;

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

                if (Request.QueryString["rsvnId"] != null)
                {
                    //string reservationId = Request.QueryString["rsvnId"].ToString();
                    //hfReservationId.Value = reservationId;

                    //UserInformationBO userInformationBO = new UserInformationBO();
                    //userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

                    //GridViewDataNPaging<ReservationBillPaymentBO, GridPaging> gridData = new GridViewDataNPaging<ReservationBillPaymentBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, 1);
                    //gridData = LoadReservationBillInfo(reservationId, 1, 1, 1);

                    //gvGuestHouseService.DataSource = gridData.GridData;
                    //gvGuestHouseService.DataBind();

                    //GridPagingDetails.InnerHtml = gridData.GridPageLinks.PreviousButton;
                    //GridPagingDetails.InnerHtml += gridData.GridPageLinks.Pagination;
                    //GridPagingDetails.InnerHtml += gridData.GridPageLinks.NextButton;

                    string rsvnId = Request.QueryString["rsvnId"].ToString();
                    RoomReservationDA reservationDa = new RoomReservationDA();
                    RoomReservationBO reservationBO = new RoomReservationBO();
                    reservationBO = reservationDa.GetRoomReservationInfoById(int.Parse(rsvnId));
                    if (reservationBO.ReservationId > 0)
                    {
                        //txtSearchReservationCodeOrGuset.Text = reservationBO.ReservationNumber + " (" + reservationBO.GuestName + ")";
                        this.txtSrcRoomNumber.Text = reservationBO.ReservationNumber;
                        this.LoadReservationNumber();
                    }
                }
                else
                {
                    LoadDummyGridData();
                }
                CheckObjectPermission();
            }
           
        }
        //************************ User Defined Function ********************//
        protected void btnSrcRoomNumber_Click(object sender, EventArgs e)
        {
            //this.ClearCommonSessionInformation();
            this.LoadReservationNumber();
        }
        private void LoadReservationNumber()
        {
            this.ddlReservationId.DataSource = null;
            this.ddlReservationId.DataBind();

            RoomReservationDA banquetReservationDA = new RoomReservationDA();
            RoomReservationBO banquetReservationBO = new RoomReservationBO();
            List<RoomReservationBO> banquetReservationBOList = new List<RoomReservationBO>();
            this.ddlReservationId.DataSource = null;
            this.ddlReservationId.DataBind();

            banquetReservationBO = banquetReservationDA.GetRoomReservationInfoByReservationNumber(this.txtSrcRoomNumber.Text.Trim());
            if (banquetReservationBO != null)
            {
                if (banquetReservationBO.ReservationId > 0)
                {
                    txtGuestName.Text = banquetReservationBO.GuestName;
                    txtGuestEmail.Text = banquetReservationBO.GuestEmail;
                    txtGuestPhone.Text = banquetReservationBO.GuestPhone;

                    hfReservationId.Value = banquetReservationBO.ReservationId.ToString();
                    banquetReservationBOList.Add(banquetReservationBO);
                    this.ddlReservationId.DataSource = banquetReservationBOList;
                    this.ddlReservationId.DataTextField = "ReservationNumber";
                    this.ddlReservationId.DataValueField = "ReservationId";
                    this.ddlReservationId.DataBind();
                }
                else
                {
                    hfReservationId.Value = "";
                    this.ddlReservationId.DataSource = banquetReservationBOList;
                    this.ddlReservationId.DataTextField = "ReservationNumber";
                    this.ddlReservationId.DataValueField = "ReservationId";
                    this.ddlReservationId.DataBind();
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + " Valid Reservation Number.", AlertType.Warning);
                }
            }
        }
        private void LoadDummyGridData()
        {
            List<ReservationBillPaymentBO> reservationBillPaymentList = new List<ReservationBillPaymentBO>();
            ReservationBillPaymentBO obj = new ReservationBillPaymentBO();

            obj.PaymentId = 0;
            obj.PaymentDate = DateTime.Now;
            obj.PaymentAmount = 0;
            obj.PaymentType = string.Empty;

            reservationBillPaymentList.Add(obj);

            //this.gvGuestHouseService.DataSource = reservationBillPaymentList;
            //this.gvGuestHouseService.DataBind();

            //this.gvConversionInfo.DataSource = reservationBillPaymentList;
            //this.gvConversionInfo.DataBind();
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
                        //isConversionRateEditable = true;
                    }
                    else
                    {
                        this.txtConversionRate.ReadOnly = false;
                        //isConversionRateEditable = false;
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
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void CheckObjectPermission()
        {
            hfIsSavePermission.Value = isSavePermission== true ? "1" : "0";
            hfIsUpdatePermission.Value = isUpdatePermission== true ? "1" : "0";
            hfIsDeletePermission.Value = isDeletePermission == true ? "1" : "0";
            hfIsViewPermission.Value = isViewPermission == true ? "1" : "0";
            
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
        private void LoadBank()
        {
            BankDA bankDA = new BankDA();
            List<BankBO> entityBOList = new List<BankBO>();
            entityBOList = bankDA.GetBankInfo();

            this.ddlBank.DataSource = entityBOList;
            this.ddlBank.DataTextField = "BankName";
            this.ddlBank.DataValueField = "BankId";
            this.ddlBank.DataBind();

            this.ddlBankId.DataSource = entityBOList;
            this.ddlBankId.DataTextField = "BankName";
            this.ddlBankId.DataValueField = "BankId";
            this.ddlBankId.DataBind();

            this.ddlBankName.DataSource = entityBOList;
            this.ddlBankName.DataTextField = "BankName";
            this.ddlBankName.DataValueField = "BankId";
            this.ddlBankName.DataBind();

            this.ddlMBankingBankId.DataSource = entityBOList;
            this.ddlMBankingBankId.DataTextField = "BankName";
            this.ddlMBankingBankId.DataValueField = "BankId";
            this.ddlMBankingBankId.DataBind();

            this.ddlBank.Items.Insert(0, new ListItem { Text = hmUtility.GetDropDownFirstValue(), Value = string.Empty });
            this.ddlBankId.Items.Insert(0, new ListItem { Text = hmUtility.GetDropDownFirstValue(), Value = string.Empty });
            this.ddlBankName.Items.Insert(0, new ListItem { Text = hmUtility.GetDropDownFirstValue(), Value = string.Empty });
            this.ddlMBankingBankId.Items.Insert(0, new ListItem { Text = hmUtility.GetDropDownFirstValue(), Value = string.Empty });
        }
        private void Cancel()
        {
            this.txtLedgerAmount.Text = string.Empty;
            this.hfReservationId.Value = "0";
            this.btnSave.Text = "Save";
            this.hfPaymentId.Value = string.Empty;
            this.hfDealId.Value = string.Empty;
            this.ddlCurrency.SelectedIndex = 0;
            this.txtCurrencyAmount.Text = string.Empty;
            this.txtConversionRate.Text = string.Empty;
            this.ddlPayMode.SelectedIndex = 0;
            this.ddlBankId.SelectedValue = "0";
            this.ddlBankName.SelectedValue = "0";
            txtSearchReservationCodeOrGuset.Focus();
        }        
        private void LoadAccountHeadInfo()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            //this.lblPaymentAccountHead.Text = "Payment Receive In";
            //CustomFieldBO CashReceiveAccountsInfo = new CustomFieldBO();
            //CashReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CashReceiveAccountsInfo");

            List<CommonPaymentModeBO> commonPaymentModeBOList = new List<CommonPaymentModeBO>();
            commonPaymentModeBOList = hmCommonDA.GetCommonPaymentModeInfo("All");

            CommonPaymentModeBO cashPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Cash").FirstOrDefault();
            CommonPaymentModeBO cardPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Card").FirstOrDefault();
            CommonPaymentModeBO chequePaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Cheque").FirstOrDefault();
            CommonPaymentModeBO companyPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Company").FirstOrDefault();
            CommonPaymentModeBO mBankingPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "M-Banking").FirstOrDefault();
            
            //this.ddlCashReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CashReceiveAccountsInfo.FieldValue.ToString() + ")");
            this.ddlCashReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cashPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlCashReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCashReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCashReceiveAccountsInfo.DataBind();

            //CustomFieldBO CardReceiveAccountsInfo = new CustomFieldBO();
            //List<NodeMatrixBO> cardEntityBOList = new List<NodeMatrixBO>();
            //CardReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CardReceiveAccountsInfo");
            //cardEntityBOList = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CardReceiveAccountsInfo.FieldValue.ToString() + ")");
            //this.ddlCardReceiveAccountsInfo.DataSource = cardEntityBOList;
            
            this.ddlCardReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cardPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            this.ddlCardReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCardReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCardReceiveAccountsInfo.DataBind();
           
            //CustomFieldBO ChequeReceiveAccountsInfo = new CustomFieldBO();
            //ChequeReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("ChequeReceiveAccountsInfo");
            //this.ddlChequeReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + ChequeReceiveAccountsInfo.FieldValue.ToString() + ")");
            this.ddlChequeReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + chequePaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlChequeReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlChequeReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlChequeReceiveAccountsInfo.DataBind();

            CustomFieldBO IncomeSourceAccountsInfo = new CustomFieldBO();
            IncomeSourceAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("IncomeSourceAccountsInfo");
            this.ddlIncomeSourceAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + IncomeSourceAccountsInfo.FieldValue.ToString() + ")");
            this.ddlIncomeSourceAccountsInfo.DataTextField = "NodeHead";
            this.ddlIncomeSourceAccountsInfo.DataValueField = "NodeId";
            this.ddlIncomeSourceAccountsInfo.DataBind();

            //CustomFieldBO MBankingReceiveAccountsInfo = new CustomFieldBO();
            //MBankingReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("MBankingReceiveAccountsInfo");
            //this.ddlMBankingReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + MBankingReceiveAccountsInfo.FieldValue.ToString() + ")");
            this.ddlMBankingReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + mBankingPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlMBankingReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlMBankingReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlMBankingReceiveAccountsInfo.DataBind();
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        private static void VoucherPost(out int tmpGLMasterId, ReservationBillPaymentBO reservationBillPaymentBO, string ddlGLProject, string txtSearchReservationCodeOrGuset, string receiveAccountsInfo, string receiveAccountsInfoText, string ddlIncomeSourceAccountsInfo, string ddlIncomeSourceAccountsInfoText)
        {
            HMUtility hmUtility = new HMUtility();

            List<GLLedgerBO> ledgerDetailListBOForDebit = new List<GLLedgerBO>();

            GLLedgerBO detailBODebit = new GLLedgerBO();
            detailBODebit.LedgerId = 0;
            if (reservationBillPaymentBO.PaymentMode == "Cash")
            {
                detailBODebit.NodeId = Convert.ToInt32(receiveAccountsInfo);   //this.ddlCashReceiveAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = receiveAccountsInfoText;   // this.ddlCashReceiveAccountsInfo.SelectedItem.Text;
            }
            else if (reservationBillPaymentBO.PaymentMode == "Card")
            {
                detailBODebit.NodeId = Convert.ToInt32(receiveAccountsInfo);  //Convert.ToInt32(this.ddlCardReceiveAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = receiveAccountsInfoText;   //this.ddlCardReceiveAccountsInfo.SelectedItem.Text;
            }
            else if (reservationBillPaymentBO.PaymentMode == "Cheque")
            {
                detailBODebit.NodeId = Convert.ToInt32(receiveAccountsInfo);  //this.ddlChequeReceiveAccountsInfo.SelectedValue);
                detailBODebit.NodeHead = receiveAccountsInfoText;   //this.ddlChequeReceiveAccountsInfo.SelectedItem.Text;
            }
            detailBODebit.LedgerMode = 1;
            detailBODebit.FieldId = reservationBillPaymentBO.FieldId; //45 for Local Currency
            //detailBO.LedgerDebitAmount = !string.IsNullOrWhiteSpace(this.txtReceiveAmount.Text) ? Convert.ToDecimal(this.txtReceiveAmount.Text) : 0;
            detailBODebit.LedgerDebitAmount = reservationBillPaymentBO.CurrencyAmount; //!string.IsNullOrWhiteSpace(this.txtLedgerAmountHiddenField.Value) ? Convert.ToDecimal(this.txtLedgerAmountHiddenField.Value) : 0; ;
            detailBODebit.CurrencyAmount = reservationBillPaymentBO.CurrencyAmount;   //!string.IsNullOrWhiteSpace(this.txtCurrencyAmount.Text) ? Convert.ToDecimal(this.txtCurrencyAmount.Text) : 0;
            detailBODebit.LedgerAmount = reservationBillPaymentBO.CurrencyAmount;    //!string.IsNullOrWhiteSpace(this.txtLedgerAmountHiddenField.Value) ? Convert.ToDecimal(this.txtLedgerAmountHiddenField.Value) : 0; ;
            detailBODebit.NodeNarration = "";
            ledgerDetailListBOForDebit.Add(detailBODebit);


            System.Web.HttpContext.Current.Session["TransactionDetailList"] = ledgerDetailListBOForDebit;

            List<GLLedgerBO> ledgerDetailListBO = System.Web.HttpContext.Current.Session["TransactionDetailList"] == null ? new List<GLLedgerBO>() : System.Web.HttpContext.Current.Session["TransactionDetailList"] as List<GLLedgerBO>;
            //Oppusite Head (Payment Head Information)
            GLLedgerBO detailBOCredit = new GLLedgerBO();
            detailBOCredit.LedgerId = 1;
            detailBOCredit.NodeId = Convert.ToInt32(ddlIncomeSourceAccountsInfo);  //this.ddlIncomeSourceAccountsInfo.SelectedValue);
            detailBOCredit.NodeHead = ddlIncomeSourceAccountsInfoText;  //this.ddlIncomeSourceAccountsInfo.SelectedItem.Text;
            detailBOCredit.LedgerMode = 2;
            detailBOCredit.FieldId = reservationBillPaymentBO.FieldId;     //Convert.ToInt32(this.ddlCurrency.SelectedValue);
            //detailBO.LedgerDebitAmount = !string.IsNullOrWhiteSpace(this.txtReceiveAmount.Text) ? Convert.ToDecimal(this.txtReceiveAmount.Text) : 0;
            detailBOCredit.LedgerDebitAmount = reservationBillPaymentBO.CurrencyAmount;  //!string.IsNullOrWhiteSpace(this.txtLedgerAmountHiddenField.Value) ? Convert.ToDecimal(this.txtLedgerAmountHiddenField.Value) : 0;
            detailBOCredit.CurrencyAmount = reservationBillPaymentBO.CurrencyAmount;    //!string.IsNullOrWhiteSpace(this.txtCurrencyAmount.Text) ? Convert.ToDecimal(this.txtCurrencyAmount.Text) : 0;
            detailBOCredit.LedgerAmount = reservationBillPaymentBO.CurrencyAmount;     // !string.IsNullOrWhiteSpace(this.txtLedgerAmountHiddenField.Value) ? Convert.ToDecimal(this.txtLedgerAmountHiddenField.Value) : 0;
            detailBOCredit.NodeNarration = "";
            ledgerDetailListBO.Add(detailBOCredit);
            System.Web.HttpContext.Current.Session["TransactionDetailList"] = ledgerDetailListBO;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GLDealMasterDA glMasterDA = new GLDealMasterDA();
            GLDealMasterBO glMasterBO = new GLDealMasterBO();

            Boolean isCash = true;
            if (reservationBillPaymentBO.PaymentMode != "Cash")
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

            glMasterBO.ProjectId = Convert.ToInt32(ddlGLProject);


            if (reservationBillPaymentBO.PaymentMode == "Cash")
            {
                glMasterBO.VoucherMode = 2;
            }
            else
            {
                glMasterBO.VoucherMode = 3;
            }

            glMasterBO.VoucherDate = DateTime.Now;
            glMasterBO.PayerOrPayee = "";
            glMasterBO.Narration = "Advance Received for the Reservation Of : " + txtSearchReservationCodeOrGuset;


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

            //int tmpGLMasterId = 0;
            string currentVoucherNo = string.Empty;
            glMasterBO.CreatedBy = userInformationBO.UserInfoId;
            Boolean status = glMasterDA.SaveGLMasterInfo(glMasterBO, out tmpGLMasterId, out currentVoucherNo, System.Web.HttpContext.Current.Session["TransactionDetailList"] as List<GLLedgerBO>, approvedBOList);
            //this.ddlGLCompany.SelectedValue = "0";
            //this.ddlGLProject.SelectedValue = "0";
            // this.txtLedgerAmount.Text = "0";
        }
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            string paymentIdList = string.Empty;

            //foreach (GridViewRow row in gvGuestHouseService.Rows)
            //{
            //    bool isSelect = ((CheckBox)row.FindControl("ChkSelect")).Checked;

            //    if (isSelect)
            //    {
            //        Label lblObjectTabIdValue = (Label)row.FindControl("lblid");
            //        if (string.IsNullOrWhiteSpace(paymentIdList.Trim()))
            //        {
            //            paymentIdList = paymentIdList + Convert.ToInt32(lblObjectTabIdValue.Text);
            //        }
            //        else
            //        {
            //            paymentIdList = paymentIdList + "," + Convert.ToInt32(lblObjectTabIdValue.Text);
            //        }
            //    }
            //}


            if (!string.IsNullOrWhiteSpace(paymentIdList))
            {
                string url = "/HotelManagement/Reports/frmReportReservationBillPayment.aspx?PaymentIdList=" + paymentIdList;
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
            }

        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static ReturnInfo PerformReservationBillPaymentSaveAction(ReservationBillPaymentBO reservationBillPaymentBO, string ddlGLCompany, string ddlGLProject, string txtSearchReservationCodeOrGuset, string receiveAccountsInfo, string receiveAccountsInfoText, string ddlIncomeSourceAccountsInfo, string ddlIncomeSourceAccountsInfoText, string ddlMBankingBankId, string ddlMBankingReceiveAccountsInfo, string remarks)
        {
            string message = "";
            string transactionHead = string.Empty;
            bool isSingle = true;

            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            GuestBillPaymentBO guestBillPayment = new GuestBillPaymentBO();

            try
            {
                isSingle = isSingle = hmUtility.GetSingleProjectAndCompany();

                if (isSingle == false)
                {
                    if (ddlGLCompany == "0")
                    {
                        Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                        if (isIntegrated)
                        {
                            message = "Please Select Company Name.";
                            rtninf.IsSuccess = false;
                            rtninf.AlertMessage = CommonHelper.AlertInfo(message, AlertType.Information);
                            return rtninf;
                        }
                    }
                    else if (ddlGLCompany != "0")
                    {
                        if (ddlGLProject == "0")
                        {
                            Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                            if (isIntegrated)
                            {
                                message = "Please select Project Name.";
                                rtninf.IsSuccess = false;
                                rtninf.AlertMessage = CommonHelper.AlertInfo(message, AlertType.Information);

                                return rtninf;
                            }
                        }
                    }
                }

                HMCommonDA hmCommonDA = new HMCommonDA();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                ReservationBillPaymentDA reservationBillPaymentDA = new ReservationBillPaymentDA();
                GuestBillPaymentDA guestBillPayDa = new GuestBillPaymentDA();

                reservationBillPaymentBO.ChecqueDate = DateTime.Now;

                if (reservationBillPaymentBO.PaymentMode == "Cash")
                {
                    reservationBillPaymentBO.BankId = 0;
                    reservationBillPaymentBO.ChecqueNumber = "";
                    reservationBillPaymentBO.CardReference = "";
                    reservationBillPaymentBO.CardNumber = "";
                    reservationBillPaymentBO.BranchName = "";
                }
                else if (reservationBillPaymentBO.PaymentMode == "Card")
                {
                    reservationBillPaymentBO.ChecqueNumber = "";
                }
                else if (reservationBillPaymentBO.PaymentMode == "Cheque")
                {
                    reservationBillPaymentBO.CardReference = "";
                    reservationBillPaymentBO.CardNumber = "";
                    reservationBillPaymentBO.BranchName = "";
                }
                else if (reservationBillPaymentBO.PaymentMode == "M-Banking")
                {
                    reservationBillPaymentBO.BankId = !string.IsNullOrWhiteSpace(ddlMBankingBankId) ? Convert.ToInt32(ddlMBankingBankId) : 0;
                    reservationBillPaymentBO.ChecqueNumber = "";
                    reservationBillPaymentBO.CardReference = "";
                    reservationBillPaymentBO.CardNumber = "";
                    reservationBillPaymentBO.BranchName = "";
                }
                else
                {
                    reservationBillPaymentBO.CardReference = "";
                    reservationBillPaymentBO.CardNumber = "";
                    reservationBillPaymentBO.BranchName = "";
                }
                reservationBillPaymentBO.Remarks = remarks;

                if (reservationBillPaymentBO.PaymentId == 0)    //string.IsNullOrWhiteSpace(hfPaymentId.Value))
                {
                    int tmpPaymentId = 0;
                    reservationBillPaymentBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = false;

                    if (reservationBillPaymentBO.PaymentType == "Advance" || reservationBillPaymentBO.PaymentType == "Refund")
                    {
                        if (reservationBillPaymentBO.PaymentType == "Refund")
                        {
                            decimal RefundAmount = 0;
                            ReservationBillPaymentDA DA = new ReservationBillPaymentDA();

                            RefundAmount = DA.GetMaxRefundForReservationBillPayment(reservationBillPaymentBO.ReservationId, 0);
                            if(reservationBillPaymentBO.PaymentAmount > RefundAmount)
                            {
                                status = false;

                                rtninf.IsSuccess = false;
                                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.MaxRefundAmount + " " + RefundAmount, AlertType.Warning);
                                throw new Exception("RefundError#" + AlertMessage.MaxRefundAmount + " " + RefundAmount);
                            }
                            else
                            {
                                status = reservationBillPaymentDA.SaveReservationBillPaymentInfo(reservationBillPaymentBO, out tmpPaymentId);
                            }
                        }
                        else
                        {
                            status = reservationBillPaymentDA.SaveReservationBillPaymentInfo(reservationBillPaymentBO, out tmpPaymentId);
                        }
                    }
                    else if (reservationBillPaymentBO.PaymentType == "NoShowCharge")
                    {
                        guestBillPayment = NoShowChargeForReservation(reservationBillPaymentBO, ddlGLCompany, ddlGLProject, txtSearchReservationCodeOrGuset, receiveAccountsInfo, receiveAccountsInfoText, ddlIncomeSourceAccountsInfo, ddlIncomeSourceAccountsInfoText);
                        guestBillPayment.CreatedBy = userInformationBO.UserInfoId;
                        guestBillPayment.ModuleName = "FrontOffice";
                        status = guestBillPayDa.SaveGuestBillPaymentInfo(guestBillPayment, out tmpPaymentId, "Others");

                        //reservationBillPaymentDA.SaveReservationNotShowBillPayment(guestBillPayment, out tmpPaymentId);
                    }

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.ReservationBillPayment.ToString(), tmpPaymentId,
                        ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ReservationBillPayment));

                        //if (logStatus)
                        //{
                        //    message = CommonHelper.SaveMessage;
                        //}
                        //else
                        //{
                        //    message = "Service Unavailable Please Try Again Later.";
                        //}

                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Success, AlertType.Success);
                    }
                }
                else
                {
                    reservationBillPaymentBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = false;
                    if (reservationBillPaymentBO.PaymentType == "Advance" || reservationBillPaymentBO.PaymentType == "Refund")
                    {
                        if (reservationBillPaymentBO.PaymentType == "Refund")
                        {
                            decimal RefundAmount = 0;
                            ReservationBillPaymentDA DA = new ReservationBillPaymentDA();

                            RefundAmount = DA.GetMaxRefundForReservationBillPayment(reservationBillPaymentBO.ReservationId, reservationBillPaymentBO.PaymentId);
                            if (reservationBillPaymentBO.PaymentAmount > RefundAmount)
                            {
                                status = false;

                                rtninf.IsSuccess = false;
                                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.MaxRefundAmount + " " + RefundAmount, AlertType.Warning);
                                throw new Exception("RefundError#" + AlertMessage.MaxRefundAmount + " " + RefundAmount);
                            }
                            else
                            {
                                status = reservationBillPaymentDA.UpdateReservationBillPaymentInfo(reservationBillPaymentBO);
                            }
                        }
                        else
                        {
                            status = reservationBillPaymentDA.UpdateReservationBillPaymentInfo(reservationBillPaymentBO);
                        }
                    }
                    else if (reservationBillPaymentBO.PaymentType == "NoShowCharge")
                    {
                        guestBillPayment = NoShowChargeForReservation(reservationBillPaymentBO, ddlGLCompany, ddlGLProject, txtSearchReservationCodeOrGuset, receiveAccountsInfo, receiveAccountsInfoText, ddlIncomeSourceAccountsInfo, ddlIncomeSourceAccountsInfoText);
                        guestBillPayment.PaymentId = reservationBillPaymentBO.PaymentId;
                        guestBillPayment.LastModifiedBy = userInformationBO.UserInfoId;
                        status = guestBillPayDa.UpdateGuestBillPaymentInfo(guestBillPayment);
                    }

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.ReservationBillPayment.ToString(), reservationBillPaymentBO.PaymentId,
                        ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ReservationBillPayment));

                        //if (logStatus)
                        //{
                        //    message = CommonHelper.UpdateMessage;
                        //}

                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Success, AlertType.Success);
                    }
                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                if (ex.Message.StartsWith("RefundError", StringComparison.OrdinalIgnoreCase))
                {
                    rtninf.AlertMessage = ex.Message.Split(new string[] { "#" }, StringSplitOptions.None)[1];
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                return rtninf;
            }

            return rtninf; //CommonHelper.MessageInfo(CommonHelper.MessageTpe.Success.ToString(), message);
        }
        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId, string paymentType)
        {
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            string message = string.Empty;
            string entity = string.Empty;
            string remarks = string.Empty;
            try
            {
                Boolean status = false;
                HMCommonDA hmCommonDA = new HMCommonDA();

                if (paymentType == "NoShowCharge")
                {
                    status = hmCommonDA.DeleteInfoById("HotelGuestBillPayment", "PaymentId", sEmpId);
                    entity = EntityTypeEnum.EntityType.GuestBillPayment.ToString();
                    remarks = hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestBillPayment);
                }
                else
                {
                    status = hmCommonDA.DeleteInfoById("HotelReservationBillPayment", "PaymentId", sEmpId);
                    entity = EntityTypeEnum.EntityType.ReservationBillPayment.ToString();
                    remarks = hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ReservationBillPayment);
                }                    
                if (status)
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),entity,sEmpId,ProjectModuleEnum.ProjectModule.FrontOffice.ToString(),remarks); 
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Success, AlertType.Success);
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
        public static ReservationBillPaymentBO FillForm(int paymentId, string paymentType)
        {
            ReservationBillPaymentBO reservationBillPaymentBO = new ReservationBillPaymentBO();
            reservationBillPaymentBO = GetReservationBillForUpdate(paymentId, paymentType);

            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetCommonCurrencyInfoById(reservationBillPaymentBO.FieldId);

            reservationBillPaymentBO.CurrencyType = commonCurrencyBO.CurrencyType;

            return reservationBillPaymentBO;
        }
        [WebMethod]
        public static List<RoomReservationBO> SearchGuestByReservationOrGuestName(string searchText, string searchType, string paymentType)
        {
            string reservationNumber = string.Empty, guestName = string.Empty;
            int allactive = 0;

            if (searchType == "RESERVATIONCODE")
            {
                reservationNumber = searchText;                
            }
            else if (searchType == "GUESTNAME")
            {
                guestName = searchText;
            }

            if (paymentType == "Advance")
            {
                allactive = 1;
            }

            RoomReservationDA roomReservationDA = new RoomReservationDA();
            List<RoomReservationBO> roomReservationList = new List<RoomReservationBO>();

            roomReservationList = roomReservationDA.GetRoomReservationInfoForRegistrationSearch(reservationNumber, guestName, allactive);

            return roomReservationList;
        }
        [WebMethod]
        public static GridViewDataNPaging<ReservationBillPaymentBO, GridPaging> LoadReservationBillInfo(string reservationId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            GridViewDataNPaging<ReservationBillPaymentBO, GridPaging> myGridData = new GridViewDataNPaging<ReservationBillPaymentBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (!string.IsNullOrWhiteSpace(reservationId))
            {
                ReservationBillPaymentDA da = new ReservationBillPaymentDA();
                List<ReservationBillPaymentBO> reservationList = da.GetReservationBillPaymentInfoByReservationId(Convert.ToInt32(reservationId), userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

                myGridData.GridPagingProcessing(reservationList, totalRecords);
            }
            return myGridData;
        }
        private static ReservationBillPaymentBO GetReservationBillForUpdate(int paymentId, string paymentType)
        {
            ReservationBillPaymentBO billPayment = new ReservationBillPaymentBO();
            ReservationBillPaymentDA reservationBillPaymentDA = new ReservationBillPaymentDA();

            if (paymentType == "Advance" || paymentType == "Refund")
            {
                billPayment = reservationBillPaymentDA.GetReservationBillPaymentInfoById(paymentId);
            }
            else if (paymentType == "NoShowCharge")
            {
                HMUtility hmUtility = new HMUtility();
                GuestBillPaymentDA guestBillPaymentDa = new GuestBillPaymentDA();
                GuestBillPaymentBO guestBillPaymentBo = new GuestBillPaymentBO();

                guestBillPaymentBo = guestBillPaymentDa.GetGuestBillPaymentInfoById(paymentId);

                billPayment.PaymentId = guestBillPaymentBo.PaymentId;  //Convert.ToInt32(reader["PaymentId"]);
                billPayment.ReservationId = Convert.ToInt32(guestBillPaymentBo.ServiceBillId); //Convert.ToInt32(reader["ReservationId"]);
                billPayment.PaymentMode = guestBillPaymentBo.PaymentMode; //reader["ReservationId"].ToString();
                billPayment.PaymentType = guestBillPaymentBo.PaymentType;
                billPayment.BankId = Convert.ToInt32(guestBillPaymentBo.BankId); //Convert.ToInt32(reader["BankId"]);
                billPayment.BranchName = guestBillPaymentBo.BranchName; //reader["BranchName"].ToString();
                billPayment.ChecqueNumber = guestBillPaymentBo.ChecqueNumber; //reader["ChecqueNumber"].ToString();
                billPayment.ChecqueDate = hmUtility.GetDateTimeFromString(guestBillPaymentBo.ChecqueDate.ToString(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat); //hmUtility.GetDateTimeFromString(reader["ChecqueDate"].ToString());

                billPayment.CardNumber = guestBillPaymentBo.CardNumber; // reader["CardNumber"].ToString();
                billPayment.CardType = guestBillPaymentBo.CardType; //reader["CardType"].ToString();
                billPayment.ExpireDate = guestBillPaymentBo.ExpireDate; //hmUtility.GetDateTimeFromString(reader["ExpireDate"].ToString());
                billPayment.CardHolderName = guestBillPaymentBo.CardHolderName; //reader["CardHolderName"].ToString();
                billPayment.CardReference = guestBillPaymentBo.CardReference; //reader["CardReference"].ToString();

                billPayment.PaymentDate = hmUtility.GetDateTimeFromString(guestBillPaymentBo.PaymentDate.ToString(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat); //hmUtility.GetDateTimeFromString(reader["PaymentDate"]);
                billPayment.FieldId = guestBillPaymentBo.FieldId; //Convert.ToInt32(reader["FieldId"]);
                billPayment.CurrencyAmount = guestBillPaymentBo.CurrencyAmount; //Convert.ToDecimal(reader["CurrencyAmount"].ToString());
                billPayment.PaymentAmount = Convert.ToDecimal(guestBillPaymentBo.PaymentAmount); //Convert.ToDecimal(reader["PaymentAmout"].ToString());
                billPayment.DealId = guestBillPaymentBo.DealId; //Convert.ToInt32(reader["DealId"]);

                billPayment.ReservedCompany = string.Empty; //Convert.ToString(reader["ReservedCompany"]);
            }

            return billPayment;
        }
        private static GuestBillPaymentBO NoShowChargeForReservation(ReservationBillPaymentBO reservationBillPaymentBO, string ddlGLCompany, string ddlGLProject, string txtSearchReservationCodeOrGuset, string receiveAccountsInfo, string receiveAccountsInfoText, string ddlIncomeSourceAccountsInfo, string ddlIncomeSourceAccountsInfoText)
        {
            GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();
            decimal noShowAmount = reservationBillPaymentBO.PaymentAmount;

            if (noShowAmount > 0)
            {
                HMUtility hmUtility = new HMUtility();
                int ddlPaidByRegistrationId = 0;

                ddlPaidByRegistrationId = reservationBillPaymentBO.ReservationId;//Convert.ToInt32(reservationId);

                HMCommonDA hmCommonDA = new HMCommonDA();
                NodeMatrixDA entityDA = new NodeMatrixDA();
                CustomFieldBO CashReceiveAccountsInfo = new CustomFieldBO();
                List<NodeMatrixBO> nMatrixBo = new List<NodeMatrixBO>();

                CashReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CashReceiveAccountsInfo");
                nMatrixBo = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CashReceiveAccountsInfo.FieldValue.ToString() + ")");

                //this.ddlCashPaymentAccountHeadForNoShow.DataTextField = "NodeHead";
                //this.ddlCashPaymentAccountHeadForNoShow.DataValueField = "NodeId";
                //this.ddlCashPaymentAccountHeadForNoShow.DataBind();

                guestBillPaymentBO.NodeId = nMatrixBo[0].NodeId; //Convert.ToInt32(ddlCashPaymentAccountHeadForNoShow.SelectedValue);
                guestBillPaymentBO.AccountsPostingHeadId = nMatrixBo[0].NodeId; //Convert.ToInt32(ddlCashPaymentAccountHeadForNoShow.SelectedValue);
                guestBillPaymentBO.BillPaidBy = reservationBillPaymentBO.ReservationId;  // Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.ServiceBillId = reservationBillPaymentBO.ReservationId;
                guestBillPaymentBO.PaymentType = reservationBillPaymentBO.PaymentType; //ddlPaymentType.SelectedValue; //"NoShowCharge";
                guestBillPaymentBO.BankId = reservationBillPaymentBO.BankId;
                guestBillPaymentBO.RegistrationId = 0; // Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.FieldId = Convert.ToInt32(reservationBillPaymentBO.FieldId); // Convert.ToInt32(ddlCurrency);
                guestBillPaymentBO.ConvertionRate = 1;
                guestBillPaymentBO.CurrencyAmount = (noShowAmount);
                guestBillPaymentBO.PaymentAmount = (noShowAmount);
                guestBillPaymentBO.ChecqueDate = DateTime.Now;

                guestBillPaymentBO.PaymentMode = reservationBillPaymentBO.PaymentMode;
                guestBillPaymentBO.PaymentModeId = 0;

                if (guestBillPaymentBO.PaymentMode == "Cash")
                {
                    guestBillPaymentBO.BankId = 0;
                    guestBillPaymentBO.ChecqueNumber = string.Empty;
                    guestBillPaymentBO.CardReference = string.Empty;
                    guestBillPaymentBO.CardNumber = string.Empty;
                    guestBillPaymentBO.BranchName = string.Empty;
                }
                else if (guestBillPaymentBO.PaymentMode == "Card")
                {
                    guestBillPaymentBO.CardNumber = reservationBillPaymentBO.CardNumber;
                    guestBillPaymentBO.CardType = reservationBillPaymentBO.CardType;
                    guestBillPaymentBO.ExpireDate = reservationBillPaymentBO.ExpireDate;
                    guestBillPaymentBO.ChecqueNumber = string.Empty;
                    reservationBillPaymentBO.ChecqueNumber = string.Empty;
                }
                else
                {
                    guestBillPaymentBO.CardReference = string.Empty;
                    guestBillPaymentBO.CardNumber = string.Empty;
                    guestBillPaymentBO.BranchName = string.Empty;
                }

                guestBillPaymentBO.CardHolderName = string.Empty;
                guestBillPaymentBO.PaymentDescription = string.Empty;

                return guestBillPaymentBO;
            }
            else return null;
        }
        protected void gvGuestHouseService_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");

                ReservationBillPaymentBO bo = (ReservationBillPaymentBO)e.Row.DataItem;

                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + bo.PaymentId + "','" + bo.PaymentType + "', '1');";
                imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + bo.PaymentId + "','" + bo.PaymentType + "', '1');";
            }
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
        [WebMethod]
        public static string SearchNLoadReservationInfo(int reservationId, string guestName, string companyName, string reservNumber, string checkInDate, string checkOutDate)
        {
            HMUtility hmUtility = new HMUtility();
            HMCommonDA commonDA = new HMCommonDA();
            GuestInformationDA guestDA = new GuestInformationDA();
            List<RoomReservationInfoByDateRangeReportBO> list = new List<RoomReservationInfoByDateRangeReportBO>();
            DateTime? checkIn = null;
            DateTime? checkOut = null;
            if (!string.IsNullOrWhiteSpace(checkInDate))
                checkIn = hmUtility.GetDateTimeFromString(checkInDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            if (!string.IsNullOrWhiteSpace(checkOutDate))
                checkOut = hmUtility.GetDateTimeFromString(checkOutDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            list = guestDA.GetReservationInfoForRegistration(reservationId, checkIn, checkOut, guestName, companyName, reservNumber);
            return commonDA.GetReservationGridInfo(list);
        }
        [WebMethod]
        public static GuestInformationBO LoadDetailInformation(string GuestId)
        {
            HMCommonDA commonDA = new HMCommonDA();
            GuestInformationBO guestBO = new GuestInformationBO();
            guestBO = commonDA.GetGuestDetailInformation(GuestId);

            GuestPreferenceDA preferenceDA = new GuestPreferenceDA();
            List<GuestPreferenceBO> preferenceList = new List<GuestPreferenceBO>();
            if (!string.IsNullOrEmpty(GuestId))
            {
                preferenceList = preferenceDA.GetGuestPreferenceInfoByGuestId(Convert.ToInt32(GuestId));

                if (preferenceList.Count > 0)
                {
                    foreach (GuestPreferenceBO preference in preferenceList)
                        if (guestBO.GuestPreferences != null)
                        {
                            guestBO.GuestPreferences += ", " + preference.PreferenceName;
                            guestBO.GuestPreferenceId += "," + preference.PreferenceId;
                        }
                        else
                        {
                            guestBO.GuestPreferences = preference.PreferenceName;
                            guestBO.GuestPreferenceId = preference.PreferenceId.ToString();
                        }
                }
            }

            return guestBO;
        }
        [WebMethod]
        public static decimal SearchMaximumRefundAmount(int reservationId, int editId)
        {
            decimal RefundAmount = 0;
            ReservationBillPaymentDA DA = new ReservationBillPaymentDA();
            RefundAmount = DA.GetMaxRefundForReservationBillPayment(reservationId, editId);

            return RefundAmount;
        }
    }
}