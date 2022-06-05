using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System.Drawing;
using System.Drawing.Printing;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using Microsoft.Reporting.WebForms;
using System.Security;
using System.Security.Permissions;


namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmGuestRoomKotBill : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        private string waiterName = string.Empty;
        HMUtility hmUtility = new HMUtility();
        protected int isLoadGridInformation = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        private string currentButtonFormName = string.Empty;
        private string strCostCenterDefaultView = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadCompanyInformation();
            }

            CheckObjectPermission();
            string queryString1 = string.Empty;
            string queryString2 = string.Empty;
            this.btnBackPreviousPage.Visible = true;
            this.btnOrderSubmit.Visible = true;
            string queryStringId = Request.QueryString["Kot"];
            this.txtBearerIdInformation.Text = "1";
            //if (Session["txtTableIdInformation"] != null)
            //{
            //    this.txtBearerIdInformation.Text = Session["txtBearerIdInformation"].ToString();
            //}
            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            hfCostCenterId.Value = currentUserInformationBO.WorkingCostCenterId.ToString();

            if (Session["txtTableIdInformation"] != null)
            {
                this.txtTableIdInformation.Text = Session["txtTableIdInformation"].ToString();
            }
            if (Session["txtTableNumberInformation"] != null)
            {
                this.txtTableNumberInformation.Text = Session["txtTableNumberInformation"].ToString();
            }
            if (Session["txtKotIdInformation"] != null)
            {
                this.txtKotIdInformation.Text = Session["txtKotIdInformation"].ToString();
            }
            if (Session["txtCategoryInformation"] != null)
            {
                this.txtCategoryInformation.Text = Session["txtCategoryInformation"].ToString();
            }
            if (!string.IsNullOrEmpty(queryStringId))
            {
                queryString1 = queryStringId.Split(':')[0].ToString();
                this.currentButtonFormName = queryString1;
                if (queryString1 == "RoomAllocation")
                {
                    this.imgBtnInnboardBill.Visible = false;
                    this.imgItemSearch.Visible = false;
                    this.btnBackPreviousPage.Visible = false;
                    this.btnOrderSubmit.Visible = false;
                    this.imgBtnSpecialRemarks.Visible = false;
                    this.imgBtnRestaurantCookBook.Visible = false;
                    this.imgBtnPaxInfo.Visible = false;
                    this.imgBtnHome.Visible = false;
                    this.imgBtnHome.Visible = false;
                    isLoadGridInformation = -1;
                    Session["txtTableIdInformation"] = null;
                    this.txtKotIdInformation.Text = string.Empty;
                    Session["txtKotIdInformation"] = null;
                    this.LoadRoomAllocation();
                }
                else if (queryString1 == "BillCreateForRoom")
                {
                    //this.imgBtnTableWiseKotBill.Visible = false;
                    this.BillCreateForRoom(queryStringId.Split(':')[1].ToString());
                }
                else if (queryString1 == "RestaurantItemCategory")
                {
                    if (Convert.ToInt32(queryStringId.Split(':')[1].ToString()) == 0)
                    {
                        imgBtnRestaurantCookBook.Visible = false;
                    }

                    this.LoadRestaurantItemCategory(Convert.ToInt32(queryStringId.Split(':')[1].ToString()));
                }
                else if (queryString1 == "RestaurantItem")
                {
                    this.LoadRestaurantItem(queryStringId.Split(':')[1].ToString());
                }
                else if (queryString1 == "KotReprint")
                {
                    ReprintKotBill(queryStringId.Split(':')[1].ToString());

                    imgBtnRestaurantCookBook.Visible = false;
                    Response.Redirect("/POS/frmGuestRoomKotBill.aspx?Kot=RoomAllocation");
                }
            }
        }
        protected void btnKotRePrintPreview_Click(object sender, EventArgs e)
        {
            string roomNumber = hfTableIdForPrint.Value;
            int kotId = 0;

            KotBillMasterViewBO entityBO = new KotBillMasterViewBO();
            KotBillMasterDA entityMDA = new KotBillMasterDA();
            KotBillDetailDA entityDA = new KotBillDetailDA();

            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
            roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumber);
            if (roomAllocationBO.RoomId > 0)
            {
                Session["txtTableNumberInformation"] = this.txtTableNumberInformation.Text;
                entityBO = entityMDA.GetKotBillMasterBySourceIdNType(currentUserInformationBO.WorkingCostCenterId, "GuestRoom", roomAllocationBO.RegistrationId);
                if (entityBO != null)
                {
                    if (entityBO.KotId > 0)
                    {
                        kotId = entityBO.KotId;
                    }
                    else
                        return;

                    string url = "/POS/Reports/frmReportKotBillInfo.aspx?kotId=" + kotId.ToString() + "&room=" + roomNumber + "&kbm=" + "kotm" + "&isrp=" + "rp";
                    string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
            }
        }
        private void LoadRoomAllocation()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberListBO = new List<RoomNumberBO>();

            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> RoomTypeInfoList = new List<RoomTypeBO>();
            RoomTypeInfoList = roomTypeDA.GetRoomTypeInfo();

            string fullContent = string.Empty;

            int RoomPossibleVacantDiv = 0;
            int RoomOccupaiedDiv = 0;

            string roomSummary = string.Empty;

            string topPart = @" <div class='row' style='padding:10px 0 10px 50px;'>
                                <div id='legendContainerForRestaurant'>                                
                                <div class='legend RoomVacantDiv'>
                                </div>
                                <div class='legendText'>
                                    Item Not Exist
                                </div>
                                <div class='legend RoomOccupaiedDiv'>
                                </div>
                                <div class='legendText'>
                                    Item Exist</div>
                            </div>  
                            </div>                          
                                <div class='panel-heading'>";

            string topTemplatePart = @"</div>
                                <div class='panel-body'>           
                                ";
            string groupNamePart = string.Empty;

            string endTemplatePart = @"
                            </div>";

            roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(0);

            List<KotBillMasterBO> entityBOList = new List<KotBillMasterBO>();
            KotBillMasterDA entityDA = new KotBillMasterDA();
            entityBOList = entityDA.GetBillDetailInformationForRoom();

            foreach (KotBillMasterBO kbMasterBO in entityBOList)
            {
                foreach (RoomNumberBO roomNumberBO in roomNumberListBO)
                {
                    if (roomNumberBO.RoomId == kbMasterBO.RoomId)
                    {
                        roomNumberBO.IsRestaurantItemExist = true;
                    }
                }
            }

            string subContent = string.Empty;


            for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
            {
                if (roomNumberListBO[iRoomNumber].StatusId == 2)
                {
                    if (roomNumberListBO[iRoomNumber].IsRestaurantItemExist)
                    {
                        //                        subContent += @"<a href='/Restaurant/frmGuestRoomKotBill.aspx?Kot=BillCreateForRoom:" + roomNumberListBO[iRoomNumber].RoomNumber;
                        //                        subContent += "'>";
                        //                        subContent = @"<div class='KOTNTableNumberDiv' style='display:none;'>" + currentUserInformationBO.WorkingCostCenterId + "," + entityListBO[iTableNumber].TableId + "," + entityListBO[iTableNumber].StatusId + "</div>";
                        //                        subContent += @"<div class='DivRoomContainer'><div class='RoomOccupaiedDiv'></div>
                        //                                            <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div></a>";
                        //                        RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;

                        subContent += @"<div class='DivRoomContainer'>";
                        subContent += @"<div class='NotDraggable RoomOccupaiedDiv'>";
                        subContent += @"<div class='KOTNTableNumberDiv' style='display:none;'>" + currentUserInformationBO.WorkingCostCenterId + "," + roomNumberListBO[iRoomNumber].RoomNumber + "," + roomNumberListBO[iRoomNumber].StatusId + "</div>";
                        subContent += @"<div class='TableNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div></div>";

                        RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                    }
                    else
                    {
                        subContent += @"<a href='/POS/frmGuestRoomKotBill.aspx?Kot=BillCreateForRoom:" + roomNumberListBO[iRoomNumber].RoomNumber;
                        subContent += "'>";
                        subContent += @"<div class='DivRoomContainer'><div class='RoomVacantDiv'>
                                                                    <div class='RoomNumberWhiteTextDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div></div></a>";
                        RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;

                        //subContent += @"<a href='/Restaurant/frmGuestRoomKotBill.aspx?Kot=BillCreateForRoom:" + roomNumberListBO[iRoomNumber].RoomNumber;
                        //subContent += @"<div class='DivRoomContainer'>";
                        //subContent += @"<div class='NotDraggable RoomVacantDiv'>";
                        //subContent += @"<div class='TableNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div></div></a>";

                        RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                    }
                }
            }

            groupNamePart = "Occupaied Room List";
            fullContent += subContent;
            this.ltlRoomTemplate.Text = topPart + groupNamePart + topTemplatePart + fullContent + endTemplatePart;
        }
        private void LoadRestaurantItemCategory(int categoryId)
        {
            string fullContent = string.Empty;
            InvCategoryDA roomNumberDA = new InvCategoryDA();
            int costCenterId = !string.IsNullOrWhiteSpace(this.hfCostCenterId.Value) ? Convert.ToInt32(this.hfCostCenterId.Value) : 0;

            List<InvCategoryBO> roomNumberListAllBO = new List<InvCategoryBO>();
            List<InvCategoryBO> roomNumberListWithZeroBO = new List<InvCategoryBO>();
            List<InvCategoryBO> roomNumberListWithoutZeroBO = new List<InvCategoryBO>();

            roomNumberListAllBO = roomNumberDA.GetInvCategoryInfoByLabel(costCenterId, categoryId);
            roomNumberListWithZeroBO = roomNumberListAllBO.Where(x => x.ChildCount == 0).ToList();
            roomNumberListWithoutZeroBO = roomNumberListAllBO.Where(x => x.ChildCount != 0).ToList();

            //------------------------------------------------Item Generate-----------------------------------------------------------------------------
            string fullItemContent = string.Empty;
            for (int iItemCategory = 0; iItemCategory < roomNumberListWithZeroBO.Count; iItemCategory++)
            {
                int itemCategory = roomNumberListWithZeroBO[iItemCategory].CategoryId;
                InvItemDA invItemDA = new InvItemDA();
                List<InvItemBO> roomNumberListBO = new List<InvItemBO>();

                Session["txtCategoryInformation"] = itemCategory;
                roomNumberListBO = invItemDA.GetInvItemInfoByCategoryId(costCenterId, itemCategory);

                string subItemContent = string.Empty;

                for (int iItem = 0; iItem < roomNumberListBO.Count; iItem++)
                {
                    string Content1 = @"<div class='DivRestaurantItemContainer'>";
                    string Content2 = @"<div class='RestaurantItemDiv'><img class='ItemImageSize' ID='ContentPlaceHolder1_img" + roomNumberListBO[iItem].ItemId + "'";
                    string Content3 = string.Format("onclick=\"return PerformAction({0}, '{1}')\"", roomNumberListBO[iItem].ItemId, roomNumberListBO[iItem].Name);
                    string Content4 = "src='" + roomNumberListBO[iItem].ImageName;
                    string Content5 = @"' /></div>
                                        <div class='ItemNameDiv'>" + roomNumberListBO[iItem].Name + "</div></div>";

                    subItemContent += Content1 + Content2 + Content3 + Content4 + Content5;
                }

                fullItemContent += subItemContent;
            }

            //------------------------------------------------Category Generate-----------------------------------------------------------------------------
            string topPart = @"
                            <div id='SearchPanel' class='panel panel-default'>
                                <div class='panel-heading'>";

            string topTemplatePart = @"</div>
                                <div class='panel-body'>           
                                ";

            string endTemplatePart = @"</div>
                            </div>";

            string subContent = string.Empty;

            for (int iItemCategory = 0; iItemCategory < roomNumberListWithoutZeroBO.Count; iItemCategory++)
            {
                string Content1 = string.Empty;
                if (roomNumberListWithoutZeroBO[iItemCategory].ChildCount.ToString() == "1")
                {
                    Content1 = @"<div class='DivRestaurantItemContainer'>
                                        <a href='/POS/frmGuestRoomKotBill.aspx?Kot=RestaurantItem:" + roomNumberListWithoutZeroBO[iItemCategory].CategoryId;
                }
                else
                {
                    Content1 = @"<div class='DivRestaurantItemContainer'>
                                        <a href='/POS/frmGuestRoomKotBill.aspx?Kot=RestaurantItemCategory:" + roomNumberListWithoutZeroBO[iItemCategory].CategoryId;
                }

                string Content2 = @"'><div class='RestaurantItemDiv'><img ID='ContentPlaceHolder1_img" + roomNumberListWithoutZeroBO[iItemCategory].CategoryId;
                string Content3 = @"' class='RestaurantItemImage' src='" + roomNumberListWithoutZeroBO[iItemCategory].ImageName;

                string Content4 = @"' /></div></a>
                                        <div class='ItemNameDiv'>" + roomNumberListWithoutZeroBO[iItemCategory].Name + "</div></div>";

                subContent += Content1 + Content2 + Content3 + Content4;
            }

            fullContent += topPart + topTemplatePart + fullItemContent + subContent + endTemplatePart;
            this.ltlRoomTemplate.Text = fullContent;
        }
        private void LoadRestaurantItem(string queryString)
        {
            if (!string.IsNullOrWhiteSpace(queryString))
            {
                int itemCategory = Convert.ToInt32(queryString);
                InvItemDA roomNumberDA = new InvItemDA();
                List<InvItemBO> roomNumberListBO = new List<InvItemBO>();

                string fullContent = string.Empty;
                int costCenterId = Convert.ToInt32(hfCostCenterId.Value);

                Session["txtCategoryInformation"] = itemCategory;
                roomNumberListBO = roomNumberDA.GetInvItemInfoByCategoryId(costCenterId, itemCategory);

                string topPart = @"
                            <div id='SearchPanel' class='panel panel-default'>
                                <div class='panel-heading'>";

                string topTemplatePart = @"Restaurant Item Information</div>
                                <div class='panel-body'>           
                                ";

                string endTemplatePart = @"</div>
                            </div>";

                string subContent = string.Empty;

                for (int iItem = 0; iItem < roomNumberListBO.Count; iItem++)
                {
                    //                    string Content1 = @"<div class='DivRestaurantItemContainer'>";
                    //                    string Content2 = @"<div class='RestaurantItemDiv'><img class='ItemImageSize' ID='ContentPlaceHolder1_img" + roomNumberListBO[iItem].ItemId;
                    //                    string Content3 = @"'  onclick='return PerformAction(" + roomNumberListBO[iItem].ItemId + ",'" + roomNumberListBO[iItem].Name + "'";
                    //                    string Content4 = @");'  src='" + roomNumberListBO[iItem].ImageName;
                    //                    string Content5 = @"' /></div>
                    //                                        <div class='ItemNameDiv'>" + roomNumberListBO[iItem].Name + "</div></div>";

                    string Content1 = @"<div class='DivRestaurantItemContainer'>";
                    string Content2 = @"<div class='RestaurantItemDiv'><img class='ItemImageSize' ID='ContentPlaceHolder1_img" + roomNumberListBO[iItem].ItemId + "'";
                    string Content3 = string.Format("onclick=\"return PerformAction({0}, '{1}')\"", roomNumberListBO[iItem].ItemId, roomNumberListBO[iItem].Name);
                    string Content4 = "src='" + roomNumberListBO[iItem].ImageName;
                    string Content5 = @"' /></div>
                                        <div class='ItemNameDiv'>" + roomNumberListBO[iItem].Name + "</div></div>";

                    subContent += Content1 + Content2 + Content3 + Content4 + Content5;
                }

                fullContent += topPart + topTemplatePart + subContent + endTemplatePart;
                this.ltlRoomTemplate.Text = fullContent;
            }
        }
        private void BillCreateForRoom(string queryString)
        {
            if (!string.IsNullOrWhiteSpace(queryString))
            {
                //int tableId = Convert.ToInt32(queryString);

                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(queryString);
                if (roomAllocationBO.RoomId > 0)
                {

                    RestaurantTableBO entityTableBO = new RestaurantTableBO();
                    RestaurantTableDA entityTableDA = new RestaurantTableDA();

                    int tmpPkId;
                    KotBillMasterBO entityBO = new KotBillMasterBO();
                    KotBillMasterDA entityDA = new KotBillMasterDA();

                    UserInformationBO currentUserInformationBO = new UserInformationBO();
                    currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    entityBO.SourceName = "GuestRoom";
                    entityBO.SourceId = roomAllocationBO.RegistrationId;

                    RestaurantBearerDA restaurantBearerDA = new RestaurantBearerDA();
                    RestaurantBearerBO restaurantBearerBO = new RestaurantBearerBO();

                    restaurantBearerBO = restaurantBearerDA.GetRestaurantBearerInfoByCostCenterIdNEmpIdNIsBearer(currentUserInformationBO.WorkingCostCenterId, currentUserInformationBO.UserInfoId, 0);
                    if (restaurantBearerBO != null)
                    {
                        if (restaurantBearerBO.BearerId > 0)
                        {
                            entityBO.BearerId = restaurantBearerBO.BearerId;
                        }
                    }

                    //entityBO.BearerId = currentUserInformationBO.UserInfoId;
                    entityBO.CostCenterId = currentUserInformationBO.WorkingCostCenterId;
                    entityBO.CreatedBy = currentUserInformationBO.UserInfoId;

                    //this.txtTableIdInformation.Text = roomAllocationBO.RegistrationId.ToString();
                    //Session["txtTableIdInformation"] = roomAllocationBO.RegistrationId.ToString();
                    //this.txtTableNumberInformation.Text = roomAllocationBO.RoomNumber;
                    //Session["txtTableNumberInformation"] = this.txtTableNumberInformation.Text;
                    //Boolean status = entityDA.SaveKotBillMasterInfo(entityBO, out tmpPkId);
                    //if (status)
                    //{

                    //    this.txtKotIdInformation.Text = tmpPkId.ToString();
                    //    Session["txtKotIdInformation"] = this.txtKotIdInformation.Text;
                    //    Response.Redirect("/Restaurant/frmGuestRoomKotBill.aspx?Kot=RestaurantItemGroup:0");
                    //}







                    this.txtTableIdInformation.Text = roomAllocationBO.RegistrationId.ToString();
                    Session["txtTableIdInformation"] = roomAllocationBO.RegistrationId.ToString();

                    //UserInformationBO currentUserInformationBO = new UserInformationBO();
                    //currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    entityTableBO = entityTableDA.GetRestaurantTableInfoById(currentUserInformationBO.WorkingCostCenterId, "GuestRoom", roomAllocationBO.RegistrationId);

                    if (entityTableBO.StatusId == 1)
                    {
                        this.txtTableNumberInformation.Text = entityTableBO.TableNumber;
                        Session["txtTableNumberInformation"] = this.txtTableNumberInformation.Text;
                        Boolean status = entityDA.SaveKotBillMasterInfo(entityBO, out tmpPkId);
                        if (status)
                        {

                            this.txtKotIdInformation.Text = tmpPkId.ToString();
                            Session["txtKotIdInformation"] = this.txtKotIdInformation.Text;
                            //Response.Redirect("/Restaurant/frmGuestRoomKotBill.aspx?Kot=RestaurantItemGroup:0");
                            Response.Redirect("/POS/frmGuestRoomKotBill.aspx?Kot=RestaurantItemCategory:0");
                        }
                    }
                    else if (entityTableBO.StatusId == 2)
                    {
                        this.txtTableNumberInformation.Text = entityTableBO.TableNumber;
                        Session["txtTableNumberInformation"] = this.txtTableNumberInformation.Text;
                        entityBO = entityDA.GetKotBillMasterInfoByTableId(entityBO.CostCenterId, "GuestRoom", roomAllocationBO.RegistrationId);
                        if (entityBO.KotId > 0)
                        {
                            Session["txtKotIdInformation"] = entityBO.KotId;
                            //Response.Redirect("/Restaurant/frmGuestRoomKotBill.aspx?Kot=RestaurantItemGroup:1");
                            Response.Redirect("/POS/frmGuestRoomKotBill.aspx?Kot=RestaurantItemCategory:0");
                        }
                    }
                }
            }
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmCostCenterChooseForKot.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            if (!isSavePermission)
            {
                Response.Redirect("/Restaurant/Login.aspx");
            }
        }
        private void ReprintKotBill(string roomNumber)
        {
            HMUtility hmUtility = new HMUtility();
            KotBillMasterViewBO entityBO = new KotBillMasterViewBO();
            KotBillMasterDA entityMDA = new KotBillMasterDA();
            KotBillDetailDA entityDA = new KotBillDetailDA();

            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
            roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumber);
            if (roomAllocationBO.RoomId > 0)
            {
                Session["txtTableNumberInformation"] = this.txtTableNumberInformation.Text;
                entityBO = entityMDA.GetKotBillMasterBySourceIdNType(currentUserInformationBO.WorkingCostCenterId, "GuestRoom", roomAllocationBO.RegistrationId);
                if (entityBO != null)
                {
                    if (entityBO.KotId > 0)
                    {
                        //Session["txtTableIdInformation"] = roomNumber;
                        txtTableNumberInformation.Text = roomNumber;
                        Session["txtKotIdInformation"] = entityBO.KotId;
                        txtKotIdInformation.Text = entityBO.KotId.ToString();
                    }

                    PrinterInfoDA da = new PrinterInfoDA();

                    int kotId = Convert.ToInt32(txtKotIdInformation.Text);

                    List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();
                    List<PrinterInfoBO> files = da.GetRestaurentItemTypeInfoByKotIdForReprint(kotId);

                    if (files.Count > 0)
                    {
                        foreach (PrinterInfoBO pinfo in files)
                        {
                            strCostCenterId = pinfo.CostCenterId;
                            strPrinterInfoId = pinfo.PrinterInfoId;
                            strCostCenterId = pinfo.CostCenterId;
                            strCostCenterName = pinfo.CostCenter;
                            txtCompanyName.Text = pinfo.KitchenOrStockName;

                            if (pinfo.DefaultView == "Table")
                            {
                                strCostCenterDefaultView = "Table # ";
                            }
                            else if (pinfo.DefaultView == "Token")
                            {
                                strCostCenterDefaultView = "Token # ";
                            }
                            else if (pinfo.DefaultView == "Room")
                            {
                                strCostCenterDefaultView = "Room # ";
                            }

                            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                            waiterName = currentUserInformationBO.UserName.ToString();

                            if (pinfo.StockType == "StockItem")
                            {
                                entityBOList = entityDA.GetKotOrderSubmitInfo(kotId, strCostCenterId, "StockItem", true);
                            }
                            else
                            {
                                entityBOList = entityDA.GetKotOrderSubmitInfo(kotId, strCostCenterId, "KitchenItem", true).Where(x => x.KitchenId == pinfo.KitchenId && x.PrinterName == pinfo.PrinterName).ToList();
                            }

                            if (entityBOList.Count > 0)
                            {
                                HMCommonSetupBO commonRestaurantBillPrintAndPreview = new HMCommonSetupBO();
                                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                                commonRestaurantBillPrintAndPreview = commonSetupDA.GetCommonConfigurationInfo("RestaurantBillPrintAndPreview", "RestaurantBillPrintAndPreview");

                                if (Convert.ToInt32(commonRestaurantBillPrintAndPreview.SetupValue) == (int)(HMConstants.RestaurantBillPrintAndPreview.DirectPrint))
                                {
                                    PrintReportKot(pinfo, entityBOList, true);
                                }
                            }
                        }

                        Boolean status = entityDA.UpdateKotBillDetailPrintFlagByOrderSubmitKotId(Convert.ToInt32(txtKotIdInformation.Text));
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Please provide Item Information.", AlertType.Warning);
                        this.btnOrderSubmit.Focus();
                        return;
                    }
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, "KOT Date Related Mismatch.", AlertType.Warning);
                    this.btnOrderSubmit.Focus();
                    return;
                }
            }
        }
        [WebMethod]
        public static KotBillDetailBO UpdateIndividualItemDetailInformation(string updateType, int costCenterId, int editId, decimal quantity, string updatedContent)
        {
            KotBillDetailBO entityBO = new KotBillDetailBO();
            KotBillDetailDA entityDA = new KotBillDetailDA();
            KotBillDetailBO srcEntityBO = new KotBillDetailBO();

            RestaurantBuffetItemBO bItemEntityBO = new RestaurantBuffetItemBO();
            RestaurantBuffetItemDA bItemEntityDA = new RestaurantBuffetItemDA();
            RestaurantComboItemBO cItemEntityBO = new RestaurantComboItemBO();
            RestaurantComboItemDA cItemEntityDA = new RestaurantComboItemDA();
            InvItemBO itemEntityBO = new InvItemBO();
            InvItemDA itemEntityDA = new InvItemDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            srcEntityBO = entityDA.GetSrcRestaurantBillDetailInfoByKotDetailId(editId);
            if (srcEntityBO.KotDetailId > 0)
            {
                if (srcEntityBO.ItemType == "BuffetItem")
                {
                    bItemEntityBO = bItemEntityDA.GetRestaurantBuffetInfoById(srcEntityBO.ItemId);
                    entityBO.UnitRate = bItemEntityBO.BuffetPrice;
                    entityBO.Amount = bItemEntityBO.BuffetPrice * Convert.ToInt32(updatedContent);
                    entityBO.ItemUnit = Convert.ToInt32(updatedContent);
                    entityBO.KotId = srcEntityBO.KotId;
                    entityBO.ItemId = srcEntityBO.ItemId;
                }
                else if (srcEntityBO.ItemType == "ComboItem")
                {
                    cItemEntityBO = cItemEntityDA.GetRestaurantComboInfoById(srcEntityBO.ItemId);
                    entityBO.UnitRate = cItemEntityBO.ComboPrice;
                    entityBO.Amount = cItemEntityBO.ComboPrice * Convert.ToInt32(updatedContent);
                    entityBO.ItemUnit = Convert.ToInt32(updatedContent);
                    entityBO.KotId = srcEntityBO.KotId;
                    entityBO.ItemId = srcEntityBO.ItemId;
                }
                else if (srcEntityBO.ItemType == "IndividualItem")
                {
                    if (updateType == "QuantityChange")
                    {
                        KotBillDetailBO kotDetailBO = entityDA.GetSrcRestaurantBillDetailInfoByKotDetailId(editId);
                        entityBO.UnitRate = kotDetailBO.UnitRate; //itemEntityBO.UnitPriceLocal;
                        entityBO.Amount = kotDetailBO.UnitRate * Convert.ToDecimal(updatedContent); //itemEntityBO.UnitPriceLocal * Convert.ToDecimal(updatedContent);
                        entityBO.ItemUnit = Convert.ToDecimal(updatedContent);
                        entityBO.KotId = srcEntityBO.KotId;
                        entityBO.ItemId = srcEntityBO.ItemId;
                    }
                    else if (updateType == "ItemNameChange")
                    {
                        entityBO.ItemName = updatedContent;
                        entityBO.KotId = srcEntityBO.KotId;
                        entityBO.ItemId = srcEntityBO.ItemId;
                    }
                    else if (updateType == "UnitPriceChange")
                    {
                        entityBO.UnitRate = Convert.ToDecimal(updatedContent);
                        entityBO.Amount = quantity * Convert.ToInt32(updatedContent);
                        entityBO.KotId = srcEntityBO.KotId;
                        entityBO.ItemId = srcEntityBO.ItemId;
                    }
                }

                entityBO.KotDetailId = editId;
                entityBO.CreatedBy = userInformationBO.UserInfoId;

                Boolean status = entityDA.UpdateKotBillDetailInfo(0, entityBO, updateType);
            }
            return entityBO;
        }
        [WebMethod]
        public static KotBillDetailBO SaveIndividualItemDetailInformation(int costCenterId, int kotId, int itemId, decimal itemQty)
        {
            KotBillDetailBO entityBO = new KotBillDetailBO();
            KotBillDetailDA entityDA = new KotBillDetailDA();

            InvItemBO itemEntityBO = new InvItemBO();
            InvItemDA itemEntityDA = new InvItemDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            itemEntityBO = itemEntityDA.GetInvItemInfoById(costCenterId, itemId);
            if (itemEntityBO.ItemId > 0)
            {
                entityBO = entityDA.GetKotBillDetailInfoByKotNItemId(costCenterId, kotId, itemId, "IndividualItem");

                if (entityBO.KotDetailId > 0)
                {
                    entityBO.ItemType = "IndividualItem";
                    entityBO.ItemId = itemId;
                    entityBO.ItemUnit = itemQty + 1;
                    entityBO.KotId = kotId;
                    entityBO.UnitRate = itemEntityBO.UnitPriceLocal;
                    entityBO.Amount = itemEntityBO.UnitPriceLocal * entityBO.ItemUnit;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;

                    Boolean status = entityDA.UpdateKotBillDetailInfo(0, entityBO, "QuantityChange");
                }
                else
                {
                    entityBO.ItemType = "IndividualItem";
                    entityBO.ItemId = itemId;
                    entityBO.ItemUnit = 1;
                    entityBO.KotId = kotId;
                    entityBO.UnitRate = itemEntityBO.UnitPriceLocal;
                    entityBO.Amount = itemEntityBO.UnitPriceLocal * entityBO.ItemUnit;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;

                    Boolean status = entityDA.SaveKotBillDetailInfo(entityBO);
                }
            }
            return entityBO;
        }
        [WebMethod]
        public static KotBillDetailBO SaveComboItemDetailInformation(int costCenterId, int kotId, int itemId)
        {
            KotBillDetailBO entityBO = new KotBillDetailBO();
            KotBillDetailDA entityDA = new KotBillDetailDA();

            RestaurantComboItemBO itemEntityBO = new RestaurantComboItemBO();
            RestaurantComboItemDA itemEntityDA = new RestaurantComboItemDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            itemEntityBO = itemEntityDA.GetRestaurantComboInfoById(itemId);
            if (itemEntityBO.ComboId > 0)
            {
                entityBO = entityDA.GetKotBillDetailInfoByKotNItemId(costCenterId, kotId, itemId, "ComboItem");

                if (entityBO.KotDetailId > 0)
                {
                    entityBO.ItemType = "ComboItem";
                    entityBO.ItemId = itemId;
                    entityBO.ItemUnit = entityBO.ItemUnit + 1;
                    entityBO.KotId = kotId;
                    entityBO.UnitRate = itemEntityBO.ComboPrice;
                    entityBO.Amount = itemEntityBO.ComboPrice * entityBO.ItemUnit;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;

                    Boolean status = entityDA.UpdateKotBillDetailInfo(0, entityBO, "QuantityChange");
                }
                else
                {
                    entityBO.ItemType = "ComboItem";
                    entityBO.ItemId = itemId;
                    entityBO.ItemUnit = 1;
                    entityBO.KotId = kotId;
                    entityBO.UnitRate = itemEntityBO.ComboPrice;
                    entityBO.Amount = itemEntityBO.ComboPrice * entityBO.ItemUnit;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;

                    Boolean status = entityDA.SaveKotBillDetailInfo(entityBO);
                }
            }
            return entityBO;
        }
        [WebMethod]
        public static KotBillDetailBO SaveBuffetItemDetailInformation(int costCenterId, int kotId, int itemId)
        {
            KotBillDetailBO entityBO = new KotBillDetailBO();
            KotBillDetailDA entityDA = new KotBillDetailDA();

            RestaurantBuffetItemBO itemEntityBO = new RestaurantBuffetItemBO();
            RestaurantBuffetItemDA itemEntityDA = new RestaurantBuffetItemDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            itemEntityBO = itemEntityDA.GetRestaurantBuffetInfoById(itemId);
            if (itemEntityBO.BuffetId > 0)
            {
                entityBO = entityDA.GetKotBillDetailInfoByKotNItemId(costCenterId, kotId, itemId, "BuffetItem");

                if (entityBO.KotDetailId > 0)
                {
                    entityBO.ItemType = "BuffetItem";
                    entityBO.ItemId = itemId;
                    entityBO.ItemUnit = entityBO.ItemUnit + 1;
                    entityBO.KotId = kotId;
                    entityBO.UnitRate = itemEntityBO.BuffetPrice;
                    entityBO.Amount = itemEntityBO.BuffetPrice * entityBO.ItemUnit;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;

                    Boolean status = entityDA.UpdateKotBillDetailInfo(0, entityBO, "QuantityChange");
                }
                else
                {
                    entityBO.ItemType = "BuffetItem";
                    entityBO.ItemId = itemId;
                    entityBO.ItemUnit = 1;
                    entityBO.KotId = kotId;
                    entityBO.UnitRate = itemEntityBO.BuffetPrice;
                    entityBO.Amount = itemEntityBO.BuffetPrice * entityBO.ItemUnit;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;

                    Boolean status = entityDA.SaveKotBillDetailInfo(entityBO);
                }
            }
            return entityBO;
        }
        [WebMethod(EnableSession = true)]
        public static string GenerateTableWiseItemGridInformation(int costCenterId, int tableId, int kotId)
        {
            string strTable = "";
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (userInformationBO != null)
            {
                Boolean isWMDeletePermission = false;

                ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
                objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmCostCenterChooseForKot.ToString());
                isWMDeletePermission = objectPermissionBO.IsDeletePermission;

                KotBillDetailDA entityDA = new KotBillDetailDA();
                List<KotBillDetailBO> files = entityDA.GetKotBillDetailInfoByKotNBearerId(costCenterId, "GuestRoom", tableId, kotId);

                Boolean isChangedExist = false;
                foreach (KotBillDetailBO drIsChanged in files)
                {
                    if (drIsChanged.IsChanged)
                    {
                        isChangedExist = true;
                        break;
                    }
                }


                strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Item Name</th><th align='left' scope='col'>Unit</th><th align='left' scope='col'>Unit Rate</th><th align='left' scope='col'>Total</th><th align='center' scope='col'>Action</th></tr>";
                int counter = 0;
                foreach (KotBillDetailBO dr in files)
                {
                    counter++;
                    if (counter % 2 == 0)
                    {
                        // It's even
                        strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 40%;'>" + dr.ItemName + "</td>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:White;'><td align='left' style='width: 40%;'>" + dr.ItemName + "</td>";
                    }

                    strTable += "<td align='left' style='width: 15%;'>" + dr.ItemUnit + "</td>";
                    strTable += "<td align='left' style='width: 15%;'>" + dr.UnitRate + "</td>";
                    strTable += "<td align='left' style='width: 15%;'>" + Math.Round((dr.ItemUnit * dr.UnitRate), 2) + "</td>";

                    strTable += "<td align='center' style='width: 20%;'>";

                    if (!dr.PrintFlag)
                    {
                        strTable += "<img src='../Images/edit.png' title='Edit'  onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.ItemId + "," + counter + ")' alt='Edit Information' border='0' />";
                        strTable += "&nbsp;<img src='../Images/delete.png' title='Delete'  onClick='javascript:return PerformDeleteAction(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + ")' alt='Delete Information' border='0' />";
                        strTable += "&nbsp;<img src='../Images/remarksadd.png' title='Add Remarks' onClick='javascript:return AddItemWiseRemarks(" + dr.KotId + "," + dr.ItemId + "," + dr.KotDetailId + ")' alt='Add Remarks' border='0' />";
                    }
                    else
                    {
                        if (!isChangedExist)
                        {
                            strTable += "&nbsp;<img src='../Images/select.png' title='Submitted' alt='Item Submitted' border='0' />";
                        }

                        strTable += "<img src='../Images/edit.png' title='Edit'  onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.ItemId + "," + counter + ")' alt='Edit Information' border='0' />";
                        if (isWMDeletePermission)
                        {
                            strTable += "&nbsp;<img src='../Images/delete.png' title='Delete'  onClick='javascript:return PerformDeleteAction(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + ")' alt='Delete Information' border='0' />";
                        }
                        strTable += "&nbsp;<img src='../Images/remarksadd.png' title='Add Remarks' onClick='javascript:return AddItemWiseRemarks(" + dr.KotId + "," + dr.ItemId + "," + dr.KotDetailId + ")' alt='Add Remarks' border='0' />";
                    }

                    //strTable += "</td></tr>";

                    strTable += "</td>";
                    strTable += "<td align='left' style='display:none;'>" + dr.ItemId + "</td>";
                    strTable += "</tr>";

                    if (dr.ItemType == "BuffetItem")
                    {
                        string strBuffetDetail = string.Empty;
                        List<RestaurantBuffetDetailBO> buffetDetailListBO = new List<RestaurantBuffetDetailBO>();
                        RestaurantBuffetDetailDA buffetDetailDA = new RestaurantBuffetDetailDA();
                        strTable += "<tr><td align='left' style='width: 40%; padding-left:20px;' colspan='5'>" + strBuffetDetail + "</td>";
                    }

                    if (dr.ItemType == "ComboItem")
                    {
                        string strComboDetail = string.Empty;
                        List<InvItemDetailsBO> ownerDetailListBO = new List<InvItemDetailsBO>();
                        InvItemDetailsDA ownerDetailDA = new InvItemDetailsDA();

                        ownerDetailListBO = ownerDetailDA.GetInvItemDetailsByItemId(dr.ItemId);
                        foreach (InvItemDetailsBO drDetail in ownerDetailListBO)
                        {
                            int tmpItemUnit = Convert.ToInt32(drDetail.ItemUnit);
                            strComboDetail += ", " + drDetail.ItemName + "(" + tmpItemUnit + ")";
                        }
                        strComboDetail = strComboDetail.Substring(2, strComboDetail.Length - 2);
                        strTable += "<tr><td align='left' style='width: 40%; padding-left:20px;' colspan='5'>" + strComboDetail + "</td>";
                    }
                }
                strTable += "</table>";
                if (strTable == "")
                {
                    strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
                }
            }
            return strTable;
        }
        [WebMethod]
        public static ReturnInfo DeleteData(int kotDetailsId, int kotId, int itemId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                KotBillDetailDA kotDa = new KotBillDetailDA();
                Boolean status = kotDa.DeleteKotBillDetail(kotDetailsId, kotId, itemId, userInformationBO.UserInfoId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
               
            }

            return rtninf;
        }
        //[WebMethod]
        //public static string DeleteData(int sEmpId)
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        HMCommonDA hmCommonDA = new HMCommonDA();

        //        Boolean status = hmCommonDA.DeleteInfoById("RestaurantKotBillDetail", "KotDetailId", sEmpId);
        //        if (status)
        //        {
        //            result = "success";
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        //lblMessage.Text = "Data Deleted Failed.";
                    //throw ex;
        //    }

        //    return result;
        //}
        protected void btnBackPreviousPage_Click(object sender, ImageClickEventArgs e)
        {
            if (this.currentButtonFormName == "TableAllocation")
            {
                Response.Redirect("/POS/frmGuestRoomKotBill.aspx?Kot=RoomAllocation");
            }
            else if (this.currentButtonFormName == "BillCreateForTable")
            {
                Response.Redirect("/POS/frmGuestRoomKotBill.aspx?Kot=RoomAllocation");
            }
            else
            {
                string queryStringId = Request.QueryString["Kot"];
                if (Convert.ToInt32(queryStringId.Split(':')[1].ToString()) == 0)
                {
                    Response.Redirect("/POS/frmGuestRoomKotBill.aspx?Kot=RoomAllocation");
                }
                else
                {
                    InvCategoryDA matrixDA = new InvCategoryDA();
                    InvCategoryBO matrixBO = new InvCategoryBO();
                    matrixBO = matrixDA.GetInvCategoryInfoById(Convert.ToInt32(queryStringId.Split(':')[1].ToString()));
                    if (matrixBO.CategoryId > 0)
                    {
                        if (matrixBO.CategoryId == matrixBO.AncestorId)
                        {
                            Response.Redirect("/POS/frmGuestRoomKotBill.aspx?Kot=RestaurantItemCategory:0");
                        }
                        else
                        {
                            Response.Redirect("/POS/frmGuestRoomKotBill.aspx?Kot=RestaurantItemCategory:" + (matrixBO.AncestorId).ToString());
                        }
                    }
                }
            }
            /*
            switch (this.currentButtonFormName)
            {
                case "TableAllocation":
                    Response.Redirect("/Restaurant/frmGuestRoomKotBill.aspx?Kot=RoomAllocation");
                    break;
                case "BillCreateForTable":
                    Response.Redirect("/Restaurant/frmGuestRoomKotBill.aspx?Kot=RoomAllocation");
                    break;
                case "RestaurantItemGroup":
                    Response.Redirect("/Restaurant/frmGuestRoomKotBill.aspx?Kot=RoomAllocation");
                    break;
                case "RestaurantComboItem":
                    Response.Redirect("/Restaurant/frmGuestRoomKotBill.aspx?Kot=RestaurantItemGroup:1");
                    break;
                case "RestaurantBuffetItem":
                    Response.Redirect("/Restaurant/frmGuestRoomKotBill.aspx?Kot=RestaurantItemGroup:1");
                    break;
                case "RestaurantItemCategory":
                    Response.Redirect("/Restaurant/frmGuestRoomKotBill.aspx?Kot=RestaurantItemGroup:1");
                    break;
                case "RestaurantItem":
                    Response.Redirect("/Restaurant/frmGuestRoomKotBill.aspx?Kot=RestaurantItemCategory:2");
                    break;
            }
             */
        }
        protected void btnOrderSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                KotBillDetailDA entityDA = new KotBillDetailDA();
                PrinterInfoDA da = new PrinterInfoDA();

                bool rePrintStatus = false;
                int kotId = Convert.ToInt32(txtKotIdInformation.Text);

                List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();
                List<PrinterInfoBO> files = da.GetRestaurentItemTypeInfoByKotId(kotId);
                if (files.Count > 0)
                {
                    foreach (PrinterInfoBO pinfo in files)
                    {
                        strCostCenterId = pinfo.CostCenterId;
                        strPrinterInfoId = pinfo.PrinterInfoId;
                        strCostCenterId = pinfo.CostCenterId;
                        strCostCenterName = pinfo.CostCenter;
                        txtCompanyName.Text = pinfo.KitchenOrStockName;

                        if (pinfo.DefaultView == "Table")
                        {
                            strCostCenterDefaultView = "Table No.";
                        }
                        else if (pinfo.DefaultView == "Token")
                        {
                            strCostCenterDefaultView = "Token No.";
                        }
                        else if (pinfo.DefaultView == "Room")
                        {
                            strCostCenterDefaultView = "Room No.";
                        }

                        UserInformationBO currentUserInformationBO = new UserInformationBO();
                        HMUtility hmUtility = new HMUtility();
                        currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                        waiterName = currentUserInformationBO.UserName.ToString();

                        if (pinfo.StockType == "StockItem")
                        {
                            //if (pinfo.IsChanged == false)
                            //{
                            entityBOList = entityDA.GetKotOrderSubmitInfo(kotId, strCostCenterId, "StockItem", false);
                            //}
                            //else
                            //{
                            //    entityBOList = entityDA.GetKotOrderSubmitInfo(kotId, strCostCenterId, "StockItem", true);
                            //}
                        }
                        else
                        {
                            //if (pinfo.IsChanged == false)
                            //{
                            entityBOList = entityDA.GetKotOrderSubmitInfo(kotId, strCostCenterId, "KitchenItem", false).Where(x => x.KitchenId == pinfo.KitchenId && x.PrinterName == pinfo.PrinterName).ToList();
                            //}
                            //else
                            //{
                            //    entityBOList = entityDA.GetKotOrderSubmitInfo(kotId, strCostCenterId, "KitchenItem", true);
                            //}
                        }

                        if (entityBOList.Count > 0)
                        {
                            //var v = entityBOList.Where(s => s.PrintFlag == false).ToList();

                            //if (v.Count > 0)
                            if (entityBOList.Count > 0)
                            {
                                rePrintStatus = true;
                                PrintReportKot(pinfo, entityBOList, false);
                            }
                            else
                            {
                                rePrintStatus = false;
                            }
                        }
                    }
                    bool status = false;

                    if (rePrintStatus)
                    {
                        status = entityDA.UpdateKotBillDetailPrintFlagByOrderSubmitKotId(Convert.ToInt32(txtKotIdInformation.Text));
                    }

                    if (status && rePrintStatus)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "KOT Successfully Submitted.", AlertType.Success);
                    }
                    else if (!status && !rePrintStatus)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "KOT already submitted.", AlertType.Information);
                    }
                }
                else
                {
                    if (hfIsItemAdded.Value == "1")
                        CommonHelper.AlertInfo(innboardMessage, "KOT already submitted.", AlertType.Information);
                    else if (hfIsItemAdded.Value == "0")
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Please provide Item Information.", AlertType.Warning);
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, "KOT is not Submitted Successfully.", AlertType.Error);
                
            }

            hfIsItemAdded.Value = "0";
            /*
            int kotId = Convert.ToInt32(this.txtKotIdInformation.Text);
            PrinterInfoDA da = new PrinterInfoDA();
            List<PrinterInfoBO> files = da.GetRestaurentItemTypeInfoByKotId(kotId);
            if (files.Count > 0)
            {
                foreach (PrinterInfoBO row in files)
                {
                    strPrinterInfoId = row.PrinterInfoId;
                    strCostCenterId = row.CostCenterId;
                    strCostCenterName = row.CostCenter;

                    ////-------Old Copy------------------
                    //using (PrintDocument doc = new PrintDocument())
                    //{
                    //    doc.DocumentName = "KotOrderSubmit";
                    //    doc.DefaultPageSettings.Landscape = false;
                    //    doc.PrintPage += new PrintPageEventHandler(pdoc_PrintPage);
                    //    //doc.PrinterSettings.PrinterName = "Samsung ML-1860 Series";
                    //    doc.PrinterSettings.PrinterName = row.PrinterName;
                    //    doc.PrintController = new StandardPrintController();
                    //    doc.Print();
                    //}

                    if (row.StockType == "StockItem")
                    {
                        using (PrintDocument doc = new PrintDocument())
                        {
                            doc.DocumentName = "KotOrderSubmit";
                            doc.DefaultPageSettings.Landscape = false;
                            doc.PrintPage += new PrintPageEventHandler(PrintActionForStockItem);
                            //doc.PrinterSettings.PrinterName = "Samsung ML-1860 Series";
                            doc.PrinterSettings.PrinterName = row.PrinterName;
                            doc.PrintController = new StandardPrintController();
                            doc.Print();
                        }
                    }
                    else
                    {
                        using (PrintDocument doc = new PrintDocument())
                        {
                            doc.DocumentName = "KotOrderSubmit";
                            doc.DefaultPageSettings.Landscape = false;
                            doc.PrintPage += new PrintPageEventHandler(PrintActionForKitchenItem);
                            //doc.PrinterSettings.PrinterName = "Samsung ML-1860 Series";
                            doc.PrinterSettings.PrinterName = row.PrinterName;
                            doc.PrintController = new StandardPrintController();
                            doc.Print();
                        }
                    }
                }

                KotBillDetailDA entityDA = new KotBillDetailDA();
                Boolean status = entityDA.UpdateKotBillDetailPrintFlagByOrderSubmitKotId(kotId);
            }
            else
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please provide Item Information.";
                this.btnOrderSubmit.Focus();
                return;
            }
            */
        }
        private void PrintActionForStockItem(object sender, PrintPageEventArgs e)
        {
            if (strCostCenterId > 0)
            {
                List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();
                KotBillDetailDA entityDA = new KotBillDetailDA();

                int kotId = Convert.ToInt32(this.txtKotIdInformation.Text);
                int costcenterId = strCostCenterId;

                entityBOList = entityDA.GetKotOrderSubmitInfo(kotId, costcenterId, "StockItem", false);

                if (entityBOList.Count > 0)
                {

                    Graphics graphics = e.Graphics;
                    Font font = new Font("Courier New", 10);
                    float fontHeight = font.GetHeight();
                    int startX = 50;
                    int startY = 55;
                    int Offset = 40;
                    graphics.DrawString(this.txtCompanyName.Text, new Font("Courier New", 14),
                                        new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset = Offset + 25;
                    string costCenterNameText = "Cost Center";
                    string costCenterName = strCostCenterName;
                    graphics.DrawString(costCenterNameText.PadRight(8, ' ') + " : " + costCenterName, new Font("Courier New", 10),
                                new SolidBrush(Color.Black), startX, startY + Offset);

                    Offset = Offset + 20;
                    string tableNameText = "Table No";
                    string tableName = this.txtTableNumberInformation.Text;
                    graphics.DrawString(tableNameText.PadRight(8, ' ') + " : " + tableName, new Font("Courier New", 10),
                                new SolidBrush(Color.Black), startX, startY + Offset);

                    Offset = Offset + 20;
                    string kotNoText = "Kot No";
                    string kotNo = this.txtKotIdInformation.Text;
                    graphics.DrawString(kotNoText.PadRight(8, ' ') + " : " + kotNo, new Font("Courier New", 10),
                                 new SolidBrush(Color.Black), startX, startY + Offset);

                    Offset = Offset + 20;
                    string kotDateText = "Kot Date";
                    DateTime dateTime = DateTime.Now;
                    //string kotDate = dateTime.ToString("MM/dd/yyyy");
                    string kotDate = String.Format("{0:f}", dateTime);
                    graphics.DrawString(kotDateText.PadRight(8, ' ') + " : " + kotDate, new Font("Courier New", 10),
                                 new SolidBrush(Color.Black), startX, startY + Offset);


                    Offset = Offset + 20;
                    String underLine = "-----------------------------------------------";
                    graphics.DrawString(underLine, new Font("Courier New", 10),
                             new SolidBrush(Color.Black), startX, startY + Offset);

                    foreach (KotBillDetailBO row in entityBOList)
                    {
                        Offset = Offset + 20;
                        string ItemName = row.ItemName;
                        string ItemQuantity = row.ItemUnit.ToString();
                        graphics.DrawString(ItemName.PadRight(35, ' ') + " : " + ItemQuantity, new Font("Courier New", 10),
                                 new SolidBrush(Color.Black), startX, startY + Offset);
                    }

                    Offset = Offset + 20;
                    underLine = "-----------------------------------------------";
                    graphics.DrawString(underLine, new Font("Courier New", 10),
                             new SolidBrush(Color.Black), startX, startY + Offset);

                    Offset = Offset + 20;
                    //EmployeeBO currentUserInformationBO = new EmployeeBO();
                    //currentUserInformationBO = hmUtility.GetCurrentRestaurantApplicationUserInfo();
                    //String DrawnBy = currentUserInformationBO.DisplayName.ToString();

                    UserInformationBO currentUserInformationBO = new UserInformationBO();
                    currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    String DrawnBy = currentUserInformationBO.UserName.ToString();
                    graphics.DrawString("Waiter Name: " + DrawnBy, new Font("Courier New", 10),
                             new SolidBrush(Color.Black), startX, startY + Offset);

                    // Special Remarks Information ----------------------------
                    if (entityBOList != null)
                    {
                        if (entityBOList.Count > 0)
                        {
                            Offset = Offset + 20;
                            graphics.DrawString("Special Remarks: " + entityBOList[0].Remarks, new Font("Courier New", 10),
                                     new SolidBrush(Color.Black), startX, startY + Offset);
                        }
                    }
                }
            }
        }
        private void PrintActionForKitchenItem(object sender, PrintPageEventArgs e)
        {
            if (strCostCenterId > 0)
            {
                List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();
                KotBillDetailDA entityDA = new KotBillDetailDA();

                int kotId = Convert.ToInt32(this.txtKotIdInformation.Text);
                int costcenterId = strCostCenterId;

                entityBOList = entityDA.GetKotOrderSubmitInfo(kotId, costcenterId, "KitchenItem", false);

                if (entityBOList.Count > 0)
                {
                    Graphics graphics = e.Graphics;
                    Font font = new Font("Courier New", 10);
                    float fontHeight = font.GetHeight();
                    int startX = 50;
                    int startY = 55;
                    int Offset = 40;
                    graphics.DrawString(this.txtCompanyName.Text, new Font("Courier New", 14),
                                        new SolidBrush(Color.Black), startX, startY + Offset);

                    Offset = Offset + 25;
                    string costCenterNameText = "Cost Center";
                    string costCenterName = strCostCenterName;
                    graphics.DrawString(costCenterNameText.PadRight(8, ' ') + " : " + costCenterName, new Font("Courier New", 10),
                                new SolidBrush(Color.Black), startX, startY + Offset);

                    Offset = Offset + 20;
                    string tableNameText = "Table No";
                    string tableName = this.txtTableNumberInformation.Text;
                    graphics.DrawString(tableNameText.PadRight(8, ' ') + " : " + tableName, new Font("Courier New", 10),
                                new SolidBrush(Color.Black), startX, startY + Offset);

                    Offset = Offset + 20;
                    string kotNoText = "Kot No";
                    string kotNo = this.txtKotIdInformation.Text;
                    graphics.DrawString(kotNoText.PadRight(8, ' ') + " : " + kotNo, new Font("Courier New", 10),
                                 new SolidBrush(Color.Black), startX, startY + Offset);

                    Offset = Offset + 20;
                    string kotDateText = "Kot Date";
                    DateTime dateTime = DateTime.Now;
                    //string kotDate = dateTime.ToString("MM/dd/yyyy");
                    string kotDate = String.Format("{0:f}", dateTime);
                    graphics.DrawString(kotDateText.PadRight(8, ' ') + " : " + kotDate, new Font("Courier New", 10),
                                 new SolidBrush(Color.Black), startX, startY + Offset);

                    Offset = Offset + 20;
                    String underLine = "-----------------------------------------------";
                    graphics.DrawString(underLine, new Font("Courier New", 10),
                             new SolidBrush(Color.Black), startX, startY + Offset);

                    foreach (KotBillDetailBO row in entityBOList)
                    {
                        Offset = Offset + 20;
                        string ItemName = row.ItemName;
                        string ItemQuantity = row.ItemUnit.ToString();
                        graphics.DrawString(ItemName.PadRight(35, ' ') + " : " + ItemQuantity, new Font("Courier New", 10),
                                 new SolidBrush(Color.Black), startX, startY + Offset);
                    }

                    Offset = Offset + 20;
                    underLine = "-----------------------------------------------";
                    graphics.DrawString(underLine, new Font("Courier New", 10),
                             new SolidBrush(Color.Black), startX, startY + Offset);

                    Offset = Offset + 20;
                    //EmployeeBO currentUserInformationBO = new EmployeeBO();
                    //currentUserInformationBO = hmUtility.GetCurrentRestaurantApplicationUserInfo();
                    //String DrawnBy = currentUserInformationBO.DisplayName.ToString();
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    String DrawnBy = userInformationBO.UserName.ToString();
                    graphics.DrawString("Waiter Name: " + DrawnBy, new Font("Courier New", 10),
                             new SolidBrush(Color.Black), startX, startY + Offset);

                    // Special Remarks Information ----------------------------
                    if (entityBOList != null)
                    {
                        if (entityBOList.Count > 0)
                        {
                            Offset = Offset + 20;
                            graphics.DrawString("Special Remarks: " + entityBOList[0].Remarks, new Font("Courier New", 10),
                                     new SolidBrush(Color.Black), startX, startY + Offset);
                        }
                    }
                }
            }
        }
        //Order Submit Information--------------------------------
        private int strPrinterInfoId = 0;
        private int strCostCenterId = 0;
        private string strCostCenterName = string.Empty;
        private string strPrinterName = string.Empty;
        void pdoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (strCostCenterId > 0)
            {

                List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();
                KotBillDetailDA entityDA = new KotBillDetailDA();

                int kotId = Convert.ToInt32(this.txtKotIdInformation.Text);
                int costcenterId = strCostCenterId;

                entityBOList = entityDA.GetKotOrderSubmitInfoByKotIdNCostCenterId(kotId, costcenterId);

                if (entityBOList.Count > 0)
                {
                    Graphics graphics = e.Graphics;
                    Font font = new Font("Courier New", 10);
                    float fontHeight = font.GetHeight();
                    int startX = 50;
                    int startY = 55;
                    int Offset = 40;
                    graphics.DrawString("Innboard Restaurant Management", new Font("Courier New", 14),
                                        new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset = Offset + 20;
                    string tableNameText = "Table No";
                    string tableName = this.txtTableNumberInformation.Text;
                    graphics.DrawString(tableNameText.PadRight(8, ' ') + " : " + tableName, new Font("Courier New", 10),
                                new SolidBrush(Color.Black), startX, startY + Offset);

                    Offset = Offset + 20;
                    string kotNoText = "Kot No";
                    string kotNo = this.txtKotIdInformation.Text;
                    graphics.DrawString(kotNoText.PadRight(8, ' ') + " : " + kotNo, new Font("Courier New", 10),
                                 new SolidBrush(Color.Black), startX, startY + Offset);

                    Offset = Offset + 20;
                    string kotDateText = "Kot Date";
                    DateTime dateTime = DateTime.Now;
                    //string kotDate = dateTime.ToString("MM/dd/yyyy");
                    string kotDate = String.Format("{0:f}", dateTime);
                    graphics.DrawString(kotDateText.PadRight(8, ' ') + " : " + kotDate, new Font("Courier New", 10),
                                 new SolidBrush(Color.Black), startX, startY + Offset);


                    Offset = Offset + 20;
                    String underLine = "--------------------------------------------";
                    graphics.DrawString(underLine, new Font("Courier New", 10),
                             new SolidBrush(Color.Black), startX, startY + Offset);

                    foreach (KotBillDetailBO row in entityBOList)
                    {
                        Offset = Offset + 20;
                        string ItemName = row.ItemName;
                        string ItemQuantity = row.ItemUnit.ToString();
                        graphics.DrawString(ItemName.PadRight(35, ' ') + " : " + ItemQuantity, new Font("Courier New", 10),
                                 new SolidBrush(Color.Black), startX, startY + Offset);
                    }

                    Offset = Offset + 20;
                    underLine = "--------------------------------------------";
                    graphics.DrawString(underLine, new Font("Courier New", 10),
                             new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset = Offset + 20;

                    UserInformationBO currentUserInformationBO = new UserInformationBO();
                    currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    String DrawnBy = currentUserInformationBO.UserName.ToString();
                    graphics.DrawString("Waiter Name: " + DrawnBy, new Font("Courier New", 10),
                             new SolidBrush(Color.Black), startX, startY + Offset);

                    // Special Remarks Information ----------------------------
                    if (entityBOList != null)
                    {
                        if (entityBOList.Count > 0)
                        {
                            Offset = Offset + 20;
                            graphics.DrawString("Special Remarks: " + entityBOList[0].Remarks, new Font("Courier New", 10),
                                     new SolidBrush(Color.Black), startX, startY + Offset);
                        }
                    }
                }
            }
        }
        private void LoadCompanyInformation()
        {
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            if (files != null)
            {
                if (files.Count > 0)
                {
                    this.txtCompanyName.Text = files[0].CompanyName;
                    this.txtCompanyAddress.Text = files[0].CompanyAddress;
                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        this.txtCompanyWeb.Text = files[0].WebAddress;
                    }
                    else
                    {
                        this.txtCompanyWeb.Text = files[0].ContactNumber;
                    }
                }
            }

        }
        protected void imgBtnTableWiseKotBill_Click(object sender, EventArgs e)
        {
            Response.Redirect("/POS/frmKotBillMaster.aspx?Kot=TableAllocation");
        }
        [WebMethod]
        public static KotBillMasterBO UpdateTablePaxInformation(int costCenterId, int paxQuantity)
        {
            KotBillMasterBO entityBO = new KotBillMasterBO();
            if (costCenterId > 0)
            {
                KotBillMasterDA entityDA = new KotBillMasterDA();
                RestaurantTableBO tableBO = new RestaurantTableBO();
                RestaurantTableDA tableDA = new RestaurantTableDA();
                tableBO = tableDA.GetRestaurantTableInfoByTableNumber(System.Web.HttpContext.Current.Session["txtTableNumberInformation"].ToString());
                int kotId = Convert.ToInt32(System.Web.HttpContext.Current.Session["txtKotIdInformation"].ToString());

                if (tableBO.TableId > 0)
                {
                    Boolean status = entityDA.UpdateKotBillMasterPaxInfo(kotId, costCenterId, paxQuantity);
                }
            }
            return entityBO;
        }
        [WebMethod]
        public static KotBillMasterBO UpdateTableSpecialRemarksInformation(int costCenterId, string remarks)
        {
            KotBillMasterBO entityBO = new KotBillMasterBO();
            if (costCenterId > 0)
            {
                KotBillMasterDA entityDA = new KotBillMasterDA();
                RestaurantTableBO tableBO = new RestaurantTableBO();
                RestaurantTableDA tableDA = new RestaurantTableDA();
                tableBO = tableDA.GetRestaurantTableInfoByTableNumber(System.Web.HttpContext.Current.Session["txtTableNumberInformation"].ToString());
                int kotId = Convert.ToInt32(System.Web.HttpContext.Current.Session["txtKotIdInformation"].ToString());

                Boolean status = false;
                if (tableBO.TableId > 0)
                {
                    status = entityDA.UpdateKotBillMasterRemarksInfo(kotId, costCenterId, tableBO.TableId, remarks);
                }
                else
                {
                    status = entityDA.UpdateKotBillMasterRemarksInfo(kotId, costCenterId, 0, remarks);
                }
            }
            return entityBO;
        }
        public void PrintReportKot(PrinterInfoBO files, List<KotBillDetailBO> entityBOList, Boolean IsReprint)
        {
            try
            {
                Double HeightInInch = 11;
                Double WidthInInch = 3.5;

                LocalReport report = new LocalReport();
                report.ReportPath = HttpContext.Current.Server.MapPath(@"~/POS/Reports/Rdlc/rptKotBill.rdlc");
                report.EnableExternalImages = true;
                report.EnableHyperlinks = true;

                DateTime dateTime = DateTime.Now;
                string kotDate = String.Format("{0:f}", dateTime);


                int kotId = 0;
                int paxQuantity = 1;

                foreach (KotBillDetailBO row in entityBOList)
                {
                    kotId = row.KotId;
                    if (row.PaxQuantity != 0)
                    {
                        paxQuantity = row.PaxQuantity;
                    }
                }

                string reportTitle = string.Empty;
                reportTitle = "KOT";
                if (kotId > 0)
                {
                    int updatedDataCount = 0;
                    KotBillDetailDA entityDA = new KotBillDetailDA();
                    updatedDataCount = entityDA.GetKotUpdatedDataCountInfo(kotId);
                    if (updatedDataCount > 0)
                    {
                        //reportTitle = "KOT (Updated)";
                        reportTitle = "KOT";
                    }
                    else
                    {
                        if (IsReprint)
                        {
                            reportTitle = "KOT (Reprint)";
                        }
                        else
                        {
                            reportTitle = "KOT";
                        }
                    }
                }

                ReportParameter[] parms = new ReportParameter[9];
                parms[0] = new ReportParameter("ReportTitle", reportTitle);
                parms[1] = new ReportParameter("CostCenter", strCostCenterName);
                parms[2] = new ReportParameter("SourceName", strCostCenterDefaultView);
                parms[3] = new ReportParameter("TableNo", txtTableNumberInformation.Text);
                //parms[4] = new ReportParameter("KotNo", txtKotIdInformation.Text);
                parms[4] = new ReportParameter("KotNo", txtKotIdInformation.Text + "   Pax : " + paxQuantity.ToString());
                parms[5] = new ReportParameter("KotDate", kotDate);
                parms[6] = new ReportParameter("WaiterName", waiterName);
                parms[7] = new ReportParameter("SpecialRemarks", entityBOList[0].Remarks);
                parms[8] = new ReportParameter("RestaurantName", txtCompanyName.Text);

                report.SetParameters(parms);

                PermissionSet permissions = new PermissionSet(PermissionState.Unrestricted);
                report.SetBasePermissionsForSandboxAppDomain(permissions);

                List<KotBillDetailBO> kotOrderSubmitEntityBOList = entityBOList.Where(x => x.ItemUnit > 0 && x.IsChanged == false).ToList();
                List<KotBillDetailBO> changedOrEditedEntityBOList = entityBOList.Where(x => x.IsChanged == true).ToList();
                List<KotBillDetailBO> voidOrDeletedItemEntityBOList = entityBOList.Where(x => x.ItemUnit < 0).ToList();

                report.DataSources.Add(new ReportDataSource("KotOrderSubmit", kotOrderSubmitEntityBOList));
                report.DataSources.Add(new ReportDataSource("ChangedOrEditedItem", changedOrEditedEntityBOList));
                report.DataSources.Add(new ReportDataSource("VoidOrDeletedItem", voidOrDeletedItemEntityBOList));

                ReportDirectPrinter print = new ReportDirectPrinter();
                print.PrintDefaultPage(report, files.PrinterName, WidthInInch, HeightInInch);

                //print.PrintDefaultPage(report, "");//Samsung ML-1860 Series
                //print.PrintWithCustomPage(report, 10, HeightInInch, files.PrinterName);
                //Export(report);
                //Print(files[0].PrinterName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [WebMethod]
        public static SpecialRemarksDetailsViewBO GetSpecialRemarksDetails(int kotId, int itemId)
        {
            InvItemSpecialRemarksDA specialRemarkDa = new InvItemSpecialRemarksDA();
            List<InvItemSpecialRemarksBO> specialRemarks = new List<InvItemSpecialRemarksBO>();

            RestaurantKotSpecialRemarksDetailDA kotRemarksDa = new RestaurantKotSpecialRemarksDetailDA();
            List<RestaurantKotSpecialRemarksDetailBO> kotRemarks = new List<RestaurantKotSpecialRemarksDetailBO>();

            SpecialRemarksDetailsViewBO remarksDetailsView = new SpecialRemarksDetailsViewBO();

            specialRemarks = specialRemarkDa.GetActiveInvItemSpecialRemarksInfo();
            kotRemarks = kotRemarksDa.GetInvItemSpecialRemarksInfoById(kotId, itemId);

            remarksDetailsView.KotRemarks = kotRemarks;
            remarksDetailsView.ItemSpecialRemarks = specialRemarks;

            return remarksDetailsView;
        }
        [WebMethod]
        public static ReturnInfo SaveKotSpecialRemarks(List<RestaurantKotSpecialRemarksDetailBO> kotSRemarksDetail, List<RestaurantKotSpecialRemarksDetailBO> kotSRemarksDetailTobeDelete, int kotDetailId)
        {
            int tempItemId = 0;
            bool status = false;
            ReturnInfo rtninf = new ReturnInfo();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int waiterId = currentUserInformationBO.UserInfoId;

            RestaurantKotSpecialRemarksDetailDA kotRemarksDa = new RestaurantKotSpecialRemarksDetailDA();

            if (kotSRemarksDetailTobeDelete.Count == 0)
            {
                status = kotRemarksDa.SaveRestaurantKotSpecialRemarksDetail(kotSRemarksDetail, waiterId, kotDetailId, out tempItemId);
            }
            else
            {
                status = kotRemarksDa.UpdateRestaurantKotSpecialRemarksDetail(kotSRemarksDetail, kotSRemarksDetailTobeDelete, waiterId, kotDetailId);
            }

            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
            }
            else
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod(EnableSession = true)]
        public static string GetRestaurantGoToRestaurantBillAction(int kotId)
        {
            string strTable = "";

            KotBillMasterDA entityDA = new KotBillMasterDA();
            KotBillMasterBO entityBOForPax = new KotBillMasterBO();
            entityBOForPax = entityDA.GetKotBillMasterInfoKotId(kotId);
            if (entityBOForPax != null)
            {
                if (entityBOForPax.SourceName == "GuestRoom")
                {
                    if (entityBOForPax.KotId > 0)
                    {
                        int registrationId = entityBOForPax.SourceId;
                        RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();
                        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                        roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(registrationId);
                        if (roomRegistrationBO != null)
                        {
                            if (roomRegistrationBO.RegistrationId > 0)
                            {
                                strTable = roomRegistrationBO.RoomNumber.ToString();
                            }
                        }
                    }
                }

            }

            return strTable;
        }
        [WebMethod]
        public static ReturnInfo LoadOccupiedPossiblePath(string KotNTableNumber, string pageTitle)
        {
            ReturnInfo rtnInfo = new ReturnInfo();
            string strTable = string.Empty;
            string[] costCenterNTableNumber = KotNTableNumber.Split(',');

            int kotId = 0;
            KotBillMasterDA masterDa = new KotBillMasterDA();
            kotId = masterDa.GETKotIDByCostNRoomNumber(costCenterNTableNumber[1], Convert.ToInt32(costCenterNTableNumber[0]));

            PrinterInfoDA da = new PrinterInfoDA();
            List<PrinterInfoBO> files = da.GetRestaurentItemTypeInfoByKotIdForReprint(kotId);
            var v = files.Where(s => s.PrintFlag == true).ToList();

            strTable += "<div class='panel-body'>";
            strTable += "<div class='row'>";
            strTable += "<div style='padding:10px'>";

            strTable += "<div style='float:left;padding-left:10px; padding-bottom:20px;'>";
            strTable += "<input type='button' style='width:115px' value='" + "KOT Continue" + "' class='btn btn-primary'";
            strTable += " onclick=\" location.href='" + "/POS/frmGuestRoomKotBill.aspx?Kot=BillCreateForRoom:" + costCenterNTableNumber[1] + "';\"  />";
            strTable += "</div>";

            strTable += "<div style='float:left;padding-left:10px; padding-bottom:5px;'>";
            strTable += "<input type='button' style='width:115px' value='" + "KOT Reprint" + "' class='btn btn-primary'";
            strTable += " onclick=\" location.href='" + "/POS/frmGuestRoomKotBill.aspx?Kot=KotReprint:" + costCenterNTableNumber[1] + "';\"  />";
            strTable += "</div>";

            strTable += "<div style='float:left;padding-left:10px; padding-bottom:5px;'>";
            strTable += "<input type='button' style='width:115px' value='" + "KOT Preview" + "' class='btn btn-primary'";
            strTable += " onclick=\"javascript:return KotPrintPreview(" + costCenterNTableNumber[1] + ")\"  />";
            strTable += "</div>";

            strTable += "<div style='float:left;padding-left:10px; padding-bottom:5px;'>";
            strTable += "<input type='button' style='width:115px' value='" + "Room Bill" + "' class='btn btn-primary'";
            strTable += " onclick=\" location.href='" + "/POS/frmRestaurantBill.aspx?RoomNumber=" + costCenterNTableNumber[1] + "';\"  />";
            strTable += "</div></div></div>";

            strTable += "<div style='padding-top:20px;'></div>";

            strTable += "<div class='row'>";
            strTable += "<div style='font-weight: bold; display:none; height:60px; border:1px solid #ccc;' id='TableShiftInfo'>";

            strTable += "<div style='margin-left:5px; margin-top:20px;' >";

            rtnInfo.IsSuccess = true;
            rtnInfo.DataStr = strTable;
            return rtnInfo;
        }
    }
}