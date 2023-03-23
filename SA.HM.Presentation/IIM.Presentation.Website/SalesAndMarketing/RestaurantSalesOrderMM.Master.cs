using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class RestaurantSalesOrderMM : System.Web.UI.MasterPage
    {
        public string innBoardDateFormat = "";
        InvCategoryDA moLocation = new InvCategoryDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            SiteTitle.Text = userInformationBO.SiteTitle;
            innBoardDateFormat = userInformationBO.ClientDateFormat;

            //lblLoggedInUser.Text = userInformationBO.DisplayName;
            lblDayOpenDate.Text = userInformationBO.DayOpenDate.ToString(userInformationBO.ServerDateFormat);

            if (Session["TableInformation"] != null)
            {
                int tableId = Convert.ToInt32(Session["TableInformation"]);
            }


            //tvLocations.Attributes.Add("onclick", "return OnTreeClick(event)");
            //GetTopLevelLocations(null);
        }
        //private void GetTopLevelLocations(bool? expand)
        //{
        //    try
        //    {
        //        TreeNode oNode = null;
        //        List<InvCategoryBO> dtObjects;
        //        string selectedVal = (tvLocations.SelectedNode != null) ? tvLocations.SelectedNode.Value : string.Empty;
        //        TreeNode selectedNode = null;
        //        tvLocations.Nodes.Clear();
        //        dtObjects = moLocation.GetInvCategoryInfoByCustomString("WHERE  lvl = 0");

        //        foreach (InvCategoryBO item in dtObjects)
        //        {
        //            oNode = new TreeNode(HttpUtility.HtmlEncode(item.Name), item.CategoryId.ToString());
        //            oNode.Expanded = false;
        //            if (selectedVal == oNode.Value)
        //                selectedNode = oNode;

        //            GetChildLocations(ref oNode, expand, ref selectedNode, selectedVal);
        //            tvLocations.Nodes.Add(oNode);
        //        }

        //        if (selectedNode != null && expand != false)
        //        {
        //            while (selectedNode.Parent != null)
        //            {
        //                selectedNode.Parent.Expanded = true;
        //                selectedNode = selectedNode.Parent;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ErrorLog.LogEvent(ErrorLog.EventType.eEVENT_CRITICAL, this.ToString(), "GetTopLevelLocations()", ex.Message, "", String.Format("\n\n{0}", ex.StackTrace), false);
        //        //Debug.Assert(false, this.ToString() + "GetTopLevelLocations() - Exception " + ex.Message);
        //    }
        //    finally
        //    {
        //    }
        //}
        //private void GetChildLocations(ref TreeNode oParent, bool? expand, ref TreeNode selectedNode, string selectedVal)
        //{
        //    try
        //    {
        //        List<InvCategoryBO> dtObjects;
        //        TreeNode oNode;
        //        int iLevel;
        //        iLevel = oParent.Depth + 1;
        //        dtObjects = moLocation.GetInvCategoryInfoByCustomString(String.Format("WHERE  lvl = {0} AND AncestorId = {1}", iLevel, oParent.Value));

        //        foreach (InvCategoryBO item in dtObjects)
        //        {
        //            oNode = new TreeNode(HttpUtility.HtmlEncode(item.Name), item.CategoryId.ToString());
        //            oNode.Expanded = false;
        //            if (selectedVal == oNode.Value)
        //                selectedNode = oNode;

        //            oParent.ChildNodes.Add(oNode);

        //            GetChildLocations(ref oNode, expand, ref selectedNode, selectedVal);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ErrorLog.LogEvent(ErrorLog.EventType.eEVENT_CRITICAL, this.ToString(), "Load_Locations", ex.Message, "", String.Format("\n\n{0}", ex.StackTrace), false);
        //        //Debug.Assert(false, String.Format("{0}Load_Locations - Exception {1}", this.ToString(), ex.Message));
        //    }
        //    finally
        //    {
        //        ////tvLocations.EndUnboundLoad();
        //        ////Cursor.Current = Cursors.Default;
        //    }
        //}
    }
}