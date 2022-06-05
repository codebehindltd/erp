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
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmNodeMatrix : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();        
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        NodeMatrixDA moLocation = new NodeMatrixDA();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            this.pnlCoaConfiguration.Visible = false;
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.txtFocusTabControl.Value = "1";
                this.LoadAccountHead();
                this.LoadAccountHeadForSearch();
                this.LoadCashFlowHead();
                this.ddlNodeId.Focus();
                this.LoadProfitLossHead();
                this.LoadBalanceSheetHead();
                tvLocations.Attributes.Add("onclick", "return OnTreeClick(event)");
                GetTopLevelLocations(null);
            }

            this.CheckObjectPermission();
        }
        protected void gvChartOfAccout_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {            
            this.gvChartOfAccout.PageIndex = e.NewPageIndex;
            this.LoadGridView();
        }
        protected void gvChartOfAccout_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");

                if (Convert.ToInt32(lblValue.Text) > 30)
                {
                    imgUpdate.Visible = isSavePermission;
                    imgDelete.Visible = isDeletePermission;
                }
                else
                {
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                }
                imgDelete.Visible = false;
            }
        }
        protected void gvChartOfAccout_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            foreach (ListItem item in chkAccountType.Items)
            {
                item.Selected = false;
            }
            int _selectedNodeId = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "CmdEdit")
            {
                this.txtFocusTabControl.Value = "1";
                NodeMatrixDA matrixDA = new NodeMatrixDA();
                NodeMatrixBO matrixBO = new NodeMatrixBO();
                List<GLAccountTypeSetupBO> AccTypeList = new List<GLAccountTypeSetupBO>();
                AccTypeList = matrixDA.GetAccountTypeInfoByNodeId(_selectedNodeId);
                matrixBO = matrixDA.GetNodeMatrixInfoById(_selectedNodeId);
                //int length = AccTypeList.Count - 1;
                int length = AccTypeList.Count;
                for (int i = 0; i < length; i++)
                {
                    foreach (ListItem item in chkAccountType.Items)
                    {
                        if (item.Value == AccTypeList[i].AccountType)
                        {
                            item.Selected = true;
                        }
                    }
                }

                ddlHeadId.SelectedValue = matrixBO.CFHeadId.ToString();
                ddlPLHeadId.SelectedValue = matrixBO.PLHeadId.ToString();
                this.ddlNodeId.SelectedValue = matrixBO.AncestorId.ToString();
                this.txtNodeNumber.Text = matrixBO.NodeNumber;
                this.txtNodeHead.Text = matrixBO.NodeHead;
                this.txtEditNodeId.Value = matrixBO.NodeId.ToString();

                //ddlActiveStat.SelectedValue = matrixBO.NodeMode == true ? "1" : "0";
                ddlActiveStat.SelectedValue = (matrixBO.NodeMode == true ? 0 : 1).ToString();

                this.LoadCashFlowHead();

                GLCashFlowHeadDA entityGLCashFlowHeadDA = new GLCashFlowHeadDA();
                GLCashFlowHeadBO entityGLCashFlowHeadBO = new GLCashFlowHeadBO();

                entityGLCashFlowHeadBO = entityGLCashFlowHeadDA.GetGLCashFlowHeadInfoByNodeId(_selectedNodeId);
                if (entityGLCashFlowHeadBO != null)
                {
                    this.ddlHeadId.SelectedValue = entityGLCashFlowHeadBO.HeadId.ToString();
                    this.txtCashFlowInformation.Value = entityGLCashFlowHeadBO.CFSetupId.ToString();
                }
                else
                {
                    this.ddlHeadId.SelectedValue = "0";
                    this.txtCashFlowInformation.Value = "0";
                }

                GLProfitLossHeadDA GLPLHeadDA = new GLProfitLossHeadDA();
                GLProfitLossHeadBO GLPLHeadBO = new GLProfitLossHeadBO();
                GLPLHeadBO = GLPLHeadDA.GetGLProfitLossHeadInfoByNodeId(_selectedNodeId);
                if (GLPLHeadBO != null)
                {
                    this.ddlPLHeadId.SelectedValue = GLPLHeadBO.PLHeadId.ToString();
                    this.txtProfitLossInformation.Value = GLPLHeadBO.PLSetupId.ToString();
                }
                else
                {
                    this.ddlPLHeadId.SelectedValue = "0";
                    this.txtProfitLossInformation.Value = "0";
                }

                GLBalanceSheetHeadDA GLBSHeadDA = new GLBalanceSheetHeadDA();
                GLBalanceSheetHeadBO GLBSHeadBO = new GLBalanceSheetHeadBO();
                GLBSHeadBO = GLBSHeadDA.GetGLBalanceSheetHeadInfoByNodeId(_selectedNodeId);
                if (GLBSHeadBO != null)
                {
                    this.ddlBSHeadId.SelectedValue = GLBSHeadBO.RCId.ToString();
                    this.txtBalanceSheetInformation.Value = GLBSHeadBO.SetupId.ToString();
                }
                else {
                    this.ddlBSHeadId.SelectedValue = "0";
                    this.txtBalanceSheetInformation.Value = "0";
                }

                this.txtEditNodeId.Value = _selectedNodeId.ToString();
                this.txtAncestorNodeId.Value = matrixBO.AncestorId.ToString();
                this.btnSave.Text = "Update";

                this.gvChartOfAccout.DataSource = null;
                this.gvChartOfAccout.DataBind();
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
            if (ddlNodeId.SelectedIndex > 0)
            {
                this.txtFocusTabControl.Value = "2";
                int NodeId = Convert.ToInt32(this.ddlNodeId.SelectedValue);
                this.LoadGridView(NodeId);
            }
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

            List<GLAccountTypeSetupBO> entityBOList = new List<GLAccountTypeSetupBO>();
            foreach (ListItem item in this.chkAccountType.Items)
            {
                GLAccountTypeSetupBO entityBO = new GLAccountTypeSetupBO();
                if (item.Selected)
                {
                    entityBO.AccountType = item.Value;
                    entityBOList.Add(entityBO);
                }
            }

            int HeadId = Int32.Parse(ddlHeadId.SelectedValue);
            int plHeadId = Int32.Parse(ddlPLHeadId.SelectedValue);
            int bsHeadId = Int32.Parse(ddlBSHeadId.SelectedValue);
            
            UserInformationBO userInformationBO = new UserInformationBO();
            UserInformationDA userInformationDA = new UserInformationDA();

            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            nodeMatrixBO.AncestorId = Convert.ToInt32(this.ddlNodeId.SelectedValue);
            nodeMatrixBO.NodeNumber = this.txtNodeNumber.Text.Trim();
            nodeMatrixBO.NodeHead = this.txtNodeHead.Text.Trim();
            if (!string.IsNullOrWhiteSpace(this.txtCashFlowInformation.Value))
            {
                nodeMatrixBO.CFSetupId = Convert.ToInt32(this.txtCashFlowInformation.Value);
            }
            nodeMatrixBO.CFHeadId = HeadId;
            if (!string.IsNullOrWhiteSpace(this.txtProfitLossInformation.Value))
            {
                nodeMatrixBO.PLSetupId = Convert.ToInt32(this.txtProfitLossInformation.Value);
            }
            nodeMatrixBO.PLHeadId = plHeadId;
            if (!string.IsNullOrWhiteSpace(this.txtBalanceSheetInformation.Value))
            {
                nodeMatrixBO.BSSetupId = Convert.ToInt32(this.txtBalanceSheetInformation.Value);
            }
            nodeMatrixBO.BSHeadId = bsHeadId;
            nodeMatrixBO.NodeMode = ddlActiveStat.SelectedIndex == 0 ? true : false;
            int NodeId = 0;

            if (string.IsNullOrWhiteSpace(txtEditNodeId.Value))
            {
                nodeMatrixBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = nodeMatrixDA.SaveNodeMatrixInfo(nodeMatrixBO, out NodeId, entityBOList);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.ChartOfAccount.ToString(), NodeId,
                    ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ChartOfAccount));
                    this.LoadAccountHead();
                    GetTopLevelLocations(null);
                    this.Cancel();
                    //Response.Redirect("/GeneralLedger/frmNodeMatrix.aspx");
                }
            }
            else
            {
                nodeMatrixBO.NodeId = Convert.ToInt32(txtEditNodeId.Value);
                nodeMatrixBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = nodeMatrixDA.UpdateNodeMatrixInfo(nodeMatrixBO, out NodeId, entityBOList);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.ChartOfAccount.ToString(), nodeMatrixBO.NodeId,
                    ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ChartOfAccount));
                    this.LoadAccountHead();
                    GetTopLevelLocations(null);
                    this.Cancel();
                }
            }
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmNodeMatrix.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (ddlNodeId.SelectedValue == "0")
            {
                if (this.btnSave.Text == "Save")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Account Head.", AlertType.Warning);
                    ddlNodeId.Focus();
                    flag = false;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(this.txtAncestorNodeId.Value))
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Account Head.", AlertType.Warning);
                        ddlNodeId.Focus();
                        flag = false;
                    }
                    else
                    {
                        if (ddlNodeId.SelectedValue == "0")
                        {
                            this.ddlNodeId.SelectedValue = this.txtAncestorNodeId.Value;
                        }
                    }
                }
            }

            return flag;
        }
        private void LoadAccountHead()
        {
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            this.ddlNodeId.DataSource = nodeMatrixDA.GetNodeMatrixInfo();
            this.ddlNodeId.DataTextField = "NodeHead";
            this.ddlNodeId.DataValueField = "NodeId";
            this.ddlNodeId.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlNodeId.Items.Insert(0, itemNodeId);
        }
        private void LoadAccountHeadForSearch()
        {
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            this.ddlNodeIdForEdit.DataSource = nodeMatrixDA.GetNodeMatrixInfo();
            this.ddlNodeIdForEdit.DataTextField = "NodeHead";
            this.ddlNodeIdForEdit.DataValueField = "NodeId";
            this.ddlNodeIdForEdit.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlNodeIdForEdit.Items.Insert(0, itemNodeId);
        }
        private void LoadGridView(int NodeId)
        {
            txtNodeHeadText.Value = "";
            NodeMatrixDA matrixDA = new NodeMatrixDA();
            NodeMatrixBO matrixBO = new NodeMatrixBO();
            List<NodeMatrixBO> List = new List<NodeMatrixBO>();
            matrixBO = matrixDA.GetNodeMatrixInfoById(NodeId);
            txtNodeHeadText.Value = matrixBO.NodeHead;
            List.Add(matrixBO);
            this.gvChartOfAccout.DataSource = List;
            this.gvChartOfAccout.DataBind();
        }
        private void LoadProfitLossHead()
        {
            GLProfitLossHeadDA GLPLHead = new GLProfitLossHeadDA();
            this.ddlPLHeadId.DataSource = GLPLHead.GetLossProfitHeadInfo();

            this.ddlPLHeadId.DataTextField = "PLHeadWithNotes";
            this.ddlPLHeadId.DataValueField = "PLHeadId";
            this.ddlPLHeadId.DataBind();

            ListItem itemHeadId = new ListItem();
            itemHeadId.Value = "0";
            itemHeadId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlPLHeadId.Items.Insert(0, itemHeadId);
        }
        private void LoadCashFlowHead()
        {
            GLCashFlowHeadDA entityDA = new GLCashFlowHeadDA();

            this.ddlHeadId.DataSource = entityDA.GetGLCashFlowHeadInfo();
            this.ddlHeadId.DataTextField = "CashFlowHeadWithNotes";
            this.ddlHeadId.DataValueField = "HeadId";
            this.ddlHeadId.DataOptionGroupField = "GroupHead";
            this.ddlHeadId.DataBind();
        }
        private void LoadBalanceSheetHead()
        {
            GLBalanceSheetHeadDA GLBSHead = new GLBalanceSheetHeadDA();
            this.ddlBSHeadId.DataSource = GLBSHead.GetGLBalanceSheetHeadInfo();

            this.ddlBSHeadId.DataTextField = "NodeHead";
            this.ddlBSHeadId.DataValueField = "RCId";
            this.ddlBSHeadId.DataBind();

            ListItem itemHeadId = new ListItem();
            itemHeadId.Value = "0";
            itemHeadId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBSHeadId.Items.Insert(0, itemHeadId);           
        }
        private void LoadGridView()
        {
            //this.CheckObjectPermission();
            //UserInformationDA userInformationDA = new UserInformationDA();
            //List<UserInformationBO> files = userInformationDA.GetUserInformation();

            //this.gvUserInformation.DataSource = files;
            //this.gvUserInformation.DataBind();
        }
        private void Cancel()
        {
            this.txtEditNodeId.Value = string.Empty;
            this.txtAncestorNodeId.Value = string.Empty;
            this.txtCashFlowInformation.Value = string.Empty;
            this.txtProfitLossInformation.Value = string.Empty;
            this.txtBalanceSheetInformation.Value = string.Empty;
            this.btnSave.Text = "Save";
            this.ddlNodeId.SelectedValue = "0";
            this.txtNodeHead.Text = string.Empty;
            this.txtNodeHead.Text = string.Empty;
            this.txtNodeNumber.Text = string.Empty;
            this.ddlPLHeadId.SelectedValue = "0";
            this.ddlBSHeadId.SelectedValue = "0";
            foreach (ListItem item in chkAccountType.Items)
            {
                item.Selected = false;
            }
            this.LoadCashFlowHead();
        }
        private void GetTopLevelLocations(bool? expand)
        {
            try
            {
                TreeNode oNode = null;
                List<NodeMatrixBO> dtObjects;
                string selectedVal = (tvLocations.SelectedNode != null) ? tvLocations.SelectedNode.Value : string.Empty;
                TreeNode selectedNode = null;
                tvLocations.Nodes.Clear();
                dtObjects = moLocation.GetNodeMatrixInfoByCustomString("WHERE  lvl = 0 AND ISNULL(NodeMode, 0) <> 0");

                foreach (NodeMatrixBO item in dtObjects)
                {
                    oNode = new TreeNode(HttpUtility.HtmlEncode(item.NodeHead), item.NodeId.ToString());
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
                List<NodeMatrixBO> dtObjects;
                TreeNode oNode;
                int iLevel;
                iLevel = oParent.Depth + 1;
                dtObjects = moLocation.GetNodeMatrixInfoByCustomString(String.Format("WHERE  lvl = {0} AND AncestorId = {1}", iLevel, oParent.Value));

                foreach (NodeMatrixBO item in dtObjects)
                {
                    oNode = new TreeNode(HttpUtility.HtmlEncode(item.NodeHead), item.NodeId.ToString());
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
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<NodeMatrixBO> GetAutoCompleteData(string searchText)
        {
            List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            nodeMatrixBOList = nodeMatrixDA.GetNodeMatrixInfoByAccountHead(searchText);

            return nodeMatrixBOList;
        }
        [WebMethod]
        public static List<string> GetAutoCompleteData1(string searchText)
        {
            HMUtility hmUtility = new HMUtility();
            searchText = hmUtility.sqlInjectionFilter(searchText, false);

            List<string> nodeMatrixBOList = new List<string>();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            nodeMatrixBOList = nodeMatrixDA.GetNodeMatrixInfoByAccountHead1(searchText, 0);

            return nodeMatrixBOList;
        }
        [WebMethod]
        public static string FillForm(string searchText)
        {
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            HMUtility hmUtility = new HMUtility();
            searchText = hmUtility.sqlInjectionFilter(searchText, false);
            string nodeMatrixBO = nodeMatrixDA.GetNodeMatrixInfoByAccountHead2(searchText);

            return nodeMatrixBO;
        }
    }
}