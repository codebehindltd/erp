using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data;
using System.Data.Common;
using HotelManagement.Entity.Inventory;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantSalesInfoDA : BaseService
    {
        public bool RestaurantSalesNPaymentAdjustment(string reportType, DateTime fromDate, DateTime toDate, string CostCenterIdLst, int categoryidId, int itemId, string referNo, string transtype)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("RestaurantSalesNPaymentAdjustment_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterIdList", DbType.String, CostCenterIdLst);
                    dbSmartAspects.AddInParameter(cmd, "@FilterBy", DbType.String, referNo);
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryidId);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceId", DbType.Int32, itemId);
                    dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transtype);
                    status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<SalesInfoReportViewBO> GetSalesRestaurantInfoForReport(string reportType, DateTime fromDate, DateTime toDate, string CostCenterIdLst, int categoryidId, int itemId, string referNo, string transtype, int cashierInfoId, int waiterInfoId, string sourceName, int sourceId, int companyId)
        {
            List<SalesInfoReportViewBO> salesList = new List<SalesInfoReportViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantSalesInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    //dbSmartAspects.AddInParameter(cmd, "@CostCenterIdList", DbType.String, CostCenterIdLst);
                    //dbSmartAspects.AddInParameter(cmd, "@FilterBy", DbType.String, referNo);
                    //dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryidId);
                    //dbSmartAspects.AddInParameter(cmd, "@ServiceId", DbType.Int32, itemId);
                    //dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transtype);

                    if (!string.IsNullOrEmpty(CostCenterIdLst))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterIdList", DbType.String, CostCenterIdLst);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterIdList", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(referNo))
                    {
                        if (referNo == "All")
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FilterBy", DbType.String, DBNull.Value);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FilterBy", DbType.String, referNo);
                        }
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FilterBy", DbType.String, DBNull.Value);
                    }

                    if (categoryidId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryidId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (itemId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ServiceId", DbType.Int32, itemId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ServiceId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transtype);

                    if (cashierInfoId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CashierInfoId", DbType.Int32, cashierInfoId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CashierInfoId", DbType.Int32, DBNull.Value);

                    if (waiterInfoId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@WaiterInfoId", DbType.Int32, waiterInfoId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@WaiterInfoId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(sourceName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SourceName", DbType.String, sourceName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SourceName", DbType.String, DBNull.Value);
                    }

                    if (sourceId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@SourceId", DbType.Int32, sourceId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SourceId", DbType.Int32, DBNull.Value);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantSales");
                    DataTable table = ds.Tables["RestaurantSales"];
                    salesList = table.AsEnumerable().Select(r =>
                                new SalesInfoReportViewBO
                                {
                                    ServiceDate = r.Field<DateTime>("ServiceDate"),
                                    ServiceDisplayDate = r.Field<string>("ServiceDisplayDate"),
                                    ReferenceNo = r.Field<string>("ReferenceNo"),
                                    CategoryId = r.Field<int?>("CategoryId"),
                                    CategoryName = r.Field<string>("CategoryName"),
                                    ServiceId = r.Field<int?>("ServiceId"),
                                    ServiceName = r.Field<string>("ServiceName"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    ItemQuantity = r.Field<decimal?>("ItemQuantity"),
                                    UnitRate = r.Field<decimal?>("UnitRate"),
                                    TotalAmount = r.Field<decimal?>("TotalAmount"),
                                    DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                                    ServiceRate = r.Field<decimal?>("ServiceRate"),
                                    ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                                    CitySDCharge = r.Field<decimal?>("CitySDCharge"),
                                    VatAmount = r.Field<decimal?>("VatAmount"),
                                    AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                                    TotalSalesAmount = r.Field<decimal?>("TotalSalesAmount"),
                                    ItemCost = r.Field<decimal?>("ItemCost"),
                                    ProfitNLossAmount = r.Field<decimal?>("ProfitNLossAmount"),
                                    SalesType = r.Field<string>("SalesType"),
                                    IsDiscountHead = r.Field<int?>("IsDiscountHead"),
                                    CompanyId = r.Field<int>("CompanyId"),
                                    CompanyName = r.Field<string>("CompanyName"),
                                    ProjectId = r.Field<int>("ProjectId"),
                                    ProjectName = r.Field<string>("ProjectName"),
                                }).ToList();
                }
            }
            return salesList;
        }

        public List<SalesInfoReportViewBO> GetSalesRestaurantInfoForReport(string reportType, DateTime fromDate, DateTime toDate, string CostCenterIdLst, int categoryidId, int itemId, string referNo, string transtype, int foodOrBeverageId, int cashierInfoId, int waiterInfoId, string sourceName, int sourceId)
        {
            //FnB
            List<SalesInfoReportViewBO> salesList = new List<SalesInfoReportViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantSalesInfoFilterByCalssificationForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    if (!string.IsNullOrEmpty(CostCenterIdLst))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterIdList", DbType.String, CostCenterIdLst);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterIdList", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(referNo))
                    {
                        if (referNo == "All")
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FilterBy", DbType.String, DBNull.Value);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FilterBy", DbType.String, referNo);
                        }
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FilterBy", DbType.String, DBNull.Value);
                    }

                    if (categoryidId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryidId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (itemId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ServiceId", DbType.Int32, itemId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ServiceId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transtype);

                    if (foodOrBeverageId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ClassificationId", DbType.Int32, foodOrBeverageId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ClassificationId", DbType.Int32, DBNull.Value);

                    if (cashierInfoId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CashierInfoId", DbType.Int32, cashierInfoId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CashierInfoId", DbType.Int32, DBNull.Value);

                    if (waiterInfoId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@WaiterInfoId", DbType.Int32, waiterInfoId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@WaiterInfoId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(sourceName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SourceName", DbType.String, sourceName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SourceName", DbType.String, DBNull.Value);
                    }

                    if (sourceId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@SourceId", DbType.Int32, sourceId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SourceId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantSales");
                    DataTable table = ds.Tables["RestaurantSales"];
                    salesList = table.AsEnumerable().Select(r =>
                                new SalesInfoReportViewBO
                                {
                                    ReferenceNo = r.Field<string>("ReferenceNo"),
                                    CategoryId = r.Field<int?>("CategoryId"),
                                    CategoryName = r.Field<string>("CategoryName"),
                                    ServiceId = r.Field<int?>("ServiceId"),
                                    ServiceName = r.Field<string>("ServiceName"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    ItemQuantity = r.Field<decimal?>("ItemQuantity"),
                                    UnitRate = r.Field<decimal?>("UnitRate"),
                                    TotalAmount = r.Field<decimal?>("TotalAmount"),
                                    ServiceRate = r.Field<decimal?>("ServiceRate"),
                                    ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                                    VatAmount = r.Field<decimal?>("VatAmount"),
                                    ItemCost = r.Field<decimal?>("ItemCost"),
                                    SalesType = r.Field<string>("SalesType"),
                                    IsDiscountHead = r.Field<int?>("IsDiscountHead"),
                                    CashierName = r.Field<string>("CashierName"),
                                    CQty = r.Field<decimal>("CQty"),
                                    CCost = r.Field<decimal>("CCost")

                                }).ToList();
                }
            }
            return salesList;
        }
        public List<SalesInfoReportViewBO> GetInvoiceWiseSalesRestaurantInfoForReport(string costCenterIdList, DateTime fromDate, DateTime toDate, int cashierId, int userGroupId)
        {
            List<SalesInfoReportViewBO> salesList = new List<SalesInfoReportViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvoiceWiseRestaurantSalesInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterIdList", DbType.String, costCenterIdList);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, cashierId);
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantSales");
                    DataTable table = ds.Tables["RestaurantSales"];
                    salesList = table.AsEnumerable().Select(r =>
                                new SalesInfoReportViewBO
                                {
                                    BillNumber = r.Field<string>("BillNumber"),
                                    BillDate = r.Field<DateTime>("BillDate"),
                                    UserInfoId = r.Field<int>("UserInfoId"),
                                    UserName = r.Field<string>("UserName"),
                                    ItemId = r.Field<int>("ItemId"),
                                    ItemName = r.Field<string>("ItemName"),
                                    UnitRate = r.Field<decimal>("UnitRate"),
                                    ItemUnit = r.Field<decimal>("ItemUnit"),
                                    LineTotal = r.Field<decimal>("LineTotal"),
                                    GrandTotal = r.Field<decimal>("GrandTotal"),
                                    RoundedAmount = r.Field<decimal>("RoundedAmount"),
                                    RoundedGrandTotal = r.Field<decimal>("RoundedGrandTotal"),
                                    TotalServiceCharge = r.Field<decimal?>("TotalServiceCharge"),
                                    TotalCitySDCharge = r.Field<decimal?>("TotalCitySDCharge"),
                                    TotalVat = r.Field<decimal>("TotalVat"),
                                    TotalAdditionalCharge = r.Field<decimal?>("TotalAdditionalCharge"),
                                    CalculatedDiscountAmount = r.Field<decimal?>("CalculatedDiscountAmount"),
                                    PaymentInformation = r.Field<string>("PaymentInformation"),
                                    GuestTotalPaymentInformation = r.Field<string>("GuestTotalPaymentInformation"),
                                    PaymentRemarks = r.Field<string>("PaymentRemarks"),
                                    KotId = r.Field<int>("KotId"),
                                    KotDate = r.Field<string>("KotDate"),
                                    KotTime = r.Field<string>("KotTime")
                                }).ToList();
                }
            }
            return salesList;
        }
        public List<DailySalesStatementBO> GetSalesRestaurantInfoForDateRange(int transactionType, DateTime fromDate, DateTime toDate, string CostCenterLst, string reportType, int userGroupId, bool? complementaryOrAll, string paymentType)
        {
            List<DailySalesStatementBO> salesList = new List<DailySalesStatementBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDailySalesStatement_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);
                    dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.Int32, transactionType);

                    if (!string.IsNullOrEmpty(CostCenterLst))
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterLst", DbType.String, CostCenterLst);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterLst", DbType.String, DBNull.Value);

                    if (complementaryOrAll != null)
                        dbSmartAspects.AddInParameter(cmd, "@ComplementaryOrAll", DbType.Boolean, complementaryOrAll);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ComplementaryOrAll", DbType.Boolean, DBNull.Value);

                    if (!string.IsNullOrEmpty(paymentType))
                        dbSmartAspects.AddInParameter(cmd, "@PaymentType", DbType.String, paymentType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PaymentType", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantSales");
                    DataTable table = ds.Tables["RestaurantSales"];

                    salesList = table.AsEnumerable().Select(r =>
                                new DailySalesStatementBO
                                {
                                    BillNo = r.Field<int?>("BillNo"),
                                    BillNumber = r.Field<string>("BillNumber"),
                                    Kot = r.Field<int?>("Kot"),
                                    Pax = r.Field<int?>("Pax"),
                                    TotalSales = r.Field<decimal?>("TotalSales"),
                                    DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                                    AfterDiscountAmount = r.Field<decimal?>("AfterDiscountAmount"),
                                    NetSalesAmount = r.Field<decimal?>("NetSalesAmount"),
                                    VatPercentage = r.Field<string>("VatPercentage"),
                                    ServiceChargePercentage = r.Field<string>("ServiceChargePercentage"),
                                    CitySDChargePercentage = r.Field<string>("CitySDChargePercentage"),
                                    AdditionalChargePercentage = r.Field<string>("AdditionalChargePercentage"),
                                    VatAmount = r.Field<decimal?>("VatAmount"),
                                    ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                                    CitySDCharge = r.Field<decimal?>("CitySDCharge"),
                                    AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                                    GrandTotal = r.Field<decimal?>("GrandTotal"),
                                    RoundedAmount = r.Field<decimal?>("RoundedAmount"),
                                    RoundedGrandTotal = r.Field<decimal?>("RoundedGrandTotal"),
                                    CostCenterId = r.Field<int?>("CostCenterId"),
                                    CostCenter = r.Field<string>("CostCenter"),
                                    CostCenterType = r.Field<string>("CostCenterType"),
                                    ItemType = r.Field<string>("ItemType"),
                                    ItemValue = r.Field<decimal?>("ItemValue"),
                                    Remarks = r.Field<string>("Remarks"),
                                    CostCenters = r.Field<string>("CostCenters"),
                                    ItemRank = r.Field<int>("ItemRank"),
                                    UserName = r.Field<string>("UserName")
                                }).ToList();
                }
            }
            return salesList;
        }
        public DailySalesStatementViewBO GetSalesRestaurantInfoForSingleDate(DateTime fromDate, DateTime toDate, string CostCenterLst, string reportType, int userGroupId, bool? complementaryOrAll, string paymentType)
        {
            DailySalesStatementViewBO salesList = new DailySalesStatementViewBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDailySalesStatementForSingleDate_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);

                    if (!string.IsNullOrEmpty(CostCenterLst))
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterLst", DbType.String, CostCenterLst);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterLst", DbType.String, DBNull.Value);

                    if (complementaryOrAll != null)
                        dbSmartAspects.AddInParameter(cmd, "@ComplementaryOrAll", DbType.Boolean, complementaryOrAll);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ComplementaryOrAll", DbType.Boolean, DBNull.Value);

                    if (!string.IsNullOrEmpty(paymentType))
                        dbSmartAspects.AddInParameter(cmd, "@PaymentType", DbType.String, paymentType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PaymentType", DbType.Boolean, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantSales");

                    salesList.SalesStatementSingleDate = ds.Tables[0].AsEnumerable().Select(r =>
                                new DailySalesStatementBO
                                {
                                    BillNo = r.Field<int?>("BillNo"),
                                    BillNumber = r.Field<string>("BillNumber"),
                                    Kot = r.Field<int?>("Kot"),
                                    Pax = r.Field<int?>("Pax"),
                                    TotalSales = r.Field<decimal?>("TotalSales"),
                                    DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                                    AfterDiscountAmount = r.Field<decimal?>("AfterDiscountAmount"),
                                    NetSalesAmount = r.Field<decimal?>("NetSalesAmount"),
                                    VatPercentage = r.Field<string>("VatPercentage"),
                                    ServiceChargePercentage = r.Field<string>("ServiceChargePercentage"),
                                    CitySDChargePercentage = r.Field<string>("CitySDChargePercentage"),
                                    AdditionalChargePercentage = r.Field<string>("AdditionalChargePercentage"),
                                    VatAmount = r.Field<decimal?>("VatAmount"),
                                    ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                                    CitySDCharge = r.Field<decimal?>("CitySDCharge"),
                                    AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                                    GrandTotal = r.Field<decimal?>("GrandTotal"),
                                    RoundedAmount = r.Field<decimal?>("RoundedAmount"),
                                    RoundedGrandTotal = r.Field<decimal?>("RoundedGrandTotal"),
                                    CostCenterId = r.Field<int?>("CostCenterId"),
                                    CostCenter = r.Field<string>("CostCenter"),
                                    ItemType = r.Field<string>("ItemType"),
                                    ItemValue = r.Field<decimal?>("ItemValue"),
                                    Remarks = r.Field<string>("Remarks"),
                                    CostCenters = r.Field<string>("CostCenters"),
                                    ItemRank = r.Field<int>("ItemRank"),
                                    UserName = r.Field<string>("UserName")
                                }).ToList();

                    salesList.SalesStatementSummary = ds.Tables[1].AsEnumerable().Select(r =>
                                new DailySalesStatementBO
                                {
                                    BillNo = r.Field<int?>("BillNo"),
                                    BillNumber = r.Field<string>("BillNumber"),
                                    Kot = r.Field<int?>("Kot"),
                                    Pax = r.Field<int?>("Pax"),
                                    TotalSales = r.Field<decimal?>("TotalSales"),
                                    DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                                    AfterDiscountAmount = r.Field<decimal?>("AfterDiscountAmount"),
                                    NetSalesAmount = r.Field<decimal?>("NetSalesAmount"),
                                    VatPercentage = r.Field<string>("VatPercentage"),
                                    ServiceChargePercentage = r.Field<string>("ServiceChargePercentage"),
                                    CitySDChargePercentage = r.Field<string>("CitySDChargePercentage"),
                                    AdditionalChargePercentage = r.Field<string>("AdditionalChargePercentage"),
                                    VatAmount = r.Field<decimal?>("VatAmount"),
                                    ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                                    CitySDCharge = r.Field<decimal?>("CitySDCharge"),
                                    AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                                    GrandTotal = r.Field<decimal?>("GrandTotal"),
                                    RoundedAmount = r.Field<decimal?>("RoundedAmount"),
                                    RoundedGrandTotal = r.Field<decimal?>("RoundedGrandTotal"),
                                    CostCenterId = r.Field<int?>("CostCenterId"),
                                    CostCenter = r.Field<string>("CostCenter"),
                                    ItemType = r.Field<string>("ItemType"),
                                    ItemValue = r.Field<decimal?>("ItemValue"),
                                    Remarks = r.Field<string>("Remarks"),
                                    CostCenters = r.Field<string>("CostCenters"),
                                    ItemRank = r.Field<int>("ItemRank"),
                                    UserName = r.Field<string>("UserName")
                                }).ToList();
                }
            }
            return salesList;
        }
        public List<SalesInfoReportViewBO> GetGuestPaymentSummeryInfoForReport(DateTime fromDate, DateTime toDate, string CostCenterIdLst, int serviceId)
        {
            List<SalesInfoReportViewBO> salesList = new List<SalesInfoReportViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestPaymentSummeryInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterIdList", DbType.String, CostCenterIdLst);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceId", DbType.Int32, serviceId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantSales");
                    DataTable table = ds.Tables["RestaurantSales"];
                    salesList = table.AsEnumerable().Select(r =>
                                new SalesInfoReportViewBO
                                {
                                    GuestTotalPaymentInformation = r.Field<string>("GuestTotalPaymentInformation")
                                }).ToList();
                }
            }
            return salesList;
        }
        public List<SalesInfoReportViewBO> GetCashierCollectionSummeryInformationForReport(int cashierId, DateTime fromDate, DateTime toDate)
        {
            List<SalesInfoReportViewBO> salesList = new List<SalesInfoReportViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCashierCollectionSummeryInformationForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CashierId", DbType.Int32, cashierId);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantSales");
                    DataTable table = ds.Tables["RestaurantSales"];
                    salesList = table.AsEnumerable().Select(r =>
                                new SalesInfoReportViewBO
                                {
                                    GuestTotalPaymentInformation = r.Field<string>("GuestTotalPaymentInformation")
                                }).ToList();
                }
            }
            return salesList;
        }
        public List<InvItemTransactionBO> GetDateWiseSalesInfo(DateTime fromDate, DateTime toDate)
        {
            List<InvItemTransactionBO> transactionList = new List<InvItemTransactionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDateWiseSalesInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemTransactionBO entityBO = new InvItemTransactionBO();
                                entityBO.TransactionId = Convert.ToInt32(reader["TransactionId"]);
                                entityBO.TransactionDate = Convert.ToDateTime(reader["TransactionDate"]);
                                entityBO.StartBillNumber = reader["StartBillNumber"].ToString();
                                entityBO.EndingBillNumber = reader["EndingBillNumber"].ToString();
                                entityBO.TotalBillCount = Convert.ToInt16(reader["TotalBillCount"].ToString());
                                entityBO.TotalVoidQuantity = Convert.ToInt16(reader["TotalVoidQuantity"].ToString());
                                entityBO.GrossSalesAmount = Convert.ToDecimal(reader["GrossSalesAmount"].ToString());
                                entityBO.TotalNetSalesAmount = Convert.ToDecimal(reader["TotalNetSalesAmount"].ToString());
                                entityBO.TotalServiceChargeAmount = Convert.ToDecimal(reader["TotalServiceChargeAmount"].ToString());
                                entityBO.TotalVatAmount = Convert.ToDecimal(reader["TotalVatAmount"].ToString());
                                transactionList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return transactionList;
        }
        public List<MachineTestBO> GetMachineTestInformation(int costCenterId, DateTime fromDate, DateTime toDate)
        {
            List<MachineTestBO> transactionList = new List<MachineTestBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMachineTestInformation_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                MachineTestBO entityBO = new MachineTestBO();
                                entityBO.TestId = Convert.ToInt32(reader["TestId"]);
                                entityBO.TestDate = Convert.ToDateTime(reader["TestDate"]);
                                entityBO.TestDateString = reader["TestDateString"].ToString();
                                entityBO.MachineId = Convert.ToInt32(reader["TestId"]);
                                entityBO.MachineName = reader["MachineName"].ToString();
                                entityBO.BeforeMachineReadNumber = reader["BeforeMachineReadNumber"].ToString();
                                entityBO.TestQuantity = Convert.ToDecimal(reader["TestQuantity"].ToString());
                                entityBO.AfterMachineReadNumber = reader["AfterMachineReadNumber"].ToString();
                                entityBO.Remarks = reader["Remarks"].ToString();
                                entityBO.UserName = reader["UserName"].ToString();
                                transactionList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return transactionList;
        }
    }
}
