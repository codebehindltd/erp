using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Restaurant
{
    public class KotBillMasterDA : BaseService
    {
        public string GenarateRestaurantTokenNumber(int costCenterId)
        {
            string TokenNumber = string.Empty;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GenarateRestaurantTokenNumber_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                TokenNumber = Convert.ToInt32(reader["TokenNumber"]).ToString();
                            }
                        }
                    }
                }
            }
            return TokenNumber;
        }
        public string GenarateTokenNumberByKot(int kotId)
        {
            string tokenNumber = string.Empty, query = string.Empty;

            query = "SELECT DBO.FnDisplayTokenNumber(" + kotId.ToString() + ")";

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    cmd.Connection = conn;
                    tokenNumber = cmd.ExecuteScalar().ToString();
                }
            }
            return tokenNumber;
        }
        public Boolean SaveKotBillMasterInfo(KotBillMasterBO entityBO, out int tmpPKId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveKotBillMasterInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, entityBO.BearerId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, entityBO.CostCenterId);
                    dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, entityBO.SourceName);
                    dbSmartAspects.AddInParameter(command, "@SourceId", DbType.Int32, entityBO.SourceId);
                    dbSmartAspects.AddInParameter(command, "@TokenNumber", DbType.String, entityBO.TokenNumber);
                    dbSmartAspects.AddInParameter(command, "@TotalAmount", DbType.Decimal, entityBO.TotalAmount);
                    dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, entityBO.PaxQuantity);
                    dbSmartAspects.AddInParameter(command, "@IsBillHoldup", DbType.Boolean, entityBO.IsBillHoldup);

                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, entityBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@KotId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpPKId = Convert.ToInt32(command.Parameters["@KotId"].Value);
                }
            }
            return status;
        }
        public Boolean SaveKotBillMasterInfoForNewTouchScreen(KotBillMasterBO entityBO, out int tmpPKId)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveKotBillMasterInfoForAll_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, entityBO.BearerId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, entityBO.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, entityBO.SourceName);
                            dbSmartAspects.AddInParameter(command, "@SourceId", DbType.Int32, entityBO.SourceId);
                            dbSmartAspects.AddInParameter(command, "@TokenNumber", DbType.String, entityBO.TokenNumber);
                            dbSmartAspects.AddInParameter(command, "@TotalAmount", DbType.Decimal, entityBO.TotalAmount);
                            dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, entityBO.PaxQuantity);
                            dbSmartAspects.AddInParameter(command, "@IsBillHoldup", DbType.Boolean, entityBO.IsBillHoldup);
                            dbSmartAspects.AddInParameter(command, "@KotStatus", DbType.String, entityBO.KotStatus);

                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, entityBO.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@KotId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;

                            tmpPKId = Convert.ToInt32(command.Parameters["@KotId"].Value);

                            if (status)
                            {
                                transction.Commit();
                            }
                            else
                            {
                                transction.Rollback();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        status = false;
                        throw ex;
                    }
                }
            }

            return status;
        }
        public Boolean UpdateKotBillMasterForToken(Int32 kotId, string tokenNumber)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterForToken_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(command, "@TokenNumber", DbType.String, tokenNumber);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean UpdateKotBillMasterForHoldUp(Int32 kotId, bool isBillHoldup)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterForHoldUp_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(command, "@IsBillHoldup", DbType.String, isBillHoldup);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public KotBillMasterBO GetKotBillMasterInfoKotId(int kotId)
        {
            KotBillMasterBO entityBO = new KotBillMasterBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetKotBillMasterInfoKotId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);

                                entityBO.BearerId = Convert.ToInt32(reader["BearerId"]);
                                entityBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                entityBO.SourceName = reader["SourceName"].ToString();
                                entityBO.SourceId = Convert.ToInt32(reader["SourceId"]);
                                entityBO.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                entityBO.PaxQuantity = Convert.ToInt32(reader["PaxQuantity"]);
                                entityBO.Remarks = reader["Remarks"].ToString();
                                entityBO.TokenNumber = reader["TokenNumber"].ToString();
                                entityBO.IsBillProcessed = Convert.ToBoolean(reader["IsBillProcessed"].ToString());
                                entityBO.KotStatus = (reader["KotStatus"] == null ? string.Empty : reader["KotStatus"].ToString());
                                entityBO.IsBillHoldup = Convert.ToBoolean(reader["IsBillHoldup"].ToString());

                                if (reader["LastModifiedBy"] != DBNull.Value)
                                    entityBO.LastModifiedBy = Convert.ToInt32(reader["LastModifiedBy"]);
                                if (reader["LastModifiedDate"] != DBNull.Value)
                                {
                                    entityBO.LastModifiedDate = Convert.ToDateTime(reader["LastModifiedDate"]);
                                }
                                if (reader["CreatedBy"] != DBNull.Value)
                                {
                                    entityBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                }
                                if (reader["CreatedDate"] != DBNull.Value)
                                {
                                    entityBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                }
                                if (reader["KotDate"] != DBNull.Value)
                                {
                                    entityBO.KotDate = Convert.ToDateTime(reader["KotDate"]);
                                    entityBO.KotDateForAPI = Convert.ToDateTime(reader["KotDate"]);
                                }
                            }
                        }
                    }
                }
            }
            return entityBO;
        }        
        public List<KotBillMasterBO> GetPendingKotInfoByKotDate(DateTime kotDate)
        {
            List<KotBillMasterBO> entityBOList = new List<KotBillMasterBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPendingKotInfoByKotDate_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotDate", DbType.DateTime, kotDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                KotBillMasterBO entityBO = new KotBillMasterBO();
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.CostCenter = reader["CostCenter"].ToString();
                                entityBO.TransactionType = reader["TransactionType"].ToString();
                                entityBO.TransactionBy = reader["TransactionBy"].ToString();
                                entityBO.CreatedByName = reader["CreatedByName"].ToString();
                                entityBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                entityBO.CreatedDateDisplay = reader["CreatedDateDisplay"].ToString();

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public Boolean UpdateKotBillMasterForKotPending(Int32 kotId, string kotStatus)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterForKotPending_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(command, "@KotStatus", DbType.String, kotStatus);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public KotBillMasterBO GetWaiterInformationByKotId(int kotId)
        {
            KotBillMasterBO entityBO = new KotBillMasterBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetWaiterInformationByKotId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.WaiterName = reader["WaiterName"].ToString();
                            }
                        }
                    }
                }
            }
            return entityBO;
        }
        public List<KotBillDetailBO> GetKotBillDetailInfoByKotId(int kotId)
        {
            List<KotBillDetailBO> kotDetails = new List<KotBillDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetKotBillDetailsKotId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "KotBillDetail");
                    DataTable Table = ds.Tables["KotBillDetail"];

                    kotDetails = Table.AsEnumerable().Select(r => new KotBillDetailBO
                    {
                        KotDetailId = r.Field<Int32>("KotDetailId"),
                        KotId = r.Field<Int32>("KotId"),
                        ItemType = r.Field<string>("ItemType"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        ItemUnit = r.Field<decimal>("ItemUnit"),
                        UnitRate = r.Field<decimal>("UnitRate"),
                        Amount = r.Field<decimal>("Amount"),
                        DiscountedAmount = r.Field<decimal>("DiscountedAmount"),
                        ServiceRate = r.Field<decimal>("ServiceRate"),
                        ServiceCharge = r.Field<decimal>("ServiceCharge"),
                        VatAmount = r.Field<decimal>("ServiceCharge"),
                        PrintFlag = r.Field<bool>("PrintFlag"),
                        IsChanged = r.Field<bool>("IsChanged"),
                        IsDeleted = r.Field<bool>("IsDeleted"),
                        IsDispatch = r.Field<bool>("IsDispatch"),
                        ItemCost = r.Field<decimal>("ItemCost")

                    }).ToList();
                }
            }
            return kotDetails;
        }
        public KotBillMasterBO GetKotBillMasterInfoByTableId(int workingCostCenterId, string orderType, int tableId)
        {
            KotBillMasterBO entityBO = new KotBillMasterBO();
            //using (DbConnection conn = dbSmartAspects.CreateConnection())
            //{
            //    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetKotBillMasterInfoByTableId_SP"))
            //    {
            //        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, workingCostCenterId);
            //        dbSmartAspects.AddInParameter(cmd, "@OrderType", DbType.String, orderType);
            //        dbSmartAspects.AddInParameter(cmd, "@TableId", DbType.Int32, tableId);

            //        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
            //        {
            //            if (reader != null)
            //            {
            //                while (reader.Read())
            //                {
            //                    entityBO.KotId = Convert.ToInt32(reader["KotId"]);
            //                    entityBO.KotDate = Convert.ToDateTime(reader["KotDate"]);
            //                    entityBO.BearerId = Convert.ToInt32(reader["BearerId"]);
            //                    entityBO.BearerName = reader["BearerName"].ToString();
            //                    entityBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
            //                    entityBO.SourceName = reader["SourceName"].ToString();
            //                    entityBO.SourceId = Convert.ToInt32(reader["SourceId"]);
            //                    entityBO.PaxQuantity = Convert.ToInt32(reader["PaxQuantity"]);
            //                    entityBO.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
            //                    entityBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
            //                }
            //            }
            //        }
            //    }
            //}
            return entityBO;
        }
        public KotBillMasterViewBO GetKotBillMasterBySourceIdNType(int costCenterId, string sourceName, int sourceId)
        {
            KotBillMasterViewBO kot = new KotBillMasterViewBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetKotBillMasterBySourceIdNType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@SourceName", DbType.String, sourceName);
                    dbSmartAspects.AddInParameter(cmd, "@SourceId", DbType.Int32, sourceId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantTokenInfo");
                    DataTable Table = ds.Tables["RestaurantTokenInfo"];

                    kot = Table.AsEnumerable().Select(r => new KotBillMasterViewBO
                    {
                        KotId = r.Field<Int32>("KotId"),
                        CostCenterId = r.Field<Int32?>("CostCenterId"),
                        SourceName = r.Field<string>("SourceName"),
                        SourceId = r.Field<Int32?>("SourceId"),
                        TokenNumber = r.Field<string>("TokenNumber"),
                        IsBillHoldup = r.Field<bool?>("IsBillHoldup")

                    }).FirstOrDefault();
                }
            }

            return kot;
        }
        public KotBillMasterViewBO GetKotBillMasterByTokenNumberNType(int costCenterId, string sourceName, string tokenNumber)
        {
            KotBillMasterViewBO kot = new KotBillMasterViewBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetKotBillMasterBySourceIdNType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@SourceName", DbType.String, sourceName);
                    dbSmartAspects.AddInParameter(cmd, "@TokenNumber", DbType.String, tokenNumber);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantTokenInfo");
                    DataTable Table = ds.Tables["RestaurantTokenInfo"];

                    kot = Table.AsEnumerable().Select(r => new KotBillMasterViewBO
                    {
                        KotId = r.Field<Int32>("KotId"),
                        CostCenterId = r.Field<Int32?>("CostCenterId"),
                        SourceName = r.Field<string>("SourceName"),
                        SourceId = r.Field<Int32?>("SourceId"),
                        TokenNumber = r.Field<string>("TokenNumber"),
                        IsBillHoldup = r.Field<bool?>("IsBillHoldup")

                    }).FirstOrDefault();
                }
            }

            return kot;
        }
        public List<KotBillMasterBO> GetBillDetailInfoByKotId(string sourceName, int kotId)
        {
            List<KotBillMasterBO> entityBOList = new List<KotBillMasterBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBillDetailInfoByKotId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SourceName", DbType.String, sourceName);
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                KotBillMasterBO entityBO = new KotBillMasterBO();
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.PaxQuantity = Convert.ToInt32(reader["PaxQuantity"]);
                                entityBO.SourceId = Convert.ToInt32(reader["TableId"]);
                                entityBO.SourceNumber = reader["TableNumber"].ToString();
                                entityBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);

                                if (!string.IsNullOrEmpty(reader["BillId"].ToString()))
                                    entityBO.BillId = Convert.ToInt32(reader["BillId"].ToString());

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<KotBillMasterBO> GetRestaurantTokenInfoForBill(int costcenterId, string sourceName)
        {
            List<KotBillMasterBO> entityBOList = new List<KotBillMasterBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantTokenInfoForBill_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costcenterId);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterView", DbType.String, sourceName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                KotBillMasterBO entityBO = new KotBillMasterBO();
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                entityBO.SourceName = reader["SourceName"].ToString();
                                entityBO.SourceId = Convert.ToInt32(reader["SourceId"]);
                                entityBO.TokenNumber = reader["TokenNumber"].ToString();
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<KotBillMasterBO> GetRestaurantTokenInfoForFoodDispatch(int costcenterId, string sourceName)
        {
            List<KotBillMasterBO> entityBOList = new List<KotBillMasterBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantTokenInfoForFoodDispatch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costcenterId);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterView", DbType.String, sourceName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                KotBillMasterBO entityBO = new KotBillMasterBO();
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                entityBO.SourceName = reader["SourceName"].ToString();
                                entityBO.SourceId = Convert.ToInt32(reader["SourceId"]);
                                entityBO.TokenNumber = reader["TokenNumber"].ToString();
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public int GETKotIDByCostNTableID(int tableId, int costCenterId)
        {
            int kotId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GETKotIDByCostNTableID_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TableId", DbType.Int32, tableId);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                kotId = Convert.ToInt32(reader["KotId"].ToString());
                            }
                        }
                    }
                }
            }

            return kotId;
        }
        public int GETKotIDByCostNRoomNumber(string roomNumber, int costCenterId)
        {
            int kotId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GETKotIDByCostNRoomNumber_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNumber);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                kotId = Convert.ToInt32(reader["KotId"].ToString());
                            }
                        }
                    }
                }
            }

            return kotId;
        }
        public Boolean UpdateKotBillMasterPaxInfo(int kotId, int costCenterId, int tableId, int paxQuantity)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterPaxInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, tableId);
                    dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, paxQuantity);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public Boolean UpdateKotBillMasterPaxInfo(int kotId, int costCenterId, int paxQuantity)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillPaxInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, paxQuantity);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public Boolean UpdateKotWaiterInformation(int kotId, int costCenterId, int waiterId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotWaiterInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(command, "@WaiterId", DbType.Int32, waiterId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean UpdateKotBillMasterRemarksInfo(int kotId, int costCenterId, int tableId, string remarks)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterRemarksInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, tableId);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, remarks);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean UpdateRestaurantTableStatus(int costCenterId, int tableId, int statusId, int lastModifiedBy)
        {
            Boolean status = false;
            int transactionCount = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantTableStatus_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenterId);
                            dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, tableId);
                            dbSmartAspects.AddInParameter(command, "@StatusId", DbType.Int32, statusId);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction) > 0 ? true : false;

                        }
                        if (status)
                        {
                            transaction.Commit();
                        }
                        else
                            transaction.Rollback();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                }
            }
            return status;
        }
        public Boolean UpdateRestaurantTableShift(int costCenterId, int oldTableId, int newTableId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantTableShift_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(command, "@OldTableId", DbType.Int32, oldTableId);
                    dbSmartAspects.AddInParameter(command, "@NewTableId", DbType.Int32, newTableId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<KotBillMasterBO> GetBillDetailInformationForRoom()
        {
            List<KotBillMasterBO> entityBOList = new List<KotBillMasterBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBillDetailInformationForRoom_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                KotBillMasterBO entityBO = new KotBillMasterBO();
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);

                                entityBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                entityBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                entityBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);

                                //entityBO.PaxQuantity = Convert.ToInt32(reader["PaxQuantity"]);
                                //entityBO.SourceId = Convert.ToInt32(reader["TableId"]);
                                //entityBO.SourceNumber = reader["TableNumber"].ToString();

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }

        public KotBillMasterBO GetBillDetailInformationForRoomByRoomNumber(string roomNumer)
        {
            KotBillMasterBO entityBO = new KotBillMasterBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBillDetailInformationForRoomByRoomNumber_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomNumer", DbType.String, roomNumer);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);

                                entityBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                entityBO.RoomId = Convert.ToInt32(reader["RoomId"]);
                                entityBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                entityBO.IsStopChargePosting = Convert.ToBoolean(reader["IsStopChargePosting"]);
                                entityBO.BillId = Convert.ToInt32(reader["BillId"]);
                                //entityBO.SourceId = Convert.ToInt32(reader["TableId"]);
                                //entityBO.SourceNumber = reader["TableNumber"].ToString();
                            }
                        }
                    }
                }
            }
            return entityBO;
        }
        public KotBillMasterBO GetCategoryWisePercentageDiscountInfo(int kotId, string margeKotId, int costCenterId, string strCategoryIdList, string discountType, decimal discountAmount)
        {
            KotBillMasterBO entityBO = new KotBillMasterBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCategoryWisePercentageDiscountInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(cmd, "@MargeKotId", DbType.String, margeKotId);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@CategoryIdList", DbType.String, strCategoryIdList);
                    dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, discountType);
                    dbSmartAspects.AddInParameter(cmd, "@DiscountPercent", DbType.Decimal, discountAmount);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                            }
                        }
                    }
                }
            }
            return entityBO;
        }
        public KotBillMasterBO GetCategoryWiseDiscountAmountByDefaultSetting(int kotId, int costCenterId, decimal discountAmount, string discountType)
        {
            KotBillMasterBO entityBO = new KotBillMasterBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCategoryWiseDiscountByDefaultSetting_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@DiscountPercent", DbType.Decimal, discountAmount);
                    dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, discountType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                entityBO.CategoryList = Convert.ToString(reader["CategoryList"]);
                            }
                        }
                    }
                }
            }

            return entityBO;
        }
        public List<KotBillMasterBO> GetRestaurantTokenInfo()
        {
            List<KotBillMasterBO> kot = new List<KotBillMasterBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantTokenInfo_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantTokenInfo");
                    DataTable Table = ds.Tables["RestaurantTokenInfo"];

                    kot = Table.AsEnumerable().Select(r => new KotBillMasterBO
                    {
                        KotId = r.Field<Int32>("KotId"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        SourceName = r.Field<string>("SourceName"),
                        SourceId = r.Field<Int32>("SourceId"),
                        TokenNumber = r.Field<string>("TokenNumber"),
                        IsBillHoldup = r.Field<bool>("IsBillHoldup")

                    }).ToList();
                }
            }

            return kot;
        }
        public KotBillMasterBO GetKotBillMasterInfoByKotIdNSourceName(int kotId, string sourceName)
        {
            KotBillMasterBO entityBO = new KotBillMasterBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetKotBillMasterInfoByKotIdNSourceName_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(cmd, "@SourceName", DbType.String, sourceName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.KotDate = Convert.ToDateTime(reader["KotDate"]);
                                entityBO.BearerId = Convert.ToInt32(reader["BearerId"]);
                                entityBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                entityBO.SourceName = reader["SourceName"].ToString();
                                entityBO.SourceId = Convert.ToInt32(reader["SourceId"]);
                                entityBO.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                entityBO.PaxQuantity = Convert.ToInt32(reader["PaxQuantity"]);
                                entityBO.Remarks = reader["Remarks"].ToString();
                                entityBO.TokenNumber = reader["TokenNumber"].ToString();
                                entityBO.IsBillProcessed = Convert.ToBoolean(reader["IsBillProcessed"].ToString());
                                entityBO.KotStatus = (reader["KotStatus"] == null ? string.Empty : reader["KotStatus"].ToString());
                                entityBO.IsBillHoldup = Convert.ToBoolean(reader["IsBillHoldup"].ToString());
                            }
                        }
                    }
                }
            }
            return entityBO;
        }

        public bool GetIsKotItemEmptyOrNotByKotId(int kotId)
        {
            string query = string.Format("SELECT ISNULL(SUM(ItemUnit), 0.00) TotalItem FROM RestaurantKotBillDetail WHERE KotId = {0}", kotId);
            decimal totalItem = 0.00M;

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
                                totalItem = Convert.ToDecimal(reader["TotalItem"]);
                            }
                        }
                    }
                }
            }
            return totalItem > 0 ? false : true;
        }
        public bool GetIsKotIsSettledOrNotByKotId(int kotId, int tableId)
        {
            string query = string.Format("SELECT ISNULL(IsBillSettlement, 0) IsBillSettlement FROM RestaurantBill rb " +
                                        "INNER JOIN RestaurantBillDetail rbd ON rb.BillId = rbd.BillId " +
                                        "WHERE BillPaidBySourceId = {0} AND rbd.KotId = {1}", tableId, kotId);

            bool isBillReSettlement = false;

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
                                isBillReSettlement = Convert.ToBoolean(reader["IsBillSettlement"]);
                            }
                        }
                    }
                }
            }
            return isBillReSettlement;
        }

        public bool GetIsKotIsSettledOrNotByBillId(int billId, int tableId)
        {
            string query = string.Format("SELECT ISNULL(IsBillSettlement, 0) IsBillSettlement FROM RestaurantBill rb " +
                                        "INNER JOIN RestaurantBillDetail rbd ON rb.BillId = rbd.BillId " +
                                        "WHERE BillPaidBySourceId = {0} AND rbd.BillId = {1}", tableId, billId);

            bool isBillReSettlement = false;

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
                                isBillReSettlement = Convert.ToBoolean(reader["IsBillSettlement"]);
                            }
                        }
                    }
                }
            }
            return isBillReSettlement;
        }

        public bool GetIsBillExistsByKotId(int kotId, int tableId)
        {
            string query = string.Format("SELECT ISNULL(rb.BillId, 0) IsBillExists FROM RestaurantBill rb " +
                                        "INNER JOIN RestaurantBillDetail rbd ON rb.BillId = rbd.BillId " +
                                        "WHERE BillPaidBySourceId = {0} AND rbd.KotId = {1}", tableId, kotId);

            int BillId = 0;

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
                                BillId = Convert.ToInt32(reader["IsBillExists"]);
                            }
                        }
                    }
                }
            }
            return BillId > 0 ? true : false;
        }

        public Boolean UpdateRoomShiftForRestaurant(int costcenterId, int oldRoomId, string oldRoomNumber, int kotId, string newRoomNumber, int registrationId)
        {
            Boolean status = false;
            int transactionCount = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRoomShiftForRestaurant_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costcenterId);
                        dbSmartAspects.AddInParameter(command, "@OldRoomId", DbType.Int32, oldRoomId);
                        dbSmartAspects.AddInParameter(command, "@OldRoomNumber", DbType.String, oldRoomNumber);
                        dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                        dbSmartAspects.AddInParameter(command, "@NewRoomNumber", DbType.String, newRoomNumber);
                        dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, registrationId);

                        status = dbSmartAspects.ExecuteNonQuery(command, transaction) > 0 ? true : false;
                        if (status)
                        {
                            transactionCount = transactionCount + 1;
                        }
                    }
                    if (transactionCount == 1)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
            }
            return status;
        }

        public Boolean CleanRestaurantOrderForRommService(int costCenterId, int kotId, int roomId, int lastModifiedBy)
        {
            Boolean status = false;
           
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CleanRestaurantOrderForRommService_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenterId);
                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                            dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, roomId);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction) > 0 ? true : false;

                        }
                        if (status)
                        {
                            transaction.Commit();
                        }
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

    }
}
