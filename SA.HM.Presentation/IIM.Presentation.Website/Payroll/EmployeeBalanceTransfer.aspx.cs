using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class EmployeeBalanceTransfer : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadEmployeeInfo();
            }

        }
        private void LoadEmployeeInfo()
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();

            empList = empDa.GetEmployeeInfo();
            ddlTransferFrom.DataSource = empList;
            ddlTransferFrom.DataTextField = "DisplayName";
            ddlTransferFrom.DataValueField = "EmpId";
            ddlTransferFrom.DataBind();

            ddlTransferTo.DataSource = empList;
            ddlTransferTo.DataTextField = "DisplayName";
            ddlTransferTo.DataValueField = "EmpId";
            ddlTransferTo.DataBind();
            
            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            ddlTransferFrom.Items.Insert(0, FirstItem);
            ddlTransferTo.Items.Insert(0, FirstItem);

            ddlTransferFromSearch.DataSource = empList;
            ddlTransferFromSearch.DataTextField = "DisplayName";
            ddlTransferFromSearch.DataValueField = "EmpId";
            ddlTransferFromSearch.DataBind();

            ddlTransferToSearch.DataSource = empList;
            ddlTransferToSearch.DataTextField = "DisplayName";
            ddlTransferToSearch.DataValueField = "EmpId";
            ddlTransferToSearch.DataBind();

            ListItem FirstItemSearch = new ListItem();
            FirstItemSearch.Value = "0";
            FirstItemSearch.Text = hmUtility.GetDropDownFirstAllValue();
            ddlTransferToSearch.Items.Insert(0, FirstItemSearch);
            ddlTransferFromSearch.Items.Insert(0, FirstItemSearch);
        }
        [WebMethod]
        public static ReturnInfo SaveUpdateBalanceTransferInformation(PayrollEmployeeBalanceTransferBO PayrollEmployeeBalanceTransferBO)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (PayrollEmployeeBalanceTransferBO.Id == 0)
            {
                PayrollEmployeeBalanceTransferBO.CreatedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            else
            {
                PayrollEmployeeBalanceTransferBO.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            long OutId;
            PayrollEmployeeBalanceTransferDA DA = new PayrollEmployeeBalanceTransferDA();

            status = DA.SaveUpdateEmployeeBalanceTransferInfo(PayrollEmployeeBalanceTransferBO, out OutId);
            if (status)
            {
                if (PayrollEmployeeBalanceTransferBO.Id == 0)
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpTransfer.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTransfer));

                }
                else
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.EmpTransfer.ToString(), OutId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTransfer));
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
        public static GridViewDataNPaging<PayrollEmployeeBalanceTransferBO, GridPaging> LoadBalanceTransferSearch(int TransferFrom,int TransferTo, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<PayrollEmployeeBalanceTransferBO, GridPaging> myGridData = new GridViewDataNPaging<PayrollEmployeeBalanceTransferBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<PayrollEmployeeBalanceTransferBO> contactInformation = new List<PayrollEmployeeBalanceTransferBO>();
            PayrollEmployeeBalanceTransferDA DA = new PayrollEmployeeBalanceTransferDA();
            contactInformation = DA.GetEmployeeBalanceTransferBySearchCriteriaForPagination(TransferFrom, TransferTo, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(contactInformation, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo ApproveAction(int Id, string approvedStatus)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PayrollEmployeeBalanceTransferDA DA = new PayrollEmployeeBalanceTransferDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = DA.BalanceTransferApproval(Id, approvedStatus, userInformationBO.UserInfoId);

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isTransferProductReceiveDisable = new HMCommonSetupBO();

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    if (approvedStatus == "Checked")
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                    }
                    else if (approvedStatus == "Approved")
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    }
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.EmpTransfer.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTransfer));
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
        public static ReturnInfo PerformDeleteAction(long Id)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            HMCommonDA DA = new HMCommonDA();
            status = DA.DeleteInfoById("PayrollEmployeeBalanceTransfer", "Id", Id);
            if (status)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.EmpTransfer.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTransfer));
            }
            else
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninfo;
        }

        [WebMethod]
        public static PayrollEmployeeBalanceTransferBO FillForm(long Id)
        {

            PayrollEmployeeBalanceTransferBO BalanceTransferBO = new PayrollEmployeeBalanceTransferBO();
            PayrollEmployeeBalanceTransferDA DA = new PayrollEmployeeBalanceTransferDA();
            BalanceTransferBO = DA.GetBalanceTransferById(Id);

            return BalanceTransferBO;
        }
    }
}