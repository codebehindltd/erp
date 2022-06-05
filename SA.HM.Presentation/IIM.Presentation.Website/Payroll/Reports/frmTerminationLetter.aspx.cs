using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmTerminationLetter : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadDepartment();
            }
        }

        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            this.ddlDepartmentId.DataSource = entityDA.GetDepartmentInfo();
            this.ddlDepartmentId.DataTextField = "Name";
            this.ddlDepartmentId.DataValueField = "DepartmentId";
            this.ddlDepartmentId.DataBind();

            ddlDepartmentId.Items.Insert(0, new ListItem { Text = hmUtility.GetDropDownFirstValue(), Value = "0" });
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (!IsFormValidation())
            {
                return;
            }
            dispalyReport = 1;
            int empId = 0, departmentId = 0;
            DateTime terminationDate, decisionDate, appoinmentDate;

            HiddenField hfEmployeeId = (HiddenField)this.employeeeSearch.FindControl("hfEmployeeId");
            empId = Convert.ToInt32(hfEmployeeId.Value);

            if (empId == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "an Employee.", AlertType.Warning);
                return;
            }
            if (ddlDepartmentId.SelectedIndex != 0)
            {
                departmentId = Convert.ToInt32(ddlDepartmentId.SelectedValue);
            }

            //terminationDate = Convert.ToDateTime(txtTerminationDate.Text);
            //decisionDate = Convert.ToDateTime(txtDecisionDate.Text);
            terminationDate = CommonHelper.DateTimeToMMDDYYYY(txtTerminationDate.Text);
            decisionDate = CommonHelper.DateTimeToMMDDYYYY(txtDecisionDate.Text);
            //appoinmentDate = Convert.ToDateTime(txtAppointDate.Text);

            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empBO = new List<EmployeeBO>();
            empBO = empDA.GetEmployeeByIdForLetters(empId, departmentId);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/TerminationLetter.rdlc");

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

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("TerminationDate", terminationDate.ToString()));
            reportParam.Add(new ReportParameter("DecisionDate", decisionDate.ToString()));
            reportParam.Add(new ReportParameter("AppointeddDate", empBO[0].JoinDate.ToString()));
            reportParam.Add(new ReportParameter("IssueDate", DateTime.Now.ToString()));
            rvTransaction.LocalReport.SetParameters(reportParam);            

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], empBO));

            rvTransaction.LocalReport.DisplayName = "Termination Letter";

            rvTransaction.LocalReport.Refresh();
        }
        private bool IsFormValidation()
        {
            bool status = true;
            if (string.IsNullOrEmpty(txtTerminationDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Termination Date.", AlertType.Warning);
                status = false;
                txtTerminationDate.Focus();
            }
            else if (string.IsNullOrEmpty(txtDecisionDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Decision Date.", AlertType.Warning);
                status = false;
                txtDecisionDate.Focus();
            }
            //else if (string.IsNullOrEmpty(txtAppointDate.Text))
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Appointed Date.", AlertType.Warning);
            //    status = false;
            //    txtAppointDate.Focus();
            //}
            return status;
        }
    }
}