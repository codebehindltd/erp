using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Text.RegularExpressions;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.SalesManagment;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmPFConfiguration : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            try
            {
                if (!IsPostBack)
                {
                    this.LoadPayrollProvidentFundTitleText();
                    this.CheckObjectPermission();
                    this.LoadCompanyContributionOn();
                    this.LoadAttendanceDevice();
                    this.LoadSalaryHead();
                    this.LoadOverTimeMode();
                    this.LoadSalaryMonthStartDate();
                    this.LoadBonusMonthlyEffectedPeriod();
                    this.LoadBasicHeadSetupInfo();
                    this.LoadSalaryProcessSystem();
                    this.LoadLeaveType();
                    this.LoadBonusHeadSetupInfo();
                    this.LoadMonthlyWorkingDay();
                    this.LoadWorkingHour();
                    this.LoadMinimumOvertimeHour();
                    this.LoadInsteadLeaveForOneHoliday();
                    this.LoadMonthlyWorkingDayForAbsentree();
                    this.LoadInsteadLeaveHead();
                    this.LoadLoan();
                    this.LoadPF();
                    this.LoadTax();
                    this.LoadGratuity();
                    this.LoadBonus();
                    LoadAmountType();
                    GetDependsOnHeadList();
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }
        private void LoadPayrollProvidentFundTitleText()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            PanelHeadingTitleText.InnerText = "Employee " + userInformationBO.PayrollProvidentFundTitleText + "  Information";
        }
        private void CheckObjectPermission()
        {
            
            btnOverTimeInfoSave.Visible = isSavePermission;
            btnSalaryMonthStartDateInfoSave.Visible = isSavePermission;
            btnEmpPFSave.Visible = isSavePermission;
        }
        private bool IsValidMail(string Email)
        {
            bool status = true;
            string expression = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|" + @"0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z]" + @"[a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";

            Match match = Regex.Match(Email, expression, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }
        private void LoadAttendanceDevice()
        {
            List<AttendanceDeviceBO> attendanceDeviceBOList = new List<AttendanceDeviceBO>();
            AttendanceDeviceDA attendanceDeviceDA = new AttendanceDeviceDA();
            attendanceDeviceBOList = attendanceDeviceDA.GetAllAttendanceDeviceInfo();

            if (attendanceDeviceBOList != null)
            {
                this.ddlAttendanceDevice.DataSource = attendanceDeviceBOList;
                this.ddlAttendanceDevice.DataTextField = "ReaderId";
                this.ddlAttendanceDevice.DataValueField = "ReaderId";
                this.ddlAttendanceDevice.DataBind();

                ListItem item = new ListItem();
                item.Value = "0";
                //item.Value = "---None---";
                item.Text = hmUtility.GetDropDownFirstValue();
                this.ddlAttendanceDevice.Items.Insert(0, item);
            }
        }
        //--Overtime Information----------------------------------------------        
        private void LoadOverTimeMode()
        {

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Overtime", "Overtime Setup");
            string mainString = commonSetupBO.SetupValue;
            if (!string.IsNullOrEmpty(mainString))
            {

                this.txtOverTimeSetupId.Value = commonSetupBO.SetupId.ToString();
                string[] dataArray = mainString.Split('~');
                this.ddlSalaryHeadId.SelectedValue = dataArray[0];
                //this.txtMonthlyTotalHour.Text = dataArray[1];
                this.btnOverTimeInfoSave.Text = "Update";
            }
        }
        private bool IsFrmOverTimeInfoValid()
        {

            bool flag = true;
            if (this.ddlSalaryHeadId.SelectedIndex == 0)
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "Please Select Valid Overtime Head";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Valid Overtime Head.", AlertType.Warning);
                this.ddlSalaryHeadId.Focus();
                flag = false;
            }
            //if (string.IsNullOrWhiteSpace(this.txtMonthlyTotalHour.Text.Trim()))
            //{
            //    this.isMessageBoxEnable = 1;
            //    this.lblMessage.Text = "Please Provide Valid From Input";
            //    this.txtMonthlyTotalHour.Focus();
            //    flag = false;
            //}
            //else
            //{
            //    this.isMessageBoxEnable = -1;
            //    this.lblMessage.Text = string.Empty;               
            //}
            return flag;
        }        
        private void ClearOverTimeInfo()
        {
            txtOverTimeSetupId.Value = string.Empty;
        }
        //--Salary Month Start Date Information----------------------------------------------
        private void LoadSalaryMonthStartDate()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Salary Start Date", "Salary Start Date Setup");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                this.ddlStartDate.SelectedValue = commonSetupBO.SetupValue;
                this.txtStartDateId.Value = commonSetupBO.SetupId.ToString();
                this.btnSalaryMonthStartDateInfoSave.Text = "Update";
            }
        }
        private void LoadBonusMonthlyEffectedPeriod()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollBonusMonthlyEffectedPeriod", "PayrollBonusMonthlyEffectedPeriod");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                this.ddlBonusMonthlyPeriod.SelectedValue = commonSetupBO.SetupValue;
                this.hfBonusMonthlyPeriodId.Value = commonSetupBO.SetupId.ToString();
            }
        }
        private void LoadSalaryProcessSystem()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Salary Process System", "Salary Process System Setup");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                this.ddlSalaryProcessSystem.SelectedValue = commonSetupBO.SetupValue;
                this.txtSalaryProcessId.Value = commonSetupBO.SetupId.ToString();
                this.btnSalaryProcess.Text = "Update";
            }
        }
        protected void btnSalaryProcess_Click(object sender, EventArgs e)
        {

            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(this.txtSalaryProcessId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtSalaryProcessId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "Salary Process System Setup";
            commonSetupBO.TypeName = "Salary Process System";
            commonSetupBO.SetupValue = ddlSalaryProcessSystem.SelectedValue;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Salary Process system Updated Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Salary Process system Updated Successfull.", AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpSalaryProcess.ToString(), tmpSetupId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpSalaryProcess));
                this.btnSalaryProcess.Text = "Update";
            }
            else
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Salary Process system Saved Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Salary Process system Saved Successfull.", AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.EmpSalaryProcess.ToString(), tmpSetupId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpSalaryProcess));
                this.btnSalaryProcess.Text = "Update";
            }


        }
        protected void btnSalaryMonthStartDateInfoSave_Click(object sender, EventArgs e)
        {
            //if (!IsFrmBasicHeadSetupInfoValid())
            //{
            //    return;
            //}
            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(this.txtStartDateId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtStartDateId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "Salary Start Date Setup";
            commonSetupBO.TypeName = "Salary Start Date";
            commonSetupBO.SetupValue = ddlStartDate.SelectedValue;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Salary Start Date Updated Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Salary Start Date Updated Successfull.", AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.EmpSalaryProcess.ToString(), tmpSetupId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "Salary Start Date Updated");
            }
            else
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Salary Start Date Saved Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Salary Start Date Saved Successfull.", AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.EmpSalaryProcess.ToString(), tmpSetupId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "Salary Start Date Saved");
                this.btnBasicHeadSetup.Text = "Update";
            }

        }
        protected void btnBonusMonthlyPeriodInfoSave_Click(object sender, EventArgs e)
        {
            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(this.hfBonusMonthlyPeriodId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.hfBonusMonthlyPeriodId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "PayrollBonusMonthlyEffectedPeriod";
            commonSetupBO.TypeName = "PayrollBonusMonthlyEffectedPeriod";
            commonSetupBO.SetupValue = ddlBonusMonthlyPeriod.SelectedValue;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Bonus Monthly Period Updated Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Bonus Monthly Period Updated Successfull.", AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.EmpBonus.ToString(), tmpSetupId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                          "Monthly Bonus Period Updated");
            }
            else
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Bonus Monthly Period Saved Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Bonus Monthly Period Saved Successfull.", AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.EmpBonus.ToString(), tmpSetupId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "Monthly Bonus Period Saved");
            }

        }
        private void ClearSalaryMonthStartDateInfo()
        {
            txtStartDateId.Value = string.Empty;
        }
        //--Salary Head Information---------------------------------------------- 
        public void LoadSalaryHead()
        {
            SalaryHeadDA salaryHeadDA = new SalaryHeadDA();
            List<SalaryHeadBO> fields = new List<SalaryHeadBO>();
            fields = salaryHeadDA.GetSalaryHeadInfo();
            this.ddlSalaryHeadId.DataSource = fields;
            this.ddlSalaryHeadId.DataTextField = "SalaryHead";
            this.ddlSalaryHeadId.DataValueField = "SalaryHeadId";
            this.ddlSalaryHeadId.DataBind();

            ListItem item1 = new ListItem();
            item1.Value = "0";
            item1.Text = hmUtility.GetDropDownFirstValue();
            this.ddlSalaryHeadId.Items.Insert(0, item1);

            this.ddlBasicSalaryHeadId.DataSource = fields;
            this.ddlBasicSalaryHeadId.DataTextField = "SalaryHead";
            this.ddlBasicSalaryHeadId.DataValueField = "SalaryHeadId";
            this.ddlBasicSalaryHeadId.DataBind();

            ListItem itemBasicSalary = new ListItem();
            itemBasicSalary.Value = "0";
            itemBasicSalary.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBasicSalaryHeadId.Items.Insert(0, itemBasicSalary);

            this.ddlBonusHeadId.DataSource = fields;
            this.ddlBonusHeadId.DataTextField = "SalaryHead";
            this.ddlBonusHeadId.DataValueField = "SalaryHeadId";
            this.ddlBonusHeadId.DataBind();

            ListItem bonus = new ListItem();
            bonus.Value = "0";
            bonus.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBonusHeadId.Items.Insert(0, bonus);

            this.ddlDependsOn.DataSource = fields;
            this.ddlDependsOn.DataTextField = "SalaryHead";
            this.ddlDependsOn.DataValueField = "SalaryHeadId";
            this.ddlDependsOn.DataBind();

            ListItem item2 = new ListItem();
            item2.Value = "0";
            item2.Text = hmUtility.GetDropDownFirstValue();
            this.ddlDependsOn.Items.Insert(0, item2);

            ddlEmployeeContributionHeadId.DataSource = fields;
            ddlEmployeeContributionHeadId.DataTextField = "SalaryHead";
            ddlEmployeeContributionHeadId.DataValueField = "SalaryHeadId";
            ddlEmployeeContributionHeadId.DataBind();

            ddlEmployeeContributionHeadId.Items.Insert(0, itemBasicSalary);

            ddlCompanyContributionHeadId.DataSource = fields;
            ddlCompanyContributionHeadId.DataTextField = "SalaryHead";
            ddlCompanyContributionHeadId.DataValueField = "SalaryHeadId";
            ddlCompanyContributionHeadId.DataBind();

            ddlCompanyContributionHeadId.Items.Insert(0, itemBasicSalary);

        }
        protected void btnBonusHead_Click(object sender, EventArgs e)
        {
            if (!IsFrmBasicHeadSetupInfoValid())
            {
                return;
            }
            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(this.hfBonusHeadId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.hfBonusHeadId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "PayrollBonusHeadId";
            commonSetupBO.TypeName = "PayrollBonusHeadId";
            commonSetupBO.SetupValue = ddlBonusHeadId.SelectedValue;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Bonus Head Updated Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Bonus Head Updated Successfull.", AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.BonusHead.ToString(), tmpSetupId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "Bonus Head Updated");
                this.SetTab("BonusTab");
            }
            else
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Bonus Head Saved Successfull";
                this.btnBasicHeadSetup.Text = "Update";
                CommonHelper.AlertInfo(innboardMessage, "Bonus Head Saved Successfull.", AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.BonusHead.ToString(), tmpSetupId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "Bonus Head Updated");
                this.SetTab("BonusTab");
            }
        }
        public void LoadLeaveType()
        {
            LeaveTypeDA leaveDA = new LeaveTypeDA();
            List<LeaveTypeBO> list = new List<LeaveTypeBO>();
            list = leaveDA.GetLeaveTypeInfo();

            this.ddlInsteadLeaveHeadId.DataSource = list;
            this.ddlInsteadLeaveHeadId.DataTextField = "TypeName";
            this.ddlInsteadLeaveHeadId.DataValueField = "LeaveTypeId";
            this.ddlInsteadLeaveHeadId.DataBind();

            ListItem item1 = new ListItem();
            item1.Value = "0";
            item1.Text = hmUtility.GetDropDownFirstValue();
            this.ddlInsteadLeaveHeadId.Items.Insert(0, item1);

        }
        private void LoadBasicHeadSetupInfo()
        {

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Basic Salary Head", "Basic Salary Setup");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                txtBasicSetupId.Value = commonSetupBO.SetupId.ToString();
                this.ddlBasicSalaryHeadId.SelectedValue = commonSetupBO.SetupValue;
                this.txtBasicSetupId.Value = commonSetupBO.SetupId.ToString();
                this.btnBasicHeadSetup.Text = "Update";

            }
        }
        private void LoadBonusHeadSetupInfo()
        {

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollBonusHeadId", "PayrollBonusHeadId");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                hfBonusHeadId.Value = commonSetupBO.SetupId.ToString();
                this.ddlBonusHeadId.SelectedValue = commonSetupBO.SetupValue;
                this.btnBonusHead.Text = "Update";

            }
        }
        private bool IsFrmBasicHeadSetupInfoValid()
        {
            bool flag = true;
            if (this.ddlBasicSalaryHeadId.SelectedIndex == 0)
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "Please Select Valid Basic Salary Head";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Valid Basic Salary Head.", AlertType.Success);
                this.ddlBasicSalaryHeadId.Focus();
                flag = false;
            }
            //else
            //{
            //    this.isMessageBoxEnable = -1;
            //    this.lblMessage.Text = string.Empty;
            //}
            return flag;
        }
        protected void btnBasicHeadSetup_Click(object sender, EventArgs e)
        {
            if (!IsFrmBasicHeadSetupInfoValid())
            {
                return;
            }
            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(this.txtBasicSetupId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtBasicSetupId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "Basic Salary Setup";
            commonSetupBO.TypeName = "Basic Salary Head";
            commonSetupBO.SetupValue = ddlBasicSalaryHeadId.SelectedValue;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Salary Head Updated Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Salary Head Updated Successfull.", AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.SalaryHead.ToString(), tmpSetupId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "Salary Head Updated");
            }
            else
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Salary Head Saved Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Salary Head Saved Successfull.", AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.SalaryHead.ToString(), tmpSetupId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "Salary Head Saved");
                this.btnBasicHeadSetup.Text = "Update";
            }

        }
        protected void btnOverTimeInfoSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmOverTimeInfoValid())
            {
                return;
            }
            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(this.txtOverTimeSetupId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtOverTimeSetupId.Value);
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "Overtime Setup";
            commonSetupBO.TypeName = "Overtime";
            commonSetupBO.SetupValue = this.ddlSalaryHeadId.SelectedValue + '~' + "0";
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Overtime setup Updated Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Overtime setup Updated Successfull.", AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.OverTimeSetup.ToString(), tmpSetupId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "Overtime Setup Updated");
            }
            else
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Overtime setup saved Successfull";
                this.btnOverTimeInfoSave.Text = "Update";
                CommonHelper.AlertInfo(innboardMessage, "Overtime setup saved Successfull.", AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.OverTimeSetup.ToString(), tmpSetupId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "OverTime Setup Saved ");
            }
        }
        private void LoadMonthlyWorkingDay()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollMonthlyWorkingDay", "MonthlyWorkingDay");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                this.txtMonthlyWorkingDay.Text = commonSetupBO.SetupValue;
                this.txtMonthlyWorkingDayId.Value = commonSetupBO.SetupId.ToString();
                this.btnMonthlyWorkingDay.Text = "Update";
            }
        }
        protected void btnMonthlyWorkingDay_Click(object sender, EventArgs e)
        {

            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(this.txtMonthlyWorkingDayId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtMonthlyWorkingDayId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "MonthlyWorkingDay";
            commonSetupBO.TypeName = "PayrollMonthlyWorkingDay";
            commonSetupBO.SetupValue = txtMonthlyWorkingDay.Text;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Monthly Working Day Configuration Updated Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Monthly Working Day Configuration Updated Successfull.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.MonthlyWorkingDayConfiguration.ToString(), commonSetupBO.SetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.MonthlyWorkingDayConfiguration));

            }
            else
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Monthly Working Day Configuration Saved Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Monthly Working Day Configuration Saved Successfull.", AlertType.Success);
                this.btnBasicHeadSetup.Text = "Update";
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                              EntityTypeEnum.EntityType.MonthlyWorkingDayConfiguration.ToString(), tmpSetupId,
                              ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.MonthlyWorkingDayConfiguration));

            }

        }
        private void LoadWorkingHour()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollDailyWorkingHour", "DailyWorkingHour");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                this.txtWorkingHour.Text = commonSetupBO.SetupValue;
                this.txtWorkingHourId.Value = commonSetupBO.SetupId.ToString();
                this.btnWorkingHour.Text = "Update";
            }
        }
        protected void btnWorkingHour_Click(object sender, EventArgs e)
        {
            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(this.txtWorkingHourId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtWorkingHourId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "PayrollDailyWorkingHour";
            commonSetupBO.TypeName = "DailyWorkingHour";
            commonSetupBO.SetupValue = txtWorkingHour.Text;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Monthly Working Hour Configuration Updated Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Monthly Working Hour Configuration Updated Successfull.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.WorkingHourConfiguration.ToString(), commonSetupBO.SetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.WorkingHourConfiguration));

            }
            else
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Monthly Working Hour Configuration Saved Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Monthly Working Hour Configuration Saved Successfull.", AlertType.Success);
                this.btnWorkingHour.Text = "Update";
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                EntityTypeEnum.EntityType.WorkingHourConfiguration.ToString(), tmpSetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.WorkingHourConfiguration));

            }


        }
        private void LoadMinimumOvertimeHour()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollMinimumOvertimeHour", "MinimumOvertimeHour");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                this.txtMinimumOvertimeHour.Text = commonSetupBO.SetupValue;
                this.txtMinimumOvertimeHourId.Value = commonSetupBO.SetupId.ToString();
                this.btnMinimumOvertimeHour.Text = "Update";
            }
        }
        protected void btnMinimumOvertimeHour_Click(object sender, EventArgs e)
        {
            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(this.txtMinimumOvertimeHourId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtMinimumOvertimeHourId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "PayrollMinimumOvertimeHour";
            commonSetupBO.TypeName = "MinimumOvertimeHour";
            commonSetupBO.SetupValue = txtMinimumOvertimeHour.Text;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Minimum Overtime Hour Configuration Updated Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Minimum Overtime Hour Configuration Updated Successfull.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.MinimumOvertimeHourConfiguration.ToString(), commonSetupBO.SetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.MinimumOvertimeHourConfiguration));


            }
            else
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Minimum Overtime Hour Configuration Saved Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Minimum Overtime Hour Configuration Saved Successfull.", AlertType.Success);
                this.btnMinimumOvertimeHour.Text = "Update";
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                EntityTypeEnum.EntityType.MinimumOvertimeHourConfiguration.ToString(), tmpSetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.MinimumOvertimeHourConfiguration));

            }
        }
        private void LoadInsteadLeaveForOneHoliday()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollInsteadLeaveForOneHoliday", "InsteadLeaveForOneHoliday");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                this.txtInsteadLeaveForOneHoliday.Text = commonSetupBO.SetupValue;
                this.txtInsteadLeaveForOneHolidayId.Value = commonSetupBO.SetupId.ToString();
                this.btnInsteadLeaveForOneHoliday.Text = "Update";
            }
        }
        protected void btnInsteadLeaveForOneHoliday_Click(object sender, EventArgs e)
        {
            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(this.txtInsteadLeaveForOneHolidayId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtInsteadLeaveForOneHolidayId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "PayrollInsteadLeaveForOneHoliday";
            commonSetupBO.TypeName = "InsteadLeaveForOneHoliday";
            commonSetupBO.SetupValue = txtInsteadLeaveForOneHoliday.Text;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Instead Leave For One Holiday Configuration Updated Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Instead Leave For One Holiday Configuration Updated Successfull.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.InsteadLeaveForOneHolidayConfiguration.ToString(), commonSetupBO.SetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InsteadLeaveForOneHolidayConfiguration));


            }
            else
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Instead Leave For One Holiday Configuration Saved Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Instead Leave For One Holiday Configuration Saved Successfull.", AlertType.Success);
                this.btnInsteadLeaveForOneHoliday.Text = "Update";
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                EntityTypeEnum.EntityType.InsteadLeaveForOneHolidayConfiguration.ToString(), tmpSetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InsteadLeaveForOneHolidayConfiguration));

            }
        }
        private void LoadMonthlyWorkingDayForAbsentree()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollInsteadLeaveForOneHoliday", "InsteadLeaveForOneHoliday");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                this.txtMonthlyWorkingDayForAbsentree.Text = commonSetupBO.SetupValue;
                this.txtMonthlyWorkingDayForAbsentreeId.Value = commonSetupBO.SetupId.ToString();
                this.btnMonthlyWorkingDayForAbsentree.Text = "Update";
            }
        }
        protected void btnMonthlyWorkingDayForAbsentree_Click(object sender, EventArgs e)
        {
            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(this.txtMonthlyWorkingDayForAbsentreeId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtMonthlyWorkingDayForAbsentreeId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "PayrollInsteadLeaveForOneHoliday";
            commonSetupBO.TypeName = "InsteadLeaveForOneHoliday";
            commonSetupBO.SetupValue = txtMonthlyWorkingDayForAbsentree.Text;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Monthly Working Day For Absentree Configuration Updated Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Monthly Working Day For Absentree Configuration Updated Successfull.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.MonthlyWorkingDayForAbsentreeConfiguration.ToString(), commonSetupBO.SetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.MonthlyWorkingDayForAbsentreeConfiguration));


            }
            else
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Monthly Working Day For Absentree Configuration Saved Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Monthly Working Day For Absentree Configuration Saved Successfull.", AlertType.Success);
                this.btnMonthlyWorkingDayForAbsentree.Text = "Update";
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                EntityTypeEnum.EntityType.MonthlyWorkingDayForAbsentreeConfiguration.ToString(), tmpSetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.MonthlyWorkingDayForAbsentreeConfiguration));

            }
        }
        private void LoadInsteadLeaveHead()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollInsteadLeaveHeadId", "InsteadLeaveHeadId");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                this.ddlInsteadLeaveHeadId.SelectedValue = commonSetupBO.SetupValue;
                this.txtInsteadLeaveHeadId.Value = commonSetupBO.SetupId.ToString();
                this.btnInsteadLeaveHead.Text = "Update";
            }
        }
        protected void btnInsteadLeaveHead_Click(object sender, EventArgs e)
        {
            int tmpSetupId = 0;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            if (!string.IsNullOrEmpty(this.txtInsteadLeaveHeadId.Value))
            {
                commonSetupBO.SetupId = Int32.Parse(this.txtInsteadLeaveHeadId.Value);
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            commonSetupBO.CreatedBy = userInformationBO.CreatedBy;
            commonSetupBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            commonSetupBO.SetupName = "PayrollInsteadLeaveHeadId";
            commonSetupBO.TypeName = "InsteadLeaveHeadId";
            commonSetupBO.SetupValue = ddlInsteadLeaveHeadId.SelectedValue;
            Boolean status = commonSetupDA.SaveOrUpdateCommonConfiguration(commonSetupBO, out tmpSetupId);
            if (tmpSetupId == 0)
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Instead Leave Head Configuration Updated Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Instead Leave Head Configuration Updated Successfull.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                EntityTypeEnum.EntityType.InsteadLeaveHeadConfiguration.ToString(), commonSetupBO.SetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InsteadLeaveHeadConfiguration));


            }
            else
            {
                //this.isMessageBoxEnable = 2;
                //lblMessage.Text = "Instead Leave Head Configuration Saved Successfull";
                CommonHelper.AlertInfo(innboardMessage, "Instead Leave Head Configuration Saved Successfull.", AlertType.Success);
                this.btnInsteadLeaveHead.Text = "Update";
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                EntityTypeEnum.EntityType.InsteadLeaveHeadConfiguration.ToString(), tmpSetupId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InsteadLeaveHeadConfiguration));

            }

        }
        protected void btnEmpTaxSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsTaxFrmValid())
                {
                    return;
                }

                TaxSettingBO taxBO = new TaxSettingBO();
                EmpTaxDA taxDA = new EmpTaxDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                taxBO.TaxBandForMale = Convert.ToDecimal(this.txtTaxBandM.Text.Trim());
                taxBO.TaxBandForFemale = Convert.ToDecimal(this.txtTaxBandF.Text.Trim());
                taxBO.IsTaxPaidByCompany = this.chkIsTaxPaidbyCmp.Checked ? true : false;
                taxBO.CompanyContributionType = this.ddlCmpContType.SelectedItem.Text;
                if (!string.IsNullOrEmpty(txtCmpContAmount.Text))
                    taxBO.CompanyContributionAmount = Convert.ToDecimal(this.txtCmpContAmount.Text.Trim());
                taxBO.IsTaxDeductFromSalary = this.chkIsTaxDdctFrmSlry.Checked ? true : false;
                taxBO.EmployeeContributionType = this.ddlEmpContType.SelectedItem.Text;
                taxBO.Remarks = this.txtRemarks.Text;

                int hiddenId = Convert.ToInt32(txtTaxSettingId.Value);
                int tmpId;
                if (hiddenId > 0)
                {
                    taxBO.TaxSettingId = hiddenId;
                    taxBO.LastModifiedBy = userInformationBO.UserInfoId;
                    taxBO.LastModifiedDate = DateTime.Now;

                    Boolean status = taxDA.UpdateEmpTaxInfo(taxBO);
                    if (status)
                    {
                        //this.isMessageBoxEnable = 2;
                        //lblMessage.Text = "Update Operation Successfull.";
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.TaxSetting.ToString(), taxBO.TaxSettingId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.TaxSetting));
                    }
                }
                else
                {
                    taxBO.CreatedBy = userInformationBO.UserInfoId;
                    taxBO.CreatedDate = DateTime.Now;

                    Boolean status = taxDA.SaveEmpTaxInformation(taxBO, out tmpId);
                    if (status)
                    {
                        //this.isMessageBoxEnable = 2;
                        //lblMessage.Text = "Save Operation Successfull.";
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.TaxSetting.ToString(), tmpId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.TaxSetting));
                        LoadTax();
                        //this.btnEmpTaxSave.Text = "Update";
                    }
                }
                SetTab("TaxTab");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        protected void btnEmpTaxClear_Click(object sender, EventArgs e)
        {
            this.txtTaxBandM.Text = string.Empty;
            this.txtTaxBandF.Text = string.Empty;
            this.chkIsTaxPaidbyCmp.Checked = false;
            this.ddlCmpContType.SelectedIndex = 0;
            this.txtCmpContAmount.Text = string.Empty;
            this.chkIsTaxDdctFrmSlry.Checked = false;
            this.ddlEmpContType.SelectedIndex = 0;
            this.txtRemarks.Text = string.Empty;
            SetTab("TaxTab");
        }
        protected void btnEmpPFSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsPFFrmValid())
                {
                    return;
                }

                PFSettingBO pfBO = new PFSettingBO();
                EmpPFDA pfDA = new EmpPFDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                string pfEmployeeContId = ddlEmployeeContributionHeadId.Text;
                string pfCompanyContId = ddlCompanyContributionHeadId.Text;
                string pfCompanyContributionOn = ddlCompanyContributionOn.SelectedValue;

                pfBO.EmployeeContributionInPercentage = Convert.ToDecimal(this.txtEmpCont.Text.Trim());
                pfBO.CompanyContributionInPercentange = Convert.ToDecimal(this.txtCmpCont.Text.Trim());
                pfBO.EmployeeCanContributeMaxOfBasicSalary = Convert.ToDecimal(this.txtEmpMaxCont.Text.Trim());
                pfBO.InterestDistributionRate = Convert.ToDecimal(this.txtIntDisRt.Text.Trim());
                pfBO.CompanyContributionEligibilityYear = Convert.ToInt32(txtCmpContElegYear.Text);

                int hiddenId = Convert.ToInt32(txtPFSettingId.Value);
                int tmpId;

                if (hiddenId > 0)
                {
                    pfBO.PFSettingId = hiddenId;
                    pfBO.LastModifiedBy = userInformationBO.UserInfoId;
                    pfBO.LastModifiedDate = DateTime.Now;

                    Boolean status = pfDA.UpdateEmpPFInfo(pfBO, pfEmployeeContId, pfCompanyContId, pfCompanyContributionOn);
                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.PFSetting.ToString(), pfBO.PFSettingId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PFSetting));
                    }
                }
                else
                {
                    pfBO.CreatedBy = userInformationBO.UserInfoId;
                    pfBO.CreatedDate = DateTime.Now;

                    Boolean status = pfDA.SaveEmpPFInformation(pfBO, pfEmployeeContId, pfCompanyContId, pfCompanyContributionOn, out tmpId);
                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.PFSetting.ToString(), tmpId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PFSetting));
                        LoadPF();
                        //this.btnEmpPFSave.Text = "Update";
                    }
                }
                SetTab("PFTab");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        protected void btnEmpPFClear_Click(object sender, EventArgs e)
        {
            this.txtEmpCont.Text = string.Empty;
            this.txtCmpCont.Text = string.Empty;
            this.txtEmpMaxCont.Text = string.Empty;
            this.txtIntDisRt.Text = string.Empty;
            this.txtCmpContElegYear.Text = string.Empty;
            SetTab("PFTab");
        }
        protected void btnEmpLoanSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsLoanFrmValid())
                {
                    return;
                }

                LoanSettingBO loanBO = new LoanSettingBO();
                EmpLoanDA loanDA = new EmpLoanDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                loanBO.CompanyLoanInterestRate = Convert.ToDecimal(this.txtCmpLnIntRate.Text.Trim());
                loanBO.PFLoanInterestRate = Convert.ToDecimal(this.txtPFlnIntRate.Text.Trim());
                loanBO.MaxAmountCanWithdrawFromPFInPercentage = Convert.ToInt32(this.txtMaxAmtWdrwfmPF.Text.Trim());
                loanBO.MinPFMustAvailableToAllowLoan = Convert.ToDecimal(this.txtMinPFavlfrLn.Text.Trim());
                loanBO.MinJobLengthToAllowCompanyLoan = Convert.ToDecimal(this.txtMinJobLnthfrCmpLn.Text.Trim());
                loanBO.DurationToAllowNextLoanAfterCompletetionTakenLoan = Convert.ToInt32(this.txtDrtnfrNxtLn.Text.Trim());

                int hiddenId = Convert.ToInt32(txtLoanSettingId.Value);
                int tmpId;
                if (hiddenId > 0)
                {
                    loanBO.LoanSettingId = hiddenId;
                    loanBO.LastModifiedBy = userInformationBO.UserInfoId;
                    loanBO.LastModifiedDate = DateTime.Now;

                    Boolean status = loanDA.UpdateEmpLoanInfo(loanBO);
                    if (status)
                    {
                        //this.isMessageBoxEnable = 2;
                        //lblMessage.Text = "Update Operation Successfull.";
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.LoanSetting.ToString(), loanBO.LoanSettingId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LoanSetting));
                    }
                }
                else
                {
                    loanBO.CreatedBy = userInformationBO.UserInfoId;
                    loanBO.CreatedDate = DateTime.Now;

                    Boolean status = loanDA.SaveEmpLoanInformation(loanBO, out tmpId);
                    if (status)
                    {
                        //this.isMessageBoxEnable = 2;
                        //lblMessage.Text = "Save Operation Successfull.";
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.LoanSetting.ToString(), tmpId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LoanSetting));
                        LoadLoan();
                        //this.btnEmpLoanSave.Text = "Update";
                    }
                }
                SetTab("LoanTab");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        protected void btnEmpLoanClear_Click(object sender, EventArgs e)
        {
            this.txtCmpLnIntRate.Text = string.Empty;
            this.txtPFlnIntRate.Text = string.Empty;
            this.txtMaxAmtWdrwfmPF.Text = string.Empty;
            this.txtMinPFavlfrLn.Text = string.Empty;
            this.txtMinJobLnthfrCmpLn.Text = string.Empty;
            this.txtDrtnfrNxtLn.Text = string.Empty;
            SetTab("LoanTab");
        }
        protected void btnEmpGratutitySave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsGratuityFrmValid())
                {
                    return;
                }

                GratutitySettingsBO gratuityBO = new GratutitySettingsBO();
                EmpGratuityDA gratuityDA = new EmpGratuityDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                gratuityBO.GratuityWillAffectAfterJobLengthInYear = Convert.ToInt32(this.txtNoofJobYearfrGrty.Text.Trim());
                gratuityBO.IsGratuityBasedOnBasic = this.chkIsGrtybsdonBasic.Checked ? true : false;
                gratuityBO.GratutiyPercentage = Convert.ToDecimal(this.txtGrtyPercntge.Text.Trim());
                gratuityBO.NumberOfGratuityAdded = Convert.ToInt32(this.txtGrtyNoAdded.Text.Trim());

                int hiddenId = Convert.ToInt32(txtGratuityId.Value);
                int tmpId;
                if (hiddenId > 0)
                {
                    gratuityBO.GratuityId = hiddenId;
                    gratuityBO.LastModifiedBy = userInformationBO.UserInfoId;
                    gratuityBO.LastModifiedDate = DateTime.Now;

                    Boolean status = gratuityDA.UpdateEmpGratuityInfo(gratuityBO);
                    if (status)
                    {
                        //this.isMessageBoxEnable = 2;
                        //lblMessage.Text = "Update Operation Successfull.";
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.GratutitySettings.ToString(), gratuityBO.GratuityId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GratutitySettings));
                    }
                }
                else
                {
                    gratuityBO.CreatedBy = userInformationBO.UserInfoId;
                    gratuityBO.CreatedDate = DateTime.Now;

                    Boolean status = gratuityDA.SaveEmpGratuityInfo(gratuityBO, out tmpId);
                    if (status)
                    {
                        //this.isMessageBoxEnable = 2;
                        //lblMessage.Text = "Save Operation Successfull.";
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.GratutitySettings.ToString(), tmpId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GratutitySettings));
                        LoadGratuity();
                        //this.btnEmpLoanSave.Text = "Update";
                    }
                }
                SetTab("GratuityTab");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        protected void btnEmpGratutityClear_Click(object sender, EventArgs e)
        {
            this.txtNoofJobYearfrGrty.Text = string.Empty;
            this.chkIsGrtybsdonBasic.Checked = false;
            this.txtGrtyPercntge.Text = string.Empty;
            this.txtGrtyNoAdded.Text = string.Empty;
            SetTab("GratuityTab");
        }
        protected void btnEmpBonusSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<EmpBonusBO> addList = new List<EmpBonusBO>();
                List<EmpBonusBO> deleteList = new List<EmpBonusBO>();

                addList = JsonConvert.DeserializeObject<List<EmpBonusBO>>(hfSaveObj.Value);
                deleteList = JsonConvert.DeserializeObject<List<EmpBonusBO>>(hfDeleteObj.Value);

                if (deleteList.Count == 0)
                {
                    if (!IsBonusFrmValid())
                    {
                        return;
                    }
                }

                EmpBonusBO bonusBO = new EmpBonusBO();
                EmpBonusDA bonusDA = new EmpBonusDA();
                int tmpId;

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                bonusBO.BonusType = ddlBonusType.SelectedValue;
                if (bonusBO.BonusType == "PeriodicBonus")
                {
                    bonusBO.BonusAmount = Convert.ToDecimal(txtBonusAmount.Text);
                    bonusBO.AmountType = ddlAmountType.SelectedValue;
                    bonusBO.DependsOnHead = Convert.ToInt32(ddlDependsOn.SelectedValue);
                    bonusBO.EffectivePeriod = Convert.ToByte(ddlBonusMonthlyPeriod.SelectedValue);
                    bonusBO.BonusDate = null;
                }


                for (int i = 0; i < addList.Count; i++)
                {
                    addList[i].EffectivePeriod = null;
                    addList[i].CreatedBy = userInformationBO.UserInfoId;
                }

                if (bonusBO.BonusType == "FestivalBonus")
                {
                    if (addList.Count == 0 && deleteList.Count == 0)
                    {
                        //if (deleteList.Count == 0)
                        //{
                            CommonHelper.AlertInfo(innboardMessage, "Please add or delete at least one Bonus.", AlertType.Warning);
                            ddlBonusType.SelectedIndex = 0;
                            return;
                        //}
                    }
                    if (addList.Count > 0 || deleteList.Count > 0)
                    {
                        Boolean status = bonusDA.SaveOrDeleteEmpFestivalBonusInfo(deleteList, addList);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Operation Successfull.", AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.EmpBonus.ToString(), 0,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "Employee Festival Bonus Info Changed");
                        }
                    }
                    //else
                    //{
                    //    Boolean status = bonusDA.UpdateEmpFestivalBonusInfo(addList);
                    //    if (status)
                    //    {
                    //        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    //    }
                    //}
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(hfBonusId.Value))
                    {
                        bonusBO.BonusSettingId = Convert.ToInt32(hfBonusId.Value);
                        bonusBO.LastModifiedBy = userInformationBO.UserInfoId;
                        //bonusBO.LastModifiedDate = DateTime.Now;

                        Boolean status = bonusDA.UpdateEmpPeriodicBonusInfo(bonusBO);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.EmpBonus.ToString(), bonusBO.BonusSettingId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpBonus));
                        }
                    }
                    else
                    {
                        bonusBO.CreatedBy = userInformationBO.UserInfoId;
                        //bonusBO.CreatedDate = DateTime.Now;

                        Boolean status = bonusDA.SaveEmpPeriodicBonusInfo(bonusBO, out tmpId);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.EmpBonus.ToString(), tmpId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpBonus));
                        }
                    }
                }
                LoadBonus();
                SetTab("BonusTab");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        protected void btnEmpBonusClear_Click(object sender, EventArgs e)
        {
            ddlBonusType.SelectedIndex = 0;
            txtBonusAmount.Text = string.Empty;
            ddlAmountType.SelectedIndex = 0;
            ddlDependsOn.SelectedIndex = 0;
            txtBonusDate.Text = string.Empty;
            ddlBonusMonthlyPeriod.SelectedIndex = 7;
        }
        protected void btnAttendanceDeviceUpdate_Click(object sender, EventArgs e)
        {            
            try
            {
                if (!IsAttendanceDeviceFrmValid())
                {
                    return;
                }

                AttendanceDeviceBO attendanceDeviceBO = new AttendanceDeviceBO();
                AttendanceDeviceDA attendanceDeviceDA = new AttendanceDeviceDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                attendanceDeviceBO.ReaderId = this.ddlAttendanceDevice.SelectedValue;
                attendanceDeviceBO.ReaderType = this.ddlDeviceType.SelectedValue;
                attendanceDeviceBO.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);

                Boolean status = attendanceDeviceDA.UpdateAttendanceDeviceInfo(attendanceDeviceBO);
                if (status)
                {
                    long deviceId = long.Parse(attendanceDeviceBO.ReaderId);
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.AttendanceDevice.ToString(), deviceId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.AttendanceDevice));
                }
                SetTab("AttendanceDevice");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
            
        }
        private bool IsLoanFrmValid()
        {
            bool flag = true;
            int checkNumber;

            if (txtCmpLnIntRate.Text == "")
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "Please Provide Company Loan Interest Rate";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Company Loan Interest Rate.", AlertType.Warning);
                flag = false;
                txtCmpLnIntRate.Focus();
            }
            else if (int.TryParse(txtCmpLnIntRate.Text, out checkNumber) == false)
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtCmpLnIntRate.Focus();
            }
            else if (txtPFlnIntRate.Text == "")
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "Please Provide Provident Fund Loan Interest Rate";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Provident Fund Loan Interest Rate.", AlertType.Warning);
                flag = false;
                txtPFlnIntRate.Focus();
            }
            else if (int.TryParse(txtPFlnIntRate.Text, out checkNumber) == false)
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtPFlnIntRate.Focus();
            }
            else if (txtMaxAmtWdrwfmPF.Text == "")
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "Please Provide Max Amount Can Withdraw From Provident Fund";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Max Amount Can Withdraw From Provident Fund.", AlertType.Warning);
                flag = false;
                txtMaxAmtWdrwfmPF.Focus();
            }
            else if (int.TryParse(txtMaxAmtWdrwfmPF.Text, out checkNumber) == false)
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtMaxAmtWdrwfmPF.Focus();
            }
            else if (txtMinPFavlfrLn.Text == "")
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "Please Provide Min Provident Fund Must Available To Allow Loan";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Min Provident Fund Must Available To Allow Loan.", AlertType.Warning);
                flag = false;
                txtMinPFavlfrLn.Focus();
            }
            else if (int.TryParse(txtMinPFavlfrLn.Text, out checkNumber) == false)
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtMinPFavlfrLn.Focus();
            }
            else if (txtMinJobLnthfrCmpLn.Text == "")
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "Please Provide Min Job Length To Allow Company Loan";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Min Job Length To Allow Company Loan.", AlertType.Warning);
                flag = false;
                txtMinJobLnthfrCmpLn.Focus();
            }
            else if (int.TryParse(txtMinJobLnthfrCmpLn.Text, out checkNumber) == false)
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtMinJobLnthfrCmpLn.Focus();
            }
            else if (txtDrtnfrNxtLn.Text == "")
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "Please Provide Duration For Next Loan After Completion Taken Loan";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Duration For Next Loan After Completion Taken Loan.", AlertType.Warning);
                flag = false;
                txtDrtnfrNxtLn.Focus();
            }
            else if (int.TryParse(txtDrtnfrNxtLn.Text, out checkNumber) == false)
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtDrtnfrNxtLn.Focus();
            }
            SetTab("LoanTab");
            return flag;
        }
        private bool IsPFFrmValid()
        {
            bool flag = true;
            decimal checkNumber;
            int checkNumberYear;

            if (txtEmpCont.Text == "")
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Please Provide Employee Contribution.", AlertType.Warning);
                flag = false;
                txtEmpCont.Focus();
            }
            else if (decimal.TryParse(txtEmpCont.Text, out checkNumber) == false)
            {                
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtEmpCont.Focus();
            }
            else if (txtCmpCont.Text == "")
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Company Contribution.", AlertType.Warning);
                flag = false;
                txtCmpCont.Focus();
            }
            else if (decimal.TryParse(txtCmpCont.Text, out checkNumber) == false)
            {                
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtCmpCont.Focus();
            }
            else if (txtEmpMaxCont.Text == "")
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Employee Can Contribute Max Of Basic Salary.", AlertType.Warning);
                flag = false;
                txtEmpMaxCont.Focus();
            }
            else if (decimal.TryParse(txtEmpMaxCont.Text, out checkNumber) == false)
            {                
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtEmpMaxCont.Focus();
            }
            else if (decimal.TryParse(txtIntDisRt.Text, out checkNumber) == false)
            {                
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtIntDisRt.Focus();
            }
            else if (string.IsNullOrEmpty(txtCmpContElegYear.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Company Contribution Eligibility Year.", AlertType.Warning);
                flag = false;
                txtCmpContElegYear.Focus();
            }
            else if (int.TryParse(txtCmpContElegYear.Text, out checkNumberYear) == false)
            {
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtCmpContElegYear.Focus();
            }
            SetTab("PFTab");
            return flag;
        }
        private bool IsTaxFrmValid()
        {
            bool flag = true;
            int checkNumber;

            if (txtTaxBandM.Text == "")
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "Please Provide Tax Band for Male";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Tax Band for Male.", AlertType.Warning);
                flag = false;
                txtTaxBandM.Focus();
            }
            else if (int.TryParse(txtTaxBandM.Text, out checkNumber) == false)
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtTaxBandM.Focus();
            }
            else if (txtTaxBandF.Text == "")
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "Please Provide Tax Band for Female";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Tax Band for Female.", AlertType.Warning);
                flag = false;
                txtTaxBandF.Focus();
            }
            else if (int.TryParse(txtTaxBandF.Text, out checkNumber) == false)
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtTaxBandF.Focus();
            }
            //else if (int.TryParse(txtCmpContAmount.Text, out checkNumber) == false)
            //{
            //    this.isMessageBoxEnable = 1;
            //    this.lblMessage.Text = "You have not entered a valid number";
            //    flag = false;
            //    txtCmpContAmount.Focus();
            //}
            SetTab("TaxTab");
            return flag;
        }
        private bool IsGratuityFrmValid()
        {
            bool flag = true;
            int checkNumber;

            if (txtNoofJobYearfrGrty.Text == "")
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "Please Provide No of Job Year For Gratutity";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "No of Job Year For Gratutity.", AlertType.Warning);
                flag = false;
                txtNoofJobYearfrGrty.Focus();
            }
            else if (int.TryParse(txtNoofJobYearfrGrty.Text, out checkNumber) == false)
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtNoofJobYearfrGrty.Focus();
            }
            else if (txtGrtyNoAdded.Text == "")
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "Please Provide No. of Gratutity Added";
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "No. of Gratutity Added.", AlertType.Warning);
                flag = false;
                txtGrtyNoAdded.Focus();
            }
            else if (int.TryParse(txtGrtyNoAdded.Text, out checkNumber) == false)
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtGrtyNoAdded.Focus();
            }
            else if (int.TryParse(txtGrtyPercntge.Text, out checkNumber) == false)
            {
                //this.isMessageBoxEnable = 1;
                //this.lblMessage.Text = "You have not entered a valid number";
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning);
                flag = false;
                txtGrtyPercntge.Focus();
            }
            SetTab("GratuityTab");
            return flag;
        }
        private bool IsBonusFrmValid()
        {
            bool flag = true;
            if (ddlBonusType.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Bonus Type.", AlertType.Warning);
                ddlBonusType.SelectedIndex = 0;
                flag = false;
            }
            else if (string.IsNullOrWhiteSpace(txtBonusAmount.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Bonus Amount.", AlertType.Warning);
                ddlBonusType.SelectedIndex = 0;
                flag = false;
            }
            else if (ddlAmountType.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Amount Type.", AlertType.Warning);
                ddlBonusType.SelectedIndex = 0;
                flag = false;
            }
            //else if (string.IsNullOrWhiteSpace(txtBonusDate.Text))
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Bonus Date.", AlertType.Warning);
            //    flag = false;
            //}
            if (ddlAmountType.SelectedValue == "Percent(%)")
            {
                if (txtBonusAmount.Text.Length > 3)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Percentage cann't be more than 100%.", AlertType.Warning);
                    flag = false;
                }
            }
            SetTab("BonusTab");
            return flag;
        }
        private bool IsAttendanceDeviceFrmValid()
        {
            bool flag = true;
            if (ddlAttendanceDevice.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Attendance Device.", AlertType.Warning);
                ddlAttendanceDevice.SelectedIndex = 0;
                flag = false;
            }
            else if (ddlDeviceType.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Device Type.", AlertType.Warning);
                ddlDeviceType.SelectedIndex = 0;
                flag = false;
            }
            SetTab("AttendanceDevice");
            return flag;
        }
        private void LoadLoan()
        {
            LoanSettingBO loanBO = new LoanSettingBO();
            EmpLoanDA loanDA = new EmpLoanDA();
            loanBO = loanDA.GetEmpLoanInformation();

            this.txtCmpLnIntRate.Text = loanBO.CompanyLoanInterestRate.ToString();
            this.txtPFlnIntRate.Text = loanBO.PFLoanInterestRate.ToString();
            this.txtMaxAmtWdrwfmPF.Text = loanBO.MaxAmountCanWithdrawFromPFInPercentage.ToString();
            this.txtMinPFavlfrLn.Text = loanBO.MinPFMustAvailableToAllowLoan.ToString();
            this.txtMinJobLnthfrCmpLn.Text = loanBO.MinJobLengthToAllowCompanyLoan.ToString();
            this.txtDrtnfrNxtLn.Text = loanBO.DurationToAllowNextLoanAfterCompletetionTakenLoan.ToString();
            this.txtLoanSettingId.Value = loanBO.LoanSettingId.ToString();

            int hiddenId = Convert.ToInt32(txtLoanSettingId.Value);
            if (hiddenId > 0)
            {
                this.btnEmpLoanSave.Text = "Update";
            }

        }
        private void LoadPF()
        {
            PFSettingBO pfBO = new PFSettingBO();
            EmpPFDA pfDA = new EmpPFDA();
            pfBO = pfDA.GetEmpPFInformation();

            this.txtEmpCont.Text = pfBO.EmployeeContributionInPercentage.ToString();
            this.txtCmpCont.Text = pfBO.CompanyContributionInPercentange.ToString();
            this.txtEmpMaxCont.Text = pfBO.EmployeeCanContributeMaxOfBasicSalary.ToString();
            this.txtIntDisRt.Text = pfBO.InterestDistributionRate.ToString();
            this.txtPFSettingId.Value = pfBO.PFSettingId.ToString();
            this.txtCmpContElegYear.Text = pfBO.CompanyContributionEligibilityYear.ToString();

            HMCommonSetupBO commonSetupEmployeeContributionBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupEmployeeContributionBO = commonSetupDA.GetCommonConfigurationInfo("PayrollPFEmployeeContributionId", "PayrollPFEmployeeContributionId");
            if (!string.IsNullOrEmpty(commonSetupEmployeeContributionBO.SetupValue))
            {
                ddlEmployeeContributionHeadId.SelectedValue = commonSetupEmployeeContributionBO.SetupValue;
            }

            HMCommonSetupBO commonSetupCompanyContributionBO = new HMCommonSetupBO();
            commonSetupCompanyContributionBO = commonSetupDA.GetCommonConfigurationInfo("PayrollPFCompanyContributionId", "PayrollPFCompanyContributionId");
            if (!string.IsNullOrEmpty(commonSetupCompanyContributionBO.SetupValue))
            {
                ddlCompanyContributionHeadId.SelectedValue = commonSetupCompanyContributionBO.SetupValue;
            }

            HMCommonSetupBO commonSetupPFCompanyContributionOnBO = new HMCommonSetupBO();
            commonSetupPFCompanyContributionOnBO = commonSetupDA.GetCommonConfigurationInfo("PFCompanyContributionOn", "PFCompanyContributionOn");
            if (!string.IsNullOrEmpty(commonSetupPFCompanyContributionOnBO.SetupValue))
            {
                ddlCompanyContributionOn.SelectedValue = commonSetupPFCompanyContributionOnBO.SetupValue;
            }

            int hiddenId = Convert.ToInt32(txtPFSettingId.Value);
            if (hiddenId > 0)
            {
                this.btnEmpPFSave.Text = "Update";
            }
        }
        private void LoadTax()
        {
            TaxSettingBO taxBO = new TaxSettingBO();
            EmpTaxDA taxDA = new EmpTaxDA();
            taxBO = taxDA.GetEmpTaxInformation();

            this.txtTaxBandM.Text = taxBO.TaxBandForMale.ToString();
            this.txtTaxBandF.Text = taxBO.TaxBandForFemale.ToString();
            if (taxBO.IsTaxPaidByCompany.Equals(true))
            {
                this.chkIsTaxPaidbyCmp.Checked = true;
            }
            if (!String.IsNullOrWhiteSpace(taxBO.CompanyContributionType))
            {
                this.ddlCmpContType.SelectedItem.Text = taxBO.CompanyContributionType.ToString();
            }
            this.txtCmpContAmount.Text = taxBO.CompanyContributionAmount.ToString();
            if (taxBO.IsTaxDeductFromSalary.Equals(true))
            {
                this.chkIsTaxDdctFrmSlry.Checked = true;
            }
            if (!string.IsNullOrWhiteSpace(taxBO.EmployeeContributionType))
            {
                this.ddlEmpContType.SelectedItem.Text = taxBO.EmployeeContributionType.ToString();
            }
            if (!string.IsNullOrWhiteSpace(taxBO.Remarks))
            {
                this.txtRemarks.Text = taxBO.Remarks.ToString();
            }
            this.txtTaxSettingId.Value = taxBO.TaxSettingId.ToString();

            int hiddenId = Convert.ToInt32(txtTaxSettingId.Value);
            if (hiddenId > 0)
            {
                this.btnEmpTaxSave.Text = "Update";
            }
        }
        private void LoadGratuity()
        {
            GratutitySettingsBO gratuityBO = new GratutitySettingsBO();
            EmpGratuityDA gratuityDA = new EmpGratuityDA();
            gratuityBO = gratuityDA.GetEmpGratuityInfo();

            this.txtNoofJobYearfrGrty.Text = gratuityBO.GratuityWillAffectAfterJobLengthInYear.ToString();
            if (gratuityBO.IsGratuityBasedOnBasic.Equals(true))
            {
                this.chkIsGrtybsdonBasic.Checked = true;
            }
            this.txtGrtyPercntge.Text = gratuityBO.GratutiyPercentage.ToString();
            this.txtGrtyNoAdded.Text = gratuityBO.NumberOfGratuityAdded.ToString();
            this.txtGratuityId.Value = gratuityBO.GratuityId.ToString();

            int hiddenId = Convert.ToInt32(txtGratuityId.Value);
            if (hiddenId > 0)
            {
                this.btnEmpGratutitySave.Text = "Update";
            }
        }
        private void LoadBonus()
        {
            List<EmpBonusBO> perBonusList = new List<EmpBonusBO>();
            List<EmpBonusBO> fesBonusList = new List<EmpBonusBO>();
            EmpBonusDA bonusDA = new EmpBonusDA();
            perBonusList = bonusDA.GetBonusList("PeriodicBonus");
            fesBonusList = bonusDA.GetBonusList("FestivalBonus");

            if (perBonusList.Count != 0)
            {
                hfBonusId.Value = perBonusList[0].BonusSettingId.ToString();
                ddlBonusType.Text = perBonusList[0].BonusType.ToString();
                txtBonusAmount.Text = perBonusList[0].BonusAmount.ToString();
                ddlAmountType.Text = perBonusList[0].AmountType.ToString();
                ddlDependsOn.Text = perBonusList[0].DependsOnHead.ToString();
            }
            if (fesBonusList.Count != 0)
            {
                ltlTableWiseBonusAdd.InnerHtml = GenerateBonusTable(fesBonusList);
                //txtBonusAmount.Text = fesBonusList[0].BonusAmount.ToString();
                //txtBonusDate.Text = fesBonusList[0].ViewBonusDate.ToString();
                //ddlAmountType.SelectedValue = fesBonusList[0].AmountType.ToString();
                //ddlDependsOn.SelectedValue = fesBonusList[0].DependsOnHead.ToString();
            }
        }
        public void LoadAmountType()
        {
            SalaryFormulaDA salaryFormulaDA = new SalaryFormulaDA();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = salaryFormulaDA.GetCustomFields("AmountType", hmUtility.GetDropDownFirstValue());
            this.ddlAmountType.DataSource = fields;
            this.ddlAmountType.DataTextField = "FieldValue";
            this.ddlAmountType.DataValueField = "FieldValue";
            this.ddlAmountType.DataBind();
        }       
        public void GetDependsOnHeadList()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("Basic Salary Head", "Basic Salary Setup");

            SalaryHeadDA salaryHeadDA = new SalaryHeadDA();
            var entity = salaryHeadDA.GetSalaryHeadInfoById(Int32.Parse(commonSetupBO.SetupValue));

            List<ItemViewBO> List = new List<ItemViewBO>();
            ItemViewBO itemViewBO = new ItemViewBO();
            itemViewBO.ItemId = entity.SalaryHeadId;
            itemViewBO.ItemName = entity.SalaryHead;
            List.Add(itemViewBO);

            ddlDependsOn.DataSource = List;
            ddlDependsOn.DataValueField = "ItemId";
            ddlDependsOn.DataTextField = "ItemName";
            ddlDependsOn.DataBind();
        }
        private void SetTab(string TabName)
        {
            if (TabName == "SettingsTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "TaxTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");

            }
            else if (TabName == "PFTab")
            {
                C.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "LoanTab")
            {
                D.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "GratuityTab")
            {
                E.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "BonusTab")
            {
                F.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                G.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "AttendanceDevice")
            {
                G.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
                D.Attributes.Add("class", "ui-state-default ui-corner-top");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                E.Attributes.Add("class", "ui-state-default ui-corner-top");
                F.Attributes.Add("class", "ui-state-default ui-corner-top");
            }

        }
        public string GenerateBonusTable(List<EmpBonusBO> bonusList)
        {
            string strTable = "";
            var deleteLink = "";

            strTable += "<table cellspacing='0' cellpadding='4' id='BonusInformation' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='display:none'>Bonus Id</th><th style='display:none'>Bonus Type</th><th style='display:none'>Depends On</th><th align='left' scope='col' style='width: 30%;'>Bonus Amount</th><th align='left' scope='col' style='width: 30%;'>Amount Type</th><th align='left' scope='col' style='width: 30%;'>Bonus Date</th><th align='center' scope='col' style='width: 10%;'>Action</th></tr></thead>";
            strTable += "<tbody>";
            int counter = 0;
            if (bonusList != null)
            {
                foreach (EmpBonusBO dr in bonusList)
                {
                    deleteLink = "<a href=\"#\" onclick= 'DeleteBonus(this)' ><img alt=\"Edit\" src=\"../Images/delete.png\" /></a>";
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

                    strTable += "<td align='left' style=\"display:none;\">" + dr.BonusSettingId + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.BonusType + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.DependsOnHead + "</td>";
                    strTable += "<td align='left' style=\"width:30%; text-align:Left;\">" + dr.BonusAmount + "</td>";
                    strTable += "<td align='left' style=\"width:30%; text-align:Left;\">" + dr.AmountType + "</td>";
                    strTable += "<td align='left' style=\"width:30%; text-align:Left;\">" + dr.ViewBonusDate + "</td>";

                    strTable += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";
                    strTable += "</tr>";

                }
            }
            strTable += "</tbody>";
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }

            return strTable;
        }
        protected void ddlAttendanceDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlAttendanceDevice.SelectedValue) > 0)
            {
                AttendanceDeviceBO attendanceDeviceBO = new AttendanceDeviceBO();
                AttendanceDeviceDA attendanceDeviceDA = new AttendanceDeviceDA();
                attendanceDeviceBO = attendanceDeviceDA.GetAttendanceDeviceInfoById(ddlAttendanceDevice.SelectedValue);
                if (attendanceDeviceBO != null)
                {
                    if (!attendanceDeviceBO.ReaderId.Equals(""))
                    {
                        this.ddlDeviceType.SelectedValue = attendanceDeviceBO.ReaderType;
                        SetTab("AttendanceDevice");
                    }
                }
            }
        }
        private void LoadCompanyContributionOn()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> featureList = new List<CustomFieldBO>();
            featureList = commonDA.GetCustomField("CompanyContributionOn");

            ddlCompanyContributionOn.DataSource = featureList;
            ddlCompanyContributionOn.DataTextField = "Description";
            ddlCompanyContributionOn.DataValueField = "FieldValue";
            ddlCompanyContributionOn.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCompanyContributionOn.Items.Insert(0, item);
        }
    }
}
