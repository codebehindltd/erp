using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpLeaveAdjustment : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility(); 
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.CheckObjectPermission();
                this.LoadSalaryProcessMonth();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            LeaveInformationBO empLeaveInfoBO = new LeaveInformationBO();
            EmpSalaryProcessDA empSalaryProcessDA = new EmpSalaryProcessDA();

            string selectedMonthRange = this.ddlEffectedMonth.SelectedValue.ToString();

            int startMonth = Convert.ToInt32(selectedMonthRange.Split('-')[0]);
            int endMonth = Convert.ToInt32(selectedMonthRange.Split('-')[1]);
            int salaryMonthStartDateDay = Convert.ToInt32(selectedMonthRange.Split('-')[2]);

            string fromSalaryDate = string.Empty;
            string toSalaryDate = string.Empty;

            if (startMonth == endMonth)
            {
                fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year;
                if (startMonth != 12)
                {
                    toSalaryDate = (endMonth + 1).ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year;
                }
                else
                {
                    toSalaryDate = "01" + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + (DateTime.Now.Year + 1);
                }

            }
            else
            {
                if (startMonth < endMonth)
                {
                    fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year;
                    toSalaryDate = endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year;
                }
                else
                {
                    fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year;
                    toSalaryDate = endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + (DateTime.Now.Year + 1);
                }
            }

            if (!string.IsNullOrEmpty(fromSalaryDate))
            {
                empLeaveInfoBO.FromDate = hmUtility.GetDateTimeFromString(fromSalaryDate, userInformationBO.ServerDateFormat);
            }

            if (!string.IsNullOrEmpty(toSalaryDate))
            {
                empLeaveInfoBO.ToDate = hmUtility.GetDateTimeFromString(toSalaryDate, userInformationBO.ServerDateFormat);
            }

            if (!string.IsNullOrWhiteSpace(txtSrcEmpCode.Text))
            {
                EmployeeNameDiv.Visible = true;
                EmployeeBO empBo = new EmployeeBO();
                EmployeeDA employeeDA = new EmployeeDA();
                empBo = employeeDA.GetEmployeeInfoByCode(txtSrcEmpCode.Text.Trim());
                if (empBo != null)
                {
                    if (empBo.EmpId > 0)
                    {
                        empLeaveInfoBO.EmpId = empBo.EmpId;
                        txtEmployeeName.Text = empBo.EmployeeName;
                    }
                    else
                    {
                        empLeaveInfoBO.EmpId = 0;
                    }
                }
                else
                {
                    empLeaveInfoBO.EmpId = 0;
                }
            }
            else
            {
                EmployeeNameDiv.Visible = false;
                empLeaveInfoBO.EmpId = 0;
            }

            LeaveInformationDA entityDA = new LeaveInformationDA();
            List<EmpOverTimeBO> employeeLeave = entityDA.GetPayrollInsteadLeaveForApproval(empLeaveInfoBO);

            this.gvLeaveInformation.DataSource = employeeLeave;
            this.gvLeaveInformation.DataBind();
        }
        protected void gvLeaveInformation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdSave")
            {
                try
                {
                    int overTimeId = Convert.ToInt32(e.CommandArgument.ToString());
                    GridViewRow row = (GridViewRow)((Control)e.CommandSource).Parent.Parent;
                    DropDownList dpdListEstatus = (DropDownList)row.FindControl("dpdListEstatus");
                    string val = dpdListEstatus.SelectedValue;
                    
                    this.ApprovedGuest(overTimeId, val);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (e.CommandName == "CmdLeave")
            {
                int overTimeId = Convert.ToInt32(e.CommandArgument.ToString());
                this.ApprovedGuest(overTimeId, "Leave");
            }
            else if (e.CommandName == "CmdPayment")
            {
                int overTimeId = Convert.ToInt32(e.CommandArgument.ToString());
                this.ApprovedGuest(overTimeId, "Payment");
            }
        }
        protected void gvLeaveInformation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {

                EmpOverTimeBO empOverTimeBO = (EmpOverTimeBO)e.Row.DataItem;
                DropDownList dpdListEstatus = (DropDownList)e.Row.FindControl("dpdListEstatus");

                //if (empOverTimeBO.OverTimeStatusForIL.Equals("0"))
                //    dpdListEstatus.SelectedIndex = 0;
                //else if (empOverTimeBO.OverTimeStatusForIL.Equals("1"))
                //    dpdListEstatus.SelectedIndex = 1;
                //else
                //    dpdListEstatus.SelectedIndex = 2;
            }
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            btnSearch.Visible = isSavePermission;
        }
        private void LoadSalaryProcessMonth()
        {
            EmpSalaryProcessDA entityDA = new EmpSalaryProcessDA();
            this.ddlEffectedMonth.DataSource = entityDA.GetSalaryProcessMonth();
            this.ddlEffectedMonth.DataTextField = "MonthHead";
            this.ddlEffectedMonth.DataValueField = "MonthValue";
            this.ddlEffectedMonth.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlEffectedMonth.Items.Insert(0, item);
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        public void ApprovedGuest(int overTimeId, string transactionType)
        {
            HMUtility hmUtility = new HMUtility();
            LeaveInformationBO leaveInformationBO = new LeaveInformationBO();
            LeaveInformationDA leaveInformationDA = new LeaveInformationDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            HMCommonSetupBO payrollInsteadLeaveHeadBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            payrollInsteadLeaveHeadBO = commonSetupDA.GetCommonConfigurationInfo("InsteadLeaveHeadId", "PayrollInsteadLeaveHeadId");
            string insteadLeaveHeadId = payrollInsteadLeaveHeadBO.SetupValue;

            HMCommonSetupBO payrollInsteadLeaveForOneHolidayBO = new HMCommonSetupBO();
            //HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            payrollInsteadLeaveForOneHolidayBO = commonSetupDA.GetCommonConfigurationInfo("InsteadLeaveForOneHoliday", "PayrollInsteadLeaveForOneHoliday");
            int insteadLeaveForOneHoliday = !string.IsNullOrWhiteSpace(payrollInsteadLeaveForOneHolidayBO.SetupValue) ? Convert.ToInt32(payrollInsteadLeaveForOneHolidayBO.SetupValue) : 0;

            EmpOverTimeBO empOverTimeBO = new EmpOverTimeBO();
            EmpOverTimeDA empOverTimeDA = new EmpOverTimeDA();
            empOverTimeBO = empOverTimeDA.GetOverTimeInfoByID(overTimeId);
            int tmpLeaveId;
            if (empOverTimeBO != null)
            {
                if (empOverTimeBO.EmpId > 0)
                {
                    if (!string.IsNullOrEmpty(insteadLeaveHeadId))
                    {
                        //if (transactionType == "Payment")
                        //{
                        leaveInformationBO.EmpId = empOverTimeBO.EmpId;
                        leaveInformationBO.LeaveMode = "Planned";
                        leaveInformationBO.LeaveTypeId = Convert.ToInt32(insteadLeaveHeadId);
                        leaveInformationBO.FromDate = empOverTimeBO.OverTimeDate;
                        leaveInformationBO.ToDate = empOverTimeBO.OverTimeDate;
                        if (transactionType == "Payment")
                        {
                            leaveInformationBO.NoOfDays = 1;
                        }
                        else if (transactionType == "Substitute Leave")
                        {
                            leaveInformationBO.NoOfDays = insteadLeaveForOneHoliday;
                        }
                        else
                        {
                            leaveInformationBO.NoOfDays = 0;
                        }
                        leaveInformationBO.TransactionType = "Addition";
                        leaveInformationBO.ExpireDate = DateTime.Now.AddYears(2);
                        leaveInformationBO.LeaveStatus = "Approved";
                        leaveInformationBO.CreatedBy = userInformationBO.UserInfoId;

                        Boolean status = leaveInformationDA.SaveAndUpdateEmpLeaveInformation(leaveInformationBO, out tmpLeaveId);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Approved Operation Successfull.", AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpLeaveInfo.ToString(), tmpLeaveId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            "Employee Leave Approved");
                        }
                        //}
                        //else
                        //{
                        //    if (insteadLeaveForOneHoliday > 0)
                        //    {
                        //        int count = 0;
                        //        for (int i = 1; i <= insteadLeaveForOneHoliday; i++)
                        //        {
                        //            leaveInformationBO.EmpId = empOverTimeBO.EmpId;
                        //            leaveInformationBO.LeaveMode = "Planned";
                        //            leaveInformationBO.LeaveTypeId = Convert.ToInt32(insteadLeaveHeadId);
                        //            leaveInformationBO.FromDate = empOverTimeBO.OverTimeDate;
                        //            leaveInformationBO.ToDate = empOverTimeBO.OverTimeDate;
                        //            leaveInformationBO.NoOfDays = 1;
                        //            leaveInformationBO.LeaveStatus = "Approved";
                        //            leaveInformationBO.CreatedBy = userInformationBO.UserInfoId;
                        //            int tmpUserInfoId = 0;
                        //            Boolean status = leaveInformationDA.SaveEmpLeaveInformation(leaveInformationBO, out tmpUserInfoId);
                        //            if (status)
                        //            {
                        //                count = count + 1;
                        //                //this.isMessageBoxEnable = 2;
                        //                //lblMessage.Text = "Approved Operation Successfull.";

                        //            }
                        //        }

                        //        if (count == insteadLeaveForOneHoliday)
                        //        { 
                        //        this.isMessageBoxEnable = 2;
                        //        lblMessage.Text = "Approved Operation Successfull.";
                        //        }
                        //    }
                        //}
                    }


                }
            }
        }
    }
}