using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.Collections;
using HotelManagement.Entity.GeneralLedger;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace HotelManagement.Presentation.Website.PurchaseManagment.Reports
{
    public partial class frmReportSupplierStatement : BasePage
    {
        protected int _GeneralLedgerInfo = -1;
        protected string reportSearchType = "0";
        protected int isMessageBoxEnable = -1;
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        protected int isCompanyProjectPanelEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            isSingle = hmUtility.GetSingleProjectAndCompany();
            if (!IsPostBack)
            {
                if (isSingle == true)
                {
                    isCompanyProjectPanelEnable = 1;
                    LoadSingleProjectAndCompany();
                }
                else
                {
                    this.LoadGLCompany(false);
                    this.LoadGLProject(false);
                }
                this.LoadSupplierInfo();
                this.LoadCommonDropDownHiddenField();
                this.LoadFiscalYear();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                return;
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/RptSupplierStatement.rdlc");

            if (!File.Exists(reportPath))
                return;

            _GeneralLedgerInfo = 1;

            rvTransaction.LocalReport.ReportPath = reportPath;

            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            if (string.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);                
            }
            else
            {
                startDate = txtStartDate.Text;
            }
            if (string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime);                
            }
            else
            {
                endDate = txtEndDate.Text;
            }

            int fiscalYearId = Convert.ToInt32(ddlFiscalYear.SelectedValue);
            if (fiscalYearId > 0)
            {
                GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
                GLFiscalYearBO fiscalyearBO = new GLFiscalYearBO();
                fiscalyearBO = fiscalYearDA.GetFiscalYearId(fiscalYearId);

                startDate = fiscalyearBO.ReportFromDate;
                endDate = fiscalyearBO.ReportToDate;
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);
            string nodeId = ddlSupplier.SelectedValue, project = ddlGLProject.SelectedValue, reportType = "Cash";

            List<ReportParameter> reportParam = new List<ReportParameter>();

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            if (files[0].CompanyId > 0)
            {
                reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    reportParam.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));

            reportParam.Add(new ReportParameter("NameOfAccountsHead", this.ddlSupplier.SelectedItem.Text));

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            rvTransaction.LocalReport.SetParameters(reportParam);

            GLCommonReportDA commonReportDa = new GLCommonReportDA();
            List<CashNBankBookStatementInfoForReportBO> voucherRegister = new List<CashNBankBookStatementInfoForReportBO>();
            voucherRegister = commonReportDa.GetCashNBankBookStatementInfo(FromDate, ToDate, nodeId, project, reportType);

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], voucherRegister));

            rvTransaction.LocalReport.DisplayName = "Cash Book Statement";
            rvTransaction.LocalReport.Refresh();

            reportSearchType = this.ddlSearchType.SelectedValue;

            //HMCommonDA hmCommonDA = new HMCommonDA();
            //if (isSingle == false)
            //{
            //    if (this.ddlGLProject.SelectedIndex == 0)
            //    {
            //        this.isMessageBoxEnable = 1;
            //        lblMessage.Text = "Please Select Project Name";
            //        return;
            //    }
            //}

            //if (!IsFormValid())
            //{
            //    return;
            //}
            //GLCommonReportDA entityDA = new GLCommonReportDA();

            //int NodeId = Convert.ToInt32(this.ddlNodeId.SelectedValue);

            //CompanyDA companyDA = new CompanyDA();
            //List<CompanyBO> files = companyDA.GetCompanyInfo();
            //if (files != null)
            //{
            //    if (files.Count > 0)
            //    {
            //        this.txtCompanyName.Text = files[0].CompanyName;
            //        this.txtCompanyAddress.Text = files[0].CompanyAddress;
            //        if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
            //        {
            //            this.txtCompanyWeb.Text = files[0].WebAddress;
            //        }
            //        else
            //        {
            //            this.txtCompanyWeb.Text = files[0].ContactNumber;
            //        }
            //    }
            //}
            //string startDate = string.Empty;
            //string endDate = string.Empty;
            //if (string.IsNullOrWhiteSpace(this.txtStartDate.Text))
            //{
            //    startDate = hmUtility.GetFromDate();
            //}
            //else
            //{
            //    startDate = this.txtStartDate.Text;
            //}
            //if (string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            //{
            //    endDate = hmUtility.GetToDate();
            //}
            //else
            //{
            //    endDate = this.txtEndDate.Text;
            //}
            //DateTime FromDate = hmUtility.GetDateTimeFromString(startDate);
            //DateTime ToDate = hmUtility.GetDateTimeFromString(endDate);
            //_GeneralLedgerInfo = 1;

            ////-- Company Logo -------------------------------
            //string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            //rvTransaction.LocalReport.EnableExternalImages = true;

            //List<ReportParameter> paramLogo = new List<ReportParameter>();
            //paramLogo.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            //rvTransaction.LocalReport.SetParameters(paramLogo);
            ////-- Company Logo ------------------End----------

            //TransactionDataSource.SelectParameters[0].DefaultValue = FromDate.ToString();
            //TransactionDataSource.SelectParameters[1].DefaultValue = ToDate.AddDays(1).AddSeconds(-1).ToString();
            //TransactionDataSource.SelectParameters[2].DefaultValue = this.ddlNodeId.SelectedValue.ToString();
            //TransactionDataSource.SelectParameters[3].DefaultValue = this.txtCompanyName.Text;
            //TransactionDataSource.SelectParameters[4].DefaultValue = this.txtCompanyAddress.Text;
            //TransactionDataSource.SelectParameters[5].DefaultValue = this.txtCompanyWeb.Text;
            //TransactionDataSource.SelectParameters[6].DefaultValue = this.ddlGLProject.SelectedValue;
            //TransactionDataSource.SelectParameters[7].DefaultValue = "Cash";
            //rvTransaction.LocalReport.Refresh();
        }
        protected void ddlGLCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGLCompany.SelectedIndex != -1)
            {
                this.LoadGLProject(this.ddlGLCompany.SelectedValue);
            }
        }
        //************************ User Defined Function ********************//
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadSingleProjectAndCompany()
        {
            this.LoadGLCompany(true);
            this.LoadGLProject(true);


        }
        private void LoadGLCompany(bool isSingle)
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            var List = entityDA.GetAllGLCompanyInfo();
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            if (isSingle == true)
            {
                companyList.Add(List[0]);
                this.ddlGLCompany.DataSource = companyList;
                this.ddlGLCompany.DataTextField = "Name";
                this.ddlGLCompany.DataValueField = "CompanyId";
                this.ddlGLCompany.DataBind();
            }
            else
            {
                this.ddlGLCompany.DataSource = List;
                this.ddlGLCompany.DataTextField = "Name";
                this.ddlGLCompany.DataValueField = "CompanyId";
                this.ddlGLCompany.DataBind();
                ListItem itemCompany = new ListItem();
                itemCompany.Value = "0";
                itemCompany.Text = hmUtility.GetDropDownFirstValue();
                this.ddlGLCompany.Items.Insert(0, itemCompany);
            }


        }
        private void LoadGLProject(bool isSingle)
        {
            GLProjectDA entityDA = new GLProjectDA();
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            var List = entityDA.GetAllGLProjectInfo();
            if (isSingle == true)
            {
                projectList.Add(List[0]);
                this.ddlGLProject.DataSource = projectList;
                this.ddlGLProject.DataTextField = "Name";
                this.ddlGLProject.DataValueField = "ProjectId";
                this.ddlGLProject.DataBind();
            }
            else
            {
                this.ddlGLProject.DataSource = List;
                this.ddlGLProject.DataTextField = "Name";
                this.ddlGLProject.DataValueField = "ProjectId";
                this.ddlGLProject.DataBind();

                ListItem itemProject = new ListItem();
                itemProject.Value = "0";
                itemProject.Text = hmUtility.GetDropDownFirstValue();
                this.ddlGLProject.Items.Insert(0, itemProject);
            }
            this.SingleprojectId.Value = List[0].ProjectId.ToString();
        }
        private void LoadSupplierInfo()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO supplierAccountsHeadIdBO = new HMCommonSetupBO();
            supplierAccountsHeadIdBO = commonSetupDA.GetCommonConfigurationInfo("SupplierAccountsHeadId", "SupplierAccountsHeadId");

            if (supplierAccountsHeadIdBO != null)
            {
                if (supplierAccountsHeadIdBO.SetupId > 0)
                {
                    int nodeId = Convert.ToInt32(supplierAccountsHeadIdBO.SetupValue);
                    this.ddlSupplier.DataSource = entityDA.GetNodeMatrixInfoByAncestorNodeId(nodeId);
                    this.ddlSupplier.DataTextField = "NodeHead";
                    this.ddlSupplier.DataValueField = "NodeId";
                    this.ddlSupplier.DataBind();
                }
            }

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlSupplier.Items.Insert(0, itemNodeId);
        }
        private void LoadGLProject(string companyId)
        {
            GLProjectDA entityDA = new GLProjectDA();
            this.ddlGLProject.DataSource = entityDA.GetGLProjectInfoByGLCompany(Convert.ToInt32(companyId));
            this.ddlGLProject.DataTextField = "Name";
            this.ddlGLProject.DataValueField = "ProjectId";
            this.ddlGLProject.DataBind();

            ListItem itemProject = new ListItem();
            itemProject.Value = "0";
            itemProject.Text = hmUtility.GetDropDownFirstValue();
            this.ddlGLProject.Items.Insert(0, itemProject);
        }
        private void LoadFiscalYear()
        {
            int projectId = Convert.ToInt32(this.SingleprojectId.Value);
            List<GLFiscalYearBO> fiscalYearList = new List<GLFiscalYearBO>();
            GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
            fiscalYearList = fiscalYearDA.GetFiscalYearListByProjectId(projectId);

            this.ddlFiscalYear.DataSource = fiscalYearList;
            this.ddlFiscalYear.DataTextField = "FiscalYearName";
            this.ddlFiscalYear.DataValueField = "FiscalYearId";
            this.ddlFiscalYear.DataBind();

            ListItem itemProject = new ListItem();
            itemProject.Value = "0";
            itemProject.Text = hmUtility.GetDropDownFirstValue();
            this.ddlFiscalYear.Items.Insert(0, itemProject);
        }
        private bool IsFormValid()
        {
            bool status = true;
            if (this.ddlSupplier.SelectedValue == "0")
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Provide Supplier Name";
                status = false;
            }

            return status;
        }
        [WebMethod]
        public static List<string> GetAutoCompleteData(string searchText, string searchProjectId)
        {
            List<string> nodeMatrixBOList = new List<string>();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            int projectId = 0;
            if (!string.IsNullOrWhiteSpace(searchProjectId))
            {
                projectId = Convert.ToInt32(searchProjectId);
                nodeMatrixBOList = nodeMatrixDA.GetNodeMatrixInfoByProjectIdNCashOrBankOrSearchText(projectId, "Cash", searchText);
            }
            return nodeMatrixBOList;
        }
        [WebMethod]
        public static string FillForm(string searchText)
        {
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            string nodeMatrixBO = nodeMatrixDA.GetNodeMatrixInfoByAccountHead2(searchText);

            return nodeMatrixBO;
        }
    }
}