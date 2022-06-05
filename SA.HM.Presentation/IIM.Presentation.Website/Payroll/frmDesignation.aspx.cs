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
    public partial class frmDesignation : BasePage
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
                this.LoadGridView();
                this.SetTab("EntryTab");
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
                //  imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //  imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvGuestHouseService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int designationId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(designationId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("PayrollDesignation", "DesignationId", designationId);
                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    }
                    LoadGridView();
                    this.SetTab("SearchTab");
                }
                catch(Exception ex){
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.txtName.Text))
                {                    
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Name.", AlertType.Warning);
                    this.txtName.Focus();
                }
                else
                {
                    DesignationBO bo = new DesignationBO();
                    DesignationDA da = new DesignationDA();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    bo.Name = this.txtName.Text;
                    bo.Remarks = this.txtRemarks.Text;
                    bo.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

                    if (string.IsNullOrWhiteSpace(txtDesignationId.Value))
                    {
                        if (DuplicateCheckDynamicaly("Name", txtName.Text, 0) == 1)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Designation" + AlertMessage.DuplicateValidation, AlertType.Warning);
                            txtName.Focus();
                            return;
                        }

                        int tmpPKId = 0;
                        bo.CreatedBy = userInformationBO.UserInfoId;
                        Boolean status = da.SaveDesignationInfo(bo, out tmpPKId);
                        if (status)
                        {                            
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmployeeDesignation.ToString(), tmpPKId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeeDesignation));
                            this.LoadGridView();
                            this.Cancel();
                        }
                    }
                    else
                    {
                        bo.DesignationId = Convert.ToInt32(txtDesignationId.Value);
                        bo.LastModifiedBy = userInformationBO.UserInfoId;
                        Boolean status = da.UpdateDesignationInfo(bo);
                        if (status)
                        {                            
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmployeeDesignation.ToString(), bo.DesignationId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeeDesignation));
                            this.LoadGridView();
                            this.Cancel();
                        }
                    }
                }
            }
            catch(Exception ex){
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        //************************ User Defined Function ********************//
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

            DesignationDA da = new DesignationDA();
            List<DesignationBO> files = da.GetDesignationInfo();

            this.gvGuestHouseService.DataSource = files;
            this.gvGuestHouseService.DataBind();
            SetTab("SearchTab");
        }
        private void Cancel()
        {
            this.txtName.Text = string.Empty;
            this.txtRemarks.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.txtDesignationId.Value = string.Empty;
            this.txtName.Focus();
        }
        public void FillForm(int EditId)
        {
            DesignationBO bo = new DesignationBO();
            DesignationDA da = new DesignationDA();
            bo = da.GetDesignationInfoById(EditId);
            txtName.Text = bo.Name;
            txtRemarks.Text = bo.Remarks;
            ddlActiveStat.SelectedIndex = bo.ActiveStat == true ? 0 : 1;
            txtDesignationId.Value = bo.DesignationId.ToString();
            btnSave.Visible = isUpdatePermission;
            btnSave.Text = "Update";


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
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "PayrollDesignation";
            string pkFieldName = "DesignationId";
            string pkFieldValue = txtDesignationId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        //************************ User Defined WebMethod ********************//
        //[WebMethod]
        //public static string DeleteData(int sEmpId)
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        HMCommonDA hmCommonDA = new HMCommonDA();

        //        Boolean status = hmCommonDA.DeleteInfoById("PayrollDesignation", "DesignationId", sEmpId);
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
    }
}