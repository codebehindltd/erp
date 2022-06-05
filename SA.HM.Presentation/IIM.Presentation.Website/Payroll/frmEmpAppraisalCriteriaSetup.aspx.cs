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
    public partial class frmEmpAppraisalCriteriaSetup : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            hfMinApprisalDate.Value = hmUtility.GetStringFromDateTime(DateTime.Now.Date);
            if (!IsPostBack)
            {
                LoadEmployeeInfo();
                GetAllMarksIndicator();
                LoadDepartment();
                LoadCommonDropDownHiddenField();
            }
            CheckObjectPermission();
        }
        private void LoadEmployeeInfo()
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();

            empList = empDa.GetEmployeeInfo();
            ddlEvaluatorName.DataSource = empList;
            ddlEvaluatorName.DataTextField = "DisplayName";
            ddlEvaluatorName.DataValueField = "EmpId";
            ddlEvaluatorName.DataBind();

            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            ddlEvaluatorName.Items.Insert(0, FirstItem);
        }
        protected void CheckObjectPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }

        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            this.ddlDepartmentId.DataSource = entityDA.GetDepartmentInfo();
            this.ddlDepartmentId.DataTextField = "Name";
            this.ddlDepartmentId.DataValueField = "DepartmentId";
            this.ddlDepartmentId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlDepartmentId.Items.Insert(0, item);
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
            //approvedUser = (UserControl)mpContentPlaceHolder.FindControl("approvedByEmployee");
            employeeForLoanSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeForLoanSearch");

            ((TextBox)loanAllocation.FindControl("txtSearchEmployee")).Text = string.Empty;
            ((TextBox)loanAllocation.FindControl("txtEmployeeName")).Text = string.Empty;

            // ((TextBox)approvedUser.FindControl("txtSearchEmployee")).Text = string.Empty;
            // ((TextBox)approvedUser.FindControl("txtEmployeeName")).Text = string.Empty;
            ((TextBox)employeeForLoanSearch.FindControl("txtEmployeeName")).Text = string.Empty;

            ((HiddenField)loanAllocation.FindControl("hfEmployeeId")).Value = "0";
            // ((HiddenField)approvedUser.FindControl("hfEmployeeId")).Value = "0";
            ((HiddenField)employeeForLoanSearch.FindControl("hfEmployeeId")).Value = "0";
        }

        private void GetAllMarksIndicator()
        {
            AppraisalMarksIndDA apprMarksIndDA = new AppraisalMarksIndDA();
            List<AppraisalMarksIndicatorBO> apprMarksIndList = new List<AppraisalMarksIndicatorBO>();

            apprMarksIndList = apprMarksIndDA.GetAllAppraisalMarksIndInfo();

            AppraisalRatnFactDA apprRtngFactDA = new AppraisalRatnFactDA();
            List<AppraisalRatingFactorBO> apprRtngFactList = new List<AppraisalRatingFactorBO>();
            List<AppraisalRatingFactorBO> apf = new List<AppraisalRatingFactorBO>();

            apprRtngFactList = apprRtngFactDA.GetAllRatingFactorInfo();

            string accordion = string.Empty, options = RatingFactorProcess();
            accordion = "<div id=\"AppraisalAccordion\" style=\"width:100%;\" >";

            int accordionCounter = 1;
            //int accordionChildCounter = 1;

            foreach (AppraisalMarksIndicatorBO apr in apprMarksIndList)
            {
                //accordion += "<h3 id=\"h" + accordionCounter + "\"> Indicator: " + apr.AppraisalIndicatorName + "</h3>";
                if (string.IsNullOrWhiteSpace(apr.Remarks))
                {
                    accordion += "<h3 id=\"h" + accordionCounter + "\"> Indicator: " + apr.AppraisalIndicatorName + "</h3>";
                }
                else
                {
                    accordion += "<h3 title = '" + apr.Remarks + "' id=\"h" + accordionCounter + "\"> Indicator: " + apr.AppraisalIndicatorName + "</h3>";
                }

                apf = (from ap in apprRtngFactList
                       where ap.AppraisalIndicatorId == apr.MarksIndicatorId
                       select ap).ToList();

                accordion += "<div>";
                //accordion += "<span id='mi" + apr.AppraisalIndicatorName.Trim().Replace(" ", string.Empty) + "' style='display:none;'>" + apr.MarksIndicatorId + "</span>" +
                //             "<span id='apw" + apr.AppraisalIndicatorName.Trim().Replace(" ", string.Empty) + "' style='display:none;'>" + apr.AppraisalWeight.ToString() + "</span>" +
                //             GridProcess(apf, options, apr.AppraisalIndicatorName) +
                //        "</div>";

                accordion += "<span id='mi" + accordionCounter + "' style='display:none;'>" + apr.MarksIndicatorId + "</span>" +
                             "<span id='apw" + accordionCounter + "' style='display:none;'>" + apr.AppraisalWeight.ToString() + "</span>" +
                             GridProcess(apf, options, accordionCounter) +
                        "</div>";

                accordionCounter++;
            }
            accordion += "</div>";
            appraisalEvalutionConatainer.InnerHtml = accordion;
        }
        private string GridProcess(List<AppraisalRatingFactorBO> apf, string options, int accordionCounter)
        {
            int rowCounter = 0;
            string grid = string.Empty, alternateColor = string.Empty;

            grid = "<table id=\"" + accordionCounter + "\" style=\"width: 100%;\" cellspacing=\"0\" cellpadding=\"4\">" +
                        "<thead>" +
                            "<tr style=\"color: White; background-color: #44545E; font-weight: bold;\">" +
                                "<th style = 'text-align: center; width:10%;' > <input type='checkbox' class='checkAllRatingFactor' id='checkAll" + accordionCounter + "'/> </th>" +
                                "<th style=\"width: 70%; text-align: left;\">" +
                                    "Rating Factors" +
                                "</th>" +
                                "<th style=\"width: 20%;\">" +
                                    "Weight" +
                                "</th>" +
                            "</tr>" +
                        "</thead>" +
                        "<tbody>";
            foreach (AppraisalRatingFactorBO ap in apf)
            {
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
                            "</td>" +
                            "<td style = 'text-align: center; width:10%;' >" +
                                "<input type='checkbox' id='chk" + ap.RatingFactorId + "' />" +
                            "</td>" +
                            "<td style=\"width: 70%;\">" +
                                ap.RatingFactorName +
                            "</td>" +
                            "<td style=\"width: 10%; text-align: center;\">" +
                                ap.RatingWeight +
                            "</td>" +
                            "<td style=\"display:none;\">" +
                                ap.RatingFacotrDetailsId +
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

            options = "<select id=\"\">";

            foreach (AppraisalRatingScaleBO rs in rtngScale)
            {
                options += "<option value=\"" + rs.RatingValue + "," + (rs.IsRemarksMandatory ? 1 : 0) + "\">" + rs.RatingScaleName + "</option>";
            }

            options += " </select>";

            return options;

        }
        private bool IsEvaluationFrmValid()
        {
            bool flag = true;
            //if()
            //{

            //}

            return flag;
        }

        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }

        [WebMethod]
        public static ReturnInfo SaveAppraisalEvalutionCriteria(AppraisalEvalutionByBO AppraisalEvalutionBy, List<string> EmpLst, List<AppraisalEvalutionRatingFactorDetailsBO> AppraisalEvalutionCriteria)
        {
            int evaluationId = 0;
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            AppraisalEvalutionBy.CreatedBy = userInformationBO.UserInfoId;
            AppraisalEvalutionBy.ApprovalStatus = HMConstants.ApprovalStatus.Submit.ToString();

            AppraisalEvaluationDA apprEvaDA = new AppraisalEvaluationDA();
            bool status = apprEvaDA.SaveAppraisalEvaluationCriteria(AppraisalEvalutionBy, EmpLst, AppraisalEvalutionCriteria, out evaluationId);

            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.AppraisalEvaluation.ToString(), evaluationId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.AppraisalEvaluation));
                //return true;
            }
            else
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }

        [WebMethod]
        public static ReturnInfo UpdateAppraisalEvalution(AppraisalEvalutionByBO appraisalEvalution, List<AppraisalEvalutionRatingFactorDetailsBO> newAppraisalEvalutionRatingList, List<AppraisalEvalutionRatingFactorDetailsBO> editAppraisalEvalutionRatingList, List<AppraisalEvalutionRatingFactorDetailsBO> deleteAppraisalEvalutionRatingList)
        {
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            appraisalEvalution.EvalutiorId = userInformationBO.UserInfoId;
            appraisalEvalution.EvaluationFromDate = DateTime.Now;
            appraisalEvalution.LastModifiedBy = userInformationBO.UserInfoId;

            AppraisalEvaluationDA apprEvaDA = new AppraisalEvaluationDA();
            bool status = apprEvaDA.UpdateAppraisalEvaluation(appraisalEvalution, newAppraisalEvalutionRatingList, editAppraisalEvalutionRatingList, deleteAppraisalEvalutionRatingList);
            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpAllowDeduct.ToString(), appraisalEvalution.AppraisalEvalutionById,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAllowDeduct));
                //return true;
            }
            else
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                //return false;
            }
            return rtninf;
        }

        [WebMethod]
        public static GridViewDataNPaging<AppraisalEvaluationViewBO, GridPaging> SearchApprEvaluationAndLoadGridInformation(string empId, string appraisalType, DateTime? fromDate, DateTime? toDate, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<AppraisalEvaluationViewBO, GridPaging> myGridData = new GridViewDataNPaging<AppraisalEvaluationViewBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            //DateTime? startDate = null;
            //DateTime? endDate = null;
            //if (!string.IsNullOrEmpty(fromDate))
            //{
            //    startDate = hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat);
            //}
            //if (!string.IsNullOrEmpty(toDate))
            //{
            //    endDate = hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat);
            //}

            HMCommonDA commonDA = new HMCommonDA();
            AppraisalEvaluationDA apprEvaDA = new AppraisalEvaluationDA();
            List<AppraisalEvaluationViewBO> apprEvaList = new List<AppraisalEvaluationViewBO>();
            apprEvaList = apprEvaDA.GetApprEvaluationInfoWithPagination(empId, appraisalType, fromDate, toDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<AppraisalEvaluationViewBO> distinctItems = new List<AppraisalEvaluationViewBO>();
            distinctItems = apprEvaList.GroupBy(test => test.AppraisalEvalutionById).Select(group => group.First()).ToList();


            //myGridData.GridPagingProcessing(guestInfoList, totalRecords);
            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static AppraisalEvaluationDetailsViewBO EditAppraisalEvaluation(int apprEvaId)
        {
            AppraisalEvaluationDetailsViewBO apprEvaViewBO = new AppraisalEvaluationDetailsViewBO();
            AppraisalEvaluationDA apprEvaDA = new AppraisalEvaluationDA();

            AppraisalEvalutionByBO apprEvaBO = new AppraisalEvalutionByBO();
            apprEvaBO = apprEvaDA.GetAppraisalEvaluationInfoById(apprEvaId);

            List<AppraisalEvalutionRatingFactorDetailsBO> apprEvaList = new List<AppraisalEvalutionRatingFactorDetailsBO>();
            apprEvaList = apprEvaDA.GetAppraisalEvaluationDetailsInfoByApprIdId(apprEvaBO.AppraisalEvalutionById, apprEvaBO.EmpId, apprEvaBO.EvalutiorId);

            //AppraisalRatingScaleDA rtngScaleDA = new AppraisalRatingScaleDA();
            //List<AppraisalRatingScaleBO> rtngScale = new List<AppraisalRatingScaleBO>();


            //rtngScale = rtngScaleDA.GetAllRatingFactorScale();

            apprEvaViewBO.Master = apprEvaBO;
            apprEvaViewBO.Details = apprEvaList;
            //apprEvaViewBO.Master.EvaluationFromDate != null txtEvaltnDate.Text=CommonHelper.DateTimeClientFormatWiseConversionForSaveUpdate(apprEvaViewBO.Master.EvaluationFromDate);
            //apprEvaViewBO.Master.EvaluationToDate = CommonHelper.DateTimeClientFormatWiseConversionForSaveUpdate(apprEvaViewBO.Master.EvaluationToDate);
            //apprEvaViewBO.RatingFactorScale = rtngScale;


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
                            EntityTypeEnum.EntityType.EmpAllowDeduct.ToString(), sEmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpAllowDeduct));
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

        [WebMethod]
        public static List<EmployeeBO> LoadDepartmentalManager(int departmentId)
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeBO> boList = new List<EmployeeBO>();

            boList = empDa.GetEmployeeByDepartment(departmentId);

            return boList;
        }

        [WebMethod]
        public static List<EmployeeBO> LoadDepartmentalWiseEmployee(int departmentId)
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeBO> boList = new List<EmployeeBO>();

            boList = empDa.GetEmployeeByDepartment(departmentId);

            return boList;
        }

        protected void btnEmpApprEvaluation_Click(object sender, EventArgs e)
        {

        }
    }
}
