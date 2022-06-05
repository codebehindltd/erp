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
using HotelManagement.Data.Payroll;

namespace HotelManagement.Presentation.Website.SalesManagment
{
    public partial class frmPMSalesReturn : System.Web.UI.Page
    {

        ArrayList arrayDelete;
        protected int _RestaurantComboId;
        protected int isMessageBoxEnable = -1;
        protected int IsService = -1;
        protected int btnPadding = -1;
        HMUtility hmUtility = new HMUtility();
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lblTransactionFor.Visible = false;
                this.txtControlTransationFor.Visible = false;
                Session["PMProductReceive"] = null;
                this.lblQuantityLabel.Visible = false;
                this.lblReceivedQuantityLabel.Visible = false;
                this.lblPOQuantity.Visible = false;
                this.lblReceivedQuantity.Visible = false;
                this.LoadBillNumber();
                this.LoadProduct();
                txtQuantity_Serial.Attributes.Add("class", "CustomTextBox");
            }
            arrayDelete = new ArrayList();
        }
        protected void ddlBillNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadTransactionFor();
            this.LoadProduct();
            this.ProductQuantityStatus();
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
                txtQuantity_Serial.Attributes.Add("class", "ThreeColumnTextBox");
                btnPadding = 1;
            }
            else
            {
                lblQuantity_Serial.Text = "Quantity";
                txtQuantity_Serial.Attributes.Add("class", "CustomTextBox");
            }
            this.ProductQuantityStatus();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {
                return;
            }
            PMProductReturnDA receiveDA = new PMProductReturnDA();
            Boolean status = true; //receiveDA.SavePMSalesReturnInfo(Session["PMProductReceive"] as List<PMProductReturnBO>);
            if (status == true)
            {
                isMessageBoxEnable = 2;
                lblMessage.Text = "Product Returned Successfully";
                this.ClearForm();
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int tmSerialId = 0;
            if (!IsDetailsFormValid())
            {
                return;
            }

            if (this.ddlBillNumber.SelectedValue != "0")
            {
                tmSerialId = ValidSerialNumber();
                if (tmSerialId == 0)
                {
                    isMessageBoxEnable = 1;
                    lblMessage.Text = "Invalid Product Serial Number.";
                    return;
                }
            }

            int dynamicDetailId = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            List<PMProductReturnBO> detailListBO = Session["PMProductReceive"] == null ? new List<PMProductReturnBO>() : Session["PMProductReceive"] as List<PMProductReturnBO>;
            decimal totalAddedCount = 0;
            int serialNumberCount = 0;
            if (detailListBO != null)
            {
                foreach (PMProductReturnBO row in detailListBO)
                {
                    //if (row.ProductId == Convert.ToInt32(this.ddlProductId.SelectedValue))
                    //{
                    //    // totalAddedCount = totalAddedCount + 1;
                    //    totalAddedCount = totalAddedCount + Convert.ToInt32(row.Quantity);
                    //}

                    //if (row.SerialNumber.ToString() == this.txtQuantity_Serial.Text)
                    //{
                    //    serialNumberCount = serialNumberCount + 1;
                    //}
                }
                decimal dueProductQuantity = 0;

                //dueProductQuantity = Convert.ToDecimal(this.lblReceivedQuantity.Text);

                decimal enteredQuantity = 0;

                if (this.lblQuantity_Serial.Text != "Serial Number")
                {
                    enteredQuantity = Convert.ToDecimal(this.txtQuantity_Serial.Text);
                }

                //if ((totalAddedCount + enteredQuantity) > dueProductQuantity)
                //{
                //    isMessageBoxEnable = 1;
                //    lblMessage.Text = "You can not receive more quantity than ordered";
                //    return;
                //}


                if (serialNumberCount > 0)
                {
                    isMessageBoxEnable = 1;
                    lblMessage.Text = "Serial Number Already Exist.";
                    return;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(lblHiddenOrderDetailtId.Text))
                        dynamicDetailId = Convert.ToInt32(lblHiddenOrderDetailtId.Text);
                    PMProductReturnBO detailBO = dynamicDetailId == 0 ? new PMProductReturnBO() : detailListBO.Where(x => x.ReturnId == dynamicDetailId).FirstOrDefault();
                    if (detailListBO.Contains(detailBO))
                        detailListBO.Remove(detailBO);

                    detailBO.ReturnType = this.ddlReturnType.SelectedItem.Text;

                    detailBO.TransactionId = Int32.Parse(this.ddlBillNumber.SelectedValue);

                    //detailBO.ProductId = Convert.ToInt32(this.ddlProductId.SelectedValue);
                    //detailBO.ProductName = this.ddlProductId.SelectedItem.Text;
                   // detailBO.SerialId = tmSerialId;
                    if (lblQuantity_Serial.Text == "Quantity")
                    {
                       // detailBO.Quantity = Convert.ToDecimal(txtQuantity_Serial.Text);
                        //detailBO.SerialNumber = "";
                    }
                    else
                    {
                       // detailBO.SerialNumber = txtQuantity_Serial.Text;
                        //detailBO.Quantity = 1;
                    }
                    detailBO.Remarks = this.txtRemarks.Text;
                    detailBO.CreatedBy = userInformationBO.UserInfoId;

                    detailBO.ReturnId = dynamicDetailId == 0 ? detailListBO.Count + 1 : dynamicDetailId;
                    detailListBO.Add(detailBO);
                    Session["PMProductReceive"] = detailListBO;
                    this.gvProductReceive.DataSource = Session["PMProductReceive"] as List<PMProductReturnBO>;
                    this.gvProductReceive.DataBind();
                    this.clearProductFields();
                }
            }
        }
        protected void gvProductReceive_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            this.gvProductReceive.PageIndex = e.NewPageIndex;
            this.LoadProductGridView();

        }
        protected void gvProductReceive_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                /*       ImageButton imgSelect = (ImageButton)e.Row.FindControl("ImgSelect");
                       imgSelect.Attributes["onclick"] = "javascript:return PerformCompanyAction('" + lblValue.Text + "');";*/
            }
        }
        protected void gvProductReceive_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int _ownerDetailId;
            arrayDelete = new ArrayList();
            if (e.CommandName == "CmdDelete")
            {
                _ownerDetailId = Convert.ToInt32(e.CommandArgument.ToString());
                var ownerDetailBO = (List<PMProductReceivedBO>)Session["PMProductReceive"];
                var ownerDetail = ownerDetailBO.Where(x => x.ReceivedId == _ownerDetailId).FirstOrDefault();
                ownerDetailBO.Remove(ownerDetail);
                Session["PMProductReceive"] = ownerDetailBO;
                arrayDelete.Add(_ownerDetailId);
                this.gvProductReceive.DataSource = Session["PMProductReceive"] as List<PMProductReceivedBO>;
                this.gvProductReceive.DataBind();

            }
        }
        protected void ddlReturnType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadTransactionFor();
        }
        //************************ User Defined Function ********************//
        private void LoadBillNumber()
        {
            PMSalesDetailsDA entityDA = new PMSalesDetailsDA();
            this.ddlBillNumber.DataSource = entityDA.GetAllSalesInformation();
            this.ddlBillNumber.DataTextField = "BillNumber";
            this.ddlBillNumber.DataValueField = "SalesId";
            this.ddlBillNumber.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "Internal";
            this.ddlBillNumber.Items.Insert(0, item);
        }
        private void LoadProduct()
        {
            int salesId = Int32.Parse(ddlBillNumber.SelectedValue);

            if (salesId != 0)
            {
                PMSalesDetailsDA entityDA = new PMSalesDetailsDA();
                ddlProductId.DataSource = entityDA.GetPMSalesDetailsBySalesId(salesId);
                this.ddlProductId.DataTextField = "ItemName";
                this.ddlProductId.DataValueField = "ItemId";
                this.ddlProductId.DataBind();
            }
            else
            {
                InvItemDA entityDA = new InvItemDA();
                ddlProductId.DataSource = entityDA.GetInvItemInfo();
                this.ddlProductId.DataTextField = "Name";
                this.ddlProductId.DataValueField = "ItemId";
                this.ddlProductId.DataBind();
            }

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlProductId.Items.Insert(0, item);
        }
        private void LoadTransactionFor()
        {
            this.lblTransactionFor.Visible = false;
            this.txtControlTransationFor.Visible = false;
        }
        private void ProductQuantityStatus()
        {
            this.lblQuantityLabel.Visible = false;
            this.lblReceivedQuantityLabel.Visible = false;
            this.lblPOQuantity.Visible = false;
            this.lblReceivedQuantity.Visible = false;
            PMPurchaseOrderDA productDA = new PMPurchaseOrderDA();
            PMPurchaseOrderDetailsBO productBO = new PMPurchaseOrderDetailsBO();
            int pOrderId = Int32.Parse(this.ddlBillNumber.SelectedValue);
            int selectedProduct = Int32.Parse(this.ddlProductId.SelectedValue);
            productBO = productDA.GetPMPurchaseOrderDetailsByProductId(pOrderId, selectedProduct);

            if (productBO != null)
            {
                if ((productBO.Quantity + productBO.QuantityReceived) != 0)
                {
                    this.lblQuantityLabel.Visible = true;
                    this.lblReceivedQuantityLabel.Visible = true;
                    this.lblPOQuantity.Visible = true;
                    this.lblReceivedQuantity.Visible = true;
                    this.lblPOQuantity.Text = productBO.Quantity.ToString();
                    this.lblReceivedQuantity.Text = productBO.QuantityReceived.ToString();
                }
            }
        }
        public bool isValidForm()
        {
            bool status = true;
            if (Session["PMProductReceive"] == null)
            {
                status = false;
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Add Product Quantity Information";
                this.ddlProductId.Focus();
            }
            else if (ddlProductId.SelectedIndex == 0)
            {
                status = false;
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Select Product Id";
                this.ddlProductId.Focus();
            }
            return status;

        }
        private void ClearForm()
        {
            Session["PMProductReceive"] = null;
            this.gvProductReceive.DataSource = Session["PMProductReceive"] as List<PMProductReceivedBO>;
            this.gvProductReceive.DataBind();
            this.txtQuantity_Serial.Text = "";
            this.ddlBillNumber.SelectedIndex = 0;
            this.ddlProductId.SelectedIndex = 0;
        }
        private int ValidSerialNumber()
        {
            int tmpSerialId = 0;
            PMProductSerialInfoDA entityDA = new PMProductSerialInfoDA();
            PMProductSerialInfoBO entityBO = new PMProductSerialInfoBO();

            entityBO = entityDA.GetPMProductSerialInfoBySerialNumberForSaleReturn(Convert.ToInt32(this.ddlBillNumber.SelectedValue), Convert.ToInt32(this.ddlProductId.SelectedValue), this.txtQuantity_Serial.Text);
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
                    isMessageBoxEnable = 1;
                    lblMessage.Text = "Please Fill The Quantity";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtQuantity_Serial.Text))
                {
                    status = false;
                    isMessageBoxEnable = 1;
                    lblMessage.Text = "Please Fill The Serial Number";
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
            item.Text = "---None---";
            this.ddlProductId.Items.Insert(0, item);
        }
        private void LoadProductGridView()
        {
            gvProductReceive.DataSource = null;
            gvProductReceive.DataBind();
        }
    }
}