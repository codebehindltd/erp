using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.Inventory;
using HotelManagement.Presentation.Website.Common;
using System.Collections;
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

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmProductOutForRoom : BasePage
    {
        HiddenField innboardMessage;
        protected int _RestaurantComboId;
        protected int IsService = -1;
        protected int btnPadding = -1;

        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                Session["PMProductOut"] = null;
                LoadCostCenter();
                LoadStockBy();
                LoadCategory();
                LoadProduct();
                LoadCommonDropDownHiddenField();
                LoadRoomType();

                txtFromDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy");
                txtToDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy");
                CheckPermission();
                
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                return;
            }

            PMProductOutDA outDA = new PMProductOutDA();
            Boolean status = true;//outDA.SaveProductOutInfo(Session["PMProductOut"] as List<HotelRoomInventoryBO>);
            if (status == true)
            {
                CommonHelper.AlertInfo("Product Out " + AlertMessage.Success, AlertType.Success);
                ClearForm();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridview();
        }
        //************************ User Defined Function ********************//

      private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }
        
        public void LoadGridview()
        {
            SetTab("SearchTab");
            PMProductOutDA outDa = new PMProductOutDA();
            List<HotelRoomInventoryBO> outList = new List<HotelRoomInventoryBO>();

            DateTime? fromDate = DateTime.Now, toDate = DateTime.Now;
            int costCenterIdFrom = 0, locationIdFrom = 0, roomId = 0;

            if (!string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                //fromDate = Convert.ToDateTime(txtFromDate.Text);
                fromDate = CommonHelper.DateTimeToMMDDYYYY(txtFromDate.Text);
            }

            if (!string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                //toDate = Convert.ToDateTime(txtToDate.Text);
                toDate = CommonHelper.DateTimeToMMDDYYYY(txtToDate.Text);
            }

            if (ddlSearchLocation.SelectedValue != "0" && ddlSearchLocation.SelectedValue != "")
            {
                locationIdFrom = Convert.ToInt32(hfSearchLocationId.Value);
            }

            if (ddlSearchRoomId.SelectedValue != "0" && ddlSearchRoomId.SelectedValue != "")
            {
                roomId = Convert.ToInt32(ddlSearchRoomId.SelectedValue);
            }
            
            outList = outDa.GetProductOutForRoomSearch(roomId, costCenterIdFrom, locationIdFrom, fromDate, toDate);

            gvProductOutInfo.DataSource = outList;
            gvProductOutInfo.DataBind();
        }

        public void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetCostCentreTabInfo();
            ddlCostCenter.DataSource = List;
            ddlCostCenter.DataTextField = "CostCenter";
            ddlCostCenter.DataValueField = "CostCenterId";
            ddlCostCenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCostCenter.Items.Insert(0, item);

            ddlSearchCostCenter.DataSource = List;
            ddlSearchCostCenter.DataTextField = "CostCenter";
            ddlSearchCostCenter.DataValueField = "CostCenterId";
            ddlSearchCostCenter.DataBind();
            ddlSearchCostCenter.Items.Insert(0, item);
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllInvItemCatagoryInfoByServiceType("Product");
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

        private void LoadRoomType()
        {
            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> entityBO = new List<RoomTypeBO>();
            entityBO = roomTypeDA.GetRoomTypeInfo();
            ddlRoomType.DataSource = entityBO;
            ddlRoomType.DataTextField = "RoomType";
            ddlRoomType.DataValueField = "RoomTypeId";
            ddlRoomType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlRoomType.Items.Insert(0, item);
          
            ddlSearchRoomType.DataSource = entityBO;
            ddlSearchRoomType.DataTextField = "RoomType";
            ddlSearchRoomType.DataValueField = "RoomTypeId";
            ddlSearchRoomType.DataBind();
            ddlSearchRoomType.Items.Insert(0, item);
            ddlSearchRoomType.SelectedValue = "0";

        }

        private bool IsFormValid()
        {
            bool status = true;
            List<HotelRoomInventoryBO> detailListBO = Session["PMProductOut"] == null ? new List<HotelRoomInventoryBO>() : Session["PMProductOut"] as List<HotelRoomInventoryBO>;

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

        private void LocationByCostCenter(int costCenterId)
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvItemLocationByCostCenter(costCenterId);

            ddlSearchLocation.DataSource = location;
            ddlSearchLocation.DataTextField = "Name";
            ddlSearchLocation.DataValueField = "LocationId";
            ddlSearchLocation.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSearchLocation.Items.Insert(0, item);
        }

        public void LoadRoomByRoomType(string roomTypeId)
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();
            RoomNumberDA roomDa = new RoomNumberDA();

            roomNumberList = roomDa.GetRoomNumberByRoomType(Convert.ToInt32(roomTypeId));

            ddlSearchRoomId.DataSource = roomNumberList;
            ddlSearchRoomId.DataTextField = "RoomNumber";
            ddlSearchRoomId.DataValueField = "RoomId";
            ddlSearchRoomId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSearchRoomId.Items.Insert(0, item);
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

        protected void ddlSearchCostCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationByCostCenter(Convert.ToInt32(ddlSearchCostCenter.SelectedValue));
            SetTab("SearchTab");
        }

        protected void ddlSearchRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRoomByRoomType(ddlSearchRoomType.SelectedValue);
            SetTab("SearchTab");
        }

        protected void gvProductOutInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                var item = (HotelRoomInventoryBO)e.Row.DataItem;

                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                ImageButton ImgDetailsApproved = (ImageButton)e.Row.FindControl("ImgDetailsApproved");

                if (item.Status != "Approved")
                {
                    imgUpdate.Visible = isUpdatePermission;
                    imgDelete.Visible = isDeletePermission;
                    ImgDetailsApproved.Visible = isSavePermission;
                }
                else
                {
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                    ImgDetailsApproved.Visible = false;
                }
            }
        }

        protected void gvProductOutInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                if (e.CommandName == "CmdDelete")
                {
                    int InventoryOutId = Convert.ToInt32(e.CommandArgument.ToString());
                    string result = string.Empty;

                    PMProductOutDA outDa = new PMProductOutDA();
                    bool status = outDa.DeleteProductOutForRoom(InventoryOutId);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        LoadGridview();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                    SetTab("SearchTab");
                }
                if (e.CommandName == "CmdItemOutApproved")
                {
                    int InventoryOutId = Convert.ToInt32(e.CommandArgument.ToString());

                    GridViewRow gvRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int rowIndex = gvRow.RowIndex;

                    string outFor = gvProductOutInfo.Rows[rowIndex].Cells[2].Text;

                    HotelRoomInventoryBO productOut = new HotelRoomInventoryBO();
                    PMProductOutDA productReceiveDa = new PMProductOutDA();

                    productOut.InventoryOutId = InventoryOutId;
                    productOut.LastModifiedBy = userInformationBO.UserInfoId;
                    productOut.Status = "Approved";

                    bool status = productReceiveDa.UpdateProductOutStatusNItemStockForDirectOutForRoom(productOut);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
                        LoadGridview();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }

        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
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
                                ItemName = s.Name
                            }).ToList();

            return itemViewList;
        }

        [WebMethod]
        public static List<RoomNumberBO> LoadRoomNumberByRoomType(string roomTypeId)
        {
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();
            RoomNumberDA roomDa = new RoomNumberDA();

            roomNumberList = roomDa.GetRoomNumberByRoomType(Convert.ToInt32(roomTypeId));

            return roomNumberList;
        }

        [WebMethod]
        public static ReturnInfo SaveProductOut(HotelRoomInventoryBO ProductOut, List<HotelRoomInventoryDetailsBO> AddedOutDetails, List<HotelRoomInventoryDetailsBO> EditedOutDetails, List<HotelRoomInventoryDetailsBO> DeletedOutDetails)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
                invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsProductOutApprovalEnable", "IsProductOutApprovalEnable");

                if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedEnable))
                {
                    ProductOut.Status = HMConstants.ApprovalStatus.Pending.ToString();
                }
                else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == Convert.ToInt32(HMConstants.PurchaseOrderTemplate.ApprovedDisable))
                {
                    ProductOut.Status = HMConstants.ApprovalStatus.Pending.ToString();//HMConstants.ApprovalStatus.Approved.ToString();
                }

                PMProductOutDA outDa = new PMProductOutDA();
                int outId;
                if (ProductOut.InventoryOutId == 0)
                {
                    ProductOut.CreatedBy = userInformationBO.UserInfoId;
                    ProductOut.OutDate = DateTime.Now;
                    status = outDa.SaveProductOutForRoom(ProductOut, AddedOutDetails, out outId);

                    if (status)
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                      bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                      EntityTypeEnum.EntityType.ProductOutForRoom.ToString(), outId,
                      ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                      hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductOutForRoom));
                    }
                }
                else
                {
                    ProductOut.LastModifiedBy = userInformationBO.UserInfoId;
                    status = outDa.UpdateProductOutForRoom(ProductOut, AddedOutDetails, EditedOutDetails, DeletedOutDetails);

                    if (status)
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                     EntityTypeEnum.EntityType.ProductOutForRoom.ToString(), ProductOut.InventoryOutId,
                     ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                     hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductOutForRoom));
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
        public static ProductOutFroRoomViewBO FillForm(int InventoryOutId)
        {
            PMProductOutDA outDA = new PMProductOutDA();
            ProductOutFroRoomViewBO viewBo = new ProductOutFroRoomViewBO();

            viewBo.ProductOut = outDA.GetProductOutForRoomById(InventoryOutId);
            viewBo.ProductOutDetails = outDA.GetProductForRoomOutDetailsById(InventoryOutId);

            return viewBo;
        }

        [WebMethod]
        public static List<HotelRoomInventoryDetailsBO> GetProductOutDetails(int InventoryOutId)
        {
            PMProductOutDA outDA = new PMProductOutDA();
            return outDA.GetProductForRoomOutDetailsById(InventoryOutId);
        }

        [WebMethod]
        public static ArrayList GetRoomsByRoomType(int RoomTypeId)
        {
            List<RoomNumberBO> roomList = new List<RoomNumberBO>();
            RoomNumberDA roomNumberDA = new RoomNumberDA();

            roomList = roomNumberDA.GetRoomNumberByRoomType(RoomTypeId);

            return new ArrayList(roomList);
        }

  

    }
}