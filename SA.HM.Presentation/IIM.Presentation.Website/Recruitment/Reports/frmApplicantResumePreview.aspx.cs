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

namespace HotelManagement.Presentation.Website.Recruitment
{
    public partial class frmApplicantResumePreview : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();        
        protected int _RoomStatusInfoByDate = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string queryStringId = Request.QueryString["EmpId"];
                if (!string.IsNullOrEmpty(queryStringId))
                {
                    EmployeeDA employeeDA = new EmployeeDA();
                    EmployeeBO employeeBO = new EmployeeBO();

                    employeeBO = employeeDA.GetEmployeeInfoById(Convert.ToInt32(queryStringId));
                    if (employeeBO != null)
                    {
                        if (employeeBO.EmpId > 0)
                        {
                            if (employeeBO.IsApplicant)
                            {
                                this.GenerateApplicantInformation(Convert.ToInt32(queryStringId));
                            }
                            else
                            {
                                this.GenerateEmployeeInformation(Convert.ToInt32(queryStringId));
                            }
                        }
                    }
                    
                }               
            }
        }
        private void GenerateApplicantInformation(int empId)
        {
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
        private void GenerateEmployeeInformation(int empId)
        {
            _RoomStatusInfoByDate = 1;

            EmployeeDA employeeDA = new EmployeeDA();
            DateTime? date = null;
            //Documents Info
            DocumentsDA docDA = new DocumentsDA();
            List<DocumentsBO> docBO = new List<DocumentsBO>();
            docBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("Employee Document", empId);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmployeeProfile.rdlc");

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

            if (docBO.Count == 0)
            {
                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, @"/Payroll/Images/Documents/defaultempimg.png")));
            }
            else
            {
                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, docBO[0].Path + docBO[0].Name)));
            }

            rvTransaction.LocalReport.SetParameters(reportParam);


            //Training Info
            List<EmpCareerTrainingBO> trainingList = new List<EmpCareerTrainingBO>();
            EmpCareerTrainingDA trainingDA = new EmpCareerTrainingDA();
            trainingList = trainingDA.GetEmpCareerTrainingByEmpIdForResume(empId);

            //Reference Info
            List<EmpReferenceBO> referenceList1 = new List<EmpReferenceBO>();
            List<EmpReferenceBO> referenceList2 = new List<EmpReferenceBO>();
            List<EmpReferenceBO> totalRefrence = new List<EmpReferenceBO>();
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
            var totalRef = referenceList1.Union(referenceList2);
            //Education Info
            List<EmpEducationBO> educationList = new List<EmpEducationBO>();
            EmpEducationDA educationDA = new EmpEducationDA();
            educationList = educationDA.GetEmpEducationEmpId(empId);

            //Experience Info
            List<EmpExperienceBO> experienceList = new List<EmpExperienceBO>();
            EmpExperienceDA experienceDA = new EmpExperienceDA();
            experienceList = experienceDA.GetEmpExperienceByEmpIdForResume(empId);

            //Career Info
            List<EmpCareerInfoBO> careerInfo = new List<EmpCareerInfoBO>();
            EmpCareerInfoDA careerDA = new EmpCareerInfoDA();
            careerInfo = careerDA.GetEmpCareerInfoForReport(empId);

            //Appraisal Info
            List<AppraisalEvaluationReportViewBO> appraisalList = new List<AppraisalEvaluationReportViewBO>();
            AppraisalEvaluationDA appraisalDA = new AppraisalEvaluationDA();
            appraisalList = appraisalDA.GetAppraisalEvaluationForReport(null, null, empId.ToString(), date, date);

            //Disciplinary Info
            List<DisciplinaryActionReportViewBO> disciplinaryList = new List<DisciplinaryActionReportViewBO>();
            DisciplinaryActionDA disciplinaryDA = new DisciplinaryActionDA();
            disciplinaryList = disciplinaryDA.GetDisciplinaryActionForReport(empId);

            //Promotion Info
            List<EmpExperienceBO> promotionList = new List<EmpExperienceBO>();
            promotionList = experienceList.Where(a => a.ExperienceId == 0).ToList();
            experienceList = experienceList.Except(promotionList).ToList();

            //Benefit Info
            BenefitDA empBenefitDA = new BenefitDA();
            List<PayrollEmpBenefitBO> benefitList = new List<PayrollEmpBenefitBO>();
            benefitList = empBenefitDA.GetEmpBenefitListByEmpId(empId);

            //Leave Status
            LeaveInformationDA leaveDA = new LeaveInformationDA();
            List<LeaveInformationBO> leaveStatusList = new List<LeaveInformationBO>();
            leaveStatusList = leaveDA.GetEmpLeaveSummary(empId);

            //Increment Info
            EmpIncrementDA incrementDA = new EmpIncrementDA();
            List<EmpIncrementBO> incrementList = new List<EmpIncrementBO>();
            incrementList = incrementDA.GetAllIncrement();
            incrementList = incrementList.Where(a => a.EmpId == empId).ToList();

            //PF Info
            EmpPFDA pfDA = new EmpPFDA();
            List<PFReportViewBO> pfList = new List<PFReportViewBO>();
            pfList = pfDA.GetEmpPFForProfile(empId);

            //Gratuity Info
            EmpGratuityDA gratuityDA = new EmpGratuityDA();
            List<EmpGratuityBO> gratuityList = new List<EmpGratuityBO>();
            gratuityList = gratuityDA.GetEmpGratuityForProfile(empId);

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[4], trainingList));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], educationList));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[1], experienceList));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[5], careerInfo));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[6], employeeList));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[2], referenceList2));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[3], referenceList1));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[7], appraisalList));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[8], disciplinaryList));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[9], promotionList));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[10], benefitList));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[11], leaveStatusList));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[12], incrementList));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[13], pfList));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[14], gratuityList));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[15], totalRef));

            rvTransaction.LocalReport.DisplayName = "Employee Profile";

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