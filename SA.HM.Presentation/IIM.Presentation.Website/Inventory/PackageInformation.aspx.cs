using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
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
    public partial class PackageInformation : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CheckPermission();
        }
        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }

        [WebMethod]
        public static GridViewDataNPaging<InvServicePackage, GridPaging> SearchPackage(string name, bool isActive, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<InvServicePackage, GridPaging> myGridData = new GridViewDataNPaging<InvServicePackage, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            List<InvServicePackage> servicePackages = new List<InvServicePackage>();
            InvServicePackageDA servicePackageDA = new InvServicePackageDA();
            servicePackages = servicePackageDA.GetAllServicePackageWithPagination(name, isActive, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(servicePackages, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo SaveOrUpdate(InvServicePackage ServicePackage)
        {
            ReturnInfo info = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            InvServicePackageDA servicePackageDA = new InvServicePackageDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ServicePackage.CreatedBy = userInformationBO.UserInfoId;

            int id = 0;

            try
            {
                info.IsSuccess = servicePackageDA.SaveOrUpdatePackage(ServicePackage, out id);

                if (info.IsSuccess)
                {
                    if (ServicePackage.ServicePackageId == 0)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                                EntityTypeEnum.EntityType.InvServicePackage.ToString(), id,
                                                    ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                                                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvServicePackage));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        info.Data = 0;
                    }
                    else
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.InvServicePackage.ToString(), ServicePackage.ServicePackageId,
                                ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                                    hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvServicePackage));
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
        public static InvServicePackage GetPackageInformationById(int id)
        {
            InvServicePackage stage = new InvServicePackage();
            CommonDA commonDA = new CommonDA();

            stage = commonDA.GetTableRow<InvServicePackage>("InvServicePackage", "ServicePackageId", id.ToString());

            return stage;
        }

        [WebMethod]
        public static ReturnInfo DeletePackage(int id)
        {
            ReturnInfo info = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            InvServicePackageDA servicePackageDA = new InvServicePackageDA();

            try
            {
                info.IsSuccess = hmCommonDA.DeleteInfoById("InvServicePackage", "ServicePackageId", id);

                if (info.IsSuccess)
                {

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                        EntityTypeEnum.EntityType.InvServicePackage.ToString(), id,
                            ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                                hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.InvServicePackage));
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