using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
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
    public partial class SiteSurveyInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static GridViewDataNPaging<SMSiteSurveyNoteBO, GridPaging> LoadSiteSurveyForSearch(int dealId, int companyId, int contactId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<SMSiteSurveyNoteBO, GridPaging> myGridData = new GridViewDataNPaging<SMSiteSurveyNoteBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<SMSiteSurveyNoteBO> siteSurveyNoteInfo = new List<SMSiteSurveyNoteBO>();
            SiteSurveyNoteDA DA = new SiteSurveyNoteDA();
            siteSurveyNoteInfo = DA.GetSiteSurveyInformationPagination(dealId, companyId, contactId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(siteSurveyNoteInfo, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static string LoadSurveyDocument(long id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("SiteSurveyDoc", id);
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);

            string strTable = "";
            strTable += "<div style='color: White; background-color: #44545E;width:750px;'>";
            int counter = 0;
            foreach (DocumentsBO dr in docList)
            {
                if (dr.Extention == ".jpg")
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:250px; height:250px; float:left;padding:30px'>";
                    strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
                else
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:100px; height:100px; float:left;padding:30px'>";
                    strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img style='width: 100px; height: 100px;' src='" + dr.IconImage + "' alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
            }
            strTable += "</div>";
            if (strTable == "")
            {
                strTable = "<tr><td align='center'>No Record Available!</td></tr>";
            }
            return strTable;

        }
        [WebMethod]
        public static SMSiteSurveyNoteViewBO LoadSurveyDetailsById(long id)
        {
            int CompanyId = 0;
            long ContactId = 0;
            SMSiteSurveyNoteViewBO SiteSurveyNoteView = new SMSiteSurveyNoteViewBO();
            SiteSurveyNoteDA surveyNoteDA = new SiteSurveyNoteDA();
            GuestCompanyDA companyDA = new GuestCompanyDA();
            ContactInformationDA contactDA = new ContactInformationDA();
            DealDA dealDA = new DealDA();

            SiteSurveyNoteView.SiteSurveyNote = surveyNoteDA.GetSiteSurveyNoteDetailsById(id);
            CompanyId = SiteSurveyNoteView.SiteSurveyNote.CompanyId ?? default(int);
            ContactId = SiteSurveyNoteView.SiteSurveyNote.ContactId ?? default(int);

            if (CompanyId != 0)
            {
                SiteSurveyNoteView.GuestCompany = companyDA.GetGuestCompanyInfoById(CompanyId);
            }
            if (ContactId != 0)
            {
                SiteSurveyNoteView.ContactInformation = contactDA.GetContactInformationById(ContactId);
            }
            //SiteSurveyNoteView.Deal = dealDA.GetDealInfoBOById(SiteSurveyNoteView.SiteSurveyNote.DealId ?? default(int));

            return SiteSurveyNoteView;
        }
        [WebMethod]
        public static int CheckFeedback(string id)
        {
            int HasFeedback = 0;
            SiteSurveyNoteDA siteSurveyNoteDA = new SiteSurveyNoteDA();
            HasFeedback = siteSurveyNoteDA.CheckFeedback(Convert.ToInt32(id));
            return HasFeedback;
        }
        [WebMethod]
        public static SMSiteSurveyFeedbackViewBO LoadFeedbackDetailsById(int result)
        {
            SMSiteSurveyFeedbackViewBO feedbackViewBOs = new SMSiteSurveyFeedbackViewBO();
            SiteSurveyNoteDA siteSurveyNoteDA = new SiteSurveyNoteDA();
            feedbackViewBOs.SMSiteSurveyFeedbackBO = siteSurveyNoteDA.GetSiteSurveyFeedbackByFeedbackId(result);
            feedbackViewBOs.SMSiteSurveyFeedbackDetailsBOList = siteSurveyNoteDA.GetSiteSurveyFeedbackDetailsByFeedbackId(result);
            return feedbackViewBOs;
        }
       
    }
}