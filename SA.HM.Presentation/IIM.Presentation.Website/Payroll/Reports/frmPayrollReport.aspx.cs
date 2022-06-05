using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmPayrollReport : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string param = Request.QueryString["TId"];
                string[] param1 = param.Split(',');
                string strTransactionId = param1[0];
                string reportType = param1[1];

                if (!string.IsNullOrWhiteSpace(strTransactionId) && !string.IsNullOrEmpty(reportType))
                {
                    int transactionId = Convert.ToInt32(strTransactionId);
                    if (transactionId > 0)
                    {
                        GenerateReport(transactionId, reportType);
                    }
                }
            }
        }

        private void GenerateReport(int transactionId, string reportType)
        {
            dispalyReport = 1;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            Boolean isPayrollLetterEnableWithoutHeaderFooter = false;
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isPayrollLetterEnableWithoutHeaderFooterBO = new HMCommonSetupBO();
            isPayrollLetterEnableWithoutHeaderFooterBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollLetterEnableWithoutHeaderFooter", "IsPayrollLetterEnableWithoutHeaderFooter");
            if (isPayrollLetterEnableWithoutHeaderFooterBO != null)
            {
                if (isPayrollLetterEnableWithoutHeaderFooterBO.SetupValue != "0")
                {
                    isPayrollLetterEnableWithoutHeaderFooter = true;
                }
            }

            string reportPath = "";
            if (reportType == "Promotion")
            {
                if (isPayrollLetterEnableWithoutHeaderFooter)
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpPromotionLetterWTHF.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpPromotionLetter.rdlc");
                }
            }
            else if (reportType == "Increment")
            {
                if (isPayrollLetterEnableWithoutHeaderFooter)
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpIncrementLetterWTHF.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpIncrementLetter.rdlc");
                }
            }
            else if (reportType == "AppoinmentLetter")
            {
                if (isPayrollLetterEnableWithoutHeaderFooter)
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpAppoinmentLetterWTHF.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpAppoinmentLetter.rdlc");
                }
            }
            else if (reportType == "JoiningAgreement")
            {
                if (isPayrollLetterEnableWithoutHeaderFooter)
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpJoiningAgreementWTHF.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpJoiningAgreement.rdlc");
                }
            }
            else if (reportType == "ServiceBond")
            {
                if (isPayrollLetterEnableWithoutHeaderFooter)
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpServiceBondWTHF.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpServiceBond.rdlc");
                }
            }
            else if (reportType == "DSOAC")
            {
                if (isPayrollLetterEnableWithoutHeaderFooter)
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpDSOACWTHF.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpDSOAC.rdlc");
                }
            }
            else if (reportType == "ConfirmationLetter")
            {
                if (isPayrollLetterEnableWithoutHeaderFooter)
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpConfirmationLetterWTHF.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmpConfirmationLetter.rdlc");
                }
            }


            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            HMCommonDA hmCommonDA = new HMCommonDA();
            string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");

            EmployeeDA empDA = new EmployeeDA();
            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();

            if (reportType == "Promotion")
            {
                empList = empDA.GetEmpPromotionLetterForReport(transactionId);
            }
            else if (reportType == "Increment")
            {
                empList = empDA.GetEmpIncrementLetterForReport(transactionId);
            }

            else if (reportType == "AppoinmentLetter")
            {
                empList = empDA.GetEmpAppoinmentLetterForReport(transactionId);
            }
            else if (reportType == "JoiningAgreement")
            {
                empList = empDA.GetEmpJoiningAgreementForReport(transactionId);
            }
            else if (reportType == "ServiceBond")
            {
                empList = empDA.GetEmpServiceBondLetterForReport(transactionId);
            }
            else if (reportType == "DSOAC")
            {
                empList = empDA.GetEmpDSOACLetterForReport(transactionId);
            }
            else if (reportType == "ConfirmationLetter")
            {
                empList = empDA.GetEmpConfirmationLetterForReport(transactionId);
            }

            List<ReportParameter> reportParam = new List<ReportParameter>();

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

            int glCompanyId = 0;
            if (empList != null)
            {
                if (empList.Count > 0)
                {
                    glCompanyId = empList[0].GLCompanyId;
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

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            reportParam.Add(new ReportParameter("CompanyProfile", companyName));
            reportParam.Add(new ReportParameter("CompanyAddress", companyAddress));
            reportParam.Add(new ReportParameter("CompanyWeb", webAddress));

            rvTransaction.LocalReport.SetParameters(reportParam);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], empList));

            if (reportType == "Promotion")
            {
                rvTransaction.LocalReport.DisplayName = "Promotion Letter.";
            }
            else if (reportType == "Increment")
            {
                rvTransaction.LocalReport.DisplayName = "Increment Letter.";
            }
            else if (reportType == "AppoinmentLetter")
            {
                rvTransaction.LocalReport.DisplayName = "Appoinment Letter.";
            }
            else if (reportType == "JoiningAgreement")
            {
                rvTransaction.LocalReport.DisplayName = "Joining Agreement.";
            }
            else if (reportType == "ServiceBond")
            {
                rvTransaction.LocalReport.DisplayName = "Service Bond.";
            }
            else if (reportType == "DSOAC")
            {
                rvTransaction.LocalReport.DisplayName = "DSOAC.";
            }

            rvTransaction.LocalReport.Refresh();
        }
    }
}