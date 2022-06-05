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
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.RetailPOS;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class ItemTransfer : BasePage
    {
        public ItemTransfer()
            : base("ItemTransferInformation")
        {

        }
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                isSerialAutoLoad();
                LoadProductOutFor();
                LoadBillNumber();
                LoadRequisition();
                LoadCostCenter();
                LoadStockBy();
                LoadCategory();
                LoadProduct();
                LoadProductInfo();
                CheckPermission();
                LoadQuotation();
                IsAttributeItemShow();
            }
        }
        private void IsAttributeItemShow()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsItemAttributeEnable", "IsItemAttributeEnable");
            if (homePageSetupBO != null)
            {
                if (homePageSetupBO.SetupId > 0)
                {
                    hfIsItemAttributeEnable.Value = homePageSetupBO.SetupValue;
                }
            }
        }
        private void isSerialAutoLoad()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO IsSeriaFillBO = new HMCommonSetupBO();
            IsSeriaFillBO = commonSetupDA.GetCommonConfigurationInfo("IsItemSerialFillWithAutoSearch", "IsItemSerialFillWithAutoSearch");
            hfIsItemSerialFillWithAutoSearch.Value = IsSeriaFillBO.SetupValue;

        }
        private void LoadRequisition()
        {
            PMRequisitionDA entityDA = new PMRequisitionDA();
            List<PMRequisitionBO> requisitionList = entityDA.GetApprovedNNotDeliveredRequisitionForOut();
            ddlRequisition.DataSource = requisitionList;
            ddlRequisition.DataTextField = "PRNumber";
            ddlRequisition.DataValueField = "RequisitionId";
            ddlRequisition.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            if (requisitionList.Count > 1)
                ddlRequisition.Items.Insert(0, item);
        }
        public void LoadCostCenter()
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId)
                    .Where(a => a.CostCenterType == "Inventory").ToList();

            var mainStore = List.Where(i => i.OutletType == 2).FirstOrDefault();
            if (mainStore != null)
            {
                hfMainStoreId.Value = mainStore.CostCenterId.ToString();
                hfDefaultLocationId.Value = mainStore.DefaultStockLocationId.ToString();
            }

            ddlCostCenterFrom.DataSource = List;
            ddlCostCenterFrom.DataTextField = "CostCenter";
            ddlCostCenterFrom.DataValueField = "CostCenterId";
            ddlCostCenterFrom.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            if (List.Count > 1)
                ddlCostCenterFrom.Items.Insert(0, item);

            ddlCostCenterTo.DataSource = List;
            ddlCostCenterTo.DataTextField = "CostCenter";
            ddlCostCenterTo.DataValueField = "CostCenterId";
            ddlCostCenterTo.DataBind();
            if (List.Count > 1)
                ddlCostCenterTo.Items.Insert(0, item);

            //ddlSearchCostCenterFrom.DataSource = List;
            //ddlSearchCostCenterFrom.DataTextField = "CostCenter";
            //ddlSearchCostCenterFrom.DataValueField = "CostCenterId";
            //ddlSearchCostCenterFrom.DataBind();
            //ddlSearchCostCenterFrom.Items.Insert(0, item);

            //ddlSearchCostCenterTo.DataSource = List;
            //ddlSearchCostCenterTo.DataTextField = "CostCenter";
            //ddlSearchCostCenterTo.DataValueField = "CostCenterId";
            //ddlSearchCostCenterTo.DataBind();
            //ddlSearchCostCenterTo.Items.Insert(0, item);
        }
        private void LoadProductInfo()
        {
            //List<InvItemBO> productList = new List<InvItemBO>();
            //InvItemDA productDA = new InvItemDA();
            //productList = productDA.GetInvItemInfoByCategoryId(0, Convert.ToInt32(ddlCategory.SelectedValue));

            //ddlSalesOrderProduct.DataSource = productList;
            //ddlSalesOrderProduct.DataTextField = "Name";
            //ddlSalesOrderProduct.DataValueField = "ItemId";

            //ddlSalesOrderProduct.DataBind();
            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //ddlSalesOrderProduct.Items.Insert(0, item);

            //hfProductForSales.Value = JsonConvert.SerializeObject(productList);
        }
        private void LoadBillNumber()
        {
            //PMSalesDetailsDA entityDA = new PMSalesDetailsDA();
            //ddlBillNumber.DataSource = entityDA.GetAllSalesInformation();
            //ddlBillNumber.DataTextField = "BillNumber";
            //ddlBillNumber.DataValueField = "SalesId";
            //ddlBillNumber.DataBind();

            //ddlBillNumber.Items.Insert(0, new ListItem(hmUtility.GetDropDownFirstValue(), "0"));
        }

        [WebMethod]
        public static List<RestaurantBillBO> GetBillNoByText(string searchTerm)
        {
            HMCommonDA commonDA = new HMCommonDA();
            RestaurentBillDA rrDA = new RestaurentBillDA();
            List<RestaurantBillBO> reservationInfoList = new List<RestaurantBillBO>();
            reservationInfoList = rrDA.GetBillNoByText(searchTerm);

            return reservationInfoList;
        }

        private void LoadProductOutFor()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("ProductOutFor");

            ddlOutType.DataSource = fields;
            ddlOutType.DataTextField = "Description";
            ddlOutType.DataValueField = "FieldValue";
            ddlOutType.DataBind();

            ddlSearchProductOutFor.DataSource = fields;
            ddlSearchProductOutFor.DataTextField = "Description";
            ddlSearchProductOutFor.DataValueField = "FieldValue";
            ddlSearchProductOutFor.DataBind();

            //ListItem itemR = new ListItem();
            //itemR.Value = "Requisition";
            //itemR.Text = "Requisition Wise Transfer";
            //ddlOutType.Items.Insert(0, itemR);
            //ddlSearchProductOutFor.Items.Insert(0, itemR);

            //ListItem itemRe = new ListItem();
            //itemRe.Value = "StockTransfer";
            //itemRe.Text = "Stock Transfer";
            //ddlOutType.Items.Insert(1, itemRe);
            //ddlSearchProductOutFor.Items.Insert(1, itemRe);

            //ListItem itemSales = new ListItem();
            //itemSales.Value = "SalesOut";
            //itemSales.Text = "Pre Sales";
            //ddlOutType.Items.Insert(2, itemSales);
            //ddlSearchProductOutFor.Items.Insert(2, itemSales);

            //ListItem billing = new ListItem();
            //billing.Value = "Billing";
            //billing.Text = "Billing";
            //ddlOutType.Items.Insert(3, billing);
            //ddlSearchProductOutFor.Items.Insert(3, billing);

            ddlOutType.Items.Insert(0, new ListItem(hmUtility.GetDropDownFirstValue(), "0"));
            ddlSearchProductOutFor.Items.Insert(0, new ListItem(hmUtility.GetDropDownFirstAllValue(), "All"));

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
        private void LoadStockBy()
        {
            //List<InvUnitHeadBO> headListBO = new List<InvUnitHeadBO>();
            //InvUnitHeadDA da = new InvUnitHeadDA();
            //headListBO = da.GetInvUnitHeadInfo();

            //ddlStockBy.DataSource = headListBO;
            //ddlStockBy.DataTextField = "HeadName";
            //ddlStockBy.DataValueField = "UnitHeadId";
            //ddlStockBy.DataBind();

            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //ddlStockBy.Items.Insert(0, item);
        }
        private void LoadProduct()
        {
            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //ddlProduct.Items.Insert(0, item);
        }
        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }
        public void LoadQuotation()
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            SalesTransferDA DA = new SalesTransferDA();
            var List = DA.GetQuotationForItemOut();

            ddlQuotation.DataSource = List;
            ddlQuotation.DataTextField = "QuotationNo";
            ddlQuotation.DataValueField = "QuotationId";
            ddlQuotation.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            ddlQuotation.Items.Insert(0, item);

        }
        [WebMethod]
        public static RequisitionViewBO LoadProductByRequisitionId(int requisitionId)
        {
            RequisitionViewBO viewBo = new RequisitionViewBO();
            PMRequisitionDA entityDA = new PMRequisitionDA();

            List<PMRequisitionDetailsBO> requisitionBO = new List<PMRequisitionDetailsBO>();
            viewBo.Requisition = entityDA.GetPMRequisitionInfoByID(requisitionId);
            requisitionBO = entityDA.GetPMRequisitionDetailsByIDForOut(requisitionId, viewBo.Requisition.ToLocationId);
            requisitionBO = requisitionBO.Where(r => r.DeliverStatus != "Full").ToList();

            viewBo.RequisitionDetails = requisitionBO;

            return viewBo;
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
        public static List<InvItemAutoSearchBO> ItemSearch(string searchTerm, int companyId, int projectId, int costCenterId, int categoryId, int locationId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemForAutoSearchWithoutSupplier("Transfer", searchTerm, companyId, projectId, costCenterId, ConstantHelper.CustomerSupplierAutoSearch.SupplierItem.ToString(), categoryId, locationId);

            return itemInfo;
        }
        [WebMethod]
        public static List<SerialDuplicateBO> SerialSearch(string serialNumber, int companyId, int projectId, int locationId, int itemId)
        {
            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();

            PMProductOutDA outDA = new PMProductOutDA();
            serial = outDA.GetCompanyProjectWiseAvailableSerialForAutoSearch(serialNumber, companyId, projectId, locationId, itemId);

            return serial;
        }
        [WebMethod]
        public static ReturnInfo SerialAvailabilityCheck(string FromLocationId,
                                                    List<PMProductOutSerialInfoBO> ItemSerialDetails)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isProductOutApproval = new HMCommonSetupBO();
            string serialId = string.Empty, message = string.Empty;
            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();
            PMProductOutDA outDA = new PMProductOutDA();
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                isProductOutApproval = commonSetupDA.GetCommonConfigurationInfo("IsProductOutApprovalEnable", "IsProductOutApprovalEnable");

                foreach (PMProductOutSerialInfoBO srl in ItemSerialDetails.Where(s => s.OutSerialId == 0))
                {
                    if (serialId != string.Empty)
                    {
                        serialId += "," + srl.SerialNumber;
                    }
                    else
                    {
                        serialId = srl.SerialNumber;
                    }
                }

                serial = outDA.SerialAvailabilityCheck(serialId, Convert.ToInt64(FromLocationId));

                foreach (SerialDuplicateBO p in serial)
                {
                    if (message != "")
                        message = ", " + p.ItemName + "(" + p.SerialNumber + ")";
                    else
                        message = p.ItemName + "(" + p.SerialNumber + ")";
                }

                if (!string.IsNullOrEmpty(message))
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo("This Item Serial Does Not Exists. " + message, AlertType.Error);
                    return rtninfo;
                }
                else
                {
                    rtninfo.IsSuccess = true;
                    return rtninfo;
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
        public static ReturnInfo SaveItemOutOrder(PMProductOutBO ProductOut,
                                                  List<PMProductOutDetailsBO> TransferItemAdded,
                                                  List<PMProductOutDetailsBO> TransferItemDeleted,
                                                  List<PMProductOutSerialInfoBO> ItemSerialDetails,
                                                  List<PMProductOutSerialInfoBO> DeletedSerialzableProduct)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            PMProductOutDA outDA = new PMProductOutDA();
            int outId = 0;
            bool isApprovalProcessEnable = true;
            string serialId = string.Empty, message = string.Empty, itemName = string.Empty;

            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();
            List<PMProductOutDetailsBO> editedReceivedDetails = new List<PMProductOutDetailsBO>();
            HMCommonSetupBO isProductOutApproval = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                isProductOutApproval = commonSetupDA.GetCommonConfigurationInfo("IsProductOutApprovalEnable", "IsProductOutApprovalEnable");

                foreach (PMProductOutSerialInfoBO srl in ItemSerialDetails.Where(s => s.OutSerialId == 0))
                {
                    if (serialId != string.Empty)
                    {
                        serialId += "," + srl.SerialNumber;
                    }
                    else
                    {
                        serialId = srl.SerialNumber;
                    }
                }

                serial = outDA.SerialAvailabilityCheck(serialId, Convert.ToInt64(ProductOut.FromLocationId));

                foreach (SerialDuplicateBO p in serial)
                {
                    if (message != "")
                        message = ", " + p.ItemName + "(" + p.SerialNumber + ")";
                    else
                        message = p.ItemName + "(" + p.SerialNumber + ")";
                }

                if (!string.IsNullOrEmpty(message))
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo("These Item Serial Does Not Exists. " + message, AlertType.Error);
                    return rtninfo;
                }

                if (ProductOut.OutId == 0)
                {
                    ProductOut.OutDate = DateTime.Now;
                    ProductOut.CreatedBy = userInformationBO.UserInfoId;
                    ProductOut.Status = HMConstants.ApprovalStatus.Pending.ToString();

                    if (Convert.ToInt32(isProductOutApproval.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedEnable))
                    {
                        ProductOut.Status = HMConstants.ApprovalStatus.Pending.ToString();
                        isApprovalProcessEnable = true;
                    }
                    else if (Convert.ToInt32(isProductOutApproval.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedDisable))
                    {
                        ProductOut.Status = HMConstants.ApprovalStatus.Approved.ToString();
                        isApprovalProcessEnable = false;
                    }

                    rtninfo.IsSuccess = outDA.SaveProductOutInfo(ProductOut, TransferItemAdded, ItemSerialDetails, isApprovalProcessEnable, out outId);

                    if (rtninfo.IsSuccess)
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.PMProductOut.ToString(), outId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductReceive));

                    }

                }
                else
                {

                    ProductOut.LastModifiedBy = userInformationBO.UserInfoId;
                    outId = ProductOut.OutId;

                    List<PMProductReceivedDetailsBO> EditedReceivedDetails = new List<PMProductReceivedDetailsBO>();

                    editedReceivedDetails = (from pod in TransferItemAdded where pod.OutDetailsId > 0 select pod).ToList();
                    TransferItemAdded = (from pod in TransferItemAdded where pod.OutDetailsId == 0 select pod).ToList();

                    ItemSerialDetails = (from srl in ItemSerialDetails where srl.OutSerialId == 0 select srl).ToList();

                    rtninfo.IsSuccess = outDA.UpdateProductOutInfo(ProductOut, TransferItemAdded, editedReceivedDetails, TransferItemDeleted,
                                                                   ItemSerialDetails, DeletedSerialzableProduct);

                    if (rtninfo.IsSuccess)
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.PMProductOut.ToString(), outId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductReceive));

                    }
                }

                if (!rtninfo.IsSuccess)
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
        public static GridViewDataNPaging<PMProductOutBO, GridPaging> SearchOutOrder(string outType, DateTime? fromDate, DateTime? toDate,
                                                                                        string issueNumber, string status,
                                                                                        int gridRecordsCount, int pageNumber,
                                                                                        int isCurrentOrPreviousPage
                                                                                       )
        {

            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<PMProductOutBO, GridPaging> myGridData = new GridViewDataNPaging<PMProductOutBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (fromDate == null) fromDate = DateTime.Now;
            if (toDate == null) toDate = DateTime.Now;

            PMProductOutDA receiveDA = new PMProductOutDA();
            List<PMProductOutBO> orderList = new List<PMProductOutBO>();

            orderList = receiveDA.GetProductOutForSearch(outType, fromDate, toDate, issueNumber, status, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords).Where(x => x.IssueType != "FixedAsset").ToList();

            myGridData.GridPagingProcessing(orderList, totalRecords);
            return myGridData;
        }


        [WebMethod]
        public static OutOrderViewBO EditItemOut(string issueType, int outId, int requisitionOrSalesId)
        {
            OutOrderViewBO viewBo = new OutOrderViewBO();
            PMProductOutDA orderDa = new PMProductOutDA();

            viewBo.ProductOut = orderDa.GetProductOutById(outId);
            viewBo.ProductSerialInfo = orderDa.GetItemOutSerialById(outId);

            if (requisitionOrSalesId > 0 && viewBo.ProductOut.ProductOutFor == "Requisition")
            {
                viewBo.ProductOutDetails = orderDa.GetItemOutDetailsFromRequisitionByOutId(outId, requisitionOrSalesId);
            }
            else if (requisitionOrSalesId > 0 && viewBo.ProductOut.ProductOutFor == "SalesOut")
            {
                viewBo.ProductOutDetails = orderDa.GetItemOutDetailsFromQuotationByOutId(outId, requisitionOrSalesId);
            }
            else if (requisitionOrSalesId > 0 && viewBo.ProductOut.ProductOutFor == "Billing")
            {
                viewBo.ProductOutDetails = orderDa.GetItemOutDetailsFromBillingByOutId(outId, requisitionOrSalesId);
            }
            else
            {
                viewBo.ProductOutDetails = orderDa.GetItemOutDetailsByOutId(outId);
            }

            return viewBo;
        }
        [WebMethod]
        public static ReturnInfo OutOrderApproval(string productOutFor, int outId, string approvedStatus, int requisitionOrSalesId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductOutDA orderDa = new PMProductOutDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDa.OutOrderApproval(productOutFor, outId, approvedStatus, requisitionOrSalesId, userInformationBO.UserInfoId);

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isTransferProductReceiveDisable = new HMCommonSetupBO();

                isTransferProductReceiveDisable = commonSetupDA.GetCommonConfigurationInfo("IsTransferProductReceiveDisable", "IsTransferProductReceiveDisable");

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;

                    if (approvedStatus == HMConstants.ApprovalStatus.Checked.ToString())
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                    }
                    else
                    {
                        if (isTransferProductReceiveDisable.SetupValue == "1")
                        {
                            rtninf.IsSuccess = orderDa.ItemReceiveOutOrder(productOutFor, outId, requisitionOrSalesId, userInformationBO.UserInfoId);
                        }
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    }

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.PMProductOut.ToString(), outId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductOut));


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
        public static ReturnInfo OutOrderDelete(string issueType, int outId, string approvedStatus, int createdBy)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductOutDA orderDa = new PMProductOutDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDa.OutOrderDelete(issueType, outId, approvedStatus, createdBy, userInformationBO.UserInfoId);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.PMProductOut.ToString(), outId,
                              ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductOut));

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
        public static List<PMRequisitionBO> LoadNotReceivedRequisitionOrder()
        {
            PMRequisitionDA entityDA = new PMRequisitionDA();
            List<PMRequisitionBO> requisitionList = new List<PMRequisitionBO>();
            requisitionList = entityDA.GetApprovedNNotDeliveredRequisitionForOut();

            return requisitionList;
        }
        [WebMethod]
        public static List<QuotationViewDetailsBO> GetQuotationDetailsByQuotationId(long quotationId, int costCenterId, int locationId)
        {
            List<QuotationViewDetailsBO> quotationDetailsList = new List<QuotationViewDetailsBO>();
            SalesTransferDA DA = new SalesTransferDA();
            quotationDetailsList = DA.GetQuotationDetailsByQuotationId(quotationId, costCenterId, locationId);
            return quotationDetailsList;
        }

        [WebMethod]
        public static List<BillingViewDetailsBO> GetBillingDetailsByBillingId(long BillingId, int costCenterId, int locationId)
        {
            List<BillingViewDetailsBO> quotationDetailsList = new List<BillingViewDetailsBO>();
            SalesTransferDA DA = new SalesTransferDA();
            quotationDetailsList = DA.GetBillingDetailsByBillingId(BillingId, costCenterId, locationId);
            return quotationDetailsList;
        }

        [WebMethod]
        public static List<InvItemAttributeBO> GetInvItemAttributeByItemIdAndAttributeType(int ItemId, string attributeType)
        {
            InvItemAttributeDA DA = new InvItemAttributeDA();
            List<InvItemAttributeBO> InvItemAttributeBOList = new List<InvItemAttributeBO>();
            InvItemAttributeBOList = DA.GetInvItemAttributeByItemIdAndAttributeType(ItemId, attributeType);

            return InvItemAttributeBOList;
        }
        [WebMethod]
        public static InvItemStockInformationBO GetInvItemStockInfoByItemAndAttributeId(int companyId, int projectId, int itemId, int colorId, int sizeId, int styleId, int locationId)
        {
            SalesTransferDA DA = new SalesTransferDA();
            InvItemStockInformationBO StockInformation = new InvItemStockInformationBO();
            StockInformation = DA.GetInvItemStockInfoByItemAndAttributeId(companyId, projectId, itemId, colorId, sizeId, styleId, locationId);

            return StockInformation;
        }

    }
}