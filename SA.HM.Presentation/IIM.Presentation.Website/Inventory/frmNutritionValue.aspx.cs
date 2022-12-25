using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class frmNutritionValue : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }

        [WebMethod]
        public static List<InvNutrientInfoBO> GetNutritionType()
        {
            List<InvNutrientInfoBO> nutritionType = new List<InvNutrientInfoBO>();
            InvNutrientInfoDA nutrientDa = new InvNutrientInfoDA();
            nutritionType = nutrientDa.GetNutritionType();
            return nutritionType;
        }
        [WebMethod]
        public static List<InvNutrientInfoBO> GetNutrientInformation()
        {
            List<InvNutrientInfoBO> nutrientInfo = new List<InvNutrientInfoBO>();
            InvNutrientInfoDA nutrientInfoDa = new InvNutrientInfoDA();
            nutrientInfo = nutrientInfoDa.GetNutrientInformations();
            return nutrientInfo;
        }
        [WebMethod]
        public static List<InvItemBO> GetIngredients()
        {
            List<InvItemBO> invItemBo = new List<InvItemBO>();
            InvItemDA invItemDa = new InvItemDA();
            invItemBo = invItemDa.GetInvItemInfo();
            return invItemBo;
        }
        
    }
}