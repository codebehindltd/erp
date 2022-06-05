using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpGrade : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();        
        protected int isNewAddButtonEnable = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadEmpProvisionPeriod();
            }
        }
        private void LoadEmpProvisionPeriod()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("EmpProvisionPeriod");

            this.ddlEmpProvisionPeriod.DataSource = fields;
            this.ddlEmpProvisionPeriod.DataTextField = "FieldValue";
            this.ddlEmpProvisionPeriod.DataValueField = "FieldId";
            this.ddlEmpProvisionPeriod.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlEmpProvisionPeriod.Items.Insert(0, item);

        }
        private void CheckObjectPermission()
        {
            
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void Cancel()
        {
            this.ddlEmpProvisionPeriod.SelectedValue = "0";
            this.ddlActiveStat.SelectedValue = "0";
            this.chkIsManagement.Checked = false;
            this.txtRemarks.Text = string.Empty;
            this.txtName.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.ddlActiveStat.Focus();
        }
        private bool IsFrmValid()
        {
            bool flag = true;

            if (string.IsNullOrWhiteSpace(this.txtName.Text.Trim()))
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Grade Name.", AlertType.Warning);
                flag = false;
                txtName.Focus();
            }            
            return flag;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFrmValid())
                {
                    return;
                }                
                EmpGradeBO gradeBO = new EmpGradeBO();
                EmpGradeDA gradeDA = new EmpGradeDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                gradeBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
                gradeBO.Remarks = this.txtRemarks.Text.Trim().ToString();
                gradeBO.Name = this.txtName.Text.Trim().ToString();
                gradeBO.ProvisionPeriodId = Convert.ToInt32(this.ddlEmpProvisionPeriod.SelectedValue);
                gradeBO.IsManagement = this.chkIsManagement.Checked ? true : false;

                if (string.IsNullOrWhiteSpace(txtGradeId.Value))
                {
                    if (DuplicateCheckDynamicaly("Name", txtName.Text, 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Grade Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        txtName.Focus();
                        return;
                    }

                    int tmpUserInfoId = 0;
                    gradeBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = gradeDA.SaveGradeInfo(gradeBO, out tmpUserInfoId);

                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpGrade.ToString(), tmpUserInfoId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpGrade));
                        this.Cancel();
                    }
                    else
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, "You Can't Save. Grade Name" + txtName.Text + "  Already Inserted.", AlertType.Warning);
                        this.Cancel();
                    }

                }
                else
                {
                    gradeBO.GradeId = Convert.ToInt32(txtGradeId.Value);
                    gradeBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = gradeDA.UpdateGradeInfo(gradeBO);
                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpGrade.ToString(), gradeBO.GradeId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpGrade));
                        this.Cancel();
                        txtGradeId.Value = "";
                    }
                }
                SetTab("EntryTab");
            }
            catch(Exception ex){
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
            
        }

        public void LoadGridView()
        {
            this.CheckObjectPermission();

            string Name = txtSGradeName.Text;
            Boolean ActiveStat = ddlSActiveStat.SelectedIndex == 0 ? true : false;

            EmpGradeBO empGradeBO = new EmpGradeBO();
            EmpGradeDA empGradeDA = new EmpGradeDA();
            List<EmpGradeBO> files = new List<EmpGradeBO>();
            files = empGradeDA.GetGradeInfoBySearchCriteria(Name,ActiveStat);
            this.gvGrade.DataSource = files;
            this.gvGrade.DataBind();
            this.SetTab("SearchTab");
        }

        protected void gvGrade_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {            
            this.gvGrade.PageIndex = e.NewPageIndex;
            //this.LoadGridView();
        }
        protected void gvGrade_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
             //   imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
             //   imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvGrade_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int gradeId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(gradeId);
                this.btnSave.Visible = isUpdatePermission;
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("PayrollEmpGrade", "GradeId", gradeId);
                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.EmpGrade.ToString(), gradeId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpGrade));
                    }
                    LoadGridView();
                    this.SetTab("SearchTab");
                }
                catch(Exception ex){
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
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
            EmpGradeBO gradeBO = new EmpGradeBO();
            EmpGradeDA gradeDA = new EmpGradeDA();
            gradeBO = gradeDA.GetGradeInfoById(EditId);
            txtGradeId.Value = gradeBO.GradeId.ToString();
            txtName.Text = gradeBO.Name;
            if (gradeBO.IsManagement.Equals(true))
            {
                this.chkIsManagement.Checked = true;
            }
            ddlEmpProvisionPeriod.SelectedValue = gradeBO.ProvisionPeriodId.ToString();
            txtRemarks.Text = gradeBO.Remarks;
            ddlActiveStat.SelectedValue = (gradeBO.ActiveStat == true ? 0 : 1).ToString();
   
        }

        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "PayrollEmpGrade";
            string pkFieldName = "GradeId";
            string pkFieldValue = txtGradeId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
    }
}