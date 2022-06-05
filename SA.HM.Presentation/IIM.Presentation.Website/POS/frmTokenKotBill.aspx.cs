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
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;

using System.IO;
using System.Data;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing;
using System.Security;
using System.Security.Permissions;
using Microsoft.Reporting.WebForms;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmTokenKotBill : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isLoadGridInformation = 1;
        private int strPrinterInfoId = 0;
        private int strCostCenterId = 0;
        private string strCostCenterName = string.Empty;
        private string strPrinterName = string.Empty;
        private string waiterName = string.Empty;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
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

            CheckObjectPermission();
            string queryString1 = string.Empty;
            string queryString2 = string.Empty;
            this.btnBackPreviousPage.Visible = true;
            this.imgBtnTableDesign.Visible = false;
            this.imgBtnPaxInfo.Visible = true;
            this.imgItemSearch.Visible = true;
            if (!IsRestaurantOrderSubmitDisable)
            {
                this.btnOrderSubmit.Visible = true;
            }
            else
            {
                this.btnOrderSubmit.Visible = false;
            }
            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            string queryStringId = string.Empty;
            string costCenterId = string.Empty;
            queryStringId = Request.QueryString["Kot"];

            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

            if (!string.IsNullOrWhiteSpace(Request.QueryString["CostCenterId"]))
            {
                costCenterId = Request.QueryString["CostCenterId"];

                Boolean status = userInformationDA.UpdateUserWorkingCostCenterInfo("SecurityUser", currentUserInformationBO.UserInfoId, Convert.ToInt32(costCenterId));
                if (status)
                {
                    currentUserInformationBO.WorkingCostCenterId = Convert.ToInt32(costCenterId);
                    Session.Add("UserInformationBOSession", currentUserInformationBO);
                }

                CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
                costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoById(currentUserInformationBO.WorkingCostCenterId);
                if (costCentreTabBO != null)
                {
                    if (costCentreTabBO.IsRestaurant)
                    {
                        if (costCentreTabBO.DefaultView == "Token")
                        {
                            Response.Redirect("/POS/frmTokenKotBill.aspx?Kot=TokenInformation");
                        }
                        //else if (costCentreTabBO.DefaultView == "Table")
                        //{
                        //    Response.Redirect("/Restaurant/frmKotBillMaster.aspx?Kot=TableAllocation");
                        //}
                        //else if (costCentreTabBO.DefaultView == "Room")
                        //{
                        //    Response.Redirect("/Restaurant/frmGuestRoomKotBill.aspx?Kot=RoomAllocation");
                        //}
                    }
                }
                else
                {
                    Response.Redirect("/POS/frmTokenKotBill.aspx?Kot=TableAllocation");
                }
            }

            this.txtBearerIdInformation.Text = "1";
            this.pnlRoomWiseKotBill.Visible = false;

            //if (Session["txtTableIdInformation"] != null)
            //{
            //currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            hfCostCenterId.Value = currentUserInformationBO.WorkingCostCenterId.ToString();
            //}

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
                if (queryString1 == "TokenInformation")
                {
                    /*
                    this.pnlRoomWiseKotBill.Visible = true;
                    this.btnBackPreviousPage.Visible = false;
                    this.imgBtnTableDesign.Visible = false;
                    this.imgBtnPaxInfo.Visible = false;
                    this.imgBtnSpecialRemarks.Visible = false;
                    this.imgItemSearch.Visible = false;
                    this.imgBtnRestaurantCookBook.Visible = false;
                    this.btnOrderSubmit.Visible = false;
                    isLoadGridInformation = -1;
                    Session["txtTableIdInformation"] = null;
                    this.txtKotIdInformation.Text = string.Empty;
                    Session["txtKotIdInformation"] = null;
                    this.LoadTableAllocation();
                    */
                    Response.Redirect("/POS/frmTokenKotBill.aspx?Kot=BillCreateForToken");
                }
                else if (queryString1 == "BillCreateForToken")
                {
                    int tokenCostCenterId = !string.IsNullOrWhiteSpace(this.hfCostCenterId.Value) ? Convert.ToInt32(this.hfCostCenterId.Value) : 0;
                    KotBillMasterDA kotBillMasterDA = new Data.Restaurant.KotBillMasterDA();
                    string tokenNumber = kotBillMasterDA.GenarateRestaurantTokenNumber(tokenCostCenterId);
                    this.BillCreateForToken(tokenNumber);
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
                    ReprintKotBill(Convert.ToInt32(queryStringId.Split(':')[1].ToString()));

                    imgBtnRestaurantCookBook.Visible = false;
                    //this.LoadRestaurantItemCategory(0);
                    Response.Redirect("/POS/frmTokenKotBill.aspx?Kot=TableAllocation");
                }

                //else if (queryString1 == "RestaurantItemGroup")
                //{
                //    this.LoadRestaurantItemGroup();
                //}
                //else if (queryString1 == "RestaurantComboItem")
                //{
                //    this.LoadRestaurantComboItem();
                //}
                //else if (queryString1 == "RestaurantBuffetItem")
                //{
                //    this.LoadRestaurantBuffetItem();
                //}
                //else if (queryString1 == "RestaurantItemCategory")
                //{
                //    this.LoadRestaurantItemCategory();
                //}
            }


            if (Session["txtKotIdInformation"] != null)
            {
                KotBillMasterDA entityDA = new KotBillMasterDA();
                KotBillMasterBO entityBOForPax = new KotBillMasterBO();
                entityBOForPax = entityDA.GetKotBillMasterInfoKotId(Convert.ToInt32(Session["txtKotIdInformation"]));
                if (entityBOForPax.KotId > 0)
                {
                    txtTouchKeypadResultForPax.Text = entityBOForPax.PaxQuantity.ToString();
                }
                else
                {
                    txtTouchKeypadResultForPax.Text = "0";
                }
            }
            else
            {
                txtTouchKeypadResultForPax.Text = "0";
            }

            if (!IsPostBack)
            {
                IsRestaurantIntegrateWithFrontOffice();
            }
        }
        protected void btnBackPreviousPage_Click(object sender, ImageClickEventArgs e)
        {
            if (this.currentButtonFormName == "TokenInformation")
            {
                Response.Redirect("/POS/frmTokenKotBill.aspx?Kot=TokenInformation");
            }
            else if (this.currentButtonFormName == "BillCreateForToken")
            {
                Response.Redirect("/POS/frmTokenKotBill.aspx?Kot=BillCreateForToken");
            }
            else
            {
                string queryStringId = Request.QueryString["Kot"];
                if (Convert.ToInt32(queryStringId.Split(':')[1].ToString()) == 0)
                {
                    Response.Redirect("/POS/frmTokenKotBill.aspx?Kot=TokenInformation");
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
                            Response.Redirect("/POS/frmTokenKotBill.aspx?Kot=RestaurantItemCategory:0");
                        }
                        else
                        {
                            Response.Redirect("/POS/frmTokenKotBill.aspx?Kot=RestaurantItemCategory:" + (matrixBO.AncestorId).ToString());
                        }
                    }
                }
            }
            /*
            switch (this.currentButtonFormName)
            {
                case "TableAllocation":
                    Response.Redirect("/Restaurant/frmKotBillMaster.aspx?Kot=TableAllocation");
                    break;
                case "BillCreateForTable":
                    Response.Redirect("/Restaurant/frmKotBillMaster.aspx?Kot=TableAllocation");
                    break;
                case "RestaurantItemGroup":
                    Response.Redirect("/Restaurant/frmKotBillMaster.aspx?Kot=TableAllocation");
                    break;
                case "RestaurantComboItem":
                    Response.Redirect("/Restaurant/frmKotBillMaster.aspx?Kot=RestaurantItemGroup:1");
                    break;
                case "RestaurantBuffetItem":
                    Response.Redirect("/Restaurant/frmKotBillMaster.aspx?Kot=RestaurantItemGroup:1");
                    break;
                case "RestaurantItemCategory":
                    Response.Redirect("/Restaurant/frmKotBillMaster.aspx?Kot=RestaurantItemGroup:1");
                    break;
                case "RestaurantItem":
                    Response.Redirect("/Restaurant/frmKotBillMaster.aspx?Kot=RestaurantItemCategory:2");
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
                    {
                        CommonHelper.AlertInfo(innboardMessage, "KOT already submitted.", AlertType.Information);
                    }
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
        }
        protected void imgBtnRoomWiseKotBill_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/POS/frmGuestRoomKotBill.aspx?Kot=RoomAllocation");
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
                        //this.btnOrderSubmit.Visible = false;
                        IsRestaurantOrderSubmitDisable = false;
                    }
                    else
                    {
                        //this.btnOrderSubmit.Visible = true;
                        IsRestaurantOrderSubmitDisable = true;
                    }
                }
            }

            //this.imgBtnRoomWiseKotBill.Visible = false;
        }
        private void IsRestaurantIntegrateWithFrontOffice()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithFrontOffice", "IsRestaurantIntegrateWithFrontOffice");
            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        this.imgBtnRoomWiseKotBill.Visible = false;
                    }
                    else
                    {
                        this.imgBtnRoomWiseKotBill.Visible = true;
                    }
                }
            }

            this.imgBtnRoomWiseKotBill.Visible = false;
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
                    //string kotDate = dateTime.ToString("MM/dd/yyyy");
                    string kotDate = String.Format("{0:f}", dateTime);
                    graphics.DrawString(kotDateText.PadRight(8, ' ') + " : " + kotDate, new Font("Courier New", 10),
                                 new SolidBrush(Color.Black), startX, startY + Offset);


                    Offset = Offset + 20;
                    String underLine = "----------------------------------------------";
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
                    underLine = "----------------------------------------------";
                    graphics.DrawString(underLine, new Font("Courier New", 10),
                             new SolidBrush(Color.Black), startX, startY + Offset);
                    Offset = Offset + 20;

                    UserInformationBO currentUserInformationBO = new UserInformationBO();
                    currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    String DrawnBy = currentUserInformationBO.UserName.ToString();
                    graphics.DrawString("Bearer Name: " + DrawnBy, new Font("Courier New", 10),
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
                    //string kotDate = dateTime.ToString("MM/dd/yyyy");
                    string kotDate = String.Format("{0:f}", dateTime);
                    graphics.DrawString(kotDateText.PadRight(8, ' ') + " : " + kotDate, new Font("Courier New", 10),
                                 new SolidBrush(Color.Black), startX, startY + Offset);


                    Offset = Offset + 20;
                    String underLine = "----------------------------------------------";
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
                    underLine = "----------------------------------------------";
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
                    String underLine = "----------------------------------------------";
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
                    underLine = "----------------------------------------------";
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
        private void LoadTableAllocation()
        {
            TableManagementDA entityDA = new TableManagementDA();
            List<TableManagementBO> entityListBO = new List<TableManagementBO>();

            string fullContent = string.Empty;

            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            entityListBO = entityDA.GetTableManagementInfo(currentUserInformationBO.WorkingCostCenterId);
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
                    Content0 = @"<a href='/POS/frmKotBillMaster.aspx?Kot=BillCreateForTable:" + entityListBO[iTableNumber].TableId;
                    Content1 = @"'><div class='NotDraggable RestaurantTableAvailableDiv' style='width:" + entityListBO[iTableNumber].TableWidth + "px; height:" + entityListBO[iTableNumber].TableHeight + "px; top:" + entityListBO[iTableNumber].YCoordinate + "px; left:" + entityListBO[iTableNumber].XCoordinate + "px;' id='" + entityListBO[iTableNumber].TableManagementId + "'>";
                    Content2 = @"<div class='RestaurantTableAvailableDiv'>
                                        </div>
                                        <div class='TableNumberDiv'>" + entityListBO[iTableNumber].TableNumber + "</div></div></a>";

                }
                if (entityListBO[iTableNumber].StatusId == 2)
                {
                    Content0 = @"<div class='NotDraggable RestaurantTableBookedDiv' style='width:" + entityListBO[iTableNumber].TableWidth + "px; height:" + entityListBO[iTableNumber].TableHeight + "px; top:" + entityListBO[iTableNumber].YCoordinate + "px; left:" + entityListBO[iTableNumber].XCoordinate + "px;' id='" + entityListBO[iTableNumber].TableManagementId + "'>";
                    Content1 = @"<div class='KOTNTableNumberDiv' style='display:none;'>" + currentUserInformationBO.WorkingCostCenterId + "," + entityListBO[iTableNumber].TableId + "," + entityListBO[iTableNumber].StatusId + "</div>";
                    Content2 = @"<div class='TableNumberDiv'>" + entityListBO[iTableNumber].TableNumber + "</div></div>";

                    //                    Content0 = @"<a href='/Restaurant/frmKotBillMaster.aspx?Kot=BillCreateForTable:" + entityListBO[iTableNumber].TableId;
                    //                    Content1 = @"'><div class='NotDraggable RestaurantTableBookedDiv' style='width:" + entityListBO[iTableNumber].TableWidth + "px; height:" + entityListBO[iTableNumber].TableHeight + "px; top:" + entityListBO[iTableNumber].YCoordinate + "px; left:" + entityListBO[iTableNumber].XCoordinate + "px;' id='" + entityListBO[iTableNumber].TableManagementId + "'>";
                    //                    Content2 = @"<div class='RestaurantTableBookedDiv'>
                    //                                        </div>
                    //                                        <div class='TableNumberDiv'>" + entityListBO[iTableNumber].TableNumber + "</div></div></a>";

                }
                if (entityListBO[iTableNumber].StatusId == 3)
                {
                    Content0 = @"<a href='/POS/frmKotBillMaster.aspx?Kot=BillCreateForTable:" + entityListBO[iTableNumber].TableId;
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
                            <div id='SearchPanel' class='block'>
                                <a href='#' class='block-heading' data-toggle='collapse'>";

            string topTemplatePart = @"</a>
                                <div class='block-body collapse in'>           
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
                                        <a href='/POS/frmTokenKotBill.aspx?Kot=RestaurantItem:" + roomNumberListWithoutZeroBO[iItemCategory].CategoryId;
                }
                else
                {
                    Content1 = @"<div class='DivRestaurantItemContainer'>
                                        <a href='/POS/frmTokenKotBill.aspx?Kot=RestaurantItemCategory:" + roomNumberListWithoutZeroBO[iItemCategory].CategoryId;
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

                string topPart = @"<div class='divClear'>
                            </div>
                            <div id='SearchPanel' class='block'>
                                <a href='#' class='block-heading' data-toggle='collapse'>";

                string topTemplatePart = @"Item Information</a>
                                <div class='block-body collapse in'>           
                                ";

                string endTemplatePart = @"</div>
                            </div>";

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
        private void BillCreateForToken(string queryString)
        {
            if (!string.IsNullOrWhiteSpace(queryString))
            {
                int tableId = Convert.ToInt32(queryString);

                RestaurantTableBO entityTableBO = new RestaurantTableBO();
                RestaurantTableDA entityTableDA = new RestaurantTableDA();

                int tmpPkId;
                KotBillMasterBO entityBO = new KotBillMasterBO();
                KotBillMasterDA entityDA = new KotBillMasterDA();

                entityBO.SourceName = "RestaurantToken";
                entityBO.SourceId = tableId;

                this.txtTableIdInformation.Text = tableId.ToString();
                Session["txtTableIdInformation"] = tableId.ToString();

                UserInformationBO currentUserInformationBO = new UserInformationBO();
                currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

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

                //entityTableBO = entityTableDA.GetRestaurantTableInfoById(currentUserInformationBO.WorkingCostCenterId, "RestaurantTable", tableId);
                entityBO.CostCenterId = currentUserInformationBO.WorkingCostCenterId;

                RestaurantTokenDA billmasterda = new RestaurantTokenDA();
                string tokenNumber = billmasterda.GenarateRestaurantTokenNumber(entityBO.CostCenterId, currentUserInformationBO.UserInfoId);
                entityBO.TokenNumber = tokenNumber;
                //if (entityTableBO.StatusId == 0)
                //{
                    this.txtTableNumberInformation.Text = entityTableBO.TableNumber;
                    Session["txtTableNumberInformation"] = this.txtTableNumberInformation.Text;
                    Boolean status = entityDA.SaveKotBillMasterInfo(entityBO, out tmpPkId);
                    if (status)
                    {                        
                        this.txtKotIdInformation.Text = tmpPkId.ToString();
                        Session["txtKotIdInformation"] = this.txtKotIdInformation.Text;
                        //Response.Redirect("/Restaurant/frmKotBillMaster.aspx?Kot=RestaurantItemGroup:0");
                        Response.Redirect("/POS/frmTokenKotBill.aspx?Kot=RestaurantItemCategory:0");
                    }
                //}
                //else if (entityTableBO.StatusId == 1)
                //{
                //    this.txtTableNumberInformation.Text = entityTableBO.TableNumber;
                //    Session["txtTableNumberInformation"] = this.txtTableNumberInformation.Text;
                //    Boolean status = entityDA.SaveKotBillMasterInfo(entityBO, out tmpPkId);
                //    if (status)
                //    {

                //        this.txtKotIdInformation.Text = tmpPkId.ToString();
                //        Session["txtKotIdInformation"] = this.txtKotIdInformation.Text;
                //        //Response.Redirect("/Restaurant/frmKotBillMaster.aspx?Kot=RestaurantItemGroup:0");
                //        Response.Redirect("/Restaurant/frmTokenKotBill.aspx?Kot=RestaurantItemCategory:0");
                //    }
                //}
                //else if (entityTableBO.StatusId == 2)
                //{
                //    this.txtTableNumberInformation.Text = entityTableBO.TableNumber;
                //    Session["txtTableNumberInformation"] = this.txtTableNumberInformation.Text;
                //    entityBO = entityDA.GetKotBillMasterInfoByTableId("RestaurantTable", tableId);
                //    if (entityBO.KotId > 0)
                //    {
                //        Session["txtKotIdInformation"] = entityBO.KotId;
                //        //Response.Redirect("/Restaurant/frmKotBillMaster.aspx?Kot=RestaurantItemGroup:1");
                //        Response.Redirect("/Restaurant/frmTokenKotBill.aspx?Kot=RestaurantItemCategory:0");
                //    }
                //}
            }
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (userInformationBO != null)
            {
                ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
                objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmCostCenterChooseForKot.ToString());

                isSavePermission = objectPermissionBO.IsSavePermission;
                isDeletePermission = objectPermissionBO.IsDeletePermission;
                if (!isSavePermission)
                {
                    Response.Redirect("/Login.aspx");
                }
            }
            else
            {
                Response.Redirect("/Login.aspx");
            }
        }
        private void ReprintKotBill(int tableId)
        {
            HMUtility hmUtility = new HMUtility();
            KotBillMasterBO entityBO = new KotBillMasterBO();
            KotBillMasterDA entityMDA = new KotBillMasterDA();
            KotBillDetailDA entityDA = new KotBillDetailDA();

            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            Session["txtTableNumberInformation"] = this.txtTableNumberInformation.Text;
            entityBO = entityMDA.GetKotBillMasterInfoByTableId(currentUserInformationBO.WorkingCostCenterId, "RestaurantTable", tableId);
            if (entityBO.KotId > 0)
            {
                Session["txtTableIdInformation"] = tableId.ToString();
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

                    //UserInformationBO currentUserInformationBO = new UserInformationBO();
                    //HMUtility hmUtility = new HMUtility();
                    //currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
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
                        PrintReportKot(pinfo, entityBOList, true);
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
        [WebMethod]
        public static KotBillDetailBO UpdateIndividualItemDetailInformation(string updateType, int costCenterId, int editId, decimal quantity, string updatedContent)
        {
            HMUtility hmUtility = new HMUtility();

            KotBillDetailBO entityBO = new KotBillDetailBO();
            KotBillDetailDA entityDA = new KotBillDetailDA();
            KotBillDetailBO srcEntityBO = new KotBillDetailBO();

            RestaurantBuffetItemBO bItemEntityBO = new RestaurantBuffetItemBO();
            RestaurantBuffetItemDA bItemEntityDA = new RestaurantBuffetItemDA();
            RestaurantComboItemBO cItemEntityBO = new RestaurantComboItemBO();
            RestaurantComboItemDA cItemEntityDA = new RestaurantComboItemDA();
            InvItemBO itemEntityBO = new InvItemBO();
            InvItemDA itemEntityDA = new InvItemDA();

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
                        //itemEntityBO = itemEntityDA.GetInvItemInfoById(costCenterId, srcEntityBO.ItemId);
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
                        entityBO.Amount = quantity * Convert.ToDecimal(updatedContent);
                        entityBO.KotId = srcEntityBO.KotId;
                        entityBO.ItemId = srcEntityBO.ItemId;
                    }
                }

                entityBO.KotDetailId = editId;

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                entityBO.CreatedBy = userInformationBO.UserInfoId;

                Boolean status = entityDA.UpdateKotBillDetailInfo(0, entityBO, updateType);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), updateType, entityBO.ItemId,
                                    ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), updateType);
                }

            }
            return entityBO;
            //KotBillDetailBO entityBO = new KotBillDetailBO();
            //KotBillDetailDA entityDA = new KotBillDetailDA();
            //KotBillDetailBO srcEntityBO = new KotBillDetailBO();

            //RestaurantBuffetItemBO bItemEntityBO = new RestaurantBuffetItemBO();
            //RestaurantBuffetItemDA bItemEntityDA = new RestaurantBuffetItemDA();
            //RestaurantComboItemBO cItemEntityBO = new RestaurantComboItemBO();
            //RestaurantComboItemDA cItemEntityDA = new RestaurantComboItemDA();
            //InvItemBO itemEntityBO = new InvItemBO();
            //InvItemDA itemEntityDA = new InvItemDA();

            //srcEntityBO = entityDA.GetSrcRestaurantBillDetailInfoByKotDetailId(editId);
            //if (srcEntityBO.KotDetailId > 0)
            //{
            //    if (srcEntityBO.ItemType == "BuffetItem")
            //    {
            //        bItemEntityBO = bItemEntityDA.GetRestaurantBuffetInfoById(srcEntityBO.ItemId);
            //        entityBO.UnitRate = bItemEntityBO.BuffetPrice;
            //        entityBO.Amount = bItemEntityBO.BuffetPrice * Convert.ToInt32(updatedContent);
            //        entityBO.ItemUnit = Convert.ToInt32(updatedContent);
            //    }
            //    else if (srcEntityBO.ItemType == "ComboItem")
            //    {
            //        cItemEntityBO = cItemEntityDA.GetRestaurantComboInfoById(srcEntityBO.ItemId);
            //        entityBO.UnitRate = cItemEntityBO.ComboPrice;
            //        entityBO.Amount = cItemEntityBO.ComboPrice * Convert.ToInt32(updatedContent);
            //        entityBO.ItemUnit = Convert.ToInt32(updatedContent);
            //    }
            //    else if (srcEntityBO.ItemType == "IndividualItem")
            //    {
            //        if (updateType == "QuantityChange")
            //        {
            //            itemEntityBO = itemEntityDA.GetInvItemInfoById(costCenterId, srcEntityBO.ItemId);
            //            entityBO.UnitRate = itemEntityBO.UnitPriceLocal;
            //            entityBO.Amount = itemEntityBO.UnitPriceLocal * Convert.ToDecimal(updatedContent);
            //            entityBO.ItemUnit = Convert.ToDecimal(updatedContent);
            //        }
            //        else if (updateType == "ItemNameChange")
            //        {
            //            entityBO.ItemName = updatedContent;
            //        }
            //        else if (updateType == "UnitPriceChange")
            //        {
            //            entityBO.UnitRate = Convert.ToDecimal(updatedContent);
            //            entityBO.Amount = quantity * Convert.ToInt32(updatedContent);
            //        }
            //    }

            //    entityBO.KotDetailId = editId;

            //    Boolean status = entityDA.UpdateKotBillDetailInfo(entityBO, updateType);
            //}
            //return entityBO;
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
                List<KotBillDetailBO> files = entityDA.GetKotBillDetailInfoByKotNBearerId(costCenterId, "RestaurantToken", tableId, kotId);

                Boolean isChangedExist = false;
                foreach (KotBillDetailBO drIsChanged in files)
                {
                    if (drIsChanged.IsChanged)
                    {
                        isChangedExist = true;
                        break;
                    }
                }


                strTable += "<table cellspacing='0' cellpadding='4' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
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
                    strTable += "<td align='left' style='width: 15%;'>" + Math.Round(dr.Amount, 2) + "</td>";
                    
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
        [WebMethod]
        public static KotBillMasterBO UpdateTablePaxInformation(int costCenterId, int paxQuantity)
        {
            KotBillMasterBO entityBO = new KotBillMasterBO();
            if (costCenterId > 0)
            {
                KotBillMasterDA entityDA = new KotBillMasterDA();
                //RestaurantTableBO tableBO = new RestaurantTableBO();
                //RestaurantTableDA tableDA = new RestaurantTableDA();
                //tableBO = tableDA.GetRestaurantTableInfoByTableNumber(System.Web.HttpContext.Current.Session["txtTableNumberInformation"].ToString());
                int kotId = Convert.ToInt32(System.Web.HttpContext.Current.Session["txtKotIdInformation"].ToString());

                if (kotId > 0)
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
        [WebMethod]
        public static string LoadOccupiedPossiblePath(string KotNTableNumber, string pageTitle)
        {
            HMUtility hmUtility = new HMUtility();
            Boolean isWMDeletePermission = false;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmCostCenterChooseForKot.ToString());
            isWMDeletePermission = objectPermissionBO.IsDeletePermission;

            //int kotId = Convert.ToInt32(System.Web.HttpContext.Current.Session["txtKotIdInformation"].ToString());

            string strTable = string.Empty;
            string[] costCenterNTableNumber = KotNTableNumber.Split(',');

            List<TableManagementBO> tableList = new List<TableManagementBO>();
            tableList = new TableManagementDA().GetTableInfoByCostCenterNStatus(Convert.ToInt32(costCenterNTableNumber[0]), 1);

            int kotId = 0;
            KotBillMasterDA masterDa = new KotBillMasterDA();
            kotId = masterDa.GETKotIDByCostNTableID(Convert.ToInt32(costCenterNTableNumber[1]), Convert.ToInt32(costCenterNTableNumber[0]));

            PrinterInfoDA da = new PrinterInfoDA();
            List<PrinterInfoBO> files = da.GetRestaurentItemTypeInfoByKotIdForReprint(kotId);
            var v = files.Where(s => s.PrintFlag == true).ToList();

            // strTable += "<a href='#page-stats' class='block-heading' data-toggle='collapse'>" + pageTitle + " </a>";
            strTable += "<div style='padding:10px'>";

            strTable += "<div style='float:left;padding-left:10px; padding-bottom:20px;width:115px'>";
            strTable += "<input type='button' style='width:115px' value='" + "KOT Continue" + "' class='TransactionalButton btn btn-primary'";
            strTable += " onclick=\" location.href='" + "/Restaurant/frmTokenKotBill.aspx?Kot=BillCreateForTable:" + costCenterNTableNumber[1] + "';\"  />";
            strTable += "</div>";

            strTable += "<div style='float:left;padding-left:10px; padding-bottom:20px;width:115px'>";
            strTable += "<input type='button' style='width:115px' value='" + "Table Change" + "' class='TransactionalButton btn btn-primary'";
            strTable += " onclick=\"return OpenTableShiptPanel('" + costCenterNTableNumber[0] + "', '" + costCenterNTableNumber[1] + "' );\"  />";
            strTable += "</div>";

            if (isWMDeletePermission)
            {
                strTable += "<div style='float:left;padding-left:10px;width:115px'>";
                strTable += "<input type='button' style='width:115px' value='" + "Table Clean" + "' class='TransactionalButton btn btn-primary'";
                strTable += " onclick=\"return OpenTableCleanPanel('" + costCenterNTableNumber[0] + "', '" + costCenterNTableNumber[1] + "' );\"  />";
                strTable += "</div>";
            }
            else
            {
                if (costCenterNTableNumber[2] == "2" && v.Count == 0)
                {
                    strTable += "<div style='float:left;padding-left:10px;width:115px'>";
                    strTable += "<input type='button' style='width:115px' value='" + "Table Clean" + "' class='TransactionalButton btn btn-primary'";
                    strTable += " onclick=\"return OpenTableCleanPanel('" + costCenterNTableNumber[0] + "', '" + costCenterNTableNumber[1] + "' );\"  />";
                    strTable += "</div>";
                }
            }

            strTable += "<div style='float:left;padding-left:10px; padding-bottom:20px;width:115px'>";
            strTable += "<input type='button' style='width:115px' value='" + "Table Bill" + "' class='TransactionalButton btn btn-primary'";
            strTable += " onclick=\" location.href='" + "/Restaurant/frmRestaurantBill.aspx?tableId=" + costCenterNTableNumber[1] + "';\"  />";
            strTable += "</div>";

            if (!isWMDeletePermission)
            {
                if (costCenterNTableNumber[2] == "2" && v.Count == 0)
                {
                    strTable += "<div class='divClear'></div>";
                }
            }

            strTable += "<div style='float:left;padding-left:10px; padding-bottom:5px;width:115px'>";
            strTable += "<input type='button' style='width:115px' value='" + "KOT Reprint" + "' class='TransactionalButton btn btn-primary'";
            strTable += " onclick=\" location.href='" + "/Restaurant/frmTokenKotBill.aspx?Kot=KotReprint:" + costCenterNTableNumber[1] + "';\"  />";
            strTable += "</div>";

            if (!isWMDeletePermission)
            {
                if (costCenterNTableNumber[2] == "2" && v.Count > 0)
                {
                    strTable += "<div class='divClear'></div>";
                }
            }

            strTable += "<div style='float:left;padding-left:10px; padding-bottom:5px;width:147px'>";
            strTable += "<input type='button' style='width:115px' value='" + "KOT Preview" + "' class='TransactionalButton btn btn-primary'";
            strTable += " onclick=\"javascript:return KotPrintPreview(" + costCenterNTableNumber[1] + ")\"  />";
            strTable += "</div>";

            strTable += "<div class='divClear'></div>";

            strTable += "<div style='font-weight: bold; display:none; height:60px;' class='block' id='TableShiftInfo'>";

            strTable += "<div style='width:500px; margin-left:5px; margin-top:20px;' >";
            string availAbleTable = string.Empty;
            if (tableList.Count() > 0)
            {
                availAbleTable = "<select id='ddlAvailableTable'>";
                foreach (TableManagementBO tb in tableList)
                {
                    availAbleTable += "<option value='" + tb.TableId + "'>" + tb.TableNumber + "</option>";
                }
                availAbleTable += "</select>";

                strTable += "Available Table:&nbsp;&nbsp;" +
                             availAbleTable;
            }

            strTable += "<input type='button' style='width:130px' value='" + "Change" + "' class='TransactionalButton btn btn-primary'";
            strTable += " onclick=\"return UpdateTableShift('" + costCenterNTableNumber[0] + "', '" + costCenterNTableNumber[1] + "' );\"  />";
            strTable += "</div> </div>";

            //strTable += "<div style='font-weight: bold; display:none; height:60px;' class='block' id='CleanRemarks'>";

            strTable += "<div style='font-weight: bold; display:none; height:60px;' class='block' id='TableCleanInfo'>";

            strTable += "<div style='float:left;padding-left:28px; padding-bottom:5px; margin-top:10px; margin-bottom:10px; width:150px'>";
            //strTable += "<input type='label' style='width:150px' value='" + "Remarks" + "'";
            //strTable += "<input type='text' style='width:150px'";

            strTable += "<div style='float:left;padding-left:28px; padding-bottom:5px; margin-top:10px; width:150px'>";
            strTable += "<input type='button' style='width:150px' value='" + "Clean Table" + "' class='TransactionalButton btn btn-primary'";
            strTable += " onclick=\"return UpdateTableStatus('" + costCenterNTableNumber[0] + "', '" + costCenterNTableNumber[1] + "' );\"  />";
            strTable += "</div>";
            strTable += "</div>";

            strTable += "</div>";

            return strTable;
        }
        [WebMethod]
        public static bool UpdateRestaurantTableStatus(int costCenterId, int tableId, int statusId)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            int lastModifiedBy = userInformationBO.UserInfoId;

            KotBillMasterDA entityDA = new KotBillMasterDA();

            bool updateStatus = entityDA.UpdateRestaurantTableStatus(costCenterId, tableId, statusId, lastModifiedBy);

            return updateStatus;
        }
        [WebMethod]
        public static bool UpdateRestaurantTableShift(int costCenterId, int oldTableId, int newTableId)
        {
            KotBillMasterDA entityDA = new KotBillMasterDA();

            bool updateStatus = entityDA.UpdateRestaurantTableShift(costCenterId, oldTableId, newTableId);

            return updateStatus;
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
        private int m_currentPageIndex;
        private IList<Stream> m_streams;
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
                //string kotDate = String.Format("{0:f}", dateTime);
                string kotDate = hmUtility.GetStringFromDateTime(dateTime) + " " + dateTime.ToString("hh:mm:ss tt");


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

                KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();

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

                            
                            KotBillMasterBO kotBillMasterBO = new KotBillMasterBO();
                            kotBillMasterBO = kotBillMasterDA.GetKotBillMasterInfoKotId(kotId);
                            if (kotBillMasterBO.KotId > 0)
                            {
                                dateTime = kotBillMasterBO.KotDate;
                                kotDate = hmUtility.GetStringFromDateTime(dateTime) + " " + dateTime.ToString("hh:mm:ss tt");
                            }
                        }
                        else
                        {
                            reportTitle = "KOT";
                        }
                    }
                }

                KotBillMasterBO waiterInformationBO = new KotBillMasterBO();
                waiterInformationBO = kotBillMasterDA.GetWaiterInformationByKotId(kotId);
                if (waiterInformationBO != null)
                {
                    waiterName = waiterInformationBO.WaiterName;
                }

                ReportParameter[] parms = new ReportParameter[9];
                parms[0] = new ReportParameter("ReportTitle", reportTitle);
                parms[1] = new ReportParameter("CostCenter", strCostCenterName);
                parms[2] = new ReportParameter("SourceName", "Token # ");
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

                //report.DataSources.Add(new ReportDataSource("KotOrderSubmit", entityBOList));

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
        private Stream CreateStream(string name,
        string fileNameExtension, Encoding encoding,
        string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }
        public void Export(LocalReport report)
        {
            string deviceInfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>3.5in</PageWidth>
                <PageHeight>11in</PageHeight>
                <MarginTop>0.25in</MarginTop>
                <MarginLeft>0.25in</MarginLeft>
                <MarginRight>0.25in</MarginRight>
                <MarginBottom>0.25in</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }
        private void Print(string printerName)
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings.PrinterName = printerName;  // "Samsung ML-1860 Series";

            PaperSize pkCustomSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", 300, 1400);
            printDoc.DefaultPageSettings.PaperSize = pkCustomSize;
            printDoc.DefaultPageSettings.Margins.Top = 0;
            printDoc.DefaultPageSettings.Margins.Bottom = 0;
            printDoc.DefaultPageSettings.Margins.Left = 0;
            printDoc.DefaultPageSettings.Margins.Right = 0;

            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }
        protected void btnPrintFromServer_Click(object sender, EventArgs e)
        {
            try
            {
                KotBillDetailDA entityDA = new KotBillDetailDA();
                PrinterInfoDA da = new PrinterInfoDA();

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

                        UserInformationBO currentUserInformationBO = new UserInformationBO();
                        HMUtility hmUtility = new HMUtility();
                        currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                        waiterName = currentUserInformationBO.UserName.ToString();

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
                            PrintReportKot(pinfo, entityBOList, false);
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
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, ex.Message, AlertType.Error);
                
            }
        }
        protected void btnKotPrintPreview_Click(object sender, EventArgs e)
        {
            string url = "/POS/Reports/frmReportKotBillInfo.aspx?kotId=" + txtKotIdInformation.Text + "&tno=" + txtTableIdInformation.Text + "&kbm=" + "kotm";
            string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemSearch(string itemName, int costCenterId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemNameWiseItemDetailsForAutoSearch(itemName, costCenterId,ConstantHelper.CustomerSupplierAutoSearch.CustomerItem.ToString());

            return itemInfo;
        }
        protected void btnKotRePrintPreview_Click(object sender, EventArgs e)
        {
            int tableId = Convert.ToInt32(hfTableIdForPrint.Value);
            int kotId = 0;

            KotBillMasterBO entityBO = new KotBillMasterBO();
            KotBillMasterDA entityMDA = new KotBillMasterDA();
            KotBillDetailDA entityDA = new KotBillDetailDA();

            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            entityBO = entityMDA.GetKotBillMasterInfoByTableId(currentUserInformationBO.WorkingCostCenterId, "RestaurantTable", tableId);
            if (entityBO.KotId > 0)
            {
                kotId = entityBO.KotId;
            }
            else
                return;

            string url = "/POS/Reports/frmReportKotBillInfo.aspx?kotId=" + kotId.ToString() + "&tno=" + tableId.ToString() + "&kbm=" + "kotm" + "&isrp=" + "rp";
            string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
    }
}