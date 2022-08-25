using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using Microsoft.Reporting.WebForms;
using HotelManagement.Entity.HMCommon;
using System.IO;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportPayslip : BasePage
    {
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGLCompany();
                LoadSalaryProcessMonth();
                LoadYearList();
                LoadDepartment();
                LoadGrade();
                LoadWorkStation();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            int departmentId = 0, gradeId = 0, workStationId = 0, empId = 0;
            string currencyType = string.Empty;

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO salaryExecutionProcess = new HMCommonSetupBO();
            salaryExecutionProcess = commonSetupDA.GetCommonConfigurationInfo("PayrollSalaryExecutionProcessType", "PayrollSalaryExecutionProcessType");

            if (this.ddlEffectedMonth.SelectedValue == "0")
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Provide Process Month";
                this.ddlEffectedMonth.Focus();
            }
            else
            {
                HiddenField hfEmployeeId = (HiddenField)this.EmployeeSearchControl.FindControl("hfEmployeeId");
                empId = Convert.ToInt32(hfEmployeeId.Value);

                DateTime processDateFrom = DateTime.Now, processDateTo = DateTime.Now;
                _RoomStatusInfoByDate = 1;

                currencyType = ddlCurrencyType.SelectedValue;

                HMCommonDA hmCommonDA = new HMCommonDA();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                string selectedMonthRange = this.ddlEffectedMonth.SelectedValue.ToString();
                processDateFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedMonthRange, ddlYear.SelectedValue));
                processDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue));

                HMCommonSetupBO isOnlyPdfEnableWhenReportExportBO = new HMCommonSetupBO();
                isOnlyPdfEnableWhenReportExportBO = commonSetupDA.GetCommonConfigurationInfo("IsOnlyPdfEnableWhenReportExport", "IsOnlyPdfEnableWhenReportExport");
                if (isOnlyPdfEnableWhenReportExportBO != null)
                {
                    if (isOnlyPdfEnableWhenReportExportBO.SetupValue == "1")
                    {
                        if (hmUtility.GetCurrentApplicationUserInfo().IsAdminUser != true)
                        {
                            CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Excel.ToString());
                            CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Word.ToString());
                        }
                    }
                }

                rvTransaction.LocalReport.DataSources.Clear();
                rvTransaction.ProcessingMode = ProcessingMode.Local;
                rvTransaction.ShowPrintButton = true;
                
                string reportName = string.Empty;

                if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.Regular)
                {
                    reportName = "EmpPayslip";
                }
                else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.RedCross)
                {
                    reportName = "EmpPayslipRedcross";
                }
                else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.IPTech)
                {
                    reportName = "EmpPayslipForIPTech";
                }
                else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.SouthSudan)
                {
                    reportName = "EmpPayslipSouthSudan";
                }

                var reportPath = "";
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/" + reportName.Trim() + ".rdlc");

                if (!File.Exists(reportPath))
                    return;

                rvTransaction.LocalReport.ReportPath = reportPath;
                departmentId = Convert.ToInt32(ddlDepartmentId.SelectedValue);
                gradeId = Convert.ToInt32(ddlGrade.SelectedValue);

                workStationId = !string.IsNullOrWhiteSpace(ddlWorkStation.SelectedValue) ? Convert.ToInt32(ddlWorkStation.SelectedValue) : 0;
                List<ReportParameter> paramReport = new List<ReportParameter>();

                string companyName = string.Empty;
                string companyAddress = string.Empty;
                string webAddress = string.Empty;

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> files = companyDA.GetCompanyInfo();
                if (files[0].CompanyId > 0)
                {
                    companyName = files[0].CompanyName;
                    companyAddress = files[0].CompanyAddress;

                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        webAddress = files[0].WebAddress;
                    }
                    else
                    {
                        webAddress = files[0].ContactNumber;
                    }
                }

                DateTime currentDate = DateTime.Now;
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
                string footerPoweredByInfo = string.Empty;

                footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                paramReport.Add(new ReportParameter("PrintDateTime", printDate));

                string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
                rvTransaction.LocalReport.EnableExternalImages = true;

                int glCompanyId = Convert.ToInt32(ddlGLCompany.SelectedValue);

                EmployeeDA empDA = new EmployeeDA();
                List<EmployeePayslipBO> payslip = new List<EmployeePayslipBO>();

                if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.Regular)
                {
                    payslip = empDA.GetEmployeePayslip(glCompanyId, empId, processDateFrom, processDateTo, Convert.ToInt16(ddlYear.SelectedValue), departmentId, gradeId, workStationId, currencyType);
                }
                else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.RedCross)
                {
                    payslip = empDA.GetEmployeePayslipForRedcross(glCompanyId, empId, processDateFrom, processDateTo, Convert.ToInt16(ddlYear.SelectedValue), departmentId, gradeId, workStationId);
                }
                else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.IPTech)
                {
                    payslip = empDA.GetEmployeePayslipForIPTech(glCompanyId, empId, processDateFrom, processDateTo, Convert.ToInt16(ddlYear.SelectedValue), departmentId, gradeId, workStationId, currencyType);
                }
                else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.SouthSudan)
                {
                    payslip = empDA.GetEmployeePayslipForSouthSudan(glCompanyId, empId, processDateFrom, processDateTo, Convert.ToInt16(ddlYear.SelectedValue), departmentId, gradeId, workStationId);
                }

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
                            if (!string.IsNullOrWhiteSpace(glCompanyBO.ImageName))
                            {
                                imageName = glCompanyBO.ImageName;
                            }
                        }
                    }
                }

                //byte[] QrImage;
                //HMCommonDA hmCommonQrImageDA = new HMCommonDA();
                //QrImage = hmCommonQrImageDA.GenerateQrCode(reportName + "; " + files[0].CompanyName + "; " + files[0].CompanyAddress + ";");
                //paramReport.Add(new ReportParameter("QrImage", Convert.ToBase64String(QrImage)));

                HMCommonDA hmCommonQrImageDA = new HMCommonDA();
                foreach (EmployeePayslipBO row in payslip)
                {
                    string strQrCode = string.Empty;
                    //strQrCode = "Payslip" + "; " + row.EmpCode + "; " + row.EmployeeName + "; " + row.TotalAllowance + "; " + companyName + "; " + companyAddress + "; " + printDate + ";";
                    strQrCode = "Payslip" + "; " + row.EmpCode + "; " + row.TotalAllowance + "; " + companyName + "; " + printDate + ";";
                    row.QrEmployeeImage = hmCommonQrImageDA.GenerateQrCode(strQrCode);
                }

                paramReport.Add(new ReportParameter("CompanyProfile", companyName));
                paramReport.Add(new ReportParameter("CompanyAddress", companyAddress));
                paramReport.Add(new ReportParameter("CompanyWeb", webAddress));
                paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
                paramReport.Add(new ReportParameter("month", ddlEffectedMonth.SelectedItem.ToString()));
                paramReport.Add(new ReportParameter("year", ddlYear.SelectedItem.ToString()));
                paramReport.Add(new ReportParameter("CurrencyType", ddlCurrencyType.SelectedValue.ToString()));

                rvTransaction.LocalReport.SetParameters(paramReport);
                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], payslip));

                rvTransaction.LocalReport.DisplayName = "Employee Payslip";
                rvTransaction.LocalReport.Refresh();
            }

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();
            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Portrait.ToString());
            frmPrint.Attributes["src"] = reportSource;
        }
        //************************ User Defined Function ********************//
        private void LoadGLCompany()
        {
            hfIsSingle.Value = "0";
            GLCompanyDA entityDA = new GLCompanyDA();
            var List = entityDA.GetAllGLCompanyInfo();

            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            if (List.Count == 1)
            {
                companyList.Add(List[0]);
                ddlGLCompany.DataSource = companyList;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
                hfIsSingle.Value = "1";
            }
            else
            {
                ddlGLCompany.DataSource = List;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
                hfIsSingle.Value = "0";
                System.Web.UI.WebControls.ListItem itemCompany = new System.Web.UI.WebControls.ListItem();
                itemCompany.Value = "0";
                itemCompany.Text = hmUtility.GetDropDownFirstAllValue();
                ddlGLCompany.Items.Insert(0, itemCompany);
            }
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
            this.ddlEffectedMonth.SelectedIndex = DateTime.Now.Month - 1;
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlEffectedMonth.Items.Insert(0, item);
        }
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            this.ddlDepartmentId.DataSource = entityDA.GetDepartmentInfo();
            this.ddlDepartmentId.DataTextField = "Name";
            this.ddlDepartmentId.DataValueField = "DepartmentId";
            this.ddlDepartmentId.DataBind();

            ddlDepartmentId.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Text = hmUtility.GetDropDownFirstAllValue(), Value = "0" });
        }
        private void LoadGrade()
        {
            EmpGradeDA gradeDA = new EmpGradeDA();
            var List = gradeDA.GetGradeInfo(); ;
            this.ddlGrade.DataSource = List;
            this.ddlGrade.DataTextField = "Name";
            this.ddlGrade.DataValueField = "GradeId";
            this.ddlGrade.DataBind();

            this.ddlGrade.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Text = hmUtility.GetDropDownFirstAllValue(), Value = "0" });
        }
        private void LoadWorkStation()
        {
            EmployeeDA workStationDA = new EmployeeDA();
            List<EmpWorkStationBO> entityBOList = new List<EmpWorkStationBO>();
            entityBOList = workStationDA.GetEmployWorkStation();

            this.ddlWorkStation.DataSource = entityBOList;
            this.ddlWorkStation.DataTextField = "WorkStationName";
            this.ddlWorkStation.DataValueField = "WorkStationId";
            this.ddlWorkStation.DataBind();

            ddlWorkStation.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Text = hmUtility.GetDropDownFirstAllValue(), Value = "0" });
        }
    }
}