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
    public partial class frmBestEmployeeNomination : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected int isMessageBoxEnable = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckObjectPermission();
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadDepartment();
                LoadCommonDropDownHiddenField();
                LoadAllEmployeeByDepartment();
            }
        }

        protected void CheckObjectPermission()
        {
            btnEmpApprEvaluation.Visible = isSavePermission;
        }

        protected void ProcessTypeDropDown_Change(object sender, EventArgs e)
        {
            LoadAllEmployeeByDepartment();
        }
        protected void DepartmentDropDown_Change(object sender, EventArgs e)
        {
            LoadAllEmployeeByDepartment();
        }
        protected void YearDropDown_Change(object sender, EventArgs e)
        {
            LoadAllEmployeeByDepartment();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string empGrid = string.Empty, alternateColor = string.Empty;
            short rowCounter = 0;
            int departmentId = 0;

            if (ddlSDepartment.SelectedIndex != 0)
            {
                departmentId = Convert.ToInt32(ddlSDepartment.SelectedValue);
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + " Department ", AlertType.Warning);
                this.SetTab("SearchTab");
                return;
            }

            AppraisalEvaluationDA appDa = new AppraisalEvaluationDA();
            List<BestEmployeeNominationViewBO> employeeLst = new List<BestEmployeeNominationViewBO>();

            employeeLst = appDa.GetEmployeeOfTheYearMonthNomination(departmentId, Convert.ToInt16(ddlSYear.SelectedValue), Convert.ToByte(ddlSMonth.SelectedValue), ddlSProcessType.SelectedValue);

            if (employeeLst.Count > 0)
            {

                hfBestEmpNomineeId.Value = employeeLst[0].BestEmpNomineeId.ToString();

                empGrid += "<table id='gvSearchEmployee' class='table table-bordered table-condensed table-responsive' width='100%'>" +

                                "<colgroup>" +
                                "   <col style='width: 53%;' />" +
                                "   <col style='width: 15%;' />" +
                                "   <col style='width: 25%;' />" +
                                "</colgroup>" +

                               " <thead>" +
                               "     <tr style='color: White; background-color: #44545E; font-weight: bold;'>" +
                               "         <th style='text-align: left;'>" +
                               "             Name" +
                               "         </th>" +
                               "         <th style='text-align: left;'>" +
                               "             ID" +
                               "         </th>" +
                               "         <th style='text-align: left;'>" +
                               "             Designation" +
                               "         </th>" +
                               "     </tr>" +
                               " </thead>" +
                               " <tbody>";

                foreach (BestEmployeeNominationViewBO emp in employeeLst)
                {
                    rowCounter++;
                    if (rowCounter % 2 == 0)
                    {
                        alternateColor = "style=\"background-color:#E3EAEB;\"";
                    }
                    else
                        alternateColor = "style=\"background-color:#FFFFFF;\"";

                    empGrid += "<tr " + alternateColor + ">" +
                                "<td style=\"display:none;\">" +
                                    emp.EmpId +
                                "</td>" +
                                "<td style=\"display:none;\">" +
                                    emp.BestEmpNomineeDetailsId +
                                "</td>" +
                                  "<td style=\"width: 53%;\">" +
                                    emp.EmployeeName +
                                "</td>" +
                                "<td style=\"width: 15%;\">" +
                                    emp.EmpCode +
                                "</td>" +
                                "<td style=\"width: 25%;\">" +
                                    emp.Designation +
                                "</td>" +
                            "</tr>";
                }
                empGrid += " </tbody> </table>";


                SearchInfo.InnerHtml = empGrid;
            }
            else
            {
                SearchInfo.InnerHtml = string.Empty;
            }
            this.SetTab("SearchTab");
        }

        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            ddlDepartmentId.DataSource = entityDA.GetDepartmentInfo();
            ddlDepartmentId.DataTextField = "Name";
            ddlDepartmentId.DataValueField = "DepartmentId";
            ddlDepartmentId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlDepartmentId.Items.Insert(0, item);

            ddlSDepartment.DataSource = entityDA.GetDepartmentInfo();
            ddlSDepartment.DataTextField = "Name";
            ddlSDepartment.DataValueField = "DepartmentId";
            ddlSDepartment.DataBind();

            ddlSDepartment.Items.Insert(0, item);
        }
        private void LoadAllEmployeeByDepartment()
        {
            string empGrid = string.Empty, alternateColor = string.Empty;
            short rowCounter = 0;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeBO> employeeLst = new List<EmployeeBO>();
            List<BestEmployeeNominationViewBO> employeeList = new List<BestEmployeeNominationViewBO>();
            EmployeeBO employee = new EmployeeBO();

            AppraisalEvaluationDA appDA = new AppraisalEvaluationDA();

            if (userInformationBO.EmpId > 0)
            {
                if (ddlBeastEmployeeType.SelectedValue == "Year")
                {
                    if (ddlDepartmentId.SelectedIndex != 0)
                    {
                        employeeList = appDA.GetMonthlySelectedBestEmployee(Convert.ToInt32(ddlDepartmentId.SelectedValue), Convert.ToInt16(ddlSelectionYear.SelectedValue));
                    }
                    //hfBestEmpNomineeId.Value = employeeList[0].BestEmpNomineeId.ToString();

                    empGrid += "<table cellspacing='0' cellpadding='4' id='gvEmployee' width='100%'>" +

                                "<colgroup>" +
                                "   <col style='width: 7%;' />" +
                                "   <col style='width: 53%;' />" +
                                "   <col style='width: 15%;' />" +
                                "   <col style='width: 25%;' />" +
                                "</colgroup>" +

                               " <thead>" +
                               "     <tr style='color: White; background-color: #44545E; font-weight: bold;'>" +
                               "         <th>" +
                               "             <input type='checkbox' id='checkAllEmployee' title='Select All Employee' />" +
                               "         </th>" +
                               "         <th style='text-align: left;'>" +
                               "             Name" +
                               "         </th>" +
                               "         <th style='text-align: left;'>" +
                               "             ID" +
                               "         </th>" +
                               "         <th style='text-align: left;'>" +
                               "             Designation" +
                               "         </th>" +
                               "     </tr>" +
                               " </thead>" +
                               " <tbody>";

                    foreach (BestEmployeeNominationViewBO emp in employeeList)
                    {
                        rowCounter++;
                        if (rowCounter % 2 == 0)
                        {
                            alternateColor = "style=\"background-color:#E3EAEB;\"";
                        }
                        else
                            alternateColor = "style=\"background-color:#FFFFFF;\"";

                        empGrid += "<tr " + alternateColor + ">" +
                                    "<td style=\"display:none;\">" +
                                        emp.EmpId +
                                    "</td>" +
                                    "<td style=\"display:none;\">" +
                                    emp.BestEmpNomineeId +
                                    "</td>" +
                                    "<td style=\"display:none;\">" + 
                                    emp.BestEmpNomineeDetailsId +
                                    "</td>" +
                                    "<td style = 'text-align: center; width:7%;' >" +
                                        "<input type='checkbox' id='chk" + emp.EmpId + "' />" +
                                    "</td>" +
                                      "<td style=\"width: 53%;\">" +
                                        emp.EmployeeName +
                                    "</td>" +
                                    "<td style=\"width: 15%;\">" +
                                        emp.EmpCode +
                                    "</td>" +
                                    "<td style=\"width: 25%;\">" +
                                        emp.Designation +
                                    "</td>" +
                                "</tr>";
                    }
                    empGrid += " </tbody> </table>";
                }
                else if (ddlBeastEmployeeType.SelectedValue == "Month")
                {
                    if (ddlDepartmentId.SelectedIndex != 0)
                    {
                        employeeLst = empDa.GetEmployeeByDepartment(Convert.ToInt32(ddlDepartmentId.SelectedValue));
                    }
                    else
                    {
                        employee = empDa.GetEmployeeInfoById(userInformationBO.EmpId);
                        employeeLst = empDa.GetEmployeeByDepartment(employee.DepartmentId);
                        ddlDepartmentId.SelectedValue = employee.DepartmentId.ToString();
                    }
                    empGrid += "<table id='gvEmployee' class='table table-bordered table-condensed table-responsive' width='100%'>" +

                                                    "<colgroup>" +
                                                    "   <col style='width: 7%;' />" +
                                                    "   <col style='width: 53%;' />" +
                                                    "   <col style='width: 15%;' />" +
                                                    "   <col style='width: 25%;' />" +
                                                    "</colgroup>" +

                                                   " <thead>" +
                                                   "     <tr style='color: White; background-color: #44545E; font-weight: bold;'>" +
                                                   "         <th>" +
                                                   "             <input type='checkbox' id='checkAllEmployee' title='Select All Employee' />" +
                                                   "         </th>" +
                                                   "         <th style='text-align: left;'>" +
                                                   "             Name" +
                                                   "         </th>" +
                                                   "         <th style='text-align: left;'>" +
                                                   "             ID" +
                                                   "         </th>" +
                                                   "         <th style='text-align: left;'>" +
                                                   "             Designation" +
                                                   "         </th>" +
                                                   "     </tr>" +
                                                   " </thead>" +
                                                   " <tbody>";

                    foreach (EmployeeBO emp in employeeLst)
                    {
                        rowCounter++;
                        if (rowCounter % 2 == 0)
                        {
                            alternateColor = "style=\"background-color:#E3EAEB;\"";
                        }
                        else
                            alternateColor = "style=\"background-color:#FFFFFF;\"";

                        empGrid += "<tr " + alternateColor + ">" +
                                    "<td style=\"display:none;\">" +
                                        emp.EmpId +
                                    "</td>" +
                                    "<td style=\"display:none;\">" +
                                     0 +"</td>" +
                                    "<td style=\"display:none;\">" +
                                     0 +"</td>" +
                                    "<td style = 'text-align: center; width:7%;' >" +
                                        "<input type='checkbox' id='chk" + emp.EmpId + "' />" +
                                    "</td>" +
                                      "<td style=\"width: 53%;\">" +
                                        emp.DisplayName +
                                    "</td>" +
                                    "<td style=\"width: 15%;\">" +
                                        emp.EmpCode +
                                    "</td>" +
                                    "<td style=\"width: 25%;\">" +
                                        emp.Designation +
                                    "</td>" +
                                    "<td style=\"width: 25%;\">< a href =\"javascript:void();\" onclick=\"javascript:return PerformCancelAction(\'" + emp.EmpId + "\');\"><img alt=\"Cancel\" src=\"../Images/delete.png\" title=\"Delete\" /></a>" +
                                    "</td>" +
                                "</tr>";
                    }
                    empGrid += " </tbody> </table>";

                }
            }

            DepartmentWiseEmployee.InnerHtml = empGrid;
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void SetTab(string TabName)
        {

            if (TabName == "SearchTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }

        [WebMethod]
        public static ReturnInfo SaveBestEmployeeNomination(BestEmployeeNominationBO BestEmployeeNomination, List<BestEmployeeNominationDetailsBO> BestEmployeeNominationDetails, string processType)
        {
            HMUtility hmUtility = new HMUtility();
            int bestEmpNomineeId = 0;
            ReturnInfo rtninf = new ReturnInfo();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            BestEmployeeNomination.CreatedBy = userInformationBO.UserInfoId;
            BestEmployeeNomination.ApprovedStatus = HMConstants.ApprovalStatus.Submit.ToString();
            BestEmployeeNomination.Status = "Close";

            AppraisalEvaluationDA apprEvaDA = new AppraisalEvaluationDA();
            bool status = false;
            if (processType == "Month")
            {
                status = apprEvaDA.SaveBestEmployeeNomination(BestEmployeeNomination, BestEmployeeNominationDetails, out bestEmpNomineeId);
            }
            else
            {
                status = apprEvaDA.UpdateMonthlyBestEmpForYearlyBestEmp(BestEmployeeNominationDetails);
            }

            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.BestEmpNomination.ToString(), bestEmpNomineeId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BestEmpNomination));
                //return true;
            }
            else
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }

        //[WebMethod]
        //public static ReturnInfo UpdateAppraisalEvalution(AppraisalEvalutionByBO appraisalEvalution, List<AppraisalEvalutionRatingFactorDetailsBO> appraisalEvalutionRatingList)
        //{
        //    ReturnInfo rtninf = new ReturnInfo();

        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

        //    appraisalEvalution.EvalutiorId = userInformationBO.UserInfoId;
        //    appraisalEvalution.EvaluationFromDate = DateTime.Now;
        //    appraisalEvalution.LastModifiedBy = userInformationBO.UserInfoId;

        //    AppraisalEvaluationDA apprEvaDA = new AppraisalEvaluationDA();
        //    bool status = apprEvaDA.UpdateAppraisalEvaluation(appraisalEvalution, appraisalEvalutionRatingList);
        //    if (status)
        //    {
        //        rtninf.IsSuccess = true;
        //        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
        //        //return true;
        //    }
        //    else
        //    {
        //        rtninf.IsSuccess = false;
        //        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
        //        //return false;
        //    }
        //    return rtninf;
        //}

        //[WebMethod]
        //public static GridViewDataNPaging<AppraisalEvaluationViewBO, GridPaging> SearchApprEvaluationAndLoadGridInformation(string empId, string appraisalType, string fromDate, string toDate, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        //{
        //    HMUtility hmUtility = new HMUtility();
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

        //    int totalRecords = 0;

        //    GridViewDataNPaging<AppraisalEvaluationViewBO, GridPaging> myGridData = new GridViewDataNPaging<AppraisalEvaluationViewBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
        //    pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

        //    DateTime? startDate = null;
        //    DateTime? endDate = null;
        //    if (!string.IsNullOrEmpty(fromDate))
        //    {
        //        startDate = hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat);
        //    }
        //    if (!string.IsNullOrEmpty(toDate))
        //    {
        //        endDate = hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat);
        //    }

        //    HMCommonDA commonDA = new HMCommonDA();
        //    AppraisalEvaluationDA apprEvaDA = new AppraisalEvaluationDA();
        //    List<AppraisalEvaluationViewBO> apprEvaList = new List<AppraisalEvaluationViewBO>();
        //    apprEvaList = apprEvaDA.GetApprEvaluationInfoWithPagination(empId, appraisalType, startDate, endDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

        //    List<AppraisalEvaluationViewBO> distinctItems = new List<AppraisalEvaluationViewBO>();
        //    distinctItems = apprEvaList.GroupBy(test => test.AppraisalEvalutionById).Select(group => group.First()).ToList();


        //    //myGridData.GridPagingProcessing(guestInfoList, totalRecords);
        //    myGridData.GridPagingProcessing(distinctItems, totalRecords);

        //    return myGridData;
        //}

        //[WebMethod]
        //public static AppraisalEvaluationDetailsViewBO EditAppraisalEvaluation(int apprEvaId)
        //{
        //    AppraisalEvaluationDetailsViewBO apprEvaViewBO = new AppraisalEvaluationDetailsViewBO();
        //    AppraisalEvaluationDA apprEvaDA = new AppraisalEvaluationDA();

        //    AppraisalEvalutionByBO apprEvaBO = new AppraisalEvalutionByBO();
        //    apprEvaBO = apprEvaDA.GetAppraisalEvaluationInfoById(apprEvaId);

        //    List<AppraisalEvalutionRatingFactorDetailsBO> apprEvaList = new List<AppraisalEvalutionRatingFactorDetailsBO>();
        //    //apprEvaList = apprEvaDA.GetAppraisalEvaluationDetailsInfoByApprIdId(apprEvaBO.AppraisalEvalutionById);

        //    AppraisalRatingScaleDA rtngScaleDA = new AppraisalRatingScaleDA();
        //    List<AppraisalRatingScaleBO> rtngScale = new List<AppraisalRatingScaleBO>();


        //    rtngScale = rtngScaleDA.GetAllRatingFactorScale();

        //    apprEvaViewBO.Master = apprEvaBO;
        //    apprEvaViewBO.Details = apprEvaList;
        //    apprEvaViewBO.RatingFactorScale = rtngScale;


        //    return apprEvaViewBO;
        //}

        //[WebMethod]
        //public static ReturnInfo DeleteApprEvaluationById(int sEmpId)
        //{
        //    string result = string.Empty;
        //    ReturnInfo rtninf = new ReturnInfo();
        //    try
        //    {
        //        AppraisalEvaluationDA apprEvaDA = new AppraisalEvaluationDA();
        //        Boolean status = apprEvaDA.DeleteAppraisalEvaluationById(sEmpId);
        //        if (status)
        //        {
        //            rtninf.IsSuccess = true;
        //            rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
        //        }
        //        else
        //        {
        //            rtninf.IsSuccess = false;
        //            rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
        //    }
        //    return rtninf;
        //}

        //[WebMethod]
        //public static List<EmployeeBO> LoadDepartmentalManager(int departmentId)
        //{
        //    EmployeeDA empDa = new EmployeeDA();
        //    List<EmployeeBO> boList = new List<EmployeeBO>();

        //    boList = empDa.GetEmployeeByDepartment(departmentId);

        //    return boList;
        //}

        //[WebMethod]
        //public static List<EmployeeBO> LoadDepartmentalWiseEmployee(int departmentId)
        //{
        //    EmployeeDA empDa = new EmployeeDA();
        //    List<EmployeeBO> boList = new List<EmployeeBO>();

        //    boList = empDa.GetEmployeeByDepartment(departmentId);

        //    return boList;
        //}

    }
}
