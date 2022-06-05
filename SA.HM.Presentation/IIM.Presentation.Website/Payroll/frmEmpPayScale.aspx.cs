using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpPayScale : System.Web.UI.Page
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
                this.lblMessage.Text = "Data Deleted successfully.";
            }
            if (!IsPostBack)
            {
                LoadGridView();
                LoadGrade();
            }


        }

        public void LoadGridView()
        {
            this.CheckObjectPermission();
            EmpPayScaleBO payScaleBO = new EmpPayScaleBO();
            EmpPayScaleDA payScaleDA = new EmpPayScaleDA();

            List<EmpPayScaleBO> list = new List<EmpPayScaleBO>();
            list = payScaleDA.GetAllPayScaleInfo();
            this.gvEmpPayScale.DataSource = list;
            this.gvEmpPayScale.DataBind();
        }



        protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            EmpPayScaleBO payScaleBO = new EmpPayScaleBO();
            EmpPayScaleDA payScaleDA = new EmpPayScaleDA();
            payScaleBO = payScaleDA.GetPayScaleInfoByGradeId(Int32.Parse(ddlGrade.SelectedItem.Value.ToString()));
            if (payScaleBO.PayScaleId > 0)
            {
                txtBasicAmount.Text = payScaleBO.BasicAmount.ToString();
                txtPayScaleId.Value = payScaleBO.PayScaleId.ToString();
                this.btnSave.Text = "Update";
            }
            else
            {
                txtPayScaleId.Value = "0";
            }
        }


        protected void gvEmpPayScale_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvEmpPayScale.PageIndex = e.NewPageIndex;
            this.LoadGridView();
        }


        protected void gvEmpPayScale_RowDataBound(object sender, GridViewRowEventArgs e)
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
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmEmpPayScale.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void Cancel()
        {

            this.ddlGrade.SelectedValue = "0";
            this.txtBasicAmount.Text = string.Empty;
            this.txtPayScaleId.Value = string.Empty;
            this.btnSave.Text = "Save";
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (string.IsNullOrWhiteSpace(this.txtBasicAmount.Text.Trim()))
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please provide Basic Amount.";
                flag = false;
                txtBasicAmount.Focus();
            }

            else if (ddlGrade.SelectedValue == "0")
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please select Grade. ";
                flag = false;
                ddlGrade.Focus();
            }
            else
            {
                this.isMessageBoxEnable = -1;
                this.lblMessage.Text = string.Empty;
            }
            return flag;
        }
        [WebMethod]
        public static string DeleteData(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("PayrollEmpPayScale", "PayScaleId", sEmpId);
                if (status)
                {
                    result = "success";
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.EmpPayScale.ToString(), sEmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpPayScale));
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Data Deleted Failed.";
                throw ex;
            }
            return result;
        }
        [WebMethod]
        public static EmpPayScaleBO FillForm(int EditId)
        {
            EmpPayScaleBO payScaleBO = new EmpPayScaleBO();
            EmpPayScaleDA payScaleDA = new EmpPayScaleDA();
            payScaleBO = payScaleDA.GetPayScaleByID(EditId);
            return payScaleBO;
        }

        [WebMethod]
        public static EmpPayScaleBO OnDDLChange(string gradeValue)
        {
            EmpPayScaleBO payScaleBO = new EmpPayScaleBO();
            EmpPayScaleDA payScaleDA = new EmpPayScaleDA();
            payScaleBO = payScaleDA.GetPayScaleInfoByGradeId(Int32.Parse(gradeValue.ToString()));
            return payScaleBO;
        }

        public void LoadGrade()
        {
            EmpGradeDA gradeDA = new EmpGradeDA();
            List<EmpGradeBO> fields = new List<EmpGradeBO>();
            fields = gradeDA.GetGradeInfo();
            this.ddlGrade.DataSource = fields;
            this.ddlGrade.DataTextField = "Name";
            this.ddlGrade.DataValueField = "GradeId";
            this.ddlGrade.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---None---";
            this.ddlGrade.Items.Insert(0, item);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }
            lblMessage.Text = string.Empty;
            string aa = txtBasicAmount.Text.ToString();

            EmpPayScaleBO payScaleBO = new EmpPayScaleBO();
            EmpPayScaleDA payScaleDA = new EmpPayScaleDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            payScaleBO.GradeId = Int32.Parse(ddlGrade.SelectedItem.Value.ToString());
            payScaleBO.BasicAmount = Convert.ToDecimal(txtBasicAmount.Text.ToString());
            if (txtPayScaleId.Value == "")
            {
                int tmpUserInfoId = 0;
                payScaleBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = payScaleDA.SavePayScaleInfo(payScaleBO, out tmpUserInfoId);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Saved Operation Successfull.";

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpPayScale.ToString(), tmpUserInfoId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpPayScale));

                    this.LoadGridView();
                    this.ddlGrade.SelectedItem.Value = payScaleBO.GradeId.ToString();
                    this.Cancel();
                }
            }
            else
            {

                payScaleBO.PayScaleId = Convert.ToInt32(txtPayScaleId.Value);
                payScaleBO.LastModifiedBy = userInformationBO.UserInfoId;

                Boolean status = payScaleDA.UpdatePayScaleInfo(payScaleBO);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Update Operation Successfull.";

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpPayScale.ToString(), payScaleBO.PayScaleId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpPayScale));

                    this.LoadGridView();
                    this.ddlGrade.SelectedItem.Value = payScaleBO.GradeId.ToString();
                    this.Cancel();
                }
            }
        }

        protected void Close_Click(object sender, EventArgs e)
        {
            Cancel();
        }
    }
}