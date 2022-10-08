using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.HMCommon;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.Common
{
    public partial class InnboardWebUtility : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public static void BillPrintPreviewDynamicallyForReport(string currencyRate, string strIsBillSplited, string serviceType, string SelectdIndividualTransferedPaymentId, string SelectdPaymentId, string SelectdIndividualPaymentId, string SelectdIndividualServiceId, string SelectdIndividualRoomId, string SelectdServiceId, string SelectdRoomId, string strStartDate, string EndDate, string ddlRegistrationId, string txtSrcRegistrationIdList)
        {
            if (serviceType == "Preview")
            {
                serviceType = string.Empty;
            }

            string[] splitStr;
            splitStr = ddlRegistrationId.Split(',');
            if (splitStr.Length > 1)
            {
                System.Web.HttpContext.Current.Session["registrationIdSessionInfo"] = ddlRegistrationId;
                ddlRegistrationId = splitStr[0];
            }

            //Activity Log Process for Bill Preview...........
            HMUtility hmUtility = new HMUtility();
            long registrationId = !string.IsNullOrWhiteSpace(ddlRegistrationId) ? Convert.ToInt32(ddlRegistrationId) : 0;
            Boolean logStatus = hmUtility.CreateActivityLogEntity("Bill Preview", EntityTypeEnum.EntityType.RoomRegistration.ToString(), registrationId,
                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Is Bill Splited: " + strIsBillSplited);


            int frontOfficeInvoiceTemplate = 1;
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO frontOfficeInvoiceTemplateBO = new HMCommonSetupBO();
            frontOfficeInvoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("FrontOfficeInvoiceTemplate", "FrontOfficeInvoiceTemplate");
            if (frontOfficeInvoiceTemplateBO != null)
            {
                frontOfficeInvoiceTemplate = Convert.ToInt32(frontOfficeInvoiceTemplateBO.SetupValue);
            }

            System.Web.HttpContext.Current.Session["CheckOutMasterRegistrationId"] = ddlRegistrationId;


            if (frontOfficeInvoiceTemplate == 1)
            {
                //InvoiceTemplateOne
                BillPrintPreviewDynamicallyForReportDynamically(frontOfficeInvoiceTemplate, currencyRate, strIsBillSplited, serviceType, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdIndividualServiceId, SelectdIndividualRoomId, SelectdServiceId, SelectdRoomId, strStartDate, EndDate, ddlRegistrationId, txtSrcRegistrationIdList);
            }
            else if (frontOfficeInvoiceTemplate == 2)
            {
                // InvoiceTemplateTwo
                GetFrontOfficeGuestBillInfoForInvoiceDynamically(frontOfficeInvoiceTemplate, currencyRate, strIsBillSplited, serviceType, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdIndividualServiceId, SelectdIndividualRoomId, SelectdServiceId, SelectdRoomId, strStartDate, EndDate, ddlRegistrationId, txtSrcRegistrationIdList);
            }
            else if (frontOfficeInvoiceTemplate == 3)
            {
                // InvoiceTemplateThree
                GetFrontOfficeGuestBillInfoForInvoiceDynamically(frontOfficeInvoiceTemplate, currencyRate, strIsBillSplited, serviceType, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdIndividualServiceId, SelectdIndividualRoomId, SelectdServiceId, SelectdRoomId, strStartDate, EndDate, ddlRegistrationId, txtSrcRegistrationIdList);
            }
            else if (frontOfficeInvoiceTemplate == 4)
            {
                // InvoiceTemplateThree
                GetFrontOfficeGuestBillInfoForInvoiceDynamically(frontOfficeInvoiceTemplate, currencyRate, strIsBillSplited, serviceType, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdIndividualServiceId, SelectdIndividualRoomId, SelectdServiceId, SelectdRoomId, strStartDate, EndDate, ddlRegistrationId, txtSrcRegistrationIdList);
            }
        }
        private static void GetFrontOfficeGuestBillInfoForInvoiceDynamically(int frontOfficeInvoiceTemplate, string currencyRate, string strIsBillSplited, string serviceType, string SelectdIndividualTransferedPaymentId, string SelectdPaymentId, string SelectdIndividualPaymentId, string SelectdIndividualServiceId, string SelectdIndividualRoomId, string SelectdServiceId, string SelectdRoomId, string strStartDate, string EndDate, string ddlRegistrationId, string txtSrcRegistrationIdList)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            //string StartDate = hmUtility.GetStringFromDateTime(hmUtility.GetDateTimeFromString(strStartDate, userInformationBO.ServerDateFormat).AddYears(-1));
            string StartDate = hmUtility.GetStringFromDateTime(hmUtility.GetDateTimeFromString(strStartDate, userInformationBO.ServerDateFormat));

            HttpContext.Current.Session["BillPreviewCurrencyRateInformation"] = currencyRate;
            HttpContext.Current.Session["ReportGuestBillInfoDataSource"] = null;

            string registrationIdList = string.Empty;
            string transferedRegistrationIdList = string.Empty;
            if (ddlRegistrationId != "")
            {
                if (!string.IsNullOrWhiteSpace(txtSrcRegistrationIdList))
                {
                    registrationIdList = txtSrcRegistrationIdList;
                    transferedRegistrationIdList = txtSrcRegistrationIdList;
                }
                else
                {
                    registrationIdList = ddlRegistrationId.ToString();
                    transferedRegistrationIdList = ddlRegistrationId.ToString();
                }
            }

            HMCommonDA hmCommonDA = new HMCommonDA();
            registrationIdList = hmCommonDA.GetRegistrationIdList(registrationIdList);

            int IsBillSplited = !string.IsNullOrWhiteSpace(strIsBillSplited) ? Convert.ToInt32(strIsBillSplited) : 0;

            GuestHouseCheckOutDA billEntityDA = new GuestHouseCheckOutDA();
            List<GuestServiceBillApprovedBO> reportDataSourceList = new List<GuestServiceBillApprovedBO>();

            #region Service Information
            //// Service Information -------------------------------------------------------------------------------------------------||
            string strServiceStringParamer = string.Empty;
            List<GuestServiceBillApprovedBO> entityServiceListBOList = new List<GuestServiceBillApprovedBO>();
            entityServiceListBOList = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "GuestService", registrationIdList, strServiceStringParamer, IsBillSplited, 0);

            string selectedServiceIdList = string.Empty;
            selectedServiceIdList = SelectdServiceId;

            string selectedIndividualServiceIdList = string.Empty;
            selectedIndividualServiceIdList = SelectdIndividualServiceId;

            if (IsBillSplited == 1)
            {
                List<string> serviceIds = new List<string>();
                selectedServiceIdList = !string.IsNullOrWhiteSpace(selectedServiceIdList) ? selectedServiceIdList : "-1";
                selectedIndividualServiceIdList = !string.IsNullOrWhiteSpace(selectedIndividualServiceIdList) ? selectedIndividualServiceIdList : "-1";

                if (serviceType == "IndividualService")
                {
                    serviceIds = selectedIndividualServiceIdList.Split(',').ToList();

                    var v = from s in serviceIds select s;

                    var entityServiceListBO = from p in entityServiceListBOList
                                              where (p.ServiceDate.Date >= hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date
                                                        && p.ServiceDate.Date <= hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date) &&
                                              v.Contains(p.ApprovedId.ToString()) &&
                                              p.IsPaidService == 0
                                              select p;

                    reportDataSourceList.AddRange(entityServiceListBO);
                }
                else
                {
                    serviceIds = selectedServiceIdList.Split(',').ToList();

                    var v = from s in serviceIds select s;

                    var entityServiceListBO = from p in entityServiceListBOList
                                              where (p.ServiceDate.Date >= hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date
                                                        && p.ServiceDate.Date <= hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date) &&
                                              v.Contains(p.ServiceId.ToString()) &&
                                              p.IsPaidService == 0
                                              select p;

                    reportDataSourceList.AddRange(entityServiceListBO);
                }
            }
            else
            {
                var entityServiceListBO = from p in entityServiceListBOList
                                          where (p.ServiceDate.Date >= hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date
                                                    && p.ServiceDate.Date <= hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date) &&
                                              p.IsPaidService == 0
                                          select p;
                reportDataSourceList.AddRange(entityServiceListBO);
            }

            #endregion
            #region Bill Payment when Paid Service Achive Information
            //// Bill Payment when Paid Service Achive Information-------------------------------------------------------------------------------------------------||
            string strServiceStringParamerBillPaymentWhenPaidServiceAchive = string.Empty;
            List<GuestServiceBillApprovedBO> entityServiceListBOBillPaymentWhenPaidServiceAchiveList = new List<GuestServiceBillApprovedBO>();
            entityServiceListBOBillPaymentWhenPaidServiceAchiveList = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "BillPaymentWhenPaidServiceAchive", registrationIdList, strServiceStringParamerBillPaymentWhenPaidServiceAchive, IsBillSplited, 0);

            string selectedServiceIdListBillPaymentWhenPaidServiceAchive = string.Empty;
            selectedServiceIdListBillPaymentWhenPaidServiceAchive = SelectdServiceId;

            string selectedIndividualServiceIdListBillPaymentWhenPaidServiceAchive = string.Empty;
            selectedIndividualServiceIdListBillPaymentWhenPaidServiceAchive = SelectdIndividualServiceId;

            if (IsBillSplited == 1)
            {
                List<string> serviceIds = new List<string>();
                selectedServiceIdListBillPaymentWhenPaidServiceAchive = !string.IsNullOrWhiteSpace(selectedServiceIdListBillPaymentWhenPaidServiceAchive) ? selectedServiceIdListBillPaymentWhenPaidServiceAchive : "-1";
                selectedIndividualServiceIdListBillPaymentWhenPaidServiceAchive = !string.IsNullOrWhiteSpace(selectedIndividualServiceIdListBillPaymentWhenPaidServiceAchive) ? selectedIndividualServiceIdListBillPaymentWhenPaidServiceAchive : "-1";

                if (serviceType == "IndividualService")
                {
                    serviceIds = selectedIndividualServiceIdListBillPaymentWhenPaidServiceAchive.Split(',').ToList();

                    var v = from s in serviceIds select s;

                    var entityServiceListBOBillPaymentWhenPaidServiceAchive = from p in entityServiceListBOBillPaymentWhenPaidServiceAchiveList
                                                                              where (p.ServiceDate.Date >= hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date
                                                                                        && p.ServiceDate.Date <= hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date) &&
                                                                              v.Contains(p.ApprovedId.ToString()) &&
                                                                              p.IsPaidService == 0
                                                                              select p;

                    reportDataSourceList.AddRange(entityServiceListBOBillPaymentWhenPaidServiceAchive);
                }
                else
                {
                    serviceIds = selectedServiceIdListBillPaymentWhenPaidServiceAchive.Split(',').ToList();

                    var v = from s in serviceIds select s;

                    var entityServiceListBOBillPaymentWhenPaidServiceAchive = from p in entityServiceListBOBillPaymentWhenPaidServiceAchiveList
                                                                              where (p.ServiceDate.Date >= hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date
                                                                                        && p.ServiceDate.Date <= hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date) &&
                                                                              v.Contains(p.ServiceId.ToString()) &&
                                                                              p.IsPaidService == 0
                                                                              select p;

                    reportDataSourceList.AddRange(entityServiceListBOBillPaymentWhenPaidServiceAchive);
                }
            }
            else
            {
                var entityServiceListBOBillPaymentWhenPaidServiceAchive = from p in entityServiceListBOBillPaymentWhenPaidServiceAchiveList
                                                                          where (p.ServiceDate.Date >= hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date
                                                                                    && p.ServiceDate.Date <= hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date) &&
                                                                              p.IsPaidService == 0
                                                                          select p;
                reportDataSourceList.AddRange(entityServiceListBOBillPaymentWhenPaidServiceAchive);
            }

            #endregion
            #region Room Information
            //// Room Information -------------------------------------------------------------------------------------------------||
            string strRoomStringParamer = string.Empty;
            List<GuestServiceBillApprovedBO> entityRoomListBOList = new List<GuestServiceBillApprovedBO>();
            entityRoomListBOList = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "RoomService", registrationIdList, strRoomStringParamer, IsBillSplited, 0);


            string selectedRoomIdList = string.Empty;
            selectedRoomIdList = SelectdRoomId;

            string selectedIndividualRoomIdList = string.Empty;
            selectedIndividualRoomIdList = SelectdIndividualRoomId;

            if (IsBillSplited == 1)
            {
                List<string> serviceIds = new List<string>();
                selectedRoomIdList = !string.IsNullOrWhiteSpace(selectedRoomIdList) ? selectedRoomIdList : "-1";
                selectedIndividualRoomIdList = !string.IsNullOrWhiteSpace(selectedIndividualRoomIdList) ? selectedIndividualRoomIdList : "-1";

                if (serviceType == "IndividualService")
                {
                    serviceIds = selectedIndividualRoomIdList.Split(',').ToList();

                    var v = from s in serviceIds select s;

                    var entityRoomListBO = from p in entityRoomListBOList
                                           where (p.ServiceDate.Date >= hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date
                                                     && p.ServiceDate.Date <= hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date) &&
                                           v.Contains(p.ApprovedId.ToString()) &&
                                           p.IsPaidService == 0
                                           select p;

                    reportDataSourceList.AddRange(entityRoomListBO);
                }
                else
                {
                    serviceIds = selectedRoomIdList.Split(',').ToList();

                    var v = from s in serviceIds select s;

                    var entityRoomListBO = from p in entityRoomListBOList
                                           where (p.ServiceDate.Date >= hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date
                                                     && p.ServiceDate.Date <= hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date) &&
                                           v.Contains(p.ServiceId.ToString()) &&
                                           p.IsPaidService == 0
                                           select p;

                    reportDataSourceList.AddRange(entityRoomListBO);
                }
            }
            else
            {
                var entityRoomListBO = from p in entityRoomListBOList
                                       where (p.ServiceDate.Date >= hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date
                                                 && p.ServiceDate.Date <= hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date) &&
                                           p.IsPaidService == 0
                                       select p;
                reportDataSourceList.AddRange(entityRoomListBO);
            }

            #endregion
            #region Own Payment Information
            // Own Payment Information -------------------------------------------------------------------------------------------------||
            string strOwnPaymentStringParamer = string.Empty;

            string selectedOwnPaymentIdList = string.Empty;
            selectedOwnPaymentIdList = SelectdPaymentId;

            string selectedIndividualOwnPaymentIdList = string.Empty;
            selectedIndividualOwnPaymentIdList = SelectdIndividualPaymentId;

            List<GuestServiceBillApprovedBO> entityOwnPaymentListBO = new List<GuestServiceBillApprovedBO>();

            if (IsBillSplited == 1)
            {
                List<string> serviceIds = new List<string>();
                selectedOwnPaymentIdList = !string.IsNullOrWhiteSpace(selectedOwnPaymentIdList) ? selectedOwnPaymentIdList : "-1";
                selectedIndividualOwnPaymentIdList = !string.IsNullOrWhiteSpace(selectedIndividualOwnPaymentIdList) ? selectedIndividualOwnPaymentIdList : "-1";

                if (serviceType == "IndividualService")
                {
                    entityOwnPaymentListBO = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "GuestPayment", registrationIdList, strOwnPaymentStringParamer, IsBillSplited, 0);

                    serviceIds = selectedIndividualOwnPaymentIdList.Split(',').ToList();

                    var v = from s in serviceIds select s;

                    var entityOwnPaymentBO = from p in entityOwnPaymentListBO
                                             where (p.ServiceDate.Date >= hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date
                                                       && p.ServiceDate.Date <= hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date) &&
                                             v.Contains(p.ApprovedId.ToString()) &&
                                             p.IsPaidService == 0
                                             select p;

                    reportDataSourceList.AddRange(entityOwnPaymentBO);
                }
                else
                {
                    selectedOwnPaymentIdList = selectedOwnPaymentIdList.Replace("5001", "'Advance'");
                    selectedOwnPaymentIdList = selectedOwnPaymentIdList.Replace("5001", "'Payment'");
                    selectedOwnPaymentIdList = selectedOwnPaymentIdList.Replace("5002", "'PaidOut'");
                    selectedOwnPaymentIdList = selectedOwnPaymentIdList.Replace("-1", "'maMun'");

                    entityOwnPaymentListBO = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "GroupGuestPayment", registrationIdList, strOwnPaymentStringParamer, IsBillSplited, 0);

                    selectedOwnPaymentIdList = selectedOwnPaymentIdList.Replace("'Advance'", "Advance");
                    selectedOwnPaymentIdList = selectedOwnPaymentIdList.Replace("'Payment'", "Payment");
                    selectedOwnPaymentIdList = selectedOwnPaymentIdList.Replace("'PaidOut'", "PaidOut");
                    selectedOwnPaymentIdList = selectedOwnPaymentIdList.Replace("'maMun'", "maMun");

                    serviceIds = selectedOwnPaymentIdList.Split(',').ToList();

                    var v = from s in serviceIds select s;

                    var entityOwnPaymentBO = (from p in entityOwnPaymentListBO
                                              where (p.ServiceDate.Date >= hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date
                                                        && p.ServiceDate.Date <= hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date) &&
                                              v.Contains(p.PaymentType.ToString()) &&
                                              p.IsPaidService == 0
                                              select p).ToList();

                    reportDataSourceList.AddRange(entityOwnPaymentBO);
                }
            }
            else
            {
                entityOwnPaymentListBO = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "GuestPayment", registrationIdList, strOwnPaymentStringParamer, IsBillSplited, 0);

                var entityOwnPaymentBO = from p in entityOwnPaymentListBO
                                         where (p.ServiceDate.Date >= hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date
                                                   && p.ServiceDate.Date <= hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date) &&
                                             p.IsPaidService == 0
                                         select p;
                reportDataSourceList.AddRange(entityOwnPaymentBO);
            }

            #endregion
            #region Bill Transfer Information
            // Bill Transfer Information -------------------------------------------------------------------------------------------------||

            string[] splitStr;
            splitStr = registrationIdList.Split(',');

            //if (splitStr.Length == 1)
            if (splitStr.Length >= 1)
            {
                string strBillTransferStringParamer = string.Empty;
                List<GuestServiceBillApprovedBO> entityBillTransferListBO = new List<GuestServiceBillApprovedBO>();
                //entityBillTransferListBO = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "BillTransfer", transferedRegistrationIdList, strBillTransferStringParamer, IsBillSplited, 0);
                entityBillTransferListBO = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "BillTransfer", ddlRegistrationId, strBillTransferStringParamer, IsBillSplited, 0);
                var entityBillTransferBO = from p in entityBillTransferListBO
                                           where (p.ServiceDate.Date >= hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date
                                                     && p.ServiceDate.Date <= hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date) &&
                                               p.IsPaidService == 0
                                           select p;
                reportDataSourceList.AddRange(entityBillTransferBO);
            }
            #endregion
            #region Others Room Payment Information
            //// Others Room Payment Information -------------------------------------------------------------------------------------------------||
            List<GuestServiceBillApprovedBO> entityPaymentTransferedListBO = new List<GuestServiceBillApprovedBO>();
            if (!string.IsNullOrWhiteSpace(SelectdIndividualTransferedPaymentId))
            {
                if (Convert.ToInt32(SelectdIndividualTransferedPaymentId) > 0)
                {
                    //// Others Room Payment Information -------------------------------------------------------------------------------------------------||
                    string strPaymentTransferedStringParamer = string.Empty;

                    List<string> serviceIds = new List<string>();
                    serviceIds = SelectdIndividualTransferedPaymentId.Split(',').ToList();

                    var v = from s in serviceIds select s;

                    entityPaymentTransferedListBO = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "PaymentTransfered", registrationIdList, strPaymentTransferedStringParamer, IsBillSplited, 0);

                    var entityPaymentTransferedBO = from p in entityPaymentTransferedListBO
                                                    where (p.ServiceDate.Date >= hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date
                                                              && p.ServiceDate.Date <= hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date) &&
                                                              v.Contains(p.ApprovedId.ToString()) &&
                                                              p.IsPaidService == 0
                                                    select p;
                    reportDataSourceList.AddRange(entityPaymentTransferedBO);
                }
            }

            //List<GuestServiceBillApprovedBO> SortedDataSourceList = reportDataSourceList.OrderBy(a => a.ServiceDate).ThenBy(a => a.GuestService).ToList();
            List<GuestServiceBillApprovedBO> SortedDataSourceList = reportDataSourceList.OrderBy(a => a.CreatedDate).ThenBy(a => a.GuestService).ToList();

            //// Distinct Related Work -------------------------
            List<GuestServiceBillApprovedBO> distinctDataSourceList = SortedDataSourceList.GroupBy(p => new { p.ApprovedId, p.ServiceId, p.ServiceDate, p.Reference, p.GuestService, p.ServiceRate }).Select(g => g.First()).ToList();
            decimal currencyRateAmount = !string.IsNullOrWhiteSpace(currencyRate) ? Convert.ToDecimal(currencyRate) : 0;
            if (currencyRateAmount > 0)
            {
                Boolean isUsdInvoice = false;
                foreach (GuestServiceBillApprovedBO row in distinctDataSourceList)
                {
                    row.DiscountAmount = row.DiscountAmount / row.CurrencyExchangeRate;
                    row.BillAmount = row.BillAmount / row.CurrencyExchangeRate;
                    row.PaidAmount = row.PaidAmount / row.CurrencyExchangeRate;

                    row.ServiceCharge = row.ServiceCharge / row.CurrencyExchangeRate;
                    row.CitySDCharge = row.CitySDCharge / row.CurrencyExchangeRate;
                    row.VatAmount = row.VatAmount / row.CurrencyExchangeRate;
                    row.AdditionalCharge = row.AdditionalCharge / row.CurrencyExchangeRate;
                    row.ServiceRate = row.ServiceRate / row.CurrencyExchangeRate;

                    row.RoomRate = row.RoomRate / row.CurrencyExchangeRate;

                    row.AdvanceAmount = row.AdvanceAmount / row.CurrencyExchangeRate;
                    row.AdvancePayment = row.AdvancePayment / row.CurrencyExchangeRate;

                    row.UnitPrice = row.UnitPrice / row.CurrencyExchangeRate;
                    row.TotalRoomCharge = row.TotalRoomCharge / row.CurrencyExchangeRate;

                    if (!isUsdInvoice)
                    {
                        isUsdInvoice = true;
                        row.InvoiceCurrencyId = 2;
                    }

                    //row.BillAmount = row.BillAmount / currencyRateAmount;
                    //row.PaidAmount = row.PaidAmount / currencyRateAmount;

                    //row.ServiceCharge = row.ServiceCharge / currencyRateAmount;
                    //row.VatAmount = row.VatAmount / currencyRateAmount;
                    //row.ServiceRate = row.ServiceRate / currencyRateAmount;
                    //if (!isUsdInvoice)
                    //{
                    //    isUsdInvoice = true;
                    //    row.InvoiceCurrencyId = 2;
                    //}
                }
            }

            HttpContext.Current.Session["ReportGuestBillInfoDataSource"] = distinctDataSourceList.OrderBy(x => x.CreatedDate).ToList();

            #endregion
            #region Group Check Out
            ////////------------Group Check Out --------------------------------------------------------------
            ////if (entityOwnPaymentListBO.Count == 0 && entityBillTransferListBO.Count == 0 && entityOthersRoomPaymentListBO.Count == 0 && entityPaymentTransferedListBO.Count == 0)
            ////{
            ////    decimal totalBill = 0;
            ////    foreach (GuestServiceBillApprovedBO row in distinctDataSourceList)
            ////    {
            ////        totalBill = totalBill + row.BillAmount;
            ////    }


            ////    List<GuestServiceBillApprovedBO> entityGroupPaymentListBO = new List<GuestServiceBillApprovedBO>();

            ////    registrationIdList = registrationIdList + "~" + totalBill.ToString();

            ////    string strGroupPaymentStringParamer = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + hmUtility.GetDateTimeFromString(StartDate) + "') AND dbo.FnDate('" + hmUtility.GetDateTimeFromString(EndDate) + "'))";
            ////    entityGroupPaymentListBO = billEntityDA.GetGuestServiceBillInfo("GuestGroupPaymentForIndividualRoom", registrationIdList, strOwnPaymentStringParamer, IsBillSplited, 0);


            ////    reportDataSourceList.AddRange(entityGroupPaymentListBO);
            ////}


            ////HttpContext.Current.Session["ReportGuestBillInfoDataSource"] = distinctDataSourceList;

            #endregion
        }
        private static void BillPrintPreviewDynamicallyForReportDynamically(int frontOfficeInvoiceTemplate, string currencyRate, string strIsBillSplited, string serviceType, string SelectdIndividualTransferedPaymentId, string SelectdPaymentId, string SelectdIndividualPaymentId, string SelectdIndividualServiceId, string SelectdIndividualRoomId, string SelectdServiceId, string SelectdRoomId, string strStartDate, string EndDate, string ddlRegistrationId, string txtSrcRegistrationIdList)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            //string StartDate = hmUtility.GetStringFromDateTime(hmUtility.GetDateTimeFromString(strStartDate, userInformationBO.ServerDateFormat).AddYears(-1));
            string StartDate = hmUtility.GetStringFromDateTime(hmUtility.GetDateTimeFromString(strStartDate, userInformationBO.ServerDateFormat));

            HttpContext.Current.Session["BillPreviewCurrencyRateInformation"] = currencyRate;
            HttpContext.Current.Session["ReportGuestBillInfoDataSource"] = null;

            string registrationIdList = string.Empty;
            string transferedRegistrationIdList = string.Empty;
            if (ddlRegistrationId != "")
            {
                if (!string.IsNullOrWhiteSpace(txtSrcRegistrationIdList))
                {
                    registrationIdList = txtSrcRegistrationIdList;
                    transferedRegistrationIdList = txtSrcRegistrationIdList;
                }
                else
                {
                    registrationIdList = ddlRegistrationId.ToString();
                    transferedRegistrationIdList = ddlRegistrationId.ToString();
                }
            }

            HMCommonDA hmCommonDA = new HMCommonDA();
            registrationIdList = hmCommonDA.GetRegistrationIdList(registrationIdList);

            int IsBillSplited = !string.IsNullOrWhiteSpace(strIsBillSplited) ? Convert.ToInt32(strIsBillSplited) : 0;

            GuestHouseCheckOutDA billEntityDA = new GuestHouseCheckOutDA();
            List<GuestServiceBillApprovedBO> reportDataSourceList = new List<GuestServiceBillApprovedBO>();

            //// Service Information -------------------------------------------------------------------------------------------------||
            string strServiceStringParamer = string.Empty;

            string selectedServiceIdList = string.Empty;
            selectedServiceIdList = SelectdServiceId;

            string selectedIndividualServiceIdList = string.Empty;
            selectedIndividualServiceIdList = SelectdIndividualServiceId;

            if (IsBillSplited == 1)
            {
                selectedServiceIdList = !string.IsNullOrWhiteSpace(selectedServiceIdList) ? selectedServiceIdList : "-1";
                selectedIndividualServiceIdList = !string.IsNullOrWhiteSpace(selectedIndividualServiceIdList) ? selectedIndividualServiceIdList : "-1";

                if (serviceType == "IndividualService")
                {
                    strServiceStringParamer = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "') AND dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "')) AND ApprovedId IN(" + selectedIndividualServiceIdList + ")";
                }
                else
                {
                    strServiceStringParamer = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "') AND dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "')) AND ServiceId IN(" + selectedServiceIdList + ")";
                }
            }
            else
            {
                strServiceStringParamer = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "') AND dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "'))";
            }

            strServiceStringParamer = strServiceStringParamer + " AND ISNULL(gsba.IsPaidService,0) = 0";
            List<GuestServiceBillApprovedBO> entityServiceListBO = new List<GuestServiceBillApprovedBO>();
            entityServiceListBO = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "GuestService", registrationIdList, strServiceStringParamer, IsBillSplited, 0);
            reportDataSourceList.AddRange(entityServiceListBO);


            ////// Bill Payment when Paid Service Achive Information-------------------------------------------------------------------------------------------------||
            //string strServiceStringParamerBillPaymentWhenPaidServiceAchive = string.Empty;

            //string selectedServiceIdListBillPaymentWhenPaidServiceAchive = string.Empty;
            //selectedServiceIdListBillPaymentWhenPaidServiceAchive = SelectdServiceId;

            //string selectedIndividualServiceIdListBillPaymentWhenPaidServiceAchive = string.Empty;
            //selectedIndividualServiceIdListBillPaymentWhenPaidServiceAchive = SelectdIndividualServiceId;

            //if (IsBillSplited == 1)
            //{
            //    selectedServiceIdListBillPaymentWhenPaidServiceAchive = !string.IsNullOrWhiteSpace(selectedServiceIdListBillPaymentWhenPaidServiceAchive) ? selectedServiceIdListBillPaymentWhenPaidServiceAchive : "-1";
            //    selectedIndividualServiceIdListBillPaymentWhenPaidServiceAchive = !string.IsNullOrWhiteSpace(selectedIndividualServiceIdListBillPaymentWhenPaidServiceAchive) ? selectedIndividualServiceIdListBillPaymentWhenPaidServiceAchive : "-1";

            //    if (serviceType == "IndividualService")
            //    {
            //        strServiceStringParamerBillPaymentWhenPaidServiceAchive = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "') AND dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "')) AND ApprovedId IN(" + selectedIndividualServiceIdListBillPaymentWhenPaidServiceAchive + ")";
            //    }
            //    else
            //    {
            //        strServiceStringParamerBillPaymentWhenPaidServiceAchive = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "') AND dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "')) AND ServiceId IN(" + selectedServiceIdListBillPaymentWhenPaidServiceAchive + ")";
            //    }
            //}
            //else
            //{
            //    strServiceStringParamerBillPaymentWhenPaidServiceAchive = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "') AND dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "'))";
            //}

            //strServiceStringParamerBillPaymentWhenPaidServiceAchive = strServiceStringParamerBillPaymentWhenPaidServiceAchive + " AND ISNULL(gsba.IsPaidService,0) = 0";
            //List<GuestServiceBillApprovedBO> entityServiceListBOBillPaymentWhenPaidServiceAchive = new List<GuestServiceBillApprovedBO>();
            //entityServiceListBOBillPaymentWhenPaidServiceAchive = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "BillPaymentWhenPaidServiceAchive", registrationIdList, strServiceStringParamerBillPaymentWhenPaidServiceAchive, IsBillSplited, 0);
            //reportDataSourceList.AddRange(entityServiceListBOBillPaymentWhenPaidServiceAchive);


            //// Room Information -------------------------------------------------------------------------------------------------||
            string strRoomStringParamer = string.Empty;

            string selectedRoomIdList = string.Empty;
            selectedRoomIdList = SelectdRoomId;

            string selectedIndividualRoomIdList = string.Empty;
            selectedIndividualRoomIdList = SelectdIndividualRoomId;

            if (IsBillSplited == 1)
            {
                selectedRoomIdList = !string.IsNullOrWhiteSpace(selectedRoomIdList) ? selectedRoomIdList : "-1";
                selectedIndividualRoomIdList = !string.IsNullOrWhiteSpace(selectedIndividualRoomIdList) ? selectedIndividualRoomIdList : "-1";

                if (serviceType == "IndividualService")
                {
                    strRoomStringParamer = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "') AND dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "')) AND ApprovedId IN(" + selectedIndividualRoomIdList + ")";
                }
                else
                {
                    strRoomStringParamer = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "') AND dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "')) AND ServiceId IN(" + selectedRoomIdList + ")";
                }
            }
            else
            {
                strRoomStringParamer = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "') AND dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "'))";
            }

            List<GuestServiceBillApprovedBO> entityRoomListBO = new List<GuestServiceBillApprovedBO>();
            entityRoomListBO = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "RoomService", registrationIdList, strRoomStringParamer, IsBillSplited, 0);
            reportDataSourceList.AddRange(entityRoomListBO);

            // Own Payment Information -------------------------------------------------------------------------------------------------||
            string strOwnPaymentStringParamer = string.Empty;

            string selectedOwnPaymentIdList = string.Empty;
            selectedOwnPaymentIdList = SelectdPaymentId;

            string selectedIndividualOwnPaymentIdList = string.Empty;
            selectedIndividualOwnPaymentIdList = SelectdIndividualPaymentId;

            List<GuestServiceBillApprovedBO> entityOwnPaymentListBO = new List<GuestServiceBillApprovedBO>();

            if (IsBillSplited == 1)
            {
                selectedOwnPaymentIdList = !string.IsNullOrWhiteSpace(selectedOwnPaymentIdList) ? selectedOwnPaymentIdList : "-1";
                selectedIndividualOwnPaymentIdList = !string.IsNullOrWhiteSpace(selectedIndividualOwnPaymentIdList) ? selectedIndividualOwnPaymentIdList : "-1";

                if (serviceType == "IndividualService")
                {
                    strOwnPaymentStringParamer = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "') AND dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "')) AND ApprovedId IN(" + selectedIndividualOwnPaymentIdList + ")";
                    entityOwnPaymentListBO = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "GuestPayment", registrationIdList, strOwnPaymentStringParamer, IsBillSplited, 0);
                }
                else
                {
                    selectedOwnPaymentIdList = selectedOwnPaymentIdList.Replace("5001", "'Advance'");
                    selectedOwnPaymentIdList = selectedOwnPaymentIdList.Replace("5001", "'Payment'");
                    selectedOwnPaymentIdList = selectedOwnPaymentIdList.Replace("5002", "'PaidOut'");
                    selectedOwnPaymentIdList = selectedOwnPaymentIdList.Replace("-1", "'maMun'");

                    strOwnPaymentStringParamer = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "') AND dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "')) AND PaymentType IN(" + selectedOwnPaymentIdList + ")";
                    entityOwnPaymentListBO = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "GroupGuestPayment", registrationIdList, strOwnPaymentStringParamer, IsBillSplited, 0);
                }
            }
            else
            {
                strOwnPaymentStringParamer = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "') AND dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "'))";
                entityOwnPaymentListBO = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "GuestPayment", registrationIdList, strOwnPaymentStringParamer, IsBillSplited, 0);
            }

            reportDataSourceList.AddRange(entityOwnPaymentListBO);

            // Bill Transfer Information -------------------------------------------------------------------------------------------------||
            string strBillTransferStringParamer = string.Empty;
            strBillTransferStringParamer = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "') AND dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "'))";

            List<GuestServiceBillApprovedBO> entityBillTransferListBO = new List<GuestServiceBillApprovedBO>();
            //entityBillTransferListBO = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "BillTransfer", transferedRegistrationIdList, strBillTransferStringParamer, IsBillSplited, 0);

            entityBillTransferListBO = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "BillTransfer", ddlRegistrationId, strBillTransferStringParamer, IsBillSplited, 0);
            reportDataSourceList.AddRange(entityBillTransferListBO);

            List<GuestServiceBillApprovedBO> entityPaymentTransferedListBO = new List<GuestServiceBillApprovedBO>();
            if (!string.IsNullOrWhiteSpace(SelectdIndividualTransferedPaymentId))
            {
                if (Convert.ToInt32(SelectdIndividualTransferedPaymentId) > 0)
                {
                    //// Others Room Payment Information -------------------------------------------------------------------------------------------------||
                    string strPaymentTransferedStringParamer = string.Empty;
                    strPaymentTransferedStringParamer = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "') AND dbo.FnDate('" + hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) + "')) AND ApprovedId IN(" + SelectdIndividualTransferedPaymentId + ")";

                    entityPaymentTransferedListBO = billEntityDA.GetGuestServiceBillInfoDynamically(frontOfficeInvoiceTemplate, "PaymentTransfered", registrationIdList, strPaymentTransferedStringParamer, IsBillSplited, 0);
                    reportDataSourceList.AddRange(entityPaymentTransferedListBO);
                }
            }

            List<GuestServiceBillApprovedBO> SortedDataSourceList = reportDataSourceList.OrderBy(a => a.ServiceDate).ThenBy(a => a.GuestService).ToList(); //reportDataSourceList.OrderBy(o => o.ServiceDate).ToList();

            //// Distinct Related Work -------------------------
            List<GuestServiceBillApprovedBO> distinctDataSourceList = SortedDataSourceList.GroupBy(p => new { p.ApprovedId, p.ServiceId, p.ServiceDate, p.Reference, p.GuestService, p.ServiceRate }).Select(g => g.First()).ToList();
            decimal currencyRateAmount = !string.IsNullOrWhiteSpace(currencyRate) ? Convert.ToDecimal(currencyRate) : 0;
            if (currencyRateAmount > 0)
            {
                Boolean isUsdInvoice = false;
                foreach (GuestServiceBillApprovedBO row in distinctDataSourceList)
                {
                    row.BillAmount = row.BillAmount / currencyRateAmount;
                    row.PaidAmount = row.PaidAmount / currencyRateAmount;
                    if (!isUsdInvoice)
                    {
                        isUsdInvoice = true;
                        row.InvoiceCurrencyId = 46;
                    }
                }
            }

            HttpContext.Current.Session["ReportGuestBillInfoDataSource"] = distinctDataSourceList.OrderBy(x => x.ServiceDate).ToList();



            ////////------------Group Check Out --------------------------------------------------------------
            ////if (entityOwnPaymentListBO.Count == 0 && entityBillTransferListBO.Count == 0 && entityOthersRoomPaymentListBO.Count == 0 && entityPaymentTransferedListBO.Count == 0)
            ////{
            ////    decimal totalBill = 0;
            ////    foreach (GuestServiceBillApprovedBO row in distinctDataSourceList)
            ////    {
            ////        totalBill = totalBill + row.BillAmount;
            ////    }


            ////    List<GuestServiceBillApprovedBO> entityGroupPaymentListBO = new List<GuestServiceBillApprovedBO>();

            ////    registrationIdList = registrationIdList + "~" + totalBill.ToString();

            ////    string strGroupPaymentStringParamer = " WHERE (dbo.FnDate(gsba.ServiceDate) BETWEEN dbo.FnDate('" + hmUtility.GetDateTimeFromString(StartDate) + "') AND dbo.FnDate('" + hmUtility.GetDateTimeFromString(EndDate) + "'))";
            ////    entityGroupPaymentListBO = billEntityDA.GetGuestServiceBillInfo("GuestGroupPaymentForIndividualRoom", registrationIdList, strOwnPaymentStringParamer, IsBillSplited, 0);


            ////    reportDataSourceList.AddRange(entityGroupPaymentListBO);
            ////}


            ////HttpContext.Current.Session["ReportGuestBillInfoDataSource"] = distinctDataSourceList;

        }
    }
}