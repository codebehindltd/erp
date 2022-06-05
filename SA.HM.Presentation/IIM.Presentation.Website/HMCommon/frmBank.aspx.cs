using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using System.Web.Services;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Newtonsoft.Json;
using HotelManagement.Data.GeneralLedger;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmBank : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        HMCommonDA hmCommonDA = new HMCommonDA();
        //protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
            }

            if (!IsPostBack)
            {
                this.CheckObjectPermission();
                LoadBankAccountsHead();
                GetCommonConfiguration();
            }
        }
        protected void gvGuestHouseService_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.CheckObjectPermission();
        }
        protected void gvGuestHouseService_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvGuestHouseService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int BankId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(BankId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("CommonBank", "BankId", BankId);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                }

                this.SetTab("SearchTab");
            }
        }

        private void LoadBankAccountsHead()
        {
            NodeMatrixDA entityDA = new NodeMatrixDA();
            CustomFieldBO ChequeReceiveAccountsInfo = new CustomFieldBO();
            this.ddlBankAccounts.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE CHARINDEX('.1.5.16.22.', Hierarchy) = 1 AND IsTransactionalHead = 1");

            this.ddlBankAccounts.DataTextField = "NodeHead";
            this.ddlBankAccounts.DataValueField = "NodeId";
            this.ddlBankAccounts.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBankAccounts.Items.Insert(0, itemNodeId);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtBankName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Bank Name.", AlertType.Warning);
                this.txtBankName.Focus();
            }
            else
            {
                BankBO bankBO = new BankBO();
                BankDA bankDA = new BankDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                bankBO.BankName = this.txtBankName.Text;
                bankBO.BranchName = this.txtBranchName.Text;
                bankBO.AccountName = this.txtAccountName.Text;
                bankBO.AccountNumber = this.txtAccountNumber.Text;
                bankBO.AccountType = this.txtAccountType.Text;
                bankBO.Description = this.txtRemarksForBankInfo.Text;
                bankBO.BankHeadId = Convert.ToInt32( ddlBankAccounts.SelectedValue);
                bankBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

                if (string.IsNullOrWhiteSpace(txtBankId.Value))
                {
                    int tmpBankId = 0;
                    bankBO.CreatedBy = userInformationBO.UserInfoId;
                    //Added by Arif [06-11-2017]
                    bool isBankExist = hmCommonDA.DuplicateDataCountDynamicaly("CommonBank", "BankName", bankBO.BankName) > 0;
                    if (isBankExist)
                    {
                        CommonHelper.AlertInfo(innboardMessage, string.Format("Your entered {0} {1} is already exist.", lblSBankName.Text, bankBO.BankName), AlertType.Warning);
                        this.Cancel();
                        return;
                    }
                    Boolean status = bankDA.SaveBankInfo(bankBO, out tmpBankId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.Bank.ToString(), tmpBankId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Bank));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        this.Cancel();
                    }
                }
                else
                {
                    bankBO.BankId = Convert.ToInt32(txtBankId.Value);
                    bankBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean isBankExist = hmCommonDA.DuplicateCheckDynamicaly("CommonBank", "BankName", bankBO.BankName, 1, "BankId", bankBO.BankId.ToString()) > 0;
                    if (isBankExist)
                    {
                        CommonHelper.AlertInfo(innboardMessage, string.Format("Your entered {0} {1} is already exist.", lblSBankName.Text, bankBO.BankName), AlertType.Warning);
                        this.Cancel();
                        return;
                    }
                    Boolean status = bankDA.UpdateBankInfo(bankBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.Bank.ToString(), bankBO.BankId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Bank));
                        this.Cancel();
                    }
                }

                this.SetTab("EntryTab");
            }
        }
        //************************ User Defined Function ********************//
        private void Cancel()
        {
            this.txtBankName.Text = string.Empty;

            this.txtBranchName.Text = string.Empty;
            this.txtAccountName.Text = string.Empty;
            this.txtAccountNumber.Text = string.Empty;
            this.txtAccountType.Text = string.Empty;
            this.txtRemarksForBankInfo.Text = string.Empty;
            this.ddlBankAccounts.SelectedIndex = 0;

            this.ddlActiveStat.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.txtBankId.Value = string.Empty;
            this.txtBankName.Focus();
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmBank.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;

            btnSave.Visible = isSavePermission;
            //if (!isSavePermission)
            //{
            //    isNewAddButtonEnable = -1;
            //}

            if (isSavePermission)
            {
                IsBankSavePermission.Value = "1";
            }
            else
            {
                IsBankSavePermission.Value = "0";
            }

            if (isDeletePermission)
            {
                IsBankDeletePermission.Value = "1";
            }
            else
            {
                IsBankDeletePermission.Value = "0";
            }

        }

        private void GetCommonConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsBankIntegratedWithAccounts", "IsBankIntegratedWithAccounts");

            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsBankIntegratedWithAccounts.Value = setUpBO.SetupValue;
            }
        }
            public void FillForm(int EditId)
        {
            BankBO bankBO = new BankBO();
            BankDA bankDA = new BankDA();
            bankBO = bankDA.GetBankInfoById(EditId);
            ddlActiveStat.SelectedValue = (bankBO.ActiveStat == true ? 0 : 1).ToString();
            txtBankId.Value = bankBO.BankId.ToString();
            txtBankName.Text = bankBO.BankName.ToString();
        }
        private void SetTab(string TabName)
        {
            if (TabName == "SearchTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("CommonBank", "BankId", sEmpId);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.Bank.ToString(), sEmpId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Bank));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rtninf;
        }
        [WebMethod]
        public static string LoadBreadCrumbsInformation()
        {
            string breadCrumbs = string.Empty;
            breadCrumbs = "<span class='divider'>/</span><a href='/HMCommon/frmBank.aspx'>Bank</a><span class='divider'></span>";
            return breadCrumbs;
        }
        [WebMethod]
        public static GridViewDataNPaging<BankBO, GridPaging> SearchBankAndLoadGridInformation(string bankName, Boolean activeStat, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<BankBO, GridPaging> myGridData = new GridViewDataNPaging<BankBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            BankDA bankDA = new BankDA();
            List<BankBO> bankInfoList = new List<BankBO>();
            bankInfoList = bankDA.GetBankInformationBySearchCriteriaForPaging(bankName, activeStat, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<BankBO> distinctItems = new List<BankBO>();
            distinctItems = bankInfoList.GroupBy(test => test.BankName).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static BankBO LoadDetailInformation(int bankId)
        {
            BankDA bankDA = new BankDA();
            return bankDA.GetBankInfoById(bankId);
        }
    }
}