using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmInvLocation : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;

        InvLocationDA moLocation = new InvLocationDA();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadCostCenterInfoGridView();
                txtFocusTabControl.Value = "1";
                LoadCategoryHead();
                LoadAccountHeadForSearch();
                ddlNodeId.Focus();
                tvLocations.Attributes.Add("onclick", "return OnTreeClick(event)");
                GetTopLevelLocations(null);
            }

            CheckObjectPermission();
        }
        protected void gvChartOfAccout_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            lblMessage.Text = string.Empty;
            gvChartOfAccout.PageIndex = e.NewPageIndex;
        }
        protected void gvChartOfAccout_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");

                //if (Convert.ToInt32(lblValue.Text) > 2)
                //{
                //    imgUpdate.Visible = isSavePermission;
                //    imgDelete.Visible = isDeletePermission;
                //}
                //else
                //{
                //    imgUpdate.Visible = false;
                //    imgDelete.Visible = false;
                //}
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = false;

            }
        }
        protected void gvChartOfAccout_RowCommand(object sender, GridViewCommandEventArgs e)
        {           
            int _selectedNodeId = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "CmdEdit")
            {
                txtFocusTabControl.Value = "1";
                InvLocationDA matrixDA = new InvLocationDA();
                InvLocationBO matrixBO = new InvLocationBO();

                matrixBO = matrixDA.GetInvLocationInfoById(_selectedNodeId);

                ddlNodeId.SelectedValue = matrixBO.AncestorId.ToString();
                txtNodeHead.Text = matrixBO.Name;
                txtCodeNumber.Text = matrixBO.Code;
                //ddlCostCenterId.SelectedValue = matrixBO.CostCenterId.ToString();
                txtDescription.Text = matrixBO.Description;
                txtEditNodeId.Value = matrixBO.LocationId.ToString();
                ddlIsStoreLocation.SelectedIndex = matrixBO.IsStoreLocation == true ? 1 : 0;
                ddlActiveStat.SelectedIndex = matrixBO.ActiveStat == true ? 0 : 1;

                txtEditNodeId.Value = _selectedNodeId.ToString();
                txtAncestorNodeId.Value = matrixBO.AncestorId.ToString();
                btnSave.Visible = isUpdatePermission;
                btnSave.Text = "Update";

                gvChartOfAccout.DataSource = null;
                gvChartOfAccout.DataBind();

                LoadInvLocationCostCenterMappingInfo(_selectedNodeId);

                SetTab("A");
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
            //if (ddlNodeId.SelectedIndex > 0)
            //{
            //    txtFocusTabControl.Value = "2";
            //    int NodeId = Convert.ToInt32(ddlNodeId.SelectedValue);
            //    LoadGridView(NodeId);
            //}

            string searchText = string.Empty;
            searchText = txtAccHead.Text.Trim();
            LoadGridView(searchText);

            SetTab("B");
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Cancel();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }
            int OwnerIdForDocuments = 0;
            lblMessage.Text = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            UserInformationDA userInformationDA = new UserInformationDA();

            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            InvLocationBO nodeMatrixBO = new InvLocationBO();
            InvLocationDA nodeMatrixDA = new InvLocationDA();

            if (ddlNodeId.SelectedValue == "0")
            {
                nodeMatrixBO.AncestorId = -1;
                //nodeMatrixBO.CostCenterId = Convert.ToInt32(ddlCostCenterId.SelectedValue);
            }
            else
            {
                nodeMatrixBO.AncestorId = Convert.ToInt32(ddlNodeId.SelectedValue);

                //InvLocationBO costCenterInfoBO = new InvLocationBO();
                //List<InvLocationBO> List = new List<InvLocationBO>();

                //costCenterInfoBO = nodeMatrixDA.GetInvLocationInfoById(nodeMatrixBO.AncestorId);
                //if (costCenterInfoBO != null)
                //{
                //    if (costCenterInfoBO.LocationId > 0)
                //    {
                //        nodeMatrixBO.CostCenterId = costCenterInfoBO.CostCenterId;
                //    }
                //    else
                //    {
                //        nodeMatrixBO.CostCenterId = Convert.ToInt32(ddlCostCenterId.SelectedValue);
                //    }
                //}
                //else
                //{
                //    nodeMatrixBO.CostCenterId = Convert.ToInt32(ddlCostCenterId.SelectedValue);
                //}
            }

            nodeMatrixBO.Name = txtNodeHead.Text.Trim();
            nodeMatrixBO.Code = txtCodeNumber.Text;
            //nodeMatrixBO.CostCenterId = Convert.ToInt32(ddlCostCenterId.SelectedValue);
            nodeMatrixBO.Description = txtDescription.Text;
            nodeMatrixBO.IsStoreLocation = ddlIsStoreLocation.SelectedIndex == 1 ? true : false;
            nodeMatrixBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
            int NodeId = 0;


            List<InvLocationCostCenterMappingBO> costCenterList = new List<InvLocationCostCenterMappingBO>();

            int rowsKitchenItem = gvCategoryCostCenterInfo.Rows.Count;
            for (int i = 0; i < rowsKitchenItem; i++)
            {
                CheckBox cb = (CheckBox)gvCategoryCostCenterInfo.Rows[i].FindControl("chkIsSavePermission");
                if (cb.Checked == true)
                {
                    InvLocationCostCenterMappingBO costCenter = new InvLocationCostCenterMappingBO();
                    Label lbl = (Label)gvCategoryCostCenterInfo.Rows[i].FindControl("lblCostCentreId");

                    costCenter.CostCenterId = Convert.ToInt32(lbl.Text);
                    if (!string.IsNullOrEmpty(txtEditNodeId.Value))
                    {
                        costCenter.LocationId = Int32.Parse(txtEditNodeId.Value);
                    }
                    else
                    {
                        costCenter.LocationId = 0;
                    }
                    costCenterList.Add(costCenter);
                }
            }

            if (string.IsNullOrWhiteSpace(txtEditNodeId.Value))
            {
                nodeMatrixBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = nodeMatrixDA.SaveInvLocationInfo(nodeMatrixBO, costCenterList, out NodeId);
                if (status)
                {
                    OwnerIdForDocuments = NodeId;
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), 
                    EntityTypeEnum.EntityType.ChartOfAccount.ToString(), NodeId,
                    ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(),
                    hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ChartOfAccount));
                    LoadCategoryHead();
                    GetTopLevelLocations(null);
                    Cancel();
                    //Response.Redirect("/GeneralLedger/frmNodeMatrix.aspx");
                }
            }
            else
            {
                nodeMatrixBO.LocationId = Convert.ToInt32(txtEditNodeId.Value);

                if (nodeMatrixBO.LocationId == nodeMatrixBO.AncestorId)
                {
                    nodeMatrixBO.AncestorId = -1;
                }

                nodeMatrixBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = nodeMatrixDA.UpdateInvLocationInfo(nodeMatrixBO, costCenterList, out NodeId);
                if (status)
                {
                    OwnerIdForDocuments = nodeMatrixBO.LocationId;
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.ChartOfAccount.ToString(), nodeMatrixBO.LocationId,
                    ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ChartOfAccount));
                    LoadCategoryHead();
                    GetTopLevelLocations(null);
                    Cancel();
                }
            }
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (btnSave.Text != "Save")
            {
                if (string.IsNullOrWhiteSpace(txtAncestorNodeId.Value))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Account Head.", AlertType.Warning);
                    txtAncestorNodeId.Focus();
                    flag = false;
                }
                else
                {
                    if (ddlNodeId.SelectedValue == "0")
                    {
                        ddlNodeId.SelectedValue = txtAncestorNodeId.Value;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(txtNodeHead.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Account Head.", AlertType.Warning);
                txtNodeHead.Focus();
                flag = false;
            }
            
            return flag;
        }
        private void LoadCostCenterInfoGridView()
        {
            CheckObjectPermission();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> files = costCentreTabDA.GetCostCentreTabInfo();

            gvCategoryCostCenterInfo.DataSource = files;
            gvCategoryCostCenterInfo.DataBind();
        }
        
        private void LoadCategoryHead()
        {
            InvLocationDA productCategoryDA = new InvLocationDA();
            ddlNodeId.DataSource = productCategoryDA.GetInvLocationInfo();
            ddlNodeId.DataTextField = "Name";
            ddlNodeId.DataValueField = "LocationId";
            ddlNodeId.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlNodeId.Items.Insert(0, itemNodeId);
        }
        private void LoadAccountHeadForSearch()
        {
            InvLocationDA nodeMatrixDA = new InvLocationDA();
            ddlNodeIdForEdit.DataSource = nodeMatrixDA.GetInvLocationInfo();
            ddlNodeIdForEdit.DataTextField = "Name";
            ddlNodeIdForEdit.DataValueField = "LocationId";
            ddlNodeIdForEdit.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlNodeIdForEdit.Items.Insert(0, itemNodeId);
        }
        private void LoadGridView(string searchText)
        {
            txtNodeHeadText.Value = "";
            InvLocationDA matrixDA = new InvLocationDA();
            InvLocationBO matrixBO = new InvLocationBO();
            List<InvLocationBO> List = new List<InvLocationBO>();
            List = matrixDA.GetLocationInfoByAutoSearch(searchText);

            gvChartOfAccout.DataSource = List;
            gvChartOfAccout.DataBind();

            //LoadInvLocationCostCenterMappingInfo(NodeId);
        }
        private void Cancel()
        {
            txtCodeNumber.Text = string.Empty;
            txtEditNodeId.Value = string.Empty;
            txtAncestorNodeId.Value = string.Empty;
            btnSave.Text = "Save";
            ddlNodeId.SelectedValue = "0";
            txtNodeHead.Text = string.Empty;
            txtNodeHead.Text = string.Empty;
            txtDescription.Text = string.Empty;
            ddlIsStoreLocation.SelectedIndex = 0;
            ddlActiveStat.SelectedIndex = 0;

            LoadCostCenterInfoGridView();            
        }
        private void GetTopLevelLocations(bool? expand)
        {
            try
            {
                TreeNode oNode = null;
                List<InvLocationBO> dtObjects;
                string selectedVal = (tvLocations.SelectedNode != null) ? tvLocations.SelectedNode.Value : string.Empty;
                TreeNode selectedNode = null;
                tvLocations.Nodes.Clear();
                dtObjects = moLocation.GetInvLocationInfoByCustomString("WHERE  lvl = 0");

                foreach (InvLocationBO item in dtObjects)
                {
                    oNode = new TreeNode(HttpUtility.HtmlEncode(item.Name), item.LocationId.ToString());
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
                //ErrorLog.LogEvent(ErrorLog.EventType.eEVENT_CRITICAL, ToString(), "GetTopLevelLocations()", ex.Message, "", String.Format("\n\n{0}", ex.StackTrace), false);
                //Debug.Assert(false, ToString() + "GetTopLevelLocations() - Exception " + ex.Message);
                throw ex;
            }
            finally
            {
            }
        }
        private void LoadInvLocationCostCenterMappingInfo(int EditId)
        {
            List<InvLocationCostCenterMappingBO> costListStockItem = new List<InvLocationCostCenterMappingBO>();
            InvCategoryCostCenterMappingDA costStockItemDA = new InvCategoryCostCenterMappingDA();
            costListStockItem = costStockItemDA.GetInvLocationCostCenterMappingByCategoryId(EditId);
            int rowsStockItem = gvCategoryCostCenterInfo.Rows.Count;

            List<InvLocationCostCenterMappingBO> listStockItem = new List<InvLocationCostCenterMappingBO>();
            for (int i = 0; i < rowsStockItem; i++)
            {
                InvLocationCostCenterMappingBO costCenterStockItem = new InvLocationCostCenterMappingBO();
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
        private void GetChildLocations(ref TreeNode oParent, bool? expand, ref TreeNode selectedNode, string selectedVal)
        {
            try
            {
                List<InvLocationBO> dtObjects;
                TreeNode oNode;
                int iLevel;
                iLevel = oParent.Depth + 1;
                dtObjects = moLocation.GetInvLocationInfoByCustomString(String.Format("WHERE  lvl = {0} AND AncestorId = {1}", iLevel, oParent.Value));

                foreach (InvLocationBO item in dtObjects)
                {
                    oNode = new TreeNode(HttpUtility.HtmlEncode(item.Name), item.LocationId.ToString());
                    oNode.Expanded = false;
                    if (selectedVal == oNode.Value)
                        selectedNode = oNode;

                    oParent.ChildNodes.Add(oNode);

                    GetChildLocations(ref oNode, expand, ref selectedNode, selectedVal);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogEvent(ErrorLog.EventType.eEVENT_CRITICAL, ToString(), "Load_Locations", ex.Message, "", String.Format("\n\n{0}", ex.StackTrace), false);
                //Debug.Assert(false, String.Format("{0}Load_Locations - Exception {1}", ToString(), ex.Message));
                throw ex;
            }
            finally
            {
                ////tvLocations.EndUnboundLoad();
                ////Cursor.Current = Cursors.Default;
            }
        }

        private void SetTab(string TabName)
        {
            if (TabName == "B")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "A")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }

        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<InvLocationBO> GetAutoCompleteData(string searchText)
        {
            List<InvLocationBO> nodeMatrixBOList = new List<InvLocationBO>();
            InvLocationDA nodeMatrixDA = new InvLocationDA();

            nodeMatrixBOList = nodeMatrixDA.GetInvLocationInfoByLocation(searchText);

            return nodeMatrixBOList;
        }
        [WebMethod]
        public static List<string> GetAutoCompleteData1(string searchText)
        {
            HMUtility hmUtility = new HMUtility();
            searchText = hmUtility.sqlInjectionFilter(searchText, false);

            List<string> nodeMatrixBOList = new List<string>();
            InvLocationDA nodeMatrixDA = new InvLocationDA();

            nodeMatrixBOList = nodeMatrixDA.GetInvLocationInfoByLocationNVoucherForm(searchText, 0);

            return nodeMatrixBOList;
        }
        [WebMethod]
        public static string FillForm(string searchText)
        {
            InvLocationDA nodeMatrixDA = new InvLocationDA();
            HMUtility hmUtility = new HMUtility();
            searchText = hmUtility.sqlInjectionFilter(searchText, false);
            string nodeMatrixBO = nodeMatrixDA.GetInvLocationInfoBySpecificLocation(searchText);

            return nodeMatrixBO;
        }
    }
}