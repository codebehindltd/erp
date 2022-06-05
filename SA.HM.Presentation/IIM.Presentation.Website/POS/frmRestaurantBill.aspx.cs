using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.UserInformation;
using System.Web.Services;
using HotelManagement.Entity;
using HotelManagement.Data.Payroll;
using System.Collections;
using Microsoft.Reporting.WebForms;
using System.Security;
using System.Security.Permissions;
using HotelManagement.Data.Membership;
using HotelManagement.Entity.Membership;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmRestaurantBill : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        private int strPrinterInfoId = 0;
        private int strCostCenterId = 0;
        private int kotIdInformation = 0;
        private Boolean IsRestaurantOrderSubmitDisable = false;
        private Boolean IsRestaurantTokenInfoDisable = false;
        private string tableNumberInformation = string.Empty;
        private string strCostCenterName = string.Empty;
        private string strCostCenterDefaultView = string.Empty;
        private string strPrinterName = string.Empty;
        private string waiterName = string.Empty;
        private string companyName = string.Empty;
        protected int isRoomNumberBoxEnable = -1;
        protected int isBillProcessedBoxEnable = -1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        HMUtility hmUtility = new HMUtility();
        protected int isBillProcessedForTokenSystem = -1;
        protected int isBillProcessedForMember = -1;
        UserInformationDA userInformationDA = new UserInformationDA();
        private string homePanelPathInfo = "frmCostCenterChooseForKot.aspx";
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddMoreBill.Visible = false;
            chkIsBillSplit.Visible = false;
            lblIsBillSplit.Visible = false;
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            string costCenterId = string.Empty;
            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (!string.IsNullOrWhiteSpace(Request.QueryString["CostCenterId"]))
            {
                costCenterId = Request.QueryString["CostCenterId"];

                Boolean status = userInformationDA.UpdateUserWorkingCostCenterInfo("SecurityUser", currentUserInformationBO.UserInfoId, Convert.ToInt32(costCenterId));
                if (status)
                {
                    currentUserInformationBO.WorkingCostCenterId = Convert.ToInt32(costCenterId);
                    Session.Add("UserInformationBOSession", currentUserInformationBO);
                }
                if (Request.QueryString["tableId"] != null)
                {
                    Response.Redirect("/POS/frmRestaurantBill.aspx?tableId=" + Request.QueryString["tableId"]);
                }
                else if (Request.QueryString["tokenId"] != null)
                {
                    Response.Redirect("/POS/frmRestaurantBill.aspx?tokenId=" + Request.QueryString["tokenId"]);
                }
                else if (Request.QueryString["RoomNumber"] != null)
                {
                    Response.Redirect("/POS/frmRestaurantBill.aspx?RoomNumber=" + Request.QueryString["RoomNumber"]);
                }
                else
                {
                    Response.Redirect("frmCostCenterChooseForTS.aspx");
                }
            }

            hfCostCenterId.Value = currentUserInformationBO.WorkingCostCenterId.ToString();

            if (!IsPostBack)
            {
                LoadCurrency();
                LoadRestaurantBearerInfo();
                LoadIsConversionRateEditable();
                IsLocalCurrencyDefaultSelected();
                LoadLocalCurrencyId();
                LoadBusinessPromotion();
                IsRestaurantBillAmountWillRound();
                IsRestaurantPaymentModeCashDefault();
                LoadRackRateServiceChargeVatPanelInformation();
                LoadCommonSetupForRackRateServiceChargeVatInformation();
                GuestPaymentInformationDiv.Visible = false;
                LoadBank();
                LoadMemberList();
                LoadAccountHeadInfo();
                HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = null;
                HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] = null;
                HttpContext.Current.Session["CompareCategoryWisePercentageDiscountInfo"] = null;
                IsRestaurantIntegrateWithPayroll();
                IsRestaurantIntegrateWithFrontOffice();
                if (IsRestaurantIntegrateWithPayrollVal.Value != "1")
                {
                    ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("Employee"));
                }
                if (hfIsRestaurantIntegrateWithFrontOffice.Value != "1")
                {
                    ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("Other Room"));
                }

                LoadPayRoomInfo();
                LoadEmployeeInfo();
                LoadRegisteredGuestCompanyInfo();
                string cardValidation = System.Web.Configuration.WebConfigurationManager.AppSettings["CardValidation"].ToString();
                txtCardValidation.Value = cardValidation.ToString();

                if (Request.QueryString["tokenId"] != null)
                {
                    SourceIdHiddenField.Value = Request.QueryString["tokenId"];
                }
                else if (Request.QueryString["tableId"] != null)
                {
                    SourceIdHiddenField.Value = Request.QueryString["tableId"];
                    ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("3"));
                }
                else if (Request.QueryString["RoomNumber"] != null)
                {
                    ddlPayMode.SelectedValue = "Other Room";
                    SourceIdHiddenField.Value = Request.QueryString["RoomNumber"];
                    hfComplimentaryRoomNumber.Value = Request.QueryString["RoomNumber"];
                    if (hfIsRestaurantIntegrateWithFrontOffice.Value == "1")
                    {
                        ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("Employee"));

                        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                        RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                        roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(SourceIdHiddenField.Value);
                        if (roomAllocationBO.RoomId > 0)
                        {
                            this.txtCustomerName.Text = "Name: " + roomAllocationBO.GuestName + ", Room# " + SourceIdHiddenField.Value;
                        }

                        RoomNumberDA roomNumberDA = new RoomNumberDA();
                        List<RoomNumberBO> files = roomNumberDA.GetRoomNumberInfoBySearchCriteria(SourceIdHiddenField.Value);
                        if (files != null)
                        {
                            List<RoomNumberBO> roomNumberBOList = new List<RoomNumberBO>();
                            List<RoomNumberBO> newRoomNumberBOList = new List<RoomNumberBO>();
                            if (files.Count == 1)
                            {
                                if (hfIsRestaurantIntegrateWithFrontOffice.Value == "1")
                                {
                                    roomNumberBOList = roomNumberDA.GetRoomNumberInfoByCondition(0, 0);
                                }

                                foreach (RoomNumberBO row in files)
                                {
                                    newRoomNumberBOList = roomNumberBOList.Where(a => a.RoomId == row.RoomId).ToList();
                                    ddlRoomNumberId.DataSource = newRoomNumberBOList;
                                    ddlRoomNumberId.DataTextField = "RoomNumber";
                                    ddlRoomNumberId.DataValueField = "RoomId";
                                    ddlRoomNumberId.DataBind();
                                    ddlRoomNumberId.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                }

                CheckObjectPermission();
                LoadGridView();
                LoadWaiterAndKotNumber();
                LoadBank();
                CalculateAmountTotal();
                LoadRoomNumber();
                LoadCommonDropDownHiddenField();

                int currentCostCenterId = !string.IsNullOrWhiteSpace(hfCostCenterId.Value) ? Convert.ToInt32(hfCostCenterId.Value) : 0;
                int currentTableId = !string.IsNullOrWhiteSpace(SourceIdHiddenField.Value) ? Convert.ToInt32(SourceIdHiddenField.Value) : 0;
                isBillProcessedForMember = -1;
                CheckingRestaurantBill(currentCostCenterId, currentTableId);
                CheckingGroupTable();
            }

            imgBtnRoomDesign.Visible = true;

            if (Request.QueryString["tableId"] != null)
            {
                imgBtnRoomDesign.Visible = false;
                imgBtnTableDesign.Visible = true;
                AddMoreTableForBillPanel.Visible = true;
                pnlAlreadyAddedNewTable.Visible = true;
            }
            else
            {
                imgBtnTableDesign.Visible = false;
                AddMoreTableForBillPanel.Visible = false;
                pnlAlreadyAddedNewTable.Visible = false;

                if (Request.QueryString["tokenId"] != null)
                {
                    imgBtnRoomDesign.Visible = false;
                    btnAddMoreBill.Visible = true;
                    if (string.IsNullOrWhiteSpace(txtBillIdForInvoicePreview.Value))
                    {
                        btnSave.Visible = false;
                        btnPaymentPosting.Visible = true;
                        GuestPaymentInformationDiv.Visible = true;
                        isBillProcessedForTokenSystem = 1;
                    }
                    else
                    {
                        Response.Redirect(homePanelPathInfo);
                    }
                }
            }

            LoadHomePanelButtonInfo();
        }
        protected void gvRestaurantSavedBill_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                /*  imgUpdate.Visible = isSavePermission;
                  imgDelete.Visible = isDeletePermission;*/
            }
        }
        protected void gvRestaurantSavedBill_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            /* lblMessage.Text = string.Empty;
             gvBearerInformation.PageIndex = e.NewPageIndex;
             LoadGridView();*/
        }
        protected void gvRestaurantSavedBill_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdBillPreview")
            {
                int billID = Int32.Parse(txtBillId.Value);
                int aaa = Convert.ToInt32(e.CommandArgument.ToString());
                string url = "/POS/Reports/frmReportBillInfo.aspx?billID=" + billID;
                string s = "window.open('" + url + "', 'popup_window', 'width=640,height=680,left=300,top=50,resizable=yes');";
                ClientScript.RegisterStartupScript(GetType(), "script", s, true);
            }
        }
        protected void gvRestaurentBill_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            /*if (e.Row.DataItem != null)
              {
                  Label lblValue = (Label)e.Row.FindControl("lblid");
                  ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                  ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                  imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                  imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                  imgUpdate.Visible = isSavePermission;
                  imgDelete.Visible = isDeletePermission;
              }*/
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Cancel();
        }
        protected void btnSrcRoomNumber_Click(object sender, EventArgs e)
        {
            isRoomNumberBoxEnable = 1;
        }
        protected void ddlRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRelatedInformation();
        }
        protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadAccountHeadInfo();
        }
        protected void btnCategoryWiseDiscountOK_Click(object sender, EventArgs e)
        {
            int counter = 0;
            List<InvCategoryBO> discountCategoryBOList = new List<InvCategoryBO>();

            foreach (GridViewRow row in gvPercentageDiscountCategory.Rows)
            {
                counter = counter + 1;
                bool isChecked = ((CheckBox)row.FindControl("chkIsCheckedPermission")).Checked;
                Label lblidValue = (Label)row.FindControl("lblid");
                InvCategoryBO discountCategoryBO = new InvCategoryBO();
                discountCategoryBO.CategoryId = Convert.ToInt32(lblidValue.Text);
                discountCategoryBOList.Add(discountCategoryBO);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            LoadHomePanelButtonInfo();

            List<string> strCategoryIdList = new List<string>();
            string categoryIdList = string.Empty;
            int isRestaurantBillInclusive = !string.IsNullOrWhiteSpace(hfIsRestaurantBillInclusive.Value) ? Convert.ToInt32(hfIsRestaurantBillInclusive.Value) : 0;
            KotBillMasterDA entityDA = new KotBillMasterDA();
            RestaurantBillBO restaurentBillBO = new RestaurantBillBO();
            List<RestaurantBillBO> categoryWisePercentageDiscountBOList = new List<RestaurantBillBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (hfIsCustomBearerInfoEnable.Value == "1")
            {
                restaurentBillBO.BearerId = Convert.ToInt32(ddlWaiterNameDisplay.SelectedValue);
            }
            else
            {
                restaurentBillBO.BearerId = 0;
            }

            if (cbIsComplementary.Checked)
            {
                HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
            }

            if (HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] != null)
            {
                strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                foreach (string rowCWPD in strCategoryIdList)
                {
                    RestaurantBillBO categoryWisePercentageDiscountBO = new RestaurantBillBO();
                    categoryWisePercentageDiscountBO.ClassificationId = Convert.ToInt32(rowCWPD);
                    categoryWisePercentageDiscountBOList.Add(categoryWisePercentageDiscountBO);
                    categoryIdList += categoryIdList != string.Empty ? ("," + rowCWPD) : rowCWPD;
                }
            }

            if (string.IsNullOrWhiteSpace(txtBillIdForInvoicePreview.Value))
            {
                if (!IsFrmValid())
                {
                    return;
                }

                restaurentBillBO.CostCenterId = Convert.ToInt32(hfCostCenterId.Value);
                restaurentBillBO.DiscountType = ddlDiscountType.SelectedValue.ToString();

                if (restaurentBillBO.DiscountType == "Fixed")
                {
                    restaurentBillBO.DiscountTransactionId = 0;
                }
                else if (restaurentBillBO.DiscountType == "Percentage")
                {
                    restaurentBillBO.DiscountTransactionId = 0;
                }
                else if (restaurentBillBO.DiscountType == "Member")
                {
                    string[] ddlMemberIdSelectedValue = ddlMemberId.SelectedValue.Split(',');
                    int memMemberId = Convert.ToInt32(ddlMemberIdSelectedValue[0]);
                    restaurentBillBO.DiscountTransactionId = memMemberId;
                }
                else if (restaurentBillBO.DiscountType == "BusinessPromotion")
                {
                    restaurentBillBO.DiscountTransactionId = Convert.ToInt32(ddlBusinessPromotionId.SelectedValue.Split('~')[0]);
                }

                restaurentBillBO.DiscountAmount = Convert.ToDecimal(txtDiscountAmount.Text);
                restaurentBillBO.CalculatedDiscountAmount = !string.IsNullOrWhiteSpace(txtCalculatedDiscountAmount.Value) ? Convert.ToDecimal(txtCalculatedDiscountAmount.Value) : 0;
                restaurentBillBO.ServiceCharge = !string.IsNullOrWhiteSpace(txtServiceChargeAmountHiddenField.Value) ? Convert.ToDecimal(txtServiceChargeAmountHiddenField.Value) : 0; //Convert.ToDecimal(txtServiceCharge.Text);
                restaurentBillBO.VatAmount = !string.IsNullOrWhiteSpace(txtVatAmountHiddenField.Value) ? Convert.ToDecimal(txtVatAmountHiddenField.Value) : 0; //Convert.ToDecimal(txtVatAmount.Text);
                restaurentBillBO.CustomerName = txtCustomerName.Text;
                restaurentBillBO.PaxQuantity = !string.IsNullOrWhiteSpace(txtPaxQuantity.Text) ? Convert.ToInt32(txtPaxQuantity.Text) : 1;
                restaurentBillBO.CreatedBy = userInformationBO.UserInfoId;
                restaurentBillBO.TableId = Int32.Parse(SourceIdHiddenField.Value);
                restaurentBillBO.BillDate = DateTime.Now;
                restaurentBillBO.BillPaymentDate = DateTime.Now;

                if (Request.QueryString["tokenId"] != null)
                {
                    restaurentBillBO.SourceName = "RestaurantToken";
                    restaurentBillBO.BillPaidBySourceId = Int32.Parse(Request.QueryString["tokenId"]);
                }
                else if (Request.QueryString["tableId"] != null)
                {
                    restaurentBillBO.SourceName = "RestaurantTable";
                    restaurentBillBO.BillPaidBySourceId = Int32.Parse(Request.QueryString["tableId"]);
                }
                else if (Request.QueryString["RoomNumber"] != null)
                {
                    restaurentBillBO.SourceName = "GuestRoom";
                    restaurentBillBO.BillPaidBySourceId = Int32.Parse(Request.QueryString["RoomNumber"]);
                }

                if (cbServiceCharge.Checked)
                {
                    restaurentBillBO.IsInvoiceServiceChargeEnable = true;
                }
                else
                {
                    restaurentBillBO.IsInvoiceServiceChargeEnable = false;
                }

                if (cbVatAmount.Checked)
                {
                    restaurentBillBO.IsInvoiceVatAmountEnable = true;
                }
                else
                {
                    restaurentBillBO.IsInvoiceVatAmountEnable = false;
                }

                if (isRestaurantBillInclusive == 0)
                {
                    restaurentBillBO.SalesAmount = !string.IsNullOrWhiteSpace(txtSalesAmount.Text) ? Convert.ToDecimal(txtSalesAmount.Text) : 0;
                    restaurentBillBO.GrandTotal = Convert.ToDecimal(txtGrandTotalHiddenField.Value);
                }
                else
                {
                    restaurentBillBO.SalesAmount = !string.IsNullOrWhiteSpace(txtSalesAmount.Text) ? Convert.ToDecimal(txtSalesAmount.Text) : 0;
                    restaurentBillBO.GrandTotal = !string.IsNullOrWhiteSpace(txtSalesAmount.Text) ? Convert.ToDecimal(txtSalesAmount.Text) - restaurentBillBO.CalculatedDiscountAmount : 0;
                }

                if (cbIsComplementary.Checked)
                { restaurentBillBO.IsComplementary = true; }
                else
                { restaurentBillBO.IsComplementary = false; }

                if (restaurentBillBO.DiscountType == "Percentage" && restaurentBillBO.DiscountAmount == 100)
                {
                    restaurentBillBO.IsComplementary = true;
                }

                List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();
                KotBillMasterBO entityBO = new KotBillMasterBO();

                if (Request.QueryString["tokenId"] != null)
                {
                    if (hfSelectedTableId.Value != "")
                        hfSelectedTableId.Value = Request.QueryString["tokenId"] + "," + hfSelectedTableId.Value;
                    else
                        hfSelectedTableId.Value = Request.QueryString["tokenId"];

                    string[] tableId = hfSelectedTableId.Value.Split(',');
                    string tablesId = string.Empty, kotIdList = string.Empty;

                    for (int i = 0; i < tableId.Count(); i++)
                    {
                        entityBO = entityDA.GetKotBillMasterInfoByTableId(Convert.ToInt32(hfCostCenterId.Value), "RestaurantToken", int.Parse(tableId[i]));
                        RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                        restaurentBillDetailBO.KotId = entityBO.KotId;
                        restaurentBillDetailBOList.Add(restaurentBillDetailBO);
                    }

                    restaurentBillBO.KotId = restaurentBillDetailBOList[0].KotId;
                }
                else if (Request.QueryString["tableId"] != null)
                {
                    if (hfSelectedTableId.Value != "")
                        hfSelectedTableId.Value = Request.QueryString["tableId"] + "," + hfSelectedTableId.Value;
                    else
                        hfSelectedTableId.Value = Request.QueryString["tableId"];

                    string[] tableId = hfSelectedTableId.Value.Split(',');
                    string tablesId = string.Empty, kotIdList = string.Empty;

                    for (int i = 0; i < tableId.Count(); i++)
                    {
                        entityBO = entityDA.GetKotBillMasterInfoByTableId(restaurentBillBO.CostCenterId, "RestaurantTable", int.Parse(tableId[i]));

                        RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                        restaurentBillDetailBO.KotId = entityBO.KotId;
                        restaurentBillDetailBOList.Add(restaurentBillDetailBO);
                    }

                    restaurentBillBO.KotId = restaurentBillDetailBOList[0].KotId;
                }
                else if (Request.QueryString["RoomNumber"] != null)
                {
                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                    RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                    roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(Request.QueryString["RoomNumber"]);
                    if (roomAllocationBO.RoomId > 0)
                    {
                        entityBO = entityDA.GetKotBillMasterInfoByTableId(restaurentBillBO.CostCenterId, "GuestRoom", roomAllocationBO.RegistrationId);
                        RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                        restaurentBillDetailBO.KotId = entityBO.KotId;
                        restaurentBillDetailBOList.Add(restaurentBillDetailBO);
                    }
                }

                bool isBillSettlement = entityDA.GetIsBillExistsByKotId(entityBO.KotId, restaurentBillBO.BillPaidBySourceId);

                if (isBillSettlement)
                {
                    Response.Redirect("frmRestaurantBill.aspx?tableId=" + restaurentBillBO.BillPaidBySourceId.ToString());
                }

                int SourceId = 0;
                SourceId = Int32.Parse(SourceIdHiddenField.Value);
                if (SourceId > 0)
                {
                    List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
                    RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
                    int billID = 0;

                    int success = restaurentBillDA.SaveRestaurantBill(restaurentBillBO, restaurentBillDetailBOList, guestPaymentDetailListForGrid, categoryWisePercentageDiscountBOList, categoryIdList, false, true, out billID);
                    txtBillId.Value = success.ToString();
                    if (success > 0)
                    {
                        string url = "/POS/Reports/frmReportBillInfo.aspx?billID=" + txtBillId.Value;
                        string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes');";
                        ClientScript.RegisterStartupScript(GetType(), "script", s, true);
                    }
                }
                GuestPaymentInformationDiv.Visible = true;

                int currentCostCenterId = !string.IsNullOrWhiteSpace(hfCostCenterId.Value) ? Convert.ToInt32(hfCostCenterId.Value) : 0;
                int currentTableId = !string.IsNullOrWhiteSpace(SourceIdHiddenField.Value) ? Convert.ToInt32(SourceIdHiddenField.Value) : 0;
                CheckingRestaurantBill(currentCostCenterId, currentTableId);

                LoadGridView();

            }
            else
            {
                if (!string.IsNullOrWhiteSpace(txtBillIdForInvoicePreview.Value))
                {
                    RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
                    restaurentBillBO.BillId = Convert.ToInt32(txtBillIdForInvoicePreview.Value);
                    restaurentBillBO.CustomerName = txtCustomerName.Text;
                    restaurentBillBO.BillDate = DateTime.Now;
                    restaurentBillBO.BillPaymentDate = DateTime.Now;
                    restaurentBillBO.DiscountType = ddlDiscountType.SelectedValue.ToString();
                    restaurentBillBO.CostCenterId = Convert.ToInt32(hfCostCenterId.Value);

                    if (restaurentBillBO.DiscountType == "Fixed")
                    {
                        restaurentBillBO.DiscountTransactionId = 0;
                    }
                    else if (restaurentBillBO.DiscountType == "Percentage")
                    {
                        restaurentBillBO.DiscountTransactionId = 0;
                    }
                    else if (restaurentBillBO.DiscountType == "Member")
                    {
                        string[] ddlMemberIdSelectedValue = ddlMemberId.SelectedValue.Split(',');
                        int memMemberId = Convert.ToInt32(ddlMemberIdSelectedValue[0]);
                        restaurentBillBO.DiscountTransactionId = memMemberId;
                    }
                    else if (restaurentBillBO.DiscountType == "BusinessPromotion")
                    {
                        restaurentBillBO.DiscountTransactionId = Convert.ToInt32(ddlBusinessPromotionId.SelectedValue);
                    }

                    restaurentBillBO.DiscountAmount = !string.IsNullOrWhiteSpace(txtDiscountAmount.Text) ? Convert.ToDecimal(txtDiscountAmount.Text) : 0; //Convert.ToDecimal(txtDiscountAmount.Text);
                    restaurentBillBO.CalculatedDiscountAmount = !string.IsNullOrWhiteSpace(txtCalculatedDiscountAmount.Value) ? Convert.ToDecimal(txtCalculatedDiscountAmount.Value) : 0;
                    restaurentBillBO.ServiceCharge = !string.IsNullOrWhiteSpace(txtServiceChargeAmountHiddenField.Value) ? Convert.ToDecimal(txtServiceChargeAmountHiddenField.Value) : 0; //Convert.ToDecimal(txtServiceCharge.Text);
                    restaurentBillBO.VatAmount = !string.IsNullOrWhiteSpace(txtVatAmountHiddenField.Value) ? Convert.ToDecimal(txtVatAmountHiddenField.Value) : 0; //Convert.ToDecimal(txtVatAmount.Text);
                    restaurentBillBO.PaxQuantity = !string.IsNullOrWhiteSpace(txtPaxQuantity.Text) ? Convert.ToInt32(txtPaxQuantity.Text) : 1;

                    if (Request.QueryString["tokenId"] != null)
                    {
                        restaurentBillBO.SourceName = "RestaurantToken";
                        restaurentBillBO.BillPaidBySourceId = Int32.Parse(Request.QueryString["tokenId"]);
                    }
                    if (Request.QueryString["tableId"] != null)
                    {
                        restaurentBillBO.SourceName = "RestaurantTable";
                        restaurentBillBO.BillPaidBySourceId = Int32.Parse(Request.QueryString["tableId"]);

                        KotBillMasterBO billMasterBO = new KotBillMasterBO();
                        billMasterBO = entityDA.GetKotBillMasterInfoByTableId(Convert.ToInt32(hfCostCenterId.Value), "RestaurantTable", restaurentBillBO.BillPaidBySourceId);

                        restaurentBillBO.KotId = billMasterBO.KotId;
                    }
                    if (Request.QueryString["RoomNumber"] != null)
                    {
                        restaurentBillBO.SourceName = "GuestRoom";
                        restaurentBillBO.BillPaidBySourceId = Int32.Parse(Request.QueryString["RoomNumber"]);
                    }

                    if (cbServiceCharge.Checked)
                    {
                        restaurentBillBO.IsInvoiceServiceChargeEnable = true;
                    }
                    else
                    {
                        restaurentBillBO.IsInvoiceServiceChargeEnable = false;
                    }

                    if (cbVatAmount.Checked)
                    {
                        restaurentBillBO.IsInvoiceVatAmountEnable = true;
                    }
                    else
                    {
                        restaurentBillBO.IsInvoiceVatAmountEnable = false;
                    }

                    if (isRestaurantBillInclusive == 0)
                    {
                        restaurentBillBO.SalesAmount = !string.IsNullOrWhiteSpace(txtSalesAmount.Text) ? Convert.ToDecimal(txtSalesAmount.Text) : 0;
                        restaurentBillBO.GrandTotal = Convert.ToDecimal(txtGrandTotalHiddenField.Value);
                    }
                    else
                    {
                        restaurentBillBO.SalesAmount = !string.IsNullOrWhiteSpace(txtSalesAmount.Text) ? Convert.ToDecimal(txtSalesAmount.Text) : 0;
                        restaurentBillBO.GrandTotal = !string.IsNullOrWhiteSpace(txtSalesAmount.Text) ? Convert.ToDecimal(txtSalesAmount.Text) - restaurentBillBO.CalculatedDiscountAmount : 0;
                    }

                    if (cbIsComplementary.Checked)
                    { restaurentBillBO.IsComplementary = true; }
                    else
                    { restaurentBillBO.IsComplementary = false; }

                    if (restaurentBillBO.DiscountType == "Percentage" && restaurentBillBO.DiscountAmount == 100)
                    {
                        restaurentBillBO.IsComplementary = true;
                    }

                    List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();

                    KotBillMasterBO entityBO = new KotBillMasterBO();

                    if (Request.QueryString["tokenId"] != null)
                    {
                        if (hfSelectedTableId.Value != "")
                            hfSelectedTableId.Value = Request.QueryString["tokenId"] + "," + hfSelectedTableId.Value;
                        else
                            hfSelectedTableId.Value = Request.QueryString["tokenId"];

                        string[] tableId = hfSelectedTableId.Value.Split(',');
                        string tablesId = string.Empty, kotIdList = string.Empty;

                        for (int i = 0; i < tableId.Count(); i++)
                        {
                            entityBO = entityDA.GetKotBillMasterInfoByTableId(Convert.ToInt32(hfCostCenterId.Value), "RestaurantToken", int.Parse(tableId[i]));

                            RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                            restaurentBillDetailBO.KotId = entityBO.KotId;
                            restaurentBillDetailBO.BillId = restaurentBillBO.BillId;
                            restaurentBillDetailBOList.Add(restaurentBillDetailBO);
                        }

                        restaurentBillBO.KotId = restaurentBillDetailBOList[0].KotId;
                    }
                    else if (Request.QueryString["tableId"] != null)
                    {
                        if (hfSelectedTableId.Value != "")
                            hfSelectedTableId.Value = Request.QueryString["tableId"] + "," + hfSelectedTableId.Value;
                        else
                            hfSelectedTableId.Value = Request.QueryString["tableId"];

                        string[] tableId = hfSelectedTableId.Value.Split(',');
                        string tablesId = string.Empty, kotIdList = string.Empty;

                        for (int i = 0; i < tableId.Count(); i++)
                        {
                            entityBO = entityDA.GetKotBillMasterInfoByTableId(restaurentBillBO.CostCenterId, "RestaurantTable", int.Parse(tableId[i]));
                            RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                            restaurentBillDetailBO.KotId = entityBO.KotId;
                            restaurentBillDetailBO.BillId = restaurentBillBO.BillId;
                            restaurentBillDetailBOList.Add(restaurentBillDetailBO);
                        }

                        restaurentBillBO.KotId = restaurentBillDetailBOList[0].KotId;
                    }
                    else if (Request.QueryString["RoomNumber"] != null)
                    {
                        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                        RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                        roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(Request.QueryString["RoomNumber"]);
                        if (roomAllocationBO.RoomId > 0)
                        {
                            entityBO = entityDA.GetKotBillMasterInfoByTableId(restaurentBillBO.CostCenterId, "GuestRoom", roomAllocationBO.RegistrationId);

                            RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                            restaurentBillDetailBO.KotId = entityBO.KotId;
                            restaurentBillDetailBO.BillId = restaurentBillBO.BillId;
                            restaurentBillDetailBOList.Add(restaurentBillDetailBO);
                        }
                    }

                    restaurentBillDetailBOList = restaurentBillDetailBOList.GroupBy(t => t.KotId).Select(grp => grp.First()).Where(x => x.KotId != 0).ToList();
                    restaurentBillBO.PaxQuantity = !string.IsNullOrWhiteSpace(txtPaxQuantity.Text) ? Convert.ToInt32(txtPaxQuantity.Text) : 1;

                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                    Boolean updateSuccess = restaurentBillDA.UpdateRestaurantBillSummaryAmount(restaurentBillBO, restaurentBillDetailBOList, categoryWisePercentageDiscountBOList, categoryIdList, false, true, userInformationBO.UserInfoId);

                    //---------------------------------------------------
                    // // Nazrul Bhai work here*******************************************************************


                    //string kotSave = hfNewlyAddedKotId.Value, kotDelete = hfDeletedKotId.Value;
                    //restaurentBillDA.SaveUpdateRestaurantBillDetail(txtBillIdForInvoicePreview.Value, kotSave, kotDelete);

                    LoadGridView();

                    hfNewlyAddedTableId.Value = string.Empty;
                    hfNewlyAddedKotId.Value = string.Empty;
                    hfDeletedTableId.Value = string.Empty;
                    hfDeletedKotId.Value = string.Empty;
                    //--------------------------------------------------------

                    string url = "/POS/Reports/frmReportBillInfo.aspx?billID=" + txtBillIdForInvoicePreview.Value;
                    string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes');";
                    ClientScript.RegisterStartupScript(GetType(), "script", s, true);
                }
            }
        }
        protected void btnFinalSave_Click(object sender, EventArgs e)
        {
            KotBillMasterDA entityDA = new KotBillMasterDA();

            string categoryIdList = string.Empty;
            List<string> strCategoryIdList = new List<string>();
            List<RestaurantBillBO> categoryWisePercentageDiscountBOList = new List<RestaurantBillBO>();
            RestaurantBillBO restaurentBillBO = new RestaurantBillBO();

            int SourceId = 0;
            SourceId = Int32.Parse(SourceIdHiddenField.Value);

            if (HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] != null)
            {
                strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                foreach (string rowCWPD in strCategoryIdList)
                {
                    RestaurantBillBO categoryWisePercentageDiscountBO = new RestaurantBillBO();
                    categoryWisePercentageDiscountBO.ClassificationId = Convert.ToInt32(rowCWPD);
                    categoryWisePercentageDiscountBOList.Add(categoryWisePercentageDiscountBO);

                    categoryIdList += categoryIdList != string.Empty ? ("," + rowCWPD) : rowCWPD;
                }
            }

            if (hfIsCustomBearerInfoEnable.Value == "1")
            {
                restaurentBillBO.BearerId = Convert.ToInt32(ddlWaiterNameDisplay.SelectedValue);
            }
            else
            {
                restaurentBillBO.BearerId = 0;
            }

            LoadHomePanelButtonInfo();

            if (Request.QueryString["tokenId"] != null)
            {
                BillProcessingForTokenSystem();
            }
            else
            {
                try
                {
                    int isRestaurantBillInclusive = !string.IsNullOrWhiteSpace(hfIsRestaurantBillInclusive.Value) ? Convert.ToInt32(hfIsRestaurantBillInclusive.Value) : 0;
                    if (string.IsNullOrWhiteSpace(txtBillIdForInvoicePreview.Value))
                    {
                        if (!IsFrmValid())
                        {
                            return;
                        }

                        if (!CheckingPaymentInformationForBill())
                        {
                            return;
                        }

                        int transactionId = 0;
                        string transactionHead = string.Empty;
                        HMCommonDA hmCommonDA = new HMCommonDA();
                        CustomFieldBO customField = new CustomFieldBO();

                        customField = hmCommonDA.GetCustomFieldByFieldName("GuestBillPayment");

                        if (customField == null)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Please Contact With Administrator for Accounts Mapping.", AlertType.Warning);
                            return;
                        }
                        else
                        {
                            transactionId = Convert.ToInt32(customField.FieldValue);
                            CommonNodeMatrixBO commonNodeMatrixBO = new CommonNodeMatrixBO();
                            commonNodeMatrixBO = hmCommonDA.GetCommonNodeMatrixInfoById(transactionId);
                            if (commonNodeMatrixBO == null)
                            {
                                CommonHelper.AlertInfo(innboardMessage, "Please Contact With Administrator for Accounts Mapping.", AlertType.Warning);
                                return;
                            }
                            else
                            {
                                UserInformationBO userInformationBO = new UserInformationBO();
                                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                                restaurentBillBO.BillDate = DateTime.Now;
                                restaurentBillBO.BillPaymentDate = DateTime.Now;
                                restaurentBillBO.CostCenterId = Convert.ToInt32(hfCostCenterId.Value);
                                restaurentBillBO.DiscountType = ddlDiscountType.SelectedValue.ToString();

                                if (restaurentBillBO.DiscountType == "Fixed")
                                {
                                    restaurentBillBO.DiscountTransactionId = 0;
                                }
                                else if (restaurentBillBO.DiscountType == "Percentage")
                                {
                                    restaurentBillBO.DiscountTransactionId = 0;
                                }
                                else if (restaurentBillBO.DiscountType == "Member")
                                {
                                    string[] ddlMemberIdSelectedValue = ddlMemberId.SelectedValue.Split(',');
                                    int memMemberId = Convert.ToInt32(ddlMemberIdSelectedValue[0]);
                                    restaurentBillBO.DiscountTransactionId = memMemberId;
                                }
                                else if (restaurentBillBO.DiscountType == "BusinessPromotion")
                                {
                                    restaurentBillBO.DiscountTransactionId = Convert.ToInt32(ddlBusinessPromotionId.SelectedValue.Split('~')[0]);
                                }

                                restaurentBillBO.DiscountAmount = Convert.ToDecimal(txtDiscountAmount.Text);
                                restaurentBillBO.CalculatedDiscountAmount = !string.IsNullOrWhiteSpace(txtCalculatedDiscountAmount.Value) ? Convert.ToDecimal(txtCalculatedDiscountAmount.Value) : 0;
                                restaurentBillBO.ServiceCharge = !string.IsNullOrWhiteSpace(txtServiceChargeAmountHiddenField.Value) ? Convert.ToDecimal(txtServiceChargeAmountHiddenField.Value) : 0; //Convert.ToDecimal(txtServiceCharge.Text);
                                restaurentBillBO.VatAmount = !string.IsNullOrWhiteSpace(txtVatAmountHiddenField.Value) ? Convert.ToDecimal(txtVatAmountHiddenField.Value) : 0; //Convert.ToDecimal(txtVatAmount.Text);
                                restaurentBillBO.CustomerName = txtCustomerName.Text;
                                restaurentBillBO.PayMode = ddlPayMode.SelectedItem.Text;
                                restaurentBillBO.BankId = Convert.ToInt32(ddlBankName.SelectedValue);
                                restaurentBillBO.CardType = ddlCardType.SelectedValue.ToString();
                                restaurentBillBO.CardNumber = txtCardNumber.Text;
                                restaurentBillBO.PaxQuantity = !string.IsNullOrWhiteSpace(txtPaxQuantity.Text) ? Convert.ToInt32(txtPaxQuantity.Text) : 1;

                                if (Request.QueryString["tokenId"] != null)
                                {
                                    restaurentBillBO.SourceName = "RestaurantToken";
                                    restaurentBillBO.BillPaidBySourceId = Int32.Parse(Request.QueryString["tokenId"]);
                                }
                                else if (Request.QueryString["tableId"] != null)
                                {
                                    restaurentBillBO.SourceName = "RestaurantTable";
                                    restaurentBillBO.BillPaidBySourceId = Int32.Parse(Request.QueryString["tableId"]);
                                }
                                else if (Request.QueryString["RoomNumber"] != null)
                                {
                                    restaurentBillBO.SourceName = "GuestRoom";
                                    restaurentBillBO.BillPaidBySourceId = Int32.Parse(Request.QueryString["RoomNumber"]);
                                }

                                restaurentBillBO.CreatedBy = userInformationBO.UserInfoId;

                                if (hfPaidServiceDiscount.Value == "Due Amount   :  0")
                                {
                                    hfPaidServiceDiscount.Value = string.Empty;
                                }
                                decimal paidServiceDiscount = !string.IsNullOrWhiteSpace(hfPaidServiceDiscount.Value) ? Convert.ToDecimal(hfPaidServiceDiscount.Value) : 0; //Convert.ToDecimal(txtVatAmount.Text);

                                if (restaurentBillBO.DiscountType == "Fixed")
                                {
                                    restaurentBillBO.DiscountAmount = restaurentBillBO.DiscountAmount + paidServiceDiscount;
                                }
                                else
                                {
                                    restaurentBillBO.DiscountAmount = restaurentBillBO.DiscountAmount + (paidServiceDiscount / 100);
                                }

                                restaurentBillBO.TableId = Int32.Parse(SourceIdHiddenField.Value);
                                if (string.IsNullOrEmpty(txtExpireDate.Text))
                                {
                                    restaurentBillBO.ExpireDate = null;
                                }
                                else
                                {
                                    restaurentBillBO.ExpireDate = hmUtility.GetDateTimeFromString(txtExpireDate.Text, userInformationBO.ServerDateFormat);
                                }
                                restaurentBillBO.CardHolderName = txtCardHolderName.Text;
                                if (ddlPayMode.SelectedValue == "2")
                                {
                                    //   restaurentBillBO.RegistrationId = Convert.ToInt32(ddlRegistrationId.SelectedValue);
                                }
                                else if (ddlPayMode.SelectedValue == "3")
                                {
                                    restaurentBillBO.RegistrationId = Convert.ToInt32(SourceIdHiddenField.Value);
                                }

                                if (isRestaurantBillInclusive == 0)
                                {
                                    restaurentBillBO.SalesAmount = !string.IsNullOrWhiteSpace(txtSalesAmount.Text) ? Convert.ToDecimal(txtSalesAmount.Text) : 0;
                                    restaurentBillBO.GrandTotal = Convert.ToDecimal(txtGrandTotalHiddenField.Value);
                                }
                                else
                                {
                                    restaurentBillBO.SalesAmount = Convert.ToDecimal(txtGrandTotalHiddenField.Value);
                                    restaurentBillBO.GrandTotal = !string.IsNullOrWhiteSpace(txtSalesAmount.Text) ? Convert.ToDecimal(txtSalesAmount.Text) - restaurentBillBO.CalculatedDiscountAmount : 0;
                                }

                                if (cbServiceCharge.Checked)
                                {
                                    restaurentBillBO.IsInvoiceServiceChargeEnable = true;
                                }
                                else
                                {
                                    restaurentBillBO.IsInvoiceServiceChargeEnable = false;
                                }

                                if (cbVatAmount.Checked)
                                {
                                    restaurentBillBO.IsInvoiceVatAmountEnable = true;
                                }
                                else
                                {
                                    restaurentBillBO.IsInvoiceVatAmountEnable = false;
                                }

                                if (cbIsComplementary.Checked)
                                { restaurentBillBO.IsComplementary = true; }
                                else
                                { restaurentBillBO.IsComplementary = false; }

                                if (restaurentBillBO.DiscountType == "Percentage" && restaurentBillBO.DiscountAmount == 100)
                                {
                                    restaurentBillBO.IsComplementary = true;
                                }

                                List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();

                                if (Request.QueryString["tokenId"] != null)
                                {
                                    if (hfSelectedTableId.Value != "")
                                        hfSelectedTableId.Value = Request.QueryString["tokenId"] + "," + hfSelectedTableId.Value;
                                    else
                                        hfSelectedTableId.Value = Request.QueryString["tokenId"];
                                }
                                else if (Request.QueryString["tableId"] != null)
                                {
                                    if (hfSelectedTableId.Value != "")
                                        hfSelectedTableId.Value = Request.QueryString["tableId"] + "," + hfSelectedTableId.Value;
                                    else
                                        hfSelectedTableId.Value = Request.QueryString["tableId"];
                                }
                                else if (Request.QueryString["RoomNumber"] != null)
                                {
                                    if (hfSelectedTableId.Value != "")
                                        hfSelectedTableId.Value = Request.QueryString["RoomNumber"] + "," + hfSelectedTableId.Value;
                                    else
                                        hfSelectedTableId.Value = Request.QueryString["RoomNumber"];
                                }

                                string[] tableId = hfSelectedTableId.Value.Split(',');
                                string tablesId = string.Empty, kotIdList = string.Empty;

                                KotBillMasterBO entityBO = new KotBillMasterBO();

                                for (int i = 0; i < tableId.Count(); i++)
                                {
                                    entityBO = entityDA.GetKotBillMasterInfoByTableId(Convert.ToInt32(hfCostCenterId.Value), "RestaurantTable", int.Parse(tableId[i]));

                                    RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                                    restaurentBillDetailBO.KotId = entityBO.KotId;

                                    restaurentBillDetailBOList.Add(restaurentBillDetailBO);
                                }

                                restaurentBillBO.KotId = restaurentBillDetailBOList[0].KotId;

                                bool isBillSettlement = entityDA.GetIsKotIsSettledOrNotByBillId(restaurentBillDetailBOList[0].BillId, restaurentBillBO.BillPaidBySourceId);

                                if (isBillSettlement)
                                {
                                    Response.Redirect("Restaurant/frmKotBillMaster.aspx?Kot=TableAllocation");
                                }

                                if (SourceId > 0)
                                {
                                    List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

                                    RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
                                    int billID = 0;

                                    int success = restaurentBillDA.SaveRestaurantBill(restaurentBillBO, restaurentBillDetailBOList, guestPaymentDetailListForGrid, categoryWisePercentageDiscountBOList, categoryIdList, true, true, out billID);
                                    txtBillId.Value = success.ToString();
                                    if (success > 0)
                                    {
                                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RestaurantBill.ToString(), billID,
                                        ProjectModuleEnum.ProjectModule.RestaurantManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantBill));
                                        foreach (GuestBillPaymentBO row in guestPaymentDetailListForGrid)
                                        {
                                            if (row.PaidServiceId > 0)
                                            {
                                                if (row.PaymentMode == "Other Room")
                                                {
                                                    ApprovedGuestPaidService(billID, row.RegistrationId, row.PaidServiceId);
                                                }
                                            }
                                        }

                                        if (billID > 0)
                                        {
                                            Boolean updateRegistrationSuccess = restaurentBillDA.UpdateRestaurantBillRegistrationIdInfoForOtherRoomPayment(billID);
                                        }

                                        int currentCostCenterId = !string.IsNullOrWhiteSpace(hfCostCenterId.Value) ? Convert.ToInt32(hfCostCenterId.Value) : 0;
                                        string currentTableIdList = hfSelectedTableId.Value;

                                        if (Request.QueryString["tokenId"] != null)
                                        {
                                        }
                                        else if (Request.QueryString["tableId"] != null)
                                        {
                                            Boolean rbSuccess = restaurentBillDA.UpdateRestaurantCostCenterTableMappingInfo(currentCostCenterId, currentTableIdList);

                                            if (rbSuccess)
                                                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                                            else
                                                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                                        }

                                        Cancel();
                                        string url = "/POS/Reports/frmReportBillInfo.aspx?billID=" + txtBillId.Value;
                                        string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes');";
                                        ClientScript.RegisterStartupScript(GetType(), "script", s, true);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!CheckingPaymentInformationForBill())
                        {
                            return;
                        }

                        if (Request.QueryString["tokenId"] != null)
                        {
                            if (hfSelectedTableId.Value != "")
                                hfSelectedTableId.Value = Request.QueryString["tokenId"] + "," + hfSelectedTableId.Value;
                            else
                                hfSelectedTableId.Value = Request.QueryString["tokenId"];
                        }
                        else if (Request.QueryString["tableId"] != null)
                        {
                            if (hfSelectedTableId.Value != "")
                                hfSelectedTableId.Value = Request.QueryString["tableId"] + "," + hfSelectedTableId.Value;
                            else
                                hfSelectedTableId.Value = Request.QueryString["tableId"];
                        }
                        else if (Request.QueryString["RoomNumber"] != null)
                        {
                            if (hfSelectedTableId.Value != "")
                                hfSelectedTableId.Value = Request.QueryString["RoomNumber"] + "," + hfSelectedTableId.Value;
                            else
                                hfSelectedTableId.Value = Request.QueryString["RoomNumber"];
                        }

                        RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
                        restaurentBillBO.BillId = Convert.ToInt32(txtBillIdForInvoicePreview.Value);
                        restaurentBillBO.BillDate = DateTime.Now;
                        restaurentBillBO.BillPaymentDate = DateTime.Now;
                        restaurentBillBO.CustomerName = txtCustomerName.Text;
                        restaurentBillBO.CalculatedDiscountAmount = !string.IsNullOrWhiteSpace(txtCalculatedDiscountAmount.Value) ? Convert.ToDecimal(txtCalculatedDiscountAmount.Value) : 0;
                        restaurentBillBO.DiscountType = ddlDiscountType.SelectedValue.ToString();
                        restaurentBillBO.CostCenterId = Convert.ToInt32(hfCostCenterId.Value);

                        if (restaurentBillBO.DiscountType == "Fixed")
                        {
                            restaurentBillBO.DiscountTransactionId = 0;
                        }
                        else if (restaurentBillBO.DiscountType == "Percentage")
                        {
                            restaurentBillBO.DiscountTransactionId = 0;
                        }
                        else if (restaurentBillBO.DiscountType == "Member")
                        {
                            string[] ddlMemberIdSelectedValue = ddlMemberId.SelectedValue.Split(',');
                            int memMemberId = Convert.ToInt32(ddlMemberIdSelectedValue[0]);
                            restaurentBillBO.DiscountTransactionId = memMemberId;
                        }
                        else if (restaurentBillBO.DiscountType == "BusinessPromotion")
                        {
                            restaurentBillBO.DiscountTransactionId = Convert.ToInt32(ddlBusinessPromotionId.SelectedValue.Split('~')[0]);
                        }

                        restaurentBillBO.DiscountAmount = Convert.ToDecimal(txtDiscountAmount.Text);
                        restaurentBillBO.ServiceCharge = !string.IsNullOrWhiteSpace(txtServiceChargeAmountHiddenField.Value) ? Convert.ToDecimal(txtServiceChargeAmountHiddenField.Value) : 0; //Convert.ToDecimal(txtServiceCharge.Text);
                        restaurentBillBO.VatAmount = !string.IsNullOrWhiteSpace(txtVatAmountHiddenField.Value) ? Convert.ToDecimal(txtVatAmountHiddenField.Value) : 0; //Convert.ToDecimal(txtVatAmount.Text);
                        restaurentBillBO.PaxQuantity = !string.IsNullOrWhiteSpace(txtPaxQuantity.Text) ? Convert.ToInt32(txtPaxQuantity.Text) : 1;

                        if (Request.QueryString["tokenId"] != null)
                        {
                            restaurentBillBO.SourceName = "RestaurantToken";
                            restaurentBillBO.BillPaidBySourceId = Int32.Parse(Request.QueryString["tokenId"]);
                        }
                        if (Request.QueryString["tableId"] != null)
                        {
                            restaurentBillBO.SourceName = "RestaurantTable";
                            restaurentBillBO.BillPaidBySourceId = Int32.Parse(Request.QueryString["tableId"]);

                            KotBillMasterBO billMasterBO = new KotBillMasterBO();
                            billMasterBO = entityDA.GetKotBillMasterInfoByTableId(Convert.ToInt32(hfCostCenterId.Value), "RestaurantTable", restaurentBillBO.BillPaidBySourceId);

                            restaurentBillBO.KotId = billMasterBO.KotId;
                        }
                        if (Request.QueryString["RoomNumber"] != null)
                        {
                            restaurentBillBO.SourceName = "GuestRoom";
                            restaurentBillBO.BillPaidBySourceId = Int32.Parse(Request.QueryString["RoomNumber"]);
                        }

                        if (cbIsComplementary.Checked)
                        { restaurentBillBO.IsComplementary = true; }
                        else
                        { restaurentBillBO.IsComplementary = false; }

                        if (restaurentBillBO.DiscountType == "Percentage" && restaurentBillBO.DiscountAmount == 100)
                        {
                            restaurentBillBO.IsComplementary = true;
                        }

                        if (isRestaurantBillInclusive == 0)
                        {
                            restaurentBillBO.SalesAmount = !string.IsNullOrWhiteSpace(txtSalesAmount.Text) ? Convert.ToDecimal(txtSalesAmount.Text) : 0;
                            restaurentBillBO.GrandTotal = Convert.ToDecimal(txtGrandTotalHiddenField.Value);
                        }
                        else
                        {
                            restaurentBillBO.SalesAmount = Convert.ToDecimal(txtGrandTotalHiddenField.Value);
                            restaurentBillBO.GrandTotal = !string.IsNullOrWhiteSpace(txtSalesAmount.Text) ? Convert.ToDecimal(txtSalesAmount.Text) - restaurentBillBO.CalculatedDiscountAmount : 0;
                        }

                        if (cbServiceCharge.Checked)
                        {
                            restaurentBillBO.IsInvoiceServiceChargeEnable = true;
                        }
                        else
                        {
                            restaurentBillBO.IsInvoiceServiceChargeEnable = false;
                        }

                        if (cbVatAmount.Checked)
                        {
                            restaurentBillBO.IsInvoiceVatAmountEnable = true;
                        }
                        else
                        {
                            restaurentBillBO.IsInvoiceVatAmountEnable = false;
                        }

                        //------------------- More Table Added

                        List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();
                        KotBillMasterBO entityBO = new KotBillMasterBO();

                        if (Request.QueryString["tokenId"] != null)
                        {
                            if (hfSelectedTableId.Value != "")
                                hfSelectedTableId.Value = Request.QueryString["tokenId"] + "," + hfSelectedTableId.Value;
                            else
                                hfSelectedTableId.Value = Request.QueryString["tokenId"];

                            string[] tableId = hfSelectedTableId.Value.Split(',');
                            string tablesId = string.Empty, kotIdList = string.Empty;

                            for (int i = 0; i < tableId.Count(); i++)
                            {
                                entityBO = entityDA.GetKotBillMasterInfoByTableId(Convert.ToInt32(hfCostCenterId.Value), "RestaurantToken", int.Parse(tableId[i]));

                                RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                                restaurentBillDetailBO.KotId = entityBO.KotId;
                                restaurentBillDetailBO.BillId = restaurentBillBO.BillId;

                                restaurentBillDetailBOList.Add(restaurentBillDetailBO);
                            }

                            restaurentBillBO.KotId = restaurentBillDetailBOList[0].KotId;
                        }
                        else if (Request.QueryString["tableId"] != null)
                        {
                            if (hfSelectedTableId.Value != "")
                                hfSelectedTableId.Value = Request.QueryString["tableId"] + "," + hfSelectedTableId.Value;
                            else
                                hfSelectedTableId.Value = Request.QueryString["tableId"];

                            string[] tableId = hfSelectedTableId.Value.Split(',');
                            string tablesId = string.Empty, kotIdList = string.Empty;

                            for (int i = 0; i < tableId.Count(); i++)
                            {
                                entityBO = entityDA.GetKotBillMasterInfoByTableId(restaurentBillBO.CostCenterId, "RestaurantTable", int.Parse(tableId[i]));

                                RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();

                                restaurentBillDetailBO.KotId = entityBO.KotId;
                                restaurentBillDetailBO.BillId = restaurentBillBO.BillId;

                                restaurentBillDetailBOList.Add(restaurentBillDetailBO);
                            }

                            restaurentBillBO.KotId = restaurentBillDetailBOList[0].KotId;
                        }
                        else if (Request.QueryString["RoomNumber"] != null)
                        {
                            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                            RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                            roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(Request.QueryString["RoomNumber"]);
                            if (roomAllocationBO.RoomId > 0)
                            {
                                entityBO = entityDA.GetKotBillMasterInfoByTableId(restaurentBillBO.CostCenterId, "GuestRoom", roomAllocationBO.RegistrationId);

                                RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                                restaurentBillDetailBO.KotId = entityBO.KotId;
                                restaurentBillDetailBO.BillId = restaurentBillBO.BillId;

                                restaurentBillDetailBOList.Add(restaurentBillDetailBO);
                            }
                        }

                        bool isBillSettlement = entityDA.GetIsKotIsSettledOrNotByBillId(restaurentBillDetailBOList[0].BillId, restaurentBillBO.BillPaidBySourceId);

                        if (isBillSettlement)
                        {
                            Response.Redirect("frmKotBillMaster.aspx?Kot=TableAllocation");
                        }

                        restaurentBillDetailBOList = restaurentBillDetailBOList.GroupBy(t => t.KotId).Select(grp => grp.First()).Where(x => x.KotId != 0).ToList();

                        restaurentBillBO.PaxQuantity = !string.IsNullOrWhiteSpace(txtPaxQuantity.Text) ? Convert.ToInt32(txtPaxQuantity.Text) : 1;

                        //-----------------------
                        UserInformationBO userInformationBO = new UserInformationBO();
                        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                        Boolean updateSuccess = restaurentBillDA.UpdateRestaurantBillSummaryAmount(restaurentBillBO, restaurentBillDetailBOList, categoryWisePercentageDiscountBOList, categoryIdList, false, true, userInformationBO.UserInfoId);

                        int billId = !string.IsNullOrWhiteSpace(txtBillIdForInvoicePreview.Value) ? Convert.ToInt32(txtBillIdForInvoicePreview.Value) : 0;
                        List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

                        foreach (GuestBillPaymentBO row in guestPaymentDetailListForGrid)
                        {
                            if (row.PaidServiceId > 0)
                            {
                                if (row.PaymentMode == "Other Room")
                                {
                                    ApprovedGuestPaidService(restaurentBillBO.BillId, row.RegistrationId, row.PaidServiceId);
                                }
                            }
                        }

                        if (guestPaymentDetailListForGrid.Count <= 0)
                        {
                            if (restaurentBillBO.IsComplementary)
                            {
                                if (Convert.ToInt32(ddlComplementaryRoomId.SelectedValue) > 0)
                                {
                                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                                    List<RoomRegistrationBO> roomRegistrationBO = new List<RoomRegistrationBO>();
                                    roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(Convert.ToInt32(ddlComplementaryRoomId.SelectedValue));
                                    if (roomRegistrationBO != null)
                                    {
                                        int registrationId = roomRegistrationBO[0].RegistrationId;
                                        guestPaymentDetailListForGrid.Add(new GuestBillPaymentBO
                                        {
                                            NodeId = Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                                            PaymentType = "Other Room",
                                            AccountsPostingHeadId = Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                                            BillPaidBy = 0,
                                            BankId = registrationId,
                                            RegistrationId = registrationId,
                                            FieldId = !string.IsNullOrWhiteSpace(hfLocalCurrencyId.Value) ? Convert.ToInt32(hfLocalCurrencyId.Value) : 0,
                                            ConvertionRate = 1,
                                            CurrencyAmount = 0,
                                            PaymentAmount = 0,
                                            ChecqueDate = DateTime.Now,
                                            PaymentMode = "Other Room",
                                            PaymentId = 1,
                                            CardNumber = "",
                                            CardType = "",
                                            ExpireDate = null,
                                            ChecqueNumber = "",
                                            CardHolderName = "",
                                            PaymentDescription = ""
                                        });
                                    }
                                }
                                else
                                {
                                    guestPaymentDetailListForGrid.Add(new GuestBillPaymentBO
                                    {
                                        NodeId = Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                                        PaymentType = "Advance",
                                        AccountsPostingHeadId = Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                                        BillPaidBy = 0,
                                        BankId = 0,
                                        RegistrationId = 0,
                                        FieldId = !string.IsNullOrWhiteSpace(hfLocalCurrencyId.Value) ? Convert.ToInt32(hfLocalCurrencyId.Value) : 0,
                                        ConvertionRate = 1,
                                        CurrencyAmount = 0,
                                        PaymentAmount = 0,
                                        ChecqueDate = DateTime.Now,
                                        PaymentMode = (restaurentBillBO.IsComplementary == true ? "Discount" : "Cash"),
                                        PaymentId = 1,
                                        CardNumber = "",
                                        CardType = "",
                                        ExpireDate = null,
                                        ChecqueNumber = "",
                                        CardHolderName = "",
                                        PaymentDescription = ""
                                    });
                                }
                            }
                            else
                            {
                                guestPaymentDetailListForGrid.Add(new GuestBillPaymentBO
                                {
                                    NodeId = Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                                    PaymentType = "Advance",
                                    AccountsPostingHeadId = Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                                    BillPaidBy = 0,
                                    BankId = 0,
                                    RegistrationId = 0,
                                    FieldId = !string.IsNullOrWhiteSpace(hfLocalCurrencyId.Value) ? Convert.ToInt32(hfLocalCurrencyId.Value) : 0,
                                    ConvertionRate = 1,
                                    CurrencyAmount = 0,
                                    PaymentAmount = 0,
                                    ChecqueDate = DateTime.Now,
                                    PaymentMode = (restaurentBillBO.IsComplementary == true ? "Discount" : "Cash"),
                                    PaymentId = 1,
                                    CardNumber = "",
                                    CardType = "",
                                    ExpireDate = null,
                                    ChecqueNumber = "",
                                    CardHolderName = "",
                                    PaymentDescription = ""
                                });
                            }
                        }
                        decimal changeAmount = 0;
                        if (!string.IsNullOrEmpty(hfChangeAmount.Value))
                        {
                            changeAmount = Convert.ToDecimal(hfChangeAmount.Value);
                        }
                        if (changeAmount > 0)
                        {
                            guestPaymentDetailListForGrid.Add(new GuestBillPaymentBO
                            {
                                RefundAccountHead = Convert.ToInt32(ddlCashReceiveAccountsInfo.SelectedValue),
                                PaymentType = "Refund",
                                AccountsPostingHeadId = Convert.ToInt32(ddlRefundAccountHead.SelectedValue),
                                BillPaidBy = 0,
                                BankId = 0,
                                RegistrationId = 0,
                                FieldId = !string.IsNullOrWhiteSpace(hfLocalCurrencyId.Value) ? Convert.ToInt32(hfLocalCurrencyId.Value) : 0,
                                ConvertionRate = 1,
                                CurrencyAmount = Convert.ToDecimal(changeAmount),
                                PaymentAmount = Convert.ToDecimal(changeAmount),
                                ChecqueDate = null,
                                PaymentMode = "Refund",
                                PaymentId = 0,
                                CardNumber = "",
                                CardType = "",
                                ExpireDate = null,
                                ChecqueNumber = "",
                                CardHolderName = "",
                                PaymentDescription = ""
                            });
                        }

                        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                        int createdBy = userInformationBO.UserInfoId;
                        int success = restaurentBillDA.UpdateRestaurantBillPayment(billId, guestPaymentDetailListForGrid, createdBy);

                        txtBillId.Value = success.ToString();
                        if (success > 0)
                        {
                            int currentCostCenterId = !string.IsNullOrWhiteSpace(hfCostCenterId.Value) ? Convert.ToInt32(hfCostCenterId.Value) : 0;
                            string currentTableIdList = hfSelectedTableId.Value;

                            if (Request.QueryString["tokenId"] != null)
                            {
                                restaurentBillDA.RestaurantBillSettlementInfoByBillId(billId);
                                Response.Redirect(homePanelPathInfo);
                            }
                            else if (Request.QueryString["tableId"] != null)
                            {
                                Boolean rbSuccess = restaurentBillDA.UpdateRestaurantCostCenterTableMappingInfo(currentCostCenterId, currentTableIdList);

                                if (rbSuccess)
                                {
                                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                                    Cancel();
                                }
                                else
                                {
                                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                                }

                                restaurentBillDA.RestaurantBillSettlementInfoByBillId(billId);

                                if (restaurentBillBO.BillId > 0)
                                {
                                    Boolean updateRegistrationSuccess = restaurentBillDA.UpdateRestaurantBillRegistrationIdInfoForOtherRoomPayment(restaurentBillBO.BillId);
                                }
                                Response.Redirect(homePanelPathInfo);
                            }
                            else if (Request.QueryString["RoomNumber"] != null)
                            {
                                restaurentBillDA.RestaurantBillSettlementInfoByBillId(billId);
                                if (restaurentBillBO.BillId > 0)
                                {
                                    Boolean updateRegistrationSuccess = restaurentBillDA.UpdateRestaurantBillRegistrationIdInfoForOtherRoomPayment(restaurentBillBO.BillId);
                                }

                                Response.Redirect(homePanelPathInfo);
                            }
                        }

                        if (restaurentBillBO.BillId > 0)
                        {
                            Boolean updateRegistrationSuccess = restaurentBillDA.UpdateRestaurantBillRegistrationIdInfoForOtherRoomPayment(restaurentBillBO.BillId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    
                }
            }
            hfChangeAmount.Value = "0";
        }
        protected void btnUpdateBillPayment_Click(object sender, EventArgs e)
        {
            string tablesId = string.Empty, kotIdList = string.Empty, newlyAddedTableKotId = string.Empty;
            string tableNumber = hfSelectedTableNumber.Value;
            int PaxQuantity = 0;

            if (hfSelectedTableId.Value != "")
                tablesId = SourceIdHiddenField.Value + "," + hfSelectedTableId.Value;
            else
                tablesId = SourceIdHiddenField.Value;

            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            PaxQuantity = !string.IsNullOrWhiteSpace(hfPaxQuantityHiddenField.Value) ? Convert.ToInt32(hfPaxQuantityHiddenField.Value) : 0;

            KotBillMasterBO entityBO = new KotBillMasterBO();
            KotBillMasterDA entityDA = new KotBillMasterDA();
            KotBillDetailDA da = new KotBillDetailDA();

            string[] tableId = tablesId.Split(',');
            string newlyAddedTableIds = hfNewlyAddedTableId.Value.ToString();

            for (int i = 0; i < tableId.Count(); i++)
            {
                entityBO = entityDA.GetKotBillMasterInfoByTableId(currentUserInformationBO.WorkingCostCenterId, "RestaurantTable", int.Parse(tableId[i]));

                PaxQuantity += entityBO.PaxQuantity;

                if (!string.IsNullOrEmpty(kotIdList))
                {
                    kotIdList += "," + entityBO.KotId.ToString();
                }
                else
                {
                    kotIdList = entityBO.KotId.ToString();
                }

                string[] newlyAddedTableId = newlyAddedTableIds.Split(',');
                bool notAdded = false;
                for (int j = 0; j < newlyAddedTableId.Count(); j++)
                {
                    if (newlyAddedTableId[j] == tableId[i])
                    {
                        notAdded = true;
                    }
                }

                if (notAdded)
                {
                    if (!string.IsNullOrEmpty(newlyAddedTableKotId))
                    {
                        newlyAddedTableKotId += "," + entityBO.KotId.ToString();
                    }
                    else
                    {
                        newlyAddedTableKotId = entityBO.KotId.ToString();
                    }
                }
            }

            hfNewlyAddedKotId.Value = newlyAddedTableKotId;

            string discountType = ddlDiscountType.SelectedValue;
            decimal discountPercentage = !string.IsNullOrEmpty(txtDiscountAmount.Text.Trim()) ? Convert.ToDecimal(txtDiscountAmount.Text.Trim()) : 0M;

            string categoryIdList = string.Empty;
            List<string> strCategoryIdList = new List<string>();

            if (Session["CategoryWisePercentageDiscountInfo"] != null)
            {
                strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                foreach (string rowCWPD in strCategoryIdList)
                {
                    categoryIdList += categoryIdList != string.Empty ? ("," + rowCWPD) : rowCWPD;
                }
            }

            List<KotBillDetailBO> kotDetails = new List<KotBillDetailBO>();
            kotDetails = da.GetRestaurantBillDetailInfoDynamicallyForInvoice(currentUserInformationBO.WorkingCostCenterId.ToString(), kotIdList, discountType, discountPercentage, categoryIdList, kotIdList, cbIsComplementary.Checked);

            if (kotDetails.Count > 0)
            {
                txtPaxQuantity.Text = PaxQuantity.ToString();

                foreach (KotBillDetailBO row in kotDetails)
                {
                    row.ServiceRate = Math.Round(row.ServiceRate, 2);
                    row.Amount = Math.Round(row.Amount);
                    row.ItemLineTotal = Math.Round((row.ServiceRate + row.ServiceCharge + row.VatAmount), 2);
                }

                gvRestaurentBill.DataSource = kotDetails;
                gvRestaurentBill.DataBind();
                LoadSplitBillListBox(kotDetails);
                CalculateAmountTotal();

                if (Request.QueryString["tableId"].ToString() != tablesId)
                {
                    AddedNewTables.InnerText = tableNumber;
                    hfSelectedTableNumber.Value = tableNumber;
                    AlreadyAddedNewTable.Style.Remove("display");
                }
                else
                {
                    AddedNewTables.InnerText = "";
                    hfSelectedTableNumber.Value = "";
                    AlreadyAddedNewTable.Style.Add("display", "none");
                }

                SourceIdHiddenField.Value = Request.QueryString["tableId"];
                hfCostCenterId.Value = currentUserInformationBO.WorkingCostCenterId.ToString();
            }
        }
        //************************ User Defined Function ********************//
        private void InvoicePreview(string strBillId)
        {
            if (!string.IsNullOrWhiteSpace(strBillId))
            {
                string sRrl = "/POS/Reports/frmReportBillInfo.aspx?billID=" + strBillId;
                string sLink = "window.open('" + sRrl + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes');";
                ClientScript.RegisterStartupScript(GetType(), "script", sLink, true);
            }
        }
        private void LoadRestaurantBearerInfo()
        {
            hfIsCustomBearerInfoEnable.Value = "0";
            RestaurantBearerDA entityDA = new RestaurantBearerDA();
            if (!string.IsNullOrWhiteSpace(hfCostCenterId.Value))
            {
                int costCenterId = Convert.ToInt32(hfCostCenterId.Value);
                this.ddlWaiterNameDisplay.DataSource = entityDA.GetActiveRestaurantBearerInfo(costCenterId, 1);
                this.ddlWaiterNameDisplay.DataTextField = "EmployeeName";
                this.ddlWaiterNameDisplay.DataValueField = "EmpId";
                this.ddlWaiterNameDisplay.DataBind();

                ListItem item = new ListItem();
                item.Value = "0";
                item.Text = hmUtility.GetDropDownFirstValue();
                this.ddlWaiterNameDisplay.Items.Insert(0, item);
            }
            else
            {
                cbTxtWaiterNameDisplay.Visible = false;
            }
        }
        private void IsRestaurantBillAmountWillRound()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO isRestaurantBillAmountWillRoundBO = new HMCommonSetupBO();
            isRestaurantBillAmountWillRoundBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillAmountWillRound", "IsRestaurantBillAmountWillRound");

            hfIsRestaurantBillAmountWillRound.Value = "1";
            if (isRestaurantBillAmountWillRoundBO != null)
            {
                hfIsRestaurantBillAmountWillRound.Value = isRestaurantBillAmountWillRoundBO.SetupValue;
            }
        }
        private void LoadHomePanelButtonInfo()
        {
            pnlHomeButtonInfo.Visible = false;
            if (Request.QueryString["tokenId"] != null)
            {
                int kotNumberInfo = !string.IsNullOrWhiteSpace(hftxtKotNumber.Value) ? Convert.ToInt32(hftxtKotNumber.Value) : 0;
                if (kotNumberInfo == 0)
                {
                    pnlHomeButtonInfo.Visible = true;
                    hfHomePanelInfo.Value = homePanelPathInfo;
                }
            }
        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("All");

            ddlCurrency.DataSource = currencyListBO;
            ddlCurrency.DataTextField = "CurrencyName";
            ddlCurrency.DataValueField = "CurrencyId";
            ddlCurrency.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCurrency.Items.Insert(0, item);
        }
        private void LoadIsConversionRateEditable()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsConversionRateEditable", "IsConversionRateEditable");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        txtConversionRate.ReadOnly = true;
                    }
                    else
                    {
                        txtConversionRate.ReadOnly = false;
                    }
                }
            }
        }
        private void IsLocalCurrencyDefaultSelected()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsLocalCurrencyDefaultSelected", "IsLocalCurrencyDefaultSelected");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        CommonCurrencyDA headDA = new CommonCurrencyDA();
                        List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
                        currencyListBO = headDA.GetConversionHeadInfoByType("All");

                        ddlCurrency.DataSource = currencyListBO;
                        ddlCurrency.DataTextField = "CurrencyName";
                        ddlCurrency.DataValueField = "CurrencyId";
                        ddlCurrency.DataBind();

                        CommonCurrencyBO currencyBO = currencyListBO.Where(x => x.CurrencyType == "Local").SingleOrDefault();
                        ddlCurrency.SelectedValue = currencyBO.CurrencyId.ToString();
                    }
                }
            }
        }
        private void LoadLocalCurrencyId()
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetLocalCurrencyInfo();
            hfLocalCurrencyId.Value = commonCurrencyBO.CurrencyId.ToString();
        }
        private void IsRestaurantPaymentModeCashDefault()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantPaymentModeCashDefault", "IsRestaurantPaymentModeCashDefault");
            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("0"));
                    }
                }
            }
        }
        private void LoadBusinessPromotion()
        {
            BusinessPromotionDA bpDA = new BusinessPromotionDA();
            ddlBusinessPromotionId.DataSource = bpDA.GetCurrentActiveBusinessPromotionInfo();
            ddlBusinessPromotionId.DataTextField = "BPHead";
            ddlBusinessPromotionId.DataValueField = "BusinessPromotionIdNPercentAmount";
            ddlBusinessPromotionId.DataBind();

            ListItem itemReservation = new ListItem();
            itemReservation.Value = "0";
            itemReservation.Text = hmUtility.GetDropDownFirstValue();
            ddlBusinessPromotionId.Items.Insert(0, itemReservation);
        }
        private void LoadRackRateServiceChargeVatPanelInformation()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isInnboardVatEnableBO = new HMCommonSetupBO();
            isInnboardVatEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsInnboardVatEnable", "IsInnboardVatEnable");

            HMCommonSetupBO isInnboardServiceChargeEnableBO = new HMCommonSetupBO();
            isInnboardServiceChargeEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsInnboardServiceChargeEnable", "IsInnboardServiceChargeEnable");

            if (isInnboardVatEnableBO.SetupValue == "1" || isInnboardServiceChargeEnableBO.SetupValue == "1")
            {
                cbServiceCharge.Checked = true;
                cbVatAmount.Checked = true;
                pnlRackRateServiceChargeVatInformation.Visible = true;
            }
            else
            {
                pnlRackRateServiceChargeVatInformation.Visible = false;
                cbServiceCharge.Checked = false;
                cbVatAmount.Checked = false;
            }
        }
        private void LoadCommonSetupForRackRateServiceChargeVatInformation()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO commonSetupBO;

            commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsInclusiveBill", "IsInclusiveBill");
            hfIsRestaurantBillInclusive.Value = commonSetupBO.SetupValue;

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsInnboardVatEnable", "IsInnboardVatEnable");
            hfIsVatServiceChargeEnable.Value = commonSetupBO.SetupValue;
            int isVatServiceChargeEnable = !string.IsNullOrWhiteSpace(hfIsVatServiceChargeEnable.Value) ? Convert.ToInt32(hfIsVatServiceChargeEnable.Value) : 0;

            int isRestaurantBillInclusive = !string.IsNullOrWhiteSpace(hfIsRestaurantBillInclusive.Value) ? Convert.ToInt32(hfIsRestaurantBillInclusive.Value) : 0;
            if (isRestaurantBillInclusive == 1)
            {
                if (isVatServiceChargeEnable == 0)
                {
                    NetAmountDivInfo.Visible = false;
                    lblName.Text = "Sales Amount";
                    lblGrandTotal.Text = "Grand Total";
                }
                else
                {
                    NetAmountDivInfo.Visible = true;
                    lblName.Text = "Sales Amount";
                    lblGrandTotal.Text = "Service Rate";
                }
            }
            else
            {
                NetAmountDivInfo.Visible = false;
                lblGrandTotal.Text = "Grand Total";
                lblName.Text = "Sales Amount";
            }

            commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("RestaurantVat", "RestaurantVat");
            hfRestaurantVatAmount.Value = commonSetupBO.SetupValue;

            commonSetupBO = new HMCommonSetupBO();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("RestaurantServiceCharge", "RestaurantServiceCharge");
            hfRestaurantServiceCharge.Value = commonSetupBO.SetupValue;
        }
        private void CheckingRestaurantBill(int costCenterId, int tableId)
        {
            RestaurantBillBO billBO = new RestaurantBillBO();
            RestaurentBillDA billDA = new RestaurentBillDA();

            string sourceName = string.Empty;

            if (Request.QueryString["tokenId"] != null)
            {
                sourceName = "RestaurantToken";
            }
            if (Request.QueryString["tableId"] != null)
            {
                sourceName = "RestaurantTable";
            }
            if (Request.QueryString["RoomNumber"] != null)
            {
                sourceName = "GuestRoom";
            }

            int kotId = 0;

            billBO = billDA.GetLastRestaurantBillInfoByCostCenterIdNTable(sourceName, costCenterId, tableId);

            if (billBO != null)
            {
                if (billBO.BillId > 0)
                {
                    RestaurantBearerDA bearerDa = new RestaurantBearerDA();
                    RestaurantBearerBO bearerbo = new RestaurantBearerBO();
                    bearerbo = bearerDa.GetRestaurantBearerInfoById(billBO.BearerId);

                    if (bearerbo != null)
                    {
                        txtWaiterNameDisplay.Text = bearerbo.UserName;
                    }

                    txtBillIdForInvoicePreview.Value = billBO.BillId.ToString();
                    btnSave.Text = "Invoice Preview";
                    chkIsBillSplit.Visible = true;
                    lblIsBillSplit.Visible = true;
                    GuestPaymentInformationDiv.Visible = true;
                    ddlDiscountType.SelectedValue = billBO.DiscountType;

                    if (billBO.DiscountType == "Fixed")
                    {
                        //restaurentBillBO.DiscountTransactionId = 0;
                    }
                    else if (billBO.DiscountType == "Percentage")
                    {
                        //restaurentBillBO.DiscountTransactionId = 0;
                    }
                    else if (billBO.DiscountType == "Member")
                    {
                        //isBillProcessedForMember = 1;
                        //ddlMemberId.SelectedValue = billBO.DiscountTransactionId.ToString();

                        //BusinessPromotionDA commonDA = new BusinessPromotionDA();
                        //BusinessPromotionBO businessPromotionBO = new BusinessPromotionBO();
                        //businessPromotionBO = commonDA.LoadDiscountRelatedInformation("Member", Convert.ToInt32(ddlMemberId.SelectedValue));
                        //if (businessPromotionBO != null)
                        //{
                        //    if (businessPromotionBO.BusinessPromotionId > 0)
                        //    {
                        //        hfTxtMemberCode.Value = businessPromotionBO.BPHead;
                        //    }
                        //}
                    }
                    else if (billBO.DiscountType == "BusinessPromotion")
                    {
                        ddlBusinessPromotionId.SelectedValue = billBO.BusinessPromotionIdNPercentAmount.ToString();
                    }

                    txtDiscountAmount.Text = billBO.DiscountAmount.ToString();

                    if (HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] != null)
                    {
                        hfCategoryWiseTotalDiscountAmount.Value = billBO.CalculatedDiscountAmount.ToString();
                    }
                    else
                    {
                        hfCategoryWiseTotalDiscountAmount.Value = "0";
                    }

                    txtCalculatedDiscountAmount.Value = billBO.CalculatedDiscountAmount.ToString();
                    txtCustomerName.Text = billBO.CustomerName;
                    cbIsComplementary.Checked = billBO.IsComplementary;
                    cbServiceCharge.Checked = billBO.IsInvoiceServiceChargeEnable;
                    cbVatAmount.Checked = billBO.IsInvoiceVatAmountEnable;
                }
                else
                {
                    txtDiscountAmount.Text = "0";
                    btnSave.Text = "Invoice Generate";
                    GuestPaymentInformationDiv.Visible = false;
                }

                List<RestaurantBillBO> classificationDiscountBOList = new List<RestaurantBillBO>();
                classificationDiscountBOList = billDA.GetRestaurantBillClassificationDiscountInfo(billBO.BillId);

                if (classificationDiscountBOList != null)
                {
                    List<string> categoryIdList = new List<string>();
                    foreach (RestaurantBillBO rb in classificationDiscountBOList)
                    {
                        categoryIdList.Add(rb.ClassificationId.ToString());
                    }

                    if (categoryIdList.Count > 0)
                        HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] = categoryIdList;

                    if (!string.IsNullOrWhiteSpace(hftxtKotNumber.Value))
                    {
                        InvCategoryDA invCategoryDA = new InvCategoryDA();
                        List<InvCategoryBO> invCategoryLst = new List<InvCategoryBO>();
                        kotId = Convert.ToInt32(hftxtKotNumber.Value);
                        invCategoryLst = invCategoryDA.GetInvCategoryDetailsForRestaurantBill(kotId);
                        gvPercentageDiscountCategory.DataSource = invCategoryLst;
                        gvPercentageDiscountCategory.DataBind();

                    }
                }
            }
            else
            {
                txtDiscountAmount.Text = "0";
                btnSave.Text = "Invoice Generate";
                GuestPaymentInformationDiv.Visible = false;
            }
        }
        private void CheckingGroupTable()
        {
            if (!string.IsNullOrWhiteSpace(txtBillIdForInvoicePreview.Value))
            {
                int billId = Convert.ToInt32(txtBillIdForInvoicePreview.Value);
                RestaurentBillDA billDA = new RestaurentBillDA();
                List<RestaurantBillBO> restaurantBillDetailInfoBOList = new List<RestaurantBillBO>();
                restaurantBillDetailInfoBOList = billDA.GetRestaurantBillDetailInfoByBillId(billId);

                if (restaurantBillDetailInfoBOList != null)
                {
                    foreach (RestaurantBillBO row in restaurantBillDetailInfoBOList)
                    {
                        if (hfSelectedTableId.Value != "")
                            hfSelectedTableId.Value = row.KotId.ToString() + "," + hfSelectedTableId.Value;
                        else
                            hfSelectedTableId.Value = row.KotId.ToString();
                    }
                }
            }
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
                        hfIsRestaurantIntegrateWithFrontOffice.Value = "0";
                        ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("2"));
                    }
                    else
                    {
                        hfIsRestaurantIntegrateWithFrontOffice.Value = "1";
                    }
                }
            }
        }
        private void IsRestaurantIntegrateWithPayroll()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithPayroll", "IsRestaurantIntegrateWithPayroll");
            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        IsRestaurantIntegrateWithPayrollVal.Value = "0";
                        ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("Employee"));
                    }
                    else
                    {
                        IsRestaurantIntegrateWithPayrollVal.Value = "1";
                    }
                }
            }
        }
        public static void OpenNewBrowserWindow(string Url, Control control)
        {
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Open", "window.open('" + Url + "');", true);
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO oldMenuEnbale = new HMCommonSetupBO();
            oldMenuEnbale = commonSetupDA.GetCommonConfigurationInfo("IsOldMenuEnable", "IsOldMenuEnable");

            if (oldMenuEnbale.SetupValue == "1")
            {
                objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.FrmChooseTableForBill.ToString());
            }
            else
            {
                objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmCostCenterChooseForRB.ToString());
            }

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            ddlDiscountType.Enabled = isSavePermission;
            txtDiscountAmount.Enabled = isSavePermission;
            cbServiceCharge.Enabled = isSavePermission;
            cbVatAmount.Enabled = isSavePermission;
            cbIsComplementary.Enabled = isSavePermission;
        }
        private void LoadBank()
        {
            BankDA da = new BankDA();
            List<BankBO> files = da.GetBankInfo();
            ddlBankName.DataSource = files;
            ddlBankName.DataTextField = "BankName";
            ddlBankName.DataValueField = "BankId";
            ddlBankName.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlBankName.Items.Insert(0, item);

            ddlBankId.DataSource = files;
            ddlBankId.DataTextField = "BankName";
            ddlBankId.DataValueField = "BankId";
            ddlBankId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            ddlBankId.Items.Insert(0, itemBank);

            ddlChequeBankId.DataSource = files;
            ddlChequeBankId.DataTextField = "BankName";
            ddlChequeBankId.DataValueField = "BankId";
            ddlChequeBankId.DataBind();
            ddlChequeBankId.Items.Insert(0, itemBank);
        }
        private void LoadMemberList()
        {
            MemMemberBasicDA memberDA = new MemMemberBasicDA();
            List<MemMemberBasicsBO> memberList = memberDA.GetMemActiveMemberListInfo();
            ddlMemberId.DataSource = memberList;
            ddlMemberId.DataTextField = "MembershipNumber";
            ddlMemberId.DataValueField = "MemberIdNDiscount";
            ddlMemberId.DataBind();

            ListItem itemMember = new ListItem();
            itemMember.Value = "0";
            itemMember.Text = hmUtility.GetDropDownFirstValue();
            ddlMemberId.Items.Insert(0, itemMember);
        }
        private void LoadGridView()
        {
            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            string categoryIdList = string.Empty;
            List<string> strCategoryIdList = new List<string>();
            string discountType = string.Empty, margeKotId = string.Empty;
            decimal discountPercentage = 0;

            if (Request.QueryString["tokenId"] != null)
            {
                hfSelectedTableId.Value = string.Empty;
                hfAlreadyAddedKotId.Value = string.Empty;
                hfAlreadyAddedTable.Value = string.Empty;
                hfSelectedTableNumber.Value = string.Empty;

                int currentTableId = !string.IsNullOrWhiteSpace(Request.QueryString["tokenId"].ToString()) ? Convert.ToInt32(Request.QueryString["tokenId"].ToString()) : 0;
                CheckingRestaurantBill(currentUserInformationBO.WorkingCostCenterId, currentTableId);

                discountType = ddlDiscountType.SelectedValue;
                discountPercentage = !string.IsNullOrEmpty(txtDiscountAmount.Text.Trim()) ? Convert.ToDecimal(txtDiscountAmount.Text.Trim()) : 0;

                if (Session["CategoryWisePercentageDiscountInfo"] != null)
                {
                    strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                    foreach (string rowCWPD in strCategoryIdList)
                    {
                        categoryIdList += categoryIdList != string.Empty ? ("," + rowCWPD) : rowCWPD;
                    }
                }

                KotBillMasterBO entityBO = new KotBillMasterBO();
                KotBillMasterDA entityDA = new KotBillMasterDA();

                if (!string.IsNullOrWhiteSpace(Request.QueryString["tokenId"]))
                {
                    int PaxQuantity = 0;
                    int tableId = Convert.ToInt32(Request.QueryString["tokenId"]);
                    KotBillDetailDA da = new KotBillDetailDA();
                    entityBO = entityDA.GetKotBillMasterInfoByTableId(currentUserInformationBO.WorkingCostCenterId, "RestaurantToken", tableId);
                    if (entityBO.KotId > 0)
                    {
                        if (!string.IsNullOrEmpty(hftxtKotNumber.Value))
                        {
                            margeKotId += hftxtKotNumber.Value.ToString() + "," + entityBO.KotId.ToString();
                        }
                        else
                        {
                            margeKotId = entityBO.KotId.ToString();
                        }

                        Boolean IsBillGenerated = false;
                        string kotIdList = string.Empty;
                        List<KotBillMasterBO> entityKotDetailIdBOList = new List<KotBillMasterBO>();
                        entityKotDetailIdBOList = entityDA.GetBillDetailInfoByKotId("RestaurantToken", entityBO.KotId);

                        if (entityKotDetailIdBOList != null)
                        {
                            foreach (KotBillMasterBO row in entityKotDetailIdBOList)
                            {
                                if (row.PaxQuantity != 0)
                                {
                                    PaxQuantity = row.PaxQuantity;
                                }

                                if (kotIdList != "")
                                    kotIdList = kotIdList + "," + row.KotId.ToString();
                                else
                                    kotIdList = row.KotId.ToString();

                                if (hfSelectedTableId.Value != "")
                                    hfSelectedTableId.Value = hfSelectedTableId.Value + "," + row.SourceId.ToString();
                                else
                                    hfSelectedTableId.Value = row.SourceId.ToString();

                                if (Request.QueryString["tokenId"].ToString() != row.SourceId.ToString())
                                {
                                    if (hfSelectedTableNumber.Value != "")
                                        hfSelectedTableNumber.Value += "," + row.SourceNumber;
                                    else
                                        hfSelectedTableNumber.Value = row.SourceNumber;

                                    AlreadyAddedNewTable.Style.Remove("display");
                                }
                            }

                            hfAlreadyAddedTable.Value = hfSelectedTableId.Value;
                            hfAlreadyAddedKotId.Value = kotIdList;

                            if (hfSelectedTableNumber.Value == string.Empty)
                            {
                                AddedNewTables.InnerText = "";
                                hfSelectedTableNumber.Value = "";
                                AlreadyAddedNewTable.Style.Add("display", "none");
                            }
                            else
                            {
                                AddedNewTables.InnerText = hfSelectedTableNumber.Value;
                            }
                        }

                        if (!IsBillGenerated)
                        {
                            Session["txtKotIdInformation"] = entityBO.KotId;
                            txtKotId.Value = entityBO.KotId.ToString();
                            txtPaxQuantity.Text = entityBO.PaxQuantity.ToString();

                            KotBillDetailDA entityDetailDA = new KotBillDetailDA();
                            List<KotBillDetailBO> entityDetailBO = entityDetailDA.GetRestaurantBillDetailInfoDynamicallyForInvoice(currentUserInformationBO.WorkingCostCenterId.ToString(), entityBO.KotId.ToString(), discountType, discountPercentage, categoryIdList, margeKotId.ToString(), cbIsComplementary.Checked);

                            foreach (KotBillDetailBO row in entityDetailBO)
                            {
                                row.ServiceRate = Math.Round(row.ServiceRate, 2);
                                row.Amount = Math.Round(row.Amount);
                                row.ItemLineTotal = Math.Round((row.ServiceRate + row.ServiceCharge + row.VatAmount), 2);
                            }

                            gvRestaurentBill.DataSource = entityDetailBO;
                            gvRestaurentBill.DataBind();
                            LoadSplitBillListBox(entityDetailBO);
                            CalculateAmountTotal();
                        }
                        else
                        {
                            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                            List<KotBillDetailBO> kotDetails = new List<KotBillDetailBO>();
                            kotDetails = da.GetRestaurantBillDetailInfoDynamicallyForInvoice(currentUserInformationBO.WorkingCostCenterId.ToString(), kotIdList, discountType, discountPercentage, categoryIdList, margeKotId, cbIsComplementary.Checked);

                            if (kotDetails.Count > 0)
                            {
                                txtPaxQuantity.Text = PaxQuantity.ToString();

                                foreach (KotBillDetailBO row in kotDetails)
                                {
                                    row.ServiceRate = Math.Round(row.ServiceRate, 2);
                                    row.Amount = Math.Round(row.Amount);
                                    row.ItemLineTotal = Math.Round((row.ServiceRate + row.ServiceCharge + row.VatAmount), 2);
                                }

                                gvRestaurentBill.DataSource = kotDetails;
                                gvRestaurentBill.DataBind();
                                LoadSplitBillListBox(kotDetails);
                                CalculateAmountTotal();

                                SourceIdHiddenField.Value = Request.QueryString["tokenId"];
                                hfCostCenterId.Value = currentUserInformationBO.WorkingCostCenterId.ToString();
                            }
                        }
                    }
                }
            }
            else if (Request.QueryString["tableId"] != null)
            {
                hfSelectedTableId.Value = string.Empty;
                hfAlreadyAddedKotId.Value = string.Empty;
                hfAlreadyAddedTable.Value = string.Empty;
                hfSelectedTableNumber.Value = string.Empty;

                hfCostCenterId.Value = currentUserInformationBO.WorkingCostCenterId.ToString();
                int workingCostCenterId = !string.IsNullOrWhiteSpace(hfCostCenterId.Value) ? Convert.ToInt32(hfCostCenterId.Value) : 0;

                KotBillMasterBO entityBO = new KotBillMasterBO();
                KotBillMasterDA entityDA = new KotBillMasterDA();

                if (!string.IsNullOrWhiteSpace(Request.QueryString["tableId"]))
                {
                    int PaxQuantity = 0;
                    int tableId = Convert.ToInt32(Request.QueryString["tableId"]);

                    int currentTableId = !string.IsNullOrWhiteSpace(Request.QueryString["tableId"].ToString()) ? Convert.ToInt32(Request.QueryString["tableId"].ToString()) : 0;
                    CheckingRestaurantBill(currentUserInformationBO.WorkingCostCenterId, currentTableId);

                    discountType = ddlDiscountType.SelectedValue;
                    discountPercentage = !string.IsNullOrEmpty(txtDiscountAmount.Text.Trim()) ? Convert.ToDecimal(txtDiscountAmount.Text.Trim()) : 0;

                    if (Session["CategoryWisePercentageDiscountInfo"] != null)
                    {
                        strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                        foreach (string rowCWPD in strCategoryIdList)
                        {
                            categoryIdList += categoryIdList != string.Empty ? ("," + rowCWPD) : rowCWPD;
                        }
                    }

                    KotBillDetailDA da = new KotBillDetailDA();
                    entityBO = entityDA.GetKotBillMasterInfoByTableId(workingCostCenterId, "RestaurantTable", tableId);
                    if (entityBO.KotId > 0)
                    {
                        if (!string.IsNullOrEmpty(hftxtKotNumber.Value))
                        {
                            margeKotId += hftxtKotNumber.Value.ToString() + "," + entityBO.KotId.ToString();
                        }
                        else
                        {
                            margeKotId = entityBO.KotId.ToString();
                        }

                        Boolean IsBillGenerated = false;
                        string kotIdList = string.Empty;
                        List<KotBillMasterBO> entityKotDetailIdBOList = new List<KotBillMasterBO>();
                        entityKotDetailIdBOList = entityDA.GetBillDetailInfoByKotId("RestaurantTable", entityBO.KotId);

                        if (entityKotDetailIdBOList != null)
                        {
                            foreach (KotBillMasterBO row in entityKotDetailIdBOList)
                            {
                                IsBillGenerated = true;
                                if (row.PaxQuantity != 0)
                                {
                                    PaxQuantity = row.PaxQuantity;
                                }

                                if (kotIdList != "")
                                    kotIdList = kotIdList + "," + row.KotId.ToString();
                                else
                                    kotIdList = row.KotId.ToString();

                                if (hfSelectedTableId.Value != "")
                                    hfSelectedTableId.Value = hfSelectedTableId.Value + "," + row.SourceId.ToString();
                                else
                                    hfSelectedTableId.Value = row.SourceId.ToString();

                                if (Request.QueryString["tableId"].ToString() != row.SourceId.ToString())
                                {
                                    if (hfSelectedTableNumber.Value != "")
                                        hfSelectedTableNumber.Value += "," + row.SourceNumber;
                                    else
                                        hfSelectedTableNumber.Value = row.SourceNumber;

                                    AlreadyAddedNewTable.Style.Remove("display");
                                }
                            }

                            hfAlreadyAddedTable.Value = hfSelectedTableId.Value;
                            hfAlreadyAddedKotId.Value = kotIdList;

                            if (hfSelectedTableNumber.Value == string.Empty)
                            {
                                AddedNewTables.InnerText = "";
                                hfSelectedTableNumber.Value = "";
                                AlreadyAddedNewTable.Style.Add("display", "none");
                            }
                            else
                            {
                                AddedNewTables.InnerText = hfSelectedTableNumber.Value;
                            }
                        }

                        if (!IsBillGenerated)
                        {
                            Session["txtKotIdInformation"] = entityBO.KotId;
                            txtKotId.Value = entityBO.KotId.ToString();
                            txtPaxQuantity.Text = entityBO.PaxQuantity.ToString();
                            KotBillDetailDA entityDetailDA = new KotBillDetailDA();
                            List<KotBillDetailBO> entityDetailBO = entityDetailDA.GetRestaurantBillDetailInfoDynamicallyForInvoice(currentUserInformationBO.WorkingCostCenterId.ToString(), entityBO.KotId.ToString(), discountType, discountPercentage, categoryIdList, margeKotId.ToString(), cbIsComplementary.Checked);

                            foreach (KotBillDetailBO row in entityDetailBO)
                            {
                                row.ServiceRate = Math.Round(row.ServiceRate, 2);
                                row.Amount = Math.Round(row.Amount);
                                row.ItemLineTotal = Math.Round((row.ServiceRate + row.ServiceCharge + row.VatAmount), 2);
                            }

                            gvRestaurentBill.DataSource = entityDetailBO;
                            gvRestaurentBill.DataBind();
                            LoadSplitBillListBox(entityDetailBO);
                            CalculateAmountTotal();
                        }
                        else
                        {
                            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                            List<KotBillDetailBO> kotDetails = new List<KotBillDetailBO>();
                            kotDetails = da.GetRestaurantBillDetailInfoDynamicallyForInvoice(currentUserInformationBO.WorkingCostCenterId.ToString(), kotIdList, discountType, discountPercentage, categoryIdList, margeKotId, cbIsComplementary.Checked);

                            if (kotDetails.Count > 0)
                            {
                                txtPaxQuantity.Text = PaxQuantity.ToString();

                                foreach (KotBillDetailBO row in kotDetails)
                                {
                                    row.ServiceRate = Math.Round(row.ServiceRate, 2);
                                    row.Amount = Math.Round(row.Amount);
                                    row.ItemLineTotal = Math.Round((row.ServiceRate + row.ServiceCharge + row.VatAmount), 2);
                                }

                                gvRestaurentBill.DataSource = kotDetails;
                                gvRestaurentBill.DataBind();
                                LoadSplitBillListBox(kotDetails);
                                CalculateAmountTotal();

                                SourceIdHiddenField.Value = Request.QueryString["tableId"];
                                hfCostCenterId.Value = currentUserInformationBO.WorkingCostCenterId.ToString();
                            }
                        }
                    }
                }
            }
            else if (Request.QueryString["RoomNumber"] != null)
            {
                KotBillMasterBO entityBO = new KotBillMasterBO();
                KotBillMasterDA entityDA = new KotBillMasterDA();

                if (!string.IsNullOrWhiteSpace(Request.QueryString["RoomNumber"]))
                {
                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                    RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                    roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(Request.QueryString["RoomNumber"]);
                    if (roomAllocationBO.RoomId > 0)
                    {
                        int currentTableId = roomAllocationBO.RegistrationId;
                        CheckingRestaurantBill(currentUserInformationBO.WorkingCostCenterId, currentTableId);

                        discountType = ddlDiscountType.SelectedValue;
                        discountPercentage = !string.IsNullOrEmpty(txtDiscountAmount.Text.Trim()) ? Convert.ToDecimal(txtDiscountAmount.Text.Trim()) : 0;

                        if (Session["CategoryWisePercentageDiscountInfo"] != null)
                        {
                            strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                            foreach (string rowCWPD in strCategoryIdList)
                            {
                                categoryIdList += categoryIdList != string.Empty ? ("," + rowCWPD) : rowCWPD;
                            }
                        }

                        SourceIdHiddenField.Value = roomAllocationBO.RegistrationId.ToString();
                        KotBillDetailDA da = new KotBillDetailDA();
                        entityBO = entityDA.GetKotBillMasterInfoByTableId(currentUserInformationBO.WorkingCostCenterId, "GuestRoom", roomAllocationBO.RegistrationId);

                        if (entityBO.KotId > 0)
                        {
                            if (!string.IsNullOrEmpty(hftxtKotNumber.Value))
                            {
                                margeKotId += hftxtKotNumber.Value.ToString() + "," + entityBO.KotId.ToString();
                            }
                            else
                            {
                                margeKotId = entityBO.KotId.ToString();
                            }

                            Session["txtKotIdInformation"] = entityBO.KotId;
                            txtKotId.Value = entityBO.KotId.ToString();
                            KotBillDetailDA entityDetailDA = new KotBillDetailDA();
                            List<KotBillDetailBO> entityDetailBO = entityDetailDA.GetRestaurantBillDetailInfoDynamicallyForInvoice(currentUserInformationBO.WorkingCostCenterId.ToString(), entityBO.KotId.ToString(), discountType, discountPercentage, categoryIdList, margeKotId.ToString(), cbIsComplementary.Checked);

                            foreach (KotBillDetailBO row in entityDetailBO)
                            {
                                row.Amount = Math.Round(row.Amount);
                                row.ItemLineTotal = Math.Round(row.ServiceRate + row.ServiceCharge + row.VatAmount);
                            }

                            gvRestaurentBill.DataSource = entityDetailBO;
                            gvRestaurentBill.DataBind();
                            LoadSplitBillListBox(entityDetailBO);
                            CalculateAmountTotal();
                        }
                    }
                }
            }
        }
        private void LoadSplitBillListBox(List<KotBillDetailBO> kotDetails)
        {
            List<KotBillDetailBO> kotDetailsInfoList = new List<KotBillDetailBO>();

            if (kotDetails != null)
            {
                foreach (KotBillDetailBO row in kotDetails)
                {
                    if (Convert.ToInt32(row.ItemUnit) > 1)
                    {
                        int i = 1;
                        while (i <= Convert.ToInt32(row.ItemUnit))
                        {
                            kotDetailsInfoList.Add(row);
                            i++;
                        }
                    }
                    else
                    {
                        kotDetailsInfoList.Add(row);
                    }
                }
            }

            this.lstLeft.DataSource = kotDetailsInfoList;
            this.lstLeft.DataTextField = "ItemName";
            this.lstLeft.DataValueField = "ItemId";
            this.lstLeft.DataBind();
        }
        private void LoadWaiterAndKotNumber()
        {
            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int sourceId = 0;
            string sourceName = string.Empty;

            if (Request.QueryString["tokenId"] != null)
            {
                sourceName = "RestaurantToken";
                sourceId = Convert.ToInt32(Request.QueryString["tokenId"]);
            }
            else if (Request.QueryString["tableId"] != null)
            {
                sourceName = "RestaurantTable";
                sourceId = Convert.ToInt32(Request.QueryString["tableId"]);
            }
            else if (Request.QueryString["RoomNumber"] != null)
            {
                sourceName = "GuestRoom";
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(Request.QueryString["RoomNumber"]);
                if (roomAllocationBO.RoomId > 0)
                {
                    sourceId = roomAllocationBO.RegistrationId;
                }
            }

            hfSelectedTableId.Value = string.Empty;
            hfAlreadyAddedKotId.Value = string.Empty;
            hfAlreadyAddedTable.Value = string.Empty;
            hfSelectedTableNumber.Value = string.Empty;

            KotBillMasterBO entityBO = new KotBillMasterBO();
            KotBillMasterDA entityDA = new KotBillMasterDA();

            int PaxQuantity = 0;

            KotBillDetailDA da = new KotBillDetailDA();
            entityBO = entityDA.GetKotBillMasterInfoByTableId(currentUserInformationBO.WorkingCostCenterId, sourceName, sourceId);
            if (entityBO.KotId > 0)
            {
                hftxtKotNumber.Value = entityBO.KotId.ToString();
                txtKotNumberDisplay.Text = entityBO.KotId.ToString();
                txtWaiterNameDisplay.Text = entityBO.BearerName.ToString();
            }

            if (!string.IsNullOrWhiteSpace(hftxtKotNumber.Value))
            {
                InvCategoryDA invCategoryDA = new InvCategoryDA();
                List<InvCategoryBO> invCategoryLst = new List<InvCategoryBO>();
                int kotId = Convert.ToInt32(hftxtKotNumber.Value);
                invCategoryLst = invCategoryDA.GetInvCategoryDetailsForRestaurantBill(kotId);
                gvPercentageDiscountCategory.DataSource = invCategoryLst;
                gvPercentageDiscountCategory.DataBind();
            }
        }
        private void Cancel()
        {
            ddlBankName.SelectedValue = "0";
            ddlPayMode.SelectedIndex = 0;
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = null;
            txtDiscountAmount.Text = "0.00";
            txtServiceCharge.Text = "0.00";
            txtVatAmount.Text = "0.00";
            txtCardNumber.Text = string.Empty;
            txtCustomerName.Text = string.Empty;
            txtGrandTotal.Text = "0.00";
            txtSalesAmount.Text = "0.00";
            gvRestaurentBill.DataSource = null;
            gvRestaurentBill.DataBind();
            LoadSplitBillListBox(null);
            txtKotNumberDisplay.Text = "0";
            txtSalesAmount.Text = "0.00";
            txtAdvanceAmount.Text = "0.00";
            ddlDiscountType.SelectedIndex = 0;
            txtDiscountAmount.Text = "0.00";

            txtDiscountedAmountHiddenField.Value = "0.00";
            txtDiscountedAmount.Text = "0.00";

            txtPaxQuantity.Text = "0.00";
            txtServiceChargeAmountHiddenField.Value = "0.00";
            txtServiceCharge.Text = "0.00";
            txtVatAmountHiddenField.Value = "0.00";
            txtVatAmount.Text = "0.00";

            txtGrandTotalHiddenField.Value = "0.00";
            txtCalculatedDiscountAmount.Value = "0.00";

            txtGrandTotal.Text = "0.00";
            txtNetAmount.Text = "0.00";
            txtCustomerName.Text = string.Empty;

            GuestPaymentInformationDiv.Visible = false;
            btnPaymentPosting.Visible = false;
        }
        private bool IsFrmValid()
        {
            bool flag = true;
            if (string.IsNullOrWhiteSpace(txtDiscountAmount.Text.Trim()))
            {
                txtDiscountAmount.Focus();
                CommonHelper.AlertInfo(innboardMessage, "Please provide Discount Amount.", AlertType.Warning);
                flag = false;
            }
            else if (string.IsNullOrWhiteSpace(txtSalesAmount.Text.Trim()))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide Sales Amount.", AlertType.Warning);
                txtSalesAmount.Focus();
                flag = false;
            }
            else if (string.IsNullOrWhiteSpace(txtGrandTotal.Text.Trim()))
            {
                txtGrandTotal.Focus();
                CommonHelper.AlertInfo(innboardMessage, "Please provide Grand Total", AlertType.Warning);
                flag = false;
            }
            else if (string.IsNullOrWhiteSpace(txtVatAmount.Text.Trim()))
            {
                txtVatAmount.Focus();
                CommonHelper.AlertInfo(innboardMessage, "Please provide Vat Amount", AlertType.Warning);
                flag = false;
            }

            return flag;
        }
        public void CalculateAmountTotal()
        {
            int currentCostCenterId = !string.IsNullOrWhiteSpace(hfCostCenterId.Value) ? Convert.ToInt32(hfCostCenterId.Value) : 0;
            int currentTableId = !string.IsNullOrWhiteSpace(SourceIdHiddenField.Value) ? Convert.ToInt32(SourceIdHiddenField.Value) : 0;
            CheckingRestaurantBill(currentCostCenterId, currentTableId);

            List<KotBillDetailBO> kotDetails = new List<KotBillDetailBO>();
            kotDetails = (List<KotBillDetailBO>)gvRestaurentBill.DataSource;

            if (kotDetails != null)
            {
                string roundChar = "0";

                if (hfIsRestaurantBillAmountWillRound.Value == "0")
                {
                    roundChar = "#0.00";
                }

                decimal serviceVatTotal = 0;
                var summary = new CalculatePayment();

                if (cbIsComplementary.Checked == true)
                {
                    summary = (from kbd in kotDetails
                               where kbd.ItemUnit > 0
                               group kbd by 1 into kot
                               select new CalculatePayment
                               {
                                   NetAmount = kot.Sum(s => s.UnitRate * s.ItemUnit),
                                   DiscountedAmount = 0.00M,
                                   ServiceCharge = 0.00M,
                                   VatAmount = 0.00M,
                                   ServiceRate = 0.00M,
                                   GrandTotal = 0.00M

                               }).FirstOrDefault();
                }
                else if (cbIsComplementary.Checked == false)
                {
                    summary = (from kbd in kotDetails
                               where kbd.ItemUnit > 0
                               group kbd by 1 into kot
                               select new CalculatePayment
                               {
                                   NetAmount = kot.Sum(s => s.UnitRate * s.ItemUnit),
                                   DiscountedAmount = kot.Sum(s => s.DiscountedAmount),
                                   ServiceCharge = kot.Sum(s => s.ServiceCharge),
                                   VatAmount = kot.Sum(s => s.VatAmount),
                                   ServiceRate = kot.Sum(s => s.ServiceRate),
                                   GrandTotal = kot.Sum(s => (s.ServiceRate + s.ServiceCharge + s.VatAmount))
                               }).FirstOrDefault();

                }

                if (cbServiceCharge.Checked == false && cbVatAmount.Checked == false)
                {
                    serviceVatTotal = summary.ServiceCharge + summary.VatAmount;
                }
                else if (cbServiceCharge.Checked == false)
                {
                    serviceVatTotal = summary.ServiceCharge;
                }
                else if (cbVatAmount.Checked == false)
                {
                    serviceVatTotal = summary.VatAmount;
                }

                txtSalesAmount.Text = new HMCommonDA().CurrencyMask(summary.NetAmount.ToString(roundChar));
                txtSalesAmountHiddenField.Value = summary.NetAmount.ToString(roundChar);

                //if (cbIsComplementary.Checked == false)
                if (cbIsComplementary.Checked == true)
                {
                    txtDiscountedAmount.Text = "0";
                    txtDiscountedAmountHiddenField.Value = "0";
                }
                else
                {
                    txtDiscountedAmount.Text = summary.DiscountedAmount.ToString("#0.00");
                    txtDiscountedAmountHiddenField.Value = summary.DiscountedAmount.ToString("#0.00");
                }

                txtAdvanceAmount.Text = "0.00"; //AmtDisTotal.ToString("#0.00");

                txtVatAmount.Text = cbVatAmount.Checked == true ? summary.VatAmount.ToString("#0.00") : "0";
                txtVatAmountHiddenField.Value = cbVatAmount.Checked == true ? summary.VatAmount.ToString("#0.00") : "0";

                txtServiceCharge.Text = cbServiceCharge.Checked == true ? summary.ServiceCharge.ToString("#0.00") : "0";
                txtServiceChargeAmountHiddenField.Value = cbServiceCharge.Checked == true ? summary.ServiceCharge.ToString("#0.00") : "0";

                //AmtDisTotal = !string.IsNullOrWhiteSpace(txtDiscountAmount.Text) ? Convert.ToDecimal(txtDiscountAmount.Text) : 0;

                if (hfIsRestaurantBillInclusive.Value == "0")
                {
                    if (hfIsRestaurantBillAmountWillRound.Value == "0")
                    {
                        txtGrandTotal.Text = (summary.GrandTotal - serviceVatTotal).ToString(roundChar);
                        txtGrandTotalHiddenField.Value = (summary.GrandTotal - serviceVatTotal).ToString(roundChar);

                        txtNetAmount.Text = txtGrandTotalHiddenField.Value;
                        txtNetAmountHiddenField.Value = txtGrandTotalHiddenField.Value;
                        txtPaymentAmount.Text = txtGrandTotalHiddenField.Value;
                    }
                    else
                    {
                        txtGrandTotal.Text = Math.Round((summary.GrandTotal - serviceVatTotal), MidpointRounding.AwayFromZero).ToString();
                        txtGrandTotalHiddenField.Value = Math.Round((summary.GrandTotal - serviceVatTotal), MidpointRounding.AwayFromZero).ToString();

                        txtNetAmount.Text = txtGrandTotalHiddenField.Value;
                        txtNetAmountHiddenField.Value = txtGrandTotalHiddenField.Value;
                        txtPaymentAmount.Text = txtGrandTotalHiddenField.Value;
                    }
                }
                else
                {
                    if (hfIsRestaurantBillAmountWillRound.Value == "0")
                    {
                        txtGrandTotal.Text = summary.ServiceRate.ToString(roundChar);
                        txtGrandTotalHiddenField.Value = summary.ServiceRate.ToString(roundChar);

                        txtNetAmount.Text = summary.GrandTotal.ToString(roundChar);
                        txtNetAmountHiddenField.Value = summary.GrandTotal.ToString(roundChar);
                        txtPaymentAmount.Text = summary.GrandTotal.ToString(roundChar);
                    }
                    else
                    {
                        txtGrandTotal.Text = Math.Round(summary.ServiceRate, MidpointRounding.AwayFromZero).ToString();
                        txtGrandTotalHiddenField.Value = Math.Round(summary.ServiceRate, MidpointRounding.AwayFromZero).ToString();

                        txtNetAmount.Text = Math.Round(summary.GrandTotal, MidpointRounding.AwayFromZero).ToString();
                        txtNetAmountHiddenField.Value = Math.Round(summary.GrandTotal, MidpointRounding.AwayFromZero).ToString();
                        txtPaymentAmount.Text = Math.Round(summary.GrandTotal, MidpointRounding.AwayFromZero).ToString();
                    }
                }

                txtReceiveAmount.Text = txtGrandTotal.Text;

                if (gvRestaurentBill.Rows.Count == 0)
                {
                    btnSave.Visible = false;
                    isBillProcessedBoxEnable = 1;
                }
                else
                {
                    isBillProcessedBoxEnable = -1;
                    btnSave.Visible = true;
                }
            }
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadRoomNumber()
        {
            string firstItem = string.Empty;
            ListItem itemComplementaryRoomService = new ListItem();
            if (hfIsRestaurantIntegrateWithFrontOffice.Value == "1")
            {
                int condition = 0;
                RoomNumberDA roomNumberDA = new RoomNumberDA();

                List<RoomNumberBO> AvailableRoomList = roomNumberDA.GetRoomNumberInfoByCondition(0, condition);
                ddlRoomId.DataSource = AvailableRoomList;
                ddlRoomId.DataTextField = "RoomNumber";
                ddlRoomId.DataValueField = "RoomId";
                ddlRoomId.DataBind();

                ListItem itemRoom = new ListItem();
                itemRoom.Value = "0";
                itemRoom.Text = hmUtility.GetDropDownFirstValue();
                ddlRoomId.Items.Insert(0, itemRoom);

                List<RoomNumberBO> AvailableComplementaryRoomList = new List<RoomNumberBO>();

                AvailableComplementaryRoomList.AddRange(AvailableRoomList);

                foreach (RoomNumberBO row in AvailableComplementaryRoomList)
                {
                    if (row.RoomNumber == hfComplimentaryRoomNumber.Value)
                    {
                        firstItem = "Room # " + row.RoomNumber;
                        itemComplementaryRoomService.Value = row.RoomId.ToString();
                        itemComplementaryRoomService.Text = firstItem;
                    }
                }

                foreach (RoomNumberBO row in AvailableComplementaryRoomList)
                {
                    row.RoomNumber = "Room # " + row.RoomNumber;
                }

                ddlComplementaryRoomId.DataSource = AvailableComplementaryRoomList;
                ddlComplementaryRoomId.DataTextField = "RoomNumber";
                ddlComplementaryRoomId.DataValueField = "RoomId";
                ddlComplementaryRoomId.DataBind();

                ListItem itemComplementaryRoom = new ListItem();
                itemComplementaryRoom.Value = "0";
                itemComplementaryRoom.Text = "Walk-In Guest";

                if (!string.IsNullOrEmpty(hfComplimentaryRoomNumber.Value))
                {
                    ddlComplementaryRoomId.Items.Insert(0, itemComplementaryRoomService);
                    ddlComplementaryRoomId.Enabled = false;
                }
                else
                {
                    ddlComplementaryRoomId.Items.Insert(0, itemComplementaryRoom);
                    ddlComplementaryRoomId.Enabled = true;
                }
                hfComplimentaryRoomNumber.Value = string.Empty;
            }
            else
            {
                ddlComplementaryRoomId.DataSource = null;
                ddlComplementaryRoomId.DataTextField = "RoomNumber";
                ddlComplementaryRoomId.DataValueField = "RoomId";
                ddlComplementaryRoomId.DataBind();

                ListItem itemComplementaryRoom = new ListItem();
                itemComplementaryRoom.Value = "0";
                itemComplementaryRoom.Text = "Walk-In Guest";
                ddlComplementaryRoomId.Items.Insert(0, itemComplementaryRoom);
            }
        }
        private void LoadRelatedInformation()
        {
            if (ddlRoomId.SelectedIndex != -1)
            {
                int roomId = Convert.ToInt32(ddlRoomId.SelectedValue);
            }
        }
        private void LoadPayRoomInfo()
        {
            if (hfIsRestaurantIntegrateWithFrontOffice.Value == "1")
            {
                RoomNumberDA roomNumberDA = new RoomNumberDA();
                ddlRoomNumberId.DataSource = roomNumberDA.GetRoomNumberInfoByCondition(0, 0);
                ddlRoomNumberId.DataTextField = "RoomNumber";
                ddlRoomNumberId.DataValueField = "RoomId";
                ddlRoomNumberId.DataBind();

                ListItem itemRoom = new ListItem();
                itemRoom.Value = "0";
                itemRoom.Text = hmUtility.GetDropDownFirstValue();
                ddlRoomNumberId.Items.Insert(0, itemRoom);
            }
        }
        private void LoadEmployeeInfo()
        {
            if (IsRestaurantIntegrateWithPayrollVal.Value == "1")
            {
                EmployeeDA entityDA = new EmployeeDA();
                ddlEmpId.DataSource = entityDA.GetEmployeeInfo();
                ddlEmpId.DataTextField = "DisplayName";
                ddlEmpId.DataValueField = "EmpId";
                ddlEmpId.DataBind();

                ListItem itemEmpId = new ListItem();
                itemEmpId.Value = "0";
                itemEmpId.Text = hmUtility.GetDropDownFirstValue();
                ddlEmpId.Items.Insert(0, itemEmpId);
            }
        }
        private void LoadRegisteredGuestCompanyInfo()
        {
            GuestCompanyDA companyDa = new GuestCompanyDA();
            ddlCompanyName.DataSource = companyDa.GetGuestCompanyInfo();
            ddlCompanyName.DataTextField = "CompanyName";
            ddlCompanyName.DataValueField = "CompanyId";
            ddlCompanyName.DataBind();

            ListItem itemCompanyName = new ListItem();
            itemCompanyName.Value = "0";
            itemCompanyName.Text = hmUtility.GetDropDownFirstValue();
            ddlCompanyName.Items.Insert(0, itemCompanyName);
        }
        private void LoadAccountHeadInfo()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            lblPaymentAccountHead.Text = "Payment Receive In";

            List<CommonPaymentModeBO> commonPaymentModeBOList = new List<CommonPaymentModeBO>();
            commonPaymentModeBOList = hmCommonDA.GetCommonPaymentModeInfo("All");

            CommonPaymentModeBO cashPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Cash").FirstOrDefault();
            CommonPaymentModeBO cardPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Card").FirstOrDefault();
            CommonPaymentModeBO chequePaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Cheque").FirstOrDefault();
            CommonPaymentModeBO companyPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Company").FirstOrDefault();
            CommonPaymentModeBO mBankingPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "M-Banking").FirstOrDefault();
            CommonPaymentModeBO refundPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Refund").FirstOrDefault();

            this.ddlCashReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cashPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlCashReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCashReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCashReceiveAccountsInfo.DataBind();

            this.ddlCardReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cardPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlCardReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCardReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCardReceiveAccountsInfo.DataBind();

            this.ddlCardPaymentAccountHeadId.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cardPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlCardPaymentAccountHeadId.DataTextField = "NodeHead";
            this.ddlCardPaymentAccountHeadId.DataValueField = "NodeId";
            this.ddlCardPaymentAccountHeadId.DataBind();

            this.ddlChecquePaymentAccountHeadId.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + chequePaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            this.ddlChecquePaymentAccountHeadId.DataTextField = "NodeHead";
            this.ddlChecquePaymentAccountHeadId.DataValueField = "NodeId";
            this.ddlChecquePaymentAccountHeadId.DataBind();

            this.ddlCompanyPaymentAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + companyPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            ddlCompanyPaymentAccountHead.DataTextField = "NodeHead";
            ddlCompanyPaymentAccountHead.DataValueField = "NodeId";
            ddlCompanyPaymentAccountHead.DataBind();

            this.ddlRefundAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + refundPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            this.ddlRefundAccountHead.DataTextField = "NodeHead";
            this.ddlRefundAccountHead.DataValueField = "NodeId";
            this.ddlRefundAccountHead.DataBind();

            CustomFieldBO EmployeePaymentToAccountsInfo = new CustomFieldBO();
            EmployeePaymentToAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("EmployeePaymentAccountsInfo");
            this.ddlEmployeePaymentAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + EmployeePaymentToAccountsInfo.FieldValue.ToString() + ")");
            this.ddlEmployeePaymentAccountHead.DataTextField = "NodeHead";
            this.ddlEmployeePaymentAccountHead.DataValueField = "NodeId";
            this.ddlEmployeePaymentAccountHead.DataBind();
        }
        public static string LoadGuestPaymentDetailGridViewByWM(string paymentDescription)
        {
            string strTable = "";
            List<GuestBillPaymentBO> detailList = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            if (detailList != null)
            {
                strTable += "<table style='width:100%' class='table table-bordered table-condensed table-responsive' id='ReservationDetailGrid'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Payment Mode</th><th align='left' scope='col'>Description</th><th align='left' scope='col'>Amount</th><th align='center' scope='col'>Action</th></tr>";
                int counter = 0;
                foreach (GuestBillPaymentBO dr in detailList)
                {
                    counter++;
                    if (counter % 2 == 0)
                    {
                        // It's even
                        strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 40%;'>" + dr.PaymentMode + "</td>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:White;'><td align='left' style='width: 40%;'>" + dr.PaymentMode + "</td>";
                    }
                    strTable += "<td align='left' style='width: 40%;'>" + dr.PaymentDescription + "</td>";
                    strTable += "<td align='left' style='width: 20%;'>" + dr.PaymentAmount + "</td>";
                    strTable += "<td align='center' style='width: 15%;'>";
                    strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformGuestPaymentDetailDelete(" + dr.PaymentId + "," + dr.PaidServiceId + ")' alt='Delete Information' border='0' />";
                    strTable += "</td></tr>";
                }
                strTable += "</table>";
                if (strTable == "")
                {
                    strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
                }
            }
            return strTable;
        }
        private bool CheckingPaymentInformationForBill()
        {
            bool flag = true;
            decimal sum = 0;
            Boolean IsPaidServiceExist = false;
            var List = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            if (List != null)
            {
                if (List.Count > 0)
                {
                    for (int i = 0; i < List.Count; i++)
                    {
                        if (List[i].PaymentMode == "Refund")
                        {
                            sum = sum - Convert.ToDecimal(List[i].PaymentAmount);
                        }
                        else
                        {
                            sum = sum + Convert.ToDecimal(List[i].PaymentAmount);
                        }

                        int paidServiceValue = !string.IsNullOrWhiteSpace(List[i].PaidServiceId.ToString()) ? Convert.ToInt32(List[i].PaidServiceId) : 0;

                        if (paidServiceValue > 0)
                        {
                            IsPaidServiceExist = true;
                        }
                    }
                }
            }

            if (IsPaidServiceExist == false)
            {
                decimal mtxtGrandTotal = 0;
                decimal changeAmount = 0;
                int isRestaurantBillInclusive = !string.IsNullOrWhiteSpace(hfIsRestaurantBillInclusive.Value) ? Convert.ToInt32(hfIsRestaurantBillInclusive.Value) : 0;
                if (isRestaurantBillInclusive == 1)
                {
                    mtxtGrandTotal = !string.IsNullOrWhiteSpace(txtNetAmountHiddenField.Value) ? Convert.ToDecimal(txtNetAmountHiddenField.Value) : 0;
                }
                else
                {
                    mtxtGrandTotal = !string.IsNullOrWhiteSpace(txtGrandTotalHiddenField.Value) ? Convert.ToDecimal(txtGrandTotalHiddenField.Value) : 0;
                }
                if (!string.IsNullOrEmpty(hfChangeAmount.Value))
                {
                    changeAmount = Convert.ToDecimal(hfChangeAmount.Value);
                }
                if (changeAmount == 0)
                {
                    if (Math.Round(sum, MidpointRounding.AwayFromZero) != Math.Round((mtxtGrandTotal), MidpointRounding.AwayFromZero))
                    {
                        txtPaymentAmount.Focus();
                        CommonHelper.AlertInfo(innboardMessage, "Grand Total and Guest Payment Amount is not Equal.", AlertType.Warning);
                        flag = false;
                    }
                }
            }
            return flag;
        }
        public void ApprovedGuestPaidService(int restaurantBillId, int registrationId, int rowId)
        {
            string guestServiceType = "RestaurantService";
            int tmpApprovedId = 0;
            Boolean IsGuestTodaysBillAdd = false;

            GHServiceBillBO serviceList = new GHServiceBillBO();
            List<GHServiceBillBO> files = new List<GHServiceBillBO>();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

            RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();
            roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Convert.ToInt32(registrationId));

            if (roomRegistrationBO != null)
            {
                GuestHouseCheckOutDA guestHouseCheckOutDA = new GuestHouseCheckOutDA();
                GuestHouseCheckOutDetailBO IsGuestTodaysBillAddBOInfo = guestHouseCheckOutDA.GetIsGuestTodaysBillAdd(registrationId.ToString());
                if (IsGuestTodaysBillAddBOInfo.IsGuestTodaysBillAdd == 1)
                {
                    IsGuestTodaysBillAdd = true;
                    files = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch(guestServiceType, rowId, DateTime.Now, roomRegistrationBO.RoomNumber.ToString()).Where(x => x.IsPaidService == true).ToList();
                }
                else
                {
                    if (DateTime.Now.Date == roomRegistrationBO.ArriveDate.Date)
                    {
                        files = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch(guestServiceType, rowId, DateTime.Now, roomRegistrationBO.RoomNumber.ToString()).Where(x => x.IsPaidService == true).ToList();
                    }
                    else
                    {
                        files = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch(guestServiceType, rowId, DateTime.Now.AddDays(-1), roomRegistrationBO.RoomNumber.ToString()).Where(x => x.IsPaidService == true).ToList();
                    }
                }

                if (files.Count > 0)
                {
                    serviceList.ApprovedId = files[0].ApprovedId;
                    serviceList.PaymentMode = files[0].RoomNumber.ToString();
                    serviceList.RegistrationId = Int32.Parse(files[0].RegistrationId.ToString());
                    serviceList.ServiceBillId = Int32.Parse(files[0].ServiceBillId.ToString());
                    if (IsGuestTodaysBillAdd == true)
                    {
                        serviceList.ServiceDate = Convert.ToDateTime(files[0].ServiceDate.ToString());
                    }
                    else
                    {
                        if (DateTime.Now.Date == roomRegistrationBO.ArriveDate.Date)
                        {
                            serviceList.ServiceDate = Convert.ToDateTime(files[0].ServiceDate.ToString());
                        }
                        else
                        {
                            serviceList.ServiceDate = Convert.ToDateTime(files[0].ServiceDate.ToString()).AddDays(-1);
                        }
                    }

                    serviceList.ServiceType = files[0].ServiceType.ToString();
                    serviceList.ServiceId = Int32.Parse(files[0].ServiceId.ToString());
                    serviceList.ServiceName = files[0].ServiceName.ToString();
                    serviceList.ServiceQuantity = Convert.ToDecimal(files[0].ServiceQuantity.ToString());
                    serviceList.ServiceRate = Convert.ToDecimal(files[0].ServiceRate.ToString());
                    serviceList.DiscountAmount = Convert.ToDecimal(files[0].DiscountAmount.ToString());

                    serviceList.VatAmount = Convert.ToDecimal(files[0].VatAmount.ToString());
                    serviceList.ServiceCharge = Convert.ToDecimal(files[0].ServiceCharge.ToString());

                    serviceList.ApprovedStatus = true;
                    serviceList.IsPaidService = Convert.ToBoolean(files[0].IsPaidService.ToString());

                    if (IsGuestTodaysBillAdd == true)
                    {
                        serviceList.ApprovedDate = DateTime.Now;
                    }
                    else
                    {
                        if (DateTime.Now.Date == roomRegistrationBO.ArriveDate.Date)
                        {
                            serviceList.ApprovedDate = DateTime.Now;
                        }
                        else
                        {
                            serviceList.ApprovedDate = DateTime.Now.AddDays(-1);
                        }
                    }
                    serviceList.CreatedBy = userInformationBO.UserInfoId;

                    serviceList.VatAmountPercent = Convert.ToDecimal(files[0].VatAmountPercent.ToString());
                    serviceList.ServiceChargePercent = Convert.ToDecimal(files[0].ServiceChargePercent.ToString());
                    serviceList.CalculatedPercentAmount = Convert.ToDecimal(files[0].CalculatedPercentAmount.ToString());
                    serviceList.IsPaidServiceAchieved = true;
                    serviceList.RestaurantBillId = restaurantBillId;

                    if (serviceList.ApprovedId > 0)
                    {
                        Boolean status = roomRegistrationDA.UpdateGuestServiceBillApprovedInfo(serviceList);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Achieve Operation Successfull.", AlertType.Warning);
                        }
                    }
                    else
                    {
                        Boolean status = roomRegistrationDA.SaveGuestServiceBillApprovedInfo(serviceList, out tmpApprovedId);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "Achieve Operation Successfull.", AlertType.Warning);
                        }
                    }
                }
            }
        }
        private void BillProcessingForTokenSystem()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO commonRestaurantBillPrintAndPreview = new HMCommonSetupBO();
            commonRestaurantBillPrintAndPreview = commonSetupDA.GetCommonConfigurationInfo("RestaurantBillPrintAndPreview", "RestaurantBillPrintAndPreview");

            string categoryIdList = string.Empty;
            List<string> strCategoryIdList = new List<string>();
            List<RestaurantBillBO> categoryWisePercentageDiscountBOList = new List<RestaurantBillBO>();
            if (Session["CategoryWisePercentageDiscountInfo"] != null)
            {
                strCategoryIdList = HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] as List<string>;

                foreach (string rowCWPD in strCategoryIdList)
                {
                    RestaurantBillBO categoryWisePercentageDiscountBO = new RestaurantBillBO();
                    categoryWisePercentageDiscountBO.ClassificationId = Convert.ToInt32(rowCWPD);
                    categoryWisePercentageDiscountBOList.Add(categoryWisePercentageDiscountBO);

                    categoryIdList += categoryIdList != string.Empty ? ("," + rowCWPD) : rowCWPD;
                }
            }

            int isRestaurantBillInclusive = !string.IsNullOrWhiteSpace(hfIsRestaurantBillInclusive.Value) ? Convert.ToInt32(hfIsRestaurantBillInclusive.Value) : 0;

            if (string.IsNullOrWhiteSpace(txtBillIdForInvoicePreview.Value))
            {
                if (!IsFrmValid())
                {
                    return;
                }

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RestaurantBillBO restaurentBillBO = new RestaurantBillBO();
                restaurentBillBO.CostCenterId = Convert.ToInt32(hfCostCenterId.Value);
                restaurentBillBO.DiscountType = ddlDiscountType.SelectedValue.ToString();

                if (restaurentBillBO.DiscountType == "Fixed")
                {
                    restaurentBillBO.DiscountTransactionId = 0;
                }
                else if (restaurentBillBO.DiscountType == "Percentage")
                {
                    restaurentBillBO.DiscountTransactionId = 0;
                }
                else if (restaurentBillBO.DiscountType == "Member")
                {
                    string[] ddlMemberIdSelectedValue = ddlMemberId.SelectedValue.Split(',');
                    int memMemberId = Convert.ToInt32(ddlMemberIdSelectedValue[0]);
                    restaurentBillBO.DiscountTransactionId = memMemberId;
                }
                else if (restaurentBillBO.DiscountType == "BusinessPromotion")
                {
                    restaurentBillBO.DiscountTransactionId = Convert.ToInt32(ddlBusinessPromotionId.SelectedValue);
                }

                restaurentBillBO.DiscountAmount = Convert.ToDecimal(txtDiscountAmount.Text);
                restaurentBillBO.CalculatedDiscountAmount = !string.IsNullOrWhiteSpace(txtCalculatedDiscountAmount.Value) ? Convert.ToDecimal(txtCalculatedDiscountAmount.Value) : 0;
                restaurentBillBO.ServiceCharge = !string.IsNullOrWhiteSpace(txtServiceChargeAmountHiddenField.Value) ? Convert.ToDecimal(txtServiceChargeAmountHiddenField.Value) : 0; //Convert.ToDecimal(txtServiceCharge.Text);
                restaurentBillBO.VatAmount = !string.IsNullOrWhiteSpace(txtVatAmountHiddenField.Value) ? Convert.ToDecimal(txtVatAmountHiddenField.Value) : 0; //Convert.ToDecimal(txtVatAmount.Text);
                restaurentBillBO.CustomerName = txtCustomerName.Text;
                restaurentBillBO.PaxQuantity = !string.IsNullOrWhiteSpace(txtPaxQuantity.Text) ? Convert.ToInt32(txtPaxQuantity.Text) : 1;
                restaurentBillBO.CreatedBy = userInformationBO.UserInfoId;
                restaurentBillBO.TableId = Int32.Parse(SourceIdHiddenField.Value);
                restaurentBillBO.BillDate = DateTime.Now;
                restaurentBillBO.BillPaymentDate = DateTime.Now;

                pnlHomeButtonInfo.Visible = false;
                if (Request.QueryString["tokenId"] != null)
                {
                    restaurentBillBO.SourceName = "RestaurantToken";
                    restaurentBillBO.BillPaidBySourceId = Int32.Parse(Request.QueryString["tokenId"]);
                    pnlHomeButtonInfo.Visible = true;
                    hfHomePanelInfo.Value = "frmCostCenterChooseForKot.aspx";
                }
                else if (Request.QueryString["tableId"] != null)
                {
                    restaurentBillBO.SourceName = "RestaurantTable";
                    restaurentBillBO.BillPaidBySourceId = Int32.Parse(Request.QueryString["tableId"]);
                }
                else if (Request.QueryString["RoomNumber"] != null)
                {
                    restaurentBillBO.SourceName = "GuestRoom";
                    restaurentBillBO.BillPaidBySourceId = Int32.Parse(Request.QueryString["RoomNumber"]);
                }

                if (cbServiceCharge.Checked)
                {
                    restaurentBillBO.IsInvoiceServiceChargeEnable = true;
                }
                else
                {
                    restaurentBillBO.IsInvoiceServiceChargeEnable = false;
                }

                if (cbVatAmount.Checked)
                {
                    restaurentBillBO.IsInvoiceVatAmountEnable = true;
                }
                else
                {
                    restaurentBillBO.IsInvoiceVatAmountEnable = false;
                }

                if (isRestaurantBillInclusive == 0)
                {
                    restaurentBillBO.SalesAmount = !string.IsNullOrWhiteSpace(txtSalesAmount.Text) ? Convert.ToDecimal(txtSalesAmount.Text) : 0;
                    restaurentBillBO.GrandTotal = Convert.ToDecimal(txtGrandTotalHiddenField.Value);
                }
                else
                {
                    restaurentBillBO.SalesAmount = Convert.ToDecimal(txtGrandTotalHiddenField.Value);
                    restaurentBillBO.GrandTotal = !string.IsNullOrWhiteSpace(txtSalesAmount.Text) ? Convert.ToDecimal(txtSalesAmount.Text) - restaurentBillBO.CalculatedDiscountAmount : 0;
                }

                if (cbIsComplementary.Checked)
                { restaurentBillBO.IsComplementary = true; }
                else
                { restaurentBillBO.IsComplementary = false; }

                List<RestaurantBillDetailBO> restaurentBillDetailBOList = new List<RestaurantBillDetailBO>();

                KotBillMasterBO entityBO = new KotBillMasterBO();
                KotBillMasterDA entityDA = new KotBillMasterDA();

                if (Request.QueryString["tokenId"] != null)
                {
                    if (hfSelectedTableId.Value != "")
                        hfSelectedTableId.Value = Request.QueryString["tokenId"] + "," + hfSelectedTableId.Value;
                    else
                        hfSelectedTableId.Value = Request.QueryString["tokenId"];

                    string[] tableId = hfSelectedTableId.Value.Split(',');
                    string tablesId = string.Empty, kotIdList = string.Empty;

                    for (int i = 0; i < tableId.Count(); i++)
                    {
                        entityBO = entityDA.GetKotBillMasterInfoByTableId(restaurentBillBO.CostCenterId, "RestaurantToken", int.Parse(tableId[i]));

                        RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                        restaurentBillDetailBO.KotId = entityBO.KotId;

                        restaurentBillDetailBOList.Add(restaurentBillDetailBO);
                    }
                }
                else if (Request.QueryString["tableId"] != null)
                {
                    if (hfSelectedTableId.Value != "")
                        hfSelectedTableId.Value = Request.QueryString["tableId"] + "," + hfSelectedTableId.Value;
                    else
                        hfSelectedTableId.Value = Request.QueryString["tableId"];

                    string[] tableId = hfSelectedTableId.Value.Split(',');
                    string tablesId = string.Empty, kotIdList = string.Empty;

                    for (int i = 0; i < tableId.Count(); i++)
                    {
                        entityBO = entityDA.GetKotBillMasterInfoByTableId(restaurentBillBO.CostCenterId, "RestaurantTable", int.Parse(tableId[i]));
                        RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                        restaurentBillDetailBO.KotId = entityBO.KotId;
                        restaurentBillDetailBOList.Add(restaurentBillDetailBO);
                    }
                }
                else if (Request.QueryString["RoomNumber"] != null)
                {
                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                    RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                    roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(Request.QueryString["RoomNumber"]);
                    if (roomAllocationBO.RoomId > 0)
                    {
                        entityBO = entityDA.GetKotBillMasterInfoByTableId(restaurentBillBO.CostCenterId, "GuestRoom", roomAllocationBO.RegistrationId);
                        RestaurantBillDetailBO restaurentBillDetailBO = new RestaurantBillDetailBO();
                        restaurentBillDetailBO.KotId = entityBO.KotId;
                        restaurentBillDetailBOList.Add(restaurentBillDetailBO);
                    }
                }

                List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

                if (guestPaymentDetailListForGrid.Count <= 0)
                {
                    if (restaurentBillBO.IsComplementary)
                    {
                        if (Convert.ToInt32(ddlComplementaryRoomId.SelectedValue) > 0)
                        {
                            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                            List<RoomRegistrationBO> roomRegistrationBO = new List<RoomRegistrationBO>();
                            roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(Convert.ToInt32(ddlComplementaryRoomId.SelectedValue));
                            if (roomRegistrationBO != null)
                            {
                                int registrationId = roomRegistrationBO[0].RegistrationId;
                                guestPaymentDetailListForGrid.Add(new GuestBillPaymentBO
                                {
                                    NodeId = Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                                    PaymentType = "Other Room",
                                    AccountsPostingHeadId = Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                                    BillPaidBy = 0,
                                    BankId = registrationId,
                                    RegistrationId = registrationId,
                                    FieldId = !string.IsNullOrWhiteSpace(hfLocalCurrencyId.Value) ? Convert.ToInt32(hfLocalCurrencyId.Value) : 0,
                                    ConvertionRate = 1,
                                    CurrencyAmount = 0,
                                    PaymentAmount = 0,
                                    ChecqueDate = DateTime.Now,
                                    PaymentMode = "Other Room",
                                    PaymentId = 1,
                                    CardNumber = "",
                                    CardType = "",
                                    ExpireDate = null,
                                    ChecqueNumber = "",
                                    CardHolderName = "",
                                    PaymentDescription = ""
                                });
                            }
                        }
                        else
                        {
                            guestPaymentDetailListForGrid.Add(new GuestBillPaymentBO
                            {
                                NodeId = Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                                PaymentType = "Advance",
                                AccountsPostingHeadId = Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                                BillPaidBy = 0,
                                BankId = 0,
                                RegistrationId = 0,
                                FieldId = !string.IsNullOrWhiteSpace(hfLocalCurrencyId.Value) ? Convert.ToInt32(hfLocalCurrencyId.Value) : 0,
                                ConvertionRate = 1,
                                CurrencyAmount = 0,
                                PaymentAmount = 0,
                                ChecqueDate = DateTime.Now,
                                PaymentMode = (restaurentBillBO.IsComplementary == true ? "Discount" : "Cash"),
                                PaymentId = 1,
                                CardNumber = "",
                                CardType = "",
                                ExpireDate = null,
                                ChecqueNumber = "",
                                CardHolderName = "",
                                PaymentDescription = ""
                            });
                        }
                    }
                    else
                    {
                        guestPaymentDetailListForGrid.Add(new GuestBillPaymentBO
                        {
                            NodeId = Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                            PaymentType = "Advance",
                            AccountsPostingHeadId = Convert.ToInt32(ddlEmployeePaymentAccountHead.SelectedValue),
                            BillPaidBy = 0,
                            BankId = 0,
                            RegistrationId = 0,
                            FieldId = !string.IsNullOrWhiteSpace(hfLocalCurrencyId.Value) ? Convert.ToInt32(hfLocalCurrencyId.Value) : 0,
                            ConvertionRate = 1,
                            CurrencyAmount = 0,
                            PaymentAmount = 0,
                            ChecqueDate = DateTime.Now,
                            PaymentMode = (restaurentBillBO.IsComplementary == true ? "Discount" : "Cash"),
                            PaymentId = 1,
                            CardNumber = "",
                            CardType = "",
                            ExpireDate = null,
                            ChecqueNumber = "",
                            CardHolderName = "",
                            PaymentDescription = ""
                        });
                    }
                }

                RestaurentBillDA restaurentBillDA = new RestaurentBillDA();
                int SourceId = 0;
                SourceId = Int32.Parse(SourceIdHiddenField.Value);
                if (SourceId > 0)
                {
                    int billID = 0;

                    int success = restaurentBillDA.SaveRestaurantBill(restaurentBillBO, restaurentBillDetailBOList, guestPaymentDetailListForGrid, categoryWisePercentageDiscountBOList, categoryIdList, true, true, out billID);
                    txtBillIdForInvoicePreview.Value = billID.ToString();

                    txtBillId.Value = success.ToString();
                    if (success > 0)
                    {
                        GuestPaymentInformationDiv.Visible = true;
                        int currentCostCenterId = !string.IsNullOrWhiteSpace(hfCostCenterId.Value) ? Convert.ToInt32(hfCostCenterId.Value) : 0;
                        int currentTableId = !string.IsNullOrWhiteSpace(SourceIdHiddenField.Value) ? Convert.ToInt32(SourceIdHiddenField.Value) : 0;
                        int rBillId = !string.IsNullOrWhiteSpace(txtBillIdForInvoicePreview.Value) ? Convert.ToInt32(txtBillIdForInvoicePreview.Value) : 0;
                        Cancel();
                                               
                        HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
                        invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("RestaurantBillInvoiceTemplate", "RestaurantBillInvoiceTemplate");

                        if (Convert.ToInt32(commonRestaurantBillPrintAndPreview.SetupValue) == (int)(HMConstants.RestaurantBillPrintAndPreview.DirectPrint))
                        {
                            PrinterInfoDA da = new PrinterInfoDA();
                            RestaurentBillDA rda = new RestaurentBillDA();
                            KotBillDetailDA billDA = new KotBillDetailDA();

                            List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();
                            List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();

                            restaurantBill = rda.GetRestaurantBillReport(billID);
                            if (restaurantBill != null)
                            {
                                if (restaurantBill.Count > 0)
                                {
                                    tableNumberInformation = restaurantBill[0].TableNumber;
                                    kotIdInformation = Convert.ToInt32(restaurantBill[0].KotId);
                                    List<PrinterInfoBO> files = da.GetRestaurentItemTypeInfoByKotId(Convert.ToInt32(restaurantBill[0].KotId));
                                    List<PrinterInfoBO> invoicePrinter = da.GetPrinterInfoByCostCenterPrintType(restaurantBill[0].CostCenterId, "InvoiceItem");
                                    if (invoicePrinter != null)
                                    {
                                        if (invoicePrinter.Count > 0)
                                        {
                                            if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == (int)(HMConstants.PrintTemplate.Template1))
                                            {
                                                PrintReportRestaurantBill(invoicePrinter[0], restaurantBill);

                                                foreach (PrinterInfoBO pinfo in files)
                                                {
                                                    strCostCenterId = pinfo.CostCenterId;
                                                    strPrinterInfoId = pinfo.PrinterInfoId;
                                                    strCostCenterId = pinfo.CostCenterId;
                                                    strCostCenterName = pinfo.CostCenter;
                                                    companyName = pinfo.KitchenOrStockName;

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

                                                    UserInformationBO currentUserInformationBO = new UserInformationBO();
                                                    currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                                                    waiterName = currentUserInformationBO.UserName.ToString();

                                                    if (pinfo.StockType == "StockItem")
                                                    {
                                                        entityBOList = billDA.GetKotOrderSubmitInfo(Convert.ToInt32(restaurantBill[0].KotId), restaurantBill[0].CostCenterId, "StockItem", true);
                                                    }
                                                    else
                                                    {
                                                        entityBOList = billDA.GetKotOrderSubmitInfo(Convert.ToInt32(restaurantBill[0].KotId), restaurantBill[0].CostCenterId, "KitchenItem", true).Where(x => x.KitchenId == pinfo.KitchenId && x.PrinterName == pinfo.PrinterName).ToList();
                                                    }

                                                    if (entityBOList.Count > 0)
                                                    {
                                                        PrintReportKot(pinfo, entityBOList, false);
                                                    }
                                                }

                                                Response.Redirect("frmCostCenterChooseForKot.aspx");
                                            }
                                            else if (Convert.ToInt32(invoiceTemplateBO.SetupValue) == (int)(HMConstants.PrintTemplate.Template4))
                                            {
                                                entityBOList = billDA.GetKotOrderSubmitInfo(Convert.ToInt32(restaurantBill[0].KotId), restaurantBill[0].CostCenterId, "AllItem", false);

                                                List<KotBillDetailBO> kotOrderSubmitEntityBOList = entityBOList.Where(x => x.ItemUnit > 0 && x.IsChanged == false).ToList();
                                                List<KotBillDetailBO> changedOrEditedEntityBOList = entityBOList.Where(x => x.IsChanged == true).ToList();
                                                List<KotBillDetailBO> voidOrDeletedItemEntityBOList = entityBOList.Where(x => x.ItemUnit < 0).ToList();

                                                PrintReportKot(invoicePrinter[0], restaurantBill, kotOrderSubmitEntityBOList, changedOrEditedEntityBOList, voidOrDeletedItemEntityBOList, entityBOList[0].Remarks, false);

                                                Response.Redirect("frmCostCenterChooseForKot.aspx");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    string url = "/POS/Reports/frmReportBillInfo.aspx?billID=" + txtBillId.Value;
                                    string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes');";
                                    ClientScript.RegisterStartupScript(GetType(), "script", s, true);
                                }
                            }
                        }
                        else
                        {
                            string url = "/POS/Reports/frmReportBillInfo.aspx?billID=" + txtBillId.Value;
                            string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes');";
                            ClientScript.RegisterStartupScript(GetType(), "script", s, true);
                        }
                    }
                }
            }

            hftxtKotNumber.Value = "0";
        }
        public void PrintNShowRestaurantBill(int billID)
        {
            HMCommonSetupBO commonSetupDirectPrint = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupDirectPrint = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillShowNDirectPrint", "IsRestaurantBillShowNDirectPrint");

            if (Convert.ToInt32(commonSetupDirectPrint) == 0)
            {
                return;
            }

            PrinterInfoDA da = new PrinterInfoDA();
            RestaurentBillDA rda = new RestaurentBillDA();
            List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();
            List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();

            restaurantBill = rda.GetRestaurantBillReport(billID);
            List<PrinterInfoBO> invoicePrinter = da.GetPrinterInfoByCostCenterPrintType(restaurantBill[0].CostCenterId, "InvoiceItem");

            PrintReportRestaurantBill(invoicePrinter[0], restaurantBill);
        }
        public void PrintReportRestaurantBill(PrinterInfoBO files, List<RestaurantBillReportBO> restaurantBill)
        {
            try
            {
                Double HeightInInch = 11;
                Double WidthInInch = 3.5;

                LocalReport report = new LocalReport();
                report.ReportPath = HttpContext.Current.Server.MapPath(@"~/POS/Reports/Rdlc/rptBillForPos.rdlc");
                report.EnableExternalImages = true;
                report.EnableHyperlinks = true;

                DateTime dateTime = DateTime.Now;
                string kotDate = String.Format("{0:f}", dateTime);

                int kotId = Convert.ToInt32(restaurantBill[0].KotId);

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> filesCom = companyDA.GetCompanyInfo();
                List<ReportParameter> reportParam = new List<ReportParameter>();

                if (filesCom[0].CompanyId > 0)
                {
                    reportParam.Add(new ReportParameter("CompanyProfile", filesCom[0].CompanyName));
                    reportParam.Add(new ReportParameter("CompanyAddress", filesCom[0].CompanyAddress));
                    reportParam.Add(new ReportParameter("VatRegistrationNo", filesCom[0].VatRegistrationNo));

                    if (!string.IsNullOrWhiteSpace(filesCom[0].WebAddress))
                    {
                        reportParam.Add(new ReportParameter("CompanyWeb", filesCom[0].WebAddress));
                    }

                    reportParam.Add(new ReportParameter("ContactNumber", filesCom[0].ContactNumber));
                }

                HMCommonDA hmCommonDA = new HMCommonDA();
                //reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderMiddleImagePath"))));
                string imageName = hmCommonDA.GetOutletImageNameByCostCenterId(restaurantBill[0].CostCenterId);
                if (!string.IsNullOrWhiteSpace(imageName))
                {
                    reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
                }
                else
                {
                    reportParam.Add(new ReportParameter("Path", "Hide"));
                }

                reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
                reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
                reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));
                reportParam.Add(new ReportParameter("RestaurantMushakInfo", hmCommonDA.GetCustomFieldValueByFieldName("RestaurantMushakInfo")));

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isCompanyNameShowOnRestaurantInvoice = new HMCommonSetupBO();
                isCompanyNameShowOnRestaurantInvoice = commonSetupDA.GetCommonConfigurationInfo("IsCompanyNameShowOnRestaurantInvoice", "IsCompanyNameShowOnRestaurantInvoice");
                if (Convert.ToInt32(isCompanyNameShowOnRestaurantInvoice.SetupValue) == 1)
                {
                    reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "1"));
                }
                else
                {
                    reportParam.Add(new ReportParameter("IsCompanyNameShowOnRestaurantInvoice", "0"));
                }

                IsRestaurantOrderSubmitDisableInfo();

                if (IsRestaurantOrderSubmitDisable)
                {
                    reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "Yes"));
                }
                else
                {
                    reportParam.Add(new ReportParameter("IsRestaurantOrderSubmitDisable", "No"));
                }

                IsRestaurantTokenInfoDisableInfo();


                if (IsRestaurantTokenInfoDisable)
                {
                    reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "Yes"));
                }
                else
                {
                    reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "No"));
                }

                HMCommonSetupBO isRestaurantIntegrateWithFrontOfficeBO = new HMCommonSetupBO();
                isRestaurantIntegrateWithFrontOfficeBO = commonSetupDA.GetCommonConfigurationInfo("IsGuestNameAndRoomNoTextShowInInvoice", "IsGuestNameAndRoomNoTextShowInInvoice");
                if (isRestaurantIntegrateWithFrontOfficeBO != null)
                {
                    if (isRestaurantIntegrateWithFrontOfficeBO.SetupValue == "1")
                    {
                        reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "1"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "0"));
                    }
                }

                DateTime currentDate = DateTime.Now;
                HMCommonDA printDateDA = new HMCommonDA();
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

                reportParam.Add(new ReportParameter("PrintDateTime", printDate));

                PermissionSet permissions = new PermissionSet(PermissionState.Unrestricted);
                report.SetBasePermissionsForSandboxAppDomain(permissions);

                report.SetParameters(reportParam);

                var dataSet = report.GetDataSourceNames();

                report.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill));

                ReportDirectPrinter print = new ReportDirectPrinter();
                print.PrintDefaultPage(report, files.PrinterName, WidthInInch, HeightInInch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                parms[3] = new ReportParameter("TableNo", tableNumberInformation);
                parms[4] = new ReportParameter("KotNo", kotIdInformation.ToString());
                parms[5] = new ReportParameter("KotDate", kotDate);
                parms[6] = new ReportParameter("WaiterName", waiterName);
                parms[7] = new ReportParameter("SpecialRemarks", entityBOList[0].Remarks);
                parms[8] = new ReportParameter("RestaurantName", companyName);

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

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void PrintReportKot(PrinterInfoBO files, List<RestaurantBillReportBO> restaurantBill, List<KotBillDetailBO> kotOrderSubmitEntityBOList, List<KotBillDetailBO> changedOrEditedEntityBOList, List<KotBillDetailBO> voidOrDeletedItemEntityBOList, string specialRemarks, Boolean IsReprint)
        {
            try
            {
                Double HeightInInch = 11;
                Double WidthInInch = 3.5;

                LocalReport report = new LocalReport();
                report.ReportPath = HttpContext.Current.Server.MapPath(@"~/POS/Reports/Rdlc/rptBillForPosToken.rdlc");
                report.EnableExternalImages = true;
                report.EnableHyperlinks = true;

                DateTime dateTime = DateTime.Now;
                string kotDate = String.Format("{0:f}", dateTime);

                int kotId = Convert.ToInt32(restaurantBill[0].KotId);

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

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> filesCom = companyDA.GetCompanyInfo();
                List<ReportParameter> reportParam = new List<ReportParameter>();

                if (filesCom[0].CompanyId > 0)
                {
                    reportParam.Add(new ReportParameter("CompanyProfile", filesCom[0].CompanyName));
                    reportParam.Add(new ReportParameter("CompanyAddress", filesCom[0].CompanyAddress));
                    reportParam.Add(new ReportParameter("VatRegistrationNo", filesCom[0].VatRegistrationNo));

                    if (!string.IsNullOrWhiteSpace(filesCom[0].WebAddress))
                    {
                        reportParam.Add(new ReportParameter("CompanyWeb", filesCom[0].WebAddress));
                    }

                    reportParam.Add(new ReportParameter("ContactNumber", filesCom[0].ContactNumber));
                }

                HMCommonDA hmCommonDA = new HMCommonDA();
                //reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderMiddleImagePath"))));
                string imageName = hmCommonDA.GetOutletImageNameByCostCenterId(restaurantBill[0].CostCenterId);
                if (!string.IsNullOrWhiteSpace(imageName))
                {
                    reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
                }
                else
                {
                    reportParam.Add(new ReportParameter("Path", "Hide"));
                }

                reportParam.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
                reportParam.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
                reportParam.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));

                UserInformationBO currentUserInformationBO = new UserInformationBO();
                HMUtility hmUtility = new HMUtility();
                currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                waiterName = currentUserInformationBO.UserName.ToString();

                strCostCenterName = filesCom[0].CompanyName;
                strCostCenterDefaultView = "Token # ";
                companyName = filesCom[0].CompanyName;

                reportParam.Add(new ReportParameter("ReportTitle", "KOT"));
                reportParam.Add(new ReportParameter("CostCenter", strCostCenterName));
                reportParam.Add(new ReportParameter("SourceName", strCostCenterDefaultView));
                reportParam.Add(new ReportParameter("TableNo", tableNumberInformation));
                reportParam.Add(new ReportParameter("KotDate", kotDate));
                reportParam.Add(new ReportParameter("WaiterName", waiterName));
                reportParam.Add(new ReportParameter("RestaurantName", companyName));
                reportParam.Add(new ReportParameter("KotNo", restaurantBill[0].KotId.ToString()));
                reportParam.Add(new ReportParameter("SpecialRemarks", specialRemarks));

                IsRestaurantOrderSubmitDisableInfo();

                if (IsRestaurantOrderSubmitDisable)
                {
                    reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "Yes"));
                }
                else
                {
                    reportParam.Add(new ReportParameter("IsRestaurantTokenInfoDisable", "No"));
                }

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isRestaurantIntegrateWithFrontOfficeBO = new HMCommonSetupBO();
                isRestaurantIntegrateWithFrontOfficeBO = commonSetupDA.GetCommonConfigurationInfo("IsGuestNameAndRoomNoTextShowInInvoice", "IsGuestNameAndRoomNoTextShowInInvoice");
                if (isRestaurantIntegrateWithFrontOfficeBO != null)
                {
                    if (isRestaurantIntegrateWithFrontOfficeBO.SetupValue == "1")
                    {
                        reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "1"));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("IsGuestNameAndRoomNoTextShowInInvoice", "0"));
                    }
                }

                DateTime currentDate = DateTime.Now;
                HMCommonDA printDateDA = new HMCommonDA();
                string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);

                reportParam.Add(new ReportParameter("PrintDateTime", printDate));

                PermissionSet permissions = new PermissionSet(PermissionState.Unrestricted);
                report.SetBasePermissionsForSandboxAppDomain(permissions);

                report.SetParameters(reportParam);

                var dataSet = report.GetDataSourceNames();

                report.DataSources.Add(new ReportDataSource(dataSet[0], restaurantBill));
                report.DataSources.Add(new ReportDataSource(dataSet[1], kotOrderSubmitEntityBOList));
                report.DataSources.Add(new ReportDataSource(dataSet[2], changedOrEditedEntityBOList));
                report.DataSources.Add(new ReportDataSource(dataSet[3], voidOrDeletedItemEntityBOList));

                ReportDirectPrinter print = new ReportDirectPrinter();
                print.PrintDefaultPage(report, files.PrinterName, WidthInInch, HeightInInch);
                CommonHelper.AlertInfo(innboardMessage, "Print Operation Successfull", AlertType.Success);
            }
            catch (Exception ex)
            {
                throw ex;
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
                        //btnOrderSubmit.Visible = false;
                        IsRestaurantOrderSubmitDisable = false;
                    }
                    else
                    {
                        //btnOrderSubmit.Visible = true;
                        IsRestaurantOrderSubmitDisable = true;
                    }
                }
            }

            //imgBtnRoomWiseKotBill.Visible = false;
        }
        private void IsRestaurantTokenInfoDisableInfo()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantTokenInfoDisable", "IsRestaurantTokenInfoDisable");
            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        //btnOrderSubmit.Visible = false;
                        IsRestaurantTokenInfoDisable = false;
                    }
                    else
                    {
                        //btnOrderSubmit.Visible = true;
                        IsRestaurantTokenInfoDisable = true;
                    }
                }
            }

            //imgBtnRoomWiseKotBill.Visible = false;
        }
        //************************ User Defined Web Method ********************//
        [WebMethod(EnableSession = true)]
        public static string PerformGetTotalPaidAmountByWebMethod()
        {
            decimal sum = 0;
            var List = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            if (List.Count > 0)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    if (List[i].PaymentMode == "Refund")
                    {
                        sum = sum - Convert.ToDecimal(List[i].PaymentAmount);
                    }
                    else
                    {
                        sum = sum + Convert.ToDecimal(List[i].PaymentAmount);
                    }
                }
            }
            return sum.ToString();
        }
        [WebMethod(EnableSession = true)]
        public static ArrayList PerformDeleteGuestPaymentByWebMethod(int paymentId, int paidServiceId)
        {
            List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            GuestBillPaymentBO singleEntityBOEdit = guestPaymentDetailListForGrid.Where(x => x.PaymentId == paymentId).FirstOrDefault();

            if (guestPaymentDetailListForGrid.Contains(singleEntityBOEdit))
            {
                guestPaymentDetailListForGrid.Remove(singleEntityBOEdit);
            }

            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;
            HttpContext.Current.Session["CompanyPaymentRoomIdList"] = null;
            HttpContext.Current.Session["CompanyPaymentServiceIdList"] = null;

            int IsAlreadyServiceAdded = 0;

            var vps = guestPaymentDetailListForGrid.Where(s => s.PaidServiceId != 0).FirstOrDefault();

            if (vps == null)
            {
                if (paidServiceId != 0)
                {
                    IsAlreadyServiceAdded = 0;
                }
            }
            else
            {
                IsAlreadyServiceAdded = vps.PaidServiceId;
            }

            ArrayList list = new ArrayList();
            list.Add(LoadGuestPaymentDetailGridViewByWM(""));
            list.Add(IsAlreadyServiceAdded.ToString());

            return list;
        }
        [WebMethod(EnableSession = true)]
        public static ArrayList PerformSaveGuestPaymentDetailsInformationByWebMethod(bool isEdit, string paymentDescription, string ddlCurrency, string hfLocalCurrencyId, string conversionRate, string ddlPayMode, string ddlBankId, string txtReceiveLeadgerAmount, string ddlRegistrationId, string ddlCashPaymentAccountHead, string txtCardNumber, string ddlCardType, string txtExpireDate, string txtCardHolderName, string txtChecqueNumber, string ddlChecquePaymentAccountHeadId, string ddlCardPaymentAccountHeadId, string ddlCompanyPaymentAccountHead, string ddlPaidByRoomId, string RefundAccountHead, string ddlEmpId, string ddlEmployeePaymentAccountHead, string serviceId)
        {
            HMUtility hmUtility = new HMUtility();
            int dynamicDetailId = 0;
            int ddlPaidByRegistrationId = 0;

            List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

            GuestBillPaymentBO singleEntityBOEdit = guestPaymentDetailListForGrid.Where(x => x.PaymentMode == ddlPayMode).FirstOrDefault();

            if (guestPaymentDetailListForGrid != null)
            {
                dynamicDetailId = guestPaymentDetailListForGrid.Count + 1;
            }

            GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();
            guestBillPaymentBO.CompanyId = 0;

            if (ddlPayMode == "Other Room")
            {
                if (!string.IsNullOrWhiteSpace(ddlPaidByRoomId))
                {
                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                    List<RoomRegistrationBO> billPaidByInfoList = new List<RoomRegistrationBO>();

                    billPaidByInfoList = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(Convert.ToInt32(ddlPaidByRoomId));
                    if (billPaidByInfoList != null)
                    {
                        foreach (RoomRegistrationBO row in billPaidByInfoList)
                        {
                            ddlPaidByRegistrationId = row.RegistrationId;
                        }
                    }
                    else
                    {
                        ddlPaidByRegistrationId = Convert.ToInt32(ddlRegistrationId);
                    }
                }
                else
                {
                    ddlPaidByRegistrationId = Convert.ToInt32(ddlRegistrationId);
                }
            }
            else
            {
                ddlPaidByRegistrationId = Convert.ToInt32(ddlRegistrationId);
            }

            if (ddlPayMode == "Company")
            {
                GuestCompanyBO companyBO = new GuestCompanyBO();
                GuestCompanyDA companyDA = new GuestCompanyDA();
                companyBO = companyDA.GetGuestCompanyInfoById(Convert.ToInt32(ddlCompanyPaymentAccountHead));
                if (companyBO != null)
                {
                    if (companyBO.CompanyId > 0)
                    {
                        int accountsPostingHeadId = companyBO.NodeId;
                        guestBillPaymentBO.NodeId = accountsPostingHeadId;
                        guestBillPaymentBO.PaymentType = ddlPayMode;
                        guestBillPaymentBO.AccountsPostingHeadId = accountsPostingHeadId;
                        guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlRegistrationId);
                        guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlRegistrationId);
                        guestBillPaymentBO.BankId = Convert.ToInt32(ddlRegistrationId);
                        guestBillPaymentBO.CompanyId = companyBO.CompanyId;
                    }
                }
            }
            else if (ddlPayMode == "Employee")
            {
                guestBillPaymentBO.NodeId = Convert.ToInt32(ddlEmployeePaymentAccountHead);
                guestBillPaymentBO.PaymentType = ddlPayMode;
                guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlEmployeePaymentAccountHead);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlPaidByRegistrationId);
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlEmpId);
                guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlEmpId);
            }
            else if (ddlPayMode == "Other Room")
            {
                guestBillPaymentBO.NodeId = Convert.ToInt32(1);
                guestBillPaymentBO.PaymentType = ddlPayMode;
                guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(1);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlPaidByRegistrationId);
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlPaidByRegistrationId);
                guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlPaidByRegistrationId);
            }
            else if (ddlPayMode == "Refund")
            {
                ddlCurrency = hfLocalCurrencyId;
                conversionRate = "1";
                guestBillPaymentBO.RefundAccountHead = Int32.Parse(RefundAccountHead);
                guestBillPaymentBO.PaymentMode = "Refund";
                guestBillPaymentBO.CurrencyAmount = guestBillPaymentBO.CurrencyAmount * 1;
                guestBillPaymentBO.PaymentAmount = guestBillPaymentBO.PaymentAmount * 1;
                guestBillPaymentBO.PaymentType = "Refund";
                guestBillPaymentBO.AccountsPostingHeadId = Int32.Parse(RefundAccountHead);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlRegistrationId);
            }
            else
            {
                if (ddlPayMode == "Cash")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCashPaymentAccountHead);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPaymentAccountHead);
                }
                else if (ddlPayMode == "Card")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCardPaymentAccountHeadId);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCardPaymentAccountHeadId);
                }
                else if (ddlPayMode == "Cheque")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlChecquePaymentAccountHeadId);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlChecquePaymentAccountHeadId);
                    ddlCardType = string.Empty;
                }

                guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlBankId);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.PaymentType = "Advance";
            }

            if (ddlCurrency == hfLocalCurrencyId)
            {
                guestBillPaymentBO.IsUSDTransaction = false;
                guestBillPaymentBO.FieldId = Convert.ToInt32(ddlCurrency);
                guestBillPaymentBO.ConvertionRate = 1;
                guestBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
                guestBillPaymentBO.PaymentAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            }
            else
            {
                guestBillPaymentBO.IsUSDTransaction = true;
                guestBillPaymentBO.FieldId = Convert.ToInt32(ddlCurrency);
                guestBillPaymentBO.ConvertionRate = !string.IsNullOrWhiteSpace(conversionRate) ? Convert.ToDecimal(conversionRate) : 1;
                guestBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
                guestBillPaymentBO.PaymentAmount = guestBillPaymentBO.CurrencyAmount * guestBillPaymentBO.ConvertionRate;
            }

            guestBillPaymentBO.ChecqueDate = DateTime.Now;
            guestBillPaymentBO.PaymentMode = ddlPayMode;
            guestBillPaymentBO.PaymentId = dynamicDetailId;
            guestBillPaymentBO.CardNumber = txtCardNumber;
            if (guestBillPaymentBO.BankId > 0)
            {
                guestBillPaymentBO.CardType = ddlCardType;
            }
            else
            {
                guestBillPaymentBO.CardType = string.Empty;
            }
            if (string.IsNullOrEmpty(txtExpireDate))
            {
                guestBillPaymentBO.ExpireDate = null;
            }
            else
            {
                guestBillPaymentBO.ExpireDate = hmUtility.GetDateTimeFromString(txtExpireDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            guestBillPaymentBO.ChecqueNumber = txtChecqueNumber;
            guestBillPaymentBO.CardHolderName = txtCardHolderName;
            guestBillPaymentBO.PaymentDescription = paymentDescription;
            guestBillPaymentBO.PaymentId = dynamicDetailId;

            if (!string.IsNullOrEmpty(serviceId))
            {
                guestBillPaymentBO.PaidServiceId = int.Parse(serviceId);
            }
            else
            {
                guestBillPaymentBO.PaidServiceId = 0;
            }

            guestPaymentDetailListForGrid.Add(guestBillPaymentBO);
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;

            var pds = guestPaymentDetailListForGrid.Where(s => s.PaidServiceId > 0).FirstOrDefault();
            int IsAlreadyServiceAdded = 0;

            if (pds != null)
            {
                IsAlreadyServiceAdded = pds.PaidServiceId;
            }

            ArrayList list = new ArrayList();
            list.Add(LoadGuestPaymentDetailGridViewByWM(""));
            list.Add(IsAlreadyServiceAdded.ToString());

            return list;
        }
        [WebMethod(EnableSession = true)]
        public static string PerformForRoomGuestInformationByWebMethod(string ddlPaidByRoomId)
        {
            string CustomerName = string.Empty;

            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomRegistrationBO> billPaidByInfoList = new List<RoomRegistrationBO>();

            billPaidByInfoList = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(Convert.ToInt32(ddlPaidByRoomId));
            if (billPaidByInfoList != null)
            {

                CustomerName = billPaidByInfoList[0].GuestName;
            }

            return CustomerName;
        }
        [WebMethod(EnableSession = true)]
        public static string GetAlreadyOccupiedTable(int costCenterId, int tableId)
        {
            List<TableManagementBO> tableList = new List<TableManagementBO>();
            tableList = new TableManagementDA().GetTableInfoByCostCenterNStatus(costCenterId, 2);

            if (tableList.Count() > 0)
            {
                tableList = tableList.Where(t => t.TableId != tableId).ToList();
            }

            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'> <thead> <tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='center' scope='col'>Select</th><th align='left' scope='col'>Table Number</th></tr> </thead> <tbody>";
            int counter = 0;
            foreach (TableManagementBO dr in tableList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    strTable += "<tr style='background-color:White;'>";
                }

                strTable += "<td align='center' style='width: 60px'>";
                strTable += "&nbsp;<input type='checkbox'  id='" + dr.TableId + "' name='" + dr.TableNumber + "' value='" + dr.TableId + "' >";

                strTable += "</td><td align='left' style='width: 160px'>" + dr.TableNumber + "</td></tr>";

            }
            strTable += "</tbody> </table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            strTable += "     <button type='button' onClick='javascript:return GetCheckedTable()' id='btnAddCheckedTable' class='TransactionalButton btn btn-primary'> OK</button>";
            strTable += "     <button type='button' onclick='javascript:return ClosePopup()' id='btnAddRoomId' class='TransactionalButton btn btn-primary'> Cancel</button>";

            return strTable;
        }
        [WebMethod(EnableSession = true)]
        public static GuestPaidServiceNRegInfoViewBO GetPaidByDetailsInformationForRestaurantInvoice(string costCenterId, string transactionType, string transactionId)
        {
            GuestPaidServiceNRegInfoViewBO gpsr = new GuestPaidServiceNRegInfoViewBO();

            string PayByDetails = string.Empty;
            HMCommonDA commonDa = new HMCommonDA();
            PayByDetails = commonDa.GetPaidByDetailsInformationForRestaurantInvoice(transactionType, transactionId);

            gpsr.PayByDetails = PayByDetails;

            ////-------------- Paid Service Related Information ----------------------------------------
            //if (transactionType == "GuestRoom")
            //{
            //    if (!string.IsNullOrWhiteSpace(transactionId))
            //    {
            //        int IsGuestTodaysBillAddInfo = 0;
            //        List<GHServiceBillBO> files = new List<GHServiceBillBO>();
            //        List<GHServiceBillBO> previousDayServices = new List<GHServiceBillBO>();
            //        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

            //        if (Convert.ToInt32(transactionId) > 0)
            //        {
            //            List<RoomRegistrationBO> roomRegistrationBO = new List<RoomRegistrationBO>();
            //            roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(Convert.ToInt32(transactionId));

            //            GuestHouseCheckOutDA guestHouseCheckOutDA = new GuestHouseCheckOutDA();
            //            GuestHouseCheckOutDetailBO IsGuestTodaysBillAddBOInfo = guestHouseCheckOutDA.GetIsGuestTodaysBillAdd(transactionId);
            //            if (IsGuestTodaysBillAddBOInfo.IsGuestTodaysBillAdd == 0)
            //            {
            //                IsGuestTodaysBillAddInfo = 0;
            //                List<GHServiceBillBO> previousDaysAllServices = new List<GHServiceBillBO>();

            //                if (roomRegistrationBO != null)
            //                {
            //                    if (DateTime.Now.Date == roomRegistrationBO[0].ArriveDate.Date)
            //                    {
            //                        previousDaysAllServices = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch("RestaurantService", 0, DateTime.Now, roomRegistrationBO[0].RoomNumber.ToString());
            //                        previousDayServices.AddRange(previousDaysAllServices.Where(x => x.IsPaidService == true).ToList());
            //                    }
            //                    else
            //                    {
            //                        previousDaysAllServices = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch("RestaurantService", 0, DateTime.Now.AddDays(-1), roomRegistrationBO[0].RoomNumber.ToString());
            //                        previousDayServices.AddRange(previousDaysAllServices.Where(x => x.IsPaidService == true && x.IsPaidServiceAchieved == false).ToList());
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                IsGuestTodaysBillAddInfo = 1;

            //                files = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch("RestaurantService", 0, DateTime.Now, roomRegistrationBO[0].RoomNumber.ToString());

            //                List<GHServiceBillBO> paidServiceListBO = new List<GHServiceBillBO>();
            //                paidServiceListBO = files.Where(x => x.IsPaidService == true).ToList();
            //                foreach (GHServiceBillBO row in paidServiceListBO)
            //                {
            //                    row.TotalCalculatedAmount = Math.Round(row.TotalCalculatedAmount);
            //                }
            //            }
            //        }

            //        HotelGuestServiceInfoDA paidServiceDA = new HotelGuestServiceInfoDA();
            //        List<HotelGuestServiceInfoBO> paidServiceList = new List<HotelGuestServiceInfoBO>();
            //        paidServiceList = paidServiceDA.GetHotelGuestServiceInfoByCostCenter(Convert.ToInt32(costCenterId));

            //        List<GHServiceBillBO> costCenterItems = new List<GHServiceBillBO>();
            //        List<GHServiceBillBO> totalGuestServices = new List<GHServiceBillBO>();
            //        totalGuestServices.AddRange(previousDayServices);
            //        totalGuestServices.AddRange(files);

            //        foreach (HotelGuestServiceInfoBO row in paidServiceList)
            //        {
            //            costCenterItems.AddRange(totalGuestServices.Where(x => x.ServiceId == row.ServiceId && x.IsPaidService == true && x.IsPaidServiceAchieved == false).ToList());
            //        }

            //        HotelGuestServiceInfoDA serviceDa = new HotelGuestServiceInfoDA();
            //        List<RoomRegistrationBO> roomRegistrationList = new List<RoomRegistrationBO>();
            //        roomRegistrationList = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(Convert.ToInt32(transactionId));

            //        if (roomRegistrationList.Count > 0)
            //        {
            //            ArrayList list = new ArrayList();
            //            int count = costCenterItems.Count;
            //            for (int i = 0; i < count; i++)
            //            {
            //                list.Add(new ListItem(
            //                                        costCenterItems[i].ServiceName,
            //                                        costCenterItems[i].ServiceBillId.ToString() + "," + costCenterItems[i].TotalCalculatedAmount.ToString()
            //                                         ));
            //            }

            //            gpsr.RegistrationPaidService = list;
            //        }
            //    }
            //}

            return gpsr;
        }
        [WebMethod]
        public static BusinessPromotionBO LoadDiscountRelatedInformation(string discountType, int transactionId)
        {
            BusinessPromotionDA commonDA = new BusinessPromotionDA();
            BusinessPromotionBO businessPromotionBO = new BusinessPromotionBO();
            return commonDA.LoadDiscountRelatedInformation(discountType, transactionId);

        }
        [WebMethod]
        public static List<BusinessPromotionBO> LoadBusinessPromotionRelatedInformation(int promotionId)
        {
            BusinessPromotionDA commonDA = new BusinessPromotionDA();
            BusinessPromotionBO businessPromotionBO = new BusinessPromotionBO();
            List<BusinessPromotionBO> businessPromotionBOList = commonDA.LoadBusinessPromotionRelatedInformation(promotionId);

            return businessPromotionBOList;
        }
        [WebMethod(EnableSession = true)]
        public static string PerformPercentageDiscountInformation(int kotId, string margeKotId, int costCenterId, List<string> strCategoryIdList, string discountType, string strDiscountAmount)
        {
            string categoryIdList = string.Empty;

            foreach (string categoryId in strCategoryIdList)
            {
                if (string.IsNullOrWhiteSpace(categoryIdList))
                {
                    categoryIdList = categoryId.ToString();
                }
                else
                {
                    categoryIdList = categoryIdList + "," + categoryId.ToString();
                }
            }

            decimal discountPercentAmount = !string.IsNullOrWhiteSpace(strDiscountAmount) ? Convert.ToDecimal(strDiscountAmount) : 0;

            KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();
            KotBillMasterBO kotBillMasterBO = new KotBillMasterBO();

            if (!string.IsNullOrEmpty(margeKotId))
            {
                margeKotId += kotId.ToString() + "," + margeKotId;
            }
            else
            {
                margeKotId = kotId.ToString();
            }

            kotBillMasterBO = kotBillMasterDA.GetCategoryWisePercentageDiscountInfo(kotId, margeKotId, costCenterId, categoryIdList, discountType, discountPercentAmount);
            HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] = strCategoryIdList;
            return kotBillMasterBO.TotalAmount.ToString();
        }
        [WebMethod]
        public static CommonCurrencyBO LoadCurrencyType(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetCommonCurrencyInfoById(currecyType);
            return commonCurrencyBO;
        }
        [WebMethod]
        public static CommonCurrencyConversionBO LoadCurrencyConversionRate(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyConversionBO conversionBO = new CommonCurrencyConversionBO();
            conversionBO = commonCurrencyDA.GetCurrencyConversionRate(currecyType);
            return conversionBO;
        }
        protected void gvPercentageDiscountCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                if (HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] != null)
                {
                    List<string> classificationDiscountBOList = Session["CategoryWisePercentageDiscountInfo"] as List<string>;
                    Label lblidValue = (Label)e.Row.FindControl("lblid");

                    if (!string.IsNullOrWhiteSpace(lblidValue.Text))
                    {
                        var vc = (from cat in classificationDiscountBOList where cat.Contains(lblidValue.Text.Trim()) select cat).FirstOrDefault();

                        if (vc != null)
                        {
                            ((CheckBox)e.Row.FindControl("chkIsCheckedPermission")).Checked = true;
                        }
                        else
                        {
                            ((CheckBox)e.Row.FindControl("chkIsCheckedPermission")).Checked = false;
                        }
                    }
                }
                else if (Session["CompareCategoryWisePercentageDiscountInfo"] != null)
                {
                    List<RestaurantBillBO> classificationDiscountBOList = Session["CompareCategoryWisePercentageDiscountInfo"] as List<RestaurantBillBO>;
                    Label lblidValue = (Label)e.Row.FindControl("lblid");

                    if (!string.IsNullOrWhiteSpace(lblidValue.Text))
                    {
                        var vc = (from cat in classificationDiscountBOList where cat.ClassificationId.ToString() == lblidValue.Text.Trim() select cat).FirstOrDefault();

                        if (vc != null)
                        {
                            ((CheckBox)e.Row.FindControl("chkIsCheckedPermission")).Checked = true;
                        }
                        else
                        {
                            ((CheckBox)e.Row.FindControl("chkIsCheckedPermission")).Checked = false;
                        }
                    }
                }
            }
        }
        [WebMethod]
        public static KotWiseVatNSChargeNDiscountNComplementaryBO GetKotWiseVatNSChargeNDiscountNComplementary(int costCenterId, string categoryIdList, string discountType, decimal discountPercent, int KotId, string margeKotId, bool isComplementary, bool isInvoiceVatEnable, bool isInvoiceServiceEnable)
        {
            KotWiseVatNSChargeNDiscountNComplementaryBO kotBillSummary = new KotWiseVatNSChargeNDiscountNComplementaryBO();
            RestaurentBillDA da = new RestaurentBillDA();

            if (!string.IsNullOrEmpty(categoryIdList))
            {
                string[] categoryLst = categoryIdList.Split(',');
                List<string> cat = new List<string>();

                foreach (string s in categoryLst)
                {
                    cat.Add(s);
                }

                HttpContext.Current.Session["CategoryWisePercentageDiscountInfo"] = cat;
            }
            else
            {
                HttpContext.Current.Session.Remove("CategoryWisePercentageDiscountInfo");
            }

            if (string.IsNullOrEmpty(margeKotId))
            {
                margeKotId = KotId.ToString();
            }
            else
            {
                margeKotId = KotId.ToString() + "," + margeKotId;
            }

            //kotBillSummary = da.GetKotWiseVatNSChargeNDiscountNComplementary(costCenterId, categoryIdList, discountType, discountPercent, KotId, margeKotId, isComplementary, isInvoiceVatEnable, isInvoiceServiceEnable);

            return kotBillSummary;
        }
    }
}

public class CalculatePayment
{
    public decimal NetAmount { get; set; }
    public decimal DiscountedAmount { get; set; }
    public decimal ServiceCharge { get; set; }
    public decimal VatAmount { get; set; }
    public decimal ServiceRate { get; set; }
    public decimal GrandTotal { get; set; }
}