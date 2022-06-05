using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.VehicleManagement;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.VehicleManagement;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.VehicleManagement
{
    public partial class VMVehicleInformation : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAccountHead();
                LoadManufacturer();
                LoadVehicleType();
            }
        }
        private void LoadAccountHead()
        {
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
            entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("3").Where(m => m.IsTransactionalHead == true).ToList();

            ddlAccountHeadId.DataSource = entityBOList;
            ddlAccountHeadId.DataTextField = "HeadWithCode";
            ddlAccountHeadId.DataValueField = "NodeId";
            ddlAccountHeadId.DataBind();

            ddlAccountHeadIdSrc.DataSource = entityBOList;
            ddlAccountHeadIdSrc.DataTextField = "HeadWithCode";
            ddlAccountHeadIdSrc.DataValueField = "NodeId";
            ddlAccountHeadIdSrc.DataBind();

            ListItem listItem = new ListItem();
            listItem.Value = "0";
            listItem.Text = hmUtility.GetDropDownFirstValue();
            ddlAccountHeadId.Items.Insert(0, listItem);
            ddlAccountHeadIdSrc.Items.Insert(0, listItem);
        }
        private void LoadManufacturer()
        {
            VMSetupDA setupDA = new VMSetupDA();
            VMVehicleManufacturerDA manufacturerDA = new VMVehicleManufacturerDA();
            List<VMManufacturerBO> manufacturerBO = new List<VMManufacturerBO>();

            manufacturerBO = manufacturerDA.GetManufacturerInfoForDDL();

            ddlManufacturerId.DataSource = manufacturerBO;
            ddlManufacturerId.DataTextField = "BrandName";
            ddlManufacturerId.DataValueField = "Id";
            ddlManufacturerId.DataBind();

            ddlManufacturerIdSrc.DataSource = manufacturerBO;
            ddlManufacturerIdSrc.DataTextField = "BrandName";
            ddlManufacturerIdSrc.DataValueField = "Id";
            ddlManufacturerIdSrc.DataBind();

            ListItem listItem = new ListItem();
            listItem.Value = "0";
            listItem.Text = hmUtility.GetDropDownFirstValue();
            ddlManufacturerId.Items.Insert(0, listItem);
            ddlManufacturerIdSrc.Items.Insert(0, listItem);
        }
        private void LoadVehicleType()
        {
            VMVehicleTypeDA typeDA = new VMVehicleTypeDA();
            List<VMVehicleTypeBO> typeBOs = new List<VMVehicleTypeBO>();

            typeBOs = typeDA.GetTypeInfoForDDL();

            ddlVehicleType.DataSource = typeBOs;
            ddlVehicleType.DataTextField = "TypeName";
            ddlVehicleType.DataValueField = "Id";
            ddlVehicleType.DataBind();

            ddlVehicleTypeSrc.DataSource = typeBOs;
            ddlVehicleTypeSrc.DataTextField = "TypeName";
            ddlVehicleTypeSrc.DataValueField = "Id";
            ddlVehicleTypeSrc.DataBind();

            ListItem listItem = new ListItem();
            listItem.Value = "0";
            listItem.Text = hmUtility.GetDropDownFirstValue();
            ddlVehicleType.Items.Insert(0, listItem);
            ddlVehicleTypeSrc.Items.Insert(0, listItem);
        }

        [WebMethod]
        public static ReturnInfo SaveUpdate(VMVehicleInformationBO vMVehicle)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            int OutId = 0;
            long OwnerIdForDocuments = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            VMSetupDA setupDA = new VMSetupDA();
            try
            {
                vMVehicle.CreatedBy = userInformationBO.UserInfoId;
                rtninfo.IsSuccess = setupDA.SaveUpdateVehicle(vMVehicle, out OutId);

                if (rtninfo.IsSuccess)
                {
                    if (vMVehicle.Id == 0)
                    {
                        OwnerIdForDocuments = OutId;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                EntityTypeEnum.EntityType.VMDriverInformation.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.VMDriverInformation));
                    }
                    else
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                                EntityTypeEnum.EntityType.VMDriverInformation.ToString(), vMVehicle.Id,
                            ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.VMDriverInformation));
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return rtninfo;
        }
        [WebMethod]
        public static GridViewDataNPaging<VMVehicleInformationBO, GridPaging> SearchGridPaging(string vehicleName, string model, string airConditioning, int manufacturerId, int accountHeadId, int vehicleTypeId, string status, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            VMSetupDA setupDA = new VMSetupDA();
            List<VMVehicleInformationBO> infoBOs = new List<VMVehicleInformationBO>();
            int totalRecords = 0;

            GridViewDataNPaging<VMVehicleInformationBO, GridPaging> myGridData = new GridViewDataNPaging<VMVehicleInformationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            infoBOs = setupDA.GetVehicleInformationGridding( vehicleName,  model,  airConditioning,  manufacturerId,  accountHeadId,  vehicleTypeId,  status, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);
            myGridData.GridPagingProcessing(infoBOs, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static VMVehicleInformationBO FillForm(long Id)
        {
            VMSetupDA vMSetupDA = new VMSetupDA();
            VMVehicleInformationBO infoBO = new VMVehicleInformationBO();
            infoBO = vMSetupDA.GetVehicleInformationById(Id);

            return infoBO;
        }
        [WebMethod]
        public static ReturnInfo DeleteData(long Id)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            HMCommonDA DA = new HMCommonDA();
            status = DA.DeleteInfoById("VMVehicleInformation", "Id", Id);
            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.LostNFound.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LostNFound));
            }
            return rtninf;
        }
    }
}