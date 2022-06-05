using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class CashRequisitionApprovalIframe : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;

        public CashRequisitionApprovalIframe() : base("CashRequisitionOrBillVoucherInformation")
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                if (!isValidEmployee())
                {
                    return;
                }
                CheckPermission();                
                LoadEmployee();
                LoadGLCompany(false);
                LoadSupplierInfo();
                LoadSupplierAccountsHeadId();
                LoadExpenditureEquivalantHead();
                CheckAdminUser();
                IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects();
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomProductId.Value = seatingId.ToString();

                FileUpload();
            }

        }
        private void LoadSupplierInfo()
        {
            PMSupplierDA entityDA = new PMSupplierDA();
            List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();
            supplierBOList = entityDA.GetPMSupplierInfo();

            ddlSupplierId.DataSource = supplierBOList;
            ddlSupplierId.DataTextField = "Name";
            ddlSupplierId.DataValueField = "SupplierId";
            ddlSupplierId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSupplierId.Items.Insert(0, item);
        }
        private void LoadSupplierAccountsHeadId()
        {
            hfSupplierAccountsHeadId.Value = "0";
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SupplierAccountsHeadId", "SupplierAccountsHeadId");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    hfSupplierAccountsHeadId.Value = commonSetupBO.SetupValue;
                }
            }
        }
        private void IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects()
        {

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects", "IsCashRequisitionAdjustmentWithDifferentCompanyOrProjects");
            if (homePageSetupBO != null)
            {
                if (homePageSetupBO.SetupId > 0)
                {
                    hfIsCashRequisitionAdjustmentWithDifferentCompanyOrProjects.Value = homePageSetupBO.SetupValue;

                }
            }
        }

        private void CheckAdminUser()
        {
            Boolean IsAdminUser = false;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                IsAdminUser = true;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 18).Count() > 0)
                    {
                        IsAdminUser = true;
                    }
                }
            }
            #endregion


            if (!IsAdminUser)
            {
                ddlAssignEmployee.SelectedValue = userInformationBO.EmpId.ToString();
                ddlAssignEmployee.Enabled = false;

            }
            else
            {
                ddlAssignEmployee.SelectedValue = "0";
            }
        }
        public bool isValidEmployee()
        {
            bool status = true;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (userInformationBO.EmpId == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Your Employee is not mapped with software user, Please contact with admin.", AlertType.Warning);
                status = false;
            }
            return status;
        }
        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }

        private void LoadExpenditureEquivalantHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
            //entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("4").Where(x => x.IsTransactionalHead == true).ToList();
            entityBOList = entityDA.GetActiveNodeMatrixInfo().Where(x => x.IsTransactionalHead == true).ToList();

            ddlAccountExpenseHead.DataSource = entityBOList;
            ddlAccountExpenseHead.DataTextField = "HeadWithCode";
            ddlAccountExpenseHead.DataValueField = "NodeId";
            ddlAccountExpenseHead.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            ddlAccountExpenseHead.Items.Insert(0, itemBank);

            ddlTransactionFromAccountHeadId.DataSource = entityBOList;
            ddlTransactionFromAccountHeadId.DataTextField = "HeadWithCode";
            ddlTransactionFromAccountHeadId.DataValueField = "NodeId";
            ddlTransactionFromAccountHeadId.DataBind();

            ddlTransactionFromAccountHeadId.Items.Insert(0, itemBank);
        }

        private void LoadGLCompany(bool isSingle)
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            var List = entityDA.GetAllGLCompanyInfo();
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();

            //hfCompanyAll.Value = JsonConvert.SerializeObject(List);

            if (isSingle == true)
            {
                companyList.Add(List[0]);
                ddlGLCompany.DataSource = companyList;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();

                ddlCompany.DataSource = companyList;
                ddlCompany.DataTextField = "Name";
                ddlCompany.DataValueField = "CompanyId";
                ddlCompany.DataBind();
            }
            else
            {
                ddlGLCompany.DataSource = List;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();

                ddlCompany.DataSource = List;
                ddlCompany.DataTextField = "Name";
                ddlCompany.DataValueField = "CompanyId";
                ddlCompany.DataBind();


                ListItem itemCompany = new ListItem();
                itemCompany.Value = "0";
                itemCompany.Text = hmUtility.GetDropDownFirstValue();
                ddlGLCompany.Items.Insert(0, itemCompany);
                ddlCompany.Items.Insert(0, itemCompany);

            }
        }

        private void LoadEmployee()
        {
            EmployeeDA EmpDA = new EmployeeDA();
            List<EmployeeBO> EmpBO = new List<EmployeeBO>();
            EmpBO = EmpDA.GetEmployeeInfo();

            ddlAssignEmployee.DataSource = EmpBO;
            ddlAssignEmployee.DataTextField = "DisplayName";
            ddlAssignEmployee.DataValueField = "EmpId";
            ddlAssignEmployee.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlAssignEmployee.Items.Insert(0, item);

        }

        [WebMethod]
        public static ReturnInfo SaveOrUpdateBillVoucher(CashRequisitionBO billRequisition, List<CashRequisitionBO> RequisitionForBillVoucherNewlyAdded,
                                                  List<CashRequisitionBO> RequisitionForBillVoucherDeleted, string hfRandom, string deletedDocument)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            CashRequisitionDA cashRequisitionDA = new CashRequisitionDA();
            billRequisition.CreatedBy = userInformationBO.UserInfoId;
            string TransactionNo;
            string TransactionTypeCash;
            string ApproveStatusCash;
            long id = 0;
            long OwnerIdForDocuments = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            try
            {
                status = cashRequisitionDA.SaveOrUpdateBillVoucher(billRequisition, RequisitionForBillVoucherNewlyAdded, RequisitionForBillVoucherDeleted, out id, out TransactionNo, out TransactionTypeCash, out ApproveStatusCash);


                if (status)
                {
                    info.IsSuccess = true;
                    if (billRequisition.Id == 0)
                    {
                        OwnerIdForDocuments = id;
                        info.PrimaryKeyValue = id.ToString();
                        info.TransactionNo = TransactionNo;
                        info.TransactionType = TransactionTypeCash;
                        info.TransactionStatus = ApproveStatusCash;
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        info.Data = 0;
                    }
                    else
                    {
                        OwnerIdForDocuments = billRequisition.Id;
                        info.PrimaryKeyValue = billRequisition.Id.ToString();
                        info.TransactionNo = TransactionNo;
                        info.TransactionType = TransactionTypeCash;
                        info.TransactionStatus = ApproveStatusCash;
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }

                    DocumentsDA documentsDA = new DocumentsDA();
                    if (!string.IsNullOrEmpty(deletedDocument))
                    {
                        bool delete = documentsDA.DeleteDocument(deletedDocument);
                    }
                    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(hfRandom));
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
        public static List<GLProjectBO> LoadProjectByCompanyId(int companyId)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            GLProjectDA entityDA = new GLProjectDA();
            projectList = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(Convert.ToInt32(companyId), Convert.ToInt32(userInformationBO.UserGroupId));

            return projectList;
        }

        [WebMethod]
        public static CashRequisitionBO GetRequsitionById(long id)
        {
            CashRequisitionBO cashRequisition = new CashRequisitionBO();
            CashRequisitionDA cashRequisitionDA = new CashRequisitionDA();

            cashRequisition = cashRequisitionDA.GetRequsitionById(id);

            return cashRequisition;
        }

        [WebMethod]
        public static List<CashRequisitionBO> GetAdjustmentDetailsById(long id)
        {
            List<CashRequisitionBO> cashRequisition = new List<CashRequisitionBO>();
            CashRequisitionDA cashRequisitionDA = new CashRequisitionDA();

            cashRequisition = cashRequisitionDA.GetBillVoucherById(id);

            return cashRequisition;
        }
        // Documents //
        [WebMethod]
        public static List<DocumentsBO> LoadDocument(long id, int randomId, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }

            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("CashRequisitionApprovalDoc", randomId);

            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("CashRequisitionApprovalDoc", id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "LoadDetailGridInformation", jscript, true);

            flashUpload.QueryParameters = "CashRequisitionApprovalDocId=" + Server.UrlEncode(RandomProductId.Value);

            //flashUpload.QueryParameters = "InventoryProductId=" + Server.UrlEncode(RandomProductId.Value) + "~" + txtCode.Text;
        }

    }
}