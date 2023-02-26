using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Inventory;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Entity.Banquet;
using HotelManagement.Entity.PurchaseManagment;

namespace HotelManagement.Data.Inventory
{
    public class InvItemDA : BaseService
    {
        public List<InvItemBO> GetInvItemInfo()
        {
            List<InvItemBO> productList = new List<InvItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO productBO = new InvItemBO();
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.CodeAndName = reader["Code"].ToString() + " - " + reader["Name"].ToString();
                                productBO.Description = reader["Description"].ToString();
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                productBO.ProductType = reader["ProductType"].ToString();
                                productBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                productBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                productBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                productBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                productBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                                productBO.StockType = reader["StockType"].ToString();
                                productBO.ServiceType = reader["ServiceType"].ToString();
                                productBO.StockBy = Int32.Parse(reader["StockBy"].ToString());
                                productBO.IsSupplierItem = Convert.ToBoolean(reader["IsSupplierItem"].ToString());
                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }

        public List<InvItemAutoSearchBO> GetItemDetailsForAutoSearchByCategoryAndCustomerNSupplierItem(string itemName, int categoryId, int IsCustomerItem, int IsSupplierItem)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemDetailsForAutoSearchByCategoryAndCustomerNSupplierItem_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);

                    if (categoryId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@IsCustomerItem", DbType.Int32, IsCustomerItem);
                    dbSmartAspects.AddInParameter(cmd, "@IsSupplierItem", DbType.Int32, IsSupplierItem);




                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        ItemName = r.Field<string>("ItemName"),
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        ImageName = r.Field<string>("ImageName"),
                        ProductType = r.Field<string>("ProductType"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitHead = r.Field<string>("HeadName")

                    }).ToList();
                }
            }

            return itemInfo;
        }



        public List<InvItemAutoSearchBO> GetItemDetailsForAutoSearchByCategoryAndCustomerNSupplierItemNClient(int costCenterId, string itemName, int categoryId, int clientId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemDetailsForAutoSearchByCategoryAndCustomerNSupplierItemNClient_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    //dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    if (categoryId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);
                    dbSmartAspects.AddInParameter(cmd, "@clientId", DbType.Int32, clientId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        ItemName = r.Field<string>("ItemName"),
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        ImageName = r.Field<string>("ImageName"),
                        ProductType = r.Field<string>("ProductType"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitHead = r.Field<string>("HeadName"),
                        UnitPrice = r.Field<decimal>("UnitPrice"),
                        VatAmount = r.Field<decimal>("VatAmount")

                    }).ToList();
                }
            }

            return itemInfo;
        }
        public InvItemBO GetInvItemInfoByItemId(int itemId)
        {
            InvItemBO productBO = new InvItemBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemInfoByItemId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.Description = reader["Description"].ToString();
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                productBO.ProductType = reader["ProductType"].ToString();
                                productBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                productBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                productBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                productBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                productBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                                productBO.StockType = reader["StockType"].ToString();
                                productBO.StockBy = Int32.Parse(reader["StockBy"].ToString());
                            }
                        }
                    }
                }
            }
            return productBO;
        }

        public InvItemBO GetInvItemInfoByItemNCategoryId(int itemId)
        {
            InvItemBO productBO = new InvItemBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemInfoByItemNCategoryId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.Description = reader["Description"].ToString();
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                productBO.ProductType = reader["ProductType"].ToString();
                                productBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                productBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                productBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                productBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                productBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                                productBO.StockType = reader["StockType"].ToString();
                                productBO.StockBy = Int32.Parse(reader["StockBy"].ToString());
                            }
                        }
                    }
                }
            }
            return productBO;
        }
        public List<InvItemBO> GetInvItemInfo(bool hasRecipe)
        {
            List<InvItemBO> productList = new List<InvItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvRecipeItemInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@IsRecipe", DbType.Boolean, hasRecipe);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO productBO = new InvItemBO();
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.CodeAndName = reader["CodeAndName"].ToString();
                                productBO.Description = reader["Description"].ToString();
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                productBO.ProductType = reader["ProductType"].ToString();
                                productBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                productBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                productBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                productBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                productBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                                productBO.StockType = reader["StockType"].ToString();
                                productBO.StockBy = Int32.Parse(reader["StockBy"].ToString());
                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }
        public List<InvItemBO> GetInvItemInfoByCostCenterIdNItemType(int costCenterId, string itemType)
        {
            List<InvItemBO> productList = new List<InvItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemInfoByCostCenterIdNItemType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, itemType);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO productBO = new InvItemBO();
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.Description = reader["Description"].ToString();
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                productBO.ProductType = reader["ProductType"].ToString();
                                productBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                                productBO.StockType = reader["StockType"].ToString();
                                productBO.StockBy = Int32.Parse(reader["StockBy"].ToString());
                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }
        public InvItemBO GetInvItemInfoById(int costCenterId, int itemId)
        {
            InvItemBO productBO = new InvItemBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.Name = reader["Name"].ToString();
                                productBO.DisplayName = reader["DisplayName"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.Description = reader["Description"].ToString();
                                productBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.ItemType = reader["ItemType"].ToString();
                                productBO.CategoryName = reader["CategoryName"].ToString();
                                productBO.ProductType = reader["ProductType"].ToString();
                                productBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                productBO.AverageCost = Convert.ToDecimal(reader["AverageCost"]);
                                productBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                productBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                productBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                productBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                productBO.TypeId = Int32.Parse(reader["TypeId"].ToString());
                                productBO.StockType = reader["StockType"].ToString();
                                productBO.StockBy = Int32.Parse(reader["StockBy"].ToString());
                                productBO.SalesStockBy = Int32.Parse(reader["SalesStockBy"].ToString());
                                productBO.ClassificationId = Convert.ToInt32(reader["ClassificationId"]);
                                productBO.IsCustomerItem = Convert.ToBoolean(reader["IsCustomerItem"].ToString());
                                productBO.IsSupplierItem = Convert.ToBoolean(reader["IsSupplierItem"].ToString());
                                productBO.IsAttributeItem = Convert.ToBoolean(reader["IsAttributeItem"]);

                                if (reader["IsItemEditable"].ToString() != string.Empty)
                                    productBO.IsItemEditable = Convert.ToBoolean(reader["IsItemEditable"].ToString());
                                else
                                    productBO.IsItemEditable = false;

                                productBO.Model = reader["Model"].ToString();
                                productBO.CountryId = Convert.ToInt32(reader["CountryId"]);

                                if (reader["AdjustmentFrequency"].ToString() != string.Empty)
                                    productBO.AdjustmentFrequency = reader["AdjustmentFrequency"].ToString();

                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                            }
                        }
                    }
                }
            }
            return productBO;
        }
        public InvItemViewBO GetInvItemPriceForBanquet(int categoryId, int costCenterId, int itemId)
        {
            InvItemViewBO viewBO = new InvItemViewBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemPriceForBanquet_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                //if (!string.IsNullOrEmpty(reader["ItemId"].ToString()))
                                viewBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                viewBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                            }
                        }
                    }

                }
            }
            return viewBO;
        }
        public bool SaveInvItemInfo(InvItemBO productBO, List<InvItemSuppierMappingBO> supplierList, List<InvItemCostCenterMappingBO> costCenterList, List<RestaurantRecipeDetailBO> recipeItemList, out int tmpProductId)
        {
            Boolean status = false;
            tmpProductId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveInvItemInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@ItemType", DbType.String, productBO.ItemType);
                            dbSmartAspects.AddInParameter(command, "@Name", DbType.String, productBO.Name);
                            dbSmartAspects.AddInParameter(command, "@DisplayName", DbType.String, productBO.DisplayName);
                            dbSmartAspects.AddInParameter(command, "@Code", DbType.String, productBO.Code);
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, productBO.Description);
                            dbSmartAspects.AddInParameter(command, "@CategoryId", DbType.Int32, productBO.CategoryId);
                            dbSmartAspects.AddInParameter(command, "@ManufacturerId", DbType.Int32, productBO.ManufacturerId);
                            dbSmartAspects.AddInParameter(command, "@ProductType", DbType.String, productBO.ProductType);
                            dbSmartAspects.AddInParameter(command, "@PurchasePrice", DbType.Decimal, productBO.PurchasePrice);
                            dbSmartAspects.AddInParameter(command, "@SellingLocalCurrencyId", DbType.Int32, productBO.SellingLocalCurrencyId);
                            dbSmartAspects.AddInParameter(command, "@UnitPriceLocal", DbType.Decimal, productBO.UnitPriceLocal);
                            dbSmartAspects.AddInParameter(command, "@SellingUsdCurrencyId", DbType.Int32, productBO.SellingUsdCurrencyId);
                            dbSmartAspects.AddInParameter(command, "@UnitPriceUsd", DbType.Decimal, productBO.UnitPriceUsd);
                            dbSmartAspects.AddInParameter(command, "@StockType", DbType.String, productBO.StockType);
                            dbSmartAspects.AddInParameter(command, "@StockBy", DbType.Int32, productBO.StockBy);
                            dbSmartAspects.AddInParameter(command, "@SalesStockBy", DbType.Int32, productBO.SalesStockBy);
                            dbSmartAspects.AddInParameter(command, "@ClassificationId", DbType.Int32, productBO.ClassificationId);
                            dbSmartAspects.AddInParameter(command, "@IsCustomerItem", DbType.Boolean, productBO.IsCustomerItem);
                            dbSmartAspects.AddInParameter(command, "@IsSupplierItem", DbType.Boolean, productBO.IsSupplierItem);
                            dbSmartAspects.AddInParameter(command, "@IsRecipe", DbType.Boolean, productBO.IsRecipe);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentFrequency", DbType.String, productBO.AdjustmentFrequency);
                            dbSmartAspects.AddInParameter(command, "@ServiceWarranty", DbType.Int32, productBO.ServiceWarranty);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, productBO.CreatedBy);
                            dbSmartAspects.AddInParameter(command, "@IsItemEditable", DbType.Int32, productBO.IsItemEditable);
                            if (productBO.IsAttributeItem)
                                dbSmartAspects.AddInParameter(command, "@IsAttributeItem", DbType.Boolean, productBO.IsAttributeItem);
                            else
                                dbSmartAspects.AddInParameter(command, "@IsAttributeItem", DbType.Boolean, false);

                            dbSmartAspects.AddInParameter(command, "@CountryId", DbType.Int32, productBO.CountryId);
                            dbSmartAspects.AddInParameter(command, "@Model", DbType.String, productBO.Model);


                            dbSmartAspects.AddOutParameter(command, "@ItemId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            tmpProductId = Convert.ToInt32(command.Parameters["@ItemId"].Value);

                            Boolean uploadStatus = SaveInventoryProductImage(tmpProductId, productBO.RandomItemId);

                            if (costCenterList != null)
                            {
                                int costCount = costCenterList.Count;
                                for (int i = 0; i < costCount; i++)
                                {
                                    costCenterList[i].ItemId = tmpProductId;
                                }

                                if (costCount > 0)
                                {
                                    Boolean costStatus = SaveCostCenterList(costCenterList);
                                }
                            }

                        }

                        if (status)
                        {
                            if (supplierList != null)
                            {
                                using (DbCommand commandMapping = dbSmartAspects.GetStoredProcCommand("SaveInvItemSupplierMappingInfo_SP"))
                                {
                                    foreach (InvItemSuppierMappingBO mappingBO in supplierList)
                                    {
                                        commandMapping.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandMapping, "@ItemId", DbType.Int32, tmpProductId);
                                        dbSmartAspects.AddInParameter(commandMapping, "@SupplierId", DbType.Int32, mappingBO.SupplierId);
                                        dbSmartAspects.AddOutParameter(commandMapping, "@MappingId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandMapping) > 0 ? true : false;
                                    }
                                }
                            }

                            if (recipeItemList != null)
                            {
                                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveRestaurantRecipeDetailInfo_SP"))
                                {
                                    foreach (RestaurantRecipeDetailBO detailBO in recipeItemList)
                                    {
                                        cmd.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(cmd, "@RecipeId", DbType.Int32, detailBO.RecipeId);
                                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, tmpProductId);
                                        dbSmartAspects.AddInParameter(cmd, "@RecipeItemId", DbType.Int32, detailBO.RecipeItemId);
                                        dbSmartAspects.AddInParameter(cmd, "@RecipeItemName", DbType.String, detailBO.RecipeItemName);
                                        dbSmartAspects.AddInParameter(cmd, "@UnitHeadId", DbType.Int32, detailBO.UnitHeadId);
                                        dbSmartAspects.AddInParameter(cmd, "@ItemUnit", DbType.Decimal, detailBO.ItemUnit);
                                        dbSmartAspects.AddInParameter(cmd, "@ItemCost", DbType.Decimal, detailBO.ItemCost);
                                        dbSmartAspects.AddInParameter(cmd, "@IsGradientCanChange", DbType.Boolean, detailBO.IsGradientCanChange);

                                        status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                                    }
                                }
                            }
                        }

                        if (productBO.IsRecipe && status)
                        {
                            using (DbCommand cmdAc = dbSmartAspects.GetStoredProcCommand("UpdateItemAvgCostForRecipe_SP"))
                            {
                                foreach (InvItemCostCenterMappingBO cci in costCenterList)
                                {
                                    cmdAc.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(cmdAc, "@ItemId", DbType.Int32, tmpProductId);
                                    status = dbSmartAspects.ExecuteNonQuery(cmdAc, transaction) > 0 ? true : false;
                                }
                            }
                        }
                        if (!String.IsNullOrEmpty(productBO.ItemAttribute) && status && productBO.IsAttributeItem)
                        {
                            using (DbCommand cmdAttribute = dbSmartAspects.GetStoredProcCommand("SaveAndUpdateAttributesForItem_SP"))
                            {
                                cmdAttribute.Parameters.Clear();
                                dbSmartAspects.AddInParameter(cmdAttribute, "@ItemId", DbType.Int32, tmpProductId);
                                dbSmartAspects.AddInParameter(cmdAttribute, "@ItemAttributes", DbType.String, productBO.ItemAttribute);
                                status = dbSmartAspects.ExecuteNonQuery(cmdAttribute, transaction) > 0 ? true : false;
                            }
                        }
                        if (status)
                            transaction.Commit();
                        else
                            transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status;
        }

        public bool SaveAndDeleteModifiedRecipe(List<RestaurantRecipeDetailBO> receipeList, List<RestaurantRecipeDetailBO> deleteReceipeList, int itemId, int recipeItemId)
        {
            bool status = false;
            int statusInt = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                try
                {
                    if (receipeList.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveAndDeleteModifiedRecipe_SP"))
                        {
                            foreach (RestaurantRecipeDetailBO detailBO in receipeList)
                            {
                                if (detailBO.Id == 0)
                                {
                                    cmd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                                    dbSmartAspects.AddInParameter(cmd, "@RecipeItemId", DbType.Int32, recipeItemId);
                                    dbSmartAspects.AddInParameter(cmd, "@UnitHead", DbType.String, detailBO.HeadName);
                                    dbSmartAspects.AddInParameter(cmd, "@UnitQuantity", DbType.Decimal, detailBO.ItemUnit);
                                    dbSmartAspects.AddInParameter(cmd, "@AdditionalCost", DbType.Decimal, detailBO.AditionalCost);
                                    dbSmartAspects.AddInParameter(cmd, "@TotalCost", DbType.Decimal, detailBO.TotalCost);
                                    dbSmartAspects.AddInParameter(cmd, "@UnitHeadId", DbType.Int32, detailBO.UnitHeadId);

                                    statusInt = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? 1 : 0;
                                }
                            }
                        }

                    }
                    if (deleteReceipeList.Count > 0)
                    {

                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteReceipeModifierType_SP"))
                        {
                            foreach (RestaurantRecipeDetailBO detailBO in deleteReceipeList)
                            {
                                cmd.Parameters.Clear();
                                dbSmartAspects.AddInParameter(cmd, "@RecipeId", DbType.Int32, detailBO.RecipeId);
                                statusInt = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? 1 : 0;
                            }

                        }

                    }



                    if (statusInt > 0)
                        status = true;
                    else
                        status = false;

                }
                catch (Exception e)
                {
                    status = false;

                }

            }


            return status;

        }

        private bool SaveCostCenterList(List<InvItemCostCenterMappingBO> costCenterList)
        {
            bool status = false;
            InvItemCostCenterMappingDA costDA = new InvItemCostCenterMappingDA();
            int costCount = costCenterList.Count;
            int mappingId;
            for (int i = 0; i < costCount; i++)
            {
                status = costDA.SaveInvItemCostCenterMappingInfo(costCenterList[i], out mappingId);
            }

            return status;
        }
        public Boolean SaveInventoryProductImage(int productId, int randomProductId)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateEmployeeDocumentAndSignature_SP"))
                {
                    commandDocument.Parameters.Clear();
                    dbSmartAspects.AddInParameter(commandDocument, "@EmpId", DbType.Int32, productId);
                    dbSmartAspects.AddInParameter(commandDocument, "@RandomId", DbType.String, randomProductId);
                    status = dbSmartAspects.ExecuteNonQuery(commandDocument) > 0 ? true : false;
                }
            }

            return status;
        }
        public bool UpdateInvItemInfo(InvItemBO productBO, List<InvItemSuppierMappingBO> supplierList, List<InvItemCostCenterMappingBO> costCenterList, List<RestaurantRecipeDetailBO> deleteDetailList, List<RestaurantRecipeDetailBO> addDetailList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateInvItemInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, productBO.ItemId);
                            dbSmartAspects.AddInParameter(command, "@ItemType", DbType.String, productBO.ItemType);
                            dbSmartAspects.AddInParameter(command, "@Name", DbType.String, productBO.Name);
                            dbSmartAspects.AddInParameter(command, "@DisplayName", DbType.String, productBO.DisplayName);
                            dbSmartAspects.AddInParameter(command, "@Code", DbType.String, productBO.Code);
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, productBO.Description);
                            dbSmartAspects.AddInParameter(command, "@CategoryId", DbType.Int32, productBO.CategoryId);
                            dbSmartAspects.AddInParameter(command, "@ManufacturerId", DbType.Int32, productBO.ManufacturerId);
                            dbSmartAspects.AddInParameter(command, "@ProductType", DbType.String, productBO.ProductType);
                            dbSmartAspects.AddInParameter(command, "@PurchasePrice", DbType.Decimal, productBO.PurchasePrice);
                            dbSmartAspects.AddInParameter(command, "@SellingLocalCurrencyId", DbType.Int32, productBO.SellingLocalCurrencyId);
                            dbSmartAspects.AddInParameter(command, "@UnitPriceLocal", DbType.Decimal, productBO.UnitPriceLocal);
                            dbSmartAspects.AddInParameter(command, "@SellingUsdCurrencyId", DbType.Int32, productBO.SellingUsdCurrencyId);
                            dbSmartAspects.AddInParameter(command, "@UnitPriceUsd", DbType.Decimal, productBO.UnitPriceUsd);
                            dbSmartAspects.AddInParameter(command, "@StockType", DbType.String, productBO.StockType);
                            dbSmartAspects.AddInParameter(command, "@StockBy", DbType.Int32, productBO.StockBy);
                            dbSmartAspects.AddInParameter(command, "@SalesStockBy", DbType.Int32, productBO.SalesStockBy);
                            dbSmartAspects.AddInParameter(command, "@ClassificationId", DbType.Int32, productBO.ClassificationId);
                            dbSmartAspects.AddInParameter(command, "@IsCustomerItem", DbType.Boolean, productBO.IsCustomerItem);
                            dbSmartAspects.AddInParameter(command, "@IsSupplierItem", DbType.Boolean, productBO.IsSupplierItem);
                            dbSmartAspects.AddInParameter(command, "@IsRecipe", DbType.Boolean, productBO.IsRecipe);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentFrequency", DbType.String, productBO.AdjustmentFrequency);
                            dbSmartAspects.AddInParameter(command, "@ServiceWarranty", DbType.Int32, productBO.ServiceWarranty);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, productBO.LastModifiedBy);
                            dbSmartAspects.AddInParameter(command, "@IsItemEditable", DbType.Int32, productBO.IsItemEditable);
                            if (productBO.IsAttributeItem)
                                dbSmartAspects.AddInParameter(command, "@IsAttributeItem", DbType.Boolean, productBO.IsAttributeItem);
                            else
                                dbSmartAspects.AddInParameter(command, "@IsAttributeItem", DbType.Boolean, false);
                            dbSmartAspects.AddInParameter(command, "@CountryId", DbType.Int32, productBO.CountryId);
                            dbSmartAspects.AddInParameter(command, "@Model", DbType.String, productBO.Model);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                            if (productBO.IsSupplierItem == false)
                            {
                                supplierList = null;
                            }
                        }

                        if (deleteDetailList != null)
                        {
                            if (status)
                            {
                                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteRestaurantRecipeDetailsInfo_SP"))
                                {
                                    foreach (RestaurantRecipeDetailBO detailBO in deleteDetailList)
                                    {
                                        cmd.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(cmd, "@RecipeId", DbType.Int32, detailBO.RecipeId);
                                        status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                    }
                                }
                            }
                        }

                        if (addDetailList != null)
                        {
                            if (status)
                            {
                                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveRestaurantRecipeDetailInfo_SP"))
                                {
                                    foreach (RestaurantRecipeDetailBO detailBO in addDetailList)
                                    {
                                        cmd.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(cmd, "@RecipeId", DbType.Int32, detailBO.RecipeId);
                                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, productBO.ItemId);
                                        dbSmartAspects.AddInParameter(cmd, "@RecipeItemId", DbType.Int32, detailBO.RecipeItemId);
                                        dbSmartAspects.AddInParameter(cmd, "@RecipeItemName", DbType.String, detailBO.RecipeItemName);
                                        dbSmartAspects.AddInParameter(cmd, "@UnitHeadId", DbType.Int32, detailBO.UnitHeadId);
                                        dbSmartAspects.AddInParameter(cmd, "@ItemUnit", DbType.Decimal, detailBO.ItemUnit);
                                        dbSmartAspects.AddInParameter(cmd, "@ItemCost", DbType.Decimal, detailBO.ItemCost);
                                        dbSmartAspects.AddInParameter(cmd, "@IsGradientCanChange", DbType.Boolean, detailBO.IsGradientCanChange);

                                        status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                    }
                                }
                            }
                        }

                        if (productBO.IsRecipe && status)
                        {
                            using (DbCommand cmdAc = dbSmartAspects.GetStoredProcCommand("UpdateItemAvgCostForRecipe_SP"))
                            {
                                cmdAc.Parameters.Clear();
                                dbSmartAspects.AddInParameter(cmdAc, "@ItemId", DbType.Int32, productBO.ItemId);
                                status = dbSmartAspects.ExecuteNonQuery(cmdAc, transaction) > 0 ? true : false;
                            }
                        }

                        if (supplierList != null)
                        {
                            if (supplierList.Count > 0)
                            {
                                Boolean supplierStatus = UpdateInvItemSupplierMapping(supplierList);
                            }
                            else if (supplierList.Count == 0)
                            {
                                InvItemCostCenterMappingDA costDA = new InvItemCostCenterMappingDA();
                                Boolean sStatus = costDA.DeleteAllInvItemSupplierMappingInfoForItemId(productBO.ItemId);
                            }
                        }

                        if (costCenterList != null)
                        {
                            Boolean costStatus = UpdatePMProductCostCenterMapping(costCenterList, productBO.ItemId);
                        }
                        if (!String.IsNullOrEmpty(productBO.ItemAttribute) && status)
                        {
                            using (DbCommand cmdAttribute = dbSmartAspects.GetStoredProcCommand("SaveAndUpdateAttributesForItem_SP"))
                            {
                                cmdAttribute.Parameters.Clear();
                                dbSmartAspects.AddInParameter(cmdAttribute, "@ItemId", DbType.Int32, productBO.ItemId);
                                dbSmartAspects.AddInParameter(cmdAttribute, "@ItemAttributes", DbType.String, productBO.ItemAttribute);
                                status = dbSmartAspects.ExecuteNonQuery(cmdAttribute, transaction) > 0 ? true : false;
                            }
                        }
                        if (status && !productBO.IsAttributeItem)
                        {
                            using (DbCommand cmdAttribute = dbSmartAspects.GetStoredProcCommand("DeleteAttributesByItemId_SP"))
                            {
                                cmdAttribute.Parameters.Clear();
                                dbSmartAspects.AddInParameter(cmdAttribute, "@ItemId", DbType.Int32, productBO.ItemId);
                                status = dbSmartAspects.ExecuteNonQuery(cmdAttribute, transaction) > 0 ? true : false;
                                status = true;
                            }
                        }
                        if (status)
                            transaction.Commit();
                        else
                            transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status;
        }
        private bool UpdateInvItemSupplierMapping(List<InvItemSuppierMappingBO> costCenterList)
        {
            InvItemCostCenterMappingDA costDA = new InvItemCostCenterMappingDA();
            List<InvItemSuppierMappingBO> dbCostList = new List<InvItemSuppierMappingBO>();
            dbCostList = costDA.GetInvItemSupplierMappingByItemId(costCenterList[0].ItemId);

            // Both In
            List<int> idList = new List<int>();
            int mappingId;
            for (int i = 0; i < costCenterList.Count; i++)
            {
                int succ = IsSupplierAvailableInDb(dbCostList, costCenterList[i]);
                if (succ > 0)
                {
                    //Update
                    costCenterList[i].MappingId = succ;
                    bool status = costDA.UpdateInvItemSuppierMappingInfo(costCenterList[i]);
                    idList.Add(succ);
                }
                else
                {
                    //Insert
                    bool status = costDA.SaveInvItemSupplierMappingInfo(costCenterList[i], out mappingId);
                    idList.Add(mappingId);
                }
            }

            string saveAndUpdatedIdList = string.Empty;
            for (int j = 0; j < idList.Count; j++)
            {
                if (string.IsNullOrWhiteSpace(saveAndUpdatedIdList))
                {
                    saveAndUpdatedIdList = idList[j].ToString();
                }
                else
                {
                    saveAndUpdatedIdList = saveAndUpdatedIdList + "," + idList[j];
                }
            }

            Boolean deleteStatus = costDA.DeleteAllInvItemSupplierMappingInfoWithoutMappingIdList(costCenterList[0].ItemId, saveAndUpdatedIdList);
            return true;
        }
        private int IsSupplierAvailableInDb(List<InvItemSuppierMappingBO> dbCostList, InvItemSuppierMappingBO costBO)
        {
            int isInDB = 0;
            for (int j = 0; j < dbCostList.Count; j++)
            {
                if (dbCostList[j].ItemId == costBO.ItemId && costBO.SupplierId == dbCostList[j].SupplierId)
                {
                    isInDB = dbCostList[j].MappingId;
                }
            }
            return isInDB;
        }
        private bool UpdatePMProductCostCenterMapping(List<InvItemCostCenterMappingBO> costCenterList)
        {
            InvItemCostCenterMappingDA costDA = new InvItemCostCenterMappingDA();
            List<InvItemCostCenterMappingBO> dbCostList = new List<InvItemCostCenterMappingBO>();
            dbCostList = costDA.GetInvItemCostCenterMappingByItemId(costCenterList[0].ItemId);
            bool rtn = false;

            int mappingId;

            var newlyAdded = (from cc in costCenterList
                              where cc.MappingId == 0
                              select cc
                              ).ToList();

            var updateItem = (from cn in dbCostList
                              join cc in costCenterList on cn.CostCenterId equals cc.CostCenterId
                              where cc.MappingId != 0
                              select cc
                              ).ToList();


            var deletedItem = (from cn in dbCostList
                               where !(from cc in costCenterList select cc.CostCenterId).Contains(cn.CostCenterId)
                               select cn
                              ).ToList();

            foreach (InvItemCostCenterMappingBO up in updateItem)
                rtn = costDA.UpdateInvItemCostCenterMappingInfo(up);

            foreach (InvItemCostCenterMappingBO np in newlyAdded)
                rtn = costDA.SaveInvItemCostCenterMappingInfo(np, out mappingId);

            foreach (InvItemCostCenterMappingBO dp in deletedItem)
                rtn = costDA.DeleteInvItemCostCenterMappingInfoByMappingId(dp.MappingId);

            return rtn;
        }
        private bool UpdatePMProductCostCenterMapping(List<InvItemCostCenterMappingBO> costCenterList, int itemId)
        {
            InvItemCostCenterMappingDA costDA = new InvItemCostCenterMappingDA();
            List<InvItemCostCenterMappingBO> dbCostList = new List<InvItemCostCenterMappingBO>();
            dbCostList = costDA.GetInvItemCostCenterMappingByItemId(itemId);

            bool rtn = false;
            int mappingId = 0;

            var newlyAdded = (from cc in costCenterList
                              where cc.MappingId == 0
                              select cc
                              ).ToList();

            var updateItem = (from cn in dbCostList
                              join cc in costCenterList on cn.CostCenterId equals cc.CostCenterId
                              where cc.MappingId != 0
                              select cc
                              ).ToList();

            var deletedItem = (from cn in dbCostList
                               where !(from cc in costCenterList select cc.CostCenterId).Contains(cn.CostCenterId)
                               select cn
                              ).ToList();

            foreach (InvItemCostCenterMappingBO up in updateItem)
                rtn = costDA.UpdateInvItemCostCenterMappingInfo(up);

            foreach (InvItemCostCenterMappingBO np in newlyAdded)
                rtn = costDA.SaveInvItemCostCenterMappingInfo(np, out mappingId);

            foreach (InvItemCostCenterMappingBO dp in deletedItem)
                rtn = costDA.DeleteInvItemCostCenterMappingInfoByMappingId(dp.MappingId);

            return rtn;
        }
        private int IsAvailableInDb(List<InvItemCostCenterMappingBO> dbCostList, InvItemCostCenterMappingBO costBO)
        {
            int isInDB = 0;
            for (int j = 0; j < dbCostList.Count; j++)
            {
                if (dbCostList[j].ItemId == costBO.ItemId && costBO.CostCenterId == dbCostList[j].CostCenterId)
                {
                    isInDB = dbCostList[j].MappingId;
                }
            }
            return isInDB;
        }
        public List<InvItemBO> GetInvItemInfoByCategoryId(int costCenterId, int categoryId)
        {
            List<InvItemBO> productList = new List<InvItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemInfoByCategoryId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    if (categoryId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    }

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO productBO = new InvItemBO();
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.DisplayName = Convert.ToString(reader["DisplayName"]);
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.CategoryName = reader["CategoryName"].ToString();
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.ItemNameAndCode = reader["ItemNameAndCode"].ToString();
                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                                productBO.ImageName = reader["ImageName"].ToString();
                                productBO.Hierarchy = reader["Hierarchy"].ToString();
                                productBO.StockBy = Convert.ToInt32(reader["StockBy"]);
                                productBO.ProductType = reader["ProductType"].ToString();
                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }
        public List<InvItemBO> GetInvItemInfoByCategory(int costCenterId, int categoryId)
        {
            List<InvItemBO> productList = new List<InvItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemInfoByCategory_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO productBO = new InvItemBO();
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.CategoryName = reader["CategoryName"].ToString();
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                                productBO.ImageName = reader["ImageName"].ToString();
                                productBO.Hierarchy = reader["Hierarchy"].ToString();
                                productBO.ProductType = reader["ProductType"].ToString();
                                productBO.StockType = reader["TypeName"].ToString();
                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }
        public List<InvItemBO> GetItemBySupplier(int supplierId, int costCenterId)
        {
            List<InvItemBO> itemInfo = new List<InvItemBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemBySupplier_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        StockBy = r.Field<int>("StockBy"),
                        HeadName = r.Field<string>("HeadName"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice")

                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<InvItemBO> GetItemBySupplierNew(int supplierId, int costCenterId, int categoryId, string prNumber)
        {
            List<InvItemBO> itemInfo = new List<InvItemBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemBySupplierNew_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    if (categoryId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    if (prNumber != "0")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PRNumber", DbType.String, prNumber);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@PRNumber", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        StockBy = r.Field<int>("StockBy"),
                        HeadName = r.Field<string>("HeadName"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice")

                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<InvItemBO> GetDynamicallyItemInformationByCategoryId(int costCenterId, int categoryId)
        {
            List<InvItemBO> productList = new List<InvItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDynamicallyItemInformationByCategoryId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO productBO = new InvItemBO();
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.CategoryName = reader["CategoryName"].ToString();
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                                productBO.ImageName = reader["ImageName"].ToString();
                                productBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPrice"]);
                                productBO.IsItemEditable = Convert.ToBoolean(reader["IsItemEditable"]);
                                productBO.ItemWiseDiscountType = Convert.ToString(reader["ItemWiseDiscountType"]);
                                productBO.ItemWiseIndividualDiscount = Convert.ToDecimal(reader["ItemWiseIndividualDiscount"]);

                                productBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                productBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                productBO.CitySDCharge = Convert.ToDecimal(reader["CitySDCharge"]);
                                productBO.AdditionalChargeType = reader["AdditionalChargeType"].ToString();
                                productBO.AdditionalCharge = Convert.ToDecimal(reader["AdditionalCharge"]);

                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }
        public List<InvItemBO> GetItemNameAndItemIdByCategoryId(int costCenterId, int categoryId)
        {
            List<InvItemBO> productList = new List<InvItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemNameAndItemIdByCategoryId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO productBO = new InvItemBO();
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.CategoryName = reader["CategoryName"].ToString();
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();

                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }
        public List<InvItemBO> GetInvItemInfoBySearchCriteria(string itemType, string name, string code, int CategoryId)
        {
            List<InvItemBO> productList = new List<InvItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, itemType);
                    dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                    dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, code);
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, CategoryId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO productBO = new InvItemBO();
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.Description = reader["Description"].ToString();
                                productBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.ProductType = reader["ProductType"].ToString();
                                productBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                productBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                productBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                productBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                productBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                productBO.StockType = reader["UnitPriceUsd"].ToString();

                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }
        public List<InvItemBO> GetInvItemInfoBySearchCriteriaForPagination(string orderBy, string itemType, string name, string displayName, string code, string categoryId, string classification, int recordPerPage, int pageIndex, out int totalRecords)
        {
            int CategoryId = Convert.ToInt32(categoryId);
            int ClassificationId = Convert.ToInt32(classification);
            List<InvItemBO> productList = new List<InvItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemInfoBySearchCriteriaForPagination_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, itemType);

                    if (!string.IsNullOrEmpty(name))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(displayName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DisplayName", DbType.String, displayName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DisplayName", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(code))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, code);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, DBNull.Value);
                    }

                    if (CategoryId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, CategoryId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    }

                    if (ClassificationId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ClassificationId", DbType.Int32, ClassificationId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ClassificationId", DbType.Int32, DBNull.Value);
                    }
                    dbSmartAspects.AddInParameter(cmd, "@OrderBy", DbType.String, orderBy);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO productBO = new InvItemBO();
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.Description = reader["Description"].ToString();
                                productBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.ProductType = reader["ProductType"].ToString();
                                productBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                productBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                productBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                productBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                productBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                productBO.StockType = reader["UnitPriceUsd"].ToString();
                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                                productBO.Model = reader["Model"].ToString();

                                productList.Add(productBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return productList;
        }
        public List<InvItemBO> GetInvItemInfoByCategoryIdAndmanufacturerId(int categoryId, int manufacturerId)
        {
            List<InvItemBO> productList = new List<InvItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemInfoByCategoryIdAndmanufacturerId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    dbSmartAspects.AddInParameter(cmd, "@ManufacturerId", DbType.Int32, manufacturerId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO productBO = new InvItemBO();
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.Description = reader["Description"].ToString();
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                productBO.ProductType = reader["ProductType"].ToString();
                                productBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                productBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                productBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                productBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                productBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }
        public int InvItemDuplicateChecking(int isUpdate, int itemId, int categoryId, string itemName)
        {
            Boolean status = false;
            int IsDuplicate = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("InvItemDuplicateChecking_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@IsUpdate", DbType.Int32, isUpdate);
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, itemId);
                    dbSmartAspects.AddInParameter(command, "@CategoryId", DbType.Int32, categoryId);
                    dbSmartAspects.AddInParameter(command, "@ItemName", DbType.String, itemName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(command))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                IsDuplicate = Convert.ToInt32(reader["IsDuplicate"]);
                            }
                        }
                    }
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return IsDuplicate;
        }
        public List<InvItemBO> GetInvItemNServiceInfo()
        {
            List<InvItemBO> productList = new List<InvItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemNServiceInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO productBO = new InvItemBO();
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.Description = reader["Description"].ToString();
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                productBO.ProductType = reader["ProductType"].ToString();
                                productBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                productBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                productBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                productBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                productBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }
        public Boolean DeleteInvItemInfoByItemId(int itemId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteInvItemInfoByItemId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, itemId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean DeleteInvItemCostCenterMappingInfo(int itemId, string oldStockType, string newStockType)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteInvItemCostCenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, itemId);
                    dbSmartAspects.AddInParameter(command, "@OldStockType", DbType.String, oldStockType);
                    dbSmartAspects.AddInParameter(command, "@NewStockType", DbType.String, newStockType);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<InvItemInformationReportBO> GetInventoryItemInformation(int categoryId, int itemId, string adjustmentFrequency, int classificationId)
        {
            List<InvItemInformationReportBO> itemInfo = new List<InvItemInformationReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemInformationReport_SP"))
                {
                    if (categoryId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    }

                    if (itemId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(adjustmentFrequency))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AdjustmentFrequency", DbType.String, adjustmentFrequency);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AdjustmentFrequency", DbType.String, DBNull.Value);
                    }

                    if (classificationId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ClassificationId", DbType.Int32, classificationId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ClassificationId", DbType.Int32, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemInformationReportBO
                    {
                        CategoryName = r.Field<string>("CategoryName"),
                        Code = r.Field<string>("Code"),
                        Name = r.Field<string>("Name"),
                        ItemId = r.Field<int>("ItemId"),
                        StockBy = r.Field<string>("StockBy"),
                        AdjustmentFrequency = r.Field<string>("AdjustmentFrequency"),
                        ClassificationId = r.Field<int>("ClassificationId"),
                        Classification = r.Field<string>("Classification"),
                        CostCenter = r.Field<string>("CostCenter"),
                        CostCenterId = r.Field<int>("CostCenterId"),
                        UnitPriceLocal = r.Field<decimal?>("UnitPriceLocal"),
                        UnitPriceUsd = r.Field<decimal?>("UnitPriceUsd"),
                        ForeignCurrencyType = r.Field<string>("ForeignCurrencyType"),
                        LocalCurrencyType = r.Field<string>("LocalCurrencyType")

                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<InvItemInformationReportBO> GetInventoryItemInformationWitoutCostCenter(int categoryId, int itemId, string adjustmentFrequency, int classificationId)
        {
            List<InvItemInformationReportBO> itemInfo = new List<InvItemInformationReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemInfoWitoutCostCenterReport_SP"))
                {
                    if (categoryId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    }

                    if (itemId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(adjustmentFrequency))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AdjustmentFrequency", DbType.String, adjustmentFrequency);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AdjustmentFrequency", DbType.String, DBNull.Value);
                    }

                    if (classificationId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ClassificationId", DbType.Int32, classificationId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ClassificationId", DbType.Int32, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemInformationReportBO
                    {
                        CategoryName = r.Field<string>("CategoryName"),
                        Code = r.Field<string>("Code"),
                        Name = r.Field<string>("Name"),
                        ItemId = r.Field<int>("ItemId"),
                        StockBy = r.Field<string>("StockBy"),
                        AdjustmentFrequency = r.Field<string>("AdjustmentFrequency"),
                        ClassificationId = r.Field<int>("ClassificationId"),
                        Classification = r.Field<string>("Classification")
                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<InvItemBO> GetInventoryItemInformationByCategory(int costCenterId, int categoryId, bool? isCustomerItem, bool? isSupplierItem)
        {
            List<InvItemBO> productList = new List<InvItemBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemByCategoryId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (costCenterId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (isCustomerItem != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, isCustomerItem);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, DBNull.Value);

                    if (isSupplierItem != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsSupplierItem", DbType.Boolean, isSupplierItem);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsSupplierItem", DbType.Boolean, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO productBO = new InvItemBO();
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.CategoryName = reader["CategoryName"].ToString();
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                                productBO.ImageName = reader["ImageName"].ToString();
                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }
        public List<InvItemAutoSearchBO> GetItemNameWiseItemDetailsForAutoSearch(string itemName, int costCenterId, string isCustomerOrSupplierItem)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemNameWiseItemDetailsForAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@IsCustomerSupplierItemOrBoth", DbType.String, isCustomerOrSupplierItem);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        ItemName = r.Field<string>("ItemName"),
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        ImageName = r.Field<string>("ImageName"),
                        ProductType = r.Field<string>("ProductType"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitHead = r.Field<string>("UnitHead")

                    }).ToList();
                }
            }

            return itemInfo;
        }

        public List<InvItemAutoSearchBO> GetItemDetailsForAutoSearch(string itemName, int companyId, int costCenterId, string isCustomerOrSupplierItem, int categoryId, int supplierId, int locationId = 0)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemDetailsForAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);

                    if (categoryId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    if (costCenterId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (locationId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@IsCustomerSupplierItemOrBoth", DbType.String, isCustomerOrSupplierItem);
                    dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.String, supplierId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        ItemName = r.Field<string>("ItemName"),
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        ImageName = r.Field<string>("ImageName"),
                        ProductType = r.Field<string>("ProductType"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitHead = r.Field<string>("HeadName"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice"),
                        LastPurchaseDate = r.Field<DateTime?>("LastPurchaseDate"),
                        ManufacturerName = r.Field<string>("ManufacturerName"),
                        Model = r.Field<string>("Model"),
                        Description = r.Field<string>("Description"),
                        IsAttributeItem = r.Field<Boolean>("IsAttributeItem")
                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<InvItemAutoSearchBO> GetCompanyProjectWiseItemDetailsForAutoSearch(string itemName, int companyId, int projectId, int costCenterId, string isCustomerOrSupplierItem, int categoryId, int supplierId, int locationId = 0)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyProjectWiseItemDetailsForAutoSearch_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);

                    if (categoryId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    if (costCenterId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (locationId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@IsCustomerSupplierItemOrBoth", DbType.String, isCustomerOrSupplierItem);

                    if (supplierId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.String, supplierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        ItemName = r.Field<string>("ItemName"),
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        ImageName = r.Field<string>("ImageName"),
                        ProductType = r.Field<string>("ProductType"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitHead = r.Field<string>("HeadName"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice"),
                        LastPurchaseDate = r.Field<DateTime?>("LastPurchaseDate"),
                        ManufacturerName = r.Field<string>("ManufacturerName"),
                        Model = r.Field<string>("Model"),
                        Description = r.Field<string>("Description"),
                        IsAttributeItem = r.Field<Boolean>("IsAttributeItem"),
                    }).ToList();
                }
            }

            return itemInfo;
        }

        public List<InvItemAutoSearchBO> GetItemDetailsForAutoSearchWithOutSupplier(string itemName, int costCenterId, string isCustomerOrSupplierItem, int categoryId, int locationId = 0)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemDetailsForAutoSearchWithOutSupplier_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);

                    if (categoryId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    if (locationId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@IsCustomerSupplierItemOrBoth", DbType.String, isCustomerOrSupplierItem);


                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        ItemName = r.Field<string>("ItemName"),
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        ImageName = r.Field<string>("ImageName"),
                        ProductType = r.Field<string>("ProductType"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitHead = r.Field<string>("HeadName"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice"),
                        LastPurchaseDate = r.Field<DateTime?>("LastPurchaseDate")

                    }).ToList();
                }
            }

            return itemInfo;
        }

        public List<InvItemAutoSearchBO> GetItemForAutoSearchWithoutSupplier(string type, string itemName, int companyId, int projectId, int costCenterId, string isCustomerOrSupplierItem, int categoryId, int locationId = 0)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                if (type == "Transfer")
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAvailableItemForAutoSearchWithoutSupplier_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);

                        if (categoryId > 0)
                            dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                        if (locationId > 0)
                            dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@IsCustomerSupplierItemOrBoth", DbType.String, isCustomerOrSupplierItem);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                        DataTable Table = ds.Tables["ItemInfo"];

                        itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                        {
                            ItemId = r.Field<int>("ItemId"),
                            Name = r.Field<string>("Name"),
                            ItemName = r.Field<string>("ItemName"),
                            Code = r.Field<string>("Code"),
                            CategoryId = r.Field<int>("CategoryId"),
                            ImageName = r.Field<string>("ImageName"),
                            ProductType = r.Field<string>("ProductType"),
                            StockBy = r.Field<int>("StockBy"),
                            UnitHead = r.Field<string>("HeadName"),
                            //StockQuantity = r.Field<decimal>("StockQuantity"),
                            PurchasePrice = r.Field<decimal>("PurchasePrice"),
                            LastPurchaseDate = r.Field<DateTime?>("LastPurchaseDate")

                        }).ToList();
                    }
                }
                else if (type == "Requisition")
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAvailableItemForAutoSearchForRequisitionWithoutSupplier_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);

                        if (categoryId > 0)
                            dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                        if (locationId > 0)
                            dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@IsCustomerSupplierItemOrBoth", DbType.String, isCustomerOrSupplierItem);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "ItemList");
                        DataTable Table = ds.Tables["ItemList"];

                        itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                        {
                            ItemId = r.Field<int>("ItemId"),
                            Name = r.Field<string>("Name"),
                            ItemName = r.Field<string>("ItemName"),
                            Code = r.Field<string>("Code"),
                            CategoryId = r.Field<int>("CategoryId"),
                            ImageName = r.Field<string>("ImageName"),
                            ProductType = r.Field<string>("ProductType"),
                            StockBy = r.Field<int>("StockBy"),
                            UnitHead = r.Field<string>("HeadName"),
                            //StockQuantity = r.Field<decimal>("StockQuantity"),
                            PurchasePrice = r.Field<decimal>("PurchasePrice"),
                            LastPurchaseDate = r.Field<DateTime?>("LastPurchaseDate")

                        }).ToList();
                    }
                }
            }

            return itemInfo;
        }


        public List<InvItemAutoSearchBO> GetNameNModelWiseItemDetailsForAutoSearch(string searchTerm)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNameNModelWiseItemDetailsForAutoSearch_SP"))
                {
                    if (!string.IsNullOrEmpty(searchTerm))
                        dbSmartAspects.AddInParameter(cmd, "@SearchTerm", DbType.String, searchTerm);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SearchTerm", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        ProductType = r.Field<string>("ProductType"),
                        StockBy = r.Field<int>("StockBy"),
                        StockType = r.Field<string>("StockType"),
                        UnitHead = r.Field<string>("UnitHead")

                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<InvItemAutoSearchBO> GetItemNameNCategoryForAutoSearch(string itemName, int categoryId, bool? isCustomerItem, bool? isSupplierItem, bool isCatgoryConcatWithItem)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemNameNCategoryForAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);

                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (isCustomerItem != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, isCustomerItem);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, DBNull.Value);

                    if (isSupplierItem != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsSupplierItem", DbType.Boolean, isSupplierItem);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsSupplierItem", DbType.Boolean, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    if (isCatgoryConcatWithItem)
                    {
                        itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                        {
                            ItemId = r.Field<int>("ItemId"),
                            Name = r.Field<string>("Name") + " (" + r.Field<string>("CategoryName") + ")",
                            Code = r.Field<string>("Code"),
                            CategoryId = r.Field<int>("CategoryId"),
                            ImageName = r.Field<string>("ImageName"),
                            IsRecipe = r.Field<bool>("IsRecipe")

                        }).ToList();
                    }
                    else
                    {
                        itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                        {
                            ItemId = r.Field<int>("ItemId"),
                            Name = r.Field<string>("Name"),
                            Code = r.Field<string>("Code"),
                            CategoryId = r.Field<int>("CategoryId"),
                            CategoryName = r.Field<string>("CategoryName"),
                            ImageName = r.Field<string>("ImageName")

                        }).ToList();
                    }
                }
            }

            return itemInfo;
        }
        public List<InvItemAutoSearchBO> GetItemNameNCategoryCostcenterForAutoSearch(string itemName, int categoryId, bool? isCustomerItem, bool? isSupplierItem, int costCenterId, bool isCatgoryConcatWithItem)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemNameNCategoryCostcenterForAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);

                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (isCustomerItem != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, isCustomerItem);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, DBNull.Value);

                    if (isSupplierItem != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsSupplierItem", DbType.Boolean, isSupplierItem);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsSupplierItem", DbType.Boolean, DBNull.Value);

                    if (costCenterId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    if (isCatgoryConcatWithItem)
                    {
                        itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                        {
                            ItemId = r.Field<int>("ItemId"),
                            //Name = r.Field<string>("Name") + " (" + r.Field<string>("CategoryName") + ")",
                            Name = r.Field<string>("Code") + " - " + r.Field<string>("Name"),
                            Code = r.Field<string>("Code"),
                            CategoryId = r.Field<int>("CategoryId"),
                            ImageName = r.Field<string>("ImageName"),
                            StockBy = r.Field<int>("StockBy"),
                            UnitHead = r.Field<string>("UnitHead"),
                            IsAttributeItem = r.Field<Boolean>("IsAttributeItem")
                        }).ToList();
                    }
                    else
                    {
                        itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                        {
                            ItemId = r.Field<int>("ItemId"),
                            Name = r.Field<string>("Name"),
                            Code = r.Field<string>("Code"),
                            CategoryId = r.Field<int>("CategoryId"),
                            CategoryName = r.Field<string>("CategoryName"),
                            ImageName = r.Field<string>("ImageName"),
                            StockBy = r.Field<int>("StockBy"),
                            UnitHead = r.Field<string>("UnitHead"),
                            IsAttributeItem = r.Field<Boolean>("IsAttributeItem")
                        }).ToList();
                    }
                }
            }

            return itemInfo;
        }
        public List<InvItemAutoSearchBO> GetItemNameNCategoryCostcenterLocationForAutoSearch(string itemName, int categoryId, bool? isCustomerItem, bool? isSupplierItem, int costCenterId, int locationId, bool isCatgoryConcatWithItem)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemNameNCategoryCostcenterLocationForAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);

                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (isCustomerItem != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, isCustomerItem);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, DBNull.Value);

                    if (isSupplierItem != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsSupplierItem", DbType.Boolean, isSupplierItem);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsSupplierItem", DbType.Boolean, DBNull.Value);

                    if (costCenterId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (locationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    if (isCatgoryConcatWithItem)
                    {
                        itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                        {
                            ItemId = r.Field<int>("ItemId"),
                            Name = r.Field<string>("Name") + " (" + r.Field<string>("CategoryName") + ")",
                            Code = r.Field<string>("Code"),
                            CategoryId = r.Field<int>("CategoryId"),
                            StockQuantity = r.Field<decimal>("StockQuantity"),
                            AverageCost = r.Field<decimal>("AverageCost"),
                            ImageName = r.Field<string>("ImageName")

                        }).ToList();
                    }
                    else
                    {
                        itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                        {
                            ItemId = r.Field<int>("ItemId"),
                            Name = r.Field<string>("Name"),
                            Code = r.Field<string>("Code"),
                            CategoryId = r.Field<int>("CategoryId"),
                            CategoryName = r.Field<string>("CategoryName"),
                            StockQuantity = r.Field<decimal>("StockQuantity"),
                            AverageCost = r.Field<decimal>("AverageCost"),
                            ImageName = r.Field<string>("ImageName")

                        }).ToList();
                    }
                }
            }

            return itemInfo;
        }
        public List<InvItemAutoSearchBO> GetItemNameForAutoSearch(string itemName, int categoryId, int costcenterId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemNameForAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);
                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costcenterId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        ItemNameAndCode = r.Field<string>("ItemNameAndCode"),
                        CategoryId = r.Field<int>("CategoryId"),
                        ImageName = r.Field<string>("ImageName"),
                        StockBy = r.Field<int>("StockBy"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        UnitPrice = r.Field<decimal>("UnitPrice")

                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<InvItemAutoSearchBO> GetNameCategoryCostcenterWiseItemDetailsForAutoSearch(string searchTerm, int categoryId, int costcenterId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNameCategoryCostcenterWiseItemDetailsForAutoSearch_SP"))
                {
                    if (!string.IsNullOrEmpty(searchTerm))
                        dbSmartAspects.AddInParameter(cmd, "@SearchTerm", DbType.String, searchTerm);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SearchTerm", DbType.String, DBNull.Value);

                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (costcenterId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostcenterId", DbType.Int32, costcenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostcenterId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        ProductType = r.Field<string>("ProductType")

                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<RestaurantRecipeDetailBO> GetRecipeItemInfoByItemId(int itemId)
        {
            int count = 0;
            List<RestaurantRecipeDetailBO> recipeItemList = new List<RestaurantRecipeDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemRecipeInfoByItemId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    recipeItemList = Table.AsEnumerable().Select(r => new RestaurantRecipeDetailBO
                    {
                        RecipeId = r.Field<Int32>("RecipeId"),
                        RecipeItemId = r.Field<Int32>("RecipeItemId"),
                        RecipeItemName = r.Field<string>("RecipeItemName"),
                        ItemUnit = r.Field<decimal>("ItemUnit"),
                        ItemCost = r.Field<decimal>("ItemCost"),
                        UnitHeadId = r.Field<Int32>("UnitHeadId"),
                        HeadName = r.Field<string>("HeadName"),
                        IsGradientCanChange = r.Field<bool>("IsGradientCanChange")

                    }).ToList();
                }
            }
            return recipeItemList;
        }
        
        public List<OverheadExpensesBO> GetInvFGNNutrientRequiredOEById(int itemId)
        {
            List<OverheadExpensesBO> accountHeadList = new List<OverheadExpensesBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvFGNNutrientRequiredOEById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "OEInfo");
                    DataTable Table = ds.Tables["OEInfo"];

                    accountHeadList = Table.AsEnumerable().Select(r => new OverheadExpensesBO
                    {
                        FinishProductId = r.Field<Int32>("FGId"),
                        NodeId = r.Field<Int32>("NodeId"),
                        AccountHead = r.Field<string>("AccountHead"),
                        Amount = r.Field<decimal>("OEAmount"),
                        Remarks = r.Field<string>("OERemarks"),
                        HeadWithCode = r.Field<string>("HeadWithCode")
                    }).ToList();
                }
            }
            return accountHeadList;
        }
        public RestaurantRecipeDetailBO GetReceipeItemCost(int itemId, int stockById, decimal quantity)
        {
            RestaurantRecipeDetailBO recipeItem = new RestaurantRecipeDetailBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReceipeItemCost_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    dbSmartAspects.AddInParameter(cmd, "@StockById", DbType.Int32, stockById);
                    dbSmartAspects.AddInParameter(cmd, "@Quantity", DbType.Decimal, quantity);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    recipeItem = Table.AsEnumerable().Select(r => new RestaurantRecipeDetailBO
                    {
                        ItemCost = r.Field<decimal>("ItemCost")

                    }).FirstOrDefault();
                }
            }
            return recipeItem;
        }
        public List<ItemWiseStockReportViewBO> GetItemWiseStockInfoForReport(DateTime stockDate, string reportType, int costCenterId, int locationId, int categoryId, int itemId, int colorId, int sizeId, int styleId, string stockType, string showTransaction,
                                                                            int manufacturerId, int classificationId, string model, int companyId, int projectId)
        {
            List<ItemWiseStockReportViewBO> itemWiseStockList = new List<ItemWiseStockReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemWiseStockForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@StockDate", DbType.DateTime, stockDate);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);

                    if (costCenterId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (locationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (itemId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);

                    if (colorId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ColorId", DbType.Int32, colorId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ColorId", DbType.Int32, DBNull.Value);

                    if (sizeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@SizeId", DbType.Int32, sizeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SizeId", DbType.Int32, DBNull.Value);

                    if (styleId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@StyleId", DbType.Int32, styleId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@StyleId", DbType.Int32, DBNull.Value);

                    if (stockType != "0")
                        dbSmartAspects.AddInParameter(cmd, "@StockType", DbType.String, stockType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@StockType", DbType.String, DBNull.Value);

                    if (manufacturerId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ManufacturerId", DbType.Int32, manufacturerId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ManufacturerId", DbType.Int32, DBNull.Value);

                    if (classificationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ClassificationId", DbType.Int32, classificationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ClassificationId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(model))
                        dbSmartAspects.AddInParameter(cmd, "@Model", DbType.String, model);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Model", DbType.String, DBNull.Value);

                    if (showTransaction == "WithoutZero")
                        dbSmartAspects.AddInParameter(cmd, "@StockQuantityWithZero", DbType.Int32, 1);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@StockQuantityWithZero", DbType.Int32, DBNull.Value);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];

                    if (reportType == "ItemWiseStock")
                    {
                        itemWiseStockList = Table.AsEnumerable().Select(r => new ItemWiseStockReportViewBO
                        {
                            CategoryId = r.Field<Int32?>("CategoryId"),
                            CategoryName = r.Field<string>("CategoryName"),
                            ItemId = r.Field<Int32?>("ItemId"),
                            ItemName = r.Field<string>("ItemName"),
                            ColorName = r.Field<string>("ColorName"),
                            SizeName = r.Field<string>("SizeName"),
                            StyleName = r.Field<string>("StyleName"),
                            StockQuantity = r.Field<decimal>("StockQuantity"),
                            AverageCost = r.Field<decimal?>("AverageCost"),
                            HeadName = r.Field<string>("HeadName"),
                            SerialNumber = r.Field<string>("SerialNumber"),
                            SerialStatus = r.Field<string>("SerialStatus"),
                            Code = r.Field<string>("Code"),
                            ManufacturerName = r.Field<string>("ManufacturerName"),
                            Model = r.Field<string>("Model")
                        }).ToList();
                    }
                    else if (reportType == "CategoryWiseStock")
                    {
                        itemWiseStockList = Table.AsEnumerable().Select(r => new ItemWiseStockReportViewBO
                        {
                            CategoryId = r.Field<Int32?>("CategoryId"),
                            CategoryName = r.Field<string>("CategoryName"),
                            ItemId = r.Field<Int32?>("ItemId"),
                            ItemName = r.Field<string>("ItemName"),
                            ColorName = r.Field<string>("ColorName"),
                            SizeName = r.Field<string>("SizeName"),
                            StyleName = r.Field<string>("StyleName"),
                            StockQuantity = r.Field<decimal>("StockQuantity"),
                            AverageCost = r.Field<decimal?>("AverageCost"),
                            HeadName = r.Field<string>("HeadName"),
                            SerialNumber = r.Field<string>("SerialNumber"),
                            SerialStatus = r.Field<string>("SerialStatus"),
                            Code = r.Field<string>("Code"),
                            ManufacturerName = r.Field<string>("ManufacturerName"),
                            Model = r.Field<string>("Model")
                        }).ToList();
                    }
                    else if (reportType == "CostcenterWiseStock")
                    {
                        itemWiseStockList = Table.AsEnumerable().Select(r => new ItemWiseStockReportViewBO
                        {
                            ItemId = r.Field<Int32?>("ItemId"),
                            ItemName = r.Field<string>("ItemName"),
                            ColorName = r.Field<string>("ColorName"),
                            SizeName = r.Field<string>("SizeName"),
                            StyleName = r.Field<string>("StyleName"),
                            CostCenterId = r.Field<Int32?>("CostCenterId"),
                            CostCenter = r.Field<string>("CostCenter"),
                            StockQuantity = r.Field<decimal>("StockQuantity"),
                            AverageCost = r.Field<decimal?>("AverageCost"),
                            HeadName = r.Field<string>("HeadName"),
                            SerialNumber = r.Field<string>("SerialNumber"),
                            SerialStatus = r.Field<string>("SerialStatus"),
                            Code = r.Field<string>("Code"),
                            ManufacturerName = r.Field<string>("ManufacturerName"),
                            Model = r.Field<string>("Model")
                        }).ToList();
                    }
                    else if (reportType == "LocationWiseStock")
                    {
                        itemWiseStockList = Table.AsEnumerable().Select(r => new ItemWiseStockReportViewBO
                        {
                            ItemId = r.Field<Int32?>("ItemId"),
                            ItemName = r.Field<string>("ItemName"),
                            ColorName = r.Field<string>("ColorName"),
                            SizeName = r.Field<string>("SizeName"),
                            StyleName = r.Field<string>("StyleName"),
                            CategoryName = r.Field<string>("CategoryName"),
                            CostCenterId = r.Field<Int32?>("CostCenterId"),
                            CostCenter = r.Field<string>("CostCenter"),
                            LocationId = r.Field<Int32?>("LocationId"),
                            LocationName = r.Field<string>("LocationName"),
                            StockQuantity = r.Field<decimal>("StockQuantity"),
                            AverageCost = r.Field<decimal?>("AverageCost"),
                            HeadName = r.Field<string>("HeadName"),
                            SerialNumber = r.Field<string>("SerialNumber"),
                            SerialStatus = r.Field<string>("SerialStatus"),
                            Code = r.Field<string>("Code"),
                            ManufacturerName = r.Field<string>("ManufacturerName"),
                            Model = r.Field<string>("Model")
                        }).ToList();
                    }
                    else if (reportType == "ProjectWiseStock")
                    {
                        itemWiseStockList = Table.AsEnumerable().Select(r => new ItemWiseStockReportViewBO
                        {
                            ItemId = r.Field<Int32?>("ItemId"),
                            ItemName = r.Field<string>("ItemName"),
                            ColorName = r.Field<string>("ColorName"),
                            SizeName = r.Field<string>("SizeName"),
                            StyleName = r.Field<string>("StyleName"),
                            CategoryName = r.Field<string>("CategoryName"),
                            CostCenterId = r.Field<Int32?>("CostCenterId"),
                            CostCenter = r.Field<string>("CostCenter"),
                            LocationId = r.Field<Int32?>("LocationId"),
                            LocationName = r.Field<string>("LocationName"),
                            StockQuantity = r.Field<decimal>("StockQuantity"),
                            AverageCost = r.Field<decimal?>("AverageCost"),
                            HeadName = r.Field<string>("HeadName"),
                            SerialNumber = r.Field<string>("SerialNumber"),
                            SerialStatus = r.Field<string>("SerialStatus"),
                            Code = r.Field<string>("Code"),
                            ManufacturerName = r.Field<string>("ManufacturerName"),
                            Model = r.Field<string>("Model"),
                            CompanyName = r.Field<string>("CompanyName"),
                            ProjectName = r.Field<string>("ProjectName")
                        }).ToList();
                    }
                }
            }

            return itemWiseStockList;
        }
        public List<ItemWiseStockReportViewBO> GetProjectStockInfoForReport(int projectId, int costCenterId, int locationId, int categoryId, int itemId, string stockType, string showTransaction,
                                                                            int manufacturerId, int classificationId, string model)
        {
            List<ItemWiseStockReportViewBO> itemWiseStockList = new List<ItemWiseStockReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProjectStockInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);

                    if (costCenterId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (locationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (itemId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);

                    if (stockType != "0")
                        dbSmartAspects.AddInParameter(cmd, "@StockType", DbType.String, stockType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@StockType", DbType.String, DBNull.Value);
                    if (manufacturerId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ManufacturerId", DbType.Int32, manufacturerId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ManufacturerId", DbType.Int32, DBNull.Value);

                    if (classificationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ClassificationId", DbType.Int32, classificationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ClassificationId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(model))
                        dbSmartAspects.AddInParameter(cmd, "@Model", DbType.String, model);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Model", DbType.String, DBNull.Value);

                    if (showTransaction == "WithoutZero")
                        dbSmartAspects.AddInParameter(cmd, "@StockQuantityWithZero", DbType.Int32, 1);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@StockQuantityWithZero", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];

                    itemWiseStockList = Table.AsEnumerable().Select(r => new ItemWiseStockReportViewBO
                    {
                        ItemId = r.Field<Int32?>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        CategoryName = r.Field<string>("CategoryName"),
                        CostCenterId = r.Field<Int32?>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        LocationId = r.Field<Int32?>("LocationId"),
                        LocationName = r.Field<string>("LocationName"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        AverageCost = r.Field<decimal?>("AverageCost"),
                        HeadName = r.Field<string>("HeadName"),
                        SerialNumber = r.Field<string>("SerialNumber"),
                        SerialStatus = r.Field<string>("SerialStatus"),
                        Code = r.Field<string>("Code"),
                        ManufacturerName = r.Field<string>("ManufacturerName"),
                        Model = r.Field<string>("Model")
                    }).ToList();
                }
            }

            return itemWiseStockList;
        }
        public List<ItemWiseStockReportViewBO> GetCostcenterLocationWiseItemStock(int costCenterId, int locationId, int itemId)
        {
            List<ItemWiseStockReportViewBO> itemWiseStockList = new List<ItemWiseStockReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCostcenterLocationWiseItemStock_SP"))
                {
                    if (costCenterId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (locationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    if (itemId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];

                    itemWiseStockList = Table.AsEnumerable().Select(r => new ItemWiseStockReportViewBO
                    {
                        ItemId = r.Field<Int32?>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        CostCenterId = r.Field<Int32?>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        LocationId = r.Field<Int32?>("LocationId"),
                        LocationName = r.Field<string>("LocationName"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        StockById = r.Field<Int32>("StockById"),
                        HeadName = r.Field<string>("HeadName")

                    }).ToList();
                }
            }

            return itemWiseStockList;
        }
        public List<RecipeReportBO> GetItemRecipeReport(int itemId)
        {
            List<RecipeReportBO> itemRecipe = new List<RecipeReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemRecipeReport_SP"))
                {
                    if (itemId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];

                    itemRecipe = Table.AsEnumerable().Select(r => new RecipeReportBO
                    {
                        ItemId = r.Field<Int32>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        RecipeItemId = r.Field<Int32>("RecipeItemId"),
                        RecipeItemName = r.Field<string>("RecipeItemName"),
                        RecipeItemCodeAndName = r.Field<string>("RecipeItemCodeAndName"),
                        ItemUnit = r.Field<decimal>("ItemUnit"),
                        ItemCost = r.Field<decimal>("ItemCost"),
                        HeadName = r.Field<string>("HeadName"),
                        PreparationInstructions = r.Field<string>("PreparationInstructions"),
                        ServingInstructions = r.Field<string>("ServingInstructions"),
                        IngredientPrepComments = r.Field<string>("IngredientPrepComments"),
                        ItemRank = r.Field<Int64>("ItemRank")

                    }).ToList();
                }
            }

            return itemRecipe;
        }
        public List<RecipeReportBO> GetFinishedGoodsNutrientSummaryInformation(int itemId)
        {
            List<RecipeReportBO> itemRecipe = new List<RecipeReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFinishedGoodsNutrientSummaryInformation_SP"))
                {
                    if (itemId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];

                    itemRecipe = Table.AsEnumerable().Select(r => new RecipeReportBO
                    {
                        ItemId = r.Field<Int32>("ItemId"),
                        FinishedGoodsCode = r.Field<string>("FinishedGoodsCode"),
                        FinishedGoodsName = r.Field<string>("FinishedGoodsName"),
                        FinishedGoodsCodeAndName = r.Field<string>("FinishedGoodsCodeAndName"),
                        TotalRawMaterialsCost = r.Field<decimal>("TotalRawMaterialsCost"),
                        TotalOverheadCost = r.Field<decimal>("TotalOverheadCost"),
                        TotalCost = r.Field<decimal>("TotalCost"),
                        SalesRate = r.Field<decimal>("SalesRate"),
                        ProfitAndLoss = r.Field<decimal>("ProfitAndLoss"),
                    }).ToList();
                }
            }

            return itemRecipe;
        }
        public List<RecipeReportBO> GetItemNutrientInformationReport(int itemId)
        {
            List<RecipeReportBO> itemRecipe = new List<RecipeReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvNutrientInformationByFGId_SP"))
                {                    
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];

                    itemRecipe = Table.AsEnumerable().Select(r => new RecipeReportBO
                    {
                        ItemId = r.Field<Int32>("ItemId"),
                        FinishedGoodsName = r.Field<string>("FinishedGoodsName"),
                        FinishedGoodsCode = r.Field<string>("FinishedGoodsCode"),
                        NutrientTypeName = r.Field<string>("NutrientTypeName"),
                        NutrientTypeCode = r.Field<string>("NutrientTypeCode"),
                        NutrientName = r.Field<string>("NutrientName"),
                        NutrientCode = r.Field<string>("NutrientCode"),
                        RequiredValue = r.Field<decimal>("RequiredValue"),
                        CalculatedFormula = r.Field<decimal>("CalculatedFormula"),
                        CalculatedValue = r.Field<decimal>("CalculatedValue"),

                    }).ToList();
                }
            }

            return itemRecipe;
        }
        public List<InvItemAutoSearchBO> GetItemNameNCategoryForRecipeAutoSearch(string itemName, int categoryId, bool? isCustomerItem, string itemType)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemNameNCategoryForRecipeAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);

                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (isCustomerItem != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, isCustomerItem);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, DBNull.Value);

                    if (!string.IsNullOrEmpty(itemType))
                        dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, itemType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name") + " (" + r.Field<string>("CategoryName") + ")",
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        IsRecipe = r.Field<bool>("IsRecipe"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        UnitPriceUsd = r.Field<decimal>("UnitPriceUsd"),
                        UnitHead = r.Field<string>("HeadName")

                    }).ToList();
                }
            }

            return itemInfo;
        }

        
        public List<InvItemAutoSearchBO> RawMaterialAutoSearch(string itemName)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("RawMaterialAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name") + " (" + r.Field<string>("CategoryName") + ")",
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        IsRecipe = r.Field<bool>("IsRecipe"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        UnitPriceUsd = r.Field<decimal>("UnitPriceUsd"),
                        UnitHead = r.Field<string>("HeadName")

                    }).ToList();
                }
            }

            return itemInfo;
        }


        public List<InvItemAutoSearchBO> RawMaterialAutoSearch(string itemName, int categoryId, bool? isCustomerItem, string itemType)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemNameNCategoryForRecipeAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);

                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (isCustomerItem != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, isCustomerItem);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, DBNull.Value);

                    if (!string.IsNullOrEmpty(itemType))
                        dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, itemType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name") + " (" + r.Field<string>("CategoryName") + ")",
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        IsRecipe = r.Field<bool>("IsRecipe"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        UnitPriceUsd = r.Field<decimal>("UnitPriceUsd"),
                        UnitHead = r.Field<string>("HeadName")

                    }).ToList();
                }
            }

            return itemInfo;
        }

        public List<InvItemAutoSearchBO> GetItemNameNCategoryForRecipeModifierAutoSearch(string itemName, int categoryId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemNameNCategoryForRecipeModifierAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);

                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name") + " (" + r.Field<string>("CategoryName") + ")",
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        IsRecipe = r.Field<bool>("IsRecipe"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        UnitPriceUsd = r.Field<decimal>("UnitPriceUsd"),
                        UnitHead = r.Field<string>("HeadName")

                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<InvItemAutoSearchBO> GetItemServiceByCategory(string itemName, int categoryId, bool? isCustomerItem)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemServiceByCategory_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);

                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (isCustomerItem != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, isCustomerItem);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name") + " (" + r.Field<string>("CategoryName") + ")",
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        IsRecipe = r.Field<bool>("IsRecipe"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        UnitPriceUsd = r.Field<decimal>("UnitPriceUsd"),
                        UnitHead = r.Field<string>("HeadName"),
                        AverageCost = r.Field<decimal>("AverageCost")
                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<InvItemAutoSearchBO> GetItemByCodeCategoryNameWiseItemDetailsForAutoSearch(string itemCode, string itemName, string categoryName, int costCenterId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemByCodeCategoryNameWiseItemDetailsForAutoSearch_SP"))
                {
                    if (!string.IsNullOrEmpty(itemCode))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemCode", DbType.String, itemCode);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemCode", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(itemName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(categoryName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemCategory", DbType.String, categoryName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemCategory", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitHead = r.Field<string>("UnitHead"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        DiscountType = r.Field<string>("DiscountType"),
                        DiscountAmount = r.Field<decimal>("DiscountAmount"),
                        StockQuantity = r.Field<decimal>("StockQuantity")

                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<InvItemAutoSearchBO> GetItemByCodeCategoryNameWiseItemDetailsForAutoSearchForBilling(string itemCode, string itemName, string categoryName, int costCenterId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemByCodeCategoryNameWiseItemDetailsForAutoSearchForBilling_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (!string.IsNullOrEmpty(itemCode))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemCode", DbType.String, itemCode);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemCode", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(itemName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(categoryName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemCategory", DbType.String, categoryName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemCategory", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitHead = r.Field<string>("UnitHead"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        DiscountType = r.Field<string>("DiscountType"),
                        DiscountAmount = r.Field<decimal>("DiscountAmount"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        ServiceCharge = r.Field<decimal>("ServiceCharge"),
                        SDCharge = r.Field<decimal>("SDCharge"),
                        VatAmount = r.Field<decimal>("VatAmount"),
                        AdditionalCharge = r.Field<decimal>("AdditionalCharge"),
                        IsAttributeItem = r.Field<bool>("IsAttributeItem"),
                        ProductType = r.Field<string>("ProductType")
                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<InvItemAutoSearchBO> GetItemByCodeColorSizeStyleCategoryNameWiseItemDetailsForAutoSearchForBilling(string itemCode, string itemName, int colorId, int sizeId, int styleId, string categoryName, int costCenterId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemByCodeColorSizeStyleCategoryNameWiseItemDetailsForAutoSearchForBilling_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (!string.IsNullOrEmpty(itemCode))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemCode", DbType.String, itemCode);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemCode", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(itemName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(categoryName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemCategory", DbType.String, categoryName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemCategory", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@ColorId", DbType.Int32, colorId);
                    dbSmartAspects.AddInParameter(cmd, "@SizeId", DbType.Int32, sizeId);
                    dbSmartAspects.AddInParameter(cmd, "@StyleId", DbType.Int32, styleId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),


                        ColorId = r.Field<int>("ColorId"),
                        ColorText = r.Field<string>("ColorText"),
                        SizeId = r.Field<int>("SizeId"),
                        SizeText = r.Field<string>("SizeText"),
                        StyleId = r.Field<int>("StyleId"),
                        StyleText = r.Field<string>("StyleText"),


                        StockBy = r.Field<int>("StockBy"),
                        UnitHead = r.Field<string>("UnitHead"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        DiscountType = r.Field<string>("DiscountType"),
                        DiscountAmount = r.Field<decimal>("DiscountAmount"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        ServiceCharge = r.Field<decimal>("ServiceCharge"),
                        SDCharge = r.Field<decimal>("SDCharge"),
                        VatAmount = r.Field<decimal>("VatAmount"),
                        AdditionalCharge = r.Field<decimal>("AdditionalCharge"),
                        IsAttributeItem = r.Field<bool>("IsAttributeItem")

                    }).ToList();
                }
            }

            return itemInfo;
        }
        //----------------------- Stock adjustment ---------------------------------------------------------------------------
        public List<ItemAdjustmentDetailsByItemAccessFrequencyBO> GetItemAdjustmentDetailsByItemAccessFrequency(int companyId, int projectId, int costCenterId, int locationId, int categoryId, DateTime adjustmentDate, string adjustmentFrequency, int itemId)
        {
            List<ItemAdjustmentDetailsByItemAccessFrequencyBO> itemWiseStockList = new List<ItemAdjustmentDetailsByItemAccessFrequencyBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemAdjustmentDetailsByItemAccessFrequency_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@AdjustmentFrequency", DbType.String, adjustmentFrequency);

                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);

                    if (costCenterId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (locationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    if (itemId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@AdjustmentDate", DbType.DateTime, adjustmentDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];

                    itemWiseStockList = Table.AsEnumerable().Select(r => new ItemAdjustmentDetailsByItemAccessFrequencyBO
                    {
                        ItemId = r.Field<Int32?>("ItemId"),
                        ColorId = r.Field<int>("ColorId"),
                        SizeId = r.Field<int>("SizeId"),
                        StyleId = r.Field<int>("StyleId"),
                        ItemName = r.Field<string>("ItemName"),
                        StockById = r.Field<int?>("StockById"),
                        StockBy = r.Field<string>("StockBy"),
                        OpeningStock = r.Field<decimal?>("OpeningStock"),
                        ReceivedQuantity = r.Field<decimal?>("ReceivedQuantity"),
                        ActualUsageQuantity = r.Field<decimal?>("ActualUsageQuantity"),
                        WastageQuantity = r.Field<decimal?>("WastageQuantity"),
                        StockQuantity = r.Field<decimal?>("StockQuantity"),
                        StockQuantityAfterWastageDeduction = r.Field<decimal?>("StockQuantityAfterWastageDeduction"),
                        AdjustmentFrequency = r.Field<string>("AdjustmentFrequency"),
                        ProductType = r.Field<string>("ProductType")

                    }).ToList();
                }
            }

            return itemWiseStockList;
        }
        public bool SaveItemStockAdjustment(ItemStockAdjustmentBO stockAdjustment, List<ItemStockAdjustmentDetailsBO> stockAdjustmentDetails, List<InvItemStockAdjustmentSerialInfoBO> AddedSerialzableProduct, out int stockAdjustmentId)
        {
            int status = 0;
            bool retVal = false;
            stockAdjustmentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("SaveItemStockAdjustment_SP"))
                        {
                            commandOut.Parameters.Clear();
                            dbSmartAspects.AddInParameter(commandOut, "@CompanyId", DbType.Int32, stockAdjustment.CompanyId);
                            dbSmartAspects.AddInParameter(commandOut, "@ProjectId", DbType.Int32, stockAdjustment.ProjectId);
                            dbSmartAspects.AddInParameter(commandOut, "@AdjustmentDate", DbType.DateTime, stockAdjustment.AdjustmentDate);
                            dbSmartAspects.AddInParameter(commandOut, "@CostCenterId", DbType.Int32, stockAdjustment.CostCenterId);
                            dbSmartAspects.AddInParameter(commandOut, "@LocationId", DbType.Int32, stockAdjustment.LocationId);
                            dbSmartAspects.AddInParameter(commandOut, "@AdjustmentFrequency", DbType.String, stockAdjustment.AdjustmentFrequency);
                            dbSmartAspects.AddInParameter(commandOut, "@ApprovedStatus", DbType.String, stockAdjustment.ApprovedStatus);
                            dbSmartAspects.AddInParameter(commandOut, "@CreatedBy", DbType.Int32, stockAdjustment.CreatedBy);
                            dbSmartAspects.AddOutParameter(commandOut, "@StockAdjustmentId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);
                            stockAdjustmentId = Convert.ToInt32(commandOut.Parameters["@StockAdjustmentId"].Value);
                        }

                        if (status > 0 && stockAdjustmentDetails.Count > 0)
                        {
                            foreach (ItemStockAdjustmentDetailsBO sad in stockAdjustmentDetails)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveItemStockAdjustmentDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockAdjustmentId", DbType.Int32, stockAdjustmentId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, sad.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationId", DbType.Int32, sad.LocationId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, sad.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OpeningQuantity", DbType.Decimal, sad.OpeningQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ReceiveQuantity", DbType.Decimal, sad.ReceiveQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ActualUsage", DbType.Decimal, sad.ActualUsage);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@WastageQuantity", DbType.Decimal, sad.WastageQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockQuantity", DbType.Decimal, sad.StockQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ActualQuantity", DbType.Decimal, sad.ActualQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, sad.ColorId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, sad.SizeId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, sad.StyleId);
                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }
                        if (status > 0 && AddedSerialzableProduct.Count > 0)
                        {
                            foreach (InvItemStockAdjustmentSerialInfoBO sad in AddedSerialzableProduct)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveItemStockAdjustmentSerial_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockAdjustmentId", DbType.Int32, stockAdjustmentId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, sad.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationId", DbType.Int32, stockAdjustment.LocationId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@SerialNumber", DbType.String, sad.SerialNumber);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public bool UpdateItemStockAdjustment(ItemStockAdjustmentBO stockAdjustment, List<ItemStockAdjustmentDetailsBO> stockAdjustmentDetails, List<ItemStockAdjustmentDetailsBO> stockAdjustmentDetailsEdit, List<ItemStockAdjustmentDetailsBO> stockAdjustmentDetailsDelete)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if (stockAdjustmentDetails.Count > 0)
                        {
                            foreach (ItemStockAdjustmentDetailsBO sad in stockAdjustmentDetails)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveItemStockAdjustmentDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockAdjustmentId", DbType.Int32, stockAdjustment.StockAdjustmentId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, sad.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationId", DbType.Int32, sad.LocationId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, sad.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OpeningQuantity", DbType.Decimal, sad.OpeningQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ReceiveQuantity", DbType.Decimal, sad.ReceiveQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ActualUsage", DbType.Decimal, sad.ActualUsage);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@WastageQuantity", DbType.Decimal, sad.WastageQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockQuantity", DbType.Decimal, sad.StockQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ActualQuantity", DbType.Decimal, sad.ActualQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, sad.ColorId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, sad.SizeId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, sad.StyleId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }
                        else if (stockAdjustmentDetails.Count == 0)
                        {
                            status = 1;
                        }

                        if (status > 0 && stockAdjustmentDetailsEdit.Count > 0)
                        {
                            foreach (ItemStockAdjustmentDetailsBO sad in stockAdjustmentDetailsEdit)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateItemStockAdjustmentDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockAdjustmentDetailsId", DbType.Int32, sad.StockAdjustmentDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockAdjustmentId", DbType.Int32, stockAdjustment.StockAdjustmentId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, sad.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationId", DbType.Int32, sad.LocationId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, sad.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OpeningQuantity", DbType.Decimal, sad.OpeningQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ReceiveQuantity", DbType.Decimal, sad.ReceiveQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ActualUsage", DbType.Decimal, sad.ActualUsage);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@WastageQuantity", DbType.Decimal, sad.WastageQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockQuantity", DbType.Decimal, sad.StockQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ActualQuantity", DbType.Decimal, sad.ActualQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, sad.ColorId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, sad.SizeId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, sad.StyleId);
                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && stockAdjustmentDetailsDelete.Count > 0)
                        {
                            foreach (ItemStockAdjustmentDetailsBO sad in stockAdjustmentDetailsDelete)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@TableName", DbType.String, "InvItemStockAdjustmentDetails");
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@TablePKField", DbType.String, "StockAdjustmentDetailsId");
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@TablePKId", DbType.String, sad.StockAdjustmentDetailsId);
                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public bool ApprovedAdjustmentStatusNUpdateItemStock(int stockAdjustmentId, string approvedStatus, int createdBy)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateApprovedAdjustmentStatusNItemStock_SP"))
                        {
                            cmdOutDetails.Parameters.Clear();
                            //dbSmartAspects.AddInParameter(cmdOutDetails, "@CostCenterId", DbType.Int32, costCenterId);
                            //dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationId", DbType.String, locationId);
                            dbSmartAspects.AddInParameter(cmdOutDetails, "@StockAdjustmentId", DbType.Int32, stockAdjustmentId);
                            dbSmartAspects.AddInParameter(cmdOutDetails, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(cmdOutDetails, "@CreatedBy", DbType.Int32, createdBy);
                            dbSmartAspects.AddOutParameter(cmdOutDetails, "@mErr", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public bool ApprovedAdjustmentStatusNUpdateItemStock(int stockAdjustmentId, string approvedStatus)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateAdjustmentStatusNItemStock_SP"))
                        {
                            cmdOutDetails.Parameters.Clear();
                            dbSmartAspects.AddInParameter(cmdOutDetails, "@StockAdjustmentId", DbType.Int32, stockAdjustmentId);
                            dbSmartAspects.AddInParameter(cmdOutDetails, "@ApprovedStatus", DbType.String, approvedStatus);
                            status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public Boolean DeleteItemStockAdjustment(int stockAdjustmentId)
        {
            int status = 0;
            bool retVal = false;


            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "InvItemStockAdjustmentDetails");
                            dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, "StockAdjustmentId");
                            dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, stockAdjustmentId);
                            status = dbSmartAspects.ExecuteNonQuery(command);
                        }

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "InvItemStockAdjustment");
                                dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, "StockAdjustmentId");
                                dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, stockAdjustmentId);
                                status = dbSmartAspects.ExecuteNonQuery(command);
                            }
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public List<ItemStockAdjustmentBO> GetItemAdjustmentInfoSearch(int costCenterId, int locationId, DateTime? fromDate, DateTime? toDate)
        {
            List<ItemStockAdjustmentBO> itemAdjustment = new List<ItemStockAdjustmentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemAdjustmentInfoSearch_SP"))
                {
                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    if (costCenterId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (locationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RequsitionDetails");
                    DataTable Table = ds.Tables["RequsitionDetails"];

                    itemAdjustment = Table.AsEnumerable().Select(r => new ItemStockAdjustmentBO
                    {
                        StockAdjustmentId = r.Field<int>("StockAdjustmentId"),
                        AdjustmentDate = r.Field<DateTime>("AdjustmentDate"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        AdjustmentFrequency = r.Field<string>("AdjustmentFrequency")

                    }).ToList();
                }
            }
            return itemAdjustment;
        }
        public ItemStockAdjustmentBO GetItemAdjustmentInfoById(int stockAdjustmentId)
        {
            ItemStockAdjustmentBO itemAdjustment = new ItemStockAdjustmentBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemAdjustmentInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@StockAdjustmentId", DbType.Int32, stockAdjustmentId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RequsitionDetails");
                    DataTable Table = ds.Tables["RequsitionDetails"];

                    itemAdjustment = Table.AsEnumerable().Select(r => new ItemStockAdjustmentBO
                    {
                        StockAdjustmentId = r.Field<int>("StockAdjustmentId"),
                        AdjustmentDate = r.Field<DateTime>("AdjustmentDate"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus")

                    }).FirstOrDefault();
                }
            }
            return itemAdjustment;
        }
        public List<ItemStockAdjustmentDetailsBO> GetItemAdjustmentDetailsById(int stockAdjustmentId)
        {
            List<ItemStockAdjustmentDetailsBO> itemAdjustment = new List<ItemStockAdjustmentDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemAdjustmentDetailsById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@StockAdjustmentId", DbType.Int32, stockAdjustmentId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RequsitionDetails");
                    DataTable Table = ds.Tables["RequsitionDetails"];

                    itemAdjustment = Table.AsEnumerable().Select(r => new ItemStockAdjustmentDetailsBO
                    {
                        StockAdjustmentDetailsId = r.Field<int>("StockAdjustmentDetailsId"),
                        StockAdjustmentId = r.Field<int>("StockAdjustmentId"),
                        ItemId = r.Field<int>("ItemId"),
                        LocationId = r.Field<Int32>("LocationId"),
                        StockById = r.Field<Int32>("StockById"),
                        OpeningQuantity = r.Field<decimal>("OpeningQuantity"),
                        ReceiveQuantity = r.Field<decimal>("ReceiveQuantity"),
                        ActualUsage = r.Field<decimal>("ActualUsage"),
                        WastageQuantity = r.Field<decimal>("WastageQuantity"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        ActualQuantity = r.Field<decimal>("ActualQuantity"),
                        ItemName = r.Field<string>("ItemName"),
                        CategoryName = r.Field<string>("CategoryName"),
                        LocationName = r.Field<string>("LocationName"),
                        StockByName = r.Field<string>("StockByName")

                    }).ToList();
                }
            }
            return itemAdjustment;
        }
        public List<ItemStockAdjustmentDetailsBO> GetItemAdjustmentDetailsByDateNCostcenterId(int companyId, int projectId, int costCenterId, int locationId, DateTime adjustmentDate)
        {
            List<ItemStockAdjustmentDetailsBO> itemAdjustment = new List<ItemStockAdjustmentDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemAdjustmentDetailsByDateId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    dbSmartAspects.AddInParameter(cmd, "@AdjustmentDate", DbType.DateTime, adjustmentDate);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RequsitionDetails");
                    DataTable Table = ds.Tables["RequsitionDetails"];

                    itemAdjustment = Table.AsEnumerable().Select(r => new ItemStockAdjustmentDetailsBO
                    {
                        StockAdjustmentDetailsId = r.Field<int>("StockAdjustmentDetailsId"),
                        StockAdjustmentId = r.Field<int>("StockAdjustmentId"),
                        ItemId = r.Field<int>("ItemId"),
                        LocationId = r.Field<Int32>("LocationId"),
                        StockById = r.Field<Int32>("StockById"),
                        OpeningQuantity = r.Field<decimal>("OpeningQuantity"),
                        ReceiveQuantity = r.Field<decimal>("ReceiveQuantity"),
                        ActualUsage = r.Field<decimal>("ActualUsage"),
                        WastageQuantity = r.Field<decimal>("WastageQuantity"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        ActualQuantity = r.Field<decimal>("ActualQuantity"),
                        ItemName = r.Field<string>("ItemName"),
                        CategoryName = r.Field<string>("CategoryName"),
                        LocationName = r.Field<string>("LocationName"),
                        StockByName = r.Field<string>("StockByName"),
                        ColorId = r.Field<int>("ColorId"),
                        SizeId = r.Field<int>("SizeId"),
                        StyleId = r.Field<int>("StyleId")

                    }).ToList();
                }
            }
            return itemAdjustment;
        }
        //--------------------------- Inventory Transaction Mode
        public List<InvTransactionModeBO> GetInvTransactionMode()
        {
            List<InvTransactionModeBO> productList = new List<InvTransactionModeBO>();
            InvTransactionModeBO productBO;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInventoryTransactionMode_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                productBO = new InvTransactionModeBO();
                                productBO.TModeId = Convert.ToInt32(reader["TModeId"]);
                                productBO.HeadName = reader["HeadName"].ToString();
                                productBO.CalculationType = reader["CalculationType"].ToString();
                                productBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }
        //------------------Inventory Item Stock Variance ----------------------------------
        public List<ItemWiseStockReportViewBO> GetCostcenterLocationWiseItemNUsage(int locationId, int categoryId, DateTime? fromDate, DateTime? toDate, bool isCustomerItem, bool isSupplierItem)
        {
            List<ItemWiseStockReportViewBO> itemWiseStockList = new List<ItemWiseStockReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCostcenterLocationWiseItemNUsage_SP"))
                {
                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (locationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@IsCustomerItem", DbType.Boolean, isCustomerItem);
                    dbSmartAspects.AddInParameter(cmd, "@IsSupplierItem", DbType.Boolean, isSupplierItem);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];

                    itemWiseStockList = Table.AsEnumerable().Select(r => new ItemWiseStockReportViewBO
                    {
                        ItemId = r.Field<Int32?>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        ItemCode = r.Field<string>("ItemCode"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        StockById = r.Field<Int32>("StockById"),
                        HeadName = r.Field<string>("HeadName"),
                        ActualUsage = r.Field<decimal>("ActualUsage"),
                        UsageCost = r.Field<decimal>("UsageCost"),
                        UnitPrice = r.Field<decimal>("UnitPrice")

                    }).ToList();
                }
            }

            return itemWiseStockList;
        }
        public bool SaveItemStockVariance(InvItemStockVarianceBO ItemStockVariance, List<InvItemStockVarianceDetailsBO> StockVarianceDetails, out int stockVarianceId)
        {
            int status = 0;
            bool retVal = false;
            stockVarianceId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("SaveItemStockVariance_SP"))
                        {
                            commandOut.Parameters.Clear();
                            dbSmartAspects.AddInParameter(commandOut, "@CompanyId", DbType.Int32, ItemStockVariance.CompanyId);
                            dbSmartAspects.AddInParameter(commandOut, "@ProjectId", DbType.Int32, ItemStockVariance.ProjectId);
                            dbSmartAspects.AddInParameter(commandOut, "@StockVarianceDate", DbType.DateTime, ItemStockVariance.StockVarianceDate);
                            dbSmartAspects.AddInParameter(commandOut, "@LocationId", DbType.Int32, StockVarianceDetails[0].LocationId);
                            dbSmartAspects.AddInParameter(commandOut, "@CostCenterId", DbType.Int32, ItemStockVariance.CostCenterId);
                            dbSmartAspects.AddInParameter(commandOut, "@ApprovedStatus", DbType.String, ItemStockVariance.ApprovedStatus);
                            dbSmartAspects.AddInParameter(commandOut, "@CreatedBy", DbType.Int32, ItemStockVariance.CreatedBy);
                            dbSmartAspects.AddOutParameter(commandOut, "@StockVarianceId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);
                            stockVarianceId = Convert.ToInt32(commandOut.Parameters["@StockVarianceId"].Value);
                        }

                        if (status > 0 && StockVarianceDetails.Count > 0)
                        {
                            foreach (InvItemStockVarianceDetailsBO sad in StockVarianceDetails)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveItemStockVarianceDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockVarianceId", DbType.Int32, stockVarianceId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, sad.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationId", DbType.Int32, sad.LocationId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, sad.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@TModeId", DbType.Int32, sad.TModeId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@UsageQuantity", DbType.Decimal, sad.UsageQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@UnitPrice", DbType.Decimal, sad.UnitPrice);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@UsageCost", DbType.Decimal, sad.UsageCost);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@VarianceQuantity", DbType.Decimal, sad.VarianceQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@VarianceCost", DbType.Decimal, (sad.VarianceQuantity * sad.UnitPrice));
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Reason", DbType.String, sad.Reason);
                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public List<InvItemStockVarianceBO> GetItemStockVarianceInfoSearch(int costCenterId, int locationId, DateTime? fromDate, DateTime? toDate)
        {
            List<InvItemStockVarianceBO> itemStockVariance = new List<InvItemStockVarianceBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemStockVarianceSearch_SP"))
                {
                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    if (costCenterId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (locationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RequsitionDetails");
                    DataTable Table = ds.Tables["RequsitionDetails"];

                    itemStockVariance = Table.AsEnumerable().Select(r => new InvItemStockVarianceBO
                    {
                        StockVarianceId = r.Field<int>("StockVarianceId"),
                        StockVarianceDate = r.Field<DateTime>("StockVarianceDate"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus")

                    }).ToList();
                }
            }
            return itemStockVariance;
        }
        public InvItemStockVarianceBO GetItemStockVarianceById(int stockVarianceId)
        {
            InvItemStockVarianceBO itemAdjustment = new InvItemStockVarianceBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemStockVarianceById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@StockVarianceId", DbType.Int32, stockVarianceId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RequsitionDetails");
                    DataTable Table = ds.Tables["RequsitionDetails"];

                    itemAdjustment = Table.AsEnumerable().Select(r => new InvItemStockVarianceBO
                    {
                        StockVarianceId = r.Field<int>("StockVarianceId"),
                        StockVarianceDate = r.Field<DateTime>("StockVarianceDate"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus")

                    }).FirstOrDefault();
                }
            }
            return itemAdjustment;
        }
        public List<InvItemStockVarianceDetailsBO> GetItemVarianceDetailsById(int stockVarianceId)
        {
            List<InvItemStockVarianceDetailsBO> itemVariance = new List<InvItemStockVarianceDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemVarianceDetailsById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@StockVarianceId", DbType.String, stockVarianceId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RequsitionDetails");
                    DataTable Table = ds.Tables["RequsitionDetails"];

                    itemVariance = Table.AsEnumerable().Select(r => new InvItemStockVarianceDetailsBO
                    {
                        StockVarianceDetailsId = r.Field<int>("StockVarianceDetailsId"),
                        StockVarianceId = r.Field<int>("StockVarianceId"),
                        ItemId = r.Field<int>("ItemId"),
                        LocationId = r.Field<Int32>("LocationId"),
                        StockById = r.Field<Int32>("StockById"),
                        TModeId = r.Field<Int32>("TModeId"),

                        UsageQuantity = r.Field<decimal>("UsageQuantity"),
                        UsageCost = r.Field<decimal>("UsageCost"),
                        VarianceQuantity = r.Field<decimal>("VarianceQuantity"),
                        VarianceCost = r.Field<decimal>("VarianceCost"),

                        ItemName = r.Field<string>("ItemName"),
                        LocationName = r.Field<string>("LocationName"),
                        StockByName = r.Field<string>("StockByName"),
                        TransactionMode = r.Field<string>("TransactionMode"),
                        Reason = r.Field<string>("Reason")

                    }).ToList();
                }
            }
            return itemVariance;
        }
        public InvItemStockInformationBO GetInvCompanyProjectWiseItemStockInfo(int companyId, int projectId, int itemId, int locationId)
        {
            InvItemStockInformationBO stockInfo = new InvItemStockInformationBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvCompanyProjectWiseItemStockInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.String, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.String, projectId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.String, itemId);
                    dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.String, locationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemStockInfo");
                    DataTable Table = ds.Tables["ItemStockInfo"];

                    stockInfo = Table.AsEnumerable().Select(r => new InvItemStockInformationBO
                    {
                        StockId = r.Field<int>("StockId"),
                        ItemId = r.Field<int>("ItemId"),
                        LocationId = r.Field<Int32>("LocationId"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        AverageCost = r.Field<decimal>("AverageCost"),
                        HeadName = r.Field<string>("HeadName")

                    }).SingleOrDefault();
                }
            }
            return stockInfo;
        }
        public InvItemStockInformationBO GetInvItemStockInfo(int itemId, int locationId)
        {
            InvItemStockInformationBO stockInfo = new InvItemStockInformationBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemStock_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.String, itemId);
                    dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.String, locationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemStockInfo");
                    DataTable Table = ds.Tables["ItemStockInfo"];

                    stockInfo = Table.AsEnumerable().Select(r => new InvItemStockInformationBO
                    {
                        StockId = r.Field<int>("StockId"),
                        ItemId = r.Field<int>("ItemId"),
                        LocationId = r.Field<Int32>("LocationId"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        HeadName = r.Field<string>("HeadName")

                    }).SingleOrDefault();
                }
            }
            return stockInfo;
        }
        public InvItemStockInformationBO GetInvItemStockInfoByItemLocationNProject(int itemId, int locationId, int projectId)
        {
            InvItemStockInformationBO stockInfo = new InvItemStockInformationBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemStockInfoByItemLocationNProject_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.String, itemId);
                    dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.String, locationId);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.String, projectId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemStockInfo");
                    DataTable Table = ds.Tables["ItemStockInfo"];

                    stockInfo = Table.AsEnumerable().Select(r => new InvItemStockInformationBO
                    {
                        StockId = r.Field<int>("StockId"),
                        ItemId = r.Field<int>("ItemId"),
                        LocationId = r.Field<Int32>("LocationId"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        HeadName = r.Field<string>("HeadName")

                    }).SingleOrDefault();
                }
            }
            return stockInfo;
        }
        public InvItemStockInformationBO GetInvItemStockInfoByItemAndAttributeId(int itemId, int colorId, int sizeId, int styleId, int LocationId)//,int companyId)
        {

            InvItemStockInformationBO StockInformation = new InvItemStockInformationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemStockInfoByItemAndAttributeIdForInventory_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);

                    if (colorId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ColorId", DbType.Int32, colorId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ColorId", DbType.Int32, DBNull.Value);

                    if (sizeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@SizeId", DbType.Int32, sizeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SizeId", DbType.Int32, DBNull.Value);

                    if (styleId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@StyleId", DbType.Int32, styleId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@StyleId", DbType.Int32, DBNull.Value);


                    //if (companyId != 0)
                    //    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    //else
                    //    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "UnitHead");
                    DataTable Table = ds.Tables["UnitHead"];

                    StockInformation = Table.AsEnumerable().Select(r => new InvItemStockInformationBO
                    {
                        StockId = r.Field<int>("StockId"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        ItemId = r.Field<int>("ItemId")
                    }).FirstOrDefault();
                }
            }
            return StockInformation;
        }

        public InvItemStockInformationBO GetInvItemStockInfoByItemAndAttributeIdForPurchase(int itemId, int colorId, int sizeId, int styleId, int LocationId, int companyId)
        {

            InvItemStockInformationBO StockInformation = new InvItemStockInformationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemStockInfoByItemAndAttributeIdForPurchase_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);

                    if (colorId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ColorId", DbType.Int32, colorId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ColorId", DbType.Int32, DBNull.Value);

                    if (sizeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@SizeId", DbType.Int32, sizeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SizeId", DbType.Int32, DBNull.Value);

                    if (styleId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@StyleId", DbType.Int32, styleId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@StyleId", DbType.Int32, DBNull.Value);

                    if (LocationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "UnitHead");
                    DataTable Table = ds.Tables["UnitHead"];

                    StockInformation = Table.AsEnumerable().Select(r => new InvItemStockInformationBO
                    {
                        StockId = r.Field<int>("StockId"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        ItemId = r.Field<int>("ItemId")
                    }).FirstOrDefault();
                }
            }
            return StockInformation;
        }

        public List<ItemStockAdjustmentDetailsBO> GetStockAdjustmentInformation(string searchCriteria)
        {
            List<ItemStockAdjustmentDetailsBO> itemAdjustment = new List<ItemStockAdjustmentDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetStockAdjustmentInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "StockAdjustmentInfo");
                    DataTable Table = ds.Tables["StockAdjustmentInfo"];

                    itemAdjustment = Table.AsEnumerable().Select(r => new ItemStockAdjustmentDetailsBO
                    {
                        StockAdjustmentId = r.Field<int>("StockAdjustmentId"),
                        AdjustmentDate = r.Field<DateTime>("AdjustmentDate"),
                        AdjustmentDateString = r.Field<string>("AdjustmentDateString"),
                        CostCenterId = r.Field<int>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        AdjustmentFrequency = r.Field<string>("AdjustmentFrequency"),
                        LocationId = r.Field<Int32>("LocationId"),
                        Location = r.Field<string>("Location"),
                        ItemId = r.Field<int>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        UnitHead = r.Field<string>("UnitHead"),
                        OpeningQuantity = r.Field<decimal>("OpeningQuantity"),
                        ReceiveQuantity = r.Field<decimal>("ReceiveQuantity"),
                        UsageQuantity = r.Field<decimal>("UsageQuantity"),
                        WastageQuantity = r.Field<decimal>("WastageQuantity"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        ActualQuantity = r.Field<decimal>("ActualQuantity"),
                        StockVariance = r.Field<decimal>("StockVariance")

                    }).ToList();
                }
            }
            return itemAdjustment;
        }
        public List<ItemStockAdjustmentDetailsBO> GetItemWastageInformation(string searchCriteria)
        {
            List<ItemStockAdjustmentDetailsBO> itemAdjustment = new List<ItemStockAdjustmentDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemWastageInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "StockAdjustmentInfo");
                    DataTable Table = ds.Tables["StockAdjustmentInfo"];

                    itemAdjustment = Table.AsEnumerable().Select(r => new ItemStockAdjustmentDetailsBO
                    {
                        StockAdjustmentId = r.Field<int>("StockAdjustmentId"),
                        AdjustmentDate = r.Field<DateTime>("AdjustmentDate"),
                        AdjustmentDateString = r.Field<string>("AdjustmentDateString"),
                        CostCenterId = r.Field<int>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        LocationId = r.Field<Int32>("LocationId"),
                        Location = r.Field<string>("Location"),
                        ItemId = r.Field<int>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        UnitHead = r.Field<string>("UnitHead"),
                        UnitPrice = r.Field<decimal>("UnitPrice"),
                        StockVariance = r.Field<decimal>("StockVariance"),
                        VarianceCost = r.Field<decimal>("VarianceCost"),
                        WastageType = r.Field<string>("WastageType"),
                        Reason = r.Field<string>("Reason")
                    }).ToList();
                }
            }
            return itemAdjustment;
        }
        public List<ItemTransactionSummaryReportBO> GetItemTransactionSummaryReport(DateTime dateFrom, DateTime dateTo, int categoryId, int itemId, int locationId)
        {
            List<ItemTransactionSummaryReportBO> itemAdjustment = new List<ItemTransactionSummaryReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("ItemTransactionSummaryReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.Date, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.Date, dateTo);

                    if (categoryId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (itemId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);

                    if (locationId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemTransaction");
                    DataTable Table = ds.Tables["ItemTransaction"];

                    itemAdjustment = Table.AsEnumerable().Select(r => new ItemTransactionSummaryReportBO
                    {
                        ItemTransactionId = r.Field<long>("ItemTransactionId"),
                        TransactionDate = r.Field<DateTime>("TransactionDate"),
                        LocationId = r.Field<int?>("LocationId"),
                        ItemId = r.Field<int>("ItemId"),
                        AverageCost = r.Field<decimal?>("AverageCost"),
                        DayOpeningQuantity = r.Field<decimal>("DayOpeningQuantity"),
                        TransactionalOpeningQuantity = r.Field<decimal>("TransactionalOpeningQuantity"),
                        ReceiveQuantity = r.Field<decimal?>("ReceiveQuantity"),
                        OutItemQuantity = r.Field<decimal?>("OutItemQuantity"),
                        WastageQuantity = r.Field<decimal?>("WastageQuantity"),
                        AdjustmentQuantity = r.Field<decimal?>("AdjustmentQuantity"),
                        SalesQuantity = r.Field<decimal?>("SalesQuantity"),
                        ClosingQuantity = r.Field<decimal>("ClosingQuantity"),
                        ItemName = r.Field<string>("ItemName"),
                        HeadName = r.Field<string>("HeadName"),
                        LocationName = r.Field<string>("LocationName")

                    }).ToList();
                }
            }
            return itemAdjustment;
        }

        public List<ItemTransactionDetailsReportBO> GetItemTransactionDetailsReport(DateTime dateFrom, DateTime dateTo, int categoryId, int itemId, int locationId)
        {
            List<ItemTransactionDetailsReportBO> itemAdjustment = new List<ItemTransactionDetailsReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("ItemTransactionDetailsReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.Date, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.Date, dateTo);

                    if (categoryId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (itemId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);

                    if (locationId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemTransaction");
                    DataTable Table = ds.Tables["ItemTransaction"];

                    itemAdjustment = Table.AsEnumerable().Select(r => new ItemTransactionDetailsReportBO
                    {
                        ItemTransactionId = r.Field<long>("ItemTransactionId"),
                        TransactionDate = r.Field<DateTime>("TransactionDate"),
                        TransactionType = r.Field<string>("TransactionType"),
                        LocationId = r.Field<int?>("LocationId"),
                        ItemId = r.Field<int>("ItemId"),
                        AverageCost = r.Field<decimal?>("AverageCost"),
                        DayOpeningQuantity = r.Field<decimal>("DayOpeningQuantity"),
                        TransactionalOpeningQuantity = r.Field<decimal>("TransactionalOpeningQuantity"),
                        TransactionQuantity = r.Field<decimal>("TransactionQuantity"),
                        ClosingQuantity = r.Field<decimal>("ClosingQuantity"),
                        ItemName = r.Field<string>("ItemName"),
                        HeadName = r.Field<string>("HeadName"),
                        LocationName = r.Field<string>("LocationName"),
                        //SerialNumber = r.Field<string>("SerialNumber")

                    }).ToList();
                }
            }
            return itemAdjustment;
        }

        //-------------------------------Inventory Service And Price Matrix --------------------------------------------
        public List<ServicePriceMatrixBO> GetServicePriceMatrix()
        {
            List<ServicePriceMatrixBO> serviceList = new List<ServicePriceMatrixBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetServicePriceMatrix_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ServicePriceMatrixBO sv = new ServicePriceMatrixBO();
                                sv.ServicePriceMatrixId = Convert.ToInt32(reader["ServicePriceMatrixId"]);
                                sv.ItemId = Convert.ToInt32(reader["ItemId"]);
                                sv.ServicePackageId = Convert.ToInt32(reader["ServicePackageId"]);
                                sv.ServiceBandWidthId = Convert.ToInt32(reader["ServiceBandWidthId"]);
                                sv.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                sv.IsActive = Convert.ToBoolean(reader["IsActive"]);
                                sv.ActiveStatus = reader["ActiveStatus"].ToString();
                                serviceList.Add(sv);
                            }
                        }
                    }
                }
            }
            return serviceList;
        }
        public ServicePriceMatrixBO GetServicePriceMatrix(int itemId, int servicePackageId, int serviceBandWidthId)
        {
            ServicePriceMatrixBO sv = new ServicePriceMatrixBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetServicePriceMatrix_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    dbSmartAspects.AddInParameter(cmd, "@ServicePackageId", DbType.Int32, servicePackageId);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceBandWidthId", DbType.Int32, serviceBandWidthId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                sv.ServicePriceMatrixId = Convert.ToInt32(reader["ServicePriceMatrixId"]);
                                sv.ItemId = Convert.ToInt32(reader["ItemId"]);
                                sv.ServicePackageId = Convert.ToInt32(reader["ServicePackageId"]);
                                sv.ServiceBandWidthId = Convert.ToInt32(reader["ServiceBandWidthId"]);
                                sv.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                sv.IsActive = Convert.ToBoolean(reader["IsActive"]);
                                sv.ActiveStatus = reader["ActiveStatus"].ToString();
                                sv.Description = reader["Description"].ToString();
                            }
                        }
                    }
                }
            }
            return sv;
        }
        public List<InvServicePackage> GetServicePackage()
        {
            List<InvServicePackage> serviceList = new List<InvServicePackage>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetServicePackage_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvServicePackage sv = new InvServicePackage();
                                sv.ServicePackageId = Convert.ToInt32(reader["ServicePackageId"]);
                                sv.PackageName = Convert.ToString(reader["PackageName"]);
                                sv.ServicePackageId = Convert.ToInt32(reader["ServicePackageId"]);
                                sv.IsActive = Convert.ToBoolean(reader["IsActive"]);
                                serviceList.Add(sv);
                            }
                        }
                    }
                }
            }
            return serviceList;
        }
        public List<ServiceBandwidthBO> GetServiceBandwidth()
        {
            List<ServiceBandwidthBO> serviceList = new List<ServiceBandwidthBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetServiceBandwidth_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ServiceBandwidthBO sv = new ServiceBandwidthBO();
                                sv.ServiceBandWidthId = Convert.ToInt32(reader["ServiceBandWidthId"]);
                                sv.BandWidthName = Convert.ToString(reader["BandWidthName"]);
                                sv.Uplink = Convert.ToInt32(reader["Uplink"]);
                                if (reader["Downlink"] != DBNull.Value)
                                    sv.Downlink = Convert.ToInt32(reader["Downlink"]);
                                sv.UplinkFrequency = Convert.ToString(reader["UplinkFrequency"]);
                                if (reader["DownlinkFrequency"] != DBNull.Value)
                                    sv.DownlinkFrequency = Convert.ToString(reader["DownlinkFrequency"]);
                                sv.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                sv.ActiveStatus = reader["ActiveStatus"].ToString();
                                serviceList.Add(sv);
                            }
                        }
                    }
                }
            }
            return serviceList;
        }
        public List<InvItemAutoSearchBO> GetItemSearchQuatation(string itemName, string itemType = "", int categoryId = 0, bool? isCustomerItem = false)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemSearchQuatation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);

                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (isCustomerItem != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, isCustomerItem);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, DBNull.Value);

                    if (!string.IsNullOrEmpty(itemType))
                        dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, itemType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name") + " (" + r.Field<string>("CategoryName") + ")",
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        IsRecipe = r.Field<bool>("IsRecipe"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        UnitPriceUsd = r.Field<decimal>("UnitPriceUsd"),
                        UnitHead = r.Field<string>("HeadName"),
                        AverageCost = r.Field<decimal>("AverageCost")

                    }).ToList();
                }
            }

            return itemInfo;
        }


        //----Cost Center Wise Item and Category
        public List<InvCategoryBO> GetCategoryByCostcenter(int costCenterId)
        {
            string query = string.Format(@"SELECT DISTINCT icm.CategoryId, cat.Name
                                            FROM InvItemCostCenterMapping icm INNER JOIN InvCategory cat ON icm.CategoryId = cat.CategoryId
                                            WHERE icm.CostCenterId = {0}
                                         ", costCenterId);

            List<InvCategoryBO> category = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryBO sv = new InvCategoryBO();

                                sv.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                sv.Name = Convert.ToString(reader["Name"]);

                                category.Add(sv);
                            }
                        }
                    }
                }
            }
            return category;
        }

        public List<InvItemBO> GetItemByCostcenter(int costCenterId)
        {
            string query = string.Format(@"SELECT DISTINCT icm.ItemId, itm.Name 
                                            FROM InvItemCostCenterMapping icm INNER JOIN InvItem itm ON icm.ItemId = itm.ItemId
                                            WHERE icm.CostCenterId = {0}
                                         ", costCenterId);

            List<InvItemBO> item = new List<InvItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO sv = new InvItemBO();

                                sv.ItemId = Convert.ToInt32(reader["ItemId"]);
                                sv.Name = Convert.ToString(reader["Name"]);

                                item.Add(sv);
                            }
                        }
                    }
                }
            }
            return item;
        }

        public List<InvItemBO> GetItemByCategoryNCostcenter(int costCenterId, int categoryId)
        {
            List<InvItemBO> productList = new List<InvItemBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemByCategoryNCostcenter_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (costCenterId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO productBO = new InvItemBO();
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.CategoryName = reader["CategoryName"].ToString();
                                productBO.Name = reader["Name"].ToString();
                                productBO.SizeName = reader["SizeName"].ToString();
                                productBO.ColorName = reader["ColorName"].ToString();
                                productBO.StyleName = reader["StyleName"].ToString();
                                productBO.UnitHead = reader["UnitHead"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.ItemNameAndCode = reader["ItemNameAndCode"].ToString();
                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                                productBO.ImageName = reader["ImageName"].ToString();
                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }

        public InvItemBO GetItemByItemAndCostCenterForBarCode(int itemId)
        {
            InvItemBO productBO = new InvItemBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemByItemAndCostCenterForBarCode_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                            }
                        }
                    }
                }
            }
            return productBO;
        }

        public List<InvItemBO> GetInvItemInfoByCostCenterIdNCatagoryIdForPagination(int costCenterId, int categoryId, int recordPerPage, int pageIndex, out int totalRecords)
        {

            List<InvItemBO> productList = new List<InvItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemInfoByCostCenterIdNCatagoryIdForPagination_SP"))
                {
                    if (categoryId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    }

                    if (costCenterId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO productBO = new InvItemBO();
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.Description = reader["Description"].ToString();
                                productBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.ProductType = reader["ProductType"].ToString();
                                productBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                productBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                productBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                productBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                productBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                productBO.StockType = reader["UnitPriceUsd"].ToString();
                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                                productList.Add(productBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return productList;
        }

        //
        public List<InvItemBO> GetAllFinishedGoodItems()
        {
            List<InvItemBO> itemList = new List<InvItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllFinishedGoodItems_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO it = new InvItemBO();
                                it.ItemId = Convert.ToInt32(reader["ItemId"]);
                                it.Name = Convert.ToString(reader["Name"]);
                                it.AverageCost = Convert.ToDecimal(reader["AverageCost"]);
                                itemList.Add(it);
                            }
                        }
                    }
                }
            }
            return itemList;
        }

        public List<InvItemAutoSearchBO> GetIChangeableRecipeItemByItemID(int itemId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetChangeableRecipeItemByItemID_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@itemId", DbType.Int32, itemId);


                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("RecipeItemId"),
                        Name = r.Field<string>("RecipeItemName")


                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<RestaurantRecipeDetailBO> LoadRecipeByKotIdAndItemId(int userInfoId, int ItemId, int kotid, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<RestaurantRecipeDetailBO> InvItemBOList = new List<RestaurantRecipeDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRecipeByKotIdAndItemIdForGridPaging_SP"))
                {
                    if (userInfoId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, userInfoId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, DBNull.Value);


                    if (ItemId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, ItemId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);

                    if (kotid != 0)
                        dbSmartAspects.AddInParameter(cmd, "@kotid", DbType.Int32, kotid);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@kotid", DbType.Int32, DBNull.Value);


                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet CashDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, CashDS, "Cash");
                    DataTable Table = CashDS.Tables["Cash"];

                    InvItemBOList = Table.AsEnumerable().Select(r => new RestaurantRecipeDetailBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        RecipeItemId = r.Field<int>("RecipeItemId"),
                        RecipeItemName = r.Field<string>("RecipeItemName"),
                        ItemUnit = r.Field<decimal>("ItemUnit"),
                        ItemCost = r.Field<decimal>("ItemCost"),
                        HeadName = r.Field<string>("HeadName")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return InvItemBOList;
        }

        public List<RestaurantRecipeDetailBO> GetExistingRecipeForRecipe(int itemId)
        {
            List<RestaurantRecipeDetailBO> itemInfo = new List<RestaurantRecipeDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetChangeableRecipeItemByItemID_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@itemId", DbType.Int32, itemId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantRecipeDetailBO it = new RestaurantRecipeDetailBO();
                                it.ItemId = Convert.ToInt32(reader["RecipeItemId"]);
                                it.Name = Convert.ToString(reader["RecipeItemName"]);
                                it.ItemCost = Convert.ToDecimal(reader["ItemCost"]);
                                it.RecipeId = 0;
                                it.PreviousTypeId = 0;
                                it.PreviousTotalCost = it.ItemCost;

                                it.RecipeModifierTypes = new List<RestaurantRecipeDetailBO>();
                                RestaurantRecipeDetailBO rdb = new RestaurantRecipeDetailBO();
                                rdb.TypeId = 0;
                                rdb.HeadName = "Default";
                                rdb.TotalCost = it.ItemCost;
                                it.RecipeModifierTypes.Add(rdb);
                                List<RestaurantRecipeDetailBO> ModifierTypesInfo = new List<RestaurantRecipeDetailBO>();


                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetPreviousRecipeModifierTypes_SP"))
                                {

                                    dbSmartAspects.AddInParameter(command, "@IngredientId", DbType.Int32, it.ItemId);
                                    dbSmartAspects.AddInParameter(command, "@itemId", DbType.Int32, itemId);


                                    DataSet dsTemp = new DataSet();
                                    dbSmartAspects.LoadDataSet(command, dsTemp, "ItemInfo");
                                    DataTable TableNew = dsTemp.Tables["ItemInfo"];


                                    ModifierTypesInfo = TableNew.AsEnumerable().Select(r => new RestaurantRecipeDetailBO
                                    {
                                        TypeId = r.Field<int>("Id"),
                                        HeadName = r.Field<string>("UnitHead"),
                                        ItemUnit = r.Field<Decimal>("UnitQuantity"),
                                        AditionalCost = r.Field<Decimal>("AdditionalCost"),
                                        TotalCost = r.Field<Decimal>("TotalCost")


                                    }).ToList();

                                }
                                it.RecipeModifierTypes.AddRange(ModifierTypesInfo);


                                // itemInfo;



                                itemInfo.Add(it);
                            }
                        }
                    }

                }
            }

            return itemInfo;
        }

        public bool SaveNewRecipe(List<RestaurantRecipeDetailBO> newRecipeeItems, List<int> deletedItemIdList, int kotId)
        {
            bool status = false;

            int statusInt = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                try
                {
                    if (newRecipeeItems.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveNewRecipeItems_SP"))
                        {
                            foreach (RestaurantRecipeDetailBO detailBO in newRecipeeItems)
                            {

                                cmd.Parameters.Clear();
                                dbSmartAspects.AddInParameter(cmd, "@RecipeId", DbType.Int32, detailBO.RecipeId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, detailBO.ItemId);
                                dbSmartAspects.AddInParameter(cmd, "@RecipeItemId", DbType.Int32, detailBO.RecipeItemId);
                                dbSmartAspects.AddInParameter(cmd, "@RecipeItemName", DbType.String, detailBO.RecipeItemName);
                                dbSmartAspects.AddInParameter(cmd, "@ItemCost", DbType.Decimal, detailBO.ItemCost);
                                dbSmartAspects.AddInParameter(cmd, "@HeadName", DbType.String, detailBO.HeadName);
                                dbSmartAspects.AddInParameter(cmd, "@TypeId", DbType.Int32, detailBO.TypeId);
                                dbSmartAspects.AddInParameter(cmd, "@kotId", DbType.Int32, kotId);
                                dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, detailBO.Status);

                                statusInt = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? 1 : 0;

                            }
                        }

                    }
                    if (deletedItemIdList.Count > 0)
                    {

                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteNewRecipeInKOT_SP"))
                        {
                            foreach (int deletedId in deletedItemIdList)
                            {
                                cmd.Parameters.Clear();
                                dbSmartAspects.AddInParameter(cmd, "@RecipeId", DbType.Int32, deletedId);
                                statusInt = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? 1 : 0;
                            }

                        }

                    }



                    if (statusInt > 0)
                        status = true;
                    else
                        status = false;

                }
                catch (Exception e)
                {
                    status = false;

                }

            }

            return status;
        }

        public List<RestaurantRecipeDetailBO> GetNewItemsForRecipe(int itemId)
        {
            List<RestaurantRecipeDetailBO> itemInfo = new List<RestaurantRecipeDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNewItemsForRecipe_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@itemId", DbType.Int32, itemId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantRecipeDetailBO it = new RestaurantRecipeDetailBO();
                                it.ItemId = Convert.ToInt32(reader["RecipeItemId"]);
                                it.Name = Convert.ToString(reader["RecipeItemName"]);
                                itemInfo.Add(it);
                            }
                        }
                    }

                }
            }



            return itemInfo;
        }

        public int GetRecipeType(int itemId)
        {
            int isRecipe = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRecipeType"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@itemId", DbType.Int32, itemId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                isRecipe = Convert.ToInt32(reader["IsRecipe"]);
                            }
                        }
                    }

                }
            }



            return isRecipe;
        }

        public bool UpdateDynamicKotRecipeCost(int itemId, int kotId)
        {
            bool status;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateDynamicKotRecipeCost_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);

                    status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;

                }
            }



            return status;
        }

        public List<RestaurantRecipeDetailBO> GetNewModifiedRecipeItems(int itemId, int kotId)
        {
            List<RestaurantRecipeDetailBO> itemInfo = new List<RestaurantRecipeDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNewModifiedRecipeItems_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@itemId", DbType.Int32, itemId);
                    dbSmartAspects.AddInParameter(cmd, "@kotId", DbType.Int32, kotId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantRecipeDetailBO it = new RestaurantRecipeDetailBO();
                                it.RecipeId = Convert.ToInt32(reader["RecipeId"]);
                                it.ItemId = Convert.ToInt32(reader["ItemId"]);
                                it.RecipeItemId = Convert.ToInt32(reader["RecipeItemId"]);
                                it.RecipeItemName = Convert.ToString(reader["RecipeItemName"]);
                                it.UnitHeadId = Convert.ToInt32(reader["UnitHeadId"]);
                                it.ItemUnit = Convert.ToDecimal(reader["ItemUnit"]);
                                it.ItemCost = Convert.ToDecimal(reader["ItemCost"]);
                                it.HeadName = Convert.ToString(reader["HeadName"]);
                                it.TypeId = Convert.ToInt32(reader["TypeId"]);
                                it.Status = Convert.ToString(reader["Status"]);


                                itemInfo.Add(it);
                            }
                        }
                    }

                }
            }



            return itemInfo;
        }


        public List<RestaurantRecipeDetailBO> GetChangeableRecipeItemDetails(int IngredientId, int itemid)
        {
            List<RestaurantRecipeDetailBO> itemInfo = new List<RestaurantRecipeDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetChangeableRecipeItemDetails_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@IngredientId", DbType.Int32, IngredientId);
                    dbSmartAspects.AddInParameter(cmd, "@itemId", DbType.Int32, itemid);


                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new RestaurantRecipeDetailBO
                    {
                        RecipeId = r.Field<int>("RecipeId"),
                        UnitHeadId = r.Field<int>("UnitHeadId"),
                        ItemUnit = r.Field<Decimal>("ItemUnit"),
                        ItemCost = r.Field<Decimal>("ItemCost"),
                        AverageCost = r.Field<Decimal>("AverageCost"),
                        AverageUnitHeadId = r.Field<int>("AverageUnitHeadId"),
                        AverageUnitHead = r.Field<string>("AverageUnitHead")


                    }).ToList();
                }
            }

            return itemInfo;
        }

        public List<RestaurantRecipeDetailBO> GetPreviousRecipeModifierTypes(int IngredientId, int itemid)
        {
            List<RestaurantRecipeDetailBO> itemInfo = new List<RestaurantRecipeDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPreviousRecipeModifierTypes_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@IngredientId", DbType.Int32, IngredientId);
                    dbSmartAspects.AddInParameter(cmd, "@itemId", DbType.Int32, itemid);


                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new RestaurantRecipeDetailBO
                    {
                        Id = r.Field<int>("Id"),
                        HeadName = r.Field<string>("UnitHead"),
                        ItemUnit = r.Field<Decimal>("UnitQuantity"),
                        AditionalCost = r.Field<Decimal>("AdditionalCost"),
                        TotalCost = r.Field<Decimal>("TotalCost")


                    }).ToList();
                }
            }

            return itemInfo;
        }


        public List<InvItemAutoSearchBO> GetInvItemByCostCenterNCategoryIdForAutoSearch(string itemName, int costCenterId, int categoryId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemByCostCenterNCategoryIdForAutoSearch_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(itemName))
                        dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, DBNull.Value);

                    if (categoryId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (costCenterId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);


                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name")

                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<InvItemAutoSearchBO> GetInvItemByCostCenterIdForAutoSearch(string itemName, int costCenterId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemByCostCenterForAutoSearch_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(itemName))
                        dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, DBNull.Value);

                    if (costCenterId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);


                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        ItemNameAndCode = r.Field<string>("ItemNameAndCode")

                    }).ToList();
                }
            }

            return itemInfo;
        }

        public List<InvItemAutoSearchBO> GetAvailableFixedAssetItemForAutoSearch(string itemName, int companyId, int projectId, int costCenterId, int categoryId, int locationId = 0)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAvailableFixedAssetItemForAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName.Trim());
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);

                    if (categoryId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    if (locationId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        ItemName = r.Field<string>("ItemName"),
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        ImageName = r.Field<string>("ImageName"),
                        ProductType = r.Field<string>("ProductType"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitHead = r.Field<string>("HeadName"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice"),
                        LastPurchaseDate = r.Field<DateTime?>("LastPurchaseDate")

                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<InvItemAutoSearchBO> GetAvailableFixedAssetItemForReturnAutoSearch(string transferFor, int empId, string itemName, int costCenterId, int categoryId, int locationId = 0)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAvailableFixedAssetItemForReturnAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TransferFor", DbType.String, transferFor.Trim());
                    if (!string.IsNullOrEmpty(itemName.Trim()))
                        dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName.Trim());
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, DBNull.Value);

                    if (categoryId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    if (empId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@OutFor", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@OutFor", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    if (locationId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        ProductType = r.Field<string>("ProductType"),
                        UnitHead = r.Field<string>("HeadName"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice"),

                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<InvItemViewForBarcodeBO> GetItemForBarcodeInFixedAsset(int itemid, int locationId, int categoryId)
        {
            List<InvItemViewForBarcodeBO> itemInfo = new List<InvItemViewForBarcodeBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemForBarcodeInFixedAsset_SP"))
                {
                    if (itemid > 0)
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemid);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);

                    if (categoryId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (locationId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemViewForBarcodeBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        CostCenter = r.Field<string>("CostCenter"),
                        ItemCode = r.Field<string>("ItemCode"),
                        Location = r.Field<string>("Location")

                    }).ToList();
                }
            }
            return itemInfo;
        }
        public List<InvItemAutoSearchBO> GetItemByCategoryAutoSearch(string itemName, int categoryId, bool? isCustomerItem)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemByCategoryAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemName);

                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (isCustomerItem != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, isCustomerItem);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsCutomerItem", DbType.Boolean, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name") + " (" + r.Field<string>("CategoryName") + ")",
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        IsRecipe = r.Field<bool>("IsRecipe"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        UnitPriceUsd = r.Field<decimal>("UnitPriceUsd"),
                        UnitHead = r.Field<string>("HeadName"),
                        AverageCost = r.Field<decimal>("AverageCost"),
                        Model = r.Field<string>("Model"),
                        ManufacturerName = r.Field<string>("ManufacturerName"),
                        CategoryName = r.Field<string>("CategoryName")

                    }).ToList();
                }
            }

            return itemInfo;
        }

        public InvItemAttributeViewBO GetAllInvItemAttributeAndSetupType()
        {
            InvItemAttributeViewBO InvItemAttributeView = new InvItemAttributeViewBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetAllItemAttributeAndSetupTypeGroup_SP"))
                {

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(command, ds, "SalesCall");

                    InvItemAttributeView.InvItemAttributeBOList = ds.Tables[0].AsEnumerable().Select(r => new InvItemAttributeBO()
                    {
                        Id = r.Field<long>("Id"),
                        Name = r.Field<string>("Name"),
                        Description = r.Field<string>("Description"),
                        SetupType = r.Field<string>("SetupType")

                    }).ToList();
                    InvItemAttributeView.InvItemAttributeSetupTypeList = InvItemAttributeView.InvItemAttributeBOList.GroupBy(u => u.SetupType).Select(grp => grp.Key).ToList();
                }
            }
            return InvItemAttributeView;

        }
        public InvItemAttributeViewBO GetAllInvItemAttributeBySetupType(int setupTypeId)
        {
            InvItemAttributeViewBO InvItemAttributeView = new InvItemAttributeViewBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetAllInvItemAttributeBySetupType_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@SetupTypeId", DbType.Int32, setupTypeId);
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(command, ds, "SalesCall");

                    InvItemAttributeView.InvItemAttributeBOList = ds.Tables[0].AsEnumerable().Select(r => new InvItemAttributeBO()
                    {
                        Id = r.Field<long>("Id"),
                        Name = r.Field<string>("Name"),
                        Description = r.Field<string>("Description"),
                        SetupType = r.Field<string>("SetupType")

                    }).ToList();
                    InvItemAttributeView.InvItemAttributeSetupTypeList = InvItemAttributeView.InvItemAttributeBOList.GroupBy(u => u.SetupType).Select(grp => grp.Key).ToList();
                }
            }
            return InvItemAttributeView;

        }
        public List<InvItemAttributeBO> GetInvItemAttributeInfo()
        {
            List<InvItemAttributeBO> productList = new List<InvItemAttributeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllItemAttributeAndSetupTypeGroup_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemAttributeBO productBO = new InvItemAttributeBO();
                                productBO.Id = Convert.ToInt32(reader["Id"]);
                                productBO.Name = reader["Name"].ToString();
                                productBO.Description = reader["Description"].ToString();
                                productBO.SetupType = reader["SetupType"].ToString();
                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }

        public List<InvItemAttributeBO> GetItemAttributeByItemId(int itemId)
        {
            List<InvItemAttributeBO> itemAdjustment = new List<InvItemAttributeBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemAttributeByItemId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RequsitionDetails");
                    DataTable Table = ds.Tables["RequsitionDetails"];

                    itemAdjustment = Table.AsEnumerable().Select(r => new InvItemAttributeBO
                    {
                        Id = r.Field<int>("AttributeId"),
                        Name = r.Field<string>("Name"),
                        SetupType = r.Field<string>("SetupType")
                    }).ToList();
                }
            }
            return itemAdjustment;
        }
        public List<InvItemAutoSearchBO> GetItemInformationForAutoSearch(int companyId, int itemId, int categoryId, int costcenterId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemInformationForAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costcenterId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        ItemNameAndCode = r.Field<string>("ItemNameAndCode"),
                        CategoryId = r.Field<int>("CategoryId"),
                        ImageName = r.Field<string>("ImageName"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitHead = r.Field<string>("UnitHead"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        UnitPrice = r.Field<decimal>("UnitPrice"),
                        DiscountPercent = r.Field<decimal>("DiscountPercent")
                    }).ToList();
                }
            }

            return itemInfo;
        }
        public List<InvItemBO> GetInvFinishedItemInformation()
        {
            List<InvItemBO> productList = new List<InvItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvFinishedItemInformation_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemBO productBO = new InvItemBO();
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.Name = reader["Name"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.CodeAndName = reader["Code"].ToString() + " - " + reader["Name"].ToString();
                                productBO.Description = reader["Description"].ToString();
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                productBO.ProductType = reader["ProductType"].ToString();
                                productBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                productBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                productBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                productBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                productBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                productBO.ServiceWarranty = Int32.Parse(reader["ServiceWarranty"].ToString());
                                productBO.StockType = reader["StockType"].ToString();
                                productBO.ServiceType = reader["ServiceType"].ToString();
                                productBO.StockBy = Int32.Parse(reader["StockBy"].ToString());
                                productBO.IsSupplierItem = Convert.ToBoolean(reader["IsSupplierItem"].ToString());
                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }

        public bool SaveIngredientNNutrientInfo(List<RestaurantRecipeDetailBO> AddedIngredientInfo, List<RestaurantRecipeDetailBO> DeletedItemList,
                                                            InvNutrientInfoBO NutrientRequiredMasterInfo, List<InvNutrientInfoBO> AddedNutrientRequiredValueInfo,
                                                            List<InvNutrientInfoBO> DeletedNutrientRequiredValueList, List<OverheadExpensesBO> AddedOverheadExpenses,
                                                            List<OverheadExpensesBO> DeletedAccountHead, int userInfoId)
        {
            Int64 status = 0, Id = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if(DeletedItemList.Count > 0)
                        {
                            using(DbCommand cmdDelItem = dbSmartAspects.GetStoredProcCommand("DeleteInvRMItem_SP"))
                            {
                                foreach(RestaurantRecipeDetailBO rd in DeletedItemList)
                                {
                                    cmdDelItem.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdDelItem, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdDelItem, "@RecipeItemId", DbType.Int32, rd.RecipeItemId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdDelItem, transction);
                                }
                            }
                        }
                        if (AddedIngredientInfo.Count > 0)
                        {
                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantRecipeDetailInfo_SP"))
                            {
                                foreach (RestaurantRecipeDetailBO detailBO in AddedIngredientInfo)
                                {
                                    cmd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, detailBO.RecipeId);
                                    dbSmartAspects.AddInParameter(cmd, "@RecipeItemId", DbType.Int32, detailBO.RecipeItemId);
                                    dbSmartAspects.AddInParameter(cmd, "@RecipeItemName", DbType.String, detailBO.RecipeItemName);
                                    dbSmartAspects.AddInParameter(cmd, "@UnitHeadId", DbType.Int32, detailBO.UnitHeadId);
                                    dbSmartAspects.AddInParameter(cmd, "@ItemUnit", DbType.Decimal, detailBO.ItemUnit);
                                    dbSmartAspects.AddInParameter(cmd, "@ItemCost", DbType.Decimal, detailBO.ItemCost);
                                    dbSmartAspects.AddInParameter(cmd, "@IsGradientCanChange", DbType.Boolean, detailBO.IsGradientCanChange);

                                    status = dbSmartAspects.ExecuteNonQuery(cmd, transction);
                                }
                            }
                        }
                        if (DeletedNutrientRequiredValueList.Count > 0)
                        {
                            using (DbCommand cmdDelNRV = dbSmartAspects.GetStoredProcCommand("DeleteInvRMRequiredValueItem_SP"))
                            {
                                foreach (InvNutrientInfoBO ni in DeletedNutrientRequiredValueList)
                                {
                                    cmdDelNRV.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdDelNRV, "@ItemId", DbType.Int32, ni.ItemId);
                                    dbSmartAspects.AddInParameter(cmdDelNRV, "@NutrientId", DbType.Int32, ni.NutrientId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdDelNRV, transction);
                                }
                            }
                        }
                        using (DbCommand cmdOut = dbSmartAspects.GetStoredProcCommand("SaveNRVMasterInfo_SP"))
                        {
                            cmdOut.Parameters.Clear();
                            dbSmartAspects.AddInParameter(cmdOut, "@ItemId", DbType.Int32, NutrientRequiredMasterInfo.ItemId);
                            dbSmartAspects.AddInParameter(cmdOut, "@ItemName", DbType.String, NutrientRequiredMasterInfo.ItemName);
                            dbSmartAspects.AddInParameter(cmdOut, "@CreatedBy", DbType.Int64, userInfoId);

                            dbSmartAspects.AddOutParameter(cmdOut, "@Id", DbType.Int64, sizeof(Int64));
                            status = dbSmartAspects.ExecuteNonQuery(cmdOut, transction);

                            Id = Convert.ToInt64(cmdOut.Parameters["@Id"].Value);
                        }
                        if (AddedNutrientRequiredValueInfo.Count > 0)
                        {
                            foreach (InvNutrientInfoBO ni in AddedNutrientRequiredValueInfo)
                            {
                                using (DbCommand cmdSave = dbSmartAspects.GetStoredProcCommand("SaveNutrientRequiredValues_SP"))
                                {
                                    cmdSave.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdSave, "@Id", DbType.Int64, Id);
                                    dbSmartAspects.AddInParameter(cmdSave, "@NutrientId", DbType.Int32, ni.NutrientId);
                                    dbSmartAspects.AddInParameter(cmdSave, "@NutrientName", DbType.String, ni.NutrientName);
                                    dbSmartAspects.AddInParameter(cmdSave, "@RequiredValue", DbType.Decimal, ni.RequiredValue);
                                    dbSmartAspects.AddInParameter(cmdSave, "@CreatedBy", DbType.Int32, userInfoId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdSave, transction);
                                }
                            }
                        }
                        if (DeletedAccountHead.Count > 0)
                        {
                            using (DbCommand cmdDelOE = dbSmartAspects.GetStoredProcCommand("DeleteInvRMOverheadExpenseItem_SP"))
                            {
                                foreach (OverheadExpensesBO oe in DeletedAccountHead)
                                {
                                    cmdDelOE.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdDelOE, "@FinishProductId", DbType.Int32, oe.FinishProductId);
                                    dbSmartAspects.AddInParameter(cmdDelOE, "@NodeId", DbType.Int32, oe.NodeId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdDelOE, transction);
                                }
                            }
                        }
                        if (AddedOverheadExpenses.Count > 0)
                        {
                            foreach (OverheadExpensesBO oe in AddedOverheadExpenses)
                            {
                                using (DbCommand oEDetails = dbSmartAspects.GetStoredProcCommand("SaveInvIngredientNNutrientOEDetails_SP"))
                                {
                                    oEDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(oEDetails, "@FGId", DbType.Int64, oe.FinishProductId);
                                    dbSmartAspects.AddInParameter(oEDetails, "@NodeId", DbType.Int32, oe.NodeId);
                                    dbSmartAspects.AddInParameter(oEDetails, "@Amount", DbType.Decimal, oe.Amount);
                                    dbSmartAspects.AddInParameter(oEDetails, "@Remarks", DbType.String, oe.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(oEDetails, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch
                    {
                        retVal = false;
                        transction.Rollback();
                    }
                }
            }

            return retVal;
        }
    }
}