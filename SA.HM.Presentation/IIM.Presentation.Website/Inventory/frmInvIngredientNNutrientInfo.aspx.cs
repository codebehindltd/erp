using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.PurchaseManagment;
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
    public partial class frmInvIngredientNNutrientInfo : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadItemName();
                LoadStockBy();
                LoadNutrientInfo();
                LoadAccountHead();
            }
        }

        private void LoadItemName()
        {
            InvItemDA invItemDa = new InvItemDA();
            List<InvItemBO> invItemBo = new List<InvItemBO>();
            invItemBo = invItemDa.GetInvFinishedItemInformation();

            ddlItemName.DataSource = invItemBo;
            ddlItemName.DataTextField = "CodeAndName";
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
        private void LoadAccountHead()
        {
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            ddlAccountHead.DataSource = nodeMatrixDA.GetNodeMatrixInfoByCustomString("WHERE CHARINDEX('.4.', Hierarchy) = 1 AND IsTransactionalHead = 1");
            ddlAccountHead.DataTextField = "HeadWithCode";
            ddlAccountHead.DataValueField = "NodeId";
            ddlAccountHead.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlAccountHead.Items.Insert(0, itemNodeId);
        }


        [WebMethod]
        public static List<RestaurantRecipeDetailBO> GetInvFinishGoodsInformationById(int itemId)
        {
            InvItemDA invDA = new InvItemDA();
            List<RestaurantRecipeDetailBO> recipeList = new List<RestaurantRecipeDetailBO>();
            recipeList = invDA.GetRecipeItemInfoByItemId(itemId);
            return recipeList;
        }

        [WebMethod]
        public static List<InvNutrientInfoBO> GetInvNutrientInformationById(int itemId)
        {
            InvNutrientInfoDA invDa = new InvNutrientInfoDA();
            List<InvNutrientInfoBO> nutrientList = new List<InvNutrientInfoBO>();
            nutrientList = invDa.GetNRVDetailsByItemId(itemId);
            return nutrientList;
        }
        [WebMethod]
        public static List<OverheadExpensesBO> GetInvFGNNutrientRequiredOEById(int itemId)
        {
            InvItemDA invDa = new InvItemDA();
            List<OverheadExpensesBO> accountHeadList = new List<OverheadExpensesBO>();
            accountHeadList = invDa.GetInvFGNNutrientRequiredOEById(itemId);
            return accountHeadList;
        }
        [WebMethod]
        public static List<InvItemAutoSearchBO> RawMaterialAutoSearch(string itemName)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.RawMaterialAutoSearch(itemName);

            return itemInfo;
        }
        [WebMethod]
        public static ReturnInfo SaveIngredientNNutrientInfo(List<RestaurantRecipeDetailBO> AddedIngredientInfo, List<RestaurantRecipeDetailBO> DeletedItemList, 
                                                            InvNutrientInfoBO NutrientRequiredMasterInfo, List<InvNutrientInfoBO> AddedNutrientRequiredValueInfo,
                                                            List<InvNutrientInfoBO> DeletedNutrientRequiredValueList, List<OverheadExpensesBO> AddedOverheadExpenses,
                                                            List<OverheadExpensesBO> DeletedAccountHead)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                InvItemDA itemDa = new InvItemDA();
                status = itemDa.SaveIngredientNNutrientInfo(AddedIngredientInfo, DeletedItemList, NutrientRequiredMasterInfo, AddedNutrientRequiredValueInfo, DeletedNutrientRequiredValueList, AddedOverheadExpenses, DeletedAccountHead, userInformationBO.UserInfoId);
                if (status)
                {
                    rtninfo.IsSuccess = true;
                    if (AddedIngredientInfo.Count > 0 || AddedNutrientRequiredValueInfo.Count > 0)
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                    else
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
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