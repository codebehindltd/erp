using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;
using System.Globalization;
using System.Threading;

namespace HotelManagement.Data.Restaurant
{
    public class KotBillDetailDA : BaseService
    {
        public Boolean SaveKotBillDetailInfo(KotBillDetailBO entityBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveKotBillDetailInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, entityBO.KotId);
                    dbSmartAspects.AddInParameter(command, "@ItemType", DbType.String, entityBO.ItemType);
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, entityBO.ItemId);

                    dbSmartAspects.AddInParameter(command, "@ColorId", DbType.Int32, 0);
                    dbSmartAspects.AddInParameter(command, "@SizeId", DbType.Int32, 0);
                    dbSmartAspects.AddInParameter(command, "@StyleId", DbType.Int32, 0);

                    dbSmartAspects.AddInParameter(command, "@ItemUnit", DbType.Decimal, entityBO.ItemUnit);
                    dbSmartAspects.AddInParameter(command, "@UnitRate", DbType.Decimal, entityBO.UnitRate);
                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, entityBO.Amount);
                    dbSmartAspects.AddInParameter(command, "@InvoiceDiscount", DbType.Decimal, 0);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, entityBO.CreatedBy);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, entityBO.Remarks);

                    dbSmartAspects.AddInParameter(command, "@BagWeight", DbType.Int32, 0);
                    dbSmartAspects.AddInParameter(command, "@NoOfBag", DbType.Int32, 0);


                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean UpdateKotBillDetailInfo(Int32 isItemCanEditDelete, KotBillDetailBO entityBO, string updateType)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillDetailInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@IsItemCanEditDelete", DbType.Int32, isItemCanEditDelete);
                    dbSmartAspects.AddInParameter(command, "@KotDetailId", DbType.Int32, entityBO.KotDetailId);
                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, entityBO.KotId);
                    dbSmartAspects.AddInParameter(command, "@ItemType", DbType.String, entityBO.ItemType);
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, entityBO.ItemId);
                    dbSmartAspects.AddInParameter(command, "@ItemUnit", DbType.Decimal, entityBO.ItemUnit);
                    dbSmartAspects.AddInParameter(command, "@UnitRate", DbType.Decimal, entityBO.UnitRate);
                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, entityBO.Amount);
                    dbSmartAspects.AddInParameter(command, "@UpdateType", DbType.String, updateType);
                    dbSmartAspects.AddInParameter(command, "@ItemName", DbType.String, entityBO.ItemName);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, entityBO.CreatedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public int GetKotUpdatedDataCountInfo(int kotId)
        {
            int ppdatedDataCount = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetKotUpdatedDataCountInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ppdatedDataCount = Convert.ToInt32(reader["UpdatedDataCount"]);
                            }
                        }
                    }
                }
            }
            return ppdatedDataCount;
        }        
        public KotBillDetailBO GetSrcRestaurantBillDetailInfoByKotDetailId(int kotDetailId)
        {
            KotBillDetailBO entityBO = new KotBillDetailBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSrcRestaurantBillDetailInfoByKotDetailId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotDetailId", DbType.Int32, kotDetailId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.KotDetailId = Convert.ToInt32(reader["KotDetailId"]);
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.ItemType = reader["ItemType"].ToString();
                                entityBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                entityBO.ItemName = reader["ItemName"].ToString();
                                entityBO.ItemCode = reader["ItemCode"].ToString();
                                entityBO.Category = reader["Category"].ToString();
                                entityBO.ItemUnit = Convert.ToDecimal(reader["ItemUnit"]);
                                entityBO.UnitRate = Convert.ToDecimal(reader["UnitRate"]);
                                entityBO.Amount = Convert.ToDecimal(reader["Amount"]);
                            }
                        }
                    }
                }
            }
            return entityBO;
        }
        public List<KotBillDetailBO> GetKotBillDetailInfoByKotNBearerId(int costCenterId, string sourceName, int sourceId, int kotId)
        {
            List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetKotBillDetailInfoByTableNBearerId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@SourceName", DbType.String, sourceName);
                    dbSmartAspects.AddInParameter(cmd, "@SourceId", DbType.Int32, sourceId);
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                KotBillDetailBO entityBO = new KotBillDetailBO();

                                entityBO.KotDetailId = Convert.ToInt32(reader["KotDetailId"]);
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.ItemType = reader["ItemType"].ToString();
                                entityBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                entityBO.ItemName = reader["ItemName"].ToString();
                                entityBO.ItemCode = reader["ItemCode"].ToString();
                                entityBO.Category = reader["Category"].ToString();
                                entityBO.ItemUnit = Convert.ToDecimal(reader["ItemUnit"]);
                                entityBO.UnitRate = Convert.ToDecimal(reader["UnitRate"]);
                                entityBO.Amount = Convert.ToDecimal(reader["Amount"]);
                                entityBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                entityBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                entityBO.PrintFlag = Convert.ToBoolean(reader["PrintFlag"]);
                                entityBO.IsChanged = Convert.ToBoolean(reader["IsChanged"]);
                                entityBO.IsItemEditable = Convert.ToBoolean(reader["IsItemEditable"]);

                                entityBOList.Add(entityBO);

                            }
                        }
                    }
                }
            }
            return entityBOList;
        }        
        public List<KotBillDetailBO> GetRestaurantBillDetailInfoDynamicallyForInvoice(string costCenterId, string kotId, string discountType, decimal discountPercentage, string categoryIdList, string margeKotId, bool isComplementary)
        {
            List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBillDetailInfoDynamicallyForInvoice_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.String, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@KotIdList", DbType.String, kotId);

                    if (!string.IsNullOrEmpty(categoryIdList))
                        dbSmartAspects.AddInParameter(cmd, "@CategoryIdList", DbType.String, categoryIdList);
                    else dbSmartAspects.AddInParameter(cmd, "@CategoryIdList", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, discountType);
                    dbSmartAspects.AddInParameter(cmd, "@DiscountPercent", DbType.Decimal, discountPercentage);
                    dbSmartAspects.AddInParameter(cmd, "@MargeKotId", DbType.String, kotId);
                    dbSmartAspects.AddInParameter(cmd, "@IsComplementary", DbType.Boolean, isComplementary);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                KotBillDetailBO entityBO = new KotBillDetailBO();
                                entityBO.KotDetailId = Convert.ToInt32(reader["KotDetailId"]);
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.ItemType = reader["ItemType"].ToString();
                                entityBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                entityBO.ItemName = reader["ItemName"].ToString();
                                entityBO.ItemCode = reader["ItemCode"].ToString();
                                entityBO.Category = reader["Category"].ToString();
                                entityBO.ItemUnit = Convert.ToDecimal(reader["ItemUnit"]);
                                entityBO.UnitRate = Convert.ToDecimal(reader["UnitRate"]);
                                entityBO.Amount = Convert.ToDecimal(reader["Amount"]);
                                entityBO.PrintFlag = Convert.ToBoolean(reader["PrintFlag"]);
                                entityBO.ItemWiseDiscount = Convert.ToDecimal(reader["ItemWiseDiscount"]);
                                entityBO.DiscountedAmount = Convert.ToDecimal(reader["DiscountedAmount"]);
                                entityBO.ServiceRate = Convert.ToDecimal(reader["ServiceRate"]);
                                entityBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                entityBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public KotBillDetailBO GetKotBillDetailInfoByKotNItemId(int costCenterId, int kotId, int itemId, string itemType)
        {
            KotBillDetailBO entityBO = new KotBillDetailBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetKotBillDetailInfoByKotNItemId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, itemType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.KotDetailId = Convert.ToInt32(reader["KotDetailId"]);
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.ItemType = reader["ItemType"].ToString();
                                entityBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                entityBO.ItemName = reader["ItemName"].ToString();
                                entityBO.ItemCode = reader["ItemCode"].ToString();
                                entityBO.Category = reader["Category"].ToString();
                                entityBO.ItemUnit = Convert.ToDecimal(reader["ItemUnit"]);
                                entityBO.UnitRate = Convert.ToDecimal(reader["UnitRate"]);
                                entityBO.Amount = Convert.ToDecimal(reader["Amount"]);

                                entityBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                entityBO.CitySDCharge = Convert.ToDecimal(reader["SDCharge"]);
                                entityBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                entityBO.AdditionalCharge = Convert.ToDecimal(reader["AdditionalCharge"]);


                            }
                        }
                    }
                }
            }
            return entityBO;
        }
        public List<KotBillDetailBO> GetKotOrderSubmitInfo(int kotId, int costCenterId, string stockType, bool isReprint)
        {
            List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();
            string spName = string.Empty;

            if (isReprint)
            {
                spName = "GetKotOrderReprint_SP";
            }
            else
            {
                spName = "GetKotOrderSubmitInfo_SP";
            }

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand(spName))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@StockType", DbType.String, stockType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                KotBillDetailBO entityBO = new KotBillDetailBO();

                                entityBO.KotDetailId = Convert.ToInt32(reader["KotDetailId"]);
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.ItemType = reader["ItemType"].ToString();
                                entityBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                entityBO.ItemName = reader["ItemName"].ToString();
                                entityBO.ItemCode = reader["ItemCode"].ToString();
                                entityBO.Category = reader["Category"].ToString();
                                entityBO.ItemUnit = Convert.ToDecimal(reader["ItemUnit"]);
                                entityBO.UnitRate = Convert.ToDecimal(reader["UnitRate"]);
                                entityBO.Amount = Convert.ToDecimal(reader["Amount"]);
                                entityBO.Remarks = reader["Remarks"].ToString();
                                entityBO.KitchenId = Convert.ToInt32(reader["KitchenId"]);
                                entityBO.PrinterName = reader["PrinterName"].ToString();
                                entityBO.PrintFlag = Convert.ToBoolean(reader["PrintFlag"]);
                                entityBO.IsChanged = Convert.ToBoolean(reader["IsChanged"]);
                                entityBO.PaxQuantity = Convert.ToInt32(reader["PaxQuantity"]);
                                entityBO.NewItemCount = Convert.ToInt32(reader["NewItemCount"]);
                                entityBO.VoidOrDeletedItemCount = Convert.ToInt32(reader["VoidOrDeletedItemCount"]);
                                entityBO.EditedOrChangedItemCount = Convert.ToInt32(reader["EditedOrChangedItemCount"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<KotBillDetailBO> GetKotOrderSubmitInfoByKotIdNCostCenterId(int kotId, int costCenterId)
        {
            List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetKotOrderSubmitInfoByKotIdNCostCenterId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                KotBillDetailBO entityBO = new KotBillDetailBO();

                                entityBO.KotDetailId = Convert.ToInt32(reader["KotDetailId"]);
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.ItemType = reader["ItemType"].ToString();
                                entityBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                entityBO.ItemName = reader["ItemName"].ToString();
                                entityBO.ItemCode = reader["ItemCode"].ToString();
                                entityBO.Category = reader["Category"].ToString();
                                entityBO.ItemUnit = Convert.ToDecimal(reader["ItemUnit"]);
                                entityBO.UnitRate = Convert.ToDecimal(reader["UnitRate"]);
                                entityBO.Amount = Convert.ToDecimal(reader["Amount"]);

                                entityBOList.Add(entityBO);

                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public Boolean UpdateKotBillDetailPrintFlagByOrderSubmitKotId(int kotId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillDetailPrintFlagByOrderSubmitKotId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean UpdateKotBillDetailPrintFlagByOrderSubmitBillId(int billId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillDetailPrintFlagByOrderSubmitBillId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, billId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool DeleteKotBillDetail(int kotDetailId, int kotId, int itemId, int userInfoId)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteRestaurantKotBillDetail_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@KotDetailId", DbType.Int32, kotDetailId);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }

                    if (status)
                    {
                        bool commandDeleteStatus = false;
                        using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteRestaurantKotSpecialRemarksDetailByKotItemId_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandDelete, "@KotId", DbType.Int32, kotId);
                            dbSmartAspects.AddInParameter(commandDelete, "@ItemId", DbType.Int32, itemId);
                            commandDeleteStatus = dbSmartAspects.ExecuteNonQuery(commandDelete) > 0 ? true : false;
                        }

                        transaction.Commit();
                    }
                    else
                        transaction.Rollback();
                }
            }

            return status;
        }
        public bool ClearOrderedItem(int costcenterId, int kotId)
        {
            bool status = false;

            string query = string.Format("DELETE FROM RestaurantKotBillDetail " +
                                         "WHERE KotId = {0}", kotId.ToString());

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }

                    if (status)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
            }

            return status;
        }
        public List<KotBillDetailBO> GetKotBillDetailListByKotId(int kotId)
        {
            List<KotBillDetailBO> kotDetailList = new List<KotBillDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetKotDetailByKotId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "KotDetailInfo");
                    DataTable Table = ds.Tables["KotDetailInfo"];

                    kotDetailList = Table.AsEnumerable().Select(r => new KotBillDetailBO
                    {
                        KotDetailId = r.Field<int>("KotDetailId"),
                        KotId = r.Field<int>("KotId"),
                        ItemId = r.Field<int>("ItemId"),
                        ItemType = r.Field<string>("ItemType"),
                        ItemName = r.Field<string>("ItemName"),
                        ItemUnit = r.Field<decimal>("ItemUnit"),
                        UnitRate = r.Field<decimal>("UnitRate"),
                        Amount = r.Field<decimal>("Amount"),
                        IsDispatch = r.Field<bool>("IsDispatch")

                    }).ToList();
                }
            }
            return kotDetailList;
        }
        public Boolean UpdateKotDetailForFoodDispatchByKotId(int kotId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotDetailBillForFoodDispatchByKotId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean UpdateKotDetailForFoodDispatchByKotDetailId(string type, int kotDetailId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotDetailBillForFoodDisNUnDispatchByKotDetailId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@KotDetailId", DbType.Int32, kotDetailId);
                    dbSmartAspects.AddInParameter(command, "@Type", DbType.String, type);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        //--------------------------------New Touch Screen Methods
        public Boolean UpdateKotBillDetailPrintFlagByOrderSubmitMultipleKotId(string kotId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillDetailPrintFlagByOrderSubmitMultipleKotId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.String, kotId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<KotBillDetailBO> GetKotOrderSubmitInfoForMultipleTable(string kotIdLst, int costCenterId, string stockType, bool isReprint)
        {
            List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();
            string spName = string.Empty;

            if (isReprint)
            {
                spName = "GetKotOrderReprintForMultipleTable_SP";
            }
            else
            {
                spName = "GetKotOrderSubmitInfoForMultipleTable_SP";
            }

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand(spName))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.String, kotIdLst);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@StockType", DbType.String, stockType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                KotBillDetailBO entityBO = new KotBillDetailBO();

                                entityBO.KotDetailId = Convert.ToInt32(reader["KotDetailId"]);
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.ItemType = reader["ItemType"].ToString();
                                entityBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                entityBO.ItemName = reader["ItemName"].ToString();
                                entityBO.ItemCode = reader["ItemCode"].ToString();
                                entityBO.Category = reader["Category"].ToString();
                                entityBO.ItemUnit = Convert.ToDecimal(reader["ItemUnit"]);
                                entityBO.UnitRate = Convert.ToDecimal(reader["UnitRate"]);
                                entityBO.Amount = Convert.ToDecimal(reader["Amount"]);
                                entityBO.Remarks = reader["Remarks"].ToString();
                                entityBO.KitchenId = Convert.ToInt32(reader["KitchenId"]);
                                entityBO.PrinterName = reader["PrinterName"].ToString();
                                entityBO.PrintFlag = Convert.ToBoolean(reader["PrintFlag"]);
                                entityBO.IsChanged = Convert.ToBoolean(reader["IsChanged"]);
                                entityBO.PaxQuantity = Convert.ToInt32(reader["PaxQuantity"]);
                                entityBO.NewItemCount = Convert.ToInt32(reader["NewItemCount"]);
                                entityBO.VoidOrDeletedItemCount = Convert.ToInt32(reader["VoidOrDeletedItemCount"]);
                                entityBO.EditedOrChangedItemCount = Convert.ToInt32(reader["EditedOrChangedItemCount"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<KotBillDetailBO> GetRestaurantOrderItemByMultipleKotId(string costCenterId, string kotId, string sourceName)
        {
            List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantOrderItemByMultipleKotIdForBilling_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.String, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@KotIdList", DbType.String, kotId);
                    dbSmartAspects.AddInParameter(cmd, "@SourceName", DbType.String, sourceName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                KotBillDetailBO entityBO = new KotBillDetailBO();

                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.KotDetailId = Convert.ToInt32(reader["KotDetailId"]);


                                entityBO.StockBy = Convert.ToInt32(reader["StockBy"]);
                                entityBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                entityBO.UnitHead = reader["UnitHead"].ToString();
                                entityBO.Remarks = reader["Remarks"].ToString();
                                entityBO.BagWeight = Convert.ToInt32(reader["BagWeight"]);
                                entityBO.NoOfBag = Convert.ToInt32(reader["NoOfBag"]);

                                entityBO.ItemType = reader["ItemType"].ToString();
                                entityBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                entityBO.ItemName = reader["ItemName"].ToString();
                                entityBO.ItemCode = reader["ItemCode"].ToString();
                                entityBO.Category = reader["Category"].ToString();
                                entityBO.ItemUnit = Convert.ToDecimal(reader["ItemUnit"]);
                                entityBO.UnitRate = Convert.ToDecimal(reader["UnitRate"]);
                                entityBO.Amount = Convert.ToDecimal(reader["Amount"]);
                                entityBO.PrintFlag = Convert.ToBoolean(reader["PrintFlag"]);                               
                                entityBO.IsItemEditable = Convert.ToBoolean(reader["IsItemEditable"]);

                                entityBO.ClassificationId = Convert.ToInt32(reader["ClassificationId"]);

                                entityBO.ItemWiseDiscountType = Convert.ToString(reader["ItemWiseDiscountType"]);
                                entityBO.ItemWiseIndividualDiscount = Convert.ToDecimal(reader["ItemWiseIndividualDiscount"]);
                                entityBO.IsBillPreviewButtonEnable = Convert.ToBoolean(reader["IsBillPreviewButtonEnable"]);

                                entityBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                entityBO.CitySDCharge = Convert.ToDecimal(reader["SDCharge"]);
                                entityBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                entityBO.AdditionalCharge = Convert.ToDecimal(reader["AdditionalCharge"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<KotBillDetailBO> GetSalesOrderDetailsId(int costCenterId, int SOrderId)
        {
            List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesOrderDetailsId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@SOrderId", DbType.Int32, SOrderId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                KotBillDetailBO entityBO = new KotBillDetailBO();

                                entityBO.KotId = Convert.ToInt32(reader["SOrderId"]);
                                entityBO.KotDetailId = Convert.ToInt32(reader["DetailId"]);
                                if (reader["Quantity"] != DBNull.Value)
                                {
                                    entityBO.PaxQuantity = Convert.ToInt32(reader["Quantity"]);
                                }
                                
                                if (reader["ColorId"] != DBNull.Value)
                                {
                                    entityBO.ColorId = Convert.ToInt32(reader["ColorId"]);
                                }
                                if (reader["SizeId"] != DBNull.Value)
                                {
                                    entityBO.SizeId = Convert.ToInt32(reader["SizeId"]);
                                }
                                if (reader["StyleId"] != DBNull.Value)
                                {
                                    entityBO.StyleId = Convert.ToInt32(reader["StyleId"]);
                                }

                                entityBO.ColorName = reader["ColorName"].ToString();

                                entityBO.SizeName = reader["SizeName"].ToString();
                                
                                entityBO.StyleName = reader["StyleName"].ToString();
                                
                                if(reader["StockById"] != DBNull.Value)
                                {
                                    entityBO.StockBy = Convert.ToInt32(reader["StockById"]);
                                }

                                entityBO.Remarks = reader["Remarks"].ToString();

                                entityBO.ItemType = reader["ItemType"].ToString();
                                entityBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                entityBO.ItemName = reader["ItemName"].ToString();
                                entityBO.ItemCode = reader["ItemCode"].ToString();
                                entityBO.ItemUnit = Convert.ToDecimal(reader["ItemUnit"]);
                                entityBO.UnitRate = Convert.ToDecimal(reader["UnitRate"]);
                                entityBO.Amount = Convert.ToDecimal(reader["Amount"]);
                                if (reader["ServiceCharge"] != DBNull.Value)
                                {
                                    entityBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                }
                                if (reader["VatAmount"] != DBNull.Value)
                                {
                                    entityBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                }
                                entityBO.ItemCost = Convert.ToDecimal(reader["ItemCost"]);
                                entityBO.BagWeight = Convert.ToInt32(reader["BagWeight"]);
                                entityBO.NoOfBag = Convert.ToInt32(reader["NoOfBag"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<KotBillDetailBO> GetKotBillDetailInfoForBillNumber(string billNumber)
        {
            List<KotBillDetailBO> entityBOList = new List<KotBillDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetKotBillDetailInfoForBillNumber_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BillNumber", DbType.String, billNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                KotBillDetailBO entityBO = new KotBillDetailBO();

                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.KotDetailId = Convert.ToInt32(reader["KotDetailId"]);
                                entityBO.ItemType = reader["ItemType"].ToString();
                                entityBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                entityBO.ItemName = reader["ItemName"].ToString();
                                //entityBO.ItemCode = reader["ItemCode"].ToString();
                                entityBO.Category = reader["Category"].ToString();
                                entityBO.ItemUnit = Convert.ToDecimal(reader["ItemUnit"]);
                                entityBO.UnitRate = Convert.ToDecimal(reader["UnitRate"]);
                                entityBO.Amount = Convert.ToDecimal(reader["Amount"]);
                                entityBO.PrintFlag = Convert.ToBoolean(reader["PrintFlag"]);
                                entityBO.EmpId = Convert.ToInt32(reader["EmpId"]);
                                entityBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                entityBO.Category = reader["Category"].ToString();
                                entityBO.DeliveryStatus = reader["DeliveryStatus"].ToString();

                                //entityBO.ClassificationId = Convert.ToInt32(reader["ClassificationId"]);

                                //entityBO.ItemWiseDiscountType = Convert.ToString(reader["ItemWiseDiscountType"]);
                                //entityBO.ItemWiseIndividualDiscount = Convert.ToDecimal(reader["ItemWiseIndividualDiscount"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
    }
}
