using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.PurchaseManagment;

namespace HotelManagement.Presentation.Website.PurchaseManagment.Reports
{
    public partial class frmReportPMPurchaseOrder : Page
    {
        int _POrderId;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            string pOrderId = Request.QueryString["POrderId"];
            if (!String.IsNullOrEmpty(pOrderId))
            {
                this.txtReportId.Value = pOrderId;
                if (!IsPostBack)
                {
                    LoadReport(Int32.Parse(pOrderId));
                    this.SetSelected(Int32.Parse(pOrderId));
                }
            }
        }
        protected void btnChangeStatus_Click(object sender, EventArgs e)
        {
            int orderId = Int32.Parse(txtReportId.Value);
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int currentUserIs = userInformationBO.UserInfoId;
            string approvedStatus = ddlChangeStatus.SelectedItem.Text;
            PMPurchaseOrderDA orderDetailsDA = new PMPurchaseOrderDA();

            Boolean status = orderDetailsDA.UpdatePurchaseOrderStatus(orderId, approvedStatus, currentUserIs);
            if (status)
            {
                this.isMessageBoxEnable = 2;
                lblMessage.Text = "Status Change Operation Successfull";
                this.SetSelected(orderId);
            }
        }
        //************************ User Defined Function ********************//
        private void SetSelected(int POrderId)
        {
            PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
            PMPurchaseOrderDA orderDA = new PMPurchaseOrderDA();
            orderBO = orderDA.GetPMPurchaseOrderInfoByOrderId(POrderId);
            if (orderBO.POrderId > 0)
            {
                if (orderBO.ApprovedStatus.ToString() != "Approved")
                {
                    this.btnChangeStatus.Visible = true;
                    this.ddlChangeStatus.SelectedValue = orderBO.ApprovedStatus.ToString();
                }
                else
                {
                    this.btnChangeStatus.Visible = false;
                    this.ddlChangeStatus.SelectedValue = orderBO.ApprovedStatus.ToString();
                }
            }
            else
            {
                this.ddlChangeStatus.SelectedIndex = 0;
            }
        }
        private void LoadReport(int POrderId)
        {
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/rptPMPurchaseOrder.rdlc");

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

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

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            rvTransaction.LocalReport.EnableExternalImages = true;
            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();

            PMPurchaseOrderDA purchaseDa = new PMPurchaseOrderDA();
            List<PMPurchaseOrderInfoReportBO> purchaseInfo = new List<PMPurchaseOrderInfoReportBO>();
            purchaseInfo = purchaseDa.GetPMPurchaseOrderInfoForReport(POrderId);

            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], purchaseInfo));
            rvTransaction.LocalReport.DisplayName = "Purshase Order";
            rvTransaction.LocalReport.Refresh();
        }
    }
}