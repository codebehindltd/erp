using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.RetailPOS;
using HotelManagement.Entity.SalesAndMarketing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class SalesTransferDA : BaseService
    {
        public List<SMSalesTransferBO> GetSalesTransfer(int QuotationId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            totalRecords = 0;
            List<SMSalesTransferBO> SalesTransferList = new List<SMSalesTransferBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesTransferForPaging_SP"))
                    {

                        if (QuotationId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int32, QuotationId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMSalesTransferBO SalesTransfer = new SMSalesTransferBO();

                                    SalesTransfer.SalesTransferId = Convert.ToInt64(reader["SalesTransferId"]);
                                    if (reader["DealId"] != DBNull.Value)
                                    {
                                        SalesTransfer.DealId = Convert.ToInt64(reader["DealId"]);
                                        SalesTransfer.DealName = (reader["DealName"].ToString());
                                    }
                                    SalesTransfer.QuotationId = Convert.ToInt32(reader["QuotationId"]);
                                    SalesTransfer.QuotationNo = reader["QuotationNo"].ToString();
                                    SalesTransfer.IsApproved = Convert.ToBoolean(reader["IsApproved"]);
                                    SalesTransfer.TransferNumber = reader["TransferNumber"].ToString();
                                    SalesTransferList.Add(SalesTransfer);
                                }
                            }
                        }
                        totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SalesTransferList;
        }
        public List<SMQuotationDetailsBO> GetQuotationDetailsBySalesTransferId(Int64 SalesTransferId)
        {
            List<SMQuotationDetailsBO> quotation = new List<SMQuotationDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetQuotationItemDetailsBySalesTransferId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SalesTransferId", DbType.Int64, SalesTransferId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMQuotationDetailsBO q = new SMQuotationDetailsBO();

                                q.QuotationDetailsId = Convert.ToInt64(reader["QuotationDetailsId"]);

                                q.ItemId = Convert.ToInt32(reader["ItemId"]);
                                q.StockBy = Convert.ToInt32(reader["StockBy"]);
                                q.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                q.SalesQuantity = Convert.ToDecimal(reader["SalesQuantity"]);
                                q.ItemName = reader["ItemName"].ToString();
                                q.HeadName = reader["HeadName"].ToString();
                                q.SalesTransferDetailId = Convert.ToInt64(reader["SalesTransferDetailId"]);
                                q.RemainingDeliveryQuantity = Convert.ToDecimal(reader["RemainingDeliveryQuantity"]);
                                q.ProductType = reader["ProductType"].ToString();
                                q.StockQuantity = Convert.ToDecimal(reader["StockQuantity"]);
                                q.TransferedQuantity = Convert.ToDecimal(reader["TransferedQuantity"]);
                                quotation.Add(q);
                            }
                        }
                    }
                }
            }
            return quotation;
        }
        public List<SMQuotationBO> GetAllQuotation()
        {
            List<SMQuotationBO> QuotationList = new List<SMQuotationBO>();
            string query = string.Format("SELECT QuotationId,QuotationNo FROM SMQuotation WHERE IsAccepted=1");

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "SalesCall");
                        DataTable Table = ds.Tables["SalesCall"];

                        QuotationList = Table.AsEnumerable().Select(r => new SMQuotationBO
                        {
                            QuotationId = r.Field<long>("QuotationId"),
                            QuotationNo = r.Field<string>("QuotationNo")

                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return QuotationList;

        }
        public List<SMQuotationBO> GetQuotationForItemOut()
        {
            List<SMQuotationBO> QuotationList = new List<SMQuotationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                try
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetQuotation_SP"))
                    {

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMQuotationBO q = new SMQuotationBO();
                                    q.QuotationId = Convert.ToInt64(reader["QuotationId"]);
                                    q.QuotationNo = reader["QuotationNo"].ToString();

                                    QuotationList.Add(q);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return QuotationList;
        }
        public List<QuotationViewDetailsBO> GetQuotationDetailsByQuotationId(long quotationId, int costCenterId, int locationId)
        {
            List<QuotationViewDetailsBO> QuotationDetailsList = new List<QuotationViewDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                try
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetQuotationDetailsByQuotationId_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotationId);
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    QuotationViewDetailsBO q = new QuotationViewDetailsBO();
                                    q.QuotationDetailsId = Convert.ToInt64(reader["QuotationDetailsId"]);
                                    q.ItemName = reader["ItemName"].ToString();
                                    q.ItemId = Convert.ToInt32(reader["ItemId"]);
                                    q.ItemType = reader["ItemType"].ToString();
                                    q.ProductType = reader["ProductType"].ToString();
                                    q.HeadName = reader["HeadName"].ToString();
                                    q.StockBy = Convert.ToInt32(reader["StockBy"]);
                                    q.Quantity = Convert.ToInt32(reader["Quantity"]);
                                    q.StockQuantity = Convert.ToInt32(reader["StockQuantity"]);
                                    q.RemainingDeliveryQuantity = Convert.ToDecimal(reader["RemainingDeliveryQuantity"]);
                                    q.DeliveredQuantity = Convert.ToDecimal(reader["DeliveredQuantity"]);
                                    q.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                    //q.SalesTransferDetailId = Convert.ToInt64(reader["SalesTransferDetailId"]);
                                    QuotationDetailsList.Add(q);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return QuotationDetailsList;
        }

        public List<BillingViewDetailsBO> GetBillingDetailsByBillingId(long BillingId, int costCenterId, int locationId)
        {
            List<BillingViewDetailsBO> BillingViewDetailsList = new List<BillingViewDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                try
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBillingDetailsByBillingId_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@BillingId", DbType.Int64, BillingId);
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    BillingViewDetailsBO q = new BillingViewDetailsBO();
                                    q.KotDetailId = Convert.ToInt64(reader["KotDetailId"]);
                                    q.ItemName = reader["ItemName"].ToString();
                                    q.ItemId = Convert.ToInt32(reader["ItemId"]);
                                    q.ItemType = reader["ItemType"].ToString();
                                    q.ProductType = reader["ProductType"].ToString();
                                    q.HeadName = reader["HeadName"].ToString();
                                    q.StockBy = Convert.ToInt32(reader["StockBy"]);
                                    q.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                    q.StockQuantity = Convert.ToInt32(reader["StockQuantity"]);
                                    q.RemainingDeliveryQuantity = Convert.ToDecimal(reader["RemainingDeliveryQuantity"]);
                                    q.DeliveredQuantity = Convert.ToDecimal(reader["DeliveredQuantity"]);
                                    q.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                    //q.SalesTransferDetailId = Convert.ToInt64(reader["SalesTransferDetailId"]);
                                    BillingViewDetailsList.Add(q);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return BillingViewDetailsList;
        }

        public long CheckIfQuotationIsTransfered(Int64 QuotationId)
        {
            long SalesTransferId = 0;

            string query = string.Format("SELECT TOP 1 ISNULL(st.SalesTransferId, 0) as SalesTransferId  FROM SMSalesTransfer st where st.QuotationId={0} ORDER BY st.SalesTransferId DESC", QuotationId);

            try
            {
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
                                    if (reader["SalesTransferId"] == DBNull.Value)
                                    {
                                        SalesTransferId = 0;
                                    }
                                    else
                                    {
                                        SalesTransferId = Convert.ToInt64(reader["SalesTransferId"]);
                                    }
                                }
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SalesTransferId;

        }
        public bool SaveSalesTransfer(SMSalesTransferBO salesTransferBO, List<SMSalesTransferDetailsBO> SMSalesTransferDetailsBO, List<SMSalesTransferDetailsBO> TransferItemForEdit,
            List<SMSalesTransferDetailsBO> TransferItemDeleted, List<SMSalesItemSerialTransferBO> SMSalesItemSerialTransferBO, List<SMSalesItemSerialTransferBO> DeletedSerialzableProduct, out long transferOutId, out int transferOuDetailstId)
        {
            bool status = false;
            transferOutId = 0;
            transferOuDetailstId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if (salesTransferBO.SalesTransferId == 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveSalesTransfer_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@SalesTransferId", DbType.Int64, salesTransferBO.SalesTransferId);
                                if (salesTransferBO.DealId != 0 || salesTransferBO.DealId != null)
                                {
                                    dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, salesTransferBO.DealId);
                                }
                                else
                                {
                                    dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, DBNull.Value);
                                }
                                dbSmartAspects.AddInParameter(command, "@QuotationId", DbType.Int64, salesTransferBO.QuotationId);
                                dbSmartAspects.AddInParameter(command, "@CostCenterID", DbType.Int64, salesTransferBO.CostCenterId);
                                dbSmartAspects.AddInParameter(command, "@LocationId", DbType.Int64, salesTransferBO.LocationID);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, salesTransferBO.CreatedBy);

                                dbSmartAspects.AddOutParameter(command, "@TransferOutId", DbType.Int64, sizeof(Int64));


                                status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;

                                transferOutId = Convert.ToInt64(command.Parameters["@TransferOutId"].Value);
                            }
                        }
                        if (SMSalesTransferDetailsBO.Count > 0)
                        {
                            using (DbCommand cmdSalesDetails = dbSmartAspects.GetStoredProcCommand("SaveSalesTransferDetails_SP"))
                            {
                                foreach (SMSalesTransferDetailsBO rd in SMSalesTransferDetailsBO)
                                {
                                    cmdSalesDetails.Parameters.Clear();
                                    if (salesTransferBO.SalesTransferId == 0)
                                    {
                                        dbSmartAspects.AddInParameter(cmdSalesDetails, "@SalesTransferId", DbType.Int64, transferOutId);
                                    }
                                    else
                                    {
                                        dbSmartAspects.AddInParameter(cmdSalesDetails, "@SalesTransferId", DbType.Int64, salesTransferBO.SalesTransferId);

                                    }
                                    dbSmartAspects.AddInParameter(cmdSalesDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdSalesDetails, "@Quantity", DbType.Int32, rd.Quantity);
                                    dbSmartAspects.AddInParameter(cmdSalesDetails, "@StockById", DbType.Int32, rd.StockById);
                                    dbSmartAspects.AddOutParameter(cmdSalesDetails, "@SaleDetailsId", DbType.Int32, sizeof(Int32));
                                    status = dbSmartAspects.ExecuteNonQuery(cmdSalesDetails, transction) > 0 ? true : false;

                                    transferOuDetailstId = Convert.ToInt32(cmdSalesDetails.Parameters["@SaleDetailsId"].Value);
                                    //rd.SalesTransferDetailId = transferOuDetailstId;
                                }
                            }
                        }
                        if (TransferItemDeleted.Count > 0)
                        {
                            using (DbCommand cmdSalesDetails = dbSmartAspects.GetStoredProcCommand("DeleteSalesTransferDetailsBySalesTransferId_SP"))
                            {
                                foreach (SMSalesTransferDetailsBO rd in TransferItemDeleted)
                                {
                                    cmdSalesDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdSalesDetails, "@SalesTransferId", DbType.Int64, salesTransferBO.SalesTransferId);
                                    dbSmartAspects.AddInParameter(cmdSalesDetails, "@SalesTransferDetailId", DbType.Int64, rd.SalesTransferDetailId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdSalesDetails, transction) > 0 ? true : false;
                                    //rd.SalesTransferDetailId = transferOuDetailstId;
                                }
                            }
                        }
                        if (SMSalesItemSerialTransferBO.Count > 0)
                        {
                            using (DbCommand cmdSalesDetails = dbSmartAspects.GetStoredProcCommand("SaveSalesSerialItemDetails_SP"))
                            {
                                foreach (SMSalesItemSerialTransferBO rd in SMSalesItemSerialTransferBO)
                                {
                                    cmdSalesDetails.Parameters.Clear();
                                    if (salesTransferBO.SalesTransferId == 0)
                                    {
                                        dbSmartAspects.AddInParameter(cmdSalesDetails, "@SalesTransferId", DbType.Int64, transferOutId);
                                    }
                                    else
                                    {
                                        dbSmartAspects.AddInParameter(cmdSalesDetails, "@SalesTransferId", DbType.Int64, salesTransferBO.SalesTransferId);
                                    }

                                    dbSmartAspects.AddInParameter(cmdSalesDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdSalesDetails, "@SerialNumber", DbType.String, rd.SerialNumber);


                                    status = dbSmartAspects.ExecuteNonQuery(cmdSalesDetails, transction) > 0 ? true : false;

                                    //rd.SalesTransferDetailId = transferOuDetailstId;
                                }
                            }
                        }
                        if (salesTransferBO.SalesTransferId > 0 && TransferItemForEdit.Count > 0)
                        {
                            using (DbCommand cmdSalesDetails = dbSmartAspects.GetStoredProcCommand("UpdateSalesTransferDetails_SP"))
                            {
                                foreach (SMSalesTransferDetailsBO rd in TransferItemForEdit)
                                {
                                    cmdSalesDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(cmdSalesDetails, "@SalesTransferId", DbType.Int64, salesTransferBO.SalesTransferId);
                                    dbSmartAspects.AddInParameter(cmdSalesDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdSalesDetails, "@Quantity", DbType.Int32, rd.Quantity);
                                    dbSmartAspects.AddOutParameter(cmdSalesDetails, "@SaleDetailsId", DbType.Int32, sizeof(Int32));
                                    status = dbSmartAspects.ExecuteNonQuery(cmdSalesDetails, transction) > 0 ? true : false;

                                    transferOuDetailstId = Convert.ToInt32(cmdSalesDetails.Parameters["@SaleDetailsId"].Value);
                                }
                            }
                        }
                        if (DeletedSerialzableProduct.Count > 0)
                        {
                            using (DbCommand commandDeletePO = dbSmartAspects.GetStoredProcCommand("DeleteSalesSerialItemInfo_SP"))
                            {
                                foreach (SMSalesItemSerialTransferBO serial in DeletedSerialzableProduct)
                                {
                                    commandDeletePO.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeletePO, "@SalesItemSerialTransferId", DbType.Int32, serial.SalesItemSerialTransferId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@SalesTransferId", DbType.Int32, serial.SalesTransferId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@SerialNumber", DbType.String, serial.SerialNumber);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ItemId", DbType.Int32, serial.ItemId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePO, transction) > 0 ? true : false;
                                }
                            }
                        }
                        if (status)
                            transction.Commit();
                        else
                            transction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }

                }
            }
            return status;
        }
        public SMSalesTransferBO GetSalesItemTransferInfoById(long SalesTransferId)
        {
            SMSalesTransferBO salesTransferBO = new SMSalesTransferBO();
            string query = string.Format("SELECT * from SMSalesTransfer WHERE SalesTransferId={0}", SalesTransferId);

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "SalesCall");
                        DataTable Table = ds.Tables["SalesCall"];

                        salesTransferBO = Table.AsEnumerable().Select(r => new SMSalesTransferBO
                        {
                            TransferNumber = r.Field<string>("TransferNumber"),
                            CostCenterId = r.Field<Int32>("CostCenterId"),
                            LocationID = r.Field<int>("LocationId"),
                            IsApproved = r.Field<bool>("IsApproved"),
                            QuotationId = r.Field<Int64>("QuotationId")

                        }).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return salesTransferBO;
        }
        public SMSalesTransferDetailsInvoiceViewBO GetSalesTransferDetailsForInvoice(long transferId)
        {
            SMSalesTransferDetailsInvoiceViewBO salesTransfer = new SMSalesTransferDetailsInvoiceViewBO();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesTransferDetailsForInvoice_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SalesTransferId", DbType.Int64, transferId);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "SalesTransfer");

                        salesTransfer.TransferNCompanyInfo = ds.Tables[0].AsEnumerable().Select(r => new SMSalesTransferReportViewBO()
                        {
                            TransferNumber = r.Field<string>("TransferNumber"),
                            DealName = r.Field<string>("DealName"),
                            QuotationNo = r.Field<string>("QuotationNo"),
                            Remarks = r.Field<string>("Remarks"),
                            ApprovedByName = r.Field<string>("ApprovedByName"),
                            CompanyAddress = r.Field<string>("CompanyAddress"),
                            CompanyName = r.Field<string>("CompanyName"),
                            ContactNumber = r.Field<string>("ContactNumber")

                        }).ToList();

                        salesTransfer.Items = ds.Tables[1].AsEnumerable().Select(r => new SMSalesTransferDetailsBO()
                        {
                            ItemName = r.Field<string>("ItemName"),
                            AverageCost = r.Field<decimal?>("AverageCost"),
                            Quantity = r.Field<decimal>("Quantity"),
                            StockBy = r.Field<string>("StockBy")
                        }).ToList();

                        salesTransfer.Serials = ds.Tables[2].AsEnumerable().Select(r => new SMSalesItemSerialTransferBO()
                        {
                            ItemName = r.Field<string>("ItemName"),
                            SerialNumber = r.Field<string>("SerialNumber")
                        }).ToList();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return salesTransfer;
        }
        public bool SalesItemApproval(int SalesTransferId, string approvedStatus, string Remarks, int checkedOrApprovedBy)
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
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SalesItemTransferApproval_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@SalesTransferId", DbType.Int32, SalesTransferId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, checkedOrApprovedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, Remarks);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        }

                        if (status > 0 && approvedStatus == "Approved")
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateSalesItemTransferNItemStock_SP"))
                            {
                                cmdOutDetails.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmdOutDetails, "@SalesTransferId", DbType.Int32, SalesTransferId);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@Status", DbType.String, approvedStatus);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@LastModifiedBy", DbType.Int32, checkedOrApprovedBy);

                                status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
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
        public List<SMSalesItemSerialTransferBO> GetSalesItemSerialTransferBySalesTransferId(Int64 SalesTransferId)
        {
            List<SMSalesItemSerialTransferBO> serialList = new List<SMSalesItemSerialTransferBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesItemSerialTransferBySalesTransferId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SalesTransferId", DbType.Int64, SalesTransferId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMSalesItemSerialTransferBO serial = new SMSalesItemSerialTransferBO();

                                serial.SalesItemSerialTransferId = Convert.ToInt64(reader["SalesItemSerialTransferId"]);
                                serial.SalesTransferId = Convert.ToInt64(reader["SalesTransferId"]);
                                serial.ItemId = Convert.ToInt32(reader["ItemId"]);
                                serial.SerialNumber = reader["SerialNumber"].ToString();

                                serialList.Add(serial);
                            }
                        }
                    }
                }
            }
            return serialList;
        }
        public bool DeleteSalesItemTransfer(long id)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                try
                {
                    using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("DeleteSalesItemTransfer_SP"))
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@SalesTransferId", DbType.Int32, id);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster) > 0 ? true : false; ;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return status;
        }
        public SalesNQuotationViewBO GetSalesNQuotationViewBOForApprove(long transferId)
        {
            SalesNQuotationViewBO SalesNQuotationView = new SalesNQuotationViewBO();
            List<SMQuotationDetailsBO> detailsBOList = new List<SMQuotationDetailsBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetQuotationForApprove_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SalesTransferId", DbType.Int64, transferId);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SalesNQuotationView.Company = reader["Company"].ToString();
                                    SalesNQuotationView.QuotationNo = reader["QuotationNo"].ToString();
                                    SalesNQuotationView.TransferNumber = reader["TransferNumber"].ToString();
                                    SalesNQuotationView.DealName = reader["DealName"].ToString();
                                }
                            }
                        }

                    }
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetQuotationDetailsForApprove_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SalesTransferId", DbType.Int64, transferId);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMQuotationDetailsBO detailsBO = new SMQuotationDetailsBO();
                                    detailsBO.ItemName = reader["ItemName"].ToString();
                                    detailsBO.HeadName = reader["HeadName"].ToString();
                                    detailsBO.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                    detailsBO.SalesQuantity = Convert.ToDecimal(reader["SalesQuantity"]);
                                    detailsBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                    detailsBOList.Add(detailsBO);
                                }
                            }
                        }

                    }
                    SalesNQuotationView.SMQuotationDetailsBOList = detailsBOList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return SalesNQuotationView;
        }
        public InvItemStockInformationBO GetInvItemStockInfoByItemAndAttributeId(int companyId, int projectId, int itemId, int colorId, int sizeId, int styleId, int LocationId)
        {

            InvItemStockInformationBO StockInformation = new InvItemStockInformationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemStockInfoByItemAndAttributeId_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, DBNull.Value);

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

                    dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "UnitHead");
                    DataTable Table = ds.Tables["UnitHead"];

                    StockInformation = Table.AsEnumerable().Select(r => new InvItemStockInformationBO
                    {
                        StockId = r.Field<int>("StockId"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        AverageCost = r.Field<decimal>("AverageCost"),
                        ItemId = r.Field<int>("ItemId")
                    }).FirstOrDefault();
                }
            }
            return StockInformation;
        }

    }
}
