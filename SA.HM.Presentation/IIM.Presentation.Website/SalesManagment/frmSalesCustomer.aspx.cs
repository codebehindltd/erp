using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.HMCommon;
using System.Net.Mail;

namespace HotelManagement.Presentation.Website.SalesManagment
{
    public partial class frmSalesCustomer : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadCustomerType();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            SalesCustomerBO customarBO = new SalesCustomerBO();
            SalesCustomerDA customerDA = new SalesCustomerDA();
            customarBO.CustomerType = this.ddlCustomerType.SelectedItem.Text;
            customarBO.Name = txtName.Text;
            //customarBO.Code = txtCode.Text;
            customarBO.Email = txtEmail.Text;
            customarBO.WebAddress = txtWebAddress.Text;
            customarBO.Phone = txtPhone.Text;
            customarBO.Address = txtAddress.Text;

            customarBO.ContactPerson = txtContactPerson.Text;
            customarBO.ContactDesignation = txtContactDesignation.Text;
            customarBO.Department = txtDepartment.Text;
            customarBO.ContactEmail = txtContactEmail.Text;
            customarBO.ContactPhone = txtContactPhone.Text;
            customarBO.ContactFax = txtContactFax.Text;

            customarBO.ContactPerson2 = txtContactPerson2.Text;
            customarBO.ContactDesignation2 = txtContactDesignation2.Text;
            customarBO.Department2 = txtDepartment2.Text;
            customarBO.ContactEmail2 = txtContactEmail2.Text;
            customarBO.ContactPhone2 = txtContactPhone2.Text;
            customarBO.ContactFax2 = txtContactFax2.Text;

