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
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.GeneralLedger.Reports
{
    public partial class frmReportRevenueStatement : System.Web.UI.Page
    {
        protected int isMessageBoxEnable = -1;
        protected string reportSearchType = "0";
        protected int _IsReportPanelEnable = -1;
        string FinalString = "";
        string url = "";
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        protected int isCompanyProjectPanelEnable = -1;
        protected int IsConfigurableTemplateVisible = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            url = HttpContext.Current.Request.Url.AbsoluteUri;
            url = SplitPageName(url);
            isSingle = hmUtility.GetSingleProjectAndCompany();
            if (!IsPostBack)
            {
                Session["GLConfigurableBalanceSheetBOList"] = null;
                LiabilitiesAmountHiddenField.Value = "0";
                AssetsAmountHiddenField.Value = "0";

                if (isSingle == true)
                {
                    isCompanyProjectPanelEnable = 1;
                    LoadSingleProjectAndCompany();
                }
                else
                {
                    LoadGLCompany(false);
                    LoadGLProject(false);
                }
                rvTransaction.LocalReport.EnableHyperlinks = true;
                LoadCommonDropDownHiddenField();
                LoadFiscalYear();
                LoadGLDonor();
                LoadMonth();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }
        protected void ddlGLCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGLCompany.SelectedIndex != -1)
            {
                LoadGLProject(ddlGLCompany.SelectedValue);
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
        private void GenerateReport()
        {
            _IsReportPanelEnable = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();

            DateTime startDate = DateTime.Now, endDate = DateTime.Now, dateTime = DateTime.Now;
            int fiscalYearId = 0, monthId = DateTime.Now.Month, year = DateTime.Now.Year;
            int reportTypeId = 0;

            CustomFieldBO customField = new CustomFieldBO();
            CustomFieldBO reportTypeBo = new CustomFieldBO();
            reportTypeBo = hmCommonDA.GetCustomFieldByTypeAndValue("ReportType", "Hotel Revenue");

            if (reportTypeBo.FieldId > 0) {
                reportTypeId = reportTypeBo.FieldId;
            }

            if (ddlReportMonth.SelectedValue != "0")
            {
                monthId = Convert.ToInt32(ddlReportMonth.SelectedValue);
            }

            if (ddlFiscalYear.SelectedValue != "0")
            {
                fiscalYearId = Convert.ToInt32(ddlFiscalYear.SelectedValue);
                if (fiscalYearId > 0)
                {
                    GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
                    GLFiscalYearBO fiscalyearBO = new GLFiscalYearBO();
                    fiscalyearBO = fiscalYearDA.GetFiscalYearId(fiscalYearId);

                    startDate = fiscalyearBO.FromDate;
                    endDate = fiscalyearBO.ToDate;
                }
            }

            if (startDate.Year == endDate.Year)
            {
                year = startDate.Year;
            }
            else
            {
                if (monthId >= 7 && monthId <= 12)
                {
                    year = startDate.Year;
                }
                else
                {
                    year = endDate.Year;
                }
            }

            startDate = new DateTime(year, monthId, 1);
            endDate = startDate.AddMonths(1).AddDays(-1);

            int companyId = 0, projectId = 0, donorId = 0;

            if (ddlGLCompany.SelectedValue != "0" || ddlGLCompany.SelectedValue != "")
            {
                companyId = Convert.ToInt32(ddlGLCompany.SelectedValue);
            }

            if (ddlGLProject.SelectedValue != "0" || ddlGLProject.SelectedValue != "")
            {
                projectId = Convert.ToInt32(ddlGLProject.SelectedValue);
            }

            if (ddlDonor.SelectedValue != "0" || ddlDonor.SelectedValue != "")
            {
                donorId = Convert.ToInt32(ddlDonor.SelectedValue);
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.LocalReport.EnableHyperlinks = true;
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            rvTransactionDetails.LocalReport.DataSources.Clear();
            rvTransactionDetails.LocalReport.EnableHyperlinks = true;
            rvTransactionDetails.ProcessingMode = ProcessingMode.Local;
            rvTransactionDetails.LocalReport.EnableExternalImages = true;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/rptRevenueStatement.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/rptRevenueStatementDetails.rdlc");

            if (!File.Exists(reportPath))
                return;
           
            rvTransactionDetails.LocalReport.ReportPath = reportPath;

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
            
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            paramReport.Add(new ReportParameter("ReportMonth", startDate.ToString("MMMM yyyy")));

            rvTransaction.LocalReport.SetParameters(paramReport);
            rvTransactionDetails.LocalReport.SetParameters(paramReport);

            GLCommonReportDA glDa = new GLCommonReportDA();
            List<GLRevenueBO> revenue = new List<GLRevenueBO>();
            List<GLRevenueDetailsBO> revenueDetails = new List<GLRevenueDetailsBO>();

            revenue = glDa.GetRevenueStatement(fiscalYearId, startDate, endDate, Convert.ToInt16(monthId), reportTypeId, companyId, projectId, donorId);
            revenueDetails = glDa.GetRevenueDetailsStatement(fiscalYearId, startDate, endDate, Convert.ToInt16(monthId), reportTypeId, companyId, projectId, donorId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], revenue));

            var reportDetailsDataset = rvTransactionDetails.LocalReport.GetDataSourceNames();
            rvTransactionDetails.LocalReport.DataSources.Add(new ReportDataSource(reportDetailsDataset[0], revenueDetails));

            rvTransaction.LocalReport.DisplayName = "Revenue Statement";
            rvTransaction.LocalReport.Refresh();

            rvTransactionDetails.LocalReport.DisplayName = "Revenue Statement Details";
            rvTransactionDetails.LocalReport.Refresh();

            // hfNotesNodes.Value = notesNodesId;

            frmPrint.Attributes["src"] = "";
        }
        private void SubReportProcessingForInvoiceDetail(object sender, SubreportProcessingEventArgs e)
        {
            NodeMatrixDA entityDA = new NodeMatrixDA();
            //string startDate = string.Empty;
            //string endDate = string.Empty;
            //if (string.IsNullOrWhiteSpace(txtStartDate.Text))
            //{
            //    startDate = hmUtility.GetFromDate();
            //}
            //else
            //{
            //    startDate = txtStartDate.Text;
            //}
            //if (string.IsNullOrWhiteSpace(txtEndDate.Text))
            //{
            //    endDate = hmUtility.GetToDate();
            //}
            //else
            //{
            //    endDate = txtEndDate.Text;
            //}
            //DateTime FromDate = hmUtility.GetDateTimeFromString(startDate);
            //DateTime ToDate = hmUtility.GetDateTimeFromString(endDate);
            //int projectId = Convert.ToInt32(ddlGLProject.SelectedValue);
            List<GLConfigurableBalanceSheetBO> entityBOList = Session["GLConfigurableBalanceSheetBOList"] as List<GLConfigurableBalanceSheetBO>; //entityDA.GetGLReportDynamicallyForReport(FromDate, ToDate, "", "", "", projectId, "", "");
            List<GLConfigurableBalanceSheetBO> liabilitiesBOList = entityBOList.Where(x => x.AccountType == "Liabilities").ToList();
            e.DataSources.Add(new ReportDataSource("BalanceSheetLiabilities", liabilitiesBOList));
            decimal LiabilitiesAmount = 0;

            foreach (GLConfigurableBalanceSheetBO row in liabilitiesBOList)
            {
                LiabilitiesAmount = LiabilitiesAmount + row.CalculatedNodeAmount;
            }

            List<GLConfigurableBalanceSheetBO> assetesBOList = entityBOList.Where(x => x.AccountType == "Assets").ToList();
            e.DataSources.Add(new ReportDataSource("BalanceSheetAssets", assetesBOList));
            decimal AssetsAmount = 0;

            foreach (GLConfigurableBalanceSheetBO row in assetesBOList)
            {
                AssetsAmount = AssetsAmount + row.CalculatedNodeAmount;
            }
            LiabilitiesAmountHiddenField.Value = LiabilitiesAmount.ToString();
            AssetsAmountHiddenField.Value = AssetsAmount.ToString();
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
        }
        public string SplitPageName(string url)
        {

            string[] words = url.Split('/');
            int length = words.Length;
            for (int i = 0; i < length - 1; i++)
            {
                FinalString = FinalString + words[i] + '/';
            }
            return FinalString;

        }
        private void LoadSingleProjectAndCompany()
        {
            LoadGLCompany(true);
            LoadGLProject(true);
        }
        private void LoadGLCompany(bool isSingle)
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            var List = entityDA.GetAllGLCompanyInfo();

            hfCompanyAll.Value = JsonConvert.SerializeObject(List);

            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            if (isSingle == true)
            {
                companyList.Add(List[0]);
                ddlGLCompany.DataSource = companyList;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
            }
            else
            {
                ddlGLCompany.DataSource = List;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
                ListItem itemCompany = new ListItem();
                itemCompany.Value = "0";
                itemCompany.Text = hmUtility.GetDropDownFirstValue();
                ddlGLCompany.Items.Insert(0, itemCompany);
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
                ddlGLProject.DataSource = projectList;
                ddlGLProject.DataTextField = "Name";
                ddlGLProject.DataValueField = "ProjectId";
                ddlGLProject.DataBind();
            }
            else
            {
                ddlGLProject.DataSource = List;
                ddlGLProject.DataTextField = "Name";
                ddlGLProject.DataValueField = "ProjectId";
                ddlGLProject.DataBind();

                ListItem itemProject = new ListItem();
                itemProject.Value = "0";
                itemProject.Text = hmUtility.GetDropDownFirstAllValue();
                ddlGLProject.Items.Insert(0, itemProject);
            }
            SingleprojectId.Value = List[0].ProjectId.ToString();
        }
        private void LoadGLProject(string companyId)
        {
            GLProjectDA entityDA = new GLProjectDA();
            ddlGLProject.DataSource = entityDA.GetGLProjectInfoByGLCompany(Convert.ToInt32(companyId));
            ddlGLProject.DataTextField = "Name";
            ddlGLProject.DataValueField = "ProjectId";
            ddlGLProject.DataBind();

            ListItem itemProject = new ListItem();
            itemProject.Value = "0";
            itemProject.Text = hmUtility.GetDropDownFirstAllValue();
            ddlGLProject.Items.Insert(0, itemProject);
        }
        private void LoadFiscalYear()
        {
            int projectId = Convert.ToInt32(SingleprojectId.Value);
            List<GLFiscalYearBO> fiscalYearList = new List<GLFiscalYearBO>();
            GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
            fiscalYearList = fiscalYearDA.GetFiscalYearListByProjectId(projectId);

            ddlFiscalYear.DataSource = fiscalYearList;
            ddlFiscalYear.DataTextField = "FiscalYearName";
            ddlFiscalYear.DataValueField = "FiscalYearId";
            ddlFiscalYear.DataBind();

            ListItem itemProject = new ListItem();
            itemProject.Value = "0";
            itemProject.Text = hmUtility.GetDropDownFirstValue();
            ddlFiscalYear.Items.Insert(0, itemProject);
        }
        private bool IsFormValid()
        {
            bool status = true;
            if (isSingle == false)
            {
                //if (ddlGLProject.SelectedIndex == 0)
                //{
                //    isMessageBoxEnable = 1;
                //    lblMessage.Text = "Please Select Project Name";
                //    status = false;
                //}
            }

            return status;
        }
        private void LoadGLDonor()
        {
            GLDonorDA entityDA = new GLDonorDA();
            List<GLDonorBO> donorList = new List<GLDonorBO>();
            donorList = entityDA.GetAllGLDonorInfo();

            ddlDonor.DataSource = donorList;
            ddlDonor.DataTextField = "Name";
            ddlDonor.DataValueField = "DonorId";
            ddlDonor.DataBind();
            ListItem itemDonor = new ListItem();
            itemDonor.Value = "0";
            itemDonor.Text = hmUtility.GetDropDownFirstAllValue();
            ddlDonor.Items.Insert(0, itemDonor);

        }

        private void LoadMonth()
        {
            List<MonthYearBO> monthyear = new List<MonthYearBO>();
            monthyear = CommonHelper.MonthGeneration("MMMM");

            ddlReportMonth.DataSource = monthyear;
            ddlReportMonth.DataTextField = "MonthName";
            ddlReportMonth.DataValueField = "MonthId";
            ddlReportMonth.DataBind();

            ListItem itemDonor = new ListItem();
            itemDonor.Value = "0";
            itemDonor.Text = hmUtility.GetDropDownFirstValue();
            ddlReportMonth.Items.Insert(0, itemDonor);

        }

    }
}