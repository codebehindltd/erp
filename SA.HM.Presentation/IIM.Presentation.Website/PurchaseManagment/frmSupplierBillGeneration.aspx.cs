using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Membership;
using HotelManagement.Data.Membership;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.HotelManagement;
using System.Web.Services;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmSupplierBillGeneration : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadSupplierInfo();
            }
        }

        private void LoadSupplierInfo()
        {
            PMSupplierDA entityDA = new PMSupplierDA();
            ddlSupplier.DataSource = entityDA.GetPMSupplierInfo();
            ddlSupplier.DataTextField = "Name";
            ddlSupplier.DataValueField = "SupplierId";
            ddlSupplier.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSupplier.Items.Insert(0, item);
        }

        [WebMethod]
        public static List<PMSupplierPaymentLedgerBO> SupplierBillBySearch(int supplierId)
        {
            List<PMSupplierPaymentLedgerBO> supplierPaymentLedgerBO = new List<PMSupplierPaymentLedgerBO>();
            PMSupplierDA supplierDA = new PMSupplierDA();
            
            supplierPaymentLedgerBO = supplierDA.SupplierBillBySearch(supplierId);

            return supplierPaymentLedgerBO;
        }

        [WebMethod]
        public static ReturnInfo GenerateSuplierBill(List<PMSupplierPaymentLedgerBO> supplierPaymentLedger)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            PMSupplierDA supplierDA = new PMSupplierDA();

            try
            {
                rtninfo.IsSuccess = supplierDA.UpdateSupplierPaymentLedgerForBillGeneration(supplierPaymentLedger);
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                
            }

            if (rtninfo.IsSuccess)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
            }

            return rtninfo;
        }
    }
}