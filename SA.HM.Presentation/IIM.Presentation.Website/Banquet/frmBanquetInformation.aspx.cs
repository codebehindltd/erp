using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;
using HotelManagement.Data.Banquet;
using HotelManagement.Entity.Banquet;
using HotelManagement.Data.HMCommon;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.Banquet
{
    public partial class frmBanquetInformation : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        protected int isEntryPanelVisible = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadBanquetStatus();
                LoadCostCenter();
                LoadAccountHead();
                LoadExpenseAccountHead();
                IsBanquetIntegrateWithAccounts();
                CheckPermission();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFormValid())
                {
                    this.isNewAddButtonEnable = 2;
                    return;
                }
                BanquetInformationBO entityBO = new BanquetInformationBO();
                BanquetInformationDA entityDA = new BanquetInformationDA();
                UserInformationBO userInformationBO = new UserInformationBO();

                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                entityBO.CostCenterId = Int32.Parse(ddlCostCenter.SelectedValue);
                entityBO.Name = txtName.Text;
                entityBO.Capacity = Convert.ToDecimal(this.txtCapacity.Text);
                entityBO.Status = Int32.Parse(ddlStatus.SelectedValue);
                entityBO.UnitPrice = Convert.ToDecimal(txtUnitPrice.Text);
                entityBO.Description = this.txtDescription.Text;
                
                if (ddlAccountHead.SelectedIndex == 0 && hfIsBanquetIntegrateWithAccounts.Value == "0")
                {
                    entityBO.AccountsPostingHeadId = ddlAccountHead.SelectedIndex;
                }
                else
                {
                    entityBO.AccountsPostingHeadId = Convert.ToInt64(ddlAccountHead.SelectedValue);
                }
                if (ddlExpAccountHead.SelectedIndex == 0 && hfIsBanquetIntegrateWithAccounts.Value == "0")
                {
                    entityBO.ExpenseAccountsPostingHeadId = ddlExpAccountHead.SelectedIndex;
                }
                else
                {
                    entityBO.ExpenseAccountsPostingHeadId = Convert.ToInt64(ddlExpAccountHead.SelectedValue);
                }
                if (string.IsNullOrWhiteSpace(txtBanquetId.Value))
                {
                    if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Banquet Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        this.txtName.Focus();
                        return;
                    }


                    Int64 tmpBanquetId = 0;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = entityDA.SaveBanquetInformation(entityBO, out tmpBanquetId);
                    if (status)
                    {
                        this.isNewAddButtonEnable = 2;
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.BanquetInformation.ToString(), tmpBanquetId, ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetInformation));

                        this.Cancel();
                    }
                }
                else
                {
                    if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 1) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Theme Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        this.txtName.Focus();
                        return;
                    }

                    entityBO.Id = Convert.ToInt64(this.txtBanquetId.Value);
                    entityBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = entityDA.UpdateBanquetInfo(entityBO);
                    if (status)
                    {
                        this.isNewAddButtonEnable = 2;
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.BanquetInformation.ToString(), entityBO.Id,
                            ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetInformation));
                        this.Cancel();
                    }
                }
                this.SetTab("EntryTab");
            }
            catch (Exception ex)
            {
                //CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
        }
        protected void gvBanquet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                //    imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //    imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvBanquet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int banquetId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(banquetId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                try
                {
                    HMCommonDA hmCommonDA = new HMCommonDA();
                    Boolean status = hmCommonDA.DeleteInfoById("BanquetInformation", "Id", banquetId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.BanquetInformation.ToString(), banquetId, ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetInformation));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    }
                    LoadGridView();
                    this.SetTab("SearchTab");
                }
                catch (Exception ex)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    
                }
            }
        }
        protected void gvBanquet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvBanquet.PageIndex = e.NewPageIndex;
            this.LoadGridView();
        }
        //************************ User Defined Function ********************//
        private void LoadCostCenter()
        {
            CostCentreTabDA entityDA = new CostCentreTabDA();

            var List = entityDA.GetCostCentreTabInfoByType("Banquet");

            this.ddlCostCenter.DataSource = List;
            this.ddlCostCenter.DataTextField = "CostCenter";
            this.ddlCostCenter.DataValueField = "CostCenterId";
            this.ddlCostCenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCostCenter.Items.Insert(0, item);

            //this.ddlSearchCostCenter.DataSource = List;
            //this.ddlSearchCostCenter.DataTextField = "CostCenter";
            //this.ddlSearchCostCenter.DataValueField = "CostCenterId";
            //this.ddlSearchCostCenter.DataBind();
            //this.ddlSearchCostCenter.Items.Insert(0, item);
        }
        public void LoadBanquetStatus()
        {


            RestaurantTableStatusBO bo = new RestaurantTableStatusBO();
            RestaurantTableStatusDA da = new RestaurantTableStatusDA();
            var List = da.GetTableStatusInfo();
            this.ddlStatus.DataSource = List;
            this.ddlStatus.DataTextField = "StatusName";
            this.ddlStatus.DataValueField = "StatusId";
            this.ddlStatus.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlStatus.Items.Insert(0, item);


            this.ddlSStatus.DataSource = List;
            this.ddlSStatus.DataTextField = "StatusName";
            this.ddlSStatus.DataValueField = "StatusId";
            this.ddlSStatus.DataBind();
            this.ddlSStatus.Items.Insert(0, item);
        }
        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isEntryPanelVisible = -1;
            }
        }
        private void LoadGridView()
        {
            string Name = txtSItemName.Text;
            int Status = Int32.Parse(ddlSStatus.SelectedValue);

            BanquetInformationDA da = new BanquetInformationDA();
            List<BanquetInformationBO> files = da.GetBanquetInfoBySearchCriteria(Name, Status);

            this.gvBanquet.DataSource = files;
            this.gvBanquet.DataBind();
            this.SetTab("SearchTab");
        }
        private void Cancel()
        {
            this.ddlCostCenter.SelectedValue = "0";
            this.ddlStatus.SelectedValue = "0";
            this.txtName.Text = string.Empty;
            this.txtCapacity.Text = string.Empty;

            this.txtDescription.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.txtUnitPrice.Text = "";
            this.txtBanquetId.Value = string.Empty;
            this.txtName.Focus();
            this.ddlAccountHead.SelectedValue = "0";
            this.ddlExpAccountHead.SelectedValue = "0";
        }
        private bool IsFormValid()
        {
            bool status = true;
            decimal result;

            if (string.IsNullOrWhiteSpace(this.txtName.Text))
            {
                this.isEntryPanelVisible = 1;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Banquet Name.", AlertType.Warning);
                this.txtName.Focus();
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtCapacity.Text))
            {
                this.isEntryPanelVisible = 1;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Capacity.", AlertType.Warning);
                this.txtCapacity.Focus();
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtUnitPrice.Text))
            {
                this.isEntryPanelVisible = 1;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Unit Price.", AlertType.Warning);
                this.txtUnitPrice.Focus();
                status = false;
            }
            else if (!Decimal.TryParse(txtUnitPrice.Text.ToString(), out result))
            {
                this.isEntryPanelVisible = 1;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Unit Price.", AlertType.Warning);
                this.txtUnitPrice.Focus();
                status = false;
            }
            else if (!Decimal.TryParse(txtCapacity.Text.ToString(), out result))
            {
                this.isEntryPanelVisible = 1;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Capacity.", AlertType.Warning);
                this.txtCapacity.Focus();
                status = false;
            }
            else if (ddlCostCenter.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Cost Center.", AlertType.Warning);
                this.ddlCostCenter.Focus();
                status = false;
            }
            else if (ddlStatus.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Status.", AlertType.Warning);
                this.ddlStatus.Focus();
                status = false;
            }
            else if (ddlAccountHead.SelectedIndex == 0 && hfIsBanquetIntegrateWithAccounts.Value == "1")
            {
                isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, "Account Head" + AlertMessage.FormatValidation, AlertType.Warning);
                ddlAccountHead.Focus();
            }
            else if (ddlExpAccountHead.SelectedIndex == 0 && hfIsBanquetIntegrateWithAccounts.Value == "1")
            {
                isNewAddButtonEnable = 2;
                CommonHelper.AlertInfo(innboardMessage, "Expense Account Head" + AlertMessage.FormatValidation, AlertType.Warning);
                ddlExpAccountHead.Focus();
            }

            return status;
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "BanquetInformation";
            string pkFieldName = "Id";
            string pkFieldValue = this.txtBanquetId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
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
        public void FillForm(Int64 EditId)
        {

            BanquetInformationBO entityBO = new BanquetInformationBO();
            BanquetInformationDA entityDA = new BanquetInformationDA();

            entityBO = entityDA.GetBanquetInformationById(EditId);

            ddlCostCenter.SelectedValue = entityBO.CostCenterId.ToString();
            ddlStatus.SelectedValue = entityBO.Status.ToString();
            txtBanquetId.Value = entityBO.Id.ToString();
            txtName.Text = entityBO.Name;
            txtCapacity.Text = entityBO.Capacity.ToString();
            txtUnitPrice.Text = entityBO.UnitPrice.ToString();
            txtDescription.Text = entityBO.Description;
            ddlAccountHead.SelectedValue = entityBO.AccountsPostingHeadId.ToString();
            ddlExpAccountHead.SelectedValue = entityBO.ExpenseAccountsPostingHeadId.ToString();
        }
        private void LoadAccountHead()
        {
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
            entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("3").Where(m => m.IsTransactionalHead == true).ToList();

            ddlAccountHead.DataSource = entityBOList;
            ddlAccountHead.DataTextField = "HeadWithCode";
            ddlAccountHead.DataValueField = "NodeId";
            ddlAccountHead.DataBind();

            ListItem listItem = new ListItem();
            listItem.Value = "0";
            listItem.Text = hmUtility.GetDropDownFirstValue();
            ddlAccountHead.Items.Insert(0, listItem);
        }
        private void LoadExpenseAccountHead()
        {
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
            entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("4").Where(m => m.IsTransactionalHead == true).ToList();

            ddlExpAccountHead.DataSource = entityBOList;
            ddlExpAccountHead.DataTextField = "HeadWithCode";
            ddlExpAccountHead.DataValueField = "NodeId";
            ddlExpAccountHead.DataBind();

            ListItem listItem = new ListItem();
            listItem.Value = "0";
            listItem.Text = hmUtility.GetDropDownFirstValue();
            ddlExpAccountHead.Items.Insert(0, listItem);
        }
        private void IsBanquetIntegrateWithAccounts()
        {
            hfIsBanquetIntegrateWithAccounts.Value = "0";
            pnlIsBanquetIntegrateWithAccounts.Visible = false;

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO IsBanquetIntegrateWithAccountsBO = new HMCommonSetupBO();

            IsBanquetIntegrateWithAccountsBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetIntegrateWithAccounts", "IsBanquetIntegrateWithAccounts");
            if (IsBanquetIntegrateWithAccountsBO != null)
            {
                if (IsBanquetIntegrateWithAccountsBO.SetupValue == "1")
                {
                    hfIsBanquetIntegrateWithAccounts.Value = "1";
                    pnlIsBanquetIntegrateWithAccounts.Visible = true;
                }
            }
        }
    }
}