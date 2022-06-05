using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using System.Web.Services;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;


namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmDepartment : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();        
        protected int isNewAddButtonEnable = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");            
        }
        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void LoadGridView()
        {
            this.CheckObjectPermission();

            String DepartmentName = txtSDepartmentName.Text;
            Boolean ActiveStat = ddlSActiveStat.SelectedIndex == 0 ? true : false;
            DepartmentDA da = new DepartmentDA();
            List<DepartmentBO> files = da.GetDepartmentInfoBySearchCriteria(DepartmentName,ActiveStat);

            this.gvGuestHouseService.DataSource = files;
            this.gvGuestHouseService.DataBind();
            this.SetTab("SearchTab");
        }
        private void Cancel()
        {
            this.txtName.Text = string.Empty;
            this.txtRemarks.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            
            this.btnSave.Text = "Save";
            this.txtDepartmentId.Value = string.Empty;
            this.txtName.Focus();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.txtName.Text))
                {                    
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Name.", AlertType.Warning);
                    this.txtName.Focus();
                    txtName.Focus();
                }
                else if (DuplicateCheckDynamicaly("Name", txtName.Text, 0) > 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Department Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                    this.txtName.Focus();                   
                }
                else
                {                    
                    DepartmentBO bo = new DepartmentBO();
                    DepartmentDA da = new DepartmentDA();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    bo.Name = this.txtName.Text;
                    bo.Remarks = this.txtRemarks.Text;
                    bo.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

                    if (string.IsNullOrWhiteSpace(txtDepartmentId.Value))
                    {
                        int tmpPKId = 0;
                        bo.CreatedBy = userInformationBO.UserInfoId;
                        Boolean status = da.SaveDepartmentInfo(bo, out tmpPKId);
                        if (status)
                        {                            
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.Benefit.ToString(), tmpPKId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Benefit));
                            this.Cancel();
                        }
                    }
                    else
                    {
                        bo.DepartmentId = Convert.ToInt32(txtDepartmentId.Value);
                        bo.LastModifiedBy = userInformationBO.UserInfoId;
                        Boolean status = da.UpdateDepartmentInfo(bo);
                        if (status)
                        {                            
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmployeeDepartment.ToString(), bo.DepartmentId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeeDepartment));
                            this.Cancel();
                        }
                    }

                    this.SetTab("EntryTab");
                }
                this.SetTab("EntryTab");
            }
            catch(Exception ex){
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        protected void gvGuestHouseService_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {            
            this.gvGuestHouseService.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void gvGuestHouseService_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvGuestHouseService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int departmentId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(departmentId);
                this.btnSave.Visible = isUpdatePermission;
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            //else if (e.CommandName == "CmdDelete")
            //{
            //    try
            //    {
            //        HMCommonDA hmCommonDA = new HMCommonDA();

            //        Boolean status = hmCommonDA.DeleteInfoById("PayrollDepartment", "DepartmentId", departmentId);
            //        if (status)
            //        {                        
            //            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
            //            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
            //                EntityTypeEnum.EntityType.EmployeeDepartment.ToString(), departmentId,
            //                ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
            //                hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeeDepartment));
            //        }
            //        LoadGridView();
            //        this.SetTab("SearchTab");
            //    }
            //    catch(Exception ex){
            //        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            //    }
            //}
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
            DepartmentBO bo = new DepartmentBO();
            DepartmentDA da = new DepartmentDA();
            bo = da.GetDepartmentInfoById(EditId);

            txtRemarks.Text = bo.Remarks;
            txtName.Text = bo.Name;
            ddlActiveStat.SelectedValue = (bo.ActiveStat == true ? 0 : 1).ToString();
            txtDepartmentId.Value = bo.DepartmentId.ToString();           
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "PayrollDepartment";
            string pkFieldName = "DepartmentId";
            string pkFieldValue = this.txtDepartmentId.Value;
            int IsDuplicate = 0;
            if (!string.IsNullOrWhiteSpace(pkFieldValue))
            {
                isUpdate = 1;
            }
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }

        [WebMethod]
        public static int CheckReference(int departmentId)
        {
            int DepartmentId = 0;
            DepartmentDA da = new DepartmentDA();
            DepartmentId = da.CheckDepartmentReference(departmentId);

            return DepartmentId;
        }
        [WebMethod]
        public static ReturnInfo DeleteData(int departmentId)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            HMCommonDA DA = new HMCommonDA();
            status = DA.DeleteInfoById("PayrollDepartment", "DepartmentId", departmentId);
            if (status)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.EmployeeDepartment.ToString(), departmentId,
                               ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeeDepartment));
            }
            return rtninfo;
        }
    }
}