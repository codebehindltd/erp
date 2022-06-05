using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.ViewModel
{
    public class SetUpData
    {
        public List<CommonCostCenter> CommonCostCenters { get; set; }
        public List<HotelGuestCompany> HotelGuestCompanys { get; set; }
        public List<InvUnitHead> InvUnitHeads { get; set; }
        public List<InvUnitConversion> InvUnitConversions { get; set; }
        public List<InvLocation> InvLocations { get; set; }
        public List<InvLocationCostCenterMapping> InvLocationCostCenterMappings { get; set; }
        public List<InvCategory> InvCategorys { get; set; }
        public List<InvCategoryCostCenterMapping> InvCategoryCostCenterMappings { get; set; }
        public List<InvInventoryAccountVsItemCategoryMappping> InvInventoryAccountVsItemCategoryMapppings { get; set; }
        public List<InvCogsAccountVsItemCategoryMappping> InvCogsAccountVsItemCategoryMapppings { get; set; }
        public List<InvItem> InvItems { get; set; }
        public List<InvItemCostCenterMapping> InvItemCostCenterMappings { get; set; }
        public List<RestaurantRecipeDetail> RestaurantRecipeDetails { get; set; }
        public List<InvItemClassification> InvItemClassifications { get; set; }
        public List<InvItemClassificationCostCenterMapping> InvItemClassificationCostCenterMappings { get; set; }
        public List<HotelRoomType> HotelRoomTypes { get; set; }
        public List<HotelRoomNumber> HotelRoomNumbers { get; set; }
    }
}
