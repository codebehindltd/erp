using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmInvCategory : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        InvCategoryDA moLocation = new InvCategoryDA();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomCategoryId.Value = seatingId.ToString();
                tempCategoryId.Value = seatingId.ToString();
                hfCategoryId.Value = "0";
                hfParentCategory.Value = "0";
                FileUpload();

                this.txtFocusTabControl.Value = "1";
                this.LoadCategoryHead();
                this.LoadAccountHeadForSearch();
                this.LoadCostCenterInfoGridView();
                this.ddlNodeId.Focus();
                tvLocations.Attributes.Add("onclick", "return OnTreeClick(event)");
                this.GetTopLevelLocations(null);
                this.LoadInventoryAccountsHead();
                this.LoadCogsAccountsHead();
                this.LoadddlFixedAssetAccountsHead();
                this.LoadDepreciationAccountsHead();
                this.IsInventoryIntegrateWithAccounts();
                IsInvCategoryCodeAutoGenerate();
            }
            this.CheckObjectPermission();
        }
        protected void gvChartOfAccout_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = false;
            }
        }
        protected void gvChartOfAccout_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _selectedNodeId = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "CmdEdit")
            {
                this.txtFocusTabControl.Value = "1";
                InvCategoryDA matrixDA = new InvCategoryDA();
                InvCategoryBO matrixBO = new InvCategoryBO();

                matrixBO = matrixDA.GetInvCategoryInfoById(_selectedNodeId);

                if (matrixBO.CategoryId != matrixBO.AncestorId)
                {
                    this.ddlNodeId.SelectedValue = matrixBO.AncestorId.ToString();
                }
                else this.ddlNodeId.SelectedValue = "0";
                this.txtNodeNumber.Text = matrixBO.Code;
                this.txtNodeHead.Text = matrixBO.Name;
                this.ddlServiceType.SelectedValue = matrixBO.ServiceType;
                this.txtDescription.Text = matrixBO.Description;
                this.txtEditNodeId.Value = matrixBO.CategoryId.ToString();
                ddlActiveStat.SelectedIndex = matrixBO.ActiveStat == true ? 0 : 1;
                this.txtEditNodeId.Value = _selectedNodeId.ToString();
                this.txtAncestorNodeId.Value = matrixBO.AncestorId.ToString();
                RandomCategoryId.Value = matrixBO.CategoryId.ToString();
                hfCategoryId.Value = matrixBO.CategoryId.ToString();
                FileUpload();
                btnSave.Visible = isUpdatePermission;
                this.btnSave.Text = "Update";
                this.gvChartOfAccout.DataSource = null;
                this.gvChartOfAccout.DataBind();

                LoadInvCategoryCostCenterMappingInfo(_selectedNodeId);

                if (hfIsInventoryIntegrateWithAccounts.Value == "1")
                {
                    // // Inventory Accounts Mapping.........................................
                    CogsAccountVsItemCategoryMapppingBO catagoryInventoryMapping = new CogsAccountVsItemCategoryMapppingBO();
                    catagoryInventoryMapping = matrixDA.GetInventoryAccountVsItemCategoryMappping(_selectedNodeId);

                    if (catagoryInventoryMapping != null)
                    {
                        if (ddlServiceType.SelectedValue == "FixedAsset")
                        {
                            ddlFixedAssetAccounts.SelectedValue = catagoryInventoryMapping.NodeId.ToString();
                        }
                        else
                        {
                            ddlInventoryAccounts.SelectedValue = catagoryInventoryMapping.NodeId.ToString();
                        }
                    }

                    // // COGS Accounts Mapping.........................................
                    CogsAccountVsItemCategoryMapppingBO catagoryCOGSMapping = new CogsAccountVsItemCategoryMapppingBO();
                    catagoryCOGSMapping = matrixDA.GetCogsAccountVsItemCategoryMappping(_selectedNodeId);

                    if (catagoryCOGSMapping != null)
                    {
                        if (ddlServiceType.SelectedValue == "FixedAsset")
                        {
                            ddlDepreciationAccounts.SelectedValue = catagoryCOGSMapping.NodeId.ToString();
                        }
                        else
                        {
                            ddlCogsAccounts.SelectedValue = catagoryCOGSMapping.NodeId.ToString();
                        }

                    }
                }
                SetTab("Entry");
            }
            else if (e.CommandName == "CmdDelete")
            {

            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            string searchText = string.Empty;
            searchText = txtAccHead.Text.Trim();
            this.LoadGridView(searchText);

            SetTab("Search");
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.Cancel();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }
            if (btnSave.Text == "Save")
            {
                if (DuplicateCheckDynamicaly("Name", this.txtNodeHead.Text, 0) > 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Category Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                    this.txtNodeHead.Focus();
                    return;
                }

                if (DuplicateCheckDynamicaly("Code", this.txtNodeNumber.Text, 0) > 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Code Number" + AlertMessage.DuplicateValidation, AlertType.Warning);
                    this.txtNodeNumber.Focus();
                    return;
                }
            }

            int OwnerIdForDocuments = 0;
            lblMessage.Text = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            UserInformationDA userInformationDA = new UserInformationDA();
            InvCategoryCostCenterMappingDA costCenterMappingDA = new InvCategoryCostCenterMappingDA();

            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            InvCategoryBO nodeMatrixBO = new InvCategoryBO();
            InvCategoryDA nodeMatrixDA = new InvCategoryDA();

            if (this.ddlNodeId.SelectedValue == "0" || hfParentCategory.Value == "1")
            {
                nodeMatrixBO.AncestorId = -1;
            }
            else
            {
                nodeMatrixBO.AncestorId = Convert.ToInt32(this.ddlNodeId.SelectedValue);
            }

            nodeMatrixBO.Code = this.txtNodeNumber.Text.Trim();
            nodeMatrixBO.Name = this.txtNodeHead.Text.Trim();
            nodeMatrixBO.ServiceType = this.ddlServiceType.SelectedValue;
            nodeMatrixBO.Description = this.txtDescription.Text;
            nodeMatrixBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
            nodeMatrixBO.RandomCategoryId = Int32.Parse(RandomCategoryId.Value);

            if (ddlServiceType.SelectedValue == "FixedAsset")
            {
                if (ddlFixedAssetAccounts.SelectedValue != "0")
                    nodeMatrixBO.InventoryNodeId = Convert.ToInt32(ddlFixedAssetAccounts.SelectedValue);

                if (ddlDepreciationAccounts.SelectedValue != "0")
                    nodeMatrixBO.CogsNodeId = Convert.ToInt32(ddlDepreciationAccounts.SelectedValue);
            }
            else
            {
                if (ddlInventoryAccounts.SelectedValue != "0")
                    nodeMatrixBO.InventoryNodeId = Convert.ToInt32(ddlInventoryAccounts.SelectedValue);

                if (ddlCogsAccounts.SelectedValue != "0")
                    nodeMatrixBO.CogsNodeId = Convert.ToInt32(ddlCogsAccounts.SelectedValue);
            }

            int NodeId = 0;

            List<InvCategoryCostCenterMappingBO> costCenterList = new List<InvCategoryCostCenterMappingBO>();
            List<InvCategoryCostCenterMappingBO> costCenterListUnselect = new List<InvCategoryCostCenterMappingBO>();

            int rowsKitchenItem = gvCategoryCostCenterInfo.Rows.Count;
            for (int i = 0; i < rowsKitchenItem; i++)
            {
                InvCategoryCostCenterMappingBO costCenter = new InvCategoryCostCenterMappingBO();
                CheckBox cb = (CheckBox)gvCategoryCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                if (cb.Checked == true)
                {
                    Label lbl = (Label)gvCategoryCostCenterInfo.Rows[i].FindControl("lblCostCentreId");
                    costCenter.CostCenterId = Convert.ToInt32(lbl.Text);
                    if (!string.IsNullOrEmpty(txtCategoryId.Value))
                    {
                        costCenter.CategoryId = Int32.Parse(txtCategoryId.Value);
                    }
                    else
                    {
                        costCenter.CategoryId = 0;
                    }
                    costCenterList.Add(costCenter);
                }
                else
                {
                    Label lbl = (Label)gvCategoryCostCenterInfo.Rows[i].FindControl("lblCostCentreId");
                    costCenter.CostCenterId = Convert.ToInt32(lbl.Text);
                    if (hfCategoryId.Value != "0")
                    {
                        costCenter.CategoryId = Int32.Parse(hfCategoryId.Value);
                    }
                    else
                    {
                        costCenter.CategoryId = 0;
                    }
                    costCenterListUnselect.Add(costCenter);
                }
            }

            // save op
            if (string.IsNullOrWhiteSpace(txtEditNodeId.Value))
            {
                nodeMatrixBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = nodeMatrixDA.SaveInvCatagoryInfo(nodeMatrixBO, costCenterList, out NodeId);
                if (status)
                {
                    OwnerIdForDocuments = NodeId;
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.ItemCategory.ToString(), NodeId,
                    ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ItemCategory));
                    this.LoadCategoryHead();
                    GetTopLevelLocations(null);
                    this.Cancel();
                }
            }
            else //update op
            {
                nodeMatrixBO.CategoryId = Convert.ToInt32(txtEditNodeId.Value);

                if (nodeMatrixBO.CategoryId == nodeMatrixBO.AncestorId)
                {
                    nodeMatrixBO.AncestorId = -1;
                }

                CogsAccountVsItemCategoryMapppingBO catagoryInventoryMapping = new CogsAccountVsItemCategoryMapppingBO();
                catagoryInventoryMapping = nodeMatrixDA.GetInventoryAccountVsItemCategoryMappping(nodeMatrixBO.CategoryId);

                if (catagoryInventoryMapping != null)
                {
                    if (ddlInventoryAccounts.SelectedValue != "0")
                    {
                        nodeMatrixBO.InventoryNodeId = Convert.ToInt32(ddlInventoryAccounts.SelectedValue);
                        catagoryInventoryMapping.NodeId = Convert.ToInt32(ddlInventoryAccounts.SelectedValue);
                    }
                    else
                        nodeMatrixBO.InventoryNodeId = catagoryInventoryMapping.NodeId;
                }

                CogsAccountVsItemCategoryMapppingBO catagoryCogsMapping = new CogsAccountVsItemCategoryMapppingBO();
                catagoryCogsMapping = nodeMatrixDA.GetCogsAccountVsItemCategoryMappping(nodeMatrixBO.CategoryId);

                if (catagoryCogsMapping != null)
                {
                    if (ddlCogsAccounts.SelectedValue != "0")
                    {
                        nodeMatrixBO.CogsNodeId = Convert.ToInt32(ddlCogsAccounts.SelectedValue);
                        catagoryCogsMapping.NodeId = Convert.ToInt32(ddlCogsAccounts.SelectedValue);
                    }
                    else
                        nodeMatrixBO.CogsNodeId = catagoryCogsMapping.NodeId;
                }

                List<InvCategoryCostCenterMappingBO> alreadySaved = new List<InvCategoryCostCenterMappingBO>();
                //List<InvCategoryCostCenterMappingBO> costCenterDeleteList = new List<InvCategoryCostCenterMappingBO>();

                alreadySaved = costCenterMappingDA.GetInvCategoryCostCenterMappingByCategoryId(nodeMatrixBO.CategoryId);

                var costCenterDeleteList = (from data in alreadySaved
                                            where (
                                            from item in costCenterListUnselect
                                            select item.CostCenterId).Contains(data.CostCenterId)
                                            select data).ToList();


                nodeMatrixBO.LastModifiedBy = userInformationBO.UserInfoId;
                foreach (var item in costCenterList)
                {
                    if (item.CategoryId == 0)
                    {
                        item.CategoryId = nodeMatrixBO.CategoryId;
                    }
                }
                Boolean status = nodeMatrixDA.UpdateInvCatagoryInfo(nodeMatrixBO, costCenterList, costCenterDeleteList, catagoryInventoryMapping, catagoryCogsMapping, out NodeId);

                if (status)
                {
                    OwnerIdForDocuments = nodeMatrixBO.CategoryId;
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.ItemCategory.ToString(), nodeMatrixBO.CategoryId,
                    ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ItemCategory));
                    this.LoadCategoryHead();
                    GetTopLevelLocations(null);
                    this.Cancel();
                }
            }

            // Update Uploaded Documents Information
            HMCommonDA hmCommonDA = new HMCommonDA();
            string docPath = Server.MapPath("") + "\\Images\\Category\\";
            Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformation("InventoryProductCategory", docPath, OwnerIdForDocuments);
        }
        //************************ User Defined Function ********************//
        private void IsInventoryIntegrateWithAccounts()
        {
            hfIsInventoryIntegrateWithAccounts.Value = "0";
            pnlIsInventoryIntegrateWithAccounts.Visible = false;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isInventoryIntegrateWithAccountsBO = new HMCommonSetupBO();
            isInventoryIntegrateWithAccountsBO = commonSetupDA.GetCommonConfigurationInfo("IsInventoryIntegrateWithAccounts", "IsInventoryIntegrateWithAccounts");
            if (isInventoryIntegrateWithAccountsBO != null)
            {
                if (isInventoryIntegrateWithAccountsBO.SetupValue == "1")
                {
                    hfIsInventoryIntegrateWithAccounts.Value = "1";
                    pnlIsInventoryIntegrateWithAccounts.Visible = true;
                }
            }
        }
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            flashUpload.QueryParameters = "InventoryProductCategoryId=" + Server.UrlEncode(RandomCategoryId.Value);
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "InvCategory";
            string pkFieldName = "CategoryId";
            string pkFieldValue = this.txtEditNodeId.Value;
            int IsDuplicate = 0;
            if (!string.IsNullOrWhiteSpace(pkFieldValue))
            {
                isUpdate = 1;
            }
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (this.btnSave.Text != "Save")
            {
                if (string.IsNullOrWhiteSpace(this.txtAncestorNodeId.Value))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Category.", AlertType.Warning);
                    this.txtAncestorNodeId.Focus();
                    flag = false;
                }
                else if (string.IsNullOrWhiteSpace(this.txtNodeHead.Text))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Category.", AlertType.Warning);
                    this.txtNodeHead.Focus();
                    flag = false;
                }
                else if (string.IsNullOrEmpty(txtNodeNumber.Text))
                {
                    if (hfIsInvCategoryCodeAutoGenerate.Value == "0")
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Category Code.", AlertType.Warning);
                        this.txtNodeNumber.Focus();
                        flag = false;
                    }
                }
                else
                {
                    if (ddlNodeId.SelectedValue == "0")
                    {
                        this.ddlNodeId.SelectedValue = this.txtAncestorNodeId.Value;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(this.txtNodeHead.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Account Head.", AlertType.Warning);
                this.txtNodeHead.Focus();
                flag = false;
            }

            if (ddlServiceType.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Category Type.", AlertType.Warning);
                this.ddlServiceType.Focus();
                flag = false;
            }

            if (ddlServiceType.SelectedValue == "Product")
            {
                if (ddlInventoryAccounts.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Inventory Head.", AlertType.Warning);
                    this.ddlInventoryAccounts.Focus();
                    flag = false;
                    return flag;
                }

                if (ddlCogsAccounts.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Expense Head.", AlertType.Warning);
                    this.ddlCogsAccounts.Focus();
                    flag = false;
                    return flag;
                }
            }
            else if (ddlServiceType.SelectedValue == "Service")
            {
                if (ddlInventoryAccounts.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Inventory Head.", AlertType.Warning);
                    this.ddlInventoryAccounts.Focus();
                    flag = false;
                    return flag;
                }

                if (ddlCogsAccounts.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Expense Head.", AlertType.Warning);
                    this.ddlCogsAccounts.Focus();
                    flag = false;
                    return flag;
                }
            }
            else if (ddlServiceType.SelectedValue == "FixedAsset")
            {
                if (ddlFixedAssetAccounts.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Fixed Asset Head.", AlertType.Warning);
                    this.ddlFixedAssetAccounts.Focus();
                    flag = false;
                    return flag;
                }

                if (ddlDepreciationAccounts.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Depreciation Head.", AlertType.Warning);
                    this.ddlDepreciationAccounts.Focus();
                    flag = false;
                    return flag;
                }
            }

            return flag;
        }
        private void LoadInventoryAccountsHead()
        {
            NodeMatrixDA entityDA = new NodeMatrixDA();
            CustomFieldBO ChequeReceiveAccountsInfo = new CustomFieldBO();
            this.ddlInventoryAccounts.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE CHARINDEX('.1.5.19.', Hierarchy) = 1 AND IsTransactionalHead = 1");

            this.ddlInventoryAccounts.DataTextField = "NodeHead";
            this.ddlInventoryAccounts.DataValueField = "NodeId";
            this.ddlInventoryAccounts.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlInventoryAccounts.Items.Insert(0, itemNodeId);
        }
        private void LoadCogsAccountsHead()
        {
            NodeMatrixDA entityDA = new NodeMatrixDA();
            CustomFieldBO ChequeReceiveAccountsInfo = new CustomFieldBO();
            //this.ddlCogsAccounts.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE CHARINDEX('.4.13.', Hierarchy) = 1 AND IsTransactionalHead = 1");
            this.ddlCogsAccounts.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE CHARINDEX('.4.', Hierarchy) = 1 AND IsTransactionalHead = 1");

            this.ddlCogsAccounts.DataTextField = "NodeHead";
            this.ddlCogsAccounts.DataValueField = "NodeId";
            this.ddlCogsAccounts.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCogsAccounts.Items.Insert(0, itemNodeId);
        }
        private void LoadddlFixedAssetAccountsHead()
        {
            NodeMatrixDA entityDA = new NodeMatrixDA();
            CustomFieldBO ChequeReceiveAccountsInfo = new CustomFieldBO();
            this.ddlFixedAssetAccounts.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE CHARINDEX('.1.6.', Hierarchy) = 1 AND IsTransactionalHead = 1");

            this.ddlFixedAssetAccounts.DataTextField = "NodeHead";
            this.ddlFixedAssetAccounts.DataValueField = "NodeId";
            this.ddlFixedAssetAccounts.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlFixedAssetAccounts.Items.Insert(0, itemNodeId);
        }
        private void LoadDepreciationAccountsHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();

            // // ---------- Operating Expense
            List<NodeMatrixBO> entityBOAditionalList = new List<NodeMatrixBO>();
            entityBOAditionalList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("14").Where(x => x.IsTransactionalHead == true).ToList();
            entityBOList.AddRange(entityBOAditionalList);

            // // ---------- Non Operating Expense
            List<NodeMatrixBO> entityExpenditureBOList = new List<NodeMatrixBO>();
            entityExpenditureBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("15").Where(x => x.IsTransactionalHead == true).ToList();
            if (entityExpenditureBOList != null)
            {
                entityBOList.AddRange(entityExpenditureBOList);
            }

            ddlDepreciationAccounts.DataSource = entityBOList;
            ddlDepreciationAccounts.DataTextField = "HeadWithCode";
            ddlDepreciationAccounts.DataValueField = "NodeId";
            ddlDepreciationAccounts.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            ddlDepreciationAccounts.Items.Insert(0, itemBank);
        }
        private void LoadCategoryHead()
        {
            InvCategoryDA productCategoryDA = new InvCategoryDA();
            this.ddlNodeId.DataSource = productCategoryDA.GetInvCatagoryInfo();
            this.ddlNodeId.DataTextField = "Name";
            this.ddlNodeId.DataValueField = "CategoryId";
            this.ddlNodeId.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlNodeId.Items.Insert(0, itemNodeId);
        }
        private void LoadAccountHeadForSearch()
        {
            InvCategoryDA nodeMatrixDA = new InvCategoryDA();
            this.ddlNodeIdForEdit.DataSource = nodeMatrixDA.GetInvCatagoryInfo();
            this.ddlNodeIdForEdit.DataTextField = "Name";
            this.ddlNodeIdForEdit.DataValueField = "CategoryId";
            this.ddlNodeIdForEdit.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlNodeIdForEdit.Items.Insert(0, itemNodeId);
        }
        private void LoadCostCenterInfoGridView()
        {
            this.CheckObjectPermission();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> files = costCentreTabDA.GetCostCentreTabInfo();

            this.gvCategoryCostCenterInfo.DataSource = files;
            this.gvCategoryCostCenterInfo.DataBind();
        }
        private void LoadGridView(string searchText)
        {
            txtNodeHeadText.Value = "";
            InvCategoryDA matrixDA = new InvCategoryDA();
            InvCategoryBO matrixBO = new InvCategoryBO();
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            List = matrixDA.GetInvCategoryByAutoSearch(searchText);
            //txtCategoryId.Value = matrixBO.CategoryId.ToString();
            //txtNodeHeadText.Value = matrixBO.Code + "(" + matrixBO.Name + ")";
            //List.Add(matrixBO);
            this.gvChartOfAccout.DataSource = List;
            this.gvChartOfAccout.DataBind();
        }
        private void LoadInvCategoryCostCenterMappingInfo(int EditId)
        {
            List<InvCategoryCostCenterMappingBO> costListStockItem = new List<InvCategoryCostCenterMappingBO>();
            InvCategoryCostCenterMappingDA costStockItemDA = new InvCategoryCostCenterMappingDA();
            costListStockItem = costStockItemDA.GetInvCategoryCostCenterMappingByCategoryId(EditId);
            int rowsStockItem = gvCategoryCostCenterInfo.Rows.Count;

            List<InvCategoryCostCenterMappingBO> listStockItem = new List<InvCategoryCostCenterMappingBO>();
            for (int i = 0; i < rowsStockItem; i++)
            {
                InvCategoryCostCenterMappingBO costCenterStockItem = new InvCategoryCostCenterMappingBO();
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
        private void Cancel()
        {
            this.txtEditNodeId.Value = string.Empty;
            this.txtAncestorNodeId.Value = string.Empty;
            this.btnSave.Text = "Save";
            this.ddlNodeId.SelectedValue = "0";
            this.txtNodeHead.Text = string.Empty;
            this.txtNodeHead.Text = string.Empty;
            this.txtNodeNumber.Text = string.Empty;
            this.ddlServiceType.SelectedValue = "0";
            this.txtDescription.Text = string.Empty;
            this.ddlInventoryAccounts.SelectedValue = "0";
            this.ddlCogsAccounts.SelectedValue = "0";
            this.ddlFixedAssetAccounts.SelectedValue = "0";
            this.ddlDepreciationAccounts.SelectedValue = "0";
            this.ddlServiceType.SelectedValue = "None";
            //ContentPlaceHolder1_gvCategoryCostCenterInfo_ChkCreate

            int rowsStockItem = gvCategoryCostCenterInfo.Rows.Count;

            if (rowsStockItem > 0)
            {
                CheckBox cb2 = ((CheckBox)this.gvCategoryCostCenterInfo.HeaderRow.FindControl("ChkCreate"));
                //CheckBox cb2 = (CheckBox)gvCategoryCostCenterInfo.FindControl("ChkCreate");
                cb2.Checked = false;
            }

            for (int i = 0; i < rowsStockItem; i++)
            {
                CheckBox cb = (CheckBox)gvCategoryCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                cb.Checked = false;
            }



            Random rd = new Random();
            int seatingId = rd.Next(100000, 999999);
            RandomCategoryId.Value = seatingId.ToString();
            tempCategoryId.Value = seatingId.ToString();
            hfCategoryId.Value = "0";
            hfParentCategory.Value = "0";
            FileUpload();
            SetTab("Entry");
        }
        private void SetTab(string TabName)
        {
            if (TabName == "Search")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "Entry")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
                C.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        private void GetTopLevelLocations(bool? expand)
        {
            try
            {
                TreeNode oNode = null;
                List<InvCategoryBO> dtObjects;
                string selectedVal = (tvLocations.SelectedNode != null) ? tvLocations.SelectedNode.Value : string.Empty;
                TreeNode selectedNode = null;
                tvLocations.Nodes.Clear();
                dtObjects = moLocation.GetInvCategoryInfoByCustomString("WHERE  lvl = 0");

                foreach (InvCategoryBO item in dtObjects)
                {
                    oNode = new TreeNode(HttpUtility.HtmlEncode(item.Name), item.CategoryId.ToString());
                    oNode.Expanded = false;
                    if (selectedVal == oNode.Value)
                        selectedNode = oNode;

                    GetChildLocations(ref oNode, expand, ref selectedNode, selectedVal);
                    tvLocations.Nodes.Add(oNode);
                }

                if (selectedNode != null && expand != false)
                {
                    while (selectedNode.Parent != null)
                    {
                        selectedNode.Parent.Expanded = true;
                        selectedNode = selectedNode.Parent;
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogEvent(ErrorLog.EventType.eEVENT_CRITICAL, this.ToString(), "GetTopLevelLocations()", ex.Message, "", String.Format("\n\n{0}", ex.StackTrace), false);
                //Debug.Assert(false, this.ToString() + "GetTopLevelLocations() - Exception " + ex.Message);
                throw ex;
            }
            finally
            {
            }
        }
        private void GetChildLocations(ref TreeNode oParent, bool? expand, ref TreeNode selectedNode, string selectedVal)
        {
            try
            {
                List<InvCategoryBO> dtObjects;
                TreeNode oNode;
                int iLevel;
                iLevel = oParent.Depth + 1;
                dtObjects = moLocation.GetInvCategoryInfoByCustomString(String.Format("WHERE  lvl = {0} AND AncestorId = {1}", iLevel, oParent.Value));

                foreach (InvCategoryBO item in dtObjects)
                {
                    oNode = new TreeNode(HttpUtility.HtmlEncode(item.Name), item.CategoryId.ToString());
                    oNode.Expanded = false;
                    if (selectedVal == oNode.Value)
                        selectedNode = oNode;

                    oParent.ChildNodes.Add(oNode);

                    GetChildLocations(ref oNode, expand, ref selectedNode, selectedVal);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogEvent(ErrorLog.EventType.eEVENT_CRITICAL, this.ToString(), "Load_Locations", ex.Message, "", String.Format("\n\n{0}", ex.StackTrace), false);
                //Debug.Assert(false, String.Format("{0}Load_Locations - Exception {1}", this.ToString(), ex.Message));
                throw ex;
            }
            finally
            {
                ////tvLocations.EndUnboundLoad();
                ////Cursor.Current = Cursors.Default;
            }
        }
        private void IsInvCategoryCodeAutoGenerate()
        {
            code.Visible = true;
            //txtNodeNumber.Visible = true;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsInvCategoryCodeAutoGenerate", "IsInvCategoryCodeAutoGenerate");
            if (homePageSetupBO != null)
            {
                if (homePageSetupBO.SetupId > 0)
                {
                    hfIsInvCategoryCodeAutoGenerate.Value = homePageSetupBO.SetupValue;
                    if (homePageSetupBO.SetupValue == "1")
                    {
                        code.Visible = false;
                        //txtNodeNumber.Visible = false;
                    }
                }
            }
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<InvCategoryBO> GetAutoCompleteData(string searchText)
        {
            List<InvCategoryBO> nodeMatrixBOList = new List<InvCategoryBO>();
            InvCategoryDA nodeMatrixDA = new InvCategoryDA();

            nodeMatrixBOList = nodeMatrixDA.GetInvCategoryInfoByCategory(searchText);
            return nodeMatrixBOList;
        }
        [WebMethod]
        public static List<string> GetAutoCompleteData1(string searchText)
        {
            HMUtility hmUtility = new HMUtility();
            searchText = hmUtility.sqlInjectionFilter(searchText, false);

            List<string> nodeMatrixBOList = new List<string>();
            InvCategoryDA nodeMatrixDA = new InvCategoryDA();

            nodeMatrixBOList = nodeMatrixDA.GetInvCategoryInfoByCategoryNVoucherForm(searchText, 0);
            return nodeMatrixBOList;
        }
        [WebMethod]
        public static string FillForm(string searchText)
        {
            InvCategoryDA nodeMatrixDA = new InvCategoryDA();
            HMUtility hmUtility = new HMUtility();
            searchText = hmUtility.sqlInjectionFilter(searchText, false);
            string nodeMatrixBO = nodeMatrixDA.GetInvCategoryInfoBySpecificCategory(searchText);
            return nodeMatrixBO;
        }
        [WebMethod]
        public static string GetUploadedImageByWebMethod(int OwnerId, string docType)
        {
            string strTable = "";
            DocumentsDA docDA = new DocumentsDA();
            var docList = docDA.GetDocumentsInfoByDocCategoryAndOwnerId(docType, OwnerId);
            if (docList.Count > 0)
            {
                var Image = docList[0];

                strTable += "<img src='" + Image.Path + Image.Name + "'  alt='No Image Selected' border='0' style='height:150px;'/>";
            }
            return strTable;
        }
    }
}