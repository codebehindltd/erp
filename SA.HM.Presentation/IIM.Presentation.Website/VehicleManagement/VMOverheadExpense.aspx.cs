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
    public partial class VMOverheadExpense : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadOverHeadName();
                LoadCashAndCashEquivalantAccountHead();
                LoadCurrency();
                LoadBank();
                LoadCommonDropDownHiddenField();
                LoadVehicleNumber(); 
            }
        }

        private void LoadOverHeadName()
        {
            VMOverHeadDA bankDA = new VMOverHeadDA();
            List<VMOverHeadBO> entityBOList = new List<VMOverHeadBO>();
            entityBOList = bankDA.GetVMOverHeadNameInfo();

            this.ddlOverHeadId.DataSource = entityBOList;
            this.ddlOverHeadId.DataTextField = "OverheadName";
            this.ddlOverHeadId.DataValueField = "Id";
            this.ddlOverHeadId.DataBind();

            this.ddlSrcOverHeadId.DataSource = entityBOList;
            this.ddlSrcOverHeadId.DataTextField = "OverheadName";
            this.ddlSrcOverHeadId.DataValueField = "Id";
            this.ddlSrcOverHeadId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlOverHeadId.Items.Insert(0, itemBank);

            ListItem itemBankSrc = new ListItem();
            itemBankSrc.Value = "0";
            itemBankSrc.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlSrcOverHeadId.Items.Insert(0, itemBankSrc);
        }

        private void LoadCashAndCashEquivalantAccountHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();

            this.ddlIncomeAccountHead.DataSource = entityDA.GetNodeMatrixInfoByAncestorNodeId(16).Where(x => x.IsTransactionalHead == true).ToList();
            this.ddlIncomeAccountHead.DataTextField = "HeadWithCode";
            this.ddlIncomeAccountHead.DataValueField = "NodeId";
            this.ddlIncomeAccountHead.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlIncomeAccountHead.Items.Insert(0, item);
        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("All");

            this.ddlCurrencyType.DataSource = currencyListBO;
            this.ddlCurrencyType.DataTextField = "CurrencyName";
            this.ddlCurrencyType.DataValueField = "CurrencyId";
            this.ddlCurrencyType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCurrencyType.Items.Insert(0, item);
        }
        private void LoadBank()
        {
            BankDA bankDA = new BankDA();
            List<BankBO> entityBOList = new List<BankBO>();
            entityBOList = bankDA.GetBankInfo();

            this.ddlCompanyBank.DataSource = entityBOList;
            this.ddlCompanyBank.DataTextField = "BankName";
            this.ddlCompanyBank.DataValueField = "BankId";
            this.ddlCompanyBank.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCompanyBank.Items.Insert(0, itemBank);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadVehicleNumber()
        {
            VMSetupDA bankDA = new VMSetupDA();
            List<VMVehicleInformationBO> entityBOList = new List<VMVehicleInformationBO>();
            entityBOList = bankDA.GetAllVehicleInformation();

            this.ddlVehicleId.DataSource = entityBOList;
            this.ddlVehicleId.DataTextField = "NumberPlate";
            this.ddlVehicleId.DataValueField = "Id";
            this.ddlVehicleId.DataBind();

            this.ddlSrcVehicleId.DataSource = entityBOList;
            this.ddlSrcVehicleId.DataTextField = "NumberPlate";
            this.ddlSrcVehicleId.DataValueField = "Id";
            this.ddlSrcVehicleId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlVehicleId.Items.Insert(0, itemBank);

            ListItem itemBankSrc = new ListItem();
            itemBankSrc.Value = "0";
            itemBankSrc.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlSrcVehicleId.Items.Insert(0, itemBankSrc);
        }
        [WebMethod]
        public static GridViewDataNPaging<VMOverheadExpenseBO, GridPaging> SearchPaidServiceAndLoadGridInformation(DateTime fromDate, DateTime toDate, int LCId, int overHeadId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            GridViewDataNPaging<VMOverheadExpenseBO, GridPaging> myGridData = new GridViewDataNPaging<VMOverheadExpenseBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            VMOverheadExpenseDA paidServiceDA = new VMOverheadExpenseDA();
            List<VMOverheadExpenseBO> paidServiceList = new List<VMOverheadExpenseBO>();
            paidServiceList = paidServiceDA.GetOverHeadExpenseInfoBySearchCriteriaForPagination(fromDate, toDate, LCId, overHeadId, Convert.ToInt32(userInformationBO.UserInfoId), userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<VMOverheadExpenseBO> distinctItems = new List<VMOverheadExpenseBO>();
            distinctItems = paidServiceList.GroupBy(test => test.Id).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo DeletePaidServiceById(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean delStatus = hmCommonDA.DeleteInfoById("VMOverheadExpense", "Id", sEmpId);
                if (delStatus)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                             EntityTypeEnum.EntityType.HotelService.ToString(), sEmpId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelService));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo SaveOrUpdateOverHeadExpense(VMOverheadExpenseBO VMOverheadExpenseBO)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (VMOverheadExpenseBO.Id == 0)
            {
                VMOverheadExpenseBO.CreatedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            else
            {
                VMOverheadExpenseBO.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            int OutId;
            VMOverheadExpenseDA DA = new VMOverheadExpenseDA();
            status = DA.SaveVMOverHeadExpenseInfo(VMOverheadExpenseBO, out OutId);
            if (status)
            {
                if (VMOverheadExpenseBO.Id == 0)
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
            return rtninfo;
        }
        [WebMethod]
        public static VMOverheadExpenseBO FillForm(int Id)
        {

            VMOverheadExpenseBO VMOverheadExpenseBO = new VMOverheadExpenseBO();
            VMOverheadExpenseDA DA = new VMOverheadExpenseDA();
            VMOverheadExpenseBO = DA.GetLCOverHeadExpenseInfoById(Id);

            return VMOverheadExpenseBO;
        }
        [WebMethod]
        public static ReturnInfo DeleteVehicleOverHeadExpense(long Id)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            HMCommonDA DA = new HMCommonDA();
            status = DA.DeleteInfoById("VMOverheadExpense", "Id", Id);
            if (status)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.LCOverHead.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.LCManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LCOverHead));
            }
            return rtninfo;
        }
        [WebMethod]
        public static ReturnInfo ExpenseApproval(int Id, string approvedStatus)
        {
            ReturnInfo rtninf = new ReturnInfo();
            VMOverheadExpenseDA DA = new VMOverheadExpenseDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = DA.ExpenseApproval(Id, approvedStatus, userInformationBO.UserInfoId);

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
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.PMProductOut.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductOut));
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
        public static CommonCurrencyBO LoadCurrencyType(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetCommonCurrencyInfoById(currecyType);
            return commonCurrencyBO;
        }
        [WebMethod]
        public static CommonCurrencyConversionBO LoadCurrencyConversionRate(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyConversionBO conversionBO = new CommonCurrencyConversionBO();
            conversionBO = commonCurrencyDA.GetCurrencyConversionRate(currecyType);
            return conversionBO;
        }
    }
}