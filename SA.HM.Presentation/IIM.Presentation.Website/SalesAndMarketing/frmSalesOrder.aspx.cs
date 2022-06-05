using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;
using HotelManagement.Data.HMCommon;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class frmSalesOrder : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        int UserId = 1;
        ArrayList arrayDelete;
        protected int _OrderId;
        HMUtility hmUtility = new HMUtility();
        private int IsPurchaseOrderApprovalEnable;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            UserId = userInformationBO.UserInfoId;
            POrderTemplate1.Visible = true;
            POrderTemplate2.Visible = false;
            hfpurchaseOrderTemplate.Value = "1";
            //CheckNApproveDiv.Visible = true;

            if (!IsPostBack)
            {
                LoadCurrency();
                LoadAffiliatedCompany();
                LoadPRNumber();
                LoadCategory();
                LoadCommonDropDownHiddenField();
                LoadAllCostCentreTabInfo();
                LoadStockBy();
                IsPurchaseOrderApprovalEnable = 0;
                LoadIsPurchaseOrderApprovalEnable();
                //LoadUserInformation();
                if (Session["PurchaseOrderEditId"] != null)
                {
                    hfPOrderId.Value = Session["PurchaseOrderEditId"].ToString();
                    hfIsEditedFromApprovedForm.Value = "1";
                    hfpurchaseOrderTemplate.Value = "1";
                    Session.Remove("PurchaseOrderEditId");
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            hfIsEditedFromApprovedForm.Value = "0";
            hfPOrderId.Value = "0";
            LoadSearchInformation();
        }
        protected void gvOrderInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int POrderId = 0, supplierId = 0;
                POrderId = Convert.ToInt32(e.CommandArgument.ToString());

                GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                Label lblSupplier = (Label)row.FindControl("lblSupplierId");

                supplierId = Convert.ToInt32(lblSupplier.Text);

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                int approvedBy = userInformationBO.UserInfoId;
                string approvedStatus = string.Empty;
                PMPurchaseOrderDA orderDetailsDA = new PMPurchaseOrderDA();
                Boolean status = false;

                if (e.CommandName == "CmdDelete")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    status = orderDetailsDA.DeleteSMSalesOrder(POrderId);
                    LoadSearchInformation();
                }
                else if (e.CommandName == "CmdReportPO")
                {
                    string url = "/SalesAndMarketing/Reports/frmReportSalesOrderInvoice.aspx?POrderId=" + POrderId + "&SupId=" + supplierId;
                    string s = "window.open('" + url + "', 'popup_window', 'width=770,height=680,left=300,top=50,resizable=yes');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        protected void gvOrderInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrderInfo.PageIndex = e.NewPageIndex;
        }
        protected void gvOrderInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                Label lblApprovedStatusValue = (Label)e.Row.FindControl("lblApprovedStatus");
                ImageButton imgReportPO = (ImageButton)e.Row.FindControl("ImgReportPO");
                Label lblDeliveryStatusValue = (Label)e.Row.FindControl("lblDeliveryStatus");
                ImageButton imgBillStatus = (ImageButton)e.Row.FindControl("ImgBillStatus");

                if (lblDeliveryStatusValue.Text == "Full")
                {
                    imgBillStatus.Visible = true;
                }
                else
                {
                    imgBillStatus.Visible = false;
                }

                if (lblApprovedStatusValue.Text != "Approved")
                {
                    imgUpdate.Visible = true;
                    imgDelete.Visible = true;
                    imgReportPO.Visible = true;
                }
                else
                {
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                    imgReportPO.Visible = true;
                }
            }
        }
        protected void gvOrderDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //     gvOrderDetails.PageIndex = e.NewPageIndex;
        }
        protected void gvOrderDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblQuantityValue = (Label)e.Row.FindControl("lblQuantity");
                Label lblPurchasePriceValue = (Label)e.Row.FindControl("lblPurchasePrice");
                Label lblTotalPriceValue = (Label)e.Row.FindControl("lblTotalPrice");

                lblTotalPriceValue.Text = (Convert.ToDecimal(lblQuantityValue.Text) * Convert.ToDecimal(lblPurchasePriceValue.Text)).ToString();
            }
        }
        //************************ User Defined Function ********************//
        private void LoadIsPurchaseOrderApprovalEnable()
        {
            IsPurchaseOrderApprovalEnable = 1;
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
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
            ddlPurchasePriceLocal.DataSource = fields;
            ddlPurchasePriceLocal.DataTextField = "FieldValue";
            ddlPurchasePriceLocal.DataValueField = "FieldId";
            ddlPurchasePriceLocal.DataBind();
            ddlPurchasePriceLocal.SelectedIndex = 0;

            ShowHideCurrencyInformation();
        }
        private void AddEditODeleteDetail()
        {
            //Delete------------
            if (Session["arrayDelete"] == null)
            {
                arrayDelete = new ArrayList();
                Session.Add("arrayDelete", arrayDelete);
            }
            else
                arrayDelete = Session["arrayDelete"] as ArrayList;
        }
        private bool IsOrderFormValid()
        {
            int Count = 0;
            bool status = true;
            List<PMPurchaseOrderDetailsBO> List = Session["PMPurchaseOrderDetails"] as List<PMPurchaseOrderDetailsBO>;
            if (List != null)
            {
                Count = List.Count;
            }

            if (string.IsNullOrEmpty(txtReceivedByDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Enter Delivery Date", AlertType.Warning);
                txtReceivedByDate.Focus();
                status = false;
            }
            else if (ddlSupplier.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Select Company Name", AlertType.Warning);
                ddlSupplier.Focus();
                status = false;
            }
            else if (Count == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Add Order Detail Information", AlertType.Warning);
                ddlPRNumber.Focus();
                status = false;
            }

            return status;
        }
        private bool IsDetailsFormValid()
        {
            bool status = true;
            Decimal number;
            if (ddlProductId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Select Product", AlertType.Warning);
                ddlProductId.Focus();
                status = false;
            }
            else if (string.IsNullOrEmpty(txtQuantity.Text))
            {
                txtQuantity.Text = "";
                CommonHelper.AlertInfo(innboardMessage, "Please Enter Valid Quantity", AlertType.Warning);
                txtQuantity.Focus();
                status = false;
            }
            else if (!Decimal.TryParse(txtQuantity.Text, out number))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Enter Valid Quantity", AlertType.Warning);
                txtQuantity.Focus();
                status = false;
            }
            else if (string.IsNullOrEmpty(txtPurchasePrice.Text))
            {
                txtPurchasePrice.Text = "";
                CommonHelper.AlertInfo(innboardMessage, "Please Enter Valid Unit Price", AlertType.Warning);
                txtPurchasePrice.Focus();
                status = false;
            }
            else if (!Decimal.TryParse(txtPurchasePrice.Text, out number))
            {
                txtPurchasePrice.Text = "";
                CommonHelper.AlertInfo(innboardMessage, "Please Enter Valid Unit Price", AlertType.Warning);
                txtPurchasePrice.Focus();
                status = false;
            }
            return status;
        }
        public void LoadAffiliatedCompany()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = guestCompanyDA.GetAffiliatedGuestCompanyInfo();
            ddlSupplier.DataSource = files;
            ddlSupplier.DataTextField = "CompanyName";
            ddlSupplier.DataValueField = "CompanyId";
            ddlSupplier.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            ddlSupplier2.DataSource = files;
            ddlSupplier2.DataTextField = "CompanyName";
            ddlSupplier2.DataValueField = "CompanyId";
            ddlSupplier2.DataBind();
            ddlSupplier2.Items.Insert(0, item);
        }
        private void LoadPRNumber()
        {
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "Ad Hoc Purchase";
            ddlPRNumber.Items.Insert(0, item);
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllInvItemCatagoryInfoByServiceType("Product");
            ddlCategory.DataSource = List;
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---All---";
            ddlCategory.Items.Insert(0, item);
        }
        private void LoadProductInfo()
        {
            if (ddlPRNumber.SelectedValue == "0")
            {
                InvItemDA productDA = new InvItemDA();
                ddlProductId.DataSource = productDA.GetInvItemInfoByCategoryId(0, Convert.ToInt32(ddlCategory.SelectedValue));
                ddlProductId.DataTextField = "NAME";
                ddlProductId.DataValueField = "ItemId";
            }
            else
            {
                PMRequisitionDA entityDA = new PMRequisitionDA();
                ddlProductId.DataSource = entityDA.GetPMRequisitionDetailInfoById(Convert.ToInt32(ddlPRNumber.SelectedValue));
                ddlProductId.DataTextField = "ProductName";
                ddlProductId.DataValueField = "ProductId";
            }

            ddlProductId.DataBind();
        }
        private void LoadAllCostCentreTabInfo()
        {
            CostCentreTabDA entityDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();

            costCentreTabBOList = entityDA.GetAllRestaurantTypeCostCentreTabInfo();

            this.ddlCostCentre.DataSource = costCentreTabBOList;
            this.ddlCostCentre.DataTextField = "CostCenter";
            this.ddlCostCentre.DataValueField = "CostCenterId";
            this.ddlCostCentre.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCostCentre.Items.Insert(0, item);

            this.ddlCostCentre2.DataSource = costCentreTabBOList;
            this.ddlCostCentre2.DataTextField = "CostCenter";
            this.ddlCostCentre2.DataValueField = "CostCenterId";
            this.ddlCostCentre2.DataBind();
            this.ddlCostCentre2.Items.Insert(0, item);

            ListItem item2 = new ListItem();
            item2.Value = "0";
            item2.Text = hmUtility.GetDropDownFirstAllValue();

            this.ddlSrcCostCenter.DataSource = costCentreTabBOList;
            this.ddlSrcCostCenter.DataTextField = "CostCenter";
            this.ddlSrcCostCenter.DataValueField = "CostCenterId";
            this.ddlSrcCostCenter.DataBind();
            this.ddlSrcCostCenter.Items.Insert(0, item2);            
        }
        private void LoadStockBy()
        {
            List<InvUnitHeadBO> headListBO = new List<InvUnitHeadBO>();
            InvUnitHeadDA da = new InvUnitHeadDA();
            headListBO = da.GetInvUnitHeadInfo();

            this.ddlStockBy.DataSource = headListBO;
            this.ddlStockBy.DataTextField = "HeadName";
            this.ddlStockBy.DataValueField = "UnitHeadId";
            this.ddlStockBy.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlStockBy.Items.Insert(0, item);
        }
        //private void LoadUserInformation()
        //{
        //    UserInformationDA entityDA = new UserInformationDA();
        //    List<UserInformationBO> userInformationListBO = new List<UserInformationBO>();
        //    userInformationListBO = entityDA.GetUserInformation().Where(x => x.UserGroupId != 8 && x.UserGroupId != 9).ToList();

        //    this.ddlCheckedBy.DataSource = userInformationListBO;
        //    this.ddlCheckedBy.DataTextField = "UserName";
        //    this.ddlCheckedBy.DataValueField = "UserInfoId";
        //    this.ddlCheckedBy.DataBind();

        //    ListItem itemEmployee = new ListItem();
        //    itemEmployee.Value = "0";
        //    itemEmployee.Text = hmUtility.GetDropDownFirstValue();
        //    this.ddlCheckedBy.Items.Insert(0, itemEmployee);

        //    this.ddlApprovedBy.DataSource = userInformationListBO;
        //    this.ddlApprovedBy.DataTextField = "UserName";
        //    this.ddlApprovedBy.DataValueField = "UserInfoId";
        //    this.ddlApprovedBy.DataBind();

        //    this.ddlApprovedBy.Items.Insert(0, itemEmployee);
        //}
        //private void SetTab(string TabName)
        //{
        //    if (TabName == "SearchTab")
        //    {
        //        B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
        //        A.Attributes.Add("class", "ui-state-default ui-corner-top");
        //    }
        //    else if (TabName == "EntryTab")
        //    {
        //        A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
        //        B.Attributes.Add("class", "ui-state-default ui-corner-top");
        //    }
        //}
        private void ShowHideCurrencyInformation()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("LocalNUsd");

            List<CommonCurrencyBO> localCurrencyListBO = new List<CommonCurrencyBO>();
            localCurrencyListBO = currencyListBO.Where(x => x.CurrencyType == "Local").ToList();

            ddlPurchasePriceLocal.DataSource = localCurrencyListBO;
            ddlPurchasePriceLocal.DataTextField = "CurrencyName";
            ddlPurchasePriceLocal.DataValueField = "CurrencyId";
            ddlPurchasePriceLocal.DataBind();
            ddlPurchasePriceLocal.SelectedIndex = 0;
            lblPurchasePriceLocal.Text = "Unit Price(" + ddlPurchasePriceLocal.SelectedItem.Text + ")";
        }
        private void LoadSearchInformation()
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                fromDate = CommonHelper.DateTimeToMMDDYYYY(txtFromDate.Text);
            }
            if (!string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                toDate = CommonHelper.DateTimeToMMDDYYYY(txtToDate.Text);
            }

            string PONumber = txtSPONumber.Text;
            string status = ddlStatus.SelectedValue;

            Int32 costCenterId = Convert.ToInt32(ddlSrcCostCenter.SelectedValue);

            PMPurchaseOrderDA detalisDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();
            orderList = detalisDA.GetSMSalesOrderInfoBySearchCriteria("Product", fromDate, toDate, PONumber, costCenterId, status);
            gvOrderInfo.DataSource = orderList;
            gvOrderInfo.DataBind();

            //SetTab("SearchTab");
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        public static void OpenNewBrowserWindow(string Url, Control control)
        {
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Open", "window.open('" + Url + "');", true);
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static InvItemBO GetPurchasePrice(int itemId, int requisitionId)
        {
            PMRequisitionDA productDA = new PMRequisitionDA();
            InvItemBO productBO = new InvItemBO();
            PMPurchaseOrderDA detailsDA = new PMPurchaseOrderDA();
            PMPurchaseOrderDetailsBO Details = new PMPurchaseOrderDetailsBO();

            if (requisitionId == 0)
                productBO = productDA.GetInvItemInfoWithAdhocPurchaseQuantityById(itemId);
            else if (requisitionId > 0)
                productBO = productDA.GetInvItemInfoWithRequsitionQuantityById(itemId, requisitionId);

            return productBO;
        }
        [WebMethod]
        public static List<ItemViewBO> LoadProductListOnPONumberChange(string Category, string PRNumber)
        {
            List<ItemViewBO> viewList = new List<ItemViewBO>();
            if (PRNumber == "0")
            {
                InvItemDA productDA = new InvItemDA();
                var productList = productDA.GetInventoryItemInformationByCategory(0, Convert.ToInt32(Category), null, null);

                for (int i = 0; i < productList.Count; i++)
                {
                    ItemViewBO viewBO = new ItemViewBO();
                    viewBO.ItemId = productList[i].ItemId;
                    viewBO.ItemName = productList[i].Name;
                    viewList.Add(viewBO);
                }
            }
            else
            {
                PMRequisitionDA entityDA = new PMRequisitionDA();
                var requisitionList = entityDA.GetPMRequisitionDetailInfoById(Convert.ToInt32(PRNumber));
                for (int i = 0; i < requisitionList.Count; i++)
                {
                    ItemViewBO viewBO = new ItemViewBO();
                    viewBO.ItemId = requisitionList[i].ItemId;
                    viewBO.ItemName = requisitionList[i].ItemName;
                    viewList.Add(viewBO);
                }
            }

            return viewList;
        }
        [WebMethod]
        public static List<PMPurchaseOrderDetailsBO> PerformLoadPMProductDetailOnDisplayMode(string pOrderId)
        {
            PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
            PMPurchaseOrderDA orderDetailDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderDetailsBO> orderDetailListBO = new List<PMPurchaseOrderDetailsBO>();
            orderDetailListBO = orderDetailDA.GetSMSalesOrderDetailByOrderId(Int32.Parse(pOrderId));
            return orderDetailListBO;
        }
        [WebMethod]
        public static ReturnInfo SavePurchaseOrder(PMPurchaseOrderBO PurchaseOrder, List<PMPurchaseOrderDetailsBO> AddedPurchaseOrderDetails, List<PMPurchaseOrderDetailsBO> EditedPurchaseOrderDetails, List<PMPurchaseOrderDetailsBO> DeletedPurchaseOrderDetails)
        {
            ReturnInfo rtninf = new ReturnInfo();
            int tmpOrderId = 0;
            string porderNumber = string.Empty;
            bool status = false;

            try
            {
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsPurchaseOrderApprovalEnable", "IsPurchaseOrderApprovalEnable");

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                PMPurchaseOrderDA orderDetailsDA = new PMPurchaseOrderDA();

                //PurchaseOrder.ApprovedBy = userInformationBO.UserInfoId;
                PurchaseOrder.POType = "Product";

                if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedEnable))
                {
                    PurchaseOrder.ApprovedStatus = HMConstants.ApprovalStatus.Pending.ToString();
                }
                else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedDisable))
                {
                    PurchaseOrder.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();
                }

                if (PurchaseOrder.POrderId == 0)
                {
                    PurchaseOrder.CreatedBy = userInformationBO.UserInfoId;
                    status = orderDetailsDA.SaveSMSalesOrderInfo(PurchaseOrder, AddedPurchaseOrderDetails, out tmpOrderId, out porderNumber);

                    if (status && Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedDisable))
                    {
                        orderDetailsDA.UpdatePurchaseOrderStatus(tmpOrderId, HMConstants.ApprovalStatus.Approved.ToString(), userInformationBO.UserInfoId);
                    }

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.ProductPurchaseOrder.ToString(), tmpOrderId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductPurchaseOrder));
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }
                else
                {
                    PurchaseOrder.LastModifiedBy = userInformationBO.UserInfoId;
                    status = orderDetailsDA.UpdateSMSalesOrderInfo(PurchaseOrder, AddedPurchaseOrderDetails, EditedPurchaseOrderDetails, DeletedPurchaseOrderDetails);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.ProductPurchaseOrder.ToString(), PurchaseOrder.POrderId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductPurchaseOrder));
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }

                //HMCommonDA commonDa = new HMCommonDA();
                //CustomFieldBO customFieldObject = new CustomFieldBO();
                //customFieldObject = commonDa.GetCustomFieldByFieldName("PurchaseOrderApprovedByEmail");

                //if (customFieldObject != null)
                //{
                //    var po = orderDetailsDA.GetSMSalesOrderInfoByOrderId(PurchaseOrder.POrderId);

                //    EmailHelper.SendEmail(string.Empty, customFieldObject.FieldValue.ToString(), "Approval Pending For Sales Order No " + po.PONumber,
                //        "Please Approved The Sales Order.", string.Empty);
                //}

                if (!status)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod]
        public static PurchaseOrderViewBO FillForms(int pOrderId)
        {
            PurchaseOrderViewBO viewBo = new PurchaseOrderViewBO();
            PMPurchaseOrderDA orderDetailDA = new PMPurchaseOrderDA();

            viewBo.PurchaseOrder = orderDetailDA.GetSMSalesOrderInfoByOrderId(pOrderId);
            viewBo.PurchaseOrderDetails = orderDetailDA.GetSMSalesOrderDetailByOrderId(pOrderId);

            return viewBo;
        }
        [WebMethod]
        public static PurchaseOrderViewBO FillFormForTemplate2(int pOrderId)
        {
            PurchaseOrderViewBO viewBo = new PurchaseOrderViewBO();
            PMPurchaseOrderDA orderDetailDA = new PMPurchaseOrderDA();

            viewBo.PurchaseOrder = orderDetailDA.GetPMPurchaseOrderInfoByOrderId(pOrderId);
            viewBo.PurchaseOrderDetails = orderDetailDA.GetPMPurchaseOrderDetailByOrderId(pOrderId);

            viewBo.CostCenterId = viewBo.PurchaseOrderDetails[0].CostCenterId;

            string grid = string.Empty, tr = string.Empty;
            int rowCount = 0, poDetailId = 0, isEdited = 0;
            decimal grandTotal = 0;

            foreach (PMPurchaseOrderDetailsBO pod in viewBo.PurchaseOrderDetails)
            {
                if (rowCount % 2 == 0)
                {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else
                {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:25%;'>" + pod.ProductName + "</td>";
                tr += "<td style='width:15%;'>" + pod.StockBy + "</td>";
                tr += "<td style='width:15%;'>" + pod.StockQuantity + "</td>";
                tr += "<td style='width:15%;'>" + pod.PurchasePrice + "</td>";

                tr += "<td style='width:15%; text-align:center;'> <input type='text' id='txt" + pod.ProductId.ToString() + "' value = '" + pod.Quantity + "' onblur='CheckInputValue(this)' style='width:65px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";

                tr += "<td style='width:15%; text-align:right; font-weight: bold;'>" + (pod.Quantity * pod.PurchasePrice).ToString("0.00") + "</td>";

                tr += "<td style='display:none'>" + pod.DetailId + "</td>";
                tr += "<td style='display:none'>" + pod.ProductId + "</td>";
                tr += "<td style='display:none'>" + pod.StockById + "</td>";
                tr += "<td style='display:none'>" + pod.Quantity + "</td>";
                tr += "<td style='display:none'>" + isEdited + "</td>";
                tr += "</tr>";

                grandTotal += (pod.Quantity * pod.PurchasePrice);
                rowCount++;
            }

            grid += GridHeader() + tr + "</tbody> " + GridFooter(grandTotal) + " </table>";
            viewBo.PurchaseOrderGrid = grid;

            viewBo.PurchaseOrderDetails = null;

            return viewBo;
        }
        [WebMethod]
        public static string LoadItemForPurchaseBySupplier(int supplierId, int costCenterId)
        {
            InvItemDA itemDa = new InvItemDA();
            List<InvItemBO> item = new List<InvItemBO>();
            item = itemDa.GetItemBySupplier(supplierId, costCenterId);

            string grid = string.Empty, tr = string.Empty;
            int rowCount = 0, poDetailId = 0, isEdited = 0;

            foreach (InvItemBO itm in item)
            {
                if (rowCount % 2 == 0)
                {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else
                {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:25%;'>" + itm.Name + "</td>";
                tr += "<td style='width:15%;'>" + itm.HeadName + "</td>";
                tr += "<td style='width:15%;'>" + itm.StockQuantity + "</td>";
                tr += "<td style='width:15%;'>" + itm.PurchasePrice + "</td>";

                tr += "<td style='width:15%; text-align:center;'> <input type='text' id='txt" + itm.ItemId.ToString() + "' value = '" + itm.Quantity + "' onblur='CheckInputValue(this)' style='width:65px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";

                tr += "<td style='width:15%; text-align:right; font-weight: bold;'></td>";

                tr += "<td style='display:none'>" + poDetailId + "</td>";
                tr += "<td style='display:none'>" + itm.ItemId + "</td>";
                tr += "<td style='display:none'>" + itm.StockBy + "</td>";
                tr += "<td style='display:none'>0</td>";
                tr += "<td style='display:none'>" + isEdited + "</td>";

                tr += "</tr>";

                rowCount++;
            }

            grid += GridHeader() + tr + "</tbody> " + GridFooter(0) + " </table>";
            return grid;
        }
        public static string GridHeader()
        {
            string gridHead = string.Empty;
            gridHead += "<table id='ProductPurchaseGrid' class='table table-bordered table-condensed table-responsive' style='width: 100%;'>" +
                         "       <thead>" +
                         "           <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>" +
                         "               <th style='width: 25%;'>" +
                         "                   Item" +
                         "               </th>" +
                         "               <th style='width: 15%;'>" +
                         "                   Stock By" +
                         "               </th>" +
                         "               <th style='width: 15%;'>" +
                         "                   Current Stock" +
                         "               </th>" +
                         "               <th style='width: 15%;'>" +
                         "                   Purchase Price" +
                         "               </th>" +
                         "               <th style='width: 15%;'>" +
                         "                   Quantity" +
                         "               </th>" +
                         "               <th style='width: 15%'>" +
                         "                   Total Amount" +
                         "               </th>" +
                         "               <th style='display: none'>" +
                         "                   poDetailId" +
                         "               </th>" +
                         "               <th style='display: none'>" +
                         "                   ItemId" +
                         "               </th>" +
                         "               <th style='display: none'>" +
                         "                   StockById" +
                         "               </th>" +
                         "               <th style='display: none'>" +
                         "                   DBQuantity" +
                         "               </th>" +
                         "               <th style='display: none'>" +
                         "                   IsEdited" +
                         "               </th>" +
                         "           </tr>" +
                         "       </thead>" +
                         "       <tbody>";

            return gridHead;
        }
        public static string GridFooter(decimal totalPrice)
        {
            string gridFooter = string.Empty;
            gridFooter += "<tfoot>" +
                            "<tr style='color: White; background-color: #62737D; text-align: left; font-weight: bold;'>" +
                              "<td colspan='5' style='height:21px; text-align:right;'>Grand Total: </td> " +
                              "<td style='height:21px; text-align:right;'>" + totalPrice.ToString("0.00") + "</td> " +
                            "</tr>" +
                            "</tfoot>";

            return gridFooter;
        }
        [WebMethod]
        public static List<InvItemCostCenterMappingBO> LoadCurrentStockQuantity(int costcenterId, int itemId)
        {
            InvItemCostCenterMappingDA mappingDA = new InvItemCostCenterMappingDA();
            List<InvItemCostCenterMappingBO> boList = new List<InvItemCostCenterMappingBO>();
            boList = mappingDA.GetInvItemCostCenterMappingInfo(costcenterId, itemId);
            return boList;
        }
        [WebMethod]
        public static InvItemAutoSearchBO GetItemNameForAutoSearch(string itemName, int costcenterId)
        {
            List<InvItemAutoSearchBO> itemInfoListBO = new List<InvItemAutoSearchBO>();
            int categoryId = 0;

            InvItemDA itemDA = new InvItemDA();
            InvItemAutoSearchBO itemInfo = new InvItemAutoSearchBO();
            itemInfoListBO = itemDA.GetItemNameForAutoSearch(itemName, categoryId, costcenterId);
            if (itemInfoListBO != null)
            {
                if (itemInfoListBO.Count > 0)
                {
                    itemInfo = itemInfoListBO[0];
                }
            }
            return itemInfo;
        }
        [WebMethod]
        public static InvItemAutoSearchBO GetItemInformationForAutoSearch(int companyId, int itemId, int costcenterId)
        {
            List<InvItemAutoSearchBO> itemInfoListBO = new List<InvItemAutoSearchBO>();
            int categoryId = 0;

            InvItemDA itemDA = new InvItemDA();
            InvItemAutoSearchBO itemInfo = new InvItemAutoSearchBO();
            itemInfoListBO = itemDA.GetItemInformationForAutoSearch(companyId, itemId, categoryId, costcenterId);
            if (itemInfoListBO != null)
            {
                if (itemInfoListBO.Count > 0)
                {
                    itemInfo = itemInfoListBO[0];
                }
            }
            return itemInfo;
        }
        [WebMethod]
        public static List<InvUnitHeadBO> LoadRelatedStockBy(int stockById)
        {
            InvUnitHeadDA unitHeadDA = new InvUnitHeadDA();
            List<InvUnitHeadBO> unitHeadList = new List<InvUnitHeadBO>();
            unitHeadList = unitHeadDA.GetRelatedStockBy(stockById);
            return unitHeadList;
        }
    }
}