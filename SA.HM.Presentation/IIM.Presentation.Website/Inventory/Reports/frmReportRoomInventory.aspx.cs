using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;
using HotelManagement.Data;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;

namespace HotelManagement.Presentation.Website.Inventory.Reports
{
    public partial class frmReportRoomInventory : Page
    {
        int _offset = -360;
        int _mindiff = 0;
        protected int _RoomStatusInfoByDate = -1;
        private Boolean isViewPermission = false;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadRoomNumber();
                this.LoadProduct();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                this.CheckObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmReportRoomStatusHistory.ToString());
                this.LoadCurrentDate();
            }
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
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            //this.txtFromDate.Text = dateTime.ToString("MM/dd/yyyy");
            //this.txtToDate.Text = dateTime.ToString("MM/dd/yyyy");
        }
        private void LoadRoomNumber()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            this.ddlRoomId.DataSource = roomNumberDA.GetRoomNumberInfo();
            this.ddlRoomId.DataTextField = "RoomNumber";
            this.ddlRoomId.DataValueField = "RoomId";
            this.ddlRoomId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---All---";
            this.ddlRoomId.Items.Insert(0, item);
        }
        private void LoadProduct()
        {
            InvItemDA entityDA = new InvItemDA();
            this.ddlItemId.DataSource = entityDA.GetInvItemInfo();
            this.ddlItemId.DataTextField = "Name";
            this.ddlItemId.DataValueField = "ItemId";
            this.ddlItemId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---All---";
            this.ddlItemId.Items.Insert(0, item);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //var usDtfi = new CultureInfo("en-US", false).DateTimeFormat; //--MM/dd/yyyy
            //var ukDtfi = new CultureInfo("en-GB", false).DateTimeFormat; //--dd/MM/yyyy

            //var fromDate = DateTime.Now;
            //var toDate = DateTime.Now;

            //if (!string.IsNullOrEmpty(txtFromDate.Text.Trim()))
            //{
            //    fromDate = hmUtility.GetDateTimeFromString(txtFromDate.Text.Trim());
            //}

            //if (!string.IsNullOrEmpty(txtToDate.Text.Trim()))
            //{
            //    toDate = hmUtility.GetDateTimeFromString(txtToDate.Text.Trim());
            //}

            //fromDate = hmUtility.GetDateTimeFromString(fromDate.ToString());
            //toDate = hmUtility.GetDateTimeFromString(toDate.ToString());

            //_RoomStatusInfoByDate = 1;
            //ReportDataSource.SelectParameters[0].DefaultValue = fromDate.ToString();
            //ReportDataSource.SelectParameters[1].DefaultValue = toDate.AddDays(1).AddSeconds(-1).ToString();
            //ReportDataSource.SelectParameters[2].DefaultValue = this.ddlItemId.SelectedValue.ToString();
            //ReportDataSource.SelectParameters[3].DefaultValue = this.ddlRoomId.SelectedValue.ToString();
            //rvTransaction.LocalReport.Refresh();

            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else {
                startDate = txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else {
                endDate = txtToDate.Text;
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);            

            //int productId = Convert.ToInt32(this.ddlProductId.SelectedValue);
            int itemId = Convert.ToInt32(this.ddlItemId.SelectedValue);
            int roomId = Convert.ToInt32(this.ddlRoomId.SelectedValue);

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptReportRoomInventoryInformation.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

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

            //string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            //rvTransaction.LocalReport.EnableExternalImages = true;

            //paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            //paramReport.Add(new ReportParameter("PrintDateTime", DateTime.Now.ToString()));

            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            List<ReportParameter> paramReport = new List<ReportParameter>();
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(paramReport);

            RoomInventoryInformationDA roomInvInfoDA = new RoomInventoryInformationDA();
            List<RoomInvInfoReportViewBO> roomInvInfoBO = new List<RoomInvInfoReportViewBO>();
            roomInvInfoBO = roomInvInfoDA.GetRoomInventoryInfo(FromDate, ToDate, itemId, roomId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], roomInvInfoBO));

            rvTransaction.LocalReport.DisplayName = "Room Inventory Information";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
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