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
    public partial class ContactInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                long contactId = Convert.ToInt64(Request.QueryString["conid"]);
                hfContactId.Value = contactId.ToString();
                //FileUpload(contactId);
            }
        }

        private void FileUpload(long contactId)
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            //flashUpload.QueryParameters = "ContactDocId=" + Server.UrlEncode(contactId.ToString());
        }
        [WebMethod]
        public static SMContactInformationViewBO FillForm(int id)
        {
            ContactInformationDA contactDA = new ContactInformationDA();
            SMContactInformationViewBO contactInformation = new SMContactInformationViewBO();

            contactInformation = contactDA.GetContactInformationByIdForView(id);

            contactInformation.Documents = new DocumentsDA().GetDocumentsByUserTypeAndUserId("ContactDocument", id);
            contactInformation.Documents = new HMCommonDA().GetDocumentListWithIcon(contactInformation.Documents);

            return contactInformation;
        }
        [WebMethod]
        public static List<DocumentsBO> LoadContactDocument(long id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("ContactDocument", id);
            
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }
        [WebMethod]
        public static ReturnInfo DeleteContactDocument(long documentId)
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
        [WebMethod]
        public static GuestCompanyBO GetPreviousCompanyInfoById(int id)
        {
            GuestCompanyBO guestCompanyBO = new GuestCompanyBO();
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();

            guestCompanyBO = guestCompanyDA.GetGuestCompanyInfoById(id);

            return guestCompanyBO;
        }
    }
}