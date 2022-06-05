using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;
using System.Web.Services;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Inventory;


namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmKitchenInformation : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                //this.LoadCostCenter();
                LoadCostCenterInfoGridView();
            }
            CheckObjectPermission();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.txtBankName.Text))
                {
                    this.isNewAddButtonEnable = 2;
                    CommonHelper.AlertInfo(innboardMessage, "Please Provide Kitchen Name", AlertType.Warning);
                    this.txtBankName.Focus();
                }
                else if (DuplicateCheckDynamicaly("KitchenName", this.txtBankName.Text, 0) > 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Kitchen Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                    this.txtBankName.Focus();
                    //return;
                }
                else
                {                    

                    RestaurantKitchenBO resKitchenBO = new RestaurantKitchenBO();
                    RestaurantKitchenDA resKitchenDA = new RestaurantKitchenDA();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    //resKitchenBO.CostCenterId = Convert.ToInt32(this.ddlCostCenter.SelectedValue);
                    resKitchenBO.CostCenterId = 0;
                    resKitchenBO.KitchenName = this.txtBankName.Text;
                    resKitchenBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

                    List<RestaurantKitchenCostCenterMappingBO> costCenterList = new List<RestaurantKitchenCostCenterMappingBO>();

                    int rowsKitchenItem = gvCategoryCostCenterInfo.Rows.Count;
                    for (int i = 0; i < rowsKitchenItem; i++)
                    {
                        CheckBox cb = (CheckBox)gvCategoryCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                        if (cb.Checked == true)
                        {
                            RestaurantKitchenCostCenterMappingBO costCenter = new RestaurantKitchenCostCenterMappingBO();
                            Label lbl = (Label)gvCategoryCostCenterInfo.Rows[i].FindControl("lblCostCentreId");

                            costCenter.CostCenterId = Convert.ToInt32(lbl.Text);
                            if (!string.IsNullOrEmpty(hfKitchenId.Value))
                            {
                                costCenter.KitchenId = Int32.Parse(hfKitchenId.Value);
                            }
                            else
                            {
                                costCenter.KitchenId = 0;
                            }
                            costCenterList.Add(costCenter);
                        }
                    }

                    if (string.IsNullOrWhiteSpace(hfKitchenId.Value))
                    {
                        int tmpKitchenId = 0;
                        resKitchenBO.CreatedBy = userInformationBO.UserInfoId;
                        Boolean status = resKitchenDA.SaveRestaurantKitchenInfo(resKitchenBO, costCenterList, out tmpKitchenId);
                        if (status)
                        {
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RestaurantKitchen.ToString(), tmpKitchenId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantKitchen));
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            this.Cancel();
                        }
                    }
                    else
                    {
                        resKitchenBO.KitchenId = Convert.ToInt32(hfKitchenId.Value);
                        resKitchenBO.LastModifiedBy = userInformationBO.UserInfoId;
                        Boolean status = resKitchenDA.UpdateRestaurantKitchenInfo(resKitchenBO, costCenterList);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RestaurantKitchen.ToString(), resKitchenBO.KitchenId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantKitchen));
                            this.Cancel();
                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                        }
                    }
                    if (gvGuestHouseService.Rows.Count > 0)
                    {
                        this.LoadGridView();
                    }
                    this.SetTab("EntryTab");
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        protected void gvGuestHouseService_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvGuestHouseService.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void gvGuestHouseService_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvGuestHouseService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int KitchenId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(KitchenId);
                this.btnSave.Visible = isUpdatePermission;
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("RestaurantKitchen", "KitchenId", KitchenId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.RestaurantKitchen.ToString(), KitchenId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantKitchen));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                }
                LoadGridView();
                this.SetTab("SearchTab");
            }
        }
        //************************ User Defined Function ********************//
        private void Cancel()
        {
            //this.ddlCostCenter.SelectedValue = "0";
            this.txtBankName.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.hfKitchenId.Value = string.Empty;
            this.txtBankName.Focus();

            LoadCostCenterInfoGridView();
        }
        //private void LoadCostCenter()
        //{
        //    CostCentreTabDA entityDA = new CostCentreTabDA();
        //    this.ddlCostCenter.DataSource = entityDA.GetAllRestaurantTypeCostCentreTabInfo();
        //    this.ddlCostCenter.DataTextField = "CostCenter";
        //    this.ddlCostCenter.DataValueField = "CostCenterId";
        //    this.ddlCostCenter.DataBind();

        //    ListItem item = new ListItem();
        //    item.Value = "0";
        //    item.Text = hmUtility.GetDropDownFirstValue();
        //    this.ddlCostCenter.Items.Insert(0, item);
        //}
        private void LoadGridView()
        {
            this.CheckObjectPermission();
            string BankName = txtSBankName.Text;
            Boolean ActiveStat = ddlSActiveStat.SelectedIndex == 0 ? true : false;
            RestaurantKitchenDA da = new RestaurantKitchenDA();
            List<RestaurantKitchenBO> files = da.GetRestaurantKitchenInfoBySearchCriteria(BankName, ActiveStat);
            this.gvGuestHouseService.DataSource = files;
            this.gvGuestHouseService.DataBind();
            
            SetTab("SearchTab");
        }
        private void LoadCostCenterInfoGridView()
        {
            this.CheckObjectPermission();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> files = costCentreTabDA.GetCostCentreTabInfo();

            this.gvCategoryCostCenterInfo.DataSource = files;
            this.gvCategoryCostCenterInfo.DataBind();
        }
        private void LoadRestaurantKitchenCostCenterMappingInfo(int EditId)
        {
            List<RestaurantKitchenCostCenterMappingBO> costListStockItem = new List<RestaurantKitchenCostCenterMappingBO>();
            InvCategoryCostCenterMappingDA costStockItemDA = new InvCategoryCostCenterMappingDA();
            costListStockItem = costStockItemDA.GetRestaurantKitchenCostCenterMappingByKitchenId(EditId);
            int rowsStockItem = gvCategoryCostCenterInfo.Rows.Count;

            List<RestaurantKitchenCostCenterMappingBO> listStockItem = new List<RestaurantKitchenCostCenterMappingBO>();
            for (int i = 0; i < rowsStockItem; i++)
            {
                RestaurantKitchenCostCenterMappingBO costCenterStockItem = new RestaurantKitchenCostCenterMappingBO();
                Label lbl = (Label)gvCategoryCostCenterInfo.Rows[i].FindControl("lblCostCentreId");
                costCenterStockItem.CostCenterId = Int32.Parse(lbl.Text);
                listStockItem.Add(costCenterStockItem);
            }


            for (int i = 0; i < listStockItem.Count; i++)
            {
                for (int j = 0; j < costListStockItem.Count; j++)
                {
                    if (listStockItem[i].CostCenterId == costListStockItem[j].CostCenterId)
                    {
                        CheckBox cb = (CheckBox)gvCategoryCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                        cb.Checked = true;
                    }
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
        public void FillForm(int EditId)
        {
            RestaurantKitchenBO bankBO = new RestaurantKitchenBO();
            RestaurantKitchenDA bankDA = new RestaurantKitchenDA();
            bankBO = bankDA.GetRestaurantKitchenInfoById(EditId);
            ddlActiveStat.SelectedValue = (bankBO.ActiveStat == true ? 0 : 1).ToString();
            hfKitchenId.Value = bankBO.KitchenId.ToString();
            txtBankName.Text = bankBO.KitchenName.ToString();
            //ddlCostCenter.SelectedValue = bankBO.CostCenterId.ToString();
            LoadCostCenterInfoGridView();
            LoadRestaurantKitchenCostCenterMappingInfo(EditId);
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
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "RestaurantKitchen";
            string pkFieldName = "KitchenId";
            string pkFieldValue = this.hfKitchenId.Value;
            int IsDuplicate = 0;
            if (!string.IsNullOrWhiteSpace(pkFieldValue))
            {
                isUpdate = 1;
            }
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            ReturnInfo rtnInf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("RestaurantKitchen", "KitchenId", sEmpId);
                if (status)
                {
                    rtnInf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Data Deleted Failed.";
                throw ex;
            }

            return rtnInf;
        }
        [WebMethod]
        public static string LoadBreadCrumbsInformation()
        {
            string breadCrumbs = string.Empty;
            breadCrumbs = "<span class='divider'>/</span><a href='/HMCommon/frmKitchenInformation.aspx'>Bank</a><span class='divider'></span>";
            return breadCrumbs;
        }
    }
}