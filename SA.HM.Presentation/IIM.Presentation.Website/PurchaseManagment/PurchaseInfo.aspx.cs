using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;
using System.Net.Mail;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class PurchaseInfo : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadGLCompany();
                LoadAllCostCentreInfo();
                LoadSupplierInfo();
                CheckPermission();
                LoadDescriptionConfiguratipn();
                IsAdminUser();
            }
        }
        private void LoadGLCompany()
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            List<GLCompanyBO> GLCompanyBOList = new List<GLCompanyBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.UserInfoId == 1)
            {
                GLCompanyBOList = entityDA.GetAllGLCompanyInfo();
            }
            else
            {
                GLCompanyBOList = entityDA.GetAllGLCompanyInfoByUserGroupId(userInformationBO.UserGroupId);
            }

            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            if (GLCompanyBOList != null)
            {
                if (GLCompanyBOList.Count == 1)
                {
                    hfIsSingleGLCompany.Value = "1";
                    companyList.Add(GLCompanyBOList[0]);
                    this.ddlGLCompanyId.DataSource = companyList;
                    this.ddlGLCompanyId.DataTextField = "Name";
                    this.ddlGLCompanyId.DataValueField = "CompanyId";
                    this.ddlGLCompanyId.DataBind();
                }
                else
                {
                    hfIsSingleGLCompany.Value = "2";
                    this.ddlGLCompanyId.DataSource = GLCompanyBOList;
                    this.ddlGLCompanyId.DataTextField = "Name";
                    this.ddlGLCompanyId.DataValueField = "CompanyId";
                    this.ddlGLCompanyId.DataBind();
                    ListItem itemCompany = new ListItem();
                    itemCompany.Value = "0";
                    itemCompany.Text = hmUtility.GetDropDownFirstAllValue();
                    this.ddlGLCompanyId.Items.Insert(0, itemCompany);
                }
            }
        }
        private void LoadAllCostCentreInfo()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfo();
            costCentreTabBOList = costCentreTabBOList.Where(o => o.OutletType == 2).ToList();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();

            ddlCostCenterSearch.DataSource = costCentreTabBOList;
            ddlCostCenterSearch.DataTextField = "CostCenter";
            ddlCostCenterSearch.DataValueField = "CostCenterId";
            ddlCostCenterSearch.DataBind();
            ddlCostCenterSearch.Items.Insert(0, item);
        }

        private void LoadSupplierInfo()
        {
            PMSupplierDA entityDA = new PMSupplierDA();
            List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();

            supplierBOList = entityDA.GetPMSupplierInfo();

            ddlSupplier.DataSource = supplierBOList;
            ddlSupplier.DataTextField = "Name";
            ddlSupplier.DataValueField = "SupplierId";
            ddlSupplier.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSupplier.Items.Insert(0, item);
        }
        private void LoadDescriptionConfiguratipn()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            List<HMCommonSetupBO> setUpBOList = new List<HMCommonSetupBO>();
            setUpBOList = commonSetupDA.GetAllCommonConfigurationInfo();

            setUpBO = setUpBOList.Where(x => x.TypeName == "IsItemDescriptionSuggestInPurchaseOrder" && x.SetupName == "IsItemDescriptionSuggestInPurchaseOrder").FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                hfIsItemDescriptionSuggestInPurchaseOrder.Value = setUpBO.SetupValue.ToString();
            }
        }
        private void IsAdminUser()
        {
            Boolean IsAdminUser = false;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                IsAdminUser = true;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 12).Count() > 0)
                    {
                        IsAdminUser = true;
                    }
                }
            }
            #endregion

            if (IsAdminUser)
            {
                hfIsAdminUser.Value = "1";
            }
            else
            {
                hfIsAdminUser.Value = "0";
            }
        }
        [WebMethod]
        public static GridViewDataNPaging<PMPurchaseOrderBO, GridPaging> SearchPurchaseOrder(int companyId, string orderType, string poType, DateTime? fromDate, DateTime? toDate,
                                                                                          string poNumber, string requisitionNumber, string status, int? costCenterId, int? supplierId,
                                                                                          int gridRecordsCount, int pageNumber,
                                                                                          int isCurrentOrPreviousPage
                                                                                         )
        {

            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<PMPurchaseOrderBO, GridPaging> myGridData = new GridViewDataNPaging<PMPurchaseOrderBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            PMPurchaseOrderDA detalisDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();
            orderList = detalisDA.GetPMPurchaseOrderInfoBySearchCriteria(companyId, orderType, poType, fromDate, toDate, poNumber, requisitionNumber, status, costCenterId, supplierId, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(orderList, totalRecords);
            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo PurchaseOrderApproval(string poType, int pOrderId, string approvedStatus)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PurchaseOrderViewBO viewBo = new PurchaseOrderViewBO();
            PMPurchaseOrderDA orderDetailDA = new PMPurchaseOrderDA();
            string TransactionNo = "";
            string TransactionType = "";
            string ApproveStatus = "";

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDetailDA.PurchaseOrderApproval(poType, pOrderId, approvedStatus, userInformationBO.UserInfoId, out TransactionNo, out TransactionType, out ApproveStatus);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;

                    rtninf.PrimaryKeyValue = pOrderId.ToString();
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

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ProductPurchaseOrder.ToString(), pOrderId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductPurchaseOrder));

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
        public static ReturnInfo PurchaseOrderDelete(string pOType, int pOrderId, string approvedStatus, int createdBy)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PurchaseOrderViewBO viewBo = new PurchaseOrderViewBO();
            PMPurchaseOrderDA orderDetailDA = new PMPurchaseOrderDA();
            string TransactionNo = "";
            string TransactionType = "";
            string ApproveStatus = "";

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDetailDA.PurchaseOrderDelete(pOType, pOrderId, approvedStatus, createdBy, userInformationBO.UserInfoId, out TransactionNo, out TransactionType, out ApproveStatus);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    rtninf.PrimaryKeyValue = pOrderId.ToString();
                    rtninf.TransactionNo = TransactionNo;
                    rtninf.TransactionType = TransactionType;
                    rtninf.TransactionStatus = ApproveStatus;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.ProductPurchaseOrder.ToString(), pOrderId,
                       ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductPurchaseOrder));

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
        public static ReturnInfo ReOpenPurchaseOrder(int pOrderId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMPurchaseOrderDA orderDetailDA = new PMPurchaseOrderDA();
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDetailDA.ReOpenPurchaseOrder(pOrderId);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Success, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ProductPurchaseOrder.ToString(), pOrderId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductPurchaseOrder));

                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;

        }
        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }

    }
}