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
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.POS.Reports
{
    public partial class frmReportDailySalesStatement : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int _IsReportPanelEnable = -1;
        protected Boolean isRestaurantReportRestrictionForAllUser = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IsRestaurantReportRestrictionForAllUser();
                LoadCostCenter();
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
        public static string GetHTMLCostcenterGridView(List<CostCentreTabBO> List)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='text-align:center' scope='col'>Select</th><th align='left' scope='col'>Cost Center</th></tr>";
            //strTable += "<tr> <td colspan='2'>";
            //strTable += "<div style=\"height: 100%; overflow-y: scroll; text-align: left;\">";
            //strTable += "<table cellspacing='0' cellpadding='4' width='100%'>";
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

                strTable += "<td align='center'>";
                strTable += "&nbsp;<input type='checkbox'  id='" + dr.CostCenterId + "' name='" + dr.CostCenter + "' value='" + dr.CostCenterId + "' >";

                strTable += "</td><td align='left'>" + dr.CostCenter + "</td></tr>";

            }

            strTable += "</td> </tr> </table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            //strTable += "<div class='divClear'></div>";
            strTable += "<div style='margin-top:12px;'>";
            strTable += "     <button type='button' onClick='javascript:return GetCheckedCostcenter()' id='btnAddCostcenterId' class='btn btn-primary'> OK</button>";
            strTable += "     <button type='button' onclick='javascript:return CloseCostcenterDialog()' id='btnCancelCostcenterId' class='btn btn-primary'> Cancel</button>";
            strTable += "</div>";
            return strTable;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;
            int transactionType = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty, costcenterIdList = string.Empty;
            bool? complementaryOrAll = null;

            IsRestaurantReportRestrictionForAllUser();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            //CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.PDF.ToString());
            //CommonHelper.DisableReportExportFormat(rvTransaction, CommonHelper.ReportImportFormat.Word.ToString());

            hfReportTypeSelectedVal.Value = ddlReportType.SelectedValue;

            if (ddlFilterBy.SelectedValue == "1")
                complementaryOrAll = true;
            else if (ddlFilterBy.SelectedValue == "2")
                complementaryOrAll = false;
            else if (ddlFilterBy.SelectedValue == "0")
                complementaryOrAll = null;

            if (hfReportTypeSelectedVal.Value == "SingleDate")
            {
                if (string.IsNullOrWhiteSpace(this.txtFromToDate.Text))
                {
                    this.txtFromDate.Text = hmUtility.GetStringFromDateTime(DateTime.Now);
                }
                else
                {
                    this.txtFromDate.Text = this.txtFromToDate.Text;
                }
                this.txtToDate.Text = this.txtFromDate.Text;                
            }


            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
            }
            else
            {
                startDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
            }
            else
            {
                endDate = this.txtToDate.Text;
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            //if (!string.IsNullOrEmpty(hfCostcenterId.Value))
            //    costcenterId = hfCostcenterId.Value;

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

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";

            if (hfReportTypeSelectedVal.Value == "DateRange")
            {                
                reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptDailySalesStatementNew.rdlc");
            }
            else if (hfReportTypeSelectedVal.Value == "SingleDate")
            { 
                reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptDailySalesStatementSummaryNew.rdlc");                
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
            
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            //List<ReportParameter> param2 = new List<ReportParameter>();
            //List<ReportParameter> param3 = new List<ReportParameter>();

            paramReport.Add(new ReportParameter("FromDate", startDate));
            paramReport.Add(new ReportParameter("ToDate", endDate));
            //rvTransaction.LocalReport.SetParameters(param2);
            //rvTransaction.LocalReport.SetParameters(param3);

            string reportName = "Daily Sales Statement";
            //List<ReportParameter> paramReportName = new List<ReportParameter>();
            paramReport.Add(new ReportParameter("ReportName", reportName));
            //rvTransaction.LocalReport.SetParameters(paramReportName);

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(paramReport);

            RestaurantSalesInfoDA salesDA = new RestaurantSalesInfoDA();
            List<DailySalesStatementBO> salesList = new List<DailySalesStatementBO>();
            DailySalesStatementViewBO salesSummary = new DailySalesStatementViewBO();

            if (hfReportTypeSelectedVal.Value == "DateRange")
            {
                transactionType = Convert.ToInt32(ddlTransactionType.SelectedValue);
                salesList = salesDA.GetSalesRestaurantInfoForDateRange(transactionType, FromDate, ToDate, costcenterIdList, ddlReportType.SelectedValue, userInformationBO.UserGroupId, complementaryOrAll, ddlPaymentType.SelectedValue);

                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salesList));
            }
            else if (hfReportTypeSelectedVal.Value == "SingleDate")
            {
                transactionType = 2;
                salesSummary = salesDA.GetSalesRestaurantInfoForSingleDate(FromDate, ToDate, costcenterIdList, ddlReportType.SelectedValue, userInformationBO.UserGroupId, complementaryOrAll, ddlPaymentType.SelectedValue);

                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salesSummary.SalesStatementSingleDate));
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[1], salesSummary.SalesStatementSummary));
            }

            rvTransaction.LocalReport.DisplayName = "Daily Sales Statement";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
    }
}