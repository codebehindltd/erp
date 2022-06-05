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
using HotelManagement.Entity.Banquet;
using HotelManagement.Data.Banquet;

namespace HotelManagement.Presentation.Website.Banquet
{
    public partial class frmBanquetBillSearch : BasePage
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
                CheckPermission();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchVoucher();
        }
        protected void gvRestaurantSavedBill_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.SearchVoucher();
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
        private void CheckPermission()
        {
            hfIsSavePermission.Value = isSavePermission == true ? "1" : "0";
            hfIsUpdatePermission.Value = isUpdatePermission == true ? "1" : "0";
            hfIsDeletePermission.Value = isDeletePermission == true ? "1" : "0";
            hfIsViewPermission.Value = isViewPermission == true ? "1" : "0";
        }
        private void SearchVoucher()
        {
            string fromDate = string.Empty;
            string toDate = string.Empty;
            if (string.IsNullOrWhiteSpace(this.txtSearchDate.Text))
            {
                fromDate = hmUtility.GetFromDate();
            }
            else
            {
                fromDate = this.txtSearchDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtSearchDate.Text))
            {
                toDate = hmUtility.GetToDate();
            }
            else
            {
                toDate = this.txtSearchDate.Text;
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
            int costCenterId = currentUserInformationBO.WorkingCostCenterId;

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
            AllList = costCentreTabDA.GetCostCentreTabInfo();

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
                    shortList.Add(bo);
                }
            }

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
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.BanquetBillPayment.ToString(), billID, ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetBillPayment));
                        Boolean updateSuccess = restaurentBillDA.UpdateRestaurantBillRegistrationIdInfoForOtherRoomPayment(billID);
                        if(updateSuccess)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.BanquetBillPayment.ToString(), billID, ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetBillPayment));
                        }
                        DateTime FromDate = hmUtility.GetDateTimeFromString(txtSearchDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
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

                    BanquetReservationDA restaurentBillDA = new BanquetReservationDA();
                    long billID = 0;
                    string deletedPaymentIds = string.Empty;
                    string deletedTransferedPaymentIds = string.Empty;
                    string deletedAchievementPaymentIds = string.Empty;

                    guestPaymentDetailListForGrid = guestPaymentDetailListForGrid.Where(g => g.ServiceBillId == null).ToList();

                    if (txtBillId.Trim() != string.Empty)
                    {
                        billID = Convert.ToInt64(txtBillId.Trim());

                        BanquetReservationBO billBO = new BanquetReservationBO();
                        billBO = restaurentBillDA.GetBanquetReservationInfoById(billID);
                        if (billBO.Id > 0)
                        {
                            restaurantBillDate = billBO.ArriveDate;

                            foreach (GuestBillPaymentBO row in guestPaymentDetailListForGrid)
                            {
                                row.CreatedBy =Convert.ToInt32(billBO.CreatedBy);
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

                    int success = restaurentBillDA.SaveUpdateBanquetBill(restaurantBillDate, guestPaymentDetailListForGrid, deletedPaymentIds, deletedTransferedPaymentIds, deletedAchievementPaymentIds,Convert.ToInt32(billID));

                    if (success > 0)
                    {
                        ReturnResult = "1";
                        Boolean updateSuccess = restaurentBillDA.UpdateBanquetBillRegistrationIdInfoForOtherRoomPayment(Convert.ToInt32(billID));
                    }
                    else
                    {
                        ReturnResult = "0";
                    }
                }
            }

            catch(Exception ex)
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
                billPaymentList = paymentDa.GetGuestBillPaymentInfoByServiceId("Banquet", billId);
            }
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = billPaymentList;

            return billPaymentList;
        }
        [WebMethod]
        public static GridViewDataNPaging<BanquetReservationBO, GridPaging> SearchBillAndLoadGridInformation(string strSearchDate, string billNo, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int userInfoId = currentUserInformationBO.UserInfoId;

            int totalRecords = 0;

            GridViewDataNPaging<BanquetReservationBO, GridPaging> myGridData = new GridViewDataNPaging<BanquetReservationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            BanquetReservationDA rrDA = new BanquetReservationDA();
            List<BanquetReservationBO> reservationInfoList = new List<BanquetReservationBO>();

            DateTime fromDate = DateTime.Now.AddDays(-30);

            if (!string.IsNullOrWhiteSpace(billNo))
            {
                fromDate = DateTime.Now.AddYears(-10);
            }

            DateTime toDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(strSearchDate))
            {
                fromDate = hmUtility.GetDateTimeFromString(strSearchDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(strSearchDate))
            {
                toDate = hmUtility.GetDateTimeFromString(strSearchDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            //reservationInfoList = rrDA.GetBanquetReservationInfoForBillSearch(fromDate, toDate, billNo, userInformationBO.GridViewPageSize, pageNumber, out totalRecords).Where(x => x.IsBillSettlement != false).ToList();
            reservationInfoList = rrDA.GetBanquetReservationInfoForBillSearch(fromDate, toDate, billNo, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<BanquetReservationBO> distinctItems = new List<BanquetReservationBO>();
            distinctItems = reservationInfoList.GroupBy(test => test.Id).Select(group => group.First()).ToList();

            //HMCommonDA hmCoomnoDA = new HMCommonDA();
            //DayCloseBO dayCloseBO = new DayCloseBO();

            //DateTime transactionDate = !string.IsNullOrWhiteSpace(strSearchDate) ? hmUtility.GetDateTimeFromString(strSearchDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) : DateTime.Now;

            //foreach (BanquetReservationBO row in distinctItems)
            //{
            //    dayCloseBO = hmCoomnoDA.GetHotelDayCloseInformation(transactionDate);
            //    if (dayCloseBO != null)
            //    {
            //        if (dayCloseBO.DayCloseId > 0)
            //        {
            //            row.IsDayClosed = 1;
            //        }
            //    }
            //}

            myGridData.GridPagingProcessing(distinctItems, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static bool UpdateRestaurantBill(string billStatus, string remarks, int billId, int costcenterId)
        {
            bool status = false;
           
            BanquetReservationDA banquetReservationDA = new BanquetReservationDA();
            BanquetReservationBO banquetReservationBO = new BanquetReservationBO();
            banquetReservationBO.BillStatus = billStatus;
            banquetReservationBO.Remarks = remarks;
            banquetReservationBO.Id = billId;
            banquetReservationBO.CostCenterId = costcenterId;

            UserInformationBO userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            banquetReservationBO.BillVoidBy = userInformationBO.UserInfoId;
            banquetReservationBO.LastModifiedBy = userInformationBO.UserInfoId;

            status = banquetReservationDA.UpdateBanquetReservationBillStatus(banquetReservationBO);

            if (status)
                return true;
            else return false;
        }       
    }
}