using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.SupportAndTicket;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.SupportAndTicket;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SupportAndTicket
{
    public partial class SupportCallDetailsIframe : BasePage
    {
        HMUtility hmUtility = new HMUtility();

        public SupportCallDetailsIframe() : base("SupportCallBillingInformation")
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSupportDropDown();
                LoadCaseOwner();
                LoadCategory();

                CheckPermission();

            }
        }

        private void CheckPermission()
        {

            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }

        private void LoadSupportDropDown()
        {
            List<STSupportNCaseSetupInfoBO> SupportNCaseSetupList = new List<STSupportNCaseSetupInfoBO>();
            SupportNCaseSetupDA DA = new SupportNCaseSetupDA();
            SupportNCaseSetupList = DA.SupportNCaseSetupInfoBySetupType("SupportStage");

            ddlSupportStage.DataSource = SupportNCaseSetupList;
            ddlSupportStage.DataTextField = "Name";
            ddlSupportStage.DataValueField = "Id";
            ddlSupportStage.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSupportStage.Items.Insert(0, item);

            SupportNCaseSetupList = DA.SupportNCaseSetupInfoBySetupType("SupportCategory");

            ddlSupportCategory.DataSource = SupportNCaseSetupList;
            ddlSupportCategory.DataTextField = "Name";
            ddlSupportCategory.DataValueField = "Id";
            ddlSupportCategory.DataBind();

            ddlSupportCategory.Items.Insert(0, item);

            SupportNCaseSetupList = DA.SupportNCaseSetupInfoBySetupType("Case");

            ddlCase.DataSource = SupportNCaseSetupList;
            ddlCase.DataTextField = "Name";
            ddlCase.DataValueField = "Id";
            ddlCase.DataBind();


            ddlCase.Items.Insert(0, item);

            SupportNCaseSetupList = DA.SupportNCaseSetupInfoBySetupType("SupportType");

            ddlSupportType.DataSource = SupportNCaseSetupList;
            ddlSupportType.DataTextField = "Name";
            ddlSupportType.DataValueField = "Id";
            ddlSupportType.DataBind();


            ddlSupportType.Items.Insert(0, item);

            SupportNCaseSetupList = DA.SupportNCaseSetupInfoBySetupType("SupportPriority");

            ddlSupportPriority.DataSource = SupportNCaseSetupList;
            ddlSupportPriority.DataTextField = "Name";
            ddlSupportPriority.DataValueField = "Id";
            ddlSupportPriority.DataBind();


            ddlSupportPriority.Items.Insert(0, item);




        }

        private void LoadCaseOwner()
        {
            EmployeeDA EmpDA = new EmployeeDA();
            List<EmployeeBO> CaseOwnerBO = new List<EmployeeBO>();
            CaseOwnerBO = EmpDA.GetEmployeeInfo();

            ddlCaseOwner.DataSource = CaseOwnerBO;
            ddlCaseOwner.DataTextField = "DisplayName";
            ddlCaseOwner.DataValueField = "EmpId";
            ddlCaseOwner.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCaseOwner.Items.Insert(0, item);

            DepartmentDA entityDA = new DepartmentDA();
            ddlSupportForwardTo.DataSource = entityDA.GetDepartmentInfo();
            ddlSupportForwardTo.DataTextField = "Name";
            ddlSupportForwardTo.DataValueField = "DepartmentId";
            ddlSupportForwardTo.DataBind();

            ddlSupportForwardTo.Items.Insert(0, item);


        }

        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllInvItemCatagoryInfoByServiceType("All");
            ddlCategory.DataSource = List;
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();

            ddlCategoryForSupport.DataSource = List;
            ddlCategoryForSupport.DataTextField = "Name";
            ddlCategoryForSupport.DataValueField = "CategoryId";
            ddlCategoryForSupport.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---All---";
            ddlCategory.Items.Insert(0, item);
            ddlCategoryForSupport.Items.Insert(0, item);
        }

        [WebMethod]
        public static List<GuestCompanyBO> ClientSearch(string searchTerm)
        {

            //List<GuestCompanyBO> companyList = new List<GuestCompanyBO>();
            //GuestCompanyDA guestCompanyDA = new GuestCompanyDA();

            //companyList = guestCompanyDA.GetCompanyInfoByNameForAutoComplete(searchTerm);


            //return companyList;

            List<GuestCompanyBO> companyInfo = new List<GuestCompanyBO>();
            GuestCompanyDA itemDa = new GuestCompanyDA();
            companyInfo = itemDa.GetGuestCompanyInfo(searchTerm);

            return companyInfo;
        }

        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemSearch(string searchTerm, int categoryId, int IsCustomerItem, int IsSupplierItem)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemDetailsForAutoSearchByCategoryAndCustomerNSupplierItem(searchTerm, categoryId, IsCustomerItem, IsSupplierItem);

            return itemInfo;
        }

        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemSearchWithClient(string searchTerm, int categoryId, int IsCustomerItem, int IsSupplierItem, int ClientId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            //itemInfo = itemDa.GetItemDetailsForAutoSearchByCategoryAndCustomerNSupplierItemNClient(searchTerm, categoryId, IsCustomerItem, IsSupplierItem, ClientId);

            return itemInfo;
        }

        [WebMethod]
        public static ReturnInfo GenerateSupportBill(List<long> idList, string contactId, string paymentInstructionId)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            SupportNCaseSetupDA supportDA = new SupportNCaseSetupDA();
            try
            {
                status = supportDA.GenerateSupportBill(idList, contactId, paymentInstructionId, userInformationBO.UserInfoId, "Generate");

                if (status)
                {
                    CommonDA commonDA = new CommonDA();
                    foreach (long id in idList)
                    {
                        supportDA.CallCenterCallCenterBillingAccountsVoucherPostingProcess_SP(id);
                        commonDA.AutoCompanyBillGenerationProcess("SupportNTicket", id, userInformationBO.UserInfoId);
                    }

                    info.IsSuccess = true;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.BillGenerate, AlertType.Success);
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

        [WebMethod]
        public static STSupportBO GetSupportById(long id)
        {
            STSupportBO BO = new STSupportBO();
            SupportNCaseSetupDA supportDA = new SupportNCaseSetupDA();

            BO = supportDA.GetSupportCallInformationById(id);

            return BO;
        }

        [WebMethod]
        public static List<DocumentsBO> LoadContactDocument(long id, int randomId, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }

            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("SupportAndTicketDoc", randomId);

            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SupportAndTicketDoc", id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }

        [WebMethod]
        public static List<BankBO> GetBankInfoForAutoComplete(string bankName)
        {
            BankDA bpDA = new BankDA();
            return bpDA.GetBankInfoForAutoComplete(bankName);
        }

        [WebMethod]
        public static List<ContactInformationBO> GetContactInfoForAutoComplete(string contactName, int companyId)
        {
            ContactInformationDA DA = new ContactInformationDA();
            return DA.GetContactInfoForAutoComplete(contactName, companyId, "Billing");
        }

    }
}