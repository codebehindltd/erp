using HotelManagement.Entity.Restaurant;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.Restaurant
{
    public class ItemSalesPopularityInfoDA : BaseService
    {
        public List<ItemSalesPopularityInfoBO> GetInventoryItemInformationWitoutCostCenter(DateTime dateFrom, DateTime dateTo, int categoryId, int classificationId, int costCenterId)
        {
            List<ItemSalesPopularityInfoBO> itemSales = new List<ItemSalesPopularityInfoBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantItemSalesPopularityInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
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

                    if (classificationId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ClassificationId", DbType.Int32, classificationId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ClassificationId", DbType.Int32, DBNull.Value);
                    }
                    
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, dateTo);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemSalesPopularity");
                    DataTable Table = ds.Tables["ItemSalesPopularity"];

                    itemSales = Table.AsEnumerable().Select(r => new ItemSalesPopularityInfoBO
                    {
                        CategoryName = r.Field<string>("CategoryName"),
                        Classification = r.Field<string>("Classification"),
                        CostCenter = r.Field<string>("CostCenter"),
                        ItemName = r.Field<string>("ItemName"),
                        Amount = r.Field<decimal>("Amount"),
                        Quantity = r.Field<decimal>("Quantity"),
                        Discount = r.Field<decimal>("Discount"),
                        Value = r.Field<decimal>("Value"),
                        Rate = r.Field<decimal>("Rate")
                        
                    }).ToList();
                }
            }

            return itemSales;
        }
    }
}
