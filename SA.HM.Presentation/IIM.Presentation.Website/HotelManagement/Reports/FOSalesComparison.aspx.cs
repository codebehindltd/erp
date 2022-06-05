using HotelManagement.Data;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class FOSalesComparison : BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                FilterByItem();
                LoadServiceInfo();
                LoadRoomInformation();
            }
        }
        private void FilterByItem()
        {
            ddlFilterBy.Items.Add(new System.Web.UI.WebControls.ListItem("--- All ---", "All"));
            ddlFilterBy.Items.Add(new System.Web.UI.WebControls.ListItem("Room Sale", "Room"));
            ddlFilterBy.Items.Add(new System.Web.UI.WebControls.ListItem("Cash Sale", "Cash"));
            ddlFilterBy.Items.Add(new System.Web.UI.WebControls.ListItem("Card Sale", "Card"));
            ddlFilterBy.Items.Add(new System.Web.UI.WebControls.ListItem("Company Sales", "Company"));
            ddlFilterBy.Items.Add(new System.Web.UI.WebControls.ListItem("Employee Sales", "Employee"));
            //ddlFilterBy.Items.Add(new ListItem("Cheque Sale", "ChequeSale"));
        }
        private void LoadServiceInfo()
        {
            HotelGuestServiceInfoDA serviceDA = new HotelGuestServiceInfoDA();
            List<HotelGuestServiceInfoBO> list = new List<HotelGuestServiceInfoBO>();
            list = serviceDA.GetAllGuestServiceInfo();

            string grid = GetHTMLServiceGridView(list);
            ltServiceInformation.Text = grid;
        }
        private void LoadRoomInformation()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            ddlRoomInfoId.DataSource = roomNumberDA.GetRoomNumberInfo();
            ddlRoomInfoId.DataTextField = "RoomNumber";
            ddlRoomInfoId.DataValueField = "RoomId";
            ddlRoomInfoId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlRoomInfoId.Items.Insert(0, item);
        }
        public static string GetHTMLServiceGridView(List<HotelGuestServiceInfoBO> List)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='text-align:center' scope='col' style='width:80px;' >Select</th><th align='left' scope='col'>Service</th></tr>";

            int counter = 0;
            foreach (HotelGuestServiceInfoBO dr in List)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:White;'>";
                }

                strTable += "<td align='center'>";
                strTable += "&nbsp;<input type='checkbox'  id='" + dr.ServiceId + "' name='" + dr.ServiceName.Replace("'", "&#39;") + "' value='" + dr.ServiceId + "' >";
                strTable += "</td><td align='left'>" + dr.ServiceName + "</td></tr>";
            }

            strTable += "</td> </tr> </table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            return strTable;
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();
            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Portrait.ToString());
            frmPrint.Attributes["src"] = reportSource;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
            string startDate2 = string.Empty;
            string endDate2 = string.Empty;
            string serviceIdList = string.Empty, serviceNameList = string.Empty;

            DateTime dateTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = this.txtToDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtFromDate2.Text))
            {
                startDate2 = hmUtility.GetStringFromDateTime(dateTime);
                this.txtFromDate2.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate2 = this.txtFromDate2.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate2.Text))
            {
                endDate2 = hmUtility.GetStringFromDateTime(dateTime);
                this.txtToDate2.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate2 = this.txtToDate2.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            ToDate = ToDate.AddDays(1).AddSeconds(-1);

            DateTime FromDate2 = hmUtility.GetDateTimeFromString(startDate2, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate2 = hmUtility.GetDateTimeFromString(endDate2, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            ToDate2 = ToDate2.AddDays(1).AddSeconds(-1);

            DateTime ReportDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            string reportName = "Report";
            

            if (this.ddlFilterBy.SelectedValue == "All")
            {
                reportName = "Sales Comparison";
            }
            else if (this.ddlFilterBy.SelectedValue == "Room")
            {
                reportName = "Sales Comparison (Room Sales)";
            }
            else
            {
                reportName = "Sales Comparison" + " (" + this.ddlFilterBy.SelectedValue + ")";
            }
           
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptSalesComparison.rdlc");

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

            _RoomStatusInfoByDate = 1;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            rvTransaction.LocalReport.SetParameters(paramReport);

            List<ReportParameter> param1 = new List<ReportParameter>();
            param1.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            param1.Add(new ReportParameter("FromDate", startDate));
            param1.Add(new ReportParameter("ToDate", endDate));
            param1.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            rvTransaction.LocalReport.SetParameters(param1);
            List<ReportParameter> paramReportName = new List<ReportParameter>();
            paramReportName.Add(new ReportParameter("ReportName", reportName));
            rvTransaction.LocalReport.SetParameters(paramReportName);

            List<ReportParameter> paramPrintDateTime = new List<ReportParameter>();
            paramPrintDateTime.Add(new ReportParameter("PrintDateTime", printDate));
            rvTransaction.LocalReport.SetParameters(paramPrintDateTime);
            string filterBy = this.ddlFilterBy.SelectedValue;
            string transactionType = this.ddlTransactionType.SelectedValue;
            string roomNumber = ddlRoomInfoId.SelectedItem.Text;
            string roomId = ddlRoomInfoId.SelectedValue;

            if (!string.IsNullOrEmpty(hfServiceIdList.Value))
                serviceIdList = hfServiceIdList.Value;

            if (string.IsNullOrWhiteSpace(serviceIdList))
            {
                serviceIdList = "-1";
            }

            serviceNameList = hfServiceNameList.Value;
            List<ReportParameter> printData = new List<ReportParameter>();
            var printService = string.Empty;
            if (!string.IsNullOrEmpty(serviceNameList))
            {
                printService = "Service(s) Name: " + serviceNameList;
            }
            //else
            //{
            //    printData.Add(new ReportParameter("Services", ""));
            //}
            string title = "Total Sales";
            if (filterBy == "Room" && roomId != "0")
            {
                title = "Room Sales" + " ( " + roomNumber + " )";
            }
            else if (filterBy == "Room" && roomId == "0")
            {
                title = "Room Sales (All)";
            }
            printData.Add(new ReportParameter("Title", title));
            printData.Add(new ReportParameter("FromDate2", startDate2));
            printData.Add(new ReportParameter("ToDate2", endDate2));
            printData.Add(new ReportParameter("Services", printService));

            rvTransaction.LocalReport.SetParameters(printData);
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
            //roomregistrationDA.RoomNightAuditProcess(registrationIdList, DateTime.Now, 0, userInformationBO.UserInfoId);
            roomregistrationDA.RoomNightAuditProcess(registrationIdList, ToDate, 0, userInformationBO.UserInfoId);
            // // --------------Room Related Sales Information Process ----------------------END

            AllReportDA reportDA = new AllReportDA();
            ServiceSalesInfoReportViewBO salesBO = new ServiceSalesInfoReportViewBO();
            //List<ServiceSalesInfoReportViewBO> salesInfoBO = new List<ServiceSalesInfoReportViewBO>();
            //List<ServiceSalesInfoReportViewBO> salesInfoBO2 = new List<ServiceSalesInfoReportViewBO>();
            salesBO.salesInfo1 = reportDA.GetSalesInformation("SalesDetails", FromDate, ToDate, filterBy, serviceIdList, serviceNameList, transactionType);
            salesBO.salesInfo2 = reportDA.GetSalesInformation("SalesDetails", FromDate2, ToDate2, filterBy, serviceIdList, serviceNameList, transactionType);
            if (roomId != "0" )
            {
                salesBO.salesInfo1 = salesBO.salesInfo1.Where(x => x.RoomNumber == roomNumber).ToList();
                salesBO.salesInfo2 = salesBO.salesInfo2.Where(x => x.RoomNumber == roomNumber).ToList();
            }
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salesBO.salesInfo1));
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[1], salesBO.salesInfo2));

            rvTransaction.LocalReport.DisplayName = "Sales Service Comparison";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
    }
}