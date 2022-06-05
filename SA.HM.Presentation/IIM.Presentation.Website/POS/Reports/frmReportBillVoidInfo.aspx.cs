using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.POS.Reports
{
    public partial class frmReportBillVoidInfo : BasePage
    {
        protected int _IsReportPanelEnable = -1;
        HMUtility hmUtility = new HMUtility();
        protected Boolean isRestaurantReportRestrictionForAllUser = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.IsRestaurantReportRestrictionForAllUser();
            }
            rvTransaction.LocalReport.EnableExternalImages = true;
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
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtFromDate.Text;
            }

            if (string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtToDate.Text;
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurantCancelBill.rdlc");

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
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            rvTransaction.LocalReport.SetParameters(paramReport);

            Boolean IsAdminUser = false;
            this.IsRestaurantReportRestrictionForAllUser();
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

            string costcenterIdList = string.Empty;
            foreach (CostCentreTabBO row in entityListBO)
            {
                if (!string.IsNullOrEmpty(costcenterIdList))
                    costcenterIdList += ", " + row.CostCenterId;
                else costcenterIdList += row.CostCenterId;
            }

            RestaurentBillDA restaurantBillDA = new RestaurentBillDA();
            List<RestaurantBillBO> restaurantCancelBillList = new List<RestaurantBillBO>();
            restaurantCancelBillList = restaurantBillDA.GetRestaurantCancelBill(costcenterIdList, FromDate, ToDate);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], restaurantCancelBillList));

            rvTransaction.LocalReport.DisplayName = "Bill Void Information";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Portrait.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
    }
}