using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using Microsoft.Reporting.WebForms;
using HotelManagement.Entity.HMCommon;
using System.IO;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportPFStatement : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!Page.IsPostBack)
            {
                LoadPayrollProvidentFundTitleText();
                LoadDepartment();
                LoadCommonDropDownHiddenField();
                LoadYearList();
                LoadSalaryProcessMonth();
                ControlShowHide();
                ddlEmployee.Visible = false;
                lblEmployee.Visible = false;
            }
        }
        private void LoadPayrollProvidentFundTitleText()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            PanelHeadingTitleText.InnerText = userInformationBO.PayrollProvidentFundTitleText + "  Statement";
            PanelHeadingTitleText2.InnerText = userInformationBO.PayrollProvidentFundTitleText + "  Statement";
        }
        private void ControlShowHide()
        {
            Boolean IsAdminUser = false;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                IsAdminUser = true;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 18).Count() > 0)
                    {
                        IsAdminUser = true;
                    }
                }
            }
            #endregion

            EmployeeInformationDiv.Visible = IsAdminUser;
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
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
        }
        private void LoadYearList()
        {
            ddlYear.DataSource = hmUtility.GetReportYearList();
            ddlYear.DataBind();
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
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
        protected void EmpDropDown_Change(object sender, EventArgs e)
        {
            int departId = Convert.ToInt32(ddlDepartment.SelectedValue);
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetEmployeeByDepartment(departId);

            if (departId == 0)
            {
                ddlEmployee.Visible = false;
                lblEmployee.Visible = false;
            }
            else
            {
                ddlEmployee.Visible = true;
                lblEmployee.Visible = true;
            }
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
            if (this.ddlEffectedMonth.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Process Month.", AlertType.Warning);
                ddlEffectedMonth.Focus();
                return;
            }
            else
            {
                dispalyReport = 1;
                int departmentId = 0, empId = 0, year = 0;
                DateTime processDateTo = DateTime.Now;

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                if (userInformationBO.IsAdminUser)
                {
                    if (ddlDepartment.SelectedValue == "0")
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Department.", AlertType.Warning);
                        ddlDepartment.Focus();
                        return;
                    }
                    else if (ddlEmployee.SelectedValue == "0")
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Employee.", AlertType.Warning);
                        ddlEmployee.Focus();
                        return;
                    }

                    if (ddlDepartment.SelectedIndex != 0)
                    {
                        departmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
                    }
                    if (departmentId != 0 && ddlEmployee.SelectedIndex != 0)
                    {
                        empId = Convert.ToInt32(ddlEmployee.SelectedValue);
                    }
                }
                else
                {
                    empId = Convert.ToInt32(userInformationBO.EmpId);
                }

                string selectedMonthRange = this.ddlEffectedMonth.SelectedValue.ToString();
                processDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue));
                if (ddlYear.SelectedValue != "0")
                {
                    year = Convert.ToInt32(ddlYear.SelectedValue);
                }

                rvTransaction.LocalReport.DataSources.Clear();
                rvTransaction.ProcessingMode = ProcessingMode.Local;
                rvTransaction.LocalReport.EnableExternalImages = true;

                HMCommonDA hmCommonDA = new HMCommonDA();
                string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");

                string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptPFStatement.rdlc");

                if (!File.Exists(reportPath))
                    return;

                rvTransaction.LocalReport.ReportPath = reportPath;
                int glCompanyId = 0;
                string companyName = string.Empty;
                string companyAddress = string.Empty;
                string webAddress = string.Empty;
                string telephoneNumber = string.Empty;
                string hotLineNumber = string.Empty;

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> files = companyDA.GetCompanyInfo();
                if (files[0].CompanyId > 0)
                {
                    companyName = files[0].CompanyName;
                    companyAddress = files[0].CompanyAddress;
                    webAddress = files[0].WebAddress;
                    telephoneNumber = files[0].Telephone;
                    hotLineNumber = files[0].HotLineNumber;
                }

                List<ReportParameter> reportParam = new List<ReportParameter>();

                if (glCompanyId == 0)
                {
                    if (empId != 0)
                    {
                        EmployeeBO employeeBO = new EmployeeBO();
                        EmployeeDA employeeDA = new EmployeeDA();
                        employeeBO = employeeDA.GetEmployeeInfoById(empId);
                        if (employeeBO.EmpId > 0)
                        {
                            glCompanyId = employeeBO.EmpCompanyId;
                        }
                    }
                }

                if (glCompanyId > 0)
                {
                    GLCompanyBO glCompanyBO = new GLCompanyBO();
                    GLCompanyDA glCompanyDA = new GLCompanyDA();
                    glCompanyBO = glCompanyDA.GetGLCompanyInfoById(glCompanyId);
                    if (glCompanyBO != null)
                    {
                        if (glCompanyBO.CompanyId > 0)
                        {
                            companyName = glCompanyBO.Name;
                            if (!string.IsNullOrWhiteSpace(glCompanyBO.CompanyAddress))
                            {
                                companyAddress = glCompanyBO.CompanyAddress;
                            }
                            if (!string.IsNullOrWhiteSpace(glCompanyBO.WebAddress))
                            {
                                webAddress = glCompanyBO.WebAddress;
                            }
                            if (!string.IsNullOrWhiteSpace(glCompanyBO.Telephone))
                            {
                                telephoneNumber = glCompanyBO.Telephone;
                            }
                            if (!string.IsNullOrWhiteSpace(glCompanyBO.HotLineNumber))
                            {
                                hotLineNumber = glCompanyBO.HotLineNumber;
                            }
                            if (!string.IsNullOrWhiteSpace(glCompanyBO.ImageName))
                            {
                                imageName = glCompanyBO.ImageName;
                            }
                        }
                    }
                }

                DateTime currentDate = DateTime.Now;
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
                string footerPoweredByInfo = string.Empty;
                footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                reportParam.Add(new ReportParameter("CompanyProfile", companyName));
                reportParam.Add(new ReportParameter("CompanyAddress", companyAddress));
                reportParam.Add(new ReportParameter("CompanyWeb", webAddress));
                reportParam.Add(new ReportParameter("TelephoneNumber", telephoneNumber));
                reportParam.Add(new ReportParameter("HotLineNumber", hotLineNumber));

                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
                reportParam.Add(new ReportParameter("PrintDateTime", printDate));
                reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                reportParam.Add(new ReportParameter("PoweredByQrCode", Convert.ToBase64String(userInformationBO.PoweredByQrCode)));
                reportParam.Add(new ReportParameter("ReportYear", year.ToString()));
                reportParam.Add(new ReportParameter("PayrollProvidentFundTitleText", userInformationBO.PayrollProvidentFundTitleText));
                rvTransaction.LocalReport.SetParameters(reportParam);

                EmpPFDA empPFDA = new EmpPFDA();
                List<PFStatementReportViewBO> viewList = new List<PFStatementReportViewBO>();
                viewList = empPFDA.GetPFStatementReportInfo(processDateTo, year, departmentId, empId);

                HMCommonDA hmCommonQrImageDA = new HMCommonDA();
                foreach (PFStatementReportViewBO row in viewList)
                {
                    string strQrCode = string.Empty;
                    strQrCode = userInformationBO.PayrollProvidentFundTitleText + " Balance; " + row.LastDeductedMonthYear + "; " + row.EmpCode + "; " + row.CurrencyName + " " + row.TotalContribution + "; " + printDate + ";";
                    row.QrEmployeeImage = hmCommonQrImageDA.GenerateQrCode(strQrCode);
                }

                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList));

                rvTransaction.LocalReport.DisplayName = userInformationBO.PayrollProvidentFundTitleText + " Statement";
                rvTransaction.LocalReport.Refresh();
            }
        }
    }
}