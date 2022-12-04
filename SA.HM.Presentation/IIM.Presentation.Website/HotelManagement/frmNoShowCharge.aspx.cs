using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.GeneralLedger;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmNoShowCharge : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected int LocalCurrencyId;
        protected int isIntegratedGeneralLedgerDiv = 1;
        protected int isPaymentSave = -1;
        protected int isSaveSuccessful = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            isIntegratedGeneralLedgerDiv = -1;
            if (!IsPostBack)
            {
                //LoadNoShowReservations();
                LoadDefaultReservations();
                LoadCurrency();
                LoadBank();
                LoadAccountHeadInfo();
                LoadLocalCurrencyId();
                Boolean isIntegrated = hmUtility.GetIsIntegratedGeneralLedger();
                if (!isIntegrated)
                {
                    isIntegratedGeneralLedgerDiv = -1;
                }
                CheckPermission();
            }
        }
        protected void gvGuestHouseService_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //this.lblMessage.Text = string.Empty;
            this.gvGuestHouseService.PageIndex = e.NewPageIndex;
            //this.LoadGridView(this.ddlReservationNo.SelectedValue);
        }
        protected void gvGuestHouseService_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                //ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                //ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                //imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                //imgUpdate.Visible = isSavePermission;
                //imgDelete.Visible = isDeletePermission;
                //imgDelete.Visible = false;
            }
        }
        protected void gvGuestHouseService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "CmdPaymentPreview")
            //{
            //    string url = "/HotelManagement/Reports/frmReportGuestPaymentInvoice.aspx?PaymentIdList=" + Convert.ToInt32(e.CommandArgument.ToString());
            //    string sPopUp = "window.open('" + url + "', 'popup_window', 'width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
            //    ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
            //    this.SearchInformation();
            //}
        }
        protected void ddlReservationNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadGridView(this.ddlReservationNo.SelectedValue);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            GuestBillPaymentDA reservationBillPaymentDA = new GuestBillPaymentDA();

            List<ReservationDetailBO> reservationDetailList = new List<ReservationDetailBO>();
            List<GuestBillPaymentBO> reservationBillPaymentList = new List<GuestBillPaymentBO>();
            List<GuestBillPaymentBO> deledtBillPaymentList = new List<GuestBillPaymentBO>();
            List<GuestBillPaymentBO> finaldeledtBillPaymentList = new List<GuestBillPaymentBO>();

            reservationDetailList = JsonConvert.DeserializeObject<List<ReservationDetailBO>>(hfSaveObj.Value);
            reservationBillPaymentList = JsonConvert.DeserializeObject<List<GuestBillPaymentBO>>(hfPaymentSaveObj.Value);
            deledtBillPaymentList = JsonConvert.DeserializeObject<List<GuestBillPaymentBO>>(hfDeletedPaymentRowId.Value);
            finaldeledtBillPaymentList = JsonConvert.DeserializeObject<List<GuestBillPaymentBO>>(hfDeletedPaymentId.Value);
            string paymentIdList = hfPaymentIdList.Value;

            foreach (GuestBillPaymentBO bp in deledtBillPaymentList)
            {
                GuestBillPaymentBO billPayment = new GuestBillPaymentBO();
                billPayment = reservationBillPaymentList.Where(s => s.PaymentId == bp.PaymentId).FirstOrDefault();
                reservationBillPaymentList.Remove(billPayment);
            }
            foreach (GuestBillPaymentBO bp in finaldeledtBillPaymentList)
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status2 = hmCommonDA.DeleteInfoById("HotelGuestBillPayment", "PaymentId", bp.PaymentId);
                if (status2)
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.GuestBillPayment.ToString(), bp.PaymentId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestBillPayment));
            }

            //string paymentIdList = string.Empty;

            foreach (GuestBillPaymentBO gbp in reservationBillPaymentList)
            {
                gbp.ChecqueDate = DateTime.Now;
                gbp.PaymentAmount = gbp.ConvertionRate * gbp.CurrencyAmount;
                gbp.PaymentDescription = "No-Show";
                gbp.ModuleName = "FrontOffice";
                gbp.PaymentModeId = 0;

                if (string.IsNullOrWhiteSpace(txtPaymentId.Value))
                {
                    int tmpPaymentId = 0;
                    gbp.CreatedBy = userInformationBO.UserInfoId;
                    Boolean paymentStatus = reservationBillPaymentDA.SaveGuestBillPaymentInfo(gbp, out tmpPaymentId, "NoShow");
                    if (paymentStatus)
                    {
                        if (string.IsNullOrEmpty(paymentIdList))
                            paymentIdList = tmpPaymentId.ToString();
                        else
                            paymentIdList = paymentIdList + ", " + tmpPaymentId.ToString();
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GuestBillPayment.ToString(), tmpPaymentId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestBillPayment));
                        //if (this.ddlReservationNo.SelectedIndex != -1)
                        //{
                        //    this.LoadGridView(this.ddlReservationNo.SelectedValue);
                        //}
                    }
                }
                else
                {
                    gbp.PaymentId = Convert.ToInt32(txtPaymentId.Value);
                    gbp.LastModifiedBy = userInformationBO.UserInfoId;
                    gbp.DealId = Convert.ToInt32(txtDealId.Value);
                    Boolean paymentStatus = reservationBillPaymentDA.UpdateGuestBillPaymentInfo(gbp);
                    if (paymentStatus)
                    {
                        if (string.IsNullOrEmpty(paymentIdList))
                            paymentIdList = gbp.PaymentId.ToString();
                        else
                            paymentIdList = paymentIdList + ", " + gbp.PaymentId.ToString();
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GuestBillPayment.ToString(), gbp.DealId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestBillPayment));
                        //if (this.ddlReservationNo.SelectedIndex != -1)
                        //{
                        //    this.LoadGridView(this.ddlReservationNo.SelectedValue);
                        //}
                    }
                }
            }
            ReservationDetailDA reservationDetailDA = new ReservationDetailDA();
            Boolean status = reservationDetailDA.UpdateReservationDetailForNoShowCharge(reservationDetailList);

            if (status)
            {
                foreach (var item in reservationDetailList)
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomReservationNoShow.ToString(), item.ReservationDetailId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomReservationNoShow));
                }

                isSaveSuccessful = 1;
                string url = "/HotelManagement/Reports/frmReportNoShowPaymentInvoice.aspx?PaymentIdList=" + paymentIdList;
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
            }
            else
            {
                isSaveSuccessful = 2;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtReservationDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a date.", AlertType.Warning);
                this.txtReservationDate.Focus();
                return;
            }
            else
            {
                DateTime searchDate = hmUtility.GetDateTimeFromString(txtReservationDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                string reservationType = ddlReservationType.SelectedValue;
                if (reservationType == "1")
                {
                    LoadNoShowReservations(searchDate);
                }
                else if (reservationType == "2")
                {

                    LoadChargedNoShowReservations(searchDate);
                    hfIsSearch.Value = "1";
                }
            }
        }
        //************************ User Defined Function ********************//
        private void LoadDefaultReservations()
        {
            RoomReservationDA reservationDA = new RoomReservationDA();
            List<RoomReservationBO> noshowReservList = new List<RoomReservationBO>();

            this.ddlReservationNo.DataSource = noshowReservList;
            this.ddlReservationNo.DataTextField = "ReservationNumber";
            this.ddlReservationNo.DataValueField = "ReservationId";
            this.ddlReservationNo.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlReservationNo.Items.Insert(0, item);
        }
        private void LoadNoShowReservations(DateTime searchdate)
        {
            RoomReservationDA reservationDA = new RoomReservationDA();
            List<RoomReservationBO> noshowReservList = new List<RoomReservationBO>();
            noshowReservList = reservationDA.GetNoShowRoomReservationInfo(searchdate, 1);

            this.ddlReservationNo.DataSource = noshowReservList;
            this.ddlReservationNo.DataTextField = "ReservationNumber";
            this.ddlReservationNo.DataValueField = "ReservationId";
            this.ddlReservationNo.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlReservationNo.Items.Insert(0, item);
        }
        private void LoadChargedNoShowReservations(DateTime searchdate)
        {
            RoomReservationDA reservationDA = new RoomReservationDA();
            List<RoomReservationBO> noshowReservList = new List<RoomReservationBO>();
            noshowReservList = reservationDA.GetNoShowRoomReservationInfo(searchdate, 0);

            this.ddlReservationNo.DataSource = noshowReservList;
            this.ddlReservationNo.DataTextField = "ReservationNumber";
            this.ddlReservationNo.DataValueField = "ReservationId";
            this.ddlReservationNo.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlReservationNo.Items.Insert(0, item);
        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("All");

            this.ddlCurrency.DataSource = currencyListBO;
            this.ddlCurrency.DataTextField = "CurrencyName";
            this.ddlCurrency.DataValueField = "CurrencyId";
            this.ddlCurrency.DataBind();

            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //this.ddlCurrency.Items.Insert(0, item);
        }
        private void LoadLocalCurrencyId()
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetLocalCurrencyInfo();

            LocalCurrencyId = commonCurrencyBO.CurrencyId;
            hfLocalCurrencyId.Value = commonCurrencyBO.CurrencyId.ToString();
            hfLocalCurrencyName.Value = commonCurrencyBO.CurrencyName;
            //hfLocalCurrencyId.Value = commonCurrencyBO.CurrencyId.ToString();
        }
        private void LoadBank()
        {
            BankDA bankDA = new BankDA();
            List<BankBO> entityBOList = new List<BankBO>();
            entityBOList = bankDA.GetBankInfo();

            this.ddlBankId.DataSource = entityBOList;
            this.ddlBankId.DataTextField = "BankName";
            this.ddlBankId.DataValueField = "BankId";
            this.ddlBankId.DataBind();

            this.ddlCompanyBank.DataSource = entityBOList;
            this.ddlCompanyBank.DataTextField = "BankName";
            this.ddlCompanyBank.DataValueField = "BankId";
            this.ddlCompanyBank.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBankId.Items.Insert(0, itemBank);
            this.ddlCompanyBank.Items.Insert(0, itemBank);
        }
        private void LoadAccountHeadInfo()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            this.lblPaymentAccountHead.Text = "Payment Receive In";
            //CustomFieldBO CashReceiveAccountsInfo = new CustomFieldBO();
            //CashReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CashReceiveAccountsInfo");

            List<CommonPaymentModeBO> commonPaymentModeBOList = new List<CommonPaymentModeBO>();
            commonPaymentModeBOList = hmCommonDA.GetCommonPaymentModeInfo("All");

            CommonPaymentModeBO cashPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Cash").FirstOrDefault();
            this.ddlCashReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cashPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            this.ddlCashReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCashReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCashReceiveAccountsInfo.DataBind();

            //this.ddlPaymentFromAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CashReceiveAccountsInfo.FieldValue.ToString() + ") AND NodeId != 8");
            //this.ddlPaymentFromAccountsInfo.DataTextField = "NodeHead";
            //this.ddlPaymentFromAccountsInfo.DataValueField = "NodeId";
            //this.ddlPaymentFromAccountsInfo.DataBind();

            //CustomFieldBO PaymentToAccountsInfo = new CustomFieldBO();
            //PaymentToAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("PaymentToCustomerForCashOut");
            //this.ddlPaymentToAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + PaymentToAccountsInfo.FieldValue.ToString() + ") AND NodeId != 8");
            //this.ddlPaymentToAccountsInfo.DataTextField = "NodeHead";
            //this.ddlPaymentToAccountsInfo.DataValueField = "NodeId";
            //this.ddlPaymentToAccountsInfo.DataBind();

            CustomFieldBO CardReceiveAccountsInfo = new CustomFieldBO();
            CardReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CardReceiveAccountsInfo");
            this.ddlCardReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CardReceiveAccountsInfo.FieldValue.ToString() + ")");
            this.ddlCardReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlCardReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlCardReceiveAccountsInfo.DataBind();

            CustomFieldBO ChequeReceiveAccountsInfo = new CustomFieldBO();
            ChequeReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("ChequeReceiveAccountsInfo");
            this.ddlChequeReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + ChequeReceiveAccountsInfo.FieldValue.ToString() + ")");
            this.ddlChequeReceiveAccountsInfo.DataTextField = "NodeHead";
            this.ddlChequeReceiveAccountsInfo.DataValueField = "NodeId";
            this.ddlChequeReceiveAccountsInfo.DataBind();

            CustomFieldBO IncomeSourceAccountsInfo = new CustomFieldBO();
            IncomeSourceAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("IncomeSourceAccountsInfo");
            this.ddlIncomeSourceAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + IncomeSourceAccountsInfo.FieldValue.ToString() + ")");
            this.ddlIncomeSourceAccountsInfo.DataTextField = "NodeHead";
            this.ddlIncomeSourceAccountsInfo.DataValueField = "NodeId";
            this.ddlIncomeSourceAccountsInfo.DataBind();

            CustomFieldBO CompanyPaymentAccountsInfo = new CustomFieldBO();
            CompanyPaymentAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CompanyPaymentAccountsInfo");
            this.ddlCompanyPaymentAccountHead.DataSource = entityDA.GetNodeMatrixInfoByAncestorNodeId(Convert.ToInt32(CompanyPaymentAccountsInfo.FieldValue));
            this.ddlCompanyPaymentAccountHead.DataTextField = "NodeHead";
            this.ddlCompanyPaymentAccountHead.DataValueField = "NodeId";
            this.ddlCompanyPaymentAccountHead.DataBind();
        }
        private void LoadGridView(string reservationId)
        {
            //this.CheckObjectPermission();
            //if (reservationId == "-1")
            //{
            //    isSearchPanelEnable = -1;
            //    return;
            //}
            if (Convert.ToInt32(reservationId) > 0)
            {
                GuestBillPaymentDA da = new GuestBillPaymentDA();
                List<GuestBillPaymentBO> files = da.GetGuestBillPaymentInfoByRegistrationId("Reservation", Convert.ToInt32(reservationId));
                //isSearchPanelEnable = 1;
                this.gvGuestHouseService.DataSource = files;
                this.gvGuestHouseService.DataBind();
                //this.gvPaymentInfo.DataSource = files;
                //this.gvPaymentInfo.DataBind();

                //if (files.Count() > 0)
                //{
                //    this.btnPaymentPreview.Visible = true;
                //    this.btnGroupPaymentPreview.Visible = true;
                //}
                //else
                //{
                //    this.btnPaymentPreview.Visible = false;
                //    this.btnGroupPaymentPreview.Visible = false;
                //}
                decimal paidTotal = 0;
                foreach (GuestBillPaymentBO gbp in files)
                {
                    paidTotal = paidTotal + gbp.PaymentAmount;
                }
                PaidTotal.InnerText = "Paid Total:" + paidTotal;
                PaymentDiv.InnerHtml = "";
            }
            else
            {
                //isSearchPanelEnable = -1;
                this.gvGuestHouseService.DataSource = null;
                this.gvGuestHouseService.DataBind();
                //this.gvPaymentInfo.DataSource = null;
                //this.gvPaymentInfo.DataBind();
                //this.btnPaymentPreview.Visible = false;
                //this.btnGroupPaymentPreview.Visible = false;
            }
        }
        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;
            btnPaymentSave.Visible = isSavePermission;
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static string LoadNoShowChargedReservations(int reservationId)
        {
            string strTable = "", noshowType = "Charged";
            int counter = 0;
            decimal totalAmount = 0, conversionRate = 0, currentConversionRate = 0;

            List<ReservationDetailBO> reservationDetailList = new List<ReservationDetailBO>();
            ReservationDetailDA reservationDetailDA = new ReservationDetailDA();
            if (reservationId > 0)
                reservationDetailList = reservationDetailDA.GetReservationDetailForNoShowReservations(reservationId, noshowType);

            strTable += "<table  id='NoShowChargedTable' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='display:none'></th><th style='display:none'></th>";
            strTable += "<th align='left' scope='col' style='width: 20%;'>Room No</th>";
            strTable += "<th align='left' scope='col' style='width: 40%;'>Room Charge</th>";
            strTable += "<th align='left' scope='col' style='width: 40%;'>No Show Charge</th>";
            strTable += "<th style='display:none'></th>";
            strTable += "</tr></thead><tbody>";

            conversionRate = reservationDetailList[0].ConversionRate;
            currentConversionRate = LoadCurrencyConversionRate(reservationDetailList[0].CurrencyType).ConversionRate;

            foreach (ReservationDetailBO rd in reservationDetailList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    strTable += "<tr style='background-color:White;'>";
                }
                else
                {
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }

                strTable += "<td align='left' style=\"display:none;\">" + rd.ReservationDetailId + "</td>";
                strTable += "<td align='left' style=\"display:none;\">" + rd.ReservationId + "</td>";
                strTable += "<td align='left' style=\"'width:20%;\">" + rd.RoomNumber + "</td>";
                strTable += "<td align='left' style=\"'width:40%;\">" + rd.CurrencyHead + " " + rd.UnitPrice.ToString() + "</td>";
                if (rd.NoShowCharge > 0)
                {
                    strTable += "<td align='left' style='width:40%;'><input id='NoshowRate' class='form-control quantitydecimal' onChange='javascript:return CalCulateTotalAmount()' type='text' value='" + rd.NoShowCharge + "'/></td>";
                    totalAmount = totalAmount + rd.NoShowCharge;
                }
                else
                {
                    strTable += "<td align='left' style='width:40%;'><input id='NoshowRate' class='form-control quantitydecimal' onChange='javascript:return CalCulateTotalAmount()' type='text' value='" + rd.UnitPrice + "'/></td>";
                    totalAmount = totalAmount + rd.UnitPrice;
                }

                strTable += "<td align='left' style=\"display:none;\">" + rd.ConversionRate + "</td>";
                strTable += "<td align='left' style=\"display:none;\">" + currentConversionRate + "</td>";

                strTable += "</tr>";
            }

            strTable += "</tbody> </table>";
            if (conversionRate != 0)
                strTable += "<div id='ConversionRate' class='control-label' style='padding-left:10px; font-weight:bold;'>Conversion Rate: " + conversionRate + " </div>";
            strTable += "<div id='TotalAmount' class='control-label' style='padding-left:10px; font-weight:bold;'>Total Amount: " + totalAmount + " </div>";

            if (reservationDetailList.Count == 0)
            {
                strTable = "";
            }
            return strTable;
        }
        [WebMethod]
        public static string LoadNoShowHoldReservations(int reservationId)
        {
            string strTable = "", noshowType = "Hold";
            int counter = 0;

            List<ReservationDetailBO> reservationDetailList = new List<ReservationDetailBO>();
            ReservationDetailDA reservationDetailDA = new ReservationDetailDA();
            if (reservationId > 0)
                reservationDetailList = reservationDetailDA.GetReservationDetailForNoShowReservations(reservationId, noshowType);

            strTable += "<table  id='NoShowHoldTable' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='display:none'></th><th style='display:none'></th>";
            strTable += "<th align='center' scope='col' style='width: 20%;'>Room No</th>";
            strTable += "<th align='center' scope='col' style='width: 40%;'>Room Charge</th>";
            strTable += "<th align='center' scope='col' style='width: 40%;'>Holdup Room Charge</th>";
            strTable += "</tr></thead><tbody>";

            foreach (ReservationDetailBO rd in reservationDetailList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    strTable += "<tr style='background-color:White;'>";
                }
                else
                {
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }

                strTable += "<td align='left' style=\"display:none;\">" + rd.ReservationDetailId + "</td>";
                strTable += "<td align='left' style=\"display:none;\">" + rd.ReservationId + "</td>";
                strTable += "<td align='center' style=\"'width:20%;\">" + rd.RoomNumber + "</td>";
                strTable += "<td align='center' style=\"'width:40%;\">" + rd.UnitPrice + "</td>";
                if (rd.NoShowCharge > 0)
                {
                    strTable += "<td align='center' style='width:40%;'><input type='text' class='form-control' value='" + rd.NoShowCharge + "' /></td>";
                }
                else
                    strTable += "<td align='center' style='width:40%;'><input type='text' class='form-control' value='" + rd.UnitPrice + "' /></td>";

                strTable += "</tr>";
            }

            strTable += "</tbody> </table>";

            if (reservationDetailList.Count == 0)
            {
                strTable = "";
            }

            return strTable;
        }
        [WebMethod]
        public static List<GuestBillPaymentBO> LoadBillPayment(int reservationId)
        {
            GuestBillPaymentDA da = new GuestBillPaymentDA();
            List<GuestBillPaymentBO> files = new List<GuestBillPaymentBO>();
            if (reservationId > 0)
                files = da.GetGuestBillPaymentInfoForNoShowChargeByReservationId(reservationId);

            return files;
        }
        [WebMethod]
        public static GuestBillPaymentBO FillForm(int EditId)
        {
            GuestBillPaymentBO reservationBillPaymentBO = new GuestBillPaymentBO();
            GuestBillPaymentDA reservationBillPaymentDA = new GuestBillPaymentDA();

            reservationBillPaymentBO = reservationBillPaymentDA.GetGuestBillPaymentInfoById(EditId);

            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetCommonCurrencyInfoById(reservationBillPaymentBO.FieldId);

            reservationBillPaymentBO.CurrencyType = commonCurrencyBO.CurrencyType;

            return reservationBillPaymentBO;
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
    }
}