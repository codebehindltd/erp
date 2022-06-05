using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Inventory;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantKitchenDA : BaseService
    {
        public List<RestaurantKitchenBO> GetALLKotByKitchenId(int userInfoId, int KitchenId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<RestaurantKitchenBO> RestaurantKitchenBOList = new List<RestaurantKitchenBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetALLKotByKitchenIdForGridPaging_SP"))
                {
                    if (userInfoId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, userInfoId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, DBNull.Value);


                    if (KitchenId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@KitchenId", DbType.Int32, KitchenId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@KitchenId", DbType.Int32, DBNull.Value);


                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet CashDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, CashDS, "Cash");
                    DataTable Table = CashDS.Tables["Cash"];

                    RestaurantKitchenBOList = Table.AsEnumerable().Select(r => new RestaurantKitchenBO
                    {
                        KotId = r.Field<Int32>("KotId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        IsCanDeliver = r.Field<Int32>("IsCanDeliver"),

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return RestaurantKitchenBOList;
        }

        public bool CompleteDeliveryProcess(int itemId, int kotId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CompleteDeliveryProcess_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@kotId", DbType.Int32, kotId);
                        if (itemId != 0)
                            dbSmartAspects.AddInParameter(command, "@itemId", DbType.Int32, itemId);
                        else
                            dbSmartAspects.AddInParameter(command, "@itemId", DbType.Int32, DBNull.Value);

                        status = (dbSmartAspects.ExecuteNonQuery(command) > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        public List<InvItemBO> LoadKotDetailsForKitchen(int userInfoId, int KitchenId, int kotid, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<InvItemBO> InvItemBOList = new List<InvItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetALLKotDetailsForKitchenForGridPaging_SP"))
                {
                    if (userInfoId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, userInfoId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, DBNull.Value);


                    if (KitchenId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@KitchenId", DbType.Int32, KitchenId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@KitchenId", DbType.Int32, DBNull.Value);

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

                    InvItemBOList = Table.AsEnumerable().Select(r => new InvItemBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("ItemName"),
                        ItemType = r.Field<string>("ItemType"),
                        ItemUnit = r.Field<decimal>("ItemUnit")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return InvItemBOList;
        }
        public List<RestaurantKitchenBO> GetRestaurantKitchenInfo()
        {
            List<RestaurantKitchenBO> bankList = new List<RestaurantKitchenBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantKitchenInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantKitchenBO bank = new RestaurantKitchenBO();

                                bank.KitchenId = Convert.ToInt32(reader["KitchenId"]);
                                bank.KitchenName = reader["KitchenName"].ToString();
                                bank.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bank.ActiveStatus = reader["ActiveStatus"].ToString();

                                bankList.Add(bank);
                            }
                        }
                    }
                }
            }
            return bankList;
        }
        public List<RestaurantKitchenBO> GetRestaurantKitchenInfoByUserInfoId(int userInfoId)
        {
            List<RestaurantKitchenBO> bankList = new List<RestaurantKitchenBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantKitchenInfoByUserInfoId_SP"))
                {
                    if (userInfoId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, DBNull.Value);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantKitchenBO bank = new RestaurantKitchenBO();

                                bank.KitchenId = Convert.ToInt32(reader["KitchenId"]);
                                bank.KitchenName = reader["KitchenName"].ToString();
                                bank.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bank.ActiveStatus = reader["ActiveStatus"].ToString();

                                bankList.Add(bank);
                            }
                        }
                    }
                }
            }
            return bankList;
        }
        public Boolean SaveRestaurantKitchenInfo(RestaurantKitchenBO bankBO, List<RestaurantKitchenCostCenterMappingBO> costCenterList, out int tmpBankId)
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurantKitchenInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(command, "@KitchenName", DbType.String, bankBO.KitchenName);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bankBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bankBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@KitchenId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        tmpBankId = Convert.ToInt32(command.Parameters["@KitchenId"].Value);

                        using (DbCommand commandMapping = dbSmartAspects.GetStoredProcCommand("SaveRestaurantKitchenCostCenterMappingInfo_SP"))
                        {
                            foreach (RestaurantKitchenCostCenterMappingBO mappingBO in costCenterList)
                            {
                                commandMapping.Parameters.Clear();

                                dbSmartAspects.AddInParameter(commandMapping, "@KitchenId", DbType.Int32, tmpBankId);
                                dbSmartAspects.AddInParameter(commandMapping, "@CostCenterId", DbType.Int32, mappingBO.CostCenterId);
                                dbSmartAspects.AddOutParameter(commandMapping, "@MappingId", DbType.Int64, sizeof(Int32));
                                status = dbSmartAspects.ExecuteNonQuery(commandMapping, transction);
                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        public Boolean UpdateRestaurantKitchenInfo(RestaurantKitchenBO bankBO, List<RestaurantKitchenCostCenterMappingBO> costCenterList)
        {
            bool retVal = false;
            int status = 0;
            Boolean costStatus = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantKitchenInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {

                        dbSmartAspects.AddInParameter(command, "@KitchenId", DbType.Int32, bankBO.KitchenId);
                        dbSmartAspects.AddInParameter(command, "@KitchenName", DbType.String, bankBO.KitchenName);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bankBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, bankBO.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(command, transction);

                        if (costCenterList != null)
                        {
                            if (costCenterList.Count > 0)
                            {
                                costStatus = UpdateResKitchenCostCenterMapping(costCenterList);
                                if (status > 0)
                                    if (costStatus)
                                    {
                                        transction.Commit();
                                        retVal = true;
                                    }
                                    else
                                    {
                                        retVal = false;
                                    }
                            }
                            else
                            {
                                transction.Commit();
                                retVal = true;
                            }
                        }
                        else
                        {
                            transction.Commit();
                            retVal = true;
                        }
                    }
                }
            }
            return retVal;
        }
        public RestaurantKitchenBO GetRestaurantKitchenInfoById(int bankId)
        {
            RestaurantKitchenBO bank = new RestaurantKitchenBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantKitchenInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KitchenId", DbType.Int32, bankId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bank.KitchenId = Convert.ToInt32(reader["KitchenId"]);
                                bank.KitchenName = reader["KitchenName"].ToString();
                                bank.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bank.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return bank;
        }
        public List<RestaurantKitchenBO> GetRestaurantKitchenInfoBySearchCriteria(string kitchenName, bool ActiveStat)
        {
            List<RestaurantKitchenBO> bankList = new List<RestaurantKitchenBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantKitchenInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KitchenName", DbType.String, kitchenName);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, ActiveStat);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantKitchenBO bank = new RestaurantKitchenBO();
                                bank.KitchenId = Convert.ToInt32(reader["KitchenId"]);
                                bank.KitchenName = reader["KitchenName"].ToString();
                                bank.CostCenter = reader["CostCenter"].ToString();
                                bank.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bank.ActiveStatus = reader["ActiveStatus"].ToString();
                                bankList.Add(bank);
                            }
                        }
                    }
                }
            }
            return bankList;
        }
        public List<RestaurantKitchenBO> GetRestaurantKitchenInfoByCostCenterId(int costCenterId)
        {
            List<RestaurantKitchenBO> bankList = new List<RestaurantKitchenBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantKitchenInfoByCostCenterId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantKitchenBO bank = new RestaurantKitchenBO();
                                bank.KitchenId = Convert.ToInt32(reader["KitchenId"]);
                                bank.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                bank.CostCenter = reader["CostCenter"].ToString();
                                bank.KitchenName = reader["KitchenName"].ToString();
                                bank.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bank.ActiveStatus = reader["ActiveStatus"].ToString();
                                bankList.Add(bank);
                            }
                        }
                    }
                }
            }
            return bankList;
        }
        private bool UpdateResKitchenCostCenterMapping(List<RestaurantKitchenCostCenterMappingBO> costCenterList)
        {
            InvCategoryCostCenterMappingDA costDA = new InvCategoryCostCenterMappingDA();
            List<RestaurantKitchenCostCenterMappingBO> dbCostList = new List<RestaurantKitchenCostCenterMappingBO>();
            dbCostList = costDA.GetRestaurantKitchenCostCenterMappingByKitchenId(costCenterList[0].KitchenId);

            List<long> idList = new List<long>();
            long mappingId;

            for (int i = 0; i < costCenterList.Count; i++)
            {
                long succ = IsAvailableInDb(dbCostList, costCenterList[i]);
                if (succ > 0)
                {
                    //Update
                    costCenterList[i].MappingId = succ;
                    bool status = UpdateResKitchenCostCenterMappingInfo(costCenterList[i]);
                    idList.Add(succ);
                }
                else
                {
                    //Insert
                    bool status = SaveResKitchenCostCenterMappingInfo(costCenterList[i], out mappingId);
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
            Boolean deleteStatus = DeleteAllResKitchenCostCenterMappingInfoWithoutMappingIdList(costCenterList[0].KitchenId, saveAndUpdatedIdList);
            return true;
        }
        private long IsAvailableInDb(List<RestaurantKitchenCostCenterMappingBO> dbCostList, RestaurantKitchenCostCenterMappingBO costBO)
        {
            long isInDB = 0;
            for (int j = 0; j < dbCostList.Count; j++)
            {
                if (dbCostList[j].KitchenId == costBO.KitchenId && costBO.CostCenterId == dbCostList[j].CostCenterId)
                {
                    isInDB = dbCostList[j].MappingId;
                }
            }
            return isInDB;
        }
        public bool SaveResKitchenCostCenterMappingInfo(RestaurantKitchenCostCenterMappingBO costCenter, out long MappingId)
        {

            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurantKitchenCostCenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@KitchenId", DbType.Int32, costCenter.KitchenId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenter.CostCenterId);
                    dbSmartAspects.AddOutParameter(command, "@MappingId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    MappingId = Convert.ToInt64(command.Parameters["@MappingId"].Value);
                }
            }
            return status;
        }
        public bool UpdateResKitchenCostCenterMappingInfo(RestaurantKitchenCostCenterMappingBO costCenter)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantKitchenCostCenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@MappingId", DbType.Int32, costCenter.MappingId);
                    dbSmartAspects.AddInParameter(command, "@KitchenId", DbType.Int32, costCenter.KitchenId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenter.CostCenterId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean DeleteAllResKitchenCostCenterMappingInfoWithoutMappingIdList(int kitchenId, string mappingIdList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteAllResKitchenCostCenterMappingInfoWithoutMappingIdList_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@KitchenId", DbType.Int32, kitchenId);
                    dbSmartAspects.AddInParameter(command, "@MappingIdList", DbType.String, mappingIdList);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
