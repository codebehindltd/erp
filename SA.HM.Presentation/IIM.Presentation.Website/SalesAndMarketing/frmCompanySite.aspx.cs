using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HMCommon;
using System.Web.Services;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.SalesAndMarketing;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class frmCompanySite : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        private Boolean isViewPermission = false;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                txtSiteName.Focus();
                LoadCompany();
                //LoadIndustry();
                //LoadLocation();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }
        private void CreadeNodeMatrixAccountHeadInfo(int AncestorId, out int NodeId)
        {
            NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            // Need to Fix----------------------------------
            nodeMatrixBO.AncestorId = AncestorId;
            nodeMatrixBO.NodeHead = txtSiteName.Text.Trim();
            nodeMatrixBO.NodeMode = true;
            Boolean status = nodeMatrixDA.SaveNodeMatrixInfoFromOtherPage(nodeMatrixBO, out NodeId);
        }
        private void UpdateNodeMatrixAccountHeadInfo(int nodeId)
        {
            NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            nodeMatrixBO.NodeId = nodeId;
            nodeMatrixBO.NodeHead = txtSiteName.Text.Trim();
            nodeMatrixBO.NodeMode = true;
            Boolean status = nodeMatrixDA.UpdateNodeMatrixInfoFromOtherPage(nodeMatrixBO);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }
        protected void gvGuestCompany_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            lblMessage.Text = string.Empty;
            gvGuestCompany.PageIndex = e.NewPageIndex;
            LoadGridView();
        }
        protected void gvGuestCompany_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvGuestCompany_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int companyId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(companyId);
                btnSave.Text = "Update";
                SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("HotelGuestCompany", "CompanyId", companyId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                        EntityTypeEnum.EntityType.GuestCompany.ToString(), companyId,
                        ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestCompany));
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                HMCommonDA hmCommonDA = new HMCommonDA();
                CompanySiteBO siteBo = new CompanySiteBO();
                GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                siteBo.CompanyId = Convert.ToInt32(ddlCompanyId.SelectedValue);
                siteBo.SiteName = txtSiteName.Text;
                siteBo.BusinessContactName = txtBusinessContactName.Text.Trim();
                siteBo.BusinessContactEmail = txtBusinessContactEmail.Text;
                siteBo.BusinessContactPhone = txtBusinessContactPhone.Text;

                siteBo.TechnicalContactName = txtTechnicalContactName.Text.Trim();
                siteBo.TechnicalContactEmail = txtTechnicalContactEmail.Text;
                siteBo.TechnicalContactPhone = txtTechnicalContactPhone.Text;

                siteBo.BillingContactName = txtBillingContactName.Text.Trim();
                siteBo.BillingContactEmail = txtBillingContactEmail.Text;
                siteBo.BillingContactPhone = txtBillingContactPhone.Text;

                siteBo.Remarks = txtRemarks.Text;

                if (string.IsNullOrWhiteSpace(hfSiteId.Value))
                {
                    if (DuplicateCheckDynamicaly("CompanyName", txtSiteName.Text, 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Company Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        return;
                    }

                    int tmpCompanyId = 0;
                    siteBo.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = guestCompanyDA.SaveCompanySite(siteBo, out tmpCompanyId);

                    if (status)
                    {                        
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                        EntityTypeEnum.EntityType.GuestCompany.ToString(), tmpCompanyId,
                        ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestCompany));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Cancel();
                    }
                }
                else
                {
                    if (DuplicateCheckDynamicaly("CompanyName", txtSiteName.Text, 1) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Company Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        txtSiteName.Focus();
                        return;
                    }

                    siteBo.SiteId = Convert.ToInt32(hfSiteId.Value);
                    siteBo.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = guestCompanyDA.UpdateCompanySite(siteBo);
                    if (status)
                    {                        
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                        EntityTypeEnum.EntityType.GuestCompany.ToString(), siteBo.SiteId,
                        ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CompanySite));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Cancel();
                    }
                }

            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
               
            }

        }

        private void LoadCompany()
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<GuestCompanyBO> company = new List<GuestCompanyBO>();
            company = companyDA.GetGuestCompanyInfo();

            ddlCompanyId.DataSource = company;
            ddlCompanyId.DataTextField = "CompanyName";
            ddlCompanyId.DataValueField = "CompanyId";
            ddlCompanyId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCompanyId.Items.Insert(0, item);
        }
        private void LoadLocation()
        {
            LocationDA locationDA = new LocationDA();
            List<LocationBO> locationBO = new List<LocationBO>();
            locationBO = locationDA.GetLocationInfo();

            //ddlLocation.DataSource = locationBO;
            //ddlLocation.DataTextField = "LocationName";
            //ddlLocation.DataValueField = "LocationId";
            //ddlLocation.DataBind();

            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //ddlLocation.Items.Insert(0, item);
        }
        private void LoadGetAccountManager()
        {
            //AccountManagerDA locationDA = new AccountManagerDA();
            //List<AccountManagerBO> locationBO = new List<AccountManagerBO>();
            //locationBO = locationDA.GetAccountManager();

            //ddlReferenceId.DataSource = locationBO;
            //ddlReferenceId.DataTextField = "DisplayName";
            //ddlReferenceId.DataValueField = "AccountManagerId";
            //ddlReferenceId.DataBind();

            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //ddlReferenceId.Items.Insert(0, item);
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "HotelGuestCompany";
            string pkFieldName = "CompanyId";
            string pkFieldValue = hfSiteId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmGuestCompany.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void LoadGridView()
        {
            CheckObjectPermission();
            string Email = txtSBusinessContactEmail.Text;
            string siteName = txtSSiteName.Text;
            string contactPhone = txtSBusinessContactPhone.Text;
            string contactName = txtSBusinessContactName.Text;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<CompanySiteBO> files = new List<CompanySiteBO>();

            files = guestCompanyDA.GetCompanySiteBySearchCriteria(siteName, contactName, Email, contactPhone);

            gvGuestCompany.DataSource = files;
            gvGuestCompany.DataBind();
            SetTab("SearchTab");
        }
        private void Cancel()
        {
            txtSiteName.Text = string.Empty;
            txtBusinessContactPhone.Text = string.Empty;
            txtBusinessContactName.Text = string.Empty;
            txtBusinessContactEmail.Text = string.Empty;
            txtTechnicalContactEmail.Text = string.Empty;
            txtTechnicalContactName.Text = string.Empty;
            txtTechnicalContactPhone.Text = string.Empty;
            txtBillingContactEmail.Text = string.Empty;
            txtBillingContactName.Text = string.Empty;
            txtBillingContactPhone.Text = string.Empty;

            txtRemarks.Text = string.Empty;
            hfSiteId.Value = string.Empty;
            btnSave.Text = "Save";
            txtSiteName.Focus();
            ddlCompanyId.SelectedIndex = 0;
            SetTab("EntryTab");
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
            CompanySiteBO site = new CompanySiteBO();
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();

            site = guestCompanyDA.GetCompanySiteById(EditId);

            txtSiteName.Text = site.SiteName;

            txtBusinessContactName.Text = site.BusinessContactName;
            txtBusinessContactEmail.Text = site.BusinessContactEmail;
            txtBusinessContactPhone.Text = site.BusinessContactPhone;

            txtBillingContactName.Text = site.BillingContactName;
            txtBillingContactPhone.Text = site.BillingContactPhone;
            txtBillingContactEmail.Text = site.BillingContactEmail;

            txtTechnicalContactName.Text = site.TechnicalContactName;
            txtTechnicalContactEmail.Text = site.TechnicalContactEmail;
            txtTechnicalContactPhone.Text = site.TechnicalContactPhone;

            txtRemarks.Text = site.Remarks;

            hfSiteId.Value = site.SiteId.ToString();
            ddlCompanyId.SelectedValue = site.CompanyId.ToString();
        }
        private void LoadIndustry()
        {
            IndustryDA industryDA = new IndustryDA();
            List<IndustryBO> industryBO = new List<IndustryBO>();
            industryBO = industryDA.GetIndustryInfo();

            //ddlIndustryId.DataSource = industryBO;
            //ddlIndustryId.DataTextField = "IndustryName";
            //ddlIndustryId.DataValueField = "IndustryId";
            //ddlIndustryId.DataBind();

            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //ddlIndustryId.Items.Insert(0, item);
        }

    }
}