using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Payroll;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.Recruitment.Reports
{
    public partial class frmReportApplicantList : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadJobCircular();
            }
        }

        private void LoadJobCircular()
        {
            JobCircularNRecruitmentDA entityDA = new JobCircularNRecruitmentDA();
            List<PayrollJobCircularBO> jobCircularlst = new List<PayrollJobCircularBO>();

            jobCircularlst = entityDA.GetJobCircularByDate(DateTime.Now);

            this.ddlJobCircular.DataSource = jobCircularlst;
            this.ddlJobCircular.DataTextField = "JobTitle";
            this.ddlJobCircular.DataValueField = "JobCircularId";
            this.ddlJobCircular.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlJobCircular.Items.Insert(0, item);
        }

        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            isMessageBoxEnable = 1;
            int jobCircularId = 0;

            HMCommonDA hmCommonDA = new HMCommonDA();
            if (ddlJobCircular.SelectedIndex != 0)
            {
                jobCircularId = Convert.ToInt32(ddlJobCircular.SelectedValue);
            }
            else {
                CommonHelper.AlertInfo(innboardMessage, "Please Select Job Circular.", AlertType.Warning);
                return;
            }
            string reportType = ddlReportType.SelectedValue;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Recruitment/Reports/Rdlc/RptApplicantList.rdlc");

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

            //_RoomStatusInfoByDate = 1;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(paramReport);


            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> applicantList = new List<EmployeeBO>();
            applicantList = empDA.GetApplicants(jobCircularId, reportType);
            //if (reportType == "0")
            //{
            //    applicantList = empDA.GetShortListedApplicants(jobCircularId);
            //}
            //else
            //{
            //    applicantList = empDA.GetAppointedApplicants(jobCircularId);
            //}

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], applicantList));

            rvTransaction.LocalReport.DisplayName = "Applicant List";
            rvTransaction.LocalReport.Refresh();
        }

        //private void LoadCommonDropDownHiddenField()
        //{
        //    CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
        //}
        //[WebMethod]
        //public static List<EmployeeBO> LoadEmployee(string activeStat)
        //{
        //    EmployeeDA empDA = new EmployeeDA();
        //    List<EmployeeBO> empList = new List<EmployeeBO>();
        //    empList = empDA.GetEmployeeInfoByStatus(activeStat);

        //    return empList;
        //}
    }
}