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
using static HotelManagement.Presentation.Website.Common.ConstantHelper;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class frmGuestCompany : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isUpdatePermission = false;
        private Boolean isDeletePermission = false;
        private Boolean isViewPermission = false;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                string pageNumber = string.Empty;
                string gridRecordCounts = string.Empty;
                string isCurrentRPreviouss = string.Empty;
                if (Request.QueryString["pn"] != null)
                {
                    pageNumber = Request.QueryString["pn"];
                    gridRecordCounts = Request.QueryString["grc"];
                    isCurrentRPreviouss = Request.QueryString["icp"];
                    LoadGridView(Convert.ToInt32(pageNumber), Convert.ToInt32(gridRecordCounts), Convert.ToInt32(isCurrentRPreviouss));
                }

                txtCompanyName.Focus();
                LoadIndustry();
                //LoadLocation();
                LoadReference();

                LoadAccountManager();
                //LoadCountry();
                //LoadCity();
                LoadLifeCycleStage();
                LoadOwnership();
                LoadParentCompany();
                LoadCompanyType();
                LoadRandonNumber();
                //LoadUser();

                FileUpload();
                LoadSignupStatus();
                LoadCRMConfiguration();
            }
        }
        private void LoadRandonNumber()
        {
            Random rd = new Random();
            int seatingId = rd.Next(100000, 999999);
            RandomProductId.Value = seatingId.ToString();
            tempProductId.Value = seatingId.ToString();
        }
        private void LoadCRMConfiguration()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsShippingAddresswillshow", "IsShippingAddresswillshow");
            DivShippingAddress.Visible = setUpBO.SetupValue == "1";

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsDiscountPercentageWillShow", "IsDiscountPercentageWillShow");
            DivDiscountPercentage.Visible = setUpBO.SetupValue == "1";

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCreditLimitWillShow", "IsCreditLimitWillShow");
            DivCreditLimit.Visible = setUpBO.SetupValue == "1";

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsNumberOfEmployeeWillShow", "IsNumberOfEmployeeWillShow");
            DivNoOfEmployee.Visible = setUpBO.SetupValue == "1";

            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsAnnualRevenueWillShow", "IsAnnualRevenueWillShow");
            DivAnnualRevenue.Visible = setUpBO.SetupValue == "1";

            BillingAreaLabel.Visible = false;
            txtBillingLocation.Visible = false;
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCRMAreaFieldEnable", "IsCRMAreaFieldEnable");
            if (setUpBO.SetupId > 0)
            {
                if (setUpBO.SetupValue == "1")
                {
                    BillingAreaLabel.Visible = true;
                    txtBillingLocation.Visible = true;
                }
            }

            hfIsCRMCompanyNumberEnable.Value = "0";
            CompanyNumberColumnDiv.Visible = false;
            CompanyNumberSrcDiv.Visible = false;
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCRMCompanyNumberEnable", "IsCRMCompanyNumberEnable");
            if (setUpBO.SetupId > 0)
            {
                if (setUpBO.SetupValue == "1")
                {
                    hfIsCRMCompanyNumberEnable.Value = "1";
                    CompanyNumberColumnDiv.Visible = true;
                    CompanyNumberSrcDiv.Visible = true;
                }
            }

            hfIsCompanyHyperlinkEnableFromGrid.Value = "0";
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCompanyHyperlinkEnableFromGrid", "IsCompanyHyperlinkEnableFromGrid");
            if (setUpBO.SetupId > 0)
            {
                if (setUpBO.SetupValue == "1")
                {
                    hfIsCompanyHyperlinkEnableFromGrid.Value = "1";
                }
            }
        }
        private void CreadeNodeMatrixAccountHeadInfo(int AncestorId, out int NodeId)
        {
            NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            // Need to Fix----------------------------------
            nodeMatrixBO.AncestorId = AncestorId;
            nodeMatrixBO.NodeHead = txtCompanyName.Text.Trim();
            nodeMatrixBO.NodeMode = true;
            Boolean status = nodeMatrixDA.SaveNodeMatrixInfoFromOtherPage(nodeMatrixBO, out NodeId);
        }
        private void UpdateNodeMatrixAccountHeadInfo(int nodeId)
        {
            NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            nodeMatrixBO.NodeId = nodeId;
            nodeMatrixBO.NodeHead = txtCompanyName.Text.Trim();
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
            LoadGridView(1, 1, 0);
        }
        protected void gvGuestCompany_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                ImageButton imgShowDocuments = (ImageButton)e.Row.FindControl("ImgShowDocuments");

                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isSavePermission;

                Label lblCreatedByValue = (Label)e.Row.FindControl("lblCreatedBy");
                if (hfIsHotelGuestCompanyRescitionForAllUsers.Value == "1")
                {
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    if (lblCreatedByValue.Text == userInformationBO.UserInfoId.ToString())
                    {
                        imgUpdate.Visible = isUpdatePermission;
                        imgDelete.Visible = isDeletePermission;
                        imgShowDocuments.Visible = isViewPermission;
                    }
                    else
                    {
                        imgUpdate.Visible = false;
                        imgDelete.Visible = false;
                        imgShowDocuments.Visible = false;
                    }
                }
                else
                {
                    imgUpdate.Visible = isUpdatePermission;
                    imgDelete.Visible = isDeletePermission;
                    imgShowDocuments.Visible = isViewPermission;
                }

                imgUpdate.Visible = isUpdatePermission;
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
                    if (!IsValidationForDelete(companyId))
                    {
                        return;
                    }

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
                LoadGridView(1, 1, 0);
                SetTab("SearchTab");
            }
        }

        public static bool IsValidEmail(string email)
        {
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFrmValid())
                {
                    isNewAddButtonEnable = 2;
                    return;
                }

                int OwnerIdForDocuments = 0;

                HMCommonDA hmCommonDA = new HMCommonDA();
                GuestCompanyBO guestCompanyBO = new GuestCompanyBO();
                GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                if (ddlParentCompany.SelectedValue == "0")
                {
                    guestCompanyBO.AncestorId = -1;
                }
                else
                {
                    guestCompanyBO.AncestorId = Convert.ToInt32(ddlParentCompany.SelectedValue);
                }

                guestCompanyBO.CompanyOwnerId = Convert.ToInt32(ddlCompanyOwner.SelectedValue);
                guestCompanyBO.OwnershipId = Convert.ToInt32(ddlOwnership.SelectedValue);
                guestCompanyBO.CompanyName = txtCompanyName.Text;
                guestCompanyBO.CompanyType = Convert.ToInt32(ddlCompanyType.SelectedValue);
                guestCompanyBO.IndustryId = Convert.ToInt32(ddlIndustry.SelectedValue);
                guestCompanyBO.TicketNumber = txtTicketNo.Text;
                guestCompanyBO.DiscountPercent = Convert.ToDecimal(txtDiscountPercent.Text);
                guestCompanyBO.CreditLimit = Convert.ToDecimal(txtCreditLimit.Text);
                guestCompanyBO.AnnualRevenue = !string.IsNullOrWhiteSpace(txtAnnualRevenue.Text) ? Convert.ToDecimal(txtAnnualRevenue.Text) : 0;
                guestCompanyBO.NumberOfEmployee = !string.IsNullOrWhiteSpace(txtNoOfEmployee.Text) ? Convert.ToInt32(txtNoOfEmployee.Text) : 0;
                guestCompanyBO.LifeCycleStageId = Convert.ToInt32(ddlLifeCycleStageId.SelectedValue);
                guestCompanyBO.BillingStreet = txtBillingStreet.Text;
                guestCompanyBO.BillingPostCode = txtBillingPostCode.Text;
                guestCompanyBO.BillingCountryId = Convert.ToInt32(ddlBillingCountryId.SelectedValue);
                guestCompanyBO.BillingCityId = Convert.ToInt32(ddlBillingCityId.SelectedValue);
                guestCompanyBO.BillingStateId = Convert.ToInt32(ddlBillingStateId.SelectedValue);
                guestCompanyBO.ShippingStreet = txtShippingStreet.Text;
                guestCompanyBO.ShippingPostCode = txtShippingPostCode.Text;
                guestCompanyBO.ShippingCountryId = Convert.ToInt32(ddlShippingCountryId.SelectedValue);
                guestCompanyBO.ShippingCityId = Convert.ToInt32(ddlShippingCityId.SelectedValue);
                guestCompanyBO.ShippingStateId = Convert.ToInt32(ddlShippingStateId.SelectedValue);
                guestCompanyBO.EmailAddress = txtEmail.Text.Trim();
                guestCompanyBO.WebAddress = txtWebsite.Text;
                guestCompanyBO.Fax = txtFax.Text;
                guestCompanyBO.ContactNumber = txtPhone.Text;
                guestCompanyBO.Remarks = txtRemarks.Text;

                string deletedDocument = hfGuestDeletedDoc.Value;
                SalesMarketingLogType<GuestCompanyBO> logDA = new SalesMarketingLogType<GuestCompanyBO>();
                if (string.IsNullOrWhiteSpace(hfCompanyId.Value))
                {
                    if (DuplicateCheckDynamicaly("CompanyName", txtCompanyName.Text, 0) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Company Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        return;
                    }

                    int tmpCompanyId = 0;
                    guestCompanyBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = guestCompanyDA.SaveGuestCompanyInfo(guestCompanyBO, out tmpCompanyId);

                    if (status)
                    {
                        OwnerIdForDocuments = tmpCompanyId;
                        guestCompanyBO.CompanyId = tmpCompanyId;

                        logDA.Log(ConstantHelper.SalesandMarketingLogType.CompanyCreated, guestCompanyBO, guestCompanyBO);

                        int tmpNodeId = 0;
                        string paymentMode = "Company";

                        tmpNodeId = hmCommonDA.GetCommonPaymentModeInfo(paymentMode).FirstOrDefault().ReceiveAccountsPostingId;
                        Boolean postingStatus = hmCommonDA.UpdateTableInfoForNodeId("HotelGuestCompany", "CompanyId", tmpCompanyId, tmpNodeId);

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                        EntityTypeEnum.EntityType.GuestCompany.ToString(), tmpCompanyId,
                        ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestCompany));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Cancel();
                    }
                }
                else
                {
                    if (DuplicateCheckDynamicaly("CompanyName", txtCompanyName.Text, 1) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Company Name" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        txtCompanyName.Focus();
                        return;
                    }

                    guestCompanyBO.CompanyId = Convert.ToInt32(hfCompanyId.Value);
                    guestCompanyBO.LastModifiedBy = userInformationBO.UserInfoId;

                    GuestCompanyBO previousBO = new GuestCompanyBO();
                    previousBO = guestCompanyDA.GetGuestCompanyInfoById(guestCompanyBO.CompanyId);

                    Boolean status = guestCompanyDA.UpdateGuestCompanyInfo(guestCompanyBO);

                    if (status)
                    {
                        ContactInformationDA contactInformationDA = new ContactInformationDA();
                        if (!string.IsNullOrEmpty(deletedDocument))
                        {
                            bool delete = contactInformationDA.DeleteContactDocument(deletedDocument);
                        }

                        logDA.Log(ConstantHelper.SalesandMarketingLogType.CompanyActivity, guestCompanyBO, previousBO);

                        OwnerIdForDocuments = guestCompanyBO.CompanyId;
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                        EntityTypeEnum.EntityType.GuestCompany.ToString(), guestCompanyBO.CompanyId,
                        ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestCompany));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Cancel();
                    }
                }

                string docPath = Server.MapPath("") + "\\Images\\Company\\";
                Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(RandomProductId.Value));

            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);

            }

        }
        //************************ User Defined Function ********************//
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "LoadDetailGridInformation", jscript, true);
            flashUpload.QueryParameters = "CompanyDocId=" + Server.UrlEncode(RandomProductId.Value);
        }

        private void LoadCity()
        {
            CityDA cityDA = new CityDA();
            List<CityBO> cityBO = new List<CityBO>();
            cityBO = cityDA.GetCityInfo();

            ddlBillingCityId.DataSource = cityBO;
            ddlBillingCityId.DataTextField = "CityName";
            ddlBillingCityId.DataValueField = "CityId";
            ddlBillingCityId.DataBind();

            ddlShippingCityId.DataSource = cityBO;
            ddlShippingCityId.DataTextField = "CityName";
            ddlShippingCityId.DataValueField = "CityId";
            ddlShippingCityId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlBillingCityId.Items.Insert(0, item);
            ddlShippingCityId.Items.Insert(0, item);
        }
        private void LoadLocation()
        {
            LocationDA locationDA = new LocationDA();
            List<LocationBO> locationBO = new List<LocationBO>();
            locationBO = locationDA.GetLocationInfo();

            ddlLocation.DataSource = locationBO;
            ddlLocation.DataTextField = "LocationName";
            ddlLocation.DataValueField = "LocationId";
            ddlLocation.DataBind();

            ddlBillingStateId.DataSource = locationBO;
            ddlBillingStateId.DataTextField = "LocationName";
            ddlBillingStateId.DataValueField = "LocationId";
            ddlBillingStateId.DataBind();

            ddlShippingStateId.DataSource = locationBO;
            ddlShippingStateId.DataTextField = "LocationName";
            ddlShippingStateId.DataValueField = "LocationId";
            ddlShippingStateId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlLocation.Items.Insert(0, item);

            ddlShippingStateId.Items.Insert(0, item);
            ddlBillingStateId.Items.Insert(0, item);
        }
        private void LoadReference()
        {
            int isAdminUser = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            AccountManagerDA accountManagerDA = new AccountManagerDA();
            List<AccountManagerBO> accountManagerBOList = new List<AccountManagerBO>();

            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                isAdminUser = 1;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 22).Count() > 0)
                    {
                        isAdminUser = 1;
                    }
                }
            }
            #endregion

            accountManagerBOList = accountManagerDA.GetAccountManager(isAdminUser, "CRM", userInformationBO.UserInfoId);

            ddlReferenceId.DataSource = accountManagerBOList;
            ddlReferenceId.DataTextField = "DisplayName";
            ddlReferenceId.DataValueField = "AccountManagerId";
            ddlReferenceId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlReferenceId.Items.Insert(0, item);
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "HotelGuestCompany";
            string pkFieldName = "CompanyId";
            string pkFieldValue = txtCompanyId.Value;
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
            isUpdatePermission = objectPermissionBO.IsUpdatePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            isViewPermission = objectPermissionBO.IsViewPermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }

            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                hfIsHotelGuestCompanyRescitionForAllUsers.Value = "0";
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 22).Count() > 0)
                    {
                        hfIsHotelGuestCompanyRescitionForAllUsers.Value = "0";
                    }
                }
            }
            #endregion
        }
        private bool IsFrmValid()
        {
            Decimal number;
            bool flag = true;
            if (ddlCompanyOwner.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "appropriate company owner.", AlertType.Warning);
                ddlCompanyOwner.Focus();
                flag = false;
            }
            if (txtCompanyName.Text.Trim() == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Company Name.", AlertType.Warning);
                txtCompanyName.Focus();
                flag = false;
            }
            else if (ddlLifeCycleStageId.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Life cycle stage", AlertType.Warning);
                ddlLifeCycleStageId.Focus();
                flag = false;
            }
            else if (txtPhone.Text.Trim() == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Phone Number.", AlertType.Warning);
                txtPhone.Focus();
                flag = false;
            }
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(strRegex);
                if (re.IsMatch(txtEmail.Text))
                { }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Email Address.", AlertType.Warning);
                    txtEmail.Focus();
                    flag = false;
                }
            }
            if (!string.IsNullOrWhiteSpace(txtAnnualRevenue.Text))
            {
                string strRegex = @"^(0|[1-9]\d*)?(\.\d+)?(?<=\d)$";
                Regex re = new Regex(strRegex);
                if (re.IsMatch(txtAnnualRevenue.Text))
                { }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid Annual Revenue.", AlertType.Warning);
                    txtAnnualRevenue.Focus();
                    flag = false;
                }
            }
            return flag;
        }
        private bool IsValidationForDelete(Int64 companyId)
        {
            bool flag = true;

            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            Boolean status = guestCompanyDA.CompanyDeleteValidation(companyId);
            if (!status)
            {
                CommonHelper.AlertInfo(innboardMessage, "This Company already used, So you can't delete.", AlertType.Warning);
                txtCompanyName.Focus();
                flag = false;
            }
            return flag;
        }
        [WebMethod]
        public static GridViewDataNPaging<GuestCompanyBO, GridPaging> LoadCompanyForSearch(string companyName, Int32 companyType, string contactNumber, string companyEmail, Int64 countryId, Int64 stateId, Int64 cityId, Int64 areaId, int lifeCycleStage, Int32 companyOwnerId, Int32 dateSearchCriteria, string SearchFromDate, string SearchToDate, string companyNumber, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            int totalRecords = 0;
            var checkOb = new frmGuestCompany();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            DateTime dateTime = DateTime.Now;
            string startDate = string.Empty;
            string endDate = string.Empty;
            if (string.IsNullOrWhiteSpace(SearchFromDate))
            {
                startDate = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = SearchFromDate;
            }
            if (string.IsNullOrWhiteSpace(SearchToDate))
            {
                endDate = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = SearchToDate;
            }
            DateTime fromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime toDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);


            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = new List<GuestCompanyBO>();

            GridViewDataNPaging<GuestCompanyBO, GridPaging> myGridData = new GridViewDataNPaging<GuestCompanyBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            // // // ------User Admin Authorization BO Session Information --------------------------------
            int isAdminUser = 0;

            // // // -----------IsHotelGuestCompanyRestrictionForAllUsers
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsHotelGuestCompanyRestrictionForAllUsers", "IsHotelGuestCompanyRestrictionForAllUsers");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                if (setUpBO.SetupValue == "1")
                {
                    isAdminUser = 1;
                }
                else
                {
                    AccountManagerDA accountManagerDA = new AccountManagerDA();
                    List<AccountManagerBO> accountManagerBOList = new List<AccountManagerBO>();

                    #region User Admin Authorization
                    if (userInformationBO.UserInfoId == 1)
                    {
                        isAdminUser = 1;
                    }
                    else
                    {
                        List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                        adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                        if (adminAuthorizationList != null)
                        {
                            if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 22).Count() > 0)
                            {
                                isAdminUser = 1;
                            }
                        }
                    }

                    if (companyOwnerId == 0)
                    {
                        isAdminUser = 1;
                    }
                    #endregion
                }
            }

            files = guestCompanyDA.GetGuestCompanyInfoBySearchCriteria(isAdminUser, userInformationBO.UserInfoId, companyName, companyType, contactNumber, companyEmail, countryId, stateId, cityId, areaId, lifeCycleStage, companyOwnerId, dateSearchCriteria, fromDate, toDate, companyNumber, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            if (isAdminUser == 1)
            {
                foreach (GuestCompanyBO row in files)
                {
                    row.IsDetailPanelEnableForCompany = 1;
                    row.IsDetailPanelEnableForParentCompany = 1;
                }
            }

            myGridData.GridPagingProcessing(files, totalRecords);
            return myGridData;

        }
        [WebMethod]
        public static ReturnInfo DeleteCompany(long Id)
        {
            ReturnInfo rtninf = new ReturnInfo();

            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            bool status = hmCommonDA.DeleteInfoById("HotelGuestCompany", "CompanyId", Id);
            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.SalaryHead.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalaryHead));
            }
            else
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Warning);
            }
            return rtninf;
        }
        private void LoadGridView(int pageNumber, int isCurrentOrPreviousPage, int gridRecordsCount)
        {
            //if (ddlCriteria.SelectedValue == "0")
            //{
            //    CommonHelper.AlertInfo(innboardMessage, "Please Provide Criteria (Date).", AlertType.Warning);
            //}
            //else
            //{
            int totalRecords = 0;

            CheckObjectPermission();
            IsHotelGuestCompanyRescitionForAllUsers();

            string companyName = txtSCompanyName.Text;
            int companyType = Convert.ToInt32(ddlSrcCompanyType.SelectedValue);
            string contactNumber = txtSContactNumber.Text;
            string companyEmail = txtSCompanyEmail.Text;

            Int64 countryId = 0;
            Int64 stateId = 0;
            Int64 cityId = 0;
            Int64 areaId = 0;

            if (!string.IsNullOrWhiteSpace(txtBillingCountry.Text))
            {
                countryId = Convert.ToInt32(hfBillingCountryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(txtBillingState.Text))
            {
                stateId = Convert.ToInt32(hfBillingStateId.Value);
            }

            if (!string.IsNullOrWhiteSpace(txtBillingCity.Text))
            {
                cityId = Convert.ToInt32(hfBillingCityId.Value);
            }

            if (!string.IsNullOrWhiteSpace(txtBillingLocation.Text))
            {
                areaId = Convert.ToInt32(hfBillingLocationId.Value);
            }

            int lifeCycleStage = Convert.ToInt32(ddlSrcLifeCycleStage.SelectedValue);
            int companyOwnerId = Convert.ToInt32(ddlSrcOwnerId.SelectedValue);
            int dateSearchCriteria = Convert.ToInt32(ddlCriteria.SelectedValue);

            DateTime dateTime = DateTime.Now;
            string startDate = string.Empty;
            string endDate = string.Empty;
            if (string.IsNullOrWhiteSpace(this.txtSearchFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtSearchFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = this.txtSearchFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtSearchToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtSearchToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = this.txtSearchToDate.Text;
            }
            DateTime fromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime toDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            string CompanyNumber = txtCompanyNumber.Text;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = new List<GuestCompanyBO>();

            GridViewDataNPaging<GuestCompanyBO, GridPaging> myGridData = new GridViewDataNPaging<GuestCompanyBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            // // // ------User Admin Authorization BO Session Information --------------------------------
            int isAdminUser = 0;
            AccountManagerDA accountManagerDA = new AccountManagerDA();
            List<AccountManagerBO> accountManagerBOList = new List<AccountManagerBO>();

            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                isAdminUser = 1;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 22).Count() > 0)
                    {
                        isAdminUser = 1;
                    }
                }
            }
            #endregion


            files = guestCompanyDA.GetGuestCompanyInfoBySearchCriteria(isAdminUser, userInformationBO.UserInfoId, companyName, companyType, contactNumber, companyEmail, countryId, stateId, cityId, areaId, lifeCycleStage, companyOwnerId, dateSearchCriteria, fromDate, toDate, CompanyNumber, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(files, totalRecords, "GridPagingForSearchCompany");

            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                hfIsHotelGuestCompanyRescitionForAllUsers.Value = "0";
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 22).Count() > 0)
                    {
                        hfIsHotelGuestCompanyRescitionForAllUsers.Value = "0";
                    }
                }
            }
            #endregion

            if (files.Count > 0)
                //gridPaging.Text = myGridData.GridPageLinks.PreviousButton + myGridData.GridPageLinks.Pagination + myGridData.GridPageLinks.NextButton;

                gvGuestCompany.DataSource = files;
            gvGuestCompany.DataBind();
            SetTab("SearchTab");
            //}
        }
        private void IsHotelGuestCompanyRescitionForAllUsers()
        {
            hfIsHotelGuestCompanyRescitionForAllUsers.Value = "0";
            HMCommonSetupBO commonSetupBORequisition = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            commonSetupBORequisition = commonSetupDA.GetCommonConfigurationInfo("IsHotelGuestCompanyRescitionForAllUsers", "IsHotelGuestCompanyRescitionForAllUsers");

            if (commonSetupBORequisition != null)
            {
                if (commonSetupBORequisition.SetupId > 0)
                {
                    if (commonSetupBORequisition.SetupValue == "1")
                    {
                        hfIsHotelGuestCompanyRescitionForAllUsers.Value = "1";
                    }
                }
            }

        }
        private void Cancel()
        {
            txtCompanyName.Text = string.Empty;
            txtCompanyAddress.Text = string.Empty;
            txtEmailAddress.Text = string.Empty;
            txtWebAddress.Text = string.Empty;
            txtContactNumber.Text = string.Empty;

            txtPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtWebsite.Text = string.Empty;
            txtFax.Text = string.Empty;

            txtContactDesignation.Text = string.Empty;
            txtTelephoneNumber.Text = string.Empty;
            txtContactPerson.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            txtCompanyId.Value = string.Empty;
            txtDiscountPercent.Text = "0";
            txtCreditLimit.Text = "0";
            btnSave.Text = "Save";
            txtCompanyName.Focus();
            ddlSignupStatus.SelectedIndex = 0;
            ddlReferenceId.SelectedIndex = 0;
            ddlIndustryId.SelectedIndex = 0;
            txtNumberOfEmployee.Text = string.Empty;
            txtAnnualRevenue.Text = string.Empty;
            hfCompanyId.Value = string.Empty;
            hfGuestDeletedDoc.Value = string.Empty;

            //ddlCompanyOwner.SelectedValue = "0";
            ddlParentCompany.SelectedValue = "0";
            ddlCompanyType.SelectedValue = "0";
            ddlOwnership.SelectedValue = "0";
            ddlLifeCycleStageId.SelectedValue = "0";
            ddlIndustry.SelectedValue = "0";

            txtBillingStreet.Text = string.Empty;
            txtBillingPostCode.Text = string.Empty;
            ddlBillingCityId.SelectedValue = "0";
            ddlBillingStateId.SelectedValue = "0";
            ddlBillingCountryId.SelectedValue = "19";

            txtShippingStreet.Text = string.Empty;
            txtShippingPostCode.Text = string.Empty;
            txtTicketNo.Text = string.Empty;
            ddlShippingCityId.SelectedValue = "0";
            ddlShippingStateId.SelectedValue = "0";
            ddlShippingCountryId.SelectedValue = "19";
            LoadAccountManager();
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
            GuestCompanyBO guestCompanyBO = new GuestCompanyBO();
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();

            guestCompanyBO = guestCompanyDA.GetGuestCompanyInfoById(EditId);
            LoadRandonNumber();

            FileUpload();
            hfCompanyId.Value = guestCompanyBO.CompanyId.ToString();
            txtCompanyName.Text = guestCompanyBO.CompanyName;
            txtCompanyAddress.Text = guestCompanyBO.CompanyAddress;
            txtEmail.Text = guestCompanyBO.EmailAddress;
            txtWebsite.Text = guestCompanyBO.WebAddress;
            txtPhone.Text = guestCompanyBO.ContactNumber;
            txtFax.Text = guestCompanyBO.Fax;
            txtContactDesignation.Text = guestCompanyBO.ContactDesignation;
            txtTelephoneNumber.Text = guestCompanyBO.TelephoneNumber;
            txtContactPerson.Text = guestCompanyBO.ContactPerson;
            txtRemarks.Text = guestCompanyBO.Remarks;
            txtDiscountPercent.Text = guestCompanyBO.DiscountPercent.ToString();
            txtCreditLimit.Text = guestCompanyBO.CreditLimit.ToString();
            txtCompanyId.Value = guestCompanyBO.CompanyId.ToString();
            txtNodeId.Value = guestCompanyBO.NodeId.ToString();
            if (guestCompanyBO.SignupStatusId != null && guestCompanyBO.SignupStatusId != 0)
                ddlSignupStatus.SelectedValue = guestCompanyBO.SignupStatusId.ToString();
            ddlLocation.SelectedValue = guestCompanyBO.LocationId.ToString();
            ddlIndustry.SelectedValue = guestCompanyBO.IndustryId.ToString();
            if (guestCompanyBO.NumberOfEmployee != null)
                txtNumberOfEmployee.Text = guestCompanyBO.NumberOfEmployee.ToString();
            if (guestCompanyBO.AnnualRevenue != null)
                txtAnnualRevenue.Text = guestCompanyBO.AnnualRevenue.ToString();
            txtTicketNo.Text = guestCompanyBO.TicketNumber.ToString();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (guestCompanyBO.CompanyOwnerId == 0 || guestCompanyBO.CompanyOwnerId == 1)
            {
                ddlCompanyOwner.SelectedValue = "0";
            }
            else
            {
                ddlCompanyOwner.SelectedValue = guestCompanyBO.CompanyOwnerId.ToString();
            }
            if (guestCompanyBO.AncestorId < 0)
            {
                ddlParentCompany.SelectedValue = "0";
            }
            else if (guestCompanyBO.AncestorId == guestCompanyBO.CompanyId)
            {
                ddlParentCompany.SelectedValue = "0";
            }
            else
            {
                ddlParentCompany.SelectedValue = guestCompanyBO.AncestorId.ToString();
            }
            ddlCompanyType.SelectedValue = guestCompanyBO.CompanyType.ToString();
            ddlOwnership.SelectedValue = guestCompanyBO.OwnershipId.ToString();
            ddlLifeCycleStageId.SelectedValue = guestCompanyBO.LifeCycleStageId.ToString();
            txtNoOfEmployee.Text = guestCompanyBO.NumberOfEmployee.ToString();

            txtBillingStreet.Text = guestCompanyBO.BillingStreet.ToString();
            txtBillingPostCode.Text = guestCompanyBO.BillingPostCode.ToString();
            ddlBillingCityId.SelectedValue = guestCompanyBO.BillingCityId.ToString();
            ddlBillingStateId.SelectedValue = guestCompanyBO.BillingStateId.ToString();
            if (guestCompanyBO.BillingCountryId == 0)
            {
                ddlBillingCountryId.SelectedValue = "19";
            }
            else
            {
                ddlBillingCountryId.SelectedValue = guestCompanyBO.BillingCountryId.ToString();

            }
            txtShippingStreet.Text = guestCompanyBO.ShippingStreet.ToString();
            txtShippingPostCode.Text = guestCompanyBO.ShippingPostCode.ToString();
            ddlShippingCityId.SelectedValue = guestCompanyBO.ShippingCityId.ToString();
            ddlShippingStateId.SelectedValue = guestCompanyBO.ShippingStateId.ToString();
            if (guestCompanyBO.ShippingCountryId == 0)
            {
                ddlShippingCountryId.SelectedValue = "19";
            }
            else
            {
                ddlShippingCountryId.SelectedValue = guestCompanyBO.ShippingCountryId.ToString();

            }
        }
        public static string CreateDivForEmails(HotelCompanyContactDetailsBO detailsBO)
        {
            string div = string.Empty;
            div += "<div>" +
                " < div class='col-md-10' style='padding: 0;'> " +
                " <input style = 'margin-top: 5px' class='form-control' id='txtaddedEmail' name='DynamicEmail' type='text' value='" + detailsBO.FieldValue + "'>" +
                " </div>" +
                "<div class='col-md-1'>" +
                " <input class='img-rounded' type='image' src='../Images/delete.png' style='padding-top:5px;margin-left: 5px; position:center; margin-top:5px;' onclick='RemoveEmailTextBox(this)'> " +
                "</div>" +
                "</div>";

            return div;
        }
        private void LoadAccountManager()
        {
            int isAdminUser = 0;
            ddlSrcOwnerId.Enabled = true;
            ddlCompanyOwner.Enabled = true;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            AccountManagerDA accountManagerDA = new AccountManagerDA();
            List<AccountManagerBO> accountManagerBOList = new List<AccountManagerBO>();

            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                isAdminUser = 1;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 22).Count() > 0)
                    {
                        isAdminUser = 1;
                    }
                }
            }
            #endregion

            accountManagerBOList = accountManagerDA.GetAccountManager(isAdminUser, "CRM", userInformationBO.UserInfoId);

            ddlSrcOwnerId.DataSource = accountManagerBOList;
            ddlSrcOwnerId.DataTextField = "DisplayName";
            ddlSrcOwnerId.DataValueField = "UserInfoId";
            ddlSrcOwnerId.DataBind();

            if (accountManagerBOList != null)
            {
                if (accountManagerBOList.Count > 0)
                {
                    btnAdd.Visible = true;
                }
                else
                {
                    btnAdd.Visible = false;
                }
            }

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSrcOwnerId.Items.Insert(0, item);
            ddlCompanyOwner.Items.Insert(0, item);
        }
        private void LoadIndustry()
        {
            IndustryDA industryDA = new IndustryDA();
            List<IndustryBO> industryBO = new List<IndustryBO>();
            industryBO = industryDA.GetIndustryInfo();

            ddlIndustry.DataSource = industryBO;
            ddlIndustry.DataTextField = "IndustryName";
            ddlIndustry.DataValueField = "IndustryId";
            ddlIndustry.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlIndustry.Items.Insert(0, item);
        }
        private void LoadCompanyType()
        {
            SalesMarketingDA salesMarketingDA = new SalesMarketingDA();
            List<SMCompanyTypeInformationBO> typeBO = new List<SMCompanyTypeInformationBO>();
            typeBO = salesMarketingDA.GetCompanyTypeForDDL();

            ddlCompanyType.DataSource = typeBO;
            ddlCompanyType.DataTextField = "TypeName";
            ddlCompanyType.DataValueField = "Id";
            ddlCompanyType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCompanyType.Items.Insert(0, item);

            ddlSrcCompanyType.DataSource = typeBO;
            ddlSrcCompanyType.DataTextField = "TypeName";
            ddlSrcCompanyType.DataValueField = "Id";
            ddlSrcCompanyType.DataBind();

            ListItem itemType = new ListItem();
            itemType.Value = "0";
            itemType.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSrcCompanyType.Items.Insert(0, itemType);
        }
        private void LoadParentCompany()
        {
            List<GuestCompanyBO> companyBOs = new List<GuestCompanyBO>();
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();

            companyBOs = guestCompanyDA.GetGuestCompanyInfo();

            ddlParentCompany.DataSource = companyBOs;
            ddlParentCompany.DataTextField = "CompanyName";
            ddlParentCompany.DataValueField = "CompanyId";
            ddlParentCompany.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlParentCompany.Items.Insert(0, item);
        }
        private void LoadOwnership()
        {
            SalesMarketingDA salesMarketingDA = new SalesMarketingDA();
            List<SMOwnershipInformationBO> typeBO = new List<SMOwnershipInformationBO>();
            typeBO = salesMarketingDA.GetOwnershipInfoForDDL();

            ddlOwnership.DataSource = typeBO;
            ddlOwnership.DataTextField = "OwnershipName";
            ddlOwnership.DataValueField = "Id";
            ddlOwnership.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlOwnership.Items.Insert(0, item);
        }
        private void LoadLifeCycleStage()
        {
            LifeCycleStageDA lifeCycleStageDA = new LifeCycleStageDA();
            List<SMLifeCycleStageBO> sMLifeCycles = new List<SMLifeCycleStageBO>();
            sMLifeCycles = lifeCycleStageDA.GetLifeCycleForDdl();

            ddlLifeCycleStageId.DataSource = sMLifeCycles;
            ddlLifeCycleStageId.DataTextField = "LifeCycleStage";
            ddlLifeCycleStageId.DataValueField = "Id";
            ddlLifeCycleStageId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlLifeCycleStageId.Items.Insert(0, item);

            ddlSrcLifeCycleStage.DataSource = sMLifeCycles;
            ddlSrcLifeCycleStage.DataTextField = "LifeCycleStage";
            ddlSrcLifeCycleStage.DataValueField = "Id";
            ddlSrcLifeCycleStage.DataBind();

            ListItem itemStage = new ListItem();
            itemStage.Value = "0";
            itemStage.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSrcLifeCycleStage.Items.Insert(0, itemStage);
        }
        private void LoadCountry()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CountriesBO> countryList = commonDA.GetAllCountries();
            ddlBillingCountryId.DataSource = countryList;
            ddlBillingCountryId.DataTextField = "CountryName";
            ddlBillingCountryId.DataValueField = "CountryId";
            ddlBillingCountryId.DataBind();

            ddlShippingCountryId.DataSource = countryList;
            ddlShippingCountryId.DataTextField = "CountryName";
            ddlShippingCountryId.DataValueField = "CountryId";
            ddlShippingCountryId.DataBind();

            string bangladesh = "19";

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO commonCountrySetupBO = new HMCommonSetupBO();
            commonCountrySetupBO = commonSetupDA.GetCommonConfigurationInfo("CompanyCountryId", "CompanyCountryId");

            if (commonCountrySetupBO != null)
            {
                bangladesh = commonCountrySetupBO.SetupValue;
            }

            ddlBillingCountryId.SelectedValue = bangladesh;
            ddlShippingCountryId.SelectedValue = bangladesh;
        }

        private void LoadSignupStatus()
        {
            CompanySignupStatusDA statusDA = new CompanySignupStatusDA();

            List<SMCompanySignupStatus> statusList = statusDA.GetAllSignupStatus();
            ddlSignupStatus.DataSource = statusList;
            ddlSignupStatus.DataTextField = "Status";
            ddlSignupStatus.DataValueField = "Id";
            ddlSignupStatus.DataBind();

        }
        //************************ User Defined Function ********************//
        [WebMethod]
        public static string GetUploadedImageByWebMethod(int OwnerId, string docType)
        {
            string strTable = "";
            DocumentsDA docDA = new DocumentsDA();
            var docList = docDA.GetDocumentsInfoByDocCategoryAndOwnerId(docType, OwnerId);
            if (docList.Count > 0)
            {
                var Image = docList[0];
                strTable += "<img src='" + Image.Path + Image.Name + "'  alt='No Image Selected' border='0' style='height:150px;' />";
            }
            return strTable;

        }
        [WebMethod]
        public static string GetDocumentsByUserTypeAndUserId(string companyId)
        {

            List<DocumentsBO> docList = new List<DocumentsBO>();
            DocumentsDA docDA = new DocumentsDA();
            docList = docDA.GetDocumentsByUserTypeAndUserId("CompanyDocument", Int32.Parse(companyId));
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            string strTable = "";
            strTable += "<div style='color: White; background-color: #44545E;width:750px;'>";
            int counter = 0;
            foreach (DocumentsBO dr in docList)
            {
                if (dr.Extention == ".jpg")
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:250px; height:250px; float:left;padding:30px'>";
                    strTable += "<a target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
                else
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:100px; height:100px; float:left;padding:30px'>";
                    strTable += "<a target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img style='width: 100px; height: 100px;' src='" + dr.IconImage + "' alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
            }
            strTable += "</div>";
            if (strTable == "")
            {
                strTable = "<tr><td align='center'>No Record Available!</td></tr>";
            }
            return strTable;

        }
        [WebMethod]
        public static List<DocumentsBO> LoadCompanyDocument(long id, int randomId, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }

            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("CompanyDocument", randomId);

            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("CompanyDocument", id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }
        [WebMethod]
        public static ReturnInfo DeleteCompanyDocument(long documentId)
        {
            ReturnInfo info = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            List<DocumentsBO> docList = new List<DocumentsBO>();

            info.IsSuccess = new DocumentsDA().DeleteDocumentsByDocumentId(documentId);

            if (info.IsSuccess)
            {
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.SMDeal.ToString(), documentId, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
            }
            else
            {
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }
        [WebMethod]
        public static List<GuestCompanyBO> GetCompanyByAutoSearch(string searchTerm)
        {
            List<GuestCompanyBO> companyInfo = new List<GuestCompanyBO>();
            GuestCompanyDA itemDa = new GuestCompanyDA();
            companyInfo = itemDa.GetGuestCompanyInfoByCompanyName(searchTerm);

            return companyInfo;
        }

        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            Response.Redirect("/SalesAndMarketing/frmGuestCompany.aspx");
        }
    }
}