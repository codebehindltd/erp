using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Inventory;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.Restaurant;

namespace HotelManagement.Data.Inventory
{
    public class InvCategoryCostCenterMappingDA: BaseService
    {
        public bool SaveInvCategoryCostCenterMappingInfo(InvCategoryCostCenterMappingBO costCenter, out int MappingId)
        {

            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveInvCategoryCostCenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CategoryId", DbType.Int32, costCenter.CategoryId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenter.CostCenterId);                    
                    dbSmartAspects.AddOutParameter(command, "@MappingId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    MappingId = Convert.ToInt32(command.Parameters["@MappingId"].Value);
                }
            }
            return status;
        }
        public bool SaveInvLocationCostCenterMappingInfo(InvLocationCostCenterMappingBO costCenter, out int MappingId)
        {

            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveInvLocationCostCenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@LocationId", DbType.Int32, costCenter.LocationId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenter.CostCenterId);
                    dbSmartAspects.AddOutParameter(command, "@MappingId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    MappingId = Convert.ToInt32(command.Parameters["@MappingId"].Value);
                }
            }
            return status;
        }
        public bool UpdateInvCategoryCostCenterMappingInfo(InvCategoryCostCenterMappingBO costCenter)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateInvCategoryCostCenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@MappingId", DbType.Int32, costCenter.MappingId);
                    dbSmartAspects.AddInParameter(command, "@CategoryId", DbType.Int32, costCenter.CategoryId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenter.CostCenterId);                    
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool UpdateInvLocationCostCenterMappingInfo(InvLocationCostCenterMappingBO costCenter)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateInvLocationCostCenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@MappingId", DbType.Int32, costCenter.MappingId);
                    dbSmartAspects.AddInParameter(command, "@LocationId", DbType.Int32, costCenter.LocationId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenter.CostCenterId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<InvCategoryCostCenterMappingBO> GetInvCategoryCostCenterMappingByCategoryId(int categoryId)
        {
            List<InvCategoryCostCenterMappingBO> costList = new List<InvCategoryCostCenterMappingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvCategoryCostCenterMappingByItemId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryCostCenterMappingBO costBO = new InvCategoryCostCenterMappingBO();
                                costBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                costBO.MappingId = Convert.ToInt32(reader["MappingId"]);
                                costBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);                                
                                costList.Add(costBO);
                            }
                        }
                    }
                }
            }
            return costList;
        }
        public List<InvLocationCostCenterMappingBO> GetInvLocationCostCenterMappingByCategoryId(int locationId)
        {
            List<InvLocationCostCenterMappingBO> costList = new List<InvLocationCostCenterMappingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvLocationCostCenterMappingByItemId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvLocationCostCenterMappingBO costBO = new InvLocationCostCenterMappingBO();
                                costBO.LocationId = Convert.ToInt32(reader["LocationId"]);
                                costBO.MappingId = Convert.ToInt32(reader["MappingId"]);
                                costBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costList.Add(costBO);
                            }
                        }
                    }
                }
            }
            return costList;
        }
        public List<RestaurantKitchenCostCenterMappingBO> GetRestaurantKitchenCostCenterMappingByKitchenId(int kitchenId)
        {
            List<RestaurantKitchenCostCenterMappingBO> costCenterInfo = new List<RestaurantKitchenCostCenterMappingBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantKitchenCostCenterMappingByKitchenId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KitchenId", DbType.Int32, kitchenId);                    

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "CostCenterInfo");
                    DataTable Table = ds.Tables["CostCenterInfo"];

                    costCenterInfo = Table.AsEnumerable().Select(r => new RestaurantKitchenCostCenterMappingBO
                    {
                        MappingId = r.Field<long>("MappingId"),
                        CostCenterId = r.Field<int>("CostCenterId"),
                        KitchenId = r.Field<int>("KitchenId")                        

                    }).ToList();
                }
            }

            return costCenterInfo;
        }
        public Boolean DeleteAllInvItemCostCenterMappingInfoWithoutMappingIdList(int categoryId, string mappingIdList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteAllInvCategoryCostCenterMappingInfoWithoutMappingIdList_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CategoryId", DbType.Int32, categoryId);
                    dbSmartAspects.AddInParameter(command, "@MappingIdList", DbType.String, mappingIdList);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean DeleteAllInvLocationCostCenterMappingInfoWithoutMappingIdList(int categoryId, string mappingIdList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteAllInvLocationCostCenterMappingInfoWithoutMappingIdList_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@LocationId", DbType.Int32, categoryId);
                    dbSmartAspects.AddInParameter(command, "@MappingIdList", DbType.String, mappingIdList);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
