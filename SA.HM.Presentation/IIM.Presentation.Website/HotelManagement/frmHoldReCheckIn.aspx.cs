using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data;
using System.Collections;
using System.Globalization;
using HotelManagement.Entity;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using System.Web.Services;
using System.IO;
using Newtonsoft.Json;

using HotelManagement.Entity.SalesManagment;
using System.Text.RegularExpressions;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmHoldReCheckIn : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        ArrayList arrayDelete;
        protected int isMessageBoxEnable = -1;
        protected int isRegistrationEddited = -1;
        protected int isReservationEddited = -1;
        protected int isAccountsPostingPanelEnable = -1;
        protected int aireportPickupInformationPanelEnable = -1;
        protected int isIntegratedGeneralLedgerDiv = 1;
        protected int IsRoomAvailableForRegistrationEnable = -1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        private Boolean isConversionRateEditable = false;
        HMUtility hmUtility = new HMUtility();
        HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
        protected int RegistrationId = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            hfIsPaidServiceAlreadyLoded.Value = "0";
            this.AddEditODeleteDetail();
            string cardValidation = System.Web.Configuration.WebConfigurationManager.AppSettings["CardValidation"].ToString();
            //txtCardValidation.Value = cardValidation.ToString();
            Session["PMSalesDetailList"] = null;
            string ddl = ddlRoomId.SelectedValue;
            QSReservationId.Value = string.Empty;
            if (!IsPostBack)
            {
                string hfPageNumber = string.Empty;
                string hfGridRecordCounts = string.Empty;
                string hfIsCurrentRPreviouss = string.Empty;
                LoadIsRoomOverbookingEnable();
                if (Request.QueryString["pn"] != null)
                {
                    hfPageNumber = Request.QueryString["pn"];
                    hfGridRecordCounts = Request.QueryString["grc"];
                    hfIsCurrentRPreviouss = Request.QueryString["icp"];

                    //LoadGridView(Convert.ToInt32(hfPageNumber), Convert.ToInt32(hfIsCurrentRPreviouss), Convert.ToInt32(hfGridRecordCounts));
                }

                //Session["_RoomRegistrationId"] = null;
                this.LoadRackRateServiceChargeVatPanelInformation();
                this.LoadTotalRoomTariffLabelChange();

                HMCommonSetupBO commonCountrySetupBO = new HMCommonSetupBO();
                commonCountrySetupBO = commonSetupDA.GetCommonConfigurationInfo("CompanyCountryId", "CompanyCountryId");

                if (commonCountrySetupBO != null)
                {
                    hfDefaultCountryId.Value = commonCountrySetupBO.SetupValue;
                }

                if (Session["MessegePanelEnableForSelectedRoomNumber"] != null)
                {
                    if (Session["MessegePanelEnableForSelectedRoomNumber"].ToString() == "True")
                    {
                        if (Session["PopUpPanelEnableForSelectedRoomNumber"] != null)
                        {
                            string url = Session["PopUpPanelEnableForSelectedRoomNumber"].ToString();
                            string sPopUp = "window.open('" + url + "', 'popup_window', 'width=715,height=780,left=300,top=50,resizable=yes, scrollbars=1');";
                            ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
                        }
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    }

                    //Session["MessegePanelEnableForSelectedRoomNumber"] = null;
                    Session["PopUpPanelEnableForSelectedRoomNumber"] = null;
                }
                //this.LoadBank();
                this.LoadProfession();
                //this.LoadCurrency();
                this.LoadRRPaymentMode();
                this.LoadReservationGuest();
                this.LoadCurrentDate();
                this.LoadComplementaryItem();
                this.LoadProbableDepartureTime();
                //LoadIsConversionRateEditable();
                //if (Session["_RoomRegistrationId"] == null)
                //this.SetDefaultComplementaryItem();

                this.RoomRegistrationNumberGenerationForGuest();
                this.DeleteTempGuestRegistration();
                this.ClearCommonSessionInformation();
                this.LoadCommonDropDownHiddenField();
                this.LoadGuestReference();
                this.LoadRoomType();
                this.LoadRoomNumber(Convert.ToInt32(this.ddlRoomType.SelectedValue));
                string queryStringId = Request.QueryString["SelectedRoomNumber"];
                string Source = Request.QueryString["source"];
                int selectedRoom = 0;
                RoomReservationDA reservationDA = new RoomReservationDA();
                RoomReservationBO reservationBO = new RoomReservationBO();

                if (!string.IsNullOrEmpty(queryStringId))
                {
                    if (!string.IsNullOrEmpty(Source) && Source == "Reservation")
                    {
                        selectedRoom = Int32.Parse(queryStringId);
                        reservationBO = reservationDA.GetRoomReservationInfoByRoomId(selectedRoom, DateTime.Now);
                        QSReservationId.Value = reservationBO.ReservationId.ToString();
                        ddlRoomIdHiddenField.Value = queryStringId;
                    }
                    else
                    {
                        QSReservationId.Value = string.Empty;
                        ddlRoomIdHiddenField.Value = string.Empty;
                        this.ddlRoomId.SelectedValue = queryStringId;
                    }
                }

                if (this.ddlRoomId.SelectedIndex != -1)
                {
                    this.CommonValueSet(Convert.ToInt32(this.ddlRoomId.SelectedValue), 0);
                }

                //this.LoadAffiliatedCompany();
                //this.LoadRoomReservation();
                //this.LoadBusinessPromotion();
                //this.ddlReservationId.Enabled = false;
                //this.LoadAccountHeadInfo();
                this.LoadCountryList();
                //this.LoadSearchCountryList();
                this.LoadGuestSource();
                LoadVIPGuestType();
                //if (chkIsFromReservation.Checked)
                //{
                //    this.ddlReservationId.Enabled = true;
                //}
                //else
                //{
                //    this.ddlReservationId.Enabled = false;
                //}

                Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                if (!isIntegrated)
                {
                    isIntegratedGeneralLedgerDiv = -1;
                }

                //string TransectionId = Request.QueryString["TransectionId"];
                //if (!string.IsNullOrEmpty(TransectionId))
                //{
                //    this.FillForm(Int32.Parse(TransectionId));
                //}

                //string registrationId = Request.QueryString["RegId"];
                //if (!string.IsNullOrEmpty(registrationId))
                //{
                //    int Id = Convert.ToInt32(registrationId);
                //    if (Id > 0)
                //    {
                //        this.FillForm(Id);
                //        this.SetTab("EntryTab");
                //    }
                //}

                RoomNumberBO numberBO = new RoomNumberBO();
                RoomNumberDA numberDA = new RoomNumberDA();
                string RoomTypeId = Request.QueryString["RoomTypeId"];
                string SelectedRoomTypeId = "";
                if (!string.IsNullOrEmpty(Request.QueryString["SelectedRoomNumber"]))
                {
                    numberBO = numberDA.GetRoomNumberInfoById(Int32.Parse(Request.QueryString["SelectedRoomNumber"]));
                    SelectedRoomTypeId = numberBO.RoomTypeId.ToString();
                }

                string ReservationId = Request.QueryString["ReservationId"];
                if (!string.IsNullOrEmpty(RoomTypeId))
                {
                    //chkIsFromReservation.Checked = true;
                    //ddlReservationId.SelectedValue = ReservationId;
                    //ddlReservationId.Enabled = true;
                    hiddendReservationId.Value = ReservationId;
                    this.ddlRoomType.SelectedValue = RoomTypeId;
                    this.ddlEntitleRoomType.SelectedValue = RoomTypeId;
                    this.LoadRoomNumber(Convert.ToInt32(this.ddlRoomType.SelectedValue));
                }

                if (!string.IsNullOrEmpty(SelectedRoomTypeId))
                {
                    hiddendReservationId.Value = ReservationId;
                    this.ddlRoomType.SelectedValue = SelectedRoomTypeId;
                    this.ddlEntitleRoomType.SelectedValue = SelectedRoomTypeId;
                    this.LoadRoomNumber(Convert.ToInt32(this.ddlRoomType.SelectedValue));

                    if (!string.IsNullOrEmpty(Request.QueryString["SelectedRoomNumber"]))
                    {
                        this.ddlRoomId.SelectedValue = Request.QueryString["SelectedRoomNumber"];
                    }

                    int roomId = Int32.Parse(this.ddlRoomId.SelectedValue);
                    if (roomId > 0)
                    {
                        RoomNumberBO roomNumberBO = new RoomNumberBO();
                        RoomNumberDA roomNumberDA = new RoomNumberDA();

                        roomNumberBO = roomNumberDA.GetRoomNumberInfoById(roomId);

                        RoomTypeBO roomTypeBO = new RoomTypeBO();
                        RoomTypeDA roomTypeDA = new RoomTypeDA();

                        roomTypeBO = roomTypeDA.GetRoomTypeInfoById(roomNumberBO.RoomTypeId);
                        this.txtViewRoomType.Text = roomTypeBO.RoomType.ToString();
                        this.txtUnitPrice.Text = roomTypeBO.RoomRate.ToString();

                        if (txtEntiteledRoomType.Value == "")
                        {

                            this.txtRoomRate.Text = roomTypeBO.RoomRate.ToString();
                            this.txtUnitPriceHiddenField.Value = roomTypeBO.RoomRate.ToString();
                        }
                    }
                }

                //string statusRoomId = Request.QueryString["RoomId"];
                //if (!string.IsNullOrEmpty(statusRoomId))
                //{
                //    RoomNumberBO roomNumberBO = numberDA.GetRoomNumberInfoById(Int32.Parse(statusRoomId));
                //    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                //    RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                //    roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumberBO.RoomNumber);
                //    if (roomAllocationBO.RoomId > 0)
                //    {
                //        Session["_RoomRegistrationId"] = roomAllocationBO.RegistrationId.ToString();
                //        this.FillForm(roomAllocationBO.RegistrationId);
                //    }
                //}

                LoadCommonSetupForRackRateServiceChargeVatInformation();

                //string registrationId = Request.QueryString["RegId"];
                //if (!string.IsNullOrEmpty(registrationId))
                //{
                //    RegistrationId = Convert.ToInt32(registrationId);
                //}  
                string registrationId = Request.QueryString["RegId"];
                if (!string.IsNullOrEmpty(registrationId))
                {
                    hfHoldRegistrationId.Value = registrationId;
                    FillForm(Convert.ToInt32(registrationId));
                }
            }
            //else
            //{
            //    Session["_RoomRegistrationId"] = null;
            //}
            string jscript = "function UploadComplete(){};";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            flashUpload.QueryParameters = "guestId=" + Server.UrlEncode(Convert.ToString(RandomOwnerId.Value));

            if (!IsPostBack)
            {
                if (!IsRoomAvailableForRegistration("PageLoad"))
                {
                    return;
                }
                if (Session["MessegePanelEnableForSelectedRoomNumber"] != null)
                {
                    Session["MessegePanelEnableForSelectedRoomNumber"] = null;
                }
            }            
        }
        private void RoomRegistrationNumberGenerationForGuest()
        {
            int registrationId = 0;

            Random rd = new Random();
            registrationId = rd.Next(100000, 999999);
            Session["_RoomRegistrationId"] = registrationId;
            RandomOwnerId.Value = registrationId.ToString();
            tempRegId.Value = registrationId.ToString();
        }
        protected void btnAddDetailGuest_Click(object sender, EventArgs e)
        {
            this.CommonValueSet(Convert.ToInt32(this.ddlRoomId.SelectedValue), 0);
            if (!isValidDetailForm())
            {
                return;
            }
            List<GuestInformationBO> registrationDetailListBO = Session["RegistrationDetailList"] == null ? new List<GuestInformationBO>() : Session["RegistrationDetailList"] as List<GuestInformationBO>;
            List<GuestDocumentsBO> documentListBO = Session["DocumentList"] == null ? new List<GuestDocumentsBO>() : Session["DocumentList"] as List<GuestDocumentsBO>;
            GuestInformationBO detailBO = new GuestInformationBO();
            GuestDocumentsBO documentBO = new GuestDocumentsBO();

            detailBO.GuestAddress1 = this.txtGuestAddress1.Text;//
            detailBO.GuestAddress2 = this.txtGuestAddress2.Text;//
            detailBO.GuestAuthentication = ""; // this.ddlGuestAuthentication.Text;
            detailBO.ProfessionId = Convert.ToInt32(ddlProfessionId.SelectedValue);
            detailBO.GuestCity = this.txtGuestCity.Text;//
            detailBO.GuestCountryId = Int32.Parse(ddlGuestCountry.SelectedValue);//

            if (!string.IsNullOrWhiteSpace(this.txtGuestDOB.Text))//
            {
                detailBO.GuestDOB = hmUtility.GetDateTimeFromString(this.txtGuestDOB.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.GuestDOB = null;
            }
            detailBO.GuestDrivinlgLicense = this.txtGuestDrivinlgLicense.Text;//
            detailBO.GuestEmail = this.txtGuestEmail.Text;//
            detailBO.GuestName = this.txtGuestName.Text;//
            if (string.IsNullOrEmpty(hiddenGuestId.Value))
            {
                detailBO.GuestId = 0;
                documentBO.GuestId = 0;
            }
            else
            {
                detailBO.GuestId = Int32.Parse(hiddenGuestId.Value);//
                documentBO.GuestId = Int32.Parse(hiddenGuestId.Value);
            }
            detailBO.GuestNationality = this.txtGuestNationality.Text;
            detailBO.GuestPhone = this.txtGuestPhone.Text;
            detailBO.GuestSex = this.ddlGuestSex.Text;
            detailBO.GuestZipCode = this.txtGuestZipCode.Text;
            detailBO.NationalId = this.txtNationalId.Text;
            detailBO.PassportNumber = this.txtPassportNumber.Text;
            if (!string.IsNullOrWhiteSpace(this.txtPExpireDate.Text))//
            {
                detailBO.PExpireDate = hmUtility.GetDateTimeFromString(this.txtPExpireDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.PExpireDate = null;
            }
            if (!string.IsNullOrWhiteSpace(this.txtPIssueDate.Text))//
            {
                detailBO.PIssueDate = hmUtility.GetDateTimeFromString(this.txtPIssueDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.PIssueDate = null;
            }
            detailBO.PIssuePlace = this.txtPIssuePlace.Text;
            if (!string.IsNullOrWhiteSpace(this.txtVExpireDate.Text))//
            {
                detailBO.VExpireDate = hmUtility.GetDateTimeFromString(this.txtVExpireDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.VExpireDate = null;
            }
            detailBO.VisaNumber = this.txtVisaNumber.Text;
            if (!string.IsNullOrWhiteSpace(this.txtVIssueDate.Text))//
            {
                detailBO.VIssueDate = hmUtility.GetDateTimeFromString(this.txtVIssueDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.VIssueDate = null;
            }

            registrationDetailListBO.Add(detailBO);
            Session["RegistrationDetailList"] = registrationDetailListBO;
            this.CheckObjectPermission();
            //  this.gvRegistrationDetail.DataSource = Session["RegistrationDetailList"] as List<GuestInformationBO>;
            //  this.gvRegistrationDetail.DataBind();

            this.ClearDetailPart();
        }
        protected void btnCancelDetailGuest_Click(object sender, EventArgs e)
        {
            this.ClearDetailPart();
        }
        protected void ddlRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            int EditId = Convert.ToInt32(this.ddlRoomId.SelectedValue);
            //this.CommonValueSet(EditId, 0);
        }
        protected void ddlEntitleRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int EditId = Convert.ToInt32(this.ddlEntitleRoomType.SelectedValue);
            //this.CommonValueSet(0, EditId);
        }
        protected void btnNewRegistration_Click(object sender, EventArgs e)
        {
            this.Cancel();
            //  this.btnNewRegistration.Visible = false;
        }
        protected void ddlReservationId_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.chkIsFromReservation.Checked)
            //{
            //    if (this.ddlReservationId.SelectedIndex != -1)
            //    {
            //        RoomReservationDA reservationDA = new RoomReservationDA();
            //        ReservationBillPaymentBO paymentBO = new ReservationBillPaymentBO();
            //        paymentBO = reservationDA.GetRoomReservationAdvanceAmountInfoById(Convert.ToInt32(this.ddlReservationId.SelectedValue));                    
            //    }
            //}
        }
        protected void gvRegistrationDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvRoomRegistration_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Visible = isSavePermission;
                //  imgDelete.Visible = isDeletePermission;
            }
        }
        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    this.LoadGridView(1, 1, 0);
        //}
        protected void gvRoomRegistration_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdEdit")
            {
                Session["_RoomRegistrationId"] = e.CommandArgument.ToString();
                this.FillForm(Convert.ToInt32(e.CommandArgument.ToString()));
                hfIsPaidServiceAlreadyLoded.Value = "0";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                try
                {
                    //this._RoomRegistrationId = Convert.ToInt32(e.CommandArgument.ToString());
                    //Session["_RoomRegistrationId"] = this._RoomRegistrationId;
                    //this.DeleteData(this._RoomRegistrationId);
                    //this.Cancel();
                    //this.LoadGridView();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (e.CommandName == "CmdPreview")
            {
                try
                {
                    string url = "/HotelManagement/Reports/frmReportRegistrationDetailInfo.aspx?RegistrationId=" + Convert.ToInt32(e.CommandArgument.ToString());
                    string sPopUp = "window.open('" + url + "', 'popup_window', 'width=715,height=780,left=300,top=50,resizable=yes, scrollbars=1');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

            this.Cancel();
            //this.isMessageBoxEnable = 1;
            //this.lblMessage.Text = this.ddlRoomId.SelectedValue.ToString();

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFrmValid())
                {
                    return;
                }

                if (!IsRoomAvailableForRegistration("btnSave"))
                {
                    return;
                }

                decimal roomRate;

                txtRoomRate.Text = !string.IsNullOrWhiteSpace(txtRoomRate.Text) ? txtRoomRate.Text : "0";
                txtDiscountAmount.Text = !string.IsNullOrWhiteSpace(txtDiscountAmount.Text) ? txtDiscountAmount.Text : "0";

                if (decimal.TryParse(txtRoomRate.Text, out roomRate))
                {
                    // it's a valid integer => you could use the distance variable here
                    decimal discountAmount;
                    if (decimal.TryParse(txtDiscountAmount.Text, out discountAmount))
                    {
                        // it's a valid integer => you could use the distance variable here
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Entered Discount Amount is not in correct format.", AlertType.Warning);
                        this.txtDiscountAmount.Focus();
                        this.SetTab("EntryTab");
                        return;
                    }
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, "Entered Negotiated Rate is not in correct format.", AlertType.Warning);
                    this.txtRoomRate.Focus();
                    this.SetTab("EntryTab");
                    return;
                }               

                int transactionId = 0;
                int registrationId = 0;
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
                    transactionId = Convert.ToInt32(customField.FieldValue);
                    CommonNodeMatrixBO commonNodeMatrixBO = new CommonNodeMatrixBO();
                    commonNodeMatrixBO = hmCommonDA.GetCommonNodeMatrixInfoById(transactionId);
                    if (commonNodeMatrixBO == null)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Please Contact With Administrator for Accounts Mapping.", AlertType.Warning);
                        return;
                    }
                    else
                    {
                        UserInformationBO userInformationBO = new UserInformationBO();
                        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                        int guestNumber = !string.IsNullOrWhiteSpace(this.txtNumberOfPersonAdult.Text) ? Convert.ToInt32(this.txtNumberOfPersonAdult.Text) : 1;

                        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                        RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();
                        //GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();

                        if (this.cbServiceCharge.Checked)
                        {
                            roomRegistrationBO.IsServiceChargeEnable = true;
                        }
                        else
                        {
                            roomRegistrationBO.IsServiceChargeEnable = false;
                        }

                        if (this.cbVatAmount.Checked)
                        {
                            roomRegistrationBO.IsVatAmountEnable = true;
                        }
                        else
                        {
                            roomRegistrationBO.IsVatAmountEnable = false;
                        }
                        
                        roomRegistrationBO.ArriveDate = hmUtility.GetDateTimeFromString(this.txtReCheckInDate.Text, userInformationBO.ServerDateFormat);                        
                        roomRegistrationBO.ExpectedCheckOutDate = hmUtility.GetDateTimeFromString(this.txtDepartureDate.Text, userInformationBO.ServerDateFormat);
                        roomRegistrationBO.IsCompanyGuest = Convert.ToBoolean(this.ddlIsCompanyGuest.SelectedIndex);
                        roomRegistrationBO.IsHouseUseRoom = Convert.ToBoolean(this.ddlIsHouseUseRoom.SelectedIndex);
                        roomRegistrationBO.RoomId = Convert.ToInt32(this.ddlRoomIdHiddenField.Value);                        
                        roomRegistrationBO.EntitleRoomType = Convert.ToInt32(this.ddlRoomType.SelectedValue);                        
                        roomRegistrationBO.DiscountType = this.ddlDiscountType.SelectedValue.ToString();
                        roomRegistrationBO.UnitPrice = !string.IsNullOrWhiteSpace(this.txtUnitPriceHiddenField.Value) ? Convert.ToDecimal(this.txtUnitPriceHiddenField.Value) : 0;
                        if (ddlIsCompanyGuest.SelectedValue == "Yes")
                        {
                            roomRegistrationBO.RoomRate = 0;
                            roomRegistrationBO.IsCompanyGuest = true;
                        }
                        else
                        {
                            roomRegistrationBO.RoomRate = !string.IsNullOrWhiteSpace(this.txtRoomRate.Text) ? Convert.ToDecimal(this.txtRoomRate.Text) : 0;
                            if (roomRegistrationBO.RoomRate == 0)
                            {
                                roomRegistrationBO.IsCompanyGuest = false;
                            }
                        }

                        if (ddlIsHouseUseRoom.SelectedValue == "Yes")
                        {
                            roomRegistrationBO.RoomRate = 0;
                            roomRegistrationBO.IsHouseUseRoom = true;
                        }
                        else
                        {
                            //roomRegistrationBO.RoomRate = !string.IsNullOrWhiteSpace(this.txtRoomRate.Text) ? Convert.ToDecimal(this.txtRoomRate.Text) : 0;
                            if (roomRegistrationBO.RoomRate == 0)
                            {
                                roomRegistrationBO.IsHouseUseRoom = false;
                            }
                        }

                        if (txtDiscountAmount.Text.Trim() != "" || txtDiscountAmount.Text.Trim() != "0")
                        {
                            roomRegistrationBO.DiscountAmount = Convert.ToDecimal(txtDiscountAmount.Text.Trim());
                        }
                        else
                        {
                            roomRegistrationBO.DiscountAmount = 0;
                        }                        

                        roomRegistrationBO.CommingFrom = this.txtCommingFrom.Text;
                        roomRegistrationBO.NextDestination = this.txtNextDestination.Text;
                        roomRegistrationBO.VisitPurpose = this.txtVisitPurpose.Text;
                        roomRegistrationBO.IsFamilyOrCouple = this.cbFamilyOrCouple.Checked ? true : false;
                        roomRegistrationBO.NumberOfPersonAdult = !string.IsNullOrWhiteSpace(this.txtNumberOfPersonAdult.Text) ? Int32.Parse(this.txtNumberOfPersonAdult.Text) : 1;
                        roomRegistrationBO.GuestSourceId = Int32.Parse(ddlGuestSource.SelectedValue);
                        if (this.chkIsReturnedGuest.Checked)
                        {
                            roomRegistrationBO.IsReturnedGuest = true;
                        }
                        else
                        {
                            roomRegistrationBO.IsReturnedGuest = false;
                        }
                        roomRegistrationBO.IsVIPGuest = this.chkIsVIPGuest.Checked ? true : false;
                        roomRegistrationBO.VIPGuestTypeId = ddlVIPGuestType.SelectedIndex != 0 ? Convert.ToInt32(ddlVIPGuestType.SelectedValue) : 0;

                        roomRegistrationBO.NumberOfPersonChild = string.IsNullOrWhiteSpace(this.txtNumberOfPersonChild.Text) ? 0 : Convert.ToInt32(this.txtNumberOfPersonChild.Text.Trim());                        
                        roomRegistrationBO.ContactPerson = this.txtContactPerson.Text;
                        roomRegistrationBO.ContactNumber = this.txtContactNumber.Text;
                        
                        roomRegistrationBO.IsRoomOwner = this.ddlRoomOwner.SelectedIndex;
                        roomRegistrationBO.ReferenceId = Convert.ToInt32(this.ddlReferenceId.SelectedValue);
                        roomRegistrationBO.Remarks = this.txtRemarks.Text;

                        List<RegistrationComplementaryItemBO> complementaryItemBOList = new List<RegistrationComplementaryItemBO>();
                        // -- Complementary Item Information-------------------------------------------------
                        for (int i = 0; i < chkComplementaryItem.Items.Count; i++)
                        {
                            if (chkComplementaryItem.Items[i].Selected)
                            {
                                RegistrationComplementaryItemBO complementaryItemBO = new RegistrationComplementaryItemBO();
                                complementaryItemBO.ComplementaryItemId = Int32.Parse(chkComplementaryItem.Items[i].Value);
                                complementaryItemBOList.Add(complementaryItemBO);
                            }
                        }
                        // -- Complementary Item Information----------------------------------------End----

                        // -- Airport Pickup and Drop Information------------------------------------------
                        roomRegistrationBO.AirportPickUp = this.ddlAirportPickUp.SelectedValue;
                        roomRegistrationBO.AirportDrop = this.ddlAirportDrop.SelectedValue;

                        int isAirportPickupDropExist = 0;
                        if (!string.IsNullOrWhiteSpace(this.txtArrivalFlightName.Text))
                        {
                            isAirportPickupDropExist = 1;
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtArrivalFlightNumber.Text))
                        {
                            isAirportPickupDropExist = 1;
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtDepartureFlightName.Text))
                        {
                            isAirportPickupDropExist = 1;
                        }
                        if (!string.IsNullOrWhiteSpace(this.txtDepartureFlightNumber.Text))
                        {
                            isAirportPickupDropExist = 1;
                        }
                        int AHour = 0;
                        DateTime date = DateTime.Now;
                        roomRegistrationBO.IsAirportPickupDropExist = isAirportPickupDropExist;
                        roomRegistrationBO.ArrivalFlightName = this.txtArrivalFlightName.Text;
                        roomRegistrationBO.ArrivalFlightNumber = this.txtArrivalFlightNumber.Text;
                        //if( == "NaN")
                        this.txtArrivalMin.Text = this.txtArrivalMin.Text == "NaN" ? "" : this.txtArrivalMin.Text;
                        this.txtArrivalHour.Text = this.txtArrivalHour.Text == "NaN" ? "" : this.txtArrivalHour.Text;
                        int AMin = !string.IsNullOrWhiteSpace(this.txtArrivalMin.Text) ? Convert.ToInt32(this.txtArrivalMin.Text) : 0;
                        if (!string.IsNullOrWhiteSpace(this.txtArrivalHour.Text))
                        {
                            AHour = this.ddlArrivalAmPm.SelectedIndex == 0 ? (Convert.ToInt32(this.txtArrivalHour.Text) % 12) : ((Convert.ToInt32(this.txtArrivalHour.Text) % 12) + 12);
                        }
                        DateTime currentDate = DateTime.Today;
                        //roomRegistrationBO.ArrivalTime = currentDate.AddHours(AHour).AddMinutes(AMin);
                        roomRegistrationBO.ArrivalTime = currentDate.Date;

                        roomRegistrationBO.DepartureFlightName = this.txtDepartureFlightName.Text;
                        roomRegistrationBO.DepartureFlightNumber = this.txtDepartureFlightNumber.Text;                        

                        if (!string.IsNullOrWhiteSpace(txtDepartureHour.Text))
                        {
                            roomRegistrationBO.DepartureTime = Convert.ToDateTime(txtDepartureHour.Text);
                        }
                        else
                        {
                            roomRegistrationBO.DepartureTime = null;
                        }
                        // -- Airport Pickup and Drop Information-------------------------------End--------

                        // -- Advance Payment Information--------------------------------------------------
                        GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();
                        GuestBillPaymentDA guestBillPaymentDA = new GuestBillPaymentDA();

                        //--------** Paid Service Save, Update **-------------------------

                        //List<RegistrationServiceInfoBO> paidServiceDetails = new List<RegistrationServiceInfoBO>();
                        //bool paidServiceDeleted = false;

                        //paidServiceDetails = JsonConvert.DeserializeObject<List<RegistrationServiceInfoBO>>(hfPaidServiceSaveObj.Value.ToString());
                        //paidServiceDeleted = hfPaidServiceDeleteObj.Value.ToString().Trim() == "1" ? true : false;

                        //if (!paidServiceDeleted && paidServiceDetails != null)
                        //{
                        //    paidServiceDetails = (from ps in paidServiceDetails where ps.DetailServiceId == 0 select ps).ToList();

                        //    if (paidServiceDetails.Count == 0)
                        //    {
                        //        paidServiceDetails = null;
                        //    }
                        //}

                        // -- Advance Payment Information-------------------------------------End--------
                        //roomRegistrationBO.RegistrationId = Convert.ToInt32(Session["_RoomRegistrationId"]);

                        // -- Credit Card Information ---------------------------------------------------
                        roomRegistrationBO.CardType = ddlCreditCardType.SelectedValue;
                        roomRegistrationBO.CardNumber = txtCardNo.Text;
                        roomRegistrationBO.CardHolderName = txtCardHolder.Text;
                        if (!string.IsNullOrEmpty(txtExpiryDate.Text))
                        {
                            //roomRegistrationBO.CardExpireDate = Convert.ToDateTime(txtExpiryDate.Text);
                            roomRegistrationBO.CardExpireDate = CommonHelper.DateTimeToMMDDYYYY(txtExpiryDate.Text);
                        }
                        else
                        {
                            roomRegistrationBO.CardExpireDate = null;
                        }
                        roomRegistrationBO.CardReference = txtCardRef.Text;

                        if (!string.IsNullOrEmpty(hfHoldRegistrationId.Value))
                        {
                            roomRegistrationBO.RegistrationId = Convert.ToInt32(hfHoldRegistrationId.Value);
                        }
                        roomRegistrationBO.LastModifiedBy = userInformationBO.UserInfoId;

                        //--------** Complimentary Item Process **---------------

                        HMComplementaryItemDA complementaryItemDA = new HMComplementaryItemDA();
                        List<HMComplementaryItemBO> complementaryItemAlreadySaved = new List<HMComplementaryItemBO>();
                        complementaryItemAlreadySaved = complementaryItemDA.GetComplementaryItemInfoByRegistrationId(roomRegistrationBO.RegistrationId);

                        List<RegistrationComplementaryItemBO> newlyAddedComplementaryItem = new List<RegistrationComplementaryItemBO>();
                        List<RegistrationComplementaryItemBO> deletedComplementaryItem = new List<RegistrationComplementaryItemBO>();

                        RegistrationComplementaryItemBO complementaryItem;

                        foreach (HMComplementaryItemBO cmitm in complementaryItemAlreadySaved)
                        {
                            var v = (from c in complementaryItemBOList where c.ComplementaryItemId == cmitm.ComplementaryItemId select c).FirstOrDefault();

                            if (v == null)
                            {
                                complementaryItem = new RegistrationComplementaryItemBO();
                                complementaryItem.ComplementaryItemId = cmitm.ComplementaryItemId;
                                complementaryItem.RegistrationId = roomRegistrationBO.RegistrationId;
                                complementaryItem.RCItemId = cmitm.RCItemId;

                                deletedComplementaryItem.Add(complementaryItem);
                            }
                        }

                        foreach (RegistrationComplementaryItemBO cmitm in complementaryItemBOList)
                        {
                            var v = (from c in complementaryItemAlreadySaved where c.ComplementaryItemId == cmitm.ComplementaryItemId select c).FirstOrDefault();

                            if (v == null)
                            {
                                complementaryItem = new RegistrationComplementaryItemBO();
                                complementaryItem.ComplementaryItemId = cmitm.ComplementaryItemId;
                                complementaryItem.RegistrationId = roomRegistrationBO.RegistrationId;

                                newlyAddedComplementaryItem.Add(complementaryItem);
                            }
                        }

                        //-------------------------------------------------------
                        string alreadyRegisteredGuestDeletedId = string.Empty;
                        alreadyRegisteredGuestDeletedId = hfDeletedGuest.Value;

                        Boolean status = roomRegistrationDA.UpdateHoldRoomRegistrationInfo(roomRegistrationBO, newlyAddedComplementaryItem, deletedComplementaryItem);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomRegistration.ToString(), roomRegistrationBO.RegistrationId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomRegistration));

                            this.LoadRoomNumber(0);

                            this.CommonValueSet(Convert.ToInt32(this.ddlRoomId.SelectedValue), 0);
                            this.Cancel();
                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                        }

                    }
                }

                //if (!string.IsNullOrEmpty(Request.QueryString["SelectedRoomNumber"]))
                //{
                //    Session["MessegePanelEnableForSelectedRoomNumber"] = "True";
                //    Session["PopUpPanelEnableForSelectedRoomNumber"] = "/HotelManagement/Reports/frmReportRegistrationDetailInfo.aspx?RegistrationId=" + registrationId;
                //    Response.Redirect("/HotelManagement/frmRoomRegistration.aspx");
                //}
                //else
                //{
                //    Session["MessegePanelEnableForSelectedRoomNumber"] = null;
                //    Session["PopUpPanelEnableForSelectedRoomNumber"] = null;
                //}
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        //************************ User Defined Function ********************//
        private bool IsRoomAvailableForRegistration(string loadFrom)
        {
            bool flag = true;
            if (Session["MessegePanelEnableForSelectedRoomNumber"] == null)
            {
                int availableRoomQty = 0;
                HMUtility hmUtility = new HMUtility();
                RoomNumberBO roomBO = new RoomNumberBO();
                RoomNumberDA roomDA = new RoomNumberDA();

                DateTime dateTime = DateTime.Now;
                DateTime StartDate = dateTime;
                DateTime EndDate = dateTime;

                StartDate = DateTime.Now;
                int roomTypeId = Convert.ToInt32(this.ddlRoomType.SelectedValue);

                if (roomTypeId > 0)
                {
                    if (!string.IsNullOrWhiteSpace(txtDepartureDate.Text))
                    {
                        EndDate = hmUtility.GetDateTimeFromString(txtDepartureDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                    }
                    else
                    {
                        EndDate = dateTime.AddDays(1);
                    }

                    List<RoomNumberBO> RoomNumberBOList = roomDA.GetRoomNumberInfoForCalender(StartDate, EndDate);

                    List<RoomNumberBO> typeWiseRoomNumberBOList = RoomNumberBOList.Where(x => x.RoomTypeId == roomTypeId).ToList();

                    RoomNumberDA roomNumberDA = new RoomNumberDA();
                    List<RoomNumberBO> typeWiseAssignedRoomNumberBOList = new List<RoomNumberBO>();

                    int _reservationId = 0;
                    //if (chkIsFromReservation.Checked)
                    //{
                    //    _reservationId = Int32.Parse(this.ddlReservationId.SelectedValue);
                    //}

                    typeWiseAssignedRoomNumberBOList = roomNumberDA.GetAvailableRoomNumberInformation(roomTypeId, 0, StartDate, EndDate, _reservationId).Where(x => x.RoomTypeId == roomTypeId).ToList();

                    List<RoomNumberBO> typeWiseUnassignedRoomNumberBOList = typeWiseRoomNumberBOList.Where(x => x.RoomId > 5000).ToList();

                    if (_reservationId > 0)
                    {
                        availableRoomQty = typeWiseAssignedRoomNumberBOList.Count;
                    }
                    else
                    {
                        availableRoomQty = typeWiseAssignedRoomNumberBOList.Count - typeWiseUnassignedRoomNumberBOList.Count;
                    }

                    if (availableRoomQty <= 0)
                    {


                        //if (!string.IsNullOrEmpty(Request.QueryString["SelectedRoomNumber"]))
                        //{
                        if (loadFrom != "btnSave")
                        {
                            IsRoomAvailableForRegistrationEnable = 1;
                            isMessageBoxEnable = -1;

                            this.isMessageBoxEnable = 1;
                            this.lblMessage.Text = "This type of room is not available. If you continue then it will overbooking.";

                            //CommonHelper.AlertInfo(innboardMessage, "This type of room is not available.", AlertType.Warning);
                            this.SetTab("EntryTab");
                            flag = false;
                            return flag;
                        }
                        else
                        {
                            IsRoomAvailableForRegistrationEnable = -1;
                        }
                        //numberBO = numberDA.GetRoomNumberInfoById(Int32.Parse(Request.QueryString["SelectedRoomNumber"]));
                        //SelectedRoomTypeId = numberBO.RoomTypeId.ToString();
                        //}
                        //else
                        //{
                        //    isMessageBoxEnable = -1;
                        //    IsRoomAvailableForRegistrationEnable = -1;
                        //    this.isMessageBoxEnable = 1;
                        //    this.lblMessage.Text = "This type of room is not available. If you continue then it will over booking.";

                        //    CommonHelper.AlertInfo(innboardMessage, "This type of room is not available.", AlertType.Warning);
                        //    this.SetTab("EntryTab");
                        //    flag = false;
                        //    return flag;
                        //}
                    }
                }
            }
            return flag;
        }
        private void LoadRackRateServiceChargeVatPanelInformation()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isInnboardVatEnableBO = new HMCommonSetupBO();
            isInnboardVatEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsInnboardVatEnable", "IsInnboardVatEnable");

            HMCommonSetupBO isInnboardServiceChargeEnableBO = new HMCommonSetupBO();
            isInnboardServiceChargeEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsInnboardServiceChargeEnable", "IsInnboardServiceChargeEnable");

            if (Convert.ToInt32(isInnboardVatEnableBO.SetupValue) + Convert.ToInt32(isInnboardServiceChargeEnableBO.SetupValue) == 0)
            {
                this.pnlRackRateServiceChargeVatInformation.Visible = false;
                this.cbServiceCharge.Checked = false;
                this.cbVatAmount.Checked = false;
            }
            else
            {
                this.cbServiceCharge.Checked = true;
                this.cbVatAmount.Checked = true;
                this.pnlRackRateServiceChargeVatInformation.Visible = true;
            }
        }
        private void LoadTotalRoomTariffLabelChange()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InclusiveHotelManagementBill", "Inclusive HotelManagement Bill Setup");
            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        this.lblTotalRoomRate.Text = "Total Room Tariff";
                    }
                    else
                    {
                        this.lblTotalRoomRate.Text = "Room Tariff";
                    }
                }
                else
                {
                    this.lblTotalRoomRate.Text = "Total Room Tariff";
                }
            }
        }
        private void LoadIsRoomOverbookingEnable()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRoomOverbookingEnable", "IsRoomOverbookingEnable");
            hfIsRoomOverbookingEnable.Value = "0";
            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        hfIsRoomOverbookingEnable.Value = "1";
                    }
                    else
                    {
                        hfIsRoomOverbookingEnable.Value = "0";
                    }
                }
            }
        }
        //private void LoadBank()
        //{
        //    BankDA bankDA = new BankDA();
        //    List<BankBO> entityBOList = new List<BankBO>();
        //    entityBOList = bankDA.GetBankInfo();

        //    this.ddlBankId.DataSource = entityBOList;
        //    this.ddlBankId.DataTextField = "BankName";
        //    this.ddlBankId.DataValueField = "BankId";
        //    this.ddlBankId.DataBind();

        //    ListItem itemBank = new ListItem();
        //    itemBank.Value = "0";
        //    itemBank.Text = hmUtility.GetDropDownFirstValue();
        //    this.ddlBankId.Items.Insert(0, itemBank);
        //}
        private void LoadProfession()
        {
            CommonProfessionDA professionDA = new CommonProfessionDA();
            List<CommonProfessionBO> entityBOList = new List<CommonProfessionBO>();
            entityBOList = professionDA.GetProfessionInfo();

            this.ddlProfessionId.DataSource = entityBOList;
            this.ddlProfessionId.DataTextField = "ProfessionName";
            this.ddlProfessionId.DataValueField = "ProfessionId";
            this.ddlProfessionId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlProfessionId.Items.Insert(0, item);
            //ddlProfessionId.SelectedValue = "0";
        }
        //private void LoadGridView(int pageNumber, int isCurrentOrPreviousPage, int gridRecordsCount)
        //{
        //    string RoomNumber = txtSearchRoomNumber.Text;
        //    string GueastName = txtSearchGuestName.Text;
        //    DateTime? CheckInDate = new DateTime();
        //    if (!string.IsNullOrWhiteSpace(txtChkInDate.Text))
        //    {
        //        CheckInDate = hmUtility.GetDateTimeFromString(txtChkInDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
        //    }
        //    else
        //    {
        //        CheckInDate = null;
        //    }
        //    string CompanyName = string.Empty, registrationNumber = string.Empty;
        //    int CountryId = 0;

        //    CompanyName = txtSearchCompanyName.Text;
        //    registrationNumber = txtSearchRegistrationNumber.Text;
        //    if (ddlSearchCountry.SelectedValue != "")
        //    {
        //        CountryId = Int32.Parse(ddlSearchCountry.SelectedValue);
        //    }

        //    this.CheckObjectPermission();

        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
        //    int totalRecords = 0;

        //    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
        //    List<RoomAlocationBO> roomAllocationBOList = new List<RoomAlocationBO>();
        //    //userInformationBO.GridViewPageSize
        //    GridViewDataNPaging<RoomAlocationBO, GridPaging> myGridData = new GridViewDataNPaging<RoomAlocationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
        //    pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

        //    //roomAllocationBOList = roomRegistrationDA.GetActiveRegistrationInfoBySearchCriteria(RoomNumber, GueastName, CheckInDate, CompanyName, CountryId, registrationNumber); userInformationBO.GridViewPageSize,
        //    roomAllocationBOList = roomRegistrationDA.GetRoomRegistrationInformationBySearchCriteriaForPaging(RoomNumber, GueastName, CheckInDate, CompanyName, CountryId, registrationNumber, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

        //    List<RoomAlocationBO> distinctItems = new List<RoomAlocationBO>();
        //    distinctItems = roomAllocationBOList.GroupBy(test => test.RegistrationId).Select(group => group.First()).ToList();

        //    myGridData.GridPagingProcessing(distinctItems, totalRecords, "GridPagingForSearchRegistration");

        //    if (roomAllocationBOList != null)
        //    {
        //        //this.gvRoomRegistration.DataSource = roomAllocationBOList;
        //        this.gvRoomRegistration.DataSource = distinctItems;
        //        this.gvRoomRegistration.DataBind();

        //        gridPaging.Text = myGridData.GridPageLinks.PreviousButton + myGridData.GridPageLinks.Pagination + myGridData.GridPageLinks.NextButton;
        //    }
        //    else
        //    {
        //        this.gvRoomRegistration.DataSource = null;
        //        this.gvRoomRegistration.DataBind();
        //        this.txtSrcRoomNumber.Focus();

        //        CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Room Number.", AlertType.Warning);
        //    }
        //    this.SetTab("SearchTab");
        //}
        //private void LoadCurrency()
        //{
        //    CommonCurrencyDA headDA = new CommonCurrencyDA();
        //    List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
        //    currencyListBO = headDA.GetConversionHeadInfoByType("LocalNUsd");

        //    this.ddlCurrency.DataSource = currencyListBO;
        //    this.ddlCurrency.DataTextField = "CurrencyName";
        //    this.ddlCurrency.DataValueField = "CurrencyId";
        //    this.ddlCurrency.DataBind();

        //    List<CommonCurrencyBO> allCurrencyListBO = new List<CommonCurrencyBO>();
        //    allCurrencyListBO = headDA.GetConversionHeadInfoByType("All");

        //    //this.ddlPaymentCurrency.DataSource = allCurrencyListBO;
        //    //this.ddlPaymentCurrency.DataTextField = "CurrencyName";
        //    //this.ddlPaymentCurrency.DataValueField = "CurrencyId";
        //    //this.ddlPaymentCurrency.DataBind();

        //    ListItem item = new ListItem();
        //    item.Value = "0";
        //    item.Text = hmUtility.GetDropDownFirstValue();
        //    //this.ddlPaymentCurrency.Items.Insert(0, item);
        //}
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
                        // this.txtConversionRate.ReadOnly = true;
                        isConversionRateEditable = true;
                    }
                    else
                    {
                        //this.txtConversionRate.ReadOnly = false;
                        isConversionRateEditable = false;
                    }
                }
            }
        }
        private void LoadRRPaymentMode()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();

            fields = commonDA.GetCustomField("RRPaymentMode", hmUtility.GetDropDownFirstValue());
            if (fields != null)
            {
                if (fields.Count > 1)
                {
                    fields.RemoveAt(0);
                }
            }

            this.ddlPayFor.DataSource = fields;
            this.ddlPayFor.DataTextField = "FieldValue";
            this.ddlPayFor.DataValueField = "FieldId";
            this.ddlPayFor.DataBind();

            ListItem itemPayFor = new ListItem();
            itemPayFor.Value = "0";
            itemPayFor.Text = hmUtility.GetDropDownFirstValue();
            this.ddlPayFor.Items.Insert(0, itemPayFor);
        }
        private void LoadComplementaryItem()
        {
            HMComplementaryItemDA entityDA = new HMComplementaryItemDA();
            List<HMComplementaryItemBO> files = entityDA.GetActiveHMComplementaryItemInfo();
            this.chkComplementaryItem.DataSource = files;
            this.chkComplementaryItem.DataTextField = "ItemName";
            this.chkComplementaryItem.DataValueField = "ComplementaryItemId";
            this.chkComplementaryItem.DataBind();
            //SetTab("SearchTab");
        }
        private void SetDefaultComplementaryItem()
        {
            HMComplementaryItemDA comDA = new HMComplementaryItemDA();
            List<HMComplementaryItemBO> comList = new List<HMComplementaryItemBO>();
            comList = comDA.GetActiveHMComplementaryItemInfo();
            for (int i = 0; i < comList.Count; i++)
            {
                if (comList[i].IsDefaultItem == true && comList[i].ActiveStat == true)
                    SetDefaultComplementaryItemById(comList[i].ComplementaryItemId);
            }
        }
        private void SetDefaultComplementaryItemById(int ComplementaryItemId)
        {
            for (int i = 0; i < chkComplementaryItem.Items.Count; i++)
            {
                if (chkComplementaryItem.Items[i].Value == ComplementaryItemId.ToString())
                {
                    chkComplementaryItem.Items[i].Selected = true;
                }
            }
        }
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtReCheckInDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            this.txtDisplayCheckInDate.Text = this.txtReCheckInDate.Text; //hmUtility.GetStringFromDateTime(dateTime);
            this.txtCheckInDateHiddenField.Text = this.txtReCheckInDate.Text; //hmUtility.GetStringFromDateTime(dateTime);
            this.txtDepartureDate.Text = hmUtility.GetStringFromDateTime(dateTime.AddDays(1));
        }
        public static void OpenNewBrowserWindow(string Url, Control control)
        {
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Open", "window.open('" + Url + "');", true);
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
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmRoomRegistration.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;

        }
        public void LoadGuestReference()
        {
            GuestReferenceDA entityDA = new GuestReferenceDA();
            List<GuestReferenceBO> files = entityDA.GetAllGuestRefference();
            ddlReferenceId.DataSource = files;
            ddlReferenceId.DataTextField = "Name";
            ddlReferenceId.DataValueField = "ReferenceId";
            ddlReferenceId.DataBind();

            ListItem itemReference = new ListItem();
            itemReference.Value = "0";
            itemReference.Text = hmUtility.GetDropDownFirstValue();
            this.ddlReferenceId.Items.Insert(0, itemReference);
        }
        //public void LoadAffiliatedCompany()
        //{
        //    GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
        //    List<GuestCompanyBO> files = guestCompanyDA.GetAffiliatedGuestCompanyInfo();
        //    ddlCompanyName.DataSource = files;
        //    ddlCompanyName.DataTextField = "CompanyName";
        //    ddlCompanyName.DataValueField = "CompanyId";
        //    ddlCompanyName.DataBind();

        //    ListItem itemReference = new ListItem();
        //    itemReference.Value = "0";
        //    itemReference.Text = hmUtility.GetDropDownFirstValue();
        //    this.ddlCompanyName.Items.Insert(0, itemReference);
        //}
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
        public bool isValidDetailForm()
        {
            bool status = true;

            if (string.IsNullOrWhiteSpace(this.txtGuestName.Text))
            {
                status = false;
                this.txtGuestName.Focus();
                CommonHelper.AlertInfo(innboardMessage, "Please provide Guest Name.", AlertType.Warning);
            }
            else if (string.IsNullOrWhiteSpace(this.txtGuestDOB.Text))
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, "Please provide Date Of Birth.", AlertType.Warning);
                this.txtGuestDOB.Focus();
            }

            return status;
        }
        private void ClearDetailPart()
        {
            string bangladesh = "19";

            this.txtGuestName.Text = string.Empty;
            this.hiddenGuestId.Value = string.Empty;
            this.txtGuestDOB.Text = string.Empty;
            this.ddlGuestSex.SelectedIndex = 0;
            this.txtGuestAddress1.Text = string.Empty;
            this.txtGuestAddress2.Text = string.Empty;
            this.txtGuestEmail.Text = string.Empty;
            this.txtGuestPhone.Text = string.Empty;
            this.txtGuestCity.Text = string.Empty;
            this.txtGuestZipCode.Text = string.Empty;
            this.ddlGuestCountry.SelectedValue = bangladesh;
            this.txtGuestNationality.Text = string.Empty;
            this.txtGuestDrivinlgLicense.Text = string.Empty;
            this.ddlProfessionId.SelectedValue = "1";
            this.txtNationalId.Text = string.Empty;
            this.txtVisaNumber.Text = string.Empty;
            this.txtVIssueDate.Text = string.Empty;

            this.txtVExpireDate.Text = string.Empty;
            this.txtPassportNumber.Text = string.Empty;
            this.txtPIssueDate.Text = string.Empty;
            this.txtPIssuePlace.Text = string.Empty;
            this.txtPExpireDate.Text = string.Empty;
        }
        private void Cancel()
        {
            IsRoomAvailableForRegistrationEnable = -1;
            hfIsEditAfterRegistration.Value = "";
            this.ddlRoomType.Enabled = true;
            this.ddlRoomId.Enabled = true;
            this.ddlEntitleRoomType.Enabled = true;
            //this.chkIsFromReservation.Enabled = true;
            //this.ddlReservationId.Enabled = true;
            this.txtDepartureDate.Text = string.Empty;
            this.ddlIsCompanyGuest.SelectedValue = "0";
            this.ddlIsHouseUseRoom.SelectedValue = "0";
            this.ddlRoomId.SelectedIndex = -1;
            this.txtRoomRate.Text = string.Empty;
            this.ddlRoomType.SelectedIndex = 0;
            ddlDiscountType.SelectedIndex = 0;
            this.txtDiscountAmount.Text = string.Empty;
            this.txtUnitPrice.Text = string.Empty;
            //this.chkIsFromReservation.Checked = false;
            this.txtCommingFrom.Text = string.Empty;
            this.txtNextDestination.Text = string.Empty;
            this.txtVisitPurpose.Text = string.Empty;
            //  this.txtLedgerAmount.Text = string.Empty;
            this.txtNumberOfPersonAdult.Text = "1";
            this.txtNumberOfPersonChild.Text = string.Empty;
            //this.chkIsLitedCompany.Checked = false;
            //this.ddlCompanyName.SelectedIndex = -1;
            //-------this.txtCompanyPhone.Text = string.Empty;
            //this.ddlBusinessPromotionId.SelectedValue = "0";
            this.ddlProfessionId.SelectedValue = "1";
            this.ddlRoomOwner.SelectedIndex = 0;
            this.chkIsReturnedGuest.Checked = false;
            this.chkIsVIPGuest.Checked = false;
            ddlVIPGuestType.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            Session["RegistrationDetailList"] = null;
            Session["arrayDelete"] = null;
            Session["TotalPaymentSummary"] = null;
            Session["GuestPaymentDetailListForGrid"] = null;
            Session["_RoomRegistrationId"] = null;
            // Session["_RoomRegistrationId"] = null;
            // this.gvRegistrationDetail.DataSource = Session["RegistrationDetailList"] as List<GuestInformationBO>;
            // this.gvRegistrationDetail.DataBind();

            this.txtArrivalFlightName.Text = string.Empty;
            this.txtArrivalFlightNumber.Text = string.Empty;
            this.txtArrivalHour.Text = "12";
            this.txtArrivalMin.Text = "00";
            this.ddlArrivalAmPm.SelectedIndex = 0;

            this.txtDepartureFlightName.Text = string.Empty;
            this.txtDepartureFlightNumber.Text = string.Empty;
            LoadProbableDepartureTime();
            //this.txtDepartureHour.Text = "12:00";
            //this.txtDepartureMin.Text = "00";
            //this.ddlDepartureAmPm.SelectedIndex = 0;
            HiddenCompanyId.Value = string.Empty;
            QSReservationId.Value = string.Empty;
            this.ddlRoomIdHiddenField.Value = string.Empty;
            //this.ddlBankId.SelectedValue = "0";
            for (int i = 0; i < chkComplementaryItem.Items.Count; i++)
            {
                chkComplementaryItem.Items[i].Selected = false;
            }

            hfPaidServiceSaveObj.Value = string.Empty;
            hfPaidServiceDeleteObj.Value = string.Empty;
            hfIsPaidServiceAlreadyLoded.Value = "0";
            hfIsPaidServiceAlreadySavedDb.Value = "0";
            hfInitialCurrencyType.Value = "";
            hfPreviousCurrencyType.Value = "";
            hfIsCurrencyChange.Value = "0";
            hfConversionRate.Value = "";
            this.txtRemarks.Text = string.Empty;

            hfIsComplementaryPaidService.Value = "0";

            this.LoadCurrentDate();
            this.SetTab("EntryTab");

            this.RoomRegistrationNumberGenerationForGuest();
        }
        private void LoadRoomNumber(int roomTypeId)
        {
            int isReservation = 0;
            //if (chkIsFromReservation.Checked)
            //{
            //    if (this.ddlReservationId.SelectedIndex != -1)
            //    {
            //        isReservation = Convert.ToInt32(this.ddlReservationId.SelectedValue);
            //    }
            //}

            //if (this.ddlReservationId.SelectedIndex > 0)
            //{
            //    isReservation = 1;
            //}
            ListItem item = new ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = hmUtility.GetDropDownFirstValue();
            if (ddlRoomType.SelectedValue == "0")
            {

                RoomNumberDA roomNumberDA = new RoomNumberDA();
                this.ddlRoomId.DataSource = roomNumberDA.GetRoomNumberInfo();
                this.ddlRoomId.DataTextField = "RoomNumber";
                this.ddlRoomId.DataValueField = "RoomId";
                this.ddlRoomId.DataBind();

            }
            else
            {
                DateTime dateTime = DateTime.Now;
                DateTime StartDate = dateTime;
                DateTime EndDate = dateTime;
                if (!string.IsNullOrWhiteSpace(this.txtReCheckInDate.Text))
                {
                    StartDate = hmUtility.GetDateTimeFromString(this.txtReCheckInDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                    //StartDate = hmUtility.GetDateTimeFromString(this.txtReCheckInDate.Text);
                }

                if (!string.IsNullOrWhiteSpace(this.txtDepartureDate.Text))
                {
                    EndDate = hmUtility.GetDateTimeFromString(this.txtDepartureDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                }

                RoomNumberDA roomNumberDA = new RoomNumberDA();
                this.ddlRoomId.DataSource = roomNumberDA.GetAvailableRoomNumberInfoForRegistrationForm(roomTypeId, isReservation, StartDate, EndDate);
                this.ddlRoomId.DataTextField = "RoomNumber";
                this.ddlRoomId.DataValueField = "RoomId";
                this.ddlRoomId.DataBind();
            }
            this.ddlRoomId.Items.Insert(0, item);
        }
        private void LoadRoomType()
        {
            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> entityBO = new List<RoomTypeBO>();
            entityBO = roomTypeDA.GetRoomTypeInfo();
            this.ddlRoomType.DataSource = entityBO;
            this.ddlRoomType.DataTextField = "RoomType";
            this.ddlRoomType.DataValueField = "RoomTypeId";
            this.ddlRoomType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = hmUtility.GetDropDownFirstValue();

            this.ddlEntitleRoomType.DataSource = entityBO;
            this.ddlEntitleRoomType.DataTextField = "RoomType";
            this.ddlEntitleRoomType.DataValueField = "RoomTypeId";
            this.ddlEntitleRoomType.DataBind();
            this.ddlRoomType.Items.Insert(0, item);
            this.ddlEntitleRoomType.Items.Insert(0, item);
            //ddlEntitleRoomType.SelectedValue = "0";

        }
        private void LoadGuestSource()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("GuestSource", hmUtility.GetDropDownFirstValue());

            this.ddlGuestSource.DataSource = fields;
            this.ddlGuestSource.DataTextField = "FieldValue";
            this.ddlGuestSource.DataValueField = "FieldId";
            this.ddlGuestSource.DataBind();
        }
        private void LoadCountryList()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CountriesBO> countryList = commonDA.GetAllCountries();
            this.ddlGuestCountry.DataSource = countryList;
            this.ddlGuestCountry.DataTextField = "CountryName";
            this.ddlGuestCountry.DataValueField = "CountryId";
            this.ddlGuestCountry.DataBind();
            //string bangladesh = "19";
            //ddlGuestCountry.SelectedValue = bangladesh;
        }
        //private void LoadSearchCountryList()
        //{
        //    HMCommonDA commonDA = new HMCommonDA();
        //    List<CountriesBO> countryList = commonDA.GetAllCountries();
        //    ddlSearchCountry.ClearSelection();
        //    this.ddlSearchCountry.DataSource = countryList;
        //    this.ddlSearchCountry.DataTextField = "CountryName";
        //    this.ddlSearchCountry.DataValueField = "CountryId";
        //    this.ddlSearchCountry.DataBind();
        //    ListItem itemCountry = new ListItem();
        //    itemCountry.Value = "0";
        //    itemCountry.Text = hmUtility.GetDropDownFirstValue();
        //    this.ddlSearchCountry.Items.Insert(0, itemCountry);
        //}
        //private void LoadRoomReservation()
        //{
        //    RoomReservationDA roomReservationDA = new RoomReservationDA();
        //    this.ddlReservationId.DataSource = roomReservationDA.GetRoomReservationInfoForRegistration(1);
        //    this.ddlReservationId.DataTextField = "ReservedCompany";
        //    this.ddlReservationId.DataValueField = "ReservationId";
        //    this.ddlReservationId.DataBind();

        //    ListItem itemReservation = new ListItem();
        //    itemReservation.Value = "0";
        //    itemReservation.Text = hmUtility.GetDropDownFirstValue();
        //    this.ddlReservationId.Items.Insert(0, itemReservation);
        //}
        //private void LoadBusinessPromotion()
        //{
        //    BusinessPromotionDA bpDA = new BusinessPromotionDA();
        //    this.ddlBusinessPromotionId.DataSource = bpDA.GetCurrentActiveBusinessPromotionInfo();
        //    this.ddlBusinessPromotionId.DataTextField = "BPHead";
        //    this.ddlBusinessPromotionId.DataValueField = "BusinessPromotionId";
        //    this.ddlBusinessPromotionId.DataBind();

        //    ListItem itemReservation = new ListItem();
        //    itemReservation.Value = "0";
        //    itemReservation.Text = hmUtility.GetDropDownFirstValue();
        //    this.ddlBusinessPromotionId.Items.Insert(0, itemReservation);
        //}
        private void LoadVIPGuestType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("VIPGuestType", hmUtility.GetDropDownFirstValue());

            ddlVIPGuestType.DataSource = fields;
            ddlVIPGuestType.DataTextField = "FieldValue";
            ddlVIPGuestType.DataValueField = "FieldId";
            ddlVIPGuestType.DataBind();
        }
        private void LoadProbableDepartureTime()
        {
            txtDepartureHour.Text = "12:00";
        }
        private void FillForm(int registrationId)
        {
            HMUtility hmUtility = new HMUtility();
            hfHoldRegistrationId.Value = registrationId.ToString();
            //hfIsEditAfterRegistration.Value = "1";
            //hfIsPaidServiceAlreadyLoded.Value = "0";

            this.btnSave.Text = "Update";
            Session["_RoomRegistrationId"] = registrationId;
            //RegistrationId = registrationId;

            //  RandomOwnerId.Value = registrationId.ToString();
            tempRegId.Value = registrationId.ToString();
            //ReservationIdHiddenField.Value = reservationId.ToString();

            //Master Information------------------------
            RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(registrationId);

            //LoadIsConversionRateEditable();

            //Session["_RoomRegistrationId"] = roomRegistrationBO.RegistrationId;

            //this.chkIsFromReservation.Checked = roomRegistrationBO.IsFromReservation;
            hiddendReservationId.Value = roomRegistrationBO.ReservationId.ToString();
            //if (this.chkIsFromReservation.Checked)
            //{
            //    isRegistrationEddited = 1;
            //    //this.chkIsFromReservation.Enabled = false;
            //    this.ddlReservationId.SelectedItem.Text = !string.IsNullOrWhiteSpace(roomRegistrationBO.ReservationInfo.ToString()) ? roomRegistrationBO.ReservationInfo.ToString() : "--- Please Select ---";
            //    //this.ddlReservationId.Enabled = false;
            //}

            //this.txtReCheckInDate.Text = hmUtility.GetStringFromDateTime(roomRegistrationBO.ArriveDate);
            this.txtCheckInDate.Text = hmUtility.GetStringFromDateTime(roomRegistrationBO.ArriveDate);
            this.txtReCheckInDate.Text = hmUtility.GetStringFromDateTime(DateTime.Now);
            this.txtDisplayCheckInDate.Text = hmUtility.GetStringFromDateTime(DateTime.Now);
            this.txtDepartureDate.Text = hmUtility.GetStringFromDateTime(roomRegistrationBO.ExpectedCheckOutDate);
            //this.ddlBusinessPromotionId.SelectedValue = roomRegistrationBO.BusinessPromotionId.ToString();
            //this.chkIsLitedCompany.Checked = roomRegistrationBO.IsListedCompany;
            //this.ddlCompanyName.SelectedValue = roomRegistrationBO.CompanyId.ToString();
            //HiddenCompanyId.Value = roomRegistrationBO.CompanyId.ToString();
            //this.ddlCurrency.SelectedValue = roomRegistrationBO.CurrencyType.ToString();
            //this.txtConversionRate.Text = roomRegistrationBO.ConversionRate.ToString();
            //hfConversionRate.Value = roomRegistrationBO.ConversionRate.ToString();
            this.ddlRoomType.SelectedIndex = 0;
            //this.ddlRoomId.SelectedValue = roomRegistrationBO.RoomId.ToString();
            //this.ddlRoomType.Enabled = false;
            //this.ddlRoomId.SelectedItem.Text = roomRegistrationBO.RoomNumber.ToString();
            this.ddlRoomIdHiddenField.Value = roomRegistrationBO.RoomId.ToString();
            //this.ddlRoomId.SelectedItem.Text = roomRegistrationBO.RoomNumber.ToString();
            ddlRoomId.Enabled = true;

            this.txtUnitPrice.Text = roomRegistrationBO.UnitPrice.ToString();
            this.txtUnitPriceHiddenField.Value = roomRegistrationBO.UnitPrice.ToString();
            this.txtRoomRate.Text = roomRegistrationBO.RoomRate.ToString();

            this.cbServiceCharge.Checked = roomRegistrationBO.IsServiceChargeEnable;
            this.cbVatAmount.Checked = roomRegistrationBO.IsVatAmountEnable;

            this.txtCommingFrom.Text = roomRegistrationBO.CommingFrom;
            this.txtNextDestination.Text = roomRegistrationBO.NextDestination;
            this.txtVisitPurpose.Text = roomRegistrationBO.VisitPurpose;
            this.ddlGuestSource.SelectedValue = roomRegistrationBO.GuestSourceId.ToString();
            this.ddlReferenceId.SelectedValue = roomRegistrationBO.ReferenceId.ToString();

            ddlDiscountType.SelectedValue = roomRegistrationBO.DiscountType;
            txtDiscountAmount.Text = roomRegistrationBO.DiscountAmount.ToString();

            this.ddlIsCompanyGuest.SelectedIndex = roomRegistrationBO.IsCompanyGuest == true ? 1 : 0;
            this.ddlIsHouseUseRoom.SelectedIndex = roomRegistrationBO.IsHouseUseRoom == true ? 1 : 0;

            //if (roomRegistrationBO.IsCompanyGuest == true)
            //    hfIsComplementaryPaidService.Value = "1";
            //else
            //    hfIsComplementaryPaidService.Value = "0";

            this.ddlRoomOwner.SelectedIndex = Convert.ToInt32(roomRegistrationBO.IsRoomOwner);
            this.chkIsReturnedGuest.Checked = roomRegistrationBO.IsReturnedGuest;
            this.chkIsVIPGuest.Checked = roomRegistrationBO.IsVIPGuest;
            ddlVIPGuestType.SelectedValue = roomRegistrationBO.VIPGuestTypeId.ToString();
            this.txtRemarks.Text = roomRegistrationBO.Remarks;
            this.cbFamilyOrCouple.Checked = roomRegistrationBO.IsFamilyOrCouple;
            this.txtNumberOfPersonAdult.Text = roomRegistrationBO.NumberOfPersonAdult.ToString();
            this.txtNumberOfPersonChild.Text = roomRegistrationBO.NumberOfPersonChild.ToString();

            //--Credit Card Information----------
            if (!string.IsNullOrEmpty(roomRegistrationBO.CardNumber))
            {
                this.CreditCardInfo.Visible = false;
            }
            ddlCreditCardType.SelectedValue = roomRegistrationBO.CardType;
            txtCardNo.Text = roomRegistrationBO.CardNumber;
            txtExpiryDate.Text = roomRegistrationBO.CardExpireDateShow;
            txtCardHolder.Text = roomRegistrationBO.CardHolderName;
            txtCardRef.Text = roomRegistrationBO.CardReference;
                        
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            //--Load Aireport Pickup/ Drop Information-----------
            this.ddlAirportDrop.SelectedValue = roomRegistrationBO.AirportDrop;
            if (roomRegistrationBO.APDId != 0)
            {
                aireportPickupInformationPanelEnable = -1;
                this.txtArrivalFlightName.Text = roomRegistrationBO.ArrivalFlightName;
                this.txtArrivalFlightNumber.Text = roomRegistrationBO.ArrivalFlightNumber;
                this.txtArrivalHour.Text = Convert.ToInt32(roomRegistrationBO.ArrivalTime.ToString("%h")) == 0 ? "12" : roomRegistrationBO.ArrivalTime.ToString("%h");
                this.txtArrivalMin.Text = roomRegistrationBO.ArrivalTime.ToString("mm");
                this.ddlArrivalAmPm.SelectedIndex = Convert.ToInt32(roomRegistrationBO.ArrivalTime.ToString("HH")) == 0 ? 0 : 1;

                this.txtDepartureFlightName.Text = roomRegistrationBO.DepartureFlightName;
                this.txtDepartureFlightNumber.Text = roomRegistrationBO.DepartureFlightNumber;

                if (roomRegistrationBO.DepartureTime != null)
                {
                    this.txtDepartureHour.Text = Convert.ToDateTime(roomRegistrationBO.DepartureTime.ToString()).ToString(userInformationBO.TimeFormat);
                }
                else
                {
                    this.txtDepartureHour.Text = null;
                }                
            }
            else
            {
                this.txtArrivalFlightName.Text = string.Empty;
                this.txtArrivalFlightNumber.Text = string.Empty;
                this.txtArrivalHour.Text = "12";
                this.txtArrivalMin.Text = "00";
                this.ddlArrivalAmPm.SelectedIndex = 0;

                this.txtDepartureFlightName.Text = string.Empty;
                this.txtDepartureFlightNumber.Text = string.Empty;
                LoadProbableDepartureTime();
                //this.txtDepartureHour.Text = "12:00";
                //this.txtDepartureMin.Text = "00";
                //this.ddlDepartureAmPm.SelectedIndex = 0;
            }
            ////--Load Complementary Item Information--------------- 
            //for (int i = 0; i < chkComplementaryItem.Items.Count; i++)
            //{
            //    if (Int32.Parse(chkComplementaryItem.Items[i].Value) == ComplementaryItemId)
            //    {
            //        chkComplementaryItem.Items[i].Selected = false;
            //    }
            //}

            List<HMComplementaryItemBO> complementaryList = new List<HMComplementaryItemBO>();
            HMComplementaryItemDA complementaryItemDA = new HMComplementaryItemDA();
            complementaryList = complementaryItemDA.GetComplementaryItemInfoByRegistrationId(registrationId);

            foreach (HMComplementaryItemBO dr in complementaryList)
            {
                for (int i = 0; i < chkComplementaryItem.Items.Count; i++)
                {
                    if (Int32.Parse(chkComplementaryItem.Items[i].Value) == dr.ComplementaryItemId)
                    {
                        chkComplementaryItem.Items[i].Selected = true;
                    }
                }
            }


            //----------Advance Payment Information-----------------
            List<GuestBillPaymentBO> guestBillPaymentBO = new List<GuestBillPaymentBO>();
            GuestBillPaymentDA guestBillPaymentDA = new GuestBillPaymentDA();

            //this.SetTab("EntryTab");
        }
        private void CommonValueSet(int roomId, int roomTypeId)
        {
            if (roomId > 0)
            {
                RoomNumberBO roomNumberBO = new RoomNumberBO();
                RoomNumberDA roomNumberDA = new RoomNumberDA();

                roomNumberBO = roomNumberDA.GetRoomNumberInfoById(roomId);

                RoomTypeBO roomTypeBO = new RoomTypeBO();
                RoomTypeDA roomTypeDA = new RoomTypeDA();

                roomTypeBO = roomTypeDA.GetRoomTypeInfoById(roomNumberBO.RoomTypeId);
                this.txtViewRoomType.Text = roomTypeBO.RoomType.ToString();
                this.txtUnitPrice.Text = roomTypeBO.RoomRate.ToString();

                if (txtEntiteledRoomType.Value == "")
                {
                    this.ddlEntitleRoomType.SelectedValue = roomNumberBO.RoomTypeId.ToString();
                    ddlEntitleRoomType.SelectedValue = "0";
                    this.txtRoomRate.Text = roomTypeBO.RoomRate.ToString();
                }
            }

            //if (roomTypeId > 0)
            //{
            //    RoomTypeBO roomTypeBO = new RoomTypeBO();
            //    RoomTypeDA roomTypeDA = new RoomTypeDA();

            //    roomTypeBO = roomTypeDA.GetRoomTypeInfoById(roomTypeId);
            //    this.txtRoomRate.Text = roomTypeBO.RoomRate.ToString();
            //}

        }
        private bool IsFrmValid()
        {
            this.txtUnitPrice.Text = this.txtUnitPriceHiddenField.Value;
            bool flag = true;

            if (ddlRoomId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Room Number.", AlertType.Warning);
                flag = false;
                return flag;
            }

            decimal comRoomRate = !string.IsNullOrWhiteSpace(this.txtRoomRate.Text) ? Convert.ToDecimal(this.txtRoomRate.Text) : 0;
            if (comRoomRate < 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Entered Negotiated Rate is not in correct format.", AlertType.Warning);
                this.txtRoomRate.Focus();
                this.SetTab("GuestInfoTab");
                flag = false;
                return flag;
            }

            int registrationId = Convert.ToInt32(Session["_RoomRegistrationId"]);

            if (registrationId > 0)
            {
                string tableName = "HotelGuestRegistration";
                string fieldName = "RegistrationId";
                string fieldValue = registrationId.ToString();
                int IsDuplicate = 0;
                HMCommonDA hmCommonDA = new HMCommonDA();
                IsDuplicate = hmCommonDA.DuplicateDataCountDynamicaly(tableName, fieldName, fieldValue);
                if (IsDuplicate == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please provide at least one Guest Information.", AlertType.Warning);
                    this.txtNumberOfPersonAdult.Focus();
                    this.SetTab("GuestInfoTab");
                    flag = false;
                    return flag;
                }
            }
            //if (!string.IsNullOrWhiteSpace(this.txtContactNumber.Text.Trim()))
            //{
            //    var match = Regex.Match(txtContactNumber.Text, @"^(?:(?:\(?(?:00|\+0)([1-4]\d\d|[1-9]\d?)\)?)?[\-\.\ \\\/]?)?((?:\(?\d{1,}\)?[\-\.\ \\\/]?){0,})(?:[\-\.\ \\\/]?(?:#|ext\.?|extension|x)[\-\.\ \\\/]?(\d+))?$");
            //    if (!match.Success)
            //    {
            //        CommonHelper.AlertInfo(innboardMessage, "Please provide valid mobile number.", AlertType.Warning);
            //        return flag = false;
            //    }
            //}

            if (string.IsNullOrWhiteSpace(this.txtDepartureDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide Departure Date.", AlertType.Warning);
                this.txtDepartureDate.Focus();
                flag = false;
            }
            else if (this.btnSave.Text == "Save")
            {
                if (this.ddlRoomId.SelectedValue == "0")
                {
                    if (string.IsNullOrWhiteSpace(this.ddlRoomIdHiddenField.Value))
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Please provide Room Number.", AlertType.Warning);
                        this.ddlRoomId.Focus();
                        flag = false;
                    }
                }
                else
                {
                    RoomNumberDA numberDA = new RoomNumberDA();
                    RoomNumberBO numberBO = numberDA.GetRoomNumberInfoById(Int32.Parse(this.ddlRoomId.SelectedValue));
                    if (numberBO.StatusId != 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Please provide Avilable Room Number.", AlertType.Warning);
                        this.ddlRoomId.Focus();
                        flag = false;
                    }
                }
            }
            else if (this.btnSave.Text == "Update")
            {
                if (string.IsNullOrWhiteSpace(this.ddlRoomIdHiddenField.Value))
                {
                    CommonHelper.AlertInfo(innboardMessage, "Please provide Room Number.", AlertType.Warning);
                    this.ddlRoomId.Focus();
                    flag = false;
                }
            }
            else if (string.IsNullOrWhiteSpace(this.txtNumberOfPersonAdult.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide Number Of Person(Adult).", AlertType.Warning);
                this.txtNumberOfPersonAdult.Focus();
                this.SetTab("GuestInfoTab");
                flag = false;
            }
            else if (hmUtility.GetDateTimeFromString(txtReCheckInDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) > hmUtility.GetDateTimeFromString(txtDepartureDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat))
            {
                CommonHelper.AlertInfo(innboardMessage, "Departure Date cannot be less than Check In Date.", AlertType.Warning);
                this.txtDepartureDate.Focus();
                this.SetTab("GuestInfoTab");
                flag = false;
            }

            if (chkIsVIPGuest.Checked == true)
            {
                if (ddlVIPGuestType.SelectedIndex == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "VIP Guest Type.", AlertType.Warning);
                    ddlVIPGuestType.Focus();
                    flag = false;
                }
            }

            return flag;
        }
        private void SetTab(string TabName)
        {

            if (TabName == "SearchTab")
            {
                //B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                //B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "GuestInfoTab")
            {
                C.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                //B.Attributes.Add("class", "ui-state-default ui-corner-top");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "BussinessPromotionTab")
            {
                D.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                //B.Attributes.Add("class", "ui-state-default ui-corner-top");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        private void LoadReservationGuest()
        {
            GuestInformationDA guestDA = new GuestInformationDA();
            var guestList = guestDA.GetAllGuestInformation();
            this.ddlReservationGuest.DataSource = guestList;
            this.ddlReservationGuest.DataTextField = "GuestName";
            this.ddlReservationGuest.DataValueField = "GuestId";
            this.ddlReservationGuest.DataBind();

            ListItem itemCompany = new ListItem();
            itemCompany.Value = "0";
            itemCompany.Text = hmUtility.GetDropDownFirstValue();
            this.ddlReservationGuest.Items.Insert(0, itemCompany);

        }
        //private void LoadAccountHeadInfo()
        //{
        //    HMCommonDA hmCommonDA = new HMCommonDA();
        //    NodeMatrixDA entityDA = new NodeMatrixDA();
        //    this.lblPaymentAccountHead.Text = "Payment Receive In";
        //    CustomFieldBO CashReceiveAccountsInfo = new CustomFieldBO();
        //    CashReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CashReceiveAccountsInfo");

        //    this.ddlCashReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CashReceiveAccountsInfo.FieldValue.ToString() + ") AND NodeId != 8");
        //    this.ddlCashReceiveAccountsInfo.DataTextField = "NodeHead";
        //    this.ddlCashReceiveAccountsInfo.DataValueField = "NodeId";
        //    this.ddlCashReceiveAccountsInfo.DataBind();

        //    CustomFieldBO CardReceiveAccountsInfo = new CustomFieldBO();
        //    List<NodeMatrixBO> cardEntityBOList = new List<NodeMatrixBO>();
        //    CardReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CardReceiveAccountsInfo");
        //    cardEntityBOList = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CardReceiveAccountsInfo.FieldValue.ToString() + ") AND NodeId != 8");
        //    this.ddlCardReceiveAccountsInfo.DataSource = cardEntityBOList;
        //    this.ddlCardReceiveAccountsInfo.DataTextField = "NodeHead";
        //    this.ddlCardReceiveAccountsInfo.DataValueField = "NodeId";
        //    this.ddlCardReceiveAccountsInfo.DataBind();

        //    this.ddlAccountsPostingHeadId.DataSource = cardEntityBOList;
        //    this.ddlAccountsPostingHeadId.DataTextField = "NodeHead";
        //    this.ddlAccountsPostingHeadId.DataValueField = "NodeId";
        //    this.ddlAccountsPostingHeadId.DataBind();

        //    CustomFieldBO ChequeReceiveAccountsInfo = new CustomFieldBO();
        //    ChequeReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("ChequeReceiveAccountsInfo");
        //}
        private static string GetUserDetailHtml(List<GuestInformationBO> registrationDetailListBO, string deletedGuestId)
        {
            string strTable = "";
            strTable += "<table style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";

            strTable += "<th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Email</th> <th align='left' scope='col'>Action</th></tr>";
            int counter = 0;

            string deletedAfterRegistration = string.Empty;

            foreach (GuestInformationBO dr in registrationDetailListBO)
            {
                if (!string.IsNullOrEmpty(deletedGuestId) && deletedGuestId.Contains(dr.GuestId.ToString()))
                {
                    continue;
                }

                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr id='tr" + dr.GuestId + "' style='background-color:#E3EAEB;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr id='tr" + dr.GuestId + "' style='background-color:White;'>";
                }

                strTable += "<td align='left' style='width: 50%'>" + dr.GuestName + "</td>";
                strTable += "<td align='left' style='width: 30%'>" + dr.GuestEmail + "</td>";
                strTable += "<td align='left' style='width: 20%'>";
                strTable += "&nbsp;<img src='../Images/edit.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return PerformEditActionForGuestDetail('" + dr.GuestId + "', '" + dr.RegistrationId + "')\" alt='Edit Information' border='0' />";
                strTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return PerformDeleteActionForGuestDetail('" + dr.GuestId + "', '" + dr.RegistrationId + "')\" alt='Delete Information' border='0' />";
                strTable += "</td>";
                strTable += "</tr>";
            }

            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            return strTable;
        }
        public void DeleteTempGuestRegistration()
        {
            RoomRegistrationDA regDA = new RoomRegistrationDA();
            int registrationId = Convert.ToInt32(Session["_RoomRegistrationId"]);
            Boolean status = regDA.DeleteTempGuestRegistration(registrationId);
            if (status)
                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),EntityTypeEnum.EntityType.RoomRegistration.ToString(), registrationId,ProjectModuleEnum.ProjectModule.FrontOffice.ToString(),hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomRegistration));
        }
        private static decimal GetGrandTotal(List<PMSalesDetailBO> salesDetailList, string Currency)
        {
            int Count = salesDetailList.Count;
            decimal salesAmount = 0;
            for (int i = 0; i < Count; i++)
            {
                if (Currency == salesDetailList[0].SellingLocalCurrencyId.ToString())
                {
                    salesAmount = salesAmount + Convert.ToDecimal(salesDetailList[i].TotalPrice);
                }
                else
                {
                    salesAmount = salesAmount + Convert.ToDecimal(salesDetailList[i].TotalPrice);
                }
            }
            return salesAmount;
        }
        public static string LoadGuestPaymentDetailGridViewByWM()
        {
            string strTable = "";
            List<GuestBillPaymentBO> detailList = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            if (detailList != null)
            {
                strTable += "<table style='width:100%' class='table table-bordered table-condensed table-responsive' id='ReservationDetailGrid'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Payment Type</th><th align='left' scope='col'>Payment Mode</th><th align='left' scope='col'>Amount</th><th align='center' scope='col'>Action</th></tr>";
                int counter = 0;
                foreach (GuestBillPaymentBO dr in detailList)
                {
                    counter++;
                    if (counter % 2 == 0)
                    {
                        // It's even
                        strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 30%;'>" + dr.PaymentType + "</td>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:White;'><td align='left' style='width: 30%;'>" + dr.PaymentType + "</td>";
                    }
                    strTable += "<td align='left' style='width: 25%;'>" + dr.PaymentMode + "</td>";
                    strTable += "<td align='left' style='width: 30%;'>" + dr.PaymentAmount + "</td>";
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
        private void ClearCommonSessionInformation()
        {
            this.Session["PMSalesDetailList"] = null;
            this.Session["RegistrationDetailList"] = null;
            this.Session["DocumentList"] = null;
            this.Session["GuestPaymentDetailListForGrid"] = null;
            this.Session["TransactionDetailList"] = null;
            //this.Session["_RoomRegistrationId"] = null;
            this.Session["DocumentList"] = null;
            this.Session["arrayDelete"] = null;
            this.Session["TotalPaymentSummary"] = null;
            this.Session["CompanyPaymentRoomIdList"] = null;
            this.Session["CompanyPaymentServiceIdList"] = null;
        }
        private void LoadCommonSetupForRackRateServiceChargeVatInformation()
        {
            List<CostCentreTabBO> costCentreTabBO = new List<CostCentreTabBO>();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoByType("FrontOffice");
            if (costCentreTabBO != null)
            {
                if (costCentreTabBO.Count > 0)
                {
                    hfInclusiveHotelManagementBill.Value = costCentreTabBO[0].IsVatSChargeInclusive.ToString();
                    hfGuestHouseVat.Value = costCentreTabBO[0].VatAmount.ToString();
                    hfGuestHouseServiceCharge.Value = costCentreTabBO[0].ServiceCharge.ToString();
                }
            }
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static RoomTypeBO FillFormByRoom(int EditId)
        {
            RoomNumberBO roomNumberBO = new RoomNumberBO();
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            roomNumberBO = roomNumberDA.GetRoomNumberInfoById(EditId);


            RoomTypeBO roomTypeBO = new RoomTypeBO();
            RoomTypeDA roomTypeDA = new RoomTypeDA();
            roomTypeBO = roomTypeDA.GetRoomTypeInfoById(roomNumberBO.RoomTypeId);

            return roomTypeBO;
        }
        [WebMethod]
        public static RoomTypeBO PerformFillFormActionByTypeId(int EditId)
        {
            RoomTypeBO roomTypeBO = new RoomTypeBO();
            RoomTypeDA roomTypeDA = new RoomTypeDA();
            roomTypeBO = roomTypeDA.GetRoomTypeInfoById(EditId);
            return roomTypeBO;
        }
        [WebMethod]
        public static ReservationDetailBO PerformFillFormActionByTypeIdTest(int reservationId, int roomTypeId)
        {
            ReservationDetailDA detailDA = new ReservationDetailDA();
            ReservationDetailBO DetailBO = detailDA.GetRoomReservationDetailByResIdAndRoomTypeId(reservationId, roomTypeId);
            return DetailBO;
        }
        [WebMethod]
        public static string LoadGrid(int actionId)
        {
            ReservationDetailDA detailDA = new ReservationDetailDA();
            List<ReservationDetailBO> entityListBO = new List<ReservationDetailBO>();
            entityListBO = detailDA.GetReservationDetailForRegistrationByRegiId(actionId);

            List<RoomTypeBO> typeList = new List<RoomTypeBO>();
            RoomTypeDA typeDA = new RoomTypeDA();
            typeList = typeDA.GetRoomTypeInfo();

            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation_Room'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col'>Room Type</th><th align='left' scope='col'>Room Number</th><th align='left' scope='col'></th></tr>";
            int counter = 0;
            foreach (ReservationDetailBO dr in entityListBO)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;cursor: pointer'><td align='left' style='width: 70%;cursor: pointer'  onClick=\"javascript:return SetRegistrationInfoByRoomTypeId(" + dr.RoomTypeId + ",'" + dr.DirtyRoomNumber + "')\">" + dr.RoomType + "</td>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:White;cursor: pointer'><td align='left' style='width: 70%;cursor: pointer'  onClick=\"javascript:return SetRegistrationInfoByRoomTypeId(" + dr.RoomTypeId + ",'" + dr.DirtyRoomNumber + "')\">" + dr.RoomType + "</td>";
                }

                strTable += "<td align='left' style='width: 30%;cursor: pointer'  onClick=\"javascript:return SetRegistrationInfoByRoomTypeId(" + dr.RoomTypeId + ",'" + dr.DirtyRoomNumber + "')\">" + dr.RoomNumber + "</td>";
            }
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }

            return strTable;
        }
        [WebMethod]
        public static string SearchGuestAndLoadGridInformation(string companyName, string DateOfBirth, string EmailAddress, string FromDate, string ToDate, string GuestName, string MobileNumber, string NationalId, string PassportNumber, string RegistrationNumber, string RoomNumber)
        {

            // KotBillDetailDA entityDA = new KotBillDetailDA();
            // List<KotBillDetailBO> files = entityDA.GetKotBillDetailInfoByKotNBearerId("RestaurantTable", tableId, kotId);
            HMUtility hmUtility = new HMUtility();
            HMCommonDA commonDA = new HMCommonDA();
            GuestInformationDA guestDA = new GuestInformationDA();
            List<GuestInformationBO> list = new List<GuestInformationBO>();

            DateTime? dateOfBirth = null;
            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (!string.IsNullOrWhiteSpace(DateOfBirth))
                dateOfBirth = hmUtility.GetDateTimeFromString(DateOfBirth, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            if (!string.IsNullOrWhiteSpace(FromDate))
                hmUtility.GetDateTimeFromString(FromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            if (!string.IsNullOrWhiteSpace(ToDate))
                hmUtility.GetDateTimeFromString(ToDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            list = guestDA.GetGuestInformationBySearchCriteria(GuestName, EmailAddress, MobileNumber, NationalId, PassportNumber, dateOfBirth, companyName, RoomNumber, fromDate, toDate, RegistrationNumber);
            return commonDA.GetHTMLGuestGridView(list);

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
                            guestBO.GuestPreferenceId += ", " + preference.PreferenceId;
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
        public static ArrayList PopulateRooms(int RoomTypeId, int ResevationId, string FromDate, string ToDate)
        {
            HMUtility hmUtility = new HMUtility();
            DateTime dateTime = DateTime.Now;
            DateTime StartDate = dateTime;
            DateTime EndDate = dateTime.AddDays(1);
            ArrayList list = new ArrayList();
            List<RoomNumberBO> roomList = new List<RoomNumberBO>();
            RoomNumberDA roomNumberDA = new RoomNumberDA();

            if (!string.IsNullOrWhiteSpace(FromDate))
            {
                StartDate = hmUtility.GetDateTimeFromString(FromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                //StartDate = hmUtility.GetDateTimeFromString(FromDate);
            }

            if (!string.IsNullOrWhiteSpace(ToDate))
            {
                EndDate = hmUtility.GetDateTimeFromString(ToDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                //EndDate = hmUtility.GetDateTimeFromString(ToDate);
            }

            if (ResevationId > 0)
            {
                roomList = roomNumberDA.GetAvailableRoomNumberInfoForRegistrationForm(RoomTypeId, ResevationId, StartDate, EndDate);
            }
            else
            {
                roomList = roomNumberDA.GetAvailableRoomNumberInfoForRegistrationForm(RoomTypeId, 0, StartDate, EndDate);
            }
            int count = roomList.Count;
            for (int i = 0; i < count; i++)
            {
                list.Add(new ListItem(
                                        roomList[i].RoomNumber.ToString(),
                                        roomList[i].RoomId.ToString()
                                         ));
            }
            return list;
        }
        [WebMethod]
        public static ArrayList PopulateReservationDropDown(int IsAllActiveReservation)
        {
            ArrayList list = new ArrayList();
            List<RoomReservationBO> roomList = new List<RoomReservationBO>();
            RoomReservationDA roomReservationDA = new RoomReservationDA();
            roomList = roomReservationDA.GetRoomReservationInfoForRegistration(IsAllActiveReservation);


            int count = roomList.Count;
            for (int i = 0; i < count; i++)
            {
                list.Add(new ListItem(
                                        roomList[i].ReservedCompany.ToString(),
                                        roomList[i].ReservationId.ToString()
                                         ));
            }
            return list;
        }
        [WebMethod(EnableSession = true)]
        public static string SaveGuestInformationAsDetail(string tempRegId, string IntOwner, string isEdit, string txtGuestName, string txtGuestEmail, string hiddenGuestId, string txtGuestDrivinlgLicense, string txtGuestDOB, string txtGuestAddress1, string txtGuestAddress2, string ddlProfessionId, string txtGuestCity, string ddlGuestCountry, string txtGuestNationality, string txtGuestPhone, string ddlGuestSex, string txtGuestZipCode, string txtNationalId, string txtPassportNumber, string txtPExpireDate, string txtPIssueDate, string txtPIssuePlace, string txtVExpireDate, string txtVisaNumber, string txtVIssueDate, string ReservationId, string guestDeletedDoc, string deletedGuestId, string selectedPreferenceId)
        {
            //GuestTemidPrint = "Paramater From Client:" + tempRegId;

            HMUtility hmUtility = new HMUtility();
            RoomRegistrationDA regDA = new RoomRegistrationDA();
            GuestInformationBO detailBO = new GuestInformationBO();
            detailBO.tempOwnerId = Int32.Parse(IntOwner);
            detailBO.GuestAddress1 = txtGuestAddress1;//
            detailBO.GuestAddress2 = txtGuestAddress2;//
            detailBO.GuestAuthentication = ""; // this.ddlGuestAuthentication.Text;
            detailBO.ProfessionId = Int32.Parse(ddlProfessionId);
            detailBO.GuestCity = txtGuestCity;//
            detailBO.GuestCountryId = Int32.Parse(ddlGuestCountry);//
            if (!string.IsNullOrWhiteSpace(txtGuestDOB))//
            {
                detailBO.GuestDOB = hmUtility.GetDateTimeFromString(txtGuestDOB, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.GuestDOB = null;
            }
            detailBO.GuestDrivinlgLicense = txtGuestDrivinlgLicense;//
            detailBO.GuestEmail = txtGuestEmail;//
            detailBO.GuestName = txtGuestName;//
            if (string.IsNullOrEmpty(hiddenGuestId))
            {
                detailBO.GuestId = 0;
                //   documentBO.GuestId = 0;
            }
            else
            {
                detailBO.GuestId = Int32.Parse(hiddenGuestId);//

                //GuestTemidPrint += ", Hidden GuestId:" + hiddenGuestId;
                // documentBO.GuestId = Int32.Parse(hiddenGuestId);
            }
            detailBO.GuestNationality = txtGuestNationality;
            detailBO.GuestPhone = txtGuestPhone;
            detailBO.GuestSex = ddlGuestSex;
            detailBO.GuestZipCode = txtGuestZipCode;
            detailBO.NationalId = txtNationalId;
            detailBO.PassportNumber = txtPassportNumber;
            if (!string.IsNullOrWhiteSpace(txtPExpireDate))//
            {
                detailBO.PExpireDate = hmUtility.GetDateTimeFromString(txtPExpireDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.PExpireDate = null;
            }
            if (!string.IsNullOrWhiteSpace(txtPIssueDate))//
            {
                detailBO.PIssueDate = hmUtility.GetDateTimeFromString(txtPIssueDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.PIssueDate = null;
            }
            detailBO.PIssuePlace = txtPIssuePlace;
            if (!string.IsNullOrWhiteSpace(txtVExpireDate))//
            {
                detailBO.VExpireDate = hmUtility.GetDateTimeFromString(txtVExpireDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.VExpireDate = null;
            }
            detailBO.VisaNumber = txtVisaNumber;
            if (!string.IsNullOrWhiteSpace(txtVIssueDate))//
            {
                detailBO.VIssueDate = hmUtility.GetDateTimeFromString(txtVIssueDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.VIssueDate = null;
            }
            //detailBO.GuestPreferences = gstPreferences;

            DateTime checkIndate = DateTime.Now;
            //  detailBO.Document = documentBO;

            List<GuestPreferenceMappingBO> preferenList = new List<GuestPreferenceMappingBO>();

            long prfId = 0;
            if (!string.IsNullOrEmpty(selectedPreferenceId))
            {
                string[] preferenceIds = selectedPreferenceId.Split(',');
                for (int i = 0; i < preferenceIds.Count(); i++)
                {
                    GuestPreferenceMappingBO preferenceBO = new GuestPreferenceMappingBO();
                    prfId = Convert.ToInt64(preferenceIds[i]);
                    preferenceBO.PreferenceId = prfId;
                    preferenList.Add(preferenceBO);
                }
            }

            if (string.IsNullOrEmpty(isEdit))
            {
                bool status = regDA.SaveTemporaryGuest(detailBO, tempRegId, checkIndate, 0, preferenList);
                if (status)
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestInformation.ToString(), detailBO.RegistrationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestInformation));
                //GuestTemidPrint += ", When Save GuestId:" + tempRegId;
            }
            else
            {
                detailBO.GuestId = Int32.Parse(isEdit);
                GuestInformationDA guestDA = new GuestInformationDA();
                Boolean status = guestDA.UpdateGuestInformation(detailBO, guestDeletedDoc, preferenList);
                //GuestTemidPrint += ", When Update GuestId:" + tempRegId;
                if(status)
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GuestInformation.ToString(), detailBO.RegistrationId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestInformation));
                if (Int32.Parse(ReservationId) != 0)
                {
                    Boolean success = guestDA.SaveGuestRegistrationInformation(Int32.Parse(tempRegId), detailBO.GuestId);
                    if (success)
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomRegistration.ToString(), Int32.Parse(tempRegId), ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomRegistration));
                }
            }
            // return GetUserDetailHtml(registrationDetailListBO);
            List<GuestInformationBO> guestInformationList = new List<GuestInformationBO>();
            GuestInformationDA guestInformationDA = new GuestInformationDA();

            //GuestTemidPrint += ", Before Get Guest Info GuestId:" + tempRegId;
            //GuestTemidPrint += ", Before Get Guest Info RegistrationId:" + RegistrationId;


            guestInformationList = guestInformationDA.GetGuestInformationDetailByRegiId(Convert.ToInt32(tempRegId));

            return GetUserDetailHtml(guestInformationList, deletedGuestId);

        }
        [WebMethod(EnableSession = true)]
        public static string PerformDeleteActionForGuestDetailByWM(int GuestId, int RegistrationId)
        {
            RoomRegistrationDA regDA = new RoomRegistrationDA();
            Boolean status = regDA.DeleteTempGuestRegistrationInfoByGuestId(RegistrationId, GuestId);
            List<GuestInformationBO> guestInformationList = new List<GuestInformationBO>();
            GuestInformationDA guestInformationDA = new GuestInformationDA();
            guestInformationList = guestInformationDA.GetGuestInformationDetailByRegiId(RegistrationId);
            return GetUserDetailHtml(guestInformationList, string.Empty);
        }
        [WebMethod(EnableSession = true)]
        public static GuestInformationViewBO PerformEditActionForGuestDetailByWM(int GuestId, int RegistrationId)
        {
            GuestInformationViewBO guestInfo = new GuestInformationViewBO();

            GuestInformationBO guestBO = new GuestInformationBO();
            GuestInformationDA guestDA = new GuestInformationDA();
            guestBO = guestDA.GetGuestInformationByGuestId(GuestId);

            List<DocumentsBO> docList = new List<DocumentsBO>();
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("Guest", GuestId);

            GuestPreferenceDA preferenceDA = new GuestPreferenceDA();
            List<GuestPreferenceBO> preferenceList = new List<GuestPreferenceBO>();
            preferenceList = preferenceDA.GetGuestPreferenceInfoByGuestId(GuestId);

            if (preferenceList.Count > 0)
            {
                foreach (GuestPreferenceBO preference in preferenceList)
                    if (guestInfo.GuestPreference != null)
                    {
                        guestInfo.GuestPreference += ", " + preference.PreferenceName;
                        guestInfo.GuestPreferenceId += ", " + preference.PreferenceId;
                    }
                    else
                    {
                        guestInfo.GuestPreference = preference.PreferenceName;
                        guestInfo.GuestPreferenceId = preference.PreferenceId.ToString();
                    }
            }

            string extension = ".txt, .doc, .docx, .pdf, .trf";

            foreach (DocumentsBO dc in docList)
            {
                if (!extension.Contains(dc.Extention))
                    dc.Path = (dc.Path + dc.Name);
                else
                    dc.Path = string.Empty;

                dc.Name = dc.Name.Remove(dc.Name.LastIndexOf('.'));
            }

            guestInfo.GuestInfo = guestBO;
            guestInfo.GuestDoc = docList;

            return guestInfo;
        }
        [WebMethod]
        public static string GetDocumentsByUserTypeAndUserId(string GuestId)
        {
            string UserType = "";
            int UserId = 0;
            List<DocumentsBO> docList = new List<DocumentsBO>();
            DocumentsDA docDA = new DocumentsDA();
            docList = docDA.GetDocumentsByUserTypeAndUserId("Guest", Int32.Parse(GuestId));

            string strTable = "";
            strTable += "<div style='color: White; background-color: #44545E;width:750px;'>";
            int counter = 0;
            foreach (DocumentsBO dr in docList)
            {
                string ImgSource = dr.Path + dr.Name;
                counter++;
                strTable += "<div style=' width:250px; height:250px; float:left;padding:30px'>";
                strTable += "<img id= style='width: 100px; height: 100px;' src='" + ImgSource + "'  alt='Image preview' />";
                strTable += "</div>";
            }
            strTable += "</div>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }

            return strTable;

        }
        [WebMethod]
        public static string GetTempRegistration()
        {
            List<GuestInformationBO> guestInformationList = new List<GuestInformationBO>();
            GuestInformationDA guestInformationDA = new GuestInformationDA();
            guestInformationList = guestInformationDA.GetGuestInformationDetailByRegiId(Convert.ToInt32(HttpContext.Current.Session["_RoomRegistrationId"]));
            return GetUserDetailHtml(guestInformationList, string.Empty);
        }
        [WebMethod]
        public static ArrayList LoadComplementaryItemByWM(int reservationId)
        {
            ArrayList list = new ArrayList();
            HMComplementaryItemBO comItemBO = new HMComplementaryItemBO();
            HMComplementaryItemDA comItemDA = new HMComplementaryItemDA();
            List<HMComplementaryItemBO> itemList = new List<HMComplementaryItemBO>();
            itemList = comItemDA.GetComplementaryItemInfoByReservationId(reservationId);

            foreach (HMComplementaryItemBO item in itemList)
            {
                list.Add(new ListItem(item.ComplementaryItemId.ToString(), item.ItemName.ToString()));
            }
            return list;
        }
        [WebMethod]
        public static ArrayList PopulateGuest(int ReservationId)
        {

            ArrayList list = new ArrayList();
            List<GuestInformationBO> guestList = new List<GuestInformationBO>();

            GuestInformationDA entityDA = new GuestInformationDA();
            guestList = entityDA.GetGuestInformationDetailByResId(Convert.ToInt32(ReservationId), false);
            int count = guestList.Count;
            for (int i = 0; i < count; i++)
            {
                list.Add(new ListItem(
                                        guestList[i].GuestName.ToString(),
                                        guestList[i].GuestId.ToString()
                                         ));
            }
            return list;

        }
        [WebMethod]
        public static string SetSelectedBussinessPromotionWM(int ReservationId)
        {
            RoomReservationDA reservationDA = new RoomReservationDA();
            RoomReservationBO reservationBO = reservationDA.GetRoomReservationInfoById(ReservationId);
            return reservationBO.BusinessPromotionId.ToString();

        }
        [WebMethod]
        public static RoomReservationBO GetRelatedDataByReservationId(int ReservationId)
        {
            RoomReservationDA reservationDA = new RoomReservationDA();
            RoomReservationBO reservationBO = reservationDA.GetRoomReservationInfoById(ReservationId);
            return reservationBO;
        }
        [WebMethod]
        public static GuestCompanyBO GetCalculatedDiscount(int companyId, int promId)
        {
            GuestCompanyBO companyBO = new GuestCompanyBO();
            GuestCompanyDA companyDA = new GuestCompanyDA();
            BusinessPromotionBO promBO = new BusinessPromotionBO();
            BusinessPromotionDA promDA = new BusinessPromotionDA();
            companyBO = companyDA.GetGuestCompanyInfoById(companyId);
            promBO = promDA.GetBusinessPromotionInfoById(promId);

            //decimal max = promBO.PercentAmount;
            if (promBO.PercentAmount > companyBO.DiscountPercent)
            {
                companyBO.DiscountPercent = promBO.PercentAmount;
            }
            //return max.ToString();

            return companyBO;
        }
        [WebMethod]
        public static GuestCompanyBO GetCompanyInformationByCompanyId(string companyId)
        {
            GuestCompanyBO companyBO = new GuestCompanyBO();
            GuestCompanyDA companyDA = new GuestCompanyDA();
            companyBO = companyDA.GetGuestCompanyInfoById(Int32.Parse(companyId));
            return companyBO;

        }
        [WebMethod]
        public static string GetGuestRegistrationHistoryGuestId(int GuestId)
        {
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            List<RoomRegistrationBO> registrationList = new List<RoomRegistrationBO>();
            registrationList = registrationDA.GetGuestRegistrationHistoryByGuestId(GuestId);

            string strTable = "";
            strTable += "<table  width='100%' class='table table-bordered table-condensed table-responsive' id='TableGuestHistory'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";


            strTable += "<th align='center' scope='col'>Registration Number</th><th align='left' scope='col'>Arrival Date</th> <th align='left' scope='col'>Checkout Date</th></tr>";
            int counter = 0;
            foreach (RoomRegistrationBO dr in registrationList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:White;'>";
                }
                strTable += "<td align='left' style='width: 33%'>" + dr.RegistrationNumber + "</td>";
                strTable += "<td align='left' style='width: 33%'>" + dr.ArriveDate.ToString("MM/dd/yy") + "</td>";
                if (dr.CheckOutDate != DateTime.MinValue)
                {
                    strTable += "<td align='left' style='width: 33%'>" + dr.CheckOutDate.ToString("MM/dd/yy") + "</td>";
                }
                else
                {
                    strTable += "<td align='left' style='width: 33%'>" + "Not CheckOut Yet. " + "</td>";
                }
                strTable += "</tr>";
            }


            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            return strTable;
        }
        [WebMethod(EnableSession = true)]
        public static string GetCalculatedGrandTotal(string Currency)
        {
            var dataSource = HttpContext.Current.Session["PMSalesDetailList"] as List<PMSalesDetailBO>;
            decimal salesAmount = GetGrandTotal(dataSource, Currency);
            return salesAmount.ToString();
        }
        [WebMethod(EnableSession = true)]
        public static string PerformSaveGuestPaymentDetailsInformationByWebMethod(bool isEdit, string ddlCurrency, string ddlCurrencyType, string txtConversionRate, string ddlPaymentType, string ddlPayMode, string txtReceiveLeadgerAmount, string ddlCashPaymentAccountHead, string ddlBankId, string txtCardNumber, string ddlCardType, string txtExpireDate, string txtCardHolderName, string txtChecqueNumber, string ddlCardReceiveAccountsInfo, string ddlCompanyPaymentAccountHead, string paymentDescription)
        {
            int dynamicDetailId = 0;
            List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

            if (guestPaymentDetailListForGrid != null)
            {
                dynamicDetailId = guestPaymentDetailListForGrid.Count + 1;
            }

            GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();

            guestBillPaymentBO.PaymentType = ddlPaymentType;
            //guestBillPaymentBO.FieldId = 45;
            //txtConversionRate = !string.IsNullOrWhiteSpace(txtConversionRate) ? txtConversionRate : "1";
            //txtReceiveLeadgerAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? txtReceiveLeadgerAmount : "0";

            if (ddlCurrencyType == "Local")
            {
                guestBillPaymentBO.FieldId = !string.IsNullOrWhiteSpace(ddlCurrency) ? Convert.ToInt32(ddlCurrency) : 1;
                guestBillPaymentBO.ConvertionRate = 1;
                guestBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
                guestBillPaymentBO.PaymentAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            }
            else
            {
                guestBillPaymentBO.FieldId = !string.IsNullOrWhiteSpace(ddlCurrency) ? Convert.ToInt32(ddlCurrency) : 1;
                guestBillPaymentBO.ConvertionRate = !string.IsNullOrWhiteSpace(txtConversionRate) ? Convert.ToDecimal(txtConversionRate) : 1;
                guestBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
                guestBillPaymentBO.PaymentAmount = guestBillPaymentBO.CurrencyAmount * guestBillPaymentBO.ConvertionRate;
            }

            //Boolean validAmount = true;

            //decimal conversionRate;
            //if (decimal.TryParse(txtConversionRate, out conversionRate))
            //{
            //    // it's a valid integer => you could use the distance variable here
            //    decimal receiveLeadgerAmount;
            //    if (decimal.TryParse(txtReceiveLeadgerAmount, out receiveLeadgerAmount))
            //    {
            //        // it's a valid integer => you could use the distance variable here
            //    }
            //    else
            //    {
            //        validAmount = false;
            //    }
            //}
            //else
            //{
            //    validAmount = false;
            //}

            //if (validAmount == true)
            //{
            //guestBillPaymentBO.ConvertionRate = !string.IsNullOrWhiteSpace(txtConversionRate) ? Convert.ToDecimal(txtConversionRate) : 1;
            //decimal tmpCurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            //guestBillPaymentBO.CurrencyAmount = tmpCurrencyAmount * guestBillPaymentBO.ConvertionRate;
            //guestBillPaymentBO.PaymentAmount = tmpCurrencyAmount * guestBillPaymentBO.ConvertionRate;
            guestBillPaymentBO.ChecqueDate = DateTime.Now;
            guestBillPaymentBO.PaymentMode = ddlPayMode;
            guestBillPaymentBO.PaymentId = dynamicDetailId;
            guestBillPaymentBO.BankId = !string.IsNullOrWhiteSpace(ddlBankId) ? Convert.ToInt32(ddlBankId) : 0;
            guestBillPaymentBO.CardNumber = txtCardNumber;
            guestBillPaymentBO.CardType = ddlCardType;
            guestBillPaymentBO.ChecqueNumber = txtChecqueNumber;
            guestBillPaymentBO.PaymentDescription = paymentDescription;
            HMUtility hmUtility = new HMUtility();
            if (string.IsNullOrEmpty(txtExpireDate))
            {
                guestBillPaymentBO.ExpireDate = null;
            }
            else
            {
                guestBillPaymentBO.ExpireDate = hmUtility.GetDateTimeFromString(txtExpireDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            guestBillPaymentBO.CardHolderName = txtCardHolderName;

            if (ddlPaymentType == "Advance")
            {

                if (ddlPayMode == "Cash")
                {
                    guestBillPaymentBO.AccountsPostingHeadId = Int32.Parse(ddlCashPaymentAccountHead);
                }
                else if (ddlPayMode == "Card")
                {
                    guestBillPaymentBO.AccountsPostingHeadId = Int32.Parse(ddlCardReceiveAccountsInfo);
                }
                else if (ddlPayMode == "Cheque")
                {
                    //guestBillPaymentBO.AccountsPostingHeadId = Int32.Parse(ddlAccountsPostingHeadId);
                }
            }
            else if (ddlPaymentType == "PaidOut")
            {
                guestBillPaymentBO.AccountsPostingHeadId = Int32.Parse(ddlCashPaymentAccountHead);
            }

            guestPaymentDetailListForGrid.Add(guestBillPaymentBO);
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;

            //}
            return LoadGuestPaymentDetailGridViewByWM();
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

            return LoadGuestPaymentDetailGridViewByWM();
        }
        [WebMethod(EnableSession = true)]
        public static string PerformGetTotalPaidAmountByWebMethod()
        {
            decimal sum = 0;
            var List = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            if (List != null)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    if (List[i].PaymentType == "Advance")
                    {
                        sum = sum + Convert.ToDecimal(List[i].PaymentAmount);
                    }
                    else
                    {
                        sum = sum - Convert.ToDecimal(List[i].PaymentAmount);
                    }
                }
            }
            return sum.ToString();
        }
        [WebMethod]
        public static SearchDetailsInfoBO GuestDetailsSearchForAutoPopup(string guestName, string guestDOB, string guestEmail, string guestPhone, string guestCountry, string nationalId, string passportNumber)
        {
            GuestInformationDA ginfoDA = new GuestInformationDA();
            GuestInformationBO guestInfo = new GuestInformationBO();
            SearchDetailsInfoBO searchDetails = new SearchDetailsInfoBO();

            HMUtility hmUtility = new HMUtility();
            guestDOB = hmUtility.GetUnivarsalDateTimeFromString(guestDOB, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            guestInfo = ginfoDA.GetGuestInformationBySearchCriteriaForAutoPopup(guestName, guestDOB, guestEmail, guestPhone, guestCountry, nationalId, passportNumber);

            if (guestInfo.GuestId != 0)
            {
                string gdoc = GetDocumentsByUserTypeAndUserId(guestInfo.GuestId.ToString());
                string ghistory = GetGuestRegistrationHistoryGuestId(guestInfo.GuestId);

                searchDetails.GuestInfo = guestInfo;
                searchDetails.GuestDocuments = gdoc;
                searchDetails.GuestRegistrationHistory = ghistory;
            }
            else
            {
                searchDetails.GuestInfo = null;
                searchDetails.GuestDocuments = null;
                searchDetails.GuestRegistrationHistory = null;
            }

            return searchDetails;
        }
        [WebMethod]
        public static RoomRegistrationBO GetRackRateServiceChargeVatInformation(string negotiatedRoomRate, string isServiceChargeEnable, string isVatEnable)
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            RoomRegistrationBO roomRegistrationBO = roomRegistrationDA.GetRackRateServiceChargeVatInformation(negotiatedRoomRate, isServiceChargeEnable, isVatEnable);
            return roomRegistrationBO;
        }
        [WebMethod]
        public static PaidServiceViewBO GetPaidServiceDetails(int registrationId, int currencyId, string currencyType, string convertionRate, int isCeomplementaryService)
        {
            HotelGuestServiceInfoDA serviceDa = new HotelGuestServiceInfoDA();
            List<HotelGuestServiceInfoBO> paidServiceLst = new List<HotelGuestServiceInfoBO>();
            List<RegistrationServiceInfoBO> registrationPaidService = new List<RegistrationServiceInfoBO>();

            paidServiceLst = serviceDa.GetHotelGuestServiceInfo(1, 1, 1);

            if (registrationId != 0)
            {
                registrationPaidService = serviceDa.GetRegistrationServiceInfoByRegistrationId(registrationId, currencyType);

                if (registrationPaidService.Count > 0)
                {
                    var crType = registrationPaidService.Where(c => c.CurrencyType != 0).FirstOrDefault();

                    if (crType.CurrencyType != Convert.ToInt32(currencyId))
                    {
                        if (currencyType != "Local")
                        {
                            registrationPaidService = (from d in registrationPaidService
                                                       from p in paidServiceLst
                                                       where d.ServiceId == p.ServiceId
                                                       select new RegistrationServiceInfoBO
                                                       {
                                                           DetailServiceId = d.DetailServiceId,
                                                           RegistrationId = d.RegistrationId,
                                                           ServiceId = d.ServiceId,
                                                           ServiceName = d.ServiceName,
                                                           ServiceRate = d.ServiceRate,
                                                           UnitPrice = p.UnitPriceUsd,
                                                           ConversionRate = d.ConversionRate,
                                                           IsAchieved = d.IsAchieved

                                                       }).ToList();
                        }
                        else
                        {
                            registrationPaidService = (from d in registrationPaidService
                                                       from p in paidServiceLst
                                                       where d.ServiceId == p.ServiceId
                                                       select new RegistrationServiceInfoBO
                                                       {
                                                           DetailServiceId = d.DetailServiceId,
                                                           RegistrationId = d.RegistrationId,
                                                           ServiceId = d.ServiceId,
                                                           ServiceName = d.ServiceName,
                                                           ServiceRate = d.ServiceRate,
                                                           UnitPrice = p.UnitPriceLocal,
                                                           ConversionRate = d.ConversionRate,
                                                           IsAchieved = d.IsAchieved

                                                       }).ToList();
                        }
                    }
                    else if (crType.CurrencyType == Convert.ToInt32(currencyId))
                    {

                    }
                }
            }

            if (isCeomplementaryService == 0)
            {
                foreach (RegistrationServiceInfoBO rps in registrationPaidService)
                {
                    if (rps.UnitPrice == 0)
                    {
                        var c = (from s in paidServiceLst
                                 where s.ServiceId == rps.ServiceId
                                 select s).FirstOrDefault();

                        if (c != null)
                        {
                            if (currencyType != "Local")
                            {
                                rps.UnitPrice = c.UnitPriceUsd;
                            }
                            else
                            {
                                rps.UnitPrice = c.UnitPriceLocal;
                            }
                        }
                    }
                }
            }

            PaidServiceViewBO viewBo = new PaidServiceViewBO();
            viewBo.PaidService = paidServiceLst;
            viewBo.RegistrationPaidService = registrationPaidService;

            return viewBo;
        }
        [WebMethod]
        public static PaidServiceViewBO GetPaidServiceDetailsFromReservation(int reservationId, int currencyId, string currencyType, string convertionRate)
        {
            HotelGuestServiceInfoDA serviceDa = new HotelGuestServiceInfoDA();
            List<HotelGuestServiceInfoBO> paidServiceLst = new List<HotelGuestServiceInfoBO>();
            List<RegistrationServiceInfoBO> reservationPaidService = new List<RegistrationServiceInfoBO>();

            paidServiceLst = serviceDa.GetHotelGuestServiceInfo(0, 1, 1);

            if (reservationId != 0)
            {
                reservationPaidService = serviceDa.GetReservationServiceInfoByReservationId(reservationId, currencyType);

                if (reservationPaidService.Count > 0)
                {
                    var crType = reservationPaidService.Where(c => c.CurrencyType != 0).FirstOrDefault();

                    if (crType.CurrencyType != Convert.ToInt32(currencyId))
                    {
                        if (currencyType != "Local")
                        {
                            reservationPaidService = (from d in reservationPaidService
                                                      from p in paidServiceLst
                                                      where d.ServiceId == p.ServiceId
                                                      select new RegistrationServiceInfoBO
                                                      {
                                                          DetailServiceId = d.DetailServiceId,
                                                          RegistrationId = d.RegistrationId,
                                                          ServiceId = d.ServiceId,
                                                          ServiceName = d.ServiceName,
                                                          ServiceRate = d.ServiceRate,
                                                          UnitPrice = p.UnitPriceUsd,
                                                          ConversionRate = d.ConversionRate,
                                                          IsAchieved = d.IsAchieved

                                                      }).ToList();
                        }
                        else
                        {
                            reservationPaidService = (from d in reservationPaidService
                                                      from p in paidServiceLst
                                                      where d.ServiceId == p.ServiceId
                                                      select new RegistrationServiceInfoBO
                                                      {
                                                          DetailServiceId = d.DetailServiceId,
                                                          RegistrationId = d.RegistrationId,
                                                          ServiceId = d.ServiceId,
                                                          ServiceName = d.ServiceName,
                                                          ServiceRate = d.ServiceRate,
                                                          UnitPrice = p.UnitPriceLocal,
                                                          ConversionRate = d.ConversionRate,
                                                          IsAchieved = d.IsAchieved

                                                      }).ToList();
                        }
                    }
                    else if (crType.CurrencyType == Convert.ToInt32(currencyId))
                    {
                        //if (currencyType != "45")
                        //{
                        //    registrationPaidService = (from d in registrationPaidService
                        //                               from p in paidServiceLst
                        //                               where d.PaidServiceId == p.PaidServiceId
                        //                               select new RegistrationServiceInfoBO
                        //                               {
                        //                                   DetailPaidServiceId = d.DetailPaidServiceId,
                        //                                   RegistrationId = d.RegistrationId,
                        //                                   PaidServiceId = d.PaidServiceId,
                        //                                   ServiceName = d.ServiceName,
                        //                                   PaidServicePrice = d.PaidServicePrice,
                        //                                   UnitPrice = p.UnitPriceUsd,
                        //                                   ConversionRate = d.ConversionRate,
                        //                                   IsAchieved = d.IsAchieved

                        //                               }).ToList();
                        //}
                        //else
                        //{
                        //    registrationPaidService = (from d in registrationPaidService
                        //                               from p in paidServiceLst
                        //                               where d.PaidServiceId == p.PaidServiceId
                        //                               select new RegistrationServiceInfoBO
                        //                               {
                        //                                   DetailPaidServiceId = d.DetailPaidServiceId,
                        //                                   RegistrationId = d.RegistrationId,
                        //                                   PaidServiceId = d.PaidServiceId,
                        //                                   ServiceName = d.ServiceName,
                        //                                   PaidServicePrice = d.PaidServicePrice,
                        //                                   UnitPrice = p.UnitPriceLocal,
                        //                                   ConversionRate = d.ConversionRate,
                        //                                   IsAchieved = d.IsAchieved

                        //                               }).ToList();
                        //}
                    }

                    //if (currencyType != "45")
                    //{
                    //    var v = registrationPaidService.Where(c => c.ConversionRate != 0).FirstOrDefault();

                    //    if (v == null)
                    //    {
                    //        registrationPaidService = (from d in registrationPaidService
                    //                                   from p in paidServiceLst
                    //                                   where d.PaidServiceId == p.PaidServiceId
                    //                                   select new RegistrationServiceInfoBO
                    //                                   {
                    //                                       DetailPaidServiceId = d.DetailPaidServiceId,
                    //                                       RegistrationId = d.RegistrationId,
                    //                                       PaidServiceId = d.PaidServiceId,
                    //                                       ServiceName = d.ServiceName,
                    //                                       PaidServicePrice = d.PaidServicePrice,
                    //                                       UnitPrice = p.UnitPriceUsd,
                    //                                       ConversionRate = d.ConversionRate,
                    //                                       IsAchieved = d.IsAchieved

                    //                                   }).ToList();
                    //    }
                    //}
                    //else
                    //{
                    //    if (!string.IsNullOrEmpty(convertionRate) && currencyType != "45")
                    //    {
                    //        registrationPaidService = (from d in registrationPaidService
                    //                                   from p in paidServiceLst
                    //                                   where d.PaidServiceId == p.PaidServiceId
                    //                                   select new RegistrationServiceInfoBO
                    //                                   {
                    //                                       DetailPaidServiceId = d.DetailPaidServiceId,
                    //                                       RegistrationId = d.RegistrationId,
                    //                                       PaidServiceId = d.PaidServiceId,
                    //                                       ServiceName = d.ServiceName,
                    //                                       PaidServicePrice = d.PaidServicePrice,
                    //                                       UnitPrice = p.UnitPriceUsd, // (d.UnitPrice / Convert.ToDecimal(convertionRate)),
                    //                                       ConversionRate = d.ConversionRate,
                    //                                       IsAchieved = d.IsAchieved

                    //                                   }).ToList();
                    //    }
                    //    else
                    //    {

                    //    }
                    //}
                }
            }
            PaidServiceViewBO viewBo = new PaidServiceViewBO();
            viewBo.PaidService = paidServiceLst;
            viewBo.RegistrationPaidService = reservationPaidService;

            return viewBo;
        }
        [WebMethod]
        public static string GetNationality(int countryId)
        {
            CountriesBO country = new CountriesBO();

            try
            {
                HMCommonDA commonDa = new HMCommonDA();
                country = commonDa.GetCountriesById(countryId);

            }
            catch (Exception ex)
            {
                country.Nationality = string.Empty;
            }

            return country.Nationality;
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
        public static HMCommonSetupBO LoadReservationNRegistrationXtraValidation()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("ReservationNRegistrationXtraValidation", "ReservationNRegistrationXtraValidation");
            return commonSetupBO;
        }
        [WebMethod]
        public static string LoadGuestReferenceInfo()
        {
            List<GuestPreferenceBO> guestReferenceList = new List<GuestPreferenceBO>();
            GuestPreferenceDA guestReferenceDA = new GuestPreferenceDA();

            string HTML = string.Empty;

            guestReferenceList = guestReferenceDA.GetActiveGuestPreferenceInfo();
            HTML = GetHTMLGuestReferenceGridView(guestReferenceList);

            return HTML;
        }
        public static string GetHTMLGuestReferenceGridView(List<GuestPreferenceBO> List)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='GuestReferenceInformation' width='100%' border: '1px solid #cccccc'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='center' scope='col'>Select</th><th align='left' scope='col'>Preference</th></tr>";
            //strTable += "<tr> <td colspan='2'>";
            //strTable += "<div style=\"height: 375px; overflow-y: scroll; text-align: left;\">";
            //strTable += "<table cellspacing='0' cellpadding='4' width='100%' id='GuestReference' >";
            int counter = 0;
            foreach (GuestPreferenceBO dr in List)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:White;'>";
                }

                strTable += "<td style='display:none;'>" + dr.PreferenceId + "</td>";
                strTable += "<td align='center' style='width: 20px'>";
                strTable += "&nbsp;<input type='checkbox'  id='" + dr.PreferenceId + "' name='" + dr.PreferenceName + "' value='" + dr.PreferenceId + "' >";
                strTable += "</td><td align='left' style='width: 138px'>" + dr.PreferenceName + "</td></tr>";
            }

            //strTable += "</table> </div> </td> </tr> </table>";
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Preference Available !</td></tr>";
            }
            
            strTable += "<div style='margin-top:12px;'>";
            strTable += "   <button type='button' onClick='javascript:return GetCheckedGuestPreference()' id='btnAddRoomId' class='btn btn-primary'> OK</button>";
            strTable += "   <button type='button' onclick='javascript:return ClosePreferenceDialog()' id='btnAddRoomId' class='btn btn-primary'> Cancel</button>";
            strTable += "</div>";
            return strTable;
        }
        [WebMethod]
        public static string LoadGuestPreferences(int guestId)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            GuestPreferenceDA guestPrefDA = new GuestPreferenceDA();
            List<GuestPreferenceBO> gstPrefList = new List<GuestPreferenceBO>();

            gstPrefList = guestPrefDA.GetGuestPreferenceInfoByGuestId(guestId);
            return hmCommonDA.GetPreferenceListView(gstPrefList);
        }
    }
}