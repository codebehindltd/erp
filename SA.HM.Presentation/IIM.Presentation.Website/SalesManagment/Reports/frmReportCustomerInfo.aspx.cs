using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.SalesManagment.Reports
{
    public partial class frmReportCustomerInfo : System.Web.UI.Page
    {
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadCustomerInfo();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            _RoomStatusInfoByDate = 1;

            int customerId = Convert.ToInt32(this.ddlCustomer.SelectedValue);

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

            //TransactionDataSource.SelectParameters[0].DefaultValue = this.ddlCustomer.SelectedValue;
            //TransactionDataSource.SelectParameters[1].DefaultValue = this.txtCompanyName.Text;
            //TransactionDataSource.SelectParameters[2].DefaultValue = this.txtCompanyAddress.Text;
            //TransactionDataSource.SelectParameters[3].DefaultValue = this.txtCompanyWeb.Text;

            //rvTransaction.LocalReport.Refresh();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/SalesManagment/Reports/Rdlc/RptCustomerInformation.rdlc");

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

            SalesCustomerDA salesDA = new SalesCustomerDA();
            List<SalesCustomerBO> salesList = new List<SalesCustomerBO>();
            salesList = salesDA.GetSalesCustomerInfoForReport(customerId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salesList));

            rvTransaction.LocalReport.DisplayName = "Sales Information";
            rvTransaction.LocalReport.Refresh();
        }
        //************************ User Defined Function ********************//
        private void LoadCustomerInfo()
        {
            SalesCustomerDA entityDA = new SalesCustomerDA();

            this.ddlCustomer.DataSource = entityDA.GetAllSalesCustomerInfo();
            this.ddlCustomer.DataTextField = "Name";
            this.ddlCustomer.DataValueField = "CustomerId";
            this.ddlCustomer.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---All---";
            this.ddlCustomer.Items.Insert(0, item);
        }
    }
}