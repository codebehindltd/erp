using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.SalesAndMarketing;
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
    public partial class CompanyInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = Convert.ToInt32(Request.QueryString["id"]);
                //FileUpload(id);
                LoadCRMConfiguration();
                //FillForm(id);

            }
        }
        private void LoadCRMConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsShippingAddresswillshow", "IsShippingAddresswillshow");
            DivShippingAddress.Visible = setUpBO.SetupValue == "1";

            //setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDiscountPercentageWillShow", "IsDiscountPercentageWillShow");
            //DivDiscountPercentage.Visible = setUpBO.SetupValue == "1";

            //setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCreditLimitWillShow", "IsCreditLimitWillShow");
            //DivCreditLimit.Visible = setUpBO.SetupValue == "1";

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsNumberOfEmployeeWillShow", "IsNumberOfEmployeeWillShow");
            DivNoOfEmployee.Visible = setUpBO.SetupValue == "1";

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsAnnualRevenueWillShow", "IsAnnualRevenueWillShow");
            DivAnnualRevenue.Visible = setUpBO.SetupValue == "1";
        }
        private void FileUpload(int contactId)
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            //flashUpload.QueryParameters = "CompanyDocId=" + Server.UrlEncode(contactId.ToString());

            //flashUpload.QueryParameters = "InventoryProductId=" + Server.UrlEncode(RandomProductId.Value) + "~" + txtCode.Text;
        }
        private void FillForm(int id)
        {
            GuestCompanyBO guestCompanyBO = new GuestCompanyBO();
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();

            guestCompanyBO = guestCompanyDA.GetGuestCompanyInfoById(id);
            lblCompanyName.Text = guestCompanyBO.CompanyName;
            lblCompanyEmail.Text = guestCompanyBO.EmailAddress;            
            lblWebAddress.Text = guestCompanyBO.WebAddress;            
            lblNumberOfEmployee.Text = guestCompanyBO.NumberOfEmployee.ToString();
            lblAnnualRevenue.Text = guestCompanyBO.AnnualRevenue.ToString();

        }
        [WebMethod]
        public static ReturnInfo SaveAddedContacts(List<ContactInformationBO> contacts, int companyId)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo returnInfo = new ReturnInfo();
            ContactInformationDA DA = new ContactInformationDA();
            foreach (var item in contacts)
            {
                item.CompanyId = companyId;
            }
            try
            {
                returnInfo.IsSuccess = DA.SaveCompanyWiseContact(contacts);

                if (returnInfo.IsSuccess)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                        EntityTypeEnum.EntityType.ContactInformation.ToString(), contacts[0].Id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), 
                        hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ContactInformation));
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else
                {
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }

            catch (Exception ex)
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return returnInfo;
        }
        [WebMethod]
        public static List<ContactInformationBO> LoadContactsDropdown()
        {
            //ddlContacts
            List<ContactInformationBO> contactInformation = new List<ContactInformationBO>();
            ContactInformationDA DA = new ContactInformationDA();
            contactInformation = DA.GetContactInformation().Where(x => x.CompanyId == 0).ToList();

            return contactInformation;

        }
        [WebMethod]
        public static GuestCompanyBO GetCompanyInfoById(int companyId)
        {
            GuestCompanyBO guestCompanyBO = new GuestCompanyBO();
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();

            guestCompanyBO = guestCompanyDA.GetGuestCompanyInfoById(companyId);

            return guestCompanyBO;
        }
        [WebMethod]
        public static List<SMDeal> GetDealInfoByCompanyId(int companyId)
        {
            List<SMDeal> dealList = new List<SMDeal>();
            DealDA dealDA = new DealDA();

            dealList = dealDA.GetDealInfoByCompanyId(companyId);
            return dealList;
        }
        [WebMethod]
        public static List<ContactInformationBO> GetContactsByCompanyId(int companyId)
        {
            List<ContactInformationBO> contactInformation = new List<ContactInformationBO>();
            ContactInformationDA DA = new ContactInformationDA();
            contactInformation = DA.GetContactInformationByCompanyId(companyId);

            return contactInformation;
        }
        [WebMethod]
        public static ReturnInfo DeleteContact(long deleteId, int companyId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();

            try
            {
                ContactInformationDA contactInformationDA = new ContactInformationDA();

                HMCommonDA hmCommonDA = new HMCommonDA();
                bool status = contactInformationDA.DeleteCompanyFromContact(deleteId);

                if (status)
                {
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                        EntityTypeEnum.EntityType.GuestCompany.ToString(), deleteId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), 
                        hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestCompany));
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

                throw ex;
            }

            return rtninf;
        }
        [WebMethod]
        public static List<DocumentsBO> LoadCompanyDocument(long id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("CompanyDocument", id);

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }
        [WebMethod]
        public static ReturnInfo DeleteCompanyDocument(long documentId)
        {
            ReturnInfo info = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            List<DocumentsBO> docList = new List<DocumentsBO>();

            info.IsSuccess = new DocumentsDA().DeleteDocumentsByDocumentId(documentId);

            if (info.IsSuccess)
            {
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.SMDeal.ToString(), documentId, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
            }
            else
            {
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }
    }
}