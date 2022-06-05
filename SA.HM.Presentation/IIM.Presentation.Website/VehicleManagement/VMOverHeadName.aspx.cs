using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.VehicleManagement;
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
    public partial class VMOverHeadName : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadIncomeAccountHead();
            }
        }
        private void LoadIncomeAccountHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            HMCommonSetupBO isVehicleManagementIntegratedwithAccounts = new HMCommonSetupBO();

            isVehicleManagementIntegratedwithAccounts = commonSetupDA.GetCommonConfigurationInfo("IsVehicleManagementIntegratedwithAccounts", "IsVehicleManagementIntegratedwithAccounts");
            if (isVehicleManagementIntegratedwithAccounts != null)
            {
                if (isVehicleManagementIntegratedwithAccounts.SetupValue == "0")
                {
                    AccountHeadDiv.Visible = false;
                }
                else
                {
                    AccountHeadDiv.Visible = true;

                    this.ddlIncomeAccountHead.DataSource = entityDA.GetNodeMatrixInfoByAncestorNodeId(4).Where(x => x.IsTransactionalHead == true).ToList();
                    this.ddlIncomeAccountHead.DataTextField = "HeadWithCode";
                    this.ddlIncomeAccountHead.DataValueField = "NodeId";
                    this.ddlIncomeAccountHead.DataBind();

                    ListItem item = new ListItem();
                    item.Value = "0";
                    item.Text = hmUtility.GetDropDownFirstValue();
                    this.ddlIncomeAccountHead.Items.Insert(0, item);
                }
            }

        }
        [WebMethod]
        public static ReturnInfo SaveUpdateVMOverHeadInformation(VMOverHeadBO VMOverHeadBO)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (VMOverHeadBO.Id == 0)
            {
                VMOverHeadBO.CreatedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            else
            {
                VMOverHeadBO.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            int OutId;
            VMOverHeadDA DA = new VMOverHeadDA();

            status = DA.SaveVMOverHeadNameInfo(VMOverHeadBO, out OutId);
            if (status)
            {
                if (VMOverHeadBO.Id == 0)
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.LCOverHead.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LCOverHead));

                }
                else
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.LCOverHead.ToString(), OutId,
                           ProjectModuleEnum.ProjectModule.FrontOffice.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LCOverHead));
                }


            }
            else
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }
            return rtninfo;
        }
        [WebMethod]
        public static GridViewDataNPaging<VMOverHeadBO, GridPaging> SearchPaidServiceAndLoadGridInformation(string serviceName, bool activeStat, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<VMOverHeadBO, GridPaging> myGridData = new GridViewDataNPaging<VMOverHeadBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            VMOverHeadDA paidServiceDA = new VMOverHeadDA();
            List<VMOverHeadBO> paidServiceList = new List<VMOverHeadBO>();
            paidServiceList = paidServiceDA.GetOverHeadInfoBySearchCriteriaForPagination(serviceName, activeStat, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<VMOverHeadBO> distinctItems = new List<VMOverHeadBO>();
            distinctItems = paidServiceList.GroupBy(test => test.Id).Select(group => group.First()).ToList();


            //myGridData.GridPagingProcessing(guestInfoList, totalRecords);
            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate, string id)
        {
            string tableName = "VMOverHead";
            string pkFieldName = "Id";
            string pkFieldValue = id;
            int IsDuplicate = 0;
            
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        [WebMethod]
        public static VMOverHeadBO FillForm(int Id)
        {

            VMOverHeadBO LCOverHeadBO = new VMOverHeadBO();
            VMOverHeadDA DA = new VMOverHeadDA();
            LCOverHeadBO = DA.GetLCOverHeadNameInfoById(Id);

            return LCOverHeadBO;
        }
        [WebMethod]
        public static ReturnInfo DeleteVMOverHead(long Id)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            HMCommonDA DA = new HMCommonDA();
            status = DA.DeleteInfoById("VMOverHead", "Id", Id);
            if (status)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.LCOverHead.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.FrontOffice.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LCOverHead));
            }
            return rtninfo;
        }
    }
}