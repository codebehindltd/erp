using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using HotelManagement.Entity.Banquet;
using HotelManagement.Data.Banquet;
using System.Web.Services;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Text.RegularExpressions;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Inventory;
using Newtonsoft.Json;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Presentation.Website.Banquet
{
    public partial class frmBanquetReservationNew : BasePage
    {
        HiddenField innboardMessage;
        protected int _RoomReservationId;
        protected int _proId = -1;
        ArrayList arrayDelete;
        protected int _reservationId;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        private Boolean isBanquetRateEditableEnable = false;
        //**************************** Handlers ****************************//  
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            hfMinCheckInDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.Date);
            _RoomReservationId = 0;
            this.AddEditODeleteDetail();

            //cbServiceCharge.Enabled = false;
            //cbVatAmount.Enabled = false;

            if (!IsPostBack)
            {
                Session["ReservationDetailList"] = null;
                this.LoadBanquetInfo();
                this.LoadCountry();
                this.LoadSeatingPlan();
                this.LoadOccassion();
                this.LoadAffiliatedCompany();
                this.LoadCommonDropDownHiddenField();
                this.LoadIsBanquetRateEditableEnable();
                this.LoadSearchBanquetInfo();
                this.LoadRefferenceName();
                this.SetDefaulTime();
                this.LoadCommonSetupForRackRateServiceChargeVatInformation();
                this.hfDuplicateReservarionValidation.Value = "0";
                string editId = Request.QueryString["editId"];
                if (!string.IsNullOrWhiteSpace(editId))
                {
                    int Id = Convert.ToInt32(editId);
                    if (Id > 0)
                    {
                        FillForm(Id);
                    }
                }

                this.LoadGrandTotalLabelChange();
                CheckPermission();
            }

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Cancel();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFormValid())
                {
                    return;
                }

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                BanquetReservationBO reservationBO = new BanquetReservationBO();
                BanquetReservationDA reservationDA = new BanquetReservationDA();
                //Store Data
                if (chkIsReturnedGuest.Checked == true)
                {
                    reservationBO.IsReturnedClient = true;
                }
                else
                {
                    reservationBO.IsReturnedClient = false;
                }
                reservationBO.RefferenceId = Int32.Parse(ddlRefferenceId.SelectedValue);
                reservationBO.BanquetId = Int32.Parse(ddlBanquetId.SelectedValue);
                reservationBO.ReservationMode = this.ddlReservationMode.SelectedValue.ToString();
                reservationBO.Address = this.txtAddress.Text;
                reservationBO.ZipCode = null;
                reservationBO.CountryId = Int32.Parse(ddlCountryId.SelectedValue);
                reservationBO.PhoneNumber = txtPhoneNumber.Text;
                reservationBO.BookingFor = "";
                if (ddlReservationMode.SelectedValue == "Company")
                {
                    if (chkIsLitedCompany.Checked)
                    {
                        reservationBO.IsListedCompany = true;
                        reservationBO.Name = ddlCompanyId.SelectedItem.Text;
                        reservationBO.ContactPerson = txtContactPerson.Text;
                        reservationBO.CompanyId = Convert.ToInt32(ddlCompanyId.SelectedValue);
                        if (reservationBO.CompanyId == 0)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Please provide an enlisted company.", AlertType.Warning);
                            return;
                        }
                    }
                    else
                    {
                        reservationBO.IsListedCompany = false;
                        reservationBO.Name = txtName.Text;
                        reservationBO.ContactPerson = txtContactPerson.Text;
                        reservationBO.CompanyId = 0;
                    }
                }
                else
                {
                    reservationBO.IsListedCompany = false;
                    reservationBO.Name = txtName.Text;
                    reservationBO.ContactPerson = txtName.Text;
                    reservationBO.CompanyId = 0;
                }

                reservationBO.ContactPhone = txtContactPhone.Text;
                reservationBO.ContactEmail = txtContactEmail.Text;
                string arriveTime = txtProbableArrivalHour.Text;
                int arrhour = Convert.ToInt32(arriveTime);
                int arrmin = Convert.ToInt32(0);
                reservationBO.ArriveDate = hmUtility.GetDateTimeFromString(this.txtPartyDate.Text, userInformationBO.ServerDateFormat).AddHours(arrhour).AddMinutes(arrmin);

                string deparTime = txtProbableDepartureHour.Text;
                int dephour = Convert.ToInt32(deparTime);
                int depmin = Convert.ToInt32(0);
                reservationBO.DepartureDate = hmUtility.GetDateTimeFromString(this.txtPartyDate.Text, userInformationBO.ServerDateFormat).AddHours(dephour).AddMinutes(depmin);
                reservationBO.NumberOfPersonAdult = !string.IsNullOrWhiteSpace(this.txtNumberOfPersonAdult.Text) ? Convert.ToInt32(this.txtNumberOfPersonAdult.Text) : 0;
                reservationBO.NumberOfPersonChild = !string.IsNullOrWhiteSpace(this.txtNumberOfPersonChild.Text) ? Convert.ToInt32(this.txtNumberOfPersonChild.Text) : 0;

                reservationBO.OccessionTypeId = Int32.Parse(ddlOccessionTypeId.SelectedValue);
                reservationBO.SeatingId = Int32.Parse(ddlSeatingId.SelectedValue);
                this.LoadIsBanquetRateEditableEnable();
                if (isBanquetRateEditableEnable)
                {
                    reservationBO.BanquetRate = !string.IsNullOrWhiteSpace(this.txtBanquetRate.Text) ? Convert.ToDecimal(this.txtBanquetRate.Text) : 0;
                }
                else
                {
                    reservationBO.BanquetRate = !string.IsNullOrWhiteSpace(this.hfBanquetRate.Value) ? Convert.ToDecimal(this.hfBanquetRate.Value) : 0;
                }

                reservationBO.TotalAmount = !string.IsNullOrWhiteSpace(this.hfTotalAmount.Value) ? Convert.ToDecimal(this.hfTotalAmount.Value) : 0;
                reservationBO.DiscountType = this.ddlDiscountType.SelectedValue.ToString();
                reservationBO.DiscountAmount = !string.IsNullOrWhiteSpace(this.txtDiscountAmount.Text) ? Convert.ToDecimal(this.txtDiscountAmount.Text) : 0;
                reservationBO.DiscountedAmount = !string.IsNullOrWhiteSpace(this.hfDiscountedAmount.Value) ? Convert.ToDecimal(this.hfDiscountedAmount.Value) : 0;
                reservationBO.Remarks = txtRemarks.Text;

                if (this.cbServiceCharge.Checked)
                {
                    reservationBO.IsInvoiceServiceChargeEnable = true;
                }
                else
                {
                    reservationBO.IsInvoiceServiceChargeEnable = false;
                }

                if (this.cbVatAmount.Checked)
                {
                    reservationBO.IsInvoiceVatAmountEnable = true;
                }
                else
                {
                    reservationBO.IsInvoiceVatAmountEnable = false;
                }

                // -------------------------------------------------------------------------------------------------------------------------------------------
                List<BanquetReservationDetailBO> addList = new List<BanquetReservationDetailBO>();
                List<BanquetReservationDetailBO> deleteList = new List<BanquetReservationDetailBO>();
                List<BanquetReservationClassificationDiscountBO> discountLst = new List<BanquetReservationClassificationDiscountBO>();
                List<BanquetReservationClassificationDiscountBO> discountDeletedLst = new List<BanquetReservationClassificationDiscountBO>();

                addList = JsonConvert.DeserializeObject<List<BanquetReservationDetailBO>>(hfSaveObj.Value);
                deleteList = JsonConvert.DeserializeObject<List<BanquetReservationDetailBO>>(hfDeleteObj.Value);
                discountLst = JsonConvert.DeserializeObject<List<BanquetReservationClassificationDiscountBO>>(hfClassificationDiscountSave.Value);
                // -------------------------------------------------------------------------------------------------------------------------------------------

                reservationBO.InvoiceServiceCharge = !string.IsNullOrWhiteSpace(this.hfServiceCharge.Value) ? Convert.ToDecimal(this.hfServiceCharge.Value) : 0;
                reservationBO.InvoiceVatAmount = !string.IsNullOrWhiteSpace(this.hfVatAmount.Value) ? Convert.ToDecimal(this.hfVatAmount.Value) : 0;

                int costCenterId = !string.IsNullOrWhiteSpace(this.hfCostCenterId.Value) ? Convert.ToInt32(this.hfCostCenterId.Value) : 0;
                if (costCenterId > 0)
                {
                    reservationBO.CostCenterId = costCenterId;
                    if (this.btnSave.Text.Equals("Save"))
                    {
                        long tmpSalesId = 0;
                        reservationBO.CreatedBy = userInformationBO.UserInfoId;
                        bool status = reservationDA.SaveBanquetReservationInfo(reservationBO, addList, discountLst, out tmpSalesId);

                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.BanquetReservation.ToString(), tmpSalesId,
                            ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetReservation));
                        }
                    }
                    else
                    {
                        discountDeletedLst = JsonConvert.DeserializeObject<List<BanquetReservationClassificationDiscountBO>>(hfClassificationDiscountDelete.Value);
                        reservationBO.Id = Convert.ToInt64(Session["_reservationId"]);
                        reservationBO.LastModifiedBy = userInformationBO.UserInfoId;
                        Boolean status = reservationDA.UpdateBanquetReservationInfo(reservationBO, addList, null , deleteList, Session["arrayDelete"] as ArrayList, discountLst, discountDeletedLst);

                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.BanquetReservation.ToString(), reservationBO.Id,
                                ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetReservation));
                        }
                    }
                    Cancel();
                    SetTab("Entry");
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, "Banquet and Cost Center Mapping Related Configuration Missing.", AlertType.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        //************************ User Defined Function ********************//
        private void LoadGrandTotalLabelChange()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetBillInclusive", "IsBanquetBillInclusive");
            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        this.lblGrandTotal.Text = "Grand Total";
                    }
                    else
                    {
                        this.lblGrandTotal.Text = "Net Amount";
                    }
                }
                else
                {
                    this.lblGrandTotal.Text = "Grand Total";
                }
            }
        }
        private void LoadIsBanquetRateEditableEnable()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetRateEditableEnable", "IsBanquetRateEditableEnable");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        this.txtBanquetRate.Enabled = true;
                        isBanquetRateEditableEnable = true;
                    }
                    else
                    {
                        this.txtBanquetRate.Enabled = false;
                        isBanquetRateEditableEnable = false;
                    }
                }
            }
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
        private void LoadCommonSetupForRackRateServiceChargeVatInformation()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO commonSetupBO;

            commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetBillInclusive", "IsBanquetBillInclusive");
            hfIsBanquetBillInclusive.Value = commonSetupBO.SetupValue;

            commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("BanquetVatAmount", "BanquetVatAmount");
            hfBanquetVatAmount.Value = commonSetupBO.SetupValue;

            commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("BanquetServiceCharge", "BanquetServiceCharge");
            hfBanquetServiceCharge.Value = commonSetupBO.SetupValue;

            commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("CompanyCountryId", "CompanyCountryId");
            //hfGuestHouseServiceCharge.Value = commonSetupBO.SetupValue;
            ddlCountryId.SelectedValue = commonSetupBO.SetupValue;

            commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetBillAmountWillRound", "IsBanquetBillAmountWillRound");
            hfIsBanquetBillAmountWillRound.Value = commonSetupBO.SetupValue;
        }
        public static void OpenNewBrowserWindow(string Url, Control control)
        {
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Open", "window.open('" + Url + "');", true);
        }
        private void DeleteData(int _reservationId)
        {
            BanquetReservationDA reservationDA = new BanquetReservationDA();
            try
            {
                Boolean statusApproved = reservationDA.DeleteBanquetReservationInfoById(_reservationId);
                if (statusApproved)
                {
                    //this.isMessageBoxEnable = 2;
                    //lblMessage.Text = "Delete Operation Successfull";
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.BanquetReservation.ToString(), _reservationId,
                        ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetReservation));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    //this.LoadGridView();
                    this.Cancel();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        private void FillForm(int ReservationId)
        {
            //Master Information------------------------
            BanquetReservationBO reservationBO = new BanquetReservationBO();
            BanquetReservationDA reservationDA = new BanquetReservationDA();
            reservationBO = reservationDA.GetBanquetReservationInfoById(ReservationId);
            Session["_reservationId"] = reservationBO.Id;
            hfReservationId.Value = reservationBO.Id.ToString();
            hfBanquetId.Value = reservationBO.BanquetId.ToString();
            txtPartyDate.Text = hmUtility.GetStringFromDateTime(reservationBO.ArriveDate);
            this.txtProbableArrivalHour.Text = hmUtility.GetHourFromDateTime(reservationBO.ArriveDate); //.ToString("HH");
            this.txtProbableDepartureHour.Text = hmUtility.GetHourFromDateTime(reservationBO.DepartureDate); //.ToString("HH");
            hfCostCenterId.Value = reservationBO.CostCenterId.ToString();
            txtAddress.Text = reservationBO.Address;
            txtRemarks.Text = reservationBO.Remarks;
            txtContactEmail.Text = reservationBO.ContactEmail;
            txtContactPerson.Text = reservationBO.ContactPerson;
            txtContactPhone.Text = reservationBO.ContactPhone;
            ddlDiscountType.SelectedValue = reservationBO.DiscountType;
            txtDiscountAmount.Text = reservationBO.DiscountAmount.ToString();
            txtDiscountedAmount.Text = reservationBO.DiscountedAmount.ToString();
            hfDiscountedAmount.Value = reservationBO.DiscountedAmount.ToString();
            txtName.Text = reservationBO.Name;
            ddlCountryId.SelectedValue = reservationBO.CountryId.ToString();
            ddlBanquetId.SelectedValue = reservationBO.BanquetId.ToString();
            ddlReservationMode.SelectedValue = reservationBO.ReservationMode.ToString();
            if (reservationBO.IsListedCompany == true)
            {
                chkIsLitedCompany.Checked = true;
            }
            else
            {
                chkIsLitedCompany.Checked = false;
            }
            ddlCompanyId.SelectedValue = reservationBO.CompanyId.ToString();
            ddlOccessionTypeId.SelectedValue = reservationBO.OccessionTypeId.ToString();
            ddlSeatingId.SelectedValue = reservationBO.SeatingId.ToString();
            ddlBanquetId.SelectedValue = reservationBO.BanquetId.ToString();
            txtNumberOfPersonAdult.Text = reservationBO.NumberOfPersonAdult.ToString();
            txtNumberOfPersonChild.Text = reservationBO.NumberOfPersonChild.ToString();
            txtPhoneNumber.Text = reservationBO.PhoneNumber;
            txtReservationId.Value = reservationBO.Id.ToString();
            ddlRefferenceId.SelectedValue = reservationBO.RefferenceId.ToString();

            if (reservationBO.IsReturnedClient == true)
            {
                chkIsReturnedGuest.Checked = true;
            }
            else
            {
                chkIsReturnedGuest.Checked = false;
            }

            txtTotalAmount.Text = reservationBO.TotalAmount.ToString();
            hfTotalAmount.Value = reservationBO.TotalAmount.ToString();
            this.cbServiceCharge.Checked = reservationBO.IsInvoiceServiceChargeEnable;
            this.cbVatAmount.Checked = reservationBO.IsInvoiceVatAmountEnable;
            txtBanquetRate.Text = reservationBO.BanquetRate.ToString();
            hfBanquetRate.Value = reservationBO.BanquetRate.ToString();
            this.btnSave.Text = "Update";

            //Detail Information------------------------
            //ltlTableWiseItemInformation.InnerHtml = GenerateItemDetailTable(ReservationId);

            // Discount Policy
            List<BanquetReservationClassificationDiscountBO> discountLst = new List<BanquetReservationClassificationDiscountBO>();
            discountLst = reservationDA.GetBanquetReservationClassificationDiscount(ReservationId);

            hfClassificationDiscountAlreadySave.Value = JsonConvert.SerializeObject(discountLst);
        }
        private void ClearDetailPart()
        {
            //btnAddDetailGuest.Text = "Add";
            ddlItemType.SelectedIndex = -1;
            chkIscomplementary.Checked = false;
            ddlItemId.SelectedIndex = -1;
            txtItemUnit.Text = string.Empty;
            txtUnitPrice.Text = string.Empty;
            //this.lblHiddenId.Text = string.Empty;
            this.txtHiddenProductId.Value = "";
            this._RoomReservationId = 0;
        }
        private void Cancel()
        {
            this.btnSave.Text = "Save";
            Session["ReservationDetailList"] = null;
            Session["arrayDelete"] = null;
            txtName.Text = "";
            ddlBanquetId.SelectedValue = "0";
            txtAddress.Text = "";
            txtRemarks.Text = "";
            txtBanquetRate.Text = "";
            //txtZipCode.Text = "";
            ddlCountryId.SelectedValue = "19";
            //txtEmailAddress.Text = "";
            txtPhoneNumber.Text = "";
            //txtBookingFor.Text = "";
            txtContactPerson.Text = "";
            txtContactPhone.Text = "";
            txtContactEmail.Text = "";
            txtPartyDate.Text = "";
            //txtDepartureDate.Text = "";
            txtProbableArrivalHour.Text = "12";
            txtProbableDepartureHour.Text = "12";
            txtNumberOfPersonAdult.Text = "";
            txtNumberOfPersonChild.Text = "";
            ddlOccessionTypeId.SelectedValue = "0";
            ddlSeatingId.SelectedValue = "0";
            //this.gvRegistrationDetail.DataSource = Session["ReservationDetailList"] as List<BanquetReservationDetailBO>;
            //this.gvRegistrationDetail.DataBind();
            this.ClearDetailPart();
            this.txtItemUnit.Text = "0";
            this.txtVatAmount.Text = "0";
            //this.txtGrandTotal.Text = "0";
            this.txtDiscountAmount.Text = "0";
            this.chkIsReturnedGuest.Checked = false;
            this.ddlRefferenceId.SelectedIndex = 0;
            this.txtServiceCharge.Text = "0";
            //this.txtVatAmount.Text = "0";
            this.txtGrandTotal.Text = "0";
            this.txtTotalAmount.Text = "0";
            //ltlTableWiseItemInformation.InnerHtml = string.Empty;

            this.hfDiscountedAmount.Value = "0";
            this.txtDiscountedAmount.Text = "0";
            this.txtServiceCharge.Text = "0";
            this.hfServiceCharge.Value = "0";
            this.txtVatAmount.Text = "0";
            this.hfVatAmount.Value = "0";
            this.txtGrandTotal.Text = "0";

        }
        private bool IsValidMail(string Email)
        {
            bool status = true;
            string expression = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|" + @"0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z]" + @"[a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";

            Match match = Regex.Match(Email, expression, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }
        private bool IsFormValid()
        {
            decimal result;
            bool status = true;
            //if (String.IsNullOrEmpty(this.txtName.Text))
            //{
            //    //this.isMessageBoxEnable = 1;
            //    //lblMessage.Text = "Please provide Name.";
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Company/Person Name.", AlertType.Warning);
            //    txtName.Focus();
            //    status = false;
            //}
            //else if (String.IsNullOrEmpty(this.txtContactEmail.Text))
            //{
            //    //this.isMessageBoxEnable = 1;
            //    //lblMessage.Text = "Please Provide Email Address.";
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Email Address.", AlertType.Warning);
            //    txtContactEmail.Focus();
            //    status = false;
            //}
            //else 
            if (!String.IsNullOrEmpty(this.txtContactEmail.Text))
            {
                if (!IsValidMail(txtContactEmail.Text))
                {
                    string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                    Regex re = new Regex(strRegex);
                    if (!re.IsMatch(txtContactEmail.Text))
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Email Address is not valid.", AlertType.Warning);
                        this.txtContactEmail.Focus();
                        status = false;
                    }
                }
            }
            else if (this.ddlBanquetId.SelectedIndex == 0)
            {
                //this.isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Select Banquet Name.";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Banquet Name.", AlertType.Warning);
                ddlBanquetId.Focus();
                status = false;
            }
            else if (String.IsNullOrWhiteSpace(this.txtPartyDate.Text))
            {
                //this.isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Enter Arrival Date.";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Party Start Date.", AlertType.Warning);
                txtPartyDate.Focus();
                status = false;
            }
            //else if (String.IsNullOrWhiteSpace(this.txtDepartureDate.Text))
            //{                
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Party End Date.", AlertType.Warning);
            //    txtDepartureDate.Focus();
            //    status = false;
            //}
            else if (this.ddlOccessionTypeId.SelectedIndex == 0)
            {
                //this.isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Select Occasion Type.";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Occasion Type.", AlertType.Warning);
                ddlOccessionTypeId.Focus();
                status = false;
            }
            else if (this.ddlSeatingId.SelectedIndex == 0)
            {
                //this.isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Select Seating Name.";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Seating Name.", AlertType.Warning);
                ddlSeatingId.Focus();
                status = false;
            }
            else if (String.IsNullOrWhiteSpace(this.txtNumberOfPersonAdult.Text))
            {
                //this.isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Provide Number of Chield or Adult.";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Number of Adult.", AlertType.Warning);
                txtNumberOfPersonAdult.Focus();
                status = false;
            }
            else if (!String.IsNullOrWhiteSpace(this.txtNumberOfPersonAdult.Text))
            {
                if (!Decimal.TryParse(this.txtNumberOfPersonAdult.Text, out result))
                {
                    //this.isMessageBoxEnable = 1;
                    //lblMessage.Text = "please Provide Correct Number of  Adult.";
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Correct Number of  Adult.", AlertType.Warning);
                    txtNumberOfPersonAdult.Focus();
                    status = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(this.txtPhoneNumber.Text.Trim()))
            {
                var match = Regex.Match(txtPhoneNumber.Text.Trim(), @"^(?:(?:\(?(?:00|\+0)([1-4]\d\d|[1-9]\d?)\)?)?[\-\.\ \\\/]?)?((?:\(?\d{1,}\)?[\-\.\ \\\/]?){0,})(?:[\-\.\ \\\/]?(?:#|ext\.?|extension|x)[\-\.\ \\\/]?(\d+))?$");
                if (!match.Success)
                {
                    txtPhoneNumber.Focus();
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Valid Phone Number.", AlertType.Warning);
                    status = false;
                }
            }
            else if (!string.IsNullOrWhiteSpace(this.txtContactPhone.Text.Trim()))
            {
                var match = Regex.Match(txtContactPhone.Text.Trim(), @"^(?:(?:\(?(?:00|\+0)([1-4]\d\d|[1-9]\d?)\)?)?[\-\.\ \\\/]?)?((?:\(?\d{1,}\)?[\-\.\ \\\/]?){0,})(?:[\-\.\ \\\/]?(?:#|ext\.?|extension|x)[\-\.\ \\\/]?(\d+))?$");
                if (!match.Success)
                {
                    txtContactPhone.Focus();
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Valid Mobile Number.", AlertType.Warning);
                    status = false;
                }
            }
            //else if (!String.IsNullOrWhiteSpace(this.txtNumberOfPersonChild.Text))
            //{
            //    if (!Decimal.TryParse(this.txtNumberOfPersonChild.Text, out result))
            //    {
            //        //this.isMessageBoxEnable = 1;
            //        //lblMessage.Text = "Please Provide Correct Number of  Chield.";
            //        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Correct Number of  Child.", AlertType.Warning);
            //        txtNumberOfPersonChild.Focus();
            //        status = false;
            //    }
            //}
            return status;
        }
        private void SetTab(string TabName)
        {
            if (TabName == "Search")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "Entry")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadBanquetInfo()
        {
            //ddlBanquetId
            BanquetInformationBO banquetBO = new BanquetInformationBO();
            BanquetInformationDA banquetDA = new BanquetInformationDA();
            var List = banquetDA.GetAllBanquetInformation();
            this.ddlBanquetId.DataSource = List;
            this.ddlBanquetId.DataTextField = "Name";
            this.ddlBanquetId.DataValueField = "BanquetId";
            this.ddlBanquetId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBanquetId.Items.Insert(0, item);
        }
        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;
        }
        private bool IsDetailFormValid()
        {
            bool status = true;
            decimal result;
            if (this.txtHiddenProductId.Value == "")
            {
                //this.isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Select Valid Product.";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Valid Product.", AlertType.Warning);
                this.ddlItemId.Focus();
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtItemUnit.Text) || !Decimal.TryParse(this.txtItemUnit.Text, out result))
            {
                //this.isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Provide Quantity.";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Quantity.", AlertType.Warning);
                this.txtItemUnit.Focus();
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtUnitPrice.Text) || !Decimal.TryParse(this.txtUnitPrice.Text, out result))
            {
                //this.isMessageBoxEnable = 1;
                //lblMessage.Text = "Please Provide Quantity.";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Quantity.", AlertType.Warning);
                this.txtUnitPrice.Focus();
                status = false;
            }
            return status;
        }
        private void LoadRefferenceName()
        {

            BanquetRefferenceDA refferenceDA = new BanquetRefferenceDA();
            var List = refferenceDA.GetAllBanquetRefference();
            this.ddlRefferenceId.DataSource = List;
            this.ddlRefferenceId.DataTextField = "Name";
            this.ddlRefferenceId.DataValueField = "RefferenceId";
            this.ddlRefferenceId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlRefferenceId.Items.Insert(0, item);
        }
        private void SetDefaulTime()
        {
            this.txtProbableArrivalHour.Text = "12";
            //this.txtProbableArrivalMinute.Text = "00";
            //this.ddlProbableArrivalAMPM.SelectedIndex = 1;

            this.txtProbableDepartureHour.Text = "12";
            //this.txtProbableDepartureMinute.Text = "00";
            //this.ddlProbableDepartureAMPM.SelectedIndex = 1;
        }
        private void LoadSearchBanquetInfo()
        {
            //ddlBanquetId
            BanquetInformationBO banquetBO = new BanquetInformationBO();
            BanquetInformationDA banquetDA = new BanquetInformationDA();
            var List = banquetDA.GetAllBanquetInformation();
            this.ddlSearchBanquetName.DataSource = List;
            this.ddlSearchBanquetName.DataTextField = "Name";
            this.ddlSearchBanquetName.DataValueField = "BanquetId";
            this.ddlSearchBanquetName.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlSearchBanquetName.Items.Insert(0, item);
        }
        private void LoadOccassion()
        {
            BanquetOccessionTypeBO occessionBO = new BanquetOccessionTypeBO();
            BanquetOccessionTypeDA occessionDA = new BanquetOccessionTypeDA();
            var List = occessionDA.GetAllBanquetTheme();
            this.ddlOccessionTypeId.DataSource = List;
            this.ddlOccessionTypeId.DataTextField = "Name";
            this.ddlOccessionTypeId.DataValueField = "Id";
            this.ddlOccessionTypeId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlOccessionTypeId.Items.Insert(0, item);
        }
        private void LoadSeatingPlan()
        {
            BanquetSeatingPlanDA planDA = new BanquetSeatingPlanDA();
            var List = planDA.GetBanquetPlanInformation();
            this.ddlSeatingId.DataSource = List;
            this.ddlSeatingId.DataTextField = "Name";
            this.ddlSeatingId.DataValueField = "Id";
            this.ddlSeatingId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlSeatingId.Items.Insert(0, item);

        }
        private void LoadCountry()
        {
            HMCommonDA commonDA = new HMCommonDA();
            var List = commonDA.GetAllCountries();
            this.ddlCountryId.DataSource = List;
            this.ddlCountryId.DataTextField = "CountryName";
            this.ddlCountryId.DataValueField = "CountryId";
            this.ddlCountryId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCountryId.Items.Insert(0, item);
        }
        public void LoadAffiliatedCompany()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = guestCompanyDA.GetAffiliatedGuestCompanyInfo();
            ddlCompanyId.DataSource = files;
            ddlCompanyId.DataTextField = "CompanyName";
            ddlCompanyId.DataValueField = "CompanyId";
            ddlCompanyId.DataBind();

            ListItem itemReference = new ListItem();
            itemReference.Value = "0";
            itemReference.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCompanyId.Items.Insert(0, itemReference);
        }
        private decimal CalculateGrandTotal(List<BanquetReservationDetailBO> salesDetailList)
        {
            int Count = salesDetailList.Count;
            decimal salesAmount = 0;
            for (int i = 0; i < Count; i++)
            {
                salesAmount = salesAmount + Convert.ToDecimal(salesDetailList[i].TotalPrice);
            }
            return salesAmount;
        }
        public string GenerateItemDetailTable(int reservationId)
        {
            string strTable = "";
            var deleteLink = "";
            //var editLink = "";

            BanquetReservationDetailDA reservationDA = new BanquetReservationDetailDA();
            List<BanquetReservationDetailBO> detailList = new List<BanquetReservationDetailBO>();
            detailList = reservationDA.GetBanquetReservationDetailInfoByReservationId(reservationId, 1);

            strTable += "<table id='RecipeItemInformation' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th align='left' scope='col' style='width: 45%;'>Item Name</th><th align='left' scope='col' style='width: 15%;'>Unit Price</th><th align='left' scope='col' style='width: 15%;'>Unit</th><th align='left' scope='col' style='width: 15%;'>Amount</th><th align='center' scope='col' style='width: 10%;'>Action</th></tr></thead>";
            strTable += "<tbody>";
            int counter = 0;
            if (detailList != null)
            {
                foreach (BanquetReservationDetailBO dr in detailList)
                {
                    //editLink = "<a href=\"#\" onclick= 'EditReservationDetail(this)' ><img alt=\"Delete\" src=\"../Images/edit.png\" /></a>";
                    deleteLink = "<a href=\"#\" onclick= 'DeleteDetailInfo(this)' ><img alt=\"Edit\" src=\"../Images/delete.png\" /></a>";
                    counter++;

                    if (counter % 2 == 0)
                    {
                        // It's even
                        strTable += "<tr style='background-color:White;'>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:#E3EAEB;'>";
                    }

                    strTable += "<td align='left' style=\"display:none;\">" + dr.Id + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.ReservationId + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.ItemTypeId + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.ItemType + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.ItemId + "</td>";
                    strTable += "<td align='left' style='width: 45%;'>" + dr.ItemName + "</td>";
                    strTable += "<td align='left' style='width: 15%;'>" + dr.UnitPrice + "</td>";
                    strTable += "<td align='left' style='width: 15%;'>" + dr.ItemUnit + "</td>";
                    strTable += "<td align='left' style='width: 15%;'>" + dr.TotalPrice + "</td>";

                    strTable += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";
                    strTable += "</tr>";

                }
            }
            strTable += "</tbody>";
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }

            return strTable;
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static GuestCompanyBO GetAffiliatedCompany(int companyId)
        {
            GuestCompanyBO companyBO = new GuestCompanyBO();
            GuestCompanyDA companyDA = new GuestCompanyDA();
            companyBO = companyDA.GetGuestCompanyInfoById(companyId);

            return companyBO;
        }
        [WebMethod]
        public static InvItemViewBO GetProductDataByCriteria(int categoryId, int costCenterId, string ddlItemId)
        {
            InvItemViewBO viewBO = new InvItemViewBO();
            InvItemDA itemDA = new InvItemDA();
            var obj = itemDA.GetInvItemPriceForBanquet(categoryId, costCenterId, Int32.Parse(ddlItemId));
            if (obj != null)
            {
                viewBO.UnitPriceLocal = obj.UnitPrice;
                viewBO.ItemId = obj.ItemId;
            }

            return viewBO;
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
        public static string GetUploadedImageByWebMethod(int OwnerId, string docType)
        {
            string strTable = "";
            //int ownerId = Int32.Parse(ddlSeatingId.SelectedValue);
            DocumentsDA docDA = new DocumentsDA();
            var docList = docDA.GetDocumentsInfoByDocCategoryAndOwnerId(docType, OwnerId);
            if (docList.Count > 0)
            {
                var Image = docList[0];

                strTable += "<img src='" + Image.Path + Image.Name + "'  alt='No Image Selected' border='0' />";
            }
            return strTable;
        }
        [WebMethod]
        public static BanquetInformationBO GetBanquetInfoByCriteria(string ddlItemId)
        {
            BanquetInformationBO banquetBO = new BanquetInformationBO();
            BanquetInformationDA banquetDA = new BanquetInformationDA();
            banquetBO = banquetDA.GetBanquetInformationById(Int32.Parse(ddlItemId));
            return banquetBO;
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

            //this.ddlItemType.DataSource = List;
            //this.ddlItemType.DataTextField = "MatrixInfo";
            //this.ddlItemType.DataValueField = "CategoryId";
            //this.ddlItemType.DataBind();
            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstAllValue();
            //this.ddlItemType.Items.Insert(0, item);

            return List;
        }
        [WebMethod]
        public static BanquetReservationDetailBO GetReservationDetailInfo(string detailId, string reservationId)
        {
            int dId = Convert.ToInt32(detailId);
            int rId = Convert.ToInt32(reservationId);
            BanquetReservationDetailBO detailBO = new BanquetReservationDetailBO();
            BanquetReservationDetailDA detailDA = new BanquetReservationDetailDA();
            detailBO = detailDA.GetBanquetReservationDetailInfoById(dId, rId);
            return detailBO;
        }
        [WebMethod]
        public static string GetBanquetReservationInfoForDuplicateChecking(int banquetId, string fromDate, string arriveTime, string departTime)
        {
            DateTime reservationDate;
            DateTime dateTime = DateTime.Now;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                reservationDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                fromDate = hmUtility.GetStringFromDateTime(dateTime);
                reservationDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            List<BanquetReservationBO> detailBOList = new List<BanquetReservationBO>();
            BanquetReservationDA detailDA = new BanquetReservationDA();
            detailBOList = detailDA.GetBanquetReservationInfoForDuplicateChecking(banquetId, reservationDate, reservationDate);
            return detailBOList.Count > 0 ? "1" : "0";
        }
        [WebMethod]
        public static GridViewDataNPaging<BanquetReservationBO, GridPaging> SearchReservationAndLoad(string name, string reservationNo, string banquetId, string email, string phone, string arriveDate, string departDate, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            DateTime? arrDate = null, depDate = null;
            int? banId = null;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            if (!string.IsNullOrWhiteSpace(arriveDate))
            {
                arrDate = hmUtility.GetDateTimeFromString(arriveDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(departDate))
            {
                depDate = hmUtility.GetDateTimeFromString(departDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (banquetId == "0")
            {
                banId = null;
            }
            else
            {
                banId = Convert.ToInt32(banquetId);
            }

            GridViewDataNPaging<BanquetReservationBO, GridPaging> myGridData = new GridViewDataNPaging<BanquetReservationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            BanquetReservationDA reservationDA = new BanquetReservationDA();
            List<BanquetReservationBO> reservationList = new List<BanquetReservationBO>();
            reservationList = reservationDA.GetAllReservationList(name, reservationNo, email, phone, banId, arrDate, depDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<BanquetReservationBO> distinctItems = new List<BanquetReservationBO>();
            distinctItems = reservationList.GroupBy(test => test.Id).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);
            return myGridData;
        }
    }
}
