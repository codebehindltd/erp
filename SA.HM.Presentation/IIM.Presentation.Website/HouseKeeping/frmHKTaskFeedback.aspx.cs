using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.HouseKeeping;
using HotelManagement.Entity.HouseKeeping;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using System.Web.Services;
using HotelManagement.Entity.Payroll;
using System.Text.RegularExpressions;

namespace HotelManagement.Presentation.Website.HouseKeeping
{
    public partial class frmHKTaskFeedback : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int IsSuccess = -1;
        List<TaskWiseEmployeeVWBO> assignedEmployeeList = new List<TaskWiseEmployeeVWBO>();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadEmployee();
                LoadHKRoomStatus();
                LoadTemplateNo();
                LoadCommonDropDownHiddenField();
                if (hfTemplateNo.Value == "1")
                {
                    LoadAssignedRoomsNTasks();
                }
            }
        }
        protected void gvTaskFeedback_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTaskFeedback.PageIndex = e.NewPageIndex;
            LoadAssignedRoomsNTasks();

            ClearGrid();
        }
        protected void gvTaskFeedback_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                DropDownList ddlHKRoomStatus = (DropDownList)e.Row.FindControl("ddlHKRoomStatus");
                DropDownList ddlAssignedEmployee = (DropDownList)e.Row.FindControl("ddlAssignedEmployee");

                TextBox txtFeedback = (TextBox)e.Row.FindControl("txtFeedback");
                Label lblFORoomStatus = (Label)e.Row.FindControl("lblFORoomStatus");
                Label lblEmpId = (Label)e.Row.FindControl("lblEmpId");

                HKRoomStatusDA HKRoomStatusDA = new HKRoomStatusDA();
                List<HKRoomStatusBO> files = HKRoomStatusDA.GetHKRoomStatusType().ToList();

                ddlHKRoomStatus.DataSource = files;
                ddlHKRoomStatus.DataTextField = "StatusName";
                ddlHKRoomStatus.DataValueField = "HKRoomStatusId";
                ddlHKRoomStatus.DataBind();

                ddlAssignedEmployee.DataSource = assignedEmployeeList;
                ddlAssignedEmployee.DataTextField = "FullName";
                ddlAssignedEmployee.DataValueField = "EmpId";
                ddlAssignedEmployee.DataBind();

                if (lblFORoomStatus.Text == "Occupied")
                {
                    string hkRoomStatus = (e.Row.FindControl("lblHKRoomStatus") as Label).Text;
                    if (!string.IsNullOrEmpty(hkRoomStatus))
                    {
                        if (hkRoomStatus != "Out of Order" || hkRoomStatus != "Out of Service")
                        {
                            ddlHKRoomStatus.Items.FindByText(hkRoomStatus).Selected = true;
                        }
                    }
                }
                else
                {
                    string hkRoomStatus = (e.Row.FindControl("lblHKRoomStatus") as Label).Text;
                    if (!string.IsNullOrEmpty(hkRoomStatus))
                        ddlHKRoomStatus.Items.FindByText(hkRoomStatus).Selected = true;
                }

                if (lblFORoomStatus.Text == "Occupied")
                {
                    ddlHKRoomStatus.Items.Remove(ddlHKRoomStatus.Items.FindByText("Out of Order"));
                    ddlHKRoomStatus.Items.Remove(ddlHKRoomStatus.Items.FindByText("Out of Service"));
                }
                
                CheckBox cb = (CheckBox)e.Row.FindControl("chkIsSavePermission");
                if (!string.IsNullOrEmpty(txtFeedback.Text))
                {
                    cb.Checked = true;
                    ddlAssignedEmployee.SelectedValue = lblEmpId.Text;
                }
            }
        }
        protected void ddlShift_Change(object sender, EventArgs e)
        {
            //LoadHKRoomStatus();
            //LoadData();
            ClearGrid();
            LoadShiftWiseTask();
        }
        protected void ddlAllClean_Change(object sender, EventArgs e)
        {
            string hkStatus = ddlAllClean.SelectedValue;
            int rows = gvTaskFeedback.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                DropDownList ddlHKRoomStatus = (DropDownList)gvTaskFeedback.Rows[i].FindControl("ddlHKRoomStatus");
                if (hkStatus != "0")
                {
                    ddlHKRoomStatus.SelectedValue = hkStatus;
                }
            }
            IsSuccess = 1;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<TaskAssignmentRoomWiseBO> updatetaskList = new List<TaskAssignmentRoomWiseBO>();
            int err = 0;
            int rows = gvTaskFeedback.Rows.Count;
            int count = 0;           

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            for (int i = 0; i < rows; i++)
            {
                TaskAssignmentRoomWiseBO bo = new TaskAssignmentRoomWiseBO();
                CheckBox cb = (CheckBox)gvTaskFeedback.Rows[i].FindControl("chkIsSavePermission");
                if (cb.Checked == true)
                {
                    count++;                    
                    HotelRoomDiscrepancyBO discrebo = new HotelRoomDiscrepancyBO();

                    Label lblTaskId = (Label)gvTaskFeedback.Rows[i].FindControl("lblTaskId");
                    Label lblRoomId = (Label)gvTaskFeedback.Rows[i].FindControl("lblRoomId");

                    DropDownList ddlHKRoomStatus = (DropDownList)gvTaskFeedback.Rows[i].FindControl("ddlHKRoomStatus");
                    DropDownList ddlAssignedEmployee = (DropDownList)gvTaskFeedback.Rows[i].FindControl("ddlAssignedEmployee");

                    TextBox txtFeedback = (TextBox)gvTaskFeedback.Rows[i].FindControl("txtFeedback");
                    TextBox txtInTime = (TextBox)gvTaskFeedback.Rows[i].FindControl("txtInTime");
                    TextBox txtOutTime = (TextBox)gvTaskFeedback.Rows[i].FindControl("txtOutTime");

                    bo.TaskId = Convert.ToInt32(lblTaskId.Text);
                    bo.RoomId = Convert.ToInt32(lblRoomId.Text);
                    bo.EmpId = Convert.ToInt32(ddlAssignedEmployee.SelectedValue);
                    bo.HKRoomStatusId = Convert.ToInt64(ddlHKRoomStatus.SelectedValue);
                    bo.HKStatusName = ddlHKRoomStatus.SelectedItem.Text;
                    bo.Feedbacks = txtFeedback.Text;

                    if (string.IsNullOrWhiteSpace(txtInTime.Text.Trim()))
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Please Provide Valid In-Time.", AlertType.Warning);
                        txtInTime.Focus();
                        IsSuccess = 1;
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtOutTime.Text.Trim()))
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Please Provide Valid Out-Time.", AlertType.Warning);
                        txtOutTime.Focus();
                        IsSuccess = 1;
                        return;
                    }

                    if (!string.IsNullOrEmpty(txtInTime.Text))
                    {
                        bo.InTime = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(txtInTime.Text.Trim()).ToString("HH:mm:ss"));
                    }
                        

                    if (!string.IsNullOrEmpty(txtOutTime.Text))
                    {
                        bo.OutTime = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(txtOutTime.Text.Trim()).ToString("HH:mm:ss"));
                    }

                    if (string.IsNullOrEmpty(txtFeedback.Text))
                    {
                        bo.Feedbacks = ddlHKRoomStatus.SelectedItem.Text;
                    }

                    updatetaskList.Add(bo);
                }
            }

            if (err == 1)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide feedback for each selected room.", AlertType.Warning);
                return;
            }
            if (count == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please check the checkbox for operation.", AlertType.Warning);
                return;
            }
            HKRoomStatusDA roomStatusDA = new HKRoomStatusDA();
            bool status = false;
            if (updatetaskList.Count > 0)
            {
                status = roomStatusDA.UpdateEmpTaskFromFeedback(updatetaskList);
            }

            if (status)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.TaskAssignmentRoomWise.ToString(),
                       0, ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(), "Room Wise Task Assignment Updated");

                ClearData();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        //************************ User Defined Function ********************//
        private void LoadEmployee()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            UserInformationDA uiDA = new UserInformationDA();
            UserInformationBO bo = uiDA.GetUserInformationByEmpId(userInformationBO.UserInfoId);
            if (bo != null)
            {
                if (bo.EmpId > 0)
                {
                    //txtEmployee.Text = bo.DisplayName;
                }
            }
        }
        private void LoadHKRoomStatus()
        {
            HKRoomStatusDA HKRoomStatusDA = new HKRoomStatusDA();
            List<HKRoomStatusBO> files = HKRoomStatusDA.GetHKRoomStatusType();
            var shift = "";
            if (hfTemplateNo.Value == "1")
            {
                shift = ddlShift.SelectedValue.ToString();
            }
            else
            {
                //shift = ddlShift2.SelectedValue.ToString();
            }
            if (shift == "AM")
            {
                files = files.Where(a => a.StatusName == "Clean").ToList();
            }
            else
            {
                files = files.Where(a => a.StatusName == "Turn Down").ToList();
            }

            ddlAllClean.DataSource = files;
            ddlAllClean.DataTextField = "StatusName";
            ddlAllClean.DataValueField = "HKRoomStatusId";
            ddlAllClean.DataBind();

            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            ddlAllClean.Items.Insert(0, FirstItem);
        }
        private void LoadAssignedRoomsNTasks()
        {
            LoadData();
        }
        private void LoadData()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            HKRoomStatusDA hkRoomStatusDA = new HKRoomStatusDA();
            List<TaskAssignmentRoomWiseBO> roomStatusAndTaskList = new List<TaskAssignmentRoomWiseBO>();
            roomStatusAndTaskList = hkRoomStatusDA.GetAssignedTaskDetailsById(Convert.ToInt64(ddlTaskSequence.SelectedValue));

            List<TaskWiseEmployeeVWBO> assignedEmployee = new List<TaskWiseEmployeeVWBO>();
            assignedEmployee = hkRoomStatusDA.GetEmployeeByTaskId(Convert.ToInt64(ddlTaskSequence.SelectedValue));
            TaskWiseEmployeeVWBO a = new TaskWiseEmployeeVWBO();
            a.EmpId = 0;
            a.FullName = "---Please Select---";

            assignedEmployeeList.Add(a);
            assignedEmployeeList.AddRange(assignedEmployee);

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            List<HotelEmpTaskAssignmentBO> taskList = new List<HotelEmpTaskAssignmentBO>();

            gvTaskFeedback.DataSource = roomStatusAndTaskList;
            gvTaskFeedback.DataBind();

            IsSuccess = 1;
            //LoadHKRoomStatus();
        }
        private void LoadShiftWiseTask()
        {
            HKRoomStatusDA hkRoomStatusDA = new HKRoomStatusDA();
            List<HotelEmpTaskAssignmentBO> taskList = new List<HotelEmpTaskAssignmentBO>();

            taskList = hkRoomStatusDA.GetTaskAssignment(ddlShift.SelectedValue);

            ddlTaskSequence.DataSource = taskList;
            ddlTaskSequence.DataTextField = "TaskSequence";
            ddlTaskSequence.DataValueField = "TaskId";
            ddlTaskSequence.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlTaskSequence.Items.Insert(0, item);
        }
        private void LoadTemplateNo()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsHKTaskFeedbackSingleEmployee", "IsHKTaskFeedbackSingleEmployee");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    hfTemplateNo.Value = commonSetupBO.SetupValue;
                }
            }
        }
        private void ClearData()
        {
            ddlAllClean.SelectedValue = "0";
            hfEmpId.Value = "0";

            ClearGrid();
        }
        private void ClearGrid()
        {
            List<TaskAssignmentRoomWiseBO> roomStatusAndTaskList = new List<TaskAssignmentRoomWiseBO>();

            gvTaskFeedback.DataSource = roomStatusAndTaskList;
            gvTaskFeedback.DataBind();
        }       
        protected string GetStringFromDateTime(string dateTime)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (dateTime != string.Empty)
                return Convert.ToDateTime(dateTime).ToString(userInformationBO.TimeFormat);
            else return string.Empty;
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<EmployeeBO> LoadEmployee(int depId)
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetTaskAssignedEmployeeByDepartment(depId);

            return empList;
        }
    }
}