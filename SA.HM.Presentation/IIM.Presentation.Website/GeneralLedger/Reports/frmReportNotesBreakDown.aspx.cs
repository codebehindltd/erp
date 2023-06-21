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
using Microsoft.Reporting.WebForms;
using HotelManagement.Presentation.Website.Common;
using Newtonsoft.Json;
using HotelManagement.Entity.UserInformation;
using System.IO;

namespace HotelManagement.Presentation.Website.GeneralLedger.Reports
{
    public partial class frmReportNotesBreakDown : System.Web.UI.Page
    {
        protected int _IsReportPanelEnable = -1;
        protected string reportSearchType = "0";
        protected int isMessageBoxEnable = -1;
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        protected int isCompanyProjectPanelEnable = -1;

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
                    LoadGLCompany(false);
                    LoadGLProject(false);
                }

                LoadCommonDropDownHiddenField();
                LoadFiscalYear();
                LoadGLDonor();
                LoadNotesNumber();
            }
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
            GenerateReport();
        }
        protected void ddlGLCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGLCompany.SelectedIndex != -1)
            {
                LoadGLProject(this.ddlGLCompany.SelectedValue);
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
            HMCommonDA hmCommonDA = new HMCommonDA();
            if (!IsFormValid())
            {
                return;
            }

            _IsReportPanelEnable = 1;

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

            int companyId = 0, projectId = 0, donorId = 0;
            string showZeroTransaction = string.Empty;

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
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/GeneralLedger/Reports/Rdlc/rptNotesBreakDown.rdlc");

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

            int nodeId = Convert.ToInt32(ddlNotesNumber.SelectedValue);

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

            //List<ReportParameter> paramLogo = new List<ReportParameter>();
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            paramReport.Add(new ReportParameter("ReportDateFrom", FromDate.ToString("dd-MMM-yyyy")));
            paramReport.Add(new ReportParameter("ReportDateTo", ToDate.ToString("dd-MMM-yyyy")));

            rvTransaction.LocalReport.SetParameters(paramReport);
            //-- Company Logo ------------------End----------

            GLCommonReportDA commonReportDa = new GLCommonReportDA();
            List<LedgerBookReportBO> generalLedger = new List<LedgerBookReportBO>();

            generalLedger = commonReportDa.GetNotesBreakdownReport(FromDate, ToDate, nodeId, companyId, projectId, donorId, string.Empty, string.Empty);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], generalLedger));

            rvTransaction.LocalReport.DisplayName = "Notes Breakdown";
            rvTransaction.LocalReport.Refresh();

            reportSearchType = ddlSearchType.SelectedValue;

            frmPrint.Attributes["src"] = "";
        }

        private void LoadNotesNumber()
        {
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> nodeMatrixList = new List<NodeMatrixBO>();

            nodeMatrixList = entityDA.GetNodeMatrixInfoByCustomString(" WHERE NotesNumber IS NOT NULL AND IsTransactionalHead IS NULL AND NodeMode = 1 ");

            var nm = (from n in nodeMatrixList
                      select new
                      {
                          HeadNotes = n.NodeHead + " (" + n.NotesNumber + ")",
                          NodeId = n.NodeId

                      }).ToList();


            ddlNotesNumber.DataSource = nm;
            ddlNotesNumber.DataTextField = "HeadNotes";
            ddlNotesNumber.DataValueField = "NodeId";
            ddlNotesNumber.DataBind();

            ListItem itemCompany = new ListItem();
            itemCompany.Value = "0";
            itemCompany.Text = hmUtility.GetDropDownFirstValue();
            ddlNotesNumber.Items.Insert(0, itemCompany);

        }

        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
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
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();

            hfCompanyAll.Value = JsonConvert.SerializeObject(List);

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
            int projectId = Convert.ToInt32(this.SingleprojectId.Value);
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
                //if (this.ddlGLProject.SelectedIndex == 0)
                //{
                //    isMessageBoxEnable = 1;
                //    //lblMessage.Text = "Please Select Project Name";
                //    status = false;
                //}
            }

            return status;
        }

    }
}