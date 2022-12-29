using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Inventory;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class frmNutrientRequiredValues : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadItemName();
                LoadNutrientInfo();
            }
        }
        private void LoadItemName()
        {
            InvItemDA invItemDa = new InvItemDA();
            List<InvItemBO> invItemBo = new List<InvItemBO>();
            invItemBo = invItemDa.GetInvItemInfo();

            ddlItemName.DataSource = invItemBo;
            ddlItemName.DataTextField = "Name";
            ddlItemName.DataValueField = "ItemId";
            ddlItemName.DataBind();

            ListItem itemSearch = new ListItem();
            itemSearch.Value = "0";
            itemSearch.Text = hmUtility.GetDropDownFirstValue();
            ddlItemName.Items.Insert(0, itemSearch);
        }
        
        private void LoadNutrientInfo()
        {
            InvNutrientInfoDA invNutrientDa = new InvNutrientInfoDA();
            List<InvNutrientInfoBO> invNutrientBo = new List<InvNutrientInfoBO>();
            invNutrientBo = invNutrientDa.GetNutrientInformations();

            ddlNutrient.DataSource = invNutrientBo;
            ddlNutrient.DataTextField = "Name";
            ddlNutrient.DataValueField = "NutrientId";
            ddlNutrient.DataBind();

            ListItem itemSearch = new ListItem();
            itemSearch.Value = "0";
            itemSearch.Text = hmUtility.GetDropDownFirstValue();
            ddlNutrient.Items.Insert(0, itemSearch);
        }
    }
}