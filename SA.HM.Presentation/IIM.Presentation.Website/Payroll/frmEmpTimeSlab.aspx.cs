using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpTimeSlab : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        protected int isDoubleDay = -1;
        protected int isRoster = -1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                this.isMessageBoxEnable = 2;
                this.lblMessage.Text = "Delete Operation Successfull.";
            }
            if (!IsPostBack)
            {
                this.LoadGridView();
                this.LoadEmployee();
                this.LoadTimeSlab();
                this.SetSelectedDay();
            }
        }

        private void SetSelectedDay()
        {
            ddlSunDay.SelectedIndex = 0;
            ddlMonDay.SelectedIndex = 1;
            ddlTuesDay.SelectedIndex = 2;
            ddlWedDay.SelectedIndex = 3;
            ddlThuDay.SelectedIndex = 4;
            ddlFriday.SelectedIndex = 5;
            ddlSatDay.SelectedIndex = 6;

        }
        private void LoadTimeSlab()
        {
            TimeSlabHeadDA headDA = new TimeSlabHeadDA();
            TimeSlabHeadBO headBO = new TimeSlabHeadBO();
            List<TimeSlabHeadBO> List = new List<TimeSlabHeadBO>();
            List = headDA.GetAllTimeSlabHeadInfo();
            ddlDayFiveTS.DataSource = List;
            ddlDayFourTS.DataSource = List;
            ddlDayOneTS.DataSource = List;
            ddlDaySevenTS.DataSource = List;
            ddlDaySixTS.DataSource = List;
            ddlDayThreeTS.DataSource = List;
            ddlDayTwoTS.DataSource = List;
            ddlTimeSlabId.DataSource = List;
            ddlTimeSlabId.DataSource = List;

            this.ddlTimeSlabId.DataTextField = "TimeSlabHead";
            this.ddlTimeSlabId.DataValueField = "TimeSlabId";
            ddlDayFiveTS.DataTextField = "TimeSlabHead";
            ddlDayFourTS.DataTextField = "TimeSlabHead";
            ddlDayOneTS.DataTextField = "TimeSlabHead";
            ddlDaySevenTS.DataTextField = "TimeSlabHead";
            ddlDaySixTS.DataTextField = "TimeSlabHead";
            ddlDayThreeTS.DataTextField = "TimeSlabHead";
            ddlDayTwoTS.DataTextField = "TimeSlabHead";

            ddlDayFiveTS.DataValueField = "TimeSlabId";
            ddlDayFourTS.DataValueField = "TimeSlabId";
            ddlDayOneTS.DataValueField = "TimeSlabId";
            ddlDaySevenTS.DataValueField = "TimeSlabId";
            ddlDaySixTS.DataValueField = "TimeSlabId";
            ddlDayThreeTS.DataValueField = "TimeSlabId";
            ddlDayTwoTS.DataValueField = "TimeSlabId";

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            item.Text = "---None---";

            this.ddlTimeSlabId.DataBind();
            ddlDayFiveTS.DataBind();
            ddlDayFourTS.DataBind();
            ddlDayOneTS.DataBind();
            ddlDaySevenTS.DataBind();
            ddlDaySixTS.DataBind();
            ddlDayThreeTS.DataBind();
            ddlDayTwoTS.DataBind();
            this.ddlTimeSlabId.Items.Insert(0, item);
            ddlDayFiveTS.Items.Insert(0, item);
            ddlDayFourTS.Items.Insert(0, item);
            ddlDayOneTS.Items.Insert(0, item);
            ddlDaySevenTS.Items.Insert(0, item);
            ddlDaySixTS.Items.Insert(0, item);
            ddlDayThreeTS.Items.Insert(0, item);
            ddlDayTwoTS.Items.Insert(0, item);
        }
        private void LoadEmployee()
        {
            EmployeeDA entityDA = new EmployeeDA();
            this.ddlEmpId.DataSource = entityDA.GetEmployeeInfo();
            this.ddlEmpId.DataTextField = "EmployeeName";
            this.ddlEmpId.DataValueField = "EmpId";
            this.ddlEmpId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = "---None---";
            this.ddlEmpId.Items.Insert(0, item);
        }
        private void LoadGridView()
        {
            this.CheckObjectPermission();
            EmpTimeSlabDA slabDA = new EmpTimeSlabDA();
            List<EmpTimeSlabBO> List = new List<EmpTimeSlabBO>();
            List = slabDA.GetAllTimeSlabInfo();
            gvTimeSlab.DataSource = List;
            gvTimeSlab.DataBind();
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmEmpTimeSlab.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        protected void gvTimeSlab_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            this.lblMessage.Text = string.Empty;
            this.gvTimeSlab.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void gvTimeSlab_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                //  imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //  imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvTimeSlab_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int timeSlabId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(timeSlabId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("PayrollEmpTimeSlab", "EmpTimeSlabId", timeSlabId);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    this.lblMessage.Text = "Delete Operation Successfull.";
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.EmpTimeSlab.ToString(), timeSlabId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTimeSlab));
                }
                LoadGridView();
                this.SetTab("SearchTab");
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
        private void Cancel()
        {
            this.ddlActiveStat.SelectedIndex = 0;
            this.ddlEmpId.SelectedIndex = 0;
            this.ddlWeekEndSecond.SelectedIndex = 0;
            this.ddlTimeSlabId.SelectedIndex = 0;
            this.ddlWeekEndFirst.SelectedIndex = 0;
            this.ddlWeekEndMode.SelectedIndex = 0;
            this.txtSlabEffectDate.Text = string.Empty;
            this.txtEmpTimeSlabId.Value = string.Empty;

            this.btnSave.Text = "Save";

        }
        private bool IsFrmValid()
        {
            bool flag = true;

            if (ddlTimeSlabId.SelectedValue=="Fixed" && this.ddlTimeSlabId.SelectedIndex == 0)
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please select Time Slab Name.";
                flag = false;
                ddlTimeSlabId.Focus();
            }
            else if (this.ddlEmpId.SelectedIndex == 0)
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please select Employee.";
                flag = false;
                ddlEmpId.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtSlabEffectDate.Text.Trim()))
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please provide Effect Date.";
                txtSlabEffectDate.Focus();
                flag = false;
            }

            else
            {
                this.isMessageBoxEnable = -1;
                this.lblMessage.Text = string.Empty;
            }
            return flag;
        }
        [WebMethod]
        public static string DeleteData(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("PayrollEmpTimeSlab", "EmpTimeSlabId", sEmpId);
                if (status)
                {
                    result = "success";
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.EmpTimeSlab.ToString(), sEmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTimeSlab));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public void FillForm(int EditId)
        {
            EmpTimeSlabDA slabDA = new EmpTimeSlabDA();
            EmpTimeSlabBO slabBO = new EmpTimeSlabBO();
            slabBO = slabDA.GetTimeSlabInfoById(EditId);
            slabBO.EffectDate = hmUtility.GetStringFromDateTime(slabBO.SlabEffectDate);
            txtEmpTimeSlabId.Value = slabBO.EmpTimeSlabId.ToString();
            List<EmpTimeSlabRosterBO> rosterList = new List<EmpTimeSlabRosterBO>();
            rosterList = slabDA.GetEmpTimeSlabRosterByEmpTimeSlabId(EditId);
            if (rosterList.Count > 0)
            {
                ddlTimeSlabType.SelectedValue = "Roster";
                isRoster = 1;
                foreach (EmpTimeSlabRosterBO roster in rosterList)
                {

                    if (roster.DayName == "Sunday")
                    {
                        txtEmpTimeSlabId.Value = roster.EmpTimeSlabId.ToString();
                       ddlSunDay.SelectedValue = roster.DayName;
                       ddlDayOneTS.SelectedValue = roster.TimeSlabId.ToString();
                    }
                    else if (roster.DayName == "Monday")
                    {
                        txtEmpTimeSlabId.Value = roster.EmpTimeSlabId.ToString();
                         ddlMonDay. SelectedValue = roster.DayName;
                         ddlDayTwoTS. SelectedValue = roster.TimeSlabId.ToString();
                    }
                    else if (roster.DayName == "Tuesday")
                    {
                        txtEmpTimeSlabId.Value = roster.EmpTimeSlabId.ToString();
                       ddlTuesDay. SelectedValue = roster.DayName;
                       ddlDayThreeTS. SelectedValue = roster.TimeSlabId.ToString();
                    }
                    else if (roster.DayName == "Wednesday")
                    {
                        txtEmpTimeSlabId.Value = roster.EmpTimeSlabId.ToString();
                       ddlWedDay. SelectedValue = roster.DayName;
                       ddlDayFourTS.SelectedValue = roster.TimeSlabId.ToString();
                    }
                    else if (roster.DayName == "Thursday")
                    {
                        txtEmpTimeSlabId.Value = roster.EmpTimeSlabId.ToString();
                       ddlThuDay. SelectedValue = roster.DayName;
                       ddlDayFiveTS. SelectedValue = roster.TimeSlabId.ToString();
                    }
                    else if (roster.DayName == "Friday")
                    {
                        txtEmpTimeSlabId.Value = roster.EmpTimeSlabId.ToString();
                       ddlFriday. SelectedValue = roster.DayName;
                       ddlDaySixTS. SelectedValue = roster.TimeSlabId.ToString();
                    }
                    else if (roster.DayName == "Saturday")
                    {
                        txtEmpTimeSlabId.Value = roster.EmpTimeSlabId.ToString();
                       ddlSatDay. SelectedValue = roster.DayName;
                       ddlDaySevenTS.SelectedValue = roster.TimeSlabId.ToString();
                    }
                }
            
            }

            ddlActiveStat.SelectedIndex = slabBO.ActiveStat == true ? 0 : 1;
            txtSlabEffectDate.Text = slabBO.EffectDate;
            ddlEmpId.SelectedValue = slabBO.EmpId.ToString();
            ddlTimeSlabId.SelectedValue = slabBO.TimeSlabId.ToString();
            ddlWeekEndMode.SelectedValue = slabBO.WeekEndMode;
            ddlWeekEndFirst.SelectedValue = slabBO.WeekEndFirst;
            ddlWeekEndSecond.SelectedValue = slabBO.WeekEndSecond;

            if (slabBO.WeekEndMode == "Double")
            {
                isDoubleDay = 1;
            }
            else
            {
                isDoubleDay = -1;
            }
            btnSave.Text = "Update";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //EmpTimeSlabRosterBO
            if (!IsFrmValid())
            {
                return;
            }
            List<EmpTimeSlabRosterBO> List = new List<EmpTimeSlabRosterBO>();
            lblMessage.Text = string.Empty;
            if (ddlTimeSlabType.SelectedValue == "Fixed")
            {
            }
            else if (ddlTimeSlabType.SelectedValue == "Roster")
            {   EmpTimeSlabRosterBO rosterBO=new EmpTimeSlabRosterBO();
                
                rosterBO.DayName = ddlSunDay.SelectedValue;
                rosterBO.TimeSlabId = Int32.Parse(ddlDayOneTS.SelectedValue);
                List.Add(rosterBO);
                rosterBO = new EmpTimeSlabRosterBO();

                rosterBO.DayName = ddlMonDay.SelectedValue;
                rosterBO.TimeSlabId = Int32.Parse(ddlDayTwoTS.SelectedValue);
                List.Add(rosterBO);
                rosterBO = new EmpTimeSlabRosterBO();

                rosterBO.DayName = ddlTuesDay.SelectedValue;
                rosterBO.TimeSlabId = Int32.Parse(ddlDayThreeTS.SelectedValue);
                List.Add(rosterBO);
                rosterBO = new EmpTimeSlabRosterBO();

                rosterBO.DayName = ddlWedDay.SelectedValue;
                rosterBO.TimeSlabId = Int32.Parse(ddlDayFourTS.SelectedValue);
                List.Add(rosterBO);
                rosterBO = new EmpTimeSlabRosterBO();

                rosterBO.DayName = ddlThuDay.SelectedValue;
                rosterBO.TimeSlabId = Int32.Parse(ddlDayFiveTS.SelectedValue); 
                List.Add(rosterBO);
                rosterBO = new EmpTimeSlabRosterBO();

                rosterBO.DayName =ddlFriday.SelectedValue; 
                rosterBO.TimeSlabId = Int32.Parse(ddlDaySixTS.SelectedValue);
                List.Add(rosterBO);
                rosterBO = new EmpTimeSlabRosterBO();

                rosterBO.DayName = ddlSatDay.SelectedValue;
                rosterBO.TimeSlabId = Int32.Parse(ddlDaySevenTS.SelectedValue);
                List.Add(rosterBO);
                rosterBO = null;
            }


            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            EmpTimeSlabBO slabBO = new EmpTimeSlabBO();
            EmpTimeSlabDA slabDA = new EmpTimeSlabDA();

            slabBO.TimeSlabId = Int32.Parse(ddlTimeSlabId.SelectedItem.Value);
            slabBO.SlabEffectDate = hmUtility.GetDateTimeFromString(txtSlabEffectDate.Text, userInformationBO.ServerDateFormat);
            slabBO.WeekEndMode = ddlWeekEndMode.SelectedItem.Value;

            if (slabBO.WeekEndMode == "Double")
            {
                slabBO.WeekEndFirst = ddlWeekEndFirst.SelectedItem.Value;
                slabBO.WeekEndSecond = ddlWeekEndSecond.SelectedItem.Value;
            }
            else
            {
                slabBO.WeekEndFirst = ddlWeekEndFirst.SelectedItem.Value;
                slabBO.WeekEndSecond = "";
            }

            slabBO.EmpId = Int32.Parse(ddlEmpId.SelectedItem.Value);
            slabBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

            if (string.IsNullOrWhiteSpace(txtEmpTimeSlabId.Value))
            {
                int tmpUserInfoId = 0;
                slabBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status;
                if (List.Count == 0)
                {
                     status = slabDA.SaveAllTimeSlabInfo(slabBO, out tmpUserInfoId);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpTimeSlab.ToString(), tmpUserInfoId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            "All time slab info saved");
                }
                else
                {
                     status = slabDA.SaveEachDayTimeSlabInfo(slabBO,List, out tmpUserInfoId);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpTimeSlab.ToString(), tmpUserInfoId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            "Each day time slab info saved");
                }
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Saved Operation Successfull.";
                    btnSave.Text = "Save";
                    this.LoadGridView();
                    this.Cancel();
                }
                else
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "You Can't Save.This Salary Formula Already Inserted.";
                    this.LoadGridView();
                    this.Cancel();
                }
            }
            else
            {
                slabBO.EmpTimeSlabId = Convert.ToInt32(txtEmpTimeSlabId.Value);
                slabBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status;
                if (List.Count == 0)
                {
                    status = slabDA.UpdateTimeSlabInfo(slabBO);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpTimeSlab.ToString(), slabBO.EmpTimeSlabId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            "All time slab info updated");
                }
                else
                {
                    status = slabDA.UpdateEachDayTimeSlabInfo(slabBO,List);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpTimeSlab.ToString(), slabBO.EmpTimeSlabId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            "Each day time slab info updated");
                }
                if (status)
                {
                    btnSave.Text = "Save";
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Update Operation Successfull.";
                    this.LoadGridView();
                    this.Cancel();

                    txtEmpTimeSlabId.Value = "";
                }
            }

        }
        private void WeekModeChange()
        {
            if (ddlWeekEndMode.SelectedValue != "Double")
            {
                ddlWeekEndSecond.Enabled = false;
                // $("#<%=ddlWeekEndSecond.ClientID %>").attr("disabled", true);
                //$("#<%=ddlWeekEndSecond.ClientID %>").addClass('readOnly');
            }
            else
            {

                ddlWeekEndSecond.Enabled = true;
                ddlWeekEndFirst.Enabled = true;
                //$("#<%=ddlWeekEndSecond.ClientID %>").attr("disabled", false);
                //$("#<%=ddlWeekEndSecond.ClientID %>").removeClass('readOnly');
            }
        }

    }
}