using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.SalesManagment
{
    public partial class frmSalesService : System.Web.UI.Page
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
                this.ShowHideCurrencyInformation();
                this.LoadCurrency();
                this.LoadCategory();
                this.LoadBandwidthNType();
                HideBandwidthType();
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
            SalesServiceBO serviceBO = new SalesServiceBO();
            SalesServiceDA serviceDA = new SalesServiceDA();
            serviceBO.Name = txtName.Text;
            serviceBO.Code = txtCode.Text;
            serviceBO.Description = txtDescription.Text;
            serviceBO.Frequency = this.ddlFrequency.SelectedItem.Text;
            //serviceBO.PurchasePrice = Convert.ToDecimal(this.txtPurchasePrice.Text);
            serviceBO.PurchasePrice = !string.IsNullOrWhiteSpace(this.txtPurchasePrice.Text) ? Convert.ToDecimal(this.txtPurchasePrice.Text) : 0;
            serviceBO.CategoryId = Int32.Parse(this.ddlCategoryId.SelectedValue);
            serviceBO.SellingLocalCurrencyId = Int32.Parse(this.ddlSellingPriceLocal.SelectedValue);
            //serviceBO.UnitPriceLocal = Convert.ToDecimal(this.txtSellingPriceLocal.Text);
            serviceBO.UnitPriceLocal = !string.IsNullOrWhiteSpace(this.txtSellingPriceLocal.Text) ? Convert.ToDecimal(this.txtSellingPriceLocal.Text) : 0;
            serviceBO.SellingUsdCurrencyId = Int32.Parse(this.ddlSellingPriceUsd.SelectedValue);
            //serviceBO.UnitPriceUsd = Convert.ToDecimal(this.txtSellingPriceUsd.Text);
            serviceBO.UnitPriceUsd = !string.IsNullOrWhiteSpace(this.txtSellingPriceUsd.Text) ? Convert.ToDecimal(this.txtSellingPriceUsd.Text) : 0;

            serviceBO.BandwidthType = Convert.ToInt32(ddlBandwidthType.SelectedValue);
            serviceBO.Bandwidth = Convert.ToInt32(ddlBandwidth.SelectedValue);

            if (string.IsNullOrWhiteSpace(txtServiceId.Value))
            {

                if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 0) == 1)
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Category Name Already Exist";
                    this.txtName.Focus();
                    return;
                }
                else if (DuplicateCheckDynamicaly("Code", this.txtCode.Text, 0) == 1)
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = " Category Code Already Exist";
                    txtCode.Focus();
                    return;
                }
                int tmpServiceId = 0;
                serviceBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = serviceDA.SaveSalesServiceInfo(serviceBO, out tmpServiceId);
                if (status)
                {
                    lblMessage.Text = "Saved Operation Successfull";
                    this.isMessageBoxEnable = 2;
                    this.isNewAddButtonEnable = 2;
                    this.Cancel();
                }
            }
            else
            {

                if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 1) == 1)
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Category Name Already Exist";
                    this.txtName.Focus();
                    return;
                }
                else if (DuplicateCheckDynamicaly("Code", this.txtCode.Text, 1) == 1)
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = " Category Code Already Exist";
                    txtCode.Focus();
                    return;
                }
                serviceBO.ServiceId = Convert.ToInt32(txtServiceId.Value);
                serviceBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = serviceDA.UpdateSalesServiceInfo(serviceBO);
                if (status)
                {
                    lblMessage.Text = "Update Operation Successfull";
                    this.isMessageBoxEnable = 2;
                    this.isNewAddButtonEnable = 2;
                    this.Cancel();
                }
            }

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadSearchResult();
        }
        protected void gvSalesService_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvSalesService_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvSalesService.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();

        }
        protected void gvSalesService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdEdit")
            {
                int serviceId = Convert.ToInt32(e.CommandArgument.ToString());
                SalesServiceBO serviceBO = new SalesServiceBO();
                SalesServiceDA serviceDA = new SalesServiceDA();
                serviceBO = serviceDA.GetSalesServiceInfoByServiceId(serviceId);
                this.FillForm(serviceBO);
                txtServiceId.Value = serviceId.ToString();
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                int serviceId = Convert.ToInt32(e.CommandArgument.ToString());
                string result = string.Empty;

                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("SalesService", "ServiceId", serviceId);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    this.lblMessage.Text = "Delete Operation Successfully";
                }
                //this.SetTab("SearchTab");
                this.LoadSearchResult();
            }
        }
        //************************ User Defined Function ********************//
        private void Cancel()
        {
            this.txtName.Text = string.Empty;
            this.txtCode.Text = string.Empty;
            this.txtServiceId.Value = string.Empty;
            this.txtDescription.Text = string.Empty;
            this.txtSearchCode.Text = string.Empty;
            this.txtSearchName.Text = string.Empty;
            this.ddlCategoryId.SelectedIndex = 0;
            this.ddlFrequency.SelectedIndex = 0;
            this.txtPurchasePrice.Text = string.Empty;
            this.ddlSellingPriceLocal.SelectedIndex = 0;
            this.txtSellingPriceLocal.Text = string.Empty;
            this.ddlSellingPriceUsd.SelectedIndex = 1;
            this.txtSellingPriceUsd.Text = string.Empty;

            ddlBandwidthType.SelectedIndex = 0;
            ddlBandwidth.SelectedIndex = 0;

            this.btnSave.Text = "Save";
            this.SetTab("EntryTab");
            this.txtName.Focus();
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (string.IsNullOrEmpty(txtName.Text))
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please Enter Service Name";
                this.txtName.Focus();
                flag = false;
            }
            else if (string.IsNullOrEmpty(txtCode.Text))
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please Enter Service Code";
                this.txtCode.Focus();
                flag = false;

            }
            else if (this.ddlCategoryId.SelectedIndex == 0)
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please Select Category";
                this.ddlCategoryId.Focus();
                flag = false;

            }
            return flag;
        }
        private void LoadCurrency()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("Currency", hmUtility.GetDropDownFirstValue());
            if (fields != null)
            {
                if (fields.Count > 1)
                {
                    fields.RemoveAt(0);
                }
            }
            this.ddlSellingPriceLocal.DataSource = fields;
            this.ddlSellingPriceLocal.DataTextField = "FieldValue";
            this.ddlSellingPriceLocal.DataValueField = "FieldId";
            this.ddlSellingPriceLocal.DataBind();
            this.ddlSellingPriceLocal.SelectedIndex = 0;
            this.lblSellingPriceLocal.Text = "Selling Price(" + this.ddlSellingPriceLocal.SelectedItem.Text + ")";


            this.ddlSellingPriceUsd.DataSource = fields;
            this.ddlSellingPriceUsd.DataTextField = "FieldValue";
            this.ddlSellingPriceUsd.DataValueField = "FieldId";
            this.ddlSellingPriceUsd.DataBind();
            this.ddlSellingPriceUsd.SelectedIndex = 1;
            this.lblSellingPriceUsd.Text = "Selling Price(" + this.ddlSellingPriceUsd.SelectedItem.Text + ")";
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> categoryList = new List<InvCategoryBO>();
            InvCategoryDA categoryDA = new InvCategoryDA();

            this.ddlCategoryId.DataSource = categoryDA.GetAllInvItemCatagoryInfoByServiceType("Service");
            this.ddlCategoryId.DataTextField = "Name";
            this.ddlCategoryId.DataValueField = "CategoryId";
            this.ddlCategoryId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCategoryId.Items.Insert(0, item);

        }
        private void LoadBandwidthNType()
        {
            SalesBandwidthInfoDA bandwidthDA = new SalesBandwidthInfoDA();

            List<SalesBandwidthInfoBO> bandwidthInfoListForddlBandwidthType = new List<SalesBandwidthInfoBO>();
            bandwidthInfoListForddlBandwidthType = bandwidthDA.GetSalesBandwidthInfoByBandwidthType("BandwidthType");
            ddlBandwidthType.DataSource = bandwidthInfoListForddlBandwidthType;
            ddlBandwidthType.DataTextField = "BandwidthName";
            ddlBandwidthType.DataValueField = "BandwidthInfoId";
            ddlBandwidthType.DataBind();
            ddlBandwidthType.Items.Insert(0, new ListItem("---Please Select---", "0"));

            List<SalesBandwidthInfoBO> bandwidthInfoListddlBandwidth = new List<SalesBandwidthInfoBO>();
            bandwidthInfoListddlBandwidth = bandwidthDA.GetSalesBandwidthInfoByBandwidthType("Bandwidth");
            ddlBandwidth.DataSource = bandwidthInfoListddlBandwidth;
            ddlBandwidth.DataTextField = "BandwidthName";
            ddlBandwidth.DataValueField = "BandwidthInfoId";
            ddlBandwidth.DataBind();
            ddlBandwidth.Items.Insert(0, new ListItem("---Please Select---", "0"));

        }

        private void LoadSearchResult()
        {
            CheckObjectPermission();
            List<SalesServiceBO> serviceList = new List<SalesServiceBO>();
            SalesServiceDA serviceDA = new SalesServiceDA();
            string Name = txtSearchName.Text;
            string Code = txtSearchCode.Text;

            serviceList = serviceDA.GetSaleServicInfoBySearchCriteria(Name, Code);
            gvSalesService.DataSource = serviceList;
            gvSalesService.DataBind();
            this.SetTab("SearchTab");
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
        private void FillForm(SalesServiceBO serviceBO)
        {
            txtCode.Text = serviceBO.Code;
            txtName.Text = serviceBO.Name;
            txtDescription.Text = serviceBO.Description;
            ddlSellingPriceLocal.SelectedValue = serviceBO.SellingLocalCurrencyId.ToString();
            txtSellingPriceLocal.Text = serviceBO.UnitPriceLocal.ToString();
            txtSellingPriceUsd.Text = serviceBO.UnitPriceUsd.ToString();
            ddlSellingPriceUsd.SelectedValue = serviceBO.SellingUsdCurrencyId.ToString();
            txtPurchasePrice.Text = serviceBO.PurchasePrice.ToString();
            txtServiceId.Value = serviceBO.ServiceId.ToString();
            ddlCategoryId.SelectedValue = serviceBO.CategoryId.ToString();
            ddlFrequency.SelectedValue = serviceBO.Frequency.ToString();
            ddlBandwidthType.SelectedValue = serviceBO.BandwidthType.ToString();
            ddlBandwidth.SelectedValue = serviceBO.Bandwidth.ToString();

        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "SalesService";
            string pkFieldName = "ServiceId";
            string pkFieldValue = this.txtServiceId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        private void ShowHideCurrencyInformation()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("CurrencyType", "CurrencyConfiguration");
            if (commonSetupBO.SetupId > 0)
            {
                if (commonSetupBO.SetupValue == "Single")
                {
                    USDCurrencyInfo.Visible = false;
                }
                else
                {
                    USDCurrencyInfo.Visible = true;
                }
            }
        }

        private void HideBandwidthType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();

            fields = commonDA.GetCustomField("IsBandwidthInfoEnable", hmUtility.GetDropDownFirstValue());
            bandwidthTypeHide.Visible = fields[1].FieldValue == "Yes" ? true : false;
        }
    }
}