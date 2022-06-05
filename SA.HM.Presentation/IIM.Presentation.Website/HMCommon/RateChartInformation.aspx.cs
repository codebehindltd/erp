using HotelManagement.Data;
using HotelManagement.Data.Banquet;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class RateChartInformation : BasePage
    {

        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStockBy();
                LoadCategory();
                LoadBandwidthServiceType();
                LoadRestaurant();
                LoadBanquet();
                LoadContractPeriod();
                LoadCompany();
                LoadServiceItemType();
            }
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllActiveInvItemCatagoryInfoByServiceType("Product");
            ddlCategory.DataSource = List;
            ddlCategory.DataTextField = "MatrixInfo";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCategory.Items.Insert(0, item);

            List<InvCategoryBO> serviceCategory = new List<InvCategoryBO>();
            serviceCategory = da.GetAllActiveInvItemCatagoryInfoByServiceType("Service");
            ddlCategoryService.DataSource = serviceCategory;
            ddlCategoryService.DataTextField = "MatrixInfo";
            ddlCategoryService.DataValueField = "CategoryId";
            ddlCategoryService.DataBind();
            ddlCategoryService.Items.Insert(0, item);

        }
        private void LoadStockBy()
        {
            List<InvUnitHeadBO> headListBO = new List<InvUnitHeadBO>();
            InvUnitHeadDA da = new InvUnitHeadDA();
            headListBO = da.GetInvUnitHeadInfo();

            ddlServiceStockBy.DataSource = headListBO;
            ddlServiceStockBy.DataTextField = "HeadName";
            ddlServiceStockBy.DataValueField = "UnitHeadId";
            ddlServiceStockBy.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlServiceStockBy.Items.Insert(0, item);

        }
        private void LoadBandwidthServiceType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("BandwidthServiceType", hmUtility.GetDropDownFirstValue());

            ddlBandwidthServiceType.DataSource = fields;
            ddlBandwidthServiceType.DataTextField = "FieldValue";
            ddlBandwidthServiceType.DataValueField = "FieldId";
            ddlBandwidthServiceType.DataBind();
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadContractPeriod()
        {
            SalesQuotationNBillingDA serviceDA = new SalesQuotationNBillingDA();
            List<ContractPeriodBO> serviceList = new List<ContractPeriodBO>();
            serviceList = serviceDA.GetContractPeriod().Where(i => i.ActiveStat).ToList();

            ddlContractPeriod.DataSource = serviceList;
            ddlContractPeriod.DataTextField = "ContractPeriodName";
            ddlContractPeriod.DataValueField = "ContractPeriodId";
            ddlContractPeriod.DataBind();

            hfContactPeriod.Value = JsonConvert.SerializeObject(serviceList);

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlContractPeriod.Items.Insert(0, item);
        }
        private void LoadRestaurant()
        {

            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetCostCentreTabInfoByType("Restaurant");

            ddlRestaurant.DataSource = List;
            ddlRestaurant.DataTextField = "CostCenter";
            ddlRestaurant.DataValueField = "CostCenterId";
            ddlRestaurant.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            if (List.Count > 0)
            {
                ddlRestaurant.Items.Insert(0, item);
            }
        }
        private void LoadBanquet()
        {
            BanquetInformationDA banquetDA = new BanquetInformationDA();
            var List = banquetDA.GetAllBanquetInformation();

            ddlBanquet.DataSource = List;
            ddlBanquet.DataTextField = "Name";
            ddlBanquet.DataValueField = "Id";
            ddlBanquet.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            if (List.Count > 0)
            {
                ddlBanquet.Items.Insert(0, item);
            }
        }
        private void LoadCompany()
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<GuestCompanyBO> companyBOList = new List<GuestCompanyBO>();
            companyBOList = companyDA.GetGuestCompanyInfo();

            ddlCompany.DataSource = companyBOList;
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();

            ddlSearchCompany.DataSource = companyBOList;
            ddlSearchCompany.DataTextField = "CompanyName";
            ddlSearchCompany.DataValueField = "CompanyId";
            ddlSearchCompany.DataBind();
            if (companyBOList.Count > 0)
            {
                ListItem item = new ListItem();
                item.Value = "0";
                item.Text = hmUtility.GetDropDownFirstValue();
                ddlCompany.Items.Insert(0, item);
                ListItem item2 = new ListItem();
                item2.Value = "0";
                item2.Text = hmUtility.GetDropDownFirstAllValue();
                ddlSearchCompany.Items.Insert(0, item2);
            }
        }
        private void LoadServiceItemType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CustomFieldBO> ServiceTypeList = new List<CustomFieldBO>();
            ServiceTypeList = commonDA.GetCustomField("CRMQuotaionServiceType");

            ddlServiceItemType.DataSource = ServiceTypeList;
            ddlServiceItemType.DataTextField = "Description";
            ddlServiceItemType.DataValueField = "FieldValue";
            ddlServiceItemType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlServiceItemType.Items.Insert(0, item);

        }
        [WebMethod]
        public static List<InvUnitHeadBO> LoadRelatedStockBy(int stockById)
        {
            InvUnitHeadDA unitHeadDA = new InvUnitHeadDA();
            List<InvUnitHeadBO> unitHeadList = new List<InvUnitHeadBO>();

            unitHeadList = unitHeadDA.GetRelatedStockBy(stockById);

            return unitHeadList;
        }
        [WebMethod]
        public static List<ServicePriceMatrixBO> GetPackageByItemBy(int itemId)
        {
            CommonDA commonDA = new CommonDA();
            List<ServicePriceMatrixBO> packages = new List<ServicePriceMatrixBO>();
            if (itemId > 0)
                packages = commonDA.GetAllTableRow<ServicePriceMatrixBO>("InvServicePriceMatrix", "ItemId", itemId.ToString());
            else
                packages = commonDA.GetAllTableRow<ServicePriceMatrixBO>("InvServicePriceMatrix");
            return packages;
        }
        [WebMethod]
        public static ServicePriceMatrixBO GetServicePriceMatrix(int servicePriceMatrixId)
        {
            InvServicePriceMatrixDA priceMatrixDA = new InvServicePriceMatrixDA();
            ServicePriceMatrixBO servicePriceMatrix = new ServicePriceMatrixBO();

            servicePriceMatrix = priceMatrixDA.GetServicePriceMatrixById(servicePriceMatrixId);

            return servicePriceMatrix;
        }

        [WebMethod]
        public static ReturnInfo SaveOrUpdateRateChart(RateChartMaster RateChartMaster, List<RateChartDetail> RateChartDetail, List<long> DeletedRateChartDetail, List<long> DeletedRateChartDiscountDetail)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            long RateChartMasterId = 0;
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RateChartDA rateChartDA = new RateChartDA();

                RateChartMaster.CreatedBy = userInformationBO.UserInfoId;

                string DeletedRateChartDetailIdList = string.Join(",", DeletedRateChartDetail);
                string DeletedRateChartDiscountDetailIdList = string.Join(",", DeletedRateChartDiscountDetail);

                returnInfo.IsSuccess = rateChartDA.SaveOrUpdateRateChart(RateChartMaster, RateChartDetail, DeletedRateChartDetailIdList, DeletedRateChartDiscountDetailIdList, out RateChartMasterId);
                if (returnInfo.IsSuccess)
                {
                    if (RateChartMaster.Id == 0)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.SMQuotation.ToString(), RateChartMasterId,
                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMQuotation));
                        returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                    else
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.SMQuotation.ToString(), RateChartMaster.Id,
                               ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMQuotation));
                        returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }
                else
                {
                    returnInfo.IsSuccess = false;
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                returnInfo.IsSuccess = false;
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return returnInfo;
        }

        [WebMethod]
        public static RateChartMasterEditView GetRateChartById(long id)
        {
            RateChartMasterEditView rateChart = new RateChartMasterEditView();
            RateChartDA rateChartDA = new RateChartDA();
            CommonDA commonDA = new CommonDA();
            rateChart.RateChartMaster = commonDA.GetTableRow<RateChartMaster>("RateChartMaster", "Id", id.ToString());
            rateChart.RateChartDetails = rateChartDA.GetRateChartDetailByRateChartMasterIdForEdit(id);
            return rateChart;
        }

        [WebMethod]
        public static GridViewDataNPaging<RateChartMaster, GridPaging> GetRateChartListWithPagination(string promotionName, int companyId, string effectFrom, string effectTo, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            DateTime EffectFrom = DateTime.Now, EffectTo = DateTime.Now;

            int totalRecords = 0;

            if (!string.IsNullOrWhiteSpace(effectFrom))
                EffectFrom = CommonHelper.DateTimeToMMDDYYYY(effectFrom, userInformationBO.ClientDateFormat);
            if (!string.IsNullOrWhiteSpace(effectTo))
                EffectTo = CommonHelper.DateTimeToMMDDYYYY(effectTo, userInformationBO.ClientDateFormat);


            GridViewDataNPaging<RateChartMaster, GridPaging> myGridData = new GridViewDataNPaging<RateChartMaster, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            RateChartDA rateChartDA = new RateChartDA();
            List<RateChartMaster> RateChartList = new List<RateChartMaster>();
            RateChartList = rateChartDA.GetRateChartBySearchCriteriaWithPagination(promotionName, companyId, EffectFrom, EffectTo, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(RateChartList, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo DeleteRateChart(long rateChartId)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            RateChartDA rateChartDA = new RateChartDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            try
            {
                rtninf.IsSuccess = rateChartDA.DeleteRateChartById(rateChartId);

                if (rtninf.IsSuccess)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.SMQuotation.ToString(), rateChartId,
                           ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMQuotation));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return rtninf;

        }
        [WebMethod]
        public static List<RateChartDiscountDetailView> GetTypeList(string serviceType)
        {
            List<RateChartDiscountDetailView> detailsViews = new List<RateChartDiscountDetailView>();

            if (serviceType == "Restaurant")
            {
                InvItemClassificationCostCenterMappingDA invItemClassificationDA = new InvItemClassificationCostCenterMappingDA();
                var List = invItemClassificationDA.GetActiveItemClassificationInfo();

                detailsViews = List.Select(i => new RateChartDiscountDetailView
                {
                    Type = "Classification",
                    TypeName = i.ClassificationName,
                    TypeId = i.ClassificationId,
                    DiscountAmount = 0,
                    DiscountAmountUSD = 0,
                    OfferredPrice = 0
                }).ToList();
            }
            else if (serviceType == "GuestRoom")
            {
                RoomTypeDA roomTypeDA = new RoomTypeDA();
                var List = roomTypeDA.GetRoomTypeInfo();

                detailsViews = List.Select(i => new RateChartDiscountDetailView
                {
                    Type = "RoomType",
                    TypeName = i.RoomType,
                    TypeId = i.RoomTypeId,
                    UnitPrice = i.RoomRate,
                    DiscountAmount = 0,
                    DiscountAmountUSD = 0,
                    OfferredPrice = 0
                }).ToList();
            }
            else if (serviceType == "Banquet")
            {
                InvItemClassificationCostCenterMappingDA invItemClassificationDA = new InvItemClassificationCostCenterMappingDA();
                var List = invItemClassificationDA.GetActiveItemClassificationInfo();

                detailsViews.Add(new RateChartDiscountDetailView
                {
                    Type = "HallRent",
                    TypeName = "Hall Rent",
                    DiscountAmount = 0,
                    DiscountAmountUSD = 0,
                    OfferredPrice = 0
                });
                detailsViews.Add(new RateChartDiscountDetailView
                {
                    Type = "Requisites",
                    TypeName = "Requisites",
                    DiscountAmount = 0,
                    DiscountAmountUSD = 0,
                    OfferredPrice = 0
                });

                detailsViews.AddRange(List.Select(i => new RateChartDiscountDetailView
                {
                    Type = "Classification",
                    TypeName = i.ClassificationName,
                    TypeId = i.ClassificationId,
                    DiscountAmount = 0,
                    DiscountAmountUSD = 0,
                    OfferredPrice = 0
                }).ToList());
            }
            else if (serviceType == "ServiceOutlet")
            {
                HotelGuestServiceInfoDA guestHouseServiceDA = new HotelGuestServiceInfoDA();
                var List = guestHouseServiceDA.GetHotelGuestServiceInfo(1, 0, 0);
                detailsViews = List.Select(i => new RateChartDiscountDetailView
                {
                    Type = "GuestService",
                    TypeName = i.ServiceName,
                    TypeId = i.ServiceId,
                    UnitPrice = i.UnitPriceLocal,
                    DiscountAmount = 0,
                    DiscountAmountUSD = 0,
                    OfferredPrice = 0
                }).ToList();
            }

            return detailsViews;
        }
    }
}