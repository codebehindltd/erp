using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HMCommon;
using System.Configuration;
using System.Globalization;
using HotelManagement.Data;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using System.Web.Services;
using HotelManagement.Entity;
using HotelManagement.Data.Banquet;
using HotelManagement.Entity.Banquet;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using Newtonsoft.Json;
using System.Collections;
using HotelManagement.Data.LCManagement;
using HotelManagement.Entity.LCManagement;

namespace HotelManagement.Presentation.Website.LCManagement
{
    public partial class frmLCSettlement : System.Web.UI.Page
    {
        protected bool isSingle = true;
        HiddenField innboardMessage;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        HMUtility hmUtility = new HMUtility();
        protected int isCompanyProjectPanelEnable = -1;
        protected int isIntegratedGeneralLedgerDiv = 1;
        protected int LocalCurrencyId;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            isSingle = hmUtility.GetSingleProjectAndCompany();
            if (!IsPostBack)
            {
                this.pnlBillPrintPreview.Visible = false;
                HttpContext.Current.Session["BillPreviewCurrencyRateInformation"] = null;
                this.LoadBank();
                this.LoadLabelForSalesTotal();
                this.ClearCommonSessionInformation();
                this.LoadRegisteredGuestCompanyInfo();
                hfGuestPaymentDetailsInformationDiv.Value = "1";
                string cardValidation = System.Web.Configuration.WebConfigurationManager.AppSettings["CardValidation"].ToString();
                txtCardValidation.Value = cardValidation.ToString();
                this.CheckObjectPermission();
                this.LoadRoomNumber();
                this.LoadCurrency();
                LoadIsConversionRateEditable();
                IsLocalCurrencyDefaultSelected();
                this.LoadCommonDropDownHiddenField();
                this.LoadAccountHeadInfo();
                Session["TransactionDetailList"] = null;
                Session["GuestPaymentDetailListForGrid"] = null;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //this.Cancel();
            Clear();
            Response.Redirect("/LCManagement/frmLCSettlement.aspx");
        }
        protected void gvGuestHouseCheckOut_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //this.gvGuestHouseCheckOut.PageIndex = e.NewPageIndex;
            //this.LoadGridView();
        }
        protected void gvGuestHouseCheckOut_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "CmdEdit")
            //{
            //    this._BillId = Convert.ToInt32(e.CommandArgument.ToString());
            //    Session["_RoomTypeId"] = this._BillId;
            //    this.FillForm(this._BillId);
            //}
            //else if (e.CommandName == "CmdDelete")
            //{
            //    try
            //    {
            //        this._BillId = Convert.ToInt32(e.CommandArgument.ToString());
            //        Session["_RoomTypeId"] = this._BillId;
            //        this.DeleteData(this._BillId);
            //        this.Cancel();
            //        this.LoadGridView();

            //    }
            //    catch
            //    {
            //    }
            //}
        }
        protected void ddlRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadRelatedInformation();
        }
        protected void btnBillPreview_Click(object sender, EventArgs e)
        {
            if (this.ddlRoomId.SelectedValue != "0")
            {
                //this.GoToPrintPreviewReport("0");
                //Response.Redirect("/HotelManagement/Reports/frmReportGuestBillInfo.aspx");
            }
            else
            {
                //this.isMessageBoxEnable = 1;
                //lblMessage.Text = "Please select a Room Number.";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "a Room Number.", AlertType.Warning);
                this.txtSrcBillNumber.Focus();
            }
        }
        protected void btnSrcBillNumber_Click(object sender, EventArgs e)
        {
            this.SrcBillNumberProcess();            
        }
        protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadAccountHeadInfo();
        }
        protected void gvIndividualServiceInformationForBillSplit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ((CheckBox)e.Row.FindControl("chkIsSelected")).Checked = true;
            }
        }
        protected void gvGroupServiceInformationForBillSplit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ((CheckBox)e.Row.FindControl("chkIsSelected")).Checked = true;
            }
        }
        protected void btnUpdateReservation_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            BanquetReservationBO reservationBO = new BanquetReservationBO();
            BanquetReservationDA reservationDA = new BanquetReservationDA();

            reservationBO.ReservationId = Int32.Parse(ddlReservationId.SelectedValue);
            reservationBO.CostCenterId = !string.IsNullOrWhiteSpace(hfCostcenterId.Value) ? Convert.ToInt32(hfCostcenterId.Value) : 0;
            reservationBO.DiscountType = hfDiscountType.Value;
            reservationBO.DiscountAmount = !string.IsNullOrWhiteSpace(hfDiscountAmount.Value) ? Convert.ToDecimal(hfDiscountAmount.Value) : 0;
            reservationBO.NumberOfPersonAdult = !string.IsNullOrWhiteSpace(hfNumberOfPersonAdult.Value) ? Convert.ToInt32(hfNumberOfPersonAdult.Value) : 0;
            reservationBO.NumberOfPersonChild = !string.IsNullOrWhiteSpace(hfNumberOfPersonChild.Value) ? Convert.ToInt32(hfNumberOfPersonChild.Value) : 0;

            // -------------------------------------------------------------------------------------------------------------------------------------------
            List<BanquetReservationDetailBO> addList = new List<BanquetReservationDetailBO>();
            // List<BanquetReservationDetailBO> editList = new List<BanquetReservationDetailBO>();
            List<BanquetReservationDetailBO> deleteList = new List<BanquetReservationDetailBO>();
            List<BanquetReservationClassificationDiscountBO> discountLst = new List<BanquetReservationClassificationDiscountBO>();
            List<BanquetReservationClassificationDiscountBO> discountDeletedLst = new List<BanquetReservationClassificationDiscountBO>();

            addList = JsonConvert.DeserializeObject<List<BanquetReservationDetailBO>>(hfSaveObj.Value);
            //editList = JsonConvert.DeserializeObject<List<BanquetReservationDetailBO>>(hfEditObj.Value);
            deleteList = JsonConvert.DeserializeObject<List<BanquetReservationDetailBO>>(hfDeleteObj.Value);
            discountLst = JsonConvert.DeserializeObject<List<BanquetReservationClassificationDiscountBO>>(hfClassificationDiscountSave.Value);
            // -------------------------------------------------------------------------------------------------------------------------------------------

            Boolean status = reservationDA.UpdateBanquetReservationInfoForAddMoreItem(reservationBO, addList, deleteList, Session["arrayDelete"] as ArrayList, discountLst, discountDeletedLst);

            if (status)
            {
                CommonHelper.AlertInfo(innboardMessage, "Item Added Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.BanquetReservation.ToString(), reservationBO.ReservationId,
                    ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetReservation));
                this.SrcBillNumberProcess(); 
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            LCInformationBO lcInformationBO = new LCInformationBO();
            LCInformationDA lcInformationDA = new LCInformationDA();

            lcInformationBO.LCId = Int32.Parse(ddlReservationId.SelectedValue);
            lcInformationBO.IsLCSettlement = true;
            lcInformationBO.SettlementBy = userInformationBO.UserInfoId;
            lcInformationBO.SettlementDescription = this.txtRemarks.Text;

            //decimal totalSalesAmount = !string.IsNullOrWhiteSpace(HiddenFieldSalesTotal.Value) ? Convert.ToDecimal(HiddenFieldSalesTotal.Value) : 0;
            //decimal grandTotalAmount = !string.IsNullOrWhiteSpace(HiddenFieldGrandTotal.Value) ? Convert.ToDecimal(HiddenFieldGrandTotal.Value) : 0;
            //if (totalSalesAmount != grandTotalAmount)
            //{
            //    GuestCheckOutDiscount(Int32.Parse(this.ddlReservationId.SelectedValue));
            //}

            if (btnSave.Text.Equals("Settlement"))
            {
                Boolean status = lcInformationDA.LCSettlementInformation(lcInformationBO);
                if (status)
                {
                    Clear();
                    txtSrcBillNumber.Text = string.Empty;
                    Session["HiddenFieldCompanyPaymentButtonInfo"] = null;
                    CommonHelper.AlertInfo(innboardMessage, "LC Settlement Successfull.", AlertType.Success);
                    //Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.CheckOut.ToString(), EntityTypeEnum.EntityType.RoomCheckOut.ToString(), MasterId,
                    //    ProjectModuleEnum.ProjectModule.HotelManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomCheckOut));
                    //string url = "/Banquet/Reports/frmReportReservationBillInfo.aspx?ReservationId=" + reservationBO.ReservationId;
                    //string s = "window.open('" + url + "', 'popup_window', 'width=790,height=780,left=300,top=50,resizable=yes');";
                    //ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, "Settlement Operation Failed.", AlertType.Warning);
                }
            }
        }
        //************************ User Defined Function ********************//
        private void SrcBillNumberProcess()
        {
            this.ClearCommonSessionInformation();
            this.ClearUnCommonSessionInformation();
            Session["GuestPaymentDetailListForGrid"] = null;
            if (!string.IsNullOrWhiteSpace(this.txtSrcBillNumber.Text))
            {
                LCInformationDA lcInformationDA = new LCInformationDA();
                LCInformationBO lcInformationBO = new LCInformationBO();
                List<LCInformationBO> lcInformationBOList = new List<LCInformationBO>();

                lcInformationBO = lcInformationDA.GetLCInformationByLCNumber(this.txtSrcBillNumber.Text);
                if (lcInformationBO != null)
                {
                    if (lcInformationBO.LCId > 0)
                    {
                        lcInformationBOList.Add(lcInformationBO);
                        this.ddlReservationId.DataSource = lcInformationBOList;
                        this.ddlReservationId.DataTextField = "LCNumber";
                        this.ddlReservationId.DataValueField = "LCId";
                        this.ddlReservationId.DataBind();

                        this.txtBanquetId.Text = lcInformationBO.LCNumber;
                        this.txtLCOpenDate.Text = hmUtility.GetStringFromDateTime(lcInformationBO.LCOpenDate);
                        //this.txtLCMatureDate.Text = hmUtility.GetStringFromDateTime(lcInformationBO.LCMatureDate);
                        if (lcInformationBO.LCMatureDate != null)
                        {
                            txtLCMatureDate.Text = hmUtility.GetStringFromDateTime(Convert.ToDateTime(lcInformationBO.LCMatureDate));
                        }
                        else
                        {
                            txtLCMatureDate.Text = string.Empty;
                        }
                        this.txtSupplierName.Text = lcInformationBO.Supplier;
                        LoadItemDetails(lcInformationBO.LCId);
                        LoadOverheadDetails(lcInformationBO.LCId);
                        LoadPaymentDetails(lcInformationBO.LCId);

                        decimal totalItemAmount = !string.IsNullOrWhiteSpace(this.hfTotalItemAmount.Value) ? Convert.ToDecimal(this.hfTotalItemAmount.Value) : 0;
                        decimal totalOverHeadExpenseAmount = !string.IsNullOrWhiteSpace(this.hfTotalOverHeadExpenseAmount.Value) ? Convert.ToDecimal(this.hfTotalOverHeadExpenseAmount.Value) : 0;

                        this.txtTotalLCCost.Text = (totalItemAmount + totalOverHeadExpenseAmount).ToString("#0.00");

                        //this.txtSrcRegistrationIdList.Value = this.ddlReservationId.SelectedValue;
                        //this.txtRemarks.Text = banquetReservationBO.Remarks;
                        //this.LoadRelatedInformation();
                        //hfCostcenterId.Value = banquetReservationBO.CostCenterId.ToString();
                        //hfReservationId.Value = banquetReservationBO.ReservationId.ToString();
                        //hfDiscountType.Value = banquetReservationBO.DiscountType;
                        //hfDiscountAmount.Value = banquetReservationBO.DiscountAmount.ToString();
                        //txtNumberOfPersonAdult.Text = banquetReservationBO.NumberOfPersonAdult.ToString();
                        //txtNumberOfPersonChild.Text = banquetReservationBO.NumberOfPersonChild.ToString();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + " Valid LC Number.", AlertType.Warning);
                    }
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + " Valid LC Number.", AlertType.Warning);
                }
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + " Valid LC Number.", AlertType.Warning);
            }
        }
        private void LoadItemDetails(long lcId)
        {
            LCInformationViewBO viewBo = new LCInformationViewBO();
            LCInformationDA lcDA = new LCInformationDA();

            gvItemDetail.DataSource = lcDA.GetLCDetailInformationById(lcId);
            gvItemDetail.DataBind();
            CalculateTotalItemAmount();
        }
        private void LoadOverheadDetails(long lcId)
        {
            int totalRecords = 0;
            OverHeadExpenseDA paidServiceDA = new OverHeadExpenseDA();
            List<OverHeadExpenseBO> paidServiceList = new List<OverHeadExpenseBO>();
            paidServiceList = paidServiceDA.GetOverHeadExpenseInfoBySearchCriteriaForPagination(lcId.ToString(), 100, 1, out totalRecords);

            gvOverheadDetail.DataSource = paidServiceList;
            gvOverheadDetail.DataBind();
            CalculateTotalOverHeadExpense();
        }
        private void LoadPaymentDetails(long lcId)
        {
            LCInformationViewBO viewBo = new LCInformationViewBO();
            LCInformationDA lcDA = new LCInformationDA();

            gvPaymentDetail.DataSource = lcDA.GetLCTotalPaymentById(lcId);
            gvPaymentDetail.DataBind();
            CalculateTotalPaymentAmount();
        }
        private void LoadLabelForSalesTotal()
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
            this.ddlSalesTotalLocal.DataSource = fields;
            this.ddlSalesTotalLocal.DataTextField = "FieldValue";
            this.ddlSalesTotalLocal.DataValueField = "FieldId";
            this.ddlSalesTotalLocal.DataBind();
            this.ddlSalesTotalLocal.SelectedIndex = 0;
            //this.lblSalesTotalLocal.Text = "Sales Amount (" + this.ddlSalesTotalLocal.SelectedItem.Text + ")";
            //this.lblGrandTotalLocal.Text = "Grand Total (" + this.ddlSalesTotalLocal.SelectedItem.Text + ")";

            this.lblSalesTotalLocal.Text = "Sales Amount";
            this.lblGrandTotalLocal.Text = "Grand Total";

            this.ddlSalesTotalUsd.DataSource = fields;
            this.ddlSalesTotalUsd.DataTextField = "FieldValue";
            this.ddlSalesTotalUsd.DataValueField = "FieldId";
            this.ddlSalesTotalUsd.DataBind();
            this.ddlSalesTotalUsd.SelectedIndex = 1;
            this.lblSalesTotalUsd.Text = "Sales Amount (" + this.ddlSalesTotalUsd.SelectedItem.Text + ")";
            this.lblGrandTotalUsd.Text = "Grand Total (" + this.ddlSalesTotalUsd.SelectedItem.Text + ")";
        }
        private void LoadBank()
        {
            BankDA bankDA = new BankDA();
            List<BankBO> entityBOList = new List<BankBO>();
            entityBOList = bankDA.GetBankInfo();

            this.ddlBankId.DataSource = entityBOList;
            this.ddlBankId.DataTextField = "BankName";
            this.ddlBankId.DataValueField = "BankId";
            this.ddlBankId.DataBind();

            this.ddlChequeBankId.DataSource = entityBOList;
            this.ddlChequeBankId.DataTextField = "BankName";
            this.ddlChequeBankId.DataValueField = "BankId";
            this.ddlChequeBankId.DataBind();

            this.ddlMBankingBankId.DataSource = entityBOList;
            this.ddlMBankingBankId.DataTextField = "BankName";
            this.ddlMBankingBankId.DataValueField = "BankId";
            this.ddlMBankingBankId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBankId.Items.Insert(0, itemBank);
            this.ddlChequeBankId.Items.Insert(0, itemBank);
            this.ddlMBankingBankId.Items.Insert(0, itemBank);
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
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void CheckObjectPermission()
        {
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            //ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            //objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmBanquetBillSettlement.ToString());

            //isSavePermission = objectPermissionBO.IsSavePermission;
            //isDeletePermission = objectPermissionBO.IsDeletePermission;
            ////btnSave.Visible = isSavePermission;
            ////btnAddDetailGuest.Visible = isSavePermission;
            ////btnAddMoreBill.Visible = isSavePermission;
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
        private void LoadLocalCurrencyId()
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetLocalCurrencyInfo();

            LocalCurrencyId = commonCurrencyBO.CurrencyId;
            hfLocalCurrencyId.Value = commonCurrencyBO.CurrencyId.ToString();
        }
        private void LoadReservationNumber(int roomId)
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomRegistrationBO> entityBOList = new List<RoomRegistrationBO>();
            entityBOList = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(roomId);
            //if (entityBOList.Count > 0)
            //{
            //    txtExpectedCheckOutDate.Text = hmUtility.GetStringFromDateTime(entityBOList[0].ExpectedCheckOutDate);
            //}
            //this.ddlReservationId.DataSource = entityBOList;
            //this.ddlReservationId.DataTextField = "RegistrationNumber";
            //this.ddlReservationId.DataValueField = "RegistrationId";
            //this.ddlReservationId.DataBind();

            this.txtSrcRegistrationIdList.Value = this.ddlReservationId.SelectedValue.ToString();

            if (!string.IsNullOrWhiteSpace(this.ddlReservationId.SelectedValue))
            {
                GuestCompanyBO guestCompanyInfo = new GuestCompanyBO();
                GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
                guestCompanyInfo = guestCompanyDA.GetGuestCompanyInfoByRegistrationId(Convert.ToInt32(this.ddlReservationId.SelectedValue));
                if (guestCompanyInfo != null)
                {
                    this.hfGuestCompanyInformation.Value = guestCompanyInfo.CompanyName;
                }
            }


        }
        private void LoadRoomNumber()
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

            this.ddlPaidByRegistrationId.DataSource = roomNumberDA.GetRoomNumberInfoByCondition(0, condition);
            this.ddlPaidByRegistrationId.DataTextField = "RoomNumber";
            this.ddlPaidByRegistrationId.DataValueField = "RoomId";
            this.ddlPaidByRegistrationId.DataBind();
            this.ddlPaidByRegistrationId.Items.Insert(0, itemRoom);
        }        
        private void LoadItemAndRequisitesGridView()
        {
            //if (ddlReservationId.SelectedIndex != -1)
            //{
            //    BanquetBillPaymentDA banquetDa = new BanquetBillPaymentDA();
            //    List<BanquetReservationBillGenerateReportBO> BanquetReservationBill = new List<BanquetReservationBillGenerateReportBO>();
            //    List<BanquetReservationBillGenerateReportBO> BanquetItemDetail = new List<BanquetReservationBillGenerateReportBO>();
            //    List<BanquetReservationBillGenerateReportBO> BanquetRequisitesDetail = new List<BanquetReservationBillGenerateReportBO>();

            //    BanquetReservationBill = banquetDa.GetBanquetReservationBillGenerateReport(Convert.ToInt32(ddlReservationId.SelectedValue));
            //    if (BanquetReservationBill != null)
            //    {
            //        BanquetItemDetail = BanquetReservationBill.Where(test => test.ItemType != "Requisites" & test.ItemType != "Payments").ToList();
            //        BanquetRequisitesDetail = BanquetReservationBill.Where(test => test.ItemType == "Requisites").ToList();

            //        this.gvItemDetail.DataSource = BanquetItemDetail;
            //        this.gvItemDetail.DataBind();

            //        this.gvRequisitesDetail.DataSource = BanquetRequisitesDetail;
            //        this.gvRequisitesDetail.DataBind();

            //        // Service Charge Total Calculation-----------
            //        decimal calculatedServiceChargeTotal = BanquetReservationBill[0].ServiceCharge;
            //        this.txtServiceChargeTotal.Text = calculatedServiceChargeTotal.ToString("#0.00");
            //        // Vat Total Calculation-----------
            //        decimal calculatedVatTotal = BanquetReservationBill[0].VatAmount;
            //        this.txtVatTotal.Text = calculatedVatTotal.ToString("#0.00");
            //        // Discount Total Calculation-----------
            //        decimal calculatedDiscountTotal = BanquetReservationBill[0].CalculatedDiscountAmount;
            //        this.txtDiscountAmountTotal.Text = calculatedDiscountTotal.ToString("#0.00");
            //        this.txtDiscountAmount.Text = "0.00";

            //        decimal calculatedGuestAdvanceAmount = !string.IsNullOrWhiteSpace(this.hfAdvancePaymentAmount.Value) ? Convert.ToDecimal(this.hfAdvancePaymentAmount.Value) : 0;

            //        ////// Sales Total Calculation-----------
            //        ////this.txtSalesTotal.Text = Math.Round(BanquetReservationBill[0].TotalAmount - calculatedGuestAdvanceAmount - calculatedDiscountTotal).ToString("#0.00");
            //        ////this.HiddenFieldSalesTotal.Value = Math.Round(BanquetReservationBill[0].TotalAmount - calculatedGuestAdvanceAmount - calculatedDiscountTotal).ToString("#0.00");

            //        ////this.txtGrandTotal.Text = Math.Round(BanquetReservationBill[0].TotalAmount - calculatedGuestAdvanceAmount - calculatedDiscountTotal).ToString("#0.00");
            //        ////this.HiddenFieldGrandTotal.Value = Math.Round(BanquetReservationBill[0].TotalAmount - calculatedGuestAdvanceAmount - calculatedDiscountTotal).ToString("#0.00");

            //        //// Sales Total Calculation-----------
            //        //this.txtSalesTotal.Text = Math.Round(BanquetReservationBill[0].GrandTotal - calculatedGuestAdvanceAmount - calculatedDiscountTotal).ToString("#0.00");
            //        //this.HiddenFieldSalesTotal.Value = Math.Round(BanquetReservationBill[0].GrandTotal - calculatedGuestAdvanceAmount - calculatedDiscountTotal).ToString("#0.00");

            //        //this.txtGrandTotal.Text = Math.Round(BanquetReservationBill[0].GrandTotal - calculatedGuestAdvanceAmount - calculatedDiscountTotal).ToString("#0.00");
            //        //this.HiddenFieldGrandTotal.Value = Math.Round(BanquetReservationBill[0].GrandTotal - calculatedGuestAdvanceAmount - calculatedDiscountTotal).ToString("#0.00");
            //        this.txtSalesTotal.Text = Math.Round(BanquetReservationBill[0].GrandTotal - calculatedGuestAdvanceAmount).ToString("#0.00");
            //        this.HiddenFieldSalesTotal.Value = Math.Round(BanquetReservationBill[0].GrandTotal - calculatedGuestAdvanceAmount).ToString("#0.00");

            //        this.txtGrandTotal.Text = Math.Round(BanquetReservationBill[0].GrandTotal - calculatedGuestAdvanceAmount).ToString("#0.00");
            //        this.HiddenFieldGrandTotal.Value = Math.Round(BanquetReservationBill[0].GrandTotal - calculatedGuestAdvanceAmount).ToString("#0.00");

            //        this.txtBanquetId.Text = BanquetReservationBill[0].BanquetName;
            //        this.txtLCOpenDate.Text = (BanquetReservationBill[0].BanquetRate).ToString("#0.00");
            //        this.txtLCMatureDate.Text = BanquetReservationBill[0].SeatingName;

            //        //--Remove Paid By RoomId from Room Number List----------------------------------------------
            //        if (this.ddlPayMode.SelectedIndex != -1)
            //        {
            //            this.ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("Company"));
            //            //this.ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("Cheque"));
            //        }

            //        if (BanquetReservationBill[0].CompanyId > 0)
            //        {
            //            GuestCompanyDA companyDa = new GuestCompanyDA();
            //            List<GuestCompanyBO> companyListBO = new List<GuestCompanyBO>();
            //            companyListBO = companyDa.GetGuestCompanyInfo().Where(x => x.CompanyId == BanquetReservationBill[0].CompanyId).ToList();
            //            this.ddlCompanyName.DataSource = companyListBO;
            //            this.ddlCompanyName.DataTextField = "CompanyName";
            //            this.ddlCompanyName.DataValueField = "NodeId";
            //            this.ddlCompanyName.DataBind();


            //            ListItem itemRoom = new ListItem();
            //            itemRoom.Value = "Company";
            //            itemRoom.Text = "Company";
            //            this.ddlPayMode.Items.Insert(4, itemRoom);

            //            //ListItem chequeItem = new ListItem("Cheque", "Cheque", true);
            //            //this.ddlPayMode.Items.Add(chequeItem);
            //        }
            //        this.pnlBillPrintPreview.Visible = true;
            //    }
            //    else
            //    {
            //        this.gvItemDetail.DataSource = null;
            //        this.gvItemDetail.DataBind();

            //        this.gvRequisitesDetail.DataSource = null;
            //        this.gvRequisitesDetail.DataBind();

            //        this.txtIndividualRoomVatAmount.Text = "0";
            //        this.txtIndividualRoomServiceCharge.Text = "0";
            //        this.txtIndividualRoomDiscountAmount.Text = "0";
            //        this.txtIndividualRoomGrandTotal.Text = "0";
            //    }
            //}
            //else
            //{
            //    this.gvItemDetail.DataSource = null;
            //    this.gvItemDetail.DataBind();

            //    this.gvRequisitesDetail.DataSource = null;
            //    this.gvRequisitesDetail.DataBind();

            //    this.txtIndividualRoomVatAmount.Text = "0";
            //    this.txtIndividualRoomServiceCharge.Text = "0";
            //    this.txtIndividualRoomDiscountAmount.Text = "0";
            //    this.txtIndividualRoomGrandTotal.Text = "0";
            //}


        }
        private void CalculateSalesTotal()
        {
            /*
            // Vat Total Calculation-----------
            decimal calculatedGuestRoomVatTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRoomVatAmount.Text) ? Convert.ToDecimal(this.txtIndividualRoomVatAmount.Text) : 0;
            decimal calculatedGuestServiceVatTotal = !string.IsNullOrWhiteSpace(this.txtIndividualServiceVatAmount.Text) ? Convert.ToDecimal(this.txtIndividualServiceVatAmount.Text) : 0;
            decimal calculatedRestaurantVatTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRestaurantVatAmount.Text) ? Convert.ToDecimal(this.txtIndividualRestaurantVatAmount.Text) : 0;
            decimal calculatedExtraRoomVatTotal = !string.IsNullOrWhiteSpace(this.txtIndividualExtraRoomVatAmount.Text) ? Convert.ToDecimal(this.txtIndividualExtraRoomVatAmount.Text) : 0;

            decimal calculatedVatTotal = calculatedGuestRoomVatTotal + calculatedGuestServiceVatTotal + calculatedRestaurantVatTotal + calculatedExtraRoomVatTotal;
            this.txtVatTotal.Text = calculatedVatTotal.ToString("#0.00");

            // Service Charge Total Calculation-----------
            decimal calculatedGuestRoomServiceChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRoomServiceCharge.Text) ? Convert.ToDecimal(this.txtIndividualRoomServiceCharge.Text) : 0;
            decimal calculatedGuestServiceServiceChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualServiceServiceCharge.Text) ? Convert.ToDecimal(this.txtIndividualServiceServiceCharge.Text) : 0;
            decimal calculatedRestaurantServiceChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRestaurantServiceCharge.Text) ? Convert.ToDecimal(this.txtIndividualRestaurantServiceCharge.Text) : 0;
            decimal calculatedExtraRoomServiceChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualExtraRoomServiceCharge.Text) ? Convert.ToDecimal(this.txtIndividualExtraRoomServiceCharge.Text) : 0;

            decimal calculatedServiceChargeTotal = calculatedGuestRoomServiceChargeTotal + calculatedGuestServiceServiceChargeTotal + calculatedRestaurantServiceChargeTotal + calculatedExtraRoomServiceChargeTotal;
            this.txtServiceChargeTotal.Text = calculatedServiceChargeTotal.ToString("#0.00");

            // Discount Total Calculation-----------
            decimal calculatedGuestRoomDiscountTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRoomDiscountAmount.Text) ? Convert.ToDecimal(this.txtIndividualRoomDiscountAmount.Text) : 0;
            decimal calculatedGuestServiceDiscountTotal = !string.IsNullOrWhiteSpace(this.txtIndividualServiceDiscountAmount.Text) ? Convert.ToDecimal(this.txtIndividualServiceDiscountAmount.Text) : 0;
            decimal calculatedRestaurantDiscountTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRestaurantDiscountAmount.Text) ? Convert.ToDecimal(this.txtIndividualRestaurantDiscountAmount.Text) : 0;
            decimal calculatedExtraRoomDiscountTotal = !string.IsNullOrWhiteSpace(this.txtIndividualExtraRoomDiscountAmount.Text) ? Convert.ToDecimal(this.txtIndividualExtraRoomDiscountAmount.Text) : 0;

            decimal calculatedDiscountTotal = calculatedGuestRoomDiscountTotal + calculatedGuestServiceDiscountTotal + calculatedRestaurantDiscountTotal + calculatedExtraRoomDiscountTotal;
            this.txtDiscountAmountTotal.Text = calculatedDiscountTotal.ToString("#0.00");

            // Sales Total Calculation-----------
            decimal calculatedGuestRoomTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRoomGrandTotal.Text) ? Convert.ToDecimal(this.txtIndividualRoomGrandTotal.Text) : 0;
            decimal calculatedGuestServiceTotal = !string.IsNullOrWhiteSpace(this.txtIndividualServiceGrandTotal.Text) ? Convert.ToDecimal(this.txtIndividualServiceGrandTotal.Text) : 0;
            decimal calculatedRestaurantTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRestaurantGrandTotal.Text) ? Convert.ToDecimal(this.txtIndividualRestaurantGrandTotal.Text) : 0;
            //decimal calculatedExtraRoomTotal = !string.IsNullOrWhiteSpace(this.txtIndividualExtraRoomGrandTotal.Text) ? Convert.ToDecimal(this.txtIndividualExtraRoomGrandTotal.Text) : 0;
            decimal calculatedExtraRoomTotal = 0;
            decimal calculatedAdvancePaymentAmountTotal = !string.IsNullOrWhiteSpace(this.txtAdvancePaymentAmount.Text) ? Convert.ToDecimal(this.txtAdvancePaymentAmount.Text) : 0;

            //decimal calculatedSalesTotal = (calculatedGuestRoomTotal + calculatedGuestServiceTotal + calculatedRestaurantTotal + calculatedExtraRoomTotal) - calculatedAdvancePaymentAmountTotal - calculatedDiscountTotal;
            decimal calculatedSalesTotal = (calculatedGuestRoomTotal + calculatedGuestServiceTotal + calculatedRestaurantTotal + calculatedExtraRoomTotal) - calculatedAdvancePaymentAmountTotal;
            this.txtSalesTotal.Text = Math.Round(calculatedSalesTotal).ToString("#0.00");
            this.HiddenFieldSalesTotal.Value = Math.Round(calculatedSalesTotal).ToString("#0.00");

            this.txtDiscountAmount.Text = "0.00";
            this.txtGrandTotal.Text = Math.Round(calculatedSalesTotal).ToString("#0.00");
            this.HiddenFieldGrandTotal.Value = Math.Round(calculatedSalesTotal).ToString("#0.00");


            //decimal conversionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 1;
            if (!string.IsNullOrWhiteSpace(this.ddlReservationId.SelectedValue))
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomRegistrationBO registrationBO = new RoomRegistrationBO();
                registrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Convert.ToInt32(this.ddlReservationId.SelectedValue));
                this.txtConversionRate.Text = registrationBO.ConversionRate.ToString();
                this.txtConversionRateHiddenField.Value = registrationBO.ConversionRate.ToString();
                this.btnLocalBillPreview.Text = "Bill Preview" + " (" + registrationBO.LocalCurrencyHead + ")";
                this.btnUSDBillPreview.Text = "Bill Preview (USD)";
            }
            decimal conversionRate = !string.IsNullOrWhiteSpace(this.txtConversionRateHiddenField.Value) ? Convert.ToDecimal(this.txtConversionRateHiddenField.Value) : 1;
            if (conversionRate > 0)
            {
                this.lblSalesTotalUsd.Visible = true;
                this.txtSalesTotalUsd.Visible = true;
                this.lblGrandTotalUsd.Visible = true;
                this.txtGrandTotalUsd.Visible = true;
                this.txtSalesTotalUsd.Text = (calculatedSalesTotal / conversionRate).ToString("#0.00"); //Math.Round(calculatedSalesTotal / conversionRate).ToString("#0.00");
                this.hfTxtSalesTotalUsd.Value = this.txtSalesTotalUsd.Text;
                this.txtGrandTotalUsd.Text = this.txtSalesTotalUsd.Text;
                this.hfGrandTotalUsd.Value = this.txtSalesTotalUsd.Text;
            }
            else
            {
                this.txtSalesTotalUsd.Text = "0.00";
                this.lblSalesTotalUsd.Visible = false;
                this.txtSalesTotalUsd.Visible = false;
                this.lblGrandTotalUsd.Visible = false;
                this.txtGrandTotalUsd.Visible = false;
            }
             */
        }
        private void CalculateTotalItemAmount()
        {
            decimal itemQty = 0, itemQtyTmp;
            decimal AmtTotalAmount = 0, AmtTotalAmountTmp;

            for (int i = 0; i < gvItemDetail.Rows.Count; i++)
            {
                itemQtyTmp = 0;
                AmtTotalAmountTmp = 0;

                if (decimal.TryParse(((Label)gvItemDetail.Rows[i].FindControl("lblItemUnit")).Text, out itemQtyTmp))
                    itemQty += itemQtyTmp;

                if (decimal.TryParse(((Label)gvItemDetail.Rows[i].FindControl("lblUnitPrice")).Text, out AmtTotalAmountTmp))
                    AmtTotalAmount += AmtTotalAmountTmp;
            }

            this.hfTotalItemAmount.Value = (itemQty * AmtTotalAmount).ToString();
            this.lblTotalItemAmount.Text = "Total Item Amount: " + (itemQty * AmtTotalAmount).ToString("#0.00");
        }
        private void CalculateTotalOverHeadExpense()
        {
            decimal AmtTotalAmount = 0, AmtTotalAmountTmp;

            for (int i = 0; i < gvOverheadDetail.Rows.Count; i++)
            {
                AmtTotalAmountTmp = 0;

                if (decimal.TryParse(((Label)gvOverheadDetail.Rows[i].FindControl("lblExpenseAmount")).Text, out AmtTotalAmountTmp))
                    AmtTotalAmount += AmtTotalAmountTmp;
            }

            this.hfTotalOverHeadExpenseAmount.Value = (AmtTotalAmount).ToString();
            this.lblTotalOverHeadExpenseAmount.Text = "Total Overhead Expense: " + AmtTotalAmount.ToString("#0.00");
        }
        private void CalculateTotalPaymentAmount()
        {
            decimal AmtTotalAmount = 0, AmtTotalAmountTmp;

            for (int i = 0; i < gvPaymentDetail.Rows.Count; i++)
            {
                AmtTotalAmountTmp = 0;

                if (decimal.TryParse(((Label)gvPaymentDetail.Rows[i].FindControl("lblAmount")).Text, out AmtTotalAmountTmp))
                    AmtTotalAmount += AmtTotalAmountTmp;
            }

            this.lblTotalPaymentAmount.Text = "Total Payment Amount: " + AmtTotalAmount.ToString("#0.00");
        }
        private void Cancel()
        {
            //this.ddlReservationId.SelectedIndex = 0;
            txtSrcRegistrationIdList.Value = string.Empty;
            //GuestPaymentDetailsInformationDiv.Visible = true;
            hfGuestPaymentDetailsInformationDiv.Value = "1";
            //this.LoadCurrentDate();
            this.ddlPayMode.SelectedIndex = 0;
            this.ddlChequeReceiveAccountsInfo.SelectedIndex = 0;
            this.txtChecqueNumber.Text = string.Empty;
            this.ddlCardPaymentAccountHeadId.SelectedIndex = 0;
            this.ddlCardType.SelectedIndex = 0;
            this.txtCardNumber.Text = string.Empty;
            this.txtCardHolderName.Text = string.Empty;
            this.txtExpireDate.Text = string.Empty;
            this.ddlBankId.SelectedValue = "0";
            this.btnSave.Text = "Save";
            this.ddlReservationId.Focus();
        }
        private bool IsFrmValid()
        {
            bool flag = true;

            //decimal grandTotal = !string.IsNullOrWhiteSpace(HiddenFieldGrandTotal.Value) ? Convert.ToDecimal(HiddenFieldGrandTotal.Value) : 0;
            //decimal totalPaid = !string.IsNullOrWhiteSpace(HiddenFieldTotalPaid.Value) ? Convert.ToDecimal(HiddenFieldTotalPaid.Value) : 0;
            //if (grandTotal != totalPaid)
            //{
            //    //this.isMessageBoxEnable = 1;
            //    //this.lblMessage.Text = "Grand Total and Guest Payment Amount is not Equal..";
            //    CommonHelper.AlertInfo(innboardMessage, "Grand Total and Guest Payment Amount is not Equal..", AlertType.Warning);
            //    this.ddlPayMode.Focus();
            //    flag = false;
            //    return flag;
            //}


            if (string.IsNullOrWhiteSpace(this.txtSrcBillNumber.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a valid LC Number.", AlertType.Warning);
                this.txtSrcBillNumber.Focus();
                flag = false;
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "", AlertType.Warning);
            }
            return flag;
        }
        private void LoadRelatedInformation()
        {
            if (ddlReservationId.SelectedIndex != -1)
            {
                this.LoadPaymentInformation();
                this.LoadItemAndRequisitesGridView();
            }
        }
        private void LoadPaymentInformation()
        {
            if (ddlReservationId.SelectedIndex != -1)
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                string registrationId = this.ddlReservationId.SelectedValue.ToString();

                PaymentSummaryBO paymentSummaryBO = new PaymentSummaryBO();
                BanquetBillPaymentDA paymentSummaryDA = new BanquetBillPaymentDA();
                paymentSummaryBO = paymentSummaryDA.GetGuestBillPaymentSummaryInfoByBanquetReservationId(registrationId, 0);
                this.txtAdvancePaymentAmount.Text = Math.Round(paymentSummaryBO.TotalPayment).ToString();
                this.hfAdvancePaymentAmount.Value = Math.Round(paymentSummaryBO.TotalPayment).ToString();

            }
            else
            {
                this.txtAdvancePaymentAmount.Text = "0";
                this.hfAdvancePaymentAmount.Value = "0";
            }
        }
        private void LoadAccountHeadInfo()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            this.lblPaymentAccountHead.Text = "Payment Receive In";

            List<CommonPaymentModeBO> commonPaymentModeBOList = new List<CommonPaymentModeBO>();
            commonPaymentModeBOList = hmCommonDA.GetCommonPaymentModeInfo("All");

            CommonPaymentModeBO cashPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Cash").FirstOrDefault();
            CommonPaymentModeBO cardPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Card").FirstOrDefault();
            CommonPaymentModeBO chequePaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Cheque").FirstOrDefault();
            CommonPaymentModeBO companyPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Company").FirstOrDefault();
            CommonPaymentModeBO mBankingPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "M-Banking").FirstOrDefault();
            CommonPaymentModeBO refundPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Refund").FirstOrDefault();

            this.ddlCashReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cashPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlCashReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCashReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCashReceiveAccountsInfo.DataBind();

            this.ddlCardReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cardPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlCardReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCardReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCardReceiveAccountsInfo.DataBind();

            this.ddlCardPaymentAccountHeadId.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cardPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            this.ddlCardPaymentAccountHeadId.DataTextField = "NodeHead";
            this.ddlCardPaymentAccountHeadId.DataValueField = "NodeId";
            this.ddlCardPaymentAccountHeadId.DataBind();

            this.ddlChequeReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + chequePaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlChequeReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlChequeReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlChequeReceiveAccountsInfo.DataBind();
            
            this.ddlMBankingReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + mBankingPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlMBankingReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlMBankingReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlMBankingReceiveAccountsInfo.DataBind();

            this.ddlRefundAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + refundPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            this.ddlRefundAccountHead.DataTextField = "NodeHead";
            this.ddlRefundAccountHead.DataValueField = "NodeId";
            this.ddlRefundAccountHead.DataBind();
        }
        public static string LoadGuestPaymentDetailGridViewByWM(string paymentDescription)
        {
            string strTable = "";
            List<GuestBillPaymentBO> detailList = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            if (detailList != null)
            {
                strTable += "<table id='ReservationDetailGrid' class='table table-bordered table-condensed table-responsive' style='width:100%'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Payment Mode</th><th align='left' scope='col'>Description</th><th align='left' scope='col'>Amount</th><th align='center' scope='col'>Action</th></tr>";
                int counter = 0;
                foreach (GuestBillPaymentBO dr in detailList)
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
                    strTable += "<td align='left' style='width: 40%;'>" + dr.PaymentDescription + "</td>";
                    strTable += "<td align='left' style='width: 20%;'>" + dr.PaymentAmount + "</td>";
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
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        private void ClearCommonSessionInformation()
        {
            this.Session["TransactionDetailList"] = null;
            this.Session["GuestPaymentDetailListForGrid"] = null;
            this.Session["CheckOutPayMode"] = null;
            this.Session["CurrentRegistrationId"] = null;
            this.Session["IsCheckOutBillPreview"] = null;
            this.Session["GuestBillRoomIdParameterValue"] = null;
            this.Session["GuestBillServiceIdParameterValue"] = null;
            this.Session["GuestPaymentDetailListForGrid"] = null;
            this.Session["CompanyPaymentRoomIdList"] = null;
            this.Session["CompanyPaymentServiceIdList"] = null;
        }
        private void ClearUnCommonSessionInformation()
        {
            this.Session["txtStartDate"] = null;
            this.Session["IsBillSplited"] = null;
            this.Session["GuestBillFromDate"] = null;
            this.Session["GuestBillToDate"] = null;
            this.Session["AddedExtraRoomInformation"] = null;
            this.Session["CheckOutRegistrationIdList"] = null;
        }
        private void Clear()
        {
            txtVatTotal.Text = string.Empty;
            txtServiceChargeTotal.Text = string.Empty;
            txtDiscountAmountTotal.Text = string.Empty;
            txtAdvancePaymentAmount.Text = string.Empty;
            txtSalesTotal.Text = string.Empty;
            txtIndividualRoomVatAmount.Text = string.Empty;
            txtIndividualRoomServiceCharge.Text = string.Empty;
            txtIndividualRoomDiscountAmount.Text = string.Empty;
            txtIndividualRoomGrandTotal.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            txtRebateRemarks.Text = string.Empty;
            ddlReservationId.SelectedIndex = -1;
            this.pnlBillPrintPreview.Visible = false;

            gvItemDetail.DataSource = null;
            gvItemDetail.DataBind();

            gvOverheadDetail.DataSource = null;
            gvOverheadDetail.DataBind();

            gvPaymentDetail.DataSource = null;
            gvPaymentDetail.DataBind();

        }
        //************************ User Defined WebMethod ********************//       
        [WebMethod]
        public static string GetTotalBillAmountByWebMethod(string ddlReservationId, string SelectdRoomId, string SelectdServiceId, string StartDate, string EndDate)
        {
            GuestBillSplitDA entityDA = new GuestBillSplitDA();
            GuestServiceBillApprovedBO entityBO = new GuestServiceBillApprovedBO();

            HMUtility hmUtility = new HMUtility();
            string startDate = hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            string endDate = hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            entityBO = entityDA.GetGuestServiceTotalAmountInfo(ddlReservationId, SelectdRoomId, SelectdServiceId, startDate, endDate);
            return Math.Round(entityBO.ServiceTotalAmount).ToString("#0.00"); // entityBO.ServiceTotalAmount.ToString();
        }
        [WebMethod(EnableSession = true)]
        public static string PerformSaveGuestPaymentDetailsInformationByWebMethod(bool isEdit, string paymentDescription, string ddlCurrency, string currencyType, string localCurrencyId, string conversionRate, string ddlPayMode, string ddlBankId, string txtReceiveLeadgerAmount, string ddlReservationId, string ddlCashPaymentAccountHead, string txtCardNumber, string ddlCardType, string txtExpireDate, string txtCardHolderName, string txtChecqueNumber, string ddlChecquePaymentAccountHeadId, string ddlCardPaymentAccountHeadId, string ddlCompanyPaymentAccountHead, string ddlMBankingBankId, string ddlMBankingReceiveAccountsInfo, string ddlPaidByRoomId, string RefundAccountHead)
        {
            HMUtility hmUtility = new HMUtility();
            int dynamicDetailId = 0;
            int ddlPaidByRegistrationId = 0;

            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            CustomFieldBO banquetRoomSalesAccountHeadInfoBO = new CustomFieldBO();
            banquetRoomSalesAccountHeadInfoBO = hmCommonDA.GetCustomFieldByFieldName("BanquetRoomSalesAccountHeadInfo");
            int banquetRoomSalesAccountHeadInfo = !string.IsNullOrWhiteSpace(banquetRoomSalesAccountHeadInfoBO.FieldValue) ? Convert.ToInt32(banquetRoomSalesAccountHeadInfoBO.FieldValue) : 0;

            List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

            GuestBillPaymentBO singleEntityBOEdit = guestPaymentDetailListForGrid.Where(x => x.PaymentMode == ddlPayMode).FirstOrDefault();

            if (guestPaymentDetailListForGrid != null)
            {
                dynamicDetailId = guestPaymentDetailListForGrid.Count + 1;
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
                        ddlPaidByRegistrationId = Convert.ToInt32(ddlReservationId);
                    }
                }
                else
                {
                    ddlPaidByRegistrationId = Convert.ToInt32(ddlReservationId);
                }
            }
            else
            {
                ddlPaidByRegistrationId = Convert.ToInt32(ddlReservationId);
            }

            if (ddlPayMode == "Company")
            {
                guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCompanyPaymentAccountHead);
                guestBillPaymentBO.PaymentType = ddlPayMode;
                guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCompanyPaymentAccountHead);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlReservationId);
                guestBillPaymentBO.RegistrationId = 0;
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlReservationId);
            }
            //else if (ddlPayMode == "Employee")
            //{
            //    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlEmployeePaymentAccountHead);
            //    guestBillPaymentBO.PaymentType = ddlPayMode;
            //    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlEmployeePaymentAccountHead);
            //    guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlPaidByRegistrationId);
            //    guestBillPaymentBO.BankId = Convert.ToInt32(ddlEmpId);
            //    guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlEmpId);
            //}
            else if (ddlPayMode == "Other Room")
            {
                guestBillPaymentBO.NodeId = banquetRoomSalesAccountHeadInfo;
                guestBillPaymentBO.PaymentType = ddlPayMode;
                guestBillPaymentBO.AccountsPostingHeadId = banquetRoomSalesAccountHeadInfo;
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlPaidByRegistrationId);
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlPaidByRegistrationId);
                guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlPaidByRegistrationId);
            }
            else if (ddlPayMode == "Refund")
            {
                ddlCurrency = localCurrencyId;
                conversionRate = "1";
                guestBillPaymentBO.RefundAccountHead = Int32.Parse(RefundAccountHead);
                guestBillPaymentBO.PaymentMode = "Refund";
                guestBillPaymentBO.CurrencyAmount = guestBillPaymentBO.CurrencyAmount * 1;
                guestBillPaymentBO.PaymentAmount = guestBillPaymentBO.PaymentAmount * 1;
                guestBillPaymentBO.PaymentType = "Refund";
                guestBillPaymentBO.AccountsPostingHeadId = Int32.Parse(RefundAccountHead);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlReservationId);
                guestBillPaymentBO.RegistrationId = 0;
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlReservationId);
            }
            else
            {
                if (ddlPayMode == "Cash")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCashPaymentAccountHead);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPaymentAccountHead);
                    guestBillPaymentBO.RegistrationId = 0;
                }
                else if (ddlPayMode == "Card")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCardPaymentAccountHeadId);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCardPaymentAccountHeadId);
                    guestBillPaymentBO.RegistrationId = 0;
                }
                else if (ddlPayMode == "Cheque")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlChecquePaymentAccountHeadId);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlChecquePaymentAccountHeadId);
                    ddlCardType = string.Empty;
                    guestBillPaymentBO.RegistrationId = 0;
                }
                if (ddlPayMode == "M-Banking")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlMBankingReceiveAccountsInfo);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlMBankingReceiveAccountsInfo);
                }
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlBankId);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlReservationId);
                guestBillPaymentBO.PaymentType = "Advance";
            }

            guestBillPaymentBO.PaymentDescription = paymentDescription;
            guestBillPaymentBO.BankId = Convert.ToInt32(ddlBankId);
            if (ddlPayMode == "M-Banking")
            {
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlMBankingBankId);
            }

            if (currencyType == "Local")
            {
                guestBillPaymentBO.IsUSDTransaction = false;
                guestBillPaymentBO.FieldId = Int32.Parse(ddlCurrency);
                guestBillPaymentBO.ConvertionRate = 1;
                guestBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
                guestBillPaymentBO.PaymentAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            }
            else
            {
                guestBillPaymentBO.IsUSDTransaction = true;
                guestBillPaymentBO.FieldId = Int32.Parse(ddlCurrency);
                guestBillPaymentBO.ConvertionRate = !string.IsNullOrWhiteSpace(conversionRate) ? Convert.ToDecimal(conversionRate) : 1;
                guestBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
                guestBillPaymentBO.PaymentAmount = guestBillPaymentBO.CurrencyAmount * guestBillPaymentBO.ConvertionRate;
            }

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
            return LoadGuestPaymentDetailGridViewByWM(paymentDescription);
        }
        [WebMethod(EnableSession = true)]
        public static string PerformDeleteGuestPaymentByWebMethod(int paymentId)
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

            return LoadGuestPaymentDetailGridViewByWM("");
        }
        [WebMethod(EnableSession = true)]
        public static string PerformGetTotalPaidAmountByWebMethod()
        {

            var List = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            decimal sum = 0;
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].PaymentMode == "Refund")
                {
                    sum = sum - Convert.ToDecimal(List[i].PaymentAmount);
                }
                else
                {
                    sum = sum + Convert.ToDecimal(List[i].PaymentAmount);
                }
            }
            return Math.Round(sum).ToString();
        }
        [WebMethod(EnableSession = true)]
        public static string PerformCompanyPayBill(string serviceType, string SelectdServiceApprovedId, string SelectdRoomApprovedId, string SelectdServiceId, string SelectdRoomId, string SelectdPaymentId, string StartDate, string EndDate, string ddlReservationId, string txtSrcRegistrationIdList)
        {
            HttpContext.Current.Session["CompanyPaymentRoomIdList"] = null;
            HttpContext.Current.Session["CompanyPaymentServiceIdList"] = null;
            HMUtility hmUtility = new HMUtility();
            //Room Information----------------
            List<GuestServiceBillApprovedBO> entityRoomBOList = new List<GuestServiceBillApprovedBO>();

            int totalRoomIdOut = SelectdRoomId.Split(',').Length - 1;
            for (int i = 0; i < totalRoomIdOut; i++)
            {
                GuestServiceBillApprovedBO entityRoomBO = new GuestServiceBillApprovedBO();
                entityRoomBO.RegistrationId = Convert.ToInt32(ddlReservationId);
                entityRoomBO.ServiceId = Convert.ToInt32(SelectdRoomId.Split(',')[i]);
                entityRoomBO.ArriveDate = hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                entityRoomBO.CheckOutDate = hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                entityRoomBOList.Add(entityRoomBO);
            }
            HttpContext.Current.Session["CompanyPaymentRoomIdList"] = entityRoomBOList;

            //Service Information----------------
            List<GuestServiceBillApprovedBO> entityServiceBOList = new List<GuestServiceBillApprovedBO>();

            int totalServiceIdOut = SelectdServiceId.Split(',').Length - 1;
            for (int i = 0; i < totalServiceIdOut; i++)
            {
                GuestServiceBillApprovedBO entityServiceBO = new GuestServiceBillApprovedBO();
                entityServiceBO.RegistrationId = Convert.ToInt32(ddlReservationId);
                entityServiceBO.ServiceId = Convert.ToInt32(SelectdServiceId.Split(',')[i]);
                entityServiceBO.ArriveDate = hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                entityServiceBO.CheckOutDate = hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                entityServiceBOList.Add(entityServiceBO);
            }

            HttpContext.Current.Session["CompanyPaymentServiceIdList"] = entityServiceBOList;

            decimal calculatedAmount = 0;
            if (!string.IsNullOrWhiteSpace(SelectdPaymentId))
            {
                //Payment Information----------------
                //List<GuestServiceBillApprovedBO> entityPaymentBOList = new List<GuestServiceBillApprovedBO>();
                List<GuestBillPaymentBO> guestPaymentBOList = new List<GuestBillPaymentBO>();
                GuestBillPaymentDA guestBillPaymentDA = new GuestBillPaymentDA();

                int paymentId = SelectdPaymentId.Split(',').Length;
                for (int i = 0; i < paymentId; i++)
                {
                    calculatedAmount = calculatedAmount + guestBillPaymentDA.GetGuestBillPaymentSummaryInfoByPaymentType(txtSrcRegistrationIdList, Convert.ToInt32(SelectdPaymentId.Split(',')[i]));
                }
            }

            //HttpContext.Current.Session["CompanyPaymentServiceIdList"] = entityServiceBOList;
            return calculatedAmount.ToString();
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
        public static List<InvCategoryBO> LoadCategory(int costCenterId)
        {
            List<InvCategoryBO> categoryList = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            categoryList = da.GetCostCenterWiseInvItemCatagoryInfo("Product", costCenterId);

            List<InvCategoryBO> requisitesList = new List<InvCategoryBO>();
            InvCategoryBO requisitesBO = new InvCategoryBO();
            requisitesBO.CategoryId = 100000;
            requisitesBO.Name = "Requisites";
            requisitesBO.MatrixInfo = "Requisites";
            requisitesList.Add(requisitesBO);

            List<InvCategoryBO> List = new List<InvCategoryBO>();
            List.AddRange(requisitesList);
            List.AddRange(categoryList);            

            return List;
        }
        [WebMethod]
        public static List<InvItemBO> GetInvItemByCategoryNCostCenter(int costCenterId, int CategoryId)
        {
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetDynamicallyItemInformationByCategoryId(costCenterId, CategoryId);

            return productList;
        }
        [WebMethod]
        public static InvItemViewBO GetProductDataByCriteria(int categoryId, int costCenterId, string ddlItemId)
        {
            InvItemViewBO viewBO = new InvItemViewBO();
            InvItemDA itemDA = new InvItemDA();
            var obj = itemDA.GetInvItemPriceForBanquet(categoryId, costCenterId, Int32.Parse(ddlItemId));
            viewBO.UnitPriceLocal = obj.UnitPrice;
            viewBO.ItemId = obj.ItemId;

            return viewBO;
        }
    }
}