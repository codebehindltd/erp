using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.GeneralLedger.Reports
{
    public partial class frmReportBudgetStatement : System.Web.UI.Page
    {
        protected int _IsReportPanelEnable = -1;
        protected string reportSearchType = "0";
        protected string hfFiscalId = "0";
        protected int isMessageBoxEnable = -1;
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        protected int isCompanyProjectPanelEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            //isSingle = hmUtility.GetSingleProjectAndCompany();
            if (!IsPostBack)
            {
                LoadCommonDropDownHiddenField();
                LoadCompany();
                LoadFiscalYear();
            }
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadCompany()
        {
            GLCompanyDA companyDA = new GLCompanyDA();
            List<GLCompanyBO> List = new List<GLCompanyBO>();
            List = companyDA.GetAllGLCompanyInfo();

            ddlCompany.DataSource = List;
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlCompany.Items.Insert(0, itemNodeId);
        }
        private void LoadFiscalYear()
        {
            ArrayList list = new ArrayList();
            List<GLFiscalYearBO> fiscalYearList = new List<GLFiscalYearBO>();
            GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
            fiscalYearList = fiscalYearDA.GetAllFiscalYear();

            ddlFiscalYear.DataSource = fiscalYearList;
            ddlFiscalYear.DataTextField = "FiscalYearName";
            ddlFiscalYear.DataValueField = "FiscalYearId";
            ddlFiscalYear.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlFiscalYear.Items.Insert(0, itemNodeId);
        }


        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;

            ////string currentPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //Process p = new Process();
            //p.StartInfo = new ProcessStartInfo()
            //{
            //    CreateNoWindow = true,
            //    Verb = "print",
            //    FileName =  reportSource //put the correct path here
            //};
            //p.Start();
        }
        //************************ User Defined Function ********************//
        private void GenerateReport()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            int fiscalYearId = Convert.ToInt32(hfFiscalYear.Value);

            int companyId = 0, projectId = 0, donorId = 0;
            string companyName = string.Empty;
            string companyAddress = string.Empty;
            string webAddress = string.Empty;
            //-- Company Logo -------------------------------
            string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            string projectName = string.Empty;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();
            if (files[0].CompanyId > 0)
            {
                companyName = files[0].CompanyName;
                companyAddress = files[0].CompanyAddress;

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    webAddress = files[0].WebAddress;
                }
                else
                {
                    webAddress = files[0].ContactNumber;
                }
            }

            if (hfCompanyId.Value != "0" && hfCompanyId.Value != "")
            {
                companyId = Convert.ToInt32(hfCompanyId.Value);
                GLCompanyBO glCompanyBO = new GLCompanyBO();
                GLCompanyDA glCompanyDA = new GLCompanyDA();
                glCompanyBO = glCompanyDA.GetGLCompanyInfoById(companyId);
                if (glCompanyBO != null)
                {
                    if (glCompanyBO.CompanyId > 0)
                    {
                        companyName = glCompanyBO.Name;
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.CompanyAddress))
                        {
                            companyAddress = glCompanyBO.CompanyAddress;
                        }
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.WebAddress))
                        {
                            webAddress = glCompanyBO.WebAddress;
                        }
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.ImageName))
                        {
                            imageName = glCompanyBO.ImageName;
                        }
                    }
                }
            }

            if (hfBudgetType.Value != "0" && hfBudgetType.Value != "")
            {
                lblBudgetType.Text = hfBudgetType.Value;
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/RptBudgetStatement.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;
            rvTransaction.LocalReport.EnableExternalImages = true;
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("CompanyProfile", companyName));
            paramReport.Add(new ReportParameter("CompanyAddress", companyAddress));
            paramReport.Add(new ReportParameter("CompanyWeb", webAddress));
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
            paramReport.Add(new ReportParameter("CompanyName", companyName));
            paramReport.Add(new ReportParameter("CompanyProject", projectName));
            rvTransaction.LocalReport.SetParameters(paramReport);

            GLReportDA reportDA = new GLReportDA();
            List<BudgetStatementReportDTO> budgets = new List<BudgetStatementReportDTO>();
            budgets = reportDA.GetBudgetStatement(fiscalYearId, companyId, projectId, donorId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], budgets));

            rvTransaction.LocalReport.DisplayName = "Budget Statement";
            rvTransaction.LocalReport.Refresh();

            hfFiscalId = hfFiscalYear.Value;
            frmPrint.Attributes["src"] = "";
        }

        [WebMethod]
        public static List<GLProjectBO> GetGLProjectByGLCompanyId(int companyId)
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            GLProjectDA entityDA = new GLProjectDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            projectList = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(companyId, userInformationBO.UserGroupId);

            return projectList;
        }
        [WebMethod]
        public static List<NodeMatrixBO> GetAutoCompleteData(string searchText)
        {
            List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            nodeMatrixBOList = nodeMatrixDA.GetNodeMatrixInfoByNameAndTransactionFlag(searchText, false);
            return nodeMatrixBOList;
        }
        [WebMethod]
        public static List<GLFiscalYearBO> PopulateFiscalYear(int projectId)
        {
            ArrayList list = new ArrayList();
            List<GLFiscalYearBO> fiscalYearList = new List<GLFiscalYearBO>();
            GLFiscalYearDA entityDA = new GLFiscalYearDA();
            fiscalYearList = entityDA.GetFiscalYearListByProjectId(Convert.ToInt32(projectId));

            return fiscalYearList;
        }

        [WebMethod]
        public static string GetBudgetType(int glCompanyId)
        {
            GLCompanyDA companyDA = new GLCompanyDA();
            GLCompanyBO glCompany = new GLCompanyBO();
            glCompany = companyDA.GetGLCompanyInfoById(glCompanyId);

            return glCompany.BudgetType;
        }
    }
}