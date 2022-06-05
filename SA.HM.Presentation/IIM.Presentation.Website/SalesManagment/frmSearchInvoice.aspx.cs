using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.SalesManagment
{
    public partial class frmSearchInvoice : System.Web.UI.Page
    {
        int billCount;
        protected int isMessageBoxEnable = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }
        protected void gvSalesInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _salesId;
            int _InvoiceId;
            string[] arg = new string[2];
            arg = e.CommandArgument.ToString().Split(',');
            _salesId = Int32.Parse(arg[0]);
            _InvoiceId = Int32.Parse(arg[1]);
            string ss_salesId = e.CommandArgument.ToString();
            string From = "Invoice";
            if (e.CommandName == "CmdInvoice")
            {
                //string Fullurl = "Reports/frmReportPMSalesInvoice.aspx?SalesId=" + _salesId + "&From=" + From + "&InvoiceId=" + _InvoiceId;
                //OpenNewBrowserWindow(Fullurl, this);

                string url = "Reports/frmReportPMSalesInvoice.aspx?SalesId=" + _salesId + "&From=" + From + "&InvoiceId=" + _InvoiceId;
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=715,height=780,left=300,top=50,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
            }
        }
        protected void gvSalesInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                billCount = billCount + 1;
            }
        }
        //************************ User Defined Function ********************//
        private void LoadGridView()
        {
            string InvoiceNumber = txtInvoiceNumber.Text;
            string CustomerCode = txtCustomerCode.Text;
            List<PMSalesBO> list = new List<PMSalesBO>();
            PMSalesDetailsDA da = new PMSalesDetailsDA();
            list = da.GetPMSalesByInvoiceNumberAndCustomerId(InvoiceNumber, CustomerCode);
            this.gvSalesInvoice.DataSource = list;
            this.gvSalesInvoice.DataBind();
        }
        public static void OpenNewBrowserWindow(string Url, Control control)
        {
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Open", "window.open('" + Url + "');", true);
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
    }
}