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
    public partial class frmReportEmpSalarySheetTemp : System.Web.UI.Page
    {
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        string selectedMonthRange = string.Empty, processYear = string.Empty, processId = string.Empty,
               currencyType = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployeeCompany();
                GenerateReport();
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
        private void GenerateReport()
        {
            processId = Request.QueryString["pid"].ToString();
            selectedMonthRange = Request.QueryString["mr"].ToString();
            processYear = Request.QueryString["yr"].ToString();
            currencyType = Request.QueryString["ct"].ToString();

            DateTime processDateFrom = DateTime.Now, processDateTo = DateTime.Now;
            string reportName = string.Empty;

            HMCommonDA hmCommonDA = new HMCommonDA();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            string IsPayrollAttendancePartWillShowOnSalarySheet = "0";
            HMCommonSetupBO IsPayrollAttendancePartWillShowOnSalarySheetBO = new HMCommonSetupBO();
            IsPayrollAttendancePartWillShowOnSalarySheetBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollAttendancePartWillShowOnSalarySheet", "IsPayrollAttendancePartWillShowOnSalarySheet");
            if (IsPayrollAttendancePartWillShowOnSalarySheetBO != null)
            {
                if (IsPayrollAttendancePartWillShowOnSalarySheetBO.SetupId > 0)
                {
                    IsPayrollAttendancePartWillShowOnSalarySheet = IsPayrollAttendancePartWillShowOnSalarySheetBO.SetupValue;
                }
            }

            HMCommonSetupBO salaryExecutionProcess = new HMCommonSetupBO();
            salaryExecutionProcess = commonSetupDA.GetCommonConfigurationInfo("PayrollSalaryExecutionProcessType", "PayrollSalaryExecutionProcessType");

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            processDateFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedMonthRange, processYear));
            processDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, processYear));

            CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.PDF.ToString());
            CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Word.ToString());

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";

            if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.Regular)
            {
                if (hfIsSingle.Value == "1")
                {
                    if (IsPayrollAttendancePartWillShowOnSalarySheet == "0")
                    {
                        reportName = "RptEmployeeSalarySheetsForSingleCompany";
                    }
                    else
                    {
                        reportName = "RptEmployeeSalarySheetsForSingleCompanyWAS";
                    }
                }
                else
                {
                    if (IsPayrollAttendancePartWillShowOnSalarySheet == "0")
                    {
                        reportName = "RptEmployeeSalarySheets";
                    }
                    else
                    {
                        reportName = "RptEmployeeSalarySheetsWAS";
                    }
                }
            }
            else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.RedCross)
            {
                reportName = "RptEmployeeSalarySheetTempRedcross";
            }
            else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.IPTech)
            {
                reportName = "RptEmployeeSalarySheetTempIPTech";
            }
            else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.SouthSudan)
            {
                reportName = "RptEmployeeSalarySheetTempSouthSudan";
            }

            reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/" + reportName + ".rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            //-- Company Logo -------------------------------
            string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            List<ReportParameter> paramLogo = new List<ReportParameter>();
            paramLogo.Add(new ReportParameter("WaterMarkImagePath", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/WaterMarkTextA4.png")));
            paramLogo.Add(new ReportParameter("month", processDateFrom.ToString("MMM")));
            paramLogo.Add(new ReportParameter("year", processYear));

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

            EmpSalaryProcessDA salaryProcessDA = new EmpSalaryProcessDA();
            HMCommonSetupBO IsCompanyProjectWiseEmployeeSalaryProcessEnable = new HMCommonSetupBO();
            IsCompanyProjectWiseEmployeeSalaryProcessEnable = commonSetupDA.GetCommonConfigurationInfo("IsCompanyProjectWiseEmployeeSalaryProcessEnable", "IsCompanyProjectWiseEmployeeSalaryProcessEnable");
            if (IsCompanyProjectWiseEmployeeSalaryProcessEnable != null)
            {
                if (IsCompanyProjectWiseEmployeeSalaryProcessEnable.SetupValue == "1")
                {
                    SalaryProcessMonthBO SalaryProcessMonthBO = new SalaryProcessMonthBO();
                    SalaryProcessMonthBO = salaryProcessDA.GetTemporarySalaryProcessInformationByProcessId(Convert.ToInt32(processId));
                    if (SalaryProcessMonthBO != null)
                    {
                        if (SalaryProcessMonthBO.CompanyId > 0)
                        {
                            GLCompanyBO glCompanyBO = new GLCompanyBO();
                            GLCompanyDA glCompanyDA = new GLCompanyDA();
                            glCompanyBO = glCompanyDA.GetGLCompanyInfoById(SalaryProcessMonthBO.CompanyId);
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
                    }
                }
            }            

            paramLogo.Add(new ReportParameter("CompanyProfile", companyName));
            paramLogo.Add(new ReportParameter("CompanyAddress", companyAddress));
            paramLogo.Add(new ReportParameter("CompanyWeb", webAddress));
            paramLogo.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;

            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            paramLogo.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramLogo.Add(new ReportParameter("PrintDateTime", printDate));

            rvTransaction.LocalReport.SetParameters(paramLogo);

            List<SalarySheetBO> salarySheetList = new List<SalarySheetBO>();

            if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.Regular)
            {
                salarySheetList = salaryProcessDA.EmployeeSalarySheetsTemp(Convert.ToInt32(processId), processDateFrom, processDateTo, Convert.ToInt16(processYear), currencyType);
            }
            else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.RedCross)
            {
                salarySheetList = salaryProcessDA.EmployeeSalarySheetsTempForRedcross(Convert.ToInt32(processId), processDateFrom, processDateTo, Convert.ToInt16(processYear));
            }
            else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.IPTech)
            {
                salarySheetList = salaryProcessDA.EmployeeSalarySheetsTempForIPTech(Convert.ToInt32(processId), processDateFrom, processDateTo, Convert.ToInt16(processYear), currencyType);
            }
            else if (Convert.ToInt32(salaryExecutionProcess.SetupValue) == (int)HMConstants.PayrollSalaryExecutionProcessType.SouthSudan)
            {
                salarySheetList = salaryProcessDA.EmployeeSalarySheetsTempForSouthSudan(Convert.ToInt32(processId), processDateFrom, processDateTo, Convert.ToInt16(processYear));
            }

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salarySheetList));

            rvTransaction.LocalReport.DisplayName = "Employee Salary Sheet";
            rvTransaction.LocalReport.Refresh();
        }
    }
}