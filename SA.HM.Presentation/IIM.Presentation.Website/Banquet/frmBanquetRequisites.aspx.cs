using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Banquet;
using HotelManagement.Data.Banquet;
using HotelManagement.Data.HMCommon;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using System.Text.RegularExpressions;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.Banquet
{
    public partial class frmBanquetRequisites : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        protected int isEntryPanelVisible = -1;
        //**************************** Handlers ****************************//

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if(!IsPostBack)
            {
                LoadAccountHead();
                this.IsBanquetIntegrateWithAccounts();
                LoadExpenseAccountHead();
                CheckPermission();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
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


                BanquetRequisitesBO entityBO = new BanquetRequisitesBO();
                BanquetRequisitesDA entityDA = new BanquetRequisitesDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                entityBO.Name = txtName.Text;
                entityBO.Code = this.txtCode.Text;
                entityBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
                entityBO.UnitPrice = Convert.ToDecimal(txtUnitPrice.Text);
                entityBO.Description = this.txtDescription.Text;
                
                if (ddlAccountHead.SelectedIndex == 0 && hfIsBanquetIntegrateWithAccounts.Value == "0")
                {
                    entityBO.AccountsPostingHeadId = ddlAccountHead.SelectedIndex;
                }
                else
                {
                    entityBO.AccountsPostingHeadId = Convert.ToInt32(ddlAccountHead.SelectedValue);
                }
                if (ddlExpAccountHead.SelectedIndex == 0 && hfIsBanquetIntegrateWithAccounts.Value == "0")
                {
                    entityBO.ExpenseAccountsPostingHeadId = ddlExpAccountHead.SelectedIndex;
                }
                else
                {
                    entityBO.ExpenseAccountsPostingHeadId = Convert.ToInt32(ddlExpAccountHead.SelectedValue);
                }

                if (string.IsNullOrWhiteSpace(txtRequisitesId.Value))
                {
                    if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Requisites  Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        this.txtName.Focus();
                        return;
                    }
                    else if (DuplicateCheckDynamicaly("Code", this.txtCode.Text, 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Requisites  Code" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        txtCode.Focus();
                        return;
                    }

                    long tmpRequisitesId = 0;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = entityDA.SaveBanquetRequisitesInfo(entityBO, out tmpRequisitesId);
                    if (status)
                    {
                        this.isNewAddButtonEnable = 2;
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.BanquetRequisites.ToString(), tmpRequisitesId
                       , ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetRequisites));

                        this.Cancel();
                    }
                }
                else
                {
                    
                    if (DuplicateCheckDynamicaly("Name", this.txtName.Text, 1) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Requisites Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        this.txtName.Focus();
                        return;
                    }
                    else if (DuplicateCheckDynamicaly("Code", this.txtCode.Text, 1) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Requisites Code" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        txtCode.Focus();

                        return;
                    }
                    entityBO.Id = Convert.ToInt64(txtRequisitesId.Value);
                    entityBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = entityDA.UpdateBanquetRequisitesInfo(entityBO);
                    if (status)
                    {
                        this.isNewAddButtonEnable = 2;
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.BanquetRequisites.ToString(), entityBO.Id,
                        ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetRequisites));
                        this.Cancel();
                    }
                }
                
                this.SetTab("EntryTab");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
               
            }
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "BanquetRequisites";
            string pkFieldName = "Id";
            string pkFieldValue = this.txtRequisitesId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }

        protected void gvRequisites_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvRequisites.PageIndex = e.NewPageIndex;
            this.LoadGridView();
        }
        protected void gvRequisites_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gvRequisites_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int itemId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(itemId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                try
                {
                    HMCommonDA hmCommonDA = new HMCommonDA();
                    Boolean status = hmCommonDA.DeleteInfoById("BanquetRequisites", "Id", itemId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.BanquetRequisites.ToString(), itemId,
                        ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetRequisites));
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
        //************************ User Defined Function ********************//
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
            string Code = txtSItemCode.Text;
            Boolean ActiveStat = ddlSActiveStat.SelectedIndex == 0 ? true : false;

            BanquetRequisitesDA da = new BanquetRequisitesDA();
            List<BanquetRequisitesBO> files = da.GetBanquetRequisitesInfoBySearchCriteria(Name, Code, ActiveStat);

            this.gvRequisites.DataSource = files;
            this.gvRequisites.DataBind();
            this.SetTab("SearchTab");
        }
        private void Cancel()
        {
            this.ddlActiveStat.SelectedValue = "0";
            this.txtName.Text = string.Empty;
            this.txtCode.Text = string.Empty;
            this.hiddenTxtCode.Value = string.Empty;
            this.txtDescription.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.txtUnitPrice.Text = string.Empty;
            this.txtRequisitesId.Value = string.Empty;
            this.txtName.Focus();
            this.ddlAccountHead.SelectedValue = "0";
            this.ddlExpAccountHead.SelectedValue = "0";
        }
        private bool IsFormValid()
        {
            bool status = true;
            if (string.IsNullOrWhiteSpace(this.txtName.Text))
            {
                this.isEntryPanelVisible = 1;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Requisites Name.", AlertType.Warning);
                this.txtName.Focus();
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtCode.Text))
            {
                this.isEntryPanelVisible = 1;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Requisites Code.", AlertType.Warning);
                this.txtName.Focus();
                status = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtUnitPrice.Text))
            {
                this.isEntryPanelVisible = 1;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Unit Price.", AlertType.Warning);
                this.txtUnitPrice.Focus();
                status = false;
            }
            else if (!string.IsNullOrWhiteSpace(this.txtUnitPrice.Text))
            {
                var match = Regex.Match(txtUnitPrice.Text, @"^[1-9]\d*(\.\d+)?$");
                if (!match.Success)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Unit Price" + AlertMessage.FormatValidation, AlertType.Warning);
                    this.txtUnitPrice.Focus();
                    status = false;
                }
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

        //[WebMethod]
        //public static string DeleteData(int sEmpId)
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        HMCommonDA hmCommonDA = new HMCommonDA();

        //        Boolean status = hmCommonDA.DeleteInfoById("RestaurantItem", "ItemId", sEmpId);
        //        if (status)
        //        {
        //            result = "success";
        //        }
        //    }
        //    catch
        //    {
        //        //lblMessage.Text = "Data Deleted Failed.";
        //    }

        //    return result;
        //}

        public void FillForm(long EditId)
        {
            BanquetRequisitesBO entityBO = new BanquetRequisitesBO();
            BanquetRequisitesDA entityDA = new BanquetRequisitesDA();

            entityBO = entityDA.GetBanquetRequisitesInfoById(EditId);

            ddlActiveStat.SelectedValue = (entityBO.ActiveStat == true ? 0 : 1).ToString();
            txtRequisitesId.Value = entityBO.Id.ToString();
            txtName.Text = entityBO.Name;
            txtCode.Text = entityBO.Code;
            hiddenTxtCode.Value = entityBO.Code;
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
                if (IsBanquetIntegrateWithAccountsBO.SetupValue=="1")
                {
                    hfIsBanquetIntegrateWithAccounts.Value = "1";
                    pnlIsBanquetIntegrateWithAccounts.Visible = true;
                }
            }
        }
    }
}