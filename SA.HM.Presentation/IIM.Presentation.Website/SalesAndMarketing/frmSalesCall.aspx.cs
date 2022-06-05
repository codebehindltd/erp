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
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using System.Web.Services;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using Newtonsoft.Json;
using System.Collections;
using Mamun.Presentation.Website.Common;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.SalesManagment;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class frmSalesCall : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            hfMinCheckInDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.Date);
            if (!IsPostBack)
            {
                Session["EmployeeList"] = null;
                LoadFollowUpType();
                LoadPurpose();
                LoadCompany();
                SetDefaulTime();
                LoadEmployee();
                LoadLocation();
                LoadCity();
                LoadIndustry();
                LoadReference();
                LoadIndustryForEnlistedCompany();
                LoadReferenceForEnlistedCompany();
                LoadCommonDropDownHiddenField();
                LoadSalesCallNotification();
                LoadCIType();
                LoadActionPlan();
                LoadOpportunityStatus();
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Cancel();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            SalesCallDA salesCallDA = new SalesCallDA();
            SMCompanySalesCallBO salesCallBO = new SMCompanySalesCallBO();

            //salesCallBO.CompanyId = Convert.ToInt32(this.ddlCompany.SelectedValue);
            if (hfCmpSearch.Value != string.Empty)
            {
                salesCallBO.CompanyId = Convert.ToInt32(hfCmpSearch.Value);
            }
            else
            {
                int tmpCompanyId = 0;
                GuestCompanyBO companyBO = new GuestCompanyBO();
                GuestCompanyDA companyDA = new GuestCompanyDA();

                companyBO.CompanyName = Request.Form["cmpName"];
                companyBO.CompanyAddress = this.txtNlCompanyAddress.Text;
                companyBO.EmailAddress = this.txtNllblEmailAddress.Text;
                companyBO.WebAddress = this.txtNlWebAddress.Text;
                companyBO.ContactNumber = this.txtNlContactNumber.Text;
                companyBO.ContactPerson = this.txtNlContactPerson.Text;
                companyBO.TelephoneNumber = this.txtNlTelephoneNumber.Text;
                companyBO.SignupStatus = "Prospective";
                companyBO.CompanyOwnerId = ddlReferenceId.SelectedValue != "0" ? Convert.ToInt32(ddlReferenceId.SelectedValue) : 0;
                companyBO.IndustryId = ddlIndustryId.SelectedValue != "0" ? Convert.ToInt32(ddlIndustryId.SelectedValue) : 0;
                HotelCompanyContactDetailsView detailsView = new HotelCompanyContactDetailsView();

                Boolean status = companyDA.SaveGuestCompanyInfo(companyBO, out tmpCompanyId);

                salesCallBO.CompanyId = tmpCompanyId;
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                        EntityTypeEnum.EntityType.GuestCompany.ToString(), tmpCompanyId,
                        ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestCompany));
                }
            }

            //int parsedValue;
            //if (!int.TryParse(txtProbableInitialHour.Text, out parsedValue))
            //{
            //    CommonHelper.AlertInfo(innboardMessage, "Wrong input for Initial Date Probable Time", AlertType.Warning);
            //    return;
            //}
            //else
            //{

            //}

            //if (!int.TryParse(txtProbableFollowupHour.Text, out parsedValue))
            //{
            //    CommonHelper.AlertInfo(innboardMessage, "Wrong input for Follow-up Date Probable Time", AlertType.Warning);
            //    return;
            //}
            //else
            //{

            //}

            //int pIHour = this.ddlProbableInitialAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtProbableInitialHour.Text) % 12) : ((Convert.ToInt32(this.txtProbableInitialHour.Text) % 12) + 12);


            if (salesCallBO.CompanyId > 0)
            {
                if (!string.IsNullOrWhiteSpace(hfProbableInitialHour.Value))
                {
                    string InitialTime = (hfProbableInitialHour.Value.Replace("AM", "")).Replace("PM", "");
                    string[] IniTime = InitialTime.Split(':');
                    int pIHour = Convert.ToInt32(IniTime[0]);
                    int pIMin = Convert.ToInt32(IniTime[1]);
                    salesCallBO.InitialDate = hmUtility.GetDateTimeFromString(hfInitialDate.Value, userInformationBO.ServerDateFormat).AddHours(pIHour).AddMinutes(pIMin);
                }
                else
                {
                    string InitialTime = (txtProbableInitialHour.Text.Replace("AM", "")).Replace("PM", "");
                    string[] IniTime = InitialTime.Split(':');
                    int pIHour = Convert.ToInt32(IniTime[0]);
                    int pIMin = Convert.ToInt32(IniTime[1]);
                    salesCallBO.InitialDate = hmUtility.GetDateTimeFromString(this.txtInitialDate.Text, userInformationBO.ServerDateFormat).AddHours(pIHour).AddMinutes(pIMin);
                }
            }
            else
            {
                string InitialTime = (txtProbableInitialHour.Text.Replace("AM", "")).Replace("PM", "");
                string[] IniTime = InitialTime.Split(':');
                int pIHour = Convert.ToInt32(IniTime[0]);
                int pIMin = Convert.ToInt32(IniTime[1]);
                salesCallBO.InitialDate = hmUtility.GetDateTimeFromString(this.txtInitialDate.Text, userInformationBO.ServerDateFormat).AddHours(pIHour).AddMinutes(pIMin);
            }

            //int pFHour = this.ddlProbableFollowupAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtProbableFollowupHour.Text) % 12) : ((Convert.ToInt32(this.txtProbableFollowupHour.Text) % 12) + 12);
            string FollowUpTime = (txtProbableFollowupHour.Text.Replace("AM", "")).Replace("PM", "");
            string[] FolTime = FollowUpTime.Split(':');
            int pFHour = Convert.ToInt32(FolTime[0]);
            int pFMin = Convert.ToInt32(FolTime[1]);
            salesCallBO.FollowupDate = hmUtility.GetDateTimeFromString(this.txtFollowupDate.Text, userInformationBO.ServerDateFormat).AddHours(pFHour).AddMinutes(pFMin);

            salesCallBO.Remarks = this.txtRemarks.Text;
            salesCallBO.LocationId = this.ddlLocation.SelectedIndex == 0 ? 0 : Convert.ToInt32(this.ddlLocation.SelectedValue);
            salesCallBO.CityId = this.ddlCity.SelectedIndex == 0 ? 0 : Convert.ToInt32(this.ddlCity.SelectedValue);
            //salesCallBO.IndustryId = this.ddlIndustry.SelectedIndex == 0 ? 0 : Convert.ToInt32(this.ddlIndustry.SelectedValue);
            salesCallBO.FollowupTypeId = ddlFollowupType.SelectedIndex == 0 ? 0 : Convert.ToInt32(ddlFollowupType.SelectedValue);
            salesCallBO.FollowupType = this.txtFollowupName.Text;
            salesCallBO.PurposeId = ddlPurpose.SelectedIndex == 0 ? 0 : Convert.ToInt32(ddlPurpose.SelectedValue);
            salesCallBO.Purpose = this.txtPurposeName.Text;

            salesCallBO.SiteId = Convert.ToInt32(hfCompanySiteId.Value);
            salesCallBO.CITypeId = ddlCIType.SelectedIndex == 0 ? 0 : Convert.ToInt32(ddlCIType.SelectedValue);
            salesCallBO.ActionPlanId = ddlActionPlan.SelectedIndex == 0 ? 0 : Convert.ToInt32(ddlActionPlan.SelectedValue);
            salesCallBO.OpportunityStatusId = ddlOpportunityStatus.SelectedIndex == 0 ? 0 : Convert.ToInt32(ddlOpportunityStatus.SelectedValue);

            List<SMCompanySalesCallDetailBO> addList = new List<SMCompanySalesCallDetailBO>();
            List<SMCompanySalesCallDetailBO> deleteList = new List<SMCompanySalesCallDetailBO>();

            addList = JsonConvert.DeserializeObject<List<SMCompanySalesCallDetailBO>>(hfSaveObj.Value);
            deleteList = JsonConvert.DeserializeObject<List<SMCompanySalesCallDetailBO>>(hfDeleteObj.Value);

            if (string.IsNullOrEmpty(hfSalesCallId.Value))
            {
                int tmpSalesCallId = 0;
                salesCallBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = salesCallDA.SaveSalesCallInfo(salesCallBO, addList, out tmpSalesCallId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.SalesCall.ToString(), tmpSalesCallId, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalesCall));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    this.Cancel();
                }
            }
            else
            {
                salesCallBO.SalesCallId = Convert.ToInt32(hfSalesCallId.Value);
                salesCallBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = salesCallDA.UpdateSalesCallInfo(salesCallBO, deleteList, addList);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.SalesCall.ToString(), salesCallBO.SalesCallId, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalesCall));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    this.Cancel();
                }
            }
        }
        //************************ User Defined Function ********************//
        private void LoadFollowUpType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            List<CustomFieldBO> searchFields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("Sales&MarketingFollowUpType", hmUtility.GetDropDownFirstValue());
            searchFields = commonDA.GetCustomField("Sales&MarketingFollowUpType", hmUtility.GetDropDownFirstAllValue());

            this.ddlFollowupType.DataSource = fields;
            this.ddlFollowupType.DataTextField = "FieldValue";
            this.ddlFollowupType.DataValueField = "FieldId";
            this.ddlFollowupType.DataBind();


            this.ddlSFollowupType.DataSource = searchFields;
            this.ddlSFollowupType.DataTextField = "FieldValue";
            this.ddlSFollowupType.DataValueField = "FieldId";
            this.ddlSFollowupType.DataBind();
        }
        private void LoadPurpose()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            List<CustomFieldBO> searchFields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("Sales&MarketingPurpose", hmUtility.GetDropDownFirstValue());
            searchFields = commonDA.GetCustomField("Sales&MarketingPurpose", hmUtility.GetDropDownFirstAllValue());

            this.ddlPurpose.DataSource = fields;
            this.ddlPurpose.DataTextField = "FieldValue";
            this.ddlPurpose.DataValueField = "FieldId";
            this.ddlPurpose.DataBind();

            this.ddlSPurpose.DataSource = searchFields;
            this.ddlSPurpose.DataTextField = "FieldValue";
            this.ddlSPurpose.DataValueField = "FieldId";
            this.ddlSPurpose.DataBind();
        }
        private void LoadCIType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();

            fields = commonDA.GetCustomField("CIType", hmUtility.GetDropDownFirstValue());

            this.ddlCIType.DataSource = fields;
            this.ddlCIType.DataTextField = "FieldValue";
            this.ddlCIType.DataValueField = "FieldId";
            this.ddlCIType.DataBind();
        }
        private void LoadActionPlan()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("ActionPlan", hmUtility.GetDropDownFirstValue());

            this.ddlActionPlan.DataSource = fields;
            this.ddlActionPlan.DataTextField = "FieldValue";
            this.ddlActionPlan.DataValueField = "FieldId";
            this.ddlActionPlan.DataBind();
        }
        private void LoadOpportunityStatus()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("OpportunityStatus", hmUtility.GetDropDownFirstValue());

            this.ddlOpportunityStatus.DataSource = fields;
            this.ddlOpportunityStatus.DataTextField = "FieldValue";
            this.ddlOpportunityStatus.DataValueField = "FieldId";
            this.ddlOpportunityStatus.DataBind();
        }
        private void LoadCompany()
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<GuestCompanyBO> companyBOList = new List<GuestCompanyBO>();
            companyBOList = companyDA.GetGuestCompanyInfo();

            this.ddlCompany.DataSource = companyBOList;
            this.ddlCompany.DataTextField = "CompanyName";
            this.ddlCompany.DataValueField = "CompanyId";
            this.ddlCompany.DataBind();


            this.ddlSCompany.DataSource = companyBOList;
            this.ddlSCompany.DataTextField = "CompanyName";
            this.ddlSCompany.DataValueField = "CompanyId";
            this.ddlSCompany.DataBind();

            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //this.ddlCompany.Items.Insert(0, item);
        }
        private void SetDefaulTime()
        {
            this.txtProbableInitialHour.Text = "12:00";
            //this.txtProbableInitialMinute.Text = "00";
            //this.ddlProbableInitialAMPM.SelectedIndex = 1;

            this.txtProbableFollowupHour.Text = "12:00";
            //this.txtProbableFollowupMinute.Text = "00";
            //this.ddlProbableFollowupAMPM.SelectedIndex = 1;
        }
        private void LoadEmployee()
        {
            EmployeeDA employeeDA = new EmployeeDA();
            List<EmployeeBO> employeeBOList = new List<EmployeeBO>();
            employeeBOList = employeeDA.GetEmployeeInfo();

            this.ddlEmployee.DataSource = employeeBOList;
            this.ddlEmployee.DataTextField = "EmployeeName";
            this.ddlEmployee.DataValueField = "EmpId";
            this.ddlEmployee.DataBind();

            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //this.ddlEmployee.Items.Insert(0, item);
        }
        private void LoadLocation()
        {
            LocationDA locationDA = new LocationDA();
            List<LocationBO> locationBO = new List<LocationBO>();
            locationBO = locationDA.GetLocationInfo();

            this.ddlLocation.DataSource = locationBO;
            this.ddlLocation.DataTextField = "LocationName";
            this.ddlLocation.DataValueField = "LocationId";
            this.ddlLocation.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlLocation.Items.Insert(0, item);
        }
        private void LoadCity()
        {
            CityDA cityDA = new CityDA();
            List<CityBO> cityBO = new List<CityBO>();
            cityBO = cityDA.GetCityInfo();

            this.ddlCity.DataSource = cityBO;
            this.ddlCity.DataTextField = "CityName";
            this.ddlCity.DataValueField = "CityId";
            this.ddlCity.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCity.Items.Insert(0, item);
        }
        private void LoadIndustryForEnlistedCompany()
        {
            IndustryDA industryDA = new IndustryDA();
            List<IndustryBO> industryBO = new List<IndustryBO>();
            industryBO = industryDA.GetIndustryInfo();

            this.ddlIndustry.DataSource = industryBO;
            this.ddlIndustry.DataTextField = "IndustryName";
            this.ddlIndustry.DataValueField = "IndustryId";
            this.ddlIndustry.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlIndustry.Items.Insert(0, item);
        }
        private void LoadReferenceForEnlistedCompany()
        {
            GuestReferenceDA entityDA = new GuestReferenceDA();
            List<GuestReferenceBO> files = entityDA.GetAllGuestRefference();
            ddlReference.DataSource = files;
            ddlReference.DataTextField = "Name";
            ddlReference.DataValueField = "ReferenceId";
            ddlReference.DataBind();

            ListItem itemReference = new ListItem();
            itemReference.Value = "0";
            itemReference.Text = hmUtility.GetDropDownFirstValue();
            this.ddlReference.Items.Insert(0, itemReference);
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadIndustry()
        {
            IndustryDA industryDA = new IndustryDA();
            List<IndustryBO> industryBO = new List<IndustryBO>();
            industryBO = industryDA.GetIndustryInfo();

            this.ddlIndustryId.DataSource = industryBO;
            this.ddlIndustryId.DataTextField = "IndustryName";
            this.ddlIndustryId.DataValueField = "IndustryId";
            this.ddlIndustryId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlIndustryId.Items.Insert(0, item);
        }
        private void LoadReference()
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
        private void Cancel()
        {
            this.ddlCompany.SelectedValue = null;
            this.ddlEmployee.SelectedValue = null;
            this.txtRemarks.Text = string.Empty;
            this.ddlFollowupType.SelectedValue = null;
            this.ddlPurpose.SelectedValue = null;
            this.txtInitialDate.Text = string.Empty;
            this.txtFollowupDate.Text = string.Empty;
            this.hfDeleteObj.Value = string.Empty;
            this.hfSaveObj.Value = string.Empty;
            this.ddlLocation.SelectedIndex = 0;
            this.ddlCity.SelectedIndex = 0;
            this.ddlIndustry.SelectedIndex = 0;
            this.hfCmpSearch.Value = string.Empty;
            this.txtProbableInitialHour.Text = "12:00";
            this.txtProbableFollowupHour.Text = "12:00";
            this.txtNlCompanyAddress.Text = string.Empty;
            this.txtNlContactNumber.Text = string.Empty;
            this.txtNlContactPerson.Text = string.Empty;
            this.txtNllblEmailAddress.Text = string.Empty;
            this.txtNlTelephoneNumber.Text = string.Empty;
            this.txtNlWebAddress.Text = string.Empty;
            this.txtFollowupName.Text = string.Empty;
            this.txtPurposeName.Text = string.Empty;
            hfCompanySiteId.Value = "0";
            ddlCIType.SelectedValue = "0";
            ddlOpportunityStatus.SelectedValue = "0";
            ddlActionPlan.SelectedValue = "0";
        }
        private void LoadSalesCallNotification()
        {
            List<SalesCallViewBO> viewList = new List<SalesCallViewBO>();
            SalesCallDA salescallDA = new SalesCallDA();

            string todaysdate = DateTime.Now.ToShortDateString();

            UserInformationDA userInformationDA = new UserInformationDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            viewList = salescallDA.GetAllSalesCallInfo();
            viewList = viewList.Where(a => a.ShowFollowupDate == todaysdate).ToList();

            if (viewList != null)
            {
                if (viewList.Count > 0)
                {
                    foreach (SalesCallViewBO bo in viewList)
                    {
                        UserInformationBO participant = new UserInformationBO();
                        participant = userInformationDA.GetUserInformationByEmpId(bo.EmpId);


                        bool IsMessageSendAllGroupUser = false;

                        CommonMessageDA messageDa = new CommonMessageDA();
                        CommonMessageBO message = new CommonMessageBO();
                        CommonMessageDetailsBO detailBO = new CommonMessageDetailsBO();
                        List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();

                        message.Subjects = "Followup Notification";
                        message.MessageBody = "You have a following followup today. ";
                        message.MessageBody += "Company: " + bo.CompanyName;
                        message.MessageBody += " Address: " + bo.CompanyAddress;
                        message.MessageBody += " Followup Type: " + bo.FollowupType;
                        message.MessageBody += " Purpose: " + bo.Purpose;
                        message.MessageFrom = userInformationBO.UserInfoId;
                        message.MessageFromUserId = userInformationBO.UserId;
                        message.MessageDate = DateTime.Now;
                        message.Importance = "Normal";

                        detailBO.MessageTo = participant.UserInfoId;
                        detailBO.UserId = participant.UserId;
                        messageDetails.Add(detailBO);

                        bool status = messageDa.SaveMessage(message, messageDetails, IsMessageSendAllGroupUser);
                        if (status)
                        {
                            (this.Master as HM).MessageCount();
                        }
                    }
                }
            }
        }
        //************************ User Defined WebMethod ********************//
        [WebMethod]
        public static GridViewDataNPaging<SMCompanySalesCallBO, GridPaging> SearchSalesCallInfo(string companyName, string fromIniDate, string toIniDate, string fromFolupDate, string toFolupDate, string folUpTypeId, string purposeId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int? cmpId = 0; DateTime? frmIDate = null, toIDate = null, frmFDate = null, toFDate = null;
            int? folupId, purId;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<SMCompanySalesCallBO, GridPaging> myGridData = new GridViewDataNPaging<SMCompanySalesCallBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (!string.IsNullOrWhiteSpace(fromIniDate))
            {
                //frmIDate = Convert.ToDateTime(fromIniDate);
                frmIDate = CommonHelper.DateTimeToMMDDYYYY(fromIniDate);
            }
            if (!string.IsNullOrWhiteSpace(toIniDate))
            {
                //toIDate = Convert.ToDateTime(toIniDate);
                toIDate = CommonHelper.DateTimeToMMDDYYYY(toIniDate);
            }
            if (!string.IsNullOrWhiteSpace(fromFolupDate))
            {
                //frmFDate = Convert.ToDateTime(fromFolupDate);
                frmFDate = CommonHelper.DateTimeToMMDDYYYY(fromFolupDate);
            }
            if (!string.IsNullOrWhiteSpace(toFolupDate))
            {
                //toFDate = Convert.ToDateTime(toFolupDate);
                toIDate = CommonHelper.DateTimeToMMDDYYYY(toFolupDate);
            }
            if (folUpTypeId == "0")
            {
                folupId = null;
            }
            else
            {
                folupId = Convert.ToInt32(folUpTypeId);
            }
            if (purposeId == "0")
            {
                purId = null;
            }
            else
            {
                purId = Convert.ToInt32(purposeId);
            }

            HMCommonDA commonDA = new HMCommonDA();
            SalesCallDA salesCallDA = new SalesCallDA();
            List<SMCompanySalesCallBO> salesCallList = new List<SMCompanySalesCallBO>();
            salesCallList = salesCallDA.GetSalesCallInfoBySearchCriteriaForPaging(companyName, frmIDate, toIDate, frmFDate, toFDate, folupId, purId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<SMCompanySalesCallBO> distinctItems = new List<SMCompanySalesCallBO>();
            distinctItems = salesCallList.GroupBy(test => test.SalesCallId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;

        }
        [WebMethod]
        public static SMCompanySalesCallBO LoadDetailInformation(int salesCallId)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            SalesCallDA salesDA = new SalesCallDA();
            SMCompanySalesCallBO bo = salesDA.GetSalesCallInfoById(salesCallId);
            bo.InitialTime = bo.InitialDate.ToString(userInformationBO.TimeFormat);
            bo.FollowupTime = bo.FollowupDate.ToString(userInformationBO.TimeFormat);
            return bo;
        }
        [WebMethod]
        public static ReturnInfo DeleteSalesCallInfo(int salesCallId)
        {
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                SalesCallDA salesCallDA = new SalesCallDA();
                Boolean status = salesCallDA.DeleteSalesCallInfo(salesCallId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                
            }

            return rtninf;
        }
        [WebMethod]
        public static GuestCompanyBO LoadCompanyInfo(int companyId)
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            return companyDA.GetGuestCompanyInfoForSalesCallById(companyId);
        }
        [WebMethod]
        public static ArrayList PopulateLocations(string cityId)
        {
            ArrayList list = new ArrayList();
            List<LocationBO> projectList = new List<LocationBO>();
            LocationDA entityDA = new LocationDA();
            projectList = entityDA.GetLocationInfoByCityId(Convert.ToInt32(cityId));
            int count = projectList.Count;
            for (int i = 0; i < count; i++)
            {
                list.Add(new ListItem(
                                        projectList[i].LocationName.ToString(),
                                        projectList[i].LocationId.ToString()
                                         ));
            }
            return list;
        }

        [WebMethod]
        public static List<CompanySiteBO> LoadCompanySite(int companyId)
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<CompanySiteBO> siteList = new List<CompanySiteBO>();
            siteList = companyDA.GetCompanySiteByCompanyId(companyId);

            return siteList;
        }
    }
}