using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Inventory;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Inventory
{
    public class AllInventoryReportDA : BaseService
    {
        public List<InventoryStockVarianceViewBO> GetInventoryStockVariance(DateTime fromDate, DateTime toDate, int categoryId, int itemId, int locationId)
        {
            List<InventoryStockVarianceViewBO> invStockVariance = new List<InventoryStockVarianceViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInventoryCountVarianceReport_SP"))
                {
                    if (locationId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);
                    }
                    if (categoryId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    }
                    if (itemId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);
                    }
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "InvStockVariance");
                    DataTable Table = ds.Tables["InvStockVariance"];

                    invStockVariance = Table.AsEnumerable().Select(r => new InventoryStockVarianceViewBO
                    {
                        //CostCenterId = r.Field<int>("CostCenterId"),
                        ItemId = r.Field<int?>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        Unit = r.Field<string>("Unit"),
                        AverageCost = r.Field<decimal?>("AverageCost"),
                        ExpectedQty = r.Field<decimal?>("ExpectedQty"),
                        ActualQty = r.Field<decimal?>("ActualQty"),
                        ActualValue = r.Field<decimal?>("ActualValue"),
                        CVQty = r.Field<decimal?>("CVQty"),
                        CVValue = r.Field<decimal?>("CVValue"),
                        CountedBy = r.Field<string>("CountedBy")

                        //AverageCost = r.Field<decimal?>("AverageCost"),
                        //ExpectedQuantity = r.Field<decimal>("ExpectedQuantity"),
                        //ActualQuantity = r.Field<decimal>("ActualQuantity"),
                        //ActualCost = r.Field<decimal?>("ActualCost"),
                        //VarianceQuantity = r.Field<decimal?>("VarianceQuantity"),
                        //VarianceCost = r.Field<decimal?>("VarianceCost")

                    }).ToList();
                }
            }
            return invStockVariance;
        }
        public List<InventoryCOSDetailViewBO> GetInventoryCOSDetail(DateTime fromDate, DateTime toDate, int? categoryId, int? itemId, int? costCenterId)
        {
            List<InventoryCOSDetailViewBO> invCOSDetails = new List<InventoryCOSDetailViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInventoryCOSDetailReport_SP"))
                {
                    if (costCenterId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);
                    }
                    if (categoryId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    }
                    if (itemId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);
                    }
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "InvCOSDetails");
                    DataTable Table = ds.Tables["InvCOSDetails"];

                    invCOSDetails = Table.AsEnumerable().Select(r => new InventoryCOSDetailViewBO
                    {
                        //LocationId	ItemId	ItemName	StockBy	AverageCost	BeginingStock	PurchaseQuantity	EndingQuantiy	ActualUsageQuantity	ActualUsageCost	SalesQuantity	SalesPrice	Costs	CostsOfSales

                        ItemId = r.Field<int>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        StockBy = r.Field<string>("StockBy"),
                        AverageCost = r.Field<decimal?>("AverageCost"),
                        BeginingStock = r.Field<decimal?>("BeginingStock"),
                        PurchaseQuantity = r.Field<decimal?>("PurchaseQuantity"),
                        EndingQuantiy = r.Field<decimal?>("EndingQuantiy"),
                        ActualUsageQuantity = r.Field<decimal?>("ActualUsageQuantity"),
                        ActualUsageCost = r.Field<decimal?>("ActualUsageCost"),
                        SalesQuantity = r.Field<decimal?>("SalesQuantity"),
                        SalesPrice = r.Field<decimal?>("SalesPrice"),
                        Costs = r.Field<decimal?>("Costs"),
                        CostsOfSales = r.Field<decimal?>("CostsOfSales")
                    }).ToList();
                }
            }
            return invCOSDetails;
        }
        public List<InventoryItemUsageViewBO> GetInventoryItemUsage(DateTime fromDate, DateTime toDate, int categoryId, int itemId, int locationId)
        {
            List<InventoryItemUsageViewBO> invItemUsage = new List<InventoryItemUsageViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInventoryItemUsageReport_SP"))
                {
                    //using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInventoryCountVarianceReport_SP"))
                    //{
                    if (locationId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, locationId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);
                    }
                    if (categoryId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    }
                    if (itemId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);
                    }
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "InvItemUsage");
                    DataTable Table = ds.Tables["InvItemUsage"];

                    invItemUsage = Table.AsEnumerable().Select(r => new InventoryItemUsageViewBO
                    {
                        CostCenterId = r.Field<int>("CostCenterId"),
                        ItemId = r.Field<int?>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        HeadName = r.Field<string>("HeadName"),
                        AverageCost = r.Field<decimal?>("AverageCost"),
                        ActualUsage = r.Field<decimal?>("ActualUsage"),
                        //ActualQty = r.Field<decimal?>("ActualQty"),
                        ActualValue = r.Field<decimal?>("ActualValue"),
                        //CVQty = r.Field<decimal?>("CVQty"),
                        //CVValue = r.Field<decimal?>("CVValue"),
                        //CountedBy = r.Field<string>("CountedBy")
                        NoofTurns = r.Field<decimal>("NoofTurns"),
                        AvgDailyUsage = r.Field<decimal>("AvgDailyUsage"),
                        AvgNoofDaysOnHand = r.Field<decimal>("AvgNoofDaysOnHand"),
                        PriceFluctuation = r.Field<decimal>("PriceFluctuation"),
                        UsagePerGuest = r.Field<decimal>("UsagePerGuest")
                    }).ToList();
                }
            }
            return invItemUsage;
        }
        public List<InventoryVarianceInfoViewBO> GetInventoryVarianceInfo(DateTime fromDate, DateTime toDate, int categoryId, int itemId, int locationId, string reportType)
        {
            List<InventoryVarianceInfoViewBO> invVarianceInfo = new List<InventoryVarianceInfoViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInventoryVarianceInfoReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);

                    if (locationId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);
                    }
                    if (categoryId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    }
                    if (itemId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "InvVarianceInfo");
                    DataTable Table = ds.Tables["InvVarianceInfo"];

                    invVarianceInfo = Table.AsEnumerable().Select(r => new InventoryVarianceInfoViewBO
                    {
                        //CostCenterId = r.Field<int>("CostCenterId"),
                        ItemId = r.Field<int?>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        Unit = r.Field<string>("Unit"),
                        WastageQuantity = r.Field<decimal?>("WastageQuantity"),
                        WastageCost = r.Field<decimal?>("WastageCost"),
                        WastageAllowance = r.Field<string>("WastageAllowance"),
                        StockVarianceDate = r.Field<DateTime?>("StockVarianceDate"),
                        EnteredBy = r.Field<string>("EnteredBy"),
                        WastageReason = r.Field<string>("WastageReason")
                    }).ToList();
                }
            }
            return invVarianceInfo;
        }

        public List<ConsolidatedMenuItemSalesDetailBO> GetConsolidatedMenuItemSalesDetailBO(DateTime fromDate, DateTime toDate)
        {
            List<ConsolidatedMenuItemSalesDetailBO> invVarianceInfo = new List<ConsolidatedMenuItemSalesDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("ConsolidatedMenuItemSalesDetail_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "InvVarianceInfo");
                    DataTable Table = ds.Tables["InvVarianceInfo"];

                    invVarianceInfo = Table.AsEnumerable().Select(r => new ConsolidatedMenuItemSalesDetailBO
                    {
                        //BillId = r.Field<int?>("BillId"),
                        //BillDate = r.Field<DateTime?>("BillDate"),
                        //KotId = r.Field<int?>("KotId"),
                        ItemId = r.Field<int?>("ItemId"),
                        ItemCode = r.Field<string>("ItemCode"),
                        ItemName = r.Field<string>("ItemName"),
                        //MLVL = r.Field<string>("MLVL"),
                        SalesPrice = r.Field<decimal?>("SalesPrice"),
                        SalesQuantity = r.Field<decimal?>("SalesQuantity"),
                        SalesTTL = r.Field<decimal?>("SalesTTL"),
                        //ReturnQuantity = r.Field<decimal?>("ReturnQuantity"),
                        //ReturnPrice = r.Field<decimal?>("ReturnPrice"),
                        //ReturnTTL = r.Field<decimal?>("ReturnTTL"),
                        GrossSales = r.Field<decimal?>("GrossSales"),
                        GrossSalesTTL = r.Field<decimal?>("GrossSalesTTL"),
                        Discount = r.Field<decimal?>("Discount"),
                        DiscountTTL = r.Field<decimal?>("DiscountTTL"),
                        NetSales = r.Field<decimal?>("NetSales"),
                        NetSaleTTL = r.Field<decimal?>("NetSaleTTL")
                        //ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                        //VatAmount = r.Field<decimal?>("VatAmount"),
                        //PaxQuantity = r.Field<int?>("PaxQuantity")

                    }).ToList();
                }
            }
            return invVarianceInfo;
        }
        public List<ConsolidatedMenuItemSalesDetailBO> GetConsolidatedMenuItemSalesSummary(DateTime fromDate, DateTime toDate)
        {
            List<ConsolidatedMenuItemSalesDetailBO> invVarianceInfo = new List<ConsolidatedMenuItemSalesDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("ConsolidatedMenuItemSalesSummary_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "InvVarianceInfo");
                    DataTable Table = ds.Tables["InvVarianceInfo"];

                    invVarianceInfo = Table.AsEnumerable().Select(r => new ConsolidatedMenuItemSalesDetailBO
                    {
                        //BillId = r.Field<int?>("BillId"),
                        //BillDate = r.Field<DateTime?>("BillDate"),
                        //KotId = r.Field<int?>("KotId"),
                        ItemId = r.Field<int?>("ItemId"),
                        ItemCode = r.Field<string>("ItemCode"),
                        ItemName = r.Field<string>("ItemName"),
                        //MLVL = r.Field<string>("MLVL"),
                        //SalesPrice = r.Field<decimal?>("SalesPrice"),
                        SalesQuantity = r.Field<decimal?>("SalesQuantity"),
                        SalesTTL = r.Field<decimal?>("SalesTTL"),
                        //ReturnQuantity = r.Field<decimal?>("ReturnQuantity"),
                        //ReturnPrice = r.Field<decimal?>("ReturnPrice"),
                        //ReturnTTL = r.Field<decimal?>("ReturnTTL"),
                        //GrossSales = r.Field<decimal?>("GrossSales"),
                        //GrossSalesTTL = r.Field<decimal?>("GrossSalesTTL"),
                        //DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                        //DiscountTTL = r.Field<decimal?>("DiscountTTL"),
                        NetSales = r.Field<decimal?>("NetSales"),
                        NetSaleTTL = r.Field<decimal?>("NetSaleTTL"),
                        //ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                        //VatAmount = r.Field<decimal?>("VatAmount"),
                        //PaxQuantity = r.Field<int?>("PaxQuantity")

                    }).ToList();
                }
            }
            return invVarianceInfo;
        }

        public List<InventoryItemUsageAnalysisBO> GetInventoryItemUsageAnalysis(DateTime fromDate, DateTime toDate)
        {
            List<InventoryItemUsageAnalysisBO> invVarianceInfo = new List<InventoryItemUsageAnalysisBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("InventoryItemUsageAnalysis_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "InvVarianceInfo");
                    DataTable Table = ds.Tables["InvVarianceInfo"];

                    invVarianceInfo = Table.AsEnumerable().Select(r => new InventoryItemUsageAnalysisBO
                    {
                        //ItemCode = r.Field<string>("ItemCode"),
                        ItemId = r.Field<int?>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        Unit = r.Field<string>("Unit"),
                        AverageCost = r.Field<decimal?>("AverageCost"),
                        ActualUsage = r.Field<decimal?>("ActualUsage"),
                        //UnitPrice = r.Field<decimal?>("UnitPrice"),
                        ActualValue = r.Field<decimal?>("ActualValue"),
                        AvgDailyUsage = r.Field<decimal?>("AvgDailyUsage"),
                        //Turn = r.Field<decimal>("Turn"),
                        //AverageOfDaysHand = r.Field<decimal>("AverageOfDaysHand"),
                        PriceFluctuation = r.Field<decimal?>("PriceFluctuation"),
                        UsagePerGuest = r.Field<decimal?>("UsagePerGuest")

                    }).ToList();
                }
            }
            return invVarianceInfo;
        }

        public List<InventoryCategoryWiseUsageNCostAnalysisBO> GetInventoryCategoryWiseUsageNCostAnalysis(DateTime fromDate, DateTime toDate, int categoryId)
        {
            List<InventoryCategoryWiseUsageNCostAnalysisBO> invVarianceInfo = new List<InventoryCategoryWiseUsageNCostAnalysisBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("InventoryItemUsageByCategory_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "InvVarianceInfo");
                    DataTable Table = ds.Tables["InvVarianceInfo"];

                    invVarianceInfo = Table.AsEnumerable().Select(r => new InventoryCategoryWiseUsageNCostAnalysisBO
                    {
                        CategoryId = r.Field<int?>("CategoryId"),
                        CategoryName = r.Field<string>("CategoryName"),
                        ActualUsage = r.Field<decimal?>("ActualUsage"),
                        ActualCost = r.Field<decimal?>("ActualCost"),
                        TotalSalesAmount = r.Field<decimal?>("TotalSalesAmount"),
                        CostByPercentage = r.Field<decimal?>("CostByPercentage")

                    }).ToList();
                }
            }
            return invVarianceInfo;
        }

        public DailyConsolidatedRevenueCenterSalesDetailViewBO GetDailyConsolidatedRevenueCenterSalesDetail(DateTime fromDate, DateTime toDate)
        {
            DailyConsolidatedRevenueCenterSalesDetailViewBO viewBo = new DailyConsolidatedRevenueCenterSalesDetailViewBO();
            List<DailyConsolidatedRevenueCostCenterWiseSalesDetailsBO> costcenterWisesalesDetails = new List<DailyConsolidatedRevenueCostCenterWiseSalesDetailsBO>();
            List<DailyConsolidatedRevenueCostCenterWiseNetSalesDetailsBO> salesDetails = new List<DailyConsolidatedRevenueCostCenterWiseNetSalesDetailsBO>();
            List<DailyConsolidatedRevenueSalesPaymentDetailsBO> paymentAll = new List<DailyConsolidatedRevenueSalesPaymentDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbDataAdapter adapter = dbSmartAspects.GetDataAdapter())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DailyConsolidatedRevenueCenterSalesDetail_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);

                        adapter.SelectCommand = cmd;
                        adapter.SelectCommand.Connection = conn;

                        DataSet ds = new DataSet();
                        adapter.Fill(ds);

                        costcenterWisesalesDetails = ds.Tables[1].AsEnumerable().Select(r => new DailyConsolidatedRevenueCostCenterWiseSalesDetailsBO
                        {
                            CostCenterId = r.Field<int?>("CostCenterId"),
                            CostCenter = r.Field<string>("CostCenter"),
                            NetSales = r.Field<decimal?>("NetSales"),
                            Guests = r.Field<int?>("Guests"),
                            //Checks = r.Field<decimal?>("Checks"),
                            AvgGuest = r.Field<decimal?>("AvgGuest"),
                            AvgChecks = r.Field<decimal?>("AvgChecks"),
                            NetSaleTTL = r.Field<decimal?>("NetSaleTTL"),
                            GuestTTL = r.Field<decimal?>("GuestTTL"),
                            //ChecksTTL = r.Field<decimal?>("ChecksTTL"),
                            // TablesUse = r.Field<int?>("TablesUse"),
                            //TableTTL = r.Field<decimal?>("TableTTL"),
                            //AvgTable = r.Field<decimal?>("AvgTable"),
                            //Turns = r.Field<decimal?>("Turns")

                        }).ToList();

                        salesDetails = ds.Tables[0].AsEnumerable().Select(r => new DailyConsolidatedRevenueCostCenterWiseNetSalesDetailsBO
                        {
                            NetSales = r.Field<decimal?>("NetSales"),
                            ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                            TaxAmount = r.Field<decimal?>("TaxAmount"),
                            TotalRevenue = r.Field<decimal?>("TotalRevenue"),
                            TotalDiscounts = r.Field<decimal?>("TotalDiscounts"),
                            ReturnTotal = r.Field<int?>("ReturnTotal"),
                            ReturnAmount = r.Field<decimal?>("ReturnAmount"),
                            VoidTotal = r.Field<int?>("VoidTotal"),
                            VoidAmount = r.Field<decimal?>("VoidAmount"),
                            GrandTotal = r.Field<decimal?>("GrandTotal"),
                            ErrorCorrects = r.Field<int?>("ErrorCorrects"),
                            ErrorAmount = r.Field<decimal?>("ErrorAmount"),
                            Checks = r.Field<int?>("Checks"),
                            ChecksPaid = r.Field<int?>("ChecksPaid"),
                            Outstanding = r.Field<int?>("Outstanding"),
                            AvgChecks = r.Field<decimal?>("AvgChecks")

                        }).ToList();

                        paymentAll = ds.Tables[2].AsEnumerable().Select(r => new DailyConsolidatedRevenueSalesPaymentDetailsBO
                        {
                            PaymentMode = r.Field<string>("PaymentMode"),
                            CardType = r.Field<string>("CardType"),
                            PaymentCount = r.Field<int?>("PaymentCount"),
                            PaymentAmount = r.Field<decimal?>("PaymentAmount")

                        }).ToList();
                    }
                }
            }

            viewBo.CostcenterWisesalesDetails = costcenterWisesalesDetails;
            viewBo.SalesDetails = salesDetails;
            viewBo.PaymentDetails = paymentAll;

            return viewBo;
        }

        public DailyConsolidatedRevenueCenterSalesDetailViewBO DailyConsolidatedRevenueCostCenterSalesDetailByDineTime(DateTime fromDate, DateTime toDate)
        {
            DailyConsolidatedRevenueCenterSalesDetailViewBO viewBo = new DailyConsolidatedRevenueCenterSalesDetailViewBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbDataAdapter adapter = dbSmartAspects.GetDataAdapter())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DailyConsolidatedRevenueCostCenterSalesDetailByDineTime_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);

                        adapter.SelectCommand = cmd;
                        adapter.SelectCommand.Connection = conn;

                        DataSet ds = new DataSet();
                        adapter.Fill(ds);

                        //---*** Lunch ----------------------------

                        viewBo.PaymentDetailsLunch = ds.Tables[0].AsEnumerable().Select(r => new DailyConsolidatedRevenueSalesPaymentDetailsBO
                        {
                            PaymentMode = r.Field<string>("PaymentMode"),
                            CardType = r.Field<string>("CardType"),
                            PaymentCount = r.Field<int?>("PaymentCount"),
                            PaymentAmount = r.Field<decimal?>("PaymentAmount")

                        }).ToList();

                        viewBo.SalesDetailsLunch = ds.Tables[1].AsEnumerable().Select(r => new DailyConsolidatedRevenueCostCenterWiseNetSalesDetailsBO
                        {
                            NetSales = r.Field<decimal?>("NetSales"),
                            ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                            TaxAmount = r.Field<decimal?>("TaxAmount"),
                            TotalRevenue = r.Field<decimal?>("TotalRevenue"),
                            TotalDiscounts = r.Field<decimal?>("TotalDiscounts"),
                            ReturnTotal = r.Field<int?>("ReturnTotal"),
                            ReturnAmount = r.Field<decimal?>("ReturnAmount"),
                            VoidTotal = r.Field<int?>("VoidTotal"),
                            VoidAmount = r.Field<decimal?>("VoidAmount"),
                            GrandTotal = r.Field<decimal?>("GrandTotal"),
                            ErrorCorrects = r.Field<int?>("ErrorCorrects"),
                            ErrorAmount = r.Field<decimal?>("ErrorAmount"),
                            Checks = r.Field<int?>("Checks"),
                            ChecksPaid = r.Field<int?>("ChecksPaid"),
                            Outstanding = r.Field<int?>("Outstanding"),
                            AvgChecks = r.Field<decimal?>("AvgChecks")

                        }).ToList();

                        viewBo.CostcenterWisesalesDetailsLunch = ds.Tables[2].AsEnumerable().Select(r => new DailyConsolidatedRevenueCostCenterWiseSalesDetailsBO
                        {
                            CostCenterId = r.Field<int?>("CostCenterId"),
                            CostCenter = r.Field<string>("CostCenter"),
                            NetSales = r.Field<decimal?>("NetSales"),
                            Guests = r.Field<int?>("Guests"),
                            Checks = r.Field<decimal?>("Checks"),
                            AvgGuest = r.Field<decimal?>("AvgGuest"),
                            AvgChecks = r.Field<decimal?>("AvgChecks"),
                            NetSaleTTL = r.Field<decimal?>("NetSaleTTL"),
                            GuestTTL = r.Field<decimal?>("GuestTTL"),
                            ChecksTTL = r.Field<decimal?>("ChecksTTL"),
                            TablesUse = r.Field<int?>("TablesUse"),
                            TableTTL = r.Field<decimal?>("TableTTL"),
                            AvgTable = r.Field<decimal?>("AvgTable"),
                            Turns = r.Field<decimal?>("Turns")

                        }).ToList();

                        //---*** Snacks ----------------------------

                        viewBo.PaymentDetailsSnacks = ds.Tables[3].AsEnumerable().Select(r => new DailyConsolidatedRevenueSalesPaymentDetailsBO
                        {
                            PaymentMode = r.Field<string>("PaymentMode"),
                            CardType = r.Field<string>("CardType"),
                            PaymentCount = r.Field<int?>("PaymentCount"),
                            PaymentAmount = r.Field<decimal?>("PaymentAmount")

                        }).ToList();


                        viewBo.SalesDetailsSnacks = ds.Tables[4].AsEnumerable().Select(r => new DailyConsolidatedRevenueCostCenterWiseNetSalesDetailsBO
                        {
                            NetSales = r.Field<decimal?>("NetSales"),
                            ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                            TaxAmount = r.Field<decimal?>("TaxAmount"),
                            TotalRevenue = r.Field<decimal?>("TotalRevenue"),
                            TotalDiscounts = r.Field<decimal?>("TotalDiscounts"),
                            ReturnTotal = r.Field<int?>("ReturnTotal"),
                            ReturnAmount = r.Field<decimal?>("ReturnAmount"),
                            VoidTotal = r.Field<int?>("VoidTotal"),
                            VoidAmount = r.Field<decimal?>("VoidAmount"),
                            GrandTotal = r.Field<decimal?>("GrandTotal"),
                            ErrorCorrects = r.Field<int?>("ErrorCorrects"),
                            ErrorAmount = r.Field<decimal?>("ErrorAmount"),
                            Checks = r.Field<int?>("Checks"),
                            ChecksPaid = r.Field<int?>("ChecksPaid"),
                            Outstanding = r.Field<int?>("Outstanding"),
                            AvgChecks = r.Field<decimal?>("AvgChecks")

                        }).ToList();

                        viewBo.CostcenterWisesalesDetailsSnacks = ds.Tables[5].AsEnumerable().Select(r => new DailyConsolidatedRevenueCostCenterWiseSalesDetailsBO
                        {
                            CostCenterId = r.Field<int?>("CostCenterId"),
                            CostCenter = r.Field<string>("CostCenter"),
                            NetSales = r.Field<decimal?>("NetSales"),
                            Guests = r.Field<int?>("Guests"),
                            Checks = r.Field<decimal?>("Checks"),
                            AvgGuest = r.Field<decimal?>("AvgGuest"),
                            AvgChecks = r.Field<decimal?>("AvgChecks"),
                            NetSaleTTL = r.Field<decimal?>("NetSaleTTL"),
                            GuestTTL = r.Field<decimal?>("GuestTTL"),
                            ChecksTTL = r.Field<decimal?>("ChecksTTL"),
                            TablesUse = r.Field<int?>("TablesUse"),
                            TableTTL = r.Field<decimal?>("TableTTL"),
                            AvgTable = r.Field<decimal?>("AvgTable"),
                            Turns = r.Field<decimal?>("Turns")

                        }).ToList();

                        //---*** Dinner ----------------------------

                        viewBo.PaymentDetailsDinner = ds.Tables[6].AsEnumerable().Select(r => new DailyConsolidatedRevenueSalesPaymentDetailsBO
                        {
                            PaymentMode = r.Field<string>("PaymentMode"),
                            CardType = r.Field<string>("CardType"),
                            PaymentCount = r.Field<int?>("PaymentCount"),
                            PaymentAmount = r.Field<decimal?>("PaymentAmount")

                        }).ToList();

                        viewBo.SalesDetailsDinner = ds.Tables[7].AsEnumerable().Select(r => new DailyConsolidatedRevenueCostCenterWiseNetSalesDetailsBO
                        {
                            NetSales = r.Field<decimal?>("NetSales"),
                            ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                            TaxAmount = r.Field<decimal?>("TaxAmount"),
                            TotalRevenue = r.Field<decimal?>("TotalRevenue"),
                            TotalDiscounts = r.Field<decimal?>("TotalDiscounts"),
                            ReturnTotal = r.Field<int?>("ReturnTotal"),
                            ReturnAmount = r.Field<decimal?>("ReturnAmount"),
                            VoidTotal = r.Field<int?>("VoidTotal"),
                            VoidAmount = r.Field<decimal?>("VoidAmount"),
                            GrandTotal = r.Field<decimal?>("GrandTotal"),
                            ErrorCorrects = r.Field<int?>("ErrorCorrects"),
                            ErrorAmount = r.Field<decimal?>("ErrorAmount"),
                            Checks = r.Field<int?>("Checks"),
                            ChecksPaid = r.Field<int?>("ChecksPaid"),
                            Outstanding = r.Field<int?>("Outstanding"),
                            AvgChecks = r.Field<decimal?>("AvgChecks")

                        }).ToList();

                        viewBo.CostcenterWisesalesDetailsDinner = ds.Tables[8].AsEnumerable().Select(r => new DailyConsolidatedRevenueCostCenterWiseSalesDetailsBO
                        {
                            CostCenterId = r.Field<int?>("CostCenterId"),
                            CostCenter = r.Field<string>("CostCenter"),
                            NetSales = r.Field<decimal?>("NetSales"),
                            Guests = r.Field<int?>("Guests"),
                            Checks = r.Field<decimal?>("Checks"),
                            AvgGuest = r.Field<decimal?>("AvgGuest"),
                            AvgChecks = r.Field<decimal?>("AvgChecks"),
                            NetSaleTTL = r.Field<decimal?>("NetSaleTTL"),
                            GuestTTL = r.Field<decimal?>("GuestTTL"),
                            ChecksTTL = r.Field<decimal?>("ChecksTTL"),
                            TablesUse = r.Field<int?>("TablesUse"),
                            TableTTL = r.Field<decimal?>("TableTTL"),
                            AvgTable = r.Field<decimal?>("AvgTable"),
                            Turns = r.Field<decimal?>("Turns")

                        }).ToList();

                    }
                }
            }

            return viewBo;
        }

        public List<DatewisePurchaseCompareViewBO> GetDatewisePurchaseCompareReportInfo(DateTime fromDate, DateTime toDate, int itemId)
        {
            List<DatewisePurchaseCompareViewBO> viewList = new List<DatewisePurchaseCompareViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDatewisePurchaseCompareReportInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "InvStockVariance");
                    DataTable Table = ds.Tables["InvStockVariance"];

                    viewList = Table.AsEnumerable().Select(r => new DatewisePurchaseCompareViewBO
                    {
                        ReceivedDate = r.Field<DateTime?>("ReceivedDate"),
                        ItemName = r.Field<string>("ItemName"),
                        StockBy = r.Field<string>("StockBy"),
                        Unit = r.Field<decimal?>("Unit"),
                        PurchaseRate = r.Field<decimal?>("PurchaseRate"),
                        TotalPrice = r.Field<decimal?>("TotalPrice"),
                        Variance = r.Field<decimal?>("Variance")

                    }).ToList();
                }
            }
            return viewList;
        }

        public List<AbcAnalysisBO> GetAbcAnalysis(DateTime dtFromDate, DateTime dtToDate, string costCenterCommaSeperatedIds, int categoryId)
        {
            List<AbcAnalysisBO> viewList = new List<AbcAnalysisBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("ABCAnalysis_SP"))
                {
                    if(dtFromDate == DateTime.MinValue)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.String, "");
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.String, dtFromDate.ToString("yyyy-MM-dd"));

                    if(dtToDate == DateTime.MinValue)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.String, "");
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.String, dtToDate.ToString("yyyy-MM-dd"));

                    dbSmartAspects.AddInParameter(cmd, "@CostCenterCommaSeperatedIds", DbType.String, costCenterCommaSeperatedIds);
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "AbcAnalysis");
                    DataTable Table = ds.Tables["AbcAnalysis"];

                    viewList = Table.AsEnumerable().Select(r => new AbcAnalysisBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        NumberOfItemsSold = r.Field<decimal>("NumberOfItemsSold"),
                        UsageValue = r.Field<decimal>("UsageValue"),
                        TotalQuantity = r.Field<int>("TotalQuantity"),
                        TotalUsage = r.Field<int>("TotalUsage"),
                        AnnualSold = r.Field<decimal>("AnnualSold"),
                        AnnualUsage = r.Field<decimal>("AnnualUsage"),
                        RunningTotal = r.Field<decimal>("RunningTotal"),
                        AbcAnalysis = r.Field<string>("AbcAnalysis")

                    }).ToList();
                }
            }
            return viewList;
        }

        public List<XYZAnalysisBO> GetXyzAnalysis(DateTime dtFromDate, DateTime dtToDate, string costCenterCommaSeperatedIds, int categoryId)
        {
            List<XYZAnalysisBO> viewList = new List<XYZAnalysisBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("XYZAnalysis_SP"))
                {
                    if (dtFromDate == DateTime.MinValue)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.String, "");
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.String, dtFromDate.ToString("yyyy-MM-dd"));

                    if (dtToDate == DateTime.MinValue)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.String, "");
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.String, dtToDate.ToString("yyyy-MM-dd"));

                    dbSmartAspects.AddInParameter(cmd, "@CostCenterCommaSeperatedIds", DbType.String, costCenterCommaSeperatedIds);
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "XYZAnalysis");
                    DataTable Table = ds.Tables["XYZAnalysis"];

                    viewList = Table.AsEnumerable().Select(r => new XYZAnalysisBO
                    {
                        Name = r.Field<string>("Name"),
                        D1 = r.Field<decimal>("D1"),
                        D2 = r.Field<decimal>("D2"),
                        D3 = r.Field<decimal>("D3"),
                        D4 = r.Field<decimal>("D4"),
                        AnnualDemand = r.Field<decimal>("AnnualDemand"),
                        AverageDemand = r.Field<decimal>("AverageDemand"),
                        STD = r.Field<decimal>("STD"),
                        CV = r.Field<decimal>("CV"),
                        Status = r.Field<string>("Status")
                    }).ToList();
                }
            }
            return viewList;
        }
    }
}
