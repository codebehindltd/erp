using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.SalesAndMarketing;

namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportGuestCompanyInfo : System.Web.UI.Page
    {
        int _offset = -360;
        int _mindiff = 0;
        protected int _GuestCompany = -1;
        private Boolean isViewPermission = false;
        protected int isAffDateShow = -1;
        HMUtility hmUtility = new HMUtility();
        private int isHotelGuestCompanyRescitionForAllUsers = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGuestReference();
                IsHotelGuestCompanyRescitionForAllUsers();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                this.CheckObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmReportGuestCompanyInfo.ToString());
                LoadSignupStatus();
                LoadLifeCycleStage();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            DateTime? fromDate;
            DateTime? toDate;

            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
            {
                fromDate = hmUtility.GetDateTimeFromString(this.txtStartDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                fromDate = null;
            }
            if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            {
                toDate = hmUtility.GetDateTimeFromString(this.txtEndDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                toDate = null;
            }

            string guestCmpny = txtGuestCompany.Text.ToString();
            string contactPerson = txtContactPerson.Text.ToString();
            string contactNumber = txtContactNumber.Text.ToString();
            Int32 signupStatusId = Convert.ToInt32( ddlSignupStatus.SelectedValue);
            int lifeCycleStageId = Convert.ToInt32(ddlLifeCycleStageId.SelectedValue);

            if (ddlSignupStatus.SelectedItem.Text == "Affiliated")
            {
                isAffDateShow = 1;
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/HotelManagement/Reports/Rdlc/rptGuestCompanyInfo.rdlc");

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

            _GuestCompany = 1;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;
            string billingAddrs = "";
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("BillingAddress", billingAddrs)); 
            if (ddlReferenceId.SelectedValue != "0")
            {
                paramReport.Add(new ReportParameter("Referance", ddlReferenceId.SelectedItem.Text));
            }
            else
            {
                paramReport.Add(new ReportParameter("Referance", ""));
            }
            rvTransaction.LocalReport.SetParameters(paramReport);

            AllReportDA reportDA = new AllReportDA();
            List<HotelGuestCompanyBO> gstCmpBO = new List<HotelGuestCompanyBO>();
            if (userInformationBO.UserInfoId == 1)
            {
                gstCmpBO = reportDA.GetHotelGuestCompanyInfo(guestCmpny, contactPerson, contactNumber, signupStatusId, lifeCycleStageId, fromDate, toDate);
                if (ddlReferenceId.SelectedValue != "0")
                {
                    int referenceId = !string.IsNullOrWhiteSpace(this.ddlReferenceId.SelectedValue) ? Convert.ToInt32(this.ddlReferenceId.SelectedValue) : 0;
                    gstCmpBO = gstCmpBO.Where(x => x.CompanyOwnerId == referenceId).ToList();
                }
            }
            else
            {
                if (isHotelGuestCompanyRescitionForAllUsers == 1)
                {
                    gstCmpBO = reportDA.GetHotelGuestCompanyInfo(guestCmpny, contactPerson, contactNumber, signupStatusId, lifeCycleStageId, fromDate, toDate).Where(x => x.CreatedBy == userInformationBO.UserInfoId).ToList();
                }
                else
                {
                    gstCmpBO = reportDA.GetHotelGuestCompanyInfo(guestCmpny, contactPerson, contactNumber, signupStatusId, lifeCycleStageId, fromDate, toDate);
                    if (ddlReferenceId.SelectedValue != "0")
                    {
                        int referenceId = !string.IsNullOrWhiteSpace(this.ddlReferenceId.SelectedValue) ? Convert.ToInt32(this.ddlReferenceId.SelectedValue) : 0;
                        gstCmpBO = gstCmpBO.Where(x => x.CompanyOwnerId == referenceId).ToList();
                    }
                }
            }

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], gstCmpBO));

            rvTransaction.LocalReport.DisplayName = "Company Information";
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
        //************************ User Defined Function ********************//
        private void LoadLifeCycleStage()
        {
            LifeCycleStageDA lifeCycleStageDA = new LifeCycleStageDA();
            List<SMLifeCycleStageBO> sMLifeCycles = new List<SMLifeCycleStageBO>();
            sMLifeCycles = lifeCycleStageDA.GetLifeCycleForDdl();

            ddlLifeCycleStageId.DataSource = sMLifeCycles;
            ddlLifeCycleStageId.DataTextField = "LifeCycleStage";
            ddlLifeCycleStageId.DataValueField = "Id";
            ddlLifeCycleStageId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlLifeCycleStageId.Items.Insert(0, item);
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
            itemReference.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlReferenceId.Items.Insert(0, itemReference);
        }
        private void IsHotelGuestCompanyRescitionForAllUsers()
        {
            HMCommonSetupBO commonSetupBORequisition = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBORequisition = commonSetupDA.GetCommonConfigurationInfo("IsHotelGuestCompanyRescitionForAllUsers", "IsHotelGuestCompanyRescitionForAllUsers");

            if (commonSetupBORequisition != null)
            {
                if (commonSetupBORequisition.SetupId > 0)
                {
                    if (commonSetupBORequisition.SetupValue == "1")
                    {
                        isHotelGuestCompanyRescitionForAllUsers = 1;
                        GuestReferenceDiv.Visible = false;
                    }
                }
            }
        }
        private void LoadSignupStatus()
        {
            CompanySignupStatusDA statusDA = new CompanySignupStatusDA();

            List<SMCompanySignupStatus> statusList = statusDA.GetAllSignupStatus();
            ddlSignupStatus.DataSource = statusList;
            ddlSignupStatus.DataTextField = "Status";
            ddlSignupStatus.DataValueField = "Id";
            ddlSignupStatus.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text= hmUtility.GetDropDownFirstAllValue();

            ddlSignupStatus.Items.Insert(0, item);
        }
    }
}