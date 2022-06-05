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
    public partial class frmInvStockAdjustment : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
            invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("ProductAdjustmentTemplate", "ProductAdjustmentTemplate");
            
            if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.ViewTemplate.Template1))
            {
                StockVarianceTemplate1.Visible = true;
                StockVarianceTemplate2.Visible = false;
                hfAdjustmentProductTemplate.Value = "1";
            }
            else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.ViewTemplate.Template2))
            {
                StockVarianceTemplate1.Visible = false;
                StockVarianceTemplate2.Visible = true;
                hfAdjustmentProductTemplate.Value = "2";
            }

            if (!IsPostBack)
            {
                companyProjectUserControl.ddlFirstValueVar = "select";
                isSerialAutoLoad();
                LoadAllCostCentreTabInfo();
                LoadStockBy();
                LoadCategory();
                LoadInvTransactionMode();
                LoadCommonDropDownHiddenField();

                if (Session["StockAdjustmentId"] != null)
                {
                    hfIsEditedFromApprovedForm.Value = "1";
                    hfStockAdjustmentId.Value = Session["StockAdjustmentId"].ToString();

                    Session.Remove("StockAdjustmentId");
                }
            }
            CheckObjectPermission();
        }
        protected void gvFinishedProductInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                var item = (ItemStockAdjustmentBO)e.Row.DataItem;

                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                ImageButton ImgDetailsApproved = (ImageButton)e.Row.FindControl("ImgDetailsApproved");

                if (item.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString() && item.ApprovedStatus != HMConstants.ApprovalStatus.Cancel.ToString())
                {
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                    ImgDetailsApproved.Visible = isSavePermission;
                }
                else
                {
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                    ImgDetailsApproved.Visible = false;
                }
            }
        }
        protected void gvFinishedProductInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int stockAdjustmentId = Convert.ToInt32(e.CommandArgument.ToString());

                if (e.CommandName == "CmdDelete")
                {
                    string result = string.Empty;

                    InvItemDA itemDa = new InvItemDA();
                    bool status = itemDa.DeleteItemStockAdjustment(stockAdjustmentId);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.ItemStockAdjustment.ToString(), stockAdjustmentId,
                            ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ItemStockAdjustment));
                        LoadGrid();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                    SetTab("SearchTab");
                }
                if (e.CommandName == "CmdAdjustmentApproved")
                {
                    InvItemDA itemDa = new InvItemDA();
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                    try
                    {
                        bool status = itemDa.ApprovedAdjustmentStatusNUpdateItemStock(Convert.ToInt32(stockAdjustmentId), HMConstants.ApprovalStatus.Approved.ToString(), userInformationBO.UserInfoId);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(),
                                EntityTypeEnum.EntityType.ItemStockAdjustment.ToString(), stockAdjustmentId,
                                ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(),
                                hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ItemStockAdjustment));
                            LoadGrid();
                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                        }

                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
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
        private void isSerialAutoLoad()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO IsSeriaFillBO = new HMCommonSetupBO();
            IsSeriaFillBO = commonSetupDA.GetCommonConfigurationInfo("IsItemSerialFillWithAutoSearch", "IsItemSerialFillWithAutoSearch");
            hfIsItemSerialFillWithAutoSearch.Value = IsSeriaFillBO.SetupValue;

        }
        private void LoadAllCostCentreTabInfo()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
            invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsUserpermissionAppliedToCostcenterFilteringForPOPR", "IsUserpermissionAppliedToCostcenterFilteringForPOPR");

            costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId)
                .Where(c => c.CostCenterType == "Inventory").ToList();

            ddlCostCentre.DataSource = costCentreTabBOList;
            ddlCostCentre.DataTextField = "CostCenter";
            ddlCostCentre.DataValueField = "CostCenterId";
            ddlCostCentre.DataBind();

            ddlSearchCostCenter.DataSource = costCentreTabBOList;
            ddlSearchCostCenter.DataTextField = "CostCenter";
            ddlSearchCostCenter.DataValueField = "CostCenterId";
            ddlSearchCostCenter.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            if (costCentreTabBOList.Count > 1)
            {
                ddlCostCentre.Items.Insert(0, item);
                ddlSearchCostCenter.Items.Insert(0, item);
            }
        }
        private void CheckObjectPermission()
        {
            btnSave2.Visible = isSavePermission;
            //if (!isSavePermission)
            //{
            //    isKitchenItemCostCenterInformationDivEnable = -1;
            //}
        }
        private void LoadLocation()
        {
            InvLocationDA entityDA = new InvLocationDA();
            var List = entityDA.GetInvLocationInfo();

            ddlLocation.DataSource = List;
            ddlLocation.DataTextField = "Name";
            ddlLocation.DataValueField = "LocationId";
            ddlLocation.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            if (List.Count > 1)
                ddlLocation.Items.Insert(0, item);

            ddlStoreLocation.DataSource = List;
            ddlStoreLocation.DataTextField = "Name";
            ddlStoreLocation.DataValueField = "LocationId";
            ddlStoreLocation.DataBind();
            if (List.Count > 1)
                ddlStoreLocation.Items.Insert(0, item);
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            ///List = da.GetInvItemCatagoryInfoByServiceType("Product");
            List = da.GetAllActiveInvItemCatagoryInfoByServiceType("All");
            ddlCategory.DataSource = List;
            ddlCategory.DataTextField = "MatrixInfo";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCategory.Items.Insert(0, item);
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
        }
        private void LoadInvTransactionMode()
        {
            InvItemDA entityDA = new InvItemDA();
            var List = entityDA.GetInvTransactionMode();

            ddlInvTransactionMode.DataSource = List;
            ddlInvTransactionMode.DataTextField = "HeadName";
            ddlInvTransactionMode.DataValueField = "TModeId";
            ddlInvTransactionMode.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlInvTransactionMode.Items.Insert(0, item);

            var List1 = entityDA.GetInvTransactionMode().Where(w => w.TModeId != 1).ToList();

            ddlAdjustmentReason.DataSource = List1;
            ddlAdjustmentReason.DataTextField = "HeadName";
            ddlAdjustmentReason.DataValueField = "TModeId";
            ddlAdjustmentReason.DataBind();

            ddlAdjustmentReason.Items.Insert(0, item);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        private void LoadGrid()
        {
            SetTab("SearchTab");

            InvItemDA itemDa = new InvItemDA();
            List<ItemStockAdjustmentBO> itemStockAdjustment = new List<ItemStockAdjustmentBO>();

            DateTime? fromDate = null, toDate = null;
            int costCenterId = Convert.ToInt32(ddlSearchCostCenter.SelectedValue);
            int locationId = 0;

            if (!string.IsNullOrEmpty(ddlSearchLocation.SelectedValue))
                locationId = Convert.ToInt32(ddlSearchLocation.SelectedValue);

            fromDate = DateTime.Now.Date;
            toDate = DateTime.Now.Date;

            itemStockAdjustment = itemDa.GetItemAdjustmentInfoSearch(costCenterId, locationId, fromDate, toDate);
            gvFinishedProductInfo.DataSource = itemStockAdjustment;
            gvFinishedProductInfo.DataBind();
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
        public static ItemAdjustmentViewBO FIllForm(int stockAdjustmentId)
        {
            InvItemDA itmDa = new InvItemDA();
            ItemAdjustmentViewBO adjustmentView = new ItemAdjustmentViewBO();
            ItemAdjustmentViewBO itemAdjustment = new ItemAdjustmentViewBO();
            List<ItemStockAdjustmentDetailsBO> itemAdjustmentDetails = new List<ItemStockAdjustmentDetailsBO>();

            adjustmentView.AdjustmentItem = itmDa.GetItemAdjustmentInfoById(stockAdjustmentId);
            adjustmentView.AdjustmentItemDetails = itmDa.GetItemAdjustmentDetailsById(stockAdjustmentId);

            return adjustmentView;
        }
        [WebMethod]
        public static List<InvLocationBO> InvLocationByCostCenter(int costCenterId)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvItemLocationByCostCenter(costCenterId);

            return location;
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
        [WebMethod]
        public static decimal GetReceipeItemCost(int itemId, int stockById, decimal quantity)
        {
            RestaurantRecipeDetailBO itemInfo = new RestaurantRecipeDetailBO();

            InvItemDA itemDA = new InvItemDA();
            itemInfo = itemDA.GetReceipeItemCost(itemId, stockById, quantity);

            return itemInfo.ItemCost;
        }
        //[WebMethod]
        //public static ReturnInfo SaveStockAdjustment(ItemStockAdjustmentBO ItemStockAdjustment, List<ItemStockAdjustmentDetailsBO> StockAdjustmentDetails, List<ItemStockAdjustmentDetailsBO> StockAdjustmentDetailsEdit, List<ItemStockAdjustmentDetailsBO> DeletedAdjustItem)
        //{
        //    ReturnInfo rtninfo = new ReturnInfo();
        //    Boolean status = false;
        //    int stockAdjustmentId = 0;

        //    try
        //    {
        //        HMUtility hmUtility = new HMUtility();
        //        UserInformationBO userInformationBO = new UserInformationBO();
        //        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

        //        InvItemDA itemDa = new InvItemDA();

        //        ItemStockAdjustment.AdjustmentDate = DateTime.Now;

        //        if (ItemStockAdjustment.StockAdjustmentId == 0)
        //        {
        //            ItemStockAdjustment.ApprovedStatus = HMConstants.ApprovalStatus.Pending.ToString();
        //            ItemStockAdjustment.CreatedBy = userInformationBO.UserInfoId;

        //            status = itemDa.SaveItemStockAdjustment(ItemStockAdjustment, StockAdjustmentDetails,AddedSerialzableProduct, out stockAdjustmentId);

        //            if (status)
        //            {
        //                rtninfo.IsSuccess = true;
        //                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

        //                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
        //                EntityTypeEnum.EntityType.ItemStockAdjustment.ToString(), stockAdjustmentId,
        //                ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
        //                hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ItemStockAdjustment));
        //            }
        //        }
        //        else
        //        {
        //            ItemStockAdjustment.LastModifiedBy = userInformationBO.UserInfoId;
        //            status = itemDa.UpdateItemStockAdjustment(ItemStockAdjustment, StockAdjustmentDetails, StockAdjustmentDetailsEdit, DeletedAdjustItem);

        //            if (status)
        //            {
        //                rtninfo.IsSuccess = true;
        //                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

        //                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
        //                EntityTypeEnum.EntityType.ItemStockAdjustment.ToString(), ItemStockAdjustment.StockAdjustmentId,
        //                ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(),
        //                hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ItemStockAdjustment));
        //            }
        //        }

        //        if (!status)
        //        {
        //            rtninfo.IsSuccess = false;
        //            rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        rtninfo.IsSuccess = false;
        //        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
        //    }

        //    return rtninfo;
        //}
        [WebMethod]
        public static ReturnInfo SaveStockAdjustment2(ItemStockAdjustmentBO ItemStockAdjustment, List<ItemStockAdjustmentDetailsBO> StockAdjustDetails, List<ItemStockAdjustmentDetailsBO> StockAdjustDetailsEdit, List<ItemStockAdjustmentDetailsBO> DeletedAdjustItem, 
                                                    List<InvItemStockAdjustmentSerialInfoBO> AddedSerialzableProduct, List<InvItemStockAdjustmentSerialInfoBO> DeletedSerialzableProduct)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            int stockAdjustmentId = 0;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsProductAdjustmentApprovalEnable", "IsProductAdjustmentApprovalEnable");

                InvItemDA itemDa = new InvItemDA();

                ItemStockAdjustment.AdjustmentDate = DateTime.Now;

                if (ItemStockAdjustment.StockAdjustmentId == 0)
                {
                    //if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.ProductReceiveTemplate.ApprovedEnable))
                    //{
                    ItemStockAdjustment.ApprovedStatus = HMConstants.ApprovalStatus.Submit.ToString();
                    //}
                    //else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.ProductReceiveTemplate.ApprovedDisable))
                    //{
                    //    ItemStockAdjustment.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();
                    //}

                    ItemStockAdjustment.CreatedBy = userInformationBO.UserInfoId;

                    status = itemDa.SaveItemStockAdjustment(ItemStockAdjustment, StockAdjustDetails,AddedSerialzableProduct, out stockAdjustmentId);

                    //if (status && Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.ProductReceiveTemplate.ApprovedDisable))
                    //{
                    //    itemDa.ApprovedAdjustmentStatusNUpdateItemStock(ItemStockAdjustment.CostCenterId, StockAdjustDetails[0].LocationId, stockAdjustmentId, HMConstants.ApprovalStatus.Approved.ToString());
                    //}

                    if (status)
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                        EntityTypeEnum.EntityType.ItemStockAdjustment.ToString(), stockAdjustmentId,
                        ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(),
                        hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ItemStockAdjustment));
                    }
                }
                else
                {
                    ItemStockAdjustment.LastModifiedBy = userInformationBO.UserInfoId;
                    status = itemDa.UpdateItemStockAdjustment(ItemStockAdjustment, StockAdjustDetails, StockAdjustDetailsEdit, DeletedAdjustItem);

                    if (status)
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                        EntityTypeEnum.EntityType.ItemStockAdjustment.ToString(), ItemStockAdjustment.StockAdjustmentId,
                        ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(),
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
        public static List<ItemAdjustmentDetailsByItemAccessFrequencyBO> LoadItems(int companyId, int projectId, int costCenterId, int locationId, int categoryId, string adjustmentFrequence)
        {
            InvItemDA itemDa = new InvItemDA();
            List<ItemAdjustmentDetailsByItemAccessFrequencyBO> itemWiseStoc = new List<ItemAdjustmentDetailsByItemAccessFrequencyBO>();
            itemWiseStoc = itemDa.GetItemAdjustmentDetailsByItemAccessFrequency(companyId, projectId, costCenterId, locationId, categoryId, DateTime.Now, adjustmentFrequence, 0);

            return itemWiseStoc;
        }

        // ------------------------ Stock Adjustment Template 2 ---------------------------------------------------------
        [WebMethod]
        public static ItemAdjustmentViewBO CostcenterLocationWiseItemStock(int companyId, int projectId, int costCenterId, int locationId, int categoryId, string adjustmentFrequence, int itemId)
        {
            string grid = string.Empty, tr = string.Empty;
            int rowCount = 0, stockAdjustmentDetailsId = 0, isEdited = 0;
            decimal quantity = 0.00M;

            InvItemDA itemDa = new InvItemDA();
            List<ItemAdjustmentDetailsByItemAccessFrequencyBO> itemWiseStoc = new List<ItemAdjustmentDetailsByItemAccessFrequencyBO>();
            itemWiseStoc = itemDa.GetItemAdjustmentDetailsByItemAccessFrequency(companyId, projectId, costCenterId, locationId, categoryId, DateTime.Now, adjustmentFrequence, itemId);

            List<ItemStockAdjustmentDetailsBO> itemAdjustment = new List<ItemStockAdjustmentDetailsBO>();
            itemAdjustment = itemDa.GetItemAdjustmentDetailsByDateNCostcenterId(companyId, projectId, costCenterId, locationId, DateTime.Now);

            ItemAdjustmentViewBO adjvw = new ItemAdjustmentViewBO();
            adjvw.AdjustmentItem = new ItemStockAdjustmentBO();

            if (itemAdjustment.Count != 0)
            {
                adjvw.AdjustmentItem.StockAdjustmentId = itemAdjustment[0].StockAdjustmentId;
                adjvw.AdjustmentItem.LocationId = itemAdjustment[0].LocationId;
            }

            foreach (ItemAdjustmentDetailsByItemAccessFrequencyBO stck in itemWiseStoc)
            {
                var v = (from ad in itemAdjustment where ad.ItemId == stck.ItemId && ad.ColorId == stck.ColorId && ad.SizeId == stck.SizeId && ad.StyleId == stck.StyleId select ad).FirstOrDefault();
                if (v != null)
                {
                    stockAdjustmentDetailsId = v.StockAdjustmentDetailsId;
                    quantity = v.ActualQuantity;
                    isEdited = 1;
                }

                if (rowCount % 2 == 0)
                {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else
                {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:15%;'>" + stck.ItemName + "</td>";
                tr += "<td style='width:10%;'>" + stck.StockBy + "</td>";
                tr += "<td style='width:12%;'>" + stck.OpeningStock + "</td>";
                tr += "<td style='width:10%;'>" + stck.ReceivedQuantity + "</td>";
                tr += "<td style='width:12%;'>" + stck.ActualUsageQuantity + "</td>";
                tr += "<td style='width:10%;'>" + stck.WastageQuantity + "</td>";
                tr += "<td style='width:11%;'>" + stck.StockQuantityAfterWastageDeduction + "</td>";
                tr += "<td style='width:10%; text-align:center;'> <input type='text' class='form-control' placeholder=' >= 0' title='blank input is not adjusted. Give >=0. If zero(0) amount given please think before save.' id='txt" + stck.ItemId.ToString() + "' value = '" + (quantity == 0 ? string.Empty : quantity.ToString()) + "' onblur='CheckInputValue(this)' onkeydown='if (event.keyCode == 13) { return true;}' style='width:90px; padding:0; padding-left:2px; padding-right:2px; margin:0;' TabIndex=" + (1000 + (rowCount + 1)).ToString() + " /> </td>";
                tr += "<td style='width:5%;'></td>";
                tr += "<td style='width:5%;'>";
                if (stck.ProductType == "Serial Product")
                    tr += "<a href = 'javascript:void();' onclick = 'javascript:return AddSerialForStockAdjustment(this)' > <img alt = 'Serial' src = '../Images/serial.png' /></a>";
                tr += "</td>";
                tr += "<td style='display:none'>" + stockAdjustmentDetailsId + "</td>"; //10
                tr += "<td style='display:none'>" + stck.ItemId + "</td>"; //11
                tr += "<td style='display:none'>" + locationId + "</td>"; //12
                tr += "<td style='display:none'>" + stck.StockById + "</td>"; //13                
                tr += "<td style='display:none'>0</td>"; //14 previous value                
                tr += "<td style='display:none'>" + isEdited + "</td>"; //15
                tr += "<td style='display:none'>" + stck.ColorId + "</td>"; //16
                tr += "<td style='display:none'>" + stck.SizeId + "</td>"; //17
                tr += "<td style='display:none'>" + stck.StyleId + "</td>"; //18

                tr += "</tr>";

                rowCount++;
                quantity = 0.00M;
                stockAdjustmentDetailsId = 0;
                isEdited = 0;
            }

            grid += GridHeader() + tr + "</tbody> </table>";

            adjvw.AdjustmentItemDetailsGrid = grid;

            return adjvw;
        }
        public static string GridHeader()
        {
            string gridHead = string.Empty;

            gridHead += "<table id='ItemAdjustmentGrid' class='table table-bordered table-condensed table-responsive' style='width: 100%;'>" +
                         "       <thead>" +
                         "           <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>" +
                         "               <th style='width: 15%;'>" +
                         "                   Item Name" +
                         "               </th>" +
                         "               <th style='width: 10%;'>" +
                         "                   Stock By" +
                         "               </th>" +
                         "               <th style='width: 12%;'>" +
                         "                   Opening Stock" +
                         "               </th>" +
                         "               <th style='width: 10%;'>" +
                         "                   Received Quantity" +
                         "               </th>" +
                         "               <th style='width: 12%;'>" +
                         "                   Usage Quantity" +
                         "               </th>" +
                         "               <th style='width: 10%;'>" +
                         "                   Wastage Quantity" +
                         "               </th>" +
                         "               <th style='width: 11%;'>" +
                         "                   Stock Quantity" +
                         "               </th>" +
                         "               <th style='width: 10%;'>" +
                         "                   Actual Quantity" +
                         "               </th>" +
                         "               <th style='width: 5%;'>" +
                         "                   Variance Quantity" +
                         "               </th>" +
                         "               <th style='width: 5%;'>" +
                         "                   Action" +
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
        [WebMethod]
        public static List<InvItemStockVarianceDetailsBO> GetVarianceProductDetails(int stockVarianceId)
        {
            InvItemDA itemDa = new InvItemDA();
            return itemDa.GetItemVarianceDetailsById(stockVarianceId);
        }
        [WebMethod]
        public static List<SerialDuplicateBO> SerialSearch(string serialNumber, int locationId, int itemId)
        {
            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();

            PMProductOutDA outDA = new PMProductOutDA();
            serial = outDA.GetAvailableSerialForAutoSearch(serialNumber, locationId, itemId);

            return serial;
        }
        protected void ddlSearchCostCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvItemLocationByCostCenter(Convert.ToInt32(ddlSearchCostCenter.SelectedValue));

            ddlSearchLocation.DataSource = location;
            ddlSearchLocation.DataTextField = "Name";
            ddlSearchLocation.DataValueField = "LocationId";
            ddlSearchLocation.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSearchLocation.Items.Insert(0, item);

            B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
            // A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");


        }
    }
}