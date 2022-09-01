using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportGuestReferenceInfo : System.Web.UI.Page
    {
        int _offset = -360;
        int _mindiff = 0;
        HiddenField innboardMessage;
        protected int isMessageBoxEnable = -1;
        private Boolean isViewPermission = false;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                this.LoadGuestReference();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                this.CheckObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmReportGuestReferenceInfo.ToString());
            }

        }
        private void LoadGuestReference()
        {
            List<GuestReferenceBO> refferenceList = new List<GuestReferenceBO>();
            GuestReferenceDA referenceDA = new GuestReferenceDA();
            refferenceList = referenceDA.GetAllGuestRefference();
            ddlReferenceId.DataSource = refferenceList;
            ddlReferenceId.DataTextField = "Name";
            ddlReferenceId.DataValueField = "ReferenceId";
            ddlReferenceId.DataBind();

            System.Web.UI.WebControls.ListItem itemReference = new System.Web.UI.WebControls.ListItem();
            itemReference.Value = "0";
            itemReference.Text = hmUtility.GetDropDownFirstValue();
            this.ddlReferenceId.Items.Insert(0, itemReference);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlReferenceId.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Select Reference Name.", AlertType.Warning);
                ddlReferenceId.Focus();
                return;
            }
            else
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                string startDate = string.Empty;
                string endDate = string.Empty;
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
                DateTime? FromDate = null, ToDate = null;

                if (!string.IsNullOrWhiteSpace(startDate))
                    FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

                if (!string.IsNullOrWhiteSpace(endDate))
                {
                    ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);
                }

                rvTransaction.LocalReport.DataSources.Clear();
                rvTransaction.ProcessingMode = ProcessingMode.Local;

                var reportPath = "";
                reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestReferenceInfo.rdlc");

                if (!File.Exists(reportPath))
                    return;

                rvTransaction.LocalReport.ReportPath = reportPath;

                CompanyDA companyDA = new CompanyDA();
                List<CompanyBO> files = companyDA.GetCompanyInfo();

                List<ReportParameter> reportParam = new List<ReportParameter>();

                if (files[0].CompanyId > 0)
                {
                    reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                    reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
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

                reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
                reportParam.Add(new ReportParameter("FromDate", startDate));
                reportParam.Add(new ReportParameter("ToDate", endDate));
                reportParam.Add(new ReportParameter("ReferenceName", this.ddlReferenceId.SelectedItem.Text));
                reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                reportParam.Add(new ReportParameter("PrintDateTime", printDate));
                reportParam.Add(new ReportParameter("ReportType", ddlReportType.SelectedItem.Text));
                

                rvTransaction.LocalReport.SetParameters(reportParam);

                int refId = Convert.ToInt32(ddlReferenceId.SelectedValue);
                string reportType = ddlReportType.SelectedValue;

                AllReportDA reportDA = new AllReportDA();
                List<GuestRefReportViewBO> guestRefBO = new List<GuestRefReportViewBO>();
                guestRefBO = reportDA.GetGuestRefInfo(reportType, refId, FromDate, ToDate);

                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], guestRefBO));

                rvTransaction.LocalReport.DisplayName = "Guest Reference Information";
                rvTransaction.LocalReport.Refresh();

                _RoomStatusInfoByDate = 1;
            }

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Portrait.ToString());

            frmPrint.Attributes["src"] = reportSource;              
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
                    Response.Redirect("/HMCommon/frmHMReport.aspx");
                }
            }
            else
            {
                Response.Redirect("/HMCommon/frmHMReport.aspx");
            }
        }
    }
}