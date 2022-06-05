using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using System.IO;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using System.Web.Services;
using HotelManagement.Data;
using System.Xml;

namespace HotelManagement.Presentation.Website.POS.Reports
{
    public partial class frmReportSalesInformation : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int _IsReportPanelEnable = -1;
        protected Boolean isRestaurantReportRestrictionForAllUser = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            //if (userInformationBO.CompanyType == "Agro")
            //{
            //    Response.Redirect("/POS/Reports/frmReportAgroSalesInfo.aspx");
            //}

            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                IsRestaurantReportRestrictionForAllUser();
                LoadCostCenter();
                FilterByItem();
                LoadServiceInfo();
                LoadCategory();
                LoadCommonDropDownHiddenField();
                LoadRestaurantUserInfo();
                LoadRoomInformation();
                LoadTableInformation();
                IsRestaurantIntegrateWithFrontOffice();
                CheckIfFoodNBeverageSalesRelatedDataHide();
            }
            CheckIsOnlyRetailPOS();
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();
            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());
            frmPrint.Attributes["src"] = reportSource;
        }

        private void CheckIsOnlyRetailPOS()
        {
            int IsOnlyRetailPOS = int.Parse(Session["IsOnlyRetailPOS"].ToString());
            if (IsOnlyRetailPOS == 1)
            {
                lblWaiter.Visible = false;
                ddlWaiterInfoId.Visible = false;

                lblOutletType.Visible = false;
                ddlOutletType.Visible = false;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;
            DateTime dateTime = DateTime.Now;
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
            string costcenterIdList = string.Empty;
            string userInformation = string.Empty;
            string serviceIdList = string.Empty, serviceNameList = string.Empty;

            IsRestaurantReportRestrictionForAllUser();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = this.txtToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            ToDate = ToDate.AddDays(1).AddSeconds(-1);

            DateTime ReportDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            if (!string.IsNullOrEmpty(hfCostcenterId.Value))
            {
                costcenterIdList = hfCostcenterId.Value;
            }
            else
            {
                Boolean IsAdminUser = false;

                // // // ------User Admin Authorization BO Session Information --------------------------------
                #region User Admin Authorization
                if (userInformationBO.UserInfoId == 1)
                {
                    IsAdminUser = true;
                }
                else
                {
                    List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                    adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                    if (adminAuthorizationList != null)
                    {
                        if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 6).Count() > 0)
                        {
                            IsAdminUser = true;
                        }
                    }
                }
                #endregion

                CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
                List<CostCentreTabBO> entityListBO = new List<CostCentreTabBO>();

                if (IsAdminUser)
                {
                    entityListBO = costCentreTabDA.GetAllRestaurantTypeCostCentreTabInfo().Where(x => x.IsRestaurant == true).ToList();
                }
                else
                {
                    if (!isRestaurantReportRestrictionForAllUser)
                    {
                        entityListBO = costCentreTabDA.GetAllRestaurantTypeCostCentreTabInfo().Where(x => x.IsRestaurant == true).ToList();
                    }
                    else
                    {
                        entityListBO = costCentreTabDA.GetRestaurantTypeCostCentreTabInfo(userInformationBO.UserId, userInformationBO.UserInfoId, 0).Where(x => x.IsRestaurant == true).ToList();
                    }
                }

                //CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
                //List<CostCentreTabBO> List = costCentreTabDA.GetCostCentreTabInfoByType("Restaurant,RetailPos,Billing,OtherOutlet");
                foreach (CostCentreTabBO row in entityListBO)
                {
                    if (!string.IsNullOrEmpty(costcenterIdList))
                        costcenterIdList += ", " + row.CostCenterId;
                    else costcenterIdList += row.CostCenterId;
                }
            }

            Int32 companyId = 0;
            if (ddlReportType.SelectedValue == "CompanyWiseSalesSummary")
            {
                if (!string.IsNullOrWhiteSpace(hfCompanyName.Value))
                {
                    companyId = Convert.ToInt32(hfCompanyId.Value);
                }
            }

            int categoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
            int itemId = !string.IsNullOrWhiteSpace(txtHiddenItemId.Value) ? Convert.ToInt32(txtHiddenItemId.Value) : 0;
            string referNo = !string.IsNullOrWhiteSpace(this.txtReferenceNo.Text) ? this.txtReferenceNo.Text : "All";
            string tranType = this.ddlPaymentType.SelectedValue;

            int cashierInfoId = Convert.ToInt32(this.ddlCashierInfoId.SelectedValue);
            int waiterInfoId = Convert.ToInt32(this.ddlWaiterInfoId.SelectedValue);

            int sourceId = 0;
            string ddlOutletTypeVal = this.ddlOutletType.SelectedValue;
            string sourceName = string.Empty;
            string displaySourceName = string.Empty;
            string displaySourceNumber = string.Empty;
            if (ddlOutletTypeVal != "0")
            {
                if (ddlOutletTypeVal == "Table")
                {
                    sourceName = "RestaurantTable";
                    displaySourceName = "Table #";
                    sourceId = Convert.ToInt32(this.ddlTableInfoId.SelectedValue);
                    if (sourceId > 0)
                    {
                        displaySourceNumber = this.ddlTableInfoId.SelectedItem.Text;
                    }
                    else
                    {
                        displaySourceNumber = "All";
                    }
                }
                else if (ddlOutletTypeVal == "Token")
                {
                    sourceName = "RestaurantToken";
                    displaySourceName = "Take Away";
                    sourceId = 0;
                }
                else if (ddlOutletTypeVal == "Room")
                {
                    sourceName = "GuestRoom";
                    displaySourceName = "Room #";
                    sourceId = Convert.ToInt32(this.ddlRoomInfoId.SelectedValue);
                    if (sourceId > 0)
                    {
                        displaySourceNumber = this.ddlRoomInfoId.SelectedItem.Text;
                    }
                    else
                    {
                        displaySourceNumber = "All";
                    }
                }
            }

            if (cashierInfoId != 0)
            {
                userInformation = "Cashier: " + this.ddlCashierInfoId.SelectedItem.Text;
            }

            if (!string.IsNullOrEmpty(userInformation))
            {
                if (waiterInfoId != 0)
                {
                    userInformation += ", " + "Waiter: " + this.ddlWaiterInfoId.SelectedItem.Text;
                }
            }
            else
            {
                if (waiterInfoId != 0)
                {
                    userInformation += "Waiter: " + this.ddlWaiterInfoId.SelectedItem.Text;
                }
            }


            if (!string.IsNullOrEmpty(userInformation))
            {
                if (waiterInfoId != 0)
                {
                    userInformation += ", " + "Waiter: " + this.ddlWaiterInfoId.SelectedItem.Text;
                }
            }
            else
            {
                if (waiterInfoId != 0)
                {
                    userInformation += "Waiter: " + this.ddlWaiterInfoId.SelectedItem.Text;
                }
            }

            if (!string.IsNullOrEmpty(userInformation))
            {
                userInformation += ", " + displaySourceName + displaySourceNumber;
            }
            else
            {
                userInformation += displaySourceName + displaySourceNumber;
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            string paramReportType = string.Empty;
            string reportType = "";
            var reportPath1 = Server.MapPath(@"~/POS/Reports/Rdlc");

            if (this.ddlReportType.SelectedValue == "ItemWiseSalesInformation")
            {
                paramReportType = "ItemWiseSalesInformation";
                reportType = "Item Wise Sales Information";
                reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurantSalesInfo.rdlc");

                //using (XmlWriter writer = XmlWriter.Create(reportPath1 + "/ books.xml"))
                //{
                //    writer.WriteStartElement("book");
                //    writer.WriteElementString("title", "Graphics Programming using GDI+");
                //    writer.WriteElementString("author", "Mahesh Chand");
                //    writer.WriteElementString("publisher", "Addison-Wesley");
                //    writer.WriteElementString("price", "64.95");
                //    writer.WriteEndElement();
                //    writer.Flush();
                //}
            }
            else if (this.ddlReportType.SelectedValue == "ItemWiseSalesSummary")
            {
                paramReportType = "ItemWiseSalesSummary";
                reportType = "Item Wise Sales Summary";
                reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurantSalesSummeryInfo.rdlc");


            }
            else if (this.ddlReportType.SelectedValue == "DateWiseSalesInformation")
            {
                paramReportType = "DateWiseSalesInformation";
                reportType = "Date Wise Sales Information";
                reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptDateWiseSalesInfo.rdlc");
            }
            else if (this.ddlReportType.SelectedValue == "DateWiseSalesSummary")
            {
                paramReportType = "DateWiseSalesSummary";
                reportType = "Date Wise Sales Summary";
                reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptDateWiseSalesSummary.rdlc");
            }
            //else if (this.ddlReportType.SelectedValue == "QtyGraph")
            //{
            //    paramReportType = "QtyGraph";
            //    reportType = "Slaes Item Quantity Graph";
            //    reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurantSalesQtyGraphInfo.rdlc");
            //}
            //else if (this.ddlReportType.SelectedValue == "NetSalesGraph")
            //{
            //    paramReportType = "NetSalesGraph";
            //    reportType = "Net Sales Graph";
            //    reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurantNetSalesGraphInfo.rdlc");
            //}
            else if (this.ddlReportType.SelectedValue == "FNBSales")
            {
                paramReportType = "FNBSales";
                reportType = "Food & Beverage Sales Information";
                reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurantSalesFNBInfo.rdlc");
            }
            else if (this.ddlReportType.SelectedValue == "CompanyWiseSalesSummary")
            {
                //companyId = Convert.ToInt32(hfCompanyId.Value);
                paramReportType = "CompanyWiseSalesSummary";
                reportType = "Company Wise Sales Summary";
                reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptCompanyWiseSalesSummary.rdlc");
            }

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();

            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramReport.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            string reportName = "" + reportType + "";
            if (this.ddlFilterBy.SelectedValue == "All")
            {
                reportName = "" + reportType + "";
            }
            else if (this.ddlFilterBy.SelectedValue == "Room")
            {
                reportName = "" + reportType + " (Room Sales)";
            }
            else
            {
                reportName = "" + reportType + "" + " (" + this.ddlFilterBy.SelectedValue + ")";
            }

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("FromDate", startDate));
            paramReport.Add(new ReportParameter("ToDate", endDate));
            if (!string.IsNullOrWhiteSpace(hfCostcenterListInfo.Value))
            {
                paramReport.Add(new ReportParameter("CostCenterInfo", hfCostcenterListInfo.Value));
            }
            else
            {
                paramReport.Add(new ReportParameter("CostCenterInfo", "All"));
            }

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO isRestaurantBillAmountWillRoundBO = new HMCommonSetupBO();
            isRestaurantBillAmountWillRoundBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillAmountWillRound", "IsRestaurantBillAmountWillRound");

            if (isRestaurantBillAmountWillRoundBO != null)
            {
                paramReport.Add(new ReportParameter("IsRestaurantBillAmountWillRound", isRestaurantBillAmountWillRoundBO.SetupValue));
            }

            RestaurantSalesInfoDA salesDA = new RestaurantSalesInfoDA();
            //List<SalesInfoReportViewBO> guestPaymentSummery = new List<SalesInfoReportViewBO>();
            //guestPaymentSummery = salesDA.GetGuestPaymentSummeryInfoForReport(FromDate, ToDate, costcenterIdList, -1);
            //if (guestPaymentSummery != null)
            //{
            //    paramReport.Add(new ReportParameter("GuestPaymentSummeryInfo", guestPaymentSummery[0].GuestTotalPaymentInformation));
            //}
            //else
            //{
            //    paramReport.Add(new ReportParameter("GuestPaymentSummeryInfo", ""));
            //}
            paramReport.Add(new ReportParameter("GuestPaymentSummeryInfo", ""));

            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("ReportName", reportName));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("UserInformation", userInformation));

            rvTransaction.LocalReport.SetParameters(paramReport);

            List<SalesInfoReportViewBO> salesList = new List<SalesInfoReportViewBO>();

            //if (FromDate.Date == ToDate.Date)
            //{
            //    Boolean success = salesDA.RestaurantSalesNPaymentAdjustment(paramReportType, FromDate, ToDate, costcenterIdList, categoryId, itemId, referNo, tranType);
            //}

            if (paramReportType != "FNBSales")
            {
                salesList = salesDA.GetSalesRestaurantInfoForReport(paramReportType, FromDate, ToDate, costcenterIdList, categoryId, itemId, referNo, tranType, cashierInfoId, waiterInfoId, sourceName, sourceId, companyId);
            }
            else if (paramReportType == "FNBSales")
            {
                salesList = salesDA.GetSalesRestaurantInfoForReport(paramReportType, FromDate, ToDate, costcenterIdList, categoryId, itemId, referNo, tranType, Convert.ToInt32(ddlInvItemClassification.SelectedValue), cashierInfoId, waiterInfoId, sourceName, sourceId);
            }

            //xml write

            //if (this.ddlReportType.SelectedValue == "ItemWiseSalesInformation")
            //{
            //    using (XmlWriter writer = XmlWriter.Create(reportPath1 + "/ItemWiseSalesInformation"+ FromDate.ToString("dd-MM-yyyy-hh-mm-ss-tt")+"TO"+ ToDate.ToString("dd-MM-yyyy-hh-mm-ss-tt") + ".xml"))
            //    {
            //        writer.WriteStartElement("Report");

            //        writer.WriteStartElement("CompanyInformation");

            //        writer.WriteElementString("CompanyName", files[0].CompanyName);
            //        writer.WriteElementString("CompanyAddress", files[0].CompanyAddress);
            //        writer.WriteElementString("WebAddress", files[0].WebAddress);

            //        writer.WriteEndElement();

            //        writer.WriteStartElement("ReportInfrormation");
            //        //writer.WriteStartElement("Infrormation");

            //        writer.WriteElementString("ReportName", this.ddlReportType.SelectedValue);

            //        if (!string.IsNullOrWhiteSpace(hfCostcenterListInfo.Value))
            //        {
            //            //paramReport.Add(new ReportParameter("CostCenter", hfCostcenterListInfo.Value));
            //            writer.WriteElementString("CostCenter", hfCostcenterListInfo.Value);
            //        }
            //        else
            //        {
            //            //paramReport.Add(new ReportParameter("CostCenterInfo", "All"));
            //            writer.WriteElementString("CostCenter", "All");
            //        }


            //        writer.WriteElementString("FromDate", startDate);
            //        writer.WriteElementString("ToDate", endDate);

            //        writer.WriteEndElement();

            //        writer.WriteStartElement("ItemsInformation");
            //        for (int i =0; i< salesList.Count; i++)
            //        {
            //            writer.WriteStartElement("Item");
            //            writer.WriteElementString("CategoryName", salesList[i].CategoryName);
            //            writer.WriteElementString("ItemName", salesList[i].ServiceName);
            //            writer.WriteElementString("Date", salesList[i].ServiceDisplayDate);
            //            writer.WriteElementString("ReferenceNo", salesList[i].ReferenceNo);
            //            writer.WriteElementString("ItemQuantity", salesList[i].ItemQuantity.ToString());
            //            writer.WriteElementString("UnitRate", salesList[i].UnitRate.ToString());

            //            writer.WriteElementString("ServiceRate", salesList[i].ServiceRate.ToString());
            //            writer.WriteElementString("ServiceCharge", salesList[i].ServiceCharge.ToString());
            //            writer.WriteElementString("SDCharge", salesList[i].CitySDCharge.ToString());

            //            writer.WriteElementString("Vat", salesList[i].VatAmount.ToString());
            //            writer.WriteElementString("AdditionalCharge", salesList[i].AdditionalCharge.ToString());
            //            writer.WriteElementString("TotalAmount", salesList[i].TotalAmount.ToString());
            //            writer.WriteEndElement();
            //        }

            //        writer.WriteEndElement();
            //        writer.WriteEndElement();
            //        writer.Flush();
            //    }

            //}

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salesList));

            rvTransaction.LocalReport.DisplayName = "" + reportType + " Information";
            rvTransaction.LocalReport.Refresh();

        }
        private void IsRestaurantReportRestrictionForAllUser()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO commonSetupBOIsRestaurantBillByCashier = new HMCommonSetupBO();
            commonSetupBOIsRestaurantBillByCashier = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillByCashier", "IsRestaurantBillByCashier");
            if (commonSetupBOIsRestaurantBillByCashier != null)
            {
                if (commonSetupBOIsRestaurantBillByCashier.SetupId > 0)
                {
                    if (commonSetupBOIsRestaurantBillByCashier.SetupValue == "1")
                    {
                        HMCommonSetupBO commonSetupBORescitionForAllUsers = new HMCommonSetupBO();

                        commonSetupBORescitionForAllUsers = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantReportRestrictionForAllUser", "IsRestaurantReportRestrictionForAllUser");

                        if (commonSetupBORescitionForAllUsers != null)
                        {
                            if (commonSetupBORescitionForAllUsers.SetupId > 0)
                            {
                                if (commonSetupBORescitionForAllUsers.SetupValue == "1")
                                {
                                    isRestaurantReportRestrictionForAllUser = true;
                                }
                                else
                                {
                                    isRestaurantReportRestrictionForAllUser = false;
                                }
                            }
                            else
                            {
                                isRestaurantReportRestrictionForAllUser = false;
                            }
                        }
                        else
                        {
                            isRestaurantReportRestrictionForAllUser = false;
                        }
                    }
                }
            }
            else
            {
                isRestaurantReportRestrictionForAllUser = false;
            }
        }
        private void LoadCostCenter()
        {
            Boolean IsAdminUser = false;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                IsAdminUser = true;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 6).Count() > 0)
                    {
                        IsAdminUser = true;
                    }
                }
            }
            #endregion

            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> entityListBO = new List<CostCentreTabBO>();

            if (IsAdminUser)
            {
                entityListBO = costCentreTabDA.GetAllRestaurantTypeCostCentreTabInfo().Where(x => x.IsRestaurant == true).ToList();
            }
            else
            {
                if (!isRestaurantReportRestrictionForAllUser)
                {
                    entityListBO = costCentreTabDA.GetAllRestaurantTypeCostCentreTabInfo().Where(x => x.IsRestaurant == true).ToList();
                }
                else
                {
                    entityListBO = costCentreTabDA.GetRestaurantTypeCostCentreTabInfo(userInformationBO.UserId, userInformationBO.UserInfoId, 0).Where(x => x.IsRestaurant == true).ToList();
                }
            }
            string grid = GetHTMLCostcenterGridView(entityListBO);
            ltCostcenter.Text = grid;
        }
        private void LoadRestaurantUserInfo()
        {
            RestaurantBearerDA costCentreTabDA = new RestaurantBearerDA();

            this.ddlCashierInfoId.DataSource = costCentreTabDA.GetRestaurantUserInfo("Cashier");
            this.ddlCashierInfoId.DataTextField = "UserName";
            this.ddlCashierInfoId.DataValueField = "UserInfoId";
            this.ddlCashierInfoId.DataBind();

            this.ddlWaiterInfoId.DataSource = costCentreTabDA.GetRestaurantUserInfo("All");
            this.ddlWaiterInfoId.DataTextField = "UserName";
            this.ddlWaiterInfoId.DataValueField = "UserInfoId";
            this.ddlWaiterInfoId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCashierInfoId.Items.Insert(0, item);
            this.ddlWaiterInfoId.Items.Insert(0, item);
        }
        private void LoadRoomInformation()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            this.ddlRoomInfoId.DataSource = roomNumberDA.GetRoomNumberInfo();
            this.ddlRoomInfoId.DataTextField = "RoomNumber";
            this.ddlRoomInfoId.DataValueField = "RoomId";
            this.ddlRoomInfoId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlRoomInfoId.Items.Insert(0, item);
        }
        private void LoadTableInformation()
        {
            RestaurantTableDA roomNumberDA = new RestaurantTableDA();
            this.ddlTableInfoId.DataSource = roomNumberDA.GetRestaurantTableInfo();
            this.ddlTableInfoId.DataTextField = "TableNumber";
            this.ddlTableInfoId.DataValueField = "TableId";
            this.ddlTableInfoId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlTableInfoId.Items.Insert(0, item);
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
                        ddlOutletType.Items.Remove(ddlOutletType.Items.FindByValue("Room"));
                    }
                }
            }
        }
        private void CheckIfFoodNBeverageSalesRelatedDataHide()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsFoodNBeverageSalesRelatedDataHide", "IsFoodNBeverageSalesRelatedDataHide");

            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        ddlReportType.Items.Remove(ddlOutletType.Items.FindByValue("FNBSales"));
                        hfIsFoodNBeverageSalesRelatedDataHide.Value = "1";
                    }
                }
            }
        }

        public static string GetHTMLCostcenterGridView(List<CostCentreTabBO> List)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='center' scope='col' style='width:80px;' >Select</th><th align='left' scope='col'>Cost Center</th></tr>";
            int counter = 0;
            foreach (CostCentreTabBO dr in List)
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

                strTable += "<td align='center' style='width:30px'>";
                strTable += "&nbsp;<input type='checkbox'  id='" + dr.CostCenterId + "' name='" + dr.CostCenter + "' value='" + dr.CostCenterId + "' >";

                strTable += "</td><td align='left' style='width:150px'>" + dr.CostCenter + "</td></tr>";

            }

            strTable += "</div> </td> </tr> </table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            return strTable;
        }
        private void FilterByItem()
        {
            ddlFilterBy.Items.Add(new ListItem("--- All ---", "All"));
            ddlFilterBy.Items.Add(new ListItem("Room Sale", "Room"));
            ddlFilterBy.Items.Add(new ListItem("Cash Sale", "Cash"));
            ddlFilterBy.Items.Add(new ListItem("Card Sale", "Card"));
            ddlFilterBy.Items.Add(new ListItem("Company Sales", "Company"));
            //ddlFilterBy.Items.Add(new ListItem("Cheque Sale", "ChequeSale"));
        }
        private void LoadServiceInfo()
        {

            HotelGuestServiceInfoDA serviceDA = new HotelGuestServiceInfoDA();
            List<HotelGuestServiceInfoBO> list = new List<HotelGuestServiceInfoBO>();
            list = serviceDA.GetAllGuestServiceInfo();
            ddlServiceId.DataSource = list;
            ddlServiceId.DataValueField = "ServiceId";
            ddlServiceId.DataTextField = "ServiceName";
            ddlServiceId.DataBind();

            ListItem itemAll = new ListItem();
            itemAll.Value = "-1";
            itemAll.Text = "--- All ---";
            this.ddlServiceId.Items.Insert(0, itemAll);
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            //List = da.GetAllInvItemCatagoryInfoByServiceType("Product");
            List = da.GetInvCatagoryInfo();
            this.ddlCategory.DataSource = List;
            this.ddlCategory.DataTextField = "MatrixInfo";
            this.ddlCategory.DataValueField = "CategoryId";
            this.ddlCategory.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCategory.Items.Insert(0, item);

            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetInvItemInfoByCategoryId(0, 0);
            this.ddlItem.DataSource = productList;
            this.ddlItem.DataTextField = "Name";
            this.ddlItem.DataValueField = "ItemId";
            this.ddlItem.DataBind();

            this.ddlItem.Items.Insert(0, item);
        }
        //************************ User Defined Function ********************//

        [WebMethod]
        public static List<GuestCompanyBO> GetCompanyData(string searchText)
        {
            List<GuestCompanyBO> companyList = new List<GuestCompanyBO>();
            GuestCompanyDA suppDa = new GuestCompanyDA();

            companyList = suppDa.GetCompanyInfoBySearchCriteria(searchText);

            return companyList;
        }
        [WebMethod]
        public static List<InvItemBO> GetServiceByCriteria(int CategoryId)
        {
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetInvItemInfoByCategoryId(0, CategoryId);

            return productList;
        }
    }
}