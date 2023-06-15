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
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.GeneralLedger.Reports
{
    public partial class frmReportGeneralLedger : System.Web.UI.Page
    {
        protected int _GeneralLedgerInfo = -1;
        protected int isMessageBoxEnable = -1;
        protected string reportSearchType = "0";
        protected string hfFiscalId = "0";
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        protected int isCompanyProjectPanelEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCurrencyHead();
                LoadCommonDropDownHiddenField();
                LoadFiscalYear();
                LoadGLDonor();
            }
        }
        private void LoadCurrencyHead()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            int localCurrencyId = 1;
            List<CommonCurrencyBO> CommonCurrencyBOForLocal = new List<CommonCurrencyBO>();
            CommonCurrencyBOForLocal = headDA.GetConversionHeadInfoByType("Local");
            if (CommonCurrencyBOForLocal != null)
            {
                if (CommonCurrencyBOForLocal.Count > 0)
                {
                    localCurrencyId = CommonCurrencyBOForLocal[0].CurrencyId;
                    hflocalCurrencyId.Value = localCurrencyId.ToString();
                }
            }

            this.ddlCurrencyId.DataSource = headDA.GetConversionHeadInfoByType("All");
            this.ddlCurrencyId.DataTextField = "CurrencyName";
            this.ddlCurrencyId.DataValueField = "ConversionRate";
            this.ddlCurrencyId.DataBind();
            this.ddlCurrencyId.SelectedValue = localCurrencyId.ToString();
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
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            string withOrWithoutOpening = string.Empty;
            withOrWithoutOpening = dllWithOrWithoutOpening.SelectedValue;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            bool isTransactionalHead = false;
            isTransactionalHead = (hfIsTransactionalHead.Value == "1" ? true : false);

            var reportPath = "";

            if (ddlReportType.SelectedValue == "SummaryReport")
            {
                if (isTransactionalHead)            //Individual
                {
                    reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/rptGeneralLedger.rdlc");
                }
                else if (!isTransactionalHead)      //Group
                {
                    reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/rptGeneralGroupLedger.rdlc");
                }
            }
            if (ddlReportType.SelectedValue == "DetailsReport")
            {
                if (isTransactionalHead)        //Individual
                {
                    reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/RptGeneralDetailsLedger.rdlc");
                }
                else if (!isTransactionalHead)  //Group
                {
                    reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/rptGeneralGroupDetailsLedger.rdlc");
                }
            }

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

            if (ddlSearchType.SelectedValue == "1")
            {
                int fiscalYearId = Convert.ToInt32(hfFiscalYear.Value);
                if (fiscalYearId > 0)
                {
                    GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
                    GLFiscalYearBO fiscalyearBO = new GLFiscalYearBO();
                    fiscalyearBO = fiscalYearDA.GetFiscalYearId(fiscalYearId);

                    startDate = fiscalyearBO.ReportFromDate;
                    endDate = fiscalyearBO.ReportToDate;
                }
            }

            int companyId = 0, projectId = 0, donorId = 0;
            string companyName = string.Empty;
            string projectName = "All";
            if (hfCompanyId.Value != "0" && hfCompanyId.Value != "")
            {
                companyId = Convert.ToInt32(hfCompanyId.Value);
                companyName = hfCompanyName.Value;
            }

            if (hfProjectId.Value != "0" && hfProjectId.Value != "")
            {
                projectId = Convert.ToInt32(hfProjectId.Value);
                projectName = hfProjectId.Value != "0" ? hfProjectName.Value : "All";
            }

            if (ddlDonor.SelectedValue != "0" || ddlDonor.SelectedValue != "")
            {
                donorId = Convert.ToInt32(ddlDonor.SelectedValue);
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            Int64 nodeId = Convert.ToInt64(hfAccountsHeadId.Value);

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

            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            reportParam.Add(new ReportParameter("ReportDateFrom", FromDate.ToString("dd-MMM-yyyy")));
            reportParam.Add(new ReportParameter("ReportDateTo", ToDate.ToString("dd-MMM-yyyy")));            

            GLCommonReportDA commonReportDa = new GLCommonReportDA();
            List<LedgerBookReportBO> generalLedger = new List<LedgerBookReportBO>();
            List<GroupLedgerDetailsBO> groupLedger = new List<GroupLedgerDetailsBO>();

            if (ddlReportType.SelectedValue == "SummaryReport")
            {
                //if (ddlSearchType.SelectedValue == "1")
                //{
                //    if (isTransactionalHead)        // Individual
                //    {
                //        generalLedger = commonReportDa.GetLedgerBookReport(FromDate, ToDate, nodeId, companyId, projectId, donorId, withOrWithoutOpening);
                //    }
                //    else if (!isTransactionalHead)    //Group
                //    {
                //        generalLedger = commonReportDa.GetGroupLedgerBookReport(FromDate, ToDate, nodeId, companyId, projectId, donorId, withOrWithoutOpening);
                //    }
                //}
                //else
                {
                    if (isTransactionalHead)        // Individual
                    {
                        generalLedger = commonReportDa.GetLedgerBookReportDaterangeWise(FromDate, ToDate, nodeId, companyId, projectId, donorId, withOrWithoutOpening);
                    }
                    else if (!isTransactionalHead)    //Group
                    {
                        generalLedger = commonReportDa.GetGroupLedgerBookReportDateRangeWise(FromDate, ToDate, nodeId, companyId, projectId, donorId, withOrWithoutOpening);
                    }
                }
            }
            if (ddlReportType.SelectedValue == "DetailsReport")
            {
                //if (ddlSearchType.SelectedValue == "1")
                //{
                //    if (isTransactionalHead)          //Individual
                //    {
                //        groupLedger = commonReportDa.GetIndividualLedgerDetailsReport(FromDate, ToDate, nodeId, companyId, projectId, donorId, withOrWithoutOpening);
                //    }
                //    else if (!isTransactionalHead)     //Group
                //    {
                //        groupLedger = commonReportDa.GetGroupLedgerDetailsReport(FromDate, ToDate, nodeId, companyId, projectId, donorId, withOrWithoutOpening);
                //    }
                //}
                //else
                {
                    if (isTransactionalHead)          //Individual
                    {
                        groupLedger = commonReportDa.GetIndividualLedgerDetailsReportDateRangeWise(FromDate, ToDate, nodeId, companyId, projectId, donorId, withOrWithoutOpening);
                    }
                    else if (!isTransactionalHead)     //Group
                    {
                        groupLedger = commonReportDa.GetGroupLedgerDetailsReportDateRangeWise(FromDate, ToDate, nodeId, companyId, projectId, donorId, withOrWithoutOpening);
                    }
                }
            }
            
            // // // ------- Multi Currency Related Effects -------------------------- Start
            decimal ConversionRate = 1;
            ConversionRate = Convert.ToDecimal(ddlCurrencyId.SelectedValue);
            string reportCurrency = string.Empty;
            Session["ReportCurrencyConversionRate"] = ConversionRate;

            if (ConversionRate != 1)
            {
                reportCurrency = ddlCurrencyId.SelectedItem.Text + " (C.R: " + ddlCurrencyId.SelectedValue + ")";
            }
            else
            {
                reportCurrency = ddlCurrencyId.SelectedItem.Text;
            }

            reportParam.Add(new ReportParameter("ReportCurrency", reportCurrency));

            Session["ReportAccountsCompanyName"] = companyName;
            Session["ReportAccountsProjectName"] = projectName;
            Session["ReportCurrencyName"] = reportCurrency;
            Session["ReportPrintDate"] = printDate;

            foreach (GroupLedgerDetailsBO row in groupLedger)
            {
                if (ConversionRate != 1)
                {
                    if (row.DRAmount != 0)
                    {
                        row.DRAmount = row.DRAmount / ConversionRate;
                    }

                    if (row.CRAmount != 0)
                    {
                        row.CRAmount = row.CRAmount / ConversionRate;
                    }

                    if (row.CommulativeBalance != 0)
                    {
                        row.CommulativeBalance = row.CommulativeBalance / ConversionRate;
                    }
                }
            }
            // // // ------- Multi Currency Related Effects -------------------------- End


            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            if (ddlReportType.SelectedValue == "SummaryReport")
            {
                rvTransaction.LocalReport.SetParameters(reportParam);
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], generalLedger));
            }
            if (ddlReportType.SelectedValue == "DetailsReport")
            {
                var result = groupLedger.GroupBy(x => x.NodeId)
                     .Select(x => x.OrderBy(y => y.Rnk).Last());

                var total = result.Sum(s => s.CommulativeBalance);

                reportParam.Add(new ReportParameter("GrandTotal", total.ToString()));
                rvTransaction.LocalReport.SetParameters(reportParam);

                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], groupLedger));
            }

            rvTransaction.LocalReport.DisplayName = "General Ledger";
            rvTransaction.LocalReport.Refresh();

            reportSearchType = ddlSearchType.SelectedValue;
            hfFiscalId = hfFiscalYear.Value;

            frmPrint.Attributes["src"] = "";
        }
        
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Portrait.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
        //************************ User Defined Function ********************//
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
            CommonDropDownHiddenFieldForPleaseSelect.Value = hmUtility.GetDropDownFirstValue();
        }
        
        
        private void LoadFiscalYear()
        {
            List<GLFiscalYearBO> fiscalYearList = new List<GLFiscalYearBO>();
            GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();
            fiscalYearList = fiscalYearDA.GetAllFiscalYear();

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
            int parse;

            //if (ddlNodeId.SelectedIndex == 0)
            //{
            //    isMessageBoxEnable = 1;
            //    lblMessage.Text = "Please Provide Account Head";
            //    status = false;
            //}
            //else if (!Int32.TryParse(ddlNodeId.SelectedValue, out parse))
            //{
            //    isMessageBoxEnable = 1;
            //    lblMessage.Text = "Please Provide Correct Account Head";
            //    status = false;
            //}

            return status;
        }

        [WebMethod]
        public static List<NodeMatrixBO> GetAutoCompleteData(string searchText)
        {
            List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            nodeMatrixBOList = nodeMatrixDA.GetNodeMatrixInfoByAccountNameLikeSearch(searchText);

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