            if (string.IsNullOrWhiteSpace(txtCustomerId.Value))
            {
                int tmpCustomerId = 0;
                customarBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = customerDA.SaveSalesCustomerInfo(customarBO, out tmpCustomerId);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    this.isNewAddButtonEnable = 2;
                    lblMessage.Text = "Saved Operation Successfull";
                    this.Cancel();
                }
            }
            else
            {

                customarBO.CustomerId = Convert.ToInt32(txtCustomerId.Value);
                customarBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = customerDA.UpdateSalesCustomerInfo(customarBO);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    this.isNewAddButtonEnable = 2;
                    lblMessage.Text = "Update Operation Successfull";
                    this.Cancel();
                }
            }

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CheckObjectPermission();
            LoadGridView();
            this.SetTab("SearchTab");
        }
        protected void gvSalesCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                ImageButton ImgSelect = (ImageButton)e.Row.FindControl("ImgSelect");
                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";

                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvSalesCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvSalesCustomer.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();

        }
        protected void gvSalesCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdEdit")
            {
                int CustomerId = Convert.ToInt32(e.CommandArgument.ToString());
                SalesCustomerBO customerBO = new SalesCustomerBO();
                SalesCustomerDA customerDA = new SalesCustomerDA();
                customerBO = customerDA.GetSalesCustomerInfoByCustomerId(CustomerId);
                this.FillForm(customerBO);
                txtCustomerId.Value = CustomerId.ToString();
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                int RequisitionId = Convert.ToInt32(e.CommandArgument.ToString());
                string result = string.Empty;

                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("SalesCustomer", "CustomerId", RequisitionId);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    this.lblMessage.Text = "Delete Operation Successfully";
                }

                LoadGridView();

                this.SetTab("SearchTab");
            }
        }
        //************************ User Defined Function ********************//
        private void Cancel()
        {
            this.txtName.Text = string.Empty;
            this.txtCustomerId.Value = string.Empty;
            this.txtAddress.Text = string.Empty;
            this.txtEmail.Text = string.Empty;
            this.txtPhone.Text = string.Empty;
            this.txtWebAddress.Text = string.Empty;
            this.txtContactPerson.Text = string.Empty;
            this.txtContactDesignation.Text = string.Empty;
            this.txtContactEmail.Text = string.Empty;
            this.txtContactFax.Text = string.Empty;
            this.txtContactPhone.Text = string.Empty;
            this.txtSearchCode.Text = string.Empty;
            this.txtSearchName.Text = string.Empty;
            this.txtSearchPhone.Text = string.Empty;
            this.ddlCustomerType.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.txtName.Focus();
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            bool isContactEmail = false;
            if (string.IsNullOrEmpty(txtName.Text))
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please Enter Customer Name";
                flag = false;
            }

            else if (ddlCustomerType.SelectedIndex == 0)
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please Select Customer Type";
                flag = false;
            }
            //else if (string.IsNullOrEmpty(txtPhone.Text))
            //{
            //    this.isMessageBoxEnable = 1;
            //    this.lblMessage.Text = "Please Enter Phone Number";
            //    flag = false;
            //}
            try
            {
                if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    string address = new MailAddress(txtEmail.Text).Address;
                }
                //else
                //{
                //    this.isMessageBoxEnable = 1;
                //    this.lblMessage.Text = "Email is not in correct format";
                //    txtEmail.Focus();
                //    flag = false;
                //}

                if (!string.IsNullOrWhiteSpace(txtContactEmail.Text))
                {
                    isContactEmail = true;
                    string cAddress = new MailAddress(txtContactEmail.Text).Address;
                }
            }
            catch (FormatException)
            {
                if (isContactEmail)
                {
                    this.isMessageBoxEnable = 1;
                    this.lblMessage.Text = "Contact Email is not in correct format";
                    txtContactEmail.Focus();
                    flag = false;
                }
                else
                {
                    this.isMessageBoxEnable = 1;
                    this.lblMessage.Text = "Email is not in correct format";
                    txtEmail.Focus();
                    flag = false;
                }
            }
            return flag;
        }
        private void LoadGridView()
        {
            List<SalesCustomerBO> customerList = new List<SalesCustomerBO>();
            SalesCustomerDA customerDA = new SalesCustomerDA();
            string Name = txtSearchName.Text;
            string Code = txtSearchCode.Text;
            string Phone = txtSearchPhone.Text;
            customerList = customerDA.GetSalesCustomersInfoBySearchCriteria(Name, Code, Phone);
            gvSalesCustomer.DataSource = customerList;
            gvSalesCustomer.DataBind();
        }
        private void LoadCustomerType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("SalesCustomerType", hmUtility.GetDropDownFirstValue());

            this.ddlCustomerType.DataSource = fields;
            this.ddlCustomerType.DataTextField = "FieldValue";
            this.ddlCustomerType.DataValueField = "FieldValue";
            this.ddlCustomerType.DataBind();
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
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmSalesCustomer.ToString());
            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void FillForm(SalesCustomerBO customerBO)
        {
            this.ddlCustomerType.SelectedValue = customerBO.CustomerType;
            this.txtName.Text = customerBO.Name;
            this.txtEmail.Text = customerBO.Email;
            this.txtPhone.Text = customerBO.Phone;
            this.txtAddress.Text = customerBO.Address;
            this.txtWebAddress.Text = customerBO.WebAddress;

            this.txtContactPerson.Text = customerBO.ContactPerson;
            this.txtContactDesignation.Text = customerBO.ContactDesignation;
            this.txtDepartment.Text = customerBO.Department;
            this.txtContactEmail.Text = customerBO.ContactEmail;
            this.txtContactPhone.Text = customerBO.ContactPhone;
            this.txtContactFax.Text = customerBO.ContactFax;

            this.txtContactPerson2.Text = customerBO.ContactPerson2;
            this.txtContactDesignation2.Text = customerBO.ContactDesignation2;
            this.txtDepartment2.Text = customerBO.Department2;
            this.txtContactEmail2.Text = customerBO.ContactEmail2;
            this.txtContactPhone2.Text = customerBO.ContactPhone2;
            this.txtContactFax2.Text = customerBO.ContactFax2;
        }
    }
}