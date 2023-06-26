using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
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
    public partial class frmCompanyAccountApproval : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomDocId.Value = seatingId.ToString();
                FileUpload();
            }
        }
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            //flashUpload.QueryParameters = "TaskAssignDocId=" + Server.UrlEncode(RandomDocId.Value);
        }
        [WebMethod]
        public static List<GuestCompanyBO> GetCompanyInfoForAccountApproval(string searchTerm)
        {
            List<GuestCompanyBO> companyInfo = new List<GuestCompanyBO>();
            GuestCompanyDA companyDa = new GuestCompanyDA();
            companyInfo = companyDa.GetCompanyInfoForAccountApproval(searchTerm);
            return companyInfo;
        }
        
        [WebMethod]
        public static GuestCompanyBO GetCompanyBenefitsForFillForm(int companyId)
        {
            GuestCompanyBO companyInfo = new GuestCompanyBO();
            GuestCompanyDA companyDa = new GuestCompanyDA();
            companyInfo = companyDa.GetCompanyBenefitsForFillForm(companyId);
            return companyInfo;
        }
        
        [WebMethod]
        public static List<GuestCompanyBO> GetCompanyLegalActionForFillForm(int companyId)
        {
            List<GuestCompanyBO> companyInfo = new List<GuestCompanyBO>();
            GuestCompanyDA companyDa = new GuestCompanyDA();
            companyInfo = companyDa.GetCompanyLegalActionForFillForm(companyId);
            return companyInfo;
        }
        
        [WebMethod]
        public static GuestCompanyBO GetLastLegalActionId()
        {
            GuestCompanyBO companyInfo = new GuestCompanyBO();
            GuestCompanyDA companyDa = new GuestCompanyDA();
            companyInfo = companyDa.GetLastLegalActionId();
            return companyInfo;
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
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("CompanyAccountApprovalInfo", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("CompanyAccountApprovalInfo", (int)id));

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
        [WebMethod]
        public static ReturnInfo SaveCompanyAccountApprovalInfo(GuestCompanyBO BenefitList, List<GuestCompanyBO> LegalActions, List<int> deletedLegalActionInfoList, int randomDocId, string deletedDoc, bool activeStat)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                GuestCompanyDA gcDa = new GuestCompanyDA();

                BenefitList.AccountsApprovedBy = userInformationBO.UserInfoId;

                int id = 0;
                int OwnerIdForDocuments = 0;
                HMCommonDA hmCommonDA = new HMCommonDA();

                status = gcDa.SaveCompanyAccountApprovalInfo(BenefitList, LegalActions, deletedLegalActionInfoList, activeStat, out id);
                if (status)
                {
                    rtninfo.IsSuccess = true;
                    OwnerIdForDocuments = Convert.ToInt32(id);
                    DocumentsDA documentsDA = new DocumentsDA();
                    string s = deletedDoc;
                    string[] DeletedDocList = s.Split(',');
                    for (int i = 0; i < DeletedDocList.Length; i++)
                    {
                        DeletedDocList[i] = DeletedDocList[i].Trim();
                        Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
                    }
                    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(randomDocId));

                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }

                if (!status)
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            Random rd = new Random();
            int randomId = rd.Next(100000, 999999);
            rtninfo.Data = randomId;
            return rtninfo;
        }
    }
}