using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.LCManagement;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class ItemReceive : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        public ItemReceive()
            : base("ItemReceiveInfo")
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                companyProjectUserControl.ddlFirstValueVar = "select";
                //hfId.Value = "0";
                hfParentDoc.Value = "0";
                FileUpload();
                LoadSupplierInfo();
                LoadAllCostCentreInfo();
                LoadCurrency();
                LoadLocalCurrencyId();
                LoadCategory();
                LoadPurchaseOrder();
                CheckPermission();
                LoadLCNumber();
                LoadIsItemAttributeEnable();
                LoadAccountHead();
            }
        }
        private void LoadAccountHead()
        {
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            //ddlAccountHead.DataSource = nodeMatrixDA.GetNodeMatrixInfo();
            ddlAccountHead.DataSource = nodeMatrixDA.GetNodeMatrixInfoByCustomString("WHERE CHARINDEX('.4.', Hierarchy) = 1 AND IsTransactionalHead = 1");
            ddlAccountHead.DataTextField = "HeadWithCode";
            ddlAccountHead.DataValueField = "NodeId";
            ddlAccountHead.DataBind();

            ddlPMAccountHead.DataSource = nodeMatrixDA.GetNodeMatrixInfoByCustomString("WHERE CHARINDEX('.4.', Hierarchy) = 1 AND IsTransactionalHead = 1");
            ddlPMAccountHead.DataTextField = "HeadWithCode";
            ddlPMAccountHead.DataValueField = "NodeId";
            ddlPMAccountHead.DataBind();
            

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlAccountHead.Items.Insert(0, itemNodeId);
            ddlPMAccountHead.Items.Insert(0, itemNodeId);
        }
        #region Data Initialize
        private void LoadIsItemAttributeEnable()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            List<HMCommonSetupBO> setUpBOList = new List<HMCommonSetupBO>();
            setUpBOList = commonSetupDA.GetAllCommonConfigurationInfo();


            setUpBO = setUpBOList.Where(x => x.TypeName == "IsItemAttributeEnable" && x.SetupName == "IsItemAttributeEnable").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsItemAttributeEnable.Value = setUpBO.SetupValue;
            }
        }
        private void LoadSupplierInfo()
        {
            PMSupplierDA entityDA = new PMSupplierDA();
            List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();

            supplierBOList = entityDA.GetPMSupplierInfo();

            ddlSupplier.DataSource = supplierBOList;
            ddlSupplier.DataTextField = "Name";
            ddlSupplier.DataValueField = "SupplierId";
            ddlSupplier.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSupplier.Items.Insert(0, item);

            ddlSearchSupplier.DataSource = supplierBOList;
            ddlSearchSupplier.DataTextField = "Name";
            ddlSearchSupplier.DataValueField = "SupplierId";
            ddlSearchSupplier.DataBind();

            ListItem itemSearch = new ListItem();
            itemSearch.Value = "0";
            itemSearch.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSearchSupplier.Items.Insert(0, itemSearch);
        }

        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }
        private void LoadAllCostCentreInfo()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            //costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfo();
            costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId)
                                    .Where(o => o.OutletType == 2 && o.CostCenterType == "Inventory").ToList();
            ddlPurchaseOrderCostcenter.DataSource = costCentreTabBOList;
            ddlPurchaseOrderCostcenter.DataTextField = "CostCenter";
            ddlPurchaseOrderCostcenter.DataValueField = "CostCenterId";
            ddlPurchaseOrderCostcenter.DataBind();

            //costCentreTabBOList = costCentreTabBOList.Where(o => o.OutletType == 2).ToList();

            //ddlCostCentre.DataSource = costCentreTabBOList;
            //ddlCostCentre.DataTextField = "CostCenter";
            //ddlCostCentre.DataValueField = "CostCenterId";
            //ddlCostCentre.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();



            ddlCostCenterSearch.DataSource = costCentreTabBOList;
            ddlCostCenterSearch.DataTextField = "CostCenter";
            ddlCostCenterSearch.DataValueField = "CostCenterId";
            ddlCostCenterSearch.DataBind();
            if (costCentreTabBOList.Count > 1)
            {
                //ddlCostCentre.Items.Insert(0, item);
                ddlCostCenterSearch.Items.Insert(0, item);
            }
        }

        [WebMethod]
        public static List<CostCentreTabBO> LoadReceiveStoreByCompanyId(int companyId)
        {

            HMUtility hmUtility = new HMUtility();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            //costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfo();
            costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfoByUserGroupIdNCompanyId(userInformationBO.UserGroupId, companyId)
                                    .Where(o => o.OutletType == 2 && o.CostCenterType == "Inventory").ToList();
            return costCentreTabBOList;

        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("LocalNUsd");

            ddlCurrency.DataSource = currencyListBO;
            ddlCurrency.DataTextField = "CurrencyName";
            ddlCurrency.DataValueField = "CurrencyId";
            ddlCurrency.DataBind();

            hfCurrencyObj.Value = JsonConvert.SerializeObject(currencyListBO);
        }
        private void LoadLocalCurrencyId()
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetLocalCurrencyInfo();
            hfDefaultCurrencyId.Value = commonCurrencyBO.CurrencyId.ToString();

            ddlCurrency.SelectedValue = commonCurrencyBO.CurrencyId.ToString();

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

        private void LoadPurchaseOrder()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            PMPurchaseOrderDA orderDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();
            orderList = orderDA.GetApprovedPMPurchaseOrderInfo(userInformationBO.UserGroupId, "Product");

            hfPurchaseOrderObj.Value = JsonConvert.SerializeObject(orderList);
            
            ddlPurchaseOrderNumber.DataSource = orderList;
            ddlPurchaseOrderNumber.DataTextField = "PONumber";
            ddlPurchaseOrderNumber.DataValueField = "POrderId";
            ddlPurchaseOrderNumber.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlPurchaseOrderNumber.Items.Insert(0, item);

        }
        private void LoadLCNumber()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            LCInformationDA lcDA = new LCInformationDA();
            var lcList = lcDA.GetApprovedLCInformationInfoForReceive(userInformationBO.UserGroupId);
            var ConvertedList = (from a in lcList select new { a.LCId, a.LCNumber, a.CostCenterId, a.SupplierId, POType = "LC" }).ToList();
            ddlLCNumber.DataSource = lcList;
            ddlLCNumber.DataTextField = "LCNumber";
            ddlLCNumber.DataValueField = "LCId";
            ddlLCNumber.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlLCNumber.Items.Insert(0, item);

            hfLcInfoObj.Value = JsonConvert.SerializeObject(ConvertedList);

        }
        #endregion Data Initialize
        private void FileUpload()
        {
            Random rd = new Random();
            int seatingId = rd.Next(100000, 999999);
            RandomDocId.Value = seatingId.ToString();
            tempDocId.Value = seatingId.ToString();
            HttpContext.Current.Session["ReceiveOrderDocId"] = RandomDocId.Value;
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            flashUpload.QueryParameters = "ReceiveOrderDocId=" + Server.UrlEncode(RandomDocId.Value);
        }

        //[WebMethod]
        //public static List<DocumentsBO> GetUploadedDocumentsByWebMethod(int OwnerId, string docType)
        //{
        //    DocumentsDA docDA = new DocumentsDA();
        //    List<DocumentsBO> docBO = new List<DocumentsBO>();

        //    docBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("ReceiveOrderDocuments", OwnerId);

        //    //DocumentsDA docDA = new DocumentsDA();
        //    //var docList = docDA.GetDocumentsInfoByDocCategoryAndOwnerId(docType, OwnerId);
        //    docBO = new HMCommonDA().GetDocumentListWithIcon(docBO);
        //    return docBO;
        //}
        #region Web Method For Ajax Call

        [WebMethod]
        public static CommonCurrencyConversionBO LoadCurrencyConversionRate(int currencyId)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyConversionBO conversionBO = new CommonCurrencyConversionBO();
            conversionBO = commonCurrencyDA.GetCurrencyConversionRate(currencyId);
            return conversionBO;
        }

        [WebMethod]
        public static List<InvLocationBO> StoreLocationByCostCenter(int costCenterId)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvItemLocationByCostCenter(costCenterId);

            return location;
        }

        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemSearch(string searchTerm, int companyId, int projectId, int costCenterId, int locationId, int categoryId, int supplierId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetCompanyProjectWiseItemDetailsForAutoSearch(searchTerm, companyId, projectId, costCenterId, ConstantHelper.CustomerSupplierAutoSearch.SupplierItem.ToString(), categoryId, supplierId, locationId);

            return itemInfo;
        }
        [WebMethod]
        public static InvItemStockInformationBO GetInvItemStockInfoByItemAndAttributeId(int itemId, int colorId, int sizeId, int styleId, int locationId)
        {
            InvItemDA DA = new InvItemDA();
            InvItemStockInformationBO StockInformation = new InvItemStockInformationBO();
            StockInformation = DA.GetInvItemStockInfoByItemAndAttributeId(itemId, colorId, sizeId, styleId, locationId);

            return StockInformation;
        }
        [WebMethod]
        public static List<PMPurchaseOrderDetailsBO> LoadItemFromPurchaseOrderForReceivedByPurchaseOrderId(int porderId, string poType, int supplierId)
        {
            List<PMPurchaseOrderDetailsBO> productList = new List<PMPurchaseOrderDetailsBO>();

            PMPurchaseOrderDA entityDA = new PMPurchaseOrderDA();
            productList = entityDA.GetAvailableItemForPOrderId(porderId, poType, supplierId);

            return productList;
        }
        [WebMethod]
        public static ReturnInfo SerialAvailabilityCheck(List<PMProductSerialInfoBO> ItemSerialDetails)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isProductOutApproval = new HMCommonSetupBO();
            string serialId = string.Empty, message = string.Empty;
            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();
            PMProductReceivedDA receiveDA = new PMProductReceivedDA();
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                isProductOutApproval = commonSetupDA.GetCommonConfigurationInfo("IsProductOutApprovalEnable", "IsProductOutApprovalEnable");


                foreach (PMProductSerialInfoBO srl in ItemSerialDetails)
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

                serial = receiveDA.DuplicateSerialCheck(serialId);

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
                    rtninfo.AlertMessage = CommonHelper.AlertInfo("This Item Serial Exists. " + message, AlertType.Error);
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
        public static ReturnInfo SavePurchaseWiseReceiveOrder(PMProductReceivedBO ProductReceive,
                                                              List<PMProductReceivedDetailsBO> ReceiveOrderItem,
                                                              List<PMProductReceivedDetailsBO> ReceiveOrderItemDeleted,
                                                              List<PMProductSerialInfoBO> ItemSerialDetails,
                                                              List<PMProductSerialInfoBO> DeletedSerialzableProduct, int randomDocId, string deletedDoc, 
                                                              List<OverheadExpensesBO> AddedOverheadExpenses,
                                                              List<OverheadExpensesBO> EditedOverheadExpenses,
                                                              List<OverheadExpensesBO> AddedPaymentMethodInfos,
                                                              List<OverheadExpensesBO> EditedPaymentMethodInfos
                                                              )
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            ReturnInfo rtninfo = new ReturnInfo();
            PMProductReceivedDA receiveDA = new PMProductReceivedDA();
            int receiveId = 0;
            int OwnerIdForDocuments = 0;
            string serialId = string.Empty, message = string.Empty, itemName = string.Empty;
            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();
            bool isApprovalProcessEnable = true;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO receivedProductApprovalEnable = new HMCommonSetupBO();
                receivedProductApprovalEnable = commonSetupDA.GetCommonConfigurationInfo("IsReceivedProductApprovalEnable", "IsReceivedProductApprovalEnable");


                foreach (PMProductSerialInfoBO srl in ItemSerialDetails)
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

                serial = receiveDA.DuplicateSerialCheck(serialId);

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
                    rtninfo.AlertMessage = CommonHelper.AlertInfo("These Item Serial Already Exists. " + message, AlertType.Error);
                    return rtninfo;
                }

                if (ProductReceive.ReceivedId == 0)
                {
                    if (Convert.ToInt32(receivedProductApprovalEnable.SetupValue) == Convert.ToInt32(HMConstants.ProductReceiveTemplate.ApprovalEnable))
                    {
                        ProductReceive.Status = HMConstants.ApprovalStatus.Pending.ToString();
                        isApprovalProcessEnable = true;
                    }
                    else if (Convert.ToInt32(receivedProductApprovalEnable.SetupValue) == Convert.ToInt32(HMConstants.ProductReceiveTemplate.ApprovalDisable))
                    {
                        ProductReceive.Status = HMConstants.ApprovalStatus.Approved.ToString();
                        isApprovalProcessEnable = false;
                    }

                    ProductReceive.ReceivedDate = DateTime.Now;
                    ProductReceive.CreatedBy = userInformationBO.UserInfoId;
                    string TransactionNo = "";
                    string TransactionType = "";
                    string ApproveStatus = "";
                    //ProductReceive.Status = HMConstants.ApprovalStatus.Pending.ToString();

                    rtninfo.IsSuccess = receiveDA.SaveProductReceiveInfo(ProductReceive, ReceiveOrderItem, ItemSerialDetails, isApprovalProcessEnable, AddedOverheadExpenses, AddedPaymentMethodInfos, out receiveId, out TransactionNo, out TransactionType, out ApproveStatus);

                    if (rtninfo.IsSuccess)
                    {
                        if (rtninfo.IsSuccess)
                            OwnerIdForDocuments = Convert.ToInt32(receiveId);
                        long RandomId = HttpContext.Current.Session["ReceiveOrderDocId"] != null ? Convert.ToInt64(HttpContext.Current.Session["ReceiveOrderDocId"]) : 0;

                        DocumentsDA documentsDA = new DocumentsDA();
                        string s = deletedDoc;
                        string[] DeletedDocList = s.Split(',');
                        for (int i = 0; i < DeletedDocList.Length; i++)
                        {
                            DeletedDocList[i] = DeletedDocList[i].Trim();
                            Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
                        }
                        Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(RandomId));


                        rtninfo.PrimaryKeyValue = receiveId.ToString();
                        rtninfo.TransactionNo = TransactionNo;
                        rtninfo.TransactionType = TransactionType;
                        rtninfo.TransactionStatus = ApproveStatus;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.ProductReceive.ToString(), receiveId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductReceive));

                    }

                }
                else
                {
                    string TransactionNo = "";
                    string TransactionType = "";
                    string ApproveStatus = "";

                    ProductReceive.LastModifiedBy = userInformationBO.UserInfoId;
                    receiveId = ProductReceive.ReceivedId;

                    List<PMProductReceivedDetailsBO> EditedReceivedDetails = new List<PMProductReceivedDetailsBO>();

                    EditedReceivedDetails = (from pod in ReceiveOrderItem where pod.ReceiveDetailsId > 0 select pod).ToList();
                    ReceiveOrderItem = (from pod in ReceiveOrderItem where pod.ReceiveDetailsId == 0 select pod).ToList();

                    ItemSerialDetails = (from srl in ItemSerialDetails where srl.SerialId == 0 select srl).ToList();
                    rtninfo.IsSuccess = receiveDA.UpdateProductReceiveInfo(ProductReceive, ReceiveOrderItem, EditedReceivedDetails, AddedOverheadExpenses, AddedPaymentMethodInfos, ReceiveOrderItemDeleted, ItemSerialDetails, DeletedSerialzableProduct, out TransactionNo, out TransactionType, out ApproveStatus);

                    if (rtninfo.IsSuccess)
                    {
                        if (rtninfo.IsSuccess)
                            OwnerIdForDocuments = Convert.ToInt32(receiveId);
                        long RandomId = HttpContext.Current.Session["ReceiveOrderDocId"] != null ? Convert.ToInt64(HttpContext.Current.Session["ReceiveOrderDocId"]) : 0;

                        DocumentsDA documentsDA = new DocumentsDA();
                        string s = deletedDoc;
                        string[] DeletedDocList = s.Split(',');
                        for (int i = 0; i < DeletedDocList.Length; i++)
                        {
                            DeletedDocList[i] = DeletedDocList[i].Trim();
                            Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
                        }
                        Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(RandomId));

                        rtninfo.PrimaryKeyValue = receiveId.ToString();
                        rtninfo.TransactionNo = TransactionNo;
                        rtninfo.TransactionType = TransactionType;
                        rtninfo.TransactionStatus = ApproveStatus;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.ProductReceive.ToString(), receiveId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductReceive));

                    }
                }
                if (!rtninfo.IsSuccess)
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                Random rd = new Random();
                int randomId = rd.Next(100000, 999999);
                HttpContext.Current.Session["ReceiveOrderDocId"] = randomId;
                rtninfo.Data = randomId;
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }




        [WebMethod]
        public static GridViewDataNPaging<PMProductReceivedBO, GridPaging> SearchReceiveOrder(int companyId, int projectId, string receiveType, DateTime? fromDate, DateTime? toDate,
                                                                                          string receiveNumber, string status, int? costCenterId,
                                                                                          int? supplierId, int gridRecordsCount, int pageNumber,
                                                                                          int isCurrentOrPreviousPage
                                                                                         )
        {

            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<PMProductReceivedBO, GridPaging> myGridData = new GridViewDataNPaging<PMProductReceivedBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (fromDate == null) fromDate = DateTime.Now;
            if (toDate == null) toDate = DateTime.Now;

            PMProductReceivedDA receiveDA = new PMProductReceivedDA();
            List<PMProductReceivedBO> orderList = new List<PMProductReceivedBO>();

            orderList = receiveDA.GetProductreceiveInfo(companyId, projectId, receiveType, fromDate, toDate, receiveNumber, status, costCenterId, supplierId, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(orderList, totalRecords);
            return myGridData;
        }

        [WebMethod]
        public static ReceiveOrderViewBO EditReceiveOrder(int receivedId, int porderId)
        {
            ReceiveOrderViewBO viewBo = new ReceiveOrderViewBO();
            PMProductReceivedDA orderDetailDA = new PMProductReceivedDA();

            viewBo.ProductReceived = orderDetailDA.GetProductreceiveInfo(receivedId);

            viewBo.ProductReceivedDetails = orderDetailDA.GetProductReceiveDetailsById(receivedId);
            viewBo.ProductSerialInfo = orderDetailDA.GetProductReceiveSerialById(receivedId);
            viewBo.PaymentInformationList = orderDetailDA.GetPaymentInformationListByReceiveId(receivedId);
            if (viewBo.ProductReceived.ReceiveType == "Purchase" || viewBo.ProductReceived.ReceiveType == "AdHoc")
            {
                viewBo.OverheadExpenseInfoList = orderDetailDA.GetOverheadExpenseForProductReceiveByReceivedId(receivedId);
            }
            if (viewBo.ProductReceived.ReceiveType == "Purchase")
            {
                viewBo.ProductReceivedDetailsSummary = orderDetailDA.GetProductDetailsForReceiveFromPurchaseByReceiveId(receivedId, porderId);
            }
            else if(viewBo.ProductReceived.ReceiveType == "LC")
                viewBo.ProductReceivedDetailsSummary = orderDetailDA.GetProductDetailsForReceiveFromLCByReceiveId(receivedId, porderId);
            return viewBo;
        }



        [WebMethod]
        public static ReturnInfo ReceiveOrderApproval(string receiveType, int receivedId, string approvedStatus, int porderId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductReceivedDA orderDetailDA = new PMProductReceivedDA();
            string TransactionNo = "";
            string TransactionType = "";
            string ApproveStatus = "";

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDetailDA.ReceiveOrderApproval(receiveType, receivedId, approvedStatus, porderId, userInformationBO.UserInfoId, out TransactionNo, out TransactionType, out ApproveStatus);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    rtninf.PrimaryKeyValue = receivedId.ToString();
                    rtninf.TransactionNo = TransactionNo;
                    rtninf.TransactionType = TransactionType;
                    rtninf.TransactionStatus = ApproveStatus;

                    if (approvedStatus == HMConstants.ApprovalStatus.Checked.ToString())
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                    }
                    else
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    }

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ProductReceive.ToString(), receivedId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductReceive));


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
        public static ReturnInfo ReceiveOrderDelete(string receiveType, int receivedId, string approvedStatus, int createdBy)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductReceivedDA orderDetailDA = new PMProductReceivedDA();
            string TransactionNo = "";
            string TransactionType = "";
            string ApproveStatus = "";

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDetailDA.ReceiveOrderDelete(receiveType, receivedId, approvedStatus, createdBy, userInformationBO.UserInfoId, out TransactionNo, out TransactionType, out ApproveStatus);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    rtninf.PrimaryKeyValue = receivedId.ToString();
                    rtninf.TransactionNo = TransactionNo;
                    rtninf.TransactionType = TransactionType;
                    rtninf.TransactionStatus = ApproveStatus;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.ProductReceive.ToString(), receivedId,
                              ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductReceive));

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
        public static List<PMPurchaseOrderBO> LoadNotReceivedPurchaseOrder()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            PMPurchaseOrderDA orderDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();
            orderList = orderDA.GetApprovedPMPurchaseOrderInfo(userInformationBO.UserGroupId, "Product");

            PMPurchaseOrderBO po = new PMPurchaseOrderBO();
            po.POrderId = 0;
            po.PONumber = "Ad Hoc Purchase";

            List<PMPurchaseOrderBO> orderList1 = new List<PMPurchaseOrderBO>();
            orderList1.Add(po);
            orderList1.AddRange(orderList);

            return orderList;
        }

        [WebMethod]
        public static List<DocumentsBO> GetUploadedDocByWebMethod(int randomId, int id, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }
            List<DocumentsBO> docList = new List<DocumentsBO>();
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("ReceiveOrderDocuments", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("ReceiveOrderDocuments", (int)id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));
            foreach (DocumentsBO dc in docList)
            {

                if (dc.DocumentType == "Image")
                    dc.Path = (dc.Path + dc.Name);

                dc.Name = dc.Name.Remove(dc.Name.LastIndexOf('.'));
            }
            docList = new HMCommonDA().GetDocumentListWithIcon(docList).ToList();
            return docList;
        }

        [WebMethod]
        public static string LoadDealDocument(long id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("ReceiveOrderDocuments", id);
            //docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SalesDealFeedbackDocuments", id));
            //docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SalesQuotationDocuments", id));
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);

            string strTable = "";
            strTable += "<div style='color: White; background-color: #44545E;width:750px;'>";
            int counter = 0;
            foreach (DocumentsBO dr in docList)
            {
                if (dr.Extention == ".jpg")
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:250px; height:250px; float:left;padding:30px'>";
                    strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
                else
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:100px; height:100px; float:left;padding:30px'>";
                    strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img style='width: 100px; height: 100px;' src='" + dr.IconImage + "' alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
            }
            strTable += "</div>";
            if (strTable == "")
            {
                strTable = "<tr><td align='center'>No Record Available!</td></tr>";
            }
            return strTable;

        }


        [WebMethod]
        public static int ChangeRandomId()
        {
            Random rd = new Random();
            int randomId = rd.Next(100000, 999999);
            HttpContext.Current.Session["ReceiveOrderDocId"] = randomId;
            return randomId;
        }
        [WebMethod]
        public static List<InvItemAttributeBO> GetInvItemAttributeByItemIdAndAttributeType(int ItemId, string attributeType)
        {
            InvItemAttributeDA DA = new InvItemAttributeDA();
            List<InvItemAttributeBO> InvItemAttributeBOList = new List<InvItemAttributeBO>();
            InvItemAttributeBOList = DA.GetInvItemAttributeByItemIdAndAttributeType(ItemId, attributeType);


            return InvItemAttributeBOList;
        }
        #endregion
    }
}