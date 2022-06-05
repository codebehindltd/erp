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
using HotelManagement.Data.Payroll;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.SalesManagment;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmPMProductOut : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected int _RestaurantComboId;
        protected int IsService = -1;
        protected int btnPadding = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                Session["PMProductOut"] = null;
                IsPayrollIntegrateWithInventory();
                LoadProductOutFor();
                LoadBillNumber();
                LoadRequisition();
                LoadCostCenter();
                LoadStockBy();
                LoadCategory();
                LoadOutFor();
                LoadProduct();
                LoadProductInfo();
                LoadCommonDropDownHiddenField();
                txtQuantity_Serial.Attributes.Add("class", "CustomTextBox");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                return;
            }

            PMProductOutDA outDA = new PMProductOutDA();
            Boolean status = true;//outDA.SaveProductOutInfo(Session["PMProductOut"] as List<PMProductOutBO>);
            if (status == true)
            {
                CommonHelper.AlertInfo("Product Out " + AlertMessage.Success, AlertType.Success);
                this.ClearForm();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridview();
        }
        //protected void gvProductOutInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.DataItem != null)
        //    {
        //        var item = (PMProductOutBO)e.Row.DataItem;

        //        ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
        //        ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");

        //        if (item.Status != "Approved")
        //        {
        //            imgUpdate.Visible = true;
        //            imgDelete.Visible = true;
        //        }
        //        else
        //        {
        //            imgUpdate.Visible = false;
        //            imgDelete.Visible = false;
        //        }
        //    }
        //}
        //protected void gvProductOutInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {
        //        if (e.CommandName == "CmdDelete")
        //        {
        //            int outId = Convert.ToInt32(e.CommandArgument.ToString());
        //            string result = string.Empty;

        //            PMProductOutDA outDa = new PMProductOutDA();
        //            bool status = outDa.DeleteProductOutInfo(outId);

        //            if (status)
        //            {
        //                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
        //                LoadGridview();
        //            }
        //            else
        //            {
        //                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
        //            }
        //            this.SetTab("SearchTab");
        //        }
        //        if (e.CommandName == "CmdPOPreview")
        //        {
        //            string url = "/Inventory/Reports/frmReportProductOut.aspx?poOutId=" + Convert.ToInt32(e.CommandArgument.ToString());
        //            string sPopUp = "window.open('" + url + "', 'popup_window', 'width=830,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
        //            ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
        //            //this.SearchInformation();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
        //    }
        //}
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        //************************ User Defined Function ********************//
        public void LoadGridview()
        {
            this.SetTab("SearchTab");
            PMProductOutDA outDa = new PMProductOutDA();
            List<PMProductOutBO> outList = new List<PMProductOutBO>();

            DateTime? fromDate = DateTime.Now, toDate = DateTime.Now;
            int costCenterIdFrom = 0, costCenterIdTo = 0, locationIdFrom = 0, locationIdTo = 0;
            string productOutFor = string.Empty;

            if (!string.IsNullOrEmpty(ddlSearchProductOutFor.SelectedValue))
            {
                productOutFor = ddlSearchProductOutFor.SelectedValue;
            }

            if (!string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                //fromDate = Convert.ToDateTime(txtFromDate.Text);
                fromDate = CommonHelper.DateTimeToMMDDYYYY(txtFromDate.Text);
            }

            if (!string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                //toDate = Convert.ToDateTime(txtToDate.Text);
                toDate = CommonHelper.DateTimeToMMDDYYYY(txtToDate.Text);
            }

            if (ddlSearchCostCenterFrom.SelectedValue != "0")
            {
                costCenterIdFrom = Convert.ToInt32(ddlSearchCostCenterFrom.SelectedValue);
            }

            if (ddlSearchCostCenterTo.SelectedValue != "0")
            {
                costCenterIdTo = Convert.ToInt32(ddlSearchCostCenterTo.SelectedValue);
            }

            if (hfSearchLocationIdFrom.Value != "0")
            {
                locationIdFrom = Convert.ToInt32(hfSearchLocationIdFrom.Value);
            }

            if (hfSearchLocationIdTo.Value != "0")
            {
                locationIdTo = Convert.ToInt32(hfSearchLocationIdTo.Value);
            }

           // outList = outDa.GetProductOutForSearch(productOutFor, costCenterIdFrom, costCenterIdTo, locationIdFrom, locationIdTo, fromDate, toDate, string.Empty);

            //gvProductOutInfo.DataSource = outList;
            //gvProductOutInfo.DataBind();
        }
        private void IsPayrollIntegrateWithInventory()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollIntegrateWithInventory", "IsPayrollIntegrateWithInventory");
            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        IsPayrollIntegrateWithInventoryVal.Value = "0";
                        //ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("2"));
                    }
                    else
                    {
                        IsPayrollIntegrateWithInventoryVal.Value = "1";
                    }
                }
            }
        }
        private void LoadRequisition()
        {
            PMRequisitionDA entityDA = new PMRequisitionDA();
            ddlRequisition.DataSource = entityDA.GetApprovedNNotDeliveredRequisitionForOut();
            ddlRequisition.DataTextField = "PRNumber";
            ddlRequisition.DataValueField = "RequisitionId";
            ddlRequisition.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlRequisition.Items.Insert(0, item);
        }
        public void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetCostCentreTabInfo();
            this.ddlCostCenterFrom.DataSource = List;
            this.ddlCostCenterFrom.DataTextField = "CostCenter";
            this.ddlCostCenterFrom.DataValueField = "CostCenterId";
            this.ddlCostCenterFrom.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCostCenterFrom.Items.Insert(0, item);

            this.ddlCostCenterTo.DataSource = List;
            this.ddlCostCenterTo.DataTextField = "CostCenter";
            this.ddlCostCenterTo.DataValueField = "CostCenterId";
            this.ddlCostCenterTo.DataBind();
            this.ddlCostCenterTo.Items.Insert(0, item);

            this.ddlSearchCostCenterFrom.DataSource = List;
            this.ddlSearchCostCenterFrom.DataTextField = "CostCenter";
            this.ddlSearchCostCenterFrom.DataValueField = "CostCenterId";
            this.ddlSearchCostCenterFrom.DataBind();
            this.ddlSearchCostCenterFrom.Items.Insert(0, item);

            this.ddlSearchCostCenterTo.DataSource = List;
            this.ddlSearchCostCenterTo.DataTextField = "CostCenter";
            this.ddlSearchCostCenterTo.DataValueField = "CostCenterId";
            this.ddlSearchCostCenterTo.DataBind();
            this.ddlSearchCostCenterTo.Items.Insert(0, item);
        }
        private void LoadOutFor()
        {
            if (IsPayrollIntegrateWithInventoryVal.Value == "1")
            {
                EmployeeDA entityDA = new EmployeeDA();
                this.ddlOutFor.DataSource = entityDA.GetEmployeeInfo();
                this.ddlOutFor.DataTextField = "EmployeeName";
                this.ddlOutFor.DataValueField = "EmpId";
                this.ddlOutFor.DataBind();

                ListItem item = new ListItem();
                item.Value = "0";
                item.Text = hmUtility.GetDropDownFirstValue();
                this.ddlOutFor.Items.Insert(0, item);
            }
            else
            {
                ListItem item = new ListItem();
                item.Value = "0";
                item.Text = hmUtility.GetDropDownNoneValue();
                this.ddlOutFor.Items.Insert(0, item);
            }
        }
        private void LoadProductInfo()
        {
            List<InvItemBO> productList = new List<InvItemBO>();
            InvItemDA productDA = new InvItemDA();
            productList = productDA.GetInvItemInfoByCategoryId(0, Convert.ToInt32(ddlCategory.SelectedValue));

            ddlSalesOrderProduct.DataSource = productList;
            ddlSalesOrderProduct.DataTextField = "Name";
            ddlSalesOrderProduct.DataValueField = "ItemId";

            ddlSalesOrderProduct.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSalesOrderProduct.Items.Insert(0, item);

            hfProductForSales.Value = JsonConvert.SerializeObject(productList);
        }
        private void LoadBillNumber()
        {
            PMSalesDetailsDA entityDA = new PMSalesDetailsDA();
            this.ddlBillNumber.DataSource = entityDA.GetAllSalesInformation();
            this.ddlBillNumber.DataTextField = "BillNumber";
            this.ddlBillNumber.DataValueField = "SalesId";
            this.ddlBillNumber.DataBind();

            ddlBillNumber.Items.Insert(0, new ListItem(hmUtility.GetDropDownFirstValue(), "0"));
        }
        private void LoadProductOutFor()
        {
            ListItem itemR = new ListItem();
            itemR.Value = "Requisition";
            itemR.Text = "Requisition Transfer";
            ddlProductOutFor.Items.Insert(0, itemR);
            ddlSearchProductOutFor.Items.Insert(0, itemR);

            ListItem itemRe = new ListItem();
            itemRe.Value = "Receive";
            itemRe.Text = "Direct (Stock) Transfer";
            ddlProductOutFor.Items.Insert(1, itemRe);
            ddlSearchProductOutFor.Items.Insert(1, itemRe);

            ListItem itemSalesOut = new ListItem();
            itemSalesOut.Value = "Sales";
            itemSalesOut.Text = "Sales Stock Out";
            ddlProductOutFor.Items.Insert(1, itemSalesOut);
            ddlSearchProductOutFor.Items.Insert(1, itemSalesOut);

            ddlSearchProductOutFor.Items.Insert(0, new ListItem(hmUtility.GetDropDownFirstValue(), ""));

            //ListItem item = new ListItem();
            //item.Value = "Internal";
            //item.Text = "Internal";
            //ddlProductOutFor.Items.Insert(0, item);
            //ddlSearchProductOutFor.Items.Insert(0, item);

            //ListItem itemR = new ListItem();
            //itemR.Value = "Requisition";
            //itemR.Text = "Requisition";
            //ddlProductOutFor.Items.Insert(1, itemR);
            //ddlSearchProductOutFor.Items.Insert(1, itemR);

            //ListItem itemRe = new ListItem();
            //itemRe.Value = "Receive";
            //itemRe.Text = "Stock Transfer";
            //ddlProductOutFor.Items.Insert(2, itemRe);
            //ddlSearchProductOutFor.Items.Insert(2, itemRe);

            //ddlSearchProductOutFor.Items.Insert(0, new ListItem(hmUtility.GetDropDownFirstValue(), ""));
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetInvItemCatagoryInfoByServiceType("Product");
            this.ddlCategory.DataSource = List;
            this.ddlCategory.DataTextField = "MatrixInfo";
            this.ddlCategory.DataValueField = "CategoryId";
            this.ddlCategory.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCategory.Items.Insert(0, item);
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
        private void LoadProduct()
        {
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlProduct.Items.Insert(0, item);
        }
        private bool IsFormValid()
        {
            bool status = true;
            List<PMProductOutBO> detailListBO = Session["PMProductOut"] == null ? new List<PMProductOutBO>() : Session["PMProductOut"] as List<PMProductOutBO>;

            if (detailListBO == null)
            {
                status = false;
                CommonHelper.AlertInfo("Please Add Product Detail Information.", AlertType.Warning);
            }
            else if (detailListBO.Count == 0)
            {
                status = false;
                CommonHelper.AlertInfo("Please Add Product Detail Information.", AlertType.Warning);
            }

            //else if (this.ddlBillNumber.SelectedValue == "0")
            //{
            //    if (this.ddlOutFor.SelectedValue == "0")
            //    {
            //        status = false;
            //        isMessageBoxEnable = 1;
            //        lblMessage.Text = "Please Provide Out For Information";
            //        this.ddlOutFor.Focus();
            //    }
            //}

            //if (string.IsNullOrWhiteSpace(this.txtRemarks.Text))
            //{
            //    if (this.ddlBillNumber.SelectedValue == "0")
            //    {
            //        status = false;
            //        isMessageBoxEnable = 1;
            //        lblMessage.Text = "Please Provide Location Information";
            //        this.ddlBillNumber.Focus();
            //    }
            //    else
            //    {
            //        status = false;
            //        isMessageBoxEnable = 1;
            //        lblMessage.Text = "Please Provide Remarks Information";
            //        this.ddlBillNumber.Focus();
            //    }
            //}


            return status;
        }
        private void ClearForm()
        {
            Session["PMProductOut"] = null;
            Session["arrayDelete"] = null;

            this.txtQuantity_Serial.Text = "";
            this.ddlBillNumber.SelectedIndex = 0;
            this.ddlOutFor.SelectedIndex = 0;
            this.txtRemarks.Text = "";
        }
        private int ValidSerialNumber()
        {
            int tmpSerialId = 0;
            PMProductSerialInfoDA entityDA = new PMProductSerialInfoDA();
            PMProductSerialInfoBO entityBO = new PMProductSerialInfoBO();

            //entityBO = entityDA.GetPMProductSerialInfoBySerialNumberForSale(Convert.ToInt32(this.ddlProductId.SelectedValue), this.txtQuantity_Serial.Text);
            if (entityBO != null)
            {
                tmpSerialId = entityBO.SerialId;
            }
            return tmpSerialId;
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
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + " Quantity.", AlertType.Warning);
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
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
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
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<ItemViewBO> LoadProductBySalesId(int salesId)
        {
            PMSalesDetailsDA entityDA = new PMSalesDetailsDA();
            List<PMSalesDetailBO> salesDetailsList = new List<PMSalesDetailBO>();
            salesDetailsList = entityDA.GetPMSalesDetailsBySalesId(salesId);

            List<ItemViewBO> itemViewList = new List<ItemViewBO>();

            itemViewList = (from s in salesDetailsList
                            select new ItemViewBO
                            {
                                ItemId = s.ItemId,
                                ItemName = s.ItemName
                            }).ToList();

            return itemViewList;
        }
        [WebMethod]
        public static RequisitionViewBO LoadProductByRequisitionId(int requisitionId)
        {
            RequisitionViewBO viewBo = new RequisitionViewBO();
            PMRequisitionDA entityDA = new PMRequisitionDA();

            List<PMRequisitionDetailsBO> requisitionBO = new List<PMRequisitionDetailsBO>();
            requisitionBO = entityDA.GetPMRequisitionDetailsByIDForOut(requisitionId, 0);

            requisitionBO = requisitionBO.Where(r => r.DeliverStatus != "Full").ToList();

            viewBo.Requisition = entityDA.GetPMRequisitionInfoByID(requisitionId);
            viewBo.RequisitionDetails = requisitionBO;

            return viewBo;
        }
        [WebMethod]
        public static RequisitionViewBO LoadProductByRequisitionIdForEdit(int requisitionId)
        {
            RequisitionViewBO viewBo = new RequisitionViewBO();
            PMRequisitionDA entityDA = new PMRequisitionDA();

            List<PMRequisitionDetailsBO> requisitionBO = new List<PMRequisitionDetailsBO>();
            requisitionBO = entityDA.GetPMRequisitionDetailsByID(requisitionId); ;

            //requisitionBO = requisitionBO.Where(r => r.DeliverStatus != "Full").ToList();

            viewBo.Requisition = entityDA.GetPMRequisitionInfoByID(requisitionId);
            viewBo.RequisitionDetails = requisitionBO;

            return viewBo;
        }
        [WebMethod]
        public static string IsProductSerializable(int productId)
        {
            InvItemDA productDA = new InvItemDA();
            InvItemBO productBO = new InvItemBO();

            int isSerialProduct = 0;

            productBO = productDA.GetInvItemInfoById(0, productId);

            if (productBO.ProductType == "Serial Product")
            {
                isSerialProduct = 1;
            }

            return isSerialProduct.ToString();
        }
        [WebMethod]
        public static bool ValidateSerialNumber(int productId, string serialNumber)
        {
            bool isValidSerial = false;
            PMProductSerialInfoDA entityDA = new PMProductSerialInfoDA();
            PMProductSerialInfoBO entityBO = new PMProductSerialInfoBO();
            entityBO = entityDA.GetPMProductSerialInfoBySerialNumberForSale(productId, serialNumber);

            if (entityBO != null)
            {
                isValidSerial = true;
            }

            //var ownerDetailBO = (List<PMProductOutBO>)HttpContext.Current.Session["PMProductOut"];

            //if (ownerDetailBO != null)
            //{
            //    var ownerDetail = ownerDetailBO.Where(x => x.SerialNumber == Quantity_Serial && x.SerialNumber != "").FirstOrDefault();

            //    if (ownerDetail.ReceivedId != 0 || ownerDetail.ReceivedId.ToString() != "")
            //    {
            //        tmpSerialId = ownerDetail.ReceivedId.ToString();
            //    }
            //}

            return isValidSerial;
        }
        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemSearch(string searchTerm, int costCenetrId, int categoryId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetNameCategoryCostcenterWiseItemDetailsForAutoSearch(searchTerm, categoryId, costCenetrId);

            return itemInfo;
        }
        [WebMethod]
        public static ReturnInfo SaveProductOut(PMProductOutBO ProductOut, List<PMProductOutDetailsBO> AddedOutDetails, List<PMProductOutDetailsBO> EditedOutDetails, List<PMProductOutDetailsBO> DeletedOutDetails)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                int productReceiveApproval = 0;

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isProductOutApproval = new HMCommonSetupBO();
                HMCommonSetupBO isProductOutReceiveApproval = new HMCommonSetupBO();
                isProductOutApproval = commonSetupDA.GetCommonConfigurationInfo("IsProductOutApprovalEnable", "IsProductOutApprovalEnable");
                isProductOutReceiveApproval = commonSetupDA.GetCommonConfigurationInfo("IsProductOutReceiveApprovalEnable", "IsProductOutReceiveApprovalEnable");
                if (isProductOutReceiveApproval != null)
                {
                    productReceiveApproval = Convert.ToInt32(isProductOutReceiveApproval.SetupValue);
                }

                if (Convert.ToInt32(isProductOutApproval.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedEnable))
                {
                    ProductOut.Status = HMConstants.ApprovalStatus.Pending.ToString();
                }
                else if (Convert.ToInt32(isProductOutApproval.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedDisable))
                {
                    ProductOut.Status = HMConstants.ApprovalStatus.Approved.ToString();
                }

                PMProductOutDA outDa = new PMProductOutDA();
                int outId = 0;
                if (ProductOut.OutId == 0)
                {
                    ProductOut.CreatedBy = userInformationBO.UserInfoId;
                    ProductOut.OutDate = DateTime.Now;
                    //status = outDa.SaveProductOutInfo(ProductOut, AddedOutDetails, productReceiveApproval, out outId);

                    if (status)
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                       bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                       EntityTypeEnum.EntityType.PMProductOut.ToString(), outId,
                       ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                       hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductOut));
                    }
                }
                else
                {
                    ProductOut.LastModifiedBy = userInformationBO.UserInfoId;
                    status = outDa.UpdateProductOutInfo(ProductOut, AddedOutDetails, EditedOutDetails, DeletedOutDetails);

                    if (status)
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                       bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                       EntityTypeEnum.EntityType.PMProductOut.ToString(), ProductOut.OutId,
                       ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                       hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductOut));
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
        public static List<InvLocationBO> InvLocationByCostCenter(int costCenterId)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvItemLocationByCostCenter(costCenterId);

            return location;
        }
        [WebMethod]
        public static List<InvLocationBO> InvLocationSearch(string searchTerm, int costCenterId)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvLocationInfoByCostCenterNameCodeAutoSearch(costCenterId, searchTerm);

            return location;
        }
        [WebMethod]
        public static ProductOutViewBO FillForm(int outId)
        {
            PMProductOutDA outDA = new PMProductOutDA();
            ProductOutViewBO viewBo = new ProductOutViewBO();

            viewBo.ProductOut = outDA.GetProductOutById(outId);
            viewBo.ProductOutDetails = outDA.GetProductOutDetailsById(outId);

            return viewBo;
        }
        [WebMethod]
        public static List<PMProductOutDetailsBO> GetProductOutDetails(int outId)
        {
            PMProductOutDA outDA = new PMProductOutDA();
            return outDA.GetProductOutDetailsById(outId);
        }
        [WebMethod]
        public static bool GetItemStock(int itemId, int locationId, decimal quantity)
        {
            bool isValid = false;
            InvItemDA itemDA = new InvItemDA();
            InvItemStockInformationBO itemStockBO = new InvItemStockInformationBO();
            itemStockBO = itemDA.GetInvItemStockInfo(itemId, locationId);
            if (itemStockBO != null)
            {
                if (itemStockBO.StockQuantity >= quantity)
                {
                    isValid = true;
                }
            }
            return isValid;
        }
        [WebMethod]
        public static List<PMProductSerialInfoBO> GetSerialNumberByProductId(int productId, int outId)
        {
            PMProductSerialInfoDA entityDA = new PMProductSerialInfoDA();
            List<PMProductSerialInfoBO> productList = new List<PMProductSerialInfoBO>();
            
            if (outId == 0)
                productList = entityDA.GetPMProductInfoByCategoryId(productId);
            else
                productList = entityDA.GetSerialInfoByProductNOutId(productId, outId);

            return productList;
        }
    }
}