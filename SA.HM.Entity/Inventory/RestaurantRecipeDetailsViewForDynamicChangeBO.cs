using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class RestaurantRecipeDetailsViewForDynamicChangeBO
    {
        public List<RestaurantRecipeDetailBO> PreviousRecipe { get; set; }
        public List<RestaurantRecipeDetailBO> NewItems { get; set; }
        public List<RestaurantRecipeDetailBO> NewKotRecipe { get; set; }
    }
}
