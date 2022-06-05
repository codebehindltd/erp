using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using System.IO;
using Microsoft.Reporting.WebForms;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;
using HotelManagement.Data.HouseKeeping;
using HotelManagement.Entity.HouseKeeping;

namespace HotelManagement.Presentation.Website.HouseKeeping.Reports
{
    public partial class frmReportHKRoomDiscrepancy : System.Web.UI.Page
    {
        protected int IsSuccess = -1;
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadLastModifiedBy();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            IsSuccess = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();
            string dateTime = string.Empty;            

            DateTime? searchDate = null;

            string discrepancy = ddlDiscrepancy.SelectedValue;            
            if (!string.IsNullOrEmpty(txtSearchDate.Text))
            {
                dateTime = txtSearchDate.Text;
                searchDate = hmUtility.GetDateTimeFromString(dateTime, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }                        

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HouseKeeping/Reports/Rdlc/rptHKRoomDiscrepancy.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            List<ReportParameter> paramReport = new List<ReportParameter>();

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("CompanyProfile", hmUtility.GetHMCompanyProfile()));
            paramReport.Add(new ReportParameter("CompanyAddress", hmUtility.GetHMCompanyAddress()));

            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(paramReport);

            HKRoomStatusDA hkStatusDA = new HKRoomStatusDA();
            List<HKRoomStatusViewBO> viewList = new List<HKRoomStatusViewBO>();
            viewList = hkStatusDA.GetHKRoomDiscrepancyForReport(searchDate, discrepancy);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList));

            rvTransaction.LocalReport.DisplayName = "Room Discrepancy";
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

        private void LoadLastModifiedBy()
        {
            UserInformationDA userInfoDA = new UserInformationDA();
            List<UserInformationBO> userList = new List<UserInformationBO>();
            userList = userInfoDA.GetUserInformation();

            this.ddlLastModifiedBy.DataSource = userList;
            this.ddlLastModifiedBy.DataTextField = "UserName";
            this.ddlLastModifiedBy.DataValueField = "UserInfoId";
            this.ddlLastModifiedBy.DataBind();

            ddlLastModifiedBy.Items.Insert(0, new ListItem { Value = "0", Text = hmUtility.GetDropDownFirstAllValue() });
        }
    }
}