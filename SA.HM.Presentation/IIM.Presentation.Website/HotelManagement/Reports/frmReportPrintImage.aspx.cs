using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Presentation.Website.Common;


namespace HotelManagement.Presentation.Website.HotelManagement.Reports
{
    public partial class frmReportPrintImage : System.Web.UI.Page
    {
     
        int _offset = -360;
        int _mindiff = 0;
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                string queryStringId = Request.QueryString["GuestId"];
                string OwnerType = Request.QueryString["OwnerType"];
                txtGuestId.Text = queryStringId;
                txtGuestType.Text = OwnerType;
                LoadReport(queryStringId, OwnerType);
            }
        }
        public void LoadReport(string GuestId,string GuestType)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            _RoomStatusInfoByDate = 1;
            TransactionDataSource.SelectParameters[0].DefaultValue = GuestId.ToString(); ;
            TransactionDataSource.SelectParameters[1].DefaultValue = GuestType.ToString();
            rvTransaction.LocalReport.Refresh();
        }
    }
}