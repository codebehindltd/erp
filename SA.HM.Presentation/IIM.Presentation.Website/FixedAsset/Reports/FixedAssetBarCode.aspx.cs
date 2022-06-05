using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.FixedAsset.Reports
{
    public partial class FixedAssetBarCode : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int _RoomStatusInfoByDate = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadCommonDropDownHiddenField();
            LoadLocation();
            LoadCategory();

        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
        }

        public void LoadLocation()
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvLocationInfo();

            ddlLocation.DataSource = location;
            ddlLocation.DataTextField = "Name";
            ddlLocation.DataValueField = "LocationId";
            ddlLocation.DataBind();

            System.Web.UI.WebControls.ListItem itemNodeId = new System.Web.UI.WebControls.ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstAllValue();
            ddlLocation.Items.Insert(0, itemNodeId);
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();

            List<InvCategoryBO> FixedAssetList = new List<InvCategoryBO>();
            FixedAssetList = da.GetAllInvItemCatagoryInfoByServiceType("FixedAsset");
            List.AddRange(FixedAssetList);

            ddlCategory.DataSource = List;
            ddlCategory.DataTextField = "MatrixInfo";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCategory.Items.Insert(0, item);
        }
        
        [WebMethod]
        public static string PrintBarCodeById(int itemId,int categoryId,int locationId )
        {
            List<InvItemViewForBarcodeBO> productBOList = new List<InvItemViewForBarcodeBO>();
            InvItemDA productDA = new InvItemDA();
            productBOList = productDA.GetItemForBarcodeInFixedAsset(itemId, locationId, categoryId);

            string pathList = PrintBarcodeLabel.PrintBarcode(productBOList);
            return pathList;
        }
        [WebMethod]
        public static List<ItemViewBO> LoadProductByCategoryId(int categoryId)
        {
            InvItemDA itemda = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();

            productList = itemda.GetInvItemInfoByCategory(0, categoryId);
            List<ItemViewBO> itemViewList = new List<ItemViewBO>();

            itemViewList = (from s in productList
                            select new ItemViewBO
                            {
                                ItemId = s.ItemId,
                                ItemName = s.Name,
                                ProductType = s.ProductType

                            }).ToList();

            return itemViewList;
        }
    }
}