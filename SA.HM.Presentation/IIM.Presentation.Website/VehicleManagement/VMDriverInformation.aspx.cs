using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
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
    public partial class VMDriverInformation : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        bool IsPayrollIntegrateWithVehicleManagement = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCommonDropDownHiddenField();
                LoadIsPayrollIntegrateWithVehicleManagement();
                if (IsPayrollIntegrateWithVehicleManagement)
                {
                    LoadEmployee();

                }
            }
        }
        //private void CheckObjectPermission()
        //{
        //    hfSavePermission.Value = isSavePermission ? "1" : "0";
        //    hfDeletePermission.Value = isDeletePermission ? "1" : "0";
        //    hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        //    hfViewPermission.Value = isViewPermission ? "1" : "0";
        //}
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadEmployee()
        {
            EmployeeDA employeeDA = new EmployeeDA();
            ddlEmployee.DataSource = employeeDA.GetEmployeeInfo();
            ddlEmployee.DataTextField = "EmployeeName";
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataBind();

            ddlEmployeeSrc.DataSource = employeeDA.GetEmployeeInfo();
            ddlEmployeeSrc.DataTextField = "EmployeeName";
            ddlEmployeeSrc.DataValueField = "EmpId";
            ddlEmployeeSrc.DataBind();

            ListItem itemEmployee = new ListItem();
            itemEmployee.Value = "0";
            itemEmployee.Text = "--Please Select--";

            ddlEmployee.Items.Insert(0, itemEmployee);
            ddlEmployeeSrc.Items.Insert(0, itemEmployee);

        }
        private void LoadIsPayrollIntegrateWithVehicleManagement()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollIntegrateWithVehicleManagement", "IsPayrollIntegrateWithVehicleManagement");
            hfIsPayrollIntegrateWithVehicleManagement.Value = "0";
            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        hfIsPayrollIntegrateWithVehicleManagement.Value = "1";
                        IsPayrollIntegrateWithVehicleManagement = true;
                    }
                    else
                    {
                        hfIsPayrollIntegrateWithVehicleManagement.Value = "0";
                        IsPayrollIntegrateWithVehicleManagement = false;
                    }
                }
            }
        }

        [WebMethod]
        public static ReturnInfo SaveUpdate(VMDriverInformationBO VMDriverInformationBO)
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
                VMDriverInformationBO.CreatedBy = userInformationBO.UserInfoId;
                rtninfo.IsSuccess = setupDA.SaveDriver(VMDriverInformationBO, out OutId);

                if (rtninfo.IsSuccess)
                {
                    if (VMDriverInformationBO.Id == 0)
                    {
                        OwnerIdForDocuments = OutId;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                EntityTypeEnum.EntityType.VMDriverInformation.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.VehicleManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.VMDriverInformation));
                    }
                    else
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                                EntityTypeEnum.EntityType.VMDriverInformation.ToString(), VMDriverInformationBO.Id,
                            ProjectModuleEnum.ProjectModule.VehicleManagement.ToString(),
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
        public static GridViewDataNPaging<VMDriverInformationBO, GridPaging> SearchGridPaging(string driverName, string licenceNumber, string phone, int employeeId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            VMSetupDA setupDA = new VMSetupDA();
            List<VMDriverInformationBO> infoBOs = new List<VMDriverInformationBO>();
            int totalRecords = 0;

            GridViewDataNPaging<VMDriverInformationBO, GridPaging> myGridData = new GridViewDataNPaging<VMDriverInformationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            infoBOs = setupDA.GetDriverInformationGridding(driverName, licenceNumber, phone, employeeId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);
            myGridData.GridPagingProcessing(infoBOs, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static VMDriverInformationBO FillForm(long Id)
        {
            VMSetupDA vMSetupDA = new VMSetupDA();
            VMDriverInformationBO infoBO = new VMDriverInformationBO();
            infoBO = vMSetupDA.GetDriverInformationById(Id);

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
            status = DA.DeleteInfoById("VMDriverInformation", "Id", Id);
            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.VMDriverInformation.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.VehicleManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.VMDriverInformation));
            }
            return rtninf;
        }
    }
}