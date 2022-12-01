using HotelManagement.Entity.AirTicketing;
using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.AirTicketing
{
    public class AirlineTicketInfoDA : BaseService
    {
        public bool SaveAirlineTicketInfo(AirlineTicketMasterBO airTicketMasterInfo, List<AirlineTicketInfoBO> AddedSingleTicketInfo, List<GuestBillPaymentBO> AddedPaymentInfo)
        {
            Int64 status = 0, ticketId = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if(airTicketMasterInfo.TicketId > 0)
                        {
                            using (DbCommand cmdOut = dbSmartAspects.GetStoredProcCommand("UpdateATMasterInfo_SP"))
                            {
                                cmdOut.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmdOut, "@TicketId", DbType.Int64, airTicketMasterInfo.TicketId);
                                dbSmartAspects.AddInParameter(cmdOut, "@TransactionType", DbType.String, airTicketMasterInfo.TransactionType);
                                dbSmartAspects.AddInParameter(cmdOut, "@CompanyId", DbType.Int64, airTicketMasterInfo.CompanyId);
                                dbSmartAspects.AddInParameter(cmdOut, "@CompanyName", DbType.String, airTicketMasterInfo.CompanyName);
                                dbSmartAspects.AddInParameter(cmdOut, "@ReferenceId", DbType.Int64, airTicketMasterInfo.ReferenceId);
                                dbSmartAspects.AddInParameter(cmdOut, "@ReferenceName", DbType.String, airTicketMasterInfo.ReferenceName);
                                dbSmartAspects.AddInParameter(cmdOut, "@RegistrationNumber", DbType.String, airTicketMasterInfo.RegistrationNumber);
                                dbSmartAspects.AddInParameter(cmdOut, "@InvoiceAmount", DbType.Decimal, airTicketMasterInfo.InvoiceAmount);
                                dbSmartAspects.AddInParameter(cmdOut, "@LastModifiedBy", DbType.Int64, airTicketMasterInfo.LastModifiedBy);
                                

                                status = dbSmartAspects.ExecuteNonQuery(cmdOut, transction);

                                ticketId = Convert.ToInt64(cmdOut.Parameters["@TicketId"].Value);
                            }
                        }
                        else
                        {
                            using (DbCommand cmdOut = dbSmartAspects.GetStoredProcCommand("SaveATMasterInfo_SP"))
                            {
                                cmdOut.Parameters.Clear();
                                dbSmartAspects.AddInParameter(cmdOut, "@TransactionType", DbType.String, airTicketMasterInfo.TransactionType);
                                dbSmartAspects.AddInParameter(cmdOut, "@CompanyId", DbType.Int64, airTicketMasterInfo.CompanyId);
                                dbSmartAspects.AddInParameter(cmdOut, "@CompanyName", DbType.String, airTicketMasterInfo.CompanyName);
                                dbSmartAspects.AddInParameter(cmdOut, "@ReferenceId", DbType.Int64, airTicketMasterInfo.ReferenceId);
                                dbSmartAspects.AddInParameter(cmdOut, "@ReferenceName", DbType.String, airTicketMasterInfo.ReferenceName);
                                dbSmartAspects.AddInParameter(cmdOut, "@RegistrationNumber", DbType.String, airTicketMasterInfo.RegistrationNumber);
                                dbSmartAspects.AddInParameter(cmdOut, "@InvoiceAmount", DbType.Decimal, airTicketMasterInfo.InvoiceAmount);
                                dbSmartAspects.AddInParameter(cmdOut, "@Status", DbType.String, airTicketMasterInfo.Status);
                                dbSmartAspects.AddInParameter(cmdOut, "@CreatedBy", DbType.Int64, airTicketMasterInfo.CreatedBy);

                                dbSmartAspects.AddOutParameter(cmdOut, "@TicketId", DbType.Int64, sizeof(Int64));

                                status = dbSmartAspects.ExecuteNonQuery(cmdOut, transction);

                                ticketId = Convert.ToInt64(cmdOut.Parameters["@TicketId"].Value);
                            }
                        }
                        
                        
                        if(status > 0 && AddedSingleTicketInfo.Count > 0)
                        {
                            if(airTicketMasterInfo.TicketId > 0)
                            {
                                using (DbCommand cmdPay = dbSmartAspects.GetStoredProcCommand("DeleteAirTicketsByTicketId_SP"))
                                {
                                    cmdPay.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdPay, "@TicketId", DbType.Int64, airTicketMasterInfo.TicketId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdPay, transction);
                                }
                            }
                            foreach (AirlineTicketInfoBO at in AddedSingleTicketInfo)
                            {
                                using (DbCommand cmdSingle = dbSmartAspects.GetStoredProcCommand("SaveSingleTicketInfo_SP"))
                                {
                                    cmdSingle.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdSingle, "@TicketId", DbType.Int64, ticketId);
                                    dbSmartAspects.AddInParameter(cmdSingle, "@ClientName", DbType.String, at.ClientName);
                                    dbSmartAspects.AddInParameter(cmdSingle, "@MobileNumber", DbType.String, at.MobileNumber);
                                    dbSmartAspects.AddInParameter(cmdSingle, "@Email", DbType.String, at.Email);
                                    dbSmartAspects.AddInParameter(cmdSingle, "@Address", DbType.String, at.Address);
                                    dbSmartAspects.AddInParameter(cmdSingle, "@IssueDate", DbType.DateTime, at.IssueDate);
                                    dbSmartAspects.AddInParameter(cmdSingle, "@TicketType", DbType.String, at.TicketType);
                                    dbSmartAspects.AddInParameter(cmdSingle, "@AirlineId", DbType.Int32, at.AirlineId);
                                    dbSmartAspects.AddInParameter(cmdSingle, "@AirlineName", DbType.String, at.AirlineName);
                                    dbSmartAspects.AddInParameter(cmdSingle, "@FlightDate", DbType.DateTime, at.FlightDate);
                                    if(at.ReturnDate == null)
                                    {
                                        dbSmartAspects.AddInParameter(cmdSingle, "@ReturnDate", DbType.DateTime, DBNull.Value);
                                    }
                                    else
                                    {
                                        dbSmartAspects.AddInParameter(cmdSingle, "@ReturnDate", DbType.DateTime, at.ReturnDate);
                                    }
                                    
                                    dbSmartAspects.AddInParameter(cmdSingle, "@TicketNumber", DbType.String, at.TicketNumber);
                                    dbSmartAspects.AddInParameter(cmdSingle, "@PnrNumber", DbType.String, at.PnrNumber);
                                    dbSmartAspects.AddInParameter(cmdSingle, "@InvoiceAmount", DbType.Decimal, at.InvoiceAmount);
                                    dbSmartAspects.AddInParameter(cmdSingle, "@AirlineAmount", DbType.Decimal, at.AirlineAmount);
                                    dbSmartAspects.AddInParameter(cmdSingle, "@RoutePath", DbType.String, at.RoutePath);
                                    dbSmartAspects.AddInParameter(cmdSingle, "@Remarks", DbType.String, at.Remarks);
                                    dbSmartAspects.AddInParameter(cmdSingle, "@CreatedBy", DbType.Int64, airTicketMasterInfo.CreatedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdSingle, transction);
                                }
                            }
                        }

                        if (status > 0 && AddedPaymentInfo.Count > 0)
                        {
                            if (airTicketMasterInfo.TicketId > 0)
                            {
                                using (DbCommand cmdPay = dbSmartAspects.GetStoredProcCommand("DeletePaymentInfoByTicketId_SP"))
                                {
                                    cmdPay.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdPay, "@TicketId", DbType.Int64, airTicketMasterInfo.TicketId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdPay, transction);
                                }
                            }
                            foreach (GuestBillPaymentBO pi in AddedPaymentInfo)
                            {
                                using (DbCommand cmdPay = dbSmartAspects.GetStoredProcCommand("SavePaymentInfoForAirTicket_SP"))
                                {
                                    cmdPay.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdPay, "@TicketId", DbType.Int64, ticketId);
                                    dbSmartAspects.AddInParameter(cmdPay, "@PaymentMode", DbType.String, pi.PaymentMode);
                                    dbSmartAspects.AddInParameter(cmdPay, "@BankName", DbType.String, pi.BankName);
                                    dbSmartAspects.AddInParameter(cmdPay, "@ReceiveAmount", DbType.Decimal, pi.ReceiveAmount);
                                    dbSmartAspects.AddInParameter(cmdPay, "@PaymentModeId", DbType.Int32, pi.PaymentModeId);
                                    dbSmartAspects.AddInParameter(cmdPay, "@CurrencyTypeId", DbType.Int32, pi.CurrencyTypeId);
                                    dbSmartAspects.AddInParameter(cmdPay, "@CurrencyType", DbType.String, pi.CurrencyType);
                                    dbSmartAspects.AddInParameter(cmdPay, "@CardType", DbType.String, pi.CardType);
                                    dbSmartAspects.AddInParameter(cmdPay, "@CardTypeId", DbType.Int32, pi.CardTypeId);
                                    dbSmartAspects.AddInParameter(cmdPay, "@CardNumber", DbType.String, pi.CardNumber);
                                    dbSmartAspects.AddInParameter(cmdPay, "@BankId", DbType.Int32, pi.BankId);
                                    dbSmartAspects.AddInParameter(cmdPay, "@ChequeNumber", DbType.String, pi.ChequeNumber);
                                    dbSmartAspects.AddInParameter(cmdPay, "@CreatedBy", DbType.Int64, airTicketMasterInfo.CreatedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdPay, transction);
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
        
        public bool TicketInformationDelete(long ticketId)
        {
            Int64 status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {                        
                        using (DbCommand cmdDelete = dbSmartAspects.GetStoredProcCommand("TicketInformationDelete_SP"))
                        {
                            cmdDelete.Parameters.Clear();

                            dbSmartAspects.AddInParameter(cmdDelete, "@TicketId", DbType.Int64, ticketId);


                            status = dbSmartAspects.ExecuteNonQuery(cmdDelete, transction);
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

        public List<AirlineTicketMasterBO> GetTicketInformation(DateTime? fromDate, DateTime? toDate, string invoiceNumber, string companyName, string referenceName,
                                                               Int32 userInfoId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<AirlineTicketMasterBO> productReceive = new List<AirlineTicketMasterBO>();
            totalRecords = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTicketInformation_SP"))
                {                    
                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    if (!string.IsNullOrEmpty(invoiceNumber))
                        dbSmartAspects.AddInParameter(cmd, "@InvoiceNumber", DbType.String, invoiceNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@InvoiceNumber", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(companyName))
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(referenceName))
                        dbSmartAspects.AddInParameter(cmd, "@ReferenceName", DbType.String, referenceName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReferenceName", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));
                    
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "TicketInformation");
                    DataTable Table = ds.Tables["TicketInformation"];

                    productReceive = Table.AsEnumerable().Select(r => new AirlineTicketMasterBO
                    {
                        TicketId = r.Field<Int64>("Id"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        BillNumber = r.Field<string>("BillNumber"),
                        TransactionType = r.Field<string>("TransactionType"),
                        TransactionId = r.Field<Int64>("TransactionId"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ReferenceId = r.Field<Int64>("ReferenceId"),
                        ReferenceName = r.Field<string>("ReferenceName"),
                        RegistrationNumber = r.Field<string>("RegistrationNumber"),
                        InvoiceAmount = r.Field<Decimal?>("InvoiceAmount"),
                        Status = r.Field<string>("Status"),
                        CheckedBy = r.Field<Int32>("CheckedBy"),
                        ApprovedBy = r.Field<Int32>("ApprovedBy"),
                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanDelete = r.Field<bool>("IsCanDelete"),
                        IsCanChecked = r.Field<bool>("IsCanChecked"),
                        IsCanApproved = r.Field<bool>("IsCanApproved")

                    }).ToList();

                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value.ToString());
                }
            }

            return productReceive;
        }

        
        public AirlineTicketMasterBO GetATMasterInfo(long ticketId)
        {
            AirlineTicketMasterBO ATMasterInfo = new AirlineTicketMasterBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetATMasterInfoByTicketId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TicketId", DbType.Int64, ticketId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ATMasterInfo.TicketId = Convert.ToInt64(reader["TicketId"]);
                                ATMasterInfo.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                ATMasterInfo.BillNumber = reader["BillNumber"].ToString();
                                ATMasterInfo.TransactionType = reader["TransactionType"].ToString();
                                ATMasterInfo.TransactionId = Convert.ToInt64(reader["TransactionId"]);
                                ATMasterInfo.CompanyName = reader["CompanyName"].ToString();
                                ATMasterInfo.ReferenceId = Convert.ToInt64(reader["ReferenceId"]);
                                ATMasterInfo.ReferenceName = reader["ReferenceName"].ToString();
                            }
                        }
                    }
                }
            }
            return ATMasterInfo;
        }
        public List<AirlineTicketInfoBO> GetATInformationDetails(long ticketId)
        {
            List<AirlineTicketInfoBO> ticketList = new List<AirlineTicketInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetATInformationDetailsByTicketId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TicketId", DbType.Int64, ticketId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ticketList");
                    DataTable Table = ds.Tables["ticketList"];

                    ticketList = Table.AsEnumerable().Select(r => new AirlineTicketInfoBO
                    {
                        ClientName = r.Field<string>("ClientName"),
                        MobileNumber = r.Field<string>("MobileNo"),
                        Email = r.Field<string>("Email"),
                        Address = r.Field<string>("Address"),
                        IssueDate = r.Field<DateTime>("IssueDate"),
                        TicketType = r.Field<string>("TicketType"),
                        AirlineId = r.Field<Int32>("AirlineId"),
                        AirlineName = r.Field<string>("AirlineName"),
                        FlightDate = r.Field<DateTime>("FlightDate"),
                        ReturnDate = r.Field<DateTime?>("ReturnDate"),
                        TicketNumber = r.Field<string>("TicketNumber"),
                        PnrNumber = r.Field<string>("PNRNumber"),
                        InvoiceAmount = r.Field<decimal>("InvoiceAmount"),
                        AirlineAmount = r.Field<decimal>("AirlineAmount"),
                        RoutePath = r.Field<string>("RoutePath"),
                        Remarks = r.Field<string>("Remarks")
                    }).ToList();
                }
            }
            return ticketList;
        }
        
        public List<GuestBillPaymentBO> GetATPaymentInfo(long ticketId)
        {
            List<GuestBillPaymentBO> paymentList = new List<GuestBillPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetATPaymentInfoByTicketId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TicketId", DbType.Int64, ticketId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentList");
                    DataTable Table = ds.Tables["PaymentList"];

                    paymentList = Table.AsEnumerable().Select(r => new GuestBillPaymentBO
                    {                        
                        PaymentMode = r.Field<string>("PaymentMode"),
                        BankName = r.Field<string>("BankName"),
                        ReceiveAmount = r.Field<decimal>("ReceiveAmount"),
                        PaymentModeId = r.Field<Int32>("PaymentModeId"),
                        CurrencyTypeId = r.Field<Int32>("CurrencyTypeId"),
                        CurrencyType = r.Field<string>("CurrencyType"),
                        CardType = r.Field<string>("CardType"),
                        CardTypeId = r.Field<Int32>("CardTypeId"),
                        CardNumber = r.Field<string>("CardNumber"),
                        BankId = r.Field<Int32>("BankId"),
                        ChequeNumber = r.Field<string>("ChequeNumber")
                    }).ToList();
                }
            }
            return paymentList;
        }

        
        public bool TicketInformationCheck(long ticketId, int userInfoId)
        {
            Int64 status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmdCheck = dbSmartAspects.GetStoredProcCommand("CheckTicketInformation_SP"))
                        {
                            cmdCheck.Parameters.Clear();

                            dbSmartAspects.AddInParameter(cmdCheck, "@TicketId", DbType.Int64, ticketId);
                            dbSmartAspects.AddInParameter(cmdCheck, "@UserInfoId", DbType.Int32, userInfoId);


                            status = dbSmartAspects.ExecuteNonQuery(cmdCheck, transction);
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
    }
}
