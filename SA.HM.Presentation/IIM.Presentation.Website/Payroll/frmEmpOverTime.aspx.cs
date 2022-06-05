using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpOverTime : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();        
        protected int isNewAddButtonEnable = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                //LoadDummyGridData();
            }
            CheckObjectPermission();
        }
        private void CheckObjectPermission()
        {
            
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }

        private void LoadDummyGridData()
        {
            List<EmpOverTimeBO> overtime = new List<EmpOverTimeBO>();

            EmpOverTimeBO obj = new EmpOverTimeBO();

            obj.EmpId = 0;
            obj.OverTimeId = 0;
            obj.OverTimeDate = DateTime.Now;
           // obj.OverTimeHour = 0;

            overtime.Add(obj);

            this.gvOvertime.DataSource = overtime;
            this.gvOvertime.DataBind();
        }

        [WebMethod]
        public static ReturnInfo PerformOverTimeSaveAction(int employeeId, string overTimeId, decimal overTimeHour, string overTimeDate)
        {
            string message = "";
            ReturnInfo rtninf = new ReturnInfo();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            EmpOverTimeBO overTimeBO = new EmpOverTimeBO();
            EmpOverTimeDA overTimeDA = new EmpOverTimeDA();
            List<EmpOverTimeBO> empOverTimeList = new List<EmpOverTimeBO>();

            overTimeBO.EmpId = employeeId;
            overTimeBO.OTHour = overTimeHour;
            overTimeBO.OverTimeDate = hmUtility.GetDateTimeFromString(overTimeDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            

            if (string.IsNullOrWhiteSpace(overTimeId))
            {
                int tmpUserInfoId = 0;
                overTimeBO.CreatedBy = userInformationBO.UserInfoId;
                empOverTimeList.Add(overTimeBO);
                Boolean status = overTimeDA.SaveOvertime(empOverTimeList, overTimeBO.CreatedBy, out tmpUserInfoId);
                //overTimeDA.SaveOvertime(empOverTimeList, overTimeBO.CreatedBy, out tmpUserInfoId);
                if (status)
                {
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpOverTime.ToString(), tmpUserInfoId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpOverTime));

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);                         
                }
            }
            else
            {
                overTimeBO.OverTimeId = Convert.ToInt32(overTimeId);
                overTimeBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = overTimeDA.UpdateOverTimeInfo(overTimeBO);
                if (status)
                {
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpOverTime.ToString(), overTimeBO.OverTimeId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpOverTime));
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);                         
                }
            }

            return rtninf;
        }

        [WebMethod]
        public static GridViewDataNPaging<EmpOverTimeBO, GridPaging> LoadEmployeeOvertime(int employeeId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<EmpOverTimeBO, GridPaging> myGridData = new GridViewDataNPaging<EmpOverTimeBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            
            EmpOverTimeDA overTimeDA = new EmpOverTimeDA();
            List<EmpOverTimeBO> overtimeList = overTimeDA.GetAllOverTimeByEmployeeIdForGridPaging(employeeId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            foreach (EmpOverTimeBO bo in overtimeList)
            {
                //bo.Date = bo.OverTimeDate.ToString("dd/MM/yyyy");
                //bo.Date = bo.OverTimeDate.ToString(userInformationBO.ServerDateFormat);
            }

            myGridData.GridPagingProcessing(overtimeList, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            string message = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("PayrollEmpOverTime", "OverTimeId", sEmpId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                           EntityTypeEnum.EntityType.EmpOverTime.ToString(), sEmpId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpOverTime));
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch(Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;
        }

        [WebMethod]
        public static EmpOverTimeBO FillForm(int EditId)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            EmpOverTimeBO bo = new EmpOverTimeBO();
            EmpOverTimeDA da = new EmpOverTimeDA();
            bo = da.GetOverTimeInfoByID(EditId);
            //bo.Date = bo.OverTimeDate.ToString("dd/MM/yyyy");
           // bo.Date = bo.OverTimeDate.ToString(userInformationBO.ServerDateFormat);
            return bo;
        }

        public void LoadGridView(int ID)
        {
            this.CheckObjectPermission();
            EmpOverTimeBO overTime = new EmpOverTimeBO();
            List<EmpOverTimeBO> list = new List<EmpOverTimeBO>();
            EmpOverTimeDA overTimeDA = new EmpOverTimeDA();
            list = overTimeDA.GetAllOverTimeByImployeeID(ID);
            this.gvOvertime.DataSource = list;
            this.gvOvertime.DataBind();
        }

        //public void Clear()
        //{
        //    this.txtOverTimeHour.Text = string.Empty;
        //    this.txtOverTimeDate.Text = string.Empty;
        //    this.txtOverTimeId.Value = string.Empty;
        //    this.ddlEmpId.SelectedValue = "0";
        //    this.btnSave.Text = "Save";
        //}
        //protected void Close_Click(object sender, EventArgs e)
        //{
        //    Clear();
        //}
        //private bool IsFrmValid()
        //{
        //    bool flag = true;
        //    if (ddlEmpId.SelectedValue == "0")
        //    {
        //        this.isMessageBoxEnable = 1;
        //        this.lblMessage.Text = "Please select Employee Name.";
        //        ddlEmpId.Focus();
        //        flag = false;
        //    }
        //    else if (string.IsNullOrWhiteSpace(this.txtOverTimeDate.Text.Trim()))
        //    {
        //        this.isMessageBoxEnable = 1;
        //        this.lblMessage.Text = "Please provide Over Time Date.";
        //        flag = false;
        //        txtOverTimeDate.Focus();
        //    }
        //    else if (string.IsNullOrWhiteSpace(this.txtOverTimeHour.Text.Trim()))
        //    {
        //        this.isMessageBoxEnable = 1;
        //        this.lblMessage.Text = "Please provide Over Time Hour.";
        //        txtOverTimeHour.Focus();
        //        flag = false;
        //    }
        //    else
        //    {
        //        this.isMessageBoxEnable = -1;
        //        this.lblMessage.Text = string.Empty;
        //    }
        //    return flag;
        //}
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    if (!IsFrmValid())
        //    {
        //        return;
        //    }
        //    lblMessage.Text = string.Empty;
        //    EmpOverTimeBO overTimeBO = new EmpOverTimeBO();
        //    EmpOverTimeDA overTimeDA = new EmpOverTimeDA();


        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();


        //    overTimeBO.EmpId = Convert.ToInt32(this.ddlEmpId.SelectedValue);
        //    overTimeBO.OverTimeHour = Convert.ToDecimal(this.txtOverTimeHour.Text.Trim());
        //    overTimeBO.OverTimeDate = hmUtility.GetDateTimeFromString(this.txtOverTimeDate.Text.Trim());

        //    if (string.IsNullOrWhiteSpace(txtOverTimeId.Value))
        //    {
        //        int tmpUserInfoId = 0;
        //        overTimeBO.CreatedBy = userInformationBO.UserInfoId;
        //        Boolean status = overTimeDA.SaveOverTimeInfo(overTimeBO, out tmpUserInfoId);
        //        if (status)
        //        {
        //            this.isMessageBoxEnable = 2;
        //            lblMessage.Text = "Saved Operation Successfull.";
        //            this.LoadGridView(overTimeBO.EmpId);
        //            this.ddlEmpId.SelectedItem.Value = overTimeBO.EmpId.ToString();
        //            this.Cancel();
        //        }
        //    }
        //    else
        //    {
        //        overTimeBO.OverTimeId = Convert.ToInt32(txtOverTimeId.Value);
        //        overTimeBO.LastModifiedBy = userInformationBO.UserInfoId;
        //        Boolean status = overTimeDA.UpdateOverTimeInfo(overTimeBO);
        //        if (status)
        //        {
        //            this.isMessageBoxEnable = 2;
        //            lblMessage.Text = "Update Operation Successfull.";
        //            this.LoadGridView(overTimeBO.EmpId);
        //            this.ddlEmpId.SelectedItem.Value = overTimeBO.EmpId.ToString();
        //            this.Cancel();
        //        }
        //    }
        //}
        //private void Cancel()
        //{
        //    this.txtOverTimeDate.Text = string.Empty;
        //    this.txtOverTimeHour.Text = string.Empty;
        //    this.txtOverTimeId.Value = string.Empty;
        //    this.ddlEmpId.SelectedIndex = 0;
        //    this.btnSave.Text = "Save";
        //}
        //protected void ddlEmpId_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (this.ddlEmpId.SelectedIndex != 0)
        //    {
        //        this.LoadGridView(Int32.Parse(ddlEmpId.SelectedValue));
        //        this.txtEmpId.Value = ddlEmpId.SelectedValue.ToString();
        //    }
        //}
        //protected void gvOvertime_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    this.gvOvertime.PageIndex = e.NewPageIndex;
        //    this.LoadGridView(Int32.Parse(ddlEmpId.SelectedValue));
        //}
        //protected void gvOvertime_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.DataItem != null)
        //    {
        //        Label lblValue = (Label)e.Row.FindControl("lblid");
        //        ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
        //        ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
        //        imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
        //        imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
        //    }
        //}

        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
    }
}