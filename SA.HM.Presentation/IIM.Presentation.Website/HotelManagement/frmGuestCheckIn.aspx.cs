using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Entity.UserInformation;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmGuestCheckIn : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomDocId.Value = seatingId.ToString();
                tempDocId.Value = seatingId.ToString();
                //hfId.Value = "0";
                hfParentDoc.Value = "0";
                FileUpload();
                CheckMandatoryField();
                this.CheckPermission();
                this.LoadProfession();
                this.RandomGuestIdGeneration();
                this.LoadSearchCountryList();
                LoadGuestTitle();

                string roomId = Request.QueryString["RoomId"];
                if (!string.IsNullOrEmpty(roomId))
                {
                    RoomNumberDA numberDA = new RoomNumberDA();
                    RoomNumberBO numberBO = numberDA.GetRoomNumberInfoById(Int32.Parse(roomId));
                    txtSrcRoomNumber.Text = numberBO.RoomNumber;
                    this.SearchInformation();
                }
            }
            //flashUpload.QueryParameters = "guestId=" + Server.UrlEncode(Convert.ToString(RandomGuestId.Value));
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Clear();
        }
        protected void CheckMandatoryField()
        {
            FormWiseFieldSetupDA formWiseFieldSetupDA = new FormWiseFieldSetupDA();
            var formWiseFields = formWiseFieldSetupDA.GetFieldsByMenuLinkID(228).Where(a => a.IsMandatory == true);
            hfMandatoryFields.Value = JsonConvert.SerializeObject(formWiseFields);
        }
        protected void btnSrcRoomNumber_Click(object sender, EventArgs e)
        {
            this.SearchInformation();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }
            int id = 0;
            RoomRegistrationDA regDA = new RoomRegistrationDA();
            GuestInformationBO detailBO = new GuestInformationBO();
            HMCommonDA hmCommonDA = new HMCommonDA();
            int OwnerIdForDocuments = 0;
            detailBO.tempOwnerId = Convert.ToInt32(RandomGuestId.Value);
            detailBO.GuestAddress1 = txtCompanyName.Text;
            detailBO.GuestAddress2 = txtGuestAddress.Text;
            detailBO.GuestAuthentication = string.Empty;
            detailBO.ProfessionId = Int32.Parse(ddlProfessionId.SelectedValue);
            detailBO.GuestCity = txtGuestCity.Text;
            if (!string.IsNullOrWhiteSpace(txtGuestDOB.Text))
            {
                detailBO.GuestDOB = hmUtility.GetDateTimeFromString(txtGuestDOB.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.GuestDOB = null;
            }
            detailBO.GuestDrivinlgLicense = txtGuestDrivinlgLicense.Text;
            detailBO.GuestEmail = txtGuestEmail.Text;
            if (ddlTitle.SelectedValue == "MrNMrs.")
            {
                detailBO.Title = "Mr. & Mrs.";
            }
            else detailBO.Title = ddlTitle.SelectedValue;
            detailBO.FirstName = txtFirstName.Text;
            detailBO.LastName = txtLastName.Text;
            detailBO.GuestName = ddlTitle.SelectedItem.Text + " " + detailBO.FirstName + " " + detailBO.LastName;
            if (string.IsNullOrEmpty(hiddenGuestId.Value))
            {
                detailBO.GuestId = 0;
            }
            else
            {
                detailBO.GuestId = Int32.Parse(hiddenGuestId.Value);
            }
            detailBO.GuestNationality = txtGuestNationality.Text;
            detailBO.GuestPhone = txtGuestPhone.Text;
            detailBO.GuestSex = ddlGuestSex.SelectedValue;
            detailBO.GuestZipCode = txtGuestZipCode.Text;
            detailBO.NationalId = txtNationalId.Text;
            detailBO.PassportNumber = txtPassportNumber.Text;
            if (!string.IsNullOrWhiteSpace(txtPExpireDate.Text))
            {
                detailBO.PExpireDate = hmUtility.GetDateTimeFromString(txtPExpireDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.PExpireDate = null;
            }
            if (!string.IsNullOrWhiteSpace(txtPIssueDate.Text))
            {
                detailBO.PIssueDate = hmUtility.GetDateTimeFromString(txtPIssueDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.PIssueDate = null;
            }
            detailBO.PIssuePlace = txtPIssuePlace.Text;
            if (!string.IsNullOrWhiteSpace(txtVExpireDate.Text))
            {
                detailBO.VExpireDate = hmUtility.GetDateTimeFromString(txtVExpireDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.VExpireDate = null;
            }
            detailBO.VisaNumber = txtVisaNumber.Text;
            if (!string.IsNullOrWhiteSpace(txtVIssueDate.Text))
            {
                detailBO.VIssueDate = hmUtility.GetDateTimeFromString(txtVIssueDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                detailBO.VIssueDate = null;
            }

            string regId = hfRegistrationId.Value.ToString();
            DateTime checkInDate = DateTime.Now;
            decimal paxInRate = !string.IsNullOrWhiteSpace(this.txtPaxInRate.Text) ? Convert.ToDecimal(this.txtPaxInRate.Text) : 0;

            detailBO.GuestCountryId = Int32.Parse(ddlGuestCountry.SelectedValue);
            bool status = regDA.SaveTemporaryPaxInInfo(detailBO, regId, checkInDate, paxInRate, out id);
            if (status && detailBO.GuestId == 0)
            {
                OwnerIdForDocuments = Convert.ToInt32(id);

                DocumentsDA documentsDA = new DocumentsDA();
                string s = hfDeletedDoc.Value;
                string[] DeletedDocList = s.Split(',');
                for (int i = 0; i < DeletedDocList.Length; i++)
                {
                    DeletedDocList[i] = DeletedDocList[i].Trim();
                    Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
                }
                Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(RandomDocId.Value));

                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.PaxIn.ToString(), detailBO.GuestId,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PaxIn));
            }
            else if (status && detailBO.GuestId > 0)
            {
                OwnerIdForDocuments = Convert.ToInt32(detailBO.GuestId);

                DocumentsDA documentsDA = new DocumentsDA();
                string s = hfDeletedDoc.Value;
                string[] DeletedDocList = s.Split(',');
                for (int i = 0; i < DeletedDocList.Length; i++)
                {
                    DeletedDocList[i] = DeletedDocList[i].Trim();
                    Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
                }
                Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(RandomDocId.Value));

                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.PaxIn.ToString(), detailBO.GuestId,
                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PaxIn));
            }
            Random rd = new Random();
            int randomId = rd.Next(100000, 999999);
            RandomDocId.Value = randomId.ToString();
            this.Clear();
            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);


            //if (!string.IsNullOrEmpty(this.txtPaxInRate.Text))
            //{
            //    if (!txtPaxInRate.Text.All(c => Char.IsNumber(c)))
            //    {
            //        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "only Numeric Number for Pax In Rate.", AlertType.Warning);
            //    }
            //    else
            //    {
            //        if (!string.IsNullOrEmpty(this.txtSrcRoomNumber.Text))
            //        {
            //            //if (string.IsNullOrWhiteSpace(this.txtGuestName.Text))
            //            //{
            //            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Guest Name.", AlertType.Warning);
            //            //    return;
            //            //}
            //            if (this.ddlGuestSex.SelectedIndex == 0)
            //            {
            //                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Gender.", AlertType.Warning);
            //                return;
            //            }
            //            else if (this.ddlGuestCountry.SelectedIndex == 0)
            //            {
            //                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Country Name.", AlertType.Warning);
            //                return;
            //            }

            //            RoomRegistrationDA regDA = new RoomRegistrationDA();
            //            GuestInformationBO detailBO = new GuestInformationBO();
            //            detailBO.tempOwnerId = Convert.ToInt32(RandomGuestId.Value);
            //            detailBO.GuestAddress1 = txtCompanyName.Text;
            //            detailBO.GuestAddress2 = txtGuestAddress.Text;
            //            detailBO.GuestAuthentication = string.Empty;
            //            detailBO.ProfessionId = Int32.Parse(ddlProfessionId.SelectedValue);
            //            detailBO.GuestCity = txtGuestCity.Text;
            //            if (!string.IsNullOrWhiteSpace(txtGuestDOB.Text))
            //            {
            //                detailBO.GuestDOB = hmUtility.GetDateTimeFromString(txtGuestDOB.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            //            }
            //            else
            //            {
            //                detailBO.GuestDOB = null;
            //            }
            //            detailBO.GuestDrivinlgLicense = txtGuestDrivinlgLicense.Text;
            //            detailBO.GuestEmail = txtGuestEmail.Text;
            //            if (ddlTitle.SelectedValue == "MrNMrs.")
            //            {
            //                detailBO.Title = "Mr. & Mrs.";
            //            }
            //            else detailBO.Title = ddlTitle.SelectedValue;
            //            detailBO.FirstName = txtFirstName.Text;
            //            detailBO.LastName = txtLastName.Text;
            //            detailBO.GuestName = detailBO.Title + " " + detailBO.FirstName + " " + detailBO.LastName;
            //            if (string.IsNullOrEmpty(hiddenGuestId.Value))
            //            {
            //                detailBO.GuestId = 0;
            //            }
            //            else
            //            {
            //                detailBO.GuestId = Int32.Parse(hiddenGuestId.Value);
            //            }
            //            detailBO.GuestNationality = txtGuestNationality.Text;
            //            detailBO.GuestPhone = txtGuestPhone.Text;
            //            detailBO.GuestSex = ddlGuestSex.SelectedValue;
            //            detailBO.GuestZipCode = txtGuestZipCode.Text;
            //            detailBO.NationalId = txtNationalId.Text;
            //            detailBO.PassportNumber = txtPassportNumber.Text;
            //            if (!string.IsNullOrWhiteSpace(txtPExpireDate.Text))
            //            {
            //                detailBO.PExpireDate = hmUtility.GetDateTimeFromString(txtPExpireDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            //            }
            //            else
            //            {
            //                detailBO.PExpireDate = null;
            //            }
            //            if (!string.IsNullOrWhiteSpace(txtPIssueDate.Text))
            //            {
            //                detailBO.PIssueDate = hmUtility.GetDateTimeFromString(txtPIssueDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            //            }
            //            else
            //            {
            //                detailBO.PIssueDate = null;
            //            }
            //            detailBO.PIssuePlace = txtPIssuePlace.Text;
            //            if (!string.IsNullOrWhiteSpace(txtVExpireDate.Text))
            //            {
            //                detailBO.VExpireDate = hmUtility.GetDateTimeFromString(txtVExpireDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            //            }
            //            else
            //            {
            //                detailBO.VExpireDate = null;
            //            }
            //            detailBO.VisaNumber = txtVisaNumber.Text;
            //            if (!string.IsNullOrWhiteSpace(txtVIssueDate.Text))
            //            {
            //                detailBO.VIssueDate = hmUtility.GetDateTimeFromString(txtVIssueDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            //            }
            //            else
            //            {
            //                detailBO.VIssueDate = null;
            //            }

            //            string regId = hfRegistrationId.Value.ToString();
            //            DateTime checkInDate = DateTime.Now;
            //            decimal paxInRate = !string.IsNullOrWhiteSpace(this.txtPaxInRate.Text) ? Convert.ToDecimal(this.txtPaxInRate.Text) : 0;

            //            detailBO.GuestCountryId = Int32.Parse(ddlGuestCountry.SelectedValue);
            //            bool status = regDA.SaveTemporaryPaxInInfo(detailBO, regId, checkInDate, paxInRate);
            //            if (status && detailBO.GuestId == 0)
            //            {
            //                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.PaxIn.ToString(), detailBO.GuestId,
            //                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PaxIn));
            //            }
            //            else if (status && detailBO.GuestId > 0)
            //            {
            //                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.PaxIn.ToString(), detailBO.GuestId,
            //                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PaxIn));
            //            }
            //            this.Clear();
            //            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
            //        }
            //        else
            //        {
            //            CommonHelper.AlertInfo(innboardMessage, "Please Search Room", AlertType.Warning);
            //        }
            //    }
            //}
            //else
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Pax In Rate.", AlertType.Warning);
            //}
        }
        //************************ User Defined Function ********************//
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            //flashUpload.QueryParameters = "TaskAssignDocId=" + Server.UrlEncode(RandomDocId.Value);
        }
        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;
        }
        private void RandomGuestIdGeneration()
        {
            int randomId = 0;

            Random rd = new Random();
            randomId = rd.Next(100000, 999999);
            RandomGuestId.Value = randomId.ToString();
        }
        private void SearchInformation()
        {
            if (!string.IsNullOrWhiteSpace(this.txtSrcRoomNumber.Text))
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(this.txtSrcRoomNumber.Text);
                if (roomAllocationBO.RoomId > 0)
                {
                    this.ddlRegistrationId.Visible = true;
                    this.hfRegistrationId.Value = roomAllocationBO.RegistrationId.ToString();
                    this.hfRegistrationNumber.Value = roomAllocationBO.RegistrationNumber.ToString();
                    this.hfCheckInDate.Value = roomAllocationBO.ArriveDate.ToString();
                    //this.lblCurrencyType.Text = " (" + roomAllocationBO.CurrencyTypeHead + ")";
                    this.lblCurrencyType.Text = roomAllocationBO.CurrencyTypeHead;

                    this.LoadRegistrationNumber(roomAllocationBO.RoomId);
                }
                else
                {
                    this.ddlRegistrationId.Visible = false;
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a Valid Room Number.", AlertType.Warning);
                    this.txtSrcRoomNumber.Focus();
                }
            }
            else
            {
                this.ddlRegistrationId.Visible = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a Valid Room Number.", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
            }
        }
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
        }
        private void LoadSearchCountryList()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CountriesBO> countryList = commonDA.GetAllCountries();
            this.ddlGuestCountry.DataSource = countryList;
            this.ddlGuestCountry.DataTextField = "CountryName";
            this.ddlGuestCountry.DataValueField = "CountryId";
            this.ddlGuestCountry.DataBind();
            ListItem itemCountry = new ListItem();
            itemCountry.Value = "0";
            itemCountry.Text = hmUtility.GetDropDownFirstValue();
            this.ddlGuestCountry.Items.Insert(0, itemCountry);
        }
        private bool IsFrmValid()
        {
            bool flag = true;

            if (string.IsNullOrEmpty(this.txtSrcRoomNumber.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Search Room", AlertType.Warning);
                this.txtSrcRoomNumber.Focus();
                flag = false;
                return flag;
            }
            else if (this.ddlGuestSex.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Gender.", AlertType.Warning);
                this.ddlGuestSex.Focus();
                flag = false;
                return flag;
            }
            else if (this.ddlGuestCountry.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Country Name.", AlertType.Warning);
                this.ddlGuestCountry.Focus();
                flag = false;
                return flag;
            }

            if (ddlExtraBedCharge.SelectedValue == "Yes")
            {
                decimal paxInRate = !string.IsNullOrWhiteSpace(this.txtPaxInRate.Text) ? Convert.ToDecimal(this.txtPaxInRate.Text) : 0;
                if (paxInRate < 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Entered Pax In Rate is not in correct format.", AlertType.Warning);
                    this.txtPaxInRate.Focus();
                    flag = false;
                    return flag;
                }
            }
            return flag;
        }
        private void Clear()
        {
            this.txtSrcRoomNumber.Text = null;
            this.txtGuestName.Text = null;
            ddlTitle.SelectedIndex = 0;
            txtFirstName.Text = "";
            txtLastName.Text = "";
            this.txtGuestDOB.Text = null;
            this.ddlGuestSex.SelectedIndex = 0;
            this.txtCompanyName.Text = null;
            this.txtGuestAddress.Text = null;
            this.txtGuestEmail.Text = null;
            this.ddlProfessionId.SelectedIndex = 0;
            this.txtGuestPhone.Text = null;
            this.txtGuestCity.Text = null;
            this.txtGuestZipCode.Text = null;
            this.ddlGuestCountry.SelectedIndex = 0;
            this.txtGuestNationality.Text = null;
            this.txtGuestDrivinlgLicense.Text = null;
            this.txtNationalId.Text = null;
            this.txtVisaNumber.Text = null;
            this.txtVIssueDate.Text = null;
            this.txtVExpireDate.Text = null;
            this.txtPassportNumber.Text = null;
            this.txtPIssuePlace.Text = null;
            this.txtPIssueDate.Text = null;
            this.txtPExpireDate.Text = null;
            this.txtPaxInRate.Text = null;
        }
        private void LoadRegistrationNumber(int roomId)
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            this.ddlRegistrationId.DataSource = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(roomId);
            this.ddlRegistrationId.DataTextField = "RegistrationNumber";
            this.ddlRegistrationId.DataValueField = "RegistrationId";
            this.ddlRegistrationId.DataBind();
        }

        private void LoadGuestTitle()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> titleList = new List<CustomFieldBO>();
            titleList = commonDA.GetCustomField("GuestTitle");

            ddlTitle.DataSource = titleList;
            ddlTitle.DataValueField = "FieldValue";
            ddlTitle.DataTextField = "Description";
            ddlTitle.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlTitle.Items.Insert(0, item);
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static string SearchGuestAndLoadGridInformation(string companyName, string DateOfBirth, string EmailAddress, string FromDate, string ToDate, string GuestName, string MobileNumber, string NationalId, string PassportNumber, string RegistrationNumber, string RoomNumber)
        {
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
        [WebMethod]
        public static GuestInformationBO LoadDetailInformation(string GuestId)
        {
            HMCommonDA commonDA = new HMCommonDA();
            return commonDA.GetGuestDetailInformation(GuestId);
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
        public static List<DocumentsBO> GetUploadedDocByWebMethod(int randomId, int id, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }
            List<DocumentsBO> docList = new List<DocumentsBO>();
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("Guest", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("Guest", (int)id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));
            foreach (DocumentsBO dc in docList)
            {

                if (dc.DocumentType == "Image")
                    dc.Path = (dc.Path + dc.Name);

                dc.Name = dc.Name.Remove(dc.Name.LastIndexOf('.'));
            }
            docList = new HMCommonDA().GetDocumentListWithIcon(docList).ToList();
            return docList;
        }
    }
}