using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmWorkingPlan : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();        
        protected int isFixedWorkingPlanInfoDivEnable = 1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");            

            if (!IsPostBack)
            {
                LoadDayOne();
                LoadDayTwo();
                LoadEmployeeCategory();
                LoadSerchDayTwo();
                LoadSearchDayOne();
                LoadSearchEmployeeCategory();
                SetDefaulTime();
                CheckPermission();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
        }
        protected void gvWorkingDay_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {            
            this.gvWorkingDay.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void gvWorkingDay_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;

            }
        }
        protected void gvWorkingDay_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int workingDaysId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(workingDaysId);
                btnSave.Visible = isUpdatePermission;
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("PayrollWorkingDay", "WorkingDayId", workingDaysId);
                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                           EntityTypeEnum.EntityType.WorkingPlan.ToString(), workingDaysId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.WorkingPlan));
                    }
                }
                catch (Exception ex)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    
                }
                LoadGridView();
                this.SetTab("SearchTab");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isFormValid())
                {
                    return;
                }
                PayrollWorkingDayBO bo = new PayrollWorkingDayBO();
                PayrollWorkingDayBO Prevbo = new PayrollWorkingDayBO();
                PayrollWorkingDayDA da = new PayrollWorkingDayDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                bo.TypeId = Int32.Parse(ddlCategoryId.SelectedValue);
                bo.WorkingPlan = ddlWorkingPlan.SelectedValue;
                if (bo.WorkingPlan == "Fixed")
                {
                    bo.DayOffOne = ddlDayOffOne.SelectedValue;
                    bo.DayOffTwo = ddlDayOffTwo.SelectedValue;

                    DateTime thisDay = DateTime.Today;
                    DateTime DateIn = new DateTime();

                    string thisDayString = hmUtility.GetStringFromDateTime(thisDay);
                    DateIn = hmUtility.GetDateTimeFromString(thisDayString, userInformationBO.ServerDateFormat);
                    //int pMin = !string.IsNullOrWhiteSpace(this.txtStartMinute.Text) ? Convert.ToInt32(this.txtStartMinute.Text) : 0;
                    //int pHour = this.ddlStartAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtStartHour.Text) % 12) : ((Convert.ToInt32(this.txtStartHour.Text) % 12) + 12);
                    //bo.StartTime = DateIn.AddHours(pHour).AddMinutes(pMin);
                    //string startTime = (txtStartHour.Text.Replace("AM", "")).Replace("PM", "");
                    //string[] STime = startTime.Split(':');
                    //int pIHour = Convert.ToInt32(STime[0]);
                    //int pIMin = Convert.ToInt32(STime[1]);
                    //bo.StartTime = DateIn.AddHours(pIHour).AddMinutes(pIMin);

                    if (!String.IsNullOrEmpty(txtStartHour.Text))
                    {
                        bo.StartTime = Convert.ToDateTime((DateIn.Date.ToString("MM/dd/yyyy") + " " + txtStartHour.Text));
                    }

                    //int pEMin = !string.IsNullOrWhiteSpace(this.txtEndMinute.Text) ? Convert.ToInt32(this.txtEndMinute.Text) : 0;
                    //int pEHour = this.ddlEndAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtEndHour.Text) % 12) : ((Convert.ToInt32(this.txtEndHour.Text) % 12) + 12);
                    //bo.EndTime = DateIn.AddHours(pEHour).AddMinutes(pEMin);
                    //string endTime = (txtEndHour.Text.Replace("AM", "")).Replace("PM", "");
                    //string[] ETime = endTime.Split(':');
                    //int pFHour = Convert.ToInt32(ETime[0]);
                    //int pFMin = Convert.ToInt32(ETime[1]);
                    //bo.EndTime = DateIn.AddHours(pFHour).AddMinutes(pFMin);
                    if (!String.IsNullOrEmpty(txtEndHour.Text))
                    {
                        bo.EndTime = Convert.ToDateTime((DateIn.Date.ToString("MM/dd/yyyy") + " " + txtEndHour.Text));
                    }
                }
                else
                {
                    bo.DayOffOne = null;
                    bo.DayOffTwo = null;
                    //bo.StartTime = null;
                    //bo.EndTime = null;
                    bo.StartTime = DateTime.Now;
                    bo.EndTime = DateTime.Now;
                }

                if (string.IsNullOrWhiteSpace(txtWorkingDayId.Value))
                {

                    int count = da.GetCategoryIdCount(bo.TypeId);
                    if (count == 0)
                    {
                        int tmpPKId = 0;
                        bo.CreatedBy = userInformationBO.UserInfoId;
                        Boolean status = da.SavePayrollWorkingDayInfo(bo, out tmpPKId);
                        if (status)
                        {                            
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            this.Cancel();
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                EntityTypeEnum.EntityType.WorkingPlan.ToString(), tmpPKId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), 
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.WorkingPlan));

                        }
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Alrady Exists Plan for this Employee Type.", AlertType.Warning);
                        this.ddlCategoryId.Focus();
                        return;
                        //int workingId = -1;
                        //int tmpPKId = 0;

                        //bo.CreatedBy = userInformationBO.UserInfoId;
                        //bo.LastModifiedBy = userInformationBO.UserInfoId;
                        //Boolean status = da.UpdatePayrollWorkingDayInfoByCategoryId(bo, out workingId);
                        //if (status)
                        //{                            
                        //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        //    this.Cancel();
                        //    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.WorkingPlan.ToString(), workingId,
                        //    ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.WorkingPlan));

                        //}
                    }

                }
                else
                {
                    bo.WorkingDayId = Convert.ToInt32(txtWorkingDayId.Value);
                    bo.LastModifiedBy = userInformationBO.UserInfoId;

                    bool isUpdate = da.IsUpdateAvailable(bo.TypeId, bo.WorkingDayId);
                    if (isUpdate == true)
                    {

                        Boolean status = da.UpdatePayrollWorkingDayInfoByDayId(bo);
                        if (status)
                        {                            
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            this.Cancel();
                        }
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.WorkingPlan.ToString(), bo.WorkingDayId,
                        ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.WorkingPlan));
                    }
                    else
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, "Duplicate Employee Category.", AlertType.Warning);
                        this.ddlCategoryId.Focus();
                        return;
                    }
                }
                this.SetTab("EntryTab");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        //************************ User Defined Function ********************//
        private bool isFormValid()
        {
            bool status = true;
            if (ddlCategoryId.SelectedValue == "0")
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Employee Category.", AlertType.Warning);
                this.ddlCategoryId.Focus();
                status = false;
            }
            else if (String.IsNullOrEmpty(txtStartHour.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Start Time.", AlertType.Warning);
                txtStartHour.Focus();
                status = false;
            }
            else if (String.IsNullOrEmpty(txtEndHour.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "End Time.", AlertType.Warning);
                txtEndHour.Focus();
                status = false;
            }
            //else if (ddlDayOffOne.SelectedIndex == 0)
            //{
            //    this.isMessageBoxEnable = 1;
            //    lblMessage.Text = "Please Select Day Off One.";
            //    this.ddlCategoryId.Focus();
            //    status = false;
            //}
            return status;
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmWorkingPlan.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;

        }
        private void LoadGridView()
        {
            this.CheckObjectPermission();

            int categoryId = Int32.Parse(ddlSCategory.SelectedValue);
            string dayOne = ddlSDayOffOne.SelectedValue;
            string dayTwo = ddSDayOffTwo.SelectedValue;

            PayrollWorkingDayDA da = new PayrollWorkingDayDA();
            List<PayrollWorkingDayBO> files = da.GetPayrollWorkingDayInformationBySearchCritaria(categoryId, dayOne, dayTwo);
            this.gvWorkingDay.DataSource = files;
            this.gvWorkingDay.DataBind();
            this.SetTab("SearchTab");
        }
        private void Cancel()
        {
            this.txtWorkingDayId.Value = string.Empty;
            ddlWorkingPlan.SelectedIndex = 0;
            this.ddlCategoryId.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            ddlDayOffOne.SelectedIndex = 0;
            ddlDayOffTwo.SelectedIndex = 0;
            //ddlEndAMPM.SelectedIndex = 0;
            //ddlStartAMPM.SelectedIndex = 0;
            txtEndHour.Text = "06:00";
            //txtEndMinute.Text = "00";
            //txtStartMinute.Text = "00";
            txtStartHour.Text = "10:00";
            btnSave.Visible = isSavePermission;
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
        public void FillForm(int EditId)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            PayrollWorkingDayBO bo = new PayrollWorkingDayBO();
            PayrollWorkingDayDA da = new PayrollWorkingDayDA();
            bo = da.GetPayrollWorkingDayInfoByID(EditId);

            txtWorkingDayId.Value = bo.WorkingDayId.ToString();
            ddlWorkingPlan.SelectedValue = bo.WorkingPlan;
            if (bo.WorkingPlan != "Fixed")
            {
                isFixedWorkingPlanInfoDivEnable = -1;
            }

            ddlCategoryId.SelectedValue = bo.TypeId.ToString();
            if (!string.IsNullOrWhiteSpace(bo.DayOffOne))
            {
                ddlDayOffOne.SelectedValue = bo.DayOffOne;
            }
            else
            {
                ddlDayOffOne.SelectedIndex = 0;
            }

            if (!string.IsNullOrWhiteSpace(bo.DayOffTwo))
            {
                ddlDayOffTwo.SelectedValue = bo.DayOffTwo;
            }
            else
            {
                ddlDayOffTwo.SelectedIndex = 0;
            }

            if (bo.StartTime != null)
            {
                txtStartHour.Text = bo.StartTime.ToString(userInformationBO.TimeFormat);
                //this.txtStartHour.Text = Convert.ToInt32(Convert.ToDateTime(bo.StartTime).ToString("%h")) == 0 ? "12" : Convert.ToDateTime(bo.StartTime).ToString("%h");
                //this.txtStartMinute.Text = Convert.ToDateTime(bo.StartTime).ToString("mm");
                //DateTime CheackInDateTime = Convert.ToDateTime(bo.StartTime);
                //string S = CheackInDateTime.ToString("tt");
                //this.ddlStartAMPM.SelectedIndex = S == "AM" ? 0 : 1;
            }

            if (bo.EndTime != null)
            {
                txtEndHour.Text = bo.EndTime.ToString(userInformationBO.TimeFormat);
                //this.txtEndHour.Text = Convert.ToInt32(Convert.ToDateTime(bo.EndTime).ToString("%h")) == 0 ? "12" : Convert.ToDateTime(bo.EndTime).ToString("%h");
                //this.txtEndMinute.Text = Convert.ToDateTime(bo.EndTime).ToString("mm");
                //DateTime CheackInDateTime = Convert.ToDateTime(bo.EndTime);
                //string eS = CheackInDateTime.ToString("tt");
                //this.ddlEndAMPM.SelectedIndex = eS == "AM" ? 0 : 1;
            }

        }
        private void LoadDayOne()
        {
            DayDA dayDA = new DayDA();
            var days = dayDA.GetDayList();
            ddlDayOffOne.DataSource = days;

            ddlDayOffOne.DataTextField = "Name";
            ddlDayOffOne.DataValueField = "Name";
            ddlDayOffOne.DataBind();

            ListItem itemDay = new ListItem();
            itemDay.Value = "None";
            itemDay.Text = "--- None ---";
            this.ddlDayOffOne.Items.Insert(0, itemDay);
        }
        private void LoadDayTwo()
        {

            DayDA dayDA = new DayDA();
            ddlDayOffTwo.DataSource = dayDA.GetDayList();
            ddlDayOffTwo.DataTextField = "Name";
            ddlDayOffTwo.DataValueField = "Name";
            ddlDayOffTwo.DataBind();

            ListItem itemDay = new ListItem();
            itemDay.Value = "None";
            itemDay.Text = "--- None ---";
            this.ddlDayOffTwo.Items.Insert(0, itemDay);
        }
        private void LoadEmployeeCategory()
        {

            List<EmpTypeBO> categoryList = new List<EmpTypeBO>();
            EmpTypeDA categoryDA = new EmpTypeDA();
            categoryList = categoryDA.GetEmpTypeInfo().Where(x => x.ActiveStat == true).ToList();
            ddlCategoryId.DataSource = categoryList;
            ddlCategoryId.DataTextField = "Name";
            ddlCategoryId.DataValueField = "TypeId";
            ddlCategoryId.DataBind();

            ListItem itemDay = new ListItem();
            itemDay.Value = "0";
            itemDay.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCategoryId.Items.Insert(0, itemDay);
        }
        private void LoadSerchDayTwo()
        {
            DayDA dayDA = new DayDA();
            var days = dayDA.GetDayList();
            ddSDayOffTwo.DataSource = days;

            ddSDayOffTwo.DataTextField = "Name";
            ddSDayOffTwo.DataValueField = "Name";
            ddSDayOffTwo.DataBind();

            ListItem itemDay = new ListItem();
            itemDay.Value = "All";
            itemDay.Text = "--- All ---";
            this.ddSDayOffTwo.Items.Insert(0, itemDay);

        }
        private void LoadSearchDayOne()
        {
            DayDA dayDA = new DayDA();
            ddlSDayOffOne.DataSource = dayDA.GetDayList();
            ddlSDayOffOne.DataTextField = "Name";
            ddlSDayOffOne.DataValueField = "Name";
            ddlSDayOffOne.DataBind();

            ListItem itemDay = new ListItem();
            itemDay.Value = "All";
            itemDay.Text = "--- All ---";
            this.ddlSDayOffOne.Items.Insert(0, itemDay);
        }
        private void LoadSearchEmployeeCategory()
        {
            List<EmpTypeBO> categoryList = new List<EmpTypeBO>();
            EmpTypeDA categoryDA = new EmpTypeDA();
            categoryList = categoryDA.GetEmpTypeInfo().Where(x => x.ActiveStat == true).ToList();
            ddlSCategory.DataSource = categoryList;
            ddlSCategory.DataTextField = "Name";
            ddlSCategory.DataValueField = "TypeId";
            ddlSCategory.DataBind();

            ListItem itemCategory = new ListItem();
            itemCategory.Value = "0";
            itemCategory.Text = "--- All ---";
            this.ddlSCategory.Items.Insert(0, itemCategory);
        }
        private void SetDefaulTime()
        {
            this.txtStartHour.Text = "10:00";

            this.txtEndHour.Text = "06:00";           
        }

        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;
        }
    }
}