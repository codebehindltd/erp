using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmBankSalaryInstruction : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadYearList();
                LoadBankInfo();
                LoadSalaryProcessMonth();
                LoadGLCompany();
            }
        }
        private void LoadSalaryProcessMonth()
        {
            EmpSalaryProcessDA entityDA = new EmpSalaryProcessDA();
            this.ddlProcessMonth.DataSource = entityDA.GetSalaryProcessMonth();
            this.ddlProcessMonth.DataTextField = "MonthHead";
            this.ddlProcessMonth.DataValueField = "MonthValue";
            this.ddlProcessMonth.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlProcessMonth.Items.Insert(0, item);
        }
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
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ddlProcessMonth.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Process Month.", AlertType.Warning);
                    this.ddlProcessMonth.Focus();
                }
                else if (this.ddlYear.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Process Year.", AlertType.Warning);
                    this.ddlYear.Focus();
                }
                else if (this.ddlBankId.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Bank Name.", AlertType.Warning);
                    this.ddlBankId.Focus();
                }
                else
                {
                    dispalyReport = 1;

                    int bankId = 0;
                    string empId = string.Empty, leaveType = string.Empty, selectedMonthRange = string.Empty;

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    bankId = Convert.ToInt32(ddlBankId.SelectedValue);

                    BankBO bankBO = new BankBO();
                    BankDA bankDA = new BankDA();
                    bankBO = bankDA.GetBankInfoById(bankId);
                    if (bankBO.BankId > 0)
                    {
                        selectedMonthRange = ddlProcessMonth.SelectedValue;
                        DateTime salaryDateFrom = DateTime.Now, salaryDateTo = DateTime.Now;

                        salaryDateFrom = Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedMonthRange, ddlYear.SelectedValue));
                        salaryDateTo = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue));

                        rvTransaction.LocalReport.DataSources.Clear();
                        rvTransaction.ProcessingMode = ProcessingMode.Local;
                        rvTransaction.LocalReport.EnableExternalImages = true;

                        string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptBankSalaryInstruction.rdlc");

                        HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                        HMCommonSetupBO BankSalaryInstructionFormatBO = new HMCommonSetupBO();
                        BankSalaryInstructionFormatBO = commonSetupDA.GetCommonConfigurationInfo("BankSalaryInstructionFormat", "BankSalaryInstructionFormat");
                        if (BankSalaryInstructionFormatBO != null)
                        {
                            if (BankSalaryInstructionFormatBO.SetupValue == "1")
                            {
                                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptBankSalaryInstruction.rdlc");
                            }
                            else if (BankSalaryInstructionFormatBO.SetupValue == "2")
                            {
                                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptBankSalaryInstruction02.rdlc");
                            }
                            else if (BankSalaryInstructionFormatBO.SetupValue == "3")
                            {
                                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptBankSalaryInstruction03.rdlc");
                            }
                        }

                        if (!File.Exists(reportPath))
                            return;

                        rvTransaction.LocalReport.ReportPath = reportPath;

                        CompanyDA companyDA = new CompanyDA();
                        List<CompanyBO> files = companyDA.GetCompanyInfo();

                        HMCommonDA hmCommonDA = new HMCommonDA();
                        List<ReportParameter> reportParam = new List<ReportParameter>();
                        //-- Company Logo -------------------------------
                        string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");

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

                        reportParam.Add(new ReportParameter("CompanyProfile", companyName));
                        reportParam.Add(new ReportParameter("CompanyAddress", companyAddress));
                        reportParam.Add(new ReportParameter("CompanyWeb", webAddress));
                        reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
                        reportParam.Add(new ReportParameter("BankName", bankBO.BankName));
                        reportParam.Add(new ReportParameter("BankAccountName", bankBO.AccountName));
                        reportParam.Add(new ReportParameter("BankAccountNumber", bankBO.AccountNumber));

                        DateTime currentDate = DateTime.Now;
                        string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
                        string footerPoweredByInfo = string.Empty;

                        footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                        reportParam.Add(new ReportParameter("PrintDateTime", printDate));
                        reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                        reportParam.Add(new ReportParameter("ReportProcessMonthNYear", this.ddlProcessMonth.SelectedItem.Text + " " + this.ddlYear.SelectedValue));
                        reportParam.Add(new ReportParameter("ReportProcessBank", this.ddlBankId.SelectedItem.Text));
                        rvTransaction.LocalReport.SetParameters(reportParam);

                        EmployeeDA eveDA = new EmployeeDA();
                        List<BankSalaryAdviceBO> bankSalaryAdvice = new List<BankSalaryAdviceBO>();
                        bankSalaryAdvice = eveDA.GetBankReconciliationForReport(glCompanyId, bankId, salaryDateFrom, salaryDateTo, Convert.ToInt16(ddlYear.SelectedValue));

                        var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                        rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], bankSalaryAdvice));

                        rvTransaction.LocalReport.DisplayName = "Bank Salary Advice";
                        rvTransaction.LocalReport.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);

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
    }
}