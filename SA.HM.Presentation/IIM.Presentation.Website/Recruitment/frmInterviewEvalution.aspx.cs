using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Newtonsoft.Json;

using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Recruitment
{
    public partial class frmInterviewEvalution : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                CheckObjectPermission();
                LoadUserInformation();
                LoadJobCircular();
                LoadCommonDropDownHiddenField();
                //string applicantId = Request.QueryString["applicantId"];
                //if (!string.IsNullOrWhiteSpace(applicantId))
                //{
                //    long Id = Convert.ToInt64(applicantId);
                //    if (Id > 0)
                //    {
                //        FillFormByApplicantId(Id);
                //        SetTab("circularEntry");
                //    }
                //}
            }
        }

        protected void ddlJobCircular_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadApplicantByJobCircular(Convert.ToInt64(ddlJobCircular.SelectedValue));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool status = false;

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                ApplicantResultDA appResultDA = new ApplicantResultDA();

                List<PayrollApplicantResultBO> addList = new List<PayrollApplicantResultBO>();
                addList = JsonConvert.DeserializeObject<List<PayrollApplicantResultBO>>(hfSaveObj.Value);

                List<PayrollApplicantResultBO> editList = new List<PayrollApplicantResultBO>();
                editList = JsonConvert.DeserializeObject<List<PayrollApplicantResultBO>>(hfEditObj.Value);

                List<PayrollApplicantResultBO> deleteList = new List<PayrollApplicantResultBO>();
                deleteList = JsonConvert.DeserializeObject<List<PayrollApplicantResultBO>>(hfDeleteObj.Value);

                if (hfApplicantId.Value == string.Empty)
                {                    
                    status = appResultDA.SaveApplicantResultInfo(addList, userInformationBO.UserInfoId);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        //ClearForm();
                        ClearTotalForm();
                    }
                }
                else
                {
                    status = appResultDA.UpdateApplicantResultInfo(addList, editList, deleteList, userInformationBO.UserInfoId);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        ClearForm();
                        ClearTotalForm();
                    }
                }

                if (!status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }

                SetTab("circularEntry");
            }
            catch (Exception ex)
            {
                innboardMessage.Value = JsonConvert.SerializeObject(CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error));
                
            }
        }

        //**************************** User Defined Method ****************************//

        private void LoadUserInformation()
        {
            //UserInformationDA entityDA = new UserInformationDA();

            //this.ddlCheckedBy.DataSource = entityDA.GetUserInformation();
            //this.ddlCheckedBy.DataTextField = "UserName";
            //this.ddlCheckedBy.DataValueField = "UserInfoId";
            //this.ddlCheckedBy.DataBind();

            //ListItem itemEmployee = new ListItem();
            //itemEmployee.Value = string.Empty;
            //itemEmployee.Text = hmUtility.GetDropDownFirstValue();
            //this.ddlCheckedBy.Items.Insert(0, itemEmployee);


            //this.ddlApprovedBy.DataSource = entityDA.GetUserInformation();
            //this.ddlApprovedBy.DataTextField = "UserName";
            //this.ddlApprovedBy.DataValueField = "UserInfoId";
            //this.ddlApprovedBy.DataBind();

            //this.ddlApprovedBy.Items.Insert(0, itemEmployee);
        }

        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmEmpLoan.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            //btnSearch.Visible = isSavePermission;
        }

        private void SetTab(string TabName)
        {
            if (TabName == "circularEntry")
            {
                circularEntry.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                circularSearch.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "circularSearch")
            {
                circularSearch.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                circularEntry.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }

        private void ClearForm()
        {
            //hfJobCircularId.Value = string.Empty;
            this.btnSave.Text = "Save";
        }
        private void ClearTotalForm()
        {
            ddlJobCircular.SelectedValue = "0";
            ddlApplicant.SelectedValue = "0";
            txtRemarks.Text = "";
        }

        private void LoadJobCircular()
        {
            JobCircularNRecruitmentDA entityDA = new JobCircularNRecruitmentDA();
            List<PayrollJobCircularBO> jobCircularlst = new List<PayrollJobCircularBO>();

            DateTime? crdate = null;
            jobCircularlst = entityDA.GetJobCircularByDate(crdate);

            this.ddlJobCircular.DataSource = jobCircularlst;
            this.ddlJobCircular.DataTextField = "JobTitle";
            this.ddlJobCircular.DataValueField = "JobCircularId";
            this.ddlJobCircular.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlJobCircular.Items.Insert(0, item);

            this.ddlSearchJobCircular.DataSource = jobCircularlst;
            this.ddlSearchJobCircular.DataTextField = "JobTitle";
            this.ddlSearchJobCircular.DataValueField = "JobCircularId";
            this.ddlSearchJobCircular.DataBind();

            this.ddlSearchJobCircular.Items.Insert(0, item);
        }

        private void LoadApplicantByJobCircular(Int64 jobCircularId)
        {
            JobCircularNRecruitmentDA entityDA = new JobCircularNRecruitmentDA();
            List<PayrollJobCircularApplicantMappingBO> jobCircularlst = new List<PayrollJobCircularApplicantMappingBO>();
            jobCircularlst = entityDA.GetApplicantByJobCircular(jobCircularId);

            this.ddlApplicant.DataSource = jobCircularlst;
            this.ddlApplicant.DataTextField = "EmployeeName";
            this.ddlApplicant.DataValueField = "ApplicantId";
            this.ddlApplicant.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlApplicant.Items.Insert(0, item);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        [WebMethod]
        public static List<PayrollJobCircularApplicantMappingBO> GetApplicantByJobCircular(Int64 jobCircularId)
        {
            JobCircularNRecruitmentDA entityDA = new JobCircularNRecruitmentDA();
            List<PayrollJobCircularApplicantMappingBO> jobCircularlst = new List<PayrollJobCircularApplicantMappingBO>();
            jobCircularlst = entityDA.GetApplicantByJobCircular(jobCircularId);
            return jobCircularlst;
            //this.ddlApplicant.DataSource = jobCircularlst;
            //this.ddlApplicant.DataTextField = "EmployeeName";
            //this.ddlApplicant.DataValueField = "ApplicantId";
            //this.ddlApplicant.DataBind();

            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //this.ddlApplicant.Items.Insert(0, item);
        }

        //************************ User Defined WebMethod ********************//

        [WebMethod]
        public static GridViewDataNPaging<PayrollApplicantResultBO, GridPaging> SearchApplicantInterviewResult(string jobCircular, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage, string jobTitle, string formDate, string toDate)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;
            long jobCircularId = 0;
            if (!string.IsNullOrWhiteSpace(jobCircular))
            {
                jobCircularId = Convert.ToInt64(jobCircular);
            }
            DateTime? dateFrom = null, dateTo = null;
            if (!string.IsNullOrWhiteSpace(formDate))
            {
                //dateFrom = Convert.ToDateTime(txtFromDate.Text);
                dateFrom = CommonHelper.DateTimeToMMDDYYYY(formDate);
            }

            if (!string.IsNullOrWhiteSpace(toDate))
            {
                //dateTo = Convert.ToDateTime(txtToDate.Text);
                dateTo = CommonHelper.DateTimeToMMDDYYYY(toDate);
            }

            GridViewDataNPaging<PayrollApplicantResultBO, GridPaging> myGridData = new GridViewDataNPaging<PayrollApplicantResultBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            ApplicantResultDA applicantResultDA = new ApplicantResultDA();
            List<PayrollApplicantResultBO> actionList = new List<PayrollApplicantResultBO>();
            actionList = applicantResultDA.GetInterviewResultBySearchCriteriaForPaging(jobCircularId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords, dateFrom, dateTo, jobTitle);

            List<PayrollApplicantResultBO> distinctItems = new List<PayrollApplicantResultBO>();
            distinctItems = actionList.GroupBy(test => test.ApplicantId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
       
        [WebMethod]
        public static PayrollApplicantResultBO LoadApplicantResult(long applicantId, long jobCircularId)
        {
            PayrollApplicantResultBO resultBO = new PayrollApplicantResultBO();
            ApplicantResultDA resultDA = new ApplicantResultDA();
            resultBO = resultDA.GetInterviewResultByApplicantId(applicantId, jobCircularId);
            return resultBO;
        }

        [WebMethod]
        public static string LoadInterviews(long applicantId, long jobCircularId)
        {
            string strTable = "", obtainMarks = string.Empty, applicantResultId = "0";
            int counter = 0;

            List<InterviewTypeBO> typelist = new List<InterviewTypeBO>();
            InterviewTypeDA interviewTypeDA = new InterviewTypeDA();
            typelist = interviewTypeDA.GetInterviewTypeList();

            List<PayrollApplicantResultBO> resultList = new List<PayrollApplicantResultBO>();
            ApplicantResultDA resultDA = new ApplicantResultDA();
            resultList = resultDA.GetInterviewListByApplicantId(applicantId, jobCircularId);

            strTable += "<table  id='InterviewType' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='display:none'></th><th style='display:none'></th><th align='left' scope='col' style='width: 60%;'>Interview Name</th><th align='left' scope='col' style='width: 25%;'>Total Marks</th><th align='center' scope='col' style='width: 15%;'>Obtained Marks</th></tr></thead>";
            strTable += "<tbody>";

            foreach (InterviewTypeBO intr in typelist)
            {
                var v = (from m in resultList where m.InterviewTypeId == intr.InterviewTypeId select m).FirstOrDefault();

                if (v != null)
                {
                    obtainMarks = v.MarksObtain.ToString();
                    applicantResultId = v.ApplicantResultId.ToString();
                }

                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:White;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                strTable += "<td align='left' style=\"display:none;\">" + applicantResultId + "</td>";
                strTable += "<td align='left' style=\"display:none;\">" + intr.InterviewTypeId + "</td>";
                strTable += "<td align='left' style='display:none;'>" + obtainMarks + "</td>";
                strTable += "<td align='left' style='width: 60%;'>" + intr.InterviewName + "</td>";
                strTable += "<td align='left' style='width: 25%;'>" + intr.Marks + "</td>";
                strTable += "<td align='left' style='width: 15%;'><input type='text' class='form-control' value='" + obtainMarks + "' /></td>";

                strTable += "</tr>";

                obtainMarks = string.Empty;
                applicantResultId = "0";
            }

            strTable += "</tbody> </table>";

            return strTable;
        }
    }
}