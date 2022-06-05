using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Inventory;
using HotelManagement.Presentation.Website.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class RecipeModifierType : System.Web.UI.Page
    {

        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStockBy();
                GetFinishedGoodItems();
                //GetAllItemForRecipe();
                //GetIChangeableRecipeItemByItemID();
            }
        }

        private void LoadStockBy()
        {
            List<InvUnitHeadBO> headListBO = new List<InvUnitHeadBO>();
            InvUnitHeadDA da = new InvUnitHeadDA();
            headListBO = da.GetInvUnitHeadInfo();

            ddlStockBy.DataSource = headListBO;
            ddlStockBy.DataTextField = "HeadName";
            ddlStockBy.DataValueField = "UnitHeadId";
            ddlStockBy.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlStockBy.Items.Insert(0, item);

        }

          [WebMethod]
        public static List<InvItemAutoSearchBO> GetAllItemForRecipe()
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemNameNCategoryForRecipeModifierAutoSearch("", 0).Where(x => x.IsRecipe == false).ToList();

            return itemInfo;
        }

        [WebMethod]
        public static List<InvItemAutoSearchBO> GetIChangeableRecipeItemByItemID(int itemid)
        {
            //int itemid = 26;
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetIChangeableRecipeItemByItemID(itemid);

            //ddlIngredient.DataSource = itemInfo;
            //ddlIngredient.DataTextField = "Name";
            //ddlIngredient.DataValueField = "ItemId";
            //ddlIngredient.DataBind();

            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //ddlIngredient.Items.Insert(0, item);

            return itemInfo;
        }


        [WebMethod]
        public static List<RestaurantRecipeDetailBO> GetChangeableRecipeItemDetails(int IngredientId, int itemid)
        {
            //int itemid = 26;
            List<RestaurantRecipeDetailBO> itemInfo = new List<RestaurantRecipeDetailBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetChangeableRecipeItemDetails(IngredientId, itemid);
            
            return itemInfo;
        }

        [WebMethod]
        public static bool SaveAndDelete(List<RestaurantRecipeDetailBO> receipeList, List<RestaurantRecipeDetailBO> deleteReceipeList, int itemId, int recipeItemId)
        {
            bool status= false;

            InvItemDA itemDa = new InvItemDA();
            status = itemDa.SaveAndDeleteModifiedRecipe(receipeList, deleteReceipeList,  itemId,  recipeItemId);

            return status;

        }

        [WebMethod]
        public static List<RestaurantRecipeDetailBO> GetPreviousRecipeModifierTypes(int IngredientId, int itemid)
        {
            //int itemid = 26;
            List<RestaurantRecipeDetailBO> itemInfo = new List<RestaurantRecipeDetailBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetPreviousRecipeModifierTypes(IngredientId, itemid);

            return itemInfo;
        }



        private void GetFinishedGoodItems()
        {
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> itemList = new List<InvItemBO>();
            itemList = productDA.GetAllFinishedGoodItems();

            ddlItemName.DataSource = itemList;
            ddlItemName.DataTextField = "Name";
            ddlItemName.DataValueField = "ItemId";
            ddlItemName.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlItemName.Items.Insert(0, item);



        }
    }
}