using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HMCommon.UserControl
{
    public partial class EmployeeSearchWithBasicInfo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtEmployeeName.Text = hfEmployeeName.Value;
                txtDesignation.Text = hfEmployeeDesignation.Value;
                txtDepartment.Text = hfEmployeeDepartment.Value;
            }
        }
    }
}