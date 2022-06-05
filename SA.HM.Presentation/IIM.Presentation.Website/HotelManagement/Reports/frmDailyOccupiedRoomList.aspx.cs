using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using System.IO;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Payroll;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmDailyOccupiedRoomList : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadYearList();
            }
            rvTransaction.LocalReport.EnableExternalImages = true;
        }
        private void LoadYearList()
        {
            ddlYear.DataSource = hmUtility.GetReportYearList();
            ddlYear.DataBind();
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlEffectedMonth.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + " Process Month.", AlertType.Warning);
                ddlEffectedMonth.Focus();
                return;
            }

            HMCommonDA hmCommonDA = new HMCommonDA();
            DateTime startDate = DateTime.Now, endDate = DateTime.Now;
            int year = DateTime.Now.Year, month = 1;

            if (Convert.ToInt32(ddlYear.SelectedValue) > 0)
            {
                year = Convert.ToInt32(ddlYear.SelectedValue);
            }

            if (Convert.ToInt32(ddlEffectedMonth.SelectedValue) > 0)
            {
                month = Convert.ToInt32(ddlEffectedMonth.SelectedValue);
            }

            startDate = new DateTime(year, month, 1);
            endDate = startDate.AddMonths(1).AddDays(-1);
            
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/DailyOccupiedRoomList.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            _RoomStatusInfoByDate = 1;
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
            //paramReport.Add(new ReportParameter("PrintDate", hmUtility.GetPrintDate()));
            //paramReport.Add(new ReportParameter("FromToDate", hmUtility.GetFromDateAndToDate(startDate, endDate)));

            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("month", startDate.ToString("MMM")));
            paramReport.Add(new ReportParameter("year", year.ToString()));

            rvTransaction.LocalReport.SetParameters(paramReport);

            // // --------------Room Related Sales Information Process ----------------------
            string registrationIdList = string.Empty;
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomRegistrationBO> ActiveRoomRegistrationInfoBO = new List<RoomRegistrationBO>();
            ActiveRoomRegistrationInfoBO = roomRegistrationDA.GetActiveRoomRegistrationInfo();
            foreach (RoomRegistrationBO row in ActiveRoomRegistrationInfoBO)
            {
                if (string.IsNullOrWhiteSpace(registrationIdList))
                {
                    registrationIdList = row.RegistrationId.ToString();
                }
                else if (registrationIdList == "0")
                {
                    registrationIdList = row.RegistrationId.ToString();
                }
                else
                {
                    registrationIdList += "," + row.RegistrationId.ToString();
                }
            }

            RoomRegistrationDA roomregistrationDA = new RoomRegistrationDA();
            roomregistrationDA.RoomNightAuditProcess(registrationIdList, endDate, 0, userInformationBO.UserInfoId);
            // // --------------Room Related Sales Information Process ----------------------END

            RoomStatusDA dayUseDA = new RoomStatusDA();
            List<DailyOccupiedRoomListVwBO> dayUseBO = new List<DailyOccupiedRoomListVwBO>();
            dayUseBO = dayUseDA.GetDailyOccupiedRoomList(startDate, endDate, year);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], dayUseBO));

            rvTransaction.LocalReport.DisplayName = "Daily Occupied Room Information";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Portrait.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
    }
}