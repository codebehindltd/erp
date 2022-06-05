using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.SalesManagment;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.SalesManagment.Reports
{
    public partial class frmReportSalesInformation : System.Web.UI.Page
    {
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                this.LoadProduct();
                this.LoadCustomer();
            }

        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            _RoomStatusInfoByDate = 1;
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;


            if (!string.IsNullOrEmpty(txtStartDate.Text.Trim()))
            {
                startDate = hmUtility.GetDateTimeFromString(txtStartDate.Text.Trim(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            if (!string.IsNullOrEmpty(txtEndDate.Text.Trim()))
            {
                endDate = hmUtility.GetDateTimeFromString(txtEndDate.Text.Trim(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            int itemId = Convert.ToInt32(this.ddlProductId.SelectedValue);
            int customerId = Convert.ToInt32(this.ddlCustomerId.SelectedValue);

            //CompanyDA companyDA = new CompanyDA();
            //List<CompanyBO> files = companyDA.GetCompanyInfo();
            //if (files[0].CompanyId > 0)
            //{
            //    this.txtCompanyName.Text = files[0].CompanyName;
            //    this.txtCompanyAddress.Text = files[0].CompanyAddress;
            //    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
            //    {
            //        this.txtCompanyWeb.Text = files[0].WebAddress;
            //    }
            //    else
            //    {
            //        this.txtCompanyWeb.Text = files[0].ContactNumber;
            //    }
            //}
            //startDate = hmUtility.GetDateTimeFromString(startDate.ToString());
            //endDate = hmUtility.GetDateTimeFromString(endDate.ToString());
            //TransactionDataSource.SelectParameters[0].DefaultValue = startDate.ToString();
            //TransactionDataSource.SelectParameters[1].DefaultValue = endDate.AddDays(1).AddSeconds(-1).ToString();
            //TransactionDataSource.SelectParameters[2].DefaultValue = this.ddlProductId.SelectedValue;
            //TransactionDataSource.SelectParameters[3].DefaultValue = this.ddlCustomerId.SelectedValue;
            //TransactionDataSource.SelectParameters[4].DefaultValue = this.txtCompanyName.Text;
            //TransactionDataSource.SelectParameters[5].DefaultValue = this.txtCompanyAddress.Text;
            //TransactionDataSource.SelectParameters[6].DefaultValue = this.txtCompanyWeb.Text;

            //rvTransaction.LocalReport.Refresh();
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/SalesManagment/Reports/Rdlc/RptSalesInformation.rdlc");

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

            // _RoomStatusInfoByDate = 1;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(paramReport);

            SalesInformationDA salesDA = new SalesInformationDA();
            List<DateWiseSalesReportViewBO> salesBO = new List<DateWiseSalesReportViewBO>();
            salesBO = salesDA.GetSalesInfoForDateWiseSalesReport(startDate, endDate, itemId, customerId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salesBO));

            rvTransaction.LocalReport.DisplayName = "Date Wise Sales";
            rvTransaction.LocalReport.Refresh();
        }
        //************************ User Defined Function ********************//
        private void LoadProduct()
        {
            InvItemDA productDA = new InvItemDA();
            this.ddlProductId.DataSource = productDA.GetInvItemInfo();
            this.ddlProductId.DataTextField = "Name";
            this.ddlProductId.DataValueField = "ItemId";
            this.ddlProductId.DataBind();

            ListItem itemProduct = new ListItem();
            itemProduct.Value = "0";
            itemProduct.Text = "---All---";
            this.ddlProductId.Items.Insert(0, itemProduct);

        }
        private void LoadCustomer()
        {
            SalesCustomerDA customerDA = new SalesCustomerDA();
            this.ddlCustomerId.DataSource = customerDA.GetAllSalesCustomerInfo();
            this.ddlCustomerId.DataTextField = "Name";
            this.ddlCustomerId.DataValueField = "CustomerId";
            this.ddlCustomerId.DataBind();

            ListItem itemCustomer = new ListItem();
            itemCustomer.Value = "0";
            itemCustomer.Text = "---All---";
            this.ddlCustomerId.Items.Insert(0, itemCustomer);

        }
    }
}