using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Membership;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Membership;
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

namespace HotelManagement.Presentation.Website.Membership.Reports
{
    public partial class MemberInformationRpt : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMember();
            }
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
        private void LoadMember()
        {
            MemMemberBasicDA MemberDA = new MemMemberBasicDA();
            this.ddlMember.DataSource = MemberDA.GetMemMemberList();
            this.ddlMember.DataTextField = "FullName";
            this.ddlMember.DataValueField = "MemberId";
            this.ddlMember.DataBind();

            ListItem itemMember = new ListItem();
            itemMember.Value = "0";
            itemMember.Text = "All";
            this.ddlMember.Items.Insert(0, itemMember);

        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _RoomStatusInfoByDate = 1;
            //Documents Info
            DocumentsDA docDA = new DocumentsDA();
            List<DocumentsBO> docBO = new List<DocumentsBO>();
            docBO = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("Member Document", Convert.ToInt32(ddlMember.SelectedValue));

            CompanyDA companyDA = new CompanyDA();
            MemMemberBasicDA  memberBasicDA = new MemMemberBasicDA();

            List<CompanyBO> files = companyDA.GetCompanyInfo();
            if (files != null)
            {
                if (files.Count > 0)
                {
                    rvTransaction.LocalReport.DataSources.Clear();
                    rvTransaction.ProcessingMode = ProcessingMode.Local;
                    rvTransaction.LocalReport.EnableExternalImages = true;

                    string reportPath = Server.MapPath(@"~/Membership/Reports/Rdlc/RptMemberInformation.rdlc");

                    if (!File.Exists(reportPath))
                        return;

                    rvTransaction.LocalReport.ReportPath = reportPath;

                    List<ReportParameter> reportParam = new List<ReportParameter>();
                    reportParam.Add(new ReportParameter("CompanyName", files[0].CompanyName));
                    reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                    {
                        reportParam.Add(new ReportParameter("CompanyWebAddress", files[0].WebAddress));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("CompanyWebAddress", files[0].ContactNumber));
                    }

                    DateTime currentDate = DateTime.Now;
                    string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
                    string footerPoweredByInfo = string.Empty;
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                    footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                    reportParam.Add(new ReportParameter("PrintDateTime", printDate));
                    reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                    if (docBO.Count == 0)
                    {
                        reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, @"/Payroll/Images/Documents/defaultempimg.png")));
                    }
                    else
                    {
                        reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, docBO[0].Path + docBO[0].Name)));
                    }
                    rvTransaction.LocalReport.SetParameters(reportParam);

                    List<MemMemberBasicsBO> memMemberBasicsBOs = new List<MemMemberBasicsBO>();
                    memMemberBasicsBOs = memberBasicDA.GetMemberInfoByIdForReport(Convert.ToInt32(ddlMember.SelectedValue));

                    var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
                    rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], memMemberBasicsBOs));

                    rvTransaction.LocalReport.DisplayName = "Member Information";

                    rvTransaction.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubReportProcessingForInvoiceDetail);
                }
            }

            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";

        }
        private void SubReportProcessingForInvoiceDetail(object sender, SubreportProcessingEventArgs e)
        {
            int Id = Convert.ToInt32(this.ddlMember.SelectedValue);
            MemMemberBasicDA MemberDA = new MemMemberBasicDA();

            
            List<OnlineMemberEducationBO> empEducation = MemberDA.GetOnlineMemberEducationsById(Id);
            e.DataSources.Add(new ReportDataSource("MemberEducation", empEducation));

            List<MemMemberFamilyMemberBO> memberFamilyBOs = MemberDA.GetMemFamilyMemberByMemberId(Id);
            e.DataSources.Add(new ReportDataSource("MemberFamily", memberFamilyBOs));

            List<MemMemberReferenceBO> memMemberReferenceBOs = MemberDA.GetMemberReferenceByMemberId(Id);
            e.DataSources.Add(new ReportDataSource("MemberReference", memMemberReferenceBOs));

        }
    }
}