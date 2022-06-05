using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
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
    public partial class SiteSurveyFeedbackForTechnicalDepartment : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HMCommonDA hmCommonDA = new HMCommonDA();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCompany();
                LoadCategory();
            }
        }

        private void LoadCompany()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = new List<GuestCompanyBO>();
            files = guestCompanyDA.GetALLGuestCompanyInfo();


            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();

            ddlSearchCompany.DataSource = files;
            ddlSearchCompany.DataTextField = "CompanyName";
            ddlSearchCompany.DataValueField = "CompanyId";
            ddlSearchCompany.DataBind();

            ddlSearchCompany.Items.Insert(0, item);
        }

        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllActiveInvItemCatagoryInfoByServiceType("Product");
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();

            List<InvCategoryBO> serviceCategory = new List<InvCategoryBO>();
            serviceCategory = da.GetAllActiveInvItemCatagoryInfoByServiceType("Service");
            
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
        public static GridViewDataNPaging<SMDeal, GridPaging> LoadGridPaging(string dealNumber, string name, string dealStatus, int companyId, string dateType, string fromDate, string toDate, int gridRecordsCount, int pageNumber, int IsCurrentOrPreviousPage)
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;
            if (fromDate != "" && toDate == "")
            {
                toDate = DateTime.Now.ToShortDateString();
            }

            GridViewDataNPaging<SMDeal, GridPaging> myGridData = new GridViewDataNPaging<SMDeal, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, IsCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<SMDeal> deals = new List<SMDeal>();
            DealDA dealDA = new DealDA();
            deals = dealDA.GetDealInfoForSearchForFeedback(dealNumber, name, dealStatus, companyId, dateType, fromDate, toDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(deals, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static GridViewDataNPaging<SMSiteSurveyNoteBO, GridPaging> LoadSiteSurveyForSearch(int dealId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<SMSiteSurveyNoteBO, GridPaging> myGridData = new GridViewDataNPaging<SMSiteSurveyNoteBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<SMSiteSurveyNoteBO> siteSurveyNoteInfo = new List<SMSiteSurveyNoteBO>();
            SiteSurveyNoteDA DA = new SiteSurveyNoteDA();
            siteSurveyNoteInfo = DA.GetSiteSurveyInformationPaginationForFeedback(dealId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

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
        [WebMethod]
        public static ReturnInfo ApproveSiteSurveyNote(long dealId)
        {
            ReturnInfo info = new ReturnInfo();
            SalesMarketingLogType<SMDeal> logDA = new SalesMarketingLogType<SMDeal>();
            bool status = false;
            try
            {
                SMDeal previousBO = new DealDA().GetDealInfoBOById(dealId);
                SiteSurveyNoteDA siteSurveyNoteDA = new SiteSurveyNoteDA();
                status = siteSurveyNoteDA.ApproveSiteSurveyNote(dealId);
                if (status)
                {
                    SMDeal deal = new DealDA().GetDealInfoBOById(dealId);
                    logDA.Log(ConstantHelper.SalesandMarketingLogType.DealActivity, deal, previousBO);
                    info.IsSuccess = true;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                }
                else
                {
                    info.IsSuccess = false;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }
    }
}