using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class frmSalesOrderSearch : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            this.btnPaymentPosting.Visible = false;
            if (!IsPostBack)
            {
                this.CheckObjectPermission();
                this.hfIsDayClosed.Value = "0";
                this.IsRestaurantIntegrateWithPayroll();
                this.IsRestaurantIntegrateWithFrontOffice();
                this.LoadPayRoomInfo();
                this.LoadEmployeeInfo();
                this.LoadRegisteredGuestCompanyInfo();
                this.LoadBank();
                this.LoadRoomNumber();
                this.LoadCommonDropDownHiddenField();
                this.LoadCostCenter();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchInformation();
        }
        protected void gvRestaurantSavedBill_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.SearchInformation();
        }
        protected void gvRestaurantSavedBill_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdBillPreview")
            {
                int billID = Convert.ToInt32(e.CommandArgument.ToString());
                int aaa = Convert.ToInt32(e.CommandArgument.ToString());
                string url = "/POS/Reports/frmReportBillInfo.aspx?billID=" + billID;
                string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            hfIsSavePermission.Value = isSavePermission ? "1" : "0";
            hfIsUpdatePermission.Value = isUpdatePermission ? "1" : "0";
            hfIsDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfIsViewPermission.Value = isViewPermission ? "1" : "0";

            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO setUpBOApprovedBySignature = new HMCommonSetupBO();
            hfImageApprovedBySignature.Value = "0";
            setUpBOApprovedBySignature = commonSetupDA.GetCommonConfigurationInfo("BillingDefaultApprovedBySignature", "BillingDefaultApprovedBySignature");
            if (!string.IsNullOrWhiteSpace(setUpBOApprovedBySignature.SetupValue))
            {
                hfImageApprovedBySignature.Value = setUpBOApprovedBySignature.SetupValue;
            }
        }
        private void SearchInformation()
        {
            string fromDate = string.Empty;
            string toDate = string.Empty;
            if (string.IsNullOrWhiteSpace(this.txtSrcFromDate.Text))
            {
                fromDate = hmUtility.GetFromDate();
            }
            else
            {
                fromDate = this.txtSrcFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtSrcToDate.Text))
            {
                toDate = hmUtility.GetToDate();
            }
            else
            {
                toDate = this.txtSrcToDate.Text;
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            string BillNo = txtBillNumber.Text.Trim();
            LoadGridView(FromDate, ToDate, BillNo);
        }
        private void LoadGridView(DateTime FromDate, DateTime ToDate, string BillNo)
        {
            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int userInfoId = currentUserInformationBO.UserInfoId;
            //int costCenterId = currentUserInformationBO.WorkingCostCenterId;

            int costCenterId = Convert.ToInt32(ddlCostCenter.SelectedValue);

            List<RestaurantBillBO> entityBOList = new List<RestaurantBillBO>();
            RestaurentBillDA entityDA = new RestaurentBillDA();
            entityBOList = entityDA.GetRestaurantBillInfoBySearchCriteria(FromDate, ToDate, BillNo, "", "", userInfoId, costCenterId);
        }
        private void LoadRoomNumber()
        {
            if (hfIsRestaurantIntegrateWithFrontOffice.Value == "1")
            {
                int condition = 0;
                RoomNumberDA roomNumberDA = new RoomNumberDA();
                this.ddlRoomId.DataSource = roomNumberDA.GetRoomNumberInfoByCondition(0, condition);
                this.ddlRoomId.DataTextField = "RoomNumber";
                this.ddlRoomId.DataValueField = "RoomId";
                this.ddlRoomId.DataBind();

                ListItem itemRoom = new ListItem();
                itemRoom.Value = "0";
                itemRoom.Text = hmUtility.GetDropDownFirstValue();
                this.ddlRoomId.Items.Insert(0, itemRoom);
            }
        }
        private void LoadRelatedInformation()
        {
            if (ddlRoomId.SelectedIndex != -1)
            {
                int roomId = Convert.ToInt32(this.ddlRoomId.SelectedValue);
                //  this.LoadRegistrationNumber(roomId);
                //this.LoadServiceGridView();
                //this.LoadFormByRegistrationNumber();
            }
        }
        private void LoadPayRoomInfo()
        {
            if (hfIsRestaurantIntegrateWithFrontOffice.Value == "1")
            {
                RoomNumberDA roomNumberDA = new RoomNumberDA();
                this.ddlRoomNumberId.DataSource = roomNumberDA.GetRoomNumberInfoByCondition(0, 0);
                this.ddlRoomNumberId.DataTextField = "RoomNumber";
                this.ddlRoomNumberId.DataValueField = "RoomId";
                this.ddlRoomNumberId.DataBind();

                ListItem itemRoom = new ListItem();
                itemRoom.Value = "0";
                itemRoom.Text = hmUtility.GetDropDownFirstValue();
                this.ddlRoomNumberId.Items.Insert(0, itemRoom);
            }
        }
        private void LoadEmployeeInfo()
        {
            if (IsRestaurantIntegrateWithPayrollVal.Value == "1")
            {
                EmployeeDA entityDA = new EmployeeDA();
                this.ddlEmpId.DataSource = entityDA.GetEmployeeInfo();
                this.ddlEmpId.DataTextField = "DisplayName";
                this.ddlEmpId.DataValueField = "EmpId";
                this.ddlEmpId.DataBind();

                ListItem itemEmpId = new ListItem();
                itemEmpId.Value = "0";
                itemEmpId.Text = hmUtility.GetDropDownFirstValue();
                this.ddlEmpId.Items.Insert(0, itemEmpId);
            }
        }
        private void LoadRegisteredGuestCompanyInfo()
        {
            GuestCompanyDA companyDa = new GuestCompanyDA();
            this.ddlCompanyName.DataSource = companyDa.GetGuestCompanyInfo();
            this.ddlCompanyName.DataTextField = "CompanyName";
            this.ddlCompanyName.DataValueField = "NodeId";
            this.ddlCompanyName.DataBind();

            ListItem itemCompanyName = new ListItem();
            itemCompanyName.Value = "0";
            itemCompanyName.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCompanyName.Items.Insert(0, itemCompanyName);
        }
        private void LoadAccountHeadInfo()
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
            List<NodeMatrixBO> cardEntityList = new List<NodeMatrixBO>();
            cardEntityList = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CardReceiveAccountsInfo.FieldValue.ToString() + ")");
            this.ddlCardReceiveAccountsInfo.DataSource = cardEntityList;
            this.ddlCardReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCardReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCardReceiveAccountsInfo.DataBind();

            this.ddlChecquePaymentAccountHeadId.DataSource = cardEntityList;
            this.ddlChecquePaymentAccountHeadId.DataTextField = "NodeHead";
            this.ddlChecquePaymentAccountHeadId.DataValueField = "NodeId";
            this.ddlChecquePaymentAccountHeadId.DataBind();

            this.ddlCardPaymentAccountHeadId.DataSource = cardEntityList;
            this.ddlCardPaymentAccountHeadId.DataTextField = "NodeHead";
            this.ddlCardPaymentAccountHeadId.DataValueField = "NodeId";
            this.ddlCardPaymentAccountHeadId.DataBind();

            CustomFieldBO CompanyPaymentAccountsInfo = new CustomFieldBO();
            CompanyPaymentAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CompanyPaymentAccountsInfo");
            this.ddlCompanyPaymentAccountHead.DataSource = entityDA.GetNodeMatrixInfoByAncestorNodeId(Convert.ToInt32(CompanyPaymentAccountsInfo.FieldValue));
            this.ddlCompanyPaymentAccountHead.DataTextField = "NodeHead";
            this.ddlCompanyPaymentAccountHead.DataValueField = "NodeId";
            this.ddlCompanyPaymentAccountHead.DataBind();

            CustomFieldBO PaymentToAccountsInfo = new CustomFieldBO();
            PaymentToAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("PaymentToCustomerForCashOut");
            this.ddlRefundAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + PaymentToAccountsInfo.FieldValue.ToString() + ")");
            this.ddlRefundAccountHead.DataTextField = "NodeHead";
            this.ddlRefundAccountHead.DataValueField = "NodeId";
            this.ddlRefundAccountHead.DataBind();

            CustomFieldBO EmployeePaymentToAccountsInfo = new CustomFieldBO();
            EmployeePaymentToAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("EmployeePaymentAccountsInfo");
            this.ddlEmployeePaymentAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + EmployeePaymentToAccountsInfo.FieldValue.ToString() + ")");
            this.ddlEmployeePaymentAccountHead.DataTextField = "NodeHead";
            this.ddlEmployeePaymentAccountHead.DataValueField = "NodeId";
            this.ddlEmployeePaymentAccountHead.DataBind();
        }
        private void LoadBank()
        {
            BankDA da = new BankDA();
            List<BankBO> files = da.GetBankInfo();
            this.ddlBankName.DataSource = files;
            this.ddlBankName.DataTextField = "BankName";
            this.ddlBankName.DataValueField = "BankId";
            this.ddlBankName.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBankName.Items.Insert(0, item);

            this.ddlBankId.DataSource = files;
            this.ddlBankId.DataTextField = "BankName";
            this.ddlBankId.DataValueField = "BankId";
            this.ddlBankId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBankId.Items.Insert(0, itemBank);
        }
        public void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> AllList = new List<CostCentreTabBO>();
            AllList = costCentreTabDA.GetCostCentreTabInfoByType("Restaurant,RetailPos,OtherOutlet,Billing");

            List<CostCentreTabBO> shortList = new List<CostCentreTabBO>();
            CostCentreTabBO bo = new CostCentreTabBO();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int userInfoId = currentUserInformationBO.UserInfoId;

            RestaurantBearerDA restaurantBearerDA = new RestaurantBearerDA();
            List<RestaurantBearerBO> costCenterInfoBOList = new List<RestaurantBearerBO>();
            costCenterInfoBOList = restaurantBearerDA.GetRestaurantInfoForBearerByEmpIdNIsBearer(userInfoId, 0);

            if (costCenterInfoBOList.Count == 0)
            {
                shortList = AllList;
            }
            else
            {
                foreach (RestaurantBearerBO rb in costCenterInfoBOList)
                {
                    bo = AllList.Where(a => a.CostCenterId == rb.CostCenterId).FirstOrDefault();
                    if (bo != null)
                    {
                        if (bo.CostCenterId > 0)
                        {
                            shortList.Add(bo);
                        }
                    }
                }
            }

            //this.ddlCostCenter.DataSource = shortList.GroupBy(test => test.CostCenterId).Select(group => group.First()).ToList();

            this.ddlCostCenter.DataSource = shortList;
            this.ddlCostCenter.DataTextField = "CostCenter";
            this.ddlCostCenter.DataValueField = "CostCenterId";
            this.ddlCostCenter.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCostCenter.Items.Insert(0, item);
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void IsRestaurantIntegrateWithFrontOffice()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithFrontOffice", "IsRestaurantIntegrateWithFrontOffice");
            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        hfIsRestaurantIntegrateWithFrontOffice.Value = "0";
                        ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("2"));
                    }
                    else
                    {
                        hfIsRestaurantIntegrateWithFrontOffice.Value = "1";
                    }
                }
            }
        }
        private void IsRestaurantIntegrateWithPayroll()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithPayroll", "IsRestaurantIntegrateWithPayroll");
            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        IsRestaurantIntegrateWithPayrollVal.Value = "0";
                        ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("Employee"));
                    }
                    else
                    {
                        IsRestaurantIntegrateWithPayrollVal.Value = "1";
                    }
                }
            }
        }

        [WebMethod]
        public static List<DocumentsBO> GetUploadedDocumentsByWebMethod(int OwnerId, string docType)
        {
            DocumentsDA docDA = new DocumentsDA();
            List<DocumentsBO> docBO = new List<DocumentsBO>();

            docBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("Employee Other Documents", OwnerId);

            //DocumentsDA docDA = new DocumentsDA();
            //var docList = docDA.GetDocumentsInfoByDocCategoryAndOwnerId(docType, OwnerId);
            docBO = new HMCommonDA().GetDocumentListWithIcon(docBO);
            return docBO;
        }

        [WebMethod]
        public static bool DeleteDoc(long docId)
        {

            DocumentsDA docDA = new DocumentsDA();
            var status = docDA.DeleteDocumentsByDocumentId(docId);

            return status;
        }
        protected void ddlRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadRelatedInformation();
        }
        protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.LoadAccountHeadInfo();
        }
        protected void btnPaymentPosting_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime restaurantBillDate = DateTime.Now;

                string transactionHead = string.Empty;
                HMCommonDA hmCommonDA = new HMCommonDA();
                CustomFieldBO customField = new CustomFieldBO();

                customField = hmCommonDA.GetCustomFieldByFieldName("GuestBillPayment");

                if (customField == null)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please Contact With Administrator for Accounts Mapping.", AlertType.Warning);
                    return;
                }
                else
                {
                    List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

                    RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
                    int billID = 0;
                    string deletedPaymentIds = string.Empty;
                    string deletedTransferedPaymentIds = string.Empty;
                    string deletedAchievementPaymentIds = string.Empty;

                    guestPaymentDetailListForGrid = guestPaymentDetailListForGrid.Where(g => g.ServiceBillId == null).ToList();

                    if (txtBillId.Value.Trim() != string.Empty)
                    {
                        billID = Convert.ToInt32(txtBillId.Value);

                        RestaurantBillBO billBO = new RestaurantBillBO();
                        RestaurentBillDA billDA = new RestaurentBillDA();
                        billBO = billDA.GetBillInfoByBillId(billID);
                        if (billBO.BillId > 0)
                        {
                            restaurantBillDate = billBO.BillDate;

                            foreach (GuestBillPaymentBO row in guestPaymentDetailListForGrid)
                            {
                                row.CreatedBy = billBO.CreatedBy;
                            }
                        }
                    }

                    if (hfDeletedPaymentForPaymentId.Value.Trim() != "")
                    {
                        deletedPaymentIds = hfDeletedPaymentForPaymentId.Value.Trim();
                    }

                    if (hfDeletedPaymentForTransferedPaymentId.Value.Trim() != "")
                    {
                        deletedTransferedPaymentIds = hfDeletedPaymentForTransferedPaymentId.Value.Trim();
                    }

                    if (hfDeletedPaymentForAchievementPaymentId.Value.Trim() != "")
                    {
                        deletedAchievementPaymentIds = hfDeletedPaymentForAchievementPaymentId.Value.Trim();
                    }

                    int success = restaurentBillDA.SaveUpdateRestaurantBill(restaurantBillDate, guestPaymentDetailListForGrid, deletedPaymentIds, deletedTransferedPaymentIds, deletedAchievementPaymentIds, billID);

                    if (success > 0)
                    {
                        Boolean updateSuccess = restaurentBillDA.UpdateRestaurantBillRegistrationIdInfoForOtherRoomPayment(billID);
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);

                        DateTime FromDate = hmUtility.GetDateTimeFromString(txtSrcFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                        string BillNo = txtBillNumber.Text.Trim();

                        txtBillNumber.Text = BillNo;
                        txtBillId.Value = billID.ToString();
                        hfDeletedPaymentForPaymentId.Value = string.Empty;
                        hfDeletedPaymentForTransferedPaymentId.Value = string.Empty;
                        hfDeletedPaymentForAchievementPaymentId.Value = string.Empty;
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);

            }
        }
        //************************ User Defined Web Method ********************//
        [WebMethod(EnableSession = true)]
        public static string PerformPaymentPostingByWebMethod(string txtBillId, string hfDeletedPaymentForPaymentId, string hfDeletedPaymentForTransferedPaymentId, string hfDeletedPaymentForAchievementPaymentId)
        {
            string ReturnResult = string.Empty;
            try
            {
                DateTime restaurantBillDate = DateTime.Now;
                string transactionHead = string.Empty;
                HMCommonDA hmCommonDA = new HMCommonDA();
                CustomFieldBO customField = new CustomFieldBO();

                customField = hmCommonDA.GetCustomFieldByFieldName("GuestBillPayment");

                if (customField == null)
                {
                    //CommonHelper.AlertInfo(innboardMessage, "Please Contact With Administrator for Accounts Mapping.", AlertType.Warning);
                    //return;
                }
                else
                {
                    List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

                    RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
                    int billID = 0;
                    string deletedPaymentIds = string.Empty;
                    string deletedTransferedPaymentIds = string.Empty;
                    string deletedAchievementPaymentIds = string.Empty;

                    guestPaymentDetailListForGrid = guestPaymentDetailListForGrid.Where(g => g.ServiceBillId == null).ToList();

                    //if (txtBillId.Value.Trim() != string.Empty)
                    if (txtBillId.Trim() != string.Empty)
                    {
                        billID = Convert.ToInt32(txtBillId.Trim());

                        RestaurantBillBO billBO = new RestaurantBillBO();
                        RestaurentBillDA billDA = new RestaurentBillDA();
                        billBO = billDA.GetBillInfoByBillId(billID);
                        if (billBO.BillId > 0)
                        {
                            restaurantBillDate = billBO.BillDate;

                            foreach (GuestBillPaymentBO row in guestPaymentDetailListForGrid)
                            {
                                row.CreatedBy = billBO.CreatedBy;
                            }
                        }
                    }

                    if (hfDeletedPaymentForPaymentId.Trim() != "")
                    {
                        deletedPaymentIds = hfDeletedPaymentForPaymentId.Trim();
                    }

                    if (hfDeletedPaymentForTransferedPaymentId.Trim() != "")
                    {
                        deletedTransferedPaymentIds = hfDeletedPaymentForTransferedPaymentId.Trim();
                    }

                    if (hfDeletedPaymentForAchievementPaymentId.Trim() != "")
                    {
                        deletedAchievementPaymentIds = hfDeletedPaymentForAchievementPaymentId.Trim();
                    }

                    int success = restaurentBillDA.SaveUpdateRestaurantBill(restaurantBillDate, guestPaymentDetailListForGrid, deletedPaymentIds, deletedTransferedPaymentIds, deletedAchievementPaymentIds, billID);

                    if (success > 0)
                    {
                        ReturnResult = "1";
                        Boolean updateSuccess = restaurentBillDA.UpdateRestaurantBillRegistrationIdInfoForOtherRoomPayment(billID);
                    }
                    else
                    {
                        ReturnResult = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnResult = "0";
                throw ex;
            }

            return ReturnResult;
        }
        [WebMethod(EnableSession = true)]
        public static string PerformSaveGuestPaymentDetailsInformationByWebMethod(bool isEdit, string paymentDescription, string ddlPayMode, string ddlBankId, string txtReceiveLeadgerAmount, string ddlRegistrationId, string ddlCashPaymentAccountHead, string txtCardNumber, string ddlCardType, string txtExpireDate, string txtCardHolderName, string txtChecqueNumber, string ddlChecquePaymentAccountHeadId, string ddlCardPaymentAccountHeadId, string ddlCompanyPaymentAccountHead, string ddlPaidByRoomId, string RefundAccountHead, string ddlEmpId, string ddlEmployeePaymentAccountHead)
        {
            HMUtility hmUtility = new HMUtility();
            int dynamicDetailId = 0;
            int ddlPaidByRegistrationId = 0;

            List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

            GuestBillPaymentBO singleEntityBOEdit = guestPaymentDetailListForGrid.Where(x => x.PaymentMode == ddlPayMode).FirstOrDefault();

            if (guestPaymentDetailListForGrid != null)
            {
                dynamicDetailId = guestPaymentDetailListForGrid.Count + 25;
            }

            GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();

            if (ddlPayMode == "Other Room")
            {
                if (!string.IsNullOrWhiteSpace(ddlPaidByRoomId))
                {
                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                    List<RoomRegistrationBO> billPaidByInfoList = new List<RoomRegistrationBO>();

                    billPaidByInfoList = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(Convert.ToInt32(ddlPaidByRoomId));
                    if (billPaidByInfoList != null)
                    {
                        foreach (RoomRegistrationBO row in billPaidByInfoList)
                        {
                            ddlPaidByRegistrationId = row.RegistrationId;
                        }
                    }
                    else
                    {
                        ddlPaidByRegistrationId = Convert.ToInt32(ddlRegistrationId);
                    }
                }
                else
                {
                    ddlPaidByRegistrationId = Convert.ToInt32(ddlRegistrationId);
                }
            }
            else
            {
                ddlPaidByRegistrationId = Convert.ToInt32(ddlRegistrationId);
            }

            if (ddlPayMode == "Company")
            {
                guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCompanyPaymentAccountHead);
                guestBillPaymentBO.PaymentType = ddlPayMode;
                guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCompanyPaymentAccountHead);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlRegistrationId);
            }
            else if (ddlPayMode == "Employee")
            {
                guestBillPaymentBO.NodeId = Convert.ToInt32(ddlEmployeePaymentAccountHead);
                guestBillPaymentBO.PaymentType = ddlPayMode;
                guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlEmployeePaymentAccountHead);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlPaidByRegistrationId);
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlEmpId);
                guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlEmpId);
            }
            else if (ddlPayMode == "Other Room")
            {
                guestBillPaymentBO.NodeId = Convert.ToInt32(1);
                guestBillPaymentBO.PaymentType = ddlPayMode;
                guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(1);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlPaidByRegistrationId);
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlPaidByRegistrationId);
                guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlPaidByRegistrationId);
            }
            else if (ddlPayMode == "Refund")
            {
                guestBillPaymentBO.RefundAccountHead = Int32.Parse(RefundAccountHead);
                guestBillPaymentBO.PaymentMode = "Refund";
                guestBillPaymentBO.CurrencyAmount = guestBillPaymentBO.CurrencyAmount * 1;
                guestBillPaymentBO.PaymentAmount = guestBillPaymentBO.PaymentAmount * 1;
                guestBillPaymentBO.PaymentType = "Refund";
                guestBillPaymentBO.AccountsPostingHeadId = Int32.Parse(RefundAccountHead);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlRegistrationId);
            }
            else
            {
                if (ddlPayMode == "Cash")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCashPaymentAccountHead);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPaymentAccountHead);
                }
                else if (ddlPayMode == "Card")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCardPaymentAccountHeadId);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCardPaymentAccountHeadId);
                }
                else if (ddlPayMode == "Checque")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlChecquePaymentAccountHeadId);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlChecquePaymentAccountHeadId);
                }

                guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlBankId);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.PaymentType = "Advance";
            }

            guestBillPaymentBO.FieldId = 1; // Convert.ToInt32(ddlCurrency);
            guestBillPaymentBO.ConvertionRate = 1;
            guestBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            guestBillPaymentBO.PaymentAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            guestBillPaymentBO.ChecqueDate = DateTime.Now;
            guestBillPaymentBO.PaymentMode = ddlPayMode;
            guestBillPaymentBO.PaymentId = dynamicDetailId;
            guestBillPaymentBO.CardNumber = txtCardNumber;
            guestBillPaymentBO.CardType = ddlCardType;
            if (string.IsNullOrEmpty(txtExpireDate))
            {
                guestBillPaymentBO.ExpireDate = null;
            }
            else
            {
                guestBillPaymentBO.ExpireDate = hmUtility.GetDateTimeFromString(txtExpireDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            guestBillPaymentBO.ChecqueNumber = txtChecqueNumber;
            guestBillPaymentBO.CardHolderName = txtCardHolderName;

            guestBillPaymentBO.PaymentDescription = paymentDescription;


            guestBillPaymentBO.PaymentId = dynamicDetailId;

            guestPaymentDetailListForGrid.Add(guestBillPaymentBO);
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;

            return dynamicDetailId.ToString() + "#" + paymentDescription;
        }
        [WebMethod(EnableSession = true)]
        public static string PerformDeleteGuestPaymentByWebMethod(string transactionType, int paymentId)
        {
            List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            GuestBillPaymentBO singleEntityBOEdit = guestPaymentDetailListForGrid.Where(x => x.PaymentId == paymentId).FirstOrDefault();
            if (guestPaymentDetailListForGrid.Contains(singleEntityBOEdit))
            {
                guestPaymentDetailListForGrid.Remove(singleEntityBOEdit);
            }

            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;
            HttpContext.Current.Session["CompanyPaymentRoomIdList"] = null;
            HttpContext.Current.Session["CompanyPaymentServiceIdList"] = null;

            return (transactionType + paymentId.ToString());
        }
        [WebMethod]
        public static List<GuestBillPaymentBO> GetRestaurantBillPayment(string strBillId)
        {
            int billId = !string.IsNullOrWhiteSpace(strBillId) ? Convert.ToInt32(strBillId) : 0;
            GuestBillPaymentDA paymentDa = new GuestBillPaymentDA();
            List<GuestBillPaymentBO> billPaymentList = new List<GuestBillPaymentBO>();
            if (billId > 0)
            {
                billPaymentList = paymentDa.GetGuestBillPaymentInfoByServiceId("Restaurant", billId);
            }
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = billPaymentList;

            return billPaymentList;
        }
        [WebMethod]
        public static GridViewDataNPaging<RestaurantBillBO, GridPaging> SearchBillAndLoadGridInformation(string srcFromDate, string srcToDate, string billNo, string customerInfo, string remarks, string costCenterId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int userInfoId = currentUserInformationBO.UserInfoId;
            int costCenter = Convert.ToInt32(costCenterId);

            int totalRecords = 0;

            GridViewDataNPaging<RestaurantBillBO, GridPaging> myGridData = new GridViewDataNPaging<RestaurantBillBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            RestaurentBillDA rrDA = new RestaurentBillDA();
            List<RestaurantBillBO> reservationInfoList = new List<RestaurantBillBO>();

            DateTime fromDate = DateTime.Now.AddDays(-30);

            if (!string.IsNullOrWhiteSpace(billNo))
            {
                fromDate = DateTime.Now.AddYears(-10);
            }

            DateTime toDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(srcFromDate))
            {
                fromDate = hmUtility.GetDateTimeFromString(srcFromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(srcToDate))
            {
                toDate = hmUtility.GetDateTimeFromString(srcToDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            reservationInfoList = rrDA.GetSalesOrderInfoBySearchCriteriaForpagination(fromDate, toDate, billNo, customerInfo, remarks, userInfoId, costCenter, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            CustomFieldBO fieldBO = new CustomFieldBO();
            fieldBO = commonDA.GetCustomFieldByFieldName("RemarksDetailsForBilling");
            if (fieldBO.FieldId != 0)
            {
                if (fieldBO.ActiveStat == true)
                {
                    if (!string.IsNullOrWhiteSpace(fieldBO.FieldValue))
                    {
                        foreach (RestaurantBillBO row in reservationInfoList)
                        {
                            if (row.Remarks.Length > 101)
                            {
                                row.Remarks = row.Remarks.Substring(0, 100) + "...";
                            }
                        }
                    }
                }
            }

            List<RestaurantBillBO> distinctItems = new List<RestaurantBillBO>();
            distinctItems = reservationInfoList.GroupBy(test => test.BillId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static bool UpdateRestaurantBill(string billStatus, string remarks, int billId, int costcenterId)
        {
            bool status = false;

            RestaurentBillDA restaurantBillDA = new RestaurentBillDA();
            RestaurantBillBO restaurantBillBO = new RestaurantBillBO();
            restaurantBillBO.BillStatus = billStatus;
            restaurantBillBO.Remarks = remarks;
            restaurantBillBO.BillId = billId;
            restaurantBillBO.CostCenterId = costcenterId;

            UserInformationBO userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            restaurantBillBO.BillVoidBy = userInformationBO.UserInfoId;
            restaurantBillBO.LastModifiedBy = userInformationBO.UserInfoId;

            status = restaurantBillDA.UpdateRestaurantBill(restaurantBillBO);

            if (status)
            {
                //Activity Log Process for Bill Preview...........
                HMUtility hmUtility = new HMUtility();
                long registrationId = restaurantBillBO.BillId;
                Boolean logStatus = hmUtility.CreateActivityLogEntity("Restaurant Bill Search", EntityTypeEnum.EntityType.RestaurantBill.ToString(), registrationId,
                                    ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Bill Status: " + restaurantBillBO.BillStatus);
                return true;
            }
            else return false;
        }
        [WebMethod]
        public static ReturnInfo BillResettlement(int billId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            string sourceType = string.Empty;

            try
            {
                RestaurentBillDA billDa = new RestaurentBillDA();
                KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();
                KotBillMasterBO kotBill = new KotBillMasterBO();
                List<RestaurantBillDetailBO> billDetailList = new List<RestaurantBillDetailBO>();

                UserInformationBO userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

                billDetailList = billDa.GetRestaurantBillDetailsByBillId(billId);

                if (billDetailList.Count > 0)
                {
                    kotBill = kotBillMasterDA.GetKotBillMasterInfoKotId(billDetailList[0].KotId);
                    billDa.RestaurantBillReSettlementLog(billId, userInformationBO.UserInfoId);
                }

                if (kotBill.SourceName == ConstantHelper.RestaurantBillSource.RestaurantTable.ToString())
                {
                    sourceType = "tbl";
                }
                else if (kotBill.SourceName == ConstantHelper.RestaurantBillSource.RestaurantToken.ToString())
                {
                    sourceType = "tkn";
                }
                else if (kotBill.SourceName == ConstantHelper.RestaurantBillSource.GuestRoom.ToString())
                {
                    sourceType = "rom";
                }

                rtninf.IsSuccess = true;
                rtninf.RedirectUrl = string.Format("frmBillReSettlement.aspx?ot={0}&st={1}&sid={2}&cc={3}&kid={4}&dp={5}", "br", sourceType, kotBill.SourceId, kotBill.CostCenterId, kotBill.KotId, "../POS/frmBillSearch.aspx");

                //rtninf.RedirectUrl = "frmRestaurantBillReSettlement.aspx?IR=TokenAllocation&CostCenterId=" + kotBill.CostCenterId;

                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return rtninf;
        }
    }
}