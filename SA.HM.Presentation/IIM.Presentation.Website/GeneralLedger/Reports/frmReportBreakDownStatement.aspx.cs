using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.GeneralLedger;
using System.Collections;
using System.Web.Services;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.UserInformation;


namespace HotelManagement.Presentation.Website.GeneralLedger.Reports
{
    public partial class frmReportBreakDownStatement : System.Web.UI.Page
    {
        protected int isMessageBoxEnable = -1;
        protected string reportSearchType = "0";
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        protected int isCompanyProjectPanelEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            isSingle = hmUtility.GetSingleProjectAndCompany();
            if (!IsPostBack)
            {
                int template = Int32.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["ConfigurableBalanceSheetTemplate"].ToString());
                this.txtIsConfigurable.Text = template.ToString();

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
                this.LoadNotesNumber();
                this.LoadCommonDropDownHiddenField();
                this.LoadFiscalYear();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            if (!IsFormValid())
            {
                return;
            }
            _RoomStatusInfoByDate = 1;
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

            if (this.ddlSearchType.SelectedValue == "1")
            {
                int fiscalYearId = Convert.ToInt32(this.ddlFiscalYear.SelectedValue);
                if (fiscalYearId > 0)
                {
                    GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
                    GLFiscalYearBO fiscalyearBO = new GLFiscalYearBO();
                    fiscalyearBO = fiscalYearDA.GetFiscalYearId(fiscalYearId);

                    startDate = fiscalyearBO.ReportFromDate;
                    endDate = fiscalyearBO.ReportToDate;
                }
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);            
            
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/RptNotesBreakdownStatement.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();

            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramReport.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            //-- Company Logo -------------------------------
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

           // List<ReportParameter> paramLogo = new List<ReportParameter>();
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            rvTransaction.LocalReport.SetParameters(paramReport);
            //-- Company Logo ------------------End----------

            string projectId = this.ddlGLProject.SelectedValue;
            string notesName = this.ddlNotesNumber.SelectedItem.Text;
            int isConfig = Convert.ToInt32(this.txtIsConfigurable.Text);

            GLReportDA reportDA = new GLReportDA();
            List<NotesBreakDownReportViewBO> notesBreakDownBO = new List<NotesBreakDownReportViewBO>();
            notesBreakDownBO = reportDA.GetNotesBreakDownInfo(FromDate, ToDate, projectId, notesName, isConfig);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], notesBreakDownBO));

            rvTransaction.LocalReport.DisplayName = "Notes Break Down";
            rvTransaction.LocalReport.Refresh();

            reportSearchType = this.ddlSearchType.SelectedValue;

            frmPrint.Attributes["src"] = "";
        }
        protected void ddlGLCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGLCompany.SelectedIndex != -1)
            {
                this.LoadGLProject(this.ddlGLCompany.SelectedValue);
            }
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
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
        private void LoadNotesNumber()
        {
            GLCommonSetupDA entityDA = new GLCommonSetupDA();
            int template = Int32.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["ConfigurableBalanceSheetTemplate"].ToString());

            this.ddlNotesNumber.DataSource = entityDA.GetNotesNumberInfo(template);
            this.ddlNotesNumber.DataTextField = "NotesNumber";
            this.ddlNotesNumber.DataValueField = "NotesNumber";
            this.ddlNotesNumber.DataBind();

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
            if (isSingle == false)
            {
                if (this.ddlGLProject.SelectedIndex == 0)
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Please Select Project Name";
                    status = false;
                }
            }

            return status;
        }
    }
}