using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportEmpRoster : BasePage
    {
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadDepartment();
                this.LoadRosterInformation();                
            }
        }

        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            ddlDepartmentId.DataSource = entityDA.GetDepartmentInfo();
            ddlDepartmentId.DataTextField = "Name";
            ddlDepartmentId.DataValueField = "DepartmentId";
            ddlDepartmentId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlDepartmentId.Items.Insert(0, item);
        }
        private void LoadRosterInformation()
        {
            RosterHeadDA entityDA = new RosterHeadDA();

            this.ddlRosterId.DataSource = entityDA.GetRosterHeadInfo();
            this.ddlRosterId.DataTextField = "RosterName";
            this.ddlRosterId.DataValueField = "RosterId";
            this.ddlRosterId.DataBind();

            ListItem itemRosterId = new ListItem();
            itemRosterId.Value = "0";
            itemRosterId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlRosterId.Items.Insert(0, itemRosterId);
        }

        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            if(this.ddlRosterId.SelectedIndex == 0)
            {
                this.isMessageBoxEnable = 1;
                //this.txtGuestNameInfo.Text = string.Empty;                       
                lblMessage.Text = "Please Provide Roster Information.";
                this.ddlRosterId.Focus();
                return;
            }

            //_RoomStatusInfoByDate = 1;
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

            ////-- Company Logo -------------------------------
            //string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            //rvTransaction.LocalReport.EnableExternalImages = true;

            //List<ReportParameter> paramLogo = new List<ReportParameter>();
            //paramLogo.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            //rvTransaction.LocalReport.SetParameters(paramLogo);
            ////-- Company Logo ------------------End----------

            //TransactionDataSource.SelectParameters[0].DefaultValue = this.ddlRosterId.SelectedValue;
            //TransactionDataSource.SelectParameters[1].DefaultValue = this.txtCompanyName.Text;
            //TransactionDataSource.SelectParameters[2].DefaultValue = this.txtCompanyAddress.Text;
            //TransactionDataSource.SelectParameters[3].DefaultValue = this.txtCompanyWeb.Text;

            //rvTransaction.LocalReport.Refresh();

            int roasterId = Convert.ToInt32(this.ddlRosterId.SelectedValue);
            int departmentId = Convert.ToInt32(this.ddlDepartmentId.SelectedValue);

            EmpRosterDA roasterDA = new EmpRosterDA();
            List<EmpRoasterReportViewBO> roasterviewBO = new List<EmpRoasterReportViewBO>();
            roasterviewBO = roasterDA.GetEmpRoasterForReport(roasterId, departmentId);

            //EmpRoasterReportViewBO header = new EmpRoasterReportViewBO();
            //if (roasterviewBO.Count > 0)
            //{
            //    foreach (EmpRoasterReportViewBO roaster in roasterviewBO)
            //    {
            //        if (roaster.Code == "1")
            //        {
            //            header = roaster;
            //        }
            //    }
            //}            

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/EmpoyeeRosterReport.rdlc");

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
            
            // _RoomStatusInfoByDate = 1;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            //paramReport.Add(new ReportParameter("Date1", header.Date1));
            //paramReport.Add(new ReportParameter("Date2", header.Date2));
            //paramReport.Add(new ReportParameter("Date3", header.Date3));
            //paramReport.Add(new ReportParameter("Date4", header.Date4));
            //paramReport.Add(new ReportParameter("Date5", header.Date5));
            //paramReport.Add(new ReportParameter("Date6", header.Date6));
            //paramReport.Add(new ReportParameter("Date7", header.Date7));            

            rvTransaction.LocalReport.SetParameters(paramReport);
           
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], roasterviewBO));

            rvTransaction.LocalReport.DisplayName = "Employee Roaster";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
    }
}