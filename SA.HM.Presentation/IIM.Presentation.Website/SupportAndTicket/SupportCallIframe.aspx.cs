using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Entity.SupportAndTicket;
using HotelManagement.Data.SupportAndTicket;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.TaskManagement;
using HotelManagement.Data.TaskManagment;

namespace HotelManagement.Presentation.Website.SupportAndTicket
{
    public partial class SupportCallIframe : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        public SupportCallIframe() : base("SupportCallInformation")
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSupportDropDown();
                LoadCaseOwner();
                CheckAdminUser();
                LoadCategory();
                LoadCostCenterInfo();
                CheckPermission();

                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomProductId.Value = seatingId.ToString();
            }
        }
        private void CheckPermission()
        {

            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }
        private void LoadCostCenterInfo()
        {
            CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfoByType("CallCenter");

            if (costCentreTabBOList.Count > 0)
            {
                var vc = costCentreTabBOList.Where(c => c.CostCenterType == "CallCenter").ToList();

                if (vc.Count > 1)
                {
                    //CommonHelper.AlertInfo(innboardMessage, "Cost Center Is Not Setup Properly. Please Setup Cost Center and Try Again.", AlertType.Error);
                    return;
                }

                hfCostcenterId.Value = costCentreTabBOList[0].CostCenterId.ToString();

                costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(costCentreTabBOList[0].CostCenterId);
                if (costCentreTabBO.CostCenterId > 0)
                {
                    //hfBillPrefixCostcentrwise.Value = costCentreTabBO.BillNumberPrefix;
                    //hfIsCustomerDetailsEnable.Value = "0";
                    //if (costCentreTabBO.IsCustomerDetailsEnable == true)
                    //{
                    //    hfIsCustomerDetailsEnable.Value = "1";
                    //}

                    if (costCentreTabBO.IsVatEnable == true)
                    {
                        hfIsVatEnable.Value = "1";
                        cbTPVatAmount.Checked = true;
                        hfRestaurantVatAmount.Value = costCentreTabBO.VatAmount.ToString();
                        //txtRemarks.Text = txtRemarks.Text.Replace("@CompanyName", costCentreTabBO.CostCenter);
                    }
                    else
                    {
                        hfIsVatEnable.Value = "0";
                        cbTPVatAmount.Checked = false;
                        hfRestaurantVatAmount.Value = "0";
                    }

                    if (costCentreTabBO.IsVatSChargeInclusive == 0)
                    {
                        hfIsRestaurantBillInclusive.Value = "0";
                        ddlInclusiveOrExclusive.SelectedValue = "Exclusive";
                    }
                    else if (costCentreTabBO.IsVatSChargeInclusive == 1)
                    {
                        hfIsRestaurantBillInclusive.Value = "1";
                        ddlInclusiveOrExclusive.SelectedValue = "Inclusive";
                    }
                    else
                    {
                        hfIsRestaurantBillInclusive.Value = "1";
                        ddlInclusiveOrExclusive.SelectedValue = "Inclusive";
                    }
                }
            }
            else
            {
                //CommonHelper.AlertInfo(innboardMessage, "Cost Center Is Not Setup Properly. Please Setup Cost Center and Try Again.", AlertType.Error);
                return;
            }

            
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
        private void CheckAdminUser()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (userInformationBO.IsAdminUser == false)
            {
                ddlCaseOwner.SelectedValue = userInformationBO.EmpId.ToString();
                ddlCaseOwner.Enabled = false;

            }
            else
            {
                ddlCaseOwner.SelectedValue = "0";
            }
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
        public static ReturnInfo SaveOrUpdateSupport(STSupportBO support, List<STSupportDetailsBO> supportItems, List<STSupportDetailsBO> supportItemDeleted, List<STSupportDetailsBO> SupportItemForSupportDetails,
                                                  List<STSupportDetailsBO> SupportItemDeletedForSupportDetails, string supportCallType, string hfRandom, string deletedDocument)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            SupportNCaseSetupDA supportDA = new SupportNCaseSetupDA();
            support.CreatedBy = userInformationBO.UserInfoId;
            long id = 0;
            try
            {
                status = supportDA.SaveOrUpdateSupport(support, supportItems, supportItemDeleted, SupportItemForSupportDetails, SupportItemDeletedForSupportDetails, supportCallType, out id);


                if (status)
                {
                    info.IsSuccess = true;
                    if (support.Id == 0)
                    {
                        info.PrimaryKeyValue = id.ToString();
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        info.Data = 0;
                    }
                    else
                    {
                        info.PrimaryKeyValue = support.Id.ToString();
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }

                    DocumentsDA documentsDA = new DocumentsDA();
                    if (!string.IsNullOrEmpty(deletedDocument))
                    {
                        bool delete = documentsDA.DeleteDocument(deletedDocument);
                    }

                    HMCommonDA hmCommonDA = new HMCommonDA();
                    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(id, Convert.ToInt32(hfRandom));
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
        public static List<GuestCompanyBO> ClientSearch(string searchTerm)
        {
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
        public static List<InvItemAutoSearchBO> ItemSearchWithClient(int costCenterId, string searchTerm, int categoryId, int ClientId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemDetailsForAutoSearchByCategoryAndCustomerNSupplierItemNClient(costCenterId, searchTerm, categoryId, ClientId);

            return itemInfo;
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
        public static List<STCaseDetailHistoryBO> GetSupportCaseInternalNotesDetailsHistoryById(long id)
        {
            List<STCaseDetailHistoryBO> caseDetailHistoryBOList = new List<STCaseDetailHistoryBO>();
            SupportNCaseSetupDA supportDA = new SupportNCaseSetupDA();
            caseDetailHistoryBOList = supportDA.GetSupportCaseInternalNotesDetailsHistoryById(id);
            return caseDetailHistoryBOList;
        }
        [WebMethod]
        public static STCaseDetailHistoryBO GetSupportCaseInternalNotesDetailsInformationById(long id)
        {
            STCaseDetailHistoryBO caseDetailHistoryBO = new STCaseDetailHistoryBO();
            SupportNCaseSetupDA supportDA = new SupportNCaseSetupDA();
            caseDetailHistoryBO = supportDA.GetSupportCaseInternalNotesDetailsInformationById(id);
            return caseDetailHistoryBO;
        }
        [WebMethod]
        public static List<STSupportBO> LoadComapnyCallDetails(long companyId, long totalDetails)
        {
            List<STSupportBO> BOList = new List<STSupportBO>();
            SupportNCaseSetupDA supportDA = new SupportNCaseSetupDA();
            BOList = supportDA.LoadComapnyCallDetails(companyId, totalDetails);
            return BOList;
        }
        // Documents //
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
        public static List<DocumentsBO> LoadImplementationDocument(long id)
        {
            
            List<DocumentsBO> docList = new List<DocumentsBO>();
            
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("TaskAssignDocuments", (int)id));


            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }
        [WebMethod]
        public static List<DocumentsBO> LoadFeedbackDocument(long id)
        {

            List<DocumentsBO> docList = new List<DocumentsBO>();

            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("TaskFeedbackDocuments", (int)id));


            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }
    }
}