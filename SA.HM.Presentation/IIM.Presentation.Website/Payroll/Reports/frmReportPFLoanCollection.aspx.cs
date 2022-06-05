using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using System.Web.Services;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.UserInformation;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportPFLoanCollection : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!Page.IsPostBack)
            {
                LoadDepartment();
                LoadCommonDropDownHiddenField();
            }
        }

        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            ddlDepartment.DataSource = entityDA.GetDepartmentInfo();
            ddlDepartment.DataTextField = "Name";
            ddlDepartment.DataValueField = "DepartmentId";
            ddlDepartment.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlDepartment.Items.Insert(0, item);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        protected void EmpDropDown_Change(object sender, EventArgs e)
        {
            int departId = Convert.ToInt32(ddlDepartment.SelectedValue);
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetEmployeeByDepartment(departId);

            ddlEmployee.DataSource = empList;
            ddlEmployee.DataTextField = "DisplayName";
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEmployee.Items.Insert(0, item);
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {            
            int departmentId = 0, empId = 0;            
            if (ddlDepartment.SelectedIndex != 0)
            {
                departmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
            }
            if (departmentId != 0 && ddlEmployee.SelectedIndex != 0)
            {
                empId = Convert.ToInt32(ddlEmployee.SelectedValue);
            }

            if (empId == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "an Employee.", AlertType.Warning);
                ddlDepartment.SelectedIndex = 0;
                ddlEmployee.SelectedIndex = 0;
                return;
            }

            dispalyReport = 1;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptPFLoanCollection.rdlc");

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
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(reportParam);

            EmpLoanDA empLoanDA = new EmpLoanDA();
            List<PFLoanCollectionViewBO> viewList = new List<PFLoanCollectionViewBO>();
            viewList = empLoanDA.GetPFLoanCollection(empId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList));

            rvTransaction.LocalReport.DisplayName = "PF Loan Collection";
            rvTransaction.LocalReport.Refresh();
        }        
    }
}