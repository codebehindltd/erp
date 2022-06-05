using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class ReportEmployeeProfile : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        string empId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                ControlShowHide();
            }
        }
        private void ControlShowHide()
        {
            Boolean IsAdminUser = false;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                IsAdminUser = true;
                employeeSearchSection.Visible = true;
                EmployeeInformationDiv.Visible = true;
                button.Visible = true;
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
                        employeeSearchSection.Visible = true;
                        EmployeeInformationDiv.Visible = true;
                        button.Visible = true;
                    }
                }
            }
            #endregion

            if (!IsAdminUser)
            {
                employeeSearchSection.Visible = false;
                EmployeeInformationDiv.Visible = false;
                button.Visible = false;
                if (userInformationBO.EmpId > 0)
                {
                    empId = userInformationBO.EmpId.ToString();
                    HiddenField hfEmployeeId = (HiddenField)this.employeeeSearch1.FindControl("hfEmployeeId");
                    hfEmployeeId.Value = userInformationBO.EmpId.ToString();
                    ReportLoad(userInformationBO.EmpId.ToString());
                }
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            HiddenField hfEmployeeId = (HiddenField)this.employeeeSearch1.FindControl("hfEmployeeId");

            if (hfEmployeeId.Value != "0")
                empId = hfEmployeeId.Value;
            else
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "an employee.", AlertType.Warning);
                return;
            }
            ReportLoad(empId);
        }
        protected void ReportLoad(string empId)
        {
            _RoomStatusInfoByDate = 1;
            EmployeeDA employeeDA = new EmployeeDA();

            //Documents Info
            DocumentsDA docDA = new DocumentsDA();
            List<DocumentsBO> docImageBO = new List<DocumentsBO>();
            List<DocumentsBO> docSignatureBO = new List<DocumentsBO>();
            docImageBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("Employee Document", Convert.ToInt32(empId));
            docSignatureBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("Employee Signature", Convert.ToInt32(empId));

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptEmployeeProfile.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            //General Info
            List<EmployeeBO> employeeList = new List<EmployeeBO>();
            employeeList = employeeDA.GetEmployeeInfoForReport(Convert.ToInt32(empId));

            List<ReportParameter> reportParam = new List<ReportParameter>();
            reportParam.Add(new ReportParameter("EmpName", employeeList[0].DisplayName));
            reportParam.Add(new ReportParameter("Address", employeeList[0].PresentAddress));
            reportParam.Add(new ReportParameter("Phone", employeeList[0].PresentPhone));
            reportParam.Add(new ReportParameter("Email", employeeList[0].PersonalEmail));

            if (docImageBO.Count == 0)
            {
                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, @"/Payroll/Images/Documents/defaultempimg.png")));
            }
            else
            {
                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, docImageBO[0].Path + docImageBO[0].Name)));
            }

            if (docSignatureBO.Count == 0)
            {
                reportParam.Add(new ReportParameter("SignaturePath", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, @"/Payroll/Images/Signature/defaultsignature.jpg")));
            }
            else
            {
                reportParam.Add(new ReportParameter("SignaturePath", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, docSignatureBO[0].Path + docSignatureBO[0].Name)));
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

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollCostCenterDivHide", "IsPayrollCostCenterDivHide");
            reportParam.Add(new ReportParameter("IsPayrollCostCenterDivHide", setUpBO.SetupValue));

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollProvidentFundDeductHide", "IsPayrollProvidentFundDeductHide");
            reportParam.Add(new ReportParameter("IsPayrollProvidentFundDeductHide", setUpBO.SetupValue));

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollBenefitsHide", "IsPayrollBenefitsHide");
            reportParam.Add(new ReportParameter("IsPayrollBenefitsHide", setUpBO.SetupValue));

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollBeneficiaryHide", "IsPayrollBeneficiaryHide");
            reportParam.Add(new ReportParameter("IsPayrollBeneficiaryHide", setUpBO.SetupValue));

            rvTransaction.LocalReport.SetParameters(reportParam);

            //Reference Info
            List<EmpReferenceBO> referenceList1 = new List<EmpReferenceBO>();
            List<EmpReferenceBO> referenceList2 = new List<EmpReferenceBO>();
            List<EmpReferenceBO> totalReferenceList = new List<EmpReferenceBO>();
            EmpReferenceDA referenceDA = new EmpReferenceDA();
            referenceList1 = referenceDA.GetEmpReferenceByEmpId(Convert.ToInt32(empId));
            totalReferenceList = referenceList1;

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

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], employeeList));
            rvTransaction.LocalReport.DisplayName = "Employee Profile";
            rvTransaction.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubReportProcessingForEmployeeProfile);
            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
        private void SubReportProcessingForEmployeeProfile(object sender, SubreportProcessingEventArgs e)
        {
            HiddenField hfEmployeeId = (HiddenField)this.employeeeSearch1.FindControl("hfEmployeeId");
            int EmployeeId = 0;
            if (hfEmployeeId.Value != "0")
            {
                EmployeeId = Convert.ToInt32(hfEmployeeId.Value);
                empId = EmployeeId.ToString();

                DateTime? date = null;
                EmpEducationDA entityEducationDA = new EmpEducationDA();
                List<EmpEducationBO> empEducation = entityEducationDA.GetEmpEducationEmpId(EmployeeId);
                e.DataSources.Add(new ReportDataSource("EmployeeEducationSubReportData", empEducation));

                EmpDependentDA entityDependentDA = new EmpDependentDA();
                List<EmpDependentBO> empDependent = entityDependentDA.GetEmpDependentByEmpId(EmployeeId);
                e.DataSources.Add(new ReportDataSource("EmployeeDependent", empDependent));

                List<EmpReferenceBO> empReference = new List<EmpReferenceBO>();
                EmpReferenceDA referenceDA = new EmpReferenceDA();
                empReference = referenceDA.GetEmpReferenceByEmpId(EmployeeId);
                e.DataSources.Add(new ReportDataSource("TotalReferenceList", empReference));

                EmpGratuityDA gratuityDA = new EmpGratuityDA();
                List<EmpGratuityBO> gratuityList = new List<EmpGratuityBO>();
                gratuityList = gratuityDA.GetEmpGratuityForProfile(Convert.ToInt32(empId));
                e.DataSources.Add(new ReportDataSource("EmpGratuityList", gratuityList));
                EmpPFDA pfDA = new EmpPFDA();
                List<PFReportViewBO> pfList = new List<PFReportViewBO>();
                pfList = pfDA.GetEmpPFForProfile(Convert.ToInt32(empId));
                e.DataSources.Add(new ReportDataSource("EmpPF", pfList));

                //Experience Info
                List<EmpExperienceBO> experienceList = new List<EmpExperienceBO>();
                EmpExperienceDA experienceDA = new EmpExperienceDA();
                experienceList = experienceDA.GetEmpExperienceByEmpIdForResume(Convert.ToInt32(empId));
                e.DataSources.Add(new ReportDataSource("EmployeeExperience", experienceList));

                //Promotion Info
                List<EmpExperienceBO> promotionList = new List<EmpExperienceBO>();
                promotionList = experienceList.Where(a => a.ExperienceId == 0).ToList();
                experienceList = experienceList.Except(promotionList).ToList();
                e.DataSources.Add(new ReportDataSource("PromotionDataSet", promotionList));

                //Career Info
                List<EmpCareerInfoBO> careerInfo = new List<EmpCareerInfoBO>();
                EmpCareerInfoDA careerDA = new EmpCareerInfoDA();
                careerInfo = careerDA.GetEmpCareerInfoForReport(Convert.ToInt32(empId));

                //Appraisal Info
                List<AppraisalEvaluationReportViewBO> appraisalList = new List<AppraisalEvaluationReportViewBO>();
                AppraisalEvaluationDA appraisalDA = new AppraisalEvaluationDA();
                appraisalList = appraisalDA.GetAppraisalEvaluationForReport(null, null, empId, date, date);
                e.DataSources.Add(new ReportDataSource("EmpAppraisal", appraisalList));

                //Disciplinary Info
                List<DisciplinaryActionReportViewBO> disciplinaryList = new List<DisciplinaryActionReportViewBO>();
                DisciplinaryActionDA disciplinaryDA = new DisciplinaryActionDA();
                disciplinaryList = disciplinaryDA.GetDisciplinaryActionForReport(Convert.ToInt32(empId));
                e.DataSources.Add(new ReportDataSource("EmpDisciplinaryAction", disciplinaryList));

                //Benefit Info
                BenefitDA empBenefitDA = new BenefitDA();
                List<PayrollEmpBenefitBO> benefitList = new List<PayrollEmpBenefitBO>();
                benefitList = empBenefitDA.GetEmpBenefitListByEmpId(Convert.ToInt32(empId));
                e.DataSources.Add(new ReportDataSource("EmpBenefit", benefitList));

                //Leave Status
                List<LeaveTakenNBalanceBO> leaveStatusList = new List<LeaveTakenNBalanceBO>();
                EmployeeYearlyLeaveDA leaveDa = new EmployeeYearlyLeaveDA();
                leaveStatusList = leaveDa.GetLeaveTakenNBalanceByEmployee(Convert.ToInt32(empId), DateTime.Now);
                e.DataSources.Add(new ReportDataSource("LeaveStatus", leaveStatusList));

                List<EmpCareerTrainingBO> trainingList = new List<EmpCareerTrainingBO>();
                EmpCareerTrainingDA trainingDA = new EmpCareerTrainingDA();
                trainingList = trainingDA.GetEmpCareerTrainingByEmpIdForResume(Convert.ToInt32(empId));
                e.DataSources.Add(new ReportDataSource("TrainingDataSet", trainingList));

                //Increment Info
                EmpIncrementDA incrementDA = new EmpIncrementDA();
                List<EmpIncrementBO> incrementList = new List<EmpIncrementBO>();
                incrementList = incrementDA.GetIncrementByEmpId(Convert.ToInt32(empId));
                //incrementList = incrementList.Where(a => a.EmpId == Convert.ToInt32(empId)).ToList();
                e.DataSources.Add(new ReportDataSource("EmpIncrement", incrementList));
            }
        }
    }
}