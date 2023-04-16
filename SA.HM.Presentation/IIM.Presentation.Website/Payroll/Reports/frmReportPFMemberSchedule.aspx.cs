using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportPFMemberSchedule : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            LoadPayrollProvidentFundTitleText();
        }
        private void LoadPayrollProvidentFundTitleText()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            PanelHeadingTitleText.InnerText = userInformationBO.PayrollProvidentFundTitleText + "  Member List";
            PanelHeadingTitleText2.InnerText = userInformationBO.PayrollProvidentFundTitleText + "  Member List";
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            dispalyReport = 1;
            string memberType = ddlMemberType.SelectedValue;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptPFMemberList.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> reportParam = new List<ReportParameter>();

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
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            //reportParam.Add(new ReportParameter("PayrollProvidentFundTitleText", userInformationBO.PayrollProvidentFundTitleText));
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            if (this.ddlMemberType.SelectedValue == "Active")
            {
                reportParam.Add(new ReportParameter("ReportTitle", userInformationBO.PayrollProvidentFundTitleText + " Active Member List"));
            }
            else if (this.ddlMemberType.SelectedValue == "Inactive")
            {
                reportParam.Add(new ReportParameter("ReportTitle", userInformationBO.PayrollProvidentFundTitleText + " Terminated Member List"));
            }
            else if (this.ddlMemberType.SelectedValue == "Eligible")
            {
                reportParam.Add(new ReportParameter("ReportTitle", userInformationBO.PayrollProvidentFundTitleText + " Eligible Member List"));
            }


            rvTransaction.LocalReport.SetParameters(reportParam);

            EmpPFDA empPFDA = new EmpPFDA();
            //List<EmpPFBO> pfBOList = new List<EmpPFBO>();
            //pfBOList = empPFDA.GetAllPFMember();

            List<PFMemberReportViewBO> viewList = new List<PFMemberReportViewBO>();
            viewList = empPFDA.GetActivePFMemberList(memberType);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList));

            rvTransaction.LocalReport.DisplayName = userInformationBO.PayrollProvidentFundTitleText + " Member List";
            rvTransaction.LocalReport.Refresh();
        }
    }
}