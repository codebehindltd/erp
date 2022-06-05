using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportCostCenterWiseReconsilationSummary : BasePage
    {
        HiddenField innboardMessage;
        protected int isDisplayReport = -1;
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadSalaryProcessMonth();
                LoadYearList();
                LoadBankInfo();
            }
        }

        private void LoadYearList()
        {
            ddlYear.DataSource = hmUtility.GetReportYearList();
            ddlYear.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlYear.Items.Insert(0, item);
        }
        private void LoadSalaryProcessMonth()
        {
            EmpSalaryProcessDA entityDA = new EmpSalaryProcessDA();
            this.ddlEffectedMonth.DataSource = entityDA.GetSalaryProcessMonth();
            this.ddlEffectedMonth.DataTextField = "MonthHead";
            this.ddlEffectedMonth.DataValueField = "MonthValue";
            this.ddlEffectedMonth.DataBind();
            //this.ddlEffectedMonth.SelectedIndex = DateTime.Now.Month - 1;
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlEffectedMonth.Items.Insert(0, item);
        }
        private void LoadBankInfo()
        {
            BankDA entityDA = new BankDA();
            ddlBankId.DataSource = entityDA.GetBankInfo();
            ddlBankId.DataTextField = "BankName";
            ddlBankId.DataValueField = "BankId";
            ddlBankId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlBankId.Items.Insert(0, item);
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            isDisplayReport = 1;
            int year = 0;
            bool IsManagement = false;

            if (this.ddlEffectedMonth.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Process Month.", AlertType.Warning);
            }
            else
            {

                DateTime processDateFrom = DateTime.Now, processDateTo = DateTime.Now;

                HMCommonDA hmCommonDA = new HMCommonDA();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                string selectedMonthRange = this.ddlEffectedMonth.SelectedValue.ToString();

                //processDateFrom = hmUtility.GetDateTimeFromString(CommonHelper.SalaryDateFrom(selectedMonthRange, ddlYear.SelectedValue), userInformationBO.ServerDateFormat);
                //processDateTo = hmUtility.GetDateTimeFromString(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue), userInformationBO.ServerDateFormat);

                processDateFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedMonthRange, ddlYear.SelectedValue));
                processDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue));

                rvTransaction.LocalReport.DataSources.Clear();
                rvTransaction.ProcessingMode = ProcessingMode.Local;

                var reportPath = "";
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptCostcenterWiseReconcilationSummary.rdlc");

                if (!File.Exists(reportPath))
                    return;

                rvTransaction.LocalReport.ReportPath = reportPath;

                if (ddlYear.SelectedIndex != 0)
                {
                    year = Convert.ToInt32(ddlYear.SelectedValue);
                }                
                if (ddlType.SelectedValue == "1")
                {
                    IsManagement = true;
                }

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

                footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                paramReport.Add(new ReportParameter("PrintDateTime", printDate));
                paramReport.Add(new ReportParameter("ManagementType", IsManagement == true ? "MANAGEMENT" : "NON-MANAGEMENT"));

                string reportProcessMonthInfo = this.ddlEffectedMonth.SelectedItem.Text + "-" + this.ddlYear.SelectedItem.Text;
                paramReport.Add(new ReportParameter("ReportProcessMonthInfo", reportProcessMonthInfo));

                string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
                rvTransaction.LocalReport.EnableExternalImages = true;

                paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));                

                rvTransaction.LocalReport.SetParameters(paramReport);

                EmpSalaryProcessDA salaryProcessDA = new EmpSalaryProcessDA();
                List<CostCenterWiseReconcilationSummaryViewBO> bo1 = new List<CostCenterWiseReconcilationSummaryViewBO>();

                bo1 = salaryProcessDA.GetCostcenterWiseReconsilationSummary(processDateFrom, processDateTo, year, IsManagement);                

                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], bo1));                

                rvTransaction.LocalReport.DisplayName = "Costcenter wise Reconcilation Summary.";
                rvTransaction.LocalReport.Refresh();
            }
        }
    }
}