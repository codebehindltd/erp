using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HouseKeeping;
using HotelManagement.Entity.HouseKeeping;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using System.IO;

namespace HotelManagement.Presentation.Website.HouseKeeping.Reports
{
    public partial class frmReportHKRoomConditions : System.Web.UI.Page
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
                LoadRoomType();
                LoadHKRoomStatus();
            }
        }

        private void LoadRoomType()
        {
            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> entityBO = new List<RoomTypeBO>();
            entityBO = roomTypeDA.GetRoomTypeInfo();
            this.ddlRoomType.DataSource = entityBO;
            this.ddlRoomType.DataTextField = "RoomType";
            this.ddlRoomType.DataValueField = "RoomTypeId";
            this.ddlRoomType.DataBind();

            ddlRoomType.Items.Insert(0, new ListItem { Value = "0", Text = hmUtility.GetDropDownFirstAllValue() });
        }
        private void LoadHKRoomStatus()
        {
            HKRoomStatusDA HKRoomStatusDA = new HKRoomStatusDA();
            List<HKRoomStatusBO> files = HKRoomStatusDA.GetHKRoomStatusType();

            this.ddlHKStatus.DataSource = files;
            this.ddlHKStatus.DataTextField = "StatusName";
            this.ddlHKStatus.DataValueField = "HKRoomStatusId";
            this.ddlHKStatus.DataBind();

            ddlHKStatus.Items.Insert(0, new ListItem { Value = "0", Text = hmUtility.GetDropDownFirstAllValue() });
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            IsSuccess = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();
            int roomTypeId = 0;
            long hkStatusId = 0;

            //if (ddlRoomType.SelectedIndex == 0)
            //{
            //    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Room Type.", AlertType.Warning);                
            //    return;
            //}
            //else {

            roomTypeId = Convert.ToInt32(ddlRoomType.SelectedValue);
            hkStatusId = Convert.ToInt32(ddlHKStatus.SelectedValue);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HouseKeeping/Reports/Rdlc/rptHKRoomCondition.rdlc");

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
            viewList = hkStatusDA.GetHKRoomConditionForReport(roomTypeId, hkStatusId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList));

            rvTransaction.LocalReport.DisplayName = "Room Condition";
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