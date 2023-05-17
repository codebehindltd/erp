using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpRoster : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isEnableEmpRosterGrid = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadEmpRoster();
                this.LoadEmployee();
            }
        }
        protected void ddlEmpId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GenerateRosterGridView();
        }
        protected void btnSrcEmployees_Click(object sender, EventArgs e)
        {
            this.SrcButtonClick();
        }
        protected void gvEmpRoster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                TimeSlabHeadDA entityDA = new TimeSlabHeadDA();
                TimeSlabHeadBO entityBO = new TimeSlabHeadBO();
                List<TimeSlabHeadBO> entityBOList = new List<TimeSlabHeadBO>();
                entityBOList = entityDA.GetAllTimeSlabHeadInfo();

                DropDownList list = (DropDownList)e.Row.FindControl("ItemDropDown");

                list.DataSource = entityBOList;
                list.DataTextField = "TimeSlabHead";
                list.DataValueField = "TimeSlabId";
                list.DataBind();

                ListItem itemNodeId = new ListItem();
                itemNodeId.Value = "0";
                itemNodeId.Text = "Day Off";
                list.Items.Insert(0, itemNodeId);

                DropDownList secondTimeSlabIdList = (DropDownList)e.Row.FindControl("SecondTimeSlabId");

                secondTimeSlabIdList.DataSource = entityBOList;
                secondTimeSlabIdList.DataTextField = "TimeSlabHead";
                secondTimeSlabIdList.DataValueField = "TimeSlabId";
                secondTimeSlabIdList.DataBind();

                ListItem secondItemNodeId = new ListItem();
                secondItemNodeId.Value = "0";
                secondItemNodeId.Text = "Day Off";
                secondTimeSlabIdList.Items.Insert(0, secondItemNodeId);

                EmpRosterBO empRoster = e.Row.DataItem as EmpRosterBO;

                EmpRosterBO empRosterBO = new EmpRosterDA().GetEmpRosterInfoById(Convert.ToInt32(this.ddlRosterId.SelectedValue), Convert.ToInt32(this.ddlEmpId.SelectedValue), empRoster.RosterDate);

                if (empRosterBO != null)
                {
                    list.SelectedValue = empRosterBO.TimeSlabId.ToString();
                    secondTimeSlabIdList.SelectedValue = empRosterBO.SecondTimeSlabId.ToString();
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {
                return;
            }
            List<EmpRosterBO> entityBOList = new List<EmpRosterBO>();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            EmpRosterDA entityDA = new EmpRosterDA();
            foreach (GridViewRow row in gvEmpRoster.Rows)
            {
                EmpRosterBO entityBO = new EmpRosterBO();

                DropDownList FirstTimeSlabIdList = (DropDownList)row.FindControl("ItemDropDown");

                DropDownList SecondTimeSlabIdList = (DropDownList)row.FindControl("SecondTimeSlabId");

                entityBO.EmpId = Convert.ToInt32(this.ddlEmpId.SelectedValue);
                entityBO.RosterId = Convert.ToInt32(this.ddlRosterId.SelectedValue);
                entityBO.RosterDate = hmUtility.GetDateTimeFromString(((Label)row.FindControl("lblgvRosterDate")).Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);//hmUtility.GetDateTimeFromString(((Label)row.FindControl("lblgvRosterDate")).Text);
                entityBO.TimeSlabId = Convert.ToInt32(FirstTimeSlabIdList.SelectedValue);
                entityBO.SecondTimeSlabId = Convert.ToInt32(SecondTimeSlabIdList.SelectedValue);
                entityBO.CreatedBy = userInformationBO.UserInfoId;

                entityBOList.Add(entityBO);
            }

            Boolean status = entityDA.SaveEmpRosterInfo(entityBOList);
            if (status)
            {

                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
               
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpRoster.ToString(), 0,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            "Employee roster saved for each employee");
                
                this.SrcButtonClick();
                //this.txtSrcRoomNumber.Focus();
            }
        }
        
        //************************ User Defined Function ********************//
        private void LoadEmpRoster()
        {
            RosterHeadDA entityDA = new RosterHeadDA();
            RosterHeadBO entityBO = new RosterHeadBO();
            List<RosterHeadBO> entityBOList = new List<RosterHeadBO>();
            entityBOList = entityDA.GetRosterHeadInfo();
            this.ddlRosterId.DataSource = entityBOList;
            this.ddlRosterId.DataTextField = "RosterName";
            this.ddlRosterId.DataValueField = "RosterId";
            this.ddlRosterId.DataBind();

            ListItem rosterItem = new ListItem();
            rosterItem.Value = "0";
            rosterItem.Text = hmUtility.GetDropDownFirstValue();
            this.ddlRosterId.Items.Insert(0, rosterItem);
        }
        private void LoadEmployee()
        {
            EmployeeDA employeeDA = new EmployeeDA();
            EmployeeBO employeeBO = new EmployeeBO();
            List<EmployeeBO> fields = new List<EmployeeBO>();
            fields = employeeDA.GetEmployeeInfo();
            this.ddlEmpId.DataSource = fields;
            this.ddlEmpId.DataTextField = "EmployeeName";
            this.ddlEmpId.DataValueField = "EmpId";
            this.ddlEmpId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlEmpId.Items.Insert(0, item);
        }
        private void SrcButtonClick()
        {            
            if (this.ddlEmpId.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Employee Information.", AlertType.Warning);
                return;
            }
            if (this.ddlRosterId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Roster Information.", AlertType.Warning);
                this.ddlRosterId.Focus();
                return;
            }

            if (this.ddlEmpId.SelectedValue != "0")
            {
                EmployeeDA entityDA = new EmployeeDA();
                EmployeeBO entityBO = new EmployeeBO();
                entityBO = entityDA.GetEmployeeInfoById(Convert.ToInt32(this.ddlEmpId.SelectedValue));
                if (entityBO != null)
                {
                    if (entityBO.EmpId > 0)
                    {
                        PayrollWorkingDayDA da = new PayrollWorkingDayDA();
                        PayrollWorkingDayBO payrollWorkingDayBO = da.GetPayrollWorkingDayInfoByEmpCategoryId(entityBO.EmpTypeId);

                        if (payrollWorkingDayBO != null)
                        {
                            if (payrollWorkingDayBO.WorkingDayId > 0)
                            {
                                if (payrollWorkingDayBO.WorkingPlan == "Roster")
                                {
                                    this.GenerateRosterGridView();
                                }
                                else
                                {
                                    CommonHelper.AlertInfo(innboardMessage, "This Employee is not assigned for Roster Plan.", AlertType.Warning);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void GenerateRosterGridView()
        {
            if (this.ddlRosterId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Roster Information.", AlertType.Warning);
                this.ddlRosterId.Focus();
                return;
            }
            else if (this.ddlEmpId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Employee Information.", AlertType.Warning);
                return;
            }

            isEnableEmpRosterGrid = 1;
            int RosterId = Convert.ToInt32(this.ddlRosterId.SelectedValue);
            DateTime dateTime = DateTime.Now;
            DateTime StartDate = dateTime;
            DateTime EndDate = dateTime;
            RosterHeadBO entityBO = new RosterHeadBO();
            RosterHeadDA entityDA = new RosterHeadDA();
            entityBO = entityDA.GetRosterHeadInfoById(RosterId);
            if (entityBO != null)
            {
                if (!string.IsNullOrWhiteSpace(entityBO.FromDate.ToString()))
                {
                    StartDate = entityBO.FromDate;
                }
                else
                {
                    StartDate = hmUtility.GetDateTimeFromString(hmUtility.GetStringFromDateTime(dateTime), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                }
                if (!string.IsNullOrWhiteSpace(hmUtility.GetStringFromDateTime(entityBO.ToDate)))
                {
                    EndDate = entityBO.ToDate;
                }
                else
                {
                    EndDate = hmUtility.GetDateTimeFromString(hmUtility.GetStringFromDateTime(dateTime), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                }

                List<DateTime> DateList = GetDateArrayBetweenTwoDates(StartDate, EndDate);
                List<EmpRosterBO> empRosterBOList = new List<EmpRosterBO>();

                foreach (DateTime date in DateList)
                {
                    EmpRosterBO empRosterBO = new EmpRosterBO();

                    empRosterBO.RosterDate = date;
                    empRosterBOList.Add(empRosterBO);
                }

                this.gvEmpRoster.DataSource = empRosterBOList;
                this.gvEmpRoster.DataBind();
                this.gvEmpRoster.Focus();
            }
        }
        public static List<DateTime> GetDateArrayBetweenTwoDates(DateTime StartDate, DateTime EndDate)
        {
            var dates = new List<DateTime>();

            for (var dt = StartDate; dt <= EndDate; dt = dt.AddDays(1))
            {
                dates.Add(dt.AddDays(1).AddSeconds(-1));
            }
            return dates;
        }
        public bool isValidForm()
        {
            bool status = true;
            if (this.ddlEmpId.SelectedValue == "0")
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Employee Name.", AlertType.Warning);
                this.ddlEmpId.Focus();
            }

            return status;
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
    }
}