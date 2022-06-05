using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.SalesAndMarketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class AccountManagerTreeView : System.Web.UI.Page
    {
        AccountManagerDA moLocation = new AccountManagerDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                GetTopLevelLocations(null);
            }
        }
        private void GetTopLevelLocations(bool? expand)
        {
            try
            {
                TreeNode oNode = null;
                List<AccountManagerBO> dtObjects;
                string selectedVal = (tvLocations.SelectedNode != null) ? tvLocations.SelectedNode.Value : string.Empty;
                TreeNode selectedNode = null;
                tvLocations.Nodes.Clear();
                dtObjects = moLocation.GetAccountManagerInfoByCustomString("WHERE  lvl = 0");

                foreach (AccountManagerBO item in dtObjects)
                {
                    oNode = new TreeNode(HttpUtility.HtmlEncode(item.AccountManager), item.AccountManagerId.ToString());
                    oNode.Expanded = false;
                    if (selectedVal == oNode.Value)
                        selectedNode = oNode;

                    GetChildLocations(ref oNode, expand, ref selectedNode, selectedVal);
                    tvLocations.Nodes.Add(oNode);
                }

                if (selectedNode != null && expand != false)
                {
                    while (selectedNode.Parent != null)
                    {
                        selectedNode.Parent.Expanded = true;
                        selectedNode = selectedNode.Parent;
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogEvent(ErrorLog.EventType.eEVENT_CRITICAL, this.ToString(), "GetTopLevelLocations()", ex.Message, "", String.Format("\n\n{0}", ex.StackTrace), false);
                //Debug.Assert(false, this.ToString() + "GetTopLevelLocations() - Exception " + ex.Message);
                throw ex;
            }
            finally
            {
            }
        }
        private void GetChildLocations(ref TreeNode oParent, bool? expand, ref TreeNode selectedNode, string selectedVal)
        {
            try
            {
                List<AccountManagerBO> dtObjects;
                TreeNode oNode;
                int iLevel;
                iLevel = oParent.Depth + 1;
                dtObjects = moLocation.GetAccountManagerInfoByCustomString(String.Format("WHERE  lvl = {0} AND AncestorId = {1}", iLevel, oParent.Value));

                foreach (AccountManagerBO item in dtObjects)
                {
                    oNode = new TreeNode(HttpUtility.HtmlEncode(item.AccountManager), item.AccountManagerId.ToString());
                    oNode.Expanded = false;
                    if (selectedVal == oNode.Value)
                        selectedNode = oNode;

                    oParent.ChildNodes.Add(oNode);

                    GetChildLocations(ref oNode, expand, ref selectedNode, selectedVal);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogEvent(ErrorLog.EventType.eEVENT_CRITICAL, this.ToString(), "Load_Locations", ex.Message, "", String.Format("\n\n{0}", ex.StackTrace), false);
                //Debug.Assert(false, String.Format("{0}Load_Locations - Exception {1}", this.ToString(), ex.Message));
                throw ex;
            }
            finally
            {
                ////tvLocations.EndUnboundLoad();
                ////Cursor.Current = Cursors.Default;
            }
        }
    }
}