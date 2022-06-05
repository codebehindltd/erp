using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.IO;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.SalesManagment;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Data.Inventory;
using System.Web.Services;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.GeneralLedger;
using System.Net.Mail;
using HotelManagement.Data.PurchaseManagment;

namespace HotelManagement.Presentation.Website.SalesManagment
{
    public partial class frmPMSales : System.Web.UI.Page
    {
        ArrayList arrayDelete;
        protected int _salesId;
        protected int isMessageBoxEnable = -1;
        protected int IsService = -1;
        HMUtility hmUtility = new HMUtility();
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            this.AddEditODeleteDetail();
            if (!IsPostBack)
            {
                this.SalesInvetorySeparateConfiguration();
                string cardValidation = System.Web.Configuration.WebConfigurationManager.AppSettings["CardValidation"].ToString();
                txtCardValidation.Value = cardValidation.ToString();
                Session["arrayDelete"] = null;
                Session["PMSalesDetailList"] = null;
                Session["GuestPaymentDetailListForGrid"] = null;
                Session["PMProductOut"] = null;
                this.LoadCustomerType();
                this.LoadCustomer();
                this.LoadCurrency();
                this.LoadCommonDropDownHiddenField();
                LoadAccountHeadInfo();
                LoadBank();
                this.LoadProductCategory();
                this.LoadProductManufacturer();
                this.LoadServiceInformation();
                this.LoadServiceBundleInformation();
                this.LoadCustomerName();
                this.LoadCreditAccountHead();
                this.LoadBandwidthType();
                this.LoadSiteInformation();
                this.LoadTechnicalInformation();
                this.LoadBillingformation();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
        }
        protected void btnOwnerDetails_Click(object sender, EventArgs e)
        {
            if (!IsDetailFormValid())
            {
                return;
            }
            int dynamicDetailId = 0;

            PMSalesDetailsDA salesDetailsDA = new PMSalesDetailsDA();
            PMSalesDetailBO salesDetailsBO = new PMSalesDetailBO();

            List<PMSalesDetailBO> detailListBO = Session["PMSalesDetailList"] == null ? new List<PMSalesDetailBO>() : Session["PMSalesDetailList"] as List<PMSalesDetailBO>;
            if (!string.IsNullOrWhiteSpace(detailId.Value))
                dynamicDetailId = Convert.ToInt32(detailId.Value);
            PMSalesDetailBO detailBO = dynamicDetailId == 0 ? new PMSalesDetailBO() : detailListBO.Where(x => x.DetailId == dynamicDetailId).FirstOrDefault();
            if (detailListBO.Contains(detailBO))
                detailListBO.Remove(detailBO);
            detailBO.ServiceType = ddlServiceType.SelectedValue.ToString();
            detailBO.ItemUnit = Convert.ToDecimal(this.txtUnit.Text.ToString());
            int productId = Int32.Parse(txtHiddenProductId.Value);
            string ItemName = txtHiddenProductName.Value.ToString();
            if (detailBO.ServiceType == "Product")
            {
                detailBO.ItemId = productId;
                detailBO.ItemName = ItemName;
                salesDetailsBO = salesDetailsDA.GetSalesCurrencyInformation("PMProduct", detailBO.ItemId);
            }
            else if (detailBO.ServiceType == "Service")
            {
                detailBO.ItemId = productId;
                detailBO.ItemName = ItemName;
                salesDetailsBO = salesDetailsDA.GetSalesCurrencyInformation("SalesService", detailBO.ItemId);
            }
            else
            {
                detailBO.ItemId = productId;
                detailBO.ItemName = ItemName;
                salesDetailsBO = salesDetailsDA.GetSalesCurrencyInformation("SalesServiceBundle", detailBO.ItemId);
            }


            if (ddlCurrency.SelectedValue == salesDetailsBO.SellingLocalCurrencyId.ToString())
            {
                detailBO.TotalPrice = Convert.ToDecimal(txtUnitPriceLocal.Text) * Convert.ToDecimal(txtUnit.Text);
                detailBO.SellingLocalCurrencyId = salesDetailsBO.SellingLocalCurrencyId;
                detailBO.UnitPriceLocal = Convert.ToDecimal(txtUnitPriceLocal.Text);
                detailBO.SellingLocalCurrencyId = 0;
                detailBO.UnitPriceUsd = 0;
            }
            else if (ddlCurrency.SelectedValue == salesDetailsBO.SellingUsdCurrencyId.ToString())
            {
                detailBO.TotalPrice = Convert.ToDecimal(txtUnitPriceUSD.Text) * Convert.ToDecimal(txtUnit.Text);
                detailBO.SellingLocalCurrencyId = 0;
                detailBO.UnitPriceLocal = 0;
                detailBO.SellingUsdCurrencyId = salesDetailsBO.SellingUsdCurrencyId;
                detailBO.UnitPriceUsd = Convert.ToDecimal(txtUnitPriceUSD.Text);
            }

            detailBO.DetailId = dynamicDetailId == 0 ? detailListBO.Count + 1 : dynamicDetailId;
            detailListBO.Add(detailBO);
            Session["PMSalesDetailList"] = detailListBO;

            decimal salesAmount = this.CalculateGrandTotal(detailListBO);
            this.txtSalesAmount.Text = salesAmount.ToString();
            this.txtGrandTotal.Text = salesAmount.ToString();
            //   this.gvRoomOwnerDtail.DataSource = Session["PMSalesDetailList"] as List<PMSalesDetailBO>;
            //   this.gvRoomOwnerDtail.DataBind();
            this.ClearDetailPart();
            SetTab("Entry");
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Cancel();
        }
        protected void gvRoomOwner_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Visible = true;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void ddlServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlServiceType.SelectedIndex == 1)
            {

                this.LoadServiceItem();
            }
            else if (this.ddlServiceType.SelectedIndex == 2)
            {

                this.LoadServiceBundle();
            }
            else
            {

            }
        }
        protected void ddlFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadServiceItem();
            this.LoadServiceBundle();
        }
        protected void gvRoomOwnerDtail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                Label lblServiceType = (Label)e.Row.FindControl("lblServiceType");
                Label lblItemId = (Label)e.Row.FindControl("lblItemId");
                Label lblItemUnit = (Label)e.Row.FindControl("lblItemUnit");
                Label lblTotalPrice = (Label)e.Row.FindControl("lblTotalPrice");

                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormActionWAW('" + lblValue.Text + "','" + lblServiceType.Text + "','" + lblItemId.Text + "','" + lblItemUnit.Text + "','" + lblTotalPrice.Text + "');";
                imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteDetailsWAW('" + lblValue.Text + "');";
                imgUpdate.Visible = true;
                imgDelete.Visible = true;
            }
        }
        protected void gvRoomOwnerDtail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            this.lblMessage.Text = string.Empty;
            // this.gvRoomOwnerDtail.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void gvRoomOwnerDtail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _ownerDetailId;
            if (e.CommandName == "CmdEdit")
            {
                _ownerDetailId = Convert.ToInt32(e.CommandArgument.ToString());

                detailId.Value = _ownerDetailId.ToString();
                var ownerDetailBO = (List<PMSalesDetailBO>)Session["PMSalesDetailList"];
                var ownerDetail = ownerDetailBO.Where(x => x.DetailId == _ownerDetailId).FirstOrDefault();
                if (ownerDetail != null && ownerDetail.DetailId > 0)
                {
                    this.txtUnit.Text = ownerDetail.ItemUnit.ToString();
                    this.ddlServiceType.SelectedValue = ownerDetail.ServiceType;

                    if (ownerDetail.ServiceType == "Product")
                    {
                        IsService = 1;
                        this.ddlProductId.SelectedValue = ownerDetail.ItemId.ToString();
                    }
                    else if (ownerDetail.ServiceType == "Service")
                    {
                        IsService = 2;
                        this.ddlServiceId.SelectedValue = ownerDetail.ItemId.ToString();
                    }
                    else
                    {
                        IsService = 3;
                        this.ddlServiceBundleId.SelectedValue = ownerDetail.ItemId.ToString();
                    }
                    //    btnOwnerDetails.Text = "Edit";
                }
                else
                {
                    //        btnOwnerDetails.Text = "Add";
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _ownerDetailId = Convert.ToInt32(e.CommandArgument.ToString());
                var ownerDetailBO = (List<PMSalesDetailBO>)Session["PMSalesDetailList"];
                var ownerDetail = ownerDetailBO.Where(x => x.DetailId == _ownerDetailId).FirstOrDefault();
                ownerDetailBO.Remove(ownerDetail);
                Session["PMSalesDetailList"] = ownerDetailBO;
                arrayDelete.Add(_ownerDetailId);
                //  this.gvRoomOwnerDtail.DataSource = Session["PMSalesDetailList"] as List<PMSalesDetailBO>;
                //  this.gvRoomOwnerDtail.DataBind();


                decimal salesAmount = this.CalculateGrandTotal(ownerDetailBO);
                this.txtSalesAmount.Text = salesAmount.ToString();
                this.txtGrandTotal.Text = salesAmount.ToString();

            }
        }
        protected void gvRoomOwner_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdEdit")
            {
                this._salesId = Convert.ToInt32(e.CommandArgument.ToString());
                Session["_salesId"] = this._salesId;
                this.FillForm(this._salesId);
                SetTab("Entry");
            }
            else if (e.CommandName == "CmdDelete")
            {
                try
                {
                    this._salesId = Convert.ToInt32(e.CommandArgument.ToString());
                    Session["_salesId"] = this._salesId;
                    this.DeleteData(this._salesId);
                    this.Cancel();
                    this.LoadGridView();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                return;
            }

            if (!IsAdditionalInformationValid())
            {
                return;
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            PMSalesBO salesBO = new PMSalesBO();
            PMSalesDetailsDA da = new PMSalesDetailsDA();
            salesBO.SalesDate = hmUtility.GetDateTimeFromString(this.txtSalesDate.Text, userInformationBO.ServerDateFormat);

            // Customer Information----------------------
            SalesCustomerBO customarBO = new SalesCustomerBO();
            SalesCustomerDA customerDA = new SalesCustomerDA();
            customarBO.CustomerType = this.ddlCustomerType.SelectedItem.Text;
            customarBO.Name = txtName.Text;
            //customarBO.Code = txtCode.Text;
            customarBO.Email = txtEmail.Text;
            customarBO.WebAddress = txtWebAddress.Text;
            customarBO.Phone = txtPhone.Text;
            customarBO.Address = txtAddress.Text;


            if (ddlCustomerId.SelectedValue == "0")
            {
                //SalesCustomerBO customarBO = new SalesCustomerBO();
                //SalesCustomerDA customerDA = new SalesCustomerDA();
                //customarBO.CustomerType = this.ddlCustomerType.SelectedItem.Text;
                //customarBO.Name = txtName.Text;
                ////customarBO.Code = txtCode.Text;
                //customarBO.Email = txtEmail.Text;
                //customarBO.WebAddress = txtWebAddress.Text;
                //customarBO.Phone = txtPhone.Text;
                //customarBO.Address = txtAddress.Text;
                int tmpCustomerId = 0;
                customarBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = customerDA.SaveSalesCustomerInfo(customarBO, out tmpCustomerId);
                salesBO.CustomerId = tmpCustomerId;
            }
            else
            {
                salesBO.CustomerId = Int32.Parse(ddlCustomerId.SelectedValue);

                customarBO.CustomerId = salesBO.CustomerId;
                customarBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = customerDA.UpdateSalesCustomerInfo(customarBO);
            }

            //if (!string.IsNullOrWhiteSpace(hfIsSeparateSalesInventory.Value))
            //{
            //    if (hfIsSeparateSalesInventory.Value == "Yes")
            //    {
            //        salesBO.IsSeparateSalesInventory = 0;
            //    }
            //    else
            //    {
            //        salesBO.IsSeparateSalesInventory = 1;
            //    }
            //}

            salesBO.Remarks = txtRemarks.Text;
            salesBO.Frequency = HiddenFrequencyId.Value;
            salesBO.SalesAmount = Convert.ToDecimal(txtSalesAmount.Text);
            salesBO.VatAmount = Convert.ToDecimal(txtVatAmount.Text);
            salesBO.GrandTotal = Convert.ToDecimal(txtGrandTotal.Text);
            salesBO.FieldId = Int32.Parse(ddlCurrency.SelectedValue);
            if (chkIsSiteEnable.Checked == true)
            {
                salesBO.SiteInfoId = this.SaveSiteInformation(salesBO.CustomerId);
                salesBO.TechnicalInfoId = this.SaveTechnicalInformation(salesBO.CustomerId);
                salesBO.BillingInfoId = this.SaveBillingInformation(salesBO.CustomerId);
            }
            if (this.ddlFrequency.SelectedIndex == 0)
            {
                salesBO.BillExpireDate = salesBO.SalesDate;
            }
            else if (this.ddlFrequency.SelectedIndex == 1)
            {
                salesBO.BillExpireDate = salesBO.SalesDate.AddMonths(1);
            }
            else if (this.ddlFrequency.SelectedIndex == 2)
            {
                salesBO.BillExpireDate = salesBO.SalesDate.AddMonths(3);
            }
            else if (this.ddlFrequency.SelectedIndex == 3)
            {
                salesBO.BillExpireDate = salesBO.SalesDate.AddMonths(6);
            }
            else if (this.ddlFrequency.SelectedIndex == 4)
            {
                salesBO.BillExpireDate = salesBO.SalesDate.AddMonths(12);
            }

            
            
            if (this.btnSave.Text.Equals("Save"))
            {
                int tmpSalesId = 0;
                salesBO.CreatedBy = userInformationBO.UserInfoId;
                int salesId = da.SavePMSalesInfo(salesBO, out tmpSalesId, Session["PMSalesDetailList"] as List<PMSalesDetailBO>, Session["GuestPaymentDetailListForGrid"] as List<PMSalesBillPaymentBO>, Session["PMProductOut"] as List<PMProductOutBO>);
                if (salesId > 0)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Saved Operation Successfull";

                    this.Cancel();
                    string From = "Sales";
                    int Id = 0;
                    string url = "Reports/frmReportPMSalesInvoice.aspx?SalesId=" + salesId + "&From=" + From + "&InvoiceId=" + Id;
                    string sPopUp = "window.open('" + url + "', 'popup_window', 'width=715,height=780,left=300,top=50,resizable=yes, scrollbars=1');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
                }
            }
            else
            {
                salesBO.SalesId = Convert.ToInt32(Session["_salesId"]);
                salesBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = da.UpdatePMSalesInfo(salesBO, Session["PMSalesDetailList"] as List<PMSalesDetailBO>, Session["DeletedPMSalesDetailList"] as List<PMSalesDetailBO>,Session["arrayDelete"] as ArrayList);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Update Operation Successfull";
                    this.Cancel();
                }
            }
            txtHiddenSalesId.Value = "";
            SetTab("Entry");
        }
        //************************ User Defined Function ********************//
        private void SalesInvetorySeparateConfiguration()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardSalesInventory", "SalesInventorySeparate");
            if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
            {
                if (commonSetupBO.SetupValue == "Yes")
                {
                    hfIsSeparateSalesInventory.Value = "Yes";
                }
                else
                {
                    hfIsSeparateSalesInventory.Value = "No";
                }
            }
        }
        public static string GetSalesInvetorySeparateConfiguration()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardSalesInventory", "SalesInventorySeparate");
            return commonSetupBO.SetupValue;
        }
        private bool IsAdditionalInformationValid()
        {
            bool status = true;
            if (chkIsSiteEnable.Checked == true)
            {
                if (string.IsNullOrWhiteSpace(this.txtSiteName.Text))
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Please Enter Site Name.";
                    this.txtSiteName.Focus();
                    status = false;
                }

            }
            return status;

        }
        public int SaveSiteInformation(int customerId)
        {
            int siteInfoId = -1;
            int tempId;
            PMSalesSiteInfoDA da = new PMSalesSiteInfoDA();
            PMSalesSiteInfoBO bo = new PMSalesSiteInfoBO();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            bo.SiteName = txtSiteName.Text;
            bo.SiteEmail = txtSiteEmail.Text;
            bo.SiteContactPerson = txtSiteContactPerson.Text;
            bo.SiteAddress = txtSiteAddress.Text;
            bo.SitePhoneNumber = txtSitePhoneNumber.Text;
            bo.CustomerId = customerId;

            if (ddlSiteInformation.SelectedValue != "0")
            {
                bo.LastModifiedBy = userInformationBO.UserInfoId;
                bo.SiteInfoId = Int32.Parse(ddlSiteInformation.SelectedValue);
                siteInfoId = bo.SiteInfoId;
                Boolean status = da.UpdatePMSalesSiteInfo(bo);
            }
            else
            {
                bo.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = da.SavePMSalesSiteInfo(bo, out tempId);
                siteInfoId = tempId;
            }
            return siteInfoId;
        }
        public int SaveTechnicalInformation(int customerId)
        {
            int technicalId = -1;
            int tempId;
            PMSalesTechnicalInfoBO bo = new PMSalesTechnicalInfoBO();
            PMSalesTechnicalInfoDA da = new PMSalesTechnicalInfoDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            bo.TechnicalContactPerson = txtTechnicalContactPerson.Text;
            bo.TechnicalPersonDepartment = txtTechnicalPersonDepartment.Text;
            bo.TechnicalPersonDesignation = txtTechnicalPersonDesignation.Text;
            bo.TechnicalPersonPhone = txtTechnicalPersonPhone.Text;
            bo.TechnicalPersonEmail = txtTechnicalPersonEmail.Text;
            bo.CustomerId = customerId;
            if (ddlTechnicalInformation.SelectedValue != "0")
            {
                bo.LastModifiedBy = userInformationBO.UserInfoId;
                bo.TechnicalInfoId = Int32.Parse(ddlTechnicalInformation.SelectedValue);
                technicalId = bo.TechnicalInfoId;
                Boolean status = da.UpdatePMSalesTechnicalInfo(bo);
            }
            else
            {
                bo.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = da.SavePMSalesTechnicalInfo(bo, out tempId);
                technicalId = tempId;
            }

            return technicalId;
        }
        public int SaveBillingInformation(int customerId)
        {
            int billingId = -1;
            int tempId;
            PMSalesBillingInfoDA da = new PMSalesBillingInfoDA();
            PMSalesBillingInfoBO bo = new PMSalesBillingInfoBO();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            bo.BillingContactPerson = txtBillingContactPerson.Text;
            bo.BillingPersonDepartment = txtBillingPersonDepartment.Text;
            bo.BillingPersonDesignation = txtBillingPersonDesignation.Text;
            bo.BillingPersonPhone = txtBillingPersonPhone.Text;
            bo.BillingPersonEmail = txtBillingPersonEmail.Text;
            bo.CustomerId = customerId;
            if (ddlBillingInformarion.SelectedValue != "0")
            {
                bo.LastModifiedBy = userInformationBO.UserInfoId;
                bo.BillingInfoId = Int32.Parse(ddlTechnicalInformation.SelectedValue);
                billingId = bo.BillingInfoId;
                Boolean status = da.UpdatePMSalesBillingInfo(bo);
            }
            else
            {
                bo.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = da.SavePMSalesBillingInfo(bo, out tempId);
                billingId = tempId;
            }
            return billingId;
        }
        private void LoadSiteInformation()
        {
            PMSalesSiteInfoDA entityDA = new PMSalesSiteInfoDA();
            this.ddlSiteInformation.DataSource = entityDA.GetAllPMSalesSiteInfo();
            this.ddlSiteInformation.DataTextField = "SiteName";
            this.ddlSiteInformation.DataValueField = "SiteInfoId";
            this.ddlSiteInformation.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "New Site Information";
            this.ddlSiteInformation.Items.Insert(0, item);
        }
        private void LoadTechnicalInformation()
        {
            PMSalesTechnicalInfoDA entityDA = new PMSalesTechnicalInfoDA();
            this.ddlTechnicalInformation.DataSource = entityDA.GetAllPMSalesTechnicalInfo();
            this.ddlTechnicalInformation.DataTextField = "TechnicalContactPerson";
            this.ddlTechnicalInformation.DataValueField = "TechnicalInfoId";
            this.ddlTechnicalInformation.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "New Information";
            this.ddlTechnicalInformation.Items.Insert(0, item);
        }
        private void LoadBillingformation()
        {
            PMSalesBillingInfoDA entityDA = new PMSalesBillingInfoDA();
            this.ddlBillingInformarion.DataSource = entityDA.GetAllPMSalesBillingInfo();
            this.ddlBillingInformarion.DataTextField = "BillingContactPerson";
            this.ddlBillingInformarion.DataValueField = "BillingInfoId";
            this.ddlBillingInformarion.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "New Information";
            this.ddlBillingInformarion.Items.Insert(0, item);
        }
        private void LoadSiteInformationById(int id)
        {
            PMSalesSiteInfoDA siteDA = new PMSalesSiteInfoDA();
            var siteBO = siteDA.GetAllPMSalesSiteInfoBySiteInfoId(id);

            txtSiteName.Text = siteBO.SiteName; ;
            txtSiteEmail.Text = siteBO.SiteEmail;
            txtSiteContactPerson.Text = siteBO.SiteContactPerson;
            txtSiteAddress.Text = siteBO.SiteAddress;
            txtSitePhoneNumber.Text = siteBO.SitePhoneNumber;
            ddlSiteInformation.SelectedValue = siteBO.SiteInfoId.ToString();
        }
        private void LoadTechnicalInformationById(int id)
        {
            PMSalesTechnicalInfoDA technicalDA = new PMSalesTechnicalInfoDA();
            var technicalBO = technicalDA.GetPMSalesTechnicalInfoByTechnicalInfoId(id);

            txtTechnicalContactPerson.Text = technicalBO.TechnicalContactPerson;
            txtTechnicalPersonDepartment.Text = technicalBO.TechnicalPersonDepartment;
            txtTechnicalPersonDesignation.Text = technicalBO.TechnicalPersonDesignation;
            txtTechnicalPersonPhone.Text = technicalBO.TechnicalPersonPhone;
            txtTechnicalPersonEmail.Text = technicalBO.TechnicalPersonEmail;
            ddlTechnicalInformation.SelectedValue = technicalBO.TechnicalInfoId.ToString();
        }
        private void LoadBillingInformationById(int id)
        {
            PMSalesBillingInfoDA billingDA = new PMSalesBillingInfoDA();
            var billingBO = billingDA.GetAllPMSalesBillingInfoBillingInfoId(id);
            txtBillingContactPerson.Text = billingBO.BillingContactPerson;
            txtBillingPersonDepartment.Text = billingBO.BillingPersonDepartment;
            txtBillingPersonDesignation.Text = billingBO.BillingPersonDesignation;
            txtBillingPersonPhone.Text = billingBO.BillingPersonPhone;
            txtBillingPersonEmail.Text = billingBO.BillingPersonEmail;
        }
        private void LoadBandwidthType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();

            fields = commonDA.GetCustomField("IsBandwidthInfoEnable", hmUtility.GetDropDownFirstValue());
            txtBandwidthType.Value = fields[1].FieldValue == "Yes" ? true.ToString() : false.ToString();
        }
        private void LoadCreditAccountHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            this.lblPaymentAccountHead.Text = "Payment Through";
            CustomFieldBO customFieldForCash = new CustomFieldBO();
            customFieldForCash = hmCommonDA.GetCustomFieldByFieldName("CreditReceiveAccountsInfoForSalesBillPayment");

            var List = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + customFieldForCash.FieldValue.ToString() + ")");
            if (List.Count <= 0)
            {
                ListItem itemToRemove = ddlPayMode.Items.FindByValue("Credit");
                if (itemToRemove != null)
                {
                    ddlPayMode.Items.Remove(itemToRemove);
                }
            }

            this.ddlCreditAccountHead.DataSource = List;
            this.ddlCreditAccountHead.DataTextField = "NodeHead";
            this.ddlCreditAccountHead.DataValueField = "NodeId";
            this.ddlCreditAccountHead.DataBind();
        }
        private void LoadServiceInformation()
        {
            List<SalesServiceBO> serviceList = new List<SalesServiceBO>();
            SalesServiceDA serviceDA = new SalesServiceDA();
            serviceList = serviceDA.GetSaleServicInfo();
            ddlServiceId.DataSource = serviceList;
            this.ddlServiceId.DataTextField = "Name";
            this.ddlServiceId.DataValueField = "ServiceId";
            this.ddlServiceId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlServiceId.Items.Insert(0, item);

        }
        private void LoadServiceBundleInformation()
        {
            List<SalesServiceBundleBO> bundleList = new List<SalesServiceBundleBO>();
            SalesServiceBundleDetailsDA bundleDA = new SalesServiceBundleDetailsDA();
            bundleList = bundleDA.GetSalesServiceBundleInfo();
            ddlServiceBundleId.DataSource = bundleList;
            this.ddlServiceBundleId.DataTextField = "BundleName";
            this.ddlServiceBundleId.DataValueField = "BundleId";
            this.ddlServiceBundleId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlServiceBundleId.Items.Insert(0, item);

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

            this.ddlCurrency.DataSource = fields;
            this.ddlCurrency.DataTextField = "FieldValue";
            this.ddlCurrency.DataValueField = "FieldId";
            this.ddlCurrency.DataBind();
            this.ddlCurrency.SelectedIndex = 0;

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("CurrencyType", "CurrencyConfiguration");
            if (commonSetupBO.SetupId > 0)
            {
                if (commonSetupBO.SetupValue == "Single")
                {
                    this.ddlCurrency.Enabled = false;
                }
                else
                {
                    this.ddlCurrency.Enabled = true;
                }
            }
        }
        private void LoadCustomer()
        {
            SalesCustomerDA entityDA = new SalesCustomerDA();
            this.ddlCustomerId.DataSource = entityDA.GetAllSalesCustomerInfo();
            this.ddlCustomerId.DataTextField = "DisplayName";
            this.ddlCustomerId.DataValueField = "CustomerId";
            this.ddlCustomerId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "New Customer";
            this.ddlCustomerId.Items.Insert(0, item);
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
        private void LoadServiceItem()
        {
            SalesServiceDA serviceDA = new SalesServiceDA();
            this.ddlServiceId.DataSource = serviceDA.GetSaleServicInfoByFrequency(this.ddlFrequency.SelectedItem.Text);
            this.ddlServiceId.DataTextField = "Name";
            this.ddlServiceId.DataValueField = "serviceId";
            this.ddlServiceId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlServiceId.Items.Insert(0, item);
        }
        private void LoadServiceBundle()
        {
            SalesServiceBundleDetailsDA bundleDA = new SalesServiceBundleDetailsDA();
            List<SalesServiceBundleBO> List = bundleDA.GetSalesServiceBundleInfoByFrequency(this.ddlFrequency.SelectedItem.Text);
            this.ddlServiceBundleId.DataSource = List;
            this.ddlServiceBundleId.DataTextField = "BundleName";
            this.ddlServiceBundleId.DataValueField = "BundleId";
            this.ddlServiceBundleId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlServiceBundleId.Items.Insert(0, item);
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmPMSales.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            //btnOwnerDetails.Visible = isSavePermission;
        }
        private bool IsDetailFormValid()
        {
            bool status = true;

            if (this.ddlServiceType.SelectedIndex == 0)
            {
                if (this.txtHiddenProductId.Value == "" || Int32.Parse(this.txtHiddenProductId.Value) == 0)
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Please Select Valid Product";
                    this.ddlProductId.Focus();
                    status = false;
                }
            }
            else if (this.ddlServiceType.SelectedIndex == 1)
            {
                if (this.txtHiddenProductId.Value == "" || Int32.Parse(this.txtHiddenProductId.Value) == 0)
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Please Select Valid Service";
                    this.ddlProductId.Focus();
                    status = false;
                }
            }
            else
            {
                if (this.txtHiddenProductId.Value == "" || Int32.Parse(this.txtHiddenProductId.Value) == 0)
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Please Select Valid Service";
                    this.ddlProductId.Focus();
                    status = false;
                }

            }

            if (string.IsNullOrWhiteSpace(this.txtUnit.Text))
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Enter Quantity";
                status = false;
            }
            return status;

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
        private bool IsFormValid()
        {
            bool status = true;
            bool isContactEmail = false;
            var detailList = Session["PMSalesDetailList"] as List<PMSalesDetailBO>;
            if (string.IsNullOrWhiteSpace(this.txtSalesDate.Text))
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Enter Sales Date";
                this.txtSalesDate.Focus();
                status = false;
            }
            else if (detailList == null)
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Add Some Item";
                this.ddlServiceType.Focus();
                status = false;
            }
            else if (this.ddlCustomerId.SelectedIndex == 0)
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    this.isMessageBoxEnable = 1;
                    this.lblMessage.Text = "Please Enter Customer Name";
                    this.txtName.Focus();
                    status = false;
                }

                else if (ddlCustomerType.SelectedIndex == 0)
                {
                    this.isMessageBoxEnable = 1;
                    this.lblMessage.Text = "Please Select Customer Type";
                    this.ddlCustomerType.Focus();
                    status = false;
                }
                //else if (string.IsNullOrEmpty(txtPhone.Text))
                //{
                //    this.isMessageBoxEnable = 1;
                //    this.lblMessage.Text = "Please Enter Phone Number";
                //    this.txtPhone.Focus();
                //    status = false;
                //}
            }
            else if (txtBandwidthType.Value == "true")
            {
                if (chkIsSiteEnable.Checked == true)
                {

                    if (string.IsNullOrEmpty(txtSiteName.Text))
                    {
                        this.isMessageBoxEnable = 1;
                        this.lblMessage.Text = "Please Enter Site Name.";
                        this.txtSiteName.Focus();
                        status = false;
                    }

                    else if (string.IsNullOrEmpty(txtSiteAddress.Text))
                    {
                        this.isMessageBoxEnable = 1;
                        this.lblMessage.Text = "Please Enter Site Address.";
                        this.txtSiteAddress.Focus();
                        status = false;
                    }
                    else if (string.IsNullOrEmpty(txtSiteContactPerson.Text))
                    {
                        this.isMessageBoxEnable = 1;
                        this.lblMessage.Text = "Please Enter Site Contact Person.";
                        this.txtSiteContactPerson.Focus();
                        status = false;
                    }
                    //else if (string.IsNullOrEmpty(txtSitePhoneNumber.Text))
                    //{
                    //    this.isMessageBoxEnable = 1;
                    //    this.lblMessage.Text = "Please Enter Site Phone Number.";
                    //    this.txtSitePhoneNumber.Focus();
                    //    status = false;
                    //}
                    else if (string.IsNullOrEmpty(txtSiteEmail.Text))
                    {
                        this.isMessageBoxEnable = 1;
                        this.lblMessage.Text = "Please Enter Site Email.";
                        this.txtSiteEmail.Focus();
                        status = false;
                    }

                }
            }
            return status;
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        public static void OpenNewBrowserWindow(string Url, Control control)
        {
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Open", "window.open('" + Url + "');", true);
        }
        private void LoadGridView()
        {
            this.CheckObjectPermission();
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
            DateTime FromDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            int CustomerId = Int32.Parse(ddlCustomerName.SelectedValue);
            string BillNo = this.txtBillNo.Text.ToString();

            List<PMSalesBO> list = new List<PMSalesBO>();
            PMSalesDetailsDA da = new PMSalesDetailsDA();
            list = da.GetAllSalesInformationBySearchCriteria(FromDate, ToDate, CustomerId, BillNo);
            this.gvRoomOwner.DataSource = list;
            this.gvRoomOwner.DataBind();
            SetTab("Search");
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
        private void Cancel()
        {
            this.ddlCustomerId.SelectedValue = "0";
            this.btnSave.Text = "Save";
            Session["arrayDelete"] = null;
            Session["PMSalesDetailList"] = null;
            Session["GuestPaymentDetailListForGrid"] = null;
            this.ClearDetailPart();
            this.txtSalesAmount.Text = "0";
            this.txtRemarks.Text = string.Empty;
            this.txtVatAmount.Text = "0";
            this.txtGrandTotal.Text = "0";
            Session["PMProductOut"] = null;
            this.txtAddress.Text = string.Empty;
            this.txtWebAddress.Text = string.Empty;
            this.txtEmail.Text = string.Empty;
            this.txtPhone.Text = string.Empty;
            this.ddlCustomerType.SelectedIndex = 0;
            this.txtName.Text = string.Empty;
            this.ddlCustomerId.SelectedIndex = 0;
            this.ClearAdditionlServiceInformation();
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
        private decimal CalculateGrandTotal(List<PMSalesDetailBO> salesDetailList)
        {
            int Count = salesDetailList.Count;
            decimal salesAmount = 0;
            for (int i = 0; i < Count; i++)
            {
                if (ddlCurrency.SelectedValue == salesDetailList[0].SellingLocalCurrencyId.ToString())
                {
                    salesAmount = salesAmount + Convert.ToDecimal(salesDetailList[i].TotalPrice);
                }
                else
                {
                    salesAmount = salesAmount + Convert.ToDecimal(salesDetailList[i].TotalPrice);
                }
            }
            return salesAmount;
        }
        private void ClearDetailPart()
        {
            //   this.btnOwnerDetails.Text = "Add";
            this.ddlProductId.SelectedValue = "0";
            this.ddlServiceId.SelectedValue = "0";
            this.ddlServiceBundleId.SelectedValue = "0";
            this.ddlServiceType.SelectedIndex = 0;
            this.txtUnit.Text = "1";
            this.lblHiddenOwnerDetailtId.Text = string.Empty;
        }
        private void ClearAdditionlServiceInformation()
        {
            //Site Info
            ddlSiteInformation.SelectedIndex = 0;
            txtSiteName.Text = string.Empty;
            txtSiteAddress.Text = string.Empty;
            txtSiteContactPerson.Text = string.Empty;
            txtSitePhoneNumber.Text = string.Empty;
            txtSiteEmail.Text = string.Empty;
            //Technical Info
            ddlBillingInformarion.SelectedIndex = 0;
            txtBillingContactPerson.Text = string.Empty;
            txtBillingPersonDepartment.Text = string.Empty;
            txtBillingPersonDesignation.Text = string.Empty;
            txtBillingPersonPhone.Text = string.Empty;
            txtBillingPersonEmail.Text = string.Empty;
            // Billing Info

            ddlTechnicalInformation.SelectedIndex = 0;
            txtTechnicalContactPerson.Text = string.Empty;
            txtTechnicalPersonDepartment.Text = string.Empty;
            txtTechnicalPersonDesignation.Text = string.Empty;
            txtTechnicalPersonPhone.Text = string.Empty;
            txtTechnicalPersonEmail.Text = string.Empty;

        }
        private void FillForm(int EditId)
        {
            lblMessage.Text = "";
            this.LoadCustomer();
            //Master Information------------------------
            PMSalesBO salesBO = new PMSalesBO();
            PMSalesDetailsDA salesDA = new PMSalesDetailsDA();
            salesBO = salesDA.GetSalesInformationBySalesId(EditId);
            Session["_salesId"] = salesBO.SalesId;
            this.ddlCustomerId.SelectedValue = salesBO.CustomerId.ToString();
            txtHiddenSalesId.Value = salesBO.SalesId.ToString();
            ddlFrequency.SelectedValue = salesBO.Frequency;
            if (salesBO.BillingInfoId > 0 || salesBO.TechnicalInfoId > 0 || salesBO.SiteInfoId > 0)
            {
                this.LoadBillingInformationById(salesBO.BillingInfoId);
                this.LoadSiteInformationById(salesBO.SiteInfoId);
                this.LoadTechnicalInformationById(salesBO.TechnicalInfoId);
                chkIsSiteEnable.Checked = true;
            }
            else
            {
                chkIsSiteEnable.Checked = false;
            }

            HiddenFrequencyId.Value = "";
            SalesCustomerBO customarBO = new SalesCustomerBO();
            SalesCustomerDA customerDA = new SalesCustomerDA();
            customarBO = customerDA.GetSalesCustomerInfoByCustomerId(salesBO.CustomerId);

            txtName.Text = customarBO.Name;
            txtWebAddress.Text = customarBO.WebAddress;
            ddlCustomerType.SelectedValue = customarBO.CustomerType;
            txtPhone.Text = customarBO.Phone;
            txtEmail.Text = customarBO.Email;
            txtAddress.Text = customarBO.Address;


            this.txtRemarks.Text = salesBO.Remarks;
            this.txtSalesDate.Text = salesBO.SalesDate.ToShortDateString();
            this.txtGrandTotal.Text = salesBO.GrandTotal.ToString();
            this.txtSalesAmount.Text = salesBO.SalesAmount.ToString();
            this.txtVatAmount.Text = salesBO.VatAmount.ToString();
            this.btnSave.Text = "Update";
            //Detail Information------------------------
            List<PMSalesDetailBO> detailList = new List<PMSalesDetailBO>();
            PMSalesDetailsDA detailDA = new PMSalesDetailsDA();
            detailList = detailDA.GetPMSalesDetailsBySalesId(EditId);
            Session["PMSalesDetailList"] = detailList;
            //   this.gvRoomOwnerDtail.DataSource = Session["PMSalesDetailList"] as List<PMSalesDetailBO>;
            //   this.gvRoomOwnerDtail.DataBind();
        }
        private List<PMSalesDetailBO> GenerateDetailData(List<PMSalesDetailBO> List)
        {
            PMSalesDetailsDA salesDetailsDA = new PMSalesDetailsDA();
            PMSalesDetailBO salesDetailsBO = new PMSalesDetailBO();
            int count = List.Count;
            for (int i = 0; i < count; i++)
            {

                if (List[i].ServiceType == "Product")
                {
                    List[i].ItemId = Int32.Parse(ddlProductId.SelectedValue);
                    List[i].ItemName = this.ddlProductId.SelectedItem.Text;
                    salesDetailsBO = salesDetailsDA.GetSalesCurrencyInformation("PMProduct", List[i].ItemId);
                }
                else if (List[i].ServiceType == "Service")
                {
                    List[i].ItemId = Int32.Parse(ddlServiceId.SelectedValue);
                    List[i].ItemName = this.ddlServiceId.SelectedItem.Text;
                    salesDetailsBO = salesDetailsDA.GetSalesCurrencyInformation("SalesService", List[i].ItemId);
                }
                else
                {
                    List[i].ItemId = Int32.Parse(ddlServiceBundleId.SelectedValue);
                    List[i].ItemName = this.ddlServiceBundleId.SelectedItem.Text;
                    salesDetailsBO = salesDetailsDA.GetSalesCurrencyInformation("SalesServiceBundle", List[i].ItemId);
                }


                if (ddlCurrency.SelectedValue == salesDetailsBO.SellingLocalCurrencyId.ToString())
                {
                    List[i].TotalPrice = Convert.ToDecimal(salesDetailsBO.UnitPriceLocal) * Convert.ToDecimal(List[i].ItemUnit);
                    List[i].SellingLocalCurrencyId = salesDetailsBO.SellingLocalCurrencyId;
                    List[i].UnitPriceLocal = salesDetailsBO.UnitPriceLocal;
                    List[i].SellingUsdCurrencyId = 0;
                    List[i].UnitPriceUsd = 0;
                }
                else if (ddlCurrency.SelectedValue == salesDetailsBO.SellingUsdCurrencyId.ToString())
                {
                    List[i].TotalPrice = Convert.ToDecimal(salesDetailsBO.UnitPriceUsd) * Convert.ToDecimal(List[i].ItemUnit);
                    List[i].SellingLocalCurrencyId = 0;
                    List[i].UnitPriceLocal = 0;
                    List[i].SellingUsdCurrencyId = salesDetailsBO.SellingUsdCurrencyId;
                    List[i].UnitPriceUsd = salesDetailsBO.UnitPriceUsd;
                }
            }

            decimal salesAmount = this.CalculateGrandTotal(List);
            this.txtSalesAmount.Text = salesAmount.ToString();
            this.txtGrandTotal.Text = salesAmount.ToString();

            return List;
        }
        private void DeleteData(int pkId)
        {
            PMSalesDetailsDA detailsDA = new PMSalesDetailsDA();
            Boolean statusApproved = detailsDA.DeletePMSalesDetailsInfoBySalesId(pkId);
            if (statusApproved)
            {
                this.isMessageBoxEnable = 2;
                lblMessage.Text = "Delete Operation Successfull";
                this.LoadGridView();
                this.Cancel();
            }
        }
        private void GetInvoice(int salesId)
        {
            Int32 A = salesId.GetHashCode();
        }
        public static string LoadGuestPaymentDetailGridViewByWM()
        {
            string strTable = "";
            List<PMSalesBillPaymentBO> detailList = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<PMSalesBillPaymentBO>;
            if (detailList != null)
            {
                strTable += "<table style='width:100%' cellspacing='0' cellpadding='4' id='ReservationDetailGrid'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Payment Mode</th><th align='left' scope='col'>Amount</th><th align='center' scope='col'>Action</th></tr>";
                int counter = 0;
                foreach (PMSalesBillPaymentBO dr in detailList)
                {
                    counter++;
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
                    strTable += "<td align='left' style='width: 40%;'>" + dr.PaymentAmout + "</td>";
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
        private void LoadAccountHeadInfo()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            this.lblPaymentAccountHead.Text = "Payment Receive In";
            CustomFieldBO CashReceiveAccountsInfo = new CustomFieldBO();
            CashReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CashReceiveAccountsInfo");

            this.ddlCashReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CashReceiveAccountsInfo.FieldValue.ToString() + ")");
            this.ddlCashReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCashReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCashReceiveAccountsInfo.DataBind();

            CustomFieldBO CardReceiveAccountsInfo = new CustomFieldBO();
            CardReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CardReceiveAccountsInfo");
            this.ddlCardReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CardReceiveAccountsInfo.FieldValue.ToString() + ")");
            this.ddlCardReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCardReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCardReceiveAccountsInfo.DataBind();

            CustomFieldBO CompanyPaymentAccountsInfo = new CustomFieldBO();
            CompanyPaymentAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CompanyPaymentAccountsInfo");
            this.ddlCompanyPaymentAccountHead.DataSource = entityDA.GetNodeMatrixInfoByAncestorNodeId(Convert.ToInt32(CompanyPaymentAccountsInfo.FieldValue));
            this.ddlCompanyPaymentAccountHead.DataTextField = "NodeHead";
            this.ddlCompanyPaymentAccountHead.DataValueField = "NodeId";
            this.ddlCompanyPaymentAccountHead.DataBind();

        }
        private void LoadBank()
        {
            BankDA bankDA = new BankDA();
            var DataSource = bankDA.GetBankInfo();
            this.ddlBankName.DataSource = DataSource;
            this.ddlBankName.DataTextField = "BankName";
            this.ddlBankName.DataValueField = "BankId";
            this.ddlBankName.DataBind();

            this.ddlBankId.DataSource = DataSource;
            this.ddlBankId.DataTextField = "BankName";
            this.ddlBankId.DataValueField = "BankId";
            this.ddlBankId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBankName.Items.Insert(0, itemBank);
            this.ddlBankId.Items.Insert(0, itemBank);
        }
        private void LoadProductCategory()
        {
            List<InvCategoryBO> categoryList = new List<InvCategoryBO>();
            InvCategoryDA categoryDA = new InvCategoryDA();
            categoryList = categoryDA.GetInvCatagoryInfo();
            ddlProductCategory.DataSource = categoryList;
            this.ddlProductCategory.DataTextField = "Name";
            this.ddlProductCategory.DataValueField = "CategoryId";
            this.ddlProductCategory.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlProductCategory.Items.Insert(0, item);
        }
        private void LoadProductManufacturer()
        {
            List<InvManufacturerBO> manufacturerList = new List<InvManufacturerBO>();
            InvManufacturerDA manufacturerDA = new InvManufacturerDA();
            manufacturerList = manufacturerDA.GetManufacturerInfo();
            ddlManufacturer.DataSource = manufacturerList;
            this.ddlManufacturer.DataTextField = "Name";
            this.ddlManufacturer.DataValueField = "ManufacturerId";
            this.ddlManufacturer.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlManufacturer.Items.Insert(0, item);
        }
        private void LoadCustomerName()
        {
            List<SalesCustomerBO> customerList = new List<SalesCustomerBO>();
            SalesCustomerDA detailsDA = new SalesCustomerDA();
            customerList = detailsDA.GetDistinctSalesCustomerInfo();
            ddlCustomerName.DataSource = customerList;
            ddlCustomerName.DataTextField = "Name";
            ddlCustomerName.DataValueField = "CustomerId";
            ddlCustomerName.DataBind();

            ListItem customerName = new ListItem();
            customerName.Value = "0";
            customerName.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCustomerName.Items.Insert(0, customerName);
        }
        public static int GetIntFrequency(string frequency)
        {
            int freq = 1;

            if (frequency == "Monthly")
            {
                freq = 1;
            }
            else if (frequency == "Quaterly")
            {
                freq = 3;
            }
            else if (frequency == "Half Yearly")
            {
                freq = 6;
            }
            else if (frequency == "Yearly")
            {
                freq = 12;
            }
            return freq;
        }
        public static string GetProductDetailsGridView(List<PMSalesDetailBO> dataSource)
        {
            string inventorySeparate = GetSalesInvetorySeparateConfiguration();
            string strTable = "";
            if (dataSource != null)
            {
                strTable += "<table style='width:100%' cellspacing='0' cellpadding='4' id='ProductDetailGrid'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                if (inventorySeparate == "Yes")
                {
                    strTable += "<th align='left' scope='col'>Item Name</th><th align='left' scope='col'>Service Type</th><th align='left' scope='col'>Item Unit</th><th align='left' scope='col'>Total Price</th><th align='center' scope='col'>Action</th></tr>";

                }
                else
                {
                    strTable += "<th align='left' scope='col'>Item Name</th><th align='left' scope='col'>Serial Number</th><th align='left' scope='col'>Service Type</th><th align='left' scope='col'>Item Unit</th><th align='left' scope='col'>Total Price</th><th align='center' scope='col'>Action</th></tr>";

                }
                int counter = 0;
                foreach (PMSalesDetailBO dr in dataSource)
                {
                    counter++;
                    if (counter % 2 == 0)
                    {
                        // It's even
                        strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 20%;'>" + dr.ItemName + "</td>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:White;'><td align='left' style='width: 25%;'>" + dr.ItemName + "</td>";
                    }


                    if (inventorySeparate == "Yes")
                    {
                        strTable += "<td align='left' style='width: 20%;'>" + dr.ServiceType + "</td>";
                        strTable += "<td align='left' style='width: 20%;'>" + dr.ItemUnit + "</td>";
                        strTable += "<td align='left' style='width: 20%;'>" + dr.TotalPrice + "</td>";
                    }
                    else
                    {
                        strTable += "<td align='left' style='width: 15%;'>" + dr.SerialNumber + "</td>";
                        strTable += "<td align='left' style='width: 15%;'>" + dr.ServiceType + "</td>";
                        strTable += "<td align='left' style='width: 15%;'>" + dr.ItemUnit + "</td>";
                        strTable += "<td align='left' style='width: 15%;'>" + dr.TotalPrice + "</td>";
                    }

                    string type = "\"" + dr.ServiceType.ToString() + "\"";
                    string ItemName = "\"" + dr.ItemName.ToString() + "\"";
                    strTable += "<td align='center' style='width: 15%;'>";
                    strTable += "&nbsp;<img src='../Images/edit.png' onClick='javascript:return PerformFillFormActionWAW(" + dr.DetailId + ", " + type + ", " + ItemName + "," + dr.ItemId + "," + dr.ItemUnit + "," + dr.TotalPrice + ")' alt='Delete Information' border='0' />";
                    strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformProductDetailDelete(" + dr.DetailId + ")' alt='Delete Information' border='0' />";
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
        private static decimal GetGrandTotal(List<PMSalesDetailBO> salesDetailList, string Currency)
        {
            int Count = salesDetailList.Count;
            decimal salesAmount = 0;
            for (int i = 0; i < Count; i++)
            {
                if (Currency == salesDetailList[0].SellingLocalCurrencyId.ToString())
                {
                    salesAmount = salesAmount + Convert.ToDecimal(salesDetailList[i].TotalPrice);
                }
                else
                {
                    salesAmount = salesAmount + Convert.ToDecimal(salesDetailList[i].TotalPrice);
                }
            }
            return salesAmount;
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static ItemViewBO LoadProductDataByCriteria(string frequencyId, string serviceType, int product)
        {
            ItemViewBO viewBO = new ItemViewBO();
            if (serviceType == "Product")
            {
                InvItemDA productDA = new InvItemDA();
                InvItemBO productBO = new InvItemBO();
                productBO = productDA.GetInvItemInfoById(0, product);
                viewBO.ItemId = productBO.ItemId;
                viewBO.ItemName = productBO.Name;
                viewBO.UnitPriceLocal = productBO.UnitPriceLocal;
                viewBO.UnitPriceUsd = productBO.UnitPriceUsd;
                viewBO.ProductType = productBO.ProductType;
            }
            else if (serviceType == "Service")
            {
                SalesServiceDA serviceDA = new SalesServiceDA();
                SalesServiceBO serviceBO = new SalesServiceBO();
                serviceBO = serviceDA.GetSalesServiceInfoByServiceId(product);
                viewBO.ItemId = serviceBO.ServiceId;
                viewBO.ItemName = serviceBO.Name;
                viewBO.UnitPriceLocal = serviceBO.UnitPriceLocal;
                viewBO.UnitPriceUsd = serviceBO.UnitPriceUsd;
                viewBO.ProductType = "";
            }
            else
            {
                SalesServiceBundleDetailsDA bundleDA = new SalesServiceBundleDetailsDA();
                SalesServiceBundleBO bundleBO = new SalesServiceBundleBO();
                bundleBO = bundleDA.GetSalesServiceBundleInfoByBundleId(product);
                viewBO.ItemId = bundleBO.BundleId;
                viewBO.ItemName = bundleBO.BundleName;
                viewBO.UnitPriceLocal = bundleBO.UnitPriceLocal;
                viewBO.UnitPriceUsd = bundleBO.UnitPriceUsd;
                viewBO.ProductType = "";
            }

            return viewBO;
        }
        [WebMethod]
        public static List<ItemViewBO> GetServiceByCriteria(string frequencyId, string serviceType)
        {
            List<ItemViewBO> List = new List<ItemViewBO>();

            if (serviceType == "Product")
            {
                InvItemDA productDA = new InvItemDA();
                var list = productDA.GetInvItemInfo();
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    ItemViewBO viewBO = new ItemViewBO();
                    viewBO.ItemId = list[i].ItemId;
                    viewBO.ItemName = list[i].Name;
                    List.Add(viewBO);
                }
            }
            else if (serviceType == "Service")
            {
                SalesServiceDA serviceDA = new SalesServiceDA();

                var list = serviceDA.GetSaleServicInfoByFrequency(frequencyId);
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    ItemViewBO viewBO = new ItemViewBO();
                    viewBO.ItemId = list[i].ServiceId;
                    viewBO.ItemName = list[i].Name;
                    List.Add(viewBO);
                }
            }
            else
            {
                SalesServiceBundleDetailsDA bundleDA = new SalesServiceBundleDetailsDA();
                var list = bundleDA.GetSalesServiceBundleInfoByFrequency(frequencyId);
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    ItemViewBO viewBO = new ItemViewBO();
                    viewBO.ItemId = list[i].BundleId;
                    viewBO.ItemName = list[i].BundleName;
                    List.Add(viewBO);
                }

            }
            return List;
        }
        [WebMethod(EnableSession = true)]
        public static string PerformSaveGuestPaymentDetailsInformationByWebMethod(bool isEdit, string ddlPayMode, string txtReceiveLeadgerAmount, string ddlCashPaymentAccountHead, string txtCardNumber, string ddlCardType, string txtExpireDate, string txtCardHolderName, string txtChecqueNumber, string ddlBankId, string ddlCompanyPaymentAccountHead, string ddlCreditAccountHead)
        {

            int dynamicDetailId = 0;
            List<PMSalesBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<PMSalesBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<PMSalesBillPaymentBO>;

            PMSalesBillPaymentBO singleEntityBOEdit = guestPaymentDetailListForGrid.Where(x => x.PaymentMode == ddlPayMode).FirstOrDefault();
            if (guestPaymentDetailListForGrid.Contains(singleEntityBOEdit))
            {
                guestPaymentDetailListForGrid.Remove(singleEntityBOEdit);
            }

            if (guestPaymentDetailListForGrid != null)
            {
                dynamicDetailId = guestPaymentDetailListForGrid.Count + 1;
            }

            PMSalesBillPaymentBO guestBillPaymentBO = new PMSalesBillPaymentBO();
            if (ddlPayMode == "Company")
            {
                guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCompanyPaymentAccountHead);
                guestBillPaymentBO.PaymentType = ddlPayMode;
            }
            else if (ddlPayMode == "Other Room")
            {
                guestBillPaymentBO.NodeId = Convert.ToInt32(45);
                guestBillPaymentBO.PaymentType = ddlPayMode;

            }
            else if (ddlPayMode == "Credit")
            {
                guestBillPaymentBO.PaymentType = ddlPayMode;
                guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCreditAccountHead);
            }
            else
            {
                guestBillPaymentBO.NodeId = Convert.ToInt32(45);
                guestBillPaymentBO.PaymentType = "Advance";
            }

            guestBillPaymentBO.FieldId = 45; // Convert.ToInt32(ddlCurrency);

            //ddlPayMode, txtReceiveLeadgerAmount, ddlCashReceiveAccountsInfo, , , txtExpireDate,, ddlCompanyPaymentAccountHead

            HMUtility hmUtility = new HMUtility();
            guestBillPaymentBO.ConvertionRate = 1;
            guestBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            guestBillPaymentBO.PaymentAmout = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            //}

            if (!string.IsNullOrEmpty(txtExpireDate))
            {
                guestBillPaymentBO.ExpireDate = hmUtility.GetDateTimeFromString(txtExpireDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            guestBillPaymentBO.ChecqueDate = DateTime.Now;
            guestBillPaymentBO.PaymentMode = ddlPayMode;
            guestBillPaymentBO.PaymentId = dynamicDetailId;
            guestBillPaymentBO.CardNumber = txtCardNumber;
            guestBillPaymentBO.CardType = ddlCardType;
            guestBillPaymentBO.ChecqueNumber = txtChecqueNumber;
            //guestBillPaymentBO.PayMode = Int32.Parse(ddlPayMode);
            guestBillPaymentBO.CardHolderName = txtCardHolderName;
            guestBillPaymentBO.BankId = Int32.Parse(ddlBankId);

            guestPaymentDetailListForGrid.Add(guestBillPaymentBO);
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;
            return LoadGuestPaymentDetailGridViewByWM();
        }
        [WebMethod(EnableSession = true)]
        public static string PerformDeleteGuestPaymentByWebMethod(int paymentId)
        {
            List<PMSalesBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<PMSalesBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<PMSalesBillPaymentBO>;
            PMSalesBillPaymentBO singleEntityBOEdit = guestPaymentDetailListForGrid.Where(x => x.PaymentId == paymentId).FirstOrDefault();
            if (guestPaymentDetailListForGrid.Contains(singleEntityBOEdit))
            {
                guestPaymentDetailListForGrid.Remove(singleEntityBOEdit);
            }
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;
            HttpContext.Current.Session["CompanyPaymentRoomIdList"] = null;
            HttpContext.Current.Session["CompanyPaymentServiceIdList"] = null;

            return LoadGuestPaymentDetailGridViewByWM();
        }
        [WebMethod(EnableSession = true)]
        public static string PerformGetTotalPaidAmountByWebMethod()
        {
            decimal sum = 0;
            var List = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<PMSalesBillPaymentBO>;
            if (List != null)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    sum = sum + List[i].PaymentAmout;
                }
            }
            return sum.ToString();
        }
        [WebMethod(EnableSession = true)]
        public static SalesCustomerBO LoadCustomerInformation(int CustomerId)
        {
            SalesCustomerBO customerBO = new SalesCustomerBO();
            SalesCustomerDA customerDA = new SalesCustomerDA();
            customerBO = customerDA.GetSalesCustomerInfoByCustomerId(CustomerId);
            return customerBO;
        }
        [WebMethod(EnableSession = true)]
        public static string SaveProductDetails(string ServiceType, string ProductId, string UnitPriceLocal, string Unit, string detailId, string HiddenProductId, string HiddenProductName, string Currency, string UnitPriceUSD, string isSeparateSalesInventory, string serialNumber)
        {
            int dynamicDetailId = 0;

            PMSalesDetailsDA salesDetailsDA = new PMSalesDetailsDA();
            PMSalesDetailBO salesDetailsBO = new PMSalesDetailBO();

            List<PMSalesDetailBO> detailListBO = HttpContext.Current.Session["PMSalesDetailList"] == null ? new List<PMSalesDetailBO>() : HttpContext.Current.Session["PMSalesDetailList"] as List<PMSalesDetailBO>;
            if (!string.IsNullOrWhiteSpace(detailId))
                dynamicDetailId = Convert.ToInt32(detailId);
            PMSalesDetailBO detailBO = dynamicDetailId == 0 ? new PMSalesDetailBO() : detailListBO.Where(x => x.DetailId == dynamicDetailId).FirstOrDefault();
            if (detailListBO.Contains(detailBO))
                detailListBO.Remove(detailBO);
            detailBO.ServiceType = ServiceType.ToString();
            detailBO.ItemUnit = Convert.ToDecimal(Unit.ToString());
            int productId = Int32.Parse(HiddenProductId);
            string ItemName = HiddenProductName.ToString();

            if (!string.IsNullOrWhiteSpace(isSeparateSalesInventory))
            {
                if (isSeparateSalesInventory == "Yes")
                {
                    detailBO.IsSeparateSalesInventory = 0;
                    detailBO.SerialNumber = string.Empty;
                }
                else
                {
                    detailBO.IsSeparateSalesInventory = 1;
                    detailBO.SerialNumber = serialNumber;
                   // AddProductOutInformation(0, "0", Int32.Parse(ProductId), HiddenProductName, serialNumber, Unit, 1, "Product Out From Sales Form");
                }
            }
            
            if (detailBO.ServiceType == "Product")
            {
                detailBO.ItemId = productId;
                detailBO.ItemName = ItemName;
                salesDetailsBO = salesDetailsDA.GetSalesCurrencyInformation("PMProduct", detailBO.ItemId);
            }
            else if (detailBO.ServiceType == "Service")
            {
                detailBO.ItemId = productId;
                detailBO.ItemName = ItemName;
                salesDetailsBO = salesDetailsDA.GetSalesCurrencyInformation("SalesService", detailBO.ItemId);
            }
            else
            {
                detailBO.ItemId = productId;
                detailBO.ItemName = ItemName;
                salesDetailsBO = salesDetailsDA.GetSalesCurrencyInformation("SalesServiceBundle", detailBO.ItemId);
            }

            if (Currency == salesDetailsBO.SellingLocalCurrencyId.ToString())
            {
                detailBO.TotalPrice = Convert.ToDecimal(UnitPriceLocal) * Convert.ToDecimal(Unit);
                detailBO.SellingLocalCurrencyId = salesDetailsBO.SellingLocalCurrencyId;
                detailBO.UnitPriceLocal = Convert.ToDecimal(UnitPriceLocal);
                detailBO.SellingUsdCurrencyId = 0;
                detailBO.UnitPriceUsd = 0;
            }
            else if (Currency == salesDetailsBO.SellingUsdCurrencyId.ToString())
            {
                detailBO.TotalPrice = Convert.ToDecimal(UnitPriceUSD) * Convert.ToDecimal(Unit);
                detailBO.SellingLocalCurrencyId = 0;
                detailBO.UnitPriceLocal = 0;
                detailBO.SellingUsdCurrencyId = salesDetailsBO.SellingUsdCurrencyId;
                detailBO.UnitPriceUsd = Convert.ToDecimal(UnitPriceUSD);
            }
            detailBO.DetailId = dynamicDetailId == 0 ? detailListBO.Count + 1 : dynamicDetailId;
            detailListBO.Add(detailBO);
            HttpContext.Current.Session["PMSalesDetailList"] = detailListBO;
            
            var dataSource = HttpContext.Current.Session["PMSalesDetailList"] as List<PMSalesDetailBO>;
            return GetProductDetailsGridView(dataSource);
        }
        [WebMethod(EnableSession = true)]
        public static string DeleteProductDetails(string detailsId)
        {
            int _ownerDetailId = Convert.ToInt32(detailsId);

            var ownerDetailBO = (List<PMSalesDetailBO>)HttpContext.Current.Session["PMSalesDetailList"];
            var ownerDetail = ownerDetailBO.Where(x => x.DetailId == _ownerDetailId).FirstOrDefault();
            ownerDetailBO.Remove(ownerDetail);


            var detailBO = (List<PMSalesDetailBO>)HttpContext.Current.Session["DeletedPMSalesDetailList"];
            detailBO.Add(ownerDetail);

            HttpContext.Current.Session["PMSalesDetailList"] = ownerDetailBO;
            ArrayList list = new ArrayList();
            list = HttpContext.Current.Session["arrayDelete"] == null ? new ArrayList() : HttpContext.Current.Session["arrayDelete"] as ArrayList;
            list.Add(_ownerDetailId);
            HttpContext.Current.Session["arrayDelete"] = list as ArrayList;

            var dataSource = HttpContext.Current.Session["PMSalesDetailList"] as List<PMSalesDetailBO>;
            return GetProductDetailsGridView(dataSource);
        }
        [WebMethod(EnableSession = true)]
        public static string GetCalculatedGrandTotal(string Currency)
        {
            var dataSource = HttpContext.Current.Session["PMSalesDetailList"] as List<PMSalesDetailBO>;
            decimal salesAmount = GetGrandTotal(dataSource, Currency);
            return salesAmount.ToString();
        }
        [WebMethod(EnableSession = true)]
        public static string LoadSalesDetailGridView(string salesId)
        {
            List<PMSalesDetailBO> detailList = new List<PMSalesDetailBO>();
            PMSalesDetailsDA detailDA = new PMSalesDetailsDA();
            detailList = detailDA.GetPMSalesDetailsBySalesId(Int32.Parse(salesId));
            HttpContext.Current.Session["PMSalesDetailList"] = detailList;
            var dataSource = HttpContext.Current.Session["PMSalesDetailList"] as List<PMSalesDetailBO>;
            return GetProductDetailsGridView(dataSource);
        }
        [WebMethod(EnableSession = true)]
        public static List<InvManufacturerBO> LoadProductManufacturerByService(string SeviceType)
        {
            List<InvManufacturerBO> manufacturerList = new List<InvManufacturerBO>();
            InvManufacturerDA manufacturerDA = new InvManufacturerDA();
            manufacturerList = manufacturerDA.GetManufacturerInfoByServiceType(SeviceType);

            return manufacturerList;
        }
        [WebMethod(EnableSession = true)]
        public static List<InvCategoryBO> LoadProductCategoryByService(string SeviceType)
        {
            List<InvCategoryBO> categoryList = new List<InvCategoryBO>();
            InvCategoryDA categoryDA = new InvCategoryDA();
            categoryList = categoryDA.GetInvCategoryByService(SeviceType);
            return categoryList;
        }
        [WebMethod(EnableSession = true)]
        public static List<ItemViewBO> LoadProductInformationByCategoryAndManufacturer(string serviceType, string CategoryId, string manufacturerId)
        {

            List<ItemViewBO> List = new List<ItemViewBO>();

            if (serviceType == "Product")
            {
                InvItemDA productDA = new InvItemDA();
                var list = productDA.GetInvItemInfoByCategoryIdAndmanufacturerId(Int32.Parse(CategoryId), Int32.Parse(manufacturerId));
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    ItemViewBO viewBO = new ItemViewBO();
                    viewBO.ItemId = list[i].ItemId;
                    viewBO.ItemName = list[i].Name;
                    viewBO.Code = list[i].Code;
                    List.Add(viewBO);
                }
            }
            else if (serviceType == "Service")
            {
                SalesServiceDA serviceDA = new SalesServiceDA();

                var list = serviceDA.GetSaleServicInfoByCategoryId(Int32.Parse(CategoryId));
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    ItemViewBO viewBO = new ItemViewBO();
                    viewBO.ItemId = list[i].ServiceId;
                    viewBO.ItemName = list[i].Name;
                    viewBO.Code = list[i].Code;
                    List.Add(viewBO);
                }
            }
            else
            {
                SalesServiceBundleDetailsDA bundleDA = new SalesServiceBundleDetailsDA();
                var list = bundleDA.GetSalesServiceBundleInfo();
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    ItemViewBO viewBO = new ItemViewBO();
                    viewBO.ItemId = list[i].BundleId;
                    viewBO.ItemName = list[i].BundleName;
                    viewBO.Code = list[i].BundleCode;
                    List.Add(viewBO);
                }
            }
            return List;
        }
        [WebMethod(EnableSession = true)]
        public static int IsEnableSiteInformationCheckBox()
        {
            int isTrue = 0;
            var detailsList = HttpContext.Current.Session["PMSalesDetailList"] as List<PMSalesDetailBO>;
            if (detailsList != null)
            {
                for (int i = 0; i < detailsList.Count; i++)
                {
                    if (detailsList[i].ServiceType == "Service")
                    {
                        isTrue = 1;
                        break;
                    }
                }
            }
            return isTrue;
        }
        [WebMethod(EnableSession = true)]
        public static PMSalesSiteInfoBO GetSiteInformation(int siteId)
        {
            PMSalesSiteInfoDA siteDA = new PMSalesSiteInfoDA();
            return siteDA.GetAllPMSalesSiteInfoBySiteInfoId(siteId);
        }
        [WebMethod(EnableSession = true)]
        public static PMSalesTechnicalInfoBO GetTechnicalInformation(int technicalId)
        {
            PMSalesTechnicalInfoDA technicalDA = new PMSalesTechnicalInfoDA();
            return technicalDA.GetPMSalesTechnicalInfoByTechnicalInfoId(technicalId);
        }
        [WebMethod(EnableSession = true)]
        public static PMSalesBillingInfoBO GetBillingInformarion(int billingId)
        {
            PMSalesBillingInfoDA billingDA = new PMSalesBillingInfoDA();
            return billingDA.GetAllPMSalesBillingInfoBillingInfoId(billingId);
        }
        [WebMethod(EnableSession = true)]
        public static List<ItemViewBO> LoadSiteInformationDropDown(int customerId)
        {
            List<ItemViewBO> list = new List<ItemViewBO>();
            ItemViewBO viewBO = new ItemViewBO();
            viewBO.ItemId = 0;
            viewBO.ItemName = "New Site Information";
            viewBO.Code = "New Site Information";
            list.Add(viewBO);
            if (customerId != 0)
            {
                PMSalesSiteInfoDA siteDA = new PMSalesSiteInfoDA();
                var siteList = siteDA.GetAllPMSalesSiteInfo();
                var siteListByCustomer = siteList.Where(c => c.CustomerId == customerId);

                int count = siteListByCustomer.ToList().Count;
                var finalList = siteListByCustomer.ToList();
                for (int i = 0; i < count; i++)
                {
                    ItemViewBO itemBO = new ItemViewBO();
                    itemBO.ItemId = finalList[i].SiteInfoId;
                    itemBO.ItemName = finalList[i].SiteName;
                    itemBO.Code = finalList[i].SiteName;
                    list.Add(itemBO);
                }
            }

            return list;

        }
        [WebMethod(EnableSession = true)]
        public static List<ItemViewBO> LoadTechnicalInformationDropDown(int customerId)
        {
            List<ItemViewBO> list = new List<ItemViewBO>();
            ItemViewBO viewBO = new ItemViewBO();
            viewBO.ItemId = 0;
            viewBO.ItemName = "New Information";
            viewBO.Code = "New Information";
            list.Add(viewBO);

            if (customerId != 0)
            {
                PMSalesTechnicalInfoDA technicalDA = new PMSalesTechnicalInfoDA();
                var technicalList = technicalDA.GetAllPMSalesTechnicalInfo();
                var technicalListByCustomer = technicalList.Where(c => c.CustomerId == customerId);

                int count = technicalListByCustomer.ToList().Count;
                var finalList = technicalListByCustomer.ToList();

                for (int i = 0; i < count; i++)
                {
                    ItemViewBO itemBO = new ItemViewBO();
                    itemBO.ItemId = finalList[i].TechnicalInfoId;
                    itemBO.ItemName = finalList[i].TechnicalContactPerson;
                    itemBO.Code = finalList[i].TechnicalContactPerson;
                    list.Add(itemBO);
                }
            }

            return list;

        }
        [WebMethod(EnableSession = true)]
        public static List<ItemViewBO> LoadBillingInformationDropDown(int customerId)
        {
            List<ItemViewBO> list = new List<ItemViewBO>();
            ItemViewBO viewBO = new ItemViewBO();
            viewBO.ItemId = 0;
            viewBO.ItemName = "New Information";
            viewBO.Code = "New Information";
            list.Add(viewBO);

            if (customerId != 0)
            {
                PMSalesBillingInfoDA billingDA = new PMSalesBillingInfoDA();
                var billingList = billingDA.GetAllPMSalesBillingInfo();
                var billingListByCustomer = billingList.Where(m => m.CustomerId == customerId);

                int count = billingListByCustomer.ToList().Count;
                var finalList = billingListByCustomer.ToList();

                for (int i = 0; i < count; i++)
                {
                    ItemViewBO itemBO = new ItemViewBO();
                    itemBO.ItemId = finalList[i].BillingInfoId;
                    itemBO.ItemName = finalList[i].BillingContactPerson;
                    itemBO.Code = finalList[i].BillingContactPerson;
                    list.Add(itemBO);
                }
            }

            return list;
        }
        [WebMethod(EnableSession = true)]
        public static string LoadServiceQuantityInformation(string frequency, int serviceId)
        {
            string Quantity = string.Empty;
            SalesServiceBO serviceBO = new SalesServiceBO();
            SalesServiceDA serviceDA = new SalesServiceDA();
            serviceBO = serviceDA.GetSalesServiceInfoByServiceId(serviceId);
            if (serviceBO.Frequency == "One Time")
            {
                Quantity = "1";
            }
            else
            {
                Quantity = GetIntFrequency(frequency.ToString()).ToString();
            }
            return Quantity;
        }


        [WebMethod]
        public static string ValidateSerialNumber(int productId, string Quantity_Serial)
        {
            string tmpSerialId = string.Empty;
            PMProductSerialInfoDA entityDA = new PMProductSerialInfoDA();
            PMProductSerialInfoBO entityBO = new PMProductSerialInfoBO();
            entityBO = entityDA.GetPMProductSerialInfoBySerialNumberForSale(productId, Quantity_Serial);
            if (entityBO != null)
            {
                tmpSerialId = entityBO.SerialId.ToString();
            }

            var ownerDetailBO = (List<PMSalesDetailBO>)HttpContext.Current.Session["PMSalesDetailList"];

            if (ownerDetailBO != null)
            {
                var ownerDetail = ownerDetailBO.Where(x => x.SerialNumber == Quantity_Serial && x.SerialNumber != "").FirstOrDefault();

                if (ownerDetail.SerialNumber != "" )
                {
                    tmpSerialId = "";
                }
            }

            return tmpSerialId;
        }

        [WebMethod]
        public static string IsProductSerializable(int itemId)
        {
            InvItemDA productDA = new InvItemDA();
            InvItemBO productBO = new InvItemBO();

            int isSerialProduct = 0;

            productBO = productDA.GetInvItemInfoById(0, itemId);

            if (productBO.ProductType == "Serial Product")
            {
                isSerialProduct = 1;
            }
            return isSerialProduct.ToString();
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }

    }
}