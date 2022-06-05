using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.TaskManagment;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.TaskManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class ProjectInformation : System.Web.UI.Page
    {
        int donorId = 0;
        string url = "";
        string voucherStatus = "Approved";
        string companyName = string.Empty;
        string projectName = string.Empty;
        protected string reportSearchType = "0";
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["pid"] != "")
                {
                    url = HttpContext.Current.Request.Url.AbsoluteUri;
                    int projectId = Convert.ToInt32(Request.QueryString["pid"]);
                    hfProjectId.Value = projectId.ToString();
                    if (projectId > 0)
                    {
                        ProjectFillForm(projectId);
                        FileUpload(Request.QueryString["pid"]);                       
                    }
                    LoadCommonDropDownHiddenField();
                    LoadCRMConfiguration();
                }               
            }
        }
        private void LoadGLCompany(bool isSingle)
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            List<GLCompanyBO> companyList = entityDA.GetAllGLCompanyInfo();
            if (companyList != null)
            {
                if (companyList.Count > 0)
                {
                    companyName = companyList[0].Name;
                }
            }
        }
        private void ProjectFillForm(int projectId)
        {
            GLProjectBO projectBO = new GLProjectBO();
            GLProjectDA projectDA = new GLProjectDA();
            projectBO = projectDA.GetGLProjectInfoById(projectId);
            if (projectBO != null)
            {
                if (projectBO.ProjectId > 0)
                {
                    lblProjectName.Text = projectBO.Name;
                    projectName = projectBO.Name;
                    txtGLCompany.Text = projectBO.GLCompany;
                    txtShortName.Text = projectBO.ShortName;
                    txtCode.Text = projectBO.Code;
                    txtDescription.Text = projectBO.Description;
                    txtProjectStage.Text = projectBO.ProjectStage;

                    if (projectBO.StartDate != null)
                    {
                        txtStartDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(projectBO.StartDate));
                    }
                    else
                    {
                        txtStartDate.Text = string.Empty;
                    }

                    if (projectBO.EndDate != null)
                    {
                        txtEndDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(projectBO.EndDate));
                    }
                    else
                    {
                        txtEndDate.Text = string.Empty;
                    }

                    txtProjectAmount.Text = projectBO.ProjectAmount.ToString();

                    if (projectBO.ProjectCompanyId > 0)
                        LoadCustomerInfo(projectBO.ProjectCompanyId);

                    List<DocumentsBO> documents = LoadProjectDocument(projectBO.ProjectId);
                    GenerateProjectDocumentList(documents);
                }
            }
        }
        private void LoadCRMConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsShippingAddresswillshow", "IsShippingAddresswillshow");
            DivShippingAddress.Visible = setUpBO.SetupValue == "1";

            //setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDiscountPercentageWillShow", "IsDiscountPercentageWillShow");
            //DivDiscountPercentage.Visible = setUpBO.SetupValue == "1";

            //setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCreditLimitWillShow", "IsCreditLimitWillShow");
            //DivCreditLimit.Visible = setUpBO.SetupValue == "1";

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsNumberOfEmployeeWillShow", "IsNumberOfEmployeeWillShow");
            DivNoOfEmployee.Visible = setUpBO.SetupValue == "1";

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsAnnualRevenueWillShow", "IsAnnualRevenueWillShow");
            DivAnnualRevenue.Visible = setUpBO.SetupValue == "1";
        }
        private void LoadCustomerInfo(long? companyId)
        {

            GuestCompanyBO guestCompanyBO = new GuestCompanyBO();
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();

            guestCompanyBO = guestCompanyDA.GetGuestCompanyInfoById((int)companyId);

            lblCompanyName.InnerText = guestCompanyBO.CompanyName;
            lblCompanyType.Text = guestCompanyBO.TypeName;
            lblLifeCycleStage.Text = guestCompanyBO.LifeCycleStage;
            lblIndustry.Text = guestCompanyBO.IndustryName;
            lblOwnership.Text = guestCompanyBO.OwnershipName;
            lblAnnualRevenue.Text = guestCompanyBO.AnnualRevenue.ToString();
            lblNumberOfEmployee.Text = guestCompanyBO.NumberOfEmployee.ToString();
            lblPhone.Text = guestCompanyBO.ContactNumber;
            lblCompanyEmail.Text = guestCompanyBO.EmailAddress;
            lblFax.Text = guestCompanyBO.Fax;
            lblWebAddress.Text = guestCompanyBO.WebAddress;
            lblBillingCity.Text = guestCompanyBO.BillingCity;
            lblBillingCountry.Text = guestCompanyBO.BillingCountry;
            lblBillingPostCode.Text = guestCompanyBO.BillingPostCode;
            lblBillingState.Text = guestCompanyBO.BillingState;
            lblBillingStreet.Text = guestCompanyBO.BillingStreet;
            lblShippingCity.Text = guestCompanyBO.ShippingCity;
            lblShippingCountry.Text = guestCompanyBO.ShippingCountry;
            lblShippingPostCode.Text = guestCompanyBO.ShippingPostCode;
            lblShippingState.Text = guestCompanyBO.ShippingState;
            lblShippingStreet.Text = guestCompanyBO.ShippingStreet;
        }
        private void LoadTransactionList(int projectId, string startDate, string endDate)
        {
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            rvFiniancialReport.LocalReport.DataSources.Clear();
            rvFiniancialReport.ProcessingMode = ProcessingMode.Local;
            rvFiniancialReport.LocalReport.EnableExternalImages = true;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/RptJournalRegister.rdlc");


            if (!File.Exists(reportPath))
                return;

            rvFiniancialReport.LocalReport.ReportPath = reportPath;

            List<ReportParameter> reportParam = new List<ReportParameter>();

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            if (files[0].CompanyId > 0)
            {
                reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                reportParam.Add(new ReportParameter("CompanyName", companyName));
                reportParam.Add(new ReportParameter("CompanyProject", projectName));
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

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            reportParam.Add(new ReportParameter("ReportDateFrom", startDate));
            reportParam.Add(new ReportParameter("ReportDateTo", endDate));

            string reportName = "Transaction List";

            reportParam.Add(new ReportParameter("ReportName", reportName));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvFiniancialReport.LocalReport.SetParameters(reportParam);

            GLCommonReportDA commonReportDa = new GLCommonReportDA();
            List<GLJournalRegisterInfoForReportBO> voucherRegister = new List<GLJournalRegisterInfoForReportBO>();
            voucherRegister = commonReportDa.GetJournalRegisterInfo(FromDate, ToDate, voucherStatus, projectId.ToString(), donorId.ToString());
            var reportDataSet = rvFiniancialReport.LocalReport.GetDataSourceNames();
            rvFiniancialReport.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], voucherRegister));

            rvFiniancialReport.LocalReport.DisplayName = reportName;
            rvFiniancialReport.LocalReport.Refresh();

            reportSearchType = "";
            frmPrint.Attributes["src"] = "";
        }
        private void LoadTrialBalance(int projectId, string startDate, string endDate)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            string withOrWithoutOpening = string.Empty;

            DateTime dateTime = DateTime.Now;

            withOrWithoutOpening = "WithOpening";

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            int companyId = 0, donorId = 0;
            string showZeroTransaction = "WithoutZero";

            rvFiniancialReport.LocalReport.DataSources.Clear();
            rvFiniancialReport.ProcessingMode = ProcessingMode.Local;

            var reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/RptTrialBalanceDetails.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvFiniancialReport.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();

            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramReport.Add(new ReportParameter("CompanyName", companyName));
                paramReport.Add(new ReportParameter("CompanyProject", projectName));
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
            rvFiniancialReport.LocalReport.EnableExternalImages = true;

            //List<ReportParameter> paramLogo = new List<ReportParameter>();
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            paramReport.Add(new ReportParameter("ReportDateFrom", FromDate.ToString("dd-MMM-yyyy")));
            paramReport.Add(new ReportParameter("ReportDateTo", ToDate.ToString("dd-MMM-yyyy")));

            rvFiniancialReport.LocalReport.SetParameters(paramReport);
            //-- Company Logo ------------------End----------

            GLReportDA reportDA = new GLReportDA();
            List<TrialBalanceReportViewBO> trialBalanceBO = new List<TrialBalanceReportViewBO>();

            trialBalanceBO = reportDA.GetTrialBalanceDetails(FromDate, ToDate, companyId, projectId, donorId, showZeroTransaction, withOrWithoutOpening);

            var reportDataset = rvFiniancialReport.LocalReport.GetDataSourceNames();
            rvFiniancialReport.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], trialBalanceBO));

            rvFiniancialReport.LocalReport.DisplayName = "Trial Balance Statement";
            rvFiniancialReport.LocalReport.Refresh();

            reportSearchType = "";
            frmPrint.Attributes["src"] = "";
        }
        private void LoadCashFlow(int projectId, string startDate, string endDate)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();

            rvFiniancialReport.LocalReport.EnableHyperlinks = true;

            txtUrl.Text = url;

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            int companyId = 0, donorId = 0;

            rvFiniancialReport.LocalReport.DataSources.Clear();
            rvFiniancialReport.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/RptCashFlowStatement.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvFiniancialReport.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();

            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramReport.Add(new ReportParameter("CompanyName", companyName));
                paramReport.Add(new ReportParameter("CompanyProject", projectName));
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
            rvFiniancialReport.LocalReport.EnableExternalImages = true;

            //List<ReportParameter> paramLogo = new List<ReportParameter>();
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            paramReport.Add(new ReportParameter("ReportDateFrom", startDate));
            paramReport.Add(new ReportParameter("ReportDateTo", endDate));

            rvFiniancialReport.LocalReport.SetParameters(paramReport);
            //-- Company Logo ------------------End----------

            //Activate Session
            SessionNotesBreakDownBO notesBreakDown = new SessionNotesBreakDownBO();
            notesBreakDown.startDate = hmUtility.GetStringFromDateTime(FromDate);
            notesBreakDown.endDate = hmUtility.GetStringFromDateTime(ToDate.AddDays(1).AddSeconds(-1));
            notesBreakDown.companyName = files[0].CompanyName;
            notesBreakDown.companyWeb = files[0].WebAddress;
            notesBreakDown.glProject = projectId.ToString();
            Session["notesBreakDown"] = notesBreakDown;

            string projectUrl = txtUrl.Text;
            string withOrWithoutOpening = "WithOpening";

            GLReportDA reportDA = new GLReportDA();
            List<CashFlowStatReportViewBO> cashFlowBO = new List<CashFlowStatReportViewBO>();
            cashFlowBO = reportDA.GetCashFlowStatInfo(FromDate, ToDate, companyId, projectId, donorId, projectUrl, withOrWithoutOpening);

            List<CashFlowStatReportViewBO> cashFlow = new List<CashFlowStatReportViewBO>();
            List<CashFlowStatReportViewBO> cashFlowSummary = new List<CashFlowStatReportViewBO>();

            cashFlow = cashFlowBO.Where(c => c.GroupId < 4).ToList();
            cashFlowSummary = cashFlowBO.Where(c => c.GroupId >= 4).ToList();

            string notesNodesId = string.Empty;

            foreach (CashFlowStatReportViewBO r in cashFlow)
            {
                if (!string.IsNullOrEmpty(notesNodesId))
                    notesNodesId += "," + r.ParentNodeId.ToString();
                else
                    notesNodesId = r.ParentNodeId.ToString();
            }

            var reportDataset = rvFiniancialReport.LocalReport.GetDataSourceNames();
            rvFiniancialReport.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], cashFlow));
            rvFiniancialReport.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[1], cashFlowSummary));

            rvFiniancialReport.LocalReport.DisplayName = "Cash Flow Statement";
            rvFiniancialReport.LocalReport.Refresh();

            reportSearchType = "";
            frmPrint.Attributes["src"] = "";
        }
        private void LoadProfitLoss(int projectId, string startDate, string endDate)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            txtUrl.Text = url;

            string withOrWithoutOpening = "WithOpening";

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);
            decimal incomeTaxPercentage = Convert.ToDecimal(0);

            int companyId = 0, donorId = 0;
            bool isProfitableOrganization = true;

            rvFiniancialReport.LocalReport.DataSources.Clear();
            rvFiniancialReport.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/RptProfitNLossStatement.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvFiniancialReport.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();

            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramReport.Add(new ReportParameter("CompanyName", companyName));
                paramReport.Add(new ReportParameter("CompanyProject", projectName));
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
            string printDate = hmUtility.GetStringFromDateTime(currentDate) + " " + currentDate.ToString("hh:mm:ss tt");
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            //-- Company Logo -------------------------------
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvFiniancialReport.LocalReport.EnableExternalImages = true;
            rvFiniancialReport.LocalReport.EnableHyperlinks = true;

            //List<ReportParameter> paramLogo = new List<ReportParameter>();
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            paramReport.Add(new ReportParameter("ReportDateFrom", startDate));
            paramReport.Add(new ReportParameter("ReportDateTo", endDate));

            rvFiniancialReport.LocalReport.SetParameters(paramReport);
            //-- Company Logo ------------------End----------

            //Activate Session
            SessionNotesBreakDownBO notesBreakDown = new SessionNotesBreakDownBO();
            notesBreakDown.startDate = hmUtility.GetStringFromDateTime(FromDate);
            notesBreakDown.endDate = hmUtility.GetStringFromDateTime(ToDate.AddDays(1).AddSeconds(-1));
            notesBreakDown.companyName = files[0].CompanyName;
            notesBreakDown.companyWeb = files[0].WebAddress;
            notesBreakDown.glProject = projectId.ToString();
            Session["notesBreakDown"] = notesBreakDown;

            string projectUrl = txtUrl.Text;

            GLReportDA reportDA = new GLReportDA();
            List<ProfitNLossReportViewBO> profitNLossBO = new List<ProfitNLossReportViewBO>();
            profitNLossBO = reportDA.GetProfitNLossStatInfo(FromDate, ToDate, incomeTaxPercentage, companyId, projectId, donorId, projectUrl, withOrWithoutOpening);

            string notesNodesId = string.Empty;

            foreach (ProfitNLossReportViewBO r in profitNLossBO)
            {
                if (!string.IsNullOrEmpty(notesNodesId))
                    notesNodesId += "," + r.NodeId.ToString();
                else
                    notesNodesId = r.NodeId.ToString();
            }

            var reportDataset = rvFiniancialReport.LocalReport.GetDataSourceNames();
            rvFiniancialReport.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], profitNLossBO));

            rvFiniancialReport.LocalReport.DisplayName = "Profit and Loss Statement";
            rvFiniancialReport.LocalReport.Refresh();

            reportSearchType = "";
            frmPrint.Attributes["src"] = "";
        }
        private void LoadBalanceSheet(int projectId, string startDate, string endDate)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();

            string withOrWithoutOpening = string.Empty;
            withOrWithoutOpening = "WithOpening";


            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);
            string urlId = txtUrl.Text.Trim();

            int companyId = 0, donorId = 0;

            rvFiniancialReport.LocalReport.DataSources.Clear();
            rvFiniancialReport.LocalReport.EnableHyperlinks = true;
            rvFiniancialReport.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/RptBalanceScheetStatement.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvFiniancialReport.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();

            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramReport.Add(new ReportParameter("CompanyName", companyName));
                paramReport.Add(new ReportParameter("CompanyProject", projectName));
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
            rvFiniancialReport.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            paramReport.Add(new ReportParameter("ReportDateFrom", startDate));
            paramReport.Add(new ReportParameter("ReportDateTo", endDate));

            rvFiniancialReport.LocalReport.SetParameters(paramReport);

            GLCommonReportDA glDa = new GLCommonReportDA();
            List<BalanceSheetStatementInfoForReportBO> balanceSheet = new List<BalanceSheetStatementInfoForReportBO>();

            balanceSheet = glDa.GetBalanceSheetStatementInfo(FromDate, ToDate, companyId, projectId, donorId, urlId, withOrWithoutOpening, "");

            string notesNodesId = string.Empty;

            foreach (BalanceSheetStatementInfoForReportBO r in balanceSheet)
            {
                if (!string.IsNullOrEmpty(notesNodesId) && r.NodeId != 0)
                    notesNodesId += "," + r.NodeId.ToString();
                else
                    notesNodesId = r.NodeId.ToString();
            }

            var reportDataset = rvFiniancialReport.LocalReport.GetDataSourceNames();
            rvFiniancialReport.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], balanceSheet));

            rvFiniancialReport.LocalReport.DisplayName = "Balance Sheet Statement";
            rvFiniancialReport.LocalReport.Refresh();

            reportSearchType = "";
            frmPrint.Attributes["src"] = "";
        }
        private void FileUpload(string projectId)
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            //ClientUploader1.QueryParameters = "ProjectId=" + Server.UrlEncode(projectId);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenFieldForPleaseSelect.Value = hmUtility.GetDropDownFirstValue();
        }
        private void SetTab(string TabName)
        {
            if (TabName == "FinalcialReport")
            {
                C.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
            }

        }


        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            int projectId = Convert.ToInt32(Request.QueryString["pid"]);

            if (projectId == 0)
                return;


            GLProjectDA gLProjectDA = new GLProjectDA();

            GLProjectBO gLProjectBO = gLProjectDA.GetGLProjectInfoById(projectId);
            if (gLProjectBO != null)
                projectName = gLProjectBO.Name;

            if (Request.QueryString["cid"] != "")
            {
                int companyId = Convert.ToInt32(Request.QueryString["cid"]);
                GLCompanyDA entityDA = new GLCompanyDA();
                GLCompanyBO company = entityDA.GetGLCompanyInfoById(companyId);
                if (company != null)
                    companyName = company.Name;
            }

            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(txtReportStartDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtReportStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtReportStartDate.Text;
            }
            if (string.IsNullOrWhiteSpace(txtReportEndDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtReportEndDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtReportEndDate.Text;
            }
            if (this.ddlSearchType.SelectedValue == "1")
            {
                int fiscalYearId = Convert.ToInt32(hfFiscalYearId.Value);
                if (fiscalYearId > 0)
                {
                    GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
                    GLFiscalYearBO fiscalyearBO = new GLFiscalYearBO();
                    fiscalyearBO = fiscalYearDA.GetFiscalYearId(fiscalYearId);

                    startDate = fiscalyearBO.ReportFromDate;
                    endDate = fiscalyearBO.ReportToDate;
                }
            }
            string reportType = ddlReportType.SelectedValue;

            switch (reportType)
            {
                case "TransactionList":
                    LoadTransactionList(projectId, startDate, endDate);
                    break;
                case "TrialBalance":
                    LoadTrialBalance(projectId, startDate, endDate);
                    break;
                case "CashFlow":
                    LoadCashFlow(projectId, startDate, endDate);
                    break;
                case "Profit&Loss":
                    LoadProfitLoss(projectId, startDate, endDate);
                    break;
                case "BalanceSheet":
                    LoadBalanceSheet(projectId, startDate, endDate);
                    break;
                default:
                    break;
            }
            SetTab("FinalcialReport");
        }
        //private void LoadTaskByProjectId(int projectcId)
        //{
        //    List<SMTask> taskList = new List<SMTask>();
        //    AssignTaskDA taskDA = new AssignTaskDA();
        //    int totalRecords = 0;
        //    taskList = taskDA.GetTaskByProjectId(projectcId, 1, 1,out totalRecords);

        //}
        [WebMethod]
        public static List<DocumentsBO> LoadProjectDocument(long id, string deletedDoc = "")
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }

            List<DocumentsBO> docList = new List<DocumentsBO>();

            if (id > 0)
            {
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("GLProjectDocument", id));
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SalesDealFeedbackDocuments", id));
            }
            if (delete.Count > 0)
                docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }

        private void GenerateProjectDocumentList(List<DocumentsBO> documents)
        {
            List<DocumentsBO> guestDoc = documents;
            int totalDoc = documents.Count;
            int row = 0;
            string imagePath = "";
            string guestDocumentTable = "";
            if (totalDoc > 0)
            {
                guestDocumentTable += "<table id='dealDocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

                for (row = 0; row < totalDoc; row++)
                {
                    if (row % 2 == 0)
                    {
                        guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                    }
                    else
                    {
                        guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                    }

                    guestDocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";

                    if (guestDoc[row].Path != "")
                    {
                        imagePath += "<a style='color:#333333;' target='_blank' href='" + guestDoc[row].Path + guestDoc[row].Name + "'>";
                        if (guestDoc[row].Extention == ".jpg")
                            imagePath += "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                        else
                            imagePath += "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                        imagePath += "</a>";
                    }
                    else
                        imagePath = "";

                    guestDocumentTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";
                    imagePath = "";
                    guestDocumentTable += "<td align='left' style='width: 20%'>";
                    guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteProjetcDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                    guestDocumentTable += "</td>";
                    guestDocumentTable += "</tr>";
                }
                guestDocumentTable += "</table>";

                ProjectDocumentInfo.InnerHtml = guestDocumentTable;
            }
        }
        [WebMethod]
        public static ReturnInfo DeleteProjectDocument(long deletedDocumentId)
        {
            ReturnInfo info = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            List<DocumentsBO> docList = new List<DocumentsBO>();

            info.IsSuccess = new DocumentsDA().DeleteDocumentsByDocumentId(deletedDocumentId);
            if (info.IsSuccess)
            {
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                        EntityTypeEnum.EntityType.GLProject.ToString(), deletedDocumentId,
                            ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(),
                                hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GLProject));
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
            }
            else
            {
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }

        [WebMethod]
        public static GridViewDataNPaging<SMTask, GridPaging> LoadTaskByProjectId(int projetcId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<SMTask, GridPaging> myGridData = new GridViewDataNPaging<SMTask, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            List<SMTask> taskList = new List<SMTask>();
            AssignTaskDA taskDA = new AssignTaskDA();

            taskList = taskDA.GetTaskByProjectId(projetcId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(taskList, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static List<SMTask> LoadTaskByProjectId(int projetcId)
        {
            List<SMTask> taskList = new List<SMTask>();
            AssignTaskDA taskDA = new AssignTaskDA();
            taskList = taskDA.GetAllTaskByProjectId(projetcId);
            //taskList = taskDA.GetAllTaskByProjectId(projetcId).OrderBy(x => x.TaskDate).ThenBy(x => x.EstimatedDoneDate).ToList();
            return taskList;
        }

        [WebMethod]
        public static List<GLProjectBO> LoadProjectByCompanyId(int companyId)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            GLProjectDA entityDA = new GLProjectDA();
            List<GLProjectBO> list = new List<GLProjectBO>();
            list = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(Convert.ToInt32(companyId), Convert.ToInt32(userInformationBO.UserGroupId));
            return list;
        }

    }
}