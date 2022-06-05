using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpChangePassword : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        private int currentUserId;
        EmployeeBO employeeBO = new EmployeeBO();
        EmployeeDA employeeDA = new EmployeeDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            CheckObjectPermission();
        }
        protected void CheckObjectPermission()
        {
            btnSave.Visible = isUpdatePermission;
        }

        private bool IsFrmValid()
        {

            bool flag = true;
            if (string.IsNullOrWhiteSpace(this.txtOldEmpPassword.Text.Trim()))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "old password.", AlertType.Warning);
                this.txtOldEmpPassword.Focus();
                flag = false;
            }

            else if (string.IsNullOrWhiteSpace(this.txtEmpPassword.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "new password.", AlertType.Warning);
                this.txtEmpPassword.Focus();
                flag = false;
            }
            //else
            //{
            //    this.isMessageBoxEnable = -1;
            //    this.lblMessage.Text = string.Empty;
            //}
            return flag;
        }


        private void Cancel()
        {
            this.txtEmpPassword.Text = string.Empty;
            this.btnSave.Text = "Update";
            this.txtOldEmpPassword.Focus();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }

            if (this.txtEmpPassword.Text.Equals(this.txtEmpConfirmPassword.Text))
            {                
                employeeBO = null;
                employeeBO = employeeDA.GetEmpInformationByEmpCodeNPwd(txtEmpCode.Text.Trim(), this.txtOldEmpPassword.Text);
                if (employeeBO.EmpId > 0)
                {

                    employeeBO.EmpPassword = this.txtEmpPassword.Text;


                    Boolean status = employeeDA.ChangeEmpPassword(employeeBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "successfully changed password.", AlertType.Warning);
                        this.Cancel();
                    }
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, "The old password you gave is incorrect.", AlertType.Warning);
                    this.txtOldEmpPassword.Focus();
                }
            }
        }
    }
}