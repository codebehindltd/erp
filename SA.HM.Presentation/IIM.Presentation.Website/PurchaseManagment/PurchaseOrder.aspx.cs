using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.GeneralLedger;
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

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class PurchaseOrder : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();

        public PurchaseOrder()
            : base("PurchaseInfo")
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadCompanyInformation();
                LoadGLCompany();
                LoadSupplierInfo();
                LoadAllCostCentreInfo();
                LoadCurrency();
                LoadLocalCurrencyId();
                LoadCategory();
                LoadRequisitionOrder();
                LoadOrderType();
                CheckPermission();
                LoadDescriptionConfiguratipn();
                IsAdminUser();
                LoadIsItemAttributeEnable();
            }
        }

        #region Data Initialize
        private void LoadCompanyInformation()
        {
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            if (files[0].CompanyId > 0)
            {
                hfDeliveryAddress.Value = files[0].CompanyAddress;
                txtDeliveryAddress.Text = files[0].CompanyAddress;
            }
        }
        private void LoadGLCompany()
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            List<GLCompanyBO> GLCompanyBOList = new List<GLCompanyBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.UserInfoId == 1)
            {
                GLCompanyBOList = entityDA.GetAllGLCompanyInfo();
            }
            else
            {
                GLCompanyBOList = entityDA.GetAllGLCompanyInfoByUserGroupId(userInformationBO.UserGroupId);
            }

            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            if (GLCompanyBOList != null)
            {
                if (GLCompanyBOList.Count == 1)
                {
                    hfIsSingleGLCompany.Value = "1";
                    companyList.Add(GLCompanyBOList[0]);
                    this.ddlGLCompanyId.DataSource = companyList;
                    this.ddlGLCompanyId.DataTextField = "Name";
                    this.ddlGLCompanyId.DataValueField = "CompanyId";
                    this.ddlGLCompanyId.DataBind();

                    this.ddlSrcGLCompanyId.DataSource = companyList;
                    this.ddlSrcGLCompanyId.DataTextField = "Name";
                    this.ddlSrcGLCompanyId.DataValueField = "CompanyId";
                    this.ddlSrcGLCompanyId.DataBind();                    
                }
                else
                {
                    hfIsSingleGLCompany.Value = "2";
                    this.ddlGLCompanyId.DataSource = GLCompanyBOList;
                    this.ddlGLCompanyId.DataTextField = "Name";
                    this.ddlGLCompanyId.DataValueField = "CompanyId";
                    this.ddlGLCompanyId.DataBind();

                    ListItem itemCompany = new ListItem();
                    itemCompany.Value = "0";
                    itemCompany.Text = hmUtility.GetDropDownFirstValue();
                    this.ddlGLCompanyId.Items.Insert(0, itemCompany);

                    this.ddlSrcGLCompanyId.DataSource = GLCompanyBOList;
                    this.ddlSrcGLCompanyId.DataTextField = "Name";
                    this.ddlSrcGLCompanyId.DataValueField = "CompanyId";
                    this.ddlSrcGLCompanyId.DataBind();

                    ListItem itemSrcCompany = new ListItem();
                    itemSrcCompany.Value = "0";
                    itemSrcCompany.Text = hmUtility.GetDropDownFirstAllValue();
                    this.ddlSrcGLCompanyId.Items.Insert(0, itemSrcCompany);
                }
            }
        }
        private void LoadDescriptionConfiguratipn()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            List<HMCommonSetupBO> setUpBOList = new List<HMCommonSetupBO>();
            setUpBOList = commonSetupDA.GetAllCommonConfigurationInfo();

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsItemDescriptionSuggestInPurchaseOrder" && x.SetupName == "IsItemDescriptionSuggestInPurchaseOrder").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsItemDescriptionSuggestInPurchaseOrder.Value = setUpBO.SetupValue.ToString();
            }
        }
        private void IsAdminUser()
        {
            Boolean IsAdminUser = false;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                IsAdminUser = true;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 12).Count() > 0)
                    {
                        IsAdminUser = true;
                    }
                }
            }
            #endregion

            if (IsAdminUser)
            {
                hfIsAdminUser.Value = "1";
            }
            else
            {
                hfIsAdminUser.Value = "0";
            }
        }
        private void LoadSupplierInfo()
        {
            PMSupplierDA entityDA = new PMSupplierDA();
            List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();

            supplierBOList = entityDA.GetPMSupplierInfo();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();

            ddlSearchSupplier.DataSource = supplierBOList;
            ddlSearchSupplier.DataTextField = "Name";
            ddlSearchSupplier.DataValueField = "SupplierId";
            ddlSearchSupplier.DataBind();
            ddlSearchSupplier.Items.Insert(0, item);
        }
        private void LoadAllCostCentreInfo()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId);

            ddlRequisitionCostcenter.DataSource = costCentreTabBOList;
            ddlRequisitionCostcenter.DataTextField = "CostCenter";
            ddlRequisitionCostcenter.DataValueField = "CostCenterId";
            ddlRequisitionCostcenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            if (costCentreTabBOList.Count > 1)
                ddlRequisitionCostcenter.Items.Insert(0, item);

            costCentreTabBOList = costCentreTabBOList.Where(o => o.OutletType == 2 && o.CostCenterType == "Inventory").ToList();

            ddlCostCentre.DataSource = costCentreTabBOList;
            ddlCostCentre.DataTextField = "CostCenter";
            ddlCostCentre.DataValueField = "CostCenterId";
            ddlCostCentre.DataBind();


            if (costCentreTabBOList.Count > 1)
                ddlCostCentre.Items.Insert(0, item);

            ddlCostCenterSearch.DataSource = costCentreTabBOList;
            ddlCostCenterSearch.DataTextField = "CostCenter";
            ddlCostCenterSearch.DataValueField = "CostCenterId";
            ddlCostCenterSearch.DataBind();
            if (costCentreTabBOList.Count > 1)
                ddlCostCenterSearch.Items.Insert(0, item);

        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("All");

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
        private void LoadOrderType()
        {
            ListItem item = new ListItem();
            item.Value = "AdHoc";
            item.Text = "Ad Hoc Purchase";

            ListItem item1 = new ListItem();
            item1.Value = "Requisition";
            item1.Text = "Requisition Wise Purchase";

            ddlPurchaseOrderType.Items.Insert(0, item);
            ddlPurchaseOrderType.Items.Insert(1, item1);

        }
        private void LoadRequisitionOrder()
        {
            PMRequisitionDA entityDA = new PMRequisitionDA();
            List<PMRequisitionBO> requisitionBOList = entityDA.GetApprovedNNotDeliveredRequisitionInfo();

            ddlRequisitionOrder.DataSource = requisitionBOList;
            ddlRequisitionOrder.DataTextField = "PRNumber";
            ddlRequisitionOrder.DataValueField = "RequisitionId";
            ddlRequisitionOrder.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---Please Select---";

            ddlRequisitionOrder.Items.Insert(0, item);

            hfRequsitionOrderObj.Value = JsonConvert.SerializeObject(requisitionBOList);

        }
        private void CheckPermission()
        {

            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }
        #endregion
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
        #region Web Method For Ajax Call
        [WebMethod]
        public static List<PMSupplierBO> LoadSupplierInfoByOrderType(string supplierTypeId)
        {
            PMSupplierDA entityDA = new PMSupplierDA();
            List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();

            supplierBOList = entityDA.GetPMSupplierInfoByOrderType(supplierTypeId);
            return supplierBOList;
        }
        [WebMethod]
        public static CommonCurrencyConversionBO LoadCurrencyConversionRate(int currencyId)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyConversionBO conversionBO = new CommonCurrencyConversionBO();
            conversionBO = commonCurrencyDA.GetCurrencyConversionRate(currencyId);
            return conversionBO;
        }
        [WebMethod]
        public static GLCompanyBO LoadAccountsCompanyInformation(int companyId)
        {
            GLCompanyDA companyDA = new GLCompanyDA();
            GLCompanyBO companyBO = new GLCompanyBO();
            companyBO = companyDA.GetGLCompanyInfoById(companyId);
            return companyBO;
        }
        [WebMethod]
        public static InvItemStockInformationBO GetInvItemStockInfoByItemAndAttributeIdForPurchase(int itemId, int colorId, int sizeId, int styleId, int locationId, int companyId)
        {
            InvItemDA DA = new InvItemDA();
            InvItemStockInformationBO StockInformation = new InvItemStockInformationBO();
            StockInformation = DA.GetInvItemStockInfoByItemAndAttributeIdForPurchase(itemId, colorId, sizeId, styleId, locationId, companyId);

            return StockInformation;
        }
        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemSearch(string searchTerm, int companyId, int costCenterId, int categoryId, int supplierId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemDetailsForAutoSearch(searchTerm, companyId, costCenterId, ConstantHelper.CustomerSupplierAutoSearch.SupplierItem.ToString(), categoryId, supplierId);

            return itemInfo;
        }
        [WebMethod]
        public static List<CostCentreTabBO> LoadReceiveStoreByCompanyId(int companyId)
        {
            HMUtility hmUtility = new HMUtility();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfoByUserGroupIdNCompanyId(userInformationBO.UserGroupId, companyId)
                                    .Where(o => o.OutletType == 2 && o.CostCenterType == "Inventory").ToList();
            return costCentreTabBOList;
        }
        [WebMethod]
        public static List<RequisitionItemForPurchaseBO> GetRequisitionItemForPurchaseById(int requisitionId, int supplierId, int porderId)
        {
            PMPurchaseOrderDA entityDA = new PMPurchaseOrderDA();
            List<RequisitionItemForPurchaseBO> ri = new List<RequisitionItemForPurchaseBO>();
            ri = entityDA.GetRequisitionItemForPurchaseById(requisitionId, supplierId, porderId);

            return ri;
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
        public static ReturnInfo SaveAdhocPurchaseOrder(PMPurchaseOrderBO PurchaseOrder, List<PMPurchaseOrderDetailsBO> AddedPurchaseOrderItem,
                                List<PMPurchaseOrderDetailsBO> EditedPurchaseOrderItem, List<PMPurchaseOrderDetailsBO> DeletedPurchaseOrderItem,
                                List<PMPurchaseOrderTermsNConditionBO> TermsNConditions, List<PMPurchaseOrderTermsNConditionBO> deletedTermsNConditions)
        {
            string TransactionNo = "";
            string TransactionType = "";
            string ApproveStatus = "";
            ReturnInfo rtninf = new ReturnInfo();
            int tmpOrderId = 0;
            string porderNumber = string.Empty;
            bool status = false, isApprovalProcessEnable = true;

            try
            {
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsPurchaseOrderApprovalEnable", "IsPurchaseOrderApprovalEnable");

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                PMPurchaseOrderDA orderDetailsDA = new PMPurchaseOrderDA();

                if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedEnable))
                {
                    PurchaseOrder.ApprovedStatus = HMConstants.ApprovalStatus.Pending.ToString();
                    isApprovalProcessEnable = true;
                }
                else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedDisable))
                {
                    PurchaseOrder.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();
                    isApprovalProcessEnable = false;
                }

                if (PurchaseOrder.POrderId == 0)
                {
                    PurchaseOrder.CreatedBy = userInformationBO.UserInfoId;
                    status = orderDetailsDA.SavePMPurchaseOrderInfo(PurchaseOrder, AddedPurchaseOrderItem, isApprovalProcessEnable, out tmpOrderId, out porderNumber, out TransactionNo, out TransactionType, out ApproveStatus);
                    if (status)
                    {
                        orderDetailsDA.SavePMPurchaseOrderTermsNConditions(TermsNConditions, deletedTermsNConditions, tmpOrderId);
                    }
                    if (status && Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedDisable))
                    {
                        orderDetailsDA.UpdatePurchaseOrderStatus(tmpOrderId, HMConstants.ApprovalStatus.Approved.ToString(), userInformationBO.UserInfoId);
                    }

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.ProductPurchaseOrder.ToString(), tmpOrderId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductPurchaseOrder));

                        rtninf.IsSuccess = true;
                        rtninf.PrimaryKeyValue = tmpOrderId.ToString();
                        rtninf.TransactionNo = TransactionNo;
                        rtninf.TransactionType = TransactionType;
                        rtninf.TransactionStatus = ApproveStatus;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }
                else
                {
                    PurchaseOrder.LastModifiedBy = userInformationBO.UserInfoId;

                    EditedPurchaseOrderItem = (from pod in AddedPurchaseOrderItem where pod.DetailId > 0 select pod).ToList();
                    AddedPurchaseOrderItem = (from pod in AddedPurchaseOrderItem where pod.DetailId == 0 select pod).ToList();

                    status = orderDetailsDA.UpdatePurchaseOrderInfo(PurchaseOrder, AddedPurchaseOrderItem, EditedPurchaseOrderItem, DeletedPurchaseOrderItem, out TransactionNo, out TransactionType, out ApproveStatus);
                    if (status)
                    {
                        orderDetailsDA.SavePMPurchaseOrderTermsNConditions(TermsNConditions, deletedTermsNConditions, PurchaseOrder.POrderId);
                    }
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.ProductPurchaseOrder.ToString(), PurchaseOrder.POrderId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductPurchaseOrder));
                        rtninf.IsSuccess = true;

                        rtninf.PrimaryKeyValue = tmpOrderId.ToString();
                        rtninf.TransactionNo = TransactionNo;
                        rtninf.TransactionType = TransactionType;
                        rtninf.TransactionStatus = ApproveStatus;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }

                if (!status)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
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
        public static ReturnInfo SaveRequisitionWisePurchaseOrder(PMPurchaseOrderBO PurchaseOrder, List<PMPurchaseOrderDetailsBO> PurchaseOrderItem,
                            List<PMPurchaseOrderDetailsBO> PurchaseOrderItemFromRequisition, List<PMPurchaseOrderDetailsBO> PurchaseOrderItemDeleted,
                            List<PMPurchaseOrderTermsNConditionBO> TermsNConditions, List<PMPurchaseOrderTermsNConditionBO> deletedTermsNConditions)
        {
            string TransactionNo = "";
            string TransactionType = "";
            string ApproveStatus = "";
            ReturnInfo rtninf = new ReturnInfo();
            int tmpOrderId = 0;
            string porderNumber = string.Empty;
            bool status = false, isApprovalProcessEnable = true;

            try
            {
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsPurchaseOrderApprovalEnable", "IsPurchaseOrderApprovalEnable");

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                PMPurchaseOrderDA orderDetailsDA = new PMPurchaseOrderDA();

                if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedEnable))
                {
                    PurchaseOrder.ApprovedStatus = HMConstants.ApprovalStatus.Pending.ToString();
                    isApprovalProcessEnable = true;
                }
                else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedDisable))
                {
                    PurchaseOrder.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();
                    isApprovalProcessEnable = false;
                }

                foreach (PMPurchaseOrderDetailsBO p in PurchaseOrderItemFromRequisition)
                {
                    var v = (from po in PurchaseOrderItem where p.ItemId == po.ItemId select po).FirstOrDefault();

                    if (v != null)
                    {
                        if (!string.IsNullOrEmpty(v.Remarks))
                            p.Remarks = v.Remarks;
                    }
                }

                if (PurchaseOrder.POrderId == 0)
                {
                    PurchaseOrder.CreatedBy = userInformationBO.UserInfoId;
                    status = orderDetailsDA.SavePurchaseOrderFromRequisition(PurchaseOrder, PurchaseOrderItemFromRequisition, isApprovalProcessEnable, out tmpOrderId, out porderNumber, out TransactionNo, out TransactionType, out ApproveStatus);
                    if (status)
                    {
                        orderDetailsDA.SavePMPurchaseOrderTermsNConditions(TermsNConditions, deletedTermsNConditions, tmpOrderId);
                    }
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.ProductPurchaseOrder.ToString(), tmpOrderId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductPurchaseOrder));
                        rtninf.IsSuccess = true;
                        rtninf.PrimaryKeyValue = tmpOrderId.ToString();
                        rtninf.TransactionNo = TransactionNo;
                        rtninf.TransactionType = TransactionType;
                        rtninf.TransactionStatus = ApproveStatus;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }
                else
                {
                    PurchaseOrder.LastModifiedBy = userInformationBO.UserInfoId;

                    List<PMPurchaseOrderDetailsBO> AddedPurchaseOrderItem = new List<PMPurchaseOrderDetailsBO>();
                    List<PMPurchaseOrderDetailsBO> EditedPurchaseOrderItem = new List<PMPurchaseOrderDetailsBO>();

                    EditedPurchaseOrderItem = (from pod in PurchaseOrderItemFromRequisition where pod.DetailId > 0 select pod).ToList();
                    AddedPurchaseOrderItem = (from pod in PurchaseOrderItemFromRequisition where pod.DetailId == 0 select pod).ToList();

                    status = orderDetailsDA.UpdatePurchaseOrderInfo(PurchaseOrder, AddedPurchaseOrderItem, EditedPurchaseOrderItem, PurchaseOrderItemDeleted, out TransactionNo, out TransactionType, out ApproveStatus);
                    if (status)
                    {
                        orderDetailsDA.SavePMPurchaseOrderTermsNConditions(TermsNConditions, deletedTermsNConditions, PurchaseOrder.POrderId);
                    }
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.ProductPurchaseOrder.ToString(), PurchaseOrder.POrderId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductPurchaseOrder));
                        rtninf.IsSuccess = true;
                        rtninf.PrimaryKeyValue = tmpOrderId.ToString();
                        rtninf.TransactionNo = TransactionNo;
                        rtninf.TransactionType = TransactionType;
                        rtninf.TransactionStatus = ApproveStatus;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }

                if (!status)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
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
        public static GridViewDataNPaging<PMPurchaseOrderBO, GridPaging> SearchPurchaseOrder(int companyId, string orderType, string poType, DateTime? fromDate, DateTime? toDate,
                                                                                           string poNumber, string status, int? costCenterId,
                                                                                           int? supplierId, int gridRecordsCount, int pageNumber,
                                                                                           int isCurrentOrPreviousPage
                                                                                          )
        {

            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<PMPurchaseOrderBO, GridPaging> myGridData = new GridViewDataNPaging<PMPurchaseOrderBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            PMPurchaseOrderDA detalisDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();
            orderList = detalisDA.GetPMPurchaseOrderInfoBySearchCriteria(companyId, orderType, poType, fromDate, toDate, poNumber, "", status, costCenterId, supplierId, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);
            myGridData.GridPagingProcessing(orderList, totalRecords);
            return myGridData;
        }

        [WebMethod]
        public static PurchaseOrderViewBO EditPurchaseOrder(int pOrderId)
        {
            PurchaseOrderViewBO viewBo = new PurchaseOrderViewBO();
            PMPurchaseOrderDA orderDetailDA = new PMPurchaseOrderDA();

            viewBo.PurchaseOrder = orderDetailDA.GetPMPurchaseOrderInfoByOrderId(pOrderId);
            viewBo.PurchaseOrderDetails = orderDetailDA.GetPMPurchaseOrderDetailByOrderId(pOrderId);
            viewBo.PurchaseOrder.TermsNConditions = orderDetailDA.GetTermsNConditionsByPurchaseOrderId(viewBo.PurchaseOrder.POrderId);
            if (viewBo.PurchaseOrder.POType == "Requisition")
            {
                viewBo.PurchaseOrderDetailsSummary = orderDetailDA.GetPurchaseOrderFromRequisitionSummaryByOrderId(pOrderId);
            }

            return viewBo;
        }

        [WebMethod]
        public static ReturnInfo PurchaseOrderApproval(string poType, int pOrderId, string approvedStatus)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PurchaseOrderViewBO viewBo = new PurchaseOrderViewBO();
            PMPurchaseOrderDA orderDetailDA = new PMPurchaseOrderDA();
            string TransactionNo = "";
            string TransactionType = "";
            string ApproveStatus = "";

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDetailDA.PurchaseOrderApproval(poType, pOrderId, approvedStatus, userInformationBO.UserInfoId, out TransactionNo, out TransactionType, out ApproveStatus);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    rtninf.PrimaryKeyValue = pOrderId.ToString();
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

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ProductPurchaseOrder.ToString(), pOrderId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductPurchaseOrder));

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
        public static ReturnInfo PurchaseOrderDelete(string pOType, int pOrderId, string approvedStatus, int createdBy)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PurchaseOrderViewBO viewBo = new PurchaseOrderViewBO();
            PMPurchaseOrderDA orderDetailDA = new PMPurchaseOrderDA();
            string TransactionNo = "";
            string TransactionType = "";
            string ApproveStatus = "";

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDetailDA.PurchaseOrderDelete(pOType, pOrderId, approvedStatus, createdBy, userInformationBO.UserInfoId, out TransactionNo, out TransactionType, out ApproveStatus);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    rtninf.PrimaryKeyValue = pOrderId.ToString();
                    rtninf.TransactionNo = TransactionNo;
                    rtninf.TransactionType = TransactionType;
                    rtninf.TransactionStatus = ApproveStatus;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.ProductPurchaseOrder.ToString(), pOrderId,
                              ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductPurchaseOrder));

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
        public static List<PMRequisitionBO> LoadRequisition()
        {
            PMRequisitionDA entityDA = new PMRequisitionDA();
            List<PMRequisitionBO> requisitionBOList = entityDA.GetApprovedNNotDeliveredRequisitionInfo();

            return requisitionBOList;
        }

        [WebMethod]
        public static List<TermsNConditionsMasterBO> LoadTearmsNConditions()
        {
            List<TermsNConditionsMasterBO> conditionsLIst = new List<TermsNConditionsMasterBO>();
            TermsNConditionsDA DA = new TermsNConditionsDA();
            conditionsLIst = DA.GetTermsNConditionsByType("PurchaseOrder");
            return conditionsLIst;
        }

        [WebMethod]
        public static ReturnInfo ReOpenPurchaseOrder(int pOrderId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMPurchaseOrderDA orderDetailDA = new PMPurchaseOrderDA();
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDetailDA.ReOpenPurchaseOrder(pOrderId);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Success, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ProductPurchaseOrder.ToString(), pOrderId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductPurchaseOrder));

                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;

        }
        #endregion

    }
}