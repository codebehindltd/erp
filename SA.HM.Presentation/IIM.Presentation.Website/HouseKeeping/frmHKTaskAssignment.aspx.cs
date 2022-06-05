using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HouseKeeping;
using HotelManagement.Entity.HouseKeeping;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using System.Web.Services;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.HouseKeeping
{
    public partial class frmHKTaskAssignment : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected int IsSearchSuccess = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadFloorBlock();
                LoadFloor();
                LoadCommonDropDownHiddenField();
            }
        }
        protected void gvTaskAssignment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTaskAssignment.PageIndex = e.NewPageIndex;
            LoadDataOnIndexing();
        }
        protected void gvTaskAssignment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                DropDownList ddlHKRoomStatus = (DropDownList)e.Row.FindControl("ddlHKRoomStatus");
                Label lblTaskId = (Label)e.Row.FindControl("lblTaskId");
                CheckBox cb = (CheckBox)e.Row.FindControl("chkIsSavePermission");

                HKRoomStatusDA HKRoomStatusDA = new HKRoomStatusDA();
                List<HKRoomStatusBO> files = HKRoomStatusDA.GetHKRoomStatusType();

                ddlHKRoomStatus.DataSource = files;
                ddlHKRoomStatus.DataTextField = "StatusName";
                ddlHKRoomStatus.DataValueField = "HKRoomStatusId";
                ddlHKRoomStatus.DataBind();

                string hkRoomStatus = (e.Row.FindControl("lblHKRoomStatus") as Label).Text;
                if (!string.IsNullOrEmpty(hkRoomStatus))
                    ddlHKRoomStatus.Items.FindByText(hkRoomStatus).Selected = true;

                ddlHKRoomStatus.Enabled = false;

                if (!string.IsNullOrEmpty(lblTaskId.Text))
                {
                    if (Convert.ToInt32(lblTaskId.Text) > 0)
                    {
                        cb.Checked = true;
                    }
                }
            }
        }

        protected void gvTaskAssignmentList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int taskId = Convert.ToInt32(e.CommandArgument.ToString());
            EmpSalaryProcessDA salaryProcessDa = new EmpSalaryProcessDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (e.CommandName == "CmdEdit")
            {
                LoadTaskList(taskId);
                LoadDataOnChange(taskId);
                LoadEmployeeOnChange(taskId);
                SetTab("Entry");
            }
        }

        protected void gvEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblTaskId = (Label)e.Row.FindControl("lblTaskId");
                CheckBox cb = (CheckBox)e.Row.FindControl("chkIsSavePermission");

                if (!string.IsNullOrEmpty(lblTaskId.Text))
                {
                    if (Convert.ToInt32(lblTaskId.Text) > 0)
                    {
                        cb.Checked = true;
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            IsSearchSuccess = 1;
            LoadDataOnChange(0);
            LoadEmployeeOnChange(0);
            SetHKStatus();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Int64 taskId = 0;
            bool status = false;
            int rows = 0;

            taskId = Convert.ToInt64(hfTaskId.Value);

            HotelEmpTaskAssignmentBO savetask = new HotelEmpTaskAssignmentBO();

            List<TaskAssignmentToEmployeeBO> saveEmployeeForTaskList = new List<TaskAssignmentToEmployeeBO>();
            List<TaskAssignmentToEmployeeBO> deleteEmployeeForTaskList = new List<TaskAssignmentToEmployeeBO>();

            List<TaskAssignmentRoomWiseBO> saveRoomWiseTaskList = new List<TaskAssignmentRoomWiseBO>();
            List<TaskAssignmentRoomWiseBO> deleteRoomWiseTaskList = new List<TaskAssignmentRoomWiseBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            savetask.TaskId = taskId;
            savetask.Shift = ddlShift.SelectedValue.ToString();
            savetask.RoomNumber = "";
            savetask.FloorId = Convert.ToInt32(ddlSrcFloorId.SelectedValue);
            savetask.CreatedBy = userInformationBO.UserInfoId;

            //--------Room Assignment
            rows = gvTaskAssignment.Rows.Count;

            for (int i = 0; i < rows; i++)
            {
                CheckBox cb = (CheckBox)gvTaskAssignment.Rows[i].FindControl("chkIsSavePermission");
                Label lblRoomTaskId = (Label)gvTaskAssignment.Rows[i].FindControl("lblRoomTaskId");
                Label lblRoomId = (Label)gvTaskAssignment.Rows[i].FindControl("lblRoomId");
                TextBox txtTskInst = (TextBox)gvTaskAssignment.Rows[i].FindControl("txtTaskInstructions");
                Label lblTaskId = (Label)gvTaskAssignment.Rows[i].FindControl("lblTaskId");

                if (cb.Checked == true && lblTaskId.Text == "0")
                {
                    TaskAssignmentRoomWiseBO bo = new TaskAssignmentRoomWiseBO();
                    DropDownList ddlHKRoomStatus = (DropDownList)gvTaskAssignment.Rows[i].FindControl("ddlHKRoomStatus");

                    bo.RoomTaskId = Convert.ToInt64(lblRoomTaskId.Text);
                    bo.TaskId = Convert.ToInt64(lblTaskId.Text);
                    bo.RoomId = Convert.ToInt32(lblRoomId.Text);
                    bo.TaskDetails = txtTskInst.Text;
                    bo.TaskStatus = "Pending";
                    bo.HKRoomStatusId = Convert.ToInt64(ddlHKRoomStatus.SelectedValue);

                    saveRoomWiseTaskList.Add(bo);
                }
                else
                {
                    if (lblTaskId != null)
                    {
                        if (cb.Checked == false && lblTaskId.Text != "0")
                        {
                            TaskAssignmentRoomWiseBO bo = new TaskAssignmentRoomWiseBO();
                            bo.RoomTaskId = Convert.ToInt64(lblRoomTaskId.Text);
                            bo.TaskId = Convert.ToInt32(lblTaskId.Text);
                            deleteRoomWiseTaskList.Add(bo);
                        }
                    }
                }
            }

            //------Employee Assignment
            rows = gvEmployee.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                CheckBox cb = (CheckBox)gvEmployee.Rows[i].FindControl("chkIsSavePermission");
                Label lblEmpId = (Label)gvEmployee.Rows[i].FindControl("lblEmpId");
                Label lblTaskId = (Label)gvEmployee.Rows[i].FindControl("lblTaskId");
                Label lblEmpTaskId = (Label)gvEmployee.Rows[i].FindControl("lblEmpTaskId");

                if (cb.Checked == true && lblTaskId.Text == "0")
                {
                    TaskAssignmentToEmployeeBO bo = new TaskAssignmentToEmployeeBO();

                    bo.EmpTaskId = Convert.ToInt64(lblEmpTaskId.Text);
                    bo.TaskId = Convert.ToInt32(lblTaskId.Text);
                    bo.EmpId = Convert.ToInt32(lblEmpId.Text);

                    saveEmployeeForTaskList.Add(bo);
                }
                else
                {
                    if (lblTaskId != null)
                    {
                        if (cb.Checked == false && lblTaskId.Text != "0")
                        {
                            TaskAssignmentToEmployeeBO bo = new TaskAssignmentToEmployeeBO();
                            bo.EmpTaskId = Convert.ToInt64(lblEmpTaskId.Text);
                            bo.TaskId = Convert.ToInt32(lblTaskId.Text);
                            bo.EmpTaskId = Convert.ToInt32(lblEmpTaskId.Text);

                            deleteEmployeeForTaskList.Add(bo);
                        }
                    }
                }
            }

            HKRoomStatusDA roomStatusDA = new HKRoomStatusDA();
            long tmpTaskId;

            if (taskId == 0)
            {
                status = roomStatusDA.SaveEmpTaskAssignment(savetask, saveEmployeeForTaskList, saveRoomWiseTaskList, out tmpTaskId);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.HotelEmpTaskAssignment.ToString(), 
                        tmpTaskId, ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelEmpTaskAssignment));
                }
            }
            else if (taskId > 0)
            {
                status = roomStatusDA.UpdateEmpTaskAssignment(savetask, saveEmployeeForTaskList, saveRoomWiseTaskList, deleteEmployeeForTaskList, deleteRoomWiseTaskList);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.HotelEmpTaskAssignment.ToString(),
                        savetask.TaskId, ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelEmpTaskAssignment));
                }
            }

            
            ClearData();
        }
        protected void btnSearchTaskAssignment_Click(object sender, EventArgs e)
        {
            HKRoomStatusDA hkRoomStatusDA = new HKRoomStatusDA();
            List<HotelEmpTaskAssignmentBO> taskList = new List<HotelEmpTaskAssignmentBO>();

            string shift = ddlShiftForSearch.SelectedValue.ToString();

            taskList = hkRoomStatusDA.GetTaskAssignment(shift);

            gvTaskAssignmentList.DataSource = taskList;
            gvTaskAssignmentList.DataBind();

            SetTab("Search");

        }
        //************************ User Defined Function ********************//
        private void LoadTaskList(long taskId)
        {
            HKRoomStatusDA hkRoomStatusDA = new HKRoomStatusDA();
            HotelEmpTaskAssignmentBO task = new HotelEmpTaskAssignmentBO();
            task = hkRoomStatusDA.GetTaskAssignmentById(taskId);

            hfTaskId.Value = task.TaskId.ToString();
            ddlShift.SelectedValue = task.Shift;

            if (!string.IsNullOrEmpty(task.RoomNumber))
            {
                txtRoomNumber.Text = task.RoomNumber;
            }
            else
            {
                if (task.FloorId != null)
                    ddlSrcFloorId.SelectedValue = task.FloorId.ToString();
            }
            btnSave.Text = "Update";
        }
        private void LoadDataOnChange(long taskId)
        {
            string shift = ddlShift.SelectedValue.ToString();
            int floorId = Convert.ToInt32(ddlSrcFloorId.SelectedValue);
            int foStatusId = Convert.ToInt32(ddlFOStatus.SelectedValue);
            int blockId = Convert.ToInt32(ddlFloorBlock.SelectedValue);
            string roomNumberFrom = string.Empty, roomNumberTo = string.Empty;

            roomNumberFrom = txtRoomNumber.Text;
            roomNumberTo = txtRoomNumberTo.Text;

            HKRoomStatusDA hkRoomStatusDA = new HKRoomStatusDA();
            List<HKRoomStatusViewBO> roomStatusList = new List<HKRoomStatusViewBO>();
            List<TaskAssignmentRoomWiseBO> taskList = new List<TaskAssignmentRoomWiseBO>();
            List<HotelEmpTaskAssignmentBO> removedTaskList = new List<HotelEmpTaskAssignmentBO>();

            roomStatusList = hkRoomStatusDA.GetHKRoomConditionsForTaskAssignment(floorId, foStatusId, blockId, roomNumberFrom, roomNumberTo);
            taskList = hkRoomStatusDA.GetAssignedTaskLists(taskId);

            foreach (HKRoomStatusViewBO bo in roomStatusList)
            {
                if (bo.FORoomStatus != "Occupied")
                {
                    bo.DateIn = "";
                    bo.DateOut = "";
                }
            }

            //if (empId == 0)
            //{
            //    gvTaskAssignment.DataSource = roomStatusList;
            //    gvTaskAssignment.DataBind();
            //}

            {
                //if (taskList != null && empId > 0)
                //{
                //    removedTaskList = taskList.Where(m => m.EmpId != empId).ToList();

                //    taskList = taskList.Where(m => m.EmpId == empId).ToList();
                //}
                //foreach (HotelEmpTaskAssignmentBO bo in removedTaskList)
                //{
                //    HKRoomStatusViewBO viewBO = roomStatusList.Where(m => m.RoomId == bo.RoomId).FirstOrDefault();
                //    roomStatusList.Remove(viewBO);
                //}

                foreach (TaskAssignmentRoomWiseBO bo in taskList)
                {
                    HKRoomStatusViewBO viewBO = roomStatusList.Where(m => m.RoomId == bo.RoomId).FirstOrDefault();
                    if (viewBO != null)
                    {
                        viewBO.TaskDetails = bo.TaskDetails;
                        viewBO.TaskId = bo.TaskId;
                        viewBO.HKRoomStatus = bo.HKStatusName;
                        viewBO.RoomTaskId = bo.RoomTaskId;
                    }
                }

                if (roomStatusList.Count > 0)
                {
                    IsSearchSuccess = 1;
                }

                gvTaskAssignment.DataSource = roomStatusList;
                gvTaskAssignment.DataBind();
            }
            hfStatusType.Value = ddlStatusType.SelectedValue;
            SetTab("Entry");
        }
        private void LoadEmployeeOnChange(long taskId)
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
            invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("HouseKeepingDepartmentId", "HouseKeepingDepartmentId");

            int departmentId = 0;
            if (!string.IsNullOrEmpty(invoiceTemplateBO.SetupValue))
                departmentId = Convert.ToInt32(invoiceTemplateBO.SetupValue);

            HKRoomStatusDA roomStatusDA = new HKRoomStatusDA();
            List<TaskWiseEmployeeVWBO> empList = new List<TaskWiseEmployeeVWBO>();
            empList = roomStatusDA.GetEmployeeByDepartmentForTaskAssignment(taskId, departmentId);

            gvEmployee.DataSource = empList;
            gvEmployee.DataBind();
        }

        private void EditTaskAssignment()
        {
            HKRoomStatusDA hkRoomStatusDA = new HKRoomStatusDA();
            List<HKRoomStatusViewBO> roomStatusList = new List<HKRoomStatusViewBO>();
            List<TaskAssignmentRoomWiseBO> taskList = new List<TaskAssignmentRoomWiseBO>();
            List<HotelEmpTaskAssignmentBO> removedTaskList = new List<HotelEmpTaskAssignmentBO>();

            //roomStatusList = hkRoomStatusDA.GetRoomStatus(0, foStatusId, blockId);
            //taskList = hkRoomStatusDA.GetAssignedTaskLists(taskId);

            foreach (HKRoomStatusViewBO bo in roomStatusList)
            {
                if (bo.FORoomStatus != "Occupied")
                {
                    bo.DateIn = "";
                    bo.DateOut = "";
                }
            }

            foreach (TaskAssignmentRoomWiseBO bo in taskList)
            {
                HKRoomStatusViewBO viewBO = roomStatusList.Where(m => m.RoomId == bo.RoomId).FirstOrDefault();
                if (viewBO != null)
                {
                    viewBO.TaskDetails = bo.TaskDetails;
                    viewBO.TaskId = bo.TaskId;
                    viewBO.HKRoomStatus = bo.HKStatusName;
                    viewBO.RoomTaskId = bo.RoomTaskId;
                }
            }

            if (roomStatusList.Count > 0)
            {
                IsSearchSuccess = 1;
            }

            gvTaskAssignment.DataSource = roomStatusList;
            gvTaskAssignment.DataBind();
        }

        private void LoadDataOnIndexing()
        {
            string shift = ddlShift.SelectedValue.ToString();
            int floorId = Convert.ToInt32(ddlSrcFloorId.SelectedValue);

            HKRoomStatusDA hkRoomStatusDA = new HKRoomStatusDA();
            List<HKRoomStatusViewBO> roomStatusList = new List<HKRoomStatusViewBO>();
            List<HotelEmpTaskAssignmentBO> taskList = new List<HotelEmpTaskAssignmentBO>();
            List<HotelEmpTaskAssignmentBO> removedTaskList = new List<HotelEmpTaskAssignmentBO>();

            roomStatusList = hkRoomStatusDA.GetRoomStatus(floorId, 0, 0);
            //taskList = hkRoomStatusDA.GetAssignedTaskLists(shift);
            foreach (HKRoomStatusViewBO bo in roomStatusList)
            {
                if (bo.FORoomStatus != "Occupied")
                {
                    bo.DateIn = "";
                    bo.DateOut = "";
                }
            }

            //if (empdId == 0)
            //{
            //    gvTaskAssignment.DataSource = roomStatusList;
            //    gvTaskAssignment.DataBind();
            //}
            //else
            //{
            //    if (taskList != null && empdId > 0)
            //    {
            //        removedTaskList = taskList.Where(m => m.EmpId != empdId).ToList();
            //        taskList = taskList.Where(m => m.EmpId == empdId).ToList();
            //    }
            //    foreach (HotelEmpTaskAssignmentBO bo in removedTaskList)
            //    {
            //        HKRoomStatusViewBO viewBO = roomStatusList.Where(m => m.RoomId == bo.RoomId).FirstOrDefault();
            //        roomStatusList.Remove(viewBO);
            //    }
            //    foreach (HotelEmpTaskAssignmentBO bo in taskList)
            //    {
            //        HKRoomStatusViewBO viewBO = roomStatusList.Where(m => m.RoomId == bo.RoomId).FirstOrDefault();
            //        if (viewBO != null)
            //        {
            //            //viewBO.TaskDetails = bo.TaskDetails;
            //            viewBO.TaskId = bo.TaskId;
            //            //viewBO.HKRoomStatus = bo.HKStatusName;
            //        }
            //    }

            //    if (roomStatusList.Count > 0)
            //    {
            //        IsSuccess = 1;
            //    }

            //    gvTaskAssignment.DataSource = roomStatusList;
            //    gvTaskAssignment.DataBind();
            //}
        }
        private void SetHKStatus()
        {
            var shift = ddlShift.SelectedValue;
            int rows = gvTaskAssignment.Rows.Count;

            for (int i = 0; i < rows; i++)
            {
                DropDownList ddlHKRoomStatus = (DropDownList)gvTaskAssignment.Rows[i].FindControl("ddlHKRoomStatus");
                Label lblFORoomStatus = (Label)gvTaskAssignment.Rows[i].FindControl("lblFORoomStatus");
                Label lblHKRoomStatus = (Label)gvTaskAssignment.Rows[i].FindControl("lblHKRoomStatus");
                if (shift == "AM")
                {
                    var v = ddlHKRoomStatus.Items.FindByText("Dirty").Value;
                    //if (lblFORoomStatus.Text == "Occupied")
                    //{
                    //    ddlHKRoomStatus.SelectedValue = v;
                    //    ddlHKRoomStatus.Enabled = false;
                    //}
                }
                else if (shift == "PM")
                {
                    var v = ddlHKRoomStatus.Items.FindByText("Turn Down").Value;
                    if (lblFORoomStatus.Text == "Occupied")
                    {
                        ddlHKRoomStatus.SelectedValue = v;
                        ddlHKRoomStatus.Enabled = false;
                    }
                    if (lblFORoomStatus.Text == "Vacant")
                    {
                        if (lblHKRoomStatus.Text == "Turn Down")
                        {
                            var vv = ddlHKRoomStatus.Items.FindByText("Dirty").Value;
                            ddlHKRoomStatus.SelectedValue = vv;
                            ddlHKRoomStatus.Enabled = false;
                        }
                    }
                }

                if (lblFORoomStatus.Text == "Vacant")
                {
                    if (lblHKRoomStatus.Text == "Occupied")
                    {
                        var v = ddlHKRoomStatus.Items.FindByText("Dirty").Value;
                        ddlHKRoomStatus.SelectedValue = v;
                        ddlHKRoomStatus.Enabled = false;
                    }
                }
            }
        }
        private void LoadDepartment()
        {
            //int HouseKeepingDepartmentId = 0;
            //HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            //HMCommonSetupBO HouseKeepingDepartmentIdBO = new HMCommonSetupBO();
            //HouseKeepingDepartmentIdBO = commonSetupDA.GetCommonConfigurationInfo("HouseKeepingDepartmentId", "HouseKeepingDepartmentId");
            //if (HouseKeepingDepartmentIdBO != null)
            //{
            //    if (HouseKeepingDepartmentIdBO.SetupId > 0)
            //    {
            //        HouseKeepingDepartmentId = Convert.ToInt32(HouseKeepingDepartmentIdBO.SetupValue);
            //    }
            //}

            //DepartmentDA entityDA = new DepartmentDA();
            //List<DepartmentBO> DepartmentBOList = new List<DepartmentBO>();
            //DepartmentBOList = entityDA.GetDepartmentInfo();
            //if (HouseKeepingDepartmentId == 0)
            //{
            //    ddlDepartment.DataSource = DepartmentBOList;
            //}
            //else
            //{
            //    ddlDepartment.DataSource = DepartmentBOList.Where(x => x.DepartmentId == HouseKeepingDepartmentId).ToList();
            //}
            //ddlDepartment.DataTextField = "Name";
            //ddlDepartment.DataValueField = "DepartmentId";
            //ddlDepartment.DataBind();

            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //ddlDepartment.Items.Insert(0, item);
        }
        private void LoadFloor()
        {
            HMFloorDA floorDA = new HMFloorDA();
            ddlSrcFloorId.DataSource = floorDA.GetActiveHMFloorInfo();
            ddlSrcFloorId.DataTextField = "FloorName";
            ddlSrcFloorId.DataValueField = "FloorId";
            ddlSrcFloorId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSrcFloorId.Items.Insert(0, item);
        }
        private void LoadFloorBlock()
        {
            FloorBlockDA floorDA = new FloorBlockDA();
            List<FloorBlockBO> floorList = new List<FloorBlockBO>();
            floorList = floorDA.GetActiveFloorBlockInfo();

            ddlFloorBlock.DataSource = floorList;
            ddlFloorBlock.DataTextField = "BlockName";
            ddlFloorBlock.DataValueField = "BlockId";
            ddlFloorBlock.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlFloorBlock.Items.Insert(0, item);
        }
        private void LoadChangeEvent()
        {
            ClearData();
            LoadDataOnChange(0);
            SetHKStatus();
        }
        private void ClearData()
        {
            hfTaskId.Value = "0";
            btnSave.Text = "Save";
            ddlStatusType.SelectedValue = "0";
            ddlShift.SelectedValue = "AM";
            ddlSrcFloorId.SelectedValue = "0";

            gvEmployee.DataSource = null;
            gvEmployee.DataBind();

            gvTaskAssignment.DataSource = null;
            gvTaskAssignment.DataBind();
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }

        private void SetTab(string TabName)
        {
            if (TabName == "Search")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "Entry")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return CommonHelper.DateTimeConvertion(dateTime);
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<EmployeeBO> LoadEmployee(int depId)
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetEmployeeByDepartment(depId);

            return empList;
        }
        [WebMethod]
        public static List<EmployeeBO> LoadDesignation(int empId)
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            EmployeeBO empBO = new EmployeeBO();

            empBO = empDA.GetEmployeeInfoById(empId);
            empList.Add(empBO);

            return empList;
        }

    }
}