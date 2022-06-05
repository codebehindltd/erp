using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class AllReportDA : BaseService
    {
        public List<SalesTransactionReportViewBO> GetSalesTransactionInfo(string transactionType, string paymentMode, string serviceIdList, DateTime serviceFromDate, DateTime serviceToDate, int receivedBy)
        {
            List<SalesTransactionReportViewBO> cashInfo = new List<SalesTransactionReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFrontOfficeCashForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);

                    dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transactionType);

                    if (paymentMode == "All")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PaymentMode", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PaymentMode", DbType.String, paymentMode);
                    }

                    if (receivedBy == 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReceivedBy", DbType.Int32, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReceivedBy", DbType.Int32, receivedBy);
                    }

                    if (serviceIdList == "-1")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ServiceIdList", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ServiceIdList", DbType.String, serviceIdList);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@ServiceFromDate", DbType.DateTime, serviceFromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceToDate", DbType.DateTime, serviceToDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesTransaction");
                    DataTable Table = ds.Tables["SalesTransaction"];
                    cashInfo = Table.AsEnumerable().Select(r =>
                                new SalesTransactionReportViewBO
                                {
                                    ServiceDate = r.Field<string>("ServiceDate"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    BillNumber = r.Field<string>("BillNumber"),
                                    PaymentDescription = r.Field<string>("PaymentDescription"),
                                    PaymentMode = r.Field<string>("PaymentMode"),
                                    POSTerminalBank = r.Field<string>("POSTerminalBank"),
                                    ReceivedAmount = r.Field<decimal>("ReceivedAmount"),
                                    PaidAmount = r.Field<decimal?>("PaidAmount"),
                                    OperatedBy = r.Field<string>("OperatedBy"),
                                    ReportType = r.Field<string>("ReportType")
                                }).ToList();
                }
            }
            return cashInfo;
        }
        public List<SalesTransactionReportViewBO> GetGuestDuePaymentInfo(string reportType, int transactionId, DateTime fromDate, DateTime toDate)
        {
            List<SalesTransactionReportViewBO> cashInfo = new List<SalesTransactionReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestDuePaymentInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);
                    dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.Int32, transactionId);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesTransaction");
                    DataTable Table = ds.Tables["SalesTransaction"];
                    cashInfo = Table.AsEnumerable().Select(r =>
                                new SalesTransactionReportViewBO
                                {
                                    ServiceDate = r.Field<string>("ServiceDate"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    BillNumber = r.Field<string>("BillNumber"),
                                    PaymentDescription = r.Field<string>("PaymentDescription"),
                                    BillAmount = r.Field<decimal>("BillAmount")
                                }).ToList();
                }
            }
            return cashInfo;
        }
        public List<ServiceSalesInfoReportViewBO> GetSalesInformation(string reportType, DateTime fromDate, DateTime toDate, string filterBy, string serviceIdList, string serviceNameList, string transactionType)
        {
            List<ServiceSalesInfoReportViewBO> cashInfo = new List<ServiceSalesInfoReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                if (reportType != "SalesFormatOne")
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetServiceSalesInfoForReport_SP"))
                    {
                        cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                        dbSmartAspects.AddInParameter(cmd, "@FilterBy", DbType.String, filterBy);
                        dbSmartAspects.AddInParameter(cmd, "@ServiceIdList", DbType.String, serviceIdList);
                        dbSmartAspects.AddInParameter(cmd, "@ServiceNameList", DbType.String, serviceNameList);
                        dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transactionType);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "SalesInformation");
                        DataTable Table = ds.Tables["SalesInformation"];
                        cashInfo = Table.AsEnumerable().Select(r =>
                                    new ServiceSalesInfoReportViewBO
                                    {
                                        ServiceDate = r.Field<string>("ServiceDate"),
                                        ReferenceNo = r.Field<string>("ReferenceNo"),
                                        ServiceId = r.Field<int>("ServiceId"),
                                        ServiceName = r.Field<string>("ServiceName"),
                                        RoomNumber = r.Field<string>("RoomNumber"),
                                        DiscountAmount = r.Field<decimal>("DiscountAmount"),
                                        ServiceRate = r.Field<decimal>("ServiceRate"),
                                        ServiceCharge = r.Field<decimal>("ServiceCharge"),
                                        CitySDCharge = r.Field<decimal>("CitySDCharge"),
                                        VatAmount = r.Field<decimal?>("VatAmount"),
                                        AdditionalCharge = r.Field<decimal>("AdditionalCharge"),
                                        SalesType = r.Field<string>("SalesType"),
                                        IsDiscountHead = r.Field<int>("IsDiscountHead")
                                    }).ToList();
                    }
                }
                else
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFormatOneSalesInformationForReport_SP"))
                    {
                        cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "SalesInformation");
                        DataTable Table = ds.Tables["SalesInformation"];
                        cashInfo = Table.AsEnumerable().Select(r =>
                                    new ServiceSalesInfoReportViewBO
                                    {
                                        FOne_ID = r.Field<string>("FOne_ID"),
                                        FOne_Branch_Code = r.Field<string>("FOne_Branch_Code"),
                                        FOne_CustomerGroup = r.Field<string>("FOne_CustomerGroup"),
                                        FOne_Customer_Name = r.Field<string>("FOne_Customer_Name"),
                                        FOne_Customer_Code = r.Field<string>("FOne_Customer_Code"),
                                        FOne_Delivery_Address = r.Field<string>("FOne_Delivery_Address"),
                                        FOne_Invoice_Date_Time = r.Field<DateTime>("FOne_Invoice_Date_Time"),
                                        FOne_Delivery_Date_Time = r.Field<DateTime>("FOne_Delivery_Date_Time"),
                                        FOne_Reference_No = r.Field<string>("FOne_Reference_No"),
                                        FOne_Comments = r.Field<string>("FOne_Comments"),
                                        FOne_Sale_Type = r.Field<string>("FOne_Sale_Type"),
                                        FOne_Previous_Invoice_No = r.Field<string>("FOne_Previous_Invoice_No"),
                                        FOne_Is_Print = r.Field<string>("FOne_Is_Print"),
                                        FOne_Tender_Id = r.Field<string>("FOne_Tender_Id"),
                                        FOne_Post = r.Field<string>("FOne_Post"),
                                        FOne_LC_Number = r.Field<string>("FOne_LC_Number"),
                                        FOne_Currency_Code = r.Field<string>("FOne_Currency_Code"),
                                        FOne_CommentsD = r.Field<string>("FOne_CommentsD"),
                                        FOne_Item_Code = r.Field<string>("FOne_Item_Code"),
                                        FOne_Item_Name = r.Field<string>("FOne_Item_Name"),
                                        FOne_Quantity = r.Field<decimal?>("FOne_Quantity"),
                                        FOne_NBR_Price = r.Field<decimal?>("FOne_NBR_Price"),
                                        FOne_UOM = r.Field<string>("FOne_UOM"),
                                        FOne_VAT_Rate = r.Field<decimal?>("FOne_VAT_Rate"),
                                        FOne_SD_Rate = r.Field<decimal?>("FOne_SD_Rate"),
                                        FOne_Non_Stock = r.Field<string>("FOne_Non_Stock"),
                                        FOne_Trading_MarkUp = r.Field<string>("FOne_Trading_MarkUp"),
                                        FOne_Type = r.Field<string>("FOne_Type"),
                                        FOne_Discount_Amount = r.Field<decimal?>("FOne_Discount_Amount"),
                                        FOne_Promotional_Quantity = r.Field<decimal?>("FOne_Promotional_Quantity"),
                                        FOne_VAT_Name = r.Field<string>("FOne_VAT_Name"),
                                        FOne_SubTotal = r.Field<decimal?>("FOne_SubTotal"),
                                        FOne_Vehicle_No = r.Field<string>("FOne_Vehicle_No"),
                                        FOne_ExpDescription = r.Field<string>("FOne_ExpDescription"),
                                        FOne_ExpQuantity = r.Field<decimal?>("FOne_ExpQuantity"),
                                        FOne_ExpGrossWeight = r.Field<decimal?>("FOne_ExpGrossWeight"),
                                        FOne_ExpNetWeight = r.Field<decimal?>("FOne_ExpNetWeight"),
                                        FOne_ExpNumberFrom = r.Field<string>("FOne_ExpNumberFrom"),
                                        FOne_ExpNumberTo = r.Field<string>("FOne_ExpNumberTo")
                                    }).ToList();
                    }
                }
            }
            return cashInfo;
        }
        public List<GuestRefReportViewBO> GetGuestRefInfo(int refId, DateTime? fromDate, DateTime? toDate)
        {
            List<GuestRefReportViewBO> cashInfo = new List<GuestRefReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestReferenceInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ReferenceId", DbType.Int32, refId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestReference");
                    DataTable Table = ds.Tables["GuestReference"];
                    cashInfo = Table.AsEnumerable().Select(r =>
                                new GuestRefReportViewBO
                                {
                                    FromDate = r.Field<DateTime>("FromDate"),
                                    ToDate = r.Field<DateTime>("ToDate"),
                                    RegistrationNumber = r.Field<string>("RegistrationNumber"),
                                    GuestName = r.Field<string>("GuestName"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    RoomRent = r.Field<decimal>("RoomRent"),
                                    ArriveDate = r.Field<string>("ArriveDate"),
                                    CheckOutDate = r.Field<string>("CheckOutDate"),
                                    SalesAmount = r.Field<decimal>("SalesAmount"),
                                    ReferanceComissionRate = r.Field<decimal?>("ReferanceComissionRate"),
                                    SatyedNights = r.Field<decimal?>("SatyedNights")
                                }).ToList();
                }
            }
            return cashInfo;
        }
        public List<HotelGuestCompanyBO> GetHotelGuestCompanyInfo(string guestCompany, string contactPerson, string contactNumber, int signupStatusId, int lifeCycleStageId, DateTime? affStartDate, DateTime? affEndDate)
        {
            List<HotelGuestCompanyBO> guestCmpInfo = new List<HotelGuestCompanyBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestCompanyInfoForReport_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(guestCompany))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestCompany", DbType.String, guestCompany);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestCompany", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(contactPerson))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ContactPerson", DbType.String, contactPerson);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ContactPerson", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(contactNumber))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ContactNumber", DbType.String, contactNumber);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ContactNumber", DbType.String, DBNull.Value);
                    }
                    if (signupStatusId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SignupStatusId", DbType.Int32, signupStatusId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SignupStatusId", DbType.Int32, DBNull.Value);
                    }
                    if (lifeCycleStageId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@LifeCycleStageId", DbType.Int32, lifeCycleStageId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@LifeCycleStageId", DbType.Int32, DBNull.Value);
                    }
                    if (affStartDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AffFromDate", DbType.DateTime, affStartDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AffFromDate", DbType.DateTime, DBNull.Value);
                    }
                    if (affEndDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AffToDate", DbType.DateTime, affEndDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AffToDate", DbType.DateTime, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "HotelGuestCompany");
                    DataTable Table = ds.Tables["HotelGuestCompany"];
                    guestCmpInfo = Table.AsEnumerable().Select(r =>
                                new HotelGuestCompanyBO
                                {
                                    CompanyId = r.Field<int>("CompanyId"),
                                    CompanyName = r.Field<string>("CompanyName"),
                                    CompanyAddress = r.Field<string>("CompanyAddress"),
                                    EmailAddress = r.Field<string>("EmailAddress"),
                                    WebAddress = r.Field<string>("WebAddress"),
                                    ContactNumber = r.Field<string>("ContactNumber"),
                                    ContactPerson = r.Field<string>("ContactPerson"),
                                    Remarks = r.Field<string>("Remarks"),
                                    SignupStatus = r.Field<string>("SignupStatus"),
                                    LifeCycleStage = r.Field<string>("LifeCycleStage"),
                                    DiscountPercent = r.Field<decimal?>("DiscountPercent"),
                                    NodeId = r.Field<int?>("NodeId"),
                                    CompanyOwnerId = r.Field<int?>("CompanyOwnerId"),
                                    CreatedBy = r.Field<int>("CreatedBy")
                                }).ToList();
                }
            }
            return guestCmpInfo;
        }
        public List<NightAuditReportViewBO> GetNightAuditInfo(DateTime approveDate, string serviceName)
        {
            List<NightAuditReportViewBO> nightAuditInfo = new List<NightAuditReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTotalServiceAndRoomBillByDate_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ApprovedDate", DbType.DateTime, approveDate);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceName", DbType.String, serviceName);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "NightAudit");
                    DataTable Table = ds.Tables["NightAudit"];
                    nightAuditInfo = Table.AsEnumerable().Select(r =>
                                new NightAuditReportViewBO
                                {
                                    ReportForDate = r.Field<string>("ReportForDate"),
                                    RegistrationNumber = r.Field<string>("RegistrationNumber"),
                                    BillNumber = r.Field<string>("BillNumber"),
                                    ServiceName = r.Field<string>("ServiceName"),
                                    UnitPrice = r.Field<decimal>("UnitPrice"),
                                    Rate = r.Field<decimal>("Rate"),
                                    Quantity = r.Field<decimal>("Quantity"),
                                    VatAmount = r.Field<decimal>("VatAmount"),
                                    Charge = r.Field<decimal>("Charge"),
                                    CitySDCharge = r.Field<decimal>("CitySDCharge"),
                                    AdditionalCharge = r.Field<decimal>("AdditionalCharge"),
                                    TotalGuest = r.Field<int>("TotalGuest")
                                }).ToList();
                }
            }
            return nightAuditInfo;
        }
        public List<RoomOccupancyReportViewBO> GetRoomOccupancyInfo(string reportFor, string reportYear, DateTime fromDate, DateTime toDate)
        {
            List<RoomOccupancyReportViewBO> roomOccupancyInfo = new List<RoomOccupancyReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInnboardRoomTypeInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReportFor", DbType.String, reportFor);
                    dbSmartAspects.AddInParameter(cmd, "@ReportYear", DbType.String, reportYear);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RoomOccupancy");
                    DataTable Table = ds.Tables["RoomOccupancy"];
                    roomOccupancyInfo = Table.AsEnumerable().Select(r =>
                                new RoomOccupancyReportViewBO
                                {
                                    ReportTitle = r.Field<string>("ReportTitle"),
                                    HeadName = r.Field<string>("HeadName").Substring(0, 3),
                                    HeadValue = r.Field<int>("HeadValue"),
                                    RoomType = r.Field<string>("RoomType"),
                                    TotalAmount = r.Field<decimal>("TotalAmount")
                                }).ToList();
                }
            }
            return roomOccupancyInfo;
        }
        public List<DiscountVsActualSaleReportViewBO> GetDiscountVsActualSalesInfo(string reportYear, string durationName)
        {
            List<DiscountVsActualSaleReportViewBO> salesInfo = new List<DiscountVsActualSaleReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInnboardDiscountVSActualSalesInfoWithinYear_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReportYear", DbType.String, reportYear);
                    dbSmartAspects.AddInParameter(cmd, "@ReportDurationName", DbType.String, durationName);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "DiscountVsActualSale");
                    DataTable Table = ds.Tables["DiscountVsActualSale"];
                    salesInfo = Table.AsEnumerable().Select(r =>
                                new DiscountVsActualSaleReportViewBO
                                {
                                    ReportTitle = r.Field<string>("ReportTitle"),
                                    AmountType = r.Field<string>("AmountType"),
                                    MonthName = r.Field<string>("MonthName"),
                                    MonthValue = r.Field<int>("MonthValue"),
                                    Amount = r.Field<decimal>("Amount")
                                }).ToList();
                }
            }
            return salesInfo;
        }
        public List<RoomSalesBCReportViewBO> GetRoomSalesBCInfo(string reportYear, string durationName, string reportFor)
        {
            List<RoomSalesBCReportViewBO> salesInfo = new List<RoomSalesBCReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInnboardRevenueInfoForBarChartWithinYear_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReportYear", DbType.String, reportYear);
                    dbSmartAspects.AddInParameter(cmd, "@ReportDurationName", DbType.String, durationName);
                    dbSmartAspects.AddInParameter(cmd, "@ReportFor", DbType.String, reportFor);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RoomSalesBC");
                    DataTable Table = ds.Tables["RoomSalesBC"];
                    salesInfo = Table.AsEnumerable().Select(r =>
                                new RoomSalesBCReportViewBO
                                {
                                    ReportTitle = r.Field<string>("ReportTitle"),
                                    ServiceName = r.Field<string>("ServiceName"),
                                    MonthName = r.Field<string>("MonthName"),
                                    MonthValue = r.Field<int>("MonthValue"),
                                    TotalAmount = r.Field<decimal>("TotalAmount")
                                }).ToList();
                }
            }
            return salesInfo;
        }
        public List<GuestSourceReportViewBO> GetGuestSourceInfo(DateTime fromDate, DateTime toDate)
        {
            List<GuestSourceReportViewBO> guestSourceInfo = new List<GuestSourceReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestSourceInfoByDateRange_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestSource");
                    DataTable Table = ds.Tables["GuestSource"];
                    guestSourceInfo = Table.AsEnumerable().Select(r =>
                                new GuestSourceReportViewBO
                                {
                                    SourceName = r.Field<string>("SourceName"),
                                    TootalRoom = r.Field<int>("TootalRoom")
                                }).ToList();
                }
            }
            return guestSourceInfo;
        }
        public List<RoomSalesRevReportViewBO> GetRoomSalesRevenueInfo(string reportYear, string durationName, string reportFor)
        {
            List<RoomSalesRevReportViewBO> salesInfo = new List<RoomSalesRevReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInnboardRevenueInfoWithinYear_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReportYear", DbType.String, reportYear);
                    dbSmartAspects.AddInParameter(cmd, "@ReportDurationName", DbType.String, durationName);
                    dbSmartAspects.AddInParameter(cmd, "@ReportFor", DbType.String, reportFor);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RoomSalesRevenue");
                    DataTable Table = ds.Tables["RoomSalesRevenue"];
                    salesInfo = Table.AsEnumerable().Select(r =>
                                new RoomSalesRevReportViewBO
                                {
                                    Code = r.Field<int>("Code"),
                                    ReportTitle = r.Field<string>("ReportTitle"),
                                    ServiceName = r.Field<string>("ServiceName"),
                                    RevenueAmount = r.Field<decimal>("RevenueAmount")
                                }).ToList();
                }
            }
            return salesInfo;
        }
        public List<ReturnedGuestReportViewBO> GetReturnedGuestInfo(DateTime fromDate, DateTime toDate)
        {
            List<ReturnedGuestReportViewBO> guestSourceInfo = new List<ReturnedGuestReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReturnedGuestByDateRange_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ReturnedGuest");
                    DataTable Table = ds.Tables["ReturnedGuest"];
                    guestSourceInfo = Table.AsEnumerable().Select(r =>
                                new ReturnedGuestReportViewBO
                                {
                                    ReturnedGuest = r.Field<string>("ReturnedGuest"),
                                    TotalGuest = r.Field<int>("TotalGuest")
                                }).ToList();
                }
            }
            return guestSourceInfo;
        }
        public List<RoomOccupyReportViewBO> GetRoomOccupancyInfo(DateTime fromDate, DateTime toDate)
        {
            List<RoomOccupyReportViewBO> roomOccupancy = new List<RoomOccupyReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOccupencyOfRoomTypeByDateRange_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RoomOccupancy");
                    DataTable Table = ds.Tables["RoomOccupancy"];
                    roomOccupancy = Table.AsEnumerable().Select(r =>
                                new RoomOccupyReportViewBO
                                {
                                    TotalRoom = r.Field<int>("TotalRoom"),
                                    RoomType = r.Field<string>("RoomType")
                                }).ToList();
                }
            }
            return roomOccupancy;
        }
        public List<GuestsCountryReportViewBO> GetCountriesofGuests(DateTime fromDate, DateTime toDate)
        {
            List<GuestsCountryReportViewBO> guestsCountries = new List<GuestsCountryReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestOfCountryByDateRange_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestsCountries");
                    DataTable Table = ds.Tables["GuestsCountries"];
                    guestsCountries = Table.AsEnumerable().Select(r =>
                                new GuestsCountryReportViewBO
                                {
                                    countryName = r.Field<string>("countryName"),
                                    TootalRoom = r.Field<int>("TootalRoom")
                                }).ToList();
                }
            }
            return guestsCountries;
        }
        public List<MTDInfoBO> GetMonthToDateInfo(DateTime mtdDate)
        {
            List<MTDInfoBO> mtdInfo = new List<MTDInfoBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMonthToDateInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MTDDate", DbType.DateTime, mtdDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestsCountries");
                    DataTable Table = ds.Tables["GuestsCountries"];
                    mtdInfo = Table.AsEnumerable().Select(r =>
                                new MTDInfoBO
                                {
                                    MTDDate = r.Field<DateTime>("MTDDate"),
                                    ActualRoomsOccupied = r.Field<decimal?>("ActualRoomsOccupied"),
                                    Occupency = r.Field<decimal?>("Occupency"),
                                    ActualRoomsRevenue = r.Field<decimal?>("ActualRoomsRevenue"),
                                    AverageRate = r.Field<decimal?>("AverageRate"),
                                    RevenuePerRoom = r.Field<decimal?>("RevenuePerRoom"),
                                    MTDAVGRoomsOccupancy = r.Field<decimal?>("MTDAVGRoomsOccupancy"),
                                    MTDRoomsAverageRevenue = r.Field<decimal?>("MTDRoomsAverageRevenue"),
                                    MTDAverageRate = r.Field<decimal?>("MTDAverageRate"),
                                    MTDRevenuePerRoom = r.Field<decimal?>("MTDRevenuePerRoom")

                                }).ToList();
                }
            }

            return mtdInfo;
        }
        public List<ServiceBillTransferReportViewBO> GetServiceBillTransferInfo(DateTime startDate, DateTime endDate)
        {
            List<ServiceBillTransferReportViewBO> transferInfo = new List<ServiceBillTransferReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTransferGHServiceBillInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, startDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, endDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ServicrBillTransfer");
                    DataTable Table = ds.Tables["ServicrBillTransfer"];
                    transferInfo = Table.AsEnumerable().Select(r =>
                                new ServiceBillTransferReportViewBO
                                {
                                    TransferedDate = r.Field<string>("TransferedDate"),
                                    ServiceBillId = r.Field<int>("ServiceBillId"),
                                    ServiceRate = r.Field<decimal>("ServiceRate"),
                                    ServiceName = r.Field<string>("ServiceName"),
                                    FromRoomNumber = r.Field<string>("FromRoomNumber"),
                                    ToRoomNumber = r.Field<string>("ToRoomNumber"),
                                    TransferedBy = r.Field<string>("TransferedBy"),
                                    BillNumber = r.Field<string>("BillNumber"),
                                    Remarks = r.Field<string>("Remarks")
                                }).ToList();
                }
            }
            return transferInfo;
        }
        public List<VIPGuestReportViewBO> GetVIPGuestInfo(DateTime startDate, DateTime endDate, string guestName, int? countryId, int? companyId, string status)
        {
            List<VIPGuestReportViewBO> guestList = new List<VIPGuestReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVIPGuestInfo_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, startDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, endDate);
                    if (!string.IsNullOrWhiteSpace(guestName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, guestName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value);
                    }
                    if (countryId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int32, countryId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int32, DBNull.Value);
                    }

                    if (companyId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                    }
                    dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, status);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "VIPGuestInfo");
                    DataTable Table = ds.Tables["VIPGuestInfo"];
                    guestList = Table.AsEnumerable().Select(r =>
                                new VIPGuestReportViewBO
                                {
                                    CheckInDate = r.Field<string>("CheckInDate"),
                                    CheckOutDate = r.Field<string>("CheckOutDate"),
                                    GuestName = r.Field<string>("GuestName"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    CompanyName = r.Field<string>("CompanyName"),
                                    //Country = r.Field<string>("ToRoomNumber"),
                                    Email = r.Field<string>("Email"),
                                    CountryName = r.Field<string>("CountryName"),
                                    Remarks = r.Field<string>("Remarks"),
                                    VIPGuestType = r.Field<string>("VIPGuestType")
                                }).ToList();
                }
            }
            return guestList;
        }
        public List<GuestBirthdayReportViewBO> GetGuestBirthdayInfo(DateTime? fromDate, DateTime? toDate, int? companyId, int? countryId, string stayedNight, string guestType)
        {
            List<GuestBirthdayReportViewBO> infoList = new List<GuestBirthdayReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBirthdayReportInfo_SP"))
                {
                    if (fromDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                    }

                    if (toDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Todate", DbType.DateTime, toDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Todate", DbType.DateTime, DBNull.Value);
                    }

                    if (companyId != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                    }

                    if (countryId != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int32, countryId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int32, DBNull.Value);
                    }

                    if (stayedNight != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@StayedNight", DbType.String, stayedNight);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@StayedNight", DbType.String, DBNull.Value);
                    }
                    dbSmartAspects.AddInParameter(cmd, "@GuestType", DbType.String, guestType);


                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestsBirthday");
                    DataTable Table = ds.Tables["GuestsBirthday"];
                    infoList = Table.AsEnumerable().Select(r =>
                                new GuestBirthdayReportViewBO
                                {
                                    ViewDate = r.Field<string>("ViewDate"),
                                    GuestName = r.Field<string>("GuestName"),
                                    GuestDOB = r.Field<string>("GuestDOB"),
                                    CompanyName = r.Field<string>("CompanyName"),
                                    CountryName = r.Field<string>("CountryName"),
                                    GuestPhone = r.Field<string>("GuestPhone"),
                                    GuestEmail = r.Field<string>("GuestEmail"),
                                    SatyedNights = r.Field<int>("SatyedNights")

                                }).ToList();
                }
            }
            return infoList;
        }
        public List<RoomRegistrationBO> GetGuestCreditCardInfo(DateTime? fromDate, DateTime? toDate, int? companyId, string guestName, string regisNo, int roomNo)
        {
            List<RoomRegistrationBO> infoList = new List<RoomRegistrationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestCreditCardInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@Todate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    if (!string.IsNullOrEmpty(guestName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, guestName);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value);
                    if (!string.IsNullOrEmpty(regisNo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RegistrationNo", DbType.String, regisNo);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@RegistrationNo", DbType.String, DBNull.Value);
                    if (roomNo > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RoomNo", DbType.Int32, roomNo);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@RoomNo", DbType.Int32, DBNull.Value);


                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestsCreditCard");
                    DataTable Table = ds.Tables["GuestsCreditCard"];
                    infoList = Table.AsEnumerable().Select(r =>
                                new RoomRegistrationBO
                                {
                                    RegistrationNumber = r.Field<string>("RegistrationNumber"),
                                    GuestName = r.Field<string>("GuestName"),
                                    RoomNo = r.Field<string>("RoomNo"),
                                    ArriveDate = r.Field<DateTime>("ArriveDate"),
                                    ExpectedCheckOutDate = r.Field<DateTime>("ExpectedCheckOutDate"),
                                    CardType = r.Field<string>("CardType"),
                                    CardNumber = r.Field<string>("CardNumber"),
                                    CardExpireDate = r.Field<DateTime?>("CardExpireDate"),
                                    CardHolderName = r.Field<string>("CardHolderName")

                                }).ToList();
                }
            }
            return infoList;
        }
        public List<AdvReseservationForecastViewBO> GetAdvReservationForecastInfo(DateTime? fromDate, DateTime? toDate, int? companyId)
        {
            List<AdvReseservationForecastViewBO> infoList = new List<AdvReseservationForecastViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAdvReservationForecastInfo_SP"))
                {
                    if (fromDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                    }

                    if (toDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Todate", DbType.DateTime, toDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Todate", DbType.DateTime, DBNull.Value);
                    }

                    if (companyId != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "AdvReservationForecast");
                    DataTable Table = ds.Tables["AdvReservationForecast"];
                    infoList = Table.AsEnumerable().Select(r =>
                                new AdvReseservationForecastViewBO
                                {
                                    ReservationDate = r.Field<string>("ReservationDate"),
                                    DateIn = r.Field<DateTime?>("DateIn"),
                                    DateOut = r.Field<DateTime?>("DateOut"),
                                    ReservationNo = r.Field<string>("ReservationNo"),
                                    CompanyName = r.Field<string>("CompanyName"),
                                    OccupiedRoomNo = r.Field<int?>("OccupiedRoomNo"),
                                    RoomNumberList = r.Field<string>("RoomNumberList"),
                                    Occupancy = r.Field<decimal?>("Occupancy"),
                                    TotalNight = r.Field<int?>("TotalNight"),
                                    TotalRevenue = r.Field<decimal?>("TotalRevenue")

                                }).ToList();
                }
            }
            return infoList;
        }
        public List<AdvReseservationForecastViewBO> GetAdvBanquetReservationForecastInfo(DateTime? fromDate, DateTime? toDate, int? companyId)
        {
            List<AdvReseservationForecastViewBO> infoList = new List<AdvReseservationForecastViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAdvBanquetReservationForecastInfo_SP"))
                {
                    if (fromDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                    }

                    if (toDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Todate", DbType.DateTime, toDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Todate", DbType.DateTime, DBNull.Value);
                    }

                    if (companyId != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "AdvReservationForecast");
                    DataTable Table = ds.Tables["AdvReservationForecast"];
                    infoList = Table.AsEnumerable().Select(r =>
                                new AdvReseservationForecastViewBO
                                {
                                    ReservationDate = r.Field<string>("ReservationDate"),
                                    DateIn = r.Field<DateTime?>("DateIn"),
                                    DateOut = r.Field<DateTime?>("DateOut"),
                                    ReservationNo = r.Field<string>("ReservationNo"),
                                    CompanyName = r.Field<string>("CompanyName"),
                                    OccupiedRoomNo = r.Field<int?>("OccupiedRoomNo"),
                                    RoomNumberList = r.Field<string>("RoomNumberList"),
                                    Occupancy = r.Field<decimal?>("Occupancy"),
                                    TotalNight = r.Field<int?>("TotalNight"),
                                    TotalRevenue = r.Field<decimal?>("TotalRevenue")

                                }).ToList();
                }
            }
            return infoList;
        }
        public List<DailyStatisticalReportViewBO> GetDailyStatisticalReportInfo(DateTime? reportDate)
        {
            List<DailyStatisticalReportViewBO> infoList = new List<DailyStatisticalReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDailyStatisticalReportInfo_SP"))
                {
                    if (reportDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReportDate", DbType.DateTime, reportDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReportDate", DbType.DateTime, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "DailyStatisticalReport");
                    DataTable Table = ds.Tables["DailyStatisticalReport"];
                    infoList = Table.AsEnumerable().Select(r =>
                                new DailyStatisticalReportViewBO
                                {
                                    Details = r.Field<string>("Details"),
                                    TotalNo = r.Field<int>("TotalNo")

                                    //TotalCheckOut = r.Field<int?>("TotalCheckOut"),
                                    //TotalExpectedArrival = r.Field<int?>("TotalExpectedArrival"),
                                    //TotalExpectedDeparture = r.Field<int?>("TotalExpectedDeparture"),
                                    //TotalOutOfOrder = r.Field<int?>("TotalOutOfOrder"),
                                    //TotalLongStaying = r.Field<int?>("TotalLongStaying"),
                                    //TotalComplimentary = r.Field<int?>("TotalComplimentary"),
                                    //TotalVIPGuestNo = r.Field<int?>("TotalVIPGuestNo"),
                                    //TotalRoomSold = r.Field<int?>("TotalRoomSold"),
                                    //NetRoomOccupancy = r.Field<decimal?>("NetRoomOccupancy"),
                                    //GrossRoomOccupancy = r.Field<decimal?>("GrossRoomOccupancy"),
                                    //AverageRoomTariff = r.Field<decimal?>("AverageRoomTariff"),
                                    //TotalRoomRevenue = r.Field<decimal?>("TotalRoomRevenue")

                                }).ToList();
                }
            }
            return infoList;
        }
        public List<GuestRebateViewBO> GetGuestRebateInfo(DateTime fromDate, DateTime toDate)
        {
            List<GuestRebateViewBO> guestInfo = new List<GuestRebateViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestRebateInfoReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet reservationDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, reservationDS, "GuestRebateInfo");
                    DataTable Tabel = reservationDS.Tables["GuestRebateInfo"];

                    guestInfo = Tabel.AsEnumerable().Select(r => new GuestRebateViewBO
                    {
                        RegistrationNumber = r.Field<string>("RegistrationNumber"),
                        GuestName = r.Field<string>("GuestName"),
                        ServiceDate = r.Field<DateTime?>("ServiceDate"),
                        ServiceDateString = r.Field<string>("ServiceDateString"),
                        Reference = r.Field<string>("Reference"),
                        RebateAmount = r.Field<decimal?>("RebateAmount"),
                        RebateBy = r.Field<string>("RebateBy"),
                        RebateRemarks = r.Field<string>("RebateRemarks")
                    }).ToList();
                }

                return guestInfo;
            }
        }

        public List<PaxInReportViewBO> GetPaxInReportInfo(DateTime chkInFromDate, DateTime chkInToDate, DateTime chkOutFromDate, DateTime chkOutToDate, string searchtype)
        {
            List<PaxInReportViewBO> paxList = new List<PaxInReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPaxInReportInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ChkInFromDate", DbType.DateTime, chkInFromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ChkInToDate", DbType.DateTime, chkInToDate);
                    dbSmartAspects.AddInParameter(cmd, "@ChkOutFromDate", DbType.DateTime, chkOutFromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ChkOutToDate", DbType.DateTime, chkOutToDate);
                    dbSmartAspects.AddInParameter(cmd, "@SearchType", DbType.String, searchtype);

                    DataSet reservationDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, reservationDS, "PaxInInfo");
                    DataTable Tabel = reservationDS.Tables["PaxInInfo"];

                    paxList = Tabel.AsEnumerable().Select(r => new PaxInReportViewBO
                    {
                        RegistrationNumber = r.Field<string>("RegistrationNumber"),
                        GuestName = r.Field<string>("GuestName"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        CheckInDate = r.Field<DateTime?>("CheckInDate"),
                        CheckOutDate = r.Field<DateTime?>("CheckOutDate"),
                        PaxInRate = r.Field<Decimal>("PaxInRate"),
                        CurrencyName = r.Field<string>("CurrencyName")
                    }).ToList();
                }

                return paxList;
            }
        }
    }
}
