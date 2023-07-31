using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class GuestPaymentDA : BaseService
    {
        public List<GuestPaymentReportViewBO> GetGuestPaymentInfo(DateTime startDate, DateTime endDate, string guestType, string paymentType, string paymentMode)
        {
            List<GuestPaymentReportViewBO> guestList = new List<GuestPaymentReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestPaymentInformationForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, startDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, endDate);
                    dbSmartAspects.AddInParameter(cmd, "@GuestType", DbType.String, guestType);
                    dbSmartAspects.AddInParameter(cmd, "@PaymentType", DbType.String, paymentType);
                    dbSmartAspects.AddInParameter(cmd, "@PaymentMode", DbType.String, paymentMode);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestPayment");
                    DataTable Table = ds.Tables["GuestPayment"];
                    guestList = Table.AsEnumerable().Select(r =>
                                new GuestPaymentReportViewBO
                                {
                                    GuestType = r.Field<string>("GuestType"),
                                    PaymentType = r.Field<string>("PaymentType"),
                                    PaymentMode = r.Field<string>("PaymentMode"),
                                    BillOrRoomNumber = r.Field<string>("BillOrRoomNumber"),
                                    PaymentAmount = r.Field<decimal>("PaymentAmount"),
                                    PaymentDate = r.Field<string>("PaymentDate")
                                }).ToList();
                }
            }
            return guestList;
        }

        public List<CurrencyTransactionBO> GetCurrencyTransaction(DateTime? PaymentDateFrom, DateTime? PaymentDateTo, string tansactionNumber, int? CurrencyType, string PaymentMode, int? ReceivedBy, string CostCenter)
        {
            List<CurrencyTransactionBO> currencyTranscation = new List<CurrencyTransactionBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCurrencyTransaction_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentDateFrom", DbType.DateTime, PaymentDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@PaymentDateTo", DbType.DateTime, PaymentDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@TransactionNumber", DbType.String, tansactionNumber);
                    if (CurrencyType != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CurrencyType", DbType.Int32, CurrencyType);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CurrencyType", DbType.Int32, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(PaymentMode))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PaymentMode", DbType.String, PaymentMode);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PaymentMode", DbType.String, DBNull.Value);
                    }

                    if (ReceivedBy != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReceivedBy", DbType.Int32, ReceivedBy);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReceivedBy", DbType.Int32, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(CostCenter))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenter", DbType.String, CostCenter);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenter", DbType.String, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestPayment");
                    DataTable Table = ds.Tables["GuestPayment"];
                    currencyTranscation = Table.AsEnumerable().Select(r =>
                                new CurrencyTransactionBO
                                {
                                    DateNTime = r.Field<DateTime?>("DateNTime"),
                                    InvoiceNumber = r.Field<string>("InvoiceNumber"),
                                    Currency = r.Field<string>("Currency"),
                                    CurrencyAmount = r.Field<decimal?>("CurrencyAmount"),
                                    ConversionRate = r.Field<decimal?>("ConversionRate"),
                                    ConvertedAmount = r.Field<decimal?>("ConvertedAmount"),
                                    PaymentMode = r.Field<string>("PaymentMode"),
                                    TransactionDetails = r.Field<string>("TransactionDetails"),
                                    Remarks = r.Field<string>("Remarks"),
                                    ReceivedBy = r.Field<string>("ReceivedBy"),
                                    CostCenter = r.Field<string>("CostCenter")

                                }).ToList();
                }

                return currencyTranscation;
            }
        }
    }
}
