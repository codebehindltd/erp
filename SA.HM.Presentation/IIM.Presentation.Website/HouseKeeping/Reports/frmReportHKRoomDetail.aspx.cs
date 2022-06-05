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
using HotelManagement.Entity.HouseKeeping;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HouseKeeping;
using HotelManagement.Data.HotelManagement;

namespace HotelManagement.Presentation.Website.HouseKeeping.Reports
{
    public partial class frmReportHKRoomDetail : System.Web.UI.Page
    {
        int _offset = -360;
        int _mindiff = 0;
        protected int _RoomStatusInfoByDate = -1;
        private Boolean isViewPermission = false;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Response.Redirect("/HotelManagement/frmHMFloorManagement.aspx");
                this.LoadFORoomStatus();
                this.LoadHKRoomStatus();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                this.CheckObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmReportHKRoomDetail.ToString());
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();
            string dateTime = string.Empty;
            string shift = string.Empty;
            int? empId = null;
            string empName = string.Empty;

            DateTime? searchDate = null;

            //shift = ddlShift.SelectedValue;
            //if (ddlEmployee.SelectedIndex != 0)
            //{
            //    empId = Convert.ToInt32(ddlEmployee.SelectedValue);
            //    empName = ddlEmployee.SelectedItem.Text;
            //}
            //else
            //{
            //    empName = "All";
            //}
            //if (!string.IsNullOrEmpty(txtSearchDate.Text))
            //{
            //    dateTime = txtSearchDate.Text;
            //    searchDate = hmUtility.GetDateTimeFromString(dateTime, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            //}

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HouseKeeping/Reports/Rdlc/rptHKRoomDetail.rdlc");

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

            paramReport.Add(new ReportParameter("EmpName", empName));
            paramReport.Add(new ReportParameter("Shift", shift));
            rvTransaction.LocalReport.SetParameters(paramReport);

