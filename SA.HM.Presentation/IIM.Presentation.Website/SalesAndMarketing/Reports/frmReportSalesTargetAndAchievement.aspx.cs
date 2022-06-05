using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.SalesAndMarketing;
using System.Web.Services;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;
using System.Collections;

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class frmReportSalesTargetAndAchievement : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected int isMessageBoxEnable = -1;
        protected int _IsReportPanelVisible = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadCommonDropDownHiddenField();
                LoadCompany();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(hfFiscalYear.Value))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Fiscal Year.", AlertType.Warning);                
                this.ddlFiscalYear.Focus();
            }

            if (!string.IsNullOrWhiteSpace(hfFiscalYear.Value))
            {
                int fiscalYear = 0;
                _IsReportPanelVisible = 1;
                string fiscalYearName = string.Empty;
                fiscalYear = Convert.ToInt32(hfFiscalYear.Value);
                fiscalYearName = hfFiscalYearName.Value;

                rvTransaction.LocalReport.DataSources.Clear();
                rvTransaction.ProcessingMode = ProcessingMode.Local;

                var reportPath = "";
                reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/rptSalesTargetNAchievement.rdlc");

                if (!File.Exists(reportPath))
                    return;

                rvTransaction.LocalReport.ReportPath = reportPath;

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
                string printDate = hmUtility.GetStringFromDateTime(currentDate) + " " + currentDate.ToString("hh:mm:ss tt");
                string footerPoweredByInfo = string.Empty;
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                reportParam.Add(new ReportParameter("PrintDateTime", printDate));
                reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                reportParam.Add(new ReportParameter("FiscalYearName", fiscalYearName));
                HMCommonDA hmCommonDA = new HMCommonDA();
                string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
                rvTransaction.LocalReport.EnableExternalImages = true;
                rvTransaction.LocalReport.SetParameters(reportParam);
                var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

                PMPurchaseOrderDA detalisDA = new PMPurchaseOrderDA();
                List<SMSalesOrderBO> allOrderList = new List<SMSalesOrderBO>();
                allOrderList = detalisDA.GetAccountManagerSalesTargetReport(fiscalYear);

                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], allOrderList));

                rvTransaction.LocalReport.DisplayName = "Sales Target and Achievement Information";
                rvTransaction.LocalReport.Refresh();

                frmPrint.Attributes["src"] = "";
                ddlCompany.SelectedValue = "0";
                hfFiscalYear.Value = "";
                hfFiscalYearName.Value = "";
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
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<NodeMatrixBO> GetAutoCompleteData(string searchText)
        {
            List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            nodeMatrixBOList = nodeMatrixDA.GetNodeMatrixInfoByNameAndTransactionFlag(searchText, false);
            return nodeMatrixBOList;
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
        public static List<GLFiscalYearBO> PopulateFiscalYear(int projectId)
        {
            ArrayList list = new ArrayList();
            List<GLFiscalYearBO> fiscalYearList = new List<GLFiscalYearBO>();
            GLFiscalYearDA entityDA = new GLFiscalYearDA();
            fiscalYearList = entityDA.GetFiscalYearListByProjectId(Convert.ToInt32(projectId));

            return fiscalYearList;
        }
    }
}