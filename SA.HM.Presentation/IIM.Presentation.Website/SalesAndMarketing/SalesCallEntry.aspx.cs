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
using System.Text;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class SalesCallEntry : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                CheckingUserAdminAuthorization();
                //GetContactInformation();
                LoadEmployeeInfo();
                LoadContactDetailsTitle();
                LoadAccountManager();
            }
        }
        private void CheckingUserAdminAuthorization()
        {
            hfIsAdminUser.Value = "0";
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                hfIsAdminUser.Value = "1";
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 22).Count() > 0)
                    {
                        hfIsAdminUser.Value = "1";
                    }
                }
            }
            #endregion
        }

        [WebMethod]
        public static List<ContactInformationBO> GetContactInformationByCompanyId(int id)
        {
            List<ContactInformationBO> contactInformationList = new List<ContactInformationBO>();

            ContactInformationDA DA = new ContactInformationDA();
            List<ContactInformationBO> contactInfo = new List<ContactInformationBO>();
            contactInfo = DA.GetContactInformationByCompanyId(id);
            return contactInfo;
        }
        private void LoadContactDetailsTitle()
        {
            ContactInformationDA DA = new ContactInformationDA();
            List<SMContactDetailsTitleBO> titleList = new List<SMContactDetailsTitleBO>();
            titleList = DA.GetContactDetailsTitleByTransectionType("SocialMedia");

            ddlMessageType.DataSource = titleList;
            ddlMessageType.DataTextField = "Title";
            ddlMessageType.DataValueField = "Id";
            ddlMessageType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlMessageType.Items.Insert(0, item);
        }
        public void GetContactInformation()
        {
            List<ContactInformationBO> contactInformationList = new List<ContactInformationBO>();

            ContactInformationDA DA = new ContactInformationDA();
            List<ContactInformationBO> contactInfo = new List<ContactInformationBO>();
            contactInfo = DA.GetContactInformation();

            ddlParticipantFromClient.DataSource = contactInfo;
            ddlParticipantFromClient.DataTextField = "Name";
            ddlParticipantFromClient.DataValueField = "Id";
            ddlParticipantFromClient.DataBind();

            ddlEmailContacts.DataSource = contactInfo;
            ddlEmailContacts.DataTextField = "Name";
            ddlEmailContacts.DataValueField = "Id";
            ddlEmailContacts.DataBind();

            ddlCallContacts.DataSource = contactInfo;
            ddlCallContacts.DataTextField = "Name";
            ddlCallContacts.DataValueField = "Id";
            ddlCallContacts.DataBind();
        }
        private void LoadEmployeeInfo()
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();

            empList = empDa.GetEmployeeInfo();
            ddltxtParticipantFromOffice.DataSource = empList;
            ddltxtParticipantFromOffice.DataTextField = "DisplayName";
            ddltxtParticipantFromOffice.DataValueField = "EmpId";
            ddltxtParticipantFromOffice.DataBind();

            ddlCallParticipantFromOffice.DataSource = empList;
            ddlCallParticipantFromOffice.DataTextField = "DisplayName";
            ddlCallParticipantFromOffice.DataValueField = "EmpId";
            ddlCallParticipantFromOffice.DataBind();

            ddlEmailParticipantFromOffice.DataSource = empList;
            ddlEmailParticipantFromOffice.DataTextField = "DisplayName";
            ddlEmailParticipantFromOffice.DataValueField = "EmpId";
            ddlEmailParticipantFromOffice.DataBind();

            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            ddltxtParticipantFromOffice.Items.Insert(0, FirstItem);
            ddlCallParticipantFromOffice.Items.Insert(0, FirstItem);
            ddlEmailParticipantFromOffice.Items.Insert(0, FirstItem);
        }
        private void LoadAccountManager()
        {
            int isAdminUser = 0;
            ddlCompanyOwner.Enabled = true;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            AccountManagerDA accountManagerDA = new AccountManagerDA();
            List<AccountManagerBO> accountManagerBOList = new List<AccountManagerBO>();

            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                isAdminUser = 1;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 22).Count() > 0)
                    {
                        isAdminUser = 1;
                    }
                }
            }
            #endregion

            accountManagerBOList = accountManagerDA.GetAccountManager(isAdminUser, "CRM", userInformationBO.UserInfoId);

            ddlCompanyOwner.DataSource = accountManagerBOList;
            ddlCompanyOwner.DataTextField = "DisplayName";
            ddlCompanyOwner.DataValueField = "UserInfoId";
            ddlCompanyOwner.DataBind();

            if (accountManagerBOList != null)
            {
                if (accountManagerBOList.Count > 1)
                {
                    ListItem item = new ListItem();
                    item.Value = "0";
                    item.Text = hmUtility.GetDropDownFirstValue();
                    ddlCompanyOwner.Items.Insert(0, item);
                }
                else
                {
                    ddlCompanyOwner.Enabled = false;
                }
            }

            ddlCompanyOwner.SelectedValue = userInformationBO.UserInfoId.ToString();
        }

        [WebMethod]
        public static ReturnInfo SaveLogEntry(SalesCallEntryBO salesCall, List<SalesCallParticipantBO> participantFromCompany,
                                              List<SalesCallParticipantBO> participantFromClient, List<SalesCallParticipantBO> deletedClientPerticipent)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;
            ConstantHelper.SalesandMarketingLogType logtype = new ConstantHelper.SalesandMarketingLogType();
            StringBuilder companyParticipant = new StringBuilder();
            StringBuilder clientParticipant = new StringBuilder();
            StringBuilder companyParticipantIdList = new StringBuilder();
            StringBuilder clientParticipantIdList = new StringBuilder();
            StringBuilder logged = new StringBuilder();
            long salesCallEntryId = 0;
            salesCall.ParticipantFromClient = participantFromClient;
            try
            {
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsPurchaseOrderApprovalEnable", "IsPurchaseOrderApprovalEnable");

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                //if (salesCall.Id == 0)
                //{
                //    salesCall.LogDate = DateTime.Now;
                //}
                //else
                //{
                //    if (salesCall.IsAdminUser == "0")
                //    {
                //        salesCall.LogDate = DateTime.Now;
                //    }
                //    else
                //    { }
                //}

                //salesCall.LogDate = DateTime.Now;
                if (salesCall.AccountManagerId <= 0)
                {
                    salesCall.AccountManagerId = userInformationBO.UserInfoId;
                }
                SalesCallDA callDa = new SalesCallDA();
                SalesMarketingLogType<SalesCallEntryBO> logDA = new SalesMarketingLogType<SalesCallEntryBO>();

                foreach (SalesCallParticipantBO p in participantFromCompany)
                {
                    companyParticipant.Append(p.Contact + ", ");
                    companyParticipantIdList.Append(p.ContactId + ", ");
                }

                foreach (SalesCallParticipantBO p in participantFromClient)
                {
                    clientParticipant.Append(p.Contact + ", ");
                    clientParticipantIdList.Append(p.ContactId + ", ");
                }
                if (companyParticipant.Length > 0)
                    companyParticipant.Replace(',', ' ', companyParticipant.Length - 2, 1);
                if (clientParticipant.Length > 0)
                    clientParticipant.Replace(',', ' ', clientParticipant.Length - 2, 1);

                if (salesCall.LogType == "Log an email")
                {
                    if (salesCall.EmailType == "Send Mail")
                    {
                        logtype = ConstantHelper.SalesandMarketingLogType.LoggedEmail;
                        salesCall.LogDescription = salesCall.LogBody +
                                                   "|Email date: " + Convert.ToDateTime(salesCall.MeetingDate).ToString("dd/MM/yyyy  HH:mm") +
                                                   "~" + userInformationBO.UserName + " logged an email to " + clientParticipant.ToString() +
                                                   "~On " + salesCall.LogDate.ToString("dd/MM/yyyy HH:mm");
                    }
                    else
                    {
                        logtype = ConstantHelper.SalesandMarketingLogType.LoggedEmail;
                        salesCall.LogDescription = salesCall.LogBody +
                                                   "|Email date: " + Convert.ToDateTime(salesCall.MeetingDate).ToString("dd/MM/yyyy  HH:mm") +
                                                   "~" + userInformationBO.UserName + " logged an email from " + clientParticipant.ToString() +
                                                   "~On " + salesCall.LogDate.ToString("dd/MM/yyyy HH:mm");
                    }
                }
                else if (salesCall.LogType == "Log a call")
                {
                    logtype = ConstantHelper.SalesandMarketingLogType.LoggedCall;
                    salesCall.LogDescription = logged.ToString();
                    salesCall.LogDescription = salesCall.LogBody +
                                               "|Call log status: " + salesCall.CallStatus +
                                               "~Call date: " + Convert.ToDateTime(salesCall.MeetingDate).ToString("dd/MM/yyyy  HH:mm") +
                                               "~" + userInformationBO.UserName + " logged a call for " + clientParticipant.ToString() +
                                               "~On " + salesCall.LogDate.ToString("dd/MM/yyyy HH:mm");
                }
                else if (salesCall.LogType == "Log a message")
                {
                    logtype = ConstantHelper.SalesandMarketingLogType.LoggedMessage;
                    salesCall.LogDescription = logged.ToString();
                    salesCall.LogDescription = salesCall.LogBody +
                                               "|Message log status: " + salesCall.CallStatus +
                                               "~Messenger Id: " + salesCall.MessagengerId +
                                               "~Message date: " + Convert.ToDateTime(salesCall.MeetingDate).ToString("dd/MM/yyyy  HH:mm") +
                                               "~" + userInformationBO.UserName + " Logged a message for " + clientParticipant.ToString() +
                                               "~On " + salesCall.LogDate.ToString("dd/MM/yyyy HH:mm");
                }
                else if (salesCall.LogType == "Log a meeting")
                {
                    logtype = ConstantHelper.SalesandMarketingLogType.LoggedMeeting;
                    salesCall.LogDescription = "Logged a meeting By " + userInformationBO.UserName +
                                                                  "|Meeting date: " + Convert.ToDateTime(salesCall.MeetingDate).ToString("dd/MM/yyyy  HH:mm") +
                                                                  "|Meeting type: " + salesCall.MeetingType +
                                                                  "|Meeting location: " + salesCall.MeetingLocation +
                                                                  "|Meeting agenda: " + salesCall.MeetingAgenda +
                                                                  "|Meeting discussion: " + salesCall.LogBody +
                                                                  "|Meeting action: " + salesCall.MeetingAfterAction +
                                                                  "|Participant from office: " + companyParticipant.ToString() +
                                                                  "|Participant from client: " + salesCall.ParticipantFromParty + ". " + clientParticipant.ToString() +
                                                                  "|On " + salesCall.LogDate.ToString("dd/MM/yyyy HH:mm");
                }

                if (salesCall.Id == 0)
                {
                    salesCall.CreatedBy = userInformationBO.UserInfoId;
                    status = callDa.SaveSalesCallEntry(salesCall, participantFromCompany, participantFromClient, out salesCallEntryId);
                    salesCall.Id = salesCallEntryId;
                    rtninf.Data = salesCallEntryId;
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.SalesCall.ToString(), salesCallEntryId,
                            ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalesCall));

                        logDA.Log(logtype, salesCall, salesCall);

                    }

                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }
                else
                {
                    SalesCallEntryView previousSalesCall;
                    previousSalesCall = callDa.GetSalesCallById(salesCall.Id);

                    List<long> preCompanyParticipants = previousSalesCall.participants.Where(i => i.PrticipantType == "CompanyParticipant")
                                                            .Select(i => i.ContactId).ToList();
                    List<long> preClientParticipants = previousSalesCall.participants.Where(i => i.PrticipantType == "ClientParticipant")
                                                            .Select(i => i.ContactId).ToList();
                    List<long> participantForDelete = new List<long>();

                    participantFromCompany = participantFromCompany.Where(i => !preCompanyParticipants.Contains(i.ContactId)).ToList();
                    participantFromClient = participantFromClient.Where(i => !preClientParticipants.Contains(i.ContactId)).ToList();

                    salesCall.LastModifiedBy = userInformationBO.UserInfoId;
                    status = callDa.UpdateSalesCallEntry(salesCall, participantFromCompany, participantFromClient, companyParticipantIdList, clientParticipantIdList);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.SalesCall.ToString(), salesCall.Id,
                            ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalesCall));

                        logDA.Log(logtype, salesCall, previousSalesCall);

                        foreach (var item in deletedClientPerticipent)
                        {
                            callDa.DeleteSMLogKeepingBySalesCallEntryIdNContactId(salesCall.Id, item.ContactId);
                        }
                    }
                    rtninf.Data = salesCall.Id;
                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }

                if (!status)
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
        public static SalesCallEntryView GetLogId(long id)
        {
            SalesCallEntryView salesCall = new SalesCallEntryView();
            SalesCallDA callDA = new SalesCallDA();

            salesCall = callDA.GetSalesCallById(id);
            salesCall.OfficeParticipants = salesCall.participants.Where(p => p.PrticipantType == "CompanyParticipant").ToList();
            salesCall.participants = salesCall.participants.Where(p => p.PrticipantType == "ClientParticipant").ToList();
            return salesCall;
        }

        [WebMethod]
        public static ReturnInfo DeleteLog(long id)
        {

            ReturnInfo returnInfo = new ReturnInfo();
            SalesCallDA callDA = new SalesCallDA();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            try
            {
                status = callDA.DeleteLog(id);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.SalesCall.ToString(), id,
                            ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalesCall));
                    returnInfo.IsSuccess = true;
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);

                }
                else
                {
                    returnInfo.IsSuccess = false;
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }

            catch (Exception ex)
            {
                returnInfo.IsSuccess = false;
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return returnInfo;
        }
        [WebMethod]
        public static List<GuestCompanyBO> GetCompanyByAutoSearch(string searchTerm)
        {
            List<GuestCompanyBO> companyInfo = new List<GuestCompanyBO>();
            GuestCompanyDA itemDa = new GuestCompanyDA();
            companyInfo = itemDa.GetGuestCompanyInfoByCompanyName(searchTerm);

            return companyInfo;
        }
        [WebMethod]
        public static List<ContactInformationBO> LoadLabelByAutoSearch(string searchTerm)
        {
            ContactInformationDA DA = new ContactInformationDA();
            List<ContactInformationBO> titleList = new List<ContactInformationBO>();
            titleList = DA.GetContactByAutoSearch(searchTerm);
            return titleList;
        }
        [WebMethod]
        public static List<ContactInformationBO> GetEmployeeByCompanyId(int companyId)
        {
            List<ContactInformationBO> contactInformation = new List<ContactInformationBO>();
            ContactInformationDA DA = new ContactInformationDA();
            contactInformation = DA.GetContactInformationByCompanyId(companyId);

            return contactInformation;
        }

        [WebMethod]
        public static List<ContactInformationBO> GetContactByAccountManagerNCompany(string searchTerm, int accountManagerId, int companyId)
        {
            List<ContactInformationBO> IndustryBO = new List<ContactInformationBO>();
            ContactInformationDA itemDa = new ContactInformationDA();
            IndustryBO = itemDa.GetContactByCompanyIdNAccountManager(accountManagerId, companyId, searchTerm);

            return IndustryBO;
        }
        [WebMethod]
        public static List<SMDeal> GetDealByCompanyIdContactIdNAccountManager(string searchText, int contactId, int companyId, int accountManagerId)
        {
            List<SMDeal> IndustryBO = new List<SMDeal>();
            DealDA itemDa = new DealDA();
            IndustryBO = itemDa.GetDealByCompanyIdContactIdNAccountManager(searchText, contactId, companyId, accountManagerId);

            return IndustryBO;
        }
    }
}