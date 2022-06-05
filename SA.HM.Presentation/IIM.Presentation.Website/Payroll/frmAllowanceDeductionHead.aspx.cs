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
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmAllowanceDeductionHead : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                this.isMessageBoxEnable = 2;
                this.lblMessage.Text = "Delete Operation Successfull.";
            }
            if (!IsPostBack)
            {
                this.LoadGridView();
            }
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmAllowanceDeductionHead.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        protected void gvAllowanceDeductionHead_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvAllowanceDeductionHead.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void gvAllowanceDeductionHead_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        [WebMethod]
        public static string DeleteData(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("AllowanceDeductionHead ", "AllowDeductId", sEmpId);
                if (status)
                {
                    result = "success";

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                       EntityTypeEnum.EntityType.AllowanceDeduction.ToString(), sEmpId,
                       ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                       hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.AllowanceDeduction));
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            return result;
        }
        [WebMethod]
        public static AllowanceDeductionHeadBO FillForm(int EditId)
        {
            AllowanceDeductionHeadBO headBO = new AllowanceDeductionHeadBO();
            AllowanceDeductionHeadDA headDA = new AllowanceDeductionHeadDA();
            headBO = headDA.GetAllowanceDeductionHeadInfoByID(EditId);
            return headBO;
        }
        private void Cancel()
        {
            this.txtAllowDeductId.Value = string.Empty;
            this.txtAllowDeductName.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.ddlAllowDeductType.SelectedIndex = 0;
            this.ddlTransactionType.SelectedIndex = 0; 
            this.btnSave.Text = "Save";
            this.txtAllowDeductName.Focus();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }
            lblMessage.Text = string.Empty;


            AllowanceDeductionHeadBO headBO = new AllowanceDeductionHeadBO();
            AllowanceDeductionHeadDA headDA = new AllowanceDeductionHeadDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            headBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
            headBO.AllowDeductName = this.txtAllowDeductName.Text.Trim().ToString();
            headBO.AllowDeductType = ddlAllowDeductType.SelectedItem.Value.ToString();
            headBO.TransactionType = ddlTransactionType.SelectedItem.Value.ToString();

            if (string.IsNullOrWhiteSpace( txtAllowDeductId.Value))
            {

                int tmpUserInfoId = 0;
                headBO.CreatedBy = userInformationBO.UserInfoId;

                Boolean status = headDA.SaveAllowanceDeductionHeadInfo(headBO, out tmpUserInfoId);

                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Saved Operation Successfull.";
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                       EntityTypeEnum.EntityType.AllowanceDeduction.ToString(), tmpUserInfoId,
                       ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                       hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.AllowanceDeduction));

                    this.LoadGridView();
                    this.Cancel();
                }
                else
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "You Can't Save.  " + txtAllowDeductName.Text + "  Already Inserted.";
                    this.LoadGridView();
                    this.Cancel();
                }

            }
            else
            {
                headBO.AllowDeductId = Convert.ToInt32(txtAllowDeductId.Value);
                headBO.LastModifiedBy = userInformationBO.UserInfoId;

                Boolean status = headDA.UpdateAllowanceDeductionHeadInfo(headBO);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Update Operation Successfull.";
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                       EntityTypeEnum.EntityType.AllowanceDeduction.ToString(), headBO.AllowDeductId,
                       ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                       hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.AllowanceDeduction));
                    this.LoadGridView();
                    this.Cancel();
                    this.txtAllowDeductId.Value = "";

                }
            }
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (string.IsNullOrWhiteSpace(this.txtAllowDeductName.Text.Trim()))
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please provide valid Name. ";
                flag = false;
                txtAllowDeductName.Focus();
            }
            else if (ddlAllowDeductType.SelectedIndex==0)
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please select Type.";
                flag = false;
                ddlAllowDeductType.Focus();
            }
            else if (ddlTransactionType.SelectedIndex == 0)
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please select Transaction Type.";
                flag = false;
                ddlTransactionType.Focus();

            }
            else
            {
                this.isMessageBoxEnable = -1;
                this.lblMessage.Text = string.Empty;
            }
            return flag;
        }
        private void LoadGridView()
        {
            this.CheckObjectPermission();
            List<AllowanceDeductionHeadBO> List = new List<AllowanceDeductionHeadBO>();
            AllowanceDeductionHeadDA headDA = new AllowanceDeductionHeadDA();
            List = headDA.GetAllowanceDeductionHeadInfo();
            this.gvAllowanceDeductionHead.DataSource = List;
            this.gvAllowanceDeductionHead.DataBind();
        }
    }
}