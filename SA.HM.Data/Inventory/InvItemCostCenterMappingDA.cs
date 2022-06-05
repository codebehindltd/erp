using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Inventory;
using System.Data.Common;
using System.Data;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Data.Inventory
{
    public class InvItemCostCenterMappingDA : BaseService
    {
        public bool SaveInvItemCostCenterMappingInfo(InvItemCostCenterMappingBO costCenter, out int MappingId)
        {

            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveInvItemCostCenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, costCenter.ItemId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenter.CostCenterId);
                    dbSmartAspects.AddInParameter(command, "@KitchenId", DbType.Int32, costCenter.KitchenId);
                    dbSmartAspects.AddInParameter(command, "@MinimumStockLevel", DbType.Decimal, costCenter.MinimumStockLevel);
                    dbSmartAspects.AddInParameter(command, "@UnitPriceLocal", DbType.Decimal, costCenter.UnitPriceLocal);
                    dbSmartAspects.AddInParameter(command, "@UnitPriceUsd", DbType.Decimal, costCenter.UnitPriceUsd);
                    dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, costCenter.ServiceCharge);
                    dbSmartAspects.AddInParameter(command, "@SDCharge", DbType.Decimal, costCenter.SDCharge);
                    dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, costCenter.VatAmount);
                    dbSmartAspects.AddInParameter(command, "@AdditionalCharge", DbType.Decimal, costCenter.AdditionalCharge);
                    dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, costCenter.DiscountType);
                    dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, costCenter.DiscountAmount);
                    dbSmartAspects.AddOutParameter(command, "@MappingId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    MappingId = Convert.ToInt32(command.Parameters["@MappingId"].Value);
                }
            }
            return status;
        }
        public bool UpdateInvItemCostCenterMappingInfo(InvItemCostCenterMappingBO costCenter)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateInvItemCostCenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@MappingId", DbType.Int32, costCenter.MappingId);
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, costCenter.ItemId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenter.CostCenterId);
                    dbSmartAspects.AddInParameter(command, "@KitchenId", DbType.Int32, costCenter.KitchenId);
                    dbSmartAspects.AddInParameter(command, "@MinimumStockLevel", DbType.Decimal, costCenter.MinimumStockLevel);
                    dbSmartAspects.AddInParameter(command, "@UnitPriceLocal", DbType.Decimal, costCenter.UnitPriceLocal);
                    dbSmartAspects.AddInParameter(command, "@UnitPriceUsd", DbType.Decimal, costCenter.UnitPriceUsd);
                    dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, costCenter.ServiceCharge);
                    dbSmartAspects.AddInParameter(command, "@SDCharge", DbType.Decimal, costCenter.SDCharge);
                    dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, costCenter.VatAmount);
                    dbSmartAspects.AddInParameter(command, "@AdditionalCharge", DbType.Decimal, costCenter.AdditionalCharge);
                    dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, costCenter.DiscountType);
                    dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, costCenter.DiscountAmount);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<InvItemCostCenterMappingBO> GetInvItemCostCenterMappingByItemId(int itemId)
        {
            List<InvItemCostCenterMappingBO> costList = new List<InvItemCostCenterMappingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemCostCenterMappingByItemId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemCostCenterMappingBO costBO = new InvItemCostCenterMappingBO();
                                costBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                costBO.MappingId = Convert.ToInt32(reader["MappingId"]);
                                costBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costBO.KitchenId = Convert.ToInt32(reader["KitchenId"]);
                                costBO.MinimumStockLevel = Convert.ToDecimal(reader["MinimumStockLevel"]);
                                costBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                costBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                costBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                costBO.SDCharge = Convert.ToDecimal(reader["SDCharge"]);
                                costBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                costBO.AdditionalCharge = Convert.ToDecimal(reader["AdditionalCharge"]);
                                costBO.DiscountType = reader["DiscountType"].ToString();
                                costBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                costList.Add(costBO);
                            }
                        }
                    }
                }
            }
            return costList;
        }
        public Boolean DeleteAllInvItemCostCenterMappingInfoWithoutMappingIdList(int itemId, string mappingIdList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteAllInvItemCostCenterMappingInfoWithoutMappingIdList_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, itemId);
                    dbSmartAspects.AddInParameter(command, "@MappingIdList", DbType.String, mappingIdList);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean DeleteInvItemCostCenterMappingInfoByMappingId(int mappingId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                {
                    cmdOutDetails.Parameters.Clear();

                    dbSmartAspects.AddInParameter(cmdOutDetails, "@TableName", DbType.String, "InvItemCostCenterMapping");
                    dbSmartAspects.AddInParameter(cmdOutDetails, "@TablePKField", DbType.String, "MappingId");
                    dbSmartAspects.AddInParameter(cmdOutDetails, "@TablePKId", DbType.String, mappingId);

                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails) > 0 ? true : false;
                }

            }
            return status;
        }
        public Boolean DeleteAllInvItemSupplierMappingInfoForItemId(int itemId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteAllInvItemSupplierMappingInfoForItemId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, itemId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean DeleteAllInvItemSupplierMappingInfoWithoutMappingIdList(int itemId, string mappingIdList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteAllInvItemSupplierMappingInfoWithoutMappingIdList_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, itemId);
                    dbSmartAspects.AddInParameter(command, "@MappingIdList", DbType.String, mappingIdList);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool SaveInvItemSupplierMappingInfo(InvItemSuppierMappingBO costCenter, out int MappingId)
        {

            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveInvItemSupplierMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, costCenter.ItemId);
                    dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.Int32, costCenter.SupplierId);
                    dbSmartAspects.AddOutParameter(command, "@MappingId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    MappingId = Convert.ToInt32(command.Parameters["@MappingId"].Value);
                }
            }
            return status;
        }
        public List<InvItemSuppierMappingBO> GetInvItemSupplierMappingByItemId(int itemId)
        {
            List<InvItemSuppierMappingBO> costList = new List<InvItemSuppierMappingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemSupplierMappingByItemId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemSuppierMappingBO costBO = new InvItemSuppierMappingBO();
                                costBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                costBO.MappingId = Convert.ToInt32(reader["MappingId"]);
                                costBO.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                                costList.Add(costBO);
                            }
                        }
                    }
                }
            }
            return costList;
        }
        public bool UpdateInvItemSuppierMappingInfo(InvItemSuppierMappingBO costCenter)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateInvItemSupplierMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@MappingId", DbType.Int32, costCenter.MappingId);
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, costCenter.ItemId);
                    dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.Int32, costCenter.SupplierId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<InvItemCostCenterMappingBO> GetInvItemCostCenterMappingInfo(int costcenterId, int itemId)
        {
            List<InvItemCostCenterMappingBO> costList = new List<InvItemCostCenterMappingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemCostcenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostcenterId", DbType.Int32, costcenterId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);                    

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "InvItemCostcenterMapping");
                    DataTable Table = ds.Tables["InvItemCostcenterMapping"];

                    costList = Table.AsEnumerable().Select(r => new InvItemCostCenterMappingBO
                    {
                        MappingId = r.Field<int>("MappingId"),
                        CostCenterId = r.Field<int>("CostCenterId"),
                        ItemId = r.Field<int>("ItemId"),
                        StockQuantity = r.Field<decimal>("StockQuantity")                        
                    }).ToList();
                }
            }
            return costList;
        }

    }
}
