using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.SalesManagment;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.SalesManagment
{
    public class PMSalesBillPaymentDA : BaseService
    {

        public List<BillReceiveVeiwBO> GetInvoiceDetailsByInvoiceNumberAndCustomerCode(string InvoiceNumber, string Code, string CustomerName)
        {
            List<BillReceiveVeiwBO> invoiceList = new List<BillReceiveVeiwBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvoiceDetailsByInvoiceNumberAndCustomerCode_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@InvoiceNumber", DbType.String, InvoiceNumber);
                    dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, Code);
                    dbSmartAspects.AddInParameter(cmd, "@CustomerName", DbType.String, CustomerName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BillReceiveVeiwBO invoiceViewBO = new BillReceiveVeiwBO();
                                invoiceViewBO.InvoiceId = Convert.ToInt32(reader["InvoiceId"]);
                                invoiceViewBO.InvoiceNumber = reader["InvoiceNumber"].ToString();
                                invoiceViewBO.InvoiceAmount = Convert.ToDecimal(reader["InvoiceAmount"].ToString());
                                invoiceViewBO.DueOrAdvanceAmount = Convert.ToDecimal(reader["DueOrAdvanceAmount"].ToString());
                                invoiceViewBO.CustomerName = reader["CustomerName"].ToString();
                                invoiceViewBO.CustomerCode = reader["CustomerCode"].ToString();
                                invoiceViewBO.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                                invoiceViewBO.BillFromDate = Convert.ToDateTime(reader["BillFromDate"].ToString());
                                invoiceViewBO.BillToDate = Convert.ToDateTime(reader["BillToDate"].ToString());
                                invoiceViewBO.FieldId = Convert.ToInt32(reader["FieldId"]);
                                invoiceList.Add(invoiceViewBO);
                            }
                        }
                    }
                }
            }
            return invoiceList;
        }


        public string GenarateWhereConditionstring(DateTime FormDate, DateTime ToDate, string VoucherNo)
        {
            string Where = string.Empty;
            if (!string.IsNullOrEmpty(FormDate.ToString()) && !string.IsNullOrEmpty(ToDate.ToString()))
            {
                Where += "dbo.FnDate(gdm.VoucherDate) >= dbo.FnDate('" + FormDate + "')  AND dbo.FnDate(gdm.VoucherDate) <= dbo.FnDate('" + ToDate + "')";
            }
            if (!string.IsNullOrEmpty(VoucherNo))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += " AND gdm.VoucherNo = '" + VoucherNo + "'";
                }
                else
                {
                    Where += "  gdm.VoucherNo = '" + VoucherNo + "'";
                }
            }
            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = " WHERE " + Where;
            }
            return Where;
        }

        public bool SaveSalesBillPaymentInfo(PMSalesBillPaymentBO paymentBO, out int tmpPaymentId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveSalesBillPaymentInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@PaymentLocalAmount", DbType.Decimal, paymentBO.PaymentLocalAmount);
                    dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, paymentBO.PaymentDate);
                    dbSmartAspects.AddInParameter(command, "@FieldId", DbType.Int32, paymentBO.FieldId);
                    dbSmartAspects.AddInParameter(command, "@CustomerId", DbType.Int32, paymentBO.CustomerId);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int32, paymentBO.NodeId);
                    dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, paymentBO.CurrencyAmount);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, paymentBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@PaymentId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                }
            }
            return status;
        }

        public bool UpdateSalesBillPaymentInfo(PMSalesBillPaymentBO paymentBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePMSalesBillPaymentInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int32, paymentBO.PaymentId);
                    dbSmartAspects.AddInParameter(command, "@PaymentLocalAmount", DbType.Decimal, paymentBO.PaymentLocalAmount);
                    dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, paymentBO.PaymentDate);
                    dbSmartAspects.AddInParameter(command, "@FieldId", DbType.Int32, paymentBO.FieldId);
                    dbSmartAspects.AddInParameter(command, "@CustomerId", DbType.Int32, paymentBO.CustomerId);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int32, paymentBO.NodeId);
                    dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, paymentBO.CurrencyAmount);

                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, paymentBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public List<PMSalesBillPaymentBO> GetPaymentDetailsByDateRangeAndCustomerId(int customerId, DateTime FromDate, DateTime ToDate)
        {
            List<PMSalesBillPaymentBO> paymentList = new List<PMSalesBillPaymentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPaymentDetailsByDateRangeAndCustomerId"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CustomerId", DbType.Int32, customerId);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, FromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, ToDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMSalesBillPaymentBO paymentBO = new PMSalesBillPaymentBO();
                                paymentBO.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                paymentBO.PaymentLocalAmount = Convert.ToDecimal(reader["PaymentLocalAmount"].ToString());
                                paymentBO.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"].ToString());
                                paymentBO.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                                paymentBO.FieldId = Convert.ToInt32(reader["FieldId"]);
                                paymentBO.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                paymentList.Add(paymentBO);
                            }
                        }
                    }
                }
            }
            return paymentList;
        }

        public Boolean SaveBillPaymentInfo(List<PMSalesBillPaymentBO> guestPaymentDetailList, int CustomerId)
        {

            bool retVal = false;
            bool status = false;
            //int tmpRegistrationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                if (guestPaymentDetailList != null)
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        int countGuestBillPaymentList = 0;
                        //RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                        using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SavePMSalesBillPaymentInfo_SP"))
                        {
                            foreach (PMSalesBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                            {
                                commandGuestBillPayment.Parameters.Clear();

                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CustomerId", DbType.Int32, CustomerId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmout", DbType.Decimal, guestBillPaymentBO.PaymentAmout);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@NodeId", DbType.String, guestBillPaymentBO.NodeId);
                                if (guestBillPaymentBO.PaymentMode == "Card")
                                {
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                }
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                                //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                //tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);

                                countGuestBillPaymentList += dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);

                                //int tmpApprovedId = Convert.ToInt32(commandRoomGuestList.Parameters["@ApprovedId"].Value);
                            }
                        }

                        if (countGuestBillPaymentList > 0)
                        {
                            transction.Commit();
                            retVal = true;
                            status = true;
                        }
                        else
                        {
                            retVal = false;
                            status = false;
                        }
                    }
                }
            }

            return status;
        }


    }
}
