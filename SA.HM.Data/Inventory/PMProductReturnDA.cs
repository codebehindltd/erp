using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Inventory;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.PurchaseManagment;

namespace HotelManagement.Data.PurchaseManagment
{
    public class PMProductReturnDA : BaseService
    {
        #region "Transfer Item Return"
        public List<PMProductReturnDetailsBO> GetItemForReturnFromOutConsumption(string issueNumber)
        {
            List<PMProductReturnDetailsBO> productReceive = new List<PMProductReturnDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemForReturnFromOutConsumption_SP"))
                {
                    if (!string.IsNullOrEmpty(issueNumber))
                        dbSmartAspects.AddInParameter(cmd, "@IssueNumber", DbType.String, issueNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IssueNumber", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReturnDetailsBO
                    {
                        OutId = r.Field<int>("OutId"),
                        OutDetailsId = r.Field<int>("OutDetailsId"),
                        ProductType = r.Field<string>("ProductType"),
                        StockById = r.Field<int>("StockById"),
                        ItemId = r.Field<int>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        HeadName = r.Field<string>("HeadName"),

                        OrderQuantity = r.Field<decimal>("OrderQuantity"),
                        Quantity = r.Field<decimal>("Quantity"),
                        ReturnQuantity = r.Field<decimal>("ReturnQuantity"),

                        FromCostCenterId = r.Field<int>("FromCostCenterId"),
                        FromLocationId = r.Field<int>("FromLocationId"),
                        ToCostCenterId = r.Field<int>("ToCostCenterId"),
                        IssueType = r.Field<string>("IssueType"),
                        ReturnId = 0,
                        ReturnDetailsId = 0

                    }).ToList();
                }
            }

            return productReceive;
        }

        public List<PMProductReturnBO> GetProductReturnDetailsByReturnId(long returnId)
        {
            List<PMProductReturnBO> productReceive = new List<PMProductReturnBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductReturnDetailsByReturnId_SP"))
                {
                    if (returnId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ReturnId", DbType.Int64, returnId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReturnId", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReturn");
                    DataTable Table = ds.Tables["PMProductReturn"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReturnBO 
                    {
                        ReturnId = r.Field<long>("ReturnId"),
                        ReturnQuantity = r.Field<decimal>("ReturnQuantity"),
                        ReturnDate = r.Field<DateTime>("ReturnDate"),
                        TransactionId = r.Field<long>("TransactionId"),
                        ReturnNumber = r.Field<string>("ReturnNumber"),
                        UserName = r.Field<string>("UserName"),
                        FromCostCenter = r.Field<string>("FromCostCenter"),

                        CostCenterTo = r.Field<string>("CostCenterTo"),

                        FromLocation = r.Field<string>("FromLocation"),
                        LocationTo = r.Field<string>("LocationTo"),
                        ReturnDetailsId = r.Field<long>("ReturnDetailsId"),
                        CategoryName = r.Field<string>("CategoryName"),
                        Code = r.Field<string>("Code"),
                        ItemName = r.Field<string>("ItemName"),
                        SerialNo = r.Field<string>("SerialNo"),
                        CheckedByName = r.Field<string>("CheckedByName"),
                        ApprovedByName = r.Field<string>("ApprovedByName")

                    }).ToList();
                }
            }

                return productReceive;
        }

