using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using System.Collections;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.SalesManagment;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Newtonsoft.Json;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity;
using HotelManagement.Data;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.UserInformation;
using HotelManagement.Data.SalesAndMarketing;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmItemConsumption : BasePage
    {
        protected bool isSingle = true;
        HiddenField innboardMessage;
        protected int _RestaurantComboId;
        protected int IsService = -1;
        protected int btnPadding = -1;
        HMUtility hmUtility = new HMUtility();
        List<UserConfigurationBO> userConfiguredList;
        public frmItemConsumption() : base("ItemConsumptionInformation")
        {

        }
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                IsItemConsumptionDeliveryChallanEnable();
                companyProjectUserControl.ddlFirstValueVar = "select";
                isSerialAutoLoad();
                LoadCurrentDate();
                Session["PMProductOut"] = null;
                LoadCostCenter();
                LoadStockBy();
                LoadCategory();
                LoadProduct();
                LoadCommonDropDownHiddenField();
                LoadExpenditureEquivalantHead();
                IsProductOutApprovalEnable();
                LoadEmployee();
                IsAttributeItemShow();
                IsAverageCostEnableInItemConsumption();
            }
            IsInventoryIntegrateWithAccounts();
            CheckObjectPermission();
        }
        private void IsAttributeItemShow()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsItemAttributeEnable", "IsItemAttributeEnable");
            if (homePageSetupBO != null)
            {
                if (homePageSetupBO.SetupId > 0)
                {
                    hfIsItemAttributeEnable.Value = homePageSetupBO.SetupValue;
                }
            }
        }
        private void IsAverageCostEnableInItemConsumption()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsAverageCostEnableInItemConsumption", "IsAverageCostEnableInItemConsumption");
            if (homePageSetupBO != null)
            {
                if (homePageSetupBO.SetupId > 0)
                {
                    hfIsAverageCostEnableInItemConsumption.Value = homePageSetupBO.SetupValue;
                }
            }
        }
        private void CheckObjectPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }
        //private void LoadGLCompany()
        //{
        //    GLCompanyDA entityDA = new GLCompanyDA();
        //    List<GLCompanyBO> companyList = new List<GLCompanyBO>();
        //    companyList = entityDA.GetAllGLCompanyInfo();

        //    ddlCompany.DataSource = companyList;
        //    ddlCompany.DataTextField = "Name";
        //    ddlCompany.DataValueField = "CompanyId";
        //    ddlCompany.DataBind();            

        //    if (companyList.Count == 1)
        //    {
        //        LoadGLProjectByCompany(companyList[0].CompanyId);
        //    }
        //    else
        //    {
        //        ListItem itemCompany = new ListItem();
        //        itemCompany.Value = "0";
        //        itemCompany.Text = hmUtility.GetDropDownFirstValue();

        //        ddlCompany.Items.Insert(0, itemCompany);
        //    }

        //}
        //private void LoadDefaultGLCompanyNProject()
        //{
        //    GLCompanyDA entityDA = new GLCompanyDA();
        //    List<GLCompanyBO> companyList = new List<GLCompanyBO>();
        //    companyList.Add(entityDA.GetAllGLCompanyInfo().FirstOrDefault());

        //    ddlCompany.DataSource = companyList;
        //    ddlCompany.DataTextField = "Name";
        //    ddlCompany.DataValueField = "CompanyId";
        //    ddlCompany.DataBind();

        //    if (companyList.Count > 0)
        //    {
        //        GLProjectDA projectDA = new GLProjectDA();
        //        List<GLProjectBO> projectList = new List<GLProjectBO>();
        //        projectList.Add(projectDA.GetGLProjectInfoByGLCompany(companyList[0].CompanyId).FirstOrDefault());

        //        ddlGLProject.DataSource = projectList;
        //        ddlGLProject.DataTextField = "Name";
        //        ddlGLProject.DataValueField = "ProjectId";
        //        ddlGLProject.DataBind();
        //    }

        //}
        //private void LoadGLProjectByCompany(int companyId)
        //{
        //    GLProjectDA entityDA = new GLProjectDA();
        //    List<GLProjectBO> projectList = new List<GLProjectBO>();
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
        //    var List = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(Convert.ToInt32(companyId), Convert.ToInt32(userInformationBO.UserGroupId)).Where(x => x.IsFinalStage == false).ToList();

        //    if (List.Count == 0)
        //        List.Add(entityDA.GetGLProjectInfoByGLCompany(companyId).FirstOrDefault());
        //    ddlGLProject.DataSource = List;
        //    ddlGLProject.DataTextField = "Name";
        //    ddlGLProject.DataValueField = "ProjectId";
        //    ddlGLProject.DataBind();

        //    if (List.Count > 1)
        //    {
        //        isSingle = false;
        //        ListItem itemProject = new ListItem();
        //        itemProject.Value = "0";
        //        itemProject.Text = hmUtility.GetDropDownFirstValue();
        //        ddlGLProject.Items.Insert(0, itemProject);
        //    }
        //}
        private void isSerialAutoLoad()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO IsSeriaFillBO = new HMCommonSetupBO();
            IsSeriaFillBO = commonSetupDA.GetCommonConfigurationInfo("IsItemSerialFillWithAutoSearch", "IsItemSerialFillWithAutoSearch");
            hfIsItemSerialFillWithAutoSearch.Value = IsSeriaFillBO.SetupValue;

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                return;
            }

            PMProductOutDA outDA = new PMProductOutDA();
            Boolean status = true;//outDA.SaveProductOutInfo(Session["PMProductOut"] as List<PMProductOutBO>);
            if (status == true)
            {
                CommonHelper.AlertInfo("Product Out " + AlertMessage.Success, AlertType.Success);
                ClearForm();
            }
        }

        //************************ User Defined Function ********************//
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        }
        public void LoadCostCenter()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId)
                .Where(a => a.CostCenterType == "Inventory").ToList(); ;

            ddlCostCenter.DataSource = List;
            ddlCostCenter.DataTextField = "CostCenter";
            ddlCostCenter.DataValueField = "CostCenterId";
            ddlCostCenter.DataBind();

            //ddlOutForLocation.DataSource = List;
            //ddlOutForLocation.DataTextField = "DefaultStockLocationId";
            //ddlOutForLocation.DataValueField = "CostCenterId";
            //ddlOutForLocation.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            if (List.Count > 0)
            {
                ddlCostCenter.Items.Insert(0, item);
                ddlOutForLocation.Items.Insert(0, item);
            }
            //ddlSearchCostCenter.DataSource = List;
            //ddlSearchCostCenter.DataTextField = "CostCenter";
            //ddlSearchCostCenter.DataValueField = "CostCenterId";
            //ddlSearchCostCenter.DataBind();
            //ddlSearchCostCenter.Items.Insert(0, item);
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetInvItemCatagoryInfoByServiceType("Product");
            ddlCategory.DataSource = List;
            ddlCategory.DataTextField = "MatrixInfo";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCategory.Items.Insert(0, item);
        }
        private void LoadStockBy()
        {
            List<InvUnitHeadBO> headListBO = new List<InvUnitHeadBO>();
            InvUnitHeadDA da = new InvUnitHeadDA();
            headListBO = da.GetInvUnitHeadInfo();

            ddlStockBy.DataSource = headListBO;
            ddlStockBy.DataTextField = "HeadName";
            ddlStockBy.DataValueField = "UnitHeadId";
            ddlStockBy.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlStockBy.Items.Insert(0, item);
        }
        private void LoadProduct()
        {
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlProduct.Items.Insert(0, item);
        }

        private void LoadEmployee()
        {
            EmployeeDA empDa = new EmployeeDA();
            var employee = empDa.GetEmployeeInfo();

            ddlOutFor.DataSource = employee;
            ddlOutFor.DataTextField = "DisplayName";
            ddlOutFor.DataValueField = "EmpId";
            ddlOutFor.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlOutFor.Items.Insert(0, item);
        }
        private bool IsFormValid()
        {
            bool status = true;
            List<PMProductOutBO> detailListBO = Session["PMProductOut"] == null ? new List<PMProductOutBO>() : Session["PMProductOut"] as List<PMProductOutBO>;

            if (detailListBO == null)
            {
                status = false;
                CommonHelper.AlertInfo("Please Add Product Detail Information.", AlertType.Warning);
            }
            else if (detailListBO.Count == 0)
            {
                status = false;
                CommonHelper.AlertInfo("Please Add Product Detail Information.", AlertType.Warning);
            }

            //else if (ddlBillNumber.SelectedValue == "0")
            //{
            //    if (ddlOutFor.SelectedValue == "0")
            //    {
            //        status = false;
            //        isMessageBoxEnable = 1;
            //        lblMessage.Text = "Please Provide Out For Information";
            //        ddlOutFor.Focus();
            //    }
            //}

            //if (string.IsNullOrWhiteSpace(txtRemarks.Text))
            //{
            //    if (ddlBillNumber.SelectedValue == "0")
            //    {
            //        status = false;
            //        isMessageBoxEnable = 1;
            //        lblMessage.Text = "Please Provide Location Information";
            //        ddlBillNumber.Focus();
            //    }
            //    else
            //    {
            //        status = false;
            //        isMessageBoxEnable = 1;
            //        lblMessage.Text = "Please Provide Remarks Information";
            //        ddlBillNumber.Focus();
            //    }
            //}


            return status;
        }
        private void ClearForm()
        {
            Session["PMProductOut"] = null;
            Session["arrayDelete"] = null;
        }
        private void IsItemConsumptionDeliveryChallanEnable()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            hfIsItemConsumptionDeliveryChallanEnable.Value = "0";
            HMCommonSetupBO isItemConsumptionDeliveryChallanEnableBO = new HMCommonSetupBO();
            isItemConsumptionDeliveryChallanEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsItemConsumptionDeliveryChallanEnable", "IsItemConsumptionDeliveryChallanEnable");
            if (isItemConsumptionDeliveryChallanEnableBO != null)
            {
                if (isItemConsumptionDeliveryChallanEnableBO.SetupId > 0)
                {
                    if (isItemConsumptionDeliveryChallanEnableBO.SetupValue == "1")
                    {
                        hfIsItemConsumptionDeliveryChallanEnable.Value = "1";
                    }
                }
            }
        }
        private int ValidSerialNumber()
        {
            int tmpSerialId = 0;
            PMProductSerialInfoDA entityDA = new PMProductSerialInfoDA();
            PMProductSerialInfoBO entityBO = new PMProductSerialInfoBO();

            //entityBO = entityDA.GetPMProductSerialInfoBySerialNumberForSale(Convert.ToInt32(ddlProductId.SelectedValue), txtQuantity_Serial.Text);
            if (entityBO != null)
            {
                tmpSerialId = entityBO.SerialId;
            }
            return tmpSerialId;
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadExpenditureEquivalantHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List<NodeMatrixBO> entityBOList = new List<NodeMatrixBO>();
            entityBOList = entityDA.GetNodeMatrixInfoByAncestorNodeIdList("4").Where(x => x.IsTransactionalHead == true).ToList();

            ddlAccountExpenseHead.DataSource = entityBOList;
            ddlAccountExpenseHead.DataTextField = "HeadWithCode";
            ddlAccountExpenseHead.DataValueField = "NodeId";
            ddlAccountExpenseHead.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            ddlAccountExpenseHead.Items.Insert(0, itemBank);
        }

        private void IsInventoryIntegrateWithAccounts()
        {
            hfIsInventoryIntegrateWithAccounts.Value = "0";
            hfIsInventoryIntegratationWithAccountsAutomated.Value = "0";

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isInventoryIntegrateWithAccountsBO = new HMCommonSetupBO();
            isInventoryIntegrateWithAccountsBO = commonSetupDA.GetCommonConfigurationInfo("IsInventoryIntegrateWithAccounts", "IsInventoryIntegrateWithAccounts");

            if (isInventoryIntegrateWithAccountsBO != null)
            {
                if (isInventoryIntegrateWithAccountsBO.SetupValue == "1")
                {
                    hfIsInventoryIntegrateWithAccounts.Value = "1";
                    //LoadGLCompany();
                    HMCommonSetupBO isInventoryIntegratationWithAccountsAutomated = commonSetupDA.GetCommonConfigurationInfo("IsInventoryIntegratationWithAccountsAutomated", "IsInventoryIntegratationWithAccountsAutomated");

                    if (isInventoryIntegratationWithAccountsAutomated.SetupValue == "1")
                        hfIsInventoryIntegratationWithAccountsAutomated.Value = "1";
                }
                
            }
        }
        private void IsProductOutApprovalEnable()
        {

            hfIsProductOutApprovalEnable.Value = "1";
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO checkNApproveBO = new HMCommonSetupBO();
            checkNApproveBO = commonSetupDA.GetCommonConfigurationInfo("IsProductOutApprovalEnable", "IsProductOutApprovalEnable");
            if (Convert.ToInt32(checkNApproveBO.SetupValue) != 1)
            {
                hfIsProductOutApprovalEnable.Value = "0";
                this.ddlStatus.Items.Remove(ddlStatus.Items.FindByValue(HMConstants.ApprovalStatus.Checked.ToString()));

            }

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
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        public static ArrayList GetRoomsByRoomType(int RoomTypeId)
        {
            List<RoomNumberBO> roomList = new List<RoomNumberBO>();
            RoomNumberDA roomNumberDA = new RoomNumberDA();

            roomList = roomNumberDA.GetRoomNumberInfoByRoomType(RoomTypeId);

            return new ArrayList(roomList);
        }

        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<ItemViewBO> LoadProductByCategoryNCostcenterId(int costCenterId, int categoryId)
        {
            InvItemDA itemda = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();

            productList = itemda.GetInvItemInfoByCategory(costCenterId, categoryId);
            List<ItemViewBO> itemViewList = new List<ItemViewBO>();

            itemViewList = (from s in productList
                            select new ItemViewBO
                            {
                                ItemId = s.ItemId,
                                ItemName = s.Name,
                                ProductType = s.ProductType

                            }).ToList();

            return itemViewList;
        }

        [WebMethod]
        public static List<SerialDuplicateBO> SerialSearch(string serialNumber, int companyId, int projectId, int locationId, int itemId)
        {
            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();
            PMProductOutDA outDA = new PMProductOutDA();
            serial = outDA.GetCompanyProjectWiseAvailableSerialForAutoSearch(serialNumber, companyId, projectId, locationId, itemId);
            return serial;
        }


        [WebMethod]
        public static ReturnInfo SerialAvailabilityCheck(string FromLocationId,
                                            List<PMProductOutSerialInfoBO> ItemSerialDetails)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isProductOutApproval = new HMCommonSetupBO();
            string serialId = string.Empty, message = string.Empty;
            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();
            PMProductOutDA outDA = new PMProductOutDA();
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                isProductOutApproval = commonSetupDA.GetCommonConfigurationInfo("IsProductOutApprovalEnable", "IsProductOutApprovalEnable");

                foreach (PMProductOutSerialInfoBO srl in ItemSerialDetails.Where(s => s.OutSerialId == 0))
                {
                    if (serialId != string.Empty)
                    {
                        serialId += "," + srl.SerialNumber;
                    }
                    else
                    {
                        serialId = srl.SerialNumber;
                    }
                }

                serial = outDA.SerialAvailabilityCheck(serialId, Convert.ToInt64(FromLocationId));

                foreach (SerialDuplicateBO p in serial)
                {
                    if (message != "")
                        message = ", " + p.ItemName + "(" + p.SerialNumber + ")";
                    else
                        message = p.ItemName + "(" + p.SerialNumber + ")";
                }

                if (!string.IsNullOrEmpty(message))
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo("These Item Serial Does Not Exists. " + message, AlertType.Error);
                    return rtninfo;
                }
                else
                {
                    rtninfo.IsSuccess = true;
                    return rtninfo;
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
        public static ReturnInfo SaveProductOut(PMProductOutBO ProductOut, List<PMProductOutDetailsBO> AddedOutDetails,
                                                List<PMProductOutDetailsBO> EditedOutDetails,
                                                List<PMProductOutDetailsBO> DeletedOutDetails,
                                                List<PMProductOutSerialInfoBO> ItemSerialDetails,
                                                List<PMProductOutSerialInfoBO> DeletedSerialzableProduct
            )
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            int outId;
            string serialId = string.Empty, message = string.Empty, itemName = string.Empty;
            bool isApprovalProcessEnable = true;

            PMProductOutDA outDa = new PMProductOutDA();
            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();
            List<PMProductOutDetailsBO> editedReceivedDetails = new List<PMProductOutDetailsBO>();

            try
            {
                foreach (PMProductOutSerialInfoBO srl in ItemSerialDetails.Where(s => s.OutSerialId == 0))
                {
                    if (serialId != string.Empty)
                    {
                        serialId += "," + srl.SerialNumber;
                    }
                    else
                    {
                        serialId = srl.SerialNumber;
                    }
                }

                if (!string.IsNullOrEmpty(serialId))
                    serial = outDa.SerialAvailabilityCheck(serialId, Convert.ToInt64(ProductOut.FromLocationId));

                foreach (SerialDuplicateBO p in serial)
                {
                    if (message != "")
                        message = ", " + p.ItemName + "(" + p.SerialNumber + ")";
                    else
                        message = p.ItemName + "(" + p.SerialNumber + ")";
                }

                if (!string.IsNullOrEmpty(message))
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo("This Item Serial Does Not Exists. " + message, AlertType.Error);
                    return rtninfo;
                }

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsProductOutApprovalEnable", "IsProductOutApprovalEnable");

                if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedEnable))
                {
                    ProductOut.Status = HMConstants.ApprovalStatus.Pending.ToString();
                    isApprovalProcessEnable = true;
                }
                else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedDisable))
                {
                    ProductOut.Status = HMConstants.ApprovalStatus.Approved.ToString();
                    isApprovalProcessEnable = true;
                }

                if (ProductOut.OutId == 0)
                {
                    ProductOut.CreatedBy = userInformationBO.UserInfoId;
                    ProductOut.OutDate = DateTime.Now;
                    status = outDa.SaveProductOutInfo(ProductOut, AddedOutDetails, ItemSerialDetails, isApprovalProcessEnable, out outId);

                    if (status)
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                       EntityTypeEnum.EntityType.PMProductOut.ToString(), outId,
                       ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                       hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductOut));
                    }
                }
                else
                {
                    ProductOut.LastModifiedBy = userInformationBO.UserInfoId;
                    ItemSerialDetails = (from srl in ItemSerialDetails where srl.OutSerialId == 0 select srl).ToList();

                    status = outDa.UpdateProductOutInfo(ProductOut, AddedOutDetails, EditedOutDetails, DeletedOutDetails, ItemSerialDetails, DeletedSerialzableProduct);

                    if (status)
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                       EntityTypeEnum.EntityType.PMProductOut.ToString(), ProductOut.OutId,
                       ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                       hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductOut));
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
        public static List<InvLocationBO> InvLocationByCostCenter(int costCenterId)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvItemLocationByCostCenter(costCenterId);

            return location;
        }
        [WebMethod]
        public static OutOrderViewBO FillForm(int outId)
        {
            PMProductOutDA outDA = new PMProductOutDA();
            OutOrderViewBO viewBo = new OutOrderViewBO();

            viewBo.ProductOut = outDA.GetProductOutById(outId);
            viewBo.ProductOutDetails = outDA.GetProductOutDetailsById(outId);
            viewBo.ProductSerialInfo = outDA.GetItemOutSerialById(outId);

            return viewBo;
        }
        [WebMethod]
        public static List<PMProductOutDetailsBO> GetProductOutDetails(int outId)
        {
            PMProductOutDA outDA = new PMProductOutDA();
            return outDA.GetProductOutDetailsInfoByOutId(outId);
        }
        [WebMethod]
        public static List<IssueForBO> IssueForDetails(string issueFor)
        {
            List<IssueForBO> issueForList = new List<IssueForBO>();

            if (issueFor == "Costcenter")
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
                var costCenter = costCentreTabDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId);

                foreach (CostCentreTabBO cc in costCenter)
                {
                    issueForList.Add(new IssueForBO() { Id = cc.CostCenterId, IssueName = cc.CostCenter, DefaultStockLocationId = cc.DefaultStockLocationId });
                }
            }
            else if (issueFor == "Employee")
            {
                EmployeeDA empDa = new EmployeeDA();
                var employee = empDa.GetEmployeeInfo();

                foreach (EmployeeBO emp in employee)
                {
                    issueForList.Add(new IssueForBO() { Id = emp.EmpId, IssueName = emp.DisplayName, DefaultStockLocationId = 0 });
                }
            }

            return issueForList;
        }
        [WebMethod]
        public static List<InvUnitHeadBO> LoadRelatedStockBy(int productId)
        {
            InvUnitHeadDA unitHeadDA = new InvUnitHeadDA();
            List<InvUnitHeadBO> unitHeadList = new List<InvUnitHeadBO>();

            InvItemDA invItemDA = new InvItemDA();
            InvItemBO invItemBO = new InvItemBO();

            invItemBO = invItemDA.GetInvItemInfoByItemId(productId);
            if (invItemBO != null)
            {
                if (invItemBO.ItemId > 0)
                {
                    unitHeadList = unitHeadDA.GetRelatedStockBy(invItemBO.StockBy);
                }
            }

            return unitHeadList;
        }
        [WebMethod]
        public static InvItemStockInformationBO LoadCurrentStockQuantity(int companyId, int projectId, int costcenterId, int locationId, int itemId)
        {
            InvItemDA mappingDA = new InvItemDA();
            InvItemStockInformationBO stockInfo = new InvItemStockInformationBO();

            stockInfo = mappingDA.GetInvCompanyProjectWiseItemStockInfo(companyId, projectId, itemId, locationId);
            return stockInfo;
        }

        [WebMethod]
        public static ReturnInfo CheckOrApproveRequsition(int hfIsProductOutApprovalEnable, int outId, List<PMProductOutDetailsBO> approvedItem, List<PMProductOutDetailsBO> cancelItem)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;
            string consumptionApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                PMProductOutBO productOut = new PMProductOutBO();
                PMProductOutDA productReceiveDa = new PMProductOutDA();

                productOut = productReceiveDa.GetProductOutById(outId);
                if (approvedItem.Count == 0)
                {
                    productOut.Status = HMConstants.ApprovalStatus.Cancel.ToString();
                    productOut.LastModifiedBy = userInformationBO.UserInfoId;

                }
                else if (hfIsProductOutApprovalEnable == 1)
                {
                    if (productOut != null)
                    {
                        consumptionApprovedStatus = productOut.Status;
                        if (approvedItem.Count == 0)
                        {
                            productOut.Status = HMConstants.ApprovalStatus.Cancel.ToString();
                        }
                        if (consumptionApprovedStatus == HMConstants.ApprovalStatus.Checked.ToString())
                        {
                            productOut.Status = HMConstants.ApprovalStatus.Approved.ToString();
                            productOut.ApprovedBy = userInformationBO.UserInfoId;
                            productOut.ApprovedDate = DateTime.Now;
                            productOut.LastModifiedBy = userInformationBO.UserInfoId;
                        }
                        else
                        {
                            productOut.Status = HMConstants.ApprovalStatus.Checked.ToString();
                            productOut.CheckedBy = userInformationBO.UserInfoId;
                            productOut.CheckedDate = DateTime.Now;
                        }
                    }
                }
                else
                {
                    productOut.Status = HMConstants.ApprovalStatus.Approved.ToString();
                    productOut.ApprovedBy = userInformationBO.UserInfoId;
                    productOut.ApprovedDate = DateTime.Now;
                    productOut.LastModifiedBy = userInformationBO.UserInfoId;
                }
                if (approvedItem.Count == 0)
                    status = productReceiveDa.CancelProductOutInfo(outId);
                else
                    status = productReceiveDa.CheckOrApproveProductOutConsumption(productOut, approvedItem, cancelItem);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ProductRequisition.ToString(), outId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductRequisition));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }

            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod]
        public static GridViewDataNPaging<PMProductOutBO, GridPaging> GetConsumptionList(DateTime? fromDate, DateTime? toDate,
            string status, string issueType, string issueNumber, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage
            )
        {
            int totalRecords = 0;
            string productOutFor = "DirectOut";

            PMProductOutDA outDa = new PMProductOutDA();
            List<PMProductOutBO> outList = new List<PMProductOutBO>();
            HMUtility hmUtility = new HMUtility();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<PMProductOutBO, GridPaging> myGridData = new GridViewDataNPaging<PMProductOutBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            outList = outDa.GetProductDirectOutForSearch(productOutFor, issueType, issueNumber, fromDate, toDate, status, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(outList, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo CancelConsumption(int outId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            bool status = false;

            PMProductOutBO productOut = new PMProductOutBO();
            PMProductOutDA productOutDA = new PMProductOutDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            
            string statusType = string.Empty;
            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                productOut = productOutDA.GetProductOutById(outId);
                status = productOutDA.DeleteProductOutInfo_New(outId, productOut.Status, productOut.CreatedBy, userInformationBO.UserInfoId);

                //if (productOut.CreatedBy == userInformationBO.UserInfoId && productOut.Status == HMConstants.ApprovalStatus.Pending.ToString())
                //{
                //    status = productOutDA.DeleteProductOutNotCheckedInfo(outId);
                //    statusType = "Hard Deleted";
                //}
                //else if (productOut.Status == HMConstants.ApprovalStatus.Approved.ToString())
                //{
                //    status = productOutDA.DeleteProductOutInfo(outId);
                //}
                //else
                //{
                //    status = productOutDA.CancelProductOutInfo(outId);
                //}
                if (status)
                {
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                                   EntityTypeEnum.EntityType.PMProductOut.ToString(), outId,
                                   ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                                   hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductOut) + statusType);
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;
        }

        [WebMethod]
        public static string GetProductOutDetailsForApproval(int outId)
        {
            PMProductOutDA outDA = new PMProductOutDA();
            List<PMProductOutDetailsBO> ProductOutDetails = new List<PMProductOutDetailsBO>();

            ProductOutDetails = outDA.GetProductOutDetailsById(outId);

            int row = 0;
            string tr = string.Empty;

            tr = "<table id='DetailsRequisitionGrid' class='table table-bordered table-condensed table-responsive' style='width: 100%;'> " +
                 "<thead> " +
                 "    <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'> " +
                 "       <th style='width: 5%;'> " +
                 "       </th> " +
                 "       <th style='width: 20%;'> " +
                 "            Item Name" +
                 "        </th> " +
                 "        <th style='width: 10%;'> " +
                 "            Quantity " +
                 "        </th> " +
                 "        <th style='width: 15%;'> " +
                 "            Unit Measure " +
                 "        </th> " +
                 "        <th style='width: 20%;'> " +
                 "            Approved Quantity " +
                 "        </th> " +
                 "        <th style='display: none;'> " +
                 "            Details Id " +
                 "        </th> " +
                 "        <th style='display: none;'> " +
                 "            Is Edit" +
                 "        </th> " +
                 "    </tr> " +
                 "</thead> " +
                 "<tbody> ";

            //tr += "&nbsp;&nbsp;<a href='#' onclick= 'DeleteItemRequsition(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            string requisitionRemarks = string.Empty;
            foreach (PMProductOutDetailsBO rd in ProductOutDetails)
            {
                if (row % 2 == 0)
                {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else
                {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                if (rd.ApprovalStatus == HMConstants.ApprovalStatus.Approved.ToString() || rd.ApprovalStatus == HMConstants.ApprovalStatus.Cancel.ToString())
                {
                    tr += "<td style='width:5%;'> <input type='checkbox' checked= 'checked' disabled='disabled' id='chkb" + rd.OutDetailsId.ToString() + "' /> </td>";
                }
                else if (rd.ApprovalStatus != HMConstants.ApprovalStatus.Approved.ToString())
                {
                    tr += "<td style='width:5%;'> <input type='checkbox' checked= 'checked' id='chkb" + rd.OutDetailsId.ToString() + "' /> </td>";
                }

                tr += "<td style='width:20%;'>" + rd.ProductName + "</td>";

                tr += "<td style='width:10%;'>" + rd.Quantity + "</td>";
                tr += "<td style='width:15%;'>" + rd.StockBy + "</td>";


                if (rd.ApprovalStatus == HMConstants.ApprovalStatus.Approved.ToString() || rd.ApprovalStatus == HMConstants.ApprovalStatus.Cancel.ToString())
                {
                    tr += "<td style='width:20%; text-align:center;'> <input type='text' id='txt" + rd.OutDetailsId.ToString() + "' value = '" + rd.ApprovedQuantity + "' disabled='disabled' onblur='CheckInputValue(this)' class='form-control' style='width:65px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                }
                else if (rd.ApprovalStatus == HMConstants.ApprovalStatus.Checked.ToString())
                {
                    tr += "<td style='width:20%; text-align:center;'> <input type='text' id='txt" + rd.OutDetailsId.ToString() + "' value = '" + rd.ApprovedQuantity + "' onblur='CheckInputValue(this)' class='form-control' style='width:65px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                }
                else
                {
                    tr += "<td style='width:20%; text-align:center;'> <input type='text' id='txt" + rd.OutDetailsId.ToString() + "' value = '" + rd.Quantity + "' onblur='CheckInputValue(this)' class='form-control' style='width:65px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                }

                tr += "<td style='display:none;'>" + rd.OutDetailsId + "</td>";
                tr += "<td style='display:none;'>0</td>";

                tr += "</tr>";
            }

            tr += "</tbody> " + "</table> ";

            tr += "<div class='divClear'></ div > ";

            return tr;
        }

        [WebMethod]
        public static ReturnInfo OutOrderApproval(string productOutFor, int outId, string approvedStatus, int requisitionOrSalesId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductOutDA orderDa = new PMProductOutDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDa.DirectOutOrderApproval(productOutFor, outId, approvedStatus, requisitionOrSalesId, userInformationBO.UserInfoId);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;

                    if (approvedStatus == HMConstants.ApprovalStatus.Checked.ToString())
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                    }
                    else
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    }

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.PMProductOut.ToString(), outId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductOut));


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
        public static List<GLProjectBO> GetGLProjectByGLCompanyId(int companyId)
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            GLProjectDA entityDA = new GLProjectDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            projectList = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(companyId, userInformationBO.UserGroupId);
            if (projectList.Count == 0)
                projectList.Add(entityDA.GetGLProjectInfoByGLCompany(companyId).FirstOrDefault());
            return projectList;
        }

        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemSearch(string searchTerm, int companyId, int projectId, int costCenterId, int categoryId, int locationId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            //itemInfo = itemDa.GetItemForAutoSearchWithoutSupplier("Transfer", searchTerm, companyId, projectId, costCenterId, ConstantHelper.CustomerSupplierAutoSearch.SupplierItem.ToString(), categoryId, locationId);
            itemInfo = itemDa.GetItemForAutoSearchWithoutSupplier("Transfer", searchTerm, companyId, projectId, costCenterId, "AllItem", categoryId, locationId);

            return itemInfo;
        }
        [WebMethod]
        public static List<InvItemAttributeBO> GetInvItemAttributeByItemIdAndAttributeType(int ItemId, string attributeType)
        {
            InvItemAttributeDA DA = new InvItemAttributeDA();
            List<InvItemAttributeBO> InvItemAttributeBOList = new List<InvItemAttributeBO>();
            InvItemAttributeBOList = DA.GetInvItemAttributeByItemIdAndAttributeType(ItemId, attributeType);

            return InvItemAttributeBOList;
        }
        [WebMethod]
        public static InvItemStockInformationBO GetInvItemStockInfoByItemAndAttributeId(int companyId, int projectId, int itemId, int colorId, int sizeId, int styleId, int locationId)
        {
            SalesTransferDA DA = new SalesTransferDA();
            InvItemStockInformationBO StockInformation = new InvItemStockInformationBO();
            StockInformation = DA.GetInvItemStockInfoByItemAndAttributeId(companyId, projectId, itemId, colorId, sizeId, styleId, locationId);

            return StockInformation;
        }
    }
}