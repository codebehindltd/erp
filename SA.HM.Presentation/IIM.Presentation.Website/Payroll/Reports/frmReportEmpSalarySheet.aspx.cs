using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using System.Web.Services;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportEmpSalarySheet : BasePage
    {
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployeeCompany();
                LoadGLCompany();
                LoadDepartment();
                LoadDesignation();
                LoadGrade();
                LoadYearList();
                LoadSalaryProcessMonth();
                LoadWorkStation();
            }
        }
        private void LoadEmployeeCompany()
        {
            hfIsSingle.Value = "0";
            GLCompanyDA entityDA = new GLCompanyDA();
            List<GLCompanyBO> GLCompanyBOList = new List<GLCompanyBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.UserInfoId == 1)
            {
                GLCompanyBOList = entityDA.GetAllGLCompanyInfo();
            }
            else
            {
                GLCompanyBOList = entityDA.GetAllGLCompanyInfoByUserGroupId(userInformationBO.UserGroupId);
            }


            if (GLCompanyBOList.Count == 1)
            {
                hfIsSingle.Value = "1";
            }
            else
            {
                hfIsSingle.Value = "0";
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;
            DateTime processDateFrom = DateTime.Now, processDateTo = DateTime.Now;
            int departmentId = 0, designationId = 0, gradeId = 0, employeeId = 0, branchId = 0;

            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO salaryExecutionProcess = new HMCommonSetupBO();
            salaryExecutionProcess = commonSetupDA.GetCommonConfigurationInfo("PayrollSalaryExecutionProcessType", "PayrollSalaryExecutionProcessType");

            string reportName = string.Empty, currencyType = string.Empty;
            currencyType = ddlCurrencyType.SelectedValue;

            if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.Regular)
            {
                if (ddlReportType.SelectedValue == "All")
                {
                    if (ddlCurrencyType.SelectedValue == "LocalCurrency")
                    {
                        if (hfIsSingle.Value == "1")
                        {
                            //reportName = "RptEmployeeSalarySheetsForSingleCompany";
                            if (ddlReportFormat.SelectedValue == "Regular")
                            {
                                reportName = "RptEmployeeSalarySheetsForSingleCompany";
                            }
                            else if (ddlReportFormat.SelectedValue == "DepartmentWise")
                            {
                                reportName = "RptDepartmentWiseEmployeeSalarySheetsForSingleCompany";
                            }
                            else if (ddlReportFormat.SelectedValue == "DesignationWise")
                            {
                                reportName = "RptDesignationWiseEmployeeSalarySheetsForSingleCompany";
                            }
                            else if (ddlReportFormat.SelectedValue == "WorkStationWise")
                            {
                                reportName = "RptWorkStationWiseEmployeeSalarySheetsForSingleCompany";
                            }
                            else if (ddlReportFormat.SelectedValue == "GenderWise")
                            {
                                reportName = "RptGenderWiseEmployeeSalarySheetsForSingleCompany";
                            }
                        }
                        else
                        {
                            if (ddlReportFormat.SelectedValue == "Regular")
                            {
                                reportName = "RptEmployeeSalarySheets";
                            }
                            else if (ddlReportFormat.SelectedValue == "DepartmentWise")
                            {
                                reportName = "RptDepartmentWiseEmployeeSalarySheets";
                            }
                            else if (ddlReportFormat.SelectedValue == "DesignationWise")
                            {
                                reportName = "RptDesignationWiseEmployeeSalarySheets";
                            }                            
                            else if (ddlReportFormat.SelectedValue == "WorkStationWise")
                            {
                                reportName = "RptWorkStationWiseEmployeeSalarySheets";
                            }
                            else if (ddlReportFormat.SelectedValue == "GenderWise")
                            {
                                reportName = "RptGenderWiseEmployeeSalarySheets";
                            }
                        }
                    }
                    else
                    {
                        reportName = "RptEmployeeSalarySheetsWithCurrency";
                    }
                }
                else if (ddlReportType.SelectedValue == "Location")
                {
                    reportName = "RptEmployeeSalarySheetDepartmentLocationWise";
                }
            }
            else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.RedCross)
            {
                if (ddlReportType.SelectedValue == "All")
                {
                    reportName = "RptEmployeeSalarySheetRedcross";
                }
                else if (ddlReportType.SelectedValue == "Location")
                {
                    reportName = "RptEmployeeSalarySheetRedcross";
                }
            }
            else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.IPTech)
            {
                reportName = "RptEmployeeSalarySheetIPTech";
            }
            else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.SouthSudan)
            {
                if (ddlReportType.SelectedValue == "All")
                {
                    reportName = "RptEmployeeSalarySheetSouthSudan";
                }
                else if (ddlReportType.SelectedValue == "Location")
                {
                    reportName = "RptEmployeeSalarySheetSouthSudan";
                }
            }

            string selectedMonthRange = this.ddlEffectedMonth.SelectedValue.ToString();
            processDateFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedMonthRange, ddlYear.SelectedValue));
            processDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue));

            CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.PDF.ToString());
            CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Word.ToString());

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/" + reportName + ".rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            //-- Company Logo -------------------------------
            string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            List<ReportParameter> paramReport = new List<ReportParameter>();
            paramReport.Add(new ReportParameter("month", ddlEffectedMonth.SelectedItem.ToString()));
            paramReport.Add(new ReportParameter("year", ddlYear.SelectedItem.ToString()));
            paramReport.Add(new ReportParameter("CurrencyType", ddlCurrencyType.SelectedValue.ToString()));

            string companyName = string.Empty;
            string companyAddress = string.Empty;
            string webAddress = string.Empty;

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

            int glCompanyId = Convert.ToInt32(ddlGLCompany.SelectedValue);
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

            paramReport.Add(new ReportParameter("CompanyProfile", companyName));
            paramReport.Add(new ReportParameter("CompanyAddress", companyAddress));
            paramReport.Add(new ReportParameter("CompanyWeb", webAddress));
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;

            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));

            rvTransaction.LocalReport.SetParameters(paramReport);
            //-- Company Logo ------------------End----------

            HiddenField hfEmployeeId = (HiddenField)this.EmployeeSearchControl.FindControl("hfEmployeeId");

            employeeId = Convert.ToInt32(hfEmployeeId.Value);
            departmentId = Convert.ToInt32(ddlDepartmentId.SelectedValue);
            designationId = Convert.ToInt32(ddlDesignationId.SelectedValue);
            gradeId = Convert.ToInt32(ddlGrade.SelectedValue);
            branchId = Convert.ToInt32(ddlWorkStation.SelectedValue);

            EmpSalaryProcessDA salaryProcessDA = new EmpSalaryProcessDA();
            List<SalarySheetBO> salarySheetList = new List<SalarySheetBO>();

            if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.Regular)
            {
                salarySheetList = salaryProcessDA.EmployeeSalarySheets(glCompanyId, "SalarySheet", employeeId, departmentId, designationId, gradeId, branchId, processDateFrom, processDateTo, Convert.ToInt16(ddlYear.SelectedValue), currencyType);
            }
            else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.RedCross)
            {
                salarySheetList = salaryProcessDA.EmployeeSalarySheetForRedcross(glCompanyId, "SalarySheet", employeeId, departmentId, designationId, gradeId, branchId, processDateFrom, processDateTo, Convert.ToInt16(ddlYear.SelectedValue));
            }
            else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.IPTech)
            {
                salarySheetList = salaryProcessDA.EmployeeSalarySheetForIPTech(glCompanyId, employeeId, departmentId, designationId, gradeId, branchId, processDateFrom, processDateTo, Convert.ToInt16(ddlYear.SelectedValue), currencyType);
            }
            else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.SouthSudan)
            {
                salarySheetList = salaryProcessDA.EmployeeSalarySheetForSouthSudan(glCompanyId, "SalarySheet", employeeId, departmentId, designationId, gradeId, branchId, processDateFrom, processDateTo, Convert.ToInt16(ddlYear.SelectedValue));
            }

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salarySheetList));

            rvTransaction.LocalReport.DisplayName = "Employee Salary Sheet";
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
                ListItem itemCompany = new ListItem();
                itemCompany.Value = "0";
                itemCompany.Text = hmUtility.GetDropDownFirstAllValue();
                ddlGLCompany.Items.Insert(0, itemCompany);
            }
        }
        private void LoadSalaryProcessMonth()
        {
            EmpSalaryProcessDA entityDA = new EmpSalaryProcessDA();
            this.ddlEffectedMonth.DataSource = entityDA.GetSalaryProcessMonth();
            this.ddlEffectedMonth.DataTextField = "MonthHead";
            this.ddlEffectedMonth.DataValueField = "MonthValue";
            this.ddlEffectedMonth.DataBind();
            this.ddlEffectedMonth.SelectedIndex = DateTime.Now.Month - 1;
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlEffectedMonth.Items.Insert(0, item);
        }
        private void LoadYearList()
        {
            ddlYear.DataSource = hmUtility.GetReportYearList();
            ddlYear.DataBind();
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
        }
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            this.ddlDepartmentId.DataSource = entityDA.GetDepartmentInfo();
            this.ddlDepartmentId.DataTextField = "Name";
            this.ddlDepartmentId.DataValueField = "DepartmentId";
            this.ddlDepartmentId.DataBind();

            ddlDepartmentId.Items.Insert(0, new ListItem { Text = hmUtility.GetDropDownFirstAllValue(), Value = "0" });
        }
        private void LoadDesignation()
        {
            DesignationDA entityDA = new DesignationDA();
            ddlDesignationId.DataSource = entityDA.GetActiveDesignationInfo();
            ddlDesignationId.DataTextField = "Name";
            ddlDesignationId.DataValueField = "DesignationId";
            ddlDesignationId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlDesignationId.Items.Insert(0, item);
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
        private void LoadGrade()
        {
            EmpGradeDA gradeDA = new EmpGradeDA();
            var List = gradeDA.GetGradeInfo(); ;
            this.ddlGrade.DataSource = List;
            this.ddlGrade.DataTextField = "Name";
            this.ddlGrade.DataValueField = "GradeId";
            this.ddlGrade.DataBind();

            this.ddlGrade.Items.Insert(0, new ListItem { Text = hmUtility.GetDropDownFirstAllValue(), Value = "0" });
        }
    }
}