using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.IO;
using HotelManagement.Data;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportHouseKeeping : BasePage
    {
        int _offset = -360;
        int _mindiff = 0;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();         
            //CompanyDA companyDA = new CompanyDA();
            //List<CompanyBO> files = companyDA.GetCompanyInfo();
            //if (files != null)
            //{
            //    if (files.Count > 0)
            //    {
            //        this.txtCompanyName.Text = files[0].CompanyName;
            //        this.txtCompanyAddress.Text = files[0].CompanyAddress;
            //        if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
            //        {
            //            this.txtCompanyWeb.Text = files[0].WebAddress;
            //        }
            //        else
            //        {
            //            this.txtCompanyWeb.Text = files[0].ContactNumber;
            //        }
            //    }
            //}

            rvRoomStatusInfoByDate.LocalReport.DataSources.Clear();
            rvRoomStatusInfoByDate.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptHouseKeeping.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvRoomStatusInfoByDate.LocalReport.ReportPath = reportPath;

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

            _RoomStatusInfoByDate = 1;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvRoomStatusInfoByDate.LocalReport.EnableExternalImages = true;

            rvRoomStatusInfoByDate.LocalReport.SetParameters(paramReport);

            List<ReportParameter> param1 = new List<ReportParameter>();
            param1.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            //rvRoomStatusInfoByDate.LocalReport.SetParameters(param1);

            //List<ReportParameter> paramPrintDateTime = new List<ReportParameter>();
            param1.Add(new ReportParameter("PrintDateTime", printDate));
            param1.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo)); 
            rvRoomStatusInfoByDate.LocalReport.SetParameters(param1);
            
            //HouseKeepingInfoDataSource.SelectParameters[0].DefaultValue = this.txtCompanyName.Text;
            //HouseKeepingInfoDataSource.SelectParameters[1].DefaultValue = this.txtCompanyAddress.Text;
            //HouseKeepingInfoDataSource.SelectParameters[2].DefaultValue = this.txtCompanyWeb.Text;
            //HouseKeepingInfoDataSource.SelectParameters[3].DefaultValue = this.ddlCleanStatus.SelectedValue;
            //HouseKeepingInfoDataSource.SelectParameters[4].DefaultValue = this.txtRoomNumber.Text;
            string cleanUp = this.ddlCleanStatus.SelectedValue;
            string roomNumber = this.txtRoomNumber.Text;
            int roomStatus = Convert.ToInt32(this.ddlRoomStatus.SelectedValue);
            DateTime lastCleanDate = DateTime.Now.AddDays(5);
            if (!string.IsNullOrWhiteSpace(this.txtSrcLastCleanDate.Text))
            {
                lastCleanDate = hmUtility.GetDateTimeFromString(this.txtSrcLastCleanDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<HouseKeepingReportViewBO> houseKeepingBO = new List<HouseKeepingReportViewBO>();
            houseKeepingBO = roomNumberDA.GetHouseKeepingInfo(cleanUp, roomNumber, roomStatus, lastCleanDate);

            var reportDataset = rvRoomStatusInfoByDate.LocalReport.GetDataSourceNames();
            rvRoomStatusInfoByDate.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], houseKeepingBO));

            rvRoomStatusInfoByDate.LocalReport.DisplayName = "Room House Keeping Information";
            rvRoomStatusInfoByDate.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvRoomStatusInfoByDate.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
        //************************ User Defined Function ********************//
        
        //private void LoadCurrentDate()
        //{
        //    DateTime dateTime = DateTime.Now;
        //    this.txtSearchDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        //}
        //private bool Validate()
        //{

        //    bool status = true;
        //    if (string.IsNullOrWhiteSpace(this.txtSearchDate.Text))
        //    {
        //        lblMessage.Text = "Search Date must not be empty";
        //        status = false;
        //    }
        //    return status;

        //}
    }
}