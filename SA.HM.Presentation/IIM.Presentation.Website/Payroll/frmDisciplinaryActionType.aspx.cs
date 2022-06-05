using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmDisciplinaryActionType : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadGridView();
                this.SetTab("EntryTab");
            }
            CheckObjectPermission();
        }

        protected void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
        }

        protected void gvDisciplinaryActionType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvDisciplinaryActionType.PageIndex = e.NewPageIndex;
            //this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void gvDisciplinaryActionType_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvDisciplinaryActionType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int actionTypeId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(actionTypeId);
                this.btnSave.Visible = isUpdatePermission;
                this.btnSave.Text = "Update";                
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("PayrollDisciplinaryActionType", "DisciplinaryActionTypeId", actionTypeId);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.DisciplinaryActionType.ToString(), actionTypeId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DisciplinaryActionReason));
                    }
                    LoadGridView();
                    this.SetTab("SearchTab");
                }
                catch (Exception ex)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtActionName.Text))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Action Name.", AlertType.Warning);
                    txtActionName.Focus();
                }
                else
                {
                    DisciplinaryActionTypeBO actionTypeBO = new DisciplinaryActionTypeBO();
                    DisciplinaryActionDA disActionDA = new DisciplinaryActionDA();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    actionTypeBO.ActionName = txtActionName.Text;
                    actionTypeBO.Description = txtDescription.Text;

                    if (string.IsNullOrWhiteSpace(hfDisActionTypeId.Value))
                    {
                        actionTypeBO.CreatedBy = userInformationBO.UserInfoId;
                        Boolean status = disActionDA.SaveDisciplinaryActionTypeInfo(actionTypeBO);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.DisciplinaryActionType.ToString(), 0,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DisciplinaryActionType));
                            this.LoadGridView();
                            this.Cancel();
                        }
                    }
                    else
                    {
                        actionTypeBO.DisciplinaryActionTypeId = Convert.ToInt16(hfDisActionTypeId.Value);
                        actionTypeBO.LastModifiedBy = userInformationBO.UserInfoId;
                        Boolean status = disActionDA.UpdateDisciplinaryActionTypeInfo(actionTypeBO);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.DisciplinaryActionType.ToString(), actionTypeBO.DisciplinaryActionTypeId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DisciplinaryActionReason));
                            this.LoadGridView();
                            this.Cancel();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }

        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtActionName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            this.btnSave.Text = "Save";
            hfDisActionTypeId.Value = string.Empty;
        }

        private void LoadGridView()
        {
            //this.CheckObjectPermission();

            List<DisciplinaryActionTypeBO> actionTypeList = new List<DisciplinaryActionTypeBO>();
            DisciplinaryActionDA disActionDA = new DisciplinaryActionDA();

            actionTypeList = disActionDA.GetDisciplinaryActionTypeList();

            this.gvDisciplinaryActionType.DataSource = actionTypeList;
            this.gvDisciplinaryActionType.DataBind();
            SetTab("SearchTab");
        }
        public void FillForm(int editId)
        {
            DisciplinaryActionTypeBO actionTypeBO = new DisciplinaryActionTypeBO();
            DisciplinaryActionDA disActionDA = new DisciplinaryActionDA();
            actionTypeBO = disActionDA.GetDisciplinaryActionTypeById(editId);
            txtActionName.Text = actionTypeBO.ActionName;
            txtDescription.Text = actionTypeBO.Description;
            hfDisActionTypeId.Value = actionTypeBO.DisciplinaryActionTypeId.ToString();
            btnSave.Visible = isUpdatePermission;
            btnSave.Text = "Update";
        }
        private void Cancel()
        {
            txtActionName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            this.btnSave.Text = "Save";
            hfDisActionTypeId.Value = string.Empty;
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
    }
}