using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Inventory;
using System.Web.Services;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.Web.Script.Services;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.PurchaseManagment;
using System.Reflection;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class frmInvItem : BasePage
    {
        HiddenField innboardMessage;
        protected int isMessageBoxEnable = -1;
        private bool isCurrencySingle = false;
        HMUtility hmUtility = new HMUtility();
        protected int isKitchenItemCostCenterInformationDivEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {

            hfIsRecipeIncludedInInventory.Value = "1";
            IsRecipeIncludedInInventory();
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
            }

            if (!IsPostBack)
            {
                IsItemOriginHide();
                InventoryItemStockType();
                IsInvItemCodeAutoGenerate();

                ShowHideCurrencyInformation();
                LoadServiceType();
                LoadCurrency();
                LoadCategory();
                LoadManufacturer();
                LoadServiceWarranty();
                LoadCostCenterInfoGridView();
                LoadStockBy();
                LoadItemClassificationInfo();
                LoadSupplierInfoGridView();
                SoftwareModulePermissionList();
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomProductId.Value = seatingId.ToString();
                tempProductId.Value = seatingId.ToString();
                LoadCountryList();
                //LoadAttribute();
                IsAttributeItemShow();
            }

            CheckObjectPermission();
        }
        protected void gvProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            lblMessage.Text = string.Empty;
            //gvProduct.PageIndex = e.NewPageIndex;
            LoadGridView();
        }
        protected void gvProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;

                if (isCurrencySingle == true)
                {
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[4].Visible = false;
                }
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isCurrencySingle == true)
                {
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[4].Visible = false;
                }
            }

        }
        //private void LoadAttribute()
        //{
        //    InvItemAttributeViewBO attributeViewBO = new InvItemAttributeViewBO();
        //    InvItemDA DA = new InvItemDA();
        //    attributeViewBO = DA.GetAllInvItemAttributeAndSetupType();
        //    string tablestring = "";
        //    if(attributeViewBO!= null)
        //    {
        //        if (attributeViewBO.InvItemAttributeSetupTypeList.Count > 0)
        //        {
        //            tablestring += "<div class='col-md-12'> <div class='col-md-4'>";                    
        //            tablestring += "<table id='" + attributeViewBO.InvItemAttributeSetupTypeList[0].ToString() + "tbl' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
        //            tablestring += "<th align='left' scope='col'><input id='CheckAll" + attributeViewBO.InvItemAttributeSetupTypeList[0].ToString() + "' type='checkbox'></th><th align='left' scope='col'>" + attributeViewBO.InvItemAttributeSetupTypeList[0].ToString() + "</th></tr>";
        //            for (int i = 0; i < attributeViewBO.InvItemAttributeBOList.Count; i++)
        //            {
        //                if (attributeViewBO.InvItemAttributeBOList[i].SetupType == attributeViewBO.InvItemAttributeSetupTypeList[0])
        //                {
        //                    if (i % 2 == 0)
        //                    {
        //                        tablestring += "<tr id='trdoc" + i + "' style='background-color:#E3EAEB;'>";
        //                    }
        //                    else
        //                    {
        //                        tablestring += "<tr id='trdoc" + i + "' style='background-color:White;'>";
        //                    }
        //                    tablestring += "<td align='left' style='width: 20%'><input id=\"Check\"" + attributeViewBO.InvItemAttributeBOList[i].Id + "\" type='checkbox'>";
        //                    tablestring += "<td align='left' style='width: 80%'>" + attributeViewBO.InvItemAttributeBOList[i].Name + "";
        //                    tablestring += "<td align='left' style='display:none'>" + attributeViewBO.InvItemAttributeBOList[i].Id + "";
        //                    tablestring += "</td>";
        //                    tablestring += "</tr>";
        //                }
        //            }
        //            tablestring += "</table>";
        //            tablestring += "</div>";
        //            if (attributeViewBO.InvItemAttributeSetupTypeList.Count > 1)
        //            {
        //                tablestring += "<div class='col-md-4'>";
        //                tablestring += "<table id='" + attributeViewBO.InvItemAttributeSetupTypeList[1].ToString() + "tbl' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
        //                tablestring += "<th align='left' scope='col'><input id='CheckAll" + attributeViewBO.InvItemAttributeSetupTypeList[1].ToString() + "' type='checkbox'></th><th align='left' scope='col'>" + attributeViewBO.InvItemAttributeSetupTypeList[1].ToString() + "</th></tr>"; for (int i = 0; i < attributeViewBO.InvItemAttributeBOList.Count; i++)
        //                {
        //                    if (attributeViewBO.InvItemAttributeBOList[i].SetupType == attributeViewBO.InvItemAttributeSetupTypeList[1])
        //                    {
        //                        if (i % 2 == 0)
        //                        {
        //                            tablestring += "<tr id='trdoc" + i + "' style='background-color:#E3EAEB;'>";
        //                        }
        //                        else
        //                        {
        //                            tablestring += "<tr id='trdoc" + i + "' style='background-color:White;'>";
        //                        }
        //                        tablestring += "<td align='left' style='width: 20%'><input id=\"Check\"" + attributeViewBO.InvItemAttributeBOList[i].Id + "\" type='checkbox'>";
        //                        tablestring += "<td align='left' style='width: 80%'>" + attributeViewBO.InvItemAttributeBOList[i].Name + "";
        //                        tablestring += "<td align='left' style='display:none'>" + attributeViewBO.InvItemAttributeBOList[i].Id + "";
        //                        tablestring += "</td>";
        //                        tablestring += "</tr>";
        //                    }
        //                }
        //                tablestring += "</table>";
        //                tablestring += "</div>";
        //                //tablestring += "</div>";
        //            }

        //            if (attributeViewBO.InvItemAttributeSetupTypeList.Count > 2)
        //            {
        //                tablestring += "<div class='col-md-4'>";
        //                tablestring += "<table id='" + attributeViewBO.InvItemAttributeSetupTypeList[2].ToString() + "tbl' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
        //                tablestring += "<th align='left' scope='col'><input id='CheckAll" + attributeViewBO.InvItemAttributeSetupTypeList[2].ToString() + "' type='checkbox'></th><th align='left' scope='col'>" + attributeViewBO.InvItemAttributeSetupTypeList[2].ToString() + "</th></tr>";
        //                for (int i = 0; i < attributeViewBO.InvItemAttributeBOList.Count; i++)
        //                {
        //                    if (attributeViewBO.InvItemAttributeBOList[i].SetupType == attributeViewBO.InvItemAttributeSetupTypeList[2])
        //                    {
        //                        if (i % 2 == 0)
        //                        {
        //                            tablestring += "<tr id='trdoc" + i + "' style='background-color:#E3EAEB;'>";
        //                        }
        //                        else
        //                        {
        //                            tablestring += "<tr id='trdoc" + i + "' style='background-color:White;'>";
        //                        }
        //                        tablestring += "<td align='left' style='width: 20%'><input id=\"Check\"" + attributeViewBO.InvItemAttributeBOList[i].Id + "\" type='checkbox'>";
        //                        tablestring += "<td align='left' style='width: 80%'>" + attributeViewBO.InvItemAttributeBOList[i].Name + "";
        //                        tablestring += "<td align='left' style='display:none'>" + attributeViewBO.InvItemAttributeBOList[i].Id + "";
        //                        tablestring += "</td>";
        //                        tablestring += "</tr>";
        //                    }
        //                }
        //                tablestring += "</table>";
        //                tablestring += "</div>";
        //                tablestring += "</div>";
        //            }

        //            //if (attributeViewBO.InvItemAttributeSetupTypeList.Count > 2)
        //            //{
        //            //    tablestring += "<div class='col-md-12'> <div class='col-md-12'>";
        //            //    tablestring += "<table id='" + attributeViewBO.InvItemAttributeSetupTypeList[2].ToString() + "tbl' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
        //            //    tablestring += "<th align='left' scope='col'><input id='CheckAll" + attributeViewBO.InvItemAttributeSetupTypeList[2].ToString() + "' type='checkbox'></th><th align='left' scope='col'>" + attributeViewBO.InvItemAttributeSetupTypeList[2].ToString() + "</th></tr>";
        //            //    for (int i = 0; i < attributeViewBO.InvItemAttributeBOList.Count; i++)
        //            //    {
        //            //        if (attributeViewBO.InvItemAttributeBOList[i].SetupType == attributeViewBO.InvItemAttributeSetupTypeList[2])
        //            //        {
        //            //            if (i % 2 == 0)
        //            //            {
        //            //                tablestring += "<tr id='trdoc" + i + "' style='background-color:#E3EAEB;'>";
        //            //            }
        //            //            else
        //            //            {
        //            //                tablestring += "<tr id='trdoc" + i + "' style='background-color:White;'>";
        //            //            }
        //            //            tablestring += "<td align='left' style='width: 20%'><input id=\"Check\"" + attributeViewBO.InvItemAttributeBOList[i].Id + "\" type='checkbox'>";
        //            //            tablestring += "<td align='left' style='width: 80%'>" + attributeViewBO.InvItemAttributeBOList[i].Name + "";
        //            //            tablestring += "<td align='left' style='display:none'>" + attributeViewBO.InvItemAttributeBOList[i].Id + "";
        //            //            tablestring += "</td>";
        //            //            tablestring += "</tr>";
        //            //        }
        //            //    }
        //            //    tablestring += "</table>";
        //            //    tablestring += "</div>";
        //            //    tablestring += "</div>";
        //            //}
        //        }
        //    }

        //    AttributeTableDiv.InnerHtml = tablestring;
        //}
        private void LoadCountryList()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CountriesBO> countryList = commonDA.GetAllCountries();
            this.ddlOrigin.DataSource = countryList;
            this.ddlOrigin.DataTextField = "CountryName";
            this.ddlOrigin.DataValueField = "CountryId";
            this.ddlOrigin.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlOrigin.Items.Insert(0, item);

            string bangladesh = "19";
            // ddlOrigin.SelectedValue = bangladesh;

        }
        //protected void btnAddAttribute_Click(object sender, EventArgs e)
        //{

        //    int dynamicEducationId = 0;
        //    List<EmpEducationBO> EmpEducationListBO = Session["EmpEducationList"] == null ? new List<EmpEducationBO>() : Session["EmpEducationList"] as List<EmpEducationBO>;

        //    if (!string.IsNullOrWhiteSpace(hfEducationId.Text))
        //        dynamicEducationId = Convert.ToInt32(hfEducationId.Text);

        //    EmpEducationBO detailBO = dynamicEducationId == 0 ? new EmpEducationBO() : EmpEducationListBO.Where(x => x.EducationId == dynamicEducationId).FirstOrDefault();
        //    if (EmpEducationListBO.Contains(detailBO))
        //        EmpEducationListBO.Remove(detailBO);

        //    if (ddlExamLevel.SelectedIndex != 0)
        //    {
        //        detailBO.LevelId = Convert.ToInt32(ddlExamLevel.SelectedValue);
        //    }
        //    detailBO.ExamName = txtExamName.Text;
        //    detailBO.InstituteName = txtInstituteName.Text;
        //    detailBO.SubjectName = txtSubjectName.Text;
        //    detailBO.PassYear = txtPassYear.Text;
        //    detailBO.PassClass = txtPassClass.Text;
        //    detailBO.EducationId = dynamicEducationId == 0 ? EmpEducationListBO.Count + 1 : dynamicEducationId;
        //    EmpEducationListBO.Add(detailBO);
        //    Session["EmpEducationList"] = EmpEducationListBO;
        //    gvEmpEducation.DataSource = Session["EmpEducationList"] as List<EmpEducationBO>;
        //    gvEmpEducation.DataBind();
        //    ClearEmpEducation();

        //    txtGoToScrolling.Text = "EducationInformation";
        //    SetTab("EducationTab");
        //}
        protected void gvProduct_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _productId;
            if (e.CommandName == "CmdEdit")
            {
                try
                {
                    Cancel();
                    _productId = Convert.ToInt32(e.CommandArgument.ToString());
                    FillForm(_productId);
                    SetTab("Entry");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (e.CommandName == "CmdDelete")
            {
                try
                {
                    _productId = Convert.ToInt32(e.CommandArgument.ToString());
                    DeleteData(_productId);

                    SetTab("Search");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            LoadGridView();
        }
        protected void gvKitchenItemCostCenterInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblCostCentreIdValue = (Label)e.Row.FindControl("lblCostCentreId");
                int costCenterId = Convert.ToInt32(lblCostCentreIdValue.Text);

                RestaurantKitchenDA entityDA = new RestaurantKitchenDA();
                List<RestaurantKitchenBO> entityBOList = new List<RestaurantKitchenBO>();
                entityBOList = entityDA.GetRestaurantKitchenInfoByCostCenterId(costCenterId);

                DropDownList ddlKitchen = (e.Row.FindControl("ddlKitchen") as DropDownList);

                ddlKitchen.DataSource = entityBOList;
                ddlKitchen.DataTextField = "KitchenName";
                ddlKitchen.DataValueField = "KitchenId";
                ddlKitchen.DataBind();

                if (entityBOList.Count > 1)
                {
                    ListItem itemKitchen = new ListItem();
                    itemKitchen.Value = "0";
                    itemKitchen.Text = hmUtility.GetDropDownFirstValue();
                    ddlKitchen.Items.Insert(0, itemKitchen);
                }
                else if (entityBOList.Count == 0)
                {
                    ListItem itemKitchen = new ListItem();
                    itemKitchen.Value = "0";
                    itemKitchen.Text = hmUtility.GetDropDownNoneValue();
                    ddlKitchen.Items.Insert(0, itemKitchen);
                }
            }
        }
        protected void gvStockItemCostCenterInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //int costCenterId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdBarCodePrintInfo")
            {
                PrintBarcodeLabel.PrintBarcode128(txtCode.Text, txtName.Text, 0);
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            Cancel();
            HttpContext.Current.Session.Remove("EditedItemId");
        }
        protected void btnEditInServer_Click(object sender, EventArgs e)
        {
            int itemId = 0;
            ddlSetupType.SelectedValue = "0";
            if (hfEditedItemId.Value != "")
            {
                itemId = Convert.ToInt32(hfEditedItemId.Value);
            }

            int rowsKitchenItem = gvKitchenItemCostCenterInfo.Rows.Count;
            List<InvItemCostCenterMappingBO> listKitchenItem = new List<InvItemCostCenterMappingBO>();
            for (int i = 0; i < rowsKitchenItem; i++)
            {
                CheckBox cbKitchen = (CheckBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                cbKitchen.Checked = false;

                TextBox tbMinimumStockLevel = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtMinimumStockLevel");
                TextBox tbUnitPriceLocal = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceLocal");
                TextBox tbUnitPriceUsd = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceUsd");

                DropDownList ddlDiscountType = (DropDownList)gvKitchenItemCostCenterInfo.Rows[i].FindControl("ddlDiscountType");
                ddlDiscountType.SelectedValue = "Fixed";
                TextBox tbDiscountAmount = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtDiscountAmount");
                tbDiscountAmount.Text = string.Empty;

                tbMinimumStockLevel.Text = string.Empty;
                tbUnitPriceLocal.Text = string.Empty;
                tbUnitPriceUsd.Text = string.Empty;
            }

            int rowsStockItem = gvStockItemCostCenterInfo.Rows.Count;
            List<InvItemCostCenterMappingBO> listStockItem = new List<InvItemCostCenterMappingBO>();
            for (int i = 0; i < rowsStockItem; i++)
            {
                CheckBox cbStock = (CheckBox)gvStockItemCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                cbStock.Checked = false;

                TextBox tbMinimumStockLevel = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtMinimumStockLevel");
                TextBox tbUnitPriceLocal = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceLocal");
                TextBox tbUnitPriceUsd = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceUsd");

                DropDownList ddlDiscountType = (DropDownList)gvStockItemCostCenterInfo.Rows[i].FindControl("ddlDiscountType");
                ddlDiscountType.SelectedValue = "Fixed";
                TextBox tbDiscountAmount = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtDiscountAmount");
                tbDiscountAmount.Text = string.Empty;

                tbMinimumStockLevel.Text = string.Empty;
                tbUnitPriceLocal.Text = string.Empty;
                tbUnitPriceUsd.Text = string.Empty;
            }

            FillForm(itemId);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
            SetTab("Search");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFrmValid())
                {
                    GenerateRecipeItemTable();
                    return;
                }

                if (DuplicateCheckDynamicaly("Code", this.txtCode.Text, 0) > 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Code/Model" + AlertMessage.DuplicateValidation, AlertType.Warning);
                    this.txtCode.Focus();
                    return;
                }

                int OwnerIdForDocuments = 0;

                InvItemBO productBO = new InvItemBO();
                InvItemDA productDA = new InvItemDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                productBO.ItemAttribute = hfItemAttributeList.Value;
                //productBO.ItemType = "InventoryItem";
                productBO.Code = txtCode.Text;
                productBO.Description = txtDescription.Text;
                productBO.Name = txtName.Text;
                productBO.DisplayName = txtDisplayName.Text;
                productBO.RandomItemId = Int32.Parse(RandomProductId.Value);
                //productBO.MinimumStockLevel = !string.IsNullOrWhiteSpace(txtMinimumStockLevel.Text) ? Convert.ToDecimal(txtMinimumStockLevel.Text) : 0;
                productBO.PurchasePrice = !string.IsNullOrWhiteSpace(txtPurchasePrice.Text) ? Convert.ToDecimal(txtPurchasePrice.Text) : 0;
                productBO.SellingLocalCurrencyId = Int32.Parse(ddlSellingPriceLocal.SelectedValue);
                productBO.UnitPriceLocal = !string.IsNullOrWhiteSpace(txtSellingPriceLocal.Text) ? Convert.ToDecimal(txtSellingPriceLocal.Text) : 0;
                productBO.SellingUsdCurrencyId = Int32.Parse(ddlSellingPriceUsd.SelectedValue);
                productBO.UnitPriceUsd = !string.IsNullOrWhiteSpace(txtSellingPriceUsd.Text) ? Convert.ToDecimal(txtSellingPriceUsd.Text) : 0;
                productBO.ManufacturerId = Convert.ToInt32(ddlManufacturer.SelectedValue);
                productBO.CategoryId = Int32.Parse(ddlCategoryId.SelectedValue);
                productBO.ProductType = ddlProductType.SelectedItem.Text;
                productBO.StockType = ddlStockType.SelectedValue;

                productBO.Model = txtModelNumber.Text;
                productBO.CountryId = Int32.Parse(ddlOrigin.SelectedValue);

                if (productBO.StockType == "StockItem")
                {
                    productBO.ItemType = "IndividualItem";
                }
                else
                {
                    productBO.ItemType = ddlItemType.SelectedValue.ToString();
                }

                productBO.ServiceWarranty = Convert.ToInt32(ddlServiceWarranty.SelectedValue);
                productBO.StockBy = Int32.Parse(ddlStockBy.SelectedValue);
                productBO.SalesStockBy = Int32.Parse(ddlSalesStockBy.SelectedValue);
                productBO.ClassificationId = Convert.ToInt32(ddlClassification.SelectedValue);
                productBO.IsCustomerItem = ddlIsCustomerItem.SelectedValue == "1" ? true : false;
                productBO.IsSupplierItem = ddlIsSupplierItem.SelectedValue == "1" ? true : false;
                productBO.IsItemEditable = ddlIsItemEditable.SelectedValue == "1" ? true : false;
                productBO.IsAttributeItem = ddlIsAttributeItem.SelectedValue == "1" ? true : false;

                List<InvItemSuppierMappingBO> supplierList = new List<InvItemSuppierMappingBO>();

                if (productBO.IsSupplierItem)
                {
                    int rowsSupplierInfo = gvSupplierInfo.Rows.Count;
                    for (int i = 0; i < rowsSupplierInfo; i++)
                    {
                        CheckBox cb = (CheckBox)gvSupplierInfo.Rows[i].FindControl("chkIsSavePermission");
                        if (cb.Checked == true)
                        {
                            InvItemSuppierMappingBO invItemSuppierMappingBO = new InvItemSuppierMappingBO();
                            Label lbl = (Label)gvSupplierInfo.Rows[i].FindControl("lblSupplierId");

                            invItemSuppierMappingBO.SupplierId = Convert.ToInt32(lbl.Text);
                            if (!string.IsNullOrEmpty(txtProductId.Value))
                            {
                                invItemSuppierMappingBO.ItemId = Int32.Parse(txtProductId.Value);
                            }
                            else
                            {
                                invItemSuppierMappingBO.ItemId = 0;
                            }
                            supplierList.Add(invItemSuppierMappingBO);
                        }
                    }
                }

                productBO.AdjustmentFrequency = ddlAccessFrequancy.SelectedValue;

                List<InvItemCostCenterMappingBO> costCenterList = new List<InvItemCostCenterMappingBO>();

                if (ddlStockType.SelectedValue == "KitchenItem")
                {
                    int rowsKitchenItem = gvKitchenItemCostCenterInfo.Rows.Count;
                    for (int i = 0; i < rowsKitchenItem; i++)
                    {
                        CheckBox cb = (CheckBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                        if (cb.Checked == true)
                        {
                            InvItemCostCenterMappingBO costCenter = new InvItemCostCenterMappingBO();
                            Label lbl = (Label)gvKitchenItemCostCenterInfo.Rows[i].FindControl("lblCostCentreId");
                            Label lblMap = (Label)gvKitchenItemCostCenterInfo.Rows[i].FindControl("lblMappingId");

                            int costCentreId = !string.IsNullOrWhiteSpace(lbl.Text) ? Convert.ToInt32(lbl.Text) : 0;
                            int mappingId = !string.IsNullOrWhiteSpace(lblMap.Text) ? Convert.ToInt32(lblMap.Text) : 0;

                            TextBox tbMinimumStockLevel = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtMinimumStockLevel");
                            TextBox tbUnitPriceLocal = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceLocal");
                            TextBox tbUnitPriceUsd = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceUsd");
                            DropDownList ddlDiscountType = (DropDownList)gvKitchenItemCostCenterInfo.Rows[i].FindControl("ddlDiscountType");
                            costCenter.DiscountType = ddlDiscountType.SelectedValue;
                            TextBox tbDiscountAmount = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtDiscountAmount");
                            costCenter.DiscountAmount = 0;
                            costCenter.MinimumStockLevel = !string.IsNullOrWhiteSpace(tbMinimumStockLevel.Text) ? Convert.ToDecimal(tbMinimumStockLevel.Text) : 0;
                            costCenter.UnitPriceLocal = !string.IsNullOrWhiteSpace(tbUnitPriceLocal.Text) ? Convert.ToDecimal(tbUnitPriceLocal.Text) : 0;
                            costCenter.UnitPriceUsd = !string.IsNullOrWhiteSpace(tbUnitPriceUsd.Text) ? Convert.ToDecimal(tbUnitPriceUsd.Text) : 0;

                            TextBox tbServiceCharge = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtServiceCharge");
                            TextBox tbSDCharge = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtSDCharge");
                            TextBox tbVatAmount = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtVatAmount");
                            TextBox tbAdditionalCharge = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtAdditionalCharge");

                            costCenter.ServiceCharge = !string.IsNullOrWhiteSpace(tbServiceCharge.Text) ? Convert.ToDecimal(tbServiceCharge.Text) : 0;
                            costCenter.SDCharge = !string.IsNullOrWhiteSpace(tbSDCharge.Text) ? Convert.ToDecimal(tbSDCharge.Text) : 0;
                            costCenter.VatAmount = !string.IsNullOrWhiteSpace(tbVatAmount.Text) ? Convert.ToDecimal(tbVatAmount.Text) : 0;
                            costCenter.AdditionalCharge = !string.IsNullOrWhiteSpace(tbAdditionalCharge.Text) ? Convert.ToDecimal(tbAdditionalCharge.Text) : 0;

                            costCenter.CostCenterId = costCentreId;
                            costCenter.MappingId = mappingId;

                            if (!string.IsNullOrEmpty(txtProductId.Value))
                            {
                                costCenter.ItemId = Int32.Parse(txtProductId.Value);
                            }
                            else
                            {
                                costCenter.ItemId = 0;
                            }

                            DropDownList ddlKitchen = (DropDownList)gvKitchenItemCostCenterInfo.Rows[i].FindControl("ddlKitchen");
                            costCenter.KitchenId = Convert.ToInt32(ddlKitchen.SelectedValue);
                            costCenterList.Add(costCenter);
                        }
                    }
                }
                else
                {
                    int rowsStockItem = gvStockItemCostCenterInfo.Rows.Count;
                    for (int i = 0; i < rowsStockItem; i++)
                    {
                        CheckBox cb = (CheckBox)gvStockItemCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                        if (cb.Checked == true)
                        {
                            InvItemCostCenterMappingBO costCenter = new InvItemCostCenterMappingBO();
                            Label lbl = (Label)gvStockItemCostCenterInfo.Rows[i].FindControl("lblCostCentreId");
                            Label lblMap = (Label)gvStockItemCostCenterInfo.Rows[i].FindControl("lblMappingId");

                            int costCentreId = !string.IsNullOrWhiteSpace(lbl.Text) ? Convert.ToInt32(lbl.Text) : 0;
                            int mappingId = !string.IsNullOrWhiteSpace(lblMap.Text) ? Convert.ToInt32(lblMap.Text) : 0;

                            TextBox tbMinimumStockLevel = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtMinimumStockLevel");
                            TextBox tbUnitPriceLocal = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceLocal");
                            TextBox tbUnitPriceUsd = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceUsd");

                            DropDownList ddlDiscountType = (DropDownList)gvStockItemCostCenterInfo.Rows[i].FindControl("ddlDiscountType");
                            costCenter.DiscountType = ddlDiscountType.SelectedValue;
                            TextBox tbDiscountAmount = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtDiscountAmount");
                            costCenter.DiscountAmount = 0;

                            costCenter.MinimumStockLevel = !string.IsNullOrWhiteSpace(tbMinimumStockLevel.Text) ? Convert.ToDecimal(tbMinimumStockLevel.Text) : 0;
                            costCenter.UnitPriceLocal = !string.IsNullOrWhiteSpace(tbUnitPriceLocal.Text) ? Convert.ToDecimal(tbUnitPriceLocal.Text) : 0;
                            costCenter.UnitPriceUsd = !string.IsNullOrWhiteSpace(tbUnitPriceUsd.Text) ? Convert.ToDecimal(tbUnitPriceUsd.Text) : 0;

                            TextBox tbServiceCharge = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtServiceCharge");
                            TextBox tbSDCharge = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtSDCharge");
                            TextBox tbVatAmount = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtVatAmount");
                            TextBox tbAdditionalCharge = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtAdditionalCharge");

                            costCenter.ServiceCharge = !string.IsNullOrWhiteSpace(tbServiceCharge.Text) ? Convert.ToDecimal(tbServiceCharge.Text) : 0;
                            costCenter.SDCharge = !string.IsNullOrWhiteSpace(tbSDCharge.Text) ? Convert.ToDecimal(tbSDCharge.Text) : 0;
                            costCenter.VatAmount = !string.IsNullOrWhiteSpace(tbVatAmount.Text) ? Convert.ToDecimal(tbVatAmount.Text) : 0;
                            costCenter.AdditionalCharge = !string.IsNullOrWhiteSpace(tbAdditionalCharge.Text) ? Convert.ToDecimal(tbAdditionalCharge.Text) : 0;

                            costCenter.CostCenterId = costCentreId;
                            costCenter.MappingId = mappingId;

                            if (!string.IsNullOrEmpty(txtProductId.Value))
                            {
                                costCenter.ItemId = Int32.Parse(txtProductId.Value);
                            }
                            else
                            {
                                costCenter.ItemId = 0;
                            }

                            costCenter.KitchenId = 0;
                            costCenterList.Add(costCenter);
                        }
                    }
                }

                List<RestaurantRecipeDetailBO> receipeList = new List<RestaurantRecipeDetailBO>();
                List<RestaurantRecipeDetailBO> deleteReceipeList = new List<RestaurantRecipeDetailBO>();

                // IsRecipe
                receipeList = JsonConvert.DeserializeObject<List<RestaurantRecipeDetailBO>>(hfSaveObj.Value);
                deleteReceipeList = JsonConvert.DeserializeObject<List<RestaurantRecipeDetailBO>>(hfDeleteObj.Value);

                if (hfIsReceipeExist.Value == "1")
                    productBO.IsRecipe = true;
                else
                    productBO.IsRecipe = false;

                if (string.IsNullOrWhiteSpace(txtProductId.Value))
                {
                    if (InvItemDuplicateChecking(0, 0, Convert.ToInt32(ddlCategoryId.SelectedValue), txtName.Text) == 1)
                    {
                        CommonHelper.AlertInfo("Product Name Already Exist.", AlertType.Warning);
                        txtName.Focus();
                        return;
                    }

                    int tmpProductId = 0;
                    productBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = productDA.SaveInvItemInfo(productBO, supplierList, costCenterList, receipeList, out tmpProductId);
                    if (status)
                    {

                        OwnerIdForDocuments = tmpProductId;
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);

                        HMCommonDA hmCommonDA = new HMCommonDA();

                        DocumentsDA documentsDA = new DocumentsDA();
                        string s = hfDeletedDoc.Value;
                        string[] DeletedDocList = s.Split(',');
                        for (int i = 0; i < DeletedDocList.Length; i++)
                        {
                            DeletedDocList[i] = DeletedDocList[i].Trim();
                            if (DeletedDocList[i] != "")
                            {
                                Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
                            }
                        }
                        Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(RandomProductId.Value));

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.InvItem.ToString(), tmpProductId,
                       ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvItem));
                        Cancel();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                        GenerateRecipeItemTable();
                    }
                }
                else
                {
                    if (InvItemDuplicateChecking(1, Convert.ToInt32(txtProductId.Value), Convert.ToInt32(ddlCategoryId.SelectedValue), txtName.Text) == 1)
                    {
                        CommonHelper.AlertInfo("Product Name Already Exist.", AlertType.Warning);
                        txtName.Focus();
                        return;
                    }

                    productBO.ItemId = Convert.ToInt32(txtProductId.Value);
                    productBO.LastModifiedBy = userInformationBO.UserInfoId;

                    //// //-----Delete Item CostCenter Mapping Related Information ---------------
                    //InvItemDA invItemDA = new InvItemDA();
                    //Boolean deleteMappingInfo = invItemDA.DeleteInvItemCostCenterMappingInfo(productBO.ItemId, hfExistingStockType.Value, productBO.StockType);

                    Boolean status = productDA.UpdateInvItemInfo(productBO, supplierList, costCenterList, deleteReceipeList, receipeList);
                    if (status)
                    {
                        OwnerIdForDocuments = productBO.ItemId;

                        HMCommonDA hmCommonDA = new HMCommonDA();

                        DocumentsDA documentsDA = new DocumentsDA();
                        string s = hfDeletedDoc.Value;
                        if (!string.IsNullOrEmpty(s))
                        {
                            string[] DeletedDocList = s.Split(',');
                            for (int i = 0; i < DeletedDocList.Length; i++)
                            {
                                DeletedDocList[i] = DeletedDocList[i].Trim();
                                Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
                            }
                        }
                        Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(RandomProductId.Value));

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.InvItem.ToString(), productBO.ItemId,
                        ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvItem));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);

                        Random rd = new Random();
                        int seatingId = rd.Next(100000, 999999);
                        RandomProductId.Value = seatingId.ToString();
                        tempProductId.Value = seatingId.ToString();
                        LoadGridView();
                        Cancel();

                        HttpContext.Current.Session.Remove("EditedItemId");
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                }



            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error + "  " + CommonHelper.ExceptionErrorMessage(ex), AlertType.Error);

                if (string.IsNullOrWhiteSpace(txtProductId.Value))
                {
                    GenerateRecipeItemTable();
                }
            }
        }
        //************************ User Defined Function ********************//
        private void InventoryItemStockType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("InventoryItemStockType");
            this.hfIngredientsText.Value = fields.Where(x => x.FieldValue == "IngredientText").FirstOrDefault().Description.ToString();
            if (fields != null)
            {
                ddlStockType.DataSource = fields.Where(x => x.FieldValue != "IngredientText").ToList();
                ddlStockType.DataTextField = "Description";
                ddlStockType.DataValueField = "FieldValue";
                ddlStockType.DataBind();

                ListItem item = new ListItem();
                item.Value = "0";
                item.Text = hmUtility.GetDropDownFirstValue();
                ddlStockType.Items.Insert(0, item);
            }
        }
        private void LoadInventoryIngredientText()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("InventoryItemStockType");
            this.hfIngredientsText.Value = fields.Where(x => x.FieldValue == "IngredientText").FirstOrDefault().Description.ToString();
        }
        private void Cancel()
        {
            CommonHelper.ClearControl(Master.FindControl("ContentPlaceHolder1"));
            btnSave.Text = "Save";

            int rowsSupplierInfo = gvSupplierInfo.Rows.Count;
            for (int i = 0; i < rowsSupplierInfo; i++)
            {
                CheckBox cb = (CheckBox)gvSupplierInfo.Rows[i].FindControl("chkIsSavePermission");
                Label lbl = (Label)gvSupplierInfo.Rows[i].FindControl("lblSupplierId");

                lbl.Text = string.Empty;
                cb.Checked = false;
            }

            Random rd = new Random();
            int seatingId = rd.Next(100000, 999999);
            RandomProductId.Value = seatingId.ToString();
            tempProductId.Value = seatingId.ToString();

            int rowsStockItem = gvStockItemCostCenterInfo.Rows.Count;
            List<InvItemCostCenterMappingBO> listStockItem = new List<InvItemCostCenterMappingBO>();
            for (int i = 0; i < rowsStockItem; i++)
            {
                CheckBox cbStock = (CheckBox)gvStockItemCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                cbStock.Checked = false;

                TextBox tbMinimumStockLevel = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtMinimumStockLevel");
                TextBox tbUnitPriceLocal = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceLocal");
                TextBox tbUnitPriceUsd = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceUsd");

                DropDownList ddlDiscountType = (DropDownList)gvStockItemCostCenterInfo.Rows[i].FindControl("ddlDiscountType");
                ddlDiscountType.SelectedValue = "Fixed";
                TextBox tbDiscountAmount = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtDiscountAmount");
                tbDiscountAmount.Text = string.Empty;

                Label lblMap = (Label)gvStockItemCostCenterInfo.Rows[i].FindControl("lblMappingId");

                lblMap.Text = string.Empty;
                tbMinimumStockLevel.Text = string.Empty;
                tbUnitPriceLocal.Text = string.Empty;
                tbUnitPriceUsd.Text = string.Empty;
            }

            txtModelNumber.Text = string.Empty;
            ddlOrigin.SelectedValue = "0";
            ddlServiceWarranty.SelectedValue = "0";
            txtCode.Text = string.Empty;
            ddlIsAttributeItem.SelectedValue = "-1";

            int rowsKitchenItem = gvKitchenItemCostCenterInfo.Rows.Count;
            List<InvItemCostCenterMappingBO> listKitchenItem = new List<InvItemCostCenterMappingBO>();
            for (int i = 0; i < rowsKitchenItem; i++)
            {
                CheckBox cbKitchen = (CheckBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                cbKitchen.Checked = false;

                TextBox tbMinimumStockLevel = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtMinimumStockLevel");
                TextBox tbUnitPriceLocal = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceLocal");
                TextBox tbUnitPriceUsd = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceUsd");

                DropDownList ddlDiscountType = (DropDownList)gvKitchenItemCostCenterInfo.Rows[i].FindControl("ddlDiscountType");
                ddlDiscountType.SelectedValue = "Fixed";
                TextBox tbDiscountAmount = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtDiscountAmount");
                tbDiscountAmount.Text = string.Empty;

                Label lblMap = (Label)gvKitchenItemCostCenterInfo.Rows[i].FindControl("lblMappingId");
                lblMap.Text = string.Empty;

                tbMinimumStockLevel.Text = string.Empty;
                tbUnitPriceLocal.Text = string.Empty;
                tbUnitPriceUsd.Text = string.Empty;
            }

            IsRecipeIncludedInInventory();

            LoadInvItemSupplierMappingInfo(0);
            IsItemOriginHide();
            LoadPMProductCostCenterMappingInfo(0);
            ltlTableWiseItemInformation.InnerHtml = GenerateRecipeItemTable(0);
            LoadInventoryIngredientText();
            LoadCostCenterInfoGridView();

            Session.Remove("EditedItemId");

            //gvSupplierInfo.DataSource = null;
            //gvSupplierInfo.DataBind();

            //Response.Redirect("frmInvItem.aspx", true);
            /*
            txtCode.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtName.Text = string.Empty;
            txtProductId.Value = string.Empty;
            //txtMinimumStockLevel.Text = string.Empty;
            txtPurchasePrice.Text = string.Empty;
            txtSellingPriceLocal.Text = string.Empty;
            txtSellingPriceUsd.Text = string.Empty;
            ddlSellingPriceLocal.SelectedIndex = 0;
            ddlSellingPriceUsd.SelectedIndex = 1;
            ddlCategoryId.SelectedIndex = 0;
            ddlProductType.SelectedIndex = 0;
            ddlStockBy.SelectedIndex = 0;
            ddlItemType.SelectedIndex = 0;
            ddlManufacturer.SelectedIndex = 0;
            ddlServiceWarranty.SelectedValue = "0";
            btnSave.Text = "Save";
            ddlStockType.SelectedIndex = 0;
            ddlClassification.SelectedIndex = 0;
            txtName.Focus();
            ddlIsCustomerItem.SelectedValue = "0";
            ddlIsSupplierItem.SelectedValue = "0";
            hfItemId.Value = string.Empty;
            ddlCategory.SelectedValue = "0";

            hfIsReceipeExist.Value = "0";

            Random rd = new Random();
            int seatingId = rd.Next(100000, 999999);
            RandomProductId.Value = seatingId.ToString();
            tempProductId.Value = seatingId.ToString();
            FileUpload();

            int rowsStockItem = gvStockItemCostCenterInfo.Rows.Count;
            List<InvItemCostCenterMappingBO> listStockItem = new List<InvItemCostCenterMappingBO>();
            for (int i = 0; i < rowsStockItem; i++)
            {
                TextBox tbMinimumStockLevel = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtMinimumStockLevel");
                TextBox tbUnitPriceLocal = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceLocal");
                TextBox tbUnitPriceUsd = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceUsd");
                CheckBox cb = (CheckBox)gvStockItemCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");

                DropDownList ddlDiscountType = (DropDownList)gvStockItemCostCenterInfo.Rows[i].FindControl("ddlDiscountType");
                ddlDiscountType.SelectedValue = "Fixed";
                TextBox tbDiscountAmount = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtDiscountAmount");
                tbDiscountAmount.Text = string.Empty;

                //DropDownList ddlKitchen = (DropDownList)gvStockItemCostCenterInfo.Rows[i].FindControl("ddlKitchen");
                //ddlKitchen.SelectedIndex = 0;
                tbMinimumStockLevel.Text = string.Empty;
                tbUnitPriceLocal.Text = string.Empty;
                tbUnitPriceUsd.Text = string.Empty;
                cb.Checked = false;
            }

            int rowsKitchenItem = gvKitchenItemCostCenterInfo.Rows.Count;
            List<InvItemCostCenterMappingBO> listKitchenItem = new List<InvItemCostCenterMappingBO>();
            for (int i = 0; i < rowsKitchenItem; i++)
            {
                TextBox tbMinimumStockLevel = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtMinimumStockLevel");
                TextBox tbUnitPriceLocal = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceLocal");
                TextBox tbUnitPriceUsd = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceUsd");
                CheckBox cb = (CheckBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");

                DropDownList ddlDiscountType = (DropDownList)gvKitchenItemCostCenterInfo.Rows[i].FindControl("ddlDiscountType");
                ddlDiscountType.SelectedValue = "Fixed";
                TextBox tbDiscountAmount = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtDiscountAmount");
                tbDiscountAmount.Text = string.Empty;

                //DropDownList ddlKitchen = (DropDownList)gvKitchenItemCostCenterInfo.Rows[i].FindControl("ddlKitchen");
                //ddlKitchen.SelectedIndex = 0;
                tbMinimumStockLevel.Text = string.Empty;
                tbUnitPriceLocal.Text = string.Empty;
                tbUnitPriceUsd.Text = string.Empty;
                cb.Checked = false;
            }
            */
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "InvItem";
            string pkFieldName = "ItemId";
            string pkFieldValue = this.txtProductId.Value;
            int IsDuplicate = 0;
            if (!string.IsNullOrWhiteSpace(pkFieldValue))
            {
                isUpdate = 1;
            }
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        private void IsRecipeIncludedInInventory()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRecipeIncludedInInventory", "IsRecipeIncludedInInventory");
            if (homePageSetupBO != null)
            {
                if (homePageSetupBO.SetupId > 0)
                {
                    hfIsRecipeIncludedInInventory.Value = homePageSetupBO.SetupValue;
                }
            }
        }
        private void IsInvItemCodeAutoGenerate()
        {
            CodeModelLabel.Visible = true;
            CodeModelControl.Visible = true;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsInvItemCodeAutoGenerate", "IsInvItemCodeAutoGenerate");
            if (homePageSetupBO != null)
            {
                if (homePageSetupBO.SetupId > 0)
                {
                    hfIsInvItemCodeAutoGenerate.Value = homePageSetupBO.SetupValue;
                    if (homePageSetupBO.SetupValue == "1")
                    {
                        CodeModelLabel.Visible = false;
                        CodeModelControl.Visible = false;
                    }
                }
            }
        }
        private void IsAttributeItemShow()
        {
            IsAttributeDiv.Visible = false;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsItemAttributeEnable", "IsItemAttributeEnable");

            if (homePageSetupBO.SetupValue == "1")
            {
                IsAttributeDiv.Visible = true;
            }
        }
        private void IsItemOriginHide()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsItemOriginHide", "IsItemOriginHide");
            if (homePageSetupBO != null)
            {
                if (homePageSetupBO.SetupId > 0)
                {
                    hfIsItemOriginHide.Value = homePageSetupBO.SetupValue;
                }
            }
        }
        private void SoftwareModulePermissionList()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.UserInfoId == 1)
            {
                hfSoftwareModulePermissionListInformation.Value = "1,2,3";
            }
            else
            {
                if (Session["SoftwareModulePermissionList"] != null)
                {
                    hfSoftwareModulePermissionListInformation.Value = Session["SoftwareModulePermissionList"].ToString();
                    if (hfSoftwareModulePermissionListInformation.Value == "3")
                    {
                        ServiceWarrantyAndProductTypeDiv.Visible = false;
                    }
                }
                else
                {
                    Session["UserInformationBOSession"] = null;
                    Response.Redirect("Login.aspx");
                }
            }
        }
        public void ClearControls(Control formAllControl)
        {
            foreach (Control formControl in formAllControl.Controls)
            {
                if (formControl.GetType() == typeof(TextBox))
                {
                    (formControl as TextBox).Text = string.Empty;
                }
                if (formControl.GetType() == typeof(RadioButtonList))
                {
                    (formControl as RadioButtonList).ClearSelection();
                }
                if (formControl.GetType() == typeof(ListBox))
                {
                    (formControl as ListBox).ClearSelection();
                }
                if (formControl.GetType() == typeof(CheckBox))
                {
                    (formControl as CheckBox).Checked = false;
                }
                if (formControl.GetType() == typeof(DropDownList))
                {
                    (formControl as DropDownList).SelectedIndex = 0;
                }
            }
        }
        private void LoadSupplierInfoGridView()
        {
            CheckObjectPermission();
            PMSupplierDA costCentreTabDA = new PMSupplierDA();
            List<PMSupplierBO> files = costCentreTabDA.GetPMSupplierInfo();

            gvSupplierInfo.DataSource = files;
            gvSupplierInfo.DataBind();
        }
        private void CheckObjectPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isKitchenItemCostCenterInformationDivEnable = -1;
            }
        }
        private void LoadItemClassificationInfo()
        {
            CheckObjectPermission();
            InvItemClassificationCostCenterMappingDA commonDA = new InvItemClassificationCostCenterMappingDA();
            List<ItemClassificationBO> fields = new List<ItemClassificationBO>();
            fields = commonDA.GetActiveItemClassificationInfo();

            ddlClassification.DataSource = fields;
            ddlClassification.DataTextField = "ClassificationName";
            ddlClassification.DataValueField = "ClassificationId";
            ddlClassification.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlClassification.Items.Insert(0, item);

            ddlSClassification.DataSource = fields;
            ddlSClassification.DataTextField = "ClassificationName";
            ddlSClassification.DataValueField = "ClassificationId";
            ddlSClassification.DataBind();

            ListItem itemS = new ListItem();
            itemS.Value = "0";
            itemS.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSClassification.Items.Insert(0, itemS);
        }
        public void FillForm(int EditId)
        {
            InvItemBO productBO = new InvItemBO();
            InvItemDA productDA = new InvItemDA();

            productBO = productDA.GetInvItemInfoById(0, EditId);

            ddlCategoryId.SelectedValue = productBO.CategoryId.ToString();
            ddlItemType.SelectedValue = productBO.ItemType.ToString();
            ddlManufacturer.SelectedValue = productBO.ManufacturerId.ToString();
            ddlProductType.SelectedValue = productBO.ProductType;
            ddlStockBy.SelectedValue = productBO.StockBy.ToString();
            ddlSalesStockBy.SelectedValue = productBO.SalesStockBy.ToString();
            txtPurchasePrice.Text = productBO.PurchasePrice.ToString();
            ddlSellingPriceLocal.SelectedValue = productBO.SellingLocalCurrencyId.ToString();
            txtSellingPriceLocal.Text = productBO.UnitPriceLocal.ToString();
            ddlSellingPriceUsd.SelectedValue = productBO.SellingUsdCurrencyId.ToString();
            ddlStockType.SelectedValue = productBO.StockType.ToString();
            hfExistingStockType.Value = productBO.StockType.ToString();
            txtSellingPriceUsd.Text = productBO.UnitPriceUsd.ToString();
            ddlClassification.SelectedValue = productBO.ClassificationId.ToString();
            txtDescription.Text = productBO.Description.ToString();
            txtCode.Text = productBO.Code.ToString();

            txtModelNumber.Text = productBO.Model.ToString();
            ddlOrigin.SelectedValue = productBO.CountryId.ToString();

            hfItemCode.Value = productBO.Code.ToString();
            txtName.Text = productBO.Name.ToString();
            txtDisplayName.Text = productBO.DisplayName;
            txtProductId.Value = productBO.ItemId.ToString();
            ddlServiceWarranty.SelectedValue = productBO.ServiceWarranty.ToString();
            ddlIsCustomerItem.SelectedValue = (productBO.IsCustomerItem == true ? 1 : 0).ToString();
            ddlIsSupplierItem.SelectedValue = (productBO.IsSupplierItem == true ? 1 : 0).ToString();
            ddlIsItemEditable.SelectedValue = (productBO.IsItemEditable == true ? 1 : 0).ToString();
            ddlIsAttributeItem.SelectedValue = (productBO.IsAttributeItem == true ? 1 : 0).ToString();

            ddlAccessFrequancy.SelectedValue = productBO.AdjustmentFrequency;
            btnSave.Visible = isUpdatePermission;
            btnSave.Text = "Update";
            //RandomProductId.Value = EditId.ToString();
            if (ddlStockType.SelectedValue == "KitchenItem")
            {
                isKitchenItemCostCenterInformationDivEnable = 1;
            }

            if (productBO.IsSupplierItem)
                LoadInvItemSupplierMappingInfo(EditId);

            LoadPMProductCostCenterMappingInfo(EditId);

            ltlTableWiseItemInformation.InnerHtml = GenerateRecipeItemTable(EditId);

            //RecipeTable.InnerHtml = string.Empty;
            hfItemAttributeList.Value = string.Empty;
            List<InvItemAttributeBO> InvItemAttributeList = new List<InvItemAttributeBO>();
            InvItemAttributeList = productDA.GetItemAttributeByItemId(EditId);
            if (InvItemAttributeList.Count > 0)
            {
                ddlIsAttributeItem.SelectedValue = "1";
            }
            for (int i = 0; i < InvItemAttributeList.Count; i++)
            {
                hfItemAttributeList.Value += (i == 0 ? InvItemAttributeList[i].Id.ToString() : "," + InvItemAttributeList[i].Id.ToString());
            }
        }
        private void LoadInvItemSupplierMappingInfo(int EditId)
        {
            this.LoadSupplierInfoGridView();
            List<InvItemSuppierMappingBO> costListStockItem = new List<InvItemSuppierMappingBO>();
            InvItemCostCenterMappingDA costStockItemDA = new InvItemCostCenterMappingDA();
            costListStockItem = costStockItemDA.GetInvItemSupplierMappingByItemId(EditId);
            int rowsStockItem = gvSupplierInfo.Rows.Count;

            List<InvItemSuppierMappingBO> listStockItem = new List<InvItemSuppierMappingBO>();
            for (int i = 0; i < rowsStockItem; i++)
            {
                Label lbl = (Label)gvSupplierInfo.Rows[i].FindControl("lblSupplierId");

                if (!string.IsNullOrEmpty(lbl.Text) || (lbl.Text == "0"))
                {
                    InvItemSuppierMappingBO costCenterStockItem = new InvItemSuppierMappingBO();
                    costCenterStockItem.SupplierId = Int32.Parse(lbl.Text);
                    listStockItem.Add(costCenterStockItem);
                }
            }

            for (int i = 0; i < listStockItem.Count; i++)
            {
                for (int j = 0; j < costListStockItem.Count; j++)
                {
                    if (listStockItem[i].SupplierId == costListStockItem[j].SupplierId)
                    {
                        CheckBox cb = (CheckBox)gvSupplierInfo.Rows[i].FindControl("chkIsSavePermission");
                        cb.Checked = true;
                    }
                }
            }

        }
        private void LoadPMProductCostCenterMappingInfo(int EditId)
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();


            List<InvItemCostCenterMappingBO> costListKitchenItem = new List<InvItemCostCenterMappingBO>();
            InvItemCostCenterMappingDA costKitchenItemDA = new InvItemCostCenterMappingDA();
            costListKitchenItem = costKitchenItemDA.GetInvItemCostCenterMappingByItemId(EditId);
            int rowsKitchenItem = gvKitchenItemCostCenterInfo.Rows.Count;

            List<InvItemCostCenterMappingBO> listKitchenItem = new List<InvItemCostCenterMappingBO>();
            for (int i = 0; i < rowsKitchenItem; i++)
            {
                InvItemCostCenterMappingBO costCenter = new InvItemCostCenterMappingBO();
                Label lbl = (Label)gvKitchenItemCostCenterInfo.Rows[i].FindControl("lblCostCentreId");
                Label lblMap = (Label)gvKitchenItemCostCenterInfo.Rows[i].FindControl("lblMappingId");

                int costCenterId = !string.IsNullOrWhiteSpace(lbl.Text) ? Convert.ToInt32(lbl.Text) : 0;
                int mappingId = !string.IsNullOrWhiteSpace(lblMap.Text) ? Convert.ToInt32(lblMap.Text) : 0;

                if (!string.IsNullOrWhiteSpace(lblMap.Text) || lblMap.Text != "0")
                {
                    if (costCenterId > 0)
                    {
                        costCenter.CostCenterId = Int32.Parse(lbl.Text);
                    }
                    if (mappingId > 0)
                    {
                        costCenter.MappingId = Int32.Parse(lblMap.Text);
                    }
                    listKitchenItem.Add(costCenter);
                }
            }

            for (int i = 0; i < listKitchenItem.Count; i++)
            {
                for (int j = 0; j < costListKitchenItem.Count; j++)
                {
                    if (listKitchenItem[i].CostCenterId == costListKitchenItem[j].CostCenterId)
                    {
                        TextBox tbMinimumStockLevel = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtMinimumStockLevel");
                        TextBox tbUnitPriceLocal = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceLocal");
                        TextBox tbUnitPriceUsd = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceUsd");
                        CheckBox cb = (CheckBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                        Label lblMap = (Label)gvKitchenItemCostCenterInfo.Rows[i].FindControl("lblMappingId");

                        cb.Checked = true;
                        tbMinimumStockLevel.Text = costListKitchenItem[j].MinimumStockLevel.ToString();
                        tbUnitPriceLocal.Text = costListKitchenItem[j].UnitPriceLocal.ToString();
                        tbUnitPriceUsd.Text = costListKitchenItem[j].UnitPriceUsd.ToString();
                        lblMap.Text = costListKitchenItem[j].MappingId.ToString();

                        TextBox tbServiceCharge = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtServiceCharge");
                        TextBox tbSDCharge = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtSDCharge");
                        TextBox tbVatAmount = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtVatAmount");
                        TextBox tbAdditionalCharge = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtAdditionalCharge");

                        tbServiceCharge.Text = costListKitchenItem[j].ServiceCharge.ToString();
                        tbSDCharge.Text = costListKitchenItem[j].SDCharge.ToString();
                        tbVatAmount.Text = costListKitchenItem[j].VatAmount.ToString();
                        tbAdditionalCharge.Text = costListKitchenItem[j].AdditionalCharge.ToString();

                        // Kitchen Drop Down Information
                        Label lblCostCentreIdValue = (Label)gvKitchenItemCostCenterInfo.Rows[i].FindControl("lblCostCentreId");
                        int costCenterId = Convert.ToInt32(lblCostCentreIdValue.Text);

                        RestaurantKitchenDA entityDA = new RestaurantKitchenDA();
                        List<RestaurantKitchenBO> entityBOListKitchenItem = new List<RestaurantKitchenBO>();
                        entityBOListKitchenItem = entityDA.GetRestaurantKitchenInfoByCostCenterId(costCenterId);

                        DropDownList ddlKitchen = (DropDownList)gvKitchenItemCostCenterInfo.Rows[i].FindControl("ddlKitchen");

                        ddlKitchen.DataSource = entityBOListKitchenItem;
                        ddlKitchen.DataTextField = "KitchenName";
                        ddlKitchen.DataValueField = "KitchenId";
                        ddlKitchen.DataBind();

                        if (entityBOListKitchenItem.Count > 1)
                        {
                            ListItem itemKitchen = new ListItem();
                            itemKitchen.Value = "0";
                            itemKitchen.Text = hmUtility.GetDropDownFirstValue();
                            ddlKitchen.Items.Insert(0, itemKitchen);

                            ddlKitchen.SelectedValue = costListKitchenItem[j].KitchenId.ToString();
                        }

                        DropDownList ddlDiscountType = (DropDownList)gvKitchenItemCostCenterInfo.Rows[i].FindControl("ddlDiscountType");
                        ddlDiscountType.SelectedValue = costListKitchenItem[j].DiscountType.ToString();

                        TextBox tbDiscountAmount = (TextBox)gvKitchenItemCostCenterInfo.Rows[i].FindControl("txtDiscountAmount");
                        tbDiscountAmount.Text = costListKitchenItem[j].DiscountAmount.ToString();
                    }
                }
            }

            List<InvItemCostCenterMappingBO> costListStockItem = new List<InvItemCostCenterMappingBO>();
            //InvItemCostCenterMappingDA costStockItemDA = new InvItemCostCenterMappingDA();
            costListStockItem = costListKitchenItem;
            int rowsStockItem = gvStockItemCostCenterInfo.Rows.Count;
            List<CostCentreTabBO> costcenTerList = new List<CostCentreTabBO>();

            costcenTerList = costCentreTabDA.GetCostCentreTabInfo();

            //List<InvItemCostCenterMappingBO> listStockItem = new List<InvItemCostCenterMappingBO>();
            //for (int i = 0; i < rowsStockItem; i++)
            //{
            //    InvItemCostCenterMappingBO costCenterStockItem = new InvItemCostCenterMappingBO();
            //    Label lbl = (Label)gvStockItemCostCenterInfo.Rows[i].FindControl("lblCostCentreId");
            //    Label lblMap = (Label)gvStockItemCostCenterInfo.Rows[i].FindControl("lblMappingId");

            //    if (!string.IsNullOrEmpty(lblMap.Text) || (lblMap.Text == "0"))
            //    {
            //        costCenterStockItem.CostCenterId = Int32.Parse(lbl.Text);
            //        costCenterStockItem.MappingId = Int32.Parse(lblMap.Text);

            //        costCenterStockItem.CostCenterId = Int32.Parse(lbl.Text);
            //        listStockItem.Add(costCenterStockItem);
            //    }
            //}


            for (int i = 0; i < costcenTerList.Count; i++)
            {
                //for (int j = 0; j < costListStockItem.Count; j++)
                // {

                var mapingCostcenter = costListStockItem.Where(c => c.CostCenterId == costcenTerList[i].CostCenterId).FirstOrDefault();

                if (mapingCostcenter != null)
                {
                    TextBox tbMinimumStockLevel = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtMinimumStockLevel");
                    TextBox tbUnitPriceLocal = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceLocal");
                    TextBox tbUnitPriceUsd = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtUnitPriceUsd");
                    CheckBox cb = (CheckBox)gvStockItemCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                    Label lblMap = (Label)gvStockItemCostCenterInfo.Rows[i].FindControl("lblMappingId");

                    cb.Checked = true;
                    tbMinimumStockLevel.Text = mapingCostcenter.MinimumStockLevel.ToString();
                    tbUnitPriceLocal.Text = mapingCostcenter.UnitPriceLocal.ToString();
                    tbUnitPriceUsd.Text = mapingCostcenter.UnitPriceUsd.ToString();
                    lblMap.Text = mapingCostcenter.MappingId.ToString();

                    TextBox tbServiceCharge = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtServiceCharge");
                    TextBox tbSDCharge = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtSDCharge");
                    TextBox tbVatAmount = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtVatAmount");
                    TextBox tbAdditionalCharge = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtAdditionalCharge");

                    tbServiceCharge.Text = mapingCostcenter.ServiceCharge.ToString();
                    tbSDCharge.Text = mapingCostcenter.SDCharge.ToString();
                    tbVatAmount.Text = mapingCostcenter.VatAmount.ToString();
                    tbAdditionalCharge.Text = mapingCostcenter.AdditionalCharge.ToString();

                    DropDownList ddlDiscountType = (DropDownList)gvStockItemCostCenterInfo.Rows[i].FindControl("ddlDiscountType");
                    ddlDiscountType.SelectedValue = mapingCostcenter.DiscountType.ToString();

                    TextBox tbDiscountAmount = (TextBox)gvStockItemCostCenterInfo.Rows[i].FindControl("txtDiscountAmount");
                    tbDiscountAmount.Text = mapingCostcenter.DiscountAmount.ToString();
                }
                else
                {
                    Label lblMap = (Label)gvStockItemCostCenterInfo.Rows[i].FindControl("lblMappingId");
                    CheckBox cb = (CheckBox)gvStockItemCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                    lblMap.Text = "0";
                    cb.Checked = false;
                }
                //}
            }
        }
        private void LoadServiceType()
        {
            InvCategoryDA categoryDA = new InvCategoryDA();
            var category = categoryDA.GetAllActiveInvItemCatagoryInfoByServiceType("All");
            ddlCategoryId.DataSource = category;
            ddlCategoryId.DataTextField = "MatrixInfo";
            ddlCategoryId.DataValueField = "CategoryId";
            ddlCategoryId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCategoryId.Items.Insert(0, item);

            ddlCategory.DataSource = category;
            ddlCategory.DataTextField = "MatrixInfo";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
            ListItem item1 = new ListItem();
            item1.Value = "0";
            item1.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCategory.Items.Insert(0, item1);
        }
        public string DeleteData(int sEmpId)
        {
            string result = string.Empty;
            try
            {
                InvItemDA invItemDA = new InvItemDA();
                Boolean status = invItemDA.DeleteInvItemInfoByItemId(sEmpId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.InvItem.ToString(), sEmpId,
                        ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvItem));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    LoadGridView();
                    Cancel();
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Data Deleted Failed.";
                throw ex;
            }

            return result;
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            decimal no1, no2, no3;

            if (ddlCategoryId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Product Category.", AlertType.Warning);
                ddlCategoryId.Focus();
                flag = false;
            }
            else if (string.IsNullOrEmpty(txtName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Product Name.", AlertType.Warning);
                txtName.Focus();
                flag = false;
            }
            else if (string.IsNullOrEmpty(txtCode.Text))
            {
                if (hfIsInvItemCodeAutoGenerate.Value == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Product Code.", AlertType.Warning);
                    txtCode.Focus();
                    flag = false;
                }
            }
            else if (ddlClassification.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Item Classification.", AlertType.Warning);
                ddlClassification.Focus();
                flag = false;
            }
            else if (ddlStockBy.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Stock By.", AlertType.Warning);
                ddlStockBy.Focus();
                flag = false;
            }
            return flag;
        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("LocalNUsd");

            List<CommonCurrencyBO> localCurrencyListBO = new List<CommonCurrencyBO>();
            localCurrencyListBO = currencyListBO.Where(x => x.CurrencyType == "Local").ToList();
            List<CommonCurrencyBO> UsdCurrencyListBO = new List<CommonCurrencyBO>();
            UsdCurrencyListBO = currencyListBO.Where(x => x.CurrencyType == "Usd").ToList();

            ddlSellingPriceLocal.DataSource = localCurrencyListBO;
            ddlSellingPriceLocal.DataTextField = "CurrencyName";
            ddlSellingPriceLocal.DataValueField = "CurrencyId";
            ddlSellingPriceLocal.DataBind();
            ddlSellingPriceLocal.SelectedIndex = 0;
            lblSellingPriceLocal.Text = "Unit Price(" + ddlSellingPriceLocal.SelectedItem.Text + ")";


            ddlSellingPriceUsd.DataSource = UsdCurrencyListBO;
            ddlSellingPriceUsd.DataTextField = "CurrencyName";
            ddlSellingPriceUsd.DataValueField = "CurrencyId";
            ddlSellingPriceUsd.DataBind();
            ddlSellingPriceUsd.SelectedIndex = 1;
            lblSellingPriceUsd.Text = "Unit Price(" + ddlSellingPriceUsd.SelectedItem.Text + ")";

            gvStockItemCostCenterInfo.Columns[5].HeaderText = "Unit Price (" + ddlSellingPriceLocal.SelectedItem.Text + ")";
            gvStockItemCostCenterInfo.Columns[6].HeaderText = "Unit Price (" + ddlSellingPriceUsd.SelectedItem.Text + ")";

            gvKitchenItemCostCenterInfo.Columns[6].HeaderText = "Unit Price (" + ddlSellingPriceLocal.SelectedItem.Text + ")";
            gvKitchenItemCostCenterInfo.Columns[7].HeaderText = "Unit Price (" + ddlSellingPriceUsd.SelectedItem.Text + ")";
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllActiveInvItemCatagoryInfoByServiceType("All");
            ddlPCategoryId.DataSource = List;
            ddlPCategoryId.DataTextField = "MatrixInfo";
            ddlPCategoryId.DataValueField = "CategoryId";
            ddlPCategoryId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlPCategoryId.Items.Insert(0, item);
        }
        private void LoadServiceWarranty()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("InventoryProductServiceWarranty", hmUtility.GetDropDownFirstValue());
            if (fields != null)
            {
                if (fields.Count > 1)
                {
                    fields.RemoveAt(0);
                }
            }
            ddlServiceWarranty.DataSource = fields;
            ddlServiceWarranty.DataTextField = "FieldValue";
            ddlServiceWarranty.DataValueField = "FieldId";
            ddlServiceWarranty.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownNoneValue();
            ddlServiceWarranty.Items.Insert(0, item);
        }
        private void LoadManufacturer()
        {
            List<InvManufacturerBO> List = new List<InvManufacturerBO>();
            InvManufacturerDA da = new InvManufacturerDA();
            List = da.GetManufacturerInfo();
            ddlManufacturer.DataSource = List;
            ddlManufacturer.DataTextField = "Name";
            ddlManufacturer.DataValueField = "ManufacturerId";
            ddlManufacturer.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownNoneValue();
            ddlManufacturer.Items.Insert(0, item);
        }
        //private void LoadAttributeName()
        //{
        //    InvItemAttributeViewBO attributeViewBO = new InvItemAttributeViewBO();
        //    InvItemDA DA = new InvItemDA();
        //    int setupTypeId = Convert.ToInt32(ddlSetupType.SelectedValue);
        //    attributeViewBO = DA.GetAllInvItemAttributeAndSetupType();
        //    ddlAttributeName.DataSource = attributeViewBO.InvItemAttributeBOList;
        //    ddlAttributeName.DataTextField = "Name";
        //    ddlAttributeName.DataValueField = "Id";
        //    ddlAttributeName.DataBind();
        //    ListItem item = new ListItem();
        //    item.Value = "0";
        //    item.Text = hmUtility.GetDropDownFirstValue();
        //    ddlAttributeName.Items.Insert(0, item);
        //}
        public string GenerateRecipeItemTable(int itemId)
        {
            string strTable = "";
            var deleteLink = "";
            decimal totalRecipeCost = 0;

            InvItemDA invDA = new InvItemDA();
            List<RestaurantRecipeDetailBO> recipeList = new List<RestaurantRecipeDetailBO>();
            recipeList = invDA.GetRecipeItemInfoByItemId(itemId);

            strTable += "<table id='RecipeItemInformation' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='display:none'></th><th style='display:none'></th><th align='left' scope='col' style='width: 25%;'>Item Name</th>";
            strTable += "<th align='left' scope='col' style='width: 12%;'>Stock By</th> <th align='left' scope='col' style='width: 12%;'>Quantity</th>";
            strTable += "<th align='left' scope='col' style='width: 15%;'>Cost</th><th align='center' scope='col' style='width: 26%;'>Is Gradient Can Change?</th><th style='display:none'></th><th align='center' scope='col' style='width: 10%;'>Action</th></tr></thead>";
            strTable += "<tbody>";
            int counter = 0;
            if (recipeList != null)
            {
                foreach (RestaurantRecipeDetailBO dr in recipeList)
                {
                    totalRecipeCost += dr.ItemCost;

                    deleteLink = "<a href=\"javascript:void();\" onclick= 'DeleteItem(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
                    counter++;

                    if (counter % 2 == 0)
                    {
                        // It's even
                        strTable += "<tr style='background-color:White;'>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:#E3EAEB;'>";
                    }

                    strTable += "<td align='left' style=\"display:none;\">" + dr.RecipeId + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.RecipeItemId + "</td>";
                    strTable += "<td align='left' style='width: 25%;'>" + dr.RecipeItemName + "</td>";
                    strTable += "<td align='left' style=\"width:15%; text-align:Left;\">" + dr.HeadName + "</td>";
                    strTable += "<td align='left' style=\"width:15%; text-align:Left;\">" + dr.ItemUnit + "</td>";
                    strTable += "<td align='left' style='width: 15%;'>" + dr.ItemCost + "</td>";
                    strTable += "<td align='center' style='width: 26%;'>" + "<input type='checkbox' " + (dr.IsGradientCanChange == true ? "checked='cheked'" : "") + "/>" + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.UnitHeadId + "</td>";
                    strTable += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";
                    strTable += "</tr>";

                }
            }
            strTable += "</tbody>";
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }

            lblTotalReceipeCost.Text = totalRecipeCost.ToString();

            return strTable;
        }
        public void GenerateRecipeItemTable()
        {
            string strTable = "";
            var deleteLink = "";
            decimal totalRecipeCost = 0;

            List<RestaurantRecipeDetailBO> receipeList = new List<RestaurantRecipeDetailBO>();
            List<RestaurantRecipeDetailBO> deleteReceipeList = new List<RestaurantRecipeDetailBO>();

            receipeList = JsonConvert.DeserializeObject<List<RestaurantRecipeDetailBO>>(hfSaveObj.Value);

            if (receipeList == null || receipeList.Count == 0)
            {
                return;
            }

            strTable += "<table id='RecipeItemInformation' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='display:none'></th><th style='display:none'></th><th align='left' scope='col' style='width: 25%;'>Item Name</th>";
            strTable += "<th align='left' scope='col' style='width: 12%;'>Stock By</th> <th align='left' scope='col' style='width: 12%;'>Quantity</th>";
            strTable += "<th align='left' scope='col' style='width: 15%;'>Cost</th><th align='center' scope='col' style='width: 26%;'>Is Gradient Can Change?</th><th style='display:none'></th><th align='center' scope='col' style='width: 10%;'>Action</th></tr></thead>";
            strTable += "<tbody>";
            int counter = 0;
            if (receipeList != null)
            {
                foreach (RestaurantRecipeDetailBO dr in receipeList)
                {
                    totalRecipeCost += dr.ItemCost;

                    deleteLink = "<a href=\"javascript:void();\" onclick= 'DeleteItem(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
                    counter++;

                    if (counter % 2 == 0)
                    {
                        // It's even
                        strTable += "<tr style='background-color:White;'>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:#E3EAEB;'>";
                    }

                    strTable += "<td align='left' style=\"display:none;\">" + dr.RecipeId + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.RecipeItemId + "</td>";
                    strTable += "<td align='left' style='width: 25%;'>" + dr.RecipeItemName + "</td>";
                    strTable += "<td align='left' style=\"width:12%; text-align:Left;\">" + dr.HeadName + "</td>";
                    strTable += "<td align='left' style=\"width:12%; text-align:Left;\">" + dr.ItemUnit + "</td>";
                    strTable += "<td align='left' style='width: 15%;'>" + dr.ItemCost + "</td>";
                    strTable += "<td align='center' style='width: 26%;'>" + "<input type='checkbox' " + (dr.IsGradientCanChange == true ? "checked='cheked'" : "") + "/>" + "</td>";
                    strTable += "<td align='left' style=\"display:none;\">" + dr.UnitHeadId + "</td>";
                    strTable += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";
                    strTable += "</tr>";

                }
            }
            strTable += "</tbody>";
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }

            lblTotalReceipeCost.Text = totalRecipeCost.ToString();
            ltlTableWiseItemInformation.InnerHtml = strTable;

        }
        private void SetTab(string TabName)
        {
            if (TabName == "Search")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "Entry")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        private void LoadGridView()
        {
            List<InvItemBO> productList = new List<InvItemBO>();
            InvItemDA productDA = new InvItemDA();
            string ProductName = txtPName.Text;
            string ProductCode = txtPCode.Text;
            int CategoryId = Int32.Parse(ddlPCategoryId.SelectedValue);
            productList = productDA.GetInvItemInfoBySearchCriteria("InventoryItem", ProductName, ProductCode, CategoryId);
            CheckObjectPermission();

            isCurrencySingle = (GetHMCommonSetupForCurrency().SetupValue == "Single" ? true : false);

            //gvProduct.DataSource = productList;
            //gvProduct.DataBind();
        }
        public int InvItemDuplicateChecking(int isUpdate, int itemId, int categoryId, string itemName)
        {
            int IsDuplicate = 0;
            InvItemDA hmCommonDA = new InvItemDA();
            IsDuplicate = hmCommonDA.InvItemDuplicateChecking(isUpdate, itemId, categoryId, itemName);
            return IsDuplicate;
        }
        private void ShowHideCurrencyInformation()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = GetHMCommonSetupForCurrency();

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
        private HMCommonSetupBO GetHMCommonSetupForCurrency()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("CurrencyType", "CurrencyConfiguration");

            return commonSetupBO;
        }
        private void LoadCostCenterInfoGridView()
        {
            gvKitchenItemCostCenterInfo.DataSource = new List<CostCentreTabBO>();
            gvKitchenItemCostCenterInfo.DataBind();

            CheckObjectPermission();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> files = costCentreTabDA.GetCostCentreTabInfo();

            gvStockItemCostCenterInfo.DataSource = files;
            gvStockItemCostCenterInfo.DataBind();


            //List<CostCentreTabBO> KitchenItemCostCenterInfo = files.Where(x => x.IsRestaurant == true).ToList();
            List<CostCentreTabBO> KitchenItemCostCenterInfo = files.Where(x => x.CostCenterType == "Restaurant" || x.CostCenterType == "Banquet").ToList();

            gvKitchenItemCostCenterInfo.DataSource = KitchenItemCostCenterInfo;
            gvKitchenItemCostCenterInfo.DataBind();
        }
        private void LoadStockBy()
        {
            List<InvUnitHeadBO> headListBO = new List<InvUnitHeadBO>();
            InvUnitHeadDA da = new InvUnitHeadDA();
            headListBO = da.GetInvUnitHeadInfo();

            ddlStockBy.DataSource = headListBO;
            ddlStockBy.DataTextField = "HeadName";
            ddlStockBy.DataValueField = "UnitHeadId";
            ddlStockBy.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlStockBy.Items.Insert(0, item);

            ddlItemStockBy.DataSource = headListBO;
            ddlItemStockBy.DataTextField = "HeadName";
            ddlItemStockBy.DataValueField = "UnitHeadId";
            ddlItemStockBy.DataBind();
            ddlItemStockBy.Items.Insert(0, item);

            ddlSalesStockBy.DataSource = headListBO;
            ddlSalesStockBy.DataTextField = "HeadName";
            ddlSalesStockBy.DataValueField = "UnitHeadId";
            ddlSalesStockBy.DataBind();
            ddlSalesStockBy.Items.Insert(0, item);

        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UploadData()
        {
            //get reference to posted file and do what you want with this file
            HttpPostedFile postedfile = Context.Request.Files.Get(0) as HttpPostedFile;
            return "";

        }
        [WebMethod]
        public static string GetUploadedImageByWebMethod(int OwnerId, string docType)
        {
            string strTable = "";
            DocumentsDA docDA = new DocumentsDA();
            var docList = docDA.GetDocumentsInfoByDocCategoryAndOwnerId(docType, OwnerId);
            if (docList.Count > 0)
            {
                var Image = docList[0];

                strTable += "<img src='" + Image.Path + Image.Name + "'  alt='No Image Selected' border='0' style='height:150px;' />";
            }
            return strTable;

        }
        [WebMethod]
        public static GridViewDataNPaging<InvItemBO, GridPaging> SearchInvItemAndLoadGridInformation(string gridOrderBy, string itemName, string displayName, string itemCode, string categoryId, string classification, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<InvItemBO, GridPaging> myGridData = new GridViewDataNPaging<InvItemBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            InvItemDA invItemDA = new InvItemDA();
            List<InvItemBO> invItemList = new List<InvItemBO>();
            invItemList = invItemDA.GetInvItemInfoBySearchCriteriaForPagination(gridOrderBy, "IndividualItem", itemName, displayName, itemCode, categoryId, classification, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<InvItemBO> distinctItems = new List<InvItemBO>();
            distinctItems = invItemList.GroupBy(test => test.ItemId).Select(group => group.First()).ToList();
            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static string DeleteInvItem(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            try
            {
                InvItemDA invItemDA = new InvItemDA();
                Boolean status = invItemDA.DeleteInvItemInfoByItemId(sEmpId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.InvItem.ToString(), sEmpId,
                        ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvItem));
                    result = "success";
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
        public static List<InvItemAttributeBO> GetAttributeByWebMethod(int EditId)
        {
            InvItemDA productDA = new InvItemDA();
            List<InvItemAttributeBO> InvItemAttributeList = new List<InvItemAttributeBO>();
            InvItemAttributeList = productDA.GetItemAttributeByItemId(EditId);
            return InvItemAttributeList;
        }

        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemNCategoryAutoSearch(string itemName, int categoryId, int isCustomerItem, string itemType)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemNameNCategoryForRecipeAutoSearch(itemName, categoryId, Convert.ToBoolean(isCustomerItem), itemType);

            return itemInfo;
        }
        [WebMethod]
        public static decimal GetReceipeItemCost(int itemId, int stockById, decimal quantity)
        {
            RestaurantRecipeDetailBO itemInfo = new RestaurantRecipeDetailBO();

            InvItemDA itemDA = new InvItemDA();
            itemInfo = itemDA.GetReceipeItemCost(itemId, stockById, quantity);

            return itemInfo.ItemCost;
        }
        [WebMethod]
        public static List<InvItemAttributeBO> GetAttributeName(int setupTypeId)
        {
            InvItemAttributeViewBO attributeViewBO = new InvItemAttributeViewBO();
            InvItemDA DA = new InvItemDA();
            attributeViewBO = DA.GetAllInvItemAttributeBySetupType(setupTypeId);

            return attributeViewBO.InvItemAttributeBOList;
        }
        [WebMethod(EnableSession = true)]
        public static string EditInvItem(int itemId)
        {
            string result = string.Empty;
            try
            {
                HttpContext.Current.Session["EditedItemId"] = itemId;
                result = itemId.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        [WebMethod]
        public static List<InvUnitHeadBO> LoadRelatedStockBy(int stockById)
        {
            InvUnitHeadDA unitHeadDA = new InvUnitHeadDA();
            List<InvUnitHeadBO> unitHeadList = new List<InvUnitHeadBO>();

            unitHeadList = unitHeadDA.GetRelatedStockBy(stockById);

            return unitHeadList;
        }
        [WebMethod]
        public static string PrintBarCode(string txtCode, string txtName, decimal price)
        {
            string path = PrintBarcodeLabel.PrintBarcode(txtCode, txtName, price);
            return path;
        }
        [WebMethod]
        public static string PrintBarCodeById(int itemId)
        {
            InvItemBO productBO = new InvItemBO();
            InvItemDA productDA = new InvItemDA();
            productBO = productDA.GetItemByItemAndCostCenterForBarCode(itemId);

            string path = PrintBarcodeLabel.PrintBarcode(productBO.Code, productBO.Name, productBO.UnitPriceLocal);
            return path;
        }
        [WebMethod]
        public static List<DocumentsBO> GetUploadedDocByWebMethod(int randomId, int id, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }
            List<DocumentsBO> docList = new List<DocumentsBO>();
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("InventoryProduct", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("InventoryProduct", (int)id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));
            foreach (DocumentsBO dc in docList)
            {
                if (dc.DocumentType == "Image")
                    dc.Path = (dc.Path + dc.Name);

                dc.Name = dc.Name.Remove(dc.Name.LastIndexOf('.'));
            }
            docList = new HMCommonDA().GetDocumentListWithIcon(docList).ToList();
            return docList;
        }
    }
}