        public List<SerialDuplicateBO> SerialAvailabilityCheck(int outId, string serialNumber, Int32 locationId)
        {

            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                try
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("AvailableSerialCheckForReturnItem_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@OutId", DbType.Int32, outId);
                        dbSmartAspects.AddInParameter(cmd, "@SerialNumber", DbType.String, serialNumber);
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                        DataTable Table = ds.Tables["PMProductReceived"];

                        serial = Table.AsEnumerable().Select(r => new SerialDuplicateBO
                        {
                            ItemName = r.Field<string>("ItemName"),
                            SerialNumber = r.Field<string>("SerialNumber")

                        }).ToList();
                    }


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return serial;
        }

        public bool SaveProductReturnInfo(PMProductReturnBO ProductReturn,
                                       List<PMProductReturnDetailsBO> ReturnItemAdded,
                                       List<PMProductReturnSerialBO> ItemSerialDetails,
                                       out Int64 returnId)
        {
            int status = 0; returnId = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("SaveProductReturn_SP"))
                        {
                            commandOut.Parameters.Clear();

                            if (!string.IsNullOrEmpty(ProductReturn.ReturnType))
                                dbSmartAspects.AddInParameter(commandOut, "@ReturnType", DbType.String, ProductReturn.ReturnType);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@ReturnType", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);

                            if (ProductReturn.FromCostCenterId != 0)
                                dbSmartAspects.AddInParameter(commandOut, "@FromCostCenterId", DbType.Int64, ProductReturn.FromCostCenterId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@FromCostCenterId", DbType.Int64, DBNull.Value);

                            if (ProductReturn.FromLocationId != 0)
                                dbSmartAspects.AddInParameter(commandOut, "@FromLocationId", DbType.Int64, ProductReturn.FromLocationId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@FromLocationId", DbType.Int64, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@Status", DbType.String, ProductReturn.Status);
                            dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, ProductReturn.Remarks);

                            dbSmartAspects.AddInParameter(commandOut, "@CreatedBy", DbType.Int64, ProductReturn.CreatedBy);

                            dbSmartAspects.AddOutParameter(commandOut, "@ReturnId", DbType.Int64, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);

                            returnId = Convert.ToInt64(commandOut.Parameters["@ReturnId"].Value);
                        }

                        if (status > 0 && ReturnItemAdded.Count > 0)
                        {
                            foreach (PMProductReturnDetailsBO pout in ReturnItemAdded)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveProductReturnDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnId", DbType.Int64, returnId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, pout.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OrderQuantity", DbType.Decimal, pout.OrderQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && ItemSerialDetails.Count > 0)
                        {
                            using (DbCommand cmdReceiveDetails = dbSmartAspects.GetStoredProcCommand("SaveProductReturnSerial_SP"))
                            {
                                foreach (PMProductReturnSerialBO rd in ItemSerialDetails)
                                {
                                    cmdReceiveDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ReturnId", DbType.Int64, returnId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@SerialNumber", DbType.String, rd.SerialNumber);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdReceiveDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && ProductReturn.ReturnType == "OutReturn")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReturnStatusForOutWiseReturn_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@ReturnId", DbType.Int64, returnId);
                                dbSmartAspects.AddInParameter(commandDetails, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
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
        public bool UpdateProductReturnInfo(PMProductReturnBO ProductReturn,
                                         List<PMProductReturnDetailsBO> ReturnItemAdded,
                                         List<PMProductReturnDetailsBO> ReturnItemEdited,
                                         List<PMProductReturnDetailsBO> ReturnItemDeleted,
                                         List<PMProductReturnSerialBO> ItemSerialDetails,
                                         List<PMProductReturnSerialBO> DeletedSerialzableProduct)
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
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("UpdateProductReturn_SP"))
                        {
                            commandOut.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandOut, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);

                            if (!string.IsNullOrEmpty(ProductReturn.ReturnType))
                                dbSmartAspects.AddInParameter(commandOut, "@ReturnType", DbType.String, ProductReturn.ReturnType);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@ReturnType", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);

                            if (ProductReturn.FromCostCenterId != 0)
                                dbSmartAspects.AddInParameter(commandOut, "@FromCostCenterId", DbType.Int32, ProductReturn.FromCostCenterId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@FromCostCenterId", DbType.Int32, DBNull.Value);

                            if (ProductReturn.FromLocationId != 0)
                                dbSmartAspects.AddInParameter(commandOut, "@FromLocationId", DbType.Int32, ProductReturn.FromLocationId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@FromLocationId", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, ProductReturn.Remarks);
                            dbSmartAspects.AddInParameter(commandOut, "@LastModifiedBy", DbType.Int32, ProductReturn.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);

                        }

                        if (status > 0 && ReturnItemAdded.Count > 0)
                        {
                            foreach (PMProductReturnDetailsBO pout in ReturnItemAdded)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveProductReturnDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, pout.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OrderQuantity", DbType.Decimal, pout.OrderQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && ReturnItemEdited.Count > 0)
                        {
                            foreach (PMProductReturnDetailsBO pout in ReturnItemEdited)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateProductReturnDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnDetailsId", DbType.Int64, pout.ReturnDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, pout.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OrderQuantity", DbType.Decimal, pout.OrderQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);
                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && ReturnItemDeleted.Count > 0)
                        {
                            using (DbCommand commandDeletePO = dbSmartAspects.GetStoredProcCommand("DeleteProductReturnDetails_SP"))
                            {
                                foreach (PMProductReturnDetailsBO pout in ReturnItemDeleted)
                                {
                                    commandDeletePO.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ReturnDetailsId", DbType.Int64, pout.ReturnDetailsId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@OutDetailsId", DbType.Int32, pout.OutDetailsId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ItemId", DbType.Int32, pout.ItemId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@Quantity", DbType.Decimal, pout.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePO, transction);
                                }
                            }
                        }

                        if (status > 0 && ItemSerialDetails.Count > 0)
                        {
                            using (DbCommand cmdReceiveDetails = dbSmartAspects.GetStoredProcCommand("SaveProductReturnSerial_SP"))
                            {
                                foreach (PMProductReturnSerialBO rd in ItemSerialDetails)
                                {
                                    cmdReceiveDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@SerialNumber", DbType.String, rd.SerialNumber);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdReceiveDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && DeletedSerialzableProduct.Count > 0)
                        {
                            using (DbCommand commandDeletePO = dbSmartAspects.GetStoredProcCommand("DeleteReturnSerialInfo_SP"))
                            {
                                foreach (PMProductReturnSerialBO rd in DeletedSerialzableProduct)
                                {
                                    commandDeletePO.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ReturnSerialId", DbType.Int64, rd.ReturnSerialId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@SerialNumber", DbType.String, rd.SerialNumber);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePO, transction);
                                }
                            }
                        }

                        if (status > 0 && ProductReturn.ReturnType == "OutReturn")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReturnStatusForOutWiseReturn_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);
                                dbSmartAspects.AddInParameter(commandDetails, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
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

        public List<PMProductReturnBO> GetProductReturnForSearch(string returnType, DateTime? fromDate, DateTime? toDate,
                                                             string returnNumber, string status,
                                                             Int32 userInfoId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<PMProductReturnBO> productReceive = new List<PMProductReturnBO>();
            totalRecords = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductReturnForSearch_SP"))
                {
                    if (returnType != "All")
                        dbSmartAspects.AddInParameter(cmd, "@ReturnType", DbType.String, returnType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReturnType", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    if (!string.IsNullOrEmpty(returnNumber))
                        dbSmartAspects.AddInParameter(cmd, "@ReturnNumber", DbType.String, returnNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReturnNumber", DbType.String, DBNull.Value);

                    if (status != "All")
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, status);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReturnBO
                    {
                        ReturnId = r.Field<Int64>("ReturnId"),
                        ReturnNumber = r.Field<string>("ReturnNumber"),
                        ReturnDate = r.Field<DateTime>("ReturnDate"),
                        ReturnType = r.Field<string>("ReturnType"),
                        TransactionId = r.Field<Int64>("TransactionId"),

                        FromCostCenterId = r.Field<Int32>("FromCostCenterId"),
                        FromLocationId = r.Field<Int32>("FromLocationId"),

                        FromCostCenter = r.Field<string>("FromCostCenter"),
                        FromLocation = r.Field<string>("FromLocation"),

                        Status = r.Field<string>("Status"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CheckedBy = r.Field<Int32>("CheckedBy"),
                        ApprovedBy = r.Field<Int32>("ApprovedBy"),

                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanDelete = r.Field<bool>("IsCanDelete"),
                        IsCanChecked = r.Field<bool>("IsCanChecked"),
                        IsCanApproved = r.Field<bool>("IsCanApproved")

                    }).ToList();

                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value.ToString());
                }
            }

            return productReceive;
        }

        public PMProductReturnBO GetItemReturnById(Int64 returnId)
        {
            PMProductReturnBO productReceive = new PMProductReturnBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemReturnById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReturnId", DbType.Int64, returnId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReturnBO
                    {
                        ReturnId = r.Field<Int64>("ReturnId"),
                        TransactionId = r.Field<Int64>("TransactionId"),
                        ReturnNumber = r.Field<string>("ReturnNumber"),
                        ReturnDate = r.Field<DateTime>("ReturnDate"),
                        ReturnType = r.Field<string>("ReturnType"),

                        FromCostCenterId = r.Field<Int32>("FromCostCenterId"),
                        FromLocationId = r.Field<Int32>("FromLocationId"),

                        FromCostCenter = r.Field<string>("FromCostCenter"),
                        FromLocation = r.Field<string>("FromLocation"),
                        ToCostCenterId = r.Field<int>("ToCostCenterId"),

                        Status = r.Field<string>("Status"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CheckedBy = r.Field<Int32>("CheckedBy"),
                        ApprovedBy = r.Field<Int32>("ApprovedBy"),
                        IssueNumber = r.Field<string>("IssueNumber"),
                        IssueType = r.Field<string>("IssueType")

                    }).FirstOrDefault();
                }
            }

            return productReceive;
        }
        public List<PMProductReturnDetailsBO> GetItemReturnDetailsForEdit(Int64 returnId, Int64 transactionId)
        {
            List<PMProductReturnDetailsBO> productReceive = new List<PMProductReturnDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemReturnDetailsForEdit_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReturnId", DbType.Int64, returnId);
                    dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.Int64, transactionId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReturnDetailsBO
                    {
                        ReturnId = r.Field<Int64>("ReturnId"),
                        ReturnDetailsId = r.Field<Int64>("ReturnDetailsId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ProductType = r.Field<string>("ProductType"),
                        ItemName = r.Field<string>("ItemName"),
                        StockById = r.Field<Int32>("StockById"),
                        HeadName = r.Field<string>("HeadName"),
                        OrderQuantity = r.Field<decimal>("OrderQuantity"),
                        Quantity = r.Field<decimal>("Quantity"),
                        ReturnQuantity = r.Field<decimal>("ReturnQuantity"),
                        OutId = r.Field<Int32>("OutId"),
                        OutDetailsId = r.Field<Int32>("OutDetailsId")

                    }).ToList();
                }
            }

            return productReceive;
        }

        public List<PMProductReturnSerialBO> GetItemSerialReturnById(Int64 returnId)
        {
            List<PMProductReturnSerialBO> productReceive = new List<PMProductReturnSerialBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemReturnSerialById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReturnId", DbType.Int32, returnId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReturnSerialBO
                    {
                        ReturnSerialId = r.Field<Int64>("ReturnSerialId"),
                        ReturnId = r.Field<Int64>("ReturnId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        SerialNumber = r.Field<string>("SerialNumber")

                    }).ToList();
                }
            }

            return productReceive;
        }

        public bool ReturnTransferItemApproval(string returnType, Int64 returnId, string approvedStatus, Int64 transactionId, int checkedOrApprovedBy)
        {
            bool retVal = false;
            int status = 0;
            string OutStatus = string.Empty;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("ReturnTransferItemApproval_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@ReturnId", DbType.Int64, returnId);
                            dbSmartAspects.AddInParameter(commandMaster, "@TransactionId", DbType.Int64, transactionId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, checkedOrApprovedBy);
                            dbSmartAspects.AddOutParameter(commandMaster, "@OutStatus", DbType.String, 50);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            OutStatus = Convert.ToString(commandMaster.Parameters["@OutStatus"].Value);
                        }

                        if (status > 0 && OutStatus == "Approved")
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateConsumptionReturnWiseStock_SP"))
                            {
                                cmdOutDetails.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnId", DbType.Int64, returnId);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@Status", DbType.String, approvedStatus);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@LastModifiedBy", DbType.Int32, checkedOrApprovedBy);

                                status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                            }
                        }

                        if (status > 0 && OutStatus == "Approved")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReturnStatusForOutWiseReturn_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@ReturnId", DbType.Int64, returnId);
                                dbSmartAspects.AddInParameter(commandDetails, "@TransactionId", DbType.Int64, transactionId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, approvedStatus);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }
            return retVal;
        }


        public bool ReturnTransferItemDelete(string returnType, Int64 returnId, string approvedStatus, Int64 transactionId, int createdBy, int lastModifiedBy)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReturnStatusForConsumptionWiseReturnDelete_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandDetails, "@ReturnId", DbType.Int64, returnId);
                            dbSmartAspects.AddInParameter(commandDetails, "@TransactionId", DbType.Int64, transactionId);
                            dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, approvedStatus);

                            status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                        }

                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("OutTransferReturnDelete_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@ReturnId", DbType.Int64, returnId);
                            dbSmartAspects.AddInParameter(commandMaster, "@TransactionId", DbType.Int64, transactionId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, createdBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }
            return retVal;
        }

        #endregion

        #region "Receives Item Return"

        public List<PMProductReceivedDetailsBO> GetItemForReturnFromReceived(int receivedId, int locationId)
        {
            List<PMProductReceivedDetailsBO> productReceive = new List<PMProductReceivedDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemForReturnFromReceived_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReceivedId", DbType.Int32, receivedId);
                    dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReceivedDetailsBO
                    {
                        ReceivedId = r.Field<Int32>("ReceivedId"),
                        ReceiveDetailsId = r.Field<Int32>("ReceiveDetailsId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ColorId = r.Field<Int32>("ColorId"),
                        SizeId = r.Field<Int32>("SizeId"),
                        StyleId = r.Field<Int32>("StyleId"),
                        StockById = r.Field<Int32>("StockById"),
                        Quantity = r.Field<decimal>("Quantity"),
                        ReturnQuantity = r.Field<decimal>("ReturnQuantity"),
                        ItemName = r.Field<string>("ItemName"),
                        StockBy = r.Field<string>("StockBy"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice"),
                        ProductType = r.Field<string>("ProductType")

                    }).ToList();
                }
            }

            return productReceive;
        }

        public List<SerialDuplicateBO> SerialAvailabilityCheckForReceiveReturn(int receivedId, string serialNumber, Int32 locationId)
        {

            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                try
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("AvailableSerialCheckForReceivedReturnItem_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReceivedId", DbType.Int32, receivedId);
                        dbSmartAspects.AddInParameter(cmd, "@SerialNumber", DbType.String, serialNumber);
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                        DataTable Table = ds.Tables["PMProductReceived"];

                        serial = Table.AsEnumerable().Select(r => new SerialDuplicateBO
                        {
                            ItemName = r.Field<string>("ItemName"),
                            SerialNumber = r.Field<string>("SerialNumber")

                        }).ToList();
                    }


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return serial;
        }
        public bool SaveReceivedProductReturnInfo(PMSupplierProductReturnBO ProductReturn,
                                      List<PMSupplierProductReturnDetailsBO> ReturnItemAdded,
                                      List<PMSupplierProductReturnSerialBO> ItemSerialDetails,
                                      out Int64 returnId)
        {
            int status = 0; returnId = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("SaveProductReceiveReturn_SP"))
                        {
                            commandOut.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandOut, "@ReceivedId", DbType.Int32, ProductReturn.ReceivedId);
                            dbSmartAspects.AddInParameter(commandOut, "@POrderId", DbType.Int32, ProductReturn.POrderId);

                            if (ProductReturn.CostCenterId != 0)
                                dbSmartAspects.AddInParameter(commandOut, "@CostCenterId", DbType.Int64, ProductReturn.CostCenterId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@CostCenterId", DbType.Int64, DBNull.Value);

                            if (ProductReturn.LocationId != 0)
                                dbSmartAspects.AddInParameter(commandOut, "@LocationId", DbType.Int64, ProductReturn.LocationId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@LocationId", DbType.Int64, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@Status", DbType.String, ProductReturn.Status);
                            dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, ProductReturn.Remarks);

                            dbSmartAspects.AddInParameter(commandOut, "@CreatedBy", DbType.Int64, ProductReturn.CreatedBy);

                            dbSmartAspects.AddOutParameter(commandOut, "@ReturnId", DbType.Int64, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);

                            returnId = Convert.ToInt64(commandOut.Parameters["@ReturnId"].Value);
                        }

                        if (status > 0 && ReturnItemAdded.Count > 0)
                        {
                            foreach (PMSupplierProductReturnDetailsBO pout in ReturnItemAdded)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveProductReceiveReturnDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnId", DbType.Int64, returnId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ReceivedId", DbType.Int64, ProductReturn.ReceivedId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, pout.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, pout.ColorId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, pout.SizeId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, pout.StyleId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OrderQuantity", DbType.Decimal, pout.OrderQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && ItemSerialDetails.Count > 0)
                        {
                            using (DbCommand cmdReceiveDetails = dbSmartAspects.GetStoredProcCommand("SaveProductReceiveReturnSerial_SP"))
                            {
                                foreach (PMSupplierProductReturnSerialBO rd in ItemSerialDetails)
                                {
                                    cmdReceiveDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ReturnId", DbType.Int64, returnId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ReceivedId", DbType.Int64, ProductReturn.ReceivedId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@SerialNumber", DbType.String, rd.SerialNumber);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdReceiveDetails, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReturnStatusForReceiveWiseReturn_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@ReturnId", DbType.Int64, returnId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ReceivedId", DbType.Int64, ProductReturn.ReceivedId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
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
        public bool UpdateReceivedProductReturnInfo(PMSupplierProductReturnBO ProductReturn,
                                         List<PMSupplierProductReturnDetailsBO> ReturnItemAdded,
                                         List<PMSupplierProductReturnDetailsBO> ReturnItemEdited,
                                         List<PMSupplierProductReturnDetailsBO> ReturnItemDeleted,
                                         List<PMSupplierProductReturnSerialBO> ItemSerialDetails,
                                         List<PMSupplierProductReturnSerialBO> DeletedSerialzableProduct)
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
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("UpdateReceiveProductReturn_SP"))
                        {
                            commandOut.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandOut, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);

                            if (ProductReturn.CostCenterId != 0)
                                dbSmartAspects.AddInParameter(commandOut, "@CostCenterId", DbType.Int64, ProductReturn.CostCenterId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@CostCenterId", DbType.Int64, DBNull.Value);

                            if (ProductReturn.LocationId != 0)
                                dbSmartAspects.AddInParameter(commandOut, "@LocationId", DbType.Int64, ProductReturn.LocationId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@LocationId", DbType.Int64, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, ProductReturn.Remarks);

                            dbSmartAspects.AddInParameter(commandOut, "@LastModifiedBy", DbType.Int32, ProductReturn.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);

                        }

                        if (status > 0 && ReturnItemAdded.Count > 0)
                        {
                            foreach (PMSupplierProductReturnDetailsBO pout in ReturnItemAdded)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveProductReceiveReturnDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ReceivedId", DbType.Int64, ProductReturn.ReceivedId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, pout.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, pout.ColorId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, pout.SizeId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, pout.StyleId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OrderQuantity", DbType.Decimal, pout.OrderQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && ReturnItemEdited.Count > 0)
                        {
                            foreach (PMSupplierProductReturnDetailsBO pout in ReturnItemEdited)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateReceiveProductReturnDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnDetailsId", DbType.Int64, pout.ReturnDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ReceivedId", DbType.Int64, ProductReturn.ReceivedId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, pout.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, pout.ColorId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, pout.SizeId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, pout.StyleId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OrderQuantity", DbType.Decimal, pout.OrderQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && ReturnItemDeleted.Count > 0)
                        {
                            using (DbCommand commandDeletePO = dbSmartAspects.GetStoredProcCommand("DeleteProductReceiveReturnDetails_SP"))
                            {
                                foreach (PMSupplierProductReturnDetailsBO pout in ReturnItemDeleted)
                                {
                                    commandDeletePO.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ReturnDetailsId", DbType.Int64, pout.ReturnDetailsId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ReceivedId", DbType.Int64, ProductReturn.ReceivedId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ReceiveDetailsId", DbType.Int32, pout.ReceiveDetailsId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ItemId", DbType.Int32, pout.ItemId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@Quantity", DbType.Decimal, pout.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePO, transction);
                                }
                            }
                        }

                        if (status > 0 && ItemSerialDetails.Count > 0)
                        {
                            using (DbCommand cmdReceiveDetails = dbSmartAspects.GetStoredProcCommand("SaveProductReceiveReturnSerial_SP"))
                            {
                                foreach (PMSupplierProductReturnSerialBO rd in ItemSerialDetails)
                                {
                                    cmdReceiveDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ReceivedId", DbType.Int64, ProductReturn.ReceivedId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@SerialNumber", DbType.String, rd.SerialNumber);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdReceiveDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && DeletedSerialzableProduct.Count > 0)
                        {
                            using (DbCommand commandDeletePO = dbSmartAspects.GetStoredProcCommand("DeleteReceiveReturnSerialInfo_SP"))
                            {
                                foreach (PMSupplierProductReturnSerialBO rd in DeletedSerialzableProduct)
                                {
                                    commandDeletePO.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ReturnSerialId", DbType.Int64, rd.ReturnSerialId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ReceivedId", DbType.Int64, ProductReturn.ReceivedId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@SerialNumber", DbType.String, rd.SerialNumber);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePO, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReturnStatusForReceiveWiseReturn_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ReceivedId", DbType.Int64, ProductReturn.ReceivedId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
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

        public List<PMSupplierProductReturnBO> GetReceivedOrderReturnForSearch(DateTime? fromDate, DateTime? toDate,
                                                            string returnNumber, string status,
                                                            Int32 userInfoId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<PMSupplierProductReturnBO> productReceive = new List<PMSupplierProductReturnBO>();
            totalRecords = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReceiveOrderReturnForSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    if (!string.IsNullOrEmpty(returnNumber))
                        dbSmartAspects.AddInParameter(cmd, "@ReturnNumber", DbType.String, returnNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReturnNumber", DbType.String, DBNull.Value);

                    if (status != "All")
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, status);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMSupplierProductReturnBO
                    {
                        ReturnId = r.Field<Int64>("ReturnId"),
                        ReturnNumber = r.Field<string>("ReturnNumber"),
                        ReturnDate = r.Field<DateTime>("ReturnDate"),
                        ReceivedId = r.Field<int>("ReceivedId"),

                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        LocationId = r.Field<Int32>("LocationId"),

                        CostCenter = r.Field<string>("CostCenter"),
                        LocationName = r.Field<string>("LocationName"),

                        Status = r.Field<string>("Status"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CheckedBy = r.Field<Int32>("CheckedBy"),
                        ApprovedBy = r.Field<Int32>("ApprovedBy"),

                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanDelete = r.Field<bool>("IsCanDelete"),
                        IsCanChecked = r.Field<bool>("IsCanChecked"),
                        IsCanApproved = r.Field<bool>("IsCanApproved")

                    }).ToList();

                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value.ToString());
                }
            }

            return productReceive;
        }



        public PMSupplierProductReturnBO GetReceiveReturnById(Int64 returnId)
        {
            PMSupplierProductReturnBO productReceive = new PMSupplierProductReturnBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReceiveItemReturnById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReturnId", DbType.Int64, returnId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMSupplierProductReturnBO
                    {
                        ReturnId = r.Field<Int64>("ReturnId"),
                        ReceivedId = r.Field<int>("ReceivedId"),
                        ReturnNumber = r.Field<string>("ReturnNumber"),
                        ReturnDate = r.Field<DateTime>("ReturnDate"),

                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        LocationId = r.Field<Int32>("LocationId"),

                        CostCenter = r.Field<string>("CostCenter"),
                        LocationName = r.Field<string>("LocationName"),

                        Status = r.Field<string>("Status"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CheckedBy = r.Field<Int32>("CheckedBy"),
                        ApprovedBy = r.Field<Int32>("ApprovedBy")

                    }).FirstOrDefault();
                }
            }

            return productReceive;
        }
        public List<PMSupplierProductReturnDetailsBO> GetReceiveItemReturnDetailsForEdit(Int64 returnId, Int64 transactionId)
        {
            List<PMSupplierProductReturnDetailsBO> productReceive = new List<PMSupplierProductReturnDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReceiveItemReturnDetailsForEdit_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReturnId", DbType.Int64, returnId);
                    dbSmartAspects.AddInParameter(cmd, "@ReceivedId", DbType.Int64, transactionId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMSupplierProductReturnDetailsBO
                    {
                        ReturnId = r.Field<Int64>("ReturnId"),
                        ReturnDetailsId = r.Field<Int64>("ReturnDetailsId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ColorId = r.Field<Int32>("ColorId"),
                        SizeId = r.Field<Int32>("SizeId"),
                        StyleId = r.Field<Int32>("StyleId"),
                        ProductType = r.Field<string>("ProductType"),
                        ItemName = r.Field<string>("ItemName"),
                        StockById = r.Field<Int32>("StockById"),
                        HeadName = r.Field<string>("HeadName"),
                        OrderQuantity = r.Field<decimal>("OrderQuantity"),
                        Quantity = r.Field<decimal>("Quantity"),
                        ReturnQuantity = r.Field<decimal?>("ReturnQuantity"),
                        ReceivedId = r.Field<Int32>("ReceivedId"),
                        ReceiveDetailsId = r.Field<Int32>("ReceiveDetailsId")

                    }).ToList();
                }
            }

            return productReceive;
        }

        public List<PMSupplierProductReturnSerialBO> GetReceiveItemSerialReturnById(Int64 returnId)
        {
            List<PMSupplierProductReturnSerialBO> productReceive = new List<PMSupplierProductReturnSerialBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReceiveItemReturnSerialById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReturnId", DbType.Int32, returnId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMSupplierProductReturnSerialBO
                    {
                        ReturnSerialId = r.Field<Int64>("ReturnSerialId"),
                        ReturnId = r.Field<Int64>("ReturnId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        SerialNumber = r.Field<string>("SerialNumber")

                    }).ToList();
                }
            }

            return productReceive;
        }

        public bool ReceiveReturnApproval(Int64 returnId, string approvedStatus, int receivedId, int checkedOrApprovedBy)
        {
            bool retVal = false;
            int status = 0;
            string OutStatus = string.Empty;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("ReturnReceiveTransferItemApproval_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@ReturnId", DbType.Int64, returnId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReceivedId", DbType.Int32, receivedId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, checkedOrApprovedBy);
                            dbSmartAspects.AddOutParameter(commandMaster, "@OutStatus", DbType.String, 50);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                            OutStatus = Convert.ToString(commandMaster.Parameters["@OutStatus"].Value);
                        }

                        if (status > 0 && OutStatus == "Approved")
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateReceivedItemReturnWiseStock_SP"))
                            {
                                cmdOutDetails.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnId", DbType.Int64, returnId);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@Status", DbType.String, approvedStatus);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@LastModifiedBy", DbType.Int32, checkedOrApprovedBy);

                                status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                            }
                        }

                        if (status > 0 && OutStatus == "Approved")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReturnStatusForReceiveWiseReturn_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@ReturnId", DbType.Int64, returnId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ReceivedId", DbType.Int32, receivedId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }
            return retVal;
        }

        public bool ReceiveReturnDelete(Int64 returnId, string approvedStatus, Int32 receivedId, int createdBy, int lastModifiedBy)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReturnStatusForReceiveWiseReturnDelete_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandDetails, "@ReturnId", DbType.Int64, returnId);
                            dbSmartAspects.AddInParameter(commandDetails, "@ReceivedId", DbType.Int32, receivedId);
                            dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, approvedStatus);

                            status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                        }

                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("ReceiveItemReturnDelete_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@ReturnId", DbType.Int64, returnId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReceivedId", DbType.Int32, receivedId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, createdBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }
            return retVal;
        }


        #endregion
        public List<PMSupplierProductReturnDetailsBO> GetPurchaseReturnForChalanByReturnId(long returnId) 
        {
            List<PMSupplierProductReturnDetailsBO> proRet = new List<PMSupplierProductReturnDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPurchaseReturnForChalanByReturnId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReturnId", DbType.Int64, returnId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader !=null)
                        {
                            while (reader.Read())
                            {
                                PMSupplierProductReturnDetailsBO product = new PMSupplierProductReturnDetailsBO();
                                product.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                                product.SupplierName = Convert.ToString(reader["SupplierName"]);
                                product.ItemId = Convert.ToInt32(reader["ItemId"]);
                                product.ItemName = Convert.ToString(reader["ItemName"]);
                                product.MessureUnit = Convert.ToString(reader["MessureUnit"]);
                                product.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                product.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                product.ReturnAmount = Convert.ToDecimal(reader["ReturnAmount"]);
                                product.ReturnDate = Convert.ToString(reader["ReturnDate"]);                             
                                product.ReceivedByName = Convert.ToString(reader["ReceivedByName"]);                     
                                product.ReturnByName = Convert.ToString(reader["ReturnByName"]);   
                                product.ReturnNumber = Convert.ToString(reader["ReturnNumber"]);   
                                product.CheckedByName = Convert.ToString(reader["CheckedByName"]);   
                                product.ApprovedByName = Convert.ToString(reader["ApprovedByName"]);   
                                product.Phone = Convert.ToString(reader["Phone"]);
                                product.Email = Convert.ToString(reader["Email"]);
                                product.Remarks = Convert.ToString(reader["Reason"]);
                                product.Serial = Convert.ToString(reader["Serial"]);
                                
                                proRet.Add(product);
                            }
                        }
                    }
                }
            }

            return proRet;
        }
        public List<PMSupplierProductReturnDetailsBO> GetPurchaseReturnForReportBySearchCriteria(string fromDate, string toDate, string returnNumber, int supplierId)
        {
            List<PMSupplierProductReturnDetailsBO> proRet = new List<PMSupplierProductReturnDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPurchaseReturnForReportBySearchCriteria_SP"))
                {
                    if (!string.IsNullOrEmpty(fromDate))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));
                    }
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (!string.IsNullOrEmpty(toDate))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));
                    }
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    

                    if (!string.IsNullOrEmpty(returnNumber))
                        dbSmartAspects.AddInParameter(cmd, "@ReturnNumber", DbType.String, returnNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReturnNumber", DbType.String, DBNull.Value);

                    if (supplierId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMSupplierProductReturnDetailsBO product = new PMSupplierProductReturnDetailsBO
                                {
                                    SupplierId = Convert.ToInt32(reader["SupplierId"]),
                                    SupplierName = Convert.ToString(reader["SupplierName"]),
                                    ItemId = Convert.ToInt32(reader["ItemId"]),
                                    ItemName = Convert.ToString(reader["ItemName"]),
                                    Quantity = Convert.ToDecimal(reader["Quantity"]),
                                    MessureUnit = Convert.ToString(reader["MessureUnit"]),
                                    //PODate = Convert.ToDateTime(reader["ReceivedDate"]),
                                    //PONumber = Convert.ToString(reader["PONumber"]),
                                    ReceivedByName = Convert.ToString(reader["ReceivedByName"]),
                                    ReceiveNumber = Convert.ToString(reader["ReceiveNumber"]),
                                    ReceivedDate = Convert.ToString(reader["ReceivedDate"]),
                                    ReturnNumber = Convert.ToString(reader["ReturnNumber"]),
                                    ReturnDate = Convert.ToString(reader["ReturnDate"]),
                                    ReturnByName = Convert.ToString(reader["ReturnByName"]),
                                    
                                    
                                    PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]),
                                    ReturnAmount = Convert.ToDecimal(reader["ReturnAmount"])
                                };
                                proRet.Add(product);
                            }
                        }
                    }
                }
            }

            return proRet;
        }
        public List<PMProductReturnDetailsBO> GetItemForSalesReturn(string billNumber)
        {
            List<PMProductReturnDetailsBO> productReceive = new List<PMProductReturnDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemForSalesReturn_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BillNumber", DbType.String, billNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMProductReturnDetailsBO bank = new PMProductReturnDetailsBO();
                                bank.BillId = Convert.ToInt64(reader["BillId"]);
                                bank.KotDetailId = Convert.ToInt64(reader["KotDetailId"]);
                                bank.ProductType = reader["ProductType"].ToString();
                                bank.StockById = Convert.ToInt32(reader["StockById"]);
                                bank.ItemId = Convert.ToInt32(reader["ItemId"]);
                                bank.ItemName = reader["ItemName"].ToString();
                                bank.StockBy = reader["StockBy"].ToString();
                                bank.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                bank.CostCenterIdFrom = Convert.ToInt32(reader["CostCenterIdFrom"]);
                                bank.CostCenter = reader["CostCenter"].ToString();
                                bank.LocationIdFrom = Convert.ToInt32(reader["LocationIdFrom"]);

                                bank.ReturnId = 0;
                                bank.ReturnDetailsId = 0;
                                productReceive.Add(bank);
                            }
                        }
                    }
                }
            }
            return productReceive;
        }
        public bool DeletePMSalesReturn(Int64 returnId)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("DeletePMSalesReturn_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@ReturnId", DbType.Int64, returnId);
                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        public bool SaveProductSalesReturn(PMProductReturnBO ProductReturn,
                                       List<PMProductReturnDetailsBO> ReturnItemAdded,
                                       List<PMProductReturnSerialBO> ItemSerialDetails,
                                       out Int64 returnId)
        {
            int status = 0; returnId = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    {
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("SaveProductSalesReturn_SP"))
                        {
                            commandOut.Parameters.Clear();

                            if (!string.IsNullOrEmpty(ProductReturn.ReturnType))
                                dbSmartAspects.AddInParameter(commandOut, "@ReturnType", DbType.String, ProductReturn.ReturnType);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@ReturnType", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);

                            if (ProductReturn.FromCostCenterId != 0)
                                dbSmartAspects.AddInParameter(commandOut, "@FromCostCenterId", DbType.Int64, ProductReturn.FromCostCenterId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@FromCostCenterId", DbType.Int64, DBNull.Value);

                            if (ProductReturn.FromLocationId != 0)
                                dbSmartAspects.AddInParameter(commandOut, "@FromLocationId", DbType.Int64, ProductReturn.FromLocationId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@FromLocationId", DbType.Int64, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@Status", DbType.String, ProductReturn.Status);
                            dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, ProductReturn.Remarks);

                            dbSmartAspects.AddInParameter(commandOut, "@CreatedBy", DbType.Int64, ProductReturn.CreatedBy);

                            dbSmartAspects.AddOutParameter(commandOut, "@ReturnId", DbType.Int64, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);

                            returnId = Convert.ToInt64(commandOut.Parameters["@ReturnId"].Value);
                        }

                        if (status > 0 && ReturnItemAdded.Count > 0)
                        {
                            foreach (PMProductReturnDetailsBO pout in ReturnItemAdded)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveProductSalesReturnDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnId", DbType.Int64, returnId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@KotDetailId", DbType.Int32, pout.OutDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, pout.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OrderQuantity", DbType.Decimal, pout.OrderQuantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);

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
                }
            }

            return retVal;
        }
        public List<PMProductReturnBO> GetProductSalesReturnForSearch(string returnType, DateTime? fromDate, DateTime? toDate,
                                                             string returnNumber, string status,
                                                             Int32 userInfoId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<PMProductReturnBO> productReceive = new List<PMProductReturnBO>();
            totalRecords = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductSalesReturnForSearch_SP"))
                {
                    if (returnType != "All")
                        dbSmartAspects.AddInParameter(cmd, "@ReturnType", DbType.String, returnType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReturnType", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    if (!string.IsNullOrEmpty(returnNumber))
                        dbSmartAspects.AddInParameter(cmd, "@ReturnNumber", DbType.String, returnNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReturnNumber", DbType.String, DBNull.Value);

                    if (status != "All")
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, status);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReturnBO
                    {
                        ReturnId = r.Field<Int64>("ReturnId"),
                        ReturnNumber = r.Field<string>("ReturnNumber"),
                        ReturnDate = r.Field<DateTime>("ReturnDate"),
                        ReturnDateString = r.Field<string>("ReturnDateString"),
                        ReturnType = r.Field<string>("ReturnType"),
                        TransactionId = r.Field<Int64>("TransactionId"),

                        FromCostCenterId = r.Field<Int32>("FromCostCenterId"),
                        FromLocationId = r.Field<Int32>("FromLocationId"),

                        FromCostCenter = r.Field<string>("FromCostCenter"),
                        FromLocation = r.Field<string>("FromLocation"),

                        Status = r.Field<string>("Status"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CheckedBy = r.Field<Int32>("CheckedBy"),
                        ApprovedBy = r.Field<Int32>("ApprovedBy"),

                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanDelete = r.Field<bool>("IsCanDelete"),
                        IsCanChecked = r.Field<bool>("IsCanChecked"),
                        IsCanApproved = r.Field<bool>("IsCanApproved")

                    }).ToList();

                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value.ToString());
                }
            }

            return productReceive;
        }
        public bool UpdateProductSalesReturnInfo(PMProductReturnBO ProductReturn,
                                         List<PMProductReturnDetailsBO> ReturnItemAdded,
                                         List<PMProductReturnDetailsBO> ReturnItemEdited,
                                         List<PMProductReturnDetailsBO> ReturnItemDeleted,
                                         List<PMProductReturnSerialBO> ItemSerialDetails,
                                         List<PMProductReturnSerialBO> DeletedSerialzableProduct)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    {
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("UpdateProductSalesReturn_SP"))
                        {
                            commandOut.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandOut, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);

                            if (!string.IsNullOrEmpty(ProductReturn.ReturnType))
                                dbSmartAspects.AddInParameter(commandOut, "@ReturnType", DbType.String, ProductReturn.ReturnType);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@ReturnType", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);

                            if (ProductReturn.FromCostCenterId != 0)
                                dbSmartAspects.AddInParameter(commandOut, "@FromCostCenterId", DbType.Int32, ProductReturn.FromCostCenterId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@FromCostCenterId", DbType.Int32, DBNull.Value);

                            if (ProductReturn.FromLocationId != 0)
                                dbSmartAspects.AddInParameter(commandOut, "@FromLocationId", DbType.Int32, ProductReturn.FromLocationId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@FromLocationId", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, ProductReturn.Remarks);
                            dbSmartAspects.AddInParameter(commandOut, "@LastModifiedBy", DbType.Int32, ProductReturn.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);

                        }

                        if (status > 0 && ReturnItemAdded.Count > 0)
                        {
                            foreach (PMProductReturnDetailsBO pout in ReturnItemAdded)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveProductReturnDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    //dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);
                                    //dbSmartAspects.AddInParameter(cmdOutDetails, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);
                                    //dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    //dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, pout.ItemId);
                                    //dbSmartAspects.AddInParameter(cmdOutDetails, "@OrderQuantity", DbType.Decimal, pout.OrderQuantity);
                                    //dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && ReturnItemEdited.Count > 0)
                        {
                            foreach (PMProductReturnDetailsBO pout in ReturnItemEdited)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateProductReturnDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    //dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnDetailsId", DbType.Int64, pout.ReturnDetailsId);
                                    //dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);
                                    //dbSmartAspects.AddInParameter(cmdOutDetails, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);
                                    //dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    //dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, pout.ItemId);
                                    //dbSmartAspects.AddInParameter(cmdOutDetails, "@OrderQuantity", DbType.Decimal, pout.OrderQuantity);
                                    //dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);
                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && ReturnItemDeleted.Count > 0)
                        {
                            using (DbCommand commandDeletePO = dbSmartAspects.GetStoredProcCommand("DeleteProductReturnDetails_SP"))
                            {
                                foreach (PMProductReturnDetailsBO pout in ReturnItemDeleted)
                                {
                                    commandDeletePO.Parameters.Clear();

                                    //dbSmartAspects.AddInParameter(commandDeletePO, "@ReturnDetailsId", DbType.Int64, pout.ReturnDetailsId);
                                    //dbSmartAspects.AddInParameter(commandDeletePO, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);
                                    //dbSmartAspects.AddInParameter(commandDeletePO, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);
                                    //dbSmartAspects.AddInParameter(commandDeletePO, "@OutDetailsId", DbType.Int32, pout.OutDetailsId);
                                    //dbSmartAspects.AddInParameter(commandDeletePO, "@ItemId", DbType.Int32, pout.ItemId);
                                    //dbSmartAspects.AddInParameter(commandDeletePO, "@Quantity", DbType.Decimal, pout.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePO, transction);
                                }
                            }
                        }

                        if (status > 0 && ProductReturn.ReturnType == "OutReturn")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReturnStatusForOutWiseReturn_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@ReturnId", DbType.Int64, ProductReturn.ReturnId);
                                dbSmartAspects.AddInParameter(commandDetails, "@TransactionId", DbType.Int64, ProductReturn.TransactionId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
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
                }
            }

            return retVal;
        }
        public bool SalesReturnApproval(string returnType, Int64 returnId, string approvedStatus, Int64 transactionId, int checkedOrApprovedBy)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SalesReturnApproval_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@ReturnId", DbType.Int64, returnId);
                            dbSmartAspects.AddInParameter(commandMaster, "@TransactionId", DbType.Int64, transactionId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, checkedOrApprovedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
    }
}
