using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Banquet;
using HotelManagement.Entity.Banquet;

namespace HotelManagement.Presentation.Website.Banquet
{
    public partial class frmReportBanquetRevenueInfo : System.Web.UI.Page
    {
        int _offset = -360;
        int _mindiff = 0;
        protected int _RoomStatusInfoByDate = -1;
        private Boolean isViewPermission = false;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                string CurrentYear = DateTime.Today.Year.ToString();
                ddlYear.SelectedValue = CurrentYear;
                this.CheckObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmReportBanquetRevenueInfo.ToString());
            }
        }
        private void CheckObjectPermission(int userId, string formName)
        {
            ObjectPermissionDA objectPermissionDA = new ObjectPermissionDA();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = objectPermissionDA.GetFormPermissionByUserIdNForm(userId, formName);
            if (objectPermissionBO.ObjectPermissionId > 0)
            {
                isViewPermission = objectPermissionBO.IsViewPermission;

                if (!isViewPermission)
                {
                    Response.Redirect("/HMCommon/frmHMHome.aspx");
                }
            }
            else
            {
                Response.Redirect("/HMCommon/frmHMHome.aspx");
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;
            string ReportYear = ddlYear.SelectedValue.ToString();
            string Month = ddlMonth.SelectedValue.ToString();
            string Type = ddlReportType.SelectedValue.ToString();
            string ReportFor = "OccessionType";
            string ReportDurationName = "";

            if (Type == "Yearly")
            {
                ReportDurationName = "Yearly";
            }
            else
            {
                ReportDurationName = Month;
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Banquet/Reports/Rdlc/rptBanquetInformation.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            //List<ReportParameter> reportParam = new List<ReportParameter>();

            //CompanyDA companyDA = new CompanyDA();
            //List<CompanyBO> files = companyDA.GetCompanyInfo();
            //if (files[0].CompanyId > 0)
            //{
            //    reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
            //    reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

            //    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
            //    {
            //        reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
            //    }
            //    else
            //    {
            //        reportParam.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
            //    }
            //}

            //rvTransaction.LocalReport.SetParameters(reportParam);

            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            List<ReportParameter> reportParam = new List<ReportParameter>();
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(reportParam);

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            BanquetInformationDA banquetDa = new BanquetInformationDA();
            List<BanquetClientRevenueReportBO> banquetClientRevenue = new List<BanquetClientRevenueReportBO>();
            banquetClientRevenue = banquetDa.GetBanquetRevenueInfoForBarNLineChartReport(ReportFor, ReportDurationName, ReportYear);

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], banquetClientRevenue));
            rvTransaction.LocalReport.Refresh();
        }
    }
}