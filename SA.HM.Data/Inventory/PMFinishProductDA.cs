using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.PurchaseManagment;

namespace HotelManagement.Data.PurchaseManagment
{
    public class PMFinishProductDA : BaseService
    {
        public bool SaveFinishGoods(FinishedProductBO finishedProduct, List<FinishedProductDetailsBO> AddedFinishGoods, out int finishProductId)
        {
            int status = 0;
            finishProductId = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("SaveFinishGoods_SP"))
                        {
                            commandOut.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandOut, "@OrderDate", DbType.DateTime, finishedProduct.OrderDate);
                            dbSmartAspects.AddInParameter(commandOut, "@CostCenterId", DbType.Int32, finishedProduct.CostCenterId);
                            dbSmartAspects.AddInParameter(commandOut, "@ApprovedStatus", DbType.String, finishedProduct.ApprovedStatus);
                            dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, finishedProduct.Remarks);
                            dbSmartAspects.AddInParameter(commandOut, "@CreatedBy", DbType.Int32, finishedProduct.CreatedBy);

                            dbSmartAspects.AddOutParameter(commandOut, "@FinishProductId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);

                            finishProductId = Convert.ToInt32(commandOut.Parameters["@FinishProductId"].Value);
                        }

                        if (status > 0 && AddedFinishGoods.Count > 0)
                        {
                            foreach (FinishedProductDetailsBO fp in AddedFinishGoods)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveFinishGoodsDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@FinishProductId", DbType.Int32, finishProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductId", DbType.Int32, fp.ProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, fp.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, fp.Quantity);

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
        public bool UpdateFinishGoods(FinishedProductBO finishedProduct, List<FinishedProductDetailsBO> AddedFinishGoods, List<FinishedProductDetailsBO> EditedFinishGoods, List<FinishedProductDetailsBO> DeletedFinishGoods)
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
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("UpdateFinishGoods_SP"))
                        {
                            commandOut.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandOut, "@FinishProductId", DbType.Int32, finishedProduct.FinishProductId);
                            dbSmartAspects.AddInParameter(commandOut, "@OrderDate", DbType.DateTime, finishedProduct.OrderDate);
                            dbSmartAspects.AddInParameter(commandOut, "@CostCenterId", DbType.Int32, finishedProduct.CostCenterId);
                            dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, finishedProduct.Remarks);
                            dbSmartAspects.AddInParameter(commandOut, "@LastModifiedBy", DbType.Int32, finishedProduct.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);
                        }

                        if (status > 0 && AddedFinishGoods.Count > 0)
                        {
                            foreach (FinishedProductDetailsBO fp in AddedFinishGoods)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveFinishGoodsDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@FinishProductId", DbType.Int32, finishedProduct.FinishProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductId", DbType.Int32, fp.ProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, fp.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, fp.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && EditedFinishGoods.Count > 0)
                        {
                            foreach (FinishedProductDetailsBO fp in EditedFinishGoods)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateFinishGoodsDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@FinishedProductDetailsId", DbType.Int32, fp.FinishedProductDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@FinishProductId", DbType.Int32, fp.FinishProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductId", DbType.Int32, fp.ProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, fp.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, fp.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && DeletedFinishGoods.Count > 0)
                        {
                            foreach (FinishedProductDetailsBO fp in DeletedFinishGoods)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("DeleteFinishGoodsDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@FinishedProductDetailsId", DbType.Int32, fp.FinishedProductDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@FinishProductId", DbType.Int32, fp.FinishProductId);

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
        public bool DeleteFinishGoodsOrder(int finishedProductId)
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
                        using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("DeleteFinishGoodsOrder_SP"))
                        {
                            cmdOutDetails.Parameters.Clear();

                            dbSmartAspects.AddInParameter(cmdOutDetails, "@FinishProductId", DbType.Int32, finishedProductId);

                            status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
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
        public bool UpdateFinishGoodsOrderStatus(FinishedProductBO finishedProduct)
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
                        using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateFinishGoodsOrderStatus_SP"))
                        {
                            cmdOutDetails.Parameters.Clear();

                            dbSmartAspects.AddInParameter(cmdOutDetails, "@FinishProductId", DbType.Int32, finishedProduct.FinishProductId);
                            dbSmartAspects.AddInParameter(cmdOutDetails, "@ApprovedStatus", DbType.String, finishedProduct.ApprovedStatus);

                            status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
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
        public List<FinishedProductBO> GetFinishedProductSearch(int costCenterId, DateTime? dateFrom, DateTime? dateTo)
        {
            List<FinishedProductBO> finishGoods = new List<FinishedProductBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFinishedProductSearch_SP"))
                {
                    if (costCenterId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "FinishGoods");
                    DataTable Table = ds.Tables["FinishGoods"];

                    finishGoods = Table.AsEnumerable().Select(r => new FinishedProductBO
                    {
                        FinishProductId = r.Field<Int32>("FinishProductId"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        OrderDate = r.Field<DateTime>("OrderDate"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                }
            }

            return finishGoods;
        }
        public FinishedProductBO GetFinishedProductById(int finishedProductId)
        {
            FinishedProductBO finishGoods = new FinishedProductBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFinishedProductById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FinishProductId", DbType.Int32, finishedProductId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "FinishGoods");
                    DataTable Table = ds.Tables["FinishGoods"];

                    finishGoods = Table.AsEnumerable().Select(r => new FinishedProductBO
                    {
                        ProductId = r.Field<Int64>("FinishProductId"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        Remarks = r.Field<string>("Remarks")

                    }).FirstOrDefault();
                }
            }

            return finishGoods;
        }
        public List<FinishedProductDetailsBO> GetFinishedProductDetailsById(int finishProductId)
        {
            List<FinishedProductDetailsBO> finishGoodsDetails = new List<FinishedProductDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFinishedProductDetails_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FinishProductId", DbType.Int32, finishProductId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "FinishGoods");
                    DataTable Table = ds.Tables["FinishGoods"];

                    finishGoodsDetails = Table.AsEnumerable().Select(r => new FinishedProductDetailsBO
                    {
                        FinishedProductDetailsId = r.Field<Int32>("FinishedProductDetailsId"),
                        FinishProductId = r.Field<Int32>("FinishProductId"),
                        ProductId = r.Field<Int32>("ProductId"),
                        ProductName = r.Field<string>("ProductName"),
                        StockById = r.Field<Int32>("StockById"),
                        StockBy = r.Field<string>("StockBy"),
                        Quantity = r.Field<decimal>("Quantity")

                    }).ToList();
                }
            }

            return finishGoodsDetails;
        }
        public List<FinishedProductDetailsBO> GetInvProductionFGDetailsById(int productionId)
        {
            List<FinishedProductDetailsBO> finishGoodsDetails = new List<FinishedProductDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvProductionFGDetailsById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FinishProductId", DbType.Int32, productionId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "FinishGoods");
                    DataTable Table = ds.Tables["FinishGoods"];

                    finishGoodsDetails = Table.AsEnumerable().Select(r => new FinishedProductDetailsBO
                    {
                        LocationId = r.Field<Int32>("LocationId"),
                        LocationName = r.Field<string>("LocationName"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ItemCode = r.Field<string>("ItemCode"),
                        ItemName = r.Field<string>("ItemName"),
                        ColorId = r.Field<Int32>("ColorId"),
                        ColorName = r.Field<string>("ColorName"),
                        SizeId = r.Field<Int32>("SizeId"),
                        SizeName = r.Field<string>("SizeName"),
                        StyleId = r.Field<Int32>("StyleId"),
                        StyleName = r.Field<string>("StyleName"),
                        StockById = r.Field<Int32>("StockById"),
                        UnitName = r.Field<string>("UnitName"),
                        Quantity = r.Field<decimal>("Quantity")

                    }).ToList();
                }
            }

            return finishGoodsDetails;
        }
        public List<FinishedProductDetailsBO> GetInvProductionRMDetailsById(int productionId)
        {
            List<FinishedProductDetailsBO> finishGoodsDetails = new List<FinishedProductDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFinishedProductRMDetails_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FinishProductId", DbType.Int32, productionId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "FinishGoods");
                    DataTable Table = ds.Tables["FinishGoods"];

                    finishGoodsDetails = Table.AsEnumerable().Select(r => new FinishedProductDetailsBO
                    {
                        LocationId = r.Field<Int32>("LocationId"),
                        LocationName = r.Field<string>("LocationName"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ItemCode = r.Field<string>("ItemCode"),
                        ItemName = r.Field<string>("ItemName"),
                        ColorId = r.Field<Int32>("ColorId"),
                        ColorName = r.Field<string>("ColorName"),
                        SizeId = r.Field<Int32>("SizeId"),
                        SizeName = r.Field<string>("SizeName"),
                        StyleId= r.Field<Int32>("StyleId"),
                        StyleName = r.Field<string>("StyleName"),
                        StockById = r.Field<Int32>("StockById"),
                        UnitName = r.Field<string>("UnitName"),
                        Quantity = r.Field<decimal>("Quantity")

                    }).ToList();
                }
            }

            return finishGoodsDetails;
        }

        public List<OverheadExpensesBO> GetInvProductionOEDetailsById(int productionId)
        {
            List<OverheadExpensesBO> overheadDetails = new List<OverheadExpensesBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvProductionOEDetails_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FinishProductId", DbType.Int32, productionId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "FinishGoods");
                    DataTable Table = ds.Tables["FinishGoods"];

                    overheadDetails = Table.AsEnumerable().Select(r => new OverheadExpensesBO
                    {
                        NodeId = r.Field<Int32>("NodeId"),
                        AccountHead = r.Field<string>("AccountHead"),
                        Amount = r.Field<decimal>("OEAmount"),
                        Remarks = r.Field<string>("OERemarks")

                    }).ToList();
                }
            }
            return overheadDetails;
        }

        public List<FinishedProductBO> GetInventoryProductionSearch(int costCenterId, DateTime? dateFrom, DateTime? dateTo, string productionId, string status, int userInfoId)
        {
            List<FinishedProductBO> finishGoods = new List<FinishedProductBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInventoryProductionSearch_SP"))
                {
                    if (costCenterId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    if (productionId != null)
                        dbSmartAspects.AddInParameter(cmd, "@ProductionId", DbType.String, productionId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProductionId", DbType.String, DBNull.Value);

                    if (status != "All")
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, status);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "FinishGoods");
                    DataTable Table = ds.Tables["FinishGoods"];

                    finishGoods = Table.AsEnumerable().Select(r => new FinishedProductBO
                    {
                        Id = r.Field<Int64>("Id"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        ProductionDate = r.Field<DateTime>("ProductionDate"),
                        ProductionDateDisplay = r.Field<string>("ProductionDateDisplay"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CheckedBy = r.Field<Int32>("CheckedBy"),
                        ApprovedBy = r.Field<Int32>("ApprovedBy"),
                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanDelete = r.Field<bool>("IsCanDelete"),
                        IsCanChecked = r.Field<bool>("IsCanChecked"),
                        IsCanApproved = r.Field<bool>("IsCanApproved")

                    }).ToList();
                }
            }

            return finishGoods;
        }
        public bool SaveInventoryProduction(FinishedProductBO finishedProduct, List<FinishedProductDetailsBO> AddedRMGoods, List<FinishedProductDetailsBO> AddedFinishGoods, List<OverheadExpensesBO> AddedOverheadExpenses)
        {
            Int64 status = 0, productionId = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("SaveInvProduction_SP"))
                        {
                            commandOut.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandOut, "@ProductionDate", DbType.DateTime, finishedProduct.OrderDate);
                            dbSmartAspects.AddInParameter(commandOut, "@CostCenterId", DbType.Int32, finishedProduct.CostCenterId);
                            dbSmartAspects.AddInParameter(commandOut, "@ApprovedStatus", DbType.String, finishedProduct.ApprovedStatus);
                            dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, finishedProduct.Remarks);
                            dbSmartAspects.AddInParameter(commandOut, "@CreatedBy", DbType.Int32, finishedProduct.CreatedBy);

                            dbSmartAspects.AddOutParameter(commandOut, "@ProductionId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);

                            productionId = Convert.ToInt64(commandOut.Parameters["@ProductionId"].Value);
                        }

                        if (status > 0 && AddedFinishGoods.Count > 0)
                        {
                            foreach (FinishedProductDetailsBO fp in AddedFinishGoods)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveInvProductionFGDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductionId", DbType.Int64, productionId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationId", DbType.Int32, fp.LocationId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, fp.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, fp.ColorId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, fp.SizeId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, fp.StyleId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, fp.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, fp.Quantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@UnitPrice", DbType.Decimal, fp.UnitPrice);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@BagQuantity", DbType.Int32, fp.BagQuantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && AddedRMGoods.Count > 0)
                        {
                            foreach (FinishedProductDetailsBO fp in AddedRMGoods)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveInvProductionRMDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductionId", DbType.Int64, productionId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationId", DbType.Int32, fp.LocationId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, fp.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, fp.ColorId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, fp.SizeId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, fp.StyleId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, fp.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, fp.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }
                        if(status > 0 && AddedOverheadExpenses.Count > 0)
                        {
                            foreach(OverheadExpensesBO oe in AddedOverheadExpenses)
                            {
                                using (DbCommand oEDetails = dbSmartAspects.GetStoredProcCommand("SaveInvProductionOEDetails_SP"))
                                {
                                    oEDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(oEDetails, "@ProductionId", DbType.Int64, productionId);
                                    dbSmartAspects.AddInParameter(oEDetails, "@NodeId", DbType.Int32, oe.NodeId);
                                    dbSmartAspects.AddInParameter(oEDetails, "@Amount", DbType.Decimal, oe.Amount);
                                    dbSmartAspects.AddInParameter(oEDetails, "@Remarks", DbType.String, oe.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(oEDetails, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdaateProductionWiseItemCosting_SP"))
                            {
                                cmdOutDetails.Parameters.Clear();
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductionId", DbType.Int64, productionId);
                                dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
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
                    catch
                    {
                        retVal = false;
                        transction.Rollback();
                    }
                }
            }

            return retVal;
        }
        public bool UpdateInventoryProduction(FinishedProductBO finishedProduct, List<FinishedProductDetailsBO> AddedFinishGoods, List<FinishedProductDetailsBO> EditedFinishGoods, List<FinishedProductDetailsBO> DeletedFinishGoods, List<FinishedProductDetailsBO> AddedRMGoods, List<FinishedProductDetailsBO> EditedRMGoods, List<FinishedProductDetailsBO> DeletedRMGoods, List<OverheadExpensesBO> AddedOverheadExpenses)
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
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("UpdateInvProduction_SP"))
                        {
                            commandOut.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandOut, "@FinishProductId", DbType.Int32, finishedProduct.FinishProductId);
                            dbSmartAspects.AddInParameter(commandOut, "@OrderDate", DbType.DateTime, finishedProduct.OrderDate);
                            dbSmartAspects.AddInParameter(commandOut, "@CostCenterId", DbType.Int32, finishedProduct.CostCenterId);
                            dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, finishedProduct.Remarks);
                            dbSmartAspects.AddInParameter(commandOut, "@LastModifiedBy", DbType.Int32, finishedProduct.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);
                        }

                        if (status > 0 && AddedFinishGoods.Count > 0)
                        {
                            using (DbCommand cmdFGDeleteDetails = dbSmartAspects.GetStoredProcCommand("DeleteProductionFGDetailsByProductionId_SP"))
                            {
                                cmdFGDeleteDetails.Parameters.Clear();
                                dbSmartAspects.AddInParameter(cmdFGDeleteDetails, "@FinishProductId", DbType.Int32, finishedProduct.FinishProductId);
                                status = dbSmartAspects.ExecuteNonQuery(cmdFGDeleteDetails, transction);
                            }
                            foreach (FinishedProductDetailsBO fp in AddedFinishGoods)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveInvProductionFGDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductionId", DbType.Int32, finishedProduct.FinishProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationId", DbType.Int32, fp.LocationId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, fp.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, fp.ColorId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, fp.SizeId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, fp.StyleId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, fp.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, fp.Quantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@UnitPrice", DbType.Decimal, fp.UnitPrice);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@BagQuantity", DbType.Int32, fp.BagQuantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && EditedFinishGoods.Count > 0)
                        {
                            foreach (FinishedProductDetailsBO fp in EditedFinishGoods)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateFinishGoodsFGDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@FinishedProductDetailsId", DbType.Int32, fp.FinishedProductDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@FinishProductId", DbType.Int32, fp.FinishProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationId", DbType.Int32, fp.LocationId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductId", DbType.Int32, fp.ProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, fp.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, fp.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && DeletedFinishGoods.Count > 0)
                        {
                            foreach (FinishedProductDetailsBO fp in DeletedFinishGoods)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("DeleteFinishGoodsFGDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@FinishedProductDetailsId", DbType.Int32, fp.FinishedProductDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@FinishProductId", DbType.Int32, fp.FinishProductId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && AddedRMGoods.Count > 0)
                        {
                            using (DbCommand cmdRMDeleteDetails = dbSmartAspects.GetStoredProcCommand("DeleteProductionRMDetailsByProductionId_SP"))
                            {
                                cmdRMDeleteDetails.Parameters.Clear();
                                dbSmartAspects.AddInParameter(cmdRMDeleteDetails, "@FinishProductId", DbType.Int32, finishedProduct.FinishProductId);
                                status = dbSmartAspects.ExecuteNonQuery(cmdRMDeleteDetails, transction);
                            }
                            foreach (FinishedProductDetailsBO fp in AddedRMGoods)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveInvProductionRMDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductionId", DbType.Int32, finishedProduct.FinishProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationId", DbType.Int32, fp.LocationId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, fp.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, fp.ColorId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, fp.SizeId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, fp.StyleId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, fp.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, fp.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && EditedRMGoods.Count > 0)
                        {
                            foreach (FinishedProductDetailsBO fp in EditedRMGoods)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateFinishGoodsRMDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@FinishedProductDetailsId", DbType.Int32, fp.FinishedProductDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@FinishProductId", DbType.Int32, fp.FinishProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationId", DbType.Int32, fp.LocationId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductId", DbType.Int32, fp.ProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, fp.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, fp.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && DeletedRMGoods.Count > 0)
                        {
                            foreach (FinishedProductDetailsBO fp in DeletedRMGoods)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("DeleteFinishGoodsRMDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@FinishedProductDetailsId", DbType.Int32, fp.FinishedProductDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@FinishProductId", DbType.Int32, fp.FinishProductId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && AddedOverheadExpenses.Count > 0)
                        {
                            using (DbCommand cmdOEDeleteDetails = dbSmartAspects.GetStoredProcCommand("DeleteProductionOEDetailsByProductionId_SP"))
                            {
                                cmdOEDeleteDetails.Parameters.Clear();
                                dbSmartAspects.AddInParameter(cmdOEDeleteDetails, "@FinishProductId", DbType.Int32, finishedProduct.FinishProductId);
                                status = dbSmartAspects.ExecuteNonQuery(cmdOEDeleteDetails, transction);
                            }
                            foreach (OverheadExpensesBO oe in AddedOverheadExpenses)
                            {
                                using (DbCommand oEDetails = dbSmartAspects.GetStoredProcCommand("SaveInvProductionOEDetails_SP"))
                                {
                                    oEDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(oEDetails, "@ProductionId", DbType.Int64, finishedProduct.FinishProductId);
                                    dbSmartAspects.AddInParameter(oEDetails, "@NodeId", DbType.Int32, oe.NodeId);
                                    dbSmartAspects.AddInParameter(oEDetails, "@Amount", DbType.Decimal, oe.Amount);
                                    dbSmartAspects.AddInParameter(oEDetails, "@Remarks", DbType.String, oe.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(oEDetails, transction);
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
                    catch
                    {
                        retVal = false;
                        transction.Rollback();
                    }
                }
            }

            return retVal;
        }

        public bool DeleteInventoryProduction(int productionId)
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
                        using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("DeleteInventoryProduction_SP"))
                        {
                            cmdOutDetails.Parameters.Clear();

                            dbSmartAspects.AddInParameter(cmdOutDetails, "@Id", DbType.Int64, productionId);

                            status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
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
        public bool InventoryProductionApproval(long productionId, string approvedStatus)
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
                        using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("InventoryProductionApproval_SP"))
                        {
                            cmdOutDetails.Parameters.Clear();

                            dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductionId", DbType.Int64, productionId);
                            dbSmartAspects.AddInParameter(cmdOutDetails, "@ApprovedStatus", DbType.String, approvedStatus);

                            status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
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
                    catch
                    {
                        retVal = false;
                        transction.Rollback();
                    }
                }
            }

            return retVal;
        }
        public List<FinishedProductDetailsBO> GetInvProductionRMInformation(int productionId)
        {
            List<FinishedProductDetailsBO> finishGoodsDetails = new List<FinishedProductDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvProductionRMInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProductionId", DbType.Int32, productionId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RawMaterials");
                    DataTable Table = ds.Tables["RawMaterials"];

                    finishGoodsDetails = Table.AsEnumerable().Select(r => new FinishedProductDetailsBO
                    {
                        ProductDetailsId = r.Field<Int64>("ProductDetailsId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        ProductionId = r.Field<Int64>("ProductionId"),
                        ProductionDate = r.Field<DateTime>("ProductionDate"),
                        ProductionDateString = r.Field<string>("ProductionDateString"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        StockById = r.Field<Int32>("StockById"),
                        StockBy = r.Field<string>("StockBy"),
                        Quantity = r.Field<decimal>("Quantity"),
                        UnitCost = r.Field<decimal>("UnitCost"),
                        TotalCost = r.Field<decimal>("TotalCost"),
                        ProductType = r.Field<string>("ProductType"),
                        Remarks = r.Field<string>("Remarks"),
                        PercentageValue = r.Field<decimal>("PercentageValue")
                    }).ToList();
                }
            }

            return finishGoodsDetails;
        }
        public List<FinishedProductDetailsBO> GetInvProductionInformation(int productionId)
        {
            List<FinishedProductDetailsBO> finishGoodsDetails = new List<FinishedProductDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvProductionInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProductionId", DbType.Int32, productionId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "FinishGoods");
                    DataTable Table = ds.Tables["FinishGoods"];

                    finishGoodsDetails = Table.AsEnumerable().Select(r => new FinishedProductDetailsBO
                    {
                        ProductDetailsId = r.Field<Int64>("ProductDetailsId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        ProductionId = r.Field<Int64>("ProductionId"),
                        ProductionDate = r.Field<DateTime>("ProductionDate"),
                        ProductionDateString = r.Field<string>("ProductionDateString"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        StockById = r.Field<Int32>("StockById"),
                        StockBy = r.Field<string>("StockBy"),
                        Quantity = r.Field<decimal>("Quantity"),
                        UnitCost = r.Field<decimal>("UnitCost"),
                        TotalCost = r.Field<decimal>("TotalCost"),
                        ProductType = r.Field<string>("ProductType"),
                        Remarks = r.Field<string>("Remarks"),
                        PercentageValue = r.Field<decimal>("PercentageValue"),
                        LocationId = r.Field<Int32>("LocationId"),
                        ColorId = r.Field<Int32>("ColorId"),
                        SizeId = r.Field<Int32>("SizeId"),
                        StyleId = r.Field<Int32>("StyleId"),
                        SalesRatio = r.Field<decimal>("SalesRatio"),
                        TotalSales = r.Field<decimal>("TotalSales"),
                        SalesWiseFGCost = r.Field<decimal>("SalesWiseFGCost"),
                        SalesWiseCOGSCost = r.Field<decimal>("SalesWiseCOGSCost"),
                        TotalFGCost = r.Field<decimal>("TotalFGCost"),
                        ProfitAndLoss = r.Field<decimal>("ProfitAndLoss"),
                        ProfitRatio = r.Field<decimal>("ProfitRatio"),
                        ItemAverageCost = r.Field<decimal>("ItemAverageCost"),
                        ProductionRatio = r.Field<decimal>("ProductionRatio"),
                        TotalProduction = r.Field<decimal>("TotalProduction"),
                        ProductionWiseFGCost = r.Field<decimal>("ProductionWiseFGCost"),
                        ProductionWiseCOGSCost = r.Field<decimal>("ProductionWiseCOGSCost")
                    }).ToList();
                }
            }

            return finishGoodsDetails;
        }
        public List<OverheadExpensesBO> GetInvProductionOEInformation(int productionId)
        {
            List<OverheadExpensesBO> OverheadExpenseDetails = new List<OverheadExpensesBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvProductionOEInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProductionId", DbType.Int32, productionId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "OverheadExpense");
                    DataTable Table = ds.Tables["OverheadExpense"];

                    OverheadExpenseDetails = Table.AsEnumerable().Select(r => new OverheadExpensesBO
                    {
                        ProductionId = r.Field<Int64>("ProductionId"),
                        NodeId = r.Field<Int32>("NodeId"),
                        OverheadName = r.Field<string>("OverheadName"),
                        Amount = r.Field<decimal>("Amount"),
                        Remarks = r.Field<string>("Remarks")
                    }).ToList();
                }
            }

            return OverheadExpenseDetails;
        }
    }
}
