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
    public partial class frmEmpType : BasePage
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
                //     imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //    imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvGuestHouseService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int formulaId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(formulaId);
                btnSave.Visible = isUpdatePermission;
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("PayrollEmpType", "TypeId", formulaId);
                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.EmpType.ToString(), formulaId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpType));
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
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Category Name.", AlertType.Warning);
                    this.txtName.Focus();
                }
                else if (string.IsNullOrEmpty(txtCode.Text))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Code.", AlertType.Warning);
                    txtCode.Focus();
                }
                else
                {
                    EmpTypeBO bo = new EmpTypeBO();
                    EmpTypeDA da = new EmpTypeDA();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    bo.Name = this.txtName.Text;
                    bo.Code = this.txtCode.Text;
                    bo.Remarks = this.txtRemarks.Text;
                    bo.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
                    if (ddlTypeCategory.SelectedValue == "Contractual")
                    {
                        bo.IsContractualType = true;
                    }
                    else
                    {
                        bo.IsContractualType = false;
                    }

                    bo.TypeCategory = ddlTypeCategory.SelectedValue;

                    if (string.IsNullOrWhiteSpace(txtCategoryId.Value))
                    {
                        if (DuplicateCheckDynamicaly("Name", txtName.Text, 0) == 1)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Type Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                            txtName.Focus();
                            return;
                        }
                        if (DuplicateCheckDynamicaly("Code", txtCode.Text, 0) == 1)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Code" + AlertMessage.DuplicateValidation, AlertType.Warning);
                            txtCode.Focus();
                            return;
                        }

                        int tmpPKId = 0;
                        bo.CreatedBy = userInformationBO.UserInfoId;
                        Boolean status = da.SaveEmpTypeInfo(bo, out tmpPKId);
                        if (status)
                        {                            
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);

                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpType.ToString(), tmpPKId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpType));

                            this.LoadGridView();
                            this.Cancel();
                        }
                    }
                    else
                    {
                        bo.TypeId = Convert.ToInt32(txtCategoryId.Value);
                        bo.LastModifiedBy = userInformationBO.UserInfoId;
                        Boolean status = da.UpdateEmpTypeInfo(bo);
                        if (status)
                        {                            
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);

                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpType.ToString(), bo.TypeId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpType));

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

            EmpTypeDA da = new EmpTypeDA();
            List<EmpTypeBO> files = da.GetEmpTypeInfo();

            this.gvGuestHouseService.DataSource = files;
            this.gvGuestHouseService.DataBind();
        }
        private void Cancel()
        {
            this.txtName.Text = string.Empty;
            this.txtCode.Text = string.Empty;
            this.txtRemarks.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.txtCategoryId.Value = string.Empty;
            this.txtName.Focus();
            this.ddlTypeCategory.SelectedValue = "Regular";
        }
        public void FillForm(int EditId)
        {
            EmpTypeBO bo = new EmpTypeBO();
            EmpTypeDA da = new EmpTypeDA();

            bo = da.GetEmpTypeInfoById(EditId);
            txtName.Text = bo.Name;
            txtCode.Text = bo.Code;
            txtRemarks.Text = bo.Remarks;
            ddlTypeCategory.SelectedValue = bo.TypeCategory;
            ddlActiveStat.SelectedIndex = bo.ActiveStat == true ? 0 : 1;
            txtCategoryId.Value = bo.TypeId.ToString();
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
            string tableName = "PayrollEmpType";
            string pkFieldName = "TypeId";
            string pkFieldValue = txtCategoryId.Value;
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

        //        Boolean status = hmCommonDA.DeleteInfoById("PayrollEmpCategory", "CategoryId", sEmpId);
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