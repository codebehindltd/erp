using HotelManagement.Data.HMCommon;
using HotelManagement.Data.LCManagement;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.LCManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.LCManagement
{
    public partial class CNFTransaction : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCurrency();
                LoadCNFName();
                LoadBank();
            }
        }
        private void LoadCNFName()
        {
            LcCnfInfoDA cnfInfoDA = new LcCnfInfoDA();
            List<LCCnfInfoBO> entityBOList = new List<LCCnfInfoBO>();
            entityBOList = cnfInfoDA.GetAllCNFInfoList();

            this.ddlCNFName.DataSource = entityBOList;
            this.ddlCNFName.DataTextField = "Name";
            this.ddlCNFName.DataValueField = "SupplierId";
            this.ddlCNFName.DataBind();

            ListItem list = new ListItem();
            list.Value = "0";
            list.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCNFName.Items.Insert(0, list);
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
        [WebMethod]
        public static CommonCurrencyBO LoadCurrencyType(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetCommonCurrencyInfoById(currecyType);
            return commonCurrencyBO;
        }
        [WebMethod]
        public static ReturnInfo SaveUpdateCNFTransaction(CNFTransactionBO CNFTransactionBO)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (CNFTransactionBO.Id == 0)
            {
                CNFTransactionBO.CreatedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            else
            {
                CNFTransactionBO.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            int OutId = 0;
            CNFTransactionDA DA = new CNFTransactionDA();

            status = DA.SaveOrUpdateCNFTransaction(CNFTransactionBO, out OutId);
            if (status)
            {
                if (CNFTransactionBO.Id == 0)
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.CNFTransaction.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.LCManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CNFTransaction));

                }
                else
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.CNFTransaction.ToString(), OutId,
                           ProjectModuleEnum.ProjectModule.LCManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CNFTransaction));
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
        public static CommonCurrencyConversionBO LoadCurrencyConversionRate(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyConversionBO conversionBO = new CommonCurrencyConversionBO();
            conversionBO = commonCurrencyDA.GetCurrencyConversionRate(currecyType);
            return conversionBO;
        }
        [WebMethod]
        public static GridViewDataNPaging<CNFTransactionBO, GridPaging> LoadTransactionSearch(DateTime fromDate, DateTime toDate, string transactionType, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<CNFTransactionBO, GridPaging> myGridData = new GridViewDataNPaging<CNFTransactionBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<CNFTransactionBO> contactInformation = new List<CNFTransactionBO>();
            CNFTransactionDA DA = new CNFTransactionDA();
            contactInformation = DA.GetTransactionInformationPagination(fromDate, toDate, Convert.ToInt32(userInformationBO.UserInfoId), transactionType,  userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(contactInformation, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static CNFTransactionBO FillForm(int id)
        {
            CNFTransactionBO cNF = new CNFTransactionBO();
            CNFTransactionDA DA = new CNFTransactionDA();
            cNF = DA.GetCNFTransactionById(id);

            return cNF;
        }
        [WebMethod]
        public static ReturnInfo DeleteTransaction(int Id)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            HMCommonDA DA = new HMCommonDA();
            status = DA.DeleteInfoById("CNFTransaction", "Id", Id);
            if (status)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.CNFTransaction.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.LCManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CNFTransaction));
            }
            return rtninfo;
        }
        [WebMethod]
        public static ReturnInfo TransactionApproval(int Id, string approvedStatus)
        {
            ReturnInfo rtninf = new ReturnInfo();
            CNFTransactionDA DA = new CNFTransactionDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = DA.TransactionApproval( Id, approvedStatus, userInformationBO.UserInfoId);

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

    }
}