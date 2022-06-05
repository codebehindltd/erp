using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Data.Inventory;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Inventory
{ 
    public partial class frmInvStockVariance : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
            invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("ProductVarianceTemplate", "ProductVarianceTemplate");

            if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.ViewTemplate.Template1))
            {
                StockVarianceTemplate1.Visible = true;
                StockVarianceTemplate2.Visible = false;
                // hfReceivedProductTemplate.Value = "1";
            }
            else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.ViewTemplate.Template2))
            {
                StockVarianceTemplate1.Visible = false;
                StockVarianceTemplate2.Visible = true;
                //hfReceivedProductTemplate.Value = "2";
            }

            if (!IsPostBack)
            {
                CheckObjectPermission();
                LoadCostCenter();
                LoadStockBy();
                LoadInvTransactionMode();
                LoadCommonDropDownHiddenField();
                LoadLocation();
                LoadCategory();

                if (Session["StockAdjustmentId"] != null)
                {
                    hfIsEditedFromApprovedForm.Value = "1";
                    hfStockAdjustmentId.Value = Session["StockAdjustmentId"].ToString();

                    Session.Remove("StockAdjustmentId");
                }
            }
        }
        protected void gvFinishedProductInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                var item = (InvItemStockVarianceBO)e.Row.DataItem;

                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");

                if (item.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString() && item.ApprovedStatus != HMConstants.ApprovalStatus.Cancel.ToString())
                {
                    imgUpdate.Visible = isUpdatePermission;
                    imgDelete.Visible = isDeletePermission;
                }
                else
                {
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                }
            }
        }
        protected void gvFinishedProductInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "CmdDelete")
                {
                    int stockAdjustmentId = Convert.ToInt32(e.CommandArgument.ToString());
                    string result = string.Empty;

                    InvItemDA itemDa = new InvItemDA();
                    bool status = itemDa.DeleteItemStockAdjustment(stockAdjustmentId);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        LoadGrid();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                    this.SetTab("SearchTab");
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }
        //************************ User Defined Function ********************//
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetInvItemCatagoryInfoByServiceType("Product");
            ddlCategory.DataSource = List;
            ddlCategory.DataTextField = "MatrixInfo";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCategory.Items.Insert(0, item);

            ListItem item2 = new ListItem();
            item2.Value = "-1";
            item2.Text = hmUtility.GetDropDownFirstValue();
            ddlCategory.Items.Insert(0, item2);
        }
        private void CheckObjectPermission()
        {
            btnSave2.Visible = isSavePermission;
            //if (!isSavePermission)
            //{
            //    isKitchenItemCostCenterInformationDivEnable = -1;
            //}
        }
        private void LoadCostCenter()
        {
            CostCentreTabDA entityDA = new CostCentreTabDA();
            var List = entityDA.GetCostCentreTabInfo();

            this.ddlCostCenter.DataSource = List;
            this.ddlCostCenter.DataTextField = "CostCenter";
            this.ddlCostCenter.DataValueField = "CostCenterId";
            this.ddlCostCenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCostCenter.Items.Insert(0, item);

            this.ddlSearchCostCenter.DataSource = List;
            this.ddlSearchCostCenter.DataTextField = "CostCenter";
            this.ddlSearchCostCenter.DataValueField = "CostCenterId";
            this.ddlSearchCostCenter.DataBind();
            this.ddlSearchCostCenter.Items.Insert(0, item);
        }
        public void LoadLocation()
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvLocationInfo();

            this.ddlStoreLocation.DataSource = location;
            this.ddlStoreLocation.DataTextField = "Name";
            this.ddlStoreLocation.DataValueField = "LocationId";
            this.ddlStoreLocation.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlStoreLocation.Items.Insert(0, item);
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
        private void LoadInvTransactionMode()
        {
            InvItemDA entityDA = new InvItemDA();
            var List = entityDA.GetInvTransactionMode();

            this.ddlInvTransactionMode.DataSource = List;
            this.ddlInvTransactionMode.DataTextField = "HeadName";
            this.ddlInvTransactionMode.DataValueField = "TModeId";
            this.ddlInvTransactionMode.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlInvTransactionMode.Items.Insert(0, item);

            var List1 = entityDA.GetInvTransactionMode().Where(w => w.TModeId != 1).ToList();

            this.ddlAdjustmentReason.DataSource = List1;
            this.ddlAdjustmentReason.DataTextField = "HeadName";
            this.ddlAdjustmentReason.DataValueField = "TModeId";
            this.ddlAdjustmentReason.DataBind();

            this.ddlAdjustmentReason.Items.Insert(0, item);
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        private void LoadGrid()
        {
            this.SetTab("SearchTab");

            InvItemDA itemDa = new InvItemDA();
            List<InvItemStockVarianceBO> itemStockVariance = new List<InvItemStockVarianceBO>();

            DateTime? fromDate = null, toDate = null;
            int costCenterId = 0;

            if (ddlSearchCostCenter.SelectedValue != "0")
            {
                costCenterId = Convert.ToInt32(ddlSearchCostCenter.SelectedValue);
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

            itemStockVariance = itemDa.GetItemStockVarianceInfoSearch(costCenterId, 0, fromDate, toDate);

            gvFinishedProductInfo.DataSource = itemStockVariance;
            gvFinishedProductInfo.DataBind();
        }
        private void SetTab(string TabName)
        {
            if (TabName == "SearchTab")
            {
                //B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                //B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        public static string GridHeader()
        {
            string gridHead = string.Empty;

            gridHead += "<table id='ItemVarianceGrid' class='table table-bordered table-condensed table-responsive' style='width: 100%;'>" +
                         "       <thead>" +
                         "           <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>" +
                         "               <th style='width: 15%;'>" +
                         "                   Product Name" +
                         "               </th>" +
                         "               <th style='width: 10%;'>" +
                         "                   Stock Quantity" +
                         "               </th>" +
                         "               <th style='width: 10%;'>" +
                         "                   Stock By" +
                         "               </th>" +
                         "               <th style='width: 10%;'>" +
                         "                   Actual Usage" +
                         "               </th>" +
                         "               <th style='width: 10%;'>" +
                         "                   Usage Cost" +
                         "               </th>" +
                         "               <th style='width: 13%;'>" +
                         "                   Reason" +
                         "               </th>" +
                         "               <th style='width: 12%;'>" +
                         "                   Variance Quantity" +
                         "               </th>" +
                         "               <th style='width: 20%;'>" +
                         "                   Reamrks" +
                         "               </th>" +
                         "               <th style='display: none'>" +
                         "                   StockAdjustmentDetailsId" +
                         "               </th>" +
                         "               <th style='display: none'>" +
                         "                   ItemId" +
                         "               </th>" +
                         "               <th style='display: none'>" +
                         "                   LocationId" +
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
        private static string VarianceReasonProcess()
        {
            InvItemDA entityDA = new InvItemDA();
            List<InvTransactionModeBO> transactionMode = new List<InvTransactionModeBO>();
            transactionMode = entityDA.GetInvTransactionMode();

            string options = string.Empty;

            options = "<select class='form-control' style='height: 25px; width:150px;' >";

            foreach (InvTransactionModeBO trm in transactionMode)
            {
                options += "<option value='" + trm.TModeId + "'>" + trm.HeadName + "</option>";
            }

            options += " </select>";

            return options;

        }
        public static string AdjustmentItemDetailsForEdit(List<ItemStockAdjustmentDetailsBO> itemAdjustment)
        {
            string grid = string.Empty, tr = string.Empty;
            int rowCount = 0, isEdited = 0;

            foreach (ItemStockAdjustmentDetailsBO stck in itemAdjustment)
            {
                if (rowCount % 2 == 0)
                {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else
                {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:20%;'>" + stck.ItemName + "</td>";
                tr += "<td style='width:20%;'>" + stck.LocationName + "</td>";
                //tr += "<td style='width:15%;'>" + stck.PreviousQuantity + "</td>";
                tr += "<td style='width:15%;'>" + stck.StockByName + "</td>";
                // tr += "<td style='width:20%; text-align:center;'> <input type='text' id='txt" + stck.ItemId.ToString() + "' value = '" + stck.AdjustmentQuantity + "' onblur='CheckInputValue(this)' style='width:65px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";

                tr += "<td style='display:none'>" + stck.StockAdjustmentDetailsId + "</td>";
                tr += "<td style='display:none'>" + stck.ItemId + "</td>";
                tr += "<td style='display:none'>" + stck.LocationId + "</td>";
                tr += "<td style='display:none'>" + stck.StockById + "</td>";
                //tr += "<td style='display:none'>" + stck.AdjustmentQuantity + "</td>";
                tr += "<td style='display:none'>" + isEdited + "</td>";

                tr += "</tr>";

                rowCount++;
            }

            grid += GridHeader() + tr + "</tbody> </table>";

            return grid;
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemNCategoryAutoSearch(string itemName, int costCenterId, int locationId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemNameNCategoryCostcenterLocationForAutoSearch(itemName, 0, null, null, costCenterId, locationId, true);

            return itemInfo;
        }
        [WebMethod]
        public static List<ItemStockAdjustmentDetailsBO> GetAdjustmentProductDetails(int stockAdjustmentId)
        {
            InvItemDA itemDa = new InvItemDA();
            return itemDa.GetItemAdjustmentDetailsById(stockAdjustmentId);
        }
        [WebMethod]
        public static ItemVarianceViewBO FIllForm(int stockVarianceId)
        {
            InvItemDA itmDa = new InvItemDA();
            InvItemStockVarianceBO stockVariance = new InvItemStockVarianceBO();
            List<InvItemStockVarianceDetailsBO> stockVarianceDetails = new List<InvItemStockVarianceDetailsBO>();

            ItemVarianceViewBO varianceView = new ItemVarianceViewBO();

            varianceView.StockVariance = itmDa.GetItemStockVarianceById(stockVarianceId);
            varianceView.StockVarianceDetails = itmDa.GetItemVarianceDetailsById(stockVarianceId);

            return varianceView;
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
        public static decimal GetReceipeItemCost(int itemId, int stockById, decimal quantity)
        {
            RestaurantRecipeDetailBO itemInfo = new RestaurantRecipeDetailBO();

            InvItemDA itemDA = new InvItemDA();
            itemInfo = itemDA.GetReceipeItemCost(itemId, stockById, quantity);

            return itemInfo.ItemCost;
        }
        [WebMethod]
        public static ReturnInfo SaveStockAdjustment(ItemStockAdjustmentBO ItemStockAdjustment, List<ItemStockAdjustmentDetailsBO> StockAdjustmentDetails, List<ItemStockAdjustmentDetailsBO> StockAdjustmentDetailsEdit, List<ItemStockAdjustmentDetailsBO> DeletedAdjustItem)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                InvItemDA itemDa = new InvItemDA();

                ItemStockAdjustment.AdjustmentDate = DateTime.Now;

                if (ItemStockAdjustment.StockAdjustmentId == 0)
                {
                    ItemStockAdjustment.ApprovedStatus = HMConstants.ApprovalStatus.Pending.ToString();
                    ItemStockAdjustment.CreatedBy = userInformationBO.UserInfoId;

                    // status = itemDa.SaveItemStockAdjustment(ItemStockAdjustment, StockAdjustmentDetails);

                    if (status)
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }
                else
                {
                    ItemStockAdjustment.LastModifiedBy = userInformationBO.UserInfoId;
                    status = itemDa.UpdateItemStockAdjustment(ItemStockAdjustment, StockAdjustmentDetails, StockAdjustmentDetailsEdit, DeletedAdjustItem);

                    if (status)
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                        EntityTypeEnum.EntityType.ItemStockAdjustment.ToString(), ItemStockAdjustment.StockAdjustmentId,
                        ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                        hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ItemStockAdjustment));
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
        public static ReturnInfo SaveStockVariance(InvItemStockVarianceBO ItemStockVariance, List<InvItemStockVarianceDetailsBO> StockVarianceDetails, List<InvItemStockVarianceDetailsBO> StockVarianceDetailsEdit, List<InvItemStockVarianceDetailsBO> DeletedStockVarianceItem)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            int stockVarianceId = 0;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                InvItemDA itemDa = new InvItemDA();

                ItemStockVariance.StockVarianceDate = DateTime.Now;

                if (ItemStockVariance.StockVarianceId == 0)
                {
                    ItemStockVariance.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();
                    ItemStockVariance.CreatedBy = userInformationBO.UserInfoId;

                    status = itemDa.SaveItemStockVariance(ItemStockVariance, StockVarianceDetails, out stockVarianceId);

                    if (status)
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                        EntityTypeEnum.EntityType.InvItemStockVariance.ToString(), stockVarianceId,
                        ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                        hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvItemStockVariance));
                    }
                }
                else
                {
                    ItemStockVariance.LastModifiedBy = userInformationBO.UserInfoId;
                    //status = itemDa.UpdateItemStockAdjustment(ItemStockAdjustment, StockAdjustmentDetails, StockAdjustmentDetailsEdit, DeletedAdjustItem);

                    if (status)
                    {
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
        public static string CostcenterLocationWiseItemStock(int locationId, int categoryId, bool isCustomerItem, bool isSupplierItem)
        {
            string grid = string.Empty, tr = string.Empty;
            int rowCount = 0, stockVarianceDetailsId = 0, isEdited = 0;
            string reasons = string.Empty;
            DateTime transactionDate = DateTime.Now;
            reasons = VarianceReasonProcess();

            InvItemDA itemDa = new InvItemDA();
            List<ItemWiseStockReportViewBO> itemWiseStoc = new List<ItemWiseStockReportViewBO>();

            HMCommonDA comonDa = new HMCommonDA();
            transactionDate = comonDa.GetModuleWisePreviousDayTransaction("Restaurant");

            itemWiseStoc = itemDa.GetCostcenterLocationWiseItemNUsage(locationId, categoryId, transactionDate, DateTime.Now, isCustomerItem, isSupplierItem);

            foreach (ItemWiseStockReportViewBO stck in itemWiseStoc)
            {
                if (rowCount % 2 == 0)
                {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else
                {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:15%;'>" + stck.ItemName + "</td>";
                tr += "<td style='width:10%;'>" + stck.StockQuantity + "</td>";
                tr += "<td style='width:10%;'>" + stck.HeadName + "</td>";
                tr += "<td style='width:10%;'>" + stck.ActualUsage + "</td>";
                tr += "<td style='width:10%;'>" + stck.UsageCost.ToString("0.00") + "</td>";
                tr += "<td style='width:13%;'>" + reasons + "</td>";
                tr += "<td style='width:12%; text-align:center;'> <input type='text' id='txt" + stck.ItemId.ToString() + "' value = '' onblur='CheckInputValue(this)' class='form-control' style='width:50px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                tr += "<td style='width:20%; text-align:center;'> <input type='text' id='txtRmk" + stck.ItemId.ToString() + "' value = '' class='form-control' style='width:130px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";

                tr += "<td style='display:none'>" + stockVarianceDetailsId + "</td>"; //8
                tr += "<td style='display:none'>" + stck.ItemId + "</td>"; //9
                tr += "<td style='display:none'>" + stck.LocationId + "</td>"; //10
                tr += "<td style='display:none'>" + stck.StockById + "</td>"; //11
                tr += "<td style='display:none'>0</td>"; //12 tmode id
                tr += "<td style='display:none'>0</td>"; //13 previous value
                tr += "<td style='display:none'>" + stck.UnitPrice.ToString("0.00") + "</td>"; //14
                tr += "<td style='display:none'>" + isEdited + "</td>"; //15

                tr += "</tr>";

                rowCount++;
            }

            grid += GridHeader() + tr + "</tbody> </table>";

            return grid;
        }
        [WebMethod]
        public static List<InvItemStockVarianceDetailsBO> GetVarianceProductDetails(int stockVarianceId)
        {
            InvItemDA itemDa = new InvItemDA();
            return itemDa.GetItemVarianceDetailsById(stockVarianceId);
        }

    }
}