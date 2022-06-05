using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportGratuityStatement : BasePage
    {
        HiddenField innboardMessage;
        protected int isDisplayReport = -1;
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
        }

        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            isDisplayReport = 1;            
            bool IsManagement = false;

            if (string.IsNullOrEmpty(txtProcessDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Process Date.", AlertType.Warning);
            }
            else
            {

                DateTime processDate = DateTime.Now;

                HMCommonDA hmCommonDA = new HMCommonDA();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                //processDate = Convert.ToDateTime(txtProcessDate.Text);
                processDate = CommonHelper.DateTimeToMMDDYYYY(txtProcessDate.Text);

                if (ddlIsManagement.SelectedValue == "1")
                {
                    IsManagement = true;
                }

                EmpGratuityDA gratuityDA = new EmpGratuityDA();
                List<EmpGratuityBO> gratuityBO = new List<EmpGratuityBO>();

                gratuityBO = gratuityDA.GetGratuityStatementInfo(processDate, IsManagement); 

                rvTransaction.LocalReport.DataSources.Clear();
                rvTransaction.ProcessingMode = ProcessingMode.Local;

                var reportPath = "";
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/rptGratuityStatement.rdlc");

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

                footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

                paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
                paramReport.Add(new ReportParameter("PrintDateTime", printDate));
                paramReport.Add(new ReportParameter("TotalEmployee", gratuityBO.Count.ToString()));
                paramReport.Add(new ReportParameter("ManagementType", IsManagement == true? "MANAGEMENT" : "NON-MANAGEMENT"));
                
                string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
                rvTransaction.LocalReport.EnableExternalImages = true;

                paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));                

                rvTransaction.LocalReport.SetParameters(paramReport);

                               

                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], gratuityBO));                

                rvTransaction.LocalReport.DisplayName = "Gratuity Statement.";
                rvTransaction.LocalReport.Refresh();
            }
        }
    }
}