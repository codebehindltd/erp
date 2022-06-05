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
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpTaxDeduction : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();        
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadGender();
                CheckObjectPermission();
            }
        }
        protected void btnEmpTaxDdctnSave_Click(object sender, EventArgs e)
        {
            if (!IsTaxDeductFrmValid())
            {
                return;
            }            

            try
            {
                EmpTaxDeductionSettingBO taxDeductBO = new EmpTaxDeductionSettingBO();
                EmpTaxDeductionDA taxDeductDA = new EmpTaxDeductionDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                taxDeductBO.Gender = ddlGender.SelectedValue;
                taxDeductBO.RangeFrom = Convert.ToDecimal(this.txtRngFrm.Text.Trim());
                taxDeductBO.RangeTo = Convert.ToDecimal(this.txtRngTo.Text.Trim());
                taxDeductBO.DeductionPercentage = Convert.ToDecimal(this.txtDdctnPrcntg.Text.Trim());
                taxDeductBO.Remarks = this.txtTaxDdctnRemarks.Text;

                // int hiddenId = Convert.ToInt32(txtTaxDeductionId.Value);
                int tmpId;
                if (string.IsNullOrWhiteSpace(txtTaxDeductionId.Value))
                {
                    taxDeductBO.CreatedBy = userInformationBO.UserInfoId;
                    taxDeductBO.CreatedDate = DateTime.Now;

                    Boolean status = taxDeductDA.SaveEmpTaxDeductInfo(taxDeductBO, out tmpId);
                    if (status)
                    {
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpTaxDeductionSetting.ToString(), tmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTaxDeductionSetting));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        this.Clear();
                    }
                }
                else
                {
                    taxDeductBO.TaxDeductionId = Convert.ToInt32(txtTaxDeductionId.Value);
                    taxDeductBO.LastModifiedBy = userInformationBO.UserInfoId;
                    taxDeductBO.LastModifiedDate = DateTime.Now;

                    Boolean status = taxDeductDA.UpdateEmpTaxDeductInfo(taxDeductBO);
                    if (status)
                    {
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.EmpTaxDeductionSetting.ToString(), taxDeductBO.TaxDeductionId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTaxDeductionSetting));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        this.Clear();
                        btnEmpTaxDdctnSave.Text = "Save";
                    }
                }
            }
            catch(Exception ex){
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }

        }
        protected void btnEmpTaxDdctnClear_Click(object sender, EventArgs e)
        {
            this.txtRngFrm.Text = string.Empty;
            this.txtRngTo.Text = string.Empty;
            this.txtDdctnPrcntg.Text = string.Empty;
            this.txtTaxDdctnRemarks.Text = string.Empty;
        }
        private void LoadGender()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("Gender", hmUtility.GetDropDownFirstValue());

            ddlGender.DataSource = fields;
            ddlGender.DataTextField = "FieldValue";
            ddlGender.DataValueField = "FieldValue";
            ddlGender.DataBind();
        }
        private void Clear()
        {
            btnEmpTaxDdctnSave.Visible = isSavePermission;
            this.ddlGender.SelectedValue = hmUtility.GetDropDownFirstValue();
            this.txtRngFrm.Text = string.Empty;
            this.txtRngTo.Text = string.Empty;
            this.txtDdctnPrcntg.Text = string.Empty;
            this.txtTaxDdctnRemarks.Text = string.Empty;
        }
        protected void gvGuestHouseService_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvGuestHouseService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int taxDeductId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                btnEmpTaxDdctnSave.Visible = isUpdatePermission;
                LoadTaxDeduction(taxDeductId);
                this.btnEmpTaxDdctnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("PayrollEmpTaxDeductionSetting", "TaxDeductionId", taxDeductId);
                if (status)
                {
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                           EntityTypeEnum.EntityType.EmpTaxDeductionSetting.ToString(), taxDeductId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTaxDeductionSetting));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);                        
                }
                LoadGridView();
                this.SetTab("SearchTab");
            }
        }
        private bool IsTaxDeductFrmValid()
        {
            bool flag = true;
            int checkNumber;

            if (txtRngFrm.Text == "")
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Range From.", AlertType.Warning);    
                flag = false;
                txtRngFrm.Focus();
            }
            else if (int.TryParse(txtRngFrm.Text, out checkNumber) == false)
            {                
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning); 
                flag = false;
                txtRngFrm.Focus();
            }
            else if (txtRngTo.Text == "")
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Range To.", AlertType.Warning); 
                flag = false;
                txtRngTo.Focus();
            }
            else if (int.TryParse(txtRngTo.Text, out checkNumber) == false)
            {                
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning); 
                flag = false;
                txtRngTo.Focus();
            }
            else if (txtDdctnPrcntg.Text == "")
            {                
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Deduction.", AlertType.Warning); 
                flag = false;
                txtDdctnPrcntg.Focus();
            }
            else if (ddlGender.SelectedValue == "--- Please Select ---")
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Select Gender.", AlertType.Warning);
                flag = false;
                ddlGender.Focus();
            }
            else if (int.TryParse(txtDdctnPrcntg.Text, out checkNumber) == false)
            {                
                CommonHelper.AlertInfo(innboardMessage, "You have not entered a valid number.", AlertType.Warning); 
                flag = false;
                txtDdctnPrcntg.Focus();
            }
            return flag;
        }
        private void LoadTaxDeduction(int taxDeductId)
        {
            EmpTaxDeductionSettingBO taxDeductBO = new EmpTaxDeductionSettingBO();
            EmpTaxDeductionDA taxDeductDA = new EmpTaxDeductionDA();
            taxDeductBO = taxDeductDA.GetEmpTaxDeductInfo(taxDeductId);
            this.ddlGender.SelectedValue = taxDeductBO.Gender.ToString();
            this.txtRngFrm.Text = taxDeductBO.RangeFrom.ToString();
            this.txtRngTo.Text = taxDeductBO.RangeTo.ToString();
            this.txtDdctnPrcntg.Text = taxDeductBO.DeductionPercentage.ToString();
            if (!String.IsNullOrWhiteSpace(taxDeductBO.Remarks))
            {
                this.txtTaxDdctnRemarks.Text = taxDeductBO.Remarks.ToString();
            }
            this.txtTaxDeductionId.Value = taxDeductBO.TaxDeductionId.ToString();            
        }
        private void LoadGridView()
        {
            EmpTaxDeductionDA da = new EmpTaxDeductionDA();
            List<EmpTaxDeductionSettingBO> files = da.GetEmpAllTaxDeductInfo();

            this.gvGuestHouseService.DataSource = files;
            this.gvGuestHouseService.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
            SetTab("SearchTab");
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
        private void CheckObjectPermission()
        {
            btnEmpTaxDdctnSave.Visible = isSavePermission;
        }
    }
}