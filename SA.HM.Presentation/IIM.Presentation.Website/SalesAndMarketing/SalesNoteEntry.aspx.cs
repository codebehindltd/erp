using HotelManagement.Data.HMCommon;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class SalesNoteEntry : System.Web.UI.Page
    {
        public bool IsSalesNoteEnable;
        protected void Page_Load(object sender, EventArgs e)
        {
            IsSalesNoteEnable = CheckIsSalesNoteEnable();
        }

        private bool CheckIsSalesNoteEnable()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSalesNoteEnable", "IsSalesNoteEnable");
            return setUpBO.SetupValue == "1";
        }
        [WebMethod]
        public static SalesQuotationEditBO LoadQuotation(Int64 quotationId)
        {
            SalesQuotationEditBO quotationv = new SalesQuotationEditBO();
            SalesQuotationNBillingDA salesDA = new SalesQuotationNBillingDA();

            quotationv.Quotation = salesDA.GetQuotationById(quotationId);
            quotationv.QuotationItemDetails = salesDA.GetQuotationItemDetailsById(quotationId, CommonHelper.QuotationItemType.Item.ToString());
            quotationv.QuotationServiceDetails = salesDA.GetQuotationServiceDetailsById(quotationId, CommonHelper.QuotationItemType.Service.ToString());
            quotationv.QuotationDetails = salesDA.GetQuotationDetailsById(quotationId).Where(i => i.ItemType != "Item" && i.ItemType != "Service").OrderBy(i => i.ItemType).ToList();

            return quotationv;
        }
        [WebMethod]
        public static ReturnInfo UpdateSalesNote(List<SMQuotationDetailsBO> details)
        {
            ReturnInfo info = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            SalesQuotationNBillingDA salesDA = new SalesQuotationNBillingDA();

            try
            {
                info.IsSuccess = salesDA.UpdateQuotationDetailsSalesNote(details);
                if (info.IsSuccess)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                                EntityTypeEnum.EntityType.SMQuotationDetails.ToString(), details[0].QuotationDetailsId,
                                        ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(),
                                                hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMQuotationDetails));
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }
                else
                {
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }
        [WebMethod]
        public static ReturnInfo FinalizeSalesNote(long id, List<SMQuotationDetailsBO> details)
        {
            ReturnInfo info = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            SalesQuotationNBillingDA salesDA = new SalesQuotationNBillingDA();

            try
            {
                if (details.Count > 0)
                {
                    info.IsSuccess = salesDA.UpdateQuotationDetailsSalesNote(details);
                    if (info.IsSuccess)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                                    EntityTypeEnum.EntityType.SMQuotationDetails.ToString(), details[0].QuotationDetailsId,
                                            ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(),
                                                    hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMQuotationDetails));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                    else
                    {
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }
                else
                    info.IsSuccess = true;

                if (info.IsSuccess)
                    info.IsSuccess = salesDA.FinalizeQuotationSalesNote(id);

                if (info.IsSuccess)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                                EntityTypeEnum.EntityType.SMQuotationDetails.ToString(), id,
                                        ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(),
                                                hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMQuotationDetails));
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }
                else
                {
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }
    }
}