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
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class frmInvProduction : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                this.CheckObjectPermission();
                this.LoadCostCenter();
                this.LoadCategoryInfo();
                this.LoadGridInformation();
                this.LoadAccountHead();
                this.SetTab("EntryTab");
                if (Session["FinishProductId"] != null)
                {
                    hfIsEditedFromApprovedForm.Value = "1";
                    hfFinishProductId.Value = Session["FinishProductId"].ToString();

                    Session.Remove("FinishProductId");
                }
            }
        }
        protected void gvFinishedProductInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                var item = (FinishedProductBO)e.Row.DataItem;

                //ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                ImageButton imgApproval = (ImageButton)e.Row.FindControl("ImgApproval");
                ImageButton imgCheck = (ImageButton)e.Row.FindControl("ImgCheck");

                if (item.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString() && item.ApprovedStatus != HMConstants.ApprovalStatus.Cancel.ToString() && (item.IsCanChecked == true || item.IsCanApproved == true))
                {
                    //imgUpdate.Visible = true;
                    if(item.ApprovedStatus != HMConstants.ApprovalStatus.Checked.ToString())
                    {
                        imgCheck.Visible = true;
                        imgApproval.Visible = false;
                    }
                    else
                    {
                        imgApproval.Visible = true;
                        imgCheck.Visible = false;
                    }
                    imgDelete.Visible = true;
                    // imgApproval.Visible = true;
                }
                else
                {
                    //imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                    imgApproval.Visible = false;
                    imgCheck.Visible = false;
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
                    bool status = goodsDA.DeleteInventoryProduction(finishedProductId);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        LoadGridInformation();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                    this.SetTab("SearchTab");
                }
                else if (e.CommandName == "CmdReportRI")
                {
                    this.SetTab("SearchTab");
                    int finishProductId = Convert.ToInt32(e.CommandArgument.ToString());
                    string url = "/Inventory/Reports/frmReportProductionInfo.aspx?iFGId=" + finishProductId;
                    string s = "window.open('" + url + "', 'popup_window', 'width=800,height=680,left=300,top=50,resizable=yes');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridInformation();
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
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmInvProduction.ToString());
            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            //if (!isSavePermission)
            //{
            //    isKitchenItemCostCenterInformationDivEnable = -1;
            //}
        }
        private void LoadCostCenter()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            CostCentreTabDA entityDA = new CostCentreTabDA();
            var List = entityDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId);

            this.ddlCostCenter.DataSource = List.Where(x => x.CostCenterType == "Inventory").ToList();
            this.ddlCostCenter.DataTextField = "CostCenter";
            this.ddlCostCenter.DataValueField = "CostCenterId";
            this.ddlCostCenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCostCenter.Items.Insert(0, item);

            this.ddlSearchCostCenter.DataSource = List.Where(x => x.CostCenterType == "Inventory").ToList();
            this.ddlSearchCostCenter.DataTextField = "CostCenter";
            this.ddlSearchCostCenter.DataValueField = "CostCenterId";
            this.ddlSearchCostCenter.DataBind();

            ListItem itemSearch = new ListItem();
            itemSearch.Value = "0";
            itemSearch.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlSearchCostCenter.Items.Insert(0, itemSearch);
        }

        private void LoadAccountHead()
        {
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            ddlAccountHead.DataSource = nodeMatrixDA.GetNodeMatrixInfo();
            ddlAccountHead.DataTextField = "NodeHead";
            ddlAccountHead.DataValueField = "NodeId";
            ddlAccountHead.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlAccountHead.Items.Insert(0, itemNodeId);
        }

        private void LoadCategoryInfo()
        {
            InvCategoryDA categoryDA = new InvCategoryDA();
            var category = categoryDA.GetInvItemCatagoryInfoByServiceType("Product");

            this.ddlRMCategory.DataSource = category;
            this.ddlRMCategory.DataTextField = "MatrixInfo";
            this.ddlRMCategory.DataValueField = "CategoryId";
            this.ddlRMCategory.DataBind();

            this.ddlCategory.DataSource = category;
            this.ddlCategory.DataTextField = "MatrixInfo";
            this.ddlCategory.DataValueField = "CategoryId";
            this.ddlCategory.DataBind();

            ListItem item1 = new ListItem();
            item1.Value = "0";
            item1.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlRMCategory.Items.Insert(0, item1);
            this.ddlCategory.Items.Insert(0, item1);
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        private void LoadGridInformation()
        {
            this.SetTab("SearchTab");

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            PMFinishProductDA productDa = new PMFinishProductDA();
            List<FinishedProductBO> finishGoods = new List<FinishedProductBO>();

            DateTime? fromDate = null, toDate = null;
            int costCenterId = 0;
            string productionId = "";
            string status = "";

            if (ddlSearchCostCenter.SelectedValue != "0")
            {
                costCenterId = Convert.ToInt32(ddlSearchCostCenter.SelectedValue);
            }

            if (!string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                fromDate = CommonHelper.DateTimeToMMDDYYYY(txtFromDate.Text);
            }

            if (!string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                toDate = CommonHelper.DateTimeToMMDDYYYY(txtToDate.Text);
            }


            productionId = txtProductionId.Text;
            status = ddlStatus.SelectedValue;

            finishGoods = productDa.GetInventoryProductionSearch(costCenterId, fromDate, toDate, productionId, status, userInformationBO.UserInfoId);
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
        public static List<InvItemAutoSearchBO> ItemNCategoryAutoSearchForRMGoods(int costCenterId, string itemName, int categoryId, int locationId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();
            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemNameNCategoryCostcenterForAutoSearch(itemName, categoryId, null, null, costCenterId, true);
            return itemInfo;
        }
        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemNCategoryAutoSearchForFinishedGoods(int costCenterId, string itemName, int categoryId, int locationId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();
            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemNameNCategoryCostcenterForAutoSearch(itemName, categoryId, null, null, costCenterId, true);
            return itemInfo;
        }
        [WebMethod]
        public static InvItemAutoSearchBO GetItemNameForAutoSearch(string itemName, int costcenterId)
        {
            List<InvItemAutoSearchBO> itemInfoListBO = new List<InvItemAutoSearchBO>();
            int categoryId = 0;

            InvItemDA itemDA = new InvItemDA();
            InvItemAutoSearchBO itemInfo = new InvItemAutoSearchBO();
            itemInfoListBO = itemDA.GetItemNameForAutoSearch(itemName, categoryId, costcenterId);
            if (itemInfoListBO != null)
            {
                if (itemInfoListBO.Count > 0)
                {
                    itemInfo = itemInfoListBO[0];
                }
            }
            return itemInfo;
        }
        [WebMethod]
        public static FinishedProductViewBO FillForm(int productionId)
        {
            PMFinishProductDA goodsDA = new PMFinishProductDA();
            FinishedProductViewBO viewBo = new FinishedProductViewBO();

            viewBo.FinishedProduct = goodsDA.GetFinishedProductById(productionId);
            viewBo.FinisProductRMDetails = goodsDA.GetInvProductionRMDetailsById(productionId);
            viewBo.FinisProductDetails = goodsDA.GetInvProductionFGDetailsById(productionId);
            viewBo.FinishProductOEDetails = goodsDA.GetInvProductionOEDetailsById(productionId);

            return viewBo;
        }
        [WebMethod]
        public static List<FinishedProductDetailsBO> GetFinishProductDetails(int productionId)
        {
            PMFinishProductDA productDa = new PMFinishProductDA();
            return productDa.GetInvProductionFGDetailsById(productionId);
        }
        [WebMethod]
        public static List<InvLocationBO> InvLocationByCostCenter(int costCenterId)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvItemLocationByCostCenter(costCenterId);

            return location;
        }
        [WebMethod]
        public static ReturnInfo SaveFinishGoods(FinishedProductBO finishedProduct, List<FinishedProductDetailsBO> AddedRMGoods, List<FinishedProductDetailsBO> EditedRMGoods, List<FinishedProductDetailsBO> DeletedRMGoods, List<FinishedProductDetailsBO> AddedFinishGoods, List<FinishedProductDetailsBO> EditedFinishGoods, List<FinishedProductDetailsBO> DeletedFinishGoods, List<OverheadExpensesBO> AddedOverheadExpenses)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                PMFinishProductDA finishDa = new PMFinishProductDA();

                finishedProduct.OrderDate = DateTime.Now;

                if (finishedProduct.FinishProductId == 0)
                {
                    finishedProduct.ApprovedStatus = HMConstants.ApprovalStatus.Pending.ToString();
                    finishedProduct.CreatedBy = userInformationBO.UserInfoId;

                    status = finishDa.SaveInventoryProduction(finishedProduct, AddedRMGoods, AddedFinishGoods, AddedOverheadExpenses);

                    if (status)
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }
                else
                {
                    finishedProduct.LastModifiedBy = userInformationBO.UserInfoId;
                    status = finishDa.UpdateInventoryProduction(finishedProduct, AddedFinishGoods, EditedFinishGoods, DeletedFinishGoods, AddedRMGoods, EditedRMGoods, DeletedRMGoods, AddedOverheadExpenses);

                    if (status)
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
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
        [WebMethod]
        public static ReturnInfo FinishProductCheck(long productionId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMFinishProductDA orderDetailDA = new PMFinishProductDA();
            string approvedStatus = "Checked";
            string TransactionNo = "";
            string TransactionType = "";
            string ApproveStatus = "";

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDetailDA.InventoryProductionApproval(productionId, approvedStatus);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    rtninf.PrimaryKeyValue = productionId.ToString();
                    rtninf.TransactionNo = TransactionNo;
                    rtninf.TransactionType = TransactionType;
                    rtninf.TransactionStatus = ApproveStatus;

                    if (approvedStatus == HMConstants.ApprovalStatus.Checked.ToString())
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                    }
                    else
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    }

                    //Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ProductReceive.ToString(), receivedId,
                    //           ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductReceive));


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
        public static ReturnInfo FinishProductApproval(long productionId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMFinishProductDA orderDetailDA = new PMFinishProductDA();
            string approvedStatus = "Approved";
            string TransactionNo = "";
            string TransactionType = "";
            string ApproveStatus = "";

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDetailDA.InventoryProductionApproval(productionId, approvedStatus);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    rtninf.PrimaryKeyValue = productionId.ToString();
                    rtninf.TransactionNo = TransactionNo;
                    rtninf.TransactionType = TransactionType;
                    rtninf.TransactionStatus = ApproveStatus;

                    if (approvedStatus == HMConstants.ApprovalStatus.Checked.ToString())
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                    }
                    else
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    }

                    //Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ProductReceive.ToString(), receivedId,
                    //           ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductReceive));


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
        public static InvItemStockInformationBO GetInvItemStockInfoByItemAndAttributeIdForPurchase(int itemId, int colorId, int sizeId, int styleId, int locationId, int companyId)
        {
            InvItemDA DA = new InvItemDA();
            InvItemStockInformationBO StockInformation = new InvItemStockInformationBO();
            StockInformation = DA.GetInvItemStockInfoByItemAndAttributeIdForPurchase(itemId, colorId, sizeId, styleId, locationId, companyId);

            return StockInformation;
        }

        protected void gvFinishedProductInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFinishedProductInfo.PageIndex = e.NewPageIndex;
            this.LoadGridInformation();
        }
    }
}