﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.Collections;
using Microsoft.Reporting.WebForms;
using System.IO;
using Newtonsoft.Json;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;
using System.Web.Security;

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class frmReportCompanyLedger : BasePage
    {
        HMUtility hmUtility = new HMUtility();

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGLCompany();
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            var reportPath = "";

            if (ddlReportType.SelectedValue == "0")
            {
                reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/RptCompanyLedger.rdlc");
            }
            else
            {
                reportPath = Server.MapPath(@"~/SalesAndMarketing/Reports/Rdlc/RptCompanyPaymentLedger.rdlc");
            }

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            string startDate = string.Empty, endDate = string.Empty, paymentStatus = string.Empty, reportType = string.Empty;
            DateTime dateTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(txtDateFrom.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtDateFrom.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtDateFrom.Text;
            }
            if (string.IsNullOrWhiteSpace(txtEndDateTo.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtEndDateTo.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtEndDateTo.Text;
            }

            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);

            Int32 companyId = Convert.ToInt32(hfCompanyId.Value);
            string companyName = ddlGLCompanyId.SelectedItem.Text;
            Int32 glCompanyId = Convert.ToInt32(ddlGLCompanyId.SelectedValue);

            List<ReportParameter> reportParam = new List<ReportParameter>();

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
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

            paymentStatus = ddlPaymentStatus.SelectedValue;
            reportType = ddlReportFor.SelectedValue;

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            reportParam.Add(new ReportParameter("CompanyName", companyName));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));

            string notesNo = string.Empty;
            string notesHead = string.Empty;
            string companyLedgerNotesHead = string.Empty;

            NodeMatrixBO companyMatrixBO = new NodeMatrixBO();
            NodeMatrixDA companyMatrixDA = new NodeMatrixDA();
            companyMatrixBO = companyMatrixDA.GetNodeMatrixInfoById(17);
            if (companyMatrixBO != null)
            {
                notesNo = companyMatrixBO.NotesNumber;
                notesHead = companyMatrixBO.NodeHead;
            }

            List<CommonPaymentModeBO> commonPaymentModeBOList = new List<CommonPaymentModeBO>();
            commonPaymentModeBOList = hmCommonDA.GetCommonPaymentModeInfo("All");
            CommonPaymentModeBO companyPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Company").FirstOrDefault();
            if (companyPaymentModeInfo != null)
            {
                int companyAccountsPostingId;
                companyAccountsPostingId = companyPaymentModeInfo.PaymentAccountsPostingId;

                companyMatrixBO = companyMatrixDA.GetNodeMatrixInfoById(companyAccountsPostingId);
                if (companyMatrixBO != null)
                {
                    companyLedgerNotesHead = companyMatrixBO.NodeHead;
                }
            }

            reportParam.Add(new ReportParameter("ReportDateFrom", startDate));
            reportParam.Add(new ReportParameter("ReportDateTo", endDate));

            reportParam.Add(new ReportParameter("NotesNo", notesNo));
            reportParam.Add(new ReportParameter("NotesHead", notesHead));
            reportParam.Add(new ReportParameter("CompanyLedgerNotesHead", companyLedgerNotesHead));

            string searchNarration = txtSearchNarration.Text;
            string fromAmount = txtFromAmount.Text;
            string toAmount = txtToAmount.Text;

            
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            rvTransaction.LocalReport.SetParameters(reportParam);

            GuestCompanyDA commonReportDa = new GuestCompanyDA();
            List<CompanyPaymentLedgerReportVwBo> companyLedger = new List<CompanyPaymentLedgerReportVwBo>();

            companyLedger = commonReportDa.GetCompanyLedger(userInformationBO.UserInfoId, glCompanyId, companyId, FromDate, ToDate, paymentStatus, reportType, searchNarration, fromAmount, toAmount);

            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], companyLedger));

            rvTransaction.LocalReport.DisplayName = "Company Ledger";
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
        private void LoadGLCompany()
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            List<GLCompanyBO> GLCompanyBOList = new List<GLCompanyBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.UserInfoId == 1)
            {
                GLCompanyBOList = entityDA.GetAllGLCompanyInfo();
            }
            else
            {
                GLCompanyBOList = entityDA.GetAllGLCompanyInfoByUserGroupId(userInformationBO.UserGroupId);
            }

            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            if (GLCompanyBOList != null)
            {
                if (GLCompanyBOList.Count == 1)
                {
                    hfIsSingleGLCompany.Value = "1";
                    companyList.Add(GLCompanyBOList[0]);
                    this.ddlGLCompanyId.DataSource = companyList;
                    this.ddlGLCompanyId.DataTextField = "Name";
                    this.ddlGLCompanyId.DataValueField = "CompanyId";
                    this.ddlGLCompanyId.DataBind();
                }
                else
                {
                    hfIsSingleGLCompany.Value = "2";
                    this.ddlGLCompanyId.DataSource = GLCompanyBOList;
                    this.ddlGLCompanyId.DataTextField = "Name";
                    this.ddlGLCompanyId.DataValueField = "CompanyId";
                    this.ddlGLCompanyId.DataBind();

                    ListItem itemCompany = new ListItem();
                    itemCompany.Value = "0";
                    itemCompany.Text = hmUtility.GetDropDownFirstAllValue();
                    this.ddlGLCompanyId.Items.Insert(0, itemCompany);
                }
            }
        }
        [WebMethod]
        public static List<GuestCompanyBO> GetCompanyData(string searchText)
        {
            List<GuestCompanyBO> companyList = new List<GuestCompanyBO>();
            GuestCompanyDA suppDa = new GuestCompanyDA();

            companyList = suppDa.GetCompanyInfoBySearchCriteria(searchText);

            return companyList;
        }

    }
}