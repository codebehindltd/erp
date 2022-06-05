using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmPMServicePOApproval : System.Web.UI.Page
    {
        int _OrderId;
        protected int isMessageBoxEnable = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string fromDate = string.Empty;
            string toDate = string.Empty;
            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                fromDate = hmUtility.GetFromDate();
            }
            else
            {
                fromDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                toDate = hmUtility.GetToDate();
            }
            else
            {
                toDate = this.txtToDate.Text;
            }

            string PONumber = this.txtSPONumber.Text;
            PMPurchaseOrderDA detalisDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();
            //orderList = detalisDA.GetPMPurchaseOrderInfoBySearchCriteria("Service", hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), PONumber, this.ddlStatus.SelectedItem.Text);
            gvOrderInfo.DataSource = orderList;
            gvOrderInfo.DataBind();
        }
        protected void gvOrderInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            _OrderId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                Response.Redirect("/PurchaseManagment/frmPMServicePO.aspx?POrderId=" + _OrderId, false);
            }
            else if (e.CommandName == "CmdDelete")
            {
                this.DeleteData(this._OrderId);
            }
            else if (e.CommandName == "CmdOrderPreview")
            {
                string Fullurl = "/PurchaseManagment/Reports/frmReportPMPurchaseOrder.aspx?POrderId=" + _OrderId;
                OpenNewBrowserWindow(Fullurl, this);
                this._OrderId = -1;
            }
        }
        protected void gvOrderInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvOrderInfo.PageIndex = e.NewPageIndex;
        }
        protected void gvOrderInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                Label lblApprovedStatusValue = (Label)e.Row.FindControl("lblApprovedStatus");

                if (lblApprovedStatusValue.Text != "Approved")
                {
                    imgUpdate.Visible = true;
                    imgDelete.Visible = true;
                }
                else
                {
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                }
            }
        }
        //************************ User Defined Function ********************//
        private void DeleteData(int pkId)
        {
            PMPurchaseOrderDA orderDetailsDA = new PMPurchaseOrderDA();
            Boolean statusApproved = orderDetailsDA.DeleteOrderDetailInfoByOrderId(pkId);
            if (statusApproved)
            {
                this.isMessageBoxEnable = 2;
                lblMessage.Text = "Delete Operation Successfull";
            }
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