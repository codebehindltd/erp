using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.UserInformation;
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
    public partial class DealCreation : BasePage
    {
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDealOwner();
                LoadDealStage();
                LoadCompany();
                LoadCategory();
            }
        }

        private void LoadDealOwner()
        {
            UserInformationDA entityDA = new UserInformationDA();
            List<UserInformationBO> userInformationList = new List<UserInformationBO>();
            userInformationList = entityDA.GetUserInformation();

            ddlSearchDealOwner.DataSource = userInformationList;
            ddlSearchDealOwner.DataTextField = "UserName";
            ddlSearchDealOwner.DataValueField = "UserInfoId";
            ddlSearchDealOwner.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationList.Count > 1)
                ddlSearchDealOwner.Items.Insert(0, item);

            ddlSearchDealOwner.Enabled = false;
            ddlSearchDealOwner.SelectedValue = userInformationBO.UserInfoId.ToString();

            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                ddlSearchDealOwner.Enabled = true;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 22).Count() > 0)
                    {
                        ddlSearchDealOwner.Enabled = true;
                        ddlSearchDealOwner.SelectedValue = ddlSearchDealOwner.Items[0].Value;
                    }
                }
            }
            #endregion
        }

        private void LoadDealStage()
        {
            StageDA stageDA = new StageDA();
            List<SMDealStage> stageList = new List<SMDealStage>();
            stageList = stageDA.GetAllDealStages();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();

            this.ddlSearchDealStage.DataSource = stageList;
            this.ddlSearchDealStage.DataTextField = "DealStage";
            this.ddlSearchDealStage.DataValueField = "Id";
            this.ddlSearchDealStage.DataBind();
            this.ddlSearchDealStage.Items.Insert(0, item);
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
        public static GridViewDataNPaging<SMDeal, GridPaging> LoadGridPaging(int ownerId, string dealNumber, string name, int stageId, int companyId, string dateType, string fromDate, string toDate, int gridRecordsCount, int pageNumber, int IsCurrentOrPreviousPage)
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
            deals = dealDA.GetDealInfoForSearch(ownerId, dealNumber, name, stageId, companyId, dateType, fromDate, toDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(deals, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo DeleteDeal(int Id)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            int id = 0;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            DealDA dealDA = new DealDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            status = dealDA.SoftDeleteDeal(Id);
            if (status)
            {
                info.IsSuccess = true;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                info.Data = 0;
            }
            else
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }
        [WebMethod]
        public static string LoadDealDocument(long id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("SalesDealDocuments", id);
            docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SalesDealFeedbackDocuments", id));
            docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SalesQuotationDocuments", id));
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

    }
}