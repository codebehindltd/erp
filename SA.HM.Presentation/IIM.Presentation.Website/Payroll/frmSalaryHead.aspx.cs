using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmSalaryHead : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadSalaryType();
                LoadAccountsHead();
                CheckObjectPermission();
            }
        }
        private void LoadAccountsHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();

            // // ---------- Asset
            List<NodeMatrixBO> entityAssetBOList = new List<NodeMatrixBO>();
            entityAssetBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("1").Where(x => x.IsTransactionalHead == true).ToList();
            entityBOList.AddRange(entityAssetBOList);

            // // ---------- Liabilities
            List<NodeMatrixBO> entityLiabilitiesBOList = new List<NodeMatrixBO>();
            entityLiabilitiesBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("2").Where(x => x.IsTransactionalHead == true).ToList();
            if (entityLiabilitiesBOList != null)
            {
                entityBOList.AddRange(entityLiabilitiesBOList);
            }

            // // ---------- Income
            List<NodeMatrixBO> entityIncomeBOList = new List<NodeMatrixBO>();
            entityIncomeBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("3").Where(x => x.IsTransactionalHead == true).ToList();
            if (entityIncomeBOList != null)
            {
                entityBOList.AddRange(entityIncomeBOList);
            }

            // // ---------- Expenditure
            List<NodeMatrixBO> entityExpenditureBOList = new List<NodeMatrixBO>();
            entityExpenditureBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("4").Where(x => x.IsTransactionalHead == true).ToList();
            if (entityExpenditureBOList != null)
            {
                entityBOList.AddRange(entityExpenditureBOList);
            }

            ddlNodeId.DataSource = entityBOList;
            ddlNodeId.DataTextField = "HeadWithCode";
            ddlNodeId.DataValueField = "NodeId";
            ddlNodeId.DataBind();

            ListItem itemAccountsHead = new ListItem();
            itemAccountsHead.Value = "0";
            itemAccountsHead.Text = hmUtility.GetDropDownFirstValue();
            ddlNodeId.Items.Insert(0, itemAccountsHead);
        }
        public void LoadSalaryType()
        {
            SalaryTypeBO salaryFormulaDA = new SalaryTypeBO();
            List<SalaryTypeBO> fields = new List<SalaryTypeBO>();

            fields = salaryFormulaDA.SalaryTypeList();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO salryHeadSetup = new HMCommonSetupBO();
            salryHeadSetup = commonSetupDA.GetCommonConfigurationInfo("SalaryProcessDependsOn", "SalaryProcessDependsOn");

            if (salryHeadSetup.SetupValue.ToString() == "Basic")
            {
                fields = fields.Where(f => f.SalaryTypeValue != "AdditionalAllowance").ToList();
            }

            ddlSalaryType.DataSource = fields;
            ddlSalaryType.DataTextField = "SalaryType";
            ddlSalaryType.DataValueField = "SalaryTypeValue";
            ddlSalaryType.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlSalaryType.Items.Insert(0, itemNodeId);

            ddlSHeadType.DataSource = fields;
            ddlSHeadType.DataTextField = "SalaryType";
            ddlSHeadType.DataValueField = "SalaryTypeValue";
            ddlSHeadType.DataBind();

            ListItem itemSalaryType = new ListItem();
            itemSalaryType.Value = "0";
            itemSalaryType.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSHeadType.Items.Insert(0, itemSalaryType);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }
        protected void gvSalaryHead_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSalaryHead.PageIndex = e.NewPageIndex;
        }
        protected void gvSalaryHead_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvSalaryHead_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int headId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(headId);
                btnSave.Visible = isUpdatePermission;
                btnSave.Text = "Update";
                SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("PayrollSalaryHead", "SalaryHeadId", headId);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.SalaryHead.ToString(), headId,
                               ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalaryHead));
                    }
                    LoadGridView();
                    SetTab("SearchTab");
                }
                catch (Exception ex)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFrmValid())
                {
                    return;
                }

                SalaryHeadBO salaryHeadBO = new SalaryHeadBO();
                SalaryHeadDA salaryHeadDA = new SalaryHeadDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                salaryHeadBO.SalaryHead = txtSalaryHead.Text.Trim().ToString();
                salaryHeadBO.IsShowOnlyAllownaceDeductionPage = Convert.ToBoolean(Convert.ToInt16(ddlShowOnlyAllownaceDeductionPage.SelectedValue));
                salaryHeadBO.SalaryType = ddlSalaryType.SelectedValue;
                salaryHeadBO.NodeId = Convert.ToInt64(ddlNodeId.SelectedValue);
                salaryHeadBO.TransactionType = ddlTransactionType.SelectedValue;
                if (salaryHeadBO.TransactionType == "Yearly")
                {
                    salaryHeadBO.EffectedMonth = hmUtility.GetDateTimeFromString(txtEffectedMonth.Text, userInformationBO.ServerDateFormat);
                }

                salaryHeadBO.ContributionType = ddlContributionType.SelectedValue;
                salaryHeadBO.VoucherMode = ddlVoucherMode.SelectedValue;
                salaryHeadBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

                if (string.IsNullOrWhiteSpace(txtSalaryHeadId.Value))
                {
                    if (DuplicateCheckDynamicaly("SalaryHead", txtSalaryHead.Text, 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Salary Head" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        txtSalaryHead.Focus();
                        return;
                    }

                    int tmpUserInfoId = 0;
                    salaryHeadBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = salaryHeadDA.SaveSalaryHeadInfo(salaryHeadBO, out tmpUserInfoId);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                               EntityTypeEnum.EntityType.SalaryHead.ToString(), tmpUserInfoId,
                               ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalaryHead));
                        Cancel();
                    }
                }
                else
                {
                    salaryHeadBO.SalaryHeadId = Convert.ToInt32(txtSalaryHeadId.Value);
                    salaryHeadBO.LastModifiedBy = userInformationBO.UserInfoId;

                    Boolean status = salaryHeadDA.UpdateSalaryHeadInfo(salaryHeadBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                               EntityTypeEnum.EntityType.SalaryHead.ToString(), salaryHeadBO.SalaryHeadId,
                               ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalaryHead));
                        Cancel();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {

            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void Cancel()
        {
            SetTab("EntryTab");
            ddlActiveStat.SelectedValue = "0";
            ddlNodeId.SelectedValue = "0";
            txtSalaryHeadId.Value = string.Empty;
            ddlShowOnlyAllownaceDeductionPage.SelectedIndex = 0;
            ddlSalaryType.SelectedIndex = 0;
            ddlTransactionType.SelectedIndex = 0;
            ddlVoucherMode.SelectedIndex = 0;
            txtSalaryHead.Text = string.Empty;
            btnSave.Text = "Save";
            txtSalaryHead.Focus();
            btnSave.Visible = isSavePermission;
        }
        private bool IsFrmValid()
        {
            bool flag = true;

            if (string.IsNullOrWhiteSpace(txtSalaryHead.Text.Trim()))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Salary Head.", AlertType.Warning);
                flag = false;
                txtSalaryHead.Focus();
            }
            return flag;
        }
        public void LoadGridView()
        {
            CheckObjectPermission();
            string salaryHead = txtSSalaryHead.Text;
            int activeStat = Convert.ToInt32(ddlSActiveStat.SelectedValue);
            bool? showOnlyAllownaceDeductionPage = null;

            if (ddlSearchShowOnlyAllownaceDeductionPage.SelectedValue != "")
            {
                if (ddlSearchShowOnlyAllownaceDeductionPage.SelectedValue == "0")
                {
                    showOnlyAllownaceDeductionPage = true;
                }
                else
                {
                    showOnlyAllownaceDeductionPage = false;
                }
            }

            string headType = ddlSHeadType.SelectedValue;
            string transectionType = ddlSTransactionType.SelectedValue;

            SalaryHeadBO salaryHeadBO = new SalaryHeadBO();
            SalaryHeadDA salaryHeadDA = new SalaryHeadDA();
            List<SalaryHeadBO> files = new List<SalaryHeadBO>();
            files = salaryHeadDA.GetSalaryHeadInfoBySearchCriteria(salaryHead, activeStat, showOnlyAllownaceDeductionPage, headType, transectionType);
            gvSalaryHead.DataSource = files;
            gvSalaryHead.DataBind();
            SetTab("SearchTab");
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
        public void FillForm(int EditId)
        {
            SalaryHeadBO salaryHeadBO = new SalaryHeadBO();
            SalaryHeadDA salaryHeadDA = new SalaryHeadDA();
            salaryHeadBO = salaryHeadDA.GetSalaryHeadInfoById(EditId);

            txtSalaryHead.Text = salaryHeadBO.SalaryHead;
            txtSalaryHeadId.Value = salaryHeadBO.SalaryHeadId.ToString();
            ddlActiveStat.SelectedValue = (salaryHeadBO.ActiveStat == true ? 0 : 1).ToString();
            ddlShowOnlyAllownaceDeductionPage.SelectedValue = (salaryHeadBO.IsShowOnlyAllownaceDeductionPage) == true ? "1" : "0";
            ddlSalaryType.SelectedValue = salaryHeadBO.SalaryType;
            ddlTransactionType.SelectedValue = salaryHeadBO.TransactionType;
            ddlContributionType.SelectedValue = salaryHeadBO.ContributionType;
            ddlVoucherMode.SelectedValue = salaryHeadBO.VoucherMode;
            ddlNodeId.SelectedValue = salaryHeadBO.NodeId.ToString();
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "PayrollSalaryHead";
            string pkFieldName = "SalaryHeadId";
            string pkFieldValue = txtSalaryHeadId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
    }
}