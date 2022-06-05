using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.UserInformation;
using HotelManagement.Data.HMCommon;
using System.IO;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;

using HotelManagement.Entity.HMCommon;



namespace HotelManagement.Presentation.Website.POS.Reports
{
    public partial class frmReportSalesTransaction : BasePage
    {
        protected int _IsReportPanelEnable = -1;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected Boolean isRestaurantReportRestrictionForAllUser = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                this.IsRestaurantReportRestrictionForAllUser();
                this.IsEmployeeBasicSetUpOnly();
                this.LoadCurrentDate();
                this.LoadServiceId();                
                this.LoadRestaurantUserInfo();
            }
            this.IsRestaurantIntegrateWithFrontOffice();
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
        private void IsRestaurantIntegrateWithFrontOffice()
        {
            lblReportType.Visible = true;
            ddlReportType.Visible = true;

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithFrontOffice", "IsRestaurantIntegrateWithFrontOffice");
            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        lblReportType.Visible = false;
                        ddlReportType.Visible = false;
                    }
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();
            if (!isFormValid())
            {
                return;
            }
            
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/POS/Reports/Rdlc/rptRestaurantTransaction.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            List<ReportParameter> paramReport = new List<ReportParameter>();
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("CompanyProfile", hmUtility.GetHMCompanyProfile()));
            paramReport.Add(new ReportParameter("CompanyAddress", hmUtility.GetHMCompanyAddress()));
            paramReport.Add(new ReportParameter("PrintDate", hmUtility.GetPrintDate()));
            string searchDate = "Date From: " + txtServiceFromDate.Text + " To: " + txtServiceToDate.Text;
            paramReport.Add(new ReportParameter("SearchDate", searchDate));

            string reportName = "POS Payment Transaction Report";
            if (this.ddlReportType.SelectedValue == "All")
            {
                if (this.ddlServiceId.SelectedValue == "0")
                {
                    reportName = "POS Payment Transaction Report: All Service";
                }
                else
                {
                    reportName = "POS Payment Transaction Report: " + this.ddlServiceId.SelectedItem.Text + "";
                }
            }
            else if (this.ddlReportType.SelectedValue == "InHouseGuest")
            {
                reportName = "POS Payment Transaction Report: Front Office";
            }
            else if (this.ddlReportType.SelectedValue == "OutsideGuest")
            {
                if (this.ddlServiceId.SelectedValue == "0")
                {
                    reportName = "POS Payment Transaction Report: All Service";
                }
                else
                {
                    reportName = "POS Payment Transaction Report: " + this.ddlServiceId.SelectedItem.Text + "";
                }
            }

            if (this.ddlPaymentMode.SelectedValue == "All")
            {
                reportName = reportName + " (All Payment Type)";
            }
            else
            {
                if (this.ddlPaymentMode.SelectedValue == "Other Room")
                {
                    reportName = reportName + " (Guest Room)";
                }
                else
                {
                    reportName = reportName + " (" + this.ddlPaymentMode.SelectedValue + ")";
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            RestaurantSalesInfoDA salesDA = new RestaurantSalesInfoDA();
            List<SalesInfoReportViewBO> guestPaymentSummery = new List<SalesInfoReportViewBO>();

            DateTime serviceFromDate = hmUtility.GetDateTimeFromString(txtServiceFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime serviceToDate = hmUtility.GetDateTimeFromString(txtServiceToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

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

            GuestHouseServiceDA entityDA = new GuestHouseServiceDA();
            List<Entity.HotelManagement.GuestHouseServiceBO> entityListBO = new List<Entity.HotelManagement.GuestHouseServiceBO>();

            if (IsAdminUser)
            {
                entityListBO = entityDA.GetGuestHouseRestaurantNameInfo();
            }
            else
            {
                if (!isRestaurantReportRestrictionForAllUser)
                {
                    entityListBO = entityDA.GetGuestHouseRestaurantNameInfo();
                }
                else
                {
                    entityListBO = entityDA.GetGuestHouseRestaurantNameInfoByUserInfoId(userInformationBO.UserId, userInformationBO.UserInfoId);
                }
            }

            string costcenterIdList = string.Empty;
            int serviceId = Convert.ToInt32(ddlServiceId.SelectedValue);
            if (serviceId == -1)
            {
                foreach (Entity.HotelManagement.GuestHouseServiceBO row in entityListBO)
                {
                    if (!string.IsNullOrEmpty(costcenterIdList))
                        costcenterIdList += ", " + row.ServiceId;
                    else costcenterIdList += row.ServiceId;
                }
            }
            else
            {
                costcenterIdList = ddlServiceId.SelectedValue;
            }


            hfIndividualModuleName.Value = "RestaurantModule";
            paramReport.Add(new ReportParameter("ReportName", reportName));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));

            string reportType = ddlReportType.SelectedValue;
            string paymentMode = ddlPaymentMode.SelectedValue;

            int receivedBy = Convert.ToInt32(ddlReceivedBy.SelectedValue);
            string moduleName = hfIndividualModuleName.Value;

            RestaurantSalesTransactionDA salesTransactionDA = new RestaurantSalesTransactionDA();
            List<SalesTransactionReportViewBO> salesList = new List<SalesTransactionReportViewBO>();
            salesList = salesTransactionDA.GetRestaurantTransactionInfoForReport(reportType, paymentMode, costcenterIdList, serviceFromDate, serviceToDate, receivedBy, moduleName);

            rvTransaction.LocalReport.SetParameters(paramReport);
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salesList));

            rvTransaction.LocalReport.DisplayName = "POS Payment Transaction";
            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";
        }
        private void IsEmployeeBasicSetUpOnly()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsEmployeeBasicSetUpOnly", "IsEmployeeBasicSetUpOnly");

            if (commonSetupBO.SetupValue == "1")
            {
                this.ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("InHouseGuest"));
                this.ddlPaymentMode.Items.Remove(ddlPaymentMode.Items.FindByValue("Guest Room"));
            }
        }
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtServiceFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            this.txtServiceToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        }
        private bool isFormValid()
        {
            bool status = true;
            if (string.IsNullOrEmpty(txtServiceFromDate.Text))
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please Provide From Date.";
                txtServiceFromDate.Focus();
                status = false;
            }
            else if (string.IsNullOrEmpty(txtServiceToDate.Text))
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please Provide To Date.";
                txtServiceToDate.Focus();
                status = false;
            }
            return status;
        }        
        private void LoadServiceId()
        {
            Boolean IsAdminUser = false;
            //IsRestaurantReportRestrictionForAllUser();
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

            GuestHouseServiceDA entityDA = new GuestHouseServiceDA();
            List<Entity.HotelManagement.GuestHouseServiceBO> entityListBO = new List<Entity.HotelManagement.GuestHouseServiceBO>();

            if (IsAdminUser)
            {
                entityListBO = entityDA.GetGuestHouseRestaurantNameInfo();
            }
            else
            {
                if (!isRestaurantReportRestrictionForAllUser)
                {
                    entityListBO = entityDA.GetGuestHouseRestaurantNameInfo();
                }
                else
                {
                    entityListBO = entityDA.GetGuestHouseRestaurantNameInfoByUserInfoId(userInformationBO.UserId, userInformationBO.UserInfoId);
                }
            }

            this.ddlServiceId.DataSource = entityListBO;
            this.ddlServiceId.DataTextField = "ServiceName";
            this.ddlServiceId.DataValueField = "ServiceId";
            this.ddlServiceId.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "-1";
            item.Text = "--- All ---";
            this.ddlServiceId.Items.Insert(0, item);
        }
        private void LoadUserInformation()
        {
            UserInformationDA entityDA = new UserInformationDA();
            this.ddlReceivedBy.DataSource = entityDA.GetCashierOrWaiterInformation().Where(x => x.UserGroupId == 5).ToList(); //restaurant user
            this.ddlReceivedBy.DataTextField = "UserName";
            this.ddlReceivedBy.DataValueField = "UserInfoId";
            this.ddlReceivedBy.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = "--- All ---";
            this.ddlReceivedBy.Items.Insert(0, item);
        }
        private void LoadRestaurantUserInfo()
        {
            RestaurantBearerDA costCentreTabDA = new RestaurantBearerDA();

            ddlReceivedBy.DataSource = costCentreTabDA.GetOnlyRestaurantUser();
            ddlReceivedBy.DataTextField = "UserName";
            ddlReceivedBy.DataValueField = "UserInfoId";
            ddlReceivedBy.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlReceivedBy.Items.Insert(0, item);

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

public class PaymntRefund
{
    public string PaymentMode { get; set; }
    public string PaymentDescription { get; set; }
    public decimal? Amount { get; set; }
    public decimal? AmountCash { get; set; }
    public decimal? AmountRefund { get; set; }
}