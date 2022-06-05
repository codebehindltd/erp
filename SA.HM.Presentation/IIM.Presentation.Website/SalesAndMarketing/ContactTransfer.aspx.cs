using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class ContactTransfer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadContact();
                LoadCompany();
            }
        }

        private void LoadContact()
        {
            List<ContactInformationBO> contactInformation = new List<ContactInformationBO>();
            ContactInformationDA contactDA = new ContactInformationDA();
            contactInformation = contactDA.GetContactInformation();

            ddlContact.DataSource = contactInformation;
            ddlContact.DataTextField = "Name";
            ddlContact.DataValueField = "Id";
            ddlContact.DataBind();
        }
        private void LoadCompany()
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<GuestCompanyBO> companyBOList = new List<GuestCompanyBO>();
            companyBOList = companyDA.GetGuestCompanyInfo();

            ddlCompany.DataSource = companyBOList;
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();
        }

        [WebMethod]
        public static ReturnInfo TransferContact(SMContactTransfer contact)
        {
            SalesMarketingLogType<ContactInformationBO> logDA = new SalesMarketingLogType<ContactInformationBO>();
            ContactInformationDA contactDA = new ContactInformationDA();
            ReturnInfo info = new ReturnInfo();
            long id = 0;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            contact.CreatedBy = userInformationBO.UserInfoId;

            try
            {
                info.IsSuccess = contactDA.TransferContact(contact, true, out id);

                if (info.IsSuccess)
                {
                    ContactInformationBO previousBO = new ContactInformationBO()
                    {
                        Id = contact.ContactId,
                        CompanyId = contact.PreviousCompanyId,
                        CompanyName = contact.PreviousCompany
                    };

                    ContactInformationBO contactInformationBO = new ContactInformationBO()
                    {
                        Id = contact.ContactId,
                        CompanyId = contact.TransferredCompanyId,
                        CompanyName = contact.TransferredCompany
                    };

                    logDA.Log(ConstantHelper.SalesandMarketingLogType.ContactActivity, contactInformationBO, previousBO);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Transfer.ToString()
                                                    , EntityTypeEnum.EntityType.SMContactTransfer.ToString(), id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString()
                                                    , hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMContactTransfer));
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Transfer, AlertType.Success);
                }
                else
                {
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;


        }
        [WebMethod]
        public static ContactInformationBO GetCurrentContactInformation(long contactId)
        {
            ContactInformationDA contactDA = new ContactInformationDA();
            ContactInformationBO contactInformation = new ContactInformationBO();
            contactInformation = contactDA.GetContactInformationById(contactId);

            return contactInformation;
        }
    }
}