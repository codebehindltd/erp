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
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.SalesManagment;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using Newtonsoft.Json;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Payroll;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmSalesReturn : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCostCenter();
                LoadLocation();
            }
        }
        public void LoadCostCenter()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId);

            ddlCostCenterTo.DataSource = List;
            ddlCostCenterTo.DataTextField = "CostCenter";
            ddlCostCenterTo.DataValueField = "CostCenterId";
            ddlCostCenterTo.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            if (List.Count > 1)
                ddlCostCenterTo.Items.Insert(0, item);
        }
        public void LoadLocation()
        {
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlLocationTo.Items.Insert(0, item);
        }


        [WebMethod]
        public static ReturnInfo SaveItemOutOrder(PMProductReturnBO ProductReturn,
                                                 List<PMProductReturnDetailsBO> ReturnItemAdded,
                                                 List<PMProductReturnDetailsBO> ReturnItemDeleted,
                                                 List<PMProductReturnSerialBO> ItemSerialDetails,
                                                 List<PMProductReturnSerialBO> DeletedSerialzableProduct)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            PMProductReturnDA returnda = new PMProductReturnDA();
            Int64 returnId = 0;
            string serialId = string.Empty, message = string.Empty, itemName = string.Empty;
            List<PMProductReturnDetailsBO> EditedReturnDetails = new List<PMProductReturnDetailsBO>();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                if (ProductReturn.ReturnId == 0)
                {
                    ProductReturn.ReturnDate = DateTime.Now;
                    ProductReturn.CreatedBy = userInformationBO.UserInfoId;
                    ProductReturn.Status = HMConstants.ApprovalStatus.Pending.ToString();

                    rtninfo.IsSuccess = returnda.SaveProductSalesReturn(ProductReturn, ReturnItemAdded, ItemSerialDetails, out returnId);

                    if (rtninfo.IsSuccess)
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.PMProductReturn.ToString(), returnId,
                                ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductReturn));

                    }

                }
                else
                {

                    ProductReturn.LastModifiedBy = userInformationBO.UserInfoId;
                    returnId = ProductReturn.ReturnId;

                    EditedReturnDetails = (from pod in ReturnItemAdded where pod.ReturnDetailsId > 0 select pod).ToList();
                    ReturnItemAdded = (from pod in ReturnItemAdded where pod.ReturnDetailsId == 0 select pod).ToList();

                    ItemSerialDetails = (from srl in ItemSerialDetails where srl.ReturnSerialId == 0 select srl).ToList();

                    rtninfo.IsSuccess = returnda.UpdateProductSalesReturnInfo(ProductReturn, ReturnItemAdded, EditedReturnDetails, ReturnItemDeleted,
                                                                   ItemSerialDetails, DeletedSerialzableProduct);

                    if (rtninfo.IsSuccess)
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.PMProductReturn.ToString(), returnId,
                                ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductReturn));

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
        public static ArrayList SearchOutOrderForReturn(string billNumber)
        {
            EmployeeDA empda = new EmployeeDA();
            CostCentreTabDA costCenterda = new CostCentreTabDA();
            PMProductReturnDA returnDa = new PMProductReturnDA();
            List<PMProductReturnDetailsBO> productConsumption = new List<PMProductReturnDetailsBO>();
            string consumptionBy = string.Empty;

            productConsumption = returnDa.GetItemForSalesReturn(billNumber);

            ArrayList arr = new ArrayList();
            arr.Add(productConsumption);
            arr.Add(consumptionBy);

            return arr;
        }

        [WebMethod]
        public static GridViewDataNPaging<PMProductReturnBO, GridPaging> SearchReturnOrder(string returnType, DateTime? fromDate, DateTime? toDate,
                                                                                       string returnNumber, string status,
                                                                                       int gridRecordsCount, int pageNumber,
                                                                                       int isCurrentOrPreviousPage
                                                                                      )
        {

            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<PMProductReturnBO, GridPaging> myGridData = new GridViewDataNPaging<PMProductReturnBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (fromDate == null) fromDate = DateTime.Now;
            if (toDate == null) toDate = DateTime.Now;

            PMProductReturnDA returnda = new PMProductReturnDA();
            List<PMProductReturnBO> orderList = new List<PMProductReturnBO>();

            orderList = returnda.GetProductSalesReturnForSearch(returnType, fromDate, toDate, returnNumber, status, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(orderList, totalRecords);
            return myGridData;
        }

        [WebMethod]
        public static OutReturnViewBO EditItemOut(string returnType, Int64 returnId, Int64 transactionId)
        {
            OutReturnViewBO viewBo = new OutReturnViewBO();
            //PMProductReturnDA orderDa = new PMProductReturnDA();
            //EmployeeDA empda = new EmployeeDA();
            //CostCentreTabDA costCenterda = new CostCentreTabDA();

            //viewBo.ProductReturn = orderDa.GetSalesReturnById(returnId);
            //viewBo.ProductReturnDetails = orderDa.GetItemReturnDetailsForEdit(returnId, transactionId);
            ////viewBo.ProductReturnSerialInfo = orderDa.GetItemSerialReturnById(returnId);

            ////if (viewBo.ProductReturn.IssueType == "Employee")
            ////{
            ////    viewBo.ProductReturn.ConsumptionBy = empda.GetEmployeeInfoById(viewBo.ProductReturn.ToCostCenterId).DisplayName;
            ////}
            ////else if (viewBo.ProductReturn.IssueType == "Costcenter")
            ////{
            ////    viewBo.ProductReturn.ConsumptionBy = costCenterda.GetCostCentreTabInfoById(viewBo.ProductReturn.ToCostCenterId).CostCenter;
            ////}

            return viewBo;
        }

        [WebMethod]
        public static ReturnInfo OutOrderApproval(string returnType, Int64 returnId, string approvedStatus, Int64 transactionId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductReturnDA orderDa = new PMProductReturnDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDa.SalesReturnApproval(returnType, returnId, approvedStatus, transactionId, userInformationBO.UserInfoId);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(approvedStatus, EntityTypeEnum.EntityType.PMProductReturn.ToString(), returnId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductReturn));
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
        public static ReturnInfo OutOrderDelete(string returnType, Int64 returnId, string approvedStatus, Int64 transactionId, int createdBy)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductReturnDA orderDa = new PMProductReturnDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDa.DeletePMSalesReturn(returnId);

                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.PMProductReturn.ToString(), returnId,
                              ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductReturn));

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
        public static List<InvLocationBO> InvLocationByCostCenter(int costCenterId)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvItemLocationByCostCenter(costCenterId);

            return location;
        }
    }
}