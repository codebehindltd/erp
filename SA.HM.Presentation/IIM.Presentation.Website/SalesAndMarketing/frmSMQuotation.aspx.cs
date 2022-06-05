using HotelManagement.Data;
using HotelManagement.Data.Banquet;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Mamun.Presentation.Website.Common;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class frmSMQuotation : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                int id = Convert.ToInt32(Request.QueryString["Id"]);
                if (id > 0)
                {
                    // LoadQuotation(id);
                }
                //LoadCompany();
                LoadCommonDropDownHiddenField();
                //LoadSalesCallNotification();
                LoadServiceType();
                LoadItemOrServiceDelivery();
                LoadCategory();
                LoadBillingPeriod();
                LoadContractPeriod();
                LoadCurrentVendor();
                LoadStockBy();
                LoadBandwidthServiceType();
                LoadCompanyIndependentContacts();
                LoadCRMConfiguaration();
                CheckIsQuotationCreateFromSiteServeyFeedback();
                LoadBanquet();
                LoadRestaurant();
                LoadServiceItemType();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            SalesCallDA salesCallDA = new SalesCallDA();
            SMCompanySalesCallBO salesCallBO = new SMCompanySalesCallBO();
        }
        private void LoadDeal()
        {

        }
        private void LoadSite()
        {

        }
        //private void LoadCompany()
        //{
        //    GuestCompanyDA companyDA = new GuestCompanyDA();
        //    List<GuestCompanyBO> companyBOList = new List<GuestCompanyBO>();
        //    companyBOList = companyDA.GetGuestCompanyInfo();

        //    ddlCompany.DataSource = companyBOList;
        //    ddlCompany.DataTextField = "CompanyName";
        //    ddlCompany.DataValueField = "CompanyId";
        //    ddlCompany.DataBind();


        //    ddlSCompany.DataSource = companyBOList;
        //    ddlSCompany.DataTextField = "CompanyName";
        //    ddlSCompany.DataValueField = "CompanyId";
        //    ddlSCompany.DataBind();

        //    //ListItem item = new ListItem();
        //    //item.Value = "0";
        //    //item.Text = hmUtility.GetDropDownFirstValue();
        //    //ddlCompany.Items.Insert(0, item);
        //}
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
        private void LoadServiceType()
        {
            List<SMSegmentInformationBO> segmentList = new List<SMSegmentInformationBO>();
            SetupDA setupDA = new SetupDA();

            segmentList = setupDA.GetAllSegment();

            ddlServiceType.DataSource = segmentList;
            ddlServiceType.DataTextField = "SegmentName";
            ddlServiceType.DataValueField = "Id";
            ddlServiceType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            if (segmentList.Count > 1)
                ddlServiceType.Items.Insert(0, item);
        }
        private void LoadItemOrServiceDelivery()
        {
            SalesQuotationNBillingDA serviceDA = new SalesQuotationNBillingDA();
            List<ItemOrServiceDeliveryBO> serviceList = new List<ItemOrServiceDeliveryBO>();
            serviceList = serviceDA.GetItemOrServiceDelivery();

            ddlDeliveryBy.DataSource = serviceList;
            ddlDeliveryBy.DataTextField = "DeliveryTypeName";
            ddlDeliveryBy.DataValueField = "ItemServiceDeliveryId";
            ddlDeliveryBy.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlDeliveryBy.Items.Insert(0, item);
        }
        private void LoadBillingPeriod()
        {
            SalesQuotationNBillingDA serviceDA = new SalesQuotationNBillingDA();
            List<SMBillingPeriodBO> serviceList = new List<SMBillingPeriodBO>();
            serviceList = serviceDA.GetBillingPeriod().Where(i => i.ActiveStat).ToList();

            ddlBillingPeriod.DataSource = serviceList;
            ddlBillingPeriod.DataTextField = "BillingPeriodName";
            ddlBillingPeriod.DataValueField = "BillingPeriodId";
            ddlBillingPeriod.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlBillingPeriod.Items.Insert(0, item);
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
        private void LoadCurrentVendor()
        {
            SalesQuotationNBillingDA serviceDA = new SalesQuotationNBillingDA();
            List<SMCurrentVendorBO> serviceList = new List<SMCurrentVendorBO>();
            serviceList = serviceDA.GetCurrentVendor();

            ddlCurrentVendor.DataSource = serviceList;
            ddlCurrentVendor.DataTextField = "VendorName";
            ddlCurrentVendor.DataValueField = "CurrentVendorId";
            ddlCurrentVendor.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCurrentVendor.Items.Insert(0, item);
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

            //ddlItemStockBy.DataSource = headListBO;
            //ddlItemStockBy.DataTextField = "HeadName";
            //ddlItemStockBy.DataValueField = "UnitHeadId";
            //ddlItemStockBy.DataBind();
            //ddlItemStockBy.Items.Insert(0, item);
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

            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //ddlContractPeriod.Items.Insert(0, item);
        }

        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadSalesCallNotification()
        {
            List<SalesCallViewBO> viewList = new List<SalesCallViewBO>();
            SalesCallDA salescallDA = new SalesCallDA();

            string todaysdate = DateTime.Now.ToShortDateString();

            UserInformationDA userInformationDA = new UserInformationDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            viewList = salescallDA.GetAllSalesCallInfo();
            viewList = viewList.Where(a => a.ShowFollowupDate == todaysdate).ToList();

            if (viewList != null)
            {
                if (viewList.Count > 0)
                {
                    foreach (SalesCallViewBO bo in viewList)
                    {
                        UserInformationBO participant = new UserInformationBO();
                        participant = userInformationDA.GetUserInformationByEmpId(bo.EmpId);


                        bool IsMessageSendAllGroupUser = false;

                        CommonMessageDA messageDa = new CommonMessageDA();
                        CommonMessageBO message = new CommonMessageBO();
                        CommonMessageDetailsBO detailBO = new CommonMessageDetailsBO();
                        List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();

                        message.Subjects = "Followup Notification";
                        message.MessageBody = "You have a following followup today. ";
                        message.MessageBody += "Company: " + bo.CompanyName;
                        message.MessageBody += " Address: " + bo.CompanyAddress;
                        message.MessageBody += " Followup Type: " + bo.FollowupType;
                        message.MessageBody += " Purpose: " + bo.Purpose;
                        message.MessageFrom = userInformationBO.UserInfoId;
                        message.MessageFromUserId = userInformationBO.UserId;
                        message.MessageDate = DateTime.Now;
                        message.Importance = "Normal";

                        detailBO.MessageTo = participant.UserInfoId;
                        detailBO.UserId = participant.UserId;
                        messageDetails.Add(detailBO);

                        bool status = messageDa.SaveMessage(message, messageDetails, IsMessageSendAllGroupUser);
                        if (status)
                        {
                            (Master as HM).MessageCount();
                        }
                    }
                }
            }
        }
        private void LoadCompanyIndependentContacts()
        {
            List<ContactInformationBO> contacts = new List<ContactInformationBO>();
            ContactInformationDA contactDA = new ContactInformationDA();
            contacts = contactDA.GetContactInformation().Where(x => x.CompanyId == 0).ToList();

            ddlIndependentContact.DataSource = contacts;
            ddlIndependentContact.DataTextField = "Name";
            ddlIndependentContact.DataValueField = "Id";
            ddlIndependentContact.DataBind();

        }
        private void LoadCRMConfiguaration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDeviceOrUserWillShow", "IsDeviceOrUserWillShow");

            dvDeviceNUser.Visible = setUpBO.SetupValue == "1";

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDeliveryWillShow", "IsDeliveryWillShow");

            dvDelivery.Visible = setUpBO.SetupValue == "1";

        }
        private void CheckIsQuotationCreateFromSiteServeyFeedback()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsQuotationCreateFromSiteServeyFeedback", "IsQuotationCreateFromSiteServeyFeedback");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsQuotationCreateFromSiteServeyFeedback.Value = setUpBO.SetupValue;
            }
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
        //************************ User Defined WebMethod ********************//
        [WebMethod]
        public static GridViewDataNPaging<SMCompanySalesCallBO, GridPaging> SearchSalesCallInfo(string companyName, string fromIniDate, string toIniDate, string fromFolupDate, string toFolupDate, string folUpTypeId, string purposeId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int? cmpId = 0; DateTime? frmIDate = null, toIDate = null, frmFDate = null, toFDate = null;
            int? folupId, purId;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<SMCompanySalesCallBO, GridPaging> myGridData = new GridViewDataNPaging<SMCompanySalesCallBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (!string.IsNullOrWhiteSpace(fromIniDate))
            {
                //frmIDate = Convert.ToDateTime(fromIniDate);
                frmIDate = CommonHelper.DateTimeToMMDDYYYY(fromIniDate);
            }
            if (!string.IsNullOrWhiteSpace(toIniDate))
            {
                //toIDate = Convert.ToDateTime(toIniDate);
                toIDate = CommonHelper.DateTimeToMMDDYYYY(toIniDate);
            }
            if (!string.IsNullOrWhiteSpace(fromFolupDate))
            {
                //frmFDate = Convert.ToDateTime(fromFolupDate);
                frmFDate = CommonHelper.DateTimeToMMDDYYYY(fromFolupDate);
            }
            if (!string.IsNullOrWhiteSpace(toFolupDate))
            {
                //toFDate = Convert.ToDateTime(toFolupDate);
                toIDate = CommonHelper.DateTimeToMMDDYYYY(toFolupDate);
            }
            if (folUpTypeId == "0")
            {
                folupId = null;
            }
            else
            {
                folupId = Convert.ToInt32(folUpTypeId);
            }
            if (purposeId == "0")
            {
                purId = null;
            }
            else
            {
                purId = Convert.ToInt32(purposeId);
            }

            HMCommonDA commonDA = new HMCommonDA();
            SalesCallDA salesCallDA = new SalesCallDA();
            List<SMCompanySalesCallBO> salesCallList = new List<SMCompanySalesCallBO>();
            salesCallList = salesCallDA.GetSalesCallInfoBySearchCriteriaForPaging(companyName, frmIDate, toIDate, frmFDate, toFDate, folupId, purId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<SMCompanySalesCallBO> distinctItems = new List<SMCompanySalesCallBO>();
            distinctItems = salesCallList.GroupBy(test => test.SalesCallId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;

        }
        [WebMethod]
        public static SalesQuotationEditBO LoadQuotation(Int64 quotationId)
        {
            SalesQuotationEditBO quotationv = new SalesQuotationEditBO();
            SalesQuotationNBillingDA salesDA = new SalesQuotationNBillingDA();

            quotationv.Quotation = salesDA.GetQuotationById(quotationId);
            quotationv.QuotationItemDetails = salesDA.GetQuotationItemDetailsById(quotationId, CommonHelper.QuotationItemType.Item.ToString());
            quotationv.QuotationServiceDetails = salesDA.GetQuotationServiceDetailsById(quotationId, CommonHelper.QuotationItemType.Service.ToString());
            quotationv.QuotationDetails = salesDA.GetQuotationDetailsById(quotationId).OrderBy(i=>i.ItemType).ToList();

            return quotationv;
        }
        [WebMethod]
        public static ReturnInfo DeleteSalesCallInfo(int salesCallId)
        {
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                SalesCallDA salesCallDA = new SalesCallDA();
                Boolean status = salesCallDA.DeleteSalesCallInfo(salesCallId);
                if (status)
                {
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
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return rtninf;
        }
        [WebMethod]
        public static GuestCompanyBO LoadCompanyInfo(int companyId)
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            return companyDA.GetGuestCompanyInfoById(companyId);
        }
        [WebMethod]
        public static ArrayList PopulateLocations(string cityId)
        {
            ArrayList list = new ArrayList();
            List<LocationBO> projectList = new List<LocationBO>();
            LocationDA entityDA = new LocationDA();
            projectList = entityDA.GetLocationInfoByCityId(Convert.ToInt32(cityId));
            int count = projectList.Count;
            for (int i = 0; i < count; i++)
            {
                list.Add(new ListItem(
                                        projectList[i].LocationName.ToString(),
                                        projectList[i].LocationId.ToString()
                                         ));
            }
            return list;
        }
        [WebMethod]
        public static List<CompanySiteBO> LoadCompanySite(int companyId)
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<CompanySiteBO> siteList = new List<CompanySiteBO>();
            siteList = companyDA.GetCompanySiteByCompanyId(companyId);

            return siteList;
        }

        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemNCategoryAutoSearch(string itemName, int categoryId, int isCustomerItem, string itemType)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemSearchQuatation(itemName, itemType, categoryId, Convert.ToBoolean(isCustomerItem));

            return itemInfo;
        }
        [WebMethod]
        public static List<InvItemAutoSearchBO> GetItemServiceByCategory(string itemName, int categoryId, int isCustomerItem)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemServiceByCategory(itemName, categoryId, Convert.ToBoolean(isCustomerItem));

            return itemInfo;
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
        public static ReturnInfo SaveQuotation(SMQuotationBO Quotation, List<SMQuotationDetailsBO> QuotationItemDetails, List<SMQuotationDetailsBO> QuotationServiceDetails,
                                               List<SMQuotationDetailsBO> QuotationDeletedItemDetails, List<SMQuotationDetailsBO> QuotationDeletedServiceDetails,
                                               List<SMQuotationDetailsBO>  QuotationDetail, List<long> DeletedQuotationDetail, List<long> DeletedQuotationDiscountDetail)
        {
            ReturnInfo rtninf = new ReturnInfo();
            int quotationId = 0;
            string porderNumber = string.Empty;
            bool status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                SalesQuotationNBillingDA salesNQuotationDa = new SalesQuotationNBillingDA();
                
                if (Quotation.QuotationId == 0)
                {
                    Quotation.CreatedBy = userInformationBO.UserInfoId;
                    status = salesNQuotationDa.SaveQuotation(Quotation, QuotationItemDetails, QuotationServiceDetails, QuotationDetail,  out quotationId);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.SMQuotation.ToString(), quotationId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMQuotation));
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }
                else
                {
                    string DeletedQuotationDetailIdList = string.Join(",", DeletedQuotationDetail);
                    string DeletedQuotationDiscountDetailIdList = string.Join(",", DeletedQuotationDiscountDetail);

                    Quotation.LastModifiedBy = userInformationBO.UserInfoId;

                    List<SMQuotationDetailsBO> quotationItemDetailsNewlyAdded = new List<SMQuotationDetailsBO>();
                    List<SMQuotationDetailsBO> quotationSrviceDetailsNewlyAdded = new List<SMQuotationDetailsBO>();

                    List<SMQuotationDetailsBO> quotationItemDvetailsEdited = new List<SMQuotationDetailsBO>();
                    List<SMQuotationDetailsBO> quotationSrviceDetailsEdited = new List<SMQuotationDetailsBO>();

                    quotationItemDetailsNewlyAdded = (from qd in QuotationItemDetails where qd.QuotationDetailsId == 0 && qd.ItemType == CommonHelper.QuotationItemType.Item.ToString() select qd).ToList();
                    quotationSrviceDetailsNewlyAdded = (from qd in QuotationServiceDetails where qd.QuotationDetailsId == 0 && qd.ItemType == CommonHelper.QuotationItemType.Service.ToString() select qd).ToList();

                    quotationItemDvetailsEdited = (from qd in QuotationItemDetails where qd.QuotationDetailsId > 0 && qd.ItemType == CommonHelper.QuotationItemType.Item.ToString() select qd).ToList();
                    quotationSrviceDetailsEdited = (from qd in QuotationServiceDetails where qd.QuotationDetailsId > 0 && qd.ItemType == CommonHelper.QuotationItemType.Service.ToString() select qd).ToList();

                    status = salesNQuotationDa.UpdateQuotation(Quotation, quotationItemDetailsNewlyAdded, quotationSrviceDetailsNewlyAdded,
                                                                quotationItemDvetailsEdited, quotationSrviceDetailsEdited,
                                                                QuotationDeletedItemDetails, QuotationDeletedServiceDetails, QuotationDetail,
                                                                DeletedQuotationDetailIdList, DeletedQuotationDiscountDetailIdList
                                                               );

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.SMQuotation.ToString(), Quotation.QuotationId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMQuotation));
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
        public static ReturnInfo PerformApproval(int quotationId, int dealId)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            SalesQuotationEditBO quotationv = new SalesQuotationEditBO();
            SalesQuotationNBillingDA salesDA = new SalesQuotationNBillingDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            try
            {

                //quotationv.Quotation = salesDA.GetQuotationById(quotationId);
                rtninf.IsSuccess = salesDA.UpdateApproval(quotationId, dealId, false, userInformationBO.UserInfoId);

                if (rtninf.IsSuccess)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.SMQuotation.ToString(), quotationId,
                           ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMQuotation));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
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
        public static GridViewDataNPaging<SMQuotationViewBO, GridPaging> GetQuotationForPagination(int companyId, int dealId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<SMQuotationViewBO, GridPaging> myGridData = new GridViewDataNPaging<SMQuotationViewBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            SalesQuotationNBillingDA invItemDA = new SalesQuotationNBillingDA();
            List<SMQuotationViewBO> invItemList = new List<SMQuotationViewBO>();
            invItemList = invItemDA.GetQuotationForPagination(companyId, dealId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(invItemList, totalRecords);

            return myGridData;
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
        public static List<SMDeal> GetAllDealByCompanyIdNContactId(int? companyId, long? contactId)
        {
            DealDA dealDA = new DealDA();
            List<SMDeal> dealList = new List<SMDeal>();

            dealList = dealDA.GetAllDealByCompanyIdNContactIdForDropdown(companyId, contactId);

            return dealList;
        }

        [WebMethod]
        public static SMContactInformationViewBO GetContactInfoById(int id)
        {
            ContactInformationDA contactDA = new ContactInformationDA();
            SMContactInformationViewBO contactInformation = new SMContactInformationViewBO();

            contactInformation = contactDA.GetContactInformationByIdForView(id);

            return contactInformation;
        }

        [WebMethod]
        public static List<InvItemAutoSearchBO> LoadQuotationDetailsFromSiteServeyFeedback(int feedbackId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            SalesQuotationNBillingDA salesDA = new SalesQuotationNBillingDA();

            itemInfo = salesDA.GetQuotationDetailsFromSiteServeyFeedback(feedbackId);

            return itemInfo;
        }

        [WebMethod(EnableSession = true)]
        public static void SaveItemNServiceForCostAnalysis(List<SMQuotationDetailsBO> QuotationItemDetails, List<SMQuotationDetailsBO> QuotationServiceDetails)
        {
            HttpContext.Current.Session["QuotationItem"] = QuotationItemDetails;
            HttpContext.Current.Session["QuotationService"] = QuotationServiceDetails;

        }

        [WebMethod]
        public static SalesQuotationEditBO LoadCostAnalysis(Int64 costAnalysisId)
        {
            SalesQuotationEditBO quotationv = new SalesQuotationEditBO();
            SalesQuotationNBillingDA salesDA = new SalesQuotationNBillingDA();

            SMCostAnalysisView costAnalysis = new SMCostAnalysisView();
            CostAnalysisDA costAnalysisDA = new CostAnalysisDA();

            costAnalysis = costAnalysisDA.GetCostAnalysisViewById(costAnalysisId);
            costAnalysis.Items = costAnalysis.AllItems.Where(i => i.ItemType == CommonHelper.CostAnalysisItemType.Item.ToString()).ToList();
            costAnalysis.Services = costAnalysis.AllItems.Where(i => i.ItemType == CommonHelper.CostAnalysisItemType.Service.ToString()).ToList();

            quotationv.Quotation = new SMQuotationBO();
            quotationv.QuotationItemDetails = (from i in costAnalysis.Items
                                               select new SMQuotationDetailsBO
                                               {
                                                   ItemType = i.ItemType,
                                                   CategoryId = i.CategoryId != null ? (int)i.CategoryId : 0,
                                                   ServicePackageId = i.ServicePackageId != null ? (int)i.ServicePackageId : 0,
                                                   ServiceBandWidthId = i.ServiceBandWidthId != null ? (int)i.ServiceBandWidthId : 0,
                                                   ServiceTypeId = i.ServiceTypeId != null ? (int)i.ServiceTypeId : 0,
                                                   ItemId = i.ItemId != null ? (int)i.ItemId : 0,
                                                   StockBy = i.StockBy != null ? (int)i.StockBy : 0,
                                                   Quantity = i.Quantity,
                                                   UnitPrice = i.UnitPrice,
                                                   TotalPrice = i.TotalOfferedPrice,
                                                   Name = i.ItemName,
                                                   ItemName = i.ItemName,
                                                   HeadName = i.StockByName,
                                                   SalesQuantity = i.Quantity,
                                                   //UpLink = i.UpLink,
                                                   AverageCost = i.AverageCost,
                                               }).ToList();
            quotationv.QuotationServiceDetails = (from i in costAnalysis.Services
                                                  select new SMQuotationDetailsBO
                                                  {
                                                      ItemType = i.ItemType,
                                                      CategoryId = i.CategoryId != null ? (int)i.CategoryId : 0,
                                                      ServicePackageId = i.ServicePackageId != null ? (int)i.ServicePackageId : 0,
                                                      ServiceBandWidthId = i.ServiceBandWidthId != null ? (int)i.ServiceBandWidthId : 0,
                                                      ServiceTypeId = i.ServiceTypeId != null ? (int)i.ServiceTypeId : 0,
                                                      ItemId = i.ItemId != null ? (int)i.ItemId : 0,
                                                      StockBy = i.StockBy != null ? (int)i.StockBy : 0,
                                                      Quantity = i.Quantity,
                                                      UnitPrice = i.UnitPrice,
                                                      TotalPrice = i.TotalOfferedPrice,
                                                      Name = i.ItemName,
                                                      ItemName = i.ItemName,
                                                      HeadName = i.StockByName,
                                                      SalesQuantity = i.Quantity,
                                                      //UpLink = i.UpLink,
                                                      AverageCost = i.AverageCost,
                                                  }).ToList();
            return quotationv;
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
        public static List<SMQuotationDiscountDetailsView> GetTypeList(string serviceType)
        {
            List<SMQuotationDiscountDetailsView> detailsViews = new List<SMQuotationDiscountDetailsView>();

            if (serviceType == "Restaurant")
            {
                InvItemClassificationCostCenterMappingDA invItemClassificationDA = new InvItemClassificationCostCenterMappingDA();
                var List = invItemClassificationDA.GetActiveItemClassificationInfo();

                detailsViews = List.Select(i => new SMQuotationDiscountDetailsView
                {
                    Type = "Classification",
                    TypeName = i.ClassificationName,
                    TypeId = i.ClassificationId
                }).ToList();
            }
            else if (serviceType == "GuestRoom")
            {
                RoomTypeDA roomTypeDA = new RoomTypeDA();
                var List = roomTypeDA.GetRoomTypeInfo();

                detailsViews = List.Select(i => new SMQuotationDiscountDetailsView
                {
                    Type = "RoomType",
                    TypeName = i.RoomType,
                    TypeId = i.RoomTypeId
                }).ToList();
            }
            else if (serviceType == "Banquet")
            {
                InvItemClassificationCostCenterMappingDA invItemClassificationDA = new InvItemClassificationCostCenterMappingDA();
                var List = invItemClassificationDA.GetActiveItemClassificationInfo();

                detailsViews.Add(new SMQuotationDiscountDetailsView {
                    Type = "HallRent",
                    TypeName = "Hall Rent"
                });
                detailsViews.Add(new SMQuotationDiscountDetailsView
                {
                    Type = "Requisites",
                    TypeName = "Requisites"
                });

                detailsViews.AddRange(List.Select(i => new SMQuotationDiscountDetailsView
                {
                    Type = "Classification",
                    TypeName = i.ClassificationName,
                    TypeId = i.ClassificationId
                }).ToList());
            }
            else if (serviceType == "ServiceOutlet")
            {
                HotelGuestServiceInfoDA guestHouseServiceDA = new HotelGuestServiceInfoDA();
                var List = guestHouseServiceDA.GetHotelGuestServiceInfo(1, 0, 0);
                detailsViews = List.Select(i => new SMQuotationDiscountDetailsView
                {
                    Type = "GuestService",
                    TypeName = i.ServiceName,
                    TypeId = i.ServiceId
                }).ToList();
            }

            return detailsViews;
        }

    }
}