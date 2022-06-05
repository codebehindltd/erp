using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Restaurant;
using HotelManagement.Data.Synchronization;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.Synchronization;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class DataSynchronization : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCompany();
                LoadCommonDropDownHiddenField();
                LoadServerIp();
            }
        }

        private void LoadCompany()
        {
            GLCompanyDA companyDA = new GLCompanyDA();
            List<GLCompanyBO> List = new List<GLCompanyBO>();
            List = companyDA.GetAllGLCompanyInfo();

            ddlCompany.DataSource = List;
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlCompany.Items.Insert(0, itemNodeId);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadServerIp()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("DataSynchronizationIPAddress", "DataSynchronizationIPAddress");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                txtServerIp.Value = setUpBO.SetupValue;
            }
        }
        [WebMethod]
        public static List<GLProjectBO> GetGLProjectByGLCompanyId(int companyId)
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            GLProjectDA entityDA = new GLProjectDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            projectList = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(companyId, userInformationBO.UserGroupId);

            return projectList;
        }
        [WebMethod]
        public static GridViewDataNPaging<GLLedgerMasterVwBO, GridPaging> GetVoucherBySearchCriteria(int companyId, int projectId, DateTime? date, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<GLLedgerMasterVwBO, GridPaging> myGridData = new GridViewDataNPaging<GLLedgerMasterVwBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            VoucherEntryDA voucherDa = new VoucherEntryDA();
            List<GLLedgerMasterVwBO> voucher = new List<GLLedgerMasterVwBO>();
            voucher = voucherDa.GetVoucherByCompanyIdNProjectIdNDate(companyId, projectId, date, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(voucher, totalRecords, "SaveVoucherId");

            return myGridData;
        }

        [WebMethod]
        public static GLLedgerMasterBO GetVoucherInformationById(long voucherId)
        {
            GLLedgerMasterBO ledgerMasterBO = new GLLedgerMasterBO();
            CommonDA commonDA = new CommonDA();
            ledgerMasterBO = commonDA.GetTableRow<GLLedgerMasterBO>("GLLedgerMaster", "LedgerMasterId", voucherId.ToString());
            ledgerMasterBO.GLLedgerDetails = commonDA.GetAllTableRow<GLLedgerDetailsBO>("GLLedgerDetails", "LedgerMasterId", voucherId.ToString());
            ledgerMasterBO.GLVoucherApprovedInfos = commonDA.GetAllTableRow<GLVoucherApprovedInfoBO>("GLVoucherApprovedInfo", "DealId", voucherId.ToString());

            return ledgerMasterBO;
        }
        [WebMethod]
        public static ReturnInfo UpdateVoucherSyncInformation(long id, string voucherNo)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            VoucherEntryDA voucherEntryDA = new VoucherEntryDA();
            HMUtility hMUtility = new HMUtility();
            try
            {
                UserInformationBO userInformationBO = hMUtility.GetCurrentApplicationUserInfo();
                returnInfo.IsSuccess = voucherEntryDA.UpdateVoucherSyncInformation(id, voucherNo, userInformationBO.UserInfoId);
                if (returnInfo.IsSuccess)
                {

                    Boolean logStatus = hMUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                                            EntityTypeEnum.EntityType.GeneralLedgerVoucher.ToString(), id, ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(),
                                                   hMUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GeneralLedgerVoucher));
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
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
        public static GridViewDataNPaging<RestaurantBillBO, GridPaging> GetRestaurantBillBySearchCriteria(DateTime date, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int totalRecords = 0;

            GridViewDataNPaging<RestaurantBillBO, GridPaging> myGridData = new GridViewDataNPaging<RestaurantBillBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            List<RestaurantBillBO> entityBOList = new List<RestaurantBillBO>();
            RestaurentBillDA entityDA = new RestaurentBillDA();
            entityBOList = entityDA.GetRestaurantBillInfoByDateAfterDayClose(date, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(entityBOList, totalRecords, "SaveRestaurentBillId");

            return myGridData;
        }
        [WebMethod]
        public static RestaurantDataSyncBO GetRestaurantBilllInformationById(int billId)
        {
            RestaurantDataSyncBO restaurantSyncData = new RestaurantDataSyncBO();
            DataSyncDA dataAccess = new DataSyncDA();
            restaurantSyncData = dataAccess.GetRestaurantRelatedDataToSync(billId);

            return restaurantSyncData;
        }
        [WebMethod]
        public static ReturnInfo UpdateRestaurantBillSyncInformation(int id, string billNumber)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            RestaurentBillDA entityDA = new RestaurentBillDA();
            HMUtility hMUtility = new HMUtility();
            try
            {
                UserInformationBO userInformationBO = hMUtility.GetCurrentApplicationUserInfo();
                returnInfo.IsSuccess = entityDA.UpdateVoucherSyncInformation(id, billNumber, userInformationBO.UserInfoId);
                if (returnInfo.IsSuccess)
                {

                    Boolean logStatus = hMUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                                            EntityTypeEnum.EntityType.GeneralLedgerVoucher.ToString(), id, ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(),
                                                   hMUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GeneralLedgerVoucher));
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
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
    }
}