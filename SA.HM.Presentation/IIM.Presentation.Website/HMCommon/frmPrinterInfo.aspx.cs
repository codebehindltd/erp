using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmPrinterInfo : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadRestaurantKitchen();
                this.LoadCostCenter();
            }

            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }

            PrinterInfoBO typeBO = new PrinterInfoBO();
            PrinterInfoDA typeDA = new PrinterInfoDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            typeBO.CostCenterId = Convert.ToInt32(ddlCostCenterId.SelectedValue);
            typeBO.StockType = this.ddlStockType.SelectedValue;
            if (this.ddlStockType.SelectedValue == "KitchenItem")
            {
                typeBO.KitchenId = Convert.ToInt32(this.ddlKitchen.SelectedValue);
            }
            else
            {
                typeBO.KitchenId = 0;
            }
            typeBO.PrinterName = this.txtPrinterName.Text.ToString();

            typeBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

            if (string.IsNullOrWhiteSpace(txtPrinterInfoId.Value))
            {
                var printerBO = typeDA.GetPrinterInfoByCostCenterNType(typeBO.CostCenterId, typeBO.StockType);
                if (printerBO.PrinterInfoId > 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "A " + typeBO.StockType + " Printer already saved for this Cost Center.", AlertType.Warning);
                    return;
                }

                int tmpPrinterId = 0;
                typeBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = typeDA.SavePrinterInfo(typeBO, out tmpPrinterId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.Printer.ToString(), tmpPrinterId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Printer));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    this.Cancel();
                }
            }
            else
            {
                typeBO.PrinterInfoId = Convert.ToInt32(txtPrinterInfoId.Value);
                typeBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = typeDA.UpdatePrinterInfo(typeBO);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.Printer.ToString(), typeBO.PrinterInfoId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Printer));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    this.Cancel();
                }
            }

            this.SetTab("EntryTab");
        }
        protected void gvRestaurentTypeService_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvRestaurentTypeService.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void gvRestaurentTypeService_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                //  imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //  imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvRestaurentTypeService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int typeId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(typeId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("CommonPrinterInfo", "PrinterInfoId", typeId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.Printer.ToString(), typeId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Printer));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                }
                LoadGridView();
                this.SetTab("SearchTab");
            }
        }
        //************************ User Defined Function ********************//
        private bool IsFrmValid()
        {
            bool flag = true;

            if (this.ddlCostCenterId.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Cost Center.", AlertType.Warning);
                this.ddlCostCenterId.Focus();
                flag = false;
            }
            else if (string.IsNullOrWhiteSpace(this.txtPrinterName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Printer Name.", AlertType.Warning);
                flag = false;
                txtPrinterName.Focus();
            }
            return flag;
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmPrinterInfo.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        public void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetCostCentreTabInfo();
            this.ddlCostCenterId.DataSource = List;
            this.ddlCostCenterId.DataTextField = "CostCenter";
            this.ddlCostCenterId.DataValueField = "CostCenterId";
            this.ddlCostCenterId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCostCenterId.Items.Insert(0, item);

            this.ddlSrcCostCenterId.DataSource = List;
            this.ddlSrcCostCenterId.DataTextField = "CostCenter";
            this.ddlSrcCostCenterId.DataValueField = "CostCenterId";
            this.ddlSrcCostCenterId.DataBind();

            ListItem srcItem = new ListItem();
            srcItem.Value = "0";
            srcItem.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlSrcCostCenterId.Items.Insert(0, srcItem);
        }
        private void LoadRestaurantKitchen()
        {
            RestaurantKitchenDA entityDA = new RestaurantKitchenDA();
            List<RestaurantKitchenBO> entityBOList = new List<RestaurantKitchenBO>();
            entityBOList = entityDA.GetRestaurantKitchenInfo();

            this.ddlKitchen.DataSource = entityBOList;
            this.ddlKitchen.DataTextField = "KitchenName";
            this.ddlKitchen.DataValueField = "KitchenId";
            this.ddlKitchen.DataBind();

            ListItem itemKitchen = new ListItem();
            itemKitchen.Value = "0";
            itemKitchen.Text = hmUtility.GetDropDownFirstValue();
            this.ddlKitchen.Items.Insert(0, itemKitchen);
        }
        private void LoadGridView()
        {
            this.CheckObjectPermission();

            string PrinterName = txtSPrinterName.Text;
            Boolean ActiveStat = ddlSActiveStat.SelectedIndex == 0 ? true : false;

            PrinterInfoDA da = new PrinterInfoDA();
            List<PrinterInfoBO> files = da.GetRestaurentItemTypeInfoBySearchCriteria(Convert.ToInt32(ddlCostCenterId.SelectedValue), PrinterName, ActiveStat);

            this.gvRestaurentTypeService.DataSource = files;
            this.gvRestaurentTypeService.DataBind();
            SetTab("SearchTab");
        }
        private void Cancel()
        {
            this.ddlCostCenterId.SelectedValue = "0";
            this.txtPrinterName.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.ddlStockType.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.txtPrinterInfoId.Value = string.Empty;
            this.ddlCostCenterId.Focus();
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
            PrinterInfoBO typeBO = new PrinterInfoBO();
            PrinterInfoDA typeDA = new PrinterInfoDA();
            typeBO = typeDA.GetPrinterInfoById(EditId);
            ddlActiveStat.SelectedValue = (typeBO.ActiveStat == true ? 0 : 1).ToString();
            txtPrinterName.Text = typeBO.PrinterName;
            ddlStockType.SelectedValue = typeBO.StockType;
            ddlCostCenterId.SelectedValue = typeBO.CostCenterId.ToString();
            ddlKitchen.SelectedValue = typeBO.KitchenId.ToString();
            txtPrinterInfoId.Value = typeBO.PrinterInfoId.ToString();
        }
    }
}