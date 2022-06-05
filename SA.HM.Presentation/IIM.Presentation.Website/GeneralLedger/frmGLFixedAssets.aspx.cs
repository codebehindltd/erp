using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using System.Web.Services;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmGLFixedAssets : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadLabelInformation();
                this.LoadAccountHead();
                this.ddlNodeId.Focus();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            GLFixedAssetsDA assetsDA = new GLFixedAssetsDA();
            GLFixedAssetsBO fixedAssetBO = new GLFixedAssetsBO();

            if (this.ddlNodeId.SelectedValue != "0")
            {
                fixedAssetBO.FixedAssetsId = Int32.Parse(txtFixedAssetId.Value);
                fixedAssetBO.BlockB = Convert.ToDecimal(txtBlockB.Text);
                fixedAssetBO.BlockE = Convert.ToDecimal(txtBlockE.Text);
                fixedAssetBO.BlockF = Convert.ToDecimal(txtBlockF.Text);
                fixedAssetBO.NodeId = Int32.Parse(ddlNodeId.SelectedValue);

                if (string.IsNullOrEmpty(txtFixedAssetId.Value) || txtFixedAssetId.Value == "0")
                {
                    int tmpFixedAssetsId = 0;

                    Boolean status = assetsDA.SaveGLFixedAssetsInfo(fixedAssetBO, out tmpFixedAssetsId);
                    if (status)
                    {
                        innboardMessage.Value = JsonConvert.SerializeObject(CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Warning));
                        this.Cancel();
                    }
                }
                else
                {
                    fixedAssetBO.FixedAssetsId = Int32.Parse(txtFixedAssetId.Value);
                    Boolean status = assetsDA.UpdateGLFixedAssetsInfo(fixedAssetBO);
                    if (status)
                    {
                        innboardMessage.Value = JsonConvert.SerializeObject(CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Warning));
                        this.Cancel();
                    }
                }
            }
        }
        //************************ User Defined Function ********************//
        private void LoadLabelInformation()
        {
            var LastDay = new DateTime(DateTime.Now.Year, 12, 31).AddYears(-1);
            this.lblBlockE.Text = "Depreciation Rate(%)";
            this.lblBlockF.Text = "Accumulated Depreciation As at " + hmUtility.GetStringFromDateTime(LastDay);
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmGLFixedAssets.ToString());
            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
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
            itemNodeId.Text = "---None---";
            this.ddlNodeId.Items.Insert(0, itemNodeId);
        }
        private void Cancel()
        {
            isNewAddButtonEnable = 1;
            txtAccountHead.Text = string.Empty;
            txtFixedAssetId.Value = string.Empty;
            txtBlockB.Text = string.Empty;
            txtBlockE.Text = string.Empty;
            txtBlockF.Text = string.Empty;
            btnSave.Text = "Save";
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static GLFixedAssetsBO GetBlockValueByNodeIdByWebMethod(int NodeId)
        {
            GLFixedAssetsBO fixedAssetBO = new GLFixedAssetsBO();
            GLFixedAssetsDA fixedAssetDA = new GLFixedAssetsDA();
            fixedAssetBO = fixedAssetDA.GetGLFixedAssetsByNodeId(NodeId);
            return fixedAssetBO;
        }
    }
}