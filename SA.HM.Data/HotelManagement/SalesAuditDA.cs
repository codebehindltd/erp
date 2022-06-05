using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class SalesAuditDA : BaseService
    {
        public List<SalesAuditReportViewBO> GetSalesAuditInfo(DateTime auditDate)
        {
            List<SalesAuditReportViewBO> salesAudit = new List<SalesAuditReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesAuditInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@AuditDate", DbType.DateTime, auditDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesAudit");
                    DataTable Table = ds.Tables["SalesAudit"];
                    salesAudit = Table.AsEnumerable().Select(r =>
                                new SalesAuditReportViewBO
                                {
                                    ServiceType = r.Field<string>("ServiceType"),
                                    ServiceName = r.Field<string>("ServiceName"),
                                    Covers = r.Field<int>("Covers"),
                                    Pax = r.Field<int>("Pax"),
                                    TotalSales = r.Field<decimal>("TotalSales"),
                                    TotalVat = r.Field<decimal>("TotalVat"),
                                    TotalServiceCharge = r.Field<decimal>("TotalServiceCharge"),
                                    TotalCitySDCharge = r.Field<decimal>("TotalCitySDCharge"),
                                    TotalAdditionalCharge = r.Field<decimal>("TotalAdditionalCharge"),
                                    TotalRoomSale = r.Field<decimal>("TotalRoomSale"),
                                    TotalRoomVat = r.Field<decimal>("TotalRoomVat"),
                                    TotalRoomServiceCharge = r.Field<decimal>("TotalRoomServiceCharge"),
                                    TotalRoomCitySDCharge = r.Field<decimal>("TotalRoomCitySDCharge"),
                                    TotalRoomAdditionalCharge = r.Field<decimal>("TotalRoomAdditionalCharge"),
                                    RoomOccupied = r.Field<decimal>("RoomOccupied"),
                                    OccupencyPercent = r.Field<decimal>("OccupencyPercent"),
                                    DoubleOccupency = r.Field<decimal>("DoubleOccupency"),
                                    NoOfGuest = r.Field<decimal>("NoOfGuest"),
                                    MtdCovers = r.Field<int>("MtdCovers"),
                                    MtdPax = r.Field<int>("MtdPax"),
                                    MtdTotalSales = r.Field<decimal>("MtdTotalSales"),
                                    MtdTotalVat = r.Field<decimal>("MtdTotalVat"),
                                    MtdTotalServiceCharge = r.Field<decimal>("MtdTotalServiceCharge"),
                                    MtdTotalCitySDCharge = r.Field<decimal>("MtdTotalCitySDCharge"),
                                    MtdTotalAdditionalCharge = r.Field<decimal>("MtdTotalAdditionalCharge"),
                                    MtdTotalRoomSale = r.Field<decimal>("MtdTotalRoomSale"),
                                    MtdTotalRoomVat = r.Field<decimal>("MtdTotalRoomVat"),
                                    MtdTotalRoomServiceCharge = r.Field<decimal>("MtdTotalRoomServiceCharge"),
                                    MtdTotalRoomCitySDCharge = r.Field<decimal>("MtdTotalRoomCitySDCharge"),
                                    MtdTotalRoomAdditionalCharge = r.Field<decimal>("MtdTotalRoomAdditionalCharge"),
                                    MtdRoomOccupied = r.Field<decimal>("MtdRoomOccupied"),
                                    MtdOccupencyPercent = r.Field<decimal>("MtdOccupencyPercent"),
                                    MtdDoubleOccupency = r.Field<decimal>("MtdDoubleOccupency"),
                                    MtdNoOfGuest = r.Field<decimal>("MtdNoOfGuest"),
                                    YtdCovers = r.Field<int>("YtdCovers"),
                                    YtdPax = r.Field<int>("YtdPax"),
                                    YtdTotalSales = r.Field<decimal>("YtdTotalSales"),
                                    YtdTotalVat = r.Field<decimal>("YtdTotalVat"),
                                    YtdTotalServiceCharge = r.Field<decimal>("YtdTotalServiceCharge"),
                                    YtdTotalCitySDCharge = r.Field<decimal>("YtdTotalCitySDCharge"),
                                    YtdTotalAdditionalCharge = r.Field<decimal>("YtdTotalAdditionalCharge"),
                                    YtdTotalRoomSale = r.Field<decimal>("YtdTotalRoomSale"),
                                    YtdTotalRoomVat = r.Field<decimal>("YtdTotalRoomVat"),
                                    YtdTotalRoomServiceCharge = r.Field<decimal>("YtdTotalRoomServiceCharge"),
                                    YtdTotalRoomCitySDCharge = r.Field<decimal>("YtdTotalRoomCitySDCharge"),
                                    YtdTotalRoomAdditionalCharge = r.Field<decimal>("YtdTotalRoomAdditionalCharge"),
                                    YtdRoomOccupied = r.Field<decimal>("YtdRoomOccupied"),
                                    YtdOccupencyPercent = r.Field<decimal>("YtdOccupencyPercent"),
                                    YtdDoubleOccupency = r.Field<decimal>("YtdDoubleOccupency"),
                                    YtdNoOfGuest = r.Field<decimal>("YtdNoOfGuest")
                                }).ToList();
                }
            }
            return salesAudit;
        }
        public List<SalesAuditReportViewBO> GetSalesAuditPaymentInfo(DateTime auditDate)
        {
            List<SalesAuditReportViewBO> salesAudit = new List<SalesAuditReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesAuditPaymentInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@AuditDate", DbType.DateTime, auditDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesAuditPayment");
                    DataTable Table = ds.Tables["SalesAuditPayment"];
                    salesAudit = Table.AsEnumerable().Select(r =>
                                new SalesAuditReportViewBO
                                {
                                    ServiceType = r.Field<string>("ServiceType"),
                                    ServiceName = r.Field<string>("ServiceName"),
                                    Covers = r.Field<int>("Covers"),
                                    TotalSales = r.Field<decimal>("TotalSales"),
                                    MtdCovers = r.Field<int>("MtdCovers"),
                                    MtdTotalSales = r.Field<decimal>("MtdTotalSales"),
                                    YtdCovers = r.Field<int>("YtdCovers"),
                                    YtdTotalSales = r.Field<decimal>("YtdTotalSales")
                                }).ToList();
                }
            }
            return salesAudit;
        }
        public List<SalesAuditReportViewBO> GetPOSSalesSummaryInfo(DateTime auditDate)
        {
            List<SalesAuditReportViewBO> salesAudit = new List<SalesAuditReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPOSSalesSummaryInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@AuditDate", DbType.DateTime, auditDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesAudit");
                    DataTable Table = ds.Tables["SalesAudit"];
                    salesAudit = Table.AsEnumerable().Select(r =>
                                new SalesAuditReportViewBO
                                {
                                    ServiceType = r.Field<string>("ServiceType"),
                                    ServiceName = r.Field<string>("ServiceName"),
                                    Covers = r.Field<int>("Covers"),
                                    Pax = r.Field<int>("Pax"),
                                    TotalSales = r.Field<decimal>("TotalSales"),
                                    TotalVat = r.Field<decimal>("TotalVat"),
                                    TotalServiceCharge = r.Field<decimal>("TotalServiceCharge"),
                                    TotalCitySDCharge = r.Field<decimal>("TotalCitySDCharge"),
                                    TotalAdditionalCharge = r.Field<decimal>("TotalAdditionalCharge"),
                                    TotalRoomSale = r.Field<decimal>("TotalRoomSale"),
                                    TotalRoomVat = r.Field<decimal>("TotalRoomVat"),
                                    TotalRoomServiceCharge = r.Field<decimal>("TotalRoomServiceCharge"),
                                    TotalRoomCitySDCharge = r.Field<decimal>("TotalRoomCitySDCharge"),
                                    TotalRoomAdditionalCharge = r.Field<decimal>("TotalRoomAdditionalCharge"),
                                    RoomOccupied = r.Field<decimal>("RoomOccupied"),
                                    OccupencyPercent = r.Field<decimal>("OccupencyPercent"),
                                    DoubleOccupency = r.Field<decimal>("DoubleOccupency"),
                                    NoOfGuest = r.Field<decimal>("NoOfGuest"),
                                    MtdCovers = r.Field<int>("MtdCovers"),
                                    MtdPax = r.Field<int>("MtdPax"),
                                    MtdTotalSales = r.Field<decimal>("MtdTotalSales"),
                                    MtdTotalVat = r.Field<decimal>("MtdTotalVat"),
                                    MtdTotalServiceCharge = r.Field<decimal>("MtdTotalServiceCharge"),
                                    MtdTotalCitySDCharge = r.Field<decimal>("MtdTotalCitySDCharge"),
                                    MtdTotalAdditionalCharge = r.Field<decimal>("MtdTotalAdditionalCharge"),
                                    MtdTotalRoomSale = r.Field<decimal>("MtdTotalRoomSale"),
                                    MtdTotalRoomVat = r.Field<decimal>("MtdTotalRoomVat"),
                                    MtdTotalRoomServiceCharge = r.Field<decimal>("MtdTotalRoomServiceCharge"),
                                    MtdTotalRoomCitySDCharge = r.Field<decimal>("MtdTotalRoomCitySDCharge"),
                                    MtdTotalRoomAdditionalCharge = r.Field<decimal>("MtdTotalRoomAdditionalCharge"),
                                    MtdRoomOccupied = r.Field<decimal>("MtdRoomOccupied"),
                                    MtdOccupencyPercent = r.Field<decimal>("MtdOccupencyPercent"),
                                    MtdDoubleOccupency = r.Field<decimal>("MtdDoubleOccupency"),
                                    MtdNoOfGuest = r.Field<decimal>("MtdNoOfGuest"),
                                    YtdCovers = r.Field<int>("YtdCovers"),
                                    YtdPax = r.Field<int>("YtdPax"),
                                    YtdTotalSales = r.Field<decimal>("YtdTotalSales"),
                                    YtdTotalVat = r.Field<decimal>("YtdTotalVat"),
                                    YtdTotalServiceCharge = r.Field<decimal>("YtdTotalServiceCharge"),
                                    YtdTotalCitySDCharge = r.Field<decimal>("YtdTotalCitySDCharge"),
                                    YtdTotalAdditionalCharge = r.Field<decimal>("YtdTotalAdditionalCharge"),
                                    YtdTotalRoomSale = r.Field<decimal>("YtdTotalRoomSale"),
                                    YtdTotalRoomVat = r.Field<decimal>("YtdTotalRoomVat"),
                                    YtdTotalRoomServiceCharge = r.Field<decimal>("YtdTotalRoomServiceCharge"),
                                    YtdTotalRoomCitySDCharge = r.Field<decimal>("YtdTotalRoomCitySDCharge"),
                                    YtdTotalRoomAdditionalCharge = r.Field<decimal>("YtdTotalRoomAdditionalCharge"),
                                    YtdRoomOccupied = r.Field<decimal>("YtdRoomOccupied"),
                                    YtdOccupencyPercent = r.Field<decimal>("YtdOccupencyPercent"),
                                    YtdDoubleOccupency = r.Field<decimal>("YtdDoubleOccupency"),
                                    YtdNoOfGuest = r.Field<decimal>("YtdNoOfGuest")
                                }).ToList();
                }
            }
            return salesAudit;
        }
        public List<SalesAuditReportViewBO> GetPOSSalesSummaryPaymentInfo(DateTime auditDate)
        {
            List<SalesAuditReportViewBO> salesAudit = new List<SalesAuditReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPOSSalesSummaryPaymentInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@AuditDate", DbType.DateTime, auditDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesAuditPayment");
                    DataTable Table = ds.Tables["SalesAuditPayment"];
                    salesAudit = Table.AsEnumerable().Select(r =>
                                new SalesAuditReportViewBO
                                {
                                    ServiceType = r.Field<string>("ServiceType"),
                                    ServiceName = r.Field<string>("ServiceName"),
                                    Covers = r.Field<int>("Covers"),
                                    TotalSales = r.Field<decimal>("TotalSales"),
                                    MtdCovers = r.Field<int>("MtdCovers"),
                                    MtdTotalSales = r.Field<decimal>("MtdTotalSales"),
                                    YtdCovers = r.Field<int>("YtdCovers"),
                                    YtdTotalSales = r.Field<decimal>("YtdTotalSales")
                                }).ToList();
                }
            }
            return salesAudit;
        }
        public List<ManagerReportBO> GetManagerReportInfo(DateTime auditDate)
        {
            List<ManagerReportBO> salesAudit = new List<ManagerReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetManagerReportInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@AuditDate", DbType.DateTime, auditDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesAuditPayment");
                    DataTable Table = ds.Tables["SalesAuditPayment"];
                    salesAudit = Table.AsEnumerable().Select(r =>
                                new ManagerReportBO
                                {
                                    OrderByNumber = r.Field<int>("OrderByNumber"),
                                    ServiceType = r.Field<string>("ServiceType"),
                                    ServiceName = r.Field<string>("ServiceName"),
                                    Covers = r.Field<decimal>("Covers"),
                                    MtdCovers = r.Field<decimal>("MtdCovers"),
                                    YtdCovers = r.Field<decimal>("YtdCovers"),
                                }).ToList();
                }
            }
            return salesAudit;
        }
        public List<MonthWiseMarketSegmentReportViewBO> GetMonthWiseMarketSegmentRoomInformation(string filterBy, DateTime year , DateTime fromMonth, DateTime toMonth)
        {
            List<MonthWiseMarketSegmentReportViewBO> roomInfo = new List<MonthWiseMarketSegmentReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMonthWiseMarketSegmentRoomInformation_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@filterBy", DbType.String, filterBy);
                    dbSmartAspects.AddInParameter(cmd, "@FromMonth", DbType.DateTime, fromMonth);
                    dbSmartAspects.AddInParameter(cmd, "@ToMonth", DbType.DateTime, toMonth);
                    dbSmartAspects.AddInParameter(cmd, "@Year", DbType.DateTime, year);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RoomInfo");
                    DataTable Table = ds.Tables["RoomInfo"];
                    roomInfo = Table.AsEnumerable().Select(r =>
                                new MonthWiseMarketSegmentReportViewBO
                                {
                                    MarketSegment = r.Field<string>("MarketSegment"),
                                    ServiceMonth = r.Field<string>("ServiceMonth"),
                                    RoomRate = r.Field<decimal?>("RoomRate"),
                                    Value = r.Field<string>("Value")
                                }).ToList();
                }
            }
            return roomInfo;
        }
        public List<DtDMtDYtDMarketSegmentReportViewBO> GetMarketSegmentRoomInformationForDtDMtDYtDReport(string filterBy, DateTime searchDate)
        {
            List<DtDMtDYtDMarketSegmentReportViewBO> roomInfo = new List<DtDMtDYtDMarketSegmentReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMarketSegmentRoomInformationForDtDMtDYtDReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);

                    dbSmartAspects.AddInParameter(cmd, "@filterBy", DbType.String, filterBy);
                    dbSmartAspects.AddInParameter(cmd, "@SearchDate", DbType.DateTime, searchDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RoomInfo");
                    DataTable Table = ds.Tables["RoomInfo"];
                    roomInfo = Table.AsEnumerable().Select(r =>
                                new DtDMtDYtDMarketSegmentReportViewBO
                                {
                                    MarketSegment = r.Field<string>("MarketSegment"),
                                    ColumnGroupId = r.Field<int?>("ColumnGroupId"),
                                    ColumnGroup = r.Field<string>("ColumnGroup"),
                                    SubGroupId = r.Field<int?>("SubGroupId"),
                                    SubGroup = r.Field<string>("SubGroup"),
                                    QtNValue = r.Field<string>("QtNValue")
                                    
                                }).ToList();
                }
            }
            return roomInfo;
        }
        public List<MonthWiseMarketSegmentReportViewBO> GetSalesPersonWiseMarketSegmentRoomInformation(DateTime year, DateTime fromMonth, DateTime toMonth,int refferenceId)
        {
            List<MonthWiseMarketSegmentReportViewBO> roomInfo = new List<MonthWiseMarketSegmentReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesPersonWiseDetailsForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@Year", DbType.DateTime, year);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromMonth);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toMonth);
                    dbSmartAspects.AddInParameter(cmd, "@RefferenceId", DbType.Int32, refferenceId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RoomInfo");
                    DataTable Table = ds.Tables["RoomInfo"];
                    roomInfo = Table.AsEnumerable().Select(r =>
                                new MonthWiseMarketSegmentReportViewBO
                                {
                                    ReferenceName = r.Field<string>("ReferenceName"),
                                    MarketSegment = r.Field<string>("MarketSegment"),
                                    RoomCount = r.Field<int?>("RoomCount"),
                                    Nights = r.Field<int?>("Nights"),
                                    RoomRate = r.Field<decimal?>("RoomRate")

                                }).ToList();
                }
            }
            return roomInfo;
        }
        public List<ContributionAnalysisBO> GetContributionAnalysisForReport(string reportType, int? company, int? reference, int? country, DateTime fromDate, DateTime toDate)
        {
            List<ContributionAnalysisBO> list = new List<ContributionAnalysisBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetContributionAnalysisForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, company);
                    dbSmartAspects.AddInParameter(cmd, "@ReferenceId", DbType.Int32, reference);
                    dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int32, country);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ContributionAnalysisBO bo = new ContributionAnalysisBO();
                                //bo.SummaryDate = Convert.ToDateTime(reader["SummaryDate"]);
                                bo.ContributionTypeWiseId = Convert.ToInt64(reader["ContributionTypeWiseId"]);
                                bo.Name = reader["Name"].ToString();
                                bo.NoOfNight = Convert.ToDecimal(reader["NoOfNight"]);
                                bo.OccupencyPercent = Convert.ToDecimal(reader["OccupencyPercent"]);
                                bo.Pax = Convert.ToDecimal(reader["Pax"]);
                                bo.TotalRoomSale = Convert.ToDecimal(reader["TotalRoomSale"]);
                                bo.AverageRoomRate = Convert.ToDecimal(reader["AverageRoomRate"]);

                                list.Add(bo);
                            }
                        }
                    }
                }
            }
            return list;
        }
    }
}
