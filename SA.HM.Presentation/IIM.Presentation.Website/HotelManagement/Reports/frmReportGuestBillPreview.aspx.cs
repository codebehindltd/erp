using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Restaurant;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Net;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportGuestBillPreview : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int IsInvoiceTemplate1Visible = -1;
        string GuestBillRoomIdParameterValue = string.Empty;
        string GuestBillServiceIdParameterValue = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadGuestBillPreview();
            }
        }
        //************************ User Defined Function ********************//
        private void LoadGuestBillPreview()
        {
            if (this.Session["CheckOutRegistrationIdList"] != null)
            {
                string registrationId = this.Session["CheckOutRegistrationIdList"].ToString();

                string lrpQueryStringId = Request.QueryString["lrp"];
                if (lrpQueryStringId == "1")
                {
                    if (!String.IsNullOrEmpty(registrationId))
                    {
                        string linkRoomRegistrationIdList = string.Empty;
                        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                        List<HotelLinkedRoomDetailsBO> hotelLinkedRoomDetailsList = new List<HotelLinkedRoomDetailsBO>();
                        hotelLinkedRoomDetailsList = roomRegistrationDA.GetLinkedDetailsRoomInfoByRegistrationId(Int64.Parse(registrationId));
                        if (hotelLinkedRoomDetailsList != null)
                        {
                            if (hotelLinkedRoomDetailsList.Count > 0)
                            {
                                foreach (HotelLinkedRoomDetailsBO row in hotelLinkedRoomDetailsList)
                                {
                                    if (string.IsNullOrWhiteSpace(linkRoomRegistrationIdList))
                                    {
                                        linkRoomRegistrationIdList = row.RegistrationId.ToString();
                                    }
                                    else
                                    {
                                        linkRoomRegistrationIdList = linkRoomRegistrationIdList + "," + row.RegistrationId.ToString();
                                    }
                                }
                            }
                            else
                            {
                                linkRoomRegistrationIdList = registrationId.ToString();
                            }
                        }
                        else
                        {
                            linkRoomRegistrationIdList = registrationId.ToString();
                        }

                        registrationId = linkRoomRegistrationIdList;
                    }
                }

                this.LoadRoomGridView(registrationId);
                this.LoadServiceGridView(registrationId);
                //this.SavePendingGuestRoomInformation();
                this.LoadBillPrintPreviewDynamicallyForReport(registrationId);
                this.ReportProcessing();
            }
        }
        private void LoadBillPrintPreviewDynamicallyForReport(string registrationId)
        {
            HMUtility hmUtility = new HMUtility();
            HttpContext.Current.Session["CheckOutRegistrationIdList"] = registrationId;
            string currencyRate = "0.00";
            string isIsplite = "0";
            string serviceType = "";
            string SelectdIndividualTransferedPaymentId = "0";
            string SelectdPaymentId = "0";
            string SelectdIndividualPaymentId = "0";
            string SelectdIndividualServiceId = "0";
            string SelectdIndividualRoomId = "0";
            string SelectdServiceId = "0";
            string SelectdRoomId = "0";
            string StartDate = hmUtility.GetFromDate();
            string EndDate = hmUtility.GetToDate();
            string ddlRegistrationId = registrationId;
            string txtSrcRegistrationIdList = registrationId;

            HttpContext.Current.Session["IsBillSplited"] = "0";
            HttpContext.Current.Session["GuestBillFromDate"] = hmUtility.GetFromDate();
            HttpContext.Current.Session["GuestBillToDate"] = hmUtility.GetToDate();

            InnboardWebUtility.BillPrintPreviewDynamicallyForReport(currencyRate, isIsplite, serviceType, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdIndividualServiceId, SelectdIndividualRoomId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, ddlRegistrationId, txtSrcRegistrationIdList);

        }
        private void LoadRoomGridView(string registrationId)
        {
            decimal serviceAmountForCompanyGuest = 0;
            GuestHouseCheckOutDA da = new GuestHouseCheckOutDA();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            HMCommonDA hmCommonDA = new HMCommonDA();
            string registrationIdList = hmCommonDA.GetRegistrationIdList(registrationId);
            //List<GuestHouseCheckOutDetailBO> files = da.GetGuestHouseBill(registrationIdList, "GuestRoom");
            List<GuestHouseCheckOutDetailBO> files = da.GetGuestServiceInformationForCheckOut(registrationIdList, "GuestRoom", DateTime.Now, userInformationBO.UserInfoId);
            //List<GuestHouseCheckOutDetailBO> distinctItems = new List<GuestHouseCheckOutDetailBO>();

            //foreach (GuestHouseCheckOutDetailBO row in files)
            //{
            //    if (distinctItems.Count() > 0)
            //    {
            //        var v = (from m in distinctItems where m.RegistrationId == row.RegistrationId && m.ServiceDate.Date == row.ServiceDate.Date && m.ServiceName == row.ServiceName select m).FirstOrDefault();
            //        if (v == null)
            //            distinctItems.Add(row);
            //    }
            //    else
            //    { distinctItems.Add(row); }
            //}

            this.gvRoomDetail.DataSource = files;
            this.gvRoomDetail.DataBind();
        }
        private void LoadServiceGridView(string registrationId)
        {
            this.gvServiceDetail.DataSource = null;
            this.gvServiceDetail.DataBind();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GuestHouseCheckOutDA da = new GuestHouseCheckOutDA();
            HMCommonDA hmCommonDA = new HMCommonDA();
            string registrationIdList = hmCommonDA.GetRegistrationIdList(registrationId);
            List<GuestHouseCheckOutDetailBO> files = da.GetGuestServiceInformationForCheckOut(registrationIdList, "GuestService", DateTime.Now, userInformationBO.UserInfoId);

            //// --- Guest Service Information -----------------------------------------------------------
            //List<GuestHouseCheckOutDetailBO> guestServiceInfoList = new List<GuestHouseCheckOutDetailBO>();

            //// --- Guest Service Bill Information -----------------------------------------------------------
            //List<GuestHouseCheckOutDetailBO> guestServiceBillAlreadyAchivedInfoList = new List<GuestHouseCheckOutDetailBO>();
            //guestServiceBillAlreadyAchivedInfoList = files.Where(test => test.NightAuditApproved == "Y" && test.IsPaidService == true).ToList();


            //// --- Guest Service Bill Information -----------------------------------------------------------
            //List<GuestHouseCheckOutDetailBO> guestServiceBillInfoList = new List<GuestHouseCheckOutDetailBO>();
            //guestServiceBillInfoList = files.Where(test => test.IsPaidService == false).ToList();

            //// --- Paid Service Information ------------------------------------------------------------
            //List<GuestHouseCheckOutDetailBO> paidServiceInfoList = new List<GuestHouseCheckOutDetailBO>();
            //List<GuestHouseCheckOutDetailBO> allPaidServiceInfoList = new List<GuestHouseCheckOutDetailBO>();
            //allPaidServiceInfoList = files.Where(test => test.IsPaidService == true).ToList();

            //GuestHouseCheckOutDetailBO IsGuestTodaysBillAddBOInfo = da.GetIsGuestTodaysBillAdd(registrationIdList);
            //if (IsGuestTodaysBillAddBOInfo.IsGuestTodaysBillAdd == 0)
            //{
            //    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            //    Session["GuestRoomRegistrationBO"] = null;

            //    foreach (GuestHouseCheckOutDetailBO row in allPaidServiceInfoList)
            //    {
            //        RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();

            //        if (Session["GuestRoomRegistrationBO"] == null)
            //        {
            //            roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(row.RegistrationId);
            //            Session["GuestRoomRegistrationBO"] = roomRegistrationBO;
            //        }
            //        else
            //        {
            //            roomRegistrationBO = Session["GuestRoomRegistrationBO"] as RoomRegistrationBO;
            //        }

            //        if (roomRegistrationBO != null)
            //        {
            //            if (roomRegistrationBO.IsPaidServiceExist)
            //            {
            //                if (DateTime.Now.Date == roomRegistrationBO.ArriveDate.Date)
            //                {
            //                    foreach (GridViewRow roomInforow in gvRoomDetail.Rows)
            //                    {
            //                        AvailableGuestListBO availableGuestList = new AvailableGuestListBO();
            //                        availableGuestList.RegistrationId = Int32.Parse(((Label)roomInforow.FindControl("lblRegistrationId")).Text);

            //                        if (availableGuestList.RegistrationId == row.RegistrationId)
            //                        {
            //                            Label isNightAuditApprovedDate = (Label)roomInforow.FindControl("lblServiceDate");

            //                            DateTime roomBillDate = hmUtility.GetDateTimeFromString(isNightAuditApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            //                            if (roomBillDate.Date != DateTime.Now.Date)
            //                            {
            //                                if (roomBillDate.Date > row.ServiceDate.Date)
            //                                { }
            //                                else if (roomBillDate.Date == row.ServiceDate.Date)
            //                                {
            //                                    paidServiceInfoList.Add(row);
            //                                }
            //                                else if (roomBillDate.Date < row.ServiceDate.Date)
            //                                {
            //                                    row.ServiceDate = row.ServiceDate.AddDays(-1);
            //                                    paidServiceInfoList.Add(row);
            //                                }
            //                            }
            //                            else
            //                            {
            //                                var v = guestServiceBillAlreadyAchivedInfoList.Where(x => x.ServiceId == row.ServiceId && x.ServiceDate.Date == row.ServiceDate.Date).FirstOrDefault();

            //                                if (v == null)
            //                                {
            //                                    paidServiceInfoList.Add(row);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    if (row.ServiceDate > DateTime.Now.Date.AddDays(-1))
            //                    {
            //                        if (row.NightAuditApproved == "N")
            //                        {
            //                            row.ServiceDate = row.ServiceDate.AddDays(-1);
            //                            var v = guestServiceBillAlreadyAchivedInfoList.Where(x => x.ServiceId == row.ServiceId && x.ServiceDate.Date == row.ServiceDate.Date).FirstOrDefault();

            //                            if (v == null)
            //                            {
            //                                paidServiceInfoList.Add(row);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }

            //    guestServiceInfoList.AddRange(guestServiceBillAlreadyAchivedInfoList);
            //}
            //else
            //{
            //    paidServiceInfoList.AddRange(allPaidServiceInfoList);
            //}


            //guestServiceInfoList.AddRange(guestServiceBillInfoList);
            //guestServiceInfoList.AddRange(paidServiceInfoList);

            //this.gvServiceDetail.DataSource = guestServiceInfoList.OrderBy(x => x.ServiceDate).ToList();
            //this.gvServiceDetail.DataBind();

            this.gvServiceDetail.DataSource = files;
            this.gvServiceDetail.DataBind();

            //if (guestServiceInfoList.Count > 0)
            //{
            //    //this.CalculateGuestServiceAmountTotal();
            //}
            //else
            //{
            //    this.gvServiceDetail.DataSource = null;
            //    this.gvServiceDetail.DataBind();
            //}
        }
        //private void SavePendingGuestRoomInformation()
        //{
        //    DateTime dateTime = DateTime.Now;
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
        //    List<GuestHouseCheckOutBO> guestHouseCheckOutBOList = new List<GuestHouseCheckOutBO>();
        //    List<GHServiceBillBO> entityRoomDetailBOList = new List<GHServiceBillBO>();
        //    List<GHServiceBillBO> entityDetailBOList = new List<GHServiceBillBO>();
        //    GuestHouseCheckOutDA guestHouseCheckOutDA = new GuestHouseCheckOutDA();
        //    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
        //    int totalRoomCheckOut = 1;

        //    for (int i = 0; i <= totalRoomCheckOut; i++)
        //    {
        //        if (i == 0)
        //        {
        //            //// -- Room List Not Approved From Night Audit----------
        //            //foreach (GridViewRow row in gvRoomDetail.Rows)
        //            //{
        //            //    AvailableGuestListBO availableGuestList = new AvailableGuestListBO();
        //            //    Label isNightAuditApproved = (Label)row.FindControl("lblNightAuditApproved");

        //            //    Label isNightAuditApprovedDate = (Label)row.FindControl("lblServiceDate");

        //            //    if (isNightAuditApproved.Text == "N")
        //            //    {
        //            //        availableGuestList.RegistrationId = Int32.Parse(((Label)row.FindControl("lblRegistrationId")).Text);
        //            //        availableGuestList.RoomType = ((Label)row.FindControl("lblServiceType")).Text;
        //            //        availableGuestList.RoomId = Int32.Parse(((Label)row.FindControl("lblServiceId")).Text);
        //            //        availableGuestList.RoomNumber = Convert.ToInt32(((Label)row.FindControl("lblRoomNumber")).Text);
        //            //        availableGuestList.ServiceName = (((Label)row.FindControl("lblServiceName")).Text);
        //            //        availableGuestList.PreviousRoomRate = Convert.ToDecimal(((Label)row.FindControl("lblServiceRate")).Text) - Convert.ToDecimal(((Label)row.FindControl("lblDiscountAmount")).Text);

        //            //        availableGuestList.ServiceDate = Convert.ToDateTime(((Label)row.FindControl("lblServiceDate")).Text);
        //            //        availableGuestList.ApprovedDate = availableGuestList.ServiceDate;
        //            //        availableGuestList.RoomRate = Convert.ToDecimal(((Label)row.FindControl("lblServiceRate")).Text);
        //            //        availableGuestList.BPPercentAmount = Convert.ToDecimal(0);
        //            //        availableGuestList.BPDiscountAmount = Convert.ToDecimal(((Label)row.FindControl("lblDiscountAmount")).Text);
        //            //        availableGuestList.VatAmount = Convert.ToDecimal(((Label)row.FindControl("lblVatAmount")).Text);
        //            //        availableGuestList.ServiceCharge = Convert.ToDecimal(((Label)row.FindControl("lblServiceCharge")).Text);
        //            //        availableGuestList.CitySDCharge = 0;
        //            //        availableGuestList.AdditionalCharge = 0;
        //            //        availableGuestList.ReferenceSalesCommission = Convert.ToDecimal(((Label)row.FindControl("lblgvReferenceSalesCommission")).Text);
        //            //        availableGuestList.ApprovedStatus = true;
        //            //        availableGuestList.TotalCalculatedAmount = Convert.ToDecimal(((Label)row.FindControl("lblTotalAmount")).Text);

        //            //        int tmpApprovedId = 0;
        //            //        availableGuestList.CreatedBy = userInformationBO.UserInfoId;
        //            //        Boolean status = roomRegistrationDA.SavePendingGuestBillApprovedInfo(availableGuestList, out tmpApprovedId);

        //            //    }
        //            //}

        //            //// -- Service List Not Approved From Night Audit----------
        //            //foreach (GridViewRow row in gvServiceDetail.Rows)
        //            //{
        //            //    GHServiceBillBO entityDetailBO = new GHServiceBillBO();
        //            //    Label isNightAuditApproved = (Label)row.FindControl("lblNightAuditApproved");

        //            //    if (isNightAuditApproved.Text == "N")
        //            //    {
        //            //        GHServiceBillBO serviceList = new GHServiceBillBO();

        //            //        serviceList.RegistrationId = Int32.Parse(((Label)row.FindControl("lblRegistrationId")).Text);
        //            //        serviceList.IsPaidService = Convert.ToBoolean(((Label)row.FindControl("lblgvIsPaidService")).Text);
        //            //        serviceList.ServiceBillId = Int32.Parse(((Label)row.FindControl("lblid")).Text);
        //            //        serviceList.ServiceDate = Convert.ToDateTime(((Label)row.FindControl("lblServiceDate")).Text);
        //            //        serviceList.ApprovedDate = serviceList.ServiceDate;
        //            //        serviceList.ServiceType = ((Label)row.FindControl("lblServiceType")).Text;
        //            //        serviceList.ServiceId = Int32.Parse(((Label)row.FindControl("lblServiceId")).Text);
        //            //        serviceList.ServiceName = ((Label)row.FindControl("lblServiceName")).Text;
        //            //        serviceList.ServiceQuantity = Convert.ToDecimal(((Label)row.FindControl("lblServiceQuantity")).Text);
        //            //        serviceList.ServiceRate = Convert.ToDecimal(((Label)row.FindControl("lblServiceRate")).Text);
        //            //        serviceList.DiscountAmount = Convert.ToDecimal(((Label)row.FindControl("lblDiscountAmount")).Text);
        //            //        serviceList.VatAmount = Convert.ToDecimal(((Label)row.FindControl("lblVatAmount")).Text);
        //            //        serviceList.ServiceCharge = Convert.ToDecimal(((Label)row.FindControl("lblServiceCharge")).Text);
        //            //        serviceList.CitySDCharge = Convert.ToDecimal(((Label)row.FindControl("lblCitySDCharge")).Text);
        //            //        serviceList.AdditionalCharge = Convert.ToDecimal(((Label)row.FindControl("lblAdditionalCharge")).Text);
        //            //        serviceList.ApprovedStatus = true;
        //            //        serviceList.CreatedBy = userInformationBO.UserInfoId;
        //            //        int tmpApprovedId = 0;
        //            //        Boolean status = roomRegistrationDA.SavePendingGuestServiceBillApprovedInfo(serviceList, out tmpApprovedId);
        //            //    }
        //            //}
        //        }
        //    }

        //}
        private void ReportProcessing()
        {
            if (this.Session["CheckOutRegistrationIdList"] != null)
            {
                int frontOfficeInvoiceTemplate = 1;
                int isCashierNameShownInInvoice = 1;
                string footerPoweredByInfo = string.Empty;
                this.txtRegistrationNumber.Text = string.Empty;
                this.txtCompanyName.Text = string.Empty;
                this.txtCompanyAddress.Text = string.Empty;
                this.txtCompanyWeb.Text = string.Empty;
                //this.txtGroupCompanyName.Text = string.Empty;

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                this.txtPrintedBy.Text = userInformationBO.UserName;
                footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                HMCommonDA hmCommonDA = new HMCommonDA();
                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> files = companyDA.GetCompanyInfo();

                if (files[0].CompanyId > 0)
                {
                    this.txtCompanyName.Text = files[0].CompanyName;
                    this.txtCompanyAddress.Text = files[0].CompanyAddress;
                    this.txtVatRegistrationNo.Text = files[0].VatRegistrationNo;
                    this.txtGroupCompanyName.Text = files[0].GroupCompanyName;
                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        this.txtCompanyWeb.Text = files[0].WebAddress;
                    }
                    else
                    {
                        this.txtCompanyWeb.Text = files[0].ContactNumber;
                    }
                }

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isOnlyPdfEnableWhenReportExportBO = new HMCommonSetupBO();
                isOnlyPdfEnableWhenReportExportBO = commonSetupDA.GetCommonConfigurationInfo("IsOnlyPdfEnableWhenReportExport", "IsOnlyPdfEnableWhenReportExport");
                if (isOnlyPdfEnableWhenReportExportBO != null)
                {
                    if (isOnlyPdfEnableWhenReportExportBO.SetupValue == "1")
                    {
                        if (hmUtility.GetCurrentApplicationUserInfo().IsAdminUser != true)
                        {
                            CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Excel.ToString());
                            CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Word.ToString());

                            CommonHelper.DisableReportExportFormat(rvInvoiceTransaction, CommonHelper.ReportImportFormat.Excel.ToString());
                            CommonHelper.DisableReportExportFormat(rvInvoiceTransaction, CommonHelper.ReportImportFormat.Word.ToString());
                        }
                    }
                }

                this.txtRegistrationNumber.Text = this.Session["CheckOutRegistrationIdList"].ToString();
                this.txtIsBillSplited.Text = this.Session["IsBillSplited"].ToString();
                this.txtGuestBillFromDate.Text = this.Session["GuestBillFromDate"].ToString();
                this.txtGuestBillToDate.Text = this.Session["GuestBillToDate"].ToString();

                HMCommonSetupBO frontOfficeInvoiceTemplateBO = new HMCommonSetupBO();
                frontOfficeInvoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("FrontOfficeInvoiceTemplate", "FrontOfficeInvoiceTemplate");
                if (frontOfficeInvoiceTemplateBO != null)
                {
                    frontOfficeInvoiceTemplate = Convert.ToInt32(frontOfficeInvoiceTemplateBO.SetupValue);
                }

                //IsCashierNameShownInInvoice
                HMCommonSetupBO IsCashierNameShownInInvoiceBO = new HMCommonSetupBO();
                IsCashierNameShownInInvoiceBO = commonSetupDA.GetCommonConfigurationInfo("IsCashierNameShownInInvoice", "IsCashierNameShownInInvoice");
                if (IsCashierNameShownInInvoiceBO != null)
                {
                    isCashierNameShownInInvoice = Convert.ToInt32(IsCashierNameShownInInvoiceBO.SetupValue);
                }

                //int template = Int32.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["GuestHouseInvoiceTemplate"].ToString());

                //FrontOfficeMasterInvoiceTemplateBO
                int frontOfficeMasterInvoiceTemplate = 1;
                HMCommonSetupBO FrontOfficeMasterInvoiceTemplateBO = new HMCommonSetupBO();
                FrontOfficeMasterInvoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("FrontOfficeMasterInvoiceTemplate", "FrontOfficeMasterInvoiceTemplate");
                if (FrontOfficeMasterInvoiceTemplateBO != null)
                {
                    frontOfficeMasterInvoiceTemplate = Convert.ToInt32(FrontOfficeMasterInvoiceTemplateBO.SetupValue);
                }

                var reportPath = "";
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/InvoiceGuestBill.rdlc");

                //IsInvoiceGuestBillWithoutHeaderandFooter
                HMCommonSetupBO isInvoiceGuestBillWithoutHeaderandFooterBO = new HMCommonSetupBO();
                isInvoiceGuestBillWithoutHeaderandFooterBO = commonSetupDA.GetCommonConfigurationInfo("IsInvoiceGuestBillWithoutHeaderAndFooter", "IsInvoiceGuestBillWithoutHeaderAndFooter");
                if (isInvoiceGuestBillWithoutHeaderandFooterBO != null)
                {
                    if (isInvoiceGuestBillWithoutHeaderandFooterBO.SetupValue == "0")
                    {
                        if (frontOfficeMasterInvoiceTemplate == 1)
                        {
                            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/InvoiceGuestBill.rdlc");
                        }
                        else
                        {
                            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/InvoiceGuestBillTwo.rdlc");
                        }
                    }
                    else
                    {
                        if (frontOfficeMasterInvoiceTemplate == 1)
                        {
                            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/InvoiceGuestBillWithoutHeaderandFooter.rdlc");
                        }
                        else
                        {
                            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/InvoiceGuestBillWithoutHeaderandFooter.rdlc");
                        }

                    }
                }

                if (!File.Exists(reportPath))
                {
                    return;
                }

                rvInvoiceTransaction.LocalReport.ReportPath = reportPath;

                string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");

                // // ----- Costcenter Wise Invoice Logo Information
                List<CostCentreTabBO> costCentreTabBO = new List<CostCentreTabBO>();
                CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

                costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoByType("FrontOffice");
                if (costCentreTabBO != null)
                {
                    if (costCentreTabBO.Count > 0)
                    {
                        rvInvoiceTransaction.LocalReport.EnableExternalImages = true;
                        List<ReportParameter> paramReport = new List<ReportParameter>();
                        
                        if (!string.IsNullOrWhiteSpace(costCentreTabBO[0].CostCenterLogo))
                        {
                            ImageName = costCentreTabBO[0].CostCenterLogo;
                            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
                        }
                        else
                        {
                            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderMiddleImagePath"))));
                        }
                        
                        paramReport.Add(new ReportParameter("ThankYouMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIThankYouMessege")));
                        paramReport.Add(new ReportParameter("CorporateAddress", hmCommonDA.GetCustomFieldValueByFieldName("paramGICorporateAddress")));
                        paramReport.Add(new ReportParameter("AggrimentMessege", hmCommonDA.GetCustomFieldValueByFieldName("paramGIAggrimentMessege")));

                        paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                        paramReport.Add(new ReportParameter("GroupCompanyName", files[0].GroupCompanyName));
                        paramReport.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));
                        paramReport.Add(new ReportParameter("VatRegistrationNo", files[0].VatRegistrationNo));
                        paramReport.Add(new ReportParameter("IsBigSizeCompanyLogo", hmCommonDA.GetCustomFieldValueByFieldName("IsBigSizeCompanyLogo")));
                        paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

                        HMCommonSetupBO isWaterMarkImageEnableBO = new HMCommonSetupBO();
                        isWaterMarkImageEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsWaterMarkImageDisplayEnable", "IsWaterMarkImageDisplayEnable");
                        if (isWaterMarkImageEnableBO != null)
                        {
                            if (isWaterMarkImageEnableBO.SetupValue == "1")
                            {
                                paramReport.Add(new ReportParameter("WaterMarkImagePath", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/WaterMarkTextA4.png")));
                            }
                            else
                            {
                                paramReport.Add(new ReportParameter("WaterMarkImagePath", ""));
                            }
                        }

                        paramReport.Add(new ReportParameter("IsCashierNameShownInInvoice", isCashierNameShownInInvoice.ToString()));

                        string isDiscountApplicableOnRackRate = "0";
                        //List<CostCentreTabBO> costCentreTabBO = new List<CostCentreTabBO>();
                        //CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

                        //costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoByType("FrontOffice");

                        if (costCentreTabBO.Count > 0)
                        {
                            isDiscountApplicableOnRackRate = costCentreTabBO[0].IsDiscountApplicableOnRackRate == true ? "1" : "0";
                        }

                        if (frontOfficeInvoiceTemplate == 1)
                        {
                            paramReport.Add(new ReportParameter("FrontOfficeInvoiceTemplate", "InvoiceTemplateOne"));
                        }
                        else if (frontOfficeInvoiceTemplate == 2)
                        {
                            paramReport.Add(new ReportParameter("FrontOfficeInvoiceTemplate", "InvoiceTemplateTwo"));
                        }
                        else if (frontOfficeInvoiceTemplate == 3)
                        {
                            //paramReport.Add(new ReportParameter("FrontOfficeInvoiceTemplate", "InvoiceTemplateThree"));
                            if (isDiscountApplicableOnRackRate == "0")
                            {
                                paramReport.Add(new ReportParameter("FrontOfficeInvoiceTemplate", "InvoiceTemplateThree"));
                            }
                            else
                            {
                                paramReport.Add(new ReportParameter("FrontOfficeInvoiceTemplate", "InvoiceTemplateSix"));
                            }
                        }
                        else if (frontOfficeInvoiceTemplate == 4)
                        {
                            paramReport.Add(new ReportParameter("FrontOfficeInvoiceTemplate", "InvoiceTemplateFour"));
                        }
                        else if (frontOfficeInvoiceTemplate == 5)
                        {
                            paramReport.Add(new ReportParameter("FrontOfficeInvoiceTemplate", "InvoiceTemplateFive"));
                        }

                        if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                        {
                            paramReport.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                        }
                        else
                        {
                            paramReport.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                        }

                        // //----------------- Show Hide Related Information -------------------------------------------------------
                        string IsServiceChargeEnableConfig = "1";
                        string IsCitySDChargeEnableConfig = "1";
                        string IsVatAmountEnableConfig = "1";
                        string IsAdditionalChargeEnableConfig = "1";

                        //List<CostCentreTabBO> costCentreTabBO = new List<CostCentreTabBO>();
                        //CostCentreTabDA costCentreTabDA = new CostCentreTabDA();

                        costCentreTabBO = costCentreTabDA.GetCostCentreTabInfoByType("FrontOffice");

                        if (costCentreTabBO.Count > 0)
                        {
                            IsServiceChargeEnableConfig = costCentreTabBO[0].IsServiceChargeEnable ? "1" : "0";
                            IsCitySDChargeEnableConfig = costCentreTabBO[0].IsCitySDChargeEnable ? "1" : "0";
                            IsVatAmountEnableConfig = costCentreTabBO[0].IsVatEnable ? "1" : "0";
                            IsAdditionalChargeEnableConfig = costCentreTabBO[0].IsAdditionalChargeEnable ? "1" : "0";
                        }

                        paramReport.Add(new ReportParameter("ServiceChargeEnableConfig", IsServiceChargeEnableConfig));
                        paramReport.Add(new ReportParameter("CitySDChargeEnableConfig", IsCitySDChargeEnableConfig));
                        paramReport.Add(new ReportParameter("VatAmountEnableConfig", IsVatAmountEnableConfig));
                        paramReport.Add(new ReportParameter("AdditionalChargeEnableConfig", IsAdditionalChargeEnableConfig));

                        string isBillSummaryPartWillHide = "0";
                        HMCommonSetupBO IsBillSummaryPartWillHideBO = new HMCommonSetupBO();
                        IsBillSummaryPartWillHideBO = commonSetupDA.GetCommonConfigurationInfo("FrontOfficeMasterInvoiceTemplate", "IsBillSummaryPartWillHide");
                        if (IsBillSummaryPartWillHideBO != null)
                        {
                            isBillSummaryPartWillHide = IsBillSummaryPartWillHideBO.SetupValue;
                        }
                        paramReport.Add(new ReportParameter("IsBillSummaryPartWillHide", isBillSummaryPartWillHide));

                        rvInvoiceTransaction.LocalReport.SetParameters(paramReport);

                        GuestBillPaymentDA paymentDa = new GuestBillPaymentDA();
                        List<GenerateGuestBillReportBO> guestBill = new List<GenerateGuestBillReportBO>();

                        guestBill = paymentDa.GetGenerateGuestBill(txtRegistrationNumber.Text, txtIsBillSplited.Text, txtGuestBillFromDate.Text, txtGuestBillToDate.Text, txtPrintedBy.Text);

                        // // ----------- Guest Stayed Night Count ---------------------
                        List<GuestServiceBillApprovedBO> reportDetailsInfo = Session["ReportGuestBillInfoDataSource"] as List<GuestServiceBillApprovedBO>;
                        int nightCount = reportDetailsInfo.Where(x => x.PaymentType == "RoomService").ToList().Count;
                        foreach (GenerateGuestBillReportBO row in guestBill)
                        {
                            row.Night = nightCount;
                        }

                        //if (HttpContext.Current.Session["BillPreviewCurrencyRateInformation"] != null)
                        //{
                        //    decimal currencyRateAmount = !string.IsNullOrWhiteSpace(HttpContext.Current.Session["BillPreviewCurrencyRateInformation"].ToString()) ? Convert.ToDecimal(HttpContext.Current.Session["BillPreviewCurrencyRateInformation"]) : 0;
                        //    if (currencyRateAmount > 0)
                        //    {
                        //        foreach (GenerateGuestBillReportBO row in guestBill)
                        //        {
                        //            row.CurrencyHead = "USD";
                        //            row.RoomRate = row.RoomRate / currencyRateAmount;
                        //            row.UnitPrice = row.UnitPrice / currencyRateAmount;
                        //        }
                        //    }
                        //}

                        var reportDataset = rvInvoiceTransaction.LocalReport.GetDataSourceNames();
                        rvInvoiceTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], guestBill));

                        rvInvoiceTransaction.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubReportProcessingForInvoiceDetail);
                        rvInvoiceTransaction.LocalReport.Refresh();
                        InvoiceTemplate1.Visible = false;
                        InvoiceTemplate2.Visible = true;
                    }
                }
            }
        }
        private void SubReportProcessingForInvoiceDetail(object sender, SubreportProcessingEventArgs e)
        {
            InvoiceTemplate1.Visible = false;
            InvoiceTemplate2.Visible = true;

            e.DataSources.Add(new ReportDataSource("GuestServiceBillInfo", Session["ReportGuestBillInfoDataSource"] as List<GuestServiceBillApprovedBO>));
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rvInvoiceTransaction.LocalReport.Render("PDF", null, out mimeType,
                               out encoding, out extension, out streamids, out warnings);

                string fileName = string.Empty, fileNamePrint = string.Empty;
                DateTime dateTime = DateTime.Now;
                fileName = "OutPut" + String.Format("{0:ddMMMyyyyhhmmsstt}", dateTime) + ".pdf";
                fileNamePrint = "Print" + String.Format("{0:ddMMMyyyyhhmmsstt}", dateTime) + ".pdf";

                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName), FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                //Open exsisting pdf
                Document document = new Document(PageSize.LETTER);
                PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileName));
                //Getting a instance of new pdf wrtiter
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
                   HttpContext.Current.Server.MapPath("~/PrintDocument/" + fileNamePrint), FileMode.Create));
                document.Open();
                PdfContentByte cb = writer.DirectContent;

                int i = 0;
                int p = 0;
                int n = reader.NumberOfPages;
                Rectangle psize = reader.GetPageSize(1);

                float width = psize.Width;
                float height = psize.Height;

                //Add Page to new document
                while (i < n)
                {
                    document.NewPage();
                    p++;
                    i++;

                    PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                    cb.AddTemplate(page1, 0, 0);
                }

                //Attach javascript to the document
                PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
                //PdfAction jAction = PdfAction.JavaScript("var pp = getPrintParams();pp.interactive = pp.constants.interactionLevel.automatic;pp.printerName = getPrintParams().printerName;print(pp);\r", writer);
                writer.AddJavaScript(jAction);

                document.Close();

                frmPrint.Attributes["src"] = "../../PrintDocument/" + fileNamePrint;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}