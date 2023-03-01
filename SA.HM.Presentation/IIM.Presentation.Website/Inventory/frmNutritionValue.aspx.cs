using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
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
            invItemBo = invItemDa.GetInvItemInformationByCustomCategoryIdList("RawMaterials");
            return invItemBo;
        }
        [WebMethod]
        public static List<InvNutrientInfoBO> GetNutrientAmounts()
        {
            List<InvNutrientInfoBO> nutrientAmountList = new List<InvNutrientInfoBO>();
            InvNutrientInfoDA niDa = new InvNutrientInfoDA();
            nutrientAmountList = niDa.GetNutrientAmounts();
            return nutrientAmountList;
        }
        [WebMethod]        
        public static ReturnInfo SaveNutrientsAmount(List<InvNutrientInfoBO> AddedNutrients)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                InvNutrientInfoDA nutrientInfoDa = new InvNutrientInfoDA();
                status = nutrientInfoDa.SaveNutrientsAmount(AddedNutrients, userInformationBO.UserInfoId);
                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }

                if (!status)
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;

        }
    }
}