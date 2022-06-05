using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class PriceMatrix : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckPermission();
                LoadFrequencyUnit();
                LoadShareRatio();
                LoadFrequency();
                LoadService();
            }
        }
        private void LoadService()
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();
            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemServiceByCategory(string.Empty, 0, false);


            ddlServiceItem.DataSource = itemInfo;
            ddlServiceItem.DataValueField = "ItemId";
            ddlServiceItem.DataTextField = "Name";
            ddlServiceItem.DataBind();

            ddlSearchServiceItem.DataSource = itemInfo;
            ddlSearchServiceItem.DataValueField = "ItemId";
            ddlSearchServiceItem.DataTextField = "Name";
            ddlSearchServiceItem.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlServiceItem.Items.Insert(0, item);

            ListItem item2 = new ListItem();
            item2.Value = "0";
            item2.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSearchServiceItem.Items.Insert(0, item2);
        }
        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }
        private void LoadFrequencyUnit()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> titleList = new List<CustomFieldBO>();
            titleList = commonDA.GetCustomField("BandwidthServiceFrequency");

            ddlUplinkFrequencyUnit.DataSource = titleList;
            ddlUplinkFrequencyUnit.DataValueField = "FieldValue";
            ddlUplinkFrequencyUnit.DataTextField = "Description";
            ddlUplinkFrequencyUnit.DataBind();

            ddlDownlinkFrequencyUnit.DataSource = titleList;
            ddlDownlinkFrequencyUnit.DataValueField = "FieldValue";
            ddlDownlinkFrequencyUnit.DataTextField = "Description";
            ddlDownlinkFrequencyUnit.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlUplinkFrequencyUnit.Items.Insert(0, item);
            ddlDownlinkFrequencyUnit.Items.Insert(0, item);
        }
        private void LoadShareRatio()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> titleList = new List<CustomFieldBO>();
            titleList = commonDA.GetCustomField("ServiceShareRatio");

            ddlShareRatio.DataSource = titleList;
            ddlShareRatio.DataValueField = "FieldValue";
            ddlShareRatio.DataTextField = "Description";
            ddlShareRatio.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlShareRatio.Items.Insert(0, item);
        }
        private void LoadFrequency()
        {
            CommonDA hMCommonDA = new CommonDA();
            List<InvServiceFrequency> frequencies = new List<InvServiceFrequency>();
            frequencies = hMCommonDA.GetAllTableRow<InvServiceFrequency>("InvServiceFrequency").OrderBy(f => f.Frequency).ToList();

            ddlUplinkFrequency.DataSource = frequencies;
            ddlUplinkFrequency.DataValueField = "Id";
            ddlUplinkFrequency.DataTextField = "Frequency";
            ddlUplinkFrequency.DataBind();

            ddlDownlinkFrequency.DataSource = frequencies;
            ddlDownlinkFrequency.DataValueField = "Id";
            ddlDownlinkFrequency.DataTextField = "Frequency";
            ddlDownlinkFrequency.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlUplinkFrequency.Items.Insert(0, item);
            ddlDownlinkFrequency.Items.Insert(0, item);
        }
       
        [WebMethod]
        public static ReturnInfo SaveOrUpdatePriceMatrix(ServicePriceMatrixBO priceMatrix)
        {
            ReturnInfo info = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            InvServicePriceMatrixDA priceMatrixDA = new InvServicePriceMatrixDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            priceMatrix.CreatedBy = userInformationBO.UserInfoId;

            int id = 0;

            try
            {
                info.IsSuccess = priceMatrixDA.SaveOrUpdatePriceMatrix(priceMatrix, out id);

                if (info.IsSuccess)
                {
                    if (priceMatrix.ServicePriceMatrixId == 0)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                                EntityTypeEnum.EntityType.InvServicePriceMatrix.ToString(), id,
                                                    ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                                                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvServicePriceMatrix));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        info.Data = 0;
                    }
                    else
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.InvServicePriceMatrix.ToString(), priceMatrix.ServicePriceMatrixId,
                                ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                                    hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvServicePriceMatrix));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
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
        public static GridViewDataNPaging<ServicePriceMatrixBO, GridPaging> SearchPackage(int itemId, string packageName, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<ServicePriceMatrixBO, GridPaging> myGridData = new GridViewDataNPaging<ServicePriceMatrixBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            
            InvServicePriceMatrixDA priceMatrixDA = new InvServicePriceMatrixDA();

            List<ServicePriceMatrixBO> packageList = new List<ServicePriceMatrixBO>();
            packageList = priceMatrixDA.GetServicePackagesByItemIdNPackageNameWithPagination(itemId, packageName, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(packageList, totalRecords, "SearchPackage");

            return myGridData;
        }

        [WebMethod]
        public static ServicePriceMatrixBO GetPackageById(int servicePriceMatrixId)
        {
            CommonDA commonDA = new CommonDA();
            ServicePriceMatrixBO servicePriceMatrix = new ServicePriceMatrixBO();

            servicePriceMatrix = commonDA.GetTableRow<ServicePriceMatrixBO>("InvServicePriceMatrix", "ServicePriceMatrixId", servicePriceMatrixId.ToString());

            return servicePriceMatrix;
        }

        [WebMethod]
        public static ReturnInfo DeletePriceMatrix(int servicePriceMatrixId)
        {
            ReturnInfo info = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            HMCommonDA commonDA = new HMCommonDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            try
            {
                info.IsSuccess = commonDA.DeleteInfoById("InvServicePriceMatrix", "ServicePriceMatrixId", servicePriceMatrixId);

                if (info.IsSuccess)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                                                EntityTypeEnum.EntityType.InvServicePriceMatrix.ToString(), servicePriceMatrixId,
                                                    ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                                                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvServicePriceMatrix));
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
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