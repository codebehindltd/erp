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
    public class GuestBillPaymentDA : BaseService
    {
        public List<GuestBillPaymentBO> GetGuestBillPaymentInfo_SP()
        {
            List<GuestBillPaymentBO> bankList = new List<GuestBillPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillPaymentInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestBillPaymentBO billPayment = new GuestBillPaymentBO();

                                billPayment.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                billPayment.PaymentType = reader["PaymentType"].ToString();
                                billPayment.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                billPayment.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                billPayment.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"].ToString());
                                billPayment.Remarks = reader["Remarks"].ToString();

                                bankList.Add(billPayment);
                            }
                        }
                    }
                }
            }
            return bankList;
        }
        public Boolean SaveGuestBillPaymentInfo(GuestBillPaymentBO guestBillPaymentBO, out int tmpPaymentId, string paymentFrom)
        {
            Boolean status = false;
            tmpPaymentId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                try
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfo_SP"))
                    {
                        command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                        dbSmartAspects.AddInParameter(command, "@PaymentFrom", DbType.String, paymentFrom);
                        dbSmartAspects.AddInParameter(command, "@ModuleName", DbType.String, guestBillPaymentBO.ModuleName);
                        dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                        dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);

                        if (guestBillPaymentBO.ServiceBillId != null)
                            dbSmartAspects.AddInParameter(command, "@ServiceBillId", DbType.Int32, guestBillPaymentBO.ServiceBillId);
                        else
                            dbSmartAspects.AddInParameter(command, "@ServiceBillId", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                        dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                        dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                        dbSmartAspects.AddInParameter(command, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                        dbSmartAspects.AddInParameter(command, "@PaymentModeId", DbType.Int32, guestBillPaymentBO.PaymentModeId);
                        dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                        dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                        dbSmartAspects.AddInParameter(command, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                        dbSmartAspects.AddInParameter(command, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                        dbSmartAspects.AddInParameter(command, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                        dbSmartAspects.AddInParameter(command, "@RefundAccountHead", DbType.Int32, guestBillPaymentBO.RefundAccountHead);

                        if (guestBillPaymentBO.CardType != null)
                            dbSmartAspects.AddInParameter(command, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                        else
                            dbSmartAspects.AddInParameter(command, "@CardType", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, guestBillPaymentBO.Remarks);
                        dbSmartAspects.AddInParameter(command, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                        if (guestBillPaymentBO.PaymentMode == "Card")
                        {
                            dbSmartAspects.AddInParameter(command, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                        }
                        if (guestBillPaymentBO.CardHolderName != null)
                            dbSmartAspects.AddInParameter(command, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                        else
                            dbSmartAspects.AddInParameter(command, "@CardHolderName", DbType.String, DBNull.Value);
                        dbSmartAspects.AddInParameter(command, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);

                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@PaymentId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return status;
        }
        public Boolean SaveBanquetGuestBillPaymentInfo(GuestBillPaymentBO guestBillPaymentBO, string paymentFrom, int paymentModeId, out int tmpPaymentId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfo_SP"))
                    {
                        command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                        dbSmartAspects.AddInParameter(command, "@PaymentFrom", DbType.String, paymentFrom);
                        dbSmartAspects.AddInParameter(command, "@ModuleName", DbType.String, "Banquet");
                        dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                        dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);

                        if (guestBillPaymentBO.ServiceBillId != null)
                            dbSmartAspects.AddInParameter(command, "@ServiceBillId", DbType.Int32, guestBillPaymentBO.ServiceBillId);
                        else
                            dbSmartAspects.AddInParameter(command, "@ServiceBillId", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                        dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                        dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                        dbSmartAspects.AddInParameter(command, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                        dbSmartAspects.AddInParameter(command, "@PaymentModeId", DbType.Int32, paymentModeId);
                        dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                        dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                        dbSmartAspects.AddInParameter(command, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                        dbSmartAspects.AddInParameter(command, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                        dbSmartAspects.AddInParameter(command, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                        dbSmartAspects.AddInParameter(command, "@RefundAccountHead", DbType.Int32, guestBillPaymentBO.RefundAccountHead);
                        dbSmartAspects.AddInParameter(command, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                        dbSmartAspects.AddInParameter(command, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, guestBillPaymentBO.Remarks);
                        dbSmartAspects.AddInParameter(command, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                        if (guestBillPaymentBO.PaymentMode == "Card")
                        {
                            dbSmartAspects.AddInParameter(command, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                        }

                        dbSmartAspects.AddInParameter(command, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                        dbSmartAspects.AddInParameter(command, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@PaymentId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean UpdateGuestBillPaymentInfo(GuestBillPaymentBO guestBillPaymentBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGuestBillPaymentInfo_SP"))
                        {
                            command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                            dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int32, guestBillPaymentBO.PaymentId);
                            dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                            dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);

                            if (guestBillPaymentBO.ServiceBillId != null)
                                dbSmartAspects.AddInParameter(command, "@ServiceBillId", DbType.Int32, guestBillPaymentBO.ServiceBillId);
                            else
                                dbSmartAspects.AddInParameter(command, "@ServiceBillId", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                            dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                            dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);

                            dbSmartAspects.AddInParameter(command, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                            dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                            dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                            dbSmartAspects.AddInParameter(command, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                            dbSmartAspects.AddInParameter(command, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                            dbSmartAspects.AddInParameter(command, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                            dbSmartAspects.AddInParameter(command, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                            dbSmartAspects.AddInParameter(command, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, guestBillPaymentBO.Remarks);
                            dbSmartAspects.AddInParameter(command, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                            if (guestBillPaymentBO.PaymentMode == "Card")
                            {
                                dbSmartAspects.AddInParameter(command, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                            }

                            dbSmartAspects.AddInParameter(command, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                            dbSmartAspects.AddInParameter(command, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, guestBillPaymentBO.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public bool UpdateGuestBillPaymentInfoForLink(List<GuestBillPaymentBO> guestBillPaymentBO, int regId)
        {
            bool status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        try
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGuestBillPaymentInfoForLink"))
                            {
                                if (guestBillPaymentBO.Count>0)
                                {
                                    foreach (var item in guestBillPaymentBO)
                                    {
                                        command.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int32, item.PaymentId);
                                        dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, regId);

                                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                    }
                                }  

                            }
                            if (status == true)
                            {
                                transction.Commit();
                            }
                            else
                            {
                                transction.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            transction.Rollback();
                            throw ex;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return status;
        }
        public GuestBillPaymentBO GetGuestBillPaymentInfoById(int paymentId)
        {
            GuestBillPaymentBO billPayment = new GuestBillPaymentBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillPaymentInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int32, paymentId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                billPayment.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                billPayment.PaymentType = reader["PaymentType"].ToString();

                                if (reader["ServiceBillId"] != null)
                                    billPayment.ServiceBillId = Convert.ToInt32(reader["ServiceBillId"]);

                                billPayment.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                billPayment.PaymentMode = reader["PaymentMode"].ToString();
                                billPayment.AccountsPostingHeadId = Convert.ToInt32(reader["AccountsPostingHeadId"]);
                                billPayment.BankId = Convert.ToInt32(reader["BankId"]);
                                billPayment.BranchName = reader["BranchName"].ToString();
                                billPayment.PaymentMode = reader["PaymentMode"].ToString();
                                billPayment.ChecqueNumber = reader["ChecqueNumber"].ToString();
                                billPayment.ChecqueDate = Convert.ToDateTime(reader["ChecqueDate"].ToString());

                                billPayment.CardNumber = reader["CardNumber"].ToString();
                                billPayment.CardType = reader["CardType"].ToString();
                                if (reader["ExpireDate"] != null)
                                {
                                    billPayment.ExpireDate = Convert.ToDateTime(reader["ExpireDate"].ToString());
                                }
                                billPayment.CardHolderName = reader["CardHolderName"].ToString();
                                billPayment.CardReference = reader["CardReference"].ToString();

                                billPayment.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                billPayment.FieldId = Convert.ToInt32(reader["FieldId"]);
                                billPayment.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"].ToString());
                                billPayment.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"].ToString());
                                billPayment.DealId = Convert.ToInt32(reader["DealId"]);
                                billPayment.Remarks = reader["Remarks"].ToString();
                            }
                        }
                    }
                }
            }
            return billPayment;
        }
        public List<GuestBillPaymentBO> GetGuestBillPaymentInfoByRegistrationIdForLink(int registrationId)
        {
            List<GuestBillPaymentBO> billPaymentList = new List<GuestBillPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillPaymentInfoByRegistrationIdForLink_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestBillPaymentBO billPayment = new GuestBillPaymentBO();
                                billPayment.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                billPayment.PaymentType = reader["PaymentType"].ToString();

                                if (reader["ServiceBillId"] != null)
                                    billPayment.ServiceBillId = Convert.ToInt32(reader["ServiceBillId"]);

                                billPayment.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                billPayment.PaymentMode = reader["PaymentMode"].ToString();
                                billPayment.AccountsPostingHeadId = Convert.ToInt32(reader["AccountsPostingHeadId"]);
                                billPayment.BankId = Convert.ToInt32(reader["BankId"]);
                                billPayment.BranchName = reader["BranchName"].ToString();
                                billPayment.PaymentMode = reader["PaymentMode"].ToString();
                                billPayment.ChecqueNumber = reader["ChecqueNumber"].ToString();
                                billPayment.ChecqueDate = Convert.ToDateTime(reader["ChecqueDate"].ToString());

                                billPayment.CardNumber = reader["CardNumber"].ToString();
                                billPayment.CardType = reader["CardType"].ToString();
                                if (reader["ExpireDate"] != null)
                                {
                                    billPayment.ExpireDate = Convert.ToDateTime(reader["ExpireDate"].ToString());
                                }
                                billPayment.CardHolderName = reader["CardHolderName"].ToString();
                                billPayment.CardReference = reader["CardReference"].ToString();

                                billPayment.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                billPayment.FieldId = Convert.ToInt32(reader["FieldId"]);
                                billPayment.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"].ToString());
                                billPayment.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"].ToString());
                                billPayment.DealId = Convert.ToInt32(reader["DealId"]);
                                billPayment.Remarks = reader["Remarks"].ToString();

                                billPaymentList.Add(billPayment);
                            }
                        }
                    }
                }
            }
            return billPaymentList;
        }
        public List<GuestBillPaymentBO> GetGuestBillPaymentInfoByRegistrationId(string moduleName, int registrationId)
        {
            List<GuestBillPaymentBO> billPaymentList = new List<GuestBillPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillPaymentInfoByRegistrationId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ModuleName", DbType.String, moduleName);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestBillPaymentBO billPayment = new GuestBillPaymentBO();

                                billPayment.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                billPayment.ModuleName = reader["ModuleName"].ToString();
                                billPayment.BillNumber = reader["BillNumber"].ToString();
                                billPayment.PaymentType = reader["PaymentType"].ToString();
                                billPayment.PaymentMode = reader["PaymentMode"].ToString();
                                billPayment.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                billPayment.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                billPayment.CurrencyType = reader["CurrencyType"].ToString();
                                billPayment.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"].ToString());
                                billPayment.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"].ToString());
                                billPayment.PaymentDescription = reader["PaymentDescription"].ToString();
                                billPayment.CreatedByName = reader["CreatedByName"].ToString();
                                billPayment.IsBillEditable = Convert.ToBoolean(reader["IsBillEditable"]);
                                billPaymentList.Add(billPayment);
                            }
                        }
                    }
                }
            }
            return billPaymentList;
        }
        public List<GuestBillPaymentBO> GetGuestBillPaymentInformationByServiceBillId(string moduleName, int serviceBillId)
        {
            List<GuestBillPaymentBO> billPaymentList = new List<GuestBillPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillPaymentInformationByServiceBillId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ModuleName", DbType.String, moduleName);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, serviceBillId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestBillPaymentBO billPayment = new GuestBillPaymentBO();

                                billPayment.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                billPayment.BillNumber = reader["BillNumber"].ToString();
                                billPayment.PaymentMode = reader["PaymentMode"].ToString();
                                billPayment.PaymentType = reader["PaymentType"].ToString();
                                billPayment.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                billPayment.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                billPayment.CurrencyType = reader["CurrencyType"].ToString();
                                billPayment.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"].ToString());
                                billPayment.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"].ToString());
                                billPayment.CreatedByName = reader["UserName"].ToString();
                                billPayment.IsBillSettlement = Convert.ToBoolean(reader["IsBillSettlement"]);
                                billPaymentList.Add(billPayment);
                            }
                        }
                    }
                }
            }
            return billPaymentList;
        }
        public PaymentSummaryBO GetGuestBillPaymentSummaryInfoByRegiId(int registrationId)
        {
            PaymentSummaryBO paymentSummaryBO = new PaymentSummaryBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillPaymentSummaryInfoByRegiId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                paymentSummaryBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                paymentSummaryBO.TotalPayment = Convert.ToDecimal(reader["TotalPayment"]);
                            }
                        }
                    }
                }
            }
            return paymentSummaryBO;
        }
        public List<PaymentSummaryBO> GetGuestBillPaymentSummaryInfoByRegiIdList(string registrationIdList, int isToDaysPayment)
        {
            List<PaymentSummaryBO> paymentSummaryBOList = new List<PaymentSummaryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillPaymentSummaryInfoByRegiIdList_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationIdList);
                    dbSmartAspects.AddInParameter(cmd, "@IsToDaysPayment", DbType.Int32, isToDaysPayment);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PaymentSummaryBO paymentSummaryBO = new PaymentSummaryBO();
                                //paymentSummaryBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);

                                paymentSummaryBO.DebitBalance = Convert.ToDecimal(reader["DebitBalance"]);
                                paymentSummaryBO.CreditBalance = Convert.ToDecimal(reader["CreditBalance"]);
                                paymentSummaryBO.CurrencyExchangeRate = Convert.ToDecimal(reader["CurrencyExchangeRate"]);
                                //paymentSummaryBO.TotalPayment = Convert.ToDecimal(reader["TotalPayment"]);

                                paymentSummaryBOList.Add(paymentSummaryBO);
                            }
                        }
                    }
                }
            }
            return paymentSummaryBOList;
        }
        public List<GuestBillPaymentBO> GetGuestBillPaymentInfoByRegistrationIdList(string registrationIdList)
        {
            List<GuestBillPaymentBO> billPaymentList = new List<GuestBillPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillPaymentInfoByRegiIdList_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationIdList);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestBillPaymentBO billPayment = new GuestBillPaymentBO();

                                billPayment.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                billPayment.PaymentType = reader["PaymentType"].ToString();
                                billPayment.PaymentMode = reader["PaymentMode"].ToString();
                                billPayment.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                billPayment.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                billPayment.BillPaidBy = Convert.ToInt32(reader["BillPaidBy"]);
                                billPayment.BillPaidByRoomNumber = Convert.ToInt32(reader["BillPaidByRoomNumber"]);
                                billPayment.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                billPayment.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"].ToString());

                                billPaymentList.Add(billPayment);
                            }
                        }
                    }
                }
            }
            return billPaymentList;
        }
        public List<GuestBillPaymentBO> GetGuestAllBillPaymentInfoByRegistrationIdList(string registrationIdList)
        {
            List<GuestBillPaymentBO> billPaymentList = new List<GuestBillPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestAllBillPaymentInfoByRegiIdList_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationIdList);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestBillPaymentBO billPayment = new GuestBillPaymentBO();

                                billPayment.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                billPayment.ModuleName = reader["ModuleName"].ToString();
                                billPayment.PaymentType = reader["PaymentType"].ToString();
                                billPayment.PaymentMode = reader["PaymentMode"].ToString();
                                billPayment.RoomNumber = Convert.ToInt32(reader["RoomNumber"]);
                                billPayment.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                billPayment.BillPaidBy = Convert.ToInt32(reader["BillPaidBy"]);
                                billPayment.BillPaidByRoomNumber = Convert.ToInt32(reader["BillPaidByRoomNumber"]);
                                billPayment.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                billPayment.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"].ToString());
                                if (!string.IsNullOrWhiteSpace(reader["CheckOutDate"].ToString()))
                                {
                                    billPayment.CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"]);
                                }
                                else
                                {
                                    billPayment.CheckOutDate = null;
                                }

                                billPaymentList.Add(billPayment);
                            }
                        }
                    }
                }
            }
            return billPaymentList;
        }
        public List<GuestBillPaymentBO> GetGuestRebateInformationByRegistrationIdList(string registrationIdList)
        {
            List<GuestBillPaymentBO> billPaymentList = new List<GuestBillPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestRebateInformationByRegistrationIdList_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationIdList);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestBillPaymentBO billPayment = new GuestBillPaymentBO();

                                billPayment.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                billPayment.ModuleName = reader["ModuleName"].ToString();
                                billPayment.PaymentType = reader["PaymentType"].ToString();
                                billPayment.PaymentMode = reader["PaymentMode"].ToString();
                                billPayment.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                billPayment.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                billPayment.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"].ToString());

                                billPaymentList.Add(billPayment);
                            }
                        }
                    }
                }
            }
            return billPaymentList;
        }
        public int GetCountHotelGuestBillApprovedItem(DateTime Date, string dbTable)
        {
            int count = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCountHotelGuestRoomOrServiceBillApproved_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ApproveDate", DbType.DateTime, Date);
                    dbSmartAspects.AddInParameter(cmd, "@TableName", DbType.String, dbTable);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                count = Convert.ToInt32(reader["NumberOfApprovedItem"]);
                            }
                        }
                    }
                }
            }
            return count;
        }
        public decimal GetGuestBillPaymentSummaryInfoByPaymentType(string RegistrationIdList, int PaymentId)
        {
            decimal CalculatedAmount = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillPaymentSummaryInfoByPaymentType_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, RegistrationIdList);
                    dbSmartAspects.AddInParameter(cmd, "@PaymentType", DbType.Int32, PaymentId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CalculatedAmount = Convert.ToDecimal(reader["CalculatedAmount"]);
                            }
                        }
                    }
                }
            }
            return CalculatedAmount;
        }
        public List<GenerateGuestBillReportBO> GetGenerateGuestBill(string registrationId, string IsBillSplited, string GuestBillFromDate, string GuestBillToDate, string PrintedBy)
        {
            List<GenerateGuestBillReportBO> guestBill = new List<GenerateGuestBillReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GenerateGuestBill"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationId);
                    dbSmartAspects.AddInParameter(cmd, "@IsBillSplited", DbType.String, IsBillSplited);
                    dbSmartAspects.AddInParameter(cmd, "@GuestBillFromDate", DbType.String, GuestBillFromDate);
                    dbSmartAspects.AddInParameter(cmd, "@GuestBillToDate", DbType.String, GuestBillToDate);
                    dbSmartAspects.AddInParameter(cmd, "@PrintedBy", DbType.String, PrintedBy);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestBill");
                    DataTable Table = ds.Tables["GuestBill"];

                    guestBill = Table.AsEnumerable().Select(r => new GenerateGuestBillReportBO
                    {
                        RegistrationId = r.Field<Int32>("RegistrationId"),
                        RegistrationNumber = r.Field<string>("RegistrationNumber"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        BillNumber = r.Field<string>("BillNumber"),
                        GuestName = r.Field<string>("GuestName"),
                        TotalPerson = r.Field<int?>("TotalPerson"),
                        GuestAddress = r.Field<string>("GuestAddress"),
                        ArriveDate = r.Field<string>("ArriveDate"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        RoomType = r.Field<string>("RoomType"),
                        CurrencyHead = r.Field<string>("CurrencyHead"),
                        IsDiscountApplicableOnRackRate = r.Field<Int32>("IsDiscountApplicableOnRackRate"),
                        UnitPrice = r.Field<decimal?>("UnitPrice"),
                        RoomRate = r.Field<decimal?>("RoomRate"),
                        ExpectedCheckOutDate = r.Field<string>("ExpectedCheckOutDate"),
                        CheckOutDate = r.Field<string>("CheckOutDate"),
                        IsBillSplited = r.Field<int?>("IsBillSplited"),
                        PrintDate = r.Field<string>("PrintDate"),
                        PrintedBy = r.Field<string>("PrintedBy")

                    }).ToList();
                }
            }

            return guestBill;
        }
        public List<GuestBillPaymentBO> GetGuestBillPaymentInfoByServiceId(string moduleName, int serviceBillId)
        {
            List<GuestBillPaymentBO> billPaymentList = new List<GuestBillPaymentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillPaymentInfoByServiceBillId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ModuleName", DbType.String, moduleName);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, serviceBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestBill");
                    DataTable Table = ds.Tables["GuestBill"];

                    billPaymentList = Table.AsEnumerable().Select(r => new GuestBillPaymentBO
                    {
                        PaymentId = r.Field<Int32>("PaymentId"),
                        BillNumber = r.Field<string>("BillNumber"),
                        PaymentType = r.Field<string>("PaymentType"),
                        RegistrationId = r.Field<int>("RegistrationId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        CurrencyType = r.Field<string>("CurrencyType"),
                        PaymentAmount = r.Field<decimal>("PaymentAmount"),
                        ServiceBillId = r.Field<int?>("ServiceBillId"),
                        PaymentMode = r.Field<string>("PaymentMode"),
                        BankId = r.Field<int>("BankId"),
                        BranchName = r.Field<string>("BranchName"),
                        ChecqueNumber = r.Field<string>("ChecqueNumber"),
                        //ChecqueDate = r.Field<DateTime>("ChecqueDate"),
                        CardType = r.Field<string>("CardType"),
                        CardNumber = r.Field<string>("CardNumber"),
                        //ExpireDate = r.Field<DateTime?>("ExpireDate"),
                        CardHolderName = r.Field<string>("CardHolderName"),
                        CardReference = r.Field<string>("CardReference"),
                        PaymentDescription = r.Field<string>("PaymentDescription"),
                        TransactionType = r.Field<string>("TransactionType")

                    }).ToList();
                }
            }
            return billPaymentList;
        }
        public List<GuestBillPaymentBO> GetGuestBillPaymentInfoForNoShowChargeByReservationId(int reservationId)
        {
            List<GuestBillPaymentBO> billPaymentList = new List<GuestBillPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBillPaymentInfoForNoShowChargeByReservationId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestBillPaymentBO billPayment = new GuestBillPaymentBO();

                                billPayment.PaymentId = Convert.ToInt32(reader["PaymentId"]);
                                billPayment.BillNumber = reader["BillNumber"].ToString();
                                billPayment.PaymentType = reader["PaymentType"].ToString();
                                billPayment.PaymentMode = reader["PaymentMode"].ToString();
                                billPayment.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                billPayment.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);
                                billPayment.CurrencyType = reader["CurrencyType"].ToString();
                                billPayment.CurrencyAmount = Convert.ToDecimal(reader["CurrencyAmount"].ToString());
                                billPayment.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"].ToString());
                                billPayment.PaymentDescription = reader["PaymentDescription"].ToString();
                                billPayment.CreatedByName = reader["CreatedByName"].ToString();
                                billPayment.IsBillEditable = Convert.ToBoolean(reader["IsBillEditable"]);
                                billPaymentList.Add(billPayment);
                            }
                        }
                    }
                }
            }
            return billPaymentList;
        }
        public Boolean TransferGuestBillPaymentInfo(List<GuestBillPaymentBO> serviceBillBOList, int fromRegId, int transferRegistrationId, int userInfoId)
        {
            Boolean status = false;
            int transactionCount = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("TransferGuestBillPaymentInfo_SP"))
                    {
                        foreach (GuestBillPaymentBO serviceBill in serviceBillBOList)
                        {
                            cmd.Parameters.Clear();

                            dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int32, serviceBill.PaymentId);
                            dbSmartAspects.AddInParameter(cmd, "@PaymentAmount", DbType.Decimal, serviceBill.PaymentAmount);
                            dbSmartAspects.AddInParameter(cmd, "@Remarks", DbType.String, serviceBill.Remarks);
                            dbSmartAspects.AddInParameter(cmd, "@FromRegId", DbType.Int32, fromRegId);
                            dbSmartAspects.AddInParameter(cmd, "@TransferRegistrationId", DbType.Int32, transferRegistrationId);
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
    }
}
