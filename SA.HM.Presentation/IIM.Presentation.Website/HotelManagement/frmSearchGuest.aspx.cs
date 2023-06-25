using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.Collections;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data;
using HotelManagement.Data.Payroll;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmSearchGuest : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isEntryPanelShowEnable = 1;
        protected int isTabVisible = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                string queryString = Request.QueryString["SearchText"];
                if (!string.IsNullOrEmpty(queryString))
                {
                    this.isEntryPanelShowEnable = -1;
                    this.SearchGuestInformation();
                }
                else
                {
                    this.isEntryPanelShowEnable = 1;
                }
                this.CheckPermission();
                this.LoadPayRoomInfo();
                this.LoadEmployeeInfo();
                this.LoadRegisteredGuestCompanyInfo();
                this.LoadBank();
                this.LoadCommonDropDownHiddenField();
            }

        }
        //************************ User Defined Function ********************//
        private void CheckPermission()
        {
            if (isSavePermission)
            {
                hfIsSavePermission.Value = "1";
            }
            else
            {
                hfIsSavePermission.Value = "0";
            }

            if (isDeletePermission)
            {
                hfIsDeletePermission.Value = "1";
            }
            else
            {
                hfIsDeletePermission.Value = "0";
            }
        }
        private void SearchGuestInformation()
        {
            isTabVisible = 0;
        }
        private void LoadPayRoomInfo()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            this.ddlRoomNumberId.DataSource = roomNumberDA.GetRoomNumberInfoByCondition(0, 0);
            this.ddlRoomNumberId.DataTextField = "RoomNumber";
            this.ddlRoomNumberId.DataValueField = "RoomId";
            this.ddlRoomNumberId.DataBind();

            ListItem itemRoom = new ListItem();
            itemRoom.Value = "0";
            itemRoom.Text = hmUtility.GetDropDownFirstValue();
            this.ddlRoomNumberId.Items.Insert(0, itemRoom);
        }
        private void LoadEmployeeInfo()
        {
            if (IsRestaurantIntegrateWithPayrollVal.Value == "1")
            {
                EmployeeDA entityDA = new EmployeeDA();
                this.ddlEmpId.DataSource = entityDA.GetEmployeeInfo();
                this.ddlEmpId.DataTextField = "DisplayName";
                this.ddlEmpId.DataValueField = "EmpId";
                this.ddlEmpId.DataBind();

                ListItem itemEmpId = new ListItem();
                itemEmpId.Value = "0";
                itemEmpId.Text = hmUtility.GetDropDownFirstValue();
                this.ddlEmpId.Items.Insert(0, itemEmpId);
            }
        }
        private void LoadRegisteredGuestCompanyInfo()
        {
            GuestCompanyDA companyDa = new GuestCompanyDA();
            this.ddlCompanyName.DataSource = companyDa.GetGuestCompanyInfo();
            this.ddlCompanyName.DataTextField = "CompanyName";
            this.ddlCompanyName.DataValueField = "NodeId";
            this.ddlCompanyName.DataBind();

            ListItem itemCompanyName = new ListItem();
            itemCompanyName.Value = "0";
            itemCompanyName.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCompanyName.Items.Insert(0, itemCompanyName);
        }
        private void LoadBank()
        {
            BankDA da = new BankDA();
            List<BankBO> files = da.GetBankInfo();
            this.ddlBankName.DataSource = files;
            this.ddlBankName.DataTextField = "BankName";
            this.ddlBankName.DataValueField = "BankId";
            this.ddlBankName.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBankName.Items.Insert(0, item);

            this.ddlBankId.DataSource = files;
            this.ddlBankId.DataTextField = "BankName";
            this.ddlBankId.DataValueField = "BankId";
            this.ddlBankId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBankId.Items.Insert(0, itemBank);
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }

        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static string SearchGuestAndLoadGridInformation(string companyName, string DateOfBirth, string EmailAddress, string FromDate, string ToDate, string GuestName, string MobileNumber, string NationalId, string PassportNumber, string RegistrationNumber, string RoomNumber)
        {
            HMUtility hmUtility = new HMUtility();
            HMCommonDA commonDA = new HMCommonDA();
            GuestInformationDA guestDA = new GuestInformationDA();
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            List<GuestInformationBO> distinctItems = new List<GuestInformationBO>();

            DateTime? dateOfBirth = null;
            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (!string.IsNullOrWhiteSpace(DateOfBirth))
                dateOfBirth = hmUtility.GetDateTimeFromString(DateOfBirth, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            if (!string.IsNullOrWhiteSpace(FromDate))
                fromDate = hmUtility.GetDateTimeFromString(FromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            if (!string.IsNullOrWhiteSpace(ToDate))
                toDate = hmUtility.GetDateTimeFromString(ToDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            list = guestDA.GetGuestInformationBySearchCriteria(GuestName, EmailAddress, MobileNumber, NationalId, PassportNumber, dateOfBirth, companyName, RoomNumber, fromDate, toDate, RegistrationNumber);
            distinctItems = list.GroupBy(x => x.GuestId).Select(y => y.First()).Distinct().ToList();
            return commonDA.GetHTMLGuestGridView(distinctItems);
        }
        [WebMethod]
        public static GridViewDataNPaging<GuestInformationBO, GridPaging> SearchGuestAndLoadGridInformationPaging(string companyName, string DateOfBirth, string EmailAddress, string FromDate, string ToDate, string GuestName, string MobileNumber, string NationalId, string PassportNumber, string RegistrationNumber, string ReservationNumber, string RoomNumber, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            HMCommonDA commonDA = new HMCommonDA();
            GuestInformationDA guestDA = new GuestInformationDA();
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            List<GuestInformationBO> distinctItems = new List<GuestInformationBO>();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            int totalRecords = 0;

            string startDate = string.Empty;
            string endDate = string.Empty;
            DateTime dateTimeNow = DateTime.Now;

            GridViewDataNPaging<GuestInformationBO, GridPaging> myGridData = new GridViewDataNPaging<GuestInformationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (!string.IsNullOrWhiteSpace(FromDate))
            {
                if (string.IsNullOrWhiteSpace(ToDate))
                {
                    ToDate = hmUtility.GetStringFromDateTime(dateTimeNow);
                }
            }

            if (!string.IsNullOrWhiteSpace(ToDate))
            {
                if (string.IsNullOrWhiteSpace(FromDate))
                {
                    FromDate = hmUtility.GetStringFromDateTime(dateTimeNow);
                }
            }

            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (!string.IsNullOrWhiteSpace(FromDate))
            {
                fromDate = hmUtility.GetDateTimeFromString(FromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(ToDate))
            {
                toDate = hmUtility.GetDateTimeFromString(ToDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            list = guestDA.GetGuestInformationBySearchCriteriaForPaging(GuestName, EmailAddress, MobileNumber, NationalId, PassportNumber, DateOfBirth, companyName, RoomNumber, fromDate, toDate, RegistrationNumber, ReservationNumber, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);
            distinctItems = list.GroupBy(x => x.GuestId).Select(y => y.First()).Distinct().ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static GuestInformationBO LoadDetailInformation(string GuestId)
        {
            HMCommonDA commonDA = new HMCommonDA();
            return commonDA.GetGuestDetailInformation(GuestId);
        }
        [WebMethod]
        public static string GetDocumentsByUserTypeAndUserId(string GuestId)
        {
            string UserType = "";
            int UserId = 0;
            List<DocumentsBO> docList = new List<DocumentsBO>();
            DocumentsDA docDA = new DocumentsDA();
            docList = docDA.GetDocumentsByUserTypeAndUserId("Guest", Int32.Parse(GuestId));
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            string strTable = "";
            strTable += "<div style='color: White; background-color: #44545E;width:750px;'>";
            int counter = 0;
            foreach (DocumentsBO dr in docList)
            {
                if (dr.Extention == ".jpg")
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:250px; height:250px; float:left;padding:30px'>";
                    strTable += "<a target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img id= style='width: 100px; height: 100px;' src='" + ImgSource + "'  alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
                else
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:100px; height:100px; float:left;padding:30px'>";
                    strTable += "<a target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img id= style='width: 100px; height: 100px;' src='" + dr.IconImage + "' alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
            }
            strTable += "</div>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }
            return strTable;
        }
        [WebMethod(EnableSession = true)]
        public static string GetGuestRegistrationHistoryGuestId(int GuestId)
        {
            int isDayClosedForTheCheckOutDate = 0;
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            List<RoomRegistrationBO> registrationList = new List<RoomRegistrationBO>();
            registrationList = registrationDA.GetGuestRegistrationHistoryByGuestId(GuestId);

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmSearchGuest.ToString());

            bool isSavePermission = objectPermissionBO.IsSavePermission;
            bool isDeletePermission = objectPermissionBO.IsDeletePermission;

            string strTable = "";
            strTable += "<table  width='100%' class='table table-bordered table-condensed table-responsive' id='TableGuestHistory'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";


            strTable += "<th align='left' scope='col'>Registration Number</th><th align='left' scope='col'>Arrival Date</th> <th align='left' scope='col'>Checkout Date</th><th align='left' scope='col'>Room Number</th><th align='center' scope='col'>Action</th></tr>";
            int counter = 0;
            foreach (RoomRegistrationBO dr in registrationList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:White;'>";
                }
                strTable += "<td align='left' style='width: 25%'>" + dr.RegistrationNumber + "</td>";
                strTable += "<td align='left' style='width: 15%'>" + dr.OriginalArriveDateDisplay + "</td>";
                if (dr.CheckOutDate != DateTime.MinValue)
                {
                    strTable += "<td align='left' style='width: 15%'>" + dr.CheckOutDateDisplay + "</td>";
                    strTable += "<td align='left' style='width: 20%'>" + dr.RoomNumber + "</td>";
                    strTable += "<td align='center' style='width: 25%; cursor:pointer'>";

                    string strZeroConvertionRate = "0";
                    strTable += "&nbsp;<img src='../Images/detailsInfo.png' data-placement='bottom' data-toggle='tooltip' title='Other Information' onClick='javascript:return PerformOtherInformationShow(" + dr.RegistrationId + ")' alt='Other Information' border='0' />";

                    strTable += "&nbsp;<img src='../Images/ReportDocument.png' data-placement='bottom' data-toggle='tooltip' title='Bill Preview (" + dr.LocalCurrencyHead + ")' onClick='javascript:return PerformGuestBillInfoShow(" + dr.RegistrationId + "," + strZeroConvertionRate + ")' alt='Guest Bill' border='0' />";

                    if (dr.ConversionRate > 0)
                    {
                        strTable += "&nbsp;<img src='../Images/ReportDocument.png' data-placement='bottom' data-toggle='tooltip' title='Bill Preview (USD)' onClick='javascript:return PerformGuestBillInfoShow(" + dr.RegistrationId + "," + dr.ConversionRate + ")' alt='Guest Bill' border='0' />";
                    }

                    if (dr.CheckOutDate != null)
                    {
                        HMCommonDA hmCoomnoDA = new HMCommonDA();
                        DayCloseBO dayCloseBO = new DayCloseBO();
                        dayCloseBO = hmCoomnoDA.GetHotelDayCloseInformation(dr.CheckOutDate);
                        if (dayCloseBO != null)
                        {
                            if (dayCloseBO.DayCloseId > 0)
                            {
                                isDayClosedForTheCheckOutDate = 1;
                            }
                        }
                    }

                    if (isDayClosedForTheCheckOutDate != 1)
                    {
                        if (isSavePermission)
                        {
                            //if (userInformationBO.UserInfoId == 1)
                            if (userInformationBO.UserGroupId == 1)
                            {
                                strTable += "&nbsp;<img src='../Images/edit.png' data-placement='bottom' data-toggle='tooltip' title='Payment Type Change' onClick=\"javascript:return PerformCheckedOutPaymentModification('" + dr.RegistrationId + "')\" alt='Payment Type Change' border='0' />";
                            }
                        }
                        else
                        {
                            strTable += "";
                        }
                        if (isDeletePermission)
                        {
                            if (!dr.IsRoomNumberCheckoutOrRegistationAfterCurrentGuestCheckOut)
                            {
                                strTable += "&nbsp;<img src='../Images/delete.png' data-placement='bottom' data-toggle='tooltip' title='Re Check-In' onClick=\"javascript:return PerformCancelCheckOut('" + dr.RegistrationId + "', '" + GuestId + "')\" alt='Cancel' border='0' />";
                            }
                        }
                        else
                        {
                            strTable += "";
                        }
                    }

                    strTable += "</td>";
                }
                else
                {
                    strTable += "<td align='left' style='width: 15%'>" + "Not Check Out Yet. " + "</td>";
                    strTable += "<td align='left' style='width: 20%'>" + dr.RoomNumber + "</td>";
                    strTable += "<td align='center' style='width: 25%;'></td>";
                }

                strTable += "</tr>";
            }

            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }
            return strTable;
        }
        [WebMethod]
        public static List<GuestBillPaymentBO> GetGuestBillPayment(string strRegistrationId)
        {
            int registrationId = !string.IsNullOrWhiteSpace(strRegistrationId) ? Convert.ToInt32(strRegistrationId) : 0;
            GuestBillPaymentDA paymentDa = new GuestBillPaymentDA();
            List<GuestBillPaymentBO> billPaymentList = new List<GuestBillPaymentBO>();
            if (registrationId > 0)
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomRegistrationBO roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(registrationId);
                billPaymentList = paymentDa.GetGuestBillPaymentInfoByRegistrationId("FrontOffice", registrationId);

                //Only Checked Out Day Payment Editable Only--------------
                RoomRegistrationBO billBO = new RoomRegistrationBO();
                RoomRegistrationDA billDA = new RoomRegistrationDA();
                billBO = billDA.GetGuestCheckedOutInfoByRegistrationId(registrationId);
                if (billBO.RegistrationId > 0)
                {
                    billPaymentList = billPaymentList.Where(x => x.PaymentDate.Date == billBO.CheckOutDate.Date).ToList();
                }

                foreach (GuestBillPaymentBO row in billPaymentList)
                {
                    row.CompanyId = roomRegistrationBO.CompanyId;
                    if (row.PaymentType == "Advance/ Payment")
                    {
                        row.PaymentType = "Payment";
                        row.TransactionType = "Payment";
                    }
                    else if (row.PaymentType == "Company")
                    {
                        row.PaymentType = "Payment";
                        row.TransactionType = "Payment";
                    }
                }
            }
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = billPaymentList;
            return billPaymentList;
        }
        [WebMethod(EnableSession = true)]
        public static string PerformDeleteGuestPaymentByWebMethod(string transactionType, int paymentId)
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

            return (transactionType + paymentId.ToString());
        }
        [WebMethod(EnableSession = true)]
        public static string PerformSaveGuestPaymentDetailsInformationByWebMethod(bool isEdit, string paymentDescription, string ddlPayMode, string ddlBankId, string txtReceiveLeadgerAmount, string ddlRegistrationId, string ddlCashPaymentAccountHead, string txtCardNumber, string ddlCardType, string txtExpireDate, string txtCardHolderName, string txtChecqueNumber, string ddlChecquePaymentAccountHeadId, string ddlCardPaymentAccountHeadId, string ddlCompanyPaymentAccountHead, string ddlPaidByRoomId, string RefundAccountHead, string ddlEmpId, string ddlEmployeePaymentAccountHead)
        {
            HMUtility hmUtility = new HMUtility();
            int dynamicDetailId = 0;
            int ddlPaidByRegistrationId = 0;

            List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

            GuestBillPaymentBO singleEntityBOEdit = guestPaymentDetailListForGrid.Where(x => x.PaymentMode == ddlPayMode).FirstOrDefault();

            if (guestPaymentDetailListForGrid != null)
            {
                dynamicDetailId = guestPaymentDetailListForGrid.Count + 25;
            }

            GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();

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
                guestBillPaymentBO.NodeId = !string.IsNullOrWhiteSpace(ddlCompanyPaymentAccountHead) ? Convert.ToInt32(ddlCompanyPaymentAccountHead) : 0;
                guestBillPaymentBO.PaymentType = ddlPayMode;
                guestBillPaymentBO.AccountsPostingHeadId = !string.IsNullOrWhiteSpace(ddlCompanyPaymentAccountHead) ? Convert.ToInt32(ddlCompanyPaymentAccountHead) : 0;
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlRegistrationId);
            }
            else if (ddlPayMode == "Employee")
            {
                guestBillPaymentBO.NodeId = !string.IsNullOrWhiteSpace(ddlEmployeePaymentAccountHead) ? Convert.ToInt32(ddlEmployeePaymentAccountHead) : 0;
                guestBillPaymentBO.PaymentType = ddlPayMode;
                guestBillPaymentBO.AccountsPostingHeadId = !string.IsNullOrWhiteSpace(ddlEmployeePaymentAccountHead) ? Convert.ToInt32(ddlEmployeePaymentAccountHead) : 0;
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
                //guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlPaidByRegistrationId);
                guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlRegistrationId);
            }
            else if (ddlPayMode == "Refund")
            {
                guestBillPaymentBO.RefundAccountHead = !string.IsNullOrWhiteSpace(RefundAccountHead) ? Convert.ToInt32(RefundAccountHead) : 0;
                guestBillPaymentBO.PaymentMode = "Refund";
                guestBillPaymentBO.CurrencyAmount = guestBillPaymentBO.CurrencyAmount * 1;
                guestBillPaymentBO.PaymentAmount = guestBillPaymentBO.PaymentAmount * 1;
                guestBillPaymentBO.PaymentType = "Refund";
                guestBillPaymentBO.AccountsPostingHeadId = !string.IsNullOrWhiteSpace(RefundAccountHead) ? Convert.ToInt32(RefundAccountHead) : 0;
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlRegistrationId);
            }
            else
            {
                if (ddlPayMode == "Cash")
                {
                    guestBillPaymentBO.NodeId = !string.IsNullOrWhiteSpace(ddlCashPaymentAccountHead) ? Convert.ToInt32(ddlCashPaymentAccountHead) : 0;
                    guestBillPaymentBO.AccountsPostingHeadId = !string.IsNullOrWhiteSpace(ddlCashPaymentAccountHead) ? Convert.ToInt32(ddlCashPaymentAccountHead) : 0;
                }
                else if (ddlPayMode == "Card")
                {
                    guestBillPaymentBO.NodeId = !string.IsNullOrWhiteSpace(ddlCardPaymentAccountHeadId) ? Convert.ToInt32(ddlCardPaymentAccountHeadId) : 0;
                    guestBillPaymentBO.AccountsPostingHeadId = !string.IsNullOrWhiteSpace(ddlCardPaymentAccountHeadId) ? Convert.ToInt32(ddlCardPaymentAccountHeadId) : 0;
                }
                else if (ddlPayMode == "Checque")
                {
                    guestBillPaymentBO.NodeId = !string.IsNullOrWhiteSpace(ddlChecquePaymentAccountHeadId) ? Convert.ToInt32(ddlChecquePaymentAccountHeadId) : 0;
                    guestBillPaymentBO.AccountsPostingHeadId = !string.IsNullOrWhiteSpace(ddlChecquePaymentAccountHeadId) ? Convert.ToInt32(ddlChecquePaymentAccountHeadId) : 0;
                }

                guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.BankId = !string.IsNullOrWhiteSpace(ddlBankId) ? Convert.ToInt32(ddlBankId) : 0;
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlRegistrationId);
                guestBillPaymentBO.PaymentType = "Advance";
            }

            guestBillPaymentBO.FieldId = 1; // Convert.ToInt32(ddlCurrency);
            guestBillPaymentBO.ConvertionRate = 1;
            guestBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            guestBillPaymentBO.PaymentAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            guestBillPaymentBO.ChecqueDate = DateTime.Now;
            guestBillPaymentBO.PaymentMode = ddlPayMode;
            guestBillPaymentBO.PaymentId = dynamicDetailId;
            guestBillPaymentBO.CardNumber = txtCardNumber;
            guestBillPaymentBO.CardType = ddlCardType;
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

            guestPaymentDetailListForGrid.Add(guestBillPaymentBO);
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;

            return dynamicDetailId.ToString() + "#" + paymentDescription;
        }
        [WebMethod(EnableSession = true)]
        public static string SetSessionValueForGuestBill(string RegistrationId, string ConvertionRate)
        {
            string RegistrationIdList = string.Empty;
            HMUtility hmUtility = new HMUtility();
            HttpContext.Current.Session["IsBillSplited"] = "0";
            HttpContext.Current.Session["GuestBillFromDate"] = hmUtility.GetFromDate();
            HttpContext.Current.Session["GuestBillToDate"] = hmUtility.GetToDate();

            GuestBillPaymentDA paymentDa = new GuestBillPaymentDA();
            List<GenerateGuestBillReportBO> guestBill = new List<GenerateGuestBillReportBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            guestBill = paymentDa.GetGenerateGuestBill(RegistrationId, "0", hmUtility.GetFromDate().ToString(), hmUtility.GetToDate().ToString(), userInformationBO.UserName);

            foreach (GenerateGuestBillReportBO row in guestBill)
            {
                RegistrationIdList = row.RegistrationId.ToString();
            }

            HttpContext.Current.Session["CheckOutRegistrationIdList"] = RegistrationIdList;

            InnboardWebUtility.BillPrintPreviewDynamicallyForReport(ConvertionRate, "0", "", "-1", "-1", "-1", "-1", "-1", "-1", "-1", hmUtility.GetFromDate().ToString(), hmUtility.GetToDate().ToString(), RegistrationIdList, RegistrationIdList);

            return RegistrationId;
        }
        [WebMethod(EnableSession = true)]
        public static string SetSessionValueForGuestBillForUsd(string RegistrationId, string ConvertionRate)
        {
            HMUtility hmUtility = new HMUtility();
            HttpContext.Current.Session["IsBillSplited"] = "0";
            HttpContext.Current.Session["GuestBillFromDate"] = hmUtility.GetFromDate();
            HttpContext.Current.Session["GuestBillToDate"] = hmUtility.GetToDate();
            HttpContext.Current.Session["CheckOutRegistrationIdList"] = RegistrationId;
            InnboardWebUtility.BillPrintPreviewDynamicallyForReport(ConvertionRate, "0", "", "-1", "-1", "-1", "-1", "-1", "-1", "-1", hmUtility.GetFromDate().ToString(), hmUtility.GetToDate().ToString(), RegistrationId, RegistrationId);
            return RegistrationId;
        }
        [WebMethod]
        public static ArrayList PerformCancelCheckOut(int registrationId, int guestId)
        {
            string isCanceled = "1";
            GuestHouseCheckOutDA da = new GuestHouseCheckOutDA();
            isCanceled = da.CancelGuestHouseCheckOutInfoByRegiId(registrationId) == true ? "1" : "0";

            ArrayList arr = new ArrayList();
            arr.Add(new { IsCanceled = isCanceled, GuestId = guestId });
            return arr;
        }
        [WebMethod(EnableSession = true)]
        public static string PerformPaymentPostingByWebMethod(string txtBillId, string hfDeletedPaymentForPaymentId, string hfDeletedPaymentForTransferedPaymentId, string hfDeletedPaymentForAchievementPaymentId)
        {
            string ReturnResult = string.Empty;
            try
            {
                HMUtility hmUtility = new HMUtility();
                DateTime restaurantBillDate = DateTime.Now;
                string transactionHead = string.Empty;
                HMCommonDA hmCommonDA = new HMCommonDA();
                CustomFieldBO customField = new CustomFieldBO();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                customField = hmCommonDA.GetCustomFieldByFieldName("GuestBillPayment");

                if (customField == null)
                {
                    //CommonHelper.AlertInfo(innboardMessage, "Please Contact With Administrator for Accounts Mapping.", AlertType.Warning);
                    //return;
                }
                else
                {
                    List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

                    RoomRegistrationDA restaurentBillDA = new RoomRegistrationDA();
                    int billID = 0;
                    string deletedPaymentIds = string.Empty;
                    string deletedTransferedPaymentIds = string.Empty;
                    string deletedAchievementPaymentIds = string.Empty;

                    guestPaymentDetailListForGrid = guestPaymentDetailListForGrid.Where(g => g.ServiceBillId == null).ToList();

                    //if (txtBillId.Value.Trim() != string.Empty)
                    if (txtBillId.Trim() != string.Empty)
                    {
                        billID = Convert.ToInt32(txtBillId.Trim());
                        RoomRegistrationBO billBO = new RoomRegistrationBO();
                        RoomRegistrationDA billDA = new RoomRegistrationDA();
                        billBO = billDA.GetGuestCheckedOutInfoByRegistrationId(billID);
                        if (billBO.RegistrationId > 0)
                        {
                            restaurantBillDate = billBO.CheckOutDate;
                            foreach (GuestBillPaymentBO row in guestPaymentDetailListForGrid)
                            {
                                row.CreatedBy = userInformationBO.UserInfoId;
                            }
                        }
                    }

                    if (hfDeletedPaymentForPaymentId.Trim() != "")
                    {
                        deletedPaymentIds = hfDeletedPaymentForPaymentId.Trim();
                    }

                    if (hfDeletedPaymentForTransferedPaymentId.Trim() != "")
                    {
                        deletedTransferedPaymentIds = hfDeletedPaymentForTransferedPaymentId.Trim();
                    }

                    if (hfDeletedPaymentForAchievementPaymentId.Trim() != "")
                    {
                        deletedAchievementPaymentIds = hfDeletedPaymentForAchievementPaymentId.Trim();
                    }

                    int success = restaurentBillDA.SaveUpdateGuestBillPymentForPaymentTypeChange(restaurantBillDate, guestPaymentDetailListForGrid, deletedPaymentIds, deletedTransferedPaymentIds, deletedAchievementPaymentIds, billID);

                    if (success > 0)
                    {
                        ReturnResult = "1";
                        //Boolean updateSuccess = restaurentBillDA.UpdateRestaurantBillRegistrationIdInfoForOtherRoomPayment(billID);
                    }
                    else
                    {
                        ReturnResult = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnResult = "0";

            }

            return ReturnResult;
        }

        [WebMethod]
        public static RoomRegistrationBO PerformOtherInformationByRegistrationId(int RegistrationId)
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();

            roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(RegistrationId);
            return roomRegistrationBO;
        }
        
    }
}