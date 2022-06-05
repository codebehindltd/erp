using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportEmpDisciplinaryAction : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadDisciplinaryActionType();
                LoadDisciplinaryActionReason();
                LoadProposedDisciplinaryAction();
            }
        }
        private void LoadDisciplinaryActionType()
        {
            List<DisciplinaryActionTypeBO> typeList = new List<DisciplinaryActionTypeBO>();
            DisciplinaryActionDA disActionDA = new DisciplinaryActionDA();
            typeList = disActionDA.GetDisciplinaryActionTypeList();

            ddlSActionTypeId.DataSource = typeList;
            ddlSActionTypeId.DataTextField = "ActionName";
            ddlSActionTypeId.DataValueField = "DisciplinaryActionTypeId";
            ddlSActionTypeId.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSActionTypeId.Items.Insert(0, item);
        }
        private void LoadDisciplinaryActionReason()
        {
            List<DisciplinaryActionReasonBO> reasonList = new List<DisciplinaryActionReasonBO>();
            DisciplinaryActionDA disActionDA = new DisciplinaryActionDA();
            reasonList = disActionDA.GetDisciplinaryActionReasonList();

            ddlSActionreasonId.DataSource = reasonList;
            ddlSActionreasonId.DataTextField = "ActionReason";
            ddlSActionreasonId.DataValueField = "DisciplinaryActionReasonId";
            ddlSActionreasonId.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlSActionreasonId.Items.Insert(0, item);
        }
        private void LoadProposedDisciplinaryAction()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("EmpDisciplinaryProposedAction", hmUtility.GetDropDownFirstAllValue());

            ddlSProposedActionId.DataSource = fields;
            ddlSProposedActionId.DataTextField = "FieldValue";
            ddlSProposedActionId.DataValueField = "FieldId";
            ddlSProposedActionId.DataBind();
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            dispalyReport = 1;
            int empId = 0;
            int? actionType = null, actionReason = null, proposedAction = null;
            string startDate = string.Empty, endDate = string.Empty;
            DateTime dateTime = DateTime.Now;
            if (string.IsNullOrEmpty(txtStartDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtStartDate.Text;
            }
            if (string.IsNullOrEmpty(txtEndDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtEndDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);


            HiddenField hfEmployeeId = (HiddenField)this.employeeeSearch.FindControl("hfEmployeeId");
            empId = Convert.ToInt32(hfEmployeeId.Value);
            int empType = Convert.ToInt32(ddlEmployee.SelectedValue);

            if (empId == 0 && empType == 1)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "an Employee.", AlertType.Warning);
                return;
            }
            else if (empType == 0)
            {
                empId = 0;
            }

            if (ddlSActionTypeId.SelectedIndex != 0)
            {
                actionType = Convert.ToInt32(ddlSActionTypeId.SelectedValue);
            }
            if (ddlSActionreasonId.SelectedIndex != 0)
            {
                actionReason = Convert.ToInt32(ddlSActionreasonId.SelectedValue);
            }
            if (ddlSProposedActionId.SelectedIndex != 0)
            {
                proposedAction = Convert.ToInt32(ddlSProposedActionId.SelectedValue);
            }

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmpDisciplinaryAction.rdlc");

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

            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(reportParam);

            DisciplinaryActionDA disActionDA = new DisciplinaryActionDA();
            List<DisciplinaryActionReportViewBO> viewBO = new List<DisciplinaryActionReportViewBO>();
            viewBO = disActionDA.GetDisciplinaryActionForReport(actionType, actionReason, empId, proposedAction, FromDate, ToDate);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewBO));

            rvTransaction.LocalReport.DisplayName = "Termination Letter";

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