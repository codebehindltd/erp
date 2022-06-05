using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.IO;
using HotelManagement.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.POS.Reports
{
    public partial class frmBillByDateRange : System.Web.UI.Page
    {
        protected int _IsReportPanelEnable = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            TransactionDataSource.SelectParameters[0].DefaultValue = DateTime.Now.ToString();
            TransactionDataSource.SelectParameters[1].DefaultValue = DateTime.Now.ToString();
            rvTransaction.LocalReport.Refresh();
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;
            string startDate = string.Empty;
            string endDate = string.Empty;
            if (string.IsNullOrWhiteSpace(this.txtStartDate.Text))
            {
                startDate = hmUtility.GetFromDate();
            }
            else
            {
                startDate = this.txtStartDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            {
                endDate = hmUtility.GetToDate();
            }
            else
            {
                endDate = this.txtEndDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            TransactionDataSource.SelectParameters[0].DefaultValue = FromDate.ToString();
            TransactionDataSource.SelectParameters[1].DefaultValue = ToDate.AddDays(1).AddSeconds(-1).ToString();
            rvTransaction.LocalReport.Refresh();
        }
    }
}