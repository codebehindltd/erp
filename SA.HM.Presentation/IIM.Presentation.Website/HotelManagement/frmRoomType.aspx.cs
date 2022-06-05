using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Data.HMCommon;
using System.Web.Services;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HMCommon;
using Newtonsoft.Json;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmRoomType : BasePage
    {
        HiddenField innboardMessage;
        protected int isNewAddButtonEnable = 1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.CheckObjectPermission();
                this.LoadCurrency();
                this.LoadAccountHead();
                this.IsFrontOfficeIntegrateWithAccounts();
                IsShowMinimumRoomRate();
            }
        }
        protected void gvRoomTypeInfo_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Decimal number;
                if (string.IsNullOrWhiteSpace(txtRoomType.Text))
                {
                    isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Room Type.", AlertType.Warning);
                    txtRoomType.Focus();
                }
                else if (string.IsNullOrWhiteSpace(txtTypeCode.Text))
                {
                    isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Type Code.", AlertType.Warning);
                    txtTypeCode.Focus();
                }
                else if (string.IsNullOrWhiteSpace(txtPaxQuantity.Text))
                {
                    isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Type Pax Quantity.", AlertType.Warning);
                    txtPaxQuantity.Focus();
                }
                else if (!Decimal.TryParse(txtRoomRate.Text, out number))
                {
                    isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, "Room rate" + AlertMessage.FormatValidation, AlertType.Warning);
                    txtRoomRate.Focus();
                }
                else if (!Decimal.TryParse(txtRoomRateUSD.Text, out number))
                {
                    isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, "Room rate" + AlertMessage.FormatValidation, AlertType.Warning);
                    txtRoomRateUSD.Focus();
                }
                else if (dvMinimumRoomRate.Visible && string.IsNullOrWhiteSpace(txtMinimumRoomRate.Text))
                {
                    isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, "Minimum Room rate" + AlertMessage.FormatValidation, AlertType.Warning);
                    txtMinimumRoomRate.Focus();
                    return;
                }
                else if (dvMinimumRoomRate.Visible && string.IsNullOrWhiteSpace(txtMinimumRoomRateUSD.Text))
                {
                    isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, "Minimum Room rate" + AlertMessage.FormatValidation, AlertType.Warning);
                    txtMinimumRoomRateUSD.Focus();
                    return;
                }
                else if (ddlAccountHead.SelectedIndex == 0 && hfIsFrontOfficeIntegrateWithAccounts.Value == "1")
                {
                    isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, "Account Head" + AlertMessage.FormatValidation, AlertType.Warning);
                    ddlAccountHead.Focus();

                }
                else if (number > 0)
                {
                    RoomTypeBO roomTypeBO = new RoomTypeBO();
                    RoomTypeDA roomTypeDA = new RoomTypeDA();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    roomTypeBO.RoomType = txtRoomType.Text;
                    roomTypeBO.TypeCode = txtTypeCode.Text;
                    roomTypeBO.RoomRate = Convert.ToDecimal(txtRoomRate.Text);
                    roomTypeBO.RoomRateUSD = Convert.ToDecimal(txtRoomRateUSD.Text);
                    if (dvMinimumRoomRate.Visible)
                    {
                        roomTypeBO.MinimumRoomRate = Convert.ToDecimal(txtMinimumRoomRate.Text);
                        roomTypeBO.MinimumRoomRateUSD = Convert.ToDecimal(txtMinimumRoomRateUSD.Text);
                    }
                    roomTypeBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
                    roomTypeBO.SuiteType = ddlSuiteType.SelectedIndex == 0 ? true : false;
                    roomTypeBO.PaxQuantity = Convert.ToInt32(txtPaxQuantity.Text);
                    if (ddlAccountHead.SelectedIndex == 0 && hfIsFrontOfficeIntegrateWithAccounts.Value == "0")
                        roomTypeBO.AccountsPostingHeadId = ddlAccountHead.SelectedIndex;
                    else
                        roomTypeBO.AccountsPostingHeadId = Convert.ToInt32(ddlAccountHead.SelectedValue);

                    if (string.IsNullOrWhiteSpace(txtRoomTypeId.Value))
                    {
                        if (DuplicateCheckDynamicaly("RoomType", txtRoomType.Text, 0) == 1)
                        {
                            isNewAddButtonEnable = 2;
                            CommonHelper.AlertInfo(innboardMessage, "Room Type" + AlertMessage.DuplicateValidation, AlertType.Warning);
                            txtRoomType.Focus();
                            return;
                        }
                        int tmpRoomTypeId = 0;
                        roomTypeBO.CreatedBy = userInformationBO.UserInfoId;
                        Boolean status = roomTypeDA.SaveRoomTypeInfo(roomTypeBO, out tmpRoomTypeId);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomType.ToString(), tmpRoomTypeId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomType));
                            Cancel();
                        }
                    }
                    else
                    {
                        if (DuplicateCheckDynamicaly("RoomType", txtRoomType.Text, 1) == 1)
                        {
                            isNewAddButtonEnable = 2;
                            txtRoomType.Focus();
                            CommonHelper.AlertInfo(innboardMessage, "Room Type " + AlertMessage.DuplicateValidation, AlertType.Warning);
                            return;
                        }

                        roomTypeBO.RoomTypeId = Convert.ToInt32(txtRoomTypeId.Value);
                        roomTypeBO.LastModifiedBy = userInformationBO.UserInfoId;
                        Boolean status = roomTypeDA.UpdateRoomTypeInfo(roomTypeBO);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomType.ToString(), roomTypeBO.RoomTypeId,
                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomType));

                            Cancel();
                        }
                    }
                }
                else
                {
                    isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, "Room rate" + AlertMessage.FormatValidation, AlertType.Warning);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        protected void gvRoomTypeInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            lblMessage.Text = string.Empty;
            gvRoomTypeInfo.PageIndex = e.NewPageIndex;
            LoadGridView();
        }
        protected void gvRoomTypeInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int typetId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(typetId);
                btnSave.Text = "Update";
                SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("HotelRoomType", "RoomTypeId", typetId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.RoomType.ToString(), typetId,
                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomType));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    }
                }
                catch (Exception ex)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
                LoadGridView();
                SetTab("SearchTab");
            }
        }
        //************************ User Defined Function ********************//
        private void IsShowMinimumRoomRate()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO IsShowMinimumRoomRate = new HMCommonSetupBO();
            IsShowMinimumRoomRate = commonSetupDA.GetCommonConfigurationInfo("IsMinimumRoomRateCheckingForRoomTypeEnable", "IsMinimumRoomRateCheckingForRoomTypeEnable");
            dvMinimumRoomRate.Visible = IsShowMinimumRoomRate.SetupValue == "1";
        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("LocalNUsd");

            List<CommonCurrencyBO> localCurrencyListBO = new List<CommonCurrencyBO>();
            localCurrencyListBO = currencyListBO.Where(x => x.CurrencyType == "Local").ToList();
            List<CommonCurrencyBO> UsdCurrencyListBO = new List<CommonCurrencyBO>();
            UsdCurrencyListBO = currencyListBO.Where(x => x.CurrencyType == "Usd").ToList();

            ddlSellingPriceLocal.DataSource = localCurrencyListBO;
            ddlSellingPriceLocal.DataTextField = "CurrencyName";
            ddlSellingPriceLocal.DataValueField = "CurrencyId";
            ddlSellingPriceLocal.DataBind();
            ddlSellingPriceLocal.SelectedIndex = 0;
            lblSellingPriceLocal.Text = "Room Rate(" + ddlSellingPriceLocal.SelectedItem.Text + ")";

            ddlSellingPriceUsd.DataSource = UsdCurrencyListBO;
            ddlSellingPriceUsd.DataTextField = "CurrencyName";
            ddlSellingPriceUsd.DataValueField = "CurrencyId";
            ddlSellingPriceUsd.DataBind();
            ddlSellingPriceUsd.SelectedIndex = 1;
            lblSellingPriceUsd.Text = "Room Rate(" + ddlSellingPriceUsd.SelectedItem.Text + ")";
        }
        private void LoadAccountHead()
        {
            //HMCommonDA hmCommonDA = new HMCommonDA();
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
        private void IsFrontOfficeIntegrateWithAccounts()
        {
            hfIsFrontOfficeIntegrateWithAccounts.Value = "0";
            pnlIsFrontOfficeIntegrateWithAccounts.Visible = false;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isFrontOfficeIntegrateWithAccountsBO = new HMCommonSetupBO();
            isFrontOfficeIntegrateWithAccountsBO = commonSetupDA.GetCommonConfigurationInfo("IsFrontOfficeIntegrateWithAccounts", "IsFrontOfficeIntegrateWithAccounts");
            if (isFrontOfficeIntegrateWithAccountsBO != null)
            {
                if (isFrontOfficeIntegrateWithAccountsBO.SetupValue == "1")
                {
                    hfIsFrontOfficeIntegrateWithAccounts.Value = "1";
                    pnlIsFrontOfficeIntegrateWithAccounts.Visible = true;
                }
            }
        }
        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void LoadGridView()
        {
            CheckObjectPermission();
            string RoomType = txtSRoomType.Text;
            bool ActiveState = ddlSStatus.SelectedIndex == 0 ? true : false;
            int suiteType = Convert.ToInt32(ddlSSuiteType.SelectedValue);
            RoomTypeDA da = new RoomTypeDA();
            List<RoomTypeBO> files = da.GetRoomTypeInfoBySearchCriteria(RoomType, ActiveState, suiteType);

            gvRoomTypeInfo.DataSource = files;
            gvRoomTypeInfo.DataBind();
            SetTab("SearchTab");
        }
        private void Cancel()
        {
            txtRoomType.Text = string.Empty;
            txtTypeCode.Text = string.Empty;
            txtRoomRate.Text = string.Empty;
            txtRoomRateUSD.Text = string.Empty;
            txtMinimumRoomRate.Text = string.Empty;
            txtMinimumRoomRateUSD.Text = string.Empty;
            txtPaxQuantity.Text = string.Empty;
            ddlActiveStat.SelectedIndex = 0;
            ddlSuiteType.SelectedIndex = 0;
            txtRoomTypeId.Value = string.Empty;
            btnSave.Text = "Save";
            ddlAccountHead.SelectedValue = "0";
            txtRoomType.Focus();
            SetTab("EntryTab");
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "HotelRoomType";
            string pkFieldName = "RoomTypeId";
            string pkFieldValue = txtRoomTypeId.Value;
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
        public void FillForm(int EditId)
        {
            RoomTypeBO roomTypeBO = new RoomTypeBO();
            RoomTypeDA roomTypeDA = new RoomTypeDA();

            roomTypeBO = roomTypeDA.GetRoomTypeInfoById(EditId);

            txtRoomType.Text = roomTypeBO.RoomType.ToString();
            txtTypeCode.Text = roomTypeBO.TypeCode.ToString();
            txtRoomRate.Text = roomTypeBO.RoomRate.ToString();
            txtRoomRateUSD.Text = roomTypeBO.RoomRateUSD.ToString();
            if (dvMinimumRoomRate.Visible)
            {
                txtMinimumRoomRate.Text = roomTypeBO.MinimumRoomRate.ToString();
                txtMinimumRoomRateUSD.Text = roomTypeBO.MinimumRoomRateUSD.ToString();
            }
            txtRoomTypeId.Value = roomTypeBO.RoomTypeId.ToString();
            txtPaxQuantity.Text = roomTypeBO.PaxQuantity.ToString();
            ddlSuiteType.SelectedIndex = roomTypeBO.SuiteType == true ? 0 : 1;
            ddlAccountHead.SelectedValue = roomTypeBO.AccountsPostingHeadId.ToString();
        }

    }
}