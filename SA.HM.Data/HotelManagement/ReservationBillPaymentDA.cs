using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;

namespace HotelManagement.Data.HMCommon
{
    public class ReservationBillPaymentDA : BaseService
    {
        public List<ReservationBillPaymentBO> GetReservationBillPaymentInfo()
        {
            List<ReservationBillPaymentBO> bankList = new List<ReservationBillPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservationBillPaymentInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ReservationBillPaymentBO billPayment = new ReservationBillPaymentBO();

                                billPayment.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                billPayment.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                billPayment.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                billPayment.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"].ToString());

                                bankList.Add(billPayment);
                            }
                        }
                    }
                }
            }
            return bankList;
        }
        public Boolean SaveReservationBillPaymentInfo(ReservationBillPaymentBO guestBillPaymentBO, out int tmpPaymentId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveReservationBillPaymentInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int32, guestBillPaymentBO.ReservationId);
                    dbSmartAspects.AddInParameter(command, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                    dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                    dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                    dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                    dbSmartAspects.AddInParameter(command, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                    dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
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
                    dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, guestBillPaymentBO.Remarks);

                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@PaymentId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                }
            }
            return status;
        }

        public Boolean UpdateReservationBillPaymentInfo(ReservationBillPaymentBO guestBillPaymentBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateReservationBillPaymentInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int32, guestBillPaymentBO.PaymentId);
                    dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int32, guestBillPaymentBO.ReservationId);
                    dbSmartAspects.AddInParameter(command, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                    dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                    dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);

                    dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                    dbSmartAspects.AddInParameter(command, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                    dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
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
                    dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, guestBillPaymentBO.Remarks);

                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, guestBillPaymentBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public ReservationBillPaymentBO GetReservationBillPaymentInfoById(int paymentId)
        {
            ReservationBillPaymentBO billPayment = new ReservationBillPaymentBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservationBillPaymentInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int32, paymentId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                billPayment.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                billPayment.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                billPayment.BankId = Convert.ToInt32(reader["BankId"]);
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
                                billPayment.FieldId = Convert.ToInt32(reader["FieldId"]);
                                billPayment.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"].ToString());
                                billPayment.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"].ToString());
                                billPayment.DealId = Convert.ToInt32(reader["DealId"]);
                                billPayment.Remarks = reader["Remarks"].ToString();

                                billPayment.ReservedCompany = Convert.ToString(reader["ReservedCompany"]);
                            }
                        }
                    }
                }
            }
            return billPayment;
        }
        public List<ReservationBillPaymentBO> GetReservationBillPaymentInfoByReservationId(int reservationId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<ReservationBillPaymentBO> billPaymentList = new List<ReservationBillPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservationBillPaymentInfoByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "Increment");
                    DataTable Table = incrementDS.Tables["Increment"];

                    billPaymentList = Table.AsEnumerable().Select(r => new ReservationBillPaymentBO
                    {
                        PaymentId = r.Field<Int32>("PaymentId"),
                        ReservationId = r.Field<Int32>("ReservationId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        PaymentDateStringFormat = r.Field<DateTime>("PaymentDate").ToString("dd/MM/yyyy"),
                        FieldId = r.Field<Int32>("FieldId"),
                        CurrencyAmount = r.Field<decimal>("CurrencyAmount"),
                        PaymentAmount = r.Field<decimal>("PaymentAmount"),
                        IsPaymentTransfered = r.Field<Int32>("IsPaymentTransfered"),
                        PaymentType = r.Field<string>("PaymentType"),                        
                        CreatedByName = r.Field<string>("CreatedByName")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return billPaymentList;
        }
        public List<ReservationBillPaymentBO> GetReservationBillPaymentForReport(string searchCriteria, string type)
        {
            List<ReservationBillPaymentBO> currencyconvList = new List<ReservationBillPaymentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservationBillPaymentForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);
                    dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, type);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ReservationBillPaymentBO entityBO = new ReservationBillPaymentBO();

                                entityBO.ReservationNumber = reader["ReservationNumber"].ToString();  
                                entityBO.ReportPaymentDate = reader["ReportPaymentDate"].ToString();
                                entityBO.FieldId = Convert.ToInt32(reader["FieldId"]);
                                entityBO.CurrencyHead = reader["CurrencyHead"].ToString();
                                entityBO.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"]);    
                                entityBO.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"]);                                
                                entityBO.PaymentType = reader["PaymentType"].ToString();                                

                                currencyconvList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return currencyconvList;
        }
        public List<GuestBillPaymentInvoiceReportViewBO> GetReservationPaymentInvoiceInformation(string searchCriteria)
        {
            List<GuestBillPaymentInvoiceReportViewBO> entityBOList = new List<GuestBillPaymentInvoiceReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationPaymentForDrCr_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestBillPaymentInvoiceReportViewBO entityBO = new GuestBillPaymentInvoiceReportViewBO();

                                entityBO.PaymentMode = reader["PaymentMode"].ToString();
                                entityBO.Debit = Convert.ToDecimal(reader["Debit"]);
                                entityBO.Credit = Convert.ToDecimal(reader["Credit"]);
                                entityBO.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                entityBO.RegistrationNumber = (reader["RegistrationNumber"].ToString());
                                entityBO.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                entityBO.PaymentDateDisplay = (reader["PaymentDateDisplay"].ToString());
                                entityBO.GuestName = reader["GuestName"].ToString();
                                entityBO.Narration = (reader["Narration"].ToString());

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<GuestBillPaymentInvoiceReportViewBO> GetReservationNoShowPaymentInvoiceInformation(string searchCriteria)
        {
            List<GuestBillPaymentInvoiceReportViewBO> entityBOList = new List<GuestBillPaymentInvoiceReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationNoShowPaymentForDrCr_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestBillPaymentInvoiceReportViewBO entityBO = new GuestBillPaymentInvoiceReportViewBO();

                                entityBO.PaymentMode = reader["PaymentMode"].ToString();
                                entityBO.Debit = Convert.ToDecimal(reader["Debit"]);
                                entityBO.Credit = Convert.ToDecimal(reader["Credit"]);
                                entityBO.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                entityBO.RegistrationNumber = (reader["RegistrationNumber"].ToString());
                                entityBO.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }

        public decimal GetMaxRefundForReservationBillPayment(int ReservationId, int editId)
        {
            decimal MaxRefund = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                try
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMaxRefundForReservationBillPayment_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, ReservationId);
                        dbSmartAspects.AddInParameter(cmd, "@EditId", DbType.Int32, editId);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    MaxRefund = Convert.ToDecimal(reader["MaxRefund"].ToString());
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            return MaxRefund;
        }

    }
}
