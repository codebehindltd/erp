using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Restaurant;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace HotelManagement.Data.HotelManagement
{
    public class GHServiceBillDA : BaseService
    {
        public List<GHServiceBillBO> GetGHServiceBillInfo()
        {
            List<GHServiceBillBO> ghServiceBillList = new List<GHServiceBillBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGHServiceBillInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GHServiceBillBO ghServiceBill = new GHServiceBillBO();

                                ghServiceBill.ServiceBillId = Convert.ToInt32(reader["ServiceBillId"]);
                                ghServiceBill.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                ghServiceBill.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                ghServiceBill.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                ghServiceBill.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                ghServiceBill.ServiceName = reader["ServiceName"].ToString();
                                ghServiceBill.ServiceRate = Convert.ToDecimal(reader["ServiceRate"]);
                                ghServiceBill.ServiceQuantity = Convert.ToInt32(reader["ServiceQuantity"]);
                                ghServiceBill.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);

                                ghServiceBillList.Add(ghServiceBill);
                            }
                        }
                    }
                }
            }
            return ghServiceBillList;
        }
        public Boolean SaveGHServiceBillInfo(GHServiceBillBO ghServiceBill, List<GuestBillPaymentBO> ghServiceBillList, out int serviceBillId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGHServiceBillInfo_SP"))
                    {
                        command.Parameters.Clear();

                        dbSmartAspects.AddInParameter(command, "@ServiceDate", DbType.DateTime, ghServiceBill.ServiceDate);
                        dbSmartAspects.AddInParameter(command, "@BillNumber", DbType.String, ghServiceBill.BillNumber);
                        dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, ghServiceBill.RegistrationId);
                        dbSmartAspects.AddInParameter(command, "@GuestName", DbType.String, ghServiceBill.GuestName);
                        dbSmartAspects.AddInParameter(command, "@ServiceId", DbType.Int32, ghServiceBill.ServiceId);
                        dbSmartAspects.AddInParameter(command, "@ServiceRate", DbType.Decimal, ghServiceBill.ServiceRate);
                        dbSmartAspects.AddInParameter(command, "@ServiceQuantity", DbType.Int32, ghServiceBill.ServiceQuantity);
                        dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, ghServiceBill.DiscountAmount);
                        dbSmartAspects.AddInParameter(command, "@IsComplementary", DbType.Boolean, ghServiceBill.IsComplementary);
                        dbSmartAspects.AddInParameter(command, "@IsPaidService", DbType.Boolean, ghServiceBill.IsPaidService);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, ghServiceBill.Remarks);
                        dbSmartAspects.AddInParameter(command, "@IsServiceChargeEnable", DbType.Boolean, ghServiceBill.IsServiceChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@IsVatEnable", DbType.Boolean, ghServiceBill.IsVatAmountEnable);
                        dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, ghServiceBill.ServiceCharge);
                        dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, ghServiceBill.VatAmount);
                        dbSmartAspects.AddInParameter(command, "@CitySDCharge", DbType.Decimal, ghServiceBill.CitySDCharge);
                        dbSmartAspects.AddInParameter(command, "@AdditionalCharge", DbType.Decimal, ghServiceBill.AdditionalCharge);
                        dbSmartAspects.AddInParameter(command, "@IsCitySDChargeEnable", DbType.Boolean, ghServiceBill.IsCitySDChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@IsAdditionalChargeEnable", DbType.Boolean, ghServiceBill.IsAdditionalChargeEnable);

                        dbSmartAspects.AddInParameter(command, "@RackRate", DbType.Decimal, ghServiceBill.RackRate);

                        if (ghServiceBill.EmpId != null)
                            dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, ghServiceBill.EmpId);
                        else
                            dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, DBNull.Value);

                        if (ghServiceBill.CompanyId != null)
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, ghServiceBill.CompanyId);
                        else
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, DBNull.Value);

                        //dbSmartAspects.AddInParameter(command, "@PaymentMode", DbType.String, serviceBill.PaymentMode);
                        //dbSmartAspects.AddInParameter(command, "@FieldId", DbType.Int32, serviceBill.FieldId);
                        //dbSmartAspects.AddInParameter(command, "@PaymentAmout", DbType.Decimal, serviceBill.PaymentAmout);
                        //dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, serviceBill.CurrencyAmount);

                        //dbSmartAspects.AddInParameter(command, "@CardType", DbType.String, serviceBill.CardType);
                        //dbSmartAspects.AddInParameter(command, "@CardNumber", DbType.String, serviceBill.CardNumber);

                        //if (serviceBill.CardExpireDate != null)
                        //    dbSmartAspects.AddInParameter(command, "@CardExpireDate", DbType.Date, serviceBill.CardExpireDate);
                        //else
                        //    dbSmartAspects.AddInParameter(command, "@CardExpireDate", DbType.Date, DBNull.Value);

                        //dbSmartAspects.AddInParameter(command, "@CardHolderName", DbType.String, serviceBill.CardHolderName);

                        //dbSmartAspects.AddInParameter(command, "@BranchName", DbType.String, serviceBill.BranchName);
                        //dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int32, serviceBill.BankId);
                        //dbSmartAspects.AddInParameter(command, "@ChecqueNumber", DbType.String, serviceBill.ChecqueNumber);
                        //dbSmartAspects.AddInParameter(command, "@ChecqueDate", DbType.DateTime, serviceBill.ChecqueDate);

                        //if (serviceBill.EmpId != null)
                        //    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, serviceBill.EmpId);
                        //else
                        //    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, DBNull.Value);

                        //if (serviceBill.CompanyId != null)
                        //    dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, serviceBill.CompanyId);
                        //else
                        //    dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, ghServiceBill.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@ServiceBillId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command, transaction) > 0 ? true : false;

                        serviceBillId = Convert.ToInt32(command.Parameters["@ServiceBillId"].Value);
                    }

                    if (status)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveGHServiceBillInfoDetails_SP"))
                        {
                            foreach (GuestBillPaymentBO serviceBill in ghServiceBillList)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@ModuleName", DbType.String, "FrontOffice");
                                dbSmartAspects.AddInParameter(cmd, "@PaymentDate", DbType.DateTime, ghServiceBill.ServiceDate);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, serviceBillId);
                                dbSmartAspects.AddInParameter(cmd, "@PaymentType", DbType.String, serviceBill.PaymentType);
                                dbSmartAspects.AddInParameter(cmd, "@PaymentMode", DbType.String, serviceBill.PaymentMode);
                                dbSmartAspects.AddInParameter(cmd, "@FieldId", DbType.Int32, serviceBill.FieldId);
                                dbSmartAspects.AddInParameter(cmd, "@PaymentAmount", DbType.Decimal, serviceBill.PaymentAmount);
                                dbSmartAspects.AddInParameter(cmd, "@CurrencyAmount", DbType.Decimal, serviceBill.CurrencyAmount);
                                dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, serviceBill.CompanyId);
                                dbSmartAspects.AddInParameter(cmd, "@AccountsPostingHeadId", DbType.Int32, serviceBill.AccountsPostingHeadId);
                                dbSmartAspects.AddInParameter(cmd, "@BankId", DbType.Int32, serviceBill.BankId);
                                dbSmartAspects.AddInParameter(cmd, "@BranchName", DbType.String, serviceBill.BranchName);

                                dbSmartAspects.AddInParameter(cmd, "@ChecqueNumber", DbType.String, serviceBill.ChecqueNumber);
                                dbSmartAspects.AddInParameter(cmd, "@ChecqueDate", DbType.DateTime, serviceBill.ChecqueDate);

                                dbSmartAspects.AddInParameter(cmd, "@CardType", DbType.String, serviceBill.CardType);
                                dbSmartAspects.AddInParameter(cmd, "@CardNumber", DbType.String, serviceBill.CardNumber);

                                if (serviceBill.ExpireDate != null)
                                    dbSmartAspects.AddInParameter(cmd, "@ExpireDate", DbType.Date, serviceBill.ExpireDate);
                                else
                                    dbSmartAspects.AddInParameter(cmd, "@ExpireDate", DbType.Date, DBNull.Value);

                                dbSmartAspects.AddInParameter(cmd, "@CardHolderName", DbType.String, serviceBill.CardHolderName);
                                dbSmartAspects.AddInParameter(cmd, "@CardReference", DbType.String, serviceBill.CardReference);

                                dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, ghServiceBill.CreatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;

                            }
                        }
                    }

                    if (status)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
            }

            return status;
        }
        public Boolean UpdateGuestServiceBillInfo(GHServiceBillBO ghServiceBill, List<GuestBillPaymentBO> ghServiceNewBillList, List<GuestBillPaymentBO> ghServiceEditBillList, List<GuestBillPaymentBO> ghServiceDeleteBillList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGuestServiceBillInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ServiceBillId", DbType.Int32, ghServiceBill.ServiceBillId);
                        dbSmartAspects.AddInParameter(command, "@BillNumber", DbType.String, ghServiceBill.BillNumber);
                        dbSmartAspects.AddInParameter(command, "@ServiceDate", DbType.DateTime, ghServiceBill.ServiceDate);
                        dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, ghServiceBill.RegistrationId);
                        dbSmartAspects.AddInParameter(command, "@GuestName", DbType.String, ghServiceBill.GuestName);
                        dbSmartAspects.AddInParameter(command, "@ServiceId", DbType.Int32, ghServiceBill.ServiceId);
                        dbSmartAspects.AddInParameter(command, "@ServiceRate", DbType.Decimal, ghServiceBill.ServiceRate);
                        dbSmartAspects.AddInParameter(command, "@ServiceQuantity", DbType.Int32, ghServiceBill.ServiceQuantity);
                        dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Int32, ghServiceBill.DiscountAmount);
                        dbSmartAspects.AddInParameter(command, "@IsComplementary", DbType.Boolean, ghServiceBill.IsComplementary);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, ghServiceBill.Remarks);
                        dbSmartAspects.AddInParameter(command, "@IsServiceChargeEnable", DbType.Boolean, ghServiceBill.IsServiceChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@IsVatEnable", DbType.Boolean, ghServiceBill.IsVatAmountEnable);
                        dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, ghServiceBill.ServiceCharge);
                        dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, ghServiceBill.VatAmount);
                        dbSmartAspects.AddInParameter(command, "@CitySDCharge", DbType.Decimal, ghServiceBill.CitySDCharge);
                        dbSmartAspects.AddInParameter(command, "@AdditionalCharge", DbType.Decimal, ghServiceBill.AdditionalCharge);
                        dbSmartAspects.AddInParameter(command, "@IsCitySDChargeEnable", DbType.Boolean, ghServiceBill.IsCitySDChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@IsAdditionalChargeEnable", DbType.Boolean, ghServiceBill.IsAdditionalChargeEnable);

                        dbSmartAspects.AddInParameter(command, "@RackRate", DbType.Decimal, ghServiceBill.RackRate);

                        //dbSmartAspects.AddInParameter(command, "@PaymentMode", DbType.String, ghServiceBill.PaymentMode);
                        //dbSmartAspects.AddInParameter(command, "@FieldId", DbType.Int32, ghServiceBill.FieldId);
                        //dbSmartAspects.AddInParameter(command, "@PaymentAmout", DbType.Decimal, ghServiceBill.PaymentAmout);
                        //dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, ghServiceBill.CurrencyAmount);
                        ////dbSmartAspects.AddInParameter(command, "@CardReference", DbType.String, ghServiceBill.CardReference);
                        //dbSmartAspects.AddInParameter(command, "@CardNumber", DbType.String, ghServiceBill.CardNumber);
                        //dbSmartAspects.AddInParameter(command, "@BranchName", DbType.String, ghServiceBill.BranchName);
                        //dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int32, ghServiceBill.BankId);
                        //dbSmartAspects.AddInParameter(command, "@ChecqueNumber", DbType.String, ghServiceBill.ChecqueNumber);
                        //dbSmartAspects.AddInParameter(command, "@ChecqueDate", DbType.DateTime, ghServiceBill.ChecqueDate);

                        if (ghServiceBill.EmpId != null)
                            dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, ghServiceBill.EmpId);
                        else
                            dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, DBNull.Value);

                        if (ghServiceBill.CompanyId != null)
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, ghServiceBill.CompanyId);
                        else
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, ghServiceBill.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(command, transaction) > 0 ? true : false;
                    }

                    if (ghServiceDeleteBillList.Count > 0)
                    {
                        if (status)
                        {
                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteGHServiceBillInfoNDetails_SP"))
                            {
                                foreach (GuestBillPaymentBO serviceBill in ghServiceDeleteBillList)
                                {
                                    cmd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmd, "@PaymentId ", DbType.Int32, serviceBill.PaymentId);
                                    dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, ghServiceBill.ServiceBillId);
                                    dbSmartAspects.AddInParameter(cmd, "@IsDeleteGuestServiceBill", DbType.Boolean, false);

                                    status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                }
                            }
                        }
                    }

                    if (ghServiceEditBillList.Count > 0)
                    {
                        if (status)
                        {
                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateGHServiceBillInfoDetails_SP"))
                            {
                                foreach (GuestBillPaymentBO serviceBill in ghServiceEditBillList)
                                {
                                    cmd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmd, "@PaymentId ", DbType.Int32, serviceBill.PaymentId);
                                    dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, ghServiceBill.ServiceBillId);
                                    dbSmartAspects.AddInParameter(cmd, "@PaymentType", DbType.String, serviceBill.PaymentType);
                                    dbSmartAspects.AddInParameter(cmd, "@PaymentMode", DbType.String, serviceBill.PaymentMode);
                                    dbSmartAspects.AddInParameter(cmd, "@FieldId", DbType.Int32, serviceBill.FieldId);
                                    dbSmartAspects.AddInParameter(cmd, "@PaymentAmount", DbType.Decimal, serviceBill.PaymentAmount);
                                    dbSmartAspects.AddInParameter(cmd, "@CurrencyAmount", DbType.Decimal, serviceBill.CurrencyAmount);

                                    dbSmartAspects.AddInParameter(cmd, "@AccountsPostingHeadId", DbType.Int32, serviceBill.AccountsPostingHeadId);
                                    dbSmartAspects.AddInParameter(cmd, "@BankId", DbType.Int32, serviceBill.BankId);
                                    dbSmartAspects.AddInParameter(cmd, "@BranchName", DbType.Int32, serviceBill.BranchName);

                                    dbSmartAspects.AddInParameter(cmd, "@ChecqueNumber", DbType.String, serviceBill.ChecqueNumber);
                                    dbSmartAspects.AddInParameter(cmd, "@ChecqueDate", DbType.DateTime, serviceBill.ChecqueDate);

                                    dbSmartAspects.AddInParameter(cmd, "@CardType", DbType.String, serviceBill.CardType);
                                    dbSmartAspects.AddInParameter(cmd, "@CardNumber", DbType.String, serviceBill.CardNumber);

                                    if (serviceBill.ExpireDate != null)
                                        dbSmartAspects.AddInParameter(cmd, "@ExpireDate", DbType.Date, serviceBill.ExpireDate);
                                    else
                                        dbSmartAspects.AddInParameter(cmd, "@ExpireDate", DbType.Date, DBNull.Value);

                                    dbSmartAspects.AddInParameter(cmd, "@CardHolderName", DbType.String, serviceBill.CardHolderName);
                                    dbSmartAspects.AddInParameter(cmd, "@CardReference", DbType.String, serviceBill.CardReference);

                                    dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, ghServiceBill.LastModifiedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                }
                            }
                        }
                    }

                    //if (ghServiceBill.ServiceBillId > 0)
                    //{
                    //    if (ghServiceBill.RegistrationId == 0)
                    //    {
                    //        using (DbCommand cmdUPayment = dbSmartAspects.GetStoredProcCommand("UpdateOutSideGuestPaymentInfo_SP"))
                    //        {
                    //            dbSmartAspects.AddInParameter(cmdUPayment, "@ServiceBillId", DbType.Int32, ghServiceBill.ServiceBillId);
                    //            dbSmartAspects.AddInParameter(cmdUPayment, "@PaymentDate ", DbType.DateTime, ghServiceBill.ServiceDate);
                    //            dbSmartAspects.AddInParameter(cmdUPayment, "@LastModifiedBy", DbType.Int32, ghServiceBill.LastModifiedBy);

                    //            status = dbSmartAspects.ExecuteNonQuery(cmdUPayment, transaction) > 0 ? true : false;
                    //        }
                    //    }
                    //}

                    if (ghServiceBill.ServiceBillId > 0)
                    {
                        using (DbCommand cmdUPayment = dbSmartAspects.GetStoredProcCommand("DeleteCompanyLedgerInfoByServiceBillId_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmdUPayment, "@ServiceBillId", DbType.Int32, ghServiceBill.ServiceBillId);
                            Boolean statusDelete = dbSmartAspects.ExecuteNonQuery(cmdUPayment, transaction) > 0 ? true : false;
                        }
                    }

                    if (ghServiceNewBillList.Count > 0)
                    {
                        if (status)
                        {
                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveGHServiceBillInfoDetails_SP"))
                            {
                                foreach (GuestBillPaymentBO serviceBill in ghServiceNewBillList)
                                {
                                    cmd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmd, "@ModuleName", DbType.String, "FrontOffice");
                                    dbSmartAspects.AddInParameter(cmd, "@PaymentDate", DbType.DateTime, ghServiceBill.ServiceDate);
                                    dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, ghServiceBill.ServiceBillId);
                                    dbSmartAspects.AddInParameter(cmd, "@PaymentType", DbType.String, serviceBill.PaymentType);
                                    dbSmartAspects.AddInParameter(cmd, "@PaymentMode", DbType.String, serviceBill.PaymentMode);
                                    dbSmartAspects.AddInParameter(cmd, "@FieldId", DbType.Int32, serviceBill.FieldId);
                                    dbSmartAspects.AddInParameter(cmd, "@PaymentAmount", DbType.Decimal, serviceBill.PaymentAmount);
                                    dbSmartAspects.AddInParameter(cmd, "@CurrencyAmount", DbType.Decimal, serviceBill.CurrencyAmount);
                                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, serviceBill.CompanyId);
                                    dbSmartAspects.AddInParameter(cmd, "@AccountsPostingHeadId", DbType.Int32, serviceBill.AccountsPostingHeadId);
                                    dbSmartAspects.AddInParameter(cmd, "@BankId", DbType.Int32, serviceBill.BankId);
                                    dbSmartAspects.AddInParameter(cmd, "@BranchName", DbType.Int32, serviceBill.BranchName);

                                    dbSmartAspects.AddInParameter(cmd, "@ChecqueNumber", DbType.String, serviceBill.ChecqueNumber);
                                    dbSmartAspects.AddInParameter(cmd, "@ChecqueDate", DbType.DateTime, serviceBill.ChecqueDate);

                                    dbSmartAspects.AddInParameter(cmd, "@CardType", DbType.String, serviceBill.CardType);
                                    dbSmartAspects.AddInParameter(cmd, "@CardNumber", DbType.String, serviceBill.CardNumber);

                                    if (serviceBill.ExpireDate != null)
                                        dbSmartAspects.AddInParameter(cmd, "@ExpireDate", DbType.Date, serviceBill.ExpireDate);
                                    else
                                        dbSmartAspects.AddInParameter(cmd, "@ExpireDate", DbType.Date, DBNull.Value);

                                    dbSmartAspects.AddInParameter(cmd, "@CardHolderName", DbType.String, serviceBill.CardHolderName);
                                    dbSmartAspects.AddInParameter(cmd, "@CardReference", DbType.String, serviceBill.CardReference);

                                    dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, serviceBill.CreatedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;

                                }
                            }
                        }
                    }

                    if (status)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
            }
            return status;
        }
        public Boolean DeleteGHServiceBillInfo(int serviceBillId, int paymentId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteGHServiceBillInfoNDetails_SP"))
                    {
                        cmd.Parameters.Clear();

                        dbSmartAspects.AddInParameter(cmd, "@PaymentId ", DbType.Int32, paymentId);
                        dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, serviceBillId);
                        dbSmartAspects.AddInParameter(cmd, "@IsDeleteGuestServiceBill", DbType.Boolean, true);

                        status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                    }

                    if (status)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
            }

            return status;
        }
        public GHServiceBillBO GetGuestServiceBillInfoById(int serviceBillId)
        {
            GHServiceBillBO ghServiceBill = new GHServiceBillBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestServiceBillInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, serviceBillId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ghServiceBill.ServiceBillId = Convert.ToInt32(reader["ServiceBillId"]);
                                ghServiceBill.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                ghServiceBill.BillNumber = reader["BillNumber"].ToString();
                                ghServiceBill.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                ghServiceBill.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                ghServiceBill.GuestName = reader["GuestName"].ToString();
                                ghServiceBill.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                ghServiceBill.ServiceName = reader["ServiceName"].ToString();
                                ghServiceBill.ServiceRate = Convert.ToDecimal(reader["ServiceRate"]);
                                ghServiceBill.ServiceQuantity = Convert.ToInt32(reader["ServiceQuantity"]);
                                ghServiceBill.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                ghServiceBill.IsComplementary = Convert.ToBoolean(reader["IsComplementary"]);
                                ghServiceBill.Remarks = reader["Remarks"].ToString();
                                ghServiceBill.IsServiceChargeEnable = Convert.ToBoolean(reader["IsServiceChargeEnable"]);
                                ghServiceBill.IsVatAmountEnable = Convert.ToBoolean(reader["IsVatAmountEnable"]);
                                ghServiceBill.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                ghServiceBill.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                ghServiceBill.RackRate = Convert.ToDecimal(reader["RackRate"]);
                                ghServiceBill.TotalCalculatedAmount = Convert.ToDecimal(reader["TotalCalculatedAmount"]);

                                ghServiceBill.IsCitySDChargeEnable = Convert.ToBoolean(reader["IsCitySDChargeEnable"]);
                                ghServiceBill.IsAdditionalChargeEnable = Convert.ToBoolean(reader["IsAdditionalChargeEnable"]);
                                ghServiceBill.CitySDCharge = Convert.ToDecimal(reader["CitySDCharge"]);
                                ghServiceBill.AdditionalCharge = Convert.ToDecimal(reader["AdditionalCharge"]);

                                //ghServiceBill.BankId = Convert.ToInt32(reader["BankId"].ToString());
                                //ghServiceBill.DealId = Convert.ToInt32(reader["DealId"].ToString());
                                ////ghServiceBill.CardReference = reader["CardReference"].ToString();
                                //ghServiceBill.CardNumber = reader["CardNumber"].ToString();
                                //ghServiceBill.ChecqueDate = Convert.ToDateTime(reader["ChecqueDate"].ToString());
                                //ghServiceBill.ChecqueNumber = reader["ChecqueNumber"].ToString();
                                //ghServiceBill.BranchName = reader["BranchName"].ToString();
                                //ghServiceBill.BankId = Convert.ToInt32(reader["BankId"].ToString());
                                //ghServiceBill.PaymentAmout = Convert.ToDecimal(reader["PaymentAmout"].ToString());
                                //ghServiceBill.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"].ToString());
                                //ghServiceBill.FieldId = Convert.ToInt32(reader["FieldId"].ToString());
                                //ghServiceBill.PaymentMode = reader["PaymentMode"].ToString();
                                //ghServiceBill.ConversionRate = Convert.ToDecimal(reader["ConversionRate"].ToString());
                            }
                        }
                    }
                }
            }
            return ghServiceBill;
        }
        public List<GuestBillPaymentBO> GetGHServiceBillInfoDetailsById(int serviceBillId)
        {
            List<GuestBillPaymentBO> ghServiceBillDetails = new List<GuestBillPaymentBO>();
            GuestBillPaymentBO ghServiceBill;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGHServiceBillInfoDetailsById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, serviceBillId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ghServiceBill = new GuestBillPaymentBO();

                                ghServiceBill.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                ghServiceBill.ServiceBillId = Convert.ToInt32(reader["ServiceBillId"]);
                                //ghServiceBill.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);

                                ghServiceBill.DealId = Convert.ToInt32(reader["DealId"].ToString());
                                //ghServiceBill.CardReference = reader["CardReference"].ToString();
                                ghServiceBill.CardType = reader["CardType"].ToString();
                                ghServiceBill.CardNumber = reader["CardNumber"].ToString();

                                if (reader["ExpireDate"].ToString() != "")
                                    ghServiceBill.ExpireDate = Convert.ToDateTime(reader["ExpireDate"]);

                                ghServiceBill.CardHolderName = reader["CardHolderName"].ToString();

                                ghServiceBill.ChecqueDate = Convert.ToDateTime(reader["ChecqueDate"].ToString());
                                ghServiceBill.ChecqueNumber = reader["ChecqueNumber"].ToString();
                                ghServiceBill.BranchName = reader["BranchName"].ToString();
                                ghServiceBill.AccountsPostingHeadId = Convert.ToInt32(reader["AccountsPostingHeadId"].ToString());
                                ghServiceBill.BankId = Convert.ToInt32(reader["BankId"].ToString());
                                ghServiceBill.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"].ToString());
                                ghServiceBill.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"].ToString());
                                ghServiceBill.FieldId = Convert.ToInt32(reader["FieldId"].ToString());
                                ghServiceBill.PaymentMode = reader["PaymentMode"].ToString();
                                ghServiceBill.ConversionRate = Convert.ToDecimal(reader["ConversionRate"].ToString());

                                ghServiceBillDetails.Add(ghServiceBill);
                            }
                        }
                    }
                }
            }

            return ghServiceBillDetails;
        }
        public List<GHServiceBillBO> GetGHServiceBillInfoByRegiId(int registrationId)
        {
            List<GHServiceBillBO> ghServiceBillList = new List<GHServiceBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGHServiceBillInfoByRegiId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GHServiceBillBO ghServiceBill = new GHServiceBillBO();

                                ghServiceBill.ServiceBillId = Convert.ToInt32(reader["ServiceBillId"]);
                                ghServiceBill.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                ghServiceBill.BillNumber = reader["BillNumber"].ToString();
                                ghServiceBill.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                ghServiceBill.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                ghServiceBill.GuestName = reader["GuestName"].ToString();
                                ghServiceBill.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                ghServiceBill.ServiceName = reader["ServiceName"].ToString();
                                ghServiceBill.ServiceRate = Convert.ToDecimal(reader["ServiceRate"]);
                                ghServiceBill.ServiceQuantity = Convert.ToInt32(reader["ServiceQuantity"]);
                                ghServiceBill.DiscountAmount = Convert.ToInt32(reader["DiscountAmount"]);

                                ghServiceBillList.Add(ghServiceBill);
                            }
                        }
                    }
                }
            }
            return ghServiceBillList;
        }
        public List<GHServiceBillBO> GetGHServiceBillInfoByBillNumber(int registrationId, string billNumber)
        {
            List<GHServiceBillBO> ghServiceBillList = new List<GHServiceBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGHServiceBillInfoByBillNumber_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);
                    dbSmartAspects.AddInParameter(cmd, "@BillNumber", DbType.String, billNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GHServiceBillBO ghServiceBill = new GHServiceBillBO();

                                ghServiceBill.ServiceBillId = Convert.ToInt32(reader["ServiceBillId"]);
                                ghServiceBill.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                ghServiceBill.BillNumber = reader["BillNumber"].ToString();
                                ghServiceBill.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                ghServiceBill.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                ghServiceBill.GuestName = reader["GuestName"].ToString();
                                ghServiceBill.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                ghServiceBill.ServiceName = reader["ServiceName"].ToString();
                                ghServiceBill.ServiceRate = Convert.ToDecimal(reader["ServiceRate"]);
                                ghServiceBill.ServiceQuantity = Convert.ToInt32(reader["ServiceQuantity"]);
                                ghServiceBill.DiscountAmount = Convert.ToInt32(reader["DiscountAmount"]);

                                ghServiceBillList.Add(ghServiceBill);
                            }
                        }
                    }
                }
            }
            return ghServiceBillList;
        }
        public bool GetIsServiceBillNumberDuplicate(int serviceBillId, DateTime serviceDate, string billNumber)
        {
            bool isBillNumberDuplicate = true;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GHServiceBillNumberDuplicateCheck_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, serviceBillId);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceDate", DbType.DateTime, serviceDate);
                    dbSmartAspects.AddInParameter(cmd, "@BillNumber", DbType.String, billNumber);

                    dbSmartAspects.AddOutParameter(cmd, "@IsBillNumberDuplicate", DbType.Boolean, sizeof(Boolean));

                    dbSmartAspects.ExecuteNonQuery(cmd);

                    isBillNumberDuplicate = Convert.ToBoolean(cmd.Parameters["@IsBillNumberDuplicate"].Value);
                }
            }

            return isBillNumberDuplicate;
        }
        public List<GHServiceBillBO> GetGHServiceBillInfoBySearchCritaria(string GuestType, string BillNumber, int ServiceBillId, string RoomNumber, DateTime? FromDate, DateTime? ToDate)
        {
            List<GHServiceBillBO> billList = new List<GHServiceBillBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetServiceBillInformationBySearchCriteria_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (ServiceBillId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, ServiceBillId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, DBNull.Value);
                    if (!string.IsNullOrWhiteSpace(BillNumber))
                        dbSmartAspects.AddInParameter(cmd, "@BillNumber", DbType.String, BillNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@BillNumber", DbType.String, DBNull.Value);
                    if (FromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime,FromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                    if (ToDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, ToDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);
                    if (!string.IsNullOrWhiteSpace(RoomNumber))
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, RoomNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, DBNull.Value);
                    dbSmartAspects.AddInParameter(cmd, "@GuestType", DbType.String, GuestType);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GHServiceBillBO billBOBO = new GHServiceBillBO();
                                billBOBO.BillNumber = reader["BillNumber"].ToString();
                                billBOBO.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                billBOBO.GuestName = reader["GuestName"].ToString();
                                billBOBO.ServiceDate = Convert.ToDateTime(reader["ServiceDate"].ToString());
                                billBOBO.ServiceName = reader["ServiceName"].ToString();
                                billBOBO.ServiceRate = Convert.ToDecimal(reader["ServiceRate"].ToString());
                                billBOBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"].ToString());
                                billBOBO.ServiceQuantity = Int32.Parse(reader["ServiceQuantity"].ToString());
                                billBOBO.ServiceBillId = Int32.Parse(reader["ServiceBillId"].ToString());
                                billBOBO.ApprovedStatus = Convert.ToBoolean(reader["ApprovedStatus"]);
                                billBOBO.Reference = reader["Reference"].ToString();
                                billBOBO.RoomNumber = reader["RoomNumber"].ToString();
                                billBOBO.BillEdditableStatus = Int32.Parse(reader["BillEdditableStatus"].ToString());

                                billList.Add(billBOBO);
                            }
                        }
                    }
                }
            }
            return billList;


        }
        public List<GHServiceBillBO> GetGHServiceBillInfoBySearchCritariaForpagination(string guestType, string fromdate, string toDate, string BillNo, string ServiceNo, string RoomNo, int recordPerPage, int pageIndex, out int totalRecords)
        {
            string searchCriteria = string.Empty;
            DateTime fDate = new DateTime();
            DateTime tDate = new DateTime();
            if (!string.IsNullOrWhiteSpace(fromdate))
            {
                fDate = Convert.ToDateTime(fromdate);
            }
            else
                fDate = DateTime.Now.AddDays(-5);
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                tDate = Convert.ToDateTime(toDate);
            }
            else
                tDate = DateTime.Now;

            int serviceId = Convert.ToInt32(ServiceNo);
            searchCriteria = GenerateWhereCondition(guestType, BillNo, serviceId, RoomNo, fDate, tDate);
            List<GHServiceBillBO> billList = new List<GHServiceBillBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetServiceBillInfoBySearchCriteriaForpagination_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);
                    dbSmartAspects.AddInParameter(cmd, "@GuestType", DbType.String, guestType);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GHServiceBillBO billBOBO = new GHServiceBillBO();
                                billBOBO.BillNumber = reader["BillNumber"].ToString();
                                billBOBO.RegistrationNumber = reader["RegistrationNumber"].ToString();


                                billBOBO.GuestName = reader["GuestName"].ToString();

                                string ServiceDate = Convert.ToDateTime(reader["ServiceDate"].ToString()).ToString("dd-MMM-yyyy");
                                billBOBO.ServiceDate = Convert.ToDateTime(ServiceDate);
                                //billBOBO.ServiceDate = Convert.ToDateTime(reader["ServiceDate"].ToString());
                                billBOBO.ServiceName = reader["ServiceName"].ToString();
                                billBOBO.ServiceRate = Convert.ToDecimal(reader["ServiceRate"].ToString());
                                billBOBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"].ToString());
                                billBOBO.ServiceQuantity = Int32.Parse(reader["ServiceQuantity"].ToString());
                                billBOBO.ServiceBillId = Int32.Parse(reader["ServiceBillId"].ToString());
                                billBOBO.ApprovedStatus = Convert.ToBoolean(reader["ApprovedStatus"]);
                                billBOBO.Reference = reader["Reference"].ToString();
                                billBOBO.BillEdditableStatus = Int32.Parse(reader["BillEdditableStatus"].ToString());

                                billList.Add(billBOBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return billList;


        }
        public List<GHServiceBillBO> GetGHServiceBillInfoByRegistrationId(int registrationId)
        {
            string searchCriteria = string.Empty;
            //searchCriteria = GenerateWhereCondition(ghServiceBillBO.GuestType, ghServiceBillBO.BillNumber, ghServiceBillBO.ServiceId, ghServiceBillBO.RoomNumber, ghServiceBillBO.FromDate, ghServiceBillBO.ToDate);
            List<GHServiceBillBO> billList = new List<GHServiceBillBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGHServiceBillInfoByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GHServiceBillBO billBOBO = new GHServiceBillBO();
                                billBOBO.BillNumber = reader["BillNumber"].ToString();
                                billBOBO.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                billBOBO.GuestName = reader["GuestName"].ToString();
                                billBOBO.ServiceDate = Convert.ToDateTime(reader["ServiceDate"].ToString());
                                billBOBO.ServiceId = Int32.Parse(reader["ServiceId"].ToString());
                                billBOBO.ServiceName = reader["ServiceName"].ToString();
                                billBOBO.ServiceRate = Convert.ToDecimal(reader["ServiceRate"].ToString());
                                billBOBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"].ToString());
                                billBOBO.ServiceQuantity = Int32.Parse(reader["ServiceQuantity"].ToString());
                                billBOBO.ServiceBillId = Int32.Parse(reader["ServiceBillId"].ToString());
                                billBOBO.ApprovedStatus = Convert.ToBoolean(reader["ApprovedStatus"]);
                                billBOBO.Reference = reader["Reference"].ToString();
                                billBOBO.BillEdditableStatus = Int32.Parse(reader["BillEdditableStatus"].ToString());
                                billBOBO.ModuleName = reader["ModuleName"].ToString();

                                billList.Add(billBOBO);
                            }
                        }
                    }
                }
            }
            return billList;


        }
        public List<GHServiceBillBO> GetGuestAchievedServiceInfoByRegistrationId(int registrationId)
        {
            string searchCriteria = string.Empty;
            //searchCriteria = GenerateWhereCondition(ghServiceBillBO.GuestType, ghServiceBillBO.BillNumber, ghServiceBillBO.ServiceId, ghServiceBillBO.RoomNumber, ghServiceBillBO.FromDate, ghServiceBillBO.ToDate);
            List<GHServiceBillBO> billList = new List<GHServiceBillBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestAchievedServiceInfoByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GHServiceBillBO billBOBO = new GHServiceBillBO();
                                billBOBO.ServiceId = Int32.Parse(reader["ServiceId"].ToString());
                                billBOBO.ServiceName = reader["ServiceName"].ToString();
                                billBOBO.ModuleName = reader["ModuleName"].ToString();

                                billList.Add(billBOBO);
                            }
                        }
                    }
                }
            }
            return billList;


        }
        private string GenerateWhereCondition(string GuestType, string BillNumber, int ServiceBillId, string RoomNumber, DateTime FromDate, DateTime ToDate)
        {
            string Where = string.Empty,
            Condition = string.Empty;

            if (GuestType == "InHouseGuest")
            {
                if (ServiceBillId > 0)
                {
                    Where = " ss.ServiceId  =" + ServiceBillId + "";
                }
                if (!string.IsNullOrWhiteSpace(RoomNumber))
                {
                    if (!string.IsNullOrEmpty(Where))
                    {
                        Where += " AND rn.RoomNumber = '" + RoomNumber + "'";
                    }
                    else
                    {
                        Where = " rn.RoomNumber = '" + RoomNumber + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(BillNumber))
                {
                    if (!string.IsNullOrEmpty(Where))
                    {
                        Where += " AND sb.BillNumber LIKE '%" + BillNumber + "%'";
                    }
                    else
                    {
                        Where = " sb.BillNumber LIKE '%" + BillNumber + "%'";
                    }
                }
                if (string.IsNullOrWhiteSpace(BillNumber))
                {
                    if (!string.IsNullOrEmpty(Where))
                    {
                        //Where += " AND  hg.CheckOutDate IS NULL";
                        Where += " AND sb.RegistrationId != 0 ";

                        Where += " AND  ( dbo.FnDate(sb.ServiceDate) >= dbo.FnDate('" + FromDate.ToString("yyyy-MM-dd") + "'";
                        Where += " ) AND dbo.FnDate(sb.ServiceDate) <= dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "'" + ") )";

                    }
                    else
                    {
                        //Where += " hg.CheckOutDate IS NULL";
                        Where += " sb.RegistrationId != 0 ";

                        Where += " AND  ( dbo.FnDate(sb.ServiceDate) >= dbo.FnDate('" + FromDate.ToString("yyyy-MM-dd") + "'";
                        Where += " ) AND dbo.FnDate(sb.ServiceDate) <= dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "'" + ") )";
                    }
                }
                

            }
            else if (GuestType == "OutSideGuest")
            {

                if (ServiceBillId > 0)
                {
                    Where = " ss.ServiceId  =" + ServiceBillId + "";
                }

                if (!string.IsNullOrWhiteSpace(BillNumber))
                {
                    if (!string.IsNullOrEmpty(Where))
                    {
                        Where += " AND sb.BillNumber LIKE '%" + BillNumber + "%'";
                    }
                    else
                    {
                        Where = " sb.BillNumber LIKE '%" + BillNumber + "%'";
                    }
                }
                if (string.IsNullOrWhiteSpace(BillNumber))
                {
                    if (!string.IsNullOrEmpty(Where))
                    {
                        Where += " AND  ( dbo.FnDate(sb.ServiceDate) >= dbo.FnDate('" + FromDate.ToString("yyyy-MM-dd") + "'";
                        Where += " ) AND dbo.FnDate(sb.ServiceDate) <= dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "'" + ") )";
                        Where += " AND sb.RegistrationId = 0 ";

                    }
                    else
                    {
                        Where += "  ( dbo.FnDate(sb.ServiceDate) >= dbo.FnDate('" + FromDate.ToString("yyyy-MM-dd") + "'";
                        Where += " ) AND dbo.FnDate(sb.ServiceDate) <= dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "'" + ") )";
                        Where += " AND sb.RegistrationId = 0 ";
                    }
                }
                
            }
            else if (GuestType == "0")
            {

                if (ServiceBillId > 0)
                {
                    Where = " sb.ServiceId  =" + ServiceBillId + "";
                }

                if (!string.IsNullOrWhiteSpace(BillNumber))
                {
                    if (!string.IsNullOrEmpty(Where))
                    {
                        Where += " AND sb.BillNumber LIKE '%" + BillNumber + "%'";
                    }
                    else
                    {
                        Where = " sb.BillNumber LIKE '%" + BillNumber + "%'";
                    }
                }
                if (string.IsNullOrWhiteSpace(BillNumber))
                {
                    if (!string.IsNullOrEmpty(Where))
                    {

                        Where += " AND  ( dbo.FnDate(sb.ServiceDate) >= dbo.FnDate('" + FromDate.ToString("yyyy-MM-dd") + "'";
                        Where += " ) AND dbo.FnDate(sb.ServiceDate) <= dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "'" + ") )";

                    }
                    else
                    {
                        Where += "  ( dbo.FnDate(sb.ServiceDate) >= dbo.FnDate('" + FromDate.ToString("yyyy-MM-dd") + "'";
                        Where += " ) AND dbo.FnDate(sb.ServiceDate) <= dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "'" + ") )";
                    }
                }

            }

            //if (!string.IsNullOrEmpty(Where))
            //{
            //    Where = " WHERE " + Where;
            //}

            if (!string.IsNullOrEmpty(Where))
            {
                Where = " WHERE " + Where + " ORDER BY sb.ServiceDate DESC ";
            }
            return Where;
        }
        public int CheckDuplicateByServiceAndBillCritaria(int registrationId, int serviceBillId, string guestType, string billNumber, int ServiceId, DateTime ServiceDate)
        {
            int isSucceded = 1;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("CheckDuplicateByServiceAndBillCritaria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, serviceBillId);
                    dbSmartAspects.AddInParameter(cmd, "@GuestType", DbType.String, guestType);
                    dbSmartAspects.AddInParameter(cmd, "@BillNumber", DbType.String, billNumber);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceId", DbType.Int32, ServiceId);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceDate", DbType.DateTime, ServiceDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                //isSucceded = 0;
                                GHServiceBillBO billBOBO = new GHServiceBillBO();

                                billBOBO.ServiceBillId = Int32.Parse(reader["ServiceBillId"].ToString());
                                if (billBOBO.ServiceBillId > 0)
                                {
                                    isSucceded = 0;
                                    return isSucceded;
                                }
                            }
                        }
                    }
                }
            }
            return isSucceded;
        }
        public Boolean TransferGHServiceBillInfo(List<GHServiceBillBO> serviceBillBOList, int fromRegId, int transferRoomId, int userInfoId)
        {
            Boolean status = false;
            int transactionCount = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("TransferGHServiceBillInfo_SP"))
                    {
                        foreach (GHServiceBillBO serviceBill in serviceBillBOList)
                        {
                            cmd.Parameters.Clear();

                            dbSmartAspects.AddInParameter(cmd, "@ModuleName", DbType.String, serviceBill.ModuleName);
                            dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, serviceBill.ServiceBillId);
                            dbSmartAspects.AddInParameter(cmd, "@Remarks", DbType.String, serviceBill.Remarks);
                            dbSmartAspects.AddInParameter(cmd, "@FromRegId", DbType.Int32, fromRegId);
                            dbSmartAspects.AddInParameter(cmd, "@TransferRoomId", DbType.Int32, transferRoomId);
                            dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, userInfoId);

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                            if (status)
                            {
                                transactionCount = transactionCount + 1;
                            }

                        }
                    }


                    if (serviceBillBOList.Count == transactionCount)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
            }

            return status;
        }
        public List<RestaurantBillReportBO> GetGuestHouseServiceBillInfoByServiceBillIdForReport(int billId)
        {
            List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelGuestHouseServiceBill"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, billId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantBill");
                    DataTable Table = ds.Tables["RestaurantBill"];
                    restaurantBill = Table.AsEnumerable().Select(r => new RestaurantBillReportBO
                    {
                        BillId = r.Field<int?>("BillId"),
                        BillDate = r.Field<string>("BillDate"),
                        BillNumber = r.Field<string>("BillNumber"),
                        TableNumber = r.Field<string>("TableNumber"),
                        PaxQuantity = r.Field<int?>("PaxQuantity"),
                        CostCenter = r.Field<string>("CostCenter"),
                        CustomerName = r.Field<string>("CustomerName"),
                        PayMode = r.Field<string>("PayMode"),
                        BankId = r.Field<int?>("BankId"),
                        CardNumber = r.Field<string>("CardNumber"),
                        TotalSales = r.Field<decimal?>("TotalSales"),
                        DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                        DiscountedAmount = r.Field<decimal?>("DiscountedAmount"),
                        NetAmount = r.Field<decimal?>("NetAmount"),
                        ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                        VatAmount = r.Field<decimal?>("VatAmount"),
                        CitySDCharge = r.Field<decimal?>("CitySDCharge"),
                        AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                        GrandTotal = r.Field<decimal?>("GrandTotal"),
                        GuestTotalPaymentAmount = r.Field<decimal?>("GuestTotalPaymentAmount"),
                        GuestTotalRefundAmount = r.Field<decimal?>("GuestTotalRefundAmount"),
                        KotDetailId = r.Field<int?>("KotDetailId"),
                        KotId = r.Field<int?>("KotId"),
                        ItemType = r.Field<string>("ItemType"),
                        ItemId = r.Field<int?>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        ItemCode = r.Field<string>("ItemCode"),
                        Category = r.Field<string>("Category"),
                        ItemUnit = r.Field<decimal?>("ItemUnit"),
                        UnitRate = r.Field<decimal?>("UnitRate"),
                        Amount = r.Field<decimal?>("Amount"),
                        UserName = r.Field<string>("UserName"),
                        WaiterName = r.Field<string>("WaiterName"),
                        IsInclusiveBill = r.Field<int?>("IsInclusiveBill"),
                        IsVatServiceChargeEnable = r.Field<int?>("IsVatServiceChargeEnable"),
                        TransactionType = r.Field<string>("TransactionType"),
                        Remarks = r.Field<string>("Remarks")
                    }).ToList();
                }
            }

            return restaurantBill;
        }
        public List<BillInfoViewBO> GetBillInfoByCostCenter(int registrationId, int costCenterId, int serviceId)
        {
            List<BillInfoViewBO> viewBO = new List<BillInfoViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("LoadBillInfoForBillAdjustment_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceId", DbType.Int32, serviceId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BillInfo");
                    DataTable Table = ds.Tables["BillInfo"];
                    viewBO = Table.AsEnumerable().Select(r => new BillInfoViewBO
                    {
                        TransactionId = r.Field<int>("TransactionId"),
                        TransactionName = r.Field<string>("TransactionName")

                    }).ToList();
                }
            }

            return viewBO;
        }
        public BillInfoViewBO LoadBillDetailInfoByBillIdNType(int costCenterId, int billId)
        {
            BillInfoViewBO viewBO = new BillInfoViewBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("LoadBillDetailInfoByBillIdNType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                viewBO.TransactionId = Convert.ToInt32(reader["TransactionId"]);
                                viewBO.ServiceDate = reader["ServiceDate"].ToString();
                                viewBO.TotalSales = Convert.ToDecimal(reader["TotalSales"]);
                                viewBO.BillNumber = reader["BillNumber"].ToString();
                            }
                        }
                    }
                }
            }
            return viewBO;

            //BillInfoViewBO viewBO = new BillInfoViewBO();

            //using (DbConnection conn = dbSmartAspects.CreateConnection())
            //{
            //    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("LoadBillDetailInfoByBillIdNType_SP"))
            //    {
            //        dbSmartAspects.AddInParameter(cmd, "@ServiceType", DbType.String, strServiceType);
            //        dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);

            //        DataSet ds = new DataSet();
            //        dbSmartAspects.LoadDataSet(cmd, ds, "BillInfo");
            //        DataTable Table = ds.Tables["BillInfo"];
            //        viewBO = Table.AsEnumerable().Select(r => new BillInfoViewBO
            //        {
            //            TransactionId = r.Field<int>("TransactionId"),
            //            ServiceDate = r.Field<string>("ServiceDate"),
            //            TotalSales = r.Field<decimal>("TotalSales"),
            //            BillNumber = r.Field<string>("BillNumber")
            //        }).ToList();
            //    }
            //}

            //return viewBO;
        }
        public Boolean SaveGHServiceBillInfoForBillAdjustment(int billId, int CostCenterId, decimal adjustmentAmount, string strRemarks, int modifiedBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGHServiceBillInfoForBillAdjustment_SP"))
                    {
                        command.Parameters.Clear();

                        dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, billId);
                        dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, CostCenterId);
                        dbSmartAspects.AddInParameter(command, "@ModifiedBy", DbType.Int32, modifiedBy);
                        dbSmartAspects.AddInParameter(command, "@AdjustmentAmount", DbType.Decimal, adjustmentAmount);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, strRemarks);

                        status = dbSmartAspects.ExecuteNonQuery(command, transaction) > 0 ? true : false;
                    }

                    if (status)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
            }

            return status;
        }
    }
}
