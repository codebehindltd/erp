using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.FixedAsset
{
    public partial class FixedAssetReturn : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCostCenter();
                LoadCategory();
                LoadEmployee();
                isSerialAutoLoad();
                LoadDepartment();
            }
        }
        public void LoadCostCenter()
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId)
                    .Where(a => a.CostCenterType == "Inventory").ToList();

            ddlCostCenterFrom.DataSource = List;
            ddlCostCenterFrom.DataTextField = "CostCenter";
            ddlCostCenterFrom.DataValueField = "CostCenterId";
            ddlCostCenterFrom.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            if (List.Count > 1)
                ddlCostCenterFrom.Items.Insert(0, item);

            ddlCostCenterTo.DataSource = List;
            ddlCostCenterTo.DataTextField = "CostCenter";
            ddlCostCenterTo.DataValueField = "CostCenterId";
            ddlCostCenterTo.DataBind();
            if (List.Count > 1)
                ddlCostCenterTo.Items.Insert(0, item);


        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllInvItemCatagoryInfoByServiceType("FixedAsset");
            ddlCategory.DataSource = List;
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---All---";
            ddlCategory.Items.Insert(0, item);
        }
        private void LoadEmployee()
        {
            EmployeeDA empDa = new EmployeeDA();
            var employee = empDa.GetEmployeeInfo();

            ddlEmployee.DataSource = employee;
            ddlEmployee.DataTextField = "DisplayName";
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEmployee.Items.Insert(0, item);
        }
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            this.ddlDepartment.DataSource = entityDA.GetDepartmentInfo();
            this.ddlDepartment.DataTextField = "Name";
            this.ddlDepartment.DataValueField = "DepartmentId";
            this.ddlDepartment.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlDepartment.Items.Insert(0, item);


        }
        private void isSerialAutoLoad()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO IsSeriaFillBO = new HMCommonSetupBO();
            IsSeriaFillBO = commonSetupDA.GetCommonConfigurationInfo("IsItemSerialFillWithAutoSearch", "IsItemSerialFillWithAutoSearch");
            hfIsItemSerialFillWithAutoSearch.Value = IsSeriaFillBO.SetupValue;

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
        public static List<InvItemAutoSearchBO> ItemSearch(string transferFor, int outFor, string searchTerm, int companyId, int projectId, int costCenterId, int categoryId, int locationId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetAvailableFixedAssetItemForReturnAutoSearch(transferFor, outFor, searchTerm, costCenterId, categoryId, locationId);
            return itemInfo;
        }
        [WebMethod]
        public static List<SerialDuplicateBO> SerialSearch(string serialNumber, int companyId, int projectId, int locationId, int itemId)
        {
            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();

            PMProductOutDA outDA = new PMProductOutDA();
            serial = outDA.GetFixedAssetAvailableSerialForAutoSearch(serialNumber, companyId, projectId, locationId, itemId);

            return serial;
        }
        [WebMethod]
        public static ReturnInfo SerialAvailabilityCheck(string FromLocationId, List<PMProductOutSerialInfoBO> ItemSerialDetails)
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
                    rtninfo.AlertMessage = CommonHelper.AlertInfo("This Item Serial Does Not Exists. " + message, AlertType.Error);
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
        public static ReturnInfo SaveItemOutOrder(PMProductOutBO ProductOut, List<PMProductOutDetailsBO> TransferItemAdded,
                                                  List<PMProductOutDetailsBO> TransferItemDeleted, List<PMProductOutSerialInfoBO> ItemSerialDetails,
                                                  List<PMProductOutSerialInfoBO> DeletedSerialzableProduct)
        {

            ReturnInfo rtninfo = new ReturnInfo();
            PMProductOutDA outDA = new PMProductOutDA();
            int outId = 0;
            bool isApprovalProcessEnable = true;
            string serialId = string.Empty, message = string.Empty, itemName = string.Empty;

            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();
            List<PMProductOutDetailsBO> editedReceivedDetails = new List<PMProductOutDetailsBO>();
            HMCommonSetupBO isProductOutApproval = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

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

                serial = outDA.SerialAvailabilityCheck(serialId, Convert.ToInt64(ProductOut.FromLocationId));

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

                if (ProductOut.OutId == 0)
                {
                    ProductOut.OutDate = DateTime.Now;
                    ProductOut.CreatedBy = userInformationBO.UserInfoId;
                    ProductOut.Status = HMConstants.ApprovalStatus.Pending.ToString();

                    if (Convert.ToInt32(isProductOutApproval.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedEnable))
                    {
                        ProductOut.Status = HMConstants.ApprovalStatus.Pending.ToString();
                        isApprovalProcessEnable = true;
                    }
                    else if (Convert.ToInt32(isProductOutApproval.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedDisable))
                    {
                        ProductOut.Status = HMConstants.ApprovalStatus.Approved.ToString();
                        isApprovalProcessEnable = false;
                    }

                    rtninfo.IsSuccess = outDA.SaveProductOutInfo(ProductOut, TransferItemAdded, ItemSerialDetails, isApprovalProcessEnable, out outId);

                    if (rtninfo.IsSuccess)
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.PMProductOut.ToString(), outId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductReceive));

                    }

                }
                else
                {

                    ProductOut.LastModifiedBy = userInformationBO.UserInfoId;
                    outId = ProductOut.OutId;

                    List<PMProductReceivedDetailsBO> EditedReceivedDetails = new List<PMProductReceivedDetailsBO>();

                    editedReceivedDetails = (from pod in TransferItemAdded where pod.OutDetailsId > 0 select pod).ToList();
                    TransferItemAdded = (from pod in TransferItemAdded where pod.OutDetailsId == 0 select pod).ToList();

                    ItemSerialDetails = (from srl in ItemSerialDetails where srl.OutSerialId == 0 select srl).ToList();

                    rtninfo.IsSuccess = outDA.UpdateProductOutInfo(ProductOut, TransferItemAdded, editedReceivedDetails, TransferItemDeleted,
                                                                   ItemSerialDetails, DeletedSerialzableProduct);

                    if (rtninfo.IsSuccess)
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.PMProductOut.ToString(), outId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductReceive));

                    }
                }

                if (!rtninfo.IsSuccess)
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
        public static GridViewDataNPaging<PMProductOutBO, GridPaging> SearchOutOrder(string outType, DateTime? fromDate, DateTime? toDate,
                                                                                        string issueNumber, string status,
                                                                                        int gridRecordsCount, int pageNumber,
                                                                                        int isCurrentOrPreviousPage
                                                                                       )
        {

            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<PMProductOutBO, GridPaging> myGridData = new GridViewDataNPaging<PMProductOutBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (fromDate == null) fromDate = DateTime.Now;
            if (toDate == null) toDate = DateTime.Now;

            PMProductOutDA receiveDA = new PMProductOutDA();
            List<PMProductOutBO> orderList = new List<PMProductOutBO>();

            orderList = receiveDA.GetProductOutForSearch(outType, fromDate, toDate, issueNumber, status, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(orderList, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static OutOrderViewBO EditItemOut(string issueType, int outId, int requisitionOrSalesId)
        {
            OutOrderViewBO viewBo = new OutOrderViewBO();
            PMProductOutDA orderDa = new PMProductOutDA();

            viewBo.ProductOut = orderDa.GetProductOutById(outId);
            viewBo.ProductSerialInfo = orderDa.GetItemOutSerialById(outId);

            viewBo.ProductOutDetails = orderDa.GetItemOutDetailsByOutId(outId);

            return viewBo;
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

                rtninf.IsSuccess = orderDa.OutOrderApproval(productOutFor, outId, approvedStatus, requisitionOrSalesId, userInformationBO.UserInfoId);

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isTransferProductReceiveDisable = new HMCommonSetupBO();

                isTransferProductReceiveDisable = commonSetupDA.GetCommonConfigurationInfo("IsTransferProductReceiveDisable", "IsTransferProductReceiveDisable");

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
                        if (isTransferProductReceiveDisable.SetupValue == "1")
                        {
                            rtninf.IsSuccess = orderDa.ItemReceiveOutOrder(productOutFor, outId, requisitionOrSalesId, userInformationBO.UserInfoId);
                        }
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
        public static ReturnInfo OutOrderDelete(string issueType, int outId, string approvedStatus, int createdBy)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductOutDA orderDa = new PMProductOutDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDa.OutOrderDelete(issueType, outId, approvedStatus, createdBy, userInformationBO.UserInfoId);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.PMProductOut.ToString(), outId,
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
    }
}