using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using System.Web.Services;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmAppraisalEvaluationBy : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int isSaveButtonEnable = -1;

        public frmAppraisalEvaluationBy() : base("AppraisalEvaluationBy")
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //GetAllMarksIndicator(2);
                LoadEmployee();
            }
        }

        private void LoadEmployee()
        {
            AppraisalEvaluationDA evaluationDa = new AppraisalEvaluationDA();
            List<EmployeeForEvalutionBO> evaluationList = new List<EmployeeForEvalutionBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            this.ddlEmployee.DataSource = evaluationDa.GetEmployeeByEvalutorId(userInformationBO.EmpId);
            this.ddlEmployee.DataTextField = "EmployeeName";
            this.ddlEmployee.DataValueField = "AppraisalEvalutionById";
            this.ddlEmployee.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlEmployee.Items.Insert(0, item);

        }

        protected void btnEmpApprEvaluationSave_Click(object sender, EventArgs e)
        {

        }

        protected void btnEmpApprEvaluationClear_Click(object sender, EventArgs e)
        {
            txtEvaltnDate.Text = string.Empty;
            btnEmpApprEvaluation.Text = "Save";
            hfApprEvaId.Value = string.Empty;

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl loanAllocation, employeeForLoanSearch;

            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            loanAllocation = (UserControl)mpContentPlaceHolder.FindControl("employeeSearch");
            employeeForLoanSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeForLoanSearch");

            ((TextBox)loanAllocation.FindControl("txtSearchEmployee")).Text = string.Empty;
            ((TextBox)loanAllocation.FindControl("txtEmployeeName")).Text = string.Empty;
            ((TextBox)employeeForLoanSearch.FindControl("txtEmployeeName")).Text = string.Empty;
            ((HiddenField)loanAllocation.FindControl("hfEmployeeId")).Value = "0";
            ((HiddenField)employeeForLoanSearch.FindControl("hfEmployeeId")).Value = "0";
        }

        private void GetAllMarksIndicator(int appraisalEvalutionById)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            EmployeeForEvalutionBO evaluationEmp = new EmployeeForEvalutionBO();
            AppraisalEvaluationDA evalutionDa = new AppraisalEvaluationDA();
            evaluationEmp = evalutionDa.GetEmployeeByEvalutorNEmpId(appraisalEvalutionById);

            if (evaluationEmp != null)
            {
                if (evaluationEmp.AppraisalEvalutionById > 0)
                {
                    ddlAppraisalType.SelectedValue = evaluationEmp.AppraisalType;
                    txtCompletionBy.Text = hmUtility.GetStringFromDateTime(evaluationEmp.EvalutionCompletionBy);
                    txtEvaltnDate.Text = hmUtility.GetStringFromDateTime(evaluationEmp.EvaluationFromDate);
                    txtEvalToDate.Text = hmUtility.GetStringFromDateTime(evaluationEmp.EvaluationToDate);

                    hfApprEvaId.Value = evaluationEmp.AppraisalEvalutionById.ToString();

                    AppraisalMarksIndDA apprMarksIndDA = new AppraisalMarksIndDA();
                    List<AppraisalMarksIndicatorBO> apprMarksIndList = new List<AppraisalMarksIndicatorBO>();

                    apprMarksIndList = apprMarksIndDA.GetAppraisalMarksIndicatorBySetupCriteria(appraisalEvalutionById);


                    AppraisalRatnFactDA apprRtngFactDA = new AppraisalRatnFactDA();
                    List<AppraisalRatingFactorBO> apprRtngFactList = new List<AppraisalRatingFactorBO>();
                    List<AppraisalRatingFactorBO> apf = new List<AppraisalRatingFactorBO>();

                    apprRtngFactList = apprRtngFactDA.GetRatingFactorBySetupCriteria(appraisalEvalutionById);

                    //AppraisalEvaluationDA appraisalEvaluationDA = new AppraisalEvaluationDA();
                    //List<AppraisalEvalutionRatingFactorDetailsBO> AppraisalEvalutionRatingFactorDetailsBOList = new List<AppraisalEvalutionRatingFactorDetailsBO>();
                    //List<AppraisalEvalutionRatingFactorDetailsBO> evolutionDetails = new List<AppraisalEvalutionRatingFactorDetailsBO>();

                    //AppraisalEvalutionRatingFactorDetailsBOList = appraisalEvaluationDA.GetAppraisalEvalutionRatingFactorDetailsList(appraisalEvalutionById);


                    string accordion = string.Empty, options = RatingFactorProcess();
                    accordion = "<div id=\"AppraisalAccordion\" style=\"width:100%;\" >";

                    decimal totalWeight = 0;
                    int accordionCounter = 1;

                    foreach (AppraisalMarksIndicatorBO apr in apprMarksIndList)
                    {
                        if (string.IsNullOrWhiteSpace(apr.Remarks))
                        {
                            accordion += "<h3 id=\"h" + accordionCounter + "\"> Indicator: " + apr.AppraisalIndicatorName + "</h3>";
                        }
                        else
                        {
                            accordion += "<h3 title = '" + apr.Remarks + "' id=\"h" + accordionCounter + "\"> Indicator: " + apr.AppraisalIndicatorName + "</h3>";
                        }
                        totalWeight = 0;

                        apf = (from ap in apprRtngFactList
                               where ap.AppraisalIndicatorId == apr.MarksIndicatorId
                               select ap).ToList();

                        //evolutionDetails = (from ap in AppraisalEvalutionRatingFactorDetailsBOList
                        //                    where ap.MarksIndicatorId == apr.MarksIndicatorId
                        //                    select ap).ToList();

                        accordion += "<div>";

                        accordion += "<span id='mi" + accordionCounter + "' style='display:none;'>" + apr.MarksIndicatorId + "</span>" +
                                     "<span id='apw" + accordionCounter + "' style='display:none;'>###totalWeight###</span>" +
                                        GridProcess(apf, options, accordionCounter, out totalWeight) +
                                    "</div>";

                        accordion = accordion.Replace("###totalWeight###", totalWeight.ToString());

                        accordionCounter++;
                    }
                    accordion += "</div>";
                    appraisalEvalutionConatainer.InnerHtml = accordion;
                }
            }            
        }

        [WebMethod]
        public static List<AppraisalEvalutionRatingFactorDetailsBO> GetAppraisalEvalutionRatingFactorDetailsByAppraisalEvalutionById(int appraisalEvalutionById)
        {
            AppraisalEvaluationDA appraisalEvaluationDA = new AppraisalEvaluationDA();
            List<AppraisalEvalutionRatingFactorDetailsBO> AppraisalEvalutionRatingFactorDetailsBOList = new List<AppraisalEvalutionRatingFactorDetailsBO>();
            List<AppraisalEvalutionRatingFactorDetailsBO> evolutionDetails = new List<AppraisalEvalutionRatingFactorDetailsBO>();

            AppraisalEvalutionRatingFactorDetailsBOList = appraisalEvaluationDA.GetAppraisalEvalutionRatingFactorDetailsList(appraisalEvalutionById);

            return AppraisalEvalutionRatingFactorDetailsBOList;
        }

        private string GridProcess(List<AppraisalRatingFactorBO> apf, string options, int accordionCounter, out decimal totalWeight)
        {
            int rowCounter = 0;
            string grid = string.Empty, alternateColor = string.Empty;
            totalWeight = 0;

            grid = "<table id=\"" + accordionCounter + "\" style=\"width: 100%;\" class=\"table table-bordered table-condensed table-responsive\">" +
                        "<thead>" +
                            "<tr style=\"color: White; background-color: #44545E; font-weight: bold;\">" +
                                "<th style=\"width: 50%;\">" +
                                    "Rating Factors" +
                                "</th>" +
                                "<th style=\"width: 5%;\">" +
                                    "Weight" +
                                "</th>" +
                                "<th style=\"width: 20%;\">" +
                                    "Ratings" +
                                "</th>" +
                                "<th style=\"width: 25%;\">" +
                                    "Remarks" +
                                "</th>" +
                            "</tr>" +
                        "</thead>" +
                        "<tbody>";
            foreach (AppraisalRatingFactorBO ap in apf)
            {
                totalWeight += ap.RatingWeight;
                rowCounter++;
                if (rowCounter % 2 == 0)
                {
                    alternateColor = "style=\"background-color:#E3EAEB;\"";
                }
                else
                    alternateColor = "style=\"background-color:#FFFFFF;\"";

                grid += "<tr " + alternateColor + ">" +
                            "<td style=\"display:none;\">" +
                                ap.RatingFactorId +
                            "</td>" +
                            "<td style=\"display:none;\">" +
                                ap.RatingFacotrDetailsId +
                            "</td>" +
                            "<td style=\"width: 50%;\">" +
                                ap.RatingFactorName +
                            "</td>" +
                            "<td style=\"width: 5%; text-align: center;\">" +
                                ap.RatingWeight +
                            "</td>" +
                            "<td style=\"width: 20%;\">" +
                                options +
                            "</td>" +
                            "<td style=\"width: 25%;\">" +
                                "<input type='text' class='form-control'>" +
                            "</td>" +
                        "</tr>";
            }
            grid += " </tbody> </table>";

            return grid;
        }
        private string RatingFactorProcess()
        {
            AppraisalRatingScaleDA rtngScaleDA = new AppraisalRatingScaleDA();
            List<AppraisalRatingScaleBO> rtngScale = new List<AppraisalRatingScaleBO>();

            rtngScale = rtngScaleDA.GetAllRatingFactorScale();

            string options = string.Empty;

            options = "<select class='form-control' id=\"\">";

            foreach (AppraisalRatingScaleBO rs in rtngScale)
            {
                options += "<option value=\"" + rs.RatingValue + "," + (rs.IsRemarksMandatory ? 1 : 0) + "\">" + rs.RatingScaleNameWithRatingScale + "</option>";
            }

            options += " </select>";

            return options;

        }
        private bool IsEvaluationFrmValid()
        {
            bool flag = true;
            return flag;
        }

        [WebMethod]
        public static ReturnInfo SaveAppraisalEvalution(AppraisalEvalutionByBO appraisalEvalution, List<AppraisalEvalutionRatingFactorDetailsBO> appraisalEvalutionRatingList)
        {
            int evaluationId = 0;
            ReturnInfo rtninf = new ReturnInfo();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            appraisalEvalution.EvalutiorId = userInformationBO.UserInfoId;
            appraisalEvalution.CreatedBy = userInformationBO.UserInfoId;

            AppraisalEvaluationDA apprEvaDA = new AppraisalEvaluationDA();
            bool status = apprEvaDA.SaveAppraisalEvaluation(appraisalEvalution, appraisalEvalutionRatingList, out evaluationId);
            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                      EntityTypeEnum.EntityType.AppraisalEvaluation.ToString(), evaluationId,
                      ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                      hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.AppraisalEvaluation));
            }
            else
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }

        [WebMethod]
        public static ReturnInfo UpdateAppraisalEvalution(AppraisalEvalutionByBO appraisalEvalution, List<AppraisalEvalutionRatingFactorDetailsBO> appraisalEvalutionRatingList)
        {
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            if (!string.IsNullOrEmpty(appraisalEvalution.ApprovalStatus))
                appraisalEvalution.ApprovalStatus = HMConstants.ApprovalStatus.Approved.ToString();
            else
                appraisalEvalution.ApprovalStatus = HMConstants.ApprovalStatus.Pending.ToString();

            appraisalEvalution.LastModifiedBy = userInformationBO.UserInfoId;

            AppraisalEvaluationDA apprEvaDA = new AppraisalEvaluationDA();
            bool status = apprEvaDA.UpdateAppraisalEvaluationByEvaloator(appraisalEvalution, appraisalEvalutionRatingList);
            if (status)
            {
                rtninf.IsSuccess = true;
                if(appraisalEvalution.ApprovalStatus == "Approved")
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                      EntityTypeEnum.EntityType.AppraisalEvaluation.ToString(), appraisalEvalution.AppraisalEvalutionById,
                      ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                      hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.AppraisalEvaluation));
            }
            else
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;
        }

        [WebMethod]
        public static GridViewDataNPaging<AppraisalEvaluationViewBO, GridPaging> SearchApprEvaluationAndLoadGridInformation(string empId, string appraisalType, string fromDate, string toDate, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            
            GridViewDataNPaging<AppraisalEvaluationViewBO, GridPaging> myGridData = new GridViewDataNPaging<AppraisalEvaluationViewBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            DateTime? startDate = null;
            DateTime? endDate = null;
            if (!string.IsNullOrEmpty(fromDate))
            {
                startDate = hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                endDate = hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat);
            }

            HMCommonDA commonDA = new HMCommonDA();
            AppraisalEvaluationDA apprEvaDA = new AppraisalEvaluationDA();
            List<AppraisalEvaluationViewBO> apprEvaList = new List<AppraisalEvaluationViewBO>();

            List<AppraisalEvaluationViewBO> distinctItems = new List<AppraisalEvaluationViewBO>();
            distinctItems = apprEvaList.GroupBy(test => test.AppraisalEvalutionById).Select(group => group.First()).ToList();
            myGridData.GridPagingProcessing(distinctItems, totalRecords);
            return myGridData;
        }

        [WebMethod]
        public static AppraisalEvaluationDetailsViewBO EditAppraisalEvaluation(int apprEvaId, int empId)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            AppraisalEvaluationDetailsViewBO apprEvaViewBO = new AppraisalEvaluationDetailsViewBO();
            AppraisalEvaluationDA apprEvaDA = new AppraisalEvaluationDA();

            AppraisalEvalutionByBO apprEvaBO = new AppraisalEvalutionByBO();
            apprEvaBO = apprEvaDA.GetAppraisalEvaluationInfoById(apprEvaId);

            List<AppraisalEvalutionRatingFactorDetailsBO> apprEvaList = new List<AppraisalEvalutionRatingFactorDetailsBO>();
            apprEvaList = apprEvaDA.GetAppraisalEvaluationDetailsInfoByApprIdId(apprEvaId, empId, userInformationBO.EmpId);

            AppraisalRatingScaleDA rtngScaleDA = new AppraisalRatingScaleDA();
            List<AppraisalRatingScaleBO> rtngScale = new List<AppraisalRatingScaleBO>();

            rtngScale = rtngScaleDA.GetAllRatingFactorScale();

            apprEvaViewBO.Master = apprEvaBO;
            apprEvaViewBO.Details = apprEvaList;
            apprEvaViewBO.RatingFactorScale = rtngScale;
            return apprEvaViewBO;
        }

        [WebMethod]
        public static ReturnInfo DeleteApprEvaluationById(int sEmpId)
        {
            string result = string.Empty;
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                AppraisalEvaluationDA apprEvaDA = new AppraisalEvaluationDA();
                Boolean status = apprEvaDA.DeleteAppraisalEvaluationById(sEmpId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                      EntityTypeEnum.EntityType.AppraisalEvaluation.ToString(), sEmpId,
                      ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                      hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.AppraisalEvaluation));
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;
        }

        protected void btnEmpApprEvaluation_Click(object sender, EventArgs e)
        {

        }

        protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            isSaveButtonEnable = 1;
            GetAllMarksIndicator(Convert.ToInt32(ddlEmployee.SelectedValue));
        }
    }
}
