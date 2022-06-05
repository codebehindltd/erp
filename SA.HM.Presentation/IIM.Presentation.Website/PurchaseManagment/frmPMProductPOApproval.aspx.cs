using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;
using System.Net.Mail;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmPMProductPOApproval : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.IsPurchaseOrderCheckedByEnable();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadSearchInformation();
        }
        protected void gvOrderInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int POrderId = 0, supplierId = 0;
                POrderId = Convert.ToInt32(e.CommandArgument.ToString());

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                int approvedBy = userInformationBO.UserInfoId;
                string approvedStatus = string.Empty;
                PMPurchaseOrderDA orderDetailsDA = new PMPurchaseOrderDA();
                Boolean status = false;

                if (e.CommandName == "CmdItemPOApproved")
                {
                    approvedStatus = "Approved";
                    if (hfIsPurchaseOrderCheckedByEnable.Value == "1")
                    {
                        PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
                        orderBO = orderDetailsDA.GetPMPurchaseOrderInfoByOrderId(POrderId);
                        if (orderBO != null)
                        {
                            if (orderBO.ApprovedStatus == "Checked")
                            {
                                approvedStatus = "Approved";
                            }
                            else
                            {
                                approvedStatus = "Checked";
                            }
                        }
                    }
                    else
                    {
                        approvedStatus = "Approved";
                    }

                    status = orderDetailsDA.UpdatePurchaseOrderStatus(POrderId, approvedStatus, approvedBy);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
                        LoadSearchInformation();
                    }

                    if (!status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                }
                else if (e.CommandName == "CmdItemPOCancel")
                {
                    approvedStatus = "Cancel";
                    status = orderDetailsDA.UpdatePurchaseOrderStatus(POrderId, approvedStatus, approvedBy);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Cancel, AlertType.Success);
                        LoadSearchInformation();
                    }

                    if (!status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                }
                else if (e.CommandName == "CmdEdit")
                {
                    Session["PurchaseOrderEditId"] = POrderId.ToString();
                    Response.Redirect("frmPMProductPO.aspx");

                    //Response.Redirect("/PurchaseManagment/frmPMProductPO.aspx?POrderId=" + POrderId, false);
                }
                else if (e.CommandName == "CmdReportPO")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    Label lblSupplier = (Label)row.FindControl("lblSupplierId");

                    supplierId = Convert.ToInt32(lblSupplier.Text);

                    string url = "/PurchaseManagment/Reports/frmReportPurchaseOrderInvoice.aspx?POrderId=" + POrderId + "&SupId=" + supplierId;
                    string s = "window.open('" + url + "', 'popup_window', 'width=800,height=680,left=300,top=50,resizable=yes');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }
        }
        protected void gvOrderInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvOrderInfo.PageIndex = e.NewPageIndex;
            this.LoadSearchInformation();
        }
        protected void gvOrderInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                var item = (PMPurchaseOrderBO)e.Row.DataItem;

                ImageButton imgApproved = (ImageButton)e.Row.FindControl("ImgDetailsApproved");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgCancel = (ImageButton)e.Row.FindControl("ImgBtnCancelPO");
                ImageButton imgReportPO = (ImageButton)e.Row.FindControl("ImgReportPO");

                if (item.ApprovedStatus != "Approved")
                {
                    imgApproved.Visible = true;
                    imgCancel.Visible = true;
                    imgUpdate.Visible = true; //isSavePermission;
                    imgReportPO.Visible = false;
                }
                else
                {
                    imgApproved.Visible = false;
                    imgCancel.Visible = false;
                    imgUpdate.Visible = false;
                    imgReportPO.Visible = true;
                }
            }
        }
        //************************ User Defined Function ********************//
        private void IsPurchaseOrderCheckedByEnable()
        {
            pnlStatus.Visible = false;
            hfIsPurchaseOrderCheckedByEnable.Value = "1";
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO checkNApproveBO = new HMCommonSetupBO();
            checkNApproveBO = commonSetupDA.GetCommonConfigurationInfo("IsPurchaseOrderCheckedByEnable", "IsPurchaseOrderCheckedByEnable");
            if (Convert.ToInt32(checkNApproveBO.SetupValue) != 1)
            {
                hfIsPurchaseOrderCheckedByEnable.Value = "0";
                this.ddlStatus.Items.Remove(ddlStatus.Items.FindByValue("Checked"));
                pnlStatus.Visible = true;
            }

        }
        private void DeleteData(int pkId)
        {
            PMPurchaseOrderDA orderDetailsDA = new PMPurchaseOrderDA();
            Boolean statusApproved = orderDetailsDA.DeleteOrderDetailInfoByOrderId(pkId);
            if (statusApproved)
            {
            }
        }
        public static void OpenNewBrowserWindow(string Url, Control control)
        {
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Open", "window.open('" + Url + "');", true);
        }
        private void LoadSearchInformation()
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                fromDate = CommonHelper.DateTimeToMMDDYYYY(txtFromDate.Text);
            }

            if (!string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                toDate = CommonHelper.DateTimeToMMDDYYYY(txtToDate.Text);
            }

            string PONumber = this.txtSPONumber.Text;
            PMPurchaseOrderDA detalisDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderBO> allOrderList = new List<PMPurchaseOrderBO>();
            //allOrderList = detalisDA.GetPMPurchaseOrderInfoBySearchCriteria("Product", fromDate, toDate, PONumber, ddlStatus.SelectedValue);

            if (hfIsPurchaseOrderCheckedByEnable.Value == "1")
            {
                foreach (PMPurchaseOrderBO row in allOrderList)
                {
                    if (row.ApprovedStatus == "Submitted")
                    {
                        row.ApprovedStatus = "Pending";
                    }
                }

                List<PMPurchaseOrderBO> userWisePendingOrderList = new List<PMPurchaseOrderBO>();
                List<PMPurchaseOrderBO> userWiseCheckedOrderList = new List<PMPurchaseOrderBO>();
                List<PMPurchaseOrderBO> userWiseApprovedOrderList = new List<PMPurchaseOrderBO>();

                if (hmUtility.GetCurrentApplicationUserInfo().UserInfoId == 1)
                {
                    userWisePendingOrderList = allOrderList.Where(x => (x.ApprovedStatus == "Pending")).ToList();
                    userWiseCheckedOrderList = allOrderList.Where(x => (x.ApprovedStatus == "Checked")).ToList();
                    userWiseApprovedOrderList = allOrderList.Where(x => (x.ApprovedStatus == "Approved")).ToList();
                }
                else if (hmUtility.GetCurrentApplicationUserInfo().IsAdminUser == true)
                {
                    userWisePendingOrderList = allOrderList.Where(x => (x.ApprovedStatus == "Pending")).ToList();
                    userWiseCheckedOrderList = allOrderList.Where(x => (x.ApprovedStatus == "Checked")).ToList();
                    userWiseApprovedOrderList = allOrderList.Where(x => (x.ApprovedStatus == "Approved")).ToList();
                }
                else
                {
                    userWisePendingOrderList = allOrderList.Where(x => (x.ApprovedStatus == "Pending") && (x.CheckedBy == hmUtility.GetCurrentApplicationUserInfo().UserInfoId)).ToList();
                    userWiseCheckedOrderList = allOrderList.Where(x => (x.ApprovedStatus == "Checked") && (x.ApprovedBy == hmUtility.GetCurrentApplicationUserInfo().UserInfoId)).ToList();
                    userWiseApprovedOrderList = allOrderList.Where(x => (x.ApprovedStatus == "Approved") && (x.ApprovedBy == hmUtility.GetCurrentApplicationUserInfo().UserInfoId)).ToList();
                }

                List<PMPurchaseOrderBO> userWiseOrderList = new List<PMPurchaseOrderBO>();
                userWiseOrderList.AddRange(userWisePendingOrderList);
                userWiseOrderList.AddRange(userWiseCheckedOrderList);
                userWiseOrderList.AddRange(userWiseApprovedOrderList);

                gvOrderInfo.DataSource = userWiseOrderList;
                gvOrderInfo.DataBind();
            }
            else
            {
                gvOrderInfo.DataSource = allOrderList;
                gvOrderInfo.DataBind();
            }
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<PMPurchaseOrderDetailsBO> PerformLoadPMProductDetailOnDisplayMode(string pOrderId)
        {
            PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
            PMPurchaseOrderDA orderDetailDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderDetailsBO> orderDetailListBO = new List<PMPurchaseOrderDetailsBO>();
            orderDetailListBO = orderDetailDA.GetPMPurchaseOrderDetailByOrderId(Int32.Parse(pOrderId));
            return orderDetailListBO;
        }
    }
}