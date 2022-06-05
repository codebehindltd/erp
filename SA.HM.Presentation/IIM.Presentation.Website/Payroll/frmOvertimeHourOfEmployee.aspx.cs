using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Data.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmOvertimeHourOfEmployee : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadDepartment();
            }
        }

        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            this.ddlDepartment.DataSource = entityDA.GetDepartmentInfo();
            this.ddlDepartment.DataTextField = "Name";
            this.ddlDepartment.DataValueField = "DepartmentId";
            this.ddlDepartment.DataBind();

            this.ddlDepartment.Items.Insert(0, item);
        }

        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmLeaveInformation.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }

        private void SetTab(string TabName)
        {
            if (TabName == "entry")
            {
                entry.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                search.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "search")
            {
                search.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                entry.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }

        private void Clear()
        {
            hfTransferId.Value = "";

            btnSave.Text = "Save";
            SetTab("entry");

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl empSearch;

            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            empSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeeSearch");
            ((HiddenField)empSearch.FindControl("hfEmployeeId")).Value = string.Empty;
            ((HiddenField)empSearch.FindControl("hfEmployeeDepartment")).Value = string.Empty;
            ((TextBox)empSearch.FindControl("txtSearchEmployee")).Text = string.Empty;
            ((TextBox)empSearch.FindControl("txtEmployeeName")).Text = string.Empty;
            ((TextBox)empSearch.FindControl("txtEmpDepart")).Text = string.Empty;
        }

        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }

        private void FillForm(Int64 transferId)
        {
            EmployeeDA empDa = new EmployeeDA();
            EmpTransferBO transfer = new EmpTransferBO();

            transfer = empDa.GetEmpTransferById(transferId);

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl empSearch;
            HiddenField employeeId;
            HiddenField departmentId;

            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            empSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeeSearch");
            employeeId = (HiddenField)empSearch.FindControl("hfEmployeeId");
            departmentId = (HiddenField)empSearch.FindControl("hfEmployeeDepartment");

            hfTransferId.Value = transferId.ToString();

            //txtTransferDate.Text = transfer.TransferDate.ToString();
            employeeId.Value = Convert.ToString(transfer.EmpId);
            departmentId.Value = Convert.ToString(transfer.PreviousDepartmentId);

            ((TextBox)empSearch.FindControl("txtSearchEmployee")).Text = transfer.EmpCode;
            ((TextBox)empSearch.FindControl("txtEmployeeName")).Text = transfer.EmployeeName;
            ((TextBox)empSearch.FindControl("txtEmpDepart")).Text = transfer.PreviousDepartmentName;

            //ddlDepartmentFrom.SelectedValue = transfer.PreviousDepartmentId.ToString();
            ddlDepartment.SelectedValue = transfer.CurrentDepartmentId.ToString();
            //txtReportingDate.Text = transfer.ReportingDate.ToString();
            //txtJoinedDate.Text = transfer.JoinedDate.ToString();
            // ddlReportingTo.SelectedValue = transfer.ReportingToId.ToString();

            btnSave.Text = "Update";

            SetTab("entry");
        }

        private void LoadGrid()
        {
            DateTime dateFrom = DateTime.Now, dateTo = DateTime.Now;

            if (!string.IsNullOrEmpty(txtDateFrom.Text))
            {
                //dateFrom = Convert.ToDateTime(txtDateFrom.Text);
                dateFrom = CommonHelper.DateTimeToMMDDYYYY(txtDateFrom.Text);
            }

            if (!string.IsNullOrEmpty(txtDateTo.Text))
            {
                //dateTo = Convert.ToDateTime(txtDateTo.Text);
                dateTo = CommonHelper.DateTimeToMMDDYYYY(txtDateTo.Text);
            }

            List<EmpTransferBO> employeeList = new List<EmpTransferBO>();
            EmployeeDA empDa = new EmployeeDA();

            ContentPlaceHolder mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            UserControl empSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeForLeaveSearch"); ;
            DropDownList srcType = (DropDownList)empSearch.FindControl("ddlEmployee");
            HiddenField employeeId = (HiddenField)empSearch.FindControl("hfEmployeeId");
            var type = srcType.SelectedValue;
            var empId = employeeId.Value;

            employeeList = empDa.GetEmpTransfer(dateFrom, dateTo, Convert.ToInt32(type), Convert.ToInt32(empId));

            gvEmployeeTransfer.DataSource = employeeList;
            gvEmployeeTransfer.DataBind();

            SetTab("search");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool status = false;

            try
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                EmpTransferBO transfer = new EmpTransferBO();
                EmployeeDA empDa = new EmployeeDA();

                ContentPlaceHolder mpContentPlaceHolder;
                UserControl empSearch;
                HiddenField employeeId;
                HiddenField departmentId;

                mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
                empSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeeSearch");
                employeeId = (HiddenField)empSearch.FindControl("hfEmployeeId");
                departmentId = (HiddenField)empSearch.FindControl("hfEmployeeDepartment");

                //transfer.TransferDate = Convert.ToDateTime(txtTransferDate.Text);
                transfer.EmpId = Convert.ToInt32(employeeId.Value);
                //transfer.PreviousDepartmentId = Convert.ToInt32(ddlDepartmentFrom.SelectedValue);
                transfer.PreviousDepartmentId = Convert.ToInt32(departmentId.Value);
                transfer.CurrentDepartmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
                transfer.PreviousLocation = null;
                transfer.CurrentLocation = null;
                //transfer.ReportingDate = Convert.ToDateTime(txtReportingDate.Text);
                //transfer.JoinedDate = Convert.ToDateTime(txtJoinedDate.Text);
                // transfer.ReportingToId = Convert.ToInt32(ddlReportingTo.SelectedValue);

                transfer.TransferId = hfTransferId.Value == "" ? 0 : Convert.ToInt64(hfTransferId.Value);

                if (transfer.TransferId == 0)
                {
                    int tmpTransferId = 0;
                    transfer.CreatedBy = Convert.ToInt32(userInformationBO.UserInfoId);
                    status = empDa.SaveEmpTransfer(transfer, out tmpTransferId);

                    if (status == true)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpTransfer.ToString(), tmpTransferId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTransfer));
                        Clear();
                    }
                }
                else
                {
                    transfer.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);
                    status = empDa.UpdateEmpTransfer(transfer);

                    if (status == true)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpTransfer.ToString(), transfer.TransferId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTransfer));
                        Clear();
                    }
                }

                if (!status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void gvEmployeeTransfer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            try
            {
                if (e.CommandName == "CmdEdit")
                {
                    int transferId = Convert.ToInt32(e.CommandArgument.ToString());
                    string result = string.Empty;

                    EmployeeDA empDa = new EmployeeDA();
                    FillForm(transferId);
                }
                else if (e.CommandName == "CmdApproved")
                {
                    int transferId = Convert.ToInt32(e.CommandArgument.ToString());
                    string result = string.Empty;

                    EmployeeDA empDa = new EmployeeDA();

                    EmpTransferBO trn = new EmpTransferBO();
                    trn.TransferId = transferId;
                    trn.LastModifiedBy = userInformationBO.UserInfoId;
                    trn.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();

                    bool status = empDa.UpdateEmpTransferStatus(trn);

                    if (status)
                    {
                        //Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approved.ToString(), EntityTypeEnum.EntityType.EmpGratuity.ToString(), empId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpGratuity));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
                        LoadGrid();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                    this.SetTab("SearchTab");
                }
                else if (e.CommandName == "CmdDelete")
                {
                    int transferId = Convert.ToInt32(e.CommandArgument.ToString());
                    string result = string.Empty;

                    EmployeeDA empDa = new EmployeeDA();
                    EmpTransferBO trn = new EmpTransferBO();
                    trn.TransferId = transferId;
                    trn.LastModifiedBy = userInformationBO.UserInfoId;
                    trn.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();

                    bool status = empDa.DeleteEmpTransfer(trn);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.EmpTransfer.ToString(), trn.TransferId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTransfer));
                        LoadGrid();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                    this.SetTab("SearchTab");
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }

        protected void gvEmployeeTransfer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                var item = (EmpTransferBO)e.Row.DataItem;

                ImageButton imgApproved = (ImageButton)e.Row.FindControl("ImgApproved");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");

                if (item.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString() && item.ApprovedStatus != HMConstants.ApprovalStatus.Cancel.ToString())
                {
                    imgUpdate.Visible = true;
                    imgDelete.Visible = true;
                    imgApproved.Visible = true;
                }
                else
                {
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                    imgApproved.Visible = false;
                }
            }
        }

        protected void btnSearchOt_Click(object sender, EventArgs e)
        {
            int? employeeId = null, departmentId = null; DateTime? attendanceDate = null;
            string empId = string.Empty;

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl empSearch;
            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            empSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeeSearch");

            empId = ((HiddenField)empSearch.FindControl("hfEmployeeId")).Value;

            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");

            if (empId != "0")
            {
                employeeId = Convert.ToInt32(empId);
            }

            if (ddlDepartment.SelectedValue != "0")
            {
                departmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
            }

            if (txtAttenDanceDate.Text != "")
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                attendanceDate = hmUtility.GetDateTimeFromString(txtAttenDanceDate.Text, userInformationBO.ServerDateFormat); //Convert.ToDateTime(txtAttenDanceDate.Text);
            }

            EmpOverTimeDA overtimeDa = new EmpOverTimeDA();
            List<OvertimeHourOfEmployeeeBO> overtime = new List<OvertimeHourOfEmployeeeBO>();

            overtime = overtimeDa.GetOvertimeHourOfEmployeee(employeeId, departmentId, attendanceDate);

            string strTable = string.Empty;
            int counter = 0;

            strTable += "<table  id='OvertimeTbl' cellspacing='0' cellpadding='4' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='display:none'></th><th style='display:none'></th>";

            strTable += "<th align='left' scope='col' style='width: 15%;'>Employee Name</th>";
            strTable += "<th align='left' scope='col' style='width: 10%;'>Code</th>";
            strTable += "<th align='left' scope='col' style='width: 15%;'>Designation</th>";
            strTable += "<th align='left' scope='col' style='width: 10%;'>Attendance Date</th>";
            strTable += "<th align='left' scope='col' style='width: 10%;'>Entry Time</th>";
            strTable += "<th align='left' scope='col' style='width: 10%;'>Exit Time</th>";
            strTable += "<th align='left' scope='col' style='width: 10%;'>Total Hour</th>";
            strTable += "<th align='left' scope='col' style='width: 10%;'>OT Hour</th>";
            strTable += "<th align='left' scope='col' style='width: 10%;'>Approved OT Hour</th>";

            strTable += "</tr></thead><tbody>";

            foreach (OvertimeHourOfEmployeeeBO ot in overtime)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    strTable += "<tr style='background-color:White;'>";
                }
                else
                {
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }

                strTable += "<td align='left' style=\"display:none;\">" + ot.EmpId + "</td>";
                strTable += "<td align='left' style=\"display:none;\"></td>";
                strTable += "<td align='left' style='width:15%;'>" + ot.EmployeeName + "</td>";
                strTable += "<td align='left' style='width:10%;'>" + ot.EmpCode + "</td>";
                strTable += "<td align='left' style='width:15%;'>" + ot.Designation + "</td>";
                strTable += "<td align='left' style='width:10%;'>" + hmUtility.GetStringFromDateTime(ot.AttendanceDate) + "</td>";
                strTable += "<td align='left' style='width:10%;'>" + Convert.ToDateTime(ot.EntryTime).ToString("hh tt") + "</td>";
                strTable += "<td align='left' style='width:10%;'>" + Convert.ToDateTime(ot.ExitTime).ToString("hh tt") + "</td>";
                strTable += "<td align='left' style='width:10%;'>" + ot.TotalHour + "</td>";
                strTable += "<td align='left' style='width:10%;'>" + ot.OTHour + "</td>";
                strTable += "<td align='left' style='width:10%;'><input type='text' style='height:15px; width:30px;' value='' /></td>";
                strTable += "<td align='left' style=\"display:none;\">" + ot.EntryTime + "</td>";
                strTable += "<td align='left' style=\"display:none;\">" + ot.ExitTime + "</td>";

                strTable += "</tr>";
            }

            strTable += "</tbody> </table>";

            litOtDetails.Text = strTable;
        }

        [WebMethod]
        public static ReturnInfo OvertimeSave(List<EmpOverTimeBO> EmpOverTime)
        {
            int overTimeId = 0;
            ReturnInfo rtninf = new ReturnInfo();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                //EmpOverTime.CreatedBy = userInformationBO.UserInfoId;

                EmpOverTimeDA otDa = new EmpOverTimeDA();
                bool status = otDa.SaveOvertime(EmpOverTime, userInformationBO.UserInfoId,  out overTimeId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpOverTime.ToString(), overTimeId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpOverTime));
                    //return true;
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rtninf;
        }
    }
}