            int roomTypeId = 0, roomNumberId = 0, roomStatusId = Convert.ToInt32(this.ddlRoomStatus.SelectedValue), khStatusId = Convert.ToInt32(this.ddlHKRoomStatus.SelectedValue);

            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<HouseKeepingReportViewBO> houseKeepingBO = new List<HouseKeepingReportViewBO>();
            houseKeepingBO = roomNumberDA.GetHouseKeepingRoomDetailInfo(roomTypeId, roomNumberId, roomStatusId, khStatusId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], houseKeepingBO));

           

            rvTransaction.LocalReport.DisplayName = "House Keeping Room Detail Information";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";

            //HMCommonDA hmCommonDA = new HMCommonDA(); 
            //rvRoomStatusInfoByDate.LocalReport.DataSources.Clear();
            //rvRoomStatusInfoByDate.ProcessingMode = ProcessingMode.Local;

            //var reportPath = "";
            //reportPath = Server.MapPath(@"~/HouseKeeping/Reports/Rdlc/rptHKRoomDetail.rdlc");

            //if (!File.Exists(reportPath))
            //    return;

            //rvRoomStatusInfoByDate.LocalReport.ReportPath = reportPath;

            //CompanyDA companyDA = new CompanyDA();
            //List<CompanyBO> files = companyDA.GetCompanyInfo();

            //List<ReportParameter> paramReport = new List<ReportParameter>();

            //if (files[0].CompanyId > 0)
            //{
            //    paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
            //    paramReport.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

            //    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
            //    {
            //        paramReport.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
            //    }
            //    else
            //    {
            //        paramReport.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
            //    }
            //}

            //DateTime currentDate = DateTime.Now;
            //string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            //string footerPoweredByInfo = string.Empty;
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            //footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            //
            //string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            //rvRoomStatusInfoByDate.LocalReport.EnableExternalImages = true;

            //rvRoomStatusInfoByDate.LocalReport.SetParameters(paramReport);

            //List<ReportParameter> param1 = new List<ReportParameter>();
            //param1.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            ////rvRoomStatusInfoByDate.LocalReport.SetParameters(param1);

            ////List<ReportParameter> paramPrintDateTime = new List<ReportParameter>();
            //param1.Add(new ReportParameter("PrintDateTime", printDate));
            //param1.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo)); 
            //rvRoomStatusInfoByDate.LocalReport.SetParameters(param1);
            
            ////HouseKeepingInfoDataSource.SelectParameters[0].DefaultValue = this.txtCompanyName.Text;
            ////HouseKeepingInfoDataSource.SelectParameters[1].DefaultValue = this.txtCompanyAddress.Text;
            ////HouseKeepingInfoDataSource.SelectParameters[2].DefaultValue = this.txtCompanyWeb.Text;
            ////HouseKeepingInfoDataSource.SelectParameters[3].DefaultValue = this.ddlCleanStatus.SelectedValue;
            ////HouseKeepingInfoDataSource.SelectParameters[4].DefaultValue = this.txtRoomNumber.Text;
            //string cleanUp = this.ddlCleanStatus.SelectedValue;
            //string roomNumber = this.txtRoomNumber.Text;
            //int roomStatus = Convert.ToInt32(this.ddlRoomStatus.SelectedValue);
            //DateTime lastCleanDate = DateTime.Now.AddDays(5);
            //if (!string.IsNullOrWhiteSpace(this.txtSrcLastCleanDate.Text))
            //{
            //    lastCleanDate = hmUtility.GetDateTimeFromString(this.txtSrcLastCleanDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            //}
            
            //int roomTypeId = 0, roomNumberId = 0, roomStatusId = 0, khStatusId = 0;

            //RoomNumberDA roomNumberDA = new RoomNumberDA();
            //List<HouseKeepingReportViewBO> houseKeepingBO = new List<HouseKeepingReportViewBO>();
            //houseKeepingBO = roomNumberDA.GetHouseKeepingRoomDetailInfo(roomTypeId, roomNumberId, roomStatusId, khStatusId);

            //var reportDataset = rvRoomStatusInfoByDate.LocalReport.GetDataSourceNames();
            //rvRoomStatusInfoByDate.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], houseKeepingBO));

            //rvRoomStatusInfoByDate.LocalReport.DisplayName = "House Keeping Room Detail Information";
            //rvRoomStatusInfoByDate.LocalReport.Refresh();

            //frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
        //************************ User Defined Function ********************//
        private void LoadFORoomStatus()
        {
            RoomStatusDA roomStatusDA = new RoomStatusDA();
            List<RoomStatusBO> files = roomStatusDA.GetRoomStatusInfo();

            this.ddlRoomStatus.DataSource = files;
            this.ddlRoomStatus.DataTextField = "StatusName";
            this.ddlRoomStatus.DataValueField = "StatusId";
            this.ddlRoomStatus.DataBind();

            ddlRoomStatus.Items.Insert(0, new ListItem { Value = "0", Text = hmUtility.GetDropDownFirstAllValue() });
        }
        private void LoadHKRoomStatus()
        {
            HKRoomStatusDA HKRoomStatusDA = new HKRoomStatusDA();
            List<HKRoomStatusBO> files = HKRoomStatusDA.GetHKRoomStatusType();

            this.ddlHKRoomStatus.DataSource = files;
            this.ddlHKRoomStatus.DataTextField = "StatusName";
            this.ddlHKRoomStatus.DataValueField = "HKRoomStatusId";
            this.ddlHKRoomStatus.DataBind();

            ddlHKRoomStatus.Items.Insert(0, new ListItem { Value = "0", Text = hmUtility.GetDropDownFirstAllValue() });
        }
        private void CheckObjectPermission(int userId, string formName)
        {
            ObjectPermissionDA objectPermissionDA = new ObjectPermissionDA();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = objectPermissionDA.GetFormPermissionByUserIdNForm(userId, formName);
            if (objectPermissionBO.ObjectPermissionId > 0)
            {
                isViewPermission = objectPermissionBO.IsViewPermission;

                if (!isViewPermission)
                {
                    Response.Redirect("/HMCommon/frmHMHome.aspx");
                }
            }
            else
            {
                Response.Redirect("/HMCommon/frmHMHome.aspx");
            }
        }
        
    }
}