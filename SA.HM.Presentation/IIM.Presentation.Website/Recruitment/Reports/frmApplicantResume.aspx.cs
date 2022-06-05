using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.Payroll;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.Recruitment
{
    public partial class frmApplicantResume : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {                             
                this.LoadEmployee();
            }
        }
        private void LoadEmployee()
        {
            EmployeeDA employeeDA = new EmployeeDA();
            this.ddlEmployee.DataSource = employeeDA.GetApplicantInfo();
            this.ddlEmployee.DataTextField = "EmployeeName";
            this.ddlEmployee.DataValueField = "EmpId";
            this.ddlEmployee.DataBind();

            System.Web.UI.WebControls.ListItem itemEmployee = new System.Web.UI.WebControls.ListItem();
            itemEmployee.Value = "0";
            itemEmployee.Text = hmUtility.GetDropDownFirstValue();
            this.ddlEmployee.Items.Insert(0, itemEmployee);

        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            if (this.ddlEmployee.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Applicant Name.", AlertType.Warning);
                this.ddlEmployee.Focus();
            }
            else
            {                
                int empId = Convert.ToInt32(ddlEmployee.SelectedValue);
                _RoomStatusInfoByDate = 1;

                EmployeeDA employeeDA = new EmployeeDA();

                //Documents Info
                DocumentsDA docDA = new DocumentsDA();
                List<DocumentsBO> docImageBO = new List<DocumentsBO>();
                List<DocumentsBO> docSignatureBO = new List<DocumentsBO>();
                docImageBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("Applicant Document", empId);
                docSignatureBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("Applicant Signature", empId);

                rvTransaction.LocalReport.DataSources.Clear();
                rvTransaction.ProcessingMode = ProcessingMode.Local;
                rvTransaction.LocalReport.EnableExternalImages = true;

                string reportPath = Server.MapPath(@"~/Recruitment/Reports/Rdlc/rptApplicantResume.rdlc");

                if (!File.Exists(reportPath))
                    return;

                rvTransaction.LocalReport.ReportPath = reportPath;

                //General Info
                List<EmployeeBO> employeeList = new List<EmployeeBO>();
                employeeList = employeeDA.GetEmployeeInfoForReport(empId);

                List<ReportParameter> reportParam = new List<ReportParameter>();
                reportParam.Add(new ReportParameter("EmpName", employeeList[0].DisplayName));
                reportParam.Add(new ReportParameter("Address", employeeList[0].PresentAddress));
                reportParam.Add(new ReportParameter("Phone", employeeList[0].PresentPhone));
                reportParam.Add(new ReportParameter("Email", employeeList[0].PersonalEmail));

                if (docImageBO.Count == 0)
                {
                    reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, @"/Recruitment/Images/Documents/defaultempimg.png")));
                }
                else
                {
                    reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, docImageBO[0].Path + docImageBO[0].Name)));
                }

                if (docSignatureBO.Count == 0)
                {
                    reportParam.Add(new ReportParameter("SignaturePath", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, @"/Recruitment/Images/Signature/defaultsignature.jpg")));
                }
                else
                {
                    reportParam.Add(new ReportParameter("SignaturePath", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, docSignatureBO[0].Path + docSignatureBO[0].Name)));
                }

                rvTransaction.LocalReport.SetParameters(reportParam);


                //Training Info
                List<EmpCareerTrainingBO> trainingList = new List<EmpCareerTrainingBO>();
                EmpCareerTrainingDA trainingDA = new EmpCareerTrainingDA();
                trainingList = trainingDA.GetEmpCareerTrainingnByEmpId(empId);

                //Reference Info
                List<EmpReferenceBO> referenceList1 = new List<EmpReferenceBO>();
                List<EmpReferenceBO> referenceList2 = new List<EmpReferenceBO>();
                EmpReferenceDA referenceDA = new EmpReferenceDA();
                referenceList1 = referenceDA.GetEmpReferenceByEmpId(empId);

                long maxRow = 0;
                double mid = 0;

                if (referenceList1 != null)
                {
                    if (referenceList1.Count > 0)
                    {
                        maxRow = referenceList1.Max(m => m.RowRank);
                    }
                }

                if (maxRow > 1)
                {
                    mid = Math.Round(Convert.ToDouble(maxRow) / 2);
                    referenceList2 = referenceList1.Where(w => w.RowRank > mid && w.RowRank <= maxRow).ToList();
                    referenceList1 = referenceList1.Where(w => w.RowRank >= 1 && w.RowRank <= mid).ToList();
                }

                //Education Info
                List<EmpEducationBO> educationList = new List<EmpEducationBO>();
                EmpEducationDA educationDA = new EmpEducationDA();
                educationList = educationDA.GetEmpEducationEmpId(empId);

                //Experience Info
                List<EmpExperienceBO> experienceList = new List<EmpExperienceBO>();
                EmpExperienceDA experienceDA = new EmpExperienceDA();
                experienceList = experienceDA.GetEmpExperienceByEmpId(empId);

                //Career Info
                List<EmpCareerInfoBO> careerInfo = new List<EmpCareerInfoBO>();
                EmpCareerInfoDA careerDA = new EmpCareerInfoDA();
                careerInfo = careerDA.GetEmpCareerInfoForReport(empId);

                //Language Info
                List<EmpLanguageBO> languageInfo = new List<EmpLanguageBO>();
                EmpLanguageDA languageDA = new EmpLanguageDA();
                languageInfo = languageDA.GetEmpLanguageByEmpId(empId);

                var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], educationList));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[1], referenceList2));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[2], referenceList1));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[3], trainingList));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[4], employeeList));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[7], careerInfo));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[5], experienceList));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[6], languageInfo));

                rvTransaction.LocalReport.DisplayName = "Applicant Resume";

                //rvTransaction.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubReportProcessingForInvoiceDetail);
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
    }
}