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
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmPMServicePO : System.Web.UI.Page
    {
        int UserId = 1;
        ArrayList arrayDelete;
        protected int _OrderId;
        protected int isMessageBoxEnable = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            UserId = userInformationBO.UserInfoId;

            this.AddEditODeleteDetail();
            if (!IsPostBack)
            {
                this.LoadCommonDropDownHiddenField();
                this.LoadCurrency();
                Session["PMPurchaseOrderDetails"] = null;
                string pOrderId = Request.QueryString["POrderId"];

                if (!string.IsNullOrEmpty(pOrderId))
                {
                    this.FillForm(Int32.Parse(pOrderId));
                }

                this.LoadSupplierInfo();
                //         this.LoadProductInfo();
                this.LoadCategory();
                this.LoadProductInfoAll();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string fromDate = string.Empty;
            string toDate = string.Empty;
            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                fromDate = hmUtility.GetFromDate();
            }
            else
            {
                fromDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                toDate = hmUtility.GetToDate();
            }
            else
            {
                toDate = this.txtToDate.Text;
            }

            string PONumber = this.txtSPONumber.Text;
            PMPurchaseOrderDA detalisDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();
            //orderList = detalisDA.GetPMPurchaseOrderInfoBySearchCriteria("Service", hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), PONumber, this.ddlStatus.SelectedItem.Text);
            gvOrderInfo.DataSource = orderList;
            gvOrderInfo.DataBind();

            this.SetTab("SearchTab");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsOrderFormValid())
            {
                return;
            }
            PMPurchaseOrderDA orderDetailsDA = new PMPurchaseOrderDA();
            PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            orderBO.POType = "Service";
            orderBO.ReceivedByDate = hmUtility.GetDateTimeFromString(txtReceivedByDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            orderBO.Remarks = txtRemarks.Text;
            orderBO.SupplierId = Convert.ToInt32(this.ddlSupplier.SelectedValue.ToString());
            if (this.btnSave.Text.Equals("Save"))
            {
                int tmpOrderId = 0;
                orderBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = true; //orderDetailsDA.SavePMPurchaseOrderInfo(orderBO, out tmpOrderId, Session["PMPurchaseOrderDetails"] as List<PMPurchaseOrderDetailsBO>);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Saved Operation Successfull";
                    this.Cancel();
                }
            }
            else
            {
                orderBO.POrderId = Convert.ToInt32(Session["_OrderId"]);
                orderBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = true;//orderDetailsDA.UpdatePurchaseOrderInfo(orderBO, Session["PMPurchaseOrderDetails"] as List<PMPurchaseOrderDetailsBO>, Session["arrayDelete"] as ArrayList);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Update Operation Successfull";
                    this.Cancel();
                }
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!IsDetailsFormValid())
            {
                return;
            }
            int dynamicDetailId = 0;
            List<PMPurchaseOrderDetailsBO> detailListBO = Session["PMPurchaseOrderDetails"] == null ? new List<PMPurchaseOrderDetailsBO>() : Session["PMPurchaseOrderDetails"] as List<PMPurchaseOrderDetailsBO>;

            if (!string.IsNullOrWhiteSpace(lblHiddenOrderDetailtId.Text))
                dynamicDetailId = Convert.ToInt32(lblHiddenOrderDetailtId.Text);

            PMPurchaseOrderDetailsBO detailBO = dynamicDetailId == 0 ? new PMPurchaseOrderDetailsBO() : detailListBO.Where(x => x.DetailId == dynamicDetailId).FirstOrDefault();
            if (detailListBO.Contains(detailBO))
                detailListBO.Remove(detailBO);

            SalesServiceDA productDA = new SalesServiceDA();
            SalesServiceBO productBO = new SalesServiceBO();
            PMPurchaseOrderDA detailsDA = new PMPurchaseOrderDA();
            PMPurchaseOrderDetailsBO Details = new PMPurchaseOrderDetailsBO();
            productBO = productDA.GetSalesServiceInfoByServiceId(Convert.ToInt32(this.ddlProductId.SelectedValue));
            detailBO.ProductName = productBO.Name;
            detailBO.CategoryName = productBO.CategoryName;
            detailBO.PurchasePrice = Convert.ToDecimal(txtPurchasePrice.Text);
            detailBO.ProductId = Convert.ToInt32(this.ddlProductId.SelectedValue);

            try
            {
                detailBO.Quantity = Convert.ToDecimal(this.txtQuantity.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            detailBO.DetailId = dynamicDetailId == 0 ? detailListBO.Count + 1 : dynamicDetailId;
            detailListBO.Add(detailBO);
            Session["PMPurchaseOrderDetails"] = detailListBO;
            //          this.gvOrderDetails.DataSource = Session["PMPurchaseOrderDetails"] as List<PMPurchaseOrderDetailsBO>;
            //         this.gvOrderDetails.DataBind();
            //          this.CalculateAmountTotal();
            this.ClearDetailPart();
        }
        protected void gvOrderInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            this._OrderId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                Session["_OrderId"] = this._OrderId;
                this.FillForm(this._OrderId);
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                try
                {
                    Session["_OrderId"] = this._OrderId;
                    this.DeleteData(this._OrderId);
                    this.Cancel();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (e.CommandName == "CmdOrderPreview")
            {
                string Fullurl = "/PurchaseManagment/Reports/frmReportPMPurchaseOrder.aspx?POrderId=" + _OrderId;
                OpenNewBrowserWindow(Fullurl, this);
                this._OrderId = -1;
            }
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
        protected void gvOrderInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvOrderInfo.PageIndex = e.NewPageIndex;
        }
        protected void gvOrderInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                Label lblApprovedStatusValue = (Label)e.Row.FindControl("lblApprovedStatus");

                if (lblApprovedStatusValue.Text != "Approved")
                {
                    imgUpdate.Visible = true;
                    imgDelete.Visible = true;
                }
                else
                {
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                }
            }
        }
        //protected void gvOrderDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        //{

        //    int _ownerDetailId;
        //    if (e.CommandName == "CmdEdit")
        //    {
        //        _ownerDetailId = Convert.ToInt32(e.CommandArgument.ToString());
        //        lblHiddenOrderDetailtId.Text = _ownerDetailId.ToString();
        //        var ownerDetailBO = (List<PMPurchaseOrderDetailsBO>)Session["PMPurchaseOrderDetails"];
        //        var ownerDetail = ownerDetailBO.Where(x => x.DetailId == _ownerDetailId).FirstOrDefault();
        //        if (ownerDetail != null && ownerDetail.DetailId > 0)
        //        {
        //            this.ddlProductId.SelectedValue = ownerDetail.ProductId.ToString();
        //            this.txtPurchasePrice.Text = ownerDetail.PurchasePrice.ToString();
        //            this.txtQuantity.Text = ownerDetail.Quantity.ToString();
        //         //   btnAdd.Text = "Edit";
        //        }
        //        else
        //        {
        //         //   btnAdd.Text = "Add";
        //        }
        //    }
        //    else if (e.CommandName == "CmdDelete")
        //    {

        //        _ownerDetailId = Convert.ToInt32(e.CommandArgument.ToString());
        //        var ownerDetailBO = (List<PMPurchaseOrderDetailsBO>)Session["PMPurchaseOrderDetails"];
        //        var ownerDetail = ownerDetailBO.Where(x => x.DetailId == _ownerDetailId).FirstOrDefault();
        //        ownerDetailBO.Remove(ownerDetail);
        //        Session["PMPurchaseOrderDetails"] = ownerDetailBO;
        //        arrayDelete.Add(_ownerDetailId);
        //        this.gvOrderDetails.DataSource = Session["PMPurchaseOrderDetails"] as List<PMPurchaseOrderDetailsBO>;
        //        this.gvOrderDetails.DataBind();
        //        this.CalculateAmountTotal();
        //    }
        //}
        protected void gvOrderDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            // this.gvOrderDetails.PageIndex = e.NewPageIndex;
        }
        //************************ User Defined Function ********************//
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
            this.ddlPurchasePriceLocal.DataSource = fields;
            this.ddlPurchasePriceLocal.DataTextField = "FieldValue";
            this.ddlPurchasePriceLocal.DataValueField = "FieldId";
            this.ddlPurchasePriceLocal.DataBind();
            this.ddlPurchasePriceLocal.SelectedIndex = 0;

            this.ShowHideCurrencyInformation();
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
            bool status = true;
            int Count = 0;
            List<PMPurchaseOrderDetailsBO> List = Session["PMPurchaseOrderDetails"] as List<PMPurchaseOrderDetailsBO>;
            if (List == null)
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Add Order Detail Information";
                this.ddlProductId.Focus();
                status = false;
            }
            else
            {
                Count = List.Count;
            }

            if (string.IsNullOrEmpty(txtReceivedByDate.Text))
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Enter Received By Date";
                this.txtReceivedByDate.Focus();
                status = false;
            }
            else if (this.ddlSupplier.SelectedIndex == 0)
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Select Supplier Name";
                this.ddlSupplier.Focus();
                status = false;
            }
            else if (Count == 0)
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Add Order Detail Information";
                this.ddlProductId.Focus();
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
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Select Product";
                this.ddlProductId.Focus();
                status = false;
            }
            else if (string.IsNullOrEmpty(txtQuantity.Text))
            {
                this.isMessageBoxEnable = 1;
                txtQuantity.Text = "";
                lblMessage.Text = "Please Enter Valid Quantity";
                this.txtQuantity.Focus();
                status = false;
            }
            else if (!Decimal.TryParse(txtQuantity.Text, out number))
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Enter Valid Quantity";
                this.txtQuantity.Focus();
                status = false;
            }
            else if (string.IsNullOrEmpty(txtPurchasePrice.Text))
            {
                this.isMessageBoxEnable = 1;
                txtPurchasePrice.Text = "";
                lblMessage.Text = "Please Enter Valid Purchase Price";
                this.txtPurchasePrice.Focus();
                status = false;
            }
            else if (!Decimal.TryParse(txtPurchasePrice.Text, out number))
            {
                this.isMessageBoxEnable = 1;
                txtPurchasePrice.Text = "";
                lblMessage.Text = "Please Enter Valid Purchase Price";
                this.txtPurchasePrice.Focus();
                status = false;
            }

            return status;
        }
        private void ClearDetailPart()
        {
            //   this.btnAdd.Text = "Add";
            this.ddlProductId.SelectedValue = "0";
            this.txtQuantity.Text = string.Empty;
            this.txtPurchasePrice.Text = string.Empty;
            this.lblHiddenOrderDetailtId.Text = string.Empty;
        }
        private void Cancel()
        {
            this.txtSPONumber.Text = string.Empty;
            this.txtRemarks.Text = string.Empty;
            this.txtToDate.Text = string.Empty;
            this.txtReceivedByDate.Text = string.Empty;
            this.txtQuantity.Text = string.Empty;
            this.txtFromDate.Text = string.Empty;
            this.ddlProductId.SelectedIndex = 0;
            this.ddlSupplier.SelectedIndex = 0;
            this.txtPOrderId.Value = "";
            this.btnSave.Text = "Save";
            Session["PMPurchaseOrderDetails"] = null;
            //  this.gvOrderDetails.DataSource = Session["PMPurchaseOrderDetails"] as List<PMPurchaseOrderDetailsBO>;
            //  this.gvOrderDetails.DataBind();
            this.ClearDetailPart();
            this.lblTotalCalculateAmount.Text = "0";
        }
        private void LoadSupplierInfo()
        {
            PMSupplierDA entityDA = new PMSupplierDA();
            this.ddlSupplier.DataSource = entityDA.GetPMSupplierInfo();
            this.ddlSupplier.DataTextField = "Name";
            this.ddlSupplier.DataValueField = "SupplierId";
            this.ddlSupplier.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlSupplier.Items.Insert(0, item);
        }
        private void LoadProductInfo()
        {
            SalesServiceDA serviceDA = new SalesServiceDA();
            this.ddlProductId.DataSource = serviceDA.GetSaleServicInfo();
            this.ddlProductId.DataTextField = "Name";
            this.ddlProductId.DataValueField = "serviceId";
            this.ddlProductId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlProductId.Items.Insert(0, item);
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }

        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            SalesServiceDA serviceDA = new SalesServiceDA();

            List = da.GetAllInvItemCatagoryInfoByServiceType("Service");
            this.ddlCategory.DataSource = List;
            this.ddlCategory.DataTextField = "Name";
            this.ddlCategory.DataValueField = "CategoryId";
            this.ddlCategory.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---All---";
            this.ddlCategory.Items.Insert(0, item);
        }
        private void LoadProductInfoAll()
        {

            List<ItemViewBO> viewList = new List<ItemViewBO>();
            InvItemDA productDA = new InvItemDA();
            SalesServiceDA serviceDA = new SalesServiceDA();
            var productList = serviceDA.GetSaleServicInfo();
            for (int i = 0; i < productList.Count; i++)
            {
                ItemViewBO viewBO = new ItemViewBO();
                viewBO.ItemId = productList[i].ServiceId;
                viewBO.ItemName = productList[i].Name;
                viewList.Add(viewBO);
            }

            this.ddlProductId.DataSource = viewList;
            this.ddlProductId.DataTextField = "ItemName";
            this.ddlProductId.DataValueField = "ItemId";

            this.ddlProductId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlProductId.Items.Insert(0, item);
        }
        private void DeleteData(int pkId)
        {
            PMPurchaseOrderDA orderDetailsDA = new PMPurchaseOrderDA();
            Boolean statusApproved = orderDetailsDA.DeleteOrderDetailInfoByOrderId(pkId);
            if (statusApproved)
            {
                this.isMessageBoxEnable = 2;
                lblMessage.Text = "Delete Operation Successfull";
                this.Cancel();
            }
        }
        private void FillForm(int orderId)
        {
            lblMessage.Text = "";

            //Master Information------------------------
            PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
            PMPurchaseOrderDA orderDetailDA = new PMPurchaseOrderDA();

            orderBO = orderDetailDA.GetPMPurchaseOrderInfoByOrderId(orderId);
            Session["_OrderId"] = orderBO.POrderId;
            txtPOrderId.Value = orderBO.POrderId.ToString();
            this.ddlSupplier.SelectedValue = orderBO.SupplierId.ToString();
            this.txtRemarks.Text = orderBO.Remarks;
            this.txtReceivedByDate.Text = Convert.ToDateTime(orderBO.ReceivedByDate).ToShortDateString();
            this.btnSave.Text = "Update";


            //this.btnNewReservation.Visible = false;
            //Detail Information------------------------
            List<PMPurchaseOrderDetailsBO> orderDetailListBO = new List<PMPurchaseOrderDetailsBO>();


            orderDetailListBO = orderDetailDA.GetPMPurchaseOrderDetailForServiceByOrderId(orderId);

            Session["PMPurchaseOrderDetails"] = orderDetailListBO;

            // this.gvOrderDetails.DataSource = Session["PMPurchaseOrderDetails"] as List<PMPurchaseOrderDetailsBO>;
            // this.gvOrderDetails.DataBind();
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
        //public void CalculateAmountTotal()
        //{
        //    decimal AmtTotal = 0, tmpQuantity, tmpPurchasePrice;

        //    for (int i = 0; i < gvOrderDetails.Rows.Count; i++)
        //    {
        //        tmpQuantity = 0;
        //        tmpPurchasePrice = 0;

        //        if ((decimal.TryParse(((Label)gvOrderDetails.Rows[i].FindControl("lblQuantity")).Text, out tmpQuantity)) &&
        //            (decimal.TryParse(((Label)gvOrderDetails.Rows[i].FindControl("lblPurchasePrice")).Text, out tmpPurchasePrice)))
        //            AmtTotal += tmpQuantity * tmpPurchasePrice;
        //    }

        //    this.lblTotalCalculateAmount.Text = new HMCommonDA().CurrencyMask(AmtTotal.ToString());
        //}
        public static void OpenNewBrowserWindow(string Url, Control control)
        {
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Open", "window.open('" + Url + "');", true);
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
                    this.lblPurchasePriceLocal.Text = "Unit Price(" + this.ddlPurchasePriceLocal.SelectedItem.Text + ")";
                }
                else
                {
                    this.lblPurchasePriceLocal.Text = "Unit Price(USD)";
                }
            }
        }
        [WebMethod]
        public static decimal GetPurchasePrice(int ProductId)
        {
            SalesServiceDA productDA = new SalesServiceDA();
            SalesServiceBO productBO = new SalesServiceBO();
            PMPurchaseOrderDA detailsDA = new PMPurchaseOrderDA();
            PMPurchaseOrderDetailsBO Details = new PMPurchaseOrderDetailsBO();
            productBO = productDA.GetSalesServiceInfoByServiceId(Convert.ToInt32(ProductId));

            return productBO.PurchasePrice;
        }
        [WebMethod]
        public static List<ItemViewBO> LoadProductListOnPONumberChange(string Category)
        {
            List<ItemViewBO> viewList = new List<ItemViewBO>();

            InvItemDA productDA = new InvItemDA();
            SalesServiceDA serviceDA = new SalesServiceDA();
            var productList = serviceDA.GetSaleServicInfoByCategoryId(Convert.ToInt32(Category));
            for (int i = 0; i < productList.Count; i++)
            {
                ItemViewBO viewBO = new ItemViewBO();
                viewBO.ItemId = productList[i].ServiceId;
                viewBO.ItemName = productList[i].Name;
                viewList.Add(viewBO);
            }


            return viewList;
        }

        [WebMethod(EnableSession = true)]
        public static string SavePMServiceOrderInformation(string lblHiddenOrderDetailtId, string ddlProductId, string txtPurchasePrice, string txtQuantity, string Category)
        {

            //if (!IsDetailsFormValid())
            //{
            //    return;
            //}
            int dynamicDetailId = 0;
            List<PMPurchaseOrderDetailsBO> detailListBO = HttpContext.Current.Session["PMPurchaseOrderDetails"] == null ? new List<PMPurchaseOrderDetailsBO>() : HttpContext.Current.Session["PMPurchaseOrderDetails"] as List<PMPurchaseOrderDetailsBO>;

            if (!string.IsNullOrWhiteSpace(lblHiddenOrderDetailtId))
                dynamicDetailId = Convert.ToInt32(lblHiddenOrderDetailtId);

            PMPurchaseOrderDetailsBO detailBO = dynamicDetailId == 0 ? new PMPurchaseOrderDetailsBO() : detailListBO.Where(x => x.DetailId == dynamicDetailId).FirstOrDefault();
            if (detailListBO.Contains(detailBO))
                detailListBO.Remove(detailBO);

            SalesServiceDA productDA = new SalesServiceDA();
            SalesServiceBO productBO = new SalesServiceBO();
            PMPurchaseOrderDA detailsDA = new PMPurchaseOrderDA();
            PMPurchaseOrderDetailsBO Details = new PMPurchaseOrderDetailsBO();
            productBO = productDA.GetSalesServiceInfoByServiceId(Convert.ToInt32(ddlProductId));
            detailBO.ProductName = productBO.Name;
            detailBO.CategoryName = productBO.CategoryName;
            detailBO.PurchasePrice = Convert.ToDecimal(txtPurchasePrice);
            detailBO.ProductId = Convert.ToInt32(ddlProductId);
            detailBO.Quantity = Convert.ToDecimal(txtQuantity);
            detailBO.CategoryId = Convert.ToInt32(Category);
            detailBO.DetailId = dynamicDetailId == 0 ? detailListBO.Count + 1 : dynamicDetailId;
            detailListBO.Add(detailBO);
            HttpContext.Current.Session["PMPurchaseOrderDetails"] = detailListBO;
            var DataSource = HttpContext.Current.Session["PMPurchaseOrderDetails"] as List<PMPurchaseOrderDetailsBO>;
            return GetProductOutDetailsGridView(DataSource);
        }

        public static string GetProductOutDetailsGridView(List<PMPurchaseOrderDetailsBO> dataSource)
        {


            string strTable = "";
            if (dataSource != null)
            {
                strTable += "<table style='width:100%' cellspacing='0' cellpadding='4' id='ProductDetailGrid'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Product Name</th><th align='left' scope='col'>Quantity</th><th align='left' scope='col'>Purchase Price</th><th align='left' scope='col'>Total Price</th><th align='center' scope='col'>Action</th></tr>";
                int counter = 0;
                foreach (PMPurchaseOrderDetailsBO dr in dataSource)
                {
                    counter++;
                    if (counter % 2 == 0)
                    {
                        // It's even
                        strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 20%;'>" + dr.ProductName + "</td>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:White;'><td align='left' style='width: 25%;'>" + dr.ProductName + "</td>";
                    }
                    strTable += "<td align='left' style='width: 20%;'>" + dr.Quantity + "</td>";
                    strTable += "<td align='left' style='width: 20%;'>" + dr.PurchasePrice + "</td>";
                    strTable += "<td align='left' style='width: 20%;'>" + (dr.PurchasePrice * dr.Quantity) + "</td>";
                    strTable += "<td align='center' style='width: 15%;'>";
                    strTable += "&nbsp;<img src='../Images/edit.png' onClick='javascript:return PerformFillFormActionWAW(" + dr.DetailId + ", " + dr.PurchasePrice + ", " + dr.Quantity + "," + dr.RequisitionId + "," + dr.CategoryId + "," + dr.ProductId + ")' alt='Delete Information' border='0' />";
                    strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformProductReceiveDelete(" + dr.DetailId + ")' alt='Delete Information' border='0' />";
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


        [WebMethod(EnableSession = true)]
        public static string GetCalculatedTotal()
        {
            decimal AmtTotal = 0, tmpQuantity, tmpPurchasePrice;
            List<PMPurchaseOrderDetailsBO> detailListBO = HttpContext.Current.Session["PMPurchaseOrderDetails"] == null ? new List<PMPurchaseOrderDetailsBO>() : HttpContext.Current.Session["PMPurchaseOrderDetails"] as List<PMPurchaseOrderDetailsBO>;
            for (int i = 0; i < detailListBO.Count; i++)
            {
                tmpQuantity = detailListBO[i].Quantity;
                tmpPurchasePrice = detailListBO[i].PurchasePrice;
                AmtTotal += tmpQuantity * tmpPurchasePrice;
            }
            return new HMCommonDA().CurrencyMask(AmtTotal.ToString());
        }
        [WebMethod(EnableSession = true)]
        public static string PerformProductReceiveDelete(string detailsId)
        {
            int _ownerDetailId = Convert.ToInt32(detailsId);
            var ownerDetailBO = (List<PMPurchaseOrderDetailsBO>)HttpContext.Current.Session["PMPurchaseOrderDetails"];
            var ownerDetail = ownerDetailBO.Where(x => x.DetailId == _ownerDetailId).FirstOrDefault();
            ownerDetailBO.Remove(ownerDetail);
            HttpContext.Current.Session["PMPurchaseOrderDetails"] = ownerDetailBO;

            ArrayList list = new ArrayList();
            list = HttpContext.Current.Session["arrayDelete"] == null ? new ArrayList() : HttpContext.Current.Session["arrayDelete"] as ArrayList;
            list.Add(_ownerDetailId);
            HttpContext.Current.Session["arrayDelete"] = list as ArrayList;

            var dataSource = HttpContext.Current.Session["PMPurchaseOrderDetails"] as List<PMPurchaseOrderDetailsBO>;
            return GetProductOutDetailsGridView(dataSource);
        }

        [WebMethod(EnableSession = true)]
        public static string PerformLoadPMProductDetailOnEditMode(string pOrderId)
        {
            PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
            PMPurchaseOrderDA orderDetailDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderDetailsBO> orderDetailListBO = new List<PMPurchaseOrderDetailsBO>();
            orderDetailListBO = orderDetailDA.GetPMPurchaseOrderDetailForServiceByOrderId(Int32.Parse(pOrderId));
            HttpContext.Current.Session["PMPurchaseOrderDetails"] = orderDetailListBO;
            return GetProductOutDetailsGridView(orderDetailListBO);
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }

    }
}