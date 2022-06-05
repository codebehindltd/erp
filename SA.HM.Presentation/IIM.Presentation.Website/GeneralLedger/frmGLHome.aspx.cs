using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmGLHome : System.Web.UI.Page
    {
        protected int isCompanyModalEnable = 1;
        protected int isMessageBoxEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            string queryStringId = Request.QueryString["ProjectId"];
            
            if (!string.IsNullOrEmpty(queryStringId))
            {
                Session.Add("GeneralLedgerProjectId", queryStringId);
                Response.Redirect("/GeneralLedger/frmVoucher.aspx");
            }
            if (!IsPostBack)
            {
                this.LoadCompanyGridView();
            }
        }
        private void LoadCompanyGridView()
        {
            //this.CheckObjectPermission();
            GLCompanyDA companyDA = new GLCompanyDA();
            List<GLCompanyBO> list = new List<GLCompanyBO>();
            list = companyDA.GetAllGLCompanyInfo();
            this.gvGLCompany.DataSource = list;
            this.gvGLCompany.DataBind();
        }
       
        protected void gvGLCompany_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            //this.lblMessage.Text = string.Empty;
            this.gvGLCompany.PageIndex = e.NewPageIndex;
            //this.CheckObjectPermission();
            this.LoadCompanyGridView();
        }
        protected void gvGLCompany_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgSelect = (ImageButton)e.Row.FindControl("ImgSelect");
                imgSelect.Attributes["onclick"] = "javascript:return PerformCompanyAction('" + lblValue.Text + "');";
            }
        }

        [WebMethod]
        public static string GenerateProjectInformation(int tableId)
        {

            GLProjectDA entityDA = new GLProjectDA();
            List<GLProjectBO> files = entityDA.GetGLProjectInfoByGLCompany(tableId);

            string strTable = "";
            strTable += "<table cellspacing='0' cellpadding='4' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col'>Project Name</th><th align='left' scope='col'>Code</th><th align='left' scope='col'>Option</th></tr>";
            int counter = 0;
            foreach (GLProjectBO dr in files)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 40%;'>" + dr.Name + "</td>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:White;'><td align='left' style='width: 40%;'>" + dr.Name + "</td>";
                }

                strTable += "<td align='left' style='width: 15%;'>" + dr.Code + "</td>";

                if (dr.ProjectId > 0)
                {
                    //strTable += "<td align='center' style='width: 15%;'><img src='../Images/select.png' onClick='javascript:return AddNewItem(" + dr.ProjectId + ")' alt='Edit Information' border='0' /></td>";
                    strTable += "<td align='center' style='width: 15%;'><a href='/GeneralLedger/frmGLHome.aspx?ProjectId=" + dr.ProjectId + "'><img src='../Images/select.png' alt='Edit Information' border='0' /></a></td>";
                }

                strTable += "</tr>";
            }
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            return strTable;
        }
    }
}