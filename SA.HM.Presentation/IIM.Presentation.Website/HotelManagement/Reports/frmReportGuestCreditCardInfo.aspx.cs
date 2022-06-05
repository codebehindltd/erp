using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.IO;
using HotelManagement.Entity.UserInformation;
using Microsoft.Reporting.WebForms;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportGuestCreditCardInfo : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int IsDispaly = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCompany();
            }
        }

        private void LoadCompany()
        {
            GuestCompanyDA companyDA = new GuestCompanyDA();
            List<GuestCompanyBO> companyList = new List<GuestCompanyBO>();
            companyList = companyDA.GetGuestCompanyInfo();

            this.ddlCompanyName.DataSource = companyList;
            this.ddlCompanyName.DataTextField = "CompanyName";
            this.ddlCompanyName.DataValueField = "CompanyId";
            this.ddlCompanyName.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCompanyName.Items.Insert(0, item);
        }

        protected void btnGenarate_Click(object sender, EventArgs e)
        {

            IsDispaly = 1;
            DateTime? fromDate;
            DateTime? toDate;
            int? companyId;
            string guestName = string.Empty, regisNo = string.Empty; 
            int roomNo = 0;

            if (!string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                fromDate = hmUtility.GetDateTimeFromString(this.txtFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                fromDate = null;
            }
            if (!string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                toDate = hmUtility.GetDateTimeFromString(this.txtToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                toDate = null;
            }

            if (ddlCompanyName.SelectedIndex != 0)
            {
                companyId = Convert.ToInt32(ddlCompanyName.SelectedValue);
            }
            else
            {
                companyId = null;
            }

            guestName = txtName.Text;
            regisNo = txtRegNo.Text;
            if (!string.IsNullOrEmpty(txtRoomNo.Text))
            {
                roomNo = Convert.ToInt32(txtRoomNo.Text);
            }

            HMCommonDA hmCommonDA = new HMCommonDA();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptCreditCardInfo.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            //-- Company Logo -------------------------------
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            List<ReportParameter> paramLogo = new List<ReportParameter>();
            paramLogo.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            paramLogo.Add(new ReportParameter("PrintDateTime", printDate));
            paramLogo.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

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
            rvTransaction.LocalReport.SetParameters(paramLogo);

            AllReportDA reportDA = new AllReportDA();
            List<RoomRegistrationBO> list = new List<RoomRegistrationBO>();
            list = reportDA.GetGuestCreditCardInfo(fromDate, toDate, companyId, guestName, regisNo, roomNo);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], list));

            rvTransaction.LocalReport.DisplayName = "Guests Credit Card Info";
            rvTransaction.LocalReport.Refresh();
        }
    }
}