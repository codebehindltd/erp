using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.SalesManagment;
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

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class CostAnalysis : Page
    {

        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategory();
                LoadStockBy();
                LoadServicePackage();
                LoadServiceBandwidth();
                LoadBandwidthServiceType();
                LoadContractPeriod();
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
        private void LoadServicePackage()
        {
            InvItemDA locationDA = new InvItemDA();
            List<InvServicePackage> serviceList = new List<InvServicePackage>();
            serviceList = locationDA.GetServicePackage().Where(i => i.IsActive == true).ToList(); 

            ddlPackage.DataSource = serviceList;
            ddlPackage.DataTextField = "PackageName";
            ddlPackage.DataValueField = "ServicePackageId";
            ddlPackage.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlPackage.Items.Insert(0, item);
        }
        private void LoadServiceBandwidth()
        {
            InvItemDA locationDA = new InvItemDA();
            List<ServiceBandwidthBO> serviceList = new List<ServiceBandwidthBO>();
            serviceList = locationDA.GetServiceBandwidth();

            ddlServiceBandwidth.DataSource = serviceList;
            ddlServiceBandwidth.DataTextField = "BandWidthName";
            ddlServiceBandwidth.DataValueField = "ServiceBandWidthId";
            ddlServiceBandwidth.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlServiceBandwidth.Items.Insert(0, item);

            hfDownloadBandwidth.Value = JsonConvert.SerializeObject(serviceList);
        }

        private void LoadBandwidthServiceType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("BandwidthServiceType", hmUtility.GetDropDownFirstValue());

            ddlBandwidthServiceType.DataSource = fields;
            ddlBandwidthServiceType.DataTextField = "FieldValue";
            ddlBandwidthServiceType.DataValueField = "FieldId";
            ddlBandwidthServiceType.DataBind();

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

        [WebMethod]
        public static ReturnInfo SaveCostAnalysis(SMCostAnalysis CostAnalysis, List<SMCostAnalysisDetail> ItemDetails, List<SMCostAnalysisDetail> ServiceDetails,
                                               List<SMCostAnalysisDetail> DeletedItemDetails, List<SMCostAnalysisDetail> DeletedServiceDetails)
        {
            ReturnInfo rtninf = new ReturnInfo();
            int costAnalysisId = 0;
            string porderNumber = string.Empty;
            bool status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                CostAnalysisDA costAnalysisDA = new CostAnalysisDA();

                if (CostAnalysis.Id == 0)
                {
                    CostAnalysis.CreatedBy = userInformationBO.UserInfoId;
                    status = costAnalysisDA.SaveCostAnalysis(CostAnalysis, ItemDetails, ServiceDetails, out costAnalysisId);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.SMCostAnalysis.ToString(), costAnalysisId,
                                ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMCostAnalysis));
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }
                else
                {
                    CostAnalysis.LastModifiedBy = userInformationBO.UserInfoId;

                    status = costAnalysisDA.UpdateCostAnalysis(CostAnalysis, ItemDetails, ServiceDetails,
                                                                DeletedItemDetails, DeletedServiceDetails
                                                               );

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.SMCostAnalysis.ToString(), CostAnalysis.Id,
                               ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMCostAnalysis));
                        rtninf.IsSuccess = true;
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
        public static SMCostAnalysisView GetCostAnalysisById(long id)
        {
            SMCostAnalysisView costAnalysis = new SMCostAnalysisView();
            CostAnalysisDA costAnalysisDA = new CostAnalysisDA();
            CommonDA commonDA = new CommonDA();

            costAnalysis = costAnalysisDA.GetCostAnalysisViewById(id);
            costAnalysis.Items = costAnalysis.AllItems.Where(i => i.ItemType == CommonHelper.CostAnalysisItemType.Item.ToString()).ToList();
            costAnalysis.Services = costAnalysis.AllItems.Where(i => i.ItemType == CommonHelper.CostAnalysisItemType.Service.ToString()).ToList();

            return costAnalysis;
        }

        [WebMethod]
        public static SMCostAnalysisView GetCostAnalysisFromQuotation()
        {
            SMCostAnalysisView costAnalysis = new SMCostAnalysisView();
            CostAnalysisDA costAnalysisDA = new CostAnalysisDA();
            CommonDA commonDA = new CommonDA();

            List<SMQuotationDetailsBO> QuotationItemDetails = (List<SMQuotationDetailsBO>)HttpContext.Current.Session["QuotationItem"];
            List<SMQuotationDetailsBO> QuotationServiceDetails = (List<SMQuotationDetailsBO>)HttpContext.Current.Session["QuotationService"];

            costAnalysis.CostAnalysis = new SMCostAnalysis();
            costAnalysis.Items = (from i in QuotationItemDetails
                                  select new SMCostAnalysisDetail
                                  {
                                      ItemType = i.ItemType,
                                      CategoryId = i.CategoryId,
                                      ServicePackageId = i.ServicePackageId,
                                      ServiceBandWidthId = i.ServiceBandWidthId,
                                      ServiceTypeId = i.ServiceTypeId,
                                      ItemId = i.ItemId,
                                      ItemName = i.ItemName,
                                      StockBy = i.StockBy,
                                      Quantity = i.Quantity,
                                      UnitPrice = i.UnitPrice,
                                      AverageCost = i.AverageCost,
                                      TotalOfferedPrice = i.TotalPrice,
                                      //UpLink = i.UpLink
                                  }).ToList();

            costAnalysis.Services = (from i in QuotationServiceDetails
                                     select new SMCostAnalysisDetail
                                     {
                                         ItemType = i.ItemType,
                                         CategoryId = i.CategoryId,
                                         ServicePackageId = i.ServicePackageId,
                                         ServiceBandWidthId = i.ServiceBandWidthId,
                                         ServiceTypeId = i.ServiceTypeId,
                                         ItemId = i.ItemId,
                                         ItemName = i.ItemName,
                                         StockBy = i.StockBy,
                                         Quantity = i.Quantity,
                                         UnitPrice = i.UnitPrice,
                                         AverageCost = i.AverageCost,
                                         TotalOfferedPrice = i.TotalPrice,
                                         //UpLink = i.UpLink
                                     }).ToList();
            //costAnalysis.Services = costAnalysis.AllItems.Where(i => i.ItemType == CommonHelper.CostAnalysisItemType.Service.ToString()).ToList();

            return costAnalysis;
        }
    }
}