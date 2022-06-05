using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportEmpTraining : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int dispalyReport = -1;
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadTrainingType();
            }
        }

        private void LoadTrainingType()
        {
            EmpTrainingDA trainingDa = new EmpTrainingDA();
            List<PayrollEmpTrainingTypeBO> training = new List<PayrollEmpTrainingTypeBO>();
            training = trainingDa.GetTrainingType();

            ddlTrainingTypeId.DataSource = training;
            ddlTrainingTypeId.DataTextField = "TrainingName";
            ddlTrainingTypeId.DataValueField = "TrainingTypeId";
            ddlTrainingTypeId.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = string.Empty;
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlTrainingTypeId.Items.Insert(0, item);            
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {            
            int trainingtypeId = 0;            
            DateTime? fromDate, toDate;

            if (ddlTrainingTypeId.SelectedIndex != 0)
            {
                trainingtypeId = Convert.ToInt32(ddlTrainingTypeId.SelectedValue);
            }

            if (trainingtypeId == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Training Name.", AlertType.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                fromDate = hmUtility.GetDateTimeFromString(txtFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                fromDate = null;
            }
            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                toDate = hmUtility.GetDateTimeFromString(txtToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                toDate = null;
            }

            EmpTrainingDA empTrainingDA = new EmpTrainingDA();
            EmpTrainingBO empTrainingBO = new EmpTrainingBO();
            empTrainingBO = empTrainingDA.GetEmployeeTrainingByTrainingTypeId(trainingtypeId, fromDate, toDate);

            if (empTrainingBO == null)
            {                
                return;
            }

            dispalyReport = 1;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            rvTransaction.LocalReport.EnableExternalImages = true;

            string reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmpTraining.rdlc");

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
            reportParam.Add(new ReportParameter("TrainingName", empTrainingBO.TrainingName));
            reportParam.Add(new ReportParameter("Location", empTrainingBO.Location));
            reportParam.Add(new ReportParameter("Trainer", empTrainingBO.Trainer));
            reportParam.Add(new ReportParameter("Organizer", empTrainingBO.Organizer));
            reportParam.Add(new ReportParameter("Remarks", empTrainingBO.Remarks));
            reportParam.Add(new ReportParameter("FromDate", empTrainingBO.StartDate.ToString()));
            reportParam.Add(new ReportParameter("ToDate", empTrainingBO.EndDate.ToString()));            
            rvTransaction.LocalReport.SetParameters(reportParam);

            List<EmpTrainingDetailViewBO> viewList = new List<EmpTrainingDetailViewBO>();
            viewList = empTrainingDA.GetTrainingDetailInfoByTrainingId(empTrainingBO.TrainingId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], viewList));

            rvTransaction.LocalReport.DisplayName = "Employee Training Info";

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