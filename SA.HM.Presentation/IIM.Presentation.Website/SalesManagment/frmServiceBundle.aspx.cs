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
using HotelManagement.Entity.Inventory;
using System.Web.Services;


namespace HotelManagement.Presentation.Website.Restaurant
{
    public partial class frmServiceBundle : System.Web.UI.Page
    {
        ArrayList arrayDelete;
        protected int _RestaurantComboId;
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
                this.txtDetailsId.Value = "";
                this.LoadProductNServiceItem();
                this.LoadServiceItem();
                this.LoadCurrency();
                Session["BundleDetailList"] = null;
                txtBundleId.Value = "";
                this.ddlProductId.Visible = true;
                this.LoadCommonDropDownHiddenField();
                // this.ddlServiceId.Visible = false;
                this.LoadCurrencySetup();

            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
        }
        protected void gvRoomOwnerDtail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //  this.gvRoomOwnerDtail.PageIndex = e.NewPageIndex;
        }
        protected void gvRoomOwnerDtail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _ownerDetailId;

            if (e.CommandName == "CmdEdit")
            {
                _ownerDetailId = Convert.ToInt32(e.CommandArgument.ToString());
                lblHiddenOwnerDetailtId.Text = _ownerDetailId.ToString();
                var ownerDetailBO = (List<SalesServiceBundleDetailsBO>)Session["BundleDetailList"];
                var ownerDetail = ownerDetailBO.Where(x => x.DetailsId == _ownerDetailId).FirstOrDefault();
                if (ownerDetail != null && ownerDetail.DetailsId > 0)
                {
                    this.txtQuantity.Text = ownerDetail.Quantity.ToString();
                    this.ddlIsProductOrService.SelectedValue = ownerDetail.IsProductOrService;

                    if (ownerDetail.IsProductOrService == "Product") { IsService = -1; }
                    else { IsService = 1; }
                    this.ddlProductId.SelectedValue = ownerDetail.ProductId.ToString();
                    //    this.ddlServiceId.SelectedValue = ownerDetail.ServiceId.ToString();

                    //btnOwnerDetails.Text = "Edit";
                }
                else
                {
                    // btnOwnerDetails.Text = "Add";
                }
                SetTab("Entry");
            }
            else if (e.CommandName == "CmdDelete")
            {
                _ownerDetailId = Convert.ToInt32(e.CommandArgument.ToString());
                var ownerDetailBO = (List<SalesServiceBundleDetailsBO>)Session["BundleDetailList"];
                var ownerDetail = ownerDetailBO.Where(x => x.DetailsId == _ownerDetailId).FirstOrDefault();
                ownerDetailBO.Remove(ownerDetail);
                Session["BundleDetailList"] = ownerDetailBO;
                arrayDelete.Add(_ownerDetailId);
                // this.gvRoomOwnerDtail.DataSource = Session["BundleDetailList"] as List<SalesServiceBundleDetailsBO>;
                //  this.gvRoomOwnerDtail.DataBind();
            }
        }
        protected void gvRoomOwner_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdEdit")
            {
                this._RestaurantComboId = Convert.ToInt32(e.CommandArgument.ToString());
                txtBundleId.Value = this._RestaurantComboId.ToString();
                this.FillForm(this._RestaurantComboId);
                SetTab("Entry");
            }
            else if (e.CommandName == "CmdDelete")
            {
                try
                {
                    this._RestaurantComboId = Convert.ToInt32(e.CommandArgument.ToString());
                    Session["_bundleId"] = this._RestaurantComboId;
                    txtBundleId.Value = this._RestaurantComboId.ToString();
                    this.DeleteData(this._RestaurantComboId);
                    this.Cancel();
                    this.LoadGridView();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                SetTab("Search");
            }
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
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void ddlFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadServiceItem();
        }
        protected void ddlIsProductOrService_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlIsProductOrService.SelectedIndex == 1)
            {
                this.ddlProductId.Visible = false;
                //   this.ddlServiceId.Visible = true;
                this.LoadServiceItem();
            }
            else
            {
                this.ddlProductId.Visible = true;
                //  this.ddlServiceId.Visible = false;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                return;
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            SalesServiceBundleBO bo = new SalesServiceBundleBO();
            SalesServiceBundleDetailsDA da = new SalesServiceBundleDetailsDA();
            bo.BundleName = this.txtBundleName.Text;
            bo.BundleCode = this.txtBundleCode.Text;
            bo.Frequency = this.ddlFrequency.SelectedItem.Text;
            bo.UnitPriceLocal = Convert.ToDecimal(this.txtSellingPriceLocal.Text);
            if (txtCurrencySetup.Value == "Both")
            {
                bo.UnitPriceUsd = Convert.ToDecimal(this.txtSellingPriceUsd.Text);
                bo.SellingPriceUsd = Int32.Parse(ddlSellingPriceUsd.SelectedValue);
            }
            else
            {
                bo.UnitPriceUsd = 0;
                bo.SellingPriceUsd = 0;
            }

            bo.SellingPriceLocal = Int32.Parse(ddlSellingPriceLocal.SelectedValue);



            if (this.btnSave.Text.Equals("Save"))
            {
                if (DuplicateCheckDynamicaly("BundleName", this.txtBundleName.Text, 0) == 1)
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Bundle Name Already Exist";
                    this.txtBundleName.Focus();
                    return;
                }
                else if (DuplicateCheckDynamicaly("BundleCode", this.txtBundleCode.Text, 0) == 1)
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = " Bundle Code Already Exist";
                    txtBundleCode.Focus();
                    return;
                }
                int tmpBundleId = 0;
                bo.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = da.SaveSalesServiceBundleInfo(bo, out tmpBundleId, Session["BundleDetailList"] as List<SalesServiceBundleDetailsBO>);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Saved Operation Successfull";
                    this.LoadGridView();
                    this.Cancel();
                    btnSave.Text = "Save";
                }
            }
            else
            {
                if (DuplicateCheckDynamicaly("BundleName", this.txtBundleName.Text, 1) == 1)
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Bundle Name Already Exist";
                    this.txtBundleName.Focus();
                    return;
                }
                else if (DuplicateCheckDynamicaly("BundleCode", this.txtBundleCode.Text, 1) == 1)
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = " Bundle Code Already Exist";
                    txtBundleCode.Focus();
                    return;
                }
                bo.BundleId = Convert.ToInt32(Session["_bundleId"]);
                bo.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = da.UpdateSalesServiceBundleInfo(bo, Session["BundleDetailList"] as List<SalesServiceBundleDetailsBO>, Session["arrayDelete"] as ArrayList);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Update Operation Successfull";
                    this.LoadGridView();
                    this.Cancel();
                    btnSave.Text = "Save";
                }
            }
        }
        //************************ User Defined Function ********************//h
        public void LoadCurrencySetup()
        {

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("CurrencyType", "CurrencyConfiguration");
            if (commonSetupBO.SetupId > 0)
            {
                if (commonSetupBO.SetupValue == "Single")
                {
                    txtCurrencySetup.Value = "Single";
                }
                else
                {
                    txtCurrencySetup.Value = "Both";
                }
            }
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void Cancel()
        {
            this.txtBundleName.Text = string.Empty;
            this.txtBundleCode.Text = string.Empty;
            this.ddlFrequency.SelectedIndex = 0;
            this.txtSellingPriceLocal.Text = string.Empty;
            this.txtSellingPriceUsd.Text = string.Empty;
            this.ddlSellingPriceLocal.SelectedIndex = 0;
            this.ddlSellingPriceUsd.SelectedIndex = 1;
            this.btnSave.Text = "Save";
            Session["BundleDetailList"] = null;
            Session["arrayDelete"] = null;
            //  this.gvRoomOwnerDtail.DataSource = Session["BundleDetailList"] as List<SalesServiceBundleDetailsBO>;
            //  this.gvRoomOwnerDtail.DataBind();
            this.ClearDetailPart();
            this.ddlProductId.Visible = true;
            // this.ddlServiceId.Visible = false;
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
            string Name = this.txtSName.Text.ToString();
            string Code = this.txtSCode.Text.ToString();
            this.CheckObjectPermission();
            List<SalesServiceBundleBO> list = new List<SalesServiceBundleBO>();
            SalesServiceBundleDetailsDA da = new SalesServiceBundleDetailsDA();
            list = da.GetSalesServiceBundleInfoBySearchCriteria(Name, Code);
            this.gvRoomOwner.DataSource = list;
            this.gvRoomOwner.DataBind();
            SetTab("Search");
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
        private void LoadProductNServiceItem()
        {
            InvItemDA productDA = new InvItemDA();
            this.ddlProductId.DataSource = productDA.GetInvItemNServiceInfo();
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
            /*   SalesServiceDA serviceDA = new SalesServiceDA();
               this.ddlServiceId.DataSource = serviceDA.GetSaleServicInfoByFrequency(this.ddlFrequency.SelectedItem.Text);
               this.ddlServiceId.DataTextField = "Name";
               this.ddlServiceId.DataValueField = "serviceId";
               this.ddlServiceId.DataBind();
               ListItem item = new ListItem();
               item.Value = "0";
               item.Text = hmUtility.GetDropDownFirstValue();
               this.ddlServiceId.Items.Insert(0, item);*/
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmServiceBundle.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            //   btnOwnerDetails.Visible = isSavePermission;
        }
        private bool IsDetailFormValid()
        {
            bool status = true;
            if (this.ddlIsProductOrService.SelectedIndex == 0)
            {
                if (this.ddlProductId.SelectedIndex == 0)
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Please Select Valid Product";
                    this.ddlProductId.Focus();
                    status = false;
                }
            }
            else
            {
                /* if (this.ddlServiceId.SelectedIndex == 0)
                 {
                     this.isMessageBoxEnable = 1;
                     lblMessage.Text = "Please Select Valid Service";
                     this.ddlServiceId.Focus();
                     status = false;
                 }*/
            }

            if (string.IsNullOrWhiteSpace(this.txtQuantity.Text))
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Enter Quantity";
                status = false;
            }
            return status;

        }
        private bool IsFormValid()
        {
            bool status = true;
            var detailList = Session["BundleDetailList"] as List<SalesServiceBundleDetailsBO>;

            if (detailList == null)
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Add Some Item";
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtBundleName.Text))
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Enter Combo Name";
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtBundleCode.Text))
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Enter Code";
                status = false;
            }
            else if (txtCurrencySetup.Value == "Single")
            {
                if (string.IsNullOrWhiteSpace(this.txtSellingPriceLocal.Text))
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Please Enter Local Selling Price ";
                    status = false;
                }

            }
            else if (txtCurrencySetup.Value == "Both")
            {
                if (string.IsNullOrWhiteSpace(this.txtSellingPriceLocal.Text))
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Please Enter Local Selling Price ";
                    status = false;
                }
                else if (string.IsNullOrWhiteSpace(this.txtSellingPriceUsd.Text))
                {
                    this.isMessageBoxEnable = 1;
                    lblMessage.Text = "Please Enter USD Selling Price";
                    status = false;
                }

            }


            return status;  //
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
        private void ClearDetailPart()
        {
            //   this.btnOwnerDetails.Text = "Add";
            this.ddlProductId.SelectedValue = "0";
            //this.ddlServiceId.SelectedValue = "0";
            this.txtQuantity.Text = string.Empty;
            this.lblHiddenOwnerDetailtId.Text = string.Empty;
        }
        private void FillForm(int EditId)
        {
            lblMessage.Text = "";

            //Master Information------------------------
            SalesServiceBundleBO bundleBO = new SalesServiceBundleBO();
            SalesServiceBundleDetailsDA bundleDA = new SalesServiceBundleDetailsDA();

            bundleBO = bundleDA.GetSalesServiceBundleInfoByBundleId(EditId);
            Session["_bundleId"] = bundleBO.BundleId;
            txtBundleName.Text = bundleBO.BundleName;
            txtBundleCode.Text = bundleBO.BundleCode;
            ddlFrequency.SelectedValue = bundleBO.Frequency;
            this.txtSellingPriceLocal.Text = bundleBO.UnitPriceLocal.ToString();
            this.txtSellingPriceUsd.Text = bundleBO.UnitPriceUsd.ToString();
            this.ddlSellingPriceLocal.SelectedValue = bundleBO.SellingPriceLocal.ToString();
            if (txtCurrencySetup.Value != "Single")
            {
                this.ddlSellingPriceUsd.SelectedValue = bundleBO.SellingPriceUsd.ToString();
            }
            this.btnSave.Text = "Update";
            //Detail Information------------------------
            List<SalesServiceBundleDetailsBO> detailList = new List<SalesServiceBundleDetailsBO>();
            SalesServiceBundleDetailsDA detailDA = new SalesServiceBundleDetailsDA();
            detailList = detailDA.GetBundleDetailsByBundleId(EditId);
            detailList = ProcessedList(detailList);
            this.setTotal(detailList);
            Session["BundleDetailList"] = detailList;
            //  this.gvRoomOwnerDtail.DataSource = Session["BundleDetailList"] as List<SalesServiceBundleDetailsBO>;
            //  this.gvRoomOwnerDtail.DataBind();
        }
        private void DeleteData(int pkId)
        {
            SalesServiceBundleDetailsDA bundleDA = new SalesServiceBundleDetailsDA();
            Boolean statusApproved = bundleDA.DeleteServiceBundleDetailsInfoByBundleId(pkId);
            if (statusApproved)
            {
                this.isMessageBoxEnable = 2;
                lblMessage.Text = "Delete Operation Successfull";
                this.LoadGridView();
                SetTab("Search");
                this.Cancel();
            }
        }
        private void setTotal(List<SalesServiceBundleDetailsBO> salesDetailList)
        {
            int count = salesDetailList.Count;
            decimal localTotal = 0;
            decimal usdTotal = 0;
            for (int i = 0; i < count; i++)
            {
                localTotal = localTotal + salesDetailList[i].TotalUnitPriceLocal;
                usdTotal = usdTotal + salesDetailList[i].TotalUnitPriceUsd;
            }
            txtSellingPriceLocal.Text = localTotal.ToString();
            txtSellingPriceUsd.Text = usdTotal.ToString();
        }
        private List<SalesServiceBundleDetailsBO> ProcessedList(List<SalesServiceBundleDetailsBO> List)
        {
            PMSalesDetailsDA salesDetailsDA = new PMSalesDetailsDA();
            PMSalesDetailBO salesDetailsBO = new PMSalesDetailBO();
            InvItemDA productDA = new InvItemDA();
            InvItemBO productBO = new InvItemBO();
            SalesServiceBO serviceBO = new SalesServiceBO();
            SalesServiceDA serviceDA = new SalesServiceDA();

            int count = List.Count;
            for (int i = 0; i < count; i++)
            {
                if (List[i].IsProductOrService == "Product")
                {
                    productBO = productDA.GetInvItemInfoById(0, List[i].ProductId);
                    List[i].ItemName = productBO.Name;
                    salesDetailsBO = salesDetailsDA.GetSalesCurrencyInformation("PMProduct", List[i].ProductId);
                }
                else
                {
                    serviceBO = serviceDA.GetSalesServiceInfoByServiceId(List[i].ServiceId);
                    List[i].ItemName = serviceBO.Name;
                    salesDetailsBO = salesDetailsDA.GetSalesCurrencyInformation("SalesService", Int32.Parse(ddlProductId.SelectedValue));
                }

                List[i].SellingPriceLocal = salesDetailsBO.SellingLocalCurrencyId;
                List[i].SellingPriceUsd = salesDetailsBO.SellingUsdCurrencyId;
                List[i].UnitPriceLocal = salesDetailsBO.UnitPriceLocal;
                List[i].UnitPriceUsd = salesDetailsBO.UnitPriceUsd;
                List[i].TotalUnitPriceLocal = List[i].UnitPriceLocal * (Convert.ToDecimal(List[i].Quantity));
                List[i].TotalUnitPriceUsd = List[i].UnitPriceUsd * (Convert.ToDecimal(List[i].Quantity));
            }

            return List;
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "SalesServiceBundle";
            string pkFieldName = "BundleId";
            string pkFieldValue = txtBundleId.Value.ToString();
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        public static string GetBundleDetailsGridView(List<SalesServiceBundleDetailsBO> dataSource, string Currency)
        {
            string strTable = "";

            if (dataSource != null)
            {
                strTable += "<table style='width:100%' cellspacing='0' cellpadding='4' id='ProductDetailGrid'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";

                if (Currency == "Single")
                {
                    strTable += "<th align='left' scope='col'>Item Name</th><th align='left' scope='col'>Unit Price(Local)</th><th align='left' scope='col'>Quantity</th><th align='left' scope='col'>Total Price(Local) </th><th align='center' scope='col'>Action</th></tr>";
                }
                else
                {
                    strTable += "<th align='left' scope='col'>Item Name</th><th align='left' scope='col'>Unit Price(Local)</th><th align='left' scope='col'>Unit Price(USD)</th><th align='left' scope='col'>Quantity</th><th align='left' scope='col'>Total Price(Local) </th><th align='left' scope='col'>Total Price(USD)</th><th align='center' scope='col'>Action</th></tr>";
                }

                int counter = 0;
                foreach (SalesServiceBundleDetailsBO dr in dataSource)
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
                        strTable += "<tr style='background-color:White;'><td align='left' style='width: 20%;'>" + dr.ItemName + "</td>";
                    }

                    if (Currency == "Single")
                    {
                        strTable += "<td align='left' style='width: 20%;'>" + dr.UnitPriceLocal + "</td>";
                        strTable += "<td align='left' style='width: 20%;'>" + dr.Quantity + "</td>";
                        strTable += "<td align='left' style='width: 20%;'>" + dr.TotalUnitPriceLocal + "</td>";


                    }
                    else
                    {
                        strTable += "<td align='left' style='width: 15%;'>" + dr.UnitPriceLocal + "</td>";
                        strTable += "<td align='left' style='width: 15%;'>" + dr.UnitPriceUsd + "</td>";
                        strTable += "<td align='left' style='width: 15%;'>" + dr.Quantity + "</td>";
                        strTable += "<td align='left' style='width: 15%;'>" + dr.TotalUnitPriceLocal + "</td>";
                        strTable += "<td align='left' style='width: 15%;'>" + dr.TotalUnitPriceUsd + "</td>";
                    }

                    string Type = "\"" + dr.IsProductOrService + "\"";
                    string formatedCurrency = "\"" + Currency + "\"";
                    strTable += "<td align='center' style='width: 15%;'>";
                    strTable += "&nbsp;<img src='../Images/edit.png' onClick='javascript:return PerformFillBundleDetailsWAW(" + dr.DetailsId + "," + Type + ", " + dr.ServiceId + "," + dr.ProductId + "," + dr.Quantity + ")' alt='Delete Information' border='0' />";
                    strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformeleteDetailsBundleDelete(" + dr.DetailsId + "," + formatedCurrency + ")' alt='Delete Information' border='0' />";
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
        //************************ User Defined Web Method ********************//
        [WebMethod(EnableSession = true)]
        public static string SaveBundleDetailsInformation(string BundleId, string ProductOrService, string ProductId, string productName, string Quantity, string CurrencySetup, string DetailsId)
        {

            int dynamicDetailId = 0;
            PMSalesDetailsDA salesDetailsDA = new PMSalesDetailsDA();
            PMSalesDetailBO salesDetailsBO = new PMSalesDetailBO();
            List<SalesServiceBundleDetailsBO> detailListBO = HttpContext.Current.Session["BundleDetailList"] == null ? new List<SalesServiceBundleDetailsBO>() : HttpContext.Current.Session["BundleDetailList"] as List<SalesServiceBundleDetailsBO>;
            if (!string.IsNullOrWhiteSpace(DetailsId))
                dynamicDetailId = Convert.ToInt32(DetailsId);
            SalesServiceBundleDetailsBO detailBO = dynamicDetailId == 0 ? new SalesServiceBundleDetailsBO() : detailListBO.Where(x => x.DetailsId == dynamicDetailId).FirstOrDefault();
            if (detailListBO.Contains(detailBO))
                detailListBO.Remove(detailBO);
            detailBO.IsProductOrService = ProductOrService.ToString();
            detailBO.Quantity = Convert.ToDecimal(Quantity);
            if (detailBO.IsProductOrService == "Product")
            {
                detailBO.ItemName = productName;
                salesDetailsBO = salesDetailsDA.GetSalesCurrencyInformation("PMProduct", Int32.Parse(ProductId));
                detailBO.ProductId = Int32.Parse(ProductId);
                //detailBO.ServiceId = 0;
            }
            else
            {
                detailBO.ItemName = productName;
                salesDetailsBO = salesDetailsDA.GetSalesCurrencyInformation("SalesService", Int32.Parse(ProductId));
                detailBO.ProductId = Int32.Parse(ProductId);
                //detailBO.ServiceId = Int32.Parse(ProductId);
                //detailBO.ProductId = 0;
            }

            if (!string.IsNullOrEmpty(BundleId))
            {
                detailBO.BundleId = Int32.Parse(BundleId);
            }
            else
            {
                detailBO.BundleId = 0;
            }
            detailBO.DetailsId = dynamicDetailId == 0 ? detailListBO.Count + 1 : dynamicDetailId;
            detailBO.SellingPriceLocal = salesDetailsBO.SellingLocalCurrencyId;
            detailBO.SellingPriceUsd = salesDetailsBO.SellingUsdCurrencyId;
            detailBO.UnitPriceLocal = salesDetailsBO.UnitPriceLocal;
            detailBO.UnitPriceUsd = salesDetailsBO.UnitPriceUsd;
            detailBO.TotalUnitPriceLocal = detailBO.UnitPriceLocal * (Convert.ToDecimal(Quantity));
            detailBO.TotalUnitPriceUsd = detailBO.UnitPriceUsd * (Convert.ToDecimal(Quantity));

            detailListBO.Add(detailBO);

            HttpContext.Current.Session["BundleDetailList"] = detailListBO;
            List<SalesServiceBundleDetailsBO> bundleDetailList = HttpContext.Current.Session["BundleDetailList"] as List<SalesServiceBundleDetailsBO>;
            return GetBundleDetailsGridView(bundleDetailList, CurrencySetup);
            // setTotal(detailListBO);

        }
        [WebMethod(EnableSession = true)]
        public static SalesServiceBundleDetailsBO GetTotalAmount()
        {
            List<SalesServiceBundleDetailsBO> salesDetailList = HttpContext.Current.Session["BundleDetailList"] as List<SalesServiceBundleDetailsBO>;
            SalesServiceBundleDetailsBO detailsBO = new SalesServiceBundleDetailsBO();
            int count = salesDetailList.Count;
            decimal localTotal = 0;
            decimal usdTotal = 0;
            for (int i = 0; i < count; i++)
            {
                localTotal = localTotal + salesDetailList[i].TotalUnitPriceLocal;
                usdTotal = usdTotal + salesDetailList[i].TotalUnitPriceUsd;
            }
            detailsBO.TotalUnitPriceLocal = localTotal;
            detailsBO.TotalUnitPriceUsd = usdTotal;
            return detailsBO;
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
        public static string DeleteBundleDetails(string detailsId, string Currency)
        {
            int _ownerDetailId = Convert.ToInt32(detailsId);
            var ownerDetailBO = (List<SalesServiceBundleDetailsBO>)HttpContext.Current.Session["BundleDetailList"];
            var ownerDetail = ownerDetailBO.Where(x => x.DetailsId == _ownerDetailId).FirstOrDefault();
            ownerDetailBO.Remove(ownerDetail);
            HttpContext.Current.Session["BundleDetailList"] = ownerDetailBO;

            var arrayDelete = HttpContext.Current.Session["arrayDelete"] as ArrayList;
            arrayDelete.Add(_ownerDetailId);
            HttpContext.Current.Session["arrayDelete"] = arrayDelete as ArrayList;
            var dataSource = HttpContext.Current.Session["BundleDetailList"] as List<SalesServiceBundleDetailsBO>;
            return GetBundleDetailsGridView(dataSource, Currency); ;
        }
        [WebMethod(EnableSession = true)]
        public static string LoadBundleDetailGridView(string bundleId, string Currency)
        {
            List<SalesServiceBundleDetailsBO> detailList = new List<SalesServiceBundleDetailsBO>();
            SalesServiceBundleDetailsDA detailDA = new SalesServiceBundleDetailsDA();
            detailList = detailDA.GetBundleDetailsInformationByBundleId(Int32.Parse(bundleId));
            HttpContext.Current.Session["BundleDetailList"] = detailList;
            var dataSource = HttpContext.Current.Session["BundleDetailList"] as List<SalesServiceBundleDetailsBO>;
            return GetBundleDetailsGridView(dataSource, Currency);
        }
    }
}