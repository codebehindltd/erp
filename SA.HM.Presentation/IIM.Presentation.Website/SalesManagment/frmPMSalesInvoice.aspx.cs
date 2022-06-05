using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.SalesManagment
{
    public partial class frmPMSalesInvoice : System.Web.UI.Page
    {
        int billCount;
        int individualBillCount;
        protected int isMessageBoxEnable = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                billCount = 0;
                this.btnBillGenerate.Visible = false;
                pnlCustomerInformation.Visible = false;
            }
        }
        protected void btnBillPreview_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
            this.SetTab("Monthly");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadInvoiceDetail();
            this.SetTab("Individual");
        }
        protected void gvSalesBillGenerate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                billCount = billCount + 1;
            }

            if (billCount > 0)
            {
                this.btnBillGenerate.Visible = true;
            }
            else
            {
                this.btnBillGenerate.Visible = false;
            }
        }
        protected void btnBillGenerate_Click(object sender, EventArgs e)
        {

            PMSalesInvoiceBO entityBO = new PMSalesInvoiceBO();
            PMSalesDetailsDA entityDA = new PMSalesDetailsDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int tmpPKId = 0;
            entityBO.CreatedBy = userInformationBO.UserInfoId;
            Boolean status = entityDA.SavePMSalesInvoice(entityBO, out tmpPKId);
            if (status)
            {
                this.isMessageBoxEnable = 2;
                lblMessage.Text = "Saved Operation Successfull";
                this.LoadGridView();
                //this.LoadGridView();
                //this.Cancel();
            }

        }
        protected void gvSalesBillGenerate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _salesId;
            _salesId = Convert.ToInt32(e.CommandArgument.ToString());
            string From = "BillPreview";

            if (e.CommandName == "CmdBillGenarate")
            {
                int Id = 0;
                string Fullurl = "Reports/frmReportPMSalesInvoice.aspx?SalesId=" + _salesId + "&From=" + From + "&InvoiceId=" + Id;
                OpenNewBrowserWindow(Fullurl, this);
            }
        }
        protected void btnIndividualBillPreview_Click(object sender, EventArgs e)
        {
            this.LoadIndividualGridView();
        }
        protected void gvIndividualSalesBillGenerate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                individualBillCount = individualBillCount + 1;
            }

            if (individualBillCount > 0)
            {
                //this.btnBillGenerate.Visible = true;
            }
            else
            {
                //this.btnBillGenerate.Visible = false;
            }
        }
        protected void gvIndividualSalesBillGenerate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _salesId;
            _salesId = Convert.ToInt32(e.CommandArgument.ToString());
            string From = "IndividualBill";

            if (e.CommandName == "CmdBillGenarate")
            {
                Session["ToBillExpireDate"] = this.txtToDate.Text;
                int Id = 0;
                string Fullurl = "Reports/frmReportPMSalesInvoice.aspx?SalesId=" + _salesId + "&From=" + From + "&InvoiceId=" + Id;
                OpenNewBrowserWindow(Fullurl, this);
            }
        }
        protected void btnIndividualBillGenerate_Click(object sender, EventArgs e)
        {
            PMSalesInvoiceBO entityBO = new PMSalesInvoiceBO();
            PMSalesDetailsDA entityDA = new PMSalesDetailsDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int tmpPKId = 0;
            entityBO.CreatedBy = userInformationBO.UserInfoId;
            entityBO.BillFromDate = hmUtility.GetDateTimeFromString(this.txtToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            entityBO.CustomerId = Convert.ToInt32(this.txtCustomerId.Value);
            Boolean status = entityDA.SavePMIndividualSalesInvoice(entityBO, out tmpPKId);
            if (status)
            {
                this.isMessageBoxEnable = 2;
                lblMessage.Text = "Saved Operation Successfull";
                this.LoadIndividualGridView();
            }
        }
        //************************ User Defined Function ********************//
        private void LoadGridView()
        {
            List<PMSalesBillPreviewBO> list = new List<PMSalesBillPreviewBO>();
            PMSalesDetailsDA da = new PMSalesDetailsDA();
            list = da.GetPMSalesBillPreview();
            this.gvSalesBillGenerate.DataSource = list;
            this.gvSalesBillGenerate.DataBind();
        }
        private void SetTab(string TabName)
        {
            if (TabName == "Individual")
            {

                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "Monthly")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        private void LoadInvoiceDetail()
        {
            string InvoiceNumber = txtInvoiceNumber.Text;
            string CustomerCode = txtCustomerCode.Text;
            
            List<BillReceiveVeiwBO> viewList = new List<BillReceiveVeiwBO>();
            PMSalesBillPaymentDA paymentDA = new PMSalesBillPaymentDA();
            List<PMSalesBillPaymentBO> paymentList = new List<PMSalesBillPaymentBO>();
            viewList = paymentDA.GetInvoiceDetailsByInvoiceNumberAndCustomerCode(InvoiceNumber, CustomerCode,"");

            if (viewList != null)
            {
                if (viewList.Count > 0)
                {
                    var singleItem = viewList[0];
                    lblBillTo.Text = singleItem.BillToDate.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                    //lblBillForm.Text = singleItem.BillFromDate.ToString("dd/MM/yyyy");
                    lblBillForm.Text = singleItem.BillFromDate.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                    lblCustomerName.Text = singleItem.CustomerName.ToString();
                    lblCode.Text = singleItem.CustomerCode.ToString();
                    lblInvoiceAmount.Text = singleItem.DueOrAdvanceAmount.ToString();
                    txtCustomerId.Value = singleItem.CustomerId.ToString();
                    txtInvoiceId.Value = singleItem.InvoiceId.ToString();
                    txtHiddenFieldId.Value = singleItem.FieldId.ToString();
                    //lblFromDateValue.Text = singleItem.BillToDate.ToString("MM/dd/yyyy");
                    lblFromDateValue.Text = singleItem.BillToDate.ToString(hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                }
                if (viewList.Count > 0)
                {
                    pnlCustomerInformation.Visible = true;
                }
                else
                {
                    pnlCustomerInformation.Visible = false;
                }
            }
        }
        private void LoadIndividualGridView()
        {
            if (!string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                List<PMSalesBillPreviewBO> list = new List<PMSalesBillPreviewBO>();
                if (!string.IsNullOrWhiteSpace(txtCustomerId.Value))
                {
                    int CustomerId = Convert.ToInt32(txtCustomerId.Value);
                    DateTime fromDate = hmUtility.GetDateTimeFromString(this.lblFromDateValue.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                    DateTime toDate = hmUtility.GetDateTimeFromString(this.txtToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

                    if (fromDate <= toDate)
                    {
                        PMSalesDetailsDA da = new PMSalesDetailsDA();
                        list = da.GetPMIndividualSalesBillPreview(CustomerId, toDate);
                        this.gvIndividualSalesBillGenerate.DataSource = list;
                        this.gvIndividualSalesBillGenerate.DataBind();
                    }
                    else
                    {
                        this.isMessageBoxEnable = 1;
                        lblMessage.Text = "To Date must be gratter or equal From Date";
                        this.txtToDate.Focus();
                    }
                }
            }
            else
            {
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Enter Valid To Date";
                this.txtToDate.Focus();
            }
        }
        public static void OpenNewBrowserWindow(string Url, Control control)
        {
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Open", "window.open('" + Url + "');", true);
        }
    }
}