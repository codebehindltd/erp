using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Data.Inventory;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class frmInvFinishedProduct : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                this.CheckObjectPermission();
                this.LoadCostCenter();
                this.LoadCategoryInfo();
                this.LoadStockBy();

                if (Session["FinishProductId"] != null)
                {
                    hfIsEditedFromApprovedForm.Value = "1";
                    hfFinishProductId.Value = Session["FinishProductId"].ToString();

                    Session.Remove("FinishProductId");
                }
                CheckObjectPermission();
            }            
        }
        protected void gvFinishedProductInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                var item = (FinishedProductBO)e.Row.DataItem;

                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");

                if (item.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString() && item.ApprovedStatus != HMConstants.ApprovalStatus.Cancel.ToString())
                {
                    imgUpdate.Visible = isUpdatePermission;
                    imgDelete.Visible = isDeletePermission;
                }
                else
                {
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                }
            }
        }
        protected void gvFinishedProductInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "CmdDelete")
                {
                    int finishedProductId = Convert.ToInt32(e.CommandArgument.ToString());
                    string result = string.Empty;

                    PMFinishProductDA goodsDA = new PMFinishProductDA();
                    bool status = goodsDA.DeleteFinishGoodsOrder(finishedProductId);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.FinishedProduct.ToString(), finishedProductId,
                            ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.FinishedProduct));
                        LoadGrid();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                    this.SetTab("SearchTab");
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrWhiteSpace(this.txtBankName.Text))
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Bank Name.", AlertType.Warning);
            //    this.txtBankName.Focus();
            //}
            //else
            //{
            //    BankBO bankBO = new BankBO();
            //    BankDA bankDA = new BankDA();

            //    UserInformationBO userInformationBO = new UserInformationBO();
            //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            //    bankBO.BankName = this.txtBankName.Text;
            //    bankBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

            //    if (string.IsNullOrWhiteSpace(txtBankId.Value))
            //    {
            //        int tmpBankId = 0;
            //        bankBO.CreatedBy = userInformationBO.UserInfoId;
            //        Boolean status = bankDA.SaveBankInfo(bankBO, out tmpBankId);
            //        if (status)
            //        {
            //            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Save.ToString(), EntityTypeEnum.EntityType.Bank.ToString(), tmpBankId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Bank));
            //            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
            //            this.Cancel();
            //        }
            //    }
            //    else
            //    {
            //        bankBO.BankId = Convert.ToInt32(txtBankId.Value);
            //        bankBO.LastModifiedBy = userInformationBO.UserInfoId;
            //        Boolean status = bankDA.UpdateBankInfo(bankBO);
            //        if (status)
            //        {
            //            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
            //            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.Bank.ToString(), bankBO.BankId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Bank));
            //            this.Cancel();
            //        }
            //    }

            //    this.SetTab("EntryTab");
            //}
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {       
            btnSave.Visible = isSavePermission;
            //if (!isSavePermission)
            //{
            //    isKitchenItemCostCenterInformationDivEnable = -1;
            //}
            btnSave.Visible = isSavePermission;

            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }
        private void LoadCostCenter()
        {
            CostCentreTabDA entityDA = new CostCentreTabDA();
            var List = entityDA.GetCostCentreTabInfo();

            this.ddlCostCenter.DataSource = List;
            this.ddlCostCenter.DataTextField = "CostCenter";
            this.ddlCostCenter.DataValueField = "CostCenterId";
            this.ddlCostCenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCostCenter.Items.Insert(0, item);

            this.ddlSearchCostCenter.DataSource = List;
            this.ddlSearchCostCenter.DataTextField = "CostCenter";
            this.ddlSearchCostCenter.DataValueField = "CostCenterId";
            this.ddlSearchCostCenter.DataBind();
            this.ddlSearchCostCenter.Items.Insert(0, item);
        }
        private void LoadCategoryInfo()
        {
            InvCategoryDA categoryDA = new InvCategoryDA();
            var category = categoryDA.GetInvItemCatagoryInfoByServiceType("Product");

            this.ddlCategory.DataSource = category;
            this.ddlCategory.DataTextField = "MatrixInfo";
            this.ddlCategory.DataValueField = "CategoryId";
            this.ddlCategory.DataBind();
            ListItem item1 = new ListItem();
            item1.Value = "0";
            item1.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCategory.Items.Insert(0, item1);
        }
        private void LoadStockBy()
        {
            List<InvUnitHeadBO> headListBO = new List<InvUnitHeadBO>();
            InvUnitHeadDA da = new InvUnitHeadDA();
            headListBO = da.GetInvUnitHeadInfo();

            this.ddlItemStockBy.DataSource = headListBO;
            this.ddlItemStockBy.DataTextField = "HeadName";
            this.ddlItemStockBy.DataValueField = "UnitHeadId";
            this.ddlItemStockBy.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlItemStockBy.Items.Insert(0, item);

        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        private void LoadGrid()
        {
            this.SetTab("SearchTab");

            PMFinishProductDA productDa = new PMFinishProductDA();
            List<FinishedProductBO> finishGoods = new List<FinishedProductBO>();

            DateTime? fromDate = null, toDate = null;
            int costCenterId = 0;

            if (ddlSearchCostCenter.SelectedValue != "0")
            {
                costCenterId = Convert.ToInt32(ddlSearchCostCenter.SelectedValue);
            }

            if (!string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                //fromDate = Convert.ToDateTime(txtFromDate.Text);
                fromDate = CommonHelper.DateTimeToMMDDYYYY(txtFromDate.Text);
            }

            if (!string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                //toDate = Convert.ToDateTime(txtToDate.Text);
                toDate = CommonHelper.DateTimeToMMDDYYYY(txtToDate.Text);
            }

            finishGoods = productDa.GetFinishedProductSearch(costCenterId, fromDate, toDate);

            gvFinishedProductInfo.DataSource = finishGoods;
            gvFinishedProductInfo.DataBind();
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
        public static List<InvItemAutoSearchBO> ItemNCategoryAutoSearch(string itemName, int categoryId, int costCenterId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemNameNCategoryCostcenterForAutoSearch(itemName, categoryId, true, null, costCenterId, true);

            return itemInfo;
        }
        [WebMethod]
        public static FinishedProductViewBO FillForm(int finishProductId)
        {
            PMFinishProductDA goodsDA = new PMFinishProductDA();
            FinishedProductViewBO viewBo = new FinishedProductViewBO();

            viewBo.FinishedProduct = goodsDA.GetFinishedProductById(finishProductId);
            viewBo.FinisProductDetails = goodsDA.GetFinishedProductDetailsById(finishProductId);

            return viewBo;
        }
        [WebMethod]
        public static List<FinishedProductDetailsBO> GetFinishProductDetails(int finishProductId)
        {
            PMFinishProductDA productDa = new PMFinishProductDA();
            return productDa.GetFinishedProductDetailsById(finishProductId);
        }
        [WebMethod]
        public static ReturnInfo SaveFinishGoods(FinishedProductBO finishedProduct, List<FinishedProductDetailsBO> AddedFinishGoods, List<FinishedProductDetailsBO> EditedFinishGoods, List<FinishedProductDetailsBO> DeletedFinishGoods)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                PMFinishProductDA finishDa = new PMFinishProductDA();
                int finishProductId;
                finishedProduct.OrderDate = DateTime.Now;

                if (finishedProduct.FinishProductId == 0)
                {
                    finishedProduct.ApprovedStatus = HMConstants.ApprovalStatus.Pending.ToString();
                    finishedProduct.CreatedBy = userInformationBO.UserInfoId;

                    status = finishDa.SaveFinishGoods(finishedProduct, AddedFinishGoods, out finishProductId);

                    if (status)
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.FinishedProduct.ToString(), finishProductId,
                            ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.FinishedProduct));
                    }
                }
                else
                {
                    finishedProduct.LastModifiedBy = userInformationBO.UserInfoId;
                    status = finishDa.UpdateFinishGoods(finishedProduct, AddedFinishGoods, EditedFinishGoods, DeletedFinishGoods);

                    if (status)
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.FinishedProduct.ToString(), finishedProduct.FinishProductId,
                            ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.FinishedProduct));
                    }
                }

                if (!status)
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }
    }
}