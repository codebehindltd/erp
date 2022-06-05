using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.SalesManagment;

namespace HotelManagement.Presentation.Website.SalesManagment
{
    public partial class frmSalesBandwidthInfo : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        private bool isSavePermission = false,isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            string DeleteConfirmation = Request.QueryString["DeleteConfirmation"];

            if (!string.IsNullOrWhiteSpace(DeleteConfirmation))
            {
                this.isMessageBoxEnable = 2;
                lblMessage.Text = "Delete Operation Successfull";
            }
        }
        protected void gvBandwidthType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int bandwidthInfoId = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "CmdEdit")
            {
                FillForm(bandwidthInfoId);
                btnSave.Text = "Update";
                SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("SalesBandwidthInfo", "BandwidthInfoId", bandwidthInfoId);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    this.lblMessage.Text = "Delete Operation Successfully";
                }
                LoadGridView();
                this.SetTab("SearchTab");
            }
        }
        protected void gvBandwidthType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");

                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvBandwidthType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            lblMessage.Text = string.Empty;
            gvBandwidthType.PageIndex = e.NewPageIndex;

            LoadGridView();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBandwidthName.Text))
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Provide Bandwidth Name";
                this.txtBandwidthName.Focus();
                return;
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            SalesBandwidthInfoBO salesBandwidthInfoBO = new SalesBandwidthInfoBO();
            SalesBandwidthInfoDA bandwidthDA = new SalesBandwidthInfoDA();

            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            salesBandwidthInfoBO.BandwidthName = txtBandwidthName.Text;
            salesBandwidthInfoBO.BandwidthType = ddlBandwidthType.SelectedValue;
            salesBandwidthInfoBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

            if (string.IsNullOrWhiteSpace(txtBandwidthInfoId.Value))
            {
                int tmpBandwidthId = 0;
                salesBandwidthInfoBO.CreatedBy = userInformationBO.UserInfoId;
                bool status = bandwidthDA.SaveSalesBandwidthInfo(salesBandwidthInfoBO, out tmpBandwidthId);

                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Saved Operation Successfull";
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.SalesBandwidthInfo.ToString(), tmpBandwidthId,
                    ProjectModuleEnum.ProjectModule.SalesManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalesBandwidthInfo));
                    this.Cancel();
                }
            }
            else
            {
                salesBandwidthInfoBO.BandwidthInfoId = Convert.ToInt32(txtBandwidthInfoId.Value);
                salesBandwidthInfoBO.LastModifiedBy = userInformationBO.UserInfoId;
                bool status = bandwidthDA.UpdateSalesBandwidthInfoBO(salesBandwidthInfoBO);

                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Saved Operation Successfull";
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.SalesBandwidthInfo.ToString(), salesBandwidthInfoBO.BandwidthInfoId,
                    ProjectModuleEnum.ProjectModule.SalesManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalesBandwidthInfo)); 
                    this.Cancel();
                }
            }

            if (gvBandwidthType.Rows.Count > 0)
            {
                this.LoadGridView();
            }

            SetTab("EntryTab");
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmSalesBandwidthInfo.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;

            btnSave.Visible = isSavePermission;
        }
        private void FillForm(int EditId)
        {
            SalesBandwidthInfoDA bandwidthDA = new SalesBandwidthInfoDA();
            SalesBandwidthInfoBO salesBandwidthInfoBO = new SalesBandwidthInfoBO();
            salesBandwidthInfoBO = bandwidthDA.GetSalesBandwidthInfoById(EditId);

            txtBandwidthName.Text = salesBandwidthInfoBO.BandwidthName;
            txtBandwidthInfoId.Value = salesBandwidthInfoBO.BandwidthInfoId.ToString();
            ddlBandwidthType.SelectedValue = salesBandwidthInfoBO.BandwidthType;
            ddlActiveStat.SelectedValue = (salesBandwidthInfoBO.ActiveStat ? 0 : 1).ToString();
            this.SetTab("EntryTab");
        }
        private void LoadGridView()
        {
            CheckObjectPermission();
            string bandwidthName, bandwidthType; bool status;

            bandwidthName = txtSBandwidthName.Text;
            bandwidthType = ddlSBandwidthType.SelectedValue;
            status = ddlSActiveStat.SelectedIndex == 0 ? true : false;

            SalesBandwidthInfoDA bandwidthDA = new SalesBandwidthInfoDA();
            List<SalesBandwidthInfoBO> salesBandwidthInfoList = new List<SalesBandwidthInfoBO>();
            salesBandwidthInfoList = bandwidthDA.GetSalesBandwidthInfoBySearchCriteria(bandwidthName, bandwidthType, status);

            gvBandwidthType.DataSource = salesBandwidthInfoList;
            gvBandwidthType.DataBind();

            SetTab("SearchTab");
        }
        private void Cancel()
        {
            txtBandwidthInfoId.Value = string.Empty;
            txtBandwidthName.Text = string.Empty;
            ddlBandwidthType.SelectedIndex = 0;
            ddlSActiveStat.SelectedIndex = 0;
            btnSave.Text = "Save";
            this.SetTab("EntryTab");
            txtBandwidthName.Focus();
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
        public static string DeleteData(int bandwidthInfoId)
        {
            string result = string.Empty;
            HMCommonDA hmCommonDA = new HMCommonDA();

            bool status = hmCommonDA.DeleteInfoById("SalesBandwidthInfo", "BandwidthInfoId", bandwidthInfoId);

            if (status)
                result = "Success";

            return result;
        }
    }
}