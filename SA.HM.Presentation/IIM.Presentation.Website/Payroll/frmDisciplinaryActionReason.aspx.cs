using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmDisciplinaryActionReason : BasePage
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
            CheckObjecctPermission();
        }

        protected void CheckObjecctPermission()
        {
            btnSave.Visible = isSavePermission;
        }

        protected void gvDisciplinaryActionReason_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvDisciplinaryActionReason.PageIndex = e.NewPageIndex;
            //this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void gvDisciplinaryActionReason_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvDisciplinaryActionReason_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int actionReasonId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(actionReasonId);
                this.btnSave.Visible = isUpdatePermission;
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("PayrollDisciplinaryActionReason", "DisciplinaryActionReasonId", actionReasonId);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.DisciplinaryActionReason.ToString(), actionReasonId,
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
                if (string.IsNullOrWhiteSpace(txtActionReason.Text))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Action Reason.", AlertType.Warning);
                    txtActionReason.Focus();
                }
                else
                {
                    DisciplinaryActionReasonBO actionReasonBO = new DisciplinaryActionReasonBO();
                    DisciplinaryActionDA disActionDA = new DisciplinaryActionDA();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    actionReasonBO.ActionReason = txtActionReason.Text;
                    actionReasonBO.Remarks = txtRemarks.Text;

                    if (string.IsNullOrWhiteSpace(hfDisActionReasonId.Value))
                    {
                        actionReasonBO.CreatedBy = userInformationBO.UserInfoId;
                        Boolean status = disActionDA.SaveDisciplinaryActionReasonInfo(actionReasonBO);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.DisciplinaryActionReason.ToString(), 0,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DisciplinaryActionReason));
                            this.LoadGridView();
                            this.Cancel();
                        }
                    }
                    else
                    {
                        actionReasonBO.DisciplinaryActionReasonId = Convert.ToInt32(hfDisActionReasonId.Value);
                        actionReasonBO.LastModifiedBy = userInformationBO.UserInfoId;
                        Boolean status = disActionDA.UpdateDisciplinaryActionReasonInfo(actionReasonBO);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.DisciplinaryActionReason.ToString(), actionReasonBO.DisciplinaryActionReasonId,
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
            txtActionReason.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            this.btnSave.Text = "Save";
            hfDisActionReasonId.Value = string.Empty;
        }

        private void LoadGridView()
        {
            //this.CheckObjectPermission();

            List<DisciplinaryActionReasonBO> actionReasonList = new List<DisciplinaryActionReasonBO>();
            DisciplinaryActionDA disActionDA = new DisciplinaryActionDA();

            actionReasonList = disActionDA.GetDisciplinaryActionReasonList();

            this.gvDisciplinaryActionReason.DataSource = actionReasonList;
            this.gvDisciplinaryActionReason.DataBind();
            SetTab("SearchTab");
        }
        public void FillForm(int editId)
        {
            DisciplinaryActionReasonBO actionReasonBO = new DisciplinaryActionReasonBO();
            DisciplinaryActionDA disActionDA = new DisciplinaryActionDA();
            actionReasonBO = disActionDA.GetDisciplinaryActionReasonById(editId);
            txtActionReason.Text = actionReasonBO.ActionReason;
            txtRemarks.Text = actionReasonBO.Remarks;
            hfDisActionReasonId.Value = actionReasonBO.DisciplinaryActionReasonId.ToString();
            btnSave.Text = "Update";
        }
        private void Cancel()
        {
            txtActionReason.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            this.btnSave.Text = "Save";
            hfDisActionReasonId.Value = string.Empty;
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