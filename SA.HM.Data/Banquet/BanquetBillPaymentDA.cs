using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.SalesManagment;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.Banquet;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Data.Banquet
{
    public class BanquetBillPaymentDA : BaseService
    {
        public List<BanquetBillPaymentBO> GetBanquetBillPaymentInfo()
        {
            List<BanquetBillPaymentBO> bankList = new List<BanquetBillPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetBillPaymentInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetBillPaymentBO billPayment = new BanquetBillPaymentBO();

                                billPayment.Id = Convert.ToInt64(reader["Id"]);
                                billPayment.ReservationId = Convert.ToInt64(reader["ReservationId"]);
                                billPayment.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                billPayment.PaymentAmout = Convert.ToDecimal(reader["PaymentAmout"].ToString());

                                bankList.Add(billPayment);
                            }
                        }
                    }
                }
            }
            return bankList;
        }
        public Boolean SaveBanquetBillPaymentInfo(BanquetBillPaymentBO entityBO, out Int64 tmpPaymentId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveBanquetBillPaymentInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int64, entityBO.ReservationId);
                    dbSmartAspects.AddInParameter(command, "@PaymentAmout", DbType.Decimal, entityBO.PaymentAmout);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int64, entityBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@PaymentId", DbType.Int64, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpPaymentId = Convert.ToInt64(command.Parameters["@PaymentId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateBanquetBillPaymentInfo(BanquetBillPaymentBO entityBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBanquetBillPaymentInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, entityBO.Id);
                    dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int64, entityBO.ReservationId);
                    dbSmartAspects.AddInParameter(command, "@PaymentAmout", DbType.Decimal, entityBO.PaymentAmout);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int64, entityBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public BanquetBillPaymentBO GetBanquetBillPaymentInfoById(int paymentId)
        {
            BanquetBillPaymentBO entityBo = new BanquetBillPaymentBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetBillPaymentInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int64, paymentId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBo.Id = Convert.ToInt64(reader["Id"]);
                                entityBo.ReservationId = Convert.ToInt64(reader["ReservationId"]);
                                entityBo.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                entityBo.PaymentAmout = Convert.ToDecimal(reader["PaymentAmout"].ToString());
                                entityBo.DealId = Convert.ToInt64(reader["DealId"]);
                            }
                        }
                    }
                }
            }
            return entityBo;
        }
        public List<BanquetBillPaymentBO> GetBanquetBillPaymentInfoByReservationId(int reservationId)
        {
            List<BanquetBillPaymentBO> entityBOList = new List<BanquetBillPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetBillPaymentInfoByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int64, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetBillPaymentBO entityBO = new BanquetBillPaymentBO();

                                entityBO.Id = Convert.ToInt64(reader["Id"]);
                                entityBO.ReservationId = Convert.ToInt64(reader["ReservationId"]);
                                entityBO.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                entityBO.PaymentAmout = Convert.ToDecimal(reader["PaymentAmout"].ToString());

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<BanquetReservationBillGenerateReportBO> GetBanquetReservationBillGenerateReport(int reservationId)
        {
            List<BanquetReservationBillGenerateReportBO> banquetClientRevenue = new List<BanquetReservationBillGenerateReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("BanquetReservationBillGenerate_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int64, reservationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BanquetReservationBill");
                    DataTable Table = ds.Tables["BanquetReservationBill"];

                    banquetClientRevenue = Table.AsEnumerable().Select(r => new BanquetReservationBillGenerateReportBO
                    {
                        ReservationId = r.Field<long>("Id"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        CompanyId = r.Field<int>("CompanyId"),
                        NodeId = r.Field<int>("NodeId"),
                        Organization = r.Field<string>("Organization"),
                        Address = r.Field<string>("Address"),
                        CityName = r.Field<string>("CityName"),
                        ZipCode = r.Field<string>("ZipCode"),
                        CountryId = r.Field<int?>("CountryId"),
                        PhoneNumber = r.Field<string>("PhoneNumber"),
                        EmailAddress = r.Field<string>("EmailAddress"),
                        BookingFor = r.Field<string>("BookingFor"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        ContactEmail = r.Field<string>("ContactEmail"),
                        ContactPhone = r.Field<string>("ContactPhone"),
                        ArriveDate = r.Field<DateTime?>("ArriveDate"),
                        ArriveDateStamp = r.Field<string>("ArriveDateStamp"),
                        ArriveTimeStamp = r.Field<string>("ArriveTimeStamp"),
                        DepartureDate = r.Field<DateTime?>("DepartureDate"),
                        DepartureDateStamp = r.Field<string>("DepartureDateStamp"),
                        DepartureTimeStamp = r.Field<string>("DepartureTimeStamp"),
                        BanquetId = r.Field<long?>("BanquetId"),
                        BanquetName = r.Field<string>("BanquetName"),
                        OccessionTypeId = r.Field<long?>("OccessionTypeId"),
                        OccessionName = r.Field<string>("OccessionName"),
                        SeatingId = r.Field<long?>("SeatingId"),
                        SeatingName = r.Field<string>("SeatingName"),
                        NumberOfPersonAdult = r.Field<int>("NumberOfPersonAdult"),
                        NumberOfPersonChild = r.Field<int>("NumberOfPersonChild"),
                        RefferenceId = r.Field<long?>("RefferenceId"),
                        RefferenceName = r.Field<string>("RefferenceName"),
                        CancellationReason = r.Field<string>("CancellationReason"),
                        SpecialInstructions = r.Field<string>("SpecialInstructions"),
                        Remarks = r.Field<string>("Remarks"),
                        DetailId = r.Field<long>("DetailId"),
                        ItemType = r.Field<string>("ItemType"),
                        TransactionDate = r.Field<string>("TransactionDate"),
                        TransactionNumber = r.Field<string>("TransactionNumber"),
                        ItemId = r.Field<long?>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        ItemUnit = r.Field<decimal>("ItemUnit"),
                        UnitPrice = r.Field<decimal>("UnitPrice"),
                        TotalPrice = r.Field<decimal>("TotalPrice"),
                        TotalAmount = r.Field<decimal>("TotalAmount"),

                        DiscountType = r.Field<string>("DiscountType"),
                        DiscountAmount = r.Field<decimal>("DiscountAmount"),
                        CalculatedDiscountAmount = r.Field<decimal>("CalculatedDiscountAmount"),
                        DiscountedAmount = r.Field<decimal>("DiscountedAmount"),
                        NetAmount = r.Field<decimal>("NetAmount"),
                        GrandTotal = r.Field<decimal>("GrandTotal"),

                        BanquetRate = r.Field<decimal>("BanquetRate"),
                        IsBanquetBillInclusive = r.Field<int?>("IsBanquetBillInclusive"),

                        ServiceCharge = r.Field<decimal>("ServiceCharge"),
                        IsInvoiceServiceChargeEnable = r.Field<Boolean>("IsInvoiceServiceChargeEnable"),
                        InvoiceServiceCharge = r.Field<decimal>("InvoiceServiceCharge"),

                        CitySDCharge = r.Field<decimal>("CitySDCharge"),
                        IsInvoiceCitySDChargeEnable = r.Field<Boolean>("IsInvoiceCitySDChargeEnable"),
                        InvoiceCitySDCharge = r.Field<decimal>("InvoiceCitySDCharge"),

                        VatAmount = r.Field<decimal>("VatAmount"),
                        IsInvoiceVatAmountEnable = r.Field<Boolean>("IsInvoiceVatAmountEnable"),
                        InvoiceVatAmount = r.Field<decimal>("InvoiceVatAmount"),

                        AdditionalCharge = r.Field<decimal>("AdditionalCharge"),
                        IsInvoiceAdditionalChargeEnable = r.Field<Boolean>("IsInvoiceAdditionalChargeEnable"),
                        InvoiceAdditionalCharge = r.Field<decimal>("InvoiceAdditionalCharge"),

                        CreatedBy = r.Field<int?>("CreatedBy"),
                        CreatedByName = r.Field<string>("CreatedByName"),
                        CreatedDate = r.Field<DateTime?>("CreatedByDate"),
                        LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedByDate"),
                        IsFNBPanelVisible = r.Field<int>("IsFNBPanelVisible"),
                        IsRequisitesPanelVisible = r.Field<int>("IsRequisitesPanelVisible"),
                        
                        PaymentTotal = r.Field<decimal>("PaymentTotal"),
                        OthersBillTotal = r.Field<decimal>("OthersBillTotal"),
                        InnboardVatAmount = r.Field<string>("InnboardVatAmount"),
                        IsInnboardVatServiceChargeEnable = r.Field<int>("IsInnboardVatServiceChargeEnable"),
                        InnboardServiceChargeAmount = r.Field<string>("InnboardServiceChargeAmount")
                    }).ToList();
                }
            }

            return banquetClientRevenue;
        }
        public PaymentSummaryBO GetGuestBillPaymentSummaryInfoByBanquetReservationId(string reservationgistrationIdList, int isToDaysPayment)
        {
            PaymentSummaryBO paymentSummaryBO = new PaymentSummaryBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillPaymentSummaryInfoByBanquetReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.String, reservationgistrationIdList);
                    dbSmartAspects.AddInParameter(cmd, "@IsToDaysPayment", DbType.Int32, isToDaysPayment);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                paymentSummaryBO.TotalPayment = Convert.ToDecimal(reader["TotalPayment"]);
                            }
                        }
                    }
                }
            }
            return paymentSummaryBO;
        }
        public Boolean SaveReservationBillPaymentInfo(BanquetReservationBillPaymentBO guestBillPaymentBO, out long tmpPaymentId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveBanquetReservationBillPaymentInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int64, guestBillPaymentBO.ReservationId);
                    dbSmartAspects.AddInParameter(command, "@FieldId", DbType.Int64, guestBillPaymentBO.FieldId);
                    dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                    dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                    dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                    dbSmartAspects.AddInParameter(command, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                    dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int64, guestBillPaymentBO.BankId);
                    dbSmartAspects.AddInParameter(command, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                    dbSmartAspects.AddInParameter(command, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                    dbSmartAspects.AddInParameter(command, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                    dbSmartAspects.AddInParameter(command, "@ChecqueCompanyId", DbType.Int32, guestBillPaymentBO.ChecqueCompanyId);
                    dbSmartAspects.AddInParameter(command, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                    dbSmartAspects.AddInParameter(command, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                    if (guestBillPaymentBO.PaymentMode == "Card")
                    {
                        dbSmartAspects.AddInParameter(command, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                    }

                    dbSmartAspects.AddInParameter(command, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                    dbSmartAspects.AddInParameter(command, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);


                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int64, guestBillPaymentBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@PaymentId", DbType.Int64, sizeof(Int64));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpPaymentId = Convert.ToInt64(command.Parameters["@PaymentId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateReservationBillPaymentInfo(BanquetReservationBillPaymentBO guestBillPaymentBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBanquetReservationBillPaymentInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, guestBillPaymentBO.Id);
                    dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int64, guestBillPaymentBO.ReservationId);
                    dbSmartAspects.AddInParameter(command, "@FieldId", DbType.Int64, guestBillPaymentBO.FieldId);
                    dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                    dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);

                    dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                    dbSmartAspects.AddInParameter(command, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                    dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int64, guestBillPaymentBO.BankId);
                    dbSmartAspects.AddInParameter(command, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                    dbSmartAspects.AddInParameter(command, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                    dbSmartAspects.AddInParameter(command, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                    dbSmartAspects.AddInParameter(command, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                    dbSmartAspects.AddInParameter(command, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                    if (guestBillPaymentBO.PaymentMode == "Card")
                    {
                        dbSmartAspects.AddInParameter(command, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                    }

                    dbSmartAspects.AddInParameter(command, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                    dbSmartAspects.AddInParameter(command, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);


                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int64, guestBillPaymentBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<BanquetReservationBillPaymentBO> GetReservationBillPaymentInfoByReservationId(long reservationId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<BanquetReservationBillPaymentBO> billPaymentList = new List<BanquetReservationBillPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetReservationBillPaymentInfoByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int64, reservationId);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "Increment");
                    DataTable Table = incrementDS.Tables["Increment"];

                    billPaymentList = Table.AsEnumerable().Select(r => new BanquetReservationBillPaymentBO
                    {
                        Id = r.Field<long>("Id"),
                        ReservationId = r.Field<Int64>("ReservationId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        PaymentDateStringFormat = r.Field<DateTime>("PaymentDate").ToString(),
                        FieldId = r.Field<Int64>("FieldId"),
                        CurrencyAmount = r.Field<decimal>("CurrencyAmount"),
                        PaymentAmount = r.Field<decimal>("PaymentAmount"),
                        IsPaymentTransfered = r.Field<Int32>("IsPaymentTransfered"),
                        PaymentType = r.Field<string>("PaymentType")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return billPaymentList;
        }
        public BanquetReservationBillPaymentBO GetReservationBillPaymentInfoById(long paymentId)
        {
            BanquetReservationBillPaymentBO billPayment = new BanquetReservationBillPaymentBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetReservationBillPaymentInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int64, paymentId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                billPayment.Id = Convert.ToInt64(reader["Id"]);
                                billPayment.ReservationId = Convert.ToInt64(reader["ReservationId"]);
                                billPayment.BankId = Convert.ToInt64(reader["BankId"]);
                                billPayment.BranchName = reader["BranchName"].ToString();
                                billPayment.PaymentMode = reader["PaymentMode"].ToString();
                                billPayment.ChecqueNumber = reader["ChecqueNumber"].ToString();
                                billPayment.ChecqueDate = Convert.ToDateTime(reader["ChecqueDate"].ToString());

                                billPayment.CardNumber = reader["CardNumber"].ToString();
                                billPayment.CardType = reader["CardType"].ToString();
                                billPayment.ExpireDate = Convert.ToDateTime(reader["ExpireDate"].ToString());
                                billPayment.CardHolderName = reader["CardHolderName"].ToString();
                                billPayment.CardReference = reader["CardReference"].ToString();
                                billPayment.PaymentType = reader["PaymentType"].ToString();
                                billPayment.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                billPayment.FieldId = Convert.ToInt64(reader["FieldId"]);
                                billPayment.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"].ToString());
                                billPayment.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"].ToString());
                                billPayment.DealId = Convert.ToInt64(reader["DealId"]);

                                billPayment.ReservedCompany = Convert.ToString(reader["ReservedCompany"]);
                            }
                        }
                    }
                }
            }
            return billPayment;
        }
        public List<BanquetReservationBillPaymentBO> GetBanquetReservationBillPaymentForReport(string searchCriteria, string type)
        {
            List<BanquetReservationBillPaymentBO> reservationList = new List<BanquetReservationBillPaymentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetReservationBillPaymentForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);
                    dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, type);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetReservationBillPaymentBO entityBO = new BanquetReservationBillPaymentBO();

                                entityBO.ReportPaymentDate = reader["ReportPaymentDate"].ToString();
                                entityBO.FieldId = Convert.ToInt64(reader["FieldId"]);
                                entityBO.CurrencyHead = reader["CurrencyHead"].ToString();
                                entityBO.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"]);
                                entityBO.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"]);
                                entityBO.PaymentType = reader["PaymentType"].ToString();

                                reservationList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return reservationList;
        }
    }
}
