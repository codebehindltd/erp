using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.Restaurant;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantComboCostCenterMappingDA : BaseService
    {
        public bool SaveRestaurantComboCostCenterMappingInfo(RestaurantComboCostCenterMappingBO costCenter, out int MappingId)
        {
            
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurantComboCostCenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ComboId", DbType.Int32, costCenter.ComboId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenter.CostCenterId);
                    dbSmartAspects.AddInParameter(command, "@MinimumStockLevel", DbType.Decimal, costCenter.MinimumStockLevel);
                    dbSmartAspects.AddInParameter(command, "@UnitPriceLocal", DbType.Decimal, costCenter.UnitPriceLocal);
                    dbSmartAspects.AddInParameter(command, "@UnitPriceUsd", DbType.Decimal, costCenter.UnitPriceUsd);
                    dbSmartAspects.AddOutParameter(command, "@MappingId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    MappingId = Convert.ToInt32(command.Parameters["@MappingId"].Value);
                }
            }
            return status;
        }
        public bool UpdateRestaurantComboCostCenterMappingInfo(RestaurantComboCostCenterMappingBO costCenter)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantComboCostCenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@MappingId", DbType.Int32, costCenter.MappingId);
                    dbSmartAspects.AddInParameter(command, "@ComboId", DbType.Int32, costCenter.ComboId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenter.CostCenterId);
                    dbSmartAspects.AddInParameter(command, "@MinimumStockLevel", DbType.Decimal, costCenter.MinimumStockLevel);
                    dbSmartAspects.AddInParameter(command, "@UnitPriceLocal", DbType.Decimal, costCenter.UnitPriceLocal);
                    dbSmartAspects.AddInParameter(command, "@UnitPriceUsd", DbType.Decimal, costCenter.UnitPriceUsd);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<RestaurantComboCostCenterMappingBO> GetRestaurantComboCostCenterMappingByComboId(int ComboId)
        {
            List<RestaurantComboCostCenterMappingBO> costList = new List<RestaurantComboCostCenterMappingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantComboCostCenterMappingByComboId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ComboId", DbType.Int32, ComboId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantComboCostCenterMappingBO costBO = new RestaurantComboCostCenterMappingBO();
                                costBO.ComboId = Convert.ToInt32(reader["ComboId"]);
                                costBO.MappingId = Convert.ToInt32(reader["MappingId"]);
                                costBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costBO.MinimumStockLevel = Convert.ToInt32(reader["MinimumStockLevel"]);
                                costBO.UnitPriceLocal = Convert.ToInt32(reader["UnitPriceLocal"]);
                                costBO.UnitPriceUsd = Convert.ToInt32(reader["UnitPriceUsd"]);
                                costList.Add(costBO);
                            }
                        }
                    }
                }
            }
            return costList;
        }
        public Boolean DeleteAllRestaurantComboCostCenterMappingInfoWithoutMappingIdList(string mappingIdList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteAllRestaurantComboCostCenterMappingInfoWithoutMappingIdList_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@MappingIdList", DbType.String, mappingIdList);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

    }
}
