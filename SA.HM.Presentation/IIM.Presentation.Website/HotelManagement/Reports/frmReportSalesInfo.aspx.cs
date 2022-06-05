using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using System.IO;
using HotelManagement.Entity.UserInformation;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportSalesInfo : BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                this.IsFOSalesReportFormatOneEnable();
                this.FilterByItem();
                this.LoadServiceInfo();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
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
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            ToDate = ToDate.AddDays(1).AddSeconds(-1);

            DateTime ReportDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            string reportName = "Transaction Report";
            if (this.ddlFilterBy.SelectedValue == "All")
            {
                reportName = "Sales Information";
            }
            else if (this.ddlFilterBy.SelectedValue == "Room")
            {
                reportName = "Sales Information (Room Sales)";
            }
            else
            {
                reportName = "Sales Information" + " (" + this.ddlFilterBy.SelectedValue + ")";
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            if (ddlReportType.SelectedValue == "SalesFormatOne")
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/FormatOneSalesInformation.rdlc");
            }
            else
            {
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/DateWiseServiceSalesInfo.rdlc");
            }

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

            if (!string.IsNullOrEmpty(hfServiceIdList.Value))
                serviceIdList = hfServiceIdList.Value;

            if (string.IsNullOrWhiteSpace(serviceIdList))
            {
                serviceIdList = "-1";
            }

            serviceNameList = hfServiceNameList.Value;

            // // --------------Room Related Sales Information Process ----------------------
            string registrationIdList = string.Empty;
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomRegistrationBO> ActiveRoomRegistrationInfoBO = new List<RoomRegistrationBO>();
            ActiveRoomRegistrationInfoBO = roomRegistrationDA.GetActiveRoomRegistrationInfo();
            foreach ( RoomRegistrationBO row in ActiveRoomRegistrationInfoBO)
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
            List<ServiceSalesInfoReportViewBO> salesInfoBO = new List<ServiceSalesInfoReportViewBO>();
            if (ddlReportType.SelectedValue == "SalesFormatOne")
            {
                salesInfoBO = reportDA.GetSalesInformation(ddlReportType.SelectedValue, FromDate, ToDate, filterBy, serviceIdList, serviceNameList, transactionType);
            }
            else
            {
                salesInfoBO = reportDA.GetSalesInformation(ddlReportType.SelectedValue, FromDate, ToDate, filterBy, serviceIdList, serviceNameList, transactionType);
            }

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], salesInfoBO));

            rvTransaction.LocalReport.DisplayName = "Sales Service Information";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        private void IsFOSalesReportFormatOneEnable()
        {
            ReportTypeDiv.Visible = false;
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isFOSalesReportFormatOneEnableBO = new HMCommonSetupBO();
            isFOSalesReportFormatOneEnableBO = commonSetupDA.GetCommonConfigurationInfo("IsFOSalesReportFormatOneEnable", "IsFOSalesReportFormatOneEnable");

            if (isFOSalesReportFormatOneEnableBO != null)
            {
                if (isFOSalesReportFormatOneEnableBO.SetupId > 0)
                {
                    if (Convert.ToInt32(isFOSalesReportFormatOneEnableBO.SetupValue) > 0)
                    {
                        ReportTypeDiv.Visible = true;
                    }
                }
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
    }
}