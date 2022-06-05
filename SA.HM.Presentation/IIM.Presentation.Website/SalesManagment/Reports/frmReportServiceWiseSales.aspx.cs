using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Data.Inventory;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.SalesManagment.Reports
{
    public partial class frmReportServiceWiseSales : System.Web.UI.Page
    {
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadServiceInfo();
                LoadCustomer();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                isMessageBoxEnable = 1;
                lblMessage.Text = "Please Give Start Date";
                return;
            }
            else if (string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                isMessageBoxEnable = 1;
                lblMessage.Text = "Please Give End Date";
                return;
            }

            _RoomStatusInfoByDate = 1;
            DateTime startDate = DateTime.Now, endDate = DateTime.Now;
            string reportType = string.Empty, customerId = string.Empty;
            int customerStatus = -1;

            int serviceId = 0;

            HMCommonDA hmCommonDA = new HMCommonDA();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            if (ddlReportType.SelectedValue == "CustomerDetails")
            {
                reportPath = Server.MapPath(@"~/SalesManagment/Reports/Rdlc/RptCustomerNServiceWiseSalesInfo.rdlc");
            }
            else if (ddlReportType.SelectedValue == "ServiceDetails")
            {
                reportPath = Server.MapPath(@"~/SalesManagment/Reports/Rdlc/RptServiceWiseSalesInfo.rdlc");
            }

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            //-- Company Logo -------------------------------
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            List<ReportParameter> paramLogo = new List<ReportParameter>();
            paramLogo.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            if (files[0].CompanyId > 0)
            {
                paramLogo.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramLogo.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    paramLogo.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    paramLogo.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            paramLogo.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramLogo.Add(new ReportParameter("PrintDateTime", printDate));
            rvTransaction.LocalReport.SetParameters(paramLogo);
            //-- Company Logo ------------------End----------

            serviceId = Convert.ToInt32(ddlServiceId.SelectedValue);
            startDate = hmUtility.GetDateTimeFromString(txtStartDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            endDate = hmUtility.GetDateTimeFromString(txtEndDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            reportType = ddlReportType.SelectedValue;
            customerId = ddlCustomer.SelectedValue;
            customerStatus = Convert.ToInt32(ddlCustomerStatus.SelectedValue);

            SalesServiceDA serviceDA = new SalesServiceDA();

            List<SalesServiceViewBO> serviceList = new List<SalesServiceViewBO>();
            serviceList = serviceDA.GetServiceSalesDetailsInfoForReport(serviceId, startDate, endDate, customerId, customerStatus);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], serviceList));

            rvTransaction.LocalReport.DisplayName = "Service Sales Details";

            rvTransaction.LocalReport.Refresh();
        }
        //************************ User Defined Function ********************//
        private void LoadServiceInfo()
        {
            SalesServiceDA entityDA = new SalesServiceDA();
            this.ddlServiceId.DataSource = entityDA.GetSaleServicInfo();
            this.ddlServiceId.DataTextField = "Name";
            this.ddlServiceId.DataValueField = "ServiceId";
            this.ddlServiceId.DataBind();

            ListItem itemProduct = new ListItem();
            itemProduct.Value = "0";
            itemProduct.Text = "---All---";
            this.ddlServiceId.Items.Insert(0, itemProduct);

        }

        private void LoadCustomer()
        {
            SalesCustomerDA customerDA = new SalesCustomerDA();
            List<SalesCustomerBO> salesCustomerList = new List<SalesCustomerBO>();
            salesCustomerList = customerDA.GetAllSalesCustomerInfo();

            ddlCustomer.DataSource = salesCustomerList;
            ddlCustomer.DataTextField = "Name";
            ddlCustomer.DataValueField = "CustomerId";
            ddlCustomer.DataBind();

            ddlCustomer.Items.Insert(0, new ListItem { Text = "---All---", Value = "0" });
        }
    }
}