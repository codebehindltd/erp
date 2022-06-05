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
    public partial class frmBestEmployeeSelection : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected int isMessageBoxEnable = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            checkObjectPermission();
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadDepartment();
                LoadCommonDropDownHiddenField();
            }
        }

        protected void checkObjectPermission()
        {
            btnEmpApprEvaluation.Visible = isSavePermission;
        }

        protected void ddlDepartmentId_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAllEmployeeByDepartment(Convert.ToInt32(ddlDepartmentId.SelectedValue));
        }
        protected void ddlSelectionYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAllEmployeeByDepartment(Convert.ToInt32(ddlDepartmentId.SelectedValue));
        }
        protected void ddlSelectionMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAllEmployeeByDepartment(Convert.ToInt32(ddlDepartmentId.SelectedValue));
        }
        protected void ProcessTypeDropDown_Change(object sender, EventArgs e)
        {
            LoadAllEmployeeByDepartment(Convert.ToInt32(ddlDepartmentId.SelectedValue));
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

            employeeLst = appDa.GetEmployeeOfTheYearMonth(departmentId, Convert.ToInt16(ddlSYear.SelectedValue), Convert.ToByte(ddlSMonth.SelectedValue), ddlSProcessType.SelectedValue);

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
        private void LoadAllEmployeeByDepartment(int departmentId)
        {
            string empGrid = string.Empty, alternateColor = string.Empty;
            short rowCounter = 0;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            AppraisalEvaluationDA appDa = new AppraisalEvaluationDA();
            List<BestEmployeeNominationViewBO> employeeLst = new List<BestEmployeeNominationViewBO>();

            if (ddlBeastEmployeeType.SelectedValue == "Year")
            {
                employeeLst = appDa.GetYearlyBestEmployeeNomination(departmentId, Convert.ToInt16(ddlSelectionYear.SelectedValue), HMConstants.ApprovalStatus.Approved.ToString(), "Close");
            }
            else if (ddlBeastEmployeeType.SelectedValue == "Month")
            {
                employeeLst = appDa.GetMonthlyBestEmployeeNomination(departmentId, Convert.ToInt16(ddlSelectionYear.SelectedValue), Convert.ToByte(ddlSelectionMonth.SelectedValue), HMConstants.ApprovalStatus.Approved.ToString(), "Close");
            }
            if (employeeLst.Count > 0)
            {

                hfBestEmpNomineeId.Value = employeeLst[0].BestEmpNomineeId.ToString();

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


                DepartmentWiseEmployee.InnerHtml = empGrid;
            }
            else
            {
                DepartmentWiseEmployee.InnerHtml = string.Empty;
            }
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

        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }

        //[WebMethod]
        //public static ReturnInfo SaveBestEmployeeNomination(BestEmployeeNominationBO BestEmployeeNomination, List<BestEmployeeNominationDetailsBO> BestEmployeeNominationDetails)
        //{
        //    int bestEmpNomineeId = 0;
        //    ReturnInfo rtninf = new ReturnInfo();
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

        //    BestEmployeeNomination.CreatedBy = userInformationBO.UserInfoId;

        //    AppraisalEvaluationDA apprEvaDA = new AppraisalEvaluationDA();
        //    bool status = apprEvaDA.SaveBestEmployeeNomination(BestEmployeeNomination, BestEmployeeNominationDetails, out bestEmpNomineeId);

        //    if (status)
        //    {
        //        rtninf.IsSuccess = true;
        //        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
        //        //return true;
        //    }
        //    else
        //    {
        //        rtninf.IsSuccess = false;
        //        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
        //    }

        //    return rtninf;
        //}

        [WebMethod]
        public static ReturnInfo UpdateBestEmployeeSelection(BestEmployeeNominationBO BestEmployeeNomination, List<BestEmployeeNominationDetailsBO> BestEmployeeNominationDetails, string processType)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            BestEmployeeNomination.CreatedBy = userInformationBO.UserInfoId;

            AppraisalEvaluationDA apprEvaDA = new AppraisalEvaluationDA();
            bool status = apprEvaDA.UpdateBestEmployeeSelection(BestEmployeeNomination, BestEmployeeNominationDetails, processType);

            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                    EntityTypeEnum.EntityType.BestEmpSelection.ToString(), BestEmployeeNomination.BestEmpNomineeId, 
                    ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                    hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BestEmpSelection));
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
