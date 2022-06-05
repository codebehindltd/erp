using HotelManagement.Data;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.POS.Reports
{
    public partial class PosSalesComparison : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int _IsReportPanelEnable = -1;
        protected Boolean isRestaurantReportRestrictionForAllUser = false;
        protected void Page_Load(object sender, EventArgs e)
        {
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
            List = da.GetAllInvItemCatagoryInfoByServiceType("Product");
            this.ddlCategory.DataSource = List;
            this.ddlCategory.DataTextField = "MatrixInfo";
            this.ddlCategory.DataValueField = "CategoryId";
            this.ddlCategory.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCategory.Items.Insert(0, item);
        }
        [WebMethod]
        public static List<InvItemBO> GetServiceByCriteria(int CategoryId)
        {
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetInvItemInfoByCategoryId(0, CategoryId);

            return productList;
        }
        protected override void Render(HtmlTextWriter writer)
        {
            foreach (string val in ddlItem.Items)
            {
                Page.ClientScript.RegisterForEventValidation(ddlItem.UniqueID, val);
            }
            base.Render(writer);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;
            DateTime dateTime = DateTime.Now;
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
            string startDate2 = string.Empty;
            string endDate2 = string.Empty;
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

            if (string.IsNullOrWhiteSpace(this.txtFromDate2.Text))
            {
                startDate2 = hmUtility.GetStringFromDateTime(dateTime);
                this.txtFromDate2.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate2 = this.txtFromDate2.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate2.Text))
            {
                endDate2 = hmUtility.GetStringFromDateTime(dateTime);
                this.txtToDate2.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate2 = this.txtToDate2.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            ToDate = ToDate.AddDays(1).AddSeconds(-1);

            DateTime FromDate2 = hmUtility.GetDateTimeFromString(startDate2, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate2 = hmUtility.GetDateTimeFromString(endDate2, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            ToDate2 = ToDate2.AddDays(1).AddSeconds(-1);

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

            int categoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
            int itemId = !string.IsNullOrWhiteSpace(txtHiddenItemId.Value) ? Convert.ToInt32(txtHiddenItemId.Value) : 0;
            string itemName = !string.IsNullOrWhiteSpace(hfItemName.Value) ? hfItemName.Value : "";
            hfItemName.Value = "";
            txtHiddenItemId.Value = "0";
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
            if (this.ddlReportType.SelectedValue == "SalesDetail")
            {
                paramReportType = "SalesDetail";
                reportType = "Sales Detail";
                //reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurantSalesInfo.rdlc");
            }
            else if (this.ddlReportType.SelectedValue == "DateWiseSalesSummary")
            {
                paramReportType = "SalesSummery";
                reportType = "Date Wise Sales";
                //reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptDateWiseSalesSummeryInfo.rdlc");
            }
            else if (this.ddlReportType.SelectedValue == "SalesSummary")
            {
                paramReportType = "SalesSummery";
                reportType = "Sales Summary";
                //reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurantSalesSummeryInfo.rdlc");
            }
            else if (this.ddlReportType.SelectedValue == "QtyGraph")
            {
                paramReportType = "QtyGraph";
                reportType = "Slaes Item Quantity Graph";
                //reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurantSalesQtyGraphInfo.rdlc");
            }
            else if (this.ddlReportType.SelectedValue == "NetSalesGraph")
            {
                paramReportType = "NetSalesGraph";
                reportType = "Net Sales Graph";
                //reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurantNetSalesGraphInfo.rdlc");
            }
            else if (this.ddlReportType.SelectedValue == "FNBSales")
            {
                paramReportType = "FNBSales";
                reportType = "Food and Beverage Sales";
                //reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurantSalesFNBInfo.rdlc");
            }
            reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptPosSalesComparison.rdlc");
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
            
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            string reportName = " "+ reportType + " Information";
            if (this.ddlFilterBy.SelectedValue == "All")
            {
                reportName = "" + reportType + " Information";
            }
            else if (this.ddlFilterBy.SelectedValue == "Room")
            {
                reportName = "" + reportType + " Information (Room Sales)";
            }
            else
            {
                reportName = "" + reportType + " Information" + " (" + this.ddlFilterBy.SelectedValue + ")";
            }
            reportName = "POS Sales Comparison ( " + reportName + " )";
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("FromDate", startDate));
            paramReport.Add(new ReportParameter("ToDate", endDate));
            paramReport.Add(new ReportParameter("FromDate2", startDate2));
            paramReport.Add(new ReportParameter("ToDate2", endDate2));
            paramReport.Add(new ReportParameter("ItemName", itemName));
            paramReport.Add(new ReportParameter("GuestPaymentSummeryInfo", ""));
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
            paramReport.Add(new ReportParameter("ReportType", paramReportType));

            rvTransaction.LocalReport.SetParameters(paramReport);

            //List<SalesInfoReportViewBO> salesList = new List<SalesInfoReportViewBO>();
            SalesInfoReportViewBO BO = new SalesInfoReportViewBO();

            //if (FromDate.Date == ToDate.Date)
            //{
            //    Boolean success = salesDA.RestaurantSalesNPaymentAdjustment(paramReportType, FromDate, ToDate, costcenterIdList, categoryId, itemId, referNo, tranType);
            //}

            if (paramReportType != "FNBSales")
            {
                BO.salesInfoList = salesDA.GetSalesRestaurantInfoForReport(paramReportType, FromDate, ToDate, costcenterIdList, categoryId, itemId, referNo, tranType, cashierInfoId, waiterInfoId, sourceName, sourceId, 0);
                BO.salesInfoList2 = salesDA.GetSalesRestaurantInfoForReport(paramReportType, FromDate2, ToDate2, costcenterIdList, categoryId, itemId, referNo, tranType, cashierInfoId, waiterInfoId, sourceName, sourceId, 0);
            }
            else if (paramReportType == "FNBSales")
            {
                BO.salesInfoList = salesDA.GetSalesRestaurantInfoForReport(paramReportType, FromDate, ToDate, costcenterIdList, categoryId, itemId, referNo, tranType, Convert.ToInt32(ddlInvItemClassification.SelectedValue), cashierInfoId, waiterInfoId, sourceName, sourceId);
                BO.salesInfoList2 = salesDA.GetSalesRestaurantInfoForReport(paramReportType, FromDate2, ToDate2, costcenterIdList, categoryId, itemId, referNo, tranType, Convert.ToInt32(ddlInvItemClassification.SelectedValue), cashierInfoId, waiterInfoId, sourceName, sourceId);
            }

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], BO.salesInfoList));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[1], BO.salesInfoList2));

            rvTransaction.LocalReport.DisplayName = "" + reportType + " Information";
            rvTransaction.LocalReport.Refresh();
        }
    }
}