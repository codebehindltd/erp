using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using System.Collections;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.SalesManagment;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using Newtonsoft.Json;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmPMProductReceive : System.Web.UI.Page
    {
        ArrayList arrayDelete;
        HiddenField innboardMessage;
        protected int _RestaurantComboId;
        protected int IsService = -1;
        protected int btnPadding = -1;
        HMUtility hmUtility = new HMUtility();
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        List<int> paymentIdlist = new List<int>();
        protected int isSupplierPaymentEnableWhenProductReceive = 1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
            invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("ReceivedProductTemplate", "ReceivedProductTemplate");

            if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.ProductReceiveTemplate.Template1))
            {
                ProductReceivedTemplate1.Visible = true;
                ProductReceivedTemplate2.Visible = false;
                hfReceivedProductTemplate.Value = "1";
            }
            else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.ProductReceiveTemplate.Template2))
            {
                ProductReceivedTemplate1.Visible = false;
                ProductReceivedTemplate2.Visible = true;
                hfReceivedProductTemplate.Value = "2";
            }
            IsSupplierPaymentEnableWhenProductReceive();
            if (!IsPostBack)
            {
                LoadCommonDropDownHiddenField();
                LoadCurrency();
                LoadLocalCurrencyId();
                LoadCostCenter();
                LoadSupplierInfo();
                LoadStockBy();
                LoadPurchaseOrder();
                LoadTemplateNo();
                txtQuantity_Serial.Attributes.Add("class", "CustomTextBox");

                if (Session["ProductReceivedId"] != null)
                {
                    hfIsEditedFromApprovedForm.Value = "1";
                    hfReceivedId.Value = Session["ProductReceivedId"].ToString();

                    Session.Remove("ProductReceivedId");
                }
                Session["PRPaymentDetailListForGrid"] = null;
                Session["PRPaymentDeleteList"] = null;
            }
            arrayDelete = new ArrayList();
        }
        protected void ddlProductId_SelectedIndexChanged(object sender, EventArgs e)
        {
            InvItemDA productDA = new InvItemDA();
            InvItemBO productBO = new InvItemBO();
            int selectedProduct = Int32.Parse(ddlProductId.SelectedValue);
            productBO = productDA.GetInvItemInfoById(0, selectedProduct);
            if (productBO.ProductType == "Serial Product")
            {
                lblQuantity_Serial.Text = "Serial Number";
                txtQuantity_Serial.Attributes.Add("class", "ThreeColumnTextBox defaultLabelHeight");
                btnPadding = 1;
            }
            else
            {
                lblQuantity_Serial.Text = "Quantity";
                txtQuantity_Serial.Attributes.Add("class", "CustomTextBox");
            }
            this.ProductQuantityStatus();
        }
        protected void ddlPOrderId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadProduct();
            this.ProductQuantityStatus();
        }
        protected void gvProductReceive_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                var item = (PMProductReceivedBO)e.Row.DataItem;

                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgCancel = (ImageButton)e.Row.FindControl("ImgBtnCancelReceived");
                ImageButton imgActive = (ImageButton)e.Row.FindControl("ImgBtnActiveReceived");
                ImageButton imgReportPR = (ImageButton)e.Row.FindControl("ImgReportPR");

                if (item.Status == "Pending")
                {
                    imgUpdate.Visible = true;
                    imgCancel.Visible = true;
                    imgActive.Visible = false;
                    imgReportPR.Visible = false;
                }
                else if (item.Status == "Cancel")
                {
                    imgUpdate.Visible = true;
                    imgCancel.Visible = false;
                    imgActive.Visible = true;
                    imgReportPR.Visible = false;
                }
                else
                {
                    imgUpdate.Visible = false;
                    imgCancel.Visible = false;
                    imgActive.Visible = false;
                    imgReportPR.Visible = true;
                }
            }
        }
        protected void gvProductReceive_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvProductReceiveInfo.PageIndex = e.NewPageIndex;
        }
        protected void gvProductReceive_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                if (e.CommandName == "CmdItemReceivedCancel")
                {
                    int receivedId = Convert.ToInt32(e.CommandArgument.ToString());
                    string result = string.Empty;

                    PMProductReceivedDA productReceiveDa = new PMProductReceivedDA();
                    PMProductReceivedBO receivedProduct = new PMProductReceivedBO();

                    receivedProduct.ReceivedId = receivedId;
                    receivedProduct.LastModifiedBy = userInformationBO.UserInfoId;
                    receivedProduct.Status = "Cancel";

                    bool status = productReceiveDa.UpdateProductReceiveStatus(receivedProduct);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Cancel, AlertType.Success);
                        LoadGridview();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }

                    this.SetTab("SearchTab");
                }
                if (e.CommandName == "CmdItemReceivedActive")
                {
                    int receivedId = Convert.ToInt32(e.CommandArgument.ToString());
                    string result = string.Empty;

                    PMProductReceivedDA productReceiveDa = new PMProductReceivedDA();
                    PMProductReceivedBO receivedProduct = new PMProductReceivedBO();

                    receivedProduct.ReceivedId = receivedId;
                    receivedProduct.LastModifiedBy = userInformationBO.UserInfoId;
                    receivedProduct.Status = "Pending";

                    bool status = productReceiveDa.UpdateProductReceiveStatus(receivedProduct);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Cancel, AlertType.Success);
                        LoadGridview();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }

                    this.SetTab("SearchTab");
                }
                else if (e.CommandName == "CmdReportPR")
                {
                    int receivedId = Convert.ToInt32(e.CommandArgument.ToString());

                    string url = "/Inventory/Reports/frmReportProductReceive.aspx?PRId=" + receivedId;
                    string s = "window.open('" + url + "', 'popup_window', 'width=820,height=680,left=300,top=50,resizable=yes');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            hfIsEditedFromApprovedForm.Value = "0";
            hfReceivedId.Value = "0";
            LoadGridview();
        }
        //************************ User Defined Function ********************//
        public void LoadGridview()
        {
            this.SetTab("SearchTab");
            PMProductReceivedDA receiveDA = new PMProductReceivedDA();
            List<PMProductReceivedBO> receivedList = new List<PMProductReceivedBO>();
            string orderId = string.Empty, costCenterId = string.Empty, status = string.Empty;

            string startDate = string.Empty;
            string endDate = string.Empty;

            DateTime dateTime = DateTime.Now;
            DateTime FromDate = dateTime;
            DateTime ToDate = dateTime;

            if (!string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                startDate = txtFromDate.Text;
                FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                endDate = txtToDate.Text;
                ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            if (ddlSearchCostCenterId.SelectedValue != "0")
            {
                costCenterId = ddlSearchCostCenterId.SelectedValue;
            }
            else
            {
                costCenterId = string.Empty;
            }

            if (ddlSearchPOrderId.SelectedValue == "-1")
            {
                orderId = string.Empty;
            }
            else
            {
                orderId = ddlSearchPOrderId.SelectedValue;
            }

            //receivedList = receiveDA.GetProductreceiveInfo(FromDate, ToDate, orderId, costCenterId, status);

            gvProductReceiveInfo.DataSource = receivedList;
            gvProductReceiveInfo.DataBind();
        }
        public void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
            invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsUserpermissionAppliedToCostcenterFilteringForPOPR", "IsUserpermissionAppliedToCostcenterFilteringForPOPR");

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (Convert.ToInt16(invoiceTemplateBO.SetupValue) != 0)
            {
                costCentreTabBOList = costCentreTabDA.GetUserWisePRPOCostCentreTabInfo(userInformationBO.UserInfoId, HMConstants.CostcenterFilteringForPOPR.Purchase.ToString());
            }
            else
            {
                costCentreTabBOList = costCentreTabDA.GetUserWisePRPOCostCentreTabInfo(userInformationBO.UserInfoId, null);
            }

            this.ddlCostCenterId.DataSource = costCentreTabBOList;
            this.ddlCostCenterId.DataTextField = "CostCenter";
            this.ddlCostCenterId.DataValueField = "CostCenterId";
            this.ddlCostCenterId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCostCenterId.Items.Insert(0, item);

            ddlSearchCostCenterId.DataSource = costCentreTabBOList;
            this.ddlSearchCostCenterId.DataTextField = "CostCenter";
            this.ddlSearchCostCenterId.DataValueField = "CostCenterId";
            this.ddlSearchCostCenterId.DataBind();

            ListItem item2 = new ListItem();
            item2.Value = "0";
            item2.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlSearchCostCenterId.Items.Insert(0, item2);

            this.ddlCostCenterId2.DataSource = costCentreTabBOList;
            this.ddlCostCenterId2.DataTextField = "CostCenter";
            this.ddlCostCenterId2.DataValueField = "CostCenterId";
            this.ddlCostCenterId2.DataBind();
            this.ddlCostCenterId2.Items.Insert(0, item);
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
        private void LoadLocalCurrencyId()
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetLocalCurrencyInfo();

            //LocalCurrencyId = commonCurrencyBO.CurrencyId;
            hfLocalCurrencyId.Value = commonCurrencyBO.CurrencyId.ToString();
        }
        private void IsSupplierPaymentEnableWhenProductReceive()
        {
            isSupplierPaymentEnableWhenProductReceive = 0;
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
                    lblPurchasePriceLocal.Text = "Unit Price(" + ddlPurchasePriceLocal.SelectedItem.Text + ")";
                }
            }
        }
        private void LoadSupplierInfo()
        {
            PMSupplierDA entityDA = new PMSupplierDA();
            List<PMSupplierBO> supplierInfo = new List<PMSupplierBO>();
            supplierInfo = entityDA.GetPMSupplierInfo();

            ddlSupplier.DataSource = supplierInfo;
            ddlSupplier.DataTextField = "Name";
            ddlSupplier.DataValueField = "SupplierId";
            ddlSupplier.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSupplier.Items.Insert(0, item);

            hfAdhocSupplierId.Value = "0";
            List<PMSupplierBO> adHocSupplierInfo = new List<PMSupplierBO>();
            adHocSupplierInfo = supplierInfo.Where(x => x.IsAdhocSupplier == 1).ToList();
            if (adHocSupplierInfo != null)
            {
                if (adHocSupplierInfo.Count > 0)
                {
                    hfAdhocSupplierId.Value = adHocSupplierInfo[0].SupplierId.ToString();
                }
            }
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
        private void LoadPurchaseOrder()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            PMPurchaseOrderDA orderDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();
            orderList = orderDA.GetApprovedPMPurchaseOrderInfo(userInformationBO.UserGroupId, "Product");

            this.ddlPOrderId.DataSource = orderList;
            this.ddlPOrderId.DataTextField = "PONumber";
            this.ddlPOrderId.DataValueField = "POrderId";
            this.ddlPOrderId.DataBind();
            ListItem item = new ListItem();
            item.Value = "-1";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlPOrderId.Items.Insert(0, item);

            ddlSearchPOrderId.DataSource = orderList;
            ddlSearchPOrderId.DataTextField = "PONumber";
            ddlSearchPOrderId.DataValueField = "POrderId";
            ddlSearchPOrderId.DataBind();

            ListItem item3 = new ListItem();
            item3.Value = "-1";
            //item3.Text = hmUtility.GetDropDownFirstValue();
            item3.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlSearchPOrderId.Items.Insert(0, item3);

            ListItem item2 = new ListItem();
            item2.Value = "0";
            item2.Text = hmUtility.GetDropDownFirstValue();
            this.ddlPOrderId2.Items.Insert(0, item2);

            if (hfAdhocSupplierId.Value != "0")
            {
                ListItem itemAdhoc = new ListItem();
                itemAdhoc.Value = "0";
                itemAdhoc.Text = "Ad Hoc Receive";
                this.ddlPOrderId.Items.Insert(1, itemAdhoc);
                this.ddlSearchPOrderId.Items.Insert(1, itemAdhoc);
            }
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void ProductQuantityStatus()
        {
            PMPurchaseOrderDA productDA = new PMPurchaseOrderDA();
            PMPurchaseOrderDetailsBO productBO = new PMPurchaseOrderDetailsBO();
            int pOrderId = Int32.Parse(this.ddlPOrderId.SelectedValue);
            int selectedProduct = Int32.Parse(this.ddlProductId.SelectedValue);
            productBO = productDA.GetPMPurchaseOrderDetailsByProductId(pOrderId, selectedProduct);

            if (productBO != null)
            {
                if ((productBO.Quantity + productBO.QuantityReceived) != 0)
                {
                    this.lblPOQuantity.Text = productBO.Quantity.ToString();
                    this.lblReceivedQuantity.Text = productBO.QuantityReceived.ToString();
                }
            }
        }
        private void LoadProduct()
        {
            int POId = Int32.Parse(ddlPOrderId.SelectedValue);

            if (POId != 0)
            {
                PMPurchaseOrderDA entityDA = new PMPurchaseOrderDA();
                ddlProductId.DataSource = entityDA.GetAvailableItemForPOrderId(POId, null, 0);
            }
            else
            {
                InvItemDA entityDA = new InvItemDA();
                ddlProductId.DataSource = entityDA.GetInvItemInfo();
            }

            this.ddlProductId.DataTextField = "Name";
            this.ddlProductId.DataValueField = "ItemId";
            this.ddlProductId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlProductId.Items.Insert(0, item);
        }
        private void LoadTemplateNo()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("ReceivedProductTemplate", "ReceivedProductTemplate");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    hfTemplate.Value = commonSetupBO.SetupValue;
                }
            }
        }
        public bool isValidForm()
        {
            bool status = true;
            if (Session["PMProductReceive"] == null)
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Quantity.", AlertType.Warning);
                this.ddlProductId.Focus();
            }
            return status;

        }
        private void ClearForm()
        {
            Session["PMProductReceive"] = null;
            this.txtQuantity_Serial.Text = "";
            this.ddlPOrderId.SelectedIndex = 0;
            this.ddlProductId.SelectedIndex = 0;

        }
        private int ValidSerialNumber()
        {
            int tmpSerialId = 0;
            PMProductSerialInfoDA entityDA = new PMProductSerialInfoDA();
            PMProductSerialInfoBO entityBO = new PMProductSerialInfoBO();

            if (entityBO != null)
            {
                tmpSerialId = entityBO.SerialId;
            }
            return tmpSerialId;
        }
        private void clearProductFields()
        {
            txtQuantity_Serial.Text = "";
        }
        private bool IsDetailsFormValid()
        {
            bool status = true;
            decimal quan;
            Decimal.TryParse(txtQuantity_Serial.Text, out quan);
            if (lblQuantity_Serial.Text == "Quantity")
            {
                if (quan <= 0)
                {
                    status = false;
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Quantity.", AlertType.Warning);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtQuantity_Serial.Text))
                {
                    status = false;
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Serial Number.", AlertType.Warning);
                }
            }
            return status;
        }
        private void LoadProductItem()
        {
            InvItemDA productDA = new InvItemDA();
            this.ddlProductId.DataSource = productDA.GetInvItemInfo();
            this.ddlProductId.DataTextField = "Name";
            this.ddlProductId.DataValueField = "ItemId";
            this.ddlProductId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlProductId.Items.Insert(0, item);
        }
        private void LoadProductGridView()
        {
            //  gvProductReceive.DataSource = null;
            //  gvProductReceive.DataBind();
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
        public static string GetProductReceiveDetailsGridView(List<PMProductReceivedBO> dataSource)
        {
            string strTable = "";
            if (dataSource != null)
            {
                strTable += "<table style='width:100%' cellspacing='0' cellpadding='4' id='ProductDetailGrid'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Product Name</th><th align='left' scope='col'>Order Code</th><th align='left' scope='col'>Serial Number</th><th align='left' scope='col'>Quantity</th><th align='center' scope='col'>Action</th></tr>";
                int counter = 0;
                foreach (PMProductReceivedBO dr in dataSource)
                {
                    counter++;
                    if (counter % 2 == 0)
                    {
                        //strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 20%;'>" + dr.ProductName + "</td>";
                    }
                    else
                    {
                        // strTable += "<tr style='background-color:White;'><td align='left' style='width: 25%;'>" + dr.ProductName + "</td>";
                    }
                    strTable += "<td align='left' style='width: 20%;'>" + dr.OrderCode + "</td>";
                    // strTable += "<td align='left' style='width: 20%;'>" + dr.SerialNumber + "</td>";
                    // strTable += "<td align='left' style='width: 20%;'>" + dr.Quantity + "</td>";
                    strTable += "<td align='center' style='width: 15%;'>";
                    strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformProductReceiveDelete(" + dr.ReceivedId + ")' alt='Delete Information' border='0' />";
                    strTable += "</td></tr>";
                }
                strTable += "</table>";
                if (strTable == "")
                {
                    strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
                }
            }
            return strTable;
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        public static string LoadGuestPaymentDetailGridViewByWM(string paymentDescription, int receivedId)
        {
            string strTable = "";
            List<PMProductReceivedBillPaymentBO> detailList = new List<PMProductReceivedBillPaymentBO>();
            if (receivedId == 0)
            {
                detailList = HttpContext.Current.Session["PRPaymentDetailListForGrid"] as List<PMProductReceivedBillPaymentBO>;
            }
            else if (receivedId > 0)
            {
                PMProductReceivedDA productReceivedDA = new PMProductReceivedDA();
                detailList = productReceivedDA.GetProductreceiveBillPaymentInfo(receivedId);
            }

            if (detailList != null)
            {
                strTable += "<table style='width:100%' id='ReservationDetailGrid' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th style='display:none'></th><th align='left' scope='col'>Payment Mode</th><th align='left' scope='col'>Description</th><th align='left' scope='col'>Amount</th><th align='center' scope='col'>Action</th></tr>";
                int counter = 0;
                foreach (PMProductReceivedBillPaymentBO dr in detailList)
                {
                    counter++;
                    strTable += "<td align='left' style=\"display:none;\">" + dr.PaymentId + "</td>";
                    if (counter % 2 == 0)
                    {
                        // It's even
                        strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 40%;'>" + dr.PaymentMode + "</td>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:White;'><td align='left' style='width: 40%;'>" + dr.PaymentMode + "</td>";
                    }
                    strTable += "<td align='left' style='width: 40%;'>" + dr.PaymentDescription + "</td>";
                    strTable += "<td align='left' style='width: 20%;'>" + dr.PaymentAmount + "</td>";
                    strTable += "<td align='center' style='width: 15%;'>";
                    strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformGuestPaymentDetailDelete(" + dr.PaymentId + ")' alt='Delete Information' border='0' />";
                    strTable += "</td></tr>";
                }
                strTable += "</table>";
                if (strTable == "")
                {
                    strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
                }
            }
            return strTable;
        }
        public static string GridHeader()
        {
            string gridHead = string.Empty;

            gridHead += "<table id='ProductReceivedGrid' style='width: 100%;' class='table table-bordered table-condensed table-responsive'>" +
                         "       <thead>" +
                         "           <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>" +
                         "               <th style='width: 20%;'>" +
                         "                   Item" +
                         "               </th>" +
                         "               <th style='width: 10%;'>" +
                         "                   Stock By" +
                         "               </th>" +
                         "               <th style='width: 10%'>" +
                         "                   Current Stock" +
                         "               </th>" +
                         "               <th style='width: 10%;'>" +
                         "                   Purchase Price" +
                         "               </th>" +
                         "               <th style='width: 10%;'>" +
                         "                   Purchse Quantity" +
                         "               </th>" +
                         "               <th style='width: 14%'>" +
                         "                   Already Received" +
                         "               </th>" +
                         "               <th style='width: 14%'>" +
                         "                   Remaining Quantity" +
                         "               </th>" +
                         "               <th style='width: 12%'>" +
                         "                   Received Quantity" +
                         "               </th>" +
                         "               <th style='display: none'>" +
                         "                   receiveDetailId" +
                         "               </th>" +
                         "               <th style='display: none'>" +
                         "                   ItemId" +
                         "               </th>" +
                         "               <th style='display: none'>" +
                         "                   StockById" +
                         "               </th>" +
                         "               <th style='display: none'>" +
                         "                   Default Location Id" +
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
                              "<td style='height:21px; text-align:right;'>PO. Grand Total: </td> " +
                              "<td colspan='7' style='height:21px; text-align:left;'>" + totalPrice.ToString("0.00") + "</td> " +
                            "</tr>" +
                            "</tfoot>";

            return gridFooter;
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<InvLocationBO> GetInvLocationInfoByCostCenter(int costCenterId)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvItemLocationByCostCenter(costCenterId);

            return location;
        }
        [WebMethod(EnableSession = true)]
        public static PMPurchaseOrderDetailsBO LoadProductQuantityStatus(string POrderId, string ProductId)
        {

            PMPurchaseOrderDA productDA = new PMPurchaseOrderDA();
            PMPurchaseOrderDetailsBO productBO = new PMPurchaseOrderDetailsBO();
            int pOrderId = Int32.Parse(POrderId);
            int selectedProduct = Int32.Parse(ProductId);
            productBO = productDA.GetPMPurchaseOrderDetailsByProductId(pOrderId, selectedProduct);
            return productBO;

        }
        [WebMethod(EnableSession = true)]
        public static List<PMPurchaseOrderDetailsBO> LoadProductForNotAdhocByOrderId(string POrderId)
        {
            List<PMPurchaseOrderDetailsBO> productList = new List<PMPurchaseOrderDetailsBO>();
            int POId = Int32.Parse(POrderId);
            if (POId != 0)
            {
                PMPurchaseOrderDA entityDA = new PMPurchaseOrderDA();
                productList = entityDA.GetAvailableItemForPOrderId(POId, null, 0);
            }
            else
            {
                InvItemDA entityDA = new InvItemDA();
                //productList = entityDA.GetInvItemInfo();
            }

            return productList;
        }
        [WebMethod(EnableSession = true)]
        public static string AddProductReceiveInformation(string costCenterId, string ddlProductId, string txtQuantity_Serial, string lblQuantity_Serial, string lblPOQuantity, string lblReceivedQuantity, string ddlPOrderId, string hfOrderDetailId)
        {
            string gridView = string.Empty;
            return gridView;
        }
        [WebMethod(EnableSession = true)]
        public static string PerformProductReceiveDelete(string detailsId)
        {
            int _ownerDetailId = Convert.ToInt32(detailsId);
            var ownerDetailBO = (List<PMProductReceivedBO>)HttpContext.Current.Session["PMProductReceive"];
            var ownerDetail = ownerDetailBO.Where(x => x.ReceivedId == _ownerDetailId).FirstOrDefault();
            ownerDetailBO.Remove(ownerDetail);
            HttpContext.Current.Session["PMProductReceive"] = ownerDetailBO;

            ArrayList list = new ArrayList();
            list = HttpContext.Current.Session["arrayDelete"] == null ? new ArrayList() : HttpContext.Current.Session["arrayDelete"] as ArrayList;
            list.Add(_ownerDetailId);
            HttpContext.Current.Session["arrayDelete"] = list as ArrayList;

            var dataSource = HttpContext.Current.Session["PMProductReceive"] as List<PMProductReceivedBO>;
            return GetProductReceiveDetailsGridView(dataSource);
        }
        [WebMethod(EnableSession = true)]
        public static bool ValidateSerialNumber(string POrderId, string serialNumber)
        {
            bool isValidSerial = true;
            PMProductSerialInfoDA entityDA = new PMProductSerialInfoDA();
            PMProductSerialInfoBO entityBO = new PMProductSerialInfoBO();
            entityBO = entityDA.GetPMProductSerialInfoBySerialNumber(serialNumber);

            if (entityBO.ProductId != 0)
            {
                isValidSerial = false;
            }

            return isValidSerial;
        }
        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemSearch(string searchTerm, int costCenterId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemNameWiseItemDetailsForAutoSearch(searchTerm, costCenterId, ConstantHelper.CustomerSupplierAutoSearch.SupplierItem.ToString());

            return itemInfo;
        }
        [WebMethod]
        public static List<InvLocationBO> InvLocationByCostCenter(int costCenterId)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvItemLocationByCostCenter(costCenterId);

            return location;
        }
        [WebMethod]
        public static ReturnInfo SaveReceivedOrder(int receivedId, int pOrderId, string referenceNo, string purchaseBy, List<PMProductReceivedDetailsBO> AddedReceivedDetails, List<PMProductReceivedDetailsBO> EditedReceivedDetails, List<PMProductReceivedDetailsBO> DeletedReceivedDetails, string poSupplierId, string poCreditAmount)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            int tmpReceiveId = 0;

            try
            {
                int supplierId = !string.IsNullOrWhiteSpace(poSupplierId) ? Convert.ToInt32(poSupplierId) : 0;
                PMSupplierDA supplierDA = new PMSupplierDA();
                PMSupplierBO supplierBO = new PMSupplierBO();
                HMCommonDA hmCommonDA = new HMCommonDA();
                NodeMatrixDA entityDA = new NodeMatrixDA();
                CustomFieldBO frontOfficeRoomSalesAccountHeadInfoBO = new CustomFieldBO();
                frontOfficeRoomSalesAccountHeadInfoBO = hmCommonDA.GetCustomFieldByFieldName("FrontOfficeRoomSalesAccountHeadInfo");
                int frontOfficeRoomSalesAccountHeadInfo = !string.IsNullOrWhiteSpace(frontOfficeRoomSalesAccountHeadInfoBO.FieldValue) ? Convert.ToInt32(frontOfficeRoomSalesAccountHeadInfoBO.FieldValue) : 0;

                HMUtility hmUtility = new HMUtility();
                List<PMProductReceivedBillPaymentBO> PRPaymentDetailListForGrid = HttpContext.Current.Session["PRPaymentDetailListForGrid"] == null ? new List<PMProductReceivedBillPaymentBO>() : HttpContext.Current.Session["PRPaymentDetailListForGrid"] as List<PMProductReceivedBillPaymentBO>;
                PMProductReceivedBillPaymentBO PRBillPaymentBO = new PMProductReceivedBillPaymentBO();

                //PRBillPaymentBO.AccountsPostingHeadId = supplierId;
                supplierBO = supplierDA.GetPMSupplierInfoById(supplierId);
                PRBillPaymentBO.AccountsPostingHeadId = supplierBO.NodeId;
                PRBillPaymentBO.PaymentType = "Advance";
                PRBillPaymentBO.BankId = 0;
                PRBillPaymentBO.FieldId = 1;
                PRBillPaymentBO.ConvertionRate = 1;
                PRBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(poCreditAmount) ? Convert.ToDecimal(poCreditAmount) : 0;
                PRBillPaymentBO.PaymentAmount = !string.IsNullOrWhiteSpace(poCreditAmount) ? Convert.ToDecimal(poCreditAmount) : 0;
                PRBillPaymentBO.ChecqueDate = DateTime.Now;
                PRBillPaymentBO.PaymentMode = "Supplier";
                PRBillPaymentBO.CardNumber = string.Empty;
                PRBillPaymentBO.CardType = string.Empty;
                PRBillPaymentBO.ExpireDate = null;
                PRBillPaymentBO.ChecqueNumber = string.Empty;
                PRBillPaymentBO.CardHolderName = string.Empty;
                PRBillPaymentBO.PaymentDescription = string.Empty;
                PRBillPaymentBO.PaymentId = 0;
                PRPaymentDetailListForGrid.Add(PRBillPaymentBO);
                HttpContext.Current.Session["PRPaymentDetailListForGrid"] = PRPaymentDetailListForGrid;

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsReceivedProductApprovalEnable", "IsReceivedProductApprovalEnable");

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                PMProductReceivedBO receivedProduct = new PMProductReceivedBO();
                PMProductReceivedDA receiveDA = new PMProductReceivedDA();
                List<PMProductReceivedDetailsBO> serialzableProduct = new List<PMProductReceivedDetailsBO>();

                serialzableProduct = AddedReceivedDetails.Where(s => s.SerialNumber != string.Empty && s.ReceiveDetailsId == 0).ToList();
                receivedProduct.POrderId = pOrderId;
                receivedProduct.SupplierId = !string.IsNullOrWhiteSpace(poSupplierId) ? Convert.ToInt32(poSupplierId) : 0;
                receivedProduct.PurchaseBy = purchaseBy;
                receivedProduct.ReferenceNumber = referenceNo;

                if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.ProductReceiveTemplate.ApprovalEnable))
                {
                    receivedProduct.Status = HMConstants.ApprovalStatus.Pending.ToString();
                }
                else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.ProductReceiveTemplate.ApprovalDisable))
                {
                    receivedProduct.Status = HMConstants.ApprovalStatus.Approved.ToString();
                }

                HMCommonSetupBO isSupplierCreditPaymentAutoPostingWhenProductReceiveBO = new HMCommonSetupBO();
                isSupplierCreditPaymentAutoPostingWhenProductReceiveBO = commonSetupDA.GetCommonConfigurationInfo("IsSupplierCreditPaymentAutoPostingWhenProductReceive", "IsSupplierCreditPaymentAutoPostingWhenProductReceive");
                if (isSupplierCreditPaymentAutoPostingWhenProductReceiveBO != null)
                {
                    if (isSupplierCreditPaymentAutoPostingWhenProductReceiveBO.SetupValue == "1")
                    {
                        List<PMProductReceivedBillPaymentBO> detailList = new List<PMProductReceivedBillPaymentBO>();
                        PMProductReceivedDA productReceivedDA = new PMProductReceivedDA();
                        detailList = productReceivedDA.GetProductreceiveBillPaymentInfo(receivedId);
                        if (detailList != null)
                        {
                            if (detailList.Count > 0)
                            {
                                HttpContext.Current.Session["PRPaymentDeleteList"] = detailList;
                            }
                        }

                        //PMSupplierDA supplierDA = new PMSupplierDA();
                        //PMSupplierBO supplierBO = new PMSupplierBO();
                        if (supplierId > 0)
                        {
                            supplierBO = supplierDA.GetPMSupplierInfoById(supplierId);
                            PRBillPaymentBO.AccountsPostingHeadId = supplierBO.NodeId;
                            PRBillPaymentBO.PaymentType = "Advance";
                            PRBillPaymentBO.BankId = 0;
                            PRBillPaymentBO.FieldId = 1;
                            PRBillPaymentBO.ConvertionRate = 1;
                            PRBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(poCreditAmount) ? Convert.ToDecimal(poCreditAmount) : 0;
                            PRBillPaymentBO.PaymentAmount = !string.IsNullOrWhiteSpace(poCreditAmount) ? Convert.ToDecimal(poCreditAmount) : 0;
                            PRBillPaymentBO.ChecqueDate = DateTime.Now;
                            PRBillPaymentBO.PaymentMode = "Supplier";
                            PRBillPaymentBO.CardNumber = string.Empty;
                            PRBillPaymentBO.CardType = null;
                            PRBillPaymentBO.ExpireDate = null;
                            PRBillPaymentBO.ChecqueNumber = string.Empty;
                            PRBillPaymentBO.CardHolderName = string.Empty;
                            PRBillPaymentBO.PaymentDescription = supplierBO.Name;
                            PRBillPaymentBO.CreatedBy = userInformationBO.UserInfoId;
                            PRBillPaymentBO.PaymentId = 0;

                            PRPaymentDetailListForGrid.Add(PRBillPaymentBO);
                            HttpContext.Current.Session["PRPaymentDetailListForGrid"] = PRPaymentDetailListForGrid;
                        }
                    }

                }

                List<PMProductReceivedBillPaymentBO> addList = new List<PMProductReceivedBillPaymentBO>();
                List<PMProductReceivedBillPaymentBO> deleteList = HttpContext.Current.Session["PRPaymentDeleteList"] as List<PMProductReceivedBillPaymentBO>;
                List<PMProductReceivedBillPaymentBO> paymentList = HttpContext.Current.Session["PRPaymentDetailListForGrid"] as List<PMProductReceivedBillPaymentBO>;

                if (paymentList != null)
                {
                    foreach (PMProductReceivedBillPaymentBO bo in paymentList)
                    {
                        if (bo.PaymentId == 0)
                        {
                            addList.Add(bo);
                        }
                    }
                }

                if (receivedId == 0)
                {
                    receivedProduct.ReceivedDate = DateTime.Now;
                    receivedProduct.CreatedBy = userInformationBO.UserInfoId;

                    //status = receiveDA.SaveProductReceiveInfo(receivedProduct, AddedReceivedDetails, serialzableProduct, addList, out tmpReceiveId);

                    if (status && Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.ProductReceiveTemplate.ApprovalDisable))
                    {
                        receivedProduct.ReceivedId = tmpReceiveId;
                        receivedProduct.LastModifiedBy = userInformationBO.UserInfoId;
                        receivedProduct.Status = "Approved";
                        receiveDA.UpdateProductReceiveStatusNItemStockNAverageCost(receivedProduct);

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.ProductReceive.ToString(), tmpReceiveId,
                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductReceive));
                    }

                    if (status)
                    {
                        HttpContext.Current.Session["PRPaymentDeleteList"] = null;
                        HttpContext.Current.Session["PRPaymentDetailListForGrid"] = null;
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }
                else
                {
                    receivedProduct.ReceivedId = receivedId;
                    receivedProduct.LastModifiedBy = userInformationBO.UserInfoId;

                    //status = receiveDA.UpdateProductReceiveInfo(receivedProduct, AddedReceivedDetails, EditedReceivedDetails, DeletedReceivedDetails, serialzableProduct, addList, deleteList);

                    if (status)
                    {
                        HttpContext.Current.Session["PRPaymentDeleteList"] = null;
                        HttpContext.Current.Session["PRPaymentDetailListForGrid"] = null;
                        HttpContext.Current.Session.Remove("ProductReceivedId");

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.ProductReceive.ToString(), tmpReceiveId,
                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductReceive));

                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }

                if (!status)
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }
        [WebMethod]
        public static List<PMProductReceivedDetailsBO> ReceivedDetails(int receivedId)
        {
            PMProductReceivedDA receivedDa = new PMProductReceivedDA();
            return receivedDa.GetProductreceiveDetailsInfo(receivedId);
        }
        [WebMethod]
        public static ReceivedViewBO FillForm(int receivedId)
        {
            PMProductReceivedDA receiveDA = new PMProductReceivedDA();
            ReceivedViewBO viewBo = new ReceivedViewBO();

            List<PMProductReceivedBillPaymentBO> detailList = new List<PMProductReceivedBillPaymentBO>();
            PMProductReceivedDA productReceivedDA = new PMProductReceivedDA();
            detailList = productReceivedDA.GetProductreceiveBillPaymentInfo(receivedId);
            HttpContext.Current.Session["PRPaymentDetailListForGrid"] = detailList;

            viewBo.Received = receiveDA.GetProductreceiveInfo(receivedId);
            viewBo.ReceivedDetails = receiveDA.GetProductreceiveDetailsInfo(receivedId);

            return viewBo;
        }
        [WebMethod]
        public static ReceivedViewBO FillFormForTemplate2(int receivedId)
        {
            PMProductReceivedDA receiveDA = new PMProductReceivedDA();
            ReceivedViewBO viewBo = new ReceivedViewBO();

            List<PMProductReceivedBillPaymentBO> detailList = new List<PMProductReceivedBillPaymentBO>();
            PMProductReceivedDA productReceivedDA = new PMProductReceivedDA();
            detailList = productReceivedDA.GetProductreceiveBillPaymentInfo(receivedId);
            HttpContext.Current.Session["PRPaymentDetailListForGrid"] = detailList;

            viewBo.Received = receiveDA.GetProductreceiveInfo(receivedId);
            viewBo.ReceivedDetails = receiveDA.GetProductReceiveDetailsById(receivedId);

            viewBo.CostCenterId = viewBo.ReceivedDetails[0].CostCenterId;
            viewBo.POrderId = viewBo.Received.POrderId;
            viewBo.SupplierId = viewBo.ReceivedDetails[0].SupplierId;
            viewBo.SupplierName = viewBo.ReceivedDetails[0].SupplierName;

            string grid = string.Empty, tr = string.Empty;
            int rowCount = 0, isEdited = 0;

            foreach (PMProductReceivedDetailsBO pod in viewBo.ReceivedDetails)
            {
                if (rowCount % 2 == 0)
                {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else
                {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:20%;'>" + pod.ItemName + "</td>";
                tr += "<td style='width:10%;'>" + pod.StockBy + "</td>";
                tr += "<td style='width:10%;'>" + pod.StockQuantity + "</td>";
                tr += "<td style='width:10%;'>" + pod.PurchasePrice + "</td>";
                tr += "<td style='width:10%;'>" + pod.OrderedQuantity + "</td>";
                tr += "<td style='width:14%;'>" + pod.QuantityReceived + "</td>";
                tr += "<td style='width:14%;'>" + pod.RemainingQuantity + "</td>";

                tr += "<td style='width:12%; text-align:center;'> <input type='text' id='txt" + pod.ProductId.ToString() + "' value = '' onblur='CheckInputValue(this)' style='width:65px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";

                tr += "<td style='display:none'>" + pod.ReceiveDetailsId + "</td>";
                tr += "<td style='display:none'>" + pod.ProductId + "</td>";
                tr += "<td style='display:none'>" + pod.StockById + "</td>";
                tr += "<td style='display:none'>" + pod.LocationId + "</td>";
                tr += "<td style='display:none'>" + pod.Quantity + "</td>";
                tr += "<td style='display:none'>" + isEdited + "</td>";

                tr += "</tr>";

                rowCount++;
            }

            grid += GridHeader() + tr + "</tbody> </table>";
            viewBo.ReceivedProductGrid = grid;

            viewBo.ReceivedDetails = null;

            return viewBo;
        }
        [WebMethod]
        public static ArrayList LoadItemForReceivedByPurchaseOrderId(int orderId, int costCenterId)
        {
            PMPurchaseOrderDA pOrderDa = new PMPurchaseOrderDA();
            List<PMPurchaseOrderDetailsBO> orderDetails = new List<PMPurchaseOrderDetailsBO>();
            PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();

            orderDetails = pOrderDa.GetPurchaseOrderDetailByOrderNCostCenterId(orderId, costCenterId);
            orderBO = pOrderDa.GetPMPurchaseOrderInfoByOrderId(orderId);

            ArrayList arr = new ArrayList();

            string grid = string.Empty, tr = string.Empty;
            int rowCount = 0, receiveDetailId = 0, isEdited = 0;
            decimal grandTotal = 0;

            foreach (PMPurchaseOrderDetailsBO pod in orderDetails)
            {
                if (rowCount % 2 == 0)
                {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else
                {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:20%;'>" + pod.ProductName + "</td>";
                tr += "<td style='width:10%;'>" + pod.StockBy + "</td>";
                tr += "<td style='width:10%;'>" + pod.StockQuantity + "</td>";
                tr += "<td style='width:10%;'>" + pod.PurchasePrice + "</td>";
                tr += "<td style='width:10%;'>" + pod.Quantity + "</td>";
                tr += "<td style='width:14%;'>" + pod.QuantityReceived + "</td>";
                tr += "<td style='width:14%;'>" + pod.RemainingQuantity + "</td>";

                tr += "<td style='width:12%; text-align:center;'> <input type='text' id='txt" + pod.ProductId.ToString() + "' value = '' onblur='CheckInputValue(this)' onkeydown='if (event.keyCode == 13) {return false;}' class='form-control' style='width:65px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";

                tr += "<td style='display:none'>" + receiveDetailId + "</td>";
                tr += "<td style='display:none'>" + pod.ProductId + "</td>";
                tr += "<td style='display:none'>" + pod.StockById + "</td>";
                tr += "<td style='display:none'>" + pod.DefaultStockLocationId + "</td>";
                tr += "<td style='display:none'>0</td>";
                tr += "<td style='display:none'>" + isEdited + "</td>";

                tr += "</tr>";

                grandTotal += (pod.Quantity * pod.PurchasePrice);
                rowCount++;
            }

            grid += GridHeader() + tr + "</tbody> " + GridFooter(grandTotal) + " </table>";
            arr.Add(grid);
            arr.Add(orderBO.SupplierName);
            arr.Add(orderBO.SupplierId);

            return arr;
        }
        [WebMethod]
        public static List<PMPurchaseOrderBO> GetPurchaseOrderByCostcenter(int costCenterId)
        {
            PMPurchaseOrderDA orderDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();
            orderList = orderDA.GetApprovedPMPurchaseOrderInfo("Product", costCenterId);

            return orderList;
        }
        [WebMethod]
        public static CommonCurrencyBO LoadCurrencyType(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetCommonCurrencyInfoById(currecyType);
            return commonCurrencyBO;
        }
        [WebMethod]
        public static CommonCurrencyConversionBO LoadCurrencyConversionRate(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyConversionBO conversionBO = new CommonCurrencyConversionBO();
            conversionBO = commonCurrencyDA.GetCurrencyConversionRate(currecyType);
            return conversionBO;
        }
        [WebMethod(EnableSession = true)]
        public static string PerformSaveGuestPaymentDetailsInformationByWebMethod(bool isEdit, string paymentDescription, string ddlCurrency, string currencyType, string localCurrencyId, string conversionRate, string ddlPayMode, string ddlBankId, string txtReceiveLeadgerAmount, string ddlCashPaymentAccountHead, string txtCardNumber, string ddlCardType, string txtExpireDate, string txtCardHolderName, string txtChecqueNumber, string ddlChecquePaymentAccountHeadId, string ddlCardPaymentAccountHeadId, string supplierNodeId)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            CustomFieldBO frontOfficeRoomSalesAccountHeadInfoBO = new CustomFieldBO();
            frontOfficeRoomSalesAccountHeadInfoBO = hmCommonDA.GetCustomFieldByFieldName("FrontOfficeRoomSalesAccountHeadInfo");
            int frontOfficeRoomSalesAccountHeadInfo = !string.IsNullOrWhiteSpace(frontOfficeRoomSalesAccountHeadInfoBO.FieldValue) ? Convert.ToInt32(frontOfficeRoomSalesAccountHeadInfoBO.FieldValue) : 0;

            HMUtility hmUtility = new HMUtility();
            List<PMProductReceivedBillPaymentBO> PRPaymentDetailListForGrid = HttpContext.Current.Session["PRPaymentDetailListForGrid"] == null ? new List<PMProductReceivedBillPaymentBO>() : HttpContext.Current.Session["PRPaymentDetailListForGrid"] as List<PMProductReceivedBillPaymentBO>;
            PMProductReceivedBillPaymentBO PRBillPaymentBO = new PMProductReceivedBillPaymentBO();

            if (ddlPayMode == "Cash")
            {
                PRBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPaymentAccountHead);
            }
            else if (ddlPayMode == "Card")
            {
                PRBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCardPaymentAccountHeadId);
            }
            else if (ddlPayMode == "Cheque")
            {
                PRBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlChecquePaymentAccountHeadId);
            }
            else if (ddlPayMode == "Supplier")
            {
                PRBillPaymentBO.AccountsPostingHeadId = !string.IsNullOrEmpty(supplierNodeId) ? Convert.ToInt32(supplierNodeId) : 0;
            }
            PRBillPaymentBO.PaymentType = "Advance";

            PRBillPaymentBO.BankId = Convert.ToInt32(ddlBankId);

            if (currencyType == "Local")
            {
                PRBillPaymentBO.FieldId = Convert.ToInt32(localCurrencyId);
                PRBillPaymentBO.ConvertionRate = 1;
                PRBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
                PRBillPaymentBO.PaymentAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            }
            else
            {
                PRBillPaymentBO.FieldId = Convert.ToInt32(ddlCurrency);
                PRBillPaymentBO.ConvertionRate = !string.IsNullOrWhiteSpace(conversionRate) ? Convert.ToDecimal(conversionRate) : 1;
                PRBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
                PRBillPaymentBO.PaymentAmount = PRBillPaymentBO.CurrencyAmount * PRBillPaymentBO.ConvertionRate;
            }

            PRBillPaymentBO.ChecqueDate = DateTime.Now;
            PRBillPaymentBO.PaymentMode = ddlPayMode;
            PRBillPaymentBO.CardNumber = txtCardNumber;
            PRBillPaymentBO.CardType = ddlCardType;
            if (string.IsNullOrEmpty(txtExpireDate))
            {
                PRBillPaymentBO.ExpireDate = null;
            }
            else
            {
                PRBillPaymentBO.ExpireDate = hmUtility.GetDateTimeFromString(txtExpireDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            PRBillPaymentBO.ChecqueNumber = txtChecqueNumber;
            PRBillPaymentBO.CardHolderName = txtCardHolderName;

            PRBillPaymentBO.PaymentDescription = paymentDescription;

            PRBillPaymentBO.PaymentId = 0;

            PRPaymentDetailListForGrid.Add(PRBillPaymentBO);
            HttpContext.Current.Session["PRPaymentDetailListForGrid"] = PRPaymentDetailListForGrid;

            return LoadGuestPaymentDetailGridViewByWM(paymentDescription, 0);
        }
        [WebMethod(EnableSession = true)]
        public static string PerformDeleteGuestPaymentByWebMethod(int paymentId)
        {
            List<PMProductReceivedBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["PRPaymentDetailListForGrid"] == null ? new List<PMProductReceivedBillPaymentBO>() : HttpContext.Current.Session["PRPaymentDetailListForGrid"] as List<PMProductReceivedBillPaymentBO>;
            PMProductReceivedBillPaymentBO singleEntityBOEdit = guestPaymentDetailListForGrid.Where(x => x.PaymentId == paymentId).FirstOrDefault();
            if (guestPaymentDetailListForGrid.Contains(singleEntityBOEdit))
            {
                guestPaymentDetailListForGrid.Remove(singleEntityBOEdit);
            }
            List<PMProductReceivedBillPaymentBO> deleteList = HttpContext.Current.Session["PRPaymentDeleteList"] == null ? new List<PMProductReceivedBillPaymentBO>() : HttpContext.Current.Session["PRPaymentDeleteList"] as List<PMProductReceivedBillPaymentBO>;
            if (singleEntityBOEdit.PaymentId > 0)
            {
                deleteList.Add(singleEntityBOEdit);
            }
            HttpContext.Current.Session["PRPaymentDeleteList"] = deleteList;
            HttpContext.Current.Session["PRPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;

            return LoadGuestPaymentDetailGridViewByWM("", 0);
        }
        [WebMethod]
        public static string FillPaymentInfo(int receivedId)
        {
            return LoadGuestPaymentDetailGridViewByWM("", receivedId);
        }
        [WebMethod]
        public static PMSupplierBO GetSupplierInfoById(int supplierId)
        {
            PMSupplierDA supplierDA = new PMSupplierDA();
            PMSupplierBO supplierBO = new PMSupplierBO();

            supplierBO = supplierDA.GetPMSupplierInfoById(supplierId);

            return supplierBO;
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
        public static List<InvUnitHeadBO> LoadStockByForAdhoc()
        {
            InvUnitHeadDA unitHeadDA = new InvUnitHeadDA();
            List<InvUnitHeadBO> unitHeadList = new List<InvUnitHeadBO>();

            unitHeadList = unitHeadDA.GetInvUnitHeadInfo();

            return unitHeadList;
        }
        [WebMethod(EnableSession = true)]
        public static string PerformGetTotalPaidAmountByWebMethod()
        {
            var List = HttpContext.Current.Session["PRPaymentDetailListForGrid"] as List<PMProductReceivedBillPaymentBO>;
            decimal sum = 0;
            if (List != null)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    if (List[i].PaymentMode == "Refund")
                    {
                        sum = sum - Convert.ToDecimal(List[i].PaymentAmount);
                    }
                    else
                    {
                        sum = sum + Convert.ToDecimal(List[i].PaymentAmount);
                    }
                }
            }
            //return Math.Round(sum).ToString();
            return sum.ToString();
        }
    }
}