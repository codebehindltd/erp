using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
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

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class EmployeeInfoIframe : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        private System.Data.DataSet dsReport;
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        int EmpId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EmpId = Convert.ToInt32(Request.QueryString["empId"]);
                if (EmpId != 0)
                {
                    GenerateReport(EmpId);
                }
                
            }
        }
        private void GenerateReport(int Id)
        {
            _RoomStatusInfoByDate = 1;

            //Documents Info
            DocumentsDA docDA = new DocumentsDA();
            List<DocumentsBO> docBO = new List<DocumentsBO>();
            docBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("Employee Document", Convert.ToInt32(Id));

            CompanyDA companyDA = new CompanyDA();
            EmployeeDA employeeDA = new EmployeeDA();

            List<CompanyBO> files = companyDA.GetCompanyInfo();
            if (files != null)
            {
                if (files.Count > 0)
                {
                    rvTransaction.LocalReport.DataSources.Clear();
                    rvTransaction.ProcessingMode = ProcessingMode.Local;
                    rvTransaction.LocalReport.EnableExternalImages = true;

                    string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeInformation.rdlc");

                    if (!File.Exists(reportPath))
                        return;

                    rvTransaction.LocalReport.ReportPath = reportPath;

                    List<ReportParameter> reportParam = new List<ReportParameter>();
                    reportParam.Add(new ReportParameter("CompanyName", files[0].CompanyName));
                    reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        reportParam.Add(new ReportParameter("CompanyWebAddress", files[0].WebAddress));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("CompanyWebAddress", files[0].ContactNumber));
                    }

                    DateTime currentDate = DateTime.Now;
                    string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
                    string footerPoweredByInfo = string.Empty;
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                    footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                    reportParam.Add(new ReportParameter("PrintDateTime", printDate));
                    reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                    if (docBO.Count == 0)
                    {
                        reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, @"/Payroll/Images/Documents/defaultempimg.png")));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, docBO[0].Path + docBO[0].Name)));
                    }
                    HMCommonSetupBO setUpBO = new HMCommonSetupBO();
                    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                    
                    setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollWorkStationHide", "IsPayrollWorkStationHide");
                    reportParam.Add(new ReportParameter("IsPayrollWorkStationHide", setUpBO.SetupValue));

                    setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollDonorNameAndActivityCodeHide", "IsPayrollDonorNameAndActivityCodeHide");
                    reportParam.Add(new ReportParameter("IsPayrollDonorNameAndActivityCodeHide", setUpBO.SetupValue));

                    setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollReferenceHide", "IsPayrollReferenceHide");
                    reportParam.Add(new ReportParameter("IsPayrollReferenceHide", setUpBO.SetupValue));

                    setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollDependentHide", "IsPayrollDependentHide");
                    reportParam.Add(new ReportParameter("IsPayrollDependentHide", setUpBO.SetupValue));

                    rvTransaction.LocalReport.SetParameters(reportParam);

                    List<EmployeeBO> employeeList = new List<EmployeeBO>();
                    employeeList = employeeDA.GetEmployeeInfoForReport(Convert.ToInt32(Id));

                    var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
                    rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], employeeList));

                    rvTransaction.LocalReport.DisplayName = "Employee Information";

                    rvTransaction.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubReportProcessingForInvoiceDetail);
                }
            }

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
        private void SubReportProcessingForInvoiceDetail(object sender, SubreportProcessingEventArgs e)
        {
            int empId = Convert.ToInt32(EmpId);

            EmpEducationDA entityEducationDA = new EmpEducationDA();
            List<EmpEducationBO> empEducation = entityEducationDA.GetEmpEducationEmpId(empId);
            e.DataSources.Add(new ReportDataSource("EmployeeEducationSubReportData", empEducation));

            EmpExperienceDA entityExperienceDA = new EmpExperienceDA();
            List<EmpExperienceBO> empExperience = entityExperienceDA.GetEmpExperienceByEmpId(empId);
            e.DataSources.Add(new ReportDataSource("EmployeeExperience", empExperience));

            EmpDependentDA entityDependentDA = new EmpDependentDA();
            List<EmpDependentBO> empDependent = entityDependentDA.GetEmpDependentByEmpId(empId);
            e.DataSources.Add(new ReportDataSource("EmployeeDependent", empDependent));

            EmpNomineeDA nomineeDa = new EmpNomineeDA();
            List<EmpNomineeBO> empNominee = nomineeDa.GetEmpNomineeByEmpId(empId);
            e.DataSources.Add(new ReportDataSource("EmployeeNominee", empNominee));

            List<EmpReferenceBO> empReference = new List<EmpReferenceBO>();
            EmpReferenceDA referenceDA = new EmpReferenceDA();
            empReference = referenceDA.GetEmpReferenceByEmpId(empId);
            e.DataSources.Add(new ReportDataSource("TotalReferenceList", empReference));
        }
    }
}