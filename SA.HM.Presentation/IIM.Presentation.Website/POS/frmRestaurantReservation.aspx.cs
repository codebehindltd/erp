using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using System.Web.Services;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Banquet;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmRestaurantReservation : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected int isMessageBoxEnable = -1;
        protected int isListedCompanyVisible = -1;
        protected int isListedCompanyDropDownVisible = -1;
        protected int isNewAddButtonEnable = 1;
        private static int ReservationId;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            hfMinCheckInDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.Date);
            if (!IsPostBack)
            {
                HttpContext.Current.Session["ReservationDetailListForGrid"] = null;
                Session["_TableReservationId"] = null;
                Session["ReservationDetailList"] = null;
                Session["ReservationDetailListForGrid"] = null;
                Session["EditedReservationDetailList"] = null;
                Session["DeletedReservationDetailList"] = null;
                Session["AddedTableDetailList"] = null;
                Session["DeletedTableDetailList"] = null;
                Session["EditedTableDetailList"] = null;
                this.LoadCategory();
                this.LoadCurrency();
                this.LoadAffiliatedCompany();
                this.LoadCostCenter();
                this.LoadBusinessPromotion();
                this.LoadGuestReference();
                this.LoadProbableCheckInTime();
                this.LoadProbableCheckOutTime();
                this.LoadCommonDropDownHiddenField();
                string editId = Request.QueryString["editId"];
                if (!string.IsNullOrWhiteSpace(editId))
                {
                    int Id = Convert.ToInt32(editId);
                    if (Id > 0)
                    {
                        FillForm(Id);
                    }
                }
            }

        }
        //**************************** Handlers ****************************//
        protected void btnSave_Click(object sender, EventArgs e)
        {            
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            RestaurantReservationDA tableReservationDA = new RestaurantReservationDA();
            RestaurantReservationBO tableReservationBO = new RestaurantReservationBO();

            tableReservationBO.ReservationMode = this.ddlReservedMode.SelectedItem.Text;
            int arriveMin = !string.IsNullOrWhiteSpace(this.txtProbableArriveMinute.Text) ? Convert.ToInt32(this.txtProbableArriveMinute.Text) : 0;
            int arriveHour = this.ddlProbableArriveAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtProbableArriveHour.Text) % 12) : ((Convert.ToInt32(this.txtProbableArriveHour.Text) % 12) + 12);
            tableReservationBO.DateIn = hmUtility.GetDateTimeFromString(this.txtDateIn.Text, userInformationBO.ServerDateFormat).AddHours(arriveHour).AddMinutes(arriveMin);

            int departMin = !string.IsNullOrWhiteSpace(this.txtProbableDepartMinute.Text) ? Convert.ToInt32(this.txtProbableDepartMinute.Text) : 0;
            int departHour = this.ddlProbableDepartAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtProbableDepartHour.Text) % 12) : ((Convert.ToInt32(this.txtProbableDepartHour.Text) % 12) + 12);
            tableReservationBO.DateOut = hmUtility.GetDateTimeFromString(this.txtDateOut.Text, userInformationBO.ServerDateFormat).AddHours(departHour).AddMinutes(departMin);
            tableReservationBO.Remarks = this.txtRemarks.Text;

            if (ddlReservationMode.SelectedValue == "Pending")
            {
                int dMin = !string.IsNullOrWhiteSpace(this.txtPendingDeadlineMin.Text) ? Convert.ToInt32(this.txtPendingDeadlineMin.Text) : 0;
                int dHour = this.ddlPendingDeadlineAmPm.SelectedIndex == 0 ? (Convert.ToInt32(this.txtPendingDeadlineHour.Text) % 12) : ((Convert.ToInt32(this.txtPendingDeadlineHour.Text) % 12) + 12);
                tableReservationBO.ConfirmationDate = hmUtility.GetDateTimeFromString(this.txtConfirmationDate.Text, userInformationBO.ServerDateFormat).AddHours(dHour).AddMinutes(dMin);
            }
            else
            {
                tableReservationBO.ConfirmationDate = DateTime.Now;
            }
            //tableReservationBO.DateIn = hmUtility.GetDateTimeFromString(this.txtDateIn.Text).AddHours(pHour).AddMinutes(pMin);
            //tableReservationBO.DateOut = hmUtility.GetDateTimeFromString(this.txtDateOut.Text, userInformationBO.ServerDateFormat);

            tableReservationBO.ReservationType = this.ddlReservationType.SelectedItem.Text;
            tableReservationBO.ContactAddress = this.txtContactAddress.Text;
            tableReservationBO.ContactPerson = this.txtContactPerson.Text;
            tableReservationBO.MobileNumber = this.txtMobileNumber.Text;
            tableReservationBO.FaxNumber = this.txtFaxNumber.Text;
            tableReservationBO.ContactNumber = this.txtContactNumber.Text;
            tableReservationBO.ContactEmail = this.txtContactEmail.Text;
            //tableReservationBO.PayFor = Convert.ToInt32(this.ddlPayFor.SelectedValue);

            if (this.ddlReservedMode.SelectedItem.Text == "Company")
            {
                tableReservationBO.PaymentMode = this.ddlPaymentMode.SelectedValue.ToString();
                if (this.chkIsLitedCompany.Checked)
                {
                    tableReservationBO.IsListedCompany = true;
                    tableReservationBO.CompanyId = Int32.Parse(ddlCompanyName.SelectedValue);
                    tableReservationBO.ReservedCompany = string.Empty;
                }
                else
                {
                    tableReservationBO.IsListedCompany = false;
                    tableReservationBO.ReservedCompany = this.txtReservedCompany.Text;
                    this.ddlPaymentMode.SelectedIndex = 0;
                    tableReservationBO.PaymentMode = "Self";
                }
            }
            else
            {
                this.ddlPaymentMode.SelectedIndex = 0;
                tableReservationBO.PaymentMode = "Self";
            }
            tableReservationBO.Reason = this.txtReason.Text;
            tableReservationBO.BusinessPromotionId = Int32.Parse(ddlBusinessPromotionId.SelectedValue);
            tableReservationBO.ReferenceId = Int32.Parse(ddlReferenceId.SelectedValue);
            tableReservationBO.ReservationStatus = this.ddlReservationMode.SelectedValue;
            tableReservationBO.CurrencyType = Convert.ToInt32(this.ddlCurrency.SelectedValue);
            tableReservationBO.ConversionRate = !string.IsNullOrWhiteSpace(this.txtConversionRate.Text) ? Convert.ToDecimal(this.txtConversionRate.Text) : 0;

            // -------------------------------------------------------------------------------------------------------------------------------------------
            List<RestaurantReservationItemDetailBO> addItemList = new List<RestaurantReservationItemDetailBO>();
            List<RestaurantReservationItemDetailBO> deleteItemList = new List<RestaurantReservationItemDetailBO>();

            addItemList = JsonConvert.DeserializeObject<List<RestaurantReservationItemDetailBO>>(hfSaveObj.Value);
            deleteItemList = JsonConvert.DeserializeObject<List<RestaurantReservationItemDetailBO>>(hfDeleteObj.Value);
            // -------------------------------------------------------------------------------------------------------------------------------------------

            if (this.btnSave.Text.Equals("Save"))
            {
                int tmpCostCentreId = 0;
                string currentReservationNumber = String.Empty;
                tableReservationBO.ReservationId = ReservationId;
                tableReservationBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = tableReservationDA.SaveRestaurantReservationInfo(tableReservationBO, out tmpCostCentreId, Session["ReservationDetailList"] as List<RestaurantReservationTableDetailBO>, out currentReservationNumber, addItemList);
                if (status)
                {
                    //this.isMessageBoxEnable = 2;
                    //lblMessage.Text = "Reservation No: " + currentReservationNumber + " Saved Successfully.";

                    CommonHelper.AlertInfo(innboardMessage, "Reservation No: " + currentReservationNumber + " Saved Successfully.", AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                    EntityTypeEnum.EntityType.RoomReservation.ToString(), tmpCostCentreId,
                    ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservation));
                    this.Cancel();
                }
            }
            else
            {
                tableReservationBO.ReservationId = Convert.ToInt32(Session["_TableReservationId"]);
                RestaurantReservationTableDetailDA reservationDetailDA = new RestaurantReservationTableDetailDA();
                List<RestaurantReservationTableDetailBO> reservationDetailListBO = new List<RestaurantReservationTableDetailBO>();
                List<RestaurantReservationTableDetailBO> retriveReservationDetailListBO = new List<RestaurantReservationTableDetailBO>();

                List<RestaurantReservationTableDetailBO> retriveReservationDetailListBOForUpdate = new List<RestaurantReservationTableDetailBO>();

                retriveReservationDetailListBO = reservationDetailDA.GetRestaurantReservationTableDetailByResevationId(tableReservationBO.ReservationId);
                reservationDetailListBO = Session["ReservationDetailList"] as List<RestaurantReservationTableDetailBO>;

                List<RestaurantReservationTableDetailBO> deletedReservationDetailListBO = new List<RestaurantReservationTableDetailBO>();
                foreach (RestaurantReservationTableDetailBO reservationDetailBO in retriveReservationDetailListBO)
                {
                    RestaurantReservationTableDetailBO detailBODelete;
                    detailBODelete = reservationDetailListBO.Where(x => x.TableId == reservationDetailBO.TableId).FirstOrDefault();
                    if (detailBODelete != null)
                    {
                        retriveReservationDetailListBOForUpdate.Add(detailBODelete);
                        reservationDetailListBO.Remove(detailBODelete);
                    }
                    else
                    {
                        //reservationDetailBO.IsUpdateDetailData = 0;
                        deletedReservationDetailListBO.Add(reservationDetailBO);
                    }
                }

                Session["ReservationDetailList"] = reservationDetailListBO;

                //if (deletedReservationDetailListBO != null)
                //{
                //    HttpContext.Current.Session["AssignedDetailRoomDelete"] = deletedReservationDetailListBO;
                //}

                string currentReservationNumber = String.Empty;
                tableReservationBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = tableReservationDA.UpdateRestaurantReservationInfo(tableReservationBO, Session["ReservationDetailList"] as List<RestaurantReservationTableDetailBO>, deletedReservationDetailListBO, retriveReservationDetailListBOForUpdate, deleteItemList, addItemList, out currentReservationNumber);
                //Boolean status = tableReservationDA.UpdateRestaurantReservationInfo(tableReservationBO, Session["AddedTableDetailList"] as List<RestaurantReservationTableDetailBO>, Session["DeletedTableDetailList"] as List<RestaurantReservationTableDetailBO>, Session["EditedTableDetailList"] as List<RestaurantReservationTableDetailBO>, deleteItemList, addItemList, out currentReservationNumber);
                //Boolean status = tableReservationDA.UpdateRoomReservationInfo(tableReservationBO, retriveReservationDetailListBOForUpdate, Session["ReservationDetailList"] as List<ReservationDetailBO>, complementaryItemBOList, Session["AssignedDetailRoomDelete"] as List<ReservationDetailBO>, Session["UnassignedDetailRoomDelete"] as List<ReservationDetailBO>, Session["DeletedReservationDetailListByRoomType"] as List<ReservationDetailBO>, out currentReservationNumber);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Reservation No: " + currentReservationNumber + " Updated Successfully.", AlertType.Success);
                    //this.isMessageBoxEnable = 2;
                    //lblMessage.Text = "Reservation No: " + currentReservationNumber + " Update Successfully.";
                    //Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                    //EntityTypeEnum.EntityType.RoomReservation.ToString(), tableReservationBO.ReservationId,
                    //ProjectModuleEnum.ProjectModule.HotelManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservation));
                    //this.SaveLoadDropdown();
                    this.Cancel();
                    this.btnSave.Text = "Save"; 
                }
            }
        }
        //************************ User Defined Function ********************//
        private void LoadCostCenter()
        {
            CostCentreTabDA entityDA = new CostCentreTabDA();
            this.ddlCostCentreId.DataSource = entityDA.GetAllRestaurantTypeCostCentreTabInfo();
            this.ddlCostCentreId.DataTextField = "CostCenter";
            this.ddlCostCentreId.DataValueField = "CostCenterId";
            this.ddlCostCentreId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCostCentreId.Items.Insert(0, item);
        }
        public void LoadAffiliatedCompany()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = guestCompanyDA.GetAffiliatedGuestCompanyInfo();
            ddlCompanyName.DataSource = files;
            ddlCompanyName.DataTextField = "CompanyName";
            ddlCompanyName.DataValueField = "CompanyId";
            ddlCompanyName.DataBind();

            ListItem itemReference = new ListItem();
            itemReference.Value = "0";
            itemReference.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCompanyName.Items.Insert(0, itemReference);
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
        private void LoadBusinessPromotion()
        {
            BusinessPromotionDA bpDA = new BusinessPromotionDA();
            this.ddlBusinessPromotionId.DataSource = bpDA.GetCurrentActiveBusinessPromotionInfo();
            this.ddlBusinessPromotionId.DataTextField = "BPHead";
            this.ddlBusinessPromotionId.DataValueField = "BusinessPromotionId";
            this.ddlBusinessPromotionId.DataBind();

            ListItem itemReservation = new ListItem();
            itemReservation.Value = "0";
            itemReservation.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBusinessPromotionId.Items.Insert(0, itemReservation);
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
        private void LoadProbableCheckInTime()
        {
            this.txtProbableArriveHour.Text = "12";
            this.txtProbableArriveMinute.Text = "00";
            this.ddlProbableArriveAMPM.SelectedIndex = 1;
        }
        private void LoadProbableCheckOutTime()
        {
            this.txtProbableDepartHour.Text = "12";
            this.txtProbableDepartMinute.Text = "00";
            this.ddlProbableDepartAMPM.SelectedIndex = 1;
        }
        //Load Item Details
        private void LoadCategory()
        {
            List<InvCategoryBO> categoryList = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            categoryList = da.GetAllInvItemCatagoryInfoByServiceType("Product");

            this.ddlItemType.DataSource = categoryList;
            this.ddlItemType.DataTextField = "MatrixInfo";
            this.ddlItemType.DataValueField = "CategoryId";
            this.ddlItemType.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlItemType.Items.Insert(0, item);
        }
        private void Cancel()
        {
            Random rd = new Random();
            ReservationId = rd.Next(100000, 999999);
            //ReservationIdHiddenField.Value = ReservationId.ToString();
            this.LoadProbableCheckInTime();
            this.ddlReservedMode.SelectedIndex = 0;
            this.txtDateIn.Text = string.Empty;
            this.txtDateOut.Text = string.Empty;
            this.txtRemarks.Text = string.Empty;

            this.ddlReservationType.SelectedIndex = 0;
            this.txtReservedCompany.Text = string.Empty;
            this.txtContactAddress.Text = string.Empty;
            this.txtContactPerson.Text = string.Empty;
            this.txtMobileNumber.Text = string.Empty;
            this.txtFaxNumber.Text = string.Empty;
            this.txtContactNumber.Text = string.Empty;
            this.txtContactEmail.Text = string.Empty;
            this.ddlCurrency.SelectedIndex = 0;
            this.txtConversionRate.Text = "81";
            //this.hiddenGuestId.Value = string.Empty;
            this.txtReason.Text = string.Empty;
            //this.txtPersonPhone.Text = string.Empty;
            this.ddlReservationMode.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            //this.ClearDetailPart();
            //  this.gvRegistrationDetail.DataSource = null;
            //  this.gvRegistrationDetail.DataBind();
            //this._RoomReservationId = -1;

            //Session["_RoomReservationId"] = null;
            //Session["ReservationDetailList"] = null;
            //Session["ReservationDetailListForGrid"] = null;
            //Session["arrayDelete"] = null;
            //Session["DeletedReservationDetailListByRoomType"] = null;                      

            this.txtDiscountAmount.Text = "0";
            //this.txtRoomRate.Text = "0";
            this.ddlDiscountType.SelectedValue = "Fixed";
            
            this.SetTab("EntryTab");
        }
        private void FillForm(int reservationId)
        {
            lblMessage.Text = "";
            //ReservationId = reservationId;
            //ReservationIdHiddenField.Value = reservationId.ToString();
            //Master Information------------------------
            RestaurantReservationBO tableReservationBO = new RestaurantReservationBO();
            RestaurantReservationDA tableReservationDA = new RestaurantReservationDA();

            tableReservationBO = tableReservationDA.GetRestaurantReservationInfoById(reservationId);
            Session["_TableReservationId"] = tableReservationBO.ReservationId;
            this.ddlReservedMode.Text = tableReservationBO.ReservationMode;
            this.txtDateIn.Text = hmUtility.GetStringFromDateTime(tableReservationBO.DateIn);
            this.txtProbableArriveHour.Text = Convert.ToInt32(tableReservationBO.DateIn.ToString("%h")) == 0 ? "12" : tableReservationBO.DateIn.ToString("%h");
            this.txtProbableArriveMinute.Text = tableReservationBO.DateIn.ToString("mm");
            this.ddlProbableArriveAMPM.SelectedIndex = Convert.ToInt32(tableReservationBO.DateIn.ToString("HH")) == 0 ? 0 : 1;
            this.txtDateOut.Text = hmUtility.GetStringFromDateTime(tableReservationBO.DateOut);
            this.txtProbableDepartHour.Text = Convert.ToInt32(tableReservationBO.DateOut.ToString("%h")) == 0 ? "12" : tableReservationBO.DateOut.ToString("%h");
            this.txtProbableDepartMinute.Text = tableReservationBO.DateOut.ToString("mm");
            this.ddlProbableDepartAMPM.SelectedIndex = Convert.ToInt32(tableReservationBO.DateOut.ToString("HH")) == 0 ? 0 : 1;
            this.txtRemarks.Text = tableReservationBO.Remarks;

            this.txtConfirmationDate.Text = hmUtility.GetStringFromDateTime(tableReservationBO.ConfirmationDate);
            this.txtPendingDeadlineHour.Text = Convert.ToInt32(tableReservationBO.ConfirmationDate.ToString("HH")) == 0 ? "12" : tableReservationBO.ConfirmationDate.ToString("HH");
            this.txtPendingDeadlineMin.Text = tableReservationBO.ConfirmationDate.ToString("mm");
            this.ddlPendingDeadlineAmPm.SelectedIndex = Convert.ToInt32(tableReservationBO.ConfirmationDate.ToString("HH")) == 0 ? 0 : 1;

            this.txtDateOut.Text = hmUtility.GetStringFromDateTime(tableReservationBO.DateOut);

            this.ddlReservationType.Text = tableReservationBO.ReservationType;
            this.txtReservedCompany.Text = tableReservationBO.ReservedCompany;
            this.txtContactAddress.Text = tableReservationBO.ContactAddress;
            this.txtContactNumber.Text = tableReservationBO.ContactNumber;

            this.txtMobileNumber.Text = tableReservationBO.MobileNumber;
            this.txtFaxNumber.Text = tableReservationBO.FaxNumber;

            this.txtContactPerson.Text = tableReservationBO.ContactPerson;
            this.txtContactEmail.Text = tableReservationBO.ContactEmail;            
            this.txtReason.Text = tableReservationBO.Reason;
            this.ddlBusinessPromotionId.SelectedValue = tableReservationBO.BusinessPromotionId.ToString();
            this.ddlCurrency.SelectedValue = tableReservationBO.CurrencyType.ToString();
            this.txtConversionRate.Text = !string.IsNullOrWhiteSpace(tableReservationBO.ConversionRate.ToString()) ? tableReservationBO.ConversionRate.ToString() : "81";
            this.ddlReferenceId.SelectedValue = tableReservationBO.ReferenceId.ToString();
            if (tableReservationBO.ReservationStatus != "Registered")
            {
                this.ddlReservationMode.SelectedValue = tableReservationBO.ReservationMode;
            }
            this.chkIsLitedCompany.Checked = Convert.ToBoolean(tableReservationBO.IsListedCompany);
            this.ddlCompanyName.SelectedValue = tableReservationBO.CompanyId.ToString();

            //Detail Information------------------------
            //this.ddlDiscountType.SelectedIndex = tableReservationBO.DiscountType == "Fixed" ? 0 : 1;
            //this.txtDiscountAmount.Text = tableReservationBO.Amount.ToString();
            //decimal unitPrice = !string.IsNullOrWhiteSpace(this.txtUnitPriceHiddenField.Value) ? Convert.ToDecimal(this.txtUnitPriceHiddenField.Value) : 0;
            decimal discountAmount = !string.IsNullOrWhiteSpace(this.txtDiscountAmount.Text) ? Convert.ToDecimal(this.txtDiscountAmount.Text) : 1;

            //Table Details Info

            List<RestaurantReservationTableDetailBO> reservationDetailListBOForGrid = new List<RestaurantReservationTableDetailBO>();
            RestaurantReservationTableDetailDA reservationDetailDA = new RestaurantReservationTableDetailDA();

            reservationDetailListBOForGrid = reservationDetailDA.GetTableDetailListByRevIdForGrid(reservationId, 0);
            Session["ReservationDetailListForGrid"] = reservationDetailListBOForGrid;

            List<RestaurantReservationTableDetailBO> reservationDetailList = new List<RestaurantReservationTableDetailBO>();           
            reservationDetailList = reservationDetailDA.GetRestaurantReservationTableDetailByResevationId(reservationId);            
            Session["ReservationDetailList"] = reservationDetailList;

            this.btnSave.Text = "Update";
            //LoadDetailGridViewByWM();

            //Item Detail Information.......................................
            ltlTableWiseItemInformation.InnerHtml = GenerateItemDetailTable(reservationId);
            this.btnSave.Text = "Update";            
        }
        public static string GetHTMLRoomGridView(List<RestaurantTableBO> List)
        {

            string strTable = "";
            strTable += "<table cellspacing='0' cellpadding='4' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='center' scope='col'>Select</th><th align='left' scope='col'>Table Number</th></tr>";
            int counter = 0;
            foreach (RestaurantTableBO dr in List)
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

                strTable += "<td align='center' style='width: 60px'>";
                // strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformDeleteAction(" + dr.RoomId + ")' alt='Delete Information' border='0' />";
                strTable += "&nbsp;<input type='checkbox'  id='" + dr.TableId + "' name='" + dr.TableNumber + "' value='" + dr.TableId + "' >";

                strTable += "</td><td align='left' style='width: 160px'>" + dr.TableNumber + "</td></tr>";

            }
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }


            strTable += "     <button type='button' onClick='javascript:return GetCheckedTableCheckBox()' id='btnAddRoomId' class='TransactionalButton btn btn-primary'> OK</button>";
            strTable += "     <button type='button' onclick='javascript:return popup(-1)' id='btnAddRoomId' class='TransactionalButton btn btn-primary'> Cancel</button>";
            return strTable;

        }
        public static string LoadDetailGridViewByWM()
        {
            string strTable = "";
            List<RestaurantReservationTableDetailBO> detailList = HttpContext.Current.Session["ReservationDetailListForGrid"] as List<RestaurantReservationTableDetailBO>;
            if (detailList != null)
            {
                strTable += "<table style='width:100%' cellspacing='0' cellpadding='4' id='ReservationDetailGrid'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Cost Centre</th><th align='left' scope='col'>Table Numbers</th><th align='center' scope='col'>Action</th></tr>";
                int counter = 0;
                foreach (RestaurantReservationTableDetailBO dr in detailList)
                {
                    counter++;
                    if (counter % 2 == 0)
                    {
                        // It's even
                        strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 40%;'>" + dr.CostCentre + "</td>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:White;'><td align='left' style='width: 40%;'>" + dr.CostCentre + "</td>";
                    }
                    strTable += "<td align='left' style='width: 40%;'>" + dr.TableNumberListInfoWithCount + "</td>";
                    strTable += "<td align='center' style='width: 15%;'>";
                    if (dr.CostCenterId > 0)
                    {
                        strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformReservationDetailDelete(" + dr.CostCenterId + ")' alt='Delete Information' border='0' />";
                        strTable += "&nbsp;<img src='../Images/edit.png' onClick='javascript:return PerformReservationDetailEdit(" + dr.CostCenterId + ")' alt='Delete Information' border='0' />";
                    }
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
        private bool IsFormValid()
        {
            bool status = true;
            List<RestaurantReservationTableDetailBO> listReservationDetailBO = new List<RestaurantReservationTableDetailBO>();
            listReservationDetailBO = Session["ReservationDetailList"] as List<RestaurantReservationTableDetailBO>;
            List<GuestInformationBO> guestInformationList = new List<GuestInformationBO>();
            GuestInformationDA guestInformationDA = new GuestInformationDA();
            if (string.IsNullOrWhiteSpace(this.txtContactPerson.Text))
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Provide Contact Person.";
                txtContactPerson.Focus();
                status = false;
            }
            else if (listReservationDetailBO == null)
            {
                this.isMessageBoxEnable = 1;
                this.txtTableId.Text = string.Empty;
                lblMessage.Text = "Please Add Room Information.";
                ddlCostCentreId.Focus();
                status = false;
            }
            else if (listReservationDetailBO.Count == 0)
            {
                this.isMessageBoxEnable = 1;
                this.txtTableId.Text = string.Empty;
                lblMessage.Text = "Please Add Room Information.";
                ddlCostCentreId.Focus();
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtDateIn.Text))
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Provide Check In Date.";
                txtDateIn.Focus();
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtDateOut.Text))
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Provide Expected Check Out Date.";
                this.txtDateOut.Focus();
                status = false;
            }
            else if (this.ddlReservedMode.SelectedItem.Text == "Company")
            {
                isListedCompanyVisible = 1;
                if (this.chkIsLitedCompany.Checked)
                {
                    isListedCompanyDropDownVisible = 1;
                    if (this.ddlCompanyName.SelectedIndex == -1)
                    {
                        this.isMessageBoxEnable = 1;
                        lblMessage.Text = "Please Select Company Name.";
                        ddlCompanyName.Focus();
                        status = false;
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(this.txtReservedCompany.Text))
                    {
                        this.isMessageBoxEnable = 1;
                        lblMessage.Text = "Please Provide Company Name.";
                        txtReservedCompany.Focus();
                        status = false;
                    }
                }
            }
            else if (listReservationDetailBO == null)
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please add at least one room.";
                status = false;
            }


            if (this.ddlReservationMode.SelectedIndex != 0)
            {
                if (string.IsNullOrWhiteSpace(this.txtConfirmationDate.Text))
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Please Provide Confirmation Date.";
                    txtConfirmationDate.Focus();
                    status = false;
                }
                else if (string.IsNullOrWhiteSpace(this.txtPendingDeadlineHour.Text))
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Please Provide Probable Hour.";
                    txtPendingDeadlineHour.Focus();
                    status = false;
                }
                else if (string.IsNullOrWhiteSpace(this.txtPendingDeadlineMin.Text))
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Please Provide Probable Minute.";
                    txtPendingDeadlineMin.Focus();
                    status = false;
                }
            }

            if (string.IsNullOrWhiteSpace(txtMobileNumber.Text) && string.IsNullOrWhiteSpace(this.txtContactNumber.Text) && string.IsNullOrWhiteSpace(this.txtContactEmail.Text) && string.IsNullOrWhiteSpace(this.txtFaxNumber.Text))
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Provide Any Of Contact/Mobile/Email/Fax Number.";
                txtContactNumber.Focus();
                status = false;
            }

            if (!string.IsNullOrWhiteSpace(this.txtContactEmail.Text))
            {
                if (!(hmUtility.IsInnboardValidMail(this.txtContactEmail.Text)))
                {
                    this.isMessageBoxEnable = 1;
                    this.lblMessage.Text = "Please Provide valid Email";
                    this.txtContactEmail.Focus();
                    status = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(this.txtDateIn.Text))
            {
                DateTime yeasterDayDateTime = DateTime.Now.Date.AddDays(-1);
                DateTime CheckInDate = hmUtility.GetDateTimeFromString(this.txtDateIn.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                if (yeasterDayDateTime >= CheckInDate)
                {
                    this.isMessageBoxEnable = 1;
                    this.lblMessage.Text = "Please Provide valid Check In Date";
                    this.txtDateIn.Focus();
                    status = false;
                }
            }

            if (this.ddlCurrency.SelectedValue != "45")
            {
                if (string.IsNullOrWhiteSpace(this.txtConversionRate.Text))
                {
                    this.isMessageBoxEnable = 1;
                    this.lblMessage.Text = "Please Provide Conversion Rate";
                    this.txtConversionRate.Focus();
                    status = false;
                }
            }

            return status;
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
        public static string FormatedRoom(string TableList, int quantity)
        {
            string formatedString = "";
            int Length = TableList.Split(',').Length;
            var Tables = TableList.Split(',');
            if (quantity == Length)
            {
                formatedString = Length + "(" + TableList + ")";
            }
            else
            {
                formatedString = quantity + "( " + Length + "(" + TableList + ")" + "  " + (quantity - Length) + "( Unassigned )" + " )";

            }
            return formatedString;
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        public string GenerateItemDetailTable(int reservationId)
        {
            string strTable = "";
            var deleteLink = "";
            //var editLink = "";

            RestaurantReservationItemDetailDA reservationDA = new RestaurantReservationItemDetailDA();
            List<RestaurantReservationItemDetailBO> detailList = new List<RestaurantReservationItemDetailBO>();
            detailList = reservationDA.GetRestaurantReservationItemDetailByReservationId(reservationId);

            strTable += "<table cellspacing='0' cellpadding='4' id='RecipeItemInformation' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th align='left' scope='col' style='width: 45%;'>Item Name</th><th align='left' scope='col' style='width: 15%;'>Unit Price</th><th align='left' scope='col' style='width: 15%;'>Unit</th><th align='left' scope='col' style='width: 15%;'>Amount</th><th align='center' scope='col' style='width: 10%;'>Action</th></tr></thead>";
            strTable += "<tbody>";
            int counter = 0;
            if (detailList != null)
            {
                foreach (RestaurantReservationItemDetailBO dr in detailList)
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

                    strTable += "<td align='left' style=\"display:none;\">" + dr.ItemDetailId + "</td>";
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
        //************************ User Defined WebMethod ********************//
        [WebMethod(EnableSession = true)]
        public static string LoadTableInformationWithControl(string CostCentreId, string fromDate, string toDate, string arriveHour, string departHour, string arriveAMPM, string departAMPM)
        {
            HMUtility hmUtility = new HMUtility();
            HMCommonDA commonDA = new HMCommonDA();
            RestaurantTableDA tableNumberDA = new RestaurantTableDA();
            List<RestaurantTableBO> list = new List<RestaurantTableBO>();
            int _reservationId = 0;
            if (HttpContext.Current.Session["_TableReservationId"] != null)
            {
                _reservationId = Int32.Parse(HttpContext.Current.Session["_TableReservationId"].ToString());
            }
            int ahour = Convert.ToInt32(arriveHour);
            int dhour = Convert.ToInt32(departHour);
            if (arriveAMPM == "PM" && ahour != 12)
            {
                ahour = ahour + 12;
            }
            if (departAMPM == "PM" && dhour != 12)
            {
                dhour = dhour + 12;
            }

            DateTime dateTime = DateTime.Now;
            DateTime StartDate = dateTime;
            DateTime EndDate = dateTime;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                StartDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                fromDate = hmUtility.GetStringFromDateTime(dateTime);
                StartDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            if (!string.IsNullOrWhiteSpace(toDate))
            {
                EndDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                toDate = hmUtility.GetStringFromDateTime(dateTime);
                EndDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1);
            }

            list = tableNumberDA.GetAvailableTableNumberInformation(Convert.ToInt32(CostCentreId), ahour, dhour);
            string HTML = string.Empty;
            if (_reservationId == 0)
            {
                //List<RestaurantTableBO> list2 = list.Where(p => p.TableNumber == Convert.ToInt32(TableNumber)).ToList();
                HTML = GetHTMLRoomGridView(list);
            }
            else
            {
                //List<RestaurantTableBO> list2 = list.Where(p => p.TableNumber == Convert.ToInt32(TableNumber)).ToList();
                HTML = GetHTMLRoomGridView(list);
            }

            return HTML;
        }
        [WebMethod(EnableSession = true)]
        public static string PerformSaveTableDetailsInformationByWebMethod(bool isEdit, string txtSelectedTableNumbers, string txtSelectedTableId, string txtTableId, string prevCostCentreId, string ddlCostCentreId, string ddlCostCentreIdText, string lblHiddenId, string txtDiscountAmount, string ddlCurrency, string ddlDiscountType, string ddlReservedMode)
        {
            if (isEdit)
            {
                List<RestaurantReservationTableDetailBO> deletedReservationDetailListBO = HttpContext.Current.Session["DeletedReservationDetailList"] == null ? new List<RestaurantReservationTableDetailBO>() : HttpContext.Current.Session["DeletedReservationDetailList"] as List<RestaurantReservationTableDetailBO>;

                List<RestaurantReservationTableDetailBO> reservationDetailListBOForGridEdit = HttpContext.Current.Session["ReservationDetailListForGrid"] == null ? new List<RestaurantReservationTableDetailBO>() : HttpContext.Current.Session["ReservationDetailListForGrid"] as List<RestaurantReservationTableDetailBO>;
                RestaurantReservationTableDetailBO singleEntityBOEdit = reservationDetailListBOForGridEdit.Where(x => x.CostCenterId == Int32.Parse(ddlCostCentreId)).FirstOrDefault();
                if (reservationDetailListBOForGridEdit.Contains(singleEntityBOEdit))
                {
                    reservationDetailListBOForGridEdit.Remove(singleEntityBOEdit);
                }
                else
                {
                    RestaurantReservationTableDetailBO prevCostCenterIdBO = reservationDetailListBOForGridEdit.Where(x => x.CostCenterId == Int32.Parse(prevCostCentreId)).FirstOrDefault();
                    reservationDetailListBOForGridEdit.Remove(prevCostCenterIdBO);
                    deletedReservationDetailListBO.Add(prevCostCenterIdBO);
                    HttpContext.Current.Session["DeletedReservationDetailList"] = deletedReservationDetailListBO;
                }

                List<RestaurantReservationTableDetailBO> reservationDetailListBOEddit = HttpContext.Current.Session["ReservationDetailList"] == null ? new List<RestaurantReservationTableDetailBO>() : HttpContext.Current.Session["ReservationDetailList"] as List<RestaurantReservationTableDetailBO>;
                List<RestaurantReservationTableDetailBO> singleDetailEntityBOEditList = reservationDetailListBOEddit.Where(x => x.CostCenterId == Int32.Parse(ddlCostCentreId)).ToList();
                foreach (RestaurantReservationTableDetailBO row in singleDetailEntityBOEditList)
                {
                    reservationDetailListBOEddit.Remove(row);
                }
                HttpContext.Current.Session["ReservationDetailList"] = reservationDetailListBOEddit;


            }
            CostCentreTabBO costcentreBO = new CostCentreTabBO();
            CostCentreTabDA costcentreDA = new CostCentreTabDA();
            costcentreBO = costcentreDA.GetCostCentreTabInfoById(Int32.Parse(ddlCostCentreId));
            ddlCostCentreIdText = costcentreBO.CostCenter;
            string TableId = txtSelectedTableId;
            int TableCount = 1;

            string TableNumber = string.Empty;
            if (txtSelectedTableNumbers == "Unassigned")
            {
                TableNumber = string.Empty;
            }
            else
            {
                TableNumber = txtSelectedTableNumbers;
            }
            if (string.IsNullOrEmpty(TableNumber))
            {
                TableId = string.Empty;
            }

            int TableQty = !string.IsNullOrWhiteSpace(txtTableId) ? Convert.ToInt32(txtTableId) : 0;
            if (TableId.Split(',').Length >= TableQty)
            {
                TableCount = TableId.Split(',').Length;
            }
            else
            {
                TableCount = TableQty;
            }
            string TableTypeInfo = string.Empty;

            List<RestaurantReservationTableDetailBO> reservationDetailListBOForGrid = HttpContext.Current.Session["ReservationDetailListForGrid"] == null ? new List<RestaurantReservationTableDetailBO>() : HttpContext.Current.Session["ReservationDetailListForGrid"] as List<RestaurantReservationTableDetailBO>;
            RestaurantReservationTableDetailBO singleEntityBO = reservationDetailListBOForGrid.Where(x => x.CostCenterId == Int32.Parse(ddlCostCentreId)).FirstOrDefault();
            if (reservationDetailListBOForGrid.Contains(singleEntityBO))
            {
                int prevCount = 1;
                string prevIdList = "1";
                singleEntityBO.TableQuantity = TableCount + prevCount;
                //singleEntityBO.TotalCalculatedAmount = RoomRateAmount * singleEntityBO.RoomQuantity;
                singleEntityBO.DiscountType = ddlDiscountType;
                //singleEntityBO.UnitPrice = !string.IsNullOrWhiteSpace(txtUnitPriceHiddenField) ? Convert.ToDecimal(txtUnitPriceHiddenField) : 0;

                if (ddlDiscountType == "Fixed")
                {
                    //singleEntityBO.Discount = singleEntityBO.UnitPrice - RoomRateAmount;
                    if (txtDiscountAmount != "")
                    {
                        singleEntityBO.Amount = Convert.ToDecimal(txtDiscountAmount);
                    }

                }
                else
                {
                    //decimal percantAmount = (((singleEntityBO.UnitPrice - RoomRateAmount) / singleEntityBO.UnitPrice) * 100);
                    //singleEntityBO.Discount = percantAmount;
                    singleEntityBO.Amount = 100;
                }

                if (!string.IsNullOrWhiteSpace(TableNumber))
                {
                    singleEntityBO.TableNumberIdList = singleEntityBO.TableNumberIdList + ',' + TableId;
                }

                if (!string.IsNullOrEmpty(TableNumber))
                {
                    singleEntityBO.TableNumber = singleEntityBO.TableNumber + "," + TableNumber;
                }

                singleEntityBO.TableNumberListInfoWithCount = FormatedRoom(singleEntityBO.TableNumber, singleEntityBO.TableQuantity);
                for (int i = 0; i < reservationDetailListBOForGrid.Count; i++)
                {
                    if (reservationDetailListBOForGrid[i].CostCenterId == Int32.Parse(ddlCostCentreId))
                    {
                        reservationDetailListBOForGrid[i] = singleEntityBO;
                    }
                }

            }
            else
            {
                singleEntityBO = new RestaurantReservationTableDetailBO();
                singleEntityBO.CostCenterId = Convert.ToInt32(ddlCostCentreId);
                singleEntityBO.CostCentre = ddlCostCentreIdText;                
                singleEntityBO.TableQuantity = TableCount;                
                singleEntityBO.TableNumberIdList = TableId;
                singleEntityBO.TableNumber = TableNumber;
                singleEntityBO.TableNumberList = !string.IsNullOrWhiteSpace(TableNumber) ? TableNumber : "Unassigned";
                singleEntityBO.TableNumberListInfoWithCount = TableCount + "(" + singleEntityBO.TableNumberList + ")";
                singleEntityBO.DiscountType = ddlDiscountType;
                //singleEntityBO.UnitPrice = !string.IsNullOrWhiteSpace(txtUnitPriceHiddenField) ? Convert.ToDecimal(txtUnitPriceHiddenField) : 0;

                if (ddlDiscountType == "Fixed")
                {
                    //singleEntityBO.Discount = singleEntityBO.UnitPrice - RoomRateAmount;
                    if (txtDiscountAmount != "")
                    {
                        singleEntityBO.Amount = Convert.ToDecimal(txtDiscountAmount);
                    }

                }
                else
                {
                    //decimal percantAmount = (((singleEntityBO.UnitPrice - RoomRateAmount) / singleEntityBO.UnitPrice) * 100);
                    //singleEntityBO.Discount = percantAmount;
                    singleEntityBO.Amount = 200;
                }

                //singleEntityBO.Discount = !string.IsNullOrWhiteSpace(txtDiscountAmount) ? Convert.ToDecimal(txtDiscountAmount) : 0;
                //singleEntityBO.Amount = !string.IsNullOrWhiteSpace(txtDiscountAmount) ? Convert.ToDecimal(txtDiscountAmount) : 0;

                reservationDetailListBOForGrid.Add(singleEntityBO);
            }

            HttpContext.Current.Session["ReservationDetailListForGrid"] = reservationDetailListBOForGrid;

            string[] Tables = TableNumber.Split(',');
            string[] Id = TableId.Split(',');

            int dynamicDetailId = 0;

            if (!string.IsNullOrWhiteSpace(lblHiddenId))
                dynamicDetailId = Convert.ToInt32(lblHiddenId);

            List<RestaurantReservationTableDetailBO> reservationDetailListBOAddedList = new List<RestaurantReservationTableDetailBO>();
            List<RestaurantReservationTableDetailBO> reservationDetailListBO = HttpContext.Current.Session["ReservationDetailList"] == null ? new List<RestaurantReservationTableDetailBO>() : HttpContext.Current.Session["ReservationDetailList"] as List<RestaurantReservationTableDetailBO>;

            if (Tables[0].Length == 0)
            {
                int tableQuantity;

                if (!Int32.TryParse(txtTableId, out tableQuantity))
                {
                }
                for (int i = 0; i < tableQuantity; i++)
                {
                    RestaurantReservationTableDetailBO detailBO = new RestaurantReservationTableDetailBO();

                    detailBO.CostCenterId = Convert.ToInt32(ddlCostCentreId);
                    detailBO.CostCentre = ddlCostCentreIdText;
                    detailBO.TableId = 0;
                    //detailBO.UnitPrice = !string.IsNullOrWhiteSpace(txtUnitPriceHiddenField) ? Convert.ToDecimal(txtUnitPriceHiddenField) : 0;
                    //detailBO.RoomRate = !string.IsNullOrWhiteSpace(txtRoomRate) ? Convert.ToDecimal(txtRoomRate) : detailBO.UnitPrice;

                    if (ddlDiscountType == "Fixed")
                    {
                        //detailBO.Discount = singleEntityBO.UnitPrice - RoomRateAmount;
                        if (txtDiscountAmount != "")
                        {
                            detailBO.Amount = Convert.ToDecimal(txtDiscountAmount);// singleEntityBO.UnitPrice - RoomRateAmount;
                        }
                    }
                    else
                    {
                        // decimal percantAmount = (((singleEntityBO.UnitPrice - RoomRateAmount) / singleEntityBO.UnitPrice) * 100);
                        // detailBO.Discount = percantAmount;
                        detailBO.Amount = Convert.ToDecimal(txtDiscountAmount);// percantAmount;
                    }

                    //detailBO.CurrencyType = Convert.ToInt32(ddlCurrency);
                    detailBO.DiscountType = ddlDiscountType;
                    detailBO.TableNumber = "Unassigned";
                    detailBO.TableDetailId = 0;
                    reservationDetailListBO.Add(detailBO);
                }
            }
            else
            {
                for (int i = 0; i < Id.Length; i++)
                {
                    RestaurantReservationTableDetailBO detailBO = dynamicDetailId == 0 ? new RestaurantReservationTableDetailBO() : reservationDetailListBO.Where(x => x.TableDetailId == dynamicDetailId).FirstOrDefault();
                    if (reservationDetailListBO.Contains(detailBO))
                        reservationDetailListBO.Remove(detailBO);

                    detailBO.CostCenterId = Convert.ToInt32(ddlCostCentreId);
                    detailBO.CostCentre = ddlCostCentreId;
                    detailBO.TableId = Int32.Parse(Id[i]);
                    detailBO.TableNumber = Tables[i];                    

                    if (ddlDiscountType == "Fixed")
                    {
                        // detailBO.Discount = singleEntityBO.UnitPrice - RoomRateAmount;
                        if (txtDiscountAmount != "")
                        {
                            detailBO.Amount = Convert.ToDecimal(txtDiscountAmount);// singleEntityBO.UnitPrice - RoomRateAmount;
                        }
                    }
                    else
                    {
                        //decimal percantAmount = (((singleEntityBO.UnitPrice - RoomRateAmount) / singleEntityBO.UnitPrice) * 100);
                        //detailBO.Discount = percantAmount;
                        detailBO.Amount = Convert.ToDecimal(txtDiscountAmount);// percantAmount;
                    }

                    //detailBO.CurrencyType = Convert.ToInt32(ddlCurrency);
                    detailBO.DiscountType = ddlDiscountType;

                    detailBO.TableDetailId = dynamicDetailId == 0 ? reservationDetailListBO.Count + 1 : dynamicDetailId;
                    reservationDetailListBO.Add(detailBO);
                }
            }

            HttpContext.Current.Session["ReservationDetailList"] = reservationDetailListBO;
            //HttpContext.Current.Session["AddedTableDetailList"] = reservationDetailListBO;
             
            return LoadDetailGridViewByWM();
        }
        [WebMethod(EnableSession = true)]
        public static string PerformDeleteByWebMethod(string coscenterId)
        {
            int costCenterId = Convert.ToInt32(coscenterId);
            var reservationDetailList = (List<RestaurantReservationTableDetailBO>)HttpContext.Current.Session["ReservationDetailListForGrid"];
            var reservationDetailBO = reservationDetailList.Where(x => x.CostCenterId == costCenterId).FirstOrDefault();
            reservationDetailList.Remove(reservationDetailBO);
            HttpContext.Current.Session["ReservationDetailListForGrid"] = reservationDetailList;

            List<RestaurantReservationTableDetailBO> deletedReservationDetailList = HttpContext.Current.Session["DeletedTableDetailList"] == null ? new List<RestaurantReservationTableDetailBO>() : HttpContext.Current.Session["DeletedTableDetailList"] as List<RestaurantReservationTableDetailBO>;
            deletedReservationDetailList.Add(reservationDetailBO);
            HttpContext.Current.Session["DeletedTableDetailList"] = deletedReservationDetailList;

            List<RestaurantReservationTableDetailBO> reservationDetailListBOEddit = HttpContext.Current.Session["ReservationDetailList"] == null ? new List<RestaurantReservationTableDetailBO>() : HttpContext.Current.Session["ReservationDetailList"] as List<RestaurantReservationTableDetailBO>;
            List<RestaurantReservationTableDetailBO> singleDetailEntityBOEditList = reservationDetailListBOEddit.Where(x => x.CostCenterId == costCenterId).ToList();
            foreach (RestaurantReservationTableDetailBO row in singleDetailEntityBOEditList)
            {
                reservationDetailListBOEddit.Remove(row);
            }
            HttpContext.Current.Session["ReservationDetailList"] = reservationDetailListBOEddit;

            return LoadDetailGridViewByWM();
        }
        [WebMethod(EnableSession = true)]
        public static RestaurantReservationTableDetailBO PerformReservationDetailEditByWebMethod(int costcenterId)
        {
            int costCenterId = Convert.ToInt32(costcenterId);
            List<RestaurantReservationTableDetailBO> reservationDetailList = HttpContext.Current.Session["ReservationDetailListForGrid"] == null ? new List<RestaurantReservationTableDetailBO>() : HttpContext.Current.Session["ReservationDetailListForGrid"] as List<RestaurantReservationTableDetailBO>;
            RestaurantReservationTableDetailBO reservationDetailBO = reservationDetailList.Where(x => x.CostCenterId == costCenterId).FirstOrDefault();
            HttpContext.Current.Session["EditedTableDetailList"] = reservationDetailBO;
            if (reservationDetailBO != null)
            {
                string[] dataArray = reservationDetailBO.TableNumberListInfoWithCount.Split('(');
                reservationDetailBO.TableQuantity = Convert.ToInt32(dataArray[0]);
            }
            return reservationDetailBO;
        }
        [WebMethod]
        public static GridViewDataNPaging<RestaurantReservationBO, GridPaging> SearchResevationAndLoadGridInformation(string strFromDate, string strToDate, string contactPerson, string reserveNo, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<RestaurantReservationBO, GridPaging> myGridData = new GridViewDataNPaging<RestaurantReservationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            RestaurantReservationDA rrDA = new RestaurantReservationDA();
            List<RestaurantReservationBO> reservationInfoList = new List<RestaurantReservationBO>();


            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (!string.IsNullOrWhiteSpace(strFromDate))
            {
                fromDate = Convert.ToDateTime(strFromDate);
            }
            if (!string.IsNullOrWhiteSpace(strToDate))
            {
                toDate = Convert.ToDateTime(strToDate);
            }

            reservationInfoList = rrDA.GetRestaurantReservationInfoBySearchCriteriaForPaging(fromDate, toDate, contactPerson, reserveNo, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<RestaurantReservationBO> distinctItems = new List<RestaurantReservationBO>();
            distinctItems = reservationInfoList.GroupBy(test => test.ReservationNumber).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static string DeleteReservationRecord(int pkId)
        {
            string result = string.Empty;
            try
            {
                RestaurantReservationDA tableReservationDA = new RestaurantReservationDA();
                Boolean statusApproved = tableReservationDA.DeleteTableReservationDetailInfoById(pkId);
                if (statusApproved)
                {
                    result = "success";
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            return result;
        }
        //Item Details Information
        [WebMethod]
        public static List<InvItemBO> GetServiceByCriteria(int CategoryId)
        {
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetDynamicallyItemInformationByCategoryId(0, CategoryId);

            return productList;
        }
        [WebMethod]
        public static string GetTableDetailGridInformationByWM()
        {
            return LoadDetailGridViewByWM();
        }
        //End Item Details
    }
}