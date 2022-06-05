using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using Newtonsoft.Json;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmCostCentre : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
            }

            if (!IsPostBack)
            {
                LoadGuestServiceSDCharge();
                LoadCompany();
                LoadGLCompany();
                LoadGridView();
                LoadCostCenterTypeInfo();
                LoadCategoryInfoGridView();
                SoftwareModulePermissionList();
                pnlDefaultStockDeductionLocationInfo.Visible = false;
                LoadPayrollDept();
            }
        }
        protected void gvCostCenter_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCostCenter.PageIndex = e.NewPageIndex;
            CheckObjectPermission();
            LoadGridView();
        }
        protected void gvCostCenter_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Visible = isSavePermission;
                //imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvCostCenter_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int costCenterId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(costCenterId);
                btnSave.Text = "Update";
                SetTab("EntryTab");
                hfEditedId.Value = costCenterId.ToString();
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("CommonCostCenter", "CostCenterId", costCenterId);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                }
                LoadGridView();
                SetTab("SearchTab");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            if (string.IsNullOrWhiteSpace(txtCostCentre.Text))
            {
                isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Cost Center.", AlertType.Warning);
            }
            else
            {
                string CostCentre;
                bool IsExist = false;
                CostCentre = txtCostCentre.Text;
                int IsEditItem = Convert.ToInt32(hfEditedId.Value);
                if (IsEditItem == 0)
                {
                    IsExist = costCentreTabDA.CheckCostCenterName(CostCentre);

                }
                
                if(IsExist)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Cost Centre Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                    txtCostCentre.Focus();
                    return;
                }
                if (ddlCostCenterType.SelectedValue == "FrontOffice")
                {
                    if (DuplicateCheckDynamicaly("CostCenterType", ddlCostCenterType.SelectedValue, 0) > 0)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Front Office" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        ddlCostCenterType.Focus();
                        return;
                    }
                }
                else if (ddlCostCenterType.SelectedValue == "ServiceBill")
                {
                    if (DuplicateCheckDynamicaly("CostCenterType", ddlCostCenterType.SelectedValue, 0) > 0)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Service Bill" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        ddlCostCenterType.Focus();
                        return;
                    }
                }

                if (ddlGLCompanyId.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Accounts Company.", AlertType.Warning);
                    return;
                }

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                costCentreTabBO.CompanyId = Convert.ToInt32(ddlCompanyId.SelectedValue);
                costCentreTabBO.GLCompanyId = Convert.ToInt32(ddlGLCompanyId.SelectedValue);
                costCentreTabBO.CostCenter = txtCostCentre.Text;
                costCentreTabBO.ServiceCharge = !string.IsNullOrWhiteSpace(txtServiceCharge.Text) ? Convert.ToDecimal(txtServiceCharge.Text) : 0;
                costCentreTabBO.VatAmount = !string.IsNullOrWhiteSpace(txtVatAmount.Text) ? Convert.ToDecimal(txtVatAmount.Text) : 0;
                costCentreTabBO.CitySDCharge = !string.IsNullOrWhiteSpace(txtSDCharge.Text) ? Convert.ToDecimal(txtSDCharge.Text) : 0;
                costCentreTabBO.IsVatSChargeInclusive = Convert.ToInt32(ddlIsVatSChargeInclusive.SelectedValue);
                costCentreTabBO.AdditionalChargeType = ddlAdditionalChargeType.SelectedValue;
                costCentreTabBO.AdditionalCharge = !string.IsNullOrWhiteSpace(txtAdditionalChargeAmount.Text) ? Convert.ToDecimal(txtAdditionalChargeAmount.Text) : 0;
                costCentreTabBO.IsVatOnSDCharge = ddlVatEnableOnSDCharge.SelectedValue == "1" ? true : false;
                costCentreTabBO.IsRatePlusPlus = Convert.ToInt32(ddlGuestServiceRateIsPlusPlus.SelectedValue);
                costCentreTabBO.BillNumberPrefix = txtBillNumberPrefix.Text;

                costCentreTabBO.CostCenterType = ddlCostCenterType.SelectedValue;
                costCentreTabBO.PayrollDeptId = Convert.ToInt32(ddlPayrollDept.SelectedValue);
                costCentreTabBO.IsDefaultCostCenter = false;

                costCentreTabBO.IsServiceChargeEnable = cbServiceCharge.Checked == true ? true : false;
                costCentreTabBO.IsCitySDChargeEnable = cbSDCharge.Checked == true ? true : false;
                costCentreTabBO.IsVatEnable = cbVatAmount.Checked == true ? true : false;
                costCentreTabBO.IsAdditionalChargeEnable = cbAdditionalCharge.Checked == true ? true : false;

                if (costCentreTabBO.CostCenterType == "Restaurant")
                {
                    costCentreTabBO.IsRestaurant = true;
                    costCentreTabBO.IsDefaultCostCenter = ddlIsDefaultCostCenter.SelectedValue == "1" ? true : false;
                }
                else if (costCentreTabBO.CostCenterType == "OtherOutlet")
                {
                    costCentreTabBO.IsRestaurant = true;
                    costCentreTabBO.IsDefaultCostCenter = ddlIsDefaultCostCenter.SelectedValue == "1" ? true : false;
                }
                else
                {
                    costCentreTabBO.IsRestaurant = false;
                }
                if (costCentreTabBO.CostCenterType != "Inventory")
                {
                    costCentreTabBO.InvoiceTemplate = Convert.ToInt32(ddlInvoiceTemplate.SelectedValue);
                    costCentreTabBO.BillingStartTime = Convert.ToInt32(ddlBillingTime.SelectedValue);
                }

                costCentreTabBO.IsDiscountEnable = ddlIsDiscountEnable.SelectedValue == "1" ? true : false;

                costCentreTabBO.IsEnableItemAutoDeductFromStore = ddlIsEnableItemAutoDeductFromStore.SelectedValue == "1" ? true : false;

                costCentreTabBO.DefaultView = ddlDefaultView.SelectedValue;
                costCentreTabBO.OutletType = Convert.ToInt32(ddlOutletType.SelectedValue);
                if (btnSave.Text != "Save")
                {
                    costCentreTabBO.DefaultStockLocationId = Convert.ToInt32(ddlDefaultStockDeductionLocationId.SelectedValue);
                }
                else
                {
                    costCentreTabBO.DefaultStockLocationId = 0;
                }

                List<InvItemClassificationCostCenterMappingBO> mappingList = new List<InvItemClassificationCostCenterMappingBO>();

                int rowsKitchenItem = gvCategoryCostCenterInfo.Rows.Count;
                for (int i = 0; i < rowsKitchenItem; i++)
                {
                    CheckBox cb = (CheckBox)gvCategoryCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                    if (cb.Checked == true)
                    {
                        InvItemClassificationCostCenterMappingBO mappingBO = new InvItemClassificationCostCenterMappingBO();
                        Label lbl = (Label)gvCategoryCostCenterInfo.Rows[i].FindControl("lblCategoryId");

                        mappingBO.ClassificationId = Convert.ToInt32(lbl.Text);
                        if (!string.IsNullOrWhiteSpace(hfCostCentreId.Value))
                        {
                            mappingBO.CostCenterId = Convert.ToInt32(hfCostCentreId.Value);
                        }

                        mappingList.Add(mappingBO);
                    }
                }

                if (string.IsNullOrWhiteSpace(hfCostCentreId.Value))
                {
                    int tmpCostCentreId = 0;
                    costCentreTabBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = costCentreTabDA.SaveCostCentreTabInfo(costCentreTabBO, mappingList, out tmpCostCentreId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.CostCenter.ToString(), tmpCostCentreId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CostCenter));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        LoadGridView();
                        Cancel();
                    }
                }
                else
                {
                    costCentreTabBO.CostCenterId = Convert.ToInt32(hfCostCentreId.Value);
                    costCentreTabBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = costCentreTabDA.UpdateCostCentreTabInfo(costCentreTabBO, mappingList);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.CostCenter.ToString(), costCentreTabBO.CostCenterId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CostCenter));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        LoadGridView();
                        Cancel();
                    }
                }
                SetTab("EntryTab");
            }
        }
        //************************ User Defined Function ********************//
        private void LoadPayrollDept()
        {
            CheckObjectPermission();
            HMCommonDA commonDA = new HMCommonDA();

            List<DepartmentBO> departmentBOs = new List<DepartmentBO>();
            DepartmentDA departmentDA = new DepartmentDA();

            departmentBOs = departmentDA.GetDepartmentInfo();
            ddlPayrollDept.DataSource = departmentBOs;
            ddlPayrollDept.DataTextField = "Name";
            ddlPayrollDept.DataValueField = "DepartmentId";
            ddlPayrollDept.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlPayrollDept.Items.Insert(0, item);
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmCostCentre.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "CommonCostCenter";
            string pkFieldName = "CostCenterId";
            string pkFieldValue = hfCostCentreId.Value;
            int IsDuplicate = 0;
            if (!string.IsNullOrWhiteSpace(pkFieldValue))
            {
                isUpdate = 1;
            }
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        private void LoadGuestServiceSDCharge()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO GuestServiceSDChargeBO = new HMCommonSetupBO();
            GuestServiceSDChargeBO = commonSetupDA.GetCommonConfigurationInfo("GuestServiceSDCharge", "GuestServiceSDCharge");
            if (!string.IsNullOrEmpty(GuestServiceSDChargeBO.SetupValue))
            {
                lblGuestServiceSDCharge.Text = GuestServiceSDChargeBO.Description.ToString();
            }
        }
        private void LoadCostCenterTypeInfo()
        {
            CheckObjectPermission();
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("CostCenterType");
            ddlCostCenterType.DataSource = fields;
            ddlCostCenterType.DataTextField = "Description";
            ddlCostCenterType.DataValueField = "FieldValue";
            ddlCostCenterType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCostCenterType.Items.Insert(0, item);
        }
        private void SoftwareModulePermissionList()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.UserInfoId == 1)
            {
                hfCostCenterTypeInformation.Value = "1,2,3";
            }
            else
            {
                if (Session["SoftwareModulePermissionList"] != null)
                {
                    hfCostCenterTypeInformation.Value = Session["SoftwareModulePermissionList"].ToString();
                    if (hfCostCenterTypeInformation.Value == "3")
                    {
                        ddlCostCenterType.Items.Remove(ddlCostCenterType.Items.FindByValue("FrontOffice"));
                    }
                }
                else
                {
                    Session["UserInformationBOSession"] = null;
                    Response.Redirect("Login.aspx");
                }
            }
        }
        private void LoadCompany()
        {
            CompanyDA roomTypeDA = new CompanyDA();
            ddlCompanyId.DataSource = roomTypeDA.GetCompanyInfo();
            ddlCompanyId.DataTextField = "CompanyName";
            ddlCompanyId.DataValueField = "CompanyId";
            ddlCompanyId.DataBind();
        }
        private void LoadGLCompany()
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            var List = entityDA.GetAllGLCompanyInfo();
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            if (List != null)
            {
                if (List.Count == 1)
                {
                    hfIsSingleGLCompany.Value = "1";
                    companyList.Add(List[0]);
                    this.ddlGLCompanyId.DataSource = companyList;
                    this.ddlGLCompanyId.DataTextField = "Name";
                    this.ddlGLCompanyId.DataValueField = "CompanyId";
                    this.ddlGLCompanyId.DataBind();
                }
                else
                {
                    hfIsSingleGLCompany.Value = "2";
                    this.ddlGLCompanyId.DataSource = List;
                    this.ddlGLCompanyId.DataTextField = "Name";
                    this.ddlGLCompanyId.DataValueField = "CompanyId";
                    this.ddlGLCompanyId.DataBind();
                    ListItem itemCompany = new ListItem();
                    itemCompany.Value = "0";
                    itemCompany.Text = hmUtility.GetDropDownFirstValue();
                    this.ddlGLCompanyId.Items.Insert(0, itemCompany);
                }
            }
        }
        private void LoadCategoryInfoGridView()
        {
            CheckObjectPermission();
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("InvItemClassification", hmUtility.GetDropDownFirstValue());

            if (fields != null)
            {
                if (fields.Count > 1)
                {
                    fields.RemoveAt(0);
                }
            }

            gvCategoryCostCenterInfo.DataSource = fields;
            gvCategoryCostCenterInfo.DataBind();
        }
        private void LoadGridView()
        {
            CheckObjectPermission();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> files = costCentreTabDA.GetCostCentreTabInfo();

            gvCostCenter.DataSource = files;
            gvCostCenter.DataBind();
        }
        private void Cancel()
        {
            int rowsKitchenItem = gvCategoryCostCenterInfo.Rows.Count;
            for (int i = 0; i < rowsKitchenItem; i++)
            {
                CheckBox cb = (CheckBox)gvCategoryCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                cb.Checked = false;
            }

            hfCostCentreId.Value = string.Empty;
            ddlCompanyId.SelectedIndex = 0;
            if (hfIsSingleGLCompany.Value == "2")
            {
                ddlGLCompanyId.SelectedValue = "0";
            }
            hfIsSingleGLCompany.Value = "1";
            txtCostCentre.Text = string.Empty;
            ddlIsVatSChargeInclusive.SelectedIndex = 0;
            ddlGuestServiceRateIsPlusPlus.SelectedIndex = 0;
            ddlCostCenterType.SelectedIndex = 0;
            ddlPayrollDept.SelectedIndex = 0;
            txtServiceCharge.Text = string.Empty;
            txtVatAmount.Text = string.Empty;
            txtSDCharge.Text = string.Empty;
            ddlDefaultView.SelectedIndex = 0;
            btnSave.Text = "Save";
            txtCostCentre.Focus();
            pnlDefaultStockDeductionLocationInfo.Visible = false;
            ddlIsDefaultCostCenter.SelectedValue = "0";
            ddlAdditionalChargeType.SelectedIndex = 0;
            txtAdditionalChargeAmount.Text = string.Empty;
            cbServiceCharge.Checked = true;
            cbSDCharge.Checked = true;
            cbVatAmount.Checked = true;
            cbAdditionalCharge.Checked = true;
            txtBillNumberPrefix.Text = string.Empty;
            ddlInvoiceTemplate.SelectedValue = "1";
            ddlBillingTime.SelectedValue = "1";
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
            CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(EditId);

            ddlCompanyId.SelectedValue = costCentreTabBO.CompanyId.ToString();
            ddlGLCompanyId.SelectedValue = costCentreTabBO.GLCompanyId.ToString();
            hfCostCentreId.Value = costCentreTabBO.CostCenterId.ToString();
            txtCostCentre.Text = costCentreTabBO.CostCenter.ToString();
            ddlIsVatSChargeInclusive.SelectedValue = costCentreTabBO.IsVatSChargeInclusive.ToString();
            ddlGuestServiceRateIsPlusPlus.SelectedValue = costCentreTabBO.IsRatePlusPlus.ToString();
            ddlCostCenterType.SelectedValue = costCentreTabBO.CostCenterType.ToString();
            ddlPayrollDept.SelectedValue = costCentreTabBO.PayrollDeptId.ToString();

            txtServiceCharge.Text = costCentreTabBO.ServiceCharge.ToString();
            txtVatAmount.Text = costCentreTabBO.VatAmount.ToString();
            txtSDCharge.Text = costCentreTabBO.CitySDCharge.ToString();
            ddlAdditionalChargeType.SelectedValue = costCentreTabBO.AdditionalChargeType;
            txtAdditionalChargeAmount.Text = costCentreTabBO.AdditionalCharge.ToString();

            ddlGuestServiceRateIsPlusPlus.SelectedValue = costCentreTabBO.IsRatePlusPlus.ToString();
            ddlVatEnableOnSDCharge.SelectedValue = costCentreTabBO.IsVatOnSDCharge ? "1" : "0";

            ddlDefaultView.SelectedValue = costCentreTabBO.DefaultView.ToString();
            if(costCentreTabBO.InvoiceTemplate!= 0)
                ddlInvoiceTemplate.SelectedValue = costCentreTabBO.InvoiceTemplate.ToString();
            if (costCentreTabBO.BillingStartTime != 0)
                ddlBillingTime.SelectedValue = costCentreTabBO.BillingStartTime.ToString();
            ddlOutletType.SelectedValue = costCentreTabBO.OutletType.ToString();
            ddlIsDefaultCostCenter.SelectedValue = (costCentreTabBO.IsDefaultCostCenter == true ? "1" : "0").ToString();
            ddlIsDiscountEnable.SelectedValue = (costCentreTabBO.IsDiscountEnable == true ? "1" : "0").ToString();
            ddlIsEnableItemAutoDeductFromStore.SelectedValue = (costCentreTabBO.IsEnableItemAutoDeductFromStore == true ? "1" : "0").ToString();
            LoadInvItemClassificationCostCenterMappingInfo(EditId);
            pnlDefaultStockDeductionLocationInfo.Visible = true;
            txtBillNumberPrefix.Text = costCentreTabBO.BillNumberPrefix;

            cbServiceCharge.Checked = costCentreTabBO.IsServiceChargeEnable;
            cbSDCharge.Checked = costCentreTabBO.IsCitySDChargeEnable;
            cbVatAmount.Checked = costCentreTabBO.IsVatEnable;
            cbAdditionalCharge.Checked = costCentreTabBO.IsAdditionalChargeEnable;

            LoadDefaultStockDeductionInfo();

            int ItemsCount = ddlDefaultStockDeductionLocationId.Items.Count;
            if (ItemsCount > 1)
            {
                ddlDefaultStockDeductionLocationId.SelectedValue = costCentreTabBO.DefaultStockLocationId.ToString();
            }
        }
        private void LoadDefaultStockDeductionInfo()
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> locationListBO = new List<InvLocationBO>();
            int costCenterId = !string.IsNullOrWhiteSpace(hfCostCentreId.Value) ? Convert.ToInt32(hfCostCentreId.Value) : 0;
            locationListBO = locationDa.GetInvItemLocationByCostCenter(costCenterId);
            if (locationListBO != null)
            {
                ddlDefaultStockDeductionLocationId.DataSource = locationListBO;
                ddlDefaultStockDeductionLocationId.DataTextField = "Name";
                ddlDefaultStockDeductionLocationId.DataValueField = "LocationId";
                ddlDefaultStockDeductionLocationId.DataBind();
                ListItem item1 = new ListItem();
                item1.Value = "0";
                item1.Text = hmUtility.GetDropDownNoneValue();
                ddlDefaultStockDeductionLocationId.Items.Insert(0, item1);
            }
        }
        private void LoadInvItemClassificationCostCenterMappingInfo(int EditId)
        {
            List<InvItemClassificationCostCenterMappingBO> dbList = new List<InvItemClassificationCostCenterMappingBO>();
            InvItemClassificationCostCenterMappingDA mappingDA = new InvItemClassificationCostCenterMappingDA();
            dbList = mappingDA.GetInvItemClassificationCostCenterMappingByCostCenterId(EditId);
            int rowsStockItem = gvCategoryCostCenterInfo.Rows.Count;

            List<InvItemClassificationCostCenterMappingBO> listStockItem = new List<InvItemClassificationCostCenterMappingBO>();
            for (int i = 0; i < rowsStockItem; i++)
            {
                InvItemClassificationCostCenterMappingBO costCenterStockItem = new InvItemClassificationCostCenterMappingBO();
                Label lbl = (Label)gvCategoryCostCenterInfo.Rows[i].FindControl("lblCategoryId");
                costCenterStockItem.ClassificationId = Int32.Parse(lbl.Text);
                listStockItem.Add(costCenterStockItem);
            }


            for (int i = 0; i < listStockItem.Count; i++)
            {
                for (int j = 0; j < dbList.Count; j++)
                {
                    if (listStockItem[i].ClassificationId == dbList[j].ClassificationId)
                    {
                        CheckBox cb = (CheckBox)gvCategoryCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                        cb.Checked = true;
                    }
                }
            }

        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("CosTCentreTab", "CostCentreId", sEmpId);
                if (status)
                {
                    //result = "success";
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Data Deleted Failed.";
                throw ex;
            }

            //return result;
            return rtninf;
        }
        
    }
}