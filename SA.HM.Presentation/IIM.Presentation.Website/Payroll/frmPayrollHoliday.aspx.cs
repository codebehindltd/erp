using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmPayrollHoliday : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();        
        protected int isNewAddButtonEnable = 1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                CheckObjectPermission();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtHolidayName.Text))
            {
                this.isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Holiday Name.", AlertType.Warning);
                this.txtHolidayName.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtHolidayStartDate.Text))
            {                
                this.isNewAddButtonEnable = 2;                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Holiday Start Date.", AlertType.Warning);
                this.txtHolidayStartDate.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtHolidayEndDate.Text))
            {
                this.isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Holiday End Date.", AlertType.Warning);
                this.txtHolidayEndDate.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtDescription.Text))
            {                
                this.isNewAddButtonEnable = 2;                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Description of holiday.", AlertType.Warning);
                this.txtDescription.Focus();
            }
            else
            {
                PayrollHolidayDA holidayDA = new PayrollHolidayDA();
                PayrollHolidayBO holidayBO = new PayrollHolidayBO();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                holidayBO.HolidayName = this.txtHolidayName.Text;
                holidayBO.StartDate = hmUtility.GetDateTimeFromString(this.txtHolidayStartDate.Text, userInformationBO.ServerDateFormat);
                holidayBO.EndDate = hmUtility.GetDateTimeFromString(this.txtHolidayEndDate.Text, userInformationBO.ServerDateFormat);
                holidayBO.Description = txtDescription.Text;

                if (string.IsNullOrWhiteSpace(txtHolidayId.Value))
                {
                    int tmpHolidayId = 0;
                    holidayBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = holidayDA.SavePayrollHoliDayInfo(holidayBO, out tmpHolidayId);
                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.Bank.ToString(), tmpHolidayId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Bank));                                                
                        this.Cancel();
                    }
                }
                else
                {
                    holidayBO.HolidayId = Convert.ToInt32(txtHolidayId.Value);
                    holidayBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = holidayDA.UpdatePayrollHolidayInfo(holidayBO);
                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.PayrollHoliday.ToString(), holidayBO.HolidayId,
                            ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PayrollHoliday));
                        this.Cancel();
                    }
                }
                this.SetTab("EntryTab");
            }
            this.SetTab("EntryTab");
        }
        protected void gvPayrollHoliday_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {            
            this.gvPayrollHoliday.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void gvPayrollHoliday_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvPayrollHoliday_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
            int holidayId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(holidayId);
                btnSave.Visible = isUpdatePermission;
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("PayrollHoliday", "HolidayId", holidayId);
                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.PayrollHoliday.ToString(), holidayId,
                            ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PayrollHoliday));
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
        //************************ User Defined Function ********************//
        private void Cancel()
        {
            this.txtHolidayName.Text = string.Empty;
            this.txtHolidayStartDate.Text = string.Empty;
            this.txtHolidayEndDate.Text = string.Empty;
            this.txtDescription.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.txtHolidayId.Value = string.Empty;
            btnSave.Visible = isSavePermission;
        }
        private void LoadGridView()
        {
            this.CheckObjectPermission();
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
            if (string.IsNullOrWhiteSpace(this.txtStartDate.Text))
            {
                startDate = hmUtility.GetFromDate();
            }
            else
            {
                startDate = this.txtStartDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            {
                endDate = hmUtility.GetToDate();
            }
            else
            {
                endDate = this.txtEndDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            PayrollHolidayDA holidayDA = new PayrollHolidayDA();
            List<PayrollHolidayBO> list = holidayDA.GetPayrollHolidayInformationBySearchCritaria(FromDate, ToDate);
            this.gvPayrollHoliday.DataSource = list;
            this.gvPayrollHoliday.DataBind();
            
            SetTab("SearchTab");
        }
        private void CheckObjectPermission()
        {
            
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        public void FillForm(int EditId)
        {
            PayrollHolidayBO holidayBO = new PayrollHolidayBO();
            PayrollHolidayDA holidayDA = new PayrollHolidayDA();
            holidayBO = holidayDA.GetPayrollHolidayInfoByID(EditId);
            txtHolidayName.Text = holidayBO.HolidayName;
            txtHolidayStartDate.Text = hmUtility.GetStringFromDateTime(holidayBO.StartDate);
            txtHolidayEndDate.Text = hmUtility.GetStringFromDateTime(holidayBO.EndDate);
            txtHolidayId.Value=holidayBO.HolidayId.ToString();
            txtDescription.Text = holidayBO.Description;
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
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        //[WebMethod]
        //public static string DeleteData(int sEmpId)
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        HMCommonDA hmCommonDA = new HMCommonDA();
        //        Boolean status = hmCommonDA.DeleteInfoById("PayrollHoliday", "HolidayId", sEmpId);
        //        if (status)
        //        {
        //            result = "success";
        //        }
        //    }
        //    catch
        //    {
        //        //lblMessage.Text = "Data Deleted Failed.";
        //    }

        //    return result;
        //}
        [WebMethod]
        public static string LoadBreadCrumbsInformation()
        {
            string breadCrumbs = string.Empty;
            breadCrumbs = "<span class='divider'>/</span><a href='/HMCommon/frmPayrollHoliday.aspx'>Holiday</a><span class='divider'></span>";
            return breadCrumbs;
        }
        
    }
}