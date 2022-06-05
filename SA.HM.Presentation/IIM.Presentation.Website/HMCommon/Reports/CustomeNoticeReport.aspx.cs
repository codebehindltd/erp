using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HMCommon.Reports
{
    public partial class CustomeNoticeReport : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string param = Request.QueryString["nId"];

                if (!string.IsNullOrWhiteSpace(param))
                {
                    int noticeId = Convert.ToInt32(param);
                    if (noticeId > 0)
                    {
                        Generate(noticeId);
                    }
                }
            }
        }
        private void Generate(int Id)
        {
            dispalyReport = 1;

            CustomNoticeDA leaveDA = new CustomNoticeDA();
            CustomNoticeBO noticeBo = new CustomNoticeBO();
            noticeBo = leaveDA.GetNoticeInfoById(Id);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = "";
            reportPath = Server.MapPath(@"~/HMCommon/Reports/Rdlc/rptCustomNotice.rdlc");

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
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            //reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            //reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            reportParam.Add(new ReportParameter("AppDate", noticeBo.CreatedDate.ToString()));
            reportParam.Add(new ReportParameter("Content", noticeBo.Content));

            rvTransaction.LocalReport.SetParameters(reportParam);

            //var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            //rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], leaveBO));

            rvTransaction.LocalReport.DisplayName = "Leave Application";


            rvTransaction.LocalReport.Refresh();
        }
    }
}