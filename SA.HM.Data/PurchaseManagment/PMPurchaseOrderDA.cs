using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.PurchaseManagment;
using System.Data.Common;
using System.Data;
using System.Collections;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.SalesAndMarketing;

namespace HotelManagement.Data.PurchaseManagment
{
    public class PMPurchaseOrderDA : BaseService
    {
        public List<PMPurchaseOrderDetailsBO> GetPMPurchaseOrderDetailByOrderId(int orderId)
        {
            List<PMPurchaseOrderDetailsBO> orderDetailsList = new List<PMPurchaseOrderDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMPurchaseOrderDetailByOrderId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, orderId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMPurchaseOrderDetailsBO orderDetailsBO = new PMPurchaseOrderDetailsBO();

                                orderDetailsBO.DetailId = Int32.Parse(reader["DetailId"].ToString());
                                orderDetailsBO.POrderId = orderId;

                                orderDetailsBO.CurrencyId = Int32.Parse(reader["CurrencyId"].ToString());
                                orderDetailsBO.ConvertionRate = Convert.ToDecimal(reader["ConvertionRate"].ToString());

                                orderDetailsBO.RequisitionId = Int32.Parse(reader["RequisitionId"].ToString());
                                orderDetailsBO.StockById = Int32.Parse(reader["StockById"].ToString());
                                orderDetailsBO.StockBy = reader["HeadName"].ToString();
                                orderDetailsBO.CategoryId = Int32.Parse(reader["CategoryId"].ToString());
                                orderDetailsBO.CategoryName = reader["CategoryName"].ToString();
                                orderDetailsBO.ItemName = reader["ItemName"].ToString();
                                orderDetailsBO.ItemId = Int32.Parse(reader["ItemId"].ToString());
                                orderDetailsBO.Quantity = Convert.ToDecimal(reader["Quantity"].ToString());
                                orderDetailsBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"].ToString());

                                orderDetailsBO.StockQuantity = Convert.ToDecimal(reader["StockQuantity"].ToString());

                                if (!string.IsNullOrEmpty(reader["LastPurchaseDate"].ToString()))
                                    orderDetailsBO.LastPurchaseDate = Convert.ToDateTime(reader["LastPurchaseDate"].ToString());
                                else
                                    orderDetailsBO.LastPurchaseDate = null;

                                if (!string.IsNullOrEmpty(reader["LastPurchasePrice"].ToString()))
                                    orderDetailsBO.LastPurchasePrice = Convert.ToDecimal(reader["LastPurchasePrice"].ToString());
                                else
                                    orderDetailsBO.LastPurchasePrice = null;


                                orderDetailsBO.ColorId = Int32.Parse(reader["ColorId"].ToString());
                                orderDetailsBO.SizeId = Int32.Parse(reader["SizeId"].ToString());
                                orderDetailsBO.StyleId = Int32.Parse(reader["StyleId"].ToString());
                                orderDetailsBO.ColorText = (reader["ColorText"].ToString());
                                orderDetailsBO.SizeText = (reader["SizeText"].ToString());
                                orderDetailsBO.StyleText = (reader["StyleText"].ToString());

                                orderDetailsBO.PRNumber = reader["PRNumber"].ToString();
                                orderDetailsBO.ApprovedQuantity = Convert.ToDecimal(reader["ApprovedQuantity"].ToString());
                                orderDetailsBO.ApprovedPOQuantity = Convert.ToDecimal(reader["ApprovedPOQuantity"].ToString());
                                orderDetailsBO.RemainingPOQuantity = Convert.ToDecimal(reader["RemainingPOQuantity"].ToString());
                                orderDetailsBO.RequisitionDetailsId = Convert.ToInt32(reader["RequisitionDetailsId"].ToString());
                                orderDetailsBO.Remarks = reader["Remarks"].ToString();

                                orderDetailsList.Add(orderDetailsBO);
                            }
                        }
                    }
                }
            }
            return orderDetailsList;
        }
        public List<PMPurchaseOrderDetailsBO> GetPurchaseOrderFromRequisitionSummaryByOrderId(int orderId)
        {
            List<PMPurchaseOrderDetailsBO> orderDetailsList = new List<PMPurchaseOrderDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPurchaseOrderFromRequisitionSummaryByOrderId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, orderId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMPurchaseOrderDetailsBO orderDetailsBO = new PMPurchaseOrderDetailsBO();

                                orderDetailsBO.DetailId = Int32.Parse(reader["DetailId"].ToString());
                                orderDetailsBO.POrderId = orderId;
                                orderDetailsBO.RequisitionId = Int32.Parse(reader["RequisitionId"].ToString());
                                orderDetailsBO.StockById = Int32.Parse(reader["StockById"].ToString());
                                orderDetailsBO.StockBy = reader["HeadName"].ToString();
                                orderDetailsBO.SupplierId = Int32.Parse(reader["SupplierId"].ToString());
                                orderDetailsBO.OrderType = reader["OrderType"].ToString();
                                orderDetailsBO.CategoryId = Int32.Parse(reader["CategoryId"].ToString());
                                orderDetailsBO.CategoryName = reader["CategoryName"].ToString();
                                orderDetailsBO.ItemName = reader["ItemName"].ToString();
                                orderDetailsBO.ItemId = Int32.Parse(reader["ItemId"].ToString());


                                orderDetailsBO.ColorId = Int32.Parse(reader["ColorId"].ToString());
                                orderDetailsBO.SizeId = Int32.Parse(reader["SizeId"].ToString());
                                orderDetailsBO.StyleId = Int32.Parse(reader["StyleId"].ToString());
                                orderDetailsBO.ColorText = (reader["ColorText"].ToString());
                                orderDetailsBO.SizeText = (reader["SizeText"].ToString());
                                orderDetailsBO.StyleText = (reader["StyleText"].ToString());

                                orderDetailsBO.Quantity = Convert.ToDecimal(reader["Quantity"].ToString());
                                orderDetailsBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"].ToString());

                                orderDetailsBO.StockQuantity = Convert.ToDecimal(reader["StockQuantity"].ToString());

                                if (!string.IsNullOrEmpty(reader["LastPurchaseDate"].ToString()))
                                    orderDetailsBO.LastPurchaseDate = Convert.ToDateTime(reader["LastPurchaseDate"].ToString());
                                else
                                    orderDetailsBO.LastPurchaseDate = null;

                                if (!string.IsNullOrEmpty(reader["LastPurchasePrice"].ToString()))
                                    orderDetailsBO.LastPurchasePrice = Convert.ToDecimal(reader["LastPurchasePrice"].ToString());
                                else
                                    orderDetailsBO.LastPurchasePrice = null;

                                orderDetailsBO.Remarks = reader["Remarks"].ToString();

                                orderDetailsList.Add(orderDetailsBO);
                            }
                        }
                    }
                }
            }
            return orderDetailsList;
        }
        public List<PMPurchaseOrderDetailsBO> GetPurchaseOrderDetailByOrderNCostCenterId(int orderId, int costCenterId)
        {
            List<PMPurchaseOrderDetailsBO> orderDetailsList = new List<PMPurchaseOrderDetailsBO>();
            PMPurchaseOrderDetailsBO orderDetailsBO;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPurchaseOrderDetailByOrderNCostcenterId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, orderId);
                    dbSmartAspects.AddInParameter(cmd, "@CostcenterId", DbType.Int32, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                orderDetailsBO = new PMPurchaseOrderDetailsBO();

                                orderDetailsBO.POrderId = orderId;
                                orderDetailsBO.RequisitionId = Int32.Parse(reader["RequisitionId"].ToString());
                                orderDetailsBO.CostCenterId = Int32.Parse(reader["CostCenterId"].ToString());
                                orderDetailsBO.StockById = Int32.Parse(reader["StockById"].ToString());
                                orderDetailsBO.StockBy = reader["HeadName"].ToString();
                                orderDetailsBO.CategoryId = Int32.Parse(reader["CategoryId"].ToString());
                                orderDetailsBO.CategoryName = reader["CategoryName"].ToString();
                                orderDetailsBO.ProductName = reader["ProductName"].ToString();
                                orderDetailsBO.ProductId = Int32.Parse(reader["ItemId"].ToString());
                                orderDetailsBO.Quantity = Convert.ToDecimal(reader["Quantity"].ToString());
                                orderDetailsBO.QuantityReceived = Convert.ToDecimal(reader["QuantityReceived"].ToString());
                                orderDetailsBO.RemainingQuantity = Convert.ToDecimal(reader["RemainingQuantity"].ToString());
                                orderDetailsBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"].ToString());
                                orderDetailsBO.DetailId = Int32.Parse(reader["DetailId"].ToString());
                                orderDetailsBO.DefaultStockLocationId = Int32.Parse(reader["DefaultStockLocationId"].ToString());
                                orderDetailsBO.StockQuantity = Convert.ToDecimal(reader["StockQuantity"].ToString());

                                orderDetailsList.Add(orderDetailsBO);
                            }
                        }
                    }
                }
            }
            return orderDetailsList;
        }
        public List<PMPurchaseOrderDetailsBO> GetPMPurchaseOrderDetailForServiceByOrderId(int orderId)
        {
            List<PMPurchaseOrderDetailsBO> orderDetailsList = new List<PMPurchaseOrderDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMPurchaseOrderDetailForServiceByOrderId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, orderId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMPurchaseOrderDetailsBO orderDetailsBO = new PMPurchaseOrderDetailsBO();
                                orderDetailsBO.POrderId = orderId;
                                orderDetailsBO.RequisitionId = Int32.Parse(reader["RequisitionId"].ToString());
                                orderDetailsBO.CategoryId = Int32.Parse(reader["CategoryId"].ToString());
                                orderDetailsBO.CategoryName = reader["CategoryName"].ToString();
                                orderDetailsBO.ProductName = reader["ProductName"].ToString();
                                orderDetailsBO.ProductId = Int32.Parse(reader["ProductId"].ToString());
                                orderDetailsBO.Quantity = Convert.ToDecimal(reader["Quantity"].ToString());
                                orderDetailsBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"].ToString());
                                orderDetailsBO.DetailId = Int32.Parse(reader["DetailId"].ToString());
                                orderDetailsList.Add(orderDetailsBO);
                            }
                        }
                    }
                }
            }
            return orderDetailsList;
        }
        public PMPurchaseOrderBO GetPMPurchaseOrderInfoByOrderId(int orderId)
        {
            PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMPurchaseOrderInfoByOrderId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, orderId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                orderBO.PONumber = reader["PONumber"].ToString();
                                orderBO.POrderId = Int32.Parse(reader["POrderId"].ToString());
                                orderBO.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                                orderBO.IsLocalOrForeignPO = reader["IsLocalOrForeignPO"].ToString();
                                orderBO.SupplierId = Int32.Parse(reader["SupplierId"].ToString());
                                orderBO.OrderType = reader["OrderType"].ToString();
                                orderBO.SupplierName = reader["SupplierName"].ToString();
                                orderBO.CostCenterId = Int32.Parse(reader["CostCenterId"].ToString());
                                orderBO.CostCenter = reader["CostCenter"].ToString();

                                if (!string.IsNullOrEmpty(reader["ReceivedByDate"].ToString()))
                                {
                                    orderBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                }
                                else
                                    orderBO.ReceivedByDate = null;

                                orderBO.POType = reader["POType"].ToString();
                                orderBO.PODescription = reader["PODescription"].ToString();
                                orderBO.Remarks = reader["Remarks"].ToString();
                                orderBO.DeliveryAddress = reader["DeliveryAddress"].ToString();
                                orderBO.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                orderBO.CreatedBy = Int32.Parse(reader["CreatedBy"].ToString());
                                orderBO.CreatedDate = reader["CreatedDate"].ToString();
                                orderBO.CheckedBy = Int32.Parse(reader["CheckedBy"].ToString());
                                orderBO.ApprovedBy = Int32.Parse(reader["ApprovedBy"].ToString());

                                orderBO.CurrencyId = Int32.Parse(reader["CurrencyId"].ToString());
                                orderBO.ConvertionRate = Convert.ToDecimal(reader["ConvertionRate"].ToString());
                            }
                        }
                    }
                }
            }
            return orderBO;
        }
        public List<PMPurchaseOrderBO> GetPMPurchaseOrderInfoBySupplierId(int supplierId)
        {
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMPurchaseOrderInfoBySupplierId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PurchaseOrder");
                    DataTable Table = ds.Tables["PurchaseOrder"];

                    orderList = Table.AsEnumerable().Select(r => new PMPurchaseOrderBO
                    {
                        POrderId = r.Field<Int32>("POrderId"),
                        ReceivedByDate = r.Field<DateTime>("ReceivedByDate"),
                        PONumber = r.Field<string>("PONumber"),
                        SupplierId = r.Field<int>("SupplierId"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                }
            }
            return orderList;
        }
        public List<PMPurchaseOrderBO> GetPMPurchaseOrderInfoBySearchCriteria(int companyId, string orderType, string poType, DateTime? fromDate, DateTime? toDate,
                                                                               string poNumber, string requisitionNumber, string status, int? costCenterId, int? supplierId,
                                                                               Int32 userInfoId, int recordPerPage, int pageIndex, out int totalRecords
                                                                             )
        {
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();
            totalRecords = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMPurchaseOrderInfoBySearchCriteria_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    if (orderType != "0")
                        dbSmartAspects.AddInParameter(cmd, "@OrderType", DbType.String, orderType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@OrderType", DbType.String, DBNull.Value);

                    if (poType != "All")
                        dbSmartAspects.AddInParameter(cmd, "@POType", DbType.String, poType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@POType", DbType.String, DBNull.Value);

                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    if (!string.IsNullOrEmpty(poNumber))
                        dbSmartAspects.AddInParameter(cmd, "@PONumber", DbType.String, poNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PONumber", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(requisitionNumber))
                        dbSmartAspects.AddInParameter(cmd, "@PRNumber", DbType.String, requisitionNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PRNumber", DbType.String, DBNull.Value);

                    if (costCenterId != null)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.String, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.String, DBNull.Value);

                    if (supplierId != null)
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.String, supplierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.String, DBNull.Value);

                    if (status != "All")
                        dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, status);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
                                orderBO.POrderId = Int32.Parse(reader["POrderId"].ToString());
                                orderBO.POType = reader["POType"].ToString();
                                orderBO.PONumber = reader["PONumber"].ToString();
                                orderBO.PODate = Convert.ToDateTime(reader["PODate"].ToString());
                                orderBO.SupplierId = Int32.Parse(reader["SupplierId"].ToString());
                                orderBO.CostCenterId = Int32.Parse(reader["CostCenterId"].ToString());
                                orderBO.CostCenter = reader["CostCenter"].ToString();
                                orderBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                orderBO.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                orderBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                orderBO.SupplierName = Convert.ToString(reader["SupplierName"].ToString());
                                orderBO.PODescription = Convert.ToString(reader["PODescription"].ToString());
                                orderBO.Remarks = Convert.ToString(reader["Remarks"].ToString());
                                orderBO.ReceiveStatus = Convert.ToString(reader["ReceiveStatus"].ToString());
                                orderBO.IsCanEdit = Convert.ToBoolean(reader["IsCanEdit"].ToString());
                                orderBO.IsCanDelete = Convert.ToBoolean(reader["IsCanDelete"].ToString());
                                orderBO.IsCanChecked = Convert.ToBoolean(reader["IsCanChecked"].ToString());
                                orderBO.IsCanApproved = Convert.ToBoolean(reader["IsCanApproved"].ToString());
                                orderBO.IsCanPOReOpen = Convert.ToBoolean(reader["IsCanPOReOpen"].ToString());
                                orderBO.LocalCurrencyId = Convert.ToInt32(reader["LocalCurrencyId"].ToString());
                                orderBO.LocalCurrencyName = reader["LocalCurrencyName"].ToString();
                                orderBO.CurrencyId = Convert.ToInt32(reader["CurrencyId"].ToString());
                                orderBO.CurrencyName = reader["CurrencyName"].ToString();
                                orderBO.CurrencyType = reader["CurrencyType"].ToString();
                                orderBO.PurchaseOrderAmount = Convert.ToDecimal(reader["PurchaseOrderAmount"].ToString());

                                orderList.Add(orderBO);
                            }
                        }
                    }
                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
            }
            return orderList;
        }
        public PMPurchaseOrderDetailsBO GetPMPurchaseOrderDetailsByProductId(int pOrderId, int productId)
        {
            PMPurchaseOrderDetailsBO detailsBO = new PMPurchaseOrderDetailsBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMPurchaseOrderDetailsByProductId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, pOrderId);
                    dbSmartAspects.AddInParameter(cmd, "@ProductId", DbType.Int32, productId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                detailsBO.DetailId = Convert.ToInt32(reader["DetailId"]);
                                detailsBO.POrderId = Convert.ToInt32(reader["POrderId"]);
                                detailsBO.ProductId = Convert.ToInt32(reader["ProductId"]);
                                detailsBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                detailsBO.StockById = Convert.ToInt32(reader["StockById"]);
                                detailsBO.ProductName = reader["ProductName"].ToString();
                                detailsBO.CategoryName = reader["CategoryName"].ToString();
                                detailsBO.ProductType = reader["ProductType"].ToString();
                                detailsBO.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                detailsBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                detailsBO.QuantityReceived = Convert.ToDecimal(reader["QuantityReceived"]);
                                detailsBO.MessureUnit = reader["MessureUnit"].ToString();
                                detailsBO.ReceivedStatus = reader["ReceivedStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return detailsBO;


        }
        public bool DeleteOrderDetailInfoByOrderId(int POrderId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteOrderDetailInfoByOrderId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@POrderId", DbType.Int32, POrderId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool SavePMPurchaseOrderInfo(PMPurchaseOrderBO orderBO, List<PMPurchaseOrderDetailsBO> detailBO, bool isApprovalProcessEnable, out int tmpOrderId, out string porderNumber, out string TransactionNo, out string TransactionType, out string ApproveStatus)
        {
            bool retVal = false;
            tmpOrderId = -1;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SavePMPurchaseOrderInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, orderBO.CompanyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@POType", DbType.String, orderBO.POType);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsLocalOrForeignPO", DbType.String, orderBO.IsLocalOrForeignPO);
                            dbSmartAspects.AddInParameter(commandMaster, "@SupplierId", DbType.Int32, orderBO.SupplierId);
                            dbSmartAspects.AddInParameter(commandMaster, "@OrderType", DbType.String, orderBO.OrderType);
                            dbSmartAspects.AddInParameter(commandMaster, "@CostCenterId", DbType.Int32, orderBO.CostCenterId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReceivedByDate", DbType.DateTime, orderBO.ReceivedByDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, orderBO.ApprovedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, orderBO.Remarks);
                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, orderBO.CreatedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedBy", DbType.Int32, orderBO.CheckedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedBy", DbType.Int32, orderBO.ApprovedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@DeliveryAddress", DbType.String, orderBO.DeliveryAddress);
                            dbSmartAspects.AddInParameter(commandMaster, "@CurrencyId", DbType.Int32, orderBO.CurrencyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ConvertionRate", DbType.Decimal, orderBO.ConvertionRate);
                            dbSmartAspects.AddInParameter(commandMaster, "@PODescription", DbType.String, orderBO.PODescription);

                            dbSmartAspects.AddOutParameter(commandMaster, "@POrderId", DbType.Int32, sizeof(Int32));
                            dbSmartAspects.AddOutParameter(commandMaster, "@PorderNumber", DbType.String, 50);
                            dbSmartAspects.AddOutParameter(commandMaster, "@TransactionNo", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(commandMaster, "@TransactionType", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(commandMaster, "@ApproveStatus", DbType.String, 100);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                            tmpOrderId = Convert.ToInt32(commandMaster.Parameters["@POrderId"].Value);
                            porderNumber = Convert.ToString(commandMaster.Parameters["@PorderNumber"].Value);
                            TransactionNo = commandMaster.Parameters["@TransactionNo"].Value.ToString();
                            TransactionType = commandMaster.Parameters["@TransactionType"].Value.ToString();
                            ApproveStatus = commandMaster.Parameters["@ApproveStatus"].Value.ToString();

                        }

                        if (status > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SavePMPurchaseOrderDetailsInfo_SP"))
                            {
                                foreach (PMPurchaseOrderDetailsBO orderDetailBO in detailBO)
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@RequisitionId", DbType.Int32, orderDetailBO.RequisitionId);

                                    dbSmartAspects.AddInParameter(commandDetails, "@StockById", DbType.Int32, orderDetailBO.StockById);
                                    dbSmartAspects.AddInParameter(commandDetails, "@POrderId", DbType.Int32, tmpOrderId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int32, orderDetailBO.ItemId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Quantity", DbType.Decimal, orderDetailBO.Quantity);

                                    if (orderDetailBO.ColorId != 0)
                                        dbSmartAspects.AddInParameter(commandDetails, "@ColorId", DbType.Int32, orderDetailBO.ColorId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@ColorId", DbType.Int32, DBNull.Value);
                                    if (orderDetailBO.SizeId != 0)
                                        dbSmartAspects.AddInParameter(commandDetails, "@SizeId", DbType.Int32, orderDetailBO.SizeId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@SizeId", DbType.Int32, DBNull.Value);

                                    if (orderDetailBO.StyleId != 0)
                                        dbSmartAspects.AddInParameter(commandDetails, "@StyleId", DbType.Int32, orderDetailBO.StyleId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@StyleId", DbType.Int32, DBNull.Value);


                                    dbSmartAspects.AddInParameter(commandDetails, "@PurchasePrice", DbType.Decimal, orderDetailBO.PurchasePrice);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Remarks", DbType.String, orderDetailBO.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (isApprovalProcessEnable == false && status > 0)
                        {
                            using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("PurchaseOrderApproval_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@POType", DbType.String, orderBO.POType);
                                dbSmartAspects.AddInParameter(commandMaster, "@POrderId", DbType.Int32, tmpOrderId);
                                dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, orderBO.ApprovedStatus);
                                dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, orderBO.CreatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            }

                            if (status > 0 && orderBO.POType == "Requisition" && orderBO.ApprovedStatus == "Approved")
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdatePOStatusForRequisitionWisePurchase_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandDetails, "@POrderId", DbType.Int32, tmpOrderId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, orderBO.ApprovedStatus);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
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
        public bool UpdatePurchaseOrderInfo(PMPurchaseOrderBO orderBO, List<PMPurchaseOrderDetailsBO> AddedPurchaseOrderDetails,
            List<PMPurchaseOrderDetailsBO> EditedPurchaseOrderDetails, List<PMPurchaseOrderDetailsBO> DeletedPurchaseOrderDetails, out string TransactionNo, out string TransactionType, out string ApproveStatus)
        {
            bool retVal = false;
            int status = 0;
            int tmpOrderId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdatePMPurchaseOrderInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, orderBO.CompanyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@POrderId", DbType.Int32, orderBO.POrderId);
                            dbSmartAspects.AddInParameter(commandMaster, "@POType", DbType.String, orderBO.POType);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsLocalOrForeignPO", DbType.String, orderBO.IsLocalOrForeignPO);
                            dbSmartAspects.AddInParameter(commandMaster, "@SupplierId", DbType.Int32, orderBO.SupplierId);
                            dbSmartAspects.AddInParameter(commandMaster, "@OrderType", DbType.String, orderBO.OrderType);
                            dbSmartAspects.AddInParameter(commandMaster, "@CostCenterId", DbType.Int32, orderBO.CostCenterId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReceivedByDate", DbType.DateTime, orderBO.ReceivedByDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, orderBO.ApprovedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, orderBO.Remarks);
                            dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, orderBO.LastModifiedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedBy", DbType.Int32, orderBO.CheckedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedBy", DbType.Int32, orderBO.ApprovedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@DeliveryAddress", DbType.String, orderBO.DeliveryAddress);
                            dbSmartAspects.AddInParameter(commandMaster, "@PODescription", DbType.String, orderBO.PODescription);
                            dbSmartAspects.AddInParameter(commandMaster, "@CurrencyId", DbType.Int32, orderBO.CurrencyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ConvertionRate", DbType.Decimal, orderBO.ConvertionRate);
                            dbSmartAspects.AddOutParameter(commandMaster, "@TransactionNo", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(commandMaster, "@TransactionType", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(commandMaster, "@ApproveStatus", DbType.String, 100);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            tmpOrderId = orderBO.POrderId;

                            TransactionNo = commandMaster.Parameters["@TransactionNo"].Value.ToString();
                            TransactionType = commandMaster.Parameters["@TransactionType"].Value.ToString();
                            ApproveStatus = commandMaster.Parameters["@ApproveStatus"].Value.ToString();
                        }

                        if (status > 0 && AddedPurchaseOrderDetails.Count() > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SavePMPurchaseOrderDetailsInfo_SP"))
                            {
                                foreach (PMPurchaseOrderDetailsBO orderDetailBO in AddedPurchaseOrderDetails)
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@RequisitionId", DbType.Int32, orderDetailBO.RequisitionId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@StockById", DbType.Int32, orderDetailBO.StockById);
                                    dbSmartAspects.AddInParameter(commandDetails, "@POrderId", DbType.Int32, tmpOrderId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int32, orderDetailBO.ItemId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Quantity", DbType.Decimal, orderDetailBO.Quantity);


                                    if (orderDetailBO.ColorId != 0)
                                        dbSmartAspects.AddInParameter(commandDetails, "@ColorId", DbType.Int32, orderDetailBO.ColorId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@ColorId", DbType.Int32, DBNull.Value);
                                    if (orderDetailBO.SizeId != 0)
                                        dbSmartAspects.AddInParameter(commandDetails, "@SizeId", DbType.Int32, orderDetailBO.SizeId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@SizeId", DbType.Int32, DBNull.Value);

                                    if (orderDetailBO.StyleId != 0)
                                        dbSmartAspects.AddInParameter(commandDetails, "@StyleId", DbType.Int32, orderDetailBO.StyleId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@StyleId", DbType.Int32, DBNull.Value);




                                    dbSmartAspects.AddInParameter(commandDetails, "@PurchasePrice", DbType.Decimal, orderDetailBO.PurchasePrice);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Remarks", DbType.String, orderDetailBO.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && EditedPurchaseOrderDetails.Count() > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdatePMPurchaseOrderDetailsInfo_SP"))
                            {
                                foreach (PMPurchaseOrderDetailsBO orderDetailBO in EditedPurchaseOrderDetails)
                                {
                                    commandDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDetails, "@DetailId", DbType.Int32, orderDetailBO.DetailId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RequisitionId", DbType.Int32, orderDetailBO.RequisitionId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@StockById", DbType.Int32, orderDetailBO.StockById);
                                    dbSmartAspects.AddInParameter(commandDetails, "@POrderId", DbType.Int32, tmpOrderId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int32, orderDetailBO.ItemId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Quantity", DbType.Decimal, orderDetailBO.Quantity);


                                    if (orderDetailBO.ColorId != 0)
                                        dbSmartAspects.AddInParameter(commandDetails, "@ColorId", DbType.Int32, orderDetailBO.ColorId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@ColorId", DbType.Int32, DBNull.Value);
                                    if (orderDetailBO.SizeId != 0)
                                        dbSmartAspects.AddInParameter(commandDetails, "@SizeId", DbType.Int32, orderDetailBO.SizeId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@SizeId", DbType.Int32, DBNull.Value);

                                    if (orderDetailBO.StyleId != 0)
                                        dbSmartAspects.AddInParameter(commandDetails, "@StyleId", DbType.Int32, orderDetailBO.StyleId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@StyleId", DbType.Int32, DBNull.Value);



                                    dbSmartAspects.AddInParameter(commandDetails, "@PurchasePrice", DbType.Decimal, orderDetailBO.PurchasePrice);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Remarks", DbType.String, orderDetailBO.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && DeletedPurchaseOrderDetails.Count() > 0)
                        {
                            using (DbCommand commandDeletePO = dbSmartAspects.GetStoredProcCommand("DeletePurchaseOrderDetailsInfo_SP"))
                            {
                                foreach (PMPurchaseOrderDetailsBO po in DeletedPurchaseOrderDetails)
                                {
                                    commandDeletePO.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeletePO, "@DetailId", DbType.Int32, po.DetailId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@RequisitionId", DbType.Int32, po.RequisitionId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@POrderId", DbType.Int32, tmpOrderId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ItemId", DbType.Int32, po.ItemId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@Quantity", DbType.Decimal, po.Quantity);


                                    if (po.ColorId != 0)
                                        dbSmartAspects.AddInParameter(commandDeletePO, "@ColorId", DbType.Int32, po.ColorId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDeletePO, "@ColorId", DbType.Int32, DBNull.Value);
                                    if (po.SizeId != 0)
                                        dbSmartAspects.AddInParameter(commandDeletePO, "@SizeId", DbType.Int32, po.SizeId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDeletePO, "@SizeId", DbType.Int32, DBNull.Value);

                                    if (po.StyleId != 0)
                                        dbSmartAspects.AddInParameter(commandDeletePO, "@StyleId", DbType.Int32, po.StyleId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDeletePO, "@StyleId", DbType.Int32, DBNull.Value);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePO, transction);
                                }
                            }
                        }

                        if (status > 0 && orderBO.POType == "Requisition")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdatePOStatusForRequisitionWisePurchase_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@POrderId", DbType.Int32, tmpOrderId);
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

        public bool SavePurchaseOrderFromRequisition(PMPurchaseOrderBO orderBO,
                                                     List<PMPurchaseOrderDetailsBO> PurchaseOrderItemFromRequisition,
                                                     bool isApprovalProcessEnable, out int tmpOrderId, out string porderNumber
                                                    , out string TransactionNo, out string TransactionType, out string ApproveStatus)
        {
            bool retVal = false;
            tmpOrderId = -1;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SavePMPurchaseOrderInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, orderBO.CompanyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@POType", DbType.String, orderBO.POType);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsLocalOrForeignPO", DbType.String, orderBO.IsLocalOrForeignPO);
                            dbSmartAspects.AddInParameter(commandMaster, "@SupplierId", DbType.Int32, orderBO.SupplierId);
                            dbSmartAspects.AddInParameter(commandMaster, "@OrderType", DbType.String, orderBO.OrderType);
                            dbSmartAspects.AddInParameter(commandMaster, "@CostCenterId", DbType.Int32, orderBO.CostCenterId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReceivedByDate", DbType.DateTime, orderBO.ReceivedByDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, orderBO.ApprovedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, orderBO.Remarks);
                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, orderBO.CreatedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedBy", DbType.Int32, orderBO.CheckedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedBy", DbType.Int32, orderBO.ApprovedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@DeliveryAddress", DbType.String, orderBO.DeliveryAddress);
                            dbSmartAspects.AddInParameter(commandMaster, "@CurrencyId", DbType.Int32, orderBO.CurrencyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ConvertionRate", DbType.Decimal, orderBO.ConvertionRate);
                            dbSmartAspects.AddInParameter(commandMaster, "@PODescription", DbType.String, orderBO.PODescription);

                            dbSmartAspects.AddOutParameter(commandMaster, "@POrderId", DbType.Int32, sizeof(Int32));
                            dbSmartAspects.AddOutParameter(commandMaster, "@PorderNumber", DbType.String, 50);
                            dbSmartAspects.AddOutParameter(commandMaster, "@TransactionNo", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(commandMaster, "@TransactionType", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(commandMaster, "@ApproveStatus", DbType.String, 100);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                            tmpOrderId = Convert.ToInt32(commandMaster.Parameters["@POrderId"].Value);
                            porderNumber = Convert.ToString(commandMaster.Parameters["@PorderNumber"].Value);
                            TransactionNo = commandMaster.Parameters["@TransactionNo"].Value.ToString();
                            TransactionType = commandMaster.Parameters["@TransactionType"].Value.ToString();
                            ApproveStatus = commandMaster.Parameters["@ApproveStatus"].Value.ToString();
                        }

                        if (status > 0 && PurchaseOrderItemFromRequisition != null)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SavePMPurchaseOrderDetailsInfo_SP"))
                            {
                                foreach (PMPurchaseOrderDetailsBO orderDetailBO in PurchaseOrderItemFromRequisition)
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@RequisitionId", DbType.Int32, orderDetailBO.RequisitionId);

                                    dbSmartAspects.AddInParameter(commandDetails, "@StockById", DbType.Int32, orderDetailBO.StockById);
                                    dbSmartAspects.AddInParameter(commandDetails, "@POrderId", DbType.Int32, tmpOrderId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int32, orderDetailBO.ItemId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Quantity", DbType.Decimal, orderDetailBO.Quantity);


                                    if (orderDetailBO.ColorId != 0)
                                        dbSmartAspects.AddInParameter(commandDetails, "@ColorId", DbType.Int32, orderDetailBO.ColorId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@ColorId", DbType.Int32, DBNull.Value);
                                    if (orderDetailBO.SizeId != 0)
                                        dbSmartAspects.AddInParameter(commandDetails, "@SizeId", DbType.Int32, orderDetailBO.SizeId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@SizeId", DbType.Int32, DBNull.Value);

                                    if (orderDetailBO.StyleId != 0)
                                        dbSmartAspects.AddInParameter(commandDetails, "@StyleId", DbType.Int32, orderDetailBO.StyleId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@StyleId", DbType.Int32, DBNull.Value);



                                    dbSmartAspects.AddInParameter(commandDetails, "@PurchasePrice", DbType.Decimal, orderDetailBO.PurchasePrice);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Remarks", DbType.String, orderDetailBO.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && isApprovalProcessEnable == true && orderBO.POType == "Requisition")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdatePOStatusForRequisitionWisePurchase_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@POrderId", DbType.Int32, tmpOrderId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }
                        }

                        if (isApprovalProcessEnable == false && status > 0)
                        {
                            using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("PurchaseOrderApproval_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@POType", DbType.String, orderBO.POType);
                                dbSmartAspects.AddInParameter(commandMaster, "@POrderId", DbType.Int32, tmpOrderId);
                                dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, orderBO.ApprovedStatus);
                                dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, orderBO.CreatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            }

                            if (status > 0 && orderBO.POType == "Requisition" && orderBO.ApprovedStatus == "Approved")
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdatePOStatusForRequisitionWisePurchase_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandDetails, "@POrderId", DbType.Int32, tmpOrderId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, orderBO.ApprovedStatus);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
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

        public bool UpdatePurchaseOrderFromRequisition(PMPurchaseOrderBO orderBO, List<PMPurchaseOrderDetailsBO> AddedPurchaseOrderDetails, List<PMPurchaseOrderDetailsBO> EditedPurchaseOrderDetails, List<PMPurchaseOrderDetailsBO> DeletedPurchaseOrderDetails, out string TransactionNo, out string TransactionType, out string ApproveStatus)
        {
            bool retVal = false;
            int status = 0;
            int tmpOrderId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdatePMPurchaseOrderInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@POrderId", DbType.Int32, orderBO.POrderId);
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, orderBO.CompanyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@POType", DbType.String, orderBO.POType);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsLocalOrForeignPO", DbType.String, orderBO.IsLocalOrForeignPO);
                            dbSmartAspects.AddInParameter(commandMaster, "@SupplierId", DbType.Int32, orderBO.SupplierId);
                            dbSmartAspects.AddInParameter(commandMaster, "@CostCenterId", DbType.Int32, orderBO.CostCenterId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReceivedByDate", DbType.DateTime, orderBO.ReceivedByDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, orderBO.ApprovedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, orderBO.Remarks);
                            dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, orderBO.LastModifiedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedBy", DbType.Int32, orderBO.CheckedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedBy", DbType.Int32, orderBO.ApprovedBy);

                            dbSmartAspects.AddInParameter(commandMaster, "@CurrencyId", DbType.Int32, orderBO.CurrencyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ConvertionRate", DbType.Decimal, orderBO.ConvertionRate);
                            dbSmartAspects.AddOutParameter(commandMaster, "@TransactionNo", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(commandMaster, "@TransactionType", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(commandMaster, "@ApproveStatus", DbType.String, 100);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            tmpOrderId = orderBO.POrderId;

                            TransactionNo = commandMaster.Parameters["@TransactionNo"].Value.ToString();
                            TransactionType = commandMaster.Parameters["@TransactionType"].Value.ToString();
                            ApproveStatus = commandMaster.Parameters["@ApproveStatus"].Value.ToString();
                        }

                        if (status > 0 && AddedPurchaseOrderDetails.Count() > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SavePMPurchaseOrderDetailsInfo_SP"))
                            {
                                foreach (PMPurchaseOrderDetailsBO orderDetailBO in AddedPurchaseOrderDetails)
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@RequisitionId", DbType.Int32, orderDetailBO.RequisitionId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@StockById", DbType.Int32, orderDetailBO.StockById);
                                    dbSmartAspects.AddInParameter(commandDetails, "@POrderId", DbType.Int32, tmpOrderId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int32, orderDetailBO.ItemId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Quantity", DbType.Decimal, orderDetailBO.Quantity);
                                    dbSmartAspects.AddInParameter(commandDetails, "@PurchasePrice", DbType.Decimal, orderDetailBO.PurchasePrice);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Remarks", DbType.String, orderDetailBO.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && EditedPurchaseOrderDetails.Count() > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdatePMPurchaseOrderDetailsInfo_SP"))
                            {
                                foreach (PMPurchaseOrderDetailsBO orderDetailBO in EditedPurchaseOrderDetails)
                                {
                                    commandDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDetails, "@DetailId", DbType.Int32, orderDetailBO.DetailId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@RequisitionId", DbType.Int32, orderDetailBO.RequisitionId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@StockById", DbType.Int32, orderDetailBO.StockById);
                                    dbSmartAspects.AddInParameter(commandDetails, "@POrderId", DbType.Int32, tmpOrderId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int32, orderDetailBO.ItemId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Quantity", DbType.Decimal, orderDetailBO.Quantity);
                                    dbSmartAspects.AddInParameter(commandDetails, "@PurchasePrice", DbType.Decimal, orderDetailBO.PurchasePrice);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Remarks", DbType.String, orderDetailBO.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && DeletedPurchaseOrderDetails.Count() > 0)
                        {
                            using (DbCommand commandDeletePO = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (PMPurchaseOrderDetailsBO po in DeletedPurchaseOrderDetails)
                                {
                                    commandDeletePO.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TableName", DbType.String, "PMPurchaseOrderDetails");
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TablePKField", DbType.String, "DetailId");
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TablePKId", DbType.String, po.DetailId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePO);
                                }
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

        public bool UpdatePurchaseOrderStatus(int orderId, string approvedStatus, int approvedBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePurchaseOrderStatus_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@POrderId", DbType.Int32, orderId);
                    dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, approvedStatus);
                    dbSmartAspects.AddInParameter(command, "@ApprovedBy", DbType.Int32, approvedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;

        }
        public bool DeletePurchaseOrder(int orderId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeletePurchaseOrder_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@POrderId", DbType.Int32, orderId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<PMPurchaseOrderBO> GetAllPMPurchaseOrderInfo(string poType)
        {
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllPMPurchaseOrderInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@POType", DbType.String, poType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
                                orderBO.POrderId = Int32.Parse(reader["POrderId"].ToString());
                                orderBO.PONumber = reader["PONumber"].ToString();
                                orderBO.CreatedDate = reader["CreatedDate"].ToString();
                                orderBO.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                orderBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                orderList.Add(orderBO);
                            }
                        }
                    }
                }
            }
            return orderList;
        }
        public List<PMPurchaseOrderBO> GetApprovedPMPurchaseOrderInfo(int userGroupId, string poType)
        {
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApprovedPMPurchaseOrderInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);
                    dbSmartAspects.AddInParameter(cmd, "@POType", DbType.String, poType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();

                                orderBO.POrderId = Int32.Parse(reader["POrderId"].ToString());
                                orderBO.PONumber = reader["PONumber"].ToString();
                                orderBO.POType = reader["POType"].ToString();
                                orderBO.CreatedDate = reader["CreatedDate"].ToString();
                                orderBO.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                orderBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                orderBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"].ToString());
                                orderBO.SupplierId = Convert.ToInt32(reader["SupplierId"].ToString());
                                orderBO.CompanyId = Convert.ToInt32(reader["CompanyId"].ToString());

                                orderList.Add(orderBO);
                            }
                        }
                    }
                }
            }
            return orderList;
        }
        public List<PMPurchaseOrderBO> GetApprovedPMPurchaseOrderInfo(string poType, int costCenterId)
        {
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApprovedPorderByCostcenter_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@POType", DbType.String, poType);
                    dbSmartAspects.AddInParameter(cmd, "@CostcenterId", DbType.Int32, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
                                orderBO.POrderId = Int32.Parse(reader["POrderId"].ToString());
                                orderBO.PONumber = reader["PONumber"].ToString();
                                orderBO.CreatedDate = reader["CreatedDate"].ToString();
                                orderBO.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                orderBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                orderList.Add(orderBO);
                            }
                        }
                    }
                }
            }
            return orderList;
        }
        public List<PMPurchaseOrderDetailsBO> GetAvailableItemForPOrderId(int POId, string poType, int supplierId)
        {
            List<PMPurchaseOrderDetailsBO> productList = new List<PMPurchaseOrderDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAvailableItemForPOrderId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, POId);

                    if (!string.IsNullOrEmpty(poType))
                        dbSmartAspects.AddInParameter(cmd, "@POType", DbType.String, poType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@POType", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMPurchaseOrderDetailsBO productBO = new PMPurchaseOrderDetailsBO();
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.ItemName = reader["Name"].ToString();

                                productBO.ColorId = Convert.ToInt32(reader["ColorId"]);
                                productBO.ColorText = reader["ColorText"].ToString();
                                productBO.SizeId = Convert.ToInt32(reader["SizeId"]);
                                productBO.SizeText = reader["SizeText"].ToString();
                                productBO.StyleId = Convert.ToInt32(reader["StyleId"]);
                                productBO.StyleText = reader["StyleText"].ToString();

                                productBO.POrderId = Convert.ToInt32(reader["POrderId"].ToString());
                                productBO.StockById = Convert.ToInt32(reader["StockById"]);
                                productBO.MessureUnit = reader["HeadName"].ToString();
                                productBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"].ToString());
                                productBO.Quantity = Convert.ToDecimal(reader["Quantity"].ToString());
                                productBO.QuantityReceived = Convert.ToDecimal(reader["QuantityReceived"].ToString());
                                productBO.RemainingReceiveQuantity = Convert.ToDecimal(reader["RemainingReceiveQuantity"].ToString());
                                productBO.ReceivedId = Convert.ToInt32(reader["ReceivedId"].ToString());
                                productBO.ReceiveDetailsId = Convert.ToInt32(reader["ReceiveDetailsId"].ToString());
                                productBO.SupplierId = Convert.ToInt32(reader["SupplierId"].ToString());
                                productBO.ProductType = Convert.ToString(reader["ProductType"]);

                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }
        public List<PurchaseInformationBO> GetPurchaseInformation(int companyId, DateTime dateFrom, DateTime dateTo, int? supplierId, int? itemId,
                                                            string pONumber, int category, int costcenter, string purchaseType, int userInfoId)
        {
            List<PurchaseInformationBO> purchaseInfo = new List<PurchaseInformationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPurchaseInformationForReport_SP"))
                {
                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, dateTo);

                    if (supplierId != null || supplierId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, DBNull.Value);

                    if (itemId != null || itemId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(pONumber))
                        dbSmartAspects.AddInParameter(cmd, "@PONumber", DbType.String, pONumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PONumber", DbType.String, DBNull.Value);

                    if (category > 0)
                        dbSmartAspects.AddInParameter(cmd, "@Category", DbType.Int32, category);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Category", DbType.Int32, DBNull.Value);

                    if (costcenter > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenter", DbType.Int32, costcenter);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenter", DbType.Int32, DBNull.Value);

                    if (purchaseType != "0")
                        dbSmartAspects.AddInParameter(cmd, "@PurchaseType", DbType.String, purchaseType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PurchaseType", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PurchaseInfo");
                    DataTable Table = ds.Tables["PurchaseInfo"];

                    purchaseInfo = Table.AsEnumerable().Select(r => new PurchaseInformationBO
                    {
                        POrderId = r.Field<Int32>("POrderId"),
                        PODate = r.Field<string>("PODate"),
                        ReceivedByDate = r.Field<DateTime?>("ReceivedByDate"),
                        PONumber = r.Field<string>("PONumber"),
                        SupplierId = r.Field<int?>("SupplierId"),
                        SupplierName = r.Field<string>("SupplierName"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        Remarks = r.Field<string>("Remarks"),
                        DetailId = r.Field<int?>("DetailId"),
                        ProductId = r.Field<int?>("ProductId"),
                        ProductName = r.Field<string>("ProductName"),
                        ProductCode = r.Field<string>("ProductCode"),
                        ProductCategory = r.Field<string>("ProductCategory"),
                        CategoryCode = r.Field<string>("CategoryCode"),
                        PurchasePrice = r.Field<decimal?>("PurchasePrice"),
                        Quantity = r.Field<decimal?>("Quantity"),
                        QuantityReceived = r.Field<decimal>("QuantityReceived"),
                        MessureUnit = r.Field<string>("MessureUnit"),
                        CostCenter = r.Field<string>("CostCenter")

                    }).ToList();
                }
            }

            return purchaseInfo;
        }
        public List<ProductReturnInformationBO> GetProductReturnInformation(int supplierId, DateTime dateFrom, DateTime dateTo, string returnType)
        {
            List<ProductReturnInformationBO> purchaseInfo = new List<ProductReturnInformationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductReturnInformationForReport_SP"))
                {
                    if (supplierId == 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, dateTo);

                    if (!string.IsNullOrWhiteSpace(returnType))
                        dbSmartAspects.AddInParameter(cmd, "@ReturnType", DbType.String, returnType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReturnType", DbType.String, DBNull.Value);


                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PurchaseInfo");
                    DataTable Table = ds.Tables["PurchaseInfo"];

                    purchaseInfo = Table.AsEnumerable().Select(r => new ProductReturnInformationBO
                    {
                        ReturnId = r.Field<Int64>("ReturnId"),
                        ReturnDate = r.Field<string>("ReturnDate"),
                        SupplierId = r.Field<Int32>("SupplierId"),
                        Supplier = r.Field<string>("Supplier"),
                        ReturnType = r.Field<string>("ReturnType"),
                        TransactionId = r.Field<int?>("TransactionId"),
                        InvoiceNumber = r.Field<string>("InvoiceNumber"),
                        ProductId = r.Field<int?>("ItemId"),
                        ProductName = r.Field<string>("ProductName"),
                        SerialNumber = r.Field<string>("SerialNumber"),
                        Quantity = r.Field<decimal?>("Quantity"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                }
            }

            return purchaseInfo;
        }
        public List<PMPurchaseOrderInfoReportBO> GetPMPurchaseOrderInfoForReport(int pOrderId)
        {
            List<PMPurchaseOrderInfoReportBO> purchaseInfo = new List<PMPurchaseOrderInfoReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMPurchaseOrderInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, pOrderId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PurchaseInfo");
                    DataTable Table = ds.Tables["PurchaseInfo"];

                    purchaseInfo = Table.AsEnumerable().Select(r => new PMPurchaseOrderInfoReportBO
                    {
                        //CategoryName	CategoryCode	ServiceType	ProductName	ProductCode	PONumber	
                        //CreatedDate	ApprovedDate	DetailId	POrderId	Quantity	QuantityReceived

                        CategoryName = r.Field<string>("CategoryName"),
                        CategoryCode = r.Field<string>("CategoryCode"),
                        ServiceType = r.Field<string>("ServiceType"),
                        ProductName = r.Field<string>("ProductName"),
                        ProductCode = r.Field<string>("ProductCode"),
                        PONumber = r.Field<string>("PONumber"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        ApprovedDate = r.Field<DateTime?>("ApprovedDate"),
                        DetailId = r.Field<int>("DetailId"),
                        POrderId = r.Field<int>("POrderId"),
                        Quantity = r.Field<decimal>("Quantity"),
                        QuantityReceived = r.Field<decimal>("QuantityReceived")

                    }).ToList();
                }
            }

            return purchaseInfo;
        }
        public List<PMPurchaseOrderInfoReportBO> GetProductReceivedFromPMPurchaseOrderInfo(int pOrderId)
        {
            List<PMPurchaseOrderInfoReportBO> purchaseInfo = new List<PMPurchaseOrderInfoReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductReceivedFromPMPurchaseOrderInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, pOrderId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PurchaseInfo");
                    DataTable Table = ds.Tables["PurchaseInfo"];

                    purchaseInfo = Table.AsEnumerable().Select(r => new PMPurchaseOrderInfoReportBO
                    {
                        //CategoryName	CategoryCode	ServiceType	ProductName	ProductCode	PONumber	
                        //CreatedDate	ApprovedDate	DetailId	POrderId	Quantity	QuantityReceived

                        //CategoryName = r.Field<string>("CategoryName"),
                        //CategoryCode = r.Field<string>("CategoryCode"),
                        //ServiceType = r.Field<string>("ServiceType"),
                        ProductName = r.Field<string>("ProductName"),
                        //ProductCode = r.Field<string>("ProductCode"),
                        //PONumber = r.Field<string>("PONumber"),
                        //CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        //ApprovedDate = r.Field<DateTime?>("ApprovedDate"),
                        ProductId = r.Field<int>("ProductId"),
                        ProductTransactionId = r.Field<string>("ProductTransactionId"),
                        QuantityReceived = r.Field<decimal>("QuantityReceived")

                    }).ToList();
                }
            }

            return purchaseInfo;
        }
        public List<SupplierNCompanyInfoForPurchaseInvoiceBO> GetPurchaseInformationForInvoice(int pOrderId, int supplierId, int currencyId)
        {
            List<SupplierNCompanyInfoForPurchaseInvoiceBO> purchaseInfo = new List<SupplierNCompanyInfoForPurchaseInvoiceBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPurchaseOrderDetailsForInvoice_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, pOrderId);
                    dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    dbSmartAspects.AddInParameter(cmd, "@CurrencyId", DbType.Int32, currencyId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PurchaseInfo");
                    DataTable Table = ds.Tables["PurchaseInfo"];

                    purchaseInfo = Table.AsEnumerable().Select(r => new SupplierNCompanyInfoForPurchaseInvoiceBO
                    {
                        POrderId = r.Field<Int32>("POrderId"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        PONumber = r.Field<string>("PONumber"),
                        PODate = r.Field<DateTime>("PODate"),
                        PODateString = r.Field<string>("PODateString"),
                        ReceivedByDate = r.Field<DateTime>("ReceivedByDate"),
                        ReceivedByDateString = r.Field<string>("ReceivedByDateString"),
                        SupplierId = r.Field<int>("SupplierId"),
                        Remarks = r.Field<string>("Remarks"),
                        DeliveryAddress = r.Field<string>("DeliveryAddress"),
                        ProductId = r.Field<int>("ProductId"),
                        ItemName = r.Field<string>("ItemName"),
                        Quantity = r.Field<decimal>("Quantity"),
                        UnitPrice = r.Field<decimal>("UnitPrice"),
                        TotalPrice = r.Field<decimal>("TotalPrice"),
                        HeadName = r.Field<string>("HeadName"),
                        CreatedByName = r.Field<string>("CreatedByName"),
                        CheckedByName = r.Field<string>("CheckedByName"),
                        ApprovedByName = r.Field<string>("ApprovedByName"),
                        ItemRemarks = r.Field<string>("ItemRemarks"),
                        CurrencyName = r.Field<string>("CurrencyName")

                    }).ToList();
                }
            }

            return purchaseInfo;
        }
        public List<PurchaseOrderDetailsForInvoiceBO> GetSupplierInformationForInvoice(int supplierId)
        {
            List<PurchaseOrderDetailsForInvoiceBO> purchaseInfo = new List<PurchaseOrderDetailsForInvoiceBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyAndSupplierInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PurchaseInfo");
                    DataTable Table = ds.Tables["PurchaseInfo"];

                    purchaseInfo = Table.AsEnumerable().Select(r => new PurchaseOrderDetailsForInvoiceBO
                    {
                        CompanyCode = r.Field<string>("CompanyCode"),
                        CompanyName = r.Field<string>("CompanyName"),
                        CompanyAddress = r.Field<string>("CompanyAddress"),
                        EmailAddress = r.Field<string>("EmailAddress"),
                        WebAddress = r.Field<string>("WebAddress"),
                        ContactNumber = r.Field<string>("ContactNumber"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        SupplierName = r.Field<string>("SupplierName"),
                        SupplierCode = r.Field<string>("SupplierCode"),
                        SupplierAddress = r.Field<string>("SupplierAddress"),
                        SupplierPhone = r.Field<string>("SupplierPhone"),
                        SupplierFax = r.Field<string>("SupplierFax"),
                        SupplierEmail = r.Field<string>("SupplierEmail"),
                        SupplierWebAddress = r.Field<string>("SupplierWebAddress"),
                        SupplierContactPerson = r.Field<string>("SupplierContactPerson"),
                        SupplierContactEmail = r.Field<string>("SupplierContactEmail"),
                        SupplierContactPhone = r.Field<string>("SupplierContactPhone")

                    }).ToList();
                }
            }

            return purchaseInfo;
        }
        public List<PurchaseOrderDetailsForInvoiceBO> GetCompanyAndGuestCompanyInfo(int supplierId)
        {
            List<PurchaseOrderDetailsForInvoiceBO> purchaseInfo = new List<PurchaseOrderDetailsForInvoiceBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyAndGuestCompanyInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GuestCompanyId", DbType.Int32, supplierId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PurchaseInfo");
                    DataTable Table = ds.Tables["PurchaseInfo"];

                    purchaseInfo = Table.AsEnumerable().Select(r => new PurchaseOrderDetailsForInvoiceBO
                    {
                        CompanyCode = r.Field<string>("CompanyCode"),
                        CompanyName = r.Field<string>("CompanyName"),
                        CompanyAddress = r.Field<string>("CompanyAddress"),
                        EmailAddress = r.Field<string>("EmailAddress"),
                        WebAddress = r.Field<string>("WebAddress"),
                        ContactNumber = r.Field<string>("ContactNumber"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        SupplierName = r.Field<string>("SupplierName"),
                        SupplierCode = r.Field<string>("SupplierCode"),
                        SupplierAddress = r.Field<string>("SupplierAddress"),
                        SupplierPhone = r.Field<string>("SupplierPhone"),
                        SupplierFax = r.Field<string>("SupplierFax"),
                        SupplierEmail = r.Field<string>("SupplierEmail"),
                        SupplierWebAddress = r.Field<string>("SupplierWebAddress"),
                        SupplierContactPerson = r.Field<string>("SupplierContactPerson"),
                        SupplierContactEmail = r.Field<string>("SupplierContactEmail"),
                        SupplierContactPhone = r.Field<string>("SupplierContactPhone")

                    }).ToList();
                }
            }

            return purchaseInfo;
        }
        public List<PMPurchaseOrderBO> GetSupplierWiseReceivedPurchaseOrderInfo(int supplierId)
        {
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierWiseReceivedPurchaseOrderInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
                                orderBO.POrderId = Int32.Parse(reader["POrderId"].ToString());
                                orderBO.PONumber = reader["PONumber"].ToString();
                                orderBO.CreatedDate = reader["CreatedDate"].ToString();
                                orderBO.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                orderBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                orderList.Add(orderBO);
                            }
                        }
                    }
                }
            }
            return orderList;
        }
        // //-----Sales Order for Sales and Marketing Module--------------------------------------
        public bool SaveSMSalesOrderInfo(PMPurchaseOrderBO orderBO, List<PMPurchaseOrderDetailsBO> detailBO, out int tmpOrderId, out string porderNumber)
        {
            bool retVal = false;
            tmpOrderId = -1;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveSMSalesOrderInfo_SP"))
                        {
                            //dbSmartAspects.AddInParameter(commandMaster, "@POType", DbType.String, orderBO.POType);
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, orderBO.SupplierId);
                            dbSmartAspects.AddInParameter(commandMaster, "@CostCenterId", DbType.Int32, orderBO.CostCenterId);
                            dbSmartAspects.AddInParameter(commandMaster, "@DeliveryDate", DbType.DateTime, orderBO.ReceivedByDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, orderBO.ApprovedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, orderBO.Remarks);
                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, orderBO.CreatedBy);
                            //dbSmartAspects.AddInParameter(commandMaster, "@CheckedBy", DbType.Int32, orderBO.CheckedBy);
                            //dbSmartAspects.AddInParameter(commandMaster, "@ApprovedBy", DbType.Int32, orderBO.ApprovedBy);

                            dbSmartAspects.AddOutParameter(commandMaster, "@SOrderId", DbType.Int32, sizeof(Int32));
                            dbSmartAspects.AddOutParameter(commandMaster, "@PorderNumber", DbType.String, 50);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                            tmpOrderId = Convert.ToInt32(commandMaster.Parameters["@SOrderId"].Value);
                            porderNumber = Convert.ToString(commandMaster.Parameters["@PorderNumber"].Value);
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveSMSalesOrderDetailsInfo_SP"))
                            {
                                foreach (PMPurchaseOrderDetailsBO orderDetailBO in detailBO)
                                {
                                    commandDetails.Parameters.Clear();
                                    //dbSmartAspects.AddInParameter(commandDetails, "@RequisitionId", DbType.Int32, orderDetailBO.RequisitionId);

                                    dbSmartAspects.AddInParameter(commandDetails, "@CostCenterId", DbType.Int32, orderDetailBO.CostCenterId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@StockById", DbType.Int32, orderDetailBO.StockById);
                                    dbSmartAspects.AddInParameter(commandDetails, "@SOrderId", DbType.Int32, tmpOrderId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ProductId", DbType.Int32, orderDetailBO.ProductId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Quantity", DbType.Decimal, orderDetailBO.Quantity);
                                    dbSmartAspects.AddInParameter(commandDetails, "@PurchasePrice", DbType.Decimal, orderDetailBO.PurchasePrice);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
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
        public bool DeleteSMSalesOrder(int orderId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteSMSalesOrder_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@SOrderId", DbType.Int32, orderId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<PMPurchaseOrderBO> GetSMSalesOrderInfoBySearchCriteria(string POType, DateTime FromDate, DateTime ToDate, string PONumber, Int32 costCenterId, string status)
        {
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSMSalesOrderInfoBySearchCriteria_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    //dbSmartAspects.AddInParameter(cmd, "@POType", DbType.String, POType);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, FromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, ToDate);
                    if (!string.IsNullOrEmpty(PONumber))
                        dbSmartAspects.AddInParameter(cmd, "@SONumber", DbType.String, PONumber);
                    else dbSmartAspects.AddInParameter(cmd, "@SONumber", DbType.String, DBNull.Value);

                    if (costCenterId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (status != "All")
                        dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, status);
                    else dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, DBNull.Value);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
                                orderBO.POrderId = Int32.Parse(reader["SOrderId"].ToString());
                                orderBO.PONumber = reader["SONumber"].ToString();
                                orderBO.SupplierId = Int32.Parse(reader["CompanyId"].ToString());
                                orderBO.SupplierName = reader["CompanyName"].ToString();
                                orderBO.PONumber = reader["SONumber"].ToString();
                                orderBO.CreatedDate = reader["CreatedDate"].ToString();
                                orderBO.CheckedBy = Int32.Parse(reader["CheckedBy"].ToString());
                                orderBO.ApprovedBy = Int32.Parse(reader["ApprovedBy"].ToString());
                                orderBO.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                orderBO.ReceivedByDate = Convert.ToDateTime(reader["DeliveryDate"].ToString());
                                orderBO.PurchaseOrderTemplate = Convert.ToInt32(reader["PurchaseOrderTemplate"].ToString());
                                orderBO.DeliveryStatus = reader["DeliveryStatus"].ToString();
                                orderBO.CostCenter = reader["CostCenter"].ToString();

                                orderList.Add(orderBO);
                            }
                        }
                    }
                }
            }
            return orderList;
        }
        public List<PMPurchaseOrderDetailsBO> GetSMSalesOrderDetailByOrderId(int orderId)
        {
            List<PMPurchaseOrderDetailsBO> orderDetailsList = new List<PMPurchaseOrderDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSMSalesOrderDetailByOrderId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, orderId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMPurchaseOrderDetailsBO orderDetailsBO = new PMPurchaseOrderDetailsBO();
                                orderDetailsBO.POrderId = orderId;
                                orderDetailsBO.OrderRemarks = reader["OrderRemarks"].ToString();
                                orderDetailsBO.RequisitionId = Int32.Parse(reader["RequisitionId"].ToString());
                                orderDetailsBO.CostCenterId = Int32.Parse(reader["CostCenterId"].ToString());
                                orderDetailsBO.StockById = Int32.Parse(reader["StockById"].ToString());
                                orderDetailsBO.StockBy = reader["HeadName"].ToString();
                                orderDetailsBO.CategoryId = Int32.Parse(reader["CategoryId"].ToString());
                                orderDetailsBO.CategoryName = reader["CategoryName"].ToString();
                                orderDetailsBO.ProductName = reader["ProductName"].ToString();
                                orderDetailsBO.ProductId = Int32.Parse(reader["ItemId"].ToString());
                                orderDetailsBO.Quantity = Convert.ToDecimal(reader["Quantity"].ToString());
                                orderDetailsBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"].ToString());
                                orderDetailsBO.DetailId = Int32.Parse(reader["DetailId"].ToString());
                                orderDetailsBO.StockQuantity = Convert.ToDecimal(reader["StockQuantity"].ToString());
                                orderDetailsBO.BillId = Int64.Parse(reader["BillId"].ToString());
                                orderDetailsBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"].ToString());
                                orderDetailsBO.CashIncentive = Convert.ToDecimal(reader["CashIncentive"].ToString());

                                orderDetailsList.Add(orderDetailsBO);
                            }
                        }
                    }
                }
            }
            return orderDetailsList;
        }
        public PMPurchaseOrderBO GetSMSalesOrderInfoByOrderId(int orderId)
        {
            PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSMSalesOrderInfoByOrderId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, orderId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                orderBO.PONumber = reader["PONumber"].ToString();
                                orderBO.POrderId = Int32.Parse(reader["POrderId"].ToString());
                                orderBO.SupplierId = Int32.Parse(reader["SupplierId"].ToString());
                                orderBO.SupplierName = reader["SupplierName"].ToString();
                                orderBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                orderBO.Remarks = reader["Remarks"].ToString();
                                orderBO.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                orderBO.CreatedDate = reader["CreatedDate"].ToString();
                                orderBO.CheckedBy = Int32.Parse(reader["CheckedBy"].ToString());
                                orderBO.ApprovedBy = Int32.Parse(reader["ApprovedBy"].ToString());
                            }
                        }
                    }
                }
            }
            return orderBO;
        }
        public bool UpdateSMSalesOrderInfo(PMPurchaseOrderBO PurchaseOrder, List<PMPurchaseOrderDetailsBO> AddedPurchaseOrderDetails, List<PMPurchaseOrderDetailsBO> EditedPurchaseOrderDetails, List<PMPurchaseOrderDetailsBO> DeletedPurchaseOrderDetails)
        {
            bool retVal = false;
            int status = 0;
            int tmpOrderId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateSMSalesOrderInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@POrderId", DbType.Int32, PurchaseOrder.POrderId);
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, PurchaseOrder.SupplierId);
                            dbSmartAspects.AddInParameter(commandMaster, "@CostCenterId", DbType.Int32, PurchaseOrder.CostCenterId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, PurchaseOrder.Remarks);
                            dbSmartAspects.AddInParameter(commandMaster, "@ReceivedByDate", DbType.DateTime, PurchaseOrder.ReceivedByDate);
                            //dbSmartAspects.AddInParameter(commandMaster, "@CheckedBy", DbType.Int32, PurchaseOrder.CheckedBy);
                            //dbSmartAspects.AddInParameter(commandMaster, "@ApprovedBy", DbType.Int32, PurchaseOrder.ApprovedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, PurchaseOrder.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            tmpOrderId = PurchaseOrder.POrderId;
                        }

                        if (status > 0 && AddedPurchaseOrderDetails.Count() > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveSMSalesOrderDetailsInfo_SP"))
                            {
                                foreach (PMPurchaseOrderDetailsBO orderDetailBO in AddedPurchaseOrderDetails)
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@CostCenterId", DbType.Int32, orderDetailBO.CostCenterId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@StockById", DbType.Int32, orderDetailBO.StockById);
                                    dbSmartAspects.AddInParameter(commandDetails, "@SOrderId", DbType.Int32, tmpOrderId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ProductId", DbType.Int32, orderDetailBO.ProductId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Quantity", DbType.Decimal, orderDetailBO.Quantity);
                                    dbSmartAspects.AddInParameter(commandDetails, "@PurchasePrice", DbType.Decimal, orderDetailBO.PurchasePrice);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && EditedPurchaseOrderDetails.Count() > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateSMSalesOrderDetailsInfo_SP"))
                            {
                                foreach (PMPurchaseOrderDetailsBO po in EditedPurchaseOrderDetails)
                                {
                                    commandDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDetails, "@DetailId", DbType.Int32, po.DetailId);
                                    //dbSmartAspects.AddInParameter(commandDetails, "@RequisitionId", DbType.Int32, po.RequisitionId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CostCenterId", DbType.Int32, po.CostCenterId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@StockById", DbType.Int32, po.StockById);
                                    dbSmartAspects.AddInParameter(commandDetails, "@POrderId", DbType.Int32, tmpOrderId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ProductId", DbType.Int32, po.ProductId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Quantity", DbType.Decimal, po.Quantity);
                                    dbSmartAspects.AddInParameter(commandDetails, "@PurchasePrice", DbType.Decimal, po.PurchasePrice);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && DeletedPurchaseOrderDetails.Count() > 0)
                        {
                            using (DbCommand commandDeletePO = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (PMPurchaseOrderDetailsBO po in DeletedPurchaseOrderDetails)
                                {
                                    commandDeletePO.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TableName", DbType.String, "SMSalesOrderDetails");
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TablePKField", DbType.String, "DetailId");
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TablePKId", DbType.String, po.DetailId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePO);
                                }
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
        public bool UpdateSMSalesOrderStatus(int orderId, string approvedStatus, int approvedBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSMSalesOrderStatus_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@POrderId", DbType.Int32, orderId);
                    dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, approvedStatus);
                    dbSmartAspects.AddInParameter(command, "@ApprovedBy", DbType.Int32, approvedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;

        }
        public bool DeleteSMSalesOrderDetailInfoByOrderId(int POrderId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteSMSalesOrderDetailInfoByOrderId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@POrderId", DbType.Int32, POrderId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<SMSalesOrderBO> GetSMSaleseOrderInfo(int companyId)
        {
            List<SMSalesOrderBO> orderList = new List<SMSalesOrderBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSMSalesOrderInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SMSalesOrder");
                    DataTable Table = ds.Tables["SMSalesOrder"];

                    orderList = Table.AsEnumerable().Select(r => new SMSalesOrderBO
                    {
                        SOrderId = r.Field<int>("SOrderId"),
                        SONumber = r.Field<string>("SONumber"),
                        CompanyId = r.Field<int>("CompanyId"),
                        CompanyName = r.Field<string>("CompanyName"),
                        CostCenterId = r.Field<int>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                    }).ToList();
                }
            }
            return orderList;
        }
        public bool UpdateSalesdOrder(List<SMSalesOrderBO> salesOrderList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSalesOrderInfo_SP"))
                {
                    foreach (SMSalesOrderBO so in salesOrderList)
                    {
                        command.Parameters.Clear();

                        dbSmartAspects.AddInParameter(command, "@SOrderId", DbType.Int32, so.SOrderId);
                        dbSmartAspects.AddInParameter(command, "@DeliveryStatus", DbType.String, so.DeliveryStatus);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            return status;
        }
        public List<SupplierNCompanyInfoForPurchaseInvoiceBO> GetSalesOrderInfoForInvoice(int sOrderId, int companyId)
        {
            List<SupplierNCompanyInfoForPurchaseInvoiceBO> purchaseInfo = new List<SupplierNCompanyInfoForPurchaseInvoiceBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesOrderDetailsForInvoice_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SOrderId", DbType.Int32, sOrderId);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PurchaseInfo");
                    DataTable Table = ds.Tables["PurchaseInfo"];

                    purchaseInfo = Table.AsEnumerable().Select(r => new SupplierNCompanyInfoForPurchaseInvoiceBO
                    {
                        POrderId = r.Field<Int32>("POrderId"),
                        PONumber = r.Field<string>("PONumber"),
                        PODate = r.Field<DateTime>("PODate"),
                        ReceivedByDate = r.Field<DateTime>("ReceivedByDate"),
                        SupplierId = r.Field<int>("SupplierId"),
                        Remarks = r.Field<string>("Remarks"),
                        ProductId = r.Field<int>("ProductId"),
                        ItemName = r.Field<string>("ItemName"),
                        Quantity = r.Field<decimal>("Quantity"),
                        UnitPrice = r.Field<decimal>("UnitPrice"),
                        TotalPrice = r.Field<decimal>("TotalPrice"),
                        HeadName = r.Field<string>("HeadName")

                    }).ToList();
                }
            }

            return purchaseInfo;
        }
        public List<PurchaseOrderDetailsForInvoiceBO> GetSalesOrderSupplierInformationForInvoice(int supplierId)
        {
            List<PurchaseOrderDetailsForInvoiceBO> purchaseInfo = new List<PurchaseOrderDetailsForInvoiceBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesOrderSupplierInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, supplierId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PurchaseInfo");
                    DataTable Table = ds.Tables["PurchaseInfo"];

                    purchaseInfo = Table.AsEnumerable().Select(r => new PurchaseOrderDetailsForInvoiceBO
                    {
                        CompanyCode = r.Field<string>("CompanyCode"),
                        CompanyName = r.Field<string>("CompanyName"),
                        CompanyAddress = r.Field<string>("CompanyAddress"),
                        EmailAddress = r.Field<string>("EmailAddress"),
                        WebAddress = r.Field<string>("WebAddress"),
                        ContactNumber = r.Field<string>("ContactNumber"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        SupplierName = r.Field<string>("SupplierName"),
                        SupplierCode = r.Field<string>("SupplierCode"),
                        SupplierAddress = r.Field<string>("SupplierAddress"),
                        SupplierPhone = r.Field<string>("SupplierPhone"),
                        SupplierFax = r.Field<string>("SupplierFax"),
                        SupplierEmail = r.Field<string>("SupplierEmail"),
                        SupplierWebAddress = r.Field<string>("SupplierWebAddress"),
                        SupplierContactPerson = r.Field<string>("SupplierContactPerson"),
                        SupplierContactEmail = r.Field<string>("SupplierContactEmail"),
                        SupplierContactPhone = r.Field<string>("SupplierContactPhone")

                    }).ToList();
                }
            }

            return purchaseInfo;
        }

        //----------------------//-----------------------//--------------------------
        public List<RequisitionItemForPurchaseBO> GetRequisitionItemForPurchaseById(int requisitionId, int supplierId, int porderId)
        {
            List<RequisitionItemForPurchaseBO> requisitionList = new List<RequisitionItemForPurchaseBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRequisitionItemForPurchaseById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RequisitionId", DbType.Int32, requisitionId);
                    dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, porderId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RequisitionInfo");
                    DataTable Table = ds.Tables["RequisitionInfo"];

                    requisitionList = Table.AsEnumerable().Select(r => new RequisitionItemForPurchaseBO
                    {
                        RequisitionId = r.Field<Int32>("RequisitionId"),
                        RequisitionDetailsId = r.Field<Int64>("RequisitionDetailsId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        RequisitionQuantity = r.Field<decimal>("RequisitionQuantity"),
                        ApprovedQuantity = r.Field<decimal>("ApprovedQuantity"),
                        UnitName = r.Field<string>("UnitName"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        UnitPriceUsd = r.Field<decimal>("UnitPriceUsd"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice"),
                        LastPurchaseDate = r.Field<DateTime?>("LastPurchaseDate"),


                        ColorId = r.Field<Int32>("ColorId"),
                        SizeId = r.Field<Int32>("SizeId"),
                        StyleId = r.Field<Int32>("StyleId"),
                        ColorText = r.Field<string>("ColorText"),
                        SizeText = r.Field<string>("SizeText"),
                        StyleText = r.Field<string>("StyleText"),


                        Remarks = r.Field<string>("Remarks"),
                        StockById = r.Field<Int32>("StockById"),
                        POrderId = r.Field<Int32>("POrderId"),
                        PODetailsId = r.Field<Int32>("PODetailsId"),
                        SupplierId = r.Field<Int32?>("SupplierId"),
                        ApprovedPOQuantity = r.Field<decimal>("ApprovedPOQuantity"),
                        RemainingPOQuantity = r.Field<decimal>("RemainingPOQuantity")

                    }).ToList();
                }
            }

            return requisitionList;
        }
        public bool PurchaseOrderApproval(string poType, int pOrderId, string approvedStatus, int checkedOrApprovedBy, out string TransactionNo, out string TransactionType, out string ApproveStatus)
        {
            bool retVal = false;
            int status = 0;
            string updateStatus = "";

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("PurchaseOrderApproval_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@POType", DbType.String, poType);
                            dbSmartAspects.AddInParameter(commandMaster, "@POrderId", DbType.Int32, pOrderId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, checkedOrApprovedBy);
                            dbSmartAspects.AddOutParameter(commandMaster, "@UpdateStatus", DbType.String, 50);

                            dbSmartAspects.AddOutParameter(commandMaster, "@TransactionNo", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(commandMaster, "@TransactionType", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(commandMaster, "@ApproveStatus", DbType.String, 100);



                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            updateStatus = Convert.ToString(commandMaster.Parameters["@UpdateStatus"].Value);

                            TransactionNo = commandMaster.Parameters["@TransactionNo"].Value.ToString();
                            TransactionType = commandMaster.Parameters["@TransactionType"].Value.ToString();
                            ApproveStatus = commandMaster.Parameters["@ApproveStatus"].Value.ToString();
                        }

                        if (status > 0 && poType == "Requisition" && updateStatus == "Approved")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdatePOStatusForRequisitionWisePurchase_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@POrderId", DbType.Int32, pOrderId);
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
        public bool PurchaseOrderDelete(string pOType, int pOrderId, string approvedStatus, int createdBy, int lastModifiedBy, out string TransactionNo, out string TransactionType, out string ApproveStatus)
        {
            bool retVal = false;
            int status = 0;
            TransactionNo = "";
            TransactionType = "";
            ApproveStatus = "";

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if (pOType == "Requisition")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdatePOStatusForRequisitionWisePurchaseDelete_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@POrderId", DbType.Int32, pOrderId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }

                            if (status > 0)
                            {
                                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("PurchaseOrderDelete_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandMaster, "@POType", DbType.String, pOType);
                                    dbSmartAspects.AddInParameter(commandMaster, "@POrderId", DbType.Int32, pOrderId);
                                    dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                                    dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, createdBy);
                                    dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                                    dbSmartAspects.AddOutParameter(commandMaster, "@TransactionNo", DbType.String, 100);
                                    dbSmartAspects.AddOutParameter(commandMaster, "@TransactionType", DbType.String, 100);
                                    dbSmartAspects.AddOutParameter(commandMaster, "@ApproveStatus", DbType.String, 100);



                                    status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);


                                    TransactionNo = commandMaster.Parameters["@TransactionNo"].Value.ToString();
                                    TransactionType = commandMaster.Parameters["@TransactionType"].Value.ToString();
                                    ApproveStatus = commandMaster.Parameters["@ApproveStatus"].Value.ToString();


                                }
                            }
                        }
                        else
                        {
                            using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("PurchaseOrderDelete_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@POType", DbType.String, pOType);
                                dbSmartAspects.AddInParameter(commandMaster, "@POrderId", DbType.Int32, pOrderId);
                                dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                                dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, createdBy);
                                dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                                dbSmartAspects.AddOutParameter(commandMaster, "@TransactionNo", DbType.String, 100);
                                dbSmartAspects.AddOutParameter(commandMaster, "@TransactionType", DbType.String, 100);
                                dbSmartAspects.AddOutParameter(commandMaster, "@ApproveStatus", DbType.String, 100);



                                status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);


                                TransactionNo = commandMaster.Parameters["@TransactionNo"].Value.ToString();
                                TransactionType = commandMaster.Parameters["@TransactionType"].Value.ToString();
                                ApproveStatus = commandMaster.Parameters["@ApproveStatus"].Value.ToString();
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
        public List<PMPurchaseOrderBO> GetAllPurchaseOrder()
        {
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllPurchaseOrder_SP"))
                {

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
                                orderBO.POrderId = Int32.Parse(reader["POrderId"].ToString());
                                orderBO.PONumber = reader["PONumber"].ToString();
                                orderList.Add(orderBO);
                            }
                        }
                    }
                }
            }
            return orderList;
        }

        public bool SavePMPurchaseOrderTermsNConditions(List<PMPurchaseOrderTermsNConditionBO> TermsNCondition, List<PMPurchaseOrderTermsNConditionBO> deletedTermsNConditions, int PurchaseId)
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
                        if (TermsNCondition.Count > 0)
                        {
                            using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateTermsNConditionsForPO_SP"))
                            {
                                foreach (PMPurchaseOrderTermsNConditionBO item in TermsNCondition)
                                {
                                    commandMaster.Parameters.Clear();
                                    //dbSmartAspects.AddInParameter(commandDetails, "@RequisitionId", DbType.Int32, orderDetailBO.RequisitionId);

                                    dbSmartAspects.AddInParameter(commandMaster, "@Id", DbType.Int32, item.Id);
                                    dbSmartAspects.AddInParameter(commandMaster, "@PurchaseId", DbType.Int32, PurchaseId);
                                    dbSmartAspects.AddInParameter(commandMaster, "@TermsNConditionsId", DbType.Int32, item.TermsNConditionsId);
                                    dbSmartAspects.AddInParameter(commandMaster, "@Title", DbType.String, item.Title);
                                    dbSmartAspects.AddInParameter(commandMaster, "@DisplaySequence", DbType.Int32, item.DisplaySequence);
                                    dbSmartAspects.AddInParameter(commandMaster, "@Description", DbType.String, item.Description);

                                    status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                                }
                            }
                        }
                        if (deletedTermsNConditions.Count > 0)
                        {
                            using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("DeleteTermsNConditionsForPO_SP"))
                            {
                                foreach (PMPurchaseOrderTermsNConditionBO deletedTtem in deletedTermsNConditions)
                                {
                                    commandMaster.Parameters.Clear();
                                    //dbSmartAspects.AddInParameter(commandDetails, "@RequisitionId", DbType.Int32, orderDetailBO.RequisitionId);

                                    dbSmartAspects.AddInParameter(commandMaster, "@PurchaseId", DbType.Int32, PurchaseId);
                                    dbSmartAspects.AddInParameter(commandMaster, "@TermsNConditionsId", DbType.Int32, deletedTtem.TermsNConditionsId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                                }
                            }
                        }
                        transction.Commit();
                        retVal = true;
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

        public List<PMPurchaseOrderTermsNConditionBO> GetTermsNConditionsByPurchaseOrderId(int Id)
        {
            List<PMPurchaseOrderTermsNConditionBO> TNCList = new List<PMPurchaseOrderTermsNConditionBO>();
            string query = string.Format("SELECT * FROM PMPurchaseOrderTermsNConditions WHERE PurchaseId = {0}", Id);

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
                                    PMPurchaseOrderTermsNConditionBO TNC = new PMPurchaseOrderTermsNConditionBO();
                                    TNC.Id = Convert.ToInt64(reader["Id"]);
                                    TNC.Title = (reader["Title"].ToString());
                                    TNC.Description = (reader["Description"].ToString());
                                    TNC.PurchaseId = Convert.ToInt32(reader["PurchaseId"]);
                                    TNC.TermsNConditionsId = Convert.ToInt64(reader["TermsNConditionsId"]);
                                    TNC.DisplaySequence = Convert.ToInt32(reader["DisplaySequence"]);
                                    TNCList.Add(TNC);
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

            return TNCList;

        }
        public List<PMPurchaseOrderTermsNConditionBO> GetTermsNConditionsByPOId(int id)
        {
            List<PMPurchaseOrderTermsNConditionBO> TNCList = new List<PMPurchaseOrderTermsNConditionBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTermsNConditionsForPOReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PurchaseId", DbType.Int32, id);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMPurchaseOrderTermsNConditionBO TNC = new PMPurchaseOrderTermsNConditionBO();
                                TNC.Id = Convert.ToInt64(reader["Id"]);
                                TNC.PurchaseId = Convert.ToInt32(reader["PurchaseId"]);
                                TNC.TermsNConditionsId = Convert.ToInt64(reader["TermsNConditionsId"]);
                                TNC.DisplaySequence = Convert.ToInt32(reader["DisplaySequence"]);
                                TNC.Title = reader["Title"].ToString();
                                TNC.Description = reader["Description"].ToString();
                                TNCList.Add(TNC);
                            }
                        }
                    }
                }
            }
            return TNCList;
        }
        public bool ReOpenPurchaseOrder(int orderId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ReOpenPurchaseOrder_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@POrderId", DbType.Int32, orderId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<PMPurchaseOrderBO> GetSMSalesOrderInformation(DateTime FromDate, DateTime ToDate, string PONumber, Int32 costCenterId, Int32 productId, string status)
        {
            List<PMPurchaseOrderBO> orderList = new List<PMPurchaseOrderBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSMSalesOrderInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, FromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, ToDate);
                    if (!string.IsNullOrEmpty(PONumber))
                        dbSmartAspects.AddInParameter(cmd, "@SONumber", DbType.String, PONumber);
                    else dbSmartAspects.AddInParameter(cmd, "@SONumber", DbType.String, DBNull.Value);

                    if (costCenterId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (productId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProductId", DbType.Int32, productId);
                    else dbSmartAspects.AddInParameter(cmd, "@ProductId", DbType.Int32, DBNull.Value);

                    if (status != "All")
                        dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, status);
                    else dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, DBNull.Value);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMPurchaseOrderBO orderBO = new PMPurchaseOrderBO();
                                orderBO.POrderId = Int32.Parse(reader["POrderId"].ToString());
                                orderBO.SupplierId = Int32.Parse(reader["CompanyId"].ToString());
                                orderBO.SupplierName = reader["CompanyName"].ToString();
                                orderBO.PONumber = reader["PONumber"].ToString();
                                orderBO.ItemCode = reader["ItemCode"].ToString();
                                orderBO.ItemName = reader["ItemName"].ToString();
                                orderBO.StockByHead = reader["StockByHead"].ToString();
                                orderBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());
                                orderBO.Quantity = Convert.ToDecimal(reader["Quantity"].ToString());
                                orderBO.CreatedDate = reader["CreatedDate"].ToString();
                                orderBO.CheckedBy = Int32.Parse(reader["CheckedBy"].ToString());
                                orderBO.ApprovedBy = Int32.Parse(reader["ApprovedBy"].ToString());
                                orderBO.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                //orderBO.ReceivedByDate = Convert.ToDateTime(reader["DeliveryDate"].ToString());
                                orderBO.PurchaseOrderTemplate = Convert.ToInt32(reader["PurchaseOrderTemplate"].ToString());
                                orderBO.DeliveryStatus = reader["DeliveryStatus"].ToString();
                                orderBO.CostCenter = reader["CostCenter"].ToString();
                                orderBO.Remarks = reader["Remarks"].ToString();
                                orderBO.PODateDisplay = reader["PODateDisplay"].ToString();
                                orderBO.DeliveryDateDisplay = reader["DeliveryDateDisplay"].ToString();

                                orderList.Add(orderBO);
                            }
                        }
                    }
                }
            }
            return orderList;
        }
        public bool UpdateSalesdOrderForBillId(List<SMSalesOrderBO> salesOrderList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSalesdOrderForBillId_SP"))
                {
                    foreach (SMSalesOrderBO so in salesOrderList)
                    {
                        command.Parameters.Clear();

                        dbSmartAspects.AddInParameter(command, "@SOrderId", DbType.Int32, so.SOrderId);
                        dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, so.BillId);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            return status;
        }
        public List<SMSalesOrderBO> GetSMSaleseOrderInformation(DateTime fromDate, DateTime toDate, string costCenterId, string sONumber)
        {
            List<SMSalesOrderBO> orderList = new List<SMSalesOrderBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSMSaleseOrderInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.String, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@SONumber", DbType.String, sONumber);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SMSalesOrder");
                    DataTable Table = ds.Tables["SMSalesOrder"];

                    orderList = Table.AsEnumerable().Select(r => new SMSalesOrderBO
                    {
                        SOrderId = r.Field<int>("SOrderId"),
                        SONumber = r.Field<string>("SONumber"),
                        CompanyId = r.Field<int>("CompanyId"),
                        CompanyName = r.Field<string>("CompanyName"),
                        CostCenterId = r.Field<int>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter")
                    }).ToList();
                }
            }
            return orderList;
        }
        public List<SMSalesOrderBO> GetAccountManagerSalesTargetReport(Int32 fiscalYearId)
        {
            List<SMSalesOrderBO> orderList = new List<SMSalesOrderBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAccountManagerSalesTargetReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FiscalYearId", DbType.Int32, fiscalYearId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMSalesOrderBO orderBO = new SMSalesOrderBO();
                                orderBO.EmpId = Int32.Parse(reader["EmpId"].ToString());
                                orderBO.EmployeeName = reader["EmployeeName"].ToString();
                                orderBO.MonthId = Int32.Parse(reader["MonthId"].ToString());
                                orderBO.MonthName = reader["MonthName"].ToString();
                                orderBO.MonthTarget = Convert.ToDecimal(reader["MonthTarget"].ToString());
                                orderBO.MonthAchievement = Convert.ToDecimal(reader["MonthAchievement"].ToString());

                                orderList.Add(orderBO);
                            }
                        }
                    }
                }
            }
            return orderList;
        }
    }
}