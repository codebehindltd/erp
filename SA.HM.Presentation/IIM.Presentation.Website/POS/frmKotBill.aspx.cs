using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using System.Web.Services;

using System.IO;
using System.Data;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing;
using System.Security;
using System.Security.Permissions;
using Microsoft.Reporting.WebForms;

using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmKotBill : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isLoadGridInformation = 1;
        private int strPrinterInfoId = 0;
        private int strCostCenterId = 0;
        private string strCostCenterName = string.Empty;
        private string strCostCenterDefaultView = string.Empty;
        private string strPrinterName = string.Empty;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        private string waiterName = string.Empty;
        private string currentButtonFormName = string.Empty;
        private Boolean IsRestaurantOrderSubmitDisable = false;
        UserInformationDA userInformationDA = new UserInformationDA();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                this.LoadCompanyInformation();
                this.IsRestaurantOrderSubmitDisableInfo();
            }

            string costCenterId = string.Empty;
            CheckObjectPermission();
            string queryString1 = string.Empty;
            string queryString2 = string.Empty;
            this.btnBackPreviousPage.Visible = true;
            this.imgBtnTableDesign.Visible = true;
            this.imgBtnPaxInfo.Visible = true;
            this.imgItemSearch.Visible = true;
            this.imgBtnSpecialRemarks.Visible = true;
            this.imgBtnRestaurantCookBook.Visible = true;
            if (!IsRestaurantOrderSubmitDisable)
            {
                this.btnOrderSubmit.Visible = true;
            }
            else
            {
                this.btnOrderSubmit.Visible = false;
            }
            string queryStringId = Request.QueryString["Kot"];
            this.txtBearerIdInformation.Text = "1";
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (!string.IsNullOrWhiteSpace(Request.QueryString["CostCenterId"]))
            {
                costCenterId = Request.QueryString["CostCenterId"];

                Boolean status = userInformationDA.UpdateUserWorkingCostCenterInfo("PayrollEmployee", userInformationBO.UserInfoId, Convert.ToInt32(costCenterId));
                if (status)
                {
                    userInformationBO.WorkingCostCenterId = Convert.ToInt32(costCenterId);
                    Session.Add("EmployeeInformationBOSession", userInformationBO);
                }

                if (Request.QueryString["Kot"] == "TokenAllocation")
                {
                    Session.Add("KotBillTypeInformationBOSession", "TokenAllocation");
                    Response.Redirect("/POS/frmKotBill.aspx?Kot=TokenAllocation");
                }
                else if (Request.QueryString["Kot"] == "TableAllocation")
                {
                    Session.Add("KotBillTypeInformationBOSession", "TableAllocation");
                    Response.Redirect("/POS/frmKotBill.aspx?Kot=TableAllocation");
                }
                else if (Request.QueryString["Kot"] == "RoomAllocation")
                {
                    Session.Add("KotBillTypeInformationBOSession", "RoomAllocation");
                    Response.Redirect("/POS/frmKotBill.aspx?Kot=RoomAllocation");
                }
            }

            hfCostCenterId.Value = userInformationBO.WorkingCostCenterId.ToString();

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
                if (queryString1 == "TableAllocation")
                {
                    this.btnBackPreviousPage.Visible = false;
                    this.imgBtnTableDesign.Visible = false;
                    this.imgBtnPaxInfo.Visible = false;
                    this.imgItemSearch.Visible = false;
                    this.imgBtnSpecialRemarks.Visible = false;
                    this.imgBtnRestaurantCookBook.Visible = false;
                    this.btnOrderSubmit.Visible = false;
                    isLoadGridInformation = -1;
                    Session["txtTableIdInformation"] = null;
                    this.txtKotIdInformation.Text = string.Empty;
                    Session["txtKotIdInformation"] = null;
                    this.LoadTableAllocation();

                    if (Session["tbsMessage"] != null)
                    {
                        CommonHelper.AlertInfo(innboardMessage, Session["tbsMessage"].ToString(), AlertType.Warning);
                        Session.Remove("tbsMessage");
                    }

                    if (Session["TableAllocatedBy"] != null)
                    {
                        CommonHelper.AlertInfo(innboardMessage, Session["TableAllocatedBy"].ToString(), AlertType.Warning);
                        Session.Remove("TableAllocatedBy");
                    }
                }
                else if (queryString1 == "RoomAllocation")
                {
                    this.btnBackPreviousPage.Visible = false;
                    this.imgBtnTableDesign.Visible = false;
                    this.imgBtnPaxInfo.Visible = false;
                    this.imgItemSearch.Visible = false;
                    this.imgBtnSpecialRemarks.Visible = false;
                    this.imgBtnRestaurantCookBook.Visible = false;
                    this.btnOrderSubmit.Visible = false;
                    isLoadGridInformation = -1;
                    Session["txtTableIdInformation"] = null;
                    this.txtKotIdInformation.Text = string.Empty;
                    Session["txtKotIdInformation"] = null;
                    this.LoadRoomAllocation();

                    if (Session["tbsMessage"] != null)
                    {
                        CommonHelper.AlertInfo(innboardMessage, Session["tbsMessage"].ToString(), AlertType.Warning);
                        Session.Remove("tbsMessage");
                    }
                }
                else if (queryString1 == "TokenAllocation")
                {
                    this.btnBackPreviousPage.Visible = false;
                    this.imgBtnTableDesign.Visible = false;
                    this.imgBtnPaxInfo.Visible = false;
                    this.imgItemSearch.Visible = false;
                    this.imgBtnSpecialRemarks.Visible = false;
                    this.imgBtnRestaurantCookBook.Visible = false;
                    this.btnOrderSubmit.Visible = false;
                    isLoadGridInformation = -1;
                    Session["txtTableIdInformation"] = null;
                    this.txtKotIdInformation.Text = string.Empty;
                    Session["txtKotIdInformation"] = null;

                    int tokenCostCenterId = !string.IsNullOrWhiteSpace(this.hfCostCenterId.Value) ? Convert.ToInt32(this.hfCostCenterId.Value) : 0;
                    KotBillMasterDA kotBillMasterDA = new Data.Restaurant.KotBillMasterDA();
                    string tokenNumber = kotBillMasterDA.GenarateRestaurantTokenNumber(tokenCostCenterId);
                    this.BillCreateForToken(tokenNumber);

                    if (Session["tbsMessage"] != null)
                    {
                        CommonHelper.AlertInfo(innboardMessage, Session["tbsMessage"].ToString(), AlertType.Warning);
                        Session.Remove("tbsMessage");
                    }
                }
                else if (queryString1 == "BillCreateForTable")
                {
                    this.BillCreateForTable(queryStringId.Split(':')[1].ToString());
                }
                else if (queryString1 == "BillCreateForRoom")
                {
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
            }

            if (Session["txtKotIdInformation"] != null)
            {
                KotBillMasterDA entityDA = new KotBillMasterDA();
                KotBillMasterBO entityBOForPax = new KotBillMasterBO();
                entityBOForPax = entityDA.GetKotBillMasterInfoKotId(Convert.ToInt32(Session["txtKotIdInformation"]));
                if (entityBOForPax.KotId > 0)
                {
                    txtTouchKeypadResultForPax.Text = entityBOForPax.PaxQuantity.ToString();
                    txtSpecialRemarks.Text = entityBOForPax.Remarks.ToString();
                }
                else
                {
                    txtTouchKeypadResultForPax.Text = "0";
                    txtSpecialRemarks.Text = "";
                }
            }
            else
            {
                txtTouchKeypadResultForPax.Text = "0";
                txtSpecialRemarks.Text = "";
            }

            if (Session["KotBillTypeInformationBOSession"] != null)
            {
                this.imgBtnTableDesign.Visible = false;
                if (Session["KotBillTypeInformationBOSession"].ToString() == "TableAllocation")
                {
                    this.imgBtnTableDesign.Visible = true;
                }
            }
        }
        protected void btnBackPreviousPage_Click(object sender, ImageClickEventArgs e)
        {
            if (this.currentButtonFormName == "TableAllocation")
            {
                Response.Redirect("/POS/frmKotBill.aspx?Kot=TableAllocation");
            }
            else if (this.currentButtonFormName == "BillCreateForTable")
            {
                Response.Redirect("/POS/frmKotBill.aspx?Kot=TableAllocation");
            }
            else
            {
                string queryStringId = Request.QueryString["Kot"];
                if (Convert.ToInt32(queryStringId.Split(':')[1].ToString()) == 0)
                {
                    Response.Redirect("/POS/frmKotBill.aspx?Kot=TableAllocation");
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
                            Response.Redirect("/POS/frmKotBill.aspx?Kot=RestaurantItemCategory:0");
                        }
                        else
                        {
                            Response.Redirect("/POS/frmKotBill.aspx?Kot=RestaurantItemCategory:" + (matrixBO.AncestorId).ToString());
                        }
                    }
                }
            }
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

                        HMUtility hmUtility = new HMUtility();
                        UserInformationBO userInformationBO = new UserInformationBO();
                        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                        waiterName = userInformationBO.DisplayName.ToString();

                        if (pinfo.StockType == "StockItem")
                        {
                            entityBOList = entityDA.GetKotOrderSubmitInfo(kotId, strCostCenterId, "StockItem", false);
                        }
                        else
                        {
                            entityBOList = entityDA.GetKotOrderSubmitInfo(kotId, strCostCenterId, "KitchenItem", false).Where(x => x.KitchenId == pinfo.KitchenId && x.PrinterName == pinfo.PrinterName).ToList();
                        }

                        if (entityBOList.Count > 0)
                        {
                            rePrintStatus = false;
                            pinfo.IsChanged = false;
                            PrintReportKot(pinfo, entityBOList);
                        }
                    }

                    bool status = false;

                    if (rePrintStatus)
                    {
                        status = entityDA.UpdateKotBillDetailPrintFlagByOrderSubmitKotId(Convert.ToInt32(txtKotIdInformation.Text));
                    }

                    if (status && rePrintStatus)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Kot Order Successfully Submited.", AlertType.Success);
                    }
                    else if (!status && !rePrintStatus)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Kot Order already submited.", AlertType.Warning);
                    }
                }
                else
                {
                    if (hfIsItemAdded.Value == "1")
                        CommonHelper.AlertInfo(innboardMessage, "Kot Order already submited.", AlertType.Information);
                    else if (hfIsItemAdded.Value == "0")
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Please provide Item Information.", AlertType.Warning);
                    }

                    return;
                }
            }
            catch
            {
                CommonHelper.AlertInfo(innboardMessage, "Kot Order not Submited Successfully.", AlertType.Warning);
            }
            hfIsItemAdded.Value = "0";
        }
        protected void btnPrintFromServer_Click(object sender, EventArgs e)
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

                        HMUtility hmUtility = new HMUtility();
                        UserInformationBO userInformationBO = new UserInformationBO();
                        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                        waiterName = userInformationBO.DisplayName.ToString();

                        if (pinfo.StockType == "StockItem")
                        {
                            entityBOList = entityDA.GetKotOrderSubmitInfo(kotId, strCostCenterId, "StockItem", false);
                        }
                        else
                        {
                            entityBOList = entityDA.GetKotOrderSubmitInfo(kotId, strCostCenterId, "KitchenItem", false).Where(x => x.KitchenId == pinfo.KitchenId && x.PrinterName == pinfo.PrinterName).ToList();
                        }

                        if (entityBOList.Count > 0)
                        {
                            if (entityBOList.Count > 0)
                            {
                                rePrintStatus = true;
                                PrintReportKot(pinfo, entityBOList);
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
                CommonHelper.AlertInfo(innboardMessage, "KOT is not Submitted for Printer Related Issue.", AlertType.Error);
                
            }
            hfIsItemAdded.Value = "0";
        }
        protected void btnKotPrintPreview_Click(object sender, EventArgs e)
        {
            string url = "/POS/Reports/frmReportKotBillInfo.aspx?kotId=" + txtKotIdInformation.Text + "&tno=" + txtTableIdInformation.Text + "&kbm=" + "kotb";
            string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        //************************ User Defined Function ********************//
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
        private void LoadTableAllocation()
        {
            TableManagementDA entityDA = new TableManagementDA();
            List<TableManagementBO> entityListBO = new List<TableManagementBO>();

            string fullContent = string.Empty;
            int costCenterId = Convert.ToInt32(hfCostCenterId.Value);

            entityListBO = entityDA.GetTableManagementInfo(costCenterId);
            string topPart = @"<div id='legendContainerForRestaurant'>
                                <div class='legend RoomVacantDiv'>
                                </div>
                                <div class='legendText'>
                                    Available</div>
                                <div class='legend RoomOccupaiedDiv'>
                                </div>
                                <div class='legendText'>
                                    Occupaied
                                </div>
                            </div><div class='divClear'>
                            </div>
                            <div class='block FloorRoomAllocationBGImage'>
                                <a href='#' class='block-heading' data-toggle='collapse'>";

            string topTemplatePart = @"</a>
                                <div id='FloorRoomAllocation' class='block-body collapse in'>           
                                ";

            string endTemplatePart = @"</div>
                            </div>";

            string subContent = string.Empty;

            for (int iTableNumber = 0; iTableNumber < entityListBO.Count; iTableNumber++)
            {
                string Content0 = string.Empty;
                string Content1 = string.Empty;
                string Content2 = string.Empty;
                if (entityListBO[iTableNumber].StatusId == 1)
                {
                    Content0 = @"<a href='/POS/frmKotBill.aspx?Kot=BillCreateForTable:" + entityListBO[iTableNumber].TableId + "&tbs=1";
                    Content1 = @"'><div class='NotDraggable RestaurantTableAvailableDiv' style='width:" + entityListBO[iTableNumber].TableWidth + "px; height:" + entityListBO[iTableNumber].TableHeight + "px; top:" + entityListBO[iTableNumber].YCoordinate + "px; left:" + entityListBO[iTableNumber].XCoordinate + "px;' id='" + entityListBO[iTableNumber].TableManagementId + "'>";
                    Content2 = @"<div class='RestaurantTableAvailableDiv'>
                                        </div>
                                        <div class='TableNumberDiv'>" + entityListBO[iTableNumber].TableNumber + "</div></div></a>";
                }
                if (entityListBO[iTableNumber].StatusId == 2)
                {
                    Content0 = @"<a href='/POS/frmKotBill.aspx?Kot=BillCreateForTable:" + entityListBO[iTableNumber].TableId + "&tbs=2";
                    Content1 = @"'><div class='NotDraggable RestaurantTableBookedDiv' style='width:" + entityListBO[iTableNumber].TableWidth + "px; height:" + entityListBO[iTableNumber].TableHeight + "px; top:" + entityListBO[iTableNumber].YCoordinate + "px; left:" + entityListBO[iTableNumber].XCoordinate + "px;' id='" + entityListBO[iTableNumber].TableManagementId + "'>";
                    Content2 = @"<div class='RestaurantTableBookedDiv'>
                                        </div>
                                        <div class='TableNumberDiv'>" + entityListBO[iTableNumber].TableNumber + "</div></div></a>";
                }
                if (entityListBO[iTableNumber].StatusId == 3)
                {
                    Content0 = @"<a href='/POS/frmKotBill.aspx?Kot=BillCreateForTable:" + entityListBO[iTableNumber].TableId + "&tbs=3";
                    Content1 = @"'><div class='NotDraggable RestaurantTableReservedDiv' style='width:" + entityListBO[iTableNumber].TableWidth + "px; height:" + entityListBO[iTableNumber].TableHeight + "px; top:" + entityListBO[iTableNumber].YCoordinate + "px; left:" + entityListBO[iTableNumber].XCoordinate + "px;' id='" + entityListBO[iTableNumber].TableManagementId + "'>";
                    Content2 = @"<div class='RestaurantTableReservedDiv'>
                                        </div>
                                        <div class='TableNumberDiv'>" + entityListBO[iTableNumber].TableNumber + "</div></div></a>";
                }

                subContent += Content0 + Content1 + Content2;
            }

            fullContent += topPart + topTemplatePart + subContent + endTemplatePart;
            this.ltlRoomTemplate.Text = fullContent;
        }
        private void LoadRoomAllocation()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberListBO = new List<RoomNumberBO>();
            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> RoomTypeInfoList = new List<RoomTypeBO>();
            RoomTypeInfoList = roomTypeDA.GetRoomTypeInfo();

            string fullContent = string.Empty;

            int RoomPossibleVacantDiv = 0;
            int RoomOccupaiedDiv = 0;

            string roomSummary = string.Empty;

            string topPart = @"<div id='legendContainerForRestaurant'>                                
                                <div class='legend RoomOccupaiedDiv'>
                                </div>
                                <div class='legendText'>
                                    Occupaied
                                </div>
                                <div class='legend RoomPossibleVacantDiv'>
                                </div>
                                <div class='legendText'>
                                    Possible Vacant</div>
                            </div><div class='divClear'>
                            </div>
                            <div id='SearchPanel' class='block'>
                                <a href='#' class='block-heading' data-toggle='collapse'>";

            string topTemplatePart = @"</a>
                                <div class='block-body collapse in'>           
                                ";
            string groupNamePart = string.Empty;

            string endTemplatePart = @"</div>
                            </div>";

            roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(0);

            string subContent = string.Empty;

            for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
            {
                if (roomNumberListBO[iRoomNumber].StatusId == 2)
                {
                    if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomPossibleVacantDiv")
                    {
                        subContent += @"<a href='/POS/frmKotBill.aspx?Kot=BillCreateForRoom:" + roomNumberListBO[iRoomNumber].RoomNumber;
                        subContent += "'>";
                        subContent += @"<div class='DivRoomContainer'><div class='RoomPossibleVacantDiv'></div>
                                            <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div></a>";
                        RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                    }
                    else
                    {
                        subContent += @"<a href='/POS/frmKotBill.aspx?Kot=BillCreateForRoom:" + roomNumberListBO[iRoomNumber].RoomNumber;
                        subContent += "'>";
                        subContent += @"<div class='DivRoomContainer'><div class='RoomOccupaiedDiv'></div>
                                            <div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div></a>";
                        RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                    }
                }
            }

            roomSummary = " (Occupaied: " + RoomOccupaiedDiv + ", Possible Vacant: " + RoomPossibleVacantDiv + ")";

            groupNamePart = "Status" + roomSummary;

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
            string topPart = @"<div class='divClear'>
                            </div>                            
                           <a href='javascript:void()' class='block-heading' data-toggle='collapse'>";

            string topTemplatePart = @"</a> <div> <div style='border: 1px solid #ccc; margin:5px; overflow:hidden;'> ";
            string endTemplatePart = @"</div> </div>";

            string subContent = string.Empty;

            for (int iItemCategory = 0; iItemCategory < roomNumberListWithoutZeroBO.Count; iItemCategory++)
            {
                string Content1 = string.Empty;
                if (roomNumberListWithoutZeroBO[iItemCategory].ChildCount.ToString() == "1")
                {
                    Content1 = @"<div class='DivRestaurantItemContainer'>
                                        <a href='/POS/frmKotBill.aspx?Kot=RestaurantItem:" + roomNumberListWithoutZeroBO[iItemCategory].CategoryId;
                }
                else
                {
                    Content1 = @"<div class='DivRestaurantItemContainer'>
                                        <a href='/POS/frmKotBill.aspx?Kot=RestaurantItemCategory:" + roomNumberListWithoutZeroBO[iItemCategory].CategoryId;
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

                string topPart = @"<a href='javascript:void()' class='block-heading'>Restaurant Item Information";
                string topTemplatePart = @"</a> <div> <div style='border: 1px solid #ccc; margin:5px; overflow:hidden;'>";
                string endTemplatePart = @"</div> </div>";

                string subContent = string.Empty;

                for (int iItem = 0; iItem < roomNumberListBO.Count; iItem++)
                {
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
        private void BillCreateForTable(string queryString)
        {
            if (!string.IsNullOrWhiteSpace(queryString))
            {
                int tableStatus = 0;
                int tableId = Convert.ToInt32(queryString);
                if (Request.QueryString["tbs"] != null)
                {
                    tableStatus = int.Parse(Request.QueryString["tbs"]);
                }
                RestaurantTableBO entityTableBO = new RestaurantTableBO();
                RestaurantTableDA entityTableDA = new RestaurantTableDA();

                int tmpPkId;
                KotBillMasterBO entityBO = new KotBillMasterBO();
                KotBillMasterDA entityDA = new KotBillMasterDA();
                entityBO.SourceName = "RestaurantTable";
                entityBO.SourceId = tableId;

                this.txtTableIdInformation.Text = tableId.ToString();
                Session["txtTableIdInformation"] = tableId.ToString();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBearerDA restaurantBearerDA = new RestaurantBearerDA();
                RestaurantBearerBO restaurantBearerBO = new RestaurantBearerBO();

                restaurantBearerBO = restaurantBearerDA.GetRestaurantBearerInfoByEmpIdNIsBearer(userInformationBO.UserInfoId, 1);

                if (restaurantBearerBO != null)
                {
                    entityBO.BearerId = restaurantBearerBO.BearerId;
                    entityBO.CostCenterId = userInformationBO.WorkingCostCenterId;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;

                    entityTableBO = entityTableDA.GetRestaurantTableInfoById(userInformationBO.WorkingCostCenterId, "RestaurantTable", tableId);

                    if (tableStatus != entityTableBO.StatusId)
                    {
                        Session["tbsMessage"] = "Table status updated.";
                        Response.Redirect("/POS/frmKotBill.aspx?Kot=TableAllocation");
                    }

                    if (entityTableBO.StatusId == 1)
                    {
                        this.txtTableNumberInformation.Text = entityTableBO.TableNumber;
                        Session["txtTableNumberInformation"] = this.txtTableNumberInformation.Text;
                        Boolean status = entityDA.SaveKotBillMasterInfo(entityBO, out tmpPkId);
                        if (status)
                        {

                            this.txtKotIdInformation.Text = tmpPkId.ToString();
                            Session["txtKotIdInformation"] = this.txtKotIdInformation.Text;
                            Response.Redirect("/POS/frmKotBill.aspx?Kot=RestaurantItemCategory:0");
                        }
                    }
                    else if (entityTableBO.StatusId == 2)
                    {
                        this.txtTableNumberInformation.Text = entityTableBO.TableNumber;
                        Session["txtTableNumberInformation"] = this.txtTableNumberInformation.Text;
                        entityBO = entityDA.GetKotBillMasterInfoByTableId(entityBO.CostCenterId, "RestaurantTable", tableId);
                        if (entityBO.KotId > 0)
                        {
                            Session["txtKotIdInformation"] = entityBO.KotId;

                            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantKotContinueWithDiferentWaiter", "IsRestaurantKotContinueWithDiferentWaiter");
                            if (commonSetupBO != null)
                            {
                                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                                {
                                    if (commonSetupBO.SetupValue == "0")
                                    {
                                        if (entityBO.CreatedBy != userInformationBO.UserInfoId)
                                        {
                                            RestaurantBearerBO tableAllocatedBy = new RestaurantBearerBO();
                                            tableAllocatedBy = restaurantBearerDA.GetRestaurantBearerInfoByEmpIdNIsBearer(entityBO.CreatedBy, 1);
                                            Session["TableAllocatedBy"] = "This Table (" + entityTableBO.TableNumber + ") Is already Allocated By " + tableAllocatedBy.UserName + ".";
                                            Response.Redirect("/POS/frmKotBill.aspx?Kot=TableAllocation");
                                        }
                                        else
                                        {
                                            Response.Redirect("/POS/frmKotBill.aspx?Kot=RestaurantItemCategory:0");
                                        }
                                    }
                                    else
                                    {
                                        Response.Redirect("/POS/frmKotBill.aspx?Kot=RestaurantItemCategory:0");
                                    }
                                }
                            }
                            else
                            {
                                Response.Redirect("/POS/frmKotBill.aspx?Kot=RestaurantItemCategory:0");
                            }
                        }
                    }
                }
            }
        }
        private void BillCreateForToken(string queryString)
        {
            if (!string.IsNullOrWhiteSpace(queryString))
            {
                int tableStatus = 0;
                int tableId = Convert.ToInt32(queryString);
                if (Request.QueryString["tbs"] != null)
                {
                    tableStatus = int.Parse(Request.QueryString["tbs"]);
                }
                RestaurantTableBO entityTableBO = new RestaurantTableBO();
                RestaurantTableDA entityTableDA = new RestaurantTableDA();

                int tmpPkId;
                KotBillMasterBO entityBO = new KotBillMasterBO();
                KotBillMasterDA entityDA = new KotBillMasterDA();
                entityBO.SourceName = "RestaurantToken";
                entityBO.SourceId = tableId;

                this.txtTableIdInformation.Text = tableId.ToString();
                Session["txtTableIdInformation"] = tableId.ToString();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBearerDA restaurantBearerDA = new RestaurantBearerDA();
                RestaurantBearerBO restaurantBearerBO = new RestaurantBearerBO();

                restaurantBearerBO = restaurantBearerDA.GetRestaurantBearerInfoByEmpIdNIsBearer(userInformationBO.UserInfoId, 1);

                if (restaurantBearerBO != null)
                {
                    entityBO.BearerId = restaurantBearerBO.BearerId;
                    entityBO.CostCenterId = userInformationBO.WorkingCostCenterId;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;
                    entityBO.CostCenterId = userInformationBO.WorkingCostCenterId;

                    this.txtTableNumberInformation.Text = entityTableBO.TableNumber;
                    Session["txtTableNumberInformation"] = this.txtTableNumberInformation.Text;
                    Boolean status = entityDA.SaveKotBillMasterInfo(entityBO, out tmpPkId);
                    if (status)
                    {
                        this.txtKotIdInformation.Text = tmpPkId.ToString();
                        Session["txtKotIdInformation"] = this.txtKotIdInformation.Text;
                        Response.Redirect("/POS/frmKotBill.aspx?Kot=RestaurantItemCategory:0");
                    }
                }
            }

        }
        private void BillCreateForRoom(string queryString)
        {
            if (!string.IsNullOrWhiteSpace(queryString))
            {
                int tableStatus = 0;
                int tableId = Convert.ToInt32(queryString);

                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(tableId.ToString());

                if (roomAllocationBO.RoomId > 0)
                {
                    if (Request.QueryString["tbs"] != null)
                    {
                        tableStatus = int.Parse(Request.QueryString["tbs"]);
                    }
                    RestaurantTableBO entityTableBO = new RestaurantTableBO();
                    RestaurantTableDA entityTableDA = new RestaurantTableDA();

                    int tmpPkId;
                    KotBillMasterBO entityBO = new KotBillMasterBO();
                    KotBillMasterDA entityDA = new KotBillMasterDA();
                    entityBO.SourceName = "GuestRoom";
                    entityBO.SourceId = roomAllocationBO.RegistrationId;

                    this.txtTableIdInformation.Text = roomAllocationBO.RegistrationId.ToString();
                    Session["txtTableIdInformation"] = roomAllocationBO.RegistrationId.ToString();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    RestaurantBearerDA restaurantBearerDA = new RestaurantBearerDA();
                    RestaurantBearerBO restaurantBearerBO = new RestaurantBearerBO();

                    restaurantBearerBO = restaurantBearerDA.GetRestaurantBearerInfoByEmpIdNIsBearer(userInformationBO.UserInfoId, 1);

                    if (restaurantBearerBO != null)
                    {
                        entityBO.BearerId = restaurantBearerBO.BearerId;
                        entityBO.CostCenterId = userInformationBO.WorkingCostCenterId;
                        entityBO.CreatedBy = userInformationBO.UserInfoId;

                        entityTableBO = entityTableDA.GetRestaurantTableInfoById(userInformationBO.WorkingCostCenterId, "GuestRoom", roomAllocationBO.RegistrationId);

                        if (entityTableBO != null)
                        {
                            if (entityTableBO.TableId > 0)
                            {
                                if (entityTableBO.StatusId == 1)
                                {
                                    this.txtTableNumberInformation.Text = entityTableBO.TableNumber;
                                    Session["txtTableNumberInformation"] = this.txtTableNumberInformation.Text;
                                    Boolean status = entityDA.SaveKotBillMasterInfo(entityBO, out tmpPkId);
                                    if (status)
                                    {
                                        this.txtKotIdInformation.Text = tmpPkId.ToString();
                                        Session["txtKotIdInformation"] = this.txtKotIdInformation.Text;
                                        Response.Redirect("/POS/frmKotBill.aspx?Kot=RestaurantItemCategory:0");
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
                                        Response.Redirect("/POS/frmKotBill.aspx?Kot=RestaurantItemCategory:0");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void CheckObjectPermission()
        {
            //EmployeeBO employeeInformationBO = new EmployeeBO();
            //employeeInformationBO = hmUtility.GetCurrentApplicationBearerUserInfo();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermissionForBearer(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmCostCenterChooseForKot.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            if (!isSavePermission)
            {
                Response.Redirect("/POS/Login.aspx");
            }
        }
        private void IsRestaurantOrderSubmitDisableInfo()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantOrderSubmitDisable", "IsRestaurantOrderSubmitDisable");
            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        IsRestaurantOrderSubmitDisable = false;
                    }
                    else
                    {
                        IsRestaurantOrderSubmitDisable = true;
                    }
                }
            }
        }
        private void pdoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (strPrinterInfoId > 0)
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

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    String DrawnBy = userInformationBO.DisplayName.ToString();
                    graphics.DrawString("Waiter Name: " + DrawnBy, new Font("Courier New", 10),
                             new SolidBrush(Color.Black), startX, startY + Offset);
                }
            }
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
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    String DrawnBy = userInformationBO.DisplayName.ToString();
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
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    String DrawnBy = userInformationBO.DisplayName.ToString();
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
        public void PrintReportKot(PrinterInfoBO files, List<KotBillDetailBO> entityBOList)
        {
            Double HeightInInch = 11;
            Double WidthInInch = 3.0;

            LocalReport report = new LocalReport();
            report.ReportPath = HttpContext.Current.Server.MapPath(@"~/POS/Reports/Rdlc/rptKotBill.rdlc");
            report.EnableExternalImages = true;
            report.EnableHyperlinks = true;

            DateTime dateTime = DateTime.Now;
            string kotDate = String.Format("{0:f}", dateTime);

            int kotId = 0;

            foreach (KotBillDetailBO row in entityBOList)
            {
                kotId = row.KotId;
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
                    reportTitle = "KOT";
                }
            }

            ReportParameter[] parms = new ReportParameter[9];
            parms[0] = new ReportParameter("ReportTitle", reportTitle);
            parms[1] = new ReportParameter("CostCenter", strCostCenterName);
            parms[2] = new ReportParameter("SourceName", strCostCenterDefaultView);
            parms[3] = new ReportParameter("TableNo", txtTableNumberInformation.Text);
            parms[4] = new ReportParameter("KotNo", txtKotIdInformation.Text);
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

            HeightInInch = (entityBOList.Count + 10) * 0.64851 + 1.5 + 1;

            ReportDirectPrinter print = new ReportDirectPrinter();
            print.PrintDefaultPage(report, files.PrinterName, 3.5, 11);
        }
        //************************ User Defined Web Method ********************//
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
        public static KotBillDetailBO SaveComboItemDetailInformation(int costCenter, int kotId, int itemId)
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
                entityBO = entityDA.GetKotBillDetailInfoByKotNItemId(costCenter, kotId, itemId, "ComboItem");

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
                        itemEntityBO = itemEntityDA.GetInvItemInfoById(costCenterId, srcEntityBO.ItemId);
                        entityBO.UnitRate = itemEntityBO.UnitPriceLocal;
                        entityBO.Amount = itemEntityBO.UnitPriceLocal * Convert.ToDecimal(updatedContent);
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
        public static string GenerateTableWiseItemGridInformation(int costCenterId, int tableId, int kotId)
        {
            string strTable = string.Empty;
            CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(costCenterId);
            if (costCentreTabBO != null)
            {
                if (costCentreTabBO.CostCenterId > 0)
                {
                    string costCenterDefaultView = string.Empty;

                    if (costCentreTabBO.DefaultView == "Token")
                    {
                        costCenterDefaultView = "RestaurantToken";
                    }
                    else if (costCentreTabBO.DefaultView == "Table")
                    {
                        costCenterDefaultView = "RestaurantTable";
                    }
                    else if (costCentreTabBO.DefaultView == "Room")
                    {
                        costCenterDefaultView = "GuestRoom";
                    }

                    KotBillDetailDA entityDA = new KotBillDetailDA();
                    List<KotBillDetailBO> files = entityDA.GetKotBillDetailInfoByKotNBearerId(costCenterId, costCenterDefaultView, tableId, kotId);

                    Boolean isChangedExist = false;
                    foreach (KotBillDetailBO drIsChanged in files)
                    {
                        if (drIsChanged.IsChanged)
                        {
                            isChangedExist = true;
                            break;
                        }
                    }

                    strTable = "<div id='no-more-tables'> ";
                    strTable += "<table cellspacing='0' cellpadding='4' id='TableWiseItemInformation' table-bordered table-striped table-condensed cf> <thead class='cf'> <tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                    strTable += "<th align='left' scope='col'>Item Name</th><th align='left' scope='col'>Unit</th><th align='left' scope='col'>Unit Rate</th><th align='left' scope='col'>Total</th><th align='center' scope='col'>Action</th></tr></thead>";
                    strTable += "<tbody>";
                    int counter = 0;
                    foreach (KotBillDetailBO dr in files)
                    {
                        if (counter % 2 == 0)
                        {
                            // It's even
                            strTable += "<tr style='background-color:#E3EAEB;'> <td data-title='Item Name' align='left' style='width: 40%;'>" + dr.ItemName + "</td>";
                        }
                        else
                        {
                            // It's odd
                            strTable += "<tr style='background-color:White;'><td data-title='Item Name' align='left' style='width: 40%;'>" + dr.ItemName + "</td>";
                        }

                        strTable += "<td data-title='Unit' align='left' style='width: 15%;'>" + dr.ItemUnit + "</td>";
                        strTable += "<td data-title='Unit Rate' align='left' style='width: 15%;'>" + dr.UnitRate + "</td>";
                        strTable += "<td data-title='Total' align='left' style='width: 15%;'>" + Math.Round((dr.ItemUnit * dr.UnitRate), 2) + "</td>";

                        //strTable += "<td align='center' style='width: 15%;'>";
                        if (dr.KotDetailId > 0)
                        {
                            if (!dr.PrintFlag)
                            {
                                strTable += "<td data-title='Action' align='center' style='width: 15%;'><button type='button' class='TransactionalButton btn btn-primary' data-dismiss='alert' onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + "," + 0 + ")' alt='Action Decider'>Option</button></td>";
                            }
                            else
                            {
                                if (!isChangedExist)
                                {
                                    strTable += "<td data-title='Action' align='center' style='width: 15%;'><button type='button' class='TransactionalButton btn btn-success' data-dismiss='alert' onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + "," + 1 + ")' alt='Action Decider'>Option</button></td>";
                                }
                                else
                                {
                                    strTable += "<td data-title='Action' align='center' style='width: 15%;'><button type='button' class='TransactionalButton btn btn-primary' data-dismiss='alert' onClick='javascript:return AddNewItem(" + dr.KotDetailId + "," + dr.KotId + "," + dr.ItemId + "," + 1 + ")' alt='Action Decider'>Option</button></td>";
                                }
                            }

                        }
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
                        counter++;
                    }
                    strTable += "</tbody> </table> </div>";
                    if (strTable == "")
                    {
                        strTable = "<tr><td data-title='Item Name' colspan='4' align='center'>No Record Available!</td></tr>";
                    }
                }
            }
            return strTable;
        }
        [WebMethod]
        public static ReturnInfo DeleteData(int kotDetailId, int kotId, int itemId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                KotBillDetailDA kotDa = new KotBillDetailDA();
                Boolean status = kotDa.DeleteKotBillDetail(kotDetailId, kotId, itemId, userInformationBO.UserInfoId);
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
                    Boolean status = entityDA.UpdateKotBillMasterPaxInfo(kotId, costCenterId, tableBO.TableId, paxQuantity);
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
        [WebMethod]
        public static bool GetIsLocalNetworkRServerPrint()
        {
            bool isNetworkPrinting = false;

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isInnboardServiceChargeEnableBO = new HMCommonSetupBO();
            isInnboardServiceChargeEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsLocalNetworkKotPrint", "IsLocalNetworkKotPrint");
            isNetworkPrinting = Convert.ToInt32(isInnboardServiceChargeEnableBO.SetupValue) == 1 ? true : false;

            return isNetworkPrinting;
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
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int waiterId = userInformationBO.UserInfoId;

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
        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemSearch(string itemName, int costCenterId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemNameWiseItemDetailsForAutoSearch(itemName, costCenterId, ConstantHelper.CustomerSupplierAutoSearch.CustomerItem.ToString());

            return itemInfo;
        }
        [WebMethod]
        public static string CheckIdleTimeToSignOut(string ss)
        {
            string fullContent = "0";
            System.Web.HttpContext.Current.Session.Remove("EmployeeInformationBOSession");
            return fullContent;
        }
    }
}