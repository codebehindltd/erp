using HotelManagement.Data.HMCommon;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class InvServiceBandwidth : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        UserInformationBO userInformationBO = new UserInformationBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFrequency();
                CheckPermission();
            }
        }
        private void CheckPermission()
        {
            hfViewPermission.Value = isViewPermission ? "1" : "0";
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }
        private void LoadFrequency()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> titleList = new List<CustomFieldBO>();
            titleList = commonDA.GetCustomField("BandwidthServiceFrequency");

            ddlUplinkFrequency.DataSource = titleList;
            ddlUplinkFrequency.DataValueField = "FieldValue";
            ddlUplinkFrequency.DataTextField = "Description";
            ddlUplinkFrequency.DataBind();

            ddlDownlinkFrequency.DataSource = titleList;
            ddlDownlinkFrequency.DataValueField = "FieldValue";
            ddlDownlinkFrequency.DataTextField = "Description";
            ddlDownlinkFrequency.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlUplinkFrequency.Items.Insert(0, item);
            ddlDownlinkFrequency.Items.Insert(0, item);
        }

        [WebMethod]
        public static GridViewDataNPaging<ServiceBandwidthBO, GridPaging> GridPaging(string name, bool status, int gridRecordsCount, int pageNumber, int IsCurrentOrPreviousPage)
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<ServiceBandwidthBO, GridPaging> myGridData = new GridViewDataNPaging<ServiceBandwidthBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, IsCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<ServiceBandwidthBO> bandwidthBOs = new List<ServiceBandwidthBO>();
            SalesQuotationNBillingDA billingDA = new SalesQuotationNBillingDA();
            bandwidthBOs = billingDA.GetServiceBandwidthGriding(name, status, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(bandwidthBOs, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo Delete(int Id)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            int id = 0;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            //CustomNoticeDA customNoticeDA = new CustomNoticeDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            status = hmCommonDA.DeleteInfoById("InvServiceBandwidth", "ServiceBandWidthId", Id);
            if (status)
            {
                info.IsSuccess = true;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                info.Data = 0;
            }
            else
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }

        [WebMethod]
        public static ServiceBandwidthBO FillForm(int Id)
        {
            ServiceBandwidthBO bandwidthBO = new ServiceBandwidthBO();
            SalesQuotationNBillingDA billingDA = new SalesQuotationNBillingDA();
            bandwidthBO = billingDA.GetServiceBandwidthById(Id);
            return bandwidthBO;
        }
        [WebMethod]
        public static ReturnInfo SaveOrUpdateServiceBandwidth(ServiceBandwidthBO bandwidthBO)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            long outId = 0;
            SalesQuotationNBillingDA billingDA = new SalesQuotationNBillingDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            bandwidthBO.CreatedBy = userInformationBO.UserInfoId;

            try
            {
                status = billingDA.SaveOrUpdateBandwidth(bandwidthBO, out outId);

                if (status)
                {

                    info.IsSuccess = true;
                    if (bandwidthBO.ServiceBandWidthId == 0)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.ServiceBandwidth.ToString(), outId, ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ServiceBandwidth));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        info.Data = 0;
                    }
                    else
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.ServiceBandwidth.ToString(), bandwidthBO.ServiceBandWidthId, ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ServiceBandwidth));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }
                else
                {
                    info.IsSuccess = false;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }

            }
            catch (Exception ex)
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }
            return info;
        }
    }
}