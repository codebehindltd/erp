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
using HotelManagement.Entity.HMCommon;
using Newtonsoft.Json;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity;
using HotelManagement.Data;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class PurchaseReturn : BasePage
    {
        public PurchaseReturn() : base("PurchaseReturnInformation")
        { }
        HiddenField innboardMessage;
        protected int _RestaurantComboId;
        protected int IsService = -1;
        protected int btnPadding = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadSupplierInfo();
                LoadCommonDropDownHiddenField();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            PMProductOutDA outDA = new PMProductOutDA();
            Boolean status = true;//outDA.SaveProductOutInfo(Session["PMProductOut"] as List<PMProductOutBO>);
            if (status == true)
            {
                CommonHelper.AlertInfo("Product Out " + AlertMessage.Success, AlertType.Success);
            }
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
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSupplier.Items.Insert(0, item);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }

        [WebMethod]
        public static List<PMProductReceivedBO> GetProductReceiveDetailsByReceiveOrSupplier(DateTime? fromDate, DateTime? toDate, string receiveNumber, int? supplierId)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (fromDate == null) fromDate = DateTime.Now;
            if (toDate == null) toDate = DateTime.Now;

            PMProductReceivedDA receiveDA = new PMProductReceivedDA();
            List<PMProductReceivedBO> orderList = new List<PMProductReceivedBO>();

            orderList = receiveDA.GetProductReceiveDetailsByReceiveOrSupplier(fromDate, toDate, receiveNumber, supplierId);

            return orderList;
        }

        [WebMethod]
        public static ReceiveOrderViewBO SearcReceiveOrderItemForReturn(int receivedId, string receiveType, int porderId, int locationId)
        {
            ReceiveOrderViewBO viewBo = new ReceiveOrderViewBO();
            PMProductReturnDA returnda = new PMProductReturnDA();

            viewBo.ProductReceivedDetails = returnda.GetItemForReturnFromReceived(receivedId, locationId);

            return viewBo;
        }

        [WebMethod]
        public static ReturnInfo SaveItemReturn(PMSupplierProductReturnBO ProductReturn,
                                                List<PMSupplierProductReturnDetailsBO> ReturnItemAdded,
                                                List<PMSupplierProductReturnDetailsBO> ReturnItemDeleted,
                                                List<PMSupplierProductReturnSerialBO> ItemSerialDetails,
                                                List<PMSupplierProductReturnSerialBO> DeletedSerialzableProduct)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            PMProductReturnDA returnda = new PMProductReturnDA();
            Int64 returnId = 0;
            string serialId = string.Empty, message = string.Empty, itemName = string.Empty;
            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();
            List<PMSupplierProductReturnDetailsBO> EditedReturnDetails = new List<PMSupplierProductReturnDetailsBO>();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                foreach (PMSupplierProductReturnSerialBO srl in ItemSerialDetails.Where(s => s.ReturnId == 0))
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

                serial = returnda.SerialAvailabilityCheckForReceiveReturn(ProductReturn.ReceivedId, serialId, ProductReturn.LocationId);

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

                if (ProductReturn.ReturnId == 0)
                {
                    ProductReturn.ReturnDate = DateTime.Now;
                    ProductReturn.CreatedBy = userInformationBO.UserInfoId;
                    ProductReturn.Status = HMConstants.ApprovalStatus.Pending.ToString();

                    rtninfo.IsSuccess = returnda.SaveReceivedProductReturnInfo(ProductReturn, ReturnItemAdded, ItemSerialDetails, out returnId);

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

                    rtninfo.IsSuccess = returnda.UpdateReceivedProductReturnInfo(ProductReturn, ReturnItemAdded, EditedReturnDetails, ReturnItemDeleted,
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
        public static GridViewDataNPaging<PMSupplierProductReturnBO, GridPaging> SearchReturnOrder(DateTime? fromDate, DateTime? toDate,
                                                                                      string returnNumber, string status,
                                                                                      int gridRecordsCount, int pageNumber,
                                                                                      int isCurrentOrPreviousPage
                                                                                     )
        {

            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<PMSupplierProductReturnBO, GridPaging> myGridData = new GridViewDataNPaging<PMSupplierProductReturnBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (fromDate == null) fromDate = DateTime.Now;
            if (toDate == null) toDate = DateTime.Now;

            PMProductReturnDA returnda = new PMProductReturnDA();
            List<PMSupplierProductReturnBO> orderList = new List<PMSupplierProductReturnBO>();

            orderList = returnda.GetReceivedOrderReturnForSearch(fromDate, toDate, returnNumber, status, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(orderList, totalRecords);
            return myGridData;
        }


        [WebMethod]
        public static ReceiveOrderReturnViewBO EditItemReturn(Int64 returnId, int receivedId)
        {
            ReceiveOrderReturnViewBO viewBo = new ReceiveOrderReturnViewBO();
            PMProductReturnDA orderDa = new PMProductReturnDA();
            PMProductReceivedDA receiveDA = new PMProductReceivedDA();

            viewBo.ProductReturn = orderDa.GetReceiveReturnById(returnId);
            viewBo.ProductReturnDetails = orderDa.GetReceiveItemReturnDetailsForEdit(returnId, receivedId);
            viewBo.ProductReturnSerialInfo = orderDa.GetReceiveItemSerialReturnById(returnId);
            viewBo.ProductReceive = receiveDA.GetProductReceiveDetailsByIdForReturn(receivedId);

            return viewBo;
        }

        [WebMethod]
        public static ReturnInfo ReceiveReturnApproval(Int64 returnId, string approvedStatus, int receivedId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductReturnDA orderDa = new PMProductReturnDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDa.ReceiveReturnApproval(returnId, approvedStatus, receivedId, userInformationBO.UserInfoId);

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
        public static ReturnInfo ReceiveReturnDelete(Int64 returnId, string approvedStatus, Int32 receivedId, int createdBy)
        {
            ReturnInfo rtninf = new ReturnInfo();
            PMProductReturnDA orderDa = new PMProductReturnDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = orderDa.ReceiveReturnDelete(returnId, approvedStatus, receivedId, createdBy, userInformationBO.UserInfoId);

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
        public static bool CheckSerialAvailability(int receivedId, int locationId, string serial)
        {
            bool rtnValue = false;
            PMProductReturnDA returnda = new PMProductReturnDA();
            List<SerialDuplicateBO> serialBO = new List<SerialDuplicateBO>();

            serialBO = returnda.SerialAvailabilityCheckForReceiveReturn(receivedId, serial, locationId);
            if (serialBO.Count > 0)
            {
                rtnValue = false;
            }
            else
                rtnValue = true;

            return rtnValue;
        }

    